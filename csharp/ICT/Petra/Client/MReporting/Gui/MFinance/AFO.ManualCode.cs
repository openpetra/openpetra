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
                uco_GeneralSettings.EnableDateSelection(false);

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
        	if (!FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
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

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            String LedgerFilter = "a_ledger_number_i=" + pm.Get("param_ledger_number_i").ToInt32();

            pm.RemoveVariable("param_start_period_i");
            pm.Add("param_start_period_i",1);
            pm.Add("param_current_period", uco_GeneralSettings.GetCurrentPeiod());

            pm.Add("param_sortby", "a_account_code_c");

            // only periods can be used for this report
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
            	return false;
            }
            
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

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "Accounts");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            ACalc.AddStringParameter("param_base_currency", uco_GeneralSettings.GetBaseCurrency());
            ACalc.AddStringParameter("param_intl_currency", uco_GeneralSettings.GetInternationalCurrency());

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Summary Accounts found for current Ledger."), "AFO");
            }

            return HasData;
        }
    }
}