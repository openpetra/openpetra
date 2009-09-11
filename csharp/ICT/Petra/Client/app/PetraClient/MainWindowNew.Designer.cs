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
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.sptNavigation = new System.Windows.Forms.SplitContainer();
        	this.pnlMoreButtons = new System.Windows.Forms.Panel();
        	this.pnlNavigationCaption = new System.Windows.Forms.Panel();
        	this.lblNavigationCaption = new System.Windows.Forms.Label();
        	this.btnCollapseNavigation = new System.Windows.Forms.Button();
            this.rbtMyPetra = new System.Windows.Forms.RadioButton();
            this.rbtPartner = new System.Windows.Forms.RadioButton();
            this.rbtFinance = new System.Windows.Forms.RadioButton();
            this.rbtPersonnel = new System.Windows.Forms.RadioButton();
            this.rbtConferenceManagement = new System.Windows.Forms.RadioButton();
            this.rbtFinancialDevelopment = new System.Windows.Forms.RadioButton();
            this.rbtSystemManager = new System.Windows.Forms.RadioButton();

        	this.pnlNavigation.SuspendLayout();
        	this.sptNavigation.Panel1.SuspendLayout();
        	this.sptNavigation.Panel2.SuspendLayout();
        	this.sptNavigation.SuspendLayout();
        	this.SuspendLayout();

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
            this.sptNavigation.Panel2.Controls.Add(this.rbtMyPetra);
            this.sptNavigation.Panel2.Controls.Add(this.rbtPartner);
            this.sptNavigation.Panel2.Controls.Add(this.rbtFinance);
            this.sptNavigation.Panel2.Controls.Add(this.rbtPersonnel);
            this.sptNavigation.Panel2.Controls.Add(this.rbtConferenceManagement);
            this.sptNavigation.Panel2.Controls.Add(this.rbtFinancialDevelopment);
            this.sptNavigation.Panel2.Controls.Add(this.rbtSystemManager);
        	this.sptNavigation.Size = new System.Drawing.Size(200, 396);
        	this.sptNavigation.SplitterDistance = 210;
        	this.sptNavigation.TabIndex = 6;
        	//
        	// imageListButtons
        	//
//        	this.imageListButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtons.ImageStream")));
//        	this.imageListButtons.TransparentColor = System.Drawing.Color.Transparent;
            //
            // rbtMyPetra
            //
            this.rbtMyPetra.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtMyPetra.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtMyPetra.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtMyPetra.ImageKey = "Petra.ico";
            //this.rbtMyPetra.ImageList = this.imageListButtons;
            this.rbtMyPetra.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtMyPetra.Name = "rbtMyPetra";
            this.rbtMyPetra.Text = "My Petra";
            this.rbtMyPetra.Size = new System.Drawing.Size(200, 24);
            //
            // rbtPartner
            //
            this.rbtPartner.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtPartner.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtPartner.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtPartner.ImageKey = "";
            //this.rbtPartner.ImageList = this.imageListButtons;
            this.rbtPartner.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtPartner.Name = "rbtPartner";
            this.rbtPartner.Text = "Partner";
            this.rbtPartner.Size = new System.Drawing.Size(200, 24);
            //
            // rbtFinance
            //
            this.rbtFinance.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtFinance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtFinance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtFinance.ImageKey = "";
            //this.rbtFinance.ImageList = this.imageListButtons;
            this.rbtFinance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtFinance.Name = "rbtFinance";
            this.rbtFinance.Text = "Finance";
            this.rbtFinance.Size = new System.Drawing.Size(200, 24);
            //
            // rbtPersonnel
            //
            this.rbtPersonnel.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtPersonnel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtPersonnel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtPersonnel.ImageKey = "";
            //this.rbtPersonnel.ImageList = this.imageListButtons;
            this.rbtPersonnel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtPersonnel.Name = "rbtPersonnel";
            this.rbtPersonnel.Text = "Personnel";
            this.rbtPersonnel.Size = new System.Drawing.Size(200, 24);
            //
            // rbtConferenceManagement
            //
            this.rbtConferenceManagement.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtConferenceManagement.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtConferenceManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtConferenceManagement.ImageKey = "";
            //this.rbtConferenceManagement.ImageList = this.imageListButtons;
            this.rbtConferenceManagement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtConferenceManagement.Name = "rbtConferenceManagement";
            this.rbtConferenceManagement.Text = "ConferenceManagement";
            this.rbtConferenceManagement.Size = new System.Drawing.Size(200, 24);
            //
            // rbtFinancialDevelopment
            //
            this.rbtFinancialDevelopment.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtFinancialDevelopment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtFinancialDevelopment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtFinancialDevelopment.ImageKey = "";
            //this.rbtFinancialDevelopment.ImageList = this.imageListButtons;
            this.rbtFinancialDevelopment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtFinancialDevelopment.Name = "rbtFinancialDevelopment";
            this.rbtFinancialDevelopment.Text = "FinancialDevelopment";
            this.rbtFinancialDevelopment.Size = new System.Drawing.Size(200, 24);
            //
            // rbtSystemManager
            //
            this.rbtSystemManager.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtSystemManager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbtSystemManager.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtSystemManager.ImageKey = "";
            //this.rbtSystemManager.ImageList = this.imageListButtons;
            this.rbtSystemManager.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtSystemManager.Name = "rbtSystemManager";
            this.rbtSystemManager.Text = "SystemManager";
            this.rbtSystemManager.Size = new System.Drawing.Size(200, 24);

            //
            // TFrmMainWindowNew
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
        	this.Controls.Add(this.pnlNavigation);
            this.Name = "TFrmMainWindowNew";
            this.Text = "";

        	this.pnlNavigation.ResumeLayout(false);
        	this.sptNavigation.Panel1.ResumeLayout(false);
        	this.sptNavigation.Panel2.ResumeLayout(false);
        	this.sptNavigation.ResumeLayout(false);
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
        private System.Windows.Forms.RadioButton rbtMyPetra;
        private System.Windows.Forms.RadioButton rbtPartner;
        private System.Windows.Forms.RadioButton rbtFinance;
        private System.Windows.Forms.RadioButton rbtPersonnel;
        private System.Windows.Forms.RadioButton rbtConferenceManagement;
        private System.Windows.Forms.RadioButton rbtFinancialDevelopment;
        private System.Windows.Forms.RadioButton rbtSystemManager;
    }
}