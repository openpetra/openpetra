//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       Tim Ingham
//       timop
//
// Copyright 2004-2019 by OM International
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

namespace Ict.Petra.Server.MSysMan.Common.WebConnectors
{
    /// <summary>
    /// Reads and updates/adds System Defaults.
    /// </summary>
    public static class TSystemDefaultsConnector
    {
        /// <summary>
        /// Call this Method to find out whether a System Default is defined, that is, if it exists in the System Defaults table.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default that should be checked.</param>
        /// <returns>True if the System Default is defined, false if it isn't.</returns>
        [RequireModulePermission("SYSMAN")]
        public static bool IsSystemDefaultDefined(String ASystemDefaultName)
        {
            return new TSystemDefaults().IsSystemDefaultDefined(ASystemDefaultName);
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// <para>
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        /// </para>
        /// </summary>
        /// <remarks>SystemDefault Names are not case sensitive.</remarks>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default, or SharedConstants.SYSDEFAULT_NOT_FOUND if the
        /// specified System Default doesn't exist.
        /// </returns>
        [RequireModulePermission("SYSMAN")]
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetSystemDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// <para>
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        /// </para>
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <remarks>SystemDefault Names are not case sensitive.</remarks>
        /// <returns>The value of the System Default, or the value of <paramref name="ADefault" /> if the
        /// specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            return new TSystemDefaults().GetSystemDefault(ASystemDefaultName, ADefault);
        }

        // The following set of functions serve as shortcuts to get User Defaults of a
        // specific type.

        /// <summary>
        /// Gets the value of a System Default as a bool.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a bool, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static bool GetBooleanDefault(String ASystemDefaultName, bool ADefault)
        {
            return new TSystemDefaults().GetBooleanDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as a bool.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a bool, or true if the specified System Default
        /// was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static bool GetBooleanDefault(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetBooleanDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as a char.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a char, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Char GetCharDefault(String ASystemDefaultName, System.Char ADefault)
        {
            return new TSystemDefaults().GetCharDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as a char.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a char, or the space character if the specified System Default
        /// was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Char GetCharDefault(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetCharDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as a double.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a double, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static double GetDoubleDefault(String ASystemDefaultName, double ADefault)
        {
            return new TSystemDefaults().GetDoubleDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as a double.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a double, or 0.0 if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static double GetDoubleDefault(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetDoubleDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int16.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as an Int16, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int16 GetInt16Default(String ASystemDefaultName, System.Int16 ADefault)
        {
            return new TSystemDefaults().GetInt16Default(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int16.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as an Int16, or 0 if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int16 GetInt16Default(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetInt16Default(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int32.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as an Int32, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int32 GetInt32Default(String ASystemDefaultName, System.Int32 ADefault)
        {
            return new TSystemDefaults().GetInt32Default(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int32.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as an Int32, or 0 if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int32 GetInt32Default(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetInt32Default(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int64.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <remarks><em>Do not inquire the 'SiteKey' System Default with this Method!</em> Rather, always use the
        /// <see cref="GetSiteKeyDefault"/> Method!</remarks>
        /// <returns>The value of the System Default as an Int64, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int64 GetInt64Default(String ASystemDefaultName, System.Int64 ADefault)
        {
            return new TSystemDefaults().GetInt64Default(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int64.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <remarks><em>Do not inquire the 'SiteKey' System Default with this Method!</em> Rather, always use the
        /// <see cref="GetSiteKeyDefault"/> Method!</remarks>
        /// <returns>The value of the System Default as an Int64, or 0 if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static System.Int64 GetInt64Default(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetInt64Default(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the value of a System Default as a string.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a string, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static String GetStringDefault(String ASystemDefaultName, String ADefault)
        {
            return new TSystemDefaults().GetStringDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as a string.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a string, or <see cref="string.Empty" />
        /// if the specified System Default was not found.</returns>
        [RequireModulePermission("SYSMAN")]
        public static String GetStringDefault(String ASystemDefaultName)
        {
            return new TSystemDefaults().GetStringDefault(ASystemDefaultName);
        }

        /// <summary>
        /// Gets the SiteKey Sytem Default.
        /// </summary>
        /// <remarks>
        /// Note: The SiteKey can get changed by a user with the necessary priviledges while being logged
        /// in to OpenPetra and this gets reflected when this Method gets called.</remarks>
        /// <returns>The SiteKey of the Site.</returns>
        [RequireModulePermission("SYSMAN")]
        public static Int64 GetSiteKeyDefault()
        {
            return new TSystemDefaults().GetSiteKeyDefault();
        }

        /// <summary>
        /// Returns the System Defaults as a DataTable.
        /// </summary>
        /// <returns>System Defaults Typed DataTable.
        /// </returns>
        [RequireModulePermission("SYSMAN")]
        public static SSystemDefaultsTable GetSystemDefaults()
        {
            return new TSystemDefaults().GetSystemDefaultsTable();
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        [RequireModulePermission("SYSMAN")]
        public static bool SetSystemDefault(String AKey, String AValue)
        {
            return new TSystemDefaults().SetSystemDefault(AKey, AValue);
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        /// <param name="AAdded">True if the System Default got added, false if it already existed.</param>
        [RequireModulePermission("SYSMAN")]
        public static bool SetSystemDefault(String AKey, String AValue, out bool AAdded)
        {
            return new TSystemDefaults().SetSystemDefault(AKey, AValue, out AAdded);
        }
    }
}
