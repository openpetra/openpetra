/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections.Specialized;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
	/// <summary>
	/// setup the account hierarchy, cost centre hierarchy, and other data relevant for a General Ledger
	/// </summary>
	public class TGLSetupWebConnector
	{
		/// <summary>
		/// returns all account hierarchies available for this ledger
		/// </summary>
		/// <param name="ALedgerNumber"></param>
		/// <returns></returns>
		public static GLSetupTDS LoadAccountHierarchies(Int32 ALedgerNumber)
		{
			GLSetupTDS MainDS = new GLSetupTDS();
			
			AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
			AAccountHierarchyDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
			AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();
			
            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();
			
            return MainDS;
		}
		
		/// <summary>
		/// save modified account hierarchy etc
		/// </summary>
		/// <param name="AMainDS"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns></returns>
		public static TSubmitChangesResult SaveGLSetupTDS(ref GLSetupTDS AMainDS,
			out TVerificationResultCollection AVerificationResult)
		{
			// TODO SaveGLSetupTDS
			AVerificationResult = null;
			return TSubmitChangesResult.scrError;
		}
	}
}