//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Gives access to all System Defaults.
    /// </summary>
    public static class TSystemDefaults
    {
        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">System Default Key.</param>
        /// <param name="ADefault">The value returned if System Default is not found.</param>
        /// <returns>The value of the System Default, or ADefault.</returns>
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            return TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.GetSystemDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default was not found.</returns>
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            return TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.GetSystemDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the SiteKey Sytem Default.
        /// </summary>
        /// <remarks>
        /// Note: The SiteKey can get changed by a user with the necessary priviledges while being logged
        /// in to OpenPetra and this gets reflected when this Method gets called.</remarks>
        /// <returns>The SiteKey of the Site.</returns>
        public static Int64 GetSiteKeyDefault()
        {
            return TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.GetSiteKeyDefault();
        }
        
        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        public static void SetSystemDefault(String AKey, String AValue)
        {
            bool SystemDefaultAdded;

            SetSystemDefault(AKey, AValue, out SystemDefaultAdded);
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        /// <param name="AAdded">True if the System Default got added, false if it already existed.</param>
        public static void SetSystemDefault(String AKey, String AValue, out bool AAdded)
        {
            TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.SetSystemDefault(AKey, AValue, out AAdded);
        }
    }
}