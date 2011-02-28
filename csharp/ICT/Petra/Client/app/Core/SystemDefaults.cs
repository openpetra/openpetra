//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance.SystemDefaults;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Gives access to all System Defaults.
    ///
    /// The System Defaults are stored in the Database on the Server and are cached
    /// by the PetraServer for speed reasons and to reduce the DB queries that are
    /// run. The System Defaults are also held in a cache table on the Client side
    /// which is managed by this unit. This is done to reduce remoting bandwidth.
    /// Both the Server-side and Client-side caches can be refreshed.
    ///
    /// @Comment The Client-side cache can be refreshed by the Server by queueing
    ///   a certain ClientTask for the Client. The TClientTaskInstance class then
    ///   calls the ReloadCachedSystemDefaults procedure to make the Client refresh
    ///   its cache as soon as the next request to GetSystemDefault is made.
    /// </summary>
    public class TSystemDefaults : object
    {
        /// <summary>holds a state that tells whether the Typed DataTable is cached or not</summary>
        private static Boolean UTableCached;

        /// <summary>this Typed DataTable holds the cached System Defaults</summary>
        private static SSystemDefaultsTable USystemDefaultsDT;

        /// <summary>used to control read and write access to the cache</summary>
        private static System.Threading.ReaderWriterLock UReadWriteLock = new System.Threading.ReaderWriterLock();

        #region TSystemDefaults

        /// <summary>
        /// Loads the System Defaults into the cached Typed DataTable.
        ///
        /// The System Defaults are retrieved from the PetraServer.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void LoadSystemDefaultsTable()
        {
            try
            {
                try
                {
                    // Prevent obtaining a read lock on the cache table while we are (re)loading the cache table!
                    UReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);

                    if (USystemDefaultsDT != null)
                    {
                        USystemDefaultsDT.Rows.Clear();
                    }

                    USystemDefaultsDT = TRemote.MSysMan.Maintenance.SystemDefaults.GetSystemDefaults();
                    UTableCached = true;
                }
                finally
                {
                    // Allow getting a read lock on the cache table agin.
                    UReadWriteLock.ReleaseWriterLock();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception in TSystemDefaults.LoadSystemDefaultsTable: " + exp.ToString());
                throw;
            }
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        ///
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
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
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be
        /// returned</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default was not found
        /// </returns>
        public static String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue;
            SSystemDefaultsRow FoundSystemDefaultsRow;

            if (!UTableCached)
            {
                LoadSystemDefaultsTable();
            }

            try
            {
                /* Try to get a read lock on the cache table [We don't specify a timeout because (1) reading an emptied cache would lead to problems (it is emptied before the Server request is issued), (2) the Server request should return fairly
                 *quick] */
                UReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);

                // Look up the System Default
                FoundSystemDefaultsRow = (SSystemDefaultsRow)USystemDefaultsDT.Rows.Find(ASystemDefaultName);

                if (FoundSystemDefaultsRow != null)
                {
                    ReturnValue = FoundSystemDefaultsRow.DefaultValue;
                }
                else
                {
                    ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
                }
            }
            finally
            {
                // Release read lock on the cache table
                UReadWriteLock.ReleaseReaderLock();
            }
            return ReturnValue;
        }

        /// <summary>
        /// Causes TSystemDefaults to reload the cached System Defaults Table the next
        /// time it is accessed.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ReloadCachedSystemDefaults()
        {
            UTableCached = false;
        }

        /// <summary>
        /// Causes the PetraServer to reload the Server-side cached System Defaults.
        /// Also causes TSystemDefaults to reload the cached System Defaults Table the
        /// next time it is accessed.
        ///
        /// This function must be used in the case when in the Petra 4GL System Manager
        /// -> Maintain System Parameters some parameters are changed. The 4GL screen
        /// will need to call this function to make the PetraServer reload the cached
        /// System Defaults. The Client will then reload its cache accordingly.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ReloadCachedSystemDefaultsOnServer()
        {
            TRemote.MSysMan.Maintenance.SystemDefaults.ReloadSystemDefaultsTable();
            UTableCached = false;
        }

        #endregion
    }
}