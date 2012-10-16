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
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Common.Printing;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Gift;

namespace Ict.Petra.Client.MFinance.Gui.Common
{
    /// manual methods for the generated window
    public partial class TFrmBankStatementImport
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                cmbBankAccount.Enabled = false;

                ALedgerRow Ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
                txtCreditSum.CurrencySymbol = Ledger.BaseCurrency;
                txtDebitSum.CurrencySymbol = Ledger.BaseCurrency;
                txtAmount.CurrencySymbol = Ledger.BaseCurrency;

                pnlDetails.Visible = false;
            }
        }

        private DataView FMatchView = null;
        private DataView FTransactionView = null;

        private void RunOnceOnActivationManual()
        {
            TFrmSelectBankStatement DlgSelect = new TFrmSelectBankStatement(FPetraUtilsObject.GetCallerForm());

            DlgSelect.LedgerNumber = FLedgerNumber;

            if (DlgSelect.ShowDialog() == DialogResult.OK)
            {
                SelectBankStatement(DlgSelect.StatementKey);
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// select the bank statement that should be loaded
        /// </summary>
        /// <param name="AStatementKey"></param>
        private void SelectBankStatement(Int32 AStatementKey)
        {
            CurrentlySelectedMatch = null;
            CurrentStatement = null;

            // merge the cost centres and the motivation details from the cacheable tables
            FMainDS.ACostCentre.Merge(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, FLedgerNumber));
            FMainDS.AMotivationDetail.Merge(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, FLedgerNumber));

            // load the transactions of the selected statement, and the matches
            FMainDS.Merge(
                TRemote.MFinance.ImportExport.WebConnectors.GetBankStatementTransactionsAndMatches(
                    AStatementKey, FLedgerNumber));

            // an old version of the CSV import plugin did not set the potential gift typecode
            foreach (AEpTransactionRow r in FMainDS.AEpTransaction.Rows)
            {
                if (r.IsTransactionTypeCodeNull() && (r.TransactionAmount > 0))
                {
                    r.TransactionTypeCode = MFinanceConstants.BANK_STMT_POTENTIAL_GIFT;
                }
            }

            CurrentStatement = (AEpStatementRow)FMainDS.AEpStatement[0];

            grdAllTransactions.Columns.Clear();
            grdAllTransactions.AddTextColumn(Catalog.GetString("Nr"), FMainDS.AEpTransaction.ColumnNumberOnPaperStatement, 40);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Account Name"), FMainDS.AEpTransaction.ColumnAccountName, 150);
            grdAllTransactions.AddTextColumn(Catalog.GetString("description"), FMainDS.AEpTransaction.ColumnDescription, 200);
            grdAllTransactions.AddDateColumn(Catalog.GetString("Date Effective"), FMainDS.AEpTransaction.ColumnDateEffective);
            grdAllTransactions.AddCurrencyColumn(Catalog.GetString("Transaction Amount"), FMainDS.AEpTransaction.ColumnTransactionAmount);

            FTransactionView = FMainDS.AEpTransaction.DefaultView;
            FTransactionView.AllowNew = false;
            FTransactionView.Sort = AEpTransactionTable.GetOrderDBName() + " ASC";
            grdAllTransactions.DataSource = new DevAge.ComponentModel.BoundDataView(FTransactionView);

            TFinanceControls.InitialiseMotivationGroupList(ref cmbMotivationGroup, FLedgerNumber, true);
            TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetail, FLedgerNumber, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbGLCostCentre, FLedgerNumber, true, false, true, true);
            TFinanceControls.InitialiseAccountList(ref cmbGLAccount, FLedgerNumber, true, false, true, false);

            grdGiftDetails.Columns.Clear();
            grdGiftDetails.AddTextColumn(Catalog.GetString("Motivation"), FMainDS.AEpMatch.ColumnMotivationDetailCode, 100);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre"), FMainDS.AEpMatch.ColumnCostCentreCode, 150);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre Name"), FMainDS.AEpMatch.ColumnCostCentreName, 200);
            grdGiftDetails.AddCurrencyColumn(Catalog.GetString("Amount"), FMainDS.AEpMatch.ColumnGiftTransactionAmount);
            FMatchView = FMainDS.AEpMatch.DefaultView;
            FMatchView.AllowNew = false;
            grdGiftDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMatchView);

            TFinanceControls.InitialiseAccountList(ref cmbBankAccount, FLedgerNumber, true, false, true, true);

            if (CurrentStatement != null)
            {
                FMainDS.AEpStatement.DefaultView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey);
                cmbBankAccount.SetSelectedString(CurrentStatement.BankAccountCode);
                txtBankStatement.Text = CurrentStatement.Filename;
                dtpBankStatementDate.Date = CurrentStatement.Date;
                FMainDS.AEpStatement.DefaultView.RowFilter = string.Empty;
            }

            TransactionFilterChanged(null, null);
            grdAllTransactions.SelectRowInGrid(1);
        }

        private AMotivationDetailRow GetCurrentMotivationDetail(string AMotivationGroupCode, string AMotivationDetailCode)
        {
            return (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, AMotivationGroupCode, AMotivationDetailCode });
        }

        private void FilterMotivationDetail(object sender, EventArgs e)
        {
            TFinanceControls.ChangeFilterMotivationDetailList(ref cmbMotivationDetail, cmbMotivationGroup.GetSelectedString());
        }

        private void SetRecipientCostCentreAndField()
        {
            // look for the motivation detail.
            AMotivationDetailRow motivationDetailRow = GetCurrentMotivationDetail(
                cmbMotivationGroup.GetSelectedString(),
                cmbMotivationDetail.GetSelectedString());

            if (motivationDetailRow != null)
            {
                txtGiftAccount.Text = motivationDetailRow.AccountCode;
                txtGiftCostCentre.Text = motivationDetailRow.CostCentreCode;
            }
            else
            {
                txtGiftAccount.Text = string.Empty;
                txtGiftCostCentre.Text = string.Empty;
            }

            Int64 RecipientKey = Convert.ToInt64(txtRecipientKey.Text);
            FInKeyMinistryChanging++;
            TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, RecipientKey);
            FInKeyMinistryChanging--;

            long FieldNumber = Convert.ToInt64(txtField.Text);

            txtGiftCostCentre.Text = TRemote.MFinance.Gift.WebConnectors.IdentifyPartnerCostCentre(FLedgerNumber, FieldNumber);
        }

        private void MotivationDetailChanged(System.Object sender, EventArgs e)
        {
            SetRecipientCostCentreAndField();
        }

        Int16 FInKeyMinistryChanging = 0;
        private void KeyMinistryChanged(object sender, EventArgs e)
        {
            if ((FInKeyMinistryChanging > 0) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FInKeyMinistryChanging++;
            try
            {
                Int64 rcp = cmbMinistry.GetSelectedInt64();

                txtRecipientKey.Text = String.Format("{0:0000000000}", rcp);
            }
            finally
            {
                FInKeyMinistryChanging--;
            }
        }

        private void NewTransactionCategory(System.Object sender, EventArgs e)
        {
            // do NOT call GetValuesFromScreen to avoid disappearing transaction from the grid
            // GetValuesFromScreen();
            CurrentlySelectedMatch = null;

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;

            pnlGiftEdit.Visible = rbtGift.Checked;
            pnlGLEdit.Visible = rbtGL.Checked;

            if (rbtGift.Checked)
            {
                // select first detail
                grdGiftDetails.SelectRowInGrid(1);
                AEpMatchRow match = GetSelectedMatch();
                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());
            }

            if (rbtGL.Checked)
            {
                DisplayGLDetails();
            }

            if (rbtUnmatched.Checked && (FMatchView != null))
            {
                foreach (DataRowView rv in FMatchView)
                {
                    ((AEpMatchRow)rv.Row).Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                    if (CurrentlySelectedTransaction.EpMatchKey != ((AEpMatchRow)rv.Row).EpMatchKey)
                    {
                        ((AEpMatchRow)rv.Row).Delete();
                    }
                }
            }

            if (rbtIgnored.Checked && (FMatchView != null))
            {
                foreach (DataRowView rv in FMatchView)
                {
                    ((AEpMatchRow)rv.Row).Action = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;

                    if (CurrentlySelectedTransaction.EpMatchKey != ((AEpMatchRow)rv.Row).EpMatchKey)
                    {
                        ((AEpMatchRow)rv.Row).Delete();
                    }
                }
            }
        }

        private AEpStatementRow CurrentStatement = null;
        private BankImportTDSAEpTransactionRow CurrentlySelectedTransaction = null;
        private BankImportTDSAEpMatchRow CurrentlySelectedMatch = null;
        private bool rbtGLWasChecked = false;
        private bool rbtGiftWasChecked = false;

        /// store current selections in the a_ep_match table
        private void GetValuesFromScreen()
        {
            if (CurrentStatement != null)
            {
                CurrentStatement.LedgerNumber = FLedgerNumber;
            }

            if (CurrentlySelectedTransaction == null)
            {
                return;
            }

            if (rbtUnmatched.Checked)
            {
                if (FMatchView != null)
                {
                    for (int i = 0; i < FMatchView.Count; i++)
                    {
                        AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                        match.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                    }
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
            }

            if (rbtIgnored.Checked)
            {
                if (FMatchView != null)
                {
                    for (int i = 0; i < FMatchView.Count; i++)
                    {
                        AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                        match.Action = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;
                    }
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;
            }

            if (CurrentlySelectedMatch == null)
            {
                return;
            }

            if (rbtGiftWasChecked)
            {
                for (int i = 0; i < FMatchView.Count; i++)
                {
                    AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                    match.DonorKey = Convert.ToInt64(txtDonorKey.Text);
                    match.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;

                GetGiftDetailValuesFromScreen();

                // TODO: validation> calculate the sum of the gift details and check with the bank transaction amount
            }

            if (rbtGLWasChecked)
            {
                CurrentlySelectedMatch.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;
                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;

                GetGLValuesFromScreen();
            }
        }

        private void AllTransactionsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            pnlDetails.Visible = true;

            GetValuesFromScreen();

            CurrentlySelectedMatch = null;

            try
            {
                CurrentlySelectedTransaction = ((BankImportTDSAEpTransactionRow)grdAllTransactions.SelectedDataRowsAsDataRowView[0].Row);
            }
            catch (System.IndexOutOfRangeException)
            {
                // this can happen when the transaction type has changed, and the row disappears from the grid
                // select another row
                grdAllTransactions.SelectRowInGrid(1);
                return;
            }

            // load selections from the a_ep_match table for the new row
            FMatchView.RowFilter = AEpMatchTable.GetMatchTextDBName() +
                                   " = '" + CurrentlySelectedTransaction.MatchText + "'";

            AEpMatchRow match = (AEpMatchRow)FMatchView[0].Row;

            if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT)
            {
                rbtGift.Checked = true;

                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());

                grdGiftDetails.SelectRowInGrid(1);
                grdAllTransactions.Focus();
            }
            else if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL)
            {
                rbtGL.Checked = true;
                DisplayGLDetails();
            }
            else if (match.Action == MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING)
            {
                rbtIgnored.Checked = true;
            }
            else
            {
                rbtUnmatched.Checked = true;
            }

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;
        }

        private void GiftDetailsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            GetGiftDetailValuesFromScreen();
            CurrentlySelectedMatch = GetSelectedMatch();
            DisplayGiftDetails();
        }

        private BankImportTDSAEpMatchRow GetSelectedMatch()
        {
            DataRowView[] SelectedGridRow = grdGiftDetails.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (BankImportTDSAEpMatchRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        private void DisplayGiftDetails()
        {
            CurrentlySelectedMatch = GetSelectedMatch();

            if (CurrentlySelectedMatch != null)
            {
                txtAmount.NumberValueDecimal = CurrentlySelectedMatch.GiftTransactionAmount;
                txtRecipientKey.Text = StringHelper.FormatStrToPartnerKeyString(CurrentlySelectedMatch.RecipientKey.ToString());

                if (CurrentlySelectedMatch.IsMotivationGroupCodeNull())
                {
                    cmbMotivationGroup.SelectedIndex = -1;
                }
                else
                {
                    cmbMotivationGroup.SetSelectedString(CurrentlySelectedMatch.MotivationGroupCode);
                }

                if (CurrentlySelectedMatch.IsMotivationDetailCodeNull())
                {
                    cmbMotivationDetail.SelectedIndex = -1;
                }
                else
                {
                    cmbMotivationDetail.SetSelectedString(CurrentlySelectedMatch.MotivationDetailCode);
                }

                SetRecipientCostCentreAndField();
            }
        }

        private void GetGiftDetailValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                CurrentlySelectedMatch.MotivationGroupCode = cmbMotivationGroup.GetSelectedString();
                CurrentlySelectedMatch.MotivationDetailCode = cmbMotivationDetail.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = txtGiftCostCentre.Text;
                CurrentlySelectedMatch.GiftTransactionAmount = txtAmount.NumberValueDecimal.Value;
                CurrentlySelectedMatch.DonorKey = Convert.ToInt64(txtDonorKey.Text);
                CurrentlySelectedMatch.RecipientKey = Convert.ToInt64(txtRecipientKey.Text);

                FMainDS.ACostCentre.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    ACostCentreTable.GetCostCentreCodeDBName(), CurrentlySelectedMatch.CostCentreCode);

                if (FMainDS.ACostCentre.DefaultView.Count == 1)
                {
                    CurrentlySelectedMatch.CostCentreName = ((ACostCentreRow)FMainDS.ACostCentre.DefaultView[0].Row).CostCentreName;
                }

                FMainDS.ACostCentre.DefaultView.RowFilter = "";
            }
        }

        private void DisplayGLDetails()
        {
            // there is only one match?
            // TODO: support split GL transactions
            CurrentlySelectedMatch = (BankImportTDSAEpMatchRow)FMatchView[0].Row;

            if (CurrentlySelectedMatch != null)
            {
                if (CurrentlySelectedMatch.IsAccountCodeNull())
                {
                    cmbGLAccount.SelectedIndex = -1;
                }
                else
                {
                    cmbGLAccount.SetSelectedString(CurrentlySelectedMatch.AccountCode);
                }

                if (CurrentlySelectedMatch.IsCostCentreCodeNull())
                {
                    cmbGLCostCentre.SelectedIndex = -1;
                }
                else
                {
                    cmbGLCostCentre.SetSelectedString(CurrentlySelectedMatch.CostCentreCode);
                }

                if (!CurrentlySelectedMatch.IsReferenceNull())
                {
                    txtGLReference.Text = CurrentlySelectedMatch.Reference;
                }

                if (CurrentlySelectedMatch.IsNarrativeNull())
                {
                    txtGLNarrative.Text = CurrentlySelectedTransaction.Description;
                }
                else
                {
                    txtGLNarrative.Text = CurrentlySelectedMatch.Narrative;
                }
            }
        }

        private void GetGLValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                CurrentlySelectedMatch.AccountCode = cmbGLAccount.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = cmbGLCostCentre.GetSelectedString();
                CurrentlySelectedMatch.Reference = txtGLReference.Text;
                CurrentlySelectedMatch.Narrative = txtGLNarrative.Text;
            }
        }

        private Int32 NewMatchKey = -1;

        private void AddGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // get a new detail number
            Int32 newDetailNumber = 0;
            decimal amount = 0;
            AEpMatchRow match = null;

            for (int i = 0; i < FMatchView.Count; i++)
            {
                match = (AEpMatchRow)FMatchView[i].Row;

                if (match.Detail >= newDetailNumber)
                {
                    newDetailNumber = match.Detail + 1;
                }

                amount += match.GiftTransactionAmount;
            }

            if (match != null)
            {
                AEpMatchRow newRow = FMainDS.AEpMatch.NewRowTyped();
                newRow.EpMatchKey = NewMatchKey--;
                newRow.MatchText = match.MatchText;
                newRow.Detail = newDetailNumber;
                newRow.LedgerNumber = match.LedgerNumber;
                newRow.AccountCode = match.AccountCode;
                newRow.CostCentreCode = match.CostCentreCode;
                newRow.DonorKey = match.DonorKey;
                newRow.GiftTransactionAmount = CurrentlySelectedTransaction.TransactionAmount -
                                               amount;
                FMainDS.AEpMatch.Rows.Add(newRow);

                // select the new gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        private void RemoveGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            if (CurrentlySelectedMatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a row before deleting a detail"));
                return;
            }

            // we should never allow to delete all details, otherwise we have nothing to copy from
            // also cannot delete the first detail, since there is the foreign key from a_ep_transaction on epmatchkey?
            if (CurrentlySelectedTransaction.EpMatchKey == CurrentlySelectedMatch.EpMatchKey)
            {
                MessageBox.Show(Catalog.GetString("Cannot delete the first detail"));
            }
            else
            {
                CurrentlySelectedMatch.Delete();
                CurrentlySelectedMatch = null;

                // select the last gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        /// <summary>
        /// save the matches
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            GetValuesFromScreen();

            if (TRemote.MFinance.ImportExport.WebConnectors.CommitMatches(FMainDS.GetChangesTyped(true)))
            {
                FMainDS.AcceptChanges();
                return true;
            }
            else
            {
                MessageBox.Show(Catalog.GetString(
                        "The matches could not be stored. Please ask your System Administrator to check the log file on the server."));
                return false;
            }
        }

        private void CreateGiftBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveChanges();

            TVerificationResultCollection VerificationResult;
            Int32 GiftBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGiftBatch(FMainDS,
                FLedgerNumber,
                CurrentStatement.StatementKey,
                -1,
                out VerificationResult);

            if (GiftBatchNumber != -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Please check Gift Batch {0}"), GiftBatchNumber));
            }
            else
            {
                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
                else
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
            }
        }

        private void CreateGLBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveChanges();

            TVerificationResultCollection VerificationResult;
            Int32 GLBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGLBatch(FMainDS,
                FLedgerNumber,
                CurrentStatement.StatementKey,
                -1,
                out VerificationResult);

            if (GLBatchNumber != -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Please check GL Batch {0}"), GLBatchNumber));
            }
            else
            {
                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No GL batch has been created"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Problem: No GL batch has been created"),
                        Catalog.GetString("Error"));
                }
            }
        }

        /// <summary>
        /// this is useful for the situation, where we are using OpenPetra only for the bankimport,
        /// but need to post the gift batches in the old Petra 2.x database
        /// </summary>
        private void ExportGiftBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveChanges();

            TVerificationResultCollection VerificationResult;
            Int32 GiftBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGiftBatch(FMainDS,
                FLedgerNumber,
                CurrentStatement.StatementKey,
                -1,
                out VerificationResult);

            if (GiftBatchNumber != -1)
            {
                if ((VerificationResult != null) && (VerificationResult.Count > 0))
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Info: gift batch has been created"));
                }

                // export to csv
                TFrmGiftBatchExport exportForm = new TFrmGiftBatchExport(FPetraUtilsObject.GetForm());
                exportForm.LedgerNumber = FLedgerNumber;
                exportForm.FirstBatchNumber = GiftBatchNumber;
                exportForm.LastBatchNumber = GiftBatchNumber;
                exportForm.IncludeUnpostedBatches = true;
                exportForm.TransactionsOnly = true;
                exportForm.ExtraColumns = false;
                exportForm.OutputFilename = TAppSettingsManager.GetValue("BankImport.GiftBatchExportFilename",
                    TAppSettingsManager.GetValue("OpenPetra.PathTemp") +
                    Path.DirectorySeparatorChar +
                    "giftBatch" + GiftBatchNumber.ToString("000000") + ".csv");
                exportForm.ExportBatches(null, null);
            }
            else
            {
                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
                else
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
            }
        }

        private void PrintReport(System.Object sender, EventArgs e)
        {
            if (FMainDS.AEpTransaction.DefaultView.Count == 0)
            {
                return;
            }

            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            bool PrinterInstalled = doc.PrinterSettings.IsValid;

            if (!PrinterInstalled)
            {
                MessageBox.Show("The program cannot find a printer, and therefore cannot print!", "Problem with printing");
                return;
            }

            string ShortCodeOfBank = txtBankStatement.Text;
            string DateOfStatement = StringHelper.DateToLocalizedString(
                dtpBankStatementDate.Date.HasValue ? dtpBankStatementDate.Date.Value : DateTime.Today);
            string HtmlDocument = String.Empty;

            if (rbtListAll.Checked)
            {
                HtmlDocument =
                    PrintHTML(
                        CurrentStatement,
                        FMainDS.AEpTransaction.DefaultView, FMainDS.AEpMatch, Catalog.GetString(
                            "Full bank statement") + ", " + ShortCodeOfBank + ", " + DateOfStatement);
            }
            else if (rbtListUnmatchedGift.Checked)
            {
                HtmlDocument =
                    PrintHTML(
                        CurrentStatement,
                        FMainDS.AEpTransaction.DefaultView, FMainDS.AEpMatch, Catalog.GetString(
                            "Unmatched gifts") + ", " + ShortCodeOfBank + ", " + DateOfStatement);
            }
            else if (rbtListUnmatchedGL.Checked)
            {
                HtmlDocument =
                    PrintHTML(
                        CurrentStatement,
                        FMainDS.AEpTransaction.DefaultView, FMainDS.AEpMatch, Catalog.GetString(
                            "Unmatched GL") + ", " + ShortCodeOfBank + ", " + DateOfStatement);
            }
            else if (rbtListGift.Checked)
            {
                HtmlDocument =
                    PrintHTML(
                        CurrentStatement,
                        FMainDS.AEpTransaction.DefaultView, FMainDS.AEpMatch, Catalog.GetString(
                            "Matched gifts") + ", " + ShortCodeOfBank + ", " + DateOfStatement);
            }

            if (HtmlDocument.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("nothing to print"));
                return;
            }

            TGfxPrinter GfxPrinter = new TGfxPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(HtmlDocument,
                String.Empty,
                GfxPrinter);
            GfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.eDefaultMargins);

            PrintDialog dlg = new PrintDialog();
            dlg.Document = GfxPrinter.Document;
            dlg.AllowCurrentPage = true;
            dlg.AllowSomePages = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlg.Document.Print();
            }
        }

        /// <summary>
        /// dump unmatched gifts or other transactions to a HTML table for printing
        /// </summary>
        private static string PrintHTML(
            AEpStatementRow ACurrentStatement,
            DataView AEpTransactions, AEpMatchTable AMatches, string ATitle)
        {
            string letterTemplateFilename = TAppSettingsManager.GetValue("BankImport.ReportHTMLTemplate", false);

            if ((letterTemplateFilename.Length == 0) || !File.Exists(letterTemplateFilename))
            {
                OpenFileDialog DialogOpen = new OpenFileDialog();
                DialogOpen.Filter = "Report template (*.html)|*.html";
                DialogOpen.RestoreDirectory = true;
                DialogOpen.Title = "Open Report Template";

                if (DialogOpen.ShowDialog() == DialogResult.OK)
                {
                    letterTemplateFilename = DialogOpen.FileName;
                }
            }

            // message body from HTML template
            StreamReader reader = new StreamReader(letterTemplateFilename);
            string msg = reader.ReadToEnd();

            reader.Close();

            msg = msg.Replace("#TITLE", ATitle);
            msg = msg.Replace("#PRINTDATE", DateTime.Now.ToShortDateString());
            msg = msg.Replace("#STATEMENTNR", ACurrentStatement.IdFromBank);
            msg = msg.Replace("#STARTBALANCE", String.Format("{0:N}", ACurrentStatement.StartBalance));
            msg = msg.Replace("#ENDBALANCE", String.Format("{0:N}", ACurrentStatement.EndBalance));

            // recognise detail lines automatically
            string RowTemplate;
            msg = TPrinterHtml.GetTableRow(msg, "#DESCRIPTION", out RowTemplate);
            string rowTexts = "";

            BankImportTDSAEpTransactionRow row = null;

            AEpTransactions.Sort = BankImportTDSAEpTransactionTable.GetNumberOnPaperStatementDBName();

            Decimal Sum = 0.0m;

            DataView MatchesByMatchText = new DataView(AMatches,
                string.Empty,
                AEpMatchTable.GetMatchTextDBName(),
                DataViewRowState.CurrentRows);

            string thinLine = "<font size=\"-3\">-------------------------------------------------------------------------<br/></font>";

            foreach (DataRowView rv in AEpTransactions)
            {
                row = (BankImportTDSAEpTransactionRow)rv.Row;

                string rowToPrint = RowTemplate;

                rowToPrint = rowToPrint.Replace("#NAME", row.AccountName);
                rowToPrint = rowToPrint.Replace("#DESCRIPTION", row.Description);

                string RecipientDescription = string.Empty;

                DataRowView[] matches = MatchesByMatchText.FindRows(row.MatchText);

                AEpMatchRow match = null;

                foreach (DataRowView rvMatch in matches)
                {
                    match = (AEpMatchRow)rvMatch.Row;

                    if (RecipientDescription.Length > 0)
                    {
                        RecipientDescription += "<br/>";
                    }

                    if (!match.IsRecipientKeyNull() && (match.RecipientKey > 0))
                    {
                        RecipientDescription += match.RecipientKey.ToString() + " ";
                    }

                    RecipientDescription += match.RecipientShortName;
                }

                if (RecipientDescription.Trim().Length > 0)
                {
                    rowToPrint = rowToPrint.Replace("#RECIPIENTDESCRIPTION", "<br/>" + thinLine + RecipientDescription);
                }
                else
                {
                    rowToPrint = rowToPrint.Replace("#RECIPIENTDESCRIPTION", string.Empty);
                }

                if (!match.IsDonorKeyNull() && (match.DonorKey > 0))
                {
                    string DonorDescription = "<br/>" + thinLine + match.DonorKey.ToString() + " " + match.DonorShortName;

                    rowToPrint = rowToPrint.Replace("#DONORDESCRIPTION", DonorDescription);
                }
                else
                {
                    rowToPrint = rowToPrint.Replace("#DONORDESCRIPTION", string.Empty);
                }

                rowTexts += rowToPrint.
                            Replace("#NRONSTATEMENT", row.NumberOnPaperStatement.ToString()).
                            Replace("#AMOUNT", String.Format("{0:C}", row.TransactionAmount)).
                            Replace("#ACCOUNTNUMBER", row.BankAccountNumber).
                            Replace("#BANKSORTCODE", row.BranchCode);

                Sum += Convert.ToDecimal(row.TransactionAmount);
            }

            Sum = Math.Round(Sum, 2);

            return msg.Replace("#ROWTEMPLATE", rowTexts).Replace("#TOTALAMOUNT", String.Format("{0:C}", Sum));
        }

        private void TransactionFilterChanged(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();
            pnlDetails.Visible = false;
            CurrentlySelectedMatch = null;
            CurrentlySelectedTransaction = null;

            if (FTransactionView == null)
            {
                return;
            }

            if (rbtListAll.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey);
            }
            else if (rbtListGift.Checked)
            {
                // TODO: allow splitting a transaction, one part is GL/AP, the other is a donation?
                //       at Top Level: split transaction, results into 2 rows in aeptransaction (not stored). Merge Transactions again?

                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT);
            }
            else if (rbtListUnmatchedGift.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}' and {4} LIKE '%{5}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_UNMATCHED,
                    BankImportTDSAEpTransactionTable.GetTransactionTypeCodeDBName(),
                    MFinanceConstants.BANK_STMT_POTENTIAL_GIFT);
            }
            else if (rbtListUnmatchedGL.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}' and ({4} NOT LIKE '%{5}' OR {4} IS NULL)",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_UNMATCHED,
                    BankImportTDSAEpTransactionTable.GetTransactionTypeCodeDBName(),
                    MFinanceConstants.BANK_STMT_POTENTIAL_GIFT);
            }
            else if (rbtListIgnored.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING);
            }
            else if (rbtListGL.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL);
            }

            // update sumcredit and sumdebit
            decimal sumCredit = 0.0M;
            decimal sumDebit = 0.0M;

            foreach (DataRowView rv in FTransactionView)
            {
                AEpTransactionRow Row = (AEpTransactionRow)rv.Row;

                if (Row.TransactionAmount < 0)
                {
                    sumDebit += Row.TransactionAmount * -1.0M;
                }
                else
                {
                    sumCredit += Row.TransactionAmount;
                }
            }

            txtCreditSum.NumberValueDecimal = sumCredit;
            txtDebitSum.NumberValueDecimal = sumDebit;
            txtTransactionCount.Text = FTransactionView.Count.ToString();

            if (FTransactionView.Count > 0)
            {
                grdAllTransactions.SelectRowInGrid(1);
            }
        }
    }
}