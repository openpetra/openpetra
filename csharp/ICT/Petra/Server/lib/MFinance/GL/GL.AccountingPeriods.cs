//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.GL.Data;

using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.GL.Data.Access;


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
            TDBTransaction transaction = null;
            AAccountingPeriodTable AccountingPeriodTable = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, ref transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);
                    AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        LedgerTable[0].CurrentPeriod,
                        transaction);
                });
            AStartDate = AccountingPeriodTable[0].PeriodStartDate;
            AEndDate = AccountingPeriodTable[0].PeriodEndDate;
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
        public static Boolean GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            //Call this rather than duplicate code
            return TFinanceServerLookups.GetCurrentPostingRangeDates(ALedgerNumber, out AStartDateCurrentPeriod, out AEndDateLastForwardingPeriod);
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
        public static Int32 GetNumberOfPeriods(Int32 ALedgerNumber)
        {
            Int32 returnValue = 0;
            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable, ref transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);
                    returnValue = LedgerTable[0].NumberOfAccountingPeriods;
                });

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
        public static System.DateTime GetPeriodEndDate(Int32 ALedgerNumber, System.Int32 AYear, System.Int32 ADiffPeriod,
            System.Int32 APeriod)
        {
            return GetPeriodEndDate(ALedgerNumber, AYear, ADiffPeriod, APeriod, null);
        }

        /// <summary>
        /// get the end date of the given period
        /// </summary>
        [NoRemoting]
        public static System.DateTime GetPeriodEndDate(Int32 ALedgerNumber, System.Int32 AYear, System.Int32 ADiffPeriod,
            System.Int32 APeriod, TDataBase ADataBase)
        {
            System.Int32 RealYear = 0;
            System.Int32 RealPeriod = 0;
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DateTime ReturnValue = DateTime.Now;
            GetRealPeriod(ALedgerNumber, ADiffPeriod, AYear, APeriod, out RealPeriod, out RealYear);
            DataTable UntypedTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                ALedgerNumber,
                out typeofTable, ADataBase);
            AAccountingPeriodTable CachedDataTable = (AAccountingPeriodTable)UntypedTable;
            CachedDataTable.DefaultView.RowFilter = AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " = " + RealPeriod;

            if (CachedDataTable.DefaultView.Count > 0)
            {
                ReturnValue = ((AAccountingPeriodRow)CachedDataTable.DefaultView[0].Row).PeriodEndDate;

                ALedgerTable Ledger = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                    "",
                    false,
                    ALedgerNumber,
                    out typeofTable, ADataBase);

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
            if ((AYearNumber < 0) || (APeriodNumber < 0))
            {
                AStartDatePeriod = DateTime.MinValue;
                AEndDatePeriod = DateTime.MinValue;
                return false;
            }

            DateTime StartDatePeriod = new DateTime();
            DateTime EndDatePeriod = new DateTime();
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, Transaction);

                    // TODO: ADiffPeriod for support of different financial years

                    StartDatePeriod = AccountingPeriodTable[0].PeriodStartDate;
                    EndDatePeriod = AccountingPeriodTable[0].PeriodEndDate;

                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                    //TODO: Calendar vs Financial Date Handling - Should this be number of accounting periods rather than 12
                    StartDatePeriod = StartDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));
                    EndDatePeriod = EndDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));
                });

            AStartDatePeriod = StartDatePeriod;
            AEndDatePeriod = EndDatePeriod;

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

        private static Int32 FindFinancialYearByDate(Int32 ALedgerNumber, DateTime ADate)
        {
            Int32 yearDateBelongsTo = 99;
            DateTime yearStartDate = DateTime.Today;

            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable, ref transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);

                    if (LedgerTable.Count == 0)
                    {
                        return;
                    }

                    ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];
                    yearDateBelongsTo = LedgerRow.CurrentFinancialYear;

                    AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, transaction);

                    if (AccPeriodTable.Count == 0)
                    {
                        return;
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
                }); // Get NewOrExisting AutoReadTransaction
            //Set the year to return
            return yearDateBelongsTo;
        } // Find FinancialYear ByDate

        /// <summary>
        /// Get the accounting year and period for a given date
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
            Int32 CurrentFinancialYear;

            //Set the year to return
            Int32 YearNumber = FindFinancialYearByDate(ALedgerNumber, ADate);

            AYearNumber = YearNumber;

            if (AYearNumber == 99)
            {
                AYearNumber = 0;
                APeriodNumber = 0;
                return false;
            }

            Int32 PeriodNumber = 0;

            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable, ref transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);

                    CurrentFinancialYear = ((ALedgerRow)LedgerTable.Rows[0]).CurrentFinancialYear;

                    AAccountingPeriodTable AccPeriodTableTmp = new AAccountingPeriodTable();
                    AAccountingPeriodRow TemplateRow = AccPeriodTableTmp.NewRowTyped(false);

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.PeriodStartDate = ADate.AddYears(CurrentFinancialYear - YearNumber);
                    TemplateRow.PeriodEndDate = ADate.AddYears(CurrentFinancialYear - YearNumber);

                    StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "<=", ">=" });

                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadUsingTemplate(TemplateRow,
                        operators,
                        null,
                        transaction);

                    if (AccountingPeriodTable.Count == 0)
                    {
                        return;
                    }

                    AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                    PeriodNumber = AccountingPeriodRow.AccountingPeriodNumber;
                });

            APeriodNumber = PeriodNumber;
            return true;
        } // Get AccountingYear Period ByDate

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
            TDBTransaction ReadTransaction = null;
            ALedgerTable LedgerTable = null;

            DataTable BatchYearTable = null;
            DateTime CurrentYearEnd = DateTime.MinValue;

            ADisplayMember = "YearDate";
            AValueMember = "YearNumber";
            String YearEnd = "YearEnd";
            DataTable DatTable = new DataTable();
            DatTable.Columns.Add(AValueMember, typeof(System.Int32));
            DatTable.Columns.Add(ADisplayMember, typeof(String));
            DatTable.Columns.Add(YearEnd, typeof(String));
            DatTable.PrimaryKey = new DataColumn[] {
                DatTable.Columns[0]
            };

            System.Type TypeOfTable = null;
            TCacheable CachePopulator = new TCacheable();

            // add the years, which are retrieved by reading from the GL batch tables
            string sql =
                String.Format("SELECT DISTINCT {0} AS availYear " + " FROM PUB_{1} " + " WHERE {2} = " +
                    ALedgerNumber.ToString() + " ORDER BY 1 DESC",
                    ABatchTable.GetBatchYearDBName(),
                    ABatchTable.GetTableDBName(),
                    ABatchTable.GetLedgerNumberDBName());

            // Automatic handling of a Read-only DB Transaction - and also the automatic establishment and closing of a DB
            // Connection where a DB Transaction can be exectued (only if that should be needed).
            DBAccess.SimpleAutoReadTransactionWrapper(IsolationLevel.ReadCommitted,
                "TAccountingPeriodsWebConnector.GetAvailableGLYears", out ReadTransaction,
                delegate
                {
                    LedgerTable = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                        "",
                        false,
                        ALedgerNumber,
                        out TypeOfTable, ReadTransaction.DataBaseObj);

                    CurrentYearEnd = GetPeriodEndDate(
                        ALedgerNumber,
                        LedgerTable[0].CurrentFinancialYear,
                        ADiffPeriod,
                        LedgerTable[0].NumberOfAccountingPeriods, ReadTransaction.DataBaseObj);

                    BatchYearTable = DBAccess.GetDBAccessObj(ReadTransaction).SelectDT(sql, "BatchYearTable", ReadTransaction);
                });

            foreach (DataRow row in BatchYearTable.Rows)
            {
                DataRow resultRow = DatTable.NewRow();
                DateTime SelectableYear = CurrentYearEnd.AddYears(-1 * (LedgerTable[0].CurrentFinancialYear - Convert.ToInt32(row[0])));
                resultRow[0] = row[0];
                resultRow[1] = SelectableYear.ToString("yyyy");
                resultRow[2] = SelectableYear.ToString("dd-MMM-yyyy");
                DatTable.Rows.Add(resultRow);
            }

            // we should also check if the current year has been added, in case there are no batches yet
            if (DatTable.Rows.Find(LedgerTable[0].CurrentFinancialYear) == null)
            {
                DataRow resultRow = DatTable.NewRow();
                resultRow[0] = LedgerTable[0].CurrentFinancialYear;
                resultRow[1] = CurrentYearEnd.ToString("yyyy");
                resultRow[2] = CurrentYearEnd.ToString("dd-MMM-yyyy");
                DatTable.Rows.InsertAt(resultRow, 0);
            }

            if (AIncludeNextYear && (DatTable.Rows.Find(LedgerTable[0].CurrentFinancialYear + 1) == null))
            {
                DataRow resultRow = DatTable.NewRow();
                resultRow[0] = LedgerTable[0].CurrentFinancialYear + 1;
                resultRow[1] = CurrentYearEnd.AddYears(1).ToString("yyyy");
                resultRow[2] = CurrentYearEnd.ToString("dd-MMM-yyyy");
                DatTable.Rows.InsertAt(resultRow, 0);
            }

            return DatTable;
        }

        /// <summary>
        /// Loads all available years with GL data into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAvailableGLYearEnds(Int32 ALedgerNumber,
            Int32 ADiffPeriod,
            bool AIncludeNextYear,
            out String ADisplayMember,
            out String AValueMember)
        {
            const string INTL_DATE_FORMAT = "yyyy-MM-dd";

            //Create the table to populate the combobox
            DataTable ReturnTable = null;

            AValueMember = "YearNumber";
            ADisplayMember = "YearEndDate";

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        DateTime yearEndDate;
                        DateTime currentYearEndDate;
                        int yearNumber;
                        int currentFinancialYear;

                        ALedgerTable LedgerTable = null;
                        AAccountingPeriodTable AccountingPeriods = null;

                        LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                        AccountingPeriods = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((LedgerTable == null) || (LedgerTable.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }
                        else if ((AccountingPeriods == null) || (AccountingPeriods.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - AAccount Period data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        ALedgerRow ledgerRow = (ALedgerRow)LedgerTable[0];

                        currentYearEndDate = GetPeriodEndDate(ALedgerNumber,
                            ledgerRow.CurrentFinancialYear,
                            ADiffPeriod,
                            ledgerRow.NumberOfAccountingPeriods);

                        currentFinancialYear = ledgerRow.CurrentFinancialYear;

                        //Filter to highest period number
                        AccountingPeriods.DefaultView.RowFilter = String.Format("{0}={1}",
                            AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                            ledgerRow.NumberOfAccountingPeriods);

                        //Get last period row
                        AAccountingPeriodRow periodRow = (AAccountingPeriodRow)AccountingPeriods.DefaultView[0].Row;

                        //Create the table to populate the combobox
                        ReturnTable = new DataTable();
                        ReturnTable.Columns.Add("YearNumber", typeof(System.Int32));
                        ReturnTable.Columns.Add("YearEndDate", typeof(String));
                        ReturnTable.Columns.Add("YearEndDateLong", typeof(String));
                        ReturnTable.PrimaryKey = new DataColumn[] { ReturnTable.Columns[0] };

                        //Add the current year to the table
                        yearNumber = currentFinancialYear;
                        yearEndDate = periodRow.PeriodEndDate;

                        DataRow ResultRow = ReturnTable.NewRow();
                        ResultRow[0] = yearNumber;
                        ResultRow[1] = yearEndDate.ToString(INTL_DATE_FORMAT);
                        ResultRow[2] = yearEndDate.ToLongDateString();
                        ReturnTable.Rows.Add(ResultRow);

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
                                yearNumber);

                        DataTable BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", Transaction);

                        BatchYearTable.DefaultView.Sort = String.Format("batchYear DESC");

                        foreach (DataRowView row in BatchYearTable.DefaultView)
                        {
                            DataRow currentBatchYearRow = row.Row;
                            Int32 currentBatchYear = Convert.ToInt32(currentBatchYearRow[0]);

                            if (yearNumber != currentBatchYear)
                            {
                                yearNumber -= 1;
                                yearEndDate = DecrementYear(yearEndDate);

                                if (yearNumber != currentBatchYear)
                                {
                                    //Gap in year numbers
                                    throw new Exception(String.Format(Catalog.GetString("Year {0} not found for Ledger {1}"),
                                            yearNumber,
                                            ALedgerNumber));
                                }
                            }

                            DataRow ResultRow2 = ReturnTable.NewRow();
                            ResultRow2[0] = yearNumber;
                            ResultRow2[1] = yearEndDate.ToString(INTL_DATE_FORMAT);
                            ReturnTable.Rows.Add(ResultRow2);
                        }

                        if (AIncludeNextYear && (ReturnTable.Rows.Find(currentFinancialYear + 1) == null))
                        {
                            DataRow resultRow = ReturnTable.NewRow();
                            resultRow[0] = currentFinancialYear + 1;
                            resultRow[1] = currentYearEndDate.AddYears(1).ToString(INTL_DATE_FORMAT);
                            //resultRow[2] = currentYearEndDate.ToString("dd-MMM-yyyy");
                            ReturnTable.Rows.InsertAt(resultRow, 0);
                        }
                    }); // Get NewOrExisting AutoReadTransaction
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return ReturnTable;
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
            out String ADisplayMember,
            out String AValueMember,
            out String ADescriptionMember)
        {
            DateTime YearEndDate;
            int YearNumber;

            ADisplayMember = "YearEndDate";
            AValueMember = "YearNumber";
            ADescriptionMember = "YearEndDateLong";

            DataTable BatchTable = null;
            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);

                    AAccountingPeriodTable AccountingPeriods = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, transaction);

                    if (LedgerTable.Rows.Count < 1)
                    {
                        return;
                    }

                    ALedgerRow LedgerRow = (ALedgerRow)LedgerTable[0];

                    AccountingPeriods.DefaultView.RowFilter = String.Format("{0}={1}",
                        AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                        LedgerRow.NumberOfAccountingPeriods);

                    //Get last period row
                    AAccountingPeriodRow periodRow = (AAccountingPeriodRow)AccountingPeriods.DefaultView[0].Row;


                    //Create the table to populate the combobox
                    BatchTable = new DataTable();
                    BatchTable.Columns.Add("YearNumber", typeof(System.Int32));
                    BatchTable.Columns.Add("YearEndDate", typeof(String));
                    BatchTable.Columns.Add("YearEndDateLong", typeof(String));
                    BatchTable.PrimaryKey = new DataColumn[] { BatchTable.Columns[0] };

                    //Add the current year to the table
                    YearNumber = LedgerRow.CurrentFinancialYear;
                    YearEndDate = periodRow.PeriodEndDate;

                    DataRow ResultRow = BatchTable.NewRow();
                    ResultRow[0] = YearNumber;
                    ResultRow[1] = YearEndDate.ToShortDateString();
                    ResultRow[2] = YearEndDate.ToLongDateString();
                    BatchTable.Rows.Add(ResultRow);

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

                    DataTable BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", transaction);

                    BatchYearTable.DefaultView.Sort = String.Format("batchYear DESC");

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
                                //Gap in year numbers
                                throw new Exception(String.Format(Catalog.GetString("Year {0} not found for Ledger {1}"),
                                        YearNumber,
                                        ALedgerNumber));
                            }
                        }

                        DataRow ResultRow2 = BatchTable.NewRow();
                        ResultRow2[0] = YearNumber;
                        ResultRow2[1] = YearEndDate.ToShortDateString();
                        ResultRow2[2] = YearEndDate.ToLongDateString();
                        BatchTable.Rows.Add(ResultRow2);
                    } // foreach

                }); // Get NewOrExisting AutoReadTransaction

            return BatchTable;
        } // Get Available GLYears HOSA

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

        /// <summary>
        /// Gets the first day of the accounting period for the ledger and date specified.  Depending on the settings in the
        /// accounting period table this may or may not be the first day of the month of the date specified.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateInAPeriod"></param>
        /// <param name="AFirstDayOfPeriod">The first day in the period of the specified date.</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetFirstDayOfAccountingPeriod(Int32 ALedgerNumber, DateTime ADateInAPeriod, out DateTime AFirstDayOfPeriod)
        {
            TDBTransaction Transaction = null;
            DateTime Result = DateTime.MinValue;

            // Used by importing so the isolation level is serializable
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable, ref Transaction,
                delegate
                {
                    // Get the accounting periods for this ledger.  The table will contain more than 12 rows.
                    // The start dates will be the correct day and month but may have been inserted for an arbitrary year when the table was first created.
                    // We are really only interested in the Day anyway
                    AAccountingPeriodTable periods = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, Transaction);
                    DataView periodsView = new DataView(periods, "",
                        String.Format("{0} ASC", AAccountingPeriodTable.GetPeriodStartDateDBName()), DataViewRowState.CurrentRows);

                    AAccountingPeriodRow row = (AAccountingPeriodRow)periodsView[0].Row;
                    Result = new DateTime(ADateInAPeriod.Year, ADateInAPeriod.Month, row.PeriodStartDate.Day);

                    if (ADateInAPeriod.Day < row.PeriodStartDate.Day)
                    {
                        Result = Result.AddMonths(-1);
                    }
                });

            AFirstDayOfPeriod = Result;
            return Result != DateTime.MinValue;
        }
    }
}