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
using Ict.Petra.Server.App.Core;

#region ManualCode
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MPersonnel.Personnel.Validation;
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
    public partial class TPersonnelCacheable : TCacheableTablesLoader
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
            FCacheableTablesManager = TCacheableTablesManager.GCacheableTablesManager;
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

#region ManualCode
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Wrapper for other GetCacheableTable method
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <returns>DataTable</returns>
        public DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
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
                        case TCacheablePersonTablesEnum.CommitmentStatusList:
                        {
                            DataTable TmpTable = PmCommitmentStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DocumentTypeList:
                        {
                            DataTable TmpTable = PmDocumentTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DocumentTypeCategoryList:
                        {
                            DataTable TmpTable = PmDocumentCategoryAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.AbilityAreaList:
                        {
                            DataTable TmpTable = PtAbilityAreaAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.AbilityLevelList:
                        {
                            DataTable TmpTable = PtAbilityLevelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ApplicantStatusList:
                        {
                            DataTable TmpTable = PtApplicantStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ApplicationTypeList:
                        {
                            DataTable TmpTable = PtApplicationTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ArrivalDeparturePointList:
                        {
                            DataTable TmpTable = PtArrivalPointAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.EventRoleList:
                        {
                            DataTable TmpTable = PtCongressCodeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.ContactList:
                        {
                            DataTable TmpTable = PtContactAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.DriverStatusList:
                        {
                            DataTable TmpTable = PtDriverStatusAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.LanguageLevelList:
                        {
                            DataTable TmpTable = PtLanguageLevelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.LeadershipRatingList:
                        {
                            DataTable TmpTable = PtLeadershipRatingAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.PassportTypeList:
                        {
                            DataTable TmpTable = PtPassportTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.TransportTypeList:
                        {
                            DataTable TmpTable = PtTravelTypeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.QualificationAreaList:
                        {
                            DataTable TmpTable = PtQualificationAreaAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.QualificationLevelList:
                        {
                            DataTable TmpTable = PtQualificationLevelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.SkillCategoryList:
                        {
                            DataTable TmpTable = PtSkillCategoryAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.SkillLevelList:
                        {
                            DataTable TmpTable = PtSkillLevelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.OutreachPreferenceLevelList:
                        {
                            DataTable TmpTable = PtOutreachPreferenceLevelAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.EventApplicationTypeList:
                        {
                            DataTable TmpTable = GetEventApplicationTypeListTable(ReadTransaction, TableName);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheablePersonTablesEnum.FieldApplicationTypeList:
                        {
                            DataTable TmpTable = GetFieldApplicationTypeListTable(ReadTransaction, TableName);
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
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PmCommitmentStatusValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateCommitmentStatusListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PmCommitmentStatusAccess.SubmitChanges((PmCommitmentStatusTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.DocumentTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PmDocumentTypeValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateDocumentTypeListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PmDocumentTypeAccess.SubmitChanges((PmDocumentTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.DocumentTypeCategoryList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PmDocumentCategoryValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateDocumentTypeCategoryListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PmDocumentCategoryAccess.SubmitChanges((PmDocumentCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.AbilityAreaList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtAbilityAreaValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateAbilityAreaListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtAbilityAreaAccess.SubmitChanges((PtAbilityAreaTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.AbilityLevelList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtAbilityLevelValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateAbilityLevelListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtAbilityLevelAccess.SubmitChanges((PtAbilityLevelTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.ApplicantStatusList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtApplicantStatusValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateApplicantStatusListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtApplicantStatusAccess.SubmitChanges((PtApplicantStatusTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.ApplicationTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtApplicationTypeValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateApplicationTypeListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtApplicationTypeAccess.SubmitChanges((PtApplicationTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.ArrivalDeparturePointList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtArrivalPointValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateArrivalDeparturePointListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtArrivalPointAccess.SubmitChanges((PtArrivalPointTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.EventRoleList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtCongressCodeValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateEventRoleListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtCongressCodeAccess.SubmitChanges((PtCongressCodeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.ContactList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtContactValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateContactListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtContactAccess.SubmitChanges((PtContactTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.DriverStatusList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtDriverStatusValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateDriverStatusListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtDriverStatusAccess.SubmitChanges((PtDriverStatusTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.LanguageLevelList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtLanguageLevelValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateLanguageLevelListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtLanguageLevelAccess.SubmitChanges((PtLanguageLevelTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.LeadershipRatingList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtLeadershipRatingValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateLeadershipRatingListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtLeadershipRatingAccess.SubmitChanges((PtLeadershipRatingTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.PassportTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtPassportTypeValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidatePassportTypeListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtPassportTypeAccess.SubmitChanges((PtPassportTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.TransportTypeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtTravelTypeValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateTransportTypeListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtTravelTypeAccess.SubmitChanges((PtTravelTypeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.QualificationAreaList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtQualificationAreaValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateQualificationAreaListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtQualificationAreaAccess.SubmitChanges((PtQualificationAreaTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.QualificationLevelList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtQualificationLevelValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateQualificationLevelListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtQualificationLevelAccess.SubmitChanges((PtQualificationLevelTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.SkillCategoryList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtSkillCategoryValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateSkillCategoryListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtSkillCategoryAccess.SubmitChanges((PtSkillCategoryTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.SkillLevelList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtSkillLevelValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateSkillLevelListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtSkillLevelAccess.SubmitChanges((PtSkillLevelTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheablePersonTablesEnum.OutreachPreferenceLevelList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                PtOutreachPreferenceLevelValidation.Validate(ASubmitTable, ref AVerificationResult);
                                ValidateOutreachPreferenceLevelListManual(ref AVerificationResult, ASubmitTable);

                                if (!AVerificationResult.HasCriticalErrors)
                                {
                                    if (PtOutreachPreferenceLevelAccess.SubmitChanges((PtOutreachPreferenceLevelTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
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

            // If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
            // Cache and inform all other Clients that they need to reload this Cacheable DataTable
            // the next time something in the Client accesses it.
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                Type TmpType;
                GetCacheableTable(ACacheableTable, String.Empty, true, out TmpType);
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return SubmissionResult;
        }

#region Data Validation

        partial void ValidateCommitmentStatusListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateDocumentTypeListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateDocumentTypeCategoryListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateAbilityAreaListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateAbilityLevelListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateApplicantStatusListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateApplicationTypeListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateArrivalDeparturePointListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateEventRoleListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateContactListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateDriverStatusListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateLanguageLevelListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateLeadershipRatingListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidatePassportTypeListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateTransportTypeListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateQualificationAreaListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateQualificationLevelListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateSkillCategoryListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateSkillLevelListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateOutreachPreferenceLevelListManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

#endregion Data Validation

        private DataTable GetEventApplicationTypeListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            PtApplicationTypeRow template = new PtApplicationTypeTable().NewRowTyped(false);
            template.AppFormType = "SHORT FORM";
            return PtApplicationTypeAccess.LoadUsingTemplate(template, AReadTransaction);
#endregion ManualCode        
        }

        private DataTable GetFieldApplicationTypeListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            PtApplicationTypeRow template = new PtApplicationTypeTable().NewRowTyped(false);
            template.AppFormType = "LONG FORM";
            return PtApplicationTypeAccess.LoadUsingTemplate(template, AReadTransaction);
#endregion ManualCode        
        }
    }
}
