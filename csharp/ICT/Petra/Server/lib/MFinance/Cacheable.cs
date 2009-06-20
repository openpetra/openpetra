/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections.Specialized;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Account.Data.Access;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Gift.Data.Access;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.DataAggregates;

namespace Ict.Petra.Server.MFinance
{
    /// <summary>
    /// Returns DataTables for DB tables in the MFinance namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows are
    /// opened.
    /// </summary>
    public class TMFinanceCacheable : TCacheableTablesLoader
    {
        /// <summary>time when this object was instantiated</summary>
        private DateTime FStartTime;

        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its contents got retrieved from the DB. It returns the cached
        /// DataTable from it on subsequent calls, therefore making more no further DB
        /// queries!
        ///
        /// @comment All DataTables are retrieved as Typed DataTables, but are passed
        /// out as a normal DataTable. However, this DataTable can be cast by the
        /// caller to the appropriate TypedDataTable to have access to the features of
        /// a Typed DataTable!
        ///
        /// </summary>
        /// <param name="ACacheableTable">Tells what cachable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can be
        /// specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            String TableName;
            ABudgetTypeTable TmpABudgetTypeDT;
            ACostCentreTypesTable TmpACostCentreTypesDT;
            DataTable TmpALedgerNameDT;

            TableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable called with ATableName='" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.BudgetTypeList:
                            ABudgetTypeAccess.LoadAll(out TmpABudgetTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpABudgetTypeDT, DomainManager.GClientID);
                            break;

                        case TCacheableFinanceTablesEnum.CostCentreTypeList:
                            ACostCentreTypesAccess.LoadAll(out TmpACostCentreTypesDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpACostCentreTypesDT, DomainManager.GClientID);
                            break;

                        case TCacheableFinanceTablesEnum.LedgerNameList:
                            TmpALedgerNameDT = TALedgerNameAggregate.GetData(TableName, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpALedgerNameDT, DomainManager.GClientID);
                            break;

                        default:

                            // Unknown Standard Cacheable DataTable
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                            Enum.GetName(typeof(TCacheableFinanceTablesEnum),
                                ACacheableTable) + "' is not available as a Standard Cacheable Table (without ALedgerNumber as an Argument)");

                            //break;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }

        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and rows
        /// of a specified table that match a specified Ledger Number.
        ///
        /// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its contents got retrieved from the DB. It returns the cached
        /// DataTable from it on subsequent calls, therefore making more no further DB
        /// queries!
        ///
        /// @comment All DataTables are retrieved as Typed DataTables, but are passed
        /// out as a normal DataTable. However, this DataTable can be cast by the
        /// caller to the appropriate TypedDataTable to have access to the features of
        /// a Typed DataTable!
        ///
        /// </summary>
        /// <param name="ACacheableTable">Tells what cachable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can be
        /// specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
        /// the Cache need to match.</param>
        /// <returns>)
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            System.Int32 ALedgerNumber,
            out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            String TableName;
            AAccountTable TmpAAccountDT;
            AAccountHierarchyTable TmpAccountHierarchyDT;
            AAccountingPeriodTable TmpAAccountingPeriodDT;
            ACostCentreTable TmpACostCentreDT;
            ALedgerTable TmpALedgerDT;
            AMotivationDetailTable TmpAMotivationDetailDT;
            StringCollection FieldList;

            System.Type TmpType;
            DataView TmpView;
            TableName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(
                    this.GetType().FullName + ".GetStandardCacheableTable called with ATableName='" + TableName + "' and ALedgerNumber=" +
                    ALedgerNumber.ToString() + '.');
            }
#endif
#if DEBUGMODE
            if (DomainManager.GCacheableTablesManager.IsTableCached(TableName))
            {
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("Cached DataTable has currently " +
                        DomainManager.GCacheableTablesManager.GetCachedDataTable(TableName, out TmpType).Rows.Count.ToString() + " rows in total.");
                    Console.WriteLine("Cached DataTable has currently " +
                        Convert.ToString(DomainManager.GCacheableTablesManager.GetCachedDataTable(TableName,
                                out TmpType).Select(
                                ALedgerTable.GetLedgerNumberDBName() + " = " +
                                ALedgerNumber.ToString()).Length) + " rows with ALedgerNumber=" + ALedgerNumber.ToString() + '.');
                }
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName)))
                || ((DomainManager.GCacheableTablesManager.IsTableCached(TableName))
                    && (DomainManager.GCacheableTablesManager.GetCachedDataTable(TableName,
                            out TmpType).Select(ALedgerTable.GetLedgerNumberDBName() + " = " +
                            ALedgerNumber.ToString()).Length == 0)))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableFinanceTablesEnum.AccountList:
                            FieldList = new StringCollection();
                            FieldList.Add(AAccountTable.GetLedgerNumberDBName());
                            FieldList.Add(AAccountTable.GetAccountCodeDBName());
                            FieldList.Add(AAccountTable.GetAccountCodeShortDescDBName());
                            FieldList.Add(AAccountTable.GetAccountActiveFlagDBName());
                            FieldList.Add(AAccountTable.GetPostingStatusDBName());
                            AAccountAccess.LoadViaALedger(out TmpAAccountDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName,
                            TmpAAccountDT,
                            DomainManager.GClientID,
                            (object)ALedgerNumber);
                            break;

                        case TCacheableFinanceTablesEnum.AccountHierarchyList:
                            FieldList = new StringCollection();
                            FieldList.Add(AAccountHierarchyTable.GetLedgerNumberDBName());
                            FieldList.Add(AAccountHierarchyTable.GetAccountHierarchyCodeDBName());
                            AAccountHierarchyAccess.LoadViaALedger(out TmpAccountHierarchyDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpAccountHierarchyDT, DomainManager.GClientID,
                            (object)ALedgerNumber);
                            break;

                        case TCacheableFinanceTablesEnum.AccountingPeriodList:
                            FieldList = new StringCollection();
                            FieldList.Add(AAccountingPeriodTable.GetLedgerNumberDBName());
                            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodNumberDBName());
                            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodDescDBName());
                            FieldList.Add(AAccountingPeriodTable.GetPeriodStartDateDBName());
                            FieldList.Add(AAccountingPeriodTable.GetPeriodEndDateDBName());
                            AAccountingPeriodAccess.LoadViaALedger(out TmpAAccountingPeriodDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpAAccountingPeriodDT, DomainManager.GClientID,
                            (object)ALedgerNumber);
                            break;

                        case TCacheableFinanceTablesEnum.CostCentreList:
                            FieldList = new StringCollection();
                            FieldList.Add(ACostCentreTable.GetLedgerNumberDBName());
                            FieldList.Add(ACostCentreTable.GetCostCentreCodeDBName());
                            FieldList.Add(ACostCentreTable.GetCostCentreNameDBName());
                            FieldList.Add(ACostCentreTable.GetCostCentreToReportToDBName());
                            FieldList.Add(ACostCentreTable.GetPostingCostCentreFlagDBName());
                            FieldList.Add(ACostCentreTable.GetCostCentreActiveFlagDBName());
                            FieldList.Add(ACostCentreTable.GetCostCentreTypeDBName());
                            ACostCentreAccess.LoadViaALedger(out TmpACostCentreDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName,
                            TmpACostCentreDT,
                            DomainManager.GClientID,
                            (object)ALedgerNumber);
                            break;

                        case TCacheableFinanceTablesEnum.LedgerDetails:
                            FieldList = new StringCollection();
                            FieldList.Add(ALedgerTable.GetLedgerNumberDBName());
                            FieldList.Add(ALedgerTable.GetNumberFwdPostingPeriodsDBName());
                            FieldList.Add(ALedgerTable.GetNumberOfAccountingPeriodsDBName());
                            FieldList.Add(ALedgerTable.GetCurrentPeriodDBName());
                            FieldList.Add(ALedgerTable.GetCurrentFinancialYearDBName());
                            FieldList.Add(ALedgerTable.GetBranchProcessingDBName());
                            ALedgerAccess.LoadByPrimaryKey(out TmpALedgerDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName,
                            TmpALedgerDT,
                            DomainManager.GClientID,
                            (object)ALedgerNumber);
                            break;

                        case TCacheableFinanceTablesEnum.MotivationList:
                            FieldList = new StringCollection();
                            FieldList.Add(AMotivationDetailTable.GetLedgerNumberDBName());
                            FieldList.Add(AMotivationDetailTable.GetMotivationGroupCodeDBName());
                            FieldList.Add(AMotivationDetailTable.GetMotivationDetailCodeDBName());
                            FieldList.Add(AMotivationDetailTable.GetMotivationStatusDBName());
                            AMotivationDetailAccess.LoadViaALedger(out TmpAMotivationDetailDT, ALedgerNumber, FieldList, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpAMotivationDetailDT, DomainManager.GClientID,
                            (object)ALedgerNumber);

                            // Unknown Standard Cacheable DataTable
                            break;

                        default:
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                            Enum.GetName(typeof(TCacheableFinanceTablesEnum),
                                ACacheableTable) + "' is not available as a Standard Cacheable Table (with ALedgerNumber as an Argument)");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            TmpView = new DataView(DomainManager.GCacheableTablesManager.GetCachedDataTable(TableName,
                    out TmpType), ALedgerTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString(), "", DataViewRowState.CurrentRows);

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, TmpView, out AType);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TMFinanceCacheable() : base()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;

            // FDataCacheDataSet := new DataSet('ServerDataCache');
            FCacheableTablesManager = DomainManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// destructor
        ~TMFinanceCacheable()
        {
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif

    }
}