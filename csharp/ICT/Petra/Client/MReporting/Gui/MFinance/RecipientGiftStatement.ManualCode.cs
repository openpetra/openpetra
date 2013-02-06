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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmRecipientGiftStatement
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
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_recipientkey", txtRecipient.Text);
            ACalc.AddParameter("param_extract_name", txtExtract.Text);

            if (dtpToDate.Date.HasValue)
            {
                Int32 ToDateYear = dtpToDate.Date.Value.Year;
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
            txtRecipient.Text = AParameters.Get("param_recipientkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();
        }
        
        #region Event Handlers
        
        private void ReportTypeChanged(object ASender, System.EventArgs AEventArgs)
        {
            string ReportType = (string)cmbReportType.SelectedItem;
            
            if (ReportType == null) 
            {
                return;
            }
            
            if ((ReportType == "List")
               || (ReportType == "Email"))
            {
                txtRecipient.PartnerClass = "WORKER";
            }   
            else
            {
                txtRecipient.PartnerClass = "WORKER,UNIT";
            }
        }
        
        #endregion
    }
}