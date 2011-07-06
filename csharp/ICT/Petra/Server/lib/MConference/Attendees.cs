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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// provides methods for managing the attendees
    /// </summary>
    public class TAttendeeManagement
    {
        /// <summary>
        /// Load/Refresh Attendees
        /// </summary>
        public static void RefreshAttendees(Int64 AConferenceKey, string AEventCode)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            PcConferenceTable ConferenceTable = new PcConferenceTable();
            string OutreachPrefix = String.Empty;

            try
            {
                // Find the conference that has the beginning 5 chars(pre-fix)
                //   we need for finding all related conferences
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, Transaction);

                if (ConferenceTable.Count == 0)
                {
                    throw new Exception("Cannot find conference " + AConferenceKey.ToString());
                }

                OutreachPrefix = ConferenceTable[0].OutreachPrefix;

                // get all conference that are related to the given one
                PcConferenceRow templateConferenceRow = ConferenceTable.NewRowTyped(false);
                templateConferenceRow.OutreachPrefix = OutreachPrefix;

                ConferenceTable = PcConferenceAccess.LoadUsingTemplate(templateConferenceRow, Transaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // Run over all conference that are related to the given one
            foreach (PcConferenceRow ConferenceRow in ConferenceTable.Rows)
            {
                // get all applications for this conference
                ConferenceApplicationTDS MainDS = TApplicationManagement.GetApplications(AEventCode, "all", -1, null);

                // required for DefaultView.Find
                MainDS.PmShortTermApplication.DefaultView.Sort =
                    PmShortTermApplicationTable.GetStConfirmedOptionDBName() + "," +
                    PmShortTermApplicationTable.GetPartnerKeyDBName();
                MainDS.PmGeneralApplication.DefaultView.Sort =
                    PmGeneralApplicationTable.GetPartnerKeyDBName() + "," +
                    PmGeneralApplicationTable.GetApplicationKeyDBName() + "," +
                    PmGeneralApplicationTable.GetRegistrationOfficeDBName();

                Transaction = DBAccess.GDBAccessObj.BeginTransaction();

                try
                {
                    // load all attendees of this conference
                    MainDS.PcAttendee.Clear();
                    PcAttendeeRow templateAttendeeRow = MainDS.PcAttendee.NewRowTyped(false);
                    templateAttendeeRow.ConferenceKey = ConferenceRow.ConferenceKey;
                    PcAttendeeAccess.LoadUsingTemplate(MainDS, templateAttendeeRow, Transaction);
                    MainDS.PcAttendee.DefaultView.Sort = PcAttendeeTable.GetConferenceKeyDBName() + ", " + PcAttendeeTable.GetPartnerKeyDBName();
                }
                finally
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                foreach (PmShortTermApplicationRow ShortTermAppRow in MainDS.PmShortTermApplication.Rows)
                {
                    if (!IsAttendeeValid(MainDS, ConferenceRow.ConferenceKey, ShortTermAppRow.PartnerKey))
                    {
                        // ignore deleted applications, or cancelled applications
                        continue;
                    }

                    // Do we have a record for this attendee yet?
                    if (MainDS.PcAttendee.DefaultView.Find(new object[] { ConferenceRow.ConferenceKey, ShortTermAppRow.PartnerKey }) == -1)
                    {
                        PcAttendeeRow AttendeeRow = MainDS.PcAttendee.NewRowTyped();
                        AttendeeRow.ConferenceKey = ConferenceRow.ConferenceKey;
                        AttendeeRow.PartnerKey = ShortTermAppRow.PartnerKey;

                        DataView GenAppView = MainDS.PmGeneralApplication.DefaultView;
                        int GenAppIndex = GenAppView.Find(new object[] { ShortTermAppRow.PartnerKey, ShortTermAppRow.ApplicationKey,
                                                                         ShortTermAppRow.RegistrationOffice });

                        PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)GenAppView[GenAppIndex].Row;

                        AttendeeRow.Registered = GeneralAppRow.GenAppSendFldAcceptDate;

                        // TODO: in Petra 2.x, this was calculated from pm_staff_data, or from the partner key / 1000000
                        AttendeeRow.HomeOfficeKey = ShortTermAppRow.RegistrationOffice;

                        MainDS.PcAttendee.Rows.Add(AttendeeRow);
                    }
                }

                // now check the other way: all attendees of this conference, are they still valid?
                foreach (PcAttendeeRow AttendeeRow in MainDS.PcAttendee.Rows)
                {
                    if ((AttendeeRow.RowState != DataRowState.Added)
                        && !IsAttendeeValid(MainDS, ConferenceRow.ConferenceKey, AttendeeRow.PartnerKey))
                    {
                        // TODO: delete pc_room_alloc
                        // TODO: delete pc_extra_cost

                        AttendeeRow.Delete();
                    }
                }

                TVerificationResultCollection VerificationResult;
                ConferenceApplicationTDSAccess.SubmitChanges(MainDS, out VerificationResult);

                TLogging.Log(String.Format(
                        "RefreshAttendees: finished. OutreachPrefix: {0}, {1} Shortterm Applications, {2} Attendees",
                        OutreachPrefix, MainDS.PmShortTermApplication.Count, MainDS.PcAttendee.Count));
            }
        }

        /// <summary>
        /// if attendee is not valid anymore, the attendee should be removed from pc_attendee table
        /// </summary>
        /// <returns></returns>
        private static bool IsAttendeeValid(ConferenceApplicationTDS AMainDS,
            Int64 AConferenceKey,
            Int64 AAttendeeKey)
        {
            DataView ShortTermView = AMainDS.PmShortTermApplication.DefaultView;

            int ShortTermIndex = ShortTermView.Find(new object[] { AConferenceKey, AAttendeeKey });

            if (ShortTermIndex == -1)
            {
                return false;
            }

            PmShortTermApplicationRow ShortTermRow = (PmShortTermApplicationRow)ShortTermView[ShortTermIndex].Row;

            if (ShortTermRow.StBasicDeleteFlag)
            {
                return false;
            }

            DataView GenAppView = AMainDS.PmGeneralApplication.DefaultView;

            int GenAppIndex = GenAppView.Find(new object[] { ShortTermRow.PartnerKey, ShortTermRow.ApplicationKey, ShortTermRow.RegistrationOffice });

            if (GenAppIndex == -1)
            {
                return false;
            }

            PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)GenAppView[GenAppIndex].Row;

            if (GeneralAppRow.GenAppDeleteFlag)
            {
                return false;
            }

            // for the moment, we only want to deal with accepted registrations.
            // The online registration puts people on Hold by default, which could cause confusion here
            // if (!(GeneralAppRow.GenApplicationStatus.StartsWith("H") || GeneralAppRow.GenApplicationStatus.StartsWith("A")))
            if (!GeneralAppRow.GenApplicationStatus.StartsWith("A"))
            {
                return false;
            }

            return true;
        }
    }
}