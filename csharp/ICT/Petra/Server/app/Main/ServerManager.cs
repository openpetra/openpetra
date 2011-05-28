//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.Interfaces.ServerAdminInterface;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Server.App.Main
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
    public class TServerManager : MarshalByRefObject, IServerAdminInterface
    {
        /// <summary>Reference to the Logging object</summary>
        public static TLogging ULogger;

        /// <summary>Keeps track of the number of times this Class has been
        /// instantiated</summary>
        private Int32 FNumberServerManagerInstances;

        /// <summary>Reference to the global TSrvSettings object</summary>
        private TSrvSetting FServerSettings;

        /// <summary>System wide defaults</summary>
        private TSystemDefaultsCache FSystemDefaultsCache;

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

        /// <summary>Server setting: Name of the Server configuration file</summary>
        public String ConfigurationFileName
        {
            get
            {
                return TSrvSetting.ConfigurationFile;
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
        /// <returns>void</returns>
        public TServerManager() : base()
        {
            FNumberServerManagerInstances = 0;

            SetupServerSettings();
            ULogger = new TLogging(TSrvSetting.ServerLogFile);

            // Create SystemDefaults Cache
            FSystemDefaultsCache = new TSystemDefaultsCache();

            ClientManager.InitializeUnit();
            TClientManager.SystemDefaultsCache = FSystemDefaultsCache;
            FFirstInstance = (FNumberServerManagerInstances == 0);
            FNumberServerManagerInstances++;
        }

        /// <summary>
        /// Default destructor.
        /// </summary>
        ~TServerManager()
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
        /// Reads Command Line Arguments and stores them in a TCommandLineArguments
        /// object.
        ///
        /// </summary>
        /// <returns>Instantiated TCommandLineArguments object
        /// </returns>
        private TCommandLineArguments ReadCommandLineArguments()
        {
            TCmdOpts CommandOptionsProcessor;
            TCommandLineArguments CommandLineArguments;

            CommandLineArguments = new TCommandLineArguments();
            CommandLineArguments.ApplicationName = Environment.GetCommandLineArgs()[0];
            CommandOptionsProcessor = new TCmdOpts();

            CommandLineArguments.ConfigurationFile = TAppSettingsManager.ConfigFileName;

            // Store command line options in TCommandLineArguments object
            if (CommandOptionsProcessor.IsFlagSet("C"))
            {
                CommandLineArguments.ConfigurationFile = CommandOptionsProcessor.GetOptValue("C");
            }

            return CommandLineArguments;
        }

        /// <summary>
        /// Returns the ASCII code value of a Character.
        ///
        /// </summary>
        /// <param name="AChar">Character for which the ASCII code value should be returned</param>
        /// <returns>ASCII code value
        /// </returns>
        private Int16 Asc(Char AChar)
        {
            return System.Int16.Parse((Encoding.ASCII.GetBytes(new char[] { AChar })[0].ToString()));
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
        /// Parses settings from the Application Configuration File, determines network
        /// configuration of the Server and Database connection parameters and stores
        /// all these settings in the global TSrvSettings object.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupServerSettings()
        {
            TCommandLineArguments CmdLineArgs;
            String ServerLogFile;
            String ServerName;
            String ServerIPAddresses;
            String ODBCDsnAppSetting;
            TDBType RDBMSTypeAppSetting;
            Int16 ServerIPBasePort;
            Int16 ServerDebugLevel;
            Int16 ClientIdleStatusAfterXMinutes;
            Int16 ClientKeepAliveCheckIntervalInSeconds;
            Int16 ClientKeepAliveTimeoutAfterXSecondsLAN;
            Int16 ClientKeepAliveTimeoutAfterXSecondsRemote;
            Int16 ClientConnectionTimeoutAfterXSeconds;
            Boolean ClientAppDomainShutdownAfterKeepAliveTimeout;
            string SMTPServer;
            bool AutomaticIntranetExportEnabled;
            bool RunAsStandalone;
            string IntranetDataDestinationEmail;
            string IntranetDataSenderEmail;

            CmdLineArgs = ReadCommandLineArguments();
            new TAppSettingsManager(CmdLineArgs.ConfigurationFile);

            #region Parse settings from the Application Configuration File

            // Server.RDBMSType
            RDBMSTypeAppSetting = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType"));

            // Server.ODBC_DSN
            ODBCDsnAppSetting = TAppSettingsManager.GetValue("Server.ODBC_DSN", false);

            string DatabaseHostOrFile = TAppSettingsManager.GetValue("Server.DBHostOrFile", "localhost");
            string DatabasePort = TAppSettingsManager.GetValue("Server.DBPort", "5432");
            string DatabaseName = TAppSettingsManager.GetValue("Server.DBName", "openpetra");
            string DatabaseUserName = TAppSettingsManager.GetValue("Server.DBUserName", "petraserver");
            string DatabasePassword = TAppSettingsManager.GetValue("Server.DBPassword");

            if (TAppSettingsManager.HasValue("Server.LogFile"))
            {
                ServerLogFile =
                    TAppSettingsManager.GetValue("Server.LogFile", false).Replace("{userappdata}",
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
            else
            {
                // maybe the log file has already been set, eg. by the NUnit Server Test
                ServerLogFile = TLogging.GetLogFileName();

                if (ServerLogFile.Length == 0)
                {
                    // this is effectively the bin directory (current directory)
                    ServerLogFile = "Server.log";
                }
            }

            // Server.IPBasePort
            ServerIPBasePort = TAppSettingsManager.GetInt16("Server.IPBasePort", 9000);

            // Server.DebugLevel
            ServerDebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

            RunAsStandalone = TAppSettingsManager.GetBoolean("Server.RunAsStandalone", false);

            // Server.ClientIdleStatusAfterXMinutes
            ClientIdleStatusAfterXMinutes = TAppSettingsManager.GetInt16("Server.ClientIdleStatusAfterXMinutes", 5);

            // Server.ClientKeepAliveCheckIntervalInSeconds
            ClientKeepAliveCheckIntervalInSeconds = TAppSettingsManager.GetInt16("Server.ClientKeepAliveCheckIntervalInSeconds", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_LAN
            ClientKeepAliveTimeoutAfterXSecondsLAN = TAppSettingsManager.GetInt16("Server.ClientKeepAliveTimeoutAfterXSeconds_LAN", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_Remote
            ClientKeepAliveTimeoutAfterXSecondsRemote =
                TAppSettingsManager.GetInt16("Server.ClientKeepAliveTimeoutAfterXSeconds_Remote", (short)(ClientKeepAliveTimeoutAfterXSecondsLAN * 2));

            // Server.ClientConnectionTimeoutAfterXSeconds
            ClientConnectionTimeoutAfterXSeconds = TAppSettingsManager.GetInt16("Server.ClientConnectionTimeoutAfterXSeconds", 20);

            // Server.ClientAppDomainShutdownAfterKeepAliveTimeout
            ClientAppDomainShutdownAfterKeepAliveTimeout = TAppSettingsManager.GetBoolean("Server.ClientAppDomainShutdownAfterKeepAliveTimeout", true);

            SMTPServer = TAppSettingsManager.GetValue("Server.SMTPServer", "localhost");

            // This is disabled in processing at the moment, so we reflect that here. When it works change to true
            AutomaticIntranetExportEnabled = TAppSettingsManager.GetBoolean("Server.AutomaticIntranetExportEnabled", false);

            // The following setting specifies the email address where the Intranet Data emails are sent to when "Server.AutomaticIntranetExportEnabled" is true.
            IntranetDataDestinationEmail = TAppSettingsManager.GetValue("Server.IntranetDataDestinationEmail", "???@???.org");

            // The following setting is temporary - until we have created a GUI where users can specify the email address for the
            // responsible Personnel and Finance persons themselves. Those will be stored in SystemDefaults then.
            IntranetDataSenderEmail = TAppSettingsManager.GetValue("Server.IntranetDataSenderEmail", "???@???.org");

            // Determine network configuration of the Server
            Networking.DetermineNetworkConfig(out ServerName, out ServerIPAddresses);

            Version ServerAssemblyVersion;

            if ((System.Reflection.Assembly.GetEntryAssembly() != null) && (System.Reflection.Assembly.GetEntryAssembly().GetName() != null))
            {
                ServerAssemblyVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                // retrieve the current version of the server from the file version.txt in the bin directory
                // this is easier to manage than to check the assembly version in case you only need to quickly update the client
                string BinPath = Environment.CurrentDirectory;

                if (File.Exists(BinPath + Path.DirectorySeparatorChar + "version.txt"))
                {
                    StreamReader srVersion = new StreamReader(BinPath + Path.DirectorySeparatorChar + "version.txt");
                    TFileVersionInfo v = new TFileVersionInfo(srVersion.ReadLine());
                    ServerAssemblyVersion = new Version(v.FileMajorPart, v.FileMinorPart, v.FileBuildPart, v.FilePrivatePart);
                    srVersion.Close();
                }
            }
            else
            {
                // this is with the web services, started with xsp.exe
                ServerAssemblyVersion = new Version(0, 0, 0, 0);
            }

            #endregion

            // Store Server configuration in the static TSrvSetting class
            FServerSettings = new TSrvSetting(
                CmdLineArgs.ApplicationName,
                CmdLineArgs.ConfigurationFile,
                ServerAssemblyVersion,
                Utilities.DetermineExecutingOS(),
                RDBMSTypeAppSetting,
                ODBCDsnAppSetting,
                DatabaseHostOrFile,
                DatabasePort,
                DatabaseName,
                DatabaseUserName,
                DatabasePassword,
                ServerIPBasePort,
                ServerDebugLevel,
                ServerLogFile,
                ServerName,
                ServerIPAddresses,
                ClientIdleStatusAfterXMinutes,
                ClientKeepAliveCheckIntervalInSeconds,
                ClientKeepAliveTimeoutAfterXSecondsLAN,
                ClientKeepAliveTimeoutAfterXSecondsRemote,
                ClientConnectionTimeoutAfterXSeconds,
                ClientAppDomainShutdownAfterKeepAliveTimeout,
                SMTPServer,
                AutomaticIntranetExportEnabled,
                RunAsStandalone,
                IntranetDataDestinationEmail,
                IntranetDataSenderEmail);
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
                dbToDisconnect.CloseDBConnection();
                FDBConnections.Remove(dbToDisconnect);
            }

            return DBsToDisconnect.Count > 0;
        }

        /// <summary>
        /// Opens a Database connection to the main Database.
        /// </summary>
        /// <returns>void</returns>
        public void EstablishDBConnection()
        {
            DBAccess.GDBAccessObj = new TDataBase();
            try
            {
                DBAccess.GDBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                    TSrvSetting.PostgreSQLServer,
                    TSrvSetting.PostgreSQLServerPort,
                    TSrvSetting.PostgreSQLDatabaseName,
                    TSrvSetting.DBUsername,
                    TSrvSetting.DBPassword,
                    "");

                string DBPatchVersion = "0.0.9-0";
                bool oldLegacyDB = false;
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

                try
                {
                    // now check if the database is uptodate; otherwise run db patch against it
                    DBPatchVersion =
                        Convert.ToString(DBAccess.GDBAccessObj.ExecuteScalar(
                                "SELECT s_default_value_c FROM PUB_s_system_defaults WHERE s_default_code_c = 'CurrentDatabaseVersion'",
                                transaction));
                }
                catch (Exception)
                {
                    // this can happen when connecting to an old Petra 2.x database
                    oldLegacyDB = true;
                }
                DBAccess.GDBAccessObj.RollbackTransaction();

                TFileVersionInfo dbversion = new TFileVersionInfo(DBPatchVersion);
                TFileVersionInfo serverExeInfo = new TFileVersionInfo(TSrvSetting.ApplicationVersion);

                if (serverExeInfo.Compare(new TFileVersionInfo("0.0.9-0")) == 0)
                {
                    // this is a developer version; database patching has to be done manually
                }
                else if (!oldLegacyDB)
                {
                    if (dbversion.Compare(serverExeInfo) < 0)
                    {
                        // for a proper server, the patchtool should have already updated the database

                        // for standalone versions, we update the database on the fly when starting the server
                        if (CommonTypes.ParseDBType(DBAccess.GDBAccessObj.DBType) == TDBType.SQLite)
                        {
                            UpdateSQLiteDatabase(dbversion, serverExeInfo);
                        }
                        else
                        {
                            throw new Exception(
                                "Cannot connect to old database, please restore the latest clean demo database or run nant patchDatabase");
                        }
                    }
                }

                // $IFDEF DEBUGMODE Console.WriteLine('SystemDefault "LocalisedCountyLabel": ' + FSystemDefaultsCache.GetSystemDefault('LocalisedCountyLabel'));$ENDIF
            }
            catch (Exception)
            {
                throw;
            }

            TLogging.Log("  " + Catalog.GetString("Connected to Database."));
        }

        /// <summary>
        /// For standalone installations, we update the SQLite database on the fly
        /// </summary>
        private void UpdateSQLiteDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion)
        {
            string dbpatchfilePath = Path.GetDirectoryName(TAppSettingsManager.GetValue("Server.SQLiteBaseFile"));

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            ADBVersion.FilePrivatePart = 0;
            AExeVersion.FilePrivatePart = 0;

            try
            {
                // run all available patches. for each release there could be a patch file
                string[] sqlFiles = Directory.GetFiles(dbpatchfilePath, "*.sql");

                bool foundUpdate = true;

                // run through all sql files until we have no matching update files anymore
                while (foundUpdate)
                {
                    foundUpdate = false;

                    foreach (string sqlFile in sqlFiles)
                    {
                        if (!sqlFile.EndsWith("pg.sql") && ADBVersion.PatchApplies(sqlFile, AExeVersion))
                        {
                            foundUpdate = true;
                            StreamReader sr = new StreamReader(sqlFile);

                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine().Trim();

                                if (!line.StartsWith("--"))
                                {
                                    DBAccess.GDBAccessObj.ExecuteNonQuery(line, transaction, false);
                                }
                            }

                            sr.Close();
                            ADBVersion = TFileVersionInfo.GetLatestPatchVersionFromDiffZipName(sqlFile);
                        }
                    }
                }

                if (ADBVersion.Compare(AExeVersion) == 0)
                {
                    // if patches have been applied successfully, update the database version
                    string newVersionSql =
                        String.Format("UPDATE s_system_defaults SET s_default_value_c = '{0}' WHERE s_default_code_c = 'CurrentDatabaseVersion';",
                            AExeVersion.ToStringDotsHyphen());
                    DBAccess.GDBAccessObj.ExecuteNonQuery(newVersionSql, transaction, false);
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw new Exception(String.Format("Cannot connect to old database (version {0}), there are some missing sql patch files",
                            ADBVersion));
                }
            }
            catch (Exception e)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                throw e;
            }
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
        /// Causes PetraServer to send a test e-mail to a recipient with the current Server Email Settings.
        /// </summary>
        public string SendServerTestEmail(string ARecipients)
        {
            bool result;
            string ReturnValue;

//            Console.WriteLine("Server Email Settings:" + Environment.NewLine +
//                "SmtpServer: " + TSrvSetting.SMTPServer + Environment.NewLine +
//                "AutoIntranetExportSetting: " + TSrvSetting.AutomaticIntranetExportEnabled.ToString());


            result = Ict.Common.EMailing.SMTPEmail.SendEmail(
                ARecipients, ARecipients, ARecipients,
                "Test Email from PetraServer",
                "PetraServer Test Email, as requested on " + DateTime.Now.ToString() + ".");

            ReturnValue = "Email sending result: ";

            if (result)
            {
                ReturnValue = ReturnValue + "OK";
            }
            else
            {
                ReturnValue = ReturnValue + "FAILURE";
            }

            return ReturnValue;
        }

        /// <summary>
        /// Allows loading of a 'fake' Client AppDomain.
        /// *** For development testing purposes only ***
        /// </summary>
        /// <param name="AUserName">Fake Username for the Client AppDomain</param>
        /// <returns></returns>
        public bool LoadClientAppDomain(String AUserName)
        {
            String ClientName;

            System.Int32 ClientID;
            System.Int16 RemotingPort;
            TExecutingOSEnum ServerOS;
            TClientManager ClientManagerObj;
            Hashtable RemotingURLs;
            String WelcomeMessage;
            Int32 ProcessID;
            TPetraPrincipal UserInfo;
            Boolean SystemEnabled;
            try
            {
                ClientManagerObj = new TClientManager();
                ClientManagerObj.ConnectClient(AUserName,
                    "password",
                    "FAKEClient ",
                    "Cli.ent.IP",
                    new System.Version("0.9.0.2"),
                    TClientServerConnectionType.csctLAN,
                    out ClientName,
                    out ClientID,
                    out RemotingPort,
                    out RemotingURLs,
                    out ServerOS,
                    out ProcessID,
                    out WelcomeMessage,
                    out SystemEnabled,
                    out UserInfo);
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(String.Format(Catalog.GetString("Exception occured during manual load of Client AppDomain: {0}"), exp.Message));
                return false;
            }
        }

        /// <summary>
        /// Stores Command Line Arguments in a structured way.
        /// </summary>
        private class TCommandLineArguments : object
        {
            /// <summary>.EXE file name of the Application</summary>
            private String FApplicationName;

            /// <summary>ConfigurationFile ('C') Command Line Argument</summary>
            private String FConfigurationFile = "";

            /// <summary>ConfigurationFile ('C') Command Line Argument</summary>
            public String ConfigurationFile
            {
                get
                {
                    return FConfigurationFile;
                }

                set
                {
                    FConfigurationFile = value;
                }
            }

            /// <summary>.EXE file name of the Application</summary>
            public String ApplicationName
            {
                get
                {
                    return FApplicationName;
                }

                set
                {
                    FApplicationName = value;
                }
            }
        }
    }
}