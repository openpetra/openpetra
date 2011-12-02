//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, berndr
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Petra.Server.MSysMan.Application.ServerLookups
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MSysMan.ServerLookups
    /// sub-namespace.
    /// </summary>
    public class TSysManServerLookups
    {
        /// <summary>time when this object was instantiated</summary>
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public TSysManServerLookups() : base()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
        }

#if DEBUGMODE
        /// <summary>
        /// destructor
        /// </summary>
        ~TSysManServerLookups()
        {
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif



        /// <summary>
        /// Retrieves the current database version
        /// </summary>
        /// <param name="APetraDBVersion">Current database version</param>
        /// <returns></returns>
        public static System.Boolean GetDBVersion(out System.String APetraDBVersion)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            APetraDBVersion = "Can not retrieve DB version";

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("GetDatabaseVersion called!");
            }
#endif

            SSystemDefaultsTable SystemDefaultsDT = new SSystemDefaultsTable();
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load data
            SystemDefaultsDT = SSystemDefaultsAccess.LoadByPrimaryKey("CurrentDatabaseVersion", ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("GetDatabaseVersion: committed own transaction.");
                }
#endif
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
        public static System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
        {
            SPatchLogTable TmpTable = new SPatchLogTable();

            APatchLogDT = new SPatchLogTable();
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("GetInstalledPatches called!");
            }
#endif

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
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("GetInstalledPatches: committed own transaction.");
                    }
#endif
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
#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("Patch: " + TmpTable.DefaultView[Counter][0]);
                }
#endif

                APatchLogDT.ImportRow(TmpTable.DefaultView[Counter].Row);
            }

            return true;
        }
    }
}