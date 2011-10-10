//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert
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

namespace Ict.Petra.Server.MFinance.ICH
{
    /// <summary>
    /// Class for the performance of the Stewardship Calculation
    /// </summary>
    public class TStewardshipCalculation
    {
        private const string CONTEXT = "Stewardship Calculation";


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
            string StandardCostCentre;
            string StdSummaryCostCentre;

            //* 0003 Standard cost centres (ie, ledger 37 = "3700") */
            StandardCostCentre = String.Format(("{0:##00}"), ALedgerNumber) + "00";

            //* 0006 Standard summary cost centres (ie, ledger 37 = "3700S") */
            StdSummaryCostCentre = String.Format(("{0:##00}"), ALedgerNumber) + "00S";

            bool FirstCallResult = false;
            bool SecondCallResult = false;
            bool ThirdCallResult = false;

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": PerformStewardshipCalculation called.");
            }
#endif

            AVerificationResult = null;

            //Begin the transaction
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                FirstCallResult = GenerateAdminFeeBatch(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult);

                if (FirstCallResult)
                {
                    SecondCallResult = true;   // TODO: Second call in sequence  - e,g,   '= SecondCall(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult)'
                }

                // TODO: 0..n other calls in sequence, e.g.:
                if (SecondCallResult)
                {
                    ThirdCallResult = true;   // TODO: Third call in sequence  - e,g,   '= ThirdCall(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult)'
                }

                if (FirstCallResult && SecondCallResult && ThirdCallResult)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction committed!");
                    }
#endif
                    return true;
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    //MessageBox.Show(Messages.BuildMessageFromVerificationResult("An Error occured while creating a new Admin Fee entry:", ref AVerificationResult));


#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because of an error!");
                    }
#endif
                    return false;
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine(
                        this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because an exception occurred!");
                }
#endif
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


            const int NO_HASH_TOTAL = 0;
            string BatchDescription = string.Empty;
            int BatchNumber = 0;
            bool BatchCreated = false;
            int JournalNumber = 0;
            bool JournalCreated = false;
            int TransactionNumber = 0;
            bool CreatedSuccessfully = false;
            decimal ATransactionAmount;
            string DrAccountCode;
            string DestCostCentreCode = string.Empty;
            string DestAccountCode = string.Empty;
            string FeeDescription = string.Empty;
            decimal DrFeeTotal = 0;
            bool DrCrIndicator = true;
            int NumDecPlaces = 0;

            int AccountingPeriod;
            DateTime PeriodEndDate;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            //DEFINE BUFFER a_processed_fee_b FOR a_processed_fee.

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
                NumDecPlaces = THelperNumeric.CalcNumericFormatDecimalPlaces(NumericFormat);

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
                OrderList.Add("ORDER BY " + AProcessedFeeTable.GetFeeCodeDBName() + " ASC");
                OrderList.Add(AProcessedFeeTable.GetCostCentreCodeDBName() + " ASC");

                AProcessedFeeTable ProcessedFee = AProcessedFeeAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction, OrderList, 0, 0);

                if (ProcessedFee.Count > 0)
                {
                    //Post to Ledger - Ln 132
                    //****************4GL Transaction Starts Here********************
                    BatchDescription = "Admin Fees & Grants";

                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        APeriodNumber,
                        ADBTransaction);
                    AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    AccountingPeriod = AccountingPeriodRow.AccountingPeriodNumber;
                    PeriodEndDate = AccountingPeriodRow.PeriodEndDate;

                    //RUN gl1110o.p -> gl1110.i ->
                    BatchCreated = CreateAdminFeeBatch("NEW",
                        ALedgerNumber,
                        0,                                        /* DBNull.Value,*/
                        BatchDescription,
                        NO_HASH_TOTAL,
                        PeriodEndDate,
                        ref BatchNumber,
                        ADBTransaction,
                        ref AVerificationResult);

                    if (!BatchCreated)
                    {
                        //Petra error: GL0039
                        ErrorContext = "Admin Grant & Fee Calculations";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "There was a failure while attempting to create the batch for Ledger {0}"), ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Critical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    //RUN gl1120.i
                    JournalCreated = CreateAdminFeeJournal("NEW",
                        ALedgerNumber,
                        BatchNumber,
                        0,
                        BatchDescription,
                        "{&GENERAL-LEDGER}",
                        "{&STANDARD-JOURNAL}",
                        LedgerRow.BaseCurrency,
                        1,
                        PeriodEndDate,
                        ref JournalNumber,
                        ADBTransaction,
                        ref AVerificationResult);

                    if (!JournalCreated)
                    {
                        //Petra error: GL0035
                        ErrorContext = "Admin Grant & Fee Calculations";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "There was a failure while attempting to create the journal for Ledger {0}"), ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Critical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    /***************************
                    *  Generate the transactions
                    ***************************/

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
                                        CreatedSuccessfully = CreateTransactions("NEW",
                                            ALedgerNumber,
                                            BatchNumber,
                                            JournalNumber,
                                            0,
                                            "Fee: " + FeeDescription + " (" + CurrentFeeCode + ")",
                                            DrAccountCode,
                                            CostCentreCode,
                                            DrFeeTotal,
                                            PeriodEndDate,
                                            DrCrIndicator,
                                            null,
                                            null,
                                            0,
                                            0,
                                            null,
                                            "AG",
                                            true,
                                            0,
                                            ref TransactionNumber,
                                            ADBTransaction,
                                            ref AVerificationResult
                                            );

                                        if (!CreatedSuccessfully)
                                        {
                                            //Petra error: GL0067
                                            ErrorContext = "Create Transactions";
                                            ErrorMessage =
                                                String.Format(Catalog.GetString(
                                                        "There was a failure attempting to create the transaction for the admin charges."));
                                            ErrorType = TResultSeverity.Resv_Critical;
                                            throw new System.InvalidOperationException(ErrorMessage);
                                        }

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
                            CreatedSuccessfully = CreateTransactions("NEW",
                                ALedgerNumber,
                                BatchNumber,
                                JournalNumber,
                                0,
                                "Collected admin charges",
                                cFT.AccountCode,
                                cFT.CostCentreCode,
                                ATransactionAmount,
                                PeriodEndDate,
                                DrCrIndicator,
                                null,
                                null,
                                0,
                                0,
                                null,
                                "AG",
                                true,
                                0,
                                ref TransactionNumber,
                                ADBTransaction,
                                ref AVerificationResult
                                );

                            if (!CreatedSuccessfully)
                            {
                                //Petra error: GL0067
                                ErrorContext = "Create Transactions";
                                ErrorMessage =
                                    String.Format(Catalog.GetString(
                                            "There was a failure attempting to create the transaction for the collected admin charges."));
                                ErrorType = TResultSeverity.Resv_Critical;
                                throw new System.InvalidOperationException(ErrorMessage);
                            }
                        }
                    }

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
                        IsSuccessful = PostNewBatch(ALedgerNumber, BatchNumber, true);
                    }

                    if (!IsSuccessful)
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
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating admin fee batch: '{0}' for Ledger: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    BatchDescription,
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsSuccessful = false;
            }

            return IsSuccessful;
        }

        /// <summary>
        /// Retrieves the Ledger short name from the Partner table
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private string RetrieveLedgerName(int ALedgerNumber,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            string ReturnLedgerName = string.Empty;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            try
            {
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LedgerRow.PartnerKey, ADBTransaction);
                PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                ReturnLedgerName = PartnerRow.PartnerShortName;
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                ReturnLedgerName = "";
            }
            catch (Exception ex)
            {
                ErrorContext = "Ledger Name";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while extracting short Name for Ledger {0} from the Partner table." +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                ReturnLedgerName = "";
            }

            return ReturnLedgerName;
        }

        /// <summary>
        /// Test that a date is valid: In the current period or within
        ///  forward posting limits.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATestDate">Test Date</param>
        /// <param name="AProcessingPeriod">Processing Period</param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool CheckForValidDate(int ALedgerNumber,
            DateTime ATestDate,
            ref int AProcessingPeriod,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //****gl4320 <- gl4330.p <- gl1130fa.i <- gl1130.i <- gl2150.p

            //Return value
            bool ValidDateIndicator = false;

            int CurrentPeriod;
            int MaxPeriod;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            CurrentPeriod = LedgerRow.CurrentPeriod;

            try
            {
                AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                    LedgerRow.CurrentPeriod,
                    ADBTransaction);

                if (AccountingPeriodTable.Count == 0)
                {
                    //RUN x_table.p ("a_accounting_period":U).
                    //RUN s_errmsg.p ("X_0007":U, PROGRAM-NAME(1), lv_message, RETURN-VALUE).
                    //&1 does not exist in &2.
                    ErrorContext = "Data Validation";
                    ErrorMessage =
                        String.Format(Catalog.GetString("Accounting Period Key (Ledger = {0}, Accounting Period Number = {1}) does not exist."),
                            ALedgerNumber,
                            CurrentPeriod
                            );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                /* Must be after the period start date. */
                if (ATestDate < AccountingPeriodRow.PeriodStartDate)
                {
                    /*RUN s_errmsg.p ("GL0013":U, PROGRAM-NAME(1),
                     *                  STRING(pv_test_date_d),
                     *                  FormatInternationalDate( pv_test_date_d),
                     *                  STRING(lv_current_period)).*/
                    ErrorContext = "Data Validation";
                    ErrorMessage = String.Format(Catalog.GetString("This date {0:d} refers to a prior accounting period."),
                        ATestDate);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                /* Check if the date is in the curent period, a fwd posting, or
                 * to far ahead. */
                if (ATestDate > AccountingPeriodRow.PeriodEndDate)
                {
                    AAccountingPeriodTable AccountingPeriodTb = new AAccountingPeriodTable();
                    AAccountingPeriodRow TemplateRow8 = (AAccountingPeriodRow)AccountingPeriodTb.NewRowTyped(false);

                    TemplateRow8.LedgerNumber = ALedgerNumber;
                    TemplateRow8.PeriodStartDate = ATestDate;
                    TemplateRow8.PeriodEndDate = ATestDate;

                    StringCollection operators8 = StringHelper.InitStrArr(new string[] { "=", "<=", ">=" });
                    StringCollection OrderList8 = new StringCollection();

                    OrderList8.Add("ORDER BY " + AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " ASC");

                    AccountingPeriodTable = AAccountingPeriodAccess.LoadUsingTemplate(TemplateRow8,
                        operators8,
                        null,
                        ADBTransaction,
                        OrderList8,
                        0,
                        0);

                    if (AccountingPeriodTable.Count == 0)
                    {
                        /*RUN x_table.p ("a_accounting_period":U).
                         * RUN s_errmsg.p ("X_0007":U,PROGRAM-NAME(1),lv_message,RETURN-VALUE).*/
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString("The accounting period for {0: d} does not exist in Ledger: {1}"),
                            ATestDate,
                            ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    MaxPeriod = CurrentPeriod + LedgerRow.NumberFwdPostingPeriods;

                    /* The date is beyond the current period - is it within the
                     *  fwd posting allowance? */

                    AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    int AccountPeriodNumber = AccountingPeriodRow.AccountingPeriodNumber;

                    if ((AccountPeriodNumber <= MaxPeriod)
                        && (AccountPeriodNumber >= CurrentPeriod))
                    {
                        AProcessingPeriod = AccountPeriodNumber;
                        ValidDateIndicator = true;
                    }
                    else
                    {
                        /* The date is past the maximum fwd posting date.
                         * RUN s_errmsg.p ("GL0014":U,PROGRAM-NAME(1),STRING(pv_test_date_d),STRING(lv_current_period)).*/
                        ErrorContext = "Data Validation";
                        ErrorMessage =
                            String.Format(Catalog.GetString(
                                    "This date {0} is beyond the current accounting period and the date allowed for forward processing for Ledger: {1}."),
                                ATestDate,
                                ALedgerNumber);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                }
                else
                {
                    AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    /* The date is within the current period. */
                    AProcessingPeriod = AccountingPeriodRow.AccountingPeriodNumber;
                    ValidDateIndicator = true;
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                ValidDateIndicator = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Generate Transactions";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating transactions for Ledger: {0}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                ValidDateIndicator = false;
            }

            return ValidDateIndicator;
        }

        /// <summary>
        /// Test that a transaction is valid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ATransactionDate"></param>
        /// <param name="ANarrative"></param>
        /// <param name="ATransactionCurrency"></param>
        /// <param name="ATransactionAmount"></param>
        /// <param name="AExchangeRateToBase"></param>
        /// <param name="AJournalPeriod"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="ADebitAccountCode"></param>
        /// <param name="ACreditAccountCode"></param>
        /// <param name="AAmountInBaseCurrency"></param>
        /// <param name="AAmountInBase2"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool ValidateTransaction(int ALedgerNumber,
            string ACostCentreCode,
            string AAccountCode,
            DateTime ATransactionDate,
            string ANarrative,
            string ATransactionCurrency,
            decimal ATransactionAmount,
            decimal AExchangeRateToBase,
            int AJournalPeriod,
            bool ADebitCreditIndicator,
            string ADebitAccountCode,
            string ACreditAccountCode,
            ref decimal AAmountInBaseCurrency,
            ref decimal AAmountInBase2,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //***gl4330.p <-gl1130fa.i <- gl1130.i <-gl2150.p
            bool ValidTransactionIndicator = false;

            bool ConversionOK;
            bool ValiddateIndicator;
            int ProcessingPeriod = 0;
            string BudgetKey;
            bool Answer = false;
            int GlmSeqThisYear;

            ACostCentreTable CostCentreTable = null;
            ACostCentreRow CostCentreRow = null;
            AAccountTable AccountTable = null;
            AAccountRow AccountRow = null;

            /*
             * {sm22002.i &TABLE = "aedger"}
             * {sm22002.i &TABLE = "a_accounting_period"}
             * {sm22002.i &TABLE = "aorporate_exchange_rate"}
             */

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            try
            {
                /* The transaction narrative cannot be blank. */
                if (ANarrative.Trim() == String.Empty)
                {
                    //RUN s_errmsg.p ("GL0055":U,PROGRAM-NAME(1),"Current Amount","").
                    ErrorContext = "Narrative Entry";
                    ErrorMessage = String.Format(Catalog.GetString("Narrative field cannot be empty."));
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                /* Check the date of this transaction. */

                /* 0003 Check trans date is now back in use. May be different than
                 *  the journal date, as long as it's in the same period. */

                //***Run gl4320
                ValiddateIndicator = CheckForValidDate(ALedgerNumber,
                    ATransactionDate,
                    ref ProcessingPeriod,
                    ADBTransaction,
                    ref AVerificationResult
                    );

                if (!ValiddateIndicator)
                {
                    ErrorContext = "Date Validation";
                    ErrorMessage = String.Format(Catalog.GetString("This date {0} is invalid."), ATransactionDate);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                if (ProcessingPeriod != AJournalPeriod)
                {
                    /* The processing period must match the journal period. */
                    /*RUN s_errmsg.p ("GL0016":U,PROGRAM-NAME(1),STRING(ProcessingPeriod),STRING(pv_journal_period)).*/
                    ErrorContext = "Date Validation";
                    ErrorMessage = String.Format(Catalog.GetString("This date {0} is invalid."), ATransactionDate);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                if (!CheckCurrencyExists(ATransactionCurrency, ADBTransaction))
                {
                    /* The processing period must match the journal period. */
                    /*RUN s_errmsg.p ("GL0016":U,PROGRAM-NAME(1),STRING(ProcessingPeriod),STRING(pv_journal_period)).*/
                    ErrorContext = "Currency Validation";
                    ErrorMessage = String.Format(Catalog.GetString("Currency code: {0} does not exist."), ATransactionCurrency);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                /* The cost centre exists and must be posting. */
                CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentreCode, ADBTransaction);

                if (CostCentreTable.Count == 0)
                {
                    ErrorContext = "Cost Centre Validation";
                    ErrorMessage = String.Format(Catalog.GetString("The Cost centre (Ledger = {0}, Code = {1}) does not exist."),
                        ALedgerNumber,
                        ACostCentreCode
                        );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];

                if (CostCentreRow.PostingCostCentreFlag != true) /* {&POSTING} - set to true*/
                {
                    //RUN s_errmsg.p ("GL0017":U, PROGRAM-NAME(1),lv_message,"cost centre").
                    //The &1 is not a posting &2. The transaction cannot be posted.
                    ErrorContext = "Cost Centre Validation";
                    ErrorMessage =
                        String.Format(Catalog.GetString(
                                "The Cost centre (Ledger = {0}, Code = {1}) is not a posting cost centre. The transaction cannot be posted."),
                            ALedgerNumber,
                            ACostCentreCode
                            );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                /* Check for a blank account code. */
                if (AAccountCode == string.Empty)
                {
                    if (ADebitCreditIndicator == true)  /* DEBIT */
                    {
                        AAccountCode = ADebitAccountCode;
                    }
                    else
                    {
                        AAccountCode = ACreditAccountCode;
                    }
                }

                /* Check that the account code exists. */
                if (!ACostCentreAccess.Exists(ALedgerNumber, AAccountCode, ADBTransaction))
                {
                    //RUN s_errmsg.p ("X_0007":U,PROGRAM-NAME(1),lv_message).
                    ErrorContext = "Account Validation";
                    ErrorMessage = String.Format(Catalog.GetString("Account (Ledger = {0}, Account code = {1}) does not exist in the Account table."),
                        ALedgerNumber,
                        AAccountCode
                        );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                //Run gl4350.p
                ConversionOK = CalculateTransactionAmount(ALedgerNumber,
                    ATransactionCurrency,
                    ATransactionAmount,
                    ATransactionDate,
                    AExchangeRateToBase,
                    ref AAmountInBaseCurrency,
                    ref AAmountInBase2,
                    ADBTransaction,
                    ref AVerificationResult
                    );

                if (!ConversionOK)
                {
                    //Error in calculate the transaction amount in base currency
                    ErrorContext = "Currency Conversion";
                    ErrorMessage = Catalog.GetString("Error in calculating the transaction amount in base currency.");
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AAccountCode, ADBTransaction);

                if (AccountTable.Count == 0)
                {
                    ErrorContext = "Cost Centre Validation";
                    ErrorMessage = String.Format(Catalog.GetString("The Account (Ledger = {0}, Code = {1}) does not exist."),
                        ALedgerNumber,
                        AAccountCode
                        );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                AccountRow = (AAccountRow)AccountTable.Rows[0];

                /* If budget control is set check for a valid budget key. */
                if (AccountRow.BudgetControlFlag)
                {
                    ABudgetTable BudgetTb = new ABudgetTable();
                    ABudgetRow TemplateRow7 = (ABudgetRow)BudgetTb.NewRowTyped(false);

                    TemplateRow7.LedgerNumber = ALedgerNumber;
                    TemplateRow7.AccountCode = AAccountCode;
                    TemplateRow7.CostCentreCode = ACostCentreCode;

                    StringCollection operators7 = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                    ABudgetTable BudgetTable = ABudgetAccess.LoadUsingTemplate(TemplateRow7, operators7, null, ADBTransaction);

                    if (BudgetTable.Count == 0)
                    {
                        //RUN s_errmsg.p ("X_0007":U,PROGRAM-NAME(1),lv_message).
                        //TODO: change to option dialog
                        ErrorContext = "Budget Validation";
                        ErrorMessage =
                            String.Format(Catalog.GetString(
                                    "There is no budget information for this account/cost centre combination (Cost Centre = {0} Account code = {1})."
                                    +
                                    "Do you want to continue?"),
                                ACostCentreCode,
                                AAccountCode
                                );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    /* Check if amount will go over budget. */
                    GlmSeqThisYear = GetGlmSequence(ALedgerNumber, AAccountCode, ACostCentreCode, LedgerRow.CurrentFinancialYear, ADBTransaction);

                    if ((AAmountInBaseCurrency + GetActual(ALedgerNumber,
                             GlmSeqThisYear,
                             -1,
                             ProcessingPeriod,
                             LedgerRow.NumberOfAccountingPeriods,
                             LedgerRow.CurrentFinancialYear,
                             LedgerRow.CurrentFinancialYear,
                             true,
                             "B",
                             ADBTransaction
                             )) > GetBudget(GlmSeqThisYear,
                            -1,
                            ProcessingPeriod,
                            LedgerRow.NumberOfAccountingPeriods,
                            true,
                            "B",
                            ADBTransaction
                            ))
                    {
                        BudgetKey = "Cost Center " + CostCentreRow.CostCentreName + ", Account " + AccountRow.AccountCodeShortDesc;
                        string Message = "This transaction will cause the budget for" + BudgetKey + " to be exceeded.  Do you want to continue?";
                        //bool lv_answer = false;
                        //VIEW-AS ALERT-BOX WARNING BUTTONS YES-NO
                        //UPDATE lv_answer.

                        if (!Answer)
                        {
                            //return
                        }
                    }
                }

                if (!AccountRow.PostingStatus)
                {
                    //"GL0017":U,PROGRAM-NAME(1),lv_message,"account").
                    ErrorContext = "Account Validation";
                    ErrorMessage =
                        String.Format(Catalog.GetString(
                                "The Account (Ledger = {0}, Code = {1}) is not a posting account. The transaction cannot be posted."),
                            ALedgerNumber,
                            AAccountCode
                            );
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                ValidTransactionIndicator = true;
                //***End of gl4330.p***
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                ValidTransactionIndicator = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Generate Transactions";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating transactions for Ledger: {0}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                ValidTransactionIndicator = false;
            }

            return ValidTransactionIndicator;
        }

        /// <summary>
        /// Calculates the amount of the transaction if it is different from the
        ///  base currency.  Otherwise it returns the same amount.  Also calculates
        ///  the amount converted to a second base currency (int'l) if there is one.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATransactionCurrency">Transaction Currency</param>
        /// <param name="ATransactionAmount">Transaction Amount</param>
        /// <param name="ATransactionDate">Transaction Date</param>
        /// <param name="AExchangeRateToBase">Exchange Rate to Base Currency</param>
        /// <param name="ATransactionAmountInBase">Ref: Transaction Amount in Base Currency</param>
        /// <param name="AAmountInBase2">Ref: Amount in Base Currency 2</param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool CalculateTransactionAmount(int ALedgerNumber,
            string ATransactionCurrency,
            decimal ATransactionAmount,
            DateTime ATransactionDate,
            decimal AExchangeRateToBase,
            ref decimal ATransactionAmountInBase,
            ref decimal AAmountInBase2,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //***RUN gl4350.p <- gl4330.p <- gl1130fa.i <- gl1130.i <-gl2150.p

            //Return
            bool IsSuccessful = false;

            int NumDecPlaces2 = 2;

            AAccountingPeriodTable AccountingPeriodTable = null;
            AAccountingPeriodRow AccountingPeriodRow = null;

            ACorporateExchangeRateTable CorporateExchangeRateTable = null;
            ACorporateExchangeRateRow CorporateExchangeRateRow = null;

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            string IntlCurrency = LedgerRow.IntlCurrency;
            string BaseCurrency = LedgerRow.BaseCurrency;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                /* Calculate the transaction amount in base currency. */
                if (IntlCurrency != String.Empty)
                {
                    ACurrencyTable CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(IntlCurrency, ADBTransaction);
                    ACurrencyRow CurrencyRow = (ACurrencyRow)CurrencyTable.Rows[0];

                    NumDecPlaces2 = THelperNumeric.CalcNumericFormatDecimalPlaces(CurrencyRow.DisplayFormat);
                }

                /* If the transaction currency is different from the base,
                 *  calculate the base amount according to the exchange rate. */
                if (ATransactionCurrency != BaseCurrency)
                {
                    //RUN a_eurcon.p (ATransactionAmount,
                    ATransactionAmountInBase = ConvertBetweenEuropeanCurrencies(ATransactionAmount,
                        AExchangeRateToBase,
                        ATransactionCurrency,
                        LedgerRow.BaseCurrency,
                        ADBTransaction,
                        ref AVerificationResult
                        );
                }
                else
                {
                    ATransactionAmountInBase = ATransactionAmount;
                }

                if (IntlCurrency == String.Empty)
                {
                    /* No Int'l currency */
                    AAmountInBase2 = 0;
                    IsSuccessful = true;
                }
                else if (ATransactionCurrency == IntlCurrency)
                {
                    /* Case 1: Transaction currency = Int'l currency */
                    AAmountInBase2 = ATransactionAmount;
                    IsSuccessful = true;
                }
                else if (BaseCurrency == IntlCurrency)
                {
                    /* Case 1b: Base currency = Int'l currency */
                    AAmountInBase2 = ATransactionAmountInBase;
                    IsSuccessful = true;
                }
                else
                {
                    /* Case 2: Int'l currency must be converted from the base amount. */
                    /* Find the latest corp exchange rate from base to intl. */
                    if (LedgerRow.ProvisionalYearEndFlag)
                    {
                        AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                            LedgerRow.NumberOfAccountingPeriods,
                            ADBTransaction);
                    }
                    else
                    {
                        AAccountingPeriodTable AccountingPeriodTb = new AAccountingPeriodTable();
                        AAccountingPeriodRow TemplateRow9 = (AAccountingPeriodRow)AccountingPeriodTb.NewRowTyped(false);

                        TemplateRow9.LedgerNumber = ALedgerNumber;
                        TemplateRow9.PeriodStartDate = ATransactionDate;
                        TemplateRow9.PeriodEndDate = ATransactionDate;

                        StringCollection operators9 = StringHelper.InitStrArr(new string[] { "=", "<=", ">=" });
                        StringCollection OrderList = new StringCollection();

                        OrderList.Add("ORDER BY " + AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " ASC");

                        AccountingPeriodTable = AAccountingPeriodAccess.LoadUsingTemplate(TemplateRow9,
                            operators9,
                            null,
                            ADBTransaction,
                            OrderList,
                            0,
                            0);
                    }

                    if (AccountingPeriodTable.Count == 0)
                    {
                        ErrorContext = "Accounting Period";
                        ErrorMessage = String.Format(Catalog.GetString("Accounting Period {0} cannot be found for Ledger: {1} for date: {2:d}."),
                            AccountingPeriodRow.AccountingPeriodNumber,
                            ALedgerNumber,
                            ATransactionDate
                            );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    ACorporateExchangeRateTable CorporateExchangeRateTb = new ACorporateExchangeRateTable();
                    ACorporateExchangeRateRow TemplateRow0 = (ACorporateExchangeRateRow)CorporateExchangeRateTb.NewRowTyped(false);

                    TemplateRow0.FromCurrencyCode = BaseCurrency;
                    TemplateRow0.ToCurrencyCode = IntlCurrency;
                    TemplateRow0.DateEffectiveFrom = AccountingPeriodRow.PeriodStartDate;
                    //TemplateRow0.DateEffectiveFrom = AccountingPeriodRow.PeriodEndDate;

                    StringCollection operators0 = StringHelper.InitStrArr(new string[] { "=", "=", ">=" });
                    StringCollection OrderList0 = new StringCollection();

                    OrderList0.Add("ORDER BY " + ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + " DESC");

                    CorporateExchangeRateTable = ACorporateExchangeRateAccess.LoadUsingTemplate(TemplateRow0,
                        operators0,
                        null,
                        ADBTransaction,
                        OrderList0,
                        0,
                        0);

                    bool CorpExchRateFound = false;

                    for (int b = 0; b < CorporateExchangeRateTable.Count; b++)
                    {
                        CorporateExchangeRateRow = (ACorporateExchangeRateRow)CorporateExchangeRateTable.Rows[b];

                        if (CorporateExchangeRateRow.DateEffectiveFrom <= AccountingPeriodRow.PeriodEndDate)
                        {
                            CorpExchRateFound = true;
                            AAmountInBase2 = Math.Round(ATransactionAmountInBase / CorporateExchangeRateRow.RateOfExchange, NumDecPlaces2);
                            IsSuccessful = true;
                            break;
                        }
                    }

                    if (!CorpExchRateFound)
                    {
                        /* message "ERROR: No current corporate rate for the int'l currency!"
                         *  "Please enter a rate for period"
                         *  a_accounting_period.a_accounting_periodumber.
                         */
                        //RUN s_errmsg.p ("GL0070":U, PROGRAM-NAME(1), STRING(a_accounting_period.a_accounting_periodumber), "").  /* 0002 */
                        ErrorContext = "Accounting Period";
                        ErrorMessage =
                            String.Format(Catalog.GetString(
                                    "There is no corporate exchange rate for converting to the international currency Please enter a rate for period {0}."),
                                AccountingPeriodRow.AccountingPeriodNumber
                                );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsSuccessful = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Generate Transactions";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating transactions for Ledger: {0}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsSuccessful = false;
            }

            return IsSuccessful;
        }

        /// <summary>
        /// Post a batch consisting of journals
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="APrintReport"></param>
        /// <returns></returns>
        private bool PostNewBatch(int ALedgerNumber, int ABatchNumber, bool APrintReport)
        {
            //gl1210.p
            bool IsSuccessful = false;

            //TODO

            return IsSuccessful;
        }

        /// <summary>
        /// Relates to Petra 4GL module gl1130.i
        /// </summary>
        /// <param name="AMode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="ANarrative"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="ATransactionAmount"></param>
        /// <param name="ATransactionDate"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="ADebitAccount"></param>
        /// <param name="ACreditAccount"></param>
        /// <param name="AHeaderNumber"></param>
        /// <param name="ADetailNumber"></param>
        /// <param name="ASubType"></param>
        /// <param name="AReference"></param>
        /// <param name="ASystemGenerated"></param>
        /// <param name="ABaseAmount"></param>
        /// <param name="ATransactionNumberReturned"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool CreateTransactions(string AMode,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            int ATransactionNumber,
            string ANarrative,
            string AAccountCode,
            string ACostCentreCode,
            decimal ATransactionAmount,
            DateTime ATransactionDate,
            bool ADebitCreditIndicator,
            string ADebitAccount,
            string ACreditAccount,
            int AHeaderNumber,
            int ADetailNumber,
            string ASubType,
            string AReference,
            bool ASystemGenerated,
            decimal ABaseAmount,
            ref int ATransactionNumberReturned,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //RUN gl1130.i <- gl1130o.p <- gl2150.p
            bool TransactionCreatedOK = false;

            bool IsNewRecord = false;
            decimal AExchangeRateToBase;
            string ATransactionCurrency;
            decimal AmountInBaseCurrency = 0;
            decimal AmountInBase2 = 0;

            decimal CurrentAmount = 0;
            bool IsValid = true;
            bool AssignAttributeOK = true;

            int TransNumberToReturn = 0;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            ABatchTable BatchTable = null;
            ABatchRow BatchRow = null;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            try
            {
                AJournalTable JournalTable = AJournalAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ADBTransaction);
                AJournalRow JournalRow = (AJournalRow)JournalTable.Rows[0];

                AExchangeRateToBase = JournalRow.ExchangeRateToBase;
                ATransactionCurrency = JournalRow.TransactionCurrency;

                ATransactionTable TransTable = null;
                ATransactionRow TransRow = null;
                ATransactionRow TransRowNew = null;

                if ((AMode.ToUpper() == "NEW") || (AMode.ToUpper() == "NEW-INQUIRE-ALL"))
                {
                    /*FIND Petra._file WHERE Petra._file._FILE-NAME = "a_{&TYPE}transaction"
                     * AND Petra._file._Creator = 'PUB' /* M001
                     *
                     * FIND FIRST a_{&TYPE}transaction WHERE
                     * RECID(a_{&TYPE}transaction) = Petra._file._template*/

                    /* Set the new record flag. */
                    IsNewRecord = true;
                    ATransactionNumber = JournalRow.LastTransactionNumber + 1;
                }
                else
                {
                    TransTable = ATransactionAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, ADBTransaction);
                    TransRow = (ATransactionRow)TransTable.Rows[0];

                    if (TransTable.Count == 0)
                    {
                        //TODO: apply OP locking policies
                        bool TransactionRowLocked = false;

                        if (!TransactionRowLocked)
                        {
                            ErrorContext = "Transaction Creation";
                            ErrorMessage = Catalog.GetString("Unable to edit this record, transaction record already deleted by another user.");
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }
                        else
                        {
                            ErrorContext = "Transaction Creation";
                            ErrorMessage = Catalog.GetString("The record could not be locked. Changes were NOT saved. Please try again later.");
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }
                    }

                    //***Include gl1130fa.i***
                    if (ANarrative.Trim() == string.Empty)
                    {
                        //TODO: Set focus to the narrative field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0002":U,PROGRAM-NAME(1),"Narrative","").
                        ErrorContext = "Data Validation";
                        ErrorMessage = Catalog.GetString("Field cannot be blank.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    /* M006 Only allow blank Reference if System Parameter allows it */
                    //TODO: find system default for s_system_parameter.a_gl_ref_mandatory
                    else if ((AReference == string.Empty) && (TSystemDefaults.GetSystemDefault(SharedConstants.PETRAMODULE_FINANCE1) == "Something"))
                    {
                        //TODO: Set focus to the Reference field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0002":U,PROGRAM-NAME(1),"Reference","").
                        ErrorContext = "Data Validation";
                        ErrorMessage = Catalog.GetString("Field cannot be blank.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    /* 0001 Get the cc & ac for validation. */
                    ACostCentreTable CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentreCode, ADBTransaction);
                    AAccountTable AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AAccountCode, ADBTransaction);

                    /* 0004 Make sure cc & ac exist. */
                    if (CostCentreTable.Count == 0)
                    {
                        //TODO: Set focus to the Cost Centre field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0007":U, PROGRAM-NAME(1), ACostCentreCode, "Cost Centre")
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "The cost centre: '{0}' does not exist in the Cost Centre table."), ACostCentreCode);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    else if (AccountTable.Count == 0)
                    {
                        //TODO: Set focus to the Account field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0007":U, PROGRAM-NAME(1), AAccountCode, "Account Master")
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "The account: '{0}' does not exist in the Account Master table."), AAccountCode);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];
                    AAccountRow AccountRow = (AAccountRow)AccountTable.Rows[0];

                    string ValidCcCombo = AccountRow.ValidCcCombo.ToUpper();
                    string AccountCode = AccountRow.AccountCode;
                    string CostCentreType = CostCentreRow.CostCentreType.ToUpper();
                    string CostCentreCode = CostCentreRow.CostCentreCode;

                    /* 0001 Check for a valid cost centre/account combination. */
                    if ((ValidCcCombo != "ALL") && (ValidCcCombo != CostCentreType))
                    {
                        //TODO: Set focus to the Account Code field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0007":U, PROGRAM-NAME(1), AAccountCode, "Account Master")
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString("The account code type {0} does not match the cost centre type {1}." +
                                "Please choose a valid combination."),
                            ValidCcCombo + " [" + AccountCode + "]",
                            CostCentreType + " [" + CostCentreCode + "]"
                            );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    /* 0004 Check if the cost centre & account are active. */
                    if (!CostCentreRow.CostCentreActiveFlag)
                    {
                        //TODO: Set focus to the Cost Centre field and put in text: "ENTRY:"
                        //TODO: change to option dialog
                        //RUN s_errmsg.p ("X_0035":U, PROGRAM-NAME(1), a_cost_centre_code, "")
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "The code: '{0}' is no longer active. Do you still want to use it?"), CostCentreCode);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    else if (!AccountRow.AccountActiveFlag)
                    {
                        //TODO: Set focus to the Cost Centre field and put in text: "ENTRY:"
                        //TODO: change to option dialog
                        //RUN s_errmsg.p ("X_0035":U, PROGRAM-NAME(1), a_account_code, "")
                        ErrorContext = "Data Validation";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "The code: '{0}' is no longer active. Do you still want to use it?"), AccountCode);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    if (ATransactionDate == null)
                    {
                        //TODO: Set focus to the Date Effective field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0002":U,PROGRAM-NAME(1),"Date Effective","").
                        ErrorContext = "Data Validation";
                        ErrorMessage = Catalog.GetString("Field cannot be blank.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    //TODO: find the relationship between next two vars. as per gl1130fa.i, line s189-193 see x_gtzero.i

                    /* The transaction amount must be greater than zero. */
                    if (ATransactionAmount <= 0)
                    {
                        //TODO: Set focus to the Transaction Amount field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0008":U,PROGRAM-NAME(1),"Transaction Amount","").
                        ErrorContext = "Data Validation";
                        ErrorMessage = Catalog.GetString("The transaction amount must be greater than zero.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    else if (CurrentAmount <= 0)
                    {
                        //TODO: Set focus to the Current Amount field and put in text: "ENTRY:"
                        //RUN s_errmsg.p ("X_0008":U,PROGRAM-NAME(1),"Current Amount","").
                        ErrorContext = "Data Validation";
                        ErrorMessage = Catalog.GetString("The current amount must be greater than zero.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    for (int k = 0; k < AccountTable.Count; k++)
                    {
                        AAccountRow aRow = (AAccountRow)AccountTable.Rows[k];

                        if (aRow.ForeignCurrencyFlag)
                        {
                            if (JournalTable.Count > 0)
                            {
                                if (!((JournalRow.SubSystemCode.ToUpper() == "GL") && (JournalRow.TransactionTypeCode.ToUpper() == "REVAL"))
                                    && (AccountRow.ForeignCurrencyCode != JournalRow.TransactionCurrency))
                                {
                                    //TODO: Set focus to the Account Code field and put in text: "ENTRY:"
                                    //RUN s_errmsg.p ("GL0055":U,PROGRAM-NAME(1),"Current Amount","").
                                    ErrorContext = "Data Validation";
                                    ErrorMessage =
                                        String.Format(Catalog.GetString(
                                                "Account {0} cannot be used because it is a foreign currency account. Create a journal using {1} instead."),
                                            AAccountCode,
                                            AccountRow.ForeignCurrencyCode
                                            );
                                    ErrorType = TResultSeverity.Resv_Noncritical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }
                            }

                            break;
                        }
                    }

                    if (IsValid)
                    {
                        //***RUN gl4330.p***
                        IsValid = ValidateTransaction(ALedgerNumber,
                            ACostCentreCode,
                            AAccountCode,
                            ATransactionDate,
                            ANarrative,
                            JournalRow.TransactionCurrency,
                            ATransactionAmount,
                            AExchangeRateToBase,
                            JournalRow.JournalPeriod,
                            ADebitCreditIndicator,
                            ADebitAccount,
                            ACreditAccount,
                            ref AmountInBaseCurrency,
                            ref AmountInBase2,
                            ADBTransaction,
                            ref AVerificationResult
                            );

                        if (IsValid && (JournalRow.SubSystemCode.ToUpper() == "GL") && (JournalRow.TransactionTypeCode.ToUpper() == "REVEAL"))
                        {
                            AmountInBase2 = 0;
                        }
                    }

                    if (IsValid)
                    {
                        if (CostCentreRow.ProjectStatus)
                        {
                            AGeneralLedgerMasterTable GeneralLedgerMasterTb = new AGeneralLedgerMasterTable();
                            AGeneralLedgerMasterRow TemplateRow3 = (AGeneralLedgerMasterRow)GeneralLedgerMasterTb.NewRowTyped(false);

                            TemplateRow3.LedgerNumber = ALedgerNumber;
                            TemplateRow3.AccountCode = AAccountCode;
                            TemplateRow3.CostCentreCode = ACostCentreCode;

                            StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                            AGeneralLedgerMasterTable GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(TemplateRow3,
                                operators3,
                                null,
                                ADBTransaction);
                            AGeneralLedgerMasterRow GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];

                            if (GeneralLedgerMasterTable.Count > 0)
                            {
                                CurrentAmount = GeneralLedgerMasterRow.YtdActualBase + AmountInBaseCurrency;

                                if (CostCentreRow.ProjectConstraintAmount < CurrentAmount)
                                {
                                    //TODO: change to option dialog
                                    //Project will exceed constraint
                                    //RUN s_errmsg.p ("GL0050":U, PROGRAM-NAME(1), ACostCentreCode, "")
                                    ErrorContext = "Data Validation";
                                    ErrorMessage =
                                        String.Format(Catalog.GetString(
                                                "This transaction will exceed the given project financial constraint for: '{0}' "
                                                +
                                                "(Limit: {1} Current: {2})" +
                                                Environment.NewLine + Environment.NewLine + "Do you want to continue anyway?"),
                                            ACostCentreCode,
                                            CostCentreRow.ProjectConstraintAmount,
                                            CurrentAmount
                                            );
                                    ErrorType = TResultSeverity.Resv_Noncritical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }

                                if (CostCentreRow.ProjectConstraintDate < ATransactionDate)
                                {
                                    //Project will exceed constraint
                                    //RUN s_errmsg.p ("GL0051":U, PROGRAM-NAME(1), ACostCentreCode, "")
                                    ErrorContext = "Data Validation";
                                    ErrorMessage =
                                        String.Format(Catalog.GetString("This transaction will exceed the given project time constraint for: '{0}' "
                                                +
                                                "(Limit: {1:d} Current: {2:d})" +
                                                Environment.NewLine + Environment.NewLine + "Do you want to continue anyway?"),
                                            ACostCentreCode,
                                            CostCentreRow.ProjectConstraintDate,
                                            ATransactionDate
                                            );
                                    ErrorType = TResultSeverity.Resv_Noncritical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }
                            }

                            if (IsValid)
                            {
                                if (ABatchNumber != 0)
                                {
                                    BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ADBTransaction);
                                    //TODO: check if Batch table is locked, Journal is already open
                                    //RUN s_errmsg.p ("X_0017":U,PROGRAM-NAME(1),"Batch","").
//                                  ErrorContext = "Data Validation";
//                                  ErrorMessage = "The batch record could not be locked. Changes were NOT saved. Please try again later.";
//                                  ErrorType = TResultSeverity.Resv_Noncritical;
//                                  throw new System.InvalidOperationException(ErrorMessage);
                                }

                                if (BatchTable.Count == 0)
                                {
                                    ErrorContext = "Access Batch Table";
                                    ErrorMessage = String.Format(Catalog.GetString("Cannot find batch: {1} in Ledger: {0}"),
                                        ALedgerNumber,
                                        ABatchNumber);
                                    ErrorType = TResultSeverity.Resv_Noncritical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }

                                BatchRow = (ABatchRow)BatchTable.Rows[0];

                                /* If this is a modification, subtract the previous amounts from the
                                 *  totals in the journal & batch record. */

                                if (!IsNewRecord)
                                {
                                    if (TransRow.DebitCreditIndicator)
                                    {
                                        JournalRow.JournalDebitTotal -= TransRow.TransactionAmount;

                                        if (ABatchNumber != 0)
                                        {
                                            BatchRow.BatchDebitTotal -= TransRow.TransactionAmount;
                                        }
                                    }
                                    else
                                    {
                                        JournalRow.JournalCreditTotal -= TransRow.TransactionAmount;

                                        if (ABatchNumber != 0)
                                        {
                                            BatchRow.BatchCreditTotal -= TransRow.TransactionAmount;
                                        }
                                    }
                                }

                                if (ADebitCreditIndicator)
                                {
                                    JournalRow.JournalDebitTotal += ATransactionAmount;

                                    if (ABatchNumber != 0)
                                    {
                                        BatchRow.BatchDebitTotal += ATransactionAmount;
                                    }
                                }
                                else
                                {
                                    JournalRow.JournalCreditTotal += ATransactionAmount;

                                    if (ABatchNumber != 0)
                                    {
                                        BatchRow.BatchCreditTotal += ATransactionAmount;
                                    }
                                }

                                /* If new record, update the aast_transaction_number in the journal file. */
                                if (IsNewRecord)
                                {
                                    JournalRow.LastTransactionNumber = ATransactionNumber;
                                }

                                if (AAccountCode != TransRow.AccountCode)
                                {
                                    ATransAnalAttribTable TransAnalAttribTb = new ATransAnalAttribTable();
                                    ATransAnalAttribRow TemplateRow4 = (ATransAnalAttribRow)TransAnalAttribTb.NewRowTyped(false);

                                    TemplateRow4.LedgerNumber = ALedgerNumber;
                                    TemplateRow4.BatchNumber = ABatchNumber;
                                    TemplateRow4.JournalNumber = AJournalNumber;
                                    TemplateRow4.TransactionNumber = ATransactionNumber;
                                    TemplateRow4.AccountCode = AAccountCode;

                                    StringCollection operators4 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=", "=" });

                                    ATransAnalAttribTable TransAnalAttribTable = ATransAnalAttribAccess.LoadUsingTemplate(TemplateRow4,
                                        operators4,
                                        null,
                                        ADBTransaction);
                                    ATransAnalAttribRow TransAnalAttribRow = null;

                                    //TODO: check if TransAnalAttribTable table is locked
                                    //RUN s_errmsg.p ("X_0017":U,PROGRAM-NAME(1),"Batch","").
//                                  ErrorContext = "Account Attribute Check";
//                                  ErrorMessage = "The Transaction Analysis Attribute Table record could not be locked. Changes were NOT saved. Please try again later.";
//                                  ErrorType = TResultSeverity.Resv_Noncritical;
//                                  throw new System.InvalidOperationException(ErrorMessage);

                                    if (TransAnalAttribTable.Count > 0)
                                    {
                                        for (int m = TransAnalAttribTable.Count - 1; m >= 0; m--)
                                        {
                                            TransAnalAttribRow = (ATransAnalAttribRow)TransAnalAttribTable.Rows[m];

                                            //Delete rows from the top to keep the for loop index m coherent
                                            TransAnalAttribRow.Delete();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TransactionCreatedOK = false;
                                return TransactionCreatedOK;
                            }

                            //***end if gl1130fa.i***
                        }
                    }

                    /* If necessary, balance the base currency amounts. */

                    /* Currency conversions are not done on recurring transactions
                     * until they are submitted as regular transactions. */
                    //TODO: find what this does &IF "{&TYPE}" EQ "" &THEN    /* 0005 */

                    if (ABaseAmount != 0)
                    {
                        AmountInBaseCurrency = ABaseAmount;
                    }
                    else
                    {
                        //Run RUN gl4380o.p & gl4380o2.p from gl1130.i
                        BalanceAmountsInBaseCurrency("AmountInBaseCurrency",
                            ALedgerNumber,
                            ABatchNumber,
                            AJournalNumber,
                            -1,
                            -1,
                            ADebitCreditIndicator,
                            TransRow.DebitCreditIndicator,
                            TransRow.AmountInBaseCurrency,
                            ref AmountInBaseCurrency,
                            ADBTransaction,
                            AVerificationResult);
                    }

                    BalanceAmountsInBaseCurrency("AmountInIntlCurrency",
                        ALedgerNumber,
                        ABatchNumber,
                        AJournalNumber,
                        -1,
                        -1,
                        ADebitCreditIndicator,
                        TransRow.DebitCreditIndicator,
                        TransRow.AmountInIntlCurrency,
                        ref AmountInBaseCurrency,
                        ADBTransaction,
                        AVerificationResult);

                    if (IsValid)
                    {
                        if (AMode.ToUpper() == "NEW")
                        {
                            TransRowNew = TransTable.NewRowTyped();
                        }
                        else
                        {
                            //TODO: gl1130.i 192fflocate specific record. Don't know if doing thie approach

                            /*lv_selected_recordd_r = RECID(a_{&TYPE}transaction).
                             *  FIND FIRST a_{&TYPE}transaction
                             *  WHERE RECID(a_{&TYPE}transaction) = lv_selected_recordd_r
                             *  EXCLUSIVE-LOCK NO-WAIT NO-ERROR.*/
                            TransRowNew = TransTable.NewRowTyped(); //this will eventually change to a LoadByPrimaryKey or similar
                        }

                        TransTable.Rows.Add(TransRowNew);
                        //first two fields not used in recurring batches
                        TransRowNew.SystemGenerated = ASystemGenerated;
                        TransRowNew.AmountInIntlCurrency = AmountInBase2;

                        TransRowNew.LedgerNumber = ALedgerNumber;
                        TransRowNew.BatchNumber = ABatchNumber;
                        TransRowNew.JournalNumber = AJournalNumber;
                        TransRowNew.TransactionNumber = ATransactionNumber;
                        TransRowNew.Narrative = ANarrative;
                        TransRowNew.AccountCode = AAccountCode;
                        TransRowNew.CostCentreCode = ACostCentreCode;
                        TransRowNew.TransactionAmount = ATransactionAmount;
                        TransRowNew.AmountInBaseCurrency = AmountInBaseCurrency;
                        TransRowNew.TransactionDate = ATransactionDate;
                        TransRowNew.DebitCreditIndicator = ADebitCreditIndicator;
                        TransRowNew.HeaderNumber = AHeaderNumber;
                        TransRowNew.DetailNumber = ADetailNumber;
                        TransRowNew.SubType = ASubType;
                        TransRowNew.Reference = AReference;

                        AAnalysisAttributeTable AnalysisAttributeTb = new AAnalysisAttributeTable();
                        AAnalysisAttributeRow TemplateRow = (AAnalysisAttributeRow)AnalysisAttributeTb.NewRowTyped(false);

                        TemplateRow.LedgerNumber = ALedgerNumber;
                        TemplateRow.AccountCode = AAccountCode;
                        TemplateRow.Active = true;

                        StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                        AAnalysisAttributeTable AnalysisAttributeTable = AAnalysisAttributeAccess.LoadUsingTemplate(TemplateRow,
                            operators,
                            null,
                            ADBTransaction);
                        AAnalysisAttributeRow AnalysisAttributeRow = null;

                        //AAccountTable Ledger & AccountCode Pks
                        AAccountTable AccountTb = new AAccountTable();
                        AAccountRow TemplateRow2 = (AAccountRow)AccountTb.NewRowTyped(false);

                        TemplateRow2.LedgerNumber = ALedgerNumber;
                        TemplateRow2.AccountCode = AAccountCode;
                        TemplateRow2.AnalysisAttributeFlag = true;

                        StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                        AAccountTable AccountTable2 = AAccountAccess.LoadUsingTemplate(TemplateRow2, operators2, ADBTransaction);

                        //&IF DEFINED(ANALYSIS-SUPPLIED) EQ 0 &THEN gl1130.i
                        if (!LedgerRow.ProvisionalYearEndFlag)
                        {
                            for (int i = 0; i < AnalysisAttributeTable.Count; i++)
                            {
                                AnalysisAttributeRow = (AAnalysisAttributeRow)AnalysisAttributeTable.Rows[i];

                                if (AccountTable2.Rows.Count == 0)
                                {
                                    ErrorContext = "Transaction Creation";
                                    ErrorMessage = String.Format(Catalog.GetString("Could not find any analysis Attributes for Account {0}. " +
                                            "Changes were NOT saved. Please try again later."),
                                        AAccountCode
                                        );
                                    ErrorType = TResultSeverity.Resv_Critical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }

                                /*TODO: - for non-recurring batch
                                 * RUN gl1151e.w (a_{&TYPE}transaction.aedgerumber,
                                 * a_{&TYPE}transaction.a_batch_number,
                                 * a_{&TYPE}transaction.a_journal_number,
                                 * a_{&TYPE}transaction.a_transaction_number,
                                 * a_{&TYPE}transaction.a_account_code,
                                 * a_analysis_attribute.a_analysis_typeode,
                                 * OUTPUT AssignAttributeOK). */

                                /*If recurring batch run this instead of the above
                                 * RUN gl1151re.w (a_{&TYPE}transaction.aedgerumber,
                                 * a_{&TYPE}transaction.a_batch_number,
                                 * a_{&TYPE}transaction.a_journal_number,
                                 * a_{&TYPE}transaction.a_transaction_number,
                                 * a_{&TYPE}transaction.a_account_code,
                                 * a_analysis_attribute.a_analysis_typeode,
                                 * OUTPUT AssignAttributeOK). */

                                if (!AssignAttributeOK)
                                {
                                    //RUN s_errmsg.p ("GL0061":U,
                                    //The required analysis attribute &1 has not been entered for account &2 Transaction has not been created.
                                    ErrorContext = "Transaction Creation";
                                    ErrorMessage =
                                        String.Format(Catalog.GetString("The required analysis attribute: {0} has not been entered for account: {1}."
                                                +
                                                Environment.NewLine + Environment.NewLine + "The transaction has not been created."),
                                            AnalysisAttributeRow.AnalysisTypeCode,
                                            AnalysisAttributeRow.AnalysisTypeCode
                                            );
                                    ErrorType = TResultSeverity.Resv_Critical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }
                            }
                        }
                    }
                }

                //TODO assign the correct Trans number to TransNumberToReturn
                ATransactionNumberReturned = TransNumberToReturn;
                TransactionCreatedOK = true;
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsValid = false;
                TransactionCreatedOK = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Create Transaction";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while creating a transaction for Ledger: {0} " +
                        " Batch No.: {1}, Journal No.: {2}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber,
                    ABatchNumber,
                    AJournalNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsValid = false;
                TransactionCreatedOK = false;
            }

            return TransactionCreatedOK;
        }

        /// <summary>
        /// Relates to gl4380.i. It accounts for rounding error in conversion of transaction amounts to
        /// base currency by the following routine:
        /// if the journal's debit total = journal's credit total (in foreign
        /// currency) recalculate the transaction's amount in base currency by the
        /// formula:
        ///     base_amount = absolute value (total_debitsn_base -
        ///                   totalreditsn_base)
        /// after the debits or credits in base has been adjusted to account for
        /// the change resulting for the change being made.
        ///
        /// If the debit and credit totals in base currency are unknown to the
        /// calling program, negative values can be passed. This will cause this
        /// procedure to calculate the amounts.
        /// </summary>
        /// <param name="AAmountField"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ADebitTotal"></param>
        /// <param name="ACreditTotal"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="AOrigDebitCreditIndicator"></param>
        /// <param name="AOrigAmountInBase"></param>
        /// <param name="AAmountInBaseCurrency"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerficationResult"></param>
        private void BalanceAmountsInBaseCurrency(string AAmountField,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            decimal ADebitTotal,
            decimal ACreditTotal,
            bool ADebitCreditIndicator,
            bool AOrigDebitCreditIndicator,
            decimal AOrigAmountInBase,
            ref decimal AAmountInBaseCurrency,
            TDBTransaction ADBTransaction,
            TVerificationResultCollection AVerficationResult
            )
        {
            //gl4380.i <- gl4380o(2).p <- gl1130.i <- gl2150.p
            //ADebitTotal = 0;
            //ACreditTotal = 0;

            AJournalTable JournalTable = AJournalAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ADBTransaction);
            AJournalRow JournalRow = (AJournalRow)JournalTable.Rows[0];

            if (JournalRow.JournalDebitTotal == JournalRow.JournalCreditTotal)
            {
                ATransactionTable TransTb2 = new ATransactionTable();
                ATransactionRow TemplateRow5 = (ATransactionRow)TransTb2.NewRowTyped(false);
                //ATransactionRow TransRow2 = (ATransactionRow)TransTable.Rows[0];

                TemplateRow5.LedgerNumber = ALedgerNumber;
                TemplateRow5.BatchNumber = ABatchNumber;
                TemplateRow5.JournalNumber = AJournalNumber;

                StringCollection operators5 = StringHelper.InitStrArr(new string[] { "=", "=", "=" });

                ATransactionTable TransTable5 = ATransactionAccess.LoadUsingTemplate(TemplateRow5, operators5, null, ADBTransaction);
                ATransactionRow TransRow5 = null;

                for (int n = 0; n < TransTable5.Count; n++)
                {
                    TransRow5 = (ATransactionRow)TransTable5.Rows[n];

                    if (TransRow5.DebitCreditIndicator)
                    {
                        if (AAmountField == "AmountInBaseCurrency")
                        {
                            ADebitTotal += TransRow5.AmountInBaseCurrency;
                        }
                        else
                        {
                            ADebitTotal += TransRow5.AmountInIntlCurrency;
                        }
                    }
                    else
                    {
                        if (AAmountField == "a_amount_in_base_currency")
                        {
                            ACreditTotal += TransRow5.AmountInBaseCurrency;
                        }
                        else
                        {
                            ACreditTotal += TransRow5.AmountInIntlCurrency;
                        }
                    }
                }
            }

            if (AOrigDebitCreditIndicator)
            {
                AAmountInBaseCurrency = Math.Abs((ADebitTotal - AOrigAmountInBase) - ACreditTotal);
            }
            else
            {
                AAmountInBaseCurrency = Math.Abs((ACreditTotal - AOrigAmountInBase) - ADebitTotal);
            }
        }

//        private AnalysisAttributes[] ParseAAs(string ACode)
//        {
//            string[] Split = ACode.Split(',');
//            AnalysisAttributes[] s = new AnalysisAttributes[Convert.ToInt32(Split.Length / 2)];
//
//            for (int i = 0; i < (Split.Length / 2); i += 2)
//            {
//                s[(i/2)].Type = Split[i];
//                s[(i/2)].Value = Split[i + 1];
//            }
//
//            return s;
//        }


        /// <summary>
        /// Relates to gl1120.i.
        /// </summary>
        /// <param name="AMode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AJournalDescription"></param>
        /// <param name="ASubSystemCode"></param>
        /// <param name="ATransactionTypeCode"></param>
        /// <param name="ATransactionCurrency"></param>
        /// <param name="AExchangeRateToBase"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="AJournalNumberReturn"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool CreateAdminFeeJournal(string AMode,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            string AJournalDescription,
            string ASubSystemCode,
            string ATransactionTypeCode,
            string ATransactionCurrency,
            decimal AExchangeRateToBase,
            DateTime ADateEffective,
            ref int AJournalNumberReturn,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //RUN gl1120.i <- gl2150.p

            //Assumed valid so far
            bool IsValid = true;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            //int lv_selected_recordd_r;
            bool IsNewRecord = false;
            int ProcessingPeriod = 0;

            AJournalTable JournalTable = null;
            AJournalRow JournalRow = null;
            ABatchTable BatchTable = null;
            ABatchRow BatchRow = null;


            try
            {
                BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ADBTransaction);
                BatchRow = (ABatchRow)BatchTable.Rows[0];

                if ((AMode.ToUpper() == "NEW") || (AMode.ToUpper() == "NEW-INQUIRE-ALL"))
                {
                    JournalTable = new AJournalTable();
                    JournalRow = JournalTable.NewRowTyped(true);

                    IsNewRecord = true;
                    AJournalNumber = BatchRow.LastJournal + 1;
                }
                else
                {
                    //Implies this is called with a pre-existent batch number, i.e. non-zero.
                    JournalTable = AJournalAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ADBTransaction);
                    JournalRow = (AJournalRow)JournalTable.Rows[0];

                    bool journalRowLocked = false;

                    if (journalRowLocked)
                    {
                        ErrorContext = "Journal Creation";
                        ErrorMessage = Catalog.GetString("The record could not be locked. Journal was not created. Please try again later.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                }

                AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                    BatchRow.BatchPeriod,
                    ADBTransaction);
                AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                //***Include gl1120fa.i here***

                if (AJournalDescription.Trim() == string.Empty)
                {
                    //RUN s_errmsg.p ("x_0002",PROGRAM-NAME(1),"Description","").
                    ErrorContext = "Journal Description";
                    ErrorMessage = Catalog.GetString("The field cannot be blank");
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else if (AExchangeRateToBase == 0)
                {
                    //RUN s_errmsg.p ("x_0002",PROGRAM-NAME(1),"Description","").
                    ErrorContext = "Journal Exchange Rate";
                    ErrorMessage = Catalog.GetString("The field cannot be zero");
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else if ((ADateEffective < AccountingPeriodRow.PeriodStartDate)
                         || (ADateEffective > AccountingPeriodRow.PeriodEndDate))
                {
                    ErrorContext = "Journal Date";
                    ErrorMessage = Catalog.GetString("The journal date must be within the period of the batch");
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else
                {
                    ProcessingPeriod = BatchRow.BatchPeriod;
                }

                if (IsNewRecord && IsValid)
                {
                    BatchRow.LastJournal += 1;
                }

                //***End of gl1120fa.i***

                if (IsValid)
                {
                    JournalRow.LedgerNumber = ALedgerNumber;
                    JournalRow.JournalNumber = AJournalNumber;
                    JournalRow.JournalDescription = AJournalDescription;
                    JournalRow.SubSystemCode = ASubSystemCode;
                    JournalRow.TransactionTypeCode = ATransactionTypeCode;
                    JournalRow.TransactionCurrency = ATransactionCurrency;
                    JournalRow.ExchangeRateToBase = AExchangeRateToBase;
                    JournalRow.DateEffective = ADateEffective;
                    JournalRow.JournalPeriod = ProcessingPeriod;
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsValid = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Journal Creation";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while creating journal: '{0}' for Ledger: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    AJournalDescription,
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsValid = false;
            }

            AJournalNumberReturn = AJournalNumber;
            return IsValid;
        }

        /// <summary>
        /// Relates to gl1110.i
        /// </summary>
        /// <param name="AMode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchDescription"></param>
        /// <param name="ABatchControlTotal"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="ABatchNumberReturn"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool CreateAdminFeeBatch(string AMode,
            int ALedgerNumber,
            int ABatchNumber,
            string ABatchDescription,
            decimal ABatchControlTotal,
            DateTime ADateEffective,
            ref int ABatchNumberReturn,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //RUN gl1110.i <- gl1110o.p <- gl1250.p
            //****************4GL Transaction Starts Here********************

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            //return values
            //int ABatchNumberReturn;
            bool IsValid = true;

            //int lv_selected_recordd_r;
            bool IsNewRecord = false;
            int ProcessingPeriod = 0;

            ABatchTable BatchTable = null;
            ABatchRow BatchRow = null;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            AProcessedFeeTable ProcessedFee = null;

            //4gl
            try
            {
                string CurrentMode = AMode.ToUpper();

                if ((CurrentMode == "NEW") || (CurrentMode == "NEW-INQUIRE-ALL"))
                {
                    BatchTable = new ABatchTable();
                    BatchRow = BatchTable.NewRowTyped(true);

                    IsNewRecord = true;
                    ABatchNumber = LedgerRow.LastBatchNumber + 1;
                }
                else
                {
                    //Implies this is called with a pre-existent batch number, i.e. non-zero.
                    BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ADBTransaction);
                    BatchRow = (ABatchRow)BatchTable.Rows[0];

                    //TODO: Need to check how OpenPetra handles row locking conflicts
                    // etc. which will affect how Petra code is brought over and rewritten.
                    // If locked then catch the exception.

                    //If record not available and record is not locked then some unknown error:
                    // Throw a general exception, if is locked throw BatchRecordLockedException
                    bool ledgerRowLocked = false;

                    if (ledgerRowLocked)
                    {
                        ErrorContext = "Batch Creation";
                        ErrorMessage = Catalog.GetString("The record could not be locked. Batch was not created. Please try again later.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                }

                AProcessedFeeRow ProcessFeeRow = (AProcessedFeeRow)ProcessedFee.Rows[0];

                //RUN gl1110fa.i
                IsValid = ValidateBatch(ALedgerNumber,
                    ProcessFeeRow,
                    IsNewRecord,
                    ABatchDescription,
                    ADateEffective,
                    ref ProcessingPeriod,
                    ADBTransaction,
                    ref AVerificationResult);

                if (IsValid)
                {
                    BatchRow.LedgerNumber = ALedgerNumber;
                    BatchRow.BatchNumber = ABatchNumber;
                    BatchRow.BatchDescription = ABatchDescription;
                    BatchRow.BatchControlTotal = ABatchControlTotal;
                    BatchRow.DateEffective = ADateEffective;
                    BatchRow.BatchPeriod = ProcessingPeriod;
                }
            }
            catch (InvalidOperationException ex)
            {
                IsValid = false;
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
            }
            catch (Exception ex)
            {
                ErrorContext = "Batch Creation";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while creating batch: {0} for Ledger: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ABatchDescription,
                    ALedgerNumber
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsValid = false;
            }

            ABatchNumberReturn = ABatchNumber;
            return IsValid;
        }

        /// <summary>
        /// Validation routine for batches. Relates to gl1110fa.i
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ProcessFeeRow"></param>
        /// <param name="IsNewRecord"></param>
        /// <param name="ABatchDescription"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="AProcessingPeriod"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public bool ValidateBatch(int ALedgerNumber,
            AProcessedFeeRow ProcessFeeRow,
            bool IsNewRecord,
            string ABatchDescription,
            DateTime ADateEffective,
            ref int AProcessingPeriod,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            ) //RUN gl1110fa.i <- gl1110.i <- gl1110o.p <- gl1250.p
        {   //Return value and replaces IsValid which is true to this point as no errors have occurred.
            bool IsSuccessful = true;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                if (ABatchDescription.Trim() == String.Empty)
                {
                    //Petra error: x_0002
                    ErrorContext = "Batch Description";
                    ErrorMessage = Catalog.GetString("The field cannot be blank");
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else if (ProcessFeeRow.IsProcessedDateNull())
                {
                    //Petra error: x_0002
                    ErrorContext = "Effective Date";
                    ErrorMessage = Catalog.GetString("The field cannot be blank");
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                //Check the validity of the date
                //Run gl4320.p
                IsSuccessful = ValidateBatchDate(ALedgerNumber,
                    ADateEffective,
                    LedgerRow.CurrentPeriod,
                    ref AProcessingPeriod,
                    ADBTransaction,
                    ref AVerificationResult);

                if (IsSuccessful && IsNewRecord)  //IsValid &&
                {
                    //TODO: Check if the ledger table is locked or not
                    //RUN s_errmsg.p ("x_0017",PROGRAM-NAME(1),"Ledger","").
                    bool ledgerRowLocked = false;

                    if (ledgerRowLocked)
                    {
                        ErrorContext = "Batch Validation";
                        ErrorMessage = Catalog.GetString("The record could not be locked. Changes were NOT saved. Please try again later.");
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    else
                    {
                        LedgerRow.LastBatchNumber++;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsSuccessful = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Batch Validation";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while validating batch: '{0} for Ledger: {1}." +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ABatchDescription,
                    ALedgerNumber
                    );

                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsSuccessful = false;
            }

            return IsSuccessful;
        }

        /// <summary>
        /// Test that a date is valid: In the current period or within
        ///  forward posting limits. Relates to gl4320.p
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATestDate"></param>
        /// <param name="ACurrentPeriod"></param>
        /// <param name="AProcessingPeriod"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private bool ValidateBatchDate(int ALedgerNumber,
            DateTime ATestDate,
            int ACurrentPeriod,
            ref int AProcessingPeriod,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            //gl4320.p <- gl1110fa.i <- gl1110.i <- gl1110o.p <- gl1250.p
            //*************gl4320.p*******************
            //Return value
            bool IsValidDateIndicator = false;

            /* Retrieve info on the ledger. */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ADBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];


            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                AAccountingPeriodTable AccountingPeriodTable2 = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                    ACurrentPeriod,
                    ADBTransaction);
                AAccountingPeriodRow AccountingPeriodRow2 = (AAccountingPeriodRow)AccountingPeriodTable2.Rows[0];

                int AccPerNo = AccountingPeriodRow2.AccountingPeriodNumber;

                if (AccountingPeriodTable2.Count == 0)
                {
                    //RUN x_table.p ("a_accounting_period":U).
                    //RUN s_errmsg.p ("X_0007":U,
                    ErrorContext = "Date Validation";
                    ErrorMessage = String.Format(Catalog.GetString("Accounting Period Number: {0} does not exist for Ledger: {1}"),
                        ACurrentPeriod,
                        ALedgerNumber
                        );
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else if (ATestDate < AccountingPeriodRow2.PeriodStartDate)
                {
                    //RUN s_errmsg.p ("GL0013":U,
                    ErrorContext = "Date Validation";
                    //TODO need to find the proper international date format
                    ErrorMessage = String.Format(Catalog.GetString("Date: {0:d} refers to a prior accounting period."),
                        ATestDate
                        );
                    ErrorType = TResultSeverity.Resv_Critical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }
                else if (ATestDate > AccountingPeriodRow2.PeriodEndDate)
                {
                    AAccountingPeriodTable AccountingPeriodTable3 = new AAccountingPeriodTable();
                    AAccountingPeriodRow TemplateRow2 = (AAccountingPeriodRow)AccountingPeriodTable3.NewRowTyped(false);

                    TemplateRow2.LedgerNumber = ALedgerNumber;
                    TemplateRow2.PeriodStartDate = ATestDate;
                    TemplateRow2.PeriodEndDate = ATestDate;

                    StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "<=", ">=" });

                    AAccountingPeriodTable AccountingPeriodTable4 = AAccountingPeriodAccess.LoadUsingTemplate(TemplateRow2,
                        operators2,
                        null,
                        ADBTransaction);

                    if (AccountingPeriodTable3.Count == 0)
                    {
                        //                                    RUN x_table.p ("a_accounting_period":U).
                        //                                    RUN s_errmsg.p ("X_0007":U,
                        ErrorContext = "Date Validation";
                        ErrorMessage = String.Format(Catalog.GetString("Date: {0:d} does not belong to any valid accounting period for Ledger: {1}"),
                            ATestDate,
                            ALedgerNumber
                            );
                        ErrorType = TResultSeverity.Resv_Critical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    int lv_max_period = ACurrentPeriod + LedgerRow.NumberFwdPostingPeriods;

                    //Check if date is within the forward posting allowance
                    if ((AccPerNo <= lv_max_period) && (AccPerNo >= ACurrentPeriod))
                    {
                        IsValidDateIndicator = true;
                        AProcessingPeriod = AccPerNo;
                    }
                    else
                    {
                        /* The date is past the maximum fwd posting date. */

                        /*RUN s_errmsg.p ("GL0014":U,*/
                        ErrorContext = "Date Validation";
                        ErrorMessage = String.Format(Catalog.GetString("Date: {0:d} is past the maximum fwd posting date."),
                            ATestDate
                            );
                        ErrorType = TResultSeverity.Resv_Critical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                }
                else
                {
                    IsValidDateIndicator = true;
                    AProcessingPeriod = AccPerNo;
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsValidDateIndicator = false;
            }
            catch (Exception ex)
            {
                ErrorContext = "Date Validation";
                ErrorMessage =
                    String.Format(Catalog.GetString(
                            "Unknown error while processing date validation for Accounting Period Number: {0} and Ledger: {1}" +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ACurrentPeriod,
                        ALedgerNumber
                        );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                IsValidDateIndicator = false;
            }

            return IsValidDateIndicator;
        }

        /// <summary>
        /// Convert between European Currencies.
        /// If either currency is non euro then perform calculation using supplied rate.
        /// If both are euro (including eur) then do triangulation.
        /// </summary>
        /// <param name="AInputAmount"></param>
        /// <param name="AExchangeRate"></param>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>Converted Amount</returns>
        public decimal ConvertBetweenEuropeanCurrencies(decimal AInputAmount,
            decimal AExchangeRate,
            string ACurrencyFrom,
            string ACurrencyTo,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            decimal OutputAmount = AInputAmount;

            int NumDecPlaces;
            decimal IntermediateResult;
            decimal ExchangeRate;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                if (ACurrencyFrom == ACurrencyTo)
                {
                    ErrorContext = "Convert Between European Currencies";
                    ErrorMessage = String.Format(Catalog.GetString("Currencies are the same so no conversion required"));
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                ACurrencyTable CurrencyTbFrom = new ACurrencyTable();
                ACurrencyRow TemplateRow = (ACurrencyRow)CurrencyTbFrom.NewRowTyped(false);

                TemplateRow.CurrencyCode = ACurrencyFrom;
                TemplateRow.InEmu = true;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                //StringCollection OrderList = new StringCollection();

                ACurrencyTable CurrencyTableFrom = ACurrencyAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction);

                ACurrencyTable CurrencyTbTo = new ACurrencyTable();
                ACurrencyRow TemplateRow2 = (ACurrencyRow)CurrencyTbTo.NewRowTyped(false);

                TemplateRow2.CurrencyCode = ACurrencyTo;
                TemplateRow2.InEmu = true;

                StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=" });
                //StringCollection OrderList = new StringCollection();

                ACurrencyTable CurrencyTableTo = ACurrencyAccess.LoadUsingTemplate(TemplateRow2, operators2, null, ADBTransaction);

                if ((CurrencyTableFrom.Count == 0) || (CurrencyTableTo.Count == 0))
                {
                    CurrencyTableTo = ACurrencyAccess.LoadByPrimaryKey(ACurrencyTo, ADBTransaction);

                    if (CurrencyTableTo.Count == 0)
                    {
                        ErrorContext = "Convert Between European Currencies";
                        ErrorMessage = String.Format(Catalog.GetString(
                                "Unable to access the record for currency: {0} in the currencies table."), ACurrencyTo);
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }
                    else
                    {
                        ACurrencyRow CurrencyRowTo = (ACurrencyRow)CurrencyTableTo.Rows[0];
                        OutputAmount =
                            Math.Round(AInputAmount / AExchangeRate, THelperNumeric.CalcNumericFormatDecimalPlaces(CurrencyRowTo.DisplayFormat));
                    }
                }
                else /* EMU conversion required*/
                {
                    ACurrencyRow CurrencyRowTo = (ACurrencyRow)CurrencyTableTo.Rows[0];
                    NumDecPlaces = THelperNumeric.CalcNumericFormatDecimalPlaces(CurrencyRowTo.DisplayFormat);

                    /* Find the exchange rate into EMU */
                    /* Calculate the rate */
                    string CurrencyFrom = ACurrencyFrom.ToUpper();
                    string CurrencyTo = ACurrencyTo.ToUpper();

                    if (CurrencyFrom == MFinanceConstants.EURO_CURRENCY.ToUpper())
                    {
                        ExchangeRate = GetRate(ACurrencyTo, ACurrencyFrom, ADBTransaction, ref AVerificationResult);

                        if (ExchangeRate == 0)
                        {
                            ErrorContext = "Convert Between European Currencies";
                            ErrorMessage =
                                String.Format(Catalog.GetString(
                                        "Unable to access the daily exchange rate for currency: {0} in the exchange rate table."), ACurrencyTo);
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }
                        else
                        {
                            OutputAmount = Math.Round((AInputAmount * ExchangeRate), NumDecPlaces);
                        }
                    }
                    else if (CurrencyTo == MFinanceConstants.EURO_CURRENCY.ToUpper())
                    {
                        ExchangeRate = GetRate(ACurrencyFrom, ACurrencyTo, ADBTransaction, ref AVerificationResult);

                        if (ExchangeRate == 0)
                        {
                            ErrorContext = "Convert Between European Currencies";
                            ErrorMessage =
                                String.Format(Catalog.GetString(
                                        "Unable to access the daily exchange rate for currency: {0} in the exchange rate table."), ACurrencyTo);
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }
                        else
                        {
                            OutputAmount = Math.Round((AInputAmount / ExchangeRate), NumDecPlaces);
                        }
                    }
                    else //Go via Euro
                    {
                        ExchangeRate = GetRate(ACurrencyFrom, MFinanceConstants.EURO_CURRENCY.ToUpper(), ADBTransaction, ref AVerificationResult);

                        if (ExchangeRate == 0)
                        {
                            ErrorContext = "Convert Between European Currencies";
                            ErrorMessage =
                                String.Format(Catalog.GetString(
                                        "Unable to access the daily exchange rate for currency: {0} in the exchange rate table."), ACurrencyTo);
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }
                        else
                        {
                            IntermediateResult = Math.Round((AInputAmount / ExchangeRate), MFinanceConstants.EURO_INTERMEDIATE_DP);
                            ExchangeRate = GetRate(ACurrencyTo, MFinanceConstants.EURO_CURRENCY.ToUpper(), ADBTransaction, ref AVerificationResult);

                            OutputAmount = Math.Round((IntermediateResult * ExchangeRate), NumDecPlaces);
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                OutputAmount = AInputAmount;
            }
            catch (Exception ex)
            {
                ErrorContext = "Converting Between European Countries";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while converting from: '{0}' to: '{1}'." +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ACurrencyFrom,
                    ACurrencyTo
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                OutputAmount = 0;
            }

            return OutputAmount;
        }

        /// <summary>
        /// Retrieves the relevant exchange rate
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private decimal GetRate(string ACurrencyFrom,
            string ACurrencyTo,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            decimal RateOfExchange = 0;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            try
            {
                ADailyExchangeRateTable DailyExchangeRateTb = new ADailyExchangeRateTable();
                ADailyExchangeRateRow TemplateRow = (ADailyExchangeRateRow)DailyExchangeRateTb.NewRowTyped(false);

                TemplateRow.FromCurrencyCode = ACurrencyFrom;
                TemplateRow.ToCurrencyCode = ACurrencyTo;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                StringCollection OrderList = new StringCollection();
                //Order by Fee Code
                OrderList.Add("ORDER BY " + ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC");
                OrderList.Add(ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC");

                ADailyExchangeRateTable DailyExchangeRateTable = ADailyExchangeRateAccess.LoadUsingTemplate(TemplateRow,
                    operators,
                    null,
                    ADBTransaction,
                    OrderList,
                    0,
                    0);

                if (DailyExchangeRateTable.Count > 0)
                {
                    ADailyExchangeRateRow DailyExchangeRateRow = (ADailyExchangeRateRow)DailyExchangeRateTable.Rows[0];
                    RateOfExchange = DailyExchangeRateRow.RateOfExchange;
                }
                else
                {
                    //RUN s_errmsg.p ("X_0027":U,
                    ErrorContext = "Get Daily Exchange Rate";
                    ErrorMessage = String.Format(Catalog.GetString(
                            "Rate for converting from {0} to {1} does not exist. Do you wish to add it?"), ACurrencyFrom, ACurrencyTo);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    //TODO:Rather than throw an error ask if the user wants to add a new exchange rate

                    /*****************************************************
                    *  int dialogResult;
                    *  //throw new System.InvalidOperationException(ErrorMessage);
                    *
                    *  RateOfExchange = 0;
                    *
                    *  if (dialogResult = 1)
                    *  {
                    *    // Call the new rate entry screen.
                    *    RUN as1070ex.w (?,
                    *           "new":U,
                    *           ?,
                    *           pv_fromurr,
                    *           pv_tourr,
                    *           TODAY).
                    *    Simply rerun this procedure
                    *    RateOfExchange = GetRate(ACurrencyFrom, ACurrencyTo, ref AVerificationResult)
                    *
                    *  }
                    *  else
                    *  {
                    *   ErrorContext = "Get Daily Exchange Rate";
                    *   ErrorMessage = String.Format(Catalog.GetString("Rate for converting from {0} to {1} does not exist."), ACurrencyFrom, ACurrencyTo);
                    *   ErrorType = TResultSeverity.Resv_Noncritical;
                    *   throw new System.InvalidOperationException(ErrorMessage);
                    *  }
                    *****************************************************/
                }
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                RateOfExchange = 0;
            }
            catch (Exception ex)
            {
                ErrorContext = "Get Exchange Rate";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while getting exchange rate from: '{0}' to: '{1}'." +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ACurrencyFrom,
                    ACurrencyTo
                    );
                ErrorType = TResultSeverity.Resv_Critical;

                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                RateOfExchange = 0;
            }

            return RateOfExchange;
        }

        /// <summary>
        /// Checks whether or not a given currency exists in the Currency table
        /// </summary>
        /// <param name="ACurrencyCode">The currency code to look for</param>
        /// <param name="ADBTransaction">The current transaction</param>
        /// <returns>True if exists, else false</returns>
        public bool CheckCurrencyExists(string ACurrencyCode, TDBTransaction ADBTransaction)
        {
            return ACurrencyAccess.Exists(ACurrencyCode, ADBTransaction);
        }

        /// <summary>
        /// Get the sequence number of a a_generaledger_master.
        ///  if ACostCentreCode EQ "" then the main cost centre of the ledger [xx] is used.
        ///  returns the a_glm_sequence value of the a_generaledger_master row of the given year.
        ///  returns -1 if there is no available a_generaledger_master row.
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="AAccountCode">Account code</param>
        /// <param name="ACostCentreCode">Cost centre</param>
        /// <param name="AYear">Year</param>
        /// <param name="ADBTransaction">Current transaction</param>
        /// <returns>Returns Glm sequence number or -1 for failure</returns>
        public int GetGlmSequence(int ALedgerNumber,
            string AAccountCode,
            string ACostCentreCode,
            int AYear,
            TDBTransaction ADBTransaction
            )
        {
            if (ACostCentreCode.Trim() == string.Empty)
            {
                ACostCentreTable ACostCentreTb = new ACostCentreTable();
                ACostCentreRow TemplateRow = (ACostCentreRow)ACostCentreTb.NewRowTyped(false);

                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.CostCentreToReportTo = string.Empty;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                StringCollection OrderList = new StringCollection();
                //Order by Fee Code
                OrderList.Add("ORDER BY " + ACostCentreTable.GetLedgerNumberDBName() + " ASC");
                OrderList.Add(ACostCentreTable.GetCostCentreCodeDBName() + " ASC");

                ACostCentreTable CostCentreTable = ACostCentreAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction, OrderList, 0, 0);

                if (CostCentreTable.Count > 0)
                {
                    ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];
                    ACostCentreCode = CostCentreRow.CostCentreCode;
                }
            }

            AGeneralLedgerMasterTable GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByUniqueKey(ALedgerNumber,
                AYear,
                AAccountCode,
                ACostCentreCode,
                ADBTransaction);

            if (GeneralLedgerMasterTable.Count > 0)
            {
                AGeneralLedgerMasterRow GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];
                return GeneralLedgerMasterRow.GlmSequence;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// GetActual retrieves the actuals value of the given period, no matter if it is in a forwarding period.
        /// GetActual is similar to GetBudget. The main difference is, that forwarding periods are saved in the current year.
        /// You still need the sequenceext_year, because this_year can be older than current_financial_year of the ledger.
        /// So you need to give number_accounting_periods and current_financial_year of the ledger.
        /// You also need to give the number of the year from which you want the data.
        /// Currency_select is either "B" for base or "I" for international currency or "T" for transaction currency
        /// You want e.g. the actual data of period 13 in year 2, the current financial year is 3.
        /// The call would look like: GetActual(sequence_year_2, sequence_year_3, 13, 12, 3, 2, false, "B");
        /// That means, the function has to return the difference between year 3 period 1 and the start balance of year 3.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGlmSequenceThisYear"></param>
        /// <param name="AGlmSequenceNextYear"></param>
        /// <param name="APerodNumber"></param>
        /// <param name="ANumberOfAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYtd"></param>
        /// <param name="ACurrencySelect"></param>
        /// <param name="ADBTransaction"></param>
        /// <returns></returns>
        public decimal GetActual(int ALedgerNumber,
            int AGlmSequenceThisYear,
            int AGlmSequenceNextYear,
            int APerodNumber,
            int ANumberOfAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYtd,
            string ACurrencySelect,
            TDBTransaction ADBTransaction
            )
        {
            return GetActualInternal(ALedgerNumber,
                AGlmSequenceThisYear,
                AGlmSequenceNextYear,
                APerodNumber,
                ANumberOfAccountingPeriods,
                ACurrentFinancialYear,
                AThisYear,
                AYtd,
                false,
                ACurrencySelect,
                ADBTransaction
                );
        }

        /// <summary>
        /// Difference to GetActual is only the Balance Sheet parameter.
        ///  That is at the moment only required from within the finance library, i.e. CalculateAmount.
        ///  Balance Sheet fwd Periods: special treatment of the pl account on a balance sheet: the 14th period is treated like the 14th period and not like the 2nd.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGlmSequenceThisYear"></param>
        /// <param name="AGlmSequenceNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberOfAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYtd"></param>
        /// <param name="ABalanceSheetForwardPeriods"></param>
        /// <param name="ACurrencySelect"></param>
        /// <param name="ADBTransaction"></param>
        /// <returns></returns>
        public decimal GetActualInternal(int ALedgerNumber,
            int AGlmSequenceThisYear,
            int AGlmSequenceNextYear,
            int APeriodNumber,
            int ANumberOfAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYtd,
            bool ABalanceSheetForwardPeriods,
            string ACurrencySelect,
            TDBTransaction ADBTransaction
            )
        {
            AGeneralLedgerMasterTable GeneralLedgerMasterTable = null;
            AGeneralLedgerMasterRow GeneralLedgerMasterRow = null;
            AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;
            AAccountTable AccountTable = null;
            AAccountRow AccountRow = null;

            decimal CurrencyAmount;
            bool IncExpAccountFwdPeriod = false;

            if (AGlmSequenceThisYear == -1)
            {
                return 0;
            }

            CurrencyAmount = 0;

            if (APeriodNumber == 0)
            {
                /*Start balance*/
                GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGlmSequenceThisYear, ADBTransaction);
                GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];

                if (ACurrencySelect == "B")
                {
                    CurrencyAmount = GeneralLedgerMasterRow.StartBalanceBase;
                }
                else if (ACurrencySelect == "I")
                {
                    CurrencyAmount = GeneralLedgerMasterRow.StartBalanceIntl;
                }
                else
                {
                    CurrencyAmount = GeneralLedgerMasterRow.StartBalanceForeign;
                }
            }
            else if (APeriodNumber > ANumberOfAccountingPeriods)
            {
                /* forwarding periods only exist for the current financial year */

                if (ACurrentFinancialYear == AThisYear)
                {
                    GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGlmSequenceThisYear,
                        APeriodNumber,
                        ADBTransaction);
                }
                else
                {
                    GeneralLedgerMasterPeriodTable =
                        AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGlmSequenceNextYear,
                            (APeriodNumber - ANumberOfAccountingPeriods),
                            ADBTransaction);
                }
            }
            else
            {
                /* normal period */
                GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGlmSequenceThisYear,
                    APeriodNumber,
                    ADBTransaction);
            }

            if (GeneralLedgerMasterPeriodTable.Count > 0)
            {
                GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                if (ACurrencySelect == "B")
                {
                    CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualBase;
                }
                else if (ACurrencySelect == "I")
                {
                    CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualIntl;
                }
                else
                {
                    CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualForeign;
                }
            }

            if ((APeriodNumber > ANumberOfAccountingPeriods) && (ACurrentFinancialYear == AThisYear))
            {
                GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGlmSequenceThisYear, ADBTransaction);
                GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];

                AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, GeneralLedgerMasterRow.AccountCode, ADBTransaction);
                AccountRow = (AAccountRow)AccountTable.NewRowTyped(false);

                string AccountType = AccountRow.AccountType.ToUpper();

                if (((AccountType == "INCOME") || (AccountType == "EXPENSE")) && !ABalanceSheetForwardPeriods)
                {
                    IncExpAccountFwdPeriod = true;
                    CurrencyAmount -= GetActualInternal(ALedgerNumber,
                        AGlmSequenceThisYear,
                        AGlmSequenceNextYear,
                        ANumberOfAccountingPeriods,
                        ANumberOfAccountingPeriods,
                        ACurrentFinancialYear,
                        AThisYear,
                        true,
                        ABalanceSheetForwardPeriods,
                        ACurrencySelect,
                        ADBTransaction
                        );
                }
            }

            if (!AYtd)
            {
                if (!((APeriodNumber == (ANumberOfAccountingPeriods + 1)) && IncExpAccountFwdPeriod)
                    && !((APeriodNumber == (ANumberOfAccountingPeriods + 1)) && (ACurrentFinancialYear > AThisYear)))
                {
                    /* if it is an income expense acount, and we are in a forward period, nothing needs to be subtracted,
                     * because that was done in correcting the amount in the block above;
                     * if we are in a previous year, in a forward period, don't worry about subtracting.
                     */
                    CurrencyAmount -= GetActualInternal(ALedgerNumber,
                        AGlmSequenceThisYear,
                        AGlmSequenceNextYear,
                        (APeriodNumber - 1),
                        ANumberOfAccountingPeriods,
                        ACurrentFinancialYear,
                        AThisYear,
                        true,
                        ABalanceSheetForwardPeriods,
                        ACurrencySelect,
                        ADBTransaction
                        );
                }
            }

            return CurrencyAmount;
        }

        /// <summary>
        /// GetBudget retrieves the budget value of the given period, no matter if it is in a forwarding period.
        /// You want e.g. the budget data of period 13 in year 2. That means you also have to give the sequence number of the next year, year 3, because the budget data is never stored in forwarding periods.
        /// The number_accounting_periods you should get from the ledger.
        /// If you want the ytd data, it is calculated, starting in period 1 or period (number_accounting_periods + 1) if periodumber is a forwarding period.
        /// currency_select is either "B" for base or "I" for international currency
        /// If you want the budget from last year, just call it with GetBudget(sequence_ofast_year, sequence_of_this_year,...)
        /// If you want the budget of the next year, call it with GetBudget(sequence_ofext_year, -1, ...)
        /// </summary>
        /// <param name="AGlmSequenceThisYear"></param>
        /// <param name="AGlmSequenceNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberOfAccountingPeriods"></param>
        /// <param name="AYtd"></param>
        /// <param name="ACurrencySelect"></param>
        /// <param name="ADBTransaction"></param>
        /// <returns>Retrieve budget amount</returns>
        public decimal GetBudget(int AGlmSequenceThisYear,
            int AGlmSequenceNextYear,
            int APeriodNumber,
            int ANumberOfAccountingPeriods,
            bool AYtd,
            string ACurrencySelect,
            TDBTransaction ADBTransaction
            )
        {
            if (APeriodNumber > ANumberOfAccountingPeriods)
            {
                return CalculateBudget(AGlmSequenceNextYear,
                    1,
                    (APeriodNumber - ANumberOfAccountingPeriods),
                    AYtd,
                    ACurrencySelect,
                    ADBTransaction
                    );
            }
            else
            {
                return CalculateBudget(AGlmSequenceThisYear,
                    1,
                    APeriodNumber,
                    AYtd,
                    ACurrencySelect,
                    ADBTransaction
                    );
            }
        }

        /// <summary>
        /// CalculateBudget is only used internally as a helper function for GetBudget.
        /// Returns the budget for the given period of time,
        /// if ytd is set, this period is from start_period to end_period,
        /// otherwise it is only the value of the end_period.
        /// currency_select is either "B" for base or "I" for international currency
        /// </summary>
        /// <param name="AGlmSequence"></param>
        /// <param name="AStartPeriod"></param>
        /// <param name="AEndPeriod"></param>
        /// <param name="AYtd"></param>
        /// <param name="ACurrencySelect"></param>
        /// <param name="ADBTransaction"></param>
        /// <returns>Calculate Budget amount</returns>
        public decimal CalculateBudget(int AGlmSequence,
            int AStartPeriod,
            int AEndPeriod,
            bool AYtd,
            string ACurrencySelect,
            TDBTransaction ADBTransaction
            )
        {
            decimal CurrencyAmount;
            int YtdPeriod;
            AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

            if (AGlmSequence == -1)
            {
                return 0;
            }

            CurrencyAmount = 0;

            if (!AYtd)
            {
                AStartPeriod = AEndPeriod;
            }

            for (YtdPeriod = AStartPeriod; YtdPeriod <= AEndPeriod; YtdPeriod++)
            {
                GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGlmSequence, YtdPeriod, ADBTransaction);

                if (GeneralLedgerMasterPeriodTable.Count > 0)
                {
                    GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                    if (ACurrencySelect == "B")
                    {
                        CurrencyAmount += GeneralLedgerMasterPeriodRow.BudgetBase;
                    }
                    else
                    {
                        CurrencyAmount += GeneralLedgerMasterPeriodRow.BudgetBase;
                    }
                }
            }

            return CurrencyAmount;
        }
    }
}