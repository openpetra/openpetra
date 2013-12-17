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
using Jayrock.Json;

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
    /// For creating gift batches for conference payments
    /// </summary>
    public class TImportPrintedBadges
    {
        /// <summary>
        /// this is needed for reproducing the date printed of the badges, which seems to have been lost.
        /// it is also useful to compare what has been printed before, and if the fellowship groups have changed, the badges need to be reprinted.
        /// Column1 is Date Printed, Column2 is Firstname, Column3 is LastName, Column4 is PartnerKey, Column5 is fellowshipgroupcode.
        /// </summary>
        static public bool ImportPrintedBadges(
            string APrintedBadgesCSV,
            Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ARegisteringOffice)
        {
            string InputSeparator = StringHelper.GetCSVSeparator(APrintedBadgesCSV);

            TAttendeeManagement.RefreshAttendees(AEventPartnerKey);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();
            TApplicationManagement.GetApplications(
                ref MainDS,
                AEventPartnerKey,
                AEventCode,
                "accepted",
                ARegisteringOffice,
                String.Empty,
                false);

            try
            {
                MainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();

                string[] InputLines = APrintedBadgesCSV.Replace("\r", "").Split(new char[] { '\n' });

                int RowCount = 0;

                foreach (string InputLine in InputLines)
                {
                    RowCount++;

                    string line = InputLine;

                    if ((line.Trim().Length == 0) || line.Trim().StartsWith("#"))
                    {
                        // skip empty lines and comments
                        continue;
                    }

                    DateTime DatePrinted = DateTime.ParseExact(StringHelper.GetNextCSV(ref line, InputSeparator, ""), "yyyy/MM/dd", null);
                    string FirstName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    string LastName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    Int64 PartnerKey = StringHelper.TryStrToInt(StringHelper.GetNextCSV(ref line, InputSeparator, ""), -1);
                    string GroupCode = StringHelper.GetNextCSV(ref line, InputSeparator, "");

                    PmShortTermApplicationRow ShorttermAppRow = TApplicationManagement.FindShortTermApplication(MainDS,
                        ref PartnerKey,
                        LastName,
                        FirstName);

                    if (ShorttermAppRow == null)
                    {
                        TLogging.Log(
                            "Import PrintedBadges: Cannot find shortterm application for attendee " + FirstName + " " + LastName + " " +
                            PartnerKey.ToString());
                        continue;
                    }

                    try
                    {
                        Int32 AttendeeIndex = MainDS.PcAttendee.DefaultView.Find(PartnerKey);

                        if (AttendeeIndex == -1)
                        {
                            TLogging.Log("Cannot find PcAttendee for " + PartnerKey.ToString());
                            continue;
                        }

                        PcAttendeeRow AttendeeRow = (PcAttendeeRow)MainDS.PcAttendee.DefaultView[AttendeeIndex].Row;

                        if (AttendeeRow != null)
                        {
                            AttendeeRow.BadgePrint = DatePrinted;
                        }

                        if (ShorttermAppRow.StFgCode != GroupCode)
                        {
                            AttendeeRow.SetBadgePrintNull();
                        }

                        ConferenceApplicationTDSApplicationGridRow ApplicationRow =
                            (ConferenceApplicationTDSApplicationGridRow)MainDS.ApplicationGrid.DefaultView[MainDS.ApplicationGrid.DefaultView.Find(
                                                                                                               new object[] { ShorttermAppRow.
                                                                                                                              PartnerKey,
                                                                                                                              ShorttermAppRow.
                                                                                                                              ApplicationKey })].Row;

                        Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(ApplicationRow.JSONData);

                        if (rawDataObject.Contains("NickName") && (rawDataObject["NickName"].ToString().Trim().Length > 0))
                        {
                            string NickName = rawDataObject["NickName"].ToString();

                            if (FirstName != NickName)
                            {
                                // don't reprint badges if nickname was not printed the last time
                                // AttendeeRow.SetBadgePrintNull();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("Importing PrintedBadges: " + e.Message + " " + e.ToString());
                    }
                }

                TVerificationResultCollection VerificationResult;

                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            }
            catch (Exception ex)
            {
                TLogging.Log("Importing PrintedBadges: " + ex.Message);
                TLogging.Log(ex.ToString());
                throw;
            }

            return true;
        }
    }
}