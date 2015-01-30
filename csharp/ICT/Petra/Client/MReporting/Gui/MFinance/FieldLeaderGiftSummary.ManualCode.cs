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
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared;

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
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
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
            clbFields.AddTextColumn(Catalog.GetString("Field Key"), FFieldTable.Columns[ValueMember], 100);
            clbFields.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns[DisplayMember], 200);
            clbFields.DataBindGrid(FFieldTable, ValueMember, CheckedMember, ValueMember, false, true, false);
            FPetraUtilsObject.LoadDefaultSettings(); // This was done previously, but it was too early.
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

            if ((AReportAction == TReportActionEnum.raGenerate) && (rbtAllFields.Checked))
            {
                ACalc.AddParameter("param_clbFields", this.clbFields.GetAllStringList());
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

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ALedgerTable LedgerDetailsTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails);
            ALedgerRow Row = LedgerDetailsTable[0];
            Int32 LedgerYear = Row.CurrentFinancialYear;
            Int32 NumPeriods = Row.NumberOfAccountingPeriods;
            String CurrencyName = (cmbCurrency.SelectedItem.ToString() == "Base") ? Row.BaseCurrency : Row.IntlCurrency;
            ACalc.AddStringParameter("param_currency_name", CurrencyName);

            ACalc.AddParameter("param_year0", DateTime.Today.Year);
            ACalc.AddParameter("param_year1", DateTime.Today.Year - 1);
            ACalc.AddParameter("param_year2", DateTime.Today.Year - 2);
            ACalc.AddParameter("param_year3", DateTime.Today.Year - 3);
        } // Read Controls Manual

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

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("FieldLeaderGiftSummary", paramsDictionary);
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "FieldLeaderGiftSummary");
            return true;
        } // Load Report Data
    }
}