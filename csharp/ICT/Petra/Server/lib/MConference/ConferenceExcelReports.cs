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
using System.Data;
using System.IO;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Xml;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// a couple of customized Excel reports
    /// </summary>
    public class TConferenceExcelReports
    {
        /// <summary>
        /// a worksheet for each country, with all expected participants.
        /// with a column to tick off each participant that has arrived
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="AStream">write the Excel file into this stream</param>
        public static bool DownloadArrivalRegistration(Int64 AConferenceKey, string AEventCode, MemoryStream AStream)
        {
            TAttendeeManagement.RefreshAttendees(AConferenceKey, AEventCode);

            PPartnerTable RegistrationOffices = TApplicationManagement.GetRegistrationOffices();

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
            TApplicationManagement.GetApplications(ref MainDS, AConferenceKey, AEventCode, "accepted", -1, null, false);

            try
            {
                SortedList <string, XmlDocument>Worksheets = new SortedList <string, XmlDocument>();

                foreach (PPartnerRow regOffice in RegistrationOffices.Rows)
                {
                    XmlDocument myDoc = TYml2Xml.CreateXmlDocument();
                    Worksheets.Add(regOffice.PartnerKey.ToString(), myDoc);
                }

                MainDS.ApplicationGrid.DefaultView.Sort =
                    ConferenceApplicationTDSApplicationGridTable.GetRegistrationOfficeDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + "," +
                    ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName();

                // go through all accepted applicants
                foreach (DataRowView rv in MainDS.ApplicationGrid.DefaultView)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant = (ConferenceApplicationTDSApplicationGridRow)rv.Row;
                    int AttendeeIndex = MainDS.PcAttendee.DefaultView.Find(applicant.PartnerKey);

                    if (AttendeeIndex == -1)
                    {
                        TLogging.Log("cannot find " + applicant.PartnerKey);
                        continue;
                    }

                    PcAttendeeRow attendee = (PcAttendeeRow)MainDS.PcAttendee.DefaultView[AttendeeIndex].Row;
                    XmlDocument myDoc = Worksheets[applicant.RegistrationOffice.ToString()];

                    XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                    myDoc.DocumentElement.AppendChild(newNode);
                    XmlAttribute attr;

                    attr = myDoc.CreateAttribute("Arrived");
                    attr.Value = "O";
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("RegistrationKey");
                    attr.Value = new TVariant(applicant.PartnerKey).EncodeToString();
                    newNode.Attributes.Append(attr);

                    if (!applicant.IsPersonKeyNull())
                    {
                        attr = myDoc.CreateAttribute("PersonKey");
                        attr.Value = new TVariant(applicant.PersonKey).EncodeToString();
                        newNode.Attributes.Append(attr);
                    }

                    attr = myDoc.CreateAttribute("FamilyName");
                    attr.Value = applicant.FamilyName;
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("Firstname");
                    attr.Value = applicant.FirstName;
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("Role");
                    attr.Value = applicant.StCongressCode;
                    newNode.Attributes.Append(attr);

                    attr = myDoc.CreateAttribute("FGroup");
                    attr.Value = applicant.StFgCode;
                    newNode.Attributes.Append(attr);

                    if (TConferenceFreeTShirt.AcceptedBeforeTShirtDeadLine(attendee, applicant))
                    {
                        Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(TJsonTools.RemoveContainerControls(applicant.JSONData));

                        if (rawDataObject.Contains("TShirtStyle") && rawDataObject.Contains("TShirtSize"))
                        {
                            attr = myDoc.CreateAttribute("FreeTShirt");

                            string tsstyle = rawDataObject["TShirtStyle"].ToString();

                            if (tsstyle.IndexOf("(") != -1)
                            {
                                tsstyle = tsstyle.Substring(0, tsstyle.IndexOf("(") - 1);
                            }

                            string tssize = rawDataObject["TShirtSize"].ToString();

                            if (tssize.IndexOf("(") != -1)
                            {
                                tssize = tssize.Substring(0, tssize.IndexOf("(") - 1);
                            }

                            attr.Value = tsstyle + ", " +
                                         tssize;
                            newNode.Attributes.Append(attr);
                        }
                    }
                }

                return TCsv2Xml.Xml2ExcelStream(Worksheets, AStream);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                throw;
            }
        }
    }
}