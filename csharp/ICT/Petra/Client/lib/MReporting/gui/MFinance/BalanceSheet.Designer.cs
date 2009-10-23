/* auto generated with nant generateWinforms from BalanceSheet.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    partial class TFrmBalanceSheet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmBalanceSheet));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgReportSpecific = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpLedger = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedger = new System.Windows.Forms.TextBox();
            this.lblLedger = new System.Windows.Forms.Label();
            this.cmbAccountHierarchy = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccountHierarchy = new System.Windows.Forms.Label();
            this.grpCurrency = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCurrency = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.grpPeriodRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtStartPeriod = new System.Windows.Forms.TextBox();
            this.lblStartPeriod = new System.Windows.Forms.Label();
            this.txtEndPeriod = new System.Windows.Forms.TextBox();
            this.lblEndPeriod = new System.Windows.Forms.Label();
            this.cmbPeriodYear = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPeriodYear = new System.Windows.Forms.Label();
            this.tpgCostCentres = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrCostCentreOptions = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtSelectedCostCentres = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.clbCostCentres = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllCostCentres = new System.Windows.Forms.Button();
            this.rbtAllCostCentres = new System.Windows.Forms.RadioButton();
            this.rbtAllActiveCostCentres = new System.Windows.Forms.RadioButton();
            this.rbtAccountLevel = new System.Windows.Forms.RadioButton();
            this.chkCostCentreBreakdown = new System.Windows.Forms.CheckBox();
            this.rgrDepth = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtDetail = new System.Windows.Forms.RadioButton();
            this.rbtStandard = new System.Windows.Forms.RadioButton();
            this.rbtSummary = new System.Windows.Forms.RadioButton();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbGenerateReport = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettings = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettingsAs = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettingsDialog = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniLoadSettings1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveSettingsAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGenerateReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.tabReportSettings.SuspendLayout();
            this.tpgReportSpecific.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpLedger.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpCurrency.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpPeriodRange.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tpgCostCentres.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.rgrCostCentreOptions.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.rgrDepth.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // tpgReportSpecific
            //
            this.tpgReportSpecific.Location = new System.Drawing.Point(2,2);
            this.tpgReportSpecific.Name = "tpgReportSpecific";
            this.tpgReportSpecific.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.tpgReportSpecific.Controls.Add(this.tableLayoutPanel1);
            //
            // grpLedger
            //
            this.grpLedger.Name = "grpLedger";
            this.grpLedger.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLedger.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpLedger.Controls.Add(this.tableLayoutPanel2);
            //
            // txtLedger
            //
            this.txtLedger.Location = new System.Drawing.Point(2,2);
            this.txtLedger.Name = "txtLedger";
            this.txtLedger.Size = new System.Drawing.Size(150, 28);
            this.txtLedger.ReadOnly = true;
            //
            // lblLedger
            //
            this.lblLedger.Location = new System.Drawing.Point(2,2);
            this.lblLedger.Name = "lblLedger";
            this.lblLedger.AutoSize = true;
            this.lblLedger.Text = "Ledger:";
            this.lblLedger.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbAccountHierarchy
            //
            this.cmbAccountHierarchy.Location = new System.Drawing.Point(2,2);
            this.cmbAccountHierarchy.Name = "cmbAccountHierarchy";
            this.cmbAccountHierarchy.Size = new System.Drawing.Size(300, 28);
            this.cmbAccountHierarchy.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblAccountHierarchy
            //
            this.lblAccountHierarchy.Location = new System.Drawing.Point(2,2);
            this.lblAccountHierarchy.Name = "lblAccountHierarchy";
            this.lblAccountHierarchy.AutoSize = true;
            this.lblAccountHierarchy.Text = "Account Hierarchy:";
            this.lblAccountHierarchy.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblLedger, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLedger, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAccountHierarchy, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbAccountHierarchy, 3, 0);
            this.grpLedger.Text = "Ledger Details";
            //
            // grpCurrency
            //
            this.grpCurrency.Name = "grpCurrency";
            this.grpCurrency.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCurrency.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpCurrency.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbCurrency
            //
            this.cmbCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(150, 28);
            this.cmbCurrency.Items.AddRange(new object[] {"Base","International","Transaction"});
            //
            // lblCurrency
            //
            this.lblCurrency.Location = new System.Drawing.Point(2,2);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Text = "Currency:";
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblCurrency, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbCurrency, 1, 0);
            this.grpCurrency.Text = "Currency";
            //
            // grpPeriodRange
            //
            this.grpPeriodRange.Location = new System.Drawing.Point(2,2);
            this.grpPeriodRange.Name = "grpPeriodRange";
            this.grpPeriodRange.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.grpPeriodRange.Controls.Add(this.tableLayoutPanel4);
            //
            // txtStartPeriod
            //
            this.txtStartPeriod.Location = new System.Drawing.Point(2,2);
            this.txtStartPeriod.Name = "txtStartPeriod";
            this.txtStartPeriod.Size = new System.Drawing.Size(30, 28);
            //
            // lblStartPeriod
            //
            this.lblStartPeriod.Location = new System.Drawing.Point(2,2);
            this.lblStartPeriod.Name = "lblStartPeriod";
            this.lblStartPeriod.AutoSize = true;
            this.lblStartPeriod.Text = "from:";
            this.lblStartPeriod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtEndPeriod
            //
            this.txtEndPeriod.Location = new System.Drawing.Point(2,2);
            this.txtEndPeriod.Name = "txtEndPeriod";
            this.txtEndPeriod.Size = new System.Drawing.Size(30, 28);
            //
            // lblEndPeriod
            //
            this.lblEndPeriod.Location = new System.Drawing.Point(2,2);
            this.lblEndPeriod.Name = "lblEndPeriod";
            this.lblEndPeriod.AutoSize = true;
            this.lblEndPeriod.Text = "to:";
            this.lblEndPeriod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbPeriodYear
            //
            this.cmbPeriodYear.Location = new System.Drawing.Point(2,2);
            this.cmbPeriodYear.Name = "cmbPeriodYear";
            this.cmbPeriodYear.Size = new System.Drawing.Size(300, 28);
            this.cmbPeriodYear.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblPeriodYear
            //
            this.lblPeriodYear.Location = new System.Drawing.Point(2,2);
            this.lblPeriodYear.Name = "lblPeriodYear";
            this.lblPeriodYear.AutoSize = true;
            this.lblPeriodYear.Text = "Year:";
            this.lblPeriodYear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblStartPeriod, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtStartPeriod, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblEndPeriod, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtEndPeriod, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPeriodYear, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbPeriodYear, 5, 0);
            this.grpPeriodRange.Text = "Period Range";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.grpLedger, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpCurrency, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpPeriodRange, 0, 2);
            this.tpgReportSpecific.Text = "General Settings";
            this.tpgReportSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgCostCentres
            //
            this.tpgCostCentres.Location = new System.Drawing.Point(2,2);
            this.tpgCostCentres.Name = "tpgCostCentres";
            this.tpgCostCentres.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.tpgCostCentres.Controls.Add(this.tableLayoutPanel5);
            //
            // rgrCostCentreOptions
            //
            this.rgrCostCentreOptions.Location = new System.Drawing.Point(2,2);
            this.rgrCostCentreOptions.Name = "rgrCostCentreOptions";
            this.rgrCostCentreOptions.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.rgrCostCentreOptions.Controls.Add(this.tableLayoutPanel6);
            //
            // rbtSelectedCostCentres
            //
            this.rbtSelectedCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtSelectedCostCentres.Name = "rbtSelectedCostCentres";
            this.rbtSelectedCostCentres.AutoSize = true;
            this.rbtSelectedCostCentres.Text = "Selected Cost Centres";
            this.rbtSelectedCostCentres.Checked = true;
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            //
            // clbCostCentres
            //
            this.clbCostCentres.Name = "clbCostCentres";
            this.clbCostCentres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbCostCentres.FixedRows = 0;
            //
            // btnUnselectAllCostCentres
            //
            this.btnUnselectAllCostCentres.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllCostCentres.Name = "btnUnselectAllCostCentres";
            this.btnUnselectAllCostCentres.AutoSize = true;
            this.btnUnselectAllCostCentres.Text = "Unselect All";
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.clbCostCentres, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnUnselectAllCostCentres, 1, 0);
            this.rbtSelectedCostCentres.CheckedChanged += new System.EventHandler(this.rbtSelectedCostCentresCheckedChanged);
            //
            // rbtAllCostCentres
            //
            this.rbtAllCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtAllCostCentres.Name = "rbtAllCostCentres";
            this.rbtAllCostCentres.AutoSize = true;
            this.rbtAllCostCentres.Text = "All Cost Centres";
            //
            // rbtAllActiveCostCentres
            //
            this.rbtAllActiveCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtAllActiveCostCentres.Name = "rbtAllActiveCostCentres";
            this.rbtAllActiveCostCentres.AutoSize = true;
            this.rbtAllActiveCostCentres.Text = "All Active Cost Centres";
            //
            // rbtAccountLevel
            //
            this.rbtAccountLevel.Location = new System.Drawing.Point(2,2);
            this.rbtAccountLevel.Name = "rbtAccountLevel";
            this.rbtAccountLevel.AutoSize = true;
            this.rbtAccountLevel.Text = "Account Level";
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.rbtSelectedCostCentres, 0, 0);
            this.tableLayoutPanel6.SetColumnSpan(this.rbtAllCostCentres, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtAllCostCentres, 0, 1);
            this.tableLayoutPanel6.SetColumnSpan(this.rbtAllActiveCostCentres, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtAllActiveCostCentres, 0, 2);
            this.tableLayoutPanel6.SetColumnSpan(this.rbtAccountLevel, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtAccountLevel, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.rgrCostCentreOptions.Text = "Cost Centre Options";
            //
            // chkCostCentreBreakdown
            //
            this.chkCostCentreBreakdown.Location = new System.Drawing.Point(2,2);
            this.chkCostCentreBreakdown.Name = "chkCostCentreBreakdown";
            this.chkCostCentreBreakdown.AutoSize = true;
            this.chkCostCentreBreakdown.Text = "Cost Centre Breakdown";
            this.chkCostCentreBreakdown.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // rgrDepth
            //
            this.rgrDepth.Location = new System.Drawing.Point(2,2);
            this.rgrDepth.Name = "rgrDepth";
            this.rgrDepth.AutoSize = true;
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.AutoSize = true;
            this.rgrDepth.Controls.Add(this.tableLayoutPanel8);
            //
            // rbtDetail
            //
            this.rbtDetail.Location = new System.Drawing.Point(2,2);
            this.rbtDetail.Name = "rbtDetail";
            this.rbtDetail.AutoSize = true;
            this.rbtDetail.Text = "detail";
            this.rbtDetail.Checked = true;
            //
            // rbtStandard
            //
            this.rbtStandard.Location = new System.Drawing.Point(2,2);
            this.rbtStandard.Name = "rbtStandard";
            this.rbtStandard.AutoSize = true;
            this.rbtStandard.Text = "standard";
            //
            // rbtSummary
            //
            this.rbtSummary.Location = new System.Drawing.Point(2,2);
            this.rbtSummary.Name = "rbtSummary";
            this.rbtSummary.AutoSize = true;
            this.rbtSummary.Text = "summary";
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 3;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Controls.Add(this.rbtDetail, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.rbtStandard, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.rbtSummary, 0, 2);
            this.rgrDepth.Text = "Depth";
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.rgrCostCentreOptions, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.chkCostCentreBreakdown, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.rgrDepth, 0, 2);
            this.tpgCostCentres.Text = "CostCentre Settings";
            this.tpgCostCentres.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgReportSpecific);
            this.tabReportSettings.Controls.Add(this.tpgCostCentres);
            //
            // tbbGenerateReport
            //
            this.tbbGenerateReport.Name = "tbbGenerateReport";
            this.tbbGenerateReport.AutoSize = true;
            this.tbbGenerateReport.Click += new System.EventHandler(this.actGenerateReport);
            this.tbbGenerateReport.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbGenerateReport.Glyph"));
            this.tbbGenerateReport.ToolTipText = "Generate the report";
            this.tbbGenerateReport.Text = "&Generate";
            //
            // tbbSaveSettings
            //
            this.tbbSaveSettings.Name = "tbbSaveSettings";
            this.tbbSaveSettings.AutoSize = true;
            this.tbbSaveSettings.Click += new System.EventHandler(this.actSaveSettings);
            this.tbbSaveSettings.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSaveSettings.Glyph"));
            this.tbbSaveSettings.Text = "&Save Settings";
            //
            // tbbSaveSettingsAs
            //
            this.tbbSaveSettingsAs.Name = "tbbSaveSettingsAs";
            this.tbbSaveSettingsAs.AutoSize = true;
            this.tbbSaveSettingsAs.Click += new System.EventHandler(this.actSaveSettingsAs);
            this.tbbSaveSettingsAs.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSaveSettingsAs.Glyph"));
            this.tbbSaveSettingsAs.Text = "Save Settings &As...";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbGenerateReport,
                        tbbSaveSettings,
                        tbbSaveSettingsAs});
            //
            // mniLoadSettingsDialog
            //
            this.mniLoadSettingsDialog.Name = "mniLoadSettingsDialog";
            this.mniLoadSettingsDialog.AutoSize = true;
            this.mniLoadSettingsDialog.Click += new System.EventHandler(this.actLoadSettingsDialog);
            this.mniLoadSettingsDialog.Text = "&Open...";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniLoadSettings1
            //
            this.mniLoadSettings1.Name = "mniLoadSettings1";
            this.mniLoadSettings1.AutoSize = true;
            this.mniLoadSettings1.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings1.Text = "RecentSettings";
            //
            // mniLoadSettings2
            //
            this.mniLoadSettings2.Name = "mniLoadSettings2";
            this.mniLoadSettings2.AutoSize = true;
            this.mniLoadSettings2.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings2.Text = "RecentSettings";
            //
            // mniLoadSettings3
            //
            this.mniLoadSettings3.Name = "mniLoadSettings3";
            this.mniLoadSettings3.AutoSize = true;
            this.mniLoadSettings3.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings3.Text = "RecentSettings";
            //
            // mniLoadSettings4
            //
            this.mniLoadSettings4.Name = "mniLoadSettings4";
            this.mniLoadSettings4.AutoSize = true;
            this.mniLoadSettings4.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings4.Text = "RecentSettings";
            //
            // mniLoadSettings5
            //
            this.mniLoadSettings5.Name = "mniLoadSettings5";
            this.mniLoadSettings5.AutoSize = true;
            this.mniLoadSettings5.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings5.Text = "RecentSettings";
            //
            // mniLoadSettings
            //
            this.mniLoadSettings.Name = "mniLoadSettings";
            this.mniLoadSettings.AutoSize = true;
            this.mniLoadSettings.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniLoadSettingsDialog,
                        mniSeparator0,
                        mniLoadSettings1,
                        mniLoadSettings2,
                        mniLoadSettings3,
                        mniLoadSettings4,
                        mniLoadSettings5});
            this.mniLoadSettings.Text = "&Load Settings";
            //
            // mniSaveSettings
            //
            this.mniSaveSettings.Name = "mniSaveSettings";
            this.mniSaveSettings.AutoSize = true;
            this.mniSaveSettings.Click += new System.EventHandler(this.actSaveSettings);
            this.mniSaveSettings.Image = ((System.Drawing.Bitmap)resources.GetObject("mniSaveSettings.Glyph"));
            this.mniSaveSettings.Text = "&Save Settings";
            //
            // mniSaveSettingsAs
            //
            this.mniSaveSettingsAs.Name = "mniSaveSettingsAs";
            this.mniSaveSettingsAs.AutoSize = true;
            this.mniSaveSettingsAs.Click += new System.EventHandler(this.actSaveSettingsAs);
            this.mniSaveSettingsAs.Image = ((System.Drawing.Bitmap)resources.GetObject("mniSaveSettingsAs.Glyph"));
            this.mniSaveSettingsAs.Text = "Save Settings &As...";
            //
            // mniMaintainSettings
            //
            this.mniMaintainSettings.Name = "mniMaintainSettings";
            this.mniMaintainSettings.AutoSize = true;
            this.mniMaintainSettings.Click += new System.EventHandler(this.actMaintainSettings);
            this.mniMaintainSettings.Text = "&Maintain Settings...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniGenerateReport
            //
            this.mniGenerateReport.Name = "mniGenerateReport";
            this.mniGenerateReport.AutoSize = true;
            this.mniGenerateReport.Click += new System.EventHandler(this.actGenerateReport);
            this.mniGenerateReport.Image = ((System.Drawing.Bitmap)resources.GetObject("mniGenerateReport.Glyph"));
            this.mniGenerateReport.ToolTipText = "Generate the report";
            this.mniGenerateReport.Text = "&Generate";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
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
                           mniLoadSettings,
                        mniSaveSettings,
                        mniSaveSettingsAs,
                        mniMaintainSettings,
                        mniSeparator1,
                        mniGenerateReport,
                        mniSeparator2,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
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
                        mniSeparator3,
                        mniHelpBugReport,
                        mniSeparator4,
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
            // TFrmBalanceSheet
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 600);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.tabReportSettings);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmBalanceSheet";
            this.Text = "Balance Sheet";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.rgrDepth.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.rgrCostCentreOptions.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tpgCostCentres.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.grpPeriodRange.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpCurrency.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpLedger.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpgReportSpecific.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgReportSpecific;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpLedger;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtLedger;
        private System.Windows.Forms.Label lblLedger;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountHierarchy;
        private System.Windows.Forms.Label lblAccountHierarchy;
        private System.Windows.Forms.GroupBox grpCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Common.Controls.TCmbAutoComplete cmbCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.GroupBox grpPeriodRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtStartPeriod;
        private System.Windows.Forms.Label lblStartPeriod;
        private System.Windows.Forms.TextBox txtEndPeriod;
        private System.Windows.Forms.Label lblEndPeriod;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPeriodYear;
        private System.Windows.Forms.Label lblPeriodYear;
        private System.Windows.Forms.TabPage tpgCostCentres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox rgrCostCentreOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton rbtSelectedCostCentres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Ict.Common.Controls.TClbVersatile clbCostCentres;
        private System.Windows.Forms.Button btnUnselectAllCostCentres;
        private System.Windows.Forms.RadioButton rbtAllCostCentres;
        private System.Windows.Forms.RadioButton rbtAllActiveCostCentres;
        private System.Windows.Forms.RadioButton rbtAccountLevel;
        private System.Windows.Forms.CheckBox chkCostCentreBreakdown;
        private System.Windows.Forms.GroupBox rgrDepth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.RadioButton rbtDetail;
        private System.Windows.Forms.RadioButton rbtStandard;
        private System.Windows.Forms.RadioButton rbtSummary;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbGenerateReport;
        private System.Windows.Forms.ToolStripButton tbbSaveSettings;
        private System.Windows.Forms.ToolStripButton tbbSaveSettingsAs;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettingsDialog;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings1;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings2;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings3;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings4;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings5;
        private System.Windows.Forms.ToolStripMenuItem mniSaveSettings;
        private System.Windows.Forms.ToolStripMenuItem mniSaveSettingsAs;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainSettings;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniGenerateReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
