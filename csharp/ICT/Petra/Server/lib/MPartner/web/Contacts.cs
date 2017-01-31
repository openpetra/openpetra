//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, andreww, peters
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
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    PPartnerContactAttributeTable attributes = new PPartnerContactAttributeTable();

                    for (int i = 0; i < APartnerKeys.Count; i++)
                    {
                        PPartnerContactAttributeRow row = attributes.NewRowTyped();
                        row.ContactId = contactId;
                    }

                    PPartnerContactAttributeAccess.SubmitChanges(attributes, Transaction);

                    SubmissionOK = true;
                });
        }

        /// <summary>
        /// Adds a Contact Log record to each Partner in the given Extract
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AContactLogTable"></param>
        /// <param name="APartnerContactAttributeTable"></param>
        [RequireModulePermission("PTNRUSER")]
        public static void AddContactLog(int AExtractId,
            PContactLogTable AContactLogTable,
            PPartnerContactAttributeTable APartnerContactAttributeTable)
        {
            TDBTransaction WriteTransaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, ref WriteTransaction, ref SubmissionOK,
                delegate
                {
                    var extractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, WriteTransaction).AsEnumerable();
                    var partnerKeys = extractTable.Select(e => e.ItemArray[MExtractTable.ColumnPartnerKeyId]);

                    long ContactLogId = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_contact", WriteTransaction);

                    AContactLogTable.Rows[0][PContactLogTable.ColumnContactLogIdId] = ContactLogId;

                    PPartnerContactTable partnerContacts = new PPartnerContactTable();
                    partnerKeys.ToList().ForEach(partnerKey =>
                        {
                            PPartnerContactRow partnerContact = partnerContacts.NewRowTyped();
                            partnerContact.ContactLogId = ContactLogId;
                            partnerContact.PartnerKey = (long)partnerKey;
                            partnerContacts.Rows.Add(partnerContact);
                        });

                    foreach (PPartnerContactAttributeRow Row in APartnerContactAttributeTable.Rows)
                    {
                        Row.ContactId = ContactLogId;
                    }

                    PContactLogAccess.SubmitChanges(AContactLogTable, WriteTransaction);
                    PPartnerContactAccess.SubmitChanges(partnerContacts, WriteTransaction);
                    PPartnerContactAttributeAccess.SubmitChanges(APartnerContactAttributeTable, WriteTransaction);

                    SubmissionOK = true;
                });
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
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    PContactLogTable contacts = new PContactLogTable();
                    PContactLogRow contact = contacts.NewRowTyped();
                    contact.ContactLogId = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_contact", Transaction);
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

                    PContactLogAccess.SubmitChanges(contacts, Transaction);
                    PPartnerContactAccess.SubmitChanges(partnerContacts, Transaction);

                    SubmissionOK = true;
                });
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
        /// <param name="AContactAttributes">can be an empty table</param>
        /// <returns>the contacts table with all contacts that match</returns>
        [RequireModulePermission("PTNRUSER")]
        public static DataTable FindContacts(string AContactor,
            DateTime? AContactDate,
            string ACommentContains,
            string AMethodOfContact,
            string AModuleID,
            string AMailingCode,
            PPartnerContactAttributeTable AContactAttributes)
        {
            Boolean NewTransaction;
            DataTable Contacts = new DataTable();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                string Query = "SELECT p_contact_log.*, p_partner_contact.p_partner_key_n, p_partner.p_partner_short_name_c" +

                               " FROM p_contact_log, p_partner_contact, p_partner" +

                               " WHERE" +
                               " p_partner_contact.p_contact_log_id_i = p_contact_log.p_contact_log_id_i" +
                               " AND p_partner.p_partner_key_n = p_partner_contact.p_partner_key_n";

                if (AContactor.Length > 0)
                {
                    Query += " AND p_contact_log.p_contactor_c = '" + AContactor + "'";
                }

                if (AContactDate.HasValue)
                {
                    Query += " AND p_contact_log.s_contact_date_d = '" + AContactDate + "'";
                }

                if (AMethodOfContact.Length > 0)
                {
                    Query += " AND p_contact_log.p_contact_code_c = '" + AMethodOfContact + "'";
                }

                if (AModuleID.Length > 0)
                {
                    Query += " AND p_contact_log.s_module_id_c = '" + AModuleID + "'";
                }

                if (AMailingCode.Length > 0)
                {
                    Query += " AND p_contact_log.p_mailing_code_c = '" + AMailingCode + "'";
                }

                if (ACommentContains.Length > 0)
                {
                    Query += " AND p_contact_log.p_contact_comment_c LIKE '%" + ACommentContains + "%'";
                }

                if ((AContactAttributes != null) && (AContactAttributes.Rows.Count > 0))
                {
                    Query += " AND EXISTS (SELECT * " +
                             " FROM p_partner_contact_attribute" +
                             " WHERE" +
                             " p_partner_contact_attribute.p_contact_id_i = p_contact_log.p_contact_log_id_i" +
                             " AND (";

                    foreach (PPartnerContactAttributeRow Row in AContactAttributes.Rows)
                    {
                        Query += " (p_partner_contact_attribute.p_contact_attribute_code_c = '" + Row.ContactAttributeCode + "'" +
                                 " AND p_partner_contact_attribute.p_contact_attr_detail_code_c = '" + Row.ContactAttrDetailCode + "') OR";
                    }

                    // remove the final " OR"
                    Query = Query.Substring(0, Query.Length - 3) + "))";
                }

                DBAccess.GDBAccessObj.SelectDT(Contacts, Query, WriteTransaction);

                Contacts.PrimaryKey = new DataColumn[] {
                    Contacts.Columns["p_partner_key_n"], Contacts.Columns["p_contact_log_id_i"]
                };
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

            return Contacts;
        }

        /// <summary>
        /// This returns all the data needed for the Patner Edit Contact Log tab
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerContactLogData(long APartnerKey)
        {
            PartnerEditTDS ReturnDS = new PartnerEditTDS();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref Transaction,
                delegate
                {
                    ReturnDS.Merge(PContactLogAccess.LoadViaPPartnerPPartnerContact(APartnerKey, Transaction));
                    ReturnDS.Merge(PPartnerContactAccess.LoadViaPPartner(APartnerKey, Transaction));

                    if ((ReturnDS.PContactLog != null) && (ReturnDS.PContactLog.Count > 0))
                    {
                        foreach (PContactLogRow Row in ReturnDS.PContactLog.Rows)
                        {
                            ReturnDS.Merge(PPartnerContactAttributeAccess.LoadViaPContactLog(Row.ContactLogId, Transaction));
                        }
                    }
                });

            return ReturnDS;
        }

        /// <summary>
        /// Determines whether this ContactLog is associated with more than one Partner
        /// </summary>
        /// <param name="AContactLogId"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool IsContactLogAssociatedWithMoreThanOnePartner(long AContactLogId)
        {
            bool returnValue = false;
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    returnValue = IsContactLogAssociatedWithMoreThanOnePartner(AContactLogId, Transaction);
                });

            return returnValue;
        }

        /// <summary>
        /// Determines whether this ContactLog is associated with more than one Partner
        /// </summary>
        /// <param name="AContactLogId"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool IsContactLogAssociatedWithMoreThanOnePartner(long AContactLogId, TDBTransaction ATransaction)
        {
            return PPartnerContactAccess.CountViaPContactLog(AContactLogId, ATransaction) > 1;
        }

        /// <summary>
        /// delete all contacts that have been marked for deletion.
        /// this should help when something went wrong and needs to be corrected
        /// </summary>
        /// <param name="AContactLogs">table with deleted rows. edited or untouched rows will not be deleted.</param>
        [RequireModulePermission("PTNRUSER")]
        public static void DeleteContacts(
            DataTable AContactLogs)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    Boolean LastPartnerForThisContactLog = true;

                    foreach (DataRow contactLogRow in AContactLogs.Rows)
                    {
                        Int64 ContactLogId = Convert.ToInt64(contactLogRow["p_contact_log_id_i"]);
                        Int64 PartnerKey = Convert.ToInt64(contactLogRow["p_partner_key_n"]);

                        LastPartnerForThisContactLog = true;

                        if (IsContactLogAssociatedWithMoreThanOnePartner(ContactLogId, Transaction))
                        {
                            LastPartnerForThisContactLog = false;
                        }

                        PPartnerContactTable contactLogs = PPartnerContactAccess.LoadByPrimaryKey(
                            PartnerKey, ContactLogId, Transaction);

                        contactLogs[0].Delete();

                        PPartnerContactAccess.SubmitChanges(contactLogs, Transaction);

                        if (LastPartnerForThisContactLog)
                        {
                            // now we also need to delete the contact attributes (linked with this contact log)
                            PPartnerContactAttributeRow template = new PPartnerContactAttributeTable().NewRowTyped(false);
                            template.ContactId = ContactLogId;

                            if (PPartnerContactAttributeAccess.CountUsingTemplate(template, null, Transaction) > 0)
                            {
                                PPartnerContactAttributeAccess.DeleteUsingTemplate(template, null, Transaction);
                            }

                            // and the contact log itself needs to be deleted (if no other partner refers to it)
                            PContactLogAccess.DeleteByPrimaryKey(ContactLogId, Transaction);
                        }

                        SubmissionOK = true;
                    }
                });
        }
    }
}