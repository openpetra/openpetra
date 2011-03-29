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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.GL
{
	
	/// <summary>
	/// This is the list of vaild Ledger-Init-Flags 
	/// (the TLedgerInitFlagHandler has an internal information of the flag)
	/// </summary>
	public enum LegerInitFlag{
		/// <summary>
		/// Revaluation is a process which has to be done once each month. So the value 
		/// a) is set to true if a revaluation is done, 
		/// b) set to false if the month end process was done successful. 
		/// c) is used bevor the month end process to remember the user for the outstanding 
		/// revaluation. 
		/// </summary>
		Revaluation
	}
	
	/// <summary>
	/// LedgerInitFlag is a table wich holds a small set of "boolean" properties for each
	/// Ledger refered to the actual month. 
	/// One example is the value that a Revaluation has been done in the actual month. Some other
	/// values will be added soon. 
	/// </summary>
	 public class TLedgerInitFlagHandler{

		private TVerificationResultCollection VerificationResult = null;
		private int intLedgerNumber;
		private string strFlagName;
	
		/// <summary>
		/// This Constructor only takes and stores the initial parameters. No 
		/// Database request is done by this routine.
		/// </summary>
		/// <param name="ALedgerNumber">A valid ledger number</param>
		/// <param name="AFlagNum">A valid LegerInitFlag entry</param>
		public TLedgerInitFlagHandler(int ALedgerNumber, LegerInitFlag AFlagNum)
		{
			intLedgerNumber = ALedgerNumber;
			strFlagName = String.Empty;
			if (AFlagNum == LegerInitFlag.Revaluation) {
				strFlagName = "REVAL";
			}
			if (strFlagName.Equals(String.Empty))
			{
				throw new ApplicationException("Please define a value for the selected enum");
			}
		}

		/// <summary>
		/// The flag property controls all databse requests.
		/// </summary>
		public bool Flag
		{
			get {
				return FindRecord();
			}
			set {
				if (FindRecord()) {
					if (!value) {
						DeleteRecord();
					}
				} else {
					if (value) {
						CreateRecord();
					}
				}
			}
		}

		private bool FindRecord()
		{
			bool boolValue;
			TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
			ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
				intLedgerNumber, strFlagName, Transaction);
			boolValue = (aLedgerInitFlagTable.Rows.Count == 1);
			DBAccess.GDBAccessObj.CommitTransaction();
			HandleVerificationResuls();
			return boolValue;
		}
		
		private void CreateRecord()
		{
			TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
			ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(				
				intLedgerNumber, strFlagName, Transaction);
			ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();
			aLedgerInitFlagRow.LedgerNumber  = intLedgerNumber;
			aLedgerInitFlagRow.InitOptionName = strFlagName;
			aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
			ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);
			DBAccess.GDBAccessObj.CommitTransaction();
			HandleVerificationResuls();
		}
		
		private void DeleteRecord()
		{
			TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
			ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
				intLedgerNumber, strFlagName, Transaction);
			if (aLedgerInitFlagTable.Rows.Count == 1) {
				((ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0]).Delete();
				ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);
			}
			DBAccess.GDBAccessObj.CommitTransaction();
			HandleVerificationResuls();
		}
		
		private void HandleVerificationResuls()
		{
			if (VerificationResult != null){
				if (VerificationResult.HasCriticalError())
				{
					throw new ApplicationException("TLedgerInitFlagHandler does not work");
				}
			}
		}
		
	}
}

