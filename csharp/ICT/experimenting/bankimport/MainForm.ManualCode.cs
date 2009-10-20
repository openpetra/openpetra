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
            DialogOpen.FilterIndex = 3;
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

        private void MarkTransactionMatched(ref BankImportTDSAEpTransactionRow stmtRow, Int32 AGiftTransactionNumber)
        {
            stmtRow.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;

            foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
            {
                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                if ((detailrow.GiftTransactionNumber == AGiftTransactionNumber) && (detailrow.BatchNumber == FSelectedGiftBatch))
                {
                    stmtRow.GiftLedgerNumber = detailrow.LedgerNumber;
                    stmtRow.GiftBatchNumber = detailrow.BatchNumber;
                    stmtRow.GiftTransactionNumber = detailrow.GiftTransactionNumber;

                    if (detailrow.RecipientDescription.Length == 0)
                    {
                        detailrow.RecipientDescription = detailrow.MotivationGroupCode + "/" + detailrow.MotivationDetailCode;
                    }

                    if (stmtRow.RecipientDescription.Length > 0)
                    {
                        stmtRow.RecipientDescription += "; ";
                    }

                    stmtRow.RecipientDescription += detailrow.RecipientDescription + " (" + detailrow.GiftTransactionAmount.ToString() + ")";
                    stmtRow.DonorKey = detailrow.DonorKey;
                    stmtRow.DonorShortName = detailrow.DonorShortName;
                    detailrow.AlreadyMatched = true;
                }
            }
        }

        private bool AutoMatchGiftsAgainstPetraDB()
        {
            // first stage: collect historic matches from Petra database
            // go through each transaction of the statement,
            // and see if you can find a donation on that date with the same amount from the same bank account
            // store this as a match

            // Get all gifts at given date
            TGetData.GetGiftsByDate(ref FMainDS, FMainDS.AEpTransaction[0].DateEffective);

            // simple matching; no split gifts, bank account number fits and amount fits
            for (Int32 TransactionsCounter = 0; TransactionsCounter < FMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = FMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetBankAccountNumberDBName() + " = '" +
                                                                stmtRow.BankAccountNumber + "'";

                    if (FMainDS.AGiftDetail.DefaultView.Count == 1)
                    {
                        // found a match
                        BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)FMainDS.AGiftDetail.DefaultView[0].Row;
                        FSelectedGiftBatch = detailrow.BatchNumber;
                        MarkTransactionMatched(ref stmtRow, detailrow.GiftTransactionNumber);
                    }
                }
            }

            if (FSelectedGiftBatch == -1)
            {
                MessageBox.Show("TODO: there is no gift batch yet in Petra for this bank statement");
                return false;
            }

            for (Int32 TransactionsCounter = 0; TransactionsCounter < FMainDS.AEpTransaction.Rows.Count; TransactionsCounter++)
            {
                BankImportTDSAEpTransactionRow stmtRow = FMainDS.AEpTransaction[TransactionsCounter];

                if (stmtRow.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                stmtRow.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                                                +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetBankAccountNumberDBName() + " = '" +
                                                                stmtRow.BankAccountNumber + "' AND " +
                                                                BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                FSelectedGiftBatch.ToString() +
                                                                " AND AlreadyMatched = false";

                    if (FMainDS.AGiftDetail.DefaultView.Count > 1)
                    {
                        // TODO: donor has several gifts with same amount?
                        // look for fitting words in description
                        // gift transaction number, number of words
                        SortedList <Int32, Int32>NumberWordsMatching = new SortedList <Int32, Int32>();

                        foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
                        {
                            BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                            if (NumberWordsMatching.ContainsKey(detailrow.GiftTransactionNumber))
                            {
                                // either this is a split gift, or the donor has several bank accounts
                                // NumberWordsMatching[detailrow.GiftTransactionNumber] = -1000;
                            }
                            else
                            {
                                NumberWordsMatching.Add(detailrow.GiftTransactionNumber, 0);
                            }

                            StringCollection words =
                                StringHelper.StrSplit(Calculations.FormatShortName(detailrow.RecipientDescription,
                                        eShortNameFormat.eReverseWithoutTitle).Replace(", ", ",").Replace(" ", ","), ",");

                            foreach (string s in words)
                            {
                                if (stmtRow.Description.ToUpper().IndexOf(s.Trim().ToUpper()) > -1)
                                {
                                    NumberWordsMatching[detailrow.GiftTransactionNumber]++;
                                }
                            }
                        }

                        int MaxGiftNumber = -1;
                        int MaxCount = -1;

                        foreach (int key in NumberWordsMatching.Keys)
                        {
                            if (NumberWordsMatching[key] > MaxCount)
                            {
                                MaxCount = NumberWordsMatching[key];
                                MaxGiftNumber = key;
                            }
                        }

                        if (MaxCount > 0)
                        {
                            // found a match
                            MarkTransactionMatched(ref stmtRow, MaxGiftNumber);
                        }
                    }
                    else
                    {
                        // TODO: split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount

                        // get all gifts with that bank account number
                        FMainDS.AGiftDetail.DefaultView.RowFilter = BankImportTDSAGiftDetailTable.GetBankAccountNumberDBName() + " = '" +
                                                                    stmtRow.BankAccountNumber + "' AND " +
                                                                    BankImportTDSAGiftDetailTable.GetBatchNumberDBName() + " = " +
                                                                    FSelectedGiftBatch.ToString() +
                                                                    " AND AlreadyMatched = false";

                        if (FMainDS.AGiftDetail.DefaultView.Count > 1)
                        {
                            // key is the gift transaction number, double is the total amount
                            SortedList <Int32, double>TotalGifts = new SortedList <int, double>();

                            foreach (DataRowView rv in FMainDS.AGiftDetail.DefaultView)
                            {
                                BankImportTDSAGiftDetailRow detailrow = (BankImportTDSAGiftDetailRow)rv.Row;

                                if (TotalGifts.ContainsKey(detailrow.GiftTransactionNumber))
                                {
                                    TotalGifts[detailrow.GiftTransactionNumber] += detailrow.GiftAmount;
                                }
                                else
                                {
                                    TotalGifts.Add(detailrow.GiftTransactionNumber, detailrow.GiftAmount);
                                }
                            }

                            Int32 successTransactionNumber = -1;

                            foreach (Int32 key in TotalGifts.Keys)
                            {
                                if (TotalGifts[key] == stmtRow.TransactionAmount)
                                {
                                    if (successTransactionNumber > -1)
                                    {
                                        // TODO several gifts match this amount
                                        successTransactionNumber = -2;
                                    }
                                    else
                                    {
                                        successTransactionNumber = key;
                                    }
                                }
                            }

                            if (successTransactionNumber > -1)
                            {
                                // found a match
                                MarkTransactionMatched(ref stmtRow, successTransactionNumber);
                            }
                        }
                    }

                    if (FMainDS.AGiftDetail.DefaultView.Count == 0)
                    {
                        // try to find the donor by account number
                        // TODO: this should not be necessary anymore
                        string shortname;
                        Int64 donorkey = TGetData.GetDonorByAccountNumber(stmtRow.BankAccountNumber, out shortname);
                        stmtRow.DonorShortName = shortname;

                        if (donorkey != -1)
                        {
                            stmtRow.DonorKey = donorkey;
                        }
                    }
                }
            }

            // TODO: checksum of SelectedGiftBatch and matched transactions; move other transactions to Other state?

            // TODO: export a list of mismatching account numbers

            // second stage: use saved matches to find new matches in this statement
            // don't store them, but export them???

            return true;
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
    }
}