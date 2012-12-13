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

namespace Ict.Common
{
    /// <summary>
    /// <para>
    /// Central Inventory of application-independent Error Codes.
    /// </para>
    /// <para>
    /// The data that goes alongside an error code in this inventory can be programmatically
    /// accessed using one of the static 'Helper Methods' of the <see cref="ErrorCodes" /> Class!
    /// </para>
    /// </summary>
    /// <remarks>
    /// Error codes are used in applications because the message text and message title may be
    /// translated into any language and its meaning will be unclear to support staff who
    /// don't speak the language the message is shown in. The only way they can identify the
    /// error message in such a case is by looking up the error code.
    /// </remarks>
    public class CommonErrorCodes
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
        //  * 'GENC' - hard-coded value for General Error Codes which are not related to a specific
        //    application (e.g. OpenPetra);
        //  * a full stop ('.');
        //  * a running five-digit number with leading zeroes which is *unique*;
        //  * a single character. 'V' denotes a data verification error, 'N' denotes a non-critical error,
        //   'E' denotes any other error.

        #endregion


        #region General error codes

        /// <summary>Invalid date.</summary>
        [ErrCodeAttribute("Invalid date.",
             ErrorMessageText = "Invalid date entered.",
             FullDescription = "The date entered is not a valid date.")]
        public const String ERR_INVALIDDATE = "GENC.00001V";

        /// <summary>Date may not be empty.</summary>
        [ErrCodeAttribute("Date may not be empty.",
             FullDescription = "The date may not be empty.")]
        public const String ERR_NOUNDEFINEDDATE = "GENC.00002V";

        /// <summary>Future date not allowed.</summary>
        [ErrCodeAttribute("Future date not allowed.",
             FullDescription = "The date entered lies in the future, and that is not allowed in this case.")]
        public const String ERR_NOFUTUREDATE = "GENC.00003V";

        /// <summary>Past date not allowed.</summary>
        [ErrCodeAttribute("Past date not allowed.",
             FullDescription = "The date entered lies in the past, and that is not allowed in this case.")]
        public const String ERR_NOPASTDATE = "GENC.00004V";

        /// <summary>Invalid number entered.</summary>
        [ErrCodeAttribute("Invalid number entered.",
             FullDescription = "The value entered is not a number in the required number format.")]
        public const String ERR_INVALIDNUMBER = "GENC.00005V";

        /// <summary>Invalid numbers entered (numbers do not conform to a certain rule).</summary>
        [ErrCodeAttribute("Invalid numbers entered.",
             FullDescription = "The numeric values entered do not conform to a certain rule (that rule should be mentioned in the error message).")]
        public const String ERR_INCONGRUOUSNUMBERS = "GENC.00006V";

        /// <summary>Invalid numbers entered (numbers do not conform to a certain rule).</summary>
        [ErrCodeAttribute("Invalid values entered.",
             FullDescription = "The values entered do not conform to a certain rule (that rule should be mentioned in the error message).")]
        public const String ERR_INCONGRUOUSSTRINGS = "GENC.00007V";

        /// <summary>Alphanumeric value may not be empty.</summary>
        [ErrCodeAttribute("Alphanumeric value may not be empty.",
             FullDescription = "The value may not be empty.")]
        public const String ERR_NOEMPTYSTRING = "GENC.00008V";

        /// <summary>"Information missing."</summary>
        [ErrCodeAttribute("Information missing.",
             FullDescription = "More information is needed.",
             ErrorMessageTitle = "Information Missing")]
        public const String ERR_INFORMATIONMISSING = "GENC.00009V";

        /// <summary>Value may not be null.</summary>
        [ErrCodeAttribute("A value must be assigned.",
             FullDescription = "The value may not be empty.")]
        public const String ERR_NONULL = "GENC.00010V";

        /// <summary>Alphanumeric value is too long.</summary>
        [ErrCodeAttribute("Alphanumeric value is too long.",
             FullDescription = "The value entered contains too many characters.")]
        public const String ERR_STRINGTOOLONG = "GENC.00011V";

        /// <summary>Invalid number entered.</summary>
        [ErrCodeAttribute("Precision loss.",
             FullDescription =
                 "The value entered has more decimals than can be stored. Therefore a loss in decimal precision will occur as rounding will be applied.")
        ]
        public const String ERR_DECIMALPRECISIONLOSSROUNDING = "GENC.00012N";

        /// <summary>Date is not within date range.</summary>
        [ErrCodeAttribute("Date is not within date range.",
             FullDescription =
                 "The date entered is not allowed as it does not lie within the required date range. It must lie between {0} and {1}.")]
        public const String ERR_DATENOTINDATERANGE = "GENC.00013V";

        /// <summary>Date must have a sensible value (e.g. not too far in the past).</summary>
        [ErrCodeAttribute("Date must have a sensible value.",
             ErrorMessageText = "'{0}' is not a possible value in this case")]
        public const String ERR_UNREALISTICDATE_ERROR = "GENC.00014V";

        /// <summary>Integer time must be in range 0..86399.</summary>
        [ErrCodeAttribute("Invalid values entered.",
             FullDescription = "The text entered does not specify a valid time.")]
        public const String ERR_INVALIDINTEGERTIME = "GENC.00015V";

//        [ErrCodeAttribute("Test duplicate.")]
//        public const String ERR_NOFUTUREDATE2 = "GENC.00002V";

        #endregion
    }
}