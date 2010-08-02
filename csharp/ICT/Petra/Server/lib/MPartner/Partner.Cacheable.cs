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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.MCommon;

namespace Ict.Petra.Server.MPartner.Partner
{
    /**
     * Returns cachable DataTables for DB tables in the MPartner.Partner sub-namespace
     * that can be cached on the Client side.
     *
     * Examples of such tables are tables that form entries of ComboBoxes or Lists
     * and which would be retrieved numerous times from the Server as UI windows
     * are opened.
     */
    public class TPartnerCacheable : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        #region TMPartner_Partner_Cacheable

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerCacheable() : base()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
            FCacheableTablesManager = DomainManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// destructor
        ~TPartnerCacheable()
        {
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif



        /**
         * Returns a certain cachable DataTable that contains all columns and all
         * rows of a specified table.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no further DB
         * queries!
         *
         * @comment All DataTables are retrieved as Typed DataTables, but are passed
         * out as a normal DataTable. However, this DataTable can be cast by the
         * caller to the appropriate TypedDataTable to have access to the features of
         * a Typed DataTable!
         *
         * @param ACacheableTable Tells what cachable DataTable should be returned.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can
         * be specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * The CacheableTablesManager will notify other Clients that they need to
         * retrieve this Cacheable DataTable anew from the PetraServer the next time
         * the Client accesses the Cacheable DataTable. Otherwise set to false.
         * @param AType The Type of the DataTable (useful in case it's a
         * Typed DataTable)
         * @return DataTable If the Hash that got passed in AHashCode doesn't fit the
         * Hash that the CacheableTablesManager has for this cacheable DataTable, the
         * specified DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetStandardCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            String TableName;
            DataTable TmpTable;

            TableName = Enum.GetName(typeof(TCacheablePartnerTablesEnum), ACacheableTable);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable called with ATableName='" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheablePartnerTablesEnum.AddresseeTypeList:
                            TmpTable = PAddresseeTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.AcquisitionCodeList:
                            TmpTable = PAcquisitionAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.BusinessCodeList:
                            TmpTable = PBusinessAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.CurrencyCodeList:
                            TmpTable = ACurrencyAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelList:
                            TmpTable = PDataLabelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelUseList:
                            TmpTable = PDataLabelUseAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelLookupCategoryList:
                            TmpTable = PDataLabelLookupCategoryAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelLookupList:
                            TmpTable = PDataLabelLookupAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DenominationList:
                            TmpTable = PDenominationAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);

                            // FoundationOwnerList:     this is nonstandard!
                            // InstalledSitesList:     this is nonstandard!
                            break;

                        case TCacheablePartnerTablesEnum.InterestList:
                            TmpTable = PInterestAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.InterestCategoryList:
                            TmpTable = PInterestCategoryAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.LocationTypeList:
                            TmpTable = PLocationTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.MaritalStatusList:
                            TmpTable = PtMaritalStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.MethodOfContactList:
                            TmpTable = PMethodOfContactAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.OccupationList:
                            TmpTable = POccupationAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.PartnerStatusList:
                            TmpTable = PPartnerStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.PartnerTypeList:
                            TmpTable = PTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.ProposalStatusList:
                            TmpTable = PFoundationProposalStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.ProposalSubmissionTypeList:
                            TmpTable = PProposalSubmissionTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.RelationList:
                            TmpTable = PRelationAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.RelationCategoryList:
                            TmpTable = PRelationCategoryAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.UnitTypeList:
                            TmpTable = UUnitTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);

                            // Unknown Standard Cacheable DataTable
                            break;

                        default:
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                            Enum.GetName(typeof(TCacheablePartnerTablesEnum),
                                ACacheableTable) + "' is not available as a Standard Cacheable Table");
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

        /**
         * Returns non-standard cachable table 'FoundationOwnerList'.
         * DB Table:  s_user
         * @comment Used in Foundation Details screen.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no more DB
         * queries.
         *
         * @comment The DataTables is retrieved as Typed DataTables, but is passed
         * out as a normal DataTable. However, this DataTable can be cast by the
         * caller to the appropriate TypedDataTable to have access to the features of
         * a Typed DataTable!
         *
         * @param ATableName TableName that the returned DataTable should have.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can be
         * specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * Otherwise set to false.
         * @return DataTable If the Hash passed in in AHashCode doesn't fit the Hash that
         * the CacheableTablesManager has for this cacheable DataTable, the
         * 'FoundationOwnerList' DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetFoundationOwnerCacheableTable(String ATableName, String AHashCode, Boolean ARefreshFromDB, out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            string TableName;

            TableName = Enum.GetName(typeof(TCacheablePartnerTablesEnum), TCacheablePartnerTablesEnum.FoundationOwnerList);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetFoundationOwnerCacheableTable called.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    SUserTable TmpUserTable = new SUserTable();
                    TmpUserTable = (SUserTable)DBAccess.GDBAccessObj.SelectDT(TmpUserTable, "SELECT " + SUserTable.GetPartnerKeyDBName() + ',' +
                        SUserTable.GetUserIdDBName() + ',' +
                        SUserTable.GetFirstNameDBName() + ',' +
                        SUserTable.GetLastNameDBName() + ' ' +
                        "FROM PUB_" + SUserTable.GetTableDBName() + ' ' +
                        "WHERE " + SUserTable.GetPartnerKeyDBName() + " <> 0 " +
                        "AND " + SUserTable.GetUserIdDBName() +
                        " IN (SELECT " + SUserModuleAccessPermissionTable.GetUserIdDBName() + ' ' +
                        "FROM PUB_" + SUserModuleAccessPermissionTable.GetTableDBName() + ' ' +
                        "WHERE " + SUserModuleAccessPermissionTable.GetModuleIdDBName() +
                        " = 'DEVUSER')" + "AND " + SUserTable.GetRetiredDBName() +
                        " = FALSE", ReadTransaction, null, -1, -1);
                    SUserRow EmptyDR = TmpUserTable.NewRowTyped(false);
                    EmptyDR.PartnerKey = 0;
                    EmptyDR.UserId = "";
                    TmpUserTable.Rows.InsertAt(EmptyDR, 0);
                    DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpUserTable, DomainManager.GClientID);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetFoundationOwnerCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }

        /**
         * Returns non-standard cachable table 'InstalledSitesList'.
         * DB Table:  p_partner_ledger, p_partner
         * @comment Used eg. in New Partner Dialog.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no more DB
         * queries.
         *
         * @comment The DataTables is retrieved as Typed DataTables, but is passed
         * out as a normal DataTable. However, this DataTable can be cast by the
         * caller to the appropriate TypedDataTable to have access to the features of
         * a Typed DataTable!
         *
         * @param ATableName TableName that the returned DataTable should have.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can be
         * specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * Otherwise set to false.
         * @return DataTable If the Hash passed in in AHashCode doesn't fit the Hash that
         * the CacheableTablesManager has for this cacheable DataTable, the
         * 'InstalledSitesList' DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetInstalledSitesCacheableTable(String ATableName, String AHashCode, Boolean ARefreshFromDB, out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            StringCollection RequiredColumns;
            PPartnerLedgerTable TmpInstalledSitesDT;
            PPartnerTable PartnerDT;
            int Counter;
            string TableName;

            TableName = Enum.GetName(typeof(TCacheablePartnerTablesEnum), TCacheablePartnerTablesEnum.InstalledSitesList);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetInstalledSitesCacheableTable called.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
            {
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerLedgerTable.GetPartnerKeyDBName());
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    TmpInstalledSitesDT = PPartnerLedgerAccess.LoadAll(RequiredColumns, ReadTransaction, null, 0, 0);

                    if (TmpInstalledSitesDT.Rows.Count != 0)
                    {
                        TmpInstalledSitesDT.Columns.Remove(PPartnerLedgerTable.GetLastPartnerIdDBName());
                        TmpInstalledSitesDT.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), System.Type.GetType("System.String"));
                        RequiredColumns = new StringCollection();
                        RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());

                        for (Counter = 0; Counter <= TmpInstalledSitesDT.Rows.Count - 1; Counter += 1)
                        {
                            PartnerDT = PPartnerAccess.LoadByPrimaryKey(
                                TmpInstalledSitesDT[Counter].PartnerKey,
                                RequiredColumns,
                                ReadTransaction,
                                null,
                                0,
                                0);
                            TmpInstalledSitesDT[Counter][PPartnerTable.GetPartnerShortNameDBName()] = PartnerDT[0].PartnerShortName;
                        }
                    }

                    DataTable TmpTable = TmpInstalledSitesDT;
                    DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetInstalledSitesCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }

        /**
         * Returns non-standard cachable table 'CountyList'.
         * DB Table:  p_location
         * @comment Used eg. in Report Gift Data Export for finding donors.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no more DB
         * queries.
         *
         * @comment The DataTables is no typed at the moment
         *
         * @param ATableName TableName that the returned DataTable should have.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can be
         * specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * Otherwise set to false.
         * @return DataTable If the Hash passed in in AHashCode doesn't fit the Hash that
         * the CacheableTablesManager has for this cacheable DataTable, the
         * 'CountyList' DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetCountyListCacheableTable(String ATableName, String AHashCode, Boolean ARefreshFromDB, out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            DataTable TmpTable;

#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetCountyListCacheableTable called.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(ATableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    TmpTable = DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + PLocationTable.GetCountryCodeDBName() + ", " +
                        PLocationTable.GetCountyDBName() + " FROM PUB." +
                        PLocationTable.GetTableDBName(), ATableName, ReadTransaction);
                    DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(ATableName, TmpTable, DomainManager.GClientID);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetCountyListCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(ATableName, AHashCode, out AType);
        }

        /**
         * Returns non-standard cachable table 'CountryListFromExistingLocations'.
         * DB Table:  p_location
         * @comment Used eg. in Report Gift Data Export for finding donors.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no more DB
         * queries.
         *
         * @comment The DataTables is no typed at the moment
         *
         * @param ATableName TableName that the returned DataTable should have.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can be
         * specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * Otherwise set to false.
         * @return DataTable If the Hash passed in in AHashCode doesn't fit the Hash that
         * the CacheableTablesManager has for this cacheable DataTable, the
         * 'CountryListFromExistingLocations' DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetCountryListFromExistingLocationsCacheableTable(String ATableName,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            DataTable TmpTable;

#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetCountryListFromExistingLocationsCacheableTable called.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(ATableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    TmpTable = DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + "PUB." + PCountryTable.GetTableDBName() + '.' +
                        PCountryTable.GetCountryCodeDBName() + ", " +
                        PCountryTable.GetCountryNameDBName() + " FROM PUB." +
                        PCountryTable.GetTableDBName() + " c, PUB." +
                        PLocationTable.GetTableDBName() + " l " +
                        " WHERE " + PLocationTable.GetCountyDBName() + " IS NOT NULL AND NOT " +
                        PLocationTable.GetCountyDBName() + " = ''" +
                        " AND c." + PCountryTable.GetCountryCodeDBName() + " = l." +
                        PLocationTable.GetCountryCodeDBName(), ATableName, ReadTransaction);
                    DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(ATableName, TmpTable, DomainManager.GClientID);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(
                                this.GetType().FullName + ".GetCountryListFromExistingLocationsCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(ATableName, AHashCode, out AType);
        }

        /// <summary>
        /// Saves a specific Cachable DataTable. The whole DataTable needs to be submitted,
        /// not just changes to it!
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTable">Name of the Cacheable DataTable with changes.</param>
        /// <param name="ASubmitTable">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            string CacheableDTName = Enum.GetName(typeof(TCacheablePartnerTablesEnum), ACacheableTable);
            Type TmpType;

            // Console.WriteLine("Entering Partner.SaveChangedStandardCacheableTable...");
            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheablePartnerTablesEnum.AcquisitionCodeList:

                            if (PAcquisitionAccess.SubmitChanges((PAcquisitionTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.AddresseeTypeList:

                            if (PAddresseeTypeAccess.SubmitChanges((PAddresseeTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.BusinessCodeList:

                            if (PBusinessAccess.SubmitChanges((PBusinessTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.DenominationList:

                            if (PDenominationAccess.SubmitChanges((PDenominationTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.InterestList:

                            if (PInterestAccess.SubmitChanges((PInterestTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.InterestCategoryList:

                            if (PInterestCategoryAccess.SubmitChanges((PInterestCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.LocationTypeList:

                            if (PLocationTypeAccess.SubmitChanges((PLocationTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.MaritalStatusList:

                            if (PtMaritalStatusAccess.SubmitChanges((PtMaritalStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.MethodOfContactList:

                            if (PMethodOfContactAccess.SubmitChanges((PMethodOfContactTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.OccupationList:

                            if (POccupationAccess.SubmitChanges((POccupationTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.PartnerTypeList:

                            if (PTypeAccess.SubmitChanges((PTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.PartnerStatusList:

                            if (PPartnerStatusAccess.SubmitChanges((PPartnerStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.RelationList:

                            if (PRelationAccess.SubmitChanges((PRelationTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        case TCacheablePartnerTablesEnum.RelationCategoryList:

                            if (PRelationCategoryAccess.SubmitChanges((PRelationCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }

                            break;

                        default:

                            throw new Exception(
                            "TPartnerCacheable.SaveChangedStandardCacheableTable: unsupported Cacheabled DataTable '" + CacheableDTName + "'");
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log(
                        "TPartnerCacheable.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheabled DataTable '" +
                        CacheableDTName +
                        "':  Exception " + e.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            /*
             * If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
             * Cache and inform all other Clients that they need to reload this Cacheable DataTable
             * the next time something in the Client accesses it.
             */
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                GetStandardCacheableTable(ACacheableTable, String.Empty, true, out TmpType);
            }

            return SubmissionResult;
        }

        #endregion
    }
}