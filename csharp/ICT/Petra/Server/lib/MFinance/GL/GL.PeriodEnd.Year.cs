//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
            bool res = new TYearEnd().RunYearEnd(ALedgerNum, AIsInInfoMode,
                out AVerificationResult);

            if (!res)
            {
                AVerificationResult.Add(new TVerificationResult("Year End", "Success", "Success", TResultSeverity.Resv_Status));
            }

            return res;
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Modul for the year end calculations ...
    /// </summary>
    public class TYearEnd : TPeriodEndOperations
    {
        TLedgerInfo ledgerInfo;

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
            ledgerInfo = new TLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();

            TCarryForward carryForward = new TCarryForward(ledgerInfo);
            int intYear = 0;

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
            else
            {
                intYear = carryForward.Year;
            }

            RunPeriodEndSequence(new TReallocation(ledgerInfo),
                Catalog.GetString("Reallocation of all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(ledgerInfo.LedgerNumber, intYear),
                Catalog.GetString("Initialize the glm-entries of the next year"));

            RunPeriodEndSequence(new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber, intYear),
                Catalog.GetString("Set the account period values to the New Year"));

            if (!FInfoMode)
            {
                if (!FHasCriticalErrors)
                {
                    carryForward.SetNextPeriod();
                }
            }

            AVRCollection = verificationResults;
            return FHasCriticalErrors;
        }
    }


    /// <summary>
    /// The Reallocation Module ...
    /// </summary>
    public class TReallocation : AbstractPeriodEndOperation
    {
        TLedgerInfo ledgerInfo;
        TAccountInfo accountInfo;
        ACostCentreTable costCentres;

        TGlmpInfo glmpInfo;
        TGlmInfo glmInfo;

        TCommonAccountingTool tCommonAccountingTool;
        List <String>accountList = null;


        /// <summary>
        ///
        /// </summary>
        public TReallocation(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
        }

        private void CalculateAccountList()
        {
            accountInfo = new TAccountInfo(ledgerInfo);
            bool blnIncomeFound = false;
            bool blnExpenseFound = false;

            accountInfo.Reset();
            accountList = new List <String>();

            while (accountInfo.MoveNext())
            {
                if (accountInfo.PostingStatus)
                {
                    if (accountInfo.AccountType.Equals(TAccountTypeEnum.Income.ToString()))
                    {
                        accountList.Add(accountInfo.AccountCode);
                        blnIncomeFound = true;
                    }

                    if (accountInfo.AccountType.Equals(TAccountTypeEnum.Expense.ToString()))
                    {
                        accountList.Add(accountInfo.AccountCode);
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

            accountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);

            if (accountInfo.IsValid)
            {
                accountList.Add(accountInfo.AccountCode);
            }

/*
 * Until any evidence shows otherwise, I'm going to assume that it's OK to operate OpenPetra
 * WITHOUT an ICH Account.
 *
 *          else
 *          {
 *              TVerificationResult tvt =
 *                  new TVerificationResult(Catalog.GetString("No ICH_ACCT Account defined"),
 *                      Catalog.GetString("You need to define this account"), "",
 *                      TPeriodEndErrorAndStatusCodes.PEEC_11.ToString(),
 *                      TResultSeverity.Resv_Critical);
 *              verificationResults.Add(tvt);
 *              FHasCriticalErrors = true;
 *          }
 */
            costCentres = ACostCentreAccess.LoadViaALedger(ledgerInfo.LedgerNumber, null);
        }

        /// <summary>
        ///
        /// </summary>
        public override int JobSize {
            get
            {
                bool blnHelp = FInfoMode;
                FInfoMode = true;
                intCountJobs = 0;
                RunEndOfPeriodOperation();
                FInfoMode = blnHelp;
                return intCountJobs;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TReallocation(ledgerInfo);
        }

        /// <summary>
        ///
        /// </summary>
        public override void RunEndOfPeriodOperation()
        {
            if (accountList == null)
            {
                CalculateAccountList();
            }

            if (DoExecuteableCode)
            {
                tCommonAccountingTool =
                    new TCommonAccountingTool(ledgerInfo,
                        Catalog.GetString("Financial year end processing"));
            }

            glmpInfo = new TGlmpInfo();

            if (DoExecuteableCode)
            {
                tCommonAccountingTool.AddBaseCurrencyJournal();
                tCommonAccountingTool.JournalDescription =
                    Catalog.GetString("Period end revaluations");
                tCommonAccountingTool.SubSystemCode = CommonAccountingSubSystemsEnum.GL;
            }

            // tCommonAccountingTool.DateEffective =""; Default is "End of actual period ..."
            // Loop with all account codes

            if (accountList.Count > 0)
            {
                string strAccountCode;

                for (int i = 0; i < accountList.Count; ++i)
                {
                    strAccountCode = accountList[i];
                    glmInfo = new TGlmInfo(ledgerInfo.LedgerNumber,
                        ledgerInfo.CurrentFinancialYear,
                        strAccountCode);

                    // Loop with all cost centres
                    glmInfo.Reset();

                    ACostCentreRow currentCostCentre;
                    costCentres.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();

                    while (glmInfo.MoveNext())
                    {
                        currentCostCentre = (ACostCentreRow)costCentres.DefaultView[costCentres.DefaultView.Find(glmInfo.CostCentreCode)].Row;

                        if (currentCostCentre.PostingCostCentreFlag)
                        {
                            if (glmpInfo.SetToRow(
                                    glmInfo.GlmSequence,
                                    ledgerInfo.NumberOfAccountingPeriods))
                            {
                                if (glmpInfo.ActualBase != 0)
                                {
                                    if (glmInfo.YtdActualBase != 0)
                                    {
                                        ReallocationLoop(strAccountCode,
                                            glmInfo.CostCentreCode);
                                    }
                                }
                            }
                        }
                    }
                }

                if (DoExecuteableCode)
                {
                    tCommonAccountingTool.CloseSaveAndPost();
                }
            }
        }

        private void ReallocationLoop(String AAccountCode, string ACostCentreCode)
        {
            bool blnDebitCredit;

            accountInfo.AccountCode = glmInfo.AccountCode;

            blnDebitCredit = accountInfo.DebitCreditIndicator;

            string strCostCentreTo;
            string strAccountTo;

            string strCCAccoutType = accountInfo.SetCarryForwardAccount();

            if (accountInfo.IsValid)
            {
                strAccountTo = AAccountCode;

                if (strCCAccoutType.Equals("SAMECC"))
                {
                    strCostCentreTo = glmInfo.CostCentreCode;
                    //blnCarryForward = true;
                }
                else
                {
                    strCostCentreTo = GetStandardCostCentre();
                }
            }
            else
            {
                accountInfo.SetSpecialAccountCode(TAccountPropertyEnum.EARNINGS_BF_ACCT);
                strAccountTo = accountInfo.AccountCode;
                strCostCentreTo = GetStandardCostCentre();
            }

            if (ledgerInfo.IltAccountFlag)
            {
                accountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
                strAccountTo = accountInfo.AccountCode;
            }

            if (ledgerInfo.BranchProcessing)
            {
                strAccountTo = AAccountCode;
            }

            string strYearEnd = Catalog.GetString("YEAR-END");
            string strNarrativeMessage = Catalog.GetString("Year end re-allocation to {0}:{1}");
            string strBuildNarrative = String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode);

            if (DoExecuteableCode)
            {
                tCommonAccountingTool.AddBaseCurrencyTransaction(
                    AAccountCode, ACostCentreCode, strBuildNarrative,
                    strYearEnd, !blnDebitCredit, Math.Abs(glmInfo.YtdActualBase));
            }

            strBuildNarrative = String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode);

            if (DoExecuteableCode)
            {
                tCommonAccountingTool.AddBaseCurrencyTransaction(
                    strAccountTo, strCostCentreTo, strBuildNarrative,
                    strYearEnd, blnDebitCredit, Math.Abs(glmInfo.YtdActualBase));
            }

            ++intCountJobs;
        }

        private string GetStandardCostCentre()
        {
            return TLedgerInfo.GetStandardCostCentre(ledgerInfo.LedgerNumber);
        }
    }
}