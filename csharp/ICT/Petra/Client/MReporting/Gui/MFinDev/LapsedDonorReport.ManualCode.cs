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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmLapsedDonorReport
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

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // make sure that for each group one radio button is selected
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(grpOtherSelection, FPetraUtilsObject);
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrSorting, FPetraUtilsObject);
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrFormatCurrency, FPetraUtilsObject);

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

            ACalc.AddParameter("param_recipientkey", txtRecipient.Text);

            if ((txtMotivationDetail.Text.Length == 0)
                || (txtMotivationDetail.Text == "*"))
            {
                ACalc.AddParameter("param_motivation_detail", "%");
            }
            else
            {
                ACalc.AddParameter("param_motivation_detail", txtMotivationDetail.Text.Replace('*', '%'));
            }

            if ((txtMotivationGroup.Text.Length == 0)
                || (txtMotivationGroup.Text == "*"))
            {
                ACalc.AddParameter("param_motivation_group", "%");
            }
            else
            {
                ACalc.AddParameter("param_motivation_group", txtMotivationGroup.Text.Replace('*', '%'));
            }

            int NumDays = Convert.ToInt32(txtToleranceDays.Text);

            DateTime SelectionStartDate = dtpEndDate.Date.Value.AddDays(-NumDays);
            DateTime SelectionEndDate = dtpEndDate.Date.Value.AddDays(NumDays);
            ACalc.AddParameter("param_selection_start_date", SelectionStartDate);
            ACalc.AddParameter("param_selection_end_date", SelectionEndDate);

            int ColumnNumbers = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter < ColumnNumbers; ++Counter)
            {
                String ParameterName = ACalc.GetParameters().Get("param_calculation", Counter).ToString();

                if (ParameterName == "Gift this year")
                {
                    ACalc.AddParameter("ColumnCaption", dtpEndDate.Date.Value.Year.ToString(), Counter);
                }
                else if (ParameterName == "Gift year - 1")
                {
                    ACalc.AddParameter("ColumnCaption", (dtpEndDate.Date.Value.Year - 1).ToString(), Counter);
                }
                else if (ParameterName == "Gift year - 2")
                {
                    ACalc.AddParameter("ColumnCaption", (dtpEndDate.Date.Value.Year - 2).ToString(), Counter);
                }
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            rbtExtract.Checked = AParameters.Get("param_extract").ToBool();
            rbtAllPartners.Checked = AParameters.Get("param_all_partners").ToBool();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
            txtRecipient.Text = AParameters.Get("param_recipientkey").ToString();

            txtMotivationGroup.Text = AParameters.Get("param_motivation_group").ToString().Replace('%', '*');
            txtMotivationDetail.Text = AParameters.Get("param_motivation_detail").ToString().Replace('%', '*');
        }
    }
}