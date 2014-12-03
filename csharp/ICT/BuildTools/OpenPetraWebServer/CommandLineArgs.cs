//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp, timop
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Tools.OpenPetraWebServer
{
    class CommandLineArgs
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// The command line can be blank - in which case the full UI is used
        /// or it can have one optional parameter
        /// "path-to-exe" [-logfile:"full-path-to-logfile"]
        ///
        /// or it can have parameters as follows - in which case the simplified UI is shown with one instance per port
        ///
        /// "path-to-exe" -physicalPath:"fully-qualified-physical-path" -port:portNumber
        ///       [-virtualPath:virtualPath] [-defaultPage:defaultPage] [-r:true] [-quiet:true] [-logfile:"full-path-to-logfolder"] [-logPageRequests:true] [-maxRuntime:NN]
        ///


        private int _port = 0;
        private string _physicalPath = string.Empty;
        private string _virtualPath = string.Empty;
        private string _defaultPage = string.Empty;
        private bool _acceptRemoteConnection = false;
        private bool _logPageRequests = true;               //default is to log page requests
        private bool _suppressStartUpMessages = false;
        private Int16 _maxRuntimeInMinutes = 0;             // unlimited

        private bool _useFullUI = false;                    // we will use it if specified or if not enough info in command line or settings file
        private string _logfilePath = String.Empty;         // we will use the default location if we can

        public string PhysicalPath {
            get
            {
                return _physicalPath;
            }
        }
        public string VirtualPath {
            get
            {
                return _virtualPath;
            }
        }
        public int Port {
            get
            {
                return _port;
            }
        }
        public bool AcceptRemoteConnection {
            get
            {
                return _acceptRemoteConnection;
            }
        }
        public string DefaultPage {
            get
            {
                return _defaultPage;
            }
        }
        public bool SuppressStartupMessages {
            get
            {
                return _suppressStartUpMessages;
            }
        }
        public Int16 MaxRuntimeInMinutes {
            get
            {
                return _maxRuntimeInMinutes;
            }
        }
        public string LogfilePath
        {
            get
            {
                return _logfilePath;
            }
        }
        public bool UseFullUI
        {
            get
            {
                return _useFullUI;
            }
        }
        public bool LogPageRequests
        {
            get
            {
                return _logPageRequests;
            }
        }

        public CommandLineArgs()
        {
            _physicalPath = TAppSettingsManager.GetValue("physicalPath", string.Empty, false);      // nant sets this
            _virtualPath = TAppSettingsManager.GetValue("virtualPath", string.Empty, false);
            _defaultPage = TAppSettingsManager.GetValue("defaultPage", string.Empty, false);
            _port = TAppSettingsManager.GetInt16("port", 0);                                        // nant sets this
            _acceptRemoteConnection = TAppSettingsManager.GetBoolean("acceptRemoteConnection", false);
            _suppressStartUpMessages = TAppSettingsManager.GetBoolean("quiet", false);              // nant usually sets this property to true
            _maxRuntimeInMinutes = TAppSettingsManager.GetInt16("maxRuntimeInMinutes", 0);
            _logPageRequests = TAppSettingsManager.GetBoolean("logPageRequests", true);             // you can use the config to turn loggin OFF
            _useFullUI = TAppSettingsManager.GetBoolean("useFullUI", false);                        // we will only use the full UI if specified or if no port or path
            _logfilePath = TAppSettingsManager.GetValue("logfilePath", string.Empty, false);        // we will use default location
        }

        public void ParseCommandLine(string[] args)
        {
            Int32 tryInt;
            Int16 tryShort;

            foreach (string arg in args)
            {
                if (arg.Contains(':'))
                {
                    int pos = arg.IndexOf(':');
                    string[] parts = arg.Split(':');

                    if (parts[0].StartsWith("-po") || parts[0].StartsWith("/po"))
                    {
                        if (Int32.TryParse(arg.Substring(pos + 1), out tryInt) && (tryInt >= 80) && (tryInt <= 65535))
                        {
                            _port = tryInt;
                        }
                    }
                    else if (parts[0].StartsWith("-ph") || parts[0].StartsWith("/ph"))
                    {
                        _physicalPath = arg.Substring(pos + 1);
                    }
                    else if (parts[0].ToLower().StartsWith("-logpage") || parts[0].ToLower().StartsWith("/logpage"))
                    {
                        _logPageRequests = (arg.Substring(pos + 1) == "true");
                    }
                    else if (parts[0].StartsWith("-l") || parts[0].StartsWith("/l"))
                    {
                        _logfilePath = arg.Substring(pos + 1);
                    }
                    else if (parts[0].StartsWith("-v") || parts[0].StartsWith("/v"))
                    {
                        _virtualPath = arg.Substring(pos + 1);
                    }
                    else if (parts[0].StartsWith("-d") || parts[0].StartsWith("/d"))
                    {
                        _defaultPage = arg.Substring(pos + 1);
                    }
                    else if (parts[0].StartsWith("-r") || parts[0].StartsWith("/r"))
                    {
                        _acceptRemoteConnection = (arg.Substring(pos + 1) == "true");
                    }
                    else if (parts[0].StartsWith("-q") || parts[0].StartsWith("/q"))
                    {
                        _suppressStartUpMessages = (arg.Substring(pos + 1) == "true");
                    }
                    else if (parts[0].StartsWith("-u") || parts[0].StartsWith("/u"))
                    {
                        _useFullUI = (arg.Substring(pos + 1) == "true");
                    }
                    else if (parts[0].StartsWith("-max") || parts[0].StartsWith("/max"))
                    {
                        if (Int16.TryParse(arg.Substring(pos + 1), out tryShort) && (tryShort > 0))
                        {
                            _maxRuntimeInMinutes = tryShort;
                        }
                    }
                }
            }

            // We use the full UI if: it is specified, or we do not have a port or physical path
            _useFullUI = (_useFullUI || (_port < 80) || (_physicalPath == String.Empty));

            GetLogFilePath();
        }

        /// <summary>
        /// Returns true if the command line is valid for the Small UI
        /// </summary>
        public bool IsValid
        {
            get
            {
                bool isValid = (_physicalPath != string.Empty && _port >= 80
                                && Directory.Exists(_physicalPath));

                return isValid;
            }
        }

        private void GetLogFilePath()
        {
            if (_logfilePath == String.Empty)
            {
                // no particular location specified - so where are we??
                string pathToMe = Path.GetDirectoryName(Application.ExecutablePath);

                // since this is a development web server we can assume the folder layout is what it is in source control
                if (pathToMe.EndsWith("\\bin"))
                {
                    pathToMe = pathToMe.Substring(0, pathToMe.LastIndexOf('\\'));
                }

                if (pathToMe.Contains('\\'))
                {
                    pathToMe = pathToMe.Substring(0, pathToMe.LastIndexOf('\\'));
                    _logfilePath = Path.Combine(pathToMe, "log");
                }
            }

            // If the folder does not exists we do not create it
            if (!Directory.Exists(_logfilePath))
            {
                _logfilePath = String.Empty;
            }
        }
    }
}