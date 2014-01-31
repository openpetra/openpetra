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
    /// useful functions for putting participants of a conference into groups
    /// </summary>
    public class TFellowshipGroups
    {
        /// <summary>
        /// import fellowship groups.
        /// Column1 is Lastname, Column2 is Firstname, Column3 is PartnerKey, Column4 is GroupCode
        /// </summary>
        static public bool ImportFellowshipGroups(
            string AFellowshipGroupsCSV,
            Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ARegisteringOffice)
        {
            string InputSeparator = StringHelper.GetCSVSeparator(AFellowshipGroupsCSV);

            ConferenceApplicationTDS MainDS = new ConferenceApplicationTDS();

            TApplicationManagement.GetApplications(
                ref MainDS,
                AEventPartnerKey,
                AEventCode,
                "accepted",
                ARegisteringOffice,
                String.Empty,
                true);

            try
            {
                MainDS.PmShortTermApplication.DefaultView.Sort = PmShortTermApplicationTable.GetPartnerKeyDBName();

                string[] InputLines = AFellowshipGroupsCSV.Replace("\r", "").Split(new char[] { '\n' });

                int RowCount = 0;

                foreach (string InputLine in InputLines)
                {
                    RowCount++;

                    string line = InputLine;

                    string LastName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    string FirstName = StringHelper.GetNextCSV(ref line, InputSeparator, "");
                    Int64 PartnerKey = StringHelper.TryStrToInt(StringHelper.GetNextCSV(ref line, InputSeparator, ""), -1);
                    string GroupCode = StringHelper.GetNextCSV(ref line, InputSeparator, "");

                    PmShortTermApplicationRow ShorttermAppRow = TApplicationManagement.FindShortTermApplication(MainDS,
                        ref PartnerKey,
                        LastName,
                        FirstName);

                    if (ShorttermAppRow == null)
                    {
                        TLogging.Log(
                            "Import Fellowship groups: Cannot find shortterm application for attendee " + FirstName + " " + LastName + " " +
                            PartnerKey.ToString());
                        continue;
                    }

                    if (ShorttermAppRow.StFgCode.Length == 0)
                    {
                        ShorttermAppRow.StFgCode = GroupCode;
                    }
                    else if (ShorttermAppRow.StFgCode != GroupCode)
                    {
                        ShorttermAppRow.StFgCode = GroupCode;
                        Int32 AttendeeIndex = MainDS.PcAttendee.DefaultView.Find(PartnerKey);

                        if (AttendeeIndex == -1)
                        {
                            TLogging.Log("Cannot find PcAttendee for " + PartnerKey.ToString());
                            continue;
                        }

                        PcAttendeeRow AttendeeRow = (PcAttendeeRow)MainDS.PcAttendee.DefaultView[AttendeeIndex].Row;

                        if (AttendeeRow != null)
                        {
                            // we should reprint this badge, since the group has changed
                            AttendeeRow.SetBadgePrintNull();
                        }
                    }
                }

                ConferenceApplicationTDSAccess.SubmitChanges(MainDS);
            }
            catch (Exception ex)
            {
                TLogging.Log("Importing Fellowship groups: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// assign participants to fellowship groups if they are not assigned to a group yet
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AEventCode"></param>
        /// <param name="ASelectedRegistrationOffice"></param>
        /// <param name="ASelectedRole"></param>
        /// <param name="AMaxGroupMembersInt"></param>
        /// <returns></returns>
        public static bool CalculateFellowshipGroups(
            Int64 AEventPartnerKey,
            string AEventCode,
            Int64 ASelectedRegistrationOffice,
            string ASelectedRole,
            Int32 AMaxGroupMembersInt)
        {
            return true;
        }

        /// <summary>
        /// get all the members of the given group, with partner key and role
        /// </summary>
        public static string GetGroupMembers(string AEventCode, string AGroupCode)
        {
            if (AGroupCode.Trim().Length == 0)
            {
                return string.Empty;
            }

            // cannot use CurrentApplicants, since that might not contain the coach
            ConferenceApplicationTDS MainDS = TApplicationManagement.GetFellowshipGroupMembers(AEventCode, AGroupCode);

            MainDS.PPerson.DefaultView.Sort = PPersonTable.GetPartnerKeyDBName();

            string result = string.Empty;

            foreach (PmShortTermApplicationRow row in MainDS.PmShortTermApplication.Rows)
            {
                PPersonRow person = (PPersonRow)MainDS.PPerson.DefaultView[MainDS.PPerson.DefaultView.Find(row.PartnerKey)].Row;
                result += person.PartnerKey.ToString() + " - " + person.FamilyName + ", " + person.FirstName + " - " + row.StCongressCode +
                          Environment.NewLine;
            }

            return result;
        }
    }
}