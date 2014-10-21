//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmTrialBalance
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

                uco_AccountCostCentreSettings.InitialiseLedger(FLedgerNumber);
                uco_AccountCostCentreSettings.FReportWithBalance = false;
                uco_GeneralSettings.ShowOnlyEndPeriod();

                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);

                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
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

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("TrialBalance", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "TrialBalance");

            //
            // I need to get the name of the current ledger..

            ALedgerTable LedgerTbl = TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber);

            String LedgerName = "";
            Boolean IsClosed = false;

            if (LedgerTbl.Rows.Count > 0)
            {
                ALedgerRow LedgerRow = LedgerTbl[0];
                LedgerName = LedgerRow.LedgerName;
                //
                // I want to tell the user whether the selected period is closed
                // (although they probably know already...)
                Int32 SelPeriod = ACalc.GetParameters().GetParameter("param_end_period_i").value.ToInt32();
                Int32 SelYear   = ACalc.GetParameters().GetParameter("param_year_i").value.ToInt32();
                if ((SelYear < LedgerRow.CurrentFinancialYear) || (SelPeriod < LedgerRow.CurrentPeriod))
                {
                    IsClosed = true;
                }
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddParameter("param_period_closed", IsClosed);

            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_with_analysis_attributes", false);
            ACalc.AddColumnLayout(0, 8, 0, 3);
            ACalc.AddColumnLayout(1, 11, 0, 3);
            ACalc.SetMaxDisplayColumns(2);
            ACalc.AddColumnCalculation(0, "Debit");
            ACalc.AddColumnCalculation(1, "Credit");
        }
    }
}