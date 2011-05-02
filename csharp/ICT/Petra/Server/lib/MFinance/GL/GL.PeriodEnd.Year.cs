//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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
        /// Routine to initialize the "Hello" Message if you want to start the
        /// periodic month end.
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if critical values appeared otherwise false</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodYearEndInfo(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TYearEnd().RunYearEnd(ALedgerNum, true,
                out AVerificationResult);
        }

        /// <summary>
        /// Routine to run the finally month end ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodYearEnd(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TYearEnd().RunYearEnd(ALedgerNum, false,
                out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Modul for the year end calculations ...
    /// </summary>
    public class TYearEnd : AbstractPerdiodEndOperations
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
            blnIsInInfoMode = AInfoMode;
            ledgerInfo = new TLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();

            TCarryForward carryForward = new TCarryForward(ledgerInfo);

            if (carryForward.GetPeriodType != TCarryForwardENum.Year)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Next Month is expected ..."),
                        Catalog.GetString("In this situation you cannot run a year end routine"), "",
                        TYearEndErrorStatus.PEYM_02.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            TGetAccountingYear accountingYear = new TGetAccountingYear(ledgerInfo);
            int intYear = accountingYear.Year;

            RunPeriodEndSequence(new TReallocation(ledgerInfo),
                Catalog.GetString("Reallocation of all income and expense accounts"));

            RunPeriodEndSequence(new TGlmNewYearInit(ledgerInfo.LedgerNumber, intYear),
                Catalog.GetString("Initialize the glm-entries of the next year"));

            RunPeriodEndSequence(new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber, intYear),
                Catalog.GetString("Set the account period values to the New Year"));

            if (!blnIsInInfoMode)
            {
                if (!blnCriticalErrors)
                {
                    carryForward.SetNextPeriod();
                }
            }

            AVRCollection = verificationResults;
            return blnCriticalErrors;
        }
    }

    public class TGetAccountingYear
    {
        TLedgerInfo ledgerInfo;
        int intActualYear;

        TLedgerInitFlagHandler ledgerFlag;

        public TGetAccountingYear(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
        }

        public int Year
        {
            get
            {
                ledgerFlag =
                    new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber, TLedgerInitFlagEnum.ActualYear);
                TAccountPeriodToNewYear accountPeriod = new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber);
                int intYear = accountPeriod.ActualYear;
                ledgerFlag.AddMarker((intYear - 1).ToString());

                if (ledgerFlag.Flag)
                {
                    intActualYear = intYear - 1;
                }
                else
                {
                    intActualYear = intYear;
                    ledgerFlag.AddMarker(intYear.ToString());
                    ledgerFlag.Flag = true;
                }

                return intActualYear;
            }
        }

        public void RemoveMarker()
        {
            ledgerFlag.Flag = false;
        }
    }

    public class TReallocation : AbstractPerdiodEndOperation
    {
        TLedgerInfo ledgerInfo;
        TAccountInfo accountInfo;
        TCostCenterInfo costCenterInfo;

        TGlmpInfo glmpInfo;
        TGlmInfo glmInfo;

        TCommonAccountingTool tCommonAccountingTool;
        List <String>accountList = null;


        public TReallocation(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
        }

        private void CaculateAccountList()
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
                        TYearEndErrorStatus.PEYM_03.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            if (!blnExpenseFound)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No Expense Account found"),
                        Catalog.GetString("You shall have at least one expense"), "",
                        TYearEndErrorStatus.PEYM_04.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            accountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);

            if (accountInfo.IsValid)
            {
                accountList.Add(accountInfo.AccountCode);
            }
            else
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("No ICH_ACCT Account defined"),
                        Catalog.GetString("You need to define this account"), "",
                        TYearEndErrorStatus.PEYM_05.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            costCenterInfo = new TCostCenterInfo(ledgerInfo.LedgerNumber);
        }

        public override int JobSize {
            get
            {
                if (accountList == null)
                {
                    CaculateAccountList();
                }

                return accountList.Count;
            }
        }

        public override AbstractPerdiodEndOperation GetActualizedClone()
        {
            return new TReallocation(ledgerInfo);
        }

        public override void RunEndOfPeriodOperation()
        {
            if (accountList == null)
            {
                CaculateAccountList();
            }

            tCommonAccountingTool =
                new TCommonAccountingTool(ledgerInfo,
                    Catalog.GetString("Financial year end processing"));
            glmpInfo = new TGlmpInfo();

            tCommonAccountingTool.AddBaseCurrencyJournal();
            tCommonAccountingTool.JournalDescription =
                Catalog.GetString("Period end revaluations");
            tCommonAccountingTool.SubSystemCode = CommonAccountingSubSystemsEnum.GL;

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

                    while (glmInfo.MoveNext())
                    {
                        costCenterInfo.SetCostCenterRow(glmInfo.CostCentreCode);

                        if (costCenterInfo.IsValid)
                        {
                            if (costCenterInfo.PostingCostCentreFlag)
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
                }

                tCommonAccountingTool.CloseSaveAndPost();
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

            tCommonAccountingTool.AddBaseCurrencyTransaction(
                AAccountCode, ACostCentreCode, strBuildNarrative,
                strYearEnd, !blnDebitCredit, Math.Abs(glmInfo.YtdActualBase));


            strBuildNarrative = String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode);
            tCommonAccountingTool.AddBaseCurrencyTransaction(
                strAccountTo, strCostCentreTo, strBuildNarrative,
                strYearEnd, blnDebitCredit, Math.Abs(glmInfo.YtdActualBase));
        }

        private string GetStandardCostCentre()
        {
            return ledgerInfo.LedgerNumber.ToString() + "00";
        }
    }

    /// <summary>
    /// This object handles the transformation of the accouting interval parameters into the
    /// next year
    /// </summary>
    public class TAccountPeriodToNewYear : AbstractPerdiodEndOperation
    {
        int intLedgerNumber;
        int intActualYear;
        AAccountingPeriodTable accountingPeriodTable = null;
        AAccountingPeriodRow accountingPeriodRow = null;

        /// <summary>
        /// Constructor to define and load the complete table defined by the same ledger number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TAccountPeriodToNewYear(int ALedgerNumber, int AActualYear)
        {
            intActualYear = AActualYear;
            intLedgerNumber = ALedgerNumber;
            LoadData();
        }

        public TAccountPeriodToNewYear(int ALedgerNumber)
        {
            intLedgerNumber = ALedgerNumber;
            LoadData();
        }

        public int ActualYear
        {
            get
            {
                accountingPeriodRow = accountingPeriodTable[0];
                return accountingPeriodRow.PeriodStartDate.Year;
            }
        }

        private void LoadData()
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            try
            {
                accountingPeriodTable = AAccountingPeriodAccess.LoadViaALedger(intLedgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        public override AbstractPerdiodEndOperation GetActualizedClone()
        {
            return new TAccountPeriodToNewYear(intLedgerNumber, intActualYear);
        }

        public override int JobSize {
            get
            {
                int cnt = 0;

                for (int i = 0; i < accountingPeriodTable.Rows.Count; ++i)
                {
                    bool blnFound = false;
                    accountingPeriodRow = accountingPeriodTable[i];

                    if (accountingPeriodRow.PeriodStartDate.Year == intActualYear)
                    {
                        blnFound = true;
                    }

                    if (accountingPeriodRow.PeriodEndDate.Year == intActualYear)
                    {
                        blnFound = true;
                    }

                    if (accountingPeriodRow.EffectiveDate.Year == intActualYear)
                    {
                        blnFound = true;
                    }

                    if (blnFound)
                    {
                        ++cnt;
                    }
                }

                return cnt;
            }
        }

        override public void RunEndOfPeriodOperation()
        {
            if (!blnIsInInfoMode)
            {
                int year = intActualYear + 1;

                for (int i = 0; i < accountingPeriodTable.Rows.Count; ++i)
                {
                    accountingPeriodRow = accountingPeriodTable[i];

                    if (accountingPeriodRow.PeriodStartDate.Year == intActualYear)
                    {
                        accountingPeriodRow.PeriodStartDate =
                            accountingPeriodRow.PeriodStartDate.AddDays(1).AddYears(1).AddDays(-1);
                    }

                    if (accountingPeriodRow.PeriodEndDate.Year == intActualYear)
                    {
                        accountingPeriodRow.PeriodEndDate =
                            accountingPeriodRow.PeriodEndDate.AddDays(1).AddYears(1).AddDays(-1);
                    }

                    if (accountingPeriodRow.EffectiveDate.Year == intActualYear)
                    {
                        accountingPeriodRow.EffectiveDate =
                            accountingPeriodRow.EffectiveDate.AddDays(1).AddYears(1).AddDays(-1);
                    }
                }

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                try
                {
                    bool d = AAccountingPeriodAccess.SubmitChanges(
                        accountingPeriodTable, transaction,
                        out verificationResults);
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (Exception exception)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw exception;
                }
            }
        }
    }

    /// <summary>
    /// This Object read all glm year end record of the actual year and creates the start record for
    /// the next year
    /// </summary>
    public class TGlmNewYearInit : AbstractPerdiodEndOperation
    {
        GLBatchTDS glBatchFrom = null;
        GLBatchTDS glBatchTo = null;
        AGeneralLedgerMasterRow generalLedgerMasterRowFrom = null;
        AGeneralLedgerMasterRow generalLedgerMasterRowTo = null;

        TVerificationResultCollection tVerificationResultCollection;

        int intThisYear;
        int intNextYear;
        int intLedgerNumber;
        int intEntryCount;


        /// <summary>
        /// Ledger number and Year must be defined.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        public TGlmNewYearInit(int ALedgerNumber, int AYear)
        {
            intThisYear = AYear;
            intNextYear = intNextYear + 1;
            intLedgerNumber = ALedgerNumber;
            glBatchFrom = LoadTable(ALedgerNumber, AYear);
            glBatchTo = LoadTable(ALedgerNumber, ++AYear);
        }

        public override AbstractPerdiodEndOperation GetActualizedClone()
        {
            return new TGlmNewYearInit(intLedgerNumber, intThisYear);
        }

        public override int JobSize {
            get
            {
                bool blnOldInfoMode = blnIsInInfoMode;
                blnIsInInfoMode = true;
                RunEndOfPeriodOperation();
                blnIsInInfoMode = blnOldInfoMode;
                return intEntryCount;
            }
        }

        private GLBatchTDS LoadTable(int ALedgerNumber, int AYear)
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

            GLBatchTDS gLBatchTDS = new GLBatchTDS();

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                DBAccess.GDBAccessObj.Select(gLBatchTDS,
                    strSQL, AGeneralLedgerMasterTable.GetTableName(), transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
                return gLBatchTDS;
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        /// <summary>
        /// Next-Year records will be created.
        /// </summary>
        public override void RunEndOfPeriodOperation()
        {
            Int32 TempGLMSequence = -1;

            intEntryCount = 0;

            if (glBatchFrom.AGeneralLedgerMaster.Rows.Count > 0)
            {
                for (int i = 0; i < glBatchFrom.AGeneralLedgerMaster.Rows.Count; ++i)
                {
                    generalLedgerMasterRowFrom =
                        (AGeneralLedgerMasterRow)glBatchFrom.AGeneralLedgerMaster[i];
                    generalLedgerMasterRowTo = null;

                    for (int j = 0; j < glBatchTo.AGeneralLedgerMaster.Rows.Count; ++j)
                    {
                        generalLedgerMasterRowTo =
                            (AGeneralLedgerMasterRow)glBatchTo.AGeneralLedgerMaster[j];

                        if ((generalLedgerMasterRowFrom.AccountCode == generalLedgerMasterRowTo.AccountCode)
                            && (generalLedgerMasterRowFrom.CostCentreCode == generalLedgerMasterRowTo.CostCentreCode))
                        {
                            break;
                        }
                        else
                        {
                            generalLedgerMasterRowTo = null;
                        }
                    }

                    if (generalLedgerMasterRowTo == null)
                    {
                        if (!blnIsInInfoMode)
                        {
                            generalLedgerMasterRowTo =
                                (AGeneralLedgerMasterRow)glBatchTo.AGeneralLedgerMaster.NewRowTyped(true);
                            generalLedgerMasterRowTo.GlmSequence = TempGLMSequence;
                            TempGLMSequence--;
                            glBatchTo.AGeneralLedgerMaster.Rows.Add(generalLedgerMasterRowTo);
                        }

                        ++intEntryCount;
                    }

                    if (!blnIsInInfoMode)
                    {
                        generalLedgerMasterRowTo.LedgerNumber = generalLedgerMasterRowFrom.LedgerNumber;
                        generalLedgerMasterRowTo.Year = intNextYear;
                        generalLedgerMasterRowTo.AccountCode = generalLedgerMasterRowFrom.AccountCode;
                        generalLedgerMasterRowTo.CostCentreCode = generalLedgerMasterRowFrom.CostCentreCode;
                        generalLedgerMasterRowTo.YtdActualBase = generalLedgerMasterRowFrom.YtdActualBase;
                    }
                }
            }

            if (!blnIsInInfoMode)
            {
                TSubmitChangesResult tSubmitChangesResult =
                    GLBatchTDSAccess.SubmitChanges(glBatchTo, out tVerificationResultCollection);

                if (tSubmitChangesResult == TSubmitChangesResult.scrError)
                {
                    blnCriticalErrors = true;
                }

                if (tSubmitChangesResult == TSubmitChangesResult.scrInfoNeeded)
                {
                    if (!blnIsInInfoMode)
                    {
                        blnCriticalErrors = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Status values for error messages ...
    /// </summary>
    public enum TYearEndErrorStatus
    {
        /// <summary>
        /// The Leger is locked ...
        /// </summary>
        PEYM_01,
        /// <summary>
        /// Openpetra is not in the modus to run a year end procedure.
        /// This is only allowed if you have run the last month end period of the year
        /// </summary>
        PEYM_02,
        /// <summary>
        /// No income account in this ledger
        /// </summary>
        PEYM_03,

        /// <summary>
        /// No expense account in this ledger
        /// </summary>
        PEYM_04,

        /// <summary>
        /// No ICH account in this ledger
        /// </summary>
        PEYM_05,

        PEYM_06,
        PEYM_07
    }

    /// <summary>
    /// This is the list of status values of a_ledger.a_year_end_process_status_i which has been
    /// copied from petra. The status begins by counting from RESET_Status up to LEDGER_UPDATED
    /// and each higher level status includes the lower level ones.
    /// </summary>
    public enum TYearEndProcessStatus
    {
        /// <summary>
        /// Value the status counter is initialized with
        /// </summary>
        RESET_STATUS = 0,

        GIFT_CLOSED_OUT = 1,
        ACCOUNT_CLOSED_OUT = 2,
        GLMASTER_CLOSED_OUT = 3,
        BUDGET_CLOSED_OUT = 4,
        PERIODS_UPDATED = 7,
        SET_NEW_YEAR = 8,

        /// <summary>
        /// The leger is completely updated ...
        /// </summary>
        LEDGER_UPDATED = 10
    }
}