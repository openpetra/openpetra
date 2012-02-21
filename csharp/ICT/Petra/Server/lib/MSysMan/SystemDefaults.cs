//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MSysMan.Maintenance
{
    /// <summary>
    /// Reads and saves a DataTable for the System Defaults.
    ///
    /// </summary>
    public class TSystemDefaults
    {
        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be
        /// returned</param>
        /// <param name="ADefault">The value that should be returned if the System Default was
        /// not found</param>
        /// <returns>The value of the System Default, or the value of the ADefault
        /// parameter if the specified System Default was not found
        /// </returns>
        public static String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            String ReturnValue = ADefault;

            String Tmp = GetSystemDefault(ASystemDefaultName);

            if (Tmp != SharedConstants.SYSDEFAULT_NOT_FOUND)
            {
                ReturnValue = Tmp;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be
        /// returned</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default was not found
        /// </returns>
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
            SSystemDefaultsTable SystemDefaultsTable = GetSystemDefaults();

            // Look up the System Default
            SSystemDefaultsRow FoundSystemDefaultsRow = (SSystemDefaultsRow)SystemDefaultsTable.Rows.Find(ASystemDefaultName);

            if (FoundSystemDefaultsRow != null)
            {
                ReturnValue = FoundSystemDefaultsRow.DefaultValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the System Defaults as a DataTable.
        ///
        /// </summary>
        /// <returns>System Defaults Typed DataTable.
        /// </returns>
        public static SSystemDefaultsTable GetSystemDefaults()
        {
            // $IFDEF DEBUGMODE if TLogging.DL >= 7 then Console.WriteLine(this.GetType.FullName + '.GetSystemDefaults called.'); $ENDIF

            return TSystemDefaultsCache.GSystemDefaultsCache.GetSystemDefaultsTable();

            // $IFDEF DEBUGMODE Console.WriteLine('SystemDefault "LocalisedCountyLabel": ' + GSystemDefaultsCache.GetSystemDefault('LocalisedCountyLabel'));$ENDIF
        }

        /// <summary>
        /// Reloads the cached SystemDefaults with the current DB table contents. Also
        /// generates ClientTasks for all current Clients except itself that tells
        /// Clients to refresh their SystemDefaults cache by reloading the SystemDefaults
        /// from the PetraServer.
        ///
        /// </summary>
        /// <param name="ASystemDefaultsDataTable">The reloaded System Defaults Typed DataTable.
        /// </param>
        /// <returns>void</returns>
        public void ReloadSystemDefaultsTable(ref SSystemDefaultsTable ASystemDefaultsDataTable)
        {
            ReloadSystemDefaultsTable();
            ASystemDefaultsDataTable = GetSystemDefaults();
        }

        /// <summary>
        /// Reloads the cached SystemDefaults with the current DB table contents. Also
        /// generates ClientTasks for all current Clients except itself that tells
        /// Clients to refresh their SystemDefaults cache by reloading the SystemDefaults
        /// from the PetraServer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ReloadSystemDefaultsTable()
        {
            // $IFDEF DEBUGMODE if TLogging.DL >= 7 then Console.WriteLine(this.GetType.FullName + '.ReloadSystemDefaultsTable called.'); $ENDIF
            TSystemDefaultsCache.GSystemDefaultsCache.ReloadSystemDefaultsTable();

            // $IFDEF DEBUGMODE if TLogging.DL >= 7 then Console.WriteLine(this.GetType.FullName + '.ReloadSystemDefaultsTable: calling DomainManager.ClientTaskAddToOtherClient...'); $ENDIF
            Ict.Petra.Server.App.Core.DomainManager.ClientTaskAddToOtherClient(-1,
                SharedConstants.CLIENTTASKGROUP_SYSTEMDEFAULTSREFRESH,
                "",
                1);
        }

        /// <summary>
        /// Saves the System Defaults.
        ///
        /// @comment Currently always returns false because the function in
        /// Ict.Common.ServerSettings needs to be rewritten!
        ///
        /// </summary>
        /// <param name="ASystemDefaultsDataTable">The Typed DataTable that contains changed and/or
        /// added System Defaults.</param>
        /// <returns>true if the System Defaults could be saved successfully, otherwise
        /// false.
        /// </returns>
        public Boolean SaveSystemDefaults(SSystemDefaultsTable ASystemDefaultsDataTable)
        {
            return TSystemDefaultsCache.GSystemDefaultsCache.SaveSystemDefaults(ASystemDefaultsDataTable);
        }
    }
}