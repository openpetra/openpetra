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
            if (TLogging.DL >= 9)
            {
                //Console.WriteLine(this.GetType().FullName + ": PerformStewardshipCalculation called.");
            }

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

                    if (TLogging.DL >= 8)
                    {
                        //Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction committed!");
                    }

                    return GenerateICHStewardshipBatch(ALedgerNumber, APeriodNumber, ref AVerificationResult);
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

                    if (TLogging.DL >= 8)
                    {
                        //Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because of an error!");
                    }

                    return false;
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                if (TLogging.DL >= 8)
                {
                    //Console.WriteLine(this.GetType().FullName + ".PerformStewardshipCalculation: Transaction ROLLED BACK because an exception occurred!");
                }

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
        ///  Create the journal to create the transfer transactions in, if there are
        ///  amounts to transfer.
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
            string StandardCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);

            bool IsSuccessful = false;
            bool DrCrIndicator = true;
            bool IncomeDrCrIndicator;
            bool ExpenseDrCrIndicator;
            bool AccountDrCrIndicator;

            string IncomeAccounts = string.Empty;
            string ExpenseAccounts = string.Empty;

            int ICHProcessing;
            decimal ICHTotal = 0;

            decimal SettlementAmount;
            decimal IncomeAmount;
            decimal ExpenseAmount;
            decimal XferAmount;
            decimal IncomeAmount2;
            decimal ExpenseAmount2;
            decimal XferAmount2;

            bool TransferFound = false;
            bool PostICHBatch = false;

            string CurrentAccountCode;
            decimal AmountInBaseCurrency;
            decimal AmountInIntlCurrency;

            DateTime PeriodStartDate;
            DateTime PeriodEndDate;

            int GLBatchNumber = 0;
            int GLJournalNumber = 0;
            int GLTransactionNumber = 0;
            string BatchDescription = string.Empty; //"ICH Clearing";

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
                TFinancialYear.GetStartAndEndDateOfPeriod(ALedgerNumber, APeriodNumber, out PeriodStartDate, out PeriodEndDate, DBTransaction);

                //Create a new batch
                GLBatchTDS MainDS = TGLPosting.CreateABatch(ALedgerNumber);

                ABatchRow NewBatchRow = MainDS.ABatch[0];
                GLBatchNumber = NewBatchRow.BatchNumber;

                //Run RUN gl1110o.p to create a batch
                BatchDescription = Catalog.GetString("ICH Stewardship");
                NewBatchRow.BatchDescription = BatchDescription;
                NewBatchRow.DateEffective = PeriodEndDate;

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                GLPostingTDS PostingDS = new GLPostingTDS();
                ALedgerAccess.LoadByPrimaryKey(PostingDS, ALedgerNumber, DBTransaction);
                AAccountAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
                ACostCentreAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
                AIchStewardshipAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
                AAccountHierarchyAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);

                ATransactionAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                AJournalAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);

                ALedgerRow LedgerRow = (ALedgerRow)PostingDS.ALedger.Rows[0];

                //Create a new journal in the Batch
                //Run gl1120o.p
                AJournalRow NewJournalRow = MainDS.AJournal.NewRowTyped();
                NewJournalRow.LedgerNumber = ALedgerNumber;
                NewJournalRow.BatchNumber = GLBatchNumber;
                NewJournalRow.JournalNumber = ++NewBatchRow.LastJournal;
                NewJournalRow.JournalDescription = BatchDescription;
                NewJournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                NewJournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                NewJournalRow.TransactionCurrency = LedgerRow.BaseCurrency;
                NewJournalRow.ExchangeRateToBase = 1;
                NewJournalRow.DateEffective = PeriodEndDate;
                NewJournalRow.JournalPeriod = APeriodNumber;
                MainDS.AJournal.Rows.Add(NewJournalRow);

                GLJournalNumber = NewJournalRow.JournalNumber;
                GLTransactionNumber = NewJournalRow.LastTransactionNumber + 1;

                // ***************************
                //  Generate the transactions
                // ***************************
                // Ln: 242

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
                        String.Format(Catalog.GetString("Income Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.INCOME_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    AccountRow,
                    DBTransaction,
                    ref IncomeAccounts,
                    ref AVerificationResult);


                //Process expense accounts
                AccountRow = null;
                AccountRow = (AAccountRow)PostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.EXPENSE_HEADING });

                if (AccountRow != null)
                {
                    ExpenseDrCrIndicator = AccountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Expense Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.EXPENSE_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                BuildChildAccountList(ALedgerNumber,
                    AccountRow,
                    DBTransaction,
                    ref ExpenseAccounts,
                    ref AVerificationResult);


                //Process P&L accounts
                AccountRow = null;
                AccountRow = (AAccountRow)PostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, MFinanceConstants.PROFIT_AND_LOSS_HEADING });

                if (AccountRow != null)
                {
                    AccountDrCrIndicator = AccountRow.DebitCreditIndicator;
                }
                else
                {
                    ErrorContext = Catalog.GetString("Generating the ICH batch");
                    ErrorMessage =
                        String.Format(Catalog.GetString("Profit & Loss Account header: '{1}' does not appear in the accounts table for Ledger: {0}."),
                            ALedgerNumber,
                            MFinanceConstants.PROFIT_AND_LOSS_HEADING);
                    ErrorType = TResultSeverity.Resv_Noncritical;
                    throw new System.InvalidOperationException(ErrorMessage);
                }

                // find out the stewardship number - Ln 275
                // Increment the Last ICH No.
                ICHProcessing = ++LedgerRow.LastIchNumber;


                //Iterate through the cost centres
                string WhereClause = ACostCentreTable.GetPostingCostCentreFlagDBName() + " = True" +
                                     " AND " + ACostCentreTable.GetCostCentreTypeDBName() +
                                     " LIKE '" + MFinanceConstants.FOREIGN_CC_TYPE + "'";
                string OrderBy = ACostCentreTable.GetCostCentreCodeDBName();

                DataRow[] FoundCCRows = PostingDS.ACostCentre.Select(WhereClause, OrderBy);

                AIchStewardshipTable IchStewardshipTable = new AIchStewardshipTable();

                foreach (DataRow untypedCCRow in FoundCCRows)
                {
                    ACostCentreRow CostCentreRow = (ACostCentreRow)untypedCCRow;

                    string CostCentre = CostCentreRow.CostCentreCode;

                    //Initialise values for each Cost Centre
                    SettlementAmount = 0;
                    IncomeAmount = 0;
                    ExpenseAmount = 0;
                    XferAmount = 0;
                    IncomeAmount2 = 0;
                    ExpenseAmount2 = 0;
                    XferAmount2 = 0;

                    TransferFound = false;

                    /* 0008 Go through all of the transactions. Ln:301 */
                    WhereClause = ATransactionTable.GetCostCentreCodeDBName() + " = '" + CostCentre + "'" +
                                  " AND " + ATransactionTable.GetTransactionStatusDBName() + " = " + MFinanceConstants.POSTED +
                                  " AND " + ATransactionTable.GetIchNumberDBName() + " = 0";

                    OrderBy = ATransactionTable.GetBatchNumberDBName() + ", " +
                              ATransactionTable.GetJournalNumberDBName() + ", " +
                              ATransactionTable.GetTransactionNumberDBName();

                    DataRow[] FoundTransRows = MainDS.ATransaction.Select(WhereClause, OrderBy);

                    foreach (DataRow untypedTransRow in FoundTransRows)
                    {
                        ATransactionRow TransactionRow = (ATransactionRow)untypedTransRow;

                        CurrentAccountCode = TransactionRow.AccountCode;
                        AmountInBaseCurrency = TransactionRow.AmountInBaseCurrency;
                        AmountInIntlCurrency = TransactionRow.AmountInIntlCurrency;

                        WhereClause = AJournalTable.GetLedgerNumberDBName() + " = " + TransactionRow.LedgerNumber.ToString() +
                                      " AND " + AJournalTable.GetBatchNumberDBName() + " = " + TransactionRow.BatchNumber.ToString() +
                                      " AND " + AJournalTable.GetJournalNumberDBName() + " = " + TransactionRow.JournalNumber.ToString() +
                                      " AND " + AJournalTable.GetJournalPeriodDBName() + " = " + APeriodNumber;

                        OrderBy = AJournalTable.GetBatchNumberDBName() + ", " + AJournalTable.GetJournalNumberDBName();

                        DataRow[] FoundJournalRows = MainDS.AJournal.Select(WhereClause, OrderBy);

                        if (FoundJournalRows != null)
                        {
                            TransferFound = true;
                            PostICHBatch = true;
                            TransactionRow.IchNumber = ICHProcessing;

                            if (TransactionRow.DebitCreditIndicator == AccountDrCrIndicator)
                            {
                                SettlementAmount -= AmountInBaseCurrency;
                            }
                            else
                            {
                                SettlementAmount += AmountInBaseCurrency;
                            }

                            //Process Income (ln:333)
                            if (IncomeAccounts.Contains(CurrentAccountCode))
                            {
                                if (TransactionRow.DebitCreditIndicator == IncomeDrCrIndicator)
                                {
                                    IncomeAmount += AmountInBaseCurrency;
                                    IncomeAmount2 += AmountInIntlCurrency;
                                }
                                else
                                {
                                    IncomeAmount -= AmountInBaseCurrency;
                                    IncomeAmount2 -= AmountInIntlCurrency;
                                }
                            }

                            //process expenses
                            if (ExpenseAccounts.Contains(CurrentAccountCode)
                                && (CurrentAccountCode != MFinanceConstants.DIRECT_XFER_ACCT)
                                && (CurrentAccountCode != MFinanceConstants.ICH_ACCT_SETTLEMENT))
                            {
                                if (TransactionRow.DebitCreditIndicator = ExpenseDrCrIndicator)
                                {
                                    ExpenseAmount += AmountInBaseCurrency;
                                    ExpenseAmount2 += AmountInIntlCurrency;
                                }
                                else
                                {
                                    ExpenseAmount -= AmountInBaseCurrency;
                                    ExpenseAmount2 -= AmountInIntlCurrency;
                                }
                            }

                            //Process Direct Transfers
                            if (CurrentAccountCode == MFinanceConstants.DIRECT_XFER_ACCT)
                            {
                                if (TransactionRow.DebitCreditIndicator == ExpenseDrCrIndicator)
                                {
                                    XferAmount += AmountInBaseCurrency;
                                    XferAmount2 += AmountInIntlCurrency;
                                }
                                else
                                {
                                    XferAmount -= AmountInBaseCurrency;
                                    XferAmount2 -= AmountInIntlCurrency;
                                }
                            }
                        }
                    }  //end of foreach transaction

                    /* now mark all the gifts as processed */
                    if (TransferFound)
                    {
                        WhereClause = AGiftBatchTable.GetBatchStatusDBName() + " = '" + MFinanceConstants.BATCH_POSTED + "'" +
                                      " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " >= #" + PeriodStartDate.ToString("yyyy-MM-dd") + "#" +
                                      " AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= #" + PeriodEndDate.ToString("yyyy-MM-dd") + "#";

                        OrderBy = AGiftBatchTable.GetBatchNumberDBName();

                        AGiftBatchTable GiftBatchTable = AGiftBatchAccess.LoadViaALedger(ALedgerNumber, DBTransaction);

                        DataRow[] FoundGiftBatchRows = GiftBatchTable.Select(WhereClause, OrderBy);

                        AGiftBatchRow GiftBatchRow = null;
                        AGiftDetailTable GiftDetailTable = new AGiftDetailTable();
                        AGiftDetailRow GiftDetailRow = null;
                        int BatchNumber = 0;

                        foreach (DataRow untypedTransRow in FoundGiftBatchRows)
                        {
                            GiftBatchRow = (AGiftBatchRow)untypedTransRow;
                            BatchNumber = GiftBatchRow.BatchNumber;

                            AGiftDetailRow TemplateRow = (AGiftDetailRow)GiftDetailTable.NewRowTyped(false);

                            TemplateRow.LedgerNumber = ALedgerNumber;
                            TemplateRow.BatchNumber = BatchNumber;
                            TemplateRow.IchNumber = 0;
                            TemplateRow.CostCentreCode = CostCentre;

                            StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
                            StringCollection OrderList = new StringCollection();

                            OrderList.Add("ORDER BY");
                            OrderList.Add(AGiftDetailTable.GetGiftTransactionNumberDBName() + " ASC");
                            OrderList.Add(AGiftDetailTable.GetDetailNumberDBName() + " ASC");

                            AGiftDetailTable GiftDetailTable2 = AGiftDetailAccess.LoadUsingTemplate(TemplateRow,
                                operators,
                                null,
                                DBTransaction,
                                OrderList,
                                0,
                                0);

                            if (GiftDetailTable2 != null)
                            {
                                for (int k = 0; k < GiftDetailTable2.Count; k++)
                                {
                                    GiftDetailRow = (AGiftDetailRow)GiftDetailTable2.Rows[k];
                                    GiftDetailRow.IchNumber = ICHProcessing;
                                }
                            }
                        }
                    }

                    /* Balance the cost centre by entering an opposite transaction
                     *  to ICH settlement. Use positive amounts only. */

                    /* Increment or decrement the ICH total to be transferred. */
                    DrCrIndicator = AccountRow.DebitCreditIndicator;

                    if (DrCrIndicator == MFinanceConstants.IS_DEBIT)
                    {
                        ICHTotal += SettlementAmount;
                    }
                    else
                    {
                        ICHTotal -= SettlementAmount;
                    }

                    if (SettlementAmount < 0)
                    {
                        DrCrIndicator = !AccountDrCrIndicator;
                        SettlementAmount = -SettlementAmount;
                    }
                    else if (SettlementAmount > 0)
                    {
                        DrCrIndicator = AccountDrCrIndicator;
                    }

                    /* Generate the transction to 'balance' the foreign fund -
                     *  in the ICH settlement account. */
                    //RUN gl1130o.p ("new":U,
                    //Create a transaction
                    if (SettlementAmount > 0)
                    {
                        if (!TGLPosting.CreateATransaction(MainDS, ALedgerNumber, GLBatchNumber, GLJournalNumber,
                                Catalog.GetString("ICH Monthly Clearing"),
                                MFinanceConstants.ICH_ACCT_SETTLEMENT, CostCentre, SettlementAmount, PeriodEndDate, DrCrIndicator,
                                Catalog.GetString("ICH"), true, 0,
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

                    /* can now create corresponding report row on stewardship table */
                    if ((IncomeAmount != 0)
                        || (ExpenseAmount != 0)
                        || (XferAmount != 0))
                    {
                        AIchStewardshipRow IchStewardshipRow = IchStewardshipTable.NewRowTyped(true);

                        //MainDS.Tables.Add(IchStewardshipTable);

                        IchStewardshipRow.LedgerNumber = ALedgerNumber;
                        IchStewardshipRow.PeriodNumber = APeriodNumber;
                        IchStewardshipRow.IchNumber = ICHProcessing;
                        IchStewardshipRow.DateProcessed = DateTime.Today;
                        IchStewardshipRow.CostCentreCode = CostCentre;
                        IchStewardshipRow.IncomeAmount = IncomeAmount;
                        IchStewardshipRow.ExpenseAmount = ExpenseAmount;
                        IchStewardshipRow.DirectXferAmount = XferAmount;
                        IchStewardshipRow.IncomeAmountIntl = IncomeAmount2;
                        IchStewardshipRow.ExpenseAmountIntl = ExpenseAmount2;
                        IchStewardshipRow.DirectXferAmountIntl = XferAmount2;
                        IchStewardshipTable.Rows.Add(IchStewardshipRow);
                    }
                }   // for each cost centre

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

                /* 0006 - If the balance is 0 then this is ok (eg last minute
                 *  change of a gift from one field to another)  */
                //IF lv_ich_total_n NE 0 THEN DO:
                //RUN gl1130o.p
                if (ICHTotal != 0)
                {
                    //Create a transaction
                    if (!TGLPosting.CreateATransaction(MainDS, ALedgerNumber, GLBatchNumber, GLJournalNumber,
                            Catalog.GetString("ICH Monthly Clearing"),
                            MFinanceConstants.ICH_ACCT_ICH, StandardCostCentre, ICHTotal, PeriodEndDate, DrCrIndicator, Catalog.GetString("ICH"),
                            true, 0,
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

                    //Post the batch
                    if (PostICHBatch)
                    {
                        IsSuccessful = AIchStewardshipAccess.SubmitChanges(IchStewardshipTable, DBTransaction, out AVerificationResult);

                        if (IsSuccessful)
                        {
                            IsSuccessful = (TSubmitChangesResult.scrOK == GLBatchTDSAccess.SubmitChanges(MainDS, out AVerificationResult));

                            if (IsSuccessful)
                            {
                                IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, GLBatchNumber, out AVerificationResult);
                            }
                        }
                    }
                    else
                    {
                        AVerificationResult.Add(new TVerificationResult(ErrorContext,
                                Catalog.GetString("There have not been any transactions for the stewardship run"),
                                TResultSeverity.Resv_Status));
                    }
                }
            }
            catch (ArgumentException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ErrorContext = Catalog.GetString("Calculate Admin Fee");
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while Generating the ICH batch for Ledger: {0} and Period: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber,
                    APeriodNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                Console.WriteLine(ex.Message);
            }

            if (IsSuccessful && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (!IsSuccessful && NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return IsSuccessful;
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
            /*-----gl2150.p - begin-----*/
            //Return value
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
                    }

                    IsSuccessful = true;
                }
                else
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
                    JournalRow.JournalNumber = ++AdminFeeBatch.LastJournal;

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
                                                    AdminFeeBatch.BatchNumber,
                                                    GLJournalNumber,
                                                    "Fee: " + FeeDescription + " (" + CurrentFeeCode + ")",
                                                    DrAccountCode,
                                                    CostCentreCode,
                                                    DrFeeTotal,
                                                    AccountingPeriodRow.PeriodEndDate,
                                                    DrCrIndicator,
                                                    "AG",
                                                    true,
                                                    0,
                                                    out GLTransactionNumber))
                                            {
                                                ErrorContext = Catalog.GetString("Generating the Admin Fee batch");
                                                ErrorMessage =
                                                    String.Format(Catalog.GetString(
                                                            "Unable to create a new transaction for Ledger {0}, Batch {1} and Journal {2}."),
                                                        ALedgerNumber,
                                                        AdminFeeBatch.BatchNumber,
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
                            CreditFeeTotalDR.TransactionAmount = pFR.PeriodicAmount;
                            CreditFeeTotalDT.Rows.Add(CreditFeeTotalDR);
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
                                AdminFeeBatch.BatchNumber,
                                JournalRow.JournalNumber,
                                "Collected admin charges",
                                cFT.AccountCode,
                                cFT.CostCentreCode,
                                TransactionAmount,
                                AccountingPeriodRow.PeriodEndDate,
                                DrCrIndicator,
                                "AG",
                                true,
                                0,
                                out GLTransactionNumber);
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

                        IsSuccessful = (TSubmitChangesResult.scrOK == GLBatchTDSAccess.SubmitChanges(AdminFeeDS, out Verification));

                        if (IsSuccessful)
                        {
                            IsSuccessful = TGLPosting.PostGLBatch(ALedgerNumber, AdminFeeBatch.BatchNumber, out Verification);
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