// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
using Ict.Petra.Shared;
using Ict.Petra.Server.App.ClientDomain;

#region ManualCode
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MCommon;
#endregion ManualCode
namespace Ict.Petra.Server.MPersonnel.Person.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MPersonnel.Person sub-namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public class TPersonnelCacheable : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public TPersonnelCacheable() : base()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
            FCacheableTablesManager = DomainManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// destructor
        ~TPersonnelCacheable()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif

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
        public DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            String TableName = Enum.GetName(typeof(TCacheablePersonTablesEnum), ACacheableTable);

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetCacheableTable called for table '" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
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
                        case TCacheablePersonTablesEnum.CommitmentStatusList:
                        {
                            DataTable TmpTable = PmCommitmentStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DocumentTypeList:
                        {
                            DataTable TmpTable = PmDocumentTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DocumentTypeCategoryList:
                        {
                            DataTable TmpTable = PmDocumentCategoryAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.AbilityAreaList:
                        {
                            DataTable TmpTable = PtAbilityAreaAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.AbilityLevelList:
                        {
                            DataTable TmpTable = PtAbilityLevelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ApplicantStatusList:
                        {
                            DataTable TmpTable = PtApplicantStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ArrivalDeparturePointList:
                        {
                            DataTable TmpTable = PtArrivalPointAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.EventRoleList:
                        {
                            DataTable TmpTable = PtCongressCodeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ContactList:
                        {
                            DataTable TmpTable = PtContactAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DriverStatusList:
                        {
                            DataTable TmpTable = PtDriverStatusAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.LanguageLevelList:
                        {
                            DataTable TmpTable = PtLanguageLevelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.LeadershipRatingList:
                        {
                            DataTable TmpTable = PtLeadershipRatingAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.PartyTypeList:
                        {
                            DataTable TmpTable = PtPartyTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.PassportTypeList:
                        {
                            DataTable TmpTable = PtPassportTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.TransportTypeList:
                        {
                            DataTable TmpTable = PtTravelTypeAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.QualificationAreaList:
                        {
                            DataTable TmpTable = PtQualificationAreaAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.QualificationLevelList:
                        {
                            DataTable TmpTable = PtQualificationLevelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ValuableItemList:
                        {
                            DataTable TmpTable = PtValuableItemAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.VisionAreaList:
                        {
                            DataTable TmpTable = PtVisionAreaAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.VisionLevelList:
                        {
                            DataTable TmpTable = PtVisionLevelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.OutreachPreferenceLevelList:
                        {
                            DataTable TmpTable = PtOutreachPreferenceLevelAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
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
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            string CacheableDTName = Enum.GetName(typeof(TCacheablePersonTablesEnum), ACacheableTable);

            // Console.WriteLine("Entering Person.SaveChangedStandardCacheableTable...");
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
                        case TCacheablePersonTablesEnum.CommitmentStatusList:
                            if (PmCommitmentStatusAccess.SubmitChanges((PmCommitmentStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.DocumentTypeList:
                            if (PmDocumentTypeAccess.SubmitChanges((PmDocumentTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.DocumentTypeCategoryList:
                            if (PmDocumentCategoryAccess.SubmitChanges((PmDocumentCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.AbilityAreaList:
                            if (PtAbilityAreaAccess.SubmitChanges((PtAbilityAreaTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.AbilityLevelList:
                            if (PtAbilityLevelAccess.SubmitChanges((PtAbilityLevelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.ApplicantStatusList:
                            if (PtApplicantStatusAccess.SubmitChanges((PtApplicantStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.ArrivalDeparturePointList:
                            if (PtArrivalPointAccess.SubmitChanges((PtArrivalPointTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.EventRoleList:
                            if (PtCongressCodeAccess.SubmitChanges((PtCongressCodeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.ContactList:
                            if (PtContactAccess.SubmitChanges((PtContactTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.DriverStatusList:
                            if (PtDriverStatusAccess.SubmitChanges((PtDriverStatusTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.LanguageLevelList:
                            if (PtLanguageLevelAccess.SubmitChanges((PtLanguageLevelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.LeadershipRatingList:
                            if (PtLeadershipRatingAccess.SubmitChanges((PtLeadershipRatingTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.PartyTypeList:
                            if (PtPartyTypeAccess.SubmitChanges((PtPartyTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.PassportTypeList:
                            if (PtPassportTypeAccess.SubmitChanges((PtPassportTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.TransportTypeList:
                            if (PtTravelTypeAccess.SubmitChanges((PtTravelTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.QualificationAreaList:
                            if (PtQualificationAreaAccess.SubmitChanges((PtQualificationAreaTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.QualificationLevelList:
                            if (PtQualificationLevelAccess.SubmitChanges((PtQualificationLevelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.ValuableItemList:
                            if (PtValuableItemAccess.SubmitChanges((PtValuableItemTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.VisionAreaList:
                            if (PtVisionAreaAccess.SubmitChanges((PtVisionAreaTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.VisionLevelList:
                            if (PtVisionLevelAccess.SubmitChanges((PtVisionLevelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        case TCacheablePersonTablesEnum.OutreachPreferenceLevelList:
                            if (PtOutreachPreferenceLevelAccess.SubmitChanges((PtOutreachPreferenceLevelTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            break;
                        default:

                            throw new Exception(
                            "TPersonnelCacheable.SaveChangedStandardCacheableTable: unsupported Cacheabled DataTable '" + CacheableDTName + "'");
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
                        "TPersonnelCacheable.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheabled DataTable '" +
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
    }
}
