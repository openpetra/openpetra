//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    #region DebugLevelDetails class

    /// <summary>
    /// Class that handles reading and writing of config files that determine the debug level for client and server
    /// </summary>
    public class DebugLevelDetails
    {
        private string _branchLocation = string.Empty;

        private int _buildConfigServerLevel = -1;
        private int _clientConfigMyLevel = -1;
        private int _serverConfigMyLevel = -1;

        private const string ERR_WRITING_FILE = "An error occurred while writing file '{0}'.  The system message was: {1}";
        private const string ERR_COPYING_FILE = "An error occurred while copying the file(s).  The system message was: {0}";

        /// <summary>
        /// Gets the effective client debug level
        /// </summary>
        public int ClientDebugLevel
        {
            get
            {
                if ((_buildConfigServerLevel == -1) && (_clientConfigMyLevel >= 0))
                {
                    return _clientConfigMyLevel;
                }
                else if ((_buildConfigServerLevel >= 0) && (_clientConfigMyLevel == -1))
                {
                    return _buildConfigServerLevel;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the effective server debug level
        /// </summary>
        public int ServerDebugLevel
        {
            get
            {
                if ((_buildConfigServerLevel == -1) && (_serverConfigMyLevel >= 0))
                {
                    return _serverConfigMyLevel;
                }
                else if ((_buildConfigServerLevel >= 0) && (_serverConfigMyLevel == -1))
                {
                    return _buildConfigServerLevel;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a boolean indicating of the debug levels are set in the build config and hence are the same value
        /// </summary>
        public bool IsSetByBuildConfig
        {
            get
            {
                return _clientConfigMyLevel == -1 && _serverConfigMyLevel == -1;
            }
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="ABranchLocation">The branch location that contains the configuration files</param>
        public DebugLevelDetails(string ABranchLocation)
        {
            _branchLocation = ABranchLocation;

            string path = Path.Combine(_branchLocation, "OpenPetra.build.config");
            _buildConfigServerLevel = GetValueFromConfigFile(path, "//property[@name='Server.DebugLevel']");

            path = Path.Combine(_branchLocation, "inc\\template\\etc", "Server-postgresql.config.my");
            _serverConfigMyLevel = GetValueFromConfigFile(path, "//add[@key='Server.DebugLevel']");

            path = Path.Combine(_branchLocation, "inc\\template\\etc", "Client.config.my");
            _clientConfigMyLevel = GetValueFromConfigFile(path, "//add[@key='Client.DebugLevel']");
        }

        /// <summary>
        /// Returns true if the passed in values are not the same as the current values
        /// </summary>
        public bool HasChanges(int AClientLevel, int AServerLevel)
        {
            return (AClientLevel != ClientDebugLevel) || (AServerLevel != ServerDebugLevel);
        }

        /// <summary>
        /// Saves the new settings that are passed as parameters
        /// </summary>
        public bool SaveChanges(bool AUseBuildConfig, int AClientLevel, int AServerLevel)
        {
            // Start by checking that we have the files we need...
            string buildConfigPath = Path.Combine(_branchLocation, "OpenPetra.build.config");

            if (File.Exists(buildConfigPath) == false)
            {
                // That's bad!
                string msg = string.Format("Cannot save the changes because the file '{0}' does not exist!", buildConfigPath);
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string serverMyPath = Path.Combine(_branchLocation, "inc\\template\\etc", "Server-postgresql.config.my");
            string clientMyPath = Path.Combine(_branchLocation, "inc\\template\\etc", "Client.config.my");
            bool gotServerMyFile = File.Exists(serverMyPath);
            bool gotClientMyFile = File.Exists(clientMyPath);

            if ((AClientLevel != AServerLevel) && ((gotClientMyFile == false) || (gotServerMyFile == false)))
            {
                string msg =
                    "When you choose a different client and server debug level the system requires that you have 'client.my' and 'server.my' files.  ";
                msg +=
                    "One or both of these files is missing.  Do you want to create them now?  If you choose 'Yes', some other debugging behaviours ";
                msg += "(such as debugging timeouts) will also be affected.";

                if (MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }

                // we will need these two files
                try
                {
                    if (gotServerMyFile == false)
                    {
                        // Copy the file
                        File.Copy(serverMyPath.Replace(".my", "._my"), serverMyPath);
                        gotServerMyFile = true;
                    }

                    if (gotClientMyFile == false)
                    {
                        // Copy the file
                        File.Copy(clientMyPath.Replace(".my", ""), clientMyPath);
                        gotClientMyFile = true;
                    }
                }
                catch (Exception ex)
                {
                    msg = string.Format(ERR_COPYING_FILE, ex.Message);
                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            bool bOk = false;

            if (AUseBuildConfig || (AClientLevel == AServerLevel))
            {
                // Step 1.1 - Write to Build.Config
                if (AServerLevel == 0)
                {
                    bOk = RemoveBuildConfigEntry(buildConfigPath);
                }
                else
                {
                    bOk = SetValueInConfigFile(buildConfigPath, "project", "property", "@name='Server.DebugLevel'", AServerLevel.ToString());
                }

                // Step 1.2 - Set Server.Config to use Build.Config
                bOk = bOk && SetValueInConfigFile(serverMyPath,
                    "configuration/appSettings",
                    "add",
                    "@key='Server.DebugLevel'",
                    "${Server.DebugLevel}");

                // Step 1.3 - Set Client.Config to use Build.Config
                bOk = bOk && SetValueInConfigFile(clientMyPath,
                    "configuration/appSettings",
                    "add",
                    "@key='Client.DebugLevel'",
                    "${Server.DebugLevel}");
            }
            else
            {
                // Do not use Build.Config
                // Step 2.1 - Set Build.Config empty
                bOk = RemoveBuildConfigEntry(buildConfigPath);

                // Step 2.2 - Set Server.Config to value
                bOk = bOk && SetValueInConfigFile(serverMyPath, "configuration/appSettings", "add", "@key='Server.DebugLevel'", AServerLevel.ToString());

                // Step 2.3 - Set Client.Config to value
                bOk = bOk && SetValueInConfigFile(clientMyPath, "configuration/appSettings", "add", "@key='Client.DebugLevel'", AClientLevel.ToString());
            }

            return bOk;
        }

        private int GetValueFromConfigFile(string ConfigFilePath, string xPath)
        {
            int retValue = -1;

            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ConfigFilePath);

                    XmlNode n = doc.SelectSingleNode(xPath);

                    if ((n != null) && (n.Attributes["value"] != null) && (n.Attributes["value"].Value.StartsWith("${") == false))
                    {
                        return Convert.ToInt32(n.Attributes["value"].Value);
                    }
                }
                catch (Exception ex)
                {
                    string msg = "The Assistant failed to open or read the configuraion file: " + ConfigFilePath;
                    msg += Environment.NewLine + Environment.NewLine + "The error message from the system was: " + ex.Message;
                    MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return retValue;
        }

        private bool RemoveBuildConfigEntry(string BuildConfigPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.Load(BuildConfigPath);

                XmlNode n = doc.SelectSingleNode("/project/property[@name='Server.DebugLevel']");

                if (n == null)
                {
                    // The node is not there anyway
                    return true;
                }

                // we delete the node
                doc.SelectSingleNode("/project").RemoveChild(n);
                doc.Save(BuildConfigPath);

                return true;
            }
            catch (Exception ex)
            {
                string msg = string.Format(ERR_WRITING_FILE, BuildConfigPath, ex.Message);
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool SetValueInConfigFile(string ConfigFilePath, string RootElementName, string ConfigElementName, string XPath, string NewValue)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.Load(ConfigFilePath);
                bool needsSave = true;

                string fullXPath = string.Format("/{0}/{1}[{2}]", RootElementName, ConfigElementName, XPath);
                XmlNode n = doc.SelectSingleNode(fullXPath);

                if (n == null)
                {
                    // we create the node
                    n = doc.CreateElement(ConfigElementName);
                    XmlAttribute attName = doc.CreateAttribute("name");
                    attName.Value = "Server.DebugLevel";
                    n.Attributes.Append(attName);

                    XmlAttribute attValue = doc.CreateAttribute("value");
                    attValue.Value = NewValue;
                    n.Attributes.Append(attValue);

                    XmlNode rootNode = doc.SelectSingleNode("/" + RootElementName);
                    rootNode.InsertBefore(n, rootNode.FirstChild);
                }
                else
                {
                    XmlAttribute attValue = n.Attributes["value"];

                    if (attValue == null)
                    {
                        // That's a surprise!
                        attValue = doc.CreateAttribute("value");
                        attValue.Value = NewValue;
                        n.Attributes.Append(attValue);
                    }
                    else if (attValue.Value != NewValue)
                    {
                        attValue.Value = NewValue;
                    }
                    else
                    {
                        needsSave = false;
                    }
                }

                if (needsSave)
                {
                    doc.Save(ConfigFilePath);
                }

                return true;
            }
            catch (Exception ex)
            {
                string msg = string.Format(ERR_WRITING_FILE, ConfigFilePath, ex.Message);
                MessageBox.Show(msg, Program.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    #endregion

    #region DebugLevels class

    /// <summary>
    /// A class that encapsulates the way that debug levels affect client and server logging
    /// </summary>
    public class DebugLevels
    {
        /// <summary> The level (0-15) </summary>
        public int Level {
            get; set;
        }
        /// <summary> Impact on the client </summary>
        public string Client {
            get; set;
        }
        /// <summary> Impact on the server </summary>
        public string Server {
            get; set;
        }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="aLevel">Debug level</param>
        /// <param name="aClient">Impact of this level on client debugging</param>
        /// <param name="aServer">Impact of this level on server debugging</param>
        public DebugLevels(int aLevel, string aClient, string aServer)
        {
            Level = aLevel;
            Client = aClient;
            Server = aServer;
        }

        /// <summary>
        /// Static method that populates a List of debug level actions
        /// </summary>
        /// <returns>An IList for the DataGridView</returns>
        public static List <DebugLevels>GetDebugLevelList()
        {
            List <DebugLevels>list = new List <DebugLevels>();

            list.Add(new DebugLevels(1, "", "1. Progress tracker.  2. Finance posting"));
            list.Add(new DebugLevels(2, "", "Detailed connection information"));
            list.Add(new DebugLevels(3, "Connector logging", "1. Full text of SQL Queries  2. Co-ordinated database access"));
            list.Add(new DebugLevels(4, "More detailed reporting log file", ""));
            list.Add(new DebugLevels(5, "Some messages relating to cacheable tables", "1. First chance exceptions  2. Detailed reporting logs"));
            list.Add(new DebugLevels(6, "", "1. Results from SQL Queries  2. Partner reminders"));
            list.Add(new DebugLevels(7, "Cacheable tables level 7", "1. Cacheable tables level 7  2. User defaults  3. Partner Find"));
            list.Add(new DebugLevels(8, "", "1. Best address  2. Partner Edit"));
            list.Add(new DebugLevels(9, "", "1. Domain Manager connections  2. Reference counting  3. Partner address"));
            list.Add(new DebugLevels(10, "Full cacheable tables logging",
                    "1. DB transaction information  2. Full Stack Trace  3. Full cacheable tables  4. Access permissions"));
            list.Add(new DebugLevels(11, "", "Co-ordinated database access stack traces"));

            return list;
        }
    }

    #endregion
}