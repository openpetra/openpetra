//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MHospitality.WebConnectors
{
    /// <summary>
    /// webconnectors for the hospitality module
    /// </summary>
    public class THospitalityWebConnector
    {
        /// <summary>
        /// dummy function, please remove when real methods get added for this module
        /// </summary>
        [RequireModulePermission("CONFERENCE")]
        public static void Dummy()
        {
        }
    }
}