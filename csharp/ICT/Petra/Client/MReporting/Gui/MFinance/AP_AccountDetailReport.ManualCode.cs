//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_AccountDetailReport
    {
        private Int32 FLedgerNumber;
        private String FFromAccountCode;
        private String FToAccountCode;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();

                TFinanceControls.InitialiseAccountList(ref cmbAccountFrom, FLedgerNumber, true, false, false, false);
                cmbAccountFrom.SetSelectedString(FFromAccountCode);
                TFinanceControls.InitialiseAccountList(ref cmbAccountTo, FLedgerNumber, true, false, false, false);
                cmbAccountTo.SetSelectedString(FToAccountCode);
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (AReportAction == TReportActionEnum.raGenerate)
            {
                if (!dtpFromDate.ValidDate() || !dtpToDate.ValidDate())
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Date format problem"),
                        Catalog.GetString("Please check the date entry."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                if (dtpFromDate.Date > dtpToDate.Date)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("From date is later than to date."),
                        Catalog.GetString("Please change from date or to date."),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            ACalc.AddParameter("param_currency", "Base");
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            // Set the values for accumulating the costs to 0
            ACalc.AddParameter("CostCentreCredit", 0);
            ACalc.AddParameter("CostCentreDebit", 0);
            ACalc.AddParameter("AccountCodeCredit", 0);
            ACalc.AddParameter("AccountCodeDebit", 0);
            ACalc.AddParameter("TotalCredit", 0);
            ACalc.AddParameter("TotalDebit", 0);

            int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            // we need to know some indices of the columns
            for (int Counter = 0; Counter < MaxColumns; ++Counter)
            {
                String ColumnName = "param_column_" + ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();
                ACalc.AddParameter(ColumnName, Counter);
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            FFromAccountCode = AParameters.Get("param_account_from").ToString();
            FToAccountCode = AParameters.Get("param_account_to").ToString();
        }
    }
}