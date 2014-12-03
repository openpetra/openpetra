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
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listSites = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStopAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowRemoteConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startAutomaticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowAtStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutOpenPetraWebServerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStopNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.browseNotifyMenuItemPopup = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesNotifyMenuItemPopup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsNotifyMenuItemPopup = new System.Windows.Forms.ToolStripMenuItem();
            this.allowRemoteConnectionsNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startAutomaticallyNotifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowAtStartupNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpNotifyMenuItemPopup = new System.Windows.Forms.ToolStripMenuItem();
            this.helpNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.startTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesButton = new System.Windows.Forms.ToolStripButton();
            this.addButton = new System.Windows.Forms.ToolStripButton();
            this.removeButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.notifyIconMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // listSites
            //
            this.listSites.FormattingEnabled = true;
            this.listSites.Location = new System.Drawing.Point(12, 79);
            this.listSites.Name = "listSites";
            this.listSites.Size = new System.Drawing.Size(330, 82);
            this.listSites.TabIndex = 1;
            this.listSites.SelectedIndexChanged += new System.EventHandler(this.listSites_SelectedIndexChanged);
            this.listSites.DoubleClick += new System.EventHandler(this.listSites_DoubleClick);
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.sitesToolStripMenuItem,
                    this.helpToolStripMenuItem
                });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(354, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            //
            // sitesToolStripMenuItem
            //
            this.sitesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.startStopAllMenuItem,
                    this.selectedWebsiteToolStripMenuItem,
                    this.addSiteToolStripMenuItem,
                    this.toolStripMenuItem1,
                    this.viewLogToolStripMenuItem,
                    this.toolStripSeparator1,
                    this.optionsToolStripMenuItem
                });
            this.sitesToolStripMenuItem.Name = "sitesToolStripMenuItem";
            this.sitesToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.sitesToolStripMenuItem.Text = "&Sites";
            this.sitesToolStripMenuItem.DropDownOpening += new System.EventHandler(this.sitesToolStripMenuItem_DropDownOpening);
            //
            // startStopAllMenuItem
            //
            this.startStopAllMenuItem.Name = "startStopAllMenuItem";
            this.startStopAllMenuItem.ShortcutKeyDisplayString = "Shift+Ctrl+S";
            this.startStopAllMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) |
                                              System.Windows.Forms.Keys.S)));
            this.startStopAllMenuItem.Size = new System.Drawing.Size(187, 22);
            this.startStopAllMenuItem.Text = "&Start All";
            this.startStopAllMenuItem.Click += new System.EventHandler(this.startStopAllMenuItem_Click);
            //
            // selectedWebsiteToolStripMenuItem
            //
            this.selectedWebsiteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.removeToolStripMenuItem,
                    this.propertiesToolStripMenuItem
                });
            this.selectedWebsiteToolStripMenuItem.Name = "selectedWebsiteToolStripMenuItem";
            this.selectedWebsiteToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.selectedWebsiteToolStripMenuItem.Text = "Selected &Website";
            //
            // removeToolStripMenuItem
            //
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            //
            // propertiesToolStripMenuItem
            //
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F7";
            this.propertiesToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F7)));
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            //
            // addSiteToolStripMenuItem
            //
            this.addSiteToolStripMenuItem.Name = "addSiteToolStripMenuItem";
            this.addSiteToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.addSiteToolStripMenuItem.Text = "&Add Site ...";
            this.addSiteToolStripMenuItem.Click += new System.EventHandler(this.addSiteToolStripMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            //
            // viewLogToolStripMenuItem
            //
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.viewLogToolStripMenuItem.Text = "View &Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            //
            // optionsToolStripMenuItem
            //
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.allowRemoteConnectionsToolStripMenuItem,
                    this.startAutomaticallyToolStripMenuItem,
                    this.hideWindowAtStartupToolStripMenuItem
                });
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            //
            // allowRemoteConnectionsToolStripMenuItem
            //
            this.allowRemoteConnectionsToolStripMenuItem.Name = "allowRemoteConnectionsToolStripMenuItem";
            this.allowRemoteConnectionsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.allowRemoteConnectionsToolStripMenuItem.Text = "Allow &Remote Connections";
            this.allowRemoteConnectionsToolStripMenuItem.Click += new System.EventHandler(this.allowRemoteConnectionsToolStripMenuItem_Click);
            //
            // startAutomaticallyToolStripMenuItem
            //
            this.startAutomaticallyToolStripMenuItem.Checked = true;
            this.startAutomaticallyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startAutomaticallyToolStripMenuItem.Name = "startAutomaticallyToolStripMenuItem";
            this.startAutomaticallyToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.startAutomaticallyToolStripMenuItem.Text = "Start &Automatically";
            this.startAutomaticallyToolStripMenuItem.Click += new System.EventHandler(this.startAutomaticallyToolStripMenuItem_Click);
            //
            // hideWindowAtStartupToolStripMenuItem
            //
            this.hideWindowAtStartupToolStripMenuItem.Checked = true;
            this.hideWindowAtStartupToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideWindowAtStartupToolStripMenuItem.Name = "hideWindowAtStartupToolStripMenuItem";
            this.hideWindowAtStartupToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.hideWindowAtStartupToolStripMenuItem.Text = "&Hide Window at Startup";
            this.hideWindowAtStartupToolStripMenuItem.Click += new System.EventHandler(this.hideWindowAtStartupToolStripMenuItem_Click);
            //
            // helpToolStripMenuItem
            //
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.helpContentsToolStripMenuItem,
                    this.aboutOpenPetraWebServerMenuItem
                });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            //
            // helpContentsToolStripMenuItem
            //
            this.helpContentsToolStripMenuItem.Name = "helpContentsToolStripMenuItem";
            this.helpContentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpContentsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.helpContentsToolStripMenuItem.Text = "&Contents";
            this.helpContentsToolStripMenuItem.Click += new System.EventHandler(this.helpContentsToolStripMenuItem_Click);
            //
            // aboutOpenPetraWebServerMenuItem
            //
            this.aboutOpenPetraWebServerMenuItem.Name = "aboutOpenPetraWebServerMenuItem";
            this.aboutOpenPetraWebServerMenuItem.Size = new System.Drawing.Size(231, 22);
            this.aboutOpenPetraWebServerMenuItem.Text = "&About Open Petra Web Server";
            this.aboutOpenPetraWebServerMenuItem.Click += new System.EventHandler(this.aboutOpenPetraWebServerMenuItem_Click);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.toolStripStatusLabel
                });
            this.statusStrip1.Location = new System.Drawing.Point(0, 194);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(354, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            //
            // toolStripStatusLabel
            //
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel.Text = "Stopped";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Sites";
            //
            // notifyIcon
            //
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Right-mouse click to view the server status";
            this.notifyIcon.BalloonTipTitle = "Open Petra Web Server";
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Open Petra Web Server v1.0 - Stopped";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            //
            // notifyIconMenuStrip
            //
            this.notifyIconMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.openNotifyMenuItem,
                    this.closeNotifyMenuItem,
                    this.startStopNotifyMenuItem,
                    this.toolStripSeparator4,
                    this.browseNotifyMenuItemPopup,
                    this.propertiesNotifyMenuItemPopup,
                    this.toolStripSeparator2,
                    this.optionsNotifyMenuItemPopup,
                    this.helpNotifyMenuItemPopup,
                    this.toolStripSeparator5,
                    this.exitNotifyMenuItem
                });
            this.notifyIconMenuStrip.Name = "notifyIconMenuStrip";
            this.notifyIconMenuStrip.Size = new System.Drawing.Size(128, 198);
            this.notifyIconMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.notifyIconMenuStrip_Opening);
            //
            // openNotifyMenuItem
            //
            this.openNotifyMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openNotifyMenuItem.Name = "openNotifyMenuItem";
            this.openNotifyMenuItem.Size = new System.Drawing.Size(127, 22);
            this.openNotifyMenuItem.Text = "Open";
            this.openNotifyMenuItem.Click += new System.EventHandler(this.openNotifyMenuItem_Click);
            //
            // closeNotifyMenuItem
            //
            this.closeNotifyMenuItem.Name = "closeNotifyMenuItem";
            this.closeNotifyMenuItem.Size = new System.Drawing.Size(127, 22);
            this.closeNotifyMenuItem.Text = "&Close";
            this.closeNotifyMenuItem.Click += new System.EventHandler(this.closeNotifyMenuItem_Click);
            //
            // startStopNotifyMenuItem
            //
            this.startStopNotifyMenuItem.Name = "startStopNotifyMenuItem";
            this.startStopNotifyMenuItem.Size = new System.Drawing.Size(127, 22);
            this.startStopNotifyMenuItem.Text = "Start All";
            this.startStopNotifyMenuItem.Click += new System.EventHandler(this.startStopAllNotifyMenuItem_Click);
            //
            // toolStripSeparator4
            //
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(124, 6);
            //
            // browseNotifyMenuItemPopup
            //
            this.browseNotifyMenuItemPopup.Name = "browseNotifyMenuItemPopup";
            this.browseNotifyMenuItemPopup.Size = new System.Drawing.Size(127, 22);
            this.browseNotifyMenuItemPopup.Text = "&Browse";
            //
            // propertiesNotifyMenuItemPopup
            //
            this.propertiesNotifyMenuItemPopup.Name = "propertiesNotifyMenuItemPopup";
            this.propertiesNotifyMenuItemPopup.Size = new System.Drawing.Size(127, 22);
            this.propertiesNotifyMenuItemPopup.Text = "&Properties";
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(124, 6);
            //
            // optionsNotifyMenuItemPopup
            //
            this.optionsNotifyMenuItemPopup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.allowRemoteConnectionsNotifyMenuItem,
                    this.startAutomaticallyNotifyStripMenuItem,
                    this.hideWindowAtStartupNotifyMenuItem
                });
            this.optionsNotifyMenuItemPopup.Name = "optionsNotifyMenuItemPopup";
            this.optionsNotifyMenuItemPopup.Size = new System.Drawing.Size(127, 22);
            this.optionsNotifyMenuItemPopup.Text = "&Options";
            //
            // allowRemoteConnectionsNotifyMenuItem
            //
            this.allowRemoteConnectionsNotifyMenuItem.Name = "allowRemoteConnectionsNotifyMenuItem";
            this.allowRemoteConnectionsNotifyMenuItem.Size = new System.Drawing.Size(218, 22);
            this.allowRemoteConnectionsNotifyMenuItem.Text = "Allow &Remote Connections";
            this.allowRemoteConnectionsNotifyMenuItem.Click += new System.EventHandler(this.allowRemoteConnectionsNotifyMenuItem_Click);
            //
            // startAutomaticallyNotifyStripMenuItem
            //
            this.startAutomaticallyNotifyStripMenuItem.Checked = true;
            this.startAutomaticallyNotifyStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startAutomaticallyNotifyStripMenuItem.Name = "startAutomaticallyNotifyStripMenuItem";
            this.startAutomaticallyNotifyStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.startAutomaticallyNotifyStripMenuItem.Text = "Start &Automatically";
            this.startAutomaticallyNotifyStripMenuItem.Click += new System.EventHandler(this.startAutomaticallyNotifyStripMenuItem_Click);
            //
            // hideWindowAtStartupNotifyMenuItem
            //
            this.hideWindowAtStartupNotifyMenuItem.Checked = true;
            this.hideWindowAtStartupNotifyMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideWindowAtStartupNotifyMenuItem.Name = "hideWindowAtStartupNotifyMenuItem";
            this.hideWindowAtStartupNotifyMenuItem.Size = new System.Drawing.Size(218, 22);
            this.hideWindowAtStartupNotifyMenuItem.Text = "&Hide Window at Startup";
            this.hideWindowAtStartupNotifyMenuItem.Click += new System.EventHandler(this.hideWindowAtStartupNotifyMenuItem_Click);
            //
            // helpNotifyMenuItemPopup
            //
            this.helpNotifyMenuItemPopup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.helpNotifyMenuItem,
                    this.aboutNotifyMenuItem
                });
            this.helpNotifyMenuItemPopup.Name = "helpNotifyMenuItemPopup";
            this.helpNotifyMenuItemPopup.Size = new System.Drawing.Size(127, 22);
            this.helpNotifyMenuItemPopup.Text = "&Help";
            //
            // helpNotifyMenuItem
            //
            this.helpNotifyMenuItem.Name = "helpNotifyMenuItem";
            this.helpNotifyMenuItem.Size = new System.Drawing.Size(122, 22);
            this.helpNotifyMenuItem.Text = "Contents";
            this.helpNotifyMenuItem.Click += new System.EventHandler(this.helpNotifyMenuItem_Click);
            //
            // aboutNotifyMenuItem
            //
            this.aboutNotifyMenuItem.Name = "aboutNotifyMenuItem";
            this.aboutNotifyMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutNotifyMenuItem.Text = "About ...";
            this.aboutNotifyMenuItem.Click += new System.EventHandler(this.aboutNotifyMenuItem_Click);
            //
            // toolStripSeparator5
            //
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(124, 6);
            //
            // exitNotifyMenuItem
            //
            this.exitNotifyMenuItem.Name = "exitNotifyMenuItem";
            this.exitNotifyMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitNotifyMenuItem.Text = "Exit";
            this.exitNotifyMenuItem.Click += new System.EventHandler(this.exitNotifyMenuItem_Click);
            //
            // linkLabel
            //
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(12, 168);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(49, 13);
            this.linkLabel.TabIndex = 2;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "linkLabel";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            //
            // startTimer
            //
            this.startTimer.Interval = 5000;
            this.startTimer.Tick += new System.EventHandler(this.startTimer_Tick);
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.startButton,
                    this.stopButton,
                    this.toolStripSeparator6,
                    this.propertiesButton,
                    this.addButton,
                    this.removeButton
                });
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(354, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            //
            // startButton
            //
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(23, 22);
            this.startButton.Text = "Start Server";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // stopButton
            //
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopButton.Enabled = false;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(23, 22);
            this.stopButton.Text = "Stop Server";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //
            // toolStripSeparator6
            //
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            //
            // propertiesButton
            //
            this.propertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.propertiesButton.Image = ((System.Drawing.Image)(resources.GetObject("propertiesButton.Image")));
            this.propertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Size = new System.Drawing.Size(23, 22);
            this.propertiesButton.Text = "Web site properties";
            this.propertiesButton.Click += new System.EventHandler(this.propertiesButton_Click);
            //
            // addButton
            //
            this.addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
            this.addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 22);
            this.addButton.Text = "Add new site";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            //
            // removeButton
            //
            this.removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.Image")));
            this.removeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 22);
            this.removeButton.Text = "Remove selected site";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 216);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listSites);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Open Petra Web Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.notifyIconMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listSites;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sitesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startStopAllMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutOpenPetraWebServerMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openNotifyMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitNotifyMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.ToolStripMenuItem startStopNotifyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeNotifyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowRemoteConnectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startAutomaticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsNotifyMenuItemPopup;
        private System.Windows.Forms.ToolStripMenuItem startAutomaticallyNotifyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowRemoteConnectionsNotifyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideWindowAtStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideWindowAtStartupNotifyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseNotifyMenuItemPopup;
        private System.Windows.Forms.ToolStripMenuItem helpContentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesNotifyMenuItemPopup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem helpNotifyMenuItemPopup;
        private System.Windows.Forms.ToolStripMenuItem helpNotifyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutNotifyMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Timer startTimer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton addButton;
        private System.Windows.Forms.ToolStripButton propertiesButton;
        private System.Windows.Forms.ToolStripButton removeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
    }
}