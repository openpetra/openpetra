//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
        /// Routine to run the year end calculations ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AIsInInfoMode">True means: no calculation is done,
        /// only verification result messages are collected</param>
        /// <param name="AVerificationResult"></param>
        /// <returns>false if there's no problem</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodYearEnd(
            int ALedgerNum,
            bool AIsInInfoMode,
            out TVerificationResultCollection AVerificationResult)
        {
            try
            {
                TLedgerInfo LedgerInfo = new TLedgerInfo(ALedgerNum);
                bool res = new TYearEnd(LedgerInfo).RunYearEnd(AIsInInfoMode, out AVerificationResult);

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
                return false;
            }
        }
    }
} // Ict.Petra.Server.MFinance.GL.WebConnectors

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// This is the list of status values of a_ledger.a_year_end_process_status_i which has been
    /// copied from Petra. The status begins by counting from RESET_Status up to LEDGER_UPDATED
    /// and each higher level status includes the lower level ones.
    /// (May be obsolete - wait until Year end is done)
    /// </summary>
    public enum TYearEndProcessStatus
    {
        /// <summary>Status initial value</summary>
        RESET_STATUS = 0,
        /// <summary></summary>
        GIFT_CLOSED_OUT = 1,
        /// <summary></summary>
        ACCOUNT_CLOSED_OUT = 2,
        /// <summary></summary>
        GLMASTER_CLOSED_OUT = 3,
        /// <summary></summary>
        BUDGET_CLOSED_OUT = 4,
        /// <summary></summary>
        PERIODS_UPDATED = 7,
        /// <summary></summary>
        SET_NEW_YEAR = 8,
        /// <summary>The leger is completely updated.</summary>
        LEDGER_UPDATED = 10
    }

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

        /// <summary>
        /// Master routine ...
        /// </summary>
        /// <param name="AInfoMode"></param>
        /// <param name="AVRCollection"></param>
        /// <returns>True if an error occurred</returns>
        public bool RunYearEnd(bool AInfoMode,
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

            Int32 FOldYearNum = FledgerInfo.CurrentFinancialYear;

            RunPeriodEndSequence(new TArchive(FledgerInfo),
                Catalog.GetString("Archive old financial information"));

            RunPeriodEndSequence(new TReallocation(FledgerInfo),
                Catalog.GetString("Reallocate all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(FledgerInfo, FOldYearNum, this),
                Catalog.GetString("Initialize the database for next year"));

/* As far as we can tell, there's nothing to do for budgets:
 *          RunPeriodEndSequence(new TNewYearBudgets(FledgerInfo),
 *              Catalog.GetString("Initialise budgets for next year"));
 */

            RunPeriodEndSequence(new TResetForwardPeriodBatches(FledgerInfo, FOldYearNum),
                Catalog.GetString("Re-base last year's forward-posted batches so they're in the new year."));

            RunPeriodEndSequence(new TResetForwardPeriodICH(FledgerInfo),
                Catalog.GetString("Re-base last year's forward-posted ICH Stewardship to the new year."));

            if (!FInfoMode && !FHasCriticalErrors)
            {
                FledgerInfo.ProvisionalYearEndFlag = false;
            }

            return FHasCriticalErrors;
        }
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
        ACostCentreTable FCostCentreTbl;
        string FstrYearEnd = Catalog.GetString("YEAR-END");
        string FstrNarrativeToMessage = Catalog.GetString("Year end re-allocation to {2}-{3}");
        string FstrNarrativeFromToMessage = Catalog.GetString("Year end re-allocation from {0}-{1} to {2}-{3}");


        TGlmInfo FglmInfo;

        List <String>FAccountList = null;


        /// <summary>
        ///
        /// </summary>
        public TReallocation(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        private void CalculateAccountInfo()
        {
            FaccountInfo = new TAccountInfo(FledgerInfo);
            bool blnIncomeFound = false;
            bool blnExpenseFound = false;
            String strIncomeAccount = TAccountTypeEnum.Income.ToString();
            String strExpenseAccount = TAccountTypeEnum.Expense.ToString();

            FaccountInfo.Reset();
            FAccountList = new List <String>();

            while (FaccountInfo.MoveNext())
            {
                if (FaccountInfo.PostingStatus)
                {
                    if (FaccountInfo.AccountType == strIncomeAccount)
                    {
                        FAccountList.Add(FaccountInfo.AccountCode);
                        blnIncomeFound = true;
                    }

                    if (FaccountInfo.AccountType == strExpenseAccount)
                    {
                        FAccountList.Add(FaccountInfo.AccountCode);
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

            FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);

            if (FaccountInfo.IsValid)
            {
                FAccountList.Add(FaccountInfo.AccountCode);
            }
            else
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No ICH_ACCT Account defined"),
                        Catalog.GetString("An ICH Account must be defined."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_11.ToString(),
                        TResultSeverity.Resv_Critical);
                FverificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    FCostCentreTbl = ACostCentreAccess.LoadViaALedger(FledgerInfo.LedgerNumber, Transaction);
                });

            FCostCentreTbl.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
        }

        /// <summary>
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
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TReallocation(FledgerInfo);
        }

        /// <summary>
        /// TReallocation.RunOperation
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 CountJobs = 0;

            if (FAccountList == null)
            {
                CalculateAccountInfo();
            }

            TCommonAccountingTool YearEndBatch = null;

            if (DoExecuteableCode)
            {
                YearEndBatch = new TCommonAccountingTool(FledgerInfo,
                    Catalog.GetString("Financial year end processing"));
                YearEndBatch.AddBaseCurrencyJournal();
                YearEndBatch.JournalDescription = Catalog.GetString("YearEnd revaluations");
                YearEndBatch.SubSystemCode = CommonAccountingSubSystemsEnum.GL;
            }

            // tCommonAccountingTool.DateEffective =""; Default is "End of actual period ..."
            // Loop with all account codes
            foreach (string strAccountCode in FAccountList)
            {
                FglmInfo = new TGlmInfo(FledgerInfo.LedgerNumber,
                    FledgerInfo.CurrentFinancialYear,
                    strAccountCode);

                while (FglmInfo.MoveNext())
                {
                    Int32 CostCentreRowIdx = FCostCentreTbl.DefaultView.Find(FglmInfo.CostCentreCode);
                    ACostCentreRow currentCostCentre = (ACostCentreRow)FCostCentreTbl.DefaultView[CostCentreRowIdx].Row;

                    if (currentCostCentre.PostingCostCentreFlag)
                    {
                        TGlmpInfo glmpInfo = new TGlmpInfo(-1, -1, FglmInfo.GlmSequence, FledgerInfo.NumberOfAccountingPeriods);

                        if (glmpInfo.ActualBase + FglmInfo.ClosingPeriodActualBase != 0)
                        {
                            if (DoExecuteableCode)
                            {
                                ReallocationLoop(glmpInfo, YearEndBatch, strAccountCode, FglmInfo.CostCentreCode);
                            }

                            CountJobs++;
                        }
                    }
                }
            }

            if (DoExecuteableCode)
            {
                YearEndBatch.CloseSaveAndPost(FverificationResults);
            }

            return CountJobs;
        }

        private void ReallocationLoop(TGlmpInfo AGlmpInfo, TCommonAccountingTool YearEndBatch, String AAccountFrom, String ACostCentreFrom)
        {
            string strCostCentreTo = TLedgerInfo.GetStandardCostCentre(FledgerInfo.LedgerNumber);
            string strAccountTo;

            FaccountInfo.AccountCode = AAccountFrom;
            Boolean blnDebitCredit = FaccountInfo.DebitCreditIndicator;
            String narrativeMessage = FstrNarrativeToMessage;

            string strCCAccoutCode = FaccountInfo.SetCarryForwardAccount(); // Move FaccountInfo to the Carry Forward Account - if there is one.

            if (FaccountInfo.IsValid) // The CarryForward account exists..
            {
                strAccountTo = FaccountInfo.AccountCode;

                if (strCCAccoutCode == "SAMECC")
                {
                    strCostCentreTo = ACostCentreFrom;
                    narrativeMessage = FstrNarrativeFromToMessage;
                }
            }
            else // If there's no Carry Forward account, use EARNINGS_BF_ACCT
            {
                FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.EARNINGS_BF_ACCT);
                strAccountTo = FaccountInfo.AccountCode;
            }

            if (FledgerInfo.IltAccountFlag) // In either case, change that if ICH_ACCT is set
            {
                FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
                strAccountTo = FaccountInfo.AccountCode;
            }

            if (FledgerInfo.BranchProcessing) // Keep the original Cost Centres - don't roll up
            {
                strCostCentreTo = ACostCentreFrom;
                narrativeMessage = FstrNarrativeFromToMessage;
            }

            if (FglmInfo.YtdActualBase < 0)
            {
                blnDebitCredit = !blnDebitCredit;
            }

            Decimal TransactionAmount = Math.Abs(AGlmpInfo.ActualBase);

            YearEndBatch.AddBaseCurrencyTransaction(
                AAccountFrom, ACostCentreFrom,
                String.Format(narrativeMessage, ACostCentreFrom, AAccountFrom, strCostCentreTo, strAccountTo),
                FstrYearEnd, !blnDebitCredit, TransactionAmount);

            YearEndBatch.AddBaseCurrencyTransaction(
                strAccountTo, strCostCentreTo,
                String.Format(narrativeMessage, ACostCentreFrom, AAccountFrom, strCostCentreTo, strAccountTo),
                FstrYearEnd, blnDebitCredit, TransactionAmount);
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
                    catch (Exception Exc)
                    {
                        TLogging.Log("Exception during running the AccountPeriod To New Year operation:" + Environment.NewLine + Exc.ToString());
                        throw Exc;
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
            return 0; // I can't know the Job size?

/*
 *          bool blnOldInfoMode = FInfoMode;
 *          FInfoMode = true;
 *          int EntryCount = RunOperation();
 *          FInfoMode = blnOldInfoMode;
 *          return EntryCount;
 */
        }

        // get posting glm rows
        private DataTable LoadTable(int ALedgerNumber, int AYear, TDBTransaction ATransaction)
        {
            AGeneralLedgerMasterTable typedTable = new AGeneralLedgerMasterTable();
            string strSQL = "SELECT PUB_a_general_ledger_master.*" +
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

            try
            {
                foreach (AGeneralLedgerMasterRow GlmRowFrom in FGlmPostingFrom.Rows)
                {
                    ++EntryCount;

                    if (FInfoMode)
                    {
                        continue;
                    }

                    FGlmpFrom.DefaultView.RowFilter = "a_glm_sequence_i=" + GlmRowFrom.GlmSequence;
                    FGlmpFrom.DefaultView.Sort = "a_period_number_i";

                    // get the number of forward periods that actually exist (this should be FLedgerFwdPeriods but just incase...)
                    int ForwardPeriods = Math.Max(FGlmpFrom.DefaultView.Count - FLedgerAccountingPeriods, 0);

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
                    for (int PeriodCount = 1;
                         PeriodCount <= ForwardPeriods;
                         PeriodCount++)
                    {
                        AGeneralLedgerMasterPeriodRow OldGlmForwardPeriodRow =
                            (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[FLedgerAccountingPeriods + PeriodCount - 1].Row;
                        AGeneralLedgerMasterPeriodRow NewGlmPeriodRow =
                            (AGeneralLedgerMasterPeriodRow)GlmTDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { GlmRowTo.GlmSequence,
                                                                                                                      PeriodCount });

                        NewGlmPeriodRow.ActualBase = OldGlmForwardPeriodRow.ActualBase;
                        NewGlmPeriodRow.ActualIntl = OldGlmForwardPeriodRow.ActualIntl;

                        if (!OldGlmForwardPeriodRow.IsActualForeignNull())
                        {
                            NewGlmPeriodRow.ActualForeign = OldGlmForwardPeriodRow.ActualForeign;
                        }
                    }

                    AGeneralLedgerMasterPeriodRow LastOldGlmForwardPeriodRow =
                        (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[FGlmpFrom.DefaultView.Count - 1].Row;

                    // populate the new periods not populated by last year's forward periods
                    for (int PeriodCount = ForwardPeriods + 1;
                         PeriodCount <= FLedgerAccountingPeriods + ForwardPeriods;
                         PeriodCount++)
                    {
                        AGeneralLedgerMasterPeriodRow NewGlmPeriodRow =
                            (AGeneralLedgerMasterPeriodRow)GlmTDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { GlmRowTo.GlmSequence,
                                                                                                                      PeriodCount });

                        NewGlmPeriodRow.ActualBase = LastOldGlmForwardPeriodRow.ActualBase;
                        NewGlmPeriodRow.ActualIntl = LastOldGlmForwardPeriodRow.ActualIntl;

                        if (!LastOldGlmForwardPeriodRow.IsActualForeignNull())
                        {
                            NewGlmPeriodRow.ActualForeign = LastOldGlmForwardPeriodRow.ActualForeign;
                        }
                    }

                    // get the number of (not forward) periods that actually exist (this should be FLedgerAccountingPeriods but just incase...)
                    int ActualAccountingPeriods = Math.Min(FLedgerAccountingPeriods, FGlmpFrom.DefaultView.Count);

                    AGeneralLedgerMasterPeriodRow LastOldGlmPeriodRow =
                        (AGeneralLedgerMasterPeriodRow)FGlmpFrom.DefaultView[ActualAccountingPeriods - 1].Row;

                    //Set starting balances
                    GlmRowTo.StartBalanceBase = GlmRowFrom.ClosingPeriodActualBase + LastOldGlmPeriodRow.ActualBase;
                    GlmRowTo.StartBalanceIntl = GlmRowFrom.ClosingPeriodActualIntl + LastOldGlmPeriodRow.ActualIntl;

                    if (!LastOldGlmPeriodRow.IsActualForeignNull())
                    {
                        GlmRowTo.StartBalanceForeign = LastOldGlmPeriodRow.ActualForeign;
                    }
                }
            }
            finally
            {
                // Nothing special to do
            }

            if (DoExecuteableCode)
            {
                GlmTDS.ThrowAwayAfterSubmitChanges = true;
                GLPostingTDSAccess.SubmitChanges(GlmTDS);
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
                    catch (Exception e)
                    {
                        TLogging.Log("Error in RunOperation: " + e.Message);
                        throw e;
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
                    catch (Exception e)
                    {
                        TLogging.Log("Error in RunOperation: " + e.Message);
                        throw e;
                    }
                });

            return JobSize;
        }
    }     // TResetForwardPeriodICH
} // Ict.Petra.Server.MFinance.GL