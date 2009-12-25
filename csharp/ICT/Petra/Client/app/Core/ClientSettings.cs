/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Holds read-only Client settings (from .NET Configuration File and Command
    /// Line). These settings are determined once when the Constructor is executed.
    ///
    /// </summary>
    public class TClientSettings
    {
        private static String UConfigurationFile = "";
        private static String UPathTemp = "";
        private static String UBehaviourSeveralClients = "";
        private static Boolean UDelayedDataLoading = false;
        private static String UReportingPathReportSettings = "";
        private static Int16 UServerPollIntervalInSeconds = 0;
        private static Int16 UServerObjectKeepAliveIntervalInSeconds = 0;
        private static String URemoteDataDirectory = "";
        private static String URemoteTmpDirectory = "";
        private static Boolean URunAsStandalone = false;
        private static Boolean URunAsRemote = false;
        private static Boolean UGUIRunningOnNonStandardDPI = false;
        private static String UPetraServerAdmin_Configfile = "";
        private static String UPetraServer_Configfile = "";
        private static String UPetra_Path_Bin = "";
        private static String UPetra_Path_DB = "";
        private static String UPetra_Path_Dat = "";
        private static String UPetra_Path_Patches = "";
        private static String UPetra_Path_RemotePatches = "";
        private static String UCustomStartupMessage = "";
        private static String UPostgreSql_BaseDir = "";
        private static String UPostgreSql_DataDir = "";
        private static String UPetraWebsite_Link = "";
        private static String UPetraPatches_Link = "";

        /// <summary>Name of .NET Configuration File, if specified via command line options</summary>
        public static String ConfigurationFile
        {
            get
            {
                return UConfigurationFile;
            }
        }

        /// <summary>Temp Path (eg. for storing the Log File)</summary>
        public static String PathTemp
        {
            get
            {
                return UPathTemp;
            }
        }

        /// <summary>should it be allowed to have several clients running at the same time, or should the user be asked</summary>
        public static String BehaviourSeveralClients
        {
            get
            {
                return UBehaviourSeveralClients;
            }
        }

        /// <summary>Delayed data loading (should be true for remote connections)</summary>
        public static Boolean DelayedDataLoading
        {
            get
            {
                return UDelayedDataLoading;
            }
        }

        /// <summary>the path for the report settings in the data directory</summary>
        public static String ReportingPathReportSettings
        {
            get
            {
                return UReportingPathReportSettings;
            }
        }

        /// <summary>The interval in seconds in which the PetraClient checks for ClientTasks</summary>
        public static System.Int16 ServerPollIntervalInSeconds
        {
            get
            {
                return UServerPollIntervalInSeconds;
            }
        }

        /// <summary>The interval in seconds in which the PetraClient keeps the remoted Objects on the PetraServer alive</summary>
        public static System.Int16 ServerObjectKeepAliveIntervalInSeconds
        {
            get
            {
                return UServerObjectKeepAliveIntervalInSeconds;
            }
        }

        /// <summary>should the server be started by the client?</summary>
        public static Boolean RunAsStandalone
        {
            get
            {
                return URunAsStandalone;
            }
        }

        /// <summary>todoComment</summary>
        public static Boolean RunAsRemote
        {
            get
            {
                return URunAsRemote;
            }
        }

        /// <summary>todoComment</summary>
        public static String PetraServerAdmin_Configfile
        {
            get
            {
                return UPetraServerAdmin_Configfile;
            }
        }

        /// <summary>todoComment</summary>
        public static String PetraServer_Configfile
        {
            get
            {
                return UPetraServer_Configfile;
            }
        }

        /// <summary>the directory of the delphi executables and dll files</summary>
        public static String Petra_Path_Bin
        {
            get
            {
                return UPetra_Path_Bin;
            }
        }

        /// <summary>the location of the petra database, that is used for starting the standalone ODBC server</summary>
        public static String Petra_Path_DB
        {
            get
            {
                return UPetra_Path_DB;
            }
        }

        /// <summary>the directory that contains the directories Reportsettings, accounts, etc.</summary>
        public static String Petra_Path_Dat
        {
            get
            {
                return UPetra_Path_Dat;
            }
        }

        /// <summary>the local directory where the patches are installed by InnoSetup</summary>
        public static String Petra_Path_Patches
        {
            get
            {
                return UPetra_Path_Patches;
            }
        }

        /// <summary>the remote directory where the patches are installed by InnoSetup</summary>
        public static String Petra_Path_RemotePatches
        {
            get
            {
                return UPetra_Path_RemotePatches;
            }
        }

        /// <summary>the PostgreSql installation directory</summary>
        public static String PostgreSql_BaseDir
        {
            get
            {
                return UPostgreSql_BaseDir;
            }
        }

        /// <summary>the database directory for the postgreSql server</summary>
        public static String PostgreSql_DataDir
        {
            get
            {
                return UPostgreSql_DataDir;
            }
        }


        /// <summary>false if PetraClient is running on Standard DPI (96 DPI, Normal font size), otherwise true. This gets set from MainWindow.pas!</summary>
        public static Boolean GUIRunningOnNonStandardDPI
        {
            get
            {
                return UGUIRunningOnNonStandardDPI;
            }

            set
            {
                UGUIRunningOnNonStandardDPI = value;
            }
        }

        /// <summary>todoComment</summary>
        public static String CustomStartupMessage
        {
            get
            {
                return UCustomStartupMessage;
            }
        }

        /// <summary>Link to the petra web site</summary>
        public static String PetraWebSite
        {
            get
            {
                return UPetraWebsite_Link;
            }
        }

        /// <summary>Link to the site of petra patches</summary>
        public static String PetraPatches
        {
            get
            {
                return UPetraPatches_Link;
            }
        }

        /// <summary>
        /// Loads settings from .NET Configuration File and Command Line.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TClientSettings() : base()
        {
            TCmdOpts FCmdOptions = new TCmdOpts();
            TAppSettingsManager FAppSettings = new TAppSettingsManager();
            string hostname;

            //
            // Parse settings from the Command Line
            //
            if (FCmdOptions.IsFlagSet("C"))
            {
                UConfigurationFile = FCmdOptions.GetOptValue("C");
            }
            else
            {
                UConfigurationFile = TAppSettingsManager.ConfigFileName;
            }

            //
            // Parse settings from the Application Configuration File
            //
            UPathTemp = FAppSettings.GetValue("OpenPetra.PathTemp");

            if (UPathTemp.Contains("{userappdata}"))
            {
                // on Windows, we cannot store the database in userappdata during installation, because
                // the setup has to be run as administrator.
                // therefore the first time the user starts Petra, we need to prepare his environment
                // see also http://www.vincenzo.net/isxkb/index.php?title=Vista_considerations#Best_Practices
                UPathTemp = UPathTemp.Replace("{userappdata}",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

                if (!Directory.Exists(Path.GetDirectoryName(UPathTemp)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(UPathTemp));
                }
            }

            UBehaviourSeveralClients = "OnlyOneWithQuestion";

            if (FAppSettings.HasValue("BehaviourSeveralClients"))
            {
                UBehaviourSeveralClients = FAppSettings.GetValue("BehaviourSeveralClients");
            }

            UDelayedDataLoading = FAppSettings.GetBoolean("DelayedDataLoading", false);
            UReportingPathReportSettings = FAppSettings.GetValue("Reporting.PathReportSettings");

            UServerPollIntervalInSeconds = FAppSettings.GetInt16("ServerPollIntervalInSeconds", 5);
            UServerObjectKeepAliveIntervalInSeconds = FAppSettings.GetInt16("ServerObjectKeepAliveIntervalInSeconds", 10);

            URemoteDataDirectory = FAppSettings.GetValue("RemoteDataDirectory");
            URemoteTmpDirectory = FAppSettings.GetValue("RemoteTmpDirectory");

            URunAsStandalone = FAppSettings.GetBoolean("RunAsStandalone", false);
            URunAsRemote = FAppSettings.GetBoolean("RunAsRemote", false);
            UPetra_Path_RemotePatches = "";
            UPetra_Path_Dat = "";
            UPetra_Path_Patches = "";
            UPetraWebsite_Link = FAppSettings.GetValue("Petra.Website", "http://www.ict-software.org/petra/index.php");
            UPetraPatches_Link = FAppSettings.GetValue("Petra.PatchesSite", "http://www.ict-software.org/petra/index.php?page=PetraPatches");

            if (URunAsStandalone == true)
            {
                UPetraServerAdmin_Configfile = FAppSettings.GetValue("PetraServerAdmin.Configfile");
                UPetraServer_Configfile = FAppSettings.GetValue("PetraServer.Configfile");
                UPetra_Path_Bin = Environment.CurrentDirectory;
                UPetra_Path_DB = FAppSettings.GetValue("Petra.Path.db");
                UPetra_Path_Patches = UPetra_Path_Bin + Path.DirectorySeparatorChar + "sa-patches";
                UPostgreSql_BaseDir = FAppSettings.GetValue("PostgreSQLServer.BaseDirectory");
                UPostgreSql_DataDir = FAppSettings.GetValue("PostgreSQLServer.DataDirectory");
            }
            else
            {
                // that is needed for the dynamic loading of reports; sometimes the current directory changes, and we need to know
                // where the dlls are
                UPetra_Path_Bin = Environment.CurrentDirectory;
            }

            if (URunAsRemote == true)
            {
                UPetra_Path_Patches = FAppSettings.GetValue("Petra.Path.Patches");
                UPetra_Path_Dat = FAppSettings.GetValue("Petra.Path.Dat");
                UPetra_Path_RemotePatches = FAppSettings.GetValue("Petra.Path.RemotePatches");

                // check whether the config file refers to http: or samba directory
                // or should we check for http anyways?
                if ((!UPetra_Path_RemotePatches.ToLower().StartsWith("http://")
                     && UPetra_Path_RemotePatches.StartsWith("\\\\")))
                {
                    // expect the path to start like this: "\\LINUX\"
                    hostname = UPetra_Path_RemotePatches.Substring(2,
                        UPetra_Path_RemotePatches.Substring(2).IndexOf("\\"));

/* TODO
 *                  if (TPatchTools.ReadWebsite("http://" + hostname + "/PetraNetPatches/").Length > 0)
 *                  {
 *                      UPetra_Path_RemotePatches = "http://" + hostname + "/PetraNetPatches/";
 *                  }
 */
                }
            }

            if ((!URunAsRemote) && (!URunAsStandalone))
            {
                // network version
                UPetra_Path_Patches = UPetra_Path_Bin + Path.DirectorySeparatorChar + "net-patches";
            }

            if (FCmdOptions.IsFlagSet("StartupMessage"))
            {
                UCustomStartupMessage = FCmdOptions.GetOptValue("StartupMessage");
            }
        }
    }
}