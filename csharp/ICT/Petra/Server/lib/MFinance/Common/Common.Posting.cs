//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh, christophert
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Reflection;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Verification.Exceptions;
using Ict.Common.Data;
using Ict.Common.DB;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// provides methods for posting a batch
    /// </summary>
    public class TGLPosting
    {
        /// <summary>
        ///
        /// </summary>
        [NoRemoting]
        public delegate Int32 PrintReportOnClient(String ReportName, String ParamStr);

        /// <summary>
        /// This will be setup by CallForwarding, to allow me to call the FastReportsWrapper from here.
        /// </summary>
        public static PrintReportOnClient PrintReportOnClientDelegate;

        private const int POSTING_LOGLEVEL = 1;

        /// <summary>
        /// creates the rows for the whole current year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns>The new glm sequence, which is negative until SubmitChanges</returns>
        private static Int32 CreateGLMYear(
            ref GLPostingTDS AMainDS,
            Int32 ALedgerNumber,
            string AAccountCode,
            string ACostCentreCode)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if ((AMainDS.ALedger == null) || (AMainDS.ALedger.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger table does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AAccountCode.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Account code is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACostCentreCode.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Cost Centre code is empty!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = Ledger.CurrentFinancialYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// creates the rows for the specified year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns> The GLM Sequence</returns>
        public static Int32 CreateGLMYear(
            ref GLPostingTDS AMainDS,
            Int32 ALedgerNumber,
            int AYear,
            string AAccountCode,
            string ACostCentreCode)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if ((AMainDS.ALedger == null) || (AMainDS.ALedger.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger table does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AAccountCode.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Account code is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACostCentreCode.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Cost Centre code is empty!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = AYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// load the batch and all associated tables into the typed dataset
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        public static GLBatchTDS LoadGLBatchData(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number in Ledger {1} must be greater than 0!"),
                        Utilities.GetMethodName(true), ALedgerNumber), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS GLBatchDS = new GLBatchTDS();

            AVerifications = new TVerificationResultCollection();
            TVerificationResultCollection Verifications = AVerifications;

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        GLBatchDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, ref Transaction, ref Verifications);
                    });

                AVerifications = Verifications;
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return GLBatchDS;
        }

        /// <summary>
        /// load the batch and all associated tables into the typed dataset
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static GLBatchTDS LoadGLBatchData(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            ref TDBTransaction ATransaction,
            ref TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number in Ledger {1} must be greater than 0!"),
                        Utilities.GetMethodName(true), ALedgerNumber), ALedgerNumber, ABatchNumber);
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AVerifications == null)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - Verifications collection must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLBatchTDS GLBatchDS = new GLBatchTDS();

            try
            {
                ALedgerAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, ATransaction);
                ABatchAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, ABatchNumber, ATransaction);
                AJournalAccess.LoadViaABatch(GLBatchDS, ALedgerNumber, ABatchNumber, ATransaction);
                ATransactionAccess.LoadViaABatch(GLBatchDS, ALedgerNumber, ABatchNumber, ATransaction);
                ATransAnalAttribAccess.LoadViaABatch(GLBatchDS, ALedgerNumber, ABatchNumber, ATransaction);

                #region Validate Data

                if ((GLBatchDS.ALedger == null) || (GLBatchDS.ALedger.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }
                else if ((GLBatchDS.ABatch == null) || (GLBatchDS.ABatch.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Batch data for Ledger number {1} Batch number {2} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber,
                            ABatchNumber));
                }

                #endregion Validate Data
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return GLBatchDS;
        }

        /// <summary>
        /// load the tables that are needed for posting
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        private static GLPostingTDS LoadGLDataForPosting(Int32 ALedgerNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLPostingTDS PostingDS = new GLPostingTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(PostingDS, ALedgerNumber, Transaction);

                        // load all accounts of ledger, because we need them later for the account hierarchy tree for summarisation
                        AAccountAccess.LoadViaALedger(PostingDS, ALedgerNumber, Transaction);

                        // TODO: use cached table?
                        AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(PostingDS,
                            ALedgerNumber,
                            MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD,
                            Transaction);

                        // TODO: use cached table?
                        ACostCentreAccess.LoadViaALedger(PostingDS, ALedgerNumber, Transaction);

                        AAnalysisTypeAccess.LoadAll(PostingDS, Transaction);
                        AFreeformAnalysisAccess.LoadViaALedger(PostingDS, ALedgerNumber, Transaction);
                        AAnalysisAttributeAccess.LoadViaALedger(PostingDS, ALedgerNumber, Transaction);
                    });

                #region Validate Data

                //Only the following tables should not be empty when posting.
                if ((PostingDS.ALedger == null) || (PostingDS.ALedger.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }
                else if ((PostingDS.AAccount == null) || (PostingDS.AAccount.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Account data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }
                else if ((PostingDS.ACostCentre == null) || (PostingDS.ACostCentre.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Cost Centre data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }
                else if ((PostingDS.AAccountHierarchyDetail == null) || (PostingDS.AAccountHierarchyDetail.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - AAccount Hierarchy Detail data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }

                #endregion Validate Data
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return PostingDS;
        }

        /// <summary>
        /// Load all GLM and GLMPeriod records for the batch period and the following periods, since that will avoid loading them one by one during submitchanges.
        /// this is called after ValidateBatchAndTransactions, because the BatchYear and BatchPeriod are validated and recalculated there
        /// </summary>
        private static void LoadGLMData(ref GLPostingTDS AGLPostingDS, Int32 ALedgerNumber, ABatchRow ABatchToPost)
        {
            #region Validate Arguments

            if (AGLPostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchToPost == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch to post datarow is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLPostingTDS GLPostingDS = AGLPostingDS;
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AGeneralLedgerMasterRow GLMTemplateRow = GLPostingDS.AGeneralLedgerMaster.NewRowTyped(false);

                        GLMTemplateRow.LedgerNumber = ALedgerNumber;
                        GLMTemplateRow.Year = ABatchToPost.BatchYear;
                        AGeneralLedgerMasterAccess.LoadUsingTemplate(GLPostingDS, GLMTemplateRow, Transaction);

                        string query = "SELECT PUB_a_general_ledger_master_period.* " +
                                       "FROM PUB_a_general_ledger_master, PUB_a_general_ledger_master_period " +
                                       "WHERE PUB_a_general_ledger_master.a_ledger_number_i = ? " +
                                       "AND PUB_a_general_ledger_master.a_year_i = ? " +
                                       "AND PUB_a_general_ledger_master_period.a_glm_sequence_i = PUB_a_general_ledger_master.a_glm_sequence_i " +
                                       "AND PUB_a_general_ledger_master_period.a_period_number_i >= ?";

                        List <OdbcParameter>parameters = new List <OdbcParameter>();

                        OdbcParameter parameter = new OdbcParameter("ledgernumber", OdbcType.Int);
                        parameter.Value = ALedgerNumber;
                        parameters.Add(parameter);
                        parameter = new OdbcParameter("year", OdbcType.Int);
                        parameter.Value = ABatchToPost.BatchYear;
                        parameters.Add(parameter);
                        parameter = new OdbcParameter("period", OdbcType.Int);
                        parameter.Value = ABatchToPost.BatchPeriod;
                        parameters.Add(parameter);
                        DBAccess.GDBAccessObj.Select(GLPostingDS,
                            query,
                            GLPostingDS.AGeneralLedgerMasterPeriod.TableName, Transaction, parameters.ToArray());
                    });
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// runs validations on batch, journals and transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        private static bool ValidateGLBatchAndTransactions(ref GLBatchTDS AGLBatchDS,
            GLPostingTDS APostingDS,
            Int32 ALedgerNumber,
            ABatchRow ABatchToPost,
            out TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchToPost == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch to post datarow is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool CriticalError = false;

            AVerifications = new TVerificationResultCollection();
            TVerificationResultCollection Verifications = AVerifications;

            Int32 DateEffectiveYearNumber;
            Int32 DateEffectivePeriodNumber;

            if ((ABatchToPost.BatchStatus == MFinanceConstants.BATCH_CANCELLED) || (ABatchToPost.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), ABatchToPost.BatchStatus),
                        TResultSeverity.Resv_Critical));

                return false;
            }

            // Calculate the base currency amounts for each transaction, using the exchange rate from the journals.
            // erm - this is done already? I don't want to do it here, since my journal may contain forex-reval elements.

            // Calculate the credit and debit totals
            GLRoutines.UpdateTotalsOfBatch(ref AGLBatchDS, ABatchToPost);

            if (ABatchToPost.BatchCreditTotal != ABatchToPost.BatchDebitTotal)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It does not balance: Debit is {0:N2}, Credit is {1:N2}"), ABatchToPost.BatchDebitTotal,
                            ABatchToPost.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));

                return false;
            }
            else if ((ABatchToPost.BatchCreditTotal == 0) && ((AGLBatchDS.AJournal.Rows.Count == 0) || (AGLBatchDS.ATransaction.Rows.Count == 0)))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        Catalog.GetString("The batch has no monetary value. Please cancel it or add transactions."),
                        TResultSeverity.Resv_Critical));

                return false;
            }
            else if ((ABatchToPost.BatchControlTotal != 0)
                     && (ABatchToPost.BatchControlTotal != ABatchToPost.BatchCreditTotal))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The control total {0:n2} does not fit the Credit/Debit Total {1:n2}."),
                            ABatchToPost.BatchControlTotal,
                            ABatchToPost.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));

                return false;
            }

            //Perform additional checks
            TDBTransaction Transaction = null;
            GLBatchTDS GLBatchDS = AGLBatchDS;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    if (!TFinancialYear.IsValidPostingPeriod(ABatchToPost.LedgerNumber, ABatchToPost.DateEffective, out DateEffectivePeriodNumber,
                            out DateEffectiveYearNumber,
                            Transaction))
                    {
                        Verifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                                    ABatchToPost.DateEffective),
                                TResultSeverity.Resv_Critical));

                        CriticalError = true;
                        return;
                    }
                    else
                    {
                        // just make sure that the correct BatchPeriod is used
                        ABatchToPost.BatchPeriod = DateEffectivePeriodNumber;
                        ABatchToPost.BatchYear = DateEffectiveYearNumber;
                    }

                    // check that all transactions are inside the same period as the GL date effective of the batch
                    DateTime PostingPeriodStartDate, PostingPeriodEndDate;
                    TFinancialYear.GetStartAndEndDateOfPeriod(ABatchToPost.LedgerNumber,
                        DateEffectivePeriodNumber,
                        out PostingPeriodStartDate,
                        out PostingPeriodEndDate,
                        Transaction);

                    foreach (ATransactionRow transRow in GLBatchDS.ATransaction.Rows)
                    {
                        if ((transRow.BatchNumber == ABatchToPost.BatchNumber)
                            && (transRow.TransactionDate < PostingPeriodStartDate) || (transRow.TransactionDate > PostingPeriodEndDate))
                        {
                            Verifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                    String.Format(
                                        "invalid transaction date for transaction {0} in Batch {1} Journal {2}: {3:d-MMM-yyyy} must be inside period {4} ({5:d-MMM-yyyy} till {6:d-MMM-yyyy})",
                                        transRow.TransactionNumber, transRow.BatchNumber, transRow.JournalNumber,
                                        transRow.TransactionDate,
                                        DateEffectivePeriodNumber,
                                        PostingPeriodStartDate,
                                        PostingPeriodEndDate),
                                    TResultSeverity.Resv_Critical));

                            CriticalError = true;
                            break;
                        }
                    }
                });

            AVerifications = Verifications;

            if (CriticalError)
            {
                return false;
            }

            //More checks on transactions in journals
            DataView TransactionsOfJournalView = new DataView(AGLBatchDS.ATransaction);

            foreach (AJournalRow journal in AGLBatchDS.AJournal.Rows)
            {
                journal.DateEffective = ABatchToPost.DateEffective;
                journal.JournalPeriod = ABatchToPost.BatchPeriod;

                if (journal.JournalCreditTotal != journal.JournalDebitTotal)
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The journal {0} does not balance: Debit is {1:N2}, Credit is {2:N2}"),
                                journal.JournalNumber,
                                journal.JournalDebitTotal, journal.JournalCreditTotal),
                            TResultSeverity.Resv_Critical));

                    CriticalError = true;
                    break;
                }

                TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                foreach (DataRowView TransactionViewRow in TransactionsOfJournalView)
                {
                    ATransactionRow transaction = (ATransactionRow)TransactionViewRow.Row;

                    // check that transactions on foreign currency accounts are using the correct currency
                    // (fx reval transactions are an exception because they are posted in base currency)
                    if (journal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                    {
                        // get the account that this transaction is writing to
                        AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, transaction.AccountCode });

                        #region Validate Data

                        if (Account == null)
                        {
                            // should not get here
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(
                                    "Function:{0} - Cannot find Account {1} data for Transaction:{2} of Batch:{3}, Journal:{4}!",
                                    Utilities.GetMethodName(true),
                                    transaction.AccountCode,
                                    transaction.TransactionNumber,
                                    transaction.BatchNumber,
                                    transaction.JournalNumber));
                        }

                        #endregion Validate Data

                        if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency != Account.ForeignCurrencyCode))
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Transaction {0} in Journal {1} with currency {2} does not fit the foreign currency {3} of account {4}."),
                                        transaction.TransactionNumber, transaction.JournalNumber, journal.TransactionCurrency,
                                        Account.ForeignCurrencyCode,
                                        transaction.AccountCode),
                                    TResultSeverity.Resv_Critical));

                            CriticalError = true;
                            break;
                        }
                    }

                    if ((transaction.AmountInBaseCurrency == 0) && (transaction.TransactionAmount != 0))
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("Transaction {0} in Journal {1} has invalid base transaction amount of 0."),
                                    transaction.TransactionNumber, transaction.JournalNumber),
                                TResultSeverity.Resv_Critical));

                        CriticalError = true;
                        break;
                    }
                }

                if (CriticalError)
                {
                    break;
                }
            }

            return TVerificationHelper.IsNullOrOnlyNonCritical(AVerifications);
        }

        /// <summary>
        /// validate the attributes of the transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        private static bool ValidateAnalysisAttributes(ref GLBatchTDS AGLBatchDS,
            GLPostingTDS APostingDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number in Ledger {1} must be greater than 0!"),
                        Utilities.GetMethodName(true), ALedgerNumber), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            bool CriticalError = false;

            AVerifications = new TVerificationResultCollection();

            DataView TransactionsOfJournalView = new DataView(AGLBatchDS.ATransaction);

            foreach (AJournalRow journal in AGLBatchDS.AJournal.Rows)
            {
                if (journal.BatchNumber != ABatchNumber)
                {
                    continue;
                }

                TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                foreach (DataRowView transRowView in TransactionsOfJournalView)
                {
                    ATransactionRow transRow = (ATransactionRow)transRowView.Row;

                    // Check that all atransanalattrib records are there for all analattributes entries
                    DataView ANView = APostingDS.AAnalysisAttribute.DefaultView;
                    ANView.RowFilter = String.Format("{0} = '{1}' AND {2} = true",
                        AAnalysisAttributeTable.GetAccountCodeDBName(),
                        transRow.AccountCode, AAnalysisAttributeTable.GetActiveDBName());
                    int i = 0;

                    while (i < ANView.Count)
                    {
                        AAnalysisAttributeRow attributeRow = (AAnalysisAttributeRow)ANView[i].Row;

                        ATransAnalAttribRow aTransAttribRow =
                            (ATransAnalAttribRow)AGLBatchDS.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber,
                                                                                                      transRow.JournalNumber,
                                                                                                      transRow.TransactionNumber,
                                                                                                      attributeRow.AnalysisTypeCode });

                        if (aTransAttribRow == null)
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Missing attributes record for journal #{0} transaction #{1}  and TypeCode {2}"),
                                        transRow.JournalNumber,
                                        transRow.TransactionNumber, attributeRow.AnalysisTypeCode),
                                    TResultSeverity.Resv_Critical));

                            CriticalError = true;
                            break;
                        }
                        else
                        {
                            String v = aTransAttribRow.AnalysisAttributeValue;

                            if ((v == null) || (v.Length == 0))
                            {
                                AVerifications.Add(new TVerificationResult(
                                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                        String.Format(Catalog.GetString("Analysis Type {0} is missing values in journal #{1}, transaction #{2}"),
                                            attributeRow.AnalysisTypeCode, transRow.JournalNumber, transRow.TransactionNumber),
                                        TResultSeverity.Resv_Critical));

                                CriticalError = true;
                                break;
                            }
                            else
                            {
                                AFreeformAnalysisRow afaRow = (AFreeformAnalysisRow)APostingDS.AFreeformAnalysis.Rows.Find(
                                    new Object[] { ALedgerNumber, attributeRow.AnalysisTypeCode, v });

                                if (afaRow == null)
                                {
                                    // this would cause a constraint error and is only possible in a development/sqlite environment
                                    AVerifications.Add(new TVerificationResult(
                                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                            String.Format(Catalog.GetString("Invalid values at journal #{0} transaction #{1}  and TypeCode {2}"),
                                                transRow.JournalNumber, transRow.TransactionNumber, attributeRow.AnalysisTypeCode),
                                            TResultSeverity.Resv_Critical));

                                    CriticalError = true;
                                    break;
                                }
                                else
                                {
                                    if (!afaRow.Active)
                                    {
                                        AVerifications.Add(new TVerificationResult(
                                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber,
                                                    ALedgerNumber),
                                                String.Format(Catalog.GetString(
                                                        "Value {0} not active at journal #{1} transaction #{2}  and TypeCode {3}"), v,
                                                    transRow.JournalNumber, transRow.TransactionNumber, attributeRow.AnalysisTypeCode),
                                                TResultSeverity.Resv_Critical));

                                        CriticalError = true;
                                        break;
                                    } // if
                                } // else
                            } // else
                        } // else

                        i++;
                    } // while i

                    if (CriticalError)
                    {
                        break;
                    }
                } // foreach transRowView

                if (CriticalError)
                {
                    break;
                }
            } // foreach journal

            return TVerificationHelper.IsNullOrOnlyNonCritical(AVerifications);
        }

        /// Helper class for storing the amounts of a batch at posting level for account/costcentre combinations
        private class TAmount
        {
            /// amount in the base currency of the ledger
            public decimal baseAmount = 0.0M;

            /// amount in transaction currency; only for foreign currency accounts
            public decimal transAmount = 0.0M;

            /// generate a key for the account/costcentre combination
            public static string MakeKey(string AccountCode, string CostCentreCode)
            {
                return AccountCode + ":" + CostCentreCode;
            }

            /// get the account code from the key
            public static string GetAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the cost centre code from the key
            public static string GetCostCentreCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// Helper class for managing the account hierarchy for posting the batch
        private class TAccountTreeElement
        {
            /// Constructor
            public TAccountTreeElement(bool AInvert, bool AForeign)
            {
                Invert = AInvert;
                Foreign = AForeign;
            }

            /// is the debit credit indicator different of the reporting account to the parent account
            public bool Invert = false;

            /// is this account a foreign currency account
            public bool Foreign = false;

            /// generate a key for the reporting account/parent account combination
            public static string MakeKey(string ReportingAccountCode, string AccountCodeReportTo)
            {
                return ReportingAccountCode + ":" + AccountCodeReportTo;
            }

            /// get the reporting account code from the key
            public static string GetReportingAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the parent account code from the key
            public static string GetAccountReportToCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// <summary>
        /// mark each journal, each transaction as being posted;
        /// add sums for costcentre/account combinations
        /// </summary>
        /// <param name="MainDS">can contain several batches and journals and transactions</param>
        /// <param name="APostingDS"></param>
        /// <param name="APostingLevel">the balance changes at the posting level</param>
        /// <param name="ABatchToPost">the batch to post</param>
        /// <returns>a list with the sums for each costcentre/account combination</returns>
        private static SortedList <string, TAmount>MarkAsPostedAndCollectData(GLBatchTDS MainDS,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel, ABatchRow ABatchToPost)
        {
            DataView myView = new DataView(MainDS.ATransaction);

            myView.Sort = ATransactionTable.GetJournalNumberDBName();

            foreach (AJournalRow journal in MainDS.AJournal.Rows)
            {
                if (journal.BatchNumber != ABatchToPost.BatchNumber)
                {
                    continue;
                }

                foreach (DataRowView transactionview in myView.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;

                    if (transaction.BatchNumber != ABatchToPost.BatchNumber)
                    {
                        continue;
                    }

                    transaction.TransactionStatus = true;

                    // get the account that this transaction is writing to
                    AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { transaction.LedgerNumber, transaction.AccountCode });

                    // Set the sign of the amounts according to the debit/credit indicator
                    decimal SignBaseAmount = transaction.AmountInBaseCurrency;
                    decimal SignTransAmount = transaction.TransactionAmount;

                    if (Account.DebitCreditIndicator != transaction.DebitCreditIndicator)
                    {
                        SignBaseAmount *= -1.0M;
                        SignTransAmount *= -1.0M;
                    }

                    // TODO: do we need to check for base currency corrections?
                    // or do we get rid of these problems by not having international currency?

                    string key = TAmount.MakeKey(transaction.AccountCode, transaction.CostCentreCode);

                    if (!APostingLevel.ContainsKey(key))
                    {
                        APostingLevel.Add(key, new TAmount());
                    }

                    APostingLevel[key].baseAmount += SignBaseAmount;

                    // Only foreign currency accounts store a value in the transaction currency,
                    // if the transaction was actually in the foreign currency.

                    if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency == Account.ForeignCurrencyCode))
                    {
                        APostingLevel[key].transAmount += SignTransAmount;
                    }
                }

                journal.JournalStatus = MFinanceConstants.BATCH_POSTED;
            }

            ABatchToPost.BatchStatus = MFinanceConstants.BATCH_POSTED;

            return APostingLevel;
        }

        /// <summary>
        /// Calculate the summarization trees for each posting account and each
        /// posting cost centre. The result of the union of these trees,
        /// excluding the base posting/posting combination, is the set of
        /// accounts that receive the summary data.
        /// </summary>
        private static bool CalculateTrees(
            Int32 ALedgerNumber,
            ref SortedList <string, TAmount>APostingLevel,
            out SortedList <string, TAccountTreeElement>AAccountTree,
            out SortedList <string, string>ACostCentreTree,
            GLPostingTDS APostingDS)
        {
            // get all accounts that each posting level account is directly or indirectly posting to
            AAccountTree = new SortedList <string, TAccountTreeElement>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);

                // only once for each account, even though there might be several entries for one account in APostingLevel because of different costcentres
                if (AAccountTree.ContainsKey(TAccountTreeElement.MakeKey(AccountCode, AccountCode)))
                {
                    continue;
                }

                AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });
                bool DebitCreditIndicator = Account.DebitCreditIndicator;
                AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, AccountCode),
                    new TAccountTreeElement(false, Account.ForeignCurrencyFlag));

                AAccountHierarchyDetailRow HierarchyDetail =
                    (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, AccountCode });

                while (HierarchyDetail != null)
                {
                    Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, HierarchyDetail.AccountCodeToReportTo });

                    if (Account == null)
                    {
                        // current account is BAL SHT, and it reports nowhere (account with name = ledgernumber does not exist)
                        break;
                    }

                    AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, HierarchyDetail.AccountCodeToReportTo),
                        new TAccountTreeElement(DebitCreditIndicator != Account.DebitCreditIndicator, Account.ForeignCurrencyFlag));

                    HierarchyDetail = (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, HierarchyDetail.AccountCodeToReportTo });
                }
            }

            ACostCentreTree = new SortedList <string, string>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                // only once for each cost centre
                if (ACostCentreTree.ContainsKey(CostCentreCode + ":" + CostCentreCode))
                {
                    continue;
                }

                ACostCentreTree.Add(CostCentreCode + ":" + CostCentreCode,
                    CostCentreCode + ":" + CostCentreCode);

                ACostCentreRow CostCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentreCode });

                while (!CostCentre.IsCostCentreToReportToNull())
                {
                    ACostCentreTree.Add(CostCentreCode + ":" + CostCentre.CostCentreToReportTo,
                        CostCentreCode + ":" + CostCentre.CostCentreToReportTo);

                    CostCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentre.CostCentreToReportTo });
                }
            }

            return true;
        }

        /// <summary>
        /// for each posting level, propagate the value upwards through both the account and the cost centre hierarchy in glm master;
        /// also propagate the value from the posting period through the following periods;
        /// </summary>
        private static bool SummarizeData(
            GLPostingTDS APostingDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel,
            ref SortedList <string, TAccountTreeElement>AAccountTree,
            ref SortedList <string, string>ACostCentreTree)
        {
            if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                AFromPeriod++;
            }

            DataView GLMMasterView = APostingDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = APostingDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Combine the summarization trees for both the account and the cost centre.
                foreach (string AccountTreeKey in AAccountTree.Keys)
                {
                    if (TAccountTreeElement.GetReportingAccountCode(AccountTreeKey) == AccountCode)
                    {
                        string AccountCodeToReportTo = TAccountTreeElement.GetAccountReportToCode(AccountTreeKey);
                        TAccountTreeElement AccountTreeElement = AAccountTree[AccountTreeKey];

                        foreach (string CostCentreKey in ACostCentreTree.Keys)
                        {
                            if (CostCentreKey.StartsWith(CostCentreCode + ":"))
                            {
                                string CostCentreCodeToReportTo = CostCentreKey.Split(':')[1];
                                decimal SignBaseAmount = PostingLevelElement.baseAmount;
                                decimal SignTransAmount = PostingLevelElement.transAmount;

                                // Set the sign of the amounts according to the debit/credit indicator
                                if (AccountTreeElement.Invert)
                                {
                                    SignBaseAmount *= -1;
                                    SignTransAmount *= -1;
                                }

                                // Find the summary level, creating it if it does not already exist.
                                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });

                                if (GLMMasterIndex == -1)
                                {
                                    CreateGLMYear(
                                        ref APostingDS,
                                        APostingDS.ALedger[0].LedgerNumber,
                                        AccountCodeToReportTo,
                                        CostCentreCodeToReportTo);

                                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });
                                }

                                AGeneralLedgerMasterRow GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                                GlmRow.YtdActualBase += SignBaseAmount;

                                if (AccountTreeElement.Foreign)
                                {
                                    if (GlmRow.IsYtdActualForeignNull())
                                    {
                                        GlmRow.YtdActualForeign = SignTransAmount;
                                    }
                                    else
                                    {
                                        GlmRow.YtdActualForeign += SignTransAmount;
                                    }
                                }

                                if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    GlmRow.ClosingPeriodActualBase += SignBaseAmount;
                                }

                                // Add the period data from the posting level to the summary levels
                                for (Int32 PeriodCount = AFromPeriod;
                                     PeriodCount <= APostingDS.ALedger[0].NumberOfAccountingPeriods + APostingDS.ALedger[0].NumberFwdPostingPeriods;
                                     PeriodCount++)
                                {
                                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                                    if (GLMPeriodIndex == -1)
                                    {
                                        GlmPeriodRow = APostingDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                                        GlmPeriodRow.PeriodNumber = PeriodCount;
                                        APostingDS.AGeneralLedgerMasterPeriod.Rows.Add(GlmPeriodRow);
                                    }
                                    else
                                    {
                                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                                    }

                                    GlmPeriodRow.ActualBase += SignBaseAmount;

                                    if (AccountTreeElement.Foreign)
                                    {
                                        if (GlmPeriodRow.IsActualForeignNull())
                                        {
                                            GlmPeriodRow.ActualForeign = SignTransAmount;
                                        }
                                        else
                                        {
                                            GlmPeriodRow.ActualForeign += SignTransAmount;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// on the posting level propagate the value from the posting period through the following periods;
        /// in this version of SummarizeData, there is no calculation of summary accounts/cost centres, since that can be done by the reports
        /// </summary>
        private static bool SummarizeDataSimple(
            Int32 ALedgerNumber,
            GLPostingTDS AMainDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel)
        {
            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                AFromPeriod++;
            }

            DataView GLMMasterView = AMainDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Find the posting level, creating it if it does not already exist.
                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                AGeneralLedgerMasterRow GlmRow;

                if (GLMMasterIndex == -1)
                {
                    CreateGLMYear(
                        ref AMainDS,
                        ALedgerNumber,
                        AccountCode,
                        CostCentreCode);

                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                }

                GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                GlmRow.YtdActualBase += PostingLevelElement.baseAmount;

                AAccountRow account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

                if (account.ForeignCurrencyFlag)
                {
                    if (GlmRow.IsYtdActualForeignNull())
                    {
                        GlmRow.YtdActualForeign = PostingLevelElement.transAmount;
                    }
                    else
                    {
                        GlmRow.YtdActualForeign += PostingLevelElement.transAmount;
                    }
                }

                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                {
                    GlmRow.ClosingPeriodActualBase += PostingLevelElement.baseAmount;
                } // Last use of GlmRow in this routine ...

                // propagate the data through the following periods
                for (Int32 PeriodCount = AFromPeriod;
                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                     PeriodCount++)
                {
                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                    if (GLMPeriodIndex == -1)
                    {
                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                        GlmPeriodRow.PeriodNumber = PeriodCount;
                    }
                    else
                    {
                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                    }

                    GlmPeriodRow.ActualBase += PostingLevelElement.baseAmount;

                    if (account.ForeignCurrencyFlag)
                    {
                        if (GlmPeriodRow.IsActualForeignNull())
                        {
                            GlmPeriodRow.ActualForeign = PostingLevelElement.transAmount;
                        }
                        else
                        {
                            GlmPeriodRow.ActualForeign += PostingLevelElement.transAmount;
                        }
                    }
                }
            }

            GLMMasterView.Sort = "";
            GLMPeriodView.Sort = "";

            return true;
        }

        private static void SummarizeInternal(Int32 ALedgerNumber,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel,
            Int32 AFromPeriod,
            bool ACalculatePostingTree)
        {
            // we need the tree, because of the cost centre tree, which is not calculated by the balance sheet and other reports.
            // for testing the balances, we don't need to calculate the whole tree
            if (ACalculatePostingTree)
            {
                TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: CalculateTrees...");

                // key is PostingAccount, the value TAccountTreeElement describes the parent account and other details of the relation
                SortedList <string, TAccountTreeElement>AccountTree;

                // key is the PostingCostCentre, the value is the parent Cost Centre
                SortedList <string, string>CostCentreTree;

                // TODO Can anything of this be done in StoredProcedures? Only SQLite here?

                // this was in Petra 2.x; takes a lot of time, which the reports could do better
                // TODO: can we just calculate the cost centre tree, since that is needed for Balance Sheet,
                // but avoid calculating the whole account tree?
                CalculateTrees(ALedgerNumber, ref APostingLevel, out AccountTree, out CostCentreTree, APostingDS);

                TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: SummarizeData...");
                SummarizeData(APostingDS, AFromPeriod, ref APostingLevel, ref AccountTree, ref CostCentreTree);
            }
            else
            {
                TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: SummarizeDataSimple...");
                SummarizeDataSimple(ALedgerNumber, APostingDS, AFromPeriod, ref APostingLevel);
            }
        }

        /// <summary>
        /// Write all changes to the database; on failure the whole transaction will be rolled back
        /// </summary>
        /// <param name="AMainDS"></param>

        private static void SubmitChanges(GLPostingTDS AMainDS)
        {
            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: SubmitChanges...");
            GLPostingTDSAccess.SubmitChanges(AMainDS.GetChangesTyped(true));
            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Finished...");
        }

        /// <summary>
        /// reverse gl batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumberToReverse"></param>
        /// <param name="ADateForReversal"></param>
        /// <param name="AReversalBatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <param name="AAutoPostReverseBatch"></param>
        /// <returns></returns>
        public static bool ReverseBatch(Int32 ALedgerNumber, Int32 ABatchNumberToReverse,
            DateTime ADateForReversal,
            out Int32 AReversalBatchNumber,
            out TVerificationResultCollection AVerifications,
            bool AAutoPostReverseBatch)
        {
            bool ReturnValue = false;

            GLBatchTDS MainDS = null;

            Int32 ReversalBatchNumber = -1;

            //Error handling
            string ErrorContext = "Reverse a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            AVerifications = null;
            TVerificationResultCollection Verifications = null;

            TDBTransaction Transaction = null;
            bool SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum,
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    MainDS = LoadGLBatchData(ALedgerNumber, ABatchNumberToReverse, ref Transaction, ref Verifications);

                    // get the data from the database into the MainDS
                    if (MainDS != null)
                    {
                        ABatchRow NewBatchRow = MainDS.ABatch.NewRowTyped(true);
                        NewBatchRow.LedgerNumber = ALedgerNumber;
                        MainDS.ALedger[0].LastBatchNumber++;
                        NewBatchRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;

                        int DateEffectiveYearNumber;
                        int DateEffectivePeriodNumber;

                        if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, ADateForReversal, out DateEffectivePeriodNumber,
                                out DateEffectiveYearNumber,
                                Transaction))
                        {
                            ErrorMessage = Catalog.GetString("Date is outside of valid posting period");
                            ErrorType = TResultSeverity.Resv_Critical;
                            Verifications.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                        }
                        else
                        {
                            NewBatchRow.DateEffective = ADateForReversal;
                            NewBatchRow.BatchPeriod = DateEffectivePeriodNumber;
                            NewBatchRow.BatchYear = DateEffectiveYearNumber;

                            ABatchRow OriginalBatch = (ABatchRow)MainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumberToReverse });
                            NewBatchRow.BatchDescription = String.Format(Catalog.GetString("Reversal of {0}"), OriginalBatch.BatchDescription);
                            NewBatchRow.LastJournal = OriginalBatch.LastJournal;
                            MainDS.ABatch.Rows.Add(NewBatchRow);

                            MainDS.AJournal.DefaultView.Sort = AJournalTable.GetLedgerNumberDBName() + "," + AJournalTable.GetBatchNumberDBName();
                            DataRowView[] JournalsRowView = MainDS.AJournal.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse });

                            foreach (DataRowView rv in JournalsRowView)
                            {
                                AJournalRow OriginalJournal = (AJournalRow)rv.Row;
                                AJournalRow NewJournalRow = MainDS.AJournal.NewRowTyped();

                                DataUtilities.CopyAllColumnValues(OriginalJournal, NewJournalRow);

                                NewJournalRow.BatchNumber = NewBatchRow.BatchNumber;
                                NewJournalRow.DateEffective = NewBatchRow.DateEffective;
                                NewJournalRow.JournalPeriod = NewBatchRow.BatchPeriod;
                                NewJournalRow.JournalStatus = NewBatchRow.BatchStatus;
                                NewJournalRow.JournalDescription =
                                    String.Format(Catalog.GetString("Reversal of {0}"), OriginalJournal.JournalDescription);
                                OriginalJournal.Reversed = true;
                                MainDS.AJournal.Rows.Add(NewJournalRow);

                                MainDS.ATransaction.DefaultView.Sort = ATransactionTable.GetLedgerNumberDBName() + "," +
                                                                       ATransactionTable.GetBatchNumberDBName() + "," +
                                                                       ATransactionTable.GetJournalNumberDBName();
                                DataRowView[] TransactionsRowView =
                                    MainDS.ATransaction.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse,
                                                                                            OriginalJournal.
                                                                                            JournalNumber });

                                foreach (DataRowView rvTransaction in TransactionsRowView)
                                {
                                    ATransactionRow OriginalTransaction = (ATransactionRow)rvTransaction.Row;
                                    ATransactionRow NewTransactionRow = MainDS.ATransaction.NewRowTyped();

                                    DataUtilities.CopyAllColumnValues(OriginalTransaction, NewTransactionRow);
                                    NewTransactionRow.BatchNumber = NewJournalRow.BatchNumber;
                                    NewTransactionRow.JournalNumber = NewJournalRow.JournalNumber;
                                    NewTransactionRow.TransactionStatus = false;
                                    NewTransactionRow.DebitCreditIndicator = !OriginalTransaction.DebitCreditIndicator;
                                    NewTransactionRow.SystemGenerated = true;
                                    NewTransactionRow.TransactionDate = ADateForReversal;
                                    NewTransactionRow.Narrative = Catalog.GetString("Reverse of: ") + OriginalTransaction.Narrative +
                                                                  "(" + Catalog.GetString(" Batch: ") + OriginalTransaction.BatchNumber +
                                                                  Catalog.GetString(", Journal: ") + OriginalTransaction.JournalNumber +
                                                                  Catalog.GetString(", Transaction: ") + OriginalTransaction.TransactionNumber + ")";

                                    MainDS.ATransaction.Rows.Add(NewTransactionRow);

                                    MainDS.ATransAnalAttrib.DefaultView.Sort = ATransAnalAttribTable.GetLedgerNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetBatchNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetJournalNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetTransactionNumberDBName();
                                    DataRowView[] TransAnalAttribRowView =
                                        MainDS.ATransAnalAttrib.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse,
                                                                                                    OriginalJournal.JournalNumber,
                                                                                                    OriginalTransaction.TransactionNumber });

                                    foreach (DataRowView rvTransAnalAttrib in TransAnalAttribRowView)
                                    {
                                        ATransAnalAttribRow OriginalTransAnalAttrib = (ATransAnalAttribRow)rvTransAnalAttrib.Row;
                                        ATransAnalAttribRow NewTransAnalAttribRow = MainDS.ATransAnalAttrib.NewRowTyped();
                                        DataUtilities.CopyAllColumnValues(OriginalTransAnalAttrib, NewTransAnalAttribRow);
                                        NewTransAnalAttribRow.BatchNumber = NewTransactionRow.BatchNumber;
                                        NewTransAnalAttribRow.JournalNumber = NewTransactionRow.JournalNumber;
                                        NewTransAnalAttribRow.TransactionNumber = NewTransactionRow.TransactionNumber;
                                        MainDS.ATransAnalAttrib.Rows.Add(NewTransAnalAttribRow);
                                    }
                                }
                            }

                            // Calculate the credit and debit totals
                            GLRoutines.UpdateTotalsOfBatch(ref MainDS, NewBatchRow);

                            GLBatchTDSAccess.SubmitChanges(MainDS);

                            ReversalBatchNumber = NewBatchRow.BatchNumber;

                            // only post new batch is AAutoPostReverseBatch is true
                            if (!AAutoPostReverseBatch || PostGLBatch(ALedgerNumber, ReversalBatchNumber, out Verifications))
                            {
                                ReturnValue = true;
                            }
                        }
                    }
                    else
                    {
                        SubmissionOK = false;

                        Verifications.Add(
                            new TVerificationResult(
                                ErrorContext,
                                String.Format("Unable to Load GLBatchData ({0}, {1})",
                                    ALedgerNumber,
                                    ABatchNumberToReverse),
                                TResultSeverity.Resv_Critical));
                    }
                });

            AVerifications = Verifications;
            AReversalBatchNumber = ReversalBatchNumber;

            return ReturnValue;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            List <Int32>BatchNumbers = new List <int>();
            BatchNumbers.Add(ABatchNumber);

            return PostGLBatches(ALedgerNumber, BatchNumbers, out AVerifications);
        }

        /// <summary>
        /// post several GL Batches at once
        /// Returns true if it seems to be OK.
        /// </summary>
        public static bool PostGLBatches(Int32 ALedgerNumber, List <Int32>ABatchNumbers, out TVerificationResultCollection AVerifications)
        {
            // TODO: get a lock on this ledger, no one else is allowed to change anything.
            AVerifications = new TVerificationResultCollection();
            //For use in transaction delegate
            TVerificationResultCollection VerificationResult = AVerifications;
            TVerificationResultCollection SingleVerificationResultCollection;

            //Error handling
            string ErrorContext = "Posting a GL Batch";
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            //TODO consider removing below
            //if (!CheckPostIsAllowed(ALedgerNumber, out AVerifications))
            //{
            //    return false;
            //}

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

                        Int32 BatchPeriod = -1;

                        foreach (Int32 BatchNumber in ABatchNumbers)
                        {
                            GLBatchTDS MainDS = null;
                            GLPostingTDS PostingDS = PrepareGLBatchForPosting(out MainDS,
                                ALedgerNumber,
                                BatchNumber,
                                ref Transaction,
                                out SingleVerificationResultCollection,
                                PostingLevel,
                                ref BatchPeriod);

                            VerificationResult.AddCollection(SingleVerificationResultCollection);

                            if ((MainDS == null) || (PostingDS == null))
                            {
                                return;
                            }

                            MainDS.ThrowAwayAfterSubmitChanges = true;
                            GLBatchTDSAccess.SubmitChanges(MainDS);

                            SummarizeInternal(ALedgerNumber, PostingDS, PostingLevel, BatchPeriod, true);

                            PostingDS.ThrowAwayAfterSubmitChanges = true;
                            SubmitChanges(PostingDS);
                        }  // foreach

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage =
                            String.Format(Catalog.GetString("Unknown error while posting GL batch." +
                                    Environment.NewLine + Environment.NewLine + ex.ToString()),
                                ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Critical;

                        VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                        throw new EVerificationResultsException(ErrorMessage, VerificationResult, ex.InnerException);
                    }
                });

            AVerifications = VerificationResult;

            if (SubmissionOK == true)
            {
                String LedgerName = TLedgerInfo.GetLedgerName(ALedgerNumber);

                // Generate posting reports (on the client!)
                foreach (Int32 BatchNumber in ABatchNumbers)
                {
                    String[] Params =
                    {
                        "param_ledger_number_i=" + ALedgerNumber,
                        "param_batch_number_i=" + BatchNumber,
                        "param_ledger_name=\"" + LedgerName + "\""
                    };
                    String ParamStr = String.Join(",", Params);
                    PrintReportOnClientDelegate("Batch Posting Register", ParamStr);
                }
            }

            return SubmissionOK;
        }

        /// <summary>
        /// Only used for precalculating the new balances before the user actually posts the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AVerifications"></param>
        /// <param name="APostingDS"></param>
        /// <param name="ABatchPeriod"></param>
        /// <returns></returns>
        public static bool TestPostGLBatch(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            TDBTransaction ATransaction,
            out TVerificationResultCollection AVerifications,
            out GLPostingTDS APostingDS,
            ref Int32 ABatchPeriod)
        {
            GLBatchTDS MainDS = new GLBatchTDS();

            SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

            APostingDS = PrepareGLBatchForPosting(out MainDS,
                ALedgerNumber,
                ABatchNumber,
                ref ATransaction,
                out AVerifications,
                PostingLevel,
                ref ABatchPeriod);

            if ((MainDS != null) && (APostingDS != null))
            {
                SummarizeInternal(ALedgerNumber, APostingDS, PostingLevel, ABatchPeriod, false);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prepare posting a GL Batch, without saving to database yet.
        /// This is called by the actual PostGLBatch routine, and also by the routine for testing what would happen to the balances.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AVerifications"></param>
        /// <param name="APostingLevel"></param>
        /// <param name="ABatchPeriod"></param>
        /// <returns></returns>
        private static GLPostingTDS PrepareGLBatchForPosting(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            ref TDBTransaction ATransaction,
            out TVerificationResultCollection AVerifications,
            SortedList <string, TAmount>APostingLevel,
            ref Int32 ABatchPeriod)
        {
            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: LoadData...");
            }

            AVerifications = new TVerificationResultCollection();

            GLPostingTDS PostingDS = LoadGLDataForPosting(ALedgerNumber);
            // get the data from the database into the MainDS
            AMainDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, ref ATransaction, ref AVerifications);

            //TODO: do A NULL CHECK and use verfication results and stop in tracks.

            if ((AMainDS.ABatch == null) || (AMainDS.ABatch.Rows.Count < 1))
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting GL Batch",
                        String.Format("Unable to Load GLBatchData ({0}, {1})",
                            ALedgerNumber,
                            ABatchNumber),
                        TResultSeverity.Resv_Critical));
                return null;

                //TODO return the everificationsresult collection.
            }
            else if (AMainDS.ABatch[0].BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting GL Batch",
                        String.Format("Cannot post batch ({0}, {1}) with status: {2}",
                            ALedgerNumber,
                            ABatchNumber,
                            AMainDS.ABatch[0].BatchStatus),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Validation...");

            ABatchRow BatchToPost =
                ((ABatchRow)AMainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber }));

            if (ABatchPeriod == -1)
            {
                ABatchPeriod = BatchToPost.BatchPeriod;
            }
            else if (ABatchPeriod != BatchToPost.BatchPeriod)
            {
                AVerifications.Add(new TVerificationResult(
                        Catalog.GetString("Cannot post Batches from different periods at once!"),
                        Catalog.GetString("Batches from more than one period."),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            if (!ValidateGLBatchAndTransactions(ref AMainDS, PostingDS, ALedgerNumber, BatchToPost, out AVerifications))
            {
                return null;
            }

            if (!PostingDS.ALedger[0].ProvisionalYearEndFlag) // During YearEnd Processing, I don't require all the attributes correctly fulfiled.
            {
                if (!ValidateAnalysisAttributes(ref AMainDS, PostingDS, ALedgerNumber, ABatchNumber, out AVerifications))
                {
                    return null;
                }
            }

            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Load GLM Data...");

            // TODO
            LoadGLMData(ref PostingDS, ALedgerNumber, BatchToPost);

            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Mark as posted and collect data...");

            // post each journal, each transaction; add sums for costcentre/account combinations
            MarkAsPostedAndCollectData(AMainDS, PostingDS, APostingLevel, BatchToPost);

            // if posting goes wrong later, the transation will be rolled back
            return PostingDS;
        }

        /// <summary>
        /// Tell me whether this Batch can be cancelled
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool GLBatchCanBeCancelled(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            bool RetVal = false;
            string BatchStatus = string.Empty;

            string ErrorMessage = string.Empty;
            string ErrorContext = "Check if a Batch can be cancelled";
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            AMainDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, out AVerifications);

            // get the data from the database into the MainDS
            if (AMainDS == null)
            {
                RetVal = false;
            }
            else
            {
                try
                {
                    ABatchRow Batch = AMainDS.ABatch[0];

                    BatchStatus = Batch.BatchStatus;

                    if (BatchStatus == MFinanceConstants.BATCH_CANCELLED)
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("It was already cancelled.")),
                                TResultSeverity.Resv_Critical));
                        RetVal = false;
                    }
                    else if (BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                                TResultSeverity.Resv_Critical));
                        RetVal = false;
                    }
                    else
                    {
                        //Only if reaches here it can be deleted
                        RetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                    ErrorType = TResultSeverity.Resv_Critical;
                    AVerifications.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    throw new EVerificationResultsException("Error GLBatchCanBeCancelled", AVerifications, ex.InnerException);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// If a Batch has been created then found to be not required, it can be deleted here.
        /// (This was added for ICH and Stewardship calculations, which can otherwise leave empty batches in the ledger.)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        public static bool DeleteGLBatch(
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            GLBatchTDS TempTDS;

            if (!GLBatchCanBeCancelled(out TempTDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }
            else
            {
                ABatchRow BatchRow = TempTDS.ABatch[0];

                //
                // If I'm deleting the most recent entry (which is almost certainly the case)
                // I can wind back the Ledger's LastBatchNumber so as not to leave a gap.
                //
                if (BatchRow.BatchNumber == TempTDS.ALedger[0].LastBatchNumber)
                {
                    TempTDS.ALedger[0].LastBatchNumber--;
                }

                BatchRow.Delete();
                //
                // If this batch has journals and transactions, they need to be deleted too,
                // along with any trans_anal_attrib records.
                //
                // The call to GLBatchCanBeCancelled will have loaded all these records for me.

                foreach (DataRow row in TempTDS.AJournal.Rows)
                {
                    row.Delete();
                }

                foreach (DataRow row in TempTDS.ATransaction.Rows)
                {
                    row.Delete();
                }

                foreach (DataRow row in TempTDS.ATransAnalAttrib.Rows)
                {
                    row.Delete();
                }

/*
 * // apparently using Rows.Clear doesn't convey "delete these rows" back to the database, which is what we need:
 *
 *              TempTDS.AJournal.Rows.Clear();
 *              TempTDS.ATransaction.Rows.Clear();
 *              TempTDS.ATransAnalAttrib.Rows.Clear();
 */
                GLBatchTDSAccess.SubmitChanges(TempTDS);

                return true;
            }
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber, Boolean ACommitTransaction = true)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;
            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            MainDS = new GLBatchTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                ACommitTransaction,
                delegate
                {
                    try
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                        NewRow.LedgerNumber = ALedgerNumber;
                        MainDS.ALedger[0].LastBatchNumber++;
                        NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                        NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                        NewRow.BatchYear = MainDS.ALedger[0].CurrentFinancialYear;
                        MainDS.ABatch.Rows.Add(NewRow);

                        GLBatchTDSAccess.SubmitChanges(MainDS);

                        MainDS.AcceptChanges();

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage =
                            String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                                    Environment.NewLine + Environment.NewLine + ex.ToString()),
                                ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Critical;
                        VerificationResult = new TVerificationResultCollection();
                        VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                        throw new EVerificationResultsException(ErrorMessage, VerificationResult, ex.InnerException);
                    }
                });

            return MainDS;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchDescription"></param>
        /// <param name="ABatchControlTotal"></param>
        /// <param name="ADateEffective"></param>
        /// <returns></returns>
        public static GLBatchTDS CreateABatch(
            Int32 ALedgerNumber,
            string ABatchDescription,
            decimal ABatchControlTotal,
            DateTime ADateEffective)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;
            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            MainDS = new GLBatchTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                        NewRow.LedgerNumber = ALedgerNumber;
                        MainDS.ALedger[0].LastBatchNumber++;
                        NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                        NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                        NewRow.BatchYear = MainDS.ALedger[0].CurrentFinancialYear;

                        int FinancialYear, FinancialPeriod;

                        if (ADateEffective != default(DateTime))
                        {
                            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateEffective, out FinancialYear, out FinancialPeriod,
                                Transaction, false);
                            NewRow.DateEffective = ADateEffective;
                            NewRow.BatchPeriod = FinancialPeriod;
                            NewRow.BatchYear = FinancialYear;
                        }

                        NewRow.BatchDescription = ABatchDescription;
                        NewRow.BatchControlTotal = ABatchControlTotal;
                        MainDS.ABatch.Rows.Add(NewRow);

                        GLBatchTDSAccess.SubmitChanges(MainDS);

                        MainDS.AcceptChanges();

                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage =
                            String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                                    Environment.NewLine + Environment.NewLine + ex.ToString()),
                                ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Critical;
                        VerificationResult = new TVerificationResultCollection();
                        VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                        throw new EVerificationResultsException(ErrorMessage, VerificationResult, ex.InnerException);
                    }
                });

            return MainDS;
        }

        /// <summary>
        /// create a new recurring batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateARecurringBatch(Int32 ALedgerNumber)
        {
            bool NewTransactionStarted = false;

            GLBatchTDS MainDS = null;
            GLBatchTDS Temp = null;

            //Error handling
            string ErrorContext = "Create a recurring Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                MainDS = new GLBatchTDS();
                Temp = new GLBatchTDS();

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransactionStarted);

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                ARecurringBatchAccess.LoadViaALedger(Temp, ALedgerNumber, Transaction);

                DataView RecurringBatchDV = new DataView(Temp.ARecurringBatch);
                RecurringBatchDV.RowFilter = string.Empty;
                RecurringBatchDV.Sort = string.Format("{0} DESC",
                    ARecurringBatchTable.GetBatchNumberDBName());

                //Recurring batch numbers can be reused so check each time for current highest number
                if (RecurringBatchDV.Count > 0)
                {
                    MainDS.ALedger[0].LastRecurringBatchNumber = (int)(RecurringBatchDV[0][ARecurringBatchTable.GetBatchNumberDBName()]);
                }
                else
                {
                    MainDS.ALedger[0].LastRecurringBatchNumber = 0;
                }

                ARecurringBatchRow NewRow = MainDS.ARecurringBatch.NewRowTyped(true);
                NewRow.LedgerNumber = ALedgerNumber;
                NewRow.BatchNumber = ++MainDS.ALedger[0].LastRecurringBatchNumber;
                MainDS.ARecurringBatch.Rows.Add(NewRow);

                GLBatchTDSAccess.SubmitChanges(MainDS);

                MainDS.AcceptChanges();
                Temp.RejectChanges();
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a recurring batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult = new TVerificationResultCollection();
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                throw new EVerificationResultsException(ErrorMessage, VerificationResult, ex.InnerException);
            }
            finally
            {
                if (NewTransactionStarted)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            return MainDS;
        }

        /// <summary>
        /// Create a new journal as per gl1120.i
        /// </summary>
        public static bool CreateAJournal(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber, Int32 ABatchNumber, Int32 ALastJournalNumber,
            string AJournalDescription, string ACurrency, decimal AXRateToBase,
            DateTime ADateEffective, Int32 APeriodNumber, out Int32 AJournalNumber)
        {
            bool CreationSuccessful = false;

            AJournalNumber = 0;

            //Error handling
            string ErrorContext = "Create a Journal";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = AMainDS.AJournal.NewRowTyped();
                JournalRow.LedgerNumber = ALedgerNumber;
                JournalRow.BatchNumber = ABatchNumber;
                AJournalNumber = ALastJournalNumber + 1;
                JournalRow.JournalNumber = AJournalNumber;
                JournalRow.JournalDescription = AJournalDescription;
                JournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                JournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                JournalRow.TransactionCurrency = ACurrency;
                JournalRow.ExchangeRateToBase = AXRateToBase;
                JournalRow.DateEffective = ADateEffective;
                JournalRow.JournalPeriod = APeriodNumber;
                AMainDS.AJournal.Rows.Add(JournalRow);

                //Update the Last Journal
                ABatchRow BatchRow = (ABatchRow)AMainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber });
                BatchRow.LastJournal = AJournalNumber;

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }

        /// <summary>
        /// create a record for a_transaction
        /// </summary>
        public static bool CreateATransaction(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string ANarrative,
            string AAccountCode,
            string ACostCentreCode,
            decimal ATransAmount,
            DateTime ATransDate,
            bool ADebCredIndicator,
            string AReference,
            bool ASystemGenerated,
            decimal ABaseAmount,
            out int ATransactionNumber)
        {
            bool CreationSuccessful = false;

            ATransactionNumber = 0;

            //Error handling
            string ErrorContext = "Create a Transaction";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = (AJournalRow)AMainDS.AJournal.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber });

                //Increment the LastTransactionNumber
                JournalRow.LastTransactionNumber++;
                ATransactionNumber = JournalRow.LastTransactionNumber;

                ATransactionRow TransactionRow = AMainDS.ATransaction.NewRowTyped();

                TransactionRow.LedgerNumber = ALedgerNumber;
                TransactionRow.BatchNumber = ABatchNumber;
                TransactionRow.JournalNumber = AJournalNumber;
                TransactionRow.TransactionNumber = ATransactionNumber;
                TransactionRow.Narrative = ANarrative;
                TransactionRow.Reference = AReference;
                TransactionRow.AccountCode = AAccountCode;
                TransactionRow.CostCentreCode = ACostCentreCode;
                TransactionRow.DebitCreditIndicator = ADebCredIndicator;
                TransactionRow.SystemGenerated = ASystemGenerated;
                TransactionRow.AmountInBaseCurrency = ABaseAmount;
                TransactionRow.TransactionAmount = ATransAmount;
                TransactionRow.TransactionDate = ATransDate;

                AMainDS.ATransaction.Rows.Add(TransactionRow);

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;

                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }
    }
}
