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
    public partial class TPeriodMonthConnector
    {
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodMonthEndInfo(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TMonthEnd().RunMonthEndInfo(ALedgerNum, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    public class TMonthEnd
    {
        TVerificationResultCollection verificationResults;
        GetLedgerInfo ledgerInfo;

        // Set this value true means: Continue the checks and report but never
        // calculate ...
        bool blnCriticalErrors = false;

        public bool RunMonthEndInfo(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            try
            {
                ledgerInfo = new GetLedgerInfo(ALedgerNum);
                RunFirstChecks();
                AVRCollection = verificationResults;
                return blnCriticalErrors;
            }
            catch (TerminateException)
            {
                AVRCollection = verificationResults;
                return true;
            }
        }

        public bool RunMonthEnd(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            try
            {
                ledgerInfo = new GetLedgerInfo(ALedgerNum);
                RunFirstChecks();
                AVRCollection = verificationResults;

                if (blnCriticalErrors)
                {
                    return true;
                }
            }
            catch (TerminateException)
            {
                AVRCollection = verificationResults;
                return true;
            }

            if (verificationResults.Count != 0)
            {
                for (int i = 0; i < verificationResults.Count; ++i)
                {
                    if (verificationResults[i].ResultSeverity == TResultSeverity.Resv_Critical)
                    {
                        return false;
                    }
                }
            }

            try
            {
                RunAndAccountAdminFees();
                CarryForward();
                return false;
            }
            catch (TerminateException)
            {
                return true;
            }
        }

        private void RunFirstChecks()
        {
            verificationResults = new TVerificationResultCollection();

            if (ledgerInfo.ProvisionalYearEndFlag)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("The year end processing for Ledger {0} needs to be run."),
                        ledgerInfo.LedgerNumber.ToString()),
                    "", "PYEF-01", TResultSeverity.Resv_Critical);
                verificationResults.Add(tvr);
                blnCriticalErrors = true;
            }

            // Message is used two times ...
            string strErrorMessage1 = Catalog.GetString(
                "Some {1} batches for ledger {0} have not yet been posted.");

            GetBatchInfo getBatchInfo = new GetBatchInfo(ledgerInfo.LedgerNumber, ledgerInfo.CurrentPeriod);

            if (getBatchInfo.NumberOfBatches > 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(strErrorMessage1,
                        ledgerInfo.LedgerNumber.ToString(), getBatchInfo.BatchList),
                    "", "PYEF-02", TResultSeverity.Resv_Critical);
                verificationResults.Add(tvr);
                blnCriticalErrors = true;
            }

            GetSuspenseAccountInfo getSuspenseAccountInfo = new GetSuspenseAccountInfo(ledgerInfo.LedgerNumber);

            if (getSuspenseAccountInfo.Rows != 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("Do you want to print and check suspense account details before? ({0} recordsets found)"),
                        ledgerInfo.LedgerNumber.ToString(), getSuspenseAccountInfo.Rows),
                    "", "PYEF-03", TResultSeverity.Resv_Status);
                verificationResults.Add(tvr);
            }

            GetAccountingPeriodInfo getAccountingPeriodInfo =
                new GetAccountingPeriodInfo(ledgerInfo.LedgerNumber, ledgerInfo.CurrentPeriod);
            GetUnpostedGiftInfo getUnpostedGiftInfo;

            if (getAccountingPeriodInfo.Rows != 1)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                    String.Format(
                        Catalog.GetString("Undefinded period"),
                        ledgerInfo.LedgerNumber.ToString(), getSuspenseAccountInfo.Rows),
                    "", "PYEF-04", TResultSeverity.Resv_Critical);
                throw new TerminateException();
            }
            else
            {
                getUnpostedGiftInfo = new GetUnpostedGiftInfo(
                    ledgerInfo.LedgerNumber, getAccountingPeriodInfo.PeriodEndDate);

                if (getUnpostedGiftInfo.Rows > 0)
                {
                    TVerificationResult tvr = new TVerificationResult(
                        Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                        String.Format(
                            strErrorMessage1,
                            ledgerInfo.LedgerNumber.ToString(), getUnpostedGiftInfo.Rows),
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
                    string strMessage = Catalog.GetString("Suspense account {0} has the balance value {1}, " +
                        "which is required to be zero.");

                    for (int i = 0; i < getSuspenseAccountInfo.Rows; ++i)
                    {
                        aSuspenseAccountRow = getSuspenseAccountInfo.Row(i);
                        Get_GLM_Info get_GLM_Info = new Get_GLM_Info(ledgerInfo.LedgerNumber,
                            aSuspenseAccountRow.SuspenseAccountCode,
                            ledgerInfo.CurrentFinancialYear);

                        Get_GLMp_Info get_GLMp_Info = new Get_GLMp_Info(get_GLM_Info.Sequence,
                            ledgerInfo.CurrentPeriod);
                        decAccountTotalSum += get_GLMp_Info.ActualBase;

                        if (get_GLMp_Info.ActualBase != 0)
                        {
                            TVerificationResult tvr = new TVerificationResult(
                                Catalog.GetString("ProvisionalYearEndFlag-Problem"),
                                String.Format(strMessage, ledgerInfo.LedgerNumber, get_GLMp_Info.ActualBase), "",
                                "GL.CAT.08", TResultSeverity.Resv_Critical);
                            blnCriticalErrors = true;
                        }
                    }
                }
            }
        }

        void RunAndAccountAdminFees()
        {
            // TODO: Admin Fees and ICH stewardship ...
            // CommonAccountingTool cat = new CommonAccountingTool(ledgerInfo, "Batch Description");
        }

        void CarryForward()
        {
            CreateNewAccountingPeriod();

            if (ledgerInfo.CurrentPeriod == ledgerInfo.NumberOfAccountingPeriods)
            {
                SetProvisionalYearEndFlag(true);
            }
            else
            {
                SetNewFwdPeriodValue(ledgerInfo.CurrentPeriod + 1);
            }

            new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber,
                TLedgerInitFlagEnum.Revaluation).Flag = false;
        }

        void SetProvisionalYearEndFlag(bool AFlagValue)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Binary);
            ParametersArray[0].Value = AFlagValue;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Binary);
            ParametersArray[1].Value = !AFlagValue;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ledgerInfo.LedgerNumber;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
            strSQL += "SET " + ALedgerTable.GetYearEndFlagDBName() + " = ? ";
            strSQL += ", " + ALedgerTable.GetProvisionalYearEndFlagDBName() + " = ? ";
            strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        void SetNewFwdPeriodValue(int ANewPeriodNum)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ANewPeriodNum;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ledgerInfo.LedgerNumber;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
            strSQL += "SET " + ALedgerTable.GetCurrentPeriodDBName() + " = ? ";
            strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        void CreateNewAccountingPeriod()
        {
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
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
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