//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2017 by OM International
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
using System.Globalization;

using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Common.Conversion
{
    /// <summary>
    /// Performs Date conversions that are needed both on Server and Client side.
    /// </summary>
    public class TDate
    {
        #region Resourcestrings

        private static readonly string StrMonthDayExchangedInfo = Catalog.GetString(
            "The entered Date is not valid - the month is greater than 12.\r\n" +
            "It seems you have inadvertedly exchanged month" +
            " and day. OpenPetra will exchange month and day for you to make a valid date " +
            "- please check that the corrected date is the date you intended to enter!");

        private static readonly string StrMonthDayExchangedInfoTitle = Catalog.GetString("Invalid date - OpenPetra tries correcting");

        #endregion

        /// <summary>
        /// Converts a string to a formatted date string
        /// </summary>
        /// <param name="AParseDate">String which contains the date that should be converted</param>
        /// <param name="ADescription">String about the conversion type that is performed. This is
        /// used to gather error messages.</param>
        /// <param name="AParsedDate">String that holds the parsed date (if successful)</param>
        /// <param name="AShowVerificationError">true if error message box should be shown if conversion fails</param>
        /// <returns>true if successful, otherwise false</returns>
        public static Boolean LongDateStringToDateTimeInternal(String AParseDate,
            String ADescription,
            out object AParsedDate,
            Boolean AShowVerificationError)
        {
            Boolean ReturnValue = false;
            Int32 DayOffset;
            String TmpYear;
            String TmpMonth;
            String TmpDay;
            String TmpMonthDayExchange = "";
            String TmpShortDatePattern;
            Int16 YearStart = 0;
            Int16 RestStart = 0;

            AParsedDate = null;
            DateTimeFormatInfo CurrentDateTimeFormatInfo;

            // for testing purposes only
            // string TmpDateSeparator = CurrentDateTimeFormatInfo.DateSeparator;

            try
            {
                // TODO: implement parsing of localised short month names like 4GL does (according to user's default language setting), eg. accept 'M?R' instead of 'MAR' for March if the user's language setting is DE (German)
                // MessageBox.Show('AParseDate: ' + AParseDate);
                if (TDateChecks.IsValidDateTime(AParseDate, "") != null)
                {
                    // MessageBox.Show('No regular DateTime');
                    if ((AParseDate.StartsWith("-")) || ((AParseDate.StartsWith("+")) && (AParseDate.Length != 1)))
                    {
                        // MessageBox.Show('Calculating date from the amount that follows the + or  sign...');
                        // calculate date from the amount that follows the + or  sign
                        if (TNumericalChecks.IsValidInteger(AParseDate.Substring(1), "") == null)
                        {
                            DayOffset = System.Convert.ToInt32(AParseDate.Substring(1));

                            // MessageBox.Show('DayOffset: ' + DayOffset.ToString);
                            if (AParseDate.StartsWith("+"))
                            {
                                AParseDate = DateTime.Now.Date.AddDays(DayOffset).ToString("D");
                            }
                            else
                            {
                                AParseDate = DateTime.Now.Date.Subtract(new TimeSpan(DayOffset, 0, 0, 0)).ToString("D");
                            }
                        }
                        else
                        {
                            // characters following the + or  are not an Int32
                            TLogging.Log(TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultText + " "+
                                TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultTextCaption);
                            return ReturnValue;
                        }
                    }
                    else if (((AParseDate.Length <= 6)
                              || (AParseDate.Length <= 8)) && (AParseDate.Length != 1) && (TNumericalChecks.IsValidInteger(AParseDate, "") == null))
                    {
//                        MessageBox.Show("Checking for dates entered like eg. 211105 or 21112005 ...");

                        /*
                         * Checking for dates entered like eg. 211105 or 21112005.
                         *
                         * Notes:
                         * Petra.NET accepts date entry dependent on current Culture
                         * (=settings are taken from Windows Control Panel -> Regional and
                         * Language Options).
                         * However, 4GL Petra parses dates according to the server-wide setting
                         * '-d'in startup.pf (Progress home directory). This should normally be
                         * the same than the Windows Control Panel setting on the user's
                         * machines, so there should be no deviation.
                         */
                        CurrentDateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
                        TmpShortDatePattern = CurrentDateTimeFormatInfo.ShortDatePattern.ToUpper();

                        // TmpShortDatePattern := "MM DD";      // For testing purposes only

                        if (TmpShortDatePattern.StartsWith("Y"))
                        {
                            YearStart = 0;

                            switch (AParseDate.Length)
                            {
                                case 8:
                                    RestStart = 4;
                                    break;

                                case 6:
                                    RestStart = 2;
                                    break;

                                case 4:
                                    RestStart = 0;
                                    YearStart = -1;
                                    break;
                            }
                        }
                        else
                        {
                            RestStart = 0;

                            switch (AParseDate.Length)
                            {
                                case 6:
                                case 8:
                                    YearStart = 4;
                                    break;

                                case 4:
                                    YearStart = -1;
                                    break;
                            }
                        }

//MessageBox.Show("TmpShortDatePattern: " + TmpShortDatePattern + "; TmpDateSeparator: " + TmpDateSeparator +
//    "\r\nYearStart: " + YearStart.ToString() + "; RestStart: " + RestStart.ToString());
                        if (AParseDate.Length <= 6)
                        {
                            if (YearStart != -1)
                            {
                                TmpYear = AParseDate.Substring(YearStart, 2);

                                // Determine the correct century for twodigit years.

                                /* For compatibility reasons: This is the way how it's done in 4GL, */
                                /* in sp_date.p/ConvertStringToDate */
                                if (Convert.ToInt32(TmpYear) < 80)
                                {
                                    TmpYear = "20" + TmpYear;
                                }
                                else if (Convert.ToInt32(TmpYear) < 100)
                                {
                                    TmpYear = "19" + TmpYear;
                                }

                                /*  */
                                /* This would be the Windows way of doing it... */
                                /* I (ChristianK) found no way to retrieve the correct century from */
                                /* .NET, so it's hardcoded here, taking the default values of Windows */
                                /* XP :( */
                                /*  */
                                /* if Convert.ToInt32(TmpYear) <= 29 then */
                                /* begin */
                                /* TmpYear := '20' + TmpYear; */
                                /* end */
                                /* else */
                                /* begin */
                                /* TmpYear := '19' + TmpYear; */
                                /* end; */
                            }
                            else
                            {
                                TmpYear = DateTime.Now.Year.ToString();
                            }

//MessageBox.Show("TmpYear: " + TmpYear);
                        }
                        else
                        {
                            TmpYear = AParseDate.Substring(YearStart, 4);
                        }

                        if ((AParseDate.Length == 4) || (AParseDate.Length == 6) || (AParseDate.Length == 8))
                        {
                            if (TmpShortDatePattern.IndexOf('M') < TmpShortDatePattern.IndexOf('D'))
                            {
                                TmpMonth = AParseDate.Substring(RestStart, 2);
                                TmpDay = AParseDate.Substring(RestStart + 2, 2);
                            }
                            else
                            {
                                TmpDay = AParseDate.Substring(RestStart, 2);
                                TmpMonth = AParseDate.Substring(RestStart + 2, 2);
                            }
                        }
                        else
                        {
                            // format with other number of digits not supported
                            TLogging.Log(TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultText + " " +
                                TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultTextCaption);

                            return ReturnValue;
                        }

                        if (Convert.ToInt16(TmpMonth) > 12)
                        {
                            TmpMonthDayExchange = TmpMonth;
                            TmpMonth = TmpDay;
                            TmpDay = TmpMonthDayExchange;
                        }

                        // AParseDate := TmpYear + TmpDateSeparator + TmpMonth + TmpDateSeparator + TmpDay;    For testing purposes
                        // MessageBox.Show('AParseDate (1): ' + AParseDate);    For testing purposes
                        try
                        {
                            AParseDate = new DateTime(Convert.ToInt32(TmpYear), Convert.ToInt32(TmpMonth), Convert.ToInt32(TmpDay)).ToString("D");

                            // TmpMonth + '/' + TmpDay + '/' + TmpYear;

                            if (TmpMonthDayExchange != "")
                            {
                                TLogging.Log(StrMonthDayExchangedInfo + " " + StrMonthDayExchangedInfoTitle);
                            }
                        }
                        catch (Exception)
                        {
                            TLogging.Log(TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultText + " " +
                                TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultTextCaption);
                            return ReturnValue;
                        }

//MessageBox.Show("TmpShortDatePattern: " + TmpShortDatePattern + "; TmpDateSeparator: " + TmpDateSeparator +
//                                        "\r\nYearStart: " + YearStart.ToString() + "; RestStart: " + RestStart.ToString() +
//"; TmpDay: " + TmpDay + "; TmpMonth: " + TmpMonth + "; TmpYear: " + TmpYear + "\r\nAParseDate: " + AParseDate);
                        AParsedDate = AParseDate;
                        ReturnValue = true;
                        return ReturnValue;
                    }
                    else if (AParseDate == "")
                    {
//MessageBox.Show("Value = \"");
                        AParsedDate = DBNull.Value;
                        ReturnValue = true;
                        return ReturnValue;
                    }
                    else if ((AParseDate == "=") || (AParseDate == "+") || (AParseDate.ToLower() == "today"))
                    {
                        AParsedDate = DateTime.Now.ToString("D");
                        ReturnValue = true;
                        return ReturnValue;
                    }
                    else
                    {
                        if (AShowVerificationError)
                        {
                            // not an accepted date parse string
                            TLogging.Log(TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultText + " " +
                                TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultTextCaption);
                        }

                        return ReturnValue;
                    }
                }

                // AParseDate ready to be parsed
                AParsedDate = DateTime.Parse(AParseDate).ToString("D");
                ReturnValue = true;
            }
            catch (Exception)
            {
                TLogging.Log(TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultText + " " +
                    TDateChecks.GetInvalidDateVerificationResult(ADescription).ResultTextCaption);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts a string to a formatted date string.
        /// </summary>
        /// <param name="AParseDate">String which contains the date that should be converted</param>
        /// <param name="ADescription">String about the conversion type that is performed. This is
        /// used to gather error messages.</param>
        /// <param name="AParsedDate">String that holds the parsed date (if successful)</param>
        /// <returns>true if successful, otherwise false</returns>
        public static Boolean LongDateStringToDateTimeInternal(String AParseDate, String ADescription, out object AParsedDate)
        {
            return LongDateStringToDateTimeInternal(AParseDate, ADescription, out AParsedDate, true);
        }

        /// <summary>
        /// Converts a String into a date time.
        /// </summary>
        /// <param name="AParseDate">String which contains the date that should be converted</param>
        /// <param name="ADescription">String about the conversion type that is performed. This is
        /// used to gather error messages.</param>
        /// <param name="AVerificationResult">If conversion fails it contains detailed information
        /// about the error. If conversion is successful: null</param>
        /// <param name="AShowVerificationError">True if a error message should be shown if conversion fails.</param>
        /// <returns>The converted date. If the conversion didn't succeed than it contains
        /// the Date Min value.</returns>
        public static DateTime LongDateStringToDateTime2(String AParseDate,
            String ADescription,
            out TVerificationResult AVerificationResult,
            Boolean AShowVerificationError)
        {
            DateTime ReturnValue;

            // DateConvertEventArgs: ConvertEventArgs;
            object ResultObj;

            ResultObj = null;
            AVerificationResult = null;

            /* Result := DateTime.MinValue; */
            /*  */
            /* Convert TextBox's Text to Date */
            /* DateConvertEventArgs := new ConvertEventArgs(AParseDate, System.Type.GetType('System.DateTime')); */
            /* LongDateStringToDateTime(nil, DateConvertEventArgs); */
            /*  */
            /* AVerificationResult := TDateChecks.IsValidDateTime( */
            /* DateConvertEventArgs.Value.ToString, ''); */
            /*  */
            /* if AVerificationResult = nil then */
            /* begin */
            /* Conversion was successful > return the Date */
            /* Result := Convert.ToDateTime(DateConvertEventArgs.Value); */
            /* end; */
            if (LongDateStringToDateTimeInternal(AParseDate, ADescription, out ResultObj, AShowVerificationError))
            {
                // MessageBox.Show('LongDateStringToDateTime2: date is valid: ' + ResultObj.ToString);
                if (ResultObj != DBNull.Value)
                {
                    ReturnValue = Convert.ToDateTime(ResultObj);
                }
                else
                {
                    ReturnValue = DateTime.MinValue;
                }
            }
            else
            {
                // MessageBox.Show('LongDateStringToDateTime2: date is INvalid!');
                AVerificationResult = TDateChecks.IsValidDateTime(AParseDate, ADescription);
                ReturnValue = DateTime.MinValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts a a date time into a string.
        /// </summary>
        /// <param name="ADateTime">Date that should be converted.</param>
        /// <returns>The converted date.</returns>
        public static String DateTimeToLongDateString2(DateTime ADateTime)
        {
            // DateTime is a non-nullable value type, therefore encode an invalid date with DateTime.MinValue
            if (ADateTime == DateTime.MinValue)
            {
                return "";
            }

            return StringHelper.DateToLocalizedString(ADateTime);
        }
    }
}
