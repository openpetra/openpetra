//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Ict.Tools.DevelopersAssistant
{
    class BuildConfiguration
    {
        private string _branchLocation = String.Empty;
        private SettingsDictionary _localSettings = null;

        // These are the values that are currently in the file
        private string _DBMSType;
        private string _DBName;
        private string _password;
        private string _port;
        private string _target;
        private string _version;

        private List <string>_storedDbBuildConfig = new List <string>();                     // List of stored configuration items

        /// <summary>
        /// The string that is used when there is no entry in the Build file
        /// </summary>
        public static string DefaultString = "<default>";
        /// <summary>
        /// The possible values for the DBMS
        /// </summary>
        public static string[] Systems =
        {
            "<default>", "SQLite", "PostgreSQL", "mySQL"
        };

        public BuildConfiguration(string BranchLocation, SettingsDictionary LocalSettings)
        {
            _branchLocation = BranchLocation;
            _localSettings = LocalSettings;

            // We can work out what our favourite configurations are whatever the branch location
            _storedDbBuildConfig.Clear();
            string s = _localSettings.DbBuildConfigurations;

            if (s != String.Empty)
            {
                string[] sep =
                {
                    "&&"
                };
                string[] items = s.Split(sep, StringSplitOptions.None);

                for (int i = 0; i < items.Length; i++)
                {
                    _storedDbBuildConfig.Add(items[i]);
                }
            }

            // Now we read the content of our working (current) config.  For that we need a valid branch location
            if (_branchLocation == String.Empty)
            {
                return;
            }

            _DBMSType = DefaultString;
            _DBName = DefaultString;
            _password = DefaultString;
            _port = DefaultString;
            _target = DefaultString;
            _version = DefaultString;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(BranchLocation + @"\OpenPetra.build.config");
                _DBMSType = GetPropertyValue(xmlDoc, "DBMS.Type");
                _DBName = GetPropertyValue(xmlDoc, "DBMS.DBName");
                _password = GetPropertyValue(xmlDoc, "DBMS.Password");
                _port = GetPropertyValue(xmlDoc, "DBMS.DBPort");
                _target = GetPropertyValue(xmlDoc, "DBMS.DBHostOrFile");

                if (String.Compare(_DBMSType, "postgresql", true) == 0)
                {
                    _version = GetPropertyValue(xmlDoc, "PostgreSQL.Version");
                }

                // Save the current configuration as a favourite
                SaveCurrentConfig();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Returns the display string for the current configuration defined in the build.config file
        /// </summary>
        public string CurrentConfig
        {
            get
            {
                if (_branchLocation == String.Empty)
                {
                    return "";
                }

                string s = String.Format("DBMS: {0}{1};  Port: {2};  Database name: {3};  Password: {4}\r\nLocation: {5}",
                    Systems[GetDBMSIndex(_DBMSType)],
                    IsDefined(_version) ? " version " + _version : String.Empty,
                    _port,
                    _DBName,
                    (_password == String.Empty) ? "<blank>" : _password,
                    _target);
                return s;
            }
        }

        /// <summary>
        /// Method to populate a list box with favourite configurations.  The first item is selected, if it exists
        /// </summary>
        /// <param name="listBox">The ListBox control to populate</param>
        public void ListAllConfigs(System.Windows.Forms.ListBox listBox)
        {
            listBox.Items.Clear();

            for (int i = 0; i < _storedDbBuildConfig.Count; i++)
            {
                string[] sep =
                {
                    "++"
                };
                string[] items = _storedDbBuildConfig[i].Split(sep, StringSplitOptions.None);
                string s = String.Empty;

                if (items[0] != String.Empty)
                {
                    s += String.Format(";  DBMS: {0}{1}", Systems[GetDBMSIndex(
                                                                      items[0])],
                        (items.Length > 6 && items[6] != String.Empty) ? " version " + items[6] : String.Empty);
                }

                if (items[1] != String.Empty)
                {
                    s += String.Format(";  Name: {0}", items[1]);
                }

                if (items[2] != String.Empty)
                {
                    s += String.Format(";  Port: {0}", items[2]);
                }

                if (items[3] != String.Empty)
                {
                    s += String.Format(";  Password: {0}", items[3]);
                }
                else if (items[4] != "0")
                {
                    s += ";  Password: <blank>";
                }

                if (items[5] != String.Empty)
                {
                    s += String.Format(";  Location: {0}", items[5]);
                }

                if (s.Length > 3)
                {
                    s = s.Substring(3);
                }

                listBox.Items.Add(s);
            }

            if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
            }
        }

        public string FavouriteConfigurations
        {
            get
            {
                return MakeConfigStringArray();
            }
        }

        /// <summary>
        /// Add a new favourite configuration to the stored list
        /// </summary>
        /// <param name="NewConfig">The new configuration.  Use MakeConfigString to format it correctly</param>
        public void AddConfig(string NewConfig)
        {
            _storedDbBuildConfig.Add(NewConfig);
            _localSettings.DbBuildConfigurations = MakeConfigStringArray();
        }

        /// <summary>
        /// Remove a favourite configuration from the stored list
        /// </summary>
        /// <param name="Index">The index into the array of favourites</param>
        public void RemoveConfig(int Index)
        {
            _storedDbBuildConfig.RemoveAt(Index);
            _localSettings.DbBuildConfigurations = MakeConfigStringArray();
        }

        /// <summary>
        /// Modify the specified favourite configuration in the stored list
        /// </summary>
        /// <param name="Index">The index into the array of favourites</param>
        /// <param name="NewConfig">The new configuration.  Use MakeConfigString to format it correctly</param>
        public void EditConfig(int Index, string NewConfig)
        {
            _storedDbBuildConfig[Index] = NewConfig;
            _localSettings.DbBuildConfigurations = MakeConfigStringArray();
        }

        /// <summary>
        /// Set the specified favourite configuration as the active configuration in the Build.Config file
        /// </summary>
        /// <param name="Index">The index into the array of favourites</param>
        /// <returns>True if successful</returns>
        public bool SetConfigAsDefault(int Index)
        {
            bool bRet = false;

            if ((Index >= 0) && (Index < _storedDbBuildConfig.Count))
            {
                bRet = true;
                string dbms, dbName, port, password, location, version;
                bool isBlank;
                GetStoredConfiguration(Index, out dbms, out dbName, out port, out password, out isBlank, out location, out version);
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.PreserveWhitespace = true;
                    string xmlPath = _branchLocation + @"\OpenPetra.build.config";

                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                    }
                    else
                    {
                        xmlDoc.LoadXml("<?xml version=\"1.0\"?>\r\n<project name=\"OpenPetra-userconfig\">\r\n</project>");
                    }

                    if (dbms == String.Empty)
                    {
                        bRet &= RemoveProperty(xmlDoc, "DBMS.Type");
                    }
                    else
                    {
                        bRet &= SetPropertyValue(xmlDoc, "DBMS.Type", dbms.ToLower());
                    }

                    if (version == String.Empty)
                    {
                        bRet &= RemoveProperty(xmlDoc, "PostgreSQL.Version");
                    }
                    else
                    {
                        bRet &= SetPropertyValue(xmlDoc, "PostgreSQL.Version", version);
                    }

                    if (dbName == String.Empty)
                    {
                        bRet &= RemoveProperty(xmlDoc, "DBMS.DBName");
                    }
                    else
                    {
                        bRet &= SetPropertyValue(xmlDoc, "DBMS.DBName", dbName);
                    }

                    if (port == String.Empty)
                    {
                        bRet &= RemoveProperty(xmlDoc, "DBMS.DBPort");
                    }
                    else
                    {
                        bRet &= SetPropertyValue(xmlDoc, "DBMS.DBPort", port);
                    }

                    if (location == String.Empty)
                    {
                        bRet &= RemoveProperty(xmlDoc, "DBMS.DBHostOrFile");
                    }
                    else
                    {
                        bRet &= SetPropertyValue(xmlDoc, "DBMS.DBHostOrFile", location);
                    }

                    if (isBlank)
                    {
                        bRet &= SetPropertyValue(xmlDoc, "DBMS.Password", String.Empty);
                    }
                    else
                    {
                        if (password == String.Empty)
                        {
                            bRet &= RemoveProperty(xmlDoc, "DBMS.Password");
                        }
                        else
                        {
                            bRet &= SetPropertyValue(xmlDoc, "DBMS.Password", password);
                        }
                    }

                    if (bRet)
                    {
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "The Assistant encountered an unexpected error while writing the new properties to the configuration file.",
                            Program.APP_TITLE);
                    }
                }
                catch (Exception)
                {
                    bRet = false;
                }
            }

            return bRet;
        }

        private void SaveCurrentConfig()
        {
            string dbms = (IsDefined(_DBMSType)) ? _DBMSType : String.Empty;
            string version = (IsDefined(_version)) ? _version : String.Empty;
            string dbName = (IsDefined(_DBName)) ? _DBName : String.Empty;
            string port = (IsDefined(_port)) ? _port : String.Empty;
            string location = (IsDefined(_target)) ? _target : String.Empty;
            string password = (IsDefined(_password)) ? _password : String.Empty;
            bool isBlank = (IsDefined(_password) && _password == String.Empty);

            // Is there something here worth saving??
            bool hasContent = dbms != String.Empty || dbName != String.Empty || port != String.Empty || location != String.Empty || password !=
                              String.Empty || isBlank;

            if (hasContent)
            {
                string s = MakeConfigString(dbms, dbName, port, password, isBlank, location, version);

                // Have we got it already?
                for (int i = 0; i < _storedDbBuildConfig.Count; i++)
                {
                    if (String.Compare(_storedDbBuildConfig[i], s, true) == 0)
                    {
                        return;
                    }
                }

                AddConfig(s);
            }
        }

        /// <summary>
        /// Make a formatted configuration string for use as a favourite.  Parameters can be blank in which case default values will be inherited
        /// </summary>
        /// <param name="DBMS">The DBMS Type eg SQLite or mySQL</param>
        /// <param name="DBName">The database name</param>
        /// <param name="Port">The database port</param>
        /// <param name="Password">The database password</param>
        /// <param name="IsBlank">Set this true to force the use of a blank password</param>
        /// <param name="Location">A parameter that specifies the host location or file location depending on the DBMS type</param>
        /// <param name="Version">The version of the DBMS (currently only used for PostgreSQL)</param>
        /// <returns>The formatted string for use as a stored favourite</returns>
        public static string MakeConfigString(string DBMS, string DBName, string Port, string Password, bool IsBlank, string Location, string Version)
        {
            return String.Format("{0}++{1}++{2}++{3}++{4}++{5}++{6}",
                DBMS.ToLower(), DBName, Port, Password, (IsBlank) ? "1" : "0", Location, Version);
        }

        /// <summary>
        /// Gets the index of the specified string in the list of DBMS Types, eg SQLite=1, PostgreSQL=2 etc
        /// </summary>
        /// <param name="DBMS">The DBMS type string</param>
        /// <returns>The index</returns>
        public static int GetDBMSIndex(string DBMS)
        {
            int ret = -1;

            if (DBMS == String.Empty)
            {
                return 0;
            }

            for (int i = 0; i < Systems.Length; i++)
            {
                if (String.Compare(Systems[i], DBMS, true) == 0)
                {
                    return i;
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the specified favourite details from the stored favourites.  An empty value implies that the parameter inherits the standard default
        /// </summary>
        /// <param name="Index">The index into the favourites list</param>
        /// <param name="DBMS">Returns the DBMS Type</param>
        /// <param name="DBName">Returns the DBMS database name</param>
        /// <param name="Port">Returns the port for the DBMS server</param>
        /// <param name="Password">Returns the password for the database</param>
        /// <param name="IsBlank">Returns if the password is explicitly blank</param>
        /// <param name="Location">Returns the location parameter (the host or file path)</param>
        /// <param name="Version">Returns the DBMS version</param>
        public void GetStoredConfiguration(int Index,
            out string DBMS,
            out string DBName,
            out string Port,
            out string Password,
            out bool IsBlank,
            out string Location,
            out string Version)
        {
            DBMS = String.Empty;
            DBName = String.Empty;
            Port = String.Empty;
            Password = String.Empty;
            IsBlank = false;
            Location = String.Empty;
            Version = String.Empty;

            if ((Index >= 0) && (Index < _storedDbBuildConfig.Count))
            {
                string[] sep =
                {
                    "++"
                };
                string[] items = _storedDbBuildConfig[Index].Split(sep, StringSplitOptions.None);
                DBMS = items[0];
                DBName = items[1];
                Port = items[2];
                Password = items[3];
                IsBlank = items[4] != "0";
                Location = items[5];

                if (items.Length > 6)
                {
                    Version = items[6];
                }
            }
        }

        private string MakeConfigStringArray()
        {
            string s = String.Empty;

            for (int i = 0; i < _storedDbBuildConfig.Count; i++)
            {
                if (s != String.Empty)
                {
                    s += "&&";
                }

                s += _storedDbBuildConfig[i];
            }

            return s;
        }

        private string GetPropertyValue(XmlDocument xmlDoc, string PropertyName)
        {
            XmlNode parentNode = xmlDoc.DocumentElement;

            if (parentNode != null)
            {
                return XmlHelper.GetPropertyValue(xmlDoc, PropertyName, DefaultString, parentNode, "property", "name", "value");
            }

            return DefaultString;
        }

        private bool SetPropertyValue(XmlDocument xmlDoc, string PropertyName, string NewValue)
        {
            XmlNode parentNode = xmlDoc.DocumentElement;

            return XmlHelper.SetPropertyValue(xmlDoc, PropertyName, NewValue, parentNode, "property", "name", "value");
        }

        private bool RemoveProperty(XmlDocument xmlDoc, string PropertyName)
        {
            XmlNode parentNode = xmlDoc.DocumentElement;

            if (parentNode == null)
            {
                return true;                          // The property is not there anyway
            }

            return XmlHelper.RemoveProperty(xmlDoc, PropertyName, parentNode, "property", "name");
        }

        private bool IsDefined(string s)
        {
            return String.Compare(s, DefaultString, true) != 0;
        }
    }
}