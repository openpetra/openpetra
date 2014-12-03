//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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

namespace Ict.Tools.OPWebServerManager
{
    /// <summary>
    /// The command line class
    /// </summary>
    class CommandLineArgs
    {
        private bool _executeImmediateAction = false;
        private ImmediateActions _immediateAction = ImmediateActions.None;
        private int _port = 0;

        /// <summary>
        /// Returns true if the command line has an immediate action to execute
        /// </summary>
        public bool ExecuteImmediateAction
        {
            get
            {
                return _executeImmediateAction;
            }
        }

        /// <summary>
        /// Gets the immediate action to execute
        /// </summary>
        public ImmediateActions ImmediateAction
        {
            get
            {
                return _immediateAction;
            }
        }

        /// <summary>
        /// Gets the OP web server port that is to be targeted
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
        }

        /// <summary>
        /// Returns true if the command line is valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (_immediateAction == ImmediateActions.ShutdownAll)
                {
                    return true;
                }

                if (_immediateAction == ImmediateActions.None)
                {
                    return false;
                }

                return _port > 0;
            }
        }

        /// <summary>
        /// Parses the command line arguments
        /// </summary>
        /// <param name="args">The array of arguments passed to the main application program</param>
        public void ParseCommandLine(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("-op:"))
                {
                    string command = arg.Substring(4).Trim().ToLower();

                    switch (command)
                    {
                        case "shutdownall":
                            _immediateAction = ImmediateActions.ShutdownAll;
                            break;

                        case "shutdown":
                            _immediateAction = ImmediateActions.Shutdown;
                            break;

                        case "start":
                            _immediateAction = ImmediateActions.Start;
                            break;

                        case "stop":
                            _immediateAction = ImmediateActions.Stop;
                            break;
                    }
                }
                else if (arg.StartsWith("-port:"))
                {
                    int port;

                    if (Int32.TryParse(arg.Substring(6), out port))
                    {
                        _port = port;
                    }
                }
            }

            _executeImmediateAction = IsValid;
        }
    }
}