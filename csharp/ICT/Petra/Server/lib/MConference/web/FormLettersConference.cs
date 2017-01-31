//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;

namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// methods related to form letters for Conference Module
    ///
    /// </summary>
    public class TFormLettersConferenceWebConnector
    {
        /// <summary>
        /// Fills a Form Letter from Extract
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AExtractId"></param>
        /// <param name="AFormLetterInfo"></param>
        /// <param name="AFormDataList"></param>
        /// <returns></returns>
        [RequireModulePermission("CONFERENCE")]
        public static Boolean FillFormDataFromExtract(Int64 AEventPartnerKey, Int32 AExtractId, TFormLetterInfo AFormLetterInfo,
            out List <TFormData>AFormDataList)
        {
            Boolean ReturnValue = true;

            List <TFormData>dataList = new List <TFormData>();
            MExtractTable ExtractTable;
            Int32 RowCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Create Attendee Form Letter"));

            TDBTransaction ReadTransaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, ReadTransaction);

                    RowCounter = 0;

                    // query all rows of given extract
                    foreach (MExtractRow ExtractRow in ExtractTable.Rows)
                    {
                        RowCounter++;
                        TFormDataAttendee formDataAttendee = new TFormDataAttendee();
                        FillFormDataFromAttendee(AEventPartnerKey, ExtractRow.PartnerKey, formDataAttendee, AFormLetterInfo, ExtractRow.SiteKey,
                            ExtractRow.LocationKey);
                        dataList.Add(formDataAttendee);

                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                        {
                            dataList.Clear();
                            ReturnValue = false;
                            TLogging.Log("Retrieve Conference Form Letter Data - Job cancelled");
                            break;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Retrieving Attendee Data"),
                            (RowCounter * 100) / ExtractTable.Rows.Count);
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            AFormDataList = new List <TFormData>();
            AFormDataList = dataList;
            return ReturnValue;
        }

        /// <summary>
        /// Fills a Form Letter from Extract
        /// </summary>
        /// <param name="AEventPartnerKey"></param>
        /// <param name="AFormLetterInfo"></param>
        /// <param name="AFormDataList"></param>
        /// <returns></returns>
        [RequireModulePermission("CONFERENCE")]
        public static Boolean FillFormDataForAllAttendees(Int64 AEventPartnerKey, TFormLetterInfo AFormLetterInfo,
            out List <TFormData>AFormDataList)
        {
            Boolean ReturnValue = true;

            List <TFormData>dataList = new List <TFormData>();
            PcAttendeeTable AttendeeTable;
            Int32 RowCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Create Attendee Form Letter for All Attendee"));

            TDBTransaction ReadTransaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    AttendeeTable = PcAttendeeAccess.LoadViaPcConference(AEventPartnerKey, ReadTransaction);

                    RowCounter = 0;

                    // query all rows of given extract
                    foreach (PcAttendeeRow AttendeeRow in AttendeeTable.Rows)
                    {
                        RowCounter++;
                        TFormDataAttendee formDataAttendee = new TFormDataAttendee();
                        FillFormDataFromAttendee(AEventPartnerKey, AttendeeRow.PartnerKey, formDataAttendee, AFormLetterInfo);

                        dataList.Add(formDataAttendee);

                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                        {
                            dataList.Clear();
                            ReturnValue = false;
                            TLogging.Log("Retrieve Conference Form Letter Data for All Attendees - Job cancelled");
                            break;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Retrieving Attendee Data"),
                            (RowCounter * 100) / AttendeeTable.Rows.Count);
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            AFormDataList = new List <TFormData>();
            AFormDataList = dataList;
            return ReturnValue;
        }

        /// <summary>
        /// populate form data for given attendee key
        /// This Attendee Data and will also make a call to fill Applicant Data.
        /// </summary>
        /// <param name="AEventPartnerKey">Key of event record to be used</param>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataAttendee">Attendee object to be filled</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("CONFERENCE")]
        public static TFormDataAttendee FillFormDataFromAttendee(Int64 AEventPartnerKey, Int64 APartnerKey,
            TFormDataAttendee AFormDataAttendee,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    FillFormDataFromAttendee(AEventPartnerKey, APartnerKey, AFormDataAttendee, AFormLetterInfo, ReadTransaction, ASiteKey,
                        ALocationKey);
                });

            return AFormDataAttendee;
        }

        /// <summary>
        /// populate form data for given attendee key
        /// This Attendee Data and will also make a call to fill Applicant Data.
        /// </summary>
        /// <param name="AEventPartnerKey">Key of event record to be used</param>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataAttendee">Attendee object to be filled</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="AReadTransaction">DB Transaction</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("CONFERENCE")]
        public static void FillFormDataFromAttendee(Int64 AEventPartnerKey, Int64 APartnerKey,
            TFormDataAttendee AFormDataAttendee,
            TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;

            if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, AReadTransaction))
            {
                // first retrieve all applicant information

                TFormLettersPersonnelWebConnector.FillFormDataFromApplicant(AEventPartnerKey,
                    APartnerKey,
                    AFormDataAttendee,
                    AFormLetterInfo,
                    AReadTransaction,
                    ASiteKey,
                    ALocationKey);

                PcAttendeeTable AttTable;
                PcAttendeeRow AttRow;

                AttTable = PcAttendeeAccess.LoadByPrimaryKey(AEventPartnerKey, APartnerKey, AReadTransaction);

                if (AttTable.Count > 0)
                {
                    AttRow = (PcAttendeeRow)AttTable.Rows[0];
                    AFormDataAttendee.DiscoveryGroup = AttRow.DiscoveryGroup;
                    AFormDataAttendee.WorkGroup = AttRow.WorkGroup;

                    PcConferenceVenueTable VenueTable;
                    PcConferenceVenueRow VenueRow;

                    VenueTable = PcConferenceVenueAccess.LoadViaPcConference(AEventPartnerKey, AReadTransaction);

                    if (VenueTable.Count > 0)
                    {
                        VenueRow = (PcConferenceVenueRow)VenueTable.Rows[0];
                        AFormDataAttendee.Venue = VenueRow.VenueKey.ToString("0000000000");
                        //TODO Buidling, Room, RoomIn, RoomOut, RoomDatesMatchTravelDates, VenueName instead of VenueKey
                    }
                }
            }
        }
    }
}
