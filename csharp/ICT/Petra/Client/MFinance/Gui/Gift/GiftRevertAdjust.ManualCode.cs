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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of GiftRevertAdjust_ManualCode.
    /// </summary>
    public partial class TFrmGiftRevertAdjust
    {
        private Int32 FLedgerNumber;
        private Hashtable requestParams = new Hashtable();
        private GiftBatchTDS FGiftMainDS = new GiftBatchTDS();
        private AGiftDetailRow FGiftDetailRow = null;
        private string FCurrencyCode = null;
        private Boolean ok = false;
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;
        private int FAdjustmentBatchNumber = -1;
        private bool FAutoCompleteComments = false;
        private bool FCheckTaxDeductPctChange = false;
        private bool FCheckGiftDestinationChange = false;

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
                chkNoReceipt.Checked = value;
            }
        }

        /// <summary>
        /// Gift DS is injected if needed (only for Field Adjustment)
        /// </summary>
        public GiftBatchTDS GiftMainDS {
            set
            {
                FGiftMainDS = value;
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
                FGiftDetailRow = value;

                if ((FGiftDetailRow.GiftCommentOne != null) && (FGiftDetailRow.GiftCommentOne.Length > 0))
                {
                    txtReversalCommentOne.Text = FGiftDetailRow.GiftCommentOne;
                    cmbReversalCommentOneType.Text = FGiftDetailRow.CommentOneType;
                }

                if ((FGiftDetailRow.GiftCommentTwo != null) && (FGiftDetailRow.GiftCommentTwo.Length > 0))
                {
                    txtReversalCommentTwo.Text = FGiftDetailRow.GiftCommentTwo;
                    cmbReversalCommentTwoType.Text = FGiftDetailRow.CommentTwoType;
                }

                if ((FGiftDetailRow.GiftCommentThree != null) && (FGiftDetailRow.GiftCommentThree.Length > 0))
                {
                    txtReversalCommentThree.Text = FGiftDetailRow.GiftCommentThree;
                    cmbReversalCommentThreeType.Text = FGiftDetailRow.CommentThreeType;
                }

                AddParam("BatchNumber", FGiftDetailRow.BatchNumber);
                AddParam("GiftNumber", FGiftDetailRow.GiftTransactionNumber);
                AddParam("GiftDetailNumber", FGiftDetailRow.DetailNumber);
                AddParam("CostCentre", FGiftDetailRow.CostCentreCode);

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
                    out FStartDateCurrentPeriod,
                    out FEndDateLastForwardingPeriod,
                    out DefaultDate);
                lblValidDateRange.Text = String.Format(Catalog.GetString("(Must be between {0} and {1}.)"),
                    FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());

                // set default date for a new batch
                if (DateTime.Today > FEndDateLastForwardingPeriod)
                {
                    dtpEffectiveDate.Date = FEndDateLastForwardingPeriod;
                }
                else if (DateTime.Today < FStartDateCurrentPeriod)
                {
                    dtpEffectiveDate.Date = FStartDateCurrentPeriod;
                }
                else
                {
                    dtpEffectiveDate.Date = DateTime.Today;
                }
            }
        }

        /// <summary>
        /// Check if the tax deductable percentage has changed since the original gift and the new gift.
        /// If it has changed, ask the user which one they want to use.
        /// </summary>
        public bool CheckTaxDeductPctChange
        {
            set
            {
                FCheckTaxDeductPctChange = value;
            }
        }

        /// <summary>
        /// Check if the gift destination has changed since the original gift and the new gift.
        /// If it has changed, ask the user which one they want to use.
        /// </summary>
        public bool CheckGiftDestinationChange
        {
            set
            {
                FCheckGiftDestinationChange = value;
            }
        }

        /// <summary>
        /// Gets the batch number containg the adjusted gifts
        /// </summary>
        public int AdjustmentBatchNumber
        {
            get
            {
                return FAdjustmentBatchNumber;
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

            chkNoReceipt.Enabled = ((GiftAdjustmentFunctionEnum)requestParams["Function"] == GiftAdjustmentFunctionEnum.AdjustGift);
        }

        private void GetGiftsForReverseAdjust()
        {
            Boolean ok;
            TVerificationResultCollection Messages;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GetGiftsForReverseAdjust(requestParams, ref FGiftMainDS, out Messages);
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

        /// <summary>
        /// Comments will be automatically completed and not user definied. Comments 1 and 2 will be a direct copy from original.
        /// Comment 3 will contain date of original gift. Currently only used for tax deductible pct adjustments.
        /// </summary>
        public void AutoCompleteComments()
        {
            txtReversalCommentOne.Enabled = false;
            cmbReversalCommentOneType.Enabled = false;
            txtReversalCommentTwo.Enabled = false;
            cmbReversalCommentTwoType.Enabled = false;
            txtReversalCommentThree.Enabled = false;
            cmbReversalCommentThreeType.Enabled = false;

            FAutoCompleteComments = true;
        }

        /// <summary>
        /// Shows batch details for the batch needed to place adjusted gifts.
        /// Used for adjustments where gifts come from different batches: field change and tax deduct pct.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACurrencyCode"></param>
        /// <param name="ABankCostCentre"></param>
        /// <param name="ABankAccountCode"></param>
        /// <param name="AGiftType"></param>
        public void AddBatchDetailsToScreen(int ALedgerNumber, string ACurrencyCode, string ABankCostCentre,
            string ABankAccountCode, string AGiftType)
        {
            lblBatchDetailsLabel.Text = Catalog.GetString("Ledger") + ": " + ALedgerNumber + ",  " +
                                        Catalog.GetString("Currency") + ": " + ACurrencyCode + ",  " +
                                        Catalog.GetString("Bank Cost Centre") + ": " + ABankCostCentre + ",  " +
                                        Catalog.GetString("Bank Account") + ": " + ABankAccountCode + ",  " +
                                        Catalog.GetString("Gift Type") + ": " + AGiftType;

            grpBatchDetails.Visible = true;
            lblBatchDetailsLabel.Visible = true;
        }

        private void BtnOk_Click(System.Object sender, System.EventArgs e)
        {
            # region Validation

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

            #endregion

            AddParam("NewBatchSelected", rbtExistingBatch.Checked);

            if (rbtExistingBatch.Checked)
            {
                AddParam("NewBatchNumber", GetSelectedDetailRow().BatchNumber);
            }
            else
            {
                //check the gift batch date to use
                if ((dtpEffectiveDate.Date < FStartDateCurrentPeriod)
                    || (dtpEffectiveDate.Date > FEndDateLastForwardingPeriod)
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

            if (!FAutoCompleteComments)
            {
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
            }

            AddParam("AutoCompleteComments", FAutoCompleteComments);

            AddParam("NoReceipt", chkNoReceipt.Checked);

            if (FCheckTaxDeductPctChange || FCheckGiftDestinationChange)
            {
                CheckIfFieldsHaveChanged();
            }

            ReverseAdjust(out FAdjustmentBatchNumber);
        }

        // do the actual reversal / adjustment
        private void ReverseAdjust(out int AAdjustmentBatchNumber)
        {
            Boolean ok;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GiftRevertAdjust(requestParams, out AAdjustmentBatchNumber, FGiftMainDS);
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
                            AAdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift Batch"));
                        break;

                    case GiftAdjustmentFunctionEnum.ReverseGiftDetail:
                        MessageBox.Show(Catalog.GetString("Reversed gift detail has been successfully added to Batch " + AAdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift Detail"));
                        break;

                    case GiftAdjustmentFunctionEnum.ReverseGift:
                        MessageBox.Show(Catalog.GetString("Reversed gift has been successfully added to Batch " + AAdjustmentBatchNumber + "."),
                        Catalog.GetString("Reverse Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.AdjustGift:
                        MessageBox.Show(Catalog.GetString("Adjustment transactions have been successfully added to Batch " + AAdjustmentBatchNumber +
                            "."),
                        Catalog.GetString("Adjust Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.FieldAdjust:
                        MessageBox.Show(Catalog.GetString("Gift Field Adjustment transactions have been successfully added to Batch " +
                            AAdjustmentBatchNumber +
                            "."),
                        Catalog.GetString("Adjust Gift"));
                        break;

                    case GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust:
                        MessageBox.Show(Catalog.GetString("Tax Deductible Percentage Adjustment transactions have been successfully added to Batch "
                            +
                            AAdjustmentBatchNumber +
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

        /// <summary>
        /// If applicable, check if the tax deductable percentage and/or gift destination has changed since the original gift and the new gift.
        /// If it has changed, ask the user which one they want to use.
        /// </summary>
        private void CheckIfFieldsHaveChanged()
        {
            // these will contain recipient keys that either need their tax deduct pct updated or that need to use their original gift destination
            List <string[]>UpdateTaxDeductiblePctRecipeints = new List <string[]>();
            List <string>FixedGiftDestinationRecipeints = new List <string>();

            List <string>Recipients = new List <string>();

            // if we don't sort then they are processed in reverse order
            FGiftMainDS.AGiftDetail.DefaultView.Sort = AGiftDetailTable.GetLedgerNumberDBName() + ", " +
                                                       AGiftDetailTable.GetBatchNumberDBName() + ", " +
                                                       AGiftDetailTable.GetGiftTransactionNumberDBName() + ", " +
                                                       AGiftDetailTable.GetDetailNumberDBName();

            foreach (DataRowView RowView in FGiftMainDS.AGiftDetail.DefaultView)
            {
                AGiftDetailRow GiftDetailRow = (AGiftDetailRow)RowView.Row;

                // only want to do once for each recipient
                if (Recipients.Contains(GiftDetailRow.RecipientKey.ToString()))
                {
                    continue;
                }

                Recipients.Add(GiftDetailRow.RecipientKey.ToString());

                string RecipientName;
                TPartnerClass PartnerClass;
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                    GiftDetailRow.RecipientKey, out RecipientName, out PartnerClass);

                // Check the tax deductable percentage
                if (FCheckTaxDeductPctChange && !GiftDetailRow.IsTaxDeductibleNull() && GiftDetailRow.TaxDeductible)
                {
                    // 100% default if tax deductibility is not limited
                    decimal DefaultTaxDeductiblePct = 100;

                    // get the default tax deductible percentage for a new gift made today to the same recipient
                    PPartnerTaxDeductiblePctTable PartnerTaxDeductiblePctTable =
                        TRemote.MFinance.Gift.WebConnectors.LoadPartnerTaxDeductiblePct(GiftDetailRow.RecipientKey);

                    if ((PartnerTaxDeductiblePctTable != null) && (PartnerTaxDeductiblePctTable.Rows.Count > 0))
                    {
                        foreach (PPartnerTaxDeductiblePctRow Row in PartnerTaxDeductiblePctTable.Rows)
                        {
                            // if no valid records exist then the recipient has not limited tax deductible by default
                            if ((Row.PartnerKey == GiftDetailRow.RecipientKey) && (Row.DateValidFrom <= dtpEffectiveDate.Date.Value))
                            {
                                DefaultTaxDeductiblePct = Row.PercentageTaxDeductible;
                            }
                        }
                    }

                    // if different from current paercentage ask the user which one they want to use
                    if ((GiftDetailRow.TaxDeductiblePct != DefaultTaxDeductiblePct)
                        && (MessageBox.Show(string.Format(Catalog.GetString(
                                        "The default tax deductible percentage for the recipient {0} ({1}) for {2} ({3}%) is different from the tax deductible "
                                        +
                                        "percentage recorded for the gift detail to be adjusted ({4}%).{5}Do you want to continue to use the original percentage "
                                        +
                                        "of {4}% for the adjusted gift?"),
                                    RecipientName, GiftDetailRow.RecipientKey.ToString("0000000000"),
                                    dtpEffectiveDate.Date.Value.ToString("dd-MMM-yyyy"), DefaultTaxDeductiblePct, GiftDetailRow.TaxDeductiblePct,
                                    "\r\n\r\n"),
                                Catalog.GetString("Adjust Gift"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                            == DialogResult.No))
                    {
                        UpdateTaxDeductiblePctRecipeints.Add(
                            new string[] { GiftDetailRow.RecipientKey.ToString(), DefaultTaxDeductiblePct.ToString() });
                    }
                }

                // Check the gift destination
                if (FCheckGiftDestinationChange)
                {
                    Int64 RecipientLedgerNumber = 0;

                    // get the recipient ledger number
                    if (GiftDetailRow.RecipientKey > 0)
                    {
                        RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(GiftDetailRow.RecipientKey,
                            dtpEffectiveDate.Date.Value);
                    }

                    TVerificationResultCollection VerificationResults;

                    // if recipient ledger number belongs to a different ledger then check that it is set up for inter-ledger transfers
                    if ((RecipientLedgerNumber != 0) && (((int)RecipientLedgerNumber / 1000000 == GiftDetailRow.LedgerNumber)
                                                         || TRemote.MFinance.Gift.WebConnectors.IsRecipientLedgerNumberSetupForILT(
                                                             GiftDetailRow.LedgerNumber, GiftDetailRow.RecipientKey, RecipientLedgerNumber,
                                                             out VerificationResults)))
                    {
                        if (RecipientLedgerNumber != GiftDetailRow.RecipientLedgerNumber)
                        {
                            string NewFieldShortName;
                            string OldFieldShortName;
                            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                                RecipientLedgerNumber, out NewFieldShortName, out PartnerClass);
                            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                                GiftDetailRow.RecipientLedgerNumber, out OldFieldShortName, out PartnerClass);

                            if (MessageBox.Show(string.Format(Catalog.GetString(
                                            "The default gift destination for the recipient {0} ({1}) for {2} ({3} ({4})) is different from the gift destination "
                                            +
                                            "recorded for the gift detail to be adjusted ({5} ({6})).{7}Do you want to continue to use the original "
                                            +
                                            "gift destination of {5} ({6}) for the adjusted gift?"),
                                        RecipientName, GiftDetailRow.RecipientKey.ToString("0000000000"),
                                        dtpEffectiveDate.Date.Value.ToString("dd-MMM-yyyy"), NewFieldShortName, RecipientLedgerNumber,
                                        OldFieldShortName, GiftDetailRow.RecipientLedgerNumber, "\r\n\r\n"),
                                    Catalog.GetString("Adjust Gift"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2)
                                == DialogResult.Yes)
                            {
                                // the gift destination for this gift detail will not be changeable
                                FixedGiftDestinationRecipeints.Add(GiftDetailRow.RecipientKey.ToString());
                            }
                        }
                    }
                }
            }

            if (UpdateTaxDeductiblePctRecipeints.Count > 0)
            {
                AddParam("UpdateTaxDeductiblePct", UpdateTaxDeductiblePctRecipeints);
            }

            if (FixedGiftDestinationRecipeints.Count > 0)
            {
                AddParam("FixedGiftDestination", FixedGiftDestinationRecipeints);
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