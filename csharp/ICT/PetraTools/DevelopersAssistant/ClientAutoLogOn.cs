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
using System.Xml;
using System.IO;

namespace Ict.Tools.DevelopersAssistant
{
    class ClientAutoLogOn
    {
        private string _branchLocation = String.Empty;
        private string _user = String.Empty;
        private string _password = String.Empty;
        private string _testAction = String.Empty;

        public string UserName {
            get
            {
                return _user;
            }
        }
        public string Password {
            get
            {
                return _password;
            }
        }
        public string TestAction {
            get
            {
                return _testAction;
            }
        }
        public static string DefaultString = "";

        /** Main constructor *********************************/
        public ClientAutoLogOn(string BranchLocation)
        {
            _branchLocation = BranchLocation;

            string path = BranchLocation + @"\inc\Template\etc\Client.config.my";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
                _user = GetPropertyValue(xmlDoc, "AutoLogin");
                _password = GetPropertyValue(xmlDoc, "AutoLoginPasswd");
                _testAction = GetPropertyValue(xmlDoc, "TestAction");
            }
            catch (Exception)
            {
            }
        }

        /****************************************************************************************************
         *
         * Other Public methods that handle manipulation of the AutoLogon Features
         *
         * *************************************************************************************************/

        /// <summary>
        /// Reset the personal client configuration file by overwriting it with the standard repository version
        /// </summary>
        /// <returns>True if successful</returns>
        public bool ResetConfig()
        {
            string sourcePath = Path.Combine(_branchLocation, @"inc\template\etc\Client.config");
            string targetPath = sourcePath + @".my";

            if (!File.Exists(sourcePath))
            {
                System.Windows.Forms.MessageBox.Show("The Assistant could not find the repository copy of the standard configuration file.",
                    Program.APP_TITLE);
                return false;
            }

            try
            {
                File.Copy(sourcePath, targetPath, true);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error copying file: " + ex.Message, Program.APP_TITLE);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update the personal client configuration file with new values
        /// </summary>
        /// <param name="UseAutoLogon">Set to true if you want to use Auto-logon</param>
        /// <param name="UserName">The username to use when auto-logon is set.  This cannot be blank if UseAutoLogon is True</param>
        /// <param name="Password">The password to use when auto-logon is set.  This can be blank</param>
        /// <param name="TestAction">The comma-separated list of property=value pairs that will be applied to the 'test action'</param>
        /// <returns></returns>
        public bool UpdateConfig(bool UseAutoLogon, string UserName, string Password, string TestAction)
        {
            string parentPath = _branchLocation + @"\inc\Template\etc\Client.config";
            string path = parentPath + @".my";

            try
            {
                if (!File.Exists(path))
                {
                    if (File.Exists(parentPath))
                    {
                        File.Copy(parentPath, path);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "Failed to create your personal client configuration because the standard repository file could not be found.",
                            Program.APP_TITLE);
                        return false;
                    }
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(path);

                if (UseAutoLogon)
                {
                    SetPropertyValue(xmlDoc, "AutoLogin", UserName);
                    SetPropertyValue(xmlDoc, "AutoLoginPasswd", Password);
                }
                else
                {
                    RemoveProperty(xmlDoc, "AutoLogin");
                    RemoveProperty(xmlDoc, "AutoLoginPasswd");
                }

                if (TestAction == String.Empty)
                {
                    RemoveProperty(xmlDoc, "TestAction");
                }
                else
                {
                    SetPropertyValue(xmlDoc, "TestAction", TestAction);
                }

                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error in updating the configuration file: " + ex.Message, Program.APP_TITLE);
                return false;
            }
            return true;
        }

        /***************************************************************************************************************************************
         *
         * Private helper functions
         *
         * ************************************************************************************************************************************/
        private string GetPropertyValue(XmlDocument xmlDoc, string PropertyName)
        {
            XmlNode parentNode = xmlDoc.DocumentElement.SelectSingleNode("appSettings");

            if (parentNode != null)
            {
                return XmlHelper.GetPropertyValue(xmlDoc, PropertyName, DefaultString, parentNode, "add", "key", "value");
            }

            return DefaultString;
        }

        private bool SetPropertyValue(XmlDocument xmlDoc, string PropertyName, string NewValue)
        {
            XmlNode parentNode = xmlDoc.DocumentElement.SelectSingleNode("appSettings");

            if (parentNode == null)
            {
                parentNode = xmlDoc.CreateElement("appSettings");
                xmlDoc.DocumentElement.AppendChild(xmlDoc.CreateWhitespace("\r\n  "));
                xmlDoc.DocumentElement.AppendChild(parentNode);
                xmlDoc.DocumentElement.AppendChild(xmlDoc.CreateWhitespace("\r\n"));
            }

            return XmlHelper.SetPropertyValue(xmlDoc, PropertyName, NewValue, parentNode, "add", "key", "value");
        }

        private bool RemoveProperty(XmlDocument xmlDoc, string PropertyName)
        {
            XmlNode parentNode = xmlDoc.DocumentElement.SelectSingleNode("appSettings");

            if (parentNode == null)
            {
                return true;                          // The property is not there anyway
            }

            return XmlHelper.RemoveProperty(xmlDoc, PropertyName, parentNode, "add", "key");
        }
    }
}