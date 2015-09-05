//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmDonorGiftsToFieldsReport
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

            //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
            DateTime FromDateThisYear = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime ToDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 12, 31);
            DateTime FromDatePreviousYear = new DateTime(DateTime.Today.Year - 1, 1, 1);

            ACalc.AddParameter("param_to_date_this_year", DateTime.Today);
            ACalc.AddParameter("param_from_date_this_year", FromDateThisYear);
            ACalc.AddParameter("param_to_date_previous_year", ToDatePreviousYear);
            ACalc.AddParameter("param_from_date_previous_year", FromDatePreviousYear);

            ACalc.AddParameter("DonorAddress", "");

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

        private void SetControlsManual(TParameterList AParameters)
        {
            txtDonor.Text = AParameters.Get("param_donorkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }
    }
}