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
using Ict.Petra.Shared;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// <para>
    /// Central Inventory of OpenPetra Error Codes.
    /// </para>
    /// <para>
    /// The data that goes alongside an error code in this inventory can be programmatically
    /// accessed using one of the static 'Helper Methods' of this Class!
    /// </para>
    /// </summary>
    /// <remarks>
    /// Error codes are used in OpenPetra because the message text and message title may be
    /// translated into any language and its meaning will be unclear to support staff who
    /// don't speak the language the message is shown in. The only way they can identify the
    /// error message in such a case is by looking up the error code.
    /// </remarks>
    public class PetraErrorCodes
    {
        #region DOCUMENTATION - read this when adding Error Codes for the first time!

        //
        // Error Codes are listed as string constants whose name can be used by the programmer
        // (code completion is available) and whose string value represents the error code itself.
        //
        // Decoration with an 'ErrCodeAttribute' is highly recommended, as it is documenting the
        // Error Code and makes automatic displaying of a whole error message with plain text
        // explanation for the user very easy!

        // An error code string consists of four sections:
        //  * the abbreviated name of the OpenPetra Module in which the error occurs, or 'GEN'
        //    if it is not related to any Module;
        //  * a full stop ('.');
        //  * a running five-digit number with leading zeroes which is *unique*;
        //  * a single character. 'V' denotes a data verification error, 'N' denotes a non-critical error,
        //   'E' denotes any other error.

        #endregion


        #region General error codes

        /// <summary>General data verification error.</summary>
        [ErrCodeAttribute("General data verification error.",
             FullDescription = "This unspecific error is shown when a check on the validity of data in OpenPetra failed.",
             HelpID = "V.12345")]
        public const String ERR_GENERAL_VERIFICATION_ERROR = "GEN.00001V";

        /// <summary>Value is no longer assignable.</summary>
        [ErrCodeAttribute("Value is no longer assignable.",
             ErrorMessageText = "The code '{0}' is no longer active.\r\nDo you still want to use it?",
             ErrorMessageTitle = "Invalid Data Entered")]
        public const String ERR_VALUEUNASSIGNABLE = "GEN.00002V";

        /// <summary>No permission to access DB Table.</summary>
        [ErrCodeAttribute("You don't have permission to access the specified database table.")]
        public const String ERR_NOPERMISSIONTOACCESSTABLE = "GEN.00003E";

        /// <summary>No permission to access OpenPetra Module.</summary>
        public const String ERR_NOPERMISSIONTOACCESSMODULE = "GEN.00004E";

        /// <summary>No permission to access OpenPetra Group.</summary>
        public const String ERR_NOPERMISSIONTOACCESSGROUP = "GEN.00005E";

        /// <summary>Concurrent changes to data happened.</summary>
        public const String ERR_CONCURRENTCHANGES = "GEN.00006E";

        #endregion

        #region Partner Module-specific error codes

        /// <summary>Partner Key is invalid.</summary>
        public const String ERR_PARTNERKEY_INVALID = "PARTN.00001V";

        /// <summary>Partner Status MERGED change undone.</summary>
        public const String ERR_PARTNERSTATUSMERGEDCHANGEUNDONE = "PARTN.00002N";

        /// <summary>UnitName change undone.</summary>
        public const String ERR_UNITNAMECHANGEUNDONE = "PARTN.00003N";

        /// <summary>Bank Bic/Swift Code is invalid.</summary>
        public const String ERR_BANKBICSWIFTCODEINVALID = "PARTN.00004V";

        #endregion
    }
}