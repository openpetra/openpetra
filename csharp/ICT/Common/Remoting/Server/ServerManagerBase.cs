﻿//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2024 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web;

using Ict.Common;
using Ict.Common.DB.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Session;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// some common implementations for IServerAdminInterface
    /// </summary>
    public class TServerManagerBase : IServerAdminInterface
    {
        /// <summary>
        /// static: only initialised once for the whole server
        /// </summary>
        [ThreadStatic]
        public static IServerAdminInterface TheServerManager = null;

        /// <summary>DB Reconnection attempts (-1 = no connection established yet at all; 0 = none are being made).</summary>
        [ThreadStatic]
        protected static Int64 FDBReconnectionAttemptsCounter = -1;

        /// <summary>Whether the Timed Processing has been set up already.</summary>
        [ThreadStatic]
        protected static bool FServerTimedProcessingSetup = false;

        /// <summary>DB Reconnection attempts (-1 = no connection established yet at all; 0 = none are being made).</summary>
        public Int64 DBReconnectionAttemptsCounter
        {
            get
            {
                return FDBReconnectionAttemptsCounter;
            }

            set
            {
                FDBReconnectionAttemptsCounter = value;
            }
        }

        /// <summary>SiteKey of the OpenPetra DB that the Server is connected to.</summary>
        public Int64 SiteKey
        {
            get
            {
                Int64 ReturnValue = -1;

                try
                {
                    ReturnValue = DomainManager.GSiteKey;
                }
                catch (EDBConnectionNotEstablishedException)
                {
                    // Swallow this Exception here on purpose - we don't want to throw anything if the DB Connection
                    // isn't established yet...
                }

                return ReturnValue;
            }
        }

        /// <summary>Number of Clients that are currently connected to the Petra Server.</summary>
        public int ClientsConnected
        {
            get
            {
                return TClientManager.ClientsConnected;
            }
        }

        /// <summary>
        /// Whether the 'Server Timed Processing' has been set up.
        /// </summary>
        public bool ServerTimedProcessingSetup
        {
            get
            {
                return FServerTimedProcessingSetup;
            }
        }

        #region Properties

        /// <summary>Total number of Clients that connected to the Petra Server since the start of the Petra Server</summary>
        public int ClientsConnectedTotal
        {
            get
            {
                return TClientManager.ClientsConnectedTotal;
            }
        }

        /// <summary>Array that contains information about the Clients that are currently connected to the Petra Server.</summary>
        public ArrayList ClientList
        {
            get
            {
                return TClientManager.ClientList(false);
            }
        }

        /// <summary>Array that contains information about the Clients that are currently connected to the Petra Server.</summary>
        public ArrayList ClientListDisconnected
        {
            get
            {
                return TClientManager.ClientList(true);
            }
        }

        /// <summary>Server setting: IP Port that the Server listens for .NET Remoting calls</summary>
        public int IPPort
        {
            get
            {
                return TSrvSetting.IPBasePort;
            }
        }

        /// <summary>Server setting: Name of the Server configuration file</summary>
        public String ConfigurationFileName
        {
            get
            {
                return TSrvSetting.ConfigurationFile;
            }
        }

        /// <summary>Version of the Petra Server and operating system on which the Server runs</summary>
        public String ServerInfoVersion
        {
            get
            {
                string DLLPath = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar +
                       "Ict.Common.Remoting.Server.dll";

                return String.Format(
                    Catalog.GetString("PETRAServer {0} Build {1}  (OS: {2})"),
                    TSrvSetting.ApplicationVersion.ToString(),
                    System.IO.File.GetLastWriteTime(DLLPath).ToString(),
                    CommonTypes.ExecutingOSEnumToString(TSrvSetting.ExecutingOS));
            }
        }

        /// <summary>State of the Server</summary>
        public String ServerInfoState
        {
            get
            {
                return String.Format(Catalog.GetString("PETRAServer is running and listening @ {0}:{1}"),
                    TSrvSetting.HostIPAddresses, TSrvSetting.IPBasePort);
            }
        }

        /// <summary>Result of a call to GC.GetTotalMemory</summary>
        public System.Int64 ServerInfoMemory
        {
            get
            {
                return GC.GetTotalMemory(false);
            }
        }

        #endregion

        /// reset the static variables for each Web Request call.
        public static void ResetStaticVariables()
        {
            TheServerManager = null;
            FDBReconnectionAttemptsCounter = -1;
            FServerTimedProcessingSetup = false;
        }

        /// <summary>
        /// Initialises Logging and parses Server settings from different sources.
        ///
        /// </summary>
        public TServerManagerBase() : base()
        {
            new TAppSettingsManager(false);
            new TLogging(TAppSettingsManager.GetValue("Server.LogFile", false));
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);
        }

        /// <summary>
        /// check if a file with this security token exists
        /// </summary>
        public static bool CheckServerAdminToken(string AServerAdminToken)
        {
            string TokenFilename = TAppSettingsManager.GetValue("Server.PathTemp") +
                                   Path.DirectorySeparatorChar + "ServerAdminToken" + AServerAdminToken + ".txt";

            if (File.Exists(TokenFilename))
            {
                using (StreamReader sr = new StreamReader(TokenFilename))
                {
                    string content = sr.ReadToEnd();
                    sr.Close();

                    if (content.Trim() == AServerAdminToken)
                    {
                        TSession.SetVariable("ServerAdminToken", AServerAdminToken);
                        return true;
                    }
                }
            }
            else
            {
                TLogging.Log("cannot find security token file " + TokenFilename);
            }

            return false;
        }

        /// <summary>
        /// Ensures Logging and an 'ordered cooperative shutdown' in case an Unhandled Exception is
        /// thrown in Threads, ThreadPool work items or Finalizers anywhere in the PetraServer.
        /// </summary>
        /// <remarks>
        /// <para>Ensures proper handling of the mentioned situations which were non-fatal to
        /// the PetraServer Process in .NET 1.1, but are fatal with .NET 2.0 (also true when running
        /// on mono)!
        /// </para>
        /// <para>
        /// See http://msdn.microsoft.com/en-us/netframework/aa497241.aspx
        /// (Text block with Short Desciption 'Unhandled Exceptions will always be fatal to a process').
        /// </para>
        /// </remarks>
        /// <returns>void</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public void HookupProperShutdownProcessing()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(TExceptionHandling.UnhandledExceptionHandler);
        }

        /// <summary>
        /// Stops the Petra Server in a more controlled way than the <see cref="StopServer" /> Method.
        /// </summary>
        /// <remarks>
        /// A ClientTask is queued for all connected Clients that asks them to save the UserDefaults,
        /// to disconnect from the Server and to close. This Method monitors the connected Clients until
        /// either all Clients have responded by disconnecting or if at least one Client didn't respond and
        /// a timeout was exceeded. If this is the case and the Argument <paramref name="AForceAutomaticClosing" />
        /// is true, a different ClientTask is queued
        /// for all Clients that are still connected that asks them to close (no saving of UserDefaults and
        /// no disconnection from the server!). This is a fallback in case (1) the client(s) crashed and can't
        /// either save the UserDefaults or can't disconnect, (2) the saving of UserDefaults or
        /// the disconnecting of Clients doesn't work for some server-side reason (e.g. broken DB
        /// connection would prevent saving of UserDefaults).
        /// </remarks>
        /// <param name="AForceAutomaticClosing">Set to true to force closing of non-responding Clients (this
        /// is set to true by the ServerAdminConsole as this process is non-interactive in this case).</param>
        /// <returns>False if AForceAutomaticClosing is false and there are still clients logged in.</returns>
        public bool StopServerControlled(bool AForceAutomaticClosing)
        {
            // currently not implemented
            return false;
        }

        private void StopServerThread()
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            TLogging.Log(Catalog.GetString("SHUTDOWN PROCEDURE FINISHED"));
            Environment.Exit(0);
        }

        /// <summary>
        /// Stops the Petra Server.
        /// A GC is invoked and waits for pending tasks before the application ends.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void StopServer()
        {
            TLogging.Log(Catalog.GetString("SHUTDOWN PROCEDURE INITIATED"));
            TLogging.Log("  " + Catalog.GetString("SHUTDOWN: Executing step 1 of 2..."));

            GC.Collect();

            TLogging.Log("  " + Catalog.GetString("SHUTDOWN: Executing step 2 of 2..."));
            GC.WaitForPendingFinalizers();


            new Thread(StopServerThread).Start();

            // Server application stops here !!!
        }

        /// <summary>
        /// Requests disconnection of a certain Client from the Petra Server.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="ACantDisconnectReason">Reason why the Client cannot be disconnected</param>
        /// <returns>true if disconnection succeeded, otherwise false.
        /// </returns>
        public bool DisconnectClient(System.Int16 AClientID, out String ACantDisconnectReason)
        {
            return TClientManager.ServerDisconnectClient(AClientID,
                "Client disconnection requested by Adminstrator!", out ACantDisconnectReason);
        }

        /// <summary>
        /// Queues a ClientTask for a certain Client.
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="ATaskGroup">Group of the Task</param>
        /// <param name="ATaskCode">Code of the Task</param>
        /// <param name="ATaskParameter1">Parameter #1 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter2">Parameter #2 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter3">Parameter #3 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter4">Parameter #4 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskPriority">Priority of the Task</param>
        /// <returns>true if ClientTask was queued, otherwise false.</returns>
        public bool QueueClientTask(System.Int16 AClientID, String ATaskGroup, String ATaskCode, object ATaskParameter1,
            object ATaskParameter2, object ATaskParameter3, object ATaskParameter4, System.Int16 ATaskPriority)
        {
            return TClientManager.QueueClientTask(AClientID, ATaskGroup, ATaskCode, ATaskParameter1, ATaskParameter2,
                                                  ATaskParameter3, ATaskParameter4, ATaskPriority) >= 0;
        }

        /// <summary>
        /// Formats the client list array for output in a fixed-width font (eg. to the
        /// Console)
        /// </summary>
        /// <returns>Formatted client list.
        /// </returns>
        public String FormatClientList(Boolean AListDisconnectedClients)
        {
            return TClientManager.FormatClientList(AListDisconnectedClients);
        }

        /// <summary>
        /// Formats the client list array for output in the sysadm dialog for selection of a client id
        /// </summary>
        /// <returns>Formatted client list for sysadm dialog.
        /// </returns>
        public String FormatClientListSysadm(Boolean AListDisconnectedClients)
        {
            return TClientManager.FormatClientListSysadm(AListDisconnectedClients);
        }

        /// <summary>
        /// Performs a GarbageCollection on the Server.
        ///
        /// </summary>
        /// <returns>Result of a call to GC.GetTotalMemory after the GC was performed.
        /// </returns>
        public System.Int64 PerformGC()
        {
            GC.Collect();
            return GC.GetTotalMemory(false);
        }

        /// <summary>
        /// upgrade the database
        /// </summary>
        /// <returns>true if the database was upgraded</returns>
        public virtual bool UpgradeDatabase()
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// BackupDatabaseToYmlGZ
        /// </summary>
        public virtual string BackupDatabaseToYmlGZ()
        {
            // implemented in derived class
            return string.Empty;
        }

        /// <summary>
        /// RestoreDatabaseFromYmlGZ
        /// </summary>
        public virtual bool RestoreDatabaseFromYmlGZ(string AYmlGzData)
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// Marks all DataTables in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
        /// </summary>
        public virtual void RefreshAllCachedTables()
        {
            // implemented in derived class
        }

        /// <summary>
        /// Clears (flushes) all RDMBS Connection Pools and returns the new number of DB Connections after clearing all
        /// RDMBS Connection Pools.
        /// </summary>
        public virtual int ClearConnectionPoolAndGetNumberOfDBConnections()
        {
            // implemented in derived class
            return -1;
        }

        /// <summary>
        /// SetPassword
        /// </summary>
        public virtual bool SetPassword(string AUserID, string APassword)
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// LockSysadmin
        /// </summary>
        public virtual bool LockSysadmin()
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// AddUser
        /// </summary>
        public virtual bool AddUser(string AUserId, string APassword = "")
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// Lists the GPG keys for the Intranet server that are available to the Petra Server.
        /// </summary>
        /// <param name="List">Returns the output of the external gpg command.</param>
        /// <returns>Return code of gpg command.</returns>
        public virtual int ListGpgKeys(out string List)
        {
            List = "";
            // implemented in derived class
            return -1;
        }

        /// <summary>
        /// Imports the GPG encryption keys for the Intranet server.
        /// </summary>
        /// <param name="List">Return the output of the gpg command.</param>
        /// <returns>Return code of external gpg command.</returns>
        public virtual int ImportGpgKeys(out string List)
        {
            List = "";
            // implemented in derived class
            return -1;
        }

        /// <summary>
        /// Allows the server or admin console to run a timed job
        /// </summary>
        public virtual void PerformTimedProcessingNow(string AProcessName)
        {
            // implemented in derived class
        }

        /// Is the process job enabled?
        public virtual bool TimedProcessingJobEnabled(string AProcessName)
        {
            // implemented in derived class
            return false;
        }

        /// <summary>
        /// the daily start time for the timed processing
        /// </summary>
        public virtual string TimedProcessingDailyStartTime24Hrs
        {
            get
            {
                // implemented in derived class
                return string.Empty;
            }
        }
    }
}
