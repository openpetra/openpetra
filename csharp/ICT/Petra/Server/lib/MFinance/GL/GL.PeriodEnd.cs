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
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// If a proceure is defined which shall be assined inside a specific perodic process you have to use this
    /// class the handle the operation itself and the AbstractPerdiodEndOperation class to handle the internal
    /// parts of the operation. <br />
    /// For example the class TMonthEnd and TYearEnd inherits TPerdiodEndOperations.<br />
    /// </summary>
    public class TPerdiodEndOperations
    {
        /// <summary>
        /// If the user invokes a specific year end command, he automatically starts a server request
        /// only to make some checks and to gather some specific information. Handling this parameter
        /// enables to gather this information in the same routine which is used for the calculations.
        /// So both processes are automatically synchronized. <br />
        /// So do not run any excecutive code if the system is in the info mode.
        /// </summary>
        protected bool blnIsInInfoMode;

        /// <summary>
        /// If the system has run into a critial error, the (either in the info mode or in the executive
        /// node) you shall not run any executive code any more. But it may be useful to gather more information
        /// and to the process is not terminated completely.
        /// </summary>
        protected bool blnCriticalErrors;

        /// <summary>
        /// This is the standard VerificationResultCollection for the info and the error messages.
        /// </summary>
        protected TVerificationResultCollection verificationResults;


        /// <summary>
        /// This is for all info only routines that means JobSize has no definition
        /// </summary>
        /// <param name="Apeo"></param>
        protected void RunPeriodEndCheck(AbstractPerdiodEndOperation Apeo)
        {
            Apeo.IsInInfoMode = blnIsInInfoMode;
            Apeo.VerificationResultCollection = verificationResults;
            Apeo.RunEndOfPeriodOperation();

            if (Apeo.HasCriticalErrors)
            {
                blnCriticalErrors = true;
            }
        }

        /// <summary>
        /// Standard routine to execute the period end of each AbstractPerdiodEndOperation correctly
        /// </summary>
        /// <param name="Apeo"></param>
        /// <param name="AOperationName"></param>
        protected void RunPeriodEndSequence(AbstractPerdiodEndOperation Apeo, string AOperationName)
        {
            Apeo.IsInInfoMode = blnIsInInfoMode;
            Apeo.VerificationResultCollection = verificationResults;

            if (Apeo.JobSize == 0)
            {
                // Non Critical Problem but the user shall be informed ...
                String strTitle = Catalog.GetString("Peridic end routine hint");
                String strMessage = Catalog.GetString("There is nothing to do for the module: [{0}]");
                strMessage = String.Format(strMessage, AOperationName);
                TVerificationResult tvt =
                    new TVerificationResult(strTitle, strMessage, "",
                        TPeriodEndErrorAndStatusCodes.PEEC_01.ToString(),
                        TResultSeverity.Resv_Noncritical);
                verificationResults.Add(tvt);
            }
            else
            {
                Apeo.RunEndOfPeriodOperation();
                AbstractPerdiodEndOperation newApeo = Apeo.GetActualizedClone();
                newApeo.IsInInfoMode = true;
                newApeo.VerificationResultCollection = verificationResults;

                if (newApeo.JobSize != 0)
                {
                    // Critical Problem beause now there shall nothing to do anymore ...
                    String strTitle = Catalog.GetString("Problem occurs in module [{0}]");
                    strTitle = String.Format(strTitle, AOperationName);
                    String strMessage = Catalog.GetString(
                        "The operation has heft {0} elements which are not transformed!");
                    strMessage = String.Format(strMessage, newApeo.JobSize.ToString());
                    TVerificationResult tvt =
                        new TVerificationResult(strTitle, strMessage, "",
                            TPeriodEndErrorAndStatusCodes.PEEC_02.ToString(),
                            TResultSeverity.Resv_Critical);
                    verificationResults.Add(tvt);
                    blnCriticalErrors = true;
                }

                if (newApeo.HasCriticalErrors)
                {
                    blnCriticalErrors = true;
                }
            }

            if (Apeo.HasCriticalErrors)
            {
                blnCriticalErrors = true;
            }
        }
    }


    /// <summary>
    /// Any perdiodic end operation shall be inherit and complete this abstract class. The constructor of the
    /// inhereting class shall handle all parameters which are necessary for the RunEndOfPeriodOperation and
    /// RunEndOfPeriodOperation shall handle the data base operations
    /// </summary>
    public abstract class AbstractPerdiodEndOperation
    {
        /// <summary>
        /// This is the standard VerificationResultCollection for the info and the error messages.
        /// </summary>
        protected TVerificationResultCollection verificationResults = null;

        /// <summary>
        /// See TPerdiodEndOperations
        /// </summary>
        protected bool blnIsInInfoMode = true;

        /// <summary>
        /// See TPerdiodEndOperations
        /// </summary>
        protected bool blnCriticalErrors = false;

        protected int intCountJobs;

        /// <summary>
        /// JobSize returns the number of data base records which are affected in this operation. Be shure
        /// not only to find the data bases which are to be changed but also not to find the records which
        /// are allready changed.
        /// Or: Be shure that JobSize is zero after RunEndOfPeriodOperation has been done sucessfully.
        /// </summary>
        public abstract int JobSize {
            get;
        }

        /// <summary>
        /// The specific operation is done. Be shure to handle blnIsInInfoMode and blnCriticalErrors correctly
        /// </summary>
        public abstract void RunEndOfPeriodOperation();

        /// <summary>
        /// Method to create a duplicate based on the actualized database value(s)
        /// </summary>
        /// <returns></returns>
        public abstract AbstractPerdiodEndOperation GetActualizedClone();


        /// <summary>
        /// Summarizes the values of blnCriticalErrors and blnIsInInfoMode to the decision if an
        /// executable code shall be done or not.
        /// </summary>
        public bool DoExecuteableCode
        {
            get
            {
                return !(blnCriticalErrors | blnIsInInfoMode);
            }
        }

        /// <summary>
        /// Set-Property to set the common value of the VerificationResultCollection
        ///  (Set by TPerdiodEndOperations.RunYearEndSequence)
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            set
            {
                verificationResults = value;
            }
        }

        /// <summary>
        /// Property to set the correct info-mode (Set by TPerdiodEndOperations.RunYearEndSequence)
        /// </summary>
        public bool IsInInfoMode
        {
            set
            {
                blnIsInInfoMode = value;
            }
        }

        /// <summary>
        /// Property to read if the process could be done without critical errors.
        ///  (Used by TPerdiodEndOperations.RunYearEndSequence)
        /// </summary>
        public bool HasCriticalErrors
        {
            get
            {
                return blnCriticalErrors;
            }
        }
    }

    /// <summary>
    /// ENum-List of the accounting stati of a ledger
    /// </summary>
    public enum TCarryForwardENum
    {
        Month,
        Year
    }

    /// <summary>
    /// Zentral object which shall switch from one acounting intervall to the next
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
            }
            else if (ledgerInfo.CurrentPeriod == ledgerInfo.NumberOfAccountingPeriods)
            {
                // Set the YearEndFlag to "Switch between the months ...
                SetProvisionalYearEndFlag(true);
                SetYearMark(0, true);
            }
            else
            {
                // Conventional Month->Month Switch ...
                SetNewFwdPeriodValue(ledgerInfo.CurrentPeriod + 1);
                SetYearMark(-1, false);
            }

            new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber,
                TLedgerInitFlagEnum.Revaluation).Flag = false;
        }

        private void SetYearMark(int AYearOffset, bool AValue)
        {
            TAccountPeriodToNewYear accountPeriod = new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber);

            int intYear = accountPeriod.ActualYear;
            TLedgerInitFlagHandler ledgerInitFlagHandler =
                new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber,
                    TLedgerInitFlagEnum.ActualYear);

            ledgerInitFlagHandler.AddMarker((intYear + AYearOffset).ToString());
            ledgerInitFlagHandler.Flag = AValue;
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

        /// <summary>
        /// This value is only defined if the TCarryForwardENum holds the value "year". In normal cases you can
        /// get the value of the actual accounting year from the table a_accounting_period
        /// of the open petra database. But this values are changed in the year end routine and in order
        /// to make shure to get allways the same value, the year is stored in a_ledger_init_flag from the
        /// entrance to the TCarryForwardENum.Year-Period to the next TCarryForwardENum.Month-Period.
        /// </summary>
        public int Year
        {
            get
            {
                TAccountPeriodToNewYear accountPeriod = new TAccountPeriodToNewYear(ledgerInfo.LedgerNumber);
                int intYear = accountPeriod.ActualYear;
                TLedgerInitFlagHandler ledgerInitFlagHandler =
                    new TLedgerInitFlagHandler(ledgerInfo.LedgerNumber,
                        TLedgerInitFlagEnum.ActualYear);
                ledgerInitFlagHandler.AddMarker((intYear).ToString());

                if (ledgerInitFlagHandler.Flag)
                {
                    return intYear;
                }

                ledgerInitFlagHandler.AddMarker((intYear - 1).ToString());

                if (ledgerInitFlagHandler.Flag)
                {
                    return intYear - 1;
                }

                throw new ApplicationException("Undefined TCarryForwardENum.Year Request");
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
    }

    public enum TPeriodEndErrorAndStatusCodes
    {
        /// <summary>
        /// If a periodic end operation shall be done at least on one data base record
        /// (something like the calculation of the admin fees) this hin is shon to indicate
        /// tha no database records are affected.
        /// </summary>
        PEEC_01,

        /// <summary>
        /// Afte a specific perdio end operation has been done, the programm calculates again the
        /// number of database records which shall be changed. If this value is non zero, this
        /// error is shown.
        /// Type: Critical.
        /// </summary>
        PEEC_02,

        /// <summary>
        /// The user has required a period month end but a year end should be done first.
        /// </summary>
        PEEC_03,

        /// <summary>
        /// The user has required a period year end but a month end should be done first.
        /// </summary>
        PEEC_04,

        /// <summary>
        /// A revaluation should be done befor the month end ..
        /// </summary>
        PEEC_05,

        /// <summary>
        /// Unposted batches are found
        /// </summary>
        PEEC_06,

        /// <summary>
        /// Suspensed accountes are found
        /// </summary>
        PEEC_07,

        /// <summary>
        /// Unposted gift batches are found ...
        /// </summary>
        PEEC_08,

        /// <summary>
        /// No income accounts have been found ...
        /// </summary>
        PEEC_09,

        /// <summary>
        /// No expense accounts have been found ...
        /// </summary>
        PEEC_10,

        /// <summary>
        /// No ICH_ACCT Account is defined
        /// </summary>
        PEEC_11,
    }
}