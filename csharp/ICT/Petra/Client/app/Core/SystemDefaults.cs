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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Gives access to all System Defaults.
    ///
    /// </summary>
    public class TSystemDefaults : object
    {
        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">System Default Key</param>
        /// <param name="ADefault">The value returned if System Default not found</param>
        /// <returns>The value of the System Default, or ADefault
        /// </returns>
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            String ReturnValue;
            String Tmp;

            Tmp = GetSystemDefault(ASystemDefaultName);

            if (Tmp != SharedConstants.SYSDEFAULT_NOT_FOUND)
            {
                ReturnValue = Tmp;
            }
            else
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        ///
        /// The whole table is read from the server - it's not a problem so long as there's not
        /// thousands of system defaults, and this method isn't called too often!
        ///
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default was not found
        /// </returns>
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue;
            SSystemDefaultsRow FoundSystemDefaultsRow;

            SSystemDefaultsTable SystemDefaultsDT = TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.GetSystemDefaults();

            // Look up the System Default
            FoundSystemDefaultsRow = (SSystemDefaultsRow)SystemDefaultsDT.Rows.Find(ASystemDefaultName);

            if (FoundSystemDefaultsRow != null)
            {
                ReturnValue = FoundSystemDefaultsRow.DefaultValue;
            }
            else
            {
                ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
            }

            return ReturnValue;
        }

        /// <summary>
        /// SetSystemDefault
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default</param>
        /// <param name="AValue">String Value</param>
        public static void SetSystemDefault(String AKey, String AValue)
        {
            TRemote.MSysMan.Maintenance.SystemDefaults.WebConnectors.SetSystemDefault(AKey, AValue);
        }
    }
}