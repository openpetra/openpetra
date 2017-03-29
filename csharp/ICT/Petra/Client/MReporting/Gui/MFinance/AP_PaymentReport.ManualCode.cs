//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Windows.Forms;
using Ict.Common.Printing;
using Ict.Petra.Shared.MFinance.AP.Data;
using System.Collections;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_PaymentReport
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

            DataSet ReportSet = TRemote.MReporting.WebConnectors.GetReportDataSet("APPaymentReport", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportSet == null)
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
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Payments"], "Payments");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportSet.Tables["Suppliers"], "Suppliers");

            return true;
        }

        private static void DefineReportColumns(TRptCalculator ACalc)
        {
            int ColumnCounter = 0;

            ACalc.AddParameter("param_calculation", "", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)6.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentNumber", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)5.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentBank", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentRef", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)4.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentDate", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentCurrency", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentTotal", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            Int32 FilterByPaymentNum = 0;

            if ((txtPaymentNumFrom.Text != "") && (txtPaymentNumTo.Text != ""))
            {
                FilterByPaymentNum = 1;
            }

            Int32 FilterByDate = 0;

            if ((dtpPaymentDateFrom.Text != "") && (dtpPaymentDateTo.Text != ""))
            {
                FilterByDate = 1;
            }

            ACalc.AddParameter("param_filter_by_date", FilterByDate);
            ACalc.AddParameter("param_filter_by_payment_num", FilterByPaymentNum);
            DefineReportColumns(ACalc);
        }

        /// <summary>
        /// Call this to generate a payment report without showing the UI to the user.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AMinPaymentNumber"></param>
        /// <param name="AMaxPaymentNumber"></param>
        /// <param name="Owner"></param>
        public static void CreateReportNoGui(Int32 ALedgerNumber, Int32 AMinPaymentNumber, Int32 AMaxPaymentNumber, Form Owner)
        {
            TRptCalculator Calculator = new TRptCalculator();

            TFrmPetraReportingUtils.InitialiseCalculator(
                Calculator,
                "Finance/AccountsPayable/AP_PaymentReport.xml,Finance/finance.xml,common.xml",
                "",
                "APPaymentReport");

            Calculator.AddParameter("param_payment_num_from_i", AMinPaymentNumber);
            Calculator.AddParameter("param_payment_num_to_i", AMaxPaymentNumber);

/*
 *          Calculator.AddParameter("param_payment_date_from_i", DateTime.Now);
 *          Calculator.AddParameter("param_payment_date_to_i", DateTime.Now);
 */
            Calculator.AddParameter("param_filter_by_date", 0);
            Calculator.AddParameter("param_filter_by_payment_num", 1);
            Calculator.AddParameter("param_ledger_number_i", ALedgerNumber);
            DefineReportColumns(Calculator);

            TFrmPetraReportingUtils.GenerateReport(Calculator, Owner, "APPaymentReport", true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="PaymentNumStart"></param>
        /// <param name="PaymentNumEnd"></param>
        public void SetPaymentNumber(Int32 PaymentNumStart, Int32 PaymentNumEnd)
        {
            txtPaymentNumFrom.Text = PaymentNumStart.ToString();
            txtPaymentNumTo.Text = PaymentNumEnd.ToString();
        }
    }
}