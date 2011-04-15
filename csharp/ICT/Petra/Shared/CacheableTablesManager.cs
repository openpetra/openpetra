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
using System.Runtime.Serialization;
using System.Threading;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Common.Data;
using Ict.Common;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// This Delegate is used for sending a ClientTask to all other Clients to tell
    /// them to reload a certain Cacheable DataTable (used only if the
    /// TCacheableTablesManager is instantiated by PetraServer, but not if it is
    /// instantiated by the PetraClient).
    /// </summary>
    public delegate Int32 TDelegateSendClientTask (System.Int32 AClientID,
        String ATaskGroup, String ATaskCode,
        System.Object ATaskParameter1, System.Object ATaskParameter2,
        System.Object ATaskParameter3, System.Object ATaskParameter4,
        System.Int16 ATaskPriority,
        System.Int32 AExceptClientID);

    /// <summary>
    /// ------------------------------------------------------------------------------
    /// Contains methods that allow managing a Cache that can contain arbitrary
    /// DataTables (typed and untyped DataTables).
    ///
    /// This Class is an implementation of a Cache Manager. It gets used by *both*
    /// PetraServer and PetraClient to manage a Cache of DataTables (that would
    /// usually be lookup tables for ComboBoxes, etc.).
    /// When it is used by the PetraServer, it is instantiated only once in the main
    /// AppDomain of the PetraServer. Each time a Client requests a Cacheable
    /// DataTable from the PetraServer, a cross-AppDomain call from the Client's
    /// AppDomain to methods of the Object of this Class that is instantiated in the
    /// main AppDomain is made.
    /// When it is used by the PetraClient, it is instantiated only once by the
    /// TDataCache Class in Ict.Petra.Client.App.Core.Cache, which is the only Class
    /// that programmers need to use on the Client side.
    ///
    /// @comment This class is thread save. It protects *two* resources,
    /// UDataCacheDataSet and UDataCacheContentsDT with the same ReaderWriterLock
    /// (not with a Monitor - for performance). The DataTables that are handed out
    /// of TCacheableTablesManager are *copies* of the DataTables in the Cache.
    /// This is done to get around multi-threading reading/writing issues that
    /// might occur when the caller performs read or write operations on the
    /// DataTable - which are out of control of this class.
    ///
    /// @comment Several of the Methods are declared as 'virtual'. This is necessary
    /// for all Methods that are called cross-AppDomain. Explanation for this:
    /// mono can't cope with method calls into different AppDomains if these
    /// methods are not marked virtual (see answer of Lluis Sanchez for the filed
    /// bug #76752 in mono's bugzilla). Apparently, C# code automatically marks
    /// such methods virtual when it is JITted, but Delphi.NET code doesn't do this.
    ///
    /// @TODO Test some cache management functions (enabled by setting Properties
    /// MaxCacheSize and MaxTimeInCache) - the implementation of adhering to these
    /// settings is probably not quite finished yet (due to time constraints).
    ///
    /// </summary>
    public class TCacheableTablesManager : MarshalByRefObject
    {
        /// <summary>Holds all cached tables (typed/untyped DataTable), plus one Typed DataTable for the 'Table of Contents' of the Cache</summary>
        private static CacheableTablesTDS UDataCacheDataSet;

        /// <summary>'Table of Contents' of the Cache, used for managing the state of cached  DataTables</summary>
        private static CacheableTablesTDSContentsTable UDataCacheContentsDT;

        /// <summary>Maximum size that the Cache shouldn't exceed</summary>
        private static Int32 UMaxCacheSize;

        /// <summary>Maximum time that a DataTable in the Cache should be cached</summary>
        private static TimeSpan UMaxTimeInCache;

        /// <summary>
        /// </summary>
        public static void InitializeUnit()
        {
            UDataCacheDataSet = new CacheableTablesTDS("CacheableTables");
            UDataCacheContentsDT = UDataCacheDataSet.Contents;
            UMaxCacheSize = -1;
            UMaxTimeInCache = TimeSpan.MinValue;
        }

        /// <summary>Reference to a Delegate that is used for sending a ClientTask to all other Clients</summary>
        private TDelegateSendClientTask FDelegateSendClientTask;
        private ReaderWriterLock FReadWriteLock;

        /// <summary>Current size of the data in the Cache</summary>
        public Int32 CacheSize
        {
            get
            {
                return Get_CacheSize();
            }
        }

        /// <summary>Number of DataTables in the Cache</summary>
        public Int32 CachedTablesCount
        {
            get
            {
                return Get_CachedTablesCount();
            }
        }

        /// <summary>Maximum size that the Cache shouldn't exceed</summary>
        public Int32 MaxCacheSize
        {
            get
            {
                return Get_MaxCacheSize();
            }

            set
            {
                Set_MaxCacheSize(value);
            }
        }

        /// <summary>Maximum time that a DataTable in the Cache should be cached</summary>
        public TimeSpan MaxTimeInCache
        {
            get
            {
                return Get_MaxTimeInCache();
            }

            set
            {
                Set_MaxTimeInCache(value);
            }
        }


        #region TCacheableTablesManager

        /// <summary>
        /// Constructor
        ///
        /// </summary>
        /// <param name="ADelegateSendClientTask">Delegate that is used for sending a ClientTask
        /// to all other Clients to tell them to reload a certain Cacheable DataTable
        /// </param>
        /// <returns>void</returns>
        public TCacheableTablesManager(TDelegateSendClientTask ADelegateSendClientTask)
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                TLogging.Log(
                    this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString() + ". AppDomain '" +
                    AppDomain.CurrentDomain.FriendlyName + "'.");
            }
#endif
            FDelegateSendClientTask = ADelegateSendClientTask;
            FReadWriteLock = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// Ensures that TCacheableTablesManager exists until this AppDomain is unloaded.
        ///
        /// </summary>
        /// <returns>void</returns>
        public override object InitializeLifetimeService()
        {
            return null; // make sure that TCacheableTablesManager exists until this AppDomain is unloaded!
        }

        #region Property Accessors

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public Int32 Get_CachedTablesCount()
        {
            Int32 ReturnValue;

            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CachedTablesCount waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CachedTablesCount grabbed a ReaderLock.");
                }
#endif

                // Return the number of DataTables in UDataCacheDataSet minus 1 (to account
                // for the Contents Table)
                ReturnValue = UDataCacheDataSet.Tables.Count - 1;
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CachedTablesCount released the ReaderLock.");
                }
#endif
            }
            return ReturnValue;
        }

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public Int32 Get_CacheSize()
        {
            Int32 ReturnValue;
            int Counter;

            ReturnValue = 0;
            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CacheSize waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CacheSize grabbed a ReaderLock.");
                }
#endif

                // Add up all TableSizes
                for (Counter = 0; Counter <= UDataCacheContentsDT.Rows.Count - 1; Counter += 1)
                {
                    ReturnValue = ReturnValue + UDataCacheContentsDT[Counter].TableSize;
                }
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".get_CacheSize released the ReaderLock.");
                }
#endif
            }
            return ReturnValue;
        }

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public Int32 Get_MaxCacheSize()
        {
            return UMaxCacheSize;
        }

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public void Set_MaxCacheSize(Int32 AValue)
        {
            UMaxCacheSize = AValue;
            ShrinkCacheToMaxSize();
        }

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public TimeSpan Get_MaxTimeInCache()
        {
            return UMaxTimeInCache;
        }

        /// <summary>
        /// Property accessor
        /// </summary>
        /// <returns>void</returns>
        public void Set_MaxTimeInCache(TimeSpan AValue)
        {
            UMaxTimeInCache = AValue;
            RemoveOldestCachedTables();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the passed in DataTable to the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache</param>
        /// <returns>HashCode of the DataTable
        /// </returns>
        public String AddCachedTable(DataTable ACacheableTable)
        {
            return AddCachedTable(ACacheableTable.TableName, ACacheableTable);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable. This is the name
        /// by which the DataTable will be identified in the Cache. Also, the TableName
        /// property of the DataTable is set to this</param>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache</param>
        /// <returns>HashCode of the DataTable
        /// </returns>
        public String AddCachedTable(String ACacheableTableName, DataTable ACacheableTable)
        {
            return AddCachedTableInternal(ACacheableTableName, ACacheableTable, false);
        }

        /// <summary>
        /// Returns a DataTable from the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <param name="AType"></param>
        /// <returns>DataTable from the Cache</returns>
        /// <exception cref="ECacheableTablesMgrTableNotUpToDateException">if the Cacheable
        /// DataTable isn't in an up-to-date state. This means it needs to be retrieved
        /// anew before it can be used
        /// </exception>
        public DataTable GetCachedDataTable(String ACacheableTableName, out System.Type AType)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR;
            DataTable TmpTable;

            System.Type CachedDataTableType;
            LockCookie UpgradeLockCookie = new LockCookie();

            // Variable initialisation (just to prevent compiler warnings)
            TmpTable = new DataTable();
            CachedDataTableType = new System.Data.DataTable().GetType();
            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTable waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTable grabbed a ReaderLock.");
                }
#endif

                if (!UDataCacheDataSet.Tables.Contains(ACacheableTableName))
                {
                    throw new ECacheableTablesMgrException(
                        "TCacheableTablesManager.GetCachedDataTable: Cacheable DataTable '" + ACacheableTableName + "' does not exist in Cache");
                }

                ContentsEntryDR = GetContentsEntry(ACacheableTableName); // GetContentsEntry reuses the ReaderLock

                if (ContentsEntryDR != null)
                {
                    if (ContentsEntryDR.DataUpToDate)
                    {
                        try
                        {
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".GetCachedDataTable waiting for upgrading to a WriterLock...");
                            }
#endif

                            // Need to temporarily upgrade to a write lock to prevent other threads from obtaining a read lock on the cache table while we are modifying the Cache Contents table!
                            UpgradeLockCookie = FReadWriteLock.UpgradeToWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".GetCachedDataTable upgraded to a WriterLock.");
                            }
#endif
                            ContentsEntryDR.LastAccessed = DateTime.Now;
                        }
                        finally
                        {
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".GetCachedDataTable waiting for downgrading to a ReaderLock...");
                            }
#endif

                            // Downgrade from a WriterLock to a ReaderLock again!
                            FReadWriteLock.DowngradeFromWriterLock(ref UpgradeLockCookie);
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".GetCachedDataTable downgraded to a ReaderLock.");
                            }
#endif
                        }
                    }
                    else
                    {
                        throw new ECacheableTablesMgrTableNotUpToDateException(ACacheableTableName);
                    }
                }

                /*
                 * To get around multi-threading reading/writing issues that might occur
                 * when the caller of this function performs read or write operations on the
                 * DataTable, we must return only a *copy* of the DataTable, not a reference
                 * to the DataTable!
                 */
                TmpTable = UDataCacheDataSet.Tables[ACacheableTableName].Copy();
                CachedDataTableType = UDataCacheDataSet.Tables[ACacheableTableName].GetType();
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTable released the ReaderLock.");
                }
#endif
            }

            if (TmpTable is TTypedDataTable)
            {
                // $IFDEF DEBUGMODE  if TLogging.DL >= 7 then TLogging.Log('Calling ChangeDataTableToTypedDataTable...'); $ENDIF
                // The Copy needs to be a typed DataTable, so we need to type it
                DataUtilities.ChangeDataTableToTypedDataTable(ref TmpTable, CachedDataTableType, "");
            }

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                TLogging.Log(this.GetType().FullName + ".GetCachedDataTable: Returned Type: " + TmpTable.GetType().FullName);
            }
#endif
            AType = TmpTable.GetType();
            return TmpTable;
        }

        /// <summary>
        /// Returns the Hash Code of a DataTable in the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>Hash Code of the DataTable in the Cache</returns>
        /// <exception cref="ECacheableTablesMgrException">If the DataTable doesn't exist in the
        /// Cache (=has no entry in the 'Table of Contents' of the Cache)
        /// </exception>
        public String GetCachedDataTableHash(String ACacheableTableName)
        {
            String ReturnValue;
            CacheableTablesTDSContentsRow ContentsEntryDR;

            ContentsEntryDR = GetContentsEntry(ACacheableTableName);

            if (ContentsEntryDR != null)
            {
                ReturnValue = ContentsEntryDR.HashCode;
            }
            else
            {
                throw new ECacheableTablesMgrException(
                    this.GetType().FullName + ".GetCachedDataTableHash: Cacheable DataTable '" + ACacheableTableName +
                    "' does not exist in Cache Contents Table");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the TableSize of a DataTable in the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>TableSize of the DataTable in the Cache. This size (in bytes) is an
        /// approximated size since it isn't possible to get the size in memory of a
        /// DataTable object. It is the String Length of all columns' values of all
        /// DataRows</returns>
        /// <exception cref="ECacheableTablesMgrException">if the DataTable doesn't exist in the
        /// Cache
        /// </exception>
        public Int32 GetCachedDataTableSize(String ACacheableTableName)
        {
            Int32 ReturnValue;
            CacheableTablesTDSContentsRow ContentsEntryDR;

            ContentsEntryDR = GetContentsEntry(ACacheableTableName);

            if (ContentsEntryDR != null)
            {
                ReturnValue = ContentsEntryDR.TableSize;
            }
            else
            {
                throw new ECacheableTablesMgrException(
                    this.GetType().FullName + ".GetCachedDataTableSize: Cacheable DataTable '" + ACacheableTableName +
                    "' does not exist in Cache Contents Table");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the System.Type of a DataTable in the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>Type of the DataTable in the Cache. This is only useful for Typed
        /// DataTables</returns>
        /// <exception cref="ECacheableTablesMgrException">if the DataTable doesn't exist in the
        /// Cache
        /// </exception>
        public System.Type GetCachedDataTableType(String ACacheableTableName)
        {
            System.Type ReturnValue;
            ReturnValue = new object().GetType(); // just to get rid of Compiler warning...
            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTableType waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTableType grabbed a ReaderLock.");
                }
#endif

                if (!UDataCacheDataSet.Tables.Contains(ACacheableTableName))
                {
                    throw new ECacheableTablesMgrException(
                        this.GetType().FullName + ".GetCachedDataTableType: Cacheable DataTable '" + ACacheableTableName +
                        "' does not exist in Cache");
                }

                ReturnValue = UDataCacheDataSet.Tables[ACacheableTableName].GetType();
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".GetCachedDataTableType released the ReaderLock.");
                }
#endif
            }
            return ReturnValue;
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, the
        /// DataTable in the Cache is replaced with the one that is passed in.
        ///
        /// </summary>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void AddOrRefreshCachedTable(DataTable ACacheableTable, System.Int32 AClientID)
        {
            AddOrRefreshCachedTable(ACacheableTable.TableName, ACacheableTable, AClientID);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, the
        /// DataTable in the Cache is replaced with the one that is passed in.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">DataTable that should be added to the Cache/merged</param>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void AddOrRefreshCachedTable(String ACacheableTableName, DataTable ACacheableTable, System.Int32 AClientID)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR;

            ContentsEntryDR = GetContentsEntry(ACacheableTableName);

            if (ContentsEntryDR != null)
            {
                AddCachedTableInternal(ACacheableTableName, ACacheableTable, true);

                // Inform all Clients (except the one that calls this function) that they
                // need to refresh their Clientside cached DataTable!
                UpdateCacheOnClients(ACacheableTableName, AClientID);
            }
            else
            {
                AddCachedTableInternal(ACacheableTableName, ACacheableTable, false);
            }
        }

        /// <summary>
        /// Replaces a DataTable in the Cache with the one that is passed in.
        ///
        /// </summary>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void RefreshCachedTable(DataTable ACacheableTable, System.Int32 AClientID)
        {
            RefreshCachedTable(ACacheableTable.TableName, ACacheableTable, AClientID);
        }

        /// <summary>
        /// Replaces a DataTable in the Cache with the one that is passed in.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void RefreshCachedTable(String ACacheableTableName, DataTable ACacheableTable, System.Int32 AClientID)
        {
            AddCachedTableInternal(ACacheableTableName, ACacheableTable, true);
            UpdateCacheOnClients(ACacheableTableName, AClientID);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, a Merge
        /// operation is done.
        ///
        /// </summary>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void AddOrMergeCachedTable(DataTable ACacheableTable, System.Int32 AClientID)
        {
            AddOrMergeCachedTable(ACacheableTable.TableName, ACacheableTable, AClientID);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, a Merge
        /// operation is done.
        ///
        /// </summary>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure</param>
        /// <param name="AFilterCriteria">Filter Criteria (eg. Ledger Number for the Finance
        /// Module) that will be needed by the Clients that receive the ClientTask to
        /// be able to request the update of the filtered Cached DataTable
        /// </param>
        /// <returns>void</returns>
        public void AddOrMergeCachedTable(DataTable ACacheableTable, System.Int32 AClientID, object AFilterCriteria)
        {
            AddOrMergeCachedTable(ACacheableTable.TableName, ACacheableTable, AClientID, AFilterCriteria);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, a Merge
        /// operation is done.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure
        /// </param>
        /// <returns>void</returns>
        public void AddOrMergeCachedTable(String ACacheableTableName, DataTable ACacheableTable, System.Int32 AClientID)
        {
            AddOrMergeCachedTable(ACacheableTableName, ACacheableTable, AClientID, null);
        }

        /// <summary>
        /// Adds the passed in DataTable to the Cache. If it is already there, a Merge
        /// operation is done.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <param name="ACacheableTable">DataTable that should be added to the Cache/merged
        /// with an already existing DataTable in the Cache with the same TableName</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting a ClientTask
        /// queued for updating of the Cached DataTable (only if a Merge operation is
        /// done). This would be the ClientID of the Client that performed the call
        /// to this procedure</param>
        /// <param name="AFilterCriteria">Filter Criteria (eg. Ledger Number for the Finance
        /// Module) that will be needed by the Clients that receive the ClientTask to
        /// be able to request the update of the filtered Cached DataTable
        /// </param>
        /// <returns>void</returns>
        public void AddOrMergeCachedTable(String ACacheableTableName, DataTable ACacheableTable, System.Int32 AClientID, object AFilterCriteria)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR;
            DataTable TmpDT;

            ContentsEntryDR = GetContentsEntry(ACacheableTableName);

            if (ContentsEntryDR != null)
            {
                try
                {
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".AddOrMergeCachedTable waiting for a WriterLock...");
                    }
#endif

                    // Prevent other threads from obtaining a read lock on the cache table while we are merging the cache table!
                    FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".AddOrMergeCachedTable grabbed a WriterLock.");
                    }
#endif

                    // $IFDEF DEBUGMODE Thread.Sleep(10000); $ENDIF  Enable this ONLY for checking of correctness of multithreading cache access when debugging!!!
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        TLogging.Log(
                            this.GetType().FullName + ".AddOrMergeCachedTable: merging DataTable " + ACacheableTableName +
                            ". Rows before merging: " +
                            UDataCacheDataSet.Tables[ACacheableTableName].Rows.Count.ToString());
                    }
#endif
                    TmpDT = ACacheableTable.Copy();
                    TmpDT.TableName = ACacheableTableName;
                    UDataCacheDataSet.Merge(TmpDT);
                    ContentsEntryDR.DataUpToDate = true;
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        TLogging.Log(
                            this.GetType().FullName + ".AddOrMergeCachedTable: merged DataTable " + ACacheableTableName + ". Rows after merging: " +
                            UDataCacheDataSet.Tables[ACacheableTableName].Rows.Count.ToString());
                    }
#endif
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    FReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".AddOrMergeCachedTable released the WriterLock.");
                    }
#endif
                }

                // Inform all Clients (except the one that calls this function) that they
                // need to refresh their Clientside cached DataTable!
                UpdateCacheOnClients(ACacheableTableName, AClientID, AFilterCriteria);
            }
            else
            {
                AddCachedTableInternal(ACacheableTableName, ACacheableTable, false);
            }
        }

        /// <summary>
        /// Returns true if the DataTable with the specified TableName is in the
        /// DataCache and does not need refreshing.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>true if the DataTable with the specified TableName is in the
        /// DataCache and is uptodate, otherwise false
        /// </returns>
        public Boolean IsTableCached(String ACacheableTableName)
        {
            Boolean ReturnValue;

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                TLogging.Log(
                    this.GetType().FullName + ".IsTableCached: got called in AppDomain '" + AppDomain.CurrentDomain.FriendlyName +
                    "'. Instance hash is " + this.GetHashCode().ToString());
            }
#endif

            // Thread.GetDomain.FriendlyName
            CacheableTablesTDSContentsRow contentsRow = GetContentsEntry(ACacheableTableName);

            if (contentsRow != null)
            {
                ReturnValue = contentsRow.DataUpToDate;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Marks a DataTable in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was orginally placed in the DataTable).
        ///
        /// This will lead to ECacheableTablesMgrTableNotUpToDateException being thrown
        /// when GetCachedDataTable is called for this DataTable. This tells the caller
        /// of GetCachedDataTable that the DataTable needs to be retrieved anew before
        /// it can be used.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>void</returns>
        /// <exception cref="ECacheableTablesMgrException">if the DataTable doesn't exist in the
        /// Cache
        /// </exception>
        public void MarkCachedTableNeedsRefreshing(String ACacheableTableName)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR = null;

            if (!UDataCacheDataSet.Tables.Contains(ACacheableTableName))
            {
                // add an empty table so we can mark it as invalid
                AddOrRefreshCachedTable(ACacheableTableName, new DataTable(), -1);
            }

            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing grabbed a ReaderLock.");
                }
#endif

                ContentsEntryDR = GetContentsEntry(ACacheableTableName); // GetContentsEntry reuses the ReaderLock
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing released the ReaderLock.");
                }
#endif
            }

            if (ContentsEntryDR != null)
            {
                try
                {
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing waiting for a WriterLock...");
                    }
#endif

                    // Prevent other threads from obtaining a read lock on the cache table while we are modifying the Contents table!
                    FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing grabbed a WriterLock.");
                    }
#endif
                    ContentsEntryDR.DataUpToDate = false;
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    FReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".MarkCachedTableNeedsRefreshing released the WriterLock.");
                    }
#endif
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds/replaces the passed in DataTable to/in the Cache DataSet.
        ///
        /// A HashCode and a TableSize are calculated and an entry in the 'Table of
        /// Contents' DataTable of the Cache is made/updated.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable. This is the name
        /// by which the DataTable will be identified in the Cache. Also, the TableName
        /// property of the DataTable is set to this</param>
        /// <param name="ACacheableTable">DataTable that should be added to/replaced in the
        /// Cache</param>
        /// <param name="AReplaceExistingTable">If true, the function replaces a DataTable with
        /// the same name instead of adding it</param>
        /// <returns>HashCode of the DataTable</returns>
        /// <exception cref="ECacheableTablesMgrException">If ACacheableTable is nil; if
        /// ACacheableTableName is an empty String (''); if the DataTable already
        /// exists in the Cache and AReplaceExistingTable is false; if the DataTable
        /// doesn't exist in the Cache and AReplaceExistingTable is true
        /// </exception>
        private String AddCachedTableInternal(String ACacheableTableName, DataTable ACacheableTable, Boolean AReplaceExistingTable)
        {
            CacheableTablesTDSContentsRow NewCacheTableRow;
            Int32 TableSize = 0;
            string TableHash = "";
            CacheableTablesTDSContentsRow ContentsEntryDR;
            Boolean WriteLockTakenOut = false;

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                TLogging.Log(
                    "TCacheableTablesManager.AddCachedTableInternal: got called in AppDomain '" + AppDomain.CurrentDomain.FriendlyName +
                    "'. Instance hash is " + this.GetHashCode().ToString());
            }
#endif

            // Thread.GetDomain.FriendlyName
            if (ACacheableTable == null)
            {
                throw new ECacheableTablesMgrException(
                    "TCacheableTablesManager.AddCachedTableInternal: Cacheable DataTable '" + ACacheableTableName +
                    "' that is to be added must not be null");
            }

            if ((ACacheableTableName == null) || (ACacheableTableName == ""))
            {
                throw new ECacheableTablesMgrException(
                    "TCacheableTablesManager.AddCachedTableInternal: ACacheableTableName argument must not be nil or empty String");
            }

            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal grabbed a ReaderLock.");
                }
#endif

                if (((!AReplaceExistingTable)) && (UDataCacheDataSet.Tables.Contains(ACacheableTableName)))
                {
                    throw new ECacheableTablesMgrException(
                        "TCacheableTablesManager.AddCachedTableInternal: Cacheable DataTable '" + ACacheableTableName +
                        "' is already in Cache. Set the AReplaceExistingTable Argument to true to re-submit a Cachable DataTable to the Cache.");
                }

                if (AReplaceExistingTable)
                {
                    // replace the cached DataTable with the one passed in
                    RemoveCachedTable(ACacheableTableName, false);
                }
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal released the ReaderLock.");
                }
#endif
            }
            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal waiting for a WriterLock...");
                }
#endif

                // Prevent other threads from obtaining a read lock on the cache table while we are adding the cache table!
                FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                WriteLockTakenOut = true;
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal grabbed a WriterLock.");
                }
#endif

                // add the passed in DataTable to the Cache DataSet
                try
                {
                    UDataCacheDataSet.Tables.Add((DataTable)ACacheableTable);
                }
                catch (System.InvalidCastException)
                {
                    // problem with Mono: https://bugzilla.novell.com/show_bug.cgi?id=521951 Cannot cast from source type to destination type
                    // it happens after the table has been added, so should not cause any problems
                }

                UDataCacheDataSet.Tables[ACacheableTable.TableName].TableName = ACacheableTableName;

                if (ACacheableTable.Rows.Count != 0)
                {
                    DataUtilities.CalculateHashAndSize(ACacheableTable, out TableHash, out TableSize);
                }
                else
                {
                    TableSize = 0;
                    TableHash = "";
                }

                if (!AReplaceExistingTable)
                {
                    // Add new entry to the Cache Contents Table
                    NewCacheTableRow = UDataCacheContentsDT.NewRowTyped(true);
                    NewCacheTableRow.TableName = ACacheableTableName;
                    NewCacheTableRow.DataUpToDate = true;
                    NewCacheTableRow.DataChanged = false;
                    NewCacheTableRow.ChangesSavedExternally = false;
                    NewCacheTableRow.CachedSince = DateTime.Now;
                    NewCacheTableRow.LastAccessed = DateTime.Now;
                    NewCacheTableRow.HashCode = TableHash;
                    NewCacheTableRow.TableSize = TableSize;
                    UDataCacheContentsDT.Rows.Add(NewCacheTableRow);
#if DEBUGMODE
                    if (TLogging.DL >= 9)
                    {
                        TLogging.Log(
                            this.GetType().FullName + ".AddCachedTableInternal: added DataTable '" + NewCacheTableRow.TableName +
                            "' to the Cache.  HashCode: " + TableHash + "; TableSize: " + TableSize.ToString());
                    }
#endif
                }
                else
                {
                    // We need to release the WriterLock because GetContentsEntry wants to
                    // acquire a ReaderLock, which won't work with an open WriterLock...
                    FReadWriteLock.ReleaseWriterLock();
                    WriteLockTakenOut = false;
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal released a WriterLock.");
                    }
#endif

                    // Update entry in the Cache Contents Table
                    ContentsEntryDR = GetContentsEntry(ACacheableTableName);

                    if (ContentsEntryDR != null)
                    {
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal waiting for a WriterLock...");
                        }
#endif

                        // Prevent other threads from obtaining a read lock on the cache table while we are adding the cache table!
                        FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                        WriteLockTakenOut = true;
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal grabbed a WriterLock.");
                        }
#endif
                        ContentsEntryDR.DataUpToDate = true;
                        ContentsEntryDR.DataChanged = false;
                        ContentsEntryDR.ChangesSavedExternally = false;
                        ContentsEntryDR.HashCode = TableHash;
                        ContentsEntryDR.TableSize = TableSize;
#if DEBUGMODE
                        if (TLogging.DL >= 9)
                        {
                            TLogging.Log(
                                this.GetType().FullName + ".AddCachedTableInternal: DataTable '" + ACacheableTableName +
                                "' already exists in the Cache --> replaced it with the passed in DataTable.");
                        }
#endif
                    }
                    else
                    {
                        throw new ECacheableTablesMgrException(
                            this.GetType().FullName + ".AddCachedTableInternal: DataTable '" + ACacheableTableName +
                            "' does not exist in Cache Contents Table");
                    }
                }
            }
            finally
            {
                if (WriteLockTakenOut)
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    FReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".AddCachedTableInternal released the WriterLock.");
                    }
#endif
                }
            }

            if (ACacheableTable.Rows.Count != 0)
            {
                CacheSizeManagement();
            }

            return TableHash;
        }

        /// <summary>
        /// Manages the size (=memory consumption) of the Cache.
        ///
        /// Calls ShrinkCacheToMaxSize if MaxCacheSize Property is set and
        /// RemoveOldestCachedTables if MaxTimeInCache Property is set
        ///
        /// </summary>
        /// <returns>void</returns>
        private void CacheSizeManagement()
        {
            if (UMaxCacheSize != -1)
            {
                ShrinkCacheToMaxSize();
            }

            if (UMaxTimeInCache != TimeSpan.MinValue)
            {
                RemoveOldestCachedTables();
            }
        }

        /// <summary>
        /// Marks a DataTable as being saved to the external source of the data and
        /// having no changes to it.
        ///
        /// This will lead to ECacheableTablesMgrTableNotUpToDateException beeing thrown
        /// when GetCachedDataTable is called for this DataTable. This tells the caller
        /// of GetCachedDataTable that the DataTable needs to be retrieved anew before
        /// it can be used.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <returns>void</returns>
        /// <exception cref="ECacheableTablesMgrException">if the DataTable doesn't exist in the
        /// Cache
        /// </exception>
        public void MarkChangedCacheTableSaved(String ACacheableTableName)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR;

            ContentsEntryDR = GetContentsEntry(ACacheableTableName);

            if (ContentsEntryDR != null)
            {
                ContentsEntryDR.DataChanged = false;
                ContentsEntryDR.ChangesSavedExternally = true;
            }
            else
            {
                throw new ECacheableTablesMgrException(
                    this.GetType().FullName + ".MarkChangedCacheTableSaved: Cacheable DataTable '" + ACacheableTableName +
                    "' does not exist in Cache Contents Table");
            }
        }

        /// <summary>
        /// Returns the 'Table of Contents' entry for the specified Cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable</param>
        /// <returns>'Table of Contents' row of the Cacheable DataTable
        /// </returns>
        private CacheableTablesTDSContentsRow GetContentsEntry(String ACacheableTableName)
        {
            CacheableTablesTDSContentsRow ReturnValue;
            Boolean ReaderLockWasHeld;

            // Variable initialisation (just to prevent compiler warnings)
            ReaderLockWasHeld = false;
            try
            {
                if (!FReadWriteLock.IsReaderLockHeld)
                {
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".GetContentsEntry waiting for a ReaderLock...");
                    }
#endif

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".GetContentsEntry grabbed a ReaderLock.");
                    }
#endif
                }
                else
                {
                    ReaderLockWasHeld = true;
                }

                ReturnValue = (CacheableTablesTDSContentsRow)UDataCacheContentsDT.Rows.Find(ACacheableTableName);
            }
            finally
            {
                if (!ReaderLockWasHeld)
                {
                    // Release read lock
                    FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".GetContentsEntry released the ReaderLock.");
                    }
#endif
                }
            }

            if (ReturnValue != null)
            {
#if DEBUGMODE
                if (TLogging.DL >= 9)
                {
                    TLogging.Log(
                        this.GetType().FullName + ".GetContentsEntry: checked for DataTable '" + ACacheableTableName +
                        "' whether it is in the Cache. Result: is in Cache.");
                }
#endif
            }
            else
            {
#if DEBUGMODE
                if (TLogging.DL >= 9)
                {
                    TLogging.Log(
                        this.GetType().FullName + ".GetContentsEntry: checked for DataTable '" + ACacheableTableName +
                        "' whether it is in the Cache. Result: is NOT in Cache.");
                }
#endif
            }

            return ReturnValue;
        }

        /// <summary>
        /// Removes the specified DataTable from the Cache.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable.</param>
        /// <param name="ARemoveContentsEntry">If true, the DataTable gets removed and its entry
        /// in the Contents Table as well. If false, the Contents Table entry is not
        /// removed (this is needed only in certain circumstances and should not be done
        /// normally).</param>
        /// <returns>void</returns>
        /// <exception cref="ECacheableTablesMgrException">If the DataTable doesn't exist in the
        /// Cache or has no entry in the 'Table of Contents' of the Cache
        /// </exception>
        private void RemoveCachedTable(String ACacheableTableName, Boolean ARemoveContentsEntry)
        {
            CacheableTablesTDSContentsRow ContentsEntryDR;
            Boolean ReaderLockWasHeld;
            Boolean WriteLockTakenOut;
            LockCookie UpgradeLockCookie = new LockCookie();

            // Variable initialisation (just to prevent compiler warnings)
            ContentsEntryDR = null;
            ReaderLockWasHeld = false;
            try
            {
                ReaderLockWasHeld = false;

                if (!FReadWriteLock.IsReaderLockHeld)
                {
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".RemoveCachedTable waiting for a ReaderLock...");
                    }
#endif

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".RemoveCachedTable grabbed a ReaderLock.");
                    }
#endif
                }
                else
                {
                    ReaderLockWasHeld = true;
                }

                if (!UDataCacheDataSet.Tables.Contains(ACacheableTableName))
                {
                    throw new ECacheableTablesMgrException(
                        this.GetType().FullName + ".RemoveCachedTable: Cacheable DataTable '" + ACacheableTableName + "' does not exist in Cache");
                }

                ContentsEntryDR = GetContentsEntry(ACacheableTableName); // GetContentsEntry reuses the ReaderLock
            }
            finally
            {
                if (!ReaderLockWasHeld)
                {
                    // Release read lock
                    FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".RemoveCachedTable released the ReaderLock.");
                    }
#endif
                }
            }

            if (ContentsEntryDR != null)
            {
                WriteLockTakenOut = false;
                try
                {
                    if (!ReaderLockWasHeld)
                    {
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".RemoveCachedTable waiting for a WriterLock...");
                        }
#endif

                        // Prevent other threads from obtaining a read lock on the cache table while we are removing the cache table!
                        FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".RemoveCachedTable grabbed a WriterLock.");
                        }
#endif
                    }
                    else
                    {
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".RemoveCachedTable waiting for upgrading to a WriterLock...");
                        }
#endif

                        // Need to temporarily upgrade to a write lock to prevent other threads from obtaining a read lock on the cache table while we are (re)loading the cache table!
                        UpgradeLockCookie = FReadWriteLock.UpgradeToWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                        if (TLogging.DL >= 10)
                        {
                            TLogging.Log(this.GetType().FullName + ".RemoveCachedTable upgraded to a WriterLock.");
                        }
#endif
                    }

                    WriteLockTakenOut = true;
                    UDataCacheDataSet.Tables.Remove(ACacheableTableName);

                    if (ARemoveContentsEntry)
                    {
                        // remove Contents entry for the Cached DataTable
                        UDataCacheContentsDT.Rows.Remove(ContentsEntryDR);
                    }
                }
                finally
                {
                    if (WriteLockTakenOut)
                    {
                        if (!ReaderLockWasHeld)
                        {
                            // Other threads are now free to obtain a read lock on the cache table.
                            FReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".RemoveCachedTable released the WriterLock.");
                            }
#endif
                        }
                        else
                        {
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".RemoveCachedTable waiting for downgrading to a ReaderLock...");
                            }
#endif

                            // Downgrade from a WriterLock to a ReaderLock again!
                            FReadWriteLock.DowngradeFromWriterLock(ref UpgradeLockCookie);
#if DEBUGMODE
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log(this.GetType().FullName + ".RemoveCachedTable downgraded to a ReaderLock.");
                            }
#endif
                        }
                    }
                }
            }
            else
            {
                throw new ECacheableTablesMgrException(
                    this.GetType().FullName + ".RemoveCachedTable: Cacheable DataTable '" + ACacheableTableName +
                    "' does not exist in Cache Contents Table");
            }
        }

        private void RemoveCachedTable(String ACacheableTableName)
        {
            RemoveCachedTable(ACacheableTableName, true);
        }

        /// <summary>
        /// Reduces the size (=memory consumption) of the Cache by removing DataTables
        /// that are longer in the Cache than MaxTimeInCache specifies.
        ///
        /// @TODO Needs testing.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void RemoveOldestCachedTables()
        {
            int Counter;
            TimeSpan CacheTableAge;

            try
            {
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".RemoveOldestCachedTables waiting for a ReaderLock...");
                }
#endif

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".RemoveOldestCachedTables grabbed a ReaderLock.");
                }
#endif

                for (Counter = 0; Counter <= UDataCacheContentsDT.Rows.Count - 1; Counter += 1)
                {
                    CacheTableAge = DateTime.Now.Subtract(((CacheableTablesTDSContentsRow)UDataCacheContentsDT.Rows[Counter]).CachedSince);

                    if (CacheTableAge > UMaxTimeInCache)
                    {
                        RemoveCachedTable(((CacheableTablesTDSContentsRow)UDataCacheContentsDT.Rows[Counter]).TableName);
                    }
                }
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TLogging.DL >= 10)
                {
                    TLogging.Log(this.GetType().FullName + ".RemoveOldestCachedTables released the ReaderLock.");
                }
#endif
            }
        }

        /// <summary>
        /// Reduces the size (=memory consumption) of the Cache if the Cache size exceeds
        /// the size that is set with the MaxCacheSize Property by removing the largest
        /// DataTables until the size of the Cache is less than MaxCacheSize.
        ///
        /// @TODO Needs testing.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void ShrinkCacheToMaxSize()
        {
            DataView CacheDTByDateDV;
            Int32 Counter;
            Int32 CacheSizeBeforeShrinking;
            Int32 ShrinkedCacheSize;

            CacheSizeBeforeShrinking = Get_CacheSize();

            if (CacheSizeBeforeShrinking > UMaxCacheSize)
            {
                try
                {
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".ShrinkCacheToMaxSize waiting for a ReaderLock...");
                    }
#endif

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".ShrinkCacheToMaxSize grabbed a ReaderLock.");
                    }
#endif

                    // Cache size exceeds the size that is requested > shrink the Cache
                    CacheDTByDateDV = new DataView(UDataCacheContentsDT, "",
                        CacheableTablesTDSContentsTable.GetTableSizeDBName() + " DESC", DataViewRowState.CurrentRows);
                    ShrinkedCacheSize = CacheSizeBeforeShrinking;

                    for (Counter = 0; Counter <= CacheDTByDateDV.Count - 1; Counter += 1)
                    {
                        ShrinkedCacheSize = ShrinkedCacheSize - ((CacheableTablesTDSContentsRow)CacheDTByDateDV[Counter].Row).TableSize;
                        RemoveCachedTable(((CacheableTablesTDSContentsRow)CacheDTByDateDV[Counter].Row).TableName);

                        if (ShrinkedCacheSize <= UMaxCacheSize)
                        {
                            continue;
                        }
                    }
                }
                finally
                {
                    // Release read lock
                    FReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                    if (TLogging.DL >= 10)
                    {
                        TLogging.Log(this.GetType().FullName + ".ShrinkCacheToMaxSize released the ReaderLock.");
                    }
#endif
                }
            }
        }

        /// <summary>
        /// Queues a ClientTask for all other Clients to tell them to reload a certain
        /// Cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable.</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting the
        /// ClientTask queued (this would be the ClientID of the Client that performed
        /// the update request)
        /// </param>
        /// <returns>void</returns>
        private void UpdateCacheOnClients(String ACacheableTableName, System.Int32 AClientID)
        {
            UpdateCacheOnClients(ACacheableTableName, AClientID, null);
        }

        /// <summary>
        /// Queues a ClientTask for all other Clients to tell them to reload a certain
        /// Cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable</param>
        /// <param name="AClientID">The ClientID that should be exempt from getting the
        /// ClientTask queued (this would be the ClientID of the Client that performed
        /// the update request)</param>
        /// <param name="AFilterCriteria">Filter Criteria (eg. Ledger Number for the Finance
        /// Module) that will be needed by the Clients that receive the ClientTask to
        /// be able to request the update of the filtered Cached DataTable
        /// </param>
        /// <returns>void</returns>
        private void UpdateCacheOnClients(String ACacheableTableName, System.Int32 AClientID, object AFilterCriteria)
        {
            if (FDelegateSendClientTask != null)
            {
                // $IFDEF DEBUGMODE if TLogging.DL >= 7 then TLogging.Log(this.GetType().FullName + '.UpdateUserDefaultsOnClient: calling DomainManager.ClientTaskAdd...'); $ENDIF
                FDelegateSendClientTask(-1,
                    SharedConstants.CLIENTTASKGROUP_CACHEREFRESH,
                    ACacheableTableName,
                    AFilterCriteria,
                    null,
                    null,
                    null,
                    1,
                    AClientID);
            }
        }

        #endregion
        #endregion
    }

    /// <summary>
    /// The TCacheableTablesLoader class is designed to get base.by a Class that
    /// will be a Cache Manager (who is responsible for the actual loading and saving
    /// of the DataTables).
    /// It contains only a helper function that is used by every Cache Manager.
    ///
    /// </summary>
    public class TCacheableTablesLoader : object
    {
        /// <summary>Holds reference to an instance of TCacheableTablesManager</summary>
        protected TCacheableTablesManager FCacheableTablesManager;

        /// <summary>Used for passing in an instance of TCacheableTablesManager</summary>
        public TCacheableTablesManager CacheableTablesManager
        {
            get
            {
                return FCacheableTablesManager;
            }

            set
            {
                FCacheableTablesManager = value;
            }
        }

        #region TCacheableTablesLoader

        /// <summary>
        /// Returns a DataTable from the Cache if the HashCode of the DataTable in the
        /// Cache doesn't fit the HashCode that is passed in, otherwise it returns nil.
        ///
        /// </summary>
        /// <param name="ATableName">Name of the DataTable</param>
        /// <param name="AHashCode">HashCode that should be compared against the HashCode of
        /// the DataTable in the Cache</param>
        /// <param name="AType">Type of the DataTable in the Cache. This is only useful for Typed
        /// DataTables</param>
        /// <returns>DataTable from the Cache if the HashCode of the DataTable in the
        /// Cache doesn't fit the HashCode that is passed in, otherwise nil
        /// </returns>
        public DataTable ResultingCachedDataTable(String ATableName, String AHashCode, out System.Type AType)
        {
            DataTable ReturnValue;
            String HashCodeInCacheableTablesManager;

            HashCodeInCacheableTablesManager = FCacheableTablesManager.GetCachedDataTableHash(ATableName);
#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                TLogging.Log(
                    this.GetType().FullName + ".ResultingCachedDataTable: passed in HashCode: " + AHashCode +
                    "; HashCode in CacheableTableManager: " +
                    HashCodeInCacheableTablesManager);
            }
#endif

            if (HashCodeInCacheableTablesManager != "")
            {
                if (HashCodeInCacheableTablesManager != AHashCode)
                {
                    // Passed in HashCode doesn't match the HashCode in the CacheableTablesManager
                    // means that the caller doesn't have the identical DataTable (or an
                    // empty one) > return cached DataTable
                    ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        TLogging.Log(
                            this.GetType().FullName +
                            ".ResultingCachedDataTable: Passed in HashCode doesn't match the HashCode in the CacheableTablesManager --> return cached DataTable");
                    }
#endif
                }
                else
                {
                    // Passed in HashCode matches the HashCode in the CacheableTablesManager
                    // means that the caller has the identical DataTable > return nil
                    AType = FCacheableTablesManager.GetCachedDataTableType(ATableName);
                    ReturnValue = null;
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        TLogging.Log(
                            this.GetType().FullName +
                            ".ResultingCachedDataTable: Passed in HashCode matches the HashCode in the CacheableTablesManager --> return nil");
                    }
#endif
                }
            }
            else
            {
                // HashCode = '' means: Table has no data > return empty cached DataTable!
                ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
#if DEBUGMODE
                if (TLogging.DL >= 7)
                {
                    TLogging.Log(
                        this.GetType().FullName +
                        ".ResultingCachedDataTable: HashCode = '' means: Table has no data -> return empty cached DataTable!");
                }
#endif
            }

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                TLogging.Log(this.GetType().FullName + ".ResultingCachedDataTable: DataTable Type: " + AType.FullName);
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// Returns a DataTable from the Cache if the HashCode of the DataView of the
        /// DataTable in the Cache doesn't fit the HashCode that is passed in, otherwise
        /// it returns nil.
        ///
        /// </summary>
        /// <param name="ATableName">Name of the DataTable</param>
        /// <param name="AHashCode">HashCode that should be compared against the HashCode of
        /// the DataView of the DataTable in the Cache</param>
        /// <param name="ACacheableTableDV">DataView for which HashCode and TableSize should be
        /// calculated</param>
        /// <param name="AType">Type of the DataTable in the Cache. This is only useful for Typed
        /// DataTables</param>
        /// <returns>DataTable from the Cache if the HHashCode of the DataView of the
        /// DataTable in the Cache doesn't fit the HashCode that is passed in,
        /// otherwise nil
        /// </returns>
        public DataTable ResultingCachedDataTable(String ATableName, String AHashCode, DataView ACacheableTableDV, out System.Type AType)
        {
            DataTable ReturnValue;
            String HashCodeInCacheableTablesManager = "";
            Int32 TmpSize = 0;

            DataUtilities.CalculateHashAndSize(ACacheableTableDV, out HashCodeInCacheableTablesManager, out TmpSize);

            if (HashCodeInCacheableTablesManager != AHashCode)
            {
#if DEBUGMODE
                if (TLogging.DL >= 7)
                {
                    TLogging.Log(
                        this.GetType().FullName + ".ResultingCachedDataTable: passed in HashCode: " + AHashCode +
                        "; HashCode in CacheableTableManager: " + HashCodeInCacheableTablesManager);
                }
#endif
                ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
            }
            else
            {
                AType = FCacheableTablesManager.GetCachedDataTableType(ATableName);
                ReturnValue = null;
            }

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                TLogging.Log(this.GetType().FullName + ".ResultingCachedDataTable: DataTable Type: " + AType.FullName);
            }
#endif
            return ReturnValue;
        }

        #endregion
    }

    /// <summary>
    /// This Exception is thrown on several occasions by TCacheableTablesManager.
    /// </summary>
    [Serializable()]
    public class ECacheableTablesMgrException : ApplicationException
    {
        #region ECacheableTablesMgrException

        /// <summary>
        /// </summary>
        public ECacheableTablesMgrException() : base()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="msg"></param>
        public ECacheableTablesMgrException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ECacheableTablesMgrException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }

    /// <summary>
    /// This Exception is thrown by GetCachedDataTable if the Cacheable DataTable
    /// isn't in an up-to-date state. This means it needs to be retrieved anew before
    /// it can be used.
    /// </summary>
    [Serializable()]
    public class ECacheableTablesMgrTableNotUpToDateException : ECacheableTablesMgrException
    {
        #region ECacheableTablesMgrTableNotUpToDateException

        /// <summary>
        /// </summary>
        public ECacheableTablesMgrTableNotUpToDateException() : base()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="msg"></param>
        public ECacheableTablesMgrTableNotUpToDateException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ECacheableTablesMgrTableNotUpToDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}