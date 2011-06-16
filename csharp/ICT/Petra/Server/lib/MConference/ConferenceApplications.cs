//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Common.Verification;
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
                catch (EvaluateException)
                {
                    // no permissions for this registration office
                }
            }

            return AllowedRegistrationOffices;
        }

        /// <summary>
        /// return a list of all applicants for a given event, but only the registration office that the user has permissions for, ie. Module REG-00xx0000000
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ARegistrationOffice"></param>
        /// <param name="ARole"></param>
        /// <returns></returns>
        public static ConferenceApplicationTDS GetApplications(string AEventCode, string AApplicationStatus, Int64 ARegistrationOffice, string ARole)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            try
            {
                List <Int64>AllowedRegistrationOffices = GetRegistrationOfficeKeysOfUser(Transaction);

                foreach (Int64 RegistrationOffice in AllowedRegistrationOffices)
                {
                    if ((ARegistrationOffice == RegistrationOffice) || (ARegistrationOffice == -1))
                    {
                        MainDS.Merge(GetApplications(AEventCode, RegistrationOffice, AApplicationStatus, ARole, Transaction));
                    }
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            MainDS.AcceptChanges();

            return MainDS;
        }

        private static ConferenceApplicationTDS LoadApplicationsFromDB(string AEventCode,
            Int64? ARegisteringOffice,
            string ARole,
            Int64? APartnerKey,
            TDBTransaction ATransaction)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());
            parameter.Value = AEventCode;
            parameters.Add(parameter);

            string queryShortTermApplication = "SELECT PUB_pm_short_term_application.* " +
                                               "FROM PUB_pm_short_term_application " +
                                               "WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c = ? ";

            string queryGeneralApplication = "SELECT PUB_pm_general_application.* " +
                                             "FROM PUB_pm_short_term_application, PUB_pm_general_application " +
                                             "WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                                             "  AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                                             "  AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                                             +
                                             "  AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n";
            string queryPerson = "SELECT PUB_p_person.* " +
                                 "FROM PUB_pm_short_term_application, PUB_p_person " +
                                 "WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                                 "  AND PUB_p_person.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n";

            if ((ARole != null) && (ARole.Length > 0))
            {
                queryGeneralApplication += "  AND PUB_pm_short_term_application.pm_st_congress_code_c = ?";
                queryShortTermApplication += "  AND PUB_pm_short_term_application.pm_st_congress_code_c = ?";
                queryPerson += "  AND PUB_pm_short_term_application.pm_st_congress_code_c = ?";

                parameter = new OdbcParameter("role", OdbcType.VarChar, PmShortTermApplicationTable.GetStCongressCodeLength());
                parameter.Value = ARole;
                parameters.Add(parameter);
            }

            if (ARegisteringOffice.HasValue && (ARegisteringOffice.Value != 0))
            {
                string queryRegistrationOffice =
                    "  AND (PUB_pm_short_term_application.pm_st_field_charged_n = ? OR PUB_pm_short_term_application.pm_registration_office_n = ?)";
                queryGeneralApplication += queryRegistrationOffice;
                queryShortTermApplication += queryRegistrationOffice;
                queryPerson += queryRegistrationOffice;

                parameter = new OdbcParameter("fieldCharged", OdbcType.Decimal, 10);
                parameter.Value = ARegisteringOffice.Value;
                parameters.Add(parameter);
                parameter = new OdbcParameter("registrationOffice", OdbcType.Decimal, 10);
                parameter.Value = ARegisteringOffice.Value;
                parameters.Add(parameter);
            }

            if (APartnerKey.HasValue && (APartnerKey.Value != 0))
            {
                queryGeneralApplication += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryShortTermApplication += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";
                queryPerson += "  AND PUB_pm_short_term_application.p_partner_key_n = ?";

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
                queryGeneralApplication,
                MainDS.PmGeneralApplication.TableName, ATransaction, parameters.ToArray());

            return MainDS;
        }

        /// <summary>
        /// return a list of all applicants for a given event
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="ARegisteringOffice"></param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ARole"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private static ConferenceApplicationTDS GetApplications(string AEventCode,
            Int64 ARegisteringOffice,
            string AApplicationStatus,
            string ARole,
            TDBTransaction ATransaction)
        {
            ConferenceApplicationTDS MainDS = LoadApplicationsFromDB(AEventCode, ARegisteringOffice,
                ARole, new Nullable <Int64>(),
                ATransaction);

            foreach (PmShortTermApplicationRow shortTermRow in MainDS.PmShortTermApplication.Rows)
            {
                MainDS.PPerson.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PPersonTable.GetPartnerKeyDBName(),
                        shortTermRow.PartnerKey);
                MainDS.PmGeneralApplication.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PmGeneralApplicationTable.GetPartnerKeyDBName(),
                        shortTermRow.PartnerKey);

                PPersonRow Person = (PPersonRow)MainDS.PPerson.DefaultView[0].Row;
                PmGeneralApplicationRow GeneralApplication = (PmGeneralApplicationRow)MainDS.PmGeneralApplication.DefaultView[0].Row;

                ConferenceApplicationTDSApplicationGridRow newRow = MainDS.ApplicationGrid.NewRowTyped();
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
                newRow.StFieldCharged = shortTermRow.StFieldCharged;

                // TODO: display the description of that application status
                newRow.GenApplicationStatus = GeneralApplication.GenApplicationStatus;
                newRow.StCongressCode = shortTermRow.StCongressCode;
                newRow.JSONData = StringHelper.MD5Sum(GeneralApplication.RawApplicationData);

                if (AApplicationStatus.Length == 0)
                {
                    AApplicationStatus = "on hold";
                }

                if (AApplicationStatus != "all")
                {
                    // if there is already an application on hold for that person, drop the old row
                    MainDS.ApplicationGrid.DefaultView.RowFilter =
                        String.Format("JSONData = '{0}' AND {1} = 'H'", newRow.JSONData,
                            ConferenceApplicationTDSApplicationGridTable.GetGenApplicationStatusDBName());

                    while (MainDS.ApplicationGrid.DefaultView.Count > 0)
                    {
                        ConferenceApplicationTDSApplicationGridRow RowToDrop =
                            (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[0].Row;
                        //Console.WriteLine("dropping " + RowToDrop.FamilyName + " " + RowToDrop.FirstName + " " + RowToDrop.PartnerKey.ToString());
                        RowToDrop.Delete();
                    }
                }

                if ((AApplicationStatus == "on hold") && newRow.GenApplicationStatus.StartsWith("H"))
                {
                    MainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if ((AApplicationStatus == "accepted") && newRow.GenApplicationStatus.StartsWith("A"))
                {
                    MainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if ((AApplicationStatus == "cancelled")
                         && ((newRow.GenApplicationStatus.StartsWith("R") || newRow.GenApplicationStatus.StartsWith("C"))))
                {
                    MainDS.ApplicationGrid.Rows.Add(newRow);
                }
                else if (AApplicationStatus == "all")
                {
                    MainDS.ApplicationGrid.Rows.Add(newRow);
                }
            }

            // clear raw data, otherwise this is too big for the javascript client
            foreach (ConferenceApplicationTDSApplicationGridRow row in MainDS.ApplicationGrid.Rows)
            {
                row.JSONData = string.Empty;
            }

            MainDS.AcceptChanges();

            return MainDS;
        }

        /// <summary>
        /// get the number and name of the registration offices that the current user has access for
        /// </summary>
        /// <returns></returns>
        public static PPartnerTable GetRegistrationOffices()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

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
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

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
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            string Result = "Failure, cannot find partner";

            try
            {
                PmGeneralApplicationTable application = PmGeneralApplicationAccess.LoadByPrimaryKey(APartnerKey,
                    AApplicationKey,
                    ARegistrationOfficeKey,
                    Transaction);

                // to avoid the error on the ext.js client: Status Text: BADRESPONSE: Parse Error
                Result = application[0].RawApplicationData.Replace("&quot;", "\\\"");
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Result;
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

            if (!emailSender.SendMessage(ref msg))
            {
                TLogging.Log("There has been a problem sending the email to " + AData.email);
                return false;
            }

            return true;
        }

        private static void InsertDataIntoConferenceApplicationTDS(ConferenceApplicationTDSApplicationGridRow AChangedRow,
            ref ConferenceApplicationTDS AMainDS,
            string AEventCode, TDBTransaction ATransaction)
        {
            if (AChangedRow.RowState == DataRowState.Modified)
            {
                if (ATransaction != null)
                {
                    AMainDS.Merge(LoadApplicationsFromDB(AEventCode, null,
                            null, AChangedRow.PartnerKey,
                            ATransaction));
                }

                AMainDS.PPerson.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PPersonTable.GetPartnerKeyDBName(),
                        AChangedRow.PartnerKey);
                AMainDS.PmShortTermApplication.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PmShortTermApplicationTable.GetPartnerKeyDBName(),
                        AChangedRow.PartnerKey);
                AMainDS.PmGeneralApplication.DefaultView.RowFilter =
                    String.Format("{0}={1}",
                        PmGeneralApplicationTable.GetPartnerKeyDBName(),
                        AChangedRow.PartnerKey);

                PPersonRow Person = (PPersonRow)AMainDS.PPerson.DefaultView[0].Row;
                PmShortTermApplicationRow ShortTermApplication = (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[0].Row;
                PmGeneralApplicationRow GeneralApplication = (PmGeneralApplicationRow)AMainDS.PmGeneralApplication.DefaultView[0].Row;

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

                if ((GeneralApplication.GenApplicationStatus != AChangedRow.GenApplicationStatus) && (AChangedRow.GenApplicationStatus == "A"))
                {
                    if (GeneralApplication.IsGenAppSendFldAcceptDateNull())
                    {
                        GeneralApplication.GenAppSendFldAcceptDate = DateTime.Today;
                    }

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

                GeneralApplication.GenApplicationStatus = AChangedRow.GenApplicationStatus;
                ShortTermApplication.StCongressCode = AChangedRow.StCongressCode;
            }
        }

        /// <summary>
        /// store the adjusted applications to the database
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveApplications(ref ConferenceApplicationTDS AMainDS)
        {
            try
            {
                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    InsertDataIntoConferenceApplicationTDS(row, ref AMainDS, string.Empty, null);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                return TSubmitChangesResult.scrError;
            }


            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result = ConferenceApplicationTDSAccess.SubmitChanges(AMainDS, out VerificationResult);

            // this takes 6 seconds!
            AMainDS.AcceptChanges();

            return result;
        }

        /// <summary>
        /// store the selected application to the database
        /// </summary>
        /// <returns></returns>
        public static TSubmitChangesResult SaveApplication(string AEventCode, ConferenceApplicationTDSApplicationGridRow ARow)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                InsertDataIntoConferenceApplicationTDS(ARow, ref MainDS, AEventCode, Transaction);
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

            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result = ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            ARow.AcceptChanges();

            return result;
        }

        /// <summary>
        /// export accepted and cancelled applications to Petra
        /// </summary>
        /// <returns></returns>
        public static string DownloadApplications(Int64 AEventPartnerKey, string AEventCode, Int64 ARegistrationOffice)
        {
            // TODO: export all partners that have not been imported to the local database yet
            string result = string.Empty;

            result += "PersonPartnerKey;EventPartnerKey;ApplicationDate;AcquisitionCode;Title;FirstName;FamilyName;Street;PostCode;City;";
            result += "Country;Phone;Mobile;Email;DateOfBirth;MaritalStatus;Gender;Vegetarian;MedicalNeeds;ArrivalDate;DepartureDate;";
            result += "EventRole;AppStatus;PreviousAttendance;AppComments;NotesPerson;HorstID;FamilyPartnerKey;RecordImported";
            result = "\"" + result.Replace(";", "\";\"") + "\"\n";

            ConferenceApplicationTDS MainDS = GetApplications(AEventCode, "cancelled", ARegistrationOffice, null);
            MainDS.Merge(GetApplications(AEventCode, "accepted", ARegistrationOffice, null));

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
        /// export accepted applications to an Excel file
        /// </summary>
        /// <returns></returns>
        public static bool DownloadApplications(string AEventCode, ref ConferenceApplicationTDS AMainDS, MemoryStream AStream)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    PmShortTermApplicationRow TemplateRow = AMainDS.PmShortTermApplication.NewRowTyped(false);

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
                    attr = myDoc.CreateAttribute("DateOfBirth");
                    attr.Value = PersonRow.DateOfBirth.Value.ToString("dd-MM-yyyy");
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("DayOfBirth");
                    attr.Value = PersonRow.DateOfBirth.Value.ToString("MMdd");
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("Gender");
                    attr.Value = PersonRow.Gender;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("Role");
                    attr.Value = ShortTermApplicationRow.StCongressCode;
                    newNode.Attributes.Append(attr);
                    attr = myDoc.CreateAttribute("ApplicationDate");
                    attr.Value = GeneralApplicationRow.GenAppDate.ToString("dd-MM-yyyy");
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

                        attr.Value = DateAccepted.ToString("dd-MM-yyyy");
                        newNode.Attributes.Append(attr);
                    }

                    attr = myDoc.CreateAttribute("FieldCharged");
                    attr.Value = ShortTermApplicationRow.StFieldCharged.ToString();
                    newNode.Attributes.Append(attr);

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
        /// Import the file that you get as a result when you import the applications from the Online Registration into your local Petra.
        /// This file contains the partner keys in the local Petra,
        /// and avoids that the registration office has to redo all the importing for the next round of applicants.
        /// </summary>
        /// <param name="APartnerKeyFile"></param>
        /// <returns></returns>
        public static bool UploadPetraImportResult(string APartnerKeyFile)
        {
            XmlDocument partnerKeys = TCsv2Xml.ParseCSV2Xml(APartnerKeyFile);

            SortedList <Int64, XmlNode>PartnerKeys = new SortedList <long, XmlNode>();

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
                    Int64 RegistrationID = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "HorstID"));
                    bool RecordImported = (TXMLParser.GetAttribute(applicant, "RecordImported").ToLower() == "yes");

                    if (RecordImported)
                    {
                        // prepare SELECT WHERE IN (list of partner keys)
                        RegistrationIDs = StringHelper.AddCSV(RegistrationIDs, RegistrationID.ToString(), ",");
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

                foreach (XmlNode applicant in partnerKeys.DocumentElement.ChildNodes)
                {
                    Int64 RegistrationID = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "HorstID"));
                    bool RecordImported = (TXMLParser.GetAttribute(applicant, "RecordImported").ToLower() == "yes");

                    if (RecordImported)
                    {
                        Int64 LocalOfficePartnerKey = Convert.ToInt64(TXMLParser.GetAttribute(applicant, "PersonPartnerKey"));

                        applicationTable.DefaultView.RowFilter = String.Format(
                            "{0} = {1}",
                            PmGeneralApplicationTable.GetPartnerKeyDBName(),
                            RegistrationID);

                        if (applicationTable.DefaultView.Count > 0)
                        {
                            PmGeneralApplicationRow row = (PmGeneralApplicationRow)applicationTable.DefaultView[0].Row;
                            row.LocalPartnerKey = LocalOfficePartnerKey;
                            row.ImportedLocalPetra = true;
                        }
                    }
                }

                // store modified partners
                TVerificationResultCollection VerificationResult;

                if (PmGeneralApplicationAccess.SubmitChanges(applicationTable, Transaction, out VerificationResult))
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    return true;
                }
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

            return true;
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

                if (AVerificationResult.HasCriticalError())
                {
                    return false;
                }

                TVerificationResultCollection VerificationResult;

                if (TSubmitChangesResult.scrOK == PartnerImportExportTDSAccess.SubmitChanges(MainDS, out VerificationResult))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult("importing .ext file", e.Message, TResultSeverity.Resv_Critical));

                return false;
            }

            return true;
        }
    }
}