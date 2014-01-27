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
using System.Windows.Forms;
using System.Diagnostics;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Data;
using Ict.Common.Data.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MPartner;
using GNU.Gettext;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains Messages which can be used anywhere in the Petra Client. They are not
    /// specific for a certain Petra Module.
    /// </summary>
    public class TMessages
    {
        #region Private Message Strings

        #region General

        /// <summary>Message Number</summary>
        private static readonly string StrMessageNumber = Catalog.GetString("Message Number: ");

        /// <summary>Context</summary>
        private static readonly string StrContext = Catalog.GetString("Context: ");

        /// <summary>Release</summary>
        private static readonly string StrRelease = Catalog.GetString("Release: ");

        /// <summary>Shown when the Form contains invalid data.</summary>
        private static readonly string StrInvalidDataNeedsCorrectingTitle = Catalog.GetString("Form Contains Invalid Data!");

        /// <summary>Shown when there are Warnings associated with a Form's data.</summary>
        private static readonly string StrFormDataWarningTitle = Catalog.GetString("Form Data Warning");

        /// <summary>Shown when invalid data was entered.</summary>
        private static readonly string StrInvalidDataTitle = Catalog.GetString("Invalid Data");

        /// <summary>Shown when the Form contains deleted records and at least one of them is referenced in the DB (hence it cannot be deleted).</summary>
        private static readonly string StrDeletionNotPossibleBecauseOfReferencesTitle = Catalog.GetString("Deletion of Record Not Possible!");

        /// <summary>Shown when the Form contains invalid data at a certain point when Data Validation runs (e.g. saving of data, change of context [e.g. switching of Tab]).</summary>
        private static readonly string StrInvalidDataNeedsCorrecting = Catalog.GetString(
            "The operation cannot be performed because the form contains invalid data:");

        /// <summary>Shown when a record contains warnings at the point of changing to a different record.</summary>
        private static readonly string StrRecordChangeInvalidDataWarning = Catalog.GetString(
            "There are warning messages associated with the data in the currently edited record\r\n(you can move away from that record, though):");

        /// <summary>Shown when a record contains invalid data at the point of changing to a different record.</summary>
        private static readonly string StrRecordChangeInvalidDataNeedsCorrecting = Catalog.GetString(
            "The currently edited record contains invalid data:");

        /// <summary>Shown when the Form contains deleted records and at least one of them is referenced in the DB (hence it cannot be deleted).</summary>
        private static readonly string StrDeletionNotPossibleBecauseOfReferences = Catalog.GetString(
            "You tried to delete a record that is referenced from somewhere else; that record can therefore not be deleted.\r\n" +
            "Saving of data is not possible anymore from this screen - please close it to continue your work.");

        /// <summary>Shown when the Form contains invalid data at the point of saving data.</summary>
        private static readonly string StrRecordChangeInvalidDataNeedsCorrectingTitle = Catalog.GetString("Record Contains Invalid Data!");

        #endregion

        #region Database Concurrency

        // Notice to translators: when translating the following resourcestrings which are about database concurrency,
        // be very careful that the message that the user gets displayed does still make sense and is still correct!!!
        // (The strings are replaced in quite different ways depending on the situation.)

        /// <summary>Database Concurrency Message Title.</summary>
        private static readonly string StrDBConcurrencyTitle = Catalog.GetString("Unable to Save - Concurrent Activity Detected");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyOtherUser = Catalog.GetString(
            "Another user {0} has {1} the same record in Table '{2}' after you have" + "\r\n" +
            "{3} it.{4}" + "\r\n" + "\r\n" +
            "None of your changes can be saved, since the other user's changes could be overwritten." +
            "\r\n" + "\r\n");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencySelf = Catalog.GetString(
            "You have tried to {1} the same record in Table\r\n" + "'{2}' after you have {3}{4}." +
            "\r\n" + "\r\n" +
            "None of your current changes can be saved, since these changes could" + "\r\n" +
            "potentially conflict with each other." +
            "\r\n" + "\r\n");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyActionsRequired = Catalog.GetString("Actions required:\r\n" +
            "  * If you were in a screen: please close the screen and re-open it again.\r\n" +
            "  * You will need to repeat your changes and save them again.\r\n" +
            "  * In case the error keeps occuring: please contact your OpenPetra support team.");
        
        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyWrittenSelfAction = Catalog.GetString("modified");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyWrittenOthersAction = Catalog.GetString("modified and saved");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyWriteOthersAction = Catalog.GetString("modify and save");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDeletedSelfAction1 = Catalog.GetString("opened and before you deleted");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDeletedSelfAction2 = Catalog.GetString("deleted it somewhere else");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDeletedOthersAction = Catalog.GetString("deleted");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDeleteOthersAction = Catalog.GetString("delete");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDateInfoOthersAction = Catalog.GetString(" The other user has done this on {0}.");

        /// <summary>Part of a Database Concurrency Message.</summary>
        private static readonly string StrDBConcurrencyDateInfoSelfAction = Catalog.GetString(" it in another place on {0}");
        #endregion

        #endregion


        /// <summary>
        /// Displays a general warning message in a MessageBox.
        /// </summary>
        /// <param name="AErrorText">Error text to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgGeneralWarning(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError)
        {
            MsgGeneralError(AErrorText, ATitle, AMessageNumber, ATypeWhichRaisesError, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays a general error message in a MessageBox.
        /// </summary>
        /// <param name="AVerificationResult">TVerificationResult which contains all the information.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgGeneralError(TVerificationResult AVerificationResult, System.Type ATypeWhichRaisesError)
        {
            MessageBoxIcon MsgBoxIcon = GetMbxIconFromVerificationResult(AVerificationResult);

            MsgGeneralError(AVerificationResult.ResultText,
                AVerificationResult.ResultTextCaption,
                AVerificationResult.ResultCode,
                ATypeWhichRaisesError, MsgBoxIcon);
        }

        /// <summary>
        /// Displays a general error message in a MessageBox.
        /// </summary>
        /// <param name="AErrorText">Error text to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgGeneralError(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError)
        {
            MsgGeneralError(AErrorText, ATitle, AMessageNumber, ATypeWhichRaisesError, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays a general error message in a MessageBox.
        /// </summary>
        /// <param name="AErrorText">Error text to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="AIcon">Icon to show in the MessageBox.</param>
        public static void MsgGeneralError(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError,
            MessageBoxIcon AIcon)
        {
            if (ATypeWhichRaisesError == null)
            {
                ATypeWhichRaisesError = new StackTrace(false).GetFrame(2).GetMethod().DeclaringType;
            }

            // Remove possible trailing double Environment.NewLines
            // (Double Environment.NewLines happen if several error messages are concatenated and they are each separated by double Environment.NewLines)
            if (AErrorText.EndsWith(Environment.NewLine + Environment.NewLine))
            {
                AErrorText = AErrorText.Substring(0, AErrorText.Length - Environment.NewLine.Length);
            }

            string message = AErrorText + BuildMessageFooter(AMessageNumber,
                ATypeWhichRaisesError.Name);

            TLogging.LogAtLevel(1, ATitle + ": " + message);
            MessageBox.Show(message, ATitle, MessageBoxButtons.OK, AIcon);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox.
        /// </summary>
        /// <param name="AVerificationResult">TVerificationResult which contains all the information.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgVerificationError(TVerificationResult AVerificationResult, System.Type ATypeWhichRaisesError)
        {
            MsgVerificationError(AVerificationResult.ResultText,
                AVerificationResult.ResultTextCaption,
                AVerificationResult.ResultCode,
                ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgVerificationError(String AVerificationError, String AMessageNumber, System.Type ATypeWhichRaisesError)
        {
            MsgVerificationError(AVerificationError, String.Empty, AMessageNumber, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgVerificationError(String AVerificationError, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError)
        {
            string Title = ATitle;

            if (Title == String.Empty)
            {
                Title = StrInvalidDataTitle;
            }

            if (ATypeWhichRaisesError == null)
            {
                ATypeWhichRaisesError = new StackTrace(false).GetFrame(2).GetMethod().DeclaringType;
            }

            MsgGeneralError(AVerificationError, Title, AMessageNumber, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the data verification check before a record in a Grid is changed.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgRecordChangeVerificationError(String AVerificationError, System.Type ATypeWhichRaisesError)
        {
            MsgRecordChangeVerificationError(AVerificationError, String.Empty, false, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the data verification check before a record in a Grid is changed.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="AOnlyWarnings">Set this to true if the items in <paramref name="AVerificationError" />
        /// are only warnings.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgRecordChangeVerificationError(String AVerificationError, bool AOnlyWarnings, System.Type ATypeWhichRaisesError)
        {
            MsgRecordChangeVerificationError(AVerificationError, String.Empty, AOnlyWarnings, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the data verification check before a record in a Grid is changed.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="AOnlyWarnings">Set this to true if the items in <paramref name="AVerificationError" />
        /// are only warnings.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgRecordChangeVerificationError(String AVerificationError,
            String AMessageNumber,
            bool AOnlyWarnings,
            System.Type ATypeWhichRaisesError)
        {
            string Heading;
            MessageBoxIcon Icon;

            if (AMessageNumber == String.Empty)
            {
                AMessageNumber = PetraErrorCodes.ERR_GENERAL_VERIFICATION_ERROR;
            }

            if (ATypeWhichRaisesError == null)
            {
                ATypeWhichRaisesError = new StackTrace(false).GetFrame(2).GetMethod().DeclaringType;
            }

            if (AOnlyWarnings)
            {
                Heading = StrRecordChangeInvalidDataWarning;
                Icon = MessageBoxIcon.Warning;
            }
            else
            {
                Heading = StrRecordChangeInvalidDataNeedsCorrecting;
                Icon = MessageBoxIcon.Error;
            }

            MsgGeneralError(Heading + Environment.NewLine + Environment.NewLine +
                AVerificationError, Catalog.GetString(
                    TMessages.StrRecordChangeInvalidDataNeedsCorrectingTitle), AMessageNumber, ATypeWhichRaisesError, Icon);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the final data verification before a Form's data is saved.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgFormSaveVerificationError(String AVerificationError, System.Type ATypeWhichRaisesError)
        {
            MsgFormSaveVerificationError(AVerificationError, String.Empty, false, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the final data verification before a Form's data is saved.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="AOnlyWarnings">Set this to true if the items in <paramref name="AVerificationError" />
        /// are only warnings.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        public static void MsgFormSaveVerificationError(String AVerificationError, bool AOnlyWarnings, System.Type ATypeWhichRaisesError)
        {
            MsgFormSaveVerificationError(AVerificationError, String.Empty, AOnlyWarnings, ATypeWhichRaisesError);
        }

        /// <summary>
        /// Displays a verification error message in a MessageBox. Use this for the final data verification before a Form's data is saved.
        /// </summary>
        /// <param name="AVerificationError">Verification error to be displayed.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="AOnlyWarnings">Set this to true if the items in <paramref name="AVerificationError" />
        /// are only warnings.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="AOnlyRecordDeletionErrorsBecauseOfReference">Set to true if the message only pertains to
        /// the inability to delete (a) record(s) because they are referenced from within the DB.</param>
        public static void MsgFormSaveVerificationError(String AVerificationError,
            String AMessageNumber,
            bool AOnlyWarnings,
            System.Type ATypeWhichRaisesError,
            bool AOnlyRecordDeletionErrorsBecauseOfReference = false)
        {
            string Title;
            string Heading;
            MessageBoxIcon Icon;

            if (AMessageNumber == String.Empty)
            {
                AMessageNumber = PetraErrorCodes.ERR_GENERAL_VERIFICATION_ERROR;
            }

            if (ATypeWhichRaisesError == null)
            {
                ATypeWhichRaisesError = new StackTrace(false).GetFrame(2).GetMethod().DeclaringType;
            }

            if (AOnlyWarnings)
            {
                Title = StrFormDataWarningTitle;
                Heading = Messages.StrWarningsAttention;
                Icon = MessageBoxIcon.Warning;
            }
            else
            {
                if (!AOnlyRecordDeletionErrorsBecauseOfReference)
                {
                    Title = StrInvalidDataNeedsCorrectingTitle;
                    Heading = StrInvalidDataNeedsCorrecting;
                }
                else
                {
                    Title = StrDeletionNotPossibleBecauseOfReferencesTitle;
                    Heading = StrDeletionNotPossibleBecauseOfReferences;
                }

                Icon = MessageBoxIcon.Error;
            }

            MsgGeneralError(Heading + Environment.NewLine + Environment.NewLine +
                AVerificationError, Title, AMessageNumber,
                ATypeWhichRaisesError, Icon);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesError"></param>
        public static void MsgSecurityException(ESecurityGroupAccessDeniedException AException, System.Type ATypeWhichRaisesError)
        {
            MessageBox.Show(AException.Message +
                BuildMessageFooter(PetraErrorCodes.ERR_NOPERMISSIONTOACCESSGROUP,
                    ATypeWhichRaisesError.Name), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesError"></param>
        public static void MsgSecurityException(ESecurityAccessDeniedException AException, System.Type ATypeWhichRaisesError)
        {
            MessageBox.Show(AException.Message +
                BuildMessageFooter(PetraErrorCodes.ERR_NOPERMISSIONTOACCESSMODULE,
                    ATypeWhichRaisesError.Name), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesError"></param>
        public static void MsgSecurityException(ESecurityDBTableAccessDeniedException AException, System.Type ATypeWhichRaisesError)
        {
            String TableLabelName = StringHelper.UpperCamelCase(AException.DBTable);

            MessageBox.Show(String.Format(Catalog.GetString("You do not have permission to {0} {1} records."), AException.AccessRight,
                    TableLabelName) + BuildMessageFooter(PetraErrorCodes.ERR_NOPERMISSIONTOACCESSTABLE,
                    ATypeWhichRaisesError.Name), Catalog.GetString("Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        public static void MsgSecurityException(ESecurityPartnerAccessDeniedException AException)
        {
            MessageBox.Show(MsgSecurityExceptionString(AException), Catalog.GetString(
                    "Security Violation"), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static string MsgSecurityExceptionString(ESecurityPartnerAccessDeniedException AException)
        {
            string StrMessageAccessDenied = Catalog.GetString("Access to Partner {0}denied.");
            string SpecificMessageText;
            TPartnerAccessLevelEnum AccessLevel;

            AccessLevel = (TPartnerAccessLevelEnum)Enum.Parse(
                typeof(TPartnerAccessLevelEnum),
                Enum.GetName(typeof(TPartnerAccessLevelEnum), AException.AccessLevel));

            SpecificMessageText = String.Format(StrMessageAccessDenied,
                Environment.NewLine + "    " + AException.PartnerShortName +
                " [" + AException.PartnerKey.ToString() + "]" + Environment.NewLine);

            switch (AccessLevel)
            {
                case TPartnerAccessLevelEnum.palRestrictedToUser:
                case TPartnerAccessLevelEnum.palRestrictedToGroup:
                    SpecificMessageText = SpecificMessageText + Environment.NewLine + Environment.NewLine +
                                          Catalog.GetString("Reason: Partner is a Private Partner of another user.");
                    break;

                case TPartnerAccessLevelEnum.palRestrictedByFoundationOwnership:
                    SpecificMessageText = SpecificMessageText + Environment.NewLine + Environment.NewLine +
                                          Catalog.GetString("Reason: You must be the owner of a foundation in order to edit it.");
                    break;
            }

            return SpecificMessageText;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ATypeWhichRaisesError"></param>
        public static void MsgDBConcurrencyException(EDBConcurrencyException AException, System.Type ATypeWhichRaisesError)
        {
            String TableLabelName = StringHelper.UpperCamelCase(AException.DBTable);
            String UsersAction;
            String OtherUsersAction;
            String DateInfo;
            String MessageString = String.Empty;
            object[] ReplacePlaceholdersArray = new Object[5];            

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

                ReplacePlaceholdersArray[0] = String.Empty;                
                DateInfo = String.Empty;                
            }
            else if ((AException.DBOperation == "update") 
                && (AException is EDBConcurrencyNoRowToUpdateException))
            {
                MessageString = Catalog.GetString("You have tried to update data, but that data is not present in the OpenPetra database.\r\n" +
                    "This means that either\r\n" +
                    " * this data has been deleted before you tried to update it (presumably by somebody else); or\r\n" +
                    " * that that data wasn't present in the OpenPetra database at all before you tried to update it (this points to a programming error).\r\n\r\n");
                    
                ReplacePlaceholdersArray = null;
                
                DateInfo = String.Empty;
                OtherUsersAction = String.Empty;
                UsersAction = String.Empty;
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

            if (ReplacePlaceholdersArray != null) 
            {
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
                    BuildMessageFooter(PetraErrorCodes.ERR_CONCURRENTCHANGES,
                        ATypeWhichRaisesError.Name), StrDBConcurrencyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            else
            {
                MessageBox.Show(MessageString + StrDBConcurrencyActionsRequired +
                    BuildMessageFooter(PetraErrorCodes.ERR_CONCURRENTCHANGES,
                        ATypeWhichRaisesError.Name), StrDBConcurrencyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                
            }
        }

        /// <summary>
        /// Displays a MessageBox that shows a question and returns the users' choice.
        /// </summary>
        /// <param name="AErrCodeInfo">ErrCodeInfo which contains all the information.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="ADefaultToAnswerYes">Makes 'Yes' the button which is selected by default if true,
        /// otherwise 'No' is selected by default.</param>
        /// <returns>Users' choice.</returns>
        public static DialogResult MsgQuestion(ErrCodeInfo AErrCodeInfo, System.Type ATypeWhichRaisesError, bool ADefaultToAnswerYes)
        {
            return MsgQuestion(new TVerificationResult(ATypeWhichRaisesError, AErrCodeInfo), ATypeWhichRaisesError, ADefaultToAnswerYes);
        }

        /// <summary>
        /// Displays a MessageBox that shows a question and returns the users' choice.
        /// </summary>
        /// <param name="AVerificationResult">TVerificationResult which contains all the information.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="ADefaultToAnswerYes">Makes 'Yes' the button which is selected by default if true,
        /// otherwise 'No' is selected by default.</param>
        /// <returns>Users' choice.</returns>
        public static DialogResult MsgQuestion(TVerificationResult AVerificationResult, System.Type ATypeWhichRaisesError, bool ADefaultToAnswerYes)
        {
            MessageBoxIcon MsgBoxIcon = GetMbxIconFromVerificationResult(AVerificationResult);

            return MsgQuestion(AVerificationResult.ResultText,
                AVerificationResult.ResultTextCaption,
                AVerificationResult.ResultCode,
                ATypeWhichRaisesError, MsgBoxIcon, ADefaultToAnswerYes);
        }

        /// <summary>
        /// Displays a MessageBox that shows a question and returns the users' choice.
        /// </summary>
        /// <param name="AErrorText">Error text to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="ADefaultToAnswerYes">Makes 'Yes' the button which is selected by default if true,
        /// otherwise 'No' is selected by default.</param>
        /// <returns>Users' choice.</returns>
        public static DialogResult MsgQuestion(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError,
            bool ADefaultToAnswerYes)
        {
            return MsgQuestion(AErrorText, ATitle, AMessageNumber, ATypeWhichRaisesError, MessageBoxIcon.Question, ADefaultToAnswerYes);
        }

        /// <summary>
        /// Displays a MessageBox that shows a question and returns the users' choice.
        /// </summary>
        /// <param name="AErrorText">Error text to be displayed.</param>
        /// <param name="ATitle">Title of the MessageBox.</param>
        /// <param name="AMessageNumber">Message Number.</param>
        /// <param name="ATypeWhichRaisesError">Instance of an object which raises the Error.</param>
        /// <param name="AIcon">Icon to show in the MessageBox.</param>
        /// <param name="ADefaultToAnswerYes">Makes 'Yes' the button which is selected by default if true,
        /// otherwise 'No' is selected by default.</param>
        /// <returns>Users' choice.</returns>
        public static DialogResult MsgQuestion(String AErrorText, String ATitle, String AMessageNumber, System.Type ATypeWhichRaisesError,
            MessageBoxIcon AIcon, bool ADefaultToAnswerYes)
        {
            MessageBoxDefaultButton DefaultButton = MessageBoxDefaultButton.Button2;

            if (ATypeWhichRaisesError == null)
            {
                ATypeWhichRaisesError = new StackTrace(false).GetFrame(2).GetMethod().DeclaringType;
            }

            if (ADefaultToAnswerYes)
            {
                DefaultButton = MessageBoxDefaultButton.Button1;
            }

            return MessageBox.Show(AErrorText + BuildMessageFooter(AMessageNumber,
                    ATypeWhichRaisesError.Name), ATitle, MessageBoxButtons.YesNo, AIcon, DefaultButton);
        }

        #region Helper Methods

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMessageNumber"></param>
        /// <param name="AContext"></param>
        /// <returns></returns>
        private static String BuildMessageFooter(String AMessageNumber, String AContext)
        {
            string version = string.Empty;

            if (System.Reflection.Assembly.GetEntryAssembly() != null)
            {
                // for NUnit tests, there is no entry Assebmly
                version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            }

            return Environment.NewLine + Environment.NewLine + StrMessageNumber + AMessageNumber + "     " + StrContext + AContext +
                   Environment.NewLine + StrRelease + version;
        }

        /// <summary>
        /// Returns the matching <see cref="MessageBoxIcon" /> for a <see cref="TVerificationResult" />.
        /// </summary>
        /// <param name="AVerificationResult"><see cref="TVerificationResult" /> to inspect.</param>
        /// <returns><see cref="MessageBoxIcon" /> for the <see cref="TVerificationResult" /> passed in in Argument
        /// <paramref name="AVerificationResult" />.</returns>
        private static MessageBoxIcon GetMbxIconFromVerificationResult(TVerificationResult AVerificationResult)
        {
            if (AVerificationResult.ResultSeverity == TResultSeverity.Resv_Critical)
            {
                return MessageBoxIcon.Error;
            }
            else if (AVerificationResult.ResultSeverity == TResultSeverity.Resv_Info)
            {
                return MessageBoxIcon.Information;
            }
            else
            {
                return MessageBoxIcon.Warning;
            }
        }

        /// <summary>
        /// This routine has been written for thte TxtPetraDate-Class but it can be used anywhere
        /// else to point out that a date has exceeded a date limit.
        /// </summary>
        /// <param name="maxDate">The maximal date limit</param>
        public static void DateValueMessageMaxOverrun(DateTime maxDate)
        {
            String strMsg = Catalog.GetString("This date has exceeded the limit: {0}");

            MessageBox.Show(String.Format(
                    strMsg, StringHelper.DateToLocalizedString(maxDate)),
                Catalog.GetString("Date exceeded ..."), MessageBoxButtons.OK);
        }

        /// <summary>
        /// This routine has been written for thte TxtPetraDate-Class but it can be used anywhere
        /// else to point out that a date is before the date limit.
        /// </summary>
        /// <param name="minDate">The minimal date limit</param>
        public static void DateValueMessageMinUnderrun(DateTime minDate)
        {
            String strMsg = Catalog.GetString("This date is earlier than the date limit: {0}");

            MessageBox.Show(String.Format(
                    strMsg, StringHelper.DateToLocalizedString(minDate)),
                Catalog.GetString("Date underrun ..."), MessageBoxButtons.OK);
        }

        #endregion
    }
}