//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
    /// Class for date verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TDateChecks
    {
        #region Resourcestrings

        private static readonly string StrDateMayNotBeEmpty = Catalog.GetString("'{0}' may not be empty.");
        private static readonly string StrDateMayNotBePastDate = Catalog.GetString("'{0}' may not be a past date.");
        private static readonly string StrDateMayNotBeFutureDate = Catalog.GetString("'{0}' may not be a future date.");
        private static readonly string StrDateCannotBeLater = Catalog.GetString("'{0}' cannot be later then '{1}'.");
        private static readonly string StrDateCannotBeLaterOrEqual = Catalog.GetString("'{0}' cannot be later than or equal to '{1}'.");
        private static readonly string StrDateCannotBeEarlier = Catalog.GetString("'{0}' cannot be earlier than '{1}'.");
        private static readonly string StrDateCannotBeEarlierOrEqual = Catalog.GetString("'{0}' cannot be earlier than or equal to '{1}'.");
        private static readonly string StrMustBeDate = Catalog.GetString("{0} must be a date.");

        #endregion

        #region IsNotUndefinedDateTime

        /// <summary>
        /// Checks whether the date is not undefined. DateTime.MinValue is seen as undefined by this Method.
        /// Null values are accepted.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem.</returns>
        public static TVerificationResult IsNotUndefinedDateTime(DateTime? ADate, String ADescription)
        {
            return IsNotUndefinedDateTime(ADate, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether the date is not undefined. DateTime.MinValue is seen as undefined by this Method.
        /// Null values are accepted.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem.</returns>
        public static TVerificationResult IsNotUndefinedDateTime(DateTime? ADate, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);

            if (!ADate.HasValue)
            {
                return null;
            }

            // Check
            if (TheDate != DateTime.MinValue)
            {
                //MessageBox.Show('Date <> DateTime.MinValue');
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOUNDEFINEDDATE,
                        CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                        StrDateMayNotBeEmpty, ADescription));

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
        /// <returns>Null if <paramref name="AString" /> contains a valid DateTime, otherwise a
        /// <see cref="TVerificationResult" /> with a message which uses
        /// <paramref name="ADescription" /> is returned.</returns>
        public static TVerificationResult IsValidDateTime(String AString, String ADescription)
        {
            return IsValidDateTime(AString, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether the string <paramref name="AString" /> contains a valid DateTime.
        /// </summary>
        /// <param name="AString">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AString" /> contains a valid DateTime, otherwise a
        /// <see cref="TVerificationResult" /> with a message which uses
        /// <paramref name="ADescription" /> is returned.</returns>
        public static TVerificationResult IsValidDateTime(String AString, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            try
            {
                System.Convert.ToDateTime(AString);
            }
            catch (System.Exception)
            {
                ReturnValue = GetInvalidDateVerificationResult(ADescription);

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
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the future,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrFutureDate(DateTime? ADate, String ADescription)
        {
            return IsCurrentOrFutureDate(ADate, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether the date is today or in the future. Null values are accepted.
        /// </summary>
        /// <param name="ADate">Date to check.</param>
        /// <param name="ADescription">Name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the future,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrFutureDate(DateTime? ADate, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);

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
                    ReturnValue = new TVerificationResult(String.Empty,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOPASTDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateMayNotBePastDate, ADescription));

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
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the past,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrPastDate(DateTime? ADate, String ADescription)
        {
            return IsCurrentOrPastDate(ADate, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether the date is today or in the past. Null values are accepted.
        /// </summary>
        /// <param name="ADate">The date to check.</param>
        /// <param name="ADescription">The name of the date value.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if the date <paramref name="ADate" /> is today or in the past,
        /// otherwise a verification result with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsCurrentOrPastDate(DateTime? ADate, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;

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
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOFUTUREDATE, CommonResourcestrings.StrInvalidDateEntered +
                        Environment.NewLine + StrDateMayNotBeFutureDate, ADescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

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
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription)
        {
            return FirstLesserOrEqualThanSecondDate(ADate1, ADate2, AFirstDateDescription, ASecondDateDescription,
                null, null, null);
        }

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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;

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
                    ReturnValue = new TVerificationResult(String.Empty,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeLater, AFirstDateDescription, ASecondDateDescription));

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
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription)
        {
            return FirstLesserThanSecondDate(ADate1, ADate2, AFirstDateDescription, ASecondDateDescription,
                null, null, null);
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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;

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
                    ReturnValue = new TVerificationResult(String.Empty,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeLaterOrEqual, AFirstDateDescription, ASecondDateDescription));

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
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2,
            String AFirstDateDescription,
            String ASecondDateDescription)
        {
            return FirstGreaterOrEqualThanSecondDate(ADate1, ADate2, AFirstDateDescription, ASecondDateDescription,
                null, null, null);
        }

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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterOrEqualThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;

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
                    ReturnValue = new TVerificationResult(String.Empty,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeEarlier, AFirstDateDescription, ASecondDateDescription));

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
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription)
        {
            return FirstGreaterThanSecondDate(ADate1, ADate2, AFirstDateDescription, ASecondDateDescription,
                null, null, null);
        }

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
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstDateDescription" /> and <paramref name="ASecondDateDescription" />.</returns>
        public static TVerificationResult FirstGreaterThanSecondDate(DateTime? ADate1,
            DateTime? ADate2, string AFirstDateDescription, string ASecondDateDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue;

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
                    ReturnValue = new TVerificationResult(String.Empty,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            StrDateCannotBeEarlierOrEqual, AFirstDateDescription, ASecondDateDescription));

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
        /// <returns>A Verification Result with the error message.</returns>
        public static TVerificationResult GetInvalidDateVerificationResult(String ADescription)
        {
            String Description = THelper.NiceValueDescription(ADescription);

            return new TVerificationResult(String.Empty,
                ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDDATE, CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                    StrMustBeDate, Description));
        }

        #endregion
    }
}