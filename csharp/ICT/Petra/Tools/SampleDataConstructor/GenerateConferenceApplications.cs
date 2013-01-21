//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Import;

namespace Ict.Petra.Tools.SampleDataConstructor
{
    /// <summary>
    /// tools for generating applications for a conference
    /// </summary>
    public class SampleDataConferenceApplicants
    {
        private static void GenerateRegistrationOffices(string ACountryName, Int64 APartnerKey, string ACountryCode)
        {
            if (PUnitAccess.Exists(APartnerKey, null))
            {
                TLogging.Log("Office with key " + APartnerKey.ToString() + " already exists.");
                return;
            }

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow partnerRow = MainDS.PPartner.NewRowTyped(true);
            partnerRow.PartnerKey = APartnerKey;
            partnerRow.PartnerShortName = ACountryName;
            partnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
            partnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
            partnerRow.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_ORGANISATION;
            MainDS.PPartner.Rows.Add(partnerRow);

            PUnitRow unitRow = MainDS.PUnit.NewRowTyped(true);
            unitRow.PartnerKey = partnerRow.PartnerKey;
            unitRow.UnitName = partnerRow.PartnerShortName;
            unitRow.CountryCode = ACountryCode;
            unitRow.UnitTypeCode = MPartnerConstants.UNIT_TYPE_FIELD;
            MainDS.PUnit.Rows.Add(unitRow);

            PLocationRow locationRow = MainDS.PLocation.NewRowTyped();
            locationRow.SiteKey = partnerRow.PartnerKey;
            locationRow.LocationKey = 0;
            locationRow.StreetName = "No valid address on file";
            locationRow.CountryCode = ACountryCode;
            MainDS.PLocation.Rows.Add(locationRow);

            PPartnerLocationRow partnerlocationRow = MainDS.PPartnerLocation.NewRowTyped();
            partnerlocationRow.PartnerKey = partnerRow.PartnerKey;
            partnerlocationRow.SiteKey = locationRow.SiteKey;
            partnerlocationRow.LocationKey = locationRow.LocationKey;
            MainDS.PPartnerLocation.Rows.Add(partnerlocationRow);

            PPartnerTypeRow partnertypeRow = MainDS.PPartnerType.NewRowTyped();
            partnertypeRow.PartnerKey = partnerRow.PartnerKey;
            partnertypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_LEDGER;
            MainDS.PPartnerType.Rows.Add(partnertypeRow);

            UmUnitStructureRow unitStructureRow = MainDS.UmUnitStructure.NewRowTyped();
            unitStructureRow.ParentUnitKey = 1000000;
            unitStructureRow.ChildUnitKey = partnerRow.PartnerKey;
            MainDS.UmUnitStructure.Rows.Add(unitStructureRow);

            TVerificationResultCollection Result;
            PartnerEditTDSAccess.SubmitChanges(MainDS, out Result);

            string sqlInsertModule =
                String.Format("INSERT INTO PUB_{0}({1}, {2}) VALUES ('REG-{3:0000000000}','Registration {4}')",
                    SModuleTable.GetTableDBName(),
                    SModuleTable.GetModuleIdDBName(),
                    SModuleTable.GetModuleNameDBName(),
                    APartnerKey,
                    ACountryName);
            string sqlInsertModulePermissions =
                String.Format("INSERT INTO PUB_{0}({1}, {2}, {3}) VALUES ('DEMO', 'REG-{4:0000000000}',true)",
                    SUserModuleAccessPermissionTable.GetTableDBName(),
                    SUserModuleAccessPermissionTable.GetUserIdDBName(),
                    SUserModuleAccessPermissionTable.GetModuleIdDBName(),
                    SUserModuleAccessPermissionTable.GetCanAccessDBName(),
                    APartnerKey);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DBAccess.GDBAccessObj.ExecuteNonQuery(sqlInsertModule, Transaction);
                DBAccess.GDBAccessObj.ExecuteNonQuery(sqlInsertModulePermissions, Transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// generate the applications
        /// </summary>
        public static void GenerateApplications(string AApplicationCSVFile)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AApplicationCSVFile, ",", Encoding.UTF8);

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            // create registration offices
            GenerateRegistrationOffices("United Kingdom", 21000000, "GB");
            GenerateRegistrationOffices("France", 22000000, "FR");
            GenerateRegistrationOffices("Norway", 23000000, "NO");
            GenerateRegistrationOffices("USA", 24000000, "US");
            GenerateRegistrationOffices("Germany", 43000000, "DE");

            PUnitTable unitTable = new PUnitTable();
            // get a list of fields (all class UNIT, with unit type F)
            string sqlGetFieldPartnerKeys = "SELECT * FROM PUB_p_unit WHERE u_unit_type_code_c = 'F'";
            DBAccess.GDBAccessObj.SelectDT(unitTable, sqlGetFieldPartnerKeys, null, new OdbcParameter[0], 0, 0);

            PcConferenceTable conferenceTable = PcConferenceAccess.LoadByPrimaryKey(1110198, null);
            DateTime StartOfConference = conferenceTable[0].Start.Value;

            int counterApplicants = 0;

            while (RecordNode != null)
            {
                string JSONFormData = "{'RegistrationOffice':'#REGISTRATIONOFFICE_VALUE'," +
                                      "'EventIdentifier':'#EVENTCODE','EventPartnerKey':'#EVENTPARTNERKEY'," +
                                      "'RegistrationCountryCode':'#CULTURECODE','EventPartnerKey':'#EVENTPARTNERKEY'," +
                                      "'Role':'#ROLE','FirstName':'#FIRSTNAME','LastName':'#LASTNAME'," +
                                      "'Street':'#STREET','Postcode':'#POSTCODE','City':'#CITY','Country':'#COUNTRY_VALUE'," +
                                      "'Phone':'#PHONE','Email':'#EMAIL','DateOfBirth':'#DATEOFBIRTH','ImageID':'#IMAGEID'," +
                                      "'DateOfArrival':'#DATEOFARRIVAL','DateOfDeparture':'#DATEOFDEPARTURE'," +
                                      "'Gender':'#GENDER','Vegetarian':'#VEGETARIAN','MedicalNeeds':'#MEDICALNEEDS','PaymentInfo':'#PAYMENTINFO'}";

                StringBuilder json = new StringBuilder(JSONFormData);

                Dictionary <string, string>values = new Dictionary <string, string>();

                values.Add("EventCode", "SC001CNGRSS08");
                values.Add("EventPartnerKey", conferenceTable[0].ConferenceKey.ToString());

                string cultureCode = TXMLParser.GetAttribute(RecordNode, "RegistrationCountryCode");
                cultureCode = cultureCode.Substring(0, 2) + "-" + cultureCode.Substring(2, 2);
                values.Add("RegistrationCountryCode", cultureCode);

                Int64 RegistrationOffice = 43000000;
                string CountryIsoCode = "DE";

                foreach (PUnitRow unitRow in unitTable.Rows)
                {
                    if (cultureCode.EndsWith(unitRow.CountryCode))
                    {
                        RegistrationOffice = unitRow.PartnerKey;
                        CountryIsoCode = unitRow.CountryCode;
                    }
                }

                string role = TXMLParser.GetAttribute(RecordNode, "role");
                int age = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "age"));

                if (role == "TEEN")
                {
                    // make the age fit
                    age = 12 + age % 6;
                }
                else if (role == "CHILD")
                {
                    age = age % 11;
                }
                else
                {
                    age = 18 + age % 40;
                }

                DateTime DateOfBirth = Convert.ToDateTime(
                    TXMLParser.GetAttribute(RecordNode, "DateOfBirth"));
                int CurrentAge = StartOfConference.Year - DateOfBirth.Year;

                if (DateOfBirth > StartOfConference.AddYears(-age))
                {
                    CurrentAge--;
                }

                DateOfBirth = DateOfBirth.AddYears(CurrentAge - age);

                if (age <= 11)
                {
                    role = "TS-CHILD";
                }
                else if (age <= 15)
                {
                    role = "TS-TEEN-A";
                }
                else if (age <= 17)
                {
                    role = "TS-TEEN-O";
                }
                else
                {
                    role = "TS-" + TXMLParser.GetAttribute(RecordNode, "role");
                }

                values.Add("RegistrationOffice_Value", RegistrationOffice.ToString());
                values.Add("Role", role);
                values.Add("FormsId", "\"");
                values.Add("culturecode", cultureCode);

                values.Add("FirstName", TXMLParser.GetAttribute(RecordNode, "FirstName"));
                values.Add("LastName", TXMLParser.GetAttribute(RecordNode, "FamilyName"));
                values.Add("Gender",
                    (TXMLParser.GetAttribute(RecordNode, "Gender") == "MALE" ? "Male" : "Female"));
                values.Add("Vegetarian", "No");

                string EmailAddress = TXMLParser.GetAttribute(RecordNode, "Email");
                EmailAddress = EmailAddress.Substring(0, EmailAddress.IndexOf("@")) + "@sample.openpetra.org";

                values.Add("Email", EmailAddress);
                values.Add("Street", TXMLParser.GetAttribute(RecordNode, "Street"));
                values.Add("Postcode", TXMLParser.GetAttribute(RecordNode, "PostCode"));
                values.Add("City", TXMLParser.GetAttribute(RecordNode, "City"));
                values.Add("Country_VALUE", CountryIsoCode);
                values.Add("MedicalNeeds", "test with \"quote\" in text");
                values.Add("PaymentInfo", "NONE");

                Catalog.Init("en-GB", cultureCode);
                values.Add("DateOfBirth", DateOfBirth.ToShortDateString()); // in the culture of the country code
                values.Add("DateOfArrival", StartOfConference.ToShortDateString());
                values.Add("DateOfDeparture", StartOfConference.AddDays(5).ToShortDateString());

                values.Add("IMAGEID", "temp.jpg");

                // copy photo to the data/photos directory
                string photo = "469px-Ernest_Hemingway_1923_passport_photo.TIF.jpg";

                if (TXMLParser.GetAttribute(RecordNode, "Gender") == "FEMALE")
                {
                    photo = "388px-Droste-Hülshoff_2.jpg";
                }

                File.Copy(TAppSettingsManager.GetValue("Server.PathTemp") + "/../webserver/Samples/UploadDemo/" + photo,
                    TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar +
                    "temp.jpg", true);

                foreach (string key in values.Keys)
                {
                    string value = values[key].ToString().Trim();

                    json.Replace("#" + key.ToUpper(), value);
                }

                string result = TImportPartnerForm.DataImportFromForm("RegisterPerson", json.ToString(), false);

                if (TLogging.DebugLevel >= 10)
                {
                    TLogging.Log(result);
                }

                counterApplicants++;

                if (counterApplicants % 100 == 0)
                {
                    TLogging.Log("created " + counterApplicants.ToString() + " applicants");
                }

                RecordNode = RecordNode.NextSibling;
            }

            // TODO accept applications

            // TODO give permissions to Demo user. create one user for each registration office
        }
    }
}