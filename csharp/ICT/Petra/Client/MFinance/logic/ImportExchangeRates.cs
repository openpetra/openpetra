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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MCommon.Data;
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
        /// <param name="AExchangeRateDT">The corporate or daily exchange rate table</param>
        /// <param name="AImportMode">Determines whether corporate or daily exchange rates specified - either 'Daily' or 'Corporate'</param>
        /// <param name="AResultCollection">A validation collection to which errors will be added</param>
        /// <returns>The number of rows that were actually imported.  Rows that duplicate existing rows do not count.  
        /// This is usually because this is an attempt to import again after a failed previous attempt.</returns>
        public static int ImportCurrencyExRates(TTypedDataTable AExchangeRateDT, string AImportMode, TVerificationResultCollection AResultCollection)
        {
            OpenFileDialog DialogBox = new OpenFileDialog();

            DialogBox.Title = Catalog.GetString("Import exchange rates from spreadsheet file");
            DialogBox.Filter = Catalog.GetString("Spreadsheet files (*.csv)|*.csv");

            if (DialogBox.ShowDialog() == DialogResult.OK)
            {
                String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

                TDlgSelectCSVSeparator DlgSeparator = new TDlgSelectCSVSeparator(false);
                Boolean fileCanOpen = DlgSeparator.OpenCsvFile(DialogBox.FileName);

                if (!fileCanOpen)
                {
                    MessageBox.Show(Catalog.GetString("Unable to open file."),
                        Catalog.GetString("Import Exchange Rates"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return 0;
                }

                DlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    DlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                DlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (DlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    return ImportCurrencyExRatesFromCSV(AExchangeRateDT,
                        DialogBox.FileName,
                        DlgSeparator.SelectedSeparator,
                        DlgSeparator.NumberFormat,
                        DlgSeparator.DateFormat,
                        AImportMode,
                        AResultCollection);
                }
            }

            return 0;
        }

        /// <summary>
        /// This over-ride should be used for testing purposes.  It reads from the specified test file and always uses US numbers and date/time.
        /// You supply the separator character as a single character string
        /// </summary>
        /// <param name="AExchangeRateDT">The corporate or daily exchange rate table</param>
        /// <param name="AImportFileName">The test file to import</param>
        /// <param name="ACSVSeparator">The separator that the file uses</param>
        /// <param name="AImportMode">Determines whether corporate or daily exchange rates specified - either 'Daily' or 'Corporate'</param>
        /// <param name="AResultCollection">A validation collection to which errors will be added</param>
        /// <returns>The number of rows that were actually imported.  Rows that duplicate existing rows do not count.  
        /// This is usually because this is an attempt to import again after a failed previous attempt.</returns>
        public static int ImportCurrencyExRates(TTypedDataTable AExchangeRateDT,
            string AImportFileName,
            string ACSVSeparator,
            string AImportMode,
            TVerificationResultCollection AResultCollection)
        {
            // Test import always uses standard file with US formats
            return ImportCurrencyExRatesFromCSV(AExchangeRateDT,
                AImportFileName,
                ACSVSeparator,
                TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN,
                "MM/dd/yyyy",
                AImportMode,
                AResultCollection);
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
        /// <param name="AResultCollection">A validation collection to which errors will be added</param>
        /// <returns>The number of rows that were actually imported.  Rows that duplicate existing rows do not count.  
        /// This is usually because this is an attempt to import again after a failed previous attempt.</returns>
        private static int ImportCurrencyExRatesFromCSV(TTypedDataTable AExchangeRDT,
            string ADataFilename,
            string ACSVSeparator,
            string ANumberFormat,
            string ADateFormat,
            string AImportMode,
            TVerificationResultCollection AResultCollection)
        {
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

            bool IsShortFileFormat;
            int x, y;

            // To store the From and To currencies
            // Use an array to store these to make for easy
            //   inverting of the two currencies when calculating
            //   the inverse value.
            string[] Currencies = new string[2];

            Type DataTableType;
            int RowsImported = 0;

            ACurrencyTable allCurrencies = new ACurrencyTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("CurrencyCodeList", String.Empty, null, out DataTableType);
            allCurrencies.Merge(CacheDT);

            using (StreamReader DataFile = new StreamReader(ADataFilename, System.Text.Encoding.Default))
            {
                string ThousandsSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "," : ".");
                string DecimalSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "." : ",");

                CultureInfo MyCultureInfoDate = new CultureInfo("en-GB");
                MyCultureInfoDate.DateTimeFormat.ShortDatePattern = ADateFormat;

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

                int LineNumber = 0;

                while (!DataFile.EndOfStream)
                {
                    string Line = DataFile.ReadLine();
                    LineNumber++;

                    // See if the first line is a special case??
                    if (LineNumber == 1)
                    {
                        // see if the first line is a text header - look for digits
                        // A valid header would look like:  From,To,Date,Rate
                        bool bFoundDigit = false;

                        for (int i = 0; i < Line.Length; i++)
                        {
                            char c = Line[i];

                            if ((c >= '0') && (c <= '9'))
                            {
                                bFoundDigit = true;
                                break;
                            }
                        }

                        if (!bFoundDigit)
                        {
                            // No digits so we will assume the line is a header
                            continue;
                        }
                    }

                    //Convert separator to a char
                    char Sep = ACSVSeparator[0];
                    //Turn current line into string array of column values
                    string[] CsvColumns = Line.Split(Sep);

                    int NumCols = CsvColumns.Length;

                    //If number of columns is not 4 then import csv file is wrongly formed.
                    if (IsShortFileFormat && (NumCols < 2))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString("Failed to import the CSV currency file:{0}   {1}{0}{0}"),
                            Environment.NewLine, ADataFilename);
                        resultText +=
                            String.Format(Catalog.GetString(
                                    "Line #{1} contains {2} column(s). Import files with names like 'USD_HKD.csv', where the From and To currencies are given in the name, should contain 2 or 3 columns:{0}{0}"),
                                Environment.NewLine, LineNumber, NumCols.ToString());
                        resultText +=
                            String.Format(Catalog.GetString(
                                    "  1. Effective Date{0}  2. Exchange Rate{0}  3. Effective time in seconds (Optional for Daily Rate only)"),
                                Environment.NewLine);
                        TVerificationResult result = new TVerificationResult(AImportMode,
                            resultText,
                            CommonErrorCodes.ERR_INFORMATIONMISSING,
                            TResultSeverity.Resv_Critical);
                        AResultCollection.Add(result);
                        return RowsImported;
                    }
                    else if (!IsShortFileFormat && (NumCols < 4))
                    {
                        string resultText = String.Format(Catalog.GetString("Failed to import the CSV currency file:{0}   {1}{0}{0}"),
                            Environment.NewLine, ADataFilename);
                        resultText += String.Format(Catalog.GetString("Line #{1} contains {2} column(s). It should have 4 or 5 as follows:{0}{0}"),
                            Environment.NewLine, LineNumber, NumCols.ToString());
                        resultText +=
                            String.Format(Catalog.GetString(
                                    "    1. From Currency{0}    2. To Currency{0}    3. Effective Date{0}    4. Exchange Rate{0}    5. Effective time in seconds (Optional for Daily Rate only)"),
                                Environment.NewLine);
                        TVerificationResult result = new TVerificationResult(AImportMode,
                            resultText,
                            CommonErrorCodes.ERR_INFORMATIONMISSING,
                            TResultSeverity.Resv_Critical);
                        AResultCollection.Add(result);
                        return RowsImported;
                    }

                    if (!IsShortFileFormat)
                    {
                        //Read the values for the current line
                        //From currency
                        Currencies[0] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).ToString();
                        //To currency
                        Currencies[1] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).ToString();
                    }

                    // Perform validation on the From and To currencies at this point!!
                    if ((allCurrencies.Rows.Find(Currencies[0]) == null) || (allCurrencies.Rows.Find(Currencies[1]) == null))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString("Invalid currency in import file in line #{0}"), LineNumber.ToString());
                        TVerificationResult result = new TVerificationResult(AImportMode,
                            resultText,
                            CommonErrorCodes.ERR_INCONGRUOUSSTRINGS,
                            TResultSeverity.Resv_Critical);
                        AResultCollection.Add(result);
                        return RowsImported;
                    }

                    // Date parsing as in Petra 2.x instead of using XML date format!!!
                    string DateEffectiveStr = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).Replace("\"", String.Empty);
                    DateTime DateEffective;

                    if (!DateTime.TryParse(DateEffectiveStr, MyCultureInfoDate, DateTimeStyles.None, out DateEffective))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Invalid date ({0}) in import file in line #{1}"), DateEffectiveStr, LineNumber.ToString());
                        TVerificationResult result = new TVerificationResult(AImportMode,
                            resultText,
                            CommonErrorCodes.ERR_INVALIDDATE,
                            TResultSeverity.Resv_Critical);
                        AResultCollection.Add(result);
                        return RowsImported;
                    }

                    decimal ExchangeRate = 0.0m;
                    try
                    {
                        string ExchangeRateString =
                            StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).Replace(ThousandsSeparator, "").Replace(
                                DecimalSeparator, ".").Replace("\"", String.Empty);

                        ExchangeRate = Convert.ToDecimal(ExchangeRateString, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Invalid rate of exchange in import file in line #{0}"), LineNumber.ToString());
                        TVerificationResult result = new TVerificationResult(AImportMode,
                            resultText,
                            CommonErrorCodes.ERR_INVALIDNUMBER,
                            TResultSeverity.Resv_Critical);
                        AResultCollection.Add(result);
                        return RowsImported;
                    }

                    int TimeEffective = 7200;

                    if (AImportMode == "Daily")
                    {
                        // Daily rate imports can have an optional final column which is the time
                        // Otherwise we assume the time is a default of 7200 (02:00am)
                        if ((IsShortFileFormat && (NumCols == 3))
                            || (!IsShortFileFormat && (NumCols == 5)))
                        {
                            string timeEffectiveStr = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true);
                            int t = (int)new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(timeEffectiveStr, typeof(int));

                            if (t < 0)
                            {
                                // it wasn't in the format 02:00
                                if (!Int32.TryParse(timeEffectiveStr, out t))
                                {
                                    // Not a regular Int32 either
                                    t = -1;
                                }
                            }

                            if ((t >= 0) && (t < 86400))
                            {
                                TimeEffective = t;
                            }
                            else
                            {
                                // raise an error
                                string resultText = String.Format(Catalog.GetString(
                                        "Invalid effective time in import file in line #{0}"), LineNumber.ToString());
                                TVerificationResult result = new TVerificationResult(AImportMode,
                                    resultText,
                                    CommonErrorCodes.ERR_INVALIDINTEGERTIME,
                                    TResultSeverity.Resv_Critical);
                                AResultCollection.Add(result);
                                return RowsImported;
                            }
                        }
                    }

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
                                RowsImported++;

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
                                                                Find(new object[] { Currencies[x], Currencies[y], DateEffective, TimeEffective });

                            if (ExchangeRow == null)                                                                                    // remove 0 for Corporate
                            {
                                ExchangeRow = (ADailyExchangeRateRow)ExchangeRateDT.NewRowTyped();
                                ExchangeRow.FromCurrencyCode = Currencies[x];
                                ExchangeRow.ToCurrencyCode = Currencies[y];
                                ExchangeRow.DateEffectiveFrom = DateEffective;
                                ExchangeRow.TimeEffectiveFrom = TimeEffective;
                                ExchangeRateDT.Rows.Add(ExchangeRow);
                                RowsImported++;
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

                return RowsImported;
            }
        }
    }
}