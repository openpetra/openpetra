//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Ext.Net;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using PetraWebService;
using Ict.Petra.Shared;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;

namespace Ict.Petra.WebServer.MSysMan
{
    public partial class TChangePassword : System.Web.UI.Page
    {
        protected Ext.Net.TextField OldPassword;
        protected Ext.Net.TextField NewPassword1;
        protected Ext.Net.TextField NewPassword2;
        protected Ext.Net.FormPanel ChangePasswordForm;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check for valid user
            TOpenPetraOrg myServer = new TOpenPetraOrg();

            if (!myServer.IsUserLoggedIn())
            {
                this.Response.Redirect("Default.aspx");
                return;
            }
        }

        protected void ChangePassword(Object sender, DirectEventArgs e)
        {
            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);
            string oldPassword = values["OldPassword"].ToString().Trim();
            string newPassword = values["NewPassword1"].ToString().Trim();
            string newPassword2 = values["NewPassword2"].ToString().Trim();

            if (newPassword != newPassword2)
            {
                X.Msg.Alert("Error", "Your password has NOT been changed. <br/>You have entered two different new passwords.").Show();
                return;
            }

            TVerificationResultCollection verification;

            if (!TMaintenanceWebConnector.CheckPasswordQuality(newPassword, out verification))
            {
                X.Msg.Alert(
                    "Error",
                    "Your Password has NOT been changed. <br/>Your password is not strong enough. <br/><br/>" +
                    verification[0].ResultText)
                .Show();
                return;
            }

            TVerificationResultCollection VerificationResult;

            if (TMaintenanceWebConnector.SetUserPassword(UserInfo.GUserInfo.UserID, newPassword, oldPassword, out VerificationResult) == true)
            {
                X.Msg.Alert("Success", "Your Password has been changed!", new JFunction { Fn = "HidePasswordWindow" }).Show();
            }
            else
            {
                X.Msg.Alert("Error", "Your password has NOT been changed. <br/>You have probably entered the wrong old password").Show();
            }
        }
    }
}