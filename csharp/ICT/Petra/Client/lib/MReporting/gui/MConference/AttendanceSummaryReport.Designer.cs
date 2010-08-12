// auto generated with nant generateWinforms from AttendanceSummaryReport.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    partial class TFrmAttendanceSummaryReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAttendanceSummaryReport));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgGeneralSettings = new System.Windows.Forms.TabPage();
            this.ucoConferenceSelection = new Ict.Petra.Client.MReporting.Gui.MConference.TFrmUC_ConferenceSelection();
            this.tpgDateSettings = new System.Windows.Forms.TabPage();
            this.grpConferenceDate = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpConferenceStartDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpConferenceEndDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.grpArrivalDepartureDates = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblEarliestArrival = new System.Windows.Forms.Label();
            this.dtpEarliestArrivalDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblLatestDeparture = new System.Windows.Forms.Label();
            this.dtpLatestDepartureDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.grpSelectDateRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFromDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpToDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
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
            this.tpgDateSettings.SuspendLayout();
            this.grpConferenceDate.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpArrivalDepartureDates.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpSelectDateRange.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
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
            // tpgDateSettings
            //
            this.tpgDateSettings.Location = new System.Drawing.Point(2,2);
            this.tpgDateSettings.Name = "tpgDateSettings";
            this.tpgDateSettings.AutoSize = true;
            this.tpgDateSettings.Controls.Add(this.grpSelectDateRange);
            this.tpgDateSettings.Controls.Add(this.grpArrivalDepartureDates);
            this.tpgDateSettings.Controls.Add(this.grpConferenceDate);
            //
            // grpConferenceDate
            //
            this.grpConferenceDate.Name = "grpConferenceDate";
            this.grpConferenceDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpConferenceDate.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpConferenceDate.Controls.Add(this.tableLayoutPanel1);
            //
            // lblStartDate
            //
            this.lblStartDate.Location = new System.Drawing.Point(2,2);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpConferenceStartDate
            //
            this.dtpConferenceStartDate.Location = new System.Drawing.Point(2,2);
            this.dtpConferenceStartDate.Name = "dtpConferenceStartDate";
            this.dtpConferenceStartDate.Size = new System.Drawing.Size(94, 28);
            this.dtpConferenceStartDate.Enabled = false;
            //
            // lblEndDate
            //
            this.lblEndDate.Location = new System.Drawing.Point(2,2);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpConferenceEndDate
            //
            this.dtpConferenceEndDate.Location = new System.Drawing.Point(2,2);
            this.dtpConferenceEndDate.Name = "dtpConferenceEndDate";
            this.dtpConferenceEndDate.Size = new System.Drawing.Size(94, 28);
            this.dtpConferenceEndDate.Enabled = false;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 200));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 100));
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblStartDate, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblEndDate, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dtpConferenceStartDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtpConferenceEndDate, 1, 1);
            this.grpConferenceDate.Text = "Conference Date";
            //
            // grpArrivalDepartureDates
            //
            this.grpArrivalDepartureDates.Name = "grpArrivalDepartureDates";
            this.grpArrivalDepartureDates.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpArrivalDepartureDates.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpArrivalDepartureDates.Controls.Add(this.tableLayoutPanel2);
            //
            // lblEarliestArrival
            //
            this.lblEarliestArrival.Location = new System.Drawing.Point(2,2);
            this.lblEarliestArrival.Name = "lblEarliestArrival";
            this.lblEarliestArrival.AutoSize = true;
            this.lblEarliestArrival.Text = "Earliest Arrival:";
            this.lblEarliestArrival.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpEarliestArrivalDate
            //
            this.dtpEarliestArrivalDate.Location = new System.Drawing.Point(2,2);
            this.dtpEarliestArrivalDate.Name = "dtpEarliestArrivalDate";
            this.dtpEarliestArrivalDate.Size = new System.Drawing.Size(94, 28);
            this.dtpEarliestArrivalDate.Enabled = false;
            //
            // lblLatestDeparture
            //
            this.lblLatestDeparture.Location = new System.Drawing.Point(2,2);
            this.lblLatestDeparture.Name = "lblLatestDeparture";
            this.lblLatestDeparture.AutoSize = true;
            this.lblLatestDeparture.Text = "Latest Departure:";
            this.lblLatestDeparture.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpLatestDepartureDate
            //
            this.dtpLatestDepartureDate.Location = new System.Drawing.Point(2,2);
            this.dtpLatestDepartureDate.Name = "dtpLatestDepartureDate";
            this.dtpLatestDepartureDate.Size = new System.Drawing.Size(94, 28);
            this.dtpLatestDepartureDate.Enabled = false;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 200));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 100));
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblEarliestArrival, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblLatestDeparture, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpEarliestArrivalDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpLatestDepartureDate, 1, 1);
            this.grpArrivalDepartureDates.Text = "Arrival / Departure Dates";
            //
            // grpSelectDateRange
            //
            this.grpSelectDateRange.Name = "grpSelectDateRange";
            this.grpSelectDateRange.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSelectDateRange.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpSelectDateRange.Controls.Add(this.tableLayoutPanel3);
            //
            // lblFrom
            //
            this.lblFrom.Location = new System.Drawing.Point(2,2);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.AutoSize = true;
            this.lblFrom.Text = "From:";
            this.lblFrom.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpFromDate
            //
            this.dtpFromDate.Location = new System.Drawing.Point(2,2);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblTo
            //
            this.lblTo.Location = new System.Drawing.Point(2,2);
            this.lblTo.Name = "lblTo";
            this.lblTo.AutoSize = true;
            this.lblTo.Text = "To:";
            this.lblTo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpToDate
            //
            this.dtpToDate.Location = new System.Drawing.Point(2,2);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(94, 28);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 200));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblFrom, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblTo, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.dtpFromDate, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.dtpToDate, 1, 1);
            this.grpSelectDateRange.Text = "Select Date Range";
            this.tpgDateSettings.Text = "Date Settings";
            this.tpgDateSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgGeneralSettings);
            this.tabReportSettings.Controls.Add(this.tpgDateSettings);
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
            // TFrmAttendanceSummaryReport
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

            this.Name = "TFrmAttendanceSummaryReport";
            this.Text = "Attendance Summary Report";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpSelectDateRange.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpArrivalDepartureDates.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpConferenceDate.ResumeLayout(false);
            this.tpgDateSettings.ResumeLayout(false);
            this.tpgGeneralSettings.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgGeneralSettings;
        private Ict.Petra.Client.MReporting.Gui.MConference.TFrmUC_ConferenceSelection ucoConferenceSelection;
        private System.Windows.Forms.TabPage tpgDateSettings;
        private System.Windows.Forms.GroupBox grpConferenceDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblStartDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpConferenceStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpConferenceEndDate;
        private System.Windows.Forms.GroupBox grpArrivalDepartureDates;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblEarliestArrival;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpEarliestArrivalDate;
        private System.Windows.Forms.Label lblLatestDeparture;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpLatestDepartureDate;
        private System.Windows.Forms.GroupBox grpSelectDateRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblFrom;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpFromDate;
        private System.Windows.Forms.Label lblTo;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpToDate;
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
