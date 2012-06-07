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
using System.Security.Cryptography;
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
        /// <summary>
        /// train with imported bank statements and existing gift batches
        /// </summary>
        public static void Train(BankImportTDS AMainDS)
        {
            int stmtCounter = 0;

            // go through all statements in the dataset, and find gift matches for those days
            foreach (AEpStatementRow stmt in AMainDS.AEpStatement.Rows)
            {
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    String.Format(Catalog.GetString("training statement {0}"),
                        stmt.Filename),
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

                Int32 SelectedGiftBatch = -1;
                // simple matching; no split gifts, bank account number fits and amount fits
                // problem: recipient different????
                Int32 CountMatches = 0;

                // Get all gifts at given date
                GetGiftsByDate(stmt.LedgerNumber, AMainDS, stmt.Date);

                AMainDS.AEpTransaction.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        AEpTransactionTable.GetStatementKeyDBName(),
                        stmt.StatementKey);

                foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
                {
                    BankImportTDSAEpTransactionRow transaction = (BankImportTDSAEpTransactionRow)rv.Row;

                    // find the donor for this transaction, by his bank account number
                    Int64 DonorKey = GetDonorByBankAccountNumber(AMainDS, transaction.BankAccountNumber);

                    if (DonorKey == -1)
                    {
                        continue;
                    }

                    AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                transaction.TransactionAmount.ToString(
                        System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString();

                    if (AMainDS.AGiftDetail.DefaultView.Count == 1)
                    {
                        // found a possible match
                        CountMatches++;
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row;
                        SelectedGiftBatch = detailrow.BatchNumber;

                        // we have found exactly one gift detail which matches the donor and the amount and the date
                        // but it might be that the donation was for a different recipient
                        // do not mark matched here yet
                        //MarkTransactionMatched(ref AMainDS, ref stmtRow, detailrow);
                    }
                }

                if ((SelectedGiftBatch == -1)
                    || ((AMainDS.AEpTransaction.DefaultView.Count > 2) && (CountMatches < AMainDS.AEpTransaction.DefaultView.Count / 2)))
                {
                    // continue to next statement. we cannot find the right gift batch
                    continue;
                }

                AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                            SelectedGiftBatch.ToString();

                bool postedBatch = ((BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row).BatchStatus == "Posted";

                while (MatchTransactionsToGiftBatch(AMainDS, SelectedGiftBatch, postedBatch))
                {
                }

                // this is needed when calling CalculateMatchText in StoreCurrentMatches
                AMainDS.AEpStatement.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        AEpStatementTable.GetStatementKeyDBName(),
                        stmt.StatementKey);

                StoreCurrentMatches(AMainDS, SelectedGiftBatch);
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
            parameters[0] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[0].Value = ADateEffective;
            parameters[1] = new OdbcParameter("ALedgerNumber", OdbcType.Int);
            parameters[1].Value = ALedgerNumber;
            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.AGiftDetail.TableName, transaction, parameters);

            // get PartnerKey and banking details (most important BankAccountNumber) for all donations on the given date
            stmt = TDataBase.ReadSqlFile("BankImport.GetBankAccountByDate.sql");
            parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[0].Value = ADateEffective;

            // There can be several donors with the same banking details
            AMainDS.PBankingDetails.Constraints.Clear();

            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.PBankingDetails.TableName, transaction, parameters);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return true;
        }

        private static Int64 GetDonorByBankAccountNumber(BankImportTDS AMainDS, string ABankAccountNumber)
        {
            // TODO: what about bank sorting code? would make query more difficult, because the bank code is not directly in p_banking_details
            AMainDS.PBankingDetails.DefaultView.RowFilter = BankImportTDSPBankingDetailsTable.GetBankAccountNumberDBName() +
                                                            " = '" + ABankAccountNumber + "'";

            if (AMainDS.PBankingDetails.DefaultView.Count > 0)
            {
                // TODO: just return the first partner key; usually not 2 people owning the same bank account donate at the same time???
                BankImportTDSPBankingDetailsRow row = (BankImportTDSPBankingDetailsRow)AMainDS.PBankingDetails.DefaultView[0].Row;
                return row.PartnerKey;
            }

            return -1;
        }

        private static void MarkTransactionMatched(BankImportTDS AMainDS,
            ref BankImportTDSAEpTransactionRow transactionRow,
            BankImportTDSAGiftDetailRow giftDetail, bool AMatchAllGiftDetails)
        {
            if (AMatchAllGiftDetails)
            {
                // recursive call, for each gift detail that is matched by this transaction
                AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                            giftDetail.BatchNumber.ToString() +
                                                            " AND " + BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " = " +
                                                            giftDetail.GiftTransactionNumber.ToString() +
                                                            " AND AlreadyMatched = false";

                foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
                {
                    MarkTransactionMatched(AMainDS, ref transactionRow, (BankImportTDSAGiftDetailRow)rv.Row, false);
                }

                return;
            }

            giftDetail.AlreadyMatched = true;

            if (giftDetail.RecipientDescription.Length == 0)
            {
                giftDetail.RecipientDescription = giftDetail.MotivationGroupCode + "/" + giftDetail.MotivationDetailCode;
            }

            if (Convert.ToDecimal(transactionRow.TransactionAmount) == Convert.ToDecimal(giftDetail.GiftTransactionAmount))
            {
                // gift detail matches the whole transaction (or what is left of it)
                transactionRow.MatchAction = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                transactionRow.GiftLedgerNumber = giftDetail.LedgerNumber;
                transactionRow.GiftBatchNumber = giftDetail.BatchNumber;
                transactionRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
                transactionRow.GiftDetailNumber = giftDetail.DetailNumber;
                transactionRow.DonorKey = giftDetail.DonorKey;
            }
            else
            {
                // only parts of the bank transaction are matched by this gift detail
                // create a new transaction, and split the amounts
                BankImportTDSAEpTransactionRow newRow = AMainDS.AEpTransaction.NewRowTyped();
                newRow.StatementKey = transactionRow.StatementKey;
                newRow.Order = transactionRow.Order;
                newRow.NumberOnPaperStatement = transactionRow.NumberOnPaperStatement;
                newRow.DetailKey = giftDetail.DetailNumber;
                newRow.AccountName = transactionRow.AccountName;
                newRow.BankAccountNumber = transactionRow.BankAccountNumber;
                newRow.BranchCode = transactionRow.BranchCode;
                newRow.Description = transactionRow.Description;
                newRow.TransactionTypeCode = transactionRow.TransactionTypeCode;
                newRow.MatchAction = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                newRow.DonorKey = giftDetail.DonorKey;
                newRow.GiftLedgerNumber = giftDetail.LedgerNumber;
                newRow.GiftBatchNumber = giftDetail.BatchNumber;
                newRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
                newRow.GiftDetailNumber = giftDetail.DetailNumber;
                newRow.TransactionAmount = giftDetail.GiftTransactionAmount;

                if (transactionRow.IsOriginalAmountOnStatementNull())
                {
                    transactionRow.OriginalAmountOnStatement = transactionRow.TransactionAmount;
                }

                newRow.OriginalAmountOnStatement = transactionRow.OriginalAmountOnStatement;
                transactionRow.TransactionAmount -= newRow.TransactionAmount;

                AMainDS.AEpTransaction.Rows.Add(newRow);
            }
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

        private static Decimal SumAmounts(BankImportTDS AMainDS, Int32 ASelectedGiftBatch, Int32 AGiftTransactionNumber, bool ACheckUnmatchedOnly)
        {
            Decimal Result = 0.0m;

            DataView v = new DataView(AMainDS.AGiftDetail);

            v.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + " = " + ASelectedGiftBatch.ToString() +
                          " AND " + AGiftDetailTable.GetGiftTransactionNumberDBName() + " = " + AGiftTransactionNumber.ToString();

            // if not ACheckUnmatchedOnly: sum all gift details, both unmatched and match
            if (ACheckUnmatchedOnly)
            {
                v.RowFilter += " AND " + BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName() + "= false";
            }

            foreach (DataRowView rv in v)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                Result += Convert.ToDecimal(detailrow.GiftTransactionAmount);
            }

            return Result;
        }

        /// <summary>
        /// match imported transactions from bank statement to an existing gift batch
        /// </summary>
        /// <returns>true while new matches are found</returns>
        private static bool MatchTransactionsToGiftBatch(BankImportTDS AMainDS, Int32 ASelectedGiftBatch, bool APostedBatch)
        {
            bool newMatchFound = false;

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow transaction = (BankImportTDSAEpTransactionRow)rv.Row;

                if (transaction.MatchAction != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    // problem: what if bank account is used by several donors?
                    Int64 DonorKey = GetDonorByBankAccountNumber(AMainDS, transaction.BankAccountNumber);

                    // look for gifts that match the donor (identified by account number) and the transaction amount
                    AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                transaction.TransactionAmount.ToString(
                        System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString() + " AND " +
                                                                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                ASelectedGiftBatch.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (AMainDS.AGiftDetail.DefaultView.Count == 1)
                    {
                        // found exactly one match
                        newMatchFound = true;
                        MarkTransactionMatched(AMainDS, ref transaction, (BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row, false);
                    }
                    else if (AMainDS.AGiftDetail.DefaultView.Count > 1)
                    {
                        // donor has several gifts with same amount?
                        // look for fitting words in description
                        int MaxCount = -1;
                        BankImportTDSAGiftDetailRow MaxRow = null;

                        foreach (DataRowView rv2 in AMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv2.Row;

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
                            MarkTransactionMatched(AMainDS, ref transaction, MaxRow, false);
                        }
                    }
                    else if (AMainDS.AGiftDetail.DefaultView.Count == 0)
                    {
                        // split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount

                        // get all gifts with that bank account number
                        DonorKey = GetDonorByBankAccountNumber(AMainDS, transaction.BankAccountNumber);

                        AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                    DonorKey.ToString() + " AND " +
                                                                    BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    ASelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";

                        if (AMainDS.AGiftDetail.DefaultView.Count > 1)
                        {
                            BankImportTDSAGiftDetailRow matchingGiftDetail = null;
                            bool duplicateMatches = false;

                            foreach (DataRowView rv2 in AMainDS.AGiftDetail.DefaultView)
                            {
                                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv2.Row;

                                if ((matchingGiftDetail == null) || (detailrow.GiftTransactionNumber != matchingGiftDetail.GiftTransactionNumber))
                                {
                                    if ((SumAmounts(AMainDS, ASelectedGiftBatch,
                                             detailrow.GiftTransactionNumber, true) == Convert.ToDecimal(transaction.TransactionAmount))
                                        || (Convert.ToDecimal(detailrow.GiftTransactionAmount) == Convert.ToDecimal(transaction.TransactionAmount)))
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
                                    ref transaction,
                                    matchingGiftDetail,
                                    Convert.ToDecimal(matchingGiftDetail.GiftTransactionAmount) != Convert.ToDecimal(transaction.TransactionAmount));
                            }
                        }
                    }
                }
            }

            if (APostedBatch)
            {
                // do another loop, now looking even harder for matching gifts; match donor name, and recipient name with transaction description
                // by now the list of unassigned gifts from the old gift batch should be quite small
                for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
                {
                    BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                    if (stmtRow.MatchAction != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                    {
                        AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    ASelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";
                        BankImportTDSAGiftDetailRow BestMatch = null;
                        int BestMatchNumber = 0;

                        foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                            int matchNumber = MatchingWords(detailrow.DonorShortName, stmtRow.AccountName) +
                                              MatchingWords(detailrow.RecipientDescription, stmtRow.Description);

                            if ((matchNumber > BestMatchNumber)
                                && ((SumAmounts(AMainDS, ASelectedGiftBatch,
                                         detailrow.GiftTransactionNumber, true) == Convert.ToDecimal(stmtRow.TransactionAmount))
                                    || (Convert.ToDecimal(detailrow.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount))))
                            {
                                BestMatchNumber = matchNumber;
                                BestMatch = detailrow;
                            }
                        }

                        if (BestMatchNumber > 0)
                        {
                            newMatchFound = true;
                            MarkTransactionMatched(AMainDS,
                                ref stmtRow,
                                BestMatch,
                                Convert.ToDecimal(BestMatch.GiftTransactionAmount) != Convert.ToDecimal(stmtRow.TransactionAmount));
                        }
                    }
                }
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
        /// return the filter for GiftDetail to show all the gifts associated with the given transaction;
        /// this is useful after a bank statement has been matched against an imported gift batch, to train the system
        /// </summary>
        /// <returns>the filter; empty string if no transactions selected</returns>
        private static string FilterForMatchedGiftTransactions(BankImportTDSAEpTransactionRow ATransactionRow, Int32 ASelectedGiftBatch)
        {
            if (ATransactionRow.IsGiftTransactionNumberNull())
            {
                return String.Empty;
            }

            string Filter = BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " = " +
                            ATransactionRow.GiftTransactionNumber;

            if (ATransactionRow.GiftDetailNumber != -1)
            {
                Filter += " AND " + BankImportTDSAGiftDetailTable.GetDetailNumberDBName() + " = " +
                          ATransactionRow.GiftDetailNumber;
            }

            return BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                   ASelectedGiftBatch.ToString() +
                   " AND AlreadyMatched = true " +
                   " AND " + Filter;
        }

        /// add new (or modified) matches
        private static void CreateNewMatches(BankImportTDS AMainDS, BankImportTDS AMatchDS, string AMatchText)
        {
            // create a match with the same matchtext for each gift detail (split gift)
            foreach (DataRowView gv in AMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;

                AEpMatchRow newMatch;

                AMatchDS.AEpMatch.DefaultView.RowFilter = AEpMatchTable.GetMatchTextDBName() + " = '" + AMatchText + "' and " +
                                                          AEpMatchTable.GetDetailDBName() + " = " + giftRow.DetailNumber.ToString();

                if (AMatchDS.AEpMatch.DefaultView.Count == 0)
                {
                    newMatch = AMatchDS.AEpMatch.NewRowTyped();

                    // matchkey will be set properly on save, by sequence
                    newMatch.EpMatchKey = -1 * (AMatchDS.AEpMatch.Count + 1);
                    newMatch.MatchText = AMatchText;
                    AMatchDS.AEpMatch.Rows.Add(newMatch);
                }
                else
                {
                    newMatch = (AEpMatchRow)AMatchDS.AEpMatch.DefaultView[0].Row;
                }

                newMatch.Detail = giftRow.DetailNumber;
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

                AMatchDS.AEpMatch.DefaultView.RowFilter = "";
            }
        }

        /// <summary>
        /// store historic Gift matches
        /// </summary>
        private static void StoreCurrentMatches(BankImportTDS AMainDS, Int32 ASelectedGiftBatch)
        {
            TDBTransaction dbtransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            // for all matched FMainDS.AEpTransactions
            AMainDS.AEpTransaction.DefaultView.RowFilter += " AND " + BankImportTDSAEpTransactionTable.GetMatchActionDBName() + " = '" +
                                                            Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";

            BankImportTDS MatchDS = new BankImportTDS();

            // TODO: would it help not to load all?
            AEpMatchAccess.LoadAll(MatchDS, dbtransaction);

            MatchDS.AEpMatch.AcceptChanges();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // get the gift details assigned to this transaction
                AMainDS.AGiftDetail.DefaultView.RowFilter = FilterForMatchedGiftTransactions(tr, ASelectedGiftBatch);

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(((AEpStatementRow)AMainDS.AEpStatement.DefaultView[0].Row).BankAccountCode, tr);

                // add new (or modified) matches
                CreateNewMatches(AMainDS, MatchDS, MatchText);

                tr.EpMatchKey = ((AEpMatchRow)MatchDS.AEpMatch.DefaultView[0].Row).EpMatchKey;
            }

            TVerificationResultCollection Verification;
            AEpMatchAccess.SubmitChanges(MatchDS.AEpMatch.GetChangesTyped(), dbtransaction, out Verification);
            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }
}