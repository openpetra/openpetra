//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//       Tim Ingham
//
// Copyright 2004-2016 by OM International
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
using System.Data;

using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmSurplusDeficit
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

                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.HideDateRange();
                uco_GeneralSettings.CurrencyOptions(new object[] { "Base", "International" });
                uco_GeneralSettings.ShowOnlyEndPeriodAndQuarter();

                uco_AccountCostCentreSettings.InitialiseLedger(FLedgerNumber);

                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        /// <summary/>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AMonthMode"></param>
        public void PrintPeriodEndReport(Int32 ALedgerNumber, Boolean AMonthMode)
        {
            LedgerNumber = ALedgerNumber;
            ALedgerRow Ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            int currentPeriod = Ledger.CurrentPeriod;
            TRptCalculator Calc = new TRptCalculator();
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

            Calc.AddParameter("param_rgrCostCentres", "AllCostCentres");
            Calc.AddParameter("param_cost_centre_list_title", "All");
            Calc.AddParameter("param_rgrAccounts", "AllAccounts");
            Calc.AddParameter("param_account_list_title", "All");

            Calc.AddParameter("param_currency", "Base");
            Calc.AddParameter("param_currency_name", Ledger.BaseCurrency);

            Calc.AddParameter("param_period_checked", true);
            Calc.AddParameter("param_quarter_checked", false);

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

            String RootCostCentre = "[" + FLedgerNumber + "]";
            paramsDictionary.Add("param_cost_centre_code", new TVariant(RootCostCentre));

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("SurplusDeficit", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "SurplusDeficit");

            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);

            ACalc.AddStringParameter("param_ledger_name", LedgerName);

            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddColumnLayout(0, 8, 0, 3);
            ACalc.AddColumnLayout(1, 11, 0, 3);
            ACalc.SetMaxDisplayColumns(2);
            ACalc.AddColumnCalculation(0, "Debit");
            ACalc.AddColumnCalculation(1, "Credit");

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_with_analysis_attributes", false);
            ACalc.AddParameter("param_depth", "summary");
            ACalc.AddParameter("param_sortby", "Cost Centre");
        }
    }
}