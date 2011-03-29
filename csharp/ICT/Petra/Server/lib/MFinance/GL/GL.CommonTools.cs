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
using Ict.Common;
using Ict.Common.DB;
using System.Data;


using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.lib.MFinance.GL;


using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MFinance.GL
{
	
	 public class TLedgerInitFlagHandler{

		public static string REVALUATION = "REVAL";
		
		private TVerificationResultCollection VerificationResult;
		
		private int intLedgerNumber;
		private string strFlagName;
		private bool boolValue;
		
		public TLedgerInitFlagHandler(int ALedgerNumber, String AFlagName)
		{
			intLedgerNumber = ALedgerNumber;
			strFlagName = AFlagName;
			//System.Diagnostics.Debug.WriteLine("c" + alift.Rows.Count.ToString());
			//System.Diagnostics.Debug.WriteLine("c-boolValue: " + boolValue.ToString());
		}
		
		public bool Flag
		{
			get {
				System.Diagnostics.Debug.WriteLine("DL1:" + DBAccess.GDBAccessObj.DebugLevel.ToString());
				DBAccess.GDBAccessObj.DebugLevel = 10; 
				TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
				System.Diagnostics.Debug.WriteLine("intLedgerNumber " + intLedgerNumber.ToString());
				System.Diagnostics.Debug.WriteLine("strFlagName " + strFlagName);
				ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
					intLedgerNumber, strFlagName, Transaction);
				boolValue = (aLedgerInitFlagTable.Rows.Count == 1);
				System.Diagnostics.Debug.WriteLine("g " + aLedgerInitFlagTable.Rows.Count.ToString());
				System.Diagnostics.Debug.WriteLine("g-boolValue: " + boolValue.ToString());
				System.Diagnostics.Debug.WriteLine("DL2:" + DBAccess.GDBAccessObj.DebugLevel.ToString());
				DBAccess.GDBAccessObj.RollbackTransaction();
				return boolValue;
			}
			set {
				if (value) {
					// Create if record does not exist 
					if (boolValue) {
						TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
						ALedgerInitFlagTable alift = ALedgerInitFlagAccess.LoadByPrimaryKey(
							intLedgerNumber, strFlagName, Transaction);
						ALedgerInitFlagRow aliftr = (ALedgerInitFlagRow)alift.NewRow();
						aliftr.LedgerNumber  = intLedgerNumber;
						aliftr.InitOptionName = strFlagName; 
						alift.Rows.Add(aliftr);
						ALedgerInitFlagAccess.SubmitChanges(alift, null, out VerificationResult);
						DBAccess.GDBAccessObj.RollbackTransaction();
					}
				} else {
					// Delete if record exists
					//if (alift.Rows.Count == 1) {
						//((ALedgerInitFlagRow)alift.Rows[0]).Delete();
						//ALedgerInitFlagAccess.SubmitChanges(alift, null, out VerificationResult);
					//}
				}
			}
		}
	}
}

