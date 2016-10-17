//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAFO
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

                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.ShowCurrencySelection(false);
                uco_GeneralSettings.ShowOnlyEndPeriod();

                FPetraUtilsObject.LoadDefaultSettings();

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        private void RunOnceOnActivationManual()
        {
            // if fast reports isn't working then close the screen
            if ((FPetraUtilsObject.GetCallerForm() != null) && !FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                MessageBox.Show("No alternative reporting engine is available for this report. This screen will now be closed.", "Reporting engine");
                this.Close();
            }
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
            ACalc.AddParameter("param_daterange", "false");
        }

        //
        // New methods using the Fast-reports DLL:

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
            Calc.AddParameter("param_account_hierarchy_c", "STANDARD");
            Calc.AddParameter("param_ledger_number_i", new TVariant(ALedgerNumber));
            Calc.AddParameter("param_year_i", Ledger.CurrentFinancialYear);
            Calc.AddParameter("param_current_financial_year", true);
            Calc.AddParameter("param_end_period_i", new TVariant(currentPeriod - 1));
            Calc.AddParameter("param_current_period", new TVariant(currentPeriod));
            DateTime startDate = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_start_date", new TVariant(startDate));
            DateTime endDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_end_date", new TVariant(endDate));

            FPetraUtilsObject.FFastReportsPlugin.GenerateReport(Calc);
        }

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            pm.RemoveVariable("param_start_period_i");
            pm.Add("param_start_period_i", 1);

            ArrayList reportParam = ACalc.GetParameters().Elems;
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("AFO", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (HasData)
            {
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "Accounts");
                //
                // My report doesn't need a ledger row - only the name of the ledger.
                DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
                DataView LedgerView = new DataView(LedgerNameTable);
                LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
                String LedgerName = "";

                if (LedgerView.Count > 0)
                {
                    LedgerName = LedgerView[0].Row["LedgerName"].ToString();
                }

                ACalc.AddStringParameter("param_ledger_name", LedgerName);
                ACalc.AddStringParameter("param_base_currency", uco_GeneralSettings.GetBaseCurrency());
                ACalc.AddStringParameter("param_intl_currency", uco_GeneralSettings.GetInternationalCurrency());
            }
            else
            {
                MessageBox.Show(Catalog.GetString("No Summary Accounts found for current Ledger."), "AFO");
            }

            return HasData;
        }
    }
}