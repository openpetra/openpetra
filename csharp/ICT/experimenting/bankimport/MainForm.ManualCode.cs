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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Mono.Unix;
using Ict.Common;
using Ict.Plugins.Finance.SwiftParser;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    partial class TFrmMainForm
    {
        private IImportBankStatement FBankStatementImporter;
        private BankImportTDS FMainDS;

        private void InitializeManualCode()
        {
            FMainDS = new BankImportTDS();
            FBankStatementImporter = new TImportMT940();

            try
            {
                TGetData.InitDBConnection();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, Catalog.GetString("Error connecting to Petra 2.x"));
            }
        }

        private void ImportStatement(Object sender, EventArgs e)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString(FBankStatementImporter.GetFileFilter());
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Import bank statement file");

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // TODO: for some reason, the columns' initialisation in the constructor does not have any effect; need to do here again???
                    grdResult.Columns.Clear();
                    grdResult.AddTextColumn("transaction type", FMainDS.AEpTransaction.ColumnTransactionTypeCode);
                    grdResult.AddTextColumn("Account Name", FMainDS.AEpTransaction.ColumnAccountName);
                    grdResult.AddTextColumn("DonorKey", FMainDS.AEpTransaction.ColumnDonorKey);
                    grdResult.AddTextColumn("DonorShortName", FMainDS.AEpTransaction.ColumnDonorShortName);
                    grdResult.AddTextColumn("Account Number", FMainDS.AEpTransaction.ColumnBankAccountNumber);
                    grdResult.AddTextColumn("description", FMainDS.AEpTransaction.ColumnDescription);
                    grdResult.AddTextColumn("Recipient", FMainDS.AEpTransaction.ColumnRecipientDescription);
                    grdResult.AddTextColumn("Transaction Amount", FMainDS.AEpTransaction.ColumnTransactionAmount);

                    FMainDS.AEpTransaction.Rows.Clear();
                    FMainDS.AGiftDetail.Rows.Clear();
                    FMainDS.PBankingDetails.Rows.Clear();

                    // TODO: at the moment only support one statement by file?
                    double startBalance, endBalance;
                    string bankName;
                    FBankStatementImporter.ImportFromFile(DialogOpen.FileName, ref FMainDS, out startBalance, out endBalance, out bankName);

                    AutoMatchGiftsAgainstPetraDB();

                    FillPanelInfo(startBalance, endBalance, bankName);

                    rbtAllTransactions.Checked = true;

                    FMainDS.AEpTransaction.DefaultView.AllowNew = false;
                    grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AEpTransaction.DefaultView);
                    grdResult.AutoSizeCells();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message + Environment.NewLine + exp.StackTrace, Catalog.GetString("Error importing bank statement"));
                }
            }
        }

        private void SetFilterMatchingGifts()
        {
            FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + "= '" +
                                                           Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "' AND " +
                                                           AEpTransactionTable.GetTransactionAmountDBName().ToString(
                System.Globalization.CultureInfo.InvariantCulture) + " > 0";
        }

        private void SetFilterUnmatchedGifts()
        {
            FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " IS NULL AND " +
                                                           AEpTransactionTable.GetTransactionAmountDBName().ToString(
                System.Globalization.CultureInfo.InvariantCulture) + " > 0 AND (" +
                                                           FBankStatementImporter.GetFilterGifts() + ")";
        }

        private void SetFilterOther()
        {
            FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " IS NULL AND (" +
                                                           AEpTransactionTable.GetTransactionAmountDBName().ToString(
                System.Globalization.CultureInfo.InvariantCulture) + " < 0 OR NOT (" +
                                                           FBankStatementImporter.GetFilterGifts() + "))";
        }

        private void FilterChanged(Object sender, EventArgs e)
        {
            if (FMainDS == null)
            {
                return;
            }

            if (rbtAllTransactions.Checked)
            {
                FMainDS.AEpTransaction.DefaultView.RowFilter = "";
            }
            else if (rbtMatchedGifts.Checked)
            {
                SetFilterMatchingGifts();
            }
            else if (rbtUnmatchedGifts.Checked)
            {
                SetFilterUnmatchedGifts();
            }
            else if (rbtOther.Checked)
            {
                SetFilterOther();
            }

            //MessageBox.Show(FMainDS.AEpTransaction.DefaultView.RowFilter);
        }

        // determine the one gift batch that was posted for this bank statement
        private Int32 FSelectedGiftBatch = -1;

        private void MarkTransactionMatched(ref BankImportTDSAEpTransactionRow stmtRow, Int32 AGiftTransactionNumber, Int32 AGiftDetailNumber)
        {
            stmtRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;

//            if (stmtRow.AccountName.Contains("TEST"))
//            {
//                TLogging.Log(AGiftDetailNumber.ToString());
//                TLogging.LogStackTrace(TLoggingType.ToLogfile);
//            }

            foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                // check again for AlreadyMatched, the details might have been matched in a previous iteration of this loop
                if ((detailrow.AlreadyMatched == false) && (detailrow.GiftTransactionNumber == AGiftTransactionNumber)
                    && (detailrow.BatchNumber == FSelectedGiftBatch)
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
                    DataView v2 = new DataView(FMainDS.AGiftDetail);
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
        private void MarkTransactionMatched(ref BankImportTDSAEpTransactionRow stmtRow, BankImportTDSAGiftDetailRow giftDetail)
        {
            if (SumAmounts(giftDetail.GiftTransactionNumber) != Convert.ToDecimal(stmtRow.TransactionAmount))
            {
                // it seems that there are several different transactions treated as a split gift
                // don't mark the whole transactions as matched
                MarkTransactionMatched(ref stmtRow, giftDetail.GiftTransactionNumber, giftDetail.DetailNumber);
            }
            else
            {
                // found a match
                MarkTransactionMatched(ref stmtRow, giftDetail.GiftTransactionNumber, -1);
            }
        }

        private Int32 MatchingWords(string AShortname, string AFreeText)
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

        private Decimal SumAmounts(Int32 AGiftTransactionNumber)
        {
            Decimal Result = 0.0m;

            // attention: this does check even for already matched gift details
            DataView v = new DataView(FMainDS.AGiftDetail);

            v.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + " = " + FSelectedGiftBatch.ToString() +
                          " AND " + AGiftDetailTable.GetGiftTransactionNumberDBName() + " = " + AGiftTransactionNumber.ToString();

            foreach (DataRowView rv in v)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                Result += Convert.ToDecimal(detailrow.GiftTransactionAmount);
            }

            return Result;
        }

        private Int64 GetDonorByBankAccountNumber(string ABankAccountNumber)
        {
            FMainDS.PBankingDetails.DefaultView.RowFilter = BankImportTDSPBankingDetailsTable.GetBankAccountNumberDBName() +
                                                            " = '" + ABankAccountNumber + "'";

            if (FMainDS.PBankingDetails.DefaultView.Count > 0)
            {
                // TODO: just return the first partner key; usually not 2 people owning the same bank account donate at the same time???
                BankImportTDSPBankingDetailsRow row = (BankImportTDSPBankingDetailsRow)FMainDS.PBankingDetails.DefaultView[0].Row;
                return row.PartnerKey;
            }

            return -1;
        }

        private bool AutoMatchGiftsAgainstPetraDB()
        {
            // first stage: collect historic matches from Petra database
            // go through each transaction of the statement,
            // and see if you can find a donation on that date with the same amount from the same bank account
            // store this as a match

            FSelectedGiftBatch = -1;

            // Get all gifts at given date
            TGetData.GetGiftsByDate(ref FMainDS, FMainDS.AEpTransaction[0].DateEffective);

            // simple matching; no split gifts, bank account number fits and amount fits
            for (Int32 TransactionsCounter = 0; TransactionsCounter < FMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = FMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    Int64 DonorKey = GetDonorByBankAccountNumber(stmtRow.BankAccountNumber);

                    FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (FSelectedGiftBatch != -1)
                    {
                        FMainDS.AGiftDetail.DefaultView.RowFilter += " AND " + AGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                     FSelectedGiftBatch.ToString();
                    }

                    if (FMainDS.AGiftDetail.DefaultView.Count == 1)
                    {
                        // found a match
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)FMainDS.AGiftDetail.DefaultView[0].Row;
                        FSelectedGiftBatch = detailrow.BatchNumber;
                        MarkTransactionMatched(ref stmtRow, detailrow);
                    }
                }
            }

            if (FSelectedGiftBatch == -1)
            {
                MessageBox.Show("TODO: there is no gift batch yet in Petra for this bank statement");
                txtValueMatchedGiftBatch.Visible = false;
                return false;
            }

            for (Int32 TransactionsCounter = 0; TransactionsCounter < FMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = FMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    Int64 DonorKey = GetDonorByBankAccountNumber(stmtRow.BankAccountNumber);

                    // look for gifts that match the donor (identified by account number) and the transaction amount
                    FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                DonorKey.ToString() + " AND " +
                                                                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                FSelectedGiftBatch.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (FMainDS.AGiftDetail.DefaultView.Count > 1)
                    {
                        // donor has several gifts with same amount?
                        // look for fitting words in description
                        int MaxCount = -1;
                        BankImportTDSAGiftDetailRow MaxRow = null;

                        foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
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
                            MarkTransactionMatched(ref stmtRow, MaxRow);
                        }
                    }
                    else if (FMainDS.AGiftDetail.DefaultView.Count == 0)
                    {
                        // split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount

                        // get all gifts with that bank account number
                        DonorKey = GetDonorByBankAccountNumber(stmtRow.BankAccountNumber);

                        FMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetDonorKeyDBName() + " = " +
                                                                    DonorKey.ToString() + " AND " +
                                                                    BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    FSelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";

                        if (FMainDS.AGiftDetail.DefaultView.Count > 1)
                        {
                            BankImportTDSAGiftDetailRow matchingGiftDetail = null;
                            bool duplicateMatches = false;

                            foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
                            {
                                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                                if ((matchingGiftDetail == null) || (detailrow.GiftTransactionNumber != matchingGiftDetail.GiftTransactionNumber))
                                {
                                    if ((SumAmounts(detailrow.GiftTransactionNumber) == Convert.ToDecimal(stmtRow.TransactionAmount))
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
                                    MarkTransactionMatched(ref stmtRow, matchingGiftDetail.GiftTransactionNumber, matchingGiftDetail.DetailNumber);
                                }
                                else
                                {
                                    MarkTransactionMatched(ref stmtRow, matchingGiftDetail.GiftTransactionNumber, -1);
                                }
                            }
                        }
                    }
                }
            }

            // do another loop, now looking even harder for matching gifts; match donor name, and recipient name with transaction description
            // by now the list of unassigned gifts from the old gift batch should be quite small
            for (Int32 TransactionsCounter = 0; TransactionsCounter < FMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = FMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    FMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                FSelectedGiftBatch.ToString() +
                                                                " AND AlreadyMatched = false";
                    BankImportTDSAGiftDetailRow BestMatch = null;
                    int BestMatchNumber = 0;

                    foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
                    {
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                        int matchNumber = MatchingWords(detailrow.DonorShortName, stmtRow.AccountName) +
                                          MatchingWords(detailrow.RecipientDescription, stmtRow.Description);

                        if ((matchNumber > BestMatchNumber)
                            && ((SumAmounts(detailrow.GiftTransactionNumber) == Convert.ToDecimal(stmtRow.TransactionAmount))
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
                            MarkTransactionMatched(ref stmtRow, BestMatch.GiftTransactionNumber, BestMatch.DetailNumber);
                        }
                        else
                        {
                            // match full gift
                            MarkTransactionMatched(ref stmtRow, BestMatch.GiftTransactionNumber, -1);
                        }
                    }
                }
            }

            // TODO: checksum of SelectedGiftBatch and matched transactions; move other transactions to Other state?

            // log all gifts in the gift batch that have not been matched
            FMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                        FSelectedGiftBatch.ToString() +
                                                        " AND AlreadyMatched = false";
            double SumUnmatched = 0.0;
            double SumMatched = 0.0;

            if (FMainDS.AGiftDetail.DefaultView.Count > 0)
            {
                TLogging.Log("The following gifts in batch " + FSelectedGiftBatch.ToString() + " have not been matched: ");

                foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
                {
                    BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                    TLogging.Log(
                        detailrow.GiftTransactionAmount.ToString() + "; " + detailrow.DonorShortName + " " + detailrow.DonorKey.ToString() + " " +
                        detailrow.RecipientDescription);
                    SumUnmatched += detailrow.GiftTransactionAmount;
                }
            }

            FMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                        FSelectedGiftBatch.ToString() +
                                                        " AND AlreadyMatched = true";

            foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;
                SumMatched += detailrow.GiftTransactionAmount;
            }

            // Test: SumMatched should be the same as SumCredit of matched gift view
            Int32 countRows;
            SetFilterMatchingGifts();
            double sumCreditMatched, sumDebitMatched;
            CalculateSumsFromTransactionView(out sumCreditMatched, out sumDebitMatched, out countRows);

            txtValueMatchedGiftBatch.Visible = false;

            if (Convert.ToDecimal(sumCreditMatched) != Convert.ToDecimal(SumMatched))
            {
                txtValueMatchedGiftBatch.Visible = true;
                txtValueMatchedGiftBatch.Text = SumMatched.ToString();
                txtValueMatchedGiftBatch.BackColor = System.Drawing.Color.Red;
                TLogging.Log(String.Format("Sum of matched gift details: {0}; sum of unmatched gifts: {1}; value of gift batch {2}: {3}", SumMatched,
                        SumUnmatched, FSelectedGiftBatch, SumMatched + SumUnmatched));
                MessageBox.Show(String.Format(Catalog.GetString(
                            "There is a problem: matched gifts from gift batch are {0}, but matched gifts from bank statement are {1}"), SumMatched,
                        sumCreditMatched));
            }

            // TODO: export a list of mismatching account numbers

            TGiftMatching matchGifts = new TGiftMatching();
            matchGifts.StoreCurrentMatches(ref FMainDS, FSelectedGiftBatch);

            return true;
        }

        /// use saved matches to match the gifts in this statement
        private void FindMatches()
        {
//if (FMainDS.AGiftDetail.DefaultView.Count == 0)
//{
//    // try to find the donor by account number
//    // TODO: this should not be necessary anymore
//    string shortname;
//    Int64 donorkey = TGetData.GetDonorByAccountNumber(stmtRow.BankAccountNumber, out shortname);
//    stmtRow.DonorShortName = shortname;
//
//    if (donorkey != -1)
//    {
//        stmtRow.DonorKey = donorkey;
//    }
//}
        }

        private void CalculateSumsFromTransactionView(out double ASumCredit, out double ASumDebit, out Int32 ACount)
        {
            ASumCredit = 0.0;
            ASumDebit = 0.0;

            ACount = FMainDS.AEpTransaction.DefaultView.Count;

            foreach (DataRowView rv in FMainDS.AEpTransaction.DefaultView)
            {
                double amount = ((AEpTransactionRow)rv.Row).TransactionAmount;

                if (amount > 0)
                {
                    ASumCredit += amount;
                }
                else
                {
                    ASumDebit += amount;
                }
            }
        }

        private void FillPanelInfo(double startBalance, double endBalance, string ABankName)
        {
            Int32 countRows;

            txtBankName.Text = ABankName;
            txtDateStatement.Text = FMainDS.AEpTransaction[0].DateEffective.ToShortDateString();
            txtStartBalance.Text = startBalance.ToString();
            txtEndBalance.Text = endBalance.ToString();

            SetFilterMatchingGifts();
            double sumCreditMatched, sumDebitMatched;
            CalculateSumsFromTransactionView(out sumCreditMatched, out sumDebitMatched, out countRows);
            txtNumberMatched.Text = countRows.ToString();
            txtValueMatchedGifts.Text = sumCreditMatched.ToString();

            if (sumDebitMatched > 0)
            {
                MessageBox.Show(Catalog.GetString("Problems with import, there should be no debits in gifts"));
            }

            SetFilterUnmatchedGifts();
            double sumCreditUnmatched, sumDebitUnmatched;
            CalculateSumsFromTransactionView(out sumCreditUnmatched, out sumDebitUnmatched, out countRows);
            txtNumberUnmatched.Text = countRows.ToString();
            txtValueUnmatchedGifts.Text = sumCreditUnmatched.ToString();

            if (sumDebitUnmatched > 0)
            {
                MessageBox.Show(Catalog.GetString("Problems with import, there should be no debits in gifts"));
            }

            SetFilterOther();
            double sumCreditOther, sumDebitOther;
            CalculateSumsFromTransactionView(out sumCreditOther, out sumDebitOther, out countRows);
            txtNumberOther.Text = countRows.ToString();
            txtValueOtherCredit.Text = sumCreditOther.ToString();
            txtValueOtherDebit.Text = sumDebitOther.ToString();

            FMainDS.AEpTransaction.DefaultView.RowFilter = "";
            double sumCreditAll, sumDebitAll;
            CalculateSumsFromTransactionView(out sumCreditAll, out sumDebitAll, out countRows);
            txtNumberAltogether.Text = countRows.ToString();
            txtSumCredit.Text = sumCreditAll.ToString();
            txtSumDebit.Text = sumDebitAll.ToString();

            if (Convert.ToDecimal(startBalance + sumCreditAll + sumDebitAll) != Convert.ToDecimal(endBalance))
            {
                MessageBox.Show(Catalog.GetString("the startbalance, credit/debit all and endbalance don't add up"));
            }

            if (Convert.ToDecimal(sumCreditAll) != Convert.ToDecimal(sumCreditMatched + sumCreditUnmatched + sumCreditOther))
            {
                MessageBox.Show(Catalog.GetString("the credits don't add up"));
            }

            if (Convert.ToDecimal(sumDebitAll) != Convert.ToDecimal(sumDebitMatched + sumDebitUnmatched + sumDebitOther))
            {
                MessageBox.Show(Catalog.GetString("the debits don't add up"));
            }
        }

        private void ExportGiftBatch(object sender, EventArgs e)
        {
            rbtMatchedGifts.Checked = true;

            if (FMainDS.AEpTransaction.DefaultView.Count == 0)
            {
                return;
            }

            SaveFileDialog DialogSave = new SaveFileDialog();

            DialogSave.Filter = Catalog.GetString("Gift Batch file (*.csv)|*.csv");
            DialogSave.AddExtension = true;
            DialogSave.Title = Catalog.GetString("Export gift batch of matched gifts");

            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(DialogSave.FileName, false, System.Text.Encoding.Default);

                BankImportTDSAEpTransactionRow row = (BankImportTDSAEpTransactionRow)FMainDS.AEpTransaction.DefaultView[0].Row;

                sw.WriteLine(
                    "\"B\";\"" + txtBankName.Text + " " + row.DateEffective.ToShortDateString() + "\";\"6205\";0;" +
                    row.DateEffective.ToString("dd/MM/yyyy") + ";\"EUR\";1;\"2700\";\"Gift\"");
                sw.WriteLine();

//#if DISABLED
                FMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetDonorShortNameDBName() + "," +
                                                          BankImportTDSAEpTransactionTable.GetRecipientDescriptionDBName();

                foreach (DataRowView rv in FMainDS.AEpTransaction.DefaultView)
                {
                    row = (BankImportTDSAEpTransactionRow)rv.Row;

                    FMainDS.AGiftDetail.DefaultView.RowFilter = TGiftMatching.FilterForMatchedGiftTransactions(row, FSelectedGiftBatch);

                    if (FMainDS.AGiftDetail.DefaultView.RowFilter.Length > 0)
                    {
                        Decimal sumExport = 0.0m;

                        foreach (DataRowView gv in FMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;

                            sw.WriteLine("\"T\";" + giftRow.DonorKey.ToString() +
                                ";\"" + giftRow.DonorShortName + "\";\"\";\"\";\"" + txtBankName.Text + " " +
                                row.DateEffective.ToString("dd/MM/yyyy") +
                                "\";\"<none>\";" +
                                giftRow.RecipientKey.ToString() + ";\"" +
                                giftRow.RecipientDescription + "\";" +
                                giftRow.GiftTransactionAmount.ToString() +
                                ";no;\"" + giftRow.MotivationGroupCode + "\";\"" +
                                giftRow.MotivationDetailCode +
                                "\";\"\";\"Both\";\"\";\"\";\"\";\"\";\"\";yes");
                            sumExport += Convert.ToDecimal(giftRow.GiftTransactionAmount);
                        }

                        if (sumExport != Convert.ToDecimal(row.TransactionAmount))
                        {
                            TLogging.Log(
                                "problem " + row.DonorShortName + " " + row.RecipientDescription + " " + sumExport.ToString() + " " +
                                row.TransactionAmount.ToString());
                        }
                    }
                }

//#endif
#if DISABLED
                FMainDS.AGiftDetail.DefaultView.RowFilter =
                    BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                    FSelectedGiftBatch.ToString() +
                    " AND AlreadyMatched = true";
                FMainDS.AGiftDetail.DefaultView.Sort = BankImportTDSAGiftDetailTable.GetDonorShortNameDBName() + "," +
                                                       BankImportTDSAGiftDetailTable.GetRecipientDescriptionDBName();

                foreach (DataRowView gv in FMainDS.AGiftDetail.DefaultView)
                {
                    BankImportTDSAGiftDetailRow giftRow = (BankImportTDSAGiftDetailRow)gv.Row;

                    sw.WriteLine("\"T\";" + giftRow.DonorKey.ToString() +
                        ";\"" + giftRow.DonorShortName + "\";\"\";\"\";\"EKK 090909\";\"<none>\";" +
                        giftRow.RecipientKey.ToString() + ";\"" +
                        giftRow.RecipientDescription + "\";" +
                        giftRow.GiftTransactionAmount.ToString() +
                        ";no;\"" + giftRow.MotivationGroupCode + "\";\"" +
                        giftRow.MotivationDetailCode +
                        "\";\"\";\"Both\";\"\";\"\";\"\";\"\";\"\";yes");
                }
#endif

                sw.Close();
            }
        }
    }
}