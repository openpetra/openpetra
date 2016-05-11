//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//       Tim Ingham
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
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Petra.Server.App.Core.Security;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    public partial class TPeriodIntervalConnector
    {
        /// <summary>
        /// Cancels, and prevents database commit, of a currently running PeriodEnd operation.
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static void CancelPeriodOperation()
        {
            TLogging.Log("PeriodEnd operation was cancelled.");
            TPeriodEndOperations.FwasCancelled = true;
        }

        /// <summary>
        /// Routine to run the year end calculations ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AIsInInfoMode">True means: no calculation is done,
        /// only verification result messages are collected</param>
        /// <param name="AVerificationResult"></param>
        /// <returns>false if there's no problem</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool PeriodYearEnd(
            int ALedgerNum,
            bool AIsInInfoMode,
            out TVerificationResultCollection AVerificationResult)
        {
            try
            {
                TLedgerInfo ledgerInfo = new TLedgerInfo(ALedgerNum);
                bool res = new TYearEnd(ledgerInfo).RunYearEnd(AIsInInfoMode, out AVerificationResult);

                if (!res)
                {
                    String SuccessMsg = AIsInInfoMode ? "YearEnd check: No problems found." : "Success.";
                    AVerificationResult.Add(new TVerificationResult("Year End", SuccessMsg, "Success", TResultSeverity.Resv_Status));
                }

                return res;
            }
            catch (Exception e)
            {
                TLogging.Log("TPeriodIntervalConnector.TPeriodYearEnd() throws exception " + e.ToString());
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(
                    new TVerificationResult(
                        Catalog.GetString("Year End"),
                        Catalog.GetString("Exception: ") + e.Message,
                        TResultSeverity.Resv_Critical));
                return true;
            }
        }
    }
} // Ict.Petra.Server.MFinance.GL.WebConnectors

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Module for the year end calculations ...
    /// </summary>
    public class TYearEnd : TPeriodEndOperations
    {
        TLedgerInfo FledgerInfo;

        /// <summary>
        /// Go to period 1 of next year.
        /// </summary>
        public override void SetNextPeriod()
        {
            // Set to the first month of the "next year".
            FledgerInfo.ProvisionalYearEndFlag = false;
            FledgerInfo.CurrentPeriod = 1;
            FledgerInfo.CurrentFinancialYear = FledgerInfo.CurrentFinancialYear + 1;
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.LedgerDetails.ToString());

            TAccountPeriodToNewYear accountPeriodOperator = new TAccountPeriodToNewYear(FledgerInfo.LedgerNumber);
            accountPeriodOperator.IsInInfoMode = false;
            accountPeriodOperator.RunOperation();
        }

        /// <summary>
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public TYearEnd(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        private void PurgeProcessedFeeTable()
        {
            if (TPeriodEndOperations.FwasCancelled)
            {
                return;
            }

            TDBTransaction Transaction = null;
            Boolean ShouldCommit = true;

            if (!FInfoMode)
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    ref ShouldCommit,
                    delegate
                    {
                        String Query = "DELETE FROM a_processed_fee WHERE" +
                                       " a_ledger_number_i=" + FledgerInfo.LedgerNumber +
                                       " AND a_period_number_i<=" + FledgerInfo.NumberOfAccountingPeriods;
                        DBAccess.GDBAccessObj.ExecuteNonQuery(Query, Transaction);

                        Query = "UPDATE a_processed_fee SET a_period_number_i = a_period_number_i-" + FledgerInfo.NumberOfAccountingPeriods +
                                " WHERE a_ledger_number_i=" + FledgerInfo.LedgerNumber;
                        DBAccess.GDBAccessObj.ExecuteNonQuery(Query, Transaction);
                    });
            }
        }

        private void PurgeIchStewardshipTable()
        {
            if (TPeriodEndOperations.FwasCancelled)
            {
                return;
            }

            TDBTransaction Transaction = null;
            Boolean ShouldCommit = true;

            if (!FInfoMode)
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    ref ShouldCommit,
                    delegate
                    {
                        String Query = "DELETE FROM a_ich_stewardship WHERE" +
                                       " a_ledger_number_i=" + FledgerInfo.LedgerNumber +
                                       " AND a_period_number_i<=" + FledgerInfo.NumberOfAccountingPeriods;
                        DBAccess.GDBAccessObj.ExecuteNonQuery(Query, Transaction);

                        Query = "UPDATE a_ich_stewardship SET a_period_number_i = a_period_number_i-" + FledgerInfo.NumberOfAccountingPeriods +
                                " WHERE a_ledger_number_i=" + FledgerInfo.LedgerNumber;
                        DBAccess.GDBAccessObj.ExecuteNonQuery(Query, Transaction);
                    });
            }
        }

        /// <summary>
        /// Master routine ...
        /// </summary>
        /// <param name="AInfoMode"></param>
        /// <param name="AVRCollection"></param>
        /// <returns>True if an error occurred</returns>
        public bool RunYearEnd(
            bool AInfoMode,
            out TVerificationResultCollection AVRCollection)
        {
            FInfoMode = AInfoMode;
            AVRCollection = new TVerificationResultCollection();
            FverificationResults = AVRCollection;

            if (!FledgerInfo.ProvisionalYearEndFlag)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Month End is required!"),
                        Catalog.GetString("In this situation you cannot run Year End."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_04.ToString(),
                        TResultSeverity.Resv_Critical);
                AVRCollection.Add(tvt);
                FHasCriticalErrors = true;
                return true;
            }

            TPeriodEndOperations.FwasCancelled = false;
            /*
             * TDBTransaction Transaction = null;
             * Boolean ShouldCommit = false;
            */

            /*
             * DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
             *  TEnforceIsolationLevel.eilMinimum,
             *  ref Transaction,
             *  ref ShouldCommit,
             *  delegate
             *  {
             */
            Int32 OldYearNum = FledgerInfo.CurrentFinancialYear;

            RunPeriodEndSequence(new TArchive(FledgerInfo),
                Catalog.GetString("Archive old financial information"));

            RunPeriodEndSequence(new TReallocation(FledgerInfo),
                Catalog.GetString("Reallocate all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(FledgerInfo, OldYearNum, this),
                Catalog.GetString("Initialize the database for next year"));

            /* As far as we can tell, there's nothing to do for budgets:
             *          RunPeriodEndSequence(new TNewYearBudgets(FledgerInfo),
             *              Catalog.GetString("Initialise budgets for next year"));
             */

            RunPeriodEndSequence(new TResetForwardPeriodBatches(FledgerInfo, OldYearNum),
                Catalog.GetString("Re-base last year's forward-posted batches so they're in the new year."));

            RunPeriodEndSequence(new TResetForwardPeriodICH(FledgerInfo),
                Catalog.GetString("Re-base last year's forward-posted ICH Stewardship to the new year."));

            PurgeProcessedFeeTable();
            PurgeIchStewardshipTable();

            if (TPeriodEndOperations.FwasCancelled)
            {
                FverificationResults.Add(new TVerificationResult(Catalog.GetString("Year End"),
                        Catalog.GetString("Process was cancelled by user."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_12.ToString(),
                        TResultSeverity.Resv_Critical));
                FHasCriticalErrors = true;
            }

            if (!FInfoMode && !FHasCriticalErrors)
            {
                FledgerInfo.ProvisionalYearEndFlag = false;

                // Refresh cached ledger table, so that the client will know the current period
                TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                    TCacheableFinanceTablesEnum.LedgerDetails.ToString());
                TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                    TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());

                TLogging.LogAtLevel(1, "RunYearEnd: No errors; transaction will be committed.");
                /* ShouldCommit = true; */ // Commenting out for now until Transaction is used again
            }

//              }); // New Or Existing AutoTransaction

            return FHasCriticalErrors;
        } // RunYearEnd
    } // TYearEnd

    /// <summary>
    /// If the oldest year of data is now beyond the retention period, archive it to file and delete those records.
    /// This includes:
    ///   Gifts,
    ///   GL Records,
    ///   Exchange rates (these are multi-ledger, so can only be archived if all ledgers allow it.)
    ///
    /// NOTE CURRENTLY NOT IMPLEMENTED!
    /// </summary>
    public class TArchive : AbstractPeriodEndOperation
    {
        TLedgerInfo FledgerInfo;

        /// <summary></summary>
        /// <param name="ALedgerInfo"></param>
        public TArchive(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        /// <summary></summary>
        public override int GetJobSize()
        {
            bool blnHelp = FInfoMode;

            FInfoMode = true;
            Int32 CountJobs = RunOperation();
            FInfoMode = blnHelp;
            return CountJobs;
        }

        /// <summary></summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TArchive(FledgerInfo);
        }

        /// <summary></summary>
        public override Int32 RunOperation()
        {
            return 0;
        }
    } // TArchive

    /// <summary>
    /// Create and post the year end batch...
    /// </summary>
    public class TReallocation : AbstractPeriodEndOperation
    {
        TLedgerInfo FledgerInfo;
        TAccountInfo FaccountInfo;

        /// <summary>
        ///
        /// </summary>
        public TReallocation(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        private List <String>LoadAccountList()
        {
            List <String>accountList = null;
            FaccountInfo = new TAccountInfo(FledgerInfo.LedgerNumber);
            bool blnIncomeFound = false;
            bool blnExpenseFound = false;
            String strIncomeAccount = TAccountTypeEnum.Income.ToString();
            String strExpenseAccount = TAccountTypeEnum.Expense.ToString();

            FaccountInfo.Reset();
            accountList = new List <String>();

            while (FaccountInfo.MoveNext())
            {
                if (FaccountInfo.PostingStatus)
                {
                    if (FaccountInfo.AccountType == strIncomeAccount)
                    {
                        accountList.Add(FaccountInfo.AccountCode);
                        blnIncomeFound = true;
                    }

                    if (FaccountInfo.AccountType == strExpenseAccount)
                    {
                        accountList.Add(FaccountInfo.AccountCode);
                        blnExpenseFound = true;
                    }
                }
            }

            if (!blnIncomeFound)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No Income Account found"),
                        Catalog.GetString("At least one income account is required."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_09.ToString(),
                        TResultSeverity.Resv_Critical);
                FverificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            if (!blnExpenseFound)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No Expense Account found"),
                        Catalog.GetString("At least one expense account is required."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_10.ToString(),
                        TResultSeverity.Resv_Critical);
                FverificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

/*
 * This code is a mystery: In the 'ICH Processing Centre' special case,
 * code similar but not identical to this, would be required:
 *
 *          // Look for the ICH Account setting: If it's not specified in the a_account_property table,
 *          // this value might be in TAppSettingsManager. Otherwise it's 8500.
 *          FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
 *
 *          if (FaccountInfo.IsValid)
 *          {
 *              accountList.Add(FaccountInfo.AccountCode);
 *          }
 *          else
 *          {
 *              TVerificationResult tvt =
 *                  new TVerificationResult(Catalog.GetString("No ICH_ACCT Account defined"),
 *                      Catalog.GetString("An ICH Account must be defined."), "",
 *                      TPeriodEndErrorAndStatusCodes.PEEC_11.ToString(),
 *                      TResultSeverity.Resv_Critical);
 *              FverificationResults.Add(tvt);
 *              FHasCriticalErrors = true;
 *          }
 */
            return accountList;
        }

        private ACostCentreTable LoadCostCentreTable()
        {
            TDBTransaction Transaction = null;
            ACostCentreTable costCentreTbl = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    costCentreTbl = ACostCentreAccess.LoadViaALedger(FledgerInfo.LedgerNumber, Transaction);
                });

            costCentreTbl.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
            costCentreTbl.DefaultView.RowFilter = "a_posting_cost_centre_flag_l=TRUE";
            return costCentreTbl;
        }

        /// <summary>
        /// </summary>
        public override int GetJobSize()
        {
            return -1;
        }

        /// <summary>
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TReallocation(FledgerInfo);
        }

        /// <summary>
        /// TReallocation.RunOperation
        /// (This has been re-written to iterate over Cost Centres, rather than over accounts.)
        ///
        /// I need an Account list: all profit / loss accounts, plus ICH,
        /// and a Cost Centre list: all Cost Centres.
        ///
        /// For Each Cost Centre, I need to load GLMP entries so that I can find balances for each qualifying account.
        ///
        /// If the rollup style is surplus or deficit, I need to add up the amounts in all the GLMP entries
        /// that correspond to a qualifying account, to find out whether there's an overal surplus or deficit.
        /// If the rollup style is always or never, I don't need to do this.
        ///
        /// Then for Each account within the Cost Centre loop, I need to need to make two transactions -
        ///   the destination account is the nominated equity account,
        ///   and the destination Cost Centre is either the standard Cost Centre, or the same as the source.
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 CountJobs = 0;

            List <String>accountList = LoadAccountList();
            ACostCentreTable costCentreTbl = LoadCostCentreTable();

            Int32 ledgerNumber = FledgerInfo.LedgerNumber;
            Int32 year = FledgerInfo.CurrentFinancialYear;
            Int32 period = FledgerInfo.CurrentPeriod;
            String StandardCostCentre = FledgerInfo.GetStandardCostCentre();
            TCommonAccountingTool yearEndBatch = null;

            if (DoExecuteableCode)
            {
                yearEndBatch = new TCommonAccountingTool(FledgerInfo,
                    Catalog.GetString("Financial year end processing"));
                yearEndBatch.AddBaseCurrencyJournal();
                yearEndBatch.JournalDescription = Catalog.GetString("YearEnd revaluations");
                yearEndBatch.SubSystemCode = CommonAccountingSubSystemsEnum.GL;
            }

            string narrativeFromTo = Catalog.GetString("Year end re-allocation from {0}-{1} to {2}-{3}");
            DataTable accountBalanceTable = new DataTable();
            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    foreach (DataRowView rv in costCentreTbl.DefaultView)
                    {
                        if (TPeriodEndOperations.FwasCancelled)
                        {
                            return; // this only returns from the delegate.
                        }

                        ACostCentreRow CCRow = (ACostCentreRow)rv.Row;
                        String Query = "SELECT DISTINCT a_general_ledger_master.a_account_code_c AS Account," +
                                       " a_general_ledger_master_period.a_actual_base_n AS Balance," +
                                       " a_account.a_debit_credit_indicator_l AS Debit" +
                                       " FROM" +
                                       " a_general_ledger_master_period, a_general_ledger_master, a_account" +
                                       " WHERE" +
                                       " a_account.a_account_code_c=a_general_ledger_master.a_account_code_c" +
                                       " AND a_account.a_posting_status_l=TRUE" +
                                       " AND a_general_ledger_master_period.a_glm_sequence_i=a_general_ledger_master.a_glm_sequence_i" +
                                       " AND a_general_ledger_master.a_ledger_number_i = " + ledgerNumber +
                                       " AND a_general_ledger_master.a_year_i = " + year +
                                       " AND a_general_ledger_master.a_cost_centre_code_c = '" + CCRow.CostCentreCode + "'" +
                                       " AND a_general_ledger_master_period.a_period_number_i=" + period +
                                       " AND a_general_ledger_master_period.a_actual_base_n+a_general_ledger_master.a_closing_period_actual_base_n!=0"
                                       +
                                       " ORDER BY a_general_ledger_master.a_account_code_c";

                        accountBalanceTable = DBAccess.GDBAccessObj.SelectDT(Query, "AccountBalance", transaction);

                        String DestCC = StandardCostCentre;                                                     // Roll-up to the standard Cost Centre, unless...
                        CCRollupStyleEnum RollupStyle = CCRollupStyleEnum.Always;

                        if (Enum.TryParse <CCRollupStyleEnum>(CCRow.RollupStyle, out RollupStyle))
                        {
                            if ((RollupStyle == CCRollupStyleEnum.Deficit) || (RollupStyle == CCRollupStyleEnum.Surplus)) // I need to sum all the qualifying accounts
                            {                                                                                         // to find out whether this Cost Centre is in surplus or deficit.
                                Decimal TotalForCostCentre = 0;

                                foreach (DataRow accountBalanceRow in accountBalanceTable.Rows)
                                {
                                    if (accountList.Contains(accountBalanceRow["Account"].ToString()))
                                    {
                                        if (Convert.ToBoolean(accountBalanceRow["Debit"]))
                                        {
                                            TotalForCostCentre -= Convert.ToDecimal(accountBalanceRow["Balance"]);
                                        }
                                        else
                                        {
                                            TotalForCostCentre += Convert.ToDecimal(accountBalanceRow["Balance"]);
                                        }
                                    }
                                }

                                if (((TotalForCostCentre < 0) && (RollupStyle == CCRollupStyleEnum.Surplus))        // If I should rollup surplus, but I didn't get one,
                                    || ((TotalForCostCentre > 0) && (RollupStyle == CCRollupStyleEnum.Deficit)))    // Or I should rollup deficit, but I didn't get one,
                                {
                                    DestCC = CCRow.CostCentreCode;                                              // I'll keep the balance in the same CC.
                                }
                            }
                            else
                            {
                                if (RollupStyle == CCRollupStyleEnum.Never)                                     // Or if I've been told never to roll up
                                {
                                    DestCC = CCRow.CostCentreCode;                                              // I'll keep the balance in the same CC.
                                }
                            }
                        }

//                      TLogging.Log("Accounts for " + CCRow.CostCentreCode + ": " + accountBalanceTable.Rows.Count + ". CountJobs=" + CountJobs);
                        foreach (DataRow accountBalanceRow in accountBalanceTable.Rows)
                        {
                            String accountCode = accountBalanceRow["Account"].ToString();

                            if (!accountList.Contains(accountCode))
                            {
                                continue;
                            }

                            Boolean isDebit = Convert.ToBoolean(accountBalanceRow["Debit"]);
                            Decimal TransactionAmount = Convert.ToDecimal(accountBalanceRow["Balance"]);

                            if (TransactionAmount < 0)
                            {
                                isDebit = !isDebit;
                                TransactionAmount = 0 - TransactionAmount;
                            }

                            CountJobs++;

                            if (DoExecuteableCode)
                            {
                                ATransactionRow transRow = yearEndBatch.AddBaseCurrencyTransaction(
                                    accountCode, CCRow.CostCentreCode,
                                    String.Format(narrativeFromTo, CCRow.CostCentreCode, accountCode, DestCC, CCRow.RetEarningsAccountCode),
                                    "YEAR-END", !isDebit, TransactionAmount);
                                transRow.SystemGenerated = true;

                                transRow = yearEndBatch.AddBaseCurrencyTransaction(
                                    CCRow.RetEarningsAccountCode, DestCC,
                                    String.Format(narrativeFromTo, CCRow.CostCentreCode, accountCode, DestCC, CCRow.RetEarningsAccountCode),
                                    "YEAR-END", isDebit, TransactionAmount);
                                transRow.SystemGenerated = true;
                            }
                        } // foreach Account

                    } // foreach Cost Centre

                }); // Get NewOrExisting AutoReadTransaction

            if (DoExecuteableCode)
            {
                /* Boolean PostedOk = */
                yearEndBatch.CloseSaveAndPost(FverificationResults);

/*
 *              if (PostedOk)
 *              {
 *                  TLogging.Log("YearEnd batch posted OK.");
 *              }
 *              else
 *              {
 *                  TLogging.Log("YearEnd batch had a problem on posting.");
 *              }
 */
            }

            return CountJobs;
        }
    } // TReallocation

    /// <summary>
    /// Change the rows of the AccountingPeriod Table to the new year
    /// </summary>
    public class TAccountPeriodToNewYear : AbstractPeriodEndOperation
    {
        int FLedgerNumber;

        /// <summary>
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TAccountPeriodToNewYear(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
        }

        /// <summary>
        ///
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TAccountPeriodToNewYear(FLedgerNumber);
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public override int GetJobSize()
        {
            bool blnHelp = FInfoMode;

            FInfoMode = true;
            Int32 CountJobs = RunOperation();
            FInfoMode = blnHelp;
            return CountJobs;
        }

        /// <summary>
        /// The AccountingPeriod Rows are updated ...
        /// </summary>
        override public Int32 RunOperation()
        {
            Int32 JobSize = 0;

            AAccountingPeriodTable AccountingPeriodTbl = null;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        AccountingPeriodTbl = AAccountingPeriodAccess.LoadViaALedger(FLedgerNumber, Transaction);

                        foreach (AAccountingPeriodRow accountingPeriodRow in AccountingPeriodTbl.Rows)
                        {
                            accountingPeriodRow.PeriodStartDate =
                                accountingPeriodRow.PeriodStartDate.AddDays(1).AddYears(1).AddDays(-1);
                            accountingPeriodRow.PeriodEndDate =
                                accountingPeriodRow.PeriodEndDate.AddDays(1).AddYears(1).AddDays(-1);
                            JobSize++;
                        }

                        if (DoExecuteableCode)
                        {
                            AAccountingPeriodAccess.SubmitChanges(AccountingPeriodTbl, Transaction);
                            SubmissionOK = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.Log("Exception during running the AccountPeriod To New Year operation:");
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            return JobSize;
        }  // RunOperation
    } // TAccountPeriodToNewYear

    /// <summary>
    /// Read all glm year end records of the actual year
    /// and create the start record for the next year.
    /// Move the Ledger Row to the next year, and set the AccountingPeriod to 1.
    /// </summary>
    public class TGlmNewYearInit : AbstractPeriodEndOperation
    {
        int FOldYearNum;
        int FNewYearNum;
        TLedgerInfo FLedgerInfo;
        TYearEnd FYearEndOperator;
        AGeneralLedgerMasterTable FGlmPostingFrom = new AGeneralLedgerMasterTable();
        AGeneralLedgerMasterPeriodTable FGlmpFrom = new AGeneralLedgerMasterPeriodTable();
        GLPostingTDS GlmTDS = new GLPostingTDS();
        Int32 FLedgerAccountingPeriods;
        Int32 FLedgerFwdPeriods;


        /// <summary>
        /// </summary>
        public TGlmNewYearInit(TLedgerInfo ALedgerInfo, int AYear, TYearEnd AYearEndOperator)
        {
            FOldYearNum = AYear;
            FNewYearNum = FOldYearNum + 1;
            FLedgerInfo = ALedgerInfo;
            FYearEndOperator = AYearEndOperator;
            FLedgerAccountingPeriods = FLedgerInfo.NumberOfAccountingPeriods; // Don't call these properties in a loop,
            FLedgerFwdPeriods = FLedgerInfo.NumberFwdPostingPeriods;          // as they reload the row from the DB!

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    DataTable GlmTble = LoadTable(FLedgerInfo.LedgerNumber, FOldYearNum, Transaction);
                    FGlmPostingFrom.Merge(GlmTble);
                    GlmTble = LoadTable(FLedgerInfo.LedgerNumber, FNewYearNum, Transaction);
                    GlmTDS.AGeneralLedgerMaster.Merge(GlmTble);
                    GlmTDS.AGeneralLedgerMaster.DefaultView.Sort =
                        AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," +
                        AGeneralLedgerMasterTable.GetCostCentreCodeDBName();

                    DataTable GlmpTbl = GetGlmpRows(FLedgerInfo.LedgerNumber, FOldYearNum, Transaction, 0);
                    FGlmpFrom.Merge(GlmpTbl);
                    FGlmpFrom.DefaultView.Sort = "a_period_number_i";

                    GlmpTbl = GetGlmpRows(FLedgerInfo.LedgerNumber, FNewYearNum, Transaction, 0);
                    GlmTDS.AGeneralLedgerMasterPeriod.Merge(GlmpTbl);
                });
        }

        /// <summary>
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TGlmNewYearInit(FLedgerInfo, FOldYearNum, FYearEndOperator);
        }

        /// <summary>
        /// </summary>
        public override int GetJobSize()
        {
            return -1; // I can't know the Job size
        }

        // get posting glm rows
        private DataTable LoadTable(int ALedgerNumber, int AYear, TDBTransaction ATransaction)
        {
            AGeneralLedgerMasterTable typedTable = new AGeneralLedgerMasterTable();
            string strSQL = "SELECT DISTINCT PUB_a_general_ledger_master.*" +
                            " FROM PUB_a_general_ledger_master, PUB_a_account, PUB_a_cost_centre" +
                            " WHERE PUB_a_general_ledger_master.a_ledger_number_i = " + ALedgerNumber +
                            " AND PUB_a_general_ledger_master.a_year_i = " + AYear +

                            " AND PUB_a_account.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i" +
                            " AND PUB_a_account.a_account_code_c = PUB_a_general_ledger_master.a_account_code_c" +
                            " AND PUB_a_account.a_posting_status_l = true" +
                            " AND PUB_a_cost_centre.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i" +
                            " AND PUB_a_cost_centre.a_cost_centre_code_c = PUB_a_general_ledger_master.a_cost_centre_code_c" +
                            " AND PUB_a_cost_centre.a_posting_cost_centre_flag_l = true" +

                            " ORDER BY a_account_code_c,a_cost_centre_code_c";


            DBAccess.GDBAccessObj.SelectDT(typedTable, strSQL, ATransaction);

            return typedTable;
        }

        // get posting glmp rows
        private DataTable GetGlmpRows(int ALedgerNumber, Int32 AYear, TDBTransaction ATransaction, Int32 APeriodGreaterThan)
        {
            AGeneralLedgerMasterPeriodTable typedTable = new AGeneralLedgerMasterPeriodTable();
            string strSQL = "SELECT PUB_a_general_ledger_master_period.* " +
                            " FROM PUB_a_general_ledger_master, PUB_a_general_ledger_master_period, PUB_a_account, PUB_a_cost_centre" +

                            " WHERE PUB_a_general_ledger_master.a_ledger_number_i = " + ALedgerNumber +
                            " AND PUB_a_general_ledger_master.a_year_i = " + AYear +
                            " AND PUB_a_general_ledger_master.a_glm_sequence_i = PUB_a_general_ledger_master_period.a_glm_sequence_i" +
                            " AND PUB_a_general_ledger_master_period.a_period_number_i > " + APeriodGreaterThan +

                            " AND PUB_a_account.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i" +
                            " AND PUB_a_account.a_account_code_c = PUB_a_general_ledger_master.a_account_code_c" +
                            " AND PUB_a_account.a_posting_status_l = true" +
                            " AND PUB_a_cost_centre.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i" +
                            " AND PUB_a_cost_centre.a_cost_centre_code_c = PUB_a_general_ledger_master.a_cost_centre_code_c" +
                            " AND PUB_a_cost_centre.a_posting_cost_centre_flag_l = true" +

                            " ORDER BY PUB_a_general_ledger_master_period.a_period_number_i;";

            DBAccess.GDBAccessObj.SelectDT(typedTable, strSQL, ATransaction);

            return typedTable;
        }

        /// <summary>
        /// Next-Year records will be created, and the Ledger will be moved to the new year.
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 TempGLMSequence = -1;
            Int32 EntryCount = 0;

            if (!FInfoMode)
            {
                FYearEndOperator.SetNextPeriod();
            }

            foreach (AGeneralLedgerMasterRow GlmRowFrom in FGlmPostingFrom.Rows)
            {
                if (TPeriodEndOperations.FwasCancelled)
                {
                    return EntryCount;
                }

                ++EntryCount;

                if (FInfoMode)
                {
                    continue;
                }

                FGlmpFrom.DefaultView.RowFilter = "a_glm_sequence_i=" + GlmRowFrom.GlmSequence;
                FGlmpFrom.DefaultView.Sort = "a_period_number_i";

                AGeneralLedgerMasterRow GlmRowTo = null;
                //
                // If there's not already a GLM row for this Account / Cost Centre,
                // I need to create one now.
                //
                Int32 GlmToRowIdx = GlmTDS.AGeneralLedgerMaster.DefaultView.Find(
                    new Object[] { GlmRowFrom.AccountCode, GlmRowFrom.CostCentreCode }
                    );

                if (GlmToRowIdx >= 0)
                {
                    GlmRowTo = (AGeneralLedgerMasterRow)GlmTDS.AGeneralLedgerMaster.DefaultView[GlmToRowIdx].Row;
                }
                else        // GLM record Not present - I'll make one now...
                {
                    GlmRowTo = GlmTDS.AGeneralLedgerMaster.NewRowTyped(true);
                    GlmRowTo.GlmSequence = TempGLMSequence;
                    TempGLMSequence--;
                    GlmRowTo.LedgerNumber = FLedgerInfo.LedgerNumber;
                    GlmRowTo.AccountCode = GlmRowFrom.AccountCode;
                    GlmRowTo.CostCentreCode = GlmRowFrom.CostCentreCode;
                    GlmRowTo.Year = FNewYearNum;

                    GlmTDS.AGeneralLedgerMaster.Rows.Add(GlmRowTo);
                }

                GlmRowTo.YtdActualBase = GlmRowFrom.YtdActualBase;
                GlmRowTo.YtdActualIntl = GlmRowFrom.YtdActualIntl;

                if (!GlmRowFrom.IsYtdActualForeignNull())
                {
                    GlmRowTo.YtdActualForeign = GlmRowFrom.YtdActualForeign;
                }

                // If the GlmRowTo row already existed, its GLMP records will also exist,
                // but I need to update them with data from last year's forward periods.
                // Otherwise I need to create a clutch of matching GLMP records.

                // Create GLM period records for new year if required
                for (int PeriodCount = 1;
                     PeriodCount <= FLedgerAccountingPeriods + FLedgerFwdPeriods;
                     PeriodCount++)
                {
                    AGeneralLedgerMasterPeriodRow glmPeriodRow;

                    // If there's already a GLM and GLMP entry, I need to use the existing GLMP row for this period
                    if ((GlmToRowIdx >= 0)
                        && GlmTDS.AGeneralLedgerMasterPeriod.Rows.Contains(new object[] { GlmRowTo.GlmSequence, PeriodCount }))
                    {
                        glmPeriodRow =
                            (AGeneralLedgerMasterPeriodRow)GlmTDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { GlmRowTo.GlmSequence,
                                                                                                                      PeriodCount });
                    }
                    else // otherwise I need to create one now.
                    {
                        glmPeriodRow = GlmTDS.AGeneralLedgerMasterPeriod.NewRowTyped(true);
                        glmPeriodRow.GlmSequence = GlmRowTo.GlmSequence;
                        glmPeriodRow.PeriodNumber = PeriodCount;
                        GlmTDS.AGeneralLedgerMasterPeriod.Rows.Add(glmPeriodRow);
                    }
                }

                // carry forward period balances (if any) over to the start of the new year
                AGeneralLedgerMasterPeriodRow OldGlmForwardPeriodRow =
                    (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[FLedgerAccountingPeriods - 1].Row;
                decimal CarryForwardBase = OldGlmForwardPeriodRow.ActualBase;
                decimal CarryForwardIntl = OldGlmForwardPeriodRow.ActualIntl;
                decimal CarryForwardForeign = (OldGlmForwardPeriodRow.IsActualForeignNull()) ? 0 : OldGlmForwardPeriodRow.ActualForeign;

                Int32 LastYearPeriods = FGlmpFrom.DefaultView.Count;

                for (int PeriodCount = FLedgerAccountingPeriods + 1;
                     PeriodCount <= LastYearPeriods;
                     PeriodCount++)
                {
                    Int32 RowIdx = FGlmpFrom.DefaultView.Find(PeriodCount);

                    if (RowIdx >= 0)
                    {
                        OldGlmForwardPeriodRow = (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[RowIdx].Row;
                        CarryForwardBase = OldGlmForwardPeriodRow.ActualBase;
                        CarryForwardIntl = OldGlmForwardPeriodRow.ActualIntl;
                        CarryForwardForeign = (OldGlmForwardPeriodRow.IsActualForeignNull()) ? 0 : OldGlmForwardPeriodRow.ActualForeign;
                        OldGlmForwardPeriodRow.Delete();
                    }

                    AGeneralLedgerMasterPeriodRow NewGlmPeriodRow =
                        (AGeneralLedgerMasterPeriodRow)GlmTDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { GlmRowTo.GlmSequence,
                                                                                                                  PeriodCount -
                                                                                                                  FLedgerAccountingPeriods });


                    NewGlmPeriodRow.ActualBase = CarryForwardBase;
                    NewGlmPeriodRow.ActualIntl = CarryForwardIntl;
                    NewGlmPeriodRow.ActualForeign = CarryForwardForeign;
                }

                // Populate the remaining new periods not populated by last year's forward periods
                for (int PeriodCount = LastYearPeriods - FLedgerAccountingPeriods + 1;
                     PeriodCount <= FLedgerAccountingPeriods + FLedgerFwdPeriods;
                     PeriodCount++)
                {
                    AGeneralLedgerMasterPeriodRow NewGlmPeriodRow =
                        (AGeneralLedgerMasterPeriodRow)GlmTDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { GlmRowTo.GlmSequence, PeriodCount });
                    NewGlmPeriodRow.ActualBase = CarryForwardBase;
                    NewGlmPeriodRow.ActualIntl = CarryForwardIntl;
                    NewGlmPeriodRow.ActualForeign = CarryForwardForeign;
                }

                // get the number of (not forward) periods that actually exist (this should be FLedgerAccountingPeriods but just incase...)
                int ActualAccountingPeriods = Math.Min(FLedgerAccountingPeriods, LastYearPeriods);

                AGeneralLedgerMasterPeriodRow LastOldGlmPeriodRow =
                    (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[ActualAccountingPeriods - 1].Row;

                //Set starting balances
                GlmRowTo.StartBalanceBase = GlmRowFrom.ClosingPeriodActualBase + LastOldGlmPeriodRow.ActualBase;
                GlmRowTo.StartBalanceIntl = GlmRowFrom.ClosingPeriodActualIntl + LastOldGlmPeriodRow.ActualIntl;
                GlmRowTo.StartBalanceForeign = (LastOldGlmPeriodRow.IsActualForeignNull()) ? 0 : LastOldGlmPeriodRow.ActualForeign;
            } // foreach

            if (DoExecuteableCode)
            {
                GlmTDS.ThrowAwayAfterSubmitChanges = true;
                TLogging.LogAtLevel(1,
                    "TGlmNewYearInit: New GLMP (" + FLedgerInfo.LedgerNumber + ") for year " + FNewYearNum + " has " +
                    GlmTDS.AGeneralLedgerMasterPeriod.Rows.Count + " Rows.");
                GLPostingTDSAccess.SubmitChanges(GlmTDS);

                FGlmpFrom.ThrowAwayAfterSubmitChanges = true;

                TDBTransaction Transaction = null;
                Boolean SubmissionOk = true;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    ref SubmissionOk,
                    delegate
                    {
                        AGeneralLedgerMasterPeriodAccess.SubmitChanges(FGlmpFrom, Transaction);
                    });
            }

            return EntryCount;
        } // RunOperation
    } // TGlmNewYearInit

    /*
     * As far as we can tell, there's nothing to do with the budgets:
     *
     *  /// <summary>
     *  ///
     *  /// </summary>
     *  public class TNewYearBudgets : AbstractPeriodEndOperation
     *  {
     *      private TLedgerInfo FLedgerInfo;
     *
     *      public TNewYearBudgets(TLedgerInfo ALedgerInfo)
     *      {
     *          FLedgerInfo = ALedgerInfo;
     *      }
     *
     *      /// <summary>
     *      ///
     *      /// </summary>
     *      public override AbstractPeriodEndOperation GetActualizedClone()
     *      {
     *          return new TNewYearBudgets(FLedgerInfo);
     *      }
     *
     *      /// <summary>
     *      ///
     *      /// </summary>
     *      public override int GetJobSize()
     *      {
     *          bool blnHelp = FInfoMode;
     *          FInfoMode = true;
     *          Int32 CountJobs = RunOperation();
     *          FInfoMode = blnHelp;
     *          return CountJobs;
     *      }
     *
     *      /// <summary>
     *      /// In a_budget_period move this year’s values to last year, next year’s to this year, and set next year to zero.
     *      /// </summary>
     *      public override Int32 RunOperation()
     *      {
     *          return 0;
     *      }
     *
     *  } // TNewYearBudgets
     */

    /// <summary>
    /// Reset period columns on batch, journal and gift batch tables for periods beyond end of last year
    /// </summary>
    public class TResetForwardPeriodBatches : AbstractPeriodEndOperation
    {
        private TLedgerInfo FLedgerInfo;
        Int32 FOldYearNum;

        /// <summary>
        /// </summary>
        public TResetForwardPeriodBatches(TLedgerInfo ALedgerInfo, Int32 AOldYearNum)
        {
            FLedgerInfo = ALedgerInfo;
            FOldYearNum = AOldYearNum;
        }

        /// <summary>
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TResetForwardPeriodBatches(FLedgerInfo, FOldYearNum);
        }

        /// <summary>
        ///
        /// </summary>
        public override int GetJobSize()
        {
            bool blnHelp = FInfoMode;

            FInfoMode = true;
            Int32 CountJobs = RunOperation();
            FInfoMode = blnHelp;
            return CountJobs;
        }

        /// <summary>
        /// ResetForwardPeriodBatches.RunOperation
        ///
        /// Reset period columns on batch, journal and gift batch tables for periods beyond end of the old year
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 JobSize = 0;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        String Query =
                            "SELECT * FROM PUB_a_batch WHERE " +
                            "a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                            " AND a_batch_year_i=" + FOldYearNum +
                            " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                        ABatchTable BatchTbl = new ABatchTable();
                        DBAccess.GDBAccessObj.SelectDT(BatchTbl, Query, Transaction);

                        if (BatchTbl.Rows.Count > 0)
                        {
                            JobSize = BatchTbl.Rows.Count;

                            if (!FInfoMode)
                            {
                                foreach (ABatchRow BatchRow in BatchTbl.Rows)
                                {
                                    BatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                                    BatchRow.BatchYear += 1;
                                }

                                ABatchAccess.SubmitChanges(BatchTbl, Transaction);
                                SubmissionOK = true;
                            }
                        }

                        Query =
                            "SELECT PUB_a_journal.* FROM PUB_a_batch, PUB_a_journal WHERE " +
                            " PUB_a_journal.a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                            " AND PUB_a_batch.a_batch_number_i= PUB_a_journal.a_batch_number_i" +
                            " AND PUB_a_batch.a_batch_year_i=" + FOldYearNum +
                            " AND a_journal_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                        AJournalTable JournalTbl = new AJournalTable();
                        DBAccess.GDBAccessObj.SelectDT(JournalTbl, Query, Transaction);

                        if (JournalTbl.Rows.Count > 0)
                        {
                            if (!FInfoMode)
                            {
                                foreach (AJournalRow JournalRow in JournalTbl.Rows)
                                {
                                    JournalRow.JournalPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                                }

                                AJournalAccess.SubmitChanges(JournalTbl, Transaction);
                                SubmissionOK = true;
                            }
                        }

                        Query =
                            "SELECT * FROM PUB_a_gift_batch WHERE " +
                            " a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                            " AND a_batch_year_i=" + FOldYearNum +
                            " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                        AGiftBatchTable GiftBatchTbl = new AGiftBatchTable();
                        DBAccess.GDBAccessObj.SelectDT(GiftBatchTbl, Query, Transaction);

                        if (GiftBatchTbl.Rows.Count > 0)
                        {
                            JobSize += GiftBatchTbl.Rows.Count;

                            if (!FInfoMode)
                            {
                                foreach (AGiftBatchRow GiftBatchRow in GiftBatchTbl.Rows)
                                {
                                    GiftBatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                                    GiftBatchRow.BatchYear += 1;
                                }

                                AGiftBatchAccess.SubmitChanges(GiftBatchTbl, Transaction);
                                SubmissionOK = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            return JobSize;
        }
    }     // TResetForwardPeriodBatches

    /// <summary>
    /// Delete old year and update periods for those in new year (eg. 13 becomes 1, 14 becomes 2, etc)
    /// </summary>
    public class TResetForwardPeriodICH : AbstractPeriodEndOperation
    {
        private TLedgerInfo FLedgerInfo;

        /// <summary>
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public TResetForwardPeriodICH(TLedgerInfo ALedgerInfo)
        {
            FLedgerInfo = ALedgerInfo;
        }

        /// <summary>
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TResetForwardPeriodICH(FLedgerInfo);
        }

        /// <summary>
        ///
        /// </summary>
        public override int GetJobSize()
        {
            bool blnHelp = FInfoMode;

            FInfoMode = true;
            Int32 CountJobs = RunOperation();
            FInfoMode = blnHelp;
            return CountJobs;
        }

        /// <summary>
        /// TResetForwardPeriodICH.RunOperation
        /// Delete old year and update periods for those in new year (eg. 13 becomes 1, 14 becomes 2, etc)
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 JobSize = 0;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        String Query =
                            "SELECT * FROM PUB_a_ich_stewardship WHERE " +
                            "a_ledger_number_i=" + FLedgerInfo.LedgerNumber;
                        DataTable Tbl = DBAccess.GDBAccessObj.SelectDT(Query, "AIchStewardship", Transaction);

                        if (Tbl.Rows.Count > 0)
                        {
                            AIchStewardshipTable StewardshipTbl = new AIchStewardshipTable();
                            StewardshipTbl.Merge(Tbl);

                            for (Int32 Idx = StewardshipTbl.Rows.Count - 1; Idx >= 0; Idx--)
                            {
                                AIchStewardshipRow StewardshipRow = StewardshipTbl[Idx];

                                if (StewardshipRow.PeriodNumber > FLedgerInfo.NumberOfAccountingPeriods)
                                {
                                    StewardshipRow.PeriodNumber -= FLedgerInfo.NumberOfAccountingPeriods;
                                    JobSize++;
                                }
                                else
                                {
                                    StewardshipRow.Delete();
                                }
                            }

                            if (!FInfoMode)
                            {
                                StewardshipTbl.ThrowAwayAfterSubmitChanges = true;
                                AIchStewardshipAccess.SubmitChanges(StewardshipTbl, Transaction);
                                SubmissionOK = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.LogException(ex, Utilities.GetMethodSignature());
                        throw;
                    }
                });

            return JobSize;
        }
    }     // TResetForwardPeriodICH
} // Ict.Petra.Server.MFinance.GL