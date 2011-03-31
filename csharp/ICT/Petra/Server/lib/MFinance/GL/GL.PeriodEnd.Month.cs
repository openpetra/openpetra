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


namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    public partial class TPeriodEnd
    {
        [RequireModulePermission("FINANCE-1")]
        public bool TPeriodMonthEnd(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TMonthEnd().RunMonthEnd(ALedgerNum, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    public class TMonthEnd
    {
        GetLedgerInfo ledgerInfo;
        bool blnCriticalErrors = false;

        public bool RunMonthEndInfo(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            RunFirstChecks(ALedgerNum, out AVRCollection);
            return blnCriticalErrors;
        }

        public bool RunMonthEnd(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            RunFirstChecks(ALedgerNum, out AVRCollection);
            return blnCriticalErrors;
        }

        private void RunFirstChecks(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            AVRCollection = new TVerificationResultCollection();
            GetLedgerInfo ledgerInfo = new GetLedgerInfo(ALedgerNum);

            if (ledgerInfo.ProvisionalYearEndFlag)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("The year end processing for Ledger {0} needs to be run."),
                        ALedgerNum.ToString()),
                    "", "PYEF-01", TResultSeverity.Resv_Critical);
                blnCriticalErrors = true;
            }

            // Message is used two times ...
            string strErrorMessage1 = Catalog.GetString(
                "Some {1} batches for ledger {0} have not yet been posted.");

            GetBatchInfo getBatchInfo = new GetBatchInfo(ALedgerNum, ledgerInfo.CurrentPeriod);

            if (getBatchInfo.NumberOfBatches > 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(strErrorMessage1,
                        ALedgerNum.ToString(), getBatchInfo.BatchList),
                    "", "PYEF-02", TResultSeverity.Resv_Critical);
                blnCriticalErrors = true;
            }

            GetSuspenseAccountInfo getSuspenseAccountInfo = new GetSuspenseAccountInfo(ALedgerNum);

            if (getSuspenseAccountInfo.Rows != 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("Do you want to print and check suspense account details before? ({0} recordsets found)"),
                        ALedgerNum.ToString(), getSuspenseAccountInfo.Rows),
                    "", "PYEF-03", TResultSeverity.Resv_Critical);
                blnCriticalErrors = true;
            }

            GetAccountingPeriodInfo getAccountingPeriodInfo =
                new GetAccountingPeriodInfo(ALedgerNum, ledgerInfo.CurrentPeriod);
            GetUnpostedGiftInfo getUnpostedGiftInfo;

            if (getAccountingPeriodInfo.Rows != 1)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("Undefinded period"),
                        ALedgerNum.ToString(), getSuspenseAccountInfo.Rows),
                    "", "PYEF-04", TResultSeverity.Resv_Critical);
                blnCriticalErrors = true;
            }
            else
            {
                getUnpostedGiftInfo = new GetUnpostedGiftInfo(
                    ALedgerNum, getAccountingPeriodInfo.EffectiveDate);

                if (getUnpostedGiftInfo.Rows > 0)
                {
                    TVerificationResult tvr = new TVerificationResult(
                        Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                        String.Format(
                            strErrorMessage1,
                            ALedgerNum.ToString(), getUnpostedGiftInfo.Rows),
                        "", "PYEF-05", TResultSeverity.Resv_Critical);
                    blnCriticalErrors = true;
                }
            }

            if (ledgerInfo.CurrentPeriod == ledgerInfo.NumberOfAccountingPeriods) 
            {
            	// This means: The last accounting period of the year is running!
            	if (getSuspenseAccountInfo.Rows > 0)
            	{
            		ASuspenseAccountRow aSuspenseAccountRow;
            		decimal decAccountTotalSum = 0;
            		for (int i=0; i < getSuspenseAccountInfo.Rows; ++i)
            		{
            			aSuspenseAccountRow = getSuspenseAccountInfo.Row(i);
//            			Get_GLM_Info get_GLM_Info = new Get_GLM_Info(ALedgerNum,
//            			                                             aSuspenseAccountRow.SuspenseAccountCode,
//            			                                             ledgerInfo.CurrentPeriod);
            			                                             
            			//Get_GLMp_Info get_GLMp_Info = new Get_GLMp_Info(get_GLM_Info.Sequence, 
            			//                                                ledgerInfo.p);
            			//decAccountTotalSum += get_GLMp_Info.ActualBase;
            		} 
            	}
            }

//        lv_account_total_n = 0.
//        FOR EACH a_general_ledger_master
//            WHERE a_general_ledger_master.a_ledger_number_i = pv_ledger_number_i
//            AND a_suspense_account.a_suspense_account_code_c = a_general_ledger_master.a_account_code_c
//            AND a_general_ledger_master.a_year_i = a_ledger.a_current_financial_year_i
//            ,FIRST a_general_ledger_master_period
//            WHERE a_general_ledger_master_period.a_glm_sequence_i = a_general_ledger_master.a_glm_sequence_i
//            AND a_general_ledger_master_period.a_period_number_i = a_ledger.a_current_period_i
//            NO-LOCK
//            :
//            lv_account_total_n = lv_account_total_n + a_general_ledger_master_period.a_actual_base_n.
//        END.
//
//        IF lv_account_total_n NE 0
//        THEN DO:
//             RUN s_errmsg.p ("GL0089":U,
//                         PROGRAM-NAME(1),
//                         a_suspense_account.a_suspense_account_code_c,
//                         "").
//             RETURN.
//        END.

        }
    }

    /// <summary>
    /// Routine to finde unposted gifts batches.
    /// </summary>
    public class GetUnpostedGiftInfo
    {
        DataTable dataTable;
        public GetUnpostedGiftInfo(int ALedgerNumber, DateTime ADateEffective)
        {
            //IF CAN-FIND (FIRST a_gift_batch WHERE
            //    a_gift_batch.a_ledger_number_i EQ pv_ledger_number_i AND
            //    a_gift_batch.a_gl_effective_date_d LE
            //        a_accounting_period.a_period_end_date_d AND
            //    a_gift_batch.a_batch_status_c EQ "Unposted":U) THEN DO:

            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ADateEffective;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[2].Value = MFinanceConstants.BATCH_UNPOSTED;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "SELECT * FROM PUB_" + AGiftBatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGiftBatchTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= ? ";
            strSQL += "AND " + AGiftBatchTable.GetBatchStatusDBName() + " = ? ";
            dataTable = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AAccountingPeriodTable.GetTableDBName(), transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// Results the number of unposted gift batches.
        /// </summary>
        public int Rows
        {
            get
            {
                return dataTable.Rows.Count;
            }
        }
    }

    /// <summary>
    /// Routine to read the a_suspense_account entries
    /// </summary>
    public class GetSuspenseAccountInfo
    {
        int ledgerNumber;
        ASuspenseAccountTable table;

        /// <summary>
        /// Constructor to define ...
        /// </summary>
        /// <param name="ALedgerNumber">the ledger Number</param>
        public GetSuspenseAccountInfo(int ALedgerNumber)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            ledgerNumber = ALedgerNumber;
            table = ASuspenseAccountAccess.LoadViaALedger(ALedgerNumber, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// In case of an error message you need the number of entries.
        /// </summary>
        public int Rows
        {
            get
            {
                return table.Rows.Count;
            }
        }
        
      
        public ASuspenseAccountRow Row(int index)
        {
        	return table[index];
        }
        
    }

    /// <summary>
    /// GetBatchInfo is a class to check for a list of batches in ledgerNum and actual period which are
    /// not posted or not cancelled, that means. This routine is looking for open tasks.
    /// </summary>
    public class GetBatchInfo
    {
        DataTable batches;

        /// <summary>
        /// The contructor gets the root information and this are
        /// </summary>
        /// <param name="ALedgerNumber">the ledger number</param>
        /// <param name="ABatchPeriod">the number of the period the revaluation shall be done</param>
        public GetBatchInfo(int ALedgerNumber, int ABatchPeriod)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ABatchPeriod;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = MFinanceConstants.BATCH_POSTED;
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[3].Value = MFinanceConstants.BATCH_CANCELLED;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "SELECT * FROM PUB_" + ABatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + ABatchTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + ABatchTable.GetBatchPeriodDBName() + " = ? ";
            strSQL += "AND " + ABatchTable.GetBatchStatusDBName() + " <> ? ";
            strSQL += "AND " + ABatchTable.GetBatchStatusDBName() + " <> ? ";
            batches = DBAccess.GDBAccessObj.SelectDT(
                strSQL, ABatchTable.GetTableDBName(), transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// For a first overview you can read the number of rows and rows = 0 means that the
        /// batching jobs are done.
        /// </summary>
        public int NumberOfBatches
        {
            get
            {
                return batches.Rows.Count;
            }
        }

        /// <summary>
        /// In case of an error you can create a string for the error message ...
        /// </summary>
        public string BatchList
        {
            get
            {
                string strList = " - ";

                if (NumberOfBatches != 0)
                {
                    strList = batches.Rows[0][ABatchTable.GetBatchNumberDBName()].ToString();

                    if (NumberOfBatches > 0)
                    {
                        for (int i = 1; i < NumberOfBatches; ++i)
                        {
                            strList += ", " +
                                       batches.Rows[i][ABatchTable.GetBatchNumberDBName()].ToString();
                        }
                    }

                    strList = "(" + strList + ")";
                }

                return strList;
            }
        }
    }
}