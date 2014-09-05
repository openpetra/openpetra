//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       Tim Ingham
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
using Ict.Common;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Common.DB;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.Verification;

namespace Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors
{
    /// <summary>
    /// Reads and saves a DataTable for the System Defaults.
    ///
    /// </summary>
    public class TSystemDefaults
    {
        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">System Default Key</param>
        /// <param name="ADefault">Default to use if not found</param>
        /// <returns>Value of System Default, or ADefault
        /// </returns>
        [NoRemoting]
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            String ReturnValue = ADefault;

            String Tmp = GetSystemDefault(ASystemDefaultName);

            if (Tmp != SharedConstants.SYSDEFAULT_NOT_FOUND)
            {
                ReturnValue = Tmp;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">System Default Key</param>
        /// <returns>Value of System Default, or SYSDEFAULT_NOT_FOUND
        /// </returns>
        [NoRemoting]
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
            SSystemDefaultsTable SystemDefaultsTable = GetSystemDefaults();

            // Look up the System Default
            SSystemDefaultsRow FoundSystemDefaultsRow = (SSystemDefaultsRow)SystemDefaultsTable.Rows.Find(ASystemDefaultName);

            if (FoundSystemDefaultsRow != null)
            {
                ReturnValue = FoundSystemDefaultsRow.DefaultValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the System Defaults as a DataTable.
        ///
        /// </summary>
        /// <returns>System Defaults Typed DataTable.
        /// </returns>
        [RequireModulePermission("NONE")]
        public static SSystemDefaultsTable GetSystemDefaults()
        {
            SSystemDefaultsTable Ret;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                Ret = SSystemDefaultsAccess.LoadAll(ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return Ret;
        }

        /// <summary>
        /// Add or modify a System Default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        /// <returns>true if I believe the System Default was saved successfully</returns>
        [RequireModulePermission("NONE")]
        public static void SetSystemDefault(String AKey, String AValue)
        {
            Boolean NewTransaction = false;
            Boolean ShouldCommit = false;

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);
                SSystemDefaultsTable tbl = SSystemDefaultsAccess.LoadByPrimaryKey(AKey, Transaction);

                if (tbl.Rows.Count > 0) // I already have this. (I expect this is the case usually!)
                {
                    DataRow Row = tbl[0];
                    ((SSystemDefaultsRow)Row).DefaultValue = AValue;
                }
                else
                {
                    DataRow Row = tbl.NewRowTyped(true);
                    ((SSystemDefaultsRow)Row).DefaultCode = AKey;
                    ((SSystemDefaultsRow)Row).DefaultDescription = "Created in OpenPetra";
                    ((SSystemDefaultsRow)Row).DefaultValue = AValue;
                    tbl.Rows.Add(Row);
                }

                SSystemDefaultsAccess.SubmitChanges(tbl, Transaction);
                ShouldCommit = true;
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of a single System Default:" + Environment.NewLine + Exc.ToString());
                ShouldCommit = false;
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    if (ShouldCommit)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
            }
        }
    }
}