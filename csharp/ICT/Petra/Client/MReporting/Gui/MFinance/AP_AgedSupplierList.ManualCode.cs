//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_AgedSupplierList
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
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("APAgedSupplierList",
                true,
                new string[] { "APAgedSupplierList", "documents", "currencies" },
                ACalc,
                this,
                false,
                true,
                FLedgerNumber);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            DateTime SelDate = DateTime.Today;

            if (dtpDateSelection.Date.HasValue)
            {
                SelDate = dtpDateSelection.Date.Value;
            }

            ACalc.AddParameter("param_currency", "Base");
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_date_selection30", SelDate.AddDays(30));
            ACalc.AddParameter("param_date_selectionSub30", SelDate.AddDays(-30));
            ACalc.AddParameter("param_date_selection60", SelDate.AddDays(60));
            ACalc.AddParameter("DueDate", DateTime.Today);

            int ColumnCounter = 0;

            if (chkInvoice.Checked)
            {
                ACalc.AddParameter("param_calculation", "Document Code", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
                ACalc.AddParameter("param_calculation", "Reference", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
                ACalc.AddParameter("param_calculation", "Discount", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.5, ColumnCounter++);
            }
            else
            {
                ACalc.AddParameter("param_calculation", "Supplier", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)5.0, ColumnCounter++);
            }

            ACalc.AddParameter("param_calculation", "Overdue 30+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Overdue", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due 30+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Due 60+", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "Total Due", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter++);

            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }
    }
}