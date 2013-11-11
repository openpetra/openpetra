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
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
// using Ict.Petra.Shared.MFinance.Gift;
// using Ict.Petra.Shared.MFinance.Account;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Provides a client-side cache for DataTables of any size.
    ///
    /// Classes or GUI controls that need access to such data just call the
    /// GetCacheable... procedures to get access to the corresponding DataTable.
    ///
    /// The GetCacheable... function figures out whether this particular DataTable
    /// is already in the Client-side cache or not. If it is already there, it just
    /// gives back a reference to the Client-side cache DataSet. If it isn't, it
    /// checks whether it available from a file and whether this file contains
    /// up-to-date data. If it isn't available from file or the file is out of date,
    /// the DataTable is retrieved once from the Petra Server and persisted in a file
    /// before giving back a reference to the Client-side cache DataSet.
    /// </summary>
    public class TDataCache
    {
        /// <summary>Subdirectory under the Folder that .NET determines to be the 'IsolatedStorage' folder for the current User of the Operating System</summary>
        public const String CACHEFILESDIR = "OpenPetra/Cache";

        /// <summary>File Extension for Cacheable DataTables that are persisted in a file</summary>
        public const String CACHEABLEDT_FILE_EXTENSION = ".cdt";

        private const Int16 DEBUGLEVEL_CACHEMESSAGES = 5;

        /// <summary>Holds reference to an instance of TCacheableTablesManager (for caching of DataTables)</summary>
        public static TCacheableTablesManager UCacheableTablesManager = new TCacheableTablesManager(null);

        /// <summary>Holds reference to an instance of the Isolated Storage file system</summary>
        public static IsolatedStorageFile UIsolatedStorageFile = IsolatedStorageFile.GetStore(
            IsolatedStorageScope.User | IsolatedStorageScope.Assembly |
            IsolatedStorageScope.Roaming, null, null);


        #region TDataCache.TMCommon

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMCommon
        {
            /**
             * Returns the chosen DataTable for the Common Namespace from the Cache.
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableCommonTable(TCacheableCommonTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(ACacheableTable.ToString());
            }
        }

        #endregion


        #region TDataCache.TMCommon

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMConference
        {
            /**
             * Returns the chosen DataTable for the Conference Namespace from the Cache.
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableConferenceTable(TCacheableConferenceTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(ACacheableTable.ToString());
            }
        }

        #endregion


        #region TDataCache.TMPartner
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMPartner
        {
            /**
             * Returns the chosen DataTable for the Petra Partner Module, Partner Sub-Module
             * from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheablePartnerTable(TCacheablePartnerTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(ACacheableTable.ToString());
            }

            /**
             * Returns the chosen DataTable for the Petra Partner Module, Subscriptions
             * Sub-Module from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(Enum.GetName(typeof(TCacheableSubscriptionsTablesEnum), ACacheableTable));
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum ACacheableTable)
            {
                DataTable TmpDT;

                // Refresh the Cacheble DataTable on the Serverside and return it
                TRemote.MPartner.Subscriptions.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }

            /**
             * Returns the chosen DataTable for the Petra Partner Module, Mailing
             * Sub-Module from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableMailingTable(TCacheableMailingTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(Enum.GetName(typeof(TCacheableMailingTablesEnum), ACacheableTable));
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheableMailingTable(TCacheableMailingTablesEnum ACacheableTable)
            {
                DataTable TmpDT;

                // Refresh the Cacheble DataTable on the Serverside and return it
                TRemote.MPartner.Mailing.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheablePartnerTable(TCacheablePartnerTablesEnum ACacheableTable)
            {
                DataTable TmpDT;

                // Refresh the Cacheble DataTable on the Serverside and return it
                TRemote.MPartner.Partner.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }
        }

        #endregion


        #region TDataCache.TMFinance

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMFinance
        {
            /**
             * Returns the chosen DataTable for the Petra Finance Module from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable)
            {
                try
                {
                    string CacheableTableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
                    return TDataCache.GetCacheableDataTableFromCache(CacheableTableName);
                }
                catch (System.Runtime.Remoting.RemotingException Exc)
                {
                    // most probably a permission problem: System.Runtime.Remoting.RemotingException: Requested Service not found
                    throw new Exception(Catalog.GetString("You do not have enough permissions to access the Finance module:") + "\n" + Exc.ToString());
                }
            }

            /**
             * Returns the chosen DataTable for the Petra Finance Module from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * This overload of GetCacheableFinanceTable also considers the Ledger Number,
             * and only retrieves the rows based on the given Ledger Number.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @param ALedgerNumber The number of the current ledger that the data should be
             * from
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable, System.Int32 ALedgerNumber)
            {
                DataTable ReturnValue;
                Type DataTableType;

                ReturnValue = null;

                switch (ACacheableTable)
                {
                    case TCacheableFinanceTablesEnum.AccountHierarchyList:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.AccountHierarchyList, AAccountHierarchyTable.GetLedgerNumberDBName(
                            ), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.CostCentreList:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.CostCentreList,
                        ACostCentreTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.AccountList:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.AccountList, AAccountTable.GetLedgerNumberDBName(), ALedgerNumber,
                        out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.AccountingPeriodList:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.AccountingPeriodList, AAccountingPeriodTable.GetLedgerNumberDBName(
                            ), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.LedgerDetails:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerTable.GetLedgerNumberDBName(), ALedgerNumber,
                        out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.MotivationGroupList:
                        ReturnValue = GetBasedOnLedger(ACacheableTable,
                        AMotivationGroupTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.MotivationList:
                        ReturnValue = GetBasedOnLedger(ACacheableTable,
                        AMotivationDetailTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.FeesPayableList:
                        ReturnValue = GetBasedOnLedger(ACacheableTable,
                        AFeesPayableTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.FeesReceivableList:
                        ReturnValue = GetBasedOnLedger(ACacheableTable,
                        AFeesReceivableTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.SuspenseAccountList:
                        ReturnValue = GetBasedOnLedger(ACacheableTable,
                        ASuspenseAccountTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    case TCacheableFinanceTablesEnum.ICHStewardshipList:
                        ReturnValue = GetBasedOnLedger(TCacheableFinanceTablesEnum.ICHStewardshipList,
                        AIchStewardshipTable.GetLedgerNumberDBName(), ALedgerNumber, out DataTableType);
                        break;

                    default:
                        break;
                }

                return ReturnValue;
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and persists it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable)
            {
                DataTable TmpDT;

                // Refresh the Cacheble DataTable on the Serverside and return it
                TRemote.MFinance.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and persists it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             * @param ALedgerNumber The number of the current ledger that the data should be
             * from
             *
             */
            public static void RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum ACacheableTable, System.Int32 ALedgerNumber)
            {
                DataTable TmpDT;

                // Refresh the Cacheble DataTable on the Serverside and return it
                TRemote.MFinance.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, ALedgerNumber, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }

            /**
             * Get rows from a table that are based on a ledger; (e.g. Costcentres, Accounts)
             * The cache will only retrieve data for the one ledger, and check the next time
             * if the data is already there for another ledger
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @param ALedgerColumnDBName The name of the column in this table that has the
             * ledger number
             * @param ALedgerNumber The number of the current ledger that the data should be
             * from
             * @return The table in the cache with data from all ledgers requested till now
             *
             */
            public static DataTable GetBasedOnLedger(TCacheableFinanceTablesEnum ACacheableTable,
                String ALedgerColumnDBName,
                System.Int32 ALedgerNumber,
                out Type ADataTableType)
            {
                try
                {
                    String CacheableTableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
                    String FilterCriteria = ALedgerColumnDBName + " = " + ALedgerNumber.ToString();
                    return TDataCache.GetCacheableDataTableFromCache(CacheableTableName, FilterCriteria, (object)ALedgerNumber, out ADataTableType);
                }
                catch (System.Runtime.Remoting.RemotingException Exc)
                {
                    // most probably a permission problem: System.Runtime.Remoting.RemotingException: Requested Service not found
                    throw new Exception(Catalog.GetString("You do not have enough permissions to access the Finance module:") + "\n" + Exc.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion


        #region TDataCache.TMPersonnel

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMPersonnel
        {
            /**
             * Returns the chosen DataTable for the Petra Partner Module, Partner Sub-Module
             * from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheablePersonnelTable(TCacheablePersonTablesEnum ACacheableTable)
            {
                //return TDataCache.GetCacheableDataTableFromCache(ACacheableTable.ToString());
                return TDataCache.GetCacheableDataTableFromCache(Enum.GetName(typeof(TCacheablePersonTablesEnum), ACacheableTable));
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheablePersonnelTable(TCacheablePersonTablesEnum ACacheableTable)
            {
                // TODO
//                DataTable TmpDT;

                // Refresh the Cacheable DataTable on the Serverside and return it
//                TRemote.MPartner.Partner.Cacheable.RefreshCacheableTable(ACacheableTable, out TmpDT);
//                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);
//                Cache_Lookup.TMPartner.RefreshCacheablePartnerTable(ACacheableTable);
//
//                // Update the cached DataTable file
//                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }

            /**
             * Returns the chosen DataTable for the Petra Partner Module, Subscriptions
             * Sub-Module from the
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableUnitsTable(TCacheableUnitTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(Enum.GetName(typeof(TCacheableUnitTablesEnum), ACacheableTable));
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheableUnitsTable(TCacheableUnitTablesEnum ACacheableTable)
            {
//                DataTable TmpDT;
// TODO
                // Refresh the Cacheble DataTable on the Serverside and return it
//                TRemote.MPartner.Subscriptions.Cacheable.RefreshCacheableTable(ACacheableTable, out TmpDT);
//                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);
//
//                // Update the cached DataTable file
//                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }
        }

        #endregion


        #region TDataCache.TMSysMan

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMSysMan
        {
            /**
             * Returns the chosen DataTable for the Petra SysMan Module
             *
             * If the DataTable is not available on the Client side, it is automatically
             * retrieved from the Petra Server.
             *
             * @param ACacheableTable The cached DataTable that should be returned in the
             * DataSet
             * @return Chosen DataTable
             *
             */
            public static DataTable GetCacheableSysManTable(TCacheableSysManTablesEnum ACacheableTable)
            {
                return TDataCache.GetCacheableDataTableFromCache(ACacheableTable.ToString());
            }

            /**
             * Tells the PetraServer to reload the cacheable DataTable from the DB,
             * refreshes the DataTable in the client-side Cache and saves it to a file.
             *
             * @param ACacheableTable The cached DataTable that should be reloaded from DB.
             *
             */
            public static void RefreshCacheableSysManTable(TCacheableSysManTablesEnum ACacheableTable)
            {
                DataTable TmpDT;

                // Refresh the Cacheable DataTable on the Serverside and return it
                TRemote.MSysMan.Cacheable.WebConnectors.RefreshCacheableTable(ACacheableTable, out TmpDT);
                UCacheableTablesManager.AddOrRefreshCachedTable(TmpDT, -1);

                // Update the cached DataTable file
                TDataCache.SaveCacheableDataTableToFile(TmpDT);
            }
        }

        #endregion


        /// <summary>
        /// Causes the TDataCache to reload the specified Cache DataTable the next time
        /// it is accessed.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cache Table to be reloaded
        /// </param>
        /// <returns>void</returns>
        public static void ReloadCacheTable(String ACacheableTableName)
        {
            try
            {
                UCacheableTablesManager.MarkCachedTableNeedsRefreshing(ACacheableTableName);
            }
            catch (ECacheableTablesMgrException)
            {
                // Ignore that Exception; it just means that the Cacheable DataTable
                // hasn't been loaded into the local CacheManager yet  so no updating is necessary.

                /* TLogging.Log('TDataCache.ReloadCacheTable: Should refresh Cacheable DataTable ''' + ACacheableTableName + ''', but the DataTable isn''t cached yet > not doing anything (this is expected behaviour and doesn''t mean an error
                 *happened!).'); */
            }
            catch (Exception Exc)
            {
                TLogging.Log("TDataCache.ReloadCacheTable: Exception occured while calling 'MarkCachedTableNeedsRefreshing': " + Exc.ToString());
                throw;
            }
        }

        /// <summary>
        /// Causes the TDataCache to reload the specified Cache Table immediately with
        /// an applied FilterCriteria.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">Name of the Cache Table to be reloaded</param>
        /// <param name="AFilterCriteria">An Object containing the filter criteria value that is
        /// used by the server-side function that retrieves the data for the cacheable
        /// DataTable
        /// </param>
        /// <returns>void</returns>
        public static void ReloadCacheTable(String ACacheableTableName, object AFilterCriteria)
        {
            TCacheableFinanceTablesEnum CacheableMFinanceTable;

            try
            {
                UCacheableTablesManager.MarkCachedTableNeedsRefreshing(ACacheableTableName);
            }
            catch (ECacheableTablesMgrException)
            {
                // Ignore that Exception; it just means that the Cacheable DataTable
                // hasn't been loaded into the local CacheManager yet  so no updating is necessary.

                /* TLogging.Log('TDataCache.ReloadCacheTable: Should refresh Cacheable DataTable ''' + ACacheableTableName + ''', but the DataTable isn''t cached yet > not doing anything (this is expected behaviour and doesn''t mean an error
                 *happened!).'); */
            }
            catch (Exception Exc)
            {
                TLogging.Log("TDataCache.ReloadCacheTable: Exception occured while calling 'MarkCachedTableNeedsRefreshing': " + Exc.ToString());
                throw;
            }

            if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableFinanceTablesEnum)), ACacheableTableName) != -1)
            {
                // MFinance Namespace
                CacheableMFinanceTable = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum), ACacheableTableName);
                try
                {
                    TMFinance.GetCacheableFinanceTable(CacheableMFinanceTable, Convert.ToInt32(AFilterCriteria));
                }
                catch (Exception Exc)
                {
                    TLogging.Log("TDataCache.ReloadCacheTable: Exception occured while calling 'GetCacheableFinanceTable': " + Exc.ToString());
                    throw;
                }

                // AFilterCriteria will be the LedgerNumber
            }
            else
            {
            }
        }

        /// <summary>
        /// Returns an opened Stream for a cached DataTable that was previously
        /// serialized to binary file.
        ///
        /// @comment Uses the '.NET IsolatedStorage' to load the DataTable.
        ///
        /// </summary>
        /// <param name="ATableName">Name of the DataTable that was previously serialized to a
        /// file
        /// </param>
        /// <returns>void</returns>
        public static IsolatedStorageFileStream GetCacheableDataTableFileForReading(String ATableName)
        {
            UIsolatedStorageFile.CreateDirectory(CACHEFILESDIR);
            return new IsolatedStorageFileStream(CACHEFILESDIR + '/' + ATableName + CACHEABLEDT_FILE_EXTENSION,
                FileMode.Open,
                FileAccess.Read,
                UIsolatedStorageFile);
        }

        /// <summary>
        /// Returns an opened Stream for a cached DataTable that should be serialized to
        /// a binary file.
        ///
        /// If the file doesn't exist yet, it is created automatically.
        ///
        /// @comment Uses the '.NET IsolatedStorage' to load the DataTable. The
        /// IsolatedStorageScope is 'Roaming', so the files that are written using this
        /// function will be stored in the Windows User's Roaming Profile (and are
        /// therfore available on every Windows Workstation that the user logs on to
        /// in the same Domain)!
        ///
        /// </summary>
        /// <param name="ATableName">Name of the DataTable that should be serialized to a file
        /// </param>
        /// <returns>void</returns>
        public static IsolatedStorageFileStream GetCacheableDataTableFileForWriting(String ATableName)
        {
            IsolatedStorageFileStream ReturnValue = null;
            bool Success = false;
            int AttemptCounter = 0;

            UIsolatedStorageFile.CreateDirectory(CACHEFILESDIR);

            // For the scenario where multiple Client instances are executed at (nearly) the same time on the
            // same machine: don't fall over in case another client tries to access the file in which the
            // Cacheable DataTable should be written, but wait a bit and retry. That scenario seems a bit
            // unlikely, but it happens when many Clients are launched and are told to open the same screen
            // (e.g. Partner Edit screen) and that happens when they are launched from the PetraMultiStart
            // application.
            while ((AttemptCounter < 5)
                   && (!Success))
            {
                AttemptCounter++;

                try
                {
                    ReturnValue = new IsolatedStorageFileStream(CACHEFILESDIR + '/' + ATableName + CACHEABLEDT_FILE_EXTENSION,
                        FileMode.Create,
                        FileAccess.Write,
                        UIsolatedStorageFile);

                    Success = true;
                }
                catch (System.IO.IOException)
                {
                    Thread.Sleep(200);

                    continue;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Makes the actual PetraServer method call for the retrieval of a cacheable
        /// DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">description1</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable in the client-side cache, or
        /// '' if it isn't in the client-side cache yet (in this case the cacheable</param>
        /// <param name="ACacheableTableSystemType"></param>
        /// <returns>)</returns>
        public static DataTable GetCacheableDataTableFromPetraServer(String ACacheableTableName,
            String AHashCode,
            out System.Type ACacheableTableSystemType)
        {
            ACacheableTableSystemType = null;
            DataTable ReturnValue = null;

            if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableCommonTablesEnum)), ACacheableTableName) != -1)
            {
                // MCommon Namespace
                TCacheableCommonTablesEnum CacheableMCommonTable = (TCacheableCommonTablesEnum)Enum.Parse(typeof(TCacheableCommonTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MCommon.Cacheable.WebConnectors.GetCacheableTable(CacheableMCommonTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableConferenceTablesEnum)), ACacheableTableName) != -1)
            {
                // MConference Namespace
                TCacheableConferenceTablesEnum CacheableMConferenceTable =
                    (TCacheableConferenceTablesEnum)Enum.Parse(typeof(TCacheableConferenceTablesEnum),
                        ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MConference.Cacheable.WebConnectors.GetCacheableTable(CacheableMConferenceTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheablePartnerTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Partner Namespace
                TCacheablePartnerTablesEnum CacheableMPartnerPartnerTable =
                    (TCacheablePartnerTablesEnum)Enum.Parse(typeof(TCacheablePartnerTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Partner.Cacheable.WebConnectors.GetCacheableTable(CacheableMPartnerPartnerTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableSubscriptionsTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Subscriptions Namespace
                TCacheableSubscriptionsTablesEnum CacheableMPartnerSubscriptionsTable =
                    (TCacheableSubscriptionsTablesEnum)Enum.Parse(typeof(TCacheableSubscriptionsTablesEnum),
                        ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Subscriptions.Cacheable.WebConnectors.GetCacheableTable(CacheableMPartnerSubscriptionsTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableMailingTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Mailing Namespace
                TCacheableMailingTablesEnum CacheableMPartnerMailingTable =
                    (TCacheableMailingTablesEnum)Enum.Parse(typeof(TCacheableMailingTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Mailing.Cacheable.WebConnectors.GetCacheableTable(CacheableMPartnerMailingTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableFinanceTablesEnum)), ACacheableTableName) != -1)
            {
                // MFinance Namespace
                TCacheableFinanceTablesEnum CacheableMFinanceTable = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MFinance.Cacheable.WebConnectors.GetCacheableTable(CacheableMFinanceTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableSysManTablesEnum)), ACacheableTableName) != -1)
            {
                // MSysMan Namespace
                TCacheableSysManTablesEnum CacheableMSysManTable = (TCacheableSysManTablesEnum)Enum.Parse(typeof(TCacheableSysManTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MSysMan.Cacheable.WebConnectors.GetCacheableTable(CacheableMSysManTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheablePersonTablesEnum)), ACacheableTableName) != -1)
            {
                // MSysMan Namespace
                TCacheablePersonTablesEnum CacheableMPersonnelPersonTable = (TCacheablePersonTablesEnum)Enum.Parse(typeof(TCacheablePersonTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPersonnel.Person.Cacheable.WebConnectors.GetCacheableTable(CacheableMPersonnelPersonTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableUnitTablesEnum)), ACacheableTableName) != -1)
            {
                // MSysMan Namespace
                TCacheableUnitTablesEnum CacheableMPersonnelUnitsTable = (TCacheableUnitTablesEnum)Enum.Parse(typeof(TCacheableUnitTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPersonnel.Unit.Cacheable.WebConnectors.GetCacheableTable(CacheableMPersonnelUnitsTable,
                    AHashCode,
                    out ACacheableTableSystemType);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Makes the actual PetraServer method call for the retrieval of a cacheable
        /// DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTableName">description1</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable in the client-side cache, or
        /// '' if it isn't in the client-side cache yet (in this case the cacheable
        /// DataTable is always returned) (see @return)</param>
        /// <param name="AFilterCriteria">An Object containing the filter criteria value that is
        /// used by the server-side function that retrieves the data for the cacheable
        /// DataTable</param>
        /// <param name="ACacheableTableSystemType"></param>
        /// <returns>)
        /// If the Hash that is passed in in AHashCode doesn't fit the
        /// Hash that the server-side CacheableTablesManager has for this cacheable
        /// DataTable, the specified DataTable is returned, otherwise nil.
        /// </returns>
        public static DataTable GetCacheableDataTableFromPetraServer(String ACacheableTableName,
            String AHashCode,
            object AFilterCriteria,
            out System.Type ACacheableTableSystemType)
        {
            DataTable ReturnValue;
            TCacheableFinanceTablesEnum CacheableMFinanceTable;

            ReturnValue = null;
            ACacheableTableSystemType = null;

            if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableFinanceTablesEnum)), ACacheableTableName) != -1)
            {
                // MFinance Namespace
                CacheableMFinanceTable = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MFinance.Cacheable.WebConnectors.GetCacheableTable(CacheableMFinanceTable, AHashCode, Convert.ToInt32(
                        AFilterCriteria), out ACacheableTableSystemType);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the chosen DataTable from the Client-side Cache.
        ///
        /// If the DataTable is not available on the Client side, this procedure checks
        /// whether it available from a file and whether this file contains up-to-date
        /// data. If it isn't available from file or the file is out of date, the
        /// DataTable is retrieved once from the Petra Server and persisted in a file
        /// (as Binary Serialized DataTable) before giving it to the caller.
        /// </summary>
        /// <param name="ACacheableTableName">The cached DataTable that should be returned in the
        /// DataTable</param>
        /// <returns>Chosen DataTable.</returns>
        public static DataTable GetCacheableDataTableFromCache(String ACacheableTableName)
        {
            Type DataTableType;

            return GetCacheableDataTableFromCache(ACacheableTableName, "", null, out DataTableType);
        }

        /// <summary>
        /// Returns the chosen DataTable from the Client-side Cache.
        ///
        /// If the DataTable is not available on the Client side, this procedure checks
        /// whether it available from a file and whether this file contains up-to-date
        /// data. If it isn't available from file or the file is out of date, the
        /// DataTable is retrieved once from the Petra Server and persisted in a file
        /// (as Binary Serialized DataTable) before giving it to the caller.
        /// </summary>
        /// <remarks>Can be used with the TGetCacheableDataTableFromCache delegate.</remarks>
        /// <param name="ACacheableTableName">The cached DataTable that should be returned in the
        /// DataTable</param>
        /// <param name="AType"><see cref="System.Type" /> of the returned <see cref="DataTable" />.</param>
        /// <returns>Chosen DataTable.</returns>
        public static DataTable GetCacheableDataTableFromCache(String ACacheableTableName, out System.Type AType)
        {
            return GetCacheableDataTableFromCache(ACacheableTableName, "", null, out AType);
        }

        /// <summary>
        /// Returns the chosen DataTable from the Client-side Cache.
        ///
        /// If the DataTable is not available on the Client side, this procedure checks
        /// whether it available from a file and whether this file contains up-to-date
        /// data. If it isn't available from file or the file is out of date, the
        /// DataTable is retrieved once from the Petra Server and persisted in a file
        /// (as Binary Serialized DataTable) before giving it to the caller.
        /// </summary>
        /// <remarks>
        /// This overload needs to be used for cacheable DataTables that are
        /// returned not containing all DataRows that are available in the DB, but
        /// only some DataRows based on specified criteria (eg. some cacheable Finance
        /// DataTables).
        /// </remarks>
        /// <param name="ACacheableTableName">The cached DataTable that should be returned in the
        /// DataSet</param>
        /// <param name="AFilterCriteriaString">A criteria string that can be passed to the
        /// 'Select' method of a DataTable</param>
        /// <param name="AFilterCriteria">An Object containing the filter criteria value that is
        /// used by the server-side function that retrieves the data for the cacheable
        /// DataTable</param>
        /// <param name="ADataTableType"> The Type of the DataTable (useful in case it's a
        /// Typed DataTable).</param>
        /// <returns>Chosen DataTable.</returns>
        public static DataTable GetCacheableDataTableFromCache(String ACacheableTableName, String AFilterCriteriaString, object AFilterCriteria,
            out Type ADataTableType)
        {
            DataTable ReturnValue;
            DataTable CacheableDataTableFromCache;
            DataTable CacheableDataTableFromServer = null;
            DataTable CacheableDataTableFromFile = null;
            DataView CacheableDataTableFilteredDV = null;
            String HashCode = "";
            Int32 TmpSize = 0;
            Boolean CacheableDataTableReloadNecessary = true;

            System.Type TmpType;
            DataRow[] FilteredRows = null;

            ADataTableType = null;

            /*
             * Check whether cacheable DataTable is available in the Client-side Cache
             */
            if (UCacheableTablesManager.IsTableCached(ACacheableTableName))
            {
                if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                {
                    TLogging.Log("Cacheable DataTable '" + ACacheableTableName + "': is in Client-side ");
                }

                try
                {
                    // Cacheable DataTable is in Clientside Cache
                    CacheableDataTableFromCache = UCacheableTablesManager.GetCachedDataTable(ACacheableTableName, out ADataTableType);

                    if (AFilterCriteriaString != "")
                    {
                        /*
                         * Check if any rows of the Cacheable DataTable in the Client-side
                         * Cache match the AFilterCriteriaString
                         */
                        FilteredRows = CacheableDataTableFromCache.Select(AFilterCriteriaString);

                        if (FilteredRows.Length != 0)
                        {
                            // We have what we are looking for > no need to make a Server call
                            CacheableDataTableReloadNecessary = false;
                        }
                        else
                        {
                            // We don't have what we are looking for > need to make a Server call
                            CacheableDataTableReloadNecessary = true;
                        }
                    }
                    else
                    {
                        // We have what we are looking for > no need to make a Server call
                        CacheableDataTableReloadNecessary = false;
                    }
                }
                catch (ECacheableTablesMgrTableNotUpToDateException)
                {
                    // The Cacheable DataTable in the Clientside Cache is marked as
                    // being not uptodate, so we need to reload it from the
                    // PetraServer!
                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log("Cacheable DataTable '" + ACacheableTableName + "': needs reloading from OpenPetra Server!");
                    }

                    CacheableDataTableReloadNecessary = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if (CacheableDataTableReloadNecessary)
            {
                /*
                 * Check whether cacheable DataTable is available from file
                 */
                try
                {
                    CacheableDataTableFromFile = LoadCacheableDataTableFromFile(ACacheableTableName);
                }
                catch (Exception Exc)
                {
                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log("Cacheable DataTable '" + ACacheableTableName + "': loading from file failed!  Details: " + Exc.ToString());
                    }
                }

                if (CacheableDataTableFromFile != null)
                {
                    // Cacheable DataTable got loaded from file
                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log("Cacheable DataTable '" + ACacheableTableName + "': loaded from file.");
                    }

                    // Assign the correct TableName
                    CacheableDataTableFromFile.TableName = ACacheableTableName;

                    // Calculate HashCode for the DataTable (used to compare the DataTable
                    // with the DataTable in the Serverside Cache)
                    if (AFilterCriteriaString != "")
                    {
                        CacheableDataTableFilteredDV = new DataView(CacheableDataTableFromFile,
                            AFilterCriteriaString,
                            "",
                            DataViewRowState.CurrentRows);
                        DataUtilities.CalculateHashAndSize(CacheableDataTableFilteredDV, out HashCode, out TmpSize);
                    }
                    else
                    {
                        DataUtilities.CalculateHashAndSize(CacheableDataTableFromFile, out HashCode, out TmpSize);
                    }
                }
            }

//System.Windows.Forms.MessageBox.Show("From File: CacheableTableName Clientside:  HashCode: " + HashCode + "; Size: " + TmpSize.ToString());
            try
            {
                /*
                 * Make a call to the corresponding Server-side method to compare the
                 * HashCode and retrieve the cachable DataTable (if needed) and to
                 * retrieve the Type of the DataTable (it's a Typed DataTable in most
                 * cases).
                 */
                if ((AFilterCriteriaString != "")
                    && (AFilterCriteria != null))
                {
                    CacheableDataTableFromServer = GetCacheableDataTableFromPetraServer(ACacheableTableName,
                        HashCode,
                        AFilterCriteria,
                        out ADataTableType);
                }
                else
                {
                    CacheableDataTableFromServer = GetCacheableDataTableFromPetraServer(ACacheableTableName,
                        HashCode,
                        out ADataTableType);
                }

                /*
                 * Evaluate PetraServer response
                 */
                if (CacheableDataTableFromServer != null)
                {
                    // The PetraServer returned a DataTable. This means that it either
                    // had a more uptodate cacheable DataTable, or that the Client
                    // didn't have the DataTable at all (HashCode = '').
                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log("Cacheable DataTable '" + ACacheableTableName + "': got returned from OpenPetra Server.");
                    }

                    if (AFilterCriteriaString != "")
                    {
                        CacheableDataTableFilteredDV = new DataView(CacheableDataTableFromServer,
                            AFilterCriteriaString,
                            "",
                            DataViewRowState.CurrentRows);

                        DataRowCollection FilteredRowsColl = CacheableDataTableFilteredDV.ToTable().Rows;

                        FilteredRows = new DataRow[FilteredRowsColl.Count];

                        for (int ServerRowsCounter = 0; ServerRowsCounter < FilteredRowsColl.Count; ServerRowsCounter++)
                        {
                            FilteredRows.SetValue(FilteredRowsColl[ServerRowsCounter], ServerRowsCounter);
                        }
                    }

                    /*
                     * Add returned DataTable to the Cache - or Merge it if it already
                     * exists there (only if filtered DataRows of a DataTable are returned
                     * from the PetraServer)
                     */
                    UCacheableTablesManager.AddOrMergeCachedTable(CacheableDataTableFromServer, -1);

                    // Save the DataTable that's now in the Cache to a file
                    SaveCacheableDataTableToFile(UCacheableTablesManager.GetCachedDataTable(ACacheableTableName, out TmpType));
                }
                else
                {
                    /*
                     * The PetraServer returned no DataTable. This means that the
                     * DataTable that we have on the Client side is identical to the
                     * DataTable on the Server side. We need to add the DataTable that
                     * (potentially) was loaded from file to the Client-side  (If
                     * it is already there it might need replacing with the DataTable from
                     * the file, so we do that).
                     */
                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log(
                            "Cacheable DataTable '" + ACacheableTableName + "': OpenPetra Server tells that the Client-side DataTable is up-to-date.");
                    }

                    if (!(ADataTableType == typeof(System.Data.DataTable)))
                    {
                        /*
                         * The DataTable needs to be a typed DataTable, so we need to change
                         * the loaded DataTable to the Type that is returned from the
                         * PetraServer (so that a Typed DataTable is again a Typed a
                         * DataTable and not just a DataTable after loading it from a file).
                         */
                        DataUtilities.ChangeDataTableToTypedDataTable(ref CacheableDataTableFromFile, ADataTableType, "");
                    }

                    if (AFilterCriteriaString != "")
                    {
                        DataRowCollection FilteredRowsColl = CacheableDataTableFilteredDV.ToTable().Rows;

                        FilteredRows = new DataRow[FilteredRowsColl.Count];

                        for (int ServerRowsCounter = 0; ServerRowsCounter < FilteredRowsColl.Count; ServerRowsCounter++)
                        {
                            FilteredRows.SetValue(FilteredRowsColl[ServerRowsCounter], ServerRowsCounter);
                        }
                    }

                    // Add DataTable to the Cache that got loaded from a file
                    UCacheableTablesManager.AddOrRefreshCachedTable(ACacheableTableName, CacheableDataTableFromFile, -1);

                    if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                    {
                        TLogging.Log(
                            "Cacheable DataTable '" + ACacheableTableName +
                            "': DataTable that was loaded from file got added to Client-side  DataTable Type: " +
                            UCacheableTablesManager.GetCachedDataTable(ACacheableTableName,
                                out TmpType).GetType().FullName + "; Rows: " + UCacheableTablesManager.GetCachedDataTable(ACacheableTableName,
                                out TmpType).Rows.Count.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            ReturnValue = UCacheableTablesManager.GetCachedDataTable(ACacheableTableName, out TmpType);

            // If a Filter is set, we must return only the DataRows that match the Filter, not all DataRows
            if (FilteredRows != null)
            {
                ReturnValue.Clear();

                for (int FilteredRowsCounter = 0; FilteredRowsCounter < FilteredRows.Length; FilteredRowsCounter++)
                {
                    ReturnValue.ImportRow(FilteredRows[FilteredRowsCounter]);
                }

                ReturnValue.AcceptChanges();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the chosen DataTable from the Client-side Cache.
        ///
        /// If the DataTable is not available on the Client side, this procedure checks
        /// whether it available from a file and whether this file contains up-to-date
        /// data. If it isn't available from file or the file is out of date, the
        /// DataTable is retrieved once from the Petra Server and persisted in a file
        /// (as Binary Serialized DataTable) before giving it to the caller.
        /// </summary>
        /// <remarks>
        /// This overload needs to be used for cacheable DataTables that are
        /// returned not containing all DataRows that are available in the DB, but
        /// only some DataRows based on specified criteria (eg. some cacheable Finance
        /// DataTables). Set the <paramref name="ASpecificFilter" /> to a supported value to
        /// get the desired filtering.
        /// </remarks>
        /// <param name="ACacheableTableName">The cached DataTable that should be returned in the
        /// DataTable</param>
        /// <param name="ASpecificFilter">Hard-coded string that directs the filtering and the use of
        /// <paramref name="AFilterCriteria" />.</param>
        /// <param name="AFilterCriteria">An Object containing the filter criteria value that is
        /// used by the server-side function that retrieves the data for the cacheable DataTable.</param>
        /// <param name="ADataTableType"> The Type of the DataTable (useful in case it's a
        /// Typed DataTable).</param>
        /// <returns>Chosen DataTable, filtered as specified.</returns>
        public static DataTable GetSpecificallyFilteredCacheableDataTableFromCache(String ACacheableTableName,
            string ASpecificFilter,
            object AFilterCriteria,
            out Type ADataTableType)
        {
            TCacheableFinanceTablesEnum FinanceEnumValue;
            DataTable ReturnValue = null;

            ADataTableType = null;

            if (AFilterCriteria == null)
            {
                throw new ArgumentException("GetSpecificallyFilteredCacheableDataTableFromCache: AFilterCriteria must not be null");
            }

            if (ASpecificFilter == "Ledger")
            {
                try
                {
                    FinanceEnumValue = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum), ACacheableTableName);
                }
                catch (System.ArgumentException)
                {
                    throw new ArgumentException(
                        "GetSpecificallyFilteredCacheableDataTableFromCache: Argument 'ACacheableTableName' is not a recognized " +
                        "TCacheableFinanceTablesEnum value!");
                }
                catch (Exception)
                {
                    throw;
                }

                ReturnValue = TMFinance.GetCacheableFinanceTable(FinanceEnumValue, (int)AFilterCriteria);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Saves changes of that were made to a specific Cachable DataTable.
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable with changes.</param>
        /// <param name="AChangedCacheableDT">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public static TSubmitChangesResult SaveChangedCacheableDataTableToPetraServer(String ACacheableTableName,
            ref TTypedDataTable AChangedCacheableDT,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;
            TCacheableCommonTablesEnum CacheableMCommonTable;
            TCacheableConferenceTablesEnum CacheableMConferenceTable;
            TCacheableFinanceTablesEnum CacheableMFinanceTable;
            TCacheableSubscriptionsTablesEnum CacheableMPartnerSubscriptionsTable;
            TCacheablePartnerTablesEnum CacheableMPartnerPartnerTable;
            TCacheableMailingTablesEnum CacheableMPartnerMailingTable;
            TCacheablePersonTablesEnum CacheableMPersonnelPersonTable;
            TCacheableUnitTablesEnum CacheableMPersonnelUnitTable;
            TCacheableSysManTablesEnum CacheableMSysManTable;

            AVerificationResult = null;

            if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableCommonTablesEnum)), ACacheableTableName) != -1)
            {
                // MCommon Namespace
                CacheableMCommonTable = (TCacheableCommonTablesEnum)Enum.Parse(typeof(TCacheableCommonTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MCommon.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMCommonTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableConferenceTablesEnum)), ACacheableTableName) != -1)
            {
                // MConference Namespace
                CacheableMConferenceTable = (TCacheableConferenceTablesEnum)Enum.Parse(typeof(TCacheableConferenceTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MConference.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMConferenceTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableFinanceTablesEnum)), ACacheableTableName) != -1)
            {
                // MFinance Namespace
                CacheableMFinanceTable = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MFinance.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMFinanceTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableMailingTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Mailing Namespace
                CacheableMPartnerMailingTable = (TCacheableMailingTablesEnum)Enum.Parse(typeof(TCacheableMailingTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Mailing.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMPartnerMailingTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheablePartnerTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Partner Namespace
                CacheableMPartnerPartnerTable = (TCacheablePartnerTablesEnum)Enum.Parse(typeof(TCacheablePartnerTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Partner.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMPartnerPartnerTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableSubscriptionsTablesEnum)), ACacheableTableName) != -1)
            {
                // MPartner.Subscriptions Namespace
                CacheableMPartnerSubscriptionsTable = (TCacheableSubscriptionsTablesEnum)Enum.Parse(typeof(TCacheableSubscriptionsTablesEnum),
                    ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPartner.Subscriptions.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(
                    CacheableMPartnerSubscriptionsTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheablePersonTablesEnum)), ACacheableTableName) != -1)
            {
                // MPersonnel.Person Namespace
                CacheableMPersonnelPersonTable = (TCacheablePersonTablesEnum)Enum.Parse(typeof(TCacheablePersonTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPersonnel.Person.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMPersonnelPersonTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableUnitTablesEnum)), ACacheableTableName) != -1)
            {
                // MPersonnel.Unit Namespace
                CacheableMPersonnelUnitTable = (TCacheableUnitTablesEnum)Enum.Parse(typeof(TCacheableUnitTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MPersonnel.Unit.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMPersonnelUnitTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }
            else if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableSysManTablesEnum)), ACacheableTableName) != -1)
            {
                // MSysMan.Unit Namespace
                CacheableMSysManTable = (TCacheableSysManTablesEnum)Enum.Parse(typeof(TCacheableSysManTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MSysMan.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMSysManTable,
                    ref AChangedCacheableDT,
                    out AVerificationResult);
            }

            ReloadCacheTable(ACacheableTableName);

            return ReturnValue;
        }

        /// <summary>
        /// Saves changes of that were made to a specific Cachable DataTable.
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTableName">Name of the Cacheable DataTable with changes.</param>
        /// <param name="AChangedCacheableDT">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="AFilterCriteria">An Object containing the filter criteria value that is
        /// used by the server-side function that retrieves the data for the cacheable
        /// DataTable. (This is needed for the re-loading of the DataTable into the server-side
        /// Cache Manager after saving was successful.)</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public static TSubmitChangesResult SaveSpecificallyFilteredCacheableDataTableToPetraServer(String ACacheableTableName,
            ref TTypedDataTable AChangedCacheableDT,
            object AFilterCriteria,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;
            TCacheableFinanceTablesEnum CacheableMFinanceTable;

            AVerificationResult = null;

            if (System.Array.IndexOf(Enum.GetNames(typeof(TCacheableFinanceTablesEnum)), ACacheableTableName) != -1)
            {
                // MFinance Namespace
                CacheableMFinanceTable = (TCacheableFinanceTablesEnum)Enum.Parse(typeof(TCacheableFinanceTablesEnum), ACacheableTableName);

                // PetraServer method call
                ReturnValue = TRemote.MFinance.Cacheable.WebConnectors.SaveChangedStandardCacheableTable(CacheableMFinanceTable,
                    ref AChangedCacheableDT,
                    (int)AFilterCriteria,
                    out AVerificationResult);
            }

            ReloadCacheTable(ACacheableTableName);

            return ReturnValue;
        }

        /// <summary>
        /// Deserializes a cacheable DataTable from a binary file on disk into a
        /// DataTable.
        ///
        /// </summary>
        /// <param name="ATableName">File name of the cacheable DataTable that should be loaded</param>
        /// <returns>Deserialized DataTable
        /// </returns>
        public static DataTable LoadCacheableDataTableFromFile(String ATableName)
        {
            DataTable ReturnValue;
            StreamReader CacheDTStreamReader;
            DataTable BinaryDT;
            BinaryFormatter CacheDTFormatter;
            IsolatedStorageFileStream ISFStream;

            if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
            {
                TLogging.Log("Trying to loading cacheable DataTable from file '" + ATableName + CACHEABLEDT_FILE_EXTENSION + "'...");
            }

            BinaryDT = new DataTable(ATableName);
            CacheDTFormatter = new BinaryFormatter();
            CacheDTStreamReader = null;
            try
            {
                ISFStream = GetCacheableDataTableFileForReading(ATableName);
                try
                {
                    // , System.Text.Encoding.Unicode
                    CacheDTStreamReader = new StreamReader(ISFStream);
                    BinaryDT = (DataTable)(CacheDTFormatter.Deserialize(CacheDTStreamReader.BaseStream));
                }
                finally
                {
                    if (CacheDTStreamReader != null)
                    {
                        CacheDTStreamReader.Close();
                    }

                    ISFStream.Close();
                }

                if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                {
                    TLogging.Log("Loading cacheable DataTable done. " + BinaryDT.Rows.Count.ToString() + " DataRows loaded.");
                }

                ReturnValue = BinaryDT;
            }
            catch (System.IO.FileNotFoundException)
            {
                if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
                {
                    TLogging.Log("Cacheable DataTable '" + ATableName + "': loading from file failed - file doesn't exist.");
                }

                ReturnValue = null;
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Serialize a cached DataTable to a binary file on disk.
        ///
        /// </summary>
        /// <param name="ADataTable">DataTable that should be serialized to a file
        /// </param>
        /// <returns>void</returns>
        public static void SaveCacheableDataTableToFile(DataTable ADataTable)
        {
            DataTable BinaryDT;
            DataSet TmpDS;
            BinaryFormatter CacheDTFormatter;
            StreamWriter CacheDTStreamWriter;
            Int64 CacheDTSizeOnDisk;

            if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
            {
                TLogging.Log("Serializing DataTable to '" + ADataTable.TableName + CACHEABLEDT_FILE_EXTENSION + "'...");
            }

            BinaryDT = new DataTable(ADataTable.TableName);
            TmpDS = new DataSet();

            // Merge data from the submitted DataTable into a TBinDataTable
            TmpDS.Tables.Add(BinaryDT);
            TmpDS.Merge(ADataTable);
            CacheDTFormatter = new BinaryFormatter();

            // false, System.Text.Encoding.Unicode);
            CacheDTStreamWriter = new StreamWriter(GetCacheableDataTableFileForWriting(ADataTable.TableName));

            CacheDTFormatter.Serialize(CacheDTStreamWriter.BaseStream, BinaryDT);
            CacheDTSizeOnDisk = CacheDTStreamWriter.BaseStream.Length;
            CacheDTStreamWriter.Close();

            if (TLogging.DebugLevel >= DEBUGLEVEL_CACHEMESSAGES)
            {
                TLogging.Log("Done. File size is " + CacheDTSizeOnDisk.ToString() + " bytes.");
            }
        }

        /// <summary>
        /// this should be called to reset all caches, for example when a new patch is installed
        /// </summary>
        public static void ClearAllCaches()
        {
            UIsolatedStorageFile.Remove();

            UIsolatedStorageFile = IsolatedStorageFile.GetStore(
                IsolatedStorageScope.User | IsolatedStorageScope.Assembly |
                IsolatedStorageScope.Roaming, null, null);

            UIsolatedStorageFile.CreateDirectory(CACHEFILESDIR);
        }
    }
}