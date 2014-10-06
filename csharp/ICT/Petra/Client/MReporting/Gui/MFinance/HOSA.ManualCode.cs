//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections;
using Ict.Petra.Shared.MFinance.GL.Data;
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Gui;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmHOSA
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                uco_GeneralSettings.EnableDateSelection(false);

                FLedgerNumber = value;

                InitialiseCostCentreList();
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.CurrencyOptions(new object[] { "Base", "International" });

                FPetraUtilsObject.LoadDefaultSettings();

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        private string FCostCenterCodesDuringLoad = string.Empty;

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControlsManual(TParameterList AParameters)
        {
            FCostCenterCodesDuringLoad = AParameters.Get("param_cost_centre_codes").ToString();
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddColumnLayout(0, 8, 0, 3);
            ACalc.AddColumnLayout(1, 11, 0, 3);
            ACalc.AddColumnLayout(2, 14, 0, 7);
            ACalc.SetMaxDisplayColumns(3);
            ACalc.AddColumnCalculation(0, "Debit");
            ACalc.AddColumnCalculation(1, "Credit");
            ACalc.AddColumnCalculation(2, "Transaction Narrative");
            ACalc.AddParameter("param_daterange", "false");
            ACalc.AddParameter("param_rgrAccounts", "AllAccounts");
            ACalc.AddParameter("param_rgrCostCentres", "CostCentreList");
            // TODO need to allow to specify an ICH run number
            ACalc.AddParameter("param_ich_number", 0);

            ACalc.AddStringParameter("param_cost_centre_codes", clbCostCentres.GetCheckedStringList(true));
        }

        private void chkExcludeCostCentresChanged(System.Object sender, System.EventArgs e)
        {
            if (FLedgerNumber > 0)
            {
                InitialiseCostCentreList();
            }
        }

        /// <summary>
        /// Init the grid
        /// </summary>
        private void InitialiseCostCentreList()
        {
            TFinanceControls.InitialiseCostCentreList(
                ref clbCostCentres,
                FLedgerNumber,
                true,  // postingonly
                false,  // excludeposting
                chkExcludeInactiveCostCentres.Checked,
                rbtFields.Checked,
                rbtDepartments.Checked,
                rbtPersonalCostcentres.Checked);

            if (FCostCenterCodesDuringLoad.Length > 0)
            {
                clbCostCentres.SetCheckedStringList(FCostCenterCodesDuringLoad);
                FCostCenterCodesDuringLoad = "";
            }
            else
            {
                clbCostCentres.SetCheckedStringList("");
            }
        }

        //
        // New methods using the Fast-reports DLL:

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            String Csv = "";

            //
            // My "a_transaction" table forms the lower half of the HOSA:
            // All transactions for all the "Expense" accounts for the selected Cost Centre within the selected dates or periods.

            String LedgerFilter = "a_ledger_number_i=" + pm.Get("param_ledger_number_i").ToInt32();
            String TranctDateFilter = ""; // Optional Date Filter, as periods or dates
            String CostCentreCodes = pm.Get("param_cost_centre_codes").ToString();

            if (CostCentreCodes == String.Empty)
            {
                MessageBox.Show(Catalog.GetString("Please select one or more Cost Centres."), "HOSA");
                return false;
            }

            CostCentreCodes = CostCentreCodes.Replace('"', '\'');
            ACalc.AddStringParameter("param_cost_centre_codes", CostCentreCodes);

            if (pm.Get("param_period").ToBool() == true)
            {
                DataTable AccountingPeriodTbl =
                    (AAccountingPeriodTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                        pm.Get("param_ledger_number_i").ToInt32());
                Int32 PeriodStart = pm.Get("param_start_period_i").ToInt32();
                Int32 PeriodEnd = pm.Get("param_end_period_i").ToInt32();
                AccountingPeriodTbl.DefaultView.RowFilter = LedgerFilter + " AND a_accounting_period_number_i=" + PeriodStart;
                DateTime DateStart = Convert.ToDateTime(AccountingPeriodTbl.DefaultView[0].Row["a_period_start_date_d"]);
                pm.Add("param_start_date", DateStart);
                AccountingPeriodTbl.DefaultView.RowFilter = LedgerFilter + " AND a_accounting_period_number_i=" + PeriodEnd;
                DateTime DateEnd = Convert.ToDateTime(AccountingPeriodTbl.DefaultView[0].Row["a_period_end_date_d"]);
                pm.Add("param_end_date", DateEnd);

                String PeriodTitle = " (" + DateStart.ToString("dd-MMM-yyyy") + " - " + DateEnd.ToString("dd-MMM-yyyy") + ")";

                if (PeriodEnd > PeriodStart)
                {
                    PeriodTitle = String.Format("{0} - {1}", PeriodEnd, PeriodStart) + PeriodTitle;
                }
                else
                {
                    PeriodTitle = String.Format("{0}", PeriodStart) + PeriodTitle;
                }

                pm.Add("param_date_title", PeriodTitle);
            }
            else
            {
                String PeriodTitle = " " + pm.Get("param_start_date").DateToString("yyyy-MM-dd") + " - " +
                                     pm.Get("param_end_date").DateToString("yyyy-MM-dd");

                pm.Add("param_date_title", PeriodTitle);
            }

            TranctDateFilter = "a_transaction_date_d>='" + pm.Get("param_start_date").DateToString("yyyy-MM-dd") +
                               "' AND a_transaction_date_d<='" + pm.Get("param_end_date").DateToString("yyyy-MM-dd") + "'";
            String TranctCostCentreFilter = " AND a_cost_centre_code_c IN (" + CostCentreCodes + ") ";

            Csv = StringHelper.AddCSV(Csv,
                "AAccount/SELECT * FROM a_account WHERE " + LedgerFilter + " AND a_posting_status_l=true AND a_account_active_flag_l=true");
            Csv = StringHelper.AddCSV(Csv,
                "ACostCentre/SELECT * FROM a_cost_centre WHERE " + LedgerFilter + " AND a_cost_centre_code_c IN (" + CostCentreCodes +
                ")  AND a_posting_cost_centre_flag_l=true AND a_cost_centre_active_flag_l=true");
            Csv = StringHelper.AddCSV(
                Csv,
                "ATransaction/SELECT * FROM a_transaction WHERE " + LedgerFilter +
                TranctCostCentreFilter + " AND " + TranctDateFilter +
                " AND NOT (a_system_generated_l = true AND (a_narrative_c LIKE 'Gifts received - Gift Batch%' OR a_narrative_c LIKE 'GB - Gift Batch%' OR a_narrative_c LIKE 'Year end re-allocation%'))"
                +
                " ORDER BY a_account_code_c, a_transaction_date_d");

            GLReportingTDS ReportDs = TRemote.MReporting.WebConnectors.GetReportingDataSet(Csv);
            ArrayList reportParam = ACalc.GetParameters().Elems;
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("HOSA", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "Gifts");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.AAccount, "a_account");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.ACostCentre, "a_costCentre");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.ATransaction, "a_transaction");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = (ReportDs.ATransaction.Rows.Count > 0) || (ReportTable.Rows.Count > 0);

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Transactions found for selected Cost Centres."), "HOSA");
            }

            return HasData;
        }
    }
}