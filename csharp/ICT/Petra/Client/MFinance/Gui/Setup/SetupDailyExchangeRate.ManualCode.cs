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
using Ict.Common.IO;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;


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

        private bool blnSelectedRowChangeable = false;

        private bool blnIsInModalMode;

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
            // in RunOnceOnActivation.  By returning no rows now we reduce some horrible flicker on the screen
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = FMainDS.ADailyExchangeRate.ColumnDateModified + " = '" + DateTime.MaxValue.ToShortDateString() + "'";

            // Now we set some default settings that apply when the screen is MODELESS
            //  (If the screen will be MODAL one of the SetDataFilters methods will be called below)
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
            this.txtDetailRateOfExchange.Validated +=
                new System.EventHandler(this.ValidatedExchangeRate);
            this.txtDetailRateOfExchange.TextChanged += new EventHandler(txtDetailRateOfExchange_TextChanged);

            this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);
            this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);

            this.tbbSave.Click +=
                new System.EventHandler(this.SetTheFocusToTheGrid);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);

            // Set a non-standard sort order (newest record first)
            FMainDS.ADailyExchangeRate.DefaultView.Sort = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
                                                          ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = "";

            // Having changed the sort order we need to put the correct details in the panel (assuming we have a row to display)
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog()
        {
            throw new NotSupportedException("You cannot call ShowDialog with empty parameters.  Use one of the method signatures with multiple parameters");
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog(IWin32Window Parent)
        {
            throw new NotSupportedException("You cannot call ShowDialog with a single parameter.  Use one of the method signatures with multiple parameters");
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
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];
            baseCurrencyOfLedger = ledger.BaseCurrency;

            DateTime dateLimit = dteEffective.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string dateString = dateLimit.ToString("d", dateTimeFormat);

            // Set up the filter and re-bind to the grid
            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + baseCurrencyOfLedger + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + strCurrencyTo + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < '" + dateString + "'";
            string sort = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
                ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";
            DataView myDataView = new DataView(FMainDS.ADailyExchangeRate, filter, sort, DataViewRowState.CurrentRows);
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            ModalFormReturnValue = ExchangeDefault;
            strCurrencyToDefault = strCurrencyTo;
            dateTimeDefault = dteEffective;

            DefineModalSettings();

            return base.ShowDialog();
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
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < '" + strDteEnd + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " > '" + strDteStart + "'";
            string sort = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
                ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";
            DataView myDataView = new DataView(FMainDS.ADailyExchangeRate, filter, sort, DataViewRowState.CurrentRows);
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

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
            string sort = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
                ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";
            DataView myView = new DataView(FMainDS.ADailyExchangeRate, filter, sort, DataViewRowState.CurrentRows);

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

            // Different Dialog Title text
            this.Text = "Select an Exchange Rate";

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

/*
 *          else
 *          {
 *              blnUseDateTimeDefault = false;
 *              Close();
 *          }
 */
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
        /// The focus is send to the grid to "unfocus" the input controls and to
        /// enforce that the dataset verification routines are invoked
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void SetTheFocusToTheGrid(object sender, EventArgs e)
        {
            grdDetails.Focus();
        }

        /// <summary>
        /// Create a new DailyExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            // Check the current panel data and get it into the current record
            if (!ValidateAllData(true, true)) return;

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

            FMainDS.ADailyExchangeRate.Rows.Add(ADailyExRateRow);
            grdDetails.Refresh();

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ADailyExchangeRate.Rows.Count - 1);

            UpdateExchangeRateLabels();
        }

        /// <summary>
        /// Validated Event for txtDetailRateOfExchange
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValidatedExchangeRate(System.Object sender, EventArgs e)
        {
            ValidatedExchangeRate();
        }

        /// <summary>
        /// Main routine for txtDetailRateOfExchange
        /// </summary>
        private void ValidatedExchangeRate()
        {
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            UpdateExchangeRateLabels();

            if (blnIsInModalMode)
            {
                dtpDetailDateEffectiveFrom.Enabled = false;
            }
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels()
        {
            TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                cmbDetailToCurrencyCode.GetSelectedString(), FPreviouslySelectedDetailRow,
                txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
        }

        /// <summary>
        /// ValueChanged Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValueChangedCurrencyCode(System.Object sender, EventArgs e)
        {
            ValueChangedCurrencyCode();
        }

        /// <summary>
        /// Main routine for the ValueChanged Event of the currency boxes
        /// </summary>
        private void ValueChangedCurrencyCode()
        {
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                ValidatedExchangeRate();
                txtDetailRateOfExchange.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
            }
            else
            {
                if (blnSelectedRowChangeable)
                {
                    txtDetailRateOfExchange.Enabled = true;
                    btnInvertExchangeRate.Enabled = true;
                }
            }

            if (blnSelectedRowChangeable)
            {
                if (blnIsInModalMode)
                {
                    cmbDetailToCurrencyCode.Enabled = false;
                }
                else
                {
                    cmbDetailToCurrencyCode.Enabled = true;
                }

                cmbDetailFromCurrencyCode.Enabled = true;
            }

            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                UpdateExchangeRateLabels();
            }
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

            ValidatedExchangeRate();
        }

        ///// <summary>
        ///// A "date filter" is placed inside the table. The content of
        ///// dtpDetailDateEffectiveFrom is used for the filter.
        ///// </summary>
        ///// <param name="sender">not used</param>
        ///// <param name="e">not used</param>
        //private void UseDateToFilter(System.Object sender, EventArgs e)
        //{
        //    if (FMainDS.ADailyExchangeRate.DefaultView.RowFilter.Equals(""))
        //    {
        //        DateTime dateLimit = dtpDetailDateEffectiveFrom.Date.Value.AddDays(1.0);
        //        // Do not use local formats here!
        //        DateTimeFormatInfo dateTimeFormat = new
        //                                            System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        //        string dateString = dateLimit.ToString("d", dateTimeFormat);
        //        FMainDS.ADailyExchangeRate.DefaultView.RowFilter =
        //            ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < '" + dateString + "'";
        //    }
        //    else
        //    {
        //        FMainDS.ADailyExchangeRate.DefaultView.RowFilter = "";
        //    }

        //    cmbDetailToCurrencyCode.Enabled = false;
        //    dtpDetailDateEffectiveFrom.Enabled = false;
        //}

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
        {
            if (ARow != null)
            {
                blnSelectedRowChangeable = !(ARow.RowState == DataRowState.Unchanged);
                ValidatedExchangeRate();
                txtDetailRateOfExchange.Enabled = true;
                btnInvertExchangeRate.Enabled = (ARow.RowState == DataRowState.Added);
                blnSelectedRowChangeable = (ARow.RowState == DataRowState.Added);
                ValueChangedCurrencyCode();
            }
            else
            {
                blnSelectedRowChangeable = false;
                txtDetailRateOfExchange.Enabled = false;
                txtDetailRateOfExchange.NumberValueDecimal = null;
            }
        }

        private void txtDetailRateOfExchange_TextChanged(object sender, EventArgs e)
        {
            if (txtDetailRateOfExchange.Text.Trim() != String.Empty) UpdateExchangeRateLabels();
        }

        private void GetDetailDataFromControlsManual(ADailyExchangeRateRow ARow)
        {
            TExchangeRateCache.ResetCache();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, "Daily");
            FPetraUtilsObject.SetChangedFlag();
        }
    }
}