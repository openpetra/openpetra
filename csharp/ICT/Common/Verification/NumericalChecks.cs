//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Common.Verification;
using GNU.Gettext;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for numerical verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TNumericalChecks
    {
        #region Resourcestrings

        private static readonly string StrNumberMustBeInteger = Catalog.GetString("{0} must be an integer (= a number without a fraction).");
        private static readonly string StrNumberMustBeDecimal = Catalog.GetString("{0} must be a decimal number (= a number that has a fraction).");
        private static readonly string StrNumberMustBePositiveIntegerOr0 = Catalog.GetString(
            "{0} must be a positive integer (= a number without a fraction), or 0.");
        private static readonly string StrNumberMustBePositiveDecimalOr0 = Catalog.GetString(
            "{0} must be a positive decimal number (= a number that has a fraction), or 0.");
        private static readonly string StrNumberCannotBeGreaterThan = Catalog.GetString("{0} cannot be greater than {1}.");
        private static readonly string StrNumberCannotBeGreaterOrEqualTo = Catalog.GetString("{0} cannot be greater or equal to {1}.");

        #endregion


        #region IsValid...

        /// <summary>
        /// Checks whether a string represents a valid integer. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidInteger(String AValue, String ADescription)
        {
            return IsValidInteger(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether a string represents a valid integer. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidInteger(String AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            try
            {
                System.Convert.ToInt64(AValue);
            }
            catch (System.Exception)
            {
                ReturnValue = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeInteger, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a string represents a valid double or single number. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains details
        /// about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidDouble(String AValue, String ADescription)
        {
            return IsValidDouble(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether a string represents a valid double or single number. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains details
        /// about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidDouble(String AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            try
            {
                System.Convert.ToDouble(AValue);
            }
            catch (System.Exception)
            {
                ReturnValue = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeDecimal, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a string represents a valid decimal number. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        /// <paramref name="ADescription" />.
        public static TVerificationResult IsValidDecimal(String AValue, String ADescription)
        {
            return IsValidDecimal(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether a string represents a valid decimal number. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        /// <paramref name="ADescription" />.
        public static TVerificationResult IsValidDecimal(String AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            try
            {
                System.Convert.ToDecimal(AValue);
            }
            catch (System.Exception)
            {
                ReturnValue = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeDecimal, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsPositiveOrZero...

        /// <summary>
        /// Checks whether an integer is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Integer number.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroInteger(Int64? AValue, String ADescription)
        {
            return IsPositiveOrZeroInteger(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether an integer is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Integer number.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroInteger(Int64? AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value < 0)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveIntegerOr0, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a double or single number is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDouble(double? AValue, String ADescription)
        {
            return IsPositiveOrZeroDouble(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether a double or single number is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDouble(double? AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value < 0)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimalOr0, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a decimal number is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDecimal(decimal? AValue, String ADescription)
        {
            return IsPositiveOrZeroDecimal(AValue, ADescription, null, null, null);
        }

        /// <summary>
        /// Checks whether a decimal number is greater or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDecimal(decimal? AValue, String ADescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value < 0)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimalOr0, Description));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region FirstLesserThanSecond...

        /// <summary>
        /// Checks whether the first submitted integer number is smaller than the second submitted
        /// integer number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">First integer number.</param>
        /// <param name="AValue2">Second integer number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserThanSecondInteger(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted integer number is smaller than the second submitted
        /// integer number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">First integer number.</param>
        /// <param name="AValue2">Second integer number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, AFirstNumberDescription, ASecondNumberDescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted double or single number is smaller than the second submitted
        /// double or single number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Double or single number.</param>
        /// <param name="AValue2">Double or single number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserThanSecondDouble(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted double or single number is smaller than the second submitted
        /// double or single number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Double or single number.</param>
        /// <param name="AValue2">Double or single number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, AFirstNumberDescription, ASecondNumberDescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted decimal number is smaller than the second submitted
        /// decimal number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Decimal number.</param>
        /// <param name="AValue2">Decimal number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserThanSecondDecimal(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted decimal number is smaller than the second submitted
        /// decimal number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Decimal number.</param>
        /// <param name="AValue2">Decimal number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, AFirstNumberDescription, ASecondNumberDescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region FirstLesserOrEqualThan...

        /// <summary>
        /// Checks whether the first submitted integer number is smaller than or equal to the second submitted
        /// integer number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Integer number.</param>
        /// <param name="AValue2">Integer number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserOrEqualThanSecondInteger(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted integer number is smaller than or equal to the second submitted
        /// integer number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Integer number.</param>
        /// <param name="AValue2">Integer number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, AFirstNumberDescription, ASecondNumberDescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted double or single number is smaller than or equal to the second submitted
        /// double or single number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Double or single number.</param>
        /// <param name="AValue2">Double or single number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserOrEqualThanSecondDouble(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted double or single number is smaller than or equal to the second submitted
        /// double or single number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Double or single number.</param>
        /// <param name="AValue2">Double or single number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, AFirstNumberDescription, ASecondNumberDescription));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted decimal number is smaller than or equal to the second submitted
        /// decimal number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Decimal number.</param>
        /// <param name="AValue2">Decimal number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription)
        {
            return FirstLesserOrEqualThanSecondDecimal(AValue1, AValue2, AFirstNumberDescription, ASecondNumberDescription,
                null, null, null);
        }

        /// <summary>
        /// Checks whether the first submitted decimal number is smaller than or equal to the second submitted
        /// decimal number. Null values are accepted.
        /// </summary>
        /// <param name="AValue1">Decimal number.</param>
        /// <param name="AValue2">Decimal number.</param>
        /// <param name="AFirstNumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="ASecondNumberDescription">Description what the number is about (for
        /// the error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control " /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            AFirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            ASecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(String.Empty,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, AFirstNumberDescription, ASecondNumberDescription));

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