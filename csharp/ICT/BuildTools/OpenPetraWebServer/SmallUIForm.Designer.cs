//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2013 by OM International
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
namespace Ict.Tools.OpenPetraWebServer
{
    partial class SmallUIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmallUIForm));
            this.txtDefaultPage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPortNumber = new System.Windows.Forms.TextBox();
            this.txtPhysicalPath = new System.Windows.Forms.TextBox();
            this.txtVirtualPath = new System.Windows.Forms.TextBox();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.chkAllowRemoteConnection = new System.Windows.Forms.CheckBox();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.chkLogPageRequests = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(245, 63);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(170, 13);
            label3.TabIndex = 6;
            label3.Text = "Port number ( this must be unique )";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(13, 15);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(179, 13);
            label2.TabIndex = 2;
            label2.Text = "Physical path on the local file system";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 63);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(185, 13);
            label1.TabIndex = 4;
            label1.Text = "Optional virtual path ( e.g.OpenPetra )";
            //
            // txtDefaultPage
            //
            this.txtDefaultPage.Location = new System.Drawing.Point(13, 127);
            this.txtDefaultPage.Name = "txtDefaultPage";
            this.txtDefaultPage.ReadOnly = true;
            this.txtDefaultPage.Size = new System.Drawing.Size(202, 20);
            this.txtDefaultPage.TabIndex = 9;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(479, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Default page ( you can leave this blank if it is a standard page name like defaul" +
                               "t.htm or default.aspx )";
            //
            // txtPortNumber
            //
            this.txtPortNumber.Location = new System.Drawing.Point(248, 79);
            this.txtPortNumber.Name = "txtPortNumber";
            this.txtPortNumber.ReadOnly = true;
            this.txtPortNumber.Size = new System.Drawing.Size(58, 20);
            this.txtPortNumber.TabIndex = 7;
            //
            // txtPhysicalPath
            //
            this.txtPhysicalPath.Location = new System.Drawing.Point(13, 31);
            this.txtPhysicalPath.Name = "txtPhysicalPath";
            this.txtPhysicalPath.ReadOnly = true;
            this.txtPhysicalPath.Size = new System.Drawing.Size(565, 20);
            this.txtPhysicalPath.TabIndex = 3;
            //
            // txtVirtualPath
            //
            this.txtVirtualPath.Location = new System.Drawing.Point(13, 79);
            this.txtVirtualPath.Name = "txtVirtualPath";
            this.txtVirtualPath.ReadOnly = true;
            this.txtVirtualPath.Size = new System.Drawing.Size(164, 20);
            this.txtVirtualPath.TabIndex = 5;
            //
            // btnHide
            //
            this.btnHide.Location = new System.Drawing.Point(503, 196);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(75, 23);
            this.btnHide.TabIndex = 0;
            this.btnHide.Text = "&Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(12, 196);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            //
            // notifyIcon
            //
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            //
            // contextMenuStrip
            //
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.openToolStripMenuItem,
                    this.closeToolStripMenuItem,
                    this.startToolStripMenuItem,
                    this.browseToolStripMenuItem,
                    this.viewLogToolStripMenuItem,
                    this.helpToolStripMenuItem,
                    this.toolStripMenuItem1,
                    this.exitToolStripMenuItem
                });
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(123, 164);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            //
            // openToolStripMenuItem
            //
            this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            //
            // closeToolStripMenuItem
            //
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            //
            // startToolStripMenuItem
            //
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            //
            // browseToolStripMenuItem
            //
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.browseToolStripMenuItem.Text = "Browse";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.browseToolStripMenuItem_Click);
            //
            // viewLogToolStripMenuItem
            //
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            //
            // helpToolStripMenuItem
            //
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.contentsToolStripMenuItem,
                    this.aboutToolStripMenuItem
                });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.helpToolStripMenuItem.Text = "Help";
            //
            // contentsToolStripMenuItem
            //
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            //
            // aboutToolStripMenuItem
            //
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "About ...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(119, 6);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            //
            // lblServerStatus
            //
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(48, 164);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(168, 13);
            this.lblServerStatus.TabIndex = 10;
            this.lblServerStatus.Text = "Starting the server.  Please wait ...";
            //
            // linkLabel
            //
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(245, 164);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(49, 13);
            this.linkLabel.TabIndex = 11;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "linkLabel";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            //
            // timer
            //
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // chkAllowRemoteConnection
            //
            this.chkAllowRemoteConnection.AutoSize = true;
            this.chkAllowRemoteConnection.Enabled = false;
            this.chkAllowRemoteConnection.Location = new System.Drawing.Point(436, 79);
            this.chkAllowRemoteConnection.Name = "chkAllowRemoteConnection";
            this.chkAllowRemoteConnection.Size = new System.Drawing.Size(142, 17);
            this.chkAllowRemoteConnection.TabIndex = 12;
            this.chkAllowRemoteConnection.Text = "Allow remote connection";
            this.chkAllowRemoteConnection.UseVisualStyleBackColor = true;
            //
            // btnShowLog
            //
            this.btnShowLog.Location = new System.Drawing.Point(117, 196);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(75, 23);
            this.btnShowLog.TabIndex = 13;
            this.btnShowLog.Text = "Show Log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            //
            // chkLogPageRequests
            //
            this.chkLogPageRequests.AutoSize = true;
            this.chkLogPageRequests.Enabled = false;
            this.chkLogPageRequests.Location = new System.Drawing.Point(436, 129);
            this.chkLogPageRequests.Name = "chkLogPageRequests";
            this.chkLogPageRequests.Size = new System.Drawing.Size(114, 17);
            this.chkLogPageRequests.TabIndex = 14;
            this.chkLogPageRequests.Text = "Log page requests";
            this.chkLogPageRequests.UseVisualStyleBackColor = true;
            //
            // SmallUIForm
            //
            this.AcceptButton = this.btnHide;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 231);
            this.Controls.Add(this.chkLogPageRequests);
            this.Controls.Add(this.btnShowLog);
            this.Controls.Add(this.chkAllowRemoteConnection);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.lblServerStatus);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.txtDefaultPage);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPortNumber);
            this.Controls.Add(label3);
            this.Controls.Add(this.txtPhysicalPath);
            this.Controls.Add(label2);
            this.Controls.Add(this.txtVirtualPath);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SmallUIForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Petra Web Server Site Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SmallUIForm_FormClosing);
            this.Shown += new System.EventHandler(this.SmallUIForm_Shown);
            this.Resize += new System.EventHandler(this.SmallUIForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.CheckBox chkAllowRemoteConnection;

        /// <summary>
        /// Default page
        /// </summary>
        public System.Windows.Forms.TextBox txtDefaultPage;

        /// <summary>
        /// Port number
        /// </summary>
        public System.Windows.Forms.TextBox txtPortNumber;

        /// <summary>
        /// Physical path
        /// </summary>
        public System.Windows.Forms.TextBox txtPhysicalPath;

        /// <summary>
        /// Virtual path
        /// </summary>
        public System.Windows.Forms.TextBox txtVirtualPath;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkLogPageRequests;
    }
}