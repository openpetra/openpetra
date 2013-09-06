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
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;


namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// Deletes a conference from the server
    ///
    /// </summary>
    public class TDeleteConferenceWebConnector
    {
        /// <summary>
        /// deletes the complete conference including all conference data
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeleteConference(Int64 AConferenceKey, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting conference"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting conference"),
                20);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                string[] TableNames = new string[] {
                    PcAttendeeTable.GetTableDBName(),
                         PcConferenceCostTable.GetTableDBName(),
                         PcConferenceOptionTable.GetTableDBName(),
                         PcConferenceVenueTable.GetTableDBName(),
                         PcDiscountTable.GetTableDBName(),
                         PcEarlyLateTable.GetTableDBName(),
                         PcExtraCostTable.GetTableDBName(),
                         PcGroupTable.GetTableDBName(),
                         PcSupplementTable.GetTableDBName()
                };

                OdbcParameter[] ConferenceParameter = new OdbcParameter[] {
                    new OdbcParameter("conferencekey", OdbcType.BigInt)
                };

                ConferenceParameter[0].Value = AConferenceKey;

                foreach (string Table in TableNames)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery(
                        String.Format("DELETE FROM PUB_{0} WHERE pc_conference_key_n = ?", Table),
                        Transaction, ConferenceParameter);
                }

                PcConferenceAccess.DeleteByPrimaryKey(AConferenceKey, Transaction);

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    throw new Exception("Deletion of Conference was cancelled by the user");
                }

                DBAccess.GDBAccessObj.CommitTransaction();

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());

                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult(
                        "Problems deleting conference " + AConferenceKey.ToString(),
                        e.Message,
                        "Cannot delete conference",
                        string.Empty,
                        TResultSeverity.Resv_Critical,
                        Guid.Empty));
                DBAccess.GDBAccessObj.RollbackTransaction();
                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
                return false;
            }

            return true;
        }
    }
}