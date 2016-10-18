// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2016 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using System.Collections.Generic;
using Ict.Petra.Client.MReporting.Gui.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLPeriodEndReporting
    {
        /// <summary>
        /// All the Batches produced in the periodEnd operation
        /// </summary>
        public List <Int32>glBatchNumbers = new List <int>();
        /// <summary>
        /// Everyone needs this...
        /// </summary>
        public Int32 FLedgerNumber = 0;
        /// <summary>
        /// True to select month; false for year.
        /// </summary>
        public Boolean FMonthMode = true;

        private void RunOnceOnActivationManual()
        {
            chkGlReports.Checked = true;
            chkIeStatement.Checked = true;
            chkBalanceSheet.Checked = true;
            chkAccountDetail.Checked = true;
            chkAfo.Checked = true;
            chkSurplusDeficit.Checked = true;
            chkExec.Checked = true;
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (chkGlReports.Checked)
            {
                TFrmBatchPostingRegister batchPostingGui = new TFrmBatchPostingRegister(null);

                foreach (Int32 glBatchNumber in glBatchNumbers)
                {
                    if (glBatchNumber > 0)
                    {
                        batchPostingGui.PrintReportNoUi(FLedgerNumber, glBatchNumber);
                    }
                }
            }

            if (chkIeStatement.Checked)
            {
                TFrmIncomeExpenseStmt incomeExpenseGui = new TFrmIncomeExpenseStmt(null);
                incomeExpenseGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            if (chkBalanceSheet.Checked)
            {
                TFrmBalanceSheetStandard balanceSheetGui = new TFrmBalanceSheetStandard(null);
                balanceSheetGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            if (chkAccountDetail.Checked)
            {
                TFrmAccountDetail accountDetailGui = new TFrmAccountDetail(null);
                accountDetailGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            if (chkAfo.Checked)
            {
                TFrmAFO AfoGui = new TFrmAFO(null);
                AfoGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            if (chkSurplusDeficit.Checked)
            {
                TFrmSurplusDeficit SDGui = new TFrmSurplusDeficit(null);
                SDGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            if (chkExec.Checked)
            {
                TFrmExecutiveSummary ExecGui = new TFrmExecutiveSummary(null);
                ExecGui.PrintPeriodEndReport(FLedgerNumber, FMonthMode);
            }

            this.Close();
        }
    }
}