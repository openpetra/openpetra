//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters, christiank
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MSysMan.Validation;

namespace Ict.Petra.Client.CommonDialogs
{
    /// manual methods for the generated window
    public partial class TFrmChangePassword
    {
        private void InitializeManualCode()
        {
            UserName = string.Empty;
            PasswordNeedsChanged = true;
            txtOldPassword.PasswordChar = '*';
            txtNewPassword.PasswordChar = '*';
            txtNewPassword2.PasswordChar = '*';
        }

        /// <summary>
        /// the username of the user that this password is for
        /// </summary>
        public string UserName {
            get; set;
        }
        /// <summary>
        /// the old password. directly after login, we still have the password, the user does not need to enter it again
        /// </summary>
        public string OldPassword {
            set
            {
                if (value.Length > 0)
                {
                    txtOldPassword.Visible = false;
                    lblOldPassword.Visible = false;
                    txtOldPassword.Text = value;
                }
            }
        }

        /// <summary>
        /// do we want to enforce the change of the password
        /// </summary>
        public bool PasswordNeedsChanged {
            get; set;
        }

        private void RunOnceOnActivationManual()
        {
            if (PasswordNeedsChanged)
            {
                this.Text = Catalog.GetString("Password Change Required");
            }
            else
            {
                lblChangePassword.Visible = false;
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            TVerificationResultCollection VerificationResultCollection;

            // Client-side checks
            if (txtOldPassword.Text.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("You must enter the current password."),
                    CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtOldPassword.Focus();

                return;
            }

            if (txtNewPassword.Text.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("You must enter a new password."),
                    CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtNewPassword.Focus();

                return;
            }

            if (txtNewPassword2.Text.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("You must repeat the new password."),
                    CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtNewPassword2.Focus();

                return;
            }

            if (txtNewPassword.Text != txtNewPassword2.Text)
            {
                MessageBox.Show(Catalog.GetString("The new password and the repeated new password are not identical! Please enter them again."),
                    CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtNewPassword.Focus();
                txtNewPassword.SelectAll();

                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();  // give Windows a chance to update the Cursor

                // Request the setting of the new password (incl. server-side checks)
                if (!TRemote.MSysMan.Maintenance.WebConnectors.SetUserPassword(UserName, txtNewPassword.Text,
                        txtOldPassword.Text, PasswordNeedsChanged, TClientInfo.ClientComputerName, TClientInfo.ClientIPAddress,
                        out VerificationResultCollection))
                {
                    MessageBox.Show(String.Format(CommonDialogsResourcestrings.StrChangePasswordError, UserName) +
                        Environment.NewLine + Environment.NewLine +
                        VerificationResultCollection.BuildVerificationResultString(),
                        CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtNewPassword.Focus();
                    txtNewPassword.SelectAll();

                    return;
                }

                MessageBox.Show(String.Format(Catalog.GetString(CommonDialogsResourcestrings.StrChangePasswordSuccess), UserName),
                    CommonDialogsResourcestrings.StrChangePasswordTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}