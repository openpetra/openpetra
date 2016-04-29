//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       Tim Ingham
//       timop
//
// Copyright 2004-2015 by OM International
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

using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors
{
    /// <summary>
    /// Reads and updates/adds System Defaults. This Class exists solely for Client calls; for any server-side calls
    /// use the global (and always available!) <see cref="TSystemDefaultsCache.GSystemDefaultsCache" /> instance
    /// of <see cref="TSystemDefaultsCache" /> directly!
    /// </summary>
    /// <remarks>Utilises a thread-safe cache (<see cref="TSystemDefaultsCache" />) for speed!</remarks>
    public static class TSystemDefaults
    {
        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">System Default Key</param>
        /// <param name="ADefault">The value returned if System Default is not found.</param>
        /// <returns>The value of the System Default, or ADefault.</returns>
        [RequireModulePermission("NONE")]
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            return TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default was not found.</returns>
        [RequireModulePermission("NONE")]
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            return TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the SiteKey Sytem Default.
        /// </summary>
        /// <remarks>
        /// Note: The SiteKey can get changed by a user with the necessary priviledges while being logged
        /// in to OpenPetra and this gets reflected when this Method gets called.</remarks>
        /// <returns>The SiteKey of the Site.</returns>
        [RequireModulePermission("NONE")]
        public static Int64 GetSiteKeyDefault()
        {
            return TSystemDefaultsCache.GSystemDefaultsCache.GetSiteKeyDefault();
        }
        
        /// <summary>
        /// Returns the System Defaults as a DataTable.
        /// </summary>
        /// <returns>System Defaults Typed DataTable.
        /// </returns>
        [RequireModulePermission("NONE")]
        public static SSystemDefaultsTable GetSystemDefaults()
        {
            return TSystemDefaultsCache.GSystemDefaultsCache.GetSystemDefaultsTable();
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        [RequireModulePermission("NONE")]
        public static void SetSystemDefault(String AKey, String AValue)
        {
            TSystemDefaultsCache.GSystemDefaultsCache.SetSystemDefault(AKey, AValue);
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        /// <param name="AAdded">True if the System Default got added, false if it already existed.</param>
        [RequireModulePermission("NONE")]
        public static void SetSystemDefault(String AKey, String AValue, out bool AAdded)
        {
            TSystemDefaultsCache.GSystemDefaultsCache.SetSystemDefault(AKey, AValue, out AAdded);
        }
    }
}