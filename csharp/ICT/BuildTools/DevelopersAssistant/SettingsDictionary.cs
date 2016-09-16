//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// A class that maintains local settings between user sessions by reading and writing the settings to a file.
    /// It uses its own simple text-based serialisation
    /// </summary>
    public class SettingsDictionary : SettingsDictionaryBase
    {
        /// <summary>
        /// Sets the header content that is written to the settings file
        /// </summary>
        public string ContentHeader {
            private get; set;
        }

        /// <summary>
        /// The alternate multi-task sequence
        /// </summary>
        public string AltSequence {
            get; set;
        }

        /// <summary>
        /// The path to bzrw.exe that runs Bazaar
        /// </summary>
        public string BazaarPath {
            get; set;
        }

        /// <summary>
        /// The path to the current branch folder
        /// </summary>
        public string BranchLocation {
            get; set;
        }

        /// <summary>
        /// The delimited list of my preferred database build configurations
        /// </summary>
        public string DbBuildConfigurations {
            get; set;
        }

        /// <summary>
        /// The multi-task sequence
        /// </summary>
        public string Sequence {
            get; set;
        }

        /// <summary>
        /// The window position
        /// </summary>
        public string WindowPosition {
            get; set;
        }

        /// <summary>
        /// A comma-separated list of YAML files
        /// </summary>
        public string YAMLLocationHistory {
            get; set;
        }


        /// <summary>
        /// The selected index for the code generation combo box
        /// </summary>
        public int CodeGenerationComboID {
            get; set;
        }

        /// <summary>
        /// The selected index for the compilation combo box
        /// </summary>
        public int CompilationComboID {
            get; set;
        }

        /// <summary>
        /// The selected index for the miscellaneous combo box
        /// </summary>
        public int MiscellaneousComboID {
            get; set;
        }

        /// <summary>
        /// The selected index for the source code combo box
        /// </summary>
        public int SourceCodeComboID {
            get; set;
        }

        /// <summary>
        /// The selected index for the database combo box
        /// </summary>
        public int DatabaseComboID {
            get; set;
        }

        /// <summary>
        /// The number of seconds a task must run for before flashing the application window on completeion of a task
        /// </summary>
        public uint FlashAfterSeconds {
            get; set;
        }


        /// <summary>
        /// True if the Assistant should automatically start the server if the current task needs it to run
        /// </summary>
        public bool AutoStartServer {
            get; set;
        }

        /// <summary>
        /// True if the Assistant should stop the server if the current task requires it to be stopped
        /// </summary>
        public bool AutoStopServer {
            get; set;
        }

        /// <summary>
        /// True if the Assistant should start the server minimised
        /// </summary>
        public bool MinimiseServerAtStartup {
            get; set;
        }

        /// <summary>
        /// True if the Assistant should show the Output tab if the task completes successfully but with warnings/errors
        /// </summary>
        public bool TreatWarningsAsErrors {
            get; set;
        }

        /// <summary>
        /// True if the code generator should add a pre-build action to the Ict.Common dll
        /// </summary>
        public bool DoPreBuildOnIctCommon {
            get; set;
        }

        /// <summary>
        /// True if the code generator should add a post-build action to PetraClient exe
        /// </summary>
        public bool DoPostBuildOnPetraClient {
            get; set;
        }

        /// <summary>
        /// True if nant should compile the code that has been generated as a Winform
        /// </summary>
        public bool CompileWinForm {
            get; set;
        }

        /// <summary>
        /// True if nant should start the client after compiling the code that has been generated for a Winform
        /// </summary>
        public bool StartClientAfterCompileWinForm {
            get; set;
        }

        /// <summary>
        /// Set to true if the application should check for updates when it starts up
        /// </summary>
        public bool AutoCheckForUpdates {
            get; set;
        }

        /// <summary>
        /// Gets/Sets the user name on Launchpad for the current user
        /// </summary>
        public string LaunchpadUserName {
            get; set;
        }

        /// <summary>
        /// Gets/sets the generate solution option (for New Branch Actions) (0=none, 1=full, 2=minimal)
        /// </summary>
        public int NBA_GenerateSolutionOption
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to create config files (for New Branch Actions)
        /// </summary>
        public bool NBA_CreateMyConfigurations
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to initialise the database (for New Branch Actions)
        /// </summary>
        public bool NBA_InitialiseDatabase
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets the db configuration to use (for New Branch Actions)
        /// </summary>
        public int NBA_DatabaseConfiguration
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to launch the IDE (for New Branch Actions)
        /// </summary>
        public bool NBA_LaunchIDE
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets the solution to launch in the IDE (for New Branch Actions)
        /// </summary>
        public int NBA_IDESolution
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to launch the client (for New Branch Actions)
        /// </summary>
        public bool NBA_StartClient
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets the generate solution option (for Existing Branch Actions) (0=none, 1=full, 2=minimal)
        /// </summary>
        public int EBA_GenerateSolutionOption
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to create config files (for Existing Branch Actions)
        /// </summary>
        public bool EBA_CreateMyConfigurations
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to initialise the database (Existing New Branch Actions)
        /// </summary>
        public bool EBA_InitialiseDatabase
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets the db configuration to use (for Existing Branch Actions)
        /// </summary>
        public int EBA_DatabaseConfiguration
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to launch the IDE (for Existing Branch Actions)
        /// </summary>
        public bool EBA_LaunchIDE
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets the solution to launch in the IDE (for Existing Branch Actions)
        /// </summary>
        public int EBA_IDESolution
        {
            get; set;
        }

        /// <summary>
        /// Gets/sets whether to launch the client (for Existing Branch Actions)
        /// </summary>
        public bool EBA_StartClient
        {
            get; set;
        }

        /// <summary>
        /// Gets the branch history item at the specified index
        /// </summary>
        /// <param name="Index">A number between 1 and BRANCH_HISTORY_SIZE indicating the history item to fetch</param>
        /// <returns>Full path to the specified historical branch location</returns>
        public string GetBranchHistory(int Index)
        {
            if ((Index < 1) || (Index > BRANCH_HISTORY_SIZE))
            {
                return String.Empty;
            }

            return this["BranchHistory" + Index.ToString()];
        }

        /// <summary>
        /// Sets the branch history item at the specified index
        /// </summary>
        /// <param name="Index">A number between 1 and BRANCH_HISTORY_SIZE indicating the history item to set</param>
        /// <param name="value">Full path to the specified historical branch location</param>
        public void SetBranchHistoryItem(int Index, string value)
        {
            if ((Index < 1) || (Index > BRANCH_HISTORY_SIZE))
            {
                return;
            }

            string key = "BranchHistory" + Index.ToString();

            if (this.ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                this.Add(key, value);
            }
        }

        // Private members
        private string _path;       // path to local settings file
        private string _applicationVersion;
        private const int BRANCH_HISTORY_SIZE = 9;


        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="path">Full path to the local settings file that will be read and written</param>
        /// <param name="ApplicationVersion">The version of the application hosting this file, eg 1.0.0.100</param>
        public SettingsDictionary(string path, string ApplicationVersion)
        {
            _path = path;
            _applicationVersion = ApplicationVersion;

            // Initialise all default values of our public properties in the constructor
            AltSequence = String.Empty;
            BazaarPath = String.Empty;
            BranchLocation = String.Empty;
            DbBuildConfigurations = String.Empty;
            LaunchpadUserName = String.Empty;
            Sequence = String.Empty;
            WindowPosition = String.Empty;
            YAMLLocationHistory = String.Empty;

            CodeGenerationComboID = 2;
            CompilationComboID = 2;
            MiscellaneousComboID = 0;
            SourceCodeComboID = 13;
            DatabaseComboID = 1;
            FlashAfterSeconds = 15;

            NBA_GenerateSolutionOption = 1;
            NBA_CreateMyConfigurations = true;
            NBA_InitialiseDatabase = true;
            NBA_DatabaseConfiguration = 0;
            NBA_LaunchIDE = true;
            NBA_IDESolution = 0;
            NBA_StartClient = true;

            EBA_GenerateSolutionOption = 0;
            EBA_CreateMyConfigurations = false;
            EBA_InitialiseDatabase = true;
            EBA_DatabaseConfiguration = 0;
            EBA_LaunchIDE = true;
            EBA_IDESolution = 0;
            EBA_StartClient = true;

            AutoStartServer = true;
            AutoStopServer = true;
            MinimiseServerAtStartup = true;
            TreatWarningsAsErrors = true;
            DoPreBuildOnIctCommon = false;
            DoPostBuildOnPetraClient = false;
            CompileWinForm = true;
            StartClientAfterCompileWinForm = true;
            AutoCheckForUpdates = true;

            // Add items to our dictionary
            this.Add("AltSequence", AltSequence);
            this.Add("BazaarPath", BazaarPath);
            this.Add("BranchLocation", BranchLocation);
            this.Add("DbBuildConfigurations", DbBuildConfigurations);
            this.Add("LaunchpadUserName", LaunchpadUserName);
            this.Add("Sequence", Sequence);
            this.Add("WindowPosition", WindowPosition);
            this.Add("YAMLLocationHistory", YAMLLocationHistory);

            this.Add("CodeGenerationComboID", CodeGenerationComboID.ToString());
            this.Add("CompilationComboID", CompilationComboID.ToString());
            this.Add("MiscellaneousComboID", MiscellaneousComboID.ToString());
            this.Add("SourceCodeComboID", SourceCodeComboID.ToString());
            this.Add("DatabaseComboID", DatabaseComboID.ToString());
            this.Add("FlashAfterSeconds", FlashAfterSeconds.ToString());

            this.Add("NBA_GenerateSolutionOption", NBA_GenerateSolutionOption.ToString());
            this.Add("NBA_CreateMyConfigurations", NBA_CreateMyConfigurations ? "1" : "0");
            this.Add("NBA_InitialiseDatabase", NBA_InitialiseDatabase ? "1" : "0");
            this.Add("NBA_DatabaseConfiguration", NBA_DatabaseConfiguration.ToString());
            this.Add("NBA_LaunchIDE", NBA_LaunchIDE ? "1" : "0");
            this.Add("NBA_IDESolution", NBA_IDESolution.ToString());
            this.Add("NBA_StartClient", NBA_StartClient ? "1" : "0");

            this.Add("EBA_GenerateSolutionOption", EBA_GenerateSolutionOption.ToString());
            this.Add("EBA_CreateMyConfigurations", EBA_CreateMyConfigurations ? "1" : "0");
            this.Add("EBA_InitialiseDatabase", EBA_InitialiseDatabase ? "1" : "0");
            this.Add("EBA_DatabaseConfiguration", EBA_DatabaseConfiguration.ToString());
            this.Add("EBA_LaunchIDE", EBA_LaunchIDE ? "1" : "0");
            this.Add("EBA_IDESolution", EBA_IDESolution.ToString());
            this.Add("EBA_StartClient", EBA_StartClient ? "1" : "0");

            this.Add("AutoStartServer", AutoStartServer ? "1" : "0");
            this.Add("AutoStopServer", AutoStopServer ? "1" : "0");
            this.Add("MinimiseServerAtStartup", MinimiseServerAtStartup ? "1" : "0");
            this.Add("TreatWarningsAsErrors", TreatWarningsAsErrors ? "1" : "0");
            this.Add("DoPreBuildOnIctCommon", DoPreBuildOnIctCommon ? "1" : "0");
            this.Add("DoPostBuildOnPetraClient", DoPostBuildOnPetraClient ? "1" : "0");
            this.Add("CompileWinForm", CompileWinForm ? "1" : "0");
            this.Add("StartClientAfterCompileWinForm", StartClientAfterCompileWinForm ? "1" : "0");
            this.Add("AutoCheckForUpdates", AutoCheckForUpdates ? "1" : "0");

            for (int i = 1; i < 10; i++)
            {
                SetBranchHistoryItem(i, String.Empty);
            }
        }

        /// <summary>
        /// Loads settings from the file specified in the constructor
        /// </summary>
        public void Load()
        {
            // Call our base method that reads the file and fills the dictionary
            base.Load(_path);

            // After a load we set the latest values of each of our known properties
            AltSequence = this["AltSequence"];
            BazaarPath = this["BazaarPath"];
            BranchLocation = this["BranchLocation"];
            DbBuildConfigurations = this["DbBuildConfigurations"];
            LaunchpadUserName = this["LaunchpadUserName"];
            Sequence = this["Sequence"];
            WindowPosition = this["WindowPosition"];
            YAMLLocationHistory = this["YAMLLocationHistory"];

            CodeGenerationComboID = Convert.ToInt32(this["CodeGenerationComboID"]);
            CompilationComboID = Convert.ToInt32(this["CompilationComboID"]);
            MiscellaneousComboID = Convert.ToInt32(this["MiscellaneousComboID"]);
            SourceCodeComboID = Convert.ToInt32(this["SourceCodeComboID"]);
            DatabaseComboID = Convert.ToInt32(this["DatabaseComboID"]);
            FlashAfterSeconds = Convert.ToUInt32(this["FlashAfterSeconds"]);

            NBA_GenerateSolutionOption = Convert.ToInt32(this["NBA_GenerateSolutionOption"]);
            NBA_CreateMyConfigurations = (this["NBA_CreateMyConfigurations"] != "0");
            NBA_InitialiseDatabase = (this["NBA_InitialiseDatabase"] != "0");
            NBA_DatabaseConfiguration = Convert.ToInt32(this["NBA_DatabaseConfiguration"]);
            NBA_LaunchIDE = (this["NBA_LaunchIDE"] != "0");
            NBA_IDESolution = Convert.ToInt32(this["NBA_IDESolution"]);
            NBA_StartClient = (this["NBA_StartClient"] != "0");

            EBA_GenerateSolutionOption = Convert.ToInt32(this["EBA_GenerateSolutionOption"]);
            EBA_CreateMyConfigurations = (this["EBA_CreateMyConfigurations"] != "0");
            EBA_InitialiseDatabase = (this["EBA_InitialiseDatabase"] != "0");
            EBA_DatabaseConfiguration = Convert.ToInt32(this["EBA_DatabaseConfiguration"]);
            EBA_LaunchIDE = (this["EBA_LaunchIDE"] != "0");
            EBA_IDESolution = Convert.ToInt32(this["EBA_IDESolution"]);
            EBA_StartClient = (this["EBA_StartClient"] != "0");

            AutoStartServer = (this["AutoStartServer"] != "0");
            AutoStopServer = (this["AutoStopServer"] != "0");
            MinimiseServerAtStartup = (this["MinimiseServerAtStartup"] != "0");
            TreatWarningsAsErrors = (this["TreatWarningsAsErrors"] != "0");
            DoPreBuildOnIctCommon = (this["DoPreBuildOnIctCommon"] != "0");
            DoPostBuildOnPetraClient = (this["DoPostBuildOnPetraClient"] != "0");
            CompileWinForm = (this["CompileWinForm"] != "0");
            StartClientAfterCompileWinForm = (this["StartClientAfterCompileWinForm"] != "0");
            AutoCheckForUpdates = (this["AutoCheckForUpdates"] != "0");

            // Non-version-specific updates
            if (this.ContainsKey("YAMLLocation"))
            {
                // We now store YAML History instead of a single YAML Path
                YAMLLocationHistory = this["YAMLLocation"];
                this.Remove("YAMLLocation");
            }

            // Do version-specific upgrades
            if (this.ContainsKey("ApplicationVersion"))
            {
                // This tells us the version of OPDA that saved the ini file we have just read ...
                if (this["ApplicationVersion"].StartsWith("1.0.1."))
                {
                    // We no longer support 'clean' on the compilation combo
                    if (CompilationComboID > 0)
                    {
                        CompilationComboID = CompilationComboID - 1;
                        this["CompilationComboID"] = CompilationComboID.ToString();
                    }
                }

                //  Work out the app version from the string - prefix the string with . so there are 4 dots
                string prevAppVersion = "." + this["ApplicationVersion"];
                int nPrevAppVersion = 0;
                int nMultiplier = 1;

                // Now go through the string separating the four parts
                for (int i = 0; i < 4; i++)
                {
                    int p = prevAppVersion.LastIndexOf('.');
                    nPrevAppVersion += (nMultiplier * Convert.ToInt32(prevAppVersion.Substring(p + 1)));
                    nMultiplier *= 100;
                    prevAppVersion = prevAppVersion.Substring(0, p);
                }

                if ((nPrevAppVersion <= 1000400) && (nPrevAppVersion > 0))
                {
                    // We added client gui tests in position 7 after version 1.0.4.0
                    if (MiscellaneousComboID == 7)
                    {
                        MiscellaneousComboID++;
                        this["CompilationComboID"] = MiscellaneousComboID.ToString();
                    }
                }

                if ((nPrevAppVersion <= 1000500) && (nPrevAppVersion > 0))
                {
                    // We added 'Pull' in position 11 and 'Update' in position 12 after 1.0.5.0
                    if (SourceCodeComboID >= 11)
                    {
                        SourceCodeComboID += 2;
                        this["SourceCodeComboID"] = SourceCodeComboID.ToString();
                    }
                }
            }

            // Set up the OS environment variables
            Environment.SetEnvironmentVariable("OPDA_PATH", System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        /// <summary>
        /// Saves the current settings stored in the internal dictionary
        /// </summary>
        public void Save()
        {
            // Update the dictionary from the public properties
            this["AltSequence"] = AltSequence;
            this["BazaarPath"] = BazaarPath;
            this["BranchLocation"] = BranchLocation;
            this["DbBuildConfigurations"] = DbBuildConfigurations;
            this["LaunchpadUserName"] = LaunchpadUserName;
            this["Sequence"] = Sequence;
            this["WindowPosition"] = WindowPosition;
            this["YAMLLocationHistory"] = YAMLLocationHistory;

            this["CodeGenerationComboID"] = CodeGenerationComboID.ToString();
            this["CompilationComboID"] = CompilationComboID.ToString();
            this["MiscellaneousComboID"] = MiscellaneousComboID.ToString();
            this["SourceCodeComboID"] = SourceCodeComboID.ToString();
            this["DatabaseComboID"] = DatabaseComboID.ToString();
            this["FlashAfterSeconds"] = FlashAfterSeconds.ToString();

            this["NBA_GenerateSolutionOption"] = NBA_GenerateSolutionOption.ToString();
            this["NBA_CreateMyConfigurations"] = NBA_CreateMyConfigurations ? "1" : "0";
            this["NBA_InitialiseDatabase"] = NBA_InitialiseDatabase ? "1" : "0";
            this["NBA_DatabaseConfiguration"] = NBA_DatabaseConfiguration.ToString();
            this["NBA_LaunchIDE"] = NBA_LaunchIDE ? "1" : "0";
            this["NBA_IDESolution"] = NBA_IDESolution.ToString();
            this["NBA_StartClient"] = NBA_StartClient ? "1" : "0";

            this["EBA_GenerateSolutionOption"] = EBA_GenerateSolutionOption.ToString();
            this["EBA_CreateMyConfigurations"] = EBA_CreateMyConfigurations ? "1" : "0";
            this["EBA_InitialiseDatabase"] = EBA_InitialiseDatabase ? "1" : "0";
            this["EBA_DatabaseConfiguration"] = EBA_DatabaseConfiguration.ToString();
            this["EBA_LaunchIDE"] = EBA_LaunchIDE ? "1" : "0";
            this["EBA_IDESolution"] = EBA_IDESolution.ToString();
            this["EBA_StartClient"] = EBA_StartClient ? "1" : "0";

            this["AutoStartServer"] = AutoStartServer ? "1" : "0";
            this["AutoStopServer"] = AutoStopServer ? "1" : "0";
            this["MinimiseServerAtStartup"] = MinimiseServerAtStartup ? "1" : "0";
            this["TreatWarningsAsErrors"] = TreatWarningsAsErrors ? "1" : "0";
            this["DoPreBuildOnIctCommon"] = DoPreBuildOnIctCommon ? "1" : "0";
            this["DoPostBuildOnPetraClient"] = DoPostBuildOnPetraClient ? "1" : "0";
            this["CompileWinForm"] = CompileWinForm ? "1" : "0";
            this["StartClientAfterCompileWinForm"] = StartClientAfterCompileWinForm ? "1" : "0";
            this["AutoCheckForUpdates"] = AutoCheckForUpdates ? "1" : "0";

            // Add our appVersion key/value
            if (!this.ContainsKey("ApplicationVersion"))
            {
                this.Add("ApplicationVersion", _applicationVersion);
            }
            else
            {
                this["ApplicationVersion"] = _applicationVersion;
            }

            // Now do the low-level save of the file
            base.Save(_path, ContentHeader);
        }
    }

    /// <summary>
    /// Class that handles the External Web Links
    /// </summary>
    public class ExternalLinksDictionary : SettingsDictionaryBase
    {
        // Private members
        private string _path;               // path to external link settings file
        private string _contentHeader;      // static content header in file

        private Dictionary <string, string>_defaultLinks = new Dictionary <string, string>();

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="path">Full path to the web links file to be parsed.  If it does not exist, a new one is created containing 'standard' links.</param>
        public ExternalLinksDictionary(string path)
        {
            _path = path;
            _contentHeader = ("; File containing useful OPDA External Links");
            _contentHeader += (Environment.NewLine + "; You can modify this file.  The format for each line is ...");
            _contentHeader += (Environment.NewLine + ";   <Title> = <url> [++ <More_Info>]");
            _contentHeader += (Environment.NewLine + "; The URL can contain an = but must not contain a 'double-plus'");
            _contentHeader += (Environment.NewLine + "# Blank lines and lines that start with ; or # are ignored");
        }

        /// <summary>
        /// Loads settings from the file specified in the constructor
        /// </summary>
        public void Load()
        {
            _defaultLinks = new Dictionary <string, string>();
            _defaultLinks.Add("Database Schema",
                "https://ci.openpetra.org/job/OpenPetraDBDoc/doclinks/1/index.html" + " ++ The complete architecture of the Open Petra database");
            _defaultLinks.Add(
                "Developer's Forum",
                "http://forum.openpetra.org/" +
                " ++ This links to the main developer forum where you can join in discussion of developer topics or ask a question.");
            _defaultLinks.Add("Documentation for Developers",
                "http://www.openpetra.org/en/developers-documentation" + " ++ Useful links from the main public site for Open Petra");
            _defaultLinks.Add("Jenkins Build Server",
                "https://ci.openpetra.org/" + " ++ A link to the main Continuous Integration server that runs on Linux.");
            _defaultLinks.Add("Jenkins Server on Windows",
                "https://ci-win.openpetra.org/" + " ++ A link to the dashboard of the Continuous Integration server that runs on Windows.");
            _defaultLinks.Add("Launchpad",
                "https://code.launchpad.net/openpetraorg/" + " ++ A web interface to the code on the main Launchpad repository");
            _defaultLinks.Add("Doxygen", "http://codedoc.openpetra.org/" + " ++ A javadoc like documentation of the OpenPetra source code");
            _defaultLinks.Add("Mantis Bug Tracker",
                "https://tracker.openpetra.org/main_page.php" + " ++ This links to the main project work item database known as 'Mantis'");
            _defaultLinks.Add(
                "Mantis Bug Tracker (My View)",
                "https://tracker.openpetra.org/my_view_page.php" +
                " ++ This links to the 'My View' page in the main project work item database known as 'Mantis'");
            _defaultLinks.Add("OpenPetra Wiki", "https://wiki.openpetra.org/" + " ++ This links to the main project wiki");
            _defaultLinks.Add("Useful shortcuts",
                "http://www.openpetra.org/en/shortcuts/" + " ++ Many useful shortcuts in one place, including many listed here");

            if (!File.Exists(_path))
            {
                if (!Directory.Exists(Path.GetDirectoryName(_path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_path));
                }

                // Create a default one
                using (StreamWriter sw = new StreamWriter(_path))
                {
                    sw.WriteLine(_contentHeader);
                    sw.WriteLine();

                    foreach (KeyValuePair <string, string>kvp in _defaultLinks)
                    {
                        sw.WriteLine(String.Format("{0} = {1}"), kvp.Key, kvp.Value);
                    }

                    sw.Close();
                }
            }

            // Call our base method that reads the file and fills the dictionary
            base.Load(_path);
        }

        /// <summary>
        /// Save the changes
        /// </summary>
        public void Save()
        {
            base.Save(_path, _contentHeader);
        }

        /// <summary>
        /// Populate a list box using the keys in this dictionary
        /// </summary>
        /// <param name="listBox">The list box to be populated</param>
        public void PopulateListBox(System.Windows.Forms.ListBox listBox)
        {
            object o = listBox.SelectedItem;

            listBox.Items.Clear();

            foreach (KeyValuePair <string, string>kvp in this)
            {
                listBox.Items.Add(kvp.Key);
            }

            if (listBox.Items.Count > 0)
            {
                if (o == null)
                {
                    listBox.SelectedIndex = 0;
                }
                else if (listBox.Items.Contains(o))
                {
                    listBox.SelectedItem = o;
                }
                else
                {
                    listBox.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Get the details for the specified web link key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="url">The URL</param>
        /// <param name="info">The additional info</param>
        public void GetDetails(string key, out string url, out string info)
        {
            if (this.ContainsKey(key) && (this[key] != String.Empty))
            {
                string[] splitter = new string[] {
                    "++"
                };
                string[] items = this[key].Split(splitter, StringSplitOptions.None);
                url = items[0].Trim();

                if (items.Length > 1)
                {
                    info = items[1].TrimStart();
                }
                else
                {
                    info = String.Empty;
                }
            }
            else
            {
                url = String.Empty;
                info = String.Empty;
            }
        }

        /// <summary>
        /// Gets the suggested update at the requested index
        /// </summary>
        /// <param name="RequestedIndex">The update index to search for.  This method should be called with an initial value of 0,
        /// then call again with 1, then 2 and so on until the method returns false.</param>
        /// <param name="ForKey">The key name of the update</param>
        /// <param name="ThisValue">The current value for the key</param>
        /// <param name="SuggestedValue">The suggested value for the key</param>
        /// <returns>Returns true if there is an update at the requested index</returns>
        public bool GetSuggestedUpdate(int RequestedIndex, out string ForKey, out string ThisValue, out string SuggestedValue)
        {
            int foundIndex = -1;

            ForKey = string.Empty;
            ThisValue = string.Empty;
            SuggestedValue = string.Empty;

            foreach (KeyValuePair <string, string>kvp in _defaultLinks)
            {
                // Do we even have this key?
                if (this.ContainsKey(kvp.Key))
                {
                    // We have the key, so how does the value compare?
                    string[] thisValue = this[kvp.Key].Split(new string[] { "++" }, StringSplitOptions.None);
                    string[] suggestedValue = kvp.Value.Split(new string[] { "++" }, StringSplitOptions.None);
                    string thisUrl = thisValue[0].Trim();
                    string suggestedUrl = suggestedValue[0].Trim();

                    if (string.Compare(thisUrl, suggestedUrl, true) != 0)
                    {
                        foundIndex++;

                        if (foundIndex == RequestedIndex)
                        {
                            ForKey = kvp.Key;
                            SuggestedValue = suggestedUrl;
                            ThisValue = thisUrl;
                            return true;
                        }
                    }
                }
                else
                {
                    // We do not have the key
                    foundIndex++;

                    if (foundIndex == RequestedIndex)
                    {
                        ForKey = kvp.Key;
                        string[] suggestedValue = kvp.Value.Split(new string[] { "++" }, StringSplitOptions.None);
                        string suggestedUrl = suggestedValue[0].Trim();
                        SuggestedValue = suggestedUrl;
                        ThisValue = string.Empty;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the specified key to the program application default value.  Call this to update an existing 'out-of-date' Url
        /// </summary>
        /// <param name="KeyName">The key name</param>
        public void SetToDefault(string KeyName)
        {
            if (this.ContainsKey(KeyName))
            {
                this[KeyName] = _defaultLinks[KeyName];
            }
            else
            {
                this.Add(KeyName, _defaultLinks[KeyName]);
            }
        }
    }

    /// <summary>
    /// A base class for OPDA settings
    /// </summary>
    public class SettingsDictionaryBase : SortedDictionary <string, string>
    {
        /// <summary>
        /// The standard Save method to save the file
        /// </summary>
        /// <param name="APathToFile">Full path to file</param>
        /// <param name="AContentHeader">An optional content header for the file</param>
        protected void Save(string APathToFile, string AContentHeader)
        {
            // Make sure that the folder exists
            string folderName = Path.GetDirectoryName(APathToFile);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            // Save each key/value pair
            using (StreamWriter sw = new StreamWriter(APathToFile))
            {
                if (AContentHeader != String.Empty)
                {
                    sw.Write(AContentHeader);
                    sw.WriteLine();
                }

                foreach (KeyValuePair <string, string>kvp in this)
                {
                    string s = String.Format("{0} = {1}", kvp.Key, kvp.Value);
                    sw.WriteLine(s);
                }

                sw.WriteLine();
                sw.Close();
            }
        }

        /// <summary>
        /// Standard Load method to load settings from a file
        /// </summary>
        /// <param name="APathToFile">Full path to file to load</param>
        protected void Load(string APathToFile)
        {
            if (!File.Exists(APathToFile))
            {
                return;
            }

            // Note that we load all key/values - even ones that we have not actually specified as public properties of the class
            // So any that were in the original file are preserved
            using (StreamReader sr = new StreamReader(APathToFile))
            {
                while (!sr.EndOfStream)
                {
                    // Read each line and split it on the = sign
                    // Ignore blank lines and any lines starting with ;
                    string s = sr.ReadLine();

                    if (s.Length > 0)
                    {
                        int pos = s.IndexOf('=');

                        if (pos > 0)
                        {
                            string s1 = s.Substring(0, pos - 1).Trim();
                            string s2 = s.Substring(pos + 1).Trim();

                            if ((s1.Length > 0) && (s2.Length > 0) && !s1.StartsWith(";") && !s1.StartsWith("#"))
                            {
                                if (this.ContainsKey(s1))
                                {
                                    // one of our public properties initialised in the constructor
                                    this[s1] = s2;
                                }
                                else
                                {
                                    // an additional one that we don't know about
                                    this.Add(s1, s2);
                                }
                            }
                        }
                    }
                }

                sr.Close();
            }
        }
    }
}