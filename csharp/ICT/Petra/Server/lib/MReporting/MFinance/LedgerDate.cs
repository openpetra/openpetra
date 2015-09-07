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
using Ict.Common;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// <summary>
    /// date and period and financial year functions
    /// from the finance point of view (ledger)
    ///
    /// whereever year is given as an integer parameter (whichYear),
    /// it is the same value as used in the database for a_year_i
    /// </summary>
    public class TRptUserFunctionsDate : TRptUserFunctions
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsDate() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptUserFunctionsDate(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// register functions here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            base.FunctionSelector(ASituation, f, ops, out value);

            if (StringHelper.IsSame(f, "getStatePeriod"))
            {
                value = new TVariant(GetStatePeriod(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "periodInLastYear"))
            {
                value = new TVariant(PeriodInLastYear(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "periodInThisYearOld"))
            {
                value = new TVariant(PeriodInThisYearOld(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "getQuarterOrPeriod"))
            {
                value = new TVariant(GetQuarterOrPeriod(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt(), ops[4].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "getPeriodStartDate"))
            {
                value = new TVariant(GetPeriodStartDate(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "getPeriodEndDate"))
            {
                value = new TVariant(GetPeriodEndDate(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "getYTDPeriod"))
            {
                value = new TVariant(GetYTDPeriod(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPreviousYearCaption"))
            {
                value = new TVariant(GetPreviousYearCaption());
                return true;
            }

            if (StringHelper.IsSame(f, "GetCurrentYearCaption"))
            {
                value = new TVariant(GetCurrentYearCaption());
                return true;
            }

            if (StringHelper.IsSame(f, "GetNextYearCaption"))
            {
                value = new TVariant(GetNextYearCaption());
                return true;
            }

            if (StringHelper.IsSame(f, "getMonthName"))
            {
                value = new TVariant(GetMonthName(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "getMonthDiff"))
            {
                value = new TVariant(GetMonthDiff(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt(), ops[4].ToInt()));
                return true;
            }

            value = new TVariant();
            return false;
        }

        /// <summary>
        /// Get the start and end date of a given period in a given ledger in the given year
        /// </summary>
        /// <returns>void</returns>
        public void GetPeriodDetails(int ledgernr, int period, out DateTime startOfPeriod, out DateTime endOfPeriod, int whichyear, int column)
        {
            int currentFinancialYear = parameters.Get("param_current_financial_year_i", column).ToInt();
            TFinancialPeriod financialPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), period, whichyear,
                situation.GetParameters(), situation.GetColumn());
            AAccountingPeriodTable tab = AAccountingPeriodAccess.LoadByPrimaryKey(ledgernr, period, situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count == 1)
            {
                AAccountingPeriodRow row = tab[0];

                try
                {
                    endOfPeriod = new DateTime(row.PeriodEndDate.Year - (currentFinancialYear - financialPeriod.realYear),
                        row.PeriodEndDate.Month,
                        row.PeriodEndDate.Day);
                }
                catch (Exception)
                {
                    endOfPeriod = new DateTime(row.PeriodEndDate.Year - (currentFinancialYear - financialPeriod.realYear),
                        row.PeriodEndDate.Month,
                        row.PeriodEndDate.Day - 1);
                }
                startOfPeriod = new DateTime(row.PeriodStartDate.Year - (currentFinancialYear - financialPeriod.realYear),
                    row.PeriodStartDate.Month,
                    row.PeriodStartDate.Day);
            }
            else
            {
                endOfPeriod = DateTime.MinValue;
                startOfPeriod = DateTime.MinValue;
            }

            financialPeriod = null;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="period"></param>
        /// <param name="startOfPeriod"></param>
        /// <param name="endOfPeriod"></param>
        /// <param name="whichyear"></param>
        public void GetPeriodDetails(int ledgernr, int period, out DateTime startOfPeriod, out DateTime endOfPeriod, int whichyear)
        {
            GetPeriodDetails(ledgernr, period, out startOfPeriod, out endOfPeriod, whichyear, -1);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="whichyear"></param>
        /// <param name="period">the period whose end date is requested</param>
        /// <param name="column"></param>
        /// <returns>the date of the last day in the period of the given year</returns>
        public DateTime GetEndDateOfPeriod(int ledgernr, int whichyear, int period, int column)
        {
            DateTime startOfPeriod;
            DateTime endOfPeriod;

            GetPeriodDetails(ledgernr, period, out startOfPeriod, out endOfPeriod, whichyear, column);
            return endOfPeriod;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="whichyear"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public DateTime GetEndDateOfPeriod(int ledgernr, int whichyear, int period)
        {
            return GetEndDateOfPeriod(ledgernr, whichyear, period, -1);
        }

        /// <summary>
        /// returns the date of the given period in the selected year (param_year_i) of the ledger
        /// </summary>
        /// <returns>returns the date of the given period in the selected year (param_year_i) of the ledger</returns>
        public DateTime GetPeriodEndDate(int ledgernr, int period, int column)
        {
            return GetEndDateOfPeriod(ledgernr, parameters.Get("param_year_i", column).ToInt(), period, column);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public DateTime GetPeriodEndDate(int ledgernr, int period)
        {
            return GetPeriodEndDate(ledgernr, period, -1);
        }

        /// <summary>get the date of the first day in the period of the given year
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="whichyear"></param>
        /// <param name="period">the period whose start date is requested</param>
        /// <param name="column"></param>
        /// <returns>the date of the first day in the period of the given year</returns>
        public DateTime GetStartDateOfPeriod(int ledgernr, int whichyear, int period, int column)
        {
            DateTime startOfPeriod;
            DateTime endOfPeriod;

            GetPeriodDetails(ledgernr, period, out startOfPeriod, out endOfPeriod, whichyear, column);
            return startOfPeriod;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="whichyear"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public DateTime GetStartDateOfPeriod(int ledgernr, int whichyear, int period)
        {
            return GetStartDateOfPeriod(ledgernr, whichyear, period, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="period"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public DateTime GetPeriodStartDate(int ledgernr, int period, int column)
        {
            return GetStartDateOfPeriod(ledgernr, parameters.Get("param_year_i", column).ToInt(), period, column);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public DateTime GetPeriodStartDate(int ledgernr, int period)
        {
            return GetPeriodStartDate(ledgernr, period, -1);
        }

        /// <summary>
        /// returns the date of the last day of the year
        /// </summary>
        /// <returns>returns the date of the last day of the year</returns>
        public DateTime GetEndDateOfYear(int ledgernr, int whichyear, int column)
        {
            return GetEndDateOfPeriod(ledgernr, whichyear, parameters.Get("param_number_of_accounting_periods_i", column).ToInt(), column);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="whichyear"></param>
        /// <returns></returns>
        public DateTime GetEndDateOfYear(int ledgernr, int whichyear)
        {
            return GetEndDateOfYear(ledgernr, whichyear, -1);
        }

        /// <summary>
        /// returns either "quarter xx, startdate to enddate" or "period xx: start date to end date"
        /// the quarter has to start in one period and end in another.
        /// if it is not a quarter, the start period must be equals to the end period
        /// side effect: it will set the parameters param_start_date and param_end_date
        ///
        /// </summary>
        /// <returns></returns>
        public String GetQuarterOrPeriod(int ledgernumber, int quarter, int start_period, int end_period)
        {
            String ReturnValue;
            TVariant reportytd;
            DateTime startDate;
            DateTime endDate;

            if (quarter > 0)
            {
                ReturnValue = "Quarter " + quarter.ToString();
                start_period = ((quarter - 1) * 3) + 1;
                end_period = (quarter * 3);
            }
            else
            {
                ReturnValue = "Period " + end_period.ToString();

                // we only want to cover one period
                if (start_period != end_period)
                {
                    throw new Exception("GetQuarterOrPeriod: start period and end period must be equal");
                }
            }

            ReturnValue = ReturnValue + ": ";
            reportytd = this.parameters.Get("param_ytd", -1, -1, eParameterFit.eExact);

            if ((!reportytd.IsZeroOrNull() && (reportytd.ToString() != "mixed")))
            {
                if (reportytd.ToBool() == true)
                {
                    startDate = GetPeriodStartDate(ledgernumber, 1);
                }
                else
                {
                    startDate = GetPeriodStartDate(ledgernumber, start_period);
                }
            }
            else
            {
                startDate = GetPeriodStartDate(ledgernumber, start_period);
            }

            endDate = GetPeriodEndDate(ledgernumber, end_period);
            ReturnValue = ReturnValue +
                          StringHelper.DateToLocalizedString(startDate) +
                          " to " +
                          StringHelper.DateToLocalizedString(endDate);
            this.parameters.Add("param_start_date", startDate);
            this.parameters.Add("param_end_date", endDate);
            return ReturnValue;
        }

        /// <summary>
        /// only used if mixed ytd columns are on report;
        /// then it shows: "startdate to enddate (YTD)"
        /// side effect: it will set the parameter param_start_date_ytd
        ///
        /// </summary>
        /// <returns></returns>
        public string GetYTDPeriod(int ledgernr, int end_period, String reportytd)
        {
            string ReturnValue = "";
            int start_period;
            int numberAccountingPeriods;

            if (reportytd == "mixed")
            {
                start_period = 1;
                // currentFinancialYear = parameters.Get("param_current_financial_year_i").ToInt();
                numberAccountingPeriods = parameters.Get("param_number_of_accounting_periods_i").ToInt();

                if (end_period > numberAccountingPeriods)
                {
                    start_period = numberAccountingPeriods + 1;
                }

                this.parameters.Add("param_start_date_ytd", GetPeriodStartDate(ledgernr, start_period));
                ReturnValue =
                    StringHelper.DateToLocalizedString(GetPeriodStartDate(ledgernr, start_period)) + " to " + StringHelper.DateToLocalizedString(
                        GetPeriodEndDate(ledgernr, end_period)) + " (YTD)";
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns status CLOSED, FWD PERIOD, or CURRENT for the given period in the given year of the ledger
        /// </summary>
        /// <returns></returns>
        public string GetStatePeriod(int ledgernumber, int whichyear, int period)
        {
            string ReturnValue;
            int currentPeriod;
            int currentYear;
            TFinancialPeriod financialPeriod;

            currentPeriod = parameters.Get("param_current_period_i").ToInt();
            currentYear = parameters.Get("param_current_financial_year_i").ToInt();
            financialPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), period, whichyear,
                situation.GetParameters(), situation.GetColumn());

            if (financialPeriod.realYear < currentYear)
            {
                ReturnValue = "CLOSED";
            }
            else if (financialPeriod.realYear > currentYear)
            {
                ReturnValue = "FWD PERIOD";
            }
            else
            {
                if (currentPeriod == financialPeriod.realPeriod)
                {
                    ReturnValue = "CURRENT";
                }
                else if (currentPeriod > financialPeriod.realPeriod)
                {
                    ReturnValue = "CLOSED";
                }
                else
                {
                    ReturnValue = "FWD PERIOD";
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Boolean PeriodInLastYear(int period, int year)
        {
            return (period == -1) || (year == -1) || (year != parameters.Get("param_current_financial_year_i").ToInt());
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Boolean PeriodInThisYearOld(int period, int year)
        {
            return (period == -1) || (year == -1)
                   || (period < parameters.Get("param_current_period_i").ToInt() && (year == parameters.Get("param_current_financial_year_i").ToInt()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ledgernr"></param>
        /// <param name="periodnr"></param>
        /// <param name="numberPeriods"></param>
        /// <param name="diffyear"></param>
        /// <returns></returns>
        public string GetColumnCaptionYear(int ledgernr, int periodnr, int numberPeriods, int diffyear)
        {
            string ReturnValue;
            DateTime endDate;
            int currentYear;

            currentYear = parameters.Get("param_current_financial_year_i").ToInt();

            if (periodnr > numberPeriods)
            {
                diffyear = diffyear + 1;
            }

            diffyear = diffyear + parameters.Get("param_year_i").ToInt();
            endDate = GetEndDateOfYear(ledgernr, currentYear);

            //TODO: Calendar vs Financial Date Handling - Check if this should use financial year start/end and not assume calendar
            if ((endDate.Month == 12) && (endDate.Day == 31))
            {
                ReturnValue = Convert.ToString(endDate.Year + diffyear - currentYear);
            }
            else
            {
                ReturnValue = Convert.ToString(endDate.Year - 1 + diffyear - currentYear) + "-" + Convert.ToString(
                    endDate.Year + diffyear - currentYear);
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns the year number of the year previous to the currently selected year (param_year).
        /// If the ledger has a strange calendar setup, that also could be "2000/2001"
        ///
        /// </summary>
        /// <returns></returns>
        public String GetCurrentYearCaption()
        {
            return GetColumnCaptionYear(
                parameters.Get("param_ledger_number_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_end_period_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt(), 0);
        }

        /// <summary>
        /// returns the name of the month
        /// </summary>
        /// <returns></returns>
        public String GetMonthName(int ledgernr, int periodnr)
        {
            System.DateTime startDate;
            startDate = GetPeriodStartDate(ledgernr, periodnr);
            return StringHelper.GetLongMonthName(startDate.Month);
        }

        /// <summary>
        /// returns the year number of the currently selected year (param_year).
        /// </summary>
        /// <returns></returns>
        public String GetPreviousYearCaption()
        {
            return GetColumnCaptionYear(
                parameters.Get("param_ledger_number_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_end_period_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt(),
                -1);
        }

        /// <summary>
        /// returns the year number of the year after the currently selected year (param_year).
        /// </summary>
        /// <returns></returns>
        public String GetNextYearCaption()
        {
            return GetColumnCaptionYear(
                parameters.Get("param_ledger_number_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_end_period_i", situation.GetColumn()).ToInt(),
                parameters.Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt(),
                1);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetMonthDiff(DateTime startDate, DateTime endDate)
        {
            //TODO: Calendar vs Financial Date Handling - Check if this should use financial year start/end and not assume calendar
            return (endDate.Year) * 12 + endDate.Month - (startDate.Month * 12 + startDate.Month) + 1;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ledgerNumber"></param>
        /// <param name="year"></param>
        /// <param name="startPeriod"></param>
        /// <param name="endPeriod"></param>
        /// <returns></returns>
        public int GetMonthDiff(int ledgerNumber, int year, int startPeriod, int endPeriod)
        {
            return GetMonthDiff(GetStartDateOfPeriod(ledgerNumber, year, startPeriod), GetEndDateOfPeriod(ledgerNumber, year, endPeriod));
        }
    }
}