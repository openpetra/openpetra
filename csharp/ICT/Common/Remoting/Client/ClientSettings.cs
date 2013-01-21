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
using System.IO;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Holds read-only Client settings (from .NET Configuration File and Command
    /// Line). These settings are determined once when the Constructor is executed.
    ///
    /// </summary>
    public class TClientSettings
    {
        private static String UPathTemp = "";
        private static String UPathLog = "";
        private static Int16 UDebugLevel = 0;
        private static String UBehaviourSeveralClients = "";
        private static Boolean UDelayedDataLoading = false;
        private static String UReportingPathReportSettings = "";
        private static String UReportingPathReportUserSettings = "";
        private static Int32 UServerPollIntervalInSeconds = 0;
        private static Int32 UServerObjectKeepAliveIntervalInSeconds = 0;
        private static String URemoteDataDirectory = "";
        private static String URemoteTmpDirectory = "";
        private static Boolean URunAsStandalone = false;
        private static Boolean URunAsRemote = false;
        private static Boolean UGUIRunningOnNonStandardDPI = false;
        private static String UPetraServerAdmin_Configfile = "";
        private static String UPetraServer_Configfile = "";
        private static String UPetra_Path_Dat = "";
        private static String UPetra_Path_Patches = "";
        private static String UPetra_Path_RemotePatches = "";
        private static String UCustomStartupMessage = "";
        private static String UPostgreSql_BaseDir = "";
        private static String UPostgreSql_DataDir = "";
        private static String UPetraWebsite_Link = "";
        private static String UPetraPatches_Link = "";
        private static String UPetraSupportTeamEmail = "";
        private static String UHTMLHelpBaseURLLocal = "";
        private static String UHTMLHelpBaseURLOnInternet = "";
        private static bool ULocalHTMLHelp = true;

        /// <summary>
        /// DebugLevel for writing an xml file of reporting parameters and results to the log directory
        /// </summary>
        public static Int16 DEBUGLEVEL_REPORTINGDATA = 4;

        /// <summary>Temp Path</summary>
        public static String PathTemp
        {
            get
            {
                return UPathTemp;
            }
        }

        /// <summary>Log Path (eg. for storing the Log File)</summary>
        public static String PathLog
        {
            get
            {
                return UPathLog;
            }
        }

        /// <summary>DebugLevel for TLogging</summary>
        public static Int16 DebugLevel
        {
            get
            {
                return UDebugLevel;
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

        /// <summary>the path for the report settings in the data directory that are written by the user</summary>
        public static String ReportingPathReportUserSettings
        {
            get
            {
                return UReportingPathReportUserSettings;
            }
        }

        /// <summary>The interval in seconds in which the PetraClient checks for ClientTasks</summary>
        public static System.Int32 ServerPollIntervalInSeconds
        {
            get
            {
                return UServerPollIntervalInSeconds;
            }
        }

        /// <summary>The interval in seconds in which the PetraClient keeps the remoted Objects on the PetraServer alive</summary>
        public static System.Int32 ServerObjectKeepAliveIntervalInSeconds
        {
            get
            {
                return UServerObjectKeepAliveIntervalInSeconds;
            }
        }

        /// <summary>the data directory to be used if we want to share the data directory with other users, on a network drive</summary>
        public static string RemoteDataDirectory
        {
            get
            {
                return URemoteDataDirectory;
            }
        }

        /// <summary>the tmp directory to be used if we want to share the tmp directory with other users, on a network drive</summary>
        public static string RemoteTmpDirectory
        {
            get
            {
                return URemoteTmpDirectory;
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
                return TAppSettingsManager.ApplicationDirectory;
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

        /// <summary>Email address of the OpenPetra support team of an organisation</summary>
        public static string PetraSupportTeamEmail
        {
            get
            {
                return UPetraSupportTeamEmail;
            }
        }

        /// <summary>Base URL for the HTMLHelp for the case where it is running off local files.</summary>
        public static string HTMLHelpBaseURLLocal
        {
            get
            {
                return UHTMLHelpBaseURLLocal;
            }
        }

        /// <summary>Base URL for the HTMLHelp for the case where it is running off local files.</summary>
        public static string HTMLHelpBaseURLOnInternet
        {
            get
            {
                return UHTMLHelpBaseURLOnInternet;
            }
        }

        /// <summary>Tells whether the HTMLHelp is to be run off local files, or from an Internet location.</summary>
        public static bool LocalHTMLHelp
        {
            get
            {
                return ULocalHTMLHelp;
            }
        }

        private static string GetUserPath(string AVariableName, string ADefaultValue)
        {
            string result = TAppSettingsManager.GetValue(AVariableName, ADefaultValue);

            if (result.Length == 0)
            {
                return result;
            }

            // on Windows, we cannot store the database in userappdata during installation, because
            // the setup has to be run as administrator.
            // therefore the first time the user starts Petra, we need to prepare his environment
            // see also http://www.vincenzo.net/isxkb/index.php?title=Vista_considerations#Best_Practices
            TFileHelper.CreateDirectory(result);

            return Path.GetFullPath(result);
        }

        /// get temp path in the user directory. this is called from PetraClientMain directly
        public static string GetPathLog()
        {
            UPathTemp = GetUserPath("OpenPetra.PathTemp", Path.GetTempPath());

            UPathLog = GetUserPath("OpenPetra.PathLog", UPathTemp);

            return UPathLog;
        }

        /// get export path in the user directory. used eg. by GL Batch or Gift Batch export
        /// will create the directory if it does not exist yet.
        public static string GetExportPath()
        {
            string ExportPath = GetUserPath("OpenPetra.PathExport",
                GetUserPath("OpenPetra.PathTemp", "") + Path.DirectorySeparatorChar + "export");

            TFileHelper.CreateDirectory(ExportPath);

            return ExportPath;
        }

        /// <summary>
        /// Loads settings from .NET Configuration File and Command Line.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TClientSettings() : base()
        {
            //
            // Parse settings from the Application Configuration File
            //
            UPathLog = GetPathLog();

            UDebugLevel = TAppSettingsManager.GetInt16("Client.DebugLevel", 0);
            TLogging.DebugLevel = UDebugLevel;

            UBehaviourSeveralClients = "OnlyOneWithQuestion";

            if (TAppSettingsManager.HasValue("BehaviourSeveralClients"))
            {
                UBehaviourSeveralClients = TAppSettingsManager.GetValue("BehaviourSeveralClients");
            }

            UDelayedDataLoading = TAppSettingsManager.GetBoolean("DelayedDataLoading", false);
            UReportingPathReportSettings = GetUserPath("Reporting.PathReportSettings", "");
            UReportingPathReportUserSettings = GetUserPath("Reporting.PathReportUserSettings", "");

            UServerPollIntervalInSeconds = TAppSettingsManager.GetInt32("ServerPollIntervalInSeconds", 5);
            UServerObjectKeepAliveIntervalInSeconds = TAppSettingsManager.GetInt32("ServerObjectKeepAliveIntervalInSeconds", 10);

            URemoteDataDirectory = TAppSettingsManager.GetValue("RemoteDataDirectory");
            URemoteTmpDirectory = TAppSettingsManager.GetValue("RemoteTmpDirectory");

            URunAsStandalone = TAppSettingsManager.GetBoolean("RunAsStandalone", false);
            URunAsRemote = TAppSettingsManager.GetBoolean("RunAsRemote", false);
            UPetra_Path_RemotePatches = "";
            UPetra_Path_Dat = "";
            UPetra_Path_Patches = "";
            UPetraWebsite_Link = TAppSettingsManager.GetValue("OpenPetra.Website", "http://www.openpetra.org");
            UPetraPatches_Link = TAppSettingsManager.GetValue("OpenPetra.Path.RemotePatches",
                "http://www.example.org/index.php?page=OpenPetraPatches");
            UPetraSupportTeamEmail = TAppSettingsManager.GetValue("OpenPetra.SupportTeamEmail", String.Empty);

            if (URunAsStandalone == true)
            {
                UPetraServerAdmin_Configfile = TAppSettingsManager.GetValue("PetraServerAdmin.Configfile");
                UPetraServer_Configfile = TAppSettingsManager.GetValue("PetraServer.Configfile");
                UPetra_Path_Patches = Petra_Path_Bin + Path.DirectorySeparatorChar + "sa-patches";
                UPostgreSql_BaseDir = TAppSettingsManager.GetValue("PostgreSQLServer.BaseDirectory");
                UPostgreSql_DataDir = TAppSettingsManager.GetValue("PostgreSQLServer.DataDirectory");
            }

            if (URunAsRemote == true)
            {
                UPetra_Path_Patches = GetUserPath("OpenPetra.Path.Patches", "");
                UPetra_Path_Dat = GetUserPath("OpenPetra.Path.Dat", "");
                UPetra_Path_RemotePatches = TAppSettingsManager.GetValue("OpenPetra.Path.RemotePatches");
            }

            if ((!URunAsRemote) && (!URunAsStandalone))
            {
                // network version
                UPetra_Path_Patches = Petra_Path_Bin + Path.DirectorySeparatorChar + "net-patches";
            }

            if (TAppSettingsManager.HasValue("StartupMessage"))
            {
                UCustomStartupMessage = TAppSettingsManager.GetValue("StartupMessage");
            }

            UHTMLHelpBaseURLLocal = TAppSettingsManager.GetValue("HTMLHelpBaseURLLocal", String.Empty);
            UHTMLHelpBaseURLOnInternet = TAppSettingsManager.GetValue("HTMLHelpBaseURLOnInternet", String.Empty);
            ULocalHTMLHelp = TAppSettingsManager.GetBoolean("LocalHTMLHelp", true);
        }
    }
}