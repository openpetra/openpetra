/* auto generated with nant generateWinforms from MainWindowNew.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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
using System.Windows.Forms;

namespace Ict.Petra.Client.App.PetraClient
{
    partial class TFrmMainWindowNew
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
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmMainWindowNew));

//        	this.imageListButtons = new System.Windows.Forms.ImageList(this.components);
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.sptNavigation = new System.Windows.Forms.SplitContainer();
        	this.pnlMoreButtons = new System.Windows.Forms.Panel();
        	this.pnlNavigationCaption = new System.Windows.Forms.Panel();
        	this.lblNavigationCaption = new System.Windows.Forms.Label();
        	this.btnCollapseNavigation = new System.Windows.Forms.Button();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.stbMain.SuspendLayout();
            this.pnlContent.SuspendLayout();
        	this.pnlNavigation.SuspendLayout();
        	this.sptNavigation.Panel1.SuspendLayout();
        	this.sptNavigation.Panel2.SuspendLayout();
        	this.sptNavigation.SuspendLayout();
        	this.SuspendLayout();

            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;
        	//
        	// lblNavigationCaption
        	//
        	this.lblNavigationCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblNavigationCaption.ForeColor = System.Drawing.Color.Blue;
        	this.lblNavigationCaption.Location = new System.Drawing.Point(3, 9);
        	this.lblNavigationCaption.Name = "lblNavigationCaption";
        	this.lblNavigationCaption.Size = new System.Drawing.Size(153, 23);
        	this.lblNavigationCaption.TabIndex = 0;
        	this.lblNavigationCaption.Text = "Caption";
        	//
        	// btnCollapseNavigation
        	//
        	this.btnCollapseNavigation.Dock = System.Windows.Forms.DockStyle.Right;
        	this.btnCollapseNavigation.Location = new System.Drawing.Point(154, 0);
        	this.btnCollapseNavigation.Name = "btnCollapseNavigation";
        	this.btnCollapseNavigation.Size = new System.Drawing.Size(46, 42);
        	this.btnCollapseNavigation.TabIndex = 1;
        	this.btnCollapseNavigation.Text = "<=";
        	this.btnCollapseNavigation.UseVisualStyleBackColor = true;
        	//
        	// pnlMoreButtons
        	//
        	this.pnlMoreButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.pnlMoreButtons.Location = new System.Drawing.Point(0, 438);
        	this.pnlMoreButtons.Name = "pnlMoreButtons";
        	this.pnlMoreButtons.Size = new System.Drawing.Size(200, 28);
        	this.pnlMoreButtons.TabIndex = 2;
        	//
        	// pnlNavigationCaption
        	//
        	this.pnlNavigationCaption.Dock = System.Windows.Forms.DockStyle.Top;
        	this.pnlNavigationCaption.Location = new System.Drawing.Point(0, 0);
        	this.pnlNavigationCaption.Name = "pnlNavigationCaption";
        	this.pnlNavigationCaption.Size = new System.Drawing.Size(200, 42);
        	this.pnlNavigationCaption.TabIndex = 7;
        	this.pnlNavigationCaption.Controls.Add(this.btnCollapseNavigation);
        	this.pnlNavigationCaption.Controls.Add(this.lblNavigationCaption);
        	//
        	// pnlNavigation
        	//
        	this.pnlNavigation.Controls.Add(this.sptNavigation);
        	this.pnlNavigation.Controls.Add(this.pnlMoreButtons);
        	this.pnlNavigation.Controls.Add(this.pnlNavigationCaption);
        	this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
        	this.pnlNavigation.Location = new System.Drawing.Point(0, 0);
        	this.pnlNavigation.Name = "pnlNavigation";
        	this.pnlNavigation.Size = new System.Drawing.Size(200, 466);
        	this.pnlNavigation.TabIndex = 0;
        	//
        	// pnlContent
        	//
        	this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.pnlContent.Name = "pnlContent";
        	//
        	// sptNavigation
        	//
        	this.sptNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.sptNavigation.Location = new System.Drawing.Point(0, 42);
        	this.sptNavigation.Name = "sptNavigation";
        	this.sptNavigation.Orientation = System.Windows.Forms.Orientation.Horizontal;
        	//
        	// sptNavigation.Panel1
        	//
        	this.sptNavigation.Panel1.AutoScroll = true;
        	//
        	// sptNavigation.Panel2
        	//
        	this.sptNavigation.Size = new System.Drawing.Size(200, 396);
        	this.sptNavigation.SplitterDistance = 210;
        	this.sptNavigation.TabIndex = 6;
        	//
        	// imageListButtons
        	//
//        	this.imageListButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtons.ImageStream")));
//        	this.imageListButtons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // TFrmMainWindowNew
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(pnlContent);
        	this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmMainWindowNew";
            this.Text = "";

            this.stbMain.ResumeLayout(false);
        	this.pnlNavigation.ResumeLayout(false);
        	this.sptNavigation.Panel1.ResumeLayout(false);
        	this.sptNavigation.Panel2.ResumeLayout(false);
        	this.sptNavigation.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
//        private System.Windows.Forms.ImageList imageListButtons;
        private System.Windows.Forms.Label lblNavigationCaption;
        private System.Windows.Forms.Button btnCollapseNavigation;
        private System.Windows.Forms.SplitContainer sptNavigation;
        private System.Windows.Forms.Panel pnlNavigationCaption;
        private System.Windows.Forms.Panel pnlMoreButtons;
        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Panel pnlContent;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}