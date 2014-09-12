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
    public partial class TPeriodIntervallConnector
    {
        /// <summary>
        /// Routine to run the period end calculations ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AIsInInfoMode">True means: No Calculation is done, only
        /// verification result messages are collected</param>
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
                TLogging.Log("TPeriodIntervallConnector.TPeriodYearEnd() throws " + e.ToString());
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
    /// Modul for the year end calculations ...
    /// </summary>
    public class TYearEnd : TPeriodEndOperations
    {
        TLedgerInfo FledgerInfo;

        /// <summary>
        /// Master routine ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AInfoMode"></param>
        /// <param name="AVRCollection"></param>
        /// <returns></returns>
        public bool RunYearEnd(int ALedgerNum, bool AInfoMode,
            out TVerificationResultCollection AVRCollection)
        {
            FInfoMode = AInfoMode;
            FledgerInfo = new TLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();

            TCarryForward carryForward = new TCarryForward(FledgerInfo);

            if (carryForward.GetPeriodType != TCarryForwardENum.Year)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Month End is required!"),
                        Catalog.GetString("In this situation you cannot run Year End."), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_04.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            RunPeriodEndSequence(new TReallocation(FledgerInfo),
                Catalog.GetString("Reallocation of all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(FledgerInfo.LedgerNumber, FledgerInfo.CurrentFinancialYear),
                Catalog.GetString("Initialize glm entries for next year"));

            AVRCollection = verificationResults;
            return FHasCriticalErrors;
        }
    }


    /// <summary>
    /// The Reallocation Module ...
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
            RunEndOfPeriodOperation();
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
        public override void RunEndOfPeriodOperation()
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
    }
}