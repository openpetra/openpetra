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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ict.Common
{
    /// <summary>
    /// Interface used by Open Petra Web Server
    /// </summary>
    public interface IOPWebServerManagerActions
    {
        /// <summary>
        /// Handle a request by the Manager to shut down, optionally if the server is using the specified port
        /// </summary>
        /// <param name="AWebPort">The web port</param>
        void ManagerShutdown(int AWebPort = -1);

        /// <summary>
        /// Handle a request by the Manager to start all web site servers
        /// </summary>
        void ManagerStartAll();

        /// <summary>
        /// Handle a request by the Manager to stop all web site servers
        /// </summary>
        void ManagerStopAll();

        /// <summary>
        /// Return True to the Manager if the server has started
        /// </summary>
        bool IsServerStarted();
    }
}