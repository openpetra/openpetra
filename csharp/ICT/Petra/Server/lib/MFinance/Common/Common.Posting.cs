//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh, christophert
//
// Copyright 2004-2024 by OM International
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


using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Common.Verification.Exceptions;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

using Ict.Petra.Server.App.Core;
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
        // public static PrintReportOnClient PrintReportOnClientDelegate;

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

            int RetVal = 0;

            try
            {
                ALedgerRow LedgerRow = AMainDS.ALedger[0];

                #region Validate Data

                if (LedgerRow == null)
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }

                #endregion Validate Data

                AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

                // row.GlmSequence will be set by SubmitChanges
                GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Count * -1) - 1;
                GLMRow.LedgerNumber = ALedgerNumber;
                GLMRow.Year = LedgerRow.CurrentFinancialYear;
                GLMRow.AccountCode = AAccountCode;
                GLMRow.CostCentreCode = ACostCentreCode;

                AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

                for (int PeriodCount = 1; PeriodCount < LedgerRow.NumberOfAccountingPeriods + LedgerRow.NumberFwdPostingPeriods + 1; PeriodCount++)
                {
                    AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                    PeriodRow.GlmSequence = GLMRow.GlmSequence;
                    PeriodRow.PeriodNumber = PeriodCount;
                    AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
                }

                RetVal = GLMRow.GlmSequence;
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
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

            int RetVal = 0;

            try
            {
                ALedgerRow LedgerRow = AMainDS.ALedger[0];

                #region Validate Data

                if (LedgerRow == null)
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }

                #endregion Validate Data

                AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

                // row.GlmSequence will be set by SubmitChanges
                GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
                GLMRow.LedgerNumber = ALedgerNumber;
                GLMRow.Year = AYear;
                GLMRow.AccountCode = AAccountCode;
                GLMRow.CostCentreCode = ACostCentreCode;

                AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

                for (int PeriodCount = 1; PeriodCount < LedgerRow.NumberOfAccountingPeriods + LedgerRow.NumberFwdPostingPeriods + 1; PeriodCount++)
                {
                    AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                    PeriodRow.GlmSequence = GLMRow.GlmSequence;
                    PeriodRow.PeriodNumber = PeriodCount;
                    AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
                }

                RetVal = GLMRow.GlmSequence;
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// load the batch and all associated tables into the typed dataset
        /// </summary>
        public static GLBatchTDS LoadGLBatchData(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            TDataBase ADataBase = null)
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadGLBatchData", ADataBase);

            try
            {
                db.ReadTransaction(
                    ref Transaction,
                    delegate
                    {
                        GLBatchDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, ref Transaction, ref Verifications);
                    });

                AVerifications = Verifications;
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
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
        /// <returns>CANNOT return a Null dataset. Instead throws an error!</returns>
        public static GLBatchTDS LoadGLBatchData(Int32 ALedgerNumber,
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
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return GLBatchDS;
        }

        /// <summary>
        /// load the tables that are needed for posting
        /// </summary>
        /// <returns></returns>
        private static GLPostingTDS LoadGLDataForPosting(Int32 ALedgerNumber, TDataBase ADataBase)
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

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadGLDataForPosting", ADataBase);

            try
            {
                db.ReadTransaction(
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

                        AAnalysisTypeAccess.LoadViaALedger(PostingDS, ALedgerNumber, Transaction);
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
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return PostingDS;
        }

        /// <summary>
        /// Load all GLM and GLMPeriod records for the batch period and the following periods, since that will avoid loading them one by one during submitchanges.
        /// this is called after ValidateBatchAndTransactions, because the BatchYear and BatchPeriod are validated and recalculated there
        ///
        /// This should probably be changed, in the new, skinny summarization, only a few rows need to be accessed.
        /// </summary>
        private static void LoadGLMData(ref GLPostingTDS AGLPostingDS, Int32 ALedgerNumber, ABatchRow ABatchToPost, TDataBase ADataBase)
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
                            "Function:{0} - The GL Batch to post data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLPostingTDS GLPostingDS = AGLPostingDS;

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadGLMData", ADataBase);

            try
            {
                db.ReadTransaction(
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
                        db.Select(GLPostingDS,
                            query,
                            GLPostingDS.AGeneralLedgerMasterPeriod.TableName, Transaction, parameters.ToArray());
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
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
            out TVerificationResultCollection AVerifications,
            TDataBase ADataBase)
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
                            "Function:{0} - The GL Batch to post data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            TDataBase db = DBAccess.Connect("ValidateGLBatchAndTransactions", ADataBase);

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

            //Check the validity of the Journal and transaction numbering
            // This will also correct invalid LastJournal and LastTransaction numbers
            if (!ValidateGLBatchJournalNumbering(ref AGLBatchDS, ref ABatchToPost, ref AVerifications, db)
                || !ValidateGLJournalTransactionNumbering(ref AGLBatchDS, ref ABatchToPost, ref AVerifications, db))
            {
                return false;
            }

            // Calculate the base currency amounts for each transaction, using the exchange rate from the journals.
            // erm - this is done already? I don't want to do it here, since my journal may contain forex-reval elements.

            // Calculate the credit and debit totals
            GLRoutines.UpdateBatchTotals(ref AGLBatchDS, ref ABatchToPost);

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
            GLBatchTDS GLBatchDS = AGLBatchDS;

            TDBTransaction Transaction = new TDBTransaction();

            db.ReadTransaction(
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

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return TVerificationHelper.IsNullOrOnlyNonCritical(AVerifications);
        }

        private static bool ValidateGLBatchJournalNumbering(ref GLBatchTDS AGLBatch,
            ref ABatchRow ABatchToPost,
            ref TVerificationResultCollection AVerifications,
            TDataBase ADataBase)
        {
            #region Validate Arguments

            if ((AGLBatch.ABatch == null) || (AGLBatch.ABatch.Count == 0) || (ABatchToPost == null))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - No GL Batch data is present!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Default to most likely outcome
            bool NumberingIsValid = true;

            string SQLStatement = string.Empty;
            string TempTableName = "TempCheckForConsecutiveJournals";

            //Parameters for SQL as strings
            string prmLedgerNumber = ABatchToPost.LedgerNumber.ToString();
            string prmBatchNumber = ABatchToPost.BatchNumber.ToString();

            //Tables with alias
            string BatchTableAlias = "b";
            string bBatchTable = ABatchTable.GetTableDBName() + " " + BatchTableAlias;
            string JournalTableAlias = "j";
            string jJournalTable = AJournalTable.GetTableDBName() + " " + JournalTableAlias;

            //Table: ABudgetTable and fields
            string bLedgerNumber = BatchTableAlias + "." + ABatchTable.GetLedgerNumberDBName();
            string bBatchNumber = BatchTableAlias + "." + ABatchTable.GetBatchNumberDBName();
            string bBatchNumberAlias = "BatchNumber";
            string bBatchLastJournal = BatchTableAlias + "." + ABatchTable.GetLastJournalDBName();
            string bBatchLastJournalAlias = "BatchLastJournal";
            string jLedgerNumber = JournalTableAlias + "." + AJournalTable.GetLedgerNumberDBName();
            string jBatchNumber = JournalTableAlias + "." + AJournalTable.GetBatchNumberDBName();
            string jJournalNumber = JournalTableAlias + "." + AJournalTable.GetJournalNumberDBName();
            string jFirstJournalAlias = "FirstJournal";
            string jLastJournalAlias = "LastJournal";
            string jCountJournalAlias = "CountJournal";

            try
            {
                DataTable tempTable = AGLBatch.Tables.Add(TempTableName);
                tempTable.Columns.Add(bBatchNumberAlias, typeof(Int32));
                tempTable.Columns.Add(bBatchLastJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jFirstJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jLastJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jCountJournalAlias, typeof(Int32));

                SQLStatement = "SELECT " + bBatchNumber + " " + bBatchNumberAlias + "," +
                               "      MIN(" + bBatchLastJournal + ") " + bBatchLastJournalAlias + "," +
                               "      COALESCE(MIN(" + jJournalNumber + "), 0) " + jFirstJournalAlias + "," +
                               "      COALESCE(MAX(" + jJournalNumber + "), 0) " + jLastJournalAlias + "," +
                               "      Count(" + jJournalNumber + ") " + jCountJournalAlias +
                               " FROM " + bBatchTable + " LEFT OUTER JOIN " + jJournalTable +
                               "        ON " + bLedgerNumber + " = " + jLedgerNumber +
                               "         AND " + bBatchNumber + " = " + jBatchNumber +
                               " WHERE " + bLedgerNumber + " = " + prmLedgerNumber +
                               "   AND " + bBatchNumber + " = " + prmBatchNumber +
                               " GROUP BY " + bBatchNumber + ";";

                //Create temp table to check veracity of Journal numbering
                GLBatchTDS gLBatch = AGLBatch;
                TDBTransaction transaction = new TDBTransaction();
                TDataBase db = DBAccess.Connect("ValidateGLBatchJournalNumbering", ADataBase);

                db.ReadTransaction(
                    ref transaction,
                    delegate
                    {
                        db.Select(gLBatch, SQLStatement, TempTableName, transaction);
                    });

                //As long as Batches exist, rows will be returned
                int numTempRows = AGLBatch.Tables[TempTableName].Rows.Count;

                DataView tempDV = new DataView(AGLBatch.Tables[TempTableName]);

                //Confirm are all equal and correct - the most common
                tempDV.RowFilter = string.Format("{0}={1} And {1}={2} And {0}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                //If all records are correct, nothing to do
                if (tempDV.Count == numTempRows)
                {
                    return NumberingIsValid;
                }

                //!!Reaching this point means there are issues that need addressing.

                //Confirm that no negative numbers exist
                tempDV.RowFilter = string.Format("{0} < 0 Or {1} < 0",
                    bBatchLastJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Batches have a negative LastJournalNumber or have Journals with a negative JournalNumber!";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1}",
                            Environment.NewLine,
                            drv[bBatchNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber,
                                ABatchToPost.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //Display non-sequential journals
                tempDV.RowFilter = string.Format("{2}>0 And ({3}<>1 Or {1}<>{2})",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Batches have gaps in their Journal numbering! You will need to cancel the Batch(es) and recreate:";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1}",
                            Environment.NewLine,
                            drv[bBatchNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber,
                                ABatchToPost.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //The next most likely, is where the BatchLastJournal needs updating
                //Display mismatched journal last number
                tempDV.RowFilter = string.Format("{0}<>{1} And {1}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    ABatchToPost.LastJournal = Convert.ToInt32(tempDV[0][jLastJournalAlias]);
                }

                if (ADataBase == null)
                {
                    db.CloseDBConnection();
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (AGLBatch.Tables.Contains(TempTableName))
                {
                    AGLBatch.Tables.Remove(TempTableName);
                }
            }

            return NumberingIsValid;
        }

        private static bool ValidateGLJournalTransactionNumbering(ref GLBatchTDS AGLBatch,
            ref ABatchRow ABatchToPost,
            ref TVerificationResultCollection AVerifications,
            TDataBase ADataBase)
        {
            #region Validate Arguments

            if (AGLBatch.AJournal == null)
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - No GL Journal data is present!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Default to most likely outcome
            bool NumberingIsValid = true;

            string SQLStatement = string.Empty;
            string TempTableName = "TempCheckForConsecutiveTransactions";

            //Parameters for SQL as strings
            string prmLedgerNumber = ABatchToPost.LedgerNumber.ToString();
            string prmBatchNumber = ABatchToPost.BatchNumber.ToString();

            //Tables with alias
            string JournalTableAlias = "j";
            string jJournalTable = AJournalTable.GetTableDBName() + " " + JournalTableAlias;

            string TransactionTableAlias = "t";
            string tTransactionTable = ATransactionTable.GetTableDBName() + " " + TransactionTableAlias;

            //Fields and Aliases
            string jLedgerNumber = JournalTableAlias + "." + AJournalTable.GetLedgerNumberDBName();
            string jBatchNumber = JournalTableAlias + "." + AJournalTable.GetBatchNumberDBName();
            string jBatchNumberAlias = "BatchNumber";
            string jJournalNumber = JournalTableAlias + "." + AJournalTable.GetJournalNumberDBName();
            string jJournalNumberAlias = "JournalNumber";
            string jJournalLastTransaction = JournalTableAlias + "." + AJournalTable.GetLastTransactionNumberDBName();
            string jJournalLastTransactionAlias = "JournalLastTransaction";

            string tLedgerNumber = TransactionTableAlias + "." + ATransactionTable.GetLedgerNumberDBName();
            string tBatchNumber = TransactionTableAlias + "." + ATransactionTable.GetBatchNumberDBName();
            string tJournalNumber = TransactionTableAlias + "." + ATransactionTable.GetJournalNumberDBName();
            string tTransactionNumber = TransactionTableAlias + "." + ATransactionTable.GetTransactionNumberDBName();
            string tFirstTransactionAlias = "FirstTransaction";
            string tLastTransactionAlias = "LastTransaction";
            string tCountTransactionAlias = "CountTransaction";

            try
            {
                DataTable tempTable = AGLBatch.Tables.Add(TempTableName);
                tempTable.Columns.Add(jBatchNumberAlias, typeof(Int32));
                tempTable.Columns.Add(jJournalNumberAlias, typeof(Int32));
                tempTable.Columns.Add(jJournalLastTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tFirstTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tLastTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tCountTransactionAlias, typeof(Int32));

                SQLStatement = "SELECT " + jBatchNumber + " " + jBatchNumberAlias + ", " + jJournalNumber + " " + jJournalNumberAlias + "," +
                               "      MIN(" + jJournalLastTransaction + ") " + jJournalLastTransactionAlias + "," +
                               "      COALESCE(MIN(" + tTransactionNumber + "), 0) " + tFirstTransactionAlias + "," +
                               "      COALESCE(MAX(" + tTransactionNumber + "), 0) " + tLastTransactionAlias + "," +
                               "      Count(" + tTransactionNumber + ") " + tCountTransactionAlias +
                               " FROM " + jJournalTable + " LEFT OUTER JOIN " + tTransactionTable +
                               "        ON " + jLedgerNumber + " = " + tLedgerNumber +
                               "         AND " + jBatchNumber + " = " + tBatchNumber +
                               "         AND " + jJournalNumber + " = " + tJournalNumber +
                               " WHERE " + jLedgerNumber + " = " + prmLedgerNumber +
                               "   AND " + jBatchNumber + " = " + prmBatchNumber +
                               " GROUP BY " + jBatchNumber + ", " + jJournalNumber + ";";

                //Create temp table to check veracity of Transaction numbering
                GLBatchTDS gLBatch = AGLBatch;
                TDBTransaction transaction = new TDBTransaction();
                TDataBase db = DBAccess.Connect("ValidateGLJournalTransactionNumbering", ADataBase);

                db.ReadTransaction(
                    ref transaction,
                    delegate
                    {
                        db.Select(gLBatch, SQLStatement, TempTableName, transaction);
                    });

                //As long as Batches exist, rows will be returned
                int numTempRows = AGLBatch.Tables[TempTableName].Rows.Count;

                DataView tempDV = new DataView(AGLBatch.Tables[TempTableName]);

                //Confirm are all equal and correct - the most common
                tempDV.RowFilter = string.Format("{0}={1} And {1}={2} And {0}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                //If all records are correct, nothing to do
                if (tempDV.Count == numTempRows)
                {
                    return NumberingIsValid;
                }

                //!!Reaching this point means there are issues that need addressing.

                //Confirm that no negative numbers exist
                tempDV.RowFilter = string.Format("{0} < 0 Or {1} < 0",
                    jJournalLastTransactionAlias,
                    tFirstTransactionAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Journals have a negative LastTransactionNumber or have Transactions with a negative TransactionNumber!";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1} Journal:{2}",
                            Environment.NewLine,
                            drv[jBatchNumberAlias],
                            drv[jJournalNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber,
                                ABatchToPost.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //Display non-sequential transactions
                tempDV.RowFilter = string.Format("{2}>0 And ({3}<>1 Or {1}<>{2})",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Journals have gaps in their Transaction numbering! You will need to cancel the Journal(s) and recreate:";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}GL Batch:{1} Journal:{2}",
                            Environment.NewLine,
                            drv[jBatchNumberAlias],
                            drv[jJournalNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber,
                                ABatchToPost.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //The next most likely, is where the JournalLastTransaction needs updating
                //Display mismatched journal last number
                tempDV.RowFilter = string.Format("{0}<>{1} And {1}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                DataView journalsDV = new DataView(AGLBatch.AJournal);

                if (tempDV.Count > 0)
                {
                    //This means the LastTransactionNumber field needs to be updated and is incorrect
                    foreach (DataRowView drv in tempDV)
                    {
                        journalsDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                            AJournalTable.GetBatchNumberDBName(),
                            drv[jBatchNumberAlias],
                            AJournalTable.GetJournalNumberDBName(),
                            drv[jJournalNumberAlias]);

                        foreach (DataRowView journals in journalsDV)
                        {
                            AJournalRow journalRow = (AJournalRow)journals.Row;
                            journalRow.LastTransactionNumber = Convert.ToInt32(drv[tLastTransactionAlias]);
                        }
                    }
                }

                if (ADataBase == null)
                {
                    db.CloseDBConnection();
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (AGLBatch.Tables.Contains(TempTableName))
                {
                    AGLBatch.Tables.Remove(TempTableName);
                }
            }

            return NumberingIsValid;
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

                    int counter = 0;

                    while (counter < ANView.Count)
                    {
                        AAnalysisAttributeRow attributeRow = (AAnalysisAttributeRow)ANView[counter].Row;

                        ATransAnalAttribRow aTransAttribRow =
                            (ATransAnalAttribRow)AGLBatchDS.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber,
                                                                                                      transRow.JournalNumber,
                                                                                                      transRow.TransactionNumber,
                                                                                                      attributeRow.AnalysisTypeCode });

                        if (aTransAttribRow == null)
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post GL Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Analysis Type {0} is missing values in Journal {1}, Transaction {2}"),
                                        attributeRow.AnalysisTypeCode, transRow.JournalNumber, transRow.TransactionNumber),
                                    TResultSeverity.Resv_Critical));

                            CriticalError = true;
                            break;
                        }
                        else
                        {
                            String analAttrValue = aTransAttribRow.AnalysisAttributeValue;

                            if ((analAttrValue == null) || (analAttrValue.Length == 0))
                            {
                                AVerifications.Add(new TVerificationResult(
                                        String.Format(Catalog.GetString("Cannot post GL Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                        String.Format(Catalog.GetString("Analysis Type {0} is missing values in Journal {1}, Transaction {2}"),
                                            attributeRow.AnalysisTypeCode, transRow.JournalNumber, transRow.TransactionNumber),
                                        TResultSeverity.Resv_Critical));

                                CriticalError = true;
                                break;
                            }
                            else
                            {
                                AFreeformAnalysisRow afaRow = (AFreeformAnalysisRow)APostingDS.AFreeformAnalysis.Rows.Find(
                                    new Object[] { ALedgerNumber, attributeRow.AnalysisTypeCode, analAttrValue });

                                if (afaRow == null)
                                {
                                    // this would cause a constraint error and is only possible in a development/sqlite environment
                                    AVerifications.Add(new TVerificationResult(
                                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                            String.Format(Catalog.GetString("Analysis Type {0} has invalid value in journal {1}, transaction {2}"),
                                                attributeRow.AnalysisTypeCode, transRow.JournalNumber, transRow.TransactionNumber),
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
                                                        "Analysis Type {0} has inactive value: '{1}' in journal {2}, transaction {3}"),
                                                    attributeRow.AnalysisTypeCode,
                                                    analAttrValue, transRow.JournalNumber, transRow.TransactionNumber),
                                                TResultSeverity.Resv_Critical));

                                        CriticalError = true;
                                        break;
                                    } // if
                                } // else
                            } // else
                        } // else

                        counter++;
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

        /// <summary>
        /// mark each journal, each transaction as being posted;
        /// add sums for costcentre/account combinations
        /// </summary>
        /// <param name="AMainDS">can contain several batches and journals and transactions</param>
        /// <param name="APostingDS"></param>
        /// <param name="APostingLevel">the balance changes at the posting level</param>
        /// <param name="ABatchToPost">the batch to post</param>
        /// <returns>a list with the sums for each costcentre/account combination</returns>
        private static SortedList <string, TAmount>MarkAsPostedAndCollectData(GLBatchTDS AMainDS,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel, ABatchRow ABatchToPost)
        {
            #region Validate Arguments

            if (AMainDS == null)
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
            else if (ABatchToPost == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch to post data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            DataView TransactionsDV = new DataView(AMainDS.ATransaction);

            TransactionsDV.Sort = ATransactionTable.GetJournalNumberDBName();

            foreach (AJournalRow journal in AMainDS.AJournal.Rows)
            {
                if ((journal.BatchNumber != ABatchToPost.BatchNumber) || (journal.JournalStatus == MFinanceConstants.BATCH_CANCELLED))
                {
                    continue;
                }

                decimal debitTotal, creditTotal, debitTotalBase, creditTotalBase, debitTotalIntl, creditTotalIntl;
                decimal newBaseAmount, newIntlAmount;
                debitTotal = creditTotal = debitTotalBase = creditTotalBase = debitTotalIntl = creditTotalIntl = 0.0m;
                newBaseAmount = newIntlAmount = 0.0m;

                foreach (DataRowView transactionview in TransactionsDV.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;

                    if (transaction.BatchNumber != ABatchToPost.BatchNumber)
                    {
                        continue;
                    }

                    transaction.TransactionStatus = true;

                    // get the account that this transaction is writing to
                    AAccountRow accountRow = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { transaction.LedgerNumber,
                                                                                                       transaction.AccountCode });

                    #region Validate Data

                    if (accountRow == null)
                    {
                        throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                    "Function:{0} - Account row data for Account code {1} in Ledger number {2} does not exist or could not be accessed!"),
                                Utilities.GetMethodName(true),
                                transaction.AccountCode,
                                transaction.LedgerNumber));
                    }

                    #endregion Validate Data

                    // We need to check for base/intl currency corrections (added 27-Sep-2016)
                    #region Make balancing adjustments to Base and Intl currency amounts, if required

                    // We keep a running total of transaction amounts in this journal
                    if (transaction.DebitCreditIndicator == true)
                    {
                        debitTotal += transaction.TransactionAmount;
                        debitTotalBase += transaction.AmountInBaseCurrency;
                        debitTotalIntl += transaction.AmountInIntlCurrency;
                    }
                    else
                    {
                        creditTotal += transaction.TransactionAmount;
                        creditTotalBase += transaction.AmountInBaseCurrency;
                        creditTotalIntl += transaction.AmountInIntlCurrency;
                    }

                    if ((debitTotal == creditTotal) && (debitTotal > 0.0m)
                        && ((debitTotalBase != creditTotalBase) || (debitTotalIntl != creditTotalIntl)))
                    {
                        // Our local currency debit/credit balances ok but there is a discrepancy in the base/intl total(s)
                        // This is where we calculate the adjusted value (which should only be a cent or two!)
                        if (transaction.DebitCreditIndicator == true)
                        {
                            newBaseAmount = Math.Abs((debitTotalBase - transaction.AmountInBaseCurrency) - creditTotalBase);
                            newIntlAmount = Math.Abs((debitTotalIntl - transaction.AmountInIntlCurrency) - creditTotalIntl);
                            debitTotalBase = debitTotalBase + newBaseAmount - transaction.AmountInBaseCurrency;
                            debitTotalIntl = debitTotalIntl + newIntlAmount - transaction.AmountInIntlCurrency;
                        }
                        else
                        {
                            newBaseAmount = Math.Abs((creditTotalBase - transaction.AmountInBaseCurrency) - debitTotalBase);
                            newIntlAmount = Math.Abs((creditTotalIntl - transaction.AmountInIntlCurrency) - debitTotalIntl);
                            creditTotalBase = creditTotalBase + newBaseAmount - transaction.AmountInBaseCurrency;
                            creditTotalIntl = creditTotalIntl + newIntlAmount - transaction.AmountInIntlCurrency;
                        }

                        if (newBaseAmount != transaction.AmountInBaseCurrency)
                        {
                            TLogging.Log(string.Format(
                                    "Posting: Making transaction base currency adjustment: {0} - {1:N2} -> {2:N2} (Batch {3}/Journal {4}/Trans {5})",
                                    transaction.DebitCreditIndicator ? "DR" : "CR",
                                    transaction.AmountInBaseCurrency,
                                    newBaseAmount,
                                    journal.BatchNumber,
                                    journal.JournalNumber,
                                    transaction.TransactionNumber));
                        }

                        if (newIntlAmount != transaction.AmountInIntlCurrency)
                        {
                            TLogging.Log(string.Format(
                                    "Posting: Making transaction intl currency adjustment: {0} - {1:N2} -> {2:N2} (Batch {3}/Journal {4}/Trans {5})",
                                    transaction.DebitCreditIndicator ? "DR" : "CR",
                                    transaction.AmountInIntlCurrency,
                                    newIntlAmount,
                                    journal.BatchNumber,
                                    journal.JournalNumber,
                                    transaction.TransactionNumber));
                        }

                        // Apply the adjusted value
                        transaction.AmountInBaseCurrency = newBaseAmount;
                        transaction.AmountInIntlCurrency = newIntlAmount;
                    }

                    #endregion

                    // Set the sign of the amounts according to the debit/credit indicator
                    decimal SignBaseAmount = transaction.AmountInBaseCurrency;
                    decimal SignIntlAmount = transaction.AmountInIntlCurrency;
                    decimal SignTransAmount = transaction.TransactionAmount;

                    if (accountRow.DebitCreditIndicator != transaction.DebitCreditIndicator)
                    {
                        SignBaseAmount *= -1.0M;
                        SignIntlAmount *= -1.0M;
                        SignTransAmount *= -1.0M;
                    }

                    string key = TAmount.MakeKey(transaction.AccountCode, transaction.CostCentreCode);

                    if (!APostingLevel.ContainsKey(key))
                    {
                        APostingLevel.Add(key, new TAmount());
                    }

                    // Fill in the new total in these variables - they will be used later to update the YearToDate columns in the GLM tables
                    // See SummarizeData() method in this class
                    APostingLevel[key].BaseAmount += SignBaseAmount;
                    APostingLevel[key].IntlAmount += SignIntlAmount;

                    // Only foreign currency accounts store a value in the transaction currency,
                    // if the transaction was actually in the foreign currency.
                    if (accountRow.ForeignCurrencyFlag && (journal.TransactionCurrency == accountRow.ForeignCurrencyCode))
                    {
                        APostingLevel[key].TransAmount += SignTransAmount;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            // get all accounts that each posting level account is directly or indirectly posting to
            AAccountTree = new SortedList <string, TAccountTreeElement>();

            foreach (string postingLevelKey in APostingLevel.Keys)
            {
                string accountCode = TAmount.GetAccountCode(postingLevelKey);

                // only once for each account, even though there might be several entries for one account in APostingLevel because of different costcentres
                if (AAccountTree.ContainsKey(TAccountTreeElement.MakeKey(accountCode, accountCode)))
                {
                    continue;
                }

                AAccountRow accountRow = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, accountCode });

                #region Validate Data

                if (accountRow == null)
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Account row data for Account code {1} in Ledger number {2} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            accountCode,
                            ALedgerNumber));
                }

                #endregion Validate Data

                bool DebitCreditIndicator = accountRow.DebitCreditIndicator;
                AAccountTree.Add(TAccountTreeElement.MakeKey(accountCode, accountCode),
                    new TAccountTreeElement(false, accountRow.ForeignCurrencyFlag));

                AAccountHierarchyDetailRow HierarchyDetail =
                    (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, accountCode });

                while (HierarchyDetail != null)
                {
                    accountRow = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, HierarchyDetail.AccountCodeToReportTo });

                    if (accountRow == null)
                    {
                        // current account is BAL SHT, and it reports nowhere (account with name = ledgernumber does not exist)
                        break;
                    }

                    AAccountTree.Add(TAccountTreeElement.MakeKey(accountCode, HierarchyDetail.AccountCodeToReportTo),
                        new TAccountTreeElement(DebitCreditIndicator != accountRow.DebitCreditIndicator, accountRow.ForeignCurrencyFlag));

                    HierarchyDetail = (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, HierarchyDetail.AccountCodeToReportTo });
                }
            }

            ACostCentreTree = new SortedList <string, string>();

            foreach (string postingLevelKey in APostingLevel.Keys)
            {
                string costCentreCode = TAmount.GetCostCentreCode(postingLevelKey);

                // only once for each cost centre
                if (ACostCentreTree.ContainsKey(costCentreCode + ":" + costCentreCode))
                {
                    continue;
                }

                ACostCentreTree.Add(costCentreCode + ":" + costCentreCode,
                    costCentreCode + ":" + costCentreCode);

                ACostCentreRow costCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, costCentreCode });

                while (costCentre != null && !costCentre.IsCostCentreToReportToNull())
                {
                    ACostCentreTree.Add(costCentreCode + ":" + costCentre.CostCentreToReportTo,
                        costCentreCode + ":" + costCentre.CostCentreToReportTo);

                    costCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, costCentre.CostCentreToReportTo });
                }
            }

            return true;
        }

        /// <summary>
        /// For each posting level, propagate the value upwards through both the account and the cost centre hierarchy in glm master;
        /// also propagate the value from the posting period through the following periods;
        /// </summary>
        private static bool SummarizeData(
            GLPostingTDS APostingDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel,
            ref SortedList <string, TAccountTreeElement>AAccountTree,
            ref SortedList <string, string>ACostCentreTree)
        {
            #region Validate Arguments

            if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((APostingDS.ALedger == null) || (APostingDS.ALedger.Count == 0))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - Ledger data in the GL Posting dataset does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

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

            // Loop through the posting data collected earlier.
            foreach (string postingLevelKey in APostingLevel.Keys)
            {
                String[] keyParts = postingLevelKey.Split(':');

                string accountCode = keyParts[0];
                string costCentreCode = keyParts[1];

                TAmount postingLevelElement = APostingLevel[postingLevelKey];

                // Combine the summarization trees for both the account and the cost centre.
                foreach (string accountTreeKey in AAccountTree.Keys)
                {
                    String[] accountKeyParts = accountTreeKey.Split(':');

                    if (accountKeyParts[0] == accountCode)
                    {
                        string accountCodeToReportTo = accountKeyParts[1];
                        TAccountTreeElement accountTreeElement = AAccountTree[accountTreeKey];

                        foreach (string costCentreKey in ACostCentreTree.Keys)
                        {
                            String[] cCKeyParts = costCentreKey.Split(':');

                            if (cCKeyParts[0] == costCentreCode)
                            {
                                string costCentreCodeToReportTo = cCKeyParts[1];
                                decimal signBaseAmount = postingLevelElement.BaseAmount;
                                decimal signIntlAmount = postingLevelElement.IntlAmount;
                                decimal signTransAmount = postingLevelElement.TransAmount;

                                // Set the sign of the amounts according to the debit/credit indicator
                                if (accountTreeElement.Invert)
                                {
                                    signBaseAmount *= -1;
                                    signIntlAmount *= -1;
                                    signTransAmount *= -1;
                                }

                                // Find the summary level, creating it if it does not already exist.
                                int gLMMasterIndex = GLMMasterView.Find(new object[] { accountCodeToReportTo, costCentreCodeToReportTo });

                                if (gLMMasterIndex == -1)
                                {
                                    CreateGLMYear(
                                        ref APostingDS,
                                        APostingDS.ALedger[0].LedgerNumber,
                                        accountCodeToReportTo,
                                        costCentreCodeToReportTo);

                                    gLMMasterIndex = GLMMasterView.Find(new object[] { accountCodeToReportTo, costCentreCodeToReportTo });
                                }

                                AGeneralLedgerMasterRow gLMRow = (AGeneralLedgerMasterRow)GLMMasterView[gLMMasterIndex].Row;

                                #region Validate Data

                                if (gLMRow == null)
                                {
                                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                                "Function:{0} - GLM row data does not exist or could not be accessed!"),
                                            Utilities.GetMethodName(true)));
                                }

                                #endregion Validate Data

                                gLMRow.YtdActualBase += signBaseAmount;
                                gLMRow.YtdActualIntl += signIntlAmount;

                                if (accountTreeElement.Foreign)
                                {
                                    if (gLMRow.IsYtdActualForeignNull())
                                    {
                                        gLMRow.YtdActualForeign = signTransAmount;
                                    }
                                    else
                                    {
                                        gLMRow.YtdActualForeign += signTransAmount;
                                    }
                                }

                                if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    gLMRow.ClosingPeriodActualBase += signBaseAmount;
                                    gLMRow.ClosingPeriodActualIntl += signIntlAmount;
                                }

                                // Add the data to forward periods, to the end of the GLMP list
                                for (Int32 PeriodCount = AFromPeriod;
                                     PeriodCount <= APostingDS.ALedger[0].NumberOfAccountingPeriods + APostingDS.ALedger[0].NumberFwdPostingPeriods;
                                     PeriodCount++)
                                {
                                    int gLMPeriodIndex = GLMPeriodView.Find(new object[] { gLMRow.GlmSequence, PeriodCount });
                                    AGeneralLedgerMasterPeriodRow gLMPeriodRow;

                                    if (gLMPeriodIndex == -1)
                                    {
                                        gLMPeriodRow = APostingDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                                        gLMPeriodRow.GlmSequence = gLMRow.GlmSequence;
                                        gLMPeriodRow.PeriodNumber = PeriodCount;
                                        APostingDS.AGeneralLedgerMasterPeriod.Rows.Add(gLMPeriodRow);
                                    }
                                    else
                                    {
                                        gLMPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[gLMPeriodIndex].Row;
                                    }

                                    gLMPeriodRow.ActualBase += signBaseAmount;
                                    gLMPeriodRow.ActualIntl += signIntlAmount;

                                    if (accountTreeElement.Foreign)
                                    {
                                        if (gLMPeriodRow.IsActualForeignNull())
                                        {
                                            gLMPeriodRow.ActualForeign = signTransAmount;
                                        }
                                        else
                                        {
                                            gLMPeriodRow.ActualForeign += signTransAmount;
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
            GLPostingTDS APostingDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel,
            TDataBase ADataBase)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((APostingDS.ALedger == null) || (APostingDS.ALedger.Count == 0))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - Ledger data in the GL Posting dataset does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

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

            foreach (string postingLevelKey in APostingLevel.Keys)
            {
                String[] keyParts = postingLevelKey.Split(':');

                string accountCode = keyParts[0];
                string costCentreCode = keyParts[1];

                TAmount postingLevelElement = APostingLevel[postingLevelKey];

                // Find the posting level, creating it if it does not already exist.
                int gLMMasterIndex = GLMMasterView.Find(new object[] { accountCode, costCentreCode });
                AGeneralLedgerMasterRow gLMRow;

                if (gLMMasterIndex == -1)
                {
                    CreateGLMYear(
                        ref APostingDS,
                        ALedgerNumber,
                        accountCode,
                        costCentreCode);

                    gLMMasterIndex = GLMMasterView.Find(new object[] { accountCode, costCentreCode });
                }

                gLMRow = (AGeneralLedgerMasterRow)GLMMasterView[gLMMasterIndex].Row;

                gLMRow.YtdActualBase += postingLevelElement.BaseAmount;
                gLMRow.YtdActualIntl += postingLevelElement.IntlAmount;

                AAccountRow accountRow = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, accountCode });

                #region Validate Data

                if (accountRow == null)
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Account row data for Account code {1} in Ledger number {2} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            accountCode,
                            ALedgerNumber));
                }

                #endregion Validate Data

                if (accountRow.ForeignCurrencyFlag)
                {
                    if (gLMRow.IsYtdActualForeignNull())
                    {
                        gLMRow.YtdActualForeign = postingLevelElement.TransAmount;
                    }
                    else
                    {
                        gLMRow.YtdActualForeign += postingLevelElement.TransAmount;
                    }

                    TLedgerInitFlag flag = new TLedgerInitFlag(ALedgerNumber, MFinanceConstants.LEDGER_INIT_FLAG_REVAL,
                        ADataBase);
                    flag.SetFlagComponent(MFinanceConstants.LEDGER_INIT_FLAG_REVAL, accountRow.AccountCode);
                }

                if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
                {
                    gLMRow.ClosingPeriodActualBase += postingLevelElement.BaseAmount;
                    gLMRow.ClosingPeriodActualIntl += postingLevelElement.IntlAmount;
                } // Last use of GlmRow in this routine ...

                // propagate the data through the following periods
                for (Int32 periodCount = AFromPeriod;
                     periodCount <= APostingDS.ALedger[0].NumberOfAccountingPeriods + APostingDS.ALedger[0].NumberFwdPostingPeriods;
                     periodCount++)
                {
                    int gLMPeriodIndex = GLMPeriodView.Find(new object[] { gLMRow.GlmSequence, periodCount });
                    AGeneralLedgerMasterPeriodRow gLMPeriodRow;

                    if (gLMPeriodIndex == -1)
                    {
                        gLMPeriodRow = APostingDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                        gLMPeriodRow.GlmSequence = gLMRow.GlmSequence;
                        gLMPeriodRow.PeriodNumber = periodCount;
                    }
                    else
                    {
                        gLMPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[gLMPeriodIndex].Row;
                    }

                    gLMPeriodRow.ActualBase += postingLevelElement.BaseAmount;
                    gLMPeriodRow.ActualIntl += postingLevelElement.IntlAmount;

                    if (accountRow.ForeignCurrencyFlag)
                    {
                        if (gLMPeriodRow.IsActualForeignNull())
                        {
                            gLMPeriodRow.ActualForeign = postingLevelElement.TransAmount;
                        }
                        else
                        {
                            gLMPeriodRow.ActualForeign += postingLevelElement.TransAmount;
                        }
                    }
                }
            }

            GLMMasterView.Sort = "";
            GLMPeriodView.Sort = "";

            return true;
        }

        //
        //
        // April 2015, Tim Ingham:
        //
        // NOTE that the full summarization that includes all the summary levels has been discontinued,
        // since the Open Petra reports only use the posting levels and calculate the summaries on the fly.
        // This makes Ledger posting MUCH faster.
        //
        // The full SummarizeData method, and its supporting CalculateTrees method, is still present,
        // and we could return to it if it became necessary.
        // Note: SummarizeDataSimple sets a REVAL flag on foreign currency accounts which SummarizeData did
        // not do. So if we _did_ return to it we'd need to make sure revaluation checks were handled correctly.

        private static void SummarizeInternal(Int32 ALedgerNumber,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel,
            Int32 AFromPeriod,
            bool ACalculatePostingTree,
            TDataBase ADataBase)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (APostingDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Posting dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            if (ACalculatePostingTree)
            {
                TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: CalculateTrees...");

                // key is PostingAccount, the value TAccountTreeElement describes the parent account and other details of the relation
                SortedList <string, TAccountTreeElement>AccountTree;

                // key is the PostingCostCentre, the value is the parent Cost Centre
                SortedList <string, string>CostCentreTree;

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
                SummarizeDataSimple(ALedgerNumber, APostingDS, AFromPeriod, ref APostingLevel, ADataBase);
            }
        }

        /// <summary>
        /// Write all changes to the database; on failure the whole transaction will be rolled back
        /// </summary>
        private static void SubmitChanges(GLPostingTDS AMainDS, TDataBase ADataBase)
        {
            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: SubmitChanges...");
            GLPostingTDSAccess.SubmitChanges(AMainDS.GetChangesTyped(true), ADataBase);
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumberToReverse <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumberToReverse);
            }

            #endregion Validate Arguments

            bool ReturnValue = false;

            GLBatchTDS MainDS = null;

            Int32 ReversalBatchNumber = -1;

            //Error handling
            string ErrorContext = "Reverse a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            AVerifications = null;
            TVerificationResultCollection Verifications = new TVerificationResultCollection();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("ReverseBatch");
            bool SubmissionOK = true;

            try
            {
                db.WriteTransaction(
                    ref Transaction, ref SubmissionOK,
                    delegate
                    {
                        MainDS = LoadGLBatchData(ALedgerNumber, ABatchNumberToReverse, ref Transaction, ref Verifications);
                        ABatchRow originalBatch = (ABatchRow)MainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumberToReverse });

                        int dateEffectiveYearNumber, dateEffectivePeriodNumber;

                        if (ADateForReversal.Equals(new DateTime(1900,1,1)))
                        {
                            ADateForReversal = originalBatch.DateEffective;

                            // if DateEffective is not in an open period, then use the earliest possible date from the first open period
                            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateForReversal, out dateEffectivePeriodNumber,
                                out dateEffectiveYearNumber,
                                Transaction, true);
                        }

                        if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, ADateForReversal, out dateEffectivePeriodNumber,
                                out dateEffectiveYearNumber,
                                Transaction))
                        {
                            ErrorMessage = Catalog.GetString("Date is outside of valid posting period");
                            ErrorType = TResultSeverity.Resv_Critical;
                            Verifications.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                        }
                        else
                        {
                            // get the data from the database into the MainDS
                            ABatchRow newBatchRow = MainDS.ABatch.NewRowTyped(true);
                            newBatchRow.LedgerNumber = ALedgerNumber;
                            MainDS.ALedger[0].LastBatchNumber++;
                            newBatchRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;

                            newBatchRow.DateEffective = ADateForReversal;
                            newBatchRow.BatchPeriod = dateEffectivePeriodNumber;
                            newBatchRow.BatchYear = dateEffectiveYearNumber;

                            #region Validate Data

                            if (originalBatch == null)
                            {
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - GL Batch data for Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                                        Utilities.GetMethodName(true),
                                        ABatchNumberToReverse,
                                        ALedgerNumber));
                            }

                            #endregion Validate Data

                            newBatchRow.BatchDescription = String.Format(Catalog.GetString("Reversal of {0}"), originalBatch.BatchDescription);
                            newBatchRow.LastJournal = originalBatch.LastJournal;
                            MainDS.ABatch.Rows.Add(newBatchRow);

                            MainDS.AJournal.DefaultView.Sort = AJournalTable.GetLedgerNumberDBName() + "," + AJournalTable.GetBatchNumberDBName();
                            DataRowView[] JournalsRowView = MainDS.AJournal.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse });

                            foreach (DataRowView rvJournal in JournalsRowView)
                            {
                                AJournalRow originalJournalRow = (AJournalRow)rvJournal.Row;
                                AJournalRow newJournalRow = MainDS.AJournal.NewRowTyped();

                                DataUtilities.CopyAllColumnValues(originalJournalRow, newJournalRow);

                                newJournalRow.BatchNumber = newBatchRow.BatchNumber;
                                newJournalRow.DateEffective = newBatchRow.DateEffective;
                                newJournalRow.JournalPeriod = newBatchRow.BatchPeriod;
                                newJournalRow.JournalStatus = newBatchRow.BatchStatus;
                                newJournalRow.JournalDescription =
                                    String.Format(Catalog.GetString("Reversal of {0}"), originalJournalRow.JournalDescription);
                                originalJournalRow.Reversed = true;
                                MainDS.AJournal.Rows.Add(newJournalRow);

                                MainDS.ATransaction.DefaultView.Sort = ATransactionTable.GetLedgerNumberDBName() + "," +
                                                                       ATransactionTable.GetBatchNumberDBName() + "," +
                                                                       ATransactionTable.GetJournalNumberDBName();
                                DataRowView[] TransactionsRowView =
                                    MainDS.ATransaction.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse,
                                                                                            originalJournalRow.JournalNumber });

                                foreach (DataRowView rvTransaction in TransactionsRowView)
                                {
                                    ATransactionRow originalTransaction = (ATransactionRow)rvTransaction.Row;
                                    ATransactionRow newTransactionRow = MainDS.ATransaction.NewRowTyped();

                                    DataUtilities.CopyAllColumnValues(originalTransaction, newTransactionRow);
                                    newTransactionRow.BatchNumber = newJournalRow.BatchNumber;
                                    newTransactionRow.JournalNumber = newJournalRow.JournalNumber;
                                    newTransactionRow.TransactionStatus = false;
                                    newTransactionRow.DebitCreditIndicator = !originalTransaction.DebitCreditIndicator;
                                    newTransactionRow.SystemGenerated = true;
                                    newTransactionRow.TransactionDate = ADateForReversal;
                                    newTransactionRow.Narrative = Catalog.GetString("Reverse of: ") + originalTransaction.Narrative +
                                                                  "(" + Catalog.GetString(" Batch: ") + originalTransaction.BatchNumber +
                                                                  Catalog.GetString(", Journal: ") + originalTransaction.JournalNumber +
                                                                  Catalog.GetString(", Transaction: ") + originalTransaction.TransactionNumber + ")";
                                    newTransactionRow.IchNumber = 0;

                                    MainDS.ATransaction.Rows.Add(newTransactionRow);

                                    MainDS.ATransAnalAttrib.DefaultView.Sort = ATransAnalAttribTable.GetLedgerNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetBatchNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetJournalNumberDBName() + "," +
                                                                               ATransAnalAttribTable.GetTransactionNumberDBName();
                                    DataRowView[] TransAnalAttribRowView =
                                        MainDS.ATransAnalAttrib.DefaultView.FindRows(new object[] { ALedgerNumber, ABatchNumberToReverse,
                                                                                                    originalJournalRow.JournalNumber,
                                                                                                    originalTransaction.TransactionNumber });

                                    foreach (DataRowView rvTransAnalAttrib in TransAnalAttribRowView)
                                    {
                                        ATransAnalAttribRow originalTransAnalAttrib = (ATransAnalAttribRow)rvTransAnalAttrib.Row;
                                        ATransAnalAttribRow newTransAnalAttribRow = MainDS.ATransAnalAttrib.NewRowTyped();

                                        DataUtilities.CopyAllColumnValues(originalTransAnalAttrib, newTransAnalAttribRow);
                                        newTransAnalAttribRow.BatchNumber = newTransactionRow.BatchNumber;
                                        newTransAnalAttribRow.JournalNumber = newTransactionRow.JournalNumber;
                                        newTransAnalAttribRow.TransactionNumber = newTransactionRow.TransactionNumber;
                                        MainDS.ATransAnalAttrib.Rows.Add(newTransAnalAttribRow);
                                    }
                                }
                            }

                            // Calculate the credit and debit totals
                            GLRoutines.UpdateBatchTotals(ref MainDS, ref newBatchRow);

                            GLBatchTDSAccess.SubmitChanges(MainDS, db);

                            ReversalBatchNumber = newBatchRow.BatchNumber;

                            // only post new batch if AAutoPostReverseBatch is true
                            if (!AAutoPostReverseBatch || PostGLBatch(ALedgerNumber, ReversalBatchNumber, out Verifications))
                            {
                                ReturnValue = true;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            AVerifications = Verifications;
            AReversalBatchNumber = ReversalBatchNumber;

            db.CloseDBConnection();

            return ReturnValue;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications,
            TDataBase ADataBase = null)
        {
            List <Int32>BatchNumbers = new List <int>();
            BatchNumbers.Add(ABatchNumber);

            return PostGLBatches(ALedgerNumber, BatchNumbers, out AVerifications, ADataBase);
        }

        /// <summary>
        /// post several GL Batches at once
        /// Returns true if it seems to be OK.
        /// </summary>
        public static bool PostGLBatches(Int32 ALedgerNumber,
            List <Int32>ABatchNumbers,
            out TVerificationResultCollection AVerifications,
            TDataBase ADataBase = null)
        {
            //Used in validation of arguments
            AVerifications = new TVerificationResultCollection();

            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting GL Batch",
                        "The Ledger number must be greater than 0!",
                        TResultSeverity.Resv_Critical));
                return false;
            }
            else if (ABatchNumbers.Count == 0)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting GL Batch",
                        "No GL Batches to post",
                        TResultSeverity.Resv_Noncritical));
                return false;
            }

            foreach (Int32 batchNumber in ABatchNumbers)
            {
                if (batchNumber <= 0)
                {
                    AVerifications.Add(
                        new TVerificationResult(
                            "Posting GL Batch",
                            "The Batch number must be greater than 0!",
                            TResultSeverity.Resv_Critical));
                    return false;
                }
            }

            #endregion Validate Arguments

            // TODO: get a lock on this ledger, no one else is allowed to change anything.
            //For use in transaction delegate
            TVerificationResultCollection VerificationResult = AVerifications;
            TVerificationResultCollection SingleVerificationResultCollection;

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("PostGLBatches", ADataBase);
            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Posting GL batches"),
                            ABatchNumbers.Count * 3 + 1);

                        SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

                        Int32 BatchPeriod = -1;

                        foreach (Int32 BatchNumber in ABatchNumbers)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Posting GL batches"),
                                ABatchNumbers.IndexOf(BatchNumber) * 3);

                            GLBatchTDS mainDS = null;

                            GLPostingTDS postingDS = PrepareGLBatchForPosting(out mainDS,
                                ALedgerNumber,
                                BatchNumber,
                                ref Transaction,
                                out SingleVerificationResultCollection,
                                PostingLevel,
                                ref BatchPeriod);

                            VerificationResult.AddCollection(SingleVerificationResultCollection);

                            if ((mainDS == null) || (postingDS == null))
                            {
                                return;
                            }

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Posting GL batches"),
                                ABatchNumbers.IndexOf(BatchNumber) * 3 + 1);

                            mainDS.ThrowAwayAfterSubmitChanges = true;
                            GLBatchTDSAccess.SubmitChanges(mainDS, db);

                            SummarizeInternal(ALedgerNumber, postingDS, PostingLevel, BatchPeriod, false, db); // No summarisation is performed, from April 2015, Tim Ingham

                            postingDS.ThrowAwayAfterSubmitChanges = true;
                            SubmitChanges(postingDS, db);
                        }  // foreach

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Posting GL batches"),
                            ABatchNumbers.Count * 3 - 1);

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                if (TDBExceptionHelper.IsTransactionSerialisationException(ex))
                {
                    VerificationResult = new TVerificationResultCollection();
                    VerificationResult.Add(new TVerificationResult("PostGLBatches",
                            ErrorCodeInventory.RetrieveErrCodeInfo(PetraErrorCodes.ERR_DB_SERIALIZATION_EXCEPTION)));
                }
                else
                {
                    TLogging.LogException(ex, Utilities.GetMethodSignature());
                    throw;
                }
            }
            finally
            {
                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }

            AVerifications = VerificationResult;

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            //
            // I previously used "Client Tasks" to ask the client to print the GL Posting Register,
            // but now I don't - the client needs to do that itself.

            /*
             * if (SubmissionOK == true)
             * {
             *  String ledgerName = TLedgerInfo.GetLedgerName(ALedgerNumber);
             *
             *  // Generate posting reports (on the client!)
             *  foreach (Int32 batchNumber in ABatchNumbers)
             *  {
             *      String[] Params =
             *      {
             *          "param_ledger_number_i=" + ALedgerNumber,
             *          "param_batch_number_i=" + batchNumber,
             *          "param_ledger_name=\"" + ledgerName + "\"",
             *          "param_sortby=\"Transaction\""
             *      };
             *
             *      String paramStr = String.Join(",", Params);
             *      PrintReportOnClientDelegate("Batch Posting Register", paramStr);
             *  }
             * }
             */
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            GLBatchTDS MainDS = new GLBatchTDS();

            SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

            try
            {
                APostingDS = PrepareGLBatchForPosting(out MainDS,
                    ALedgerNumber,
                    ABatchNumber,
                    ref ATransaction,
                    out AVerifications,
                    PostingLevel,
                    ref ABatchPeriod);

                if ((MainDS != null) && (APostingDS != null))
                {
                    SummarizeInternal(ALedgerNumber, APostingDS, PostingLevel, ABatchPeriod, false, ATransaction.DataBaseObj);
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
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
        public static GLPostingTDS PrepareGLBatchForPosting(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            ref TDBTransaction ATransaction,
            out TVerificationResultCollection AVerifications,
            SortedList <string, TAmount>APostingLevel,
            ref Int32 ABatchPeriod)
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: LoadData...");
            }

            AVerifications = new TVerificationResultCollection();

            GLPostingTDS PostingDS = LoadGLDataForPosting(ALedgerNumber, ATransaction.DataBaseObj);

            // get the data from the database into the MainDS
            AMainDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, ref ATransaction, ref AVerifications);

            ALedgerRow LedgerRow = AMainDS.ALedger[0];
            string LedgerBaseCurrency = LedgerRow.BaseCurrency;
            string LedgerIntlCurrency = LedgerRow.IntlCurrency;

            ABatchRow BatchToPostRow = AMainDS.ABatch[0];

            if (BatchToPostRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
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

            if (ABatchPeriod == -1)
            {
                ABatchPeriod = BatchToPostRow.BatchPeriod;
            }
            else if (ABatchPeriod != BatchToPostRow.BatchPeriod)
            {
                AVerifications.Add(new TVerificationResult(
                        Catalog.GetString("Cannot post Batches from different periods at once!"),
                        Catalog.GetString("Batches from more than one period."),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            DateTime EffectiveDate = BatchToPostRow.DateEffective;
            DateTime StartOfCalendarMonth = new DateTime(EffectiveDate.Year, EffectiveDate.Month, 1);

            // used for setting AmountInIntlCurrency
            decimal IntlToBaseExchRate;
            TExchangeRateTools.GetCorporateExchangeRate(
                LedgerIntlCurrency, LedgerBaseCurrency,
                StartOfCalendarMonth,
                EffectiveDate,
                out IntlToBaseExchRate,
                ATransaction.DataBaseObj);

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            if (!ValidateGLBatchAndTransactions(ref AMainDS, PostingDS, ALedgerNumber, BatchToPostRow, out AVerifications, ATransaction.DataBaseObj))
            {
                return null;
            }
            else if (IntlToBaseExchRate == 0)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting GL Batch",
                        String.Format(Catalog.GetString("No Corporate Exchange rate exists for the month: {0:MMMM yyyy}!"),
                            EffectiveDate),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            // set the amount in intl currency for each transaction
            foreach (AJournalRow journalRow in AMainDS.AJournal.Rows)
            {
                string batchTransactionCurrency = journalRow.TransactionCurrency;

                foreach (ATransactionRow transRow in AMainDS.ATransaction.Rows)
                {
                    if (transRow.JournalNumber == journalRow.JournalNumber)
                    {
                        if (batchTransactionCurrency != LedgerIntlCurrency)
                        {
                            transRow.AmountInIntlCurrency = GLRoutines.CurrencyMultiply(transRow.AmountInBaseCurrency, IntlToBaseExchRate);
                        }
                        else
                        {
                            transRow.AmountInIntlCurrency = transRow.TransactionAmount;
                        }
                    }
                }
            }

            if (!PostingDS.ALedger[0].ProvisionalYearEndFlag) // During YearEnd Processing, I don't require all the attributes correctly fulfiled.
            {
                if (!ValidateAnalysisAttributes(ref AMainDS, PostingDS, ALedgerNumber, ABatchNumber, out AVerifications))
                {
                    return null;
                }
            }

            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Load GLM Data...");

            LoadGLMData(ref PostingDS, ALedgerNumber, BatchToPostRow, ATransaction.DataBaseObj);

            TLogging.LogAtLevel(POSTING_LOGLEVEL, "Posting: Mark as posted and collect data...");

            // post each journal, each transaction; add sums for costcentre/account combinations
            MarkAsPostedAndCollectData(AMainDS, PostingDS, APostingLevel, BatchToPostRow);

            // if posting goes wrong later, the transation will be rolled back
            return PostingDS;
        }

        /// <summary>
        /// Tell me whether this Batch can be cancelled
        /// </summary>
        public static bool GLBatchCanBeCancelled(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            TDataBase ADataBase = null)
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            bool RetVal = false;
            string BatchStatus = string.Empty;

            AMainDS = LoadGLBatchData(ALedgerNumber, ABatchNumber, out AVerifications, ADataBase);

            // get the data from the database into the MainDS
            ABatchRow Batch = AMainDS.ABatch[0];

            BatchStatus = Batch.BatchStatus;

            if (BatchStatus == MFinanceConstants.BATCH_CANCELLED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It was already cancelled.")),
                        TResultSeverity.Resv_Critical));
            }
            else if (BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                //Only if reaches here it can be deleted
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Cancel this batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool CancelGLBatch(
            Int32 ALedgerNumber,
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS;
            if (TGLPosting.GLBatchCanBeCancelled(out MainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                ABatchRow Batch = MainDS.ABatch[0];
                Batch.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                GLBatchTDSAccess.SubmitChanges(MainDS.GetChangesTyped(true));
                return true;
            }

            return false;
        }

        /// <summary>
        /// If a Batch has been created then found to be not required, it can be deleted here.
        /// (This was added for ICH and Stewardship calculations, which can otherwise leave empty batches in the ledger.)
        /// </summary>
        /// <returns></returns>
        public static bool DeleteGLBatch(
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            TDataBase ADataBase)
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS;

            try
            {
                if (!GLBatchCanBeCancelled(out MainDS, ALedgerNumber, ABatchNumber, out AVerifications, ADataBase))
                {
                    return false;
                }
                else
                {
                    ABatchRow batchRow = MainDS.ABatch[0];

                    //
                    // If I'm deleting the most recent entry (which is almost certainly the case)
                    // I can wind back the Ledger's LastBatchNumber so as not to leave a gap.
                    //
                    if (batchRow.BatchNumber == MainDS.ALedger[0].LastBatchNumber)
                    {
                        MainDS.ALedger[0].LastBatchNumber--;
                    }

                    batchRow.Delete();
                    //
                    // If this batch has journals and transactions, they need to be deleted too,
                    // along with any trans_anal_attrib records.
                    //
                    // The call to GLBatchCanBeCancelled will have loaded all these records for me.

                    //using Rows.Clear doesn't convey "delete these rows" back to the database, which is what we need:
                    foreach (DataRow row in MainDS.AJournal.Rows)
                    {
                        row.Delete();
                    }

                    foreach (DataRow row in MainDS.ATransaction.Rows)
                    {
                        row.Delete();
                    }

                    foreach (DataRow row in MainDS.ATransAnalAttrib.Rows)
                    {
                        row.Delete();
                    }

                    GLBatchTDSAccess.SubmitChanges(MainDS, ADataBase);
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (AVerifications.HasCriticalErrors)
            {
                throw new EVerificationResultsException(String.Format(Catalog.GetString(
                            "Function:{0} - Error trying to delete GL Batch {1} in Ledger {2}!"),
                        Utilities.GetMethodName(true), ABatchNumber, ALedgerNumber),
                    AVerifications);
            }

            return true;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber, TDataBase ADataBase, Boolean AWithGLJournal = false)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("CreateABatch", ADataBase);

            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        GLBatchTDSABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                        NewRow.LedgerNumber = ALedgerNumber;
                        MainDS.ALedger[0].LastBatchNumber++;
                        NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                        NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                        NewRow.BatchYear = MainDS.ALedger[0].CurrentFinancialYear;
                        NewRow.TransactionCurrency = MainDS.ALedger[0].BaseCurrency;

                        if (AWithGLJournal)
                        {
                            AJournalRow NewJournal = MainDS.AJournal.NewRowTyped(true);
                            NewJournal.LedgerNumber = ALedgerNumber;
                            NewJournal.BatchNumber = NewRow.BatchNumber;
                            NewJournal.JournalNumber = 1;
                            NewRow.LastJournal = 1;
                            NewJournal.JournalDescription = "Journal 1";
                            NewJournal.SubSystemCode = "GL";
                            NewJournal.TransactionCurrency = MainDS.ALedger[0].BaseCurrency;
                            NewJournal.BaseCurrency = MainDS.ALedger[0].BaseCurrency;
                            MainDS.AJournal.Rows.Add(NewJournal);
                        }

                        MainDS.ABatch.Rows.Add(NewRow);

                        GLBatchTDSAccess.SubmitChanges(MainDS, db);
                        SubmissionOK = true;
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return MainDS;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        /// <returns></returns>
        public static GLBatchTDS CreateABatch(
            Int32 ALedgerNumber,
            string ABatchDescription,
            decimal ABatchControlTotal,
            DateTime ADateEffective,
            TDataBase ADataBase = null)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("CreateABatch", ADataBase);
            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

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

                        GLBatchTDSAccess.SubmitChanges(MainDS, db);

                        SubmissionOK = true;
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return MainDS;
        }

        /// <summary>
        /// create a new recurring batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateARecurringBatch(Int32 ALedgerNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();
            GLBatchTDS TempDS = new GLBatchTDS();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("CreateARecurringBatch");
            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        ARecurringBatchAccess.LoadViaALedger(TempDS, ALedgerNumber, Transaction);

                        DataView RecurringBatchDV = new DataView(TempDS.ARecurringBatch);
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

                        //Empty the TempDS
                        TempDS.RejectChanges();

                        //Submit changes to MainDS
                        GLBatchTDSAccess.SubmitChanges(MainDS, db);

                        SubmissionOK = true;
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            db.CloseDBConnection();

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
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((AMainDS.ABatch == null) || (AMainDS.ABatch.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch table does not exist or is empty!"),
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (ACurrency.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Currency code is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AXRateToBase <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Exchange Rate to base must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool CreationSuccessful = false;

            AJournalNumber = 0;

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

                #region Validate Data

                if (BatchRow == null)
                {
                    throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                                "Function:{0} - Batch row data for GL Batch {1} from Ledger {2} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ABatchNumber,
                            ALedgerNumber));
                }

                #endregion Validate Data

                BatchRow.LastJournal = AJournalNumber;

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
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
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
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
            else if ((ABaseAmount != 0) && (Math.Sign(ATransAmount) != Math.Sign(ABaseAmount)))
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The Transaction amount is a different sign to the base currency amount!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool CreationSuccessful = false;

            ATransactionNumber = 0;

            try
            {
                AJournalRow JournalRow = (AJournalRow)AMainDS.AJournal.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber });

                #region Validate Data

                if (JournalRow == null)
                {
                    throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                                "Function:{0} - Journal row data for GL Journal {1} in Batch {2} from Ledger {3} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            AJournalNumber,
                            ABatchNumber,
                            ALedgerNumber));
                }

                #endregion Validate Data

                //Increment the LastTransactionNumber
                ATransactionNumber = ++JournalRow.LastTransactionNumber;

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
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return CreationSuccessful;
        }

        /// Helper class for storing the amounts of a batch at posting level for account/costcentre combinations
        public class TAmount
        {
            /// Amount in the base currency of the ledger
            public decimal BaseAmount = 0.0M;

            /// Amount in the intl currency of the ledger
            public decimal IntlAmount = 0.0M;

            /// Amount in transaction currency; only for foreign currency accounts
            public decimal TransAmount = 0.0M;

            /// Generate a key for the account/costcentre combination
            public static string MakeKey(string AAccountCode, string ACostCentreCode)
            {
                return AAccountCode + ":" + ACostCentreCode;
            }

            /// <summary>
            /// Get the account code from the key
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static string GetAccountCode(string AKey)
            {
                return AKey.Split(':')[0];
            }

            /// <summary>
            /// Get the cost centre code from the key
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static string GetCostCentreCode(string AKey)
            {
                return AKey.Split(':')[1];
            }
        }

        /// Helper class for managing the account hierarchy for posting the batch
        private class TAccountTreeElement
        {
            /// Is the debit credit indicator different of the reporting account to the parent account
            public bool Invert = false;

            /// Is this account a foreign currency account
            public bool Foreign = false;

            /// Constructor
            public TAccountTreeElement(bool AInvert, bool AForeign)
            {
                Invert = AInvert;
                Foreign = AForeign;
            }

            /// <summary>
            /// Generate a key for the reporting account/parent account combination
            /// </summary>
            /// <param name="AReportingAccountCode"></param>
            /// <param name="AAccountCodeReportTo"></param>
            /// <returns></returns>
            public static string MakeKey(string AReportingAccountCode, string AAccountCodeReportTo)
            {
                return AReportingAccountCode + ":" + AAccountCodeReportTo;
            }

            /// <summary>
            /// Get the reporting account code from the key
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static string GetReportingAccountCode(string AKey)
            {
                return AKey.Split(':')[0];
            }

            /// <summary>
            /// Get the parent account code from the key
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static string GetAccountReportToCode(string AKey)
            {
                return AKey.Split(':')[1];
            }
        }
    }
}
