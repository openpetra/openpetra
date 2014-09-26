//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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

namespace Ict.Tools.DevelopersAssistant
{
    class CommandArgs
    {
        /// <summary>
        /// Returns true if the command line contains the s switch
        /// </summary>
        public bool StartServer {
            get
            {
                return _startServer;
            }
        }
        /// <summary>
        /// Returns true if the command line contains the x switch
        /// </summary>
        public bool StopServer {
            get
            {
                return _stopServer;
            }
        }
        /// <summary>
        /// Returns a Startup Message if the command line contains the M: switch
        /// </summary>
        public string StartupMessage {
            get
            {
                return _startupMessage;
            }
        }

        private bool _startServer = false;
        private bool _stopServer = false;
        private string _startupMessage = String.Empty;

        /// <summary>
        /// The CommandArgs constructor
        /// </summary>
        /// <param name="args">Pass in the arguments from the application command line.  The arguments will be parsed and available as public properties</param>
        public CommandArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].CompareTo("/x") == 0)
                {
                    _stopServer = true;
                }

                if (args[i].CompareTo("/s") == 0)
                {
                    _startServer = true;
                }

                if (args[i].StartsWith("/M:"))
                {
                    _startupMessage = args[i].Substring(3);
                }
            }
        }
    }
}