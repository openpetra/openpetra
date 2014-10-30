//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Net.Mail;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Server.MPartner.ImportExport;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// Manage Conference applications
    /// </summary>
    public class TApplicationManagement
    {
        private static Int32 MINIMUM_OFFICES_TO_BECOME_ORGANIZER = 3;

        /// <summary>
        /// use the permissions of the user to get all offices that this user has permissions for
        /// </summary>
        /// <returns></returns>
        private static List <Int64>GetRegistrationOfficeKeysOfUser(TDBTransaction ATransaction)
        {
            List <Int64>AllowedRegistrationOffices = new List <long>();

            // get all offices that have registrations for this event
            DataTable offices = DBAccess.GDBAccessObj.SelectDT(
                String.Format("SELECT DISTINCT {0} FROM PUB_{1}",
                    PmShortTermApplicationTable.GetRegistrationOfficeDBName(),
                    PmShortTermApplicationTable.GetTableDBName()),
                "registrationoffice", ATransaction);

            // if there are no REG-... module permissions for anyone, allow all offices? this would help with a base database for testing?
            Int32 CountRegModules =
                Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM " + SModuleTable.GetTableDBName() + " WHERE " +
                        SModuleTable.GetModuleIdDBName() + " LIKE 'REG-%'", ATransaction));

            foreach (DataRow officeRow in offices.Rows)
            {
                Int64 RegistrationOffice = Convert.ToInt64(officeRow[0]);
                try
                {
                    if ((CountRegModules == 0) || TModuleAccessManager.CheckUserModulePermissions(String.Format("REG-{0:10}",
                                StringHelper.PartnerKeyToStr(RegistrationOffice))))
                    {
                        AllowedRegistrationOffices.Add(RegistrationOffice);
                    }
                }
                catch (ESecurityModuleAccessDeniedException)
                {
                    // no permissions for this registration office
                }
            }

            // the organizer has access to all attendees
            if (AllowedRegistrationOffices.Count > MINIMUM_OFFICES_TO_BECOME_ORGANIZER)
            {
                AllowedRegistrationOffices = new List <long>();

                foreach (DataRow officeRow in offices.Rows)
                {
                    Int64 RegistrationOffice = Convert.ToInt64(officeRow[0]);
                    AllowedRegistrationOffices.Add(RegistrationOffice);
                }
            }

            return AllowedRegistrationOffices;
        }

        /// is the current user representing the office that is organising the conference?
        /// This user gets to see all registrations.
        public static bool IsConferenceOrganisingOffice()
        {
            // TODO: check for permissions for just one specific office, linked from the config file?
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            List <Int64>AllowedRegistrationOffices = new List <long>();
            try
            {
                AllowedRegistrationOffices = GetRegistrationOfficeKeysOfUser(Transaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return AllowedRegistrationOffices.Count > MINIMUM_OFFICES_TO_BECOME_ORGANIZER;
        }

        /// <summary>
        /// return a list of all applicants for a given event,
        /// but if AConferenceOrganisingOffice is false,
        /// consider only the registration office that the user has permissions for, ie. Module REG-00xx0000000
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AEventPartnerKey">The ConferenceKey</param>
        /// <param name="AEventCode">The OutreachPrefix</param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ARegistrationOffice">if -1, then show all offices that the user has permission for</param>
        /// <param name="AConferenceOrganisingOffice">if true, all offices are considered</param>
        /// <param name="ARole"></param>
        /// <param name="AClearJSONData"></param>
        /// <returns></returns>
        public static bool GetApplications(
            ref ConferenceApplicationTDS AMainDS,
            Int64 AEventPartnerKey,
            string AEventCode,
            string AApplicationStatus,
            Int64 ARegistrationOffice,
            bool AConferenceOrganisingOffice,
            string ARole,
            bool AClearJSONData)
        {
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                // load all attendees of this conference.
                // only load once: GetApplications might be called for several application stati
                if (AMainDS.PcAttendee.Rows.Count == 0)
                {
                    PcAttendeeRow templateAttendeeRow = AMainDS.PcAttendee.NewRowTyped(false);
                    templateAttendeeRow.ConferenceKey = AEventPartnerKey;
                    PcAttendeeAccess.LoadUsingTemplate(AMainDS, templateAttendeeRow, Transaction);
                    AMainDS.PcAttendee.DefaultView.Sort = PcAttendeeTable.GetPartnerKeyDBName();
                }

                if (AConferenceOrganisingOffice && (ARegistrationOffice == -1))
                {
                    // avoid duplicates, who are registered by one office, but charged to another office
                    GetApplications(ref AMainDS, AEventCode, -1, AApplicationStatus, ARole, AClearJSONData, Transaction);
                }
                else
                {
                    List <Int64>AllowedRegistrationOffices = GetRegistrationOfficeKeysOfUser(Transaction);

                    foreach (Int64 RegistrationOffice in AllowedRegistrationOffices)
                    {
                        if ((ARegistrationOffice == RegistrationOffice) || (ARegistrationOffice == -1))
                        {
                            GetApplications(ref AMainDS, AEventCode, RegistrationOffice, AApplicationStatus, ARole, AClearJSONData, Transaction);
                        }
                    }
                }

                // required for DefaultView.Find
                AMainDS.PmShortTermApplication.DefaultView.Sort =
                    PmShortTermApplicationTable.GetStConfirmedOptionDBName() + "," +
                    PmShortTermApplicationTable.GetPartnerKeyDBName();
                AMainDS.PmGeneralApplication.DefaultView.Sort =
                    PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                    PmGeneralApplicationTable.GetApplicationKeyDBName() + "," +
                    PmGeneralApplicationTable.GetRegistrationOfficeDBName();
                AMainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetApplicationKeyDBName();
                AMainDS.PDataLabelValuePartner.DefaultView.Sort = PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + "," +
                                                                  PDataLabelValuePartnerTable.GetPartnerKeyDBName();
                AMainDS.PDataLabel.DefaultView.Sort = PDataLabelTable.GetTextDBName();
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (AMainDS.HasChanges())
            {
                AMainDS.EnforceConstraints = false;
                AMainDS.AcceptChanges();
            }

            return true;
        }

        /// <summary>
        /// return a list of all applicants for a given event, but only the registration office that the user has permissions for, ie. Module REG-00xx0000000
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ARegistrationOffice">if -1, then show all offices that the user has permission for</param>
        /// <param name="ARole"></param>
        /// <param name="AClearJSONData"></param>
        /// <returns></returns>
        public static bool GetApplications(
            ref ConferenceApplicationTDS AMainDS,
            Int64 AEventPartnerKey,
            string AEventCode,
            string AApplicationStatus,
            Int64 ARegistrationOffice,
            string ARole,
            bool AClearJSONData)
        {
            return GetApplications(ref AMainDS,
                AEventPartnerKey,
                AEventCode,
                AApplicationStatus,
                ARegistrationOffice,
                IsConferenceOrganisingOffice(),
                ARole,
                AClearJSONData);
        }

        /// <summary>
        /// get shortterm application rows and person rows of all people in a given fellowship group
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="AStFgCode"></param>
        /// <returns></returns>
        public static ConferenceApplicationTDS GetFellowshipGroupMembers(string AEventCode, string AStFgCode)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            try
            {
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

                List <OdbcParameter>parameters = new List <OdbcParameter>();

                OdbcParameter parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());
                parameter.Value = AEventCode;
                parameters.Add(parameter);

                parameter = new OdbcParameter("groupcode", OdbcType.VarChar, PmShortTermApplicationTable.GetStFgCodeLength());
                parameter.Value = AStFgCode;
                parameters.Add(parameter);

                string queryFellowshipGroupMembers =
                    "SELECT * " +
                    " FROM PUB_" + PmShortTermApplicationTable.GetTableDBName() +
                    " WHERE " + PmShortTermApplicationTable.GetConfirmedOptionCodeDBName() + " = ?" +
                    " AND " + PmShortTermApplicationTable.GetStFgCodeDBName() + " = ?";

                string queryPerson =
                    "SELECT DISTINCT PUB_p_person.* " +
                    "FROM PUB_pm_short_term_application, PUB_p_person " +
                    "WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                    "  AND PUB_p_person.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n" +
                    "  AND " + PmShortTermApplicationTable.GetStFgCodeDBName() + " = ?";

                DBAccess.GDBAccessObj.Select(MainDS,
                    queryFellowshipGroupMembers,
                    MainDS.PmShortTermApplication.TableName, Transaction, parameters.ToArray());

                DBAccess.GDBAccessObj.Select(MainDS,
                    queryPerson,
                    MainDS.PPerson.TableName, Transaction, parameters.ToArray());

                return MainDS;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// load one specific application from the database
        /// </summary>
        public static ConferenceApplicationTDS LoadApplicationFromDB(
            string AEventCode,
            Int64 APartnerKey)
        {
            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

                LoadApplicationsFromDB(ref MainDS, AEventCode, new Nullable <long>(), null, APartnerKey, transaction);

                return MainDS;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        private static bool LoadApplicationsFromDB(
            ref ConferenceApplicationTDS AMainDS,
            string AEventCode,
            Int64? ARegisteringOffice,
            string ARole,
            Int64? APartnerKey,
            TDBTransaction ATransaction)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());
            parameter.Value = AEventCode.Substring(0,5);
            parameters.Add(parameter);

            string DataLabels = "(PUB_p_data_label.p_text_c = 'MedicalNotes' OR PUB_p_data_label.p_text_c = 'Rebukes')";

            string queryShortTermApplication = "SELECT PUB_pm_short_term_application.* " +
                                               "FROM PUB_pm_short_term_application " +
                                               "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? ";

            string queryGeneralApplication = "SELECT PUB_pm_general_application.* " +
                                             "FROM PUB_pm_short_term_application, PUB_pm_general_application " +
                                             "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? " +
                                             "  AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                                             "  AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                                             +
                                             "  AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n";
            string queryPerson = "SELECT DISTINCT PUB_p_person.* " +
                                 "FROM PUB_pm_short_term_application, PUB_p_person " +
                                 "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? " +
                                 "  AND PUB_p_person.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n";
            string queryPartner = "SELECT DISTINCT PUB_p_partner.* " +
                                  "FROM PUB_p_partner, PUB_pm_short_term_application " +
                                  "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? " +
                                  "  AND (PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n" +
                                  " OR PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.pm_st_field_charged_n)";
            string queryDataLabel = "SELECT DISTINCT PUB_p_data_label_value_partner.* " +
                                    "FROM PUB_pm_short_term_application, PUB_p_data_label_value_partner, PUB_p_data_label " +
                                    "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? " +
                                    "  AND PUB_p_data_label_value_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n" +
                                    "  AND PUB_p_data_label_value_partner.p_data_label_key_i = PUB_p_data_label.p_key_i" +
                                    " AND " + DataLabels;

            if ((ARole != null) && (ARole.Length > 0))
            {
                queryGeneralApplication += "  AND PUB_pm_short_term_application.pm_st_congress_code_c LIKE '" + ARole + "%'";
                queryShortTermApplication += "  AND PUB_pm_short_term_application.pm_st_congress_code_c LIKE '" + ARole + "%'";
                queryPerson += "  AND PUB_pm_short_term_application.pm_st_congress_code_c LIKE '" + ARole + "%'";
                queryDataLabel += "  AND PUB_pm_short_term_application.pm_st_congress_code_c LIKE '" + ARole + "%'";
            }

            if (ARegisteringOffice.HasValue && (ARegisteringOffice.Value > 0))
            {
                string queryRegistrationOffice =
                    "  AND (PUB_pm_short_term_application.pm_st_field_charged_n = ? OR PUB_pm_short_term_application.pm_registration_office_n = ?)";
                queryGeneralApplication += queryRegistrationOffice;
                queryShortTermApplication += queryRegistrationOffice;
                queryPerson += queryRegistrationOffice;
                queryPartner += "  AND PUB_pm_short_term_application.pm_st_congress_code_c LIKE '" + ARole + "%'";
                queryDataLabel += queryRegistrationOffice;

                parameter = new OdbcParameter("fieldCharged", OdbcType.Decimal, 10);
                parameter.Value = ARegisteringOffice.Value;
                parameters.Add(parameter);
                parameter = new OdbcParameter("registrationOffice", OdbcType.Decimal, 10);
                parameter.Value = ARegisteringOffice.Value;
                parameters.Add(parameter);
            }

            if (APartnerKey.HasValue && (APartnerKey.Value > 0))
            {
                queryGeneralApplication += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryShortTermApplication += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryPerson += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryPartner += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryDataLabel += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";

                parameter = new OdbcParameter("partnerkey", OdbcType.Decimal, 10);
                parameter.Value = APartnerKey.Value;
                parameters.Add(parameter);
            }

            DBAccess.GDBAccessObj.Select(MainDS,
                queryShortTermApplication,
                MainDS.PmShortTermApplication.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryPerson,
                MainDS.PPerson.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryPartner,
                MainDS.PPartner.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryGeneralApplication,
                MainDS.PmGeneralApplication.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryDataLabel,
                MainDS.PDataLabelValuePartner.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                "SELECT * FROM PUB_p_data_label WHERE " + DataLabels,
                MainDS.PDataLabel.TableName, ATransaction);

            AMainDS.Merge(MainDS);

            AMainDS.PDataLabelValuePartner.DefaultView.Sort = PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + "," +
                                                              PDataLabelValuePartnerTable.GetPartnerKeyDBName();
            AMainDS.PDataLabel.DefaultView.Sort = PDataLabelTable.GetTextDBName();

            return true;
        }

        /// <summary>
        /// return a list of all applicants for a given event
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ARegisteringOffice"></param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ARole"></param>
        /// <param name="AClearJSONData"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private static ConferenceApplicationTDS GetApplications(
            ref ConferenceApplicationTDS AMainDS,
            string AEventCode,
            Int64 ARegisteringOffice,
            string AApplicationStatus,
            string ARole,
            bool AClearJSONData,
            TDBTransaction ATransaction)
        {
            LoadApplicationsFromDB(ref AMainDS,
                AEventCode, ARegisteringOffice,
                ARole, new Nullable <Int64>(),
                ATransaction);

            DataView PersonView = AMainDS.PPerson.DefaultView;

            PersonView.Sort = PPersonTable.GetPartnerKeyDBName();

            DataView GenAppView = AMainDS.PmGeneralApplication.DefaultView;
            GenAppView.Sort = PmGeneralApplicationTable.GetPartnerKeyDBName() + "," 
                + PmGeneralApplicationTable.GetApplicationKeyDBName() + "," + PmGeneralApplicationTable.GetRegistrationOfficeDBName();

            foreach (PmShortTermApplicationRow shortTermRow in AMainDS.PmShortTermApplication.Rows)
            {
                PPersonRow Person = (PPersonRow)PersonView[PersonView.Find(shortTermRow.PartnerKey)].Row;
                PmGeneralApplicationRow GeneralApplication =
                    (PmGeneralApplicationRow)GenAppView[GenAppView.Find(new Object[] { shortTermRow.PartnerKey, shortTermRow.ApplicationKey, shortTermRow.RegistrationOffice })].Row;

                ConferenceApplicationTDSApplicationGridRow newRow = AMainDS.ApplicationGrid.NewRowTyped();
                newRow.PartnerKey = shortTermRow.PartnerKey;

                if (!GeneralApplication.IsLocalPartnerKeyNull())
                {
                    newRow.PersonKey = GeneralApplication.LocalPartnerKey;
                }

                newRow.ApplicationKey = GeneralApplication.ApplicationKey;
                newRow.RegistrationOffice = GeneralApplication.RegistrationOffice;
                newRow.FirstName = Person.FirstName;
                newRow.FamilyName = Person.FamilyName;

                if (!Person.IsDateOfBirthNull())
                {
                    newRow.DateOfBirth = Person.DateOfBirth;
                }

                newRow.Gender = Person.Gender;
                newRow.GenAppDate = GeneralApplication.GenAppDate;
                newRow.Comment = GeneralApplication.Comment;
                newRow.StFgCode = shortTermRow.StFgCode;
                newRow.StFgLeader = shortTermRow.StFgLeader;
                if (!shortTermRow.IsStFieldChargedNull())
                {
                    newRow.StFieldCharged = shortTermRow.StFieldCharged;
                }
                newRow.DateOfArrival = shortTermRow.Arrival;
                newRow.DateOfDeparture = shortTermRow.Departure;

                // TODO: display the description of that application status
                newRow.GenApplicationStatus = GeneralApplication.GenApplicationStatus;
                newRow.StCongressCode = shortTermRow.StCongressCode;

                // only allow the medical team to read and write
                if (UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    newRow.MedicalNotes = TMedicalLogs.GetMedicalLogs(AMainDS, newRow.PartnerKey);
                }

                Int32 IndexLabelRebuke = AMainDS.PDataLabel.DefaultView.Find("Rebukes");

                if (IndexLabelRebuke != -1)
                {
                    Int32 RebukeLabelID = ((PDataLabelRow)AMainDS.PDataLabel.DefaultView[IndexLabelRebuke].Row).Key;

                    int IndexLabel = AMainDS.PDataLabelValuePartner.DefaultView.Find(new object[] { RebukeLabelID, newRow.PartnerKey });

                    if (IndexLabel != -1)
                    {
                        newRow.RebukeNotes = ((PDataLabelValuePartnerRow)AMainDS.PDataLabelValuePartner.DefaultView[IndexLabel].Row).ValueChar;
                    }
                }

                int indexAttendee = AMainDS.PcAttendee.DefaultView.Find(shortTermRow.PartnerKey);

                if (indexAttendee != -1)
                {
                    newRow.BadgePrint = ((PcAttendeeRow)AMainDS.PcAttendee.DefaultView[indexAttendee].Row).BadgePrint;
                }

                if (AClearJSONData)
                {
                    // only if the json data is cleared anyway, search for duplicate applications using the md5sum
                    newRow.JSONData = StringHelper.MD5Sum(GeneralApplication.RawApplicationData);
                }
                else
                {
                    newRow.JSONData = GeneralApplication.RawApplicationData;
                }

                if ((AApplicationStatus == null) || (AApplicationStatus.Length == 0))
                {
                    AApplicationStatus = "on hold";
                }

                if (AApplicationStatus == "on hold")
                {
                    // if there is already an application on hold for that person, drop the old row
                    AMainDS.ApplicationGrid.DefaultView.RowFilter =
                        String.Format("JSONData = '{0}' AND {1} = 'H'", newRow.JSONData,
                            ConferenceApplicationTDSApplicationGridTable.GetGenApplicationStatusDBName());

                    while (AMainDS.ApplicationGrid.DefaultView.Count > 0)
                    {
                        ConferenceApplicationTDSApplicationGridRow RowToDrop =
                            (ConferenceApplicationTDSApplicationGridRow)AMainDS.ApplicationGrid.DefaultView[0].Row;
                        //Console.WriteLine("dropping " + RowToDrop.FamilyName + " " + RowToDrop.FirstName + " " + RowToDrop.PartnerKey.ToString());
                        RowToDrop.Delete();
                    }
                }

                if ((AApplicationStatus == "on hold") && newRow.GenApplicationStatus.StartsWith("H"))
                {
                    AMainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if ((AApplicationStatus == "accepted") && newRow.GenApplicationStatus.StartsWith("A"))
                {
                    AMainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if ((AApplicationStatus == "cancelled")
                         && ((newRow.GenApplicationStatus.StartsWith("R") || newRow.GenApplicationStatus.StartsWith("C"))))
                {
                    AMainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if (AApplicationStatus == "all")
                {
                    AMainDS.ApplicationGrid.Rows.Add(newRow);
                }
            }

            AMainDS.ApplicationGrid.DefaultView.RowFilter = String.Empty;

            if (AClearJSONData)
            {
                // clear raw data, otherwise this is too big for the javascript client
                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    row.JSONData = string.Empty;
                }
            }

            AMainDS.ApplicationGrid.AcceptChanges();

            if (AMainDS.HasChanges())
            {
                AMainDS.EnforceConstraints = false;
                AMainDS.AcceptChanges();
            }

            return AMainDS;
        }

        /// <summary>
        /// get the number and name of the registration offices that the current user has access for
        /// </summary>
        /// <returns></returns>
        public static PPartnerTable GetRegistrationOffices()
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            PPartnerTable result = new PPartnerTable();

            try
            {
                List <Int64>offices = GetRegistrationOfficeKeysOfUser(Transaction);

                StringCollection FieldList = new StringCollection();
                FieldList.Add(PPartnerTable.GetPartnerKeyDBName());
                FieldList.Add(PPartnerTable.GetPartnerShortNameDBName());

                // get the short names of the registration offices
                foreach (Int64 OfficeKey in offices)
                {
                    PPartnerTable partnerTable = PPartnerAccess.LoadByPrimaryKey(OfficeKey, FieldList, Transaction);

                    result.Merge(partnerTable);
                }

                // remove unwanted columns
                List <string>ColumnNames = new List <string>();

                foreach (DataColumn column in result.Columns)
                {
                    ColumnNames.Add(column.ColumnName);
                }

                foreach (string columnName in ColumnNames)
                {
                    if (!FieldList.Contains(columnName))
                    {
                        result.Columns.Remove(columnName.ToString());
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            result.DefaultView.Sort = PPartnerTable.GetPartnerKeyDBName();

            return result;
        }

        /// <summary>
        /// get the data entered by the applicant
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AApplicationKey"></param>
        /// <param name="ARegistrationOfficeKey"></param>
        /// <returns></returns>
        public static string GetRawApplicationData(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOfficeKey)
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            string Result = "Failure, cannot find partner";

            try
            {
                PmGeneralApplicationTable application = PmGeneralApplicationAccess.LoadByPrimaryKey(APartnerKey,
                    AApplicationKey,
                    ARegistrationOfficeKey,
                    Transaction);

                Result = application[0].RawApplicationData;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return Result;
        }

        /// <summary>
        /// find the short term application for an applicant, by registration key, partner key, or first and last name;
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="LastName"></param>
        /// <param name="FirstName"></param>
        /// <returns></returns>
        public static PmShortTermApplicationRow FindShortTermApplication(ConferenceApplicationTDS AMainDS,
            ref Int64 APartnerKey,
            string LastName,
            string FirstName)
        {
            if ((APartnerKey <= 0) && (LastName.Length > 0) && (FirstName.Length > 0))
            {
                // try to find the partner key from the name
                string OldSort = AMainDS.ApplicationGrid.DefaultView.Sort;
                AMainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                                                           ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();
                Int32 index = AMainDS.ApplicationGrid.DefaultView.Find(new object[] { LastName, FirstName });

                if (index == -1)
                {
                    TLogging.Log("Import Fellowship groups: Cannot find attendee " + FirstName + " " + LastName);
                    AMainDS.ApplicationGrid.DefaultView.Sort = OldSort;
                    return null;
                }

                ConferenceApplicationTDSApplicationGridRow ApplicantRow =
                    (ConferenceApplicationTDSApplicationGridRow)AMainDS.ApplicationGrid.DefaultView[index].Row;
                APartnerKey = ApplicantRow.PartnerKey;

                AMainDS.ApplicationGrid.DefaultView.Sort = OldSort;
            }
            else
            {
                // is this the person key from the local Petra database, or the registration key?
                string OldSort = AMainDS.ApplicationGrid.DefaultView.Sort;
                AMainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPersonKeyDBName();
                Int32 index = AMainDS.ApplicationGrid.DefaultView.Find(APartnerKey);

                if (index != -1)
                {
                    ConferenceApplicationTDSApplicationGridRow ApplicantRow =
                        (ConferenceApplicationTDSApplicationGridRow)AMainDS.ApplicationGrid.DefaultView[index].Row;
                    APartnerKey = ApplicantRow.PartnerKey;
                }

                AMainDS.ApplicationGrid.DefaultView.Sort = OldSort;
            }

            string OldSortShortterm = AMainDS.PmShortTermApplication.DefaultView.Sort;
            AMainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();
            Int32 indexShorttermApp = AMainDS.PmShortTermApplication.DefaultView.Find(APartnerKey);

            if (indexShorttermApp == -1)
            {
                AMainDS.PmShortTermApplication.DefaultView.Sort = OldSortShortterm;

                if (APartnerKey > 0)
                {
                    APartnerKey = -1;

                    return FindShortTermApplication(AMainDS, ref APartnerKey, LastName, FirstName);
                }

                return null;
            }

            PmShortTermApplicationRow shorttermRow = (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[indexShorttermApp].Row;
            AMainDS.PmShortTermApplication.DefaultView.Sort = OldSortShortterm;
            return shorttermRow;
        }

        /// send an email to the applicant telling him that the application was accepted;
        /// only sends an email if the template exists
        public static bool SendEmail(TApplicationFormData AData)
        {
            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                "ApplicationAcceptedEmail",
                AData.registrationcountrycode,
                AData.formsid,
                "html");

            string HTMLText = string.Empty;
            string SenderAddress = string.Empty;
            string BCCAddress = string.Empty;
            string EmailSubject = string.Empty;

            if (!File.Exists(FileName))
            {
                TLogging.Log("Not sending a confirmation of accepting the application, since there is no file " + FileName);
                return true;
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                SenderAddress = r.ReadLine();
                BCCAddress = r.ReadLine();
                EmailSubject = r.ReadLine();
                HTMLText = r.ReadToEnd();
                r.Close();
            }

            if (!SenderAddress.StartsWith("From:"))
            {
                throw new Exception("missing From: line in the Email template " + FileName);
            }

            if (!BCCAddress.StartsWith("BCC:"))
            {
                throw new Exception("missing BCC: line in the Email template " + FileName);
            }

            if (!EmailSubject.StartsWith("Subject:"))
            {
                throw new Exception("missing Subject: line in the Email template " + FileName);
            }

            SenderAddress = SenderAddress.Substring("From:".Length).Trim();
            BCCAddress = BCCAddress.Substring("BCC:".Length).Trim();
            EmailSubject = EmailSubject.Substring("Subject:".Length).Trim();

            // TODO: custom replace?

            HTMLText = TJsonTools.ReplaceKeywordsWithData(AData.RawData, HTMLText);
            HTMLText = HTMLText.Replace("#HTMLRAWDATA", TJsonTools.DataToHTMLTable(AData.RawData));

            // load the language file for the specific country
            Catalog.Init(AData.registrationcountrycode, AData.registrationcountrycode);

            // send email
            TSmtpSender emailSender = new TSmtpSender();

            MailMessage msg = new MailMessage(SenderAddress,
                AData.email,
                EmailSubject,
                HTMLText);

            if (BCCAddress.Length > 0)
            {
                msg.Bcc.Add(BCCAddress);
            }

            if (!emailSender.SendMessage(msg))
            {
                TLogging.Log("There has been a problem sending the email to " + AData.email);
                return false;
            }

            return true;
        }

        private static void InsertDataIntoConferenceApplicationTDS(ConferenceApplicationTDSApplicationGridRow AChangedRow,
            ref ConferenceApplicationTDS AMainDS,
            string AEventCode, TDBTransaction ATransaction,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            if (AChangedRow.RowState == DataRowState.Modified)
            {
                if (ATransaction != null)
                {
                    LoadApplicationsFromDB(ref AMainDS, AEventCode, null,
                        null, AChangedRow.PartnerKey,
                        ATransaction);
                }

                AMainDS.PDataLabel.DefaultView.Sort = PDataLabelTable.GetTextDBName();
                AMainDS.PDataLabelValuePartner.DefaultView.Sort = PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + "," +
                                                                  PDataLabelValuePartnerTable.GetPartnerKeyDBName();

                if (AChangedRow.GenApplicationStatus == "I")
                {
                    // load duplicate applications, in case they should be set to ignore as well. see below
                    ConferenceApplicationTDS DuplicatesDS = new ConferenceApplicationTDS();
                    PmGeneralApplicationRow TemplateRow = AMainDS.PmGeneralApplication.NewRowTyped(false);
                    TemplateRow.GenApplicationStatus = "H";
                    TemplateRow.RawApplicationData = AChangedRow.JSONData;
                    PmGeneralApplicationAccess.LoadUsingTemplate(DuplicatesDS, TemplateRow, ATransaction);
                    AMainDS.Merge(DuplicatesDS);
                }

                AMainDS.PPerson.DefaultView.Sort = PPersonTable.GetPartnerKeyDBName();
                AMainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName() + "," +
                                                                  PmShortTermApplicationTable.GetApplicationKeyDBName();
                AMainDS.PmGeneralApplication.DefaultView.Sort = PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                                                                PmGeneralApplicationTable.GetApplicationKeyDBName();

                PPersonRow Person = (PPersonRow)AMainDS.PPerson.DefaultView[AMainDS.PPerson.DefaultView.Find(AChangedRow.PartnerKey)].Row;
                PmShortTermApplicationRow ShortTermApplication =
                    (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[AMainDS.PmShortTermApplication.DefaultView.Find(new object
                                                                                              [] {
                                                                                                  AChangedRow.PartnerKey,
                                                                                                  AChangedRow.ApplicationKey
                                                                                              })].Row;
                PmGeneralApplicationRow GeneralApplication =
                    (PmGeneralApplicationRow)AMainDS.PmGeneralApplication.DefaultView[AMainDS.PmGeneralApplication.DefaultView.Find(new object[] {
                                                                                              AChangedRow.PartnerKey,
                                                                                              AChangedRow.ApplicationKey
                                                                                          })].Row;

                Person.FirstName = AChangedRow.FirstName;
                Person.FamilyName = AChangedRow.FamilyName;

                if (AChangedRow.DateOfBirth.HasValue)
                {
                    Person.DateOfBirth = AChangedRow.DateOfBirth;
                }
                else
                {
                    Person.SetDateOfBirthNull();
                }

                Person.Gender = AChangedRow.Gender;
                GeneralApplication.Comment = AChangedRow.Comment;
                GeneralApplication.GenAppDate = AChangedRow.GenAppDate;

                if (!AChangedRow.IsJSONDataNull() && (AChangedRow.JSONData.Length > 0))
                {
                    GeneralApplication.RawApplicationData = AChangedRow.JSONData;
                }
                else
                {
                    // get raw application data, we have removed it because we don't need it on the client
                    GeneralApplication.RawApplicationData = GetRawApplicationData(GeneralApplication.PartnerKey,
                        GeneralApplication.ApplicationKey,
                        GeneralApplication.RegistrationOffice);
                }

                ShortTermApplication.StFgLeader = AChangedRow.StFgLeader;
                ShortTermApplication.StFgCode = AChangedRow.StFgCode;
                ShortTermApplication.StFieldCharged = AChangedRow.StFieldCharged;
                ShortTermApplication.Arrival = AChangedRow.DateOfArrival;
                ShortTermApplication.Departure = AChangedRow.DateOfDeparture;

                if (GeneralApplication.GenApplicationStatus != AChangedRow.GenApplicationStatus)
                {
                    if (AChangedRow.GenApplicationStatus == "A")
                    {
                        if (GeneralApplication.IsGenAppSendFldAcceptDateNull())
                        {
                            GeneralApplication.GenAppSendFldAcceptDate = DateTime.Today;
                        }

                        GeneralApplication.SetGenAppCancelledNull();
                        GeneralApplication.GenCancelledApp = false;

                        try
                        {
                            TApplicationFormData data = (TApplicationFormData)TJsonTools.ImportIntoTypedStructure(typeof(TApplicationFormData),
                                GeneralApplication.RawApplicationData);
                            data.RawData = GeneralApplication.RawApplicationData;

                            // attempt to send an email to that applicant, telling about the accepted application
                            SendEmail(data);
                        }
                        catch (Exception e)
                        {
                            TLogging.Log("Problem sending acceptance email " + e.Message);
                            TLogging.Log(GeneralApplication.RawApplicationData);
                            throw;
                        }
                    }
                    else if (AChangedRow.GenApplicationStatus.StartsWith("C") || AChangedRow.GenApplicationStatus.StartsWith("R"))
                    {
                        GeneralApplication.GenCancelledApp = true;

                        if (GeneralApplication.IsGenAppCancelledNull())
                        {
                            GeneralApplication.GenAppCancelled = DateTime.Today;
                        }
                    }
                    else if (AChangedRow.GenApplicationStatus == "I")
                    {
                        if (GeneralApplication.GenApplicationStatus != "H")
                        {
                            AVerificationResult.Add(new TVerificationResult("Saving application",
                                    "Cannot set Application Status from " +
                                    GeneralApplication.GenApplicationStatus +
                                    "to Ignored, please use status Cancelled",
                                    "Application status problem",
                                    TResultSeverity.Resv_Critical, new Guid()));
                            throw new Exception("Cannot set Application Status from " +
                                GeneralApplication.GenApplicationStatus +
                                "to Ignored, please use status Cancelled");
                        }

                        // drop all other applications of this person, that are on hold.
                        // data has been loaded above already.
                        DataView DuplicateView = new DataView(AMainDS.PmGeneralApplication);

                        // TODO problems with single quotes in rawData: this is not the best solution, because it will not find matches, but at least it will save.
                        try
                        {
                            DuplicateView.RowFilter =
                                String.Format("{0} = '{1}' AND {2} = 'H'",
                                    PmGeneralApplicationTable.GetRawApplicationDataDBName(),
                                    AChangedRow.JSONData.Replace("'", "&apos;"),
                                    PmGeneralApplicationTable.GetGenApplicationStatusDBName());

                            foreach (DataRowView rv in DuplicateView)
                            {
                                PmGeneralApplicationRow DuplicateApplication = (PmGeneralApplicationRow)rv.Row;
                                DuplicateApplication.GenApplicationStatus = "I";
                            }
                        }
                        catch (Exception)
                        {
                            // some problem with Invalid escape sequence: '\"'.
                        }
                    }
                }

                GeneralApplication.GenApplicationStatus = AChangedRow.GenApplicationStatus;
                ShortTermApplication.StCongressCode = AChangedRow.StCongressCode;

                // only allow the medical team to save
                if ((AChangedRow.MedicalNotes.Length > 0) && UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    Int32 IndexLabelMedical = AMainDS.PDataLabel.DefaultView.Find("MedicalNotes");

                    if (IndexLabelMedical != -1)
                    {
                        Int32 MedicalLabelID = ((PDataLabelRow)AMainDS.PDataLabel.DefaultView[IndexLabelMedical].Row).Key;

                        int IndexLabel = AMainDS.PDataLabelValuePartner.DefaultView.Find(new object[] { MedicalLabelID, AChangedRow.PartnerKey });

                        if (IndexLabel != -1)
                        {
                            ((PDataLabelValuePartnerRow)AMainDS.PDataLabelValuePartner.DefaultView[IndexLabel].Row).ValueChar =
                                AChangedRow.MedicalNotes;
                        }
                        else
                        {
                            PDataLabelValuePartnerRow newLabel = AMainDS.PDataLabelValuePartner.NewRowTyped();
                            newLabel.DataLabelKey = MedicalLabelID;
                            newLabel.PartnerKey = AChangedRow.PartnerKey;
                            newLabel.ValueChar = AChangedRow.MedicalNotes;
                            AMainDS.PDataLabelValuePartner.Rows.Add(newLabel);
                        }
                    }
                    else
                    {
                        TLogging.Log("we are missing data label for MedicalNotes");
                    }
                }

                // only allow the boundaries team to modify rebukes
                if ((AChangedRow.RebukeNotes.Length > 0) && UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                    Int32 IndexLabelRebuke = AMainDS.PDataLabel.DefaultView.Find("Rebukes");

                    if (IndexLabelRebuke != -1)
                    {
                        Int32 RebukeLabelID = ((PDataLabelRow)AMainDS.PDataLabel.DefaultView[IndexLabelRebuke].Row).Key;

                        int IndexLabel = AMainDS.PDataLabelValuePartner.DefaultView.Find(new object[] { RebukeLabelID, AChangedRow.PartnerKey });

                        if (IndexLabel != -1)
                        {
                            ((PDataLabelValuePartnerRow)AMainDS.PDataLabelValuePartner.DefaultView[IndexLabel].Row).ValueChar =
                                AChangedRow.RebukeNotes;
                        }
                        else
                        {
                            PDataLabelValuePartnerRow newLabel = AMainDS.PDataLabelValuePartner.NewRowTyped();
                            newLabel.DataLabelKey = RebukeLabelID;
                            newLabel.PartnerKey = AChangedRow.PartnerKey;
                            newLabel.ValueChar = AChangedRow.RebukeNotes;
                            AMainDS.PDataLabelValuePartner.Rows.Add(newLabel);
                        }
                    }
                    else
                    {
                        TLogging.Log("we are missing data label for Boundaries");
                    }
                }
            }
        }

        /// <summary>
        /// store the adjusted applications to the database
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveApplications(ref ConferenceApplicationTDS AMainDS)
        {
            TVerificationResultCollection VerificationResult;

            try
            {
                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    InsertDataIntoConferenceApplicationTDS(row, ref AMainDS, string.Empty, null, out VerificationResult);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return TSubmitChangesResult.scrError;
            }


            ConferenceApplicationTDSAccess.SubmitChanges(AMainDS);

            TSubmitChangesResult result = TSubmitChangesResult.scrOK;

            // this takes 6 seconds!
            AMainDS.AcceptChanges();

            return result;
        }

        /// <summary>
        /// store the selected application to the database
        /// </summary>
        /// <returns></returns>
        public static TSubmitChangesResult SaveApplication(string AEventCode,
            ConferenceApplicationTDSApplicationGridRow ARow,
            out TVerificationResultCollection AVerificationResult)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                InsertDataIntoConferenceApplicationTDS(ARow, ref MainDS, AEventCode, Transaction, out AVerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return TSubmitChangesResult.scrError;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            ConferenceApplicationTDSAccess.SubmitChanges(MainDS);

            TSubmitChangesResult result = TSubmitChangesResult.scrOK;

            ARow.AcceptChanges();

            return result;
        }

        /// <summary>
        /// export accepted and cancelled applications to Petra
        /// </summary>
        public static string DownloadApplications(Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ARegistrationOffice,
            bool AOnlyNotImportedYet = false)
        {
            string result = string.Empty;

            result += "PersonPartnerKey;EventPartnerKey;ApplicationDate;AcquisitionCode;Title;FirstName;FamilyName;Street;PostCode;City;";
            result += "Country;Phone;Mobile;Email;DateOfBirth;MaritalStatus;Gender;Vegetarian;MedicalNeeds;ArrivalDate;DepartureDate;";
            result += "EventRole;AppStatus;PreviousAttendance;AppComments;NotesPerson;HorstID;FamilyPartnerKey;RecordImported";
            result = "\"" + result.Replace(";", "\";\"") + "\"\n";

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
            GetApplications(ref MainDS, AEventPartnerKey, AEventCode, "cancelled", ARegistrationOffice, null, true);
            GetApplications(ref MainDS, AEventPartnerKey, AEventCode, "accepted", ARegistrationOffice, null, true);

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                foreach (ConferenceApplicationTDSApplicationGridRow row in MainDS.ApplicationGrid.Rows)
                {
                    PmShortTermApplicationRow TemplateRow = MainDS.PmShortTermApplication.NewRowTyped(false);

                    // one person is only registered once for the same event. each registration is a new partner key
                    TemplateRow.PartnerKey = row.PartnerKey;
                    TemplateRow.ConfirmedOptionCode = AEventCode;
                    PmShortTermApplicationRow ShortTermApplicationRow = PmShortTermApplicationAccess.LoadUsingTemplate(TemplateRow, Transaction)[0];

                    PPersonRow PersonRow = PPersonAccess.LoadByPrimaryKey(ShortTermApplicationRow.PartnerKey, Transaction)[0];
                    PLocationRow LocationRow = PLocationAccess.LoadViaPPartner(PersonRow.FamilyKey, Transaction)[0];
                    PPartnerLocationRow PartnerLocationRow = PPartnerLocationAccess.LoadViaPPartner(PersonRow.FamilyKey, Transaction)[0];

                    PmGeneralApplicationRow GeneralApplicationRow =
                        PmGeneralApplicationAccess.LoadByPrimaryKey(ShortTermApplicationRow.PartnerKey,
                            ShortTermApplicationRow.ApplicationKey,
                            ShortTermApplicationRow.RegistrationOffice,
                            Transaction)[0];

                    if (GeneralApplicationRow.IsLocalPartnerKeyNull() || !AOnlyNotImportedYet)
                    {
                        if (GeneralApplicationRow.IsLocalPartnerKeyNull())
                        {
                            // TODO should we add the old partner key?
                            result += "\"\";";
                        }
                        else
                        {
                            result += "\"" + GeneralApplicationRow.LocalPartnerKey.ToString() + "\";";
                        }

                        TApplicationFormData data = (TApplicationFormData)TJsonTools.ImportIntoTypedStructure(typeof(TApplicationFormData),
                            GeneralApplicationRow.RawApplicationData);
                        string prevConf = string.Empty;

                        if (data.numberprevconfadult != null)
                        {
                            prevConf = StringHelper.AddCSV(prevConf, data.numberprevconfadult);
                        }

                        if (data.numberprevconfparticipant != null)
                        {
                            prevConf = StringHelper.AddCSV(prevConf, data.numberprevconfparticipant);
                        }

                        if (data.numberprevconfleader != null)
                        {
                            prevConf = StringHelper.AddCSV(prevConf, data.numberprevconfleader);
                        }

                        if (data.numberprevconfhelper != null)
                        {
                            prevConf = StringHelper.AddCSV(prevConf, data.numberprevconfhelper);
                        }

                        if (data.numberprevconf != null)
                        {
                            prevConf = StringHelper.AddCSV(prevConf, data.numberprevconf);
                        }

                        result += "\"" + AEventPartnerKey.ToString() + "\";";
                        result += "\"" + GeneralApplicationRow.GenAppDate.ToString("dd-MM-yyyy") + "\";";
                        // TODO AcquisitionCode
                        result += "\"" + "\";";
                        result += "\"" + PersonRow.Title + "\";";
                        result += "\"" + PersonRow.FirstName + "\";";
                        result += "\"" + PersonRow.FamilyName + "\";";
                        result += "\"" + LocationRow.StreetName + "\";";
                        result += "\"" + LocationRow.PostalCode + "\";";
                        result += "\"" + LocationRow.City + "\";";
                        result += "\"" + LocationRow.CountryCode + "\";";
                        result += "\"" + PartnerLocationRow.TelephoneNumber + "\";";
                        result += "\"" + PartnerLocationRow.MobileNumber + "\";";
                        result += "\"" + PartnerLocationRow.EmailAddress + "\";";
                        result += "\"" + PersonRow.DateOfBirth.Value.ToString("dd-MM-yyyy") + "\";";
                        result += "\"" + PersonRow.MaritalStatus + "\";";
                        result += "\"" + PersonRow.Gender + "\";";
                        result += "\"" + /* vegetarian + */ "\";";
                        result += "\"" + /* MedicalNeeds + */ "\";";
                        result += "\"" + /* ArrivalDate + */ "\";";
                        result += "\"" + /* DepartureDate + */ "\";";
                        result += "\"" + ShortTermApplicationRow.StCongressCode + "\";";
                        result += "\"" + GeneralApplicationRow.GenApplicationStatus + "\";";
                        result += "\"" + prevConf + "\";";
                        result += "\"" + /* AppComments + */ "\";";
                        result += "\"" + /* NotesPerson + */ "\";";
                        result += "\"" + PersonRow.PartnerKey.ToString() + "\";";
                        result += "\"" + /* FamilyPartnerKey + */ "\";";
                        result += "\"" + (GeneralApplicationRow.ImportedLocalPetra ? "yes" : "") + "\";";

                        result += "\n";
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return String.Empty;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// Calculate the age in years at a reference date
        /// </summary>
        /// <param name="ADateOfBirth"></param>
        /// <param name="AReferenceDate"></param>
        /// <returns></returns>
        public static Int32 CalculateAge(DateTime? ADateOfBirth, DateTime AReferenceDate)
        {
            if (!ADateOfBirth.HasValue)
            {
                return -1;
            }

            Int32 AgeInYears =
                AReferenceDate.Year - ADateOfBirth.Value.Year;

            if (ADateOfBirth.Value.DayOfYear > AReferenceDate.DayOfYear)
            {
                // BirthdayDuringOrAfter Reference Date
                AgeInYears--;
            }

            return AgeInYears;
        }

        /// <summary>
        /// export accepted applications to an Excel file
        /// </summary>
        /// <returns></returns>
        public static bool DownloadApplications(Int64 AConferenceKey, string AEventCode, ref ConferenceApplicationTDS AMainDS, MemoryStream AStream)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                PcConferenceTable ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, Transaction);
                DateTime ConferenceStartDate = ConferenceTable[0].Start.Value;
                DateTime ConferenceEndDate = ConferenceTable[0].End.Value;

                // load all data at once
                StringBuilder sb = new StringBuilder();

                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    sb.Append(row.PartnerKey.ToString());
                    sb.Append(',');
                }

                sb.Append("-1");
                string partnerkeys = sb.ToString();

                string sqlLoadPersons = "SELECT PUB_p_person.* FROM PUB_p_person " +
                                        "WHERE p_partner_key_n IN (" + partnerkeys + ")";
                PPersonTable persons = new PPersonTable();
                DBAccess.GDBAccessObj.SelectDT(persons, sqlLoadPersons, Transaction, new OdbcParameter[0], 0, 0);

                string sqlLoadShortTermApplications = "SELECT PUB_pm_short_term_application.* FROM PUB_pm_short_term_application " +
                                                      "WHERE p_partner_key_n IN (" +
                                                      partnerkeys + ") AND pm_confirmed_option_code_c = '" + AEventCode + "'";
                PmShortTermApplicationTable shorttermapplications = new PmShortTermApplicationTable();
                DBAccess.GDBAccessObj.SelectDT(shorttermapplications, sqlLoadShortTermApplications, Transaction, new OdbcParameter[0], 0, 0);
                shorttermapplications.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();

                string sqlLoadGeneralApplications =
                    "SELECT PUB_pm_general_application.* FROM PUB_pm_general_application, PUB_pm_short_term_application " +
                    "WHERE PUB_pm_short_term_application.p_partner_key_n IN (" +
                    partnerkeys + ") AND pm_confirmed_option_code_c = '" + AEventCode + "' " +
                    "AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n "
                    +
                    "AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                    +
                    "AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n ";
                PmGeneralApplicationTable generalapplications = new PmGeneralApplicationTable();
                DBAccess.GDBAccessObj.SelectDT(generalapplications, sqlLoadGeneralApplications, Transaction, new OdbcParameter[0], 0, 0);
                generalapplications.DefaultView.Sort = PmGeneralApplicationTable.GetPartnerKeyDBName();

                string sqlLoadPartnerLocations =
                    "SELECT DISTINCT pl.* FROM PUB_p_partner_location pl, PUB_p_person pp WHERE pp.p_family_key_n = pl.p_partner_key_n AND pp.p_partner_key_n IN ("
                    +
                    partnerkeys + ")";
                PPartnerLocationTable partnerLocations = new PPartnerLocationTable();
                DBAccess.GDBAccessObj.SelectDT(partnerLocations, sqlLoadPartnerLocations, Transaction, new OdbcParameter[0], 0, 0);
                partnerLocations.DefaultView.Sort = PPartnerLocationTable.GetPartnerKeyDBName();

                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    // one person is only registered once for the same event. each registration is a new partner key
                    PmShortTermApplicationRow ShortTermApplicationRow =
                        (PmShortTermApplicationRow)shorttermapplications.DefaultView.FindRows(row.PartnerKey)[0].Row;

                    PPersonRow PersonRow =
                        (PPersonRow)persons.Rows.Find(ShortTermApplicationRow.PartnerKey);

                    PPartnerLocationRow PartnerLocationRow =
                        (PPartnerLocationRow)partnerLocations.DefaultView.FindRows(PersonRow.FamilyKey)[0].Row;

                    PmGeneralApplicationRow GeneralApplicationRow =
                        (PmGeneralApplicationRow)generalapplications.DefaultView.FindRows(row.PartnerKey)[0].Row;


                    XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                    myDoc.DocumentElement.AppendChild(newNode);
                    XmlAttribute attr;

                    if (!GeneralApplicationRow.IsLocalPartnerKeyNull())
                    {
                        attr = myDoc.CreateAttribute("LocalPartnerKey");
                        attr.Value = GeneralApplicationRow.LocalPartnerKey.ToString();
                        newNode.Attributes.Append(attr);
                    }

                    attr = myDoc.CreateAttribute("PartnerKey");
                    attr.Value = PersonRow.PartnerKey.ToString();
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("FamilyName");
                    attr.Value = PersonRow.FamilyName;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("FirstName");
                    attr.Value = PersonRow.FirstName;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("TelephoneNumber");
                    attr.Value = PartnerLocationRow.TelephoneNumber;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("MobileNumber");
                    attr.Value = PartnerLocationRow.MobileNumber;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("EmailAddress");
                    attr.Value = PartnerLocationRow.EmailAddress;
                    newNode.Attributes.Append(attr);

                    if (PersonRow.DateOfBirth.HasValue)
                    {
                        attr = myDoc.CreateAttribute("DateOfBirth");
                        attr.Value = new TVariant(PersonRow.DateOfBirth.Value).EncodeToString();
                        newNode.Attributes.Append(attr);
                        attr = myDoc.CreateAttribute("DayOfBirth");
                        attr.Value = PersonRow.DateOfBirth.Value.ToString("MMdd");
                        newNode.Attributes.Append(attr);

                        Int32 AgeAtStartOfConference = CalculateAge(PersonRow.DateOfBirth.Value, ConferenceStartDate);
                        Int32 AgeAtEndOfConference = CalculateAge(PersonRow.DateOfBirth.Value, ConferenceEndDate);

                        attr = myDoc.CreateAttribute("AgeAtStartOfConference");
                        attr.Value = new TVariant(AgeAtStartOfConference).EncodeToString();
                        newNode.Attributes.Append(attr);
                        attr = myDoc.CreateAttribute("AgeAtEndOfConference");
                        attr.Value = new TVariant(AgeAtEndOfConference).EncodeToString();
                        newNode.Attributes.Append(attr);
                    }

                    attr = myDoc.CreateAttribute("Gender");
                    attr.Value = PersonRow.Gender;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("Role");
                    attr.Value = ShortTermApplicationRow.StCongressCode;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("ApplicationDate");
                    attr.Value = new TVariant(GeneralApplicationRow.GenAppDate).EncodeToString();
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("ApplicationStatus");
                    attr.Value = GeneralApplicationRow.GenApplicationStatus;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("CommentByOffice");
                    attr.Value = GeneralApplicationRow.Comment;
                    newNode.Attributes.Append(attr);

                    if (GeneralApplicationRow.GenApplicationStatus.StartsWith("A"))
                    {
                        attr = myDoc.CreateAttribute("AcceptedDate");
                        DateTime DateAccepted = GeneralApplicationRow.GenAppDate;

                        if (!GeneralApplicationRow.IsGenAppSendFldAcceptDateNull())
                        {
                            DateAccepted = GeneralApplicationRow.GenAppSendFldAcceptDate.Value;
                        }
                        else if (!GeneralApplicationRow.IsGenAppRecvgFldAcceptNull())
                        {
                            DateAccepted = GeneralApplicationRow.GenAppRecvgFldAccept.Value;
                        }

                        attr.Value = new TVariant(DateAccepted).EncodeToString();
                        newNode.Attributes.Append(attr);
                    }

                    attr = myDoc.CreateAttribute("FieldCharged");
                    attr.Value = ShortTermApplicationRow.StFieldCharged.ToString();
                    newNode.Attributes.Append(attr);

                    if (!ShortTermApplicationRow.IsStFgCodeNull())
                    {
                        attr = myDoc.CreateAttribute("FGroup");
                        attr.Value = ShortTermApplicationRow.StFgCode.ToString();
                        newNode.Attributes.Append(attr);
                    }

                    if (!ShortTermApplicationRow.IsStFgLeaderNull())
                    {
                        attr = myDoc.CreateAttribute("FGLeader");
                        attr.Value = ShortTermApplicationRow.StFgLeader.ToString();
                        newNode.Attributes.Append(attr);
                    }

                    // now add all the values from the json data
                    TJsonTools.DataToXml(GeneralApplicationRow.RawApplicationData, ref newNode, myDoc, false);
                }

                return TCsv2Xml.Xml2ExcelStream(myDoc, AStream);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return false;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// get all accepted participants of this conference.
        /// used for seminar registration, which is an external software
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="AEventCode"></param>
        /// <returns></returns>
        [RequireModulePermission("SEMINARS")]
        public static DataTable GetAllParticipants(Int64 AConferenceKey, string AEventCode)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            GetApplications(ref MainDS, AConferenceKey, AEventCode, "accepted", -1, true, null, true);

            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnGenAppDate);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnGenApplicationStatus);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnBadgePrint);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnStFieldCharged);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnApplicationKey);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnComment);
            MainDS.ApplicationGrid.Columns.Remove(MainDS.ApplicationGrid.ColumnDateOfDeparture);

            return MainDS.ApplicationGrid;
        }

        /// <summary>
        /// Import the file that you get as a result when you import the applications from the Online Registration into your local Petra.
        /// This file contains the partner keys in the local Petra,
        /// and avoids that the registration office has to redo all the importing for the next round of applicants.
        /// </summary>
        /// <param name="APartnerKeyFile"></param>
        public static void UploadPetraImportResult(string APartnerKeyFile)
        {
            XmlDocument partnerKeys = TCsv2Xml.ParseCSV2Xml(APartnerKeyFile);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string RegistrationIDs = string.Empty;
                string RegistrationOffices = string.Empty;

                List <Int64>AllowedRegistrationOffices = GetRegistrationOfficeKeysOfUser(Transaction);

                foreach (Int64 RegistrationOffice in AllowedRegistrationOffices)
                {
                    RegistrationOffices = StringHelper.AddCSV(RegistrationOffices, RegistrationOffice.ToString(), ",");
                }

                // get a list of partner keys that should get the person key from the local office
                foreach (XmlNode applicant in partnerKeys.DocumentElement.ChildNodes)
                {
                    try
                    {
                        Int64 RegistrationID = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "HorstID"));
                        bool RecordImported = (TXMLParser.GetAttribute(applicant, "RecordImported").ToLower() == "yes");

                        if (RecordImported)
                        {
                            // prepare SELECT WHERE IN (list of partner keys)
                            RegistrationIDs = StringHelper.AddCSV(RegistrationIDs, RegistrationID.ToString(), ",");
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem importing applicant: " + e.ToString());
                    }
                }

                // get all the pm_general_application records of those partners
                PmGeneralApplicationTable applicationTable = new PmGeneralApplicationTable();
                string stmt = String.Format("SELECT * FROM PUB_{0} WHERE {1} IN ({2}) AND {3} IN ({4})",
                    PmGeneralApplicationTable.GetTableDBName(),
                    PmGeneralApplicationTable.GetRegistrationOfficeDBName(),
                    RegistrationOffices,
                    PmGeneralApplicationTable.GetPartnerKeyDBName(),
                    RegistrationIDs);
                DBAccess.GDBAccessObj.SelectDT(applicationTable, stmt, Transaction, null, 0, 0);

                applicationTable.DefaultView.Sort = PmGeneralApplicationTable.GetPartnerKeyDBName();

                foreach (XmlNode applicant in partnerKeys.DocumentElement.ChildNodes)
                {
                    Int64 RegistrationID = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "HorstID"));

                    try
                    {
                        bool RecordImported = (TXMLParser.GetAttribute(applicant, "RecordImported").ToLower() == "yes");

                        if (RecordImported)
                        {
                            Int64 LocalOfficePartnerKey = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "PersonPartnerKey"));

                            Int32 index = applicationTable.DefaultView.Find(RegistrationID);

                            if (index != -1)
                            {
                                PmGeneralApplicationRow row = (PmGeneralApplicationRow)applicationTable.DefaultView[index].Row;
                                row.LocalPartnerKey = LocalOfficePartnerKey;
                                row.ImportedLocalPetra = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("problem importing applicant: " + e.ToString());
                    }
                }

                // store modified partners
                PmGeneralApplicationAccess.SubmitChanges(applicationTable, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the uploading of the Petra Import result:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }
        }

        /// <summary>
        /// Import the applicants from a Petra extract of the local office.
        /// This should only be used for offices that don't use the online registration.
        /// for applicants that are already in the online database, we will only update the application status, nothing else.
        /// </summary>
        /// <param name="APartnerKeyFile"></param>
        /// <param name="AEventCode">only import applications for this event</param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static bool UploadPetraExtract(string APartnerKeyFile, string AEventCode, out TVerificationResultCollection AVerificationResult)
        {
            StreamReader reader = new StreamReader(APartnerKeyFile);

            string[] lines = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n").Split(new char[] { '\n' });
            reader.Close();
            TPartnerFileImport importer = new TPartnerFileImport();
            try
            {
                PartnerImportExportTDS MainDS = importer.ImportAllData(lines, AEventCode, true, out AVerificationResult);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    return false;
                }

                PartnerImportExportTDSAccess.SubmitChanges(MainDS);

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult("importing .ext file", e.Message, TResultSeverity.Resv_Critical));

                return false;
            }
        }
    }
}