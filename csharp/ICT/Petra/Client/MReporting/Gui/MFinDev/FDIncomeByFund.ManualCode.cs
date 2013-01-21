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
using System.Windows.Forms;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmFDIncomeByFund
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

                TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbPeriodYear, FLedgerNumber);
                TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbPeriodYearQuarter, FLedgerNumber);

                txtLedger.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                rbtPeriodRange.Checked = true;
                txtEndPeriod.Text = "12";
                txtQuarter.Text = "1";
                cmbPeriodYear.SelectedIndex = 0;
                cmbPeriodYearQuarter.SelectedIndex = 0;

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_currency", "base");
            ACalc.AddParameter("param_ytd", "mixed");
            ACalc.AddParameter("param_start_period_i", txtEndPeriod.Text);

            if (rbtQuarter.Checked)
            {
                ACalc.AddParameter("param_end_period_i", (Convert.ToInt16(txtQuarter.Text) * 3).ToString());
            }

            ACalc.AddParameter("param_explicit_motivation", "");
            ACalc.AddParameter("param_exclude_motivation", "");
            //ACalc.AddParameter("param_selectedAreasFunds", );
        }
    }
}