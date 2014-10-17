//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2014 by OM International
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
    public abstract class TPeriodEndOperations
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
        protected TVerificationResultCollection FverificationResults;

        /// <summary>
        /// The operator is going to set 1, (period + 1).
        /// </summary>
        public abstract void SetNextPeriod();

        /// <summary>
        /// This is for all info only routines that means JobSize has no definition
        /// </summary>
        protected void RunPeriodEndCheck(AbstractPeriodEndOperation Apeo, TVerificationResultCollection AVerificationResults)
        {
            FverificationResults = AVerificationResults;
            Apeo.VerificationResultCollection = AVerificationResults;
            Apeo.IsInInfoMode = FInfoMode;
            Apeo.RunOperation();

            if (Apeo.HasCriticalErrors)
            {
                FHasCriticalErrors = true;
            }
        }

        /// <summary>
        /// Standard routine to execute each PeriodEndOperation, and then confirm its success
        /// </summary>
        /// <param name="AOperation"></param>
        /// <param name="AOperationName"></param>
        protected void RunPeriodEndSequence(AbstractPeriodEndOperation AOperation, string AOperationName)
        {
            AOperation.IsInInfoMode = FInfoMode;
            AOperation.VerificationResultCollection = FverificationResults;
            AOperation.FPeriodEndOperator = this;

            if (FInfoMode)
            {
                if (AOperation.GetJobSize() == 0)
                {
                    // Non Critical Problem but the user shall be informed ...
                    String strTitle = Catalog.GetString("Period end status");
                    String strMessage = String.Format(Catalog.GetString("Nothing to be done for \"{0}\""), AOperationName);
                    TVerificationResult tvt =
                        new TVerificationResult(strTitle, strMessage, "",
                            TPeriodEndErrorAndStatusCodes.PEEC_01.ToString(),
                            TResultSeverity.Resv_Info);
                    FverificationResults.Add(tvt);
                }
            }
            else
            {
                // now we actually run the operation
                Int32 OperationsDone = AOperation.RunOperation();
                if (OperationsDone == 0)
                {
                    // Non Critical Problem but the user shall be informed ...
                    String strTitle = Catalog.GetString("Period end status");
                    String strMessage = String.Format(Catalog.GetString("Nothing done for \"{0}\""), AOperationName);
                    TVerificationResult tvt =
                        new TVerificationResult(strTitle, strMessage, "",
                            TPeriodEndErrorAndStatusCodes.PEEC_01.ToString(),
                            TResultSeverity.Resv_Info);
                    FverificationResults.Add(tvt);
                }

                //
                // Now I want to verify whether the job has been finished correctly...

                AbstractPeriodEndOperation VerifyOperation = AOperation.GetActualizedClone();
                VerifyOperation.IsInInfoMode = true;
                VerifyOperation.VerificationResultCollection = FverificationResults;
                Int32 RemainingItems = VerifyOperation.GetJobSize();
                if (RemainingItems > 0)
                {
                    // Critical Problem because there should be nothing left to do.
                    TVerificationResult tvt =
                        new TVerificationResult(
                            String.Format(Catalog.GetString("Problem in \"{0}\""), AOperationName),
                            String.Format(Catalog.GetString("The operation has {0} elements remaining!"),
                                RemainingItems),
                            "",
                            TPeriodEndErrorAndStatusCodes.PEEC_02.ToString(),
                            TResultSeverity.Resv_Critical);
                    FverificationResults.Add(tvt);
                    FHasCriticalErrors = true;
                }

                FHasCriticalErrors |= AOperation.HasCriticalErrors;
                FHasCriticalErrors |= VerifyOperation.HasCriticalErrors;
            }
        }
    }

    /// <summary>
    /// The period end classes inherit and complete this abstract class. The constructor of the
    /// inheriting class handles all parameters which are necessary for the RunEndOfPeriodOperation and
    /// RunEndOfPeriodOperation handles the database operations.
    /// </summary>
    public abstract class AbstractPeriodEndOperation
    {
        /// <summary>
        /// This is the standard VerificationResultCollection for the info and the error messages.
        /// </summary>
        protected TVerificationResultCollection FverificationResults = null;

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
        protected int FCountJobs;

        /// <summary>
        /// GetJobSize returns the number of database records which are affected in this operation. Be sure
        /// not only to find the databases which are to be changed but also not to find the records which
        /// are already changed.
        /// Or: Be sure that JobSize is zero after RunEndOfPeriodOperation has been done sucessfully.
        /// </summary>
        public abstract int GetJobSize();

        /// <summary>
        /// The specific operation is done. Be sure to handle blnIsInInfoMode and blnCriticalErrors correctly
        /// </summary>
        public abstract Int32 RunOperation();

        /// <summary>
        /// Method to create a duplicate based on the actualized database value(s)
        /// </summary>
        /// <returns></returns>
        public abstract AbstractPeriodEndOperation GetActualizedClone();


        /// <summary>
        /// !(FHasCriticalErrors | FInfoMode)
        /// </summary>
        public bool DoExecuteableCode
        {
            get
            {
                return !(FHasCriticalErrors | FInfoMode);
            }
        }

        /// <summary>
        /// The parent operator (TMonthEnd or TYearEnd) that requests this operation
        /// </summary>
        public TPeriodEndOperations FPeriodEndOperator;

        /// <summary>
        /// Set-Property to set the common value of the VerificationResultCollection
        ///  (Set by TPeriodEndOperations.RunYearEndSequence)
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            set
            {
                FverificationResults = value;
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
    }  // AbstractPeriodEndOperation

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

/*
    /// <summary>
    /// Central object to switch to the next accounting period
    /// </summary>
    public class TCarryForward
    {
        /// <summary>
        /// 
        /// </summary>
        public TLedgerInfo FledgerInfo;

        /// <summary>
        /// A TMounthEnd or TYearEnd object
        /// </summary>
        public TPeriodEndOperations FPeriodEndOperator;
        /// <summary>
        /// The routine requires a TLedgerInfo object ...
        /// </summary>
        public TCarryForward(TLedgerInfo ALedgerInfo, TPeriodEndOperations PeriodEndOperator=null)
        {
            FledgerInfo = ALedgerInfo;
            FPeriodEndOperator = PeriodEndOperator;
        }

        /// <summary>
        /// Sets the ledger to the next accounting period ...
        /// </summary>
        public void SetNextPeriod()
        {
            if (GetPeriodType == TCarryForwardENum.Month)
            {
                TMonthEnd.SetNextPeriod(this);
            }
            else
            {
                TYearEnd.SetNextPeriod(this);
            }

            new TLedgerInitFlagHandler(FledgerInfo.LedgerNumber,
                TLedgerInitFlagEnum.Revaluation).Flag = false;  // ( "A Revaluation has not been done.")
        }

        /// <summary>
        /// Gets the type of the actual accouting period (see TCarryForwardENum).
        /// </summary>
        public TCarryForwardENum GetPeriodType
        {
            get
            {
                if (FledgerInfo.ProvisionalYearEndFlag)
                {
                    return TCarryForwardENum.Year;
                }
                else
                {
                    return TCarryForwardENum.Month;
                }
            }
        }


    } // TCarryForward
*/
    /// <summary>
    /// This is the actual list of the different error status codes of the GL Module ...
    /// </summary>
    public enum TPeriodEndErrorAndStatusCodes
    {
        /// <summary>
        /// If a period end operation shall be done at least on one database record
        /// (something like the calculation of admin fees) this error is shown to indicate
        /// that no database records were affected.
        /// </summary>
        PEEC_01,

        /// <summary>
        /// After a specific period end operation has been done, the programm calculates again the
        /// number of database records which should be changed.
        /// If this value is non zero, there's a problem.
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