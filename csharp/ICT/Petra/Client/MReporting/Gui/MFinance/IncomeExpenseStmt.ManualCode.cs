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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmIncomeExpenseStmt
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                uco_CostCentreSettings.InitialiseCostCentreList(FLedgerNumber);
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);

                FPetraUtilsObject.LoadDefaultSettings();

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        /// <summary>
        /// Called after MonthEnd. No GUI will be displayed.
        /// </summary>
        public void PrintPeriodEndReport(Int32 ALedgerNumber, Boolean AMonthMode)
        {
            LedgerNumber = ALedgerNumber;
            ALedgerRow Ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            int currentPeriod = Ledger.CurrentPeriod;
            TRptCalculator Calc = new TRptCalculator();

            Calc.AddParameter("param_ledger_number_i", ALedgerNumber);
            Calc.AddParameter("param_year_i", Ledger.CurrentFinancialYear);
            Calc.AddParameter("param_current_financial_year", true);
            Calc.AddParameter("param_period", true);
            Calc.AddParameter("param_start_period_i", currentPeriod - 1);
            Calc.AddParameter("param_end_period_i", currentPeriod - 1);
            Calc.AddParameter("param_current_period", currentPeriod);
            DateTime startDate = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_start_date", startDate);
            DateTime endDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_end_date", endDate);

            Calc.AddStringParameter("param_depth", "Standard");

            Calc.AddParameter("param_auto_email", false);
            Calc.AddParameter("param_paginate", false);
            Calc.AddParameter("param_cost_centre_breakdown", false);
            Calc.AddParameter("param_period_breakdown", false);
            Calc.AddStringParameter("param_costcentreoptions", "All");
            Calc.AddStringParameter("param_cost_centre_list_title", "All Cost Centres");
            Calc.AddStringParameter("param_account_hierarchy_c", "STANDARD");
            Calc.AddStringParameter("param_currency", "Base");
            FPetraUtilsObject.FFastReportsPlugin.GenerateReport(Calc);
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && (!paramsDictionary.ContainsKey(p.name)))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            Int32 ParamNestingDepth = 99;
            String DepthOption = paramsDictionary["param_depth"].ToString();

            if (DepthOption == "Summary")
            {
                ParamNestingDepth = 2;
            }

            if (DepthOption == "Standard")
            {
                ParamNestingDepth = 3;
            }

            paramsDictionary.Add("param_nesting_depth", new TVariant(ParamNestingDepth));


            //
            // The table contains Actual and Budget figures, both this period and YTD, also last year and budget last year.
            // It does not contain any variance (actual / budget) figures - these are calculated in the report.

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("IncomeExpense", paramsDictionary);

            if (this.IsDisposed) // There's no cancel function as such - if the user has pressed Esc the form is closed!
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "IncomeExpense");

            //
            // I need to get the name of the current ledger..

            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_linked_partner_cc", ""); // I may want to use this for auto_email, but usually it's unused.

            //
            // For reports that must be sent on email, one page at a time,
            // I'm using the AutoEmailReports method which calls the FastReports plugin multiple times,
            // and then I'm going to return false, which will prevent the default action using this dataset.

            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            if ((pm.Get("param_auto_email").ToBool())
                && !pm.Get("param_design_template").ToBool()
                )
            {
                String CostCentreFilter = "";
                String CostCentreOptions = pm.Get("param_costcentreoptions").ToString();

                if (CostCentreOptions == "SelectedCostCentres")
                {
                    String CostCentreList = pm.Get("param_cost_centre_codes").ToString();
                    CostCentreList = CostCentreList.Replace(",", "','");                             // SQL IN List items in single quotes
                    CostCentreFilter = " AND a_cost_centre_code_c in ('" + CostCentreList + "')";
                }

                if (CostCentreOptions == "CostCentreRange")
                {
                    CostCentreFilter = " AND a_cost_centre_code_c >='" + pm.Get("param_cost_centre_code_start").ToString() +
                                       "' AND a_cost_centre_code_c >='" + pm.Get("param_cost_centre_code_end").ToString() + "'";
                }

                List <String>Status = FastReportsWrapper.AutoEmailReports(FPetraUtilsObject,
                    FPetraUtilsObject.FFastReportsPlugin,
                    ACalc,
                    FLedgerNumber,
                    CostCentreFilter);
                MessageBox.Show(String.Join("\n", Status), Catalog.GetString("Auto Email") + " " + Catalog.GetString("Income Expense Report"));
                return false;
            }

            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
        }
    }
}