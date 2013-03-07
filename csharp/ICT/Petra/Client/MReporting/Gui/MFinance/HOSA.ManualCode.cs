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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;

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

                TFinanceControls.InitialiseCostCentreList(ref cmbCostCentreCode, FLedgerNumber, false, false, true, false);
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddColumnLayout(0, 8, 0, 3);
            ACalc.AddColumnLayout(1, 11, 0, 3);
            ACalc.SetMaxDisplayColumns(2);
            ACalc.AddColumnCalculation(0, "Debit");
            ACalc.AddColumnCalculation(1, "Credit");
            ACalc.AddParameter("param_daterange", "false");
            ACalc.AddParameter("param_rgrAccounts", "AllAccounts");
            ACalc.AddParameter("param_rgrCostCentres", "CostCentreList");
        }
    }
}