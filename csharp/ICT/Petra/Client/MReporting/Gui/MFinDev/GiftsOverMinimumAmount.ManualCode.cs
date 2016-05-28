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
using System.Collections.Generic;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmGiftsOverMinimumAmount
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
                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (!dtpEndDate.ValidDate(false)
                || !dtpStartDate.ValidDate(false))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Date format problem"),
                    Catalog.GetString("Please check the date entry."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
            else
            {
                if (dtpStartDate.Date > dtpEndDate.Date)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("From date is later than to date."),
                        Catalog.GetString("Please change from date or to date."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            if (!ucoMotivationCriteria.IsAnyMotivationDetailSelected())
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("No Motivation Detail selected"),
                    Catalog.GetString("Please select at least one Motivation Detail."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("ControlSource", "", ReportingConsts.HEADERCOLUMN);
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter <= MaxColumns; ++Counter)
            {
                String ColumnName = ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();

                if (ColumnName == "Total Gifts")
                {
                    ACalc.AddParameter("param_gift_amount_column", Counter);
                }
            }

            // this parameter is added incorrectly by the generated code
            ACalc.RemoveParameter("param_minimum_amount");
            ACalc.AddParameter("param_minimum_amount", this.txtMinimumAmount.NumberValueDecimal);
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

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("GiftsOverMinimum", paramsDictionary);

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
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "GiftsOverMinimum");

            return true;
        }
    }
}