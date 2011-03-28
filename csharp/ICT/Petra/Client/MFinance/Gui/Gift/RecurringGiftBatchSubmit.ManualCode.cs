//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash,timop
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
using System.IO;
using System.Windows.Forms;

using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Manual code for the Gift Batch export
    /// </summary>
    public partial class TFrmRecurringGiftBatchSubmit
    {
        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
            dtpEffectiveDate.Date = DateTime.Now;
        }

        private Ict.Petra.Shared.MFinance.Gift.Data.RecurringGiftBatchTDS FMainDS;
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;
        private ARecurringGiftBatchRow batchRow;

        /// Batch number for the recurring batch to be submitted
        public ARecurringGiftBatchRow BatchRow

        {
            set
            {
                batchRow = value;
                FBatchNumber = batchRow.BatchNumber;
                txtExchangeRateToBase.Text = "1"; //1.0M;
                txtCurrencyCodeFrom.Text = batchRow.CurrencyCode;
            }
        }
        DateTime StartDateCurrentPeriod;
        DateTime EndDateLastForwardingPeriod;
        /// dataset for the whole screen
        public Ict.Petra.Shared.MFinance.Gift.Data.RecurringGiftBatchTDS MainDS
        {
            set
            {
                FMainDS = value;

                FLedgerNumber = FMainDS.ALedger[0].LedgerNumber;
                txtCurrencyCodeTo.Text = FMainDS.ALedger[0].BaseCurrency;

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
        /// this submits the given Batch number
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void SubmitBatch(object sender, EventArgs e)
        {
            //check the gift batch date
            if (dtpEffectiveDate.Date < StartDateCurrentPeriod)
            {
                dtpEffectiveDate.Date = StartDateCurrentPeriod;
                MessageBox.Show(Catalog.GetString("Your Date was outside the allowed posting period."));
                return;
            }

            if (dtpEffectiveDate.Date > EndDateLastForwardingPeriod)
            {
                dtpEffectiveDate.Date = EndDateLastForwardingPeriod;
                MessageBox.Show(Catalog.GetString("Your Date was outside the allowed posting period."));
                return;
            }

            foreach (ARecurringGiftRow gift in FMainDS.ARecurringGift.Rows)
            {
                if ((gift.BatchNumber == FBatchNumber) && (gift.LedgerNumber == FLedgerNumber)
                    && gift.Active)
                {
                    foreach (ARecurringGiftDetailRow giftDetail in FMainDS.ARecurringGiftDetail.Rows)
                    {
                        if ((giftDetail.BatchNumber == FBatchNumber)

                            && (giftDetail.LedgerNumber == FLedgerNumber)
                            && (giftDetail.GiftTransactionNumber == gift.GiftTransactionNumber)
                            && ((giftDetail.StartDonations == null) || (giftDetail.StartDonations <= DateTime.Today))
                            && ((giftDetail.EndDonations == null) || (giftDetail.EndDonations >= DateTime.Today)))
                        {
                            goto Found;
                        }
                    }
                }
            }

            MessageBox.Show(Catalog.GetString("There are no gifts in this batch that are active or ") + Environment.NewLine +
                Catalog.GetString("where today's date falls within the Donation Period."));
            Close();
            return;
Found:
            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("ABatchNumber", FBatchNumber);
            requestParams.Add("AExchangeRateToBase", Convert.ToDecimal(txtExchangeRateToBase.Text));
            requestParams.Add("AEffectiveDate", dtpEffectiveDate.Date);
            TVerificationResultCollection AMessages;
            Boolean submitOK = TRemote.MFinance.Gift.WebConnectors.SubmitRecurringGiftBatch(requestParams, out AMessages);

            if (submitOK)
            {
                MessageBox.Show(Catalog.GetString("Your recurring batch  was submitted successfully!"),
                    Catalog.GetString("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult(Catalog.GetString("Submitting the batch failed!") + Environment.NewLine +
                        Catalog.GetString("Reasons:"), AMessages));
            }

            Close();
        }

        void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }
    }
}