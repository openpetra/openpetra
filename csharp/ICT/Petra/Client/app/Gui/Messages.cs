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
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MPartner;
using System.Diagnostics;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains Messages which can be used anywhere in the Petra Client. They are not
    /// specific for a certain Petra Module.
    /// </summary>
    public class TMessages
    {
        /// <summary>todoComment</summary>
        public const String StrMessageNumber = "Message Number: ";

        /// <summary>todoComment</summary>
        public const String StrContext = "Context: ";

        /// <summary>todoComment</summary>
        public const String StrRelease = "Release: ";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyTitle = "Unable to Save - Concurrent Activity Detected";

        /// <summary>Notice to translators: when translating the following resourcestrings, be very careful that the message that the user gets displayed does still make sense and is still correct!!!
        /// (The strings are replaced in quite different ways depending
        /// on the situation.)</summary>
        public const String StrDBConcurrencyOtherUser = "Another user {0} has {1} the same record in Table '{2}' after you have" + "\r\n" +
                                                        "{3} it.{4}" + "\r\n" + "\r\n" +
                                                        "None of your changes can be saved, since the other user's changes could be overwritten." +
                                                        "\r\n" + "\r\n";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencySelf = "You have tried to {1} the same record in Table '{2}' after you have" + "\r\n" + "{3}{4}." +
                                                   "\r\n" + "\r\n" +
                                                   "None of your current changes can be saved, since these changes could potentially" + "\r\n" +
                                                   "conflict with each other." +
                                                   "\r\n" + "\r\n";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyActionsRequired = "Actions required:" + "\r\n" +
                                                              "  * If you were in a screen: please close the screen and re-open it again." + "\r\n" +
                                                              "  * You will need to repeat your changes and save them again.";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyWrittenSelfAction = "modified";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyWrittenOthersAction = "modified and saved";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyWriteOthersAction = "modify and save";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDeletedSelfAction1 = "opened and before you deleted";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDeletedSelfAction2 = "deleted it somewhere else";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDeletedOthersAction = "deleted";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDeleteOthersAction = "delete";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDateInfoOthersAction = " The other user has done this on {0}.";

        /// <summary>todoComment</summary>
        public const String StrDBConcurrencyDateInfoSelfAction = " it in another place on {0}";

        #region TMessages

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessageNumber"></param>
        /// <param name="AContext"></param>
        /// <returns></returns>
        public static String BuildMessageFooter(String AMessageNumber, String AContext)
        {
            return Environment.NewLine + Environment.NewLine + StrMessageNumber + AMessageNumber + Environment.NewLine + StrContext + AContext +
                   Environment.NewLine + StrRelease + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AErrorText"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageNumber"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgGeneralError(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesException)
        {
            if (ATypeWhichRaisesException == null)
            {
                ATypeWhichRaisesException = new StackTrace(false).GetFrame(3).GetMethod().DeclaringType;
            }

            MessageBox.Show(AErrorText + BuildMessageFooter(AMessageNumber,
                    ATypeWhichRaisesException.GetType().ToString()), ATitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AErrorText"></param>
        /// <param name="ATitle"></param>
        /// <param name="AMessageNumber"></param>
        public static void MsgGeneralError(String AErrorText, String ATitle, String AMessageNumber)
        {
            MsgGeneralError(AErrorText, ATitle, AMessageNumber, null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgSecurityException(ESecurityGroupAccessDeniedException AException, System.Type ATypeWhichRaisesException)
        {
            MessageBox.Show(AException.Message +
                BuildMessageFooter(ErrorCodes.PETRAERRORCODE_NOPERMISSIONTOACCESSGROUP,
                    ATypeWhichRaisesException.GetType().ToString()), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgSecurityException(ESecurityAccessDeniedException AException, System.Type ATypeWhichRaisesException)
        {
            MessageBox.Show(AException.Message +
                BuildMessageFooter(ErrorCodes.PETRAERRORCODE_NOPERMISSIONTOACCESSMODULE,
                    ATypeWhichRaisesException.GetType().ToString()), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgSecurityException(ESecurityDBTableAccessDeniedException AException, System.Type ATypeWhichRaisesException)
        {
            String TableLabelName = StringHelper.UpperCamelCase(AException.DBTable);

            MessageBox.Show(String.Format(Catalog.GetString("You do not have permission to {0} {1} records."), AException.AccessRight,
                    TableLabelName) + BuildMessageFooter(ErrorCodes.PETRAERRORCODE_NOPERMISSIONTOACCESSTABLE,
                    ATypeWhichRaisesException.GetType().ToString()), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgSecurityException(ESecurityPartnerAccessDeniedException AException, System.Type ATypeWhichRaisesException)
        {
            MessageBox.Show(MsgSecurityExceptionString(AException, ATypeWhichRaisesException), Catalog.GetString(
                    "Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static string MsgSecurityExceptionString(ESecurityPartnerAccessDeniedException AException, System.Type ATypeWhichRaisesException)
        {
            const string MESSAGE_ACCESS_DENIED = "Access to Partner {0}denied.";
            string SpecificMessageText;
            TPartnerAccessLevelEnum AccessLevel;

            AccessLevel = (TPartnerAccessLevelEnum)Enum.Parse(
                typeof(TPartnerAccessLevelEnum),
                Enum.GetName(typeof(TPartnerAccessLevelEnum), AException.AccessLevel));

            SpecificMessageText = String.Format(MESSAGE_ACCESS_DENIED,
                Environment.NewLine + "    " + AException.PartnerShortName +
                " [" + AException.PartnerKey.ToString() + "]" + Environment.NewLine);

            switch (AccessLevel)
            {
                case TPartnerAccessLevelEnum.palRestrictedToUser:
                case TPartnerAccessLevelEnum.palRestrictedToGroup:
                    SpecificMessageText = SpecificMessageText + Environment.NewLine + Environment.NewLine +
                                          "Reason: Partner is a Private Partner of another user.";
                    break;

                case TPartnerAccessLevelEnum.palRestrictedByFoundationOwnership:
                    SpecificMessageText = SpecificMessageText + Environment.NewLine + Environment.NewLine +
                                          "Reason: You must be the owner of a foundation in order to edit it.";
                    break;
            }

            return SpecificMessageText;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgDBConcurrencyException(EDBConcurrencyException AException, System.Type ATypeWhichRaisesException)
        {
            String TableLabelName;
            String UsersAction;
            String OtherUsersAction;
            String DateInfo;
            String MessageString;

            object[] ReplacePlaceholdersArray;
            TableLabelName = StringHelper.UpperCamelCase(AException.DBTable);
            ReplacePlaceholdersArray = new Object[5];

            if (AException.DBOperation == "write")
            {
                UsersAction = StrDBConcurrencyWrittenSelfAction;

                if (AException.LastModificationUser != UserInfo.GUserInfo.UserID)
                {
                    OtherUsersAction = StrDBConcurrencyWrittenOthersAction;
                    DateInfo = StrDBConcurrencyDateInfoOthersAction;
                }
                else
                {
                    OtherUsersAction = StrDBConcurrencyWriteOthersAction;
                    DateInfo = StrDBConcurrencyDateInfoSelfAction;
                }

                ReplacePlaceholdersArray[0] = "('" + AException.LastModificationUser + "')";
            }
            else if (AException.DBOperation == "delete")
            {
                if (AException.LastModificationUser != UserInfo.GUserInfo.UserID)
                {
                    UsersAction = StrDBConcurrencyDeletedSelfAction1;
                    OtherUsersAction = StrDBConcurrencyDeletedOthersAction;
                }
                else
                {
                    UsersAction = StrDBConcurrencyDeletedSelfAction2;
                    OtherUsersAction = StrDBConcurrencyDeleteOthersAction;
                }

                ReplacePlaceholdersArray[0] = "";
                DateInfo = "";
            }
            else
            {
                // Fallback
                UsersAction = StrDBConcurrencyWrittenSelfAction;

                if (AException.LastModificationUser != UserInfo.GUserInfo.UserID)
                {
                    OtherUsersAction = StrDBConcurrencyWrittenOthersAction;
                    DateInfo = StrDBConcurrencyDateInfoOthersAction;
                }
                else
                {
                    OtherUsersAction = StrDBConcurrencyWriteOthersAction;
                    DateInfo = StrDBConcurrencyDateInfoSelfAction;
                }

                ReplacePlaceholdersArray[0] = "('" + AException.LastModificationUser + "') ";
            }

            if (DateInfo != "")
            {
                // Format the Date with time only if time isn't 00:00:00
                if (AException.LastModification.Date.TimeOfDay != DateTime.Now.Date.TimeOfDay)
                {
                    DateInfo = String.Format(DateInfo, StringHelper.DateToLocalizedString(AException.LastModification, true, true));
                }
                else
                {
                    DateInfo = String.Format(DateInfo, StringHelper.DateToLocalizedString(AException.LastModification));
                }
            }

            ReplacePlaceholdersArray[1] = OtherUsersAction;
            ReplacePlaceholdersArray[2] = TableLabelName;
            ReplacePlaceholdersArray[3] = UsersAction;
            ReplacePlaceholdersArray[4] = DateInfo;

            // MessageBox.Show('ReplacePlaceholdersArray[0]: ' + ReplacePlaceholdersArray[0].ToString + "\r\n" +
            // 'ReplacePlaceholdersArray[1]: ' + ReplacePlaceholdersArray[1].ToString + "\r\n" +
            // 'ReplacePlaceholdersArray[2]: ' + ReplacePlaceholdersArray[2].ToString + "\r\n" +
            // 'ReplacePlaceholdersArray[3]: ' + ReplacePlaceholdersArray[3].ToString + "\r\n" +
            // 'ReplacePlaceholdersArray[4]: ' + ReplacePlaceholdersArray[4].ToString);
            if (AException.LastModificationUser != UserInfo.GUserInfo.UserID)
            {
                MessageString = StrDBConcurrencyOtherUser;
            }
            else
            {
                MessageString = StrDBConcurrencySelf;
            }

            MessageBox.Show(String.Format(MessageString,
                    ReplacePlaceholdersArray) + StrDBConcurrencyActionsRequired +
                BuildMessageFooter(ErrorCodes.PETRAERRORCODE_CONCURRENTCHANGES,
                    ATypeWhichRaisesException.GetType().ToString()), StrDBConcurrencyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AVerificationResult"></param>
        /// <param name="ATypeWhichRaisesException"></param>
        public static void MsgGeneralError(TVerificationResult AVerificationResult, System.Type ATypeWhichRaisesException)
        {
            MsgGeneralError(AVerificationResult.ResultText,
                AVerificationResult.ResultTextCaption,
                AVerificationResult.ResultCode,
                ATypeWhichRaisesException);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AVerificationResult"></param>
        public static void MsgGeneralError(TVerificationResult AVerificationResult)
        {
            MsgGeneralError(AVerificationResult, null);
        }

        /// <summary>
        /// This routine has been written for thte TxtPetraDate-Class but it can be used anywhere
        /// else to point out that a date has overruned a date limit.
        /// </summary>
        /// <param name="maxDate">The maximal date limit</param>
        public static void DateValueMessageMaxOverrun(DateTime maxDate)
        {
            String strMsg = Catalog.GetString("This date has overruned the limit: {0}");

            MessageBox.Show(String.Format(
                    strMsg, StringHelper.DateToLocalizedString(maxDate)),
                Catalog.GetString("Date overrun ..."), MessageBoxButtons.OK);
        }

        /// <summary>
        /// This routine has been written for thte TxtPetraDate-Class but it can be used anywhere
        /// else to point out that a date has underruned a date limit.
        /// </summary>
        /// <param name="minDate">The minimal date limit</param>
        public static void DateValueMessageMinUnderrun(DateTime minDate)
        {
            String strMsg = Catalog.GetString("This date has underruned the limit: {0}");

            MessageBox.Show(String.Format(
                    strMsg, StringHelper.DateToLocalizedString(minDate)),
                Catalog.GetString("Date underrun ..."), MessageBoxButtons.OK);
        }

        #endregion
    }
}