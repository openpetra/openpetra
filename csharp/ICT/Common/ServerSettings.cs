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
// it under the terms of the GNU General public static License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General public static License for more details.
//
// You should have received a copy of the GNU General public static License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;

using Ict.Common;

namespace Ict.Common
{
    /// <summary>
    /// Class for storing Server settings. Once instantiated, Server settings
    /// can only be read!
    /// Server Settings are gathered from the Command line, .NET Configuration files
    /// and other ways (eg. determining the OS on which the server is running
    /// on-the-fly) at Server start-up.
    ///
    /// </summary>
    public class TSrvSetting
    {
        private String FConfigurationFile;
        private TDBType FRDBMSType;
        private String FServerLogFile;
        private String FHostName;
        private String FHostIPAddresses;
        private TFileVersionInfo FApplicationVersion;
        private System.Int16 FIPBasePort;
        private TExecutingOSEnum FExecutingOS;
        private String FApplicationBinFolder;

        #region Properties
        /// <summary>Path and name of .NET Configuration File (e.g. specified via command line option '-C').</summary>
        public static String ConfigurationFile
        {
            get
            {
                return new TSrvSetting().FConfigurationFile;
            }
        }

        /// <summary>Assembly Version of the Server's .exe</summary>
        public static TFileVersionInfo ApplicationVersion
        {
            get
            {
                return new TSrvSetting().FApplicationVersion;
            }
        }

        /// <summary>Operating System the Server is running on</summary>
        public static TExecutingOSEnum ExecutingOS
        {
            get
            {
                return new TSrvSetting().FExecutingOS;
            }
        }

        /// <summary>Type of RDBMS (Relational Database Management System) that the Server is connected to</summary>
        public static TDBType RDMBSType
        {
            get
            {
                return new TSrvSetting().FRDBMSType;
            }
        }

        /// <summary>Computer name of the Server</summary>
        public static String HostName
        {
            get
            {
                return new TSrvSetting().FHostName;
            }
        }

        /// <summary>IP Address(es) of the Server</summary>
        public static String HostIPAddresses
        {
            get
            {
                return new TSrvSetting().FHostIPAddresses;
            }
        }

        /// <summary>IP Address at which the Server is listening for Client connection/disconnection requests</summary>
        public static System.Int16 IPBasePort
        {
            get
            {
                return new TSrvSetting().FIPBasePort;
            }
        }

        /// <summary>This is the path to the server log file</summary>
        public static String ServerLogFile
        {
            get
            {
                return new TSrvSetting().FServerLogFile;
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
                return new TSrvSetting().FApplicationBinFolder;
            }
        }

        #endregion

        /// <summary>
        /// Initialises the internal variables that hold the Server Settings, using the current config file.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TSrvSetting()
        {
            FConfigurationFile = TAppSettingsManager.ConfigFileName;
            FExecutingOS = Utilities.DetermineExecutingOS();

            // Server.RDBMSType
            FRDBMSType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType", "postgresql"));

            FApplicationBinFolder = TAppSettingsManager.GetValue("Server.ApplicationBinDirectory", string.Empty, false);

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
            FIPBasePort = TAppSettingsManager.GetInt16("Server.Port", 80);

            // Determine network configuration of the Server
            Networking.DetermineNetworkConfig(out FHostName, out FHostIPAddresses);

            FApplicationVersion = TFileVersionInfo.GetApplicationVersion();
        }
    }
}
