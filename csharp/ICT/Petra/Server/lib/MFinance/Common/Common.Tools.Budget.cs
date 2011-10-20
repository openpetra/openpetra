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
    /// The THandleBudgetInfo was primilary written for the year end calculation(s).
    /// </summary>
    public class THandleBudgetInfo
    {
        TLedgerInfo tHandleLedgerInfo;
        ABudgetTable aBudgetTable;
        ABudgetRow aBudgetRow;

        List <THandleBudgetPeriodInfo>budgetPeriodInfoList;

        /// <summary>
        /// The constructor internally reads in all a_budget-Table entries which belong to the
        /// ledger
        /// </summary>
        /// <param name="ATLedgerInfo">For LedgerNumber only</param>
        public THandleBudgetInfo(TLedgerInfo ATLedgerInfo)
        {
            tHandleLedgerInfo = ATLedgerInfo;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                aBudgetTable = ABudgetAccess.LoadViaALedger(tHandleLedgerInfo.LedgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
            budgetPeriodInfoList = new List <THandleBudgetPeriodInfo>();
        }

        /// <summary>
        /// Preparation of the Close-Budget-Operation(s). The relevant THandleBudgetPeriodInfo
        /// data sets are sorted out and added to a list.
        /// </summary>
        public void ReadCloseBudgetListYearEnd()
        {
            if (aBudgetTable.Rows.Count > 0)
            {
                for (int iBgt = 0; iBgt < aBudgetTable.Rows.Count; ++iBgt)        // FOR EACH a_budget
                {
                    aBudgetRow = aBudgetTable[iBgt];
                    int iHelp = tHandleLedgerInfo.NumberOfAccountingPeriods +
                                tHandleLedgerInfo.NumberFwdPostingPeriods;

                    // iBgtPrd start with 1 because the first period of a year has the
                    // number 1
                    for (int iBgtPrd = 1; iBgtPrd < iHelp; ++iBgtPrd)
                    {
                        THandleBudgetPeriodInfo tHandleBudgetPeriodInfo =
                            new THandleBudgetPeriodInfo(aBudgetRow.BudgetSequence, iBgtPrd);

                        if (tHandleBudgetPeriodInfo.IsValid)
                        {
                            budgetPeriodInfoList.Add(tHandleBudgetPeriodInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Running the close procedure by using a Transation which is defined outside
        /// of the object. The transaction is required in order to create a
        /// complete task transaction for the complete year end.
        /// </summary>
        /// <param name="ATransaction"></param>
        public void CloseBudgetListYearEnd(TDBTransaction ATransaction)
        {
            if (budgetPeriodInfoList.Count > 0)
            {
                for (int i = 0; i < budgetPeriodInfoList.Count; ++i)
                {
                    budgetPeriodInfoList[i].ClosePeriodYearEnd(ATransaction);
                }
            }
        }
    }

    /// <summary>
    /// An object which mainly shall be used by THandleBudgetInfo.
    /// </summary>
    public class THandleBudgetPeriodInfo
    {
        ABudgetPeriodTable aBudgetPeriodTable;
        ABudgetPeriodRow aBudgetPeriodRow;

        /// <summary>
        /// One cudget period info record will be loaded - if exists
        /// </summary>
        /// <param name="ABudgetSequence">1st. Primary key parameter</param>
        /// <param name="ABudgetPeriod">2nd. Primary key parameter</param>
        public THandleBudgetPeriodInfo(int ABudgetSequence, int ABudgetPeriod)
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                aBudgetPeriodTable = ABudgetPeriodAccess.LoadByPrimaryKey(
                    ABudgetSequence, ABudgetPeriod, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }

            if (aBudgetPeriodTable.Rows.Count > 0)
            {
                aBudgetPeriodRow = aBudgetPeriodTable[0];
            }
        }

        /// <summary>
        /// Returns true if a record has been found
        /// </summary>
        public bool IsValid
        {
            get
            {
                return aBudgetPeriodRow != null;
            }
        }

        /// <summary>
        /// Runs a year end closing on the budget record
        /// </summary>
        /// <param name="ATransaction">A required transaction to synchronize with all
        /// other year end operations.</param>
        public void ClosePeriodYearEnd(TDBTransaction ATransaction)
        {
            aBudgetPeriodRow.BudgetLastYear = aBudgetPeriodRow.BudgetThisYear;
            aBudgetPeriodRow.BudgetThisYear = aBudgetPeriodRow.BudgetNextYear;
            aBudgetPeriodRow.BudgetNextYear = 0;

            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[0].Value = aBudgetPeriodRow.BudgetThisYear;;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[1].Value = aBudgetPeriodRow.BudgetNextYear;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[2].Value = 0;
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = aBudgetPeriodRow.BudgetSequence;
            ParametersArray[4] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[4].Value = aBudgetPeriodRow.PeriodNumber;

            string strSQL = "UPDATE PUB_" + ABudgetPeriodTable.GetTableDBName() + " ";
            strSQL += "SET " + ABudgetPeriodTable.GetBudgetLastYearDBName() + " = ? ";
            strSQL += ", " + ABudgetPeriodTable.GetBudgetThisYearDBName() + " = ? ";
            strSQL += ", " + ABudgetPeriodTable.GetBudgetNextYearDBName() + " = ? ";
            strSQL += "WHERE " + ABudgetPeriodTable.GetBudgetSequenceDBName() + " = ? ";
            strSQL += "AND " + ABudgetPeriodTable.GetPeriodNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, ATransaction, ParametersArray);
        }
    }
}