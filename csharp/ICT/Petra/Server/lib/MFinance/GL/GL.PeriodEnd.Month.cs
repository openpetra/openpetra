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
			return  blnCriticalErrors;
		}

		public bool RunMonthEnd(int ALedgerNum,
		                       out TVerificationResultCollection AVRCollection)
		{
			RunFirstChecks(ALedgerNum, out AVRCollection);
			return  blnCriticalErrors;
		}
		
		private void RunFirstChecks(int ALedgerNum,
		                       out TVerificationResultCollection AVRCollection)
		{
			AVRCollection = new TVerificationResultCollection();
			GetLedgerInfo ledgerInfo = new GetLedgerInfo(ALedgerNum);
			if (ledgerInfo.ProvisionalYearEndFlag) {
				TVerificationResult tvr = new TVerificationResult(
					Catalog.GetString("ProvisionalYearEndFlag-Problem"),
					String.Format(
						Catalog.GetString("The year end processing for Ledger {0} needs to be run."),						ALedgerNum.ToString()),
					"", "PYEF-01", TResultSeverity.Resv_Critical);
				blnCriticalErrors = true;
			}
		}
		
	}
		public class GetBatchInfo{
			
			// Some &2 batches for ledger &1 have not yet been posted.
			
			ABatchTable batches;
			
			public GetBatchInfo(int ALedgerNumber, int ABatchPeriod)
			{
				OdbcParameter[] ParametersArray;
				ParametersArray = new OdbcParameter[2];
				ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
				ParametersArray[0].Value = ALedgerNumber; 
				ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
				ParametersArray[1].Value = ABatchPeriod;
				
				
        	    TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
        	    string strSQL = "SELECT * FROM PUB_" + ABatchTable.GetTableDBName() + " ";
        	    strSQL += "WHERE " + ABatchTable.GetLedgerNumberDBName() + " = ? ";
        	    strSQL += "AND " + ABatchTable.GetBatchPeriodDBName() + " = ? ";
        	    System.Data.DataTable b = DBAccess.GDBAccessObj.SelectDT(
        	    	strSQL, ABatchTable.GetTableDBName() , transaction, ParametersArray);
        	    System.Diagnostics.Debug.WriteLine(batches.Rows.Count.ToString());
        	    DBAccess.GDBAccessObj.CommitTransaction();
			}
			
			public int NumberOfBatches
			{
				get {
					return batches.Rows.Count;
				}
			}
			
			public string BatchList
			{
				get {
					string strList = " - ";
					if (NumberOfBatches != 0)
					{
						strList = batches[0].BatchNumber.ToString();
						if (NumberOfBatches >0)
						{
							for (int i=1; i < NumberOfBatches; ++i)
							{
								strList += ", " + batches[0].BatchNumber.ToString();
							}
						}
						strList = "(" + strList + ")";
					}
					return strList;
				}				
			}	
		}		
}
