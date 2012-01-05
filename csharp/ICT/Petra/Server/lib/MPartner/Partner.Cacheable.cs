// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
#region ManualCode
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MCommon.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.UIConnectors;
#endregion ManualCode
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.Partner.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MPartner.Partner sub-namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public class TPartnerCacheable : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerCacheable() : base()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
            FCacheableTablesManager = TCacheableTablesManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// destructor
        ~TPartnerCacheable()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif

#region ManualCode
		/// <summary>
		/// Overload of <see cref="M:GetCacheableTable(TCacheablePartnerTablesEnum, string, bool, out System.Type)" />. See description there.
		/// </summary>
		/// <remarks>Can be used with Delegate TGetCacheableDataTableFromCache.</remarks>
		/// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
	    /// <param name="AType">The Type of the DataTable (useful in case it's a
	    /// Typed DataTable)</param>
		/// <returns>The specified Cacheable DataTable is returned if the string matches a Cacheable DataTable, 
		/// otherwise <see cref="String.Empty" />.</returns>
        public DataTable GetCacheableTable(string ACacheableTable, out System.Type AType)		
		{
        	return GetCacheableTable((TCacheablePartnerTablesEnum)Enum.Parse(typeof(TCacheablePartnerTablesEnum), ACacheableTable), 
        	    String.Empty, false, out AType);
		}
#endregion ManualCode
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
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            String TableName = Enum.GetName(typeof(TCacheablePartnerTablesEnum), ACacheableTable);

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetCacheableTable called for table '" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!FCacheableTablesManager.IsTableCached(TableName))))
            {
                Boolean NewTransaction;
                TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {

                    switch(ACacheableTable)
                    {
                        case TCacheablePartnerTablesEnum.AddresseeTypeList:
                        {
                            DataTable TmpTable = PAddresseeTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.AcquisitionCodeList:
                        {
                            DataTable TmpTable = PAcquisitionAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.BusinessCodeList:
                        {
                            DataTable TmpTable = PBusinessAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.CurrencyCodeList:
                        {
                            DataTable TmpTable = ACurrencyAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DataLabelList:
                        {
                            DataTable TmpTable = PDataLabelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DataLabelUseList:
                        {
                            DataTable TmpTable = PDataLabelUseAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DataLabelLookupCategoryList:
                        {
                            DataTable TmpTable = PDataLabelLookupCategoryAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DataLabelLookupList:
                        {
                            DataTable TmpTable = PDataLabelLookupAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DenominationList:
                        {
                            DataTable TmpTable = PDenominationAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.InterestList:
                        {
                            DataTable TmpTable = PInterestAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.InterestCategoryList:
                        {
                            DataTable TmpTable = PInterestCategoryAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.LocationTypeList:
                        {
                            DataTable TmpTable = PLocationTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.MaritalStatusList:
                        {
                            DataTable TmpTable = PtMaritalStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.MethodOfContactList:
                        {
                            DataTable TmpTable = PMethodOfContactAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.OccupationList:
                        {
                            DataTable TmpTable = POccupationAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.PartnerStatusList:
                        {
                            DataTable TmpTable = PPartnerStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.PartnerTypeList:
                        {
                            DataTable TmpTable = PTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.ProposalStatusList:
                        {
                            DataTable TmpTable = PFoundationProposalStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.ProposalSubmissionTypeList:
                        {
                            DataTable TmpTable = PProposalSubmissionTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.RelationList:
                        {
                            DataTable TmpTable = PRelationAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.RelationCategoryList:
                        {
                            DataTable TmpTable = PRelationCategoryAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.UnitTypeList:
                        {
                            DataTable TmpTable = UUnitTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.CountyList:
                        {
                            DataTable TmpTable = GetCountyListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.FoundationOwnerList:
                        {
                            DataTable TmpTable = GetFoundationOwnerListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.InstalledSitesList:
                        {
                            DataTable TmpTable = GetInstalledSitesListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.CountryListFromExistingLocations:
                        {
                            DataTable TmpTable = GetCountryListFromExistingLocationsTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePartnerTablesEnum.DataLabelsForPartnerClassesList:
                        {
                            DataTable TmpTable = GetDataLabelsForPartnerClassesListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }

                        default:
                            // Unknown Standard Cacheable DataTable
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                                TableName + "' is not available as a Standard Cacheable Table");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
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
                        case TCacheablePartnerTablesEnum.AddresseeTypeList:
                            if (PAddresseeTypeAccess.SubmitChanges((PAddresseeTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.AcquisitionCodeList:
                            if (PAcquisitionAccess.SubmitChanges((PAcquisitionTable)ASubmitTable, SubmitChangesTransaction,
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
                        case TCacheablePartnerTablesEnum.CurrencyCodeList:
                            if (ACurrencyAccess.SubmitChanges((ACurrencyTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.DataLabelList:
                            if (PDataLabelAccess.SubmitChanges((PDataLabelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.DataLabelUseList:
                            if (PDataLabelUseAccess.SubmitChanges((PDataLabelUseTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.DataLabelLookupCategoryList:
                            if (PDataLabelLookupCategoryAccess.SubmitChanges((PDataLabelLookupCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.DataLabelLookupList:
                            if (PDataLabelLookupAccess.SubmitChanges((PDataLabelLookupTable)ASubmitTable, SubmitChangesTransaction,
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
                        case TCacheablePartnerTablesEnum.PartnerStatusList:
                            if (PPartnerStatusAccess.SubmitChanges((PPartnerStatusTable)ASubmitTable, SubmitChangesTransaction,
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
                        case TCacheablePartnerTablesEnum.ProposalStatusList:
                            if (PFoundationProposalStatusAccess.SubmitChanges((PFoundationProposalStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePartnerTablesEnum.ProposalSubmissionTypeList:
                            if (PProposalSubmissionTypeAccess.SubmitChanges((PProposalSubmissionTypeTable)ASubmitTable, SubmitChangesTransaction,
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
                        case TCacheablePartnerTablesEnum.UnitTypeList:
                            if (UUnitTypeAccess.SubmitChanges((UUnitTypeTable)ASubmitTable, SubmitChangesTransaction,
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
            /// If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
            /// Cache and inform all other Clients that they need to reload this Cacheable DataTable
            /// the next time something in the Client accesses it.
             */
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                Type TmpType;
                GetCacheableTable(ACacheableTable, String.Empty, true, out TmpType);
            }

            return SubmissionResult;
        }

        private DataTable GetCountyListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            return DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + PLocationTable.GetCountryCodeDBName() + ", " +
                PLocationTable.GetCountyDBName() + " FROM PUB." +
                PLocationTable.GetTableDBName(), ATableName, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetFoundationOwnerListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            // Used in Foundation Details screen.
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
                " = FALSE", AReadTransaction, null, -1, -1);
            SUserRow EmptyDR = TmpUserTable.NewRowTyped(false);
            EmptyDR.PartnerKey = 0;
            EmptyDR.UserId = "";
            TmpUserTable.Rows.InsertAt(EmptyDR, 0);
            return TmpUserTable;
#endregion ManualCode
        }

        private DataTable GetInstalledSitesListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            // Used eg. in New Partner Dialog.
            StringCollection RequiredColumns = new StringCollection();
            RequiredColumns.Add(PPartnerLedgerTable.GetPartnerKeyDBName());
            PPartnerLedgerTable TmpInstalledSitesDT = PPartnerLedgerAccess.LoadAll(RequiredColumns, AReadTransaction, null, 0, 0);

            if (TmpInstalledSitesDT.Rows.Count != 0)
            {
                TmpInstalledSitesDT.Columns.Remove(PPartnerLedgerTable.GetLastPartnerIdDBName());
                TmpInstalledSitesDT.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), System.Type.GetType("System.String"));
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());

                for (int Counter = 0; Counter <= TmpInstalledSitesDT.Rows.Count - 1; Counter += 1)
                {
                    PPartnerTable PartnerDT = PPartnerAccess.LoadByPrimaryKey(
                        TmpInstalledSitesDT[Counter].PartnerKey,
                        RequiredColumns,
                        AReadTransaction,
                        null,
                        0,
                        0);
                    TmpInstalledSitesDT[Counter][PPartnerTable.GetPartnerShortNameDBName()] = PartnerDT[0].PartnerShortName;
                }
            }

            return TmpInstalledSitesDT;
#endregion ManualCode
        }

        private DataTable GetCountryListFromExistingLocationsTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            // Used eg. in Report Gift Data Export for finding donors.
            return DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + "PUB." + PCountryTable.GetTableDBName() + '.' +
                PCountryTable.GetCountryCodeDBName() + ", " +
                PCountryTable.GetCountryNameDBName() + " FROM PUB." +
                PCountryTable.GetTableDBName() + " c, PUB." +
                PLocationTable.GetTableDBName() + " l " +
                " WHERE " + PLocationTable.GetCountyDBName() + " IS NOT NULL AND NOT " +
                PLocationTable.GetCountyDBName() + " = ''" +
                " AND c." + PCountryTable.GetCountryCodeDBName() + " = l." +
                PLocationTable.GetCountryCodeDBName(), ATableName, AReadTransaction);
#endregion ManualCode
        }

        private DataTable GetDataLabelsForPartnerClassesListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
                const string PARTNERCLASSCOL = "PartnerClass";
                const string DLAVAILCOL = "DataLabelsAvailable";

                DataTable TmpTable;
                DataRow NewDR;
                TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;

                // Create our custom Cacheable DataTable on-the-fly
                TmpTable = new DataTable(ATableName);
                TmpTable.Columns.Add(new DataColumn(PARTNERCLASSCOL, System.Type.GetType("System.String")));
                TmpTable.Columns.Add(new DataColumn(DLAVAILCOL, System.Type.GetType("System.Boolean")));


                /*
                 * Create an Instance of TOfficeSpecificDataLabelsUIConnector - PartnerKey and DataLabelUse are not important here
                 * because we only call Method 'CountLabelUse', which doesn't rely on any of them.
                 */
                OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(0,
                    TOfficeSpecificDataLabelUseEnum.Family);

                // DataLabels available for PERSONs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for FAMILYs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for CHURCHes?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.CHURCH);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for ORGANISATIONs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for UNITs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for BANKs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.BANK);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                // DataLabels available for VENUEs?
                NewDR = TmpTable.NewRow();
                NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.VENUE);
                NewDR[DLAVAILCOL] =
                    (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
                TmpTable.Rows.Add(NewDR);

                return TmpTable;
#endregion ManualCode        	
        }
    }
}
