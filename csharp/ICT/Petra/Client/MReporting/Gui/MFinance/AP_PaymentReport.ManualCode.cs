//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Windows.Forms;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_PaymentReport
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// The main screen will call this on creation, to give me my Ledger Number.
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
            ACalc.AddParameter("MaxDisplayColumns", 7);
        }

        public static void CreateReportNoGui(Int32 ALedgerNumber, Int32 APaymentNumber, Form Owner)
        {
            TRptCalculator Calculator = new TRptCalculator();
            TFrmPetraReportingUtils.InitialiseCalculator(
                Calculator, 
                "Finance/AccountsPayable/AP_PaymentReport.xml,Finance/finance.xml,common.xml", 
                "",
                "APPaymentReport");

            Calculator.AddParameter("param_payment_num_from_i", APaymentNumber);
            Calculator.AddParameter("param_payment_num_to_i", APaymentNumber);
/*
            Calculator.AddParameter("param_payment_date_from_i", DateTime.Now);
            Calculator.AddParameter("param_payment_date_to_i", DateTime.Now);
*/
            Calculator.AddParameter("param_ledger_number_i", ALedgerNumber);
            Calculator.AddParameter("MaxDisplayColumns", 7);

            TFrmPetraReportingUtils.GenerateReport(Calculator, Owner, "APPaymentReport", true);
        }
    }
}