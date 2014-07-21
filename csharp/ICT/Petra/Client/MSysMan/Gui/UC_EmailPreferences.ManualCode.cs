// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2014 by OM International
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
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Remoting.Client;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Common.IO;

namespace Ict.Petra.Client.MSysMan.Gui
{
    public partial class TUC_EmailPreferences
    {
        /// <summary>
        /// If the Email preferences are not already in UserDefaults, this loads them.
        /// </summary>
        public static void LoadEmailDefaults()
        {
            if (!TUserDefaults.HasDefault("SmtpHost"))
            {
                TUserDefaults.SetDefault("SmtpHost", TAppSettingsManager.GetValue("SmtpHost", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpPort"))
            {
                TUserDefaults.SetDefault("SmtpPort", TAppSettingsManager.GetInt16("SmtpPort", -1));
            }

            if (!TUserDefaults.HasDefault("SmtpUseSsl"))
            {
                TUserDefaults.SetDefault("SmtpUseSsl", TAppSettingsManager.GetValue("SmtpEnableSsl", false));
            }

            if (!TUserDefaults.HasDefault("SmtpUser"))
            {
                TUserDefaults.SetDefault("SmtpUser", TAppSettingsManager.GetValue("SmtpUser", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpPassword"))
            {
                TUserDefaults.SetDefault("SmtpPassword", TAppSettingsManager.GetValue("SmtpPassword", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpEnableSsl"))
            {
                TUserDefaults.SetDefault("SmtpEnableSsl", TAppSettingsManager.GetBoolean("SmtpEnableSsl", false));
            }

            if (!TUserDefaults.HasDefault("SmtpFromAccount"))
            {
                TUserDefaults.SetDefault("SmtpFromAccount", TAppSettingsManager.GetValue("SmtpFromAccount", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpDisplayName"))
            {
                TUserDefaults.SetDefault("SmtpDisplayName", TAppSettingsManager.GetValue("SmtpDisplayName", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpReplyTo"))
            {
                TUserDefaults.SetDefault("SmtpReplyTo", TAppSettingsManager.GetValue("SmtpReplyTo", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpCcTo"))
            {
                TUserDefaults.SetDefault("SmtpCcTo", TAppSettingsManager.GetValue("SmtpCcTo", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpEmailBody"))
            {
                TUserDefaults.SetDefault("SmtpEmailBody", TAppSettingsManager.GetValue("SmtpEmailBody", ""));
            }

            if (!TUserDefaults.HasDefault("SmtpSendAsAttachment"))
            {
                TUserDefaults.SetDefault("SmtpSendAsAttachment", TAppSettingsManager.GetValue("SmtpSendAsAttachment", ""));
            }
        }

        /// <summary></summary>
        public void InitializeManualCode()
        {
            txtEmailBody.AcceptsReturn = true;
            txtAccountPswd.UseSystemPasswordChar = true;

            LoadEmailDefaults();
            txtServerName.Text = TUserDefaults.GetStringDefault("SmtpHost");
            txtPort.Text = TUserDefaults.GetInt16Default("SmtpPort").ToString();
            chkUseSsl.Checked = TUserDefaults.GetBooleanDefault("SmtpUseSsl");
            txtAccountName.Text = TUserDefaults.GetStringDefault("SmtpUser");
            txtAccountPswd.Text = TUserDefaults.GetStringDefault("SmtpPassword");
            txtSenderAddress.Text = TUserDefaults.GetStringDefault("SmtpFromAccount");
            txtDisplayName.Text = TUserDefaults.GetStringDefault("SmtpDisplayName");
            txtReplyTo.Text = TUserDefaults.GetStringDefault("SmtpReplyTo");
            txtCopyMessagesTo.Text = TUserDefaults.GetStringDefault("SmtpCcTo");
            txtEmailBody.Text = TUserDefaults.GetStringDefault("SmtpEmailBody");
            chkReportsAsAttachment.Checked = TUserDefaults.GetBooleanDefault("SmtpSendAsAttachment");
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
            TUserDefaults.SetDefault("SmtpHost", txtServerName.Text);
            TUserDefaults.SetDefault("SmtpPort", Convert.ToInt16(txtPort.Text));
            TUserDefaults.SetDefault("SmtpUseSsl", chkUseSsl.Checked);
            TUserDefaults.SetDefault("SmtpUser", txtAccountName.Text);
            TUserDefaults.SetDefault("SmtpPassword", txtAccountPswd.Text);
            TUserDefaults.SetDefault("SmtpFromAccount", txtSenderAddress.Text);
            TUserDefaults.SetDefault("SmtpDisplayName", txtDisplayName.Text);
            TUserDefaults.SetDefault("SmtpReplyTo", txtReplyTo.Text);
            TUserDefaults.SetDefault("SmtpCcTo", txtCopyMessagesTo.Text);
            TUserDefaults.SetDefault("SmtpEmailBody", txtEmailBody.Text);
            TUserDefaults.SetDefault("SmtpSendAsAttachment", chkReportsAsAttachment.Checked);
        }
    }
}