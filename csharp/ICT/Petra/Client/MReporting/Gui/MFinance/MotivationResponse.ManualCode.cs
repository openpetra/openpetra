//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmMotivationResponse
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

                ReportTypeChanged(this, null);

                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // if fast reports isn't working then close the screen
            if ((FPetraUtilsObject.GetCallerForm() != null) && !FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                MessageBox.Show("No alternative reporting engine is available for this report. This screen will now be closed.", "Reporting engine");
                this.Close();
            }
        }

        private void InitializeManualCode()
        {
            cmbMailingCode.cmbCombobox.AllowBlankValue = true;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            // add this manually as string parameter as automated code would add mailing codes that are numbers wrongly as integers
            ACalc.AddStringParameter("param_mailing_code", this.cmbMailingCode.GetSelectedString());

            if (dtpToDate.Date.HasValue)
            {
                Int32 ToDateYear = dtpToDate.Date.Value.Year;
                //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
                DateTime FromDateThisYear = new DateTime(ToDateYear, 1, 1);
                DateTime ToDatePreviousYear = new DateTime(ToDateYear - 1, 12, 31);
                DateTime FromDatePreviousYear = new DateTime(ToDateYear - 1, 1, 1);

                ACalc.AddParameter("param_from_date_this_year", FromDateThisYear);
                ACalc.AddParameter("param_to_date_previous_year", ToDatePreviousYear);
                ACalc.AddParameter("param_from_date_previous_year", FromDatePreviousYear);
            }

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
            DateTime dtpFromDateDate = AParameters.Get("param_from_date").ToDate();

            cmbMailingCode.SetSelectedString(AParameters.Get("param_mailing_code").ToString(), -1);

            if ((dtpFromDateDate <= DateTime.MinValue)
                || (dtpFromDateDate >= DateTime.MaxValue))
            {
                dtpFromDate.Date = new DateTime(DateTime.Today.Year, 1, 1);
            }
        }

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            ArrayList reportParam = ACalc.GetParameters().Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("MotivationResponse", paramsDictionary);

            if (this.IsDisposed) // There's no cancel function as such - if the user has pressed Esc the form is closed!
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "MotivationResponse");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Motivation Response data found."), "Motivation Response");
            }

            return HasData;
        }

        #region Event Handlers

        private void ReportTypeChanged(object ASender, System.EventArgs AEventArgs)
        {
            if (cmbReportType.GetSelectedString() == "Totals")
            {
                chkSuppressDetail.Enabled = false;
            }
            else
            {
                chkSuppressDetail.Enabled = true;
            }
        }

        #endregion
    }
}