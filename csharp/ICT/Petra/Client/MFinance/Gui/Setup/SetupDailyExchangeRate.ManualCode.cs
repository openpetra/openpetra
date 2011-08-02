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
    public partial class TFrmSetupDailyExchangeRate
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

                FMainDS.ADailyExchangeRate.DefaultView.Sort =
                    "a_date_effective_from_d desc, a_time_effective_from_i desc";
                FMainDS.ADailyExchangeRate.DefaultView.RowFilter = "";

                btnClose.Visible = false;
                btnCancel.Visible = false;
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

            FMainDS.ADailyExchangeRate.DefaultView.RowFilter =
                "a_from_currency_code_c = '" + baseCurrencyOfLedger + "' and " +
                "a_to_currency_code_c = '" + strCurrencyTo + "' and " +
                "a_date_effective_from_d < '" + dateString + "'";

            strModalFormReturnValue = strExchangeDefault;
            strCurrencyToDefault = strCurrencyTo;
            dateTimeDefault = dteEffective;
            DefineModalSettings();
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="dteStart"></param>
        /// <param name="dteEnd"></param>
        /// <param name="strCurrencyTo"></param>
        /// <param name="decExchangeDefault"></param>
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

            FMainDS.ADailyExchangeRate.DefaultView.RowFilter =
                "a_from_currency_code_c = '" + baseCurrencyOfLedger + "' and " +
                "a_to_currency_code_c = '" + strCurrencyTo + "' and " +
                "a_date_effective_from_d < '" + strDteEnd + "' and " +
                "a_date_effective_from_d > '" + strDteStart + "'";
            strModalFormReturnValue = decExchangeDefault.ToString("0.00000000");
            dateTimeDefault = dteEnd;
            strCurrencyToDefault = strCurrencyTo;
            DefineModalSettings();
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="dteStart"></param>
        /// <param name="dteEnd"></param>
        /// <param name="strCurrencyTo"></param>
        /// <returns></returns>
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

            FMainDS.ADailyExchangeRate.DefaultView.RowFilter =
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
                    ADailyExchangeRateRow dailyExchangeRateRow =
                        (ADailyExchangeRateRow)(FMainDS.ADailyExchangeRate.DefaultView[0].Row);
                    return dailyExchangeRateRow.RateOfExchange;
                }
                catch (Exception)
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
            btnClose.Visible = true;
            btnCancel.Visible = true;
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
        /// Create a new DailyExchangeRateRow ...
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

            ADailyExchangeRateRow aDailyExchangeRateRow = FMainDS.ADailyExchangeRate.NewRowTyped();

            aDailyExchangeRateRow.FromCurrencyCode = baseCurrencyOfLedger;

            if (strCurrencyToDefault == null)
            {
                if (FPreviouslySelectedDetailRow == null)
                {
                    aDailyExchangeRateRow.ToCurrencyCode = baseCurrencyOfLedger;
                    aDailyExchangeRateRow.RateOfExchange = 1.0m;
                }
                else
                {
                    aDailyExchangeRateRow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
                    aDailyExchangeRateRow.RateOfExchange = Decimal.Parse(txtDetailRateOfExchange.Text);
                }
            }
            else
            {
                aDailyExchangeRateRow.ToCurrencyCode = strCurrencyToDefault;
                aDailyExchangeRateRow.RateOfExchange = 1.0m;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                cmbDetailFromCurrencyCode.SetSelectedString(aDailyExchangeRateRow.FromCurrencyCode);
                cmbDetailToCurrencyCode.SetSelectedString(aDailyExchangeRateRow.ToCurrencyCode);
            }

            aDailyExchangeRateRow.DateEffectiveFrom = dateDate;
            aDailyExchangeRateRow.TimeEffectiveFrom =
                (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;

            FMainDS.ADailyExchangeRate.Rows.Add(aDailyExchangeRateRow);
            grdDetails.Refresh();

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ADailyExchangeRate.Rows.Count - 1);
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
            if (FMainDS.ADailyExchangeRate.DefaultView.RowFilter.Equals(""))
            {
                DateTime dateLimit = dtpDetailDateEffectiveFrom.Date.Value.AddDays(1.0);
                // Do not use local formats here!
                DateTimeFormatInfo dateTimeFormat =
                    new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
                string dateString = dateLimit.ToString("d", dateTimeFormat);
                FMainDS.ADailyExchangeRate.DefaultView.RowFilter =
                    "a_date_effective_from_d < '" + dateString + "'";
                String strBtnUseDateToFilter2 = Catalog.GetString("Unuse Filter");
                btnUseDateToFilter.Text = strBtnUseDateToFilter2;
            }
            else
            {
                FMainDS.ADailyExchangeRate.DefaultView.RowFilter = "";
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
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
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
            ADailyExchangeRateRow actualRow = GetSelectedDetailRow();

            SelectByIndex(-1);
            FMainDS.ADailyExchangeRate.Rows.Remove(actualRow);
            FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// SelectByIndex is a copy&amp;paste routine which is always sligtly modified
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

        private void GetDetailDataFromControlsManual(ADailyExchangeRateRow ARow)
        {
            TExchangeRateCache.ResetCache();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import exchange rates from spreadsheet file");
            dialog.Filter = Catalog.GetString("Spreadsheet files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string directory = Path.GetDirectoryName(dialog.FileName);
                string[] ymlFiles = Directory.GetFiles(directory, "*.yml");
                string definitionFileName = String.Empty;

                if (ymlFiles.Length == 1)
                {
                    definitionFileName = ymlFiles[0];
                }
                else
                {
                    // show another open file dialog for the description file
                    OpenFileDialog dialogDefinitionFile = new OpenFileDialog();
                    dialogDefinitionFile.Title = Catalog.GetString("Please select a yml file that describes the content of the spreadsheet");
                    dialogDefinitionFile.Filter = Catalog.GetString("Data description files (*.yml)|*.yml");

                    if (dialogDefinitionFile.ShowDialog() == DialogResult.OK)
                    {
                        definitionFileName = dialogDefinitionFile.FileName;
                    }
                }

                if (File.Exists(definitionFileName))
                {
                    TYml2Xml parser = new TYml2Xml(definitionFileName);
                    XmlDocument dataDescription = parser.ParseYML2XML();
                    XmlNode RootNode = TXMLParser.FindNodeRecursive(dataDescription.DocumentElement, "RootNode");

                    if (Path.GetExtension(dialog.FileName).ToLower() == ".csv")
                    {
                        ImportFromCSVFile(dialog.FileName, RootNode);
                    }
                }
            }
        }

        private void ImportFromCSVFile(string ADataFilename, XmlNode ARootNode)
        {
            StreamReader dataFile = new StreamReader(ADataFilename, System.Text.Encoding.Default);

            string Separator = TXMLParser.GetAttribute(ARootNode, "Separator");
            string DateFormat = TXMLParser.GetAttribute(ARootNode, "DateFormat");
            string ThousandsSeparator = TXMLParser.GetAttribute(ARootNode, "ThousandsSeparator");
            string DecimalSeparator = TXMLParser.GetAttribute(ARootNode, "DecimalSeparator");

            // assumes date in first column, exchange rate in second column
            // picks the names of the currencies from the file name: CUR1_CUR2.csv
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ADataFilename);

            if (!fileNameWithoutExtension.Contains("_"))
            {
                MessageBox.Show(Catalog.GetString("Cannot import exchange rates, please name the file after the currencies involved, eg. KES_EUR.csv"));
            }

            string[] Currencies = fileNameWithoutExtension.Split(new char[] { '_' });

            // TODO: check for valid currency codes? at the moment should fail on foreign key
            // TODO: disconnect the grid from the datasource to avoid flickering?

            while (!dataFile.EndOfStream)
            {
                string line = dataFile.ReadLine();

                DateTime dateEffective = XmlConvert.ToDateTime(StringHelper.GetNextCSV(ref line, Separator, false), DateFormat);
                string ExchangeRateString = StringHelper.GetNextCSV(ref line, Separator, false).
                                            Replace(ThousandsSeparator, "").
                                            Replace(DecimalSeparator, ".");

                decimal ExchangeRate = Convert.ToDecimal(ExchangeRateString, System.Globalization.CultureInfo.InvariantCulture);

                ADailyExchangeRateRow exchangeRow =
                    (ADailyExchangeRateRow)FMainDS.ADailyExchangeRate.Rows.Find(new object[] { Currencies[0], Currencies[1], dateEffective,
                                                                                               0 });

                if (exchangeRow == null)
                {
                    exchangeRow = FMainDS.ADailyExchangeRate.NewRowTyped();
                    exchangeRow.FromCurrencyCode = Currencies[0];
                    exchangeRow.ToCurrencyCode = Currencies[1];
                    exchangeRow.DateEffectiveFrom = dateEffective;
                    FMainDS.ADailyExchangeRate.Rows.Add(exchangeRow);
                }

                exchangeRow.RateOfExchange = ExchangeRate;
            }

            dataFile.Close();

            FPetraUtilsObject.SetChangedFlag();
        }
    }
}