//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using System.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using System.Windows.Forms;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmBalanceSheetStandard
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

                FPetraUtilsObject.LoadDefaultSettings();

                if (FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
                {
                    FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
                }
                else
                {
                    MessageBox.Show("The FastReports plugin did not initialise.", "Reporting engine");
                }
            }
        }

        private void RunOnceOnActivationManual()
        {
            if (FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                this.tabReportSettings.Controls.Remove(tpgAdditionalSettings); // These tabs represent settings that are not supported
                this.tabReportSettings.Controls.Remove(tpgColumnSettings);     // in the FastReports based solution.
            }

            uco_GeneralSettings.ShowOnlyEndPeriod();
            uco_GeneralSettings.CurrencyOptions(new object[] { "Base", "International" });
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

            Int32 ParamNestingDepth = 3;
            String DepthOption = paramsDictionary["param_depth"].ToString();

            if (DepthOption == "summary")
            {
                ParamNestingDepth = 1;
            }

            paramsDictionary.Add("param_nesting_depth", new TVariant(ParamNestingDepth));
            String RootCostCentre = "[" + FLedgerNumber + "]";
            paramsDictionary.Add("param_cost_centre_code", new TVariant(RootCostCentre));

            //
            // The table contains extra rows for "headers" and "footers", facilitating the hierarchical printout.

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("BalanceSheet", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "BalanceSheet");

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
            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            ACalc.AddParameter("param_cost_centre_breakdown", false);
            ACalc.AddParameter("param_cost_centre_summary", false);
            ACalc.AddParameter("param_cost_centre_codes", "");
            ACalc.AddParameter("param_costcentreoptions", "AccountLevel");
        }
    }
}