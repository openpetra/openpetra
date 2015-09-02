//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, peters
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of GiftRevertAdjust_ManualCode.
    /// </summary>
    public partial class TFrmGiftRevertAdjust
    {
        private Int32 FLedgerNumber;
        private Hashtable requestParams = new Hashtable();
        private GiftBatchTDS giftMainDS = new GiftBatchTDS();
        private AGiftDetailRow giftDetailRow = null;
        private string FCurrencyCode = null;
        private Boolean ok = false;
        private Boolean FNoReceipt = false;
        private DateTime StartDateCurrentPeriod;
        private DateTime EndDateLastForwardingPeriod;

        /// <summary>
        /// Return if the revert/adjust action was Ok (then a refresh is needed; otherwise rollback was done)
        /// </summary>
        public bool Ok {
            get
            {
                return ok;
            }
        }

        /// <summary>
        /// Sets a value indicating whether to print adjusting gift transactions on periodic receipts
        /// </summary>
        public bool NoReceipt
        {
            set
            {
                FNoReceipt = value;
            }
        }

        /// <summary>
        /// Gift DS is injected if needed (only for Field Adjustment)
        /// </summary>
        public GiftBatchTDS GiftMainDS {
            set
            {
                giftMainDS = value;
            }
        }

        /// <summary>
        /// Preset the effective date.
        /// </summary>
        public DateTime ? PresetEffectiveDate
        {
            set
            {
                dtpEffectiveDate.Date = value;
            }
        }

        /// <summary>
        /// A Gift Detail Row is injected
        /// </summary>
        public AGiftDetailRow GiftDetailRow {
            set
            {
                giftDetailRow = value;

                if ((giftDetailRow.GiftCommentOne != null) && (giftDetailRow.GiftCommentOne.Length > 0))
                {
                    txtReversalCommentOne.Text = giftDetailRow.GiftCommentOne;
                    cmbReversalCommentOneType.Text = giftDetailRow.CommentOneType;
                }

                if ((giftDetailRow.GiftCommentTwo != null) && (giftDetailRow.GiftCommentTwo.Length > 0))
                {
                    txtReversalCommentTwo.Text = giftDetailRow.GiftCommentTwo;
                    cmbReversalCommentTwoType.Text = giftDetailRow.CommentTwoType;
                }

                if ((giftDetailRow.GiftCommentThree != null) && (giftDetailRow.GiftCommentThree.Length > 0))
                {
                    txtReversalCommentThree.Text = giftDetailRow.GiftCommentThree;
                    cmbReversalCommentThreeType.Text = giftDetailRow.CommentThreeType;
                }

                AddParam("BatchNumber", giftDetailRow.BatchNumber);
                AddParam("GiftNumber", giftDetailRow.GiftTransactionNumber);
                AddParam("GiftDetailNumber", giftDetailRow.DetailNumber);
                AddParam("CostCentre", giftDetailRow.CostCentreCode);

                if (((GiftAdjustmentFunctionEnum)requestParams["Function"] != GiftAdjustmentFunctionEnum.FieldAdjust)
                    && ((GiftAdjustmentFunctionEnum)requestParams["Function"] != GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust))
                {
                    // Now we have the criteria we can retrieve all the data we need from the database
                    GetGiftsForReverseAdjust();
                }
            }
        }

        /// <summary>
        /// Gift's currency code
        /// (This is needed to correctly display which batches gift reversals/adjustments can be added to.)
        /// </summary>
        public string CurrencyCode
        {
            set
            {
                FCurrencyCode = value;
            }
        }

        /// <summary>
        /// Ledger Number
        /// </summary>
        public int LedgerNumber {
            set
            {
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                FLedgerNumber = value;
                requestParams.Add("ALedgerNumber", FLedgerNumber);

                DateTime DefaultDate;
                TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                    out StartDateCurrentPeriod,
                    out EndDateLastForwardingPeriod,
                    out DefaultDate);
                lblValidDateRange.Text = String.Format(Catalog.GetString("(Must be between {0} and {1}.)"),
                    StartDateCurrentPeriod.ToShortDateString(), EndDateLastForwardingPeriod.ToShortDateString());

                // set default date for a new batch
                if (DateTime.Today > EndDateLastForwardingPeriod)
                {
                    dtpEffectiveDate.Date = EndDateLastForwardingPeriod;
                }
                else if (DateTime.Today < StartDateCurrentPeriod)
                {
                    dtpEffectiveDate.Date = StartDateCurrentPeriod;
                }
                else
                {
                    dtpEffectiveDate.Date = DateTime.Today;
                }
            }
        }

        private void InitializeManualCode()
        {
            //FLedger is still zero at this point
            FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                AGiftBatchTable.GetBatchStatusDBName(),
                MFinanceConstants.BATCH_UNPOSTED
                );
            FMainDS.AGiftBatch.DefaultView.Sort = AGiftBatchTable.GetBatchNumberDBName() + " DESC";

            SelectBatchChanged(null, null);

            rbtNewBatch.Checked = true;
        }

        private void RunOnceOnActivationManual()
        {
            grdDetails.Enabled = false;
            grdDetails.DataSource = null;
        }

        private void GetGiftsForReverseAdjust()
        {
            Boolean ok;
            TVerificationResultCollection Messages;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GetGiftsForReverseAdjust(requestParams, ref giftMainDS, out Messages);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            // If one or more of the gifts have already been reversed.
            if (!ok)
            {
                if (Messages.Count > 0)
                {
                    foreach (TVerificationResult message in Messages)
                    {
                        if (message.ResultText.Length > 0)
                        {
                            MessageBox.Show(this.Text + Catalog.GetString(" cancelled. ") + message.ResultText,
                                this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                DialogResult = System.Windows.Forms.DialogResult.Abort;
                Close();
            }
        }

        /// <summary>
        /// Some params for the server function are injected
        /// </summary>
        public void AddParam(String paramName, Object param)
        {
            requestParams.Remove(paramName);
            requestParams.Add(paramName, param);
        }

        private void BtnOk_Click(System.Object sender, System.EventArgs e)
        {
            if (rbtExistingBatch.Checked && (GetSelectedDetailRow() == null))
            {
                // nothing seleted
                MessageBox.Show(Catalog.GetString("Please select a batch."));
                return;
            }

            if (rbtNewBatch.Checked)
            {
                // if date is empty
                if (string.IsNullOrEmpty(dtpEffectiveDate.Text))
                {
                    MessageBox.Show(Catalog.GetString("Please enter a date for the new Gift Batch."));

                    dtpEffectiveDate.Focus();
                    return;
                }

                // if date is invalid
                if (!((TtxtPetraDate)dtpEffectiveDate).ValidDate(false))
                {
                    MessageBox.Show(Catalog.GetString("Please enter a valid date for the new Gift Batch."));

                    dtpEffectiveDate.Focus();
                    return;
                }
            }

            AddParam("NewBatchSelected", rbtExistingBatch.Checked);

            if (rbtExistingBatch.Checked)
            {
                AddParam("NewBatchNumber", GetSelectedDetailRow().BatchNumber);
            }
            else
            {
                //check the gift batch date to use
                if ((dtpEffectiveDate.Date < StartDateCurrentPeriod)
                    || (dtpEffectiveDate.Date > EndDateLastForwardingPeriod)
                    )
                {
                    MessageBox.Show(Catalog.GetString("Your Date was outside the allowed posting period."));
                    dtpEffectiveDate.Focus();
                    dtpEffectiveDate.SelectAll();
                    return;
                }
                else
                {
                    AddParam("GlEffectiveDate", dtpEffectiveDate.Date.Value);
                }
            }

            if (((txtReversalCommentOne.Text.Trim().Length == 0) && (cmbReversalCommentOneType.SelectedIndex != -1))
                || ((txtReversalCommentOne.Text.Trim().Length > 0) && (cmbReversalCommentOneType.SelectedIndex == -1))
                )
            {
                MessageBox.Show(Catalog.GetString("Comment 1 and Comment Type 1 must both be empty or both contain a value!"));
                txtReversalCommentOne.Focus();
                return;
            }

            if (((txtReversalCommentTwo.Text.Trim().Length == 0) && (cmbReversalCommentTwoType.SelectedIndex != -1))
                || ((txtReversalCommentTwo.Text.Trim().Length > 0) && (cmbReversalCommentTwoType.SelectedIndex == -1))
                )
            {
                MessageBox.Show(Catalog.GetString("Comment 2 and Comment Type 2 must both be empty or both contain a value!"));
                txtReversalCommentTwo.Focus();
                return;
            }

            if (((txtReversalCommentThree.Text.Trim().Length == 0) && (cmbReversalCommentThreeType.SelectedIndex != -1))
                || ((txtReversalCommentThree.Text.Trim().Length > 0) && (cmbReversalCommentThreeType.SelectedIndex == -1))
                )
            {
                MessageBox.Show(Catalog.GetString("Comment 3 and Comment Type 3 must both be empty or both contain a value!"));
                txtReversalCommentThree.Focus();
                return;
            }

            AddParam("ReversalCommentOne", txtReversalCommentOne.Text);
            AddParam("ReversalCommentTwo", txtReversalCommentTwo.Text);
            AddParam("ReversalCommentThree", txtReversalCommentThree.Text);
            AddParam("ReversalCommentOneType", cmbReversalCommentOneType.Text);
            AddParam("ReversalCommentTwoType", cmbReversalCommentTwoType.Text);
            AddParam("ReversalCommentThreeType", cmbReversalCommentThreeType.Text);
            AddParam("NoReceipt", FNoReceipt);

            ReverseAdjust();
        }

        // do the actual reversal / adjustment
        private void ReverseAdjust()
        {
            int AdjustmentBatchNumber;
            Boolean ok;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GiftRevertAdjust(requestParams, out AdjustmentBatchNumber, giftMainDS);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (ok)
            {
                GiftAdjustmentFunctionEnum Function = (GiftAdjustmentFunctionEnum)requestParams["Function"];

                switch (Function)
                {
                    case GiftAdjustmentFunctionEnum.ReverseGiftBatch :
                        MessageBox.Show(Catalog.GetString("Reversed gift batch has been successfully created with Batch Number " +
                            AdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift Batch"));
                        break;

                    case GiftAdjustmentFunctionEnum.ReverseGiftDetail:
                        MessageBox.Show(Catalog.GetString("Reversed gift detail has been successfully added to Batch " + AdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift Detail"));
                        break;

                    case GiftAdjustmentFunctionEnum.ReverseGift:
                        MessageBox.Show(Catalog.GetString("Reversed gift has been successfully added to Batch " + AdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.AdjustGift:
                        MessageBox.Show(Catalog.GetString("Adjustment transactions have been successfully added to Batch " + AdjustmentBatchNumber +
                            "."),
                        Catalog.GetString("Adjust Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.FieldAdjust:
                        MessageBox.Show(Catalog.GetString("Gift Field Adjustment transactions have been successfully added to Batch " +
                            AdjustmentBatchNumber +
                            "."),
                        Catalog.GetString("Adjust Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust:
                        MessageBox.Show(Catalog.GetString("Tax Deductible Percentage Adjustment transactions have been successfully added to Batch "
                            +
                            AdjustmentBatchNumber +
                            "."),
                        Catalog.GetString("Adjust Gift"));
                        break;
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                Close();
            }
        }

        private void SelectBatchChanged(System.Object sender, EventArgs e)
        {
            bool isChecked = rbtExistingBatch.Checked;

            if (isChecked)
            {
                //First pass FLedgerNumber = 0 so need to add Ledger to the filter when the user first checks the checkbox
                if ((FLedgerNumber != 0) && !FMainDS.AGiftBatch.DefaultView.RowFilter.Contains(AGiftBatchTable.GetLedgerNumberDBName()))
                {
                    FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = {1} AND {2} = '{3}' AND {4} = '{5}'",
                        AGiftBatchTable.GetLedgerNumberDBName(),
                        FLedgerNumber,
                        AGiftBatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED,
                        AGiftBatchTable.GetCurrencyCodeDBName(),
                        FCurrencyCode
                        );
                }

                DataView myDataView = FMainDS.AGiftBatch.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

                if (grdDetails.Rows.Count > 1)
                {
                    grdDetails.SelectRowInGrid(1);
                }

                txtReversalCommentOne.SelectAll();
                txtReversalCommentOne.Focus();
                dtpEffectiveDate.Enabled = false;

                grdDetails.AutoResizeGrid();
                grdDetails.Enabled = true;
            }
            else
            {
                grdDetails.DataSource = null;
                //bring enablement of the date textbox here to ensure enabled before setting focus
                dtpEffectiveDate.Enabled = true;
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
            }

            grdDetails.Enabled = isChecked;
            lblEffectiveDate.Enabled = !isChecked;
            lblValidDateRange.Enabled = !isChecked;
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }
    }
}