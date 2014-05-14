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
using System.Data;
using System.Text.RegularExpressions;
using Ict.Common.Verification;
using GNU.Gettext;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for string verifications that are needed both on Server and Client side.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TStringChecks
    {
        /// <summary>Email Address is invalid.</summary>
        [ErrCodeAttribute("Email Address is invalid.",
             FullDescription = "The email address entered is not a valid email address.")]
        public const String ERR_EMAILADDRESSINVALID = "GENC.00017V";

        #region Resourcestrings

        private static readonly string StrEmailAddrContainsEmailSeparator = Catalog.GetString(
            "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.");

        private static readonly string StrEmailAddrContainsEmailSeparatorNotAllowed = Catalog.GetString(
            "Reason: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses. However, in this case only ONE e-mail address is allowed!");

        private static readonly string StrEmailAddNotValid = Catalog.GetString(
            "The e-mail address '{0}' is not valid.");

        private static readonly string StrEmailAddOrPartNotValid = Catalog.GetString(
            "The e-mail address '{0}' (or a part of it, '{1}') is not valid.");

        private static readonly string StrInvalidStringOrder = Catalog.GetString("Invalid Order.\r\n{0} cannot be greater than {1}.");

        private static readonly string StrStringMustNotBeEmpty = Catalog.GetString("A value must be entered for {0}.");

        private static readonly string StrStringTooLong = Catalog.GetString(
            "The value you entered for {0} is too long - its maximum length is {1} characters, but you entered {2} characters.");

        private static readonly string StrStringInvalidValue = Catalog.GetString(
            "The value you entered for {0} is currently inactive.");

        #endregion


        #region StringMustNotBeEmpty

        /// <summary>
        /// Checks whether a strings is null or <see cref="String.Empty" />.  Null values are accepted.
        /// </summary>
        /// <param name="AValue">The string to check.</param>
        /// <param name="ADescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> is not null and not <see cref="String.Empty" />,
        /// otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult StringMustNotBeEmpty(string AValue, string ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            if ((AValue == null)
                || (AValue == String.Empty))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOEMPTYSTRING, CommonResourcestrings.StrInvalidStringEntered + Environment.NewLine +
                        StrStringMustNotBeEmpty, new string[] { Description }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region StringLengthLesserOrEqual

        /// <summary>
        /// Checks whether a strings' length is lesser or equal to the specified amount of characters.  Null values are accepted.
        /// </summary>
        /// <param name="AValue">The string to check.</param>
        /// <param name="APermittedStringLength">The permitted amount of characters.</param>
        /// <param name="ADescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AValue" /> is not null and not <see cref="String.Empty" />,
        /// otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult StringLengthLesserOrEqual(string AValue, int APermittedStringLength, string ADescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            String Description = THelper.NiceValueDescription(ADescription);

            // Check
            if ((AValue != null)
                && (AValue.Length > APermittedStringLength))
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_STRINGTOOLONG, CommonResourcestrings.StrInvalidStringEntered + Environment.NewLine +
                        StrStringTooLong, new string[] { Description, APermittedStringLength.ToString(), AValue.Length.ToString() }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region FirstLesserOrEqualThanSecondString

        /// <summary>
        /// Checks whether two strings are in correct order (lexical comparison).  Null values are accepted.
        /// </summary>
        /// <param name="ATxt1">The first string; it is supposed to be lesser or equal than ATxt2.</param>
        /// <param name="ATxt2">The second string; it is supposed to be greater or equal than ATxt1.</param>
        /// <param name="AFirstDescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="ASecondDescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="ATxt1" /> is lesser or equal than
        /// <paramref name="ATxt2" />, otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="AFirstDescription" />
        /// and <paramref name="ASecondDescription" />.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondString(String ATxt1, String ATxt2,
            String AFirstDescription, String ASecondDescription,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;

            string FirstDescription = THelper.NiceValueDescription(AFirstDescription);
            string SecondDescription = THelper.NiceValueDescription(ASecondDescription);

            // Check
            if (System.String.Compare(ATxt1, ATxt2, false) <= 0)
            {
                // Lexical comparision results in: ATxt1 <= ATxt2
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult(AResultContext,
                    ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS,
                        StrInvalidStringOrder, new string[] { FirstDescription, SecondDescription }));

                if (AResultColumn != null)
                {
                    ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region ValidateValue

        /// <summary>
        /// Used for checking if a particular lookup value relates to an inactive record
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADataTable"></param>
        /// <param name="AKeyValue"></param>
        /// <param name="AActiveColumn"></param>
        /// <param name="AResultContext"></param>
        /// <param name="AResultColumn"></param>
        /// <param name="AResultControl"></param>
        /// <returns></returns>
        public static TVerificationResult ValidateValueIsActive(Int32 ALedgerNumber, DataTable ADataTable, String AKeyValue,
            String AActiveColumn, object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue;

            DataRow foundRow = ADataTable.Rows.Find(new object[] { ALedgerNumber, AKeyValue });

            if (foundRow != null)
            {
                bool isActive = (bool)foundRow[AActiveColumn];

                if (!isActive)
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_INVALIDVALUE,
                            StrStringInvalidValue, new string[] { AKeyValue }));

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
            else
            {
                ReturnValue = null;
            }

            return ReturnValue;
        }

        #endregion


        #region ValidateEmail

        /// <summary>
        /// Checks whether an e-mail address is valid (logical only, no lookup is done).
        /// Does not allow more than one email address in the <paramref name="AEmailAddress" />
        /// Argument.
        /// </summary>
        /// <param name="AEmailAddress">E-mail address that should be verified.</param>
        /// <returns>Null if <paramref name="AEmailAddress" /> contains a valid email address,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about
        /// the problem (also in the case where more than one e-mail Address is contained in
        /// <paramref name="AEmailAddress" /> - even if the e-mail Addresses are correct!).
        /// </returns>
        public static TVerificationResult ValidateEmail(String AEmailAddress)
        {
            return ValidateEmail(AEmailAddress, false);
        }

        /// <summary>
        /// Checks whether an e-mail address is valid (logical only, no lookup is done).
        /// Does not allow more than one email address in the <paramref name="AEmailAddress" />
        /// Argument.
        /// </summary>
        /// <param name="AEmailAddress">E-mail address that should be verified.</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AEmailAddress" /> contains a valid email address,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about
        /// the problem (also in the case where more than one e-mail Address is contained in
        /// <paramref name="AEmailAddress" /> - even if the e-mail Addresses are correct!).
        /// </returns>
        public static TVerificationResult ValidateEmail(String AEmailAddress,
            object AResultContext, System.Data.DataColumn AResultColumn, System.Windows.Forms.Control AResultControl)
        {
            return ValidateEmail(AEmailAddress, false, AResultContext, AResultColumn, AResultControl);
        }

        /// <summary>
        /// Checks whether an e-mail address is valid (logical only, no lookup is done). Multiple email addresses
        /// may be contained in Argument <paramref name="AEmailAddress" /> if Argument
        /// <paramref name="AAllowMoreThanOneEMailAddress" /> is set to true.
        /// </summary>
        /// <param name="AEmailAddress">E-mail address that should be verified.</param>
        /// <param name="AAllowMoreThanOneEMailAddress">Set this to true if more than one e-mail
        /// Address can be contained in AEmailAddress. If this is set to true, all
        /// contained e-mail Addresses will be validated. Recognized separators are
        /// comma and semicolon (',' and ';').</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null).</param>
        /// <returns>Null if <paramref name="AEmailAddress" /> contains a valid email address,
        /// otherwise a <see cref="TVerificationResult" /> is returned that contains details about
        /// the problem (also in the case where <paramref name="AAllowMoreThanOneEMailAddress" />
        /// is set to false and more than one e-mail Address is contained in
        /// <paramref name="AEmailAddress" /> - even if the e-mail Addresses are correct!).
        /// </returns>
        public static TVerificationResult ValidateEmail(String AEmailAddress, Boolean AAllowMoreThanOneEMailAddress,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            System.Text.RegularExpressions.Regex RegEx;
            String SeparatorWarning = String.Empty;
            String[] EmailAddresses;
            String EmailAddressPart;
            String EmailAddressPartInfo = String.Empty;

            /*
             * RegEx taken from http://www.regexlib.com/REDetails.aspx?regexp_id=711
             * This accepts RFC 2822 email addresses in the form: blah@blah.com OR
             * Blah <blah@blah.com> RFC 2822 email 'mailbox': mailbox = name-addr | addr-spec name-addr = [display-name] "<" addr-spec ">" addr-spec = local-part "@" domain domain = rfc2821domain | rfc2821domain-literal local-part conforms to RFC
             * 2822. domain is either: An rfc 2821 domain (EXCEPT that the final sub-domain must consist of 2 or more letters only). OR An rfc 2821 address-literal. (Note, no attempt is made to fully validate an IPv6 address-literal.) Notes: This
             * pattern uses (.NET/Perl only?) features lookahead "(?<name>)" and alternation/IF (?(name)). RFC 2822 (and 822) do allow embedded comments, whitespace, and newlines within *some* parts of an email address, but this pattern above DOES
             * NOT. RFC 2822 (and 822) allow the domain to be a simple domain with NO ".", but this pattern requires a compound domain at least one "." in the domain name, as per RFC 2821 (4.1.2). RFC 2822 allows/disallows certain whitespace
             * characters in parts of an email address, such as TAB, CR, LF BUT the pattern above does NOT test for these, and assumes that they are not present in the string (on the basis that these characters are hard to enter into an edit box).
             */
            RegEx = new System.Text.RegularExpressions.Regex(
                @"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$");

            // alternative RegEx for emailchecks:
            // MSDN Example: '^([\w\.]+)@((\[[09]1,3\.[09]1,3\.[09]1,3\.)|(([\w]+\.)+))([azAZ]2,4|[09]1,3)(\]?)$'
            // simple email check: '^\S+@\S+\.\S+$'

            // Check
            if (AEmailAddress != String.Empty)
            {
                EmailAddresses = StringHelper.SplitEmailAddresses(AEmailAddress);

                if (EmailAddresses.Length > 1)
                {
                    if (!AAllowMoreThanOneEMailAddress)
                    {
                        SeparatorWarning = Environment.NewLine +
                                           StrEmailAddrContainsEmailSeparatorNotAllowed;

                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID, StrEmailAddNotValid + SeparatorWarning,
                                new string[] { AEmailAddress }));
                    }
                    else
                    {
                        for (int Counter = 0; Counter <= EmailAddresses.Length - 1; Counter += 1)
                        {
                            if (!RegEx.IsMatch(EmailAddresses[Counter].Trim()))
                            {
                                if ((AEmailAddress.IndexOf(',') != -1)
                                    || (AEmailAddress.IndexOf(';') != -1))
                                {
                                    SeparatorWarning = Environment.NewLine +
                                                       StrEmailAddrContainsEmailSeparator;
                                }

                                EmailAddressPart = EmailAddresses[Counter].Trim();

                                if (EmailAddressPart != String.Empty)
                                {
                                    EmailAddressPartInfo = EmailAddresses[Counter].Trim();
                                }

                                if (EmailAddressPartInfo != String.Empty)
                                {
                                    ReturnValue = new TVerificationResult(AResultContext,
                                        ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID, StrEmailAddOrPartNotValid +
                                            SeparatorWarning, new string[] { AEmailAddress, EmailAddressPartInfo }));
                                }
                                else
                                {
                                    ReturnValue = new TVerificationResult(AResultContext,
                                        ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID, StrEmailAddNotValid +
                                            SeparatorWarning, new string[] { AEmailAddress }));
                                }

                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (RegEx.IsMatch(AEmailAddress))
                    {
                        ReturnValue = null;
                    }
                    else
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(ERR_EMAILADDRESSINVALID, StrEmailAddNotValid, new string[] { AEmailAddress }));
                    }
                }
            }

            if (AResultColumn != null)
            {
                ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
            }

            return ReturnValue;
        }

        #endregion
    }
}