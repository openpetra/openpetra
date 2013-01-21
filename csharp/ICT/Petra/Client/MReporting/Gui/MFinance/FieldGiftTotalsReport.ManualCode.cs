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
    public partial class TFrmFieldGiftTotalsReport
    {
        private Int32 FLedgerNumber;
        private DataTable FFieldTable;

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

        private void InitFieldList()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = "Field Name";
            string ValueMember = "Field Key";

            FFieldTable = TRemote.MFinance.Reporting.WebConnectors.GetReceivingFields(FLedgerNumber, out DisplayMember, out ValueMember);

            DataColumn FirstColumn = new DataColumn(CheckedMember, typeof(bool));

            FirstColumn.DefaultValue = false;
            FFieldTable.Columns.Add(FirstColumn);

            clbFields.Columns.Clear();
            clbFields.AddCheckBoxColumn("", FFieldTable.Columns[CheckedMember], 17, false);
            clbFields.AddTextColumn(Catalog.GetString("Field Key"), FFieldTable.Columns[ValueMember], 80);
            clbFields.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns[DisplayMember], 200);
            clbFields.DataBindGrid(FFieldTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
        }

        private void UnselectAllFields(object sender, EventArgs e)
        {
            clbFields.ClearSelected();
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((AReportAction == TReportActionEnum.raGenerate)
                && (clbFields.GetCheckedStringList().Length == 0))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Please select at least one field."),
                    Catalog.GetString("No fields selected!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

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

            ACalc.AddParameter("Year0", DateTime.Today.Year);
            ACalc.AddParameter("param_Year0", DateTime.Today.Year);
            ACalc.AddParameter("Year3", DateTime.Today.Year - 3);

            ACalc.AddParameter("Month0", 1);
            ACalc.AddParameter("Month1", 2);
            ACalc.AddParameter("MonthCombined", 0);
            ACalc.AddParameter("CountCombined", 0);

            int ColumnCounter = 0;
            ACalc.AddParameter("param_calculation", "Month", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)4, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "AmountWorker", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "CountWorker", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "AmountField", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "CountField", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "AmountCombined", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "CountCombined", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)1, ColumnCounter);
            ++ColumnCounter;

            ACalc.SetMaxDisplayColumns(ColumnCounter);
        }
    }
}