//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// store and maintain all contact details with partners, eg. phone calls, letters sent/received, emails, publications sent, etc
    /// </summary>
    public class TContactsWebConnector
    {
        /// <summary>
        /// this is useful when applying contact details to a group of people at the same time
        /// </summary>
        /// <param name="APartnerKeys"></param>
        /// <param name="AContactDate"></param>
        /// <param name="AMethodOfContact"></param>
        /// <param name="AComment"></param>
        /// <param name="AModuleID"></param>
        /// <param name="AMailingCode">can be empty string</param>
        /// <param name="AVerificationResults"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool AddContact(List <Int64>APartnerKeys,
            DateTime AContactDate,
            string AMethodOfContact,
            string AComment,
            string AModuleID,
            string AMailingCode,
            out TVerificationResultCollection AVerificationResults)
        {
            Boolean NewTransaction;
            Boolean ResultValue = false;

            AVerificationResults = null;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PPartnerContactTable contacts = new PPartnerContactTable();

                foreach (Int64 partnerKey in APartnerKeys)
                {
                    PPartnerContactRow contact = contacts.NewRowTyped();
                    contact.ContactId = (contacts.Count + 1) * -1;
                    contact.PartnerKey = partnerKey;
                    contact.ContactDate = new DateTime(AContactDate.Year, AContactDate.Month, AContactDate.Day);
                    contact.ContactTime = AContactDate.Hour * 60 + AContactDate.Minute;
                    contact.ContactCode = AMethodOfContact;
                    contact.ContactComment = AComment;
                    contact.ModuleId = AModuleID;
                    contact.Contactor = UserInfo.GUserInfo.UserID;

                    if (AMailingCode.Length > 0)
                    {
                        contact.MailingCode = AMailingCode;
                    }

                    // TODO: restrictions implemented via p_restricted_l or s_user_id_c

                    contacts.Rows.Add(contact);
                }

                if (!PPartnerContactAccess.SubmitChanges(contacts, WriteTransaction, out AVerificationResults))
                {
                    // something went wrong
                    ResultValue = false;
                }
                else
                {
                    ResultValue = true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
            }

            if (ResultValue && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// get all contacts that meet the given criteria.
        /// </summary>
        /// <param name="AContactor">user id of the person who made the contact</param>
        /// <param name="AContactDate">only the date will be used, the time is not considered</param>
        /// <param name="ACommentContains">can be an empty string</param>
        /// <param name="AMethodOfContact"></param>
        /// <param name="AModuleID"></param>
        /// <param name="AMailingCode">can be an empty string</param>
        /// <returns>the contacts table with all contacts that match</returns>
        [RequireModulePermission("PTNRUSER")]
        public static PPartnerContactTable FindContacts(string AContactor,
            DateTime? AContactDate,
            string ACommentContains,
            string AMethodOfContact,
            string AModuleID,
            string AMailingCode)
        {
            Boolean NewTransaction;
            PPartnerContactTable contacts = new PPartnerContactTable();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PPartnerContactTable TempTable = new PPartnerContactTable();
                PPartnerContactRow TemplateRow = TempTable.NewRowTyped(false);

                if (AContactor.Length > 0)
                {
                    TemplateRow.Contactor = AContactor;
                }

                if (AContactDate.HasValue)
                {
                    TemplateRow.ContactDate = new DateTime(AContactDate.Value.Year, AContactDate.Value.Month, AContactDate.Value.Day);
                }

                if (AMethodOfContact.Length > 0)
                {
                    TemplateRow.ContactCode = AMethodOfContact;
                }

                if (AModuleID.Length > 0)
                {
                    TemplateRow.ModuleId = AModuleID;
                }

                if (AMailingCode.Length > 0)
                {
                    TemplateRow.MailingCode = AMailingCode;
                }

                contacts = PPartnerContactAccess.LoadUsingTemplate(TemplateRow, WriteTransaction);

                Int32 Counter = 0;

                while (Counter < contacts.Rows.Count)
                {
                    if ((ACommentContains.Length > 0) && !StringHelper.ContainsI(contacts[Counter].ContactComment, ACommentContains))
                    {
                        contacts.Rows.RemoveAt(Counter);
                    }
                    else
                    {
                        Counter++;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return contacts;
        }

        /// <summary>
        /// delete all contacts that have been marked for deletion.
        /// this should help when something went wrong and needs to be corrected
        /// </summary>
        /// <param name="APartnerContacts">table with deleted rows. edited or untouched rows will not be deleted.</param>
        /// <param name="AVerificationResults"></param>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeleteContacts(
            PPartnerContactTable APartnerContacts,
            out TVerificationResultCollection AVerificationResults)
        {
            Boolean NewTransaction;
            bool ResultValue = false;

            AVerificationResults = null;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                Int32 Counter = 0;

                while (Counter < APartnerContacts.Rows.Count)
                {
                    // remove all rows from the table that are not deleted
                    if (APartnerContacts.Rows[Counter].RowState != DataRowState.Deleted)
                    {
                        APartnerContacts.Rows.RemoveAt(Counter);
                    }
                    else
                    {
                        Counter++;
                    }
                }

                if (!PPartnerContactAccess.SubmitChanges(APartnerContacts, WriteTransaction, out AVerificationResults))
                {
                    // something went wrong
                    ResultValue = false;
                }
                else
                {
                    ResultValue = true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
            }

            if ((ResultValue == true) && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }
    }
}