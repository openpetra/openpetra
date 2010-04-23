/* auto generated with nant generateWinforms from GLCreateLedger.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    partial class TFrmGLCreateLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGLCreateLedger));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nudLedgerNumber = new System.Windows.Forms.NumericUpDown();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.txtLedgerName = new System.Windows.Forms.TextBox();
            this.lblLedgerName = new System.Windows.Forms.Label();
            this.cmbCountryCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCountryCode = new System.Windows.Forms.Label();
            this.cmbBaseCurrency = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblBaseCurrency = new System.Windows.Forms.Label();
            this.cmbIntlCurrency = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblIntlCurrency = new System.Windows.Forms.Label();
            this.dtpCalendarStartDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblCalendarStartDate = new System.Windows.Forms.Label();
            this.nudNumberOfPeriods = new System.Windows.Forms.NumericUpDown();
            this.lblNumberOfPeriods = new System.Windows.Forms.Label();
            this.nudCurrentPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentPeriod = new System.Windows.Forms.Label();
            this.nudNumberOfFwdPostingPeriods = new System.Windows.Forms.NumericUpDown();
            this.lblNumberOfFwdPostingPeriods = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbCreate = new System.Windows.Forms.ToolStripButton();
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
            // nudLedgerNumber
            //
            this.nudLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.nudLedgerNumber.Name = "nudLedgerNumber";
            this.nudLedgerNumber.Size = new System.Drawing.Size(150, 28);
            //
            // lblLedgerNumber
            //
            this.lblLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.lblLedgerNumber.Name = "lblLedgerNumber";
            this.lblLedgerNumber.AutoSize = true;
            this.lblLedgerNumber.Text = "Ledger Number:";
            this.lblLedgerNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLedgerNumber.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtLedgerName
            //
            this.txtLedgerName.Location = new System.Drawing.Point(2,2);
            this.txtLedgerName.Name = "txtLedgerName";
            this.txtLedgerName.Size = new System.Drawing.Size(150, 28);
            //
            // lblLedgerName
            //
            this.lblLedgerName.Location = new System.Drawing.Point(2,2);
            this.lblLedgerName.Name = "lblLedgerName";
            this.lblLedgerName.AutoSize = true;
            this.lblLedgerName.Text = "Ledger Name:";
            this.lblLedgerName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLedgerName.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbCountryCode
            //
            this.cmbCountryCode.Location = new System.Drawing.Point(2,2);
            this.cmbCountryCode.Name = "cmbCountryCode";
            this.cmbCountryCode.Size = new System.Drawing.Size(300, 28);
            this.cmbCountryCode.ListTable = TCmbAutoPopulated.TListTableEnum.CountryList;
            //
            // lblCountryCode
            //
            this.lblCountryCode.Location = new System.Drawing.Point(2,2);
            this.lblCountryCode.Name = "lblCountryCode";
            this.lblCountryCode.AutoSize = true;
            this.lblCountryCode.Text = "Country Code:";
            this.lblCountryCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCountryCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbBaseCurrency
            //
            this.cmbBaseCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbBaseCurrency.Name = "cmbBaseCurrency";
            this.cmbBaseCurrency.Size = new System.Drawing.Size(300, 28);
            this.cmbBaseCurrency.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            //
            // lblBaseCurrency
            //
            this.lblBaseCurrency.Location = new System.Drawing.Point(2,2);
            this.lblBaseCurrency.Name = "lblBaseCurrency";
            this.lblBaseCurrency.AutoSize = true;
            this.lblBaseCurrency.Text = "Base Currency:";
            this.lblBaseCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBaseCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbIntlCurrency
            //
            this.cmbIntlCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbIntlCurrency.Name = "cmbIntlCurrency";
            this.cmbIntlCurrency.Size = new System.Drawing.Size(300, 28);
            this.cmbIntlCurrency.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            //
            // lblIntlCurrency
            //
            this.lblIntlCurrency.Location = new System.Drawing.Point(2,2);
            this.lblIntlCurrency.Name = "lblIntlCurrency";
            this.lblIntlCurrency.AutoSize = true;
            this.lblIntlCurrency.Text = "Intl Currency:";
            this.lblIntlCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblIntlCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // dtpCalendarStartDate
            //
            this.dtpCalendarStartDate.Location = new System.Drawing.Point(2,2);
            this.dtpCalendarStartDate.Name = "dtpCalendarStartDate";
            this.dtpCalendarStartDate.Size = new System.Drawing.Size(150, 28);
            //
            // lblCalendarStartDate
            //
            this.lblCalendarStartDate.Location = new System.Drawing.Point(2,2);
            this.lblCalendarStartDate.Name = "lblCalendarStartDate";
            this.lblCalendarStartDate.AutoSize = true;
            this.lblCalendarStartDate.Text = "First Day of the Financial Year:";
            this.lblCalendarStartDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCalendarStartDate.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // nudNumberOfPeriods
            //
            this.nudNumberOfPeriods.Location = new System.Drawing.Point(2,2);
            this.nudNumberOfPeriods.Name = "nudNumberOfPeriods";
            this.nudNumberOfPeriods.Size = new System.Drawing.Size(150, 28);
            //
            // lblNumberOfPeriods
            //
            this.lblNumberOfPeriods.Location = new System.Drawing.Point(2,2);
            this.lblNumberOfPeriods.Name = "lblNumberOfPeriods";
            this.lblNumberOfPeriods.AutoSize = true;
            this.lblNumberOfPeriods.Text = "Number Of Periods:";
            this.lblNumberOfPeriods.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblNumberOfPeriods.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // nudCurrentPeriod
            //
            this.nudCurrentPeriod.Location = new System.Drawing.Point(2,2);
            this.nudCurrentPeriod.Name = "nudCurrentPeriod";
            this.nudCurrentPeriod.Size = new System.Drawing.Size(150, 28);
            //
            // lblCurrentPeriod
            //
            this.lblCurrentPeriod.Location = new System.Drawing.Point(2,2);
            this.lblCurrentPeriod.Name = "lblCurrentPeriod";
            this.lblCurrentPeriod.AutoSize = true;
            this.lblCurrentPeriod.Text = "Current Period:";
            this.lblCurrentPeriod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCurrentPeriod.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // nudNumberOfFwdPostingPeriods
            //
            this.nudNumberOfFwdPostingPeriods.Location = new System.Drawing.Point(2,2);
            this.nudNumberOfFwdPostingPeriods.Name = "nudNumberOfFwdPostingPeriods";
            this.nudNumberOfFwdPostingPeriods.Size = new System.Drawing.Size(150, 28);
            //
            // lblNumberOfFwdPostingPeriods
            //
            this.lblNumberOfFwdPostingPeriods.Location = new System.Drawing.Point(2,2);
            this.lblNumberOfFwdPostingPeriods.Name = "lblNumberOfFwdPostingPeriods";
            this.lblNumberOfFwdPostingPeriods.AutoSize = true;
            this.lblNumberOfFwdPostingPeriods.Text = "Number Of Fwd Posting Periods:";
            this.lblNumberOfFwdPostingPeriods.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblNumberOfFwdPostingPeriods.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLedgerName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCountryCode, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBaseCurrency, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblIntlCurrency, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblCalendarStartDate, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblNumberOfPeriods, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentPeriod, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblNumberOfFwdPostingPeriods, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.nudLedgerNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLedgerName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbCountryCode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbBaseCurrency, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbIntlCurrency, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtpCalendarStartDate, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.nudNumberOfPeriods, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.nudCurrentPeriod, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.nudNumberOfFwdPostingPeriods, 1, 8);
            //
            // tbbCreate
            //
            this.tbbCreate.Name = "tbbCreate";
            this.tbbCreate.AutoSize = true;
            this.tbbCreate.Click += new System.EventHandler(this.CreateLedger);
            this.tbbCreate.Text = "&Create Ledger";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbCreate});
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
            // TFrmGLCreateLedger
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmGLCreateLedger";
            this.Text = "Create Ledger";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown nudLedgerNumber;
        private System.Windows.Forms.Label lblLedgerNumber;
        private System.Windows.Forms.TextBox txtLedgerName;
        private System.Windows.Forms.Label lblLedgerName;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCountryCode;
        private System.Windows.Forms.Label lblCountryCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbBaseCurrency;
        private System.Windows.Forms.Label lblBaseCurrency;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbIntlCurrency;
        private System.Windows.Forms.Label lblIntlCurrency;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpCalendarStartDate;
        private System.Windows.Forms.Label lblCalendarStartDate;
        private System.Windows.Forms.NumericUpDown nudNumberOfPeriods;
        private System.Windows.Forms.Label lblNumberOfPeriods;
        private System.Windows.Forms.NumericUpDown nudCurrentPeriod;
        private System.Windows.Forms.Label lblCurrentPeriod;
        private System.Windows.Forms.NumericUpDown nudNumberOfFwdPostingPeriods;
        private System.Windows.Forms.Label lblNumberOfFwdPostingPeriods;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbCreate;
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
