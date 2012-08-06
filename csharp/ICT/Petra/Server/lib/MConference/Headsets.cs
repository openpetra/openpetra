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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Server.MPartner.ImportExport;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// useful functions for handing out and collecting headsets for specific sessions during a conference
    /// </summary>
    public class THeadsetManagement
    {
        private static string SESSION_CONTACT_ATTRIBUTE = "SESSION";
        private static string HEADSET_OUT_METHOD_OF_CONTACT = "HEADSET_OUT";
        private static string HEADSET_RETURN_METHOD_OF_CONTACT = "HEADSET_RETURN";
        private static string MODULE_HEADSET = "HEADSET";

        /// <summary>
        /// save message for home office reps
        /// </summary>
        /// <param name="AMessage"></param>
        public static void SetMessageForHomeOfficeReps(string AMessage)
        {
            SLogonMessageTable table = SLogonMessageAccess.LoadByPrimaryKey("HU", null);

            if (table.Rows.Count == 0)
            {
                SLogonMessageRow row = table.NewRowTyped(true);
                row.LanguageCode = "HU";
                table.Rows.Add(row);
            }

            table[0].LogonMessage = AMessage;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            TVerificationResultCollection Verification;
            SLogonMessageAccess.SubmitChanges(table, Transaction, out Verification);

            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// get message for home office reps
        /// </summary>
        public static string GetMessageForHomeOfficeReps()
        {
            SLogonMessageTable table = SLogonMessageAccess.LoadByPrimaryKey("HU", null);

            if (table.Rows.Count > 0)
            {
                return table[0].LogonMessage;
            }

            return string.Empty;
        }

        /// <summary>
        /// get the sessions available for headset usage
        /// </summary>
        public static PContactAttributeDetailTable GetSessions()
        {
            return PContactAttributeDetailAccess.LoadViaPContactAttribute(SESSION_CONTACT_ATTRIBUTE, null);
        }

        /// <summary>
        /// create a new sessions where the participants can use their headset
        /// </summary>
        public static void AddSession(string ASessionName)
        {
            if (!UserInfo.GUserInfo.IsInModule(MODULE_HEADSET))
            {
                return;
            }

            // todo check permissions
            if (ASessionName.Trim().Length == 0)
            {
                return;
            }

            PContactAttributeDetailTable table = new PContactAttributeDetailTable();
            PContactAttributeDetailRow row = table.NewRowTyped(true);
            row.ContactAttributeCode = SESSION_CONTACT_ATTRIBUTE;
            row.ContactAttrDetailCode = ASessionName;
            row.ContactAttrDetailDescr = ASessionName;
            table.Rows.Add(row);

            TDBTransaction writeTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            TVerificationResultCollection VerificationResult;
            table.ThrowAwayAfterSubmitChanges = true;

            try
            {
                if (PContactAttributeDetailAccess.SubmitChanges(table, writeTransaction, out VerificationResult))
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// add partner key from the scanner of the badges
        /// </summary>
        public static bool AddScannedKeys(string ASessionName, string APartnerKeys, bool AHandingOutHeadset)
        {
            if (!UserInfo.GUserInfo.IsInModule(MODULE_HEADSET))
            {
                return false;
            }

            if (APartnerKeys.Trim().Length == 0)
            {
                return true;
            }

            ContactTDS MainDS = new ContactTDS();

            string[] InputLines = APartnerKeys.Replace("\r", "").Split(new char[] { '\n' });

            List <Int64>Keys = new List <long>();

            foreach (string InputLine in InputLines)
            {
                Int64 PartnerKey = Convert.ToInt64(InputLine);

                if (Keys.Contains(PartnerKey))
                {
                    continue;
                }

                Keys.Add(PartnerKey);

                PPartnerContactRow PartnerContactRow = MainDS.PPartnerContact.NewRowTyped(true);
                PartnerContactRow.ContactId = (MainDS.PPartnerContact.Rows.Count + 1) * -1;
                PartnerContactRow.PartnerKey = PartnerKey;
                PartnerContactRow.ContactCode = (AHandingOutHeadset ? HEADSET_OUT_METHOD_OF_CONTACT : HEADSET_RETURN_METHOD_OF_CONTACT);
                PartnerContactRow.ContactDate = DateTime.Now;
                PartnerContactRow.ContactTime = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds);
                MainDS.PPartnerContact.Rows.Add(PartnerContactRow);

                PPartnerContactAttributeRow PartnerContactAttributeRow = MainDS.PPartnerContactAttribute.NewRowTyped(true);
                PartnerContactAttributeRow.ContactId = PartnerContactRow.ContactId;
                PartnerContactAttributeRow.ContactAttributeCode = SESSION_CONTACT_ATTRIBUTE;
                PartnerContactAttributeRow.ContactAttrDetailCode = ASessionName;
                MainDS.PPartnerContactAttribute.Rows.Add(PartnerContactAttributeRow);
            }

            TVerificationResultCollection VerificationResult;
            MainDS.ThrowAwayAfterSubmitChanges = true;
            return TSubmitChangesResult.scrOK == ContactTDSAccess.SubmitChanges(MainDS, out VerificationResult);
        }

        private static void GetDataForHeadsetReports(string AEventCode,
            string ASessionName,
            out DataTable AHeadsetsTable)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string stmtReportHeadsetsForSession = TDataBase.ReadSqlFile("Conference.ReportHeadsetsForSession.sql");

                if (!UserInfo.GUserInfo.IsInModule(MODULE_HEADSET))
                {
                    // only get the headsets for the offices that this person has permissions for
                    PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();

                    string ByRegistrationOffice = " AND PUB_pm_short_term_application.pm_registration_office_n IN (";

                    foreach (PPartnerRow partnerRow in offices.Rows)
                    {
                        ByRegistrationOffice += partnerRow.PartnerKey.ToString() + ",";
                    }

                    ByRegistrationOffice += "0)";

                    stmtReportHeadsetsForSession += ByRegistrationOffice;
                }

                OdbcParameter parameter;

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                parameter = new OdbcParameter("SessionName", OdbcType.VarChar);
                parameter.Value = ASessionName;
                parameters.Add(parameter);
                parameter = new OdbcParameter("EventCode", OdbcType.VarChar);
                parameter.Value = AEventCode;
                parameters.Add(parameter);

                AHeadsetsTable = DBAccess.GDBAccessObj.SelectDT(stmtReportHeadsetsForSession, "headsets", Transaction, parameters.ToArray());
                AHeadsetsTable.PrimaryKey = new DataColumn[] {
                    AHeadsetsTable.Columns["PartnerKey"],
                    AHeadsetsTable.Columns["RentedOutOrReturned"]
                };
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        private static void GetDataForHeadsetReports(string AEventCode,
            out DataTable AAttendeesTable)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string stmtAllParticipantsWithRoleAndCountry = TDataBase.ReadSqlFile("Conference.GetAllParticipantsWithRoleAndCountry.sql");

                if (!UserInfo.GUserInfo.IsInModule(MODULE_HEADSET))
                {
                    // only get the headsets for the offices that this person has permissions for
                    PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();

                    string ByRegistrationOffice = " AND PUB_pm_short_term_application.pm_registration_office_n IN (";

                    foreach (PPartnerRow partnerRow in offices.Rows)
                    {
                        ByRegistrationOffice += partnerRow.PartnerKey.ToString() + ",";
                    }

                    ByRegistrationOffice += "0)";

                    stmtAllParticipantsWithRoleAndCountry += ByRegistrationOffice;
                }

                OdbcParameter parameter;

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                parameter = new OdbcParameter("EventCode", OdbcType.VarChar);
                parameter.Value = AEventCode;
                parameters.Add(parameter);

                AAttendeesTable = DBAccess.GDBAccessObj.SelectDT(stmtAllParticipantsWithRoleAndCountry, "attendees", Transaction, parameters.ToArray());
                AAttendeesTable.PrimaryKey = new DataColumn[] {
                    AAttendeesTable.Columns["PartnerKey"]
                };
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// list all partners that have not returned their headsets yet
        private static XmlDocument GetUnreturnedHeadsets(DataTable AHeadsetsTable)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            foreach (DataRow row in AHeadsetsTable.Rows)
            {
                if (row["RentedOutOrReturned"].ToString() == HEADSET_OUT_METHOD_OF_CONTACT)
                {
                    if (null == AHeadsetsTable.Rows.Find(new object[] { row["PartnerKey"], HEADSET_RETURN_METHOD_OF_CONTACT }))
                    {
                        // add partner to Excel file
                        XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                        myDoc.DocumentElement.AppendChild(newNode);
                        XmlAttribute attr;

                        attr = myDoc.CreateAttribute("PartnerKey");
                        attr.Value = row["PartnerKey"].ToString();
                        newNode.Attributes.Append(attr);

                        attr = myDoc.CreateAttribute("ShortName");
                        attr.Value = row["ShortName"].ToString();
                        newNode.Attributes.Append(attr);

                        attr = myDoc.CreateAttribute("Role");
                        attr.Value = row["Role"].ToString();
                        newNode.Attributes.Append(attr);

                        attr = myDoc.CreateAttribute("FellowshipGroup");
                        attr.Value = row["FellowshipGroup"].ToString();
                        newNode.Attributes.Append(attr);

                        attr = myDoc.CreateAttribute("Country");
                        attr.Value = row["Country"].ToString();
                        newNode.Attributes.Append(attr);
                    }
                }
            }

            return myDoc;
        }

        private static XmlDocument GetStatisticsPerCountry(DataTable AHeadsetsTable, DataTable AttendeesTable)
        {
            SortedList <string, Int32>HeadsetsPerCountryAndRole = new SortedList <string, Int32>();
            SortedList <string, Int32>ParticipantsPerCountryAndRole = new SortedList <string, Int32>();

            foreach (DataRow rowAttendee in AttendeesTable.Rows)
            {
                string Country = rowAttendee["Country"].ToString() + " _ Total";
                string CountryAndRole = rowAttendee["Country"].ToString() + " _ " + rowAttendee["Role"].ToString();

                if ((null != AHeadsetsTable.Rows.Find(new object[] { rowAttendee["PartnerKey"], HEADSET_OUT_METHOD_OF_CONTACT }))
                    || (null != AHeadsetsTable.Rows.Find(new object[] { rowAttendee["PartnerKey"], HEADSET_RETURN_METHOD_OF_CONTACT })))
                {
                    if (!HeadsetsPerCountryAndRole.ContainsKey(CountryAndRole))
                    {
                        HeadsetsPerCountryAndRole.Add(CountryAndRole, 1);
                    }
                    else
                    {
                        HeadsetsPerCountryAndRole[CountryAndRole]++;
                    }

                    if (!HeadsetsPerCountryAndRole.ContainsKey(Country))
                    {
                        HeadsetsPerCountryAndRole.Add(Country, 1);
                    }
                    else
                    {
                        HeadsetsPerCountryAndRole[Country]++;
                    }
                }

                if (!ParticipantsPerCountryAndRole.ContainsKey(CountryAndRole))
                {
                    ParticipantsPerCountryAndRole.Add(CountryAndRole, 1);
                }
                else
                {
                    ParticipantsPerCountryAndRole[CountryAndRole]++;
                }

                if (!ParticipantsPerCountryAndRole.ContainsKey(Country))
                {
                    ParticipantsPerCountryAndRole.Add(Country, 1);
                }
                else
                {
                    ParticipantsPerCountryAndRole[Country]++;
                }
            }

            XmlDocument statsPerCountry = TYml2Xml.CreateXmlDocument();

            foreach (string country in ParticipantsPerCountryAndRole.Keys)
            {
                // ignore rows without any rented out headsets
                if (!HeadsetsPerCountryAndRole.ContainsKey(country))
                {
                    continue;
                }

                XmlNode newNode = statsPerCountry.CreateElement("", "ELEMENT", "");
                statsPerCountry.DocumentElement.AppendChild(newNode);
                XmlAttribute attr;

                attr = statsPerCountry.CreateAttribute("Country");
                attr.Value = country.Substring(0, country.IndexOf(" _ "));
                newNode.Attributes.Append(attr);

                attr = statsPerCountry.CreateAttribute("Role");
                attr.Value = country.Substring(country.IndexOf(" _ ") + 3);
                newNode.Attributes.Append(attr);

                attr = statsPerCountry.CreateAttribute("RentedOut");
                attr.Value = HeadsetsPerCountryAndRole[country].ToString();
                newNode.Attributes.Append(attr);

                attr = statsPerCountry.CreateAttribute("Attendees");
                attr.Value = ParticipantsPerCountryAndRole[country].ToString();;
                newNode.Attributes.Append(attr);
            }

            return statsPerCountry;
        }

        private static XmlDocument GetHeadsetUsers(DataTable AHeadsetsTable, DataTable AttendeesTable, bool AUsedHeadset)
        {
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            foreach (DataRow row in AttendeesTable.Rows)
            {
                if (AUsedHeadset ==
                    (null != AHeadsetsTable.Rows.Find(new object[] { row["PartnerKey"], HEADSET_OUT_METHOD_OF_CONTACT })))
                {
                    XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                    myDoc.DocumentElement.AppendChild(newNode);
                    XmlAttribute attr;

                    attr = myDoc.CreateAttribute("PartnerKey");
                    attr.Value = row["PartnerKey"].ToString();
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("ShortName");
                    attr.Value = row["ShortName"].ToString();
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("Role");
                    attr.Value = row["Role"].ToString();
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("FellowshipGroup");
                    attr.Value = row["FellowshipGroup"].ToString();
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("Country");
                    attr.Value = row["Country"].ToString();
                    newNode.Attributes.Append(attr);
                }
            }

            return myDoc;
        }

        /// <summary>
        /// get Excel file with unreturned headsets
        /// </summary>
        public static bool ReportHeadsetsPerSession(MemoryStream AStream, string AEventCode, string ASessionName)
        {
            DataTable HeadsetsTable;
            DataTable AttendeesTable;

            GetDataForHeadsetReports(AEventCode, ASessionName, out HeadsetsTable);
            GetDataForHeadsetReports(AEventCode, out AttendeesTable);

            SortedList <string, XmlDocument>worksheets = new SortedList <string, XmlDocument>();
            worksheets.Add("a - Unreturned", GetUnreturnedHeadsets(HeadsetsTable));
            worksheets.Add("b - Statistics per country", GetStatisticsPerCountry(HeadsetsTable, AttendeesTable));

            if (!UserInfo.GUserInfo.IsInModule(MODULE_HEADSET))
            {
                // add a list of all people who have or have not used the headset
                worksheets.Add("c - No headset", GetHeadsetUsers(HeadsetsTable, AttendeesTable, false));
                worksheets.Add("d - With headset", GetHeadsetUsers(HeadsetsTable, AttendeesTable, true));
            }

            return TCsv2Xml.Xml2ExcelStream(worksheets, AStream, false);
        }

        private static XmlDocument GenerateWorksheetStatistics(
            PContactAttributeDetailTable sessions,
            SortedList <string, SortedList <string, Int32>>HeadsetsPerCountryAndSession)
        {
            XmlDocument statistics = TYml2Xml.CreateXmlDocument();

            // first add a row with empty elements, for each session, to keep the right order of sessions
            XmlNode newNode = statistics.CreateElement("", "ELEMENT", "");

            statistics.DocumentElement.AppendChild(newNode);
            XmlAttribute attr;

            attr = statistics.CreateAttribute("Country");
            attr.Value = string.Empty;
            newNode.Attributes.Append(attr);

            for (int counterSession = sessions.Rows.Count - 1; counterSession >= 0; counterSession--)
            {
                string sessionName = sessions[counterSession].ContactAttrDetailCode;

                attr = statistics.CreateAttribute(sessionName.Replace(" ", "_"));
                attr.Value = string.Empty;
                newNode.Attributes.Append(attr);
            }

            foreach (string country in HeadsetsPerCountryAndSession.Keys)
            {
                newNode = statistics.CreateElement("", "ELEMENT", "");
                statistics.DocumentElement.AppendChild(newNode);

                attr = statistics.CreateAttribute("Country");
                attr.Value = country;
                newNode.Attributes.Append(attr);

                foreach (string sessionName in HeadsetsPerCountryAndSession[country].Keys)
                {
                    attr = statistics.CreateAttribute(sessionName.Replace(" ", "_"));
                    attr.Value = HeadsetsPerCountryAndSession[country][sessionName].ToString();
                    newNode.Attributes.Append(attr);
                }
            }

            newNode = statistics.CreateElement("", "ELEMENT", "");
            statistics.DocumentElement.AppendChild(newNode);

            newNode = statistics.CreateElement("", "ELEMENT", "");
            statistics.DocumentElement.AppendChild(newNode);

            attr = statistics.CreateAttribute("Country");
            attr.Value = "Total";
            newNode.Attributes.Append(attr);

            for (int counterSession = sessions.Rows.Count - 1; counterSession >= 0; counterSession--)
            {
                string sessionName = sessions[counterSession].ContactAttrDetailCode;
                Int32 Total = 0;

                foreach (string country in HeadsetsPerCountryAndSession.Keys)
                {
                    if (HeadsetsPerCountryAndSession[country].ContainsKey(sessionName))
                    {
                        Total += HeadsetsPerCountryAndSession[country][sessionName];
                    }
                }

                attr = statistics.CreateAttribute(sessionName.Replace(" ", "_"));
                attr.Value = Total.ToString();
                newNode.Attributes.Append(attr);
            }

            return statistics;
        }

        /// report how many people have used the headsets how many times
        private static XmlDocument GenerateWorksheetUsageByPerson(SortedList <Int64, Int32>UsagePerPartner)
        {
            SortedList <Int32, Int32>CountUsage = new SortedList <int, int>();

            foreach (Int32 count in UsagePerPartner.Values)
            {
                if (!CountUsage.ContainsKey(count))
                {
                    CountUsage.Add(count, 1);
                }
                else
                {
                    CountUsage[count]++;
                }
            }

            XmlDocument statistics = TYml2Xml.CreateXmlDocument();

            foreach (int count in CountUsage.Keys)
            {
                XmlNode newNode = statistics.CreateElement("", "ELEMENT", "");
                statistics.DocumentElement.AppendChild(newNode);

                XmlAttribute attr = statistics.CreateAttribute("NumberOfRentals");
                attr.Value = count.ToString();
                newNode.Attributes.Append(attr);

                attr = statistics.CreateAttribute("NumberOfPeople");
                attr.Value = CountUsage[count].ToString();
                newNode.Attributes.Append(attr);

                // count all people who have at least rented count many times
                Int32 Total = 0;

                foreach (int count2 in CountUsage.Keys)
                {
                    if (count2 >= count)
                    {
                        Total += CountUsage[count2];
                    }
                }

                attr = statistics.CreateAttribute("NumberOfPeopleForDraw");
                attr.Value = Total.ToString();
                newNode.Attributes.Append(attr);
            }

            return statistics;
        }

        private static XmlDocument GetPeopleWithMinimumRentalTimes(DataTable AttendeesTable,
            SortedList <Int64, Int32>UsagePerPartner,
            int AMinimumRental)
        {
            XmlDocument statistics = TYml2Xml.CreateXmlDocument();

            int counter = 1;

            foreach (Int64 partnerkey in UsagePerPartner.Keys)
            {
                if (UsagePerPartner[partnerkey] >= AMinimumRental)
                {
                    DataRow row = AttendeesTable.Rows.Find(partnerkey);

                    XmlNode newNode = statistics.CreateElement("", "ELEMENT", "");
                    statistics.DocumentElement.AppendChild(newNode);

                    XmlAttribute attr = statistics.CreateAttribute("Row");
                    attr.Value = counter.ToString();
                    newNode.Attributes.Append(attr);
                    attr = statistics.CreateAttribute("PartnerKey");
                    attr.Value = partnerkey.ToString();
                    newNode.Attributes.Append(attr);
                    attr = statistics.CreateAttribute("Name");
                    attr.Value = row["ShortName"].ToString();
                    newNode.Attributes.Append(attr);
                    attr = statistics.CreateAttribute("Country");
                    attr.Value = row["Country"].ToString();
                    newNode.Attributes.Append(attr);
                    attr = statistics.CreateAttribute("NumberOfSessions");
                    attr.Value = UsagePerPartner[partnerkey].ToString();
                    newNode.Attributes.Append(attr);

                    counter++;
                }
            }

            return statistics;
        }

        /// <summary>
        /// get Excel file with statistics for all countries and sessions
        /// </summary>
        public static bool ReportOverallStatistics(MemoryStream AStream, string AEventCode)
        {
            try
            {
                // get all sessions
                PContactAttributeDetailTable sessions = GetSessions();

                DataTable HeadsetsTable;
                DataTable AttendeesTable;

                GetDataForHeadsetReports(AEventCode, out AttendeesTable);

                SortedList <string, SortedList <string, Int32>>HeadsetsPerCountryAndSession =
                    new SortedList <string, SortedList <string, Int32>>();

                SortedList <Int64, Int32>UsagePerPartner = new SortedList <Int64, Int32>();

                foreach (PContactAttributeDetailRow row in sessions.Rows)
                {
                    string sessionName = row.ContactAttrDetailCode;

                    GetDataForHeadsetReports(AEventCode, sessionName, out HeadsetsTable);

                    foreach (DataRow rowAttendee in AttendeesTable.Rows)
                    {
                        string country = rowAttendee["Country"].ToString();
                        Int64 PartnerKey = Convert.ToInt64(rowAttendee["PartnerKey"]);

                        if ((null != HeadsetsTable.Rows.Find(new object[] { PartnerKey, HEADSET_OUT_METHOD_OF_CONTACT }))
                            || (null != HeadsetsTable.Rows.Find(new object[] { PartnerKey, HEADSET_RETURN_METHOD_OF_CONTACT })))
                        {
                            if (!HeadsetsPerCountryAndSession.ContainsKey(country))
                            {
                                HeadsetsPerCountryAndSession.Add(country, new SortedList <string, Int32>());
                            }

                            if (!HeadsetsPerCountryAndSession[country].ContainsKey(sessionName))
                            {
                                HeadsetsPerCountryAndSession[country].Add(sessionName, 1);
                            }
                            else
                            {
                                HeadsetsPerCountryAndSession[country][sessionName]++;
                            }

                            if (!UsagePerPartner.ContainsKey(PartnerKey))
                            {
                                UsagePerPartner.Add(PartnerKey, 1);
                            }
                            else
                            {
                                UsagePerPartner[PartnerKey]++;
                            }
                        }
                    }
                }

                SortedList <string, XmlDocument>worksheets = new SortedList <string, XmlDocument>();
                worksheets.Add("a - By country", GenerateWorksheetStatistics(sessions, HeadsetsPerCountryAndSession));
                worksheets.Add("b - Counting per person", GenerateWorksheetUsageByPerson(UsagePerPartner));
                worksheets.Add("c - At least 1 session", GetPeopleWithMinimumRentalTimes(AttendeesTable, UsagePerPartner, 1));
                worksheets.Add("d - At least 9 sessions", GetPeopleWithMinimumRentalTimes(AttendeesTable, UsagePerPartner, 9));
                worksheets.Add("e - At least 10 sessions", GetPeopleWithMinimumRentalTimes(AttendeesTable, UsagePerPartner, 10));

                return TCsv2Xml.Xml2ExcelStream(worksheets, AStream, false);
            }
            catch (Exception ex)
            {
                TLogging.Log("In THeadsetManagement.ReportOverallStatistics: " + ex.ToString());
                return false;
            }
        }
    }
}