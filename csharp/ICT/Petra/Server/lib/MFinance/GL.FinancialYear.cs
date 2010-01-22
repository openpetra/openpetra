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
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Server.MFinance.GL
{
    ///<summary>
    /// This connector provides methods for creating and closing a financial year or period
    ///</summary>
    public class TFinancialYear
    {
        /// <summary>
        /// check if the given date is in one of the open accounting periods of the given ledger, ie current or forward posting periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateToTest"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AYearNr"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        public static bool IsValidPeriod(Int32 ALedgerNumber,
            DateTime ADateToTest,
            out Int32 APeriodNumber,
            out Int32 AYearNr,
            TDBTransaction ATransaction)
        {
            APeriodNumber = -1;
            AYearNr = -1;
            AAccountingPeriodTable table = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, ATransaction);

            foreach (AAccountingPeriodRow row in table.Rows)
            {
                if ((row.PeriodStartDate <= ADateToTest) && (ADateToTest <= row.PeriodEndDate))
                {
                    // check if this period is either the current period or one of the forward posting periods
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);

                    if ((LedgerTable.Count == 1)
                        && ((row.AccountingPeriodNumber >= LedgerTable[0].CurrentPeriod)
                            && (row.AccountingPeriodNumber <= LedgerTable[0].CurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods)))
                    {
                        APeriodNumber = row.AccountingPeriodNumber;
                        AYearNr = LedgerTable[0].CurrentFinancialYear;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Create the first financial year in a fresh database
        /// TODO/TOTHINK: import start balances from somewhere?
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFirstYearNumber"></param>
        /// <param name="AInitialPeriod"></param>
        /// <returns></returns>
        public static bool CreateInitialFinancialYear(Int32 ALedgerNumber, Int32 AFirstYearNumber, Int32 AInitialPeriod)
        {
            // TODO: create a_general_ledger_master, create a_general_ledger_master_period for main account/costcentre?
            // ledger a_current_financial_year_i
            // ledger a_current_period_i

            return false;
        }

        /// <summary>
        /// close the current financial period, open the next period, if it was not the last period of the year
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYearNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        public static bool ClosePeriod(Int32 ALedgerNumber, Int32 AYearNumber, Int32 APeriodNumber)
        {
            // TODO: ClosePeriod
            return false;
        }

        /// <summary>
        /// Close a financial year, open the next financial year
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYearNumber"></param>
        /// <returns></returns>
        public static bool CloseYear(Int32 ALedgerNumber, Int32 AYearNumber)
        {
            // TODO: CloseYear
            return false;
        }
    }
}