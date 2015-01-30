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


namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TConferenceMasterDataWebConnector
    {
        /// <summary>
        /// returns conference master settings
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="AConferenceName"></param>
        /// <returns></returns>
        [RequireModulePermission("CONFERENCE")]
        public static ConferenceSetupTDS LoadConferenceSettings(long AConferenceKey, out string AConferenceName)
        {
            ConferenceSetupTDS MainDS = new ConferenceSetupTDS();
            string ConferenceName = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PPartnerLocationAccess.LoadViaPPartner(MainDS, AConferenceKey, Transaction);
                    PcConferenceAccess.LoadByPrimaryKey(MainDS, AConferenceKey, Transaction);
                    PcConferenceOptionAccess.LoadViaPcConference(MainDS, AConferenceKey, Transaction);
                    PcDiscountAccess.LoadViaPcConference(MainDS, AConferenceKey, Transaction);
                    PcConferenceVenueAccess.LoadViaPcConference(MainDS, AConferenceKey, Transaction);
                    PUnitAccess.LoadByPrimaryKey(MainDS, AConferenceKey, Transaction);
                    ConferenceName = PPartnerAccess.LoadByPrimaryKey(MainDS, AConferenceKey, Transaction).PartnerShortName;
                });

            foreach (ConferenceSetupTDSPcConferenceVenueRow VenueRow in MainDS.PcConferenceVenue.Rows)
            {
                string VenueName;
                TPartnerClass PartnerClass;
                MPartner.Partner.ServerLookups.WebConnectors.TPartnerServerLookups.GetPartnerShortName(VenueRow.VenueKey,
                    out VenueName,
                    out PartnerClass);
                VenueRow.VenueName = VenueName;
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            AConferenceName = ConferenceName;

            return MainDS;
        }

        /// <summary>
        /// this will store ConferenceSetupTDS
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("CONFERENCE")]
        public static TSubmitChangesResult SaveConferenceSetupTDS(ref ConferenceSetupTDS AInspectDS)
        {
            // check that conference option types exist and if not then create them
            CreateOptionTypes();

            ConferenceSetupTDSAccess.SubmitChanges(AInspectDS);

            return TSubmitChangesResult.scrOK;
        }

        private static void CreateOptionTypes()
        {
            TDBTransaction Transaction;

            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            bool RowsToAdd = false;

            string[, ] OptionTypes = new string[, ]
            {
                {
                    "ADD_ACCOMM_COST_FOR_TOTAL", "Add accommodation costs to get total costs"
                },
                {
                    "COST_PER_DAY", "Calculate conference cost per day"
                },
                {
                    "COST_PER_NIGHT", "Calculate conference cost per night"
                }
            };

            try
            {
                PcConferenceOptionTypeTable OptionTypeTable = PcConferenceOptionTypeAccess.LoadAll(Transaction);

                for (int i = 0; i < 3; i++)
                {
                    if (OptionTypeTable.Rows.Find(new object[] { OptionTypes[i, 0] }) == null)
                    {
                        PcConferenceOptionTypeRow NewRow = OptionTypeTable.NewRowTyped(false);
                        NewRow.OptionTypeCode = OptionTypes[i, 0];
                        NewRow.OptionTypeDescription = OptionTypes[i, 1];
                        NewRow.UnassignableFlag = false;
                        NewRow.DeletableFlag = false;

                        OptionTypeTable.Rows.Add(NewRow);

                        RowsToAdd = true;
                    }
                }

                if (RowsToAdd)
                {
                    // add any new rows to database
                    PcConferenceOptionTypeAccess.SubmitChanges(OptionTypeTable, Transaction);
                }

                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateOptionTypes: commit own transaction.");
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the creation of option types:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }
        }
    }
}