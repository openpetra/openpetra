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
using System.Collections.Generic;
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

using SourceGrid;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Batch export
    /// </summary>
    public partial class TFrmRecurringGLBatchSubmit
    {
        private GLBatchTDS FSubmitMainDS;
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;
        private ARecurringBatchRow FBatchRow;
        private ALedgerRow FLedgerRow;
        private string FBaseCurrencyCode;
        private Decimal FExchangeRateIntlToBase = 0;
        private Dictionary <string, decimal>FExchangeRateDictionary = new Dictionary <string, decimal>();

        DateTime FStartDateCurrentPeriod;
        DateTime FEndDateLastForwardingPeriod;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// <summary>
        /// Dataset containing data to submit
        /// </summary>
        public GLBatchTDS SubmitMainDS
        {
            set
            {
                FSubmitMainDS = value;
                FMainDS.Merge(FSubmitMainDS);

                FLedgerRow = (ALedgerRow)FSubmitMainDS.ALedger[0];
                FLedgerNumber = FLedgerRow.LedgerNumber;
                FBatchRow = (ARecurringBatchRow)FSubmitMainDS.ARecurringBatch[0];
                FBatchNumber = FBatchRow.BatchNumber;

                FBaseCurrencyCode = FLedgerRow.BaseCurrency;
                txtLedgerNumber.Text = FLedgerNumber.ToString();
                txtCurrencyCodeTo.Text = FBaseCurrencyCode;

                this.Text = String.Format(Catalog.GetString("Submit recurring Batch {0}"), FBatchRow.BatchNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();

                DateTime DefaultDate;
                TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                    out FStartDateCurrentPeriod,
                    out FEndDateLastForwardingPeriod,
                    out DefaultDate);

                lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                    FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());

                LoadJournals();
            }
        }

        private void LoadJournals()
        {
            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(FLedgerNumber, FBatchNumber));

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            //Populate the dictionary with all journal currencies
            if (JournalDV.Count > 0)
            {
                string currencyCode = string.Empty;
                decimal exchangeRate = 0;

                foreach (DataRowView drv in JournalDV)
                {
                    ARecurringJournalRow jr = (ARecurringJournalRow)drv.Row;

                    currencyCode = jr.TransactionCurrency;
                    exchangeRate = (currencyCode == FBaseCurrencyCode) ? 1 : 0;

                    if (!FExchangeRateDictionary.ContainsKey(currencyCode))
                    {
                        FExchangeRateDictionary.Add(currencyCode, exchangeRate);
                    }

                    jr.ExchangeRateToBase = exchangeRate;
                }
            }

            SelectRowInGrid(1);
        }

        private void GetDetailsFromControlsManual(ARecurringJournalRow ARow)
        {
            if (ARow == null)
            {
                return;
            }
        }

        private void InitializeManualCode()
        {
            lblValidDateRange.Width = 300;
            txtDetailTransactionCurrency.ReadOnly = true;
            txtDetailExchangeRateToBase.ReadOnly = true;
            dtpEffectiveDate.Date = DateTime.Now;
        }

        private void ShowDetailsManual(ARecurringJournalRow ARow)
        {
            bool JournalRowIsNull = (ARow == null);

            grdDetails.TabStop = (!JournalRowIsNull);

            if (JournalRowIsNull)
            {
                return;
            }

            btnGetSetExchangeRate.Enabled = (ARow.TransactionCurrency != FBaseCurrencyCode);
        }

        private void LookupCurrencyExchangeRates(DateTime ADate, String ACurrencyCode)
        {
            decimal ExchangeRateToBase = 1;

            if (ACurrencyCode != FBaseCurrencyCode)
            {
                ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    ACurrencyCode,
                    FBaseCurrencyCode,
                    ADate, true);
            }

            if ((txtDetailExchangeRateToBase.NumberValueDecimal.Value != ExchangeRateToBase)
                || (FPreviouslySelectedDetailRow.ExchangeRateToBase != ExchangeRateToBase))
            {
                txtDetailExchangeRateToBase.NumberValueDecimal = ExchangeRateToBase;
                FPreviouslySelectedDetailRow.ExchangeRateToBase = ExchangeRateToBase;
            }

            FExchangeRateIntlToBase = InternationalCurrencyExchangeRate(ADate);
        }

        private decimal InternationalCurrencyExchangeRate(DateTime ABatchEffectiveDate)
        {
            decimal IntlToBaseCurrencyExchRate = 0;

            DateTime StartOfMonth = new DateTime(ABatchEffectiveDate.Year, ABatchEffectiveDate.Month, 1);
            string LedgerIntlCurrency = FLedgerRow.IntlCurrency;

            if (FBaseCurrencyCode == LedgerIntlCurrency)
            {
                IntlToBaseCurrencyExchRate = 1;
            }
            else
            {
                IntlToBaseCurrencyExchRate = TRemote.MFinance.GL.WebConnectors.GetCorporateExchangeRate(FBaseCurrencyCode,
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
                if (FPreviouslySelectedDetailRow != null)
                {
                    LookupCurrencyExchangeRates(dateValue, FPreviouslySelectedDetailRow.TransactionCurrency);
                }
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
            TFrmSetupDailyExchangeRate SetupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            decimal SelectedExchangeRate;
            DateTime SelectedEffectiveDate;
            int SelectedEffectiveTime;

            string CurrencyCode = txtDetailTransactionCurrency.Text;

            if (SetupDailyExchangeRate.ShowDialog(
                    FLedgerNumber,
                    dtpEffectiveDate.Date.Value,
                    CurrencyCode,
                    DEFAULT_CURRENCY_EXCHANGE,
                    out SelectedExchangeRate,
                    out SelectedEffectiveDate,
                    out SelectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            if (SelectedExchangeRate > 0.0m)
            {
                FPreviouslySelectedDetailRow.ExchangeRateToBase = SelectedExchangeRate;
                txtDetailExchangeRateToBase.NumberValueDecimal = SelectedExchangeRate;
                SetRateForSameCurrency(CurrencyCode, SelectedExchangeRate, FPreviouslySelectedDetailRow.JournalNumber);
            }
        }

        private void SetRateForSameCurrency(String ACurrencyCode, Decimal AExchangeRate, Int32 ARecurringJournalNumber)
        {
            DataView JournalCurrencyDV = new DataView(FMainDS.ARecurringJournal);

            JournalCurrencyDV.RowFilter = string.Format("{0}={1} And {2}<>{3} And {4}='{5}'",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringJournalTable.GetJournalNumberDBName(),
                ARecurringJournalNumber,
                ARecurringJournalTable.GetTransactionCurrencyDBName(),
                ACurrencyCode);

            if (JournalCurrencyDV.Count > 0)
            {
                foreach (DataRowView drv in JournalCurrencyDV)
                {
                    ARecurringJournalRow jr = (ARecurringJournalRow)drv.Row;

                    jr.ExchangeRateToBase = AExchangeRate;
                }
            }

            SaveChanges();
        }

        /// <summary>
        /// this submits the given Batch number
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void SubmitBatch(object sender, EventArgs e)
        {
            String SubmitErrorMessage = string.Empty;
            String ErrorMessageTitle = "Submit Recurring Batch " + FBatchNumber.ToString();

            if ((dtpEffectiveDate.Date < FStartDateCurrentPeriod)
                || (dtpEffectiveDate.Date > FEndDateLastForwardingPeriod))
            {
                SubmitErrorMessage =
                    String.Format(Catalog.GetString("The batch date is outside the allowed posting period start/end date range: {0} to {1}"),
                        FStartDateCurrentPeriod.ToShortDateString(),
                        FEndDateLastForwardingPeriod.ToShortDateString());

                MessageBox.Show(SubmitErrorMessage, ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpEffectiveDate.Focus();
                dtpEffectiveDate.SelectAll();
                return;
            }

            // Check for any non-set exchange rates:
            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            if (JournalDV.Count > 0)
            {
                string currencyCode = string.Empty;
                bool isForeignCurrency = false;

                foreach (DataRowView drv in JournalDV)
                {
                    ARecurringJournalRow jr = (ARecurringJournalRow)drv.Row;

                    currencyCode = jr.TransactionCurrency;
                    isForeignCurrency = (currencyCode != FBaseCurrencyCode);

                    if (isForeignCurrency && (jr.ExchangeRateToBase <= 0))
                    {
                        if (SubmitErrorMessage.Length == 0)
                        {
                            SubmitErrorMessage = Catalog.GetString("The following foreign currency Journals need a valid exchange rate:") +
                                                 Environment.NewLine;
                        }

                        SubmitErrorMessage += string.Format("{0}Journal no.: {1:00} requires a rate for {2} to {3}",
                            Environment.NewLine,
                            jr.JournalNumber,
                            jr.TransactionCurrency,
                            FBaseCurrencyCode);
                    }
                    else if (isForeignCurrency)
                    {
                        FExchangeRateDictionary[currencyCode] = jr.ExchangeRateToBase;
                    }
                }

                if (SubmitErrorMessage.Length > 0)
                {
                    MessageBox.Show(SubmitErrorMessage, ErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("ABatchNumber", FBatchNumber);
            requestParams.Add("AEffectiveDate", dtpEffectiveDate.Date.Value);
            requestParams.Add("AExchangeRateIntlToBase", FExchangeRateIntlToBase);

            TVerificationResultCollection Verifications = new TVerificationResultCollection();
            Int32 NewGLBatchNumber = TRemote.MFinance.GL.WebConnectors.SubmitRecurringGLBatch(ref FMainDS,
                requestParams,
                ref FExchangeRateDictionary,
                ref Verifications);

            if (NewGLBatchNumber > 0)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Your Recurring GL Batch was submitted successfully as new GL Batch: {0}!"),
                        NewGLBatchNumber),
                    Catalog.GetString("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                string errorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    errorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                MessageBox.Show(errorMessages, Catalog.GetString("Recurring GL Batch Submit failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

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

        private void ValidateDataDetailsManual(ARecurringJournalRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            // Description is mandatory then make sure it is set
            if (ARow.ExchangeRateToBase <= 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringJournalTable.ColumnExchangeRateToBaseId];
                ValidationContext = String.Format("Recurring Batch no.: {0}, Journal no.: {1}",
                    ARow.BatchNumber,
                    ARow.JournalNumber);

                VerificationResult = TNumericalChecks.IsNegativeOrZeroDecimal(ARow.ExchangeRateToBase,
                    "Exchange rate of " + ValidationContext,
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }
    }
}