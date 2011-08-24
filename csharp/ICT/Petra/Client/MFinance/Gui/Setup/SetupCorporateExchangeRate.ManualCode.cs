//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCorporateExchangeRate
    {
        /// <summary>
        /// CultureRecord for the exchange rate ...
        /// </summary>
        private NumberFormatInfo numberFormatInfo = null;
        private NumberFormatInfo currencyFormatInfo = null;

        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private String strModalFormReturnValue;

        private String strCurrencyToDefault;
        private DateTime dateTimeDefault;
        private bool blnUseDateTimeDefault = false;

        private bool blnSelectedRowChangeable = false;

        private bool blnIsInModalMode;

        /// <summary>
        /// The definition of the ledger number is used to define some
        /// default values and it initializes the dialog to run in the non modal
        /// form ...
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, value))[0];

                baseCurrencyOfLedger = ledger.BaseCurrency;


                try
                {
                    numberFormatInfo =
                        new System.Globalization.CultureInfo(
                            ledger.CountryCode, false).NumberFormat;
                    currencyFormatInfo =
                        new System.Globalization.CultureInfo(
                            ledger.CountryCode, false).NumberFormat;
                }
                catch (System.NotSupportedException)
                {
                    // Do not use local formats here!
                    // This is the default
                    numberFormatInfo =
                        new System.Globalization.CultureInfo(
                            String.Empty, false).NumberFormat;
                    currencyFormatInfo =
                        new System.Globalization.CultureInfo(
                            String.Empty, false).NumberFormat;
                }
                numberFormatInfo.NumberDecimalDigits =
                    currencyFormatInfo.NumberDecimalDigits + 4;


                this.txtDetailRateOfExchange.Validating +=
                    new System.ComponentModel.CancelEventHandler(this.ValidatingExchangeRate);

                this.txtDetailRateOfExchange.Validated +=
                    new System.EventHandler(this.ValidatedExchangeRate);

                this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                    new System.EventHandler(this.ValueChangedCurrencyCode);
                this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                    new System.EventHandler(this.ValueChangedCurrencyCode);

                this.tbbSave.Click +=
                    new System.EventHandler(this.SetTheFocusToTheGrid);

                this.btnInvertExchangeRate.Click +=
                    new System.EventHandler(this.InvertExchangeRate);

                this.btnUseDateToFilter.Click +=
                    new System.EventHandler(this.UseDateToFilter);

                FMainDS.ACorporateExchangeRate.DefaultView.Sort =
                    "a_date_effective_from_d desc, a_time_effective_from_i desc";
                FMainDS.ACorporateExchangeRate.DefaultView.RowFilter = "";

                btnUseDateToFilter.Visible = true;
                mniImport.Enabled = true;
                tbbImport.Enabled = true;
                blnIsInModalMode = true;
                strModalFormReturnValue = "";
            }
        }


        /// <summary>
        /// In oder to run the dialog in the modal form you have to invoke this routine.
        /// The table will be filtered by the value of the base currency of the selected ledger,
        /// and the values of the two function parameters.
        /// </summary>
        /// <param name="dteEffective">Effective date of the actual acounting process</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <param name="strExchangeDefault">Defaut value for the exchange rate</param>
        public void SetDataFilters(DateTime dteEffective,
            string strCurrencyTo,
            string strExchangeDefault)
        {
            DateTime dateLimit = dteEffective.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string dateString = dateLimit.ToString("d", dateTimeFormat);

            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter =
                "a_from_currency_code_c = '" + baseCurrencyOfLedger + "' and " +
                "a_to_currency_code_c = '" + strCurrencyTo + "' and " +
                "a_date_effective_from_d < '" + dateString + "'";

            strModalFormReturnValue = strExchangeDefault;
            strCurrencyToDefault = strCurrencyTo;
            dateTimeDefault = dteEffective;
            DefineModalSettings();
        }

        public void SetDataFilters(DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo,
            decimal decExchangeDefault)
        {
            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter =
                "a_from_currency_code_c = '" + baseCurrencyOfLedger + "' and " +
                "a_to_currency_code_c = '" + strCurrencyTo + "' and " +
                "a_date_effective_from_d < '" + strDteEnd + "' and " +
                "a_date_effective_from_d > '" + strDteStart + "'";
            strModalFormReturnValue = decExchangeDefault.ToString("0.00000000");
            dateTimeDefault = dteEnd;
            strCurrencyToDefault = strCurrencyTo;
            DefineModalSettings();
        }

        public decimal GetLastExchangeValueOfIntervall(DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo)
        {
            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter =
                "a_from_currency_code_c = '" + baseCurrencyOfLedger + "' and " +
                "a_to_currency_code_c = '" + strCurrencyTo + "' and " +
                "a_date_effective_from_d < '" + strDteEnd + "' and " +
                "a_date_effective_from_d > '" + strDteStart + "'";

            if (grdDetails.Rows.Count != 0)
            {
                try
                {
                    // Code tut nicht!
                    SelectDetailRowByDataTableIndex(0);
                    ACorporateExchangeRateRow corporateExchangeRateRow =
                        (ACorporateExchangeRateRow)(FMainDS.ACorporateExchangeRate.DefaultView[0].Row);
                    return corporateExchangeRateRow.RateOfExchange;
                }
                catch (Exception ex)
                {
                    return 1.0m;
                }
            }
            else
            {
                return 1.0m;
            }
        }

        private void DefineModalSettings()
        {
            blnUseDateTimeDefault = true;
            btnUseDateToFilter.Visible = false;
            mniImport.Enabled = false;
            tbbImport.Enabled = false;

            blnIsInModalMode = true;
        }

        /// <summary>
        /// If the dialog has been used in modal form, this property shall be used to
        /// read the "answer".
        /// </summary>
        public String CurrencyExchangeRate
        {
            get
            {
                return strModalFormReturnValue;
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
                    strModalFormReturnValue = txtDetailRateOfExchange.Text;
                    blnUseDateTimeDefault = false;
                    SaveChanges();
                    Close();
                }
            }
            else
            {
                blnUseDateTimeDefault = false;
                Close();
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
        /// Create a new CorporateExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            DateTime dateTimeNow;

            if (!blnUseDateTimeDefault)
            {
                dateTimeNow = DateTime.Now;
            }
            else
            {
                dateTimeNow = dateTimeDefault;
            }

            DateTime dateDate = DateTime.Parse(dateTimeNow.ToLongDateString());
            dateTimeNow = DateTime.Now;
            DateTime dateTime = DateTime.Parse(dateTimeNow.ToLongTimeString());

            ACorporateExchangeRateRow ACorporateExRateRow = FMainDS.ACorporateExchangeRate.NewRowTyped();

            ACorporateExRateRow.FromCurrencyCode = baseCurrencyOfLedger;

            if (strCurrencyToDefault == null)
            {
                if (FPreviouslySelectedDetailRow == null)
                {
                    ACorporateExRateRow.ToCurrencyCode = baseCurrencyOfLedger;
                    ACorporateExRateRow.RateOfExchange = 1.0m;
                }
                else
                {
                    ACorporateExRateRow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
                    ACorporateExRateRow.RateOfExchange = Decimal.Parse(txtDetailRateOfExchange.Text);
                }
            }
            else
            {
                ACorporateExRateRow.ToCurrencyCode = strCurrencyToDefault;
                ACorporateExRateRow.RateOfExchange = 1.0m;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                cmbDetailFromCurrencyCode.SetSelectedString(ACorporateExRateRow.FromCurrencyCode);
                cmbDetailToCurrencyCode.SetSelectedString(ACorporateExRateRow.ToCurrencyCode);
            }

            ACorporateExRateRow.DateEffectiveFrom = dateDate;
            ACorporateExRateRow.TimeEffectiveFrom =
                (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;

            FMainDS.ACorporateExchangeRate.Rows.Add(ACorporateExRateRow);
            grdDetails.Refresh();

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ACorporateExchangeRate.Rows.Count - 1);
        }

        /// <summary>
        /// Validating Event for txtDetailRateOfExchange
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValidatingExchangeRate(System.Object sender, CancelEventArgs e)
        {
            decimal exchangeRate;

            try
            {
                exchangeRate = Decimal.Parse(txtDetailRateOfExchange.Text);

                txtDetailRateOfExchange.Text = exchangeRate.ToString("N", numberFormatInfo);
                txtDetailRateOfExchange.BackColor = Color.Empty;
            }
            catch (Exception)
            {
                txtDetailRateOfExchange.BackColor = Color.Red;
                e.Cancel = true;
            }
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
            String strLblText = Catalog.GetString("For {0} {1} you will get {2} {3}");

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            decimal exchangeRate;
            exchangeRate = Decimal.Parse(txtDetailRateOfExchange.Text);

            if (FPreviouslySelectedDetailRow == null)
            {
                lblValueOneDirection.Text = "-";
            }
            else
            {
                lblValueOneDirection.Text =
                    String.Format(numberFormatInfo, strLblText,
                        1.0m.ToString("N", currencyFormatInfo),
                        FPreviouslySelectedDetailRow.FromCurrencyCode.ToString(),
                        exchangeRate.ToString("N", numberFormatInfo),
                        FPreviouslySelectedDetailRow.ToCurrencyCode.ToString());
            }

            try
            {
                exchangeRate = 1 / exchangeRate;
            }
            catch (Exception)
            {
                exchangeRate = 0;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                lblValueOtherDirection.Text = "-";
            }
            else
            {
                lblValueOtherDirection.Text =
                    String.Format(numberFormatInfo, strLblText,
                        1.0m.ToString("N", currencyFormatInfo),
                        FPreviouslySelectedDetailRow.ToCurrencyCode.ToString(),
                        exchangeRate.ToString("N", numberFormatInfo),
                        FPreviouslySelectedDetailRow.FromCurrencyCode.ToString());
            }

            if (blnIsInModalMode)
            {
                dtpDetailDateEffectiveFrom.Enabled = false;
            }
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
                txtDetailRateOfExchange.Text = "1.0";
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
                    cmbDetailToCurrencyCode.Enabled =
                        (cmbDetailFromCurrencyCode.GetSelectedString() == baseCurrencyOfLedger);
                }

                cmbDetailFromCurrencyCode.Enabled =
                    (cmbDetailToCurrencyCode.GetSelectedString() == baseCurrencyOfLedger);
            }
        }

        /// <summary>
        /// This routines supports a small gui-calculator. The user can easily calculate the
        /// reciproke value of the exchange rate.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void InvertExchangeRate(System.Object sender, EventArgs e)
        {
            decimal exchangeRate;

            try
            {
                exchangeRate = decimal.Parse(txtDetailRateOfExchange.Text);
                exchangeRate = 1 / exchangeRate;
                exchangeRate = Math.Round(exchangeRate, numberFormatInfo.NumberDecimalDigits);
                txtDetailRateOfExchange.Text = exchangeRate.ToString("N", numberFormatInfo);
            }
            catch (Exception)
            {
            }
            ;
            ValidatedExchangeRate();
        }

        /// <summary>
        /// A "date filter" is placed inside the table. The content of
        /// dtpDetailDateEffectiveFrom is used for the filter.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void UseDateToFilter(System.Object sender, EventArgs e)
        {
            if (FMainDS.ACorporateExchangeRate.DefaultView.RowFilter.Equals(""))
            {
                DateTime dateLimit = dtpDetailDateEffectiveFrom.Date.Value.AddDays(1.0);
                // Do not use local formats here!
                DateTimeFormatInfo dateTimeFormat =
                    new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
                string dateString = dateLimit.ToString("d", dateTimeFormat);
                FMainDS.ACorporateExchangeRate.DefaultView.RowFilter =
                    "a_date_effective_from_d < '" + dateString + "'";
                String strBtnUseDateToFilter2 = Catalog.GetString("Unuse Filter");
                btnUseDateToFilter.Text = strBtnUseDateToFilter2;
            }
            else
            {
                FMainDS.ACorporateExchangeRate.DefaultView.RowFilter = "";
                String strBtnUseDateToFilter1 = Catalog.GetString("Use Date To Filter");
                btnUseDateToFilter.Text = strBtnUseDateToFilter1;
            }

            cmbDetailToCurrencyCode.Enabled = false;
            txtDetailRateOfExchange.Enabled = false;
            dtpDetailDateEffectiveFrom.Enabled = false;
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ACorporateExchangeRateRow ARow)
        {
            if (ARow != null)
            {
                blnSelectedRowChangeable = !(ARow.RowState == DataRowState.Unchanged);
                ValidatedExchangeRate();
                txtDetailRateOfExchange.Enabled = (ARow.RowState == DataRowState.Added);
                btnInvertExchangeRate.Enabled = (ARow.RowState == DataRowState.Added);
                blnSelectedRowChangeable = (ARow.RowState == DataRowState.Added);
                ValueChangedCurrencyCode();
            }
            else
            {
                blnSelectedRowChangeable = false;
                txtDetailRateOfExchange.Enabled = false;
                txtDetailRateOfExchange.Text = "";
            }
        }

        /// <summary>
        /// Routine to delete a row (only a row created in the same session can be deleted.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void DeleteRow(System.Object sender, EventArgs e)
        {
            ACorporateExchangeRateRow actualRow = GetSelectedDetailRow();

            SelectByIndex(-1);
            FMainDS.ACorporateExchangeRate.Rows.Remove(actualRow);
            FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// SelectByIndex is a copy&paste routine which is always sligtly modified
        /// and adapted to the project ..
        /// </summary>
        /// <param name="rowIndex">-1 means "noRow" and 1 is the first row</param>
        public void SelectByIndex(int rowIndex)
        {
            if (rowIndex != -1)
            {
                if (rowIndex >= grdDetails.Rows.Count)
                {
                    rowIndex = grdDetails.Rows.Count - 1;
                }

                if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
                {
                    rowIndex = 1;
                }
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                grdDetails.Selection.ResetSelection(false);
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void GetDetailDataFromControlsManual(ACorporateExchangeRateRow ARow)
        {
            TExchangeRateCache.ResetCache();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, "Corporate");
            FPetraUtilsObject.SetChangedFlag();
        }

    }
    
}