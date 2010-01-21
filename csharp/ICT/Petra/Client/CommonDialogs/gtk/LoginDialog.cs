/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       markusm, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using Mono.Unix;
using System.Security.Principal;
using Ict.Petra.Client.App.Core;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.GTK;
using Ict.Petra.Shared;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.MSysMan;
using Gtk;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// Login dialog
    /// </summary>
    public class TLoginForm : Gtk.Dialog
    {
        Gtk.Entry txtPassword;
        Gtk.Entry txtUsername;
        int FProcessID;
        string FWelcomeMessage;
        bool FSystemEnabled;
        bool FConnectionEstablished = false;

        public TLoginForm()
        {
            this.Title = Catalog.GetString("Login to Petra");
            SetSizeRequest(350, 200);
            SetIconFromFile("img/petraico-small.ico");

            Gtk.Table table = new Gtk.Table(2, 3, false);

            txtUsername = new Gtk.Entry();
            Gtk.Label lblUsername = new Gtk.Label();
            lblUsername.Text = Catalog.GetString("User name") + ":";
            txtPassword = new Gtk.Entry();
            txtPassword.Visibility = false;
            Gtk.Label lblPassword = new Gtk.Label();
            lblPassword.Text = Catalog.GetString("Password") + ":";

            table.ColumnSpacing = 12;
            table.RowSpacing = 12;

            AttachOptions option = AttachOptions.Fill;
            lblUsername.SetAlignment(0, 0);
            table.Attach(lblUsername, 0, 1, 0, 1, option, option, 0, 0);
            table.Attach(txtUsername, 1, 2, 0, 1, option, option, 0, 0);
            lblPassword.SetAlignment(0, 0);
            table.Attach(lblPassword, 0, 1, 1, 2, option, option, 0, 0);
            table.Attach(txtPassword, 1, 2, 1, 2, option, option, 0, 0);

            Gtk.Button btnOk = (Button) this.AddButton(Catalog.GetString("Login"), ResponseType.Ok);
            btnOk.Clicked += new EventHandler(LoginClick);
            Gtk.Button btnCancel = (Button) this.AddButton(Catalog.GetString("Cancel"), ResponseType.Cancel);
            btnCancel.Clicked += new EventHandler(CancelClick);

            Gtk.HBox boxLogin = new Gtk.HBox();

            Gtk.Image imgLogo = new Image("img/petraico-big.ico");

            boxLogin.PackStart(imgLogo, true, false, 0);
            boxLogin.PackStart(table, false, false, 0);

            this.VBox.PackStart(boxLogin, true, false, 0);
            this.Modal = true;

            // default button login? Enter in txtPassword will click login button
            txtPassword.KeyPressEvent += new KeyPressEventHandler(KeyPressed);

            // Default Response did not really work
            //this.DefaultResponse = ResponseType.Ok;
            this.ShowAll();
        }

        [GLib.ConnectBefore()]
        private void KeyPressed(object obj, KeyPressEventArgs args)
        {
            if ((args.Event.Key == Gdk.Key.Return)
                && (txtPassword.Text.Length > 0))
            {
                LoginClick(obj, args);
            }
        }

        private void CancelClick(object obj, EventArgs args)
        {
            this.Respond(Gtk.ResponseType.Cancel);
        }

        private void LoginClick(object obj, EventArgs args)
        {
            String ConnectionError;
            string SelUserName = txtUsername.Text.ToUpper();
            string SelPassWord = txtPassword.Text;

            // TODO: the cursor does not seem to work yet; or is a new thread required?
            this.GdkWindow.Cursor = new Gdk.Cursor(Gdk.CursorType.Watch);

            if (ConnectToPetraServer(SelUserName, SelPassWord, out ConnectionError))
            {
                // Show any message that is returned after successful login (eg. 'Petra is currently disabled due to xxx. Proceed with caution.')
                if (UserInfo.GUserInfo.LoginMessage != null)
                {
                    MessageBox.Show(UserInfo.GUserInfo.LoginMessage,
                        Catalog.GetString("Petra Login"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                TUserDefaults.GUserDefaults = new TUserDefaults();
                TLogging.UserNamePrefix = UserInfo.GUserInfo.UserID + '_' +
                                          ConnectionManagement.GConnectionManagement.ClientID.ToString();
                FConnectionEstablished = true;
                this.Respond(Gtk.ResponseType.Accept);
            }
            else
            {
                this.GdkWindow.Cursor = new Gdk.Cursor(Gdk.CursorType.Arrow);
                MessageBox.Show(ConnectionError);

                // todo: don't clos login dialog
            }
        }

        private bool ConnectToPetraServer(String AUserName, String APassWord, out String AError)
        {
            bool ReturnValue = false;

            AError = "";

            try
            {
                ReturnValue = ConnectionManagement.GConnectionManagement.ConnectToServer(AUserName, APassWord,
                    out FProcessID,
                    out FWelcomeMessage,
                    out FSystemEnabled,
                    out AError);
            }
            catch (EClientVersionMismatchException exp)
            {
                MessageBox.Show(exp.Message, "Petra Client/Server Program Version Mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            catch (ELoginFailedServerTooBusyException)
            {
#if TESTMODE
                TLogging.Log("The PetraServer is too busy to accept the Login request.");
#endif
#if  TESTMODE
#else
                MessageBox.Show("The PetraServer is too busy to accept the Login request." + "\r\n" + "\r\n" + "Please try again after a short time!",
                    "Server busy",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
#endif
                return false;
            }
            catch (EDBConnectionNotEstablishedException exp)
            {
                if (exp.Message.IndexOf("Exceeding permissible number of connections") != -1)
                {
                    TLogging.Log("Login failed because too many users are logged in.");
#if  TESTMODE
#else
                    MessageBox.Show(
                        "Too many users are logged in.",
                        "Too many users",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
#endif
                }
                else
                {
                    TLogging.Log(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                        TLoggingType.ToLogfile);
#if  TESTMODE
#else
                    MessageBox.Show("An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                        Catalog.GetString("For details see the log file.") + TLogging.GetLogFileName(),
                        "Server connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
#endif
                }

                return false;
            }
            catch (EServerConnectionServerNotReachableException)
            {
#if TESTMODE
                TLogging.Log("The PetraServer cannot be reached!");
#endif
#if  TESTMODE
#else
                MessageBox.Show("The PetraServer cannot be reached!", "No Server response", MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
                return false;
            }
            catch (EServerConnectionGeneralException exp)
            {
                TLogging.Log(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(), TLoggingType.ToLogfile);
#if  TESTMODE
#else
                MessageBox.Show("An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                    Catalog.GetString("For details see the log file.") + TLogging.GetLogFileName(),
                    "Server connection error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
#endif
                return false;
            }
            catch (Exception exp)
            {
                TLogging.Log(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(), TLoggingType.ToLogfile);
#if  TESTMODE
#else
                MessageBox.Show("An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                    Catalog.GetString("For details see the log file.") + TLogging.GetLogFileName(),
                    "Server connection error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
#endif
                return false;
            }
            return ReturnValue;
        }

        public Boolean GetReturnedParameters(out Int32 AProcessID, out String AWelcomeMessage, out Boolean ASystemEnabled)
        {
            AProcessID = -1;
            AWelcomeMessage = "";
            ASystemEnabled = false;

            if (FConnectionEstablished)
            {
                AProcessID = FProcessID;
                AWelcomeMessage = FWelcomeMessage;
                ASystemEnabled = FSystemEnabled;
            }

            return FConnectionEstablished;
        }
    }
}