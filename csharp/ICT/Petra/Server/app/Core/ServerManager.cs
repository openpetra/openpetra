//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2016 by OM International
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
        private static bool FDBConnectionBroken = false;
        private static System.Timers.Timer FCheckDBConnectionOK;
        private static int FCheckDBConnectionSyncPoint = 0;

        private IUserManager FUserManager;
        private readonly object FDBConnectionBrokenLock = new Object();
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
                new TMaintenanceLogonMessage(),
                ExceptionHandling_DBConnectionBrokenCallback);

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

            FDBConnectionCheckAccessObj = DBAccess.SimpleEstablishDBConnection("Server's DB Polling Connection");

            FDBReconnectionAttemptsCounter = 0;

            TLogging.Log("  " + Catalog.GetString("Connected to Database."));
        }

        /// <summary>
        /// Callback Method that gets called from the 'FirstChanceException' Event if the Exception meant that
        /// a DB Connection got broken and hence attempts to restore it automatically should be performed.
        /// </summary>
        /// <param name="ASource">Provided automatically by .NET - not used.</param>
        /// <param name="AException">Provided automatically by .NET (holds the Exception that just occurred)
        ///  - not used.</param>
        public void ExceptionHandling_DBConnectionBrokenCallback(object ASource, Exception AException)
        {
            bool FlagWasntSet = false;
            Thread PerformDBReconnectionThread;

            lock (FDBConnectionBrokenLock)
            {
                if (!FDBConnectionBroken)
                {
                    FDBConnectionBroken = true;

                    FlagWasntSet = true;
                }
            }

            // Guard against reentry (this callback can be called numerous times [possibly in very quick succession]!):
            // --> Only take measures if this is the first call in a row!
            if (FlagWasntSet)
            {
                // Ensure we are starting a Thread for DB re-connection attempts only if the Server's DB Polling
                // DB Connection is actually broken. (Reason for this check: This Event Handler can get called because a
                // Client's AppDomain has come across a broken DB Connection *which since has been automatically restored*...
                // if we wouldn't do this check here then we would in this case start re-connection attempts on a perfectly
                // fine Server's DB Polling connection, which could have adverse side effects if DB commands are getting
                // executed on it while this would happen!)
                if (!IsDBConnectionOK())
                {
                    StopCheckDBConnectionTimer();

                    QueueClientTaskForAllClientsRegardingBrokenDBConnection(true);

                    TDBReconnectionThread ParameterisedThreadWithCallback = new TDBReconnectionThread(this,
                        FDBConnectionEstablishmentAtStartup,
                        new TDBReconnectionThreadCallback(DBReconnectionCallback));
                    PerformDBReconnectionThread = new Thread(
                        new ThreadStart(ParameterisedThreadWithCallback.PerformDBReconnection));

                    PerformDBReconnectionThread.Start();
                }
            }
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
        /// Callback that gets called from Method <see cref="TDBReconnectionThread.PerformDBReconnection"/> upon
        /// its completion (=when it has successfully restored the Server's DB Polling connection).
        /// </summary>
        /// <param name="ADBConnectionBroken">New value for FDBConnectionBroken.</param>
        /// <param name="ADBReconnectionAttemptsCounter">New value for FDBReconnectionAttemptsCounter.</param>
        private void DBReconnectionCallback(bool ADBConnectionBroken, int ADBReconnectionAttemptsCounter)
        {
            // Update state
            lock (FDBConnectionBrokenLock)
            {
                FDBConnectionBroken = ADBConnectionBroken;
                FDBReconnectionAttemptsCounter = ADBReconnectionAttemptsCounter;
            }

            // Re-start the DB Connection check Timer (only if a check interval is specified).
            StartCheckDBConnectionTimer();

            // If the timed Server processing hasn't been setup yet (normally done at Server startup) then do it
            // now (this only needs to happen when the DB Connection wasn't available at the time of the Server startup).
            if (!FServerTimedProcessingSetup)
            {
                SetupServerTimedProcessing();
            }

            QueueClientTaskForAllClientsRegardingBrokenDBConnection(false);
        }

        /// <summary>
        /// Starts the DB Connection check Timer (only if a check interval is specified!).
        /// </summary>
        /// <remarks>This Method is not reentrant-safe and hence must not be called from multiple Threads
        /// simultaneously!</remarks>
        public void StartCheckDBConnectionTimer()
        {
            if (DBConnectionCheckInterval != 0)
            {
                FCheckDBConnectionSyncPoint = 0;

                FCheckDBConnectionOK = new System.Timers.Timer(DBConnectionCheckInterval);
                FCheckDBConnectionOK.Elapsed += new ElapsedEventHandler(CheckDBConnectionOK);

                FCheckDBConnectionOK.Start();
            }
        }

        /// <summary>
        /// Stops the DB Connection check Timer (only if a check interval is specified).
        /// </summary>
        /// <remarks>This Method is not reentrant-safe and hence must not be called from multiple Threads
        /// simultaneously!</remarks>
        public override void StopCheckDBConnectionTimer()
        {
            if (DBConnectionCheckInterval != 0)
            {
                if (FCheckDBConnectionOK != null)
                {
                    //TLogging.Log("Stopping DB Connection check timer...");

                    FCheckDBConnectionOK.Stop();

                    FCheckDBConnectionSyncPoint = -1;

                    //TLogging.Log("    DB Connection check timer stopped.");
                }
            }
        }

        /// <summary>
        /// Checks whether the DB Polling Connection is OK. Gets called by the Elapsed Event of Timer
        /// FCheckDBConnectionOK!
        /// </summary>
        /// <param name="ASender">Not evaluated.</param>
        /// <param name="AEventArgs">Not evaluated.</param>
        private static void CheckDBConnectionOK(object ASender, ElapsedEventArgs AEventArgs)
        {
            // Implementation explanation: The Interlocked.CompareExchange(Int32, Int32, Int32)
            // method overload is used to avoid reentrancy and to prevent the control thread
            // from continuing until an executing event ends. The event handler uses the
            // CompareExchange(Int32, Int32, Int32) method to set a control variable,
            // FCheckDBConnectionSyncPoint, to 1, but only if the value is currently zero.
            // This is an *atomic operation*. If the return value is zero, the control
            // variable has been set to 1 and the event handler proceeds. If the return
            // value is non-zero, the event is simply discarded to avoid reentrancy.
            // When the event handler ends, it sets the control variable back to zero.
            int sync = Interlocked.CompareExchange(ref FCheckDBConnectionSyncPoint, 1, 0);

            if (sync == 0)
            {
                // No other Elapsed Event was executing so we are fine to go ahead in this Elapsed Event.

                // Check whether the DB Connection is OK. We don't need to check the return value of the
                // IsDBConnectionOK Method because if there is a problem the automatic recovery attempts
                // that get performed when a broken DB connection gets detected jump into gear (see notes in
                // that Method's implementation)!
                IsDBConnectionOK();

                // Release control of FCheckDBConnectionSyncPoint
                FCheckDBConnectionSyncPoint = 0;
            }
        }

        /// <summary>
        /// Checks whether the DB Connection accepts SQL commands by executing a dummy query.
        /// </summary>
        /// <remarks><em>Important:</em>If this Method should be renamed then you must search
        /// for the string in the source code and rename it there as well because hard-coded checks
        /// for this Method's name are in place!</remarks>
        /// <returns>True if the DB Connection accepts SQL commands by executing a dummy query, otherwise false.</returns>
        private static bool IsDBConnectionOK()
        {
            bool ReturnValue = false;
            TDBTransaction ReadTransaction = null;

            try
            {
                if (FDBConnectionCheckAccessObj != null)
                {
                    FDBConnectionCheckAccessObj.BeginAutoReadTransaction(ref ReadTransaction, delegate
                        {
                            //TLogging.Log("IsDBConnectionOK:  Checking Server's DB Polling Connection...");

                            // Simply issue a dummy query to find out whether the DB Connection is OK!
                            FDBConnectionCheckAccessObj.ExecuteScalar("SELECT 1", ReadTransaction);
                            {
                                //TLogging.Log("IsDBConnectionOK:  Server's DB Polling Connection is OK...");

                                ReturnValue = true;
                            }
                        });
                }
            }
            catch (Exception)
            {
                // *Deliberate swallowing* of Exception as we can expect Exceptions to happen when the DB Connection
                // is somehow not OK!
                // --> When any Exception gets thrown it ends up being caught by the FistChanceException Event Handler.
                // This will 1) cause the Timer Method 'CheckDBConnectionOK' to no longer get called as the
                // 'StopCheckDBConnectionTimer' Method will get called and then 2) a loop that runs an unlimited number
                // of retries to re-establish the DB Connection gets started! (Only once the DB Connection got
                // re-established the FCheckDBConnectionOK Timer will be started again!)
            }

            if (!ReturnValue)
            {
                //TLogging.Log("IsDBConnectionOK:  Found that the DB Connection is BROKEN!!!");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Closes the Server's DB Polling Connection in an orderly fashion.
        /// </summary>
        public override void CloseDBPollingConnection()
        {
            try
            {
                if (FDBConnectionCheckAccessObj != null)
                {
                    TLogging.Log("Closing the Server's DB Polling Connection...");

                    FDBConnectionCheckAccessObj.CloseDBConnection(true);

                    TLogging.Log("    The Server's DB Polling Connection got closed.");
                }
            }
            catch (Exception Exc)
            {
                // *Deliberate swallowing* of Exception as we don't care if we should encounter DB Connection problems when
                // stopping the Server as the Server process will end anyway.
                TLogging.Log("CloseDBPollingConnection: Encountered an Exception:" + Environment.NewLine + Exc.ToString());
            }
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
        /// Clears (flushes) all RDMBS Connection Pools and returns the new number of DB Connections after clearing all
        /// RDMBS Connection Pools.
        /// </summary>
        /// <returns>New number of DB Connections after clearing all RDMBS Connection Pools.</returns>
        public override int ClearConnectionPoolAndGetNumberOfDBConnections()
        {
            return TDataBase.ClearConnectionPoolAndGetNumberOfDBConnections(TSrvSetting.RDMBSType);
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

        /// <summary>
        /// Class for running a background Thread that performs an unlimited number of retries to (re-)establish
        /// the Server's DB Polling Connection.
        /// </summary>
        private class TDBReconnectionThread
        {
            // Stateful information used in Method 'PerformDBReconnection'.
            private TServerManager FTheServerManager;
            private bool FDBConnectionEstablishmentAtStartup;
            private TDBReconnectionThreadCallback FCallbackAtEndOfThread;

            /// <summary>
            /// This Constructor sets stateful information that is used in Method 'PerformDBReconnection'.
            /// </summary>
            /// <param name="ATheServerManager">Reference to the one instance of <see cref="TServerManager"/>.</param>
            /// <param name="ADBConnectionEstablishmentAtStartup">Set to true if the re-establishing of the Server's
            /// DB Polling Connection happens at Server startup time, otherwise to false.</param>
            /// <param name="ACallbackAtEndOfThread">Callback Method that gets called only when the Server's DB Polling
            /// Connection has been re-established.</param>
            public TDBReconnectionThread(
                TServerManager ATheServerManager, bool ADBConnectionEstablishmentAtStartup,
                TDBReconnectionThreadCallback ACallbackAtEndOfThread)
            {
                FTheServerManager = ATheServerManager;
                FDBConnectionEstablishmentAtStartup = ADBConnectionEstablishmentAtStartup;
                FCallbackAtEndOfThread = ACallbackAtEndOfThread;
            }

            /// <summary>
            /// Performs an unlimited number of retries to (re-)establish the Server's DB Polling Connection.
            /// Finishes only when the Server's DB Polling Connection is restored.
            /// </summary>
            /// <remarks>This Method must only be run in a Thread as otherwise it would blocks the server
            /// completely!</remarks>
            public void PerformDBReconnection()
            {
                const int WAITING_TIME_BETWEEN_RETRIES = 2000; // 2000 Milliseconds

                string StrOpenDBConn = Catalog.GetString("open the Server's DB Polling Connection...");
                string StrReestablishDBConn = Catalog.GetString("re-establish the Server's broken DB Polling Connection...");
                string StrEstablished = Catalog.GetString("got successfully established");
                string StrRestored = Catalog.GetString("got successfully restored");
                bool DBConnectionRestored = false;

                FTheServerManager.DBReconnectionAttemptsCounter = 0;

                TLogging.LogAtLevel(1, String.Format(
                        "ExceptionHandling_DBConnectionBrokenCallback: Starting handling of broken database connection (on Thread {0})...!",
                        ThreadingHelper.GetThreadIdentifier(Thread.CurrentThread)));

                while (!DBConnectionRestored)
                {
                    try
                    {
                        FTheServerManager.DBReconnectionAttemptsCounter++;

                        TLogging.Log(String.Format("Attempt {0} to ", FTheServerManager.DBReconnectionAttemptsCounter) +
                            (FDBConnectionEstablishmentAtStartup ? StrOpenDBConn : StrReestablishDBConn));

                        FTheServerManager.EstablishDBPollingConnection();

                        DBConnectionRestored = true;
                    }
                    catch (SocketException Exc)
                    {
                        TLogging.LogAtLevel(1, Exc.Message);

                        // Getting a SocketException here *is what can be expected* until the DB connection can be successfully
                        // established - hence we are 'swallowing' this particular Exception here on purpose!
                        Thread.Sleep(WAITING_TIME_BETWEEN_RETRIES);
                    }
                    catch (NpgsqlException Exc)
                    {
                        TLogging.LogAtLevel(1, Exc.Message);

                        // Getting a NpgsqlException here *is what can be expected* until the DB connection can be successfully
                        // established - hence we are 'swallowing' this particular Exception here on purpose!
                        Thread.Sleep(WAITING_TIME_BETWEEN_RETRIES);
                    }
                    catch (EDBConnectionNotEstablishedException Exc)
                    {
                        TLogging.LogAtLevel(1, Exc.Message);

                        // Getting an EDBConnectionNotEstablishedException here *is what can be expected* until the DB connection
                        // can be successfully established - hence we are 'swallowing' this particular Exception here on purpose!
                        Thread.Sleep(WAITING_TIME_BETWEEN_RETRIES);
                    }
                    catch (Exception Exc)
                    {
                        TLogging.Log(Exc.Message);
                        TLogging.LogStackTrace(TLoggingType.ToLogfile);

                        throw;
                    }
                }

                if (FCallbackAtEndOfThread != null)
                {
                    FCallbackAtEndOfThread(false, 0);
                }
                else
                {
                    throw new EOPException("Delegate 'FCallbackAtEndOfThread' was not set up, but it must be set up " +
                        "for the ability of the 'PerformDBReconnection' Method to signalise that it has re-established the " +
                        "Server's DB Polling Connection");
                }

                // Log that we are done once the Callback Method has finished running
                if (TLogging.DebugLevel == 0)
                {
                    TLogging.Log(String.Format("  --> The Server's DB Polling Connection {0}!",
                            FDBConnectionEstablishmentAtStartup ? StrEstablished : StrRestored));
                }
                else
                {
                    TLogging.Log(
                        String.Format("FINISHED with the handling of a broken database " +
                            "connection (on Thread {0}) - the Server's DB Polling Connection {1}!",
                            ThreadingHelper.GetThreadIdentifier(Thread.CurrentThread),
                            FDBConnectionEstablishmentAtStartup ? StrEstablished : StrRestored));
                }
            }
        }
    }
}
