//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Specialized;
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
using Ict.Petra.Server.App.Core;
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
        /// Simple data reader.
        /// </summary>
        /// <param name="ATablename">DB Table name as in the DB schema</param>
        /// <param name="ASearchCriteria">A set of search criteria.</param>
        /// <param name="AResultTable">Returns typed datatable.</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static bool GetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable)
        {
            TDBTransaction ReadTransaction = null;
            TTypedDataTable ResultTable = null;

            // Automatic handling of a Read-only DB Transaction - and also the automatic establishment and closing of a DB
            // Connection where a DB Transaction can be exectued (only if that should be needed).
            DBAccess.SimpleAutoReadTransactionWrapper("TCommonDataReader.GetData", out ReadTransaction,
                delegate
                {
                    GetData(ATablename, ASearchCriteria, out ResultTable, ReadTransaction);
                });

            AResultTable = ResultTable;

            return true;
        }

        /// <summary>
        /// Simple data reader.
        /// </summary>
        /// <param name="ATablename">DB Table name as in the DB schema.</param>
        /// <param name="ASearchCriteria">A set of search criteria.</param>
        /// <param name="AResultTable">Returns typed datatable.</param>
        /// <param name="AReadTransaction">An instantiated DB Transaction.</param>
        [NoRemoting]
        public static void GetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable,
            TDBTransaction AReadTransaction)
        {
            AResultTable = null;

            // TODO: check access permissions for the current user

            // TODO: auto generate
            if (ATablename == AApSupplierTable.GetTableDBName())
            {
                AResultTable = AApSupplierAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == AApDocumentTable.GetTableDBName())
            {
                AResultTable = AApDocumentAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == ATransactionTypeTable.GetTableDBName())
            {
                AResultTable = ATransactionTypeAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == ACurrencyTable.GetTableDBName())
            {
                AResultTable = ACurrencyAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == ADailyExchangeRateTable.GetTableDBName())
            {
                AResultTable = ADailyExchangeRateAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == ACorporateExchangeRateTable.GetTableDBName())
            {
                AResultTable = ACorporateExchangeRateAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == ACurrencyLanguageTable.GetTableDBName())
            {
                AResultTable = ACurrencyLanguageAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == AFeesPayableTable.GetTableDBName())
            {
                AResultTable = AFeesPayableAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == AFeesReceivableTable.GetTableDBName())
            {
                AResultTable = AFeesReceivableAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == AAnalysisTypeTable.GetTableDBName())
            {
                AResultTable = AAnalysisTypeAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == AGiftBatchTable.GetTableDBName())
            {
                AResultTable = AGiftBatchAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == AJournalTable.GetTableDBName())
            {
                AResultTable = AJournalAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == ALedgerTable.GetTableDBName())
            {
                AResultTable = ALedgerAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == MExtractMasterTable.GetTableDBName())
            {
                if (ASearchCriteria == null)
                {
                    AResultTable = MExtractMasterAccess.LoadAll(AReadTransaction);
                }
                else
                {
                    AResultTable = MExtractMasterAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
                }
            }
            else if (ATablename == MExtractTable.GetTableDBName())
            {
                // it does not make sense to load ALL extract rows for all extract masters so search criteria needs to be set
                if (ASearchCriteria != null)
                {
                    AResultTable = MExtractAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
                }
            }
            else if (ATablename == PcAttendeeTable.GetTableDBName())
            {
                AResultTable = PcAttendeeAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PcConferenceCostTable.GetTableDBName())
            {
                AResultTable = PcConferenceCostAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PcEarlyLateTable.GetTableDBName())
            {
                AResultTable = PcEarlyLateAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PcSupplementTable.GetTableDBName())
            {
                AResultTable = PcSupplementAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PcDiscountTable.GetTableDBName())
            {
                AResultTable = PcDiscountAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PFormTable.GetTableDBName())
            {
                string[] columns = TTypedDataTable.GetColumnStringList(PFormTable.TableId);
                StringCollection fieldList = new StringCollection();

                for (int i = 0; i < columns.Length; i++)
                {
                    // Do not load the template document - we don't display it and it is big!
                    if (columns[i] != PFormTable.GetTemplateDocumentDBName())
                    {
                        fieldList.Add(columns[i]);
                    }
                }

                AResultTable = PFormAccess.LoadAll(fieldList, AReadTransaction);
            }
            else if (ATablename == PInternationalPostalTypeTable.GetTableDBName())
            {
                AResultTable = PInternationalPostalTypeAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == PtApplicationTypeTable.GetTableDBName())
            {
                AResultTable = PtApplicationTypeAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == PFormalityTable.GetTableDBName())
            {
                AResultTable = PFormalityAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == PMailingTable.GetTableDBName())
            {
                AResultTable = PMailingAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == PPartnerGiftDestinationTable.GetTableDBName())
            {
                AResultTable = PPartnerGiftDestinationAccess.LoadUsingTemplate(ASearchCriteria, AReadTransaction);
            }
            else if (ATablename == PmDocumentTypeTable.GetTableDBName())
            {
                AResultTable = PmDocumentTypeAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == SGroupTable.GetTableDBName())
            {
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_SYSADMIN) == false)
                {
                    throw new ESecurityAccessDeniedException(string.Format("No access for user {0} to Module {1}.",
                            UserInfo.GUserInfo.UserID, SharedConstants.PETRAMODULE_SYSADMIN));
                }

                AResultTable = SGroupAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == SSystemDefaultsTable.GetTableDBName())
            {
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_SYSADMIN) == false)
                {
                    throw new ESecurityAccessDeniedException(string.Format("No access for user {0} to Module {1}.",
                            UserInfo.GUserInfo.UserID, SharedConstants.PETRAMODULE_SYSADMIN));
                }

                AResultTable = SSystemDefaultsAccess.LoadAll(AReadTransaction);
            }
            else if (ATablename == SSystemDefaultsGuiTable.GetTableDBName())
            {
                AResultTable = SSystemDefaultsGuiAccess.LoadAll(AReadTransaction);
            }
            else
            {
                throw new Exception("TCommonDataReader.GetData: unknown table " + ATablename);
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            AResultTable.AcceptChanges();
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
            ref TTypedDataTable ASubmitTable, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;
            TDBTransaction WriteTransaction = null;
            TTypedDataTable SubmitTable = null;
            TVerificationResultCollection VerificationResult = null;

            SubmitTable = ASubmitTable;

            // Automatic handling of a DB Transaction - and also the automatic establishment and closing of a DB
            // Connection where a DB Transaction can be exectued (only if that should be needed).
            DBAccess.SimpleAutoTransactionWrapper("TCommonDataReader.SaveData", out WriteTransaction,
                ref ReturnValue,
                delegate
                {
                    ReturnValue = SaveData(ATablename, ref SubmitTable, out VerificationResult, WriteTransaction);
                });

            if ((ATablename == SSystemDefaultsTable.GetTableDBName()) && (ReturnValue == TSubmitChangesResult.scrOK))
            {
                // Refresh the cache immediately so clients will get the changes
                TSystemDefaultsCache.GSystemDefaultsCache.ReloadSystemDefaultsTable();
            }

            AVerificationResult = VerificationResult;

            return ReturnValue;
        }

        /// <summary>
        /// Generic method for saving some rows in a single DB table.
        /// </summary>
        /// <param name="ATablename">DB Table name as in the DB schema.</param>
        /// <param name="ASubmitTable">The DB Table as a Typed DataTable.</param>
        /// <param name="AVerificationResult">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data saving errors occur.</param>
        /// <param name="AWriteTransaction">An instantiated DB Transaction.</param>
        /// <returns></returns>
        [NoRemoting]
        public static TSubmitChangesResult SaveData(string ATablename,
            ref TTypedDataTable ASubmitTable, out TVerificationResultCollection AVerificationResult,
            TDBTransaction AWriteTransaction)
        {
            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();

                try
                {
                    if (ATablename == AAccountingPeriodTable.GetTableDBName())
                    {
                        AAccountingPeriodAccess.SubmitChanges((AAccountingPeriodTable)ASubmitTable, AWriteTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());
                    }
                    else if (ATablename == ACurrencyTable.GetTableDBName())
                    {
                        ACurrencyAccess.SubmitChanges((ACurrencyTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ADailyExchangeRateTable.GetTableDBName())
                    {
                        ADailyExchangeRateAccess.SubmitChanges((ADailyExchangeRateTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ACorporateExchangeRateTable.GetTableDBName())
                    {
                        ACorporateExchangeRateAccess.SubmitChanges((ACorporateExchangeRateTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ACurrencyLanguageTable.GetTableDBName())
                    {
                        ACurrencyLanguageAccess.SubmitChanges((ACurrencyLanguageTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == AFeesPayableTable.GetTableDBName())
                    {
                        AFeesPayableAccess.SubmitChanges((AFeesPayableTable)ASubmitTable, AWriteTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.FeesPayableList.ToString());
                    }
                    else if (ATablename == AFeesReceivableTable.GetTableDBName())
                    {
                        AFeesReceivableAccess.SubmitChanges((AFeesReceivableTable)ASubmitTable, AWriteTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.FeesReceivableList.ToString());
                    }
                    else if (ATablename == AGiftBatchTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the gift batch screens, which use a TDS
                        AGiftBatchAccess.SubmitChanges((AGiftBatchTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == AJournalTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the journal screens, which use a TDS
                        AJournalAccess.SubmitChanges((AJournalTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ARecurringJournalTable.GetTableDBName())
                    {
                        // This method is called from Submit Recurring GL Batch form - please do not remove
                        // The method is not required for changes made to the journal screens, which use a TDS
                        ARecurringJournalAccess.SubmitChanges((ARecurringJournalTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ALedgerTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Testing - please do not remove
                        ALedgerAccess.SubmitChanges((ALedgerTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == AAnalysisTypeTable.GetTableDBName())
                    {
                        AAnalysisTypeAccess.SubmitChanges((AAnalysisTypeTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == ASuspenseAccountTable.GetTableDBName())
                    {
                        ASuspenseAccountAccess.SubmitChanges((ASuspenseAccountTable)ASubmitTable, AWriteTransaction);

                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.SuspenseAccountList.ToString());
                    }
                    else if (ATablename == PcAttendeeTable.GetTableDBName())
                    {
                        PcAttendeeAccess.SubmitChanges((PcAttendeeTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PcConferenceTable.GetTableDBName())
                    {
                        PcConferenceAccess.SubmitChanges((PcConferenceTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PcConferenceCostTable.GetTableDBName())
                    {
                        PcConferenceCostAccess.SubmitChanges((PcConferenceCostTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PcEarlyLateTable.GetTableDBName())
                    {
                        PcEarlyLateAccess.SubmitChanges((PcEarlyLateTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PcSupplementTable.GetTableDBName())
                    {
                        PcSupplementAccess.SubmitChanges((PcSupplementTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PcDiscountTable.GetTableDBName())
                    {
                        PcDiscountAccess.SubmitChanges((PcDiscountTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PInternationalPostalTypeTable.GetTableDBName())
                    {
                        ValidateInternationalPostalType(ref AVerificationResult, ASubmitTable);
                        ValidateInternationalPostalTypeManual(ref AVerificationResult, ASubmitTable);

                        if (TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                        {
                            PInternationalPostalTypeAccess.SubmitChanges((PInternationalPostalTypeTable)ASubmitTable, AWriteTransaction);
                        }
                    }
                    else if (ATablename == PtApplicationTypeTable.GetTableDBName())
                    {
                        PtApplicationTypeAccess.SubmitChanges((PtApplicationTypeTable)ASubmitTable, AWriteTransaction);

                        // mark dependent lists for needing to be refreshed since there was a change in base list
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.EventApplicationTypeList.ToString());
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.FieldApplicationTypeList.ToString());
                    }
                    else if (ATablename == PFormTable.GetTableDBName())
                    {
                        PFormAccess.SubmitChanges((PFormTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PFormalityTable.GetTableDBName())
                    {
                        PFormalityAccess.SubmitChanges((PFormalityTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PMailingTable.GetTableDBName())
                    {
                        PMailingAccess.SubmitChanges((PMailingTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PPartnerGiftDestinationTable.GetTableDBName())
                    {
                        PPartnerGiftDestinationAccess.SubmitChanges((PPartnerGiftDestinationTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == PmDocumentTypeTable.GetTableDBName())
                    {
                        PmDocumentTypeAccess.SubmitChanges((PmDocumentTypeTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == SGroupTable.GetTableDBName())
                    {
                        SGroupAccess.SubmitChanges((SGroupTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == SSystemDefaultsTable.GetTableDBName())
                    {
                        SSystemDefaultsAccess.SubmitChanges((SSystemDefaultsTable)ASubmitTable, AWriteTransaction);
                    }
                    else if (ATablename == SSystemDefaultsGuiTable.GetTableDBName())
                    {
                        SSystemDefaultsGuiAccess.SubmitChanges((SSystemDefaultsGuiTable)ASubmitTable, AWriteTransaction);
                    }
                    else
                    {
                        throw new EOPAppException("TCommonDataReader.SaveData: unknown table '" + ATablename + "'");
                    }
                }
                catch (Exception Exc)
                {
                    AVerificationResult.Add(
                        new TVerificationResult(null, "Cannot SubmitChanges:" + Environment.NewLine +
                            Exc.Message, "UNDEFINED", TResultSeverity.Resv_Critical));
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