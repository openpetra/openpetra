//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
//
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// setup the motivation groups and motivation details
    /// </summary>
    public class TGiftSetupWebConnector
    {
        /// <summary>
        /// returns all motivation groups and details for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadMotivationDetails(Int32 ALedgerNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
            delegate
            {
                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
                AMotivationGroupAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                AMotivationDetailFeeAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            });

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// save modified motivation groups and cost centres
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveMotivationDetails(ref GiftBatchTDS AInspectDS)
        {
            if (AInspectDS != null)
            {
                // TODO make sure new motivation groups are created. at the moment only 1 existing motivation group is supported
                GiftBatchTDSAccess.SubmitChanges(AInspectDS);

                return TSubmitChangesResult.scrOK;
            }

            return TSubmitChangesResult.scrError;
        }
    }
}