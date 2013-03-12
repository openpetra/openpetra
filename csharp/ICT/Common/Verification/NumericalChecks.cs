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
        private static readonly string StrNumberMustBePositiveInteger = Catalog.GetString(
            "{0} must be a positive integer (= a number without a fraction).");
        private static readonly string StrNumberMustBePositiveDecimal = Catalog.GetString(
            "{0} must be a positive decimal number (= a number that has a fraction).");
        private static readonly string StrNumberMustBePositiveIntegerOr0 = Catalog.GetString(
            "{0} must be a positive integer (= a number without a fraction), or 0.");
        private static readonly string StrNumberMustBePositiveDecimalOr0 = Catalog.GetString(
            "{0} must be a positive decimal number (= a number that has a fraction), or 0.");
        private static readonly string StrNumberMustBeNegativeInteger = Catalog.GetString(
            "{0} must be a negative integer (= a number without a fraction).");
        private static readonly string StrNumberMustBeNegativeDecimal = Catalog.GetString(
            "{0} must be a negative decimal number (= a number that has a fraction).");
        private static readonly string StrNumberMustBeNegativeIntegerOr0 = Catalog.GetString(
            "{0} must be a negative integer (= a number without a fraction), or 0.");
        private static readonly string StrNumberMustBeNegativeDecimalOr0 = Catalog.GetString(
            "{0} must be a negative decimal number (= a number that has a fraction), or 0.");
        private static readonly string StrNumberMustBeNonZero = Catalog.GetString(
            "{0} must be a positive or negative number but not zero.");
        private static readonly string StrNumberCannotBeGreaterThan = Catalog.GetString("{0} cannot be greater than {1}.");
        private static readonly string StrNumberCannotBeGreaterOrEqualTo = Catalog.GetString("{0} cannot be greater or equal to {1}.");
        private static readonly string StrNumberNeedsToBeInRange = Catalog.GetString("{0} needs to be between {1} and {2}.");
        private static readonly string StrNumberPrecisionExceededBeforeDecimalPoint = Catalog.GetString(
            "{0} must not have more than {1} digits before the decimal point, but it has got {2}.");
        private static readonly string StrNumberPrecisionExceededAfterDecimalPoint = Catalog.GetString(
            "{0} has more digits after the decimal point ({1}) than can be stored ({2}). Therefore a loss in decimal precision will occur as rounding will be applied from decimal digit {3} onwards.");

        #endregion


        #region IsValid...

        /// <summary>
        /// Checks whether a string represents a valid integer. A null value is accepted.
        /// </summary>
        /// <param name="AValue">String that should be checked.</param>
        /// <param name="ADescription">Description what the integer number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidInteger(String AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            Int64 temp;

            if (!Int64.TryParse(AValue, out temp))
            {
                ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeInteger, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains details
        /// about the problem, with a message that uses <paramref name="ADescription" />.
        /// </returns>
        public static TVerificationResult IsValidDouble(String AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            Double temp;

            if (!Double.TryParse(AValue, out temp))
            {
                ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeDecimal, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or
        /// is null, otherwise a <see cref="TVerificationResult" /> is returned that contains
        /// details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        /// <paramref name="ADescription" />.
        public static TVerificationResult IsValidDecimal(String AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            Decimal temp;

            if (!Decimal.TryParse(AValue, out temp))
            {
                ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INVALIDNUMBER,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeDecimal, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsPositive...

        /// <summary>
        /// Checks whether an integer is greater than zero. A null value is accepted.
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
        public static TVerificationResult IsPositiveInteger(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value <= 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveInteger, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a double or single number is greater than zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveDouble(double? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value <= 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimal, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a decimal number is greater than zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveDecimal(decimal? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value <= 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimal, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid integer number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroInteger(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
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
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveIntegerOr0, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDouble(double? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
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
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimalOr0, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsPositiveOrZeroDecimal(decimal? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
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
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBePositiveDecimalOr0, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsNegative...

        /// <summary>
        /// Checks whether an integer is less than zero. A null value is accepted.
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
        public static TVerificationResult IsNegativeInteger(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (!(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeInteger, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a double or single number is less than zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNegativeDouble(double? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (!(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeDecimal, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a decimal number is less than zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNegativeDecimal(decimal? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (!(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeDecimal, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsNegativeOrZero...

        /// <summary>
        /// Checks whether an integer is lesser or equal to zero. A null value is accepted.
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
        public static TVerificationResult IsNegativeOrZeroInteger(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if ((AValue.Value != 0)
                && !(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeIntegerOr0, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a double or single number is lesser or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNegativeOrZeroDouble(double? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if ((AValue.Value != 0)
                && !(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeDecimalOr0, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a decimal number is lesser or equal to zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNegativeOrZeroDecimal(decimal? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if ((AValue.Value != 0)
                && !(AValue.Value < 0))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNegativeDecimalOr0, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region IsNonZero...

        /// <summary>
        /// Checks whether an integer is non-zero. A null value is accepted.
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
        public static TVerificationResult IsNonZeroInteger(Int64? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value == 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNonZero, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a double or single number is non-zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Double or single number.</param>
        /// <param name="ADescription">Description what the double or single number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid double or single number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNonZeroDouble(double? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value == 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNonZero, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a decimal number is non-zero. A null value is accepted.
        /// </summary>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a valid decimal number or is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNonZeroDecimal(decimal? AValue, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            if (!AValue.HasValue)
            {
                return null;
            }

            // Check
            if (AValue.Value == 0)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberMustBeNonZero, new string[] { Description }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, new string[] { FirstNumberDescription, SecondNumberDescription }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, new string[] { FirstNumberDescription, SecondNumberDescription }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 >= AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterOrEqualTo, new string[] { FirstNumberDescription, SecondNumberDescription }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondInteger(Int64? AValue1, Int64? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, new string[] { FirstNumberDescription, SecondNumberDescription }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDouble(double? AValue1, double? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, new string[] { FirstNumberDescription, SecondNumberDescription }));

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
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="AFirstNumberDescription" /> and <paramref name="ASecondNumberDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDecimal(decimal? AValue1, decimal? AValue2,
            string AFirstNumberDescription, string ASecondNumberDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue1.HasValue || AValue2.HasValue))
            {
                return null;
            }

            string FirstNumberDescription = THelper.NiceValueDescription(AFirstNumberDescription);
            string SecondNumberDescription = THelper.NiceValueDescription(ASecondNumberDescription);

            // Check
            if (AValue1 > AValue2)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberCannotBeGreaterThan, new string[] { FirstNumberDescription, SecondNumberDescription }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region IsInRange...

        /// <summary>
        /// Checks whether integer number is within a given range
        /// </summary>
        /// <param name="AValue">Integer number.</param>
        /// <param name="ALowerLimit">Lower range limit.</param>
        /// <param name="AUpperLimit">Upper range limit.</param>
        /// <param name="ANumberDescription">Description what the number is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem, with a message that uses
        /// <paramref name="ANumberDescription" /></returns>
        public static TVerificationResult IsInRange(Int64? AValue, Int64? ALowerLimit, Int64? AUpperLimit,
            string ANumberDescription, object AResultContext = null,
            System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if (!(AValue.HasValue))
            {
                return null;
            }

            string NumberDescription = THelper.NiceValueDescription(ANumberDescription);

            // Check
            if ((AValue < ALowerLimit)
                || (AValue > AUpperLimit))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSNUMBERS,
                        CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberNeedsToBeInRange, new string[] { NumberDescription, ALowerLimit.ToString(), AUpperLimit.ToString() }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region IsNumberPrecisionNotExceeded

        /// <summary>
        /// Checks whether a decimal's decimal digits and fractional digits are less or equal to the ones specified. A null value is accepted.
        /// </summary>
        /// <remarks>This Method can be used to check number formats as they are specified in petra.xml, e.g. "lenght=6", "decimals=2". - Set <paramref name="APrecision"></paramref> to 6 and <paramref name="AScale"></paramref>to 2 to validate this
        /// number format with this method.</remarks>
        /// <param name="AValue">Decimal number.</param>
        /// <param name="APrecision">The total count of significant digits in the whole number, that is, the number of digits to *both sides* of the decimal point.</param>
        /// <param name="AScale">The count of decimal digits in the fractional part, to the right of the decimal point.</param>
        /// <param name="ADescription">Description what the decimal number is about (for the error
        /// message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> contains a decimal number which number precision is according to
        /// <paramref name="APrecision"></paramref> and <paramref name="AScale"></paramref>, or if
        /// <paramref name="AValue" /> is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about the problem,
        /// with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsNumberPrecisionNotExceeded(decimal? AValue, int APrecision, int AScale, String ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);
            string DecimalsOnlyStr;
            string FractionOnlyStr;

            if (!AValue.HasValue)
            {
                return null;
            }

            DecimalsOnlyStr = Decimal.Truncate(AValue.Value).ToString();

            #region Check #1 - Number of digits to the left of the decimal point

            if (DecimalsOnlyStr.Length > (APrecision - AScale))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDNUMBER, CommonResourcestrings.StrInvalidNumberEntered + Environment.NewLine +
                        StrNumberPrecisionExceededBeforeDecimalPoint,
                        new string[] { Description, (APrecision - AScale).ToString(), DecimalsOnlyStr.Length.ToString() }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }

                return ReturnValue;
            }

            #endregion

            #region Check #2 - Number of significant digits to the right of the decimal point

            FractionOnlyStr = (AValue.Value - Decimal.Truncate(AValue.Value)).ToString();

            if (FractionOnlyStr.Length > 2)
            {
                FractionOnlyStr = FractionOnlyStr.Substring(2);
            }
            else if (FractionOnlyStr.Length > 1)
            {
                FractionOnlyStr = FractionOnlyStr.Substring(1);
            }

            // Remove any trailing zeroes as they do not form a significant part of the fraction for our purposes
            while (FractionOnlyStr.EndsWith("0"))
            {
                FractionOnlyStr = FractionOnlyStr.Substring(0, FractionOnlyStr.Length - 1);
            }

            if (FractionOnlyStr.Length > AScale)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_DECIMALPRECISIONLOSSROUNDING, CommonResourcestrings.StrPrecisionLossRounding +
                        Environment.NewLine +
                        StrNumberPrecisionExceededAfterDecimalPoint,
                        new string[] { Description, FractionOnlyStr.Length.ToString(), AScale.ToString(), (AScale + 1).ToString() }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            #endregion

            return ReturnValue;
        }

        #endregion
    }
}