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
    public partial class TFrmTotalGiftsThroughField
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
            int Years = Convert.ToInt16(txtYears.Text);

            if ((AReportAction == TReportActionEnum.raGenerate)
                && ((Years > 4) || (Years < 1)))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Set the year range between 1 and 4"),
                    Catalog.GetString("Wrong year range entered"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            ACalc.AddParameter("param_YearBlock", DateTime.Today.Year);

            int ColumnCounter = 0;
//            ACalc.AddParameter("param_calculation", "MonthName", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthWorker", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthWorkerCount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthField", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthFieldCount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthTotal", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
            ++ColumnCounter;
//            ACalc.AddParameter("param_calculation", "MonthTotalCount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;
//          ACalc.AddParameter("param_calculation", "YearMonthlyAverage", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
            ++ColumnCounter;
//          ACalc.AddParameter("param_calculation", "YearMonthlyAverageCount", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
/*
            ++ColumnCounter;
            ACalc.SetMaxDisplayColumns(ColumnCounter);
*/
        }
    }
}