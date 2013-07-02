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
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmOneYearMonthlyGiving
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

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtPartner.Checked && (txtRecipient.Text == "0000000000")))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("No donor selected."),
                    Catalog.GetString("Please select a donor."),
                    TResultSeverity.Resv_Critical);

                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && rbtExtract.Checked
                && (txtExtract.Text == ""))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Enter an extract name."),
                    Catalog.GetString("No extract name entered!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            if ((!dtpFromDate.Date.HasValue) || (!dtpToDate.Date.HasValue))
            {
                if (AReportAction == TReportActionEnum.raGenerate)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Invalid dates."),
                        Catalog.GetString("Provide values for From date and To date."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }
            else
            {
                if ((AReportAction == TReportActionEnum.raGenerate)
                    && (dtpFromDate.Date > dtpToDate.Date))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("From date is later than to date."),
                        Catalog.GetString("Please change from date or to date."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                if ((AReportAction == TReportActionEnum.raGenerate)
                    && (dtpFromDate.Date.Value.Year != dtpToDate.Date.Value.Year))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Year value in from-date is different than from year value in to-date."),
                        Catalog.GetString("Please use the same year."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                ACalc.AddParameter("param_year", dtpFromDate.Date.Value.Year);
                ACalc.AddParameter("param_months", dtpToDate.Date.Value.Month - dtpFromDate.Date.Value.Month + 1);
            }

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_recipient_key", txtRecipient.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);
            if (this.cmbCurrency.SelectedItem == null)
            {
                this.cmbCurrency.SelectedIndex = 0;  // I don't mind what you select - just don't select nothing!
            }

            int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter <= MaxColumns; ++Counter)
            {
                String ColumnName = ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();
                ACalc.AddParameter(ColumnName, Counter);
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            txtRecipient.Text = AParameters.Get("param_recipient_key").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }
    }
}