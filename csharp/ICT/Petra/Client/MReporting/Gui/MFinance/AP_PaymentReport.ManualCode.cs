//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Windows.Forms;
using Ict.Common.Printing;
using Ict.Petra.Shared.MFinance.AP.Data;

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

        private static void DefineReportColumns(TRptCalculator ACalc)
        {
            int ColumnCounter = 0;

            ACalc.AddParameter("param_calculation", "", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)6.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentNumber", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)5.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentBank", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentRef", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)4.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentDate", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentCurrency", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("param_calculation", "PaymentTotal", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)3.0, ColumnCounter++);
            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            DefineReportColumns(ACalc);
        }

        /// <summary>
        /// Call this to generate a payment report without showing the UI to the user.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AMinPaymentNumber"></param>
        /// <param name="AMaxPaymentNumber"></param>
        /// <param name="Owner"></param>
        public static void CreateReportNoGui(Int32 ALedgerNumber, Int32 AMinPaymentNumber, Int32 AMaxPaymentNumber, Form Owner)
        {
            TRptCalculator Calculator = new TRptCalculator();

            TFrmPetraReportingUtils.InitialiseCalculator(
                Calculator,
                "Finance/AccountsPayable/AP_PaymentReport.xml,Finance/finance.xml,common.xml",
                "",
                "APPaymentReport");

            Calculator.AddParameter("param_payment_num_from_i", AMinPaymentNumber);
            Calculator.AddParameter("param_payment_num_to_i", AMaxPaymentNumber);

/*
 *          Calculator.AddParameter("param_payment_date_from_i", DateTime.Now);
 *          Calculator.AddParameter("param_payment_date_to_i", DateTime.Now);
 */
            Calculator.AddParameter("param_ledger_number_i", ALedgerNumber);
            DefineReportColumns(Calculator);

            TFrmPetraReportingUtils.GenerateReport(Calculator, Owner, "APPaymentReport", true);
        }
    }
}