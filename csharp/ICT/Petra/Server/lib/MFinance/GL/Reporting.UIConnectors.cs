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
using Ict.Common.Remoting.Server;
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
    public class TFinanceReportingUIConnector : TConfigurableMBRObject
    {
// TODO: should be implementing IReportingUIConnectorsNamespace

        /// <summary>the currently selected ledger</summary>
        private System.Int32 FLedgerNr;

        /// <summary>
        /// initialise the object, select the given ledger
        /// </summary>
        /// <param name="ALedgerNr"></param>
        public void SelectLedger(System.Int32 ALedgerNr)
        {
            FLedgerNr = ALedgerNr;
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
            ALedgerTable CachedDataTable = (ALedgerTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                FLedgerNr,
                out typeofTable);

            if (CachedDataTable.Rows.Count > 0)
            {
                ANumberAccountingPeriods = CachedDataTable[0].NumberOfAccountingPeriods;
                ANumberForwardingPeriods = CachedDataTable[0].NumberFwdPostingPeriods;
                ACurrentPeriod = CachedDataTable[0].CurrentPeriod;
                ACurrentYear = CachedDataTable[0].CurrentFinancialYear;
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
        /// Loads all available financial years into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAvailableFinancialYears(System.Int32 ADiffPeriod, out String ADisplayMember, out String AValueMember)
        {
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetAvailableGLYears(
                FLedgerNr,
                ADiffPeriod,
                out ADisplayMember,
                out AValueMember);
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

        private string GetReportingCostCentres(ACostCentreTable ACostCentres, string ASummaryCostCentreCode)
        {
            ACostCentres.DefaultView.Sort = ACostCentreTable.GetCostCentreToReportToDBName();

            string result = string.Empty;

            DataRowView[] ReportingCostCentres = ACostCentres.DefaultView.FindRows(ASummaryCostCentreCode);

            foreach (DataRowView rv in ReportingCostCentres)
            {
                ACostCentreRow row = (ACostCentreRow)rv.Row;

                if (row.PostingCostCentreFlag)
                {
                    result = StringHelper.AddCSV(result, row.CostCentreCode);
                }
                else
                {
                    result = StringHelper.ConcatCSV(result, GetReportingCostCentres(ACostCentres, row.CostCentreCode));
                }
            }

            return result;
        }

        /// <summary>
        /// Get all cost centres that report into the given summary cost centre
        /// </summary>
        /// <returns>a CSV list of the reporting cost centres</returns>
        public string GetReportingCostCentres(String ASummaryCostCentreCode)
        {
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ACostCentreTable CachedDataTable = (ACostCentreTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.CostCentreList,
                "",
                false,
                FLedgerNr,
                out typeofTable);

            return GetReportingCostCentres(CachedDataTable, ASummaryCostCentreCode);
        }
    }
}