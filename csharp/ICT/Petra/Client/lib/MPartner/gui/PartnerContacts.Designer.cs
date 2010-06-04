// auto generated with nant generateWinforms from PartnerContacts.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmPartnerContacts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerContacts));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpContactDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblContactDate = new System.Windows.Forms.Label();
            this.txtContactor = new System.Windows.Forms.TextBox();
            this.lblContactor = new System.Windows.Forms.Label();
            this.txtCommentContains = new System.Windows.Forms.TextBox();
            this.lblCommentContains = new System.Windows.Forms.Label();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.txtMethodOfContact = new System.Windows.Forms.TextBox();
            this.lblMethodOfContact = new System.Windows.Forms.Label();
            this.txtMailingCode = new System.Windows.Forms.TextBox();
            this.lblMailingCode = new System.Windows.Forms.Label();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSearch = new System.Windows.Forms.ToolStripButton();
            this.tbbDeleteContacts = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlParameters.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlParameters
            //
            this.pnlParameters.Location = new System.Drawing.Point(2,2);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlParameters.Controls.Add(this.tableLayoutPanel2);
            //
            // dtpContactDate
            //
            this.dtpContactDate.Location = new System.Drawing.Point(2,2);
            this.dtpContactDate.Name = "dtpContactDate";
            this.dtpContactDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblContactDate
            //
            this.lblContactDate.Location = new System.Drawing.Point(2,2);
            this.lblContactDate.Name = "lblContactDate";
            this.lblContactDate.AutoSize = true;
            this.lblContactDate.Text = "Contact Date:";
            this.lblContactDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblContactDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblContactDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtContactor
            //
            this.txtContactor.Location = new System.Drawing.Point(2,2);
            this.txtContactor.Name = "txtContactor";
            this.txtContactor.Size = new System.Drawing.Size(150, 28);
            //
            // lblContactor
            //
            this.lblContactor.Location = new System.Drawing.Point(2,2);
            this.lblContactor.Name = "lblContactor";
            this.lblContactor.AutoSize = true;
            this.lblContactor.Text = "Contactor:";
            this.lblContactor.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblContactor.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblContactor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtCommentContains
            //
            this.txtCommentContains.Location = new System.Drawing.Point(2,2);
            this.txtCommentContains.Name = "txtCommentContains";
            this.txtCommentContains.Size = new System.Drawing.Size(150, 28);
            //
            // lblCommentContains
            //
            this.lblCommentContains.Location = new System.Drawing.Point(2,2);
            this.lblCommentContains.Name = "lblCommentContains";
            this.lblCommentContains.AutoSize = true;
            this.lblCommentContains.Text = "Comment Contains:";
            this.lblCommentContains.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCommentContains.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCommentContains.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtModule
            //
            this.txtModule.Location = new System.Drawing.Point(2,2);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(150, 28);
            //
            // lblModule
            //
            this.lblModule.Location = new System.Drawing.Point(2,2);
            this.lblModule.Name = "lblModule";
            this.lblModule.AutoSize = true;
            this.lblModule.Text = "Module:";
            this.lblModule.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblModule.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblModule.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtMethodOfContact
            //
            this.txtMethodOfContact.Location = new System.Drawing.Point(2,2);
            this.txtMethodOfContact.Name = "txtMethodOfContact";
            this.txtMethodOfContact.Size = new System.Drawing.Size(150, 28);
            //
            // lblMethodOfContact
            //
            this.lblMethodOfContact.Location = new System.Drawing.Point(2,2);
            this.lblMethodOfContact.Name = "lblMethodOfContact";
            this.lblMethodOfContact.AutoSize = true;
            this.lblMethodOfContact.Text = "Method Of Contact:";
            this.lblMethodOfContact.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMethodOfContact.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMethodOfContact.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtMailingCode
            //
            this.txtMailingCode.Location = new System.Drawing.Point(2,2);
            this.txtMailingCode.Name = "txtMailingCode";
            this.txtMailingCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblMailingCode
            //
            this.lblMailingCode.Location = new System.Drawing.Point(2,2);
            this.lblMailingCode.Name = "lblMailingCode";
            this.lblMailingCode.AutoSize = true;
            this.lblMailingCode.Text = "Mailing Code:";
            this.lblMailingCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMailingCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMailingCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblContactDate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblContactor, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblCommentContains, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblModule, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblMethodOfContact, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.dtpContactDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtContactor, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtCommentContains, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtModule, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtMethodOfContact, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblMailingCode, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtMailingCode, 3, 4);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlParameters, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdDetails, 0, 1);
            //
            // tbbSearch
            //
            this.tbbSearch.Name = "tbbSearch";
            this.tbbSearch.AutoSize = true;
            this.tbbSearch.Click += new System.EventHandler(this.Search);
            this.tbbSearch.Text = "Search";
            //
            // tbbDeleteContacts
            //
            this.tbbDeleteContacts.Name = "tbbDeleteContacts";
            this.tbbDeleteContacts.AutoSize = true;
            this.tbbDeleteContacts.Click += new System.EventHandler(this.DeleteContacts);
            this.tbbDeleteContacts.Text = "Delete Contacts";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSearch,
                        tbbDeleteContacts});
            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.AutoSize = true;
            this.mniClose.Click += new System.EventHandler(this.actClose);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniClose});
            this.mniFile.Text = "&File";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniHelpAboutPetra
            //
            this.mniHelpAboutPetra.Name = "mniHelpAboutPetra";
            this.mniHelpAboutPetra.AutoSize = true;
            this.mniHelpAboutPetra.Text = "&About Petra";
            //
            // mniHelpDevelopmentTeam
            //
            this.mniHelpDevelopmentTeam.Name = "mniHelpDevelopmentTeam";
            this.mniHelpDevelopmentTeam.AutoSize = true;
            this.mniHelpDevelopmentTeam.Text = "&The Development Team...";
            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.AutoSize = true;
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniHelpPetraHelp,
                        mniSeparator0,
                        mniHelpBugReport,
                        mniSeparator1,
                        mniHelpAboutPetra,
                        mniHelpDevelopmentTeam});
            this.mniHelp.Text = "&Help";
            //
            // mnuMain
            //
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuMain.AutoSize = true;
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFile,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmPartnerContacts
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmPartnerContacts";
            this.Text = "Contacts with Partners";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlParameters.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlParameters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpContactDate;
        private System.Windows.Forms.Label lblContactDate;
        private System.Windows.Forms.TextBox txtContactor;
        private System.Windows.Forms.Label lblContactor;
        private System.Windows.Forms.TextBox txtCommentContains;
        private System.Windows.Forms.Label lblCommentContains;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.TextBox txtMethodOfContact;
        private System.Windows.Forms.Label lblMethodOfContact;
        private System.Windows.Forms.TextBox txtMailingCode;
        private System.Windows.Forms.Label lblMailingCode;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSearch;
        private System.Windows.Forms.ToolStripButton tbbDeleteContacts;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
