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
        private Boolean FGiftReceiptingActivated;
        private Boolean FAccountsPayableActivated;
        private Boolean FCanGiftReceiptingBeDeactivated;
        private Boolean FCanAccountsPayableBeDeactivated;

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
                FGiftReceiptingActivated = TRemote.MFinance.Setup.WebConnectors.IsGiftReceiptingSubsystemActivated(FLedgerNumber);
                FAccountsPayableActivated = TRemote.MFinance.Setup.WebConnectors.IsAccountsPayableSubsystemActivated(FLedgerNumber);

                if (FGiftReceiptingActivated)
                {
                    FCanGiftReceiptingBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanGiftReceiptingSubsystemBeDeactivated(FLedgerNumber);
                }
                
                if (FAccountsPayableActivated)
                {
                    FCanAccountsPayableBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanAccountsPayableSubsystemBeDeactivated(FLedgerNumber);
                }

                UpdateControls();
            }
        }
        
        private void InitializeManualCode()
        {
            txtGiftReceiptingStatus.ReadOnly = true;
            txtAccountsPayableStatus.ReadOnly = true;
            txtStartingReceiptNumber.NumberValueInt = 1;
        }

        private void UpdateControls()
        {
            btnActivateGiftReceipting.Enabled = !FGiftReceiptingActivated;
            
            lblStartingReceiptNumber.Visible = !FGiftReceiptingActivated;
            txtStartingReceiptNumber.Visible = !FGiftReceiptingActivated;

            if (FGiftReceiptingActivated)
            {
                txtGiftReceiptingStatus.Text = Catalog.GetString("Activated");
                btnActivateGiftReceipting.Text = Catalog.GetString("Deactivate Gift Receipting");
                btnActivateGiftReceipting.Enabled = FCanGiftReceiptingBeDeactivated;
            }
            else
            {
                txtGiftReceiptingStatus.Text = Catalog.GetString("Not activated yet");
                btnActivateGiftReceipting.Text = Catalog.GetString("Activate Gift Receipting");
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
        }
        
        private void BtnActivateGiftReceipting_Click(System.Object sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            
            if (FGiftReceiptingActivated)
            {
                // deactivate gift receipting
                if (MessageBox.Show(Catalog.GetString("Do you want to deactivate Gift Receipting Subsystem?"),
                                    Catalog.GetString("Deactivate Gift Receipting"), 
                                    MessageBoxButtons.YesNo, 
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FGiftReceiptingActivated = !TRemote.MFinance.Setup.WebConnectors.DeactivateGiftReceiptingSubsystem(FLedgerNumber);
                    
                    UpdateControls();
                }
            }
            else
            {
                // activate gift receipting
                if (MessageBox.Show(String.Format(Catalog.GetString("Do you want to activate Gift Receipting Subsystem " +
                                                                    "with Receipting Start Number {0}?"), txtStartingReceiptNumber.NumberValueInt),
                                    Catalog.GetString("Activate Gift Receipting"), 
                                    MessageBoxButtons.YesNo, 
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (   txtStartingReceiptNumber.NumberValueInt == null
                        || txtStartingReceiptNumber.NumberValueInt <= 0)
                    {
                        MessageBox.Show(Catalog.GetString("Starting Receipt Number must be 1 or higher!"),
                                        Catalog.GetString("Activate Gift Receipting"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    
                    Result = TRemote.MFinance.Setup.WebConnectors.
                        ActivateGiftReceiptingSubsystem(FLedgerNumber, Convert.ToInt32(txtStartingReceiptNumber.NumberValueInt) - 1, out VerificationResult);
                    FGiftReceiptingActivated = (Result == TSubmitChangesResult.scrOK);

                    if (FGiftReceiptingActivated)
                    {
                        FCanGiftReceiptingBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanGiftReceiptingSubsystemBeDeactivated(FLedgerNumber);
                    }
                    
                    UpdateControls();
                }
            }
        }

        private void BtnActivateAccountsPayable_Click(System.Object sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;

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
                    Result = TRemote.MFinance.Setup.WebConnectors.
                                      ActivateAccountsPayableSubsystem(FLedgerNumber, out VerificationResult);
                    FAccountsPayableActivated = (Result == TSubmitChangesResult.scrOK);
 
                    if (FAccountsPayableActivated)
                    {
                        FCanAccountsPayableBeDeactivated = TRemote.MFinance.Setup.WebConnectors.CanAccountsPayableSubsystemBeDeactivated(FLedgerNumber);
                    }
                    
                    UpdateControls();
                }
            }
        }
    }
}