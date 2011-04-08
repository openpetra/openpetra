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
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// Manage Conference applications
    /// </summary>
    public class TApplicationManagement
    {
        /// <summary>
        /// return a list of all applicants for a given event, but only the registration office that the user has permissions for, ie. Module REG-00xx0000000
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="AApplicationStatus"></param>
        /// <returns></returns>
        public static ConferenceApplicationTDS GetApplications(string AEventCode, string AApplicationStatus)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            try
            {
                // get all offices that have registrations for this event
                DataTable offices = DBAccess.GDBAccessObj.SelectDT(
                    String.Format("SELECT DISTINCT {0} FROM PUB_{1} WHERE {2} = '{3}'",
                        PmShortTermApplicationTable.GetRegistrationOfficeDBName(),
                        PmShortTermApplicationTable.GetTableDBName(),
                        PmShortTermApplicationTable.GetConfirmedOptionCodeDBName(),
                        AEventCode),
                    "registrationoffice", Transaction);

                foreach (DataRow officeRow in offices.Rows)
                {
                    Int64 RegistrationOffice = Convert.ToInt64(officeRow[0]);
                    try
                    {
                        if (TModuleAccessManager.CheckUserModulePermissions(String.Format("REG-{0:10}",
                                    StringHelper.PartnerKeyToStr(RegistrationOffice))))
                        {
                            MainDS.Merge(GetApplications(AEventCode, RegistrationOffice, AApplicationStatus, Transaction));
                        }
                    }
                    catch (EvaluateException)
                    {
                        // no permissions for this registration office
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

        /// <summary>
        /// return a list of all applicants for a given event
        /// </summary>
        /// <param name="AEventCode"></param>
        /// <param name="ARegisteringOffice"></param>
        /// <param name="AApplicationStatus"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private static ConferenceApplicationTDS GetApplications(string AEventCode,
            Int64 ARegisteringOffice,
            string AApplicationStatus,
            TDBTransaction ATransaction)
        {
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter = new OdbcParameter("registrationoffice", OdbcType.Decimal, 10);
            parameter.Value = ARegisteringOffice;
            parameters.Add(parameter);
            parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());
            parameter.Value = AEventCode;
            parameters.Add(parameter);

            string queryShortTermApplication = "SELECT PUB_pm_short_term_application.* " +
                                               "FROM PUB_pm_short_term_application " +
                                               "WHERE PUB_pm_short_term_application.pm_registration_office_n = ? " +
                                               "  AND PUB_pm_short_term_application.pm_confirmed_option_code_c = ?";
            string queryGeneralApplication = "SELECT PUB_pm_general_application.* " +
                                             "FROM PUB_pm_short_term_application, PUB_pm_general_application " +
                                             "WHERE PUB_pm_short_term_application.pm_registration_office_n = ? " +
                                             "  AND PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                                             "  AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                                             "  AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                                             +
                                             "  AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n";
            string queryPerson = "SELECT PUB_p_person.* " +
                                 "FROM PUB_pm_short_term_application, PUB_p_person " +
                                 "WHERE PUB_pm_short_term_application.pm_registration_office_n = ? " +
                                 "  AND PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                                 "  AND PUB_p_person.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n";


            DBAccess.GDBAccessObj.Select(MainDS,
                queryShortTermApplication,
                MainDS.PmShortTermApplication.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryPerson,
                MainDS.PPerson.TableName, ATransaction, parameters.ToArray());

            DBAccess.GDBAccessObj.Select(MainDS,
                queryGeneralApplication,
                MainDS.PmGeneralApplication.TableName, ATransaction, parameters.ToArray());

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
                newRow.FirstName = Person.FirstName;
                newRow.FamilyName = Person.FamilyName;

                if (!Person.IsDateOfBirthNull())
                {
                    newRow.DateOfBirth = Person.DateOfBirth;
                }

                newRow.Gender = Person.Gender;
                newRow.GenAppDate = GeneralApplication.GenAppDate;

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
                    if (row.RowState == DataRowState.Modified)
                    {
                        AMainDS.PPerson.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PPersonTable.GetPartnerKeyDBName(),
                                row.PartnerKey);
                        AMainDS.PmShortTermApplication.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PmShortTermApplicationTable.GetPartnerKeyDBName(),
                                row.PartnerKey);
                        AMainDS.PmGeneralApplication.DefaultView.RowFilter =
                            String.Format("{0}={1}",
                                PmGeneralApplicationTable.GetPartnerKeyDBName(),
                                row.PartnerKey);

                        PPersonRow Person = (PPersonRow)AMainDS.PPerson.DefaultView[0].Row;
                        PmShortTermApplicationRow ShortTermApplication = (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[0].Row;
                        PmGeneralApplicationRow GeneralApplication = (PmGeneralApplicationRow)AMainDS.PmGeneralApplication.DefaultView[0].Row;

                        Person.FirstName = row.FirstName;
                        Person.FamilyName = row.FamilyName;

                        if (row.DateOfBirth.HasValue)
                        {
                            Person.DateOfBirth = row.DateOfBirth;
                        }
                        else
                        {
                            Person.SetDateOfBirthNull();
                        }

                        Person.Gender = row.Gender;
                        GeneralApplication.GenApplicationStatus = row.GenApplicationStatus;
                        ShortTermApplication.StCongressCode = row.StCongressCode;
                    }
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

            AMainDS.AcceptChanges();

            return result;
        }

        /// <summary>
        /// export accepted applications to Petra
        /// </summary>
        /// <returns></returns>
        public static string DownloadApplications(ref ConferenceApplicationTDS AMainDS)
        {
            // TODO: export all partners that have not been imported to the local database yet
            // TODO: export all partners where application status has changed, cancelled etc
            // TODO: currently exporting all partners that are part of the currently displayed list
            string result = string.Empty;

            result += "PersonPartnerKey;EventPartnerKey;ApplicationDate;AcquisitionCode;Title;FirstName;FamilyName;Street;PostCode;City;";
            result += "Country;Phone;Mobile;Email;DateOfBirth;MaritalStatus;Gender;Vegetarian;MedicalNeeds;ArrivalDate;DepartureDate;";
            result += "EventRole;AppStatus;PreviousAttendance;AppComments;NotesPerson;HorstID\n";

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                foreach (ConferenceApplicationTDSApplicationGridRow row in AMainDS.ApplicationGrid.Rows)
                {
                    PmShortTermApplicationRow TemplateRow = AMainDS.PmShortTermApplication.NewRowTyped(false);

                    // one person is only registered once for the same event. each registration is a new partner key
                    TemplateRow.PartnerKey = row.PartnerKey;
                    // TODO more flexible for event code
                    TemplateRow.ConfirmedOptionCode = "TS111CNGRS.08";
                    PmShortTermApplicationRow ShortTermApplicationRow = PmShortTermApplicationAccess.LoadUsingTemplate(TemplateRow, Transaction)[0];

                    PPersonRow PersonRow = PPersonAccess.LoadByPrimaryKey(ShortTermApplicationRow.PartnerKey, Transaction)[0];
                    PLocationRow LocationRow = PLocationAccess.LoadViaPPartner(PersonRow.FamilyKey, Transaction)[0];
                    PPartnerLocationRow PartnerLocationRow = PPartnerLocationAccess.LoadViaPPartner(PersonRow.FamilyKey, Transaction)[0];

                    PmGeneralApplicationRow GeneralApplicationRow =
                        PmGeneralApplicationAccess.LoadByPrimaryKey(ShortTermApplicationRow.PartnerKey,
                            ShortTermApplicationRow.ApplicationKey,
                            ShortTermApplicationRow.RegistrationOffice,
                            Transaction)[0];

                    // TODO old partner key
                    result += "\"\";";
                    // TODO event partner key in config file? different for each country?
                    result += "\"1110198\";";
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
                    result += "\"" + /* PreviousAttendance + */ "\";";
                    result += "\"" + /* AppComments + */ "\";";
                    result += "\"" + /* NotesPerson + */ "\";";
                    result += "\"" + PersonRow.PartnerKey.ToString() + "\";";
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
    }
}