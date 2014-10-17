//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
             ErrorMessageText = "{0} '{1}' is no longer assignable.\r\nIt should no longer be used.",
             ErrorMessageTitle = "Unassignable Code Used")]
        public const String ERR_VALUEUNASSIGNABLE_WARNING = "GEN.00007V";

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

        /// <summary>Values must not be the same.</summary>
        [ErrCodeAttribute("Value outside of range.",
             ErrorMessageText = "Value for '{0}' must be between {1} and {2}",
             ErrorMessageTitle = "Invalid Data Entered")]
        public const String ERR_VALUE_OUTSIDE_OF_RANGE = "GEN.00009V";

        #endregion

        #region Conference Module-specific error codes


        /// <summary>Standard cost inconsistency</summary>
        [ErrCodeAttribute("Standard cost inconsistency.",
             ErrorMessageText = "{0} day(s) at the conference costs more than {1} days!\r\n" + "Are you sure this is correct?",
             ErrorMessageTitle = "Standard cost inconsistency")]
        public const String ERR_STANDARD_COST_INCONSISTENCY = "CON.00001N";

        /// <summary>Applicable date after conference end date</summary>
        [ErrCodeAttribute("Applicable date after conference end date.",
             ErrorMessageText = "Applicable date cannot be after the end of the conference.")]
        public const String ERR_APPLICABLE_DATE_AFTER_CONFERENCE_END_DATE = "CON.00002V";

        /// <summary>Early applicable date later than a late applicable date</summary>
        [ErrCodeAttribute("Early applicable date later than a late applicable date.",
             ErrorMessageText = "There is an early registration discount which has a date later than this late registration surcharge.")]
        public const String ERR_EARLY_APPLICABLE_DATE_LATER_THAN_LATE_APPLICABLE_DATE = "CON.00003V";

        /// <summary>Late applicable date earlier than an early applicable date</summary>
        [ErrCodeAttribute("Late applicable date earlier than an early applicable date.",
             ErrorMessageText = "There is a late registration surcharge which has a date earlier than this early registration discount.")]
        public const String ERR_LATE_APPLICABLE_DATE_EARLIER_THAN_EARLY_APPLICABLE_DATE = "CON.00004V";

        /// <summary>PcDiscount discount percentage greater than 100</summary>
        [ErrCodeAttribute("Invalid discount percentage.",
             ErrorMessageText = "Discount percentages cannot be greater than 100%")]
        public const String ERR_DISCOUNT_PERCENTAGE_GREATER_THAN_100 = "CON.00005V";

        /// <summary>DiscountCriteraCode does not exist</summary>
        [ErrCodeAttribute("Internal data is missing.",
             ErrorMessageText = "Necessary internal data is missing from the database. A record is missing from the table PcDiscountCriteria.")]
        public const String ERR_DISCOUNT_CRITERIA_CODE_DOES_NOT_EXIST = "CON.00006E";

        /// <summary>CostTypeCode does not exist</summary>
        [ErrCodeAttribute("Internal data is missing.",
             ErrorMessageText = "Necessary internal data is missing from the database. A record is missing from the table PcCostType.")]
        public const String ERR_COST_TYPE_CODE_DOES_NOT_EXIST = "CON.00007E";

        #endregion

        #region Finance Module-specific error codes

        /// <summary>Suspense accounts exist despite disabling suspense accounts for a ledger.</summary>
        [ErrCodeAttribute("Suspense Accounts in use.",
             ErrorMessageText = "The use of suspense accounts cannot be	disabled because there are accounts	currently in use.")]
        public const String ERR_NO_SUSPENSE_ACCOUNTS_ALLOWED = "FIN.00001V";

        /// <summary>Too few forwarding periods.</summary>
        [ErrCodeAttribute("Too few forwarding periods.",
             ErrorMessageText = "There must be at least {0} periods because {1} periods have been used already.")]
        public const String ERR_NUMBER_FWD_PERIODS_TOO_SMALL = "FIN.00002V";

        /// <summary>Warning message that two exchange rates differ by more than 10%.</summary>
        [ErrCodeAttribute("Exchange rate may be incorrect.",
             ErrorMessageText =
                 "The rate of {0} that you have entered for {1}->{2} on {3} at {4} differs from the previous or next rate for the same currencies by more than {5:0%}.")
        ]
        public const String ERR_EXCH_RATE_MAY_BE_INCORRECT = "FIN.00003N";

        /// <summary>Period start date cannot be after 28th of a month, otherwise problems with February.</summary>
        [ErrCodeAttribute("Period start date after 28th of month.",
             ErrorMessageText = "The period cannot start after the 28th day of the month.")]
        public const String ERR_PERIOD_START_DAY_AFTER_28 = "FIN.00004V";

        /// <summary>Period date ranges need to make sure that there is no overlap and no gaps in calendar.</summary>
        [ErrCodeAttribute("Period date range incorrect.",
             ErrorMessageText = "Period {0} must end one day before the next period begins.")]
        public const String ERR_PERIOD_DATE_RANGE_WARNING = "FIN.00005N";

        /// <summary>Period date ranges need to make sure that there is no overlap and no gaps in calendar.</summary>
        [ErrCodeAttribute("Period date range incorrect.",
             ErrorMessageText = "Period {0} must end one day before the next period begins.")]
        public const String ERR_PERIOD_DATE_RANGE = "FIN.00006V";

        /// <summary>Period date ranges need to make sure that there is no overlap and no gaps in calendar.</summary>
        [ErrCodeAttribute("Current Period incorrect.",
             ErrorMessageText = "Current Period cannot be greater than Number of Periods.")]
        public const String ERR_CURRENT_PERIOD_TOO_LATE = "FIN.00007V";

        /// <summary>Partner Key must represent a Key Ministry.</summary>
        [ErrCodeAttribute("Not a Key Ministry.",
             ErrorMessageText = "Partner Key in Key Ministry field does not represent a Key Ministry.")]
        public const String ERR_NOT_A_KEY_MINISTRY = "FIN.00008V";

        /// <summary>Make sure that Key Ministry is active.</summary>
        [ErrCodeAttribute("Key Ministry not active.",
             ErrorMessageText = "Key Ministry has been deactivated and cannot be used.")]
        public const String ERR_KEY_MINISTRY_DEACTIVATED = "FIN.00009V";

        /// <summary>Allocation journal.</summary>
        [ErrCodeAttribute("Amount is too large.",
             ErrorMessageText = "The amount '{0}' is too large. An individual amount cannot be greater than the total amount.")]
        public const String ERR_AMOUNT_TOO_LARGE = "FIN.00010V";

        /// <summary>Allocation journal.</summary>
        [ErrCodeAttribute("Percentage is above 100%.",
             ErrorMessageText = "Percentage cannot be greater than 100%.")]
        public const String ERR_PERCENTAGE_TOO_LARGE = "FIN.00011V";

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

        /// <summary>Recipient Field/Motivation Group combination is invalid (must be non-gift for field=0).</summary>
        [ErrCodeAttribute("Invalid Motivation Group for Recipient's Field.",
             ErrorMessageText = "The Recipient's Field is 0 and so cannot have a Gift Motivation Group Code.")]
        public const String ERR_RECIPIENT_FIELD_MOTIVATION_GROUP = "PARTN.00999V";

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

        /// <summary>Interest must not be empty if category is set.</summary>
        [ErrCodeAttribute("Interest not set.",
             ErrorMessageText = "Interest must be selected since category '{0}' is set.",
             ErrorMessageTitle = "Interest not set")]
        public const String ERR_INTEREST_NOT_SET = "PARTN.00016V";

        /// <summary>One of Interest, Country or Field must be set for Partner Interest record.</summary>
        [ErrCodeAttribute("Insufficient Data.",
             ErrorMessageText = "At least one of Interest, Country or Field must be set.",
             ErrorMessageTitle = "Insufficient Data")]
        public const String ERR_INTEREST_NO_DATA_SET_AT_ALL = "PARTN.00017V";

        /// <summary>Partner Key is invalid (must not be null).</summary>
        [ErrCodeAttribute("Invalid Partner.",
             ErrorMessageText = "Partner Key for {0} must be set.")]
        public const String ERR_PARTNERKEY_INVALID_NOTNULL = "PARTN.00019V";

        /// <summary>Expired date must come after date effective from.</summary>
        [ErrCodeAttribute("Invalid Dates",
             ErrorMessageText = "The 'Date Effective From' cannot be after the 'Date Expired.'")]
        public const String ERR_INVALID_DATES = "PARTN.00028V";

        /// <summary>Cannot have more than one active Gift Destination.</summary>
        [ErrCodeAttribute("More than one open Gift Destination.",
             ErrorMessageText =
                 "You can only have one open Gift Destination record (no Expiry Date) per Family. All other records must have a valid Expiry Date.")]
        public const String ERR_MORETHANONE_OPEN_GIFTDESTINATION = "PARTN.00029V";

        /// <summary>Gift Destination records cannot overlap.</summary>
        [ErrCodeAttribute("Invalid Dates.",
             ErrorMessageText =
                 "The dates overlap for two of more Gift Destination records. Please ensure that no two records are active on the same day.")]
        public const String ERR_DATES_OVERLAP = "PARTN.00030V";

        /// <summary>Occupation Code is invalid.</summary>
        [ErrCodeAttribute("Invalid Occupation Code.",
             ErrorMessageText = "Invalid Occupation Code entered: The Occupation specified with Occupation Code {0} is not valid.")]
        public const String ERR_OCCUPATIONCODE_INVALID = "PARTN.00031V";

        /// <summary>Duplicate Family ID in one Family.</summary>
        [ErrCodeAttribute("Duplicate Family ID.",
             ErrorMessageText = "Duplicate Family IDs not allowed within one Family: More than one Person has been assigned Family ID {0}.")]
        public const String ERR_DUPLICATE_FAMILY_ID = "PARTN.00032V";

        /// <summary>Hyperlink with Value Format must contain THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER.</summary>
        [ErrCodeAttribute("Invalid Hyperlink with Value Format",
             ErrorMessageText =
                 "The 'Link Format' must contain a hyperlink that contains the text '{0}' (without apostrophies and all captials) somewhere in the hyperlink.")
        ]
        public const String ERR_INVALID_HYPERLINK_WITH_VALUE_NOT_CONTAINING_PLACEHOLDER = "PARTN.00033V";

        /// <summary>Hyperlink with Value Format must not consist of just THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER.</summary>
        [ErrCodeAttribute("Invalid Hyperlink with Value Format",
             ErrorMessageText =
                 "The 'Link Format' must contain a hyperlink that contains the text '{0}' (without apostrophies and all captials) somewhere in the hyperlink, and not be just '{0}'.")
        ]
        public const String ERR_INVALID_HYPERLINK_WITH_VALUE_JUST_CONTAINING_PLACEHOLDER = "PARTN.00034V";

        /// <summary>No Primary Email Address has been set desipite current E-Mail Addresses are available.</summary>
        [ErrCodeAttribute("Primary Email Address Not Set Desipite E-Mail Addresses on File",
             ErrorMessageText = "No Primary Email Address has been set although current E-Mail Addresses are available.  IMPORTANT: OpenPetra can't send emails to this Partner in automated situations unless a Primary Email Address has been chosen!")]
        public const String ERR_PRIMARY_EMAIL_ADDR_NOT_SET_DESIPITE_EMAIL_ADDR_AVAIL = "PARTN.00035N";

        /// <summary>A Primary Email Address has been set but there are no current E-Mail Addresses available.</summary>
        [ErrCodeAttribute("Primary Email Address Set But No E-Mail Addresses on File",
             ErrorMessageText = "A Primary Email Address is still set, but there are no current E-Mail Addresses available. Remove the Primary Email Address!")]
        public const String ERR_PRIMARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL = "PARTN.00036V";

        /// <summary>A Primary Email Address has been set but there are no current E-Mail Addresses available.</summary>
        [ErrCodeAttribute("Primary Email Address Set But the E-Mail Addresses record is not current",
             ErrorMessageText = "A Primary Email Address is still set, but the corresponding E-Mail Address record is not current. Change the Primary Email Address to a current E-Mail Address!")]
        public const String ERR_PRIMARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT = "PARTN.00037V";
        
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

        #region Banking Details
        /// <summary>Banking Details: only one main account.</summary>
        [ErrCodeAttribute("Only one main account.",
             ErrorMessageText = "You can only have one main bank account per partner.")]
        public const String ERR_BANKINGDETAILS_ONLYONEMAINACCOUNT = "PARTN.00013V";

        /// <summary>Banking Details: At least one main account.</summary>
        [ErrCodeAttribute("At least one main account.",
             ErrorMessageText = "You must set at least one bank account as main account.")]
        public const String ERR_BANKINGDETAILS_ATLEASTONEMAINACCOUNT = "PARTN.00014V";

        /// <summary>Partner Key is invalid (must be non-zero).</summary>
        [ErrCodeAttribute("No Bank selected.",
             ErrorMessageText = "You must select a Bank for this bank account")]
        public const String ERR_BANKINGDETAILS_NO_BANK_SELECTED = "PARTN.000017V";

        /// <summary>Account Number and IBAN cannot both be empty</summary>
        [ErrCodeAttribute("No Account Number or IBAN.",
             ErrorMessageText = "You must include an Account Number and/or IBAN for this bank account")]
        public const String ERR_BANKINGDETAILS_MISSING_ACCOUNTNUMBERORIBAN = "PARTN.000019V";

        /// <summary>IBAN must always have 34 or less characters</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText =
                 "The IBAN is longer than 34 characters. IBANs must not be longer than 34 characters (spaces are not counted). " +
                 "This is defined in the European Banking Standard (EBS).\n\n" + "The IBAN could be invalid! Check the IBAN carefully!")
        ]
        public const String ERR_IBAN_TOO_LONG = "PARTN.00020V";

        /// <summary>IBAN must begin with two letters</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The first two characters of an IBAN must be alphabetic.")
        ]
        public const String ERR_IBAN_NOTBEGINWITHTWOLETTERS = "PARTN.00021V";

        /// <summary>Third and forth characters of IBAN must be digits</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The third and fourth characters of an IBAN must be digits.")
        ]
        public const String ERR_IBAN_THIRDANDFORTHNOTDIGITS = "PARTN.00022V";

        /// <summary>Check Digits (3rd and 4th characters) must be within range 02-98</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The check digits of the IBAN are wrong (third and fourth character of the IBAN).")
        ]
        public const String ERR_IBAN_CHECKDIGITSAREWRONG = "PARTN.00023V";

        /// <summary>IBAN country is not defined</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The country {0} is not defined and so IBAN's length can not be checked.")
        ]
        public const String ERR_IBAN_COUNTRYNOTDEFINIED = "PARTN.00024N";

        /// <summary>IBAN's length must be equal to a defined length for it's country (if included)</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The length of the IBAN is wrong. IBANs for {0} ({1}) need to be {2} characters long (spaces not counted), " +
                                "but this IBAN is {3} characters long.")]
        public const String ERR_IBAN_WRONGLENGTH = "PARTN.00025V";

        /// <summary>IBAN must pass checksum check</summary>
        [ErrCodeAttribute("Invalid IBAN.",
             ErrorMessageText = "The IBAN is incorrect (checksum mismatch).")
        ]
        public const String ERR_IBAN_CHECKSUMMISMATCH = "PARTN.00026V";

        /// <summary>IBAN must pass checksum check</summary>
        [ErrCodeAttribute("Invalid Account Number.",
             ErrorMessageText = "The Bank Account Number is not valid.")
        ]
        public const String ERR_ACCOUNTNUMBER_INVALID = "PARTN.00027V";

        #endregion

        /// <summary>Partner Interest: Level needs to be within valid range.</summary>
        [ErrCodeAttribute("Level must be within valid range.",
             ErrorMessageText = "Level must be between {0} and {1}.")]
        public const String ERR_INTEREST_LEVEL_NOT_WITHIN_RANGE = "PARTN.00015V";

        /// <summary>Partner of Partner Class Church: Denomination must be assigned, but no Denominations are set up to choose from.</summary>
        [ErrCodeAttribute("Denominations must be set up.",
             ErrorMessageText =
                 "A Denomination must be assigned, but there are no Denominations set up to choose from. Please set up Denominations and then repeat the process!")
        ]
        public const String ERR_NO_DENOMINATIONS_SET_UP = "PARTN.00018V";

        #endregion

        #region Personnel Module-specific error codes

        /// <summary>Duplicate application for event.</summary>
        [ErrCodeAttribute("Duplicate application for event",
             ErrorMessageText = "An application for event {0} already exists!\r\n" +
                                "Please choose a different event.",
             ErrorMessageTitle = "Duplicate application for event")]
        public const String ERR_APPLICATION_DUPLICATE_EVENT = "PES.00001V";

        /// <summary>Passport Name must contain an opening and a closing parenthesis.</summary>
        [ErrCodeAttribute("Passport Name must contain an opening and a closing parenthesis.",
             ErrorMessageText = "The Family Name/Last Name of the Passport Name must be enclosed\r\n" +
                                "in parenthesis ['(' and ')'] but one or both parenthesis are missing!\r\n" +
                                "  Correct Example: Joseph (Meyer)",
             ErrorMessageTitle = "Invalid Passport Name")]
        public const String ERR_INDIV_DATA_PASSPORT_NAME_MISSING_PARAS = "PES.00002V";

        #endregion

        #region SysMan Module-specific error codes

        /// <summary>Password missing.</summary>
        [ErrCodeAttribute("Missing password.",
             ErrorMessageText = "You must create a new password for user {0}.",
             ErrorMessageTitle = "Missing password")]
        public const String ERR_MISSING_PASSWORD = "SYS.00001V";

        /// <summary>Password missing.</summary>
        [ErrCodeAttribute("Invalid password.",
             ErrorMessageText = "Your password must have at least {0} characters, and must contain at least one digit and one letter.",
             ErrorMessageTitle = "Invalid password")]
        public const String ERR_INVALID_PASSWORD = "SYS.00002V";

        #endregion
    }
}