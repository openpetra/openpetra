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
using System.Data;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Cacheable;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance GL screens
    ///</summary>
    public class TAccountingPeriodsWebConnector
    {
        /// <summary>
        /// retrieve the start and end dates of the current period of the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetCurrentPeriodDates(Int32 ALedgerNumber, out DateTime AStartDate, out DateTime AEndDate)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod,
                Transaction);

            AStartDate = AccountingPeriodTable[0].PeriodStartDate;
            AEndDate = AccountingPeriodTable[0].PeriodEndDate;

            DBAccess.GDBAccessObj.RollbackTransaction();

            return true;
        }

        /// <summary>
        /// Get the valid dates for posting;
        /// based on current period and number of forwarding periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            bool newTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out newTransaction);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod,
                Transaction);

            AStartDateCurrentPeriod = AccountingPeriodTable[0].PeriodStartDate;

            AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods,
                Transaction);
            AEndDateLastForwardingPeriod = AccountingPeriodTable[0].PeriodEndDate;

            if (newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return true;
        }

        /// <summary>
        /// get the real period stored in the database
        /// this is needed for reports that run on a different financial year, ahead or behind by several months
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetRealPeriod(
            System.Int32 ALedgerNumber,
            System.Int32 ADiffPeriod,
            System.Int32 AYear,
            System.Int32 APeriod,
            out System.Int32 ARealPeriod,
            out System.Int32 ARealYear)
        {
            ARealPeriod = APeriod + ADiffPeriod;
            ARealYear = AYear;

            if (ADiffPeriod == 0)
            {
                return true;
            }

            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable Ledger = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            // the period is in the last year
            // this treatment only applies to situations with different financial years.
            // in a financial year equals to the glm year, the period 0 represents the start balance
            if ((ADiffPeriod == 0) && (ARealPeriod == 0))
            {
                //do nothing
            }
            else if (ARealPeriod < 1)
            {
                ARealPeriod = Ledger[0].NumberOfAccountingPeriods + ARealPeriod;
                ARealYear = ARealYear - 1;
            }

            // forwarding periods are only allowed in the current year
            if ((ARealPeriod > Ledger[0].NumberOfAccountingPeriods) && (ARealYear != Ledger[0].CurrentFinancialYear))
            {
                ARealPeriod = ARealPeriod - Ledger[0].NumberOfAccountingPeriods;
                ARealYear = ARealYear + 1;
            }

            return true;
        }

        /// <summary>
        /// get the number of periods for the Ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 GetNumberOfPeriods(
            System.Int32 ALedgerNumber)
        {
            Int32 returnValue = 0;

            bool newTransaction = false;

            try
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out newTransaction);

                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                returnValue = LedgerTable[0].NumberOfAccountingPeriods;
            }
            finally
            {
                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// get the start date of the given period
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static System.DateTime GetPeriodStartDate(
            System.Int32 ALedgerNumber,
            System.Int32 AYear,
            System.Int32 ADiffPeriod,
            System.Int32 APeriod)
        {
            System.Int32 RealYear = 0;
            System.Int32 RealPeriod = 0;
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DateTime ReturnValue = DateTime.Now;
            GetRealPeriod(ALedgerNumber, ADiffPeriod, AYear, APeriod, out RealPeriod, out RealYear);
            DataTable CachedDataTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);
            string whereClause = AAccountingPeriodTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() + " and " +
                                 AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " = " + RealPeriod.ToString();
            DataRow[] filteredRows = CachedDataTable.Select(whereClause);

            if (filteredRows.Length > 0)
            {
                ReturnValue = ((AAccountingPeriodRow)filteredRows[0]).PeriodStartDate;

                ALedgerTable Ledger = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                    "",
                    false,
                    ALedgerNumber,
                    out typeofTable);

                ReturnValue = ReturnValue.AddYears(RealYear - Ledger[0].CurrentFinancialYear);
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the end date of the given period
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static System.DateTime GetPeriodEndDate(Int32 ALedgerNumber, System.Int32 AYear, System.Int32 ADiffPeriod, System.Int32 APeriod)
        {
            System.Int32 RealYear = 0;
            System.Int32 RealPeriod = 0;
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DateTime ReturnValue = DateTime.Now;
            GetRealPeriod(ALedgerNumber, ADiffPeriod, AYear, APeriod, out RealPeriod, out RealYear);
            DataTable CachedDataTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);
            string whereClause = AAccountingPeriodTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() + " and " +
                                 AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " = " + RealPeriod.ToString();
            DataRow[] filteredRows = CachedDataTable.Select(whereClause);

            if (filteredRows.Length > 0)
            {
                ReturnValue = ((AAccountingPeriodRow)filteredRows[0]).PeriodEndDate;

                ALedgerTable Ledger = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                    "",
                    false,
                    ALedgerNumber,
                    out typeofTable);

                ReturnValue = ReturnValue.AddYears(RealYear - Ledger[0].CurrentFinancialYear);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the start date and end date
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYearNumber"></param>
        /// <param name="ADiffPeriod"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AStartDatePeriod"></param>
        /// <param name="AEndDatePeriod"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetPeriodDates(Int32 ALedgerNumber,
            Int32 AYearNumber,
            Int32 ADiffPeriod,
            Int32 APeriodNumber,
            out DateTime AStartDatePeriod,
            out DateTime AEndDatePeriod)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, Transaction);

            // TODO: ADiffPeriod for support of different financial years

            AStartDatePeriod = AccountingPeriodTable[0].PeriodStartDate;
            AEndDatePeriod = AccountingPeriodTable[0].PeriodEndDate;

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AStartDatePeriod = AStartDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));
            AEndDatePeriod = AEndDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));

            DBAccess.GDBAccessObj.RollbackTransaction();

            return true;
        }

        /// <summary>
        /// Get the accounting year for the given date
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADate"></param>
        /// <param name="AYearNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetAccountingYearByDate(Int32 ALedgerNumber,
            DateTime ADate,
            out Int32 AYearNumber)
        {
            //Set the year to return
            AYearNumber = FindFinancialYearByDate(ALedgerNumber, ADate);

            if (AYearNumber != 99)
            {
                return true;
            }
            else
            {
                AYearNumber = 0;
                return false;
            }
        }

        private static Int32 FindFinancialYearByDate(Int32 ALedgerNumber,
            DateTime ADate)
        {
            Int32 yearDateBelongsTo;
            DateTime yearStartDate = DateTime.Today;
            bool newTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out newTransaction);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            if ((LedgerTable == null) || (LedgerTable.Count == 0))
            {
                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                return 99;
            }

            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];
            yearDateBelongsTo = LedgerRow.CurrentFinancialYear;

            AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, Transaction);

            if ((AccPeriodTable == null) || (AccPeriodTable.Count == 0))
            {
                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                return 99;
            }

            //Find earliest start date (don't assume PK order)
            AAccountingPeriodRow AccPeriodRow = null;

            for (int i = 0; i < AccPeriodTable.Count; i++)
            {
                DateTime currentStartDate;

                AccPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[i];
                currentStartDate = AccPeriodRow.PeriodStartDate;

                if (i > 0)
                {
                    if (yearStartDate > currentStartDate)
                    {
                        yearStartDate = currentStartDate;
                    }
                }
                else
                {
                    yearStartDate = currentStartDate;
                }
            }

            //Find the correct year
            while (ADate < yearStartDate)
            {
                ADate = ADate.AddYears(1);
                yearDateBelongsTo--;
            }

            if (newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            //Set the year to return
            return yearDateBelongsTo;
        }

        /// <summary>
        /// Get the azccounting year and period for a given date
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADate"></param>
        /// <param name="AYearNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber,
            DateTime ADate,
            out Int32 AYearNumber,
            out Int32 APeriodNumber)
        {
            bool newTransaction;

            //Set the year to return
            AYearNumber = FindFinancialYearByDate(ALedgerNumber, ADate);

            if (AYearNumber == 99)
            {
                AYearNumber = 0;
                APeriodNumber = 0;
                return false;
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out newTransaction);

            AAccountingPeriodTable AccPeriodTableTmp = new AAccountingPeriodTable();
            AAccountingPeriodRow TemplateRow = AccPeriodTableTmp.NewRowTyped(false);

            TemplateRow.LedgerNumber = ALedgerNumber;
            TemplateRow.PeriodStartDate = ADate.AddYears(-AYearNumber);
            TemplateRow.PeriodEndDate = ADate.AddYears(-AYearNumber);

            StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "<=", ">=" });

            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadUsingTemplate(TemplateRow,
                operators,
                null,
                Transaction);

            if ((AccountingPeriodTable == null) || (AccountingPeriodTable.Count == 0))
            {
                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                APeriodNumber = 0;
                return false;
            }

            AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

            APeriodNumber = AccountingPeriodRow.AccountingPeriodNumber;

            if (newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return true;
        }

        /// <summary>
        /// Loads all available years with GL data into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAvailableGLYears(Int32 ALedgerNumber,
            System.Int32 ADiffPeriod,
            bool AIncludeNextYear,
            out String ADisplayMember, out String AValueMember)
        {
            ADisplayMember = "YearDate";
            AValueMember = "YearNumber";
            DataTable datTable = new DataTable();
            datTable.Columns.Add(AValueMember, typeof(System.Int32));
            datTable.Columns.Add(ADisplayMember, typeof(String));
            datTable.PrimaryKey = new DataColumn[] {
                datTable.Columns[0]
            };

            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable LedgerTable = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            DateTime currentYearEnd = GetPeriodEndDate(
                ALedgerNumber,
                LedgerTable[0].CurrentFinancialYear,
                ADiffPeriod,
                LedgerTable[0].NumberOfAccountingPeriods);

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                // add the years, which are retrieved by reading from the GL batch tables
                string sql =
                    String.Format("SELECT DISTINCT {0} AS availYear " + " FROM PUB_{1} " + " WHERE {2} = " +
                        ALedgerNumber.ToString() + " ORDER BY 1 DESC",
                        ABatchTable.GetBatchYearDBName(),
                        ABatchTable.GetTableDBName(),
                        ABatchTable.GetLedgerNumberDBName());

                DataTable BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", ReadTransaction);

                foreach (DataRow row in BatchYearTable.Rows)
                {
                    DataRow resultRow = datTable.NewRow();
                    resultRow[0] = row[0];
                    resultRow[1] = currentYearEnd.AddYears(-1 * (LedgerTable[0].CurrentFinancialYear - Convert.ToInt32(row[0]))).ToString("yyyy");
                    datTable.Rows.Add(resultRow);
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // we should also check if the current year has been added, in case there are no batches yet
            if (datTable.Rows.Find(LedgerTable[0].CurrentFinancialYear) == null)
            {
                DataRow resultRow = datTable.NewRow();
                resultRow[0] = LedgerTable[0].CurrentFinancialYear;
                resultRow[1] = currentYearEnd.ToString("yyyy");
                datTable.Rows.InsertAt(resultRow, 0);
            }

            if (AIncludeNextYear && (null == datTable.Rows.Find(LedgerTable[0].CurrentFinancialYear + 1)))
            {
                DataRow resultRow = datTable.NewRow();
                resultRow[0] = LedgerTable[0].CurrentFinancialYear + 1;
                resultRow[1] = currentYearEnd.AddYears(1).ToString("yyyy");
                datTable.Rows.Add(resultRow);
            }

            return datTable;
        }

        /// <summary>
        ///    Get the available financial years from the existing batches
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADisplayMember"></param>
        /// <param name="AValueMember"></param>
        /// <param name="ADescriptionMember"></param>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAvailableGLYearsHOSA(Int32 ALedgerNumber,
            out String ADisplayMember, out String AValueMember, out String ADescriptionMember)
        {
            string RetVal = string.Empty;

            DateTime YearEndDate;
            int YearNumber;

            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            AAccountingPeriodTable AccountingPeriods = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, Transaction);

            if (LedgerTable.Count < 1)
            {
                throw new Exception("Ledger " + ALedgerNumber + " not found");
            }

            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable[0];

            AccountingPeriods.DefaultView.RowFilter = String.Format("{0}={1}",
                AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                LedgerRow.NumberOfAccountingPeriods);

            AAccountingPeriodRow periodRow = null;

            foreach (DataRowView perRow in AccountingPeriods.DefaultView)
            {
                periodRow = (AAccountingPeriodRow)perRow.Row;
                break;
            }

            YearNumber = LedgerRow.CurrentFinancialYear;
            RetVal = periodRow.PeriodEndDate + "," + YearNumber.ToString();
            YearNumber -= 1;
            YearEndDate = DecrementYear(periodRow.PeriodEndDate);

            //Retrieve all previous years
            string sql =
                String.Format("SELECT DISTINCT {0} AS batchYear" +
                    " FROM PUB_{1}" +
                    " WHERE {2} = {3} And {0} < {4}" +
                    " ORDER BY 1 DESC",
                    ABatchTable.GetBatchYearDBName(),
                    ABatchTable.GetTableDBName(),
                    ABatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    YearNumber);

            ADisplayMember = "YearEndDate";
            AValueMember = "YearNumber";
            ADescriptionMember = "YearEndDateLong";

            DataTable BatchTable = new DataTable();
            BatchTable.Columns.Add(AValueMember, typeof(System.Int32));
            BatchTable.Columns.Add(ADisplayMember, typeof(String));
            BatchTable.Columns.Add(ADescriptionMember, typeof(String));
            BatchTable.PrimaryKey = new DataColumn[] {
                BatchTable.Columns[0]
            };

            DataTable BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", Transaction);

            BatchYearTable.DefaultView.Sort = String.Format("batchYear DESC");

            YearNumber = LedgerRow.CurrentFinancialYear;
            YearEndDate = periodRow.PeriodEndDate;

            DataRow resultRow = BatchTable.NewRow();
            resultRow[0] = YearNumber;
            resultRow[1] = YearEndDate.ToShortDateString();
            resultRow[2] = YearEndDate.ToLongDateString();
            BatchTable.Rows.Add(resultRow);

            foreach (DataRowView row in BatchYearTable.DefaultView)
            {
                DataRow currentBatchYearRow = row.Row;

                Int32 currentBatchYear = (Int32)currentBatchYearRow[0];

                if (YearNumber != currentBatchYear)
                {
                    YearNumber -= 1;
                    YearEndDate = DecrementYear(YearEndDate);

                    if (YearNumber != currentBatchYear)
                    {
                        throw new Exception(String.Format(Catalog.GetString("Year {0} not found for Ledger {1}"),
                                YearNumber,
                                ALedgerNumber));
                    }
                }

                DataRow resultRow2 = BatchTable.NewRow();
                resultRow2[0] = YearNumber;
                resultRow2[1] = YearEndDate.ToShortDateString();
                resultRow2[2] = YearEndDate.ToLongDateString();
                BatchTable.Rows.Add(resultRow2);
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return BatchTable;
        }

        /// <summary>
        /// Returns the date 1 year prior to the input date, accounting for leap years
        /// </summary>
        /// <param name="ADate"></param>
        /// <returns>Input date minus one year</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DateTime DecrementYear(DateTime ADate)
        {
            DateTime RetVal;

            int iYear = ADate.Year;
            DateTime Date1 = Convert.ToDateTime("28-Feb-" + iYear.ToString());

            if ((DateTime.IsLeapYear(iYear) && (ADate > Date1)) || (DateTime.IsLeapYear(iYear - 1) && (ADate < Date1)))
            {
                RetVal = ADate.AddDays(-366);
            }
            else
            {
                RetVal = ADate.AddDays(-365);
            }

            return RetVal;
        }
    }
}