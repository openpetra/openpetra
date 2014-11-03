//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2013 by OM International
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.MReporting.Gui;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmRecipientTaxDeductiblePercentages
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

        private void RunOnceOnActivationManual()
        {
            // if fast reports isn't working then close the screen
            if ((FPetraUtilsObject.GetCallerForm() != null) && !FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                MessageBox.Show("No alternative reporting engine is available for this report. This screen will now be closed.", "Reporting engine");
                this.Close();
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtPartner.Checked && (txtRecipient.Text == "0000000000")))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("No recipient selected."),
                    Catalog.GetString("Please select a recipient."),
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

            ACalc.AddParameter("param_recipient_key", txtRecipient.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
        }

        //
        // New methods using the Fast-reports DLL:

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("RecipientTaxDeductPct", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "RecipientTaxDeductPct");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString(
                        "No Recipient Tax Deductible Percentages found for current Ledger."), "Recipient Tax Deductible Percentages");
            }

            return HasData;
        }
    }
}