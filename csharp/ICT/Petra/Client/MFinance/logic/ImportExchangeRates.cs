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
using System.Text;
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
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// this provides some static functions that import
    /// daily and corporate exchange rates
    /// </summary>

    public class TImportExchangeRates
    {
        // some constants used in validation and messaging
        private const int MAX_MESSAGE_COUNT = 20;

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

                bool dateMayBeAnInteger = TUserDefaults.GetBooleanDefault(MCommonConstants.USERDEFAULT_IMPORTEDDATESMAYBEINTEGERS, false);
                TDlgSelectCSVSeparator DlgSeparator = new TDlgSelectCSVSeparator(false);
                DlgSeparator.DateMayBeInteger = dateMayBeAnInteger;
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
                DlgSeparator.NumberFormat = (impOptions.Length > 1) ? impOptions.Substring(1) : TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
                DlgSeparator.SelectedSeparator = StringHelper.GetCSVSeparator(DlgSeparator.FileContent) ??
                                                 ((impOptions.Length > 0) ? impOptions.Substring(0, 1) : ";");

                if (DlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    // Save the settings that we specified
                    SaveOptions(DlgSeparator);

                    // Do the import and return the number of rows imported and any messages
                    return ImportCurrencyExRatesFromCSV(AExchangeRateDT,
                        DlgSeparator.FileContent,
                        DlgSeparator.SelectedSeparator,
                        DlgSeparator.NumberFormat,
                        DlgSeparator.DateFormat,
                        dateMayBeAnInteger,
                        AImportMode,
                        AResultCollection,
                        Path.GetFileNameWithoutExtension(DialogBox.FileName));
                }
                else
                {
                    // Save the options anyway
                    SaveOptions(DlgSeparator);
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
            TDlgSelectCSVSeparator separatorDialog = new TDlgSelectCSVSeparator(false);

            separatorDialog.OpenCsvFile(AImportFileName);

            return ImportCurrencyExRatesFromCSV(AExchangeRateDT,
                separatorDialog.FileContent,
                ACSVSeparator,
                TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN,
                "MM/dd/yyyy",
                false,
                AImportMode,
                AResultCollection,
                Path.GetFileNameWithoutExtension(AImportFileName));
        }

        private static void SaveOptions(TDlgSelectCSVSeparator ASeparatorDialog)
        {
            string impOptions = ASeparatorDialog.SelectedSeparator;

            impOptions += ASeparatorDialog.NumberFormat;
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", ASeparatorDialog.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }

        /// <summary>
        /// Imports currency exchange rates, daily and corporate,
        /// from a one-of-two-styles formatted CSV file
        /// </summary>
        /// <param name="AExchangeRDT">Daily or Corporate exchange rate table</param>
        /// <param name="ADataFileContent">The content of the .CSV file to process</param>
        /// <param name="ACSVSeparator"></param>
        /// <param name="ANumberFormat"></param>
        /// <param name="ADateFormat"></param>
        /// <param name="ADateMayBeAnInteger"></param>
        /// <param name="AImportMode">Daily or Corporate</param>
        /// <param name="AResultCollection">A validation collection to which errors will be added</param>
        /// <param name="ACurrencyPair">The encoding of the file</param>
        /// <returns>The number of rows that were actually imported.  Rows that duplicate existing rows do not count.
        /// This is usually because this is an attempt to import again after a failed previous attempt.</returns>
        private static int ImportCurrencyExRatesFromCSV(TTypedDataTable AExchangeRDT,
            string ADataFileContent,
            string ACSVSeparator,
            string ANumberFormat,
            string ADateFormat,
            bool ADateMayBeAnInteger,
            string AImportMode,
            TVerificationResultCollection AResultCollection,
            string ACurrencyPair = ""
            )
        {
            // Keep a list of errors/warnings and severity
            List <Tuple <string, TResultSeverity>>InvalidRows = new List <Tuple <string, TResultSeverity>>();

            // keep a variable that becomes true if any row has an invalid column count, so we can show a helpful message
            bool InvalidColumnCount = false;

            // Check our method parameters
            if ((AImportMode != "Corporate") && (AImportMode != "Daily"))
            {
                throw new ArgumentException("Invalid value '" + AImportMode + "' for mode argument: Valid values are Corporate and Daily");
            }
            else if ((AImportMode == "Corporate") && !(AExchangeRDT is ACorporateExchangeRateTable))
            {
                throw new ArgumentException("Invalid type of exchangeRateDT argument for mode: 'Corporate'. Needs to be: ACorporateExchangeRateTable");
            }
            else if ((AImportMode == "Daily") && !(AExchangeRDT is ADailyExchangeRateTable))
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

            // This table will tell us the base currencies used by the current set of ledgers
            ALedgerTable ledgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();

            List <string>allBaseCurrencies = new List <string>();
            DateTime maxRecommendedEffectiveDate = DateTime.MaxValue;
            DateTime minRecommendedEffectiveDate = DateTime.MinValue;
            int preferredPeriodStartDay = 0;

            // Use the ledger table rows to get a list of base currencies and current period end dates
            for (int i = 0; i < ledgers.Rows.Count; i++)
            {
                ALedgerRow ledger = (ALedgerRow)ledgers.Rows[i];
                string code = ledger.BaseCurrency;

                if (ledger.LedgerStatus == true)
                {
                    if (allBaseCurrencies.Contains(code) == false)
                    {
                        allBaseCurrencies.Add(code);
                    }

                    DataTable AccountingPeriods = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                        ledger.LedgerNumber);
                    AAccountingPeriodRow currentPeriodRow = (AAccountingPeriodRow)AccountingPeriods.Rows.Find(new object[] { ledger.LedgerNumber,
                                                                                                                             ledger.CurrentPeriod });
                    AAccountingPeriodRow forwardPeriodRow = (AAccountingPeriodRow)AccountingPeriods.Rows.Find(
                        new object[] { ledger.LedgerNumber,
                                       ledger.CurrentPeriod +
                                       ledger.NumberFwdPostingPeriods });

                    if ((forwardPeriodRow != null)
                        && ((maxRecommendedEffectiveDate == DateTime.MaxValue) || (forwardPeriodRow.PeriodEndDate > maxRecommendedEffectiveDate)))
                    {
                        maxRecommendedEffectiveDate = forwardPeriodRow.PeriodEndDate;
                    }

                    if ((currentPeriodRow != null)
                        && ((minRecommendedEffectiveDate == DateTime.MinValue) || (currentPeriodRow.PeriodStartDate > minRecommendedEffectiveDate)))
                    {
                        minRecommendedEffectiveDate = currentPeriodRow.PeriodStartDate;
                    }

                    if ((currentPeriodRow != null) && (preferredPeriodStartDay == 0))
                    {
                        preferredPeriodStartDay = currentPeriodRow.PeriodStartDate.Day;
                    }
                    else if ((currentPeriodRow != null) && (currentPeriodRow.PeriodStartDate.Day != preferredPeriodStartDay))
                    {
                        preferredPeriodStartDay = -1;
                    }
                }
            }

            // This will tell us the complete list of all available currencies
            ACurrencyTable allCurrencies = new ACurrencyTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("CurrencyCodeList", String.Empty, null, out DataTableType);
            allCurrencies.Merge(CacheDT);
            allCurrencies.CaseSensitive = true;

            // Start reading the file
            using (StringReader reader = new StringReader(ADataFileContent))
            {
                CultureInfo MyCultureInfoDate = StringHelper.GetCultureInfoForDateFormat(ADateFormat);

                // TODO: disconnect the grid from the datasource to avoid flickering?

                if ((ACurrencyPair.IndexOf("_") == 3)
                    && (ACurrencyPair.LastIndexOf("_") == 3)
                    && (ACurrencyPair.Length == 7))
                {
                    // File name format assumed to be like this: USD_HKD.csv
                    IsShortFileFormat = true;
                    string[] items = ACurrencyPair.Split(new char[] { '_' });
                    Currencies[0] = items[0].ToUpper();
                    Currencies[1] = items[1].ToUpper();
                }
                else
                {
                    IsShortFileFormat = false;
                }

                int LineNumber = 0;
                string Line = reader.ReadLine();

                while (Line != null)
                {
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
                            Line = reader.ReadLine();
                            continue;
                        }
                    }

                    if ((Line.Trim().Length == 0) || Line.StartsWith("#") || Line.StartsWith("/*"))
                    {
                        Line = reader.ReadLine();
                        continue;
                    }

                    int NumCols = StringHelper.GetCSVList(Line, ACSVSeparator).Count;

                    // Do we have the correct number of columns?
                    int minColCount = IsShortFileFormat ? 2 : 4;
                    int maxColCount = (AImportMode == "Daily") ? minColCount + 1 : minColCount;

                    if ((NumCols < minColCount) || (NumCols > maxColCount))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetPluralString(
                                "Line {0}: contains 1 column", "Line {0}: contains {1} columns", NumCols, true),
                            LineNumber, NumCols.ToString());

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        InvalidColumnCount = true;
                        Line = reader.ReadLine();
                        continue;
                    }

                    if (!IsShortFileFormat)
                    {
                        //Read the values for the current line
                        //From currency
                        Currencies[0] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).ToString().ToUpper();
                        //To currency
                        Currencies[1] = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).ToString().ToUpper();
                    }

                    // Perform validation on the From and To currencies at this point!!
                    if ((allCurrencies.Rows.Find(Currencies[0]) == null) && (allCurrencies.Rows.Find(Currencies[1]) == null))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: invalid currency codes ({1} and {2})"), LineNumber.ToString(), Currencies[0], Currencies[1]);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        continue;
                    }
                    else if (allCurrencies.Rows.Find(Currencies[0]) == null)
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: invalid currency code ({1})"), LineNumber.ToString(), Currencies[0]);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        Line = reader.ReadLine();
                        continue;
                    }
                    else if (allCurrencies.Rows.Find(Currencies[1]) == null)
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: invalid currency code ({1})"), LineNumber.ToString(), Currencies[1]);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        Line = reader.ReadLine();
                        continue;
                    }

                    if ((allBaseCurrencies.Contains(Currencies[0]) == false) && (allBaseCurrencies.Contains(Currencies[1]) == false))
                    {
                        //raise a non-critical error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: Warning:  One of '{1}' and '{2}' should be a base currency in one of the active ledgers."),
                            LineNumber.ToString(), Currencies[0], Currencies[1]);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Noncritical));
                    }

                    // Date parsing as in Petra 2.x instead of using XML date format!!!
                    string DateEffectiveStr = StringHelper.GetNextCSV(ref Line, ACSVSeparator, false, true).Replace("\"", String.Empty);
                    DateTime DateEffective;
                    int dateAsInt;
                    int dateLength = DateEffectiveStr.Length;

                    // Allow for 6- or 8-digit dates if the User Setting has been set
                    if (ADateMayBeAnInteger
                        && ((dateLength == 6) || (dateLength == 8)) && !DateEffectiveStr.Contains(".") && !DateEffectiveStr.Contains(","))
                    {
                        if (int.TryParse(DateEffectiveStr, out dateAsInt) && (dateAsInt > 10100) && (dateAsInt < 311300))
                        {
                            DateEffectiveStr = DateEffectiveStr.Insert(dateLength - 2, "-").Insert(dateLength - 4, "-");
                        }
                        else if (int.TryParse(DateEffectiveStr, out dateAsInt) && (dateAsInt > 1011900) && (dateAsInt < 31133000))
                        {
                            DateEffectiveStr = DateEffectiveStr.Insert(dateLength - 4, "-").Insert(dateLength - 6, "-");
                        }
                    }

                    if (!DateTime.TryParse(DateEffectiveStr, MyCultureInfoDate, DateTimeStyles.None, out DateEffective))
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: invalid date ({1})"), LineNumber.ToString(), DateEffectiveStr);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        Line = reader.ReadLine();
                        continue;
                    }

                    if (DateEffective > maxRecommendedEffectiveDate)
                    {
                        // raise a warning
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: Warning: The date '{1}' is after the latest forwarding period of any active ledger"),
                            LineNumber.ToString(), DateEffectiveStr);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Noncritical));
                    }
                    else if (DateEffective < minRecommendedEffectiveDate)
                    {
                        // raise a warning
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: Warning: The date '{1}' is before the current accounting period of any active ledger"),
                            LineNumber.ToString(), DateEffectiveStr);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Noncritical));
                    }

                    if (AImportMode == "Corporate")
                    {
                        if ((preferredPeriodStartDay >= 1) && (DateEffective.Day != preferredPeriodStartDay))
                        {
                            // raise a warning
                            string resultText = String.Format(Catalog.GetString(
                                    "Line {0}: Warning: The date '{1}' should be the first day of an accounting period used by all the active ledgers."),
                                LineNumber.ToString(), DateEffectiveStr);

                            InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Noncritical));
                        }
                    }

                    decimal ExchangeRate = 0.0m;
                    try
                    {
                        if (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN)
                        {
                            // Decimal dot: just replace thousands with nothing (comma, space and apostrophe)
                            ExchangeRate = Convert.ToDecimal(StringHelper.GetNextCSV(
                                    ref Line, ACSVSeparator, false, true).Replace(",", "").Replace(" ", "").Replace("'", ""),
                                CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            // Decimal comma: replace thousands with nothing (dot, space and apostrophe) and then comma with dot
                            ExchangeRate = Convert.ToDecimal(StringHelper.GetNextCSV(
                                    ref Line, ACSVSeparator, false, true).Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "."),
                                CultureInfo.InvariantCulture);
                        }

                        if (ExchangeRate == 0)
                        {
                            throw new EOPException(Catalog.GetString("Exchange Rate is 0!"));
                        }
                    }
                    catch (Exception)
                    {
                        // raise an error
                        string resultText = String.Format(Catalog.GetString(
                                "Line {0}: invalid rate of exchange ({1})"), LineNumber.ToString(), ExchangeRate);

                        InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                        Line = reader.ReadLine();
                        continue;
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
                                        "Line {0}: invalid effective time ({1})"), LineNumber.ToString(), t);

                                InvalidRows.Add(new Tuple <string, TResultSeverity>(resultText, TResultSeverity.Resv_Critical));
                                Line = reader.ReadLine();
                                continue;
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
                                ExchangeRow.RateOfExchange = Math.Round(1 / ExchangeRate, 10);
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
                                ExchangeRow.RateOfExchange = Math.Round(1 / ExchangeRate, 10);
                            }
                        }
                    }

                    Line = reader.ReadLine();
                }

                // if there are rows that could not be imported
                if ((InvalidRows != null) && (InvalidRows.Count > 0))
                {
                    int errorCount = 0;
                    int warningCount = 0;

                    // Go through once just to count the errors and warnings
                    foreach (Tuple <string, TResultSeverity>Row in InvalidRows)
                    {
                        if (Row.Item2 == TResultSeverity.Resv_Noncritical)
                        {
                            warningCount++;
                        }
                        else
                        {
                            errorCount++;
                        }
                    }

                    string resultText = String.Empty;
                    bool messageListIsFull = false;
                    int counter = 0;

                    if (errorCount > 0)
                    {
                        resultText = string.Format(Catalog.GetPluralString("1 row was not imported due to invalid data:",
                                "{0} rows were not imported due to invalid data:", errorCount, true), errorCount) +
                                     Environment.NewLine;
                    }

                    if (warningCount > 0)
                    {
                        resultText = string.Format(Catalog.GetPluralString("There was 1 warning associated with the imported rows:",
                                "There were {0} warnings associated with the imported rows:", warningCount, true), warningCount) +
                                     Environment.NewLine;
                    }

                    // Now go through again itemising each one
                    foreach (Tuple <string, TResultSeverity>Row in InvalidRows)
                    {
                        counter++;

                        if (counter <= MAX_MESSAGE_COUNT)
                        {
                            resultText += Environment.NewLine + Row.Item1;
                        }
                        else if (!messageListIsFull)
                        {
                            resultText += String.Format(Catalog.GetString(
                                    "{0}{0}{1} errors/warnings were reported in total.  This message contains the first {2}."),
                                Environment.NewLine, InvalidRows.Count, MAX_MESSAGE_COUNT);
                            messageListIsFull = true;
                        }
                    }

                    // additional message if one or more rows has an invalid number of columns
                    if (InvalidColumnCount && IsShortFileFormat)
                    {
                        resultText += String.Format("{0}{0}" + Catalog.GetString("Each row should contain 2 or 3 columns as follows:") + "{0}" +
                            Catalog.GetString(
                                "  1. Effective Date{0}  2. Exchange Rate{0}  3. Effective time in seconds (Optional for Daily Rate only)"),
                            Environment.NewLine);
                    }
                    else if (InvalidColumnCount && !IsShortFileFormat)
                    {
                        resultText += String.Format("{0}{0}" + Catalog.GetString("Each row should contain 4 or 5 columns as follows:") + "{0}" +
                            Catalog.GetString(
                                "    1. From Currency{0}    2. To Currency{0}    3. Effective Date{0}    4. Exchange Rate{0}    5. Effective time in seconds (Optional for Daily Rate only)"),
                            Environment.NewLine);
                    }

                    TVerificationResult result = new TVerificationResult(AImportMode,
                        resultText,
                        CommonErrorCodes.ERR_INCONGRUOUSSTRINGS,
                        (errorCount > 0) ? TResultSeverity.Resv_Critical : TResultSeverity.Resv_Noncritical);
                    AResultCollection.Add(result);
                }

                return RowsImported;
            }
        }
    }
}