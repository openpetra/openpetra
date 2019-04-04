//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using GNU.Gettext;
using Npgsql;
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
        private static TDataBase FDBConnectionCheckAccessObj;

        private IUserManager FUserManager;
        private bool FDBConnectionEstablishmentAtStartup;

        /// <summary>
        /// Whether the DB Connection Establishment happens at server startup, or not.
        /// </summary>
        public bool DBConnectionEstablishmentAtStartup
        {
            get
            {
                return FDBConnectionEstablishmentAtStartup;
            }

            set
            {
                FDBConnectionEstablishmentAtStartup = value;
            }
        }

        private delegate void TDBReconnectionThreadCallback(bool ADBConnectionBroken, int ADBReconnectionAttemptsCounter);

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
            // Create SystemDefaults Cache
            TSystemDefaultsCache.GSystemDefaultsCache = new TSystemDefaultsCache();
            DomainManager.GetSiteKeyFromSystemDefaultsCacheDelegate = @TSystemDefaultsCache.GSystemDefaultsCache.GetSiteKeyDefault;

            TCacheableTablesManager.InitializeUnit();
            TCacheableTablesManager.GCacheableTablesManager = new TCacheableTablesManager(new TDelegateSendClientTask(TClientManager.QueueClientTask));

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

            TClientManager.InitializeStaticVariables(TSystemDefaultsCache.GSystemDefaultsCache,
                FUserManager,
                new TErrorLog(),
                new TLoginLog(),
                new TMaintenanceLogonMessage());

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

            if (TAppSettingsManager.GetBoolean("Server.Processing.AutomatedIntranetExport.Enabled", false))
            {
                Assembly CommonProcessingAssembly = Assembly.Load("Ict.Petra.Server.lib.MCommon.processing");
                Type IntranetExportClass = CommonProcessingAssembly.GetType("Ict.Petra.Server.MCommon.Processing.TProcessAutomatedIntranetExport");
                TTimedProcessing.AddProcessingJob(
                    "TProcessAutomatedIntranetExport",
                    (TTimedProcessing.TProcessDelegate)Delegate.CreateDelegate(
                        typeof(TTimedProcessing.TProcessDelegate),
                        IntranetExportClass,
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
        }

        private List <TDataBase>FDBConnections = new List <TDataBase>();

        /// <summary>
        /// manage database connections for the ASP webclient
        /// </summary>
        /// <param name="ADatabaseConnection"></param>
        public void AddDBConnection(TDataBase ADatabaseConnection)
        {
            if (!FDBConnections.Contains(ADatabaseConnection))
            {
                FDBConnections.Add(ADatabaseConnection);
            }
        }

        /// <summary>
        /// disconnect database connections that are older than the given timeout in seconds.
        /// This is useful for the ASP webclient
        /// </summary>
        /// <param name="ATimeoutInSeconds"></param>
        /// <param name="AUserID">can limit to one specific username, eg. ANONYMOUS for online registration, or leave empty for all users</param>
        /// <returns></returns>
        public bool DisconnectTimedoutDatabaseConnections(Int32 ATimeoutInSeconds, string AUserID)
        {
            List <TDataBase>DBsToDisconnect = new List <TDataBase>();

            foreach (TDataBase db in FDBConnections)
            {
                if ((AUserID == null) || (AUserID.Length == 0) || (AUserID == db.UserID))
                {
                    if (db.LastDBAction.AddSeconds(ATimeoutInSeconds) < DateTime.Now)
                    {
                        DBsToDisconnect.Add(db);
                    }
                }
            }

            foreach (TDataBase dbToDisconnect in DBsToDisconnect)
            {
                TLogging.Log("Disconnecting DB connection of client " +
                    dbToDisconnect.UserID + " after timeout. Last activity was at: " +
                    dbToDisconnect.LastDBAction.ToShortTimeString());

                dbToDisconnect.CloseDBConnection();
                FDBConnections.Remove(dbToDisconnect);
            }

            return DBsToDisconnect.Count > 0;
        }

        /// <summary>
        /// (Re-)Opens a Database connection for the Server's DB Polling.
        /// </summary>
        /// <returns>void</returns>
        public void EstablishDBPollingConnection()
        {
            if (FDBConnectionCheckAccessObj != null)
            {
                try
                {
                    FDBConnectionCheckAccessObj.CloseDBConnection(true);
                }
                catch (Exception)
                {
                    // *Deliberate swallowing* of Exception as we can expect Exceptions to happen when the DB Connection
                    // is somehow not OK! - We don't mind as we are attempting to re-establish it in the next code lines!
                }
            }

            FDBConnectionCheckAccessObj = DBAccess.Connect("Server's DB Polling Connection");

            FDBReconnectionAttemptsCounter = 0;

            TLogging.Log("  " + Catalog.GetString("Connected to Database."));
        }

        private void QueueClientTaskForAllClientsRegardingBrokenDBConnection(bool AIsBroken)
        {
            string TaskCode;

            if (AIsBroken)
            {
                TaskCode = "BROKEN";
            }
            else
            {
                TaskCode = "RESTORED";
            }

            QueueClientTask(-1, SharedConstants.CLIENTTASKGROUP_DBCONNECTIONBROKEN, TaskCode, null, null, null, null, 1);
        }

        /// <summary>
        /// Opens a Database connection to the main Database.
        /// </summary>
        public TDataBase EstablishDBConnection()
        {
            TDataBase AccessObj = new TDataBase();

            AccessObj.EstablishDBConnection("Server's DB Connection");

            TLogging.Log("  " + Catalog.GetString("Connected to Database."));

            return AccessObj;
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
            TPetraIdentity PetraIdentity = new TPetraIdentity(
                "SYSADMIN", "", "", "", "", DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, 0, -1, -1, false, false, false);

            UserInfo.GUserInfo = new TPetraPrincipal(PetraIdentity, null);

            return FUserManager.SetPassword(AUserID, APassword);
        }

        /// <summary>
        /// AddUser
        /// </summary>
        public override bool AddUser(string AUserID, string APassword = "")
        {
            // we need a GUserInfo object for submitting the changes to the database later on
            TPetraIdentity PetraIdentity = new TPetraIdentity(
                "SYSADMIN", "", "", "", "", DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, 0, -1, -1, false, false, false);

            UserInfo.GUserInfo = new TPetraPrincipal(PetraIdentity, null);

            return FUserManager.AddUser(AUserID, APassword);
        }

        /// <summary>
        /// Lists the GPG keys for the Intranet server that are available to the Petra Server.
        /// </summary>
        /// <param name="List">Return the output of the gpg command.</param>
        /// <returns>Return code of external gpg command.</returns>
        public override int ListGpgKeys(out string List)
        {
            return ExecuteGpgCommand("--list-keys", out List);
        }

        /// <summary>
        /// Imports the GPG encryption keys for the Intranet server.
        /// </summary>
        /// <remarks>Keys are stored in Application\data30\gnupg for Live installs, and Application\setup\petra0300\winServer for Developer installs.</remarks>
        /// <param name="List">Return the output of the gpg command.</param>
        /// <returns>Return code of external gpg command, or 2 if gpg.exe can't be found.</returns>
        public override int ImportGpgKeys(out string List)
        {
            var InstallLocation = Path.Combine(TSrvSetting.ApplicationBinFolder, "..", "data30", "gnupg");

            if (Directory.Exists(InstallLocation))
            {
                return ImportGpgKeysFrom(InstallLocation, out List);
            }

            var DeveloperLocation = Path.Combine(TSrvSetting.ApplicationBinFolder, "..", "..", "setup", "petra0300", "winServer");

            if (Directory.Exists(DeveloperLocation))
            {
                return ImportGpgKeysFrom(DeveloperLocation, out List);
            }

            List = "Unable to locate GPG key files.";
            return 2;
        }

        private int ImportGpgKeysFrom(string Directory, out string List)
        {
            return ExecuteGpgCommand(String.Format("--import {0}", Path.Combine(Directory, "*.asc")), out List);
        }

        /// <summary>
        /// Executes a gpg command, returns its output and return code.
        /// </summary>
        /// <param name="AArguments">Arguments passed to the command.</param>
        /// <param name="Output">The Standard Error stream from the external command.</param>
        /// <returns>The command's return code, 2 if it couldn't be started or it hung, or -1 if an unexpected exception occurs while setting up the process.</returns>
        public int ExecuteGpgCommand(string AArguments, out string Output)
        {
            Process proc = null;

            Output = "";
            var ReturnCode = -1;
            try
            {
                var ProcInfo = new ProcessStartInfo("gpg.exe", AArguments);
                ProcInfo.UseShellExecute = false;
                ProcInfo.RedirectStandardOutput = true;
                ProcInfo.RedirectStandardError = true;

                proc = Process.Start(ProcInfo);

                if (proc == null)
                {
                    Output = Catalog.GetString("Error starting gpg.");
                    return 2;
                }

                if (!proc.WaitForExit(5000))
                {
                    TLogging.Log(String.Format("'gpg {0}'did not exit within 5 seconds.", AArguments));
                    return 2;
                }

                Output = proc.StandardOutput.ReadToEnd() + proc.StandardError.ReadToEnd();
                ReturnCode = proc.ExitCode;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                proc.Close();
            }
            return ReturnCode;
        }

        /// <summary>
        /// Sets up timed Server timed processing tasks (in a separate Thread, to avoid the blocking
        /// of the Server's main Thread).
        /// </summary>
        /// <description>
        /// Involves the creation of Timers and the opening and closing of a Database Connection
        /// specifically for that purpose.
        /// </description>
        public void SetupServerTimedProcessing()
        {
            Thread StartProcessingThread = new Thread(TTimedProcessing.StartProcessing);

            StartProcessingThread.Name = UserInfo.GUserInfo.UserID + "__TTimedProcessing.StartProcessing_Thread";
            TLogging.LogAtLevel(7, StartProcessingThread.Name + " starting.");

            StartProcessingThread.Start();

            FServerTimedProcessingSetup = true;
        }

        /// <summary>
        /// Allows the server or admin console to run a timed processing job now (in a separate
        /// Thread, to avoid the blocking of the Server's main Thread).
        /// </summary>
        public override void PerformTimedProcessingNow(string AProcessName)
        {
            Thread StartProcessingThread = new Thread(TTimedProcessing.RunJobManually);

            StartProcessingThread.Name = UserInfo.GUserInfo.UserID + "__TTimedProcessing.RunJobManually_Thread";
            TLogging.LogAtLevel(7, StartProcessingThread.Name + " starting.");

            StartProcessingThread.Start(AProcessName);
        }

        /// Is the process job enabled?
        public override bool TimedProcessingJobEnabled(string AProcessName)
        {
            return TTimedProcessing.IsJobEnabled(AProcessName);
        }

        /// <summary>
        /// the daily start time for the timed processing
        /// </summary>
        public override string TimedProcessingDailyStartTime24Hrs
        {
            get
            {
                return TTimedProcessing.DailyStartTime24Hrs;
            }
        }
    }
}
