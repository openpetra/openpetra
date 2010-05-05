/* auto generated with nant generateWinforms from GiftReceiptPrinting.yaml
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
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    partial class TFrmGiftReceiptPrinting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGiftReceiptPrinting));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpStartDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.preLetters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tbrLetters = new System.Windows.Forms.ToolStrip();
            this.ttxCurrentPage = new System.Windows.Forms.ToolStripTextBox();
            this.tblTotalNumberPages = new System.Windows.Forms.ToolStripLabel();
            this.tbbPrevPage = new System.Windows.Forms.ToolStripButton();
            this.tbbNextPage = new System.Windows.Forms.ToolStripButton();
            this.tbbPrintCurrentPage = new System.Windows.Forms.ToolStripButton();
            this.tbbPrint = new System.Windows.Forms.ToolStripButton();
            this.ppvLetters = new System.Windows.Forms.PrintPreviewControl();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbGenerateLetters = new System.Windows.Forms.ToolStripButton();
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
            this.preLetters.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tbrLetters.SuspendLayout();
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
            // dtpStartDate
            //
            this.dtpStartDate.Location = new System.Drawing.Point(2,2);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblStartDate
            //
            this.lblStartDate.Location = new System.Drawing.Point(2,2);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpEndDate
            //
            this.dtpEndDate.Location = new System.Drawing.Point(2,2);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblEndDate
            //
            this.lblEndDate.Location = new System.Drawing.Point(2,2);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblEndDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblStartDate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpStartDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblEndDate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpEndDate, 3, 0);
            //
            // preLetters
            //
            this.preLetters.Name = "preLetters";
            this.preLetters.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.preLetters.Controls.Add(this.tableLayoutPanel3);
            //
            // ttxCurrentPage
            //
            this.ttxCurrentPage.Name = "ttxCurrentPage";
            this.ttxCurrentPage.AutoSize = true;
            this.ttxCurrentPage.TextChanged += new System.EventHandler(this.CurrentPageTextChanged);
            //
            // tblTotalNumberPages
            //
            this.tblTotalNumberPages.Name = "tblTotalNumberPages";
            this.tblTotalNumberPages.AutoSize = true;
            this.tblTotalNumberPages.Text = "Total Number Pages";
            //
            // tbbPrevPage
            //
            this.tbbPrevPage.Name = "tbbPrevPage";
            this.tbbPrevPage.AutoSize = true;
            this.tbbPrevPage.Click += new System.EventHandler(this.PrevPageClick);
            this.tbbPrevPage.Text = "Prev Page";
            //
            // tbbNextPage
            //
            this.tbbNextPage.Name = "tbbNextPage";
            this.tbbNextPage.AutoSize = true;
            this.tbbNextPage.Click += new System.EventHandler(this.NextPageClick);
            this.tbbNextPage.Text = "Next Page";
            //
            // tbbPrintCurrentPage
            //
            this.tbbPrintCurrentPage.Name = "tbbPrintCurrentPage";
            this.tbbPrintCurrentPage.AutoSize = true;
            this.tbbPrintCurrentPage.Click += new System.EventHandler(this.PrintCurrentPage);
            this.tbbPrintCurrentPage.Text = "Print Current Page";
            //
            // tbbPrint
            //
            this.tbbPrint.Name = "tbbPrint";
            this.tbbPrint.AutoSize = true;
            this.tbbPrint.Click += new System.EventHandler(this.PrintAllPages);
            this.tbbPrint.Text = "Print";
            //
            // tbrLetters
            //
            this.tbrLetters.Name = "tbrLetters";
            this.tbrLetters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrLetters.AutoSize = true;
            this.tbrLetters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           ttxCurrentPage,
                        tblTotalNumberPages,
                        tbbPrevPage,
                        tbbNextPage,
                        tbbPrintCurrentPage,
                        tbbPrint});
            //
            // ppvLetters
            //
            this.ppvLetters.Name = "ppvLetters";
            this.ppvLetters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.tbrLetters, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.ppvLetters, 0, 1);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlParameters, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.preLetters, 0, 1);
            //
            // tbbGenerateLetters
            //
            this.tbbGenerateLetters.Name = "tbbGenerateLetters";
            this.tbbGenerateLetters.AutoSize = true;
            this.tbbGenerateLetters.Click += new System.EventHandler(this.GenerateLetters);
            this.tbbGenerateLetters.Text = "&Generate Letters";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbGenerateLetters});
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
            // TFrmGiftReceiptPrinting
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(754, 623);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmGiftReceiptPrinting";
            this.Text = "Print Annual Gift Receipts";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tbrLetters.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.preLetters.ResumeLayout(false);
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
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.GroupBox preLetters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStrip tbrLetters;
        private System.Windows.Forms.ToolStripTextBox ttxCurrentPage;
        private System.Windows.Forms.ToolStripLabel tblTotalNumberPages;
        private System.Windows.Forms.ToolStripButton tbbPrevPage;
        private System.Windows.Forms.ToolStripButton tbbNextPage;
        private System.Windows.Forms.ToolStripButton tbbPrintCurrentPage;
        private System.Windows.Forms.ToolStripButton tbbPrint;
        private System.Windows.Forms.PrintPreviewControl ppvLetters;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbGenerateLetters;
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
