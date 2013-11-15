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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmIntranetExportSettingsDialog
    {
        private string FPswd;
        private string FExtra;
        private string FReplyToEmail;
        private int FGiftDays;

        /// <summary>
        ///
        /// </summary>
        public string Password
        {
            get
            {
                return FPswd;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string OptionalMetadata
        {
            get
            {
                return FExtra;
            }
        }

        /// <summary></summary>
        public int DonationDays
        {
            get
            {
                return FGiftDays;
            }
        }

        /// <summary></summary>
        public string ReplyToEmail
        {
            get
            {
                return FReplyToEmail;
            }
        }


        private void InitializeManualCode()
        {
            string MySettings = TSystemDefaults.GetSystemDefault("IntranetExportSettings", "pswd,45,");

            string[] Setting = MySettings.Split(',');

            if (Setting.Length > 2)
            {
                FPswd = Setting[0];
                FGiftDays = Convert.ToInt32(Setting[1]);
                FExtra = Setting[2];
            }

            FReplyToEmail = TUserDefaults.GetStringDefault("ReplyToEmail");
        }

        private void RunOnceOnActivationManual()
        {
            txtDonationDays.NumberValueInt = FGiftDays;
            txtOptionalMetadata.Text = FExtra;
            txtReplyToEmail.Text = FReplyToEmail;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (txtDonationDays.NumberValueInt.HasValue)
            {
                FGiftDays = txtDonationDays.NumberValueInt.Value;
            }

            FExtra = txtOptionalMetadata.Text;

            Boolean OkToClose = true;

            if (txtReplyToEmail.Text != "")
            {
                FReplyToEmail = txtReplyToEmail.Text;
                TUserDefaults.SetDefault("ReplyToEmail", FReplyToEmail);
            }

            if (txtOldPassword.Text != "")
            {
                string PswdError = "";

                if (txtOldPassword.Text == FPswd)
                {
                    if (txtNewPassword.Text != "")
                    {
                        if (txtNewPassword.Text == txtConfirmPassword.Text)
                        {
                            FPswd = txtNewPassword.Text;
                        }
                        else
                        {
                            PswdError = Catalog.GetString("Password error - new passwords don't match.");
                        }
                    }
                    else
                    {
                        PswdError = Catalog.GetString("Password error - password cannot be empty.");
                    }
                }
                else
                {
                    PswdError = Catalog.GetString("Password error - old password incorrect.");
                }

                if (PswdError == "")
                {
                    MessageBox.Show(Catalog.GetString("New password accepted."), Catalog.GetString("Password Change"));
                }
                else
                {
                    MessageBox.Show(PswdError, Catalog.GetString("Password Change"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    OkToClose = false;
                }
            }

            if (OkToClose)
            {
                TSystemDefaults.SetSystemDefault("IntranetExportSettings", String.Format("{0},{1},{2}", FPswd, FGiftDays, FExtra));
                Close();
            }
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            Close();
        }
    }
}