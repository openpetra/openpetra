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
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmNewDonorReport
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

        private void InitializeManualCode()
        {
            txtRecipient.PartnerClass = "WORKER,UNIT,FAMILY";
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);

            // if FastReport, then ignore columns tab
            if ((FPetraUtilsObject.GetCallerForm() != null) && FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
                tabReportSettings.Controls.Remove(tpgAdditionalSettings);
            }

            ucoMotivationCriteria.ShowchkShowDetailedMotivationInformation();
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("NewDonorReport",
                true,
                new string[] { "NewDonorReportDonations", "DonorAddresses" },
                ACalc,
                this,
                false,
                true,
                FLedgerNumber);
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // make sure that for each group one radio button is selected
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrRecipientSelection, FPetraUtilsObject);
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrDonorSelection, FPetraUtilsObject);
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

            ACalc.AddParameter("param_all_partners", rbtAllPartners.Checked);
            ACalc.AddParameter("param_extract", rbtExtract.Checked);

            if (rbtExtract.Checked)
            {
                ACalc.AddParameter("param_extract_name", txtExtract.Text);
            }

            if (rbtAllRecipients.Checked)
            {
                ACalc.AddParameter("param_recipientkey", "0");
            }
            else
            {
                ACalc.AddParameter("param_recipientkey", txtRecipient.Text);
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
            DateTime dtpStartDateDate = AParameters.Get("param_start_date").ToDate();

            if ((dtpStartDateDate <= DateTime.MinValue)
                || (dtpStartDateDate >= DateTime.MaxValue))
            {
                dtpStartDateDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            dtpStartDate.Date = dtpStartDateDate;
            rbtExtract.Checked = AParameters.Get("param_extract").ToBool();
            rbtAllPartners.Checked = AParameters.Get("param_all_partners").ToBool();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
            txtRecipient.Text = AParameters.Get("param_recipientkey").ToString();

            if (Convert.ToInt64(txtRecipient.Text) == 0)
            {
                rbtAllRecipients.Checked = true;
            }
            else
            {
                rbtRecipient.Checked = true;
            }
        }
    }
}