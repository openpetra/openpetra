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
        ///
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
        /// </summary>
        /// <returns>System Defaults as a Typed DataTable.
        /// </returns>
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
        /// specified System Default doesn't exist
        /// </returns>
        private String GetSystemDefault(String ASystemDefaultName)
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
        private String GetSystemDefault(String ASystemDefaultName, String ADefault)
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
        /// get boolean default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public bool GetBooleanDefault(String AKey, bool ADefault)
        {
            return Convert.ToBoolean(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get boolean default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>true if the key does not exist, otherwise value of key</returns>
        public bool GetBooleanDefault(String AKey)
        {
            return GetBooleanDefault(AKey, true);
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public System.Char GetCharDefault(String AKey, System.Char ADefault)
        {
            return Convert.ToChar(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>space if key does not exist</returns>
        public System.Char GetCharDefault(String AKey)
        {
            return GetCharDefault(AKey, ' ');
        }

        /// <summary>
        /// get double default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public double GetDoubleDefault(String AKey, double ADefault)
        {
            return Convert.ToDouble(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get double default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0.0 if key does not exist</returns>
        public double GetDoubleDefault(String AKey)
        {
            return GetDoubleDefault(AKey, 0.0);
        }

        /// <summary>
        /// Put other User Default Constants here as well.
        /// -------------------------------------------------------------------------------}// ...{
        /// The following set of functions serve as shortcuts to get User Defaults of a
        /// specific type.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved.</param>
        /// <param name="ADefault">The value that should be returned in case the Key is not (yet)
        /// in the User Defaults.
        /// </param>
        /// <returns>void</returns>
        public System.Int16 GetInt16Default(String AKey, System.Int16 ADefault)
        {
            return Convert.ToInt16(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public System.Int16 GetInt16Default(String AKey)
        {
            return GetInt16Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public System.Int32 GetInt32Default(String AKey, System.Int32 ADefault)
        {
            return Convert.ToInt32(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public System.Int32 GetInt32Default(String AKey)
        {
            return GetInt32Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <remarks><em>Do not inquire the 'SiteKey' System Default with this Method!</em> Rather, always use the
        /// <see cref="GetSiteKeyDefault"/> Method!</remarks>
        /// <returns></returns>
        public System.Int64 GetInt64Default(String AKey, System.Int64 ADefault)
        {
            return Convert.ToInt64(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <remarks><em>Do not inquire the 'SiteKey' System Default with this Method!</em> Rather, always use the
        /// <see cref="GetSiteKeyDefault"/> Method!</remarks>
        /// <returns>0 if key does not exist</returns>
        public System.Int64 GetInt64Default(String AKey)
        {
            return GetInt64Default(AKey, 0);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public String GetStringDefault(String AKey, String ADefault)
        {
            return GetSystemDefault(AKey, ADefault);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>empty string if key does not exist</returns>
        public String GetStringDefault(String AKey)
        {
            return GetStringDefault(AKey, "");
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
        public void SetSystemDefault(String AKey, String AValue, out bool AAdded)
        {
            Boolean NewTransaction = false;
            Boolean ShouldCommit = false;
            SSystemDefaultsTable SystemDefaultsDT;

            try
            {
                TDBTransaction ReadWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

                SystemDefaultsDT = SSystemDefaultsAccess.LoadByPrimaryKey(AKey, ReadWriteTransaction);

                if (SystemDefaultsDT.Rows.Count > 0)
                {
                    // I already have this System Default in the DB --> simply update the Value in the DB.
                    // (This will often be the case!)
                    DataRow SystemDefaultsDR = SystemDefaultsDT[0];
                    ((SSystemDefaultsRow)SystemDefaultsDR).DefaultValue = AValue;

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
