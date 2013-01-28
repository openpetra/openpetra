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

        /// <summary>Value is no longer assignable (show error).</summary>
        [ErrCodeAttribute("Value is no longer assignable.",
             ErrorMessageText = "The code '{0}' is no longer active.\r\nDo you still want to use it?",
             ErrorMessageTitle = "Invalid Data Entered")]
        public const String ERR_VALUEUNASSIGNABLE = "GEN.00002V";

        /// <summary>Value is no longer assignable (just give warning).</summary>
        [ErrCodeAttribute("Value is no longer assignable.",
             ErrorMessageText = "The code '{0}' is no longer assignable.\r\nIt should no longer be used.",
             ErrorMessageTitle = "Unassignable Code Used")]
        public const String ERR_VALUEUNASSIGNABLE_WARNING = "GEN.00007N";

        /// <summary>No permission to access DB Table.</summary>
        [ErrCodeAttribute("You don't have permission to access the specified database table.")]
        public const String ERR_NOPERMISSIONTOACCESSTABLE = "GEN.00003E";

        /// <summary>No permission to access OpenPetra Module.</summary>
        public const String ERR_NOPERMISSIONTOACCESSMODULE = "GEN.00004E";

        /// <summary>No permission to access OpenPetra Group.</summary>
        public const String ERR_NOPERMISSIONTOACCESSGROUP = "GEN.00005E";

        /// <summary>Concurrent changes to data happened.</summary>
        public const String ERR_CONCURRENTCHANGES = "GEN.00006E";

        /// <summary>Values must not be the same.</summary>
        [ErrCodeAttribute("Values must not be the same.",
             ErrorMessageText = "Values '{0}' and '{1}' must not be the same",
             ErrorMessageTitle = "Invalid Data Entered")]
        public const String ERR_VALUESIDENTICAL_ERROR = "GEN.00008V";

        #endregion

        #region Finance Module-specific error codes

        /// <summary>Suspense accounts exist despite disabling suspense accounts for a ledger.</summary>
        [ErrCodeAttribute("Suspense Accounts in use.",
             ErrorMessageText = "The use of suspense accounts cannot be	disabled because there are accounts	currently in use.")]
        public const String ERR_NO_SUSPENSE_ACCOUNTS_ALLOWED = "FIN.00001V";

        /// <summary>Partner Key is invalid.</summary>
        [ErrCodeAttribute("Too small number of forwarding periods.",
             ErrorMessageText = "There must be at least {0} periods because {1} periods have been used already.")]
        public const String ERR_NUMBER_FWD_PERIODS_TOO_SMALL = "FIN.00002V";

        /// <summary>Warning message that two exchange rates differ by more than 10%.</summary>
        [ErrCodeAttribute("Exchange rate may be incorrect.",
             ErrorMessageText = "The rate of {0} that you have entered for {1}->{2} on {3} at {4} differs from the previous or next rate for the same currencies by more than {5:0%}.")]
        public const String ERR_EXCH_RATE_MAY_BE_INCORRECT = "FIN.00003N";

        #endregion

        #region Partner Module-specific error codes

        /// <summary>Partner Key is invalid.</summary>
        [ErrCodeAttribute("Invalid Partner.",
             ErrorMessageText = "Invalid Partner entered: The Partner specified with PartnerKey {0} is not valid.")]
        public const String ERR_PARTNERKEY_INVALID = "PARTN.00001V";

        /// <summary>Partner Key is invalid (must be non-zero).</summary>
        [ErrCodeAttribute("Invalid Partner.",
             ErrorMessageText = "Invalid Partner entered: PartnerKey 0 is not a valid value.")]
        public const String ERR_PARTNERKEY_INVALID_NOZERO = "PARTN.00002V";

        /// <summary>Partner Status MERGED must not be assigned.</summary>
        [ErrCodeAttribute("Partner Status MERGED must not be assigned",
             ErrorMessageText = "The Partner Status cannot be set to 'MERGED' by the user - this Partner Status\r\n" +
                                "is set only by the Partner Merge function for Partners that have been merged\r\n" +
                                "into another Partner!",
             ControlValueUndoRequested = true)]
        public const String ERR_PARTNERSTATUSMERGEDCHANGEUNDONE = "PARTN.00003V";

        /// <summary>UnitName change undone.</summary>
        public const String ERR_UNITNAMECHANGEUNDONE = "PARTN.00004N";

        /// <summary>BIC (Bank Identifier Code/SWIFT Code) is invalid.</summary>
        public const String ERR_BANKBICSWIFTCODEINVALID = "PARTN.00005V";

        /// <summary>'Branch Code' format matches the format of a BIC (Bank Identifier Code/SWIFT Code) --- Non-critical.</summary>
        [ErrCodeAttribute("'Branch Code possibly a BIC/SWIFT Code",
             ErrorMessageText = "The {0} you entered seems to be a BIC/SWIFT Code!\r\n\r\n" +
                                "Make sure that you have entered the BIC/SWIFT Code in the {1} field\r\n" +
                                "and that the information you entered in the {2} field is actually\r\nthe {3}.",
             ErrorMessageTitle = "{0} seems to be a BIC/SWIFT Code")]
        public const String ERR_BRANCHCODELIKEBIC = "PARTN.00006N";

        /// <summary>Invalid International Postal Type.</summary>
        [ErrCodeAttribute("Invalid International Postal Type.",
             ErrorMessageText = "Invalid International Postal Type entered.",
             FullDescription = "The International Postal Code entered is not a valid International Postal Type.")]
        public const String ERR_INVALIDINTERNATIONALPOSTALCODE = "PARTN.00008V";


        #region Subscriptions

        /// <summary>Subscription Status Mandatory.</summary>
        [ErrCodeAttribute("Subscription Status Mandatory",
             ErrorMessageText = "A valid Subscription Status must be selected.",
             ErrorMessageTitle = "Subscription Status Mandatory")]
        public const String ERR_SUBSCRIPTION_STATUSMANDATORY = "PARTN.00007V";

        /// <summary>Reason Ended Mandatory.</summary>
        [ErrCodeAttribute("Reason Ended Mandatory",
             ErrorMessageText = "Cannot have a cancelled or expired subscription without a reason for ending.",
             ErrorMessageTitle = "Reason Ended Mandatory")]
        public const String ERR_SUBSCRIPTION_REASONENDEDMANDATORY_WHEN_EXPIRED = "PARTN.00009V";

        /// <summary>Date Ended Mandatory.</summary>
        [ErrCodeAttribute("Date Ended Mandatory",
             ErrorMessageText = "Cannot have a cancelled or expired subscription without an end date.",
             ErrorMessageTitle = "Date Ended Mandatory")]
        public const String ERR_SUBSCRIPTION_DATEENDEDMANDATORY_WHEN_EXPIRED = "PARTN.00010V";

        /// <summary>Reason Ended must not be set for active Subscription.</summary>
        [ErrCodeAttribute("Clear Reason Ended",
             ErrorMessageText = "Cannot have a reason for ending without setting status to 'CANCELLED' or 'EXPIRED'.",
             ErrorMessageTitle = "Clear Reason Ended")]
        public const String ERR_SUBSCRIPTION_REASONENDEDSET_WHEN_ACTIVE = "PARTN.00011V";

        /// <summary>Date Ended must not be set for active Subscription.</summary>
        [ErrCodeAttribute("Clear Date Ended",
             ErrorMessageText = "Cannot have an end date without setting status to 'CANCELLED' or 'EXPIRED'.",
             ErrorMessageTitle = "Clear Date Ended")]
        public const String ERR_SUBSCRIPTION_DATEENDEDSET_WHEN_ACTIVE = "PARTN.00012V";

        #endregion

        #endregion

        #region Personnel Module-specific error codes

        /// <summary>Duplicate application for event.</summary>
        [ErrCodeAttribute("Duplicate application for event",
             ErrorMessageText = "An application for event {0} already exists!\r\n" +
                                "Please choose a different event.",
             ErrorMessageTitle = "Duplicate application for event")]
        public const String ERR_APPLICATION_DUPLICATE_EVENT = "PES.00001V";

        #endregion
    }
}