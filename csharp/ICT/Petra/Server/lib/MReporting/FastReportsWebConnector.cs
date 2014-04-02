//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Provides back-end methods to support the FastReports front end.
    /// </summary>
    public class TReportingWebConnector
    {
        /// <summary>Set this in a server utility to set the status</summary>
        public static String ServerStatus ="";

        /// <summary>Call this from the client to display the status:</summary>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static String GetServerStatus()
        {
            return ServerStatus;
        }
    }
}