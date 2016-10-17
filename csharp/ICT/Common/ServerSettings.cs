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
using System.IO;

using Ict.Common;

#region changelog

/* Unifying and standardising system settings, especially for SMTP and email - Moray
 *
 *   Removed the following unused properties:
 *     IntranetDataDestinationEmail     - now set from system default INTRANETSERVERADDRESS.
 *     IntranetDataSenderEmail          - now set from user defaults.
 *     AutomaticIntranetExportEnabled   - not required here?
 *   The old AutomaticIntranetExportEnabled has now become Server.Processing.AutomatedIntranetExport.Enabled. This and similar enablers for
 *   PartnerReminders and DataChecks are only referred to from ServerManager.cs, which references TAppSettingsManager and not TSrvSetting.
 *   If we added AutomatedIntranetExport here, we'd have to add the others too, and there seems little point without modifying ServerManager too.
 *
 *   Updated the SMTPServer property - it's now SmtpHost - and add the missing SMTP configuration:
 *     SmtpPort
 *     SmtpUser
 *     SmtpPassword
 *     SmtpEnableSsl
 *     SmtpAuthenticationType
 *
 *   Generally default values provided by this class should be the same as those set by our own "nant initConfigFiles".
 *   Most of these defaults can be found in inc\nant\OpenPetra.common.xml.
 *
 *   This class provides many different kinds of data to a variety of consumers. To add validation here, we'd have to add it to all properties,
 *   and probably start having to add decision logic too. We'd have to know what all the data was used for, and all the right exceptions to
 *   raise. Better to leave this to just hand the plain config file data to the consumers who are better positioned to know, for example, whether
 *   or not a default value is valid to use.
 */
#endregion

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
    [Serializable()]
    public class TSrvSetting
    {
        private String FConfigurationFile;
        private TDBType FRDBMSType;
        private String FDatabaseHostOrFile;
        private String FDatabasePort;
        private String FDatabaseName;
        private String FDBUsername;
        private String FDBPassword;
        private String FServerLogFile;
        private String FHostName;
        private String FHostIPAddresses;
        private TFileVersionInfo FApplicationVersion;
        private System.Int16 FIPBasePort;
        private System.Int32 FClientIdleStatusAfterXMinutes;
        private System.Int32 FClientKeepAliveCheckIntervalInSeconds;
        private System.Int32 FClientKeepAliveTimeoutAfterXSecondsLAN;
        private System.Int32 FClientKeepAliveTimeoutAfterXSecondsRemote;
        private System.Int32 FClientConnectionTimeoutAfterXSeconds;
        private bool FClientAppDomainShutdownAfterKeepAliveTimeout;
        private TExecutingOSEnum FExecutingOS;
        private string FSmtpHost;
        private int FSmtpPort;
        private string FSmtpUser;
        private string FSmtpPassword;
        private bool FSmtpEnableSsl;
        private string FSmtpAuthenticationType;
        private bool FSmtpIgnoreServerCertificateValidation;
        private bool FRunAsStandalone;
        private String FApplicationBinFolder;
        private int FDBConnectionCheckInterval;

        #region Properties
        private static TSrvSetting USingletonSrvSetting = null;

        /// Get a copy of the current values.
        public static TSrvSetting ServerSettings
        {
            get
            {
                return new TSrvSetting(USingletonSrvSetting);
            }
        }

        /// <summary>Path and name of .NET Configuration File (e.g. specified via command line option '-C').</summary>
        public static String ConfigurationFile
        {
            get
            {
                return USingletonSrvSetting.FConfigurationFile;
            }
        }

        /// <summary>Assembly Version of the Server's .exe</summary>
        public static TFileVersionInfo ApplicationVersion
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

        /// <summary>
        /// Polling interval in which to check whether the DB Connection is still OK
        /// (default = 0 = no such checks).
        /// </summary>
        public static int DBConnectionCheckInterval
        {
            get
            {
                return USingletonSrvSetting.FDBConnectionCheckInterval;
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
                if (USingletonSrvSetting != null)
                {
                    return USingletonSrvSetting.FServerLogFile;
                }

                return string.Empty;
            }
        }

        /// <summary>The amount of time in minutes after which a Client's status is set to 'Idle' when no activity occurs</summary>
        public static System.Int32 ClientIdleStatusAfterXMinutes
        {
            get
            {
                return USingletonSrvSetting.FClientIdleStatusAfterXMinutes;
            }
        }

        /// <summary>The interval in seconds in which the PetraServer checks whether the Client made contact</summary>
        public static System.Int32 ClientKeepAliveCheckIntervalInSeconds
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveCheckIntervalInSeconds;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (LAN connection)</summary>
        public static System.Int32 ClientKeepAliveTimeoutAfterXSecondsLAN
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveTimeoutAfterXSecondsLAN;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's AppDomain is teared down when no KeepAlive signal was received (Remote connection)</summary>
        public static System.Int32 ClientKeepAliveTimeoutAfterXSecondsRemote
        {
            get
            {
                return USingletonSrvSetting.FClientKeepAliveTimeoutAfterXSecondsRemote;
            }
        }

        /// <summary>The amount of time in seconds after which a Client's attempt to connect to the Server times out</summary>
        public static System.Int32 ClientConnectionTimeoutAfterXSeconds
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
        /// The hostname or IP address of the server that is running PostgreSQL for us.
        /// </summary>
        public static string PostgreSQLServer
        {
            get
            {
                return USingletonSrvSetting.FDatabaseHostOrFile;
            }
        }

        /// <summary>
        /// The port of the PostgreSQL server.
        /// </summary>
        public static string PostgreSQLServerPort
        {
            get
            {
                return USingletonSrvSetting.FDatabasePort;
            }
        }

        /// <summary>
        /// The name of the PostgreSQL database.
        /// </summary>
        public static string PostgreSQLDatabaseName
        {
            get
            {
                return USingletonSrvSetting.FDatabaseName;
            }
        }


        /// <summary>
        /// Which server to use for sending email. Default = "".
        /// </summary>
        public static string SmtpHost
        {
            get
            {
                return USingletonSrvSetting.FSmtpHost;
            }
        }

        /// <summary>
        /// The SMTP port to use. Default = 25.
        /// </summary>
        public static int SmtpPort
        {
            get
            {
                return USingletonSrvSetting.FSmtpPort;
            }
        }

        /// <summary>
        /// The username to log into the SMTP server if <see cref="SmtpAuthenticationType" /> = "config".
        /// Default = "YourSmtpUser"
        /// </summary>
        public static string SmtpUser
        {
            get
            {
                return USingletonSrvSetting.FSmtpUser;
            }
        }

        /// <summary>
        /// The password to log into the SMTP server if <see cref="SmtpAuthenticationType" /> = "config".
        /// Default = "YourSmtpPassword".
        /// </summary>
        public static string SmtpPassword
        {
            get
            {
                return USingletonSrvSetting.FSmtpPassword;
            }
        }

        /// <summary>
        /// Whether to use SSL to connect to the SMTP server. Default = true.
        /// </summary>
        public static bool SmtpEnableSsl
        {
            get
            {
                return USingletonSrvSetting.FSmtpEnableSsl;
            }
        }

        /// <summary>
        /// What type of SMTP authentication to use. Default = "config" to use the config file settings.
        /// </summary>
        public static string SmtpAuthenticationType
        {
            get
            {
                return USingletonSrvSetting.FSmtpAuthenticationType;
            }
        }

        /// <summary>
        /// Whether to pass SSL validity checks. Not recommended, but sometimes needed if you cannot find a place to get the public key for the ssl certificate,
        /// e.g. smtp.outlook365.com.
        /// </summary>
        public static bool SmtpIgnoreServerCertificateValidation
        {
            get
            {
                return USingletonSrvSetting.FSmtpIgnoreServerCertificateValidation;
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
        /// Folder that the Application is running from. Important only when the Server is run as a
        /// Windows Service as in this situation the folder that the Application is running from
        /// cannot be automatically determined (SERVICES.EXE is running the application in that
        /// situation from the {%SystemRoot%}\System32 directory)!
        /// </summary>
        public static string ApplicationBinFolder
        {
            get
            {
                return USingletonSrvSetting.FApplicationBinFolder;
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

            FConfigurationFile = ACopyFrom.FConfigurationFile;
            FExecutingOS = ACopyFrom.FExecutingOS;
            FRDBMSType = ACopyFrom.FRDBMSType;
            FDatabaseHostOrFile = ACopyFrom.FDatabaseHostOrFile;
            FDatabasePort = ACopyFrom.FDatabasePort;
            FDatabaseName = ACopyFrom.FDatabaseName;
            FDBUsername = ACopyFrom.FDBUsername;
            FDBPassword = ACopyFrom.FDBPassword;
            FDBConnectionCheckInterval = ACopyFrom.FDBConnectionCheckInterval;
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
            FSmtpHost = ACopyFrom.FSmtpHost;
            FSmtpPort = ACopyFrom.FSmtpPort;
            FSmtpUser = ACopyFrom.FSmtpUser;
            FSmtpPassword = ACopyFrom.FSmtpPassword;
            FSmtpEnableSsl = ACopyFrom.FSmtpEnableSsl;
            FSmtpAuthenticationType = ACopyFrom.FSmtpAuthenticationType;
            FSmtpIgnoreServerCertificateValidation = ACopyFrom.FSmtpIgnoreServerCertificateValidation;
            FRunAsStandalone = ACopyFrom.FRunAsStandalone;
            FApplicationBinFolder = ACopyFrom.FApplicationBinFolder;
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

            FConfigurationFile = TAppSettingsManager.ConfigFileName;
            FExecutingOS = Utilities.DetermineExecutingOS();

            // Server.RDBMSType
            FRDBMSType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType", "postgresql"));

            FDatabaseHostOrFile = TAppSettingsManager.GetValue("Server.DBHostOrFile", "localhost");
            FDatabasePort = TAppSettingsManager.GetValue("Server.DBPort", "5432");
            FDatabaseName = TAppSettingsManager.GetValue("Server.DBName", "openpetra");
            FDBUsername = TAppSettingsManager.GetValue("Server.DBUserName", "petraserver");
            FDBPassword = TAppSettingsManager.GetValue("Server.DBPassword", string.Empty, false);
            FDBConnectionCheckInterval = TAppSettingsManager.GetInt32("Server.DBConnectionCheckInterval", 0);

            FApplicationBinFolder = TAppSettingsManager.GetValue("Server.ApplicationBinDirectory", string.Empty, false);

            if (FDBPassword == "PG_OPENPETRA_DBPWD")
            {
                // get the password from the file ~/.pgpass. This currently only works for PostgreSQL on Linux
                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                           Path.DirectorySeparatorChar + ".pgpass"))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.StartsWith(FDatabaseHostOrFile + ":" + FDatabasePort + ":" + FDatabaseName + ":" + FDBUsername + ":")
                            || line.StartsWith("*:" + FDatabasePort + ":" + FDatabaseName + ":" + FDBUsername + ":"))
                        {
                            FDBPassword = line.Substring(line.LastIndexOf(':') + 1);
                            break;
                        }
                    }
                }
            }

            if (TAppSettingsManager.HasValue("Server.LogFile"))
            {
                FServerLogFile = TAppSettingsManager.GetValue("Server.LogFile", false);
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

            // Server.Port
            FIPBasePort = TAppSettingsManager.GetInt16("Server.Port", 9000);

            FRunAsStandalone = TAppSettingsManager.GetBoolean("Server.RunAsStandalone", false);

            // Server.ClientIdleStatusAfterXMinutes
            FClientIdleStatusAfterXMinutes = TAppSettingsManager.GetInt32("Server.ClientIdleStatusAfterXMinutes", 5);

            // Server.ClientKeepAliveCheckIntervalInSeconds
            FClientKeepAliveCheckIntervalInSeconds = TAppSettingsManager.GetInt32("Server.ClientKeepAliveCheckIntervalInSeconds", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_LAN
            FClientKeepAliveTimeoutAfterXSecondsLAN = TAppSettingsManager.GetInt32("Server.ClientKeepAliveTimeoutAfterXSeconds_LAN", 60);

            // Server.ClientKeepAliveTimeoutAfterXSeconds_Remote
            FClientKeepAliveTimeoutAfterXSecondsRemote =
                TAppSettingsManager.GetInt32("Server.ClientKeepAliveTimeoutAfterXSeconds_Remote", (ClientKeepAliveTimeoutAfterXSecondsLAN * 2));

            // Server.ClientConnectionTimeoutAfterXSeconds
            FClientConnectionTimeoutAfterXSeconds = TAppSettingsManager.GetInt32("Server.ClientConnectionTimeoutAfterXSeconds", 20);

            // Server.ClientAppDomainShutdownAfterKeepAliveTimeout
            FClientAppDomainShutdownAfterKeepAliveTimeout = TAppSettingsManager.GetBoolean("Server.ClientAppDomainShutdownAfterKeepAliveTimeout",
                true);

            FSmtpHost = TAppSettingsManager.GetValue("SmtpHost", "");
            FSmtpPort = TAppSettingsManager.GetInt32("SmtpPort", 25);
            FSmtpUser = TAppSettingsManager.GetValue("SmtpUser", "YourSmtpUser");
            FSmtpPassword = TAppSettingsManager.GetValue("SmtpPassword", "YourSmtpPassword");
            FSmtpEnableSsl = TAppSettingsManager.GetBoolean("SmtpEnableSsl", true);
            FSmtpAuthenticationType = TAppSettingsManager.GetValue("SmtpAuthenticationType", "config").ToLower();
            FSmtpIgnoreServerCertificateValidation = TAppSettingsManager.GetBoolean("IgnoreServerCertificateValidation", false);

            // Determine network configuration of the Server
            Networking.DetermineNetworkConfig(out FHostName, out FHostIPAddresses);

            FApplicationVersion = TFileVersionInfo.GetApplicationVersion();
        }
    }
}