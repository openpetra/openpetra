//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using GNU.Gettext;
using System.Web;

using Ict.Common;
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
        public static IServerAdminInterface TheServerManager = null;

        /// <summary>Keeps track of the number of times this Class has been
        /// instantiated</summary>
        private Int32 FNumberServerManagerInstances;

        /// <summary>this is used to read resource strings from the resx file</summary>
        /// <summary>this is to know if this is the real instance; for displaying the message SERVER STOPPED at the right time</summary>
        private Boolean FFirstInstance;

        /// <summary>Number of Clients that are currently connected to the Petra Server.</summary>
        public int ClientsConnected
        {
            get
            {
                return TClientManager.ClientsConnected;
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

        /// <summary>The Site Key</summary>
        public Int64 SiteKey
        {
            get
            {
                return DomainManager.GSiteKey;
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

        /// server is running as standalone, started with the client
        public bool RunAsStandalone
        {
            get
            {
                return TSrvSetting.RunAsStandalone;
            }
        }

        /// smtp server for sending email
        public string SMTPServer
        {
            get
            {
                return TSrvSetting.SMTPServer;
            }
        }

        #endregion

        /// <summary>
        /// Initialises Logging and parses Server settings from different sources.
        ///
        /// </summary>
        public TServerManagerBase() : base()
        {
            FNumberServerManagerInstances = 0;

            new TAppSettingsManager(false);
            new TSrvSetting();
            new TLogging(TSrvSetting.ServerLogFile);
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

            FFirstInstance = (FNumberServerManagerInstances == 0);
            FNumberServerManagerInstances++;
        }

        /// <summary>
        /// Default destructor.
        /// </summary>
        ~TServerManagerBase()
        {
            // THE VERY END OF THE SERVER :(
            // only display this for the main instance!
            if (FFirstInstance == true)
            {
                Console.WriteLine();
                TLogging.Log(Catalog.GetString("SERVER STOPPED!"));
            }
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
                new UnhandledExceptionEventHandler(ExceptionHandling.UnhandledExceptionHandler);
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
            const int SLEEP_TIME_PER_RETRY = 500; // 500 milliseconds = 0.5 seconds
            const int MAX_RETRIES = 100; // = 500 milliseconds * 100 = 50 seconds
            int Retries = 0;

            Console.WriteLine();
            TLogging.Log(Catalog.GetString("CONTROLLED SHUTDOWN PROCEDURE INITIATED"));

            // Check if there are still Clients connected
            if (ClientsConnected > 0)
            {
                // At least one Client is still connected
                TLogging.Log("  " +
                    String.Format(Catalog.GetString("CONTROLLED SHUTDOWN: Notifying all connected Clients ({0} {1}). Please wait..."),
                        ClientsConnected, ClientsConnected > 1 ? "Clients" : "Client"));

                // Queue a ClientTask for all connected Clients that asks them to save the UserDefaults,  to disconnect from the Server and to close
                QueueClientTask(-1, RemotingConstants.CLIENTTASKGROUP_DISCONNECT, "IMMEDIATE", 1);

                // Loop that checks if all Clients have responded by disconnecting or if at least one Client didn't respond and a timeout was exceeded
CheckAllClientsDisconnected:
                Thread.Sleep(SLEEP_TIME_PER_RETRY);

                if ((ClientsConnected > 0)
                    && (Retries < MAX_RETRIES))
                {
                    Retries++;

                    if (Retries % 4 == 1)
                    {
                        TLogging.Log("    " +
                            String.Format(Catalog.GetString(
                                    "CONTROLLED SHUTDOWN: There {2} still {0} {1} connected. Waiting for {3} to disconnect (Waiting {4} more seconds)..."),
                                ClientsConnected, ClientsConnected > 1 ? "Clients" : "Client",
                                ClientsConnected > 1 ? "are" : "is", ClientsConnected > 1 ? "them" : "it",
                                ((SLEEP_TIME_PER_RETRY * MAX_RETRIES) - (SLEEP_TIME_PER_RETRY * Retries)) / 1000));
                    }

                    goto CheckAllClientsDisconnected;
                }

                // Check if at least one Client is still connected
                if (Retries == MAX_RETRIES)
                {
                    // Yes there is still at least one Client connected
                    TLogging.Log("  " +
                        String.Format(Catalog.GetString(
                                "CONTROLLED SHUTDOWN: {0} did not respond to the disconnect request. Enter FORCE to force closing of the {1} and shutdown the Server, or anything else to leave this command."),
                            ClientsConnected == 1 ? "One Client" : ClientsConnected + " Clients", ClientsConnected > 1 ? "Clients" : "Client"));

                    // Special handling in case this Method is called from the ServerAdminConsole application
                    if (AForceAutomaticClosing)
                    {
                        // Check again that there are still Clients connected (could have disconnected while the user was typing 'FORCE'!)
                        if (ClientsConnected > 0)
                        {
                            // Yes there is still at least one Client connected
                            TLogging.Log("    " +
                                String.Format(Catalog.GetString(
                                        "CONTROLLED SHUTDOWN: Forcing all connected Clients ({0} {1}) to close. Please wait..."),
                                    ClientsConnected, ClientsConnected > 1 ? "Clients" : "Client"));

                            // Queue a ClienTasks for all Clients that are still connected that asks them to close (no saving of UserDefaults and no disconnection from the server!). This is a fallback mechanism.
                            QueueClientTask(-1, RemotingConstants.CLIENTTASKGROUP_DISCONNECT, "IMMEDIATE-HARDEXIT", 1);

                            // Loop as long as TSrvSetting.ClientKeepAliveCheckIntervalInSeconds is to ensure that all Clients will have got the chance to pick up the queued Client Task
                            // (since it would not be easy to determine that every connected Client has picked up this message, this is the easy way of ensuring that).
                            for (int Counter = 1; Counter <= 4; Counter++)
                            {
                                TLogging.Log("    " +
                                    String.Format(Catalog.GetString(
                                            "CONTROLLED SHUTDOWN: Waiting {0} seconds so the Server can be sure that all Clients have got the message that they need to close... ({1} more seconds to wait)"),
                                        TSrvSetting.ClientKeepAliveCheckIntervalInSeconds / 1000,
                                        (TSrvSetting.ClientKeepAliveCheckIntervalInSeconds -
                                         ((TSrvSetting.ClientKeepAliveCheckIntervalInSeconds / 4) * (Counter - 1))) / 1000));

                                Thread.Sleep(TSrvSetting.ClientKeepAliveCheckIntervalInSeconds / 4);
                            }
                        }
                        else
                        {
                            // No Clients connected anymore -> we can shut down the server.
                            TLogging.Log("  " +
                                Catalog.GetString(
                                    "CONTROLLED SHUTDOWN: All Clients have disconnected in the meantine, proceeding with shutdown immediately."));
                        }
                    }
                    else
                    {
                        // Abandon the shutdown as there are still connected clients and we are not allowed to force the shutdown
                        return false;
                    }
                }
            }
            else
            {
                // No Clients connected anymore -> we can shut down the server.
                TLogging.Log("  " + Catalog.GetString("CONTROLLED SHUTDOWN: There were no Clients connected, proceeding with shutdown immediately."));
            }

            // We are now ready to stop the server.
            StopServer();

            return true;  // this will never get executed, but is necessary for that Method to compile...
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
            Console.WriteLine();
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
            return TClientManager.ServerDisconnectClient(AClientID, out ACantDisconnectReason);
        }

        /// <summary>
        /// Queues a ClientTask for a certain Client.
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="ATaskGroup">Group of the Task</param>
        /// <param name="ATaskCode">Code of the Task</param>
        /// <param name="ATaskPriority">Priority of the Task</param>
        /// <returns>true if ClientTask was queued, otherwise false.</returns>
        public bool QueueClientTask(System.Int16 AClientID, String ATaskGroup, String ATaskCode, System.Int16 ATaskPriority)
        {
            return TClientManager.QueueClientTask(AClientID, ATaskGroup, ATaskCode, null, null, null, null, ATaskPriority) >= 0;
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
        /// RefreshAllCachedTables
        /// </summary>
        public virtual void RefreshAllCachedTables()
        {
            // implemented in derived class
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
