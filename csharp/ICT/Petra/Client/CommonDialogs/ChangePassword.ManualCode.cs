//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
        public string UserName { get; set; }
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
        public bool PasswordNeedsChanged { get; set; }

        private void RunOnceOnActivationManual()
        {
            lblChangePassword.Visible = PasswordNeedsChanged;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtNewPassword2.Text)
            {
                MessageBox.Show(Catalog.GetString("Passwords do not match! Please try again..."));
                return;
            }

            if (txtNewPassword.Text == this.txtOldPassword.Text && this.PasswordNeedsChanged)
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                    "Password not changed as the old password was reused. Please use a new password."), Catalog.GetString("Error")));
                return;
            }

            TVerificationResultCollection VerificationResultCollection;
            TVerificationResult VerificationResult;

            if (!TSharedSysManValidation.CheckPasswordQuality(txtNewPassword.Text, out VerificationResult))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "There was a problem setting the password for user {0}."), UserName) +
                    Environment.NewLine + VerificationResult.ResultText);
                return;
            }

            if (!TRemote.MSysMan.Maintenance.WebConnectors.SetUserPassword(UserName, txtNewPassword.Text,
                    txtOldPassword.Text,
                    PasswordNeedsChanged,
                    out VerificationResultCollection))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "There was a problem setting the password for user {0}."), UserName) +
                    Environment.NewLine + VerificationResultCollection.BuildVerificationResultString());
                return;
            }

            MessageBox.Show(String.Format(Catalog.GetString("Password was successfully set for user {0}"), UserName),
                            Catalog.GetString("Success"));
            this.DialogResult = DialogResult.OK;
        }
    }
}