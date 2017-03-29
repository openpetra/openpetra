//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
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
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using System.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmTotalGiftsThroughField
    {
        private Int32 FLedgerNumber;
        private bool FTaxDeductiblePercentageEnabled = false;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();

                FTaxDeductiblePercentageEnabled =
                    TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData(true, "TotalGiftsThroughField",
                false,
                new string[] { "MonthlyGifts" },
                ACalc,
                this,
                false);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int detailYears = Convert.ToInt16(txtYearsDetail.Text);
            int summaryYears = Convert.ToInt16(txtYearsSummary.Text);

            if ((AReportAction == TReportActionEnum.raGenerate)
                && ((detailYears > 99) || (detailYears < 0) || (summaryYears > 99) || (summaryYears < 0)))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Report Years"),
                    Catalog.GetString("Set the year range between 1 and 99"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            //
            // I need to get the name of the current ledger..

            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ALedgerTable LedgerDetailsTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails,
                FLedgerNumber);
            ALedgerRow Row = LedgerDetailsTable[0];
            String CurrencyName = (cmbCurrency.SelectedItem.ToString() == "Base") ? Row.BaseCurrency : Row.IntlCurrency;

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_name", CurrencyName);

            Int32 Years = Math.Max(detailYears, summaryYears);
            DateTime StartDate = new DateTime(DateTime.Now.Year - Years, 1, 1);

            ACalc.AddParameter("param_StartDate", StartDate);
            ACalc.AddParameter("param_DetailYears", detailYears);
            ACalc.AddParameter("param_SummaryYears", summaryYears);
            ACalc.AddParameter("param_TD", FTaxDeductiblePercentageEnabled);
        }
    }
}