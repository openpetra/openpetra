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
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>
    /// provides methods for managing the attendees
    /// </summary>
    public class TAttendeeManagement
    {
        /// <summary>
        /// Refresh Outreach Code for applications and conference
        /// </summary>
        public static void RefreshOutreachCode(Int64 AConferenceKey)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = true;
            PcConferenceTable ConferenceTable;
            PUnitTable UnitTable;
            PmShortTermApplicationTable ShortTermAppTable;
            ConferenceApplicationTDS MainDS;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    ConferenceTable = new PcConferenceTable();
                    UnitTable = new PUnitTable();
                    ShortTermAppTable = new PmShortTermApplicationTable();
                    MainDS = new ConferenceApplicationTDS();

                    // get the conference in order to update the OutreachPrefix
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, Transaction);

                    if (ConferenceTable.Count == 0)
                    {
                        throw new Exception("Cannot find conference " + AConferenceKey.ToString());
                    }

                    // update OutreachPrefix in conference record in case it was changed in Unit record for event
                    UnitTable = PUnitAccess.LoadByPrimaryKey(AConferenceKey, Transaction);

                    if (UnitTable[0].OutreachCode.Length >= 5)
                    {
                        ConferenceTable[0].OutreachPrefix = UnitTable[0].OutreachCode.Substring(0, 5);
                    }
                    else
                    {
                        ConferenceTable[0].OutreachPrefix = UnitTable[0].OutreachCode;
                    }

                    MainDS.Merge(ConferenceTable);

                    // update event code
                    ShortTermAppTable = PmShortTermApplicationAccess.LoadViaPUnitStConfirmedOption(AConferenceKey, Transaction);

                    foreach (PmShortTermApplicationRow ShortTermAppRow in ShortTermAppTable.Rows)
                    {
                        ShortTermAppRow.ConfirmedOptionCode = UnitTable[0].OutreachCode;
                    }

                    MainDS.Merge(ShortTermAppTable);

                    MainDS.ThrowAwayAfterSubmitChanges = true;

                    ConferenceApplicationTDSAccess.SubmitChanges(MainDS);
                });
        }

        /// <summary>
        /// Load/Refresh all Attendees for a conference
        /// </summary>
        public static void RefreshAttendees(Int64 AConferenceKey)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = true;
            PcConferenceTable ConferenceTable;
            PUnitTable UnitTable;
            string OutreachPrefix = String.Empty;
            ConferenceApplicationTDS MainDS;

            // make sure outreach codes are up to date in case it has changed in Unit record
            RefreshOutreachCode(AConferenceKey);

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    ConferenceTable = new PcConferenceTable();
                    UnitTable = new PUnitTable();
                    MainDS = new ConferenceApplicationTDS();

                    // get the conference prefix which links all outreaches associated with a conference
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, Transaction);

                    if (ConferenceTable.Count == 0)
                    {
                        throw new Exception("Cannot find conference " + AConferenceKey.ToString());
                    }

                    OutreachPrefix = ConferenceTable[0].OutreachPrefix;

                    // load application data for all conference attendees from db
                    TApplicationManagement.GetApplications(ref MainDS, AConferenceKey, OutreachPrefix, "all", -1, true, null, false);

                    // check a valid pcattendee record exists for each short term application
                    foreach (PmShortTermApplicationRow ShortTermAppRow in MainDS.PmShortTermApplication.Rows)
                    {
                        if (!IsAttendeeValid(MainDS, OutreachPrefix, ShortTermAppRow.PartnerKey))
                        {
                            // ignore deleted applications, or cancelled applications
                            continue;
                        }

                        // update outreach code in application (it may have changed)
                        UnitTable = PUnitAccess.LoadByPrimaryKey(ShortTermAppRow.StConfirmedOption, Transaction);
                        ShortTermAppRow.ConfirmedOptionCode = ((PUnitRow)UnitTable.Rows[0]).OutreachCode;

                        // Do we have a record for this attendee yet?
                        bool AttendeeRecordExists = false;

                        if (MainDS.PcAttendee.Rows.Contains(new object[] { AConferenceKey, ShortTermAppRow.PartnerKey }))
                        {
                            AttendeeRecordExists = true;
                        }

                        // create a new PcAttendee record if one does not already exist for this attendee
                        if (!AttendeeRecordExists)
                        {
                            PcAttendeeRow AttendeeRow = MainDS.PcAttendee.NewRowTyped();

                            AttendeeRow.ConferenceKey = AConferenceKey;
                            AttendeeRow.PartnerKey = ShortTermAppRow.PartnerKey;

                            if (ShortTermAppRow.ConfirmedOptionCode.Length >= 11)
                            {
                                AttendeeRow.OutreachType = ShortTermAppRow.ConfirmedOptionCode.Substring(5, 6);
                            }

                            PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)MainDS.PmGeneralApplication.Rows.Find(
                                new object[] { ShortTermAppRow.PartnerKey, ShortTermAppRow.ApplicationKey, ShortTermAppRow.RegistrationOffice });

                            DateTime DateAccepted = GeneralAppRow.GenAppDate;

                            if (!GeneralAppRow.IsGenAppSendFldAcceptDateNull())
                            {
                                DateAccepted = GeneralAppRow.GenAppSendFldAcceptDate.Value;
                            }
                            else if (!GeneralAppRow.IsGenAppRecvgFldAcceptNull())
                            {
                                DateAccepted = GeneralAppRow.GenAppRecvgFldAccept.Value;
                            }

                            AttendeeRow.Registered = DateAccepted;

                            // TODO: in Petra 2.x, this was calculated from pm_staff_data
                            AttendeeRow.HomeOfficeKey = ShortTermAppRow.RegistrationOffice;

                            if (AttendeeRow.HomeOfficeKey == 0)
                            {
                                AttendeeRow.HomeOfficeKey = ((int)AttendeeRow.PartnerKey / 1000000) * 1000000;
                            }

                            MainDS.PcAttendee.Rows.Add(AttendeeRow);
                        }
                    }

                    PcRoomAllocTable RoomAllocTable = null;
                    PcExtraCostTable ExtraCostTable = null;

                    // now check the other way: all attendees of this conference, are they still valid?
                    foreach (PcAttendeeRow AttendeeRow in MainDS.PcAttendee.Rows)
                    {
                        if ((AttendeeRow.RowState != DataRowState.Added)
                            && !IsAttendeeValid(MainDS, OutreachPrefix, AttendeeRow.PartnerKey))
                        {
                            // remove their accommodation
                            RoomAllocTable = PcRoomAllocAccess.LoadViaPcAttendee(AttendeeRow.ConferenceKey, AttendeeRow.PartnerKey, Transaction);

                            foreach (DataRow Row in RoomAllocTable.Rows)
                            {
                                Row.Delete();
                            }

                            if (RoomAllocTable != null)
                            {
                                PcRoomAllocAccess.SubmitChanges(RoomAllocTable, Transaction);
                            }

                            // remove any extra costs
                            ExtraCostTable = PcExtraCostAccess.LoadViaPcAttendee(AttendeeRow.ConferenceKey, AttendeeRow.PartnerKey, Transaction);

                            foreach (DataRow Row in ExtraCostTable.Rows)
                            {
                                Row.Delete();
                            }

                            if (ExtraCostTable != null)
                            {
                                PcExtraCostAccess.SubmitChanges(ExtraCostTable, Transaction);
                            }

                            // remove attendee
                            AttendeeRow.Delete();
                        }
                    }

                    int shorttermApplicationsCount = MainDS.PmShortTermApplication.Count;
                    int attendeeCount = MainDS.PcAttendee.Count;

                    MainDS.ThrowAwayAfterSubmitChanges = true;

                    ConferenceApplicationTDSAccess.SubmitChanges(MainDS);
                });
        }

        /// <summary>
        /// if attendee is not valid anymore, the attendee should be removed from pc_attendee table
        /// </summary>
        /// <returns></returns>
        private static bool IsAttendeeValid(ConferenceApplicationTDS AMainDS,
            string AOutreachPrefix,
            Int64 AAttendeeKey)
        {
            PmShortTermApplicationRow ShortTermRow = null;

            foreach (PmShortTermApplicationRow Row in AMainDS.PmShortTermApplication.Rows)
            {
                if ((Row.PartnerKey == AAttendeeKey) && (AOutreachPrefix.Length >= 5)
                    && (Row.ConfirmedOptionCode.Substring(0, 5) == AOutreachPrefix.Substring(0, 5)))
                {
                    ShortTermRow = Row;
                    break;
                }
            }

            if (ShortTermRow == null)
            {
                return false;
            }

            if (ShortTermRow.StBasicDeleteFlag)
            {
                return false;
            }

            PmGeneralApplicationRow GeneralAppRow = (PmGeneralApplicationRow)AMainDS.PmGeneralApplication.Rows.Find(
                new object[] { ShortTermRow.PartnerKey, ShortTermRow.ApplicationKey, ShortTermRow.RegistrationOffice });

            if (GeneralAppRow == null)
            {
                return false;
            }

            if (GeneralAppRow.GenAppDeleteFlag)
            {
                return false;
            }

            if (!(GeneralAppRow.GenApplicationStatus.StartsWith("H") || GeneralAppRow.GenApplicationStatus.StartsWith("A")))
            {
                return false;
            }

            return true;
        }
    }
}