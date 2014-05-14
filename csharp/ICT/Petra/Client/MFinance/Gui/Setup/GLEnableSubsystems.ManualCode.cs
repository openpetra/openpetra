//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;
using System.Reflection;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLEnableSubsystems
    {
        private Int32 FLedgerNumber;
        private Boolean FGiftProcessingActivated;
        private Boolean FAccountsPayableActivated;
        private Boolean FCanGiftProcessingBeDeactivated;
        private Boolean FCanAccountsPayableBeDeactivated;

        /// <summary>Delegate to update subsystem link status which needs to be updated by caller.</summary>
        public delegate void UpdateFinanceSubsystemLinkStatus();

        /// <summary>Store Delegate to update subsystem link status</summary>
        private static UpdateFinanceSubsystemLinkStatus FFinanceSubSystemLinkStatus;

        /// <summary>
        /// The applicable Ledger number
        /// </summary>
        public Int32 LedgerNumber
        {
            get
            {
                return FLedgerNumber;
            }

            set
            {
                FLedgerNumber = value;
                FGiftProcessingActivated = TRemote.MFinance.Setup.WebConnectors.IsGiftProcessingSubsystemActivated(FLedgerNumber);
                FAccountsPayableActivated = TRemote.MFinance.Setup.WebConnectors.IsAccountsPayableSubsystemActivated(FLedgerNumber);

                if (FGiftProcessingActivated)
                {
                    FCanGiftProcessingBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanGiftProcessingSubsystemBeDeactivated(FLedgerNumber);
                }

                if (FAccountsPayableActivated)
                {
                    FCanAccountsPayableBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanAccountsPayableSubsystemBeDeactivated(FLedgerNumber);
                }

                UpdateControls();
            }
        }

        /// <summary>
        /// Delegate for determinig a help topic for a given Form and Control.
        /// </summary>
        public static UpdateFinanceSubsystemLinkStatus FinanceSubSystemLinkStatus
        {
            get
            {
                return FFinanceSubSystemLinkStatus;
            }
            set
            {
                FFinanceSubSystemLinkStatus = value;
            }
        }

        private void InitializeManualCode()
        {
            txtGiftProcessingStatus.ReadOnly = true;
            txtAccountsPayableStatus.ReadOnly = true;
            txtStartingReceiptNumber.NumberValueInt = 1;
        }

        private void UpdateControls()
        {
            btnActivateGiftProcessing.Enabled = !FGiftProcessingActivated;

            lblStartingReceiptNumber.Visible = !FGiftProcessingActivated;
            txtStartingReceiptNumber.Visible = !FGiftProcessingActivated;

            if (FGiftProcessingActivated)
            {
                txtGiftProcessingStatus.Text = Catalog.GetString("Activated");
                btnActivateGiftProcessing.Text = Catalog.GetString("Deactivate Gift Processing");
                btnActivateGiftProcessing.Enabled = FCanGiftProcessingBeDeactivated;
            }
            else
            {
                txtGiftProcessingStatus.Text = Catalog.GetString("Not activated yet");
                btnActivateGiftProcessing.Text = Catalog.GetString("Activate Gift Processing");
            }

            btnActivateAccountsPayable.Enabled = !FAccountsPayableActivated;

            if (FAccountsPayableActivated)
            {
                txtAccountsPayableStatus.Text = Catalog.GetString("Activated");
                btnActivateAccountsPayable.Text = Catalog.GetString("Deactivate Accounts Payable");
                btnActivateAccountsPayable.Enabled = FCanAccountsPayableBeDeactivated;
            }
            else
            {
                txtAccountsPayableStatus.Text = Catalog.GetString("Not activated yet");
                btnActivateAccountsPayable.Text = Catalog.GetString("Activate Accounts Payable");
            }

            // now update menu links for gift processing and accounts payable in caller window (may need to be enabled/disabled)
            if (FFinanceSubSystemLinkStatus != null)
            {
                FFinanceSubSystemLinkStatus();
            }
        }

        private void BtnActivateGiftProcessing_Click(System.Object sender, EventArgs e)
        {
            if (FGiftProcessingActivated)
            {
                // deactivate gift processing
                if (MessageBox.Show(Catalog.GetString("Do you want to deactivate Gift Processing Subsystem?"),
                        Catalog.GetString("Deactivate Gift Processing"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FGiftProcessingActivated = !TRemote.MFinance.Setup.WebConnectors.DeactivateGiftProcessingSubsystem(FLedgerNumber);

                    UpdateControls();
                }
            }
            else
            {
                // activate gift processing
                if (MessageBox.Show(String.Format(Catalog.GetString("Do you want to activate Gift Processing Subsystem " +
                                "with Receipting Start Number {0}?"), txtStartingReceiptNumber.NumberValueInt),
                        Catalog.GetString("Activate Gift Processing"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if ((txtStartingReceiptNumber.NumberValueInt == null)
                        || (txtStartingReceiptNumber.NumberValueInt <= 0))
                    {
                        MessageBox.Show(Catalog.GetString("Starting Receipt Number must be 1 or higher!"),
                            Catalog.GetString("Activate Gift Processing"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    TRemote.MFinance.Setup.WebConnectors.
                    ActivateGiftProcessingSubsystem(FLedgerNumber, Convert.ToInt32(
                            txtStartingReceiptNumber.NumberValueInt) - 1);
                    FGiftProcessingActivated = true;

                    if (FGiftProcessingActivated)
                    {
                        FCanGiftProcessingBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanGiftProcessingSubsystemBeDeactivated(FLedgerNumber);
                    }

                    UpdateControls();
                }
            }
        }

        private void BtnActivateAccountsPayable_Click(System.Object sender, EventArgs e)
        {
            if (FAccountsPayableActivated)
            {
                // deactivate accounts payable
                if (MessageBox.Show(Catalog.GetString("Do you want to deactivate Accounts Payable Subsystem?"),
                        Catalog.GetString("Deactivate Accounts Payable"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FAccountsPayableActivated = !TRemote.MFinance.Setup.WebConnectors.DeactivateAccountsPayableSubsystem(FLedgerNumber);

                    UpdateControls();
                }
            }
            else
            {
                // activate accounts payable
                if (MessageBox.Show(Catalog.GetString("Do you want to activate Accounts Payable Subsystem?"),
                        Catalog.GetString("Activate Accounts Payable"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TRemote.MFinance.Setup.WebConnectors.
                    ActivateAccountsPayableSubsystem(FLedgerNumber);
                    FAccountsPayableActivated = true;

                    if (FAccountsPayableActivated)
                    {
                        FCanAccountsPayableBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanAccountsPayableSubsystemBeDeactivated(
                            FLedgerNumber);
                    }

                    UpdateControls();
                }
            }
        }
    }
}