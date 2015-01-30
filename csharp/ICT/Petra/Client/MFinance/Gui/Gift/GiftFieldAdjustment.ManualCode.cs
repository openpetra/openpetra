//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters
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
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftFieldAdjustment
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
            }
        }

        private void InitializeManualCode()
        {
            lblDateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            txtRecipientKey.PartnerClass = "WORKER,FAMILY";
        }

        private void RecipientKeyChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (APartnerKey > 0 && AValidSelection)
            {
                // get recipeint's current Gift Destination
                txtCurrentField.Text = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey, DateTime.Today).ToString();
            }
            else
            {
                txtCurrentField.Text = "0";
            }
        }

        // carry out the adjustment
        private void FieldChangeAdjustment(System.Object sender, EventArgs e)
        {
            if (!ValidateControls())
            {
                return;
            }

            GiftBatchTDS GiftBatchDS = new GiftBatchTDS();

            // get all the data needed for this Field Adjustment
            if (!GetAllDataNeeded(ref GiftBatchDS))
            {
                return;
            }

            // show the list of gift to be adjusted and ask the user for confirmation
            TFrmGiftFieldAdjustmentConfirmation ConfirmationForm = new TFrmGiftFieldAdjustmentConfirmation(this);
            ConfirmationForm.MainDS = GiftBatchDS;
            
            if (ConfirmationForm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            // sort gift batches so like batches are together
            GiftBatchDS.AGiftBatch.DefaultView.Sort = AGiftBatchTable.GetCurrencyCodeDBName() + " ASC, " + AGiftBatchTable.GetBankCostCentreDBName() + " ASC, " +
                AGiftBatchTable.GetBankAccountCodeDBName() + " ASC, " + AGiftBatchTable.GetGiftTypeDBName() + " ASC";

            GiftBatchTDS NewGiftDS = new GiftBatchTDS();
            NewGiftDS.AGiftDetail.Merge(new GiftBatchTDSAGiftDetailTable());

            for (int i = 0; i < GiftBatchDS.AGiftBatch.Rows.Count; i++)
            {
                AGiftBatchRow OldGiftBatch = (AGiftBatchRow)GiftBatchDS.AGiftBatch.DefaultView[i].Row;
                AGiftBatchRow NextGiftBatch = null;

                // add batch's gift/s to dataset
                DataView Gifts = new DataView(GiftBatchDS.AGift);
                Gifts.RowFilter = string.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    OldGiftBatch.BatchNumber);

                foreach (DataRowView giftRows in Gifts)
                {
                    AGiftRow gR = (AGiftRow)giftRows.Row;
                    NewGiftDS.AGift.ImportRow(gR);
                }

                // add batch's gift detail/s to dataset
                DataView GiftDetails = new DataView(GiftBatchDS.AGiftDetail);
                GiftDetails.RowFilter = string.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    OldGiftBatch.BatchNumber);

                foreach (DataRowView giftDetailRows in GiftDetails)
                {
                    AGiftDetailRow gDR = (AGiftDetailRow)giftDetailRows.Row;
                    NewGiftDS.AGiftDetail.ImportRow(gDR);
                }

                // if not the last row
                if (i != GiftBatchDS.AGiftBatch.Rows.Count - 1)
                {
                    NextGiftBatch = (AGiftBatchRow)GiftBatchDS.AGiftBatch.DefaultView[i+1].Row;
                }

                // if this is the last batch or if the next batch's gifts need to be added to a different new batch
                if (NextGiftBatch == null || 
                    NextGiftBatch.CurrencyCode != OldGiftBatch.CurrencyCode || 
                    NextGiftBatch.BankCostCentre != OldGiftBatch.BankCostCentre || 
                    NextGiftBatch.BankAccountCode != OldGiftBatch.BankAccountCode || 
                    NextGiftBatch.GiftType != OldGiftBatch.GiftType)
                {
                    TFrmGiftRevertAdjust AdjustForm = new TFrmGiftRevertAdjust(FPetraUtilsObject.GetForm());

                    try
                    {
                        FPetraUtilsObject.GetCallerForm().ShowInTaskbar = false;

                        AdjustForm.LedgerNumber = FLedgerNumber;
                        AdjustForm.CurrencyCode = OldGiftBatch.CurrencyCode;
                        AdjustForm.Text = "Adjust Gift";
                        AdjustForm.AddParam("Function", GiftAdjustmentFunctionEnum.FieldAdjust);
                        AdjustForm.GiftMainDS = NewGiftDS;
                        AdjustForm.NoReceipt = chkNoReceipt.Checked;

                        AdjustForm.GiftDetailRow = NewGiftDS.AGiftDetail[0];

                        if (AdjustForm.IsDisposed || AdjustForm.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.WaitCursor;
                        AdjustForm.Dispose();
                        FPetraUtilsObject.GetCallerForm().ShowInTaskbar = true;
                        NewGiftDS.AGiftDetail.Clear();
                        NewGiftDS.AGift.Clear();
                        this.Cursor = Cursors.Default;
                    }
                }
            }

            // refresh batches
            this.Cursor = Cursors.WaitCursor;
            ((TFrmGiftBatch) FPetraUtilsObject.GetCallerForm()).RefreshAll();
            this.Cursor = Cursors.Default;
            this.Close();
        }

        private bool GetAllDataNeeded(ref GiftBatchTDS AGiftBatchDS)
        {
            // if dtpEndDate is blank use the max date value
            DateTime EndTime = DateTime.MaxValue;

            if (dtpEndDate.Date.HasValue)
            {
                EndTime = dtpEndDate.Date.Value;
            }

            Boolean ok;
            TVerificationResultCollection Messages;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GetGiftsForFieldChangeAdjustment(
                    ref AGiftBatchDS,
                    FLedgerNumber,
                    Convert.ToInt64(txtRecipientKey.Text),
                    dtpStartDate.Date.Value,
                    EndTime,
                    Convert.ToInt64(txtOldFieldKey.Text),
                    out Messages);
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
                                Catalog.GetString("Gift Field Adjust"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                return false;
            }

            // if there are no gifts to be adjusted
            if (AGiftBatchDS.AGiftDetail == null || AGiftBatchDS.AGiftDetail.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no gifts to adjust."));
                return false;
            }

            return true;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }

        private bool ValidateControls()
        {
            if (!dtpStartDate.Date.HasValue)
            {
                MessageBox.Show(Catalog.GetString("Please supply a valid 'From' date."));
                dtpStartDate.Focus();
                dtpStartDate.SelectAll();
                return false;
            }

            if (!string.IsNullOrEmpty(dtpEndDate.Text) && !dtpEndDate.Date.HasValue)
            {
                MessageBox.Show(Catalog.GetString("The 'To' date is not a valid date."));
                dtpEndDate.Focus();
                dtpEndDate.SelectAll();
                return false;
            }

            if (!string.IsNullOrEmpty(dtpEndDate.Text) && dtpEndDate.Date < dtpStartDate.Date)
            {
                MessageBox.Show(Catalog.GetString("The 'To' date must come after the 'From' date."));
                dtpEndDate.Focus();
                dtpEndDate.SelectAll();
                return false;
            }

            if (txtRecipientKey.CurrentPartnerClass != TPartnerClass.FAMILY)
            {
                MessageBox.Show(Catalog.GetString("The Recipient must be a valid Family partner."));
                txtRecipientKey.Focus();
                return false;
            }

            if (txtCurrentField.Text == txtOldFieldKey.Text)
            {
                MessageBox.Show(Catalog.GetString("The Current and Old field must be different."));
                return false;
            }

            return true;
        }
    }
}