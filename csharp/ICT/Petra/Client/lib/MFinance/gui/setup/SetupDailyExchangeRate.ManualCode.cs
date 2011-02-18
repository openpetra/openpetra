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
        NumberFormatInfo numberFormatInfo = null;

        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        String baseCurrencyOfLedger;

        bool blnSelectedRowChangeable = false;

        /// <summary>
        /// The definition of the ledgernumber is only used to define some
        /// default values.
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
                }
                catch (System.NotSupportedException)
                {
                    numberFormatInfo =
                        new System.Globalization.CultureInfo(
                            "en-US", false).NumberFormat;
                }

                numberFormatInfo.NumberDecimalDigits = 8;


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
            }
        }

        /// <summary>
        /// The focus is send to the grid to "unfocus" the input controls and to
        /// enforce that the dataset is closed
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        public void SetTheFocusToTheGrid(object sender, EventArgs e)
        {
            grdDetails.Focus();
        }

        /// <summary>
        /// Create a new DailyExchangeRateRow ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateDate = DateTime.Parse(dateTimeNow.ToLongDateString());
            DateTime dateTime = DateTime.Parse(dateTimeNow.ToLongTimeString());

            ADailyExchangeRateRow aDailyExchangeRateRow = FMainDS.ADailyExchangeRate.NewRowTyped();

            aDailyExchangeRateRow.FromCurrencyCode = baseCurrencyOfLedger;

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

            aDailyExchangeRateRow.DateEffectiveFrom = dateDate;
            aDailyExchangeRateRow.TimeEffectiveFrom =
                (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;

            FMainDS.ADailyExchangeRate.Rows.Add(aDailyExchangeRateRow);

            FPetraUtilsObject.SetChangedFlag();
            SelectDetailRowByDataTableIndex(FMainDS.ADailyExchangeRate.Rows.Count - 1);
        }

        /// <summary>
        /// Validating Event for txtDetailRateOfExchange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            decimal exchangeRate;
            exchangeRate = Decimal.Parse(txtDetailRateOfExchange.Text);

            lblValueOneDirection.Text = "1.0 " +
                                        FPreviouslySelectedDetailRow.FromCurrencyCode.ToString() + " " +
                                        exchangeRate.ToString("N", numberFormatInfo) + " " +
                                        FPreviouslySelectedDetailRow.ToCurrencyCode.ToString();
            try
            {
                exchangeRate = 1 / exchangeRate;
            }
            catch (Exception)
            {
                exchangeRate = 0;
            }
            lblValueOtherDirection.Text = "1.0 " +
                                          FPreviouslySelectedDetailRow.ToCurrencyCode.ToString() + " " +
                                          exchangeRate.ToString("N", numberFormatInfo) + " " +
                                          FPreviouslySelectedDetailRow.FromCurrencyCode.ToString();
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
            }
            else
            {
                if (blnSelectedRowChangeable)
                {
                    txtDetailRateOfExchange.Enabled = true;
                }
            }

            if (blnSelectedRowChangeable)
            {
                cmbDetailToCurrencyCode.Enabled =
                    (cmbDetailFromCurrencyCode.GetSelectedString() == baseCurrencyOfLedger);

                cmbDetailFromCurrencyCode.Enabled =
                    (cmbDetailToCurrencyCode.GetSelectedString() == baseCurrencyOfLedger);
            }
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
        {
//              if (ARow.RowState == DataRowState.Added)
//              {
//                      System.Diagnostics.Debug.WriteLine("Added");
//              }
//              if (ARow.RowState == DataRowState.Deleted)
//              {
//                      System.Diagnostics.Debug.WriteLine("Deleted");
//              }
//              if (ARow.RowState == DataRowState.Detached)
//              {
//                      System.Diagnostics.Debug.WriteLine("Detached");
//              }
//              if (ARow.RowState == DataRowState.Modified)
//              {
//                      System.Diagnostics.Debug.WriteLine("Modified");
//              }
//              if (ARow.RowState == DataRowState.Unchanged)
//              {
//                      System.Diagnostics.Debug.WriteLine("Unchanged");
//              }
            blnSelectedRowChangeable = !(ARow.RowState == DataRowState.Unchanged);
            ValidatedExchangeRate();
            txtDetailRateOfExchange.Enabled = (ARow.RowState == DataRowState.Added);
            blnSelectedRowChangeable = (ARow.RowState == DataRowState.Added);
            ValueChangedCurrencyCode();
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