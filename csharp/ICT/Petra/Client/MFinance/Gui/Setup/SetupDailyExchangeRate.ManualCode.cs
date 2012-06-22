//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.IO;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core.RemoteObjects;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupDailyExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private Decimal ModalFormReturnValue;

        private String strCurrencyToDefault;
        private DateTime dateTimeDefault;
        private bool blnUseDateTimeDefault = false;

        private bool blnIsInModalMode;

        private string SortByDateDescending =
            ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetToCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
            ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";

        /// <summary>
        /// Public property that is not really used in release builds, but might be useful
        /// when developing to set the initial value for the ledger number
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, value))[0];
                baseCurrencyOfLedger = ledger.BaseCurrency;
            }
        }

        private void InitializeManualCode()
        {
            // This code runs just before the auto-generated code binds the data to the grid
            // We need to set the RowFilter to something that returns no rows because we will return the rows we actually want
            // in RunOnceOnActivation.  By returning no rows now we reduce some horrible flicker on the screen (and save time!)
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = FMainDS.ADailyExchangeRate.ColumnDateEffectiveFrom + " = '" +
                                                               DateTime.MaxValue.ToShortDateString() + "'";

            // Now we set some default settings that apply when the screen is MODELESS
            //  (If the screen will be MODAL one of the ShowDialog methods will be called below)
            btnClose.Visible = false;           // Do not show the modal buttons
            btnCancel.Visible = false;
            mniImport.Enabled = true;           // Allow imports
            tbbImport.Enabled = true;
            blnIsInModalMode = false;
            ModalFormReturnValue = 1.0m;        // Not really used when MODELESS
        }

        private void RunOnceOnActivationManual()
        {
            // Activate events we will use in manual code
            this.txtDetailRateOfExchange.TextChanged +=
                new EventHandler(txtDetailRateOfExchange_TextChanged);
            this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);
            this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);

            // Set a non-standard sort order (newest record first)
            DataView theView = FMainDS.ADailyExchangeRate.DefaultView;
            theView.Sort = SortByDateDescending;

            // Set the RowFilter - in MODAL mode it has already been set, but in MODELESS mode we need to set up to see all rows
            if (!blnIsInModalMode)
            {
                theView.RowFilter = "";
            }

            // Having changed the sort order we need to put the correct details in the panel (assuming we have a row to display)
            if (theView.Count > 0)
            {
                grdDetails.Selection.SelectRow(1, true);
            }

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();          // can be null
            ShowDetails(FPreviouslySelectedDetailRow);
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog()
        {
            throw new NotSupportedException(
                "You cannot call ShowDialog with empty parameters.  Use one of the method signatures with multiple parameters");
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog(IWin32Window Parent)
        {
            throw new NotSupportedException(
                "You cannot call ShowDialog with a single parameter.  Use one of the method signatures with multiple parameters");
        }

        /// <summary>
        /// Main method to invoke the dialog in the modal form based on a single effective date.
        /// The table will be filtered by the value of the base currency of the selected ledger,
        /// and the values of the date and foreign currency.
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteEffective">Effective date of the actual acounting process.  The grid will show all entries on or before this date.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteEffective,
            string strCurrencyTo,
            decimal ExchangeDefault)
        {
            // We can just call our alternate method, setting the start date to the beginning of time!
            return ShowDialog(LedgerNumber, DateTime.MinValue, dteEffective, strCurrencyTo, ExchangeDefault);
        }

        /// <summary>
        /// Main method to invoke the dialog in the modal form based on a date range.
        /// The table will be filtered by the value of the base currency of the selected ledger,
        /// and the values of the date range and foreign currency.
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.  The grid will show all entries between the start and end dates.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo,
            decimal ExchangeDefault)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];

            baseCurrencyOfLedger = ledger.BaseCurrency;

            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + baseCurrencyOfLedger + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + strCurrencyTo + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < '" + strDteEnd + "'";

            if (dteStart > DateTime.MinValue)
            {
                filter += (" and " + ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " > '" + strDteStart + "'");
            }

            DataView myDataView = FMainDS.ADailyExchangeRate.DefaultView;
            myDataView.RowFilter = filter;
            myDataView.Sort = SortByDateDescending;

            ModalFormReturnValue = ExchangeDefault;
            strCurrencyToDefault = strCurrencyTo;
            dateTimeDefault = dteEnd;

            DefineModalSettings();

            return base.ShowDialog();
        }

        /// <summary>
        /// Get the most recent exchange rate value of the interval.  This method does not display a dialog
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <returns>The most recent exchange rate in the specified date range</returns>
        public decimal GetLastExchangeValueOfInterval(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];

            baseCurrencyOfLedger = ledger.BaseCurrency;

            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + baseCurrencyOfLedger + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + strCurrencyTo + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < '" + strDteEnd + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " > '" + strDteStart + "'";
            DataView myView = new DataView(FMainDS.ADailyExchangeRate, filter, SortByDateDescending, DataViewRowState.CurrentRows);

            if (myView.Count > 0)
            {
                return ((ADailyExchangeRateRow)(myView.ToTable().Rows[0])).RateOfExchange;
            }
            else
            {
                return 1.0m;
            }
        }

        private void DefineModalSettings()
        {
            blnUseDateTimeDefault = true;

            // We need the accept/cancel buttons when MODAL.  It looks better if they are at the top and New is beneath
            btnClose.Visible = true;
            btnCancel.Visible = true;
            int pos1 = btnNew.Top;
            int pos2 = btnClose.Top;
            btnClose.Top = pos1;
            btnCancel.Top = pos2;
            btnNew.Top = btnCancel.Top + 2 * btnCancel.Height;

            // Import not allowed when MODAL
            mniImport.Enabled = false;
            tbbImport.Enabled = false;

            // Different Dialog Title text - and set the buttons
            this.Text = "Select an Exchange Rate";
            this.AcceptButton = btnClose;
            this.CancelButton = btnCancel;

            blnIsInModalMode = true;
            DialogResult = DialogResult.Cancel;     // assume it is cancelled for now
        }

        /// <summary>
        /// If the dialog has been used in modal form, this property shall be used to
        /// read the "answer".
        /// </summary>
        public Decimal CurrencyExchangeRate
        {
            get
            {
                return ModalFormReturnValue;
            }
        }


        /// <summary>
        /// If the dialog is used modal it shall be closed by this routine ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CloseDialog(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.CloseFormCheck())
            {
                if (CanClose())
                {
                    if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
                    {
                        ModalFormReturnValue = txtDetailRateOfExchange.NumberValueDecimal.Value;
                    }

                    blnUseDateTimeDefault = false;
                    SaveChanges();
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        /// <summary>
        /// If the dialog is used modal then it can be canceled ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CancelDialog(object sender, EventArgs e)
        {
            blnUseDateTimeDefault = false;
            Close();
        }

        /// <summary>
        /// Create a new DailyExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            // Check the current panel data and get it into the current record
            if (!ValidateAllData(true, true))
            {
                return;
            }

            // Now create an appropriate new record
            DateTime dateTimeNow;

            if (blnUseDateTimeDefault)
            {
                dateTimeNow = dateTimeDefault;
            }
            else
            {
                //For Corpoate Exchange Rate must be 1st of the month, for Daily Exchange Rate it must be now
                dateTimeNow = DateTime.Now;
            }

            DateTime dateDate = DateTime.Parse(dateTimeNow.ToLongDateString());
            dateTimeNow = DateTime.Now;
            DateTime dateTime = DateTime.Parse(dateTimeNow.ToLongTimeString());

            ADailyExchangeRateRow ADailyExRateRow = FMainDS.ADailyExchangeRate.NewRowTyped();

            if (baseCurrencyOfLedger == null)
            {
                ADailyExRateRow.FromCurrencyCode = "USD";
            }
            else
            {
                ADailyExRateRow.FromCurrencyCode = baseCurrencyOfLedger;
            }

            if (strCurrencyToDefault == null)
            {
                if (FPreviouslySelectedDetailRow == null)
                {
                    if (baseCurrencyOfLedger == null)
                    {
                        ADailyExRateRow.ToCurrencyCode = "USD";
                    }
                    else
                    {
                        ADailyExRateRow.ToCurrencyCode = baseCurrencyOfLedger;
                    }

                    ADailyExRateRow.RateOfExchange = 1.0m;
                }
                else
                {
                    ADailyExRateRow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
                    ADailyExRateRow.RateOfExchange = txtDetailRateOfExchange.NumberValueDecimal.Value;
                }
            }
            else
            {
                ADailyExRateRow.ToCurrencyCode = strCurrencyToDefault;
                ADailyExRateRow.RateOfExchange = 1.0m;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                cmbDetailFromCurrencyCode.SetSelectedString(ADailyExRateRow.FromCurrencyCode);
                cmbDetailToCurrencyCode.SetSelectedString(ADailyExRateRow.ToCurrencyCode);
            }

            ADailyExRateRow.DateEffectiveFrom = dateDate;
            ADailyExRateRow.TimeEffectiveFrom =
                (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;

            // Ensure we don't create a duplicate record
            while (FMainDS.ADailyExchangeRate.Rows.Find(new object[] {
                           ADailyExRateRow.FromCurrencyCode, ADailyExRateRow.ToCurrencyCode,
                           ADailyExRateRow.DateEffectiveFrom.ToString(), ADailyExRateRow.TimeEffectiveFrom.ToString()
                       }) != null)
            {
                ADailyExRateRow.TimeEffectiveFrom = ADailyExRateRow.TimeEffectiveFrom + 1;
            }

            int previousGridRow = grdDetails.Selection.ActivePosition.Row;
            FMainDS.ADailyExchangeRate.Rows.Add(ADailyExRateRow);
            grdDetails.Refresh();

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ADailyExchangeRate.Rows.Count - 1);
            int currentGridRow = grdDetails.Selection.ActivePosition.Row;

            if (currentGridRow == previousGridRow)
            {
                // The grid must be sorted so the new row is displayed where the old one was.  We will not have received a RowChanged event.
                // We need to enforce showing the new details.
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels()
        {
            // Call can cope with null for Row, but rate must have a valid value
            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                    cmbDetailToCurrencyCode.GetSelectedString(), FPreviouslySelectedDetailRow,
                    txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
            }
            else
            {
                TSetupExchangeRates.SetExchangeRateLabels(String.Empty, String.Empty, null, 1.0m, lblValueOneDirection, lblValueOtherDirection);
            }
        }

        /// <summary>
        /// ValueChanged Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValueChangedCurrencyCode(System.Object sender, EventArgs e)
        {
            SetEnabledStates();
        }

        /// <summary>
        /// This routines supports a small gui-calculator. The user can easily calculate the
        /// reciprocal value of the exchange rate.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void InvertExchangeRate(System.Object sender, EventArgs e)
        {
            decimal? exchangeRate;

            try
            {
                exchangeRate = txtDetailRateOfExchange.NumberValueDecimal;
                exchangeRate = 1 / exchangeRate;
                exchangeRate = Math.Round(exchangeRate.Value, 10);
                txtDetailRateOfExchange.NumberValueDecimal = exchangeRate;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
        {
            // Sadly this may get called twice, if the currencies are different, but if they were the same we need to be sure to call it once at least
            SetEnabledStates();
        }

        private void SetEnabledStates()
        {
            // Set the Enabled state of the two combo boxes
            ADailyExchangeRateRow row = FPreviouslySelectedDetailRow;
            bool bEnable = (row != null && row.RowState == DataRowState.Added && !blnIsInModalMode);

            cmbDetailFromCurrencyCode.Enabled = bEnable;
            cmbDetailToCurrencyCode.Enabled = bEnable;

            // Set the Enabled states of txtRateOfExchange and the Invert button
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                // Both currencies the same
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                txtDetailRateOfExchange.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
            }
            else
            {
                // Currencies differ
                txtDetailRateOfExchange.Enabled = !RateHasBeenUsed();     // for now, but depends if the rate for the date has been used
                btnInvertExchangeRate.Enabled = true;
            }

            btnClose.Enabled = (row != null);

            if (row == null)
            {
                txtDetailRateOfExchange.NumberValueDecimal = null;
            }

            UpdateExchangeRateLabels();
        }

        private void txtDetailRateOfExchange_TextChanged(object sender, EventArgs e)
        {
            if (txtDetailRateOfExchange.Text.Trim() != String.Empty)
            {
                UpdateExchangeRateLabels();
            }
        }

        private void GetDetailDataFromControlsManual(ADailyExchangeRateRow ARow)
        {
            //TExchangeRateCache.ResetCache();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, "Daily");
            FPetraUtilsObject.SetChangedFlag();
        }

        private void ValidateDataDetailsManual(ADailyExchangeRateRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateDailyExchangeRates(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private bool RateHasBeenUsed()
        {
            //AJournalTable t = new AJournalTable();

            //Ict.Common.Data.TTypedDataTable TypedTable;
            //TRemote.MCommon.DataReader.GetData(AJournalTable.GetTableDBName(), null, out TypedTable);
            //t.Merge(TypedTable);
            return false;
        }
    }
}