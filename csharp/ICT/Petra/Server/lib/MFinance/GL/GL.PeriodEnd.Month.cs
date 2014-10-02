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
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Cacheable.WebConnectors;
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

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    /// <summary>
    /// Routines for running the period month end check.
    /// </summary>
    public partial class TPeriodIntervalConnector
    {
        /// <summary>
        /// Month end master routine ...
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AInfoMode"></param>
        /// <param name="AVerificationResults"></param>
        /// <returns>false if there's no problem</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodMonthEnd(
            int ALedgerNumber,
            bool AInfoMode,
            out TVerificationResultCollection AVerificationResults)
        {
            TLedgerInfo ledgerInfo = new TLedgerInfo(ALedgerNumber);

            bool NewTransaction;

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                bool res = new TMonthEnd().RunMonthEnd(ALedgerNumber, AInfoMode,
                    out AVerificationResults);

                if (!res && !AInfoMode)
                {
                    AAccountingPeriodTable PeriodTbl = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, ledgerInfo.CurrentPeriod, null);

                    if (PeriodTbl.Rows.Count > 0)
                    {
                        AVerificationResults.Add(
                            new TVerificationResult(
                                Catalog.GetString("Month End"),
                                String.Format(Catalog.GetString("The period {0} - {1} has been closed."),
                                    PeriodTbl[0].PeriodStartDate.ToShortDateString(), PeriodTbl[0].PeriodEndDate.ToShortDateString()),
                                TResultSeverity.Resv_Status));
                    }
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                return res;
            }
            catch (Exception e)
            {
                TLogging.Log("TPeriodIntervallConnector.TPeriodMonthEnd() throws " + e.ToString());
                AVerificationResults = new TVerificationResultCollection();
                AVerificationResults.Add(
                    new TVerificationResult(
                        Catalog.GetString("Month End"),
                        Catalog.GetString("Uncaught Exception: ") + e.Message,
                        TResultSeverity.Resv_Critical));

                DBAccess.GDBAccessObj.RollbackTransaction();

                return true;
            }
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Main Class to serve a
    /// Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodMonthEnd request ...
    /// </summary>
    public class TMonthEnd : TPeriodEndOperations
    {
        TLedgerInfo FledgerInfo;
        /// <summary>
        ///
        /// </summary>
        [NoRemoting]
        public delegate bool StewardshipCalculation(int ALedgerNumber,
            int APeriodNumber,
            out TVerificationResultCollection AVerificationResult);
        private static StewardshipCalculation FStewardshipCalculationDelegate;

        /// <summary>
        ///
        /// </summary>
        [NoRemoting]
        public static StewardshipCalculation StewardshipCalculationDelegate
        {
            get
            {
                return FStewardshipCalculationDelegate;
            }
            set
            {
                FStewardshipCalculationDelegate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carryForward"></param>
        public override void SetNextPeriod(TCarryForward carryForward)
        {
            if (carryForward.FledgerInfo.CurrentPeriod == carryForward.FledgerInfo.NumberOfAccountingPeriods)
            {
                // Set the YearEndFlag to Switch between the months...
                carryForward.SetProvisionalYearEndFlag(true);
            }
            else
            {
                // Conventional Month->Month Switch ...
                carryForward.SetNewFwdPeriodValue(carryForward.FledgerInfo.CurrentPeriod + 1);
            }
        }

        /// <summary>
        /// Main Entry point. The parameters are the same as in
        /// Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodMonthEnd
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AInfoMode"></param>
        /// <param name="AVRCollection"></param>
        /// <returns>false if it went OK</returns>
        public bool RunMonthEnd(int ALedgerNumber, bool AInfoMode,
            out TVerificationResultCollection AVRCollection)
        {
            FInfoMode = AInfoMode;
            FledgerInfo = new TLedgerInfo(ALedgerNumber);
            FverificationResults = new TVerificationResultCollection();

            if (AInfoMode)
            {
                AAccountingPeriodTable PeriodTbl = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, FledgerInfo.CurrentPeriod, null);

                if (PeriodTbl.Rows.Count > 0)
                {
                    FverificationResults.Add(
                        new TVerificationResult(
                            Catalog.GetString("Month End"),
                            String.Format(Catalog.GetString("Current period is {0} - {1}"),
                                PeriodTbl[0].PeriodStartDate.ToShortDateString(), PeriodTbl[0].PeriodEndDate.ToShortDateString()),
                            TResultSeverity.Resv_Status));
                }
            }

            TCarryForward carryForward = new TCarryForward(FledgerInfo, this);

            if (carryForward.GetPeriodType != TCarryForwardENum.Month)
            {
                // we want to run a month end, but the provisional year end flag has been set
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Year End is required!"),
                        Catalog.GetString("In this situation you cannot run a month end routine"), "",
                        TPeriodEndErrorAndStatusCodes.PEEC_03.ToString(),
                        TResultSeverity.Resv_Critical);
                FverificationResults.Add(tvt);
                FHasCriticalErrors = true;
            }

            RunPeriodEndCheck(new RunMonthEndChecks(FledgerInfo), FverificationResults);

            if (!AInfoMode)
            {
                TVerificationResultCollection IchVerificationReults;

                if (!StewardshipCalculationDelegate(ALedgerNumber, FledgerInfo.CurrentPeriod,
                        out IchVerificationReults))
                {
                    FHasCriticalErrors = true;
                }

                // Merge VerificationResults:
                FverificationResults.AddCollection(IchVerificationReults);
            }

            // RunPeriodEndSequence(new RunMonthlyAdminFees(), "Example");

            if (!FInfoMode)
            {
                if (!FHasCriticalErrors)
                {
                    carryForward.SetNextPeriod();
                    // refresh cached ledger table, so that the client will know the current period
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableFinanceTablesEnum.LedgerDetails.ToString());
                }
            }

            //
            // The 4GL code throws out these reports:
            //
            //     Admin fee calculations report.
            //     ICH stewardship report.
            //     "Trial Balance" with account details.
            //     HOSA for each foreign cost centre (ledger/fund).
            //     Income Statement/Profit & Loss
            //     Current Accounts Payable if interfaced.  M025
            //     AFO report.
            //     Executive Summary Report.
            //
            AVRCollection = FverificationResults;
            return FHasCriticalErrors;
        }
    }

    class RunMonthEndChecks : AbstractPeriodEndOperation
    {
        TLedgerInfo FledgerInfo;

        GetSuspenseAccountInfo getSuspenseAccountInfo = null;

        public RunMonthEndChecks(TLedgerInfo ALedgerInfo)
        {
            FledgerInfo = ALedgerInfo;
        }

        public override int GetJobSize()
        {
            return 0;
        }

        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new RunMonthEndChecks(FledgerInfo);
        }

        public override void RunOperation()
        {
            CheckIfRevaluationIsDone();
            CheckForUnpostedBatches();
            CheckForUnpostedGiftBatches();
            CheckForSuspenseAccountsZero();
            CheckForSuspenseAccounts();
        }

        private void CheckIfRevaluationIsDone()
        {
            // TODO: could also check for the balance in this month of the foreign currency account. if balance is zero, no revaluation is needed.
            string testForForeignKeyAccount =
                String.Format("SELECT COUNT(*) FROM PUB_a_account WHERE {0} = {1} and {2} = true",
                    AAccountTable.GetLedgerNumberDBName(),
                    FledgerInfo.LedgerNumber,
                    AAccountTable.GetForeignCurrencyFlagDBName());

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            if (Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(testForForeignKeyAccount, transaction)) == 0)
            {
                // no revaluation is needed
                return;
            }

            if (!(new TLedgerInitFlagHandler(FledgerInfo.LedgerNumber,
                      TLedgerInitFlagEnum.Revaluation).Flag))
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("Ledger revaluation"),
                    Catalog.GetString("Please run a ledger revaluation first."), "",
                    TPeriodEndErrorAndStatusCodes.PEEC_05.ToString(), TResultSeverity.Resv_Critical);
                // Error is critical but additional checks shall be done
                verificationResults.Add(tvr);
                FHasCriticalErrors = true;
            }
        }

        private void CheckForUnpostedBatches()
        {
            GetBatchInfo getBatchInfo = new GetBatchInfo(
                FledgerInfo.LedgerNumber, FledgerInfo.CurrentPeriod);

            if (getBatchInfo.NumberOfBatches > 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("Unposted Batches found"),
                    String.Format(Catalog.GetString(
                            "Please post or cancel the batches {0} first!"),
                        getBatchInfo.ToString()),
                    "", TPeriodEndErrorAndStatusCodes.PEEC_06.ToString(), TResultSeverity.Resv_Critical);
                verificationResults.Add(tvr);
                FHasCriticalErrors = true;
            }
        }

        private void CheckForSuspenseAccounts()
        {
            if (getSuspenseAccountInfo == null)
            {
                getSuspenseAccountInfo =
                    new GetSuspenseAccountInfo(FledgerInfo.LedgerNumber);
            }

            if (getSuspenseAccountInfo.RowCount != 0)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("Suspense Accounts found"),
                    String.Format(
                        Catalog.GetString(
                            "Have you checked the details of suspense account {0}?"),
                        getSuspenseAccountInfo.ToString()),
                    "", TPeriodEndErrorAndStatusCodes.PEEC_07.ToString(), TResultSeverity.Resv_Status);
                verificationResults.Add(tvr);
            }
        }

        private void CheckForUnpostedGiftBatches()
        {
            TAccountPeriodInfo getAccountingPeriodInfo =
                new TAccountPeriodInfo(FledgerInfo.LedgerNumber, FledgerInfo.CurrentPeriod);
            GetUnpostedGiftInfo getUnpostedGiftInfo = new GetUnpostedGiftInfo(
                FledgerInfo.LedgerNumber, getAccountingPeriodInfo.PeriodEndDate);

            if (getUnpostedGiftInfo.HasRows)
            {
                TVerificationResult tvr = new TVerificationResult(
                    Catalog.GetString("Unposted Gift Batches found"),
                    String.Format(
                        "Please post or cancel the gift batches {0} first!",
                        getUnpostedGiftInfo.ToString()),
                    "", TPeriodEndErrorAndStatusCodes.PEEC_08.ToString(), TResultSeverity.Resv_Critical);
                verificationResults.Add(tvr);
                FHasCriticalErrors = true;
            }
        }

        private void CheckForSuspenseAccountsZero()
        {
            if (FledgerInfo.CurrentPeriod == FledgerInfo.NumberOfAccountingPeriods)
            {
                // This means: The last accounting period of the year is running!

                if (getSuspenseAccountInfo == null)
                {
                    getSuspenseAccountInfo =
                        new GetSuspenseAccountInfo(FledgerInfo.LedgerNumber);
                }

                if (getSuspenseAccountInfo.RowCount > 0)
                {
                    ASuspenseAccountRow aSuspenseAccountRow;

                    for (int i = 0; i < getSuspenseAccountInfo.RowCount; ++i)
                    {
                        aSuspenseAccountRow = getSuspenseAccountInfo.Row(i);
                        TGet_GLM_Info get_GLM_Info = new TGet_GLM_Info(FledgerInfo.LedgerNumber,
                            aSuspenseAccountRow.SuspenseAccountCode,
                            FledgerInfo.CurrentFinancialYear);

                        TGlmpInfo get_GLMp_Info = new TGlmpInfo(
                            -1, -1,
                            get_GLM_Info.Sequence,
                            FledgerInfo.CurrentPeriod);

                        if (get_GLMp_Info.RowExists)
                        {
                            TVerificationResult tvr = new TVerificationResult(
                                Catalog.GetString("Non Zero Suspense Account found"),
                                String.Format(Catalog.GetString("Suspense account {0} has the balance value {1}. It is required to be zero."),
                                    getSuspenseAccountInfo.ToString(),
                                    get_GLMp_Info.ActualBase), "",
                                TPeriodEndErrorAndStatusCodes.PEEC_07.ToString(), TResultSeverity.Resv_Critical);
                            verificationResults.Add(tvr);

                            FHasCriticalErrors = true;
                            verificationResults.Add(tvr);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Example ....
    /// </summary>
    class RunMonthlyAdminFees : AbstractPeriodEndOperation
    {
        public override int GetJobSize()
        {
            return 0;
        }

        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            // TODO: Some Code
            return new RunMonthlyAdminFees();
        }

        public override void RunOperation()
        {
            // TODO: Some Code
        }
    }

    /// <summary>
    /// Routine to find unposted gifts batches.
    /// </summary>
    public class GetUnpostedGiftInfo
    {
        DataTable FDataTable;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Boolean HasRows
        {
            get
            {
                return FDataTable.Rows.Count > 0;
            }
        }

        /// <summary>
        /// Direct access to the unposted gifts
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateEndOfPeriod"></param>
        public GetUnpostedGiftInfo(int ALedgerNumber, DateTime ADateEndOfPeriod)
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
            ParametersArray[1].Value = ADateEndOfPeriod;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = MFinanceConstants.BATCH_UNPOSTED;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            string strSQL = "SELECT * FROM PUB_" + AGiftBatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGiftBatchTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGiftBatchTable.GetGlEffectiveDateDBName() + " <= ? ";
            strSQL += "AND " + AGiftBatchTable.GetBatchStatusDBName() + " = ? ";
            FDataTable = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AAccountingPeriodTable.GetTableDBName(), transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        /// <summary>
        /// Creates a comma separated list of the batch numbers
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string strH;

            if (FDataTable.Rows.Count == 0)
            {
                strH = "-";
            }
            else
            {
                int ih = Convert.ToInt32(FDataTable.Rows[0][AGiftBatchTable.GetBatchNumberDBName()]);
                strH = ih.ToString();

                for (int i = 1; i < FDataTable.Rows.Count; ++i)
                {
                    strH += (", " + Convert.ToString(FDataTable.Rows[i][AGiftBatchTable.GetBatchNumberDBName()]));
                }
            }

            return "(" + strH + ")";
        }
    }

    /// <summary>
    /// Routine to read the a_suspense_account entries
    /// </summary>
    public class GetSuspenseAccountInfo
    {
        ASuspenseAccountTable table;

        /// <summary>
        /// Constructor to define ...
        /// </summary>
        /// <param name="ALedgerNumber">the ledger Number</param>
        public GetSuspenseAccountInfo(int ALedgerNumber)
        {
            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            table = ASuspenseAccountAccess.LoadViaALedger(ALedgerNumber, transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        /// <summary>
        /// In case of an error message you need the number of entries.
        /// </summary>
        public int RowCount
        {
            get
            {
                return table.Rows.Count;
            }
        }

        /// <summary>
        /// Gives direct access to the selected suspense account row ...
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ASuspenseAccountRow Row(int index)
        {
            return table[index];
        }

        /// <summary>
        /// Produces a comma separated list of suspense account codes
        /// for use in the status message(s).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string strH;

            if (RowCount == 0)
            {
                strH = "-";
            }
            else
            {
                strH = table[0].SuspenseAccountCode;

                if (RowCount > 1)
                {
                    for (int i = 1; i < RowCount; ++i)
                    {
                        strH += ", " + table[i].SuspenseAccountCode;
                    }
                }
            }

            return "(" + strH + ")";
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

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            string strSQL = "SELECT * FROM PUB_" + ABatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + ABatchTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + ABatchTable.GetBatchPeriodDBName() + " = ? ";
            strSQL += "AND " + ABatchTable.GetBatchStatusDBName() + " <> ? ";
            strSQL += "AND " + ABatchTable.GetBatchStatusDBName() + " <> ? ";
            batches = DBAccess.GDBAccessObj.SelectDT(
                strSQL, ABatchTable.GetTableDBName(), transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
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
        public override string ToString()
        {
            //get
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