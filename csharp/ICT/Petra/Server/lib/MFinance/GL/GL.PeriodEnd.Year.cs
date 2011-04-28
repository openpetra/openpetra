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
    public class TYearEnd
    {
    	
    	TSessionObject tSessionObject;

    	bool blnCriticalErrors;
        bool blnIsInInfoMode;
        TVerificationResultCollection verificationResults;
        TLedgerInfo tHandleLedgerInfo;
        TLedgerLock ledgerLock;
        List <String>accountList;
        THandleGlmInfo tHandleGlmInfo;
        THandleGlmpInfo tHandleGlmpInfo;
        THandleAccountInfo tHandleAccountInfo;
        THandleCostCenterInfo tHandleCostCenterInfo;

        TCommonAccountingTool tCommonAccountingTool;


        public bool RunYearEnd(int ALedgerNum, bool AInfoMode,
            out TVerificationResultCollection AVRCollection)
        {


        	blnIsInInfoMode = AInfoMode;
            verificationResults = new TVerificationResultCollection();
            try
            {
                ledgerLock = new TLedgerLock(ALedgerNum);
                YearEndMain(ALedgerNum);
                AVRCollection = verificationResults;
                ledgerLock.UnLock();
                return blnCriticalErrors;
            }
            catch (TerminateException terminate)
            {
                AVRCollection = terminate.ResultCollection();
                ledgerLock.UnLock();
                return true;
            }
        }

        private void YearEndMain(int ALedgerNum)
        {
            tHandleLedgerInfo = new TLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();

            tHandleLedgerInfo = new TLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();
            CheckLedger();
            
            try 
            {
                CloseGLMaster();
                UpdatePeriods();
                SetNewYear();
                Finish();
            } catch (TerminateException ex)
            {
            	throw ex;
            }
        }

        private void CheckLedger()
        {
            if (!ledgerLock.IsLocked)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Leger is locked ..."),
                        String.Format(Catalog.GetString("Ledger is locked by the user {0}"),
                            ledgerLock.LockInfo()), "",
                        TYearEndErrorStatus.PEYM_01.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            if (!tHandleLedgerInfo.ProvisionalYearEndFlag)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("YearEndFlag is set "),
                        Catalog.GetString("In this situation you cannot run a year end routine"), "",
                        TYearEndErrorStatus.PEYM_02.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }
        }


        private void CloseGLMaster()
        {
            tHandleAccountInfo = new THandleAccountInfo(tHandleLedgerInfo);
            bool blnIncomeFound = false;
            bool blnExpenseFound = false;

            tHandleAccountInfo.Reset();
            accountList = new List <String>();

            while (tHandleAccountInfo.MoveNext())
            {
                if (tHandleAccountInfo.PostingStatus)
                {
                    if (tHandleAccountInfo.AccountType.Equals(TAccountTypeEnum.Income.ToString()))
                    {
                        accountList.Add(tHandleAccountInfo.AccountCode);
                        blnIncomeFound = true;
                    }

                    if (tHandleAccountInfo.AccountType.Equals(TAccountTypeEnum.Expense.ToString()))
                    {
                        accountList.Add(tHandleAccountInfo.AccountCode);
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

            tHandleAccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);

            if (tHandleAccountInfo.IsValid)
            {
                accountList.Add(tHandleAccountInfo.AccountCode);
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

            tHandleCostCenterInfo = new THandleCostCenterInfo(tHandleLedgerInfo.LedgerNumber);


            Create_Reallocation();
            new TGlmCopy(tHandleLedgerInfo.LedgerNumber, tHandleLedgerInfo.CurrentFinancialYear).ToNextYear();
        }


        private void UpdatePeriods()
        {
        }

        private void SetNewYear()
        {
        	/// <summary>
        	/// Accounting period - datum um 1 erhöhen
        	/// </summary>
        }

        private void Finish()
        {
        }

        private void Create_Reallocation()
        {
            tCommonAccountingTool =
                new TCommonAccountingTool(tHandleLedgerInfo,
                    Catalog.GetString("Financial year end processing"));
            tHandleGlmpInfo = new THandleGlmpInfo();

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
                    tHandleGlmInfo = new THandleGlmInfo(tHandleLedgerInfo.LedgerNumber,
                        tHandleLedgerInfo.CurrentFinancialYear,
                        strAccountCode);

                    // Loop with all cost centres
                    tHandleGlmInfo.Reset();

                    while (tHandleGlmInfo.MoveNext())
                    {
                        tHandleCostCenterInfo.SetCostCenterRow(tHandleGlmInfo.CostCentreCode);

                        if (tHandleCostCenterInfo.IsValid)
                        {
                            if (tHandleCostCenterInfo.PostingCostCentreFlag)
                            {
                                if (tHandleGlmpInfo.SetToRow(
                                        tHandleGlmInfo.GlmSequence,
                                        tHandleLedgerInfo.NumberOfAccountingPeriods))
                                {
                                    if (tHandleGlmpInfo.ActualBase != 0)
                                    {
                                    	if (tHandleGlmInfo.YtdActualBase != 0)
                                    	{
                                    		Create_Reallocation2(strAccountCode,
                                    		                     tHandleGlmInfo.CostCentreCode);
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


        private void Create_Reallocation2(String AAccountCode, string ACostCentreCode)
        {
            bool blnDebitCredit;

            tHandleAccountInfo.AccountCode = tHandleGlmInfo.AccountCode;

            blnDebitCredit = tHandleAccountInfo.DebitCreditIndicator;

            bool blnCarryForward = false;
            string strCostCentreTo;
            string strAccountTo;

            string strCCAccoutType = tHandleAccountInfo.SetCarryForwardAccount();

            if (tHandleAccountInfo.IsValid)
            {
                strAccountTo = AAccountCode;

                if (strCCAccoutType.Equals("SAMECC"))
                {
                    strCostCentreTo = tHandleGlmInfo.CostCentreCode;
                    blnCarryForward = true;
                }
                else
                {
                    strCostCentreTo = GetStandardCostCentre();
                }
            }
            else
            {
                tHandleAccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.EARNINGS_BF_ACCT);
                strAccountTo = tHandleAccountInfo.AccountCode;
                strCostCentreTo = GetStandardCostCentre();
            }

            if (tHandleLedgerInfo.IltAccountFlag)
            {
                tHandleAccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
                strAccountTo = tHandleAccountInfo.AccountCode;
            }

            if (tHandleLedgerInfo.BranchProcessing)
            {
                strAccountTo = AAccountCode;
            }

            string strYearEnd = Catalog.GetString("YEAR-END");
            string strNarrativeMessage = Catalog.GetString("Year end re-allocation to {0}:{1}");
            string strBuildNarrative = String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode);
            
            System.Diagnostics.Debug.WriteLine("AddTransaction: " +
                                               AAccountCode + ":" + ACostCentreCode +
                                               " : " + blnDebitCredit.ToString());

            tCommonAccountingTool.AddBaseCurrencyTransaction(
                AAccountCode, ACostCentreCode, strBuildNarrative,
                strYearEnd, !blnDebitCredit, Math.Abs(tHandleGlmInfo.YtdActualBase));

            
            strBuildNarrative = String.Format(strNarrativeMessage, ACostCentreCode, AAccountCode);
            tCommonAccountingTool.AddBaseCurrencyTransaction(
            	strAccountTo, strCostCentreTo, strBuildNarrative,
            	strYearEnd, blnDebitCredit, Math.Abs(tHandleGlmInfo.YtdActualBase));
        }

        private int GetStatusCode(TYearEndProcessStatus status)
        {
            //Type typeHelp = typeof(TYearEndProcessStatus);

            return int.Parse(
                Enum.Format(typeof(TYearEndProcessStatus),
                    Enum.Parse(typeof(TYearEndProcessStatus),
                        status.ToString()), "d"));
        }

        private string GetStandardCostCentre()
        {
            return tHandleLedgerInfo.LedgerNumber.ToString() + "00";
        }
    }

    public class TGlmCopy
    {
    	
    	GLBatchTDS glBatchFrom = null;
    	GLBatchTDS glBatchTo = null; 
    	AGeneralLedgerMasterRow generalLedgerMasterRowFrom = null;
    	AGeneralLedgerMasterRow generalLedgerMasterRowTo = null;
    	
    	int intNextYear;

    	public TGlmCopy(int ALedgerNumber, int AYear)
    	{
    		intNextYear = ++AYear;
    		glBatchFrom = LoadTable(ALedgerNumber, AYear);
    		glBatchTo = LoadTable(ALedgerNumber, ++AYear);
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
            
            GLBatchTDS gLBatchTDSAccess = new GLBatchTDS();
            
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
            	DBAccess.GDBAccessObj.Select(gLBatchTDSAccess,
            		strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);
            	DBAccess.GDBAccessObj.CommitTransaction();
            	return gLBatchTDSAccess;
            } catch (Exception exception)
            {
            	DBAccess.GDBAccessObj.RollbackTransaction();
            	throw exception;
            }
    	}
    	
    	public void ToNextYear()
    	{
    		if (glBatchFrom.AGeneralLedgerMaster.Rows.Count > 0)
    		{
    			for (int i=0; i < glBatchFrom.AGeneralLedgerMaster.Rows.Count; ++i)
    			{
    				generalLedgerMasterRowFrom = 
    					(AGeneralLedgerMasterRow)glBatchFrom.AGeneralLedgerMaster[i];
    				generalLedgerMasterRowTo = null;
    				for (int j=0; j < glBatchTo.AGeneralLedgerMaster.Rows.Count; ++j)
    				{
    					generalLedgerMasterRowTo = 
    						(AGeneralLedgerMasterRow)glBatchTo.AGeneralLedgerMaster[j];
    					if ((generalLedgerMasterRowFrom.AccountCode == generalLedgerMasterRowTo.AccountCode) &&    					    
    					    (generalLedgerMasterRowFrom.CostCentreCode == generalLedgerMasterRowTo.CostCentreCode))
    					{
    						break;
    					} else
    					{
    						generalLedgerMasterRowTo = null;
    					}
    				}
    				if (generalLedgerMasterRowTo == null)
    				{
    					generalLedgerMasterRowTo = 
    						(AGeneralLedgerMasterRow)glBatchTo.AGeneralLedgerMaster.NewRow();
    					glBatchTo.AGeneralLedgerMaster.Rows.Add(generalLedgerMasterRowTo);
    				}
    				generalLedgerMasterRowTo.LedgerNumber = generalLedgerMasterRowFrom.LedgerNumber;
    				generalLedgerMasterRowTo.Year = intNextYear;
    				generalLedgerMasterRowTo.AccountCode = generalLedgerMasterRowFrom.AccountCode;
    				generalLedgerMasterRowTo.CostCentreCode = generalLedgerMasterRowFrom.CostCentreCode;
    				generalLedgerMasterRowTo.YtdActualBase = generalLedgerMasterRowFrom.YtdActualBase;
    			}
    		}
    		TVerificationResultCollection tVerificationResultCollection;
    		GLBatchTDSAccess.SubmitChanges(glBatchTo,out tVerificationResultCollection);
    	}
    }

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
        PEYM_05
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