//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance.Account.Data;


namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Helper class for parsing import lines in an import file
    /// </summary>
    public static class TCommonImport
    {
        /// <summary>
        /// Imports a string value from the specified text line using the specified delimiter
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ATreatEmptyStringAsText">When true the return value will be the empty string. When false the return value will be null.</param>
        /// <returns>The string value.  The AImportLine parameter will have been clipped.</returns>
        public static String ImportString(ref String AImportLine,
            String ADelimiter,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            bool ATreatEmptyStringAsText = true)
        {
            String sReturn = StringHelper.GetNextCSV(ref AImportLine, ADelimiter);

            if ((sReturn == StringHelper.CSV_STRING_FORMAT_ERROR) && (AMessages != null))
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                        Catalog.GetString("Could not parse the quoted string. Did you forget a quotation mark?"),
                        TResultSeverity.Resv_Critical));
            }

            if ((sReturn.Length == 0) && !ATreatEmptyStringAsText)
            {
                return null;
            }

            return sReturn;
        }

        /// <summary>
        /// Imports a boolean value from the specified text line using the specified delimiter.
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ADefaultString">A string to apply if the import returns empty text.  Must be either 'yes' or 'no'</param>
        /// <returns>Returns true if the text is 'yes', false if the text is 'no'. Otherwise the method returns a critical Verification Result.</returns>
        public static Boolean ImportBoolean(ref String AImportLine,
            String ADelimiter,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            String ADefaultString = "")
        {
            String sReturn = StringHelper.GetNextCSV(ref AImportLine, ADelimiter).ToLower();
            String sDefault = ADefaultString.ToLower();

            bool canBeEmptyString = ((sDefault == "yes") || (sDefault == "no"));

            if ((sReturn == String.Empty) && canBeEmptyString)
            {
                sReturn = sDefault;
            }

            if ((sReturn == "yes") || (sReturn == "no"))
            {
                return sReturn.Equals("yes");
            }

            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                    String.Format(Catalog.GetString("Cannot convert '{0}' to a Boolean. The text must be {1}. The text is not case-sensitive."),
                        sReturn,
                        canBeEmptyString ? Catalog.GetString("one of 'yes', 'no' or an empty string") : Catalog.GetString("either 'yes' or 'no'")),
                    TResultSeverity.Resv_Critical));
            return false;
        }

        /// <summary>
        /// Imports an Int64 value from the specified text line using the specified delimiter
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ADefaultString"></param>
        /// <returns>The value.  The AImportLine parameter will have been clipped.</returns>
        public static Int64 ImportInt64(ref String AImportLine,
            String ADelimiter,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            String ADefaultString = "")
        {
            String sReturn = StringHelper.GetNextCSV(ref AImportLine, ADelimiter);

            if (sReturn == String.Empty)
            {
                sReturn = ADefaultString;
            }

            Int64 retVal;

            if (Int64.TryParse(sReturn, out retVal))
            {
                return retVal;
            }

            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                    String.Format(Catalog.GetString("Cannot convert '{0}' to an integer number."), sReturn),
                    TResultSeverity.Resv_Critical));
            return 1;
        }

        /// <summary>
        /// Imports an Int32 value from the specified text line using the specified delimiter
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ADefaultString"></param>
        /// <returns>The value.  The AImportLine parameter will have been clipped.</returns>
        public static Int32 ImportInt32(ref String AImportLine,
            String ADelimiter,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            String ADefaultString = "")
        {
            String sReturn = StringHelper.GetNextCSV(ref AImportLine, ADelimiter);

            if (sReturn == String.Empty)
            {
                sReturn = ADefaultString;
            }

            Int32 retVal;

            if (Int32.TryParse(sReturn, out retVal))
            {
                return retVal;
            }

            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                    String.Format(Catalog.GetString("Cannot convert '{0}' to an integer number.."), sReturn),
                    TResultSeverity.Resv_Critical));
            return 1;
        }

        /// <summary>
        /// Imports a decimal value from the specified text line using the specified delimiter and culture info
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="ACultureInfoNumberFormat"></param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ADefaultString"></param>
        /// <returns>The value.  The AImportLine parameter will have been clipped.</returns>
        public static decimal ImportDecimal(ref String AImportLine,
            String ADelimiter,
            CultureInfo ACultureInfoNumberFormat,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            String ADefaultString = "")
        {
            String sReturn = StringHelper.GetNextCSV(ref AImportLine, ADelimiter);

            if (sReturn == String.Empty)
            {
                sReturn = ADefaultString;
            }

            try
            {
                // Always use the invariant culture
                if (ACultureInfoNumberFormat.NumberFormat.NumberDecimalSeparator == ".")
                {
                    // Decimal dot: just replace thousands with nothing (comma, space and apostrophe)
                    return Convert.ToDecimal(sReturn.Replace(",", "").Replace(" ", "").Replace("'", ""), CultureInfo.InvariantCulture);
                }
                else
                {
                    // Decimal comma: replace thousands with nothing (dot, space and apostrophe) and then comma with dot
                    return Convert.ToDecimal(sReturn.Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",",
                            "."), CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                        String.Format(Catalog.GetString("Cannot convert '{0}' to a decimal number."), sReturn),
                        TResultSeverity.Resv_Critical));
                return 1.0m;
            }
        }

        /// <summary>
        /// Imports a Date value from the specified text line using the specified delimiter
        /// </summary>
        /// <param name="AImportLine">The line containing the text to be imported.  When the method returns the imported value
        /// will have been removed from the start ready for the next call to an Import method.</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="ACultureInfoDateFormat"></param>
        /// <param name="ADateMayBeAnInteger"></param>
        /// <param name="AColumnTitle"></param>
        /// <param name="ADataColumn"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ADefaultString"></param>
        /// <returns>The date value.  The AImportLine parameter will have been clipped.</returns>
        public static DateTime ImportDate(ref String AImportLine,
            String ADelimiter,
            CultureInfo ACultureInfoDateFormat,
            bool ADateMayBeAnInteger,
            String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            String ADefaultString = "")
        {
            String sDate = StringHelper.GetNextCSV(ref AImportLine, ADelimiter);

            int dateAsInt;
            int dateLength = sDate.Length;

            if (sDate == String.Empty)
            {
                sDate = ADefaultString;
            }
            else if (ADateMayBeAnInteger && ((dateLength == 6) || (dateLength == 8)) && !sDate.Contains(".") && !sDate.Contains(","))
            {
                if (int.TryParse(sDate, out dateAsInt) && (dateAsInt > 10100) && (dateAsInt < 311300))
                {
                    sDate = sDate.Insert(dateLength - 2, "-").Insert(dateLength - 4, "-");
                }
                else if (int.TryParse(sDate, out dateAsInt) && (dateAsInt > 1011900) && (dateAsInt < 31133000))
                {
                    sDate = sDate.Insert(dateLength - 4, "-").Insert(dateLength - 6, "-");
                }
            }

            DateTime dtReturn;

            try
            {
                dtReturn = Convert.ToDateTime(sDate, ACultureInfoDateFormat);
            }
            catch (Exception)
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                        String.Format(Catalog.GetString("Cannot convert '{0}' to a date."), sDate),
                        TResultSeverity.Resv_Critical));
                TLogging.Log("Problem parsing " + sDate + " with format " + ACultureInfoDateFormat.DateTimeFormat.ShortDatePattern);
                return DateTime.Today;
            }

            return dtReturn;
        }

        /// <summary>
        /// Method to check and, if possible fix, codes that have had leading zeros removed by Excel for example.
        /// </summary>
        /// <param name="ALedgerNumber">The ledger number</param>
        /// <param name="ARowNumber">The current row number</param>
        /// <param name="AAccountCode">The account code that may get changed</param>
        /// <param name="AAccountTableRef">The account table that will be checked for valid codes</param>
        /// <param name="ACostCentreCode">The cost centre code that may get changed</param>
        /// <param name="ACostCentreTableRef">The cost centre table that will be checked for valid codes</param>
        /// <param name="AMessages">A message collection.  If a change is made a 'Information' (non-critical) message will be added to the collection</param>
        public static void FixAccountCodes(int ALedgerNumber, int ARowNumber, ref string AAccountCode, AAccountTable AAccountTableRef,
            ref string ACostCentreCode, ACostCentreTable ACostCentreTableRef, TVerificationResultCollection AMessages)
        {
            // Start with the Account code
            string code = ConvertTo4DigitCode(AAccountCode);

            if (code != AAccountCode)
            {
                // Maybe it is wrong?
                if (AAccountTableRef.Rows.Find(new object[] { ALedgerNumber, AAccountCode }) == null)
                {
                    // That one does not exist, so try our new 4 digit one
                    if (AAccountTableRef.Rows.Find(new object[] { ALedgerNumber, code }) != null)
                    {
                        // Swap the short code for the longer one
                        AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                String.Format(Catalog.GetString(
                                        "The account code '{0}' does not exist.  The code has been converted to '{1}' (which does exist).  Please check that this is what you intended."),
                                    AAccountCode, code),
                                TResultSeverity.Resv_Noncritical));
                        AAccountCode = code;
                    }
                }
            }

            // Do the same for the cost centre code
            code = ConvertTo4DigitCode(ACostCentreCode);

            if (code != ACostCentreCode)
            {
                // Maybe it is wrong?
                if (ACostCentreTableRef.Rows.Find(new object[] { ALedgerNumber, ACostCentreCode }) == null)
                {
                    // That one does not exist, so try our new 4 digit one
                    if (ACostCentreTableRef.Rows.Find(new object[] { ALedgerNumber, code }) != null)
                    {
                        // Swap the short code for the longer one
                        AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                String.Format(Catalog.GetString(
                                        "The cost centre code '{0}' does not exist.  The code has been converted to '{1}' (which does exist).  Please check that this is what you intended."),
                                    ACostCentreCode, code),
                                TResultSeverity.Resv_Noncritical));
                        ACostCentreCode = code;
                    }
                }
            }
        }

        /// <summary>
        /// Pads an account or cost centre code with leading zeros if the numeric part contains fewer than 4 digits
        /// </summary>
        /// <param name="ACode">The code to pad, if required</param>
        /// <returns>A string which may be the same as the original or may have been padded with leading zeros
        /// if the numeric portion at the beginning is not 4 digits long</returns>
        private static string ConvertTo4DigitCode(string ACode)
        {
            // Do nothing if the first character is not a digit
            if ((ACode.Length == 0) || (ACode[0] < '0') || (ACode[0] > '9'))
            {
                return ACode;
            }

            int digits = 1;

            for (int i = 1; i <= ACode.Length; i++)
            {
                if ((i == ACode.Length) || (ACode[i] < '0') || (ACode[i] > '9'))
                {
                    digits = i;
                    break;
                }
            }

            if (digits < 4)
            {
                return new String('0', 4 - digits) + ACode;
            }

            return ACode;
        }
    }
}
