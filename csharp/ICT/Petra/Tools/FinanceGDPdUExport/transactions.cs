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
using Ict.Petra.Shared.MFinance.Account.Data;
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
            string ACostCentres)
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
                    "AND T.{13} IN ({14}) ORDER BY {0}, {1}, {2}",
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
                    ATransactionTable.GetDebitCreditIndicatorDBName());

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

            DBAccess.GDBAccessObj.RollbackTransaction();
            int rowCounter = 0;

            foreach (ATransactionRow row in transactions.Rows)
            {
                StringBuilder attributes = new StringBuilder();

                DataRowView[] attribs = TransAnalAttrib.DefaultView.FindRows(new object[] { row.BatchNumber, row.JournalNumber, row.TransactionNumber });

                foreach (DataRowView rv in attribs)
                {
                    ATransAnalAttribRow attribRow = (ATransAnalAttribRow)rv.Row;

                    // also export attribRow.AnalysisTypeCode?
                    attributes.Append(attribRow.AnalysisAttributeValue);
                }

                DataRowView[] RelatedTransactions = allTransactionsInJournal.DefaultView.FindRows(new object[] { row.BatchNumber, row.JournalNumber });

                ATransactionRow[] OtherTransactions = GetOtherTransactions(row, RelatedTransactions);

                string OtherCostCentres = string.Empty;
                string OtherAccountCodes = string.Empty;

                foreach (ATransactionRow r in OtherTransactions)
                {
                    OtherCostCentres = StringHelper.AddCSV(OtherCostCentres, r.CostCentreCode);
                    OtherAccountCodes = StringHelper.AddCSV(OtherAccountCodes, r.AccountCode);
                }

                sb.Append(StringHelper.StrMerge(
                        new string[] {
                            row.CostCentreCode,
                            row.AccountCode,
                            "B" + row.BatchNumber.ToString() + "_J" + row.JournalNumber.ToString() + "_T" + row.TransactionNumber.ToString(),
                            String.Format("{0:N}", row.TransactionAmount),
                            row.DebitCreditIndicator ? "Debit" : "Credit",
                            row.TransactionDate.ToString("yyyyMMdd"),
                            OtherCostCentres,
                            OtherAccountCodes,
                            row.Narrative,
                            row.Reference,
                            attributes.ToString()
                        }, ACSVSeparator));

                sb.Append(ANewLine);

                rowCounter++;

                if (rowCounter % 500 == 0)
                {
                    TLogging.Log("Processing transactions " + rowCounter.ToString());
                }
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}