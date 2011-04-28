//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2005-2011 by OM International
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

namespace Ict.Petra.Server.MFinance.Session
{
    public class TFinanceSessionObject : TSessionObject
    {
        TLedgerInfo ledgerInfo;
        
        public TFinanceSessionObject(int ALedgerNumber)
        {
        	ledgerInfo =new TLedgerInfo(ALedgerNumber, MasterTransaction);
        }
        
        /// <summary>
        /// Access to the base info of the selected ledger
        /// </summary>
        public TLedgerInfo LedgerInfo
        {
        	get 
        	{
        		return ledgerInfo;
        	}
        }
        
    }

    /// <summary>
    /// This routine reads the line of a_ledger defined by the ledger number
    /// </summary>
    public class TLedgerInfo
    {
        int ledgerNumber;
        TDBTransaction masterTransaction;
        private ALedgerTable ledger = null;
        ALedgerRow row;

        /// <summary>
        /// Constructor to address the correct table line (relevant ledger number). The
        /// constructor only will run the database accesses including a CommitTransaction
        /// and so this object may be used to "store" the data and use the database connection
        /// for something else.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATransaction"></param>
        public TLedgerInfo(int ALedgerNumber, TDBTransaction ATransaction)
        {
            ledgerNumber = ALedgerNumber;
            masterTransaction = ATransaction;
            LoadInfoLine();
        }
        
        private void LoadInfoLine()
        {
            ledger = ALedgerAccess.LoadByPrimaryKey(ledgerNumber, masterTransaction);
            row = (ALedgerRow)ledger[0];
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

                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetProvisionalYearEndFlagDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, masterTransaction, ParametersArray);
                LoadInfoLine();
            }
        }


        public int CurrentPeriod
        {
            get
            {
                return row.CurrentPeriod;
            }
        }
        public int NumberOfAccountingPeriods
        {
            get
            {
                return row.NumberOfAccountingPeriods;
            }
        }
        public int NumberFwdPostingPeriods
        {
            get
            {
                return row.NumberFwdPostingPeriods;
            }
        }
        public int CurrentFinancialYear
        {
            get
            {
                return row.CurrentFinancialYear;
            }
        }

        public int LedgerNumber
        {
            get
            {
                return row.LedgerNumber;
            }
        }

        public bool YearEndFlag
        {
            get
            {
                return row.YearEndFlag;
            }
        }
        public int TYearEndProcessStatus
        {
            get
            {
                return row.TYearEndProcessStatus;
            }
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetYearEndProcessStatusDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, masterTransaction, ParametersArray);
                LoadInfoLine();
            }
        }

        public bool IltAccountFlag
        {
            get
            {
                return row.IltAccountFlag;
            }
        }
        public bool BranchProcessing
        {
            get
            {
                return row.BranchProcessing;
            }
        }

        public bool IltProcessingCentre
        {
            get
            {
                return row.IltProcessingCentre;
            }
        }
    }


}