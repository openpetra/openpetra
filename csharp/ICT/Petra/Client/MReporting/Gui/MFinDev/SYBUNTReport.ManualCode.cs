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
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((txtThisYear.Text == "0")
                || (txtLastYear.Text == "0"))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("No valid year entered."),
                    Catalog.GetString("Please enter a valid year."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
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

            if ((txtThisYear.Text != "0")
                && (txtLastYear.Text != "0"))
            {
                int LastYear = Convert.ToInt32(txtLastYear.Text);
                int ThisYear = Convert.ToInt32(txtThisYear.Text);

                if (LastYear >= ThisYear)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Wrong year entered."),
                        Catalog.GetString("'Gift given in year' must be less than 'No gifts in year'"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                //TODO: Calendar vs Financial Date Handling - Confirm if year end is assumed wrongly, i.e. financial year end does not necessarily = calendar year end
                ACalc.AddParameter("param_this_year_start_date", new DateTime(ThisYear, 1, 1));
                ACalc.AddParameter("param_this_year_end_date", new DateTime(ThisYear, 12, 31));
                ACalc.AddParameter("param_last_year_start_date", new DateTime(LastYear - 1, 1, 1));
                ACalc.AddParameter("param_last_year_end_date", new DateTime(LastYear - 1, 12, 31));
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            rbtExtract.Checked = AParameters.Get("param_extract").ToBool();
            rbtAllPartners.Checked = AParameters.Get("param_all_partners").ToBool();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }
    }
}