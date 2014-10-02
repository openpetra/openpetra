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
                bool res = new TYearEnd().RunYearEnd(ALedgerNum, AIsInInfoMode, out AVerificationResult);

                if (!res)
                {
                    AVerificationResult.Add(new TVerificationResult("Year End", "Success", "Success", TResultSeverity.Resv_Status));
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
        /// </summary>
        /// <param name="carryForward"></param>
        public override void SetNextPeriod(TCarryForward carryForward)
        {
            // Set to the first month of the "next year".
            carryForward.SetProvisionalYearEndFlag(false);
            carryForward.SetNewFwdPeriodValue(1);
            carryForward.FledgerInfo.CurrentFinancialYear = carryForward.FledgerInfo.CurrentFinancialYear + 1;

            TAccountPeriodToNewYear accountPeriodOperator = new TAccountPeriodToNewYear(carryForward.FledgerInfo.LedgerNumber);
            accountPeriodOperator.IsInInfoMode = false;
            accountPeriodOperator.RunOperation();
        }

        /// <summary>
        /// Master routine ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AInfoMode"></param>
        /// <param name="AVRCollection"></param>
        /// <returns>True if an error occurred</returns>
        public bool RunYearEnd(int ALedgerNum, bool AInfoMode,
            out TVerificationResultCollection AVRCollection)
        {
            FInfoMode = AInfoMode;
            FledgerInfo = new TLedgerInfo(ALedgerNum);
            AVRCollection = new TVerificationResultCollection();
            FverificationResults = AVRCollection;

            TCarryForward carryForward = new TCarryForward(FledgerInfo, this);

            if (carryForward.GetPeriodType != TCarryForwardENum.Year)
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

            return FHasCriticalErrors;
        }
    } // TYearEnd

    /// <summary>
    /// If the oldest year of data is now beyond the retention period, archive it to file and delete those records.
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
        public override void RunOperation()
        {
        }

        /// <summary></summary>
        public override int GetJobSize()
        {
            return 0;
        }

        /// <summary></summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TArchive(FledgerInfo);
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

        TGlmInfo FglmInfo;

        List <String>FAccountList = null;


        /// <summary>
        ///
        /// </summary>
        public TReallocation(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        private void CalculateAccountList()
        {
            FaccountInfo = new TAccountInfo(FledgerInfo);
            bool blnIncomeFound = false;
            bool blnExpenseFound = false;

            FaccountInfo.Reset();
            FAccountList = new List <String>();

            while (FaccountInfo.MoveNext())
            {
                if (FaccountInfo.PostingStatus)
                {
                    if (FaccountInfo.AccountType.Equals(TAccountTypeEnum.Income.ToString()))
                    {
                        FAccountList.Add(FaccountInfo.AccountCode);
                        blnIncomeFound = true;
                    }

                    if (FaccountInfo.AccountType.Equals(TAccountTypeEnum.Expense.ToString()))
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
                        Catalog.GetString("You shall have at least one income"), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_09.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            if (!blnExpenseFound)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No Expense Account found"),
                        Catalog.GetString("You shall have at least one expense"), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_10.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
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
                        Catalog.GetString("You need to define this account"), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_11.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            FCostCentreTbl = ACostCentreAccess.LoadViaALedger(FledgerInfo.LedgerNumber, null);
            FCostCentreTbl.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
        }

        /// <summary>
        ///
        /// </summary>
        public override int GetJobSize()
        {
            bool blnHelp = FInfoMode;

            FInfoMode = true;
            FCountJobs = 0;
            RunOperation();
            FInfoMode = blnHelp;
            return FCountJobs;
        }

        /// <summary>
        ///
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TReallocation(FledgerInfo);
        }

        /// <summary>
        ///
        /// </summary>
        public override void RunOperation()
        {
            if (FAccountList == null)
            {
                CalculateAccountList();
            }

            TCommonAccountingTool YearEndBatch = null;

            if (DoExecuteableCode)
            {
                YearEndBatch =
                    new TCommonAccountingTool(FledgerInfo,
                        Catalog.GetString("Financial year end processing"));
            }

            if (DoExecuteableCode)
            {
                YearEndBatch.AddBaseCurrencyJournal();
                YearEndBatch.JournalDescription =
                    Catalog.GetString("Period end revaluations");
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
                            ReallocationLoop(YearEndBatch, strAccountCode, FglmInfo.CostCentreCode);
                        }
                    }
                }
            }

            if (DoExecuteableCode)
            {
                YearEndBatch.CloseSaveAndPost();
            }
        }

        private void ReallocationLoop(TCommonAccountingTool YearEndBatch, String AAccountCode, string ACostCentreCode)
        {
            bool blnDebitCredit;

            FaccountInfo.AccountCode = FglmInfo.AccountCode;

            blnDebitCredit = FaccountInfo.DebitCreditIndicator;

            string strCostCentreTo;
            string strAccountTo;

            string strCCAccoutType = FaccountInfo.SetCarryForwardAccount();

            if (FaccountInfo.IsValid)
            {
                strAccountTo = AAccountCode;

                if (strCCAccoutType.Equals("SAMECC"))
                {
                    strCostCentreTo = FglmInfo.CostCentreCode;
                    //blnCarryForward = true;
                }
                else
                {
                    strCostCentreTo = GetStandardCostCentre();
                }
            }
            else
            {
                FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.EARNINGS_BF_ACCT);
                strAccountTo = FaccountInfo.AccountCode;
                strCostCentreTo = GetStandardCostCentre();
            }

            if (FledgerInfo.IltAccountFlag)
            {
                FaccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
                strAccountTo = FaccountInfo.AccountCode;
            }

            if (FledgerInfo.BranchProcessing)
            {
                strAccountTo = AAccountCode;
            }

            string strYearEnd = Catalog.GetString("YEAR-END");
            string strNarrativeMessage = Catalog.GetString("Year end re-allocation to {0}:{1}");

            if (DoExecuteableCode)
            {
                YearEndBatch.AddBaseCurrencyTransaction(
                    AAccountCode, ACostCentreCode,
                    String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode),
                    strYearEnd, !blnDebitCredit, Math.Abs(FglmInfo.YtdActualBase));
            }

            if (DoExecuteableCode)
            {
                YearEndBatch.AddBaseCurrencyTransaction(
                    strAccountTo, strCostCentreTo,
                    String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode),
                    strYearEnd, blnDebitCredit, Math.Abs(FglmInfo.YtdActualBase));
            }

            ++FCountJobs;
        }

        private string GetStandardCostCentre()
        {
            return TLedgerInfo.GetStandardCostCentre(FledgerInfo.LedgerNumber);
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
            return 0;
        }

        /// <summary>
        /// The AccountingPeriod Rows are updated ...
        /// </summary>
        override public void RunOperation()
        {
            bool NewTransaction;
            Boolean ShouldCommit = false;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            AAccountingPeriodTable AccountingPeriodTbl = null;

            if (DoExecuteableCode)
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
                    }

                    AAccountingPeriodAccess.SubmitChanges(AccountingPeriodTbl, Transaction);
                    ShouldCommit = true;
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
            } // if
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
        int intEntryCount;


        /// <summary>
        /// </summary>
        public TGlmNewYearInit(TLedgerInfo ALedgerInfo, int AYear)
        {
            FCurrentYear = AYear;
            FNextYear = FCurrentYear + 1;
            FLedgerInfo = ALedgerInfo;

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
            return new TGlmNewYearInit(FLedgerInfo, FCurrentYear);
        }

        /// <summary>
        /// </summary>
        public override int GetJobSize()
        {
            bool blnOldInfoMode = FInfoMode;

            FInfoMode = true;
            RunOperation();
            FInfoMode = blnOldInfoMode;
            return intEntryCount;
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
        /// Next-Year records will be created, and the will be Ledger moved to the new year.
        /// </summary>
        public override void RunOperation()
        {
            Int32 TempGLMSequence = -1;
            ALedgerRow LedgerRow = FPostingFromDS.ALedger[0];

            intEntryCount = 0;

            if (!FInfoMode)
            {
                TCarryForward carryForward = new TCarryForward(FLedgerInfo, FPeriodEndOperator);
                carryForward.SetNextPeriod();
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

                    ++intEntryCount;
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
        } // RunEndOfPeriodOperation
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
            return 0;
        }

        /// <summary>
        /// In a_budget_period move this year’s values to last year, next year’s to this year, and set next year to zero.
        /// </summary>
        public override void RunOperation()
        {
        }

    } // TNewYearBudgets
*/

        /// <summary>
        /// Reset period columns on batch, journal and gift batch tables for periods beyond end of last year
        /// </summary>
        public class TResetForwardPeriodBatches : AbstractPeriodEndOperation
        {
            private TLedgerInfo FLedgerInfo;
            private Int32 JobSize = 0;

            public TResetForwardPeriodBatches(TLedgerInfo ALedgerInfo)
            {
                FLedgerInfo = ALedgerInfo;
            }

            /// <summary>
            ///
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
                RunOperation();
                return JobSize;
            }

            /// <summary>
            /// Reset period columns on batch, journal and gift batch tables for periods beyond end of the old year
            /// </summary>
            public override void RunOperation()
            {
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
                        " AND a_batch_year_i=" + (FLedgerInfo.CurrentFinancialYear - 1) +
                        " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    ABatchTable BatchTbl = (ABatchTable)DBAccess.GDBAccessObj.SelectDT(Query, "ABatch", Transaction);

                    JobSize = BatchTbl.Rows.Count;
                    foreach (ABatchRow BatchRow in BatchTbl.Rows)
                    {
                        BatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                        BatchRow.BatchYear += 1;
                    }
                    ABatchAccess.SubmitChanges(BatchTbl, Transaction);

                    Query =
                        "SELECT PUB_a_journal.* FROM PUB_a_batch, PUB_a_journal WHERE " +
                        " PUB_a_journal.a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                        " AND PUB_a_batch.a_batch_number_i= PUB_a_journal.a_batch_number_i" +
                        " AND PUB_a_batch.a_batch_year_i=" + (FLedgerInfo.CurrentFinancialYear - 1) +
                        " AND a_journal_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    AJournalTable JournalTbl = (AJournalTable)DBAccess.GDBAccessObj.SelectDT(Query, "AJournal", Transaction);

                    foreach (AJournalRow JournalRow in JournalTbl.Rows)
                    {
                        JournalRow.JournalPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                    }
                    AJournalAccess.SubmitChanges(JournalTbl, Transaction);

                    Query =
                        "SELECT * FROM PUB_a_gift_batch WHERE " +
                        " a_ledger_number_i=" + FLedgerInfo.LedgerNumber +
                        " AND a_batch_year_i=" + (FLedgerInfo.CurrentFinancialYear - 1) +
                        " AND a_batch_period_i>" + FLedgerInfo.NumberOfAccountingPeriods;
                    AGiftBatchTable GiftBatchTbl = (AGiftBatchTable)DBAccess.GDBAccessObj.SelectDT(Query, "AGiftBatch", Transaction);

                    JobSize += GiftBatchTbl.Rows.Count;

                    foreach (AGiftBatchRow GiftBatchRow in GiftBatchTbl.Rows)
                    {
                        GiftBatchRow.BatchPeriod -= FLedgerInfo.NumberOfAccountingPeriods;
                        GiftBatchRow.BatchYear += 1;
                    }
                    if (!FInfoMode)
                    {
                        AGiftBatchAccess.SubmitChanges(GiftBatchTbl, Transaction);
                        ShouldCommit = true;
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

            }

        } // TNewYearBudgets


} // Ict.Petra.Server.MFinance.GL