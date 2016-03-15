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
using Ict.Common.DB.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Common.DB;
using Ict.Petra.Server.MSysMan.Data.Access;

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
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>Value of System Default, or ADefault.</returns>
        [NoRemoting]
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault, TDataBase ADataBase = null)
        {
            String ReturnValue = ADefault;

            String Tmp = GetSystemDefault(ASystemDefaultName, ADataBase);

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
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>Value of System Default, or SYSDEFAULT_NOT_FOUND.</returns>
        [NoRemoting]
        public static String GetSystemDefault(String ASystemDefaultName, TDataBase ADataBase = null)
        {
            String ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
            SSystemDefaultsTable SystemDefaultsTable = GetSystemDefaults(ADataBase);

            if (SystemDefaultsTable != null)
            {
                // Look up the System Default
                SSystemDefaultsRow FoundSystemDefaultsRow = (SSystemDefaultsRow)SystemDefaultsTable.Rows.Find(ASystemDefaultName);

                if (FoundSystemDefaultsRow != null)
                {
                    ReturnValue = FoundSystemDefaultsRow.DefaultValue;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the System Defaults as a DataTable.
        /// </summary>
        /// <param name="ASeparateDBConnection">Set to true if a separate instance of <see cref="TDataBase" /> should be
        /// created and an equally separate DB Connection should be established for getting the System Defaults through this.
        /// If this is false, the 'globally available' <see cref="DBAccess.GDBAccessObj" /> instance gets used instead
        /// (with the 'globally available' open DB Connection that exists for the users' AppDomain).</param>
        /// <returns>System Defaults Typed DataTable.</returns>
        [RequireModulePermission("NONE")]
        public static SSystemDefaultsTable GetSystemDefaults(bool ASeparateDBConnection)
        {
            TDataBase PrivateDataBaseObj = null;

            if (ASeparateDBConnection)
            {
                try
                {
                    PrivateDataBaseObj = new Ict.Common.DB.TDataBase();

                    PrivateDataBaseObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                        TSrvSetting.PostgreSQLServer,
                        TSrvSetting.PostgreSQLServerPort,
                        TSrvSetting.PostgreSQLDatabaseName,
                        TSrvSetting.DBUsername,
                        TSrvSetting.DBPassword,
                        "",
                        "System Defaults DB Connection");

                    return GetSystemDefaults(PrivateDataBaseObj);
                }
                finally
                {
                    if (PrivateDataBaseObj != null)
                    {
                        PrivateDataBaseObj.CloseDBConnection();
                    }
                }
            }
            else
            {
                return GetSystemDefaults(null);
            }
        }

        /// <summary>
        /// Returns the System Defaults as a DataTable.
        /// </summary>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>System Defaults Typed DataTable.</returns>
        private static SSystemDefaultsTable GetSystemDefaults(TDataBase ADataBase = null)
        {
            SSystemDefaultsTable ReturnValue = null;
            TDBTransaction ReadTransaction = null;
            bool DBAccessCallSuccessful = false;

            DBAccess.GetDBAccessObj(ADataBase).GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    TServerBusyHelper.CoordinatedAutoRetryCall("Loading all SystemDefaults", ref DBAccessCallSuccessful,
                        delegate
                        {
                            ReturnValue = SSystemDefaultsAccess.LoadAll(ReadTransaction);

                            DBAccessCallSuccessful = true;
                        });
                });

            if (!DBAccessCallSuccessful)
            {
                throw new EDBAccessLackingCoordinationException("Loading of System Default failed: server was too busy!");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Add or modify a System Default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        /// <returns>true if I believe the System Default was saved successfully.</returns>
        [RequireModulePermission("NONE")]
        public static void SetSystemDefault(String AKey, String AValue)
        {
            SetSystemDefault(AKey, AValue, null);
        }

        /// <summary>
        /// Add or modify a System Default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null. If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>true if I believe the System Default was saved successfully.</returns>
        [NoRemoting]
        public static void SetSystemDefault(String AKey, String AValue, TDataBase ADataBase)
        {
            Boolean NewTransaction = false;
            Boolean ShouldCommit = false;

            try
            {
                TDBTransaction Transaction = DBAccess.GetDBAccessObj(ADataBase).GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
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
                        DBAccess.GetDBAccessObj(ADataBase).CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GetDBAccessObj(ADataBase).RollbackTransaction();
                    }
                }
            }
        }
    }
}