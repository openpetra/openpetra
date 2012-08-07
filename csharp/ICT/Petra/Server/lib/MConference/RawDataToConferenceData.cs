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
    /// some tools to copy raw data to the correct conference tables
    /// </summary>
    public class TConferenceToolsRawData
    {
        /// <summary>
        /// copy the arrival and departure dates from the JSON data into the correct fields in the database
        /// </summary>
        public static void FixArrivalDepartureDates(Int64 AEventPartnerKey,
            string AEventCode)
        {
            // get all applications for this conference
            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TApplicationManagement.GetApplications(ref MainDS, AEventPartnerKey, AEventCode, "all", -1, null, false);

            MainDS.ApplicationGrid.DefaultView.Sort = ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName();

            string OldSortShortterm = MainDS.PmShortTermApplication.DefaultView.Sort;
            MainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();

            try
            {
                DateTime DefaultArrivalDate = DateTime.ParseExact(TAppSettingsManager.GetValue(
                        "ConferenceTool.DefaultArrivalDate"), "yyyy/MM/dd", null);
                DateTime DefaultDepartureDate = DateTime.ParseExact(TAppSettingsManager.GetValue(
                        "ConferenceTool.DefaultDepartureDate"), "yyyy/MM/dd", null);
                TLogging.Log(
                    "Zaehle: " + MainDS.PmShortTermApplication.Rows.Count.ToString() + " " + MainDS.PcAttendee.Rows.Count.ToString() + " " +
                    MainDS.ApplicationGrid.Rows.Count.ToString());

                foreach (PcAttendeeRow attendee in MainDS.PcAttendee.Rows)
                {
                    ConferenceApplicationTDSApplicationGridRow applicant =
                        (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[MainDS.ApplicationGrid.DefaultView.Find(
                                                                                                           attendee
                                                                                                           .PartnerKey)].Row;

                    Int32 ShorttermIndex = MainDS.PmShortTermApplication.DefaultView.Find(applicant.PartnerKey);

                    if (ShorttermIndex == -1)
                    {
                        continue;
                    }

                    PmShortTermApplicationRow shortTermRow = (PmShortTermApplicationRow)MainDS.PmShortTermApplication.DefaultView[ShorttermIndex].Row;

                    DateTime Arrival = DefaultArrivalDate;
                    DateTime Departure = DefaultDepartureDate;

                    Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(applicant.JSONData);

                    if (rawDataObject.Contains("DateOfArrival") && (rawDataObject["DateOfArrival"].ToString().Trim().Length > 0))
                    {
                        try
                        {
                            if (rawDataObject["DateOfArrival"].GetType() == typeof(DateTime))
                            {
                                Arrival = (DateTime)rawDataObject["DateOfArrival"];
                            }
                            else
                            {
                                Arrival = Convert.ToDateTime(rawDataObject["DateOfArrival"].ToString());
                            }
                        }
                        catch (Exception exArrivalDate)
                        {
                            TLogging.Log(rawDataObject["DateOfArrival"].ToString() + " " + rawDataObject["DateOfArrival"].GetType().ToString());
                            TLogging.Log(exArrivalDate.ToString());
                        }
                    }

                    if (rawDataObject.Contains("DateOfDeparture") && (rawDataObject["DateOfDeparture"].ToString().Trim().Length > 0))
                    {
                        try
                        {
                            if (rawDataObject["DateOfDeparture"].GetType() == typeof(DateTime))
                            {
                                Arrival = (DateTime)rawDataObject["DateOfDeparture"];
                            }
                            else
                            {
                                Arrival = Convert.ToDateTime(rawDataObject["DateOfArrival"].ToString());
                            }
                        }
                        catch (Exception exDepartureDate)
                        {
                            TLogging.Log(rawDataObject["DateOfDeparture"].ToString() + " " + rawDataObject["DateOfDeparture"].GetType().ToString());
                            TLogging.Log(exDepartureDate.ToString());
                        }
                    }

                    if (shortTermRow.IsArrivalNull())
                    {
                        shortTermRow.Arrival = Arrival;
                    }

                    if (shortTermRow.IsDepartureNull())
                    {
                        shortTermRow.Departure = Departure;
                    }
                }

                TVerificationResultCollection VerificationResult;

                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            }
            catch (Exception ex)
            {
                TLogging.Log("Fixing Arrival and Departure Dates: " + ex.Message);
                TLogging.Log(ex.ToString());
                throw;
            }
            finally
            {
                MainDS.PmShortTermApplication.DefaultView.Sort = OldSortShortterm;
            }
        }
    }
}