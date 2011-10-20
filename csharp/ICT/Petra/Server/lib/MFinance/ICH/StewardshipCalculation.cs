//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert, timop
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.AP;
using Ict.Petra.Shared.MFinance.AR;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MFinance.ICH
{
    /// <summary>
    /// Class for the performance of the Stewardship Calculation
    /// </summary>
    public class TStewardshipCalculation
    {
        /// <summary>
        /// Performs the ICH Stewardship Calculation.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if calculation succeeded, otherwise false.</returns>
        public bool PerformStewardshipCalculation(int ALedgerNumber,
            int APeriodNumber,
            out TVerificationResultCollection AVerificationResult
            )
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": PerformStewardshipCalculation called.");
            }

            AVerificationResult = null;

            //Begin the transaction
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                if (GenerateAdminFeeBatch(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult))
                {
                    DBAccess.GDBAccessObj.CommitTransaction();

                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction committed!");
                    }

                    return true;
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because of an error!");
                    }

                    return false;
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                if (TLogging.DL >= 8)
                {
                    Console.WriteLine(
                        this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because an exception occurred!");
                }

                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw Exp;
            }
        }

        /// <summary>
        /// Reads from the table holding all the fees charged for this month and generates a GL batch from it.
        /// Relates to gl2150.p
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APrintReport"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool GenerateAdminFeeBatch(int ALedgerNumber,
            int APeriodNumber,
            bool APrintReport,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            /*-----gl1250.p - begin-----*/
            //Return value
            bool IsSuccessful = true;  // TODO: change this to false once the actual implementation is in place!

            bool CreatedSuccessfully = false;
            decimal ATransactionAmount;
            string DrAccountCode;
            string DestCostCentreCode = string.Empty;
            string DestAccountCode = string.Empty;
            string FeeDescription = string.Empty;
            decimal DrFeeTotal = 0;
            bool DrCrIndicator = true;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            /* Make a temporary table to hold totals for gifts going to
             *  each account. */
            GLStewardshipCalculationTDSCreditFeeTotalTable CreditFeeTotalDT = new GLStewardshipCalculationTDSCreditFeeTotalTable();
            GLStewardshipCalculationTDSCreditFeeTotalRow CreditFeeTotalDR = null;

            //CreditFeeTotalDR = CreditFeeTotalDT.NewRowTyped();
            //GLStewardshipCalculationTDSCreditFeeTotalRow CreditFeeTotalRows = CreditFeeTotalDT.Rows.Find(new object[] {CreditFeeTotalDR.CostCentreCode, CreditFeeTotalDR.AccountCode, CreditFeeTotalDR.TransactionAmount});

            /* Retrieve info on the ledger. */
            ALedgerTable AvailableLedgers = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)AvailableLedgers.Rows[0];

            try
            {
                /* Check that we have not closed all periods for the year yet.
                 *  (Not at the provisional year end point) */
                if (LedgerRow.ProvisionalYearEndFlag)
                {
                    //Petra ErrorCode = GL0071
                    ErrorContext = "Generate Admin Fee Batch";
                    ErrorMessage = String.Format(Catalog.GetString(
                            "Cannot progress as Ledger {0} is at the provisional year-end point"), ALedgerNumber);
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                /* 0003 Finds for ledger base currency format, for report currency format */
                ACurrencyTable CurrencyInfo = ACurrencyAccess.LoadByPrimaryKey(LedgerRow.BaseCurrency, ADBTransaction);
                ACurrencyRow CurrencyRow = (ACurrencyRow)CurrencyInfo.Rows[0];

                /* 0001 Extract number of decimal places */
                string NumericFormat = CurrencyRow.DisplayFormat;
                int NumDecPlaces = THelperNumeric.CalcNumericFormatDecimalPlaces(NumericFormat);

                /* Create the journal to create the fee transactions in, if there are
                 *  fees to charge.
                 * NOTE: if the date in the processed fee table is ? then that fee
                 *  hasn't been processed. */
                AProcessedFeeTable ProcessedFeeDataTable = new AProcessedFeeTable();
                AProcessedFeeRow TemplateRow = (AProcessedFeeRow)ProcessedFeeDataTable.NewRowTyped(false);

                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.PeriodicAmount = 0;
                TemplateRow.PeriodNumber = APeriodNumber;
                TemplateRow.SetProcessedDateNull();

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "<>", "=", "=" });
                StringCollection OrderList = new StringCollection();
                //Order by Fee Code
                OrderList.Add("ORDER BY");
                OrderList.Add(AProcessedFeeTable.GetFeeCodeDBName() + " ASC");
                OrderList.Add(AProcessedFeeTable.GetCostCentreCodeDBName() + " ASC");

                AProcessedFeeTable ProcessedFee = AProcessedFeeAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction, OrderList, 0, 0);

                if (ProcessedFee.Count > 0)
                {
                    //Post to Ledger - Ln 132
                    //****************4GL Transaction Starts Here********************
                    string BatchDescription = "Admin Fees & Grants";

                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        APeriodNumber,
                        ADBTransaction);
                    AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    GLBatchTDS AdminFeeDS = TGLPosting.CreateABatch(ALedgerNumber);
                    ABatchRow AdminFeeBatch = AdminFeeDS.ABatch[0];
                    AdminFeeBatch.BatchDescription = BatchDescription;
                    AdminFeeBatch.DateEffective = AccountingPeriodRow.PeriodEndDate;
                    AdminFeeBatch.BatchPeriod = APeriodNumber;
                    AdminFeeBatch.BatchYear = LedgerRow.CurrentFinancialYear;

                    AJournalRow JournalRow = AdminFeeDS.AJournal.NewRowTyped();
                    JournalRow.LedgerNumber = ALedgerNumber;
                    JournalRow.BatchNumber = AdminFeeBatch.BatchNumber;
                    JournalRow.JournalNumber = AdminFeeBatch.LastJournal + 1;
                    AdminFeeBatch.LastJournal += 1;
                    JournalRow.JournalDescription = BatchDescription;
                    JournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                    JournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                    JournalRow.TransactionCurrency = LedgerRow.BaseCurrency;
                    JournalRow.ExchangeRateToBase = 1;
                    JournalRow.DateEffective = AccountingPeriodRow.PeriodEndDate;
                    JournalRow.JournalPeriod = APeriodNumber;
                    AdminFeeDS.AJournal.Rows.Add(JournalRow);

                    //
                    // Generate the transactions
                    //

                    /* M009 Changed the following loop for Petra 2.1 design changes. a_processed_fee
                     * now has a record for each gift detail so it is necessary to sum up all the
                     * totals for each fee code/cost centre so that only one transaction is posted
                     * for each */
                    string CurrentFeeCode = string.Empty;
                    string CostCentreCode = string.Empty;

                    for (int i = 0; i < ProcessedFee.Count; i++)
                    {
                        AProcessedFeeRow pFR = (AProcessedFeeRow)ProcessedFee.Rows[i];

                        if (CurrentFeeCode != pFR.FeeCode)
                        {
                            CurrentFeeCode = pFR.FeeCode;
                            CostCentreCode = pFR.CostCentreCode;
                            // Find first
                            AFeesPayableTable FeesPayableTable = AFeesPayableAccess.LoadByPrimaryKey(ALedgerNumber, CurrentFeeCode, ADBTransaction);
                            AFeesPayableRow FeesPayableRow = (AFeesPayableRow)FeesPayableTable.Rows[0];

                            if (FeesPayableTable.Count == 0)  //try payables instead
                            {
                                AFeesReceivableTable FeesReceivableTable = AFeesReceivableAccess.LoadByPrimaryKey(ALedgerNumber,
                                    CurrentFeeCode,
                                    ADBTransaction);
                                AFeesReceivableRow FeesReceivableRow = (AFeesReceivableRow)FeesReceivableTable.Rows[0];

                                if (FeesReceivableTable.Count == 0)
                                {
                                    //Petra error: X_0007
                                    ErrorContext = "Generate Transactions";
                                    ErrorMessage =
                                        String.Format(Catalog.GetString(
                                                "Unable to access information for Fee Code '{1}' in either the Fees Payable & Receivable Tables for Ledger {0}"),
                                            ALedgerNumber, CurrentFeeCode);
                                    ErrorType = TResultSeverity.Resv_Critical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }
                                else
                                {
                                    DrAccountCode = FeesReceivableRow.DrAccountCode;
                                    DestCostCentreCode = FeesReceivableRow.CostCentreCode;
                                    DestAccountCode = FeesReceivableRow.AccountCode;
                                    FeeDescription = FeesReceivableRow.FeeDescription;
                                }
                            }
                            else
                            {
                                DrAccountCode = FeesPayableRow.DrAccountCode;
                                DestCostCentreCode = FeesPayableRow.CostCentreCode;
                                DestAccountCode = FeesPayableRow.AccountCode;
                                FeeDescription = FeesPayableRow.FeeDescription;
                            }

                            DrFeeTotal = 0;

                            for (int j = 0; j < ProcessedFee.Count; j++)
                            {
                                AProcessedFeeRow pFR2 = (AProcessedFeeRow)ProcessedFee.Rows[j];

                                if ((pFR2.FeeCode == CurrentFeeCode) && (pFR2.CostCentreCode == CostCentreCode))
                                {
                                    //TODO: check the rounding issues
                                    DrFeeTotal += pFR2.PeriodicAmount; //ROUND(pFR2.PeriodicAmount, lv_dp)
                                }
                                else if ((pFR2.FeeCode == CurrentFeeCode) && (pFR2.CostCentreCode != CostCentreCode))
                                {
                                    if (DrFeeTotal != 0)
                                    {
                                        if (DrFeeTotal < 0)
                                        {
                                            DrCrIndicator = false; //Credit
                                            DrFeeTotal = -DrFeeTotal;
                                        }
                                        else
                                        {
                                            DrCrIndicator = true; //Debit
                                            //lv_dr_fee_total remains unchanged
                                        }

                                        /* Generate the transction to deduct the fee amount from
                                         *  the source cost centre. (Expense leg) */
                                        //RUN gl1130o.p -> gl1130.i
                                        ATransactionRow TransactionRow = AdminFeeDS.ATransaction.NewRowTyped();
                                        TransactionRow.LedgerNumber = ALedgerNumber;
                                        TransactionRow.BatchNumber = AdminFeeBatch.BatchNumber;
                                        TransactionRow.JournalNumber = JournalRow.JournalNumber;
                                        TransactionRow.Narrative = "Fee: " + FeeDescription + " (" + CurrentFeeCode + ")";
                                        TransactionRow.AccountCode = DrAccountCode;
                                        TransactionRow.CostCentreCode = CostCentreCode;
                                        TransactionRow.TransactionAmount = DrFeeTotal;
                                        TransactionRow.TransactionDate = AccountingPeriodRow.PeriodEndDate;
                                        TransactionRow.DebitCreditIndicator = DrCrIndicator;
                                        TransactionRow.Reference = "AG";
                                        TransactionRow.SystemGenerated = true;
                                        AdminFeeDS.ATransaction.Rows.Add(TransactionRow);

                                        DrFeeTotal = 0;
                                    }

                                    CostCentreCode = pFR2.CostCentreCode;
                                }
                            }
                        }

                        /* Mark each fee entry as processed. */
                        pFR.ProcessedDate = DateTime.Today.Date;
                        pFR.Timestamp =
                            (DateTime.Today.TimeOfDay.Hours * 3600 + DateTime.Today.TimeOfDay.Minutes * 60 + DateTime.Today.TimeOfDay.Seconds);

                        /* Add the charges on this account to the fee total,
                         * creating an entry if necessary. (This is for the income total) */
                        CreditFeeTotalDR = (GLStewardshipCalculationTDSCreditFeeTotalRow)CreditFeeTotalDT.Rows.Find(new object[] { DestCostCentreCode,
                                                                                                                                   DestAccountCode });                   //, CreditFeeTotalDR.TransactionAmount});

                        if (CreditFeeTotalDR != null)
                        {
                            CreditFeeTotalDR.TransactionAmount += Math.Round(pFR.PeriodicAmount, NumDecPlaces);
                        }
                        else
                        {
                            CreditFeeTotalDR = CreditFeeTotalDT.NewRowTyped();
                            CreditFeeTotalDR.CostCentreCode = DestCostCentreCode;
                            CreditFeeTotalDR.AccountCode = DestAccountCode;
                            CreditFeeTotalDR.TransactionAmount = pFR.PeriodicAmount;
                        }
                    }

                    /* Generate the transction to credit the fee amounts to
                     * the destination accounts. (Income leg) */
                    for (int k = 0; k < CreditFeeTotalDT.Count; k++)
                    {
                        GLStewardshipCalculationTDSCreditFeeTotalRow cFT = (GLStewardshipCalculationTDSCreditFeeTotalRow)CreditFeeTotalDT.Rows[k];

                        if (cFT.TransactionAmount < 0)
                        {
                            /* The case of a negative gift total should be very rare.
                             * It would only happen if, for instance, the was only
                             * a reversal but no new gifts for a certain ledger. */
                            DrCrIndicator = true; //Debit
                            ATransactionAmount = -cFT.TransactionAmount;
                        }
                        else
                        {
                            DrCrIndicator = false; //Credit
                            ATransactionAmount = cFT.TransactionAmount;
                        }

                        /* 0002 - Ok for it to be 0 as just a correction */
                        if (cFT.TransactionAmount != 0)
                        {
                            ATransactionRow TransactionRow = AdminFeeDS.ATransaction.NewRowTyped();
                            TransactionRow.LedgerNumber = ALedgerNumber;
                            TransactionRow.BatchNumber = AdminFeeBatch.BatchNumber;
                            TransactionRow.JournalNumber = JournalRow.JournalNumber;
                            TransactionRow.Narrative = "Collected admin charges";
                            TransactionRow.AccountCode = cFT.AccountCode;
                            TransactionRow.CostCentreCode = cFT.CostCentreCode;
                            TransactionRow.TransactionAmount = ATransactionAmount;
                            TransactionRow.TransactionDate = AccountingPeriodRow.PeriodEndDate;
                            TransactionRow.DebitCreditIndicator = DrCrIndicator;
                            TransactionRow.Reference = "AG";
                            TransactionRow.SystemGenerated = true;
                            AdminFeeDS.ATransaction.Rows.Add(TransactionRow);
                        }
                    }

                    TVerificationResultCollection Verification = null;

                    /* check that something has been posted - we know this if the IsSuccessful flag is still false */
                    if (!CreatedSuccessfully)
                    {
                        // MESSAGE "No fees to charge were found.(2)" VIEW-AS ALERT-BOX MESSAGE.
                        IsSuccessful = true;
                        //UNDO Post_To_Ledger, LEAVE Post_To_Ledger.
                    }
                    else
                    {
                        //Post the batch just created

                        /*RUN gl1210.p (pvedgerumber,lv_gl_batch_number,TRUE,OUTPUT IsSuccessful).*/
                        IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, AdminFeeBatch.BatchNumber, out Verification);
                    }

                    if ((Verification == null) || Verification.HasCriticalError())
                    {
                        //Petra error: GL0067
                        ErrorContext = "Posting Admin Fee Batch";
                        ErrorMessage = String.Format(Catalog.GetString("The posting of the admin fee batch failed."));
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    //End of Trnsaction block in 4GL

                    /* Print the Admin Fee Calculations report, if requested */
                    if (APrintReport && IsSuccessful)
                    {
                        //TODO
                    }
                }
                else
                {
                    /*     MESSAGE "No fees to charge were found." VIEW-AS ALERT-BOX MESSAGE. */
                    IsSuccessful = true;
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsSuccessful = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Generate Admin Fee Batch";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating admin fee batch for Ledger: {0}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsSuccessful = false;
            }

            return IsSuccessful;
        }
    }
}