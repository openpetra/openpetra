//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Perform a Tax Deductible percentage adjustment
    /// </summary>
    public static class TFrmGiftTaxDeductiblePctAdjustment
    {
        // carry out the adjustment
        /// <summary>
        /// Carry out a Tax Deductible Pct adjustment.
        /// </summary>
        /// <param name="ARecipientKey"></param>
        /// <param name="ANewPct"></param>
        /// <param name="AValidFrom"></param>
        /// <param name="ANoReceipt"></param>
        /// <param name="AParentForm"></param>
        public static void TaxDeductiblePctAdjustment(Int64 ARecipientKey, decimal ANewPct, DateTime AValidFrom, bool ANoReceipt, Form AParentForm)
        {
            GiftBatchTDS GiftBatchDS = new GiftBatchTDS();

            // get all the data needed for this Field Adjustment
            if (!GetAllDataNeeded(ref GiftBatchDS, ARecipientKey, ANewPct, AValidFrom, AParentForm))
            {
                return;
            }

            // show the list of gifts to be adjusted and ask the user for confirmation
            TFrmGiftFieldAdjustmentConfirmation ConfirmationForm = new TFrmGiftFieldAdjustmentConfirmation(AParentForm);
            ConfirmationForm.MainDS = GiftBatchDS;
            ConfirmationForm.Text = Catalog.GetString("Confirm Tax Deductible Percentage Adjustment");

            if (ConfirmationForm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            // Carry out the gift adjustment
            TFrmGiftFieldAdjustment.GiftAdjustment(GiftBatchDS, ANewPct, ANoReceipt, AParentForm);

            // refresh gift batch screen
            TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcRefreshGiftBatches, AParentForm.ToString());
            TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
        }

        private static bool GetAllDataNeeded(ref GiftBatchTDS AGiftBatchDS, Int64 ARecipientKey, decimal ANewPct, DateTime AValidFrom, Form AForm)
        {
            Boolean ok;
            TVerificationResultCollection Messages;

            try
            {
                AForm.Cursor = Cursors.WaitCursor;

                ok = TRemote.MFinance.Gift.WebConnectors.GetGiftsForTaxDeductiblePctAdjustment(
                    ref AGiftBatchDS,
                    ARecipientKey,
                    AValidFrom,
                    ANewPct,
                    out Messages);
            }
            finally
            {
                AForm.Cursor = Cursors.Default;
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
                            MessageBox.Show(AForm.Text + Catalog.GetString(" cancelled. ") + message.ResultText,
                                Catalog.GetString("Tax Deductible Percentage Adjust"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}