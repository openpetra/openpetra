//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.Reflection;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.DBUpgrades
{
    /// <summary>
    /// Upgrade the database
    /// </summary>
    public class TDBUpgrades : IDBUpgrades
    {
        /// <summary>
        /// get current database version
        /// </summary>
        private static TFileVersionInfo GetCurrentDBVersion()
        {
            string currentVersion = (string)DBAccess.GDBAccessObj.ExecuteScalar(
                "SELECT s_default_value_c FROM s_system_defaults where s_default_code_c='CurrentDatabaseVersion'",
                IsolationLevel.ReadUncommitted);

            return new TFileVersionInfo(currentVersion);
        }

        /// <summary>
        /// set current database version
        /// </summary>
        private static bool SetCurrentDBVersion(TFileVersionInfo ANewVersion)
        {
            using (TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction())
            {
                string newVersionSql =
                    String.Format("UPDATE s_system_defaults SET s_default_value_c = '{0}' WHERE s_default_code_c = 'CurrentDatabaseVersion';",
                        ANewVersion.ToStringDotsHyphen());
                DBAccess.GDBAccessObj.ExecuteNonQuery(newVersionSql, transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return true;
        }

        /// Upgrade the database to the latest version
        public bool UpgradeDatabase()
        {
            bool upgraded = false;

            while (true)
            {
                TFileVersionInfo originalDBVersion = GetCurrentDBVersion();
                TFileVersionInfo currentDBVersion = originalDBVersion;
                TLogging.LogAtLevel(1, "current DB version: " + currentDBVersion.ToStringDotsHyphen());

                System.Type t = typeof(TDBUpgrade);

                foreach (MethodInfo m in t.GetMethods())
                {
                    if (m.Name.StartsWith(
                            String.Format("UpgradeDatabase{0}{1}_",
                                originalDBVersion.FileMajorPart.ToString("0000"),
                                originalDBVersion.FileMinorPart.ToString("00"))))
                    {
                        TFileVersionInfo testDBVersion = new TFileVersionInfo(
                            m.Name.Substring("UpgradeDatabase000000_".Length, 4) + "." +
                            m.Name.Substring("UpgradeDatabase000000_".Length + 4, 2) + ".0-0");

                        // check if the exefileversion is below testDBVersion
                        if (TSrvSetting.ApplicationVersion.Compare(testDBVersion) < 0)
                        {
                            TLogging.Log("Database Upgrade: ignoring method " + m.Name +
                                " because the application version is behind: " +
                                TSrvSetting.ApplicationVersion.ToString());
                            continue;
                        }

                        TLogging.Log("Database Upgrade: applying method " + m.Name);

                        bool result = (bool)m.Invoke(null, BindingFlags.Static, null, null, null);

                        if (result == true)
                        {
                            upgraded = true;
                            currentDBVersion = testDBVersion;
                            SetCurrentDBVersion(currentDBVersion);
                        }

                        break;
                    }
                }

                // if the database version does not change anymore, then we are finished
                if (currentDBVersion.Compare(originalDBVersion) == 0)
                {
                    break;
                }
            }

            return upgraded;
        }
    }
}