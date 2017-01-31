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
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.App.Core.Security;


namespace Ict.Petra.Server.MPartner.Mailroom.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TPostcodeRegionsDataWebConnector
    {
        /// <summary>
        /// this will store PostcodeRegionsTDS
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static TSubmitChangesResult SavePostcodeRegionsTDS(ref PostcodeRegionsTDS AInspectDS)
        {
            PostcodeRegionsTDSAccess.SubmitChanges(AInspectDS);

            // Update the Cacheable DataTable in the Servers'
            // Cache and inform all other Clients that they need to reload this Cacheable DataTable
            // the next time something in the Client accesses it.
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableMailingTablesEnum.PostcodeRegionList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableMailingTablesEnum.PostcodeRegionRangeList.ToString());

            return TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// This method returns a table with postcode regions and ranges combined. Columns: Region, Range, From (Range), To (Range), Description (Region)
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DataTable GetPostCodeRegionsAndRanges()
        {
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            DataTable RegionsRangesTable = new DataTable();

            if (TLogging.DL >= 9)
            {
                Console.WriteLine("GetPostCodeRegionsAndRanges called!");
            }

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    string SqlStmt =
                        "SELECT " + PPostcodeRegionRangeTable.GetTableDBName() + "." + PPostcodeRegionRangeTable.GetRegionDBName() +
                        ", " + PPostcodeRegionTable.GetDescriptionDBName() + ", " +
                        PPostcodeRegionRangeTable.GetTableDBName() + "." + PPostcodeRegionRangeTable.GetRangeDBName() + ", " +
                        PPostcodeRangeTable.GetFromDBName() + ", " + PPostcodeRangeTable.GetToDBName() +
                        " FROM " + PPostcodeRegionRangeTable.GetTableDBName() + " INNER JOIN " + PPostcodeRangeTable.GetTableDBName() + " ON " +
                        PPostcodeRegionRangeTable.GetTableDBName() + "." + PPostcodeRegionRangeTable.GetRangeDBName() + " = " +
                        PPostcodeRangeTable.GetTableDBName() + "." + PPostcodeRangeTable.GetRangeDBName() + " INNER JOIN " +
                        PPostcodeRegionTable.GetTableDBName() +
                        " ON " + PPostcodeRegionTable.GetTableDBName() + "." + PPostcodeRegionTable.GetRegionDBName() + " = " +
                        PPostcodeRegionRangeTable.GetTableDBName() + "." + PPostcodeRegionRangeTable.GetRegionDBName() +
                        " ORDER BY " + PPostcodeRegionTable.GetRegionDBName();

                    RegionsRangesTable = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "events",
                        Transaction, SqlParameterList.ToArray());
                });

            return RegionsRangesTable;
        }
    }
}