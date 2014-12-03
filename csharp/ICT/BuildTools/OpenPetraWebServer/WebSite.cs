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
using System.Xml;

using Ict.Tools.OpenPetraRuntimeHost;

namespace Ict.Tools.OpenPetraWebServer
{
    /// <summary>
    /// This small class holds the details of one specific web site
    /// </summary>
    public class WebSite
    {
        private int _port = 80;
        private string _physicalPath = "";
        private string _virtualPath = "";
        private string _defaultPage = "";
        private string _url = "";
        private bool _logPageRequests = false;

        /// <summary>
        /// The runtime host server associated with this site
        /// </summary>
        public Server WebServer = null;

        /// <summary>
        /// gets the port number
        /// </summary>
        public int Port {
            get
            {
                return _port;
            } set
            {
                _port = value; MakeUrl();
            }
        }

        /// <summary>
        /// Gets the physical path
        /// </summary>
        public string PhysicalPath {
            get
            {
                return _physicalPath;
            } set
            {
                _physicalPath = value;
            }
        }

        /// <summary>
        /// Gets the default page, if any
        /// </summary>
        public string DefaultPage {
            get
            {
                return _defaultPage;
            } set
            {
                _defaultPage = value; MakeUrl();
            }
        }

        /// <summary>
        /// Gets the url for the site
        /// </summary>
        public string Url {
            get
            {
                return _url;
            }
        }

        /// <summary>
        /// Gets the virtual path
        /// </summary>
        public string VirtualPath
        {
            get
            {
                return _virtualPath;
            }
            set
            {
                _virtualPath = value;

                if (!_virtualPath.StartsWith("/"))
                {
                    _virtualPath = "/" + _virtualPath;
                }

                if (!_virtualPath.EndsWith("/"))
                {
                    _virtualPath = _virtualPath + "/";
                }

                MakeUrl();
            }
        }

        /// <summary>
        /// Set to true if the web site should log page requests to a file
        /// </summary>
        public bool LogPageRequests
        {
            get
            {
                return _logPageRequests;
            }
            set
            {
                _logPageRequests = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebSite()
        {
        }

        private void MakeUrl()
        {
            _url = String.Empty;

            if ((_port >= 80) && (_port <= 65535))
            {
                _url = String.Format("http://localhost{0}{1}{2}",
                    _port == 80 ? String.Empty : ":" + _port.ToString(),
                    _virtualPath,
                    _defaultPage);
            }
        }
    }

    /// <summary>
    /// This class, derived from a Dictionary contains the information about all the specified web sites
    /// It has methods for reading/writing the information to a settings file.
    /// </summary>
    public class WebSites : Dictionary <string, WebSite>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WebSites()
        {
        }

        /// <summary>
        /// Method to read the settings from an Xml file
        /// </summary>
        public void Read(out bool StartAutomatically, out bool HideAtStartup, out bool AllowRemoteConnections)
        {
            StartAutomatically = true;
            HideAtStartup = true;
            AllowRemoteConnections = false;

            Clear();

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(Program.ConfigurationFilePath);
            }
            catch
            {
                string sXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<configuration/>";
                xmlDoc.LoadXml(sXml);
            }

            XmlAttributeCollection cfgAtt = xmlDoc.DocumentElement.Attributes;

            if (cfgAtt["allowremoteconnections"] != null)
            {
                AllowRemoteConnections = !cfgAtt["allowremoteconnections"].Value.Equals("0");
            }

            if (cfgAtt["hideatstartup"] != null)
            {
                HideAtStartup = !cfgAtt["hideatstartup"].Value.Equals("0");
            }

            if (cfgAtt["startautomatically"] != null)
            {
                StartAutomatically = !cfgAtt["startautomatically"].Value.Equals("0");
            }

            XmlNodeList nodes = xmlDoc.SelectNodes("//configuration/site");

            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode n in nodes)
            {
                WebSite ws = new WebSite();
                XmlAttribute a = n.Attributes["key"];

                if (a == null)
                {
                    continue;
                }

                string siteKey = a.Value.Trim();

                a = n.Attributes["port"];

                if (a == null)
                {
                    continue;
                }

                ws.Port = Convert.ToInt32(a.Value);

                XmlNode np = n.SelectSingleNode("physicalpath");

                if (np == null)
                {
                    continue;
                }

                ws.PhysicalPath = np.InnerText;
                char[] trim =
                {
                    '\t', '\r', '\n', ' '
                };
                ws.PhysicalPath = ws.PhysicalPath.Trim(trim);
                ws.PhysicalPath = ws.PhysicalPath.Replace('/', '\\');

                // These next two are optional
                ws.VirtualPath = String.Empty;
                a = n.Attributes["virtualpath"];

                if (a != null)
                {
                    ws.VirtualPath = a.Value;
                }

                if (!ws.VirtualPath.StartsWith("/"))
                {
                    ws.VirtualPath = "/" + ws.VirtualPath;
                }

                if (!ws.VirtualPath.EndsWith("/"))
                {
                    ws.VirtualPath = ws.VirtualPath + "/";
                }

                ws.DefaultPage = String.Empty;
                a = n.Attributes["defaultpage"];

                if (a != null)
                {
                    ws.DefaultPage = a.Value;
                }

                ws.LogPageRequests = false;
                a = n.Attributes["logpagerequests"];

                if (a != null)
                {
                    ws.LogPageRequests = a.Value != "0";
                }

                Add(siteKey, ws);
            }
        }

        /// <summary>
        /// Method to save the settings to a settings file
        /// </summary>
        public void Save(bool AllowRemoteConnections, bool HideAtStartup, bool StartAutomatically)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string sXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<configuration/>";

            xmlDoc.LoadXml(sXml);

            XmlElement root = xmlDoc.DocumentElement;

            if (AllowRemoteConnections)
            {
                XmlAttribute a = xmlDoc.CreateAttribute("allowremoteconnections");
                a.Value = "1";
                root.Attributes.Append(a);
            }

            if (!HideAtStartup)
            {
                XmlAttribute a = xmlDoc.CreateAttribute("hideatstartup");
                a.Value = "0";
                root.Attributes.Append(a);
            }

            if (!StartAutomatically)
            {
                XmlAttribute a = xmlDoc.CreateAttribute("startautomatically");
                a.Value = "0";
                root.Attributes.Append(a);
            }

            foreach (KeyValuePair <string, WebSite>kvp in this)
            {
                XmlElement nextSite = xmlDoc.CreateElement("site");

                XmlAttribute a = xmlDoc.CreateAttribute("key");
                a.Value = kvp.Key;
                nextSite.Attributes.Append(a);

                a = xmlDoc.CreateAttribute("port");
                a.Value = kvp.Value.Port.ToString();
                nextSite.Attributes.Append(a);

                a = xmlDoc.CreateAttribute("virtualpath");
                a.Value = kvp.Value.VirtualPath;
                nextSite.Attributes.Append(a);

                XmlElement sitePath = xmlDoc.CreateElement("physicalpath");
                sitePath.InnerText = kvp.Value.PhysicalPath.Replace('\\', '/');
                nextSite.AppendChild(sitePath);

                if (kvp.Value.DefaultPage != "")
                {
                    a = xmlDoc.CreateAttribute("defaultpage");
                    a.Value = kvp.Value.DefaultPage;
                    nextSite.Attributes.Append(a);
                }

                if (kvp.Value.LogPageRequests)
                {
                    a = xmlDoc.CreateAttribute("logpagerequests");
                    a.Value = "1";
                    nextSite.Attributes.Append(a);
                }

                root.AppendChild(nextSite);
            }

            try
            {
                xmlDoc.Save(Program.ConfigurationFilePath);
            }
            catch (System.Xml.XmlException e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error in Xml document when saving configuration file: " + e.Message,
                    Program.ApplicationTitle,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "General error when saving configuration file: " + e.Message,
                    Program.ApplicationTitle,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }
    }
}