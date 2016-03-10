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
using System.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using GNU.Gettext;
using Ict.Common;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmDonorGiftsToFieldsReport
    {
        private Int32 FLedgerNumber;

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                string CheckedMember = "CHECKED";
                string DisplayMember = "Field Name";
                string ValueMember = "Field Key";

                DataTable Table = TRemote.MFinance.Reporting.WebConnectors.GetReceivingFields(FLedgerNumber, out DisplayMember, out ValueMember);

                DataColumn FirstColumn = new DataColumn(CheckedMember, typeof(bool));

                FirstColumn.DefaultValue = false;
                Table.Columns.Add(FirstColumn);

                clbFields.Columns.Clear();
                clbFields.AddCheckBoxColumn("", Table.Columns[CheckedMember], 17, false);
                clbFields.AddTextColumn(Catalog.GetString("Field Key"), Table.Columns[ValueMember], 100);
                clbFields.AddTextColumn(Catalog.GetString("Field Name"), Table.Columns[DisplayMember], 200);
                clbFields.DataBindGrid(Table, ValueMember, CheckedMember, ValueMember, false, true, false);
            }
        }

        private void InitReceivingFieldList()
        {
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            //
            // Verify Min and Max amounts:
            if (txtMinAmount.NumberValueInt > txtMaxAmount.NumberValueInt)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Gift Limit wrong."),
                    Catalog.GetString("Minimum Amount can't be greater than Maximum Amount."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_donorkey", txtDonor.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);

            //
            // Verify date fields
            if (dtpFromDate.ValidDate(true))
            {
                ACalc.AddParameter("param_StartDate", this.dtpFromDate.Date);
            }

            if (dtpToDate.ValidDate(true))
            {
                ACalc.AddParameter("param_EndDate", this.dtpToDate.Date);
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            txtDonor.Text = AParameters.Get("param_donorkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            ArrayList reportParam = ACalc.GetParameters().Elems;
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param")
                    && (p.name != "param_calculation")
                    && !paramsDictionary.ContainsKey(p.name)
                    )
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("DonorGiftsToField", paramsDictionary);

            if (ReportTable == null)
            {
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "DonorGiftsToField");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ALedgerTable LedgerDetailsTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails);
            ALedgerRow Row = LedgerDetailsTable[0];
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            String CurrencyName = (cmbCurrency.SelectedItem.ToString() == "Base") ? Row.BaseCurrency : Row.IntlCurrency;
            ACalc.AddStringParameter("param_currency_name", CurrencyName);

            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            return true;
        }
    }
}