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

                FTaxDeductiblePercentageEnabled = Convert.ToBoolean(
                    TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation") && (!paramsDictionary.ContainsKey(p.name)))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("TotalGiftsThroughField", paramsDictionary);
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "MonthlyGifts");
            return true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int Years = Convert.ToInt16(txtYears.Text);

            if ((AReportAction == TReportActionEnum.raGenerate)
                && ((Years > 8) || (Years < 1)))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Report Years"),
                    Catalog.GetString("Set the year range between 1 and 8"), TResultSeverity.Resv_Critical);
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

            ALedgerTable LedgerDetailsTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails);
            ALedgerRow Row = LedgerDetailsTable[0];
            Int32 LedgerYear = Row.CurrentFinancialYear;
            Int32 NumPeriods = Row.NumberOfAccountingPeriods;
            String CurrencyName = (cmbCurrency.SelectedItem.ToString() == "Base") ? Row.BaseCurrency : Row.IntlCurrency;
            Int32 CurrentPeriod = Row.CurrentPeriod;

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_name", CurrencyName);

            DateTime PeriodEndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(
                FLedgerNumber,
                LedgerYear,
                0,
                CurrentPeriod);

            Int32 PeriodThisYear = PeriodEndDate.Month;

            DateTime StartDate = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(
                FLedgerNumber,
                LedgerYear - Years + 1,
                0,
                1);

            DateTime EndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(
                FLedgerNumber,
                LedgerYear,
                0,
                NumPeriods);

            ACalc.AddParameter("param_PeriodThisYear", PeriodThisYear);
            ACalc.AddParameter("param_StartDate", StartDate);
            ACalc.AddParameter("param_EndDate", EndDate);
            ACalc.AddParameter("param_TD", FTaxDeductiblePercentageEnabled);
        }
    }
}