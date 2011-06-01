//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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

namespace Ict.Common
{
    /// <summary>
    /// Static class for storing Server settings. Once instantiated, Server settings
    /// can only be read!
    /// Server Settings are gathered from the Command line, .NET Configuration files
    /// and other ways (eg. determining the OS on which the server is running
    /// on-the-fly) at Server start-up.
    ///
    /// </summary>
    public class TSrvSetting : object
    {
        private static String UConfigurationFile;
        private static String UApplicationName;
        private static TDBType URDBMSType;
        private static String UODBCDsn;
        private static String UPostgreSQLServer;
        private static String UPostgreSQLServerPort;
        private static String UPostgreSQLDatabaseName;
        private static String UDBUsername;
        private static String UDBPassword;
        private static String UServerLogFile;
        private static String UHostName;
        private static String UHostIPAddresses;
        private static System.Version UApplicationVersion;
        private static System.Int16 UIPBasePort;
        private static System.Int16 UClientIdleStatusAfterXMinutes;
        private static System.Int16 UClientKeepAliveCheckIntervalInSeconds;
        private static System.Int16 UClientKeepAliveTimeoutAfterXSecondsLAN;
        private static System.Int16 UClientKeepAliveTimeoutAfterXSecondsRemote;
        private static System.Int16 UClientConnectionTimeoutAfterXSeconds;
        private static bool UClientAppDomainShutdownAfterKeepAliveTimeout;
        private static TExecutingOSEnum UExecutingOS;
        private static string USMTPServer;
        private static bool UAutomaticIntranetExportEnabled;
        private static string UIntranetDataDestinationEmail;
        private static string UIntranetDataSenderEmail;
        private static bool URunAsStandalone;

        #region Properties

        /// <summary>Name of .NET Configuration File, if specified via command line options</summary>
        public static String ConfigurationFile
        {
            get
            {
                return UConfigurationFile;
            }
        }

        /// <summary>Name of the Server's .exe</summary>
        public static String ApplicationName
        {
            get
            {
                return UApplicationName;
            }
        }

        /// <summary>Assembly Version of the Server's .exe</summary>
        public static System.Version ApplicationVersion
        {
            get
            {
                return UApplicationVersion;
            }
        }

        /// <summary>Operating System the Server is running on</summary>
        public static TExecutingOSEnum ExecutingOS
        {
            get
            {
                return UExecutingOS;
            }
        }

        /// <summary>Type of RDBMS (Relational Database Management System) that the Server is connected to</summary>
        public static TDBType RDMBSType
        {
            get
            {
                return URDBMSType;
            }
        }

        /// <summary>ODBC DSN used to connect to the RDBMS</summary>
        public static String ODBCDsn
        {
            get
            {
                return UODBCDsn;
            }
        }

        /// <summary>Username used to connect to the RDBMS</summary>
        public static String DBUsername
        {
            get
            {
                return UDBUsername;
            }
        }

        /// <summary>Password used to connect to the RDBMS</summary>
        public static String DBPassword
        {
            get
            {
                return UDBPassword;
            }
        }

        /// <summary>Computer name of the Server</summary>
        public static String HostName
        {
            get
            {
                return UHostName;
            }
        }

        /// <summary>IP Address(es) of the Server</summary>
        public static String HostIPAddresses
        {
            get
            {
                return UHostIPAddresses;
            }
        }

        /// <summary>IP Address at which the Server is listening for Client connection/disconnection requests</summary>
        public static System.Int16 IPBasePort
        {
            get
            {
                return UIPBasePort;
            }
        }

        /// <summary>This is the path to the server log file</summary>
        public static String ServerLogFile
        {
            get
            {
                return UServerLogFile;
            }
        }

        /// <summary>The amount of time in minutes after which a Client's status is set to 'Idle' when no activity occurs</summary>
        public static System.Int16 ClientIdleStatusAfterXMinutes
        {
            get
            {
                return UClientIdleStatusAfterXMinutes;
            }
        }

        /// <summary>The interval in seconds in which the PetraServer checks whether the Client made contact</summary>
        public static System.Int16 ClientKeepAliveCheckIntervalInSeconds
        {
            get
            {
                return UClientKeepAliveCheckIntervalInSeconds;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (LAN connection)</summary>
        public static System.Int16 ClientKeepAliveTimeoutAfterXSecondsLAN
        {
            get
            {
                return UClientKeepAliveTimeoutAfterXSecondsLAN;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (Remote connection)</summary>
        public static System.Int16 ClientKeepAliveTimeoutAfterXSecondsRemote
        {
            get
            {
                return UClientKeepAliveTimeoutAfterXSecondsRemote;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's attempt to connect to the Server times out</summary>
        public static System.Int16 ClientConnectionTimeoutAfterXSeconds
        {
            get
            {
                return UClientConnectionTimeoutAfterXSeconds;
            }
        }

        /// <summary>For debugging purposes only: if set to false, the Client's AppDomain is not teared down when no KeepAlive signal was received (allows to inspect whether all remoted objects are properly released)</summary>
        public static bool ClientAppDomainShutdownAfterKeepAliveTimeout
        {
            get
            {
                return UClientAppDomainShutdownAfterKeepAliveTimeout;
            }
        }

        /// <summary>
        /// the hostname or IP address of the server that is running PostgreSQL for us
        /// </summary>
        public static string PostgreSQLServer
        {
            get
            {
                return UPostgreSQLServer;
            }
        }

        /// <summary>
        /// the port of the PostgreSQL server
        /// </summary>
        public static string PostgreSQLServerPort
        {
            get
            {
                return UPostgreSQLServerPort;
            }
        }

        /// <summary>
        /// the name of the PostgreSQL database
        /// </summary>
        public static string PostgreSQLDatabaseName
        {
            get
            {
                return UPostgreSQLDatabaseName;
            }
        }


        /// <summary>
        /// Which server to use for sending email.
        /// </summary>
        public static string SMTPServer
        {
            get
            {
                return USMTPServer;
            }
        }

        /// <summary>
        /// A way of turning off Automatic Intranet Export.
        /// </summary>
        public static bool AutomaticIntranetExportEnabled
        {
            get
            {
                return UAutomaticIntranetExportEnabled;
            }
        }

        /// <summary>
        /// True if PetraServer is running as a server for a Standalone Petra, otherwise false.
        /// </summary>
        public static bool RunAsStandalone
        {
            get
            {
                return URunAsStandalone;
            }
        }

        /// <summary>
        /// Email address that the Intranet Upload Data should get sent to.
        /// </summary>
        public static string IntranetDataDestinationEmail
        {
            get
            {
                return UIntranetDataDestinationEmail;
            }
        }

        /// <summary>
        /// Email address of the user that creates the Intranet Upload Data.
        /// </summary>
        public static string IntranetDataSenderEmail
        {
            get
            {
                return UIntranetDataSenderEmail;
            }
        }

        #endregion


        /// <summary>
        /// Initialises the internal variables that hold the Server Settings.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TSrvSetting(String AApplicationName,
            String AConfigurationFile,
            System.Version AApplicationVersion,
            TExecutingOSEnum AExecutingOS,
            TDBType ARDMBSType,
            String AODBCDsn,
            String APostreSQLServer,
            String APostreSQLServerPort,
            String APostgreSQLDatabaseName,
            String ADBUsername,
            String ADBPassword,
            System.Int16 AIPBasePort,
            System.Int32 ADebugLevel,
            String AServerLogFile,
            String AHostName,
            String AHostIPAddresses,
            System.Int16 AClientIdleStatusAfterXMinutes,
            System.Int16 AClientKeepAliveTimeoutAfterXSecondsLAN,
            System.Int16 AClientKeepAliveCheckIntervalInSeconds,
            System.Int16 AClientKeepAliveTimeoutAfterXSecondsRemote,
            System.Int16 AClientConnectionTimeoutAfterXSeconds,
            bool AClientAppDomainShutdownAfterKeepAliveTimeout,
            string ASmtpServer,
            bool AAutomaticIntranetExportEnabled,
            bool ARunAsStandalone,
            string AIntranetDataDestinationEmail,
            string AIntranetDataSenderEmail)
        {
            UApplicationName = AApplicationName;
            UConfigurationFile = AConfigurationFile;
            UExecutingOS = AExecutingOS;
            URDBMSType = ARDMBSType;
            UODBCDsn = AODBCDsn;
            UPostgreSQLServer = APostreSQLServer;
            UPostgreSQLServerPort = APostreSQLServerPort;
            UPostgreSQLDatabaseName = APostgreSQLDatabaseName;
            UDBUsername = ADBUsername;
            UDBPassword = ADBPassword;
            UIPBasePort = AIPBasePort;
            TLogging.DebugLevel = ADebugLevel;
            UServerLogFile = AServerLogFile;
            UHostName = AHostName;
            UHostIPAddresses = AHostIPAddresses;
            UClientIdleStatusAfterXMinutes = AClientIdleStatusAfterXMinutes;
            UClientKeepAliveCheckIntervalInSeconds = AClientKeepAliveCheckIntervalInSeconds;
            UClientKeepAliveTimeoutAfterXSecondsLAN = AClientKeepAliveTimeoutAfterXSecondsLAN;
            UClientKeepAliveTimeoutAfterXSecondsRemote = AClientKeepAliveTimeoutAfterXSecondsRemote;
            UClientConnectionTimeoutAfterXSeconds = AClientConnectionTimeoutAfterXSeconds;
            UClientAppDomainShutdownAfterKeepAliveTimeout = AClientAppDomainShutdownAfterKeepAliveTimeout;
            UApplicationVersion = AApplicationVersion;
            USMTPServer = ASmtpServer;
            UAutomaticIntranetExportEnabled = AAutomaticIntranetExportEnabled;
            URunAsStandalone = ARunAsStandalone;
            UIntranetDataDestinationEmail = AIntranetDataDestinationEmail;
            UIntranetDataSenderEmail = AIntranetDataSenderEmail;
        }
    }
}