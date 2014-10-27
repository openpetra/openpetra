//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
            bool NewTransaction;

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                TLedgerInfo LedgerInfo = new TLedgerInfo(ALedgerNum);
                bool res = new TYearEnd(LedgerInfo).RunYearEnd(AIsInInfoMode, out AVerificationResult);

                if (!res)
                {
                    String SuccessMsg = AIsInInfoMode ? "YearEnd check: No problems found." : "Success.";
                    AVerificationResult.Add(new TVerificationResult("Year End", SuccessMsg, "Success", TResultSeverity.Resv_Status));
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                return res;
            }
            catch (Exception e)
            {
                TLogging.Log("TPeriodIntervalConnector.TPeriodYearEnd() throws " + e.ToString());
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(
                    new TVerificationResult(
                        Catalog.GetString("Year End"),
                        Catalog.GetString("Uncaught Exception: ") + e.Message,
                        TResultSeverity.Resv_Critical));

                DBAccess.GDBAccessObj.RollbackTransaction();

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

            RunPeriodEndSequence(new TArchive(FledgerInfo),
                Catalog.GetString("Archive old financial information"));

            RunPeriodEndSequence(new TReallocation(FledgerInfo),
                Catalog.GetString("Reallocate all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(FledgerInfo, FledgerInfo.CurrentFinancialYear, this),
                Catalog.GetString("Initialize the database for next year"));

/* As far as we can tell, there's nothing to do for budgets:
            RunPeriodEndSequence(new TNewYearBudgets(FledgerInfo),
                Catalog.GetString("Initialise budgets for next year"));
*/

            RunPeriodEndSequence(new TResetForwardPeriodBatches(FledgerInfo),
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

                        if ((glmpInfo.ActualBase != 0) && (FglmInfo.YtdActualBase != 0))
                        {
                            if (DoExecuteableCode)
                            {
                                ReallocationLoop(YearEndBatch, strAccountCode, FglmInfo.CostCentreCode);
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

        private void ReallocationLoop(TCommonAccountingTool YearEndBatch, String AAccountFrom, String ACostCentreFrom)
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

            Decimal TransactionAmount = Math.Abs(FglmInfo.YtdActualBase);

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
            bool NewTransaction;
            Int32 JobSize = 0;

            Boolean ShouldCommit = false;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            AAccountingPeriodTable AccountingPeriodTbl = null;

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
                        ShouldCommit = true;
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("Exception during running the AccountPeriod To New Year operation:" + Environment.NewLine + Exc.ToString());
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    if (ShouldCommit)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
            }
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
        GLPostingTDS FPostingFromDS = null;
        GLPostingTDS FPostingToDS = null;

        int FCurrentYear;
        int FNextYear;
        TLedgerInfo FLedgerInfo;
        TYearEnd FYearEndOperator;


        /// <summary>
        /// </summary>
        public TGlmNewYearInit(TLedgerInfo ALedgerInfo, int AYear, TYearEnd AYearEndOperator)
        {
            FCurrentYear = AYear;
            FNextYear = FCurrentYear + 1;
            FLedgerInfo = ALedgerInfo;
            FYearEndOperator = AYearEndOperator;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                FPostingFromDS = LoadTable(FLedgerInfo.LedgerNumber, FCurrentYear, transaction);
                FPostingToDS = LoadTable(FLedgerInfo.LedgerNumber, FNextYear, transaction);
                ALedgerAccess.LoadByPrimaryKey(FPostingFromDS, FLedgerInfo.LedgerNumber, transaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TGlmNewYearInit(FLedgerInfo, FCurrentYear, FYearEndOperator);
        }

        /// <summary>
        /// </summary>
        public override int GetJobSize()
        {
            bool blnOldInfoMode = FInfoMode;
            FInfoMode = true;
            int EntryCount = RunOperation();
            FInfoMode = blnOldInfoMode;
            return EntryCount;
        }

        private GLPostingTDS LoadTable(int ALedgerNumber, int AYear, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = AYear;

            string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetYearDBName() + " = ? ";

            GLPostingTDS PostingDS = new GLPostingTDS();

            DBAccess.GDBAccessObj.Select(PostingDS,
                strSQL, AGeneralLedgerMasterTable.GetTableName(), ATransaction, ParametersArray);

            return PostingDS;
        }

        /// <summary>
        /// Next-Year records will be created, and the Ledger will be moved to the new year.
        /// </summary>
        public override Int32 RunOperation()
        {
            Int32 TempGLMSequence = -1;
            ALedgerRow LedgerRow = FPostingFromDS.ALedger[0];
            Int32 EntryCount = 0;

            if (!FInfoMode)
            {
                FYearEndOperator.SetNextPeriod();
            }

            FPostingToDS.AGeneralLedgerMaster.DefaultView.Sort =
                AGeneralLedgerMasterTable.GetAccountCodeDBName() +
                "," +
                AGeneralLedgerMasterTable.GetCostCentreCodeDBName();

            foreach (AGeneralLedgerMasterRow generalLedgerMasterRowFrom in FPostingFromDS.AGeneralLedgerMaster.Rows)
            {
                AGeneralLedgerMasterRow generalLedgerMasterRowTo = null;
                //
                // If there's not already a row for this Account / Cost Centre,
                // I need to create one now...
                Int32 RowIdx = FPostingToDS.AGeneralLedgerMaster.DefaultView.Find(
                    new Object[] { generalLedgerMasterRowFrom.AccountCode, generalLedgerMasterRowFrom.CostCentreCode }
                    );

                if (RowIdx >= 0)
                {
                    generalLedgerMasterRowTo = (AGeneralLedgerMasterRow)FPostingToDS.AGeneralLedgerMaster.DefaultView[RowIdx].Row;
                }
                else        // GLM record Not present - I'll make one now...
                {
                    if (!FInfoMode)
                    {
                        generalLedgerMasterRowTo =
                            (AGeneralLedgerMasterRow)FPostingToDS.AGeneralLedgerMaster.NewRowTyped(true);
                        generalLedgerMasterRowTo.GlmSequence = TempGLMSequence;
                        TempGLMSequence--;
                        generalLedgerMasterRowTo.LedgerNumber = LedgerRow.LedgerNumber;
                        generalLedgerMasterRowTo.AccountCode = generalLedgerMasterRowFrom.AccountCode;
                        generalLedgerMasterRowTo.CostCentreCode = generalLedgerMasterRowFrom.CostCentreCode;

                        FPostingToDS.AGeneralLedgerMaster.Rows.Add(generalLedgerMasterRowTo);
                    }

                    ++EntryCount;
                }

                if (!FInfoMode)
                {
                    generalLedgerMasterRowTo.Year = FNextYear;
                    generalLedgerMasterRowTo.YtdActualBase = generalLedgerMasterRowFrom.YtdActualBase; // What if there was already a balance here?

                    Boolean IncludeForeign = !generalLedgerMasterRowFrom.IsYtdActualForeignNull();

                    if (IncludeForeign)
                    {
                        generalLedgerMasterRowTo.YtdActualForeign = generalLedgerMasterRowFrom.YtdActualForeign;
                    }

                    if (RowIdx < 0) // If I created a new generalLedgerMasterRowTo, I need to also create a clutch of matching GLMP records:
                    {
                        for (int PeriodCount = 1;
                             PeriodCount < LedgerRow.NumberOfAccountingPeriods + LedgerRow.NumberFwdPostingPeriods + 1;
                             PeriodCount++)
                        {
                            AGeneralLedgerMasterPeriodRow glmPeriodRow = FPostingToDS.AGeneralLedgerMasterPeriod.NewRowTyped(true);
                            glmPeriodRow.GlmSequence = generalLedgerMasterRowTo.GlmSequence;
                            glmPeriodRow.PeriodNumber = PeriodCount;
                            FPostingToDS.AGeneralLedgerMasterPeriod.Rows.Add(glmPeriodRow);
                            glmPeriodRow.ActualBase = generalLedgerMasterRowTo.YtdActualBase;

                            if (IncludeForeign)
                            {
                                glmPeriodRow.ActualForeign = generalLedgerMasterRowTo.YtdActualForeign;
                            }
                        }
                    }
                }
            }

            if (DoExecuteableCode)
            {
                FPostingToDS.ThrowAwayAfterSubmitChanges = true;
                GLPostingTDSAccess.SubmitChanges(FPostingToDS);
            }
            return EntryCount;
        } // RunOperation
    } // TGlmNewYearInit

    /*
     * As far as we can tell, there's nothing to do with the budgets:

        /// <summary>
        /// 
        /// </summary>
        public class TNewYearBudgets : AbstractPeriodEndOperation
        {
            private TLedgerInfo FLedgerInfo;

            public TNewYearBudgets(TLedgerInfo ALedgerInfo)
            {
                FLedgerInfo = ALedgerInfo;
            }

            /// <summary>
            ///
            /// </summary>
            public override AbstractPeriodEndOperation GetActualizedClone()
            {
                return new TNewYearBudgets(FLedgerInfo);
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
            /// In a_budget_period move this year’s values to last year, next year’s to this year, and set next year to zero.
            /// </summary>
            public override Int32 RunOperation()
            {
                return 0;
            }

        } // TNewYearBudgets
    */

    /// <summary>
        /// Reset period columns on batch, journal and gift batch tables for periods beyond end of last year
        /// </summary>
        public class TResetForwardPeriodBatches : AbstractPeriodEndOperation
        {
            private TLedgerInfo FLedgerInfo;

            /// <summary>
            /// </summary>
            /// <param name="ALedgerInfo"></param>
            public TResetForwardPeriodBatches(TLedgerInfo ALedgerInfo)
            {
                FLedgerInfo = ALedgerInfo;
            }

            /// <summary>
            /// </summary>
            public override AbstractPeriodEndOperation GetActualizedClone()
            {
                return new TResetForwardPeriodBatches(FLedgerInfo);
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
                bool NewTransaction;
                Boolean ShouldCommit = false;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    String Query =
                        "SELECT * FROM PUB_a_batch WHERE " +
                        "a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                        " AND a_batch_year_i=" + FLedgerInfo.CurrentFinancialYear +
                        " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    DataTable Tbl = DBAccess.GDBAccessObj.SelectDT(Query, "ABatch", Transaction);
                    if (Tbl.Rows.Count > 0)
                    {
                        ABatchTable BatchTbl = new ABatchTable();
                        BatchTbl.Merge(Tbl);

                        JobSize = BatchTbl.Rows.Count;

                        if (!FInfoMode)
                        {
                            foreach (ABatchRow BatchRow in BatchTbl.Rows)
                            {
                                BatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                                BatchRow.BatchYear += 1;
                            }
                            ABatchAccess.SubmitChanges(BatchTbl, Transaction);
                            ShouldCommit = true;
                        }
                    }

                    Query =
                        "SELECT PUB_a_journal.* FROM PUB_a_batch, PUB_a_journal WHERE " +
                        " PUB_a_journal.a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                        " AND PUB_a_batch.a_batch_number_i= PUB_a_journal.a_batch_number_i" +
                        " AND PUB_a_batch.a_batch_year_i=" + FLedgerInfo.CurrentFinancialYear +
                        " AND a_journal_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    Tbl = DBAccess.GDBAccessObj.SelectDT(Query, "AJournal", Transaction);
                    if (Tbl.Rows.Count > 0)
                    {
                        AJournalTable JournalTbl = new AJournalTable();
                        JournalTbl.Merge(Tbl);

                        if (!FInfoMode)
                        {
                            foreach (AJournalRow JournalRow in JournalTbl.Rows)
                            {
                                JournalRow.JournalPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                            }
                            AJournalAccess.SubmitChanges(JournalTbl, Transaction);
                            ShouldCommit = true;
                        }
                    }

                    Query =
                        "SELECT * FROM PUB_a_gift_batch WHERE " +
                        " a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                        " AND a_batch_year_i=" + FLedgerInfo.CurrentFinancialYear +
                        " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    Tbl = DBAccess.GDBAccessObj.SelectDT(Query, "AGiftBatch", Transaction);
                    if (Tbl.Rows.Count > 0)
                    {
                        AGiftBatchTable GiftBatchTbl = new AGiftBatchTable();
                        GiftBatchTbl.Merge(Tbl);

                        JobSize += GiftBatchTbl.Rows.Count;

                        if (!FInfoMode)
                        {
                            foreach (AGiftBatchRow GiftBatchRow in GiftBatchTbl.Rows)
                            {
                                GiftBatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                                GiftBatchRow.BatchYear += 1;
                            }

                            AGiftBatchAccess.SubmitChanges(GiftBatchTbl, Transaction);
                            ShouldCommit = true;
                        }
                    }
                } // try
                finally
                {
                    if (NewTransaction)
                    {
                        if (ShouldCommit)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
                        }
                        else
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
                        }
                    }
                }
                return JobSize;
            }

        } // TResetForwardPeriodBatches

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
                bool NewTransaction;
                Boolean ShouldCommit = false;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

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
                            ShouldCommit = true;
                        }
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        if (ShouldCommit)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
                        }
                        else
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
                        }
                    }
                }
                return JobSize;
            }

        } // TResetForwardPeriodICH


} // Ict.Petra.Server.MFinance.GL