// auto generated with nant generateWinforms from RelationshipReport.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    partial class TFrmRelationshipReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmRelationshipReport));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgGeneralSettings = new System.Windows.Forms.TabPage();
            this.ucoPartnerSelection = new Ict.Petra.Client.MReporting.Gui.TFrmUC_PartnerSelection();
            this.tpgRelationshipSettings = new System.Windows.Forms.TabPage();
            this.grpSelectRelationship = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkCategoryFilter = new System.Windows.Forms.CheckBox();
            this.cmbRelationCategory = new Ict.Common.Controls.TCmbAutoComplete();
            this.rbtDirectRelationship = new System.Windows.Forms.RadioButton();
            this.rbtReciprocalRelationship = new System.Windows.Forms.RadioButton();
            this.lblSelectDirectRelationship = new System.Windows.Forms.Label();
            this.lblSelectReciprocalRelationship = new System.Windows.Forms.Label();
            this.grdDirectRelationship = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.grdReciprocalRelationship = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.grpMiscellaneous = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkActivePartners = new System.Windows.Forms.CheckBox();
            this.chkMailingAddressesOnly = new System.Windows.Forms.CheckBox();
            this.chkExcludeNoSolicitations = new System.Windows.Forms.CheckBox();
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
            this.tpgRelationshipSettings.SuspendLayout();
            this.grpSelectRelationship.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpMiscellaneous.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.tpgGeneralSettings.Controls.Add(this.ucoPartnerSelection);
            //
            // ucoPartnerSelection
            //
            this.ucoPartnerSelection.Name = "ucoPartnerSelection";
            this.ucoPartnerSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgGeneralSettings.Text = "General Settings";
            this.tpgGeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgRelationshipSettings
            //
            this.tpgRelationshipSettings.Location = new System.Drawing.Point(2,2);
            this.tpgRelationshipSettings.Name = "tpgRelationshipSettings";
            this.tpgRelationshipSettings.AutoSize = true;
            this.tpgRelationshipSettings.Controls.Add(this.grpMiscellaneous);
            this.tpgRelationshipSettings.Controls.Add(this.grpSelectRelationship);
            //
            // grpSelectRelationship
            //
            this.grpSelectRelationship.Name = "grpSelectRelationship";
            this.grpSelectRelationship.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSelectRelationship.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpSelectRelationship.Controls.Add(this.tableLayoutPanel1);
            //
            // chkCategoryFilter
            //
            this.chkCategoryFilter.Location = new System.Drawing.Point(2,2);
            this.chkCategoryFilter.Name = "chkCategoryFilter";
            this.chkCategoryFilter.Size = new System.Drawing.Size(300, 28);
            this.chkCategoryFilter.CheckedChanged += new System.EventHandler(this.FilterRelationCategoryChanged);
            this.chkCategoryFilter.Text = "Filter List by Relation Category:";
            this.chkCategoryFilter.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkCategoryFilter.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // cmbRelationCategory
            //
            this.cmbRelationCategory.Location = new System.Drawing.Point(2,2);
            this.cmbRelationCategory.Name = "cmbRelationCategory";
            this.cmbRelationCategory.Size = new System.Drawing.Size(150, 28);
            //
            // rbtDirectRelationship
            //
            this.rbtDirectRelationship.Location = new System.Drawing.Point(2,2);
            this.rbtDirectRelationship.Name = "rbtDirectRelationship";
            this.rbtDirectRelationship.AutoSize = true;
            this.rbtDirectRelationship.CheckedChanged += new System.EventHandler(this.rbtRelationshipDirectionChanged);
            this.rbtDirectRelationship.Text = "Use Relationship";
            //
            // rbtReciprocalRelationship
            //
            this.rbtReciprocalRelationship.Location = new System.Drawing.Point(2,2);
            this.rbtReciprocalRelationship.Name = "rbtReciprocalRelationship";
            this.rbtReciprocalRelationship.AutoSize = true;
            this.rbtReciprocalRelationship.Text = "Use Reciprocal Relationship";
            //
            // lblSelectDirectRelationship
            //
            this.lblSelectDirectRelationship.Location = new System.Drawing.Point(2,2);
            this.lblSelectDirectRelationship.Name = "lblSelectDirectRelationship";
            this.lblSelectDirectRelationship.AutoSize = true;
            this.lblSelectDirectRelationship.Text = "(Select from list below):";
            this.lblSelectDirectRelationship.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // lblSelectReciprocalRelationship
            //
            this.lblSelectReciprocalRelationship.Location = new System.Drawing.Point(2,2);
            this.lblSelectReciprocalRelationship.Name = "lblSelectReciprocalRelationship";
            this.lblSelectReciprocalRelationship.AutoSize = true;
            this.lblSelectReciprocalRelationship.Text = "(Select from list below):";
            this.lblSelectReciprocalRelationship.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // grdDirectRelationship
            //
            this.grdDirectRelationship.Location = new System.Drawing.Point(2,2);
            this.grdDirectRelationship.Name = "grdDirectRelationship";
            this.grdDirectRelationship.Size = new System.Drawing.Size(300, 200);
            //
            // grdReciprocalRelationship
            //
            this.grdReciprocalRelationship.Location = new System.Drawing.Point(2,2);
            this.grdReciprocalRelationship.Name = "grdReciprocalRelationship";
            this.grdReciprocalRelationship.Size = new System.Drawing.Size(300, 200);
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.chkCategoryFilter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtDirectRelationship, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectDirectRelationship, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdDirectRelationship, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbRelationCategory, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtReciprocalRelationship, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectReciprocalRelationship, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdReciprocalRelationship, 1, 3);
            this.grpSelectRelationship.Text = "Select Relationship";
            //
            // grpMiscellaneous
            //
            this.grpMiscellaneous.Name = "grpMiscellaneous";
            this.grpMiscellaneous.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMiscellaneous.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpMiscellaneous.Controls.Add(this.tableLayoutPanel2);
            //
            // chkActivePartners
            //
            this.chkActivePartners.Location = new System.Drawing.Point(2,2);
            this.chkActivePartners.Name = "chkActivePartners";
            this.chkActivePartners.AutoSize = true;
            this.chkActivePartners.Text = "Active Partners";
            this.chkActivePartners.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkActivePartners.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // chkMailingAddressesOnly
            //
            this.chkMailingAddressesOnly.Location = new System.Drawing.Point(2,2);
            this.chkMailingAddressesOnly.Name = "chkMailingAddressesOnly";
            this.chkMailingAddressesOnly.AutoSize = true;
            this.chkMailingAddressesOnly.Text = "Mailing Addresses only";
            this.chkMailingAddressesOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkMailingAddressesOnly.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // chkExcludeNoSolicitations
            //
            this.chkExcludeNoSolicitations.Location = new System.Drawing.Point(2,2);
            this.chkExcludeNoSolicitations.Name = "chkExcludeNoSolicitations";
            this.chkExcludeNoSolicitations.AutoSize = true;
            this.chkExcludeNoSolicitations.Text = "Exclude 'No Solicitations'";
            this.chkExcludeNoSolicitations.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkExcludeNoSolicitations.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.chkActivePartners, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkMailingAddressesOnly, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.chkExcludeNoSolicitations, 0, 2);
            this.grpMiscellaneous.Text = "Miscellaneous Settings";
            this.tpgRelationshipSettings.Text = "Select Relationship";
            this.tpgRelationshipSettings.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.tabReportSettings.Controls.Add(this.tpgRelationshipSettings);
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
            // TFrmRelationshipReport
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

            this.Name = "TFrmRelationshipReport";
            this.Text = "Relationship Report";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tpgColumns.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpMiscellaneous.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpSelectRelationship.ResumeLayout(false);
            this.tpgRelationshipSettings.ResumeLayout(false);
            this.tpgGeneralSettings.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgGeneralSettings;
        private Ict.Petra.Client.MReporting.Gui.TFrmUC_PartnerSelection ucoPartnerSelection;
        private System.Windows.Forms.TabPage tpgRelationshipSettings;
        private System.Windows.Forms.GroupBox grpSelectRelationship;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkCategoryFilter;
        private Ict.Common.Controls.TCmbAutoComplete cmbRelationCategory;
        private System.Windows.Forms.RadioButton rbtDirectRelationship;
        private System.Windows.Forms.RadioButton rbtReciprocalRelationship;
        private System.Windows.Forms.Label lblSelectDirectRelationship;
        private System.Windows.Forms.Label lblSelectReciprocalRelationship;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDirectRelationship;
        private Ict.Common.Controls.TSgrdDataGridPaged grdReciprocalRelationship;
        private System.Windows.Forms.GroupBox grpMiscellaneous;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox chkActivePartners;
        private System.Windows.Forms.CheckBox chkMailingAddressesOnly;
        private System.Windows.Forms.CheckBox chkExcludeNoSolicitations;
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
