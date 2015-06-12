//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
                uco_GeneralSettings.HideDateRange();

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
                true,   // postingonly
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

        /// <summary>
        /// Data loader for HOSA data,
        /// Made static so it can be called from Stewardship Reports
        /// </summary>
        public static Boolean LoadReportDataStaticInner(Form ParentForm,
            TFrmPetraReportingUtils UtilsObject,
            FastReportsWrapper ReportingEngine,
            TRptCalculator ACalc)
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

            String CostCentreFilter = "";

            if (CostCentreCodes == "ALL")
            {
                CostCentreFilter = " AND a_cost_centre.a_cost_centre_type_c='Foreign' ";
            }
            else
            {
                CostCentreCodes = CostCentreCodes.Replace('"', '\'');
                ACalc.AddStringParameter("param_cost_centre_codes", CostCentreCodes);
                CostCentreFilter = " AND a_cost_centre.a_cost_centre_code_c IN (" + CostCentreCodes + ") ";
            }

            if (pm.Get("param_period").ToBool() == true)
            {
                Int32 PeriodStart = pm.Get("param_start_period_i").ToInt32();
                Int32 PeriodEnd = pm.Get("param_end_period_i").ToInt32();

                DateTime DateStart = pm.Get("param_start_date").ToDate();
                DateTime DateEnd = pm.Get("param_end_date").ToDate();

                ALedgerTable LedgerDetailsTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                    TCacheableFinanceTablesEnum.LedgerDetails);
                ALedgerRow LedgerRow = LedgerDetailsTable[0];
                Boolean IsClosed = (!pm.Get("param_current_financial_year").ToBool() || (PeriodEnd < LedgerRow.CurrentPeriod));
                ACalc.AddParameter("param_period_closed", IsClosed);

                String PeriodTitle = " (" + DateStart.ToString("dd-MMM-yyyy") + " - " + DateEnd.ToString("dd-MMM-yyyy") + ")";

                if (PeriodEnd > PeriodStart)
                {
                    PeriodTitle = String.Format("{0} - {1}", PeriodStart, PeriodEnd) + PeriodTitle;
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

            Csv = StringHelper.AddCSV(Csv,
                "AAccount/SELECT * FROM a_account WHERE " + LedgerFilter + " AND a_posting_status_l=true AND a_account_active_flag_l=true");
            Csv = StringHelper.AddCSV(Csv,
                "ACostCentre/SELECT * FROM a_cost_centre WHERE " + LedgerFilter + CostCentreFilter +
                " AND a_posting_cost_centre_flag_l=true AND a_cost_centre_active_flag_l=true");
            Csv = StringHelper.AddCSV(
                Csv,
                "ATransaction/SELECT a_transaction.* FROM a_transaction, a_cost_centre WHERE a_transaction." + LedgerFilter +
                " AND " + TranctDateFilter +
                " AND NOT (a_system_generated_l = true AND (a_narrative_c LIKE 'Gifts received - Gift Batch%' OR a_narrative_c LIKE 'GB - Gift Batch%' OR a_narrative_c LIKE 'Year end re-allocation%'))"
                +
                " AND a_transaction.a_ledger_number_i = a_cost_centre.a_ledger_number_i " +
                " AND a_transaction.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c " +
                CostCentreFilter +
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

            DataTable GiftsTable = TRemote.MReporting.WebConnectors.GetReportDataTable("HOSA", paramsDictionary);
            DataTable KeyMinGiftsTable = TRemote.MReporting.WebConnectors.GetReportDataTable("FieldGifts", paramsDictionary);

            //
            // I'm going to get rid of any Cost Centres that saw no activity in the requested period:
            for (Int32 Idx = ReportDs.ACostCentre.Rows.Count - 1; Idx >= 0; Idx--)
            {
                ACostCentreRow Row = ReportDs.ACostCentre[Idx];
                ReportDs.ATransaction.DefaultView.RowFilter = String.Format("a_cost_centre_code_c='{0}'", Row.CostCentreCode);
                GiftsTable.DefaultView.RowFilter = String.Format("CostCentre='{0}'", Row.CostCentreCode);

                if ((ReportDs.ATransaction.DefaultView.Count == 0) && (GiftsTable.DefaultView.Count == 0))
                {
                    ReportDs.ACostCentre.Rows.Remove(Row);
                }
            }

            if (ParentForm.IsDisposed)
            {
                return false;
            }

            if (GiftsTable == null)
            {
                UtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            ReportingEngine.RegisterData(GiftsTable, "Gifts");
            ReportingEngine.RegisterData(KeyMinGiftsTable, "FieldGifts");
            ReportingEngine.RegisterData(ReportDs.AAccount, "a_account");
            ReportingEngine.RegisterData(ReportDs.ACostCentre, "a_costCentre");
            ReportingEngine.RegisterData(ReportDs.ATransaction, "a_transaction");

            Boolean HasData = (ReportDs.ATransaction.Rows.Count > 0) || (GiftsTable.Rows.Count > 0);

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Transactions found for selected Cost Centres."), "HOSA");
            }

            return HasData;
        }

        //
        // New methods using the Fast-reports DLL:

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            ACalc.AddParameter("param_period", true);
            ACalc.AddStringParameter("param_linked_partner_cc", ""); // Used for auto-emailing HOSAs, this is usually blank.

            // 0 = Full Report. Currently the only option for this report.
            pm.Add("param_ich_number", 0);
            return LoadReportDataStaticInner(this, FPetraUtilsObject, FPetraUtilsObject.FFastReportsPlugin, ACalc);
        }
    }
}