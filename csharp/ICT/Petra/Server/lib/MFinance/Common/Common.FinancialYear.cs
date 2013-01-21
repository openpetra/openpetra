//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Server.MFinance.Common
{
    ///<summary>
    /// This connector provides methods for creating and closing a financial year or period
    ///</summary>
    public class TFinancialYear
    {
        /// <summary>
        /// using the calendar of the ledger to determine the financial period at the given date.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateToTest"></param>
        /// <param name="AFinancialYear"></param>
        /// <param name="AFinancialPeriod"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ADoFixDate">do you want to fix the date if it is outside the open periods;
        /// if the date is outside of the open periods, the date will be changed to the most appropriate date:
        /// if the original date is before the posting period, the first available date in the posting period will be returned,
        /// otherwise the last possible date</param>
        /// <returns>false if date needed to change</returns>
        public static bool GetLedgerDatePostingPeriod(Int32 ALedgerNumber,
            ref DateTime ADateToTest,
            out Int32 AFinancialYear,
            out Int32 AFinancialPeriod,
            TDBTransaction ATransaction,
            bool ADoFixDate)
        {
            AFinancialPeriod = -1;
            AFinancialYear = -1;
            AAccountingPeriodRow currentPeriodRow = null;
            AAccountingPeriodRow lastAllowedPeriodRow = null;
            AAccountingPeriodTable table = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, ATransaction);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);

            if (LedgerTable.Count < 1)
            {
                throw new Exception("Ledger " + ALedgerNumber + " not found");
            }

            int ACurrentPeriod = LedgerTable[0].CurrentPeriod;
            int AAllowedForwardPeriod = ACurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods;

            foreach (AAccountingPeriodRow row in table.Rows)
            {
                if (row.AccountingPeriodNumber == ACurrentPeriod)
                {
                    currentPeriodRow = row;
                }

                if (row.AccountingPeriodNumber == AAllowedForwardPeriod)
                {
                    lastAllowedPeriodRow = row;
                }

                if ((row.PeriodStartDate <= ADateToTest) && (ADateToTest <= row.PeriodEndDate))
                {
                    // check if this period is either the current period or one of the forward posting periods
                    if (LedgerTable.Count == 1)
                    {
                        AFinancialPeriod = row.AccountingPeriodNumber;

                        //This is the number of the period to which the "DateToTest" belongs
                        //This can be
                        // 1.) before the current period or in the last financial year
                        //   =>  FIX Date to be the first day of the current period
                        // 2.) greater oder eqal  currentperiod but within AllowedForwardperiod = no FIX required
                        // 3.) after the allowed Forward period or even in a future financial year: = FIX Date to be the last day of the last allowed forward period
                        if ((AFinancialPeriod >= ACurrentPeriod) && (AFinancialPeriod <= AAllowedForwardPeriod))
                        {
                            AFinancialYear = LedgerTable[0].CurrentFinancialYear;
                            return true;
                        }
                    }
                }
            }

            if (ADoFixDate)
            {
                if (ADateToTest < currentPeriodRow.PeriodStartDate)
                {
                    ADateToTest = currentPeriodRow.PeriodStartDate;
                    AFinancialYear = LedgerTable[0].CurrentFinancialYear;
                    AFinancialPeriod = currentPeriodRow.AccountingPeriodNumber;
                }
                else
                {
                    if (lastAllowedPeriodRow == null)
                    {
                        lastAllowedPeriodRow = table[table.Rows.Count - 1];
                    }

                    if (ADateToTest > lastAllowedPeriodRow.PeriodEndDate)
                    {
                        ADateToTest = lastAllowedPeriodRow.PeriodEndDate;
                        AFinancialYear = LedgerTable[0].CurrentFinancialYear;
                        AFinancialPeriod = lastAllowedPeriodRow.AccountingPeriodNumber;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// check if the given date is in one of the open accounting periods of the given ledger, ie current or forward posting periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateToTest"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AYearNr"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        public static bool IsValidPostingPeriod(Int32 ALedgerNumber,
            DateTime ADateToTest,
            out Int32 APeriodNumber,
            out Int32 AYearNr,
            TDBTransaction ATransaction)
        {
            return GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateToTest, out AYearNr, out APeriodNumber, ATransaction, false);
        }

        /// <summary>
        /// get the start and end date of the given period in the current year
        /// </summary>
        public static bool GetStartAndEndDateOfPeriod(Int32 ALedgerNumber,
            Int32 APeriodNumber,
            out DateTime APeriodStartDate,
            out DateTime APeriodEndDate,
            TDBTransaction ATransaction)
        {
            // invalid period
            if (APeriodNumber == -1)
            {
                APeriodStartDate = DateTime.MinValue;
                APeriodEndDate = DateTime.MaxValue;
                return false;
            }

            AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, ATransaction);
            AAccountingPeriodRow AccPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];

            APeriodStartDate = AccPeriodRow.PeriodStartDate;
            APeriodEndDate = AccPeriodRow.PeriodEndDate;

            return true;
        }

        /// <summary>
        /// check if the given date is in the given accounting period of the given ledger, one of the current or forward posting periods
        /// </summary>
        public static bool IsInValidPostingPeriod(Int32 ALedgerNumber,
            DateTime ADateToTest,
            int APeriodNumberToTest,
            TDBTransaction ATransaction)
        {
            int YearNr;
            int PeriodNumber;

            if (GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateToTest, out YearNr, out PeriodNumber, ATransaction, false))
            {
                return PeriodNumber == APeriodNumberToTest;
            }

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
            TCarryForward carryForward = new TCarryForward(new TLedgerInfo(ALedgerNumber));

            if (carryForward.GetPeriodType == TCarryForwardENum.Month)
            {
                carryForward.SetNextPeriod();
            }

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
            TCarryForward carryForward = new TCarryForward(new TLedgerInfo(ALedgerNumber));

            if (carryForward.GetPeriodType == TCarryForwardENum.Year)
            {
                carryForward.SetNextPeriod();
            }

            return false;
        }
    }
}