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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
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
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    if (ATablename == ACurrencyTable.GetTableDBName())
                    {
                        if (ACurrencyAccess.SubmitChanges((ACurrencyTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == ADailyExchangeRateTable.GetTableDBName())
                    {
                        if (ADailyExchangeRateAccess.SubmitChanges((ADailyExchangeRateTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == ACorporateExchangeRateTable.GetTableDBName())
                    {
                        if (ACorporateExchangeRateAccess.SubmitChanges((ACorporateExchangeRateTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == ACurrencyLanguageTable.GetTableDBName())
                    {
                        if (ACurrencyLanguageAccess.SubmitChanges((ACurrencyLanguageTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == AFeesPayableTable.GetTableDBName())
                    {
                        if (AFeesPayableAccess.SubmitChanges((AFeesPayableTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheableFinanceTablesEnum.FeesPayableList.ToString());

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == AFeesReceivableTable.GetTableDBName())
                    {
                        if (AFeesReceivableAccess.SubmitChanges((AFeesReceivableTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheableFinanceTablesEnum.FeesReceivableList.ToString());

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == AGiftBatchTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the gift batch screens, which use a TDS
                        if (AGiftBatchAccess.SubmitChanges((AGiftBatchTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == AJournalTable.GetTableDBName())
                    {
                        // This method is called from ADailyExchangeRate Setup - please do not remove
                        // The method is not required for changes made to the journal screens, which use a TDS
                        if (AJournalAccess.SubmitChanges((AJournalTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == AAnalysisTypeTable.GetTableDBName())
                    {
                        if (AAnalysisTypeAccess.SubmitChanges((AAnalysisTypeTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == ASuspenseAccountTable.GetTableDBName())
                    {
                        if (ASuspenseAccountAccess.SubmitChanges((ASuspenseAccountTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheableFinanceTablesEnum.SuspenseAccountList.ToString());

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == PInternationalPostalTypeTable.GetTableDBName())
                    {
                        ValidateInternationalPostalType(ValidationControlsDict, ref AVerificationResult, ASubmitTable);
                        ValidateInternationalPostalTypeManual(ValidationControlsDict, ref AVerificationResult, ASubmitTable);

                        if (!AVerificationResult.HasCriticalErrors)
                        {
                            if (PInternationalPostalTypeAccess.SubmitChanges((PInternationalPostalTypeTable)ASubmitTable, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            else
                            {
                                SubmissionResult = TSubmitChangesResult.scrError;
                            }
                        }
                    }
                    else if (ATablename == PtApplicationTypeTable.GetTableDBName())
                    {
                        if (PtApplicationTypeAccess.SubmitChanges((PtApplicationTypeTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            // mark dependent lists for needing to be refreshed since there was a change in base list
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheablePersonTablesEnum.EventApplicationTypeList.ToString());
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheablePersonTablesEnum.FieldApplicationTypeList.ToString());

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == PMailingTable.GetTableDBName())
                    {
                        if (PMailingAccess.SubmitChanges((PMailingTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == PmDocumentTypeTable.GetTableDBName())
                    {
                        if (PmDocumentTypeAccess.SubmitChanges((PmDocumentTypeTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else if (ATablename == SGroupTable.GetTableDBName())
                    {
                        if (SGroupAccess.SubmitChanges((SGroupTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else
                    {
                        throw new Exception("TCommonDataReader.SaveData: unknown table " + ATablename);
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
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
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

        static partial void ValidateInternationalPostalType(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateInternationalPostalTypeManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}