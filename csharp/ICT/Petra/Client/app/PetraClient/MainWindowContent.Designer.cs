//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2010 by OM International
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
namespace Ict.Petra.Client.App.PetraClient
{
    partial class TUcoMainWindowContent
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUcoMainWindowContent));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.btnHospitality = new System.Windows.Forms.Button();
            this.btnSysMan = new System.Windows.Forms.Button();
            this.btnAccounts = new System.Windows.Forms.Button();
            this.btnPersonnel = new System.Windows.Forms.Button();
            this.btnPartner = new System.Windows.Forms.Button();
            this.btnConference = new System.Windows.Forms.Button();
            this.btnDevelopment = new System.Windows.Forms.Button();
            this.lblPartner = new System.Windows.Forms.Label();
            this.lblAccounts = new System.Windows.Forms.Label();
            this.lblPersonnel = new System.Windows.Forms.Label();
            this.lblConference = new System.Windows.Forms.Label();
            this.lblDevelopment = new System.Windows.Forms.Label();
            this.lblSysMan = new System.Windows.Forms.Label();
            this.pbxPetraRocks = new System.Windows.Forms.PictureBox();
            this.lblWelcomeMessage = new System.Windows.Forms.Label();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblLastFailedLogin = new System.Windows.Forms.Label();
            this.lblFailedLoginsLabel = new System.Windows.Forms.Label();
            this.lblFailedLogins = new System.Windows.Forms.Label();
            this.lblUserLabel = new System.Windows.Forms.Label();
            this.lblLastFailedLoginLabel = new System.Windows.Forms.Label();
            this.lblLastLoginLabel = new System.Windows.Forms.Label();
            this.lblLastLogin = new System.Windows.Forms.Label();
            this.imlSysManager = new System.Windows.Forms.ImageList(this.components);
            this.imlAccounts = new System.Windows.Forms.ImageList(this.components);
            this.imlPersonnel = new System.Windows.Forms.ImageList(this.components);
            this.imlPartner = new System.Windows.Forms.ImageList(this.components);
            this.imlConference = new System.Windows.Forms.ImageList(this.components);
            this.imlDevelopment = new System.Windows.Forms.ImageList(this.components);
            this.imlButtons = new System.Windows.Forms.ImageList(this.components);
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPetraRocks)).BeginInit();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();

            //
            // Panel1
            //
            this.Panel1.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.Panel1.Controls.Add(this.btnHospitality);
            this.Panel1.Controls.Add(this.btnSysMan);
            this.Panel1.Controls.Add(this.btnAccounts);
            this.Panel1.Controls.Add(this.btnPersonnel);
            this.Panel1.Controls.Add(this.btnPartner);
            this.Panel1.Controls.Add(this.btnConference);
            this.Panel1.Controls.Add(this.btnDevelopment);
            this.Panel1.Controls.Add(this.lblPartner);
            this.Panel1.Controls.Add(this.lblAccounts);
            this.Panel1.Controls.Add(this.lblPersonnel);
            this.Panel1.Controls.Add(this.lblConference);
            this.Panel1.Controls.Add(this.lblDevelopment);
            this.Panel1.Controls.Add(this.lblSysMan);
            this.Panel1.Location = new System.Drawing.Point(5, 258);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(502, 108);
            this.Panel1.TabIndex = 52;

            //
            // btnHospitality
            //
            this.btnHospitality.Location = new System.Drawing.Point(257, 85);
            this.btnHospitality.Name = "btnHospitality";
            this.btnHospitality.Size = new System.Drawing.Size(75, 23);
            this.btnHospitality.TabIndex = 18;
            this.btnHospitality.Text = "Hospitality";
            this.btnHospitality.UseVisualStyleBackColor = true;
            this.btnHospitality.Visible = false;

            //
            // btnSysMan
            //
            this.btnSysMan.ImageIndex = 0;
            this.btnSysMan.ImageList = this.imlSysManager;
            this.btnSysMan.Location = new System.Drawing.Point(420, 4);
            this.btnSysMan.Name = "btnSysMan";
            this.btnSysMan.Size = new System.Drawing.Size(56, 56);
            this.btnSysMan.TabIndex = 6;
            this.btnSysMan.Click += new System.EventHandler(this.btnSysManClick);

            //
            // btnAccounts
            //
            this.btnAccounts.ImageIndex = 0;
            this.btnAccounts.ImageList = this.imlAccounts;
            this.btnAccounts.Location = new System.Drawing.Point(80, 4);
            this.btnAccounts.Name = "btnAccounts";
            this.btnAccounts.Size = new System.Drawing.Size(56, 56);
            this.btnAccounts.TabIndex = 2;
            this.btnAccounts.Click += new System.EventHandler(this.btnAccountsClick);

            //
            // btnPersonnel
            //
            this.btnPersonnel.ImageIndex = 0;
            this.btnPersonnel.ImageList = this.imlPersonnel;
            this.btnPersonnel.Location = new System.Drawing.Point(156, 4);
            this.btnPersonnel.Name = "btnPersonnel";
            this.btnPersonnel.Size = new System.Drawing.Size(56, 56);
            this.btnPersonnel.TabIndex = 3;
            this.btnPersonnel.Click += new System.EventHandler(this.btnPersonnelClick);

            //
            // btnPartner
            //
            this.btnPartner.ImageIndex = 0;
            this.btnPartner.ImageList = this.imlPartner;
            this.btnPartner.Location = new System.Drawing.Point(4, 4);
            this.btnPartner.Name = "btnPartner";
            this.btnPartner.Size = new System.Drawing.Size(56, 56);
            this.btnPartner.TabIndex = 1;
            this.btnPartner.Click += new System.EventHandler(this.btnPartnerClick);

            //
            // btnConference
            //
            this.btnConference.ImageIndex = 0;
            this.btnConference.ImageList = this.imlConference;
            this.btnConference.Location = new System.Drawing.Point(232, 4);
            this.btnConference.Name = "btnConference";
            this.btnConference.Size = new System.Drawing.Size(56, 56);
            this.btnConference.TabIndex = 4;
            this.btnConference.Click += new System.EventHandler(this.btnConferenceClick);

            //
            // btnDevelopment
            //
            this.btnDevelopment.ImageIndex = 0;
            this.btnDevelopment.ImageList = this.imlDevelopment;
            this.btnDevelopment.Location = new System.Drawing.Point(310, 4);
            this.btnDevelopment.Name = "btnDevelopment";
            this.btnDevelopment.Size = new System.Drawing.Size(56, 56);
            this.btnDevelopment.TabIndex = 5;
            this.btnDevelopment.Click += new System.EventHandler(this.btnPersonnelClick);

            //
            // lblPartner
            //
            this.lblPartner.Location = new System.Drawing.Point(4, 68);
            this.lblPartner.Name = "lblPartner";
            this.lblPartner.Size = new System.Drawing.Size(56, 18);
            this.lblPartner.TabIndex = 9;
            this.lblPartner.Text = "Pa&rtner";
            this.lblPartner.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // lblAccounts
            //
            this.lblAccounts.Location = new System.Drawing.Point(80, 68);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(56, 18);
            this.lblAccounts.TabIndex = 10;
            this.lblAccounts.Text = "F&inance";
            this.lblAccounts.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // lblPersonnel
            //
            this.lblPersonnel.Location = new System.Drawing.Point(151, 68);
            this.lblPersonnel.Name = "lblPersonnel";
            this.lblPersonnel.Size = new System.Drawing.Size(63, 18);
            this.lblPersonnel.TabIndex = 11;
            this.lblPersonnel.Text = "&Personnel";
            this.lblPersonnel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // lblConference
            //
            this.lblConference.Location = new System.Drawing.Point(218, 68);
            this.lblConference.Name = "lblConference";
            this.lblConference.Size = new System.Drawing.Size(80, 32);
            this.lblConference.TabIndex = 12;
            this.lblConference.Text = "C&onference Management";
            this.lblConference.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // lblDevelopment
            //
            this.lblDevelopment.Location = new System.Drawing.Point(298, 68);
            this.lblDevelopment.Name = "lblDevelopment";
            this.lblDevelopment.Size = new System.Drawing.Size(83, 32);
            this.lblDevelopment.TabIndex = 13;
            this.lblDevelopment.Text = "Financial &Development";
            this.lblDevelopment.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // lblSysMan
            //
            this.lblSysMan.Location = new System.Drawing.Point(420, 68);
            this.lblSysMan.Name = "lblSysMan";
            this.lblSysMan.Size = new System.Drawing.Size(56, 32);
            this.lblSysMan.TabIndex = 14;
            this.lblSysMan.Text = "&System Manager";
            this.lblSysMan.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // pbxPetraRocks
            //
            this.pbxPetraRocks.Image = ((System.Drawing.Image)(resources.GetObject("pbxPetraRocks.Image")));
            this.pbxPetraRocks.Location = new System.Drawing.Point(3, 3);
            this.pbxPetraRocks.Name = "pbxPetraRocks";
            this.pbxPetraRocks.Size = new System.Drawing.Size(502, 252);
            this.pbxPetraRocks.TabIndex = 53;
            this.pbxPetraRocks.TabStop = false;

            //
            // lblWelcomeMessage
            //
            this.lblWelcomeMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(150)))), ((int)(((byte)(100)))));
            this.lblWelcomeMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWelcomeMessage.Font = new System.Drawing.Font("Courier New",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblWelcomeMessage.Location = new System.Drawing.Point(297, 35);
            this.lblWelcomeMessage.Name = "lblWelcomeMessage";
            this.lblWelcomeMessage.Size = new System.Drawing.Size(182, 122);
            this.lblWelcomeMessage.TabIndex = 54;

            //
            // Panel2
            //
            this.Panel2.Controls.Add(this.lblUserName);
            this.Panel2.Controls.Add(this.lblLastFailedLogin);
            this.Panel2.Controls.Add(this.lblFailedLoginsLabel);
            this.Panel2.Controls.Add(this.lblFailedLogins);
            this.Panel2.Controls.Add(this.lblUserLabel);
            this.Panel2.Controls.Add(this.lblLastFailedLoginLabel);
            this.Panel2.Controls.Add(this.lblLastLoginLabel);
            this.Panel2.Controls.Add(this.lblLastLogin);
            this.Panel2.Controls.Add(this.Panel1);
            this.Panel2.Controls.Add(this.lblWelcomeMessage);
            this.Panel2.Controls.Add(this.pbxPetraRocks);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(510, 411);
            this.Panel2.TabIndex = 55;

            //
            // lblUserName
            //
            this.lblUserName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(42, 374);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(102, 16);
            this.lblUserName.TabIndex = 22;
            this.lblUserName.Text = "";

            //
            // lblLastFailedLogin
            //
            this.lblLastFailedLogin.Location = new System.Drawing.Point(228, 392);
            this.lblLastFailedLogin.Name = "lblLastFailedLogin";
            this.lblLastFailedLogin.Size = new System.Drawing.Size(142, 16);
            this.lblLastFailedLogin.TabIndex = 24;
            this.lblLastFailedLogin.Text = "";

            //
            // lblFailedLoginsLabel
            //
            this.lblFailedLoginsLabel.Location = new System.Drawing.Point(8, 392);
            this.lblFailedLoginsLabel.Name = "lblFailedLoginsLabel";
            this.lblFailedLoginsLabel.Size = new System.Drawing.Size(85, 16);
            this.lblFailedLoginsLabel.TabIndex = 25;
            this.lblFailedLoginsLabel.Text = "Failed Logins:";

            //
            // lblFailedLogins
            //
            this.lblFailedLogins.Location = new System.Drawing.Point(90, 392);
            this.lblFailedLogins.Name = "lblFailedLogins";
            this.lblFailedLogins.Size = new System.Drawing.Size(22, 16);
            this.lblFailedLogins.TabIndex = 26;
            this.lblFailedLogins.Text = "";

            //
            // lblUserLabel
            //
            this.lblUserLabel.Location = new System.Drawing.Point(8, 374);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.Size = new System.Drawing.Size(38, 16);
            this.lblUserLabel.TabIndex = 19;
            this.lblUserLabel.Text = "User:";

            //
            // lblLastFailedLoginLabel
            //
            this.lblLastFailedLoginLabel.Location = new System.Drawing.Point(120, 392);
            this.lblLastFailedLoginLabel.Name = "lblLastFailedLoginLabel";
            this.lblLastFailedLoginLabel.Size = new System.Drawing.Size(106, 16);
            this.lblLastFailedLoginLabel.TabIndex = 20;
            this.lblLastFailedLoginLabel.Text = "Last Failed Login:";
            this.lblLastFailedLoginLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblLastLoginLabel
            //
            this.lblLastLoginLabel.Location = new System.Drawing.Point(128, 374);
            this.lblLastLoginLabel.Name = "lblLastLoginLabel";
            this.lblLastLoginLabel.Size = new System.Drawing.Size(98, 16);
            this.lblLastLoginLabel.TabIndex = 21;
            this.lblLastLoginLabel.Text = "Last Login:";
            this.lblLastLoginLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblLastLogin
            //
            this.lblLastLogin.Location = new System.Drawing.Point(228, 374);
            this.lblLastLogin.Name = "lblLastLogin";
            this.lblLastLogin.Size = new System.Drawing.Size(142, 16);
            this.lblLastLogin.TabIndex = 23;
            this.lblLastLogin.Text = "";

            //
            // imlSysManager
            //
            this.imlSysManager.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSysManager.ImageStream")));
            this.imlSysManager.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSysManager.Images.SetKeyName(0, "");
            this.imlSysManager.Images.SetKeyName(1, "");

            //
            // imlAccounts
            //
            this.imlAccounts.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlAccounts.ImageStream")));
            this.imlAccounts.TransparentColor = System.Drawing.Color.Transparent;
            this.imlAccounts.Images.SetKeyName(0, "");
            this.imlAccounts.Images.SetKeyName(1, "");

            //
            // imlPersonnel
            //
            this.imlPersonnel.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlPersonnel.ImageStream")));
            this.imlPersonnel.TransparentColor = System.Drawing.Color.Transparent;
            this.imlPersonnel.Images.SetKeyName(0, "");
            this.imlPersonnel.Images.SetKeyName(1, "");

            //
            // imlPartner
            //
            this.imlPartner.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlPartner.ImageStream")));
            this.imlPartner.TransparentColor = System.Drawing.Color.Transparent;
            this.imlPartner.Images.SetKeyName(0, "");
            this.imlPartner.Images.SetKeyName(1, "");

            //
            // imlConference
            //
            this.imlConference.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlConference.ImageStream")));
            this.imlConference.TransparentColor = System.Drawing.Color.Transparent;
            this.imlConference.Images.SetKeyName(0, "");
            this.imlConference.Images.SetKeyName(1, "");

            //
            // imlDevelopment
            //
            this.imlDevelopment.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlDevelopment.ImageStream")));
            this.imlDevelopment.TransparentColor = System.Drawing.Color.Transparent;
            this.imlDevelopment.Images.SetKeyName(0, "");
            this.imlDevelopment.Images.SetKeyName(1, "");

            //
            // imlButtons
            //
            this.imlButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlButtons.ImageStream")));
            this.imlButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlButtons.Images.SetKeyName(0, "");

            //
            // TUcoMainWindowContent
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Panel2);
            this.Name = "TUcoMainWindowContent";
            this.Size = new System.Drawing.Size(510, 414);
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPetraRocks)).EndInit();
            this.Panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList imlButtons;
        private System.Windows.Forms.ImageList imlDevelopment;
        private System.Windows.Forms.ImageList imlConference;
        private System.Windows.Forms.ImageList imlPartner;
        private System.Windows.Forms.ImageList imlPersonnel;
        private System.Windows.Forms.ImageList imlAccounts;
        private System.Windows.Forms.ImageList imlSysManager;
        private System.Windows.Forms.Label lblLastLogin;
        private System.Windows.Forms.Label lblLastLoginLabel;
        private System.Windows.Forms.Label lblLastFailedLoginLabel;
        private System.Windows.Forms.Label lblUserLabel;
        private System.Windows.Forms.Label lblFailedLogins;
        private System.Windows.Forms.Label lblFailedLoginsLabel;
        private System.Windows.Forms.Label lblLastFailedLogin;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.Label lblWelcomeMessage;
        private System.Windows.Forms.PictureBox pbxPetraRocks;
        private System.Windows.Forms.Label lblSysMan;
        private System.Windows.Forms.Label lblDevelopment;
        private System.Windows.Forms.Label lblConference;
        private System.Windows.Forms.Label lblPersonnel;
        private System.Windows.Forms.Label lblAccounts;
        private System.Windows.Forms.Label lblPartner;
        private System.Windows.Forms.Button btnDevelopment;
        private System.Windows.Forms.Button btnConference;
        private System.Windows.Forms.Button btnPartner;
        private System.Windows.Forms.Button btnPersonnel;
        private System.Windows.Forms.Button btnAccounts;
        private System.Windows.Forms.Button btnSysMan;
        private System.Windows.Forms.Button btnHospitality;
        private System.Windows.Forms.Panel Panel1;
    }
}