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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.ImportExport
{
    /// <summary>
    /// train the matching for bank import, by comparing existing batches with old bank statements
    /// </summary>
    public class TBankImportMatching
    {
        private class GiftBatchCounter
        {
            public bool PostedBatch = false;
            public Int32 CounterMatchedGifts = 0;
        }

        /// <summary>
        /// train with imported bank statements and existing gift batches
        /// </summary>
        public static void Train(BankImportTDS AStatementDS)
        {
            int stmtCounter = 0;

            // go through all statements in the dataset, and find gift matches for those days
            foreach (AEpStatementRow stmt in AStatementDS.AEpStatement.Rows)
            {
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    String.Format(Catalog.GetString("training statement {0} {1}"),
                        stmt.Filename, StringHelper.DateToLocalizedString(stmt.Date)),
                    1 + stmtCounter);
                stmtCounter++;

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    return;
                }

                // first stage: collect historic matches from database:
                // go through each transaction of the statement,
                // and see if you can find a donation on that date with the same amount from the same bank account
                // store this as a match

                BankImportTDS MainDS = LoadData(stmt.LedgerNumber, stmt.StatementKey);

                // simple matching; no split gifts, bank account number fits and amount fits
                // problem: recipient different????
                SortedList <Int32,
                            TBankImportMatching.GiftBatchCounter>MatchedGiftBatches = new SortedList <int, TBankImportMatching.GiftBatchCounter>();

                // Get all gifts at given date
                GetGiftsByDate(stmt.LedgerNumber, MainDS, stmt.Date);

                // create the dataview only after loading, otherwise loading is much slower
                DataView GiftDetailByAmountAndDonor = new DataView(MainDS.AGiftDetail,
                    string.Empty,
                    AGiftDetailTable.GetGiftAmountDBName() + "," +
                    BankImportTDSAGiftDetailTable.GetDonorKeyDBName(),
                    DataViewRowState.CurrentRows);

                DataView GiftByAmountAndDonor = new DataView(MainDS.AGift,
                    string.Empty,
                    BankImportTDSAGiftTable.GetTotalAmountDBName() + "," +
                    AGiftTable.GetDonorKeyDBName(),
                    DataViewRowState.CurrentRows);

                MainDS.PBankingDetails.DefaultView.Sort = BankImportTDSPBankingDetailsTable.GetBankSortCodeDBName() + "," +
                                                          BankImportTDSPBankingDetailsTable.GetBankAccountNumberDBName();

                foreach (BankImportTDSAEpTransactionRow transaction in MainDS.AEpTransaction.Rows)
                {
                    // find the donor for this transaction, by his bank account number
                    Int64 DonorKey = GetDonorByBankAccountNumber(MainDS, transaction.BranchCode, transaction.BankAccountNumber);

                    if (transaction.BankAccountNumber.Length == 0)
                    {
                        // useful for NUnit testing for csv import: partnerkey in description
                        try
                        {
                            DonorKey = Convert.ToInt64(transaction.Description);
                        }
                        catch (Exception)
                        {
                            DonorKey = -1;
                        }
                    }

                    if (DonorKey == -1)
                    {
                        continue;
                    }

                    DataRowView[] giftDetails = GiftDetailByAmountAndDonor.FindRows(new object[] { transaction.TransactionAmount, DonorKey });

                    BankImportTDSAGiftDetailRow detailrow = null;

                    if (giftDetails.Length == 1)
                    {
                        // found a possible match
                        detailrow = (BankImportTDSAGiftDetailRow)giftDetails[0].Row;
                    }
                    else
                    {
                        // check if we can find a gift with several gift details, that would match this transaction amount
                        DataRowView[] gifts = GiftByAmountAndDonor.FindRows(new object[] { transaction.TransactionAmount, DonorKey });

                        if (gifts.Length >= 1)
                        {
                            AGiftRow gift = (AGiftRow)gifts[0].Row;
                            detailrow =
                                (BankImportTDSAGiftDetailRow)MainDS.AGiftDetail.Rows.Find(new object[] { gift.LedgerNumber, gift.BatchNumber,
                                                                                                         gift.GiftTransactionNumber,
                                                                                                         1 });
                        }
                    }

                    if (detailrow != null)
                    {
                        if (MatchedGiftBatches.ContainsKey(detailrow.BatchNumber))
                        {
                            MatchedGiftBatches[detailrow.BatchNumber].CounterMatchedGifts++;
                        }
                        else
                        {
                            MatchedGiftBatches.Add(detailrow.BatchNumber, new TBankImportMatching.GiftBatchCounter());
                            MatchedGiftBatches[detailrow.BatchNumber].CounterMatchedGifts++;
                            MatchedGiftBatches[detailrow.BatchNumber].PostedBatch = (detailrow.BatchStatus == "Posted");
                        }

                        // we have found exactly one gift detail which matches the donor and the amount and the date
                        // but it might be that the donation was for a different recipient
                        // do not mark matched here yet
                        //MarkTransactionMatched(ref AMainDS, ref stmtRow, detailrow);
                    }
                }

                int SelectedGiftBatch = -1;
                int maxMatches = 0;

                foreach (int GiftBatchNumber in MatchedGiftBatches.Keys)
                {
                    if (MatchedGiftBatches[GiftBatchNumber].CounterMatchedGifts > maxMatches)
                    {
                        maxMatches = MatchedGiftBatches[GiftBatchNumber].CounterMatchedGifts;
                        SelectedGiftBatch = GiftBatchNumber;
                    }
                }

                if (SelectedGiftBatch == -1)
                {
                    // no matches at all
                    continue;
                }
                else if ((MainDS.AEpTransaction.Rows.Count > 2)
                         && (MatchedGiftBatches[SelectedGiftBatch].CounterMatchedGifts < MainDS.AEpTransaction.Rows.Count / 2))
                {
                    TLogging.Log(
                        "cannot find enough gifts that look the same, for statement " + stmt.Filename + ". CountMatches: " +
                        MatchedGiftBatches[SelectedGiftBatch].CounterMatchedGifts.ToString());

                    // continue to next statement. we cannot find the right gift batch
                    continue;
                }

                CreateMatches(MainDS, stmt, SelectedGiftBatch, MatchedGiftBatches[SelectedGiftBatch].PostedBatch);
            }
        }

        /// <summary>
        /// return a table with gift details for the given date with donor partner keys and bank account numbers
        /// </summary>
        private static bool GetGiftsByDate(Int32 ALedgerNumber, BankImportTDS AMainDS, DateTime ADateEffective)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            // first get all gifts, even those that have no bank account associated
            string stmt = TDataBase.ReadSqlFile("BankImport.GetDonationsByDate.sql");

            OdbcParameter[] parameters = new OdbcParameter[2];
            parameters[0] = new OdbcParameter("ALedgerNumber", OdbcType.Int);
            parameters[0].Value = ALedgerNumber;
            parameters[1] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[1].Value = ADateEffective;

            DBAccess.GDBAccessObj.SelectDT(AMainDS.AGiftDetail, stmt, transaction, parameters, 0, 0);

            // calculate the totals of gifts
            AMainDS.AGift.Clear();

            foreach (BankImportTDSAGiftDetailRow giftdetail in AMainDS.AGiftDetail.Rows)
            {
                BankImportTDSAGiftRow giftRow =
                    (BankImportTDSAGiftRow)AMainDS.AGift.Rows.Find(new object[] { giftdetail.LedgerNumber, giftdetail.BatchNumber,
                                                                                  giftdetail.GiftTransactionNumber });

                if (giftRow == null)
                {
                    giftRow = AMainDS.AGift.NewRowTyped(true);
                    giftRow.LedgerNumber = giftdetail.LedgerNumber;
                    giftRow.BatchNumber = giftdetail.BatchNumber;
                    giftRow.GiftTransactionNumber = giftdetail.GiftTransactionNumber;
                    giftRow.TotalAmount = 0;
                    giftRow.DonorKey = giftdetail.DonorKey;
                    AMainDS.AGift.Rows.Add(giftRow);
                }

                giftRow.TotalAmount += giftdetail.GiftTransactionAmount;
            }

            // get PartnerKey and banking details (most important BankAccountNumber) for all donations on the given date
            stmt = TDataBase.ReadSqlFile("BankImport.GetBankAccountByDate.sql");
            parameters = new OdbcParameter[2];
            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
            parameters[0].Value = ALedgerNumber;
            parameters[1] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[1].Value = ADateEffective;

            // There can be several donors with the same banking details
            AMainDS.PBankingDetails.Constraints.Clear();

            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.PBankingDetails.TableName, transaction, parameters);
            DBAccess.GDBAccessObj.RollbackTransaction();

            return true;
        }

        private static Int64 GetDonorByBankAccountNumber(BankImportTDS AMainDS, string ABankSortCode, string ABankAccountNumber)
        {
            if (Regex.IsMatch(ABankAccountNumber, "^[A-Z]"))
            {
                // TODO search for IBAN / BIC instead of bank sort code and account number

                // For the moment, we try to assume sort code and account number
                // it might be wrong, but then we would not find a donor anyway.
                // we should definitely not store these calculated numbers
                // perhaps do validation against https://kontocheck.solidcharity.com
                string IBAN = ABankAccountNumber;
                ABankSortCode = IBAN.Substring(4, 8);
                ABankAccountNumber = IBAN.Substring(12).TrimStart(new char[] { '0' });
                // TLogging.Log("IBAN " + IBAN + " converted to sort code " + ABankSortCode + " and account number + " + ABankAccountNumber);
            }

            DataRowView[] bankingDetails = AMainDS.PBankingDetails.DefaultView.FindRows(new object[] { ABankSortCode, ABankAccountNumber });

            if (bankingDetails.Length > 0)
            {
                // TODO: just return the first partner key; usually not 2 people owning the same bank account donate at the same time???
                BankImportTDSPBankingDetailsRow row = (BankImportTDSPBankingDetailsRow)bankingDetails[0].Row;
                return row.PartnerKey;
            }

            return -1;
        }

        private static BankImportTDS LoadData(Int32 ALedgerNumber, Int32 AStatementKey)
        {
            TDBTransaction dbtransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            BankImportTDS MatchDS = new BankImportTDS();

            // TODO: would it help not to load all?
            AEpMatchAccess.LoadViaALedger(MatchDS, ALedgerNumber, dbtransaction);

            string sqlLoadTransactions = String.Format(
                "SELECT * FROM PUB_{0} WHERE {1} = ?",
                BankImportTDSAEpTransactionTable.GetTableDBName(),
                BankImportTDSAEpTransactionTable.GetStatementKeyDBName());

            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("statementkey", OdbcType.Int);
            parameters[0].Value = AStatementKey;
            DBAccess.GDBAccessObj.Select(MatchDS, sqlLoadTransactions, MatchDS.AEpTransaction.TableName, dbtransaction, parameters, 0, 0);

            DBAccess.GDBAccessObj.RollbackTransaction();

            MatchDS.AEpMatch.AcceptChanges();
            MatchDS.AEpTransaction.AcceptChanges();

            return MatchDS;
        }

        private static void CreateMatches(BankImportTDS AMainDS,
            AEpStatementRow ACurrentStatement,
            Int32 ASelectedGiftBatch, bool APostedBatch)
        {
            while (MatchTransactionsToGiftBatch(AMainDS,
                       ASelectedGiftBatch, APostedBatch))
            {
            }

            StoreCurrentMatches(AMainDS, ACurrentStatement, ASelectedGiftBatch);
        }

        private static void MarkTransactionMatched(
            BankImportTDS AMainDS,
            DataView AGiftDetailView,
            BankImportTDSAEpTransactionRow transactionRow,
            BankImportTDSAGiftDetailRow giftDetail, bool AMatchAllGiftDetails)
        {
            if (AMatchAllGiftDetails)
            {
                // recursive call, for each gift detail that is matched by this transaction
                DataRowView[] rows = AGiftDetailView.FindRows(new object[] { giftDetail.BatchNumber, giftDetail.GiftTransactionNumber, false });

                foreach (DataRowView rv in rows)
                {
                    MarkTransactionMatched(AMainDS, AGiftDetailView, transactionRow, (BankImportTDSAGiftDetailRow)rv.Row, false);
                }

                return;
            }

            giftDetail.AlreadyMatched = true;

            if (giftDetail.RecipientDescription.Length == 0)
            {
                giftDetail.RecipientDescription = giftDetail.MotivationGroupCode + "/" + giftDetail.MotivationDetailCode;
            }

            transactionRow.MatchAction = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
            transactionRow.GiftLedgerNumber = giftDetail.LedgerNumber;
            transactionRow.GiftBatchNumber = giftDetail.BatchNumber;
            transactionRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
            transactionRow.DonorKey = giftDetail.DonorKey;
        }

        private static Int32 MatchingWords(string AShortname, string AFreeText)
        {
            StringCollection words =
                StringHelper.StrSplit(Calculations.FormatShortName(AShortname,
                        eShortNameFormat.eReverseWithoutTitle).Replace(", ", ",").Replace(" ", ","), ",");

            Int32 Result = 0;

            foreach (string s in words)
            {
                if (AFreeText.ToUpper().IndexOf(s.Trim().ToUpper()) > -1)
                {
                    Result++;
                }
            }

            return Result;
        }

        private static Decimal SumAmounts(DataView AGiftDetailViewByTransactionNumber, Int32 ASelectedGiftBatch,
            Int32 AGiftTransactionNumber)
        {
            Decimal Result = 0.0m;

            DataRowView[] detailsOfGift = AGiftDetailViewByTransactionNumber.FindRows(
                new object[] { ASelectedGiftBatch, AGiftTransactionNumber });

            foreach (DataRowView rv in detailsOfGift)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                Result += detailrow.GiftTransactionAmount;
            }

            return Result;
        }

        /// <summary>
        /// match imported transactions from bank statement to an existing gift batch
        /// </summary>
        /// <returns>true while new matches are found</returns>
        private static bool MatchTransactionsToGiftBatch(BankImportTDS AMainDS,
            Int32 ASelectedGiftBatch,
            bool APostedBatch)
        {
            bool newMatchFound = false;

            if (TLogging.DebugLevel > 0)
            {
                TLogging.Log("begin MatchTransactionsToGiftBatch");
            }

            DataView GiftDetailWithAmountView = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                AGiftDetailTable.GetGiftAmountDBName() + "," +
                BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + "," +
                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailWithoutAmountView = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + "," +
                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailByGiftTransactionNumber = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                AGiftDetailTable.GetBatchNumberDBName() + "," +
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailByGiftTransactionNumberMatchStatus = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                AGiftDetailTable.GetBatchNumberDBName() + "," +
                AGiftDetailTable.GetGiftTransactionNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailByBatchNumberMatchStatus = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                AGiftDetailTable.GetBatchNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
            {
                if (transaction.MatchAction != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    // problem: what if bank account is used by several donors?
                    Int64 DonorKey = GetDonorByBankAccountNumber(AMainDS, transaction.BranchCode, transaction.BankAccountNumber);

                    if (transaction.BankAccountNumber.Length == 0)
                    {
                        // useful for NUnit testing for csv import: partnerkey in description
                        try
                        {
                            DonorKey = Convert.ToInt64(transaction.Description);
                        }
                        catch (Exception)
                        {
                            DonorKey = -1;
                        }
                    }

                    // look for gifts that match the donor (identified by account number) and the transaction amount
                    DataRowView[] GiftDetailsWithAmount = GiftDetailWithAmountView.FindRows(
                        new object[] { transaction.TransactionAmount,
                                       DonorKey, ASelectedGiftBatch, false });

                    if (GiftDetailsWithAmount.Length == 1)
                    {
                        decimal sumGift = SumAmounts(GiftDetailByGiftTransactionNumber, ASelectedGiftBatch,
                            ((BankImportTDSAGiftDetailRow)GiftDetailsWithAmount[0].Row).GiftTransactionNumber);

                        if (sumGift == transaction.TransactionAmount)
                        {
                            // found exactly one match
                            newMatchFound = true;
                            MarkTransactionMatched(AMainDS,
                                GiftDetailByGiftTransactionNumberMatchStatus,
                                transaction,
                                (BankImportTDSAGiftDetailRow)GiftDetailsWithAmount[0].Row,
                                false);
                        }
                    }
                    else if (GiftDetailsWithAmount.Length > 1)
                    {
                        // donor has several gifts with same amount?
                        // look for fitting words in description
                        int MaxCount = -1;
                        BankImportTDSAGiftDetailRow MaxRow = null;

                        foreach (DataRowView rv2 in GiftDetailsWithAmount)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv2.Row;

                            decimal sumGift = SumAmounts(GiftDetailByGiftTransactionNumber, ASelectedGiftBatch,
                                detailrow.GiftTransactionNumber);

                            if (sumGift != transaction.TransactionAmount)
                            {
                                continue;
                            }

                            int count = MatchingWords(detailrow.RecipientDescription, transaction.Description);

                            if (count > MaxCount)
                            {
                                MaxCount = count;
                                MaxRow = detailrow;
                            }
                        }

                        if (MaxCount > 0)
                        {
                            // found a match
                            newMatchFound = true;
                            MarkTransactionMatched(AMainDS, GiftDetailByGiftTransactionNumberMatchStatus, transaction, MaxRow, false);
                        }
                    }
                    else if (GiftDetailsWithAmount.Length == 0)
                    {
                        // split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount

                        // get all gifts with that bank account number
                        DataRowView[] GiftDetailsWithoutAmount = GiftDetailWithoutAmountView.FindRows(
                            new object[] { DonorKey, ASelectedGiftBatch, false });

                        if (GiftDetailsWithoutAmount.Length > 1)
                        {
                            BankImportTDSAGiftDetailRow matchingGiftDetail = null;
                            bool duplicateMatches = false;

                            foreach (DataRowView rv2 in GiftDetailsWithoutAmount)
                            {
                                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv2.Row;

                                if ((matchingGiftDetail == null) || (detailrow.GiftTransactionNumber != matchingGiftDetail.GiftTransactionNumber))
                                {
                                    decimal sumGift = SumAmounts(GiftDetailByGiftTransactionNumber, ASelectedGiftBatch,
                                        detailrow.GiftTransactionNumber);

                                    if (sumGift == transaction.TransactionAmount)
                                    {
                                        if ((matchingGiftDetail != null)
                                            && (matchingGiftDetail.GiftTransactionNumber != detailrow.GiftTransactionNumber))
                                        {
                                            duplicateMatches = true;
                                        }

                                        matchingGiftDetail = detailrow;
                                    }
                                }
                            }

                            // several gift details match this amount
                            if (!duplicateMatches && (matchingGiftDetail != null))
                            {
                                // found a match
                                newMatchFound = true;
                                MarkTransactionMatched(AMainDS,
                                    GiftDetailByGiftTransactionNumberMatchStatus,
                                    transaction,
                                    matchingGiftDetail,
                                    matchingGiftDetail.GiftTransactionAmount != transaction.TransactionAmount);
                            }
                        }
                    }
                }
            }

            if (APostedBatch)
            {
                if (TLogging.DebugLevel > 0)
                {
                    TLogging.Log("before extra treatment of posted batch");
                }

                // do another loop, now looking even harder for matching gifts; match donor name, and recipient name with transaction description
                // by now the list of unassigned gifts from the old gift batch should be quite small
                for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
                {
                    BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                    if (stmtRow.MatchAction != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                    {
                        DataRowView[] filteredRows = GiftDetailByBatchNumberMatchStatus.FindRows(new object[] { ASelectedGiftBatch, false });

                        BankImportTDSAGiftDetailRow BestMatch = null;
                        int BestMatchNumber = 0;

                        foreach (DataRowView rv in filteredRows)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                            int matchNumber = MatchingWords(detailrow.DonorShortName, stmtRow.AccountName) +
                                              MatchingWords(detailrow.RecipientDescription, stmtRow.Description);

                            if (matchNumber > BestMatchNumber)
                            {
                                if (SumAmounts(GiftDetailByGiftTransactionNumber, ASelectedGiftBatch,
                                        detailrow.GiftTransactionNumber) == stmtRow.TransactionAmount)
                                {
                                    BestMatchNumber = matchNumber;
                                    BestMatch = detailrow;
                                }
                                else if (detailrow.GiftTransactionAmount == stmtRow.TransactionAmount)
                                {
                                    TLogging.Log("TODO: split gift " + " " + detailrow.GiftTransactionAmount.ToString() +
                                        " " + SumAmounts(GiftDetailByGiftTransactionNumber, ASelectedGiftBatch,
                                            detailrow.GiftTransactionNumber).ToString());
                                }
                            }
                        }

                        if (BestMatchNumber > 0)
                        {
                            newMatchFound = true;
                            MarkTransactionMatched(AMainDS,
                                GiftDetailByGiftTransactionNumberMatchStatus,
                                stmtRow,
                                BestMatch,
                                BestMatch.GiftTransactionAmount != stmtRow.TransactionAmount);
                        }
                    }
                }
            }

            if (TLogging.DebugLevel > 0)
            {
                TLogging.Log("end MatchTransactionsToGiftBatch");
            }

            return newMatchFound;
        }

        /// <summary>
        /// match text should uniquely identify a gift from a certain donor with a certain purpose;
        /// use account name, description, and amount;
        /// remove umlaut and spaces, because the banks sometimes play around with them
        /// </summary>
        public static string CalculateMatchText(string ABankAccount, AEpTransactionRow tr)
        {
            string matchtext = ABankAccount + tr.AccountName + tr.Description;

            matchtext += tr.TransactionAmount.ToString("0.##");

            matchtext = matchtext.Replace(",", "").Replace("/", "").Replace("-", "").Replace(";", "").Replace(".", "");
            matchtext = matchtext.Replace("PURP+RINPRATENZAHLUNG", "");
            matchtext = matchtext.Replace("SVWZ+", "");

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

        /// add new (or modified) matches
        private static Int32 CreateNewMatches(
            BankImportTDS AMatchDS,
            DataView AMatchView,
            DataRowView[] AGiftDetails,
            string AMatchText,
            SortedList <string, AEpMatchRow>AMatchesToAddLater)
        {
            AEpMatchRow newMatch = null;

            // create a match with the same matchtext for each gift detail (split gift)
            foreach (DataRowView gv in AGiftDetails)
            {
                BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;

                DataRowView[] FilteredMatches = AMatchView.FindRows(new object[] { AMatchText, giftRow.DetailNumber - 1 });

                if (FilteredMatches.Length == 0)
                {
                    // we might have added such a match for the current statement
                    string key = AMatchText + giftRow.DetailNumber.ToString();

                    if (AMatchesToAddLater.ContainsKey(key))
                    {
                        newMatch = AMatchesToAddLater[key];
                    }
                    else
                    {
                        newMatch = AMatchDS.AEpMatch.NewRowTyped();

                        // matchkey will be set properly on save, by sequence
                        newMatch.EpMatchKey = -1 * (AMatchDS.AEpMatch.Count + AMatchesToAddLater.Count + 1);
                        newMatch.MatchText = AMatchText;
                        AMatchesToAddLater.Add(key, newMatch);
                    }
                }
                else
                {
                    newMatch = (AEpMatchRow)FilteredMatches[0].Row;
                }

                newMatch.Detail = giftRow.DetailNumber - 1;
                newMatch.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;

                newMatch.RecipientKey = giftRow.RecipientKey;
                newMatch.RecipientLedgerNumber = giftRow.RecipientLedgerNumber;
                newMatch.LedgerNumber = giftRow.LedgerNumber;
                newMatch.DonorKey = giftRow.DonorKey;
                newMatch.DonorShortName = giftRow.DonorShortName;
                newMatch.RecipientShortName = giftRow.RecipientDescription;
                newMatch.MotivationGroupCode = giftRow.MotivationGroupCode;
                newMatch.MotivationDetailCode = giftRow.MotivationDetailCode;
                newMatch.GiftCommentOne = giftRow.GiftCommentOne;
                newMatch.GiftCommentTwo = giftRow.GiftCommentTwo;
                newMatch.GiftCommentThree = giftRow.GiftCommentThree;
                newMatch.CommentOneType = giftRow.CommentOneType;
                newMatch.CommentTwoType = giftRow.CommentTwoType;
                newMatch.CommentThreeType = giftRow.CommentThreeType;
                newMatch.MailingCode = giftRow.MailingCode;
                newMatch.CostCentreCode = giftRow.CostCentreCode;
                newMatch.ChargeFlag = giftRow.ChargeFlag;
                newMatch.ConfidentialGiftFlag = giftRow.ConfidentialGiftFlag;
                newMatch.GiftTransactionAmount = giftRow.GiftTransactionAmount;
            }

            return newMatch.EpMatchKey;
        }

        /// <summary>
        /// store historic Gift matches
        /// </summary>
        private static void StoreCurrentMatches(BankImportTDS AMatchDS,
            AEpStatementRow ACurrentStatement, Int32 ASelectedGiftBatch)
        {
            DataView MatchesByTextAndDetail = new DataView(
                AMatchDS.AEpMatch, string.Empty,
                AEpMatchTable.GetMatchTextDBName() + "," + AEpMatchTable.GetDetailDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailView = new DataView(
                AMatchDS.AGiftDetail, string.Empty,
                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            // for all matched FMainDS.AEpTransactions
            DataView MatchedTransactionsView = new DataView(AMatchDS.AEpTransaction,
                string.Empty,
                AEpTransactionTable.GetStatementKeyDBName() + "," +
                BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                DataViewRowState.CurrentRows);

            DataRowView[] matchedTransactions =
                MatchedTransactionsView.FindRows(new object[] {
                        ACurrentStatement.StatementKey,
                        Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED
                    });

            SortedList <string, AEpMatchRow>MatchesToAddLater = new SortedList <string, AEpMatchRow>();

            foreach (DataRowView rv in matchedTransactions)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(ACurrentStatement.BankAccountCode, tr);

                // get the gift details assigned to this transaction
                DataRowView[] FilteredGiftDetails =
                    GiftDetailView.FindRows(
                        new object[] {
                            tr.GiftBatchNumber,
                            tr.GiftTransactionNumber,
                            true
                        });

                // add new (or modified) matches
                tr.EpMatchKey = CreateNewMatches(AMatchDS, MatchesByTextAndDetail, FilteredGiftDetails, MatchText, MatchesToAddLater);
            }

            // for speed reasons, add the new rows after clearing the sort on the view
            MatchesByTextAndDetail.Sort = string.Empty;

            foreach (AEpMatchRow m in MatchesToAddLater.Values)
            {
                AMatchDS.AEpMatch.Rows.Add(m);
            }

            AMatchDS.PBankingDetails.Clear();
            AMatchDS.AGiftDetail.Clear();
            AMatchDS.AGift.Clear();

            AMatchDS.ThrowAwayAfterSubmitChanges = true;

            if (TLogging.DebugLevel > 0)
            {
                TLogging.Log("before submitchanges");
            }

            BankImportTDSAccess.SubmitChanges(AMatchDS);

            if (TLogging.DebugLevel > 0)
            {
                TLogging.Log("after submitchanges");
            }
        }
    }
}