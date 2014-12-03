//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
namespace Ict.Tools.OPWebServerManager
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
            this.btnFindServers = new System.Windows.Forms.Button();
            this.lblResponse = new System.Windows.Forms.Label();
            this.responseTimer = new System.Windows.Forms.Timer(this.components);
            this.listWebServers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.shutdownAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.startContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressResponses = new System.Windows.Forms.ProgressBar();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownAllMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // btnFindServers
            //
            this.btnFindServers.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindServers.Location = new System.Drawing.Point(534, 33);
            this.btnFindServers.Name = "btnFindServers";
            this.btnFindServers.Size = new System.Drawing.Size(75, 23);
            this.btnFindServers.TabIndex = 0;
            this.btnFindServers.Text = "Find Servers";
            this.btnFindServers.UseVisualStyleBackColor = true;
            this.btnFindServers.Click += new System.EventHandler(this.btnFindServers_Click);
            //
            // lblResponse
            //
            this.lblResponse.Location = new System.Drawing.Point(12, 177);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(546, 86);
            this.lblResponse.TabIndex = 1;
            this.lblResponse.Text = "Response";
            //
            // responseTimer
            //
            this.responseTimer.Interval = 200;
            this.responseTimer.Tick += new System.EventHandler(this.responseTimer_Tick);
            //
            // listWebServers
            //
            this.listWebServers.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.listWebServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                    this.columnHeader1,
                    this.columnHeader2,
                    this.columnHeader3
                });
            this.listWebServers.ContextMenuStrip = this.contextMenuStrip;
            this.listWebServers.FullRowSelect = true;
            this.listWebServers.HideSelection = false;
            this.listWebServers.Location = new System.Drawing.Point(12, 67);
            this.listWebServers.MultiSelect = false;
            this.listWebServers.Name = "listWebServers";
            this.listWebServers.Size = new System.Drawing.Size(597, 107);
            this.listWebServers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listWebServers.TabIndex = 2;
            this.listWebServers.UseCompatibleStateImageBehavior = false;
            this.listWebServers.View = System.Windows.Forms.View.Details;
            this.listWebServers.Visible = false;
            this.listWebServers.SelectedIndexChanged += new System.EventHandler(this.listWebServers_SelectedIndexChanged);
            //
            // columnHeader1
            //
            this.columnHeader1.Text = "Port";
            //
            // columnHeader2
            //
            this.columnHeader2.Text = "Status";
            //
            // columnHeader3
            //
            this.columnHeader3.Text = "Physical Path";
            this.columnHeader3.Width = 465;
            //
            // contextMenuStrip
            //
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.shutdownAllContextMenuItem,
                    this.toolStripMenuItem2,
                    this.startContextMenuItem,
                    this.stopContextMenuItem,
                    this.shutdownContextMenuItem
                });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(146, 98);
            //
            // shutdownAllContextMenuItem
            //
            this.shutdownAllContextMenuItem.Name = "shutdownAllContextMenuItem";
            this.shutdownAllContextMenuItem.Size = new System.Drawing.Size(145, 22);
            this.shutdownAllContextMenuItem.Text = "Shutdown All";
            this.shutdownAllContextMenuItem.Click += new System.EventHandler(this.shutdownAllContextMenuItem_Click);
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(142, 6);
            //
            // startContextMenuItem
            //
            this.startContextMenuItem.Name = "startContextMenuItem";
            this.startContextMenuItem.Size = new System.Drawing.Size(145, 22);
            this.startContextMenuItem.Text = "Start";
            this.startContextMenuItem.Click += new System.EventHandler(this.startContextMenuItem_Click);
            //
            // stopContextMenuItem
            //
            this.stopContextMenuItem.Name = "stopContextMenuItem";
            this.stopContextMenuItem.Size = new System.Drawing.Size(145, 22);
            this.stopContextMenuItem.Text = "Stop";
            this.stopContextMenuItem.Click += new System.EventHandler(this.stopContextMenuItem_Click);
            //
            // shutdownContextMenuItem
            //
            this.shutdownContextMenuItem.Name = "shutdownContextMenuItem";
            this.shutdownContextMenuItem.Size = new System.Drawing.Size(145, 22);
            this.shutdownContextMenuItem.Text = "Shutdown";
            this.shutdownContextMenuItem.Click += new System.EventHandler(this.shutdownContextMenuItem_Click);
            //
            // progressResponses
            //
            this.progressResponses.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.progressResponses.Location = new System.Drawing.Point(12, 43);
            this.progressResponses.Maximum = 2500;
            this.progressResponses.Name = "progressResponses";
            this.progressResponses.Size = new System.Drawing.Size(491, 13);
            this.progressResponses.TabIndex = 3;
            //
            // mainMenu
            //
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.fileToolStripMenuItem,
                    this.actionToolStripMenuItem
                });
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(621, 24);
            this.mainMenu.TabIndex = 5;
            this.mainMenu.Text = "menuStrip1";
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.exitToolStripMenuItem
                });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            //
            // actionToolStripMenuItem
            //
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.shutdownAllMainMenuItem,
                    this.startMainMenuItem,
                    this.stopMainMenuItem,
                    this.shutdownMainMenuItem
                });
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.actionToolStripMenuItem.Text = "Action";
            //
            // shutdownAllMainMenuItem
            //
            this.shutdownAllMainMenuItem.Name = "shutdownAllMainMenuItem";
            this.shutdownAllMainMenuItem.Size = new System.Drawing.Size(145, 22);
            this.shutdownAllMainMenuItem.Text = "Shutdown All";
            this.shutdownAllMainMenuItem.Click += new System.EventHandler(this.shutdownAllMainMenuItem_Click);
            //
            // startMainMenuItem
            //
            this.startMainMenuItem.Name = "startMainMenuItem";
            this.startMainMenuItem.Size = new System.Drawing.Size(145, 22);
            this.startMainMenuItem.Text = "Start";
            this.startMainMenuItem.Click += new System.EventHandler(this.startMainMenuItem_Click);
            //
            // stopMainMenuItem
            //
            this.stopMainMenuItem.Name = "stopMainMenuItem";
            this.stopMainMenuItem.Size = new System.Drawing.Size(145, 22);
            this.stopMainMenuItem.Text = "Stop";
            this.stopMainMenuItem.Click += new System.EventHandler(this.stopMainMenuItem_Click);
            //
            // shutdownMainMenuItem
            //
            this.shutdownMainMenuItem.Name = "shutdownMainMenuItem";
            this.shutdownMainMenuItem.Size = new System.Drawing.Size(145, 22);
            this.shutdownMainMenuItem.Text = "Shutdown";
            this.shutdownMainMenuItem.Click += new System.EventHandler(this.shutdownMainMenuItem_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 193);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.progressResponses);
            this.Controls.Add(this.listWebServers);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.btnFindServers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Open Petra Web Server Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnFindServers;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Timer responseTimer;
        private System.Windows.Forms.ListView listWebServers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ProgressBar progressResponses;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem shutdownAllContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem startContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownContextMenuItem;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownAllMainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownMainMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}