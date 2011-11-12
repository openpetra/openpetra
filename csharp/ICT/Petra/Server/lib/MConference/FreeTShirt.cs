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
    /// free T-Shirt if application was accepted before a given deadline
    /// </summary>
    public class TConferenceFreeTShirt
    {
        private static SortedList <Int64, DateTime>TShirtDeadLines = null;

        /// <summary>
        /// determine if the attendee will get a free T-Shirt or not
        /// </summary>
        /// <param name="AAttendeeRow"></param>
        /// <param name="AApplicant"></param>
        /// <returns></returns>
        public static bool AcceptedBeforeTShirtDeadLine(PcAttendeeRow AAttendeeRow, ConferenceApplicationTDSApplicationGridRow AApplicant)
        {
            if (TShirtDeadLines == null)
            {
                TShirtDeadLines = new SortedList <Int64, DateTime>();

                // load T-Shirt Deadlines from text file
                // format: Partnerkey of Registration office, year, month, day
                // key 0 is the default date

                if (!TAppSettingsManager.HasValue("ConferenceTool.TShirtDeadlines.Path"))
                {
                    throw new Exception("Cannot find ConferenceTool.TShirtDeadlines.Path in config file");
                }

                StreamReader sr = new StreamReader(TAppSettingsManager.GetValue("ConferenceTool.TShirtDeadlines.Path"));

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (!line.Trim().StartsWith("#"))
                    {
                        TShirtDeadLines.Add(Convert.ToInt64(StringHelper.GetNextCSV(ref line)),
                            new DateTime(Convert.ToInt32(StringHelper.GetNextCSV(ref line)),
                                Convert.ToInt32(StringHelper.GetNextCSV(ref line)),
                                Convert.ToInt32(StringHelper.GetNextCSV(ref line))));
                    }
                }
            }

            DateTime DeadLine = TShirtDeadLines[0];

            if (TShirtDeadLines.ContainsKey(AApplicant.RegistrationOffice))
            {
                DeadLine = TShirtDeadLines[AApplicant.RegistrationOffice];
            }

            return AAttendeeRow.Registered.Value.CompareTo(DeadLine) <= 0;
        }

        /// <summary>
        /// we need to know how many applicants from which country will get a T-Shirt,
        /// using the same algorithm as the badge printing procedure
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="AStream">write the Excel file into this stream</param>
        public static bool DownloadTShirtNumbers(Int64 AConferenceKey, string AEventCode, MemoryStream AStream)
        {
            // get all applications for this conference
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TApplicationManagement.GetApplications(ref MainDS, AConferenceKey, AEventCode, "all", -1, null, false);

            // count the T-Shirts
            SortedList <string, Int32>TShirtCountPerCountry = new SortedList <string, int>();
            SortedList <string, Int32>TShirtCount = new SortedList <string, int>();
            MainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName();

            foreach (PcAttendeeRow attendee in MainDS.PcAttendee.Rows)
            {
                ConferenceApplicationTDSApplicationGridRow applicant =
                    (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[MainDS.ApplicationGrid.DefaultView.Find(attendee
                                                                                                       .PartnerKey)].Row;

                Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(applicant.JSONData);

                if (!rawDataObject.Contains("TShirtSize") || !rawDataObject.Contains("TShirtStyle"))
                {
                    continue;
                }

                if (AcceptedBeforeTShirtDeadLine(attendee, applicant))
                {
                    string TShirtId = applicant.RegistrationOffice.ToString() + ", " +
                                      rawDataObject["TShirtStyle"].ToString() + ", " +
                                      rawDataObject["TShirtSize"].ToString();

                    if (TShirtCountPerCountry.ContainsKey(TShirtId))
                    {
                        TShirtCountPerCountry[TShirtId]++;
                    }
                    else
                    {
                        TShirtCountPerCountry.Add(TShirtId, 1);
                    }

                    TShirtId = "Total," + rawDataObject["TShirtStyle"].ToString() + ", " +
                               rawDataObject["TShirtSize"].ToString();

                    if (TShirtCount.ContainsKey(TShirtId))
                    {
                        TShirtCount[TShirtId]++;
                    }
                    else
                    {
                        TShirtCount.Add(TShirtId, 1);
                    }
                }
            }

            // write the result to an Excel file
            XmlDocument myDoc = TYml2Xml.CreateXmlDocument();

            foreach (string key in TShirtCountPerCountry.Keys)
            {
                XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                myDoc.DocumentElement.AppendChild(newNode);
                XmlAttribute attr;

                int Counter = 1;
                string list = key;

                while (list.Length > 0)
                {
                    attr = myDoc.CreateAttribute("column" + Counter.ToString());
                    attr.Value = StringHelper.GetNextCSV(ref list);
                    newNode.Attributes.Append(attr);
                    Counter++;
                }

                attr = myDoc.CreateAttribute("columnCount");
                attr.Value = new TVariant(TShirtCountPerCountry[key]).EncodeToString();
                newNode.Attributes.Append(attr);
            }

            foreach (string key in TShirtCount.Keys)
            {
                XmlNode newNode = myDoc.CreateElement("", "ELEMENT", "");
                myDoc.DocumentElement.AppendChild(newNode);
                XmlAttribute attr;

                int Counter = 1;
                string list = key;

                while (list.Length > 0)
                {
                    attr = myDoc.CreateAttribute("column" + Counter.ToString());
                    attr.Value = StringHelper.GetNextCSV(ref list);
                    newNode.Attributes.Append(attr);
                    Counter++;
                }

                attr = myDoc.CreateAttribute("columnCount");
                attr.Value = new TVariant(TShirtCount[key]).EncodeToString();
                newNode.Attributes.Append(attr);
            }

            return TCsv2Xml.Xml2ExcelStream(myDoc, AStream);
        }
    }
}