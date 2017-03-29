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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common;

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
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();
                dtpReportDate.Date = DateTime.Now;
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
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && (!paramsDictionary.ContainsKey(p.name)))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("APCurrentPayable", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

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
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "APCurrentPayable");

            return true;
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