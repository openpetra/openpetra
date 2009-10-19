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
using Mono.Unix;
using Ict.Common;
using Ict.Plugins.Finance.SwiftParser;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    partial class TFrmMainForm
    {
        private IImportBankStatement FBankStatementImporter;
        private BankImportTDS FMainDS;
        private double FTotalAmountStatement;
        private Int32 FNumberAllTransactions;

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
                    grdResult.AddTextColumn("Account Number", FMainDS.AEpTransaction.ColumnBankAccountNumber);
                    grdResult.AddTextColumn("description", FMainDS.AEpTransaction.ColumnDescription);
                    grdResult.AddTextColumn("Transaction Amount", FMainDS.AEpTransaction.ColumnTransactionAmount);

                    // TODO: at the moment only support one statement by file?
                    FBankStatementImporter.ImportFromFile(DialogOpen.FileName, ref FMainDS, out FTotalAmountStatement, out FNumberAllTransactions);

                    AutoMatchGiftsAgainstPetraDB();

                    FillPanelInfo();

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
                FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + "= '" +
                                                               Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";
            }
            else if (rbtUnmatchedGifts.Checked)
            {
                FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " IS NULL";
            }
            else if (rbtOther.Checked)
            {
                // AEpTransactionTable.GetTransactionTypeCodeDBName() + " = '052'";
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

            foreach (AEpTransactionRow row in FMainDS.AEpTransaction.Rows)
            {
                if (row.MatchingStatus != Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED)
                {
                    TLogging.Log(AGiftDetailTable.GetGiftAmountDBName() + " = " + row.TransactionAmount.ToString() +
                        " AND " + BankImportTDSAGiftDetailTable.GetBankAccountNumberDBName() + " = '" + row.BankAccountNumber + "'");

                    FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetGiftAmountDBName() + " = " +
                                                                row.TransactionAmount.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                                                                " AND " + BankImportTDSAGiftDetailTable.GetBankAccountNumberDBName() + " = '" +
                                                                row.BankAccountNumber + "'";

                    if (FMainDS.AGiftDetail.DefaultView.Count == 1)
                    {
                        // found a match
                        row.MatchingStatus = Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED;
                    }
                    else
                    {
                        // TODO: split gifts
                        // check if total amount of gift details of same gift transaction is equal the transaction amount,
                        // or one gift is equal the transaction amount
                    }
                }
            }

            // second stage: use saved matches to find new matches in this statement
            // don't store them, but export them???

            return true;
        }

        private void FillPanelInfo()
        {
            double sumMatched = 0.0;

            FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + "= '" +
                                                           Ict.Petra.Shared.MFinance.MFinanceConstants.BANK_STMT_STATUS_MATCHED + "'";
            txtNumberMatched.Text = FMainDS.AEpTransaction.DefaultView.Count.ToString();

            foreach (DataRowView rv in FMainDS.AEpTransaction.DefaultView)
            {
                sumMatched += Math.Abs(((AEpTransactionRow)rv.Row).TransactionAmount);
            }

            txtValueMatchedGifts.Text = sumMatched.ToString();

            double sumUnmatched = 0.0;
            FMainDS.AEpTransaction.DefaultView.RowFilter = AEpTransactionTable.GetMatchingStatusDBName() + " IS NULL";
            txtNumberUnmatched.Text = FMainDS.AEpTransaction.DefaultView.Count.ToString();

            foreach (DataRowView rv in FMainDS.AEpTransaction.DefaultView)
            {
                sumUnmatched += Math.Abs(((AEpTransactionRow)rv.Row).TransactionAmount);
            }

            txtValueUnmatchedGifts.Text = sumUnmatched.ToString();

            FMainDS.AEpTransaction.DefaultView.RowFilter = "";

            double sumAll = 0.0;
            txtNumberAltogether.Text = FMainDS.AEpTransaction.DefaultView.Count.ToString();

            foreach (DataRowView rv in FMainDS.AEpTransaction.DefaultView)
            {
                sumAll += Math.Abs(((AEpTransactionRow)rv.Row).TransactionAmount);
            }

            txtValueAltogether.Text = sumAll.ToString();
        }
    }
}