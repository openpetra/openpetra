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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmTotalGivingForRecipients
    {
        private Int32 FLedgerNumber;
        private DataTable FFieldTable;
        private DataTable FTypeTable;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                lblLedger.Text = Catalog.GetString("Ledger: ") + FLedgerNumber.ToString();
                PopulateReceivingFieldList();
                FPetraUtilsObject.LoadDefaultSettings(); // This was done previously, but it was too early.
            }
        }

        private void PopulateReceivingFieldList()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = "Field Name";
            string ValueMember = "Field Key";

            FFieldTable = TRemote.MFinance.Reporting.WebConnectors.GetReceivingFields(FLedgerNumber, out DisplayMember, out ValueMember, false);

            DataColumn FirstColumn = new DataColumn(CheckedMember, typeof(bool));

            FirstColumn.DefaultValue = false;
            FFieldTable.Columns.Add(FirstColumn);

            clbFields.Columns.Clear();
            clbFields.AddCheckBoxColumn("", FFieldTable.Columns[CheckedMember], 17, false);
            clbFields.AddTextColumn(Catalog.GetString("Field Key"), FFieldTable.Columns[ValueMember], 100);
            clbFields.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns[DisplayMember], 200);
            clbFields.DataBindGrid(FFieldTable, ValueMember, CheckedMember, ValueMember, false, true, false);

            FTypeTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerTypeList);

            DataColumn CheckedColumn = new DataColumn(CheckedMember, typeof(bool));
            CheckedColumn.DefaultValue = false;

            FTypeTable.Columns.Add(CheckedColumn);
            clbTypes.Columns.Clear();
            clbTypes.AddCheckBoxColumn("", FTypeTable.Columns[CheckedMember], 17);
            clbTypes.AddTextColumn(Catalog.GetString("Partner Type"), FTypeTable.Columns[PTypeTable.GetTypeCodeDBName()], 280);
            clbTypes.DataBindGrid(FTypeTable, PTypeTable.GetTypeCodeDBName(), CheckedMember,
                PTypeTable.GetTypeCodeDBName(), false, true, false);
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

        private void SelectAllTypes(object sender, EventArgs e)
        {
            for (int Counter = 0; Counter < FTypeTable.Rows.Count; ++Counter)
            {
                FTypeTable.Rows[Counter]["CHECKED"] = true;
            }
        }

        private void UnselectAllTypes(object sender, EventArgs e)
        {
            clbTypes.ClearSelected();
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if ((AReportAction == TReportActionEnum.raGenerate)
                && (rbtPartner.Checked && (txtRecipient.Text == "0000000000")))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("No recipient selected."),
                    Catalog.GetString("Please select a recipient."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && rbtExtract.Checked
                && (txtExtract.Text == ""))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Enter an extract name."),
                    Catalog.GetString("No extract name entered!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

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
                && rbtSelectedTypes.Checked
                && (clbTypes.GetCheckedStringList().Length == 0))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Please select at least one type."),
                    Catalog.GetString("No types selected!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            ACalc.AddParameter("param_recipient_key", txtRecipient.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
            DateTime FromDateThisYear = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime ToDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 12, 31);
            DateTime FromDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 1, 1);

            ACalc.AddParameter("Year0", DateTime.Today.Year);
            ACalc.AddParameter("Year1", DateTime.Today.Year - 1);
            ACalc.AddParameter("Year2", DateTime.Today.Year - 2);
            ACalc.AddParameter("Year3", DateTime.Today.Year - 3);

            ACalc.AddParameter("param_to_date_0", DateTime.Today);
            ACalc.AddParameter("param_from_date_0", FromDateThisYear);
            ACalc.AddParameter("param_to_date_1", ToDatePreviousYear);
            ACalc.AddParameter("param_from_date_1", FromDatePreviousYear);
            ACalc.AddParameter("param_to_date_2", ToDatePreviousYear.AddYears(-1));
            ACalc.AddParameter("param_from_date_2", FromDatePreviousYear.AddYears(-1));
            ACalc.AddParameter("param_to_date_3", ToDatePreviousYear.AddYears(-2));
            ACalc.AddParameter("param_from_date_3", FromDatePreviousYear.AddYears(-2));

            int ColumnCounter = 0;
            ACalc.AddParameter("param_calculation", "PartnerKey", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "DonorName", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)6.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "DonorClass", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "Year-0", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "Year-1", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "Year-2", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "Year-3", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.0, ColumnCounter);
            ++ColumnCounter;

            ACalc.SetMaxDisplayColumns(ColumnCounter);
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            txtRecipient.Text = AParameters.Get("param_recipient_key").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }
    }
}