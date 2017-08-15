//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Text.RegularExpressions;
using Ict.Common.Verification;
using GNU.Gettext;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for general verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TGeneralChecks
    {
        #region Resourcestrings

        private static readonly string StrValueMustNotBeNull = Catalog.GetString("A value must be entered for {0}.");
        private static readonly string StrValueMustNotBeNullOrEmptyString = Catalog.GetString("A value must be entered for {0}.");

        #endregion


        #region ValueMustNotBeNull

        /// <summary>
        /// Checks whether an Object is null.
        /// </summary>
        /// <param name="AValue">The Object to check.</param>
        /// <param name="ADescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> is not null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult ValueMustNotBeNull(object AValue, string ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            if (AValue == null)
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NONULL, StrValueMustNotBeNull, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn);
                }
            }

            return ReturnValue;
        }

        #endregion


        #region ValueMustNotBeNullOrEmptyString

        /// <summary>
        /// Checks whether an Object is null value or empty string.
        /// </summary>
        /// <param name="AValue">The Object to check.</param>
        /// <param name="ADescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> is not null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult ValueMustNotBeNullOrEmptyString(object AValue, string ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            if ((AValue == null) || (AValue.ToString() == String.Empty))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NONULL, StrValueMustNotBeNullOrEmptyString, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn);
                }
            }

            return ReturnValue;
        }

        #endregion
    }
}
