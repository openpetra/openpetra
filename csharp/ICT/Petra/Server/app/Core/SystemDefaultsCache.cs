//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Data.Odbc;
using System.Threading;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Cache Manager for the System Defaults of the Petra Site.
    /// It is instantiated only once (in ServerManager) and then
    /// accessed from each ClientDomain.
    ///
    /// The SystemDefaults are read from the DB when they are first requested,
    /// all subsequent requests just use the cached System Defaults. This is fully
    /// transparent to the caller. A reload of the cached System Defaults table can
    /// be requested. Read access to the cache table is denied while the cache table
    /// is (re)loaded to allow safe multi-threading operation.
    /// </summary>
    /// <remarks>The System Defaults are retrieved from the s_system_defaults table
    /// and are put into a Typed DataTable that has the structure of this table.</remarks>
    public class TSystemDefaultsCache : ISystemDefaultsCache
    {
        /// a static variable for global access to the system defaults
        public static TSystemDefaultsCache GSystemDefaultsCache;

        /*------------------------------------------------------------------------------
         * Partner System Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>Find Screen Options</summary>
        public const String PARTNER_GIFTRECEIPTINGDEFAULTS = "GiftReceiptingDefaults";

        /*------------------------------------------------------------------------------
         * Put other User Default Constants here as well.
         * -------------------------------------------------------------------------------*/

        /// <summary>holds a state that tells whether the Typed DataTable is cached or not</summary>
        private Boolean FTableCached;

        private readonly object FTableCachedLockCookie = new object();

        /// <summary>this Typed DataTable holds the cached System Defaults</summary>
        private SSystemDefaultsTable FSystemDefaultsDT;

        /// <summary>used to control read and write access to the cache</summary>
        private System.Threading.ReaderWriterLock FReadWriteLock;


        /// <summary>
        /// constructor
        /// </summary>
        public TSystemDefaultsCache() : base()
        {
            FReadWriteLock = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// Returns the System Defaults as a Typed DataTable.
        /// <para>
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        /// </para>
        /// </summary>
        /// <returns>System Defaults as a Typed DataTable.</returns>
        public SSystemDefaultsTable GetSystemDefaultsTable()
        {
            SSystemDefaultsTable ReturnValue;

            // Obtain thread-safe access to the FTableCached Field to prevent two (or more) Threads from getting a different
            // FTableCached value!
            lock (FTableCachedLockCookie)
            {
                if (!FTableCached)
                {
                    LoadSystemDefaultsTable();

                    FTableCached = true;
                }
            }

            try
            {
                /*
                 * Try to get a read lock on the cache table [We don't specify a timeout because
                 *   (1) reading an emptied cache would lead to problems (it is emptied before the DB queries are issued),
                 *   (2) reading the DB tables into the cached table should be fairly quick]
                 */
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                ReturnValue = FSystemDefaultsDT;
            }
            finally
            {
                // Release read lock on the cache table
                FReadWriteLock.ReleaseReaderLock();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Call this Method to find out whether a System Default is defined, that is, if it exists in the System Defaults table.
        /// </summary>
        /// <remarks>SystemDefault Names are not case sensitive.</remarks>
        /// <param name="ASystemDefaultName">The System Default that should be checked.</param>
        /// <returns>True if the System Default is defined, false if it isn't.</returns>
        public bool IsSystemDefaultDefined(String ASystemDefaultName)
        {
            String Tmp = GetSystemDefault(ASystemDefaultName);

            if (Tmp != SharedConstants.SYSDEFAULT_NOT_FOUND)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue;
            SSystemDefaultsRow FoundSystemDefaultsRow;

            // Obtain thread-safe access to the FTableCached Field to prevent two (or more) Threads from getting a different
            // FTableCached value!
            lock (FTableCachedLockCookie)
            {
                if (!FTableCached)
                {
                    LoadSystemDefaultsTable();

                    FTableCached = true;
                }
            }

            try
            {
                /*
                 * Try to get a read lock on the cache table [We don't specify a timeout because
                 *   (1) reading an emptied cache would lead to problems (it is emptied before the DB queries are issued),
                 *   (2) reading the DB tables into the cached table should be fairly quick]
                 */
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);

                // FSystemDefaultsDT is not case sensitive so Find will find the first case-insensitive match
                // The code to save a default handles ensuring that we never add a row with a 'similar' but non-identical primary key
                FoundSystemDefaultsRow = (SSystemDefaultsRow)FSystemDefaultsDT.Rows.Find(ASystemDefaultName);

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
                FReadWriteLock.ReleaseReaderLock();
            }

            return ReturnValue;
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
        public String GetSystemDefault(String ASystemDefaultName, String ADefault)
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

        // The following set of functions serve as shortcuts to get User Defaults of a
        // specific type.

        /// <summary>
        /// Gets the value of a System Default as a bool.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a bool, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public bool GetBooleanDefault(String ASystemDefaultName, bool ADefault)
        {
            string s = GetSystemDefault(ASystemDefaultName, ADefault.ToString());

            if ((string.Compare(s, "no", true) == 0) || (string.Compare(s, Boolean.FalseString, true) == 0))
            {
                // 'no' and 'False' are  always false
                return false;
            }

            // Otherwise return true, which includes a default value of 'yes'
            return true;
        }

        /// <summary>
        /// Gets the value of a System Default as a bool.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a bool, or true if the specified System Default
        /// was not found.</returns>
        public bool GetBooleanDefault(String ASystemDefaultName)
        {
            return GetBooleanDefault(ASystemDefaultName, true);
        }

        /// <summary>
        /// Gets the value of a System Default as a char.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a char, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public System.Char GetCharDefault(String ASystemDefaultName, System.Char ADefault)
        {
            return Convert.ToChar(GetSystemDefault(ASystemDefaultName, ADefault.ToString()));
        }

        /// <summary>
        /// Gets the value of a System Default as a char.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a char, or the space character if the specified System Default
        /// was not found.</returns>
        public System.Char GetCharDefault(String ASystemDefaultName)
        {
            return GetCharDefault(ASystemDefaultName, ' ');
        }

        /// <summary>
        /// Gets the value of a System Default as a double.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a double, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public double GetDoubleDefault(String ASystemDefaultName, double ADefault)
        {
            return Convert.ToDouble(GetSystemDefault(ASystemDefaultName, ADefault.ToString()));
        }

        /// <summary>
        /// Gets the value of a System Default as a double.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a double, or 0.0 if the specified System Default was not found.</returns>
        public double GetDoubleDefault(String ASystemDefaultName)
        {
            return GetDoubleDefault(ASystemDefaultName, 0.0);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int16.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as an Int16, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public System.Int16 GetInt16Default(String ASystemDefaultName, System.Int16 ADefault)
        {
            return Convert.ToInt16(GetSystemDefault(ASystemDefaultName, ADefault.ToString()));
        }

        /// <summary>
        /// Gets the value of a System Default as an Int16.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as an Int16, or 0 if the specified System Default was not found.</returns>
        public System.Int16 GetInt16Default(String ASystemDefaultName)
        {
            return GetInt16Default(ASystemDefaultName, 0);
        }

        /// <summary>
        /// Gets the value of a System Default as an Int32.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as an Int32, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public System.Int32 GetInt32Default(String ASystemDefaultName, System.Int32 ADefault)
        {
            return Convert.ToInt32(GetSystemDefault(ASystemDefaultName, ADefault.ToString()));
        }

        /// <summary>
        /// Gets the value of a System Default as an Int32.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as an Int32, or 0 if the specified System Default was not found.</returns>
        public System.Int32 GetInt32Default(String ASystemDefaultName)
        {
            return GetInt32Default(ASystemDefaultName, 0);
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
        public System.Int64 GetInt64Default(String ASystemDefaultName, System.Int64 ADefault)
        {
            return Convert.ToInt64(GetSystemDefault(ASystemDefaultName, ADefault.ToString()));
        }

        /// <summary>
        /// Gets the value of a System Default as an Int64.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <remarks><em>Do not inquire the 'SiteKey' System Default with this Method!</em> Rather, always use the
        /// <see cref="GetSiteKeyDefault"/> Method!</remarks>
        /// <returns>The value of the System Default as an Int64, or 0 if the specified System Default was not found.</returns>
        public System.Int64 GetInt64Default(String ASystemDefaultName)
        {
            return GetInt64Default(ASystemDefaultName, 0);
        }

        /// <summary>
        /// Gets the value of a System Default as a string.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <param name="ADefault">The value that should be returned if the System Default was not found.</param>
        /// <returns>The value of the System Default as a string, or the value of <paramref name="ADefault" />
        /// if the specified System Default was not found.</returns>
        public String GetStringDefault(String ASystemDefaultName, String ADefault)
        {
            return GetSystemDefault(ASystemDefaultName, ADefault);
        }

        /// <summary>
        /// Gets the value of a System Default as a string.
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be returned.</param>
        /// <returns>The value of the System Default as a string, or <see cref="string.Empty" />
        /// if the specified System Default was not found.</returns>
        public String GetStringDefault(String ASystemDefaultName)
        {
            return GetStringDefault(ASystemDefaultName, "");
        }

        /// <summary>
        /// Gets the SiteKey Sytem Default.
        /// </summary>
        /// <remarks>
        /// Note: The SiteKey can get changed by a user with the necessary priviledges while being logged
        /// in to OpenPetra and this gets reflected when this Method gets called.</remarks>
        /// <returns>The SiteKey of the Site.</returns>
        public System.Int64 GetSiteKeyDefault()
        {
            const string AlternativeSiteKeyDefaultKey = "SiteKeyPetra2";
            Int64 ReturnValue;

            ReturnValue = GetInt64Default(SharedConstants.SYSDEFAULT_SITEKEY, 99000000);

            if (ReturnValue != 99000000)
            {
                return ReturnValue;
            }
            else
            {
                // This is for the case that OpenPetra connects to a legacy (Petra 2.3) database.
                // In this case we cannot add the SiteKey to the SystemDefaults, because Petra 2.3 would have a conflict
                // since it adds it on startup already to the in-memory defaults, but not to the database.
                // (See also resolved Bug #114 (https://tracker.openpetra.org/view.php?id=114)
                ReturnValue = GetInt64Default(AlternativeSiteKeyDefaultKey, 99000000);
            }

            if (ReturnValue != 99000000)
            {
                return ReturnValue;
            }
            else
            {
                // This can happen either with a legacy Petra 2.x database or with a fresh OpenPetra database without
                // any Ledger yet,
                TLogging.LogAtLevel(1, TLogging.LOG_PREFIX_INFO +
                    String.Format("There is no '{0}' or '{1}' record in the s_system_defaults DB Table. This is only OK if " +
                        "this is a new OpenPetra Site!", SharedConstants.SYSDEFAULT_SITEKEY, AlternativeSiteKeyDefaultKey));

                return ReturnValue;
            }
        }

        /// <summary>
        /// Loads the System Defaults into the cached Typed DataTable.
        ///
        /// The System Defaults are retrieved from the s_system_defaults table and are
        /// put into a Typed DataTable that has the structure of this table.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void LoadSystemDefaultsTable()
        {
            TDataBase DBAccessObj = new Ict.Common.DB.TDataBase();
            TDBTransaction ReadTransaction = null;

            // Prevent other threads from obtaining a read lock on the cache table while we are (re)loading the cache table!
            FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);

            try
            {
                if (FSystemDefaultsDT != null)
                {
                    FSystemDefaultsDT.Clear();
                }

                try
                {
                    DBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                        TSrvSetting.PostgreSQLServer,
                        TSrvSetting.PostgreSQLServerPort,
                        TSrvSetting.PostgreSQLDatabaseName,
                        TSrvSetting.DBUsername,
                        TSrvSetting.DBPassword,
                        "",
                        "SystemDefaultsCache DB Connection");

                    DBAccessObj.BeginAutoReadTransaction(IsolationLevel.RepeatableRead, ref ReadTransaction,
                        delegate
                        {
                            FSystemDefaultsDT = SSystemDefaultsAccess.LoadAll(ReadTransaction);
                        });
                }
                finally
                {
                    DBAccessObj.CloseDBConnection();
                }

                // Thread.Sleep(5000);     uncomment this for debugging. This allows checking whether read access to FSystemDefaultsDT actually waits until we release the WriterLock in the finally block.
            }
            finally
            {
                // Other threads are now free to obtain a read lock on the cache table.
                FReadWriteLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Reloads the cached TypedDataTable that holds the System Defaults immediately.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ReloadSystemDefaultsTable()
        {
            LoadSystemDefaultsTable();
        }

        /// <summary>
        /// Sets the value of a System Default. If the System Default doesn't exist yet it will be created by that call.
        /// </summary>
        /// <param name="AKey">Name of new or existing System Default.</param>
        /// <param name="AValue">String Value.</param>
        public void SetSystemDefault(String AKey, String AValue)
        {
            bool SystemDefaultAdded;

            SetSystemDefault(AKey, AValue, out SystemDefaultAdded);
        }

        /// <summary>
        /// Stores a System Default in the DB. If it was already there it gets updated, if it wasn't there it gets added.
        /// </summary>
        /// <remarks>The change gets reflected in the System Defaults Cache the next time the System Defaults Cache
        /// gets accessed.</remarks>
        /// <param name="AKey">Name of the System Default.</param>
        /// <param name="AValue">Value of the System Default.</param>
        /// <param name="AAdded">True if the System Default got added, false if it already existed.</param>
        /// <remarks>SystemDefault Names are not case sensitive.</remarks>

        public void SetSystemDefault(String AKey, String AValue, out bool AAdded)
        {
            Boolean NewTransaction = false;
            Boolean ShouldCommit = false;
            SSystemDefaultsTable SystemDefaultsDT;

            try
            {
                TDBTransaction ReadWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

                SystemDefaultsDT = SSystemDefaultsAccess.LoadAll(ReadWriteTransaction);

                // This will find the row that matches a case-insensitive search of the table primary keys
                SystemDefaultsDT.CaseSensitive = false;     // It is anyway
                SSystemDefaultsRow match = (SSystemDefaultsRow)SystemDefaultsDT.Rows.Find(AKey);

                if (match != null)
                {
                    // I already have this System Default in the DB --> simply update the Value in the DB.
                    // (This will often be the case!)
                    match.DefaultValue = AValue;

                    AAdded = false;
                }
                else
                {
                    // The System Default isn't in the DB yet --> store it in the DB.
                    var SystemDefaultsDR = SystemDefaultsDT.NewRowTyped(true);
                    SystemDefaultsDR.DefaultCode = AKey;
                    SystemDefaultsDR.DefaultDescription = "Created in OpenPetra";
                    SystemDefaultsDR.DefaultValue = AValue;

                    SystemDefaultsDT.Rows.Add(SystemDefaultsDR);

                    AAdded = true;
                }

                SSystemDefaultsAccess.SubmitChanges(SystemDefaultsDT, ReadWriteTransaction);

                ShouldCommit = true;
            }
            catch (Exception Exc)
            {
                TLogging.Log(
                    "TSystemDefaultCache.SetSystemDefault: An Exception occured during the saving of the System Default '" + AKey +
                    "'. Value to be saved: + '" + AValue + "'" +
                    Environment.NewLine + Exc.ToString());

                ShouldCommit = false;

                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    if (ShouldCommit)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();

                        // We need to ensure that the next time the System Defaults Caches gets accessed it is refreshed from the DB!!!

                        // Obtain thread-safe access to the FTableCached Field to prevent two (or more) Threads from getting a different
                        // FTableCached value!
                        lock (FTableCachedLockCookie)
                        {
                            FTableCached = false;
                        }
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
            }
        }
    }
}
