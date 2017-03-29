//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
//       Tim Ingham
//
// Copyright 2004-2017 by OM International
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
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.MReporting.Logic;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Shared.MReporting;

using Ict.Petra.Client.MSysMan.Gui;
using Ict.Common.IO;
using System.Text;


namespace Ict.Petra.Client.MFinance.Gui.ICH
{
    /// manual methods for the generated window
    public partial class TFrmStewardshipReports : System.Windows.Forms.Form
    {
        Int32 FLedgerNumber = 0;
        ALedgerRow FLedgerRow = null;
        List <String>FStatusMsg = new List <String>();

        const string STEWARDSHIP_EMAIL_ADDRESS = "ICHEMAIL";

        /// <summary>
        /// Write-only Ledger number property
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                //
                // I've been getting some grief from cached tables, so I'll mark them as being "dirty" before I do anything else:

                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.ICHStewardshipList, FLedgerNumber);
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber);


                FLedgerRow =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                TFinanceControls.InitialiseAvailableFinancialYearsListHOSA(
                    ref cmbYearEnding,
                    FLedgerNumber);

                chkHOSA.CheckedChanged += RefreshReportingOptions;
                chkStewardship.CheckedChanged += RefreshReportingOptions;
                chkFees.CheckedChanged += RefreshReportingOptions;
                //              chkRecipient.CheckedChanged += RefreshReportingOptions;
                cmbReportPeriod.SelectedValueChanged += RefreshReportingOptions;
                rbtEmailHosa.CheckedChanged += RefreshReportingOptions;
                rbtReprintHosa.CheckedChanged += RefreshReportingOptions;
                RefreshReportingOptions(null, null);
                uco_SelectedFees.LedgerNumber = FLedgerNumber;

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private void RunOnceOnActivationManual()
        {
            //
            // I don't want to see the "Select template" option on this form.
            FPetraUtilsObject.EnableDisableFastReports(false);
        }

        //
        // called on Year change.
        private void RefreshReportPeriodList(object sender, EventArgs e)
        {
            if (cmbYearEnding.SelectedIndex > -1)
            {
                TFinanceControls.InitialiseAvailableFinancialPeriodsList(
                    ref cmbReportPeriod,
                    FLedgerNumber,
                    cmbYearEnding.GetSelectedInt32(),
                    FLedgerRow.CurrentPeriod - 1,
                    false,
                    false);
            }
        }

        //
        // Called on period change.
        private void RefreshICHStewardshipNumberList(object sender, EventArgs e)
        {
            if ((cmbReportPeriod.SelectedIndex > -1) && (cmbYearEnding.SelectedIndex > -1))
            {
                Int32 accountingYear = cmbYearEnding.GetSelectedInt32();
                TFinanceControls.InitialiseICHStewardshipList(
                    cmbICHNumber, FLedgerNumber,
                    cmbYearEnding.GetSelectedInt32(),
                    cmbReportPeriod.GetSelectedInt32());

                cmbICHNumber.SelectedIndex = 0;
            }
        }

        /// <summary>Called from generated code</summary>
        public void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        //
        // Called on any report checkbox changed
        private void RefreshReportingOptions(Object Sender, EventArgs e)
        {
            if (cmbYearEnding.SelectedIndex != 0)
            {
                chkHOSA.Enabled = chkHOSA.Checked = false;
                chkFees.Enabled = chkFees.Checked = false;
            }
            else
            {
                chkHOSA.Enabled = true;
                chkFees.Enabled = true;
            }

            rbtEmailHosa.Enabled =
                rbtReprintHosa.Enabled = chkHOSA.Enabled && chkHOSA.Checked;

            if (rbtEmailHosa.Enabled && !rbtEmailHosa.Checked && !rbtReprintHosa.Checked)
            {
                rbtEmailHosa.Checked = true;
            }

            chkRecipient.Enabled =
                rbtReprintHosa.Enabled & rbtReprintHosa.Checked;

            rbtEmailStewardship.Enabled =
                rbtReprintStewardship.Enabled = chkStewardship.Enabled && chkStewardship.Checked;

            if (rbtEmailStewardship.Enabled && !rbtEmailStewardship.Checked && !rbtReprintStewardship.Checked)
            {
                rbtEmailStewardship.Checked = true;
            }

            rbtFull.Enabled =
                rbtSummary.Enabled = chkFees.Enabled && chkFees.Checked;
        }
    }
}
