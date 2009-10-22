/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    /// <summary>
    /// useful functions for gift matching. this uses an sqlite database at the moment
    /// </summary>
    public class TGiftMatching
    {
        private TDataBase FSqliteDatabase = null;

        private TDataBase ConnectDatabase(string ADBFilename)
        {
            if (!File.Exists(ADBFilename))
            {
                // create simple sqlite db, with just the a_ep_match table
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + ADBFilename);
                conn.Open();
                string createStmt = TDataBase.ReadSqlFile("CreateMatchDB.sql");
                SQLiteCommand cmd = new SQLiteCommand(createStmt, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            TDataBase db = new TDataBase();
            db.EstablishDBConnection(TDBType.SQLite, ADBFilename, "", "", "", "");
            return db;
        }

        /// <summary>
        /// return the filter for GiftDetail to show all the gifts associated with the given transaction;
        /// this is useful after a bank statement has been matched against an imported gift batch, to train the system
        /// </summary>
        /// <returns>the filter; empty string if no transactions selected</returns>
        public static string FilterForMatchedGiftTransactions(BankImportTDSAEpTransactionRow ATransactionRow, Int32 ASelectedGiftBatch)
        {
            if (ATransactionRow.GiftTransactionNumber.Length == 0)
            {
                return String.Empty;
            }

            string[] TransactionNumbers = ATransactionRow.GiftTransactionNumber.Split(',');
            string Filter = String.Empty;

            foreach (string TransactionNumber in TransactionNumbers)
            {
                string[] DetailNumbers = ATransactionRow.GiftDetailNumber.Split(',');

                if (Filter.Length > 0)
                {
                    Filter += " OR (";
                }
                else
                {
                    Filter += "(";
                }

                foreach (string DetailNumber in DetailNumbers)
                {
                    if (Convert.ToInt32(DetailNumber) == -1)
                    {
                        // all gift details
                        Filter += BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " = " +
                                  TransactionNumber;
                    }
                    else
                    {
                        Filter += BankImportTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " = " +
                                  TransactionNumber +
                                  " AND " + BankImportTDSAGiftDetailTable.GetDetailNumberDBName() + " = " +
                                  DetailNumber;
                    }
                }

                Filter += ")";
            }

            return BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                   ASelectedGiftBatch.ToString() +
                   " AND AlreadyMatched = true " +
                   " AND (" + Filter + ")";
        }

        /// <summary>
        /// match text should uniquely identify a gift from a certain donor with a certain purpose;
        /// use account name, description, and amount;
        /// remove umlaut and spaces, because the banks sometimes play around with them
        /// </summary>
        private static string CalculateMatchText(AEpTransactionRow tr)
        {
            string matchtext = tr.AccountName + tr.Description + tr.TransactionAmount;
            string oldMatchText = String.Empty;

            while (oldMatchText != matchtext)
            {
                oldMatchText = matchtext;
                matchtext = matchtext.Replace("UE", "");
                matchtext = matchtext.Replace("AE", "");
                matchtext = matchtext.Replace("OE", "");
                matchtext = matchtext.Replace("SS", "");
                matchtext = matchtext.Replace(" ", "");
            }

            return matchtext;
        }

        /// <summary>
        /// returns true if a match with exactly the same settings already exists for the transactions and gift batch;
        /// returns false if no such match is there, or if something has been changed in the gift batch
        /// </summary>
        private bool IdenticalMatchAlreadyExists(BankImportTDS AMainDS, BankImportTDS AMatchDS)
        {
            if (AMainDS.AGiftDetail.DefaultView.Count != AMatchDS.AEpMatch.Rows.Count)
            {
                return true;
            }

            AMainDS.AGiftDetail.DefaultView.Sort = AGiftDetailTable.GetDetailNumberDBName();
            AMatchDS.AEpMatch.DefaultView.Sort = AEpMatchTable.GetDetailDBName();

            // is it assigned to the same gift details?
            for (Int32 Counter = 0; Counter < AMainDS.AGiftDetail.DefaultView.Count; Counter++)
            {
                // both views are sorted by detail number, so they should fit
                BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[Counter].Row;
                AEpMatchRow matchRow = (AEpMatchRow)AMatchDS.AEpMatch.DefaultView[Counter].Row;

                bool sameData = true;
                sameData = sameData && matchRow.RecipientKey == giftRow.RecipientKey;
                sameData = sameData && matchRow.RecipientLedgerNumber == giftRow.RecipientLedgerNumber;
                sameData = sameData && matchRow.DonorKey == giftRow.DonorKey;
                sameData = sameData && matchRow.MotivationGroupCode == giftRow.MotivationGroupCode;
                sameData = sameData && matchRow.MotivationDetailCode == giftRow.MotivationDetailCode;
                sameData = sameData && matchRow.GiftCommentOne == giftRow.GiftCommentOne;
                sameData = sameData && matchRow.GiftCommentTwo == giftRow.GiftCommentTwo;
                sameData = sameData && matchRow.GiftCommentThree == giftRow.GiftCommentThree;
                sameData = sameData && matchRow.CommentOneType == giftRow.CommentOneType;
                sameData = sameData && matchRow.CommentTwoType == giftRow.CommentTwoType;
                sameData = sameData && matchRow.CommentThreeType == giftRow.CommentThreeType;
                sameData = sameData && matchRow.MailingCode == giftRow.MailingCode;
                sameData = sameData && matchRow.ChargeFlag == giftRow.ChargeFlag;
                sameData = sameData && matchRow.ConfidentialGiftFlag == giftRow.ConfidentialGiftFlag;
                sameData = sameData && matchRow.GiftTransactionAmount == giftRow.GiftTransactionAmount;

                // don't check for costcentre code, since that is modified during posting anyways???
                // sameData = sameData && matchRow.CostCentreCode == giftRow.CostCentreCode;

                if (!sameData)
                {
                    return true;
                }
            }

            // all details have already matched in exactly the same way as in the compared gift batch
            return false;
        }

        // add new (or modified) matches
        void CreateNewMatches(BankImportTDS AMainDS, BankImportTDS AMatchDS, string AMatchText)
        {
            Int32 countDetail = 1;

            // create a match with the same matchtext for each gift detail (split gift)
            foreach (DataRowView gv in AMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;
                AEpMatchRow newMatch = AMatchDS.AEpMatch.NewRowTyped();

                newMatch.MatchText = AMatchText;
                newMatch.Detail = countDetail;

                newMatch.RecipientKey = giftRow.RecipientKey;
                newMatch.RecipientLedgerNumber = giftRow.RecipientLedgerNumber;
                newMatch.DonorKey = giftRow.DonorKey;
                newMatch.MotivationGroupCode = giftRow.MotivationGroupCode;
                newMatch.MotivationDetailCode = giftRow.MotivationDetailCode;
                newMatch.GiftCommentOne = giftRow.GiftCommentOne;
                newMatch.GiftCommentTwo = giftRow.GiftCommentTwo;
                newMatch.GiftCommentThree = giftRow.GiftCommentThree;
                newMatch.CommentOneType = giftRow.CommentOneType;
                newMatch.CommentTwoType = giftRow.CommentTwoType;
                newMatch.CommentThreeType = giftRow.CommentThreeType;
                newMatch.MailingCode = giftRow.MailingCode;
                newMatch.ChargeFlag = giftRow.ChargeFlag;
                newMatch.ConfidentialGiftFlag = giftRow.ConfidentialGiftFlag;
                newMatch.GiftTransactionAmount = giftRow.GiftTransactionAmount;

                AMatchDS.AEpMatch.Rows.Add(newMatch);

                countDetail++;
            }
        }

        /// <summary>
        /// historic Gift matches are stored in a sqlite database
        /// </summary>
        public void StoreCurrentMatches(ref BankImportTDS AMainDS, Int32 ASelectedGiftBatch)
        {
            // first connect to the database (this will also create the initial database)
            if (FSqliteDatabase == null)
            {
                FSqliteDatabase = ConnectDatabase("tempMatching.db");
            }

            // for all matched FMainDS.AEpTransactions
            AMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " = '" +
                                                           Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // get the gift details assigned to this transaction
                AMainDS.AGiftDetail.DefaultView.RowFilter = FilterForMatchedGiftTransactions(tr, ASelectedGiftBatch);

                if (AMainDS.AGiftDetail.DefaultView.RowFilter.Length == 0)
                {
                    // there is no matching gift in the gift batch
                    continue;
                }

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(tr);

                // check if such a match text already exists
                // can return several matches, for split gifts
                string checkForMatch = "SELECT * FROM " + AEpMatchTable.GetTableDBName() + " WHERE " + AEpMatchTable.GetMatchTextDBName() + " = '" +
                                       MatchText + "'";
                BankImportTDS MatchDS = new BankImportTDS();
                FSqliteDatabase.Select(MatchDS, checkForMatch, AEpMatchTable.GetTableName(), null);

                bool newMatch = IdenticalMatchAlreadyExists(AMainDS, MatchDS);

                if (!newMatch)
                {
                    // no changes to the already existing match
                    continue;
                }

                // therefore delete the current matches if any exist
                foreach (AEpMatchRow matchRow in MatchDS.AEpMatch.Rows)
                {
                    matchRow.Delete();
                }

                // add new (or modified) matches
                CreateNewMatches(AMainDS, MatchDS, MatchText);

                TVerificationResultCollection Verification;
                AEpMatchAccess.SubmitChanges(MatchDS.AEpMatch, null, out Verification);
            }
        }
    }
}