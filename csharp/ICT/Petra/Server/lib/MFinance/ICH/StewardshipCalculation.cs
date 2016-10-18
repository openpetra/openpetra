//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert, timop
//
// Copyright 2004-2014 by OM International
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
using System.Runtime.Serialization;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.App.Core.Security;
using System.Collections.Generic;

namespace Ict.Petra.Server.MFinance.ICH.WebConnectors
{
    /// <summary>
    /// Class for the performance of the Stewardship Calculation
    /// </summary>
    public class TStewardshipCalculationWebConnector
    {
        /// <summary>
        /// Performs the ICH Stewardship Calculation.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AgeneratedBatches">The Client should print these batches.</param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if calculation succeeded, otherwise false.</returns>
        [RequireModulePermission("FINANCE-2")]
        public static bool PerformStewardshipCalculation(int ALedgerNumber,
            int APeriodNumber,
            out List <Int32>AgeneratedBatches,
            out TVerificationResultCollection AVerificationResult)
        {
            /*
             *          if (TLogging.DL >= 9)
             *          {
             *              Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation...");
             *          }
             */
            bool NoRecordsToProcess = false;

            List <Int32>batches = new List <Int32>();
            AgeneratedBatches = batches;
            AVerificationResult = new TVerificationResultCollection();
            TVerificationResultCollection VerificationResult = AVerificationResult;

            TDBTransaction DBTransaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                    ref DBTransaction,
                    ref SubmissionOK,
                    delegate
                    {
                        Int32 glBatchNum;

                        if (GenerateAdminFeeBatch(
                                ALedgerNumber,
                                APeriodNumber,
                                false,
                                DBTransaction,
                                out glBatchNum,
                                ref VerificationResult))
                        {
                            batches.Add(glBatchNum);

                            SubmissionOK =
                                GenerateICHStewardshipBatch(
                                    ALedgerNumber,
                                    APeriodNumber,
                                    DBTransaction,
                                    out glBatchNum,
                                    ref VerificationResult,
                                    out NoRecordsToProcess);

                            if (SubmissionOK)
                            {
                                batches.Add(glBatchNum);
                            }

                            //If Generate AdminFeeBatch was successful but no records to process in the Stewardship Batch, then still commit
                            SubmissionOK |= NoRecordsToProcess;
                        }
                    });

                AVerificationResult = VerificationResult;
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return SubmissionOK;
        }

        /// <summary>
        /// Reads from the table holding all the fees charged for this month and generates a GL batch from it.
        /// Relates to gl2150.p
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APrintReport"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AglBatchNumber">This Batch Number should be conveyed to the client for printing.</param>
        /// <param name="AVerificationResults"></param>
        /// <returns>true, unless PostGlBatch fails</returns>

        private static bool GenerateAdminFeeBatch(int ALedgerNumber,
            int APeriodNumber,
            bool APrintReport,
            TDBTransaction ADBTransaction,
            out Int32 AglBatchNumber,
            ref TVerificationResultCollection AVerificationResults
            )
        {
            bool IsSuccessful = false;

            AglBatchNumber = -1;
            bool feeTransactionCreated = false;
            string DrAccountCode;
            string DestCostCentreCode = string.Empty;
            string DestAccountCode = string.Empty;
            string FeeDescription = string.Empty;
            decimal DrFeeTotal = 0;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            if (AVerificationResults == null)
            {
                AVerificationResults = new TVerificationResultCollection();
            }

            /* Make a temporary table to hold totals for gifts going to each account.
             */
            GLStewardshipCalculationTDSCreditFeeTotalTable CreditFeeTotalDT = new GLStewardshipCalculationTDSCreditFeeTotalTable();

            /* Retrieve info on the ledger. */
            ALedgerTable AvailableLedger = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)AvailableLedger.Rows[0];

            try
            {
                /* Check that we have not closed all periods for the year yet.
                 *  (Not at the provisional year end point)
                 */
                if (LedgerRow.ProvisionalYearEndFlag)
                {
                    //Petra ErrorCode = GL0071
                    ErrorContext = Catalog.GetString("Generate Admin Fee Batch");
                    ErrorMessage = String.Format(Catalog.GetString(
                            "Cannot progress as Ledger {0} is at the provisional year-end point"), ALedgerNumber);
                    ErrorType = TResultSeverity.Resv_Critical;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                ACurrencyTable currencyInfo = ACurrencyAccess.LoadByPrimaryKey(LedgerRow.BaseCurrency, ADBTransaction);
                ACurrencyRow currencyRow = (ACurrencyRow)currencyInfo.Rows[0];
                string numericFormat = currencyRow.DisplayFormat;
                int numDecPlaces = THelperNumeric.CalcNumericFormatDecimalPlaces(numericFormat);

                /* Create the journal for fee transactions in, if there are fees to charge.
                 * NOTE: if the date in the processed fee table is null then that fee hasn't been processed. */

                AProcessedFeeTable processedFeeDataTable = new AProcessedFeeTable();

                string sqlStmt = String.Format("SELECT * FROM {0} WHERE {1}={2} AND {3}={4} AND {5} IS NULL AND {6}<>0 ORDER BY {7}, {8}",
                    AProcessedFeeTable.GetTableDBName(),
                    AProcessedFeeTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AProcessedFeeTable.GetPeriodNumberDBName(),
                    APeriodNumber,
                    AProcessedFeeTable.GetProcessedDateDBName(),
                    AProcessedFeeTable.GetPeriodicAmountDBName(),
                    AProcessedFeeTable.GetFeeCodeDBName(),
                    AProcessedFeeTable.GetCostCentreCodeDBName()
                    );

                DBAccess.GDBAccessObj.SelectDT(processedFeeDataTable, sqlStmt, ADBTransaction);

                if (processedFeeDataTable.Count == 0)
                {
                    if (TLogging.DebugLevel > 0)
                    {
                        TLogging.Log("No fees to charge were found");
                        AVerificationResults.Add(new TVerificationResult(Catalog.GetString("Admin Fee Batch"),
                                String.Format(Catalog.GetString("No admin fees charged in period {0}."), APeriodNumber),
                                TResultSeverity.Resv_Status));
                    }

                    return true;
                }

                AAccountingPeriodTable accountingPeriodTable =
                    AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, ADBTransaction);
                AAccountingPeriodRow accountingPeriodRow = (AAccountingPeriodRow)accountingPeriodTable.Rows[0];

                // Create a Batch and journal. If no fees are to be charged, I'll delete this batch later.
                GLBatchTDS adminFeeDS = TGLPosting.CreateABatch(ALedgerNumber, Catalog.GetString(
                        "Admin Fees & Grants"), 0, accountingPeriodRow.PeriodEndDate);

                ABatchRow batchRow = adminFeeDS.ABatch[0];

                AJournalRow journalRow = adminFeeDS.AJournal.NewRowTyped();
                journalRow.LedgerNumber = ALedgerNumber;
                journalRow.BatchNumber = batchRow.BatchNumber;
                journalRow.JournalNumber = ++batchRow.LastJournal;

                journalRow.JournalDescription = batchRow.BatchDescription;
                journalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                journalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                journalRow.TransactionCurrency = LedgerRow.BaseCurrency;
                journalRow.ExchangeRateToBase = 1;
                journalRow.DateEffective = accountingPeriodRow.PeriodEndDate;
                journalRow.JournalPeriod = APeriodNumber;
                adminFeeDS.AJournal.Rows.Add(journalRow);

                /*
                 * Generate Admin Fee transactions.
                 * Only one transaction is posted for each fee code/cost centre.
                 */
                int gLJournalNumber = journalRow.JournalNumber;
                string currentFeeCode = string.Empty;
                string costCentreCodeDBName = AProcessedFeeTable.GetCostCentreCodeDBName();
                bool transactionOK;

                for (int i = 0; i < processedFeeDataTable.Count; i++)
                {
                    AProcessedFeeRow feeRow = (AProcessedFeeRow)processedFeeDataTable.Rows[i];

                    if (currentFeeCode != feeRow.FeeCode)
                    {
                        currentFeeCode = feeRow.FeeCode;

                        AFeesPayableTable feesPayableTable = AFeesPayableAccess.LoadByPrimaryKey(ALedgerNumber, currentFeeCode, ADBTransaction);

                        if (feesPayableTable.Count > 0)
                        {
                            AFeesPayableRow feesPayableRow = (AFeesPayableRow)feesPayableTable.Rows[0];
                            DrAccountCode = feesPayableRow.DrAccountCode;
                            DestCostCentreCode = feesPayableRow.CostCentreCode;
                            DestAccountCode = feesPayableRow.AccountCode;
                            FeeDescription = feesPayableRow.FeeDescription;
                        }
                        else  // Try receivables instead
                        {
                            AFeesReceivableTable feesReceivableTable = AFeesReceivableAccess.LoadByPrimaryKey(ALedgerNumber,
                                currentFeeCode,
                                ADBTransaction);

                            if (feesReceivableTable.Count > 0)
                            {
                                AFeesReceivableRow feesReceivableRow = (AFeesReceivableRow)feesReceivableTable.Rows[0];
                                DrAccountCode = feesReceivableRow.DrAccountCode;
                                DestCostCentreCode = feesReceivableRow.CostCentreCode;
                                DestAccountCode = feesReceivableRow.AccountCode;
                                FeeDescription = feesReceivableRow.FeeDescription;
                            }
                            else
                            {
                                ErrorMessage = String.Format(
                                    Catalog.GetString(
                                        "Unable to access information for Fee Code '{1}' in either the Fees Payable or Fees Receivable Tables for Ledger {0}"),
                                    ALedgerNumber, currentFeeCode);

                                throw new Exception(ErrorMessage);
                            }
                        }

                        DrFeeTotal = 0;

                        //Get all the distinct CostCentres
                        DataView costCentreDV = processedFeeDataTable.DefaultView;
                        costCentreDV.Sort = costCentreCodeDBName;
                        costCentreDV.RowFilter = string.Format("{0} = '{1}'", AProcessedFeeTable.GetFeeCodeDBName(), currentFeeCode);
                        DataTable processedFeeCostCentresTable = costCentreDV.ToTable(true, costCentreCodeDBName);

                        foreach (DataRow r in processedFeeCostCentresTable.Rows)
                        {
                            String costCentreCode = r[0].ToString();
                            DataView view = processedFeeDataTable.DefaultView;
                            view.Sort = costCentreCodeDBName;
                            view.RowFilter = string.Format("{0}='{1}' And {2}='{3}'",
                                AProcessedFeeTable.GetFeeCodeDBName(),
                                currentFeeCode,
                                costCentreCodeDBName,
                                costCentreCode);

                            DataTable processedFeeDataTable2 = view.ToTable();

                            Int32 feeCodeRowCount = processedFeeDataTable2.Rows.Count;

                            for (int j = 0; j < feeCodeRowCount; j++)
                            {
                                DataRow pFR2 = processedFeeDataTable2.Rows[j];

                                DrFeeTotal = DrFeeTotal + Math.Round(Convert.ToDecimal(
                                        pFR2[AProcessedFeeTable.GetPeriodicAmountDBName()]), numDecPlaces);

                                if (j == (feeCodeRowCount - 1))   // The last CostCentre row for this feecode?
                                {
                                    if (DrFeeTotal != 0)
                                    {
                                        Boolean DrCrIndicator = true; // Debit?

                                        if (DrFeeTotal < 0)
                                        {
                                            DrCrIndicator = false;  // Credit
                                            DrFeeTotal = -DrFeeTotal;
                                        }

                                        /*
                                         * Generate the transaction to deduct the fee amount from the source cost centre. (Expense leg)
                                         */
                                        //RUN gl1130o.p -> gl1130.i
                                        //Create a transaction
                                        Int32 gLTransactionNumber = 0;

                                        if (DrFeeTotal != 0)
                                        {
                                            transactionOK = TGLPosting.CreateATransaction(adminFeeDS,
                                                ALedgerNumber,
                                                batchRow.BatchNumber,
                                                gLJournalNumber,
                                                "Fee: " + FeeDescription + " (" + currentFeeCode + ")",
                                                DrAccountCode,
                                                costCentreCode,
                                                DrFeeTotal,
                                                accountingPeriodRow.PeriodEndDate,
                                                DrCrIndicator,
                                                "AG",
                                                true,
                                                DrFeeTotal,
                                                out gLTransactionNumber);

                                            if (transactionOK)
                                            {
                                                feeTransactionCreated |= transactionOK;
                                            }
                                            else
                                            {
                                                ErrorContext = Catalog.GetString("Generating the Admin Fee batch");
                                                ErrorMessage =
                                                    String.Format(Catalog.GetString(
                                                            "Unable to create a new expense transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                                        ALedgerNumber,
                                                        batchRow.BatchNumber,
                                                        gLJournalNumber);
                                                ErrorType = TResultSeverity.Resv_Noncritical;

                                                AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                                                return false;
                                            }
                                        }

                                        DrFeeTotal = 0;
                                    }
                                }
                            }
                        }
                    }

                    /* Mark each fee entry as processed. */
                    feeRow.ProcessedDate = DateTime.Today.Date;
                    feeRow.Timestamp =
                        (DateTime.Today.TimeOfDay.Hours * 3600 + DateTime.Today.TimeOfDay.Minutes * 60 + DateTime.Today.TimeOfDay.Seconds);

                    /* Add the charges on this account to the fee total,
                     * creating an entry if necessary. (This is for the income total)
                     */
                    GLStewardshipCalculationTDSCreditFeeTotalRow creditFeeTotalRow = (GLStewardshipCalculationTDSCreditFeeTotalRow)
                                                                                     CreditFeeTotalDT.Rows.Find(new object[] { DestCostCentreCode,
                                                                                                                               DestAccountCode });

                    if (creditFeeTotalRow != null)
                    {
                        creditFeeTotalRow.TransactionAmount += Math.Round(feeRow.PeriodicAmount, numDecPlaces);
                    }
                    else
                    {
                        creditFeeTotalRow = CreditFeeTotalDT.NewRowTyped();
                        creditFeeTotalRow.CostCentreCode = DestCostCentreCode;
                        creditFeeTotalRow.AccountCode = DestAccountCode;
                        creditFeeTotalRow.TransactionAmount = Math.Round(feeRow.PeriodicAmount, numDecPlaces);
                        CreditFeeTotalDT.Rows.Add(creditFeeTotalRow);
                    }
                }

                /* Generate the transaction to credit the fee amounts to
                 * the destination accounts. (Income leg)
                 */
                for (int k = 0; k < CreditFeeTotalDT.Count; k++)
                {
                    GLStewardshipCalculationTDSCreditFeeTotalRow cFT = (GLStewardshipCalculationTDSCreditFeeTotalRow)
                                                                       CreditFeeTotalDT.Rows[k];
                    Boolean DrCrIndicator = false; // Credit?

                    decimal transactionAmount = cFT.TransactionAmount;

                    if (transactionAmount < 0)
                    {
                        /* The case of a negative gift total should be very rare.
                         * It would only happen if, for instance, the was only
                         * a reversal but no new gifts for a certain ledger.
                         */
                        DrCrIndicator = true; //Debit
                        transactionAmount = -transactionAmount;
                    }

                    if (transactionAmount != 0)
                    {
                        Int32 gLTransactionNumber = 0;
                        transactionOK = TGLPosting.CreateATransaction(adminFeeDS,
                            ALedgerNumber,
                            batchRow.BatchNumber,
                            journalRow.JournalNumber,
                            "Collected admin charges",
                            cFT.AccountCode,
                            cFT.CostCentreCode,
                            transactionAmount,
                            accountingPeriodRow.PeriodEndDate,
                            DrCrIndicator,
                            "AG",
                            true,
                            transactionAmount,
                            out gLTransactionNumber);

                        if (transactionOK)
                        {
                            feeTransactionCreated |= transactionOK;
                        }
                        else
                        {
                            ErrorContext = Catalog.GetString("Generating the Admin Fee batch");
                            ErrorMessage =
                                String.Format(Catalog.GetString(
                                        "Unable to create a new income transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    batchRow.BatchNumber,
                                    journalRow.JournalNumber);
                            ErrorType = TResultSeverity.Resv_Noncritical;

                            AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                            return false;
                        }
                    }
                } // for

                TVerificationResultCollection verification = null;

                if (!feeTransactionCreated)
                {
                    AVerificationResults.Add(new TVerificationResult(Catalog.GetString("Admin Fee Batch"),
                            String.Format(Catalog.GetString("No admin fees charged in period ({0})."), APeriodNumber),
                            TResultSeverity.Resv_Status));

                    // An empty GL Batch now exists, which I need to delete.
                    TVerificationResultCollection batchCancelResult = new TVerificationResultCollection();

                    TGLPosting.DeleteGLBatch(
                        ALedgerNumber,
                        batchRow.BatchNumber,
                        out batchCancelResult);

                    AVerificationResults.AddCollection(batchCancelResult);
                }
                else
                {
                    //Post the batch just created
                    GLBatchTDSAccess.SubmitChanges(adminFeeDS);

                    IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, batchRow.BatchNumber, out verification);

                    if (IsSuccessful)
                    {
                        AglBatchNumber = batchRow.BatchNumber;
                        AProcessedFeeAccess.SubmitChanges(processedFeeDataTable, ADBTransaction);
                    }
                }

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(verification))
                {
                    ErrorContext = Catalog.GetString("Posting Admin Fee Batch");
                    ErrorMessage = String.Format(Catalog.GetString("The posting of the admin fee batch failed."));
                    ErrorType = TResultSeverity.Resv_Noncritical;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                /* Print the Admin Fee Calculations report, if requested */
                if (APrintReport && IsSuccessful)
                {
                    //TODO
                }
            } // try
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return IsSuccessful;
        } // Generate AdminFeeBatch

        /// <summary>
        ///  Check that there are amounts to be transferred to ICH.
        ///  I.e., there are debits or credits to foreign cost centres.
        ///  Create the journal to create the transfer transactions in,
        ///  if there are amounts to transfer.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AglBatchNumber"></param>
        /// <param name="AVerificationResults"></param>
        /// <param name="ANoRecordsToProcess"></param>
        /// <returns>True if successful</returns>
        [RequireModulePermission("FINANCE-2")]
        private static bool GenerateICHStewardshipBatch(int ALedgerNumber,
            int APeriodNumber,
            TDBTransaction ADBTransaction,
            out Int32 AglBatchNumber,
            ref TVerificationResultCollection AVerificationResults,
            out bool ANoRecordsToProcess)
        {
            ANoRecordsToProcess = false;
            bool IsSuccessful = false;
            AglBatchNumber = -1;
            //Verification results handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                bool drCrIndicator = true;
                bool incomeDrCrIndicator;
                bool expenseDrCrIndicator;

                string incomeAccounts = string.Empty;
                string expenseAccounts = string.Empty;

                string standardCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);

                int currentFinancialYear = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction)[0].CurrentFinancialYear;
                DateTime periodStartDate;
                DateTime periodEndDate;
                TFinancialYear.GetStartAndEndDateOfPeriod(ALedgerNumber, APeriodNumber, out periodStartDate, out periodEndDate, ADBTransaction);

                string periodStartDateSQL = "#" + periodStartDate.ToString("yyyy-MM-dd") + "#";
                string periodEndDateSQL = "#" + periodEndDate.ToString("yyyy-MM-dd") + "#";

                // ***************************
                // * Generating the ICH batch
                // ***************************
                AGiftBatchTable giftBatchTable = new AGiftBatchTable();

                string giftQuery = "SELECT *" +
                                   " FROM " + AGiftBatchTable.GetTableDBName() +
                                   " WHERE " + AGiftBatchTable.GetLedgerNumberDBName() + " = " + ALedgerNumber +
                                   "   AND " + AGiftBatchTable.GetBatchStatusDBName() + " = '" + MFinanceConstants.BATCH_POSTED + "'" +
                                   "   AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " >= " + periodStartDateSQL +
                                   "   AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= " + periodEndDateSQL +
                                   " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName();

                DBAccess.GDBAccessObj.SelectDT(giftBatchTable, giftQuery, ADBTransaction);

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                GLPostingTDS postingDS = new GLPostingTDS();
                ALedgerAccess.LoadByPrimaryKey(postingDS, ALedgerNumber, ADBTransaction);
                AAccountAccess.LoadViaALedger(postingDS, ALedgerNumber, ADBTransaction);
                AIchStewardshipAccess.LoadViaALedger(postingDS, ALedgerNumber, ADBTransaction);
                AAccountHierarchyAccess.LoadViaALedger(postingDS, ALedgerNumber, ADBTransaction);

                ABatchTable batchTable = new ABatchTable();
                ABatchRow batchTemplateRow = (ABatchRow)batchTable.NewRowTyped(false);

                batchTemplateRow.LedgerNumber = ALedgerNumber;
                batchTemplateRow.BatchPeriod = APeriodNumber;
                batchTemplateRow.BatchYear = currentFinancialYear;
                batchTemplateRow.BatchStatus = MFinanceConstants.BATCH_POSTED;

                StringCollection operators0 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
                StringCollection orderList0 = new StringCollection();

                orderList0.Add("ORDER BY");
                orderList0.Add(ABatchTable.GetBatchNumberDBName() + " DESC");

                ABatchTable batchesInAPeriod = ABatchAccess.LoadUsingTemplate(batchTemplateRow,
                    operators0,
                    null,
                    ADBTransaction,
                    orderList0,
                    0,
                    0);

                if ((batchesInAPeriod == null) || (batchesInAPeriod.Rows.Count == 0))
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("No Batches found to process in Ledger: {0}"),
                            ALedgerNumber);
                    ErrorType = TResultSeverity.Resv_Noncritical;

                    //Even though no batches found, not an error situation
                    ANoRecordsToProcess = true;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                //Create a new batch. If it turns out I don't need one, I can delete it later.
                GLBatchTDS mainDS = TGLPosting.CreateABatch(ALedgerNumber, Catalog.GetString("ICH Stewardship"), 0, periodEndDate, ADBTransaction);

                //Load the journal and transaction data
                int batchNumber = 0;

                for (int i = 0; i < batchesInAPeriod.Count; i++)
                {
                    ABatchRow batchRow = (ABatchRow)batchesInAPeriod.Rows[i];

                    batchNumber = batchRow.BatchNumber;

                    AJournalAccess.LoadViaABatch(mainDS, ALedgerNumber, batchNumber, ADBTransaction);
                    ATransactionAccess.LoadViaABatch(mainDS, ALedgerNumber, batchNumber, ADBTransaction);
                }

                ABatchRow newBatchRow = mainDS.ABatch[0];
                AglBatchNumber = newBatchRow.BatchNumber;

                ALedgerRow ledgerRow = (ALedgerRow)mainDS.ALedger.Rows[0];

                //Create a new journal in the Batch
                AJournalRow newJournalRow = mainDS.AJournal.NewRowTyped();
                newJournalRow.LedgerNumber = ALedgerNumber;
                newJournalRow.BatchNumber = AglBatchNumber;
                newJournalRow.JournalNumber = ++newBatchRow.LastJournal;
                newJournalRow.JournalDescription = newBatchRow.BatchDescription;
                newJournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                newJournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                newJournalRow.TransactionCurrency = ledgerRow.BaseCurrency;
                newJournalRow.ExchangeRateToBase = 1;
                newJournalRow.DateEffective = periodEndDate;
                newJournalRow.JournalPeriod = APeriodNumber;
                mainDS.AJournal.Rows.Add(newJournalRow);

                int gLJournalNumber = newJournalRow.JournalNumber;
                int gLTransactionNumber = newJournalRow.LastTransactionNumber + 1;


                // ***************************
                // * Generate the transactions
                // ***************************
                AAccountRow accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.INCOME_HEADING });

                //Process income accounts
                if (accountRow != null)
                {
                    incomeDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Income Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.INCOME_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                BuildChildAccountList(ALedgerNumber,
                    accountRow,
                    ADBTransaction,
                    ref incomeAccounts);

                //Process expense accounts
                accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.EXPENSE_HEADING });

                if (accountRow != null)
                {
                    expenseDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Expense Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.EXPENSE_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                BuildChildAccountList(ALedgerNumber,
                    accountRow,
                    ADBTransaction,
                    ref expenseAccounts);

                //Process P&L accounts
                accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.PROFIT_AND_LOSS_HEADING });
                bool PlAccountDrCrIndicator;

                if (accountRow != null)
                {
                    PlAccountDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Profit & Loss Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.PROFIT_AND_LOSS_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;

                    AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                    return false;
                }

                // Increment the ICH stewardship number in the Ledger.
                int iCHProcessing = ++ledgerRow.LastIchNumber;
                decimal iCHTotal = 0;

                ACostCentreRow cCTemplateRow = postingDS.ACostCentre.NewRowTyped(false);
                cCTemplateRow.LedgerNumber = ALedgerNumber;
                cCTemplateRow.PostingCostCentreFlag = true;
                cCTemplateRow.CostCentreType = MFinanceConstants.FOREIGN_CC_TYPE;

                ACostCentreAccess.LoadUsingTemplate(postingDS, cCTemplateRow, ADBTransaction);

                //Iterate through the cost centres
                AIchStewardshipTable iCHStewardshipTable = new AIchStewardshipTable();
                Boolean ICHTransactionsIncluded = false;
                Boolean nonICHTransactionsIncluded = false;

                foreach (ACostCentreRow costCentreRow in postingDS.ACostCentre.Rows)
                {
                    // To do this work I will need to have a clearing account specified:
                    if (costCentreRow.ClearingAccount == "")
                    {
                        AVerificationResults.Add(new TVerificationResult("Generate ICH Stewardship Batch",
                                String.Format(Catalog.GetString("Fault: Cost Centre {0} ({1}) has no Clearing Account Specified."),
                                    costCentreRow.CostCentreCode, costCentreRow.CostCentreName),
                                TResultSeverity.Resv_Critical));
                        continue; // The batch is doomed, but I'll continue to look for more faults.
                    }

                    //Initialise values for each Cost Centre
                    decimal settlementAmount = 0;
                    decimal incomeAmount = 0;
                    decimal expenseAmount = 0;
                    decimal xferAmount = 0;
                    decimal incomeAmountIntl = 0;
                    decimal expenseAmountIntl = 0;
                    decimal xferAmountIntl = 0;

                    Boolean transferFound = false;

                    /* Go through all the transactions.
                     */
                    string transSQLWhereClause =
                        "a_cost_centre_code_c = '" + costCentreRow.CostCentreCode +
                        "' AND a_transaction_status_l = true" +
                        " AND a_ich_number_i = 0";

                    string transRowOrder = "a_batch_number_i, a_journal_number_i, a_transaction_number_i";

                    DataRow[] foundTransRows = mainDS.ATransaction.Select(transSQLWhereClause, transRowOrder);

                    foreach (DataRow untypedTransRow in foundTransRows)
                    {
                        ATransactionRow transRow = (ATransactionRow)untypedTransRow;

                        DataRow[] foundJournalRows = mainDS.AJournal.Select(
                            "a_batch_number_i = " + transRow.BatchNumber +
                            " AND a_journal_number_i = " + transRow.JournalNumber
                            );

                        //
                        // There should certainly be a journal for this transaction.
                        // If there isn't, that's weird.

                        if (foundJournalRows != null)
                        {
                            transferFound = true;
                            transRow.IchNumber = iCHProcessing;

                            if (transRow.DebitCreditIndicator == PlAccountDrCrIndicator)
                            {
                                settlementAmount -= transRow.AmountInBaseCurrency;
                            }
                            else
                            {
                                settlementAmount += transRow.AmountInBaseCurrency;
                            }

                            //Process Income (ln:333)
                            if (incomeAccounts.Contains(transRow.AccountCode))
                            {
                                if (transRow.DebitCreditIndicator == incomeDrCrIndicator)
                                {
                                    incomeAmount += transRow.AmountInBaseCurrency;
                                    incomeAmountIntl += transRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    incomeAmount -= transRow.AmountInBaseCurrency;
                                    incomeAmountIntl -= transRow.AmountInIntlCurrency;
                                }
                            }

                            //process expenses
                            if (expenseAccounts.Contains(transRow.AccountCode)
                                && (transRow.AccountCode != MFinanceConstants.DIRECT_XFER_ACCT)
                                && (transRow.AccountCode != MFinanceConstants.ICH_ACCT_SETTLEMENT))
                            {
                                if (transRow.DebitCreditIndicator == expenseDrCrIndicator)
                                {
                                    expenseAmount += transRow.AmountInBaseCurrency;
                                    expenseAmountIntl += transRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    expenseAmount -= transRow.AmountInBaseCurrency;
                                    expenseAmountIntl -= transRow.AmountInIntlCurrency;
                                }
                            }

                            //Process Direct Transfers
                            if (transRow.AccountCode == MFinanceConstants.DIRECT_XFER_ACCT)
                            {
                                if (transRow.DebitCreditIndicator == expenseDrCrIndicator)
                                {
                                    xferAmount += transRow.AmountInBaseCurrency;
                                    xferAmountIntl += transRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    xferAmount -= transRow.AmountInBaseCurrency;
                                    xferAmountIntl -= transRow.AmountInIntlCurrency;
                                }
                            }
                        }
                    }  //end of foreach transaction

                    /* Mark all the original gifts with the iCHProcessing value
                     */
                    if (transferFound)
                    {
                        AGiftDetailTable giftDetailTable = new AGiftDetailTable();
                        AGiftDetailRow giftDetailTemplateRow = (AGiftDetailRow)giftDetailTable.NewRowTyped(false);

                        giftDetailTemplateRow.LedgerNumber = ALedgerNumber;
                        giftDetailTemplateRow.IchNumber = 0;
                        giftDetailTemplateRow.CostCentreCode = costCentreRow.CostCentreCode;

                        foreach (AGiftBatchRow giftBatchRow in giftBatchTable.Rows)
                        {
                            giftDetailTemplateRow.BatchNumber = giftBatchRow.BatchNumber;
                            giftDetailTable = AGiftDetailAccess.LoadUsingTemplate(giftDetailTemplateRow, ADBTransaction);

                            foreach (AGiftDetailRow giftDetailRow in giftDetailTable.Rows)
                            {
                                giftDetailRow.IchNumber = iCHProcessing;
                            }

                            AGiftDetailAccess.SubmitChanges(giftDetailTable, ADBTransaction);
                        }
                    } // if transferFound

                    if ((settlementAmount == 0) // If there's no activity in this CC,
                        && (incomeAmount == 0)  // bail to the next one.
                        && (expenseAmount == 0)
                        && (xferAmount == 0))
                    {
                        continue;
                    }

                    /* Balance the cost centre by entering an opposite transaction
                     * to ICH settlement. Use positive amounts only.
                     */
                    drCrIndicator = PlAccountDrCrIndicator;

                    if (settlementAmount < 0)
                    {
                        drCrIndicator = !PlAccountDrCrIndicator;
                        settlementAmount = 0 - settlementAmount;
                    }

                    if (settlementAmount > 0)
                    {
                        /* Generate the transaction to 'balance' the foreign fund -
                         *  in the ICH settlement account.
                         */

                        //RUN gl1130o.p ("new":U,
                        //Create a transaction
                        if (!TGLPosting.CreateATransaction(mainDS, ALedgerNumber, AglBatchNumber, gLJournalNumber,
                                Catalog.GetString("ICH Monthly Clearing"),
                                MFinanceConstants.ICH_ACCT_SETTLEMENT, // DestinationAccount[CostCentreRow.CostCentreCode],
                                costCentreRow.CostCentreCode, settlementAmount, periodEndDate, drCrIndicator,
                                Catalog.GetString("ICH Process"), true, settlementAmount,
                                out gLTransactionNumber))
                        {
                            ErrorMessage =
                                String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    AglBatchNumber,
                                    gLJournalNumber);
                            ErrorType = TResultSeverity.Resv_Noncritical;

                            AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                            return false;
                        }

                        //Mark as processed
                        ATransactionRow transRow =
                            (ATransactionRow)mainDS.ATransaction.Rows.Find(new object[] { ALedgerNumber, AglBatchNumber, gLJournalNumber,
                                                                                          gLTransactionNumber });
                        transRow.IchNumber = iCHProcessing;

                        /* Increment or decrement the ICH total to be transferred after this loop.
                         * NOTE - if this is a "non-ICH fund", I need to balance it separately, and I'll do that right here.
                         */
                        if (costCentreRow.ClearingAccount == MFinanceConstants.ICH_ACCT_ICH)
                        {
                            if (drCrIndicator == MFinanceConstants.IS_DEBIT)
                            {
                                iCHTotal += settlementAmount;
                            }
                            else
                            {
                                iCHTotal -= settlementAmount;
                            }

                            ICHTransactionsIncluded = true;
                        }
                        else
                        {
                            // I'm creating a transaction right here for this "non-ICH" CostCentre.
                            // This potentially means that there will be multiple transactions to the "non-ICH" account,
                            // whereas the ICH account has only a single transaction, but that's not big deal:

                            if (!TGLPosting.CreateATransaction(mainDS, ALedgerNumber, AglBatchNumber, gLJournalNumber,
                                    Catalog.GetString("Non-ICH foreign fund Clearing"),
                                    costCentreRow.ClearingAccount,
                                    standardCostCentre, settlementAmount, periodEndDate, !drCrIndicator, Catalog.GetString("Non-ICH"),
                                    true, settlementAmount,
                                    out gLTransactionNumber))
                            {
                                ErrorContext = Catalog.GetString("Generating non-ICH transaction");
                                ErrorMessage =
                                    String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                        ALedgerNumber,
                                        AglBatchNumber,
                                        gLJournalNumber);
                                ErrorType = TResultSeverity.Resv_Noncritical;

                                AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                                return false;
                            }

                            nonICHTransactionsIncluded = true;
                        }
                    }

                    /* Now create corresponding report row on stewardship table,
                     * Only for Cost Centres that cleared to ICH
                     */
                    if ((costCentreRow.ClearingAccount == MFinanceConstants.ICH_ACCT_ICH)
                        && ((incomeAmount != 0)
                            || (expenseAmount != 0)
                            || (xferAmount != 0)))
                    {
                        AIchStewardshipRow iCHStewardshipRow = iCHStewardshipTable.NewRowTyped(true);

                        iCHStewardshipRow.LedgerNumber = ALedgerNumber;
                        iCHStewardshipRow.Year = ledgerRow.CurrentFinancialYear;
                        iCHStewardshipRow.PeriodNumber = APeriodNumber;
                        iCHStewardshipRow.IchNumber = iCHProcessing;
                        iCHStewardshipRow.DateProcessed = DateTime.Today;
                        iCHStewardshipRow.CostCentreCode = costCentreRow.CostCentreCode;
                        iCHStewardshipRow.IncomeAmount = incomeAmount;
                        iCHStewardshipRow.ExpenseAmount = expenseAmount;
                        iCHStewardshipRow.DirectXferAmount = xferAmount;
                        iCHStewardshipRow.IncomeAmountIntl = incomeAmountIntl;
                        iCHStewardshipRow.ExpenseAmountIntl = expenseAmountIntl;
                        iCHStewardshipRow.DirectXferAmountIntl = xferAmountIntl;
                        iCHStewardshipTable.Rows.Add(iCHStewardshipRow);
                    }
                }   // for each cost centre

                //
                // If I already have critical errors,
                // I need to unwind and tidy up:
                if (AVerificationResults.HasCriticalErrors)
                {
                    // An empty GL Batch now exists, which I need to delete.
                    //
                    TVerificationResultCollection batchCancelResult = new TVerificationResultCollection();

                    TGLPosting.DeleteGLBatch(
                        ALedgerNumber,
                        AglBatchNumber,
                        out batchCancelResult);
                    AglBatchNumber = -1;
                    AVerificationResults.AddCollection(batchCancelResult);
                }
                else // No critical errors
                {
                    /* Update the balance of the ICH account (like a bank account).
                     * If the total is negative, it means the ICH batch has a
                     * credit total so far. Thus, we now balance it with the opposite
                     * transaction. */

                    if (iCHTotal < 0)
                    {
                        drCrIndicator = MFinanceConstants.IS_DEBIT;
                        iCHTotal = -iCHTotal;
                    }
                    else if (iCHTotal > 0)
                    {
                        drCrIndicator = MFinanceConstants.IS_CREDIT;
                    }

                    /* Balance the ICH total.
                     * If the total is already 0 then this is ok
                     * (eg last minute change of a gift from one field to another).
                     */

                    if (iCHTotal != 0)
                    {
                        //Create a transaction
                        if (!TGLPosting.CreateATransaction(mainDS, ALedgerNumber, AglBatchNumber, gLJournalNumber,
                                Catalog.GetString("ICH Monthly Clearing"),
                                MFinanceConstants.ICH_ACCT_ICH, standardCostCentre, iCHTotal, periodEndDate, drCrIndicator,
                                Catalog.GetString("ICH"),
                                true, iCHTotal,
                                out gLTransactionNumber))
                        {
                            ErrorContext = Catalog.GetString("Generating the ICH batch");
                            ErrorMessage =
                                String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    AglBatchNumber,
                                    gLJournalNumber);
                            ErrorType = TResultSeverity.Resv_Noncritical;

                            AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                            return false;
                        }
                    }

                    //Post the batch
                    if (ICHTransactionsIncluded | nonICHTransactionsIncluded)
                    {
                        AIchStewardshipAccess.SubmitChanges(iCHStewardshipTable, ADBTransaction);

                        mainDS.ThrowAwayAfterSubmitChanges = true; // SubmitChanges will not return to me any changes made in MainDS.
                        GLBatchTDSAccess.SubmitChanges(mainDS);

                        // refresh cached ICHStewardship table
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableFinanceTablesEnum.ICHStewardshipList.ToString());

                        IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, AglBatchNumber, out AVerificationResults);
                    }
                    else // There were no transactions
                    {
                        AVerificationResults.Add(new TVerificationResult(ErrorContext,
                                Catalog.GetString("No Stewardship batch is required."),
                                TResultSeverity.Resv_Status));

                        // An empty GL Batch now exists, which I need to delete.
                        //
                        TVerificationResultCollection BatchCancelResult = new TVerificationResultCollection();

                        TGLPosting.DeleteGLBatch(
                            ALedgerNumber,
                            AglBatchNumber,
                            out BatchCancelResult);
                        AglBatchNumber = -1;
                        AVerificationResults.AddCollection(BatchCancelResult);

                        IsSuccessful = true;
                    }
                }
            } // try
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return IsSuccessful;
        } // Generate ICH Stewardship Batch

        /// <summary>
        /// To build a CSV list of accounts
        /// </summary>
        private static void BuildChildAccountList(int ALedgerNumber,
            AAccountRow AAccountRowFirst,
            TDBTransaction DBTransaction,
            ref string AChildAccounts)
        {
            //Return value
            string AccountCode = AAccountRowFirst.AccountCode;

            try
            {
                if (AAccountRowFirst.PostingStatus)
                {
                    AChildAccounts += AccountCode + ",";
                }
                else
                {
                    AAccountHierarchyDetailTable AccountHierarchyDetailTable1 = new AAccountHierarchyDetailTable();
                    AAccountHierarchyDetailRow TemplateRow = (AAccountHierarchyDetailRow)AccountHierarchyDetailTable1.NewRowTyped(false);

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.AccountHierarchyCode = MFinanceConstants.ACCOUNT_HIERARCHY_CODE;
                    TemplateRow.AccountCodeToReportTo = AAccountRowFirst.AccountCode;

                    StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                    AAccountHierarchyDetailTable AccountHierarchyDetailTable2 = AAccountHierarchyDetailAccess.LoadUsingTemplate(TemplateRow,
                        operators,
                        null,
                        DBTransaction);

                    if (AccountHierarchyDetailTable2 != null)
                    {
                        for (int m = 0; m < AccountHierarchyDetailTable2.Count; m++)
                        {
                            AAccountHierarchyDetailRow AccountHierarchyDetailRow = (AAccountHierarchyDetailRow)AccountHierarchyDetailTable2.Rows[m];

                            AAccountTable AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber,
                                AccountHierarchyDetailRow.ReportingAccountCode,
                                DBTransaction);

                            if (AccountTable != null)
                            {
                                AAccountRow AccountRow = (AAccountRow)AccountTable.Rows[0];

                                BuildChildAccountList(ALedgerNumber,
                                    AccountRow,
                                    DBTransaction,
                                    ref AChildAccounts);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
        }
    }
}
