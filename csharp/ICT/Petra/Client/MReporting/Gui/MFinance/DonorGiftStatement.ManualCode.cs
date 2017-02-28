//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using GNU.Gettext;
using Ict.Common;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Client.App.Core;
using System.Threading;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmDonorGiftStatement
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
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);

                this.tabReportSettings.Controls.Remove(tpgColumnSettings);     // Column Settings is not supported in the FastReports based solution.
                this.tabReportSettings.Controls.Remove(tpgAdditionalSettings); // We're also not doing this!
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();
            }
        }

        private void RunOnceOnActivationManual()
        {
            Boolean useGovId = TSystemDefaults.GetBooleanDefault("GovIdEnabled", false);

            if (useGovId)
            {
                pnlRequireBpkCode.Visible = true;
                chkRequireBpkCode.CheckedChanged += ChkRequireBpkCode_CheckedChanged;
                chkRequireNoBpkCode.CheckedChanged += ChkRequireNoBpkCode_CheckedChanged;
            }
        }

        private void ChkRequireNoBpkCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequireNoBpkCode.Checked)
            {
                chkRequireBpkCode.Checked = false;
            }
        }

        private void ChkRequireBpkCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequireBpkCode.Checked)
            {
                chkRequireNoBpkCode.Checked = false;
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (AReportAction == TReportActionEnum.raGenerate)
            {
                if (rbtPartner.Checked && (txtDonor.Text == "0000000000"))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("No donor selected."),
                        Catalog.GetString("Please select a donor."),
                        TResultSeverity.Resv_Critical);

                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                if (rbtExtract.Checked && (txtExtract.Text == ""))
                {
                    TVerificationResult VerificationMessage = new TVerificationResult(
                        Catalog.GetString("Enter an extract name"),
                        Catalog.GetString("No extract name entered!"), TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationMessage);
                }

                if (txtMinAmount.NumberValueInt > txtMaxAmount.NumberValueInt)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Gift Limit wrong."),
                        Catalog.GetString("Minimum Amount can't be greater than Maximum Amount."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                if (!dtpFromDate.ValidDate() || !dtpToDate.ValidDate())
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Date format problem"),
                        Catalog.GetString("Please check the date entry."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                if ((cmbReportType.SelectedItem.ToString() == "Complete")
                    && (dtpFromDate.Date > dtpToDate.Date))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("From date is later than to date."),
                        Catalog.GetString("Please change from date or to date."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            ACalc.AddParameter("ControlSource", "", ReportingConsts.HEADERCOLUMN);
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_donorkey", txtDonor.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);

            //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
            DateTime FromDateThisYear = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime ToDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 12, 31);
            DateTime FromDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 1, 1);

            ACalc.AddParameter("param_end_date_this_year", DateTime.Today);
            ACalc.AddParameter("param_start_date_this_year", FromDateThisYear);
            ACalc.AddParameter("param_end_date_previous_year", ToDatePreviousYear);
            ACalc.AddParameter("param_start_date_previous_year", FromDatePreviousYear);

            if (cmbReportType.SelectedItem.ToString() == "Totals")
            {
                DateTime FromDate = new DateTime(DateTime.Today.Year - 3, 1, 1);
                ACalc.RemoveParameter("param_from_date");
                ACalc.RemoveParameter("param_to_date");
                ACalc.AddParameter("param_from_date", FromDate);
                ACalc.AddParameter("param_to_date", DateTime.Today);

                ACalc.AddParameter("Month0", 1);
                ACalc.AddParameter("Month1", 2);
                ACalc.AddParameter("Year0", DateTime.Today.Year);
                ACalc.AddParameter("Year1", DateTime.Today.Year - 1);
                ACalc.AddParameter("Year2", DateTime.Today.Year - 2);
                ACalc.AddParameter("Year3", DateTime.Today.Year - 3);

                int ColumnCounter = 0;
                ACalc.AddParameter("param_calculation", "Month", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Year-0", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Count-0", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.8, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Year-1", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Count-1", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.8, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Year-2", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Count-2", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.8, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Year-3", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
                ++ColumnCounter;
                ACalc.AddParameter("param_calculation", "Count-3", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", (float)0.8, ColumnCounter);
                ++ColumnCounter;
                ACalc.SetMaxDisplayColumns(ColumnCounter);
            }
            else
            {
                int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

                for (int Counter = 0; Counter <= MaxColumns; ++Counter)
                {
                    String ColumnName = ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();

                    if (ColumnName == "Gift Amount")
                    {
                        ACalc.AddParameter("param_gift_amount_column", Counter);
                    }
                }
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            txtDonor.Text = AParameters.Get("param_donorkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }

        private void ReportTypeChanged(object sender, EventArgs e)
        {
            bool IsTotal = (cmbReportType.SelectedItem.ToString() == "Totals");

            if (IsTotal)
            {
                tpgColumnSettings.Text = Catalog.GetString("(Disabled)");
            }
            else
            {
                tpgColumnSettings.Text = Catalog.GetString("Column Settings");
            }

            tpgColumnSettings.Enabled = !IsTotal;
            txtMinAmount.Enabled = !IsTotal;
            txtMaxAmount.Enabled = !IsTotal;
            dtpFromDate.Enabled = !IsTotal;
            dtpToDate.Enabled = !IsTotal;
        }

        private void DonorSelectionChanged(object sender, EventArgs e)
        {
            if (tpgReportSorting.Enabled && rbtPartner.Checked)
            {
                tpgReportSorting.Enabled = false;
                this.Refresh();
            }
            else if (!tpgReportSorting.Enabled && (rbtAllDonors.Checked || rbtExtract.Checked))
            {
                tpgReportSorting.Enabled = true;
                this.Refresh();
            }
        }

        // This function is necessary because there seemed to be no way to get the DataSet back from GetReportDataSet().
        // I tested the obvious (explicit refs didn't work either):
        // DataSet ReportDataSet = null;
        // Thread t = new Thread(() => { ReportDataSet = TRemote.MReporting.WebConnectors.GetReportDataSet("DonorGiftStatement", paramsDictionary); });
        // but ReportDataSet was null afterwards, even if I did a Thread.Join or while (!ThreadFinished){Thread.Sleep(50);}
        private void GetDGSDataSet(ref DataSet Results, Dictionary <String, TVariant>AParameters, ref Boolean ThreadFinished, TRptCalculator ACalc)
        {
            Results = TRemote.MReporting.WebConnectors.GetReportDataSet("DonorGiftStatement", AParameters);

            if ((Results.Tables["Donors"] != null) && (Results.Tables["Donors"].Rows.Count != 0))
            {
                // Moved this lot in here from LoadReportData() and added AWaitForThreadComplete so that we can keep the progress bar open during this step, which in
                // testing on a large report (~2000 pages) was rather substantial (10-15 seconds) which might lead the user to think it had hung.
                Boolean useGovId = TSystemDefaults.GetBooleanDefault("GovIdEnabled", false);
                ACalc.AddParameter("param_use_gov_id", useGovId);

                // Register datatables with the report
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(Results.Tables["Donors"], "Donors");
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(Results.Tables["PartnersAddresses"], "DonorAddresses");
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(Results.Tables["Recipients"], "Recipients");
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(Results.Tables["Totals"], "Totals");
                FPetraUtilsObject.FFastReportsPlugin.RegisterData(Results.Tables["TaxRef"], "TaxRef");
            }

            ThreadFinished = true;
        }

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            //
            // I need the name of the ledger, and the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            ACalc.AddStringParameter("param_recipient", "All Recipients");

            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && !paramsDictionary.ContainsKey(p.name))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            // get data for this report
            // Previous code:
            // DataSet ReportDataSet = TRemote.MReporting.WebConnectors.GetReportDataSet("DonorGiftStatement", paramsDictionary);
            DataSet ReportDataSet = null;
            Boolean ThreadFinished = false;
            Thread t = new Thread(() => GetDGSDataSet(ref ReportDataSet, paramsDictionary, ref ThreadFinished, ACalc));
            using (TProgressDialog dialog = new TProgressDialog(t, AWaitForThreadComplete : true))
            {
                dialog.ShowDialog();
            }

            while (!ThreadFinished)
            {
                Thread.Sleep(50);
            }

            if (TRemote.MReporting.WebConnectors.DataTableGenerationWasCancelled() || this.IsDisposed)
            {
                return false;
            }

            // if no Donors
            if ((ReportDataSet.Tables["Donors"] == null) || (ReportDataSet.Tables["Donors"].Rows.Count == 0))
            {
                MessageBox.Show(Catalog.GetString("No Donors found."), "Donor Gift Statement");
                return false;
            }

            // Next few lines were moved from here into GetDGSDataSet()
            //Boolean useGovId = TSystemDefaults.GetBooleanDefault("GovIdEnabled", false);
            //ACalc.AddParameter("param_use_gov_id", useGovId);

            //// Register datatables with the report
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["Donors"], "Donors");
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["PartnersAddresses"], "DonorAddresses");
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["Recipients"], "Recipients");
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["Totals"], "Totals");
            //FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["TaxRef"], "TaxRef");
            return true;
        }
    }
}