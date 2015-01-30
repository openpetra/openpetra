//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
            TDBTransaction Transaction = null;
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    // load all attendees of this conference.
                    // only load once: GetApplications might be called for several application stati
                    if (MainDS.PcAttendee.Rows.Count == 0)
                    {
                        PcAttendeeRow templateAttendeeRow = MainDS.PcAttendee.NewRowTyped(false);
                        templateAttendeeRow.ConferenceKey = AEventPartnerKey;
                        PcAttendeeAccess.LoadUsingTemplate(MainDS, templateAttendeeRow, Transaction);
                        MainDS.PcAttendee.DefaultView.Sort = PcAttendeeTable.GetPartnerKeyDBName();
                    }

                    if (AConferenceOrganisingOffice && (ARegistrationOffice == -1))
                    {
                        // avoid duplicates, who are registered by one office, but charged to another office
                        GetApplications(ref MainDS, AEventCode, -1, AApplicationStatus, ARole, AClearJSONData, Transaction);
                    }
                    else
                    {
                        List <Int64>AllowedRegistrationOffices = GetRegistrationOfficeKeysOfUser(Transaction);

                        foreach (Int64 RegistrationOffice in AllowedRegistrationOffices)
                        {
                            if ((ARegistrationOffice == RegistrationOffice) || (ARegistrationOffice == -1))
                            {
                                GetApplications(ref MainDS, AEventCode, RegistrationOffice, AApplicationStatus, ARole, AClearJSONData, Transaction);
                            }
                        }
                    }

                    // required for DefaultView.Find
                    MainDS.PmShortTermApplication.DefaultView.Sort =
                        PmShortTermApplicationTable.GetStConfirmedOptionDBName() + "," +
                        PmShortTermApplicationTable.GetPartnerKeyDBName();
                    MainDS.PmGeneralApplication.DefaultView.Sort =
                        PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                        PmGeneralApplicationTable.GetApplicationKeyDBName() + "," +
                        PmGeneralApplicationTable.GetRegistrationOfficeDBName();
                    MainDS.PDataLabelValuePartner.DefaultView.Sort = PDataLabelValuePartnerTable.GetDataLabelKeyDBName() + "," +
                                                                     PDataLabelValuePartnerTable.GetPartnerKeyDBName();
                    MainDS.PDataLabel.DefaultView.Sort = PDataLabelTable.GetTextDBName();
                });

            AMainDS = MainDS;

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
            parameter.Value = AEventCode.Substring(0, 5);
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
            GenAppView.Sort = PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                              PmGeneralApplicationTable.GetApplicationKeyDBName() + "," + PmGeneralApplicationTable.GetRegistrationOfficeDBName();

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
    }
}