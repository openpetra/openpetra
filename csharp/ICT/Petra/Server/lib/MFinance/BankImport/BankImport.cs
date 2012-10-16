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
using System.Data.Odbc;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data; // Needed indirectly by Ict.Petra.Server.lib.MFinance.Common.dll and Ict.Petra.Shared.lib.data.dll
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.ImportExport;
using Ict.Petra.Server.App.Core;

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
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult StoreNewBankStatement(BankImportTDS AStatementAndTransactionsDS,
            out Int32 AFirstStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Processing new bank statements"),
                AStatementAndTransactionsDS.AEpStatement.Rows.Count + 1);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Saving to database"),
                0);

            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            try
            {
                SubmissionResult = BankImportTDSAccess.SubmitChanges(AStatementAndTransactionsDS, out AVerificationResult);

                AFirstStatementKey = -1;

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("starting to train"),
                        1);

                    AFirstStatementKey = AStatementAndTransactionsDS.AEpStatement[0].StatementKey;

                    // search for already posted gift batches, and do the matching for these imported statements
                    TBankImportMatching.Train(AStatementAndTransactionsDS);
                }

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
                throw;
            }

            return SubmissionResult;
        }

        /// <summary>
        /// returns the bank statements that are from or newer than the given date
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AEpStatementTable GetImportedBankStatements(Int32 ALedgerNumber, DateTime AStartDate)
        {
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AEpStatementTable localTable = new AEpStatementTable();
            AEpStatementRow row = localTable.NewRowTyped(false);

            row.LedgerNumber = ALedgerNumber;
            row.Date = AStartDate;

            StringCollection operators = new StringCollection();
            operators.Add("=");
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

            return !VerificationResult.HasCriticalErrors;
        }

        private static bool FindDonorByAccountNumber(AEpMatchRow AMatchRow,
            DataView APartnerByBankAccount,
            string ABankSortCode,
            string AAccountNumber)
        {
            DataRowView[] rows = APartnerByBankAccount.FindRows(new object[] { ABankSortCode, AAccountNumber });

            if (rows.Length == 1)
            {
                AMatchRow.DonorShortName = rows[0].Row["ShortName"].ToString();
                AMatchRow.DonorKey = Convert.ToInt64(rows[0].Row["PartnerKey"]);
                return true;
            }

            return false;
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

                if (ResultDataset.AEpStatement[0].BankAccountCode.Length == 0)
                {
                    throw new Exception("Loading Bank Statement: Bank Account must not be empty");
                }

                ACostCentreAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AMotivationDetailAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AEpTransactionAccess.LoadViaAEpStatement(ResultDataset, AStatementKey, Transaction);

                BankImportTDS TempDataset = new BankImportTDS();
                AEpTransactionAccess.LoadViaAEpStatement(TempDataset, AStatementKey, Transaction);

                AEpMatchAccess.LoadViaALedger(ResultDataset, ResultDataset.AEpStatement[0].LedgerNumber, Transaction);

                // load all bankingdetails and partner shortnames related to this statement
                string sqlLoadPartnerByBankAccount =
                    "SELECT DISTINCT p.p_partner_key_n AS PartnerKey, " +
                    "p.p_partner_short_name_c AS ShortName, " +
                    "t.p_branch_code_c AS BranchCode, " +
                    "t.a_bank_account_number_c AS BankAccountNumber " +
                    "FROM PUB_a_ep_transaction t, PUB_p_banking_details bd, PUB_p_bank b, PUB_p_partner_banking_details pbd, PUB_p_partner p " +
                    "WHERE t.a_statement_key_i = " + AStatementKey.ToString() + " " +
                    "AND bd.p_bank_account_number_c = t.a_bank_account_number_c " +
                    "AND b.p_partner_key_n = bd.p_bank_key_n " +
                    "AND b.p_branch_code_c = t.p_branch_code_c " +
                    "AND pbd.p_banking_details_key_i = bd.p_banking_details_key_i " +
                    "AND p.p_partner_key_n = pbd.p_partner_key_n";

                DataTable PartnerByBankAccount = DBAccess.GDBAccessObj.SelectDT(sqlLoadPartnerByBankAccount, "partnerByBankAccount", Transaction);
                PartnerByBankAccount.DefaultView.Sort = "BranchCode, BankAccountNumber";

                DBAccess.GDBAccessObj.RollbackTransaction();

                string BankAccountCode = ResultDataset.AEpStatement[0].BankAccountCode;

                DataView EpMatchView = new DataView(ResultDataset.AEpMatch,
                    string.Empty,
                    AEpMatchTable.GetMatchTextDBName(),
                    DataViewRowState.CurrentRows);

                SortedList <string, AEpMatchRow>MatchesToAddLater = new SortedList <string, AEpMatchRow>();

                // load the matches or create new matches
                foreach (BankImportTDSAEpTransactionRow row in ResultDataset.AEpTransaction.Rows)
                {
                    BankImportTDSAEpTransactionRow tempTransactionRow =
                        (BankImportTDSAEpTransactionRow)TempDataset.AEpTransaction.Rows.Find(
                            new object[] {
                                row.StatementKey,
                                row.Order,
                                row.DetailKey
                            });

                    // find a match with the same match text, or create a new one
                    if (row.IsMatchTextNull() || (row.MatchText.Length == 0) || !row.MatchText.StartsWith(BankAccountCode))
                    {
                        row.MatchText = TBankImportMatching.CalculateMatchText(BankAccountCode, row);

                        tempTransactionRow.MatchText = row.MatchText;
                    }

                    DataRowView[] matches = EpMatchView.FindRows(row.MatchText);

                    if (matches.Length > 0)
                    {
                        // update the recent date
                        foreach (DataRowView rv in matches)
                        {
                            AEpMatchRow r = (AEpMatchRow)rv.Row;

                            // check if the recipient key is still valid. could be that they have married, and merged into another family record
                            if ((r.RecipientKey != 0) && (Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                                                                  String.Format("SELECT COUNT(*) FROM PUB_{0} WHERE {1} = {2} AND {3} = '{4}'",
                                                                      PPartnerTable.GetTableDBName(),
                                                                      PPartnerTable.GetPartnerKeyDBName(),
                                                                      r.RecipientKey,
                                                                      PPartnerTable.GetStatusCodeDBName(),
                                                                      MPartnerConstants.PARTNERSTATUS_MERGED),
                                                                  IsolationLevel.ReadCommitted)) == 1))
                            {
                                TLogging.LogAtLevel(1, "partner has been merged: " + r.RecipientKey.ToString());
                                r.RecipientKey = 0;
                                r.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            }

                            if (r.RecentMatch < row.DateEffective)
                            {
                                r.RecentMatch = row.DateEffective;
                            }

                            if (row.IsEpMatchKeyNull() || ((row.EpMatchKey != r.EpMatchKey) && (r.Detail == 0)))
                            {
                                row.EpMatchKey = r.EpMatchKey;
                                tempTransactionRow.EpMatchKey = row.EpMatchKey;
                            }

                            // do not modify tempRow.MatchAction, because that will not be stored in the database anyway, just costs time
                            row.MatchAction = r.Action;

                            if (r.IsDonorKeyNull() || (r.DonorKey <= 0))
                            {
                                FindDonorByAccountNumber(r, PartnerByBankAccount.DefaultView, row.BranchCode, row.BankAccountNumber);
                            }
                        }
                    }
                    else if (!MatchesToAddLater.ContainsKey(row.MatchText))
                    {
                        // create new match
                        AEpMatchRow tempRow = TempDataset.AEpMatch.NewRowTyped(true);
                        tempRow.EpMatchKey = (TempDataset.AEpMatch.Count + MatchesToAddLater.Count + 1) * -1;
                        tempRow.Detail = 0;
                        tempRow.MatchText = row.MatchText;
                        tempRow.LedgerNumber = ALedgerNumber;
                        tempRow.GiftTransactionAmount = row.TransactionAmount;
                        tempRow.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                        FindDonorByAccountNumber(tempRow, PartnerByBankAccount.DefaultView, row.BranchCode, row.BankAccountNumber);

#if disabled
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
#endif

                        MatchesToAddLater.Add(tempRow.MatchText, tempRow);

                        row.EpMatchKey = tempRow.EpMatchKey;
                        tempTransactionRow.EpMatchKey = row.EpMatchKey;

                        // do not modify tempRow.MatchAction, because that will not be stored in the database anyway, just costs time
                        row.MatchAction = tempRow.Action;
                    }
                }

                // for speed reasons, add the new rows after clearing the sort on the view
                EpMatchView.Sort = string.Empty;

                foreach (AEpMatchRow m in MatchesToAddLater.Values)
                {
                    TempDataset.AEpMatch.Rows.Add(m);
                }

                TempDataset.ThrowAwayAfterSubmitChanges = true;
                // only store a_ep_transactions and a_ep_matches, but without additional typed fields (ie MatchAction)
                BankImportTDSAccess.SubmitChanges(TempDataset.GetChangesTyped(true), out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.GetType().ToString() + " in BankImport, GetBankStatementTransactionsAndMatches; " + e.Message);
                TLogging.Log(e.StackTrace);
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }

            // drop all matches that do not occur on this statement
            ResultDataset.AEpMatch.Clear();

            // reloading is faster than deleting all matches that are not needed
            string sqlLoadMatchesOfStatement =
                "SELECT DISTINCT m.* FROM PUB_a_ep_match m, PUB_a_ep_transaction t WHERE t.a_statement_key_i = ? AND m.a_match_text_c = t.a_match_text_c";

            OdbcParameter param = new OdbcParameter("statementkey", OdbcType.Int);
            param.Value = AStatementKey;

            DBAccess.GDBAccessObj.SelectDT(ResultDataset.AEpMatch,
                sqlLoadMatchesOfStatement,
                null,
                new OdbcParameter[] { param }, -1, -1);

            // update the custom field for cost centre name for each match
            foreach (BankImportTDSAEpMatchRow row in ResultDataset.AEpMatch.Rows)
            {
                ACostCentreRow ccRow = (ACostCentreRow)ResultDataset.ACostCentre.Rows.Find(new object[] { row.LedgerNumber, row.CostCentreCode });

                if (ccRow != null)
                {
                    row.CostCentreName = ccRow.CostCentreName;
                }
            }

            // remove all rows that we do not need on the client side
            ResultDataset.AGiftDetail.Clear();
            ResultDataset.AMotivationDetail.Clear();
            ResultDataset.ACostCentre.Clear();

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
            DateTime BatchDateEffective = stmt.Date;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref BatchDateEffective, out DateEffectiveYearNumber,
                    out DateEffectivePeriodNumber,
                    Transaction, true))
            {
                // just use the latest possible date
                string msg =
                    String.Format(Catalog.GetString("Date {0} is not in an open period of the ledger, using date {1} instead for the gift batch."),
                        stmt.Date.ToShortDateString(),
                        BatchDateEffective.ToShortDateString());
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Gift Batch"), msg, TResultSeverity.Resv_Info));
            }

            ACostCentreAccess.LoadViaALedger(AMainDS, ALedgerNumber, Transaction);

            AMainDS.AEpMatch.DefaultView.Sort =
                AEpMatchTable.GetActionDBName() + ", " +
                AEpMatchTable.GetMatchTextDBName();

            foreach (DataRowView dv in AMainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataRowView[] matches = AMainDS.AEpMatch.DefaultView.FindRows(new object[] {
                        MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT,
                        transactionRow.MatchText
                    });

                if (matches.Length > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)matches[0].Row;

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

            string MatchedGiftReference = stmt.Filename;

            if (!stmt.IsBankAccountKeyNull())
            {
                string sqlGetBankSortCode =
                    "SELECT bank.p_branch_code_c " +
                    "FROM PUB_p_banking_details details, PUB_p_bank bank " +
                    "WHERE details.p_banking_details_key_i = ?" +
                    "AND details.p_bank_key_n = bank.p_partner_key_n";
                OdbcParameter param = new OdbcParameter("detailkey", OdbcType.Int);
                param.Value = stmt.BankAccountKey;

                PBankTable bankTable = new PBankTable();
                DBAccess.GDBAccessObj.SelectDT(bankTable, sqlGetBankSortCode, Transaction, new OdbcParameter[] { param }, 0, 0);

                MatchedGiftReference = bankTable[0].BranchCode + " " + stmt.Date.Day.ToString();
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.CreateAGiftBatch(
                ALedgerNumber,
                BatchDateEffective,
                String.Format(Catalog.GetString("bank import for date {0}"), stmt.Date.ToShortDateString()));

            AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];

            decimal HashTotal = 0.0M;

            AMainDS.AEpTransaction.DefaultView.Sort =
                AEpTransactionTable.GetNumberOnPaperStatementDBName();

            AMainDS.AEpMatch.DefaultView.RowFilter = String.Empty;
            AMainDS.AEpMatch.DefaultView.Sort =
                AEpMatchTable.GetActionDBName() + ", " +
                AEpMatchTable.GetMatchTextDBName();

            foreach (DataRowView dv in AMainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataRowView[] matches = AMainDS.AEpMatch.DefaultView.FindRows(new object[] {
                        MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT,
                        transactionRow.MatchText
                    });

                if (matches.Length > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)matches[0].Row;

                    AGiftRow gift = GiftDS.AGift.NewRowTyped();
                    gift.LedgerNumber = giftbatchRow.LedgerNumber;
                    gift.BatchNumber = giftbatchRow.BatchNumber;
                    gift.GiftTransactionNumber = giftbatchRow.LastGiftNumber + 1;
                    gift.DonorKey = match.DonorKey;
                    gift.DateEntered = transactionRow.DateEffective;
                    gift.Reference = MatchedGiftReference;
                    GiftDS.AGift.Rows.Add(gift);
                    giftbatchRow.LastGiftNumber++;

                    foreach (DataRowView r in matches)
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

                        // do not use the description in comment one, because that could show up on the gift receipt???
                        // detail.GiftCommentOne = transactionRow.Description;

                        detail.CommentOneType = MFinanceConstants.GIFT_COMMENT_TYPE_BOTH;
                        detail.CostCentreCode = match.CostCentreCode;
                        detail.RecipientKey = match.RecipientKey;
                        detail.RecipientLedgerNumber = match.RecipientLedgerNumber;

                        // check for active cost centre
                        ACostCentreRow costcentre = (ACostCentreRow)AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, match.CostCentreCode });

                        if ((costcentre == null) || !costcentre.CostCentreActiveFlag)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("creating gift for match {0}"), transactionRow.Description),
                                    Catalog.GetString("Invalid or inactive cost centre"),
                                    TResultSeverity.Resv_Critical));
                        }

                        GiftDS.AGiftDetail.Rows.Add(detail);
                    }
                }
            }

            if (AVerificationResult.HasCriticalErrors)
            {
                return -1;
            }

            giftbatchRow.HashTotal = HashTotal;
            giftbatchRow.BatchTotal = HashTotal;

            // do not overwrite the parameter, because there might be the hint for a different gift batch date
            TVerificationResultCollection VerificationResultSubmitChanges;

            TSubmitChangesResult result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS,
                out VerificationResultSubmitChanges);

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

            GLBatchTDS GLDS = TGLTransactionWebConnector.CreateABatch(ALedgerNumber);

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
                    trans.CostCentreCode = TGLTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
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

            TSubmitChangesResult result = TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDS,
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