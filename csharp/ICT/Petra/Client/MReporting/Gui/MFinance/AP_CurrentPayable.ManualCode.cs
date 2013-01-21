// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2012 by OM International
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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_CurrentPayable
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// The main screen will call this on creation, to give me my Ledger Number.
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private static void DefineReportColumns(TRptCalculator ACalc)
        {
            int ColumnCounter = 0;

            ACalc.AddParameter("param_calculation", "ApNumber", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "SupplierKey", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "SupplierName", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)5.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "DocCode", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "DueDate", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)4.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Currency", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "CurAmount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "BaseAmount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            DefineReportColumns(ACalc);
        }
    }
}