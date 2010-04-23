/* auto generated with nant generateWinforms from AccountDetail.yaml
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
    partial class TFrmAccountDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAccountDetail));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgReportSpecific = new System.Windows.Forms.TabPage();
            this.grpLedger = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedger = new System.Windows.Forms.TextBox();
            this.lblLedger = new System.Windows.Forms.Label();
            this.cmbAccountHierarchy = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccountHierarchy = new System.Windows.Forms.Label();
            this.grpCurrency = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCurrency = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.rgrPeriod = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPeriodRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtStartPeriod = new System.Windows.Forms.TextBox();
            this.lblStartPeriod = new System.Windows.Forms.Label();
            this.txtEndPeriod = new System.Windows.Forms.TextBox();
            this.lblEndPeriod = new System.Windows.Forms.Label();
            this.cmbPeriodYear = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPeriodYear = new System.Windows.Forms.Label();
            this.rbtDateRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpDateStart = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.dtpDateEnd = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateEnd = new System.Windows.Forms.Label();
            this.rgrSorting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtSortByAccount = new System.Windows.Forms.RadioButton();
            this.rbtSortByCostCentre = new System.Windows.Forms.RadioButton();
            this.rbtSortByReference = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtReferenceFrom = new System.Windows.Forms.TextBox();
            this.lblReferenceFrom = new System.Windows.Forms.Label();
            this.txtReferenceTo = new System.Windows.Forms.TextBox();
            this.lblReferenceTo = new System.Windows.Forms.Label();
            this.rbtSortByAnalysisType = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAnalysisTypeFrom = new System.Windows.Forms.TextBox();
            this.lblAnalysisTypeFrom = new System.Windows.Forms.Label();
            this.txtAnalysisTypeTo = new System.Windows.Forms.TextBox();
            this.lblAnalysisTypeTo = new System.Windows.Forms.Label();
            this.tpgCCAccount = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrAccounts = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAccountRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbAccountStart = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccountStart = new System.Windows.Forms.Label();
            this.cmbAccountEnd = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccountEnd = new System.Windows.Forms.Label();
            this.rbtAccountList = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.clbAccounts = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllAccounts = new System.Windows.Forms.Button();
            this.rgrCostCentres = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtCostCentreRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCostCentreStart = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCostCentreStart = new System.Windows.Forms.Label();
            this.cmbCostCentreEnd = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCostCentreEnd = new System.Windows.Forms.Label();
            this.rbtCostCentreList = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.clbCostCentres = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllCostCentres = new System.Windows.Forms.Button();
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
            this.grpLedger.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpCurrency.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrPeriod.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.rgrSorting.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tpgCCAccount.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.rgrAccounts.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.rgrCostCentres.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // tpgReportSpecific
            //
            this.tpgReportSpecific.Location = new System.Drawing.Point(2,2);
            this.tpgReportSpecific.Name = "tpgReportSpecific";
            this.tpgReportSpecific.AutoSize = true;
            this.tpgReportSpecific.Controls.Add(this.rgrSorting);
            this.tpgReportSpecific.Controls.Add(this.rgrPeriod);
            this.tpgReportSpecific.Controls.Add(this.grpCurrency);
            this.tpgReportSpecific.Controls.Add(this.grpLedger);
            //
            // grpLedger
            //
            this.grpLedger.Name = "grpLedger";
            this.grpLedger.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLedger.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpLedger.Controls.Add(this.tableLayoutPanel1);
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
            this.lblLedger.Dock = System.Windows.Forms.DockStyle.Right;
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
            this.lblAccountHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedger, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLedger, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblAccountHierarchy, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbAccountHierarchy, 3, 0);
            this.grpLedger.Text = "Ledger Details";
            //
            // grpCurrency
            //
            this.grpCurrency.Name = "grpCurrency";
            this.grpCurrency.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCurrency.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpCurrency.Controls.Add(this.tableLayoutPanel2);
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
            this.lblCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblCurrency, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbCurrency, 1, 0);
            this.grpCurrency.Text = "Currency";
            //
            // rgrPeriod
            //
            this.rgrPeriod.Name = "rgrPeriod";
            this.rgrPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrPeriod.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.rgrPeriod.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtPeriodRange
            //
            this.rbtPeriodRange.Location = new System.Drawing.Point(2,2);
            this.rbtPeriodRange.Name = "rbtPeriodRange";
            this.rbtPeriodRange.AutoSize = true;
            this.rbtPeriodRange.Text = "Period Range";
            this.rbtPeriodRange.Checked = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
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
            this.lblStartPeriod.Dock = System.Windows.Forms.DockStyle.Right;
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
            this.lblEndPeriod.Dock = System.Windows.Forms.DockStyle.Right;
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
            this.lblPeriodYear.Dock = System.Windows.Forms.DockStyle.Right;
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
            this.rbtPeriodRange.CheckedChanged += new System.EventHandler(this.rbtPeriodRangeCheckedChanged);
            //
            // rbtDateRange
            //
            this.rbtDateRange.Location = new System.Drawing.Point(2,2);
            this.rbtDateRange.Name = "rbtDateRange";
            this.rbtDateRange.AutoSize = true;
            this.rbtDateRange.Text = "Date Range";
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            //
            // dtpDateStart
            //
            this.dtpDateStart.Location = new System.Drawing.Point(2,2);
            this.dtpDateStart.Name = "dtpDateStart";
            this.dtpDateStart.Size = new System.Drawing.Size(150, 28);
            //
            // dtpDateEnd
            //
            this.dtpDateEnd.Location = new System.Drawing.Point(2,2);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(150, 28);
            //
            // lblDateEnd
            //
            this.lblDateEnd.Location = new System.Drawing.Point(2,2);
            this.lblDateEnd.Name = "lblDateEnd";
            this.lblDateEnd.AutoSize = true;
            this.lblDateEnd.Text = "to:";
            this.lblDateEnd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.dtpDateStart, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblDateEnd, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.dtpDateEnd, 2, 0);
            this.rbtDateRange.CheckedChanged += new System.EventHandler(this.rbtDateRangeCheckedChanged);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtPeriodRange, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtDateRange, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.rgrPeriod.Text = "Period";
            //
            // rgrSorting
            //
            this.rgrSorting.Name = "rgrSorting";
            this.rgrSorting.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrSorting.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.rgrSorting.Controls.Add(this.tableLayoutPanel6);
            //
            // rbtSortByAccount
            //
            this.rbtSortByAccount.Location = new System.Drawing.Point(2,2);
            this.rbtSortByAccount.Name = "rbtSortByAccount";
            this.rbtSortByAccount.AutoSize = true;
            this.rbtSortByAccount.Text = "Sort by Account";
            this.rbtSortByAccount.Checked = true;
            //
            // rbtSortByCostCentre
            //
            this.rbtSortByCostCentre.Location = new System.Drawing.Point(2,2);
            this.rbtSortByCostCentre.Name = "rbtSortByCostCentre";
            this.rbtSortByCostCentre.AutoSize = true;
            this.rbtSortByCostCentre.Text = "Sort by Cost Centre";
            //
            // rbtSortByReference
            //
            this.rbtSortByReference.Location = new System.Drawing.Point(2,2);
            this.rbtSortByReference.Name = "rbtSortByReference";
            this.rbtSortByReference.AutoSize = true;
            this.rbtSortByReference.Text = "Sort By Reference";
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            //
            // txtReferenceFrom
            //
            this.txtReferenceFrom.Location = new System.Drawing.Point(2,2);
            this.txtReferenceFrom.Name = "txtReferenceFrom";
            this.txtReferenceFrom.Size = new System.Drawing.Size(150, 28);
            //
            // lblReferenceFrom
            //
            this.lblReferenceFrom.Location = new System.Drawing.Point(2,2);
            this.lblReferenceFrom.Name = "lblReferenceFrom";
            this.lblReferenceFrom.AutoSize = true;
            this.lblReferenceFrom.Text = "from:";
            this.lblReferenceFrom.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReferenceFrom.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtReferenceTo
            //
            this.txtReferenceTo.Location = new System.Drawing.Point(2,2);
            this.txtReferenceTo.Name = "txtReferenceTo";
            this.txtReferenceTo.Size = new System.Drawing.Size(150, 28);
            //
            // lblReferenceTo
            //
            this.lblReferenceTo.Location = new System.Drawing.Point(2,2);
            this.lblReferenceTo.Name = "lblReferenceTo";
            this.lblReferenceTo.AutoSize = true;
            this.lblReferenceTo.Text = "to:";
            this.lblReferenceTo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReferenceTo.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.lblReferenceFrom, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtReferenceFrom, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblReferenceTo, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtReferenceTo, 3, 0);
            this.rbtSortByReference.CheckedChanged += new System.EventHandler(this.rbtSortByReferenceCheckedChanged);
            //
            // rbtSortByAnalysisType
            //
            this.rbtSortByAnalysisType.Location = new System.Drawing.Point(2,2);
            this.rbtSortByAnalysisType.Name = "rbtSortByAnalysisType";
            this.rbtSortByAnalysisType.AutoSize = true;
            this.rbtSortByAnalysisType.Text = "Sort By Analysis Type";
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.AutoSize = true;
            //
            // txtAnalysisTypeFrom
            //
            this.txtAnalysisTypeFrom.Location = new System.Drawing.Point(2,2);
            this.txtAnalysisTypeFrom.Name = "txtAnalysisTypeFrom";
            this.txtAnalysisTypeFrom.Size = new System.Drawing.Size(150, 28);
            //
            // lblAnalysisTypeFrom
            //
            this.lblAnalysisTypeFrom.Location = new System.Drawing.Point(2,2);
            this.lblAnalysisTypeFrom.Name = "lblAnalysisTypeFrom";
            this.lblAnalysisTypeFrom.AutoSize = true;
            this.lblAnalysisTypeFrom.Text = "from:";
            this.lblAnalysisTypeFrom.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAnalysisTypeFrom.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtAnalysisTypeTo
            //
            this.txtAnalysisTypeTo.Location = new System.Drawing.Point(2,2);
            this.txtAnalysisTypeTo.Name = "txtAnalysisTypeTo";
            this.txtAnalysisTypeTo.Size = new System.Drawing.Size(150, 28);
            //
            // lblAnalysisTypeTo
            //
            this.lblAnalysisTypeTo.Location = new System.Drawing.Point(2,2);
            this.lblAnalysisTypeTo.Name = "lblAnalysisTypeTo";
            this.lblAnalysisTypeTo.AutoSize = true;
            this.lblAnalysisTypeTo.Text = "to:";
            this.lblAnalysisTypeTo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAnalysisTypeTo.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Controls.Add(this.lblAnalysisTypeFrom, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.txtAnalysisTypeFrom, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.lblAnalysisTypeTo, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.txtAnalysisTypeTo, 3, 0);
            this.rbtSortByAnalysisType.CheckedChanged += new System.EventHandler(this.rbtSortByAnalysisTypeCheckedChanged);
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.SetColumnSpan(this.rbtSortByAccount, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtSortByAccount, 0, 0);
            this.tableLayoutPanel6.SetColumnSpan(this.rbtSortByCostCentre, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtSortByCostCentre, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.rbtSortByReference, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.rbtSortByAnalysisType, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel8, 1, 3);
            this.rgrSorting.Text = "Sorting";
            this.tpgReportSpecific.Text = "General Settings";
            this.tpgReportSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgCCAccount
            //
            this.tpgCCAccount.Location = new System.Drawing.Point(2,2);
            this.tpgCCAccount.Name = "tpgCCAccount";
            this.tpgCCAccount.AutoSize = true;
            //
            // tableLayoutPanel9
            //
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.AutoSize = true;
            this.tpgCCAccount.Controls.Add(this.tableLayoutPanel9);
            //
            // rgrAccounts
            //
            this.rgrAccounts.Location = new System.Drawing.Point(2,2);
            this.rgrAccounts.Name = "rgrAccounts";
            this.rgrAccounts.AutoSize = true;
            //
            // tableLayoutPanel10
            //
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.AutoSize = true;
            this.rgrAccounts.Controls.Add(this.tableLayoutPanel10);
            //
            // rbtAccountRange
            //
            this.rbtAccountRange.Location = new System.Drawing.Point(2,2);
            this.rbtAccountRange.Name = "rbtAccountRange";
            this.rbtAccountRange.AutoSize = true;
            this.rbtAccountRange.Text = "Account Range";
            this.rbtAccountRange.Checked = true;
            //
            // tableLayoutPanel11
            //
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.AutoSize = true;
            //
            // cmbAccountStart
            //
            this.cmbAccountStart.Location = new System.Drawing.Point(2,2);
            this.cmbAccountStart.Name = "cmbAccountStart";
            this.cmbAccountStart.Size = new System.Drawing.Size(300, 28);
            this.cmbAccountStart.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblAccountStart
            //
            this.lblAccountStart.Location = new System.Drawing.Point(2,2);
            this.lblAccountStart.Name = "lblAccountStart";
            this.lblAccountStart.AutoSize = true;
            this.lblAccountStart.Text = "From:";
            this.lblAccountStart.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAccountStart.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbAccountEnd
            //
            this.cmbAccountEnd.Location = new System.Drawing.Point(2,2);
            this.cmbAccountEnd.Name = "cmbAccountEnd";
            this.cmbAccountEnd.Size = new System.Drawing.Size(300, 28);
            this.cmbAccountEnd.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblAccountEnd
            //
            this.lblAccountEnd.Location = new System.Drawing.Point(2,2);
            this.lblAccountEnd.Name = "lblAccountEnd";
            this.lblAccountEnd.AutoSize = true;
            this.lblAccountEnd.Text = "To:";
            this.lblAccountEnd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAccountEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.Controls.Add(this.lblAccountStart, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.lblAccountEnd, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.cmbAccountStart, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.cmbAccountEnd, 1, 1);
            this.rbtAccountRange.CheckedChanged += new System.EventHandler(this.rbtAccountRangeCheckedChanged);
            //
            // rbtAccountList
            //
            this.rbtAccountList.Location = new System.Drawing.Point(2,2);
            this.rbtAccountList.Name = "rbtAccountList";
            this.rbtAccountList.AutoSize = true;
            this.rbtAccountList.Text = "Account List";
            //
            // tableLayoutPanel12
            //
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.AutoSize = true;
            //
            // clbAccounts
            //
            this.clbAccounts.Name = "clbAccounts";
            this.clbAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbAccounts.FixedRows = 0;
            //
            // btnUnselectAllAccounts
            //
            this.btnUnselectAllAccounts.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllAccounts.Name = "btnUnselectAllAccounts";
            this.btnUnselectAllAccounts.AutoSize = true;
            this.btnUnselectAllAccounts.Text = "Unselect All";
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.Controls.Add(this.clbAccounts, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.btnUnselectAllAccounts, 1, 0);
            this.rbtAccountList.CheckedChanged += new System.EventHandler(this.rbtAccountListCheckedChanged);
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.Controls.Add(this.rbtAccountRange, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.rbtAccountList, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel12, 1, 1);
            this.rgrAccounts.Text = "Accounts";
            //
            // rgrCostCentres
            //
            this.rgrCostCentres.Location = new System.Drawing.Point(2,2);
            this.rgrCostCentres.Name = "rgrCostCentres";
            this.rgrCostCentres.AutoSize = true;
            //
            // tableLayoutPanel13
            //
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.AutoSize = true;
            this.rgrCostCentres.Controls.Add(this.tableLayoutPanel13);
            //
            // rbtCostCentreRange
            //
            this.rbtCostCentreRange.Location = new System.Drawing.Point(2,2);
            this.rbtCostCentreRange.Name = "rbtCostCentreRange";
            this.rbtCostCentreRange.AutoSize = true;
            this.rbtCostCentreRange.Text = "Cost Centre Range";
            this.rbtCostCentreRange.Checked = true;
            //
            // tableLayoutPanel14
            //
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.AutoSize = true;
            //
            // cmbCostCentreStart
            //
            this.cmbCostCentreStart.Location = new System.Drawing.Point(2,2);
            this.cmbCostCentreStart.Name = "cmbCostCentreStart";
            this.cmbCostCentreStart.Size = new System.Drawing.Size(300, 28);
            this.cmbCostCentreStart.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblCostCentreStart
            //
            this.lblCostCentreStart.Location = new System.Drawing.Point(2,2);
            this.lblCostCentreStart.Name = "lblCostCentreStart";
            this.lblCostCentreStart.AutoSize = true;
            this.lblCostCentreStart.Text = "From:";
            this.lblCostCentreStart.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCostCentreStart.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbCostCentreEnd
            //
            this.cmbCostCentreEnd.Location = new System.Drawing.Point(2,2);
            this.cmbCostCentreEnd.Name = "cmbCostCentreEnd";
            this.cmbCostCentreEnd.Size = new System.Drawing.Size(300, 28);
            this.cmbCostCentreEnd.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblCostCentreEnd
            //
            this.lblCostCentreEnd.Location = new System.Drawing.Point(2,2);
            this.lblCostCentreEnd.Name = "lblCostCentreEnd";
            this.lblCostCentreEnd.AutoSize = true;
            this.lblCostCentreEnd.Text = "To:";
            this.lblCostCentreEnd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCostCentreEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.RowCount = 2;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel14.Controls.Add(this.lblCostCentreStart, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.lblCostCentreEnd, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.cmbCostCentreStart, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.cmbCostCentreEnd, 1, 1);
            this.rbtCostCentreRange.CheckedChanged += new System.EventHandler(this.rbtCostCentreRangeCheckedChanged);
            //
            // rbtCostCentreList
            //
            this.rbtCostCentreList.Location = new System.Drawing.Point(2,2);
            this.rbtCostCentreList.Name = "rbtCostCentreList";
            this.rbtCostCentreList.AutoSize = true;
            this.rbtCostCentreList.Text = "Cost Centre List";
            //
            // tableLayoutPanel15
            //
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.AutoSize = true;
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
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel15.Controls.Add(this.clbCostCentres, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnUnselectAllCostCentres, 1, 0);
            this.rbtCostCentreList.CheckedChanged += new System.EventHandler(this.rbtCostCentreListCheckedChanged);
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.RowCount = 2;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Controls.Add(this.rbtCostCentreRange, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.rbtCostCentreList, 0, 1);
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel14, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel15, 1, 1);
            this.rgrCostCentres.Text = "Cost Centres";
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Controls.Add(this.rgrAccounts, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.rgrCostCentres, 0, 1);
            this.tpgCCAccount.Text = "Account/CostCentre Settings";
            this.tpgCCAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgReportSpecific);
            this.tabReportSettings.Controls.Add(this.tpgCCAccount);
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
            // TFrmAccountDetail
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
            this.Name = "TFrmAccountDetail";
            this.Text = "Account Detail";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.rgrCostCentres.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.rgrAccounts.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tpgCCAccount.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.rgrSorting.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrPeriod.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpCurrency.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpLedger.ResumeLayout(false);
            this.tpgReportSpecific.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgReportSpecific;
        private System.Windows.Forms.GroupBox grpLedger;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtLedger;
        private System.Windows.Forms.Label lblLedger;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountHierarchy;
        private System.Windows.Forms.Label lblAccountHierarchy;
        private System.Windows.Forms.GroupBox grpCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Common.Controls.TCmbAutoComplete cmbCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.GroupBox rgrPeriod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtPeriodRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtStartPeriod;
        private System.Windows.Forms.Label lblStartPeriod;
        private System.Windows.Forms.TextBox txtEndPeriod;
        private System.Windows.Forms.Label lblEndPeriod;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPeriodYear;
        private System.Windows.Forms.Label lblPeriodYear;
        private System.Windows.Forms.RadioButton rbtDateRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateStart;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateEnd;
        private System.Windows.Forms.Label lblDateEnd;
        private System.Windows.Forms.GroupBox rgrSorting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton rbtSortByAccount;
        private System.Windows.Forms.RadioButton rbtSortByCostCentre;
        private System.Windows.Forms.RadioButton rbtSortByReference;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TextBox txtReferenceFrom;
        private System.Windows.Forms.Label lblReferenceFrom;
        private System.Windows.Forms.TextBox txtReferenceTo;
        private System.Windows.Forms.Label lblReferenceTo;
        private System.Windows.Forms.RadioButton rbtSortByAnalysisType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TextBox txtAnalysisTypeFrom;
        private System.Windows.Forms.Label lblAnalysisTypeFrom;
        private System.Windows.Forms.TextBox txtAnalysisTypeTo;
        private System.Windows.Forms.Label lblAnalysisTypeTo;
        private System.Windows.Forms.TabPage tpgCCAccount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.GroupBox rgrAccounts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.RadioButton rbtAccountRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountStart;
        private System.Windows.Forms.Label lblAccountStart;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountEnd;
        private System.Windows.Forms.Label lblAccountEnd;
        private System.Windows.Forms.RadioButton rbtAccountList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private Ict.Common.Controls.TClbVersatile clbAccounts;
        private System.Windows.Forms.Button btnUnselectAllAccounts;
        private System.Windows.Forms.GroupBox rgrCostCentres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.RadioButton rbtCostCentreRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCostCentreStart;
        private System.Windows.Forms.Label lblCostCentreStart;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCostCentreEnd;
        private System.Windows.Forms.Label lblCostCentreEnd;
        private System.Windows.Forms.RadioButton rbtCostCentreList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private Ict.Common.Controls.TClbVersatile clbCostCentres;
        private System.Windows.Forms.Button btnUnselectAllCostCentres;
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
