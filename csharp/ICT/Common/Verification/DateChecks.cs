//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Common.Verification;
using GNU.Gettext;
using System.Windows.Forms;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Type of Date-between Checks.
    /// </summary>
    public enum TDateBetweenDatesCheckType
    {
        /// <summary>Unspecific check.</summary>
        dbdctUnspecific,

        /// <summary>Value must not be a future date.</summary>
        dbdctNoFutureDate,

        /// <summary>Value must not be a past date.</summary>
        dbdctNoPastDate,

        /// <summary>Value represents and unrealistic date in that circumstance (e.g. birth date below 1850).</summary>
        dbdctUnrealisticDate
    }

    /// <summary>
    /// Class for date verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TDateChecks
    {
        #region Resourcestrings

        private static readonly string StrDateMustNotBeEmpty = Catalog.GetString("{0} must not be empty.");
        private static readonly string StrDateMustNotBePastDate = Catalog.GetString("{0} must not be a past date.");
        private static readonly string StrDateMustNotBeFutureDate = Catalog.GetString("{0} must not be a future date.");
        private static readonly string StrDateMustNotBeLaterThanFirstDayOfMonth = Catalog.GetString(
            "{0} must not be later than the first day of the month.");
        private static readonly string StrDateCannotBeLater = Catalog.GetString("{0} cannot be later then {1}.");
        private static readonly string StrDateCannotBeLaterOrEqual = Catalog.GetString("{0} cannot be later than or equal to {1}.");
        private static readonly string StrDateCannotBeEarlier = Catalog.GetString("{0} cannot be earlier than {1}.");
        private static readonly string StrDateCannotBeEarlierOrEqual = Catalog.GetString("{0} cannot be earlier than or equal to {1}.");
        private static readonly string StrMustBeDate = Catalog.GetString("{0} must be a date.");
        private static readonly string StrDateNotInRange = Catalog.GetString(
            "{0} is not allowed as it does not lie within the required date range. It must lie between {1} and {2}.");
        private static readonly string StrDateNotSensible = Catalog.GetString("{0} is not a sensible value in this case.");

        #endregion

        #region IsNotUndefinedDateTime

        /// <summary>
        /// Checks whether the date is not undefined. DateTime.MinValue is seen as undefined by this Method.
        /// Null values are accepted. They are treated as valid, unless <paramref name="ATreatNullAsInvalid" /> is
        /// set to true.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="ATreatNullAsInvalid">Set this to true to treated null value in <paramref name="ADate" /> as invalid.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <remarks>Usage in the Data Validation Framework: rather than using this Method, use Method
        /// 'TSharedValidationControlHelper.IsNotInvalidDate' for checking the validity of dates as the latter can deal not only with
        /// empty dates, but dates that are invalid in other respects (e.g. exceeding a valid date range)!!!</remarks>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem.</returns>
        public static TVerificationResult IsNotUndefinedDateTime(DateTime? ADate, String ADescription,
            bool ATreatNullAsInvalid = false, object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);
            String Description = THelper.NiceValueDescription(ADescription);

            if (!ADate.HasValue)
            {
                if (!ATreatNullAsInvalid)
                {
                    return null;
                }
                else
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOUNDEFINEDDATE,
                            CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateMustNotBeEmpty, new string[] { Description }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
            }

            // Check
            if (TheDate != DateTime.MinValue)
            {
                //MessageBox.Show('Date <> DateTime.MinValue');
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOUNDEFINEDDATE,
                        CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                        StrDateMustNotBeEmpty, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the date is not undefined. DateTime.MinValue is seen as undefined by this Method.
        /// Null values are accepted.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <remarks>Usage in the Data Validation Framework: rather than using this Method, use Method
        /// 'TSharedValidationControlHelper.IsNotInvalidDate' for checking the validity of dates as the latter can deal not only with
        /// empty dates, but dates that are invalid in other respects (e.g. exceeding a valid date range)!!!</remarks>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem.</returns>
        public static TVerificationResult IsNotCorporateDateTime(DateTime? ADate, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);
            DateTime FirstOfMonth;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!ADate.HasValue)
            {
                return null;
            }

            FirstOfMonth = new DateTime(TheDate.Year, TheDate.Month, 1);

            // Checks
            if (TheDate == FirstOfMonth)
            {
                //MessageBox.Show('Date <> DateTime.MinValue');
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE,
                        CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                        StrDateMustNotBeLaterThanFirstDayOfMonth, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsValidDateTime

        /// <summary>
        /// Checks whether the string <paramref name="AString" /> contains a valid DateTime.
        /// </summary>
        /// <param name="AString">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AString" /> contains a valid DateTime, otherwise a
        /// <see cref="TVerificationResult" /> with a message which uses
        /// <paramref name="ADescription" /> is returned.</returns>
        public static TVerificationResult IsValidDateTime(String AString, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            DateTime temp;

            if (!DateTime.TryParse(AString, out temp))
            {
                ReturnValue = GetInvalidDateVerificationResult(Description, AResultContext);

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region IsCurrentOr...Date

        #region IsCurrentOrFutureDate

        /// <summary>
        /// Checks whether the date is today or in the future. Null values are accepted.
        /// </summary>
        /// <param name="ADate">Date to check.</param>
        /// <param name="ADescription">Name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the future,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrFutureDate(DateTime? ADate, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);
            String Description = THelper.NiceValueDescription(ADescription);

            if (!ADate.HasValue)
            {
                return null;
            }

            // Check
            if (TheDate >= DateTime.Today)
            {
                //MessageBox.Show('Date >= Today');
                ReturnValue = null;
            }
            else
            {
                if (TheDate != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOPASTDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateMustNotBePastDate, new string[] { Description }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsCurrentOrPastDate

        /// <summary>
        /// Checks whether the date is today or in the past. Null values are accepted.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the past,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrPastDate(DateTime? ADate, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!ADate.HasValue)
            {
                return null;
            }

            // Check
            if (ADate <= DateTime.Today)
            {
                //MessageBox.Show('Date <= Today');
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOFUTUREDATE, CommonResourcestrings.StrInvalidDateEntered +
                        Environment.NewLine + StrDateMustNotBeFutureDate, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #endregion


        #region IsDateBetweenDates

        /// <summary>
        /// Checks whether the date is within a specified date range. Null values are accepted.
        /// </summary>
        /// <remarks>This Method is capable of returning varying<see cref="TVerificationResult" /> objects! The objects
        /// returned are determined by the values specified for <paramref name="ALowerRangeCheckType" /> and
        ///  <paramref name="AUpperRangeCheckType" />!</remarks>
        /// <param name="ADate">Date to check.</param>
        /// <param name="ALowerDateRangeEnd">Lower end of the valid Date Range.</param>
        /// <param name="AUpperDateRangeEnd">Upper end of the valid Date Range.</param>
        /// <param name="ADescription">Name of the date value.</param>
        /// <param name="ALowerRangeCheckType">Type of Date Check: lower end of the valid Date Range (defaults to <see cref="TDateBetweenDatesCheckType.dbdctUnspecific" />).</param>
        /// <param name="AUpperRangeCheckType">Type of Date Check: upper end of the valid Date Range (defaults to <see cref="TDateBetweenDatesCheckType.dbdctUnspecific" />).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if the date <paramref name="ADate" /> is between the lower and the upper end of the Date Range specified
        /// (lower and upper end dates are included), otherwise a verification result with a message that uses
        /// <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsDateBetweenDates(DateTime? ADate, DateTime? ALowerDateRangeEnd, DateTime? AUpperDateRangeEnd,
            String ADescription,
            TDateBetweenDatesCheckType ALowerRangeCheckType = TDateBetweenDatesCheckType.dbdctUnspecific,
            TDateBetweenDatesCheckType AUpperRangeCheckType = TDateBetweenDatesCheckType.dbdctUnspecific,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);
            DateTime LowerDateRangeEndDate = TSaveConvert.ObjectToDate(ALowerDateRangeEnd);
            DateTime UpperDateRangeEndDate = TSaveConvert.ObjectToDate(AUpperDateRangeEnd);
            String Description = THelper.NiceValueDescription(ADescription);

            if ((!ADate.HasValue)
                || (!ALowerDateRangeEnd.HasValue)
                || (!AUpperDateRangeEnd.HasValue))
            {
                return null;
            }

            // Check
            if ((TheDate < LowerDateRangeEndDate)
                || (TheDate > UpperDateRangeEndDate))
            {
                if ((ALowerRangeCheckType == TDateBetweenDatesCheckType.dbdctUnspecific)
                    && (AUpperRangeCheckType == TDateBetweenDatesCheckType.dbdctUnspecific))
                {
                    ReturnValue = GetUnspecificDateRangeCheckVerificationResult(LowerDateRangeEndDate,
                        UpperDateRangeEndDate,
                        Description,
                        AResultContext);
                }
                else if (TheDate < LowerDateRangeEndDate)
                {
                    if (ALowerRangeCheckType == TDateBetweenDatesCheckType.dbdctNoPastDate)
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOPASTDATE, CommonResourcestrings.StrInvalidDateEntered +
                                Environment.NewLine +
                                StrDateMustNotBePastDate, new string[] { Description }));
                    }
                    else if (ALowerRangeCheckType == TDateBetweenDatesCheckType.dbdctUnrealisticDate)
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_UNREALISTICDATE_ERROR, CommonResourcestrings.StrInvalidDateEntered +
                                Environment.NewLine +
                                StrDateNotSensible, new string[] { Description }));
                    }
                    else
                    {
                        ReturnValue = GetUnspecificDateRangeCheckVerificationResult(LowerDateRangeEndDate,
                            UpperDateRangeEndDate,
                            Description,
                            AResultContext);
                    }
                }
                else if (TheDate > UpperDateRangeEndDate)
                {
                    if (AUpperRangeCheckType == TDateBetweenDatesCheckType.dbdctNoFutureDate)
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOFUTUREDATE, CommonResourcestrings.StrInvalidDateEntered +
                                Environment.NewLine +
                                StrDateMustNotBeFutureDate, new string[] { Description }));
                    }
                    else if (AUpperRangeCheckType == TDateBetweenDatesCheckType.dbdctUnrealisticDate)
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_UNREALISTICDATE_ERROR, CommonResourcestrings.StrInvalidDateEntered +
                                Environment.NewLine +
                                StrDateNotSensible, new string[] { Description }));
                    }
                    else
                    {
                        ReturnValue = GetUnspecificDateRangeCheckVerificationResult(LowerDateRangeEndDate,
                            UpperDateRangeEndDate,
                            Description,
                            AResultContext);
                    }
                }

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }
            else
            {
                ReturnValue = null;
            }

            return ReturnValue;
        }

        #endregion

        #region FirstLesser...ThanSecondDate

        /// <summary>
        /// Checks whether the first submitted date is earlier or equal than the second
        /// submitted date. Null dates are accepted.
        /// </summary>
        /// <param name="ADate1">First date.</param>
        /// <param name="ADate2">Second date.</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message).</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            String FirstDateDescription = THelper.NiceValueDescription(AFirstDateDescription);
            String SecondDateDescription = THelper.NiceValueDescription(ASecondDateDescription);

            if ((!ADate1.HasValue) || (!ADate2.HasValue))
            {
                return null;
            }

            // Check
            if (ADate1 <= ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate2 != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeLater, new string[] { FirstDateDescription, SecondDateDescription }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted date is earlier than the second submitted
        /// date. Null dates are accepted.
        /// </summary>
        /// <param name="ADate1">First date.</param>
        /// <param name="ADate2">Second date</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message).</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            String FirstDateDescription = THelper.NiceValueDescription(AFirstDateDescription);
            String SecondDateDescription = THelper.NiceValueDescription(ASecondDateDescription);

            if ((!ADate1.HasValue) || (!ADate2.HasValue))
            {
                return null;
            }

            // Check
            if (ADate1 < ADate2)
            {
                //MessageBox.Show('ADate1 < ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate2 != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeLaterOrEqual, new string[] { FirstDateDescription, SecondDateDescription }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        #endregion


        #region FirstGreater...ThanSecondDate

        #region FirstGreaterOrEqualThanSecondDate

        /// <summary>
        /// Checks whether the first submitted date is later or equal than the second
        /// submitted date. Null dates are accepted.
        /// </summary>
        /// <param name="ADate1">First date.</param>
        /// <param name="ADate2">Second date.</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message).</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            String FirstDateDescription = THelper.NiceValueDescription(AFirstDateDescription);
            String SecondDateDescription = THelper.NiceValueDescription(ASecondDateDescription);

            if ((!ADate1.HasValue) || (!ADate2.HasValue))
            {
                return null;
            }

            // Check
            if (ADate1 >= ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate1 != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeEarlier, new string[] { FirstDateDescription, SecondDateDescription }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        #endregion

        #region FirstGreaterThanSecondDate

        /// <summary>
        /// Checks whether the first submitted date is later than the second submitted
        /// date. Null dates are accepted.
        /// </summary>
        /// <param name="ADate1">First date.</param>
        /// <param name="ADate2">Second date.</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message).</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;
            String FirstDateDescription = THelper.NiceValueDescription(AFirstDateDescription);
            String SecondDateDescription = THelper.NiceValueDescription(ASecondDateDescription);

            if ((!ADate1.HasValue) || (!ADate2.HasValue))
            {
                return null;
            }

            // Check
            if (ADate1 > ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate1 != new DateTime(0001, 1, 1))
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeEarlierOrEqual, new string[] { FirstDateDescription, SecondDateDescription }));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        #endregion

        #endregion


        #region Helper Methods

        /// <summary>
        /// This is called in case a date is invalid, in order to generate a generic
        /// <see cref="TVerificationResult" />.
        /// </summary>
        /// <param name="ADescription">Either a name for the date value or an empty string.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <returns>A Verification Result with the error message.</returns>
        public static TVerificationResult GetInvalidDateVerificationResult(String ADescription, object AResultContext = null)
        {
            String Description = THelper.NiceValueDescription(ADescription);

            return new TVerificationResult(AResultContext,
                ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                    StrMustBeDate, new string[] { Description }));
        }

        /// <summary>
        /// This is called in case a date range check fails, in order to generate a 'unspecific' (in this context!)
        /// <see cref="TVerificationResult" />.
        /// </summary>
        /// <param name="ALowerDateRangeEnd">Lower end of the valid Date Range.</param>
        /// <param name="AUpperDateRangeEnd">Upper end of the valid Date Range.</param>
        /// <param name="ADescription">Name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <returns>A Verification Result with the error message.</returns>
        private static TVerificationResult GetUnspecificDateRangeCheckVerificationResult(
            DateTime? ALowerDateRangeEnd, DateTime? AUpperDateRangeEnd, String ADescription, object AResultContext)
        {
            return new TVerificationResult(AResultContext,
                ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_DATENOTINDATERANGE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                    StrDateNotInRange,
                    new string[] { ADescription,
                                   StringHelper.DateToLocalizedString(ALowerDateRangeEnd.Value),
                                   StringHelper.DateToLocalizedString(AUpperDateRangeEnd.Value) }));
        }

        #endregion
    }

    /// <summary>
    /// Class for time verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TTimeChecks
    {
        #region Resourcestrings

        private static readonly string StrMustBeTime = Catalog.GetString("{0} must be a valid time.");

        #endregion

        #region IsValidIntegerTime

        /// <summary>
        /// Checks whether an integer time is in the range 0..86399
        /// </summary>
        /// <param name="AValue">Integer number.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsValidIntegerTime(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if ((AValue.Value < 0) || (AValue.Value >= 86400))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDINTEGERTIME, CommonResourcestrings.StrInvalidStringEntered +
                        Environment.NewLine +
                        StrMustBeTime, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion
    }
}