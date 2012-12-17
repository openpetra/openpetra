//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
//
// Copyright 2004-2012 by OM International
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


namespace Ict.Petra.Client.MFinance.Gui.ICH
{
    /// <summary>
    /// Enums holding the possible reporting period selection modes
    /// </summary>
    public enum TICHReportingPeriodSelectionModeEnum
    {
        /// <summary>
        /// ICH Statement reporting period selection mode
        /// </summary>
        rpsmICHStatement,
        /// <summary>
        /// ICH Stewardship Calculation reporting period selection mode
        /// </summary>
        rpsmICHStewardshipCalc
    }


    /// manual methods for the generated window
    public partial class TFrmReportingPeriodSelectionDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// Field to store the reporting period selection mode
        /// </summary>
        public TICHReportingPeriodSelectionModeEnum FReportingPeriodSelectionMode = TICHReportingPeriodSelectionModeEnum.rpsmICHStewardshipCalc;
        /// <summary>
        /// Field to store the relevant Ledger number
        /// </summary>
        public Int32 FLedgerNumber = 0;

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            bool retVal = false;

            TVerificationResultCollection VerificationResult = null;

            try
            {
                switch (this.ReportingPeriodSelectionMode)
                {
                    case TICHReportingPeriodSelectionModeEnum.rpsmICHStewardshipCalc:

                        Cursor = Cursors.WaitCursor;

                        retVal = TRemote.MFinance.ICH.WebConnectors.PerformStewardshipCalculation(FLedgerNumber, cmbReportPeriod.GetSelectedInt32(),
                        out VerificationResult);

                        Cursor = Cursors.Default;

                        if (retVal)
                        {
                            MessageBox.Show("Stewardship Calculation Completed Successfully");
                        }
                        else
                        {
                            MessageBox.Show(
                                Messages.BuildMessageFromVerificationResult("Stewardship Calculation was UNSUCCESSFUL!",
                                    VerificationResult));
                        }

                        break;

                    case TICHReportingPeriodSelectionModeEnum.rpsmICHStatement:

                        throw new NotImplementedException("ICH Statement functionality is not yet implemented!");
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Gets or sets the ICH reporting period selection mode
        /// </summary>
        public TICHReportingPeriodSelectionModeEnum ReportingPeriodSelectionMode
        {
            get
            {
                return FReportingPeriodSelectionMode;
            }

            set
            {
                FReportingPeriodSelectionMode = value;

                if (FReportingPeriodSelectionMode == TICHReportingPeriodSelectionModeEnum.rpsmICHStatement)
                {
                    chkEmailReport.Visible = true;
                    lblEmailReport.Visible = true;
                }
                else
                {
                    chkEmailReport.Visible = false;
                    lblEmailReport.Visible = false;
                }
            }
        }

        /// <summary>
        /// Write-only Ledger number property
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                TFinanceControls.InitialiseOpenFinancialPeriodsList(
                    ref cmbReportPeriod,
                    FLedgerNumber);
            }
        }
    }
}