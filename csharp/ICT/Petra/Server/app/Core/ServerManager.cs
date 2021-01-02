//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using System.Threading;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;
using System.Diagnostics;
using System.IO;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Main class for Server startup and shutdown and Server interaction
    /// via a Server Admin application.
    ///
    /// It is designed in a way that the Server .exe file can be either a Command
    /// line application or any other form of .NET application (eg. WinForms) to
    /// provide Petra Server functionality. (The Server .exe file contains almost no
    /// logic because the logic is centralised in this class.)
    ///
    /// TServerManager gets remoted and can be accessed via an Interface from a
    /// Server Admin application such as PetraServerAdminConsole.exe
    /// </summary>
    public class TServerManager : TServerManagerBase
    {
        private IUserManager FUserManager;

        /// <summary>
        /// get a casted version of the static variable
        /// </summary>
        public static TServerManager TheCastedServerManager
        {
            get
            {
                return (TServerManager)TheServerManager;
            }
        }


        /// <summary>
        /// Initialises Logging and parses Server settings from different sources.
        /// </summary>
        public TServerManager() : base()
        {
            Assembly SysManAssembly = Assembly.Load("Ict.Petra.Server.lib.MSysMan");
            Type ImportExportType = SysManAssembly.GetType("Ict.Petra.Server.MSysMan.ImportExport.TImportExportManager");
            FImportExportManager = (IImportExportManager)Activator.CreateInstance(ImportExportType,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                null,
                null);

            Assembly DBUpgradesAssembly = Assembly.Load("Ict.Petra.Server.lib.MSysMan.DBUpgrades");
            Type DatabaseUpgradeType = DBUpgradesAssembly.GetType("Ict.Petra.Server.MSysMan.DBUpgrades.TDBUpgrades");
            FDBUpgrades = (IDBUpgrades)Activator.CreateInstance(DatabaseUpgradeType,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                null,
                null);

            Type UserManagement = SysManAssembly.GetType("Ict.Petra.Server.MSysMan.Maintenance.UserManagement.TUserManager");
            FUserManager = (IUserManager)Activator.CreateInstance(UserManagement,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                null,
                null);

            TClientManager.InitializeStaticVariables(
                FUserManager,
                new TErrorLog(),
                new TLoginLog(),
                new TMaintenanceLogonMessage());
        }

        private IImportExportManager FImportExportManager = null;
        private IDBUpgrades FDBUpgrades = null;

        /// <summary>
        /// upgrade the database
        /// </summary>
        /// <returns>true if the database was upgraded</returns>
        public override bool UpgradeDatabase()
        {
            if (FDBUpgrades != null)
            {
                return FDBUpgrades.UpgradeDatabase();
            }
            else
            {
                TLogging.Log("please initialize FDBUpgrades");
                return false;
            }
        }

        /// <summary>
        /// Returns a string with yml.gz data.
        /// </summary>
        /// <returns></returns>
        public override string BackupDatabaseToYmlGZ()
        {
            if (FImportExportManager != null)
            {
                return FImportExportManager.BackupDatabaseToYmlGZ();
            }
            else
            {
                TLogging.Log("please initialize FImportExportManager");
                return string.Empty;
            }
        }

        /// <summary>
        /// Restore the database from a string with yml.gz data.
        /// </summary>
        /// <returns></returns>
        public override bool RestoreDatabaseFromYmlGZ(string AYmlGzData)
        {
            return FImportExportManager.RestoreDatabaseFromYmlGZ(AYmlGzData);
        }

        /// <summary>
        /// Marks all DataTables in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
        /// </summary>
        public override void RefreshAllCachedTables()
        {
            TCacheableTablesManager.GCacheableTablesManager.MarkAllCachedTableNeedsRefreshing();
        }

        /// <summary>
        /// SetPassword
        /// </summary>
        public override bool SetPassword(string AUserID, string APassword)
        {
            // we need a GUserInfo object for submitting the changes to the database later on
            UserInfo.SetUserInfo(new TPetraPrincipal("SYSADMIN"));

            return FUserManager.SetPassword(AUserID, APassword);
        }

        /// <summary>
        /// LockSysadmin
        /// </summary>
        public override bool LockSysadmin()
        {
            // we need a GUserInfo object for submitting the changes to the database later on
            UserInfo.SetUserInfo(new TPetraPrincipal("SYSADMIN"));

            return FUserManager.LockSysadmin();
        }

        /// <summary>
        /// AddUser
        /// </summary>
        public override bool AddUser(string AUserID, string APassword = "")
        {
            // we need a GUserInfo object for submitting the changes to the database later on
            UserInfo.SetUserInfo(new TPetraPrincipal("SYSADMIN"));

            return FUserManager.AddUser(AUserID, APassword);
        }

        /// <summary>
        /// Allows the server or admin console to run a timed processing job now
        /// </summary>
        public override void PerformTimedProcessingNow(string AProcessName)
        {
            //
            // Set up 'Timed Processing'
            //
            TTimedProcessing.DailyStartTime24Hrs = TAppSettingsManager.GetValue("Server.Processing.DailyStartTime24Hrs", "00:30");

            if (TAppSettingsManager.GetBoolean("Server.Processing.PartnerReminders.Enabled", true))
            {
                Assembly PartnerProcessingAssembly = Assembly.Load("Ict.Petra.Server.lib.MPartner.processing");
                Type PartnerReminderClass = PartnerProcessingAssembly.GetType("Ict.Petra.Server.MPartner.Processing.TProcessPartnerReminders");
                TTimedProcessing.AddProcessingJob(
                    "TProcessPartnerReminders",
                    (TTimedProcessing.TProcessDelegate)Delegate.CreateDelegate(
                        typeof(TTimedProcessing.TProcessDelegate),
                        PartnerReminderClass,
                        "Process"));
            }

            if (TAppSettingsManager.GetBoolean("Server.Processing.DataChecks.Enabled", false))
            {
                Assembly CommonProcessingAssembly = Assembly.Load("Ict.Petra.Server.lib.MCommon.processing");
                Type ProcessDataChecksClass = CommonProcessingAssembly.GetType("Ict.Petra.Server.MCommon.Processing.TProcessDataChecks");
                TTimedProcessing.AddProcessingJob(
                    "TProcessDataChecks",
                    (TTimedProcessing.TProcessDelegate)Delegate.CreateDelegate(
                        typeof(TTimedProcessing.TProcessDelegate),
                        ProcessDataChecksClass,
                        "Process"));
            }

            TTimedProcessing.RunJobManually(AProcessName);
        }
    }
}
