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
using System.IO;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Static class for storing Server settings. Once instantiated, Server settings
    /// can only be read!
    /// Server Settings are gathered from the Command line, .NET Configuration files
    /// and other ways (eg. determining the OS on which the server is running
    /// on-the-fly) at Server start-up.
    ///
    /// </summary>
    [Serializable()]
    public class TSrvSetting
    {
        private String FConfigurationFile;
        private String FApplicationName;
        private TDBType FRDBMSType;
        private String FODBCDsn;
        private String FDatabaseHostOrFile;
        private String FDatabasePort;
        private String FDatabaseName;
        private String FDBUsername;
        private String FDBPassword;
        private String FServerLogFile;
        private String FHostName;
        private String FHostIPAddresses;
        private System.Version FApplicationVersion;
        private System.Int16 FIPBasePort;
        private System.Int16 FClientIdleStatusAfterXMinutes;
        private System.Int16 FClientKeepAliveCheckIntervalInSeconds;
        private System.Int16 FClientKeepAliveTimeoutAfterXSecondsLAN;
        private System.Int16 FClientKeepAliveTimeoutAfterXSecondsRemote;
        private System.Int16 FClientConnectionTimeoutAfterXSeconds;
        private bool FClientAppDomainShutdownAfterKeepAliveTimeout;
        private TExecutingOSEnum FExecutingOS;
        private string FSMTPServer;
        private bool FAutomaticIntranetExportEnabled;
        private string FIntranetDataDestinationEmail;
        private string FIntranetDataSenderEmail;
        private bool FRunAsStandalone;

        #region Properties
        private static TSrvSetting USingletonSrvSetting = null;

        /// get a copy of the current values
        public static TSrvSetting ServerSettings
        {
            get
            {
                return new TSrvSetting(USingletonSrvSetting);
            }
        }

        /// <summary>Name of .NET Configuration File, if specified via command line options</summary>
        public static String ConfigurationFile
        {
            get
            {
                return USingletonSrvSetting.FConfigurationFile;
            }
        }

        /// <summary>Name of the Server's .exe</summary>
        public static String ApplicationName
        {
            get
            {
                return USingletonSrvSetting.FApplicationName;
            }
        }

        /// <summary>Assembly Version of the Server's .exe</summary>
        public static System.Version ApplicationVersion
        {
            get
            {
                return USingletonSrvSetting.FApplicationVersion;
            }
        }

        /// <summary>Operating System the Server is running on</summary>
        public static TExecutingOSEnum ExecutingOS
        {
            get
            {
                return USingletonSrvSetting.FExecutingOS;
            }
        }

        /// <summary>Type of RDBMS (Relational Database Management System) that the Server is connected to</summary>
        public static TDBType RDMBSType
        {
            get
            {
                return USingletonSrvSetting.FRDBMSType;
            }
        }

        /// <summary>ODBC DSN used to connect to the RDBMS</summary>
        public static String ODBCDsn
        {
            get
            {
                return USingletonSrvSetting.FODBCDsn;
            }
        }

        /// <summary>Username used to connect to the RDBMS</summary>
        public static String DBUsername
        {
            get
            {
                return USingletonSrvSetting.FDBUsername;
            }
        }

        /// <summary>Password used to connect to the RDBMS</summary>
        public static String DBPassword
        {
            get
            {
                return USingletonSrvSetting.FDBPassword;
            }
        }

        /// <summary>Computer name of the Server</summary>
        public static String HostName
        {
            get
            {
                return USingletonSrvSetting.FHostName;
            }
        }

        /// <summary>IP Address(es) of the Server</summary>
        public static String HostIPAddresses
        {
            get
            {
                return USingletonSrvSetting.FHostIPAddresses;
            }
        }

        /// <summary>IP Address at which the Server is listening for Client connection/disconnection requests</summary>
        public static System.Int16 IPBasePort
        {
            get
            {
                return USingletonSrvSetting.FIPBasePort;
            }
        }

        /// <summary>This is the path to the server log file</summary>
        public static String ServerLogFile
        {
            get
            {
                return USingletonSrvSetting.FServerLogFile;
            }
        }

        /// <summary>The amount of time in minutes after which a Client's status is set to 'Idle' when no activity occurs</summary>
        public static System.Int16 ClientIdleStatusAfterXMinutes
        {
            get
            {
                return USingletonSrvSetting.FClientIdleStatusAfterXMinutes;
            }
        }

        /// <summary>The interval in seconds in which the PetraServer checks whether the Client made contact</summary>
        public static System.Int16 ClientKeepAliveCheckIntervalInSeconds
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveCheckIntervalInSeconds;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (LAN connection)</summary>
        public static System.Int16 ClientKeepAliveTimeoutAfterXSecondsLAN
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveTimeoutAfterXSecondsLAN;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (Remote connection)</summary>
        public static System.Int16 ClientKeepAliveTimeoutAfterXSecondsRemote
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveTimeoutAfterXSecondsRemote;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's attempt to connect to the Server times out</summary>
        public static System.Int16 ClientConnectionTimeoutAfterXSeconds
        {
            get
            {
                return USingletonSrvSetting.FClientConnectionTimeoutAfterXSeconds;
            }
        }

        /// <summary>For debugging purposes only: if set to false, the Client's AppDomain is not teared down when no KeepAlive signal was received (allows to inspect whether all remoted objects are properly released)</summary>
        public static bool ClientAppDomainShutdownAfterKeepAliveTimeout
        {
            get
            {
                return USingletonSrvSetting.FClientAppDomainShutdownAfterKeepAliveTimeout;
            }
        }

        /// <summary>
        /// the hostname or IP address of the server that is running PostgreSQL for us
        /// </summary>
        public static string PostgreSQLServer
        {
            get
            {
                return USingletonSrvSetting.FDatabaseHostOrFile;
            }
        }

        /// <summary>
        /// the port of the PostgreSQL server
        /// </summary>
        public static string PostgreSQLServerPort
        {
            get
            {
                return USingletonSrvSetting.FDatabasePort;
            }
        }

        /// <summary>
        /// the name of the PostgreSQL database
        /// </summary>
        public static string PostgreSQLDatabaseName
        {
            get
            {
                return USingletonSrvSetting.FDatabaseName;
            }
        }


        /// <summary>
        /// Which server to use for sending email.
        /// </summary>
        public static string SMTPServer
        {
            get
            {
                return USingletonSrvSetting.FSMTPServer;
            }
        }

        /// <summary>
        /// A way of turning off Automatic Intranet Export.
        /// </summary>
        public static bool AutomaticIntranetExportEnabled
        {
            get
            {
                return USingletonSrvSetting.FAutomaticIntranetExportEnabled;
            }
        }

        /// <summary>
        /// True if PetraServer is running as a server for a Standalone Petra, otherwise false.
        /// </summary>
        public static bool RunAsStandalone
        {
            get
            {
                return USingletonSrvSetting.FRunAsStandalone;
            }
        }

        /// <summary>
        /// Email address that the Intranet Fpload Data should get sent to.
        /// </summary>
        public static string IntranetDataDestinationEmail
        {
            get
            {
                return USingletonSrvSetting.FIntranetDataDestinationEmail;
            }
        }

        /// <summary>
        /// Email address of the user that creates the Intranet Fpload Data.
        /// </summary>
        public static string IntranetDataSenderEmail
        {
            get
            {
                return USingletonSrvSetting.FIntranetDataSenderEmail;
            }
        }

        #endregion

        /// Copy constructor
        public TSrvSetting(TSrvSetting ACopyFrom)
        {
            if (USingletonSrvSetting == null)
            {
                USingletonSrvSetting = this;
            }

            FApplicationName = ACopyFrom.FApplicationName;
            FConfigurationFile = ACopyFrom.FConfigurationFile;
            FExecutingOS = ACopyFrom.FExecutingOS;
            FRDBMSType = ACopyFrom.FRDBMSType;
            FODBCDsn = ACopyFrom.FODBCDsn;
            FDatabaseHostOrFile = ACopyFrom.FDatabaseHostOrFile;
            FDatabasePort = ACopyFrom.FDatabasePort;
            FDatabaseName = ACopyFrom.FDatabaseName;
            FDBUsername = ACopyFrom.FDBUsername;
            FDBPassword = ACopyFrom.FDBPassword;
            FIPBasePort = ACopyFrom.FIPBasePort;
            FServerLogFile = ACopyFrom.FServerLogFile;
            FHostName = ACopyFrom.FHostName;
            FHostIPAddresses = ACopyFrom.FHostIPAddresses;
            FClientIdleStatusAfterXMinutes = ACopyFrom.FClientIdleStatusAfterXMinutes;
            FClientKeepAliveCheckIntervalInSeconds = ACopyFrom.FClientKeepAliveCheckIntervalInSeconds;
            FClientKeepAliveTimeoutAfterXSecondsLAN = ACopyFrom.FClientKeepAliveTimeoutAfterXSecondsLAN;
            FClientKeepAliveTimeoutAfterXSecondsRemote = ACopyFrom.FClientKeepAliveTimeoutAfterXSecondsRemote;
            FClientConnectionTimeoutAfterXSeconds = ACopyFrom.FClientConnectionTimeoutAfterXSeconds;
            FClientAppDomainShutdownAfterKeepAliveTimeout = ACopyFrom.FClientAppDomainShutdownAfterKeepAliveTimeout;
            FApplicationVersion = ACopyFrom.FApplicationVersion;
            FSMTPServer = ACopyFrom.FSMTPServer;
            FAutomaticIntranetExportEnabled = ACopyFrom.FAutomaticIntranetExportEnabled;
            FRunAsStandalone = ACopyFrom.FRunAsStandalone;
            FIntranetDataDestinationEmail = ACopyFrom.FIntranetDataDestinationEmail;
            FIntranetDataSenderEmail = ACopyFrom.FIntranetDataSenderEmail;
        }

        /// <summary>
        /// Initialises the internal variables that hold the Server Settings, using the current config file.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TSrvSetting()
        {
            if (USingletonSrvSetting == null)
            {
                USingletonSrvSetting = this;
            }

            FApplicationName = TAppSettingsManager.ApplicationName;
            FConfigurationFile = TAppSettingsManager.ConfigFileName;
            FExecutingOS = Utilities.DetermineExecutingOS();

            // Server.RDBMSType
            FRDBMSType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType"));

            // Server.ODBC_DSN
            FODBCDsn = TAppSettingsManager.GetValue("Server.ODBC_DSN", false);

            FDatabaseHostOrFile = TAppSettingsManager.GetValue("Server.DBHostOrFile", "localhost").
                                  Replace("{userappdata}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            FDatabasePort = TAppSettingsManager.GetValue("Server.DBPort", "5432");
            FDatabaseName = TAppSettingsManager.GetValue("Server.DBName", "openpetra");
            FDBUsername = TAppSettingsManager.GetValue("Server.DBUserName", "petraserver");
            FDBPassword = TAppSettingsManager.GetValue("Server.DBPassword");

            if (TAppSettingsManager.HasValue("Server.LogFile"))
            {
                FServerLogFile =
                    TAppSettingsManager.GetValue("Server.LogFile", false).Replace("{userappdata}",
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
            else
            {
                // maybe the log file has already been set, eg. by the NUnit Server Test
                FServerLogFile = TLogging.GetLogFileName();

                if (FServerLogFile.Length == 0)
                {
                    // this is effectively the bin directory (current directory)
                    FServerLogFile = "Server.log";
                }
            }

            // Server.IPBasePort
            FIPBasePort = TAppSettingsManager.GetInt16("Server.IPBasePort", 9000);

            FRunAsStandalone = TAppSettingsManager.GetBoolean("Server.RunAsStandalone", false);

            // Server.ClientIdleStatusAfterXMinutes
            FClientIdleStatusAfterXMinutes = TAppSettingsManager.GetInt16("Server.ClientIdleStatusAfterXMinutes", 5);

            // Server.ClientKeepAliveCheckIntervalInSeconds
            FClientKeepAliveCheckIntervalInSeconds = TAppSettingsManager.GetInt16("Server.ClientKeepAliveCheckIntervalInSeconds", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_LAN
            FClientKeepAliveTimeoutAfterXSecondsLAN = TAppSettingsManager.GetInt16("Server.ClientKeepAliveTimeoutAfterXSeconds_LAN", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_Remote
            FClientKeepAliveTimeoutAfterXSecondsRemote =
                TAppSettingsManager.GetInt16("Server.ClientKeepAliveTimeoutAfterXSeconds_Remote", (short)(ClientKeepAliveTimeoutAfterXSecondsLAN * 2));

            // Server.ClientConnectionTimeoutAfterXSeconds
            FClientConnectionTimeoutAfterXSeconds = TAppSettingsManager.GetInt16("Server.ClientConnectionTimeoutAfterXSeconds", 20);

            // Server.ClientAppDomainShutdownAfterKeepAliveTimeout
            FClientAppDomainShutdownAfterKeepAliveTimeout = TAppSettingsManager.GetBoolean("Server.ClientAppDomainShutdownAfterKeepAliveTimeout",
                true);

            FSMTPServer = TAppSettingsManager.GetValue("Server.SMTPServer", "localhost");

            // This is disabled in processing at the moment, so we reflect that here. When it works change to true
            FAutomaticIntranetExportEnabled = TAppSettingsManager.GetBoolean("Server.AutomaticIntranetExportEnabled", false);

            // The following setting specifies the email address where the Intranet Data emails are sent to when "Server.AutomaticIntranetExportEnabled" is true.
            FIntranetDataDestinationEmail = TAppSettingsManager.GetValue("Server.IntranetDataDestinationEmail", "???@???.org");

            // The following setting is temporary - until we have created a GUI where users can specify the email address for the
            // responsible Personnel and Finance persons themselves. Those will be stored in SystemDefaults then.
            FIntranetDataSenderEmail = TAppSettingsManager.GetValue("Server.IntranetDataSenderEmail", "???@???.org");

            // Determine network configuration of the Server
            Networking.DetermineNetworkConfig(out FHostName, out FHostIPAddresses);

            if ((System.Reflection.Assembly.GetEntryAssembly() != null) && (System.Reflection.Assembly.GetEntryAssembly().GetName() != null))
            {
                FApplicationVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                // retrieve the current version of the server from the file version.txt in the bin directory
                // this is easier to manage than to check the assembly version in case you only need to quickly update the client
                string BinPath = Environment.CurrentDirectory;

                if (File.Exists(BinPath + Path.DirectorySeparatorChar + "version.txt"))
                {
                    StreamReader srVersion = new StreamReader(BinPath + Path.DirectorySeparatorChar + "version.txt");
                    TFileVersionInfo v = new TFileVersionInfo(srVersion.ReadLine());
                    FApplicationVersion = new Version(v.FileMajorPart, v.FileMinorPart, v.FileBuildPart, v.FilePrivatePart);
                    srVersion.Close();
                }
            }
            else
            {
                // this is with the web services, started with xsp.exe
                FApplicationVersion = new Version(0, 0, 0, 0);
            }
        }
    }
}