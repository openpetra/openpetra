//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, peters
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmSYBUNTReport
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

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);

                ucoGiftsInRange.InitialiseLedger(FLedgerNumber);
                ucoNoGiftsInRange.InitialiseLedger(FLedgerNumber);
                ucoNoGiftsInRange.GroupLabel = Catalog.GetString("No Gifts In Range");
            }
        }

        private void RunOnceOnActivationManual()
        {
            // if fast reports, then ignore columns tab
            if ((FPetraUtilsObject.GetCallerForm() != null) && FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // make sure that for each group one radio button is selected
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(grpSelection, FPetraUtilsObject);
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrFormatCurrency, FPetraUtilsObject);

            if ((ucoGiftsInRange.RangeTable == null) || (ucoGiftsInRange.RangeTable.Rows.Count == 0))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("'Gifts In Range' Needed."),
                    Catalog.GetString("Please enter a value for 'Gifts In Range'."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((ucoNoGiftsInRange.RangeTable == null) || (ucoNoGiftsInRange.RangeTable.Rows.Count == 0))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("'No Gifts In Range' Needed."),
                    Catalog.GetString("Please enter a value for 'No Gifts In Range'."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            // check for overlapping in the ranges
            foreach (DataRow Row in ucoGiftsInRange.RangeTable.Rows)
            {
                foreach (DataRow NoRow in ucoNoGiftsInRange.RangeTable.Rows)
                {
                    if ((((Convert.ToDateTime(NoRow["From"]) >= Convert.ToDateTime(Row["From"]))
                          && (Convert.ToDateTime(NoRow["From"]) <= Convert.ToDateTime(Row["To"])))
                         || ((Convert.ToDateTime(NoRow["To"]) >= Convert.ToDateTime(Row["From"]))
                             && (Convert.ToDateTime(NoRow["To"]) <= Convert.ToDateTime(Row["To"]))))
                        || (((Convert.ToDateTime(Row["From"]) >= Convert.ToDateTime(NoRow["From"]))
                             && (Convert.ToDateTime(Row["From"]) <= Convert.ToDateTime(NoRow["To"])))
                            || ((Convert.ToDateTime(Row["To"]) >= Convert.ToDateTime(NoRow["From"]))
                                && (Convert.ToDateTime(Row["To"]) <= Convert.ToDateTime(NoRow["To"])))))
                    {
                        TVerificationResult VerificationResult = new TVerificationResult(
                            Catalog.GetString("Overlapping Ranges"),
                            Catalog.GetString("'Gifts In Range' and 'No Gifts In Range' overlap. This is not allowed."),
                            TResultSeverity.Resv_Critical);
                        FPetraUtilsObject.AddVerificationResult(VerificationResult);
                    }
                }
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            ACalc.AddParameter("param_all_partners", rbtAllPartners.Checked);
            ACalc.AddParameter("param_extract", rbtExtract.Checked);

            if (rbtExtract.Checked)
            {
                ACalc.AddParameter("param_extract_name", txtExtract.Text);
            }

            string GiftsInRange = string.Empty;
            string NoGiftsInRange = string.Empty;

            // get range of dates that gifts must be in
            if ((ucoGiftsInRange.RangeTable != null) && (ucoGiftsInRange.RangeTable.Rows.Count > 0))
            {
                foreach (DataRow Row in ucoGiftsInRange.RangeTable.Rows)
                {
                    if (!string.IsNullOrEmpty(GiftsInRange))
                    {
                        GiftsInRange += ",";
                    }

                    GiftsInRange += Row["From"].ToString() + " - " + Row["To"].ToString();
                }
            }

            ACalc.AddParameter("param_gifts_in_range", GiftsInRange);

            // get range of dates that gifts must not be in
            if ((ucoNoGiftsInRange.RangeTable != null) && (ucoNoGiftsInRange.RangeTable.Rows.Count > 0))
            {
                foreach (DataRow Row in ucoNoGiftsInRange.RangeTable.Rows)
                {
                    if (!string.IsNullOrEmpty(NoGiftsInRange))
                    {
                        NoGiftsInRange += ",";
                    }

                    NoGiftsInRange += Row["From"].ToString() + " - " + Row["To"].ToString();
                }
            }

            ACalc.AddParameter("param_nogifts_in_range", NoGiftsInRange);

            if (string.IsNullOrEmpty(txtMinimumAmount.Text))
            {
                ACalc.AddParameter("param_minimum_amount", 0);
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            rbtExtract.Checked = AParameters.Get("param_extract").ToBool();
            rbtAllPartners.Checked = AParameters.Get("param_all_partners").ToBool();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();

            ucoGiftsInRange.Year = DateTime.Now.Year - 1;

            string GiftsInRange = AParameters.Get("param_gifts_in_range").ToString();
            string NoGiftsInRange = AParameters.Get("param_nogifts_in_range").ToString();

            if (!string.IsNullOrEmpty(GiftsInRange))
            {
                string[] GiftsInRangeArray = GiftsInRange.Split(',');

                DataRow NewInRow = ucoGiftsInRange.RangeTable.NewRow();

                foreach (string Range in GiftsInRangeArray)
                {
                    if (Range.Length == 23) // range must be in this format: yyyy-mm-dd - yyyy-mm-dd
                    {
                        NewInRow["From"] = Range.Substring(0, 10);
                        NewInRow["To"] = Range.Substring(13, 10);
                        ucoGiftsInRange.RangeTable.Rows.Add(NewInRow);
                    }
                }
            }

            if (!string.IsNullOrEmpty(NoGiftsInRange))
            {
                string[] NoGiftsInRangeArray = NoGiftsInRange.Split(',');

                DataRow NewInRow = ucoNoGiftsInRange.RangeTable.NewRow();

                foreach (string Range in NoGiftsInRangeArray)
                {
                    if (Range.Length == 23) // range must be in this format: yyyy-mm-dd - yyyy-mm-dd
                    {
                        NewInRow["From"] = Range.Substring(0, 10);
                        NewInRow["To"] = Range.Substring(13, 10);
                        ucoNoGiftsInRange.RangeTable.Rows.Add(NewInRow);
                    }
                }
            }
        }

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && !paramsDictionary.ContainsKey(p.name))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("SYBUNT", paramsDictionary);

            if (this.IsDisposed) // There's no cancel function as such - if the user has pressed Esc the form is closed!
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            DataView Dv = ReportTable.DefaultView;

            // sort the table
            Dv.Sort = ACalc.GetParameters().Get("param_sortby_readable").ToString().Replace(" ", "");

            ReportTable = Dv.ToTable();

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "SYBUNT");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Motivation Response data found."), "Motivation Response");
            }

            return HasData;
        }
    }
}