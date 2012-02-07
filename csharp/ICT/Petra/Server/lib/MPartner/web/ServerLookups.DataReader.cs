//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections.Specialized;
using System.Data;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MCommon.Data;


namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TPartnerDataReader
    {
        /// <summary>
        /// return TDS which contains conferences that match a given search string
        /// </summary>
        /// <param name="AConferenceName">match string for conference name search</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetConferenceUnits(string AConferenceName)
        {
            return GetConferenceOrOutreachUnits(true, AConferenceName);
        }

        /// <summary>
        /// return TDS which contains outreaches that match a given search string
        /// </summary>
        /// <param name="AOutreachName">match string for conference name search</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetOutreachUnits(string AOutreachName)
        {
            return GetConferenceOrOutreachUnits(false, AOutreachName);
        }

        /// <summary>
        /// return unit table records for conference or outreach
        /// </summary>
        /// <param name="AConference"></param>
        /// <param name="AEventName"></param>
        /// <returns></returns>
        private static PUnitTable GetConferenceOrOutreachUnits(bool AConference, string AEventName)
        {
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow UnitRow;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            if (AEventName == "*")
            {
                AEventName = "";
            }
            else if (AEventName.EndsWith("*"))
            {
                AEventName = AEventName.Substring(0, AEventName.Length - 1);
            }

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("GetConferenceOrOutreachUnits called!");
            }
#endif

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                // Load data
                string SqlStmt = "SELECT pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                 ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetUnitNameDBName() +
                                 ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetOutreachCodeDBName() +
                                 " FROM " + PUnitTable.GetTableDBName();

                if (AConference)
                {
                    // for conferences the unit type needs to contain 'CON' (for CONF or CONG)
                    SqlStmt = SqlStmt + " WHERE " + PUnitTable.GetUnitTypeCodeDBName() +
                              " LIKE '%CON%'";
                }
                else
                {
                    // for outreaches the outreach code is set
                    SqlStmt = SqlStmt + " WHERE " + PUnitTable.GetOutreachCodeDBName() +
                              " IS NOT NULL AND " + PUnitTable.GetOutreachCodeDBName() +
                              " <> ''";
                }

                if (AEventName.Length > 0)
                {
                    // in case there is a filter set for the event name
                    AEventName = AEventName.Replace('*', '%') + "%";
                    SqlStmt = SqlStmt + " AND " + PUnitTable.GetUnitNameDBName() +
                              " LIKE '" + AEventName + "'";
                }

                // sort rows according to name
                SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                DataTable events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "events", ReadTransaction);

                foreach (DataRow eventRow in events.Rows)
                {
                    UnitRow = (PUnitRow)UnitTable.NewRow();
                    UnitRow.PartnerKey = Convert.ToInt64(eventRow[0]);
                    UnitRow.UnitName = Convert.ToString(eventRow[1]);
                    UnitRow.OutreachCode = Convert.ToString(eventRow[2]);
                    UnitTable.Rows.Add(UnitRow);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        Console.WriteLine("GetConferenceOrOutreachUnits: committed own transaction.");
                    }
#endif
                }
            }

            return UnitTable;
        }
    }
}