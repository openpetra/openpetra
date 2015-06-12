//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, christophert
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
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Batch export
    /// </summary>
    public partial class TFrmRecurringGLBatchSubmit
    {
        private GLBatchTDS FMainDS;
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;
        private ARecurringBatchRow FBatchRow;
        private string FBaseCurrencyCode;
        private string FCurrencyCode;
        private Decimal FExchangeRateToBase = 0;
        private Decimal FExchangeRateIntlToBase = 0;

        DateTime FStartDateCurrentPeriod;
        DateTime FEndDateLastForwardingPeriod;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// dataset for the whole screen
        public GLBatchTDS MainDS
        {
            set
            {
                FMainDS = value;

                FLedgerNumber = FMainDS.ALedger[0].LedgerNumber;

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
        /// Batch number for the recurring batch to be submitted
        /// </summary>
        public ARecurringBatchRow BatchRow
        {
            set
            {
                FBatchRow = value;
                FBatchNumber = FBatchRow.BatchNumber;
                this.Text = String.Format(Catalog.GetString("Submit recurring Batch {0}"), FBatchRow.BatchNumber);

                SetCurrencyControls();
            }
        }

        private void InitializeManualCode()
        {
            dtpEffectiveDate.Date = DateTime.Now;
        }

        private void SetCurrencyControls()
        {
            FBaseCurrencyCode = FMainDS.ALedger[0].BaseCurrency;

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            if (JournalDV.Count == 0)
            {
                //Submitting an empty batch
                FCurrencyCode = FBaseCurrencyCode;
            }
            else
            {
                // All Journals in a recurring batch have the same currency code
                ARecurringJournalRow jr = (ARecurringJournalRow)JournalDV[0].Row;

                FCurrencyCode = jr.TransactionCurrency;
            }

            txtCurrencyCodeFrom.Text = FCurrencyCode;
            txtCurrencyCodeTo.Text = FBaseCurrencyCode;

            if (FCurrencyCode == FBaseCurrencyCode)
            {
                txtExchangeRateToBase.Enabled = false;
                txtExchangeRateToBase.BackColor = Color.LightPink;
            }
            else
            {
                txtExchangeRateToBase.Enabled = true;
                txtExchangeRateToBase.BackColor = Color.Empty;
            }

            LookupCurrencyExchangeRates(DateTime.Now);
        }

        private void LookupCurrencyExchangeRates(DateTime ADate)
        {
            FExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                FCurrencyCode,
                FMainDS.ALedger[0].BaseCurrency,
                ADate, true);

            txtExchangeRateToBase.Text = FExchangeRateToBase.ToString();

            FExchangeRateIntlToBase = InternationalCurrencyExchangeRate(ADate);
        }

        private decimal InternationalCurrencyExchangeRate(DateTime ABatchEffectiveDate)
        {
            decimal IntlToBaseCurrencyExchRate = 0;

            DateTime StartOfMonth = new DateTime(ABatchEffectiveDate.Year, ABatchEffectiveDate.Month, 1);
            string LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            string LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            if (LedgerBaseCurrency == LedgerIntlCurrency)
            {
                IntlToBaseCurrencyExchRate = 1;
            }
            else
            {
                IntlToBaseCurrencyExchRate = TRemote.MFinance.GL.WebConnectors.GetCorporateExchangeRate(LedgerBaseCurrency,
                    LedgerIntlCurrency,
                    StartOfMonth,
                    ABatchEffectiveDate);
            }

            return IntlToBaseCurrencyExchRate;
        }

        private void CheckBatchEffectiveDate(object sender, EventArgs e)
        {
            DateTime dateValue;
            string aDate = dtpEffectiveDate.Text;

            if (DateTime.TryParse(aDate, out dateValue))
            {
                LookupCurrencyExchangeRates(dateValue);
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

            decimal selectedExchangeRate;
            DateTime selectedEffectiveDate;
            int selectedEffectiveTime;

            if (setupDailyExchangeRate.ShowDialog(
                    FLedgerNumber,
                    dtpEffectiveDate.Date.Value,
                    txtCurrencyCodeFrom.Text,
                    DEFAULT_CURRENCY_EXCHANGE,
                    out selectedExchangeRate,
                    out selectedEffectiveDate,
                    out selectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            FExchangeRateToBase = selectedExchangeRate;
            txtExchangeRateToBase.Text = FExchangeRateToBase.ToString();
        }

        /// <summary>
        /// this submits the given Batch number
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void SubmitBatch(object sender, EventArgs e)
        {
            decimal ExchRateToBase = 0;

            if (!(Decimal.TryParse(txtExchangeRateToBase.Text, out ExchRateToBase) && (ExchRateToBase > 0)))
            {
                MessageBox.Show(Catalog.GetString("The exchange rate must be a number greater than 0."));
                txtExchangeRateToBase.Focus();
                txtExchangeRateToBase.SelectAll();
                return;
            }

            //check the GL batch date
            if ((dtpEffectiveDate.Date < FStartDateCurrentPeriod)
                || (dtpEffectiveDate.Date > FEndDateLastForwardingPeriod))
            {
                MessageBox.Show(Catalog.GetString("The batch date is outside the allowed posting period start/end date range: " +
                        FStartDateCurrentPeriod.ToShortDateString() + " to " +
                        FEndDateLastForwardingPeriod.ToShortDateString()));
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
                return;
            }

            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("ABatchNumber", FBatchNumber);
            requestParams.Add("AEffectiveDate", dtpEffectiveDate.Date.Value);
            requestParams.Add("AExchangeRateToBase", FExchangeRateToBase);
            requestParams.Add("AExchangeRateIntlToBase", FExchangeRateIntlToBase);

            TRemote.MFinance.GL.WebConnectors.SubmitRecurringGLBatch(requestParams);

            MessageBox.Show(Catalog.GetString("Your recurring batch was submitted successfully!"),
                Catalog.GetString("Success"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
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