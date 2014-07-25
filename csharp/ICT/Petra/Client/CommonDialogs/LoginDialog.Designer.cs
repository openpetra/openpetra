//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using System;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Ict.Petra.Client.CommonDialogs
{
    partial class TLoginForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
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
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TLoginForm));
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.prbLogin = new System.Windows.Forms.ProgressBar();
            this.pnlLoginControls = new System.Windows.Forms.Panel();
            this.chkRememberUserName = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPetraVersion = new System.Windows.Forms.Label();
            this.pnlLoginControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // lblUserName
            //
            this.lblUserName.Location = new System.Drawing.Point(9, 263);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(67, 23);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "&User ID:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtUserName
            //
            this.txtUserName.BackColor = System.Drawing.Color.LightGray;
            this.txtUserName.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(77, 260);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(210, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtUserNameKeyPress);
            this.txtUserName.Leave += new System.EventHandler(this.TxtUserNameLeave);
            //
            // lblPassword
            //
            this.lblPassword.Location = new System.Drawing.Point(9, 285);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(67, 23);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "&Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtPassword
            //
            this.txtPassword.AcceptsReturn = true;
            this.txtPassword.BackColor = System.Drawing.Color.LightGray;
            this.txtPassword.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(77, 284);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(210, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.WordWrap = false;
            this.txtPassword.Enter += new System.EventHandler(this.TxtPasswordOnEntering);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPasswordKeyPress);
            //
            // btnLogin
            //
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(103, 7);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(95, 24);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = " &Login";
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.Click += new System.EventHandler(this.BtnLoginClick);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(202, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = " &Quit";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            //
            // prbLogin
            //
            this.prbLogin.Location = new System.Drawing.Point(61, 9);
            this.prbLogin.Name = "prbLogin";
            this.prbLogin.Size = new System.Drawing.Size(178, 20);
            this.prbLogin.TabIndex = 10;
            this.prbLogin.Visible = false;
            //
            // pnlLoginControls
            //
            this.pnlLoginControls.Controls.Add(this.btnLogin);
            this.pnlLoginControls.Controls.Add(this.btnCancel);
            this.pnlLoginControls.Controls.Add(this.prbLogin);
            this.pnlLoginControls.Location = new System.Drawing.Point(7, 353);
            this.pnlLoginControls.Name = "pnlLoginControls";
            this.pnlLoginControls.Size = new System.Drawing.Size(300, 36);
            this.pnlLoginControls.TabIndex = 11;
            //
            // chkRememberUserName
            //
            this.chkRememberUserName.Checked = true;
            this.chkRememberUserName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRememberUserName.Location = new System.Drawing.Point(77, 308);
            this.chkRememberUserName.Name = "chkRememberUserName";
            this.chkRememberUserName.Size = new System.Drawing.Size(188, 24);
            this.chkRememberUserName.TabIndex = 12;
            this.chkRememberUserName.Text = "&Remember the User ID";
            this.chkRememberUserName.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(7, 334);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "Initial Login: demo/demo or sysadmin/CHANGEME";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // pictureBox1
            //
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(56, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(203, 223);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            //
            // lblPetraVersion
            //
            this.lblPetraVersion.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPetraVersion.Location = new System.Drawing.Point(20, 231);
            this.lblPetraVersion.Name = "lblPetraVersion";
            this.lblPetraVersion.Size = new System.Drawing.Size(267, 18);
            this.lblPetraVersion.TabIndex = 2;
            this.lblPetraVersion.Text = "Version";
            this.lblPetraVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // TLoginForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(309, 388);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblPetraVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkRememberUserName);
            this.Controls.Add(this.pnlLoginControls);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenPetra Login";
            this.Activated += new System.EventHandler(this.TLoginFormActivated);
            this.Load += new System.EventHandler(this.TLoginFormLoad);
            this.Shown += new System.EventHandler(this.TLoginFormShown);
            this.pnlLoginControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRememberUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPetraVersion;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.ProgressBar prbLogin;
        private System.Windows.Forms.Panel pnlLoginControls;
    }
}