//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       chris, christiank, timop
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
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Common.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// this provides some static functions that import
    /// daily and corporate exchange rates
    /// </summary>

    public class TImportExchangeRates
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="AExchangeRateDT">Determines whether corporate or daily exchange rates specified</param>
        /// <param name="AImportMode">Determines whether corporate or daily exchange rates specified</param>
        public static void ImportCurrencyExRates(TTypedDataTable AExchangeRateDT, string AImportMode)
        {
            OpenFileDialog DialogBox = new OpenFileDialog();

            DialogBox.Title = Catalog.GetString("Import exchange rates from spreadsheet file");
            DialogBox.Filter = Catalog.GetString("Spreadsheet files (*.csv)|*.csv");

            if (DialogBox.ShowDialog() == DialogResult.OK)
            {
                String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

                TDlgSelectCSVSeparator DlgSeparator = new TDlgSelectCSVSeparator(false);
                DlgSeparator.CSVFileName = DialogBox.FileName;

                DlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    DlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                DlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (DlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    ImportCurrencyExRatesFromCSV(AExchangeRateDT,
                        DialogBox.FileName,
                        DlgSeparator.SelectedSeparator,
                        DlgSeparator.NumberFormat,
                        DlgSeparator.DateFormat,
                        AImportMode);
                }
            }
        }

        /// <summary>
        /// Imports currency exchange rates, daily and corporate,
        /// from a one-of-two-styles formatted CSV file
        /// </summary>
        /// <param name="AExchangeRDT">Daily or Corporate exchange rate table</param>
        /// <param name="ADataFilename">The .CSV file to process</param>
        /// <param name="ACSVSeparator"></param>
        /// <param name="ANumberFormat"></param>
        /// <param name="ADateFormat"></param>
        /// <param name="AImportMode">Daily or Corporate</param>
        private static void ImportCurrencyExRatesFromCSV(TTypedDataTable AExchangeRDT,
            string ADataFilename,
            string ACSVSeparator,
            string ANumberFormat,
            string ADateFormat,
            string AImportMode)
        {
            bool IsShortFileFormat;
            int x, y;

            string[] Currencies = new string[2];

            if ((AImportMode != "Corporate") && (AImportMode != "Daily"))
            {
                throw new ArgumentException("Invalid value '" + AImportMode + "' for mode argument: Valid values are Corporate and Daily");
            }
            else if ((AImportMode == "Corporate") && (AExchangeRDT.GetType() != typeof(ACorporateExchangeRateTable)))
            {
                throw new ArgumentException("Invalid type of exchangeRateDT argument for mode: 'Corporate'. Needs to be: ACorporateExchangeRateTable");
            }
            else if ((AImportMode == "Daily") && (AExchangeRDT.GetType() != typeof(ADailyExchangeRateTable)))
            {
                throw new ArgumentException("Invalid type of exchangeRateDT argument for mode: 'Daily'. Needs to be: ADailyExchangeRateTable");
            }

            StreamReader DataFile = new StreamReader(ADataFilename, System.Text.Encoding.Default);

            string ThousandsSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "," : ".");
            string DecimalSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "." : ",");

            CultureInfo MyCultureInfoDate = new CultureInfo("en-GB");
            MyCultureInfoDate.DateTimeFormat.ShortDatePattern = ADateFormat;

            // TODO: check for valid currency codes? at the moment should fail on foreign key
            // TODO: disconnect the grid from the datasource to avoid flickering?

            string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(ADataFilename);

            if ((FileNameWithoutExtension.IndexOf("_") == 3)
                && (FileNameWithoutExtension.LastIndexOf("_") == 3)
                && (FileNameWithoutExtension.Length == 7))
            {
                // File name format assumed to be like this: USD_HKD.csv
                IsShortFileFormat = true;
                Currencies = FileNameWithoutExtension.Split(new char[] { '_' });
            }
            else
            {
                IsShortFileFormat = false;
            }

            // To store the From and To currencies
            // Use an array to store these to make for easy
            //   inverting of the two currencies when calculating
            //   the inverse value.

            while (!DataFile.EndOfStream)
            {
                string Line = DataFile.ReadLine();

                //Convert separator to a char
                char Sep = ACSVSeparator[0];
                //Turn current line into string array of column values
                string[] CsvColumns = Line.Split(Sep);

                int NumCols = CsvColumns.Length;

                //If number of columns is not 4 then import csv file is wrongly formed.
                if (IsShortFileFormat && (NumCols != 2))
                {
                    MessageBox.Show(Catalog.GetString("Failed to import the CSV currency file:\r\n\r\n" +
                            "   " + ADataFilename + "\r\n\r\n" +
                            "It contains " + NumCols.ToString() + " columns. " +
                            "Import files with names like 'USD_HKD.csv', where the From and To currencies" +
                            " are given in the name, should contain 2 columns:\r\n\r\n" +
                            "  1. Effective Date\r\n" +
                            "  2. Exchange Rate"
                            ), AImportMode + " Exchange Rates Import Error");
                    return;
                }
                else if (!IsShortFileFormat && (NumCols != 4))
                {
                    MessageBox.Show(Catalog.GetString("Failed to import the CSV currency file:\r\n\r\n" +
                            "    " + ADataFilename + "\r\n\r\n" +
                            "It contains " + NumCols.ToString() + " columns. It should be 4:\r\n\r\n" +
                            "    1. From Currency\r\n" +
                            "    2. To Currency\r\n" +
                            "    3. Effective Date\r\n" +
                            "    4. Exchange Rate"
                            ), AImportMode + " Exchange Rates Import Error");
                    return;
                }

                if (!IsShortFileFormat)
                {
                    //Read the values for the current line
                    //From currency
                    Currencies[0] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false).ToString();
                    //To currency
                    Currencies[1] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false).ToString();
                }

                //TODO: Perform validation on the From and To currencies at this point!!
                //TODO: Date parsing as in Petra 2.x instead of using XML date format!!!
                string DateEffectiveStr = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false);
                DateTime DateEffective = Convert.ToDateTime(DateEffectiveStr, MyCultureInfoDate);
                string ExchangeRateString = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false).Replace(ThousandsSeparator, "").Replace(
                    DecimalSeparator,
                    ".");

                decimal ExchangeRate = Convert.ToDecimal(ExchangeRateString, System.Globalization.CultureInfo.InvariantCulture);

                if ((AImportMode == "Corporate") && AExchangeRDT is ACorporateExchangeRateTable)
                {
                    ACorporateExchangeRateTable ExchangeRateDT = (ACorporateExchangeRateTable)AExchangeRDT;

                    // run this code in the loop twice to get ExchangeRate value and its inverse
                    for (int i = 0; i <= 1; i++)
                    {
                        //this will cause x and y to go from 0 to 1 and 1 to 0 respectively
                        x = i;
                        y = Math.Abs(i - 1);

                        ACorporateExchangeRateRow ExchangeRow = (ACorporateExchangeRateRow)ExchangeRateDT.Rows.
                                                                Find(new object[] { Currencies[x], Currencies[y], DateEffective });

                        if (ExchangeRow == null)                                                                                    // remove 0 for Corporate
                        {
                            ExchangeRow = (ACorporateExchangeRateRow)ExchangeRateDT.NewRowTyped();
                            ExchangeRow.FromCurrencyCode = Currencies[x];
                            ExchangeRow.ToCurrencyCode = Currencies[y];
                            ExchangeRow.DateEffectiveFrom = DateEffective;
                            ExchangeRateDT.Rows.Add(ExchangeRow);
                        }

                        if (i == 0)
                        {
                            ExchangeRow.RateOfExchange = ExchangeRate;
                        }
                        else
                        {
                            ExchangeRow.RateOfExchange = 1 / ExchangeRate;
                        }
                    }
                }
                else if ((AImportMode == "Daily") && AExchangeRDT is ADailyExchangeRateTable)
                {
                    ADailyExchangeRateTable ExchangeRateDT = (ADailyExchangeRateTable)AExchangeRDT;

                    // run this code in the loop twice to get ExchangeRate value and its inverse
                    for (int i = 0; i <= 1; i++)
                    {
                        //this will cause x and y to go from 0 to 1 and 1 to 0 respectively
                        x = i;
                        y = Math.Abs(i - 1);

                        ADailyExchangeRateRow ExchangeRow = (ADailyExchangeRateRow)ExchangeRateDT.Rows.
                                                            Find(new object[] { Currencies[x], Currencies[y], DateEffective, 0 });

                        if (ExchangeRow == null)                                                                                    // remove 0 for Corporate
                        {
                            ExchangeRow = (ADailyExchangeRateRow)ExchangeRateDT.NewRowTyped();
                            ExchangeRow.FromCurrencyCode = Currencies[x];
                            ExchangeRow.ToCurrencyCode = Currencies[y];
                            ExchangeRow.DateEffectiveFrom = DateEffective;
                            ExchangeRateDT.Rows.Add(ExchangeRow);
                        }

                        if (i == 0)
                        {
                            ExchangeRow.RateOfExchange = ExchangeRate;
                        }
                        else
                        {
                            ExchangeRow.RateOfExchange = 1 / ExchangeRate;
                        }
                    }
                }
            }

            DataFile.Close();
        }
    }
}