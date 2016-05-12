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
using System.Windows.Forms;
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
            out TVerificationResultCollection AVerificationResult)
        {
/*
 *          if (TLogging.DL >= 9)
 *          {
 *              Console.WriteLine("TStewardshipCalculationWebConnector.PerformStewardshipCalculation...");
 *          }
 */
            AVerificationResult = new TVerificationResultCollection();
            bool NewTransaction;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out NewTransaction);

            try
            {
                if (GenerateAdminFeeBatch(ALedgerNumber, APeriodNumber, false, DBTransaction, ref AVerificationResult))
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }

                    return GenerateICHStewardshipBatch(ALedgerNumber, APeriodNumber, ref AVerificationResult);
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

                    return false;
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured while performing the Stewardship Calculations:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

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
        /// <param name="AVerificationResults"></param>
        /// <returns>True if successful</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool GenerateICHStewardshipBatch(int ALedgerNumber,
            int APeriodNumber,
            ref TVerificationResultCollection AVerificationResults)
        {
            string StandardCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);

            bool IsSuccessful = false;
            bool DrCrIndicator = true;
            bool IncomeDrCrIndicator;
            bool ExpenseDrCrIndicator;
            bool AccountDrCrIndicator;

            string IncomeAccounts = string.Empty;
            string ExpenseAccounts = string.Empty;


            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            bool NewTransaction = false;
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            //Generating the ICH batch...
            try
            {
                DateTime PeriodStartDate;
                DateTime PeriodEndDate;
                TFinancialYear.GetStartAndEndDateOfPeriod(ALedgerNumber, APeriodNumber, out PeriodStartDate, out PeriodEndDate, DBTransaction);
                String strPeriodStartDate = "#" + PeriodStartDate.ToString("yyyy-MM-dd") + "#";
                String strPeriodEndDate = "#" + PeriodEndDate.ToString("yyyy-MM-dd") + "#";
                int CurrentFinancialYear = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction)[0].CurrentFinancialYear;

                AGiftBatchTable GiftBatchTable = new AGiftBatchTable();
                String GiftQuery = "SELECT * FROM a_gift_batch WHERE " +
                                   AGiftBatchTable.GetLedgerNumberDBName() + " = " + ALedgerNumber +
                                   " AND " + AGiftBatchTable.GetBatchStatusDBName() + " = '" + MFinanceConstants.BATCH_POSTED + "'" +
                                   " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " >= " + strPeriodStartDate +
                                   " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= " + strPeriodEndDate +
                                   " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName();

                DBAccess.GDBAccessObj.SelectDT(GiftBatchTable, GiftQuery, DBTransaction);

                //Create a new batch. If it turns out I don't need one, I can delete it later.
                GLBatchTDS MainDS = TGLPosting.CreateABatch(ALedgerNumber, Catalog.GetString("ICH Stewardship"), 0, PeriodEndDate);

                ABatchRow NewBatchRow = MainDS.ABatch[0];
                int GLBatchNumber = NewBatchRow.BatchNumber;

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                GLPostingTDS PostingDS = new GLPostingTDS();
                ALedgerAccess.LoadByPrimaryKey(PostingDS, ALedgerNumber, DBTransaction);
                AAccountAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
                AIchStewardshipAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
                AAccountHierarchyAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);

                ABatchTable BatchTable = new ABatchTable();

                ABatchRow BatchTemplateRow = (ABatchRow)BatchTable.NewRowTyped(false);

                BatchTemplateRow.LedgerNumber = ALedgerNumber;
                BatchTemplateRow.BatchPeriod = APeriodNumber;
                BatchTemplateRow.BatchYear = CurrentFinancialYear;

                StringCollection Operators0 = StringHelper.InitStrArr(new string[] { "=", "=" });
                StringCollection OrderList0 = new StringCollection();

                OrderList0.Add("ORDER BY");
                OrderList0.Add(ABatchTable.GetBatchNumberDBName() + " DESC");

                ABatchTable BatchesInAPeriod = ABatchAccess.LoadUsingTemplate(BatchTemplateRow,
                    Operators0,
                    null,
                    DBTransaction,
                    OrderList0,
                    0,
                    0);

                if ((BatchesInAPeriod != null) && (BatchesInAPeriod.Rows.Count > 0))
                {
                    int BatchNumber = 0;

                    for (int i = 0; i < BatchesInAPeriod.Count; i++)
                    {
                        ABatchRow batchRow = (ABatchRow)BatchesInAPeriod.Rows[i];

                        BatchNumber = batchRow.BatchNumber;

                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, BatchNumber, DBTransaction);
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, BatchNumber, DBTransaction);
                    }
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("No Batches found to process in Ledger: {0}"),
                            ALedgerNumber);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                ALedgerRow LedgerRow = (ALedgerRow)PostingDS.ALedger.Rows[0];

                //Create a new journal in the Batch
                //Run gl1120o.p
                AJournalRow NewJournalRow = MainDS.AJournal.NewRowTyped();
                NewJournalRow.LedgerNumber = ALedgerNumber;
                NewJournalRow.BatchNumber = GLBatchNumber;
                NewJournalRow.JournalNumber = ++NewBatchRow.LastJournal;
                NewJournalRow.JournalDescription = NewBatchRow.BatchDescription;
                NewJournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                NewJournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                NewJournalRow.TransactionCurrency = LedgerRow.BaseCurrency;
                NewJournalRow.ExchangeRateToBase = 1;
                NewJournalRow.DateEffective = PeriodEndDate;
                NewJournalRow.JournalPeriod = APeriodNumber;
                MainDS.AJournal.Rows.Add(NewJournalRow);

                int GLJournalNumber = NewJournalRow.JournalNumber;
                int GLTransactionNumber = NewJournalRow.LastTransactionNumber + 1;

                // ***************************
                //  Generate the transactions
                // ***************************

                AAccountRow AccountRow = (AAccountRow)PostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.INCOME_HEADING });

                //Process income accounts
                if (AccountRow != null)
                {
                    IncomeDrCrIndicator = AccountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Income Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.INCOME_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    AccountRow,
                    DBTransaction,
                    ref IncomeAccounts,
                    ref AVerificationResults);


                //Process expense accounts
                AccountRow = (AAccountRow)PostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.EXPENSE_HEADING });

                if (AccountRow != null)
                {
                    ExpenseDrCrIndicator = AccountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Expense Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.EXPENSE_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    AccountRow,
                    DBTransaction,
                    ref ExpenseAccounts,
                    ref AVerificationResults);


                //Process P&L accounts
                AccountRow = (AAccountRow)PostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.PROFIT_AND_LOSS_HEADING });

                if (AccountRow != null)
                {
                    AccountDrCrIndicator = AccountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Profit & Loss Account header: '{1}' not found in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.PROFIT_AND_LOSS_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                // find out the stewardship number - Ln 275
                // Increment the Last ICH No.
                int ICHProcessing = ++LedgerRow.LastIchNumber;
                decimal ICHTotal = 0;
                bool PostICHBatch = false;

                ACostCentreRow CCTemplateRow = PostingDS.ACostCentre.NewRowTyped(false);
                CCTemplateRow.LedgerNumber = ALedgerNumber;
                CCTemplateRow.PostingCostCentreFlag = true;
                CCTemplateRow.CostCentreType = MFinanceConstants.FOREIGN_CC_TYPE;

                ACostCentreAccess.LoadUsingTemplate(PostingDS, CCTemplateRow, DBTransaction);

                //Iterate through the cost centres

                AIchStewardshipTable ICHStewardshipTable = new AIchStewardshipTable();
                Boolean NonIchTransactionsIncluded = false;

                String JournalRowOrder = "a_journal_number_i";
                String TransRowOrder = "a_batch_number_i,a_journal_number_i,a_transaction_number_i";

                foreach (ACostCentreRow CostCentreRow in PostingDS.ACostCentre.Rows)
                {
                    //
                    // To do this work I will need to have a clearling account specified:
                    if (CostCentreRow.ClearingAccount == "")
                    {
                        AVerificationResults.Add(new TVerificationResult("Generate ICH Stewardship Batch",
                                String.Format(Catalog.GetString("Fault: Cost Centre {0} ({1}) has no Clearing Account Specified."),
                                    CostCentreRow.CostCentreCode, CostCentreRow.CostCentreName),
                                TResultSeverity.Resv_Critical));
                        continue; // The batch is doomed, but I'll continue to look for more faults.
                    }

                    //Initialise values for each Cost Centre
                    decimal SettlementAmount = 0;
                    decimal IncomeAmount = 0;
                    decimal ExpenseAmount = 0;
                    decimal XferAmount = 0;
                    decimal IncomeAmountIntl = 0;
                    decimal ExpenseAmountIntl = 0;
                    decimal XferAmountIntl = 0;

                    Boolean TransferFound = false;

                    /* 0008 Go through all of the transactions. Ln:301 */
                    String WhereClause = "a_cost_centre_code_c = '" + CostCentreRow.CostCentreCode +
                                         "' AND a_transaction_status_l=true AND a_ich_number_i = 0";

                    DataRow[] FoundTransRows = MainDS.ATransaction.Select(WhereClause, TransRowOrder);

                    foreach (DataRow UntypedTransRow in FoundTransRows)
                    {
                        ATransactionRow TransRow = (ATransactionRow)UntypedTransRow;

                        DataRow[] FoundJournalRows = MainDS.AJournal.Select(
                            "a_batch_number_i = " + TransRow.BatchNumber + " AND a_journal_number_i = " + TransRow.JournalNumber,
                            JournalRowOrder);

                        if (FoundJournalRows != null)
                        {
                            TransferFound = true;
                            PostICHBatch = true;
                            TransRow.IchNumber = ICHProcessing;

                            if (TransRow.DebitCreditIndicator == AccountDrCrIndicator)
                            {
                                SettlementAmount -= TransRow.AmountInBaseCurrency;
                            }
                            else
                            {
                                SettlementAmount += TransRow.AmountInBaseCurrency;
                            }

                            //Process Income (ln:333)
                            if (IncomeAccounts.Contains(TransRow.AccountCode))
                            {
                                if (TransRow.DebitCreditIndicator == IncomeDrCrIndicator)
                                {
                                    IncomeAmount += TransRow.AmountInBaseCurrency;
                                    IncomeAmountIntl += TransRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    IncomeAmount -= TransRow.AmountInBaseCurrency;
                                    IncomeAmountIntl -= TransRow.AmountInIntlCurrency;
                                }
                            }

                            //process expenses
                            if (ExpenseAccounts.Contains(TransRow.AccountCode)
                                && (TransRow.AccountCode != MFinanceConstants.DIRECT_XFER_ACCT)
                                && (TransRow.AccountCode != MFinanceConstants.ICH_ACCT_SETTLEMENT))
                            {
                                if (TransRow.DebitCreditIndicator == ExpenseDrCrIndicator)
                                {
                                    ExpenseAmount += TransRow.AmountInBaseCurrency;
                                    ExpenseAmountIntl += TransRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    ExpenseAmount -= TransRow.AmountInBaseCurrency;
                                    ExpenseAmountIntl -= TransRow.AmountInIntlCurrency;
                                }
                            }

                            //Process Direct Transfers
                            if (TransRow.AccountCode == MFinanceConstants.DIRECT_XFER_ACCT)
                            {
                                if (TransRow.DebitCreditIndicator == ExpenseDrCrIndicator)
                                {
                                    XferAmount += TransRow.AmountInBaseCurrency;
                                    XferAmountIntl += TransRow.AmountInIntlCurrency;
                                }
                                else
                                {
                                    XferAmount -= TransRow.AmountInBaseCurrency;
                                    XferAmountIntl -= TransRow.AmountInIntlCurrency;
                                }
                            }
                        }
                    }  //end of foreach transaction

                    /* now mark all the gifts as processed */
                    if (TransferFound)
                    {
                        AGiftDetailTable GiftDetailTable = new AGiftDetailTable();
                        AGiftDetailRow GiftDetailTemplateRow = (AGiftDetailRow)GiftDetailTable.NewRowTyped(false);

                        GiftDetailTemplateRow.LedgerNumber = ALedgerNumber;
                        GiftDetailTemplateRow.IchNumber = 0;
                        GiftDetailTemplateRow.CostCentreCode = CostCentreRow.CostCentreCode;

                        foreach (AGiftBatchRow GiftBatchRow in GiftBatchTable.Rows)
                        {
                            GiftDetailTemplateRow.BatchNumber = GiftBatchRow.BatchNumber;
                            GiftDetailTable = AGiftDetailAccess.LoadUsingTemplate(GiftDetailTemplateRow, DBTransaction);

                            foreach (AGiftDetailRow GiftDetailRow in GiftDetailTable.Rows)
                            {
                                GiftDetailRow.IchNumber = ICHProcessing;
                            }
                            AGiftDetailAccess.SubmitChanges(GiftDetailTable, DBTransaction);
                        }
                    } // if TransferFound

                    if ((SettlementAmount == 0) // If there's no activity in this CC,
                        && (IncomeAmount == 0)  // bail to the next one.
                        && (ExpenseAmount == 0)
                        && (XferAmount == 0))
                    {
                        continue;
                    }

                    /* Balance the cost centre by entering an opposite transaction
                     * to ICH settlement. Use positive amounts only.
                     */

                    /* Increment or decrement the ICH total to be transferred after this loop.
                     * NOTE - if this is a "non-ICH fund", I need to balance it separately, and I'll do that right here.
                     */
                    DrCrIndicator = AccountRow.DebitCreditIndicator;

                    if (CostCentreRow.ClearingAccount == MFinanceConstants.ICH_ACCT_ICH)
                    {
                        if (DrCrIndicator == MFinanceConstants.IS_DEBIT)
                        {
                            ICHTotal += SettlementAmount;
                        }
                        else
                        {
                            ICHTotal -= SettlementAmount;
                        }
                    }

                    DrCrIndicator = AccountDrCrIndicator;

                    if (SettlementAmount < 0)
                    {
                        DrCrIndicator = !AccountDrCrIndicator;
                        SettlementAmount = 0 - SettlementAmount;
                    }

                    if ((CostCentreRow.ClearingAccount != MFinanceConstants.ICH_ACCT_ICH) && (SettlementAmount != 0))
                    {
                        // I'm creating a transaction right here for this "non-ICH" CostCentre.
                        // This potentially means that there will be multiple transactions to the "non-ICH" account,
                        // whereas the ICH account has only a single transaction, but that's not big deal:

                        if (!TGLPosting.CreateATransaction(MainDS, ALedgerNumber, GLBatchNumber, GLJournalNumber,
                                Catalog.GetString("Non-ICH foreign fund Clearing"),
                                CostCentreRow.ClearingAccount,
                                StandardCostCentre, SettlementAmount, PeriodEndDate, !DrCrIndicator, Catalog.GetString("Non-ICH"),
                                true, SettlementAmount,
                                out GLTransactionNumber))
                        {
                            ErrorContext = Catalog.GetString("Generating non-ICH transaction");
                            ErrorMessage =
                                String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    GLBatchNumber,
                                    GLJournalNumber);
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }

                        NonIchTransactionsIncluded = true;
                    }

                    /* Generate the transaction to 'balance' the foreign fund -
                     *  in the ICH settlement account.
                     */

                    //RUN gl1130o.p ("new":U,
                    //Create a transaction
                    if (SettlementAmount > 0)
                    {
                        if (!TGLPosting.CreateATransaction(MainDS, ALedgerNumber, GLBatchNumber, GLJournalNumber,
                                Catalog.GetString("ICH Monthly Clearing"),
                                MFinanceConstants.ICH_ACCT_SETTLEMENT, // DestinationAccount[CostCentreRow.CostCentreCode],
                                CostCentreRow.CostCentreCode, SettlementAmount, PeriodEndDate, DrCrIndicator,
                                Catalog.GetString("ICH Process"), true, SettlementAmount,
                                out GLTransactionNumber))
                        {
                            ErrorContext = Catalog.GetString("Generating the ICH batch");
                            ErrorMessage =
                                String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                    ALedgerNumber,
                                    GLBatchNumber,
                                    GLJournalNumber);
                            ErrorType = TResultSeverity.Resv_Noncritical;
                            throw new System.InvalidOperationException(ErrorMessage);
                        }

                        //Mark as processed
                        ATransactionRow TransRow =
                            (ATransactionRow)MainDS.ATransaction.Rows.Find(new object[] { ALedgerNumber, GLBatchNumber, GLJournalNumber,
                                                                                          GLTransactionNumber });
                        TransRow.IchNumber = ICHProcessing;
                    }

                    /* Now create corresponding report row on stewardship table,
                     * Only for Cost Centres that cleared to ICH
                     */
                    if ((CostCentreRow.ClearingAccount == MFinanceConstants.ICH_ACCT_ICH)
                        && ((IncomeAmount != 0)
                            || (ExpenseAmount != 0)
                            || (XferAmount != 0)))
                    {
                        AIchStewardshipRow ICHStewardshipRow = ICHStewardshipTable.NewRowTyped(true);

                        //MainDS.Tables.Add(IchStewardshipTable);

                        ICHStewardshipRow.LedgerNumber = ALedgerNumber;
                        ICHStewardshipRow.Year = LedgerRow.CurrentFinancialYear;
                        ICHStewardshipRow.PeriodNumber = APeriodNumber;
                        ICHStewardshipRow.IchNumber = ICHProcessing;
                        ICHStewardshipRow.DateProcessed = DateTime.Today;
                        ICHStewardshipRow.CostCentreCode = CostCentreRow.CostCentreCode;
                        ICHStewardshipRow.IncomeAmount = IncomeAmount;
                        ICHStewardshipRow.ExpenseAmount = ExpenseAmount;
                        ICHStewardshipRow.DirectXferAmount = XferAmount;
                        ICHStewardshipRow.IncomeAmountIntl = IncomeAmountIntl;
                        ICHStewardshipRow.ExpenseAmountIntl = ExpenseAmountIntl;
                        ICHStewardshipRow.DirectXferAmountIntl = XferAmountIntl;
                        ICHStewardshipTable.Rows.Add(ICHStewardshipRow);
                    }
                }   // for each cost centre

                //
                // If I already have critical errors,
                // I need to unwind and tidy up:
                if (AVerificationResults.HasCriticalErrors)
                {
                    // An empty GL Batch now exists, which I need to delete.
                    //
                    TVerificationResultCollection BatchCancelResult = new TVerificationResultCollection();

                    TGLPosting.DeleteGLBatch(
                        ALedgerNumber,
                        GLBatchNumber,
                        out BatchCancelResult);
                    AVerificationResults.AddCollection(BatchCancelResult);
                }
                else
                {
                    /* Update the balance of the ICH account (like a bank account).
                     * If the total is negative, it means the ICH batch has a
                     * credit total so far. Thus, we now balance it with the opposite
                     * transaction. */

                    if (ICHTotal < 0)
                    {
                        DrCrIndicator = MFinanceConstants.IS_DEBIT;
                        ICHTotal = -ICHTotal;
                    }
                    else if (ICHTotal > 0)
                    {
                        DrCrIndicator = MFinanceConstants.IS_CREDIT;
                    }

                    /* 0006 - If the balance is 0 then this is ok
                     *  (eg last minute change of a gift from one field to another)
                     */

                    if ((ICHTotal == 0) && !NonIchTransactionsIncluded)
                    {
                        AVerificationResults.Add(new TVerificationResult(Catalog.GetString("Generating the ICH batch"),
                                Catalog.GetString("No ICH batch was required."), TResultSeverity.Resv_Status));

                        // An empty GL Batch now exists, which I need to delete.
                        //
                        TVerificationResultCollection BatchCancelResult = new TVerificationResultCollection();

                        TGLPosting.DeleteGLBatch(
                            ALedgerNumber,
                            GLBatchNumber,
                            out BatchCancelResult);
                        AVerificationResults.AddCollection(BatchCancelResult);

                        IsSuccessful = true;
                    }
                    else
                    {
                        if (ICHTotal != 0)
                        {
                            //Create a transaction
                            if (!TGLPosting.CreateATransaction(MainDS, ALedgerNumber, GLBatchNumber, GLJournalNumber,
                                    Catalog.GetString("ICH Monthly Clearing"),
                                    MFinanceConstants.ICH_ACCT_ICH, StandardCostCentre, ICHTotal, PeriodEndDate, DrCrIndicator,
                                    Catalog.GetString("ICH"),
                                    true, ICHTotal,
                                    out GLTransactionNumber))
                            {
                                ErrorContext = Catalog.GetString("Generating the ICH batch");
                                ErrorMessage =
                                    String.Format(Catalog.GetString("Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                        ALedgerNumber,
                                        GLBatchNumber,
                                        GLJournalNumber);
                                ErrorType = TResultSeverity.Resv_Noncritical;
                                throw new System.InvalidOperationException(ErrorMessage);
                            }
                        }

                        //Post the batch
                        if (PostICHBatch)
                        {
                            AIchStewardshipAccess.SubmitChanges(ICHStewardshipTable, DBTransaction);

                            MainDS.ThrowAwayAfterSubmitChanges = true; // SubmitChanges will not return to me any changes made in MainDS.
                            GLBatchTDSAccess.SubmitChanges(MainDS);
                            ALedgerAccess.SubmitChanges(PostingDS.ALedger, DBTransaction);

                            // refresh cached ICHStewardship table
                            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                                TCacheableFinanceTablesEnum.ICHStewardshipList.ToString());

                            IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, GLBatchNumber, out AVerificationResults);
                        }
                        else
                        {
                            AVerificationResults.Add(new TVerificationResult(ErrorContext,
                                    Catalog.GetString("No Stewardship batch is required."),
                                    TResultSeverity.Resv_Status));

                            // An empty GL Batch now exists, which I need to delete.
                            //
                            TVerificationResultCollection BatchCancelResult = new TVerificationResultCollection();

                            TGLPosting.DeleteGLBatch(
                                ALedgerNumber,
                                GLBatchNumber,
                                out BatchCancelResult);
                            AVerificationResults.AddCollection(BatchCancelResult);
                        } // else

                    } // else
                } // else
            } // try
            catch (ArgumentException Exc)
            {
                TLogging.Log("An ArgumentException occured during the generation of the Stewardship Batch:" + Environment.NewLine + Exc.ToString());

                if (AVerificationResults == null)
                {
                    AVerificationResults = new TVerificationResultCollection();
                }

                AVerificationResults.Add(new TVerificationResult(ErrorContext, Exc.Message, ErrorType));

                throw;
            }
            catch (InvalidOperationException Exc)
            {
                TLogging.Log(
                    "An InvalidOperationException occured during the generation of the Stewardship Batch:" + Environment.NewLine + Exc.ToString());

                if (AVerificationResults == null)
                {
                    AVerificationResults = new TVerificationResultCollection();
                }

                AVerificationResults.Add(new TVerificationResult(ErrorContext, Exc.Message, ErrorType));

                throw;
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the generation of the Stewardship Batch:" + Environment.NewLine + Exc.ToString());

                ErrorContext = Catalog.GetString("Calculate Admin Fee");
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while generating the ICH batch for Ledger: {0} and Period: {1}" +
                        Environment.NewLine + Environment.NewLine + Exc.ToString()),
                    ALedgerNumber,
                    APeriodNumber);
                ErrorType = TResultSeverity.Resv_Critical;

                if (AVerificationResults == null)
                {
                    AVerificationResults = new TVerificationResultCollection();
                }

                AVerificationResults.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));

                throw;
            }
            finally
            {
                if (IsSuccessful && NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else if (!IsSuccessful && NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return IsSuccessful;
        } // Generate ICH Stewardship Batch

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
                                String.Format(Catalog.GetString("No admin fees charged in period {0}."), APeriodNumber),
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
                                            pFR2[AProcessedFeeTable.GetPeriodicAmountDBName()]), NumDecPlaces);        //pFR2.PeriodicAmount; //ROUND(pFR2.PeriodicAmount,
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

                                            /*
                                             * Generate the transaction to deduct the fee amount from the source cost centre. (Expense leg)
                                             */
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
                         * creating an entry if necessary. (This is for the income total)
                         */
                        GLStewardshipCalculationTDSCreditFeeTotalRow CreditFeeTotalRow = (GLStewardshipCalculationTDSCreditFeeTotalRow)
                                                                                         CreditFeeTotalDT.Rows.Find(new object[] { DestCostCentreCode,
                                                                                                                                   DestAccountCode });

                        if (CreditFeeTotalRow != null)
                        {
                            CreditFeeTotalRow.TransactionAmount += Math.Round(pFR.PeriodicAmount, NumDecPlaces);
                        }
                        else
                        {
                            CreditFeeTotalRow = CreditFeeTotalDT.NewRowTyped();
                            CreditFeeTotalRow.CostCentreCode = DestCostCentreCode;
                            CreditFeeTotalRow.AccountCode = DestAccountCode;
                            CreditFeeTotalRow.TransactionAmount = Math.Round(pFR.PeriodicAmount, NumDecPlaces);
                            CreditFeeTotalDT.Rows.Add(CreditFeeTotalRow);
                        }
                    }

                    /* Generate the transaction to credit the fee amounts to
                     * the destination accounts. (Income leg)
                     */
                    for (int k = 0; k < CreditFeeTotalDT.Count; k++)
                    {
                        GLStewardshipCalculationTDSCreditFeeTotalRow cFT = (GLStewardshipCalculationTDSCreditFeeTotalRow)
                                                                           CreditFeeTotalDT.Rows[k];

                        if (cFT.TransactionAmount < 0)
                        {
                            /* The case of a negative gift total should be very rare.
                             * It would only happen if, for instance, the was only
                             * a reversal but no new gifts for a certain ledger.
                             */
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

                        GLBatchTDSAccess.SubmitChanges(AdminFeeDS);

                        IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, BatchRow.BatchNumber, out Verification);

                        if (IsSuccessful)
                        {
                            AProcessedFeeAccess.SubmitChanges(ProcessedFeeDataTable, ADBTransaction);
                        }
                    }

                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(Verification))
                    {
                        //Petra error: GL0067
                        ErrorContext = Catalog.GetString("Posting Admin Fee Batch");
                        ErrorMessage = String.Format(Catalog.GetString("The posting of the admin fee batch failed."));
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                    }

                    //End of Transaction block in 4GL

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
                ErrorMessage = String.Format(Catalog.GetString("Error while generating admin fee batch for Ledger {0}:" +
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