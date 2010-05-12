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
using Mono.Unix;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for numerical verifications
    ///
    /// </summary>
    public class TNumericalChecks
    {
        /// <summary>
        /// Checks whether the string s contains a valid integer.
        ///
        /// </summary>
        /// <param name="AString">String that should be checked</param>
        /// <param name="ADescription">Description what the integer is about (for the error
        /// message)</param>
        /// <returns>Nil if string contains a valid Integer, otherwise a verification
        /// result with a message that uses ADescription
        /// </returns>
        public static TVerificationResult IsValidInteger(String AString, String ADescription)
        {
            TVerificationResult ReturnValue = null;
            const String ERRORCODE = "X_0041";

            try
            {
                System.Convert.ToInt32(AString);
            }
            catch (System.Exception)
            {
                ReturnValue = new TVerificationResult("",
                    Catalog.GetString("Invalid number entered.") +
                    String.Format(Catalog.GetString("'{0}' must be an Integer (= a number without a fraction)."),
                        ADescription),
                    Catalog.GetString("Invalid Data"),
                    ERRORCODE,
                    TResultSeverity.Resv_Critical);
            }
            return ReturnValue;
        }
    }
}