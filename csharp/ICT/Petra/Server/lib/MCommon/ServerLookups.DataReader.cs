//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;

namespace Ict.Petra.Server.MCommon.DataReader.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public partial class TCommonDataReader
    {
        /// <summary>
        /// simple data reader;
        /// checks for permissions of the current user;
        /// </summary>
        /// <param name="ATablename"></param>
        /// <param name="ASearchCriteria">a set of search criteria</param>
        /// <param name="AResultTable">returns typed datatable</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static bool GetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable)
        {
            // TODO: check access permissions for the current user

            bool NewTransaction = false;
            TDBTransaction ReadTransaction;

            TTypedDataTable tempTable = null;

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                // TODO: auto generate
                if (ATablename == AApSupplierTable.GetTableDBName())
                {
                    tempTable = AApSupplierAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == AApDocumentTable.GetTableDBName())
                {
                    tempTable = AApDocumentAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == ATransactionTypeTable.GetTableDBName())
                {
                    tempTable = ATransactionTypeAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == ACurrencyTable.GetTableDBName())
                {
                    tempTable = ACurrencyAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == ADailyExchangeRateTable.GetTableDBName())
                {
                    tempTable = ADailyExchangeRateAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == ACorporateExchangeRateTable.GetTableDBName())
                {
                    tempTable = ACorporateExchangeRateAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == ACurrencyLanguageTable.GetTableDBName())
                {
                    tempTable = ACurrencyLanguageAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == AFeesPayableTable.GetTableDBName())
                {
                    tempTable = AFeesPayableAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == AFeesReceivableTable.GetTableDBName())
                {
                    tempTable = AFeesReceivableAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == AAnalysisTypeTable.GetTableDBName())
                {
                    tempTable = AAnalysisTypeAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == AGiftBatchTable.GetTableDBName())
                {
                    tempTable = AGiftBatchAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == AJournalTable.GetTableDBName())
                {
                    tempTable = AJournalAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == ALedgerTable.GetTableDBName())
                {
                    tempTable = ALedgerAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == MExtractMasterTable.GetTableDBName())
                {
                    if (ASearchCriteria == null)
                    {
                        tempTable = MExtractMasterAccess.LoadAll(ReadTransaction);
                    }
                    else
                    {
                        tempTable = MExtractMasterAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                    }
                }
                else if (ATablename == MExtractTable.GetTableDBName())
                {
                    // it does not make sense to load ALL extract rows for all extract masters so search criteria needs to be set
                    if (ASearchCriteria != null)
                    {
                        tempTable = MExtractAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                    }
                }
                else if (ATablename == PcAttendeeTable.GetTableDBName())
                {
                    tempTable = PcAttendeeAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PcConferenceCostTable.GetTableDBName())
                {
                    tempTable = PcConferenceCostAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PcEarlyLateTable.GetTableDBName())
                {
                    tempTable = PcEarlyLateAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PcSupplementTable.GetTableDBName())
                {
                    tempTable = PcSupplementAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PcDiscountTable.GetTableDBName())
                {
                    tempTable = PcDiscountAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PInternationalPostalTypeTable.GetTableDBName())
                {
                    tempTable = PInternationalPostalTypeAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == PtApplicationTypeTable.GetTableDBName())
                {
                    tempTable = PtApplicationTypeAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == PMailingTable.GetTableDBName())
                {
                    tempTable = PMailingAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == PPartnerFieldOfServiceTable.GetTableDBName())
                {
                    tempTable = PPartnerFieldOfServiceAccess.LoadUsingTemplate(ASearchCriteria, ReadTransaction);
                }
                else if (ATablename == PmDocumentTypeTable.GetTableDBName())
                {
                    tempTable = PmDocumentTypeAccess.LoadAll(ReadTransaction);
                }
                else if (ATablename == SGroupTable.GetTableDBName())
                {
                    tempTable = SGroupAccess.LoadAll(ReadTransaction);
                }
                else
                {
                    throw new Exception("TCommonDataReader.GetData: unknown table " + ATablename);
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.Log("TCommonDataReader.GetData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TCommonDataReader.GetData: committed own transaction.");
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            tempTable.AcceptChanges();

            // return the table
            AResultTable = tempTable;

            return true;
        }

        /// <summary>
        /// generic function for saving some rows in a single table
        /// </summary>
        /// <param name="ATablename"></param>
        /// <param name="ASubmitTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static TSubmitChangesResult SaveData(string ATablename,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;

            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();

                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    if (ATablename == AAccountingPeriodTable.GetTableDBName())
                    {
                        AAccountingPeriodAccess.SubmitChanges((AAccountingPeriodTable)ASubmitTable, SubmitChangesTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());
                    }
                    else if (ATablename == ACurrencyTable.GetTableDBName())
                    {
                        ACurrencyAccess.SubmitChanges((ACurrencyTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == ADailyExchangeRateTable.GetTableDBName())
                    {
                        ADailyExchangeRateAccess.SubmitChanges((ADailyExchangeRateTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == ACorporateExchangeRateTable.GetTableDBName())
                    {
                        ACorporateExchangeRateAccess.SubmitChanges((ACorporateExchangeRateTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == ACurrencyLanguageTable.GetTableDBName())
                    {
                        ACurrencyLanguageAccess.SubmitChanges((ACurrencyLanguageTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == AFeesPayableTable.GetTableDBName())
                    {
                        AFeesPayableAccess.SubmitChanges((AFeesPayableTable)ASubmitTable, SubmitChangesTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.FeesPayableList.ToString());
                    }
                    else if (ATablename == AFeesReceivableTable.GetTableDBName())
                    {
                        AFeesReceivableAccess.SubmitChanges((AFeesReceivableTable)ASubmitTable, SubmitChangesTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.FeesReceivableList.ToString());
                    }
                    else if (ATablename == AGiftBatchTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the gift batch screens, which use a TDS
                        AGiftBatchAccess.SubmitChanges((AGiftBatchTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == AJournalTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the journal screens, which use a TDS
                        AJournalAccess.SubmitChanges((AJournalTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == ALedgerTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Testing - please do not remove
                        ALedgerAccess.SubmitChanges((ALedgerTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == AAnalysisTypeTable.GetTableDBName())
                    {
                        AAnalysisTypeAccess.SubmitChanges((AAnalysisTypeTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == ASuspenseAccountTable.GetTableDBName())
                    {
                        ASuspenseAccountAccess.SubmitChanges((ASuspenseAccountTable)ASubmitTable, SubmitChangesTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.SuspenseAccountList.ToString());
                    }
                    else if (ATablename == PcAttendeeTable.GetTableDBName())
                    {
                        PcAttendeeAccess.SubmitChanges((PcAttendeeTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PcConferenceTable.GetTableDBName())
                    {
                        PcConferenceAccess.SubmitChanges((PcConferenceTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PcConferenceCostTable.GetTableDBName())
                    {
                        PcConferenceCostAccess.SubmitChanges((PcConferenceCostTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PcEarlyLateTable.GetTableDBName())
                    {
                        PcEarlyLateAccess.SubmitChanges((PcEarlyLateTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PcSupplementTable.GetTableDBName())
                    {
                        PcSupplementAccess.SubmitChanges((PcSupplementTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PcDiscountTable.GetTableDBName())
                    {
                        PcDiscountAccess.SubmitChanges((PcDiscountTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PInternationalPostalTypeTable.GetTableDBName())
                    {
                        ValidateInternationalPostalType(ref AVerificationResult, ASubmitTable);
                        ValidateInternationalPostalTypeManual(ref AVerificationResult, ASubmitTable);

                        if (TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                        {
                            PInternationalPostalTypeAccess.SubmitChanges((PInternationalPostalTypeTable)ASubmitTable, SubmitChangesTransaction);
                        }
                    }
                    else if (ATablename == PtApplicationTypeTable.GetTableDBName())
                    {
                        PtApplicationTypeAccess.SubmitChanges((PtApplicationTypeTable)ASubmitTable, SubmitChangesTransaction);

                        // mark dependent lists for needing to be refreshed since there was a change in base list
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.EventApplicationTypeList.ToString());
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.FieldApplicationTypeList.ToString());
                    }
                    else if (ATablename == PMailingTable.GetTableDBName())
                    {
                        PMailingAccess.SubmitChanges((PMailingTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PPartnerFieldOfServiceTable.GetTableDBName())
                    {
                        PPartnerFieldOfServiceAccess.SubmitChanges((PPartnerFieldOfServiceTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == PmDocumentTypeTable.GetTableDBName())
                    {
                        PmDocumentTypeAccess.SubmitChanges((PmDocumentTypeTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else if (ATablename == SGroupTable.GetTableDBName())
                    {
                        SGroupAccess.SubmitChanges((SGroupTable)ASubmitTable, SubmitChangesTransaction);
                    }
                    else
                    {
                        throw new EOPAppException("TCommonDataReader.SaveData: unknown table '" + ATablename + "'");
                    }

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("after submitchanges: exception " + Exc.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw;
                }
            }

            if ((AVerificationResult != null)
                && (AVerificationResult.Count > 0))
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);

                return AVerificationResult.HasCriticalErrors ? TSubmitChangesResult.scrError : TSubmitChangesResult.scrOK;
            }

            return TSubmitChangesResult.scrOK;
        }

        #region Data Validation

        static partial void ValidateInternationalPostalType(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateInternationalPostalTypeManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}