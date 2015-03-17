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
using System.Threading;
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
            if ((APartnerKey > 0) && AValidSelection)
            {
                // get recipeint's current Gift Destination
                Int64 RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey, DateTime.Today);

                TVerificationResultCollection VerificationResults;

                // if recipient ledger number belongs to a different ledger then check that it is set up for inter-ledger transfers
                if (((int)RecipientLedgerNumber / 1000000 != FLedgerNumber)
                    && !TRemote.MFinance.Gift.WebConnectors.IsRecipientLedgerNumberSetupForILT(
                        FLedgerNumber, APartnerKey, RecipientLedgerNumber, out VerificationResults))
                {
                    MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Invalid Data Entered"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // use a thread because the textbox text has not actually been set yet if using the partner find screen
                    Thread NewThread = new Thread(() => ResetReceipientKey(APartnerKey));
                    NewThread.Start();

                    return;
                }

                txtCurrentField.Text = RecipientLedgerNumber.ToString();
            }
            else
            {
                txtCurrentField.Text = "0";
            }
        }

        private void ResetReceipientKey(Int64 APartnerKey)
        {
            while (Convert.ToInt64(txtRecipientKey.Text) != APartnerKey)
            {
                Thread.Sleep(50);
            }

            txtRecipientKey.Text = "0";
        }

        // carry out the field adjustment
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

            // show the list of gifts to be adjusted and ask the user for confirmation
            TFrmGiftFieldAdjustmentConfirmation ConfirmationForm = new TFrmGiftFieldAdjustmentConfirmation(this);
            ConfirmationForm.MainDS = GiftBatchDS;

            if (ConfirmationForm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            // Carry out the gift adjustment
            GiftAdjustment(GiftBatchDS, null, chkNoReceipt.Checked, this);

            // refresh batches
            this.Cursor = Cursors.WaitCursor;
            ((TFrmGiftBatch)FPetraUtilsObject.GetCallerForm()).RefreshAll();
            this.Cursor = Cursors.Default;
            this.Close();
        }

        /// <summary>
        /// Carry out the gift adjustment (field or tax deductible pct)
        /// </summary>
        /// <param name="AGiftBatchDS">Gift Batch containing GiftDetail rows for all gifts to be adjusted.</param>
        /// <param name="ANewPct">New Tax Deductible Percentage (null if not being used)</param>
        /// <param name="ANoReceipt">True if no receipt</param>
        /// <param name="AParentForm"></param>
        public static void GiftAdjustment(GiftBatchTDS AGiftBatchDS, decimal? ANewPct, bool ANoReceipt, Form AParentForm)
        {
            // sort gift batches so like batches are together
            AGiftBatchDS.AGiftBatch.DefaultView.Sort = AGiftBatchTable.GetLedgerNumberDBName() + " ASC, " +
                                                       AGiftBatchTable.GetCurrencyCodeDBName() + " ASC, " +
                                                       AGiftBatchTable.GetBankCostCentreDBName() + " ASC, " +
                                                       AGiftBatchTable.GetBankAccountCodeDBName() + " ASC, " + AGiftBatchTable.GetGiftTypeDBName() +
                                                       " ASC";

            GiftBatchTDS NewGiftDS = new GiftBatchTDS();
            NewGiftDS.AGiftDetail.Merge(new GiftBatchTDSAGiftDetailTable());

            for (int i = 0; i < AGiftBatchDS.AGiftBatch.Rows.Count; i++)
            {
                AGiftBatchRow OldGiftBatch = (AGiftBatchRow)AGiftBatchDS.AGiftBatch.DefaultView[i].Row;
                AGiftBatchRow NextGiftBatch = null;

                // add batch's gift/s to dataset
                DataView Gifts = new DataView(AGiftBatchDS.AGift);
                Gifts.RowFilter = string.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    OldGiftBatch.BatchNumber);

                foreach (DataRowView giftRows in Gifts)
                {
                    AGiftRow gR = (AGiftRow)giftRows.Row;
                    NewGiftDS.AGift.ImportRow(gR);
                }

                // add batch's gift detail/s to dataset
                DataView GiftDetails = new DataView(AGiftBatchDS.AGiftDetail);
                GiftDetails.RowFilter = string.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    OldGiftBatch.BatchNumber);

                foreach (DataRowView giftDetailRows in GiftDetails)
                {
                    AGiftDetailRow gDR = (AGiftDetailRow)giftDetailRows.Row;
                    NewGiftDS.AGiftDetail.ImportRow(gDR);
                }

                // if not the last row
                if (i != AGiftBatchDS.AGiftBatch.Rows.Count - 1)
                {
                    NextGiftBatch = (AGiftBatchRow)AGiftBatchDS.AGiftBatch.DefaultView[i + 1].Row;
                }

                // if this is the last batch or if the next batch's gifts need to be added to a different new batch
                if ((NextGiftBatch == null)
                    || (NextGiftBatch.CurrencyCode != OldGiftBatch.CurrencyCode)
                    || (NextGiftBatch.BankCostCentre != OldGiftBatch.BankCostCentre)
                    || (NextGiftBatch.BankAccountCode != OldGiftBatch.BankAccountCode)
                    || (NextGiftBatch.GiftType != OldGiftBatch.GiftType))
                {
                    TFrmGiftRevertAdjust AdjustForm = new TFrmGiftRevertAdjust(AParentForm);

                    try
                    {
                        AParentForm.ShowInTaskbar = false;

                        AdjustForm.LedgerNumber = OldGiftBatch.LedgerNumber;
                        AdjustForm.CurrencyCode = OldGiftBatch.CurrencyCode;
                        AdjustForm.Text = "Adjust Gift";
                        AdjustForm.AddParam("Function", GiftAdjustmentFunctionEnum.FieldAdjust);
                        AdjustForm.GiftMainDS = NewGiftDS;
                        AdjustForm.NoReceipt = ANoReceipt;

                        if (ANewPct != null)
                        {
                            AdjustForm.AddParam("Function", GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust);
                            AdjustForm.AddParam("NewPct", ANewPct);
                        }
                        else
                        {
                            AdjustForm.AddParam("Function", GiftAdjustmentFunctionEnum.FieldAdjust);
                        }

                        AdjustForm.GiftDetailRow = NewGiftDS.AGiftDetail[0];

                        if (AdjustForm.IsDisposed || (AdjustForm.ShowDialog() != DialogResult.OK))
                        {
                            return;
                        }
                    }
                    finally
                    {
                        AParentForm.Cursor = Cursors.WaitCursor;
                        AdjustForm.Dispose();
                        AParentForm.ShowInTaskbar = true;
                        NewGiftDS.AGiftDetail.Clear();
                        NewGiftDS.AGift.Clear();
                        AParentForm.Cursor = Cursors.Default;
                    }
                }
            }
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
            if ((AGiftBatchDS.AGiftDetail == null) || (AGiftBatchDS.AGiftDetail.Rows.Count == 0))
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

            if (!string.IsNullOrEmpty(dtpEndDate.Text) && (dtpEndDate.Date < dtpStartDate.Date))
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