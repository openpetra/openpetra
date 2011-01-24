// auto generated with nant generateWinforms from FieldReports.yaml
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    partial class TFrmFieldReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmFieldReports));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgGeneralSettings = new System.Windows.Forms.TabPage();
            this.ucoConferenceSelection = new Ict.Petra.Client.MReporting.Gui.MConference.TFrmUC_ConferenceSelection();
            this.tpgAdditionalSettings = new System.Windows.Forms.TabPage();
            this.grpSelectFields = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdFields = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtFull = new System.Windows.Forms.RadioButton();
            this.rbtSummaries = new System.Windows.Forms.RadioButton();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.chkFinancialReport = new System.Windows.Forms.CheckBox();
            this.lblFinancialReport = new System.Windows.Forms.Label();
            this.chkAcceptedApplications = new System.Windows.Forms.CheckBox();
            this.lblAcceptedApplications = new System.Windows.Forms.Label();
            this.chkExtraCosts = new System.Windows.Forms.CheckBox();
            this.lblExtraCosts = new System.Windows.Forms.Label();
            this.cmbSignOffLines = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSignOffLines = new System.Windows.Forms.Label();
            this.grpChargedFields = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbChargedFields = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblChargedFields = new System.Windows.Forms.Label();
            this.tpgColumns = new System.Windows.Forms.TabPage();
            this.ucoReportColumns = new Ict.Petra.Client.MReporting.Gui.TFrmUC_PartnerColumns();
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
            this.tpgGeneralSettings.SuspendLayout();
            this.tpgAdditionalSettings.SuspendLayout();
            this.grpSelectFields.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.grpChargedFields.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tpgColumns.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // tpgGeneralSettings
            //
            this.tpgGeneralSettings.Location = new System.Drawing.Point(2,2);
            this.tpgGeneralSettings.Name = "tpgGeneralSettings";
            this.tpgGeneralSettings.AutoSize = true;
            this.tpgGeneralSettings.Controls.Add(this.ucoConferenceSelection);
            //
            // ucoConferenceSelection
            //
            this.ucoConferenceSelection.Name = "ucoConferenceSelection";
            this.ucoConferenceSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgGeneralSettings.Text = "General Settings";
            this.tpgGeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgAdditionalSettings
            //
            this.tpgAdditionalSettings.Location = new System.Drawing.Point(2,2);
            this.tpgAdditionalSettings.Name = "tpgAdditionalSettings";
            this.tpgAdditionalSettings.AutoSize = true;
            this.tpgAdditionalSettings.Controls.Add(this.grpChargedFields);
            this.tpgAdditionalSettings.Controls.Add(this.grpOptions);
            this.tpgAdditionalSettings.Controls.Add(this.grpMode);
            this.tpgAdditionalSettings.Controls.Add(this.grpSelectFields);
            //
            // grpSelectFields
            //
            this.grpSelectFields.Name = "grpSelectFields";
            this.grpSelectFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSelectFields.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpSelectFields.Controls.Add(this.tableLayoutPanel1);
            //
            // grdFields
            //
            this.grdFields.Location = new System.Drawing.Point(2,2);
            this.grdFields.Name = "grdFields";
            this.grdFields.Size = new System.Drawing.Size(500, 120);
            this.grdFields.DoubleClick += new System.EventHandler(this.grdFieldDoubleClick);
            //
            // pnlButtons
            //
            this.pnlButtons.Location = new System.Drawing.Point(2,2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel2);
            //
            // btnSelectAll
            //
            this.btnSelectAll.Location = new System.Drawing.Point(2,2);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.AutoSize = true;
            this.btnSelectAll.Click += new System.EventHandler(this.SelectAll);
            this.btnSelectAll.Text = "Select All";
            //
            // btnDeselectAll
            //
            this.btnDeselectAll.Location = new System.Drawing.Point(2,2);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.AutoSize = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.DeselectAll);
            this.btnDeselectAll.Text = "Deselect All";
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnSelectAll, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDeselectAll, 1, 0);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.grdFields, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlButtons, 0, 1);
            this.grpSelectFields.Text = "Select Fields";
            //
            // grpMode
            //
            this.grpMode.Name = "grpMode";
            this.grpMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMode.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpMode.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtFull
            //
            this.rbtFull.Location = new System.Drawing.Point(2,2);
            this.rbtFull.Name = "rbtFull";
            this.rbtFull.AutoSize = true;
            this.rbtFull.Text = "Full";
            //
            // rbtSummaries
            //
            this.rbtSummaries.Location = new System.Drawing.Point(2,2);
            this.rbtSummaries.Name = "rbtSummaries";
            this.rbtSummaries.AutoSize = true;
            this.rbtSummaries.Text = "Summaries";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtFull, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtSummaries, 0, 1);
            this.grpMode.Text = "Mode";
            //
            // grpOptions
            //
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.grpOptions.Controls.Add(this.tableLayoutPanel4);
            //
            // chkFinancialReport
            //
            this.chkFinancialReport.Location = new System.Drawing.Point(2,2);
            this.chkFinancialReport.Name = "chkFinancialReport";
            this.chkFinancialReport.Size = new System.Drawing.Size(30, 28);
            this.chkFinancialReport.Text = "";
            this.chkFinancialReport.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblFinancialReport
            //
            this.lblFinancialReport.Location = new System.Drawing.Point(2,2);
            this.lblFinancialReport.Name = "lblFinancialReport";
            this.lblFinancialReport.AutoSize = true;
            this.lblFinancialReport.Text = "Financial Report:";
            this.lblFinancialReport.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFinancialReport.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFinancialReport.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkAcceptedApplications
            //
            this.chkAcceptedApplications.Location = new System.Drawing.Point(2,2);
            this.chkAcceptedApplications.Name = "chkAcceptedApplications";
            this.chkAcceptedApplications.Size = new System.Drawing.Size(30, 28);
            this.chkAcceptedApplications.Text = "";
            this.chkAcceptedApplications.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblAcceptedApplications
            //
            this.lblAcceptedApplications.Location = new System.Drawing.Point(2,2);
            this.lblAcceptedApplications.Name = "lblAcceptedApplications";
            this.lblAcceptedApplications.AutoSize = true;
            this.lblAcceptedApplications.Text = "Accepted Applications Only:";
            this.lblAcceptedApplications.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAcceptedApplications.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAcceptedApplications.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkExtraCosts
            //
            this.chkExtraCosts.Location = new System.Drawing.Point(2,2);
            this.chkExtraCosts.Name = "chkExtraCosts";
            this.chkExtraCosts.Size = new System.Drawing.Size(30, 28);
            this.chkExtraCosts.Text = "";
            this.chkExtraCosts.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblExtraCosts
            //
            this.lblExtraCosts.Location = new System.Drawing.Point(2,2);
            this.lblExtraCosts.Name = "lblExtraCosts";
            this.lblExtraCosts.AutoSize = true;
            this.lblExtraCosts.Text = "List Extra Costs:";
            this.lblExtraCosts.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblExtraCosts.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblExtraCosts.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbSignOffLines
            //
            this.cmbSignOffLines.Location = new System.Drawing.Point(2,2);
            this.cmbSignOffLines.Name = "cmbSignOffLines";
            this.cmbSignOffLines.Size = new System.Drawing.Size(150, 28);
            this.cmbSignOffLines.Items.AddRange(new object[] {"no Sign Off Lines","print Financial Sign Off Lines","print Attendance Sign Off Lines"});
            //
            // lblSignOffLines
            //
            this.lblSignOffLines.Location = new System.Drawing.Point(2,2);
            this.lblSignOffLines.Name = "lblSignOffLines";
            this.lblSignOffLines.AutoSize = true;
            this.lblSignOffLines.Text = "Sign Off Lines:";
            this.lblSignOffLines.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSignOffLines.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSignOffLines.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblFinancialReport, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblAcceptedApplications, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblExtraCosts, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblSignOffLines, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.chkFinancialReport, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkAcceptedApplications, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.chkExtraCosts, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbSignOffLines, 1, 3);
            this.grpOptions.Text = "Options";
            //
            // grpChargedFields
            //
            this.grpChargedFields.Name = "grpChargedFields";
            this.grpChargedFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpChargedFields.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.grpChargedFields.Controls.Add(this.tableLayoutPanel5);
            //
            // cmbChargedFields
            //
            this.cmbChargedFields.Location = new System.Drawing.Point(2,2);
            this.cmbChargedFields.Name = "cmbChargedFields";
            this.cmbChargedFields.Size = new System.Drawing.Size(150, 28);
            this.cmbChargedFields.Items.AddRange(new object[] {"leave data as it is","charge Sending Field","charge Receiving Field","charge Registering Field"});
            //
            // lblChargedFields
            //
            this.lblChargedFields.Location = new System.Drawing.Point(2,2);
            this.lblChargedFields.Name = "lblChargedFields";
            this.lblChargedFields.AutoSize = true;
            this.lblChargedFields.Text = "If charged field is not set:";
            this.lblChargedFields.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblChargedFields.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblChargedFields.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblChargedFields, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmbChargedFields, 1, 0);
            this.grpChargedFields.Text = "Charged Fields";
            this.tpgAdditionalSettings.Text = "Additional Settings";
            this.tpgAdditionalSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgColumns
            //
            this.tpgColumns.Location = new System.Drawing.Point(2,2);
            this.tpgColumns.Name = "tpgColumns";
            this.tpgColumns.AutoSize = true;
            this.tpgColumns.Controls.Add(this.ucoReportColumns);
            //
            // ucoReportColumns
            //
            this.ucoReportColumns.Name = "ucoReportColumns";
            this.ucoReportColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgColumns.Text = "Columns";
            this.tpgColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgGeneralSettings);
            this.tabReportSettings.Controls.Add(this.tpgAdditionalSettings);
            this.tabReportSettings.Controls.Add(this.tpgColumns);
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
            // TFrmFieldReports
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 550);

            this.Controls.Add(this.tabReportSettings);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmFieldReports";
            this.Text = "";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tpgColumns.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.grpChargedFields.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpMode.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpSelectFields.ResumeLayout(false);
            this.tpgAdditionalSettings.ResumeLayout(false);
            this.tpgGeneralSettings.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgGeneralSettings;
        private Ict.Petra.Client.MReporting.Gui.MConference.TFrmUC_ConferenceSelection ucoConferenceSelection;
        private System.Windows.Forms.TabPage tpgAdditionalSettings;
        private System.Windows.Forms.GroupBox grpSelectFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Ict.Common.Controls.TSgrdDataGridPaged grdFields;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtFull;
        private System.Windows.Forms.RadioButton rbtSummaries;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox chkFinancialReport;
        private System.Windows.Forms.Label lblFinancialReport;
        private System.Windows.Forms.CheckBox chkAcceptedApplications;
        private System.Windows.Forms.Label lblAcceptedApplications;
        private System.Windows.Forms.CheckBox chkExtraCosts;
        private System.Windows.Forms.Label lblExtraCosts;
        private Ict.Common.Controls.TCmbAutoComplete cmbSignOffLines;
        private System.Windows.Forms.Label lblSignOffLines;
        private System.Windows.Forms.GroupBox grpChargedFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Ict.Common.Controls.TCmbAutoComplete cmbChargedFields;
        private System.Windows.Forms.Label lblChargedFields;
        private System.Windows.Forms.TabPage tpgColumns;
        private Ict.Petra.Client.MReporting.Gui.TFrmUC_PartnerColumns ucoReportColumns;
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
