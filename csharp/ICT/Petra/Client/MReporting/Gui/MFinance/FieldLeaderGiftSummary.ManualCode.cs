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
    /// This report is for your local cost centres only, and prints the gift information which is found on the
    /// HOSAs for foreign ledgers. It lists all the people on your field with their total gifts for this period, the
    ///	previous year and current year, and is sorted alphabetically.
    /// </summary>
    public partial class TFrmFieldLeaderGiftSummary
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

        private void RunOnceOnActivationManual() // Formerly InitFieldList
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = "Field Name";
            string ValueMember = "Field Key";

            FFieldTable = TRemote.MFinance.Reporting.WebConnectors.GetReceivingFields(FLedgerNumber, out DisplayMember, out ValueMember);

            DataColumn FirstColumn = new DataColumn(CheckedMember, typeof(bool));

            FirstColumn.DefaultValue = false;
            FFieldTable.Columns.Add(FirstColumn);
            rbtAllFields.Select();
            cmbCurrency.SelectedIndex = 0;
            clbFields.Columns.Clear();
            clbFields.AddCheckBoxColumn("", FFieldTable.Columns[CheckedMember], 17, false);
            clbFields.AddTextColumn(Catalog.GetString("Field Key"), FFieldTable.Columns[ValueMember], 80);
            clbFields.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns[DisplayMember], 200);
            clbFields.DataBindGrid(FFieldTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
        }

        private void SelectAllFields(object sender, EventArgs e)
        {
            for (int Counter = 0; Counter < FFieldTable.Rows.Count; ++Counter)
            {
                FFieldTable.Rows[Counter]["CHECKED"] = true;
            }
        }

        private void UnselectAllFields(object sender, EventArgs e)
        {
            clbFields.ClearSelected();
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((AReportAction == TReportActionEnum.raGenerate)
                && rbtSelectedFields.Checked
                && (clbFields.GetCheckedStringList().Length == 0))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Please select at least one field."),
                    Catalog.GetString("No fields selected!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (dtpFromDate.Date > dtpToDate.Date))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("From date is later than to date."),
                    Catalog.GetString("Please change from date or to date."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
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
            ACalc.AddParameter("Year1", DateTime.Today.Year - 1);
            ACalc.AddParameter("Year2", DateTime.Today.Year - 2);
            ACalc.AddParameter("Year3", DateTime.Today.Year - 3);

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
    }
}