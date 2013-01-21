//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmIncomeLocalSplit
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

                uco_Selection.InitialiseLedger(FLedgerNumber);
                uco_Selection.ShowAccountHierarchy(false);
                uco_Selection.ShowCurrencySelection(false);
                uco_Selection.EnableDateSelection(true);

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_currency", "base");
            ACalc.AddParameter("param_ytd", "mixed");
            ACalc.AddParameter("param_depth", "standard");

            ACalc.AddParameter("param_calculation", "Amount", 0);
            ACalc.AddParameter("param_ytd", false, 0);
            ACalc.AddParameter("ColumnWidth", "2", 0);
            ACalc.AddParameter("ColumnPositionIndented", (float)0.5, 0);

            ACalc.AddParameter("param_calculation", "% of Grand Income", 1);
            ACalc.AddParameter("FirstColumn", 0, 1);
            ACalc.AddParameter("param_ytd", false, 1);
            ACalc.AddParameter("ColumnWidth", "2", 1);
            ACalc.AddParameter("ColumnPositionIndented", (float)0.5, 1);

            ACalc.AddParameter("param_calculation", "AmountYTD", 2);
            ACalc.AddParameter("param_ytd", true, 2);
            ACalc.AddParameter("ColumnWidth", 2, 2);
            ACalc.AddParameter("ColumnPositionIndented", (float)0.5, 2);

            ACalc.AddParameter("param_calculation", "% of Grand Income", 3);
            ACalc.AddParameter("FirstColumn", 2, 3);
            ACalc.AddParameter("param_ytd", true, 3);
            ACalc.AddParameter("ColumnWidth", 2, 3);
            ACalc.AddParameter("ColumnPositionIndented", (float)0.5, 3);

            ACalc.AddParameter("MaxDisplayColumns", 4);
        }
    }
}