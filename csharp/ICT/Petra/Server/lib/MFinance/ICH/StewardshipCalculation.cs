//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert, timop
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
        /// <param name="AVerificationResult"></param>
        /// <returns>True if calculation succeeded, otherwise false.</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PerformStewardshipCalculation(int ALedgerNumber,
            int APeriodNumber,
            out TVerificationResultCollection AVerificationResult
            )
        {
/*
 *          if (TLogging.DL >= 9)
 *          {
 *              //Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation called.");
 *          }
 */
            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            bool NewTransaction = false;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                if (GenerateAdminFeeBatch(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult))
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }

/*
 *                  if (TLogging.DL >= 8)
 *                  {
 *                      Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation: Transaction committed!");
 *                  }
 */
                    return GenerateICHStewardshipBatch(ALedgerNumber, APeriodNumber, ref AVerificationResult);
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

/*
 *                  if (TLogging.DL >= 8)
 *                  {
 *                      Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation: Transaction ROLLED BACK because of an error!");
 *                  }
 */
                    return false;
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

/*
 *              if (TLogging.DL >= 8)
 *              {
 *                  Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation: Transaction ROLLED BACK because an exception occurred!");
 *              }
 */
                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw;
            }
        }

        /// <summary>
        /// Relates to gl2160.p, line 178 Post_To_Ledger:
        /// If we're working in an open period, make sure the summary data is up to date.
        ///  Check that there are amounts to be transferred to the clearing house.
        ///  I.e., there are debits or credits to foreign cost centres.
        ///  Create the journal to create the transfer transactions in,
        ///  if there are amounts to transfer.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if successful</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool GenerateICHStewardshipBatch(int ALedgerNumber,
            int APeriodNumber,
            ref TVerificationResultCollection AVerificationResult)
        {
            string standardCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);

            bool isSuccessful = false;
            bool drCrIndicator = true;
            bool incomeDrCrIndicator;
            bool expenseDrCrIndicator;
            bool accountDrCrIndicator;

            string incomeAccounts = string.Empty;
            string expenseAccounts = string.Empty;

            string currentAccountCode;
            decimal amountInBaseCurrency;
            decimal amountInIntlCurrency;

            DateTime periodStartDate;
            DateTime periodEndDate;

            //Error handling
            string errorContext = String.Empty;
            string errorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity errorType = TResultSeverity.Resv_Noncritical;

            bool newTransaction = false;
            TDBTransaction dBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out newTransaction);

            //Generating the ICH batch...
            try
            {
                TFinancialYear.GetStartAndEndDateOfPeriod(ALedgerNumber, APeriodNumber, out periodStartDate, out periodEndDate, dBTransaction);

                //Create a new batch. If it turns out I don't need one, I can delete it later.
                GLBatchTDS mainDS = TGLPosting.CreateABatch(ALedgerNumber, Catalog.GetString("ICH Stewardship"), 0, periodEndDate);

                ABatchRow newBatchRow = mainDS.ABatch[0];
                int gLBatchNumber = newBatchRow.BatchNumber;

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                GLPostingTDS postingDS = new GLPostingTDS();
                ALedgerAccess.LoadByPrimaryKey(postingDS, ALedgerNumber, dBTransaction);
                AAccountAccess.LoadViaALedger(postingDS, ALedgerNumber, dBTransaction);
                ACostCentreAccess.LoadViaALedger(postingDS, ALedgerNumber, dBTransaction);
                AIchStewardshipAccess.LoadViaALedger(postingDS, ALedgerNumber, dBTransaction);
                AAccountHierarchyAccess.LoadViaALedger(postingDS, ALedgerNumber, dBTransaction);

                GLBatchTDS tempDS = new GLBatchTDS();

                ABatchTable batchTable = new ABatchTable();

                ABatchRow templateRow0 = (ABatchRow)batchTable.NewRowTyped(false);

                templateRow0.LedgerNumber = ALedgerNumber;
                templateRow0.BatchPeriod = APeriodNumber;

                StringCollection operators0 = StringHelper.InitStrArr(new string[] { "=", "=" });
                StringCollection orderList0 = new StringCollection();

                orderList0.Add("ORDER BY");
                orderList0.Add(ABatchTable.GetBatchNumberDBName() + " DESC");

                ABatchTable batchTable2 = ABatchAccess.LoadUsingTemplate(templateRow0,
                    operators0,
                    null,
                    dBTransaction,
                    orderList0,
                    0,
                    0);

                if (batchTable2 != null)
                {
                    int batchNumber = 0;

                    for (int i = 0; i < batchTable2.Count; i++)
                    {
                        ABatchRow batchRow = (ABatchRow)batchTable2.Rows[i];

                        batchNumber = batchRow.BatchNumber;

                        AJournalAccess.LoadViaABatch(mainDS, ALedgerNumber, batchNumber, dBTransaction);
                        ATransactionAccess.LoadViaABatch(mainDS, ALedgerNumber, batchNumber, dBTransaction);
                    }
                }
                else
                {
                    errorContext = Catalog.GetString("Generating the ICH batch");
                    errorMessage =
                        String.Format(Catalog.GetString("No Batches found to process in Ledger: {0}"),
                            ALedgerNumber);
                    errorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(errorMessage);
                }

                ALedgerRow ledgerRow = (ALedgerRow)postingDS.ALedger.Rows[0];

                //Create a new journal in the Batch
                //Run gl1120o.p
                AJournalRow newJournalRow = mainDS.AJournal.NewRowTyped();
                newJournalRow.LedgerNumber = ALedgerNumber;
                newJournalRow.BatchNumber = gLBatchNumber;
                newJournalRow.JournalNumber = ++newBatchRow.LastJournal;
                newJournalRow.JournalDescription = newBatchRow.BatchDescription;
                newJournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                newJournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                newJournalRow.TransactionCurrency = ledgerRow.BaseCurrency;
                newJournalRow.ExchangeRateToBase = 1;
                newJournalRow.DateEffective = periodEndDate;
                newJournalRow.JournalPeriod = APeriodNumber;
                mainDS.AJournal.Rows.Add(newJournalRow);

                int GLJournalNumber = newJournalRow.JournalNumber;
                int GLTransactionNumber = newJournalRow.LastTransactionNumber + 1;

                // ***************************
                //  Generate the transactions
                // ***************************

                AAccountRow accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.INCOME_HEADING });

                //Process income accounts
                if (accountRow != null)
                {
                    incomeDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    errorContext = Catalog.GetString("Generating the ICH batch");
                    errorMessage =
                        String.Format(Catalog.GetString("Income Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.INCOME_HEADING);
                    errorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(errorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    accountRow,
                    dBTransaction,
                    ref incomeAccounts,
                    ref AVerificationResult);


                //Process expense accounts
                accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.EXPENSE_HEADING });

                if (accountRow != null)
                {
                    expenseDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    errorContext = Catalog.GetString("Generating the ICH batch");
                    errorMessage =
                        String.Format(Catalog.GetString("Expense Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.EXPENSE_HEADING);
                    errorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(errorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    accountRow,
                    dBTransaction,
                    ref expenseAccounts,
                    ref AVerificationResult);


                //Process P&L accounts
                accountRow = (AAccountRow)postingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.PROFIT_AND_LOSS_HEADING });

                if (accountRow != null)
                {
                    accountDrCrIndicator = accountRow.DebitCreditIndicator;
                }
                else
                {
                    errorContext = Catalog.GetString("Generating the ICH batch");
                    errorMessage =
                        String.Format(Catalog.GetString("Profit & Loss Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.PROFIT_AND_LOSS_HEADING);
                    errorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(errorMessage);
                }

                // find out the stewardship number - Ln 275
                // Increment the Last ICH No.
                int iCHProcessing = ++ledgerRow.LastIchNumber;
                decimal iCHTotal = 0;
                bool transferFound = false;
                bool postICHBatch = false;

                //Iterate through the cost centres
                string whereClause = ACostCentreTable.GetPostingCostCentreFlagDBName() + " = True" +
                                     " AND " + ACostCentreTable.GetCostCentreTypeDBName() +
                                     " LIKE '" + MFinanceConstants.FOREIGN_CC_TYPE + "'";
                string orderBy = ACostCentreTable.GetCostCentreCodeDBName();

                DataRow[] foundCCRows = postingDS.ACostCentre.Select(whereClause, orderBy);

                AIchStewardshipTable iCHStewardshipTable = new AIchStewardshipTable();

                foreach (DataRow untypedCCRow in foundCCRows)
                {
                    ACostCentreRow costCentreRow = (ACostCentreRow)untypedCCRow;

                    string costCentre = costCentreRow.CostCentreCode;

                    //Initialise values for each Cost Centre
                    decimal settlementAmount = 0;
                    decimal incomeAmount = 0;
                    decimal expenseAmount = 0;
                    decimal xferAmount = 0;
                    decimal incomeAmountIntl = 0;
                    decimal expenseAmountIntl = 0;
                    decimal xferAmountIntl = 0;

                    transferFound = false;

                    /* 0008 Go through all of the transactions. Ln:301 */
                    whereClause = ATransactionTable.GetCostCentreCodeDBName() + " = '" + costCentre + "'" +
                                  " AND " + ATransactionTable.GetTransactionStatusDBName() + " = " + MFinanceConstants.POSTED +
                                  " AND " + ATransactionTable.GetIchNumberDBName() + " = 0";

                    orderBy = ATransactionTable.GetBatchNumberDBName() + ", " +
                              ATransactionTable.GetJournalNumberDBName() + ", " +
                              ATransactionTable.GetTransactionNumberDBName();

                    DataRow[] foundTransRows = mainDS.ATransaction.Select(whereClause, orderBy);

                    foreach (DataRow untypedTransRow in foundTransRows)
                    {
                        ATransactionRow transactionRow = (ATransactionRow)untypedTransRow;

                        currentAccountCode = transactionRow.AccountCode;
                        amountInBaseCurrency = transactionRow.AmountInBaseCurrency;
                        amountInIntlCurrency = transactionRow.AmountInIntlCurrency;

                        whereClause = AJournalTable.GetLedgerNumberDBName() + " = " + transactionRow.LedgerNumber.ToString() +
                                      " AND " + AJournalTable.GetBatchNumberDBName() + " = " + transactionRow.BatchNumber.ToString() +
                                      " AND " + AJournalTable.GetJournalNumberDBName() + " = " + transactionRow.JournalNumber.ToString() +
                                      " AND " + AJournalTable.GetJournalPeriodDBName() + " = " + APeriodNumber;

                        orderBy = AJournalTable.GetBatchNumberDBName() + ", " + AJournalTable.GetJournalNumberDBName();

                        DataRow[] foundJournalRows = mainDS.AJournal.Select(whereClause, orderBy);

                        if (foundJournalRows != null)
                        {
                            transferFound = true;
                            postICHBatch = true;
                            transactionRow.IchNumber = iCHProcessing;

                            if (transactionRow.DebitCreditIndicator == accountDrCrIndicator)
                            {
                                settlementAmount -= amountInBaseCurrency;
                            }
                            else
                            {
                                settlementAmount += amountInBaseCurrency;
                            }

                            //Process Income (ln:333)
                            if (incomeAccounts.Contains(currentAccountCode))
                            {
                                if (transactionRow.DebitCreditIndicator == incomeDrCrIndicator)
                                {
                                    incomeAmount += amountInBaseCurrency;
                                    incomeAmountIntl += amountInIntlCurrency;
                                }
                                else
                                {
                                    incomeAmount -= amountInBaseCurrency;
                                    incomeAmountIntl -= amountInIntlCurrency;
                                }
                            }

                            //process expenses
                            if (expenseAccounts.Contains(currentAccountCode)
                                && (currentAccountCode != MFinanceConstants.DIRECT_XFER_ACCT)
                                && (currentAccountCode != MFinanceConstants.ICH_ACCT_SETTLEMENT))
                            {
                                if (transactionRow.DebitCreditIndicator = expenseDrCrIndicator)
                                {
                                    expenseAmount += amountInBaseCurrency;
                                    expenseAmountIntl += amountInIntlCurrency;
                                }
                                else
                                {
                                    expenseAmount -= amountInBaseCurrency;
                                    expenseAmountIntl -= amountInIntlCurrency;
                                }
                            }

                            //Process Direct Transfers
                            if (currentAccountCode == MFinanceConstants.DIRECT_XFER_ACCT)
                            {
                                if (transactionRow.DebitCreditIndicator == expenseDrCrIndicator)
                                {
                                    xferAmount += amountInBaseCurrency;
                                    xferAmountIntl += amountInIntlCurrency;
                                }
                                else
                                {
                                    xferAmount -= amountInBaseCurrency;
                                    xferAmountIntl -= amountInIntlCurrency;
                                }
                            }
                        }
                    }  //end of foreach transaction

                    /* now mark all the gifts as processed */
                    if (transferFound)
                    {
                        whereClause = AGiftBatchTable.GetBatchStatusDBName() + " = '" + MFinanceConstants.BATCH_POSTED + "'" +
                                      " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " >= #" + periodStartDate.ToString("yyyy-MM-dd") + "#" +
                                      " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= #" + periodEndDate.ToString("yyyy-MM-dd") + "#";

                        orderBy = AGiftBatchTable.GetBatchNumberDBName();

                        AGiftBatchTable giftBatchTable = AGiftBatchAccess.LoadViaALedger(ALedgerNumber, dBTransaction);

                        DataRow[] foundGiftBatchRows = giftBatchTable.Select(whereClause, orderBy);

                        AGiftBatchRow giftBatchRow = null;
                        AGiftDetailTable giftDetailTable = new AGiftDetailTable();
                        AGiftDetailRow giftDetailRow = null;
                        int batchNumber = 0;

                        foreach (DataRow untypedTransRow in foundGiftBatchRows)
                        {
                            giftBatchRow = (AGiftBatchRow)untypedTransRow;
                            batchNumber = giftBatchRow.BatchNumber;

                            AGiftDetailRow templateRow1 = (AGiftDetailRow)giftDetailTable.NewRowTyped(false);

                            templateRow1.LedgerNumber = ALedgerNumber;
                            templateRow1.BatchNumber = batchNumber;
                            templateRow1.IchNumber = 0;
                            templateRow1.CostCentreCode = costCentre;

                            StringCollection operators1 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
                            StringCollection orderList1 = new StringCollection();

                            orderList1.Add("ORDER BY");
                            orderList1.Add(AGiftDetailTable.GetGiftTransactionNumberDBName() + " ASC");
                            orderList1.Add(AGiftDetailTable.GetDetailNumberDBName() + " ASC");

                            AGiftDetailTable giftDetailTable2 = AGiftDetailAccess.LoadUsingTemplate(templateRow1,
                                operators1,
                                null,
                                dBTransaction,
                                orderList1,
                                0,
                                0);

                            if (giftDetailTable2 != null)
                            {
                                for (int k = 0; k < giftDetailTable2.Count; k++)
                                {
                                    giftDetailRow = (AGiftDetailRow)giftDetailTable2.Rows[k];
                                    giftDetailRow.IchNumber = iCHProcessing;
                                }
                            }
                        }
                    }

                    /* Balance the cost centre by entering an opposite transaction
                     *  to ICH settlement. Use positive amounts only. */

                    /* Increment or decrement the ICH total to be transferred. */
                    drCrIndicator = accountRow.DebitCreditIndicator;

                    if (drCrIndicator == MFinanceConstants.IS_DEBIT)
                    {
                        iCHTotal += settlementAmount;
                    }
                    else
                    {
                        iCHTotal -= settlementAmount;
                    }

                    if (settlementAmount < 0)
                    {
                        drCrIndicator = !accountDrCrIndicator;
                        settlementAmount = -settlementAmount;
                    }
                    else if (settlementAmount > 0)
                    {
                        drCrIndicator = accountDrCrIndicator;
                    }

                    /* Generate the transction to 'balance' the foreign fund -
                     *  in the ICH settlement account. */
                    //RUN gl1130o.p ("new":U,
                    //Create a transaction
                    if (settlementAmount > 0)
                    {
                        if (!TGLPosting.CreateATransaction(mainDS, ALedgerNumber, gLBatchNumber, GLJournalNumber,
                                Catalog.GetString("ICH Monthly Clearing"),
                                MFinanceConstants.ICH_ACCT_SETTLEMENT, costCentre, settlementAmount, periodEndDate, drCrIndicator,
                                Catalog.GetString("ICH"), true, settlementAmount,
                                out GLTransactionNumber))
                        {
                            errorContext = Catalog.GetString("Generating the ICH batch");
                            errorMessage =
                                String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    gLBatchNumber,
                                    GLJournalNumber);
                            errorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(errorMessage);
                        }

                        //Mark as processed
                        ATransactionRow transRow =
                            (ATransactionRow)mainDS.ATransaction.Rows.Find(new object[] { ALedgerNumber, gLBatchNumber, GLJournalNumber,
                                                                                          GLTransactionNumber });
                        transRow.IchNumber = iCHProcessing;
                    }

                    /* can now create corresponding report row on stewardship table */
                    if ((incomeAmount != 0)
                        || (expenseAmount != 0)
                        || (xferAmount != 0))
                    {
                        AIchStewardshipRow iCHStewardshipRow = iCHStewardshipTable.NewRowTyped(true);

                        //MainDS.Tables.Add(IchStewardshipTable);

                        iCHStewardshipRow.LedgerNumber = ALedgerNumber;
                        iCHStewardshipRow.PeriodNumber = APeriodNumber;
                        iCHStewardshipRow.IchNumber = iCHProcessing;
                        iCHStewardshipRow.DateProcessed = DateTime.Today;
                        iCHStewardshipRow.CostCentreCode = costCentre;
                        iCHStewardshipRow.IncomeAmount = incomeAmount;
                        iCHStewardshipRow.ExpenseAmount = expenseAmount;
                        iCHStewardshipRow.DirectXferAmount = xferAmount;
                        iCHStewardshipRow.IncomeAmountIntl = incomeAmountIntl;
                        iCHStewardshipRow.ExpenseAmountIntl = expenseAmountIntl;
                        iCHStewardshipRow.DirectXferAmountIntl = xferAmountIntl;
                        iCHStewardshipTable.Rows.Add(iCHStewardshipRow);
                    }
                }   // for each cost centre

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

                /* 0006 - If the balance is 0 then this is ok (eg last minute
                 *  change of a gift from one field to another)  */
                //IF lv_ich_total_n NE 0 THEN DO:
                //RUN gl1130o.p
                if (iCHTotal == 0)
                {
                    AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Generating the ICH batch"),
                            Catalog.GetString("No ICH batch was required."), TResultSeverity.Resv_Status));

                    // An empty GL Batch now exists, which I need to delete.
                    //
                    TVerificationResultCollection batchCancelResult = new TVerificationResultCollection();

                    TGLPosting.DeleteGLBatch(
                        ALedgerNumber,
                        gLBatchNumber,
                        out batchCancelResult);
                    AVerificationResult.AddCollection(batchCancelResult);

                    isSuccessful = true;
                }
                else
                {
                    //Create a transaction
                    if (!TGLPosting.CreateATransaction(mainDS, ALedgerNumber, gLBatchNumber, GLJournalNumber,
                            Catalog.GetString("ICH Monthly Clearing"),
                            MFinanceConstants.ICH_ACCT_ICH, standardCostCentre, iCHTotal, periodEndDate, drCrIndicator, Catalog.GetString("ICH"),
                            true, iCHTotal,
                            out GLTransactionNumber))
                    {
                        errorContext = Catalog.GetString("Generating the ICH batch");
                        errorMessage =
                            String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                ALedgerNumber,
                                gLBatchNumber,
                                GLJournalNumber);
                        errorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(errorMessage);
                    }

                    //Post the batch
                    if (postICHBatch)
                    {
                        isSuccessful = AIchStewardshipAccess.SubmitChanges(iCHStewardshipTable, dBTransaction, out AVerificationResult);

                        if (isSuccessful)
                        {
                            isSuccessful = (TSubmitChangesResult.scrOK == GLBatchTDSAccess.SubmitChanges(mainDS, out AVerificationResult));

                            if (isSuccessful)
                            {
                                isSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, gLBatchNumber, out AVerificationResult);
                            }
                        }
                    }
                    else
                    {
                        AVerificationResult.Add(new TVerificationResult(errorContext,
                                Catalog.GetString("No Stewardship batch is required."),
                                TResultSeverity.Resv_Status));

                        // An empty GL Batch now exists, which I need to delete.
                        //
                        TVerificationResultCollection batchCancelResult = new TVerificationResultCollection();

                        TGLPosting.DeleteGLBatch(
                            ALedgerNumber,
                            gLBatchNumber,
                            out batchCancelResult);
                        AVerificationResult.AddCollection(batchCancelResult);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                AVerificationResult.Add(new TVerificationResult(errorContext, ex.Message, errorType));
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(errorContext, ex.Message, errorType));
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                errorContext = Catalog.GetString("Calculate Admin Fee");
                errorMessage = String.Format(Catalog.GetString("Unknown error while Generating the ICH batch for Ledger: {0} and Period: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber,
                    APeriodNumber);
                errorType = TResultSeverity.Resv_Critical;
                AVerificationResult.Add(new TVerificationResult(errorContext, errorMessage, errorType));
                Console.WriteLine(ex.Message);
            }

            if (isSuccessful && newTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (!isSuccessful && newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return isSuccessful;
        }

        /// <summary>
        /// To build a CSV list of accounts
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountRowFirst"></param>
        /// <param name="DBTransaction"></param>
        /// <param name="AChildAccounts"></param>
        /// <param name="AVerificationResult"></param>
        private static void BuildChildAccountList(int ALedgerNumber,
            AAccountRow AAccountRowFirst,
            TDBTransaction DBTransaction,
            ref string AChildAccounts,
            ref TVerificationResultCollection AVerificationResult)
        {
            //Return value
            string AccountCode = AAccountRowFirst.AccountCode;

            //Error handling
            string ErrorContext = Catalog.GetString("List Child Accounts");
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

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
                                    ref AChildAccounts,
                                    ref AVerificationResult);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while building list of Child Accounts for Ledger: {0} and Account code: {1}" +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber,
                        AAccountRowFirst.AccountCode
                        );
                ErrorType = TResultSeverity.Resv_Critical;
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
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
        private static bool GenerateAdminFeeBatch(int ALedgerNumber,
            int APeriodNumber,
            bool APrintReport,
            TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult
            )
        {
            bool IsSuccessful = false;

            bool CreatedSuccessfully = false;
            decimal TransactionAmount;
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
            //int x = CreditFeeTotalDT.Count;
            GLStewardshipCalculationTDSCreditFeeTotalRow CreditFeeTotalDR = null;

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
                    ErrorContext = Catalog.GetString("Generate Admin Fee Batch");
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

                string sqlStmt = String.Format("SELECT * FROM {0} WHERE {1} = ? AND {2} = ? AND {3} IS NULL AND {4} <> 0 ORDER BY {5}, {6}",
                    AProcessedFeeTable.GetTableDBName(),
                    AProcessedFeeTable.GetLedgerNumberDBName(),
                    AProcessedFeeTable.GetPeriodNumberDBName(),
                    AProcessedFeeTable.GetProcessedDateDBName(),
                    AProcessedFeeTable.GetPeriodicAmountDBName(),
                    AProcessedFeeTable.GetFeeCodeDBName(),
                    AProcessedFeeTable.GetCostCentreCodeDBName()
                    );

                OdbcParameter[] parameters = new OdbcParameter[2];
                parameters[0] = new OdbcParameter("LedgerNumber", OdbcType.Int);
                parameters[0].Value = ALedgerNumber;
                parameters[1] = new OdbcParameter("PeriodNumber", OdbcType.Int);
                parameters[1].Value = APeriodNumber;

                DBAccess.GDBAccessObj.SelectDT(ProcessedFeeDataTable, sqlStmt, ADBTransaction, parameters, -1, -1);

                if (ProcessedFeeDataTable.Count == 0)
                {
                    if (TLogging.DebugLevel > 0)
                    {
                        TLogging.Log("No fees to charge were found");
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Admin Fee Batch"),
                                String.Format(Catalog.GetString("No admin fees charged in period ({0})."), APeriodNumber),
                                TResultSeverity.Resv_Status));
                    }

                    IsSuccessful = true;
                }
                else
                {
                    //Post to Ledger - Ln 132
                    //****************4GL Transaction Starts Here********************

                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        APeriodNumber,
                        ADBTransaction);
                    AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    // Create a Batch. If no fees are to be charged, I'll delete this batch later.
                    GLBatchTDS AdminFeeDS = TGLPosting.CreateABatch(ALedgerNumber, Catalog.GetString(
                            "Admin Fees & Grants"), 0, AccountingPeriodRow.PeriodEndDate);
                    ABatchRow BatchRow = AdminFeeDS.ABatch[0];

                    AJournalRow JournalRow = AdminFeeDS.AJournal.NewRowTyped();
                    JournalRow.LedgerNumber = ALedgerNumber;
                    JournalRow.BatchNumber = BatchRow.BatchNumber;
                    JournalRow.JournalNumber = ++BatchRow.LastJournal;

                    JournalRow.JournalDescription = BatchRow.BatchDescription;
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
                    int GLJournalNumber = JournalRow.JournalNumber;
                    int GLTransactionNumber = 0;
                    string CurrentFeeCode = string.Empty;
                    string CostCentreCode = string.Empty;
                    string CostCentreCodeDBName = AProcessedFeeTable.GetCostCentreCodeDBName();

                    for (int i = 0; i < ProcessedFeeDataTable.Count; i++)
                    {
                        AProcessedFeeRow pFR = (AProcessedFeeRow)ProcessedFeeDataTable.Rows[i];

                        if (CurrentFeeCode != pFR.FeeCode)
                        {
                            CurrentFeeCode = pFR.FeeCode;

                            // Find first
                            AFeesPayableTable FeesPayableTable = AFeesPayableAccess.LoadByPrimaryKey(ALedgerNumber, CurrentFeeCode, ADBTransaction);

                            if (FeesPayableTable.Count > 0)  //if null try receivables instead
                            {
                                AFeesPayableRow FeesPayableRow = (AFeesPayableRow)FeesPayableTable.Rows[0];
                                DrAccountCode = FeesPayableRow.DrAccountCode;
                                DestCostCentreCode = FeesPayableRow.CostCentreCode;
                                DestAccountCode = FeesPayableRow.AccountCode;
                                FeeDescription = FeesPayableRow.FeeDescription;
                            }
                            else
                            {
                                AFeesReceivableTable FeesReceivableTable = AFeesReceivableAccess.LoadByPrimaryKey(ALedgerNumber,
                                    CurrentFeeCode,
                                    ADBTransaction);

                                if (FeesReceivableTable.Count > 0)
                                {
                                    AFeesReceivableRow FeesReceivableRow = (AFeesReceivableRow)FeesReceivableTable.Rows[0];
                                    DrAccountCode = FeesReceivableRow.DrAccountCode;
                                    DestCostCentreCode = FeesReceivableRow.CostCentreCode;
                                    DestAccountCode = FeesReceivableRow.AccountCode;
                                    FeeDescription = FeesReceivableRow.FeeDescription;
                                }
                                else
                                {
                                    //Petra error: X_0007
                                    ErrorContext = Catalog.GetString("Generate Transactions");
                                    ErrorMessage =
                                        String.Format(Catalog.GetString(
                                                "Unable to access information for Fee Code '{1}' in either the Fees Payable & Receivable Tables for Ledger {0}"),
                                            ALedgerNumber, CurrentFeeCode);
                                    ErrorType = TResultSeverity.Resv_Critical;
                                    throw new System.InvalidOperationException(ErrorMessage);
                                }
                            }

                            DrFeeTotal = 0;

                            //Get all the distinct CostCentres
                            DataView CostCentreView = ProcessedFeeDataTable.DefaultView;
                            CostCentreView.Sort = CostCentreCodeDBName;
                            CostCentreView.RowFilter = string.Format("{0} = '{1}'", AProcessedFeeTable.GetFeeCodeDBName(), CurrentFeeCode);
                            DataTable ProcessedFeeCostCentresTable = CostCentreView.ToTable(true, CostCentreCodeDBName);

                            foreach (DataRow r in ProcessedFeeCostCentresTable.Rows)
                            {
                                CostCentreCode = r[0].ToString();
                                DataView view = ProcessedFeeDataTable.DefaultView;
                                view.Sort = CostCentreCodeDBName;
                                view.RowFilter = string.Format("{0} = '{1}' AND {2} = '{3}'",
                                    AProcessedFeeTable.GetFeeCodeDBName(),
                                    CurrentFeeCode,
                                    CostCentreCodeDBName,
                                    CostCentreCode);

                                //ProcessedFeeDataTable2 = ProcessedFeeDataTable2.Clone();
                                DataTable ProcessedFeeDataTable2 = view.ToTable();

                                Int32 FeeCodeRowCount = ProcessedFeeDataTable2.Rows.Count;

                                for (int j = 0; j < FeeCodeRowCount; j++)
                                {
                                    DataRow pFR2 = ProcessedFeeDataTable2.Rows[j];

                                    DrFeeTotal = DrFeeTotal + Math.Round(Convert.ToDecimal(
                                            pFR2[AProcessedFeeTable.GetPeriodicAmountDBName()]), NumDecPlaces);                                                                                    //pFR2.PeriodicAmount; //ROUND(pFR2.PeriodicAmount,
                                                                                                                                                                                                   // lv_dp)

                                    if (j == (FeeCodeRowCount - 1))                                      //implies last of the CostCentre rows for this feecode
                                    {
                                        if (DrFeeTotal != 0)
                                        {
                                            if (DrFeeTotal < 0)
                                            {
                                                DrCrIndicator = false;     //Credit
                                                DrFeeTotal = -DrFeeTotal;
                                            }
                                            else
                                            {
                                                DrCrIndicator = true;     //Debit
                                                //lv_dr_fee_total remains unchanged
                                            }

                                            /* Generate the transction to deduct the fee amount from
                                             *  the source cost centre. (Expense leg) */
                                            //RUN gl1130o.p -> gl1130.i
                                            //Create a transaction
                                            if (!TGLPosting.CreateATransaction(AdminFeeDS,
                                                    ALedgerNumber,
                                                    BatchRow.BatchNumber,
                                                    GLJournalNumber,
                                                    "Fee: " + FeeDescription + " (" + CurrentFeeCode + ")",
                                                    DrAccountCode,
                                                    CostCentreCode,
                                                    DrFeeTotal,
                                                    AccountingPeriodRow.PeriodEndDate,
                                                    DrCrIndicator,
                                                    "AG",
                                                    true,
                                                    DrFeeTotal,
                                                    out GLTransactionNumber))
                                            {
                                                ErrorContext = Catalog.GetString("Generating the Admin Fee batch");
                                                ErrorMessage =
                                                    String.Format(Catalog.GetString(
                                                            "Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                                        ALedgerNumber,
                                                        BatchRow.BatchNumber,
                                                        GLJournalNumber);
                                                ErrorType = TResultSeverity.Resv_Noncritical;
                                                throw new System.InvalidOperationException(ErrorMessage);
                                            }

                                            DrFeeTotal = 0;
                                        }
                                    }
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
                            CreditFeeTotalDR.TransactionAmount = Math.Round(pFR.PeriodicAmount, NumDecPlaces);
                            CreditFeeTotalDT.Rows.Add(CreditFeeTotalDR);
                        }
                    }

                    /* Generate the transaction to credit the fee amounts to
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
                            TransactionAmount = -cFT.TransactionAmount;
                        }
                        else
                        {
                            DrCrIndicator = false; //Credit
                            TransactionAmount = cFT.TransactionAmount;
                        }

                        /* 0002 - Ok for it to be 0 as just a correction */

                        if (cFT.TransactionAmount != 0)
                        {
                            GLTransactionNumber = 0;
                            CreatedSuccessfully = TGLPosting.CreateATransaction(AdminFeeDS,
                                ALedgerNumber,
                                BatchRow.BatchNumber,
                                JournalRow.JournalNumber,
                                "Collected admin charges",
                                cFT.AccountCode,
                                cFT.CostCentreCode,
                                TransactionAmount,
                                AccountingPeriodRow.PeriodEndDate,
                                DrCrIndicator,
                                "AG",
                                true,
                                TransactionAmount,
                                out GLTransactionNumber);
                        }
                    }

                    TVerificationResultCollection Verification = null;

                    /* check that something has been posted - we know this if the IsSuccessful flag is still false */
                    if (!CreatedSuccessfully)
                    {
                        IsSuccessful = true;
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Admin Fee Batch"),
                                String.Format(Catalog.GetString("No admin fees charged in period ({0})."), APeriodNumber),
                                TResultSeverity.Resv_Status));

                        // An empty GL Batch now exists, which I need to delete.
                        //
                        TVerificationResultCollection BatchCancelResult = new TVerificationResultCollection();

                        TGLPosting.DeleteGLBatch(
                            ALedgerNumber,
                            BatchRow.BatchNumber,
                            out BatchCancelResult);
                        AVerificationResult.AddCollection(BatchCancelResult);
                    }
                    else
                    {
                        //Post the batch just created

                        IsSuccessful = (TSubmitChangesResult.scrOK == GLBatchTDSAccess.SubmitChanges(AdminFeeDS, out Verification));

                        if (IsSuccessful)
                        {
                            IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, BatchRow.BatchNumber, out Verification);
                        }

                        if (IsSuccessful)
                        {
                            AProcessedFeeAccess.SubmitChanges(ProcessedFeeDataTable, ADBTransaction, out Verification);
                        }
                    }

                    if ((Verification == null) || Verification.HasCriticalErrors)
                    {
                        //Petra error: GL0067
                        ErrorContext = Catalog.GetString("Posting Admin Fee Batch");
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
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                IsSuccessful = false;
            }
            catch (Exception ex)
            {
                ErrorContext = Catalog.GetString("Generate Admin Fee Batch");
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