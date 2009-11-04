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
using System.Collections.Specialized;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Printing;
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

            UserInfo.GUserInfo = new TPetraPrincipal(new TPetraIdentity("TEMP", "", "", "", "", DateTime.Now, DateTime.Now, DateTime.Now,
                    -1, -1, -1, false, false), null);

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

                // TODO use constant for GIFT
                sameData = sameData && matchRow.Action == "GIFT";
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

                // matchkey will be set properly on save, by sequence
                newMatch.EpMatchKey = -1 * (AMatchDS.AEpMatch.Count + 1);
                newMatch.MatchText = AMatchText;
                newMatch.Detail = countDetail;
                newMatch.Action = "GIFT"; // TODO: use constant for GIFT

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
                FSqliteDatabase = ConnectDatabase(TAppSettingsManager.GetValueStatic("MatchingDB.file"));
            }

            TDBTransaction dbtransaction = FSqliteDatabase.BeginTransaction();

            // for all matched FMainDS.AEpTransactions
            AMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " = '" +
                                                           Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";
            BankImportTDS MatchDS = new BankImportTDS();

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
                MatchDS.AEpMatch.Rows.Clear();
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
                TDataBase backupDB = DBAccess.GDBAccessObj;
                DBAccess.GDBAccessObj = FSqliteDatabase;
                AEpMatchAccess.SubmitChanges(MatchDS.AEpMatch, dbtransaction, out Verification);
                DBAccess.GDBAccessObj = backupDB;
            }

            FSqliteDatabase.CommitTransaction();

            FSqliteDatabase.CloseDBConnection();

            FSqliteDatabase = null;
        }

        /// <summary>
        /// match loaded bank statement transactions to saved matches
        /// </summary>
        public bool FindMatches(ref BankImportTDS AMainDS)
        {
            // first connect to the database
            if (FSqliteDatabase == null)
            {
                FSqliteDatabase = ConnectDatabase((TAppSettingsManager.GetValueStatic("MatchingDB.file")));
            }

            AMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " IS NULL";

            BankImportTDS MatchDS = new BankImportTDS();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(tr);

                // check if such a match text already exists
                // can return several matches, for split gifts
                MatchDS.AEpMatch.Rows.Clear();
                string checkForMatch = "SELECT * FROM " + AEpMatchTable.GetTableDBName() + " WHERE " + AEpMatchTable.GetMatchTextDBName() + " = '" +
                                       MatchText + "'";
                FSqliteDatabase.Select(MatchDS, checkForMatch, AEpMatchTable.GetTableName(), null);

                if (MatchDS.AEpMatch.Rows.Count > 0)
                {
                    // there is a match
                    tr.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                    tr.EpMatchKey = MatchDS.AEpMatch[0].EpMatchKey;

                    // TODO: store donor short name in the match?
                    tr.DonorKey = MatchDS.AEpMatch[0].DonorKey;
                    tr.DonorShortName = MatchDS.AEpMatch[0].DonorShortName;

                    foreach (AEpMatchRow match in MatchDS.AEpMatch.Rows)
                    {
                        if (tr.RecipientDescription.Length > 0)
                        {
                            tr.RecipientDescription += "; ";
                        }

                        if (match.RecipientKey > 0)
                        {
                            tr.RecipientDescription += match.RecipientShortName.ToString();
                        }
                        else
                        {
                            tr.RecipientDescription += match.MotivationGroupCode + "/" + match.MotivationDetailCode;
                        }

                        tr.RecipientDescription += " (" + match.GiftTransactionAmount.ToString() + ")";
                    }
                }
            }

            FSqliteDatabase.CloseDBConnection();

            FSqliteDatabase = null;

            return true;
        }

        /// <summary>
        /// store matched gifts to a text file which can be imported into Petra 2.x
        /// </summary>
        public void WritePetraImportFile(ref BankImportTDS AMainDS, StreamWriter sw, string ABankName)
        {
            // first connect to the database
            if (FSqliteDatabase == null)
            {
                FSqliteDatabase = ConnectDatabase(TAppSettingsManager.GetValueStatic("MatchingDB.file"));

                // FSqliteDatabase.DebugLevel = 10;
            }

            BankImportTDS MatchDS = new BankImportTDS();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // find the match
                // can return several matches, for split gifts
                MatchDS.AEpMatch.Rows.Clear();
                string checkForMatch = "SELECT * FROM " + AEpMatchTable.GetTableDBName() + " WHERE " +
                                       AEpMatchTable.GetEpMatchKeyDBName() + " = " +
                                       tr.EpMatchKey.ToString();
                FSqliteDatabase.Select(MatchDS, checkForMatch, AEpMatchTable.GetTableName(), null);

                foreach (AEpMatchRow match in MatchDS.AEpMatch.Rows)
                {
                    sw.WriteLine(match.DonorKey.ToString() +
                        ";\"" + tr.DonorShortName + "\";\"\";\"\";\"" + ABankName + " " +
                        tr.DateEffective.ToString("dd/MM/yyyy") +
                        "\";\"<none>\";" +
                        match.RecipientKey.ToString() + ";\"" +
                        match.RecipientShortName + "\";" +
                        match.GiftTransactionAmount.ToString() +
                        ";no;\"" + match.MotivationGroupCode + "\";\"" +
                        match.MotivationDetailCode +
                        "\";\"\";\"Both\";\"\";\"\";\"\";\"\";\"\";yes");
                }
            }

            FSqliteDatabase.CloseDBConnection();

            FSqliteDatabase = null;
        }

        /// <summary>
        /// dump unmatched gifts or other transactions to a CSV file
        /// </summary>
        public static void WriteCSVFile(ref BankImportTDS AMainDS, StreamWriter sw)
        {
            sw.WriteLine("#TransactionTypeCode;#AccountName;#BankAccountNumber;#BranchCode;#BIC;#IBAN;#Description;#TransactionAmount");

            BankImportTDSAEpTransactionRow row = (BankImportTDSAEpTransactionRow)AMainDS.AEpTransaction.DefaultView[0].Row;

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetDonorShortNameDBName() + "," +
                                                      BankImportTDSAEpTransactionTable.GetRecipientDescriptionDBName();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                row = (BankImportTDSAEpTransactionRow)rv.Row;

                sw.WriteLine(row.TransactionTypeCode + ";" +
                    row.DateEffective.ToShortDateString() + ";" +
                    row.AccountName + ";" +
                    row.BankAccountNumber + ";" +
                    row.BranchCode + ";" +
                    row.Bic + ";" +
                    row.Iban + ";" +
                    row.Description + ";" +
                    row.TransactionAmount.ToString() + ";");
            }
        }

        /// <summary>
        /// dump unmatched gifts or other transactions to a HTML table for printing
        /// </summary>
        public static string PrintHTML(ref BankImportTDS AMainDS, string ATitle)
        {
            string letterTemplateFilename = TAppSettingsManager.GetValueStatic("TransactionList.File");

            // message body from HTML template
            StreamReader reader = new StreamReader(letterTemplateFilename);
            string msg = reader.ReadToEnd();

            reader.Close();

            msg = msg.Replace("#TITLE", ATitle);

            // recognise detail lines automatically
            string RowTemplate;
            msg = TPrinterHtml.GetTableRow(msg, "#DESCRIPTION", out RowTemplate);
            string rowTexts = "";

            BankImportTDSAEpTransactionRow row = null;

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetTransactionAmountDBName() + "," +
                                                      BankImportTDSAEpTransactionTable.GetOrderDBName();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                row = (BankImportTDSAEpTransactionRow)rv.Row;

                rowTexts += RowTemplate.
                            Replace("#NAME", row.AccountName).
                            Replace("#DESCRIPTION", row.Description).
                            Replace("#AMOUNT", String.Format("{0:C}", row.TransactionAmount)).
                            Replace("#ACCOUNTNUMBER", row.BankAccountNumber).
                            Replace("#BANKSORTCODE", row.BranchCode).
                            Replace("#TRANSACTIONTYPE", row.TransactionTypeCode);
            }

            return msg.Replace("#ROWTEMPLATE", rowTexts);
        }

        private static Int64 GetDonorByBankAccountNumber(ref BankImportTDS AMainDS, string ABankAccountNumber)
        {
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

        private static void MarkTransactionMatched(ref BankImportTDS AMainDS,
            ref BankImportTDSAEpTransactionRow stmtRow,
            Int32 ASelectedGiftBatch,
            Int32 AGiftTransactionNumber,
            Int32 AGiftDetailNumber)
        {
            stmtRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;

//            if (stmtRow.AccountName.Contains("TEST"))
//            {
//                TLogging.Log(AGiftDetailNumber.ToString());
//                TLogging.LogStackTrace(TLoggingType.ToLogfile);
//            }

            foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                // check again for AlreadyMatched, the details might have been matched in a previous iteration of this loop
                if ((detailrow.AlreadyMatched == false) && (detailrow.GiftTransactionNumber == AGiftTransactionNumber)
                    && (detailrow.BatchNumber == ASelectedGiftBatch)
                    && ((AGiftDetailNumber == -1) || (AGiftDetailNumber == detailrow.DetailNumber)))
                {
                    if (stmtRow.IsGiftBatchNumberNull())
                    {
                        stmtRow.GiftLedgerNumber = detailrow.LedgerNumber.ToString();
                        stmtRow.GiftBatchNumber = detailrow.BatchNumber.ToString();
                        stmtRow.GiftTransactionNumber = detailrow.GiftTransactionNumber.ToString();
                        stmtRow.GiftDetailNumber = AGiftDetailNumber.ToString();
                    }
                    else
                    {
                        stmtRow.GiftLedgerNumber += "," + detailrow.LedgerNumber.ToString();
                        stmtRow.GiftBatchNumber += "," + detailrow.BatchNumber.ToString();
                        stmtRow.GiftTransactionNumber += "," + detailrow.GiftTransactionNumber.ToString();
                        stmtRow.GiftDetailNumber += "," + AGiftDetailNumber.ToString();
                    }

//                    if ((stmtRow.GiftDetailNumber.IndexOf("-1,") != -1) || (stmtRow.GiftDetailNumber.IndexOf(",-1") != -1))
//                    {
//                        TLogging.Log(stmtRow.GiftDetailNumber + " " + stmtRow.DonorShortName);
//                        TLogging.LogStackTrace(TLoggingType.ToLogfile);
//                    }

                    stmtRow.DonorKey = detailrow.DonorKey;
                    stmtRow.DonorShortName = detailrow.DonorShortName;

                    // find other details of this gift transaction and mark them as matched too
                    DataView v2 = new DataView(AMainDS.AGiftDetail);
                    v2.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + " = " + detailrow.BatchNumber.ToString() +
                                   " AND " + AGiftDetailTable.GetGiftTransactionNumberDBName() + " = " + detailrow.GiftTransactionNumber.ToString() +
                                   " AND AlreadyMatched = false";

                    if (AGiftDetailNumber != -1)
                    {
                        v2.RowFilter += " AND " + AGiftDetailTable.GetDetailNumberDBName() + " = " + AGiftDetailNumber.ToString();
                    }

                    foreach (DataRowView rv2 in v2)
                    {
                        BankImportTDSAGiftDetailRow detailrow2 = (BankImportTDSAGiftDetailRow)rv2.Row;

                        if (detailrow2.RecipientDescription.Length == 0)
                        {
                            detailrow2.RecipientDescription = detailrow2.MotivationGroupCode + "/" + detailrow2.MotivationDetailCode;
                        }

                        if (stmtRow.RecipientDescription.Length > 0)
                        {
                            stmtRow.RecipientDescription += "; ";
                        }

                        stmtRow.RecipientDescription += detailrow2.RecipientDescription + " (" + detailrow2.GiftTransactionAmount.ToString() + ")";
                        detailrow2.AlreadyMatched = true;
                    }
                }
            }
        }

        /// <summary>
        /// mark all gift details of the transaction as matched
        /// </summary>
        private static void MarkTransactionMatched(ref BankImportTDS AMainDS,
            ref BankImportTDSAEpTransactionRow stmtRow,
            BankImportTDSAGiftDetailRow giftDetail)
        {
            if (SumAmounts(ref AMainDS, giftDetail.BatchNumber, giftDetail.GiftTransactionNumber) != Convert.ToDecimal(stmtRow.TransactionAmount))
            {
                // it seems that there are several different transactions treated as a split gift
                // don't mark the whole transactions as matched
                MarkTransactionMatched(ref AMainDS, ref stmtRow, giftDetail.BatchNumber, giftDetail.GiftTransactionNumber, giftDetail.DetailNumber);
            }
            else
            {
                // found a match
                MarkTransactionMatched(ref AMainDS, ref stmtRow, giftDetail.BatchNumber, giftDetail.GiftTransactionNumber, -1);
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

        private static Decimal SumAmounts(ref BankImportTDS AMainDS, Int32 ASelectedGiftBatch, Int32 AGiftTransactionNumber)
        {
            Decimal Result = 0.0m;

            // attention: this does check even for already matched gift details
            DataView v = new DataView(AMainDS.AGiftDetail);

            v.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + " = " + ASelectedGiftBatch.ToString() +
                          " AND " + AGiftDetailTable.GetGiftTransactionNumberDBName() + " = " + AGiftTransactionNumber.ToString();

            foreach (DataRowView rv in v)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                Result += Convert.ToDecimal(detailrow.GiftTransactionAmount);
            }

            return Result;
        }

        /// <summary>
        /// try to find any posted or unposted gift batch and try to match gifts
        /// </summary>
        /// <returns>the gift batch number if gift batch has already been posted</returns>
        public static Int32 AutoMatchGiftsAgainstPetraDB(ref BankImportTDS AMainDS)
        {
            // first stage: collect historic matches from Petra database
            // go through each transaction of the statement,
            // and see if you can find a donation on that date with the same amount from the same bank account
            // store this as a match

            Int32 SelectedGiftBatch = -1;

            // Get all gifts at given date
            TGetData.GetGiftsByDate(ref AMainDS, AMainDS.AEpTransaction[0].DateEffective);

            // simple matching; no split gifts, bank account number fits and amount fits
            // problem: recipient different????
            Int32 CountMatches = 0;

            for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    Int64 DonorKey = GetDonorByBankAccountNumber(ref AMainDS, stmtRow.BankAccountNumber);

                    AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (SelectedGiftBatch != -1)
                    {
                        AMainDS.AGiftDetail.DefaultView.RowFilter += " AND " + AGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                     SelectedGiftBatch.ToString();
                    }

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
            }

            if ((SelectedGiftBatch == -1) || ((AMainDS.AEpTransaction.Rows.Count > 2) && (CountMatches < AMainDS.AEpTransaction.Rows.Count / 2)))
            {
                return -1;
            }

            AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                        SelectedGiftBatch.ToString();

            bool postedBatch = ((BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row).BatchStatus == "Posted";

            for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    Int64 DonorKey = GetDonorByBankAccountNumber(ref AMainDS, stmtRow.BankAccountNumber);

                    // look for gifts that match the donor (identified by account number) and the transaction amount
                    AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString() + " AND " +
                                                                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                SelectedGiftBatch.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (AMainDS.AGiftDetail.DefaultView.Count > 1)
                    {
                        // donor has several gifts with same amount?
                        // look for fitting words in description
                        int MaxCount = -1;
                        BankImportTDSAGiftDetailRow MaxRow = null;

                        foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                            int count = MatchingWords(detailrow.RecipientDescription, stmtRow.Description);

                            if (count > MaxCount)
                            {
                                MaxCount = count;
                                MaxRow = detailrow;
                            }
                        }

                        if (MaxCount > 0)
                        {
                            // found a match
                            MarkTransactionMatched(ref AMainDS, ref stmtRow, MaxRow);
                        }
                    }
                    else if (AMainDS.AGiftDetail.DefaultView.Count == 0)
                    {
                        // split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount

                        // get all gifts with that bank account number
                        DonorKey = GetDonorByBankAccountNumber(ref AMainDS, stmtRow.BankAccountNumber);

                        AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                    DonorKey.ToString() + " AND " +
                                                                    BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    SelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";

                        if (AMainDS.AGiftDetail.DefaultView.Count > 1)
                        {
                            BankImportTDSAGiftDetailRow matchingGiftDetail = null;
                            bool duplicateMatches = false;

                            foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
                            {
                                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                                if ((matchingGiftDetail == null) || (detailrow.GiftTransactionNumber != matchingGiftDetail.GiftTransactionNumber))
                                {
                                    if ((SumAmounts(ref AMainDS, SelectedGiftBatch,
                                             detailrow.GiftTransactionNumber) == Convert.ToDecimal(stmtRow.TransactionAmount))
                                        || (Convert.ToDecimal(detailrow.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount)))
                                    {
                                        if (matchingGiftDetail != null)
                                        {
                                            duplicateMatches = true;
                                        }

                                        matchingGiftDetail = detailrow;
                                    }
                                }
                            }

                            // TODO several gifts match this amount
                            if (!duplicateMatches && (matchingGiftDetail != null))
                            {
                                // found a match
                                if (Convert.ToDecimal(matchingGiftDetail.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount))
                                {
                                    MarkTransactionMatched(ref AMainDS,
                                        ref stmtRow,
                                        SelectedGiftBatch,
                                        matchingGiftDetail.GiftTransactionNumber,
                                        matchingGiftDetail.DetailNumber);
                                }
                                else
                                {
                                    MarkTransactionMatched(ref AMainDS, ref stmtRow, SelectedGiftBatch, matchingGiftDetail.GiftTransactionNumber, -1);
                                }
                            }
                        }
                    }
                }
            }

            if (postedBatch)
            {
                // do another loop, now looking even harder for matching gifts; match donor name, and recipient name with transaction description
                // by now the list of unassigned gifts from the old gift batch should be quite small
                for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
                {
                    BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                    if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                    {
                        AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    SelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";
                        BankImportTDSAGiftDetailRow BestMatch = null;
                        int BestMatchNumber = 0;

                        foreach (DataRowView rv in AMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                            int matchNumber = MatchingWords(detailrow.DonorShortName, stmtRow.AccountName) +
                                              MatchingWords(detailrow.RecipientDescription, stmtRow.Description);

                            if ((matchNumber > BestMatchNumber)
                                && ((SumAmounts(ref AMainDS, SelectedGiftBatch,
                                         detailrow.GiftTransactionNumber) == Convert.ToDecimal(stmtRow.TransactionAmount))
                                    || (Convert.ToDecimal(detailrow.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount))))
                            {
                                BestMatchNumber = matchNumber;
                                BestMatch = detailrow;
                            }
                        }

                        if (BestMatchNumber > 0)
                        {
                            if (Convert.ToDecimal(BestMatch.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount))
                            {
                                // only matches part of the gift
                                MarkTransactionMatched(ref AMainDS,
                                    ref stmtRow,
                                    SelectedGiftBatch,
                                    BestMatch.GiftTransactionNumber,
                                    BestMatch.DetailNumber);
                            }
                            else
                            {
                                // match full gift
                                MarkTransactionMatched(ref AMainDS, ref stmtRow, SelectedGiftBatch, BestMatch.GiftTransactionNumber, -1);
                            }
                        }
                    }
                }
            }

            TGiftMatching storeMatchGifts = new TGiftMatching();
            storeMatchGifts.StoreCurrentMatches(ref AMainDS, SelectedGiftBatch);

            if (postedBatch)
            {
                return SelectedGiftBatch;
            }

            // batch has not been posted yet, so might need retraining
            return -1;
        }

        /// also do training on splitted files that match batches that have been posted
        /// then move the files to imported
        public static void Training(string ALegalEntity, IImportBankStatement ABankStatementImporter)
        {
            string OutputPath = TAppSettingsManager.GetValueStatic("MT940.Output.Path");

            string[] unfinishedFiles = Directory.GetFiles(OutputPath + Path.DirectorySeparatorChar + ALegalEntity,
                "*.sta");

            foreach (string filename in unfinishedFiles)
            {
                BankImportTDS MainDS = new BankImportTDS();

                // TODO: at the moment only support one statement by file?
                double startBalance, endBalance;
                string bankName;
                ABankStatementImporter.ImportFromFile(filename, ref MainDS, out startBalance, out endBalance, out bankName);

                if (AutoMatchGiftsAgainstPetraDB(ref MainDS) != -1)
                {
                    // move file to imported folder
                    File.Move(
                        filename,
                        OutputPath + Path.DirectorySeparatorChar + ALegalEntity + Path.DirectorySeparatorChar + "imported" +
                        Path.DirectorySeparatorChar +
                        Path.GetFileName(filename));
                }
            }
        }
    }
}