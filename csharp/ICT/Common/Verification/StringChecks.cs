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
using System.Text.RegularExpressions;
using Ict.Common.Verification;
using GNU.Gettext;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for String verifications
    ///
    /// </summary>
    public class TStringChecks : System.Object
    {
        /// <summary>
        /// Validates an e-mail address (logical only)
        ///
        /// @comment The returned TVerificationResult has a generic FResultCode set,
        /// 'X_00??'. This should be overridden by the calling procedure to supply a
        /// fitting result code.
        ///
        /// </summary>
        /// <param name="AEmailAddress">E-mail address that should be verified</param>
        /// <param name="AAllowMoreThanOneEMailAddress">Set this to true if more than one e-mail
        /// Address can be contained in AEmailAddress. If this is set to true, all
        /// contained e-mail Addresses will be validated. Recognized separators are
        /// comma and semicolon (',' and ';'). If this is set to false and if more than
        /// one e-mail Address is contained in AEmailAddress, a TVerificationResult
        /// will be generated - even if the e-mail Addresses are correct!</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult ValidateEmail(String AEmailAddress, Boolean AAllowMoreThanOneEMailAddress)
        {
            TVerificationResult ReturnValue = null;

            System.Text.RegularExpressions.Regex RegEx;
            String SeparatorWarning;
            String[] EmailAddresses;
            String EmailAddressPart;
            String EmailAddressPartInfo = "";

            /*
             *   RegEx taken from http://www.regexlib.com/REDetails.aspx?regexp_id=711
             *   This accepts RFC 2822 email addresses in the form: blah@blah.com OR
             *   Blah <blah@blah.com> RFC 2822 email 'mailbox': mailbox = name-addr | addr-spec name-addr = [display-name] "<" addr-spec ">" addr-spec = local-part "@" domain domain = rfc2821domain | rfc2821domain-literal local-part conforms to RFC
             * 2822. domain is either: An rfc 2821 domain (EXCEPT that the final sub-domain must consist of 2 or more letters only). OR An rfc 2821 address-literal. (Note, no attempt is made to fully validate an IPv6 address-literal.) Notes: This
             * pattern uses (.NET/Perl only?) features lookahead "(?<name>)" and alternation/IF (?(name)). RFC 2822 (and 822) do allow embedded comments, whitespace, and newlines within *some* parts of an email address, but this pattern above DOES
             * NOT. RFC 2822 (and 822) allow the domain to be a simple domain with NO ".", but this pattern requires a compound domain at least one "." in the domain name, as per RFC 2821 (4.1.2). RFC 2822 allows/disallows certain whitespace
             * characters in parts of an
             *   email address, such as TAB, CR, LF BUT the pattern above does NOT test for these, and assumes that they are not present in the string (on the basis that these characters are hard to enter into an edit box).
             */
            RegEx = new System.Text.RegularExpressions.Regex(
                @"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$");

            // alternative RegEx for emailchecks:
            // MSDN Example: '^([\w\.]+)@((\[[09]1,3\.[09]1,3\.[09]1,3\.)|(([\w]+\.)+))([azAZ]2,4|[09]1,3)(\]?)$'
            // simple email check: '^\S+@\S+\.\S+$'

            if (AEmailAddress != "")
            {
                EmailAddresses = AEmailAddress.Split(",;".ToCharArray());

                if (EmailAddresses.Length > 1)
                {
                    if (!AAllowMoreThanOneEMailAddress)
                    {
                        SeparatorWarning = Environment.NewLine +
                                           "Reason: The e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses. However, in this case only ONE e-mail address is allowed!";
                        ReturnValue = new TVerificationResult("",
                            "The e-mail address '" +
                            AEmailAddress +
                            "' is not valid." +
                            SeparatorWarning,
                            Catalog.GetString("Invalid Data"),
                            "X_00??",
                            TResultSeverity.Resv_Critical);
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
                                                       "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address (but they can be used to separate several e-mail addresses).";
                                }
                                else
                                {
                                    SeparatorWarning = "";
                                }

                                EmailAddressPart = EmailAddresses[Counter].Trim();

                                if (EmailAddressPart != "")
                                {
                                    EmailAddressPartInfo = " (or a part of it, '" +
                                                           EmailAddresses[Counter].Trim() + "')";
                                }

                                ReturnValue = new TVerificationResult("",
                                    "The e-mail address '" +
                                    AEmailAddress + "'" +
                                    EmailAddressPartInfo +
                                    " is not valid." +
                                    SeparatorWarning,
                                    Catalog.GetString("Invalid Data"),
                                    "X_00??",
                                    TResultSeverity.Resv_Critical);
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
                        ReturnValue = new TVerificationResult("",
                            "The e-mail address '" +
                            AEmailAddress +
                            "' is not valid.",
                            Catalog.GetString("Invalid Data"),
                            "X_00??",
                            TResultSeverity.Resv_Critical);
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload for ValidateEmail, don't allow more than one email address
        /// </summary>
        /// <param name="AEmailAddress"></param>
        /// <returns></returns>
        public static TVerificationResult ValidateEmail(String AEmailAddress)
        {
            return ValidateEmail(AEmailAddress, false);
        }

        /// <summary>
        /// Validates 2 strings, if they are in correct order
        ///
        /// </summary>
        /// <param name="ATxt1">the first string; it is supposed to be lesser or equal than ATxt2</param>
        /// <param name="ATxt2">the second string; it is supposed to be greater or equal than ATxt1</param>
        /// <param name="AFirstDescription">name of the first value</param>
        /// <param name="ASecondDescription">name of the second value</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult FirstLesserEqualThanSecond(String ATxt1, String ATxt2, String AFirstDescription, String ASecondDescription)
        {
            TVerificationResult ReturnValue;

            if (System.String.Compare(ATxt1, ATxt2, false) <= 0)
            {
                // Lexical comparision results in: ATxt1 <= ATxt2
                ReturnValue = null;
            }
            else
            {
                // TODO 2 ochristiank cInternationalisation : Read text items from resource files.
                ReturnValue = new TVerificationResult("", "Invalid Order." +
                    Environment.NewLine +
                    AFirstDescription +
                    " cannot be greater than " +
                    ASecondDescription + '.',
                    "Invalid Data entered",
                    "X_0041",
                    TResultSeverity.Resv_Critical);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Validates a string that should contain an integer
        ///
        /// </summary>
        /// <param name="AText">text that should be verified</param>
        /// <param name="ADescription">describe the text field</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult ValidateStrToInt(String AText, String ADescription)
        {
            TVerificationResult ReturnValue = null;

            try
            {
                if (AText.Trim().Length == 0)
                {
                    throw new Exception();
                }

                Convert.ToInt64(AText);
            }
            catch (Exception)
            {
                ReturnValue = new TVerificationResult("",
                    "You need to enter a valid integer number " +
                    Environment.NewLine +
                    "for '" +
                    ADescription + "'.",
                    "Invalid Data entered",
                    "X_0041",
                    TResultSeverity.Resv_Critical);
            }

            return ReturnValue;
        }
    }
}