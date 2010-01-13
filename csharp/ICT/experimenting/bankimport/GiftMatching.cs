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
            db.EstablishDBConnection(TDBType.SQLite, ADBFilename, "", "", "", "", "");
            return db;
        }

        /// <summary>
        /// return the filter for GiftDetail to show all the gifts associated with the given transaction;
        /// this is useful after a bank statement has been matched against an imported gift batch, to train the system
        /// </summary>
        /// <returns>the filter; empty string if no transactions selected</returns>
        public static string FilterForMatchedGiftTransactions(BankImportTDSAEpTransactionRow ATransactionRow, Int32 ASelectedGiftBatch)
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

        /// <summary>
        /// match text should uniquely identify a gift from a certain donor with a certain purpose;
        /// use account name, description, and amount;
        /// remove umlaut and spaces, because the banks sometimes play around with them
        /// </summary>
        private static string CalculateMatchText(BankImportTDSAEpTransactionRow tr)
        {
            string matchtext = tr.AccountName + tr.Description;

            if (tr.IsOriginalAmountOnStatementNull())
            {
                matchtext += tr.TransactionAmount;
            }
            else
            {
                matchtext += tr.OriginalAmountOnStatement;
            }

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

        /// add new (or modified) matches
        private void CreateNewMatches(BankImportTDS AMainDS, BankImportTDS AMatchDS, string AMatchText)
        {
            // create a match with the same matchtext for each gift detail (split gift)
            foreach (DataRowView gv in AMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;
                AEpMatchRow newMatch = AMatchDS.AEpMatch.NewRowTyped();

                // matchkey will be set properly on save, by sequence
                newMatch.EpMatchKey = -1 * (AMatchDS.AEpMatch.Count + 1);
                newMatch.MatchText = AMatchText;
                newMatch.Detail = giftRow.DetailNumber;
                newMatch.Action = "GIFT";                 // TODO: use constant for GIFT

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

                AMatchDS.AEpMatch.DefaultView.RowFilter = AEpMatchTable.GetMatchTextDBName() + " = '" + newMatch.MatchText + "' and " +
                                                          AEpMatchTable.GetDetailDBName() + " = " + newMatch.Detail.ToString();

                if (AMatchDS.AEpMatch.DefaultView.Count == 0)
                {
                    AMatchDS.AEpMatch.Rows.Add(newMatch);
                }

                AMatchDS.AEpMatch.DefaultView.RowFilter = "";
            }
        }

        /// <summary>
        /// historic Gift matches are stored in a sqlite database
        /// </summary>
        public void StoreCurrentMatches(ref BankImportTDS AMainDS, Int32 ASelectedGiftBatch)
        {
            TDataBase backupDB = DBAccess.GDBAccessObj;

            // first connect to the database (this will also create the initial database)
            if (FSqliteDatabase == null)
            {
                FSqliteDatabase = ConnectDatabase(TAppSettingsManager.GetValueStatic("MatchingDB.file"));
            }

            DBAccess.GDBAccessObj = FSqliteDatabase;

            TDBTransaction dbtransaction = FSqliteDatabase.BeginTransaction();

            // for all matched FMainDS.AEpTransactions
            AMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " = '" +
                                                           Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";

            // first delete all existing matches with the same match texts
            // TODO: this is not very efficient; on the other hand, there can be split gifts, and if the splitting changes, how is it ensured that the match is updated?
            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(tr);

                string deleteMatch = "DELETE FROM " + AEpMatchTable.GetTableDBName() + " WHERE " + AEpMatchTable.GetMatchTextDBName() + " = '" +
                                     MatchText + "'";
                FSqliteDatabase.ExecuteNonQuery(deleteMatch, dbtransaction, false);
            }

            BankImportTDS MatchDS = new BankImportTDS();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow tr = (BankImportTDSAEpTransactionRow)rv.Row;

                // get the gift details assigned to this transaction
                AMainDS.AGiftDetail.DefaultView.RowFilter = FilterForMatchedGiftTransactions(tr, ASelectedGiftBatch);

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(tr);

                // add new (or modified) matches
                CreateNewMatches(AMainDS, MatchDS, MatchText);

                tr.EpMatchKey = ((AEpMatchRow)MatchDS.AEpMatch.DefaultView[0].Row).EpMatchKey;
            }

            TVerificationResultCollection Verification;
            AEpMatchAccess.SubmitChanges(MatchDS.AEpMatch, dbtransaction, out Verification);
            FSqliteDatabase.CommitTransaction();
            FSqliteDatabase.CloseDBConnection();
            FSqliteDatabase = null;
            DBAccess.GDBAccessObj = backupDB;
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
                BankImportTDSAEpTransactionRow stmtRow = (BankImportTDSAEpTransactionRow)rv.Row;

                // create a match text which uniquely identifies this transaction
                string MatchText = CalculateMatchText(stmtRow);

                // check if such a match text already exists
                // can return several matches, for split gifts
                MatchDS.AEpMatch.Rows.Clear();
                string checkForMatch = "SELECT * FROM " + AEpMatchTable.GetTableDBName() + " WHERE " + AEpMatchTable.GetMatchTextDBName() + " = '" +
                                       MatchText + "'";
                FSqliteDatabase.Select(MatchDS, checkForMatch, AEpMatchTable.GetTableName(), null);

                Int32 countMatches = 0;

                foreach (AEpMatchRow match in MatchDS.AEpMatch.Rows)
                {
                    if (countMatches == 0)
                    {
                        stmtRow.DetailKey = match.Detail;
                        stmtRow.EpMatchKey = match.EpMatchKey;
                        stmtRow.DonorKey = match.DonorKey;
                        stmtRow.DonorShortName = match.DonorShortName;
                        stmtRow.RecipientDescription = match.RecipientShortName;
                        stmtRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;

                        if (stmtRow.IsOriginalAmountOnStatementNull())
                        {
                            stmtRow.OriginalAmountOnStatement = stmtRow.TransactionAmount;
                        }

                        stmtRow.TransactionAmount = match.GiftTransactionAmount;
                    }

                    if (countMatches > 0)
                    {
                        // create a copy of the transaction
                        BankImportTDSAEpTransactionRow newRow = AMainDS.AEpTransaction.NewRowTyped();
                        newRow.StatementKey = stmtRow.StatementKey;
                        newRow.NumberOnStatement = stmtRow.NumberOnStatement;
                        newRow.Order = stmtRow.Order;
                        newRow.DetailKey = match.Detail;
                        newRow.EpMatchKey = match.EpMatchKey;
                        newRow.AccountName = stmtRow.AccountName;
                        newRow.BankAccountNumber = stmtRow.BankAccountNumber;
                        newRow.BranchCode = stmtRow.BranchCode;
                        newRow.Description = stmtRow.Description;
                        newRow.TransactionTypeCode = stmtRow.TransactionTypeCode;
                        newRow.DonorShortName = match.DonorShortName;
                        newRow.DonorKey = match.DonorKey;
                        newRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                        newRow.OriginalAmountOnStatement = stmtRow.OriginalAmountOnStatement;
                        newRow.TransactionAmount = match.GiftTransactionAmount;
                        newRow.RecipientDescription = match.RecipientShortName;

                        AMainDS.AEpTransaction.Rows.Add(newRow);
                    }

                    countMatches++;
                }

                if (MatchDS.AEpMatch.Rows.Count == 0)
                {
                    // try to find the donor by the bank account number
                    string DonorName;
                    Int64 DonorKey = TGetData.GetDonorByAccountNumber(stmtRow.BankAccountNumber, out DonorName);

                    if (DonorKey != -1)
                    {
                        stmtRow.DonorKey = DonorKey;
                        stmtRow.DonorShortName = DonorName;
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
        public void WritePetraImportFile(ref BankImportTDS AMainDS, string AFilename, string ABankName)
        {
            StreamWriter sw = new StreamWriter(AFilename, false, System.Text.Encoding.Default);

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetNumberOnStatementDBName();

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

                if (tr.IsEpMatchKeyNull())
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Should not get here; MatchKey is NULL in matched table; " + AFilename + " " + tr.AccountName);
                    continue;
                }

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
                        (match.MotivationGroupCode != "GIFT" && match.MotivationDetailCode != "SUPPORT" ? "" : match.RecipientShortName) + "\";" +
                        match.GiftTransactionAmount.ToString() +
                        ";" + (match.ConfidentialGiftFlag ? "yes" : "no") + ";\"" + match.MotivationGroupCode + "\";\"" +
                        match.MotivationDetailCode +
                        "\";\"" + match.GiftCommentOne + "\";\"" + match.CommentOneType + "\";\"" +
                        match.MailingCode + "\";\"" + match.GiftCommentTwo + "\";\"" + match.CommentTwoType + "\";\"" +
                        match.GiftCommentThree + "\";\"" + match.CommentThreeType + "\";yes");
                }
            }

            FSqliteDatabase.CloseDBConnection();

            FSqliteDatabase = null;

            sw.Close();
        }

        /// <summary>
        /// dump unmatched gifts to a CSV file
        /// </summary>
        public static void WritePetraImportFileUnmatched(ref BankImportTDS AMainDS, string AFilename, string ABankName)
        {
            StreamWriter sw = new StreamWriter(AFilename, false, System.Text.Encoding.Default);

            BankImportTDSAEpTransactionRow row;

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetNumberOnStatementDBName();

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                row = (BankImportTDSAEpTransactionRow)rv.Row;

                if (row.IsDonorKeyNull() || (row.DonorKey == -1))
                {
                    sw.WriteLine("27002909" +
                        ";" + ";\"Unbekannt\";\"\";\"" + ABankName + " " +
                        row.DateEffective.ToString("dd/MM/yyyy") +
                        "\";\"<none>\";" +
                        "0" + ";\"" +
                        "\";" +
                        row.TransactionAmount.ToString() +
                        ";" + "no" + ";\"EFS\";\"01" +
                        "\";\"" + row.AccountName + "\";\"both" +
                        "\";\"" +
                        "\";\"" + row.BankAccountNumber + " BLZ: " + row.BranchCode + "\";\"both\";\"" +
                        row.Description + "\";\"both" + "\";yes");
                }
                else
                {
                    sw.WriteLine(row.DonorKey.ToString() +
                        ";\"" + row.DonorShortName + "\";\"\";\"\";\"" + ABankName + " " +
                        row.DateEffective.ToString("dd/MM/yyyy") +
                        "\";\"<none>\";" +
                        "0" + ";\"" +
                        "\";" +
                        row.TransactionAmount.ToString() +
                        ";" + "no" + ";\"EFS\";\"01" +
                        "\";\"" + "\";\"both" + "\";\"" +
                        "\";\"" + "\";\"" + "both\";\"" +
                        row.Description + "\";\"both" + "\";yes");
                }
            }

            sw.Close();
        }

        /// <summary>
        /// dump unmatched gifts or other transactions to a CSV file
        /// </summary>
        public static void WriteCSVFile(ref BankImportTDS AMainDS, StreamWriter sw)
        {
            sw.WriteLine("#TransactionTypeCode;#AccountName;#BankAccountNumber;#BranchCode;#BIC;#IBAN;#Description;#TransactionAmount");

            BankImportTDSAEpTransactionRow row;

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetNumberOnStatementDBName();

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
            msg = msg.Replace("#PRINTDATE", DateTime.Now.ToShortDateString());

            // recognise detail lines automatically
            string RowTemplate;
            msg = TPrinterHtml.GetTableRow(msg, "#DESCRIPTION", out RowTemplate);
            string rowTexts = "";

            BankImportTDSAEpTransactionRow row = null;

            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetNumberOnStatementDBName();

            Decimal Sum = 0.0m;

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                row = (BankImportTDSAEpTransactionRow)rv.Row;

                string rowToPrint = RowTemplate;

                if (row.IsDonorKeyNull())
                {
                    rowToPrint = rowToPrint.Replace("#NAME", row.AccountName);
                }
                else
                {
                    rowToPrint = rowToPrint.Replace("#NAME", row.DonorShortName);
                }

                if (row.IsRecipientDescriptionNull() || (row.RecipientDescription.Length == 0))
                {
                    rowToPrint = rowToPrint.Replace("#DESCRIPTION", row.Description);
                }
                else
                {
                    rowToPrint = rowToPrint.Replace("#DESCRIPTION", row.RecipientDescription);
                }

                //                if (row.IsRecipientKeyNull() || row.RecipientKey <= 0)
                //                {
                //                      rowToPrint = rowToPrint.Replace("#RECIPIENTKEY", row.RecipientKey.ToString());
                //                }
                // TODO: print recipientkey
                rowToPrint = rowToPrint.Replace("#RECIPIENTKEY", "");

                if (row.IsDonorKeyNull() || (row.DonorKey <= 0))
                {
                    rowToPrint = rowToPrint.Replace("#DONORKEY", "");
                }
                else
                {
                    rowToPrint = rowToPrint.Replace("#DONORKEY", row.DonorKey.ToString());
                }

                rowTexts += rowToPrint.
                            Replace("#NRONSTATEMENT", row.NumberOnStatement.ToString()).
                            Replace("#AMOUNT", String.Format("{0:C}", row.TransactionAmount)).
                            Replace("#ACCOUNTNUMBER", row.BankAccountNumber).
                            Replace("#BANKSORTCODE", row.BranchCode);

                Sum += Convert.ToDecimal(row.TransactionAmount);
            }

            Sum = Math.Round(Sum, 2);

            return msg.Replace("#ROWTEMPLATE", rowTexts).Replace("#TOTALAMOUNT", String.Format("{0:C}", Sum));
        }

        private static Int64 GetDonorByBankAccountNumber(ref BankImportTDS AMainDS, string ABankAccountNumber)
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

        private static void MarkTransactionMatched(ref BankImportTDS AMainDS,
            ref BankImportTDSAEpTransactionRow stmtRow,
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
                    MarkTransactionMatched(ref AMainDS, ref stmtRow, (BankImportTDSAGiftDetailRow)rv.Row, false);
                }

                return;
            }

            giftDetail.AlreadyMatched = true;

            if (giftDetail.RecipientDescription.Length == 0)
            {
                giftDetail.RecipientDescription = giftDetail.MotivationGroupCode + "/" + giftDetail.MotivationDetailCode;
            }

            if (Convert.ToDecimal(stmtRow.TransactionAmount) == Convert.ToDecimal(giftDetail.GiftTransactionAmount))
            {
                // gift detail matches the whole transaction (or what is left of it)
                stmtRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                stmtRow.GiftLedgerNumber = giftDetail.LedgerNumber;
                stmtRow.GiftBatchNumber = giftDetail.BatchNumber;
                stmtRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
                stmtRow.GiftDetailNumber = giftDetail.DetailNumber;
                stmtRow.DonorShortName = giftDetail.DonorShortName;
                stmtRow.DonorKey = giftDetail.DonorKey;
                stmtRow.RecipientDescription = giftDetail.RecipientDescription;
            }
            else
            {
                // only parts of the bank transaction are matched by this gift detail
                // create a new transaction, and split the amounts
                BankImportTDSAEpTransactionRow newRow = AMainDS.AEpTransaction.NewRowTyped();
                newRow.StatementKey = stmtRow.StatementKey;
                newRow.Order = stmtRow.Order;
                newRow.NumberOnStatement = stmtRow.NumberOnStatement;
                newRow.DetailKey = giftDetail.DetailNumber;
                newRow.AccountName = stmtRow.AccountName;
                newRow.BankAccountNumber = stmtRow.BankAccountNumber;
                newRow.BranchCode = stmtRow.BranchCode;
                newRow.Description = stmtRow.Description;
                newRow.TransactionTypeCode = stmtRow.TransactionTypeCode;
                newRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                newRow.DonorShortName = giftDetail.DonorShortName;
                newRow.DonorKey = giftDetail.DonorKey;
                newRow.GiftLedgerNumber = giftDetail.LedgerNumber;
                newRow.GiftBatchNumber = giftDetail.BatchNumber;
                newRow.GiftTransactionNumber = giftDetail.GiftTransactionNumber;
                newRow.GiftDetailNumber = giftDetail.DetailNumber;
                newRow.TransactionAmount = giftDetail.GiftTransactionAmount;

                if (stmtRow.IsOriginalAmountOnStatementNull())
                {
                    stmtRow.OriginalAmountOnStatement = stmtRow.TransactionAmount;
                }

                newRow.OriginalAmountOnStatement = stmtRow.OriginalAmountOnStatement;
                stmtRow.TransactionAmount -= newRow.TransactionAmount;

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

        private static Decimal SumAmounts(ref BankImportTDS AMainDS, Int32 ASelectedGiftBatch, Int32 AGiftTransactionNumber, bool ACheckUnmatchedOnly)
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
        private static bool MatchTransactionsToGiftBatch(ref BankImportTDS AMainDS, Int32 ASelectedGiftBatch, bool APostedBatch)
        {
            bool newMatchFound = false;

            for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    // problem: what if bank account is used by several donors?
                    Int64 DonorKey = GetDonorByBankAccountNumber(ref AMainDS, stmtRow.BankAccountNumber);

                    // look for gifts that match the donor (identified by account number) and the transaction amount
                    AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
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
                        MarkTransactionMatched(ref AMainDS, ref stmtRow, (BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row, false);
                    }
                    else if (AMainDS.AGiftDetail.DefaultView.Count > 1)
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
                            newMatchFound = true;
                            MarkTransactionMatched(ref AMainDS, ref stmtRow, MaxRow, false);
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
                                                                    ASelectedGiftBatch.ToString() +
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
                                    if ((SumAmounts(ref AMainDS, ASelectedGiftBatch,
                                             detailrow.GiftTransactionNumber, true) == Convert.ToDecimal(stmtRow.TransactionAmount))
                                        || (Convert.ToDecimal(detailrow.GiftTransactionAmount) == Convert.ToDecimal(stmtRow.TransactionAmount)))
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
                                MarkTransactionMatched(ref AMainDS,
                                    ref stmtRow,
                                    matchingGiftDetail,
                                    Convert.ToDecimal(matchingGiftDetail.GiftTransactionAmount) != Convert.ToDecimal(stmtRow.TransactionAmount));
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

                    if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
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
                                && ((SumAmounts(ref AMainDS, ASelectedGiftBatch,
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
                            MarkTransactionMatched(ref AMainDS,
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
        /// try to find any posted or unposted gift batch and try to match gifts
        /// </summary>
        /// <returns>the gift batch number if gift batch has already been posted</returns>
        public static Int32 AutoMatchGiftsAgainstPetraDB(ref BankImportTDS AMainDS, DateTime ADateEffective)
        {
            // first stage: collect historic matches from Petra database
            // go through each transaction of the statement,
            // and see if you can find a donation on that date with the same amount from the same bank account
            // store this as a match

            Int32 SelectedGiftBatch = -1;

            // Get all gifts at given date
            TGetData.GetGiftsByDate(ref AMainDS, ADateEffective);

            // simple matching; no split gifts, bank account number fits and amount fits
            // problem: recipient different????
            Int32 CountMatches = 0;

            for (Int32 TransactionsCounter = 0; TransactionsCounter < AMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = AMainDS.AEpTransaction[TransactionsCounter];

                Int64 DonorKey = GetDonorByBankAccountNumber(ref AMainDS, stmtRow.BankAccountNumber);

                if (DonorKey == -1)
                {
                    continue;
                }

                AMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                            stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
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

            if ((SelectedGiftBatch == -1) || ((AMainDS.AEpTransaction.Rows.Count > 2) && (CountMatches < AMainDS.AEpTransaction.Rows.Count / 2)))
            {
                return -1;
            }

            AMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                        SelectedGiftBatch.ToString();

            bool postedBatch = ((BankImportTDSAGiftDetailRow)AMainDS.AGiftDetail.DefaultView[0].Row).BatchStatus == "Posted";

            while (MatchTransactionsToGiftBatch(ref AMainDS, SelectedGiftBatch, postedBatch))
            {
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
                DateTime dateEffective;
                ABankStatementImporter.ImportFromFile(filename, ref MainDS, out startBalance, out endBalance, out dateEffective, out bankName);

                if (AutoMatchGiftsAgainstPetraDB(ref MainDS, dateEffective) != -1)
                {
                    // move file to imported folder
                    string BackupName = OutputPath + Path.DirectorySeparatorChar + ALegalEntity + Path.DirectorySeparatorChar + "imported" +
                                        Path.DirectorySeparatorChar +
                                        Path.GetFileName(filename);
                    int CountBackup = 0;

                    while (File.Exists(BackupName))
                    {
                        BackupName = OutputPath + Path.DirectorySeparatorChar + ALegalEntity + Path.DirectorySeparatorChar + "imported" +
                                     Path.DirectorySeparatorChar +
                                     Path.GetFileNameWithoutExtension(filename) + "_" + CountBackup.ToString() + ".sta";
                        CountBackup++;
                    }

                    File.Move(filename, BackupName);
                }
            }
        }
    }
}