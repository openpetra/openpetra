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
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core;

#region ManualCode
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MCommon;
#endregion ManualCode
namespace Ict.Petra.Server.MPartner.Mailing.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MPartner.Mailing sub-namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public partial class TPartnerCacheable : TCacheableTablesLoader
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
        public DataTable GetCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            String TableName = Enum.GetName(typeof(TCacheableMailingTablesEnum), ACacheableTable);

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
                        case TCacheableMailingTablesEnum.ContactAttributeList:
                        {
                            DataTable TmpTable = PContactAttributeAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.ContactAttributeDetailList:
                        {
                            DataTable TmpTable = PContactAttributeDetailAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.MethodOfContactList:
                        {
                            DataTable TmpTable = PMethodOfContactAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.MergeFormList:
                        {
                            DataTable TmpTable = PMergeFormAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.MergeFieldList:
                        {
                            DataTable TmpTable = PMergeFieldAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.MailingList:
                        {
                            DataTable TmpTable = PMailingAccess.LoadAll(ReadTransaction);
                            FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;
                        }
                        case TCacheableMailingTablesEnum.PostCodeRegionList:
                        {
                            DataTable TmpTable = GetPostCodeRegionListTable(ReadTransaction, TableName);
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
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();
            string CacheableDTName = Enum.GetName(typeof(TCacheableMailingTablesEnum), ACacheableTable);

            // Console.WriteLine("Entering Mailing.SaveChangedStandardCacheableTable...");
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
                        case TCacheableMailingTablesEnum.ContactAttributeList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateContactAttributeList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateContactAttributeListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PContactAttributeAccess.SubmitChanges((PContactAttributeTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableMailingTablesEnum.ContactAttributeDetailList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateContactAttributeDetailList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateContactAttributeDetailListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PContactAttributeDetailAccess.SubmitChanges((PContactAttributeDetailTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableMailingTablesEnum.MethodOfContactList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateMethodOfContactList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateMethodOfContactListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PMethodOfContactAccess.SubmitChanges((PMethodOfContactTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableMailingTablesEnum.MergeFormList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateMergeFormList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateMergeFormListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PMergeFormAccess.SubmitChanges((PMergeFormTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableMailingTablesEnum.MergeFieldList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateMergeFieldList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateMergeFieldListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PMergeFieldAccess.SubmitChanges((PMergeFieldTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
                            }

                            break;
                        case TCacheableMailingTablesEnum.MailingList:
                            if (ASubmitTable.Rows.Count > 0)
                            {
                                ValidateMailingList(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                                ValidateMailingListManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                                if (AVerificationResult.Count == 0)
                                {
                                    if (PMailingAccess.SubmitChanges((PMailingTable)ASubmitTable, SubmitChangesTransaction,
                                        out SingleVerificationResultCollection))
                                    {
                                        SubmissionResult = TSubmitChangesResult.scrOK;
                                    }
                                }
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

        partial void ValidateContactAttributeList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateContactAttributeListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateContactAttributeDetailList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateContactAttributeDetailListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMethodOfContactList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMethodOfContactListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMergeFormList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMergeFormListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMergeFieldList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMergeFieldListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMailingList(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        partial void ValidateMailingListManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

#endregion Data Validation

        private DataTable GetPostCodeRegionListTable(TDBTransaction AReadTransaction, string ATableName)
        {
#region ManualCode
            // Used eg. in Report Gift Data Export for finding donors by address.
            return DBAccess.GDBAccessObj.SelectDT(
                "SELECT DISTINCT " + PPostcodeRegionTable.GetRegionDBName() + " FROM PUB." + PPostcodeRegionTable.GetTableDBName(),
                ATableName,
                AReadTransaction);
#endregion ManualCode
        }
    }
}
