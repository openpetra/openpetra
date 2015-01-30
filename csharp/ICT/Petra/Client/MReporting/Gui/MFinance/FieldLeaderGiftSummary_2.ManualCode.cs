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
using System.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// This report shows the income posted to all your Local cost centres, in account order. Each recipient
    /// or Motivation Detail is listed separately.
    /// </summary>
    public partial class TFrmFieldLeaderGiftSummary_2
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

        private void RunOnceOnActivationManual()
        {
            cmbCurrency.SelectedIndex = 0;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
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

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("TransactionCount", 0);
            ACalc.AddParameter("TransactionCountAccount", 0);
            ACalc.AddParameter("SumDebitAccount", 0);
            ACalc.AddParameter("SumCreditAccount", 0);
            ACalc.AddParameter("SumTotalDebitAccount", 0);
            ACalc.AddParameter("SumTotalCreditAccount", 0);

            int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter <= MaxColumns; ++Counter)
            {
                String ColumnName = ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();

                if (ColumnName == "Debits")
                {
                    ACalc.AddParameter("param_debit_column", Counter);
                }

                if (ColumnName == "Credits")
                {
                    ACalc.AddParameter("param_credit_column", Counter);
                }
            }
        }
    }
}