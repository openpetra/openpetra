//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using System.Collections;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared;
using System.Data;
using Ict.Petra.Client.App.Core;
using System.IO;
using System.Collections.Generic;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAccountDetail
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                uco_GeneralSettings.EnableDateSelection(true);
                uco_GeneralSettings.EnforceLocalCurrencyWhenMultiplePeriodSelected = true;

                FLedgerNumber = value;

                uco_AccountCostCentreSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.ShowOnlyPeriodSelection();
                pnlSorting.Padding = new System.Windows.Forms.Padding(8); // This tweak bring controls inline.
                FPetraUtilsObject.LoadDefaultSettings();
                rbtSortByCostCentre.CheckedChanged += rbtSortByCostCentre_CheckedChanged;
            }
        }

        void rbtSortByCostCentre_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtSortByCostCentre.Checked)
            {
                chkPaginate.Checked = false;
                chkAutoEmail.Checked = false;
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_with_analysis_attributes", false);

            // if rbtSortByCostCentre is checked then these parameters are added in generated code
            if (!rbtSortByCostCentre.Checked)
            {
                ACalc.AddParameter("param_paginate", false);
                ACalc.AddParameter("param_auto_email", false);
            }
        }

        /// <summary>
        /// Called after MonthEnd. No GUI will be displayed.
        /// </summary>
        public void PrintPeriodEndReport(Int32 ALedgerNumber, Boolean AMonthMode)
        {
            LedgerNumber = ALedgerNumber;
            TRptCalculator Calc = new TRptCalculator();
            ALedgerRow Ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];

            int currentPeriod = Ledger.CurrentPeriod;
            Calc.AddParameter("param_ledger_number_i", ALedgerNumber);
            Calc.AddParameter("param_current_period", currentPeriod);
            Calc.AddParameter("param_period", true);
            Calc.AddParameter("param_current_financial_year", true);
            Calc.AddStringParameter("param_rgrAccounts", "All");
            Calc.AddStringParameter("param_rgrCostCentres", "All");
            Calc.AddStringParameter("param_account_list_title", "All Accounts");
            Calc.AddStringParameter("param_cost_centre_list_title", "All Cost Centres");

            Calc.AddParameter("param_year_i", Ledger.CurrentFinancialYear);
            Calc.AddParameter("param_start_period_i", currentPeriod - 1);
            Calc.AddParameter("param_end_period_i", currentPeriod - 1);
            DateTime startDate = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_start_date", new TVariant(startDate));
            DateTime endDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(
                ALedgerNumber, Ledger.CurrentFinancialYear, -1, currentPeriod);
            Calc.AddParameter("param_end_date", new TVariant(endDate));

            Calc.AddStringParameter("param_sortby", "Account");
            Calc.AddStringParameter("param_currency", "Base");
            Calc.AddStringParameter("param_currency_name", Ledger.BaseCurrency);

            Calc.AddParameter("param_paginate", false);
            Calc.AddParameter("param_auto_email", false);
            Calc.AddParameter("param_with_analysis_attributes", false);

            FPetraUtilsObject.FFastReportsPlugin.GenerateReport(Calc);
        }
    }
}