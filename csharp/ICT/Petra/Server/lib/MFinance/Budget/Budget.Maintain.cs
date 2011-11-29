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
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.Budget.WebConnectors
{
    /// <summary>
    /// maintain the budget
    /// </summary>
    public class TBudgetMaintainWebConnector
    {
        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadBudget(Int32 ALedgerNumber)
        {
            BudgetTDS MainDS = new BudgetTDS();

            //TODO: need to filter on Year
            ABudgetAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            ABudgetRevisionAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
            ABudgetPeriodAccess.LoadAll(MainDS, null);

//            ABudgetPeriodTable BudgetPeriodTable = new ABudgetPeriodTable();
//            ABudgetPeriodRow TemplateRow = (ABudgetPeriodRow)BudgetPeriodTable.NewRow(false);
//
//            TemplateRow.BudgetSequence;
//            ABudgetPeriodAccess.LoadViaABudgetTemplate(MainDS, TemplateRow, null);


            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// save modified budgets
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveBudget(ref BudgetTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            if (AInspectDS != null)
            {
                return BudgetTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
            }

            return TSubmitChangesResult.scrError;
        }
    }
}