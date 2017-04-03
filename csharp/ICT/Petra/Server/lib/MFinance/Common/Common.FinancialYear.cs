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
using Ict.Common.Exceptions;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            AFinancialPeriod = -1;
            AFinancialYear = -1;
            AAccountingPeriodRow currentPeriodRow = null;
            AAccountingPeriodRow lastAllowedPeriodRow = null;

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, ATransaction);

            #region Validate Data

            if ((LedgerTable == null) || (LedgerTable.Count == 0))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true),
                        ALedgerNumber));
            }
            else if ((AccountingPeriodTable == null) || (AccountingPeriodTable.Count == 0))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - Accounting Period data for Ledger number {1} does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true),
                        ALedgerNumber));
            }

            #endregion Validate Data

            try
            {
                int aCurrentPeriod = LedgerTable[0].CurrentPeriod;
                int anAllowedForwardPeriod = aCurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods;

                foreach (AAccountingPeriodRow row in AccountingPeriodTable.Rows)
                {
                    if (row.AccountingPeriodNumber == aCurrentPeriod)
                    {
                        currentPeriodRow = row;
                    }

                    if (row.AccountingPeriodNumber == anAllowedForwardPeriod)
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
                            if ((AFinancialPeriod >= aCurrentPeriod) && (AFinancialPeriod <= anAllowedForwardPeriod))
                            {
                                AFinancialYear = LedgerTable[0].CurrentFinancialYear;
                                RetVal = true;
                                break;
                            }
                        }
                    }
                }

                if (ADoFixDate && !RetVal)
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
                            lastAllowedPeriodRow = AccountingPeriodTable[AccountingPeriodTable.Rows.Count - 1];
                        }

                        if (ADateToTest > lastAllowedPeriodRow.PeriodEndDate)
                        {
                            ADateToTest = lastAllowedPeriodRow.PeriodEndDate;
                            AFinancialYear = LedgerTable[0].CurrentFinancialYear;
                            AFinancialPeriod = lastAllowedPeriodRow.AccountingPeriodNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
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
        /// Get the start and end date of the given period in the given year
        /// </summary>
        public static bool GetStartAndEndDateOfPeriod(Int32 ALedgerNumber,
            Int32 AYear,
            Int32 APeriodNumber,
            out DateTime APeriodStartDate,
            out DateTime APeriodEndDate,
            TDBTransaction ATransaction)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            // ATransaction can be null
            //else if (ATransaction == null)
            //{
            //    throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
            //                "Function:{0} - Database Transaction must not be NULL!"),
            //            Utilities.GetMethodName(true)));
            //}

            #endregion Validate Arguments

            // invalid period
            if (APeriodNumber == -1)
            {
                APeriodStartDate = DateTime.MinValue;
                APeriodEndDate = DateTime.MaxValue;
                return false;
            }

            AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, ATransaction);

            #region Validate Data

            if ((AccPeriodTable == null) || (AccPeriodTable.Count == 0))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - Accounting Period data for period {1} in Ledger number {2} does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true),
                        APeriodNumber,
                        ALedgerNumber));
            }

            #endregion Validate Data

            AAccountingPeriodRow AccPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);
            Int32 currentYear = LedgerTable[0].CurrentFinancialYear;
            Int32 yearsAgo = currentYear - AYear;

            APeriodStartDate = AccPeriodRow.PeriodStartDate.AddYears(0 - yearsAgo);
            APeriodEndDate = AccPeriodRow.PeriodEndDate.AddYears(0 - yearsAgo);

            return true;
        }

        /// <summary>
        /// Get the start and end date of the given period in the current year
        /// </summary>
        public static bool GetStartAndEndDateOfPeriod(Int32 ALedgerNumber,
            Int32 APeriodNumber,
            out DateTime APeriodStartDate,
            out DateTime APeriodEndDate,
            TDBTransaction ATransaction)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);
            Int32 currentYear = LedgerTable[0].CurrentFinancialYear;

            return GetStartAndEndDateOfPeriod(ALedgerNumber, currentYear, APeriodNumber, out APeriodStartDate, out APeriodEndDate, ATransaction);
        }

        /// <summary>
        /// check if the given date is in the given accounting period of the given ledger, one of the current or forward posting periods
        /// </summary>
        public static bool IsInValidPostingPeriod(Int32 ALedgerNumber,
            DateTime ADateToTest,
            int APeriodNumberToTest,
            TDBTransaction ATransaction)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            int YearNr;
            int PeriodNumber;

            if (GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateToTest, out YearNr, out PeriodNumber, ATransaction, false))
            {
                return PeriodNumber == APeriodNumberToTest;
            }

            return false;
        }
    }
}