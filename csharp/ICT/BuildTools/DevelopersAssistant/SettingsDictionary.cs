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
        /// The path to the current working YAML file
        /// </summary>
        public string YAMLLocation {
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
        /// The selected index for the miscellaneous combo box
        /// </summary>
        public int MiscellaneousComboID {
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

        // Private members
        private string _path;       // path to local settings file
        private string _applicationVersion;

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
            Sequence = String.Empty;
            YAMLLocation = String.Empty;

            CodeGenerationComboID = 2;
            CompilationComboID = 2;
            DatabaseComboID = 1;
            FlashAfterSeconds = 15;
            MiscellaneousComboID = 0;

            AutoStartServer = true;
            AutoStopServer = true;
            MinimiseServerAtStartup = true;
            TreatWarningsAsErrors = true;
            DoPreBuildOnIctCommon = false;
            DoPostBuildOnPetraClient = false;

            // Add items to our dictionary
            this.Add("AltSequence", AltSequence);
            this.Add("BazaarPath", BazaarPath);
            this.Add("BranchLocation", BranchLocation);
            this.Add("DbBuildConfigurations", DbBuildConfigurations);
            this.Add("Sequence", Sequence);
            this.Add("YAMLLocation", YAMLLocation);

            this.Add("CodeGenerationComboID", CodeGenerationComboID.ToString());
            this.Add("CompilationComboID", CompilationComboID.ToString());
            this.Add("DatabaseComboID", DatabaseComboID.ToString());
            this.Add("FlashAfterSeconds", FlashAfterSeconds.ToString());
            this.Add("MiscellaneousComboID", MiscellaneousComboID.ToString());

            this.Add("AutoStartServer", AutoStartServer ? "1" : "0");
            this.Add("AutoStopServer", AutoStopServer ? "1" : "0");
            this.Add("MinimiseServerAtStartup", MinimiseServerAtStartup ? "1" : "0");
            this.Add("TreatWarningsAsErrors", TreatWarningsAsErrors ? "1" : "0");
            this.Add("DoPreBuildOnIctCommon", DoPreBuildOnIctCommon ? "1" : "0");
            this.Add("DoPostBuildOnPetraClient", DoPostBuildOnPetraClient ? "1" : "0");
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
            Sequence = this["Sequence"];
            YAMLLocation = this["YAMLLocation"];

            CodeGenerationComboID = Convert.ToInt32(this["CodeGenerationComboID"]);
            CompilationComboID = Convert.ToInt32(this["CompilationComboID"]);
            DatabaseComboID = Convert.ToInt32(this["DatabaseComboID"]);
            FlashAfterSeconds = Convert.ToUInt32(this["FlashAfterSeconds"]);
            MiscellaneousComboID = Convert.ToInt32(this["MiscellaneousComboID"]);

            AutoStartServer = (this["AutoStartServer"] != "0");
            AutoStopServer = (this["AutoStopServer"] != "0");
            MinimiseServerAtStartup = (this["MinimiseServerAtStartup"] != "0");
            TreatWarningsAsErrors = (this["TreatWarningsAsErrors"] != "0");
            DoPreBuildOnIctCommon = (this["DoPreBuildOnIctCommon"] != "0");
            DoPostBuildOnPetraClient = (this["DoPostBuildOnPetraClient"] != "0");

            // Do version-specific upgrades
            if (this.ContainsKey("ApplicationVersion"))
            {
                if (this["ApplicationVersion"].StartsWith("1.0.1."))
                {
                    // We no longer support 'clean' on the compilation combo
                    if (CompilationComboID > 0)
                    {
                        CompilationComboID = CompilationComboID - 1;
                        this["CompilationComboID"] = CompilationComboID.ToString();
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
            this["Sequence"] = Sequence;
            this["YAMLLocation"] = YAMLLocation;

            this["CodeGenerationComboID"] = CodeGenerationComboID.ToString();
            this["CompilationComboID"] = CompilationComboID.ToString();
            this["DatabaseComboID"] = DatabaseComboID.ToString();
            this["FlashAfterSeconds"] = FlashAfterSeconds.ToString();
            this["MiscellaneousComboID"] = MiscellaneousComboID.ToString();

            this["AutoStartServer"] = AutoStartServer ? "1" : "0";
            this["AutoStopServer"] = AutoStopServer ? "1" : "0";
            this["MinimiseServerAtStartup"] = MinimiseServerAtStartup ? "1" : "0";
            this["TreatWarningsAsErrors"] = TreatWarningsAsErrors ? "1" : "0";
            this["DoPreBuildOnIctCommon"] = DoPreBuildOnIctCommon ? "1" : "0";
            this["DoPostBuildOnPetraClient"] = DoPostBuildOnPetraClient ? "1" : "0";

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
                    sw.WriteLine(
                        "Database Schema = http://dbdoc.openpetra.org/ ++ The complete architecture of the Open Petra database");
                    sw.WriteLine(
                        "Developer's Forum = http://forum.openpetra.org/ ++ This links to the main developer forum where you can join in discussion of developer topics or ask a question.");
                    sw.WriteLine(
                        "Documentation for Developers = http://www.openpetra.org/en/developers-documentation ++ Useful links from the main public site for Open Petra");
                    sw.WriteLine(
                        "Jenkins Build Server = https://ci.openpetra.org/ ++ A link to the main Continuous Integration server that runs on Linux.");
                    sw.WriteLine(
                        "Jenkins Server on Windows = http://ci-win.openpetra.org:8080/ ++ A link to the dashboard of the Continuous Integration server that runs on Windows.");
                    sw.WriteLine(
                        "Launchpad = http://code.openpetra.org/ ++ A web interface to the code on the main Launchpad repository");
                    sw.WriteLine(
                        "Doxygen = http://codedoc.openpetra.org/ ++ A javadoc like documentation of the OpenPetra source code");
                    sw.WriteLine(
                        "Mantis Bug Tracker = https://tracker.openpetra.org/main_page.php ++ This links to the main project work item database known as 'Mantis'");
                    sw.WriteLine(
                        "Mantis Bug Tracker (My View) = https://tracker.openpetra.org/my_view_page.php ++ This links to the 'My View' page in the main project work item database known as 'Mantis'");
                    sw.WriteLine("OpenPetra Wiki = http://wiki.openpetra.org/ ++ This links to the main project wiki");
                    sw.WriteLine("Useful shortcuts = http://www.openpetra.org/en/shortcuts/ ++ Many useful shortcuts in one place, including many listed here");

                    sw.Close();
                }
            }

            // Call our base method that reads the file and fills the dictionary
            base.Load(_path);
        }

        /// <summary>
        /// Populate a list box using the keys in this dictionary
        /// </summary>
        /// <param name="listBox">The lsit box to be populated</param>
        public void PopulateListBox(System.Windows.Forms.ListBox listBox)
        {
            listBox.Items.Clear();

            foreach (KeyValuePair <string, string>kvp in this)
            {
                listBox.Items.Add(kvp.Key);
            }

            if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
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