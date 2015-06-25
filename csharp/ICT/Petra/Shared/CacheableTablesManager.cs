//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Data;
using System.Runtime.Serialization;
using System.Threading;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.Remoting.Shared;

using Ict.Common.Exceptions;

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
    public class TCacheableTablesManager : ICacheableTablesManager
    {
        /// a static instance for this class
        public static TCacheableTablesManager GCacheableTablesManager;

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
                Int32 ReturnValue;
                int Counter;

                ReturnValue = 0;
                try
                {
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.get_CacheSize waiting for a ReaderLock...");

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.get_CacheSize grabbed a ReaderLock.");

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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.get_CacheSize released the ReaderLock.");
                }
                return ReturnValue;
            }
        }

        /// <summary>Number of DataTables in the Cache</summary>
        public Int32 CachedTablesCount
        {
            get
            {
                Int32 ReturnValue;

                try
                {
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.get_CachedTablesCount waiting for a ReaderLock...");

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
//                  TLogging.LogAtLevel(10, this.GetType().FullName + ".get_CachedTablesCount grabbed a ReaderLock.");

                    // Return the number of DataTables in UDataCacheDataSet minus 1 (to account
                    // for the Contents Table)
                    ReturnValue = UDataCacheDataSet.Tables.Count - 1;
                }
                finally
                {
                    // Release read lock
                    FReadWriteLock.ReleaseReaderLock();
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.get_CachedTablesCount released the ReaderLock.");
                }
                return ReturnValue;
            }
        }

        /// <summary>Maximum size that the Cache shouldn't exceed</summary>
        public Int32 MaxCacheSize
        {
            get
            {
                return UMaxCacheSize;
            }

            set
            {
                UMaxCacheSize = value;
                ShrinkCacheToMaxSize();
            }
        }

        /// <summary>Maximum time that a DataTable in the Cache should be cached</summary>
        public TimeSpan MaxTimeInCache
        {
            get
            {
                return UMaxTimeInCache;
            }

            set
            {
                UMaxTimeInCache = value;
                RemoveOldestCachedTables();
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
            TLogging.LogAtLevel(9,
                "TCacheableTablesManager created: Instance hash is " + this.GetHashCode().ToString() + ". AppDomain '" +
                AppDomain.CurrentDomain.FriendlyName + "'.");
            FDelegateSendClientTask = ADelegateSendClientTask;
            FReadWriteLock = new System.Threading.ReaderWriterLock();
        }

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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable waiting for a ReaderLock...");

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable grabbed a ReaderLock.");

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
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable waiting for upgrading to a WriterLock...");

                            // Need to temporarily upgrade to a write lock to prevent other threads from obtaining a read lock on the cache table while we are modifying the Cache Contents table!
                            UpgradeLockCookie = FReadWriteLock.UpgradeToWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable upgraded to a WriterLock.");
                            ContentsEntryDR.LastAccessed = DateTime.Now;
                        }
                        finally
                        {
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable waiting for downgrading to a ReaderLock...");

                            // Downgrade from a WriterLock to a ReaderLock again!
                            FReadWriteLock.DowngradeFromWriterLock(ref UpgradeLockCookie);
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable downgraded to a ReaderLock.");
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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTable released the ReaderLock.");
            }

            if (TmpTable is TTypedDataTable)
            {
                // The Copy needs to be a typed DataTable, so we need to type it
                DataUtilities.ChangeDataTableToTypedDataTable(ref TmpTable, CachedDataTableType, "");
            }

            TLogging.LogAtLevel(7, "TCacheableTablesManager.GetCachedDataTable: Returned Type: " + TmpTable.GetType().FullName);
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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTableType waiting for a ReaderLock...");

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTableType grabbed a ReaderLock.");

                if (!UDataCacheDataSet.Tables.Contains(ACacheableTableName))
                {
                    throw new ECacheableTablesMgrException(
                        "TCacheableTablesManager.GetCachedDataTableType: Cacheable DataTable '" + ACacheableTableName +
                        "' does not exist in Cache");
                }

                ReturnValue = UDataCacheDataSet.Tables[ACacheableTableName].GetType();
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetCachedDataTableType released the ReaderLock.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.AddOrMergeCachedTable waiting for a WriterLock...");

                    // Prevent other threads from obtaining a read lock on the cache table while we are merging the cache table!
                    FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.AddOrMergeCachedTable grabbed a WriterLock.");

                    TLogging.LogAtLevel(7, "TCacheableTablesManager.AddOrMergeCachedTable: merging DataTable " + ACacheableTableName +
                        ". Rows before merging: " + UDataCacheDataSet.Tables[ACacheableTableName].Rows.Count.ToString());
                    TmpDT = ACacheableTable.Copy();
                    TmpDT.TableName = ACacheableTableName;

                    try
                    {
                        UDataCacheDataSet.Merge(TmpDT);
                    }
                    catch (Exception)
                    {
                        // if the column names change, we cannot merge anymore with the table that was loaded from an old cache file
                        UDataCacheDataSet.RemoveTable(TmpDT.TableName);
                        UDataCacheDataSet.Merge(TmpDT);
                    }

                    // Remove rows from the cached DT that are no longer present in the DB Table (DataSet.Merge doesn't do this!).
                    // Note: The Cacheable DataTable must have a Primary Key for this Method to be able to perform this!
                    DataUtilities.RemoveRowsNotPresentInDT(TmpDT, UDataCacheDataSet.Tables[ACacheableTableName], true);

                    ContentsEntryDR.DataUpToDate = true;
                    TLogging.LogAtLevel(7,
                        "TCacheableTablesManager.AddOrMergeCachedTable: merged DataTable " + ACacheableTableName + ". Rows after merging: " +
                        UDataCacheDataSet.Tables[ACacheableTableName].Rows.Count.ToString());
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    FReadWriteLock.ReleaseWriterLock();
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.AddOrMergeCachedTable released the WriterLock.");
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

            TLogging.LogAtLevel(9,
                "TCacheableTablesManager.IsTableCached(String): got called in AppDomain '" + AppDomain.CurrentDomain.FriendlyName +
                "'. Instance hash is " + this.GetHashCode().ToString());

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
        /// Returns true if the DataTable with the specified TableName is in the DataCache.
        /// If it returns true the AIsUpToDate parameter will indicate if the cached table is up-to-date.
        /// This method is required by the client-side manager only.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the DataTable</param>
        /// <param name="AIsUpToDate">Will be true if the table is in the cache and up-to-date. False otherwise</param>
        /// <returns>true if the DataTable with the specified TableName is in the DataCache</returns>
        public Boolean IsTableCached(String ACacheableTableName, out Boolean AIsUpToDate)
        {
            AIsUpToDate = false;

            TLogging.LogAtLevel(9,
                "TCacheableTablesManager.IsTableCached(String, out Boolean): got called in AppDomain '" + AppDomain.CurrentDomain.FriendlyName +
                "'. Instance hash is " + this.GetHashCode().ToString());

            // Thread.GetDomain.FriendlyName
            CacheableTablesTDSContentsRow contentsRow = GetContentsEntry(ACacheableTableName);

            if (contentsRow != null)
            {
                AIsUpToDate = contentsRow.DataUpToDate;
            }

            return contentsRow != null;
        }

        /// <summary>
        /// Marks a DataTable in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing waiting for a ReaderLock...");

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing grabbed a ReaderLock.");

                ContentsEntryDR = GetContentsEntry(ACacheableTableName); // GetContentsEntry reuses the ReaderLock
            }
            finally
            {
                // Release read lock
                FReadWriteLock.ReleaseReaderLock();
                TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing released the ReaderLock.");
            }

            if (ContentsEntryDR != null)
            {
                try
                {
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing waiting for a WriterLock...");

                    // Prevent other threads from obtaining a read lock on the cache table while we are modifying the Contents table!
                    FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing grabbed a WriterLock.");
                    ContentsEntryDR.DataUpToDate = false;
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    FReadWriteLock.ReleaseWriterLock();
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.MarkCachedTableNeedsRefreshing released the WriterLock.");
                }
            }
        }

        /// <summary>
        /// Marks all DataTables in the Cache to be no longer up-to-date (=out of sync
        /// with the data that was originally placed in the DataTable).
        ///
        /// This will lead to ECacheableTablesMgrTableNotUpToDateException being thrown
        /// when GetCachedDataTable is called for this DataTable. This tells the caller
        /// of GetCachedDataTable that the DataTable needs to be retrieved anew before
        /// it can be used.
        ///
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="ECacheableTablesMgrException">if the DataTable doesn't exist in the
        /// Cache
        /// </exception>
        public void MarkAllCachedTableNeedsRefreshing()
        {
            foreach (DataTable CachedTable in UDataCacheDataSet.Tables)
            {
                MarkCachedTableNeedsRefreshing(CachedTable.TableName);
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

            TLogging.LogAtLevel(
                9,
                "TCacheableTablesManager.AddCachedTableInternal: got called in AppDomain '" + AppDomain.CurrentDomain.FriendlyName +
                "'. Instance hash is " + this.GetHashCode().ToString());

            // Thread.GetDomain.FriendlyName
            if (ACacheableTable == null)
            {
                throw new ECacheableTablesMgrException(
                    "TCacheableTablesManager.AddCachedTableInternal: Cacheable DataTable '" + ACacheableTableName + "' to be added must not be null");
            }

            if ((ACacheableTableName == null) || (ACacheableTableName == ""))
            {
                throw new ECacheableTablesMgrException(
                    "TCacheableTablesManager.AddCachedTableInternal: ACacheableTableName argument must not be null or empty String");
            }

            try
            {
                TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal waiting for a ReaderLock...");

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal grabbed a ReaderLock.");

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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal released the ReaderLock.");
            }
            try
            {
                TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal waiting for a WriterLock...");

                // Prevent other threads from obtaining a read lock on the cache table while we are adding the cache table!
                FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                WriteLockTakenOut = true;
                TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal grabbed a WriterLock.");

                if (ACacheableTable.DataSet != null)
                {
                    // TODORemoting: should we solve this problem in a better way?
                    // TLogging.Log("TCacheableTablesManager: warning: table " + ACacheableTable.TableName + " already belongs to " + ACacheableTable.DataSet.DataSetName);
                    ACacheableTable = ACacheableTable.Copy();
                }

                // add the passed in DataTable to the Cache DataSet
                UDataCacheDataSet.Tables.Add(ACacheableTable);

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
                    TLogging.LogAtLevel(9, "TCacheableTablesManager.AddCachedTableInternal: added DataTable '" + NewCacheTableRow.TableName +
                        "' to the Cache.  HashCode: " + TableHash + "; TableSize: " + TableSize.ToString());
                }
                else
                {
                    // We need to release the WriterLock because GetContentsEntry wants to
                    // acquire a ReaderLock, which won't work with an open WriterLock...
                    FReadWriteLock.ReleaseWriterLock();
                    WriteLockTakenOut = false;
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal released a WriterLock.");

                    // Update entry in the Cache Contents Table
                    ContentsEntryDR = GetContentsEntry(ACacheableTableName);

                    if (ContentsEntryDR != null)
                    {
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal waiting for a WriterLock...");

                        // Prevent other threads from obtaining a read lock on the cache table while we are adding the cache table!
                        FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                        WriteLockTakenOut = true;
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal grabbed a WriterLock.");
                        ContentsEntryDR.DataUpToDate = true;
                        ContentsEntryDR.DataChanged = false;
                        ContentsEntryDR.ChangesSavedExternally = false;
                        ContentsEntryDR.HashCode = TableHash;
                        ContentsEntryDR.TableSize = TableSize;
                        TLogging.LogAtLevel(9, "TCacheableTablesManager.AddCachedTableInternal: DataTable '" + ACacheableTableName +
                            "' already exists in the Cache --> replaced it with the passed in DataTable.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.AddCachedTableInternal released the WriterLock.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.GetContentsEntry waiting for a ReaderLock...");

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.GetContentsEntry grabbed a ReaderLock.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.GetContentsEntry released the ReaderLock.");
                }
            }

            if (ReturnValue != null)
            {
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetContentsEntry: checked for DataTable '" + ACacheableTableName +
                    "' whether it is in the Cache. Result: is in Cache.");
            }
            else
            {
                TLogging.LogAtLevel(10, "TCacheableTablesManager.GetContentsEntry: checked for DataTable '" + ACacheableTableName +
                    "' whether it is in the Cache. Result: is NOT in Cache.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable waiting for a ReaderLock...");

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable grabbed a ReaderLock.");
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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable released the ReaderLock.");
                }
            }

            if (ContentsEntryDR != null)
            {
                WriteLockTakenOut = false;
                try
                {
                    if (!ReaderLockWasHeld)
                    {
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable waiting for a WriterLock...");

                        // Prevent other threads from obtaining a read lock on the cache table while we are removing the cache table!
                        FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable grabbed a WriterLock.");
                    }
                    else
                    {
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable waiting for upgrading to a WriterLock...");

                        // Need to temporarily upgrade to a write lock to prevent other threads from obtaining a read lock on the cache table while we are (re)loading the cache table!
                        UpgradeLockCookie = FReadWriteLock.UpgradeToWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
                        TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable upgraded to a WriterLock.");
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
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable released the WriterLock.");
                        }
                        else
                        {
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable waiting for downgrading to a ReaderLock...");

                            // Downgrade from a WriterLock to a ReaderLock again!
                            FReadWriteLock.DowngradeFromWriterLock(ref UpgradeLockCookie);
                            TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveCachedTable downgraded to a ReaderLock.");
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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveOldestCachedTables waiting for a ReaderLock...");

                // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveOldestCachedTables grabbed a ReaderLock.");

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
                TLogging.LogAtLevel(10, "TCacheableTablesManager.RemoveOldestCachedTables released the ReaderLock.");
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

            CacheSizeBeforeShrinking = CacheSize;

            if (CacheSizeBeforeShrinking > UMaxCacheSize)
            {
                try
                {
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.ShrinkCacheToMaxSize waiting for a ReaderLock...");

                    // Try to get a read lock [We don't specify a timeout because reading the DB tables into the cached table should be fairly quick]
                    FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.ShrinkCacheToMaxSize grabbed a ReaderLock.");

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
                    TLogging.LogAtLevel(10, "TCacheableTablesManager.ShrinkCacheToMaxSize released the ReaderLock.");
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
    public class TCacheableTablesLoader
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
            TLogging.LogAtLevel(7, "TCacheableTablesManager.ResultingCachedDataTable: passed in HashCode: " + AHashCode +
                "; HashCode in CacheableTableManager: " + HashCodeInCacheableTablesManager);

            if (HashCodeInCacheableTablesManager != "")
            {
                if (HashCodeInCacheableTablesManager != AHashCode)
                {
                    // Passed in HashCode doesn't match the HashCode in the CacheableTablesManager
                    // means that the caller doesn't have the identical DataTable (or an
                    // empty one) > return cached DataTable
                    ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
                    TLogging.LogAtLevel(
                        7,
                        "TCacheableTablesManager.ResultingCachedDataTable: Passed in HashCode doesn't match the HashCode in the CacheableTablesManager --> return cached DataTable");
                }
                else
                {
                    // Passed in HashCode matches the HashCode in the CacheableTablesManager
                    // means that the caller has the identical DataTable > return nil
                    AType = FCacheableTablesManager.GetCachedDataTableType(ATableName);
                    ReturnValue = null;
                    TLogging.LogAtLevel(
                        7,
                        "TCacheableTablesManager.ResultingCachedDataTable: Passed in HashCode matches the HashCode in the CacheableTablesManager --> return nil");
                }
            }
            else
            {
                // HashCode = '' means: Table has no data > return empty cached DataTable!
                ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
                TLogging.LogAtLevel(7,
                    "TCacheableTablesManager.ResultingCachedDataTable: HashCode = '' means: Table has no data -> return empty cached DataTable!");
            }

            TLogging.LogAtLevel(7, this.GetType().FullName + ".ResultingCachedDataTable: DataTable Type: " + AType.FullName);
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
                TLogging.LogAtLevel(7,
                    "TCacheableTablesManager.ResultingCachedDataTable: passed in HashCode: " + AHashCode +
                    "; HashCode in CacheableTableManager: " + HashCodeInCacheableTablesManager);
                ReturnValue = FCacheableTablesManager.GetCachedDataTable(ATableName, out AType);
            }
            else
            {
                AType = FCacheableTablesManager.GetCachedDataTableType(ATableName);
                ReturnValue = null;
            }

            TLogging.LogAtLevel(7, "TCacheableTablesManager.ResultingCachedDataTable: DataTable Type: " + AType.FullName);
            return ReturnValue;
        }

        #endregion
    }

    #region ECacheableTablesMgrException

    /// <summary>
    /// This Exception is thrown on several occasions by TCacheableTablesManager.
    /// </summary>
    [Serializable()]
    public class ECacheableTablesMgrException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECacheableTablesMgrException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECacheableTablesMgrException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECacheableTablesMgrException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ECacheableTablesMgrException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region ECacheableTablesMgrTableNotUpToDateException

    /// <summary>
    /// This Exception is thrown by GetCachedDataTable if the Cacheable DataTable
    /// isn't in an up-to-date state. This means it needs to be retrieved anew before
    /// it can be used.
    /// </summary>
    [Serializable()]
    public class ECacheableTablesMgrTableNotUpToDateException : ECacheableTablesMgrException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECacheableTablesMgrTableNotUpToDateException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECacheableTablesMgrTableNotUpToDateException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECacheableTablesMgrTableNotUpToDateException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ECacheableTablesMgrTableNotUpToDateException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion
}