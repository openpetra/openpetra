//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// A class that maintains local settings between user sessions by reading and writing the settings to a file.
    /// It uses its own simple text-based serialisation
    /// </summary>
    public class SettingsDictionary : SortedDictionary <string, string>
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
            // Call our helper method that reads the file and fills the dictionary
            DoFileLoad(_path);

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

            // Now do the low-level save of the file
            DoFileSave(_path);
        }

        /***************************************************************************************************************************************
         *
         * Helpers that handle the raw saving and loading
         *
         * ************************************************************************************************************************************/
        private void DoFileSave(string path)
        {
            // Make sure that the folder exists
            string folderName = Path.GetDirectoryName(path);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            // Save each key/value pair
            using (StreamWriter sw = new StreamWriter(path))
            {
                if (ContentHeader != String.Empty)
                {
                    sw.Write(ContentHeader);
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

        private void DoFileLoad(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            // Note that we load all key/values - even ones that we have not actually specified as public properties of the class
            // So any that were in the original file are preserved
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    // Read each line and split it on the = sign
                    // Ignore blank lines and any lines starting with ;
                    string s = sr.ReadLine();

                    if (s.Length > 0)
                    {
                        string[] items = s.Split('=');

                        if (items.Length == 2)
                        {
                            string s1 = items[0].Trim();
                            string s2 = items[1].Trim();

                            if ((s1.Length > 0) && (s2.Length > 0) && !s1.StartsWith(";"))
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