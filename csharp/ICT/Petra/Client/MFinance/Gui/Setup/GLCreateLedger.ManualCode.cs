//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
    public partial class TFrmGLCreateLedger
    {
        private void InitializeManualCode()
        {
            // TODO: should we use an assistant window?
            // TODO: retention period
            // TODO: setup of gift system, AP, etc

            nudLedgerNumber.Maximum = 9999;
            nudLedgerNumber.Minimum = 1;
            nudLedgerNumber.Value = 99;
            dtpCalendarStartDate.Date = new DateTime(DateTime.Now.Year, 1, 1);

            // at the moment we only allow a maximum of 8 forward periods
            nudNumberOfFwdPostingPeriods.Maximum = 8;
            nudNumberOfFwdPostingPeriods.Minimum = 1;
            nudNumberOfFwdPostingPeriods.Value = 8;

            // only allow 12 or 13 periods for now (possibly 14 at a later time if needed)
            nudNumberOfPeriods.Maximum = 13;
            nudNumberOfPeriods.Minimum = 12;
            nudNumberOfPeriods.Value = 12;

            nudCurrentPeriod.Value = 1;
            cmbBaseCurrency.SetSelectedString("EUR");
            cmbIntlCurrency.SetSelectedString("USD");
            cmbCountryCode.SetSelectedString("DE");

            ActivateGiftReceipting_Changed(null, null);

            nudLedgerNumber.KeyDown += numericUpDown_KeyDown;
            nudNumberOfFwdPostingPeriods.KeyDown += numericUpDown_KeyDown;
            nudNumberOfPeriods.KeyDown += numericUpDown_KeyDown;
            nudCurrentPeriod.KeyDown += numericUpDown_KeyDown;
        }

        private void ActivateGiftReceipting_Changed(System.Object sender, EventArgs e)
        {
            if (chkActivateGiftReceipting.Checked)
            {
                lblStartingReceiptNumber.Enabled = true;
                txtStartingReceiptNumber.Enabled = true;
                txtStartingReceiptNumber.NumberValueInt = 1;
            }
            else
            {
                lblStartingReceiptNumber.Enabled = false;
                txtStartingReceiptNumber.Enabled = false;
                txtStartingReceiptNumber.NumberValueInt = null;
            }
        }

        private void BtnOK_Click(System.Object sender, EventArgs e)
        {
            MethodInfo method;

            TVerificationResultCollection VerificationResult;

            if (txtLedgerName.Text.Length == 0)
            {
                MessageBox.Show(
                    Catalog.GetString("Please enter a name for your ledger!"),
                    Catalog.GetString("Problem: No Ledger has been created"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!dtpCalendarStartDate.Date.HasValue)
            {
                MessageBox.Show(Catalog.GetString("Please supply valid Start date."),
                    Catalog.GetString("Problem: No Ledger has been created"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (nudCurrentPeriod.Value > nudNumberOfPeriods.Value)
            {
                MessageBox.Show(Catalog.GetString("Current Period cannot be greater than Number of Periods!"),
                    Catalog.GetString("Problem: No Ledger has been created"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (chkActivateGiftReceipting.Checked
                && ((txtStartingReceiptNumber.NumberValueInt == null)
                    || (txtStartingReceiptNumber.NumberValueInt <= 0)))
            {
                MessageBox.Show(Catalog.GetString("Starting Receipt Number must be 1 or higher!"),
                    Catalog.GetString("Problem: No Ledger has been created"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Int32 StartingReceiptNumber = 1;

            if (txtStartingReceiptNumber.NumberValueInt != null)
            {
                StartingReceiptNumber = Convert.ToInt32(txtStartingReceiptNumber.NumberValueInt) - 1;
            }

            /* hourglass cursor */
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            if (!TRemote.MFinance.Setup.WebConnectors.CreateNewLedger(
                    Convert.ToInt32(nudLedgerNumber.Value),
                    txtLedgerName.Text,
                    cmbCountryCode.GetSelectedString(),
                    cmbBaseCurrency.GetSelectedString(),
                    cmbIntlCurrency.GetSelectedString(),
                    dtpCalendarStartDate.Date.Value,
                    Convert.ToInt32(nudNumberOfPeriods.Value),
                    Convert.ToInt32(nudCurrentPeriod.Value),
                    Convert.ToInt32(nudNumberOfFwdPostingPeriods.Value),
                    chkActivateGiftReceipting.Checked,
                    StartingReceiptNumber,
                    chkActivateAccountsPayable.Checked,
                    out VerificationResult))
            {
                /* normal mouse cursor */
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.BuildVerificationResultString(),
                        Catalog.GetString("Problem: No Ledger has been created"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Problem: No Ledger has been created"),
                        Catalog.GetString("Error"));
                }
            }
            else
            {
                /* normal mouse cursor */
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                MessageBox.Show(String.Format(Catalog.GetString(
                            "Ledger {0} ({1}) has been created successfully and is now the current Ledger.\r\n\r\nPermissions for users to be able to access this Ledger can be assigned in the System Manager Module."),
                        txtLedgerName.Text,
                        nudLedgerNumber.Value),
                    Catalog.GetString("Success"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // reload permissions for user
                UserInfo.GUserInfo = TRemote.MSysMan.Security.UserManager.WebConnectors.ReloadCachedUserInfo();

                // reload list of Ledger names
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);

                // reload navigation
                Form MainWindow = FPetraUtilsObject.GetCallerForm();

                PropertyInfo CurrentLedgerProperty = MainWindow.GetType().GetProperty("CurrentLedger");
                CurrentLedgerProperty.SetValue(MainWindow, Convert.ToInt32(nudLedgerNumber.Value), null);

                method = MainWindow.GetType().GetMethod("LoadNavigationUI");

                if (method != null)
                {
                    method.Invoke(MainWindow, new object[] { true });
                }

                method = MainWindow.GetType().GetMethod("ShowCurrentLedgerInfoInStatusBar");

                if (method != null)
                {
                    method.Invoke(MainWindow, new object[] { });
                }

                Close();
            }
        }

        /// <summary>
        /// Checks for limited number of digits and no negatives
        /// in a Numeric Up/Down box
        /// </summary>
        private void numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.KeyData == Keys.Back) || (e.KeyData == Keys.Delete)))
            {
                if (sender == nudLedgerNumber)
                {
                    if ((nudLedgerNumber.Text.Length >= 4) || (e.KeyValue == 109))
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                }
                else if (sender == nudNumberOfFwdPostingPeriods)
                {
                    if ((nudNumberOfFwdPostingPeriods.Text.Length >= 2) || (e.KeyValue == 109))
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                }
                else if (sender == nudNumberOfPeriods)
                {
                    if ((nudNumberOfPeriods.Text.Length >= 2) || (e.KeyValue == 109))
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                }
                else if (sender == nudCurrentPeriod)
                {
                    if ((nudCurrentPeriod.Text.Length >= 2) || (e.KeyValue == 109))
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                }
            }
        }
    }
}