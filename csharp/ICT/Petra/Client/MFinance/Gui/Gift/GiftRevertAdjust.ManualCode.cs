//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
        private GiftBatchTDS giftMainDS = null;
//        private AGiftBatchRow giftBatchRow = null;   // TODO Decide whether to remove altogether
        private AGiftDetailRow giftDetailRow = null;
        private Boolean ok = false;
        DateTime StartDateCurrentPeriod;
        DateTime EndDateLastForwardingPeriod;

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
        /// A gift DS is injected if needed
        /// </summary>
        public GiftBatchTDS GiftMainDS {
            set
            {
                giftMainDS = value;
            }
        }

// TODO Decide whether to remove altogether
//        /// <summary>
//        /// A Gift Batch Row is injected
//        /// </summary>
//        public AGiftBatchRow GiftBatchRow {
//            set
//            {
//                giftBatchRow = value;
//            }
//        }

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
            }
        }

        /// <summary>
        /// Ledger Number is injected
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
                lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                    StartDateCurrentPeriod.ToShortDateString(), EndDateLastForwardingPeriod.ToShortDateString());
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

        private void RevertAdjust(System.Object sender, System.EventArgs e)
        {
            if (chkSelect.Checked && (GetSelectedDetailRow() == null))
            {
                // nothing seleted
                MessageBox.Show(Catalog.GetString("Please select a batch."));
                return;
            }

            if ((giftDetailRow != null) && giftDetailRow.ModifiedDetail)
            {
                MessageBox.Show(Catalog.GetString("A Gift can only be reversed once!"));
                return;
            }

            if (dtpEffectiveDate.Enabled && (dtpEffectiveDate.Text.Trim().Length == 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a valid batch date."));
                dtpEffectiveDate.Focus();
                return;
            }

            Boolean ok;
            TVerificationResultCollection AMessages;

            AddParam("NewBatchSelected", chkSelect.Checked);

            if (chkSelect.Checked)
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
                    //dtpEffectiveDate.Date = StartDateCurrentPeriod;
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

            AddParam("BatchNumber", giftDetailRow.BatchNumber);
            AddParam("GiftNumber", giftDetailRow.GiftTransactionNumber);
            AddParam("GiftDetailNumber", giftDetailRow.DetailNumber);
            AddParam("CostCentre", giftDetailRow.CostCentreCode);
            AddParam("ReversalCommentOne", txtReversalCommentOne.Text);
            AddParam("ReversalCommentTwo", txtReversalCommentTwo.Text);
            AddParam("ReversalCommentThree", txtReversalCommentThree.Text);
            AddParam("ReversalCommentOneType", cmbReversalCommentOneType.Text);
            AddParam("ReversalCommentTwoType", cmbReversalCommentTwoType.Text);
            AddParam("ReversalCommentThreeType", cmbReversalCommentThreeType.Text);

            try
            {
                this.Cursor = Cursors.WaitCursor;
                ok = TRemote.MFinance.Gift.WebConnectors.GiftRevertAdjust(requestParams, out AMessages);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (ok)
            {
                String function = (String)requestParams["Function"];

                switch (function)
                {
                    case "ReverseGiftBatch":
                        MessageBox.Show(Catalog.GetString("Your batch has been successfully reversed"),
                        Catalog.GetString("Reverse Gift Batch"));
                        break;

                    case "ReverseGiftDetail":
                        MessageBox.Show(Catalog.GetString("Your gift detail has been successfully reversed"),
                        Catalog.GetString("Reverse Gift Detail"));
                        break;

                    case "ReverseGift":
                        MessageBox.Show(Catalog.GetString("Your gift has been successfully reversed"),
                        Catalog.GetString("Reverse Gift"));
                        break;

                    case "AdjustGift":
                        MessageBox.Show(Catalog.GetString("Your gift has been successfully adjusted"),
                        Catalog.GetString("Adjust Gift"));
                        break;
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                ShowMessages(AMessages);
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                Close();
            }

            return;
        }

        private void InitializeManualCode()
        {
            grdDetails.Visible = false;

            //FLedger is still zero at this point
            FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                AGiftBatchTable.GetBatchStatusDBName(),
                MFinanceConstants.BATCH_UNPOSTED
                );
            FMainDS.AGiftBatch.DefaultView.Sort = AGiftBatchTable.GetBatchNumberDBName() + " DESC";

            SelectBatchChanged(null, null);

            //add the focused event temporarily to allow execution of more manual code right at the
            //  end of the initialisation process.
            this.btnHelp.Enter += new System.EventHandler(this.HelpFocussed);
        }

        private void HelpFocussed(System.Object sender, EventArgs e)
        {
            grdDetails.DataSource = null;
            grdDetails.Visible = true;
            dtpEffectiveDate.Focus();
            this.btnOK.Enter -= new System.EventHandler(this.HelpFocussed);
            chkSelect.Enabled = (giftMainDS == null);
        }

        private void SelectBatchChanged(System.Object sender, EventArgs e)
        {
            bool isChecked = chkSelect.Checked;

            if (isChecked)
            {
                //First pass FLedgerNumber = 0 so need to add Ledger to the filter when the user first checks the checkbox
                if ((FLedgerNumber != 0) && !FMainDS.AGiftBatch.DefaultView.RowFilter.Contains(AGiftBatchTable.GetLedgerNumberDBName()))
                {
                    FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = {1} AND {2} = '{3}'",
                        AGiftBatchTable.GetLedgerNumberDBName(),
                        FLedgerNumber,
                        AGiftBatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED
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

        private void ShowMessages(TVerificationResultCollection AMessages)
        {
            string ErrorMessages = String.Empty;

            if (AMessages.Count > 0)
            {
                foreach (TVerificationResult message in AMessages)
                {
                    ErrorMessages += "[" + message.ResultContext + "] " + message.ResultTextCaption + ": " + message.ResultText + Environment.NewLine;
                }
            }

            if (ErrorMessages.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CheckBatchEffectiveDate(object sender, EventArgs e)
        {
            DateTime dateValue;
            string aDate = dtpEffectiveDate.Text.Trim();

            if ((aDate.Length > 0) && !DateTime.TryParse(aDate, out dateValue))
            {
                MessageBox.Show(Catalog.GetString("Invalid date entered!"));
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
            }
        }
    }
}