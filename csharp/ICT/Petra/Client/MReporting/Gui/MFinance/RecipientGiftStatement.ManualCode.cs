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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using Ict.Common.Verification;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmRecipientGiftStatement
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

                ReportTypeChanged(this, null);

                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
                this.tabReportSettings.Controls.Remove(tpgColumnSettings);     // Column Settings is not supported in the FastReports based solution.
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_recipientkey", txtRecipient.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);

            if (!dtpFromDate.Date.HasValue || !dtpToDate.Date.HasValue)
            {
                FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                        Catalog.GetString("Please check the entry of the Start and End dates."),
                        Catalog.GetString("Invalid Date entered."),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                if (dtpFromDate.Date.Value > dtpToDate.Date.Value)
                {
                    FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                            Catalog.GetString("Start Date must not be later than End Date"),
                            Catalog.GetString("Invalid Date period."),
                            TResultSeverity.Resv_Critical));
                }
            }

            if (dtpToDate.Date.HasValue)
            {
                Int32 ToDateYear = dtpToDate.Date.Value.Year;
                //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
                DateTime FromDateThisYear = new DateTime(ToDateYear, 1, 1);
                DateTime ToDatePreviousYear = new DateTime(ToDateYear - 1, 12, 31);
                DateTime FromDatePreviousYear = new DateTime(ToDateYear - 1, 1, 1);

                ACalc.AddParameter("param_from_date_this_year", FromDateThisYear);
                ACalc.AddParameter("param_to_date_previous_year", ToDatePreviousYear);
                ACalc.AddParameter("param_from_date_previous_year", FromDatePreviousYear);
            }

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

        private void SetControlsManual(TParameterList AParameters)
        {
            txtRecipient.Text = AParameters.Get("param_recipientkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
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

            // get data for this report
            DataSet ReportDataSet = TRemote.MReporting.WebConnectors.GetReportDataSet("RecipientGiftStatement", paramsDictionary);

            if (TRemote.MReporting.WebConnectors.DataTableGenerationWasCancelled() || this.IsDisposed)
            {
                return false;
            }

            // if no recipients
            if ((ReportDataSet.Tables["Recipients"] == null) || (ReportDataSet.Tables["Recipients"].Rows.Count == 0))
            {
                MessageBox.Show(Catalog.GetString("No Recipients found."), "Recipient Gift Statement");
                return false;
            }

            // register datatables with the report
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["Recipients"], "Recipients");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["Donors"], "Donors");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDataSet.Tables["PartnersAddresses"], "DonorAddresses");

            //
            // I need the name of the ledger, and the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            return true;
        }

        #region Event Handlers

        private void ReportTypeChanged(object ASender, System.EventArgs AEventArgs)
        {
            string ReportType = (string)cmbReportType.SelectedItem;

            if (ReportType == null)
            {
                return;
            }

            if ((ReportType == "List") || (ReportType == "Email"))
            {
                cmbCurrency.Enabled = false;
            }
            else
            {
                cmbCurrency.Enabled = true;
            }

            if ((ReportType == "List")
                || (ReportType == "Email"))
            {
                txtRecipient.PartnerClass = "WORKER";
            }
            else
            {
                txtRecipient.PartnerClass = "WORKER,UNIT";
            }
        }

        private void RecipientSelectionChanged(object sender, EventArgs e)
        {
            if (tpgReportSorting.Enabled && rbtPartner.Checked)
            {
                tpgReportSorting.Enabled = false;
                this.Refresh();
            }
            else if (!tpgReportSorting.Enabled && (rbtAllRecipients.Checked || rbtExtract.Checked))
            {
                tpgReportSorting.Enabled = true;
                this.Refresh();
            }
        }

        #endregion
    }
}