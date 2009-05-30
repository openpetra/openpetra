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
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Partner.Data.Access;
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
            PAddresseeTypeTable TmpAddresseeTypeDT;
            PAcquisitionTable TmpAcquisitionDT;
            PBusinessTable TmpBusinessDT;
            PCountryTable TmpCountryDT;
            ACurrencyTable TmpCurrencyDT;
            PDataLabelTable TmpDataLabelDT;
            PDataLabelUseTable TmpDataLabelUseDT;
            PDataLabelLookupCategoryTable TmpDataLabelLookupCategoryDT;
            PDataLabelLookupTable TmpDataLabelLookupDT;
            PDenominationTable TmpDenominationDT;
            PInterestTable TmpInterestDT;
            PInterestCategoryTable TmpInterestCategoryDT;
            PLanguageTable TmpLanguageDT;
            PLocationTypeTable TmpLocationTypeDT;
            PtMaritalStatusTable TmpMaritalStatusDT;
            POccupationTable TmpOccupationDT;
            PPartnerStatusTable TmpPartnerStatusTableDT;
            PTypeTable TmpTypeDT;
            UUnitTypeTable TmpUnitTypeDT;
            PFoundationProposalStatusTable TmpFoundationProposalStatusDT;
            PProposalSubmissionTypeTable TmpProposalSubmissionTypeDT;

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
                            PAddresseeTypeAccess.LoadAll(out TmpAddresseeTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpAddresseeTypeDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.AcquisitionCodeList:
                            PAcquisitionAccess.LoadAll(out TmpAcquisitionDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpAcquisitionDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.BusinessCodeList:
                            PBusinessAccess.LoadAll(out TmpBusinessDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpBusinessDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.CountryList:
                            PCountryAccess.LoadAll(out TmpCountryDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpCountryDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.CurrencyCodeList:
                            ACurrencyAccess.LoadAll(out TmpCurrencyDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpCurrencyDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelList:
                            PDataLabelAccess.LoadAll(out TmpDataLabelDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpDataLabelDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelUseList:
                            PDataLabelUseAccess.LoadAll(out TmpDataLabelUseDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpDataLabelUseDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelLookupCategoryList:
                            PDataLabelLookupCategoryAccess.LoadAll(out TmpDataLabelLookupCategoryDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpDataLabelLookupCategoryDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DataLabelLookupList:
                            PDataLabelLookupAccess.LoadAll(out TmpDataLabelLookupDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpDataLabelLookupDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.DenominationList:
                            PDenominationAccess.LoadAll(out TmpDenominationDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpDenominationDT, DomainManager.GClientID);

                            // FoundationOwnerList:     this is nonstandard!
                            // InstalledSitesList:     this is nonstandard!
                            break;

                        case TCacheablePartnerTablesEnum.InterestList:
                            PInterestAccess.LoadAll(out TmpInterestDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpInterestDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.InterestCategoryList:
                            PInterestCategoryAccess.LoadAll(out TmpInterestCategoryDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpInterestCategoryDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.LanguageCodeList:
                            PLanguageAccess.LoadAll(out TmpLanguageDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpLanguageDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.LocationTypeList:
                            PLocationTypeAccess.LoadAll(out TmpLocationTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpLocationTypeDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.MaritalStatusList:
                            PtMaritalStatusAccess.LoadAll(out TmpMaritalStatusDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpMaritalStatusDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.OccupationList:
                            POccupationAccess.LoadAll(out TmpOccupationDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpOccupationDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.PartnerStatusList:
                            PPartnerStatusAccess.LoadAll(out TmpPartnerStatusTableDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpPartnerStatusTableDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.PartnerTypeList:
                            PTypeAccess.LoadAll(out TmpTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTypeDT, DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.ProposalStatusList:
                            PFoundationProposalStatusAccess.LoadAll(out TmpFoundationProposalStatusDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpFoundationProposalStatusDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.ProposalSubmissionTypeList:
                            PProposalSubmissionTypeAccess.LoadAll(out TmpProposalSubmissionTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName,
                            TmpProposalSubmissionTypeDT,
                            DomainManager.GClientID);
                            break;

                        case TCacheablePartnerTablesEnum.UnitTypeList:
                            UUnitTypeAccess.LoadAll(out TmpUnitTypeDT, ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpUnitTypeDT, DomainManager.GClientID);

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
            SUserTable TmpUserTable;
            SUserRow EmptyDR;
            string TableName;
            DataSet TmpDS;

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
                    TmpDS = new DataSet();
                    TmpDS.Tables.Add(new SUserTable());
                    TmpDS = DBAccess.GDBAccessObj.Select(TmpDS, "SELECT " + SUserTable.GetPartnerKeyDBName() + ',' +
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
                        " = FALSE", SUserTable.GetTableName(), ReadTransaction);
                    TmpUserTable = (SUserTable)TmpDS.Tables[0];
                    EmptyDR = TmpUserTable.NewRowTyped(false);
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
                    PPartnerLedgerAccess.LoadAll(out TmpInstalledSitesDT, RequiredColumns, ReadTransaction, null, 0, 0);

                    if (TmpInstalledSitesDT.Rows.Count != 0)
                    {
                        TmpInstalledSitesDT.Columns.Remove(PPartnerLedgerTable.GetLastPartnerIdDBName());
                        TmpInstalledSitesDT.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), System.Type.GetType("System.String"));
                        RequiredColumns = new StringCollection();
                        RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());

                        for (Counter = 0; Counter <= TmpInstalledSitesDT.Rows.Count - 1; Counter += 1)
                        {
                            PPartnerAccess.LoadByPrimaryKey(out PartnerDT,
                                TmpInstalledSitesDT[Counter].PartnerKey,
                                RequiredColumns,
                                ReadTransaction,
                                null,
                                0,
                                0);
                            TmpInstalledSitesDT[Counter][PPartnerTable.GetPartnerShortNameDBName()] = PartnerDT[0].PartnerShortName;
                        }
                    }

                    DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpInstalledSitesDT, DomainManager.GClientID);
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

        #endregion
    }
}