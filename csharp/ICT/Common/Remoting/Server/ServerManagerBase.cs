//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// some common implementations for IServerAdminInterface
    /// </summary>
    public class TServerManagerBase : MarshalByRefObject, IServerAdminInterface
    {
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
                // Result := 'PETRAServer x.x.x Build xxxxxx/xx/xxxx  (OS: ' + ExecutingOSEnumToString(TSrvSetting.ExecutingOS) + ')';
                return String.Format(
                    Catalog.GetString("PETRAServer {0} Build {1}  (OS: {2})"),
                    TSrvSetting.ApplicationVersion.ToString(),
                    System.IO.File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName).ToString(),
                    CommonTypes.ExecutingOSEnumToString(TSrvSetting.ExecutingOS));

                // System.Reflection.Assembly.GetEntryAssembly.FullName does not return the file path
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
        /// Ensures that the TServerManager class is instantiated only once remotely and
        /// doesn't get GCed
        /// </summary>
        /// <returns>An object of type ILease used to control the lifetime policy for this
        /// instance.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
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

            TLogging.Log(Catalog.GetString("SHUTDOWN PROCEDURE FINISHED"));
            Environment.Exit(0);

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
            bool ReturnValue;

            if (TClientManager.QueueClientTask(AClientID, ATaskGroup, ATaskCode, ATaskPriority) >= 0)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
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
        /// AddUser
        /// </summary>
        public virtual bool AddUser(string AUserId)
        {
            // implemented in derived class
            return false;
        }
    }
}