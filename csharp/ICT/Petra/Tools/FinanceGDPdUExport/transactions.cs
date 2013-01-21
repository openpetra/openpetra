//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportTransactions
    {
        /// <summary>
        /// return the transaction rows that are part of the opposite part of the transaction, ie. debit or credit
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AOtherTransactions">other transactions in the same journal</param>
        /// <returns></returns>
        private static ATransactionRow[] GetOtherTransactions(ATransactionRow ARow, DataRowView[] AOtherTransactions)
        {
            SortedList <int, ATransactionRow>SortedByTransactionNumber = new SortedList <int, ATransactionRow>();

            foreach (DataRowView rv in AOtherTransactions)
            {
                ATransactionRow r = (ATransactionRow)rv.Row;
                SortedByTransactionNumber.Add(r.TransactionNumber, r);
            }

            List <ATransactionRow>BalancingTransactions = new List <ATransactionRow>();
            decimal CurrentBalance = 0.0m;
            bool RelevantSetOfTransactions = false;

            foreach (ATransactionRow r in SortedByTransactionNumber.Values)
            {
                CurrentBalance += r.TransactionAmount * (r.DebitCreditIndicator ? -1 : 1);

                if (r.TransactionNumber == ARow.TransactionNumber)
                {
                    RelevantSetOfTransactions = true;
                }
                else if (r.DebitCreditIndicator != ARow.DebitCreditIndicator)
                {
                    BalancingTransactions.Add(r);
                }

                if (CurrentBalance == 0.0m)
                {
                    if (RelevantSetOfTransactions)
                    {
                        return BalancingTransactions.ToArray();
                    }

                    BalancingTransactions.Clear();
                }
            }

            return null;
        }

        /// <summary>
        /// export all GL Transactions in the given year, towards the specified cost centres
        /// </summary>
        public static void ExportGLTransactions(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            Int32 AFinancialYear,
            string ACostCentres,
            string AIgnoreAccounts,
            string AIgnoreReferences,
            SortedList <string, string>ATaxAnalysisAttributes,
            ref List <string>ACostCentresInvolved,
            ref List <string>AAccountsInvolved)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "transaction.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            string sql =
                String.Format("SELECT T.*, B.{4} AS a_transaction_date_d " +
                    "FROM PUB_{8} AS B, PUB_{7} AS T " +
                    "WHERE B.{9} = {10} AND B.{15} = {16} AND B.{11}='{12}' " +
                    "AND T.{9} = B.{9} AND T.{0} = B.{0} " +
                    "AND T.{13} IN ({14}) " +
                    "AND NOT T.{17} IN ({19}) " +
                    "AND NOT T.{20} IN ({21}) " +
                    "ORDER BY {0}, {1}, {2}",
                    ATransactionTable.GetBatchNumberDBName(),
                    ATransactionTable.GetJournalNumberDBName(),
                    ATransactionTable.GetTransactionNumberDBName(),
                    ATransactionTable.GetTransactionAmountDBName(),
                    ABatchTable.GetDateEffectiveDBName(),
                    ATransactionTable.GetNarrativeDBName(),
                    ATransactionTable.GetReferenceDBName(),
                    ATransactionTable.GetTableDBName(),
                    ABatchTable.GetTableDBName(),
                    ATransactionTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_POSTED,
                    ATransactionTable.GetCostCentreCodeDBName(),
                    "'" + ACostCentres.Replace(",", "','") + "'",
                    ABatchTable.GetBatchYearDBName(),
                    AFinancialYear,
                    ATransactionTable.GetAccountCodeDBName(),
                    ATransactionTable.GetDebitCreditIndicatorDBName(),
                    "'" + AIgnoreAccounts.Replace(",", "','") + "'",
                    ATransactionTable.GetReferenceDBName(),
                    "'" + AIgnoreReferences.Replace(",", "','") + "'");

            ATransactionTable transactions = new ATransactionTable();
            transactions = (ATransactionTable)DBAccess.GDBAccessObj.SelectDT(transactions, sql, Transaction, null, 0, 0);

            // get the analysis attributes
            sql =
                String.Format("SELECT A.* from PUB_{1} AS B, PUB_{13} AS T, PUB_{0} AS A " +
                    "WHERE B.{2} = {3} AND B.{4} = {5} AND B.{6}='{7}' " +
                    "AND T.{2} = B.{2} AND T.{8} = B.{8} " +
                    "AND T.{9} IN ({10}) " +
                    "AND A.{2} = T.{2} AND A.{8} = T.{8} AND A.{11} = T.{11} AND A.{12} = T.{12}",
                    ATransAnalAttribTable.GetTableDBName(),
                    ABatchTable.GetTableDBName(),
                    ABatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    ABatchTable.GetBatchYearDBName(),
                    AFinancialYear,
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_POSTED,
                    ATransactionTable.GetBatchNumberDBName(),
                    ATransactionTable.GetCostCentreCodeDBName(),
                    "'" + ACostCentres.Replace(",", "','") + "'",
                    ATransactionTable.GetJournalNumberDBName(),
                    ATransactionTable.GetTransactionNumberDBName(),
                    ATransactionTable.GetTableDBName(),
                    ABatchTable.GetBatchYearDBName());

            ATransAnalAttribTable TransAnalAttrib = new ATransAnalAttribTable();
            DBAccess.GDBAccessObj.SelectDT(TransAnalAttrib, sql, Transaction, null, 0, 0);

            TransAnalAttrib.DefaultView.Sort =
                ATransAnalAttribTable.GetBatchNumberDBName() + "," +
                ATransAnalAttribTable.GetJournalNumberDBName() + "," +
                ATransAnalAttribTable.GetTransactionNumberDBName();

            // get a list of all batches involved
            List <Int64>batches = new List <Int64>();
            StringBuilder batchnumbers = new StringBuilder();

            foreach (ATransactionRow r in transactions.Rows)
            {
                if (!batches.Contains(r.BatchNumber))
                {
                    batches.Add(r.BatchNumber);
                    batchnumbers.Append(r.BatchNumber.ToString() + ",");
                }
            }

            // get the other transactions in the same journal for finding the opposite cc/acc involved
            // for performance reasons, get all transactions of the whole batch
            sql =
                String.Format("SELECT DISTINCT TJ.* " +
                    "FROM PUB_{0} AS TJ " +
                    "WHERE TJ.{1} = {2} AND TJ.{3} IN ({4})",
                    ATransactionTable.GetTableDBName(),
                    ATransactionTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    ATransactionTable.GetBatchNumberDBName(),
                    batchnumbers.ToString() + "-1");

            ATransactionTable allTransactionsInJournal = new ATransactionTable();
            allTransactionsInJournal = (ATransactionTable)DBAccess.GDBAccessObj.SelectDT(allTransactionsInJournal, sql, Transaction, null, 0, 0);

            allTransactionsInJournal.DefaultView.Sort =
                ATransactionTable.GetBatchNumberDBName() + "," +
                ATransactionTable.GetJournalNumberDBName();

            // get all names of gift batches
            sql =
                String.Format("SELECT * FROM PUB_{0} " +
                    "WHERE {1} = {2} " +
                    "AND {3} = {4}",
                    AGiftBatchTable.GetTableDBName(),
                    AGiftBatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AGiftBatchTable.GetBatchYearDBName(),
                    AFinancialYear);

            AGiftBatchTable giftbatches = new AGiftBatchTable();
            DBAccess.GDBAccessObj.SelectDT(giftbatches, sql, Transaction, null, 0, 0);
            giftbatches.DefaultView.Sort = AGiftBatchTable.GetBatchNumberDBName();


            sql =
                String.Format("SELECT * FROM PUB_{0} " +
                    "WHERE {1} = {2}",
                    AAccountTable.GetTableDBName(),
                    AAccountTable.GetLedgerNumberDBName(),
                    ALedgerNumber);

            AAccountTable accounts = new AAccountTable();
            DBAccess.GDBAccessObj.SelectDT(accounts, sql, Transaction, null, 0, 0);
            accounts.DefaultView.Sort = AAccountTable.GetAccountCodeDBName();

            DBAccess.GDBAccessObj.RollbackTransaction();
            int rowCounter = 0;

            StringCollection costcentreCollection = StringHelper.StrSplit(ACostCentres, ",");

            foreach (ATransactionRow row in transactions.Rows)
            {
                if (row.DebitCreditIndicator)
                {
                    row.TransactionAmount *= -1.0m;
                }

                StringBuilder attributes = new StringBuilder();

                DataRowView[] attribs = TransAnalAttrib.DefaultView.FindRows(new object[] { row.BatchNumber, row.JournalNumber, row.TransactionNumber });

                decimal TaxOnIncome = 0.0m;
                decimal TaxOnExpense = 0.0m;

                // only mention tax codes, if this transaction was against a costcentre that has to pay taxes
                if (costcentreCollection.Contains(row.CostCentreCode))
                {
                    foreach (DataRowView rv in attribs)
                    {
                        ATransAnalAttribRow attribRow = (ATransAnalAttribRow)rv.Row;

                        // also export attribRow.AnalysisTypeCode?
                        attributes.Append(attribRow.AnalysisAttributeValue);

                        if (attribRow.AnalysisAttributeValue == "v19")
                        {
                            TaxOnExpense = row.TransactionAmount * 0.19m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "v7")
                        {
                            TaxOnExpense = row.TransactionAmount * 0.07m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "70v7")
                        {
                            TaxOnExpense = row.TransactionAmount * 0.7m * 0.07m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "70v19")
                        {
                            TaxOnExpense = row.TransactionAmount * 0.7m * 0.19m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "m19")
                        {
                            TaxOnIncome = row.TransactionAmount * 0.19m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "m7")
                        {
                            TaxOnIncome = row.TransactionAmount * 0.07m;
                        }
                    }
                }

                DataRowView[] RelatedTransactions = allTransactionsInJournal.DefaultView.FindRows(new object[] { row.BatchNumber, row.JournalNumber });

                ATransactionRow[] OtherTransactions = GetOtherTransactions(row, RelatedTransactions);

                string OtherCostCentres = string.Empty;
                string OtherAccountCodes = string.Empty;

                if (OtherTransactions.Length < 30)
                {
                    foreach (ATransactionRow r in OtherTransactions)
                    {
                        OtherCostCentres = StringHelper.AddCSV(OtherCostCentres, r.CostCentreCode);
                        OtherAccountCodes = StringHelper.AddCSV(OtherAccountCodes, r.AccountCode);
                    }
                }

                if (!ACostCentresInvolved.Contains(row.CostCentreCode))
                {
                    ACostCentresInvolved.Add(row.CostCentreCode);
                }

                if (!AAccountsInvolved.Contains(row.AccountCode))
                {
                    AAccountsInvolved.Add(row.AccountCode);
                }

                // we are using gift batch for receiving payments
                string Narrative = row.Narrative;

                if (Narrative.StartsWith("GB - Gift Batch ") && row.Reference.StartsWith("GB"))
                {
                    // find the account and set the account description into the narrative
                    try
                    {
                        DataRowView[] acc = accounts.DefaultView.FindRows(row.AccountCode);
                        Narrative = ((AAccountRow)acc[0].Row).AccountCodeLongDesc;
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DataRowView[] gb = giftbatches.DefaultView.FindRows(Convert.ToInt32(row.Reference.Substring(2)));
                        Narrative += " " + ((AGiftBatchRow)gb[0].Row).BatchDescription;
                    }
                    catch (Exception)
                    {
                    }
                }

                sb.Append(StringHelper.StrMerge(
                        new string[] {
                            "B" + row.BatchNumber.ToString() + "_J" + row.JournalNumber.ToString() + "_T" + row.TransactionNumber.ToString(),
                            row.CostCentreCode,
                            row.AccountCode,
                            row.TransactionDate.ToString("yyyyMMdd"),
                            OtherCostCentres,
                            OtherAccountCodes,
                            Narrative,
                            row.Reference,
                            String.Format("{0:N}", row.TransactionAmount),
                            attributes.ToString(),
                            TaxOnIncome.ToString(),
                            TaxOnExpense.ToString()
                        }, ACSVSeparator));

                sb.Append(ANewLine);

                rowCounter++;

                if (rowCounter % 500 == 0)
                {
                    TLogging.Log("Processing transactions " + rowCounter.ToString());
                }
            }

            TLogging.Log("Processing transactions " + rowCounter.ToString());

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }

        /// <summary>
        /// get the tax keys and their description used for references in transactions
        /// </summary>
        /// <returns></returns>
        public static SortedList <string, string>GetTaxAnalysisAttributes()
        {
            SortedList <string, string>TaxAnalysisAttributes = new SortedList <string, string>();
            TaxAnalysisAttributes.Add("70v19", "70 % von 19 % Vorsteuer");
            TaxAnalysisAttributes.Add("70v7", "70 % von 7 % Vorsteuer");
            TaxAnalysisAttributes.Add("m0", "Umsatzsteuer 0 %");
            TaxAnalysisAttributes.Add("m7", "Umsatzsteuer 7 %");
            TaxAnalysisAttributes.Add("m19", "Umsatzsteuer 19 %");
            TaxAnalysisAttributes.Add("mi0", "Umsatzsteuer Innenumsatz");
            TaxAnalysisAttributes.Add("meu", "Umsatzsteuer Innergemeinschaftliche Lieferung");
            TaxAnalysisAttributes.Add("md0", "Umsatzsteuer Drittland");
            TaxAnalysisAttributes.Add("USt/VSt", "Ust und VSt Schlüssel fürs Konto 9130 (VB/FO USt / Zahllast)");
            TaxAnalysisAttributes.Add("v0", "Vorsteuer 0 %");
            TaxAnalysisAttributes.Add("v7", "Vorsteuer 7 %");
            TaxAnalysisAttributes.Add("v19", "Vorsteuer 19 %");
            TaxAnalysisAttributes.Add("vi0", "Vorsteuer Innenumsatz");
            TaxAnalysisAttributes.Add("veu", "Vorsteuer Innergemeinschaftliche Lieferung");
            return TaxAnalysisAttributes;
        }

        /// <summary>
        /// export the references that are used to mark transactions for taxes
        /// </summary>
        public static void ExportTaxAnalysisAttributes(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            SortedList <string, string>ATaxAnalysisAttributes)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "steuerschluessel.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            foreach (string key in ATaxAnalysisAttributes.Keys)
            {
                sb.Append(StringHelper.StrMerge(new string[] { key, ATaxAnalysisAttributes[key] }, ';'));
                sb.Append(ANewLine);
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}