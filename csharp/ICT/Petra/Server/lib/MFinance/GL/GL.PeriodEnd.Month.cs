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
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.lib.MFinance.GL;


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

namespace Ict.Petra.Server.lib.MFinance.GL
{

	public class TMonthEnd
	{
		
		public bool RunMonthEndInfo(int ALedgerNum, 
		                           out TVerificationResultCollection AVRCollection)
		{
			AVRCollection = new TVerificationResultCollection();
			//TVerificationResult tvr = new TVerificationResult
			return  false;
		}

		public bool RunMonthEnd(int ALedgerNum,
		                       out TVerificationResultCollection AVRCollection)
		{
			AVRCollection = new TVerificationResultCollection();
			
			//bool lif = new LedgerInitFlags(42).StatusOfFlag(LedgerInitFlags.REVALUATION_FLAG);
			//TVerificationResult tvr = new TVerificationResult(
			return  false;
		}
	}
	
	public class LedgerInitFlags
	{
		public const string REVALUATION_FLAG = "Reval";  
		
		ALedgerInitFlagTable alifT;
			
		public LedgerInitFlags(int ALedgerNum)
		{
			alifT = ALedgerInitFlagAccess.LoadViaALedger(ALedgerNum, null);
		}
		
		public bool StatusOfFlag(string AFlagName)
		{
			//alifT = ALedgerInitFlagAccess.LoadViaALedgerTemplate 
			bool blnStatus = false;
			
			if (alifT.Rows.Count !=0) 
			{
				for (int i=0; i < alifT.Rows.Count; ++i)
				{
					
				}
			}
			System.Diagnostics.Debug.WriteLine(alifT.Rows.Count.ToString());
			
			return true;
		}
		
	}
}
