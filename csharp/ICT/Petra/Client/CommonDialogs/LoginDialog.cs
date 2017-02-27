//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Resources;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Win32;
using GNU.Gettext;
using System.Security.Principal;
using Ict.Petra.Client.App.Core;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// Login Form for the authentication of an OpenPetra user
    /// </summary>
    public partial class TLoginForm : System.Windows.Forms.Form, IFrmPetra
    {
        private TFrmPetraUtils FPetraUtilsObject;

        private static bool FPreviouslyShown = false;

        #region Resourcestrings

        private readonly String StrPetraLoginFormTitle = Catalog.GetString("OpenPetra Login");
        private readonly String StrDetailsInLogfile = Catalog.GetString("For details see the log file");
        private readonly String StrLoginFailed = Catalog.GetString("Login failed!");

        #endregion

        /// <summary>Private Declarations</summary>
        private String FSelUserName;
        private String FSelPassWord;
        private String FWelcomeMessage;
        private Boolean FSystemEnabled;
        private Int32 FProcessID;
        private Boolean FConnectionEstablished;

        private void DisplayLoginFailedMessage(string AMessage, string ATitle = null)
        {
            if (ATitle == null)
            {
                ATitle = StrLoginFailed;
            }

#if TESTMODE
            TLogging.Log(AMessage);
#endif
#if  TESTMODE
#else
            MessageBox.Show(AMessage, ATitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TLoginForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblUserName.Text = Catalog.GetString("&User ID") + ":";
            this.lblPassword.Text = Catalog.GetString("&Password") + ":";
            this.btnLogin.Text = Catalog.GetString(" &Login");
            this.btnCancel.Text = Catalog.GetString(" &Quit");
            this.chkRememberUserName.Text = Catalog.GetString("&Remember the User ID");
            this.label1.Text = Catalog.GetString("Initial Login: demo/demo or sysadmin/CHANGEME");
            this.lblPetraVersion.Text = Catalog.GetString("Version");
            this.Text = Catalog.GetString("OpenPetra Login");
            #endregion

            FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(null, this, null);

            string url = TAppSettingsManager.GetValue("OpenPetra.HTTPServer", string.Empty);

            if (url.Length > 0 && !url.Contains("://demo.") && !url.Contains("://localhost"))
            {
                label1.Visible = false;
            }

            this.Text = StrPetraLoginFormTitle;

            this.lblPetraVersion.Text = Catalog.GetString("Version") + " " + TApplicationVersion.GetApplicationVersion();

            //this.Height = 142;
            //pnlLoginControls.Top = 46;
        }

        private void TxtUserNameLeave(System.Object sender, System.EventArgs e)
        {
            this.txtUserName.Text = this.txtUserName.Text.ToUpper();
        }

        private void UpdateUI(Boolean APerformingLogin)
        {
            if (APerformingLogin)
            {
                this.Cursor = Cursors.WaitCursor;

//                prbLogin.Visible = true;
                btnCancel.Enabled = false;
                btnLogin.Enabled = false;
                txtPassword.Enabled = false;
                txtUserName.Enabled = false;
                chkRememberUserName.Enabled = false;

                btnLogin.Focus();

                Application.DoEvents();
            }
            else
            {
                this.Cursor = Cursors.Default;

//                prbLogin.Visible = false;
                btnCancel.Enabled = true;
                btnLogin.Enabled = true;
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
                chkRememberUserName.Enabled = true;
            }
        }

        void TLoginFormActivated(object sender, EventArgs e)
        {
            // Needed to make sure the user can activate the Application Help
            RunOnceOnActivation();
        }

        private void TLoginFormShown(System.Object sender, System.EventArgs e)
        {
            if (!FPreviouslyShown)
            {
                /* Due to a quirk with .NET 2.0 WinForms, the LoginForm must be shown *twice* to
                 * make it receive the input focus. However, we want to run the code in this Event
                 * Handler only once, so we use the following control variable.
                 */
                FPreviouslyShown = true;

                if (System.Windows.Forms.Form.ModifierKeys != Keys.Alt)
                {
                    string UserNameStr = String.Empty;

                    if ((TAppSettingsManager.GetValue("AutoLogin",
                             false) != TAppSettingsManager.UNDEFINEDVALUE) && !ReadRememberedUserName(ref UserNameStr))
                    {
                        txtUserName.Text = TAppSettingsManager.GetValue("AutoLogin").ToUpper();

                        if (TAppSettingsManager.GetValue("AutoLoginPasswd") != TAppSettingsManager.UNDEFINEDVALUE)
                        {
                            txtPassword.Text = TAppSettingsManager.GetValue("AutoLoginPasswd");
                            BtnLoginClick(null, null);
                        }
                    }
                    else
                    {
                        GetUsers();
                    }
                }
                else
                {
                    GetUsers();
                }
            }

            SetFocusToCredentials();
        }

        private void TLoginFormLoad(System.Object sender, System.EventArgs e)
        {
            /* The following commands are needed to get the input focus after having
             * displayed the Splash Screen...
             */
            TWindowHandling.SetForegroundWindowWrapper(this.Handle);
        }

        #region ENTER Key Handlers

        void TxtPasswordKeyPress(object sender, KeyPressEventArgs e)
        {
            // If the ENTER key is pressed, the Handled property is set to true,
            // to indicate the event is handled and the Login Button is 'clicked'.

            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                BtnLoginClick(this, null);
            }
        }

        private void TxtPasswordOnEntering(object sender, EventArgs e)
        {
            this.txtPassword.SelectAll();
        }

        void TxtUserNameKeyPress(object sender, KeyPressEventArgs e)
        {
            // If the ENTER key is pressed, the Handled property is set to true,
            // to indicate the event is handled and the Focus is moved to the Password TextBox.

            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        }

        void ChkRememberUserNameKeyPress(object sender, KeyPressEventArgs e)
        {
            // If the ENTER key is pressed, the Handled property is set to true,
            // to indicate the event is handled and the 'Login' Button gets pressed.

            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                btnLogin.Focus();
                Application.DoEvents();

                btnLogin.PerformClick();
            }
        }

        #endregion

        private void BtnCancelClick(System.Object sender, System.EventArgs e)
        {
            Close();
        }

        // Button 'Login' pressed
        private void BtnLoginClick(System.Object sender, System.EventArgs e)
        {
            FConnectionEstablished = false;

            // Get the selected User Name and Password
            FSelUserName = this.txtUserName.Text.ToUpper();
            FSelPassWord = (this.txtPassword.Text);

            if ((FSelUserName.Length == 0) || (FSelPassWord.Length == 0))
            {
                MessageBox.Show(Catalog.GetString("Please enter a User ID and a Password to log in into OpenPetra."),
                    StrPetraLoginFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                SetFocusToCredentials();

                return;
            }

            // don't waste a try for authentication if the CapsLock is on; otherwise the user is retired quite quickly
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                MessageBox.Show(Catalog.GetString("The Caps Lock key is ON. Please switch it off and try to login again"),
                    StrPetraLoginFormTitle);

                SetFocusToCredentials();

                return;
            }

            /* Now authenticate User against the Petra Server.
             *
             * Note: Authenticate has to establish a connection to the Petra Server first
             */
            String ConnectionError;
            this.Cursor = Cursors.WaitCursor;

            StoreUserName(FSelUserName);

            UpdateUI(true);

            if (ConnectToPetraServer(FSelUserName, FSelPassWord, out ConnectionError))
            {
                prbLogin.Value = 90;
                bool MustChangePassword = false;

                // Show any message that is returned after successful login (eg. 'Petra is currently disabled due to xxx. Proceed with caution.')
                if (UserInfo.GUserInfo.LoginMessage != null)
                {
                    // If the user is required to change their password before using OpenPetra.
                    // (This LoginMessage is set in Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors.TUserManagerWebConnector)
                    MustChangePassword = (UserInfo.GUserInfo.LoginMessage == SharedConstants.LOGINMUSTCHANGEPASSWORD);

                    if (!MustChangePassword)
                    {
                        MessageBox.Show(UserInfo.GUserInfo.LoginMessage,
                            StrPetraLoginFormTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }

                TUserDefaults.InitUserDefaults();
                prbLogin.Value = 100;
                FConnectionEstablished = true;
                this.Cursor = Cursors.Default;
                TLogging.UserNamePrefix = UserInfo.GUserInfo.UserID + '_' +
                                          TConnectionManagement.GConnectionManagement.ClientID.ToString();

                UpdateUI(false);

                if (MustChangePassword)
                {
                    if (!CreateNewPassword(this, FSelUserName, FSelPassWord, true))
                    {
                        // do nothing if password has not been successfully changed
                        return;
                    }
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                this.Cursor = Cursors.Default;

                UpdateUI(false);

                // TODORemoting do not show the error code???
                if ((ConnectionError.Length > 0) && (ConnectionError.IndexOf(" ") != -1))
                {
                    // this message box shows if the password is wrong
                    // otherwise there has already been a message box usually
                    MessageBox.Show(ConnectionError, StrPetraLoginFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    SetFocusToCredentials();
                }
            }
        }

        private bool ConnectToPetraServer(String AUserName, String APassWord, out String AError)
        {
            eLoginEnum ReturnValue = eLoginEnum.eLoginFailedForUnspecifiedError;

            ReturnValue = ((TConnectionManagement)TConnectionManagement.GConnectionManagement).ConnectToServer(AUserName, APassWord,
                out FProcessID,
                out FWelcomeMessage,
                out FSystemEnabled,
                out AError);

            if (ReturnValue == eLoginEnum.eLoginSucceeded)
            {
                return true;
            }
            else if (ReturnValue == eLoginEnum.eLoginVersionMismatch)
            {
                DisplayLoginFailedMessage(Catalog.GetString("OpenPetra Client/Server Program Version Mismatch!"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginServerTooBusy)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "The OpenPetra Server is too busy to accept the Login request.\r\n\r\nPlease try again after a short time!"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginExceedingConcurrentUsers)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "Too many users are logged in."));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginServerNotReachable)
            {
                string Message = Catalog.GetString("The OpenPetra Server cannot be reached!");
                string Title = null;

                // on Standalone, we can find the Server.log file, and check the last 10 lines for "System.Exception: Unsupported upgrade"
                string ServerLog = Path.GetDirectoryName(TLogging.GetLogFileName()) + Path.DirectorySeparatorChar + "Server.log";

                if (File.Exists(ServerLog))
                {
                    int countExceptionLine = 0;
                    string ErrorMessage = string.Empty;

                    StreamReader sr = new StreamReader(ServerLog);

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.Contains("Unsupported upgrade"))
                        {
                            countExceptionLine = 12;
                            ErrorMessage = line.Substring(line.IndexOf("Unsupported upgrade"));
                        }
                        else
                        {
                            countExceptionLine--;
                        }
                    }

                    sr.Close();

                    if (countExceptionLine > 0)
                    {
                        Message = ErrorMessage.Replace('/', Path.DirectorySeparatorChar);
                        Title = Catalog.GetString("Unsupported upgrade");
                    }
                    else
                    {
                        Message =
                            Catalog.GetString(
                                "The OpenPetra Server cannot be reached!") + Environment.NewLine + StrDetailsInLogfile + ": " +
                            ServerLog;
                        Title = Catalog.GetString("No Server Response");
                    }
                }

                DisplayLoginFailedMessage(Message, Title);

                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginAuthenticationFailed)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "Wrong username or password"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginUserIsRetired)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "User is retired"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginUserRecordLocked)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "User record is locked at the moment"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginSystemDisabled)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "The system is disabled at the moment for maintenance"));
                return false;
            }
            else if (ReturnValue == eLoginEnum.eLoginVersionMismatch)
            {
                DisplayLoginFailedMessage(
                    Catalog.GetString(
                        "Please update the client, the version does not match the server version!"));
                return false;
            }

            // catching all other login failures, eg. eLoginEnum.eLoginFailedForUnspecifiedError
            DisplayLoginFailedMessage(
                Catalog.GetString(
                    "An error occurred while trying to connect to the OpenPetra Server!") + Environment.NewLine + StrDetailsInLogfile + ": " +
                TLogging.GetLogFileName());
            return false;
        }

        #region Get user names

        /// <summary>
        /// This function gets the windows user name and the previously used login name for Petra
        ///
        /// </summary>
        /// <returns>void</returns>
        private void GetUsers()
        {
            // in some countries, don't give a clue about the username
            if (TAppSettingsManager.ToBoolean(TAppSettingsManager.GetValue("UseWindowsUserID"), false))
            {
                // Get the windows user name
                String UserNameStr = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();

                // get rid of the domain and the back slash
                UserNameStr = UserNameStr.Substring(UserNameStr.IndexOf("\\") + 1);

                ReadRememberedUserName(ref UserNameStr);

                txtUserName.Text = UserNameStr;
            }
            else
            {
                chkRememberUserName.Visible = false;
            }
        }

        /// remember previously used user name in registry?
        /// the user name is not always equals the windows login name
        /// there is a a checkbox to avoid remembering the login name
        /// do not use registry, but isolated storage
        /// <returns>true if there is a remembered user name</returns>
        private bool ReadRememberedUserName(ref string AUsername)
        {
            IsolatedStorageFileStream stream = null;
            StreamReader sr = null;

            try
            {
                IsolatedStorageFile MyIsolatedStorageFile = IsolatedStorageFile.GetStore(
                    IsolatedStorageScope.User | IsolatedStorageScope.Assembly |
                    IsolatedStorageScope.Roaming, null, null);

                MyIsolatedStorageFile.CreateDirectory("OpenPetra/Settings");
                stream = new IsolatedStorageFileStream("OpenPetra/Settings/username.txt",
                    FileMode.Open,
                    FileAccess.Read,
                    MyIsolatedStorageFile);

                sr = new StreamReader(stream);
                string storedUserName = sr.ReadLine();
                chkRememberUserName.Checked = true;

                if (storedUserName == "DONTREMEMBER")
                {
                    chkRememberUserName.Checked = false;
                }
                else
                {
                    AUsername = storedUserName;
                }
            }
            catch (Exception)
            {
                // ignore the exception, file will be created
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }

            return chkRememberUserName.Checked;
        }

        private void StoreUserName(string AUsername)
        {
            IsolatedStorageFileStream stream = null;
            StreamWriter sw = null;

            try
            {
                IsolatedStorageFile MyIsolatedStorageFile = IsolatedStorageFile.GetStore(
                    IsolatedStorageScope.User | IsolatedStorageScope.Assembly |
                    IsolatedStorageScope.Roaming, null, null);

                MyIsolatedStorageFile.CreateDirectory("OpenPetra/Settings");
                stream = new IsolatedStorageFileStream("OpenPetra/Settings/username.txt",
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    MyIsolatedStorageFile);

                sw = new StreamWriter(stream);

                if (chkRememberUserName.Visible && chkRememberUserName.Checked)
                {
                    sw.WriteLine(AUsername);
                }
                else
                {
                    sw.WriteLine("DONTREMEMBER");
                }
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        #endregion

        /// <summary>
        /// create a new password for the current user
        /// </summary>
        public static bool CreateNewPassword(Form AParentForm, string AUserName, string AOldPassword, bool APasswordNeedsChanged)
        {
            TFrmChangePassword chgPwd = new TFrmChangePassword(AParentForm);
            chgPwd.UserName = AUserName;
            chgPwd.OldPassword = AOldPassword;
            chgPwd.PasswordNeedsChanged = APasswordNeedsChanged;
            
            return (chgPwd.ShowDialog() == DialogResult.OK);
        }

        private void SetFocusToCredentials()
        {
            if (txtUserName.Text.Length == 0)
            {
                txtUserName.Focus();
            }
            else
            {
                txtPassword.Focus();
            }

            // Attempt to stop the login dialog getting stuck behind other windows with no taskbar icon
            Activate();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <returns></returns>
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

        #region Implement interface functions

        /// auto generated
        public void RunOnceOnActivation()
        {
            FPetraUtilsObject.TFrmPetra_Activated(this, null);
        }

        /// <summary>
        /// Adds event handlers for the appropiate onChange event to call a central procedure
        /// </summary>
        public void HookupAllControls()
        {
        }

        /// auto generated
        public void HookupAllInContainer(Control container)
        {
            FPetraUtilsObject.HookupAllInContainer(container);
        }

        /// auto generated
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// auto generated
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }

        #endregion
    }
}
