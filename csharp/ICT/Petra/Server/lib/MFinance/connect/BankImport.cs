//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data; // Needed indirectly by Ict.Petra.Server.lib.MFinance.Common.dll and Ict.Petra.Shared.lib.data.dll
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MFinance.ImportExport.WebConnectors
{
    /// <summary>
    /// import a bank statement from a CSV file
    /// </summary>
    public class TBankImportWebConnector
    {
        /// <summary>
        /// upload new bank statement so that it can be used for matching etc.
        /// </summary>
        /// <param name="AStmtTable"></param>
        /// <param name="ATransTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult StoreNewBankStatement(ref AEpStatementTable AStmtTable,
            AEpTransactionTable ATransTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            // TODO: check for existing statement with same filename? to avoid duplicate statements? delete older statement?

            AVerificationResult = new TVerificationResultCollection();
            SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                if (AEpStatementAccess.SubmitChanges(AStmtTable, SubmitChangesTransaction, out AVerificationResult))
                {
                    // update statement key reference
                    // supports committing several bank statements at once
                    foreach (AEpTransactionRow row in ATransTable.Rows)
                    {
                        if (row.StatementKey < 0)
                        {
                            row.StatementKey = AStmtTable[(row.StatementKey + 1) * -1].StatementKey;
                        }
                    }

                    if (AEpTransactionAccess.SubmitChanges(ATransTable, SubmitChangesTransaction, out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                TLogging.Log("after submitchanges: exception " + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }

        /// <summary>
        /// returns the bank statements that are from or newer than the given date
        /// </summary>
        /// <param name="AStartDate"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static AEpStatementTable GetImportedBankStatements(DateTime AStartDate)
        {
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AEpStatementTable localTable = new AEpStatementTable();
            AEpStatementRow row = localTable.NewRowTyped(false);

            row.Date = AStartDate;

            StringCollection operators = new StringCollection();
            operators.Add(">=");

            localTable = AEpStatementAccess.LoadUsingTemplate(row, operators, null, ReadTransaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return localTable;
        }

        /// <summary>
        /// drop a bank statement and all its transactions
        /// </summary>
        /// <param name="AEpStatementKey"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool DropBankStatement(Int32 AEpStatementKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            BankImportTDS MainDS = new BankImportTDS();

            AEpStatementAccess.LoadByPrimaryKey(MainDS, AEpStatementKey, Transaction);
            AEpTransactionAccess.LoadViaAEpStatement(MainDS, AEpStatementKey, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (AEpStatementRow stmtRow in MainDS.AEpStatement.Rows)
            {
                stmtRow.Delete();
            }

            foreach (AEpTransactionRow transactionRow in MainDS.AEpTransaction.Rows)
            {
                transactionRow.Delete();
            }

            TVerificationResultCollection VerificationResult;
            BankImportTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            return !VerificationResult.HasCriticalError();
        }

        /// <summary>
        /// match text should uniquely identify a gift from a certain donor with a certain purpose;
        /// use account name, description, and amount;
        /// remove umlaut and spaces, because the banks sometimes play around with them
        /// </summary>
        private static string CalculateMatchText(string ABankAccount, AEpTransactionRow tr)
        {
            string matchtext = ABankAccount + tr.AccountName + tr.Description;

            matchtext += tr.TransactionAmount.ToString("0.##");

            matchtext = matchtext.Replace(",", "").Replace("/", "").Replace("-", "").Replace(";", "").Replace(".", "");

            string oldMatchText = String.Empty;

            while (oldMatchText != matchtext)
            {
                oldMatchText = matchtext;
                matchtext = matchtext.ToUpper();
                matchtext = matchtext.Replace("UE", "");
                matchtext = matchtext.Replace("AE", "");
                matchtext = matchtext.Replace("OE", "");
                matchtext = matchtext.Replace("SS", "");
                matchtext = matchtext.Replace("Ü", "");
                matchtext = matchtext.Replace("Ä", "");
                matchtext = matchtext.Replace("Ö", "");
                matchtext = matchtext.Replace("ß", "");
                matchtext = matchtext.Replace(" ", "");
            }

            if (matchtext.Length > AEpTransactionTable.GetMatchTextLength())
            {
                // calculate unique check sum which is shorter than the whole match text
                MD5CryptoServiceProvider cr = new MD5CryptoServiceProvider();
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] matchbytes = encoding.GetBytes(matchtext);
                matchtext = BitConverter.ToString(cr.ComputeHash(matchbytes)).Replace("-", "").ToLower();
            }

            return matchtext;
        }

        /// <summary>
        /// returns the transactions of the bank statement, and the matches if they exist;
        /// tries to find matches too
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static BankImportTDS GetBankStatementTransactionsAndMatches(Int32 AStatementKey, Int32 ALedgerNumber)
        {
            TVerificationResultCollection VerificationResult;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            BankImportTDS ResultDataset = new BankImportTDS();

            try
            {
                AEpStatementAccess.LoadByPrimaryKey(ResultDataset, AStatementKey, Transaction);

                ACostCentreAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                // TODO load Motivation Groups as well
                AMotivationDetailAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AEpTransactionAccess.LoadViaAEpStatement(ResultDataset, AStatementKey, Transaction);

                // load the matches or create new matches
                foreach (BankImportTDSAEpTransactionRow row in ResultDataset.AEpTransaction.Rows)
                {
                    string BankAccountCode = ResultDataset.AEpStatement[0].BankAccountCode;

                    // find a match with the same match text, or create a new one
                    if (row.IsMatchTextNull() || (row.MatchText.Length == 0) || !row.MatchText.StartsWith(BankAccountCode))
                    {
                        row.MatchText = CalculateMatchText(BankAccountCode, row);
                    }

                    AEpMatchTable tempTable = new AEpMatchTable();
                    AEpMatchRow tempRow = tempTable.NewRowTyped(false);
                    tempRow.MatchText = row.MatchText;

                    tempTable = AEpMatchAccess.LoadUsingTemplate(tempRow, Transaction);

                    if (tempTable.Count > 0)
                    {
                        // update the recent date
                        bool update = false;

                        foreach (AEpMatchRow tempRow2 in tempTable.Rows)
                        {
                            if (tempRow2.RecentMatch < row.DateEffective)
                            {
                                tempRow2.RecentMatch = row.DateEffective;
                                update = true;
                            }
                        }

                        if (update)
                        {
                            AEpMatchAccess.SubmitChanges(tempTable, Transaction, out VerificationResult);
                        }

                        row.EpMatchKey = tempTable[0].EpMatchKey;
                        row.MatchAction = tempTable[0].Action;

                        ResultDataset.AEpMatch.Merge(tempTable);
                    }
                    else
                    {
                        // create new match
                        tempRow = tempTable.NewRowTyped(true);
                        tempRow.EpMatchKey = -1;
                        tempRow.Detail = 0;
                        tempRow.MatchText = row.MatchText;
                        tempRow.LedgerNumber = ALedgerNumber;
                        tempRow.GiftTransactionAmount = row.TransactionAmount;
                        tempRow.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                        // fuzzy search for the partner. only return if unique result
                        string sql =
                            "SELECT p_partner_key_n, p_partner_short_name_c FROM p_partner WHERE p_partner_short_name_c LIKE '{0}%' OR p_partner_short_name_c LIKE '{1}%'";
                        string[] names = row.AccountName.Split(new char[] { ' ' });

                        if (names.Length > 1)
                        {
                            string optionShortName1 = names[0] + ", " + names[1];
                            string optionShortName2 = names[1] + ", " + names[0];

                            DataTable partner = DBAccess.GDBAccessObj.SelectDT(String.Format(sql,
                                    optionShortName1,
                                    optionShortName2), "partner", Transaction);

                            if (partner.Rows.Count == 1)
                            {
                                tempRow.DonorKey = Convert.ToInt64(partner.Rows[0][0]);
                            }
                        }

                        tempTable.Rows.Add(tempRow);
                        AEpMatchAccess.SubmitChanges(tempTable, Transaction, out VerificationResult);
                        row.EpMatchKey = tempTable[0].EpMatchKey;
                        row.MatchAction = tempTable[0].Action;
                        ResultDataset.AEpMatch.Merge(tempTable);
                    }
                }

                AEpTransactionAccess.SubmitChanges(ResultDataset.AEpTransaction, Transaction, out VerificationResult);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log(e.GetType().ToString() + " in BankImport, GetBankStatementTransactionsAndMatches; " + e.Message);
                TLogging.Log(e.StackTrace);
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw e;
            }

            // update the custom field for cost centre name for each match
            foreach (BankImportTDSAEpMatchRow row in ResultDataset.AEpMatch.Rows)
            {
                ResultDataset.ACostCentre.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    ACostCentreTable.GetCostCentreCodeDBName(), row.CostCentreCode);

                if (ResultDataset.ACostCentre.DefaultView.Count == 1)
                {
                    row.CostCentreName = ((ACostCentreRow)ResultDataset.ACostCentre.DefaultView[0].Row).CostCentreName;
                }
            }

            ResultDataset.AcceptChanges();

            return ResultDataset;
        }

        /// <summary>
        /// commit matches into a_ep_match
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CommitMatches(BankImportTDS AMainDS)
        {
            TVerificationResultCollection VerificationResult;

            return BankImportTDSAccess.SubmitChanges(AMainDS, out VerificationResult) == TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// create a gift batch for the matched gifts, and return the gift batch number
        /// </summary>
        /// <returns>the gift batch number</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 CreateGiftBatch(BankImportTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 AStatementKey,
            Int32 AGiftBatchNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            AMainDS.AEpTransaction.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpTransactionTable.GetStatementKeyDBName(),
                    AStatementKey);
            AMainDS.AEpStatement.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    AStatementKey);
            AEpStatementRow stmt = (AEpStatementRow)AMainDS.AEpStatement.DefaultView[0].Row;

            // TODO: optional: use the preselected gift batch, AGiftBatchNumber

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, stmt.Date, out DateEffectivePeriodNumber, out DateEffectiveYearNumber,
                    Transaction))
            {
                string msg =
                    String.Format(Catalog.GetString("Cannot create a gift batch for date {0} since it is not in an open period of the ledger."),
                        stmt.Date.ToShortDateString());
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Gift Batch"), msg, TResultSeverity.Resv_Critical));
                DBAccess.GDBAccessObj.RollbackTransaction();
                return -1;
            }

            foreach (DataRowView dv in AMainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataView v = AMainDS.AEpMatch.DefaultView;
                v.RowFilter = AEpMatchTable.GetActionDBName() + " = '" + MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT + "' and " +
                              AEpMatchTable.GetMatchTextDBName() + " = '" + transactionRow.MatchText + "'";

                if (v.Count > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)v[0].Row;

                    if (match.IsDonorKeyNull() || (match.DonorKey == 0))
                    {
                        string msg =
                            String.Format(Catalog.GetString("Cannot create a gift for transaction {0} since there is no valid donor."),
                                transactionRow.Description);
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Gift Batch"), msg, TResultSeverity.Resv_Critical));
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        return -1;
                    }
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            AAccountTable AccountTable = (AAccountTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            GiftBatchTDS GiftDS = Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.CreateAGiftBatch(ALedgerNumber, stmt.Date);

            AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];
            giftbatchRow.BatchDescription = String.Format(Catalog.GetString("bank import for date {0}"), stmt.Date.ToShortDateString());

            decimal HashTotal = 0.0M;

            foreach (DataRowView dv in AMainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;
                DataView v = AMainDS.AEpMatch.DefaultView;
                v.RowFilter = AEpMatchTable.GetActionDBName() + " = '" + MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT + "' and " +
                              AEpMatchTable.GetMatchTextDBName() + " = '" + transactionRow.MatchText + "'";

                if (v.Count > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)v[0].Row;

                    AGiftRow gift = GiftDS.AGift.NewRowTyped();
                    gift.LedgerNumber = giftbatchRow.LedgerNumber;
                    gift.BatchNumber = giftbatchRow.BatchNumber;
                    gift.GiftTransactionNumber = giftbatchRow.LastGiftNumber + 1;
                    gift.DonorKey = match.DonorKey;
                    gift.DateEntered = transactionRow.DateEffective;
                    GiftDS.AGift.Rows.Add(gift);
                    giftbatchRow.LastGiftNumber++;

                    foreach (DataRowView r in v)
                    {
                        match = (AEpMatchRow)r.Row;

                        AGiftDetailRow detail = GiftDS.AGiftDetail.NewRowTyped();
                        detail.LedgerNumber = gift.LedgerNumber;
                        detail.BatchNumber = gift.BatchNumber;
                        detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                        detail.DetailNumber = gift.LastDetailNumber + 1;
                        gift.LastDetailNumber++;

                        detail.GiftTransactionAmount = match.GiftTransactionAmount;
                        HashTotal += match.GiftTransactionAmount;
                        detail.MotivationGroupCode = match.MotivationGroupCode;
                        detail.MotivationDetailCode = match.MotivationDetailCode;
                        detail.GiftCommentOne = transactionRow.Description;
                        detail.CostCentreCode = match.CostCentreCode;

                        // check for active cost centre
                        ACostCentreRow costcentre = (ACostCentreRow)AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, match.CostCentreCode });

                        if ((costcentre == null) || !costcentre.CostCentreActiveFlag)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("creating gift for match {0}"), transactionRow.Description),
                                    Catalog.GetString("Invalid or inactive cost centre"),
                                    TResultSeverity.Resv_Critical));
                        }

                        // check for active account
                        AAccountRow account = (AAccountRow)AccountTable.Rows.Find(new object[] { ALedgerNumber, match.AccountCode });

                        if ((account == null) || !account.AccountActiveFlag)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("creating gift for match {0}"), transactionRow.Description),
                                    Catalog.GetString("Invalid or inactive account code"),
                                    TResultSeverity.Resv_Critical));
                        }

                        GiftDS.AGiftDetail.Rows.Add(detail);
                    }
                }
            }

            if (AVerificationResult.HasCriticalError())
            {
                return -1;
            }

            giftbatchRow.HashTotal = HashTotal;

            TSubmitChangesResult result = Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS,
                out AVerificationResult);

            if (result == TSubmitChangesResult.scrOK)
            {
                return giftbatchRow.BatchNumber;
            }

            return -1;
        }

        /// <summary>
        /// create a GL batch for the matched GL Transactions, and return the GL batch number
        /// </summary>
        /// <returns>the GL batch number</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 CreateGLBatch(BankImportTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 AStatementKey,
            Int32 AGLBatchNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            AMainDS.AEpTransaction.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpTransactionTable.GetStatementKeyDBName(),
                    AStatementKey);
            AMainDS.AEpStatement.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    AStatementKey);
            AEpStatementRow stmt = (AEpStatementRow)AMainDS.AEpStatement.DefaultView[0].Row;

            AVerificationResult = null;

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, stmt.Date, out DateEffectivePeriodNumber, out DateEffectiveYearNumber,
                    Transaction))
            {
                string msg = String.Format(Catalog.GetString("Cannot create a GL batch for date {0} since it is not in an open period of the ledger."),
                    stmt.Date.ToShortDateString());
                TLogging.Log(msg);
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating GL Batch"), msg, TResultSeverity.Resv_Critical));

                DBAccess.GDBAccessObj.RollbackTransaction();
                return -1;
            }

            Int32 BatchYear, BatchPeriod;

            // if DateEffective is outside the range of open periods, use the most fitting date
            DateTime DateEffective = stmt.Date;
            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref DateEffective, out BatchYear, out BatchPeriod, Transaction, true);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            GLBatchTDS GLDS = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow glbatchRow = GLDS.ABatch[0];
            glbatchRow.BatchPeriod = BatchPeriod;
            glbatchRow.DateEffective = DateEffective;
            glbatchRow.BatchDescription = String.Format(Catalog.GetString("bank import for date {0}"), stmt.Date.ToShortDateString());

            decimal HashTotal = 0.0M;
            decimal DebitTotal = 0.0M;
            decimal CreditTotal = 0.0M;

            // TODO: support several journals
            // TODO: support several currencies, support other currencies than the base currency
            AJournalRow gljournalRow = GLDS.AJournal.NewRowTyped();
            gljournalRow.LedgerNumber = glbatchRow.LedgerNumber;
            gljournalRow.BatchNumber = glbatchRow.BatchNumber;
            gljournalRow.JournalNumber = glbatchRow.LastJournal + 1;
            gljournalRow.TransactionCurrency = LedgerTable[0].BaseCurrency;
            glbatchRow.LastJournal++;
            gljournalRow.JournalPeriod = glbatchRow.BatchPeriod;
            gljournalRow.DateEffective = glbatchRow.DateEffective;
            gljournalRow.JournalDescription = glbatchRow.BatchDescription;
            gljournalRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            gljournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
            gljournalRow.ExchangeRateToBase = 1.0m;
            GLDS.AJournal.Rows.Add(gljournalRow);

            foreach (DataRowView dv in AMainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataView v = AMainDS.AEpMatch.DefaultView;
                v.RowFilter = AEpMatchTable.GetActionDBName() + " = '" + MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL + "' and " +
                              AEpMatchTable.GetMatchTextDBName() + " = '" + transactionRow.MatchText + "'";

                if (v.Count > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)v[0].Row;
                    ATransactionRow trans = GLDS.ATransaction.NewRowTyped();
                    trans.LedgerNumber = glbatchRow.LedgerNumber;
                    trans.BatchNumber = glbatchRow.BatchNumber;
                    trans.JournalNumber = gljournalRow.JournalNumber;
                    trans.TransactionNumber = gljournalRow.LastTransactionNumber + 1;
                    trans.AccountCode = match.AccountCode;
                    trans.CostCentreCode = match.CostCentreCode;
                    trans.Reference = match.Reference;
                    trans.Narrative = match.Narrative;
                    trans.TransactionDate = transactionRow.DateEffective;

                    if (transactionRow.TransactionAmount < 0)
                    {
                        trans.AmountInBaseCurrency = -1 * transactionRow.TransactionAmount;
                        trans.TransactionAmount = -1 * transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = true;
                        DebitTotal += trans.AmountInBaseCurrency;
                    }
                    else
                    {
                        trans.AmountInBaseCurrency = transactionRow.TransactionAmount;
                        trans.TransactionAmount = transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = false;
                        CreditTotal += trans.AmountInBaseCurrency;
                    }

                    GLDS.ATransaction.Rows.Add(trans);
                    gljournalRow.LastTransactionNumber++;

                    // add one transaction for the bank as well
                    trans = GLDS.ATransaction.NewRowTyped();
                    trans.LedgerNumber = glbatchRow.LedgerNumber;
                    trans.BatchNumber = glbatchRow.BatchNumber;
                    trans.JournalNumber = gljournalRow.JournalNumber;
                    trans.TransactionNumber = gljournalRow.LastTransactionNumber + 1;
                    trans.AccountCode = stmt.BankAccountCode;
                    trans.CostCentreCode = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
                    trans.Reference = match.Reference;
                    trans.Narrative = match.Narrative;
                    trans.TransactionDate = transactionRow.DateEffective;

                    if (transactionRow.TransactionAmount < 0)
                    {
                        trans.AmountInBaseCurrency = -1 * transactionRow.TransactionAmount;
                        trans.TransactionAmount = -1 * transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = false;
                        CreditTotal += trans.AmountInBaseCurrency;
                    }
                    else
                    {
                        trans.AmountInBaseCurrency = transactionRow.TransactionAmount;
                        trans.TransactionAmount = transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = true;
                        DebitTotal += trans.AmountInBaseCurrency;
                    }

                    GLDS.ATransaction.Rows.Add(trans);
                    gljournalRow.LastTransactionNumber++;
                }
            }

            gljournalRow.JournalDebitTotal = DebitTotal;
            gljournalRow.JournalCreditTotal = CreditTotal;
            glbatchRow.BatchDebitTotal = DebitTotal;
            glbatchRow.BatchCreditTotal = CreditTotal;
            glbatchRow.BatchControlTotal = HashTotal;

            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.SaveGLBatchTDS(ref GLDS,
                out VerificationResult);

            if (result == TSubmitChangesResult.scrOK)
            {
                return glbatchRow.BatchNumber;
            }

            TLogging.Log("Problems storing GL Batch");
            return -1;
        }
    }
}