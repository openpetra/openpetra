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

namespace Ict.Common.Verification
{
    /// <summary>
    /// Contains Resource Strings which can be used anywhere in the verification routines.
    /// </summary>
    public class CommonResourcestrings
    {
        #region Titles

        /// <summary>Generic indication of invalid data.</summary>
        public static readonly string StrInvalidDataTitle = Catalog.GetString("Invalid Data");

        /// <summary>Generic indication of missing data.</summary>
        public static readonly string StrInformationMissingTitle = Catalog.GetString("Information Missing");

        /// <summary>Generic indication of an invalid number.</summary>
        public static readonly string StrInvalidNumberEntered = Catalog.GetString("Invalid number entered.");

        /// <summary>Generic indication of a precision loss due to rounding.</summary>
        public static readonly string StrPrecisionLossRounding = Catalog.GetString("Precision loss due to rounding.");

        /// <summary>Generic indication of invalid numbers.</summary>
        public static readonly string StrInvalidNumbersEntered = Catalog.GetString("Invalid numbers entered.");

        /// <summary>Generic indication of an invalid date.</summary>
        public static readonly string StrInvalidDateEntered = Catalog.GetString("Invalid date entered.");

        /// <summary>Generic indication of an invalid string.</summary>
        public static readonly string StrInvalidStringEntered = Catalog.GetString("Invalid value entered.");

        #endregion

        #region Message content texts

        /// <summary>Generic message for a setting that cannot be empty.</summary>
        public static readonly string StrSettingCannotBeEmpty = Catalog.GetString("The value for this setting cannot be empty text.");

        #endregion

        //        /// <summary>todoComment</summary>
        //        public static readonly string StrErrorTheCodeIsNoLongerActive = Catalog.GetString(
        //            "The code '{0}' is no longer active.\r\nDo you still want to use it?");
    }
}