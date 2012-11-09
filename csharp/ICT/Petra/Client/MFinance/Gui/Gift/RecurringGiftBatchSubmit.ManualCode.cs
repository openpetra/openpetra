//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop
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
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Setup;
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

        private GiftBatchTDS FMainDS;
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;
        private ARecurringGiftBatchRow batchRow;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// Batch number for the recurring batch to be submitted
        public ARecurringGiftBatchRow BatchRow

        {
            set
            {
                batchRow = value;
                FBatchNumber = batchRow.BatchNumber;
                txtExchangeRateToBase.Text = TExchangeRateCache.GetDailyExchangeRate(
                    FMainDS.ALedger[0].BaseCurrency,
                    batchRow.CurrencyCode,
                    DateTime.Now).ToString();
                txtCurrencyCodeFrom.Text = batchRow.CurrencyCode;

                if (batchRow.CurrencyCode == FMainDS.ALedger[0].BaseCurrency)
                {
                    txtExchangeRateToBase.Enabled = false;
                    txtExchangeRateToBase.BackColor = Color.LightPink;
                }
                else
                {
                    txtExchangeRateToBase.Enabled = true;
                    txtExchangeRateToBase.BackColor = Color.Empty;
                }
            }
        }

        DateTime FStartDateCurrentPeriod;
        DateTime FEndDateLastForwardingPeriod;
        /// dataset for the whole screen
        public GiftBatchTDS MainDS
        {
            set
            {
                FMainDS = value;

                FLedgerNumber = FMainDS.ALedger[0].LedgerNumber;
                txtCurrencyCodeTo.Text = FMainDS.ALedger[0].BaseCurrency;

                DateTime DefaultDate;
                TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                    out FStartDateCurrentPeriod,
                    out FEndDateLastForwardingPeriod,
                    out DefaultDate);
                lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                    FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());
            }
        }


        /// <summary>
        /// this submits the given Batch number
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void SubmitBatch(object sender, EventArgs e)
        {
            bool validGiftDetailFound = false;
            decimal exchRateToBase = 0;

            if (!(Decimal.TryParse(txtExchangeRateToBase.Text, out exchRateToBase) && (exchRateToBase > 0)))
            {
                MessageBox.Show(Catalog.GetString("The exchange rate must be a number greater than 0."));
                txtExchangeRateToBase.Focus();
                txtExchangeRateToBase.SelectAll();
                return;
            }

            //check the gift batch date
            if (dtpEffectiveDate.Date < FStartDateCurrentPeriod)
            {
                MessageBox.Show(Catalog.GetString("Your date was before the allowed posting period start date: " +
                        FStartDateCurrentPeriod.ToShortDateString()));
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
                return;
            }

            if (dtpEffectiveDate.Date > FEndDateLastForwardingPeriod)
            {
                MessageBox.Show(Catalog.GetString("Your date was later than the allowed posting period end date: " +
                        FEndDateLastForwardingPeriod.ToShortDateString()));
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
                return;
            }

            //Check if any details have been loaded yet
            FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = ARecurringGiftDetailTable.GetBatchNumberDBName() + "=" + FBatchNumber.ToString();

            if (FMainDS.ARecurringGiftDetail.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(FLedgerNumber, FBatchNumber));
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
                            && ((giftDetail.StartDonations == null) || (giftDetail.StartDonations <= dtpEffectiveDate.Date))
                            && ((giftDetail.EndDonations == null) || (giftDetail.EndDonations >= dtpEffectiveDate.Date)))
                        {
                            validGiftDetailFound = true;
                            break;
                        }
                    }
                }
            }

            if (!validGiftDetailFound)
            {
                MessageBox.Show(Catalog.GetString("There are no gifts in this batch that are active or ") + Environment.NewLine +
                    Catalog.GetString("where the entered Gift Batch date falls within the Donation Period."));

                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
            }
            else
            {
                Hashtable requestParams = new Hashtable();
                requestParams.Add("ALedgerNumber", FLedgerNumber);
                requestParams.Add("ABatchNumber", FBatchNumber);
                requestParams.Add("AExchangeRateToBase", Convert.ToDecimal(txtExchangeRateToBase.Text));
                requestParams.Add("AEffectiveDate", dtpEffectiveDate.Date.Value);
                TVerificationResultCollection AMessages;

                Boolean submitOK = TRemote.MFinance.Gift.WebConnectors.SubmitRecurringGiftBatch(requestParams, out AMessages);

                if (submitOK)
                {
                    MessageBox.Show(Catalog.GetString("Your recurring batch was submitted successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(Catalog.GetString("Submitting the batch failed!") +
                            Environment.NewLine +
                            Catalog.GetString("Reasons:"), AMessages));
                }

                Close();
            }
        }

        void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }

        private void CheckBatchEffectiveDate(object sender, EventArgs e)
        {
            DateTime dateValue;
            string aDate = dtpEffectiveDate.Text;

            if (DateTime.TryParse(aDate, out dateValue))
            {
                txtExchangeRateToBase.Text = TExchangeRateCache.GetDailyExchangeRate(
                    txtCurrencyCodeTo.Text,
                    txtCurrencyCodeFrom.Text,
                    dateValue).ToString();
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Invalid date entered!"));
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
            }
        }

        private void SetExchangeRateValue(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate setupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            if (setupDailyExchangeRate.ShowDialog(FLedgerNumber, dtpEffectiveDate.Date.Value,
                    txtCurrencyCodeFrom.Text,
                    DEFAULT_CURRENCY_EXCHANGE) == DialogResult.Cancel)
            {
                return;
            }

//            if (FPreviouslySelectedDetailRow.ExchangeRateToBase != setupDailyExchangeRate.CurrencyExchangeRate)
//            {
//                //Enforce save needed condition
//                FPetraUtilsObject.SetChangedFlag();
//            }

            txtExchangeRateToBase.Text = setupDailyExchangeRate.CurrencyExchangeRate.ToString();
        }
    }
}