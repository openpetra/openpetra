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
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting.UIConnectors;

namespace Ict.Petra.Server.MFinance.Reporting
{
    ///<summary>
    /// This UIConnector provides data for the finance reporting screens
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into///one* DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    ///</summary>
    public class TFinanceReportingUIConnector : TConfigurableMBRObject, IReportingUIConnectorsNamespace
    {
        /// <summary>the currently selected ledger</summary>
        private System.Int32 FLedgerNr;
        private int FNumberAccountingPeriods;
        private int FNumberForwardingPeriods;
        private int FCurrentPeriod;
        private int FCurrentYear;

        /// <summary>
        /// get the number of forwarding periods. needed to avoid warning about unused FNumberForwardingPeriods
        /// </summary>
        public int NumberForwardingPeriods
        {
            get
            {
                return FNumberForwardingPeriods;
            }
        }

        /// <summary>
        /// get the number of the current period. needed to avoid warning about unused FNumberForwardingPeriods
        /// </summary>
        public int CurrentPeriod
        {
            get
            {
                return FCurrentPeriod;
            }
        }

        /// <summary>
        /// initialise the object, select the given ledger
        /// </summary>
        /// <param name="ALedgerNr"></param>
        public void SelectLedger(System.Int32 ALedgerNr)
        {
            FLedgerNr = ALedgerNr;
            GetLedgerPeriodDetails(out FNumberAccountingPeriods, out FNumberForwardingPeriods, out FCurrentPeriod, out FCurrentYear);
        }

        /// <summary>
        /// get the details of the given ledger
        /// </summary>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ANumberForwardingPeriods"></param>
        /// <param name="ACurrentPeriod"></param>
        /// <param name="ACurrentYear"></param>
        public void GetLedgerPeriodDetails(out int ANumberAccountingPeriods,
            out int ANumberForwardingPeriods,
            out int ACurrentPeriod,
            out int ACurrentYear)
        {
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DataTable CachedDataTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                FLedgerNr,
                out typeofTable);
            string whereClause = ALedgerTable.GetLedgerNumberDBName() + " = " + FLedgerNr.ToString();
            DataRow[] filteredRows = CachedDataTable.Select(whereClause, ALedgerTable.GetLedgerNumberDBName());

            if (filteredRows.Length > 0)
            {
                ANumberAccountingPeriods = ((ALedgerRow)filteredRows[0]).NumberOfAccountingPeriods;
                ANumberForwardingPeriods = ((ALedgerRow)filteredRows[0]).NumberFwdPostingPeriods;
                ACurrentPeriod = ((ALedgerRow)filteredRows[0]).CurrentPeriod;
                ACurrentYear = ((ALedgerRow)filteredRows[0]).CurrentFinancialYear;
            }
            else
            {
                ANumberAccountingPeriods = -1;
                ANumberForwardingPeriods = -1;
                ACurrentPeriod = -1;
                ACurrentYear = -1;
            }
        }

        /// <summary>
        /// get the real period stored in the database
        /// this is needed for reports that run on a different financial year, ahead or behind by several months
        /// </summary>
        /// <param name="ADiffPeriod"></param>
        /// <param name="AYear"></param>
        /// <param name="APeriod"></param>
        /// <param name="ARealPeriod"></param>
        /// <param name="ARealYear"></param>
        public void GetRealPeriod(System.Int32 ADiffPeriod,
            System.Int32 AYear,
            System.Int32 APeriod,
            out System.Int32 ARealPeriod,
            out System.Int32 ARealYear)
        {
            ARealPeriod = APeriod + ADiffPeriod;
            ARealYear = AYear;

            if (ADiffPeriod == 0)
            {
                return;
            }

            // the period is in the last year
            // this treatment only applies to situations with different financial years.
            // in a financial year equals to the glm year, the period 0 represents the start balance
            if ((ADiffPeriod == 0) && (ARealPeriod == 0))
            {
                //do nothing
            }
            else if (ARealPeriod < 1)
            {
                ARealPeriod = FNumberAccountingPeriods + ARealPeriod;
                ARealYear = ARealYear - 1;
            }

            // forwarding periods are only allowed in the current year
            if ((ARealPeriod > FNumberAccountingPeriods) && (ARealYear != FCurrentYear))
            {
                ARealPeriod = ARealPeriod - FNumberAccountingPeriods;
                ARealYear = ARealYear + 1;
            }
        }

        /// <summary>
        /// get the start date of the given period
        /// </summary>
        /// <param name="AYear"></param>
        /// <param name="ADiffPeriod"></param>
        /// <param name="APeriod"></param>
        /// <returns></returns>
        public System.DateTime GetPeriodStartDate(System.Int32 AYear, System.Int32 ADiffPeriod, System.Int32 APeriod)
        {
            System.Int32 RealYear = 0;
            System.Int32 RealPeriod = 0;
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DateTime ReturnValue = DateTime.Now;
            GetRealPeriod(ADiffPeriod, AYear, APeriod, out RealPeriod, out RealYear);
            DataTable CachedDataTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                FLedgerNr,
                out typeofTable);
            string whereClause = AAccountingPeriodTable.GetLedgerNumberDBName() + " = " + FLedgerNr.ToString() + " and " +
                                 AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " = " + RealPeriod.ToString();
            DataRow[] filteredRows = CachedDataTable.Select(whereClause);

            if (filteredRows.Length > 0)
            {
                ReturnValue = ((AAccountingPeriodRow)filteredRows[0]).PeriodStartDate;
                ReturnValue = ReturnValue.AddYears(RealYear - FCurrentYear);
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the end date of the given period
        /// </summary>
        /// <param name="AYear"></param>
        /// <param name="ADiffPeriod"></param>
        /// <param name="APeriod"></param>
        /// <returns></returns>
        public System.DateTime GetPeriodEndDate(System.Int32 AYear, System.Int32 ADiffPeriod, System.Int32 APeriod)
        {
            System.Int32 RealYear = 0;
            System.Int32 RealPeriod = 0;
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            DateTime ReturnValue = DateTime.Now;
            GetRealPeriod(ADiffPeriod, AYear, APeriod, out RealPeriod, out RealYear);
            DataTable CachedDataTable = CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                FLedgerNr,
                out typeofTable);
            string whereClause = AAccountingPeriodTable.GetLedgerNumberDBName() + " = " + FLedgerNr.ToString() + " and " +
                                 AAccountingPeriodTable.GetAccountingPeriodNumberDBName() + " = " + RealPeriod.ToString();
            DataRow[] filteredRows = CachedDataTable.Select(whereClause);

            if (filteredRows.Length > 0)
            {
                ReturnValue = ((AAccountingPeriodRow)filteredRows[0]).PeriodEndDate;
                ReturnValue = ReturnValue.AddYears(RealYear - FCurrentYear);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Loads all available financial years into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAvailableFinancialYears(System.Int32 ADiffPeriod, out String ADisplayMember, out String AValueMember)
        {
            DataTable tab;
            DataTable BatchYearTable;
            DataRow resultRow;
            String sql;

            System.DateTime currentYearEnd;
            Int32 counter;
            TDBTransaction ReadTransaction;
            currentYearEnd = GetPeriodEndDate(FCurrentYear, ADiffPeriod, FNumberAccountingPeriods);
            ADisplayMember = "YearDate";
            AValueMember = "YearNumber";
            tab = new DataTable();
            tab.Columns.Add(AValueMember, typeof(System.Int32));
            tab.Columns.Add(ADisplayMember, typeof(String));
            counter = 0;

            // add the current year
            resultRow = tab.NewRow();
            resultRow[0] = (System.Object)FCurrentYear;
            resultRow[1] = currentYearEnd.ToString("yyyy");
            tab.Rows.InsertAt(resultRow, counter);
            counter = counter + 1;
            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                // add the previous years, which are retrieved by reading from the old batches
                // TODO: use GetDBName of the table
                sql = "SELECT DISTINCT a_batch_year_i AS availYear " + " FROM PUB_a_previous_year_batch " + " WHERE a_ledger_number_i = " +
                      FLedgerNr.ToString() + " ORDER BY 1 DESC";
                BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", ReadTransaction);

                foreach (DataRow row in BatchYearTable.Rows)
                {
                    resultRow = tab.NewRow();
                    resultRow[0] = row[0];

                    // resultRow.item[1] := DateToLocalizedString(CurrentYearEnd.AddYears(1  (CurrentFinancialYear  Convert.ToInt32(row[0]))));
                    resultRow[1] = currentYearEnd.AddYears(-1 * (FCurrentYear - Convert.ToInt32(row[0]))).ToString("yyyy");
                    tab.Rows.InsertAt(resultRow, counter);
                    counter = counter + 1;
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return tab;
        }

        /// <summary>
        /// Load all the receiving fields
        /// </summary>
        /// <param name="ADisplayMember"></param>
        /// <param name="AValueMember"></param>
        /// <returns>Table with the field keys and the field names</returns>
        public DataTable GetReceivingFields(out String ADisplayMember, out String AValueMember)
        {
            DataTable ReturnTable = new DataTable();
            String sql;

            TDBTransaction ReadTransaction;

            ADisplayMember = "FieldName";
            AValueMember = "FieldKey";

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                sql = "SELECT DISTINCT " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " AS " + AValueMember +
                      ", " +
                      PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + " AS " + ADisplayMember +
                      " FROM " + PPartnerTable.GetTableDBName() + ", " +
                      PPartnerTypeTable.GetTableDBName() +
                      " WHERE " +
                      PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetPartnerKeyDBName() + " = " + PPartnerTable.GetTableDBName() +
                      "." + PPartnerTable.GetPartnerKeyDBName() +
                      " AND (" + PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetTypeCodeDBName() + " = 'LEDGER' OR " +
                      PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetTypeCodeDBName() + " = 'COSTCENTRE' " +
                      ") ORDER BY " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName();

                ReturnTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", ReadTransaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return ReturnTable;
        }
    }
}