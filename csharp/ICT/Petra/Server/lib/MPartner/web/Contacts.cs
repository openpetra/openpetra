//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, andreww
//
// Copyright 2004-2014 by OM International
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
// generateNamespaceMap-Link-Extra-DLL System.Data.DataSetExtensions;
using System.Data.Odbc;
using System.Linq;
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
        ///
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="APartnerKeys"></param>
        /// <param name="attributeCode"></param>
        /// <param name="attributeDetailCode"></param>
        [RequireModulePermission("PTNRUSER")]
        public static void AddContactAttributeToContacts(int contactId,
            List <Int64>APartnerKeys,
            List <int>attributeCode,
            List <int>attributeDetailCode)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PPartnerContactAttributeTable attributes = new PPartnerContactAttributeTable();

                for (int i = 0; i < APartnerKeys.Count; i++)
                {
                    PPartnerContactAttributeRow row = attributes.NewRowTyped();
                    row.ContactId = contactId;
                }

                PPartnerContactAttributeAccess.SubmitChanges(attributes, WriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured while associating a Contact with a ContactAttribute:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        /// <summary>
        /// Adds a Contact Log record to each Partner in the given Extract
        /// </summary>
        /// <param name="ExtractId"></param>
        /// <param name="ContactLogTable"></param>
        [RequireModulePermission("PTNRUSER")]
        public static void AddContactLog(int ExtractId, ref PContactLogTable ContactLogTable)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                var extractTable = MExtractAccess.LoadViaMExtractMaster(ExtractId, WriteTransaction).AsEnumerable();
                var partnerKeys = extractTable.Select(e => e.ItemArray[MExtractTable.ColumnPartnerKeyId]);

                long ContactLogId = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_contact", WriteTransaction);

                ContactLogTable.Rows[0][PContactLogTable.ColumnContactLogIdId] = ContactLogId;

                PPartnerContactTable partnerContacts = new PPartnerContactTable();
                partnerKeys.ToList().ForEach(partnerKey =>
                {
                    PPartnerContactRow partnerContact = partnerContacts.NewRowTyped();
                    partnerContact.ContactLogId = ContactLogId;
                    partnerContact.PartnerKey = (long)partnerKey;
                    partnerContacts.Rows.Add(partnerContact);
                });

                PContactLogAccess.SubmitChanges(ContactLogTable, WriteTransaction);
                PPartnerContactAccess.SubmitChanges(partnerContacts, WriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                TLogging.Log(
                    "An Exception occured during the adding of a Contact to Partners in an Extract:" + 
                    Environment.NewLine + e.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
                throw;
            }

        }

        /// <summary>
        /// this is useful when applying contact details to a group of people at the same time
        /// </summary>
        /// <param name="APartnerKeys"></param>
        /// <param name="AContactDate"></param>
        /// <param name="AContactor"></param>
        /// <param name="AMethodOfContact"></param>
        /// <param name="AComment"></param>
        /// <param name="AModuleID"></param>
        /// <param name="AMailingCode"></param>
        [RequireModulePermission("PTNRUSER")]
        public static void AddContactLog(List <Int64>APartnerKeys,
            DateTime AContactDate,
            string AContactor,
            string AMethodOfContact,
            string AComment,
            string AModuleID,
            string AMailingCode)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PContactLogTable contacts = new PContactLogTable();
                PContactLogRow contact = contacts.NewRowTyped();
                contact.ContactLogId = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_contact", WriteTransaction);
                contact.ContactDate = new DateTime(AContactDate.Year, AContactDate.Month, AContactDate.Day);
                //contact.ContactTime = AContactDate.Hour * 60 + AContactDate.Minute;
                contact.ContactCode = AMethodOfContact;
                contact.ContactComment = AComment;
                contact.ModuleId = AModuleID;
                contact.Contactor = AContactor;
                contact.UserId = UserInfo.GUserInfo.UserID;
                contacts.Rows.Add(contact);

                if (AMailingCode.Length > 0)
                {
                    contact.MailingCode = AMailingCode;
                }

                // TODO: restrictions implemented via p_restricted_l or s_user_id_c

                PPartnerContactTable partnerContacts = new PPartnerContactTable();
                APartnerKeys.ForEach(partnerKey =>
                    {
                        PPartnerContactRow partnerContact = partnerContacts.NewRowTyped();
                        partnerContact.ContactLogId = contact.ContactLogId;
                        partnerContact.PartnerKey = partnerKey;
                        partnerContacts.Rows.Add(partnerContact);
                    });

                PContactLogAccess.SubmitChanges(contacts, WriteTransaction);
                PPartnerContactAccess.SubmitChanges(partnerContacts, WriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the adding of a Contact:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
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
        public static PContactLogTable FindContacts(string AContactor,
            DateTime? AContactDate,
            string ACommentContains,
            string AMethodOfContact,
            string AModuleID,
            string AMailingCode)
        {
            Boolean NewTransaction;
            PContactLogTable contacts = new PContactLogTable();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PContactLogTable TempTable = new PContactLogTable();
                PContactLogRow TemplateRow = TempTable.NewRowTyped(false);

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

                contacts = PContactLogAccess.LoadUsingTemplate(TemplateRow, WriteTransaction);

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
        /// This returns all Contact Logs associated with a particular partner.
        /// </summary>
        /// <param name="partnerKey"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PContactLogTable FindContactLogsForPartner(long partnerKey)
        {
            Boolean NewTransaction;
            PContactLogTable contacts = new PContactLogTable();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PContactLogTable TempTable = new PContactLogTable();
                PContactLogRow TemplateRow = TempTable.NewRowTyped(false);

                contacts = PContactLogAccess.LoadViaPPartnerPPartnerContact(partnerKey, WriteTransaction);
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
        /// This returns the PartnerContact records
        /// </summary>
        /// <param name="partnerKey"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PPartnerContactTable GetPartnerContacts(long partnerKey)
        {
            Boolean NewTransaction;
            PPartnerContactTable partnerContacts = new PPartnerContactTable();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                PContactLogTable TempTable = new PContactLogTable();
                PContactLogRow TemplateRow = TempTable.NewRowTyped(false);

                partnerContacts = PPartnerContactAccess.LoadViaPPartner(partnerKey, WriteTransaction);
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

            return partnerContacts;
        }

        /// <summary>
        /// Determines whether this ContactLog is associated with more than one Partner
        /// </summary>
        /// <param name="contactLogId"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool IsContactLogAssociatedWithMoreThanOnePartner(long contactLogId)
        {
            Boolean NewTransaction;
            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            bool returnValue = PPartnerContactAccess.CountViaPContactLog(contactLogId, WriteTransaction) > 1;

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return returnValue;
        }

        /// <summary>
        /// delete all contacts that have been marked for deletion.
        /// this should help when something went wrong and needs to be corrected
        /// </summary>
        /// <param name="AContactLogs">table with deleted rows. edited or untouched rows will not be deleted.</param>
        [RequireModulePermission("PTNRUSER")]
        public static void DeleteContacts(
            PContactLogTable AContactLogs)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                foreach (PContactLogRow contactLogRow in AContactLogs.Rows)
                {
                    var contactLogs = PPartnerContactAccess.LoadViaPContactLog((long)contactLogRow[PContactLogTable.ColumnContactLogIdId],
                        WriteTransaction);

                    foreach (PPartnerContactRow partnerContactRow in contactLogs.Rows)
                    {
                        partnerContactRow.Delete();
                    }

                    PPartnerContactAccess.SubmitChanges(contactLogs, WriteTransaction);
                    contactLogRow.Delete();
                }

                PContactLogAccess.SubmitChanges(AContactLogs, WriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the deletion of Contact Logs:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }
    }
}