//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, peters
//
// Copyright 2004-2011 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of DonorRecipientHistory_ManualCode.
    /// </summary>
    public partial class TFrmDonorRecipientHistory
    {
        #region Properties

        private Int32 FLedgerNumber = -1;
        private Int64 FDonorKey = -1;
        private Int64 FRecipientKey = -1;
        private string FDateFrom = string.Empty;
        private string FDateTo = string.Empty;
        private string ALL = "[" + Catalog.GetString("All") + "]";

        /// the Donor
        public long Donor
        {
            set
            {
                txtDonor.Text = String.Format("{0:0000000000}", value);
            }
        }

        /// the recipient
        public long Recipient
        {
            set
            {
                txtRecipient.Text = String.Format("{0:0000000000}", value);
            }
        }

        /// <summary>
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                if (FLedgerNumber != value)
                {
                    FLedgerNumber = value;
                    cmbLedger.SetSelectedInt32(FLedgerNumber);
                }
            }
        }

        #endregion

        #region Setup methods

        private void InitializeManualCode()
        {
            // remove from the combobox all ledger numbers which the user does not have permission to access
            DataView cmbLedgerDataView = (DataView)cmbLedger.cmbCombobox.DataSource;

            for (int i = 0; i < cmbLedgerDataView.Count; i++)
            {
                string LedgerNumberStr;

                // postgresql
                if (cmbLedgerDataView[i].Row[0].GetType().Equals(typeof(int)))
                {
                    LedgerNumberStr = ((int)cmbLedgerDataView[i].Row[0]).ToString("0000");
                }
                else // sqlite
                {
                    LedgerNumberStr = ((Int64)cmbLedgerDataView[i].Row[0]).ToString("0000");
                }

                if (!UserInfo.GUserInfo.IsInModule("LEDGER" + LedgerNumberStr))
                {
                    cmbLedgerDataView.Delete(i);
                    i--;
                }
            }

            FPetraUtilsObject.SetStatusBarText(grdDetails, Catalog.GetString("Use the mouse or navigation keys to select a data row to view"));

            // set the currency code to be blank initially
            txtGiftTotal.CurrencyCode = "   ";

            lblRecordCounter.Text = "";

            // correct the tab indexes
            int TabOrder = dtpDateFrom.TabIndex;
            dtpDateFrom.TabIndex = txtRecipient.TabIndex;
            txtRecipient.TabIndex = TabOrder;

            cmbMotivationGroup.RemoveDescriptionLabel();
            cmbMotivationDetail.RemoveDescriptionLabel();

            // catch enter on all controls, to trigger search
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);

            // catch enter on grid to view the selected transaction
            this.grdDetails.EnterKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.ViewTransaction);

            // focus on txtDonor
            this.ActiveControl = txtDonor;
        }

        private void SetupGrid()
        {
            grdDetails.Columns.Clear();
            grdDetails.AddDateColumn("Date Entered", FMainDS.Tables[TEMP_TABLE_NAME].Columns["DateEntered"]);
            grdDetails.AddTextColumn("Group", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MotivationGroupCode"], 80);
            grdDetails.AddTextColumn("Detail", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MotivationDetailCode"], 80);
            grdDetails.AddTextColumn("Receipt", FMainDS.Tables[TEMP_TABLE_NAME].Columns["ReceiptNumber"], 60);
            grdDetails.AddCurrencyColumn("Amount (Base)", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftAmount"]);
            grdDetails.AddCurrencyColumn("Amount (Intl)", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftAmountIntl"]);
            grdDetails.AddCheckBoxColumn("C", FMainDS.Tables[TEMP_TABLE_NAME].Columns["ConfidentialGiftFlag"], 17);
            grdDetails.AddTextColumn("Batch", FMainDS.Tables[TEMP_TABLE_NAME].Columns["BatchNumber"]);
            grdDetails.AddTextColumn("Trans", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftTransactionNumber"], 50);
            grdDetails.AddTextColumn("Donor", FMainDS.Tables[TEMP_TABLE_NAME].Columns["DonorDescription"], 160);
            grdDetails.AddTextColumn("Recipient", FMainDS.Tables[TEMP_TABLE_NAME].Columns["RecipientDescription"], 160);
            grdDetails.AddTextColumn("Reference", FMainDS.Tables[TEMP_TABLE_NAME].Columns["Reference"], 90);
            grdDetails.AddTextColumn("Comment One", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftCommentOne"], 200);
            grdDetails.AddTextColumn("Comment Type", FMainDS.Tables[TEMP_TABLE_NAME].Columns["CommentOneType"], 100);
            grdDetails.AddTextColumn("Recipient Field", FMainDS.Tables[TEMP_TABLE_NAME].Columns["RecipientLedgerNumber"], 100);
            grdDetails.AddTextColumn("Donor", FMainDS.Tables[TEMP_TABLE_NAME].Columns["DonorKey"], 70);
            grdDetails.AddTextColumn("Recipient", FMainDS.Tables[TEMP_TABLE_NAME].Columns["RecipientKey"], 70);
            grdDetails.AddCheckBoxColumn("Charge Fee", FMainDS.Tables[TEMP_TABLE_NAME].Columns["ChargeFlag"], 17);
            grdDetails.AddTextColumn("Method of Payment", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MethodOfPaymentCode"], 120);
            grdDetails.AddTextColumn("Method of Giving", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MethodOfGivingCode"], 110);
            grdDetails.AddTextColumn("Cost Centre Code", FMainDS.Tables[TEMP_TABLE_NAME].Columns["CostCentreCode"], 110);
            grdDetails.AddTextColumn("Comment Two", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftCommentTwo"], 100);
            grdDetails.AddTextColumn("Comment Three", FMainDS.Tables[TEMP_TABLE_NAME].Columns["GiftCommentThree"], 100);
            grdDetails.AddTextColumn("Mailing Code", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MailingCode"], 85);

            grdDetails.Columns[0].Width = 90;     // Date Entered
            grdDetails.Columns[4].Width = 100;     // Amount - Base
            grdDetails.Columns[5].Width = 90;     // Amount - Intl
        }

        private void SetupMotivationComboboxes()
        {
            /* cmbMotivationGroup */

            TFinanceControls.InitialiseMotivationGroupList(ref cmbMotivationGroup, FLedgerNumber, false);

            // remove motivation groups that are not used in the results
            List <int>RemoveIndexes = new List <int>();

            for (int i = 0; i < cmbMotivationGroup.Table.Rows.Count; i++)
            {
                bool NotFound = true;

                foreach (DataRow DetailRow in FMainDS.Tables[TEMP_TABLE_NAME].Rows)
                {
                    if (cmbMotivationGroup.Table.Rows[i]["a_motivation_group_code_c"].ToString() == DetailRow["MotivationGroupCode"].ToString())
                    {
                        NotFound = false;
                        break;
                    }
                }

                if (NotFound)
                {
                    RemoveIndexes.Add(i);
                }
            }

            for (int i = RemoveIndexes.Count - 1; i >= 0; i--)
            {
                cmbMotivationGroup.Table.Rows.RemoveAt(RemoveIndexes[i]);
            }

            DataRow BlankRow = cmbMotivationGroup.Table.NewRow();
            BlankRow["a_ledger_number_i"] = FLedgerNumber;
            BlankRow["a_motivation_group_code_c"] = ALL;
            BlankRow["a_motivation_group_description_c"] = Catalog.GetString("All groups");
            cmbMotivationGroup.Table.Rows.InsertAt(BlankRow, 0);
            cmbMotivationGroup.cmbCombobox.SelectedIndex = 0;
            cmbMotivationGroup.ColumnWidthCol2 = 300;
            cmbMotivationGroup.Enabled = true;
            cmbMotivationGroup.cmbCombobox.SelectionLength = 0;

            /* cmbMotivationDetail */

            TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetail, FLedgerNumber, false);

            // remove motivation details that are not used in the results
            RemoveIndexes.Clear();

            for (int i = 0; i < cmbMotivationDetail.Table.Rows.Count; i++)
            {
                bool NotFound = true;

                foreach (DataRow DetailRow in FMainDS.Tables[TEMP_TABLE_NAME].Rows)
                {
                    if (cmbMotivationDetail.Table.Rows[i]["a_motivation_detail_code_c"].ToString() == DetailRow["MotivationDetailCode"].ToString())
                    {
                        NotFound = false;
                        break;
                    }
                }

                if (NotFound)
                {
                    RemoveIndexes.Add(i);
                }
            }

            for (int i = RemoveIndexes.Count - 1; i >= 0; i--)
            {
                cmbMotivationDetail.Table.Rows.RemoveAt(RemoveIndexes[i]);
            }

            BlankRow = cmbMotivationDetail.Table.NewRow();
            BlankRow["a_ledger_number_i"] = FLedgerNumber;
            BlankRow["a_motivation_group_code_c"] = "";
            BlankRow["a_motivation_detail_code_c"] = ALL;
            BlankRow["a_motivation_detail_desc_c"] = Catalog.GetString("All details");
            cmbMotivationDetail.Table.Rows.InsertAt(BlankRow, 0);
            cmbMotivationDetail.cmbCombobox.SelectedIndex = 0;
            cmbMotivationDetail.ColumnWidthCol2 = 300;
            cmbMotivationDetail.Enabled = true;
            cmbMotivationDetail.cmbCombobox.SelectionLength = 0;
        }

        #endregion

        #region Search

        /// <summary>
        /// Browse: (Re)LoadTableContents, called after injection of parameters or manual via button
        /// </summary>
        public void Search(bool ALoading)
        {
            TVerificationResultCollection AMessages;
            Hashtable requestParams = new Hashtable();

            FDonorKey = Convert.ToInt64(txtDonor.Text);
            FRecipientKey = Convert.ToInt64(txtRecipient.Text);

            string motivationGroup = cmbMotivationGroup.cmbCombobox.Text;
            string motivationDetail = cmbMotivationDetail.cmbCombobox.Text;

            if (motivationGroup == ALL)
            {
                motivationGroup = "";
            }

            if (motivationDetail == ALL)
            {
                motivationDetail = "";
            }

            FDateFrom = dtpDateFrom.Date.HasValue ? dtpDateFrom.Date.Value.ToShortDateString() : String.Empty;
            FDateTo = dtpDateTo.Date.HasValue ? dtpDateTo.Date.Value.ToShortDateString() : String.Empty;

            if ((FDonorKey == 0) && (FRecipientKey == 0))
            {
                MessageBox.Show(Catalog.GetString("You must restrict the search to a donor and/or a recipient."),
                    Catalog.GetString("Donor/Recipient Error"));
                txtDonor.Focus();
                return;
            }

            if (FLedgerNumber < 0)
            {
                MessageBox.Show(Catalog.GetString("Select Ledger and then press 'Search'."), Catalog.GetString("Ledger Error"));
                return;
            }

            if ((FDateFrom.Length == 0) && (dtpDateFrom.Text.Length > 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a valid Date From."), Catalog.GetString("Date From Error"));
                dtpDateFrom.Focus();
                dtpDateFrom.SelectAll();
                return;
            }

            if ((FDateTo.Length == 0) && (dtpDateTo.Text.Length > 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a valid Date To."), Catalog.GetString("Date To Error"));
                dtpDateTo.Focus();
                dtpDateTo.SelectAll();
                return;
            }

            requestParams.Add("TempTable", TEMP_TABLE_NAME);
            requestParams.Add("Ledger", FLedgerNumber);
            requestParams.Add("Donor", FDonorKey);
            requestParams.Add("Recipient", FRecipientKey);
            requestParams.Add("MotivationGroup", motivationGroup);
            requestParams.Add("MotivationDetail", motivationDetail);
            requestParams.Add("DateFrom", FDateFrom);
            requestParams.Add("DateTo", FDateTo);

            try
            {
                Cursor = Cursors.WaitCursor;
                GiftBatchTDS newTDS = TRemote.MFinance.Gift.WebConnectors.LoadDonorRecipientHistory(
                    requestParams,
                    out AMessages);

                if ((AMessages != null) && (AMessages.Count > 0))
                {
                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(Catalog.GetString("Error calling Donor/Recipient history"), AMessages));
                    return;
                }
                else
                {
                    if (FMainDS.Tables.Contains(TEMP_TABLE_NAME))
                    {
                        FMainDS.Tables.Remove(TEMP_TABLE_NAME);
                    }

                    FMainDS = newTDS;
                }

                if (FMainDS != null)
                {
                    if (ALoading)
                    {
                        if (FMainDS.Tables[TEMP_TABLE_NAME].Rows.Count > 0)
                        {
                            //If this form is loaded from elsewhere, set DateFrom to be lowest date in returned Table
                            DataRow gdr = (DataRow)FMainDS.Tables[TEMP_TABLE_NAME].Rows[FMainDS.Tables[TEMP_TABLE_NAME].Rows.Count - 1];
                            dtpDateFrom.Date = Convert.ToDateTime(gdr["DateEntered"]);
                        }
                        else
                        {
                            dtpDateFrom.Date = null;
                            dtpDateTo.Date = null;
                        }
                    }

                    if (grdDetails.Columns.Count < 1)
                    {
                        SetupGrid();
                    }

                    DataView myDataView = FMainDS.Tables[TEMP_TABLE_NAME].DefaultView;
                    myDataView.AllowNew = false;
                    grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                }

                btnView.Enabled = grdDetails.Rows.Count > 1;

                // update controls based on results
                OnCmbMotivationChange(this, null);
                UpdateTotals();
                UpdateRecordNumberDisplay();
                SetupMotivationComboboxes();

                if (FMainDS != null)
                {
                    // select the first row in the grid
                    SelectDetailRowByDataTableIndex(1);
                    this.ActiveControl = grdDetails;
                }

                // Enabled or disable Gift Statement buttons depending on what partner keys have been provided
                if (FDonorKey > 0)
                {
                    btnDonorGiftStatement.Enabled = true;
                    mniFileDonorGiftStatement.Enabled = true;
                }
                else
                {
                    btnDonorGiftStatement.Enabled = false;
                    mniFileDonorGiftStatement.Enabled = false;
                }

                if (FRecipientKey > 0)
                {
                    btnRecipientGiftStatement.Enabled = true;
                    mniFileRecipientGiftStatement.Enabled = true;
                }
                else
                {
                    btnRecipientGiftStatement.Enabled = false;
                    mniFileRecipientGiftStatement.Enabled = false;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Helper Methods

        private void EnableLedgerDropdown()
        {
            cmbLedger.Enabled = true;
            this.ActiveControl = cmbLedger;

            // set the selected ledger to be the current ledger
            int CurrentLedger = TLstTasks.CurrentLedger;

            if (CurrentLedger > 0)
            {
                cmbLedger.SetSelectedInt32(CurrentLedger);
            }

            LedgerNumber = CurrentLedger;
        }

        // parameters to be passed to Gift Statement screens
        private TParameterList GiftStatementParameters(TFrmDonorGiftStatement ADonor = null)
        {
            TParameterList ReturnValue = new TParameterList();

            // if btnDonorGiftStatement was clicked
            if (ADonor != null)
            {
                ReturnValue.Add("param_donor", "One Donor");
                ReturnValue.Add("param_donorkey", FDonorKey);

                TRptCalculator Calc = new TRptCalculator();
                ADonor.ReadControls(Calc, TReportActionEnum.raLoad);
                ReturnValue.Add("param_min_amount", Calc.GetParameters().GetParameter("param_min_amount").value);
                ReturnValue.Add("param_max_amount", Calc.GetParameters().GetParameter("param_max_amount").value);
            }
            else
            {
                ReturnValue.Add("param_recipient", "One Recipient");
                ReturnValue.Add("param_recipientkey", FRecipientKey);
            }

            DateTime FromDate = DateTime.MaxValue;
            DateTime ToDate = DateTime.MinValue;

            // if no dates are enter use the earliest and latest dates available in the dataset
            if (string.IsNullOrEmpty(FDateFrom) || string.IsNullOrEmpty(FDateTo))
            {
                foreach (DataRow Row in FMainDS.Tables[TEMP_TABLE_NAME].Rows)
                {
                    if ((DateTime)Row["DateEntered"] < FromDate)
                    {
                        FromDate = (DateTime)Row["DateEntered"];
                    }

                    if ((DateTime)Row["DateEntered"] > ToDate)
                    {
                        ToDate = (DateTime)Row["DateEntered"];
                    }
                }
            }

            if (!string.IsNullOrEmpty(FDateFrom))
            {
                ReturnValue.Add("param_from_date", Convert.ToDateTime(FDateFrom));
            }
            else
            {
                ReturnValue.Add("param_from_date", FromDate);
            }

            if (!string.IsNullOrEmpty(FDateTo))
            {
                ReturnValue.Add("param_to_date", Convert.ToDateTime(FDateTo));
            }
            else
            {
                ReturnValue.Add("param_to_date", ToDate);
            }

            return ReturnValue;
        }

        private void UpdateTotals()
        {
            decimal sum = 0;
            decimal sumIntl = 0;

            if (FMainDS != null)
            {
                DataTable Table = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView.ToTable();

                foreach (DataRow rv in Table.Rows)
                {
                    sum += (decimal)rv["GiftAmount"];
                    sumIntl += (decimal)rv["GiftAmountIntl"];
                }

                txtNumberOfGifts.Text = (grdDetails.Rows.Count - 1).ToString();
            }

            txtGiftTotal.NumberValueDecimal = sum;
            txtGiftTotal.CurrencyCode = FMainDS.ALedger[0].BaseCurrency;
            txtGiftTotal.ReadOnly = true;
        }

        #endregion

        #region Events

        private void ViewTransaction(object sender, EventArgs e)
        {
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            if ((FPreviouslySelectedDetailRow != null) && (FMainDS != null))
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    TFrmGiftBatch gb = new TFrmGiftBatch(this);
                    gb.ViewMode = true;

                    // load dataset with data for single transaction
                    gb.ViewModeTDS = TRemote.MFinance.Gift.WebConnectors.LoadSingleTransaction(FLedgerNumber,
                        (int)FPreviouslySelectedDetailRow["BatchNumber"],
                        (int)FPreviouslySelectedDetailRow["GiftTransactionNumber"],
                        (int)FPreviouslySelectedDetailRow["DetailNumber"]);
                    gb.ShowDetailsOfOneBatch(FLedgerNumber, (int)FPreviouslySelectedDetailRow["BatchNumber"]);
                    gb.FindGiftDetail((AGiftDetailRow)gb.ViewModeTDS.AGiftDetail.Rows[0]);
                    gb.SelectTab(TFrmGiftBatch.eGiftTabs.Transactions);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void OnCmbLedgerChange(System.Object sender, EventArgs e)
        {
            LedgerNumber = cmbLedger.GetSelectedInt32();

            // if the list is dropped down while the value is changed (not the case when a value from the list is clicked on)
            if (cmbLedger.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                LedgerDroppedDown = true;
            }
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSearchClick(object sender, EventArgs e)
        {
            this.ActiveControl = btnSearch;
            Search(false);
        }

        private void BtnClearClick(object sender, EventArgs e)
        {
            if (cmbLedger.Enabled)
            {
                EnableLedgerDropdown();
            }

            txtDonor.Text = "0000000000";
            txtRecipient.Text = "0000000000";
            dtpDateFrom.Clear();
            dtpDateTo.Clear();
        }

        private void DonorGiftStatement(object sender, EventArgs e)
        {
            TFrmDonorGiftStatement DonorGiftStatement = new TFrmDonorGiftStatement(this);

            DonorGiftStatement.LedgerNumber = FLedgerNumber;
            DonorGiftStatement.SetControls(GiftStatementParameters(DonorGiftStatement));
            DonorGiftStatement.Show();
        }

        private void RecipientGiftStatement(object sender, EventArgs e)
        {
            TFrmRecipientGiftStatement RecipientGiftStatement = new TFrmRecipientGiftStatement(this);

            RecipientGiftStatement.LedgerNumber = FLedgerNumber;
            RecipientGiftStatement.SetControls(GiftStatementParameters());
            RecipientGiftStatement.Show();
        }

        private void OnCmbMotivationChange(object sender, EventArgs e)
        {
            if (FMainDS.Tables[TEMP_TABLE_NAME] == null)
            {
                return;
            }

            // add (or remove) filters from the datatable
            DataView myDataView = FMainDS.Tables[TEMP_TABLE_NAME].DefaultView;
            string Filter = "";

            if (cmbMotivationGroup.cmbCombobox.Text != "[" + Catalog.GetString("All") + "]")
            {
                Filter = "MotivationGroupCode = '" + cmbMotivationGroup.cmbCombobox.Text + "'";
            }

            if (cmbMotivationDetail.cmbCombobox.Text != "[" + Catalog.GetString("All") + "]")
            {
                if (Filter != "")
                {
                    Filter += " and ";
                }

                Filter += "MotivationDetailCode = '" + cmbMotivationDetail.cmbCombobox.Text + "'";
            }

            myDataView.RowFilter = Filter;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            // update controls
            UpdateRecordNumberDisplay();
            UpdateTotals();

            // if the list is dropped down while the value is changed (not the case when a value from the list is clicked on)
            if (cmbMotivationGroup.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                MotivationGroupDroppedDown = true;
            }

            if (cmbMotivationDetail.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                MotivationDetailDroppedDown = true;
            }
        }

        /// <summary>
        /// static method for opening the window from partner module
        /// </summary>
        /// <param name="ADonor">True if Donor, false if Recipient</param>
        /// <param name="APartnerKey"></param>
        /// <param name="AParentForm"></param>
        public static void OpenWindowDonorRecipientHistory(bool ADonor, Int64 APartnerKey, Form AParentForm)
        {
            if (APartnerKey == -1)
            {
                MessageBox.Show(Catalog.GetString("No current partner selected"));
                return;
            }

            Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory frmDRH = new  Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory(
                AParentForm);

            // if the user does not have permission to access any Ledgers
            if (((DataView)frmDRH.cmbLedger.cmbCombobox.DataSource).Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Cannot view History as you do not have access rights to any Ledgers."));
                return;
            }

            try
            {
                frmDRH.Cursor = Cursors.WaitCursor;

                if (ADonor)
                {
                    frmDRH.Donor = APartnerKey;
                }
                else
                {
                    frmDRH.Recipient = APartnerKey;
                }

                frmDRH.EnableLedgerDropdown();
                frmDRH.Search(true);
                frmDRH.Show();
            }
            finally
            {
                frmDRH.Cursor = Cursors.Default;
            }
        }

        // These are used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down.
        private bool LedgerDroppedDown = false;
        private bool MotivationGroupDroppedDown = false;
        private bool MotivationDetailDroppedDown = false;

        // use 'Enter' key to start search
        private void CatchEnterKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // make sure that the 'Enter' key has not been pressed to select a value from a combo boxes dropped down list
                if (LedgerDroppedDown && cmbLedger.ContainsFocus)
                {
                    LedgerDroppedDown = false;
                    return;
                }
                else if (MotivationGroupDroppedDown && cmbMotivationGroup.ContainsFocus)
                {
                    MotivationGroupDroppedDown = false;
                    return;
                }
                else if (MotivationDetailDroppedDown && cmbMotivationDetail.ContainsFocus)
                {
                    MotivationDetailDroppedDown = false;
                    return;
                }

                BtnSearchClick(sender, e);
                e.Handled = true;

                LedgerDroppedDown = false;
                MotivationGroupDroppedDown = false;
                MotivationDetailDroppedDown = false;
            }
            else
            {
                e.Handled = false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Manages the opening of a Partner's Donor Recipient Screen
    /// </summary>
    public static class TDonorRecipientHistoryScreenManager
    {
        /// <summary>
        /// Opens a Modal instance of the Donor Recipient Screen
        /// </summary>
        /// <param name="ADonor">True if Donor, false if Recipient</param>
        /// <param name="APartnerKey"></param>
        /// <param name="AParentForm"></param>
        /// <returns></returns>
        public static void OpenForm(bool ADonor,
            long APartnerKey,
            Form AParentForm)
        {
            TFrmDonorRecipientHistory.OpenWindowDonorRecipientHistory(ADonor, APartnerKey, AParentForm);
        }
    }
}