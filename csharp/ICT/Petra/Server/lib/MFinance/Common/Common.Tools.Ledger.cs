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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// This routine reads the line of a_ledger defined by the ledger number
    /// </summary>
    public class TLedgerInfo
    {
        int ledgerNumber;
        private ALedgerTable ledger = null;
        ALedgerRow row;

        /// <summary>
        /// Constructor to address the correct table line (relevant ledger number). The
        /// constructor only will run the database accesses including a CommitTransaction
        /// and so this object may be used to "store" the data and use the database connection
        /// for something else.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TLedgerInfo(int ALedgerNumber)
        {
            ledgerNumber = ALedgerNumber;
            LoadInfoLine();
        }

        private void LoadInfoLine()
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                ledger = ALedgerAccess.LoadByPrimaryKey(ledgerNumber, transaction);
                row = (ALedgerRow)ledger[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Property to read the value of the Revaluation account
        /// </summary>
        public string RevaluationAccount
        {
            get
            {
                return row.ForexGainsLossesAccount;
            }
        }

        /// <summary>
        /// Property to read the value of the base currency
        /// </summary>
        public string BaseCurrency
        {
            get
            {
                return row.BaseCurrency;
            }
        }

        /// <summary>
        /// Property to read the value of the ProvisionalYearEndFlag
        /// </summary>
        public bool ProvisionalYearEndFlag
        {
            get
            {
                return row.ProvisionalYearEndFlag;
            }
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Bit);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                bool NewTransaction;
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetProvisionalYearEndFlagDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, transaction, ParametersArray);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                LoadInfoLine();
            }
        }


        /// <summary>
        ///
        /// </summary>
        public int CurrentPeriod
        {
            get
            {
                return row.CurrentPeriod;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberOfAccountingPeriods
        {
            get
            {
                return row.NumberOfAccountingPeriods;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberFwdPostingPeriods
        {
            get
            {
                return row.NumberFwdPostingPeriods;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int CurrentFinancialYear
        {
            get
            {
                return row.CurrentFinancialYear;
            }
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                bool NewTransaction;
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetCurrentFinancialYearDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, transaction, ParametersArray);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                LoadInfoLine();
            }
        }


        /// <summary>
        ///
        /// </summary>
        public int LedgerNumber
        {
            get
            {
                return row.LedgerNumber;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public bool YearEndFlag
        {
            get
            {
                return row.YearEndFlag;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int YearEndProcessStatus
        {
            get
            {
                return row.YearEndProcessStatus;
            }
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                bool NewTransaction;
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetYearEndProcessStatusDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, transaction, ParametersArray);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                LoadInfoLine();
            }
        }


        /// <summary>
        ///
        /// </summary>
        public bool IltAccountFlag
        {
            get
            {
                return row.IltAccountFlag;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool BranchProcessing
        {
            get
            {
                return row.BranchProcessing;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public bool IltProcessingCentre
        {
            get
            {
                return row.IltProcessingCentre;
            }
        }

        /// <summary>
        /// return standard cost centre to be used for given ledger number
        /// </summary>
        public string GetStandardCostCentre()
        {
            return GetStandardCostCentre(LedgerNumber);
        }

        /// <summary>
        /// return standard cost centre to be used for given ledger number
        /// </summary>
        public static string GetStandardCostCentre(int ALedgerNumber)
        {
            return String.Format("{0:##00}00", ALedgerNumber);
        }
    }

    /// <summary>
    /// May be obsolete ...
    /// Wait until Year End is done well ...
    /// </summary>
    public class TLedgerLock
    {
        int intLegerNumber;
        private bool blnResult;
        private Object synchRoot = new Object();

        /// <summary>
        ///
        /// </summary>
        public TLedgerLock(int ALedgerNum)
        {
            intLegerNumber = ALedgerNum;
            TVerificationResultCollection tvr;
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLegerNumber, TLedgerInitFlagEnum.LedgerLock.ToString(), Transaction);
            ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();
            aLedgerInitFlagRow.LedgerNumber = intLegerNumber;
            aLedgerInitFlagRow.InitOptionName = TLedgerInitFlagEnum.LedgerLock.ToString();
            lock (synchRoot) {
                try
                {
                    aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
                    ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out tvr);
                    blnResult = true;

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                }
                catch (System.Data.ConstraintException)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }

                    blnResult = false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return blnResult;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public void UnLock()
        {
            if (blnResult)
            {
                TLedgerInitFlagHandler tifh =
                    new TLedgerInitFlagHandler(intLegerNumber, TLedgerInitFlagEnum.LedgerLock);
                tifh.Flag = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string LockInfo()
        {
            if (!blnResult)
            {
                try
                {
                    bool NewTransaction;
                    TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);
                    ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                        intLegerNumber, TLedgerInitFlagEnum.LedgerLock.ToString(), Transaction);
                    ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0];
                    string strAnswer = aLedgerInitFlagRow.CreatedBy + " - " +
                                       DateTime.Parse(aLedgerInitFlagRow.DateCreated.ToString()).ToLongDateString();

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }

                    return strAnswer;
                }
                catch (Exception)
                {
                    return Catalog.GetString("Free Again");
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }

    /// <summary>
    /// This is the list of valid Ledger-Init-Flags
    /// (the TLedgerInitFlagHandler has an internal information of the flag)
    /// </summary>
    public enum TLedgerInitFlagEnum
    {
        /// <summary>
        /// Revaluation is a process which has to be done once each month. So the value
        /// a) is set to true if a revaluation is done,
        /// b) set to false if the month end process was done successful.
        /// c) is used bevor the month end process to remember the user for the outstanding
        /// revaluation.
        /// </summary>
        Revaluation,

        /// <summary>
        /// Property to lock a ledger ...
        /// Not implemented in petra
        /// </summary>
        LedgerLock,


        /// <summary>
        ///
        /// </summary>
        DatabaseAllocation,

        /// <summary>
        /// Used for the period end year as a marker for the year ...
        /// </summary>
        ActualYear
    }

    /// <summary>
    /// LedgerInitFlag is a table wich holds a small set of "boolean" properties for each
    /// Ledger refered to the actual month.
    /// One example is the value that a Revaluation has been done in the actual month. Some other
    /// values will be added soon.
    /// </summary>
    public class TLedgerInitFlagHandler
    {
        private TVerificationResultCollection VerificationResult = null;
        private int intLedgerNumber;
        private string strFlagName;
        private string strFlagNameHelp;

        /// <summary>
        /// This Constructor only takes and stores the initial parameters. No
        /// Database request is done by this routine.
        /// </summary>
        /// <param name="ALedgerNumber">A valid ledger number</param>
        /// <param name="AFlagEnum">A valid LegerInitFlag entry</param>
        public TLedgerInitFlagHandler(int ALedgerNumber, TLedgerInitFlagEnum AFlagEnum)
        {
            intLedgerNumber = ALedgerNumber;
            strFlagName = String.Empty;

            if (AFlagEnum.Equals(TLedgerInitFlagEnum.Revaluation))
            {
                strFlagName = "REVALUATION-RUN";
            }
            else
            {
                strFlagName = AFlagEnum.ToString();
            }

            strFlagNameHelp = strFlagName;
        }

        /// <summary>
        ///
        /// </summary>
        public void AddMarker(string AMarker)
        {
            strFlagName = strFlagNameHelp + ":" + AMarker;
        }

        /// <summary>
        /// The flag property controls all databse requests.
        /// </summary>
        public bool Flag
        {
            get
            {
                return FindRecord();
            }
            set
            {
                if (FindRecord())
                {
                    if (!value)
                    {
                        DeleteRecord();
                    }
                }
                else
                {
                    if (value)
                    {
                        CreateRecord();
                    }
                }
            }
        }


        /// <summary>
        ///
        /// </summary>
        public void SetFlagAndName(string AName)
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);
            ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();

            aLedgerInitFlagRow.LedgerNumber = intLedgerNumber;
            aLedgerInitFlagRow.InitOptionName = strFlagName;
            aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
            ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            HandleVerificationResuls();
        }

        private bool FindRecord()
        {
            bool boolValue;
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);

            boolValue = (aLedgerInitFlagTable.Rows.Count == 1);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            HandleVerificationResuls();
            return boolValue;
        }

        private void CreateRecord()
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);
            ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();

            aLedgerInitFlagRow.LedgerNumber = intLedgerNumber;
            aLedgerInitFlagRow.InitOptionName = strFlagName;
            aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
            ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            HandleVerificationResuls();
        }

        private void DeleteRecord()
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);

            if (aLedgerInitFlagTable.Rows.Count == 1)
            {
                ((ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0]).Delete();
                ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            HandleVerificationResuls();
        }

        private void HandleVerificationResuls()
        {
            if (VerificationResult != null)
            {
                if (VerificationResult.HasCriticalErrors)
                {
                    throw new ApplicationException("TLedgerInitFlagHandler does not work");
                }
            }
        }
    }
}