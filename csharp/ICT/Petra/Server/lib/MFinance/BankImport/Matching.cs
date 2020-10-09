// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2004-2020 by OM International
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
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.BankImport.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// train the matching for bank import, by comparing existing batches with old bank statements
    /// </summary>
    public class TBankImportMatching
    {
        /// <summary>
        /// train with imported bank statements and existing gift batches
        /// </summary>
        public static void Train(AEpStatementTable AStatements)
        {
            int stmtCounter = 0;

            // go through all statements in the dataset, and find gift matches for those days
            foreach (AEpStatementRow stmt in AStatements.Rows)
            {
                TLogging.LogAtLevel(1,
                    "Training Statement " + stmt.StatementKey.ToString() + " " + stmt.Date.ToShortDateString() + " " + stmt.Filename);

                stmtCounter++;

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    return;
                }

                // first stage: collect historic matches from database:
                // go through each transaction of the statement,
                // and see if you can find a donation on that date with the same amount from the same bank account
                // store this as a match

                TLogging.LogAtLevel(1, "loading data ...");
                BankImportTDS MainDS = LoadData(stmt.LedgerNumber, stmt.StatementKey);

                // Get all gifts at given date
                TLogging.LogAtLevel(1, "get gifts by date ...");
                List <int>GiftBatchNumbers;
                GetGiftsByDate(stmt.LedgerNumber, MainDS, stmt.Date, stmt.BankAccountCode, out GiftBatchNumbers);

                int SelectedGiftBatch = -1;

                if (GiftBatchNumbers.Count > 0)
                {
                    TLogging.LogAtLevel(1, "Found gift batches: " + GiftBatchNumbers.Count.ToString());

                    foreach (int i in GiftBatchNumbers)
                    {
                        TLogging.LogAtLevel(1, "   " + i.ToString());
                    }

                    SelectedGiftBatch = FindGiftBatch(MainDS, stmt);
                    TLogging.LogAtLevel(1, " selected gift batch:   " + SelectedGiftBatch.ToString());
                }

                if (SelectedGiftBatch == -1)
                {
                    // cannot find the posted gift batch without any doubt
                    continue;
                }

                CreateMatches(MainDS, stmt, SelectedGiftBatch, true);
            }
        }

        /// <summary>
        /// there are several gift batches that might fit this bank statement. find the right one!
        /// simple matching; no split gifts, bank account number fits and amount fits
        /// </summary>
        private static int FindGiftBatch(BankImportTDS AMainDS, AEpStatementRow AStmt)
        {
            SortedList <Int32, Int32>MatchedGiftBatches = new SortedList <int, int>();

            // create the dataview only after loading, otherwise loading is much slower
            DataView GiftDetailByAmountAndDonor = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                AGiftDetailTable.GetGiftAmountDBName() + "," +
                BankImportTDSAGiftDetailTable.GetDonorKeyDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftByAmountAndDonor = new DataView(AMainDS.AGift,
                string.Empty,
                BankImportTDSAGiftTable.GetTotalAmountDBName() + "," +
                AGiftTable.GetDonorKeyDBName(),
                DataViewRowState.CurrentRows);

            AMainDS.PBankingDetails.DefaultView.Sort = BankImportTDSPBankingDetailsTable.GetBankSortCodeDBName() + "," +
                                                       BankImportTDSPBankingDetailsTable.GetBankAccountNumberDBName();

            foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
            {
                // find the donor for this transaction, by his bank account number
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

                BankImportTDSAGiftDetailRow detailrow = null;

                if (DonorKey != -1)
                {
                    DataRowView[] giftDetails = GiftDetailByAmountAndDonor.FindRows(new object[] { transaction.TransactionAmount, DonorKey });

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
                                (BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.Rows.Find(new object[] { gift.LedgerNumber, gift.BatchNumber,
                                                                                                          gift.GiftTransactionNumber,
                                                                                                          1 });
                        }
                    }
                }

                if (detailrow != null)
                {
                    if (MatchedGiftBatches.ContainsKey(detailrow.BatchNumber))
                    {
                        MatchedGiftBatches[detailrow.BatchNumber]++;
                    }
                    else
                    {
                        MatchedGiftBatches.Add(detailrow.BatchNumber, 1);
                    }
                }
            }

            int SelectedGiftBatch = -1;
            int maxMatches = 0;

            foreach (int GiftBatchNumber in MatchedGiftBatches.Keys)
            {
                if (MatchedGiftBatches[GiftBatchNumber] > maxMatches)
                {
                    maxMatches = MatchedGiftBatches[GiftBatchNumber];
                    SelectedGiftBatch = GiftBatchNumber;
                }
            }

            if ((SelectedGiftBatch != -1)
                && ((AMainDS.AEpTransaction.Rows.Count > 2)
                    && (MatchedGiftBatches[SelectedGiftBatch] < AMainDS.AEpTransaction.Rows.Count / 2)))
            {
                TLogging.Log(
                    "cannot find enough gifts that look the same, for statement " + AStmt.Filename +
                    ". CountMatches for batch " + SelectedGiftBatch.ToString() + ": " +
                    MatchedGiftBatches[SelectedGiftBatch].ToString());

                SelectedGiftBatch = -1;
            }

            return SelectedGiftBatch;
        }

        /// <summary>
        /// return a table with gift details for the given date with donor partner keys and bank account numbers
        /// </summary>
        private static bool GetGiftsByDate(Int32 ALedgerNumber,
            BankImportTDS AMainDS,
            DateTime ADateEffective,
            string ABankAccountCode,
            out List <int>AGiftBatchNumbers)
        {
            TDataBase db = DBAccess.Connect("GetGiftsByDate");
            TDBTransaction transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted);

            // first get all gifts, even those that have no bank account associated
            string stmt = TDataBase.ReadSqlFile("BankImport.GetDonationsByDate.sql");

            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("ALedgerNumber", OdbcType.Int);
            parameters[0].Value = ALedgerNumber;
            parameters[1] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[1].Value = ADateEffective;
            parameters[2] = new OdbcParameter("ABankAccountCode", OdbcType.VarChar);
            parameters[2].Value = ABankAccountCode;

            db.SelectDT(AMainDS.AGiftDetail, stmt, transaction, parameters, 0, 0);

            // calculate the totals of gifts
            AMainDS.AGift.Clear();

            AGiftBatchNumbers = new List <int>();

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

                if (!AGiftBatchNumbers.Contains(giftRow.BatchNumber))
                {
                    AGiftBatchNumbers.Add(giftRow.BatchNumber);
                }
            }

            // get PartnerKey and banking details (most important BankAccountNumber) for all donations on the given date
            stmt = TDataBase.ReadSqlFile("BankImport.GetBankAccountByDate.sql");
            parameters = new OdbcParameter[2];
            parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
            parameters[0].Value = ALedgerNumber;
            parameters[1] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[1].Value = ADateEffective;
            // TODO ? parameters[2] = new OdbcParameter("ABankAccountCode", OdbcType.VarChar);
            //parameters[2].Value = ABankAccountCode;

            // There can be several donors with the same banking details
            AMainDS.PBankingDetails.Constraints.Clear();

            db.Select(AMainDS, stmt, AMainDS.PBankingDetails.TableName, transaction, parameters);
            transaction.Rollback();

            return true;
        }

        private static Int64 GetDonorByBankAccountNumber(BankImportTDS AMainDS, string ABankSortCode, string ABankAccountNumber)
        {
            if (Regex.IsMatch(ABankAccountNumber, "^[A-Z]") && (ABankAccountNumber.Length > 2) && (ABankAccountNumber.Substring(0,2) == "DE"))
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
                if (bankingDetails.Length > 1)
                {
                    TLogging.Log("Warning: 2 people own the same bank account " + ABankSortCode + " " + ABankAccountNumber);
                }

                // TODO: just return the first partner key; usually not 2 people owning the same bank account donate at the same time???
                BankImportTDSPBankingDetailsRow row = (BankImportTDSPBankingDetailsRow)bankingDetails[0].Row;
                return row.PartnerKey;
            }

            return -1;
        }

        private static void FindDonorKeysByBankAccount(BankImportTDS AMainDS)
        {
            AMainDS.PBankingDetails.DefaultView.Sort = BankImportTDSPBankingDetailsTable.GetBankSortCodeDBName() + "," +
                                                       BankImportTDSPBankingDetailsTable.GetBankAccountNumberDBName();

            foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
            {
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

                transaction.DonorKey = DonorKey;
            }
        }

        private static BankImportTDS LoadData(Int32 ALedgerNumber, Int32 AStatementKey)
        {
            TDataBase db = DBAccess.Connect("LoadData");
            TDBTransaction dbtransaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            BankImportTDS MatchDS = new BankImportTDS();

            // TODO: would it help not to load all? use a_recent_match_d?
            AEpMatchAccess.LoadViaALedger(MatchDS, ALedgerNumber, dbtransaction);

            TLogging.LogAtLevel(1, "loaded " + MatchDS.AEpMatch.Rows.Count.ToString() + " a_ep_match rows");

            AEpTransactionAccess.LoadViaAEpStatement(MatchDS, AStatementKey, dbtransaction);

            dbtransaction.Rollback();

            MatchDS.AEpMatch.AcceptChanges();
            MatchDS.AEpTransaction.AcceptChanges();

            return MatchDS;
        }

        private static void CreateMatches(BankImportTDS AMainDS,
            AEpStatementRow ACurrentStatement,
            Int32 ASelectedGiftBatch, bool APostedBatch)
        {
            // remove all gifts and giftdetails that don't belong to the selected batch
            List <DataRow>ToDelete = new List <DataRow>();

            foreach (AGiftDetailRow giftdetail in AMainDS.AGiftDetail.Rows)
            {
                if (giftdetail.BatchNumber != ASelectedGiftBatch)
                {
                    ToDelete.Add(giftdetail);
                }
            }

            foreach (DataRow del in ToDelete)
            {
                AMainDS.AGiftDetail.Rows.Remove(del);
            }

            ToDelete = new List <DataRow>();

            foreach (AGiftRow gift in AMainDS.AGift.Rows)
            {
                if (gift.BatchNumber != ASelectedGiftBatch)
                {
                    ToDelete.Add(gift);
                }
            }

            foreach (DataRow del in ToDelete)
            {
                AMainDS.AGift.Rows.Remove(del);
            }

            ToDelete = new List <DataRow>();

            foreach (BankImportTDSAEpTransactionRow transaction  in AMainDS.AEpTransaction.Rows)
            {
                // delete transactions with negative amount
                if (transaction.TransactionAmount < 0)
                {
                    ToDelete.Add(transaction);
                }
                else
                {
                    transaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                }
            }

            foreach (DataRow del in ToDelete)
            {
                AMainDS.AEpTransaction.Rows.Remove(del);
            }

            FindDonorKeysByBankAccount(AMainDS);

            AMainDS.PBankingDetails.DefaultView.Sort = BankImportTDSPBankingDetailsTable.GetPartnerKeyDBName();

            MatchDonorsWithKnownBankaccount(AMainDS);

            while (MatchTransactionsToGiftBatch(AMainDS))
            {
                ;
            }

            if (TLogging.DebugLevel > 0)
            {
                TLogging.Log("transactions not matched yet:");

                foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
                {
                    if (transaction.MatchAction != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                    {
                        TLogging.Log(
                            "  " + transaction.DonorKey.ToString() + " " + transaction.AccountName + " --- " + transaction.Description + " " +
                            transaction.TransactionAmount.ToString());

                        if (transaction.DonorKey == -1)
                        {
                            TLogging.Log("     " + transaction.BankAccountNumber + " " + transaction.BranchCode);
                        }
                    }
                }

                TLogging.Log("gifts not matched yet:");

                foreach (BankImportTDSAGiftDetailRow giftdetail in AMainDS.AGiftDetail.Rows)
                {
                    if (!giftdetail.AlreadyMatched)
                    {
                        string HasBankDetails = "-";
                        int BankDetailsIndex = AMainDS.PBankingDetails.DefaultView.Find(giftdetail.DonorKey);

                        if (BankDetailsIndex != -1)
                        {
                            HasBankDetails = "*";
                        }

                        TLogging.Log(
                            "  " + HasBankDetails + " " + giftdetail.DonorKey.ToString() + " " + giftdetail.DonorShortName + " --- " +
                            giftdetail.RecipientDescription + " " + giftdetail.GiftAmount.ToString());

                        if (BankDetailsIndex != -1)
                        {
                            BankImportTDSPBankingDetailsRow bankdetail =
                                (BankImportTDSPBankingDetailsRow)AMainDS.PBankingDetails.DefaultView[BankDetailsIndex].Row;
                            TLogging.Log("     " + bankdetail.BankAccountNumber + " " + bankdetail.BankSortCode);
                        }
                    }
                }

                int CountMatched = 0;

                foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
                {
                    if (transaction.MatchAction == Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                    {
                        CountMatched++;
                    }
                }

                TLogging.Log("matched: " + CountMatched.ToString() + " of " + AMainDS.AEpTransaction.Rows.Count.ToString());
            }

            StoreCurrentMatches(AMainDS, ACurrentStatement.BankAccountCode);
        }

        private static void MarkTransactionMatched(
            BankImportTDS AMainDS,
            BankImportTDSAEpTransactionRow transactionRow,
            BankImportTDSAGiftDetailRow giftDetail)
        {
            giftDetail.AlreadyMatched = true;

            if (giftDetail.RecipientDescription.Length == 0)
            {
                giftDetail.RecipientDescription = giftDetail.MotivationGroupCode + "/" + giftDetail.MotivationDetailCode;
            }

            transactionRow.MatchAction = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
            transactionRow.GiftLedgerNumber = giftDetail.LedgerNumber;
            transactionRow.GiftBatchNumber = giftDetail.BatchNumber;
            transactionRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
            transactionRow.GiftDetailNumbers = StringHelper.AddCSV(transactionRow.GiftDetailNumbers, giftDetail.DetailNumber.ToString(), ",");
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

        private static Decimal SumAmounts(DataView AGiftDetailViewByTransactionNumber,
            Int32 AGiftTransactionNumber)
        {
            Decimal Result = 0.0m;

            DataRowView[] detailsOfGift = AGiftDetailViewByTransactionNumber.FindRows(
                new object[] { AGiftTransactionNumber });

            foreach (DataRowView rv in detailsOfGift)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                Result += detailrow.GiftTransactionAmount;
            }

            return Result;
        }

        private static bool MatchOneDonor(BankImportTDS AMainDS, DataRowView[] AGiftDetailWithoutAmount, DataRowView[] ATransactionsByDonor)
        {
            // check that the total amount matches
            Decimal TotalAmountStatement = 0.0m;
            Decimal TotalAmountGiftBatch = 0.0m;

            foreach (DataRowView rv in ATransactionsByDonor)
            {
                BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;
                TotalAmountStatement += trRow.TransactionAmount;
            }

            foreach (DataRowView rv in AGiftDetailWithoutAmount)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                TotalAmountGiftBatch += detailrow.GiftAmount;
            }

            if (TotalAmountGiftBatch != TotalAmountStatement)
            {
                TLogging.Log("Strange situation, amounts do not match:");

                foreach (DataRowView rv in AGiftDetailWithoutAmount)
                {
                    BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                    TLogging.Log(
                        " gift detail: " + detailrow.DonorShortName + " " + detailrow.RecipientDescription + " " + detailrow.GiftAmount.ToString());
                }

                foreach (DataRowView rv in ATransactionsByDonor)
                {
                    BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;
                    TLogging.Log(" transaction: " + trRow.AccountName + " " + trRow.Description + " " + trRow.TransactionAmount.ToString());
                }

                return false;
            }

            bool debug = false;

            foreach (DataRowView rv in AGiftDetailWithoutAmount)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                if (detailrow.DonorShortName.Contains("David"))
                {
                    debug = false;
                }
            }

            if (debug)
            {
                TLogging.Log("does this match? ");

                foreach (DataRowView rv in AGiftDetailWithoutAmount)
                {
                    BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                    TLogging.Log(
                        " gift detail: " + detailrow.DonorShortName + " " + detailrow.RecipientDescription + " " + detailrow.GiftAmount.ToString());
                }

                foreach (DataRowView rv in ATransactionsByDonor)
                {
                    BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;
                    TLogging.Log(" transaction: " + trRow.AccountName + " " + trRow.Description + " " + trRow.TransactionAmount.ToString());
                }
            }

            if ((AGiftDetailWithoutAmount.Length == 1) && (ATransactionsByDonor.Length == 1))
            {
                // found exactly one match
                MarkTransactionMatched(AMainDS,
                    (BankImportTDSAEpTransactionRow)ATransactionsByDonor[0].Row,
                    (BankImportTDSAGiftDetailRow)AGiftDetailWithoutAmount[0].Row);

                return true;
            }
            else if (AGiftDetailWithoutAmount.Length == ATransactionsByDonor.Length)
            {
                bool matched = false;

                // there is one bank transaction for each gift detail,
                // or two bank transactions that go into one gift with 2 details;
                // check for amount, and matching words
                foreach (DataRowView rv in ATransactionsByDonor)
                {
                    int maxMatchingWords = -1;
                    bool duplicate = false;
                    BankImportTDSAGiftDetailRow BestMatch = null;

                    BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;

                    foreach (DataRowView rv2 in AGiftDetailWithoutAmount)
                    {
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv2.Row;

                        if ((detailrow.GiftAmount == trRow.TransactionAmount) && !detailrow.AlreadyMatched)
                        {
                            int matchNumber = MatchingWords(detailrow.RecipientDescription, trRow.Description);

                            if (matchNumber > 0)
                            {
                                if (matchNumber == maxMatchingWords)
                                {
                                    duplicate = true;
                                }
                                else if (matchNumber > maxMatchingWords)
                                {
                                    maxMatchingWords = matchNumber;
                                    duplicate = false;
                                    BestMatch = detailrow;
                                }
                            }
                        }
                    }

                    if ((BestMatch != null) && !duplicate)
                    {
                        MarkTransactionMatched(AMainDS, trRow, BestMatch);
                        matched = true;
                    }
                }

                if (matched)
                {
                    return true;
                }
            }
            else if (ATransactionsByDonor.Length == 1)
            {
                // one bank transactions with split gifts
                foreach (DataRowView rv in AGiftDetailWithoutAmount)
                {
                    BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                    MarkTransactionMatched(AMainDS,
                        (BankImportTDSAEpTransactionRow)ATransactionsByDonor[0].Row,
                        detailrow);
                }

                return true;
            }
            else if (AGiftDetailWithoutAmount.Length == 1)
            {
                // 3 bank transactions have been merged into one split gift (very special case... 1 Euro per day...)
                foreach (DataRowView rv in ATransactionsByDonor)
                {
                    BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;

                    MarkTransactionMatched(AMainDS,
                        trRow,
                        (BankImportTDSAGiftDetailRow)AGiftDetailWithoutAmount[0].Row);
                }

                return true;
            }
            else
            {
                TLogging.Log("TODO: several split gifts, for multiple transactions");

                foreach (DataRowView rv in AGiftDetailWithoutAmount)
                {
                    BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                    TLogging.Log(
                        " gift detail: " + detailrow.DonorShortName + " " + detailrow.RecipientDescription + " " + detailrow.GiftAmount.ToString());
                }

                foreach (DataRowView rv in ATransactionsByDonor)
                {
                    BankImportTDSAEpTransactionRow trRow = (BankImportTDSAEpTransactionRow)rv.Row;
                    TLogging.Log(" transaction: " + trRow.AccountName + " " + trRow.Description + " " + trRow.TransactionAmount.ToString());
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// match imported transactions from bank statement to an existing gift batch;
        /// this method is only for donors that can be identified by their bank account
        /// </summary>
        private static void MatchDonorsWithKnownBankaccount(BankImportTDS AMainDS)
        {
            DataView GiftDetailWithoutAmountView = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView TransactionsByDonorView = new DataView(AMainDS.AEpTransaction,
                string.Empty,
                BankImportTDSAEpTransactionTable.GetDonorKeyDBName() + "," +
                BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                DataViewRowState.CurrentRows);

            foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
            {
                if ((transaction.DonorKey != -1)
                    && (transaction.MatchAction == MFinanceConstants.BANK_STMT_STATUS_UNMATCHED))
                {
                    // get all gifts of this donor, and all bank statement transactions
                    DataRowView[] GiftDetailWithoutAmount = GiftDetailWithoutAmountView.FindRows(
                        new object[] { transaction.DonorKey, false });

                    DataRowView[] TransactionsByDonor = TransactionsByDonorView.FindRows(
                        new object[] { transaction.DonorKey, MFinanceConstants.BANK_STMT_STATUS_UNMATCHED });

                    while (MatchOneDonor(AMainDS, GiftDetailWithoutAmount, TransactionsByDonor))
                    {
                        GiftDetailWithoutAmount = GiftDetailWithoutAmountView.FindRows(
                            new object[] { transaction.DonorKey, false });

                        TransactionsByDonor = TransactionsByDonorView.FindRows(
                            new object[] { transaction.DonorKey, MFinanceConstants.BANK_STMT_STATUS_UNMATCHED });
                    }
                }
            }
        }

        /// <summary>
        /// match imported transactions from bank statement to an existing gift batch
        /// </summary>
        /// <returns>true while new matches are found</returns>
        private static bool MatchTransactionsToGiftBatch(BankImportTDS AMainDS)
        {
            bool newMatchFound = false;

            DataView GiftDetailWithoutAmountView = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + "," +
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView GiftDetailByBatchNumberMatchStatus = new DataView(AMainDS.AGiftDetail,
                string.Empty,
                BankImportTDSAGiftDetailTable.GetAlreadyMatchedDBName(),
                DataViewRowState.CurrentRows);

            DataView TransactionsByBankAccountView = new DataView(AMainDS.AEpTransaction,
                string.Empty,
                BankImportTDSAEpTransactionTable.GetBankAccountNumberDBName() + "," +
                BankImportTDSAEpTransactionTable.GetBranchCodeDBName() + "," +
                BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                DataViewRowState.CurrentRows);

            foreach (BankImportTDSAEpTransactionRow transaction in AMainDS.AEpTransaction.Rows)
            {
                if (transaction.MatchAction == Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_UNMATCHED)
                {
                    DataRowView[] filteredRows = GiftDetailByBatchNumberMatchStatus.FindRows(new object[] { false });

                    BankImportTDSAGiftDetailRow BestMatch = null;
                    int BestMatchNumber = 0;

                    foreach (DataRowView rv in filteredRows)
                    {
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                        int matchNumberDonorSurname =
                            MatchingWords(Calculations.FormatShortName(detailrow.DonorShortName,
                                    eShortNameFormat.eOnlySurname), transaction.AccountName);

                        if (matchNumberDonorSurname == 0)
                        {
                            // if surname does not match: ignore, just to be sure
                            // problem: will ignore umlaut etc. can be fixed for the next time by entering the bank account into OpenPetra
                            continue;
                        }

                        int matchNumberDonor = MatchingWords(detailrow.DonorShortName, transaction.AccountName) +
                                               matchNumberDonorSurname * 3;
                        int matchNumberRecipient = MatchingWords(detailrow.RecipientDescription, transaction.Description);

                        if ((matchNumberDonor > 0) && (matchNumberRecipient > 0)
                            && ((matchNumberDonor > 1) || (matchNumberRecipient > 1))
                            && (matchNumberRecipient + matchNumberDonor > BestMatchNumber))
                        {
                            BestMatchNumber = matchNumberRecipient + matchNumberDonor;
                            BestMatch = detailrow;
                        }
                    }

                    if (BestMatch != null)
                    {
                        // get all gifts of this donor, and all bank statement transactions
                        DataRowView[] GiftDetailWithoutAmount = GiftDetailWithoutAmountView.FindRows(
                            new object[] { BestMatch.DonorKey, false });

                        DataRowView[] TransactionsByBankAccount = TransactionsByBankAccountView.FindRows(
                            new object[] { transaction.BankAccountNumber, transaction.BranchCode, MFinanceConstants.BANK_STMT_STATUS_UNMATCHED });

                        while (MatchOneDonor(AMainDS, GiftDetailWithoutAmount, TransactionsByBankAccount))
                        {
                            GiftDetailWithoutAmount = GiftDetailWithoutAmountView.FindRows(
                                new object[] { BestMatch.DonorKey, false });

                            TransactionsByBankAccount = TransactionsByBankAccountView.FindRows(
                                new object[] { transaction.BankAccountNumber, transaction.BranchCode, MFinanceConstants.BANK_STMT_STATUS_UNMATCHED });

                            newMatchFound = true;
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
            // bank changes tr.AccountName, order of firstname and surname. use account number and blz instead
            string AccountNumber = tr.BranchCode.TrimStart(new char[] { '0' }) + tr.BankAccountNumber.TrimStart(new char[] { '0' });

            string matchtext = tr.Description.ToUpper();

            matchtext = matchtext.
                        Replace(",", "").
                        Replace("/", "").
                        Replace("-", "").
                        Replace(";", "").
                        Replace(".", "").
                        Replace("'", "").
                        Replace(" ", "");

            if (matchtext.Contains("EREF+") && (matchtext.IndexOf("PURP+RINP") > matchtext.IndexOf("EREF+")))
            {
                matchtext = matchtext.Substring(0, matchtext.IndexOf("EREF+")) + matchtext.Substring(matchtext.IndexOf("PURP+RINP"));
            }
            else if (matchtext.Contains("EREF+") && (matchtext.IndexOf("SVWZ+") > matchtext.IndexOf("EREF+")))
            {
                matchtext = matchtext.Substring(0, matchtext.IndexOf("EREF+")) + matchtext.Substring(matchtext.IndexOf("SVWZ+"));
            }
            else if (matchtext.Contains("EREF+") && (matchtext.LastIndexOf("+") == matchtext.IndexOf("EREF+") + 4) && (matchtext.LastIndexOf(":") <= matchtext.IndexOf("EREF+")))
            {
                matchtext = matchtext.Substring(0, matchtext.IndexOf("EREF+"));
            }

            if (matchtext.Contains("IBAN:") && matchtext.Contains("BIC:"))
            {
                matchtext = matchtext.Substring(0, matchtext.IndexOf("IBAN:"));

                if (matchtext.Contains("EREF:"))
                {
                    matchtext = matchtext.Substring(0, matchtext.IndexOf("EREF:"));
                }

                if (matchtext.Contains("EREF:"))
                {
                    matchtext = matchtext.Substring(0, matchtext.IndexOf("EREF:"));
                }
            }

            matchtext = matchtext.Replace("PURP+RINPRATENZAHLUNG", "");
            matchtext = matchtext.Replace("PURP+RINPDauerauftragRate".ToUpper(), "");

            // abweichender Zahlungsauftraggeber
            matchtext = matchtext.Replace("ABWA+", "");

            matchtext = matchtext.Replace("SVWZ+", "");

            matchtext = ABankAccount.ToUpper() + AccountNumber + matchtext + tr.TransactionAmount.ToString("0.##").Replace(".", "");

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

        /// add new matches, and modify existing matches
        private static Int32 UpdateMatches(
            BankImportTDS AMatchDS,
            BankImportTDSAGiftDetailRow AGiftDetailRow,
            string AMatchText,
            int ADetailNr,
            SortedList <string, AEpMatchRow>AMatchesByText,
            SortedList <string, AEpMatchRow>AMatchesToAddLater)
        {
            AEpMatchRow newMatch = null;

            if (AMatchesByText.ContainsKey(AMatchText + ":::" + ADetailNr.ToString()))
            {
                newMatch = AMatchesByText[AMatchText + ":::" + ADetailNr.ToString()];
            }
            else
            {
                // we might have added such a match for the current statement
                int MatchDetail = 0;

                while (AMatchesToAddLater.ContainsKey(AMatchText + ":::" + MatchDetail.ToString())
                       || AMatchesByText.ContainsKey(AMatchText + ":::" + MatchDetail.ToString()))
                {
                    MatchDetail++;
                }

                string key = AMatchText + ":::" + MatchDetail.ToString();

                newMatch = AMatchDS.AEpMatch.NewRowTyped();

                // matchkey will be set properly on save, by sequence
                newMatch.EpMatchKey = -1 * (AMatchesToAddLater.Count + 1);
                newMatch.MatchText = AMatchText;
                AMatchesToAddLater.Add(key, newMatch);

                newMatch.Detail = MatchDetail;
            }

            newMatch.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;

            newMatch.RecipientKey = AGiftDetailRow.RecipientKey;
            newMatch.RecipientLedgerNumber = AGiftDetailRow.RecipientLedgerNumber;
            newMatch.LedgerNumber = AGiftDetailRow.LedgerNumber;
            newMatch.DonorKey = AGiftDetailRow.DonorKey;
            newMatch.DonorShortName = AGiftDetailRow.DonorShortName;
            newMatch.RecipientShortName = AGiftDetailRow.RecipientDescription;
            newMatch.MotivationGroupCode = AGiftDetailRow.MotivationGroupCode;
            newMatch.MotivationDetailCode = AGiftDetailRow.MotivationDetailCode;
            newMatch.GiftCommentOne = AGiftDetailRow.GiftCommentOne;
            newMatch.GiftCommentTwo = AGiftDetailRow.GiftCommentTwo;
            newMatch.GiftCommentThree = AGiftDetailRow.GiftCommentThree;
            newMatch.CommentOneType = AGiftDetailRow.CommentOneType;
            newMatch.CommentTwoType = AGiftDetailRow.CommentTwoType;
            newMatch.CommentThreeType = AGiftDetailRow.CommentThreeType;
            newMatch.MailingCode = AGiftDetailRow.MailingCode;
            newMatch.CostCentreCode = AGiftDetailRow.CostCentreCode;
            newMatch.ChargeFlag = AGiftDetailRow.ChargeFlag;
            newMatch.ConfidentialGiftFlag = AGiftDetailRow.ConfidentialGiftFlag;
            newMatch.GiftTransactionAmount = AGiftDetailRow.GiftTransactionAmount;

            return newMatch.EpMatchKey;
        }

        /// <summary>
        /// store historic Gift matches
        /// </summary>
        private static void StoreCurrentMatches(BankImportTDS AMatchDS, string ABankAccountCode)
        {
            TLogging.LogAtLevel(1, "StoreCurrentMatches...");

            DataView GiftDetailView = new DataView(
                AMatchDS.AGiftDetail, string.Empty,
                BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + "," +
                BankImportTDSAGiftDetailTable.GetDetailNumberDBName(),
                DataViewRowState.CurrentRows);

            SortedList <string, AEpMatchRow>MatchesToAddLater = new SortedList <string, AEpMatchRow>();

            // for speed reasons, use a sortedlist instead of a dataview
            SortedList <string, AEpMatchRow>MatchesByText = new SortedList <string, AEpMatchRow>();

            foreach (AEpMatchRow r in AMatchDS.AEpMatch.Rows)
            {
                MatchesByText[r.MatchText + ":::" + r.Detail.ToString()] = r;
            }

            foreach (BankImportTDSAEpTransactionRow tr in AMatchDS.AEpTransaction.Rows)
            {
                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(ABankAccountCode, tr);

                if (tr.MatchAction != MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    continue;
                }

                // get the gift details assigned to this transaction
                StringCollection GiftDetailNumbers = StringHelper.GetCSVList(tr.GiftDetailNumbers, ",", false);

                foreach (string strDetailNumber in GiftDetailNumbers)
                {
                    DataRowView[] FilteredGiftDetails =
                        GiftDetailView.FindRows(
                            new object[] {
                                tr.GiftTransactionNumber,
                                Convert.ToInt32(strDetailNumber)
                            });

                    // add new matches, and modify existing matches
                    UpdateMatches(
                        AMatchDS,
                        (BankImportTDSAGiftDetailRow)FilteredGiftDetails[0].Row,
                        MatchText,
                        Convert.ToInt32(strDetailNumber) - 1,
                        MatchesByText,
                        MatchesToAddLater);
                }
            }

            // for speed reasons, add the new rows at the end
            foreach (AEpMatchRow m in MatchesToAddLater.Values)
            {
                AMatchDS.AEpMatch.Rows.Add(m);
            }

            AMatchDS.PBankingDetails.Clear();
            AMatchDS.AGiftDetail.Clear();
            AMatchDS.AGift.Clear();

            AMatchDS.ThrowAwayAfterSubmitChanges = true;

            TLogging.LogAtLevel(1, "before submitchanges");

            BankImportTDSAccess.SubmitChanges(AMatchDS);

            TLogging.LogAtLevel(1, "after submitchanges");
        }
    }
}
