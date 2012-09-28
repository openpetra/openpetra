//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, berndr, timop
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Application.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MSysMan.ServerLookups
    /// sub-namespace.
    /// </summary>
    public class TSysManServerLookups
    {
        /// <summary>
        /// Retrieves the current database version
        /// </summary>
        /// <param name="APetraDBVersion">Current database version</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static System.Boolean GetDBVersion(out System.String APetraDBVersion)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            APetraDBVersion = "Can not retrieve DB version";
            TLogging.LogAtLevel(9, "TSysManServerLookups.GetDatabaseVersion called!");

            SSystemDefaultsTable SystemDefaultsDT = new SSystemDefaultsTable();
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load data
            SystemDefaultsDT = SSystemDefaultsAccess.LoadByPrimaryKey("CurrentDatabaseVersion", ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TSysManServerLookups.GetDatabaseVersion: committed own transaction.");
            }

            if (SystemDefaultsDT.Rows.Count < 1)
            {
                throw new ApplicationException(
                    "TSysManServerLookups.GetDBVersion: s_system_defaults DB Table is empty; this is unexpected and can lead to sever malfunction of OpenPetra. Contact your Support Team.");
            }

            SSystemDefaultsRow sysrow = SystemDefaultsDT.Rows[0] as SSystemDefaultsRow;

            if (sysrow == null)
            {
                throw new ApplicationException(
                    "TSysManServerLookups.GetDBVersion: s_system_defaults DB Table is empty; this is unexpected and can lead to sever malfunction of OpenPetra. Contact your Support Team.");
            }

            APetraDBVersion = sysrow.DefaultValue;

            return true;
        }

        /// <summary>
        /// Retrieves a list of all installed Patches
        /// </summary>
        /// <param name="APatchLogDT">The installed patches</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
        {
            SPatchLogTable TmpTable = new SPatchLogTable();

            APatchLogDT = new SPatchLogTable();
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            TLogging.LogAtLevel(9, "TSysManServerLookups.GetInstalledPatches called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                // Load data
                TmpTable = SPatchLogAccess.LoadAll(ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TSysManServerLookups.GetInstalledPatches: committed own transaction.");
                }
            }

            /* Sort the data...
             */
            TmpTable.DefaultView.Sort = SPatchLogTable.GetDateRunDBName() + " DESC, " +
                                        SPatchLogTable.GetPatchNameDBName() + " DESC";

            /* ...and put it in the output table.
             */
            for (int Counter = 0; Counter < TmpTable.DefaultView.Count; ++Counter)
            {
                TLogging.LogAtLevel(7, "Patch: " + TmpTable.DefaultView[Counter][0]);
                APatchLogDT.ImportRow(TmpTable.DefaultView[Counter].Row);
            }

            return true;
        }
    }
}