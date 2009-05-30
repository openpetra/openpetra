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
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Mono.Unix;
using System.Security.Principal;
using Ict.Petra.Client.App.Core;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// login form for authentication of Petra user
    /// </summary>
    public class TLoginForm : System.Windows.Forms.Form
    {
        private static bool FPreviouslyShown = false;

        /// Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>todoComment</summary>
        public const String PETRA_LOGIN_FORMTITLE = "Petra Login";

        /// <summary>todoComment</summary>
        public const String StrConnectionNotYetEst = "Please try again, \r\nthe connection has not" + " been established yet";

        /// <summary>todoComment</summary>
        public const String StrConnectionNotYetEstTitle = "Connection Problem";

        /// <summary>todoComment</summary>
        public const String StrEnterUserIDAndPwd = "Enter User ID and Password to login.";

        /// <summary>todoComment</summary>
        public const String StrConnToServerCouldntEst = "Connection to Server could not be established!";

        /// <summary>todoComment</summary>
        public const String StrLoginFailed = "Login failed!";

        /// <summary>todoComment</summary>
        public const String StrDetailsInLogfile = "For details see the log file";

        /// <summary>todoComment</summary>
        public const String StrCapsLockIsOn = "The Caps Lock key is ON. Please switch it off and try to login again";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ProgressBar prbLogin;
        private System.Windows.Forms.Panel pnlLoginControls;

        /// <summary>Private Declarations</summary>
        private String FSelUserName;
        private String FSelPassWord;
        private String FWelcomeMessage;
        private Boolean FSystemEnabled;
        private Int32 FProcessID;
        private Boolean FConnectionEstablished;

        private void DisplayLoginFailedMessage(string AMessage)
        {
            if (!((AMessage == null) || (AMessage == "")))
            {
                MessageBox.Show(AMessage, PETRA_LOGIN_FORMTITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TLoginForm));
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.prbLogin = new System.Windows.Forms.ProgressBar();
            this.pnlLoginControls = new System.Windows.Forms.Panel();
            this.pnlLoginControls.SuspendLayout();
            this.SuspendLayout();

            //
            // lblUserName
            //
            this.lblUserName.Location = new System.Drawing.Point(8, 8);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(188, 21);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "&User ID:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtUserName
            //
            this.txtUserName.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(8, 30);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(188, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Leave += new System.EventHandler(this.TxtUserName_Leave);

            //
            // lblPassword
            //
            this.lblPassword.Location = new System.Drawing.Point(8, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(188, 23);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "&Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtPassword
            //
            this.txtPassword.AcceptsReturn = true;
            this.txtPassword.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(8, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(188, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.WordWrap = false;

            //
            // btnLogin
            //
            this.btnLogin.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(4, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = " &Login";
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(4, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = " &Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            //
            // lblDatabase
            //
            this.lblDatabase.Location = new System.Drawing.Point(8, 106);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(188, 32);
            this.lblDatabase.TabIndex = 8;
            this.lblDatabase.Text = "Database:";
            this.lblDatabase.Visible = false;

            //
            // prbLogin
            //
            this.prbLogin.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.prbLogin.Location = new System.Drawing.Point(4, 38);
            this.prbLogin.Name = "prbLogin";
            this.prbLogin.Size = new System.Drawing.Size(76, 18);
            this.prbLogin.TabIndex = 10;
            this.prbLogin.Visible = false;

            //
            // pnlLoginControls
            //
            this.pnlLoginControls.Controls.Add(this.btnCancel);
            this.pnlLoginControls.Controls.Add(this.prbLogin);
            this.pnlLoginControls.Controls.Add(this.btnLogin);
            this.pnlLoginControls.Location = new System.Drawing.Point(205, 76);
            this.pnlLoginControls.Name = "pnlLoginControls";
            this.pnlLoginControls.Size = new System.Drawing.Size(82, 66);
            this.pnlLoginControls.TabIndex = 11;

            //
            // TLoginForm
            //
            this.AcceptButton = this.btnLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(293, 142);
            this.Controls.Add(this.pnlLoginControls);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Petra Login";
            this.TopMost = false;
            this.Load += new System.EventHandler(this.TLoginForm_Load);
            this.Shown += new System.EventHandler(this.TLoginForm_Shown);
            this.pnlLoginControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// This procedure disposes the login window.
        /// </summary>
        /// <param name="Disposing">true indicates that the System.Object can not be recreated with the RecreateHandle method.
        /// </param>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
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

            this.Text = PETRA_LOGIN_FORMTITLE;
            this.Height = 142;
            pnlLoginControls.Top = 46;
        }

        private void TxtUserName_Leave(System.Object sender, System.EventArgs e)
        {
            this.txtUserName.Text = this.txtUserName.Text.ToUpper();
        }

        private void UpdateUI(Boolean APerformingLogin)
        {
            if (APerformingLogin)
            {
                this.Cursor = Cursors.WaitCursor;

                prbLogin.Visible = true;
                btnCancel.Visible = false;
                btnLogin.Enabled = false;
                txtPassword.Enabled = false;
                txtUserName.Enabled = false;
                btnLogin.Focus();

                Application.DoEvents();
            }
            else
            {
                this.Cursor = Cursors.Default;

                prbLogin.Visible = false;
                btnCancel.Visible = true;
                btnLogin.Enabled = true;
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
            }
        }

        private void TLoginForm_Shown(System.Object sender, System.EventArgs e)
        {
            if (!FPreviouslyShown)
            {
                /* Due to a quirk with .NET 2.0 WinForms, the LoginForm must be shown *twice* to
                 * make it receive the input focus. However, we want to run the code in this Event
                 * Handler only once, so we use the following control variable.
                 */
                FPreviouslyShown = true;

                TCmdOpts CmdOpts = new TCmdOpts();

                if (CmdOpts.IsFlagSet("AutoLogin") == true)
                {
                    txtUserName.Text = CmdOpts.GetOptValue("AutoLogin");

                    if (CmdOpts.IsFlagSet("AutoLoginPasswd"))
                    {
                        txtPassword.Text = CmdOpts.GetOptValue("AutoLoginPasswd");
                        BtnLogin_Click(null, null);
                    }
                }
                else
                {
                    GetUsers();
                }
            }

            if (txtUserName.Text.Length == 0)
            {
                txtUserName.Focus();
            }
            else
            {
                txtPassword.Focus();
            }
        }

        private void TLoginForm_Load(System.Object sender, System.EventArgs e)
        {
            /* The following commands are needed to get the input focus after having
             * displayed the Splash Screen...
             */
            SetForegroundWindow(this.Handle);
        }

        private void BtnCancel_Click(System.Object sender, System.EventArgs e)
        {
            Close();
        }

        // Button 'Login' pressed
        private void BtnLogin_Click(System.Object sender, System.EventArgs e)
        {
            FConnectionEstablished = false;

            // Get the selected User Name and Password
            FSelUserName = this.txtUserName.Text;
            FSelPassWord = (this.txtPassword.Text);

            if ((FSelUserName.Length == 0) || (FSelPassWord.Length == 0))
            {
                MessageBox.Show(StrEnterUserIDAndPwd, PETRA_LOGIN_FORMTITLE);
                return;
            }

            // don't waste a try for authentication if the CapsLock is on; otherwise the user is retired quite quickly
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                MessageBox.Show(StrCapsLockIsOn, PETRA_LOGIN_FORMTITLE);
                return;
            }

            /* Now authenticate User against the Petra Server.
             *
             * Note: Authenticate has to establish a connection to the Petra Server first
             */
            String ConnectionError;
            this.Cursor = Cursors.WaitCursor;

            if (ConnectToPetraServer(FSelUserName, FSelPassWord, out ConnectionError))
            {
                prbLogin.Value = 90;

                // Show any message that is returned after successful login (eg. 'Petra is currently disabled due to xxx. Proceed with caution.')
                if (UserInfo.GUserInfo.LoginMessage != null)
                {
                    MessageBox.Show(UserInfo.GUserInfo.LoginMessage,
                        PETRA_LOGIN_FORMTITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                TUserDefaults.GUserDefaults = new TUserDefaults();
                prbLogin.Value = 100;
                FConnectionEstablished = true;
                this.Cursor = Cursors.Default;
                TLogging.UserNamePrefix = UserInfo.GUserInfo.UserID + '_' +
                                          TConnectionManagement.GConnectionManagement.ClientID.ToString();

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                this.Cursor = Cursors.Default;

                // TODO what todo on connection failure?
                if (ConnectionError.Length > 0)
                {
                    // this message box shows if the password is wrong
                    // otherwise there has already been a message box usually
                    MessageBox.Show(ConnectionError);
                }
            }
        }

        private bool ConnectToPetraServer(String AUserName, String APassWord, out String AError)
        {
            bool ReturnValue = false;

            AError = "";
            try
            {
                ReturnValue = TConnectionManagement.GConnectionManagement.ConnectToServer(AUserName, APassWord,
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
                        "Too many users are logged in." + "\r\n" + "\r\n" +
                        " Please contact petra-admin@ict-software.org to order more licenses if necessary.",
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
                    MessageBox.Show(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + StrDetailsInLogfile + ": " +
                        TLogging.GetLogFileName(),
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
                MessageBox.Show(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + StrDetailsInLogfile + ": " +
                    TLogging.GetLogFileName(),
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
                MessageBox.Show(
                    "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + StrDetailsInLogfile + ": " +
                    TLogging.GetLogFileName(),
                    "Server connection error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
#endif
                return false;
            }

            // TODO: exception for authentication failure
            // TODO: exception for retired user
            return ReturnValue;
        }

        #region Get user names

        /// <summary>
        /// This function gets the windows user name and the previously used login name for Petra
        ///
        /// </summary>
        /// <returns>void</returns>
        private void GetUsers()
        {
            // TODO: remember previously used user name in registry?
            //       the user name is not always equals the windows login name
            //       add a checkbox to avoid remembering the login name?

            // in some countries, don't give a clue about the username
            if (TAppSettingsManager.ToBoolean(TAppSettingsManager.GetValueStatic("UseWindowsUserID"), false))
            {
                // Get the windows user name
                String UserNameStr = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();

                // get rid of the domain and the back slash
                UserNameStr = UserNameStr.Substring(UserNameStr.IndexOf("\\") + 1);
                txtUserName.Text = UserNameStr;
            }
        }

        #endregion

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
    }
}