// auto generated with nant generateWinforms from FDIncomeByFund.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    partial class TFrmFDIncomeByFund
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmFDIncomeByFund));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgReportSpecific = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedger = new System.Windows.Forms.TextBox();
            this.lblLedger = new System.Windows.Forms.Label();
            this.rgrPeriod = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPeriodRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtStartPeriod = new System.Windows.Forms.TextBox();
            this.lblStartPeriod = new System.Windows.Forms.Label();
            this.txtEndPeriod = new System.Windows.Forms.TextBox();
            this.lblEndPeriod = new System.Windows.Forms.Label();
            this.cmbPeriodYear = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPeriodYear = new System.Windows.Forms.Label();
            this.rbtQuarter = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtQuarter = new System.Windows.Forms.TextBox();
            this.lblQuarter = new System.Windows.Forms.Label();
            this.cmbPeriodYearQuarter = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPeriodYearQuarter = new System.Windows.Forms.Label();
            this.cmbDepth = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDepth = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbGenerateReport = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettings = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettingsAs = new System.Windows.Forms.ToolStripButton();
            this.tbbLoadSettingsDialog = new System.Windows.Forms.ToolStripButton();
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
            this.mniWrapColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGenerateReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.tabReportSettings.SuspendLayout();
            this.tpgReportSpecific.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrPeriod.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            // txtLedger
            //
            this.txtLedger.Location = new System.Drawing.Point(2,2);
            this.txtLedger.Name = "txtLedger";
            this.txtLedger.Size = new System.Drawing.Size(150, 28);
            this.txtLedger.ReadOnly = true;
            this.txtLedger.TabStop = false;
            //
            // lblLedger
            //
            this.lblLedger.Location = new System.Drawing.Point(2,2);
            this.lblLedger.Name = "lblLedger";
            this.lblLedger.AutoSize = true;
            this.lblLedger.Text = "Ledger:";
            this.lblLedger.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLedger.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLedger.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // rgrPeriod
            //
            this.rgrPeriod.Name = "rgrPeriod";
            this.rgrPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrPeriod.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrPeriod.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtPeriodRange
            //
            this.rbtPeriodRange.Location = new System.Drawing.Point(2,2);
            this.rbtPeriodRange.Name = "rbtPeriodRange";
            this.rbtPeriodRange.AutoSize = true;
            this.rbtPeriodRange.Text = "Period Range";
            this.rbtPeriodRange.Checked = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
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
            this.lblStartPeriod.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.lblEndPeriod.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.lblPeriodYear.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblStartPeriod, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtStartPeriod, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblEndPeriod, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtEndPeriod, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblPeriodYear, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbPeriodYear, 5, 0);
            this.rbtPeriodRange.CheckedChanged += new System.EventHandler(this.rbtPeriodRangeCheckedChanged);
            //
            // rbtQuarter
            //
            this.rbtQuarter.Location = new System.Drawing.Point(2,2);
            this.rbtQuarter.Name = "rbtQuarter";
            this.rbtQuarter.AutoSize = true;
            this.rbtQuarter.Text = "Quarter";
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            //
            // txtQuarter
            //
            this.txtQuarter.Location = new System.Drawing.Point(2,2);
            this.txtQuarter.Name = "txtQuarter";
            this.txtQuarter.Size = new System.Drawing.Size(30, 28);
            //
            // lblQuarter
            //
            this.lblQuarter.Location = new System.Drawing.Point(2,2);
            this.lblQuarter.Name = "lblQuarter";
            this.lblQuarter.AutoSize = true;
            this.lblQuarter.Text = "Quarter:";
            this.lblQuarter.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblQuarter.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblQuarter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbPeriodYearQuarter
            //
            this.cmbPeriodYearQuarter.Location = new System.Drawing.Point(2,2);
            this.cmbPeriodYearQuarter.Name = "cmbPeriodYearQuarter";
            this.cmbPeriodYearQuarter.Size = new System.Drawing.Size(300, 28);
            this.cmbPeriodYearQuarter.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblPeriodYearQuarter
            //
            this.lblPeriodYearQuarter.Location = new System.Drawing.Point(2,2);
            this.lblPeriodYearQuarter.Name = "lblPeriodYearQuarter";
            this.lblPeriodYearQuarter.AutoSize = true;
            this.lblPeriodYearQuarter.Text = "Year:";
            this.lblPeriodYearQuarter.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPeriodYearQuarter.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPeriodYearQuarter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblQuarter, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtQuarter, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPeriodYearQuarter, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbPeriodYearQuarter, 3, 0);
            this.rbtQuarter.CheckedChanged += new System.EventHandler(this.rbtQuarterCheckedChanged);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtPeriodRange, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtQuarter, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.rgrPeriod.Text = "Period";
            //
            // cmbDepth
            //
            this.cmbDepth.Location = new System.Drawing.Point(2,2);
            this.cmbDepth.Name = "cmbDepth";
            this.cmbDepth.Size = new System.Drawing.Size(150, 28);
            this.cmbDepth.Items.AddRange(new object[] {"summary","standard","detail"});
            this.cmbDepth.Text = "standard";
            //
            // lblDepth
            //
            this.lblDepth.Location = new System.Drawing.Point(2,2);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.AutoSize = true;
            this.lblDepth.Text = "Depth:";
            this.lblDepth.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDepth.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDepth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedger, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.rgrPeriod, 2);
            this.tableLayoutPanel1.Controls.Add(this.rgrPeriod, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDepth, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLedger, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbDepth, 1, 2);
            this.tpgReportSpecific.Text = "Report parameters";
            this.tpgReportSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgReportSpecific);
            this.tabReportSettings.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
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
            // tbbLoadSettingsDialog
            //
            this.tbbLoadSettingsDialog.Name = "tbbLoadSettingsDialog";
            this.tbbLoadSettingsDialog.AutoSize = true;
            this.tbbLoadSettingsDialog.Click += new System.EventHandler(this.actLoadSettingsDialog);
            this.tbbLoadSettingsDialog.Text = "&Open...";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbGenerateReport,
                        tbbSaveSettings,
                        tbbSaveSettingsAs,
                        tbbLoadSettingsDialog});
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
            // mniWrapColumn
            //
            this.mniWrapColumn.Name = "mniWrapColumn";
            this.mniWrapColumn.AutoSize = true;
            this.mniWrapColumn.Click += new System.EventHandler(this.actWrapColumn);
            this.mniWrapColumn.Text = "&Wrap Columns";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
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
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
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
                        mniWrapColumn,
                        mniSeparator2,
                        mniGenerateReport,
                        mniSeparator3,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
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
                        mniSeparator4,
                        mniHelpBugReport,
                        mniSeparator5,
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
            // TFrmFDIncomeByFund
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.tabReportSettings);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmFDIncomeByFund";
            this.Text = "FD Income by Fund";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrPeriod.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpgReportSpecific.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgReportSpecific;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtLedger;
        private System.Windows.Forms.Label lblLedger;
        private System.Windows.Forms.GroupBox rgrPeriod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtPeriodRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtStartPeriod;
        private System.Windows.Forms.Label lblStartPeriod;
        private System.Windows.Forms.TextBox txtEndPeriod;
        private System.Windows.Forms.Label lblEndPeriod;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPeriodYear;
        private System.Windows.Forms.Label lblPeriodYear;
        private System.Windows.Forms.RadioButton rbtQuarter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtQuarter;
        private System.Windows.Forms.Label lblQuarter;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPeriodYearQuarter;
        private System.Windows.Forms.Label lblPeriodYearQuarter;
        private Ict.Common.Controls.TCmbAutoComplete cmbDepth;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbGenerateReport;
        private System.Windows.Forms.ToolStripButton tbbSaveSettings;
        private System.Windows.Forms.ToolStripButton tbbSaveSettingsAs;
        private System.Windows.Forms.ToolStripButton tbbLoadSettingsDialog;
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
        private System.Windows.Forms.ToolStripMenuItem mniWrapColumn;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniGenerateReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
