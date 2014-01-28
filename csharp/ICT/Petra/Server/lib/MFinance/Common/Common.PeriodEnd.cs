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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// If a procedure is defined which shall be assigned inside a specific perodic process you have to use this
    /// class the handle the operation itself and the AbstractPeriodEndOperation class to handle the internal
    /// parts of the operation. <br />
    /// For example the class TMonthEnd and TYearEnd inherits TPeriodEndOperations.<br />
    /// </summary>
    public class TPeriodEndOperations
    {
        /// <summary>
        /// If the user invokes a specific year end command, he automatically starts a server request
        /// only to make some checks and to gather some specific information. Handling this parameter
        /// enables to gather this information in the same routine which is used for the calculations.
        /// So both processes are automatically synchronized. <br />
        /// So do not run any excecutive code if the system is in the info mode.
        /// </summary>
        protected bool FInfoMode;

        /// <summary>
        /// If I run into a critial error, (either in info mode or in executive
        /// mode) I can't run any executive code, but it may be useful to gather more information
        /// and so the process is not terminated completely.
        /// </summary>
        protected bool FHasCriticalErrors;

        /// <summary>
        /// This is the standard VerificationResultCollection for the info and the error messages.
        /// </summary>
        protected TVerificationResultCollection verificationResults;


        /// <summary>
        /// This is for all info only routines that means JobSize has no definition
        /// </summary>
        protected void RunPeriodEndCheck(AbstractPeriodEndOperation Apeo, TVerificationResultCollection AVerificationResults)
        {
            verificationResults = AVerificationResults;
            Apeo.VerificationResultCollection = AVerificationResults;
            Apeo.IsInInfoMode = FInfoMode;
            Apeo.RunEndOfPeriodOperation();

            if (Apeo.HasCriticalErrors)
            {
                FHasCriticalErrors = true;
            }
        }

        /// <summary>
        /// Standard routine to execute the period end of each AbstractPeriodEndOperation correctly
        /// </summary>
        /// <param name="Apeo"></param>
        /// <param name="AOperationName"></param>
        protected void RunPeriodEndSequence(AbstractPeriodEndOperation Apeo, string AOperationName)
        {
            Apeo.IsInInfoMode = FInfoMode;
            Apeo.VerificationResultCollection = verificationResults;

            if (Apeo.JobSize == 0)
            {
                // Non Critical Problem but the user shall be informed ...
                String strTitle = Catalog.GetString("Periodic end routine hint");
                String strMessage = Catalog.GetString("There is nothing to be done for the module: [{0}]");
                strMessage = String.Format(strMessage, AOperationName);
                TVerificationResult tvt =
                    new TVerificationResult(strTitle, strMessage, "",
                        TPeriodEndErrorAndStatusCodes.PEEC_01.ToString(),
                        TResultSeverity.Resv_Noncritical);
                verificationResults.Add(tvt);
            }
            else if (FInfoMode == false)
            {
                // now we actually run the operation
                Apeo.RunEndOfPeriodOperation();
                AbstractPeriodEndOperation newApeo = Apeo.GetActualizedClone();
                newApeo.IsInInfoMode = true;
                newApeo.VerificationResultCollection = verificationResults;

                if (newApeo.JobSize != 0)
                {
                    // Critical Problem because there should be nothing left to do.
                    String strTitle = Catalog.GetString("Problem occurs in module [{0}]");
                    strTitle = String.Format(strTitle, AOperationName);
                    String strMessage = Catalog.GetString(
                        "The operation has left {0} elements which are not transformed!");
                    strMessage = String.Format(strMessage, newApeo.JobSize.ToString());
                    TVerificationResult tvt =
                        new TVerificationResult(strTitle, strMessage, "",
                            TPeriodEndErrorAndStatusCodes.PEEC_02.ToString(),
                            TResultSeverity.Resv_Critical);
                    verificationResults.Add(tvt);
                    FHasCriticalErrors = true;
                }

                if (newApeo.HasCriticalErrors)
                {
                    FHasCriticalErrors = true;
                }
            }

            if (Apeo.HasCriticalErrors)
            {
                FHasCriticalErrors = true;
            }
        }
    }


    /// <summary>
    /// The period end classes inherit and complete this abstract class. The constructor of the
    /// inhereting class handles all parameters which are necessary for the RunEndOfPeriodOperation and
    /// RunEndOfPeriodOperation handles the database operations.
    /// </summary>
    public abstract class AbstractPeriodEndOperation
    {
        /// <summary>
        /// This is the standard VerificationResultCollection for the info and the error messages.
        /// </summary>
        protected TVerificationResultCollection verificationResults = null;

        /// <summary>
        /// See TPeriodEndOperations
        /// </summary>
        protected bool FInfoMode = true;

        /// <summary>
        /// See TPeriodEndOperations
        /// </summary>
        protected bool FHasCriticalErrors = false;


        /// <summary>
        ///
        /// </summary>
        protected int intCountJobs;

        /// <summary>
        /// JobSize returns the number of database records which are affected in this operation. Be sure
        /// not only to find the databases which are to be changed but also not to find the records which
        /// are already changed.
        /// Or: Be sure that JobSize is zero after RunEndOfPeriodOperation has been done sucessfully.
        /// </summary>
        public abstract int JobSize {
            get;
        }

        /// <summary>
        /// The specific operation is done. Be sure to handle blnIsInInfoMode and blnCriticalErrors correctly
        /// </summary>
        public abstract void RunEndOfPeriodOperation();

        /// <summary>
        /// Method to create a duplicate based on the actualized database value(s)
        /// </summary>
        /// <returns></returns>
        public abstract AbstractPeriodEndOperation GetActualizedClone();


        /// <summary>
        /// Summarizes the values of blnCriticalErrors and blnIsInInfoMode to the decision if an
        /// executable code shall be done or not.
        /// </summary>
        public bool DoExecuteableCode
        {
            get
            {
                return !(FHasCriticalErrors | FInfoMode);
            }
        }

        /// <summary>
        /// Set-Property to set the common value of the VerificationResultCollection
        ///  (Set by TPeriodEndOperations.RunYearEndSequence)
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            set
            {
                verificationResults = value;
            }
        }

        /// <summary>
        /// Property to set the correct info-mode (Set by TPeriodEndOperations.RunYearEndSequence)
        /// </summary>
        public bool IsInInfoMode
        {
            set
            {
                FInfoMode = value;
            }
        }

        /// <summary>
        /// Property to read if the process could be done without critical errors.
        ///  (Used by TPeriodEndOperations.RunYearEndSequence)
        /// </summary>
        public bool HasCriticalErrors
        {
            get
            {
                return FHasCriticalErrors;
            }
        }
    }

    /// <summary>
    /// ENum-List of the accounting stati of a ledger
    /// </summary>
    public enum TCarryForwardENum
    {
        /// <summary></summary>
        Month,
        /// <summary></summary>
        Year
    }

    /// <summary>
    /// Central object to switch to the next accounting period
    /// </summary>
    public class TCarryForward
    {
        TLedgerInfo ledgerInfo;

        /// <summary>
        /// The routine requires a TLedgerInfo object ...
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public TCarryForward(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
        }

        /// <summary>
        /// Sets the ledger to the next accounting period ...
        /// </summary>
        public void SetNextPeriod()
        {
            if (ledgerInfo.ProvisionalYearEndFlag)
            {
                // Set to the first month of the "next year".
                SetProvisionalYearEndFlag(false);
                SetNewFwdPeriodValue(1);
                ledgerInfo.CurrentFinancialYear = ledgerInfo.CurrentFinancialYear + 1;
                TAccountPeriodToNewYear accountPeriod = new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber);
                accountPeriod.IsInInfoMode = false;
                accountPeriod.RunEndOfPeriodOperation();
            }
            else if (ledgerInfo.CurrentPeriod == ledgerInfo.NumberOfAccountingPeriods)
            {
                // Set the YearEndFlag to "Switch between the months ...
                SetProvisionalYearEndFlag(true);
            }
            else
            {
                // Conventional Month->Month Switch ...
                SetNewFwdPeriodValue(ledgerInfo.CurrentPeriod + 1);
            }

            new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber,
                TLedgerInitFlagEnum.Revaluation).Flag = false;
        }

        /// <summary>
        /// Gets the type of the actual accouting period (see TCarryForwardENum).
        /// </summary>
        public TCarryForwardENum GetPeriodType
        {
            get
            {
                if (ledgerInfo.ProvisionalYearEndFlag)
                {
                    return TCarryForwardENum.Year;
                }
                else
                {
                    return TCarryForwardENum.Month;
                }
            }
        }

        void SetProvisionalYearEndFlag(bool AFlagValue)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Bit);
            ParametersArray[0].Value = !AFlagValue;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Bit);
            ParametersArray[1].Value = AFlagValue;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ledgerInfo.LedgerNumber;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
            strSQL += "SET " + ALedgerTable.GetYearEndFlagDBName() + " = ? ";
            strSQL += ", " + ALedgerTable.GetProvisionalYearEndFlagDBName() + " = ? ";
            strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        void SetNewFwdPeriodValue(int ANewPeriodNum)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ANewPeriodNum;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ledgerInfo.LedgerNumber;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
            strSQL += "SET " + ALedgerTable.GetCurrentPeriodDBName() + " = ? ";
            strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }
    }
    /// <summary>
    /// This object handles the transformation of the accounting interval parameters into the
    /// next year
    /// </summary>
    public class TAccountPeriodToNewYear : AbstractPeriodEndOperation
    {
        int FLedgerNumber;
        AAccountingPeriodTable FaccountingPeriodTable = null;

        /// <summary>
        /// Constructor to define and load the complete table defined by ledger number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TAccountPeriodToNewYear(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
            LoadData();
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
        public override int JobSize {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the year from the first data base record in the table (PeriodStartDate).
        /// </summary>
        public int ActualYear
        {
            get
            {
                AAccountingPeriodRow accountingPeriodRow = FaccountingPeriodTable[0];
                return accountingPeriodRow.PeriodStartDate.Year;
            }
        }

        private void LoadData()
        {
            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                FaccountingPeriodTable = AAccountingPeriodAccess.LoadViaALedger(FLedgerNumber, transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// The years are updated ...
        /// </summary>
        override public void RunEndOfPeriodOperation()
        {
            bool NewTransaction;
            TDBTransaction WriteTransaction;

            if (DoExecuteableCode)
            {
                for (int i = 0; i < FaccountingPeriodTable.Rows.Count; ++i)
                {
                    AAccountingPeriodRow accountingPeriodRow = FaccountingPeriodTable[i];

                    accountingPeriodRow.PeriodStartDate =
                        accountingPeriodRow.PeriodStartDate.AddDays(1).AddYears(1).AddDays(-1);
                    accountingPeriodRow.PeriodEndDate =
                        accountingPeriodRow.PeriodEndDate.AddDays(1).AddYears(1).AddDays(-1);
                }

                WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                try
                {
                    AAccountingPeriodAccess.SubmitChanges(FaccountingPeriodTable, WriteTransaction);

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured during running the End of Period operation:" + Environment.NewLine + Exc.ToString());

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

                    throw;
                }
            }
        }
    }

    /// <summary>
    /// This Object read all glm year end records of the actual year
    /// and creates the start record for the next year
    /// </summary>
    public class TGlmNewYearInit : AbstractPeriodEndOperation
    {
        GLPostingTDS PostingFromDS = null;
        GLPostingTDS PostingToDS = null;
        AGeneralLedgerMasterRow generalLedgerMasterRowFrom = null;
        AGeneralLedgerMasterRow generalLedgerMasterRowTo = null;

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
            intNextYear = intThisYear + 1;
            intLedgerNumber = ALedgerNumber;
            PostingFromDS = LoadTable(ALedgerNumber, AYear);
            PostingToDS = LoadTable(ALedgerNumber, ++AYear);
        }

        /// <summary>
        ///
        /// </summary>
        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TGlmNewYearInit(intLedgerNumber, intThisYear);
        }

        /// <summary>
        ///
        /// </summary>
        public override int JobSize {
            get
            {
                bool blnOldInfoMode = FInfoMode;
                FInfoMode = true;
                RunEndOfPeriodOperation();
                FInfoMode = blnOldInfoMode;
                return intEntryCount;
            }
        }

        private GLPostingTDS LoadTable(int ALedgerNumber, int AYear)
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

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                DBAccess.GDBAccessObj.Select(PostingDS,
                    strSQL, AGeneralLedgerMasterTable.GetTableName(), transaction, ParametersArray);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                return PostingDS;
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Next-Year records will be created.
        /// </summary>
        public override void RunEndOfPeriodOperation()
        {
            Int32 TempGLMSequence = -1;

            intEntryCount = 0;

            if (!FInfoMode)
            {
                TCarryForward carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
                carryForward.SetNextPeriod();
            }

            if (PostingFromDS.AGeneralLedgerMaster.Rows.Count > 0)
            {
                for (int i = 0; i < PostingFromDS.AGeneralLedgerMaster.Rows.Count; ++i)
                {
                    generalLedgerMasterRowFrom =
                        (AGeneralLedgerMasterRow)PostingFromDS.AGeneralLedgerMaster[i];
                    generalLedgerMasterRowTo = null;

                    for (int j = 0; j < PostingToDS.AGeneralLedgerMaster.Rows.Count; ++j)
                    {
                        generalLedgerMasterRowTo =
                            (AGeneralLedgerMasterRow)PostingToDS.AGeneralLedgerMaster[j];

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
                        if (!FInfoMode)
                        {
                            generalLedgerMasterRowTo =
                                (AGeneralLedgerMasterRow)PostingToDS.AGeneralLedgerMaster.NewRowTyped(true);
                            generalLedgerMasterRowTo.GlmSequence = TempGLMSequence;
                            TempGLMSequence--;
                            PostingToDS.AGeneralLedgerMaster.Rows.Add(generalLedgerMasterRowTo);
                        }

                        ++intEntryCount;
                    }

                    if (!FInfoMode)
                    {
                        generalLedgerMasterRowTo.LedgerNumber = generalLedgerMasterRowFrom.LedgerNumber;
                        generalLedgerMasterRowTo.Year = intNextYear;
                        generalLedgerMasterRowTo.AccountCode = generalLedgerMasterRowFrom.AccountCode;
                        generalLedgerMasterRowTo.CostCentreCode = generalLedgerMasterRowFrom.CostCentreCode;
                        generalLedgerMasterRowTo.YtdActualBase = generalLedgerMasterRowFrom.YtdActualBase;
                    }
                }
            }

            if (DoExecuteableCode)
            {
                GLPostingTDSAccess.SubmitChanges(PostingToDS);
            }
        }
    }

    /// <summary>
    /// This is the list of status values of a_ledger.a_year_end_process_status_i which has been
    /// copied from petra. The status begins by counting from RESET_Status up to LEDGER_UPDATED
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
    /// This is the actual list of the different error status codes of the GL Module ...
    /// </summary>
    public enum TPeriodEndErrorAndStatusCodes
    {
        /// <summary>
        /// If a periodic end operation shall be done at least on one database record
        /// (something like the calculation of the admin fees) this error is shown to indicate
        /// tha no database records were affected.
        /// </summary>
        PEEC_01,

        /// <summary>
        /// Afte a specific period end operation has been done, the programm calculates again the
        /// number of database records which shall be changed. If this value is non zero, this
        /// error is shown.
        /// Type: Critical.
        /// </summary>
            PEEC_02,

        /// <summary>The user has required a month end but a year end should be done first.</summary>
            PEEC_03,

        /// <summary>The user has required a year end but a month end should be done first.</summary>
            PEEC_04,

        /// <summary>A revaluation should be done before the month end.</summary>
            PEEC_05,

        /// <summary>Unposted batches prevent period close.</summary>
            PEEC_06,

        /// <summary>Suspense accounts prevent period close.</summary>
            PEEC_07,

        /// <summary>Unposted gift batches are found prevent period close.</summary>
            PEEC_08,

        /// <summary>No income accounts have been found.</summary>
            PEEC_09,

        /// <summary>No expense accounts have been found.</summary>
            PEEC_10,

        /// <summary>No ICH_ACCT Account is defined.</summary>
            PEEC_11,
    }
}