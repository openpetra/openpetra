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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of DonorRecipientHistory_ManualCode.
    /// </summary>
    public partial class TFrmDonorRecipientHistory
    {
        private Int32 FLedgerNumber = -1;
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
                FLedgerNumber = value;
                cmbLedger.SetSelectedInt32(FLedgerNumber);

                // update the combobox lists
                TFinanceControls.InitialiseMotivationGroupList(ref cmbMotivationGroup, FLedgerNumber, false);
                DataRow BlankRow = cmbMotivationGroup.Table.NewRow();
                BlankRow["a_ledger_number_i"] = value;
                BlankRow["a_motivation_group_code_c"] = ALL;
                BlankRow["a_motivation_group_description_c"] = Catalog.GetString("All groups");
                cmbMotivationGroup.Table.Rows.InsertAt(BlankRow, 0);
                cmbMotivationGroup.cmbCombobox.SelectedIndex = 0;
                cmbMotivationGroup.ColumnWidthCol2 = 300;

                TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetail, FLedgerNumber, false);
                BlankRow = cmbMotivationDetail.Table.NewRow();
                BlankRow["a_ledger_number_i"] = value;
                BlankRow["a_motivation_group_code_c"] = "";
                BlankRow["a_motivation_detail_code_c"] = ALL;
                BlankRow["a_motivation_detail_desc_c"] = Catalog.GetString("All details");
                cmbMotivationDetail.Table.Rows.InsertAt(BlankRow, 0);
                cmbMotivationDetail.cmbCombobox.SelectedIndex = 0;
                cmbMotivationDetail.ColumnWidthCol2 = 300;
            }
        }

        private void InitializeManualCode()
        {
            // remove from the combobox all ledger numbers which the user does not have permission to access
            DataView cmbLedgerDataView = (DataView)cmbLedger.cmbCombobox.DataSource;

            for (int i = 0; i < cmbLedgerDataView.Count; i++) // cmbLedger.cmbCombobox.Items.Count; i++)
            {
                string LedgerNumber = ((int)cmbLedgerDataView[i].Row[0]).ToString("0000");

                if (!UserInfo.GUserInfo.IsInModule("LEDGER" + LedgerNumber))
                {
                    cmbLedgerDataView.Delete(i);
                    i--;
                }
            }

            FPetraUtilsObject.SetStatusBarText(grdDetails, Catalog.GetString("Use the mouse or navigation keys to select a data row to view"));

            // set the currency code to be blank initially
            txtGiftTotal.CurrencyCode = "   ";
        }

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
        }

        private void EnableLedgerDropdown()
        {
            cmbLedger.Enabled = true;

            // set the selected ledger to be the current ledger
            int CurrentLedger = TLstTasks.CurrentLedger;

            if (CurrentLedger > 0)
            {
                cmbLedger.SetSelectedInt32(CurrentLedger);
            }

            LedgerNumber = CurrentLedger;
        }

        private void SetupGrid()
        {
            grdDetails.Columns.Clear();
            grdDetails.AddDateColumn("Date Entered", FMainDS.Tables[TEMP_TABLE_NAME].Columns["DateEntered"]);
            grdDetails.AddTextColumn("Group", FMainDS.Tables[TEMP_TABLE_NAME].Columns["MotivationGroupCode"]);
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

        /// <summary>
        /// Browse: (Re)LoadTableContents, called after injection of parameters or manual via button
        /// </summary>
        public void Search(bool ALoading)
        {
            TVerificationResultCollection AMessages;
            Hashtable requestParams = new Hashtable();

            Int64 donor = Convert.ToInt64(txtDonor.Text);
            Int64 recipient = Convert.ToInt64(txtRecipient.Text);

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

            string dateFrom = dtpDateFrom.Date.HasValue ? dtpDateFrom.Date.Value.ToShortDateString() : String.Empty;
            string dateTo = dtpDateTo.Date.HasValue ? dtpDateTo.Date.Value.ToShortDateString() : String.Empty;

            if ((donor == 0) && (recipient == 0))
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

            if ((dateFrom.Length == 0) && (dtpDateFrom.Text.Length > 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a valid Date From."), Catalog.GetString("Date From Error"));
                dtpDateFrom.Focus();
                dtpDateFrom.SelectAll();
                return;
            }

            if ((dateTo.Length == 0) && (dtpDateTo.Text.Length > 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a valid Date To."), Catalog.GetString("Date To Error"));
                dtpDateTo.Focus();
                dtpDateTo.SelectAll();
                return;
            }

            requestParams.Add("TempTable", TEMP_TABLE_NAME);
            requestParams.Add("Ledger", FLedgerNumber);
            requestParams.Add("Donor", donor);
            requestParams.Add("Recipient", recipient);
            requestParams.Add("MotivationGroup", motivationGroup);
            requestParams.Add("MotivationDetail", motivationDetail);
            requestParams.Add("DateFrom", dateFrom);
            requestParams.Add("DateTo", dateTo);

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

                    SelectDetailRowByDataTableIndex(0);
                    txtNumberOfGifts.Text = (grdDetails.Rows.Count - 1).ToString();
                }

                btnView.Enabled = grdDetails.Rows.Count > 1;

                UpdateTotals();
                UpdateRecordNumberDisplay();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private String RowFilter = "";
        private void AddRowFilter(String filterCondition)
        {
            if (RowFilter.Length > 0)
            {
                RowFilter = RowFilter + " AND ";
            }

            RowFilter += filterCondition;
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSearchClick(object sender, EventArgs e)
        {
            Search(false);
        }

        /// <summary>
        /// static method for opening the window from partner module
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PartnerKey"></param>
        /// <param name="theParentForm"></param>
        public static void OpenWindowDonorRecipientHistory(String Name, Int64 PartnerKey, Form theParentForm)
        {
            if (PartnerKey == -1)
            {
                MessageBox.Show(Catalog.GetString("No current partner selected"));
                return;
            }

            Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory frmDRH = new  Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory(
                theParentForm);

            // if the user does not have permission to access any Ledgers
            if (((DataView)frmDRH.cmbLedger.cmbCombobox.DataSource).Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Cannot view History as you do not have access rights to any Ledgers."));
                return;
            }

            try
            {
                frmDRH.Cursor = Cursors.WaitCursor;

                if (Name.Equals("mniMaintainDonorHistory"))
                {
                    frmDRH.Donor = PartnerKey;
                }
                else
                {
                    frmDRH.Recipient = PartnerKey;
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

        private void UpdateTotals()
        {
            decimal sum = 0;
            decimal sumIntl = 0;

            if (grdDetails.Rows.Count > 1)
            {
                foreach (DataRow rv in FMainDS.Tables[TEMP_TABLE_NAME].Rows)
                {
                    DataRow gdr = (DataRow)rv;
                    sum += (decimal)gdr["GiftAmount"];
                    sumIntl += (decimal)gdr["GiftAmountIntl"];
                }
            }

            txtGiftTotal.NumberValueDecimal = sum;
            txtGiftTotal.CurrencyCode = FMainDS.ALedger[0].BaseCurrency;
            txtGiftTotal.ReadOnly = true;
        }
    }
}