// auto generated with nant generateWinforms from PartnerContactReport.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    partial class TFrmPartnerContactReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerContactReport));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgGeneralSettings = new System.Windows.Forms.TabPage();
            this.ucoPartnerSelection = new Ict.Petra.Client.MReporting.Gui.MPartner.TFrmUC_PartnerSelection();
            this.tpgReportSorting = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrSorting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPartnerName = new System.Windows.Forms.RadioButton();
            this.rbtPartnerKey = new System.Windows.Forms.RadioButton();
            this.tpgReportRange = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grpReportRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbContactor = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblContactor = new System.Windows.Forms.Label();
            this.cmbContact = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblContact = new System.Windows.Forms.Label();
            this.txtDateFrom = new System.Windows.Forms.TextBox();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.tpgContactAttributes = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.grdAttribute = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.grdDetail = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDummy = new System.Windows.Forms.Panel();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddDetail = new System.Windows.Forms.Button();
            this.btnRemoveDetail = new System.Windows.Forms.Button();
            this.grdSelection = new Ict.Common.Controls.TSgrdDataGridPaged();
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
            this.tpgReportSorting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrSorting.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tpgReportRange.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpReportRange.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tpgContactAttributes.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlDummy.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
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
            // tpgReportSorting
            //
            this.tpgReportSorting.Location = new System.Drawing.Point(2,2);
            this.tpgReportSorting.Name = "tpgReportSorting";
            this.tpgReportSorting.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.tpgReportSorting.Controls.Add(this.tableLayoutPanel1);
            //
            // rgrSorting
            //
            this.rgrSorting.Location = new System.Drawing.Point(2,2);
            this.rgrSorting.Name = "rgrSorting";
            this.rgrSorting.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrSorting.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtPartnerName
            //
            this.rbtPartnerName.Location = new System.Drawing.Point(2,2);
            this.rbtPartnerName.Name = "rbtPartnerName";
            this.rbtPartnerName.AutoSize = true;
            this.rbtPartnerName.Text = "Partner Name";
            this.rbtPartnerName.Checked = true;
            //
            // rbtPartnerKey
            //
            this.rbtPartnerKey.Location = new System.Drawing.Point(2,2);
            this.rbtPartnerKey.Name = "rbtPartnerKey";
            this.rbtPartnerKey.AutoSize = true;
            this.rbtPartnerKey.Text = "Partner Key";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtPartnerName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtPartnerKey, 0, 1);
            this.rgrSorting.Text = "Address Details";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.rgrSorting, 0, 0);
            this.tpgReportSorting.Text = "Sorting";
            this.tpgReportSorting.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgReportRange
            //
            this.tpgReportRange.Location = new System.Drawing.Point(2,2);
            this.tpgReportRange.Name = "tpgReportRange";
            this.tpgReportRange.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.tpgReportRange.Controls.Add(this.tableLayoutPanel3);
            //
            // grpReportRange
            //
            this.grpReportRange.Location = new System.Drawing.Point(2,2);
            this.grpReportRange.Name = "grpReportRange";
            this.grpReportRange.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.grpReportRange.Controls.Add(this.tableLayoutPanel4);
            //
            // cmbContactor
            //
            this.cmbContactor.Location = new System.Drawing.Point(2,2);
            this.cmbContactor.Name = "cmbContactor";
            this.cmbContactor.Size = new System.Drawing.Size(150, 28);
            //
            // lblContactor
            //
            this.lblContactor.Location = new System.Drawing.Point(2,2);
            this.lblContactor.Name = "lblContactor";
            this.lblContactor.AutoSize = true;
            this.lblContactor.Text = "Contactor::";
            this.lblContactor.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblContactor.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblContactor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbContact
            //
            this.cmbContact.Location = new System.Drawing.Point(2,2);
            this.cmbContact.Name = "cmbContact";
            this.cmbContact.Size = new System.Drawing.Size(150, 28);
            //
            // lblContact
            //
            this.lblContact.Location = new System.Drawing.Point(2,2);
            this.lblContact.Name = "lblContact";
            this.lblContact.AutoSize = true;
            this.lblContact.Text = "Contact:";
            this.lblContact.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblContact.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDateFrom
            //
            this.txtDateFrom.Location = new System.Drawing.Point(2,2);
            this.txtDateFrom.Name = "txtDateFrom";
            this.txtDateFrom.Size = new System.Drawing.Size(150, 28);
            //
            // lblDateFrom
            //
            this.lblDateFrom.Location = new System.Drawing.Point(2,2);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Text = "Date From::";
            this.lblDateFrom.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateFrom.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDateTo
            //
            this.txtDateTo.Location = new System.Drawing.Point(2,2);
            this.txtDateTo.Name = "txtDateTo";
            this.txtDateTo.Size = new System.Drawing.Size(150, 28);
            //
            // lblDateTo
            //
            this.lblDateTo.Location = new System.Drawing.Point(2,2);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Text = "Date To::";
            this.lblDateTo.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateTo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblContactor, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblContact, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblDateFrom, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblDateTo, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.cmbContactor, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbContact, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtDateFrom, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDateTo, 1, 3);
            this.grpReportRange.Text = "Report Range";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.grpReportRange, 0, 0);
            this.tpgReportRange.Text = "Report Range";
            this.tpgReportRange.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgContactAttributes
            //
            this.tpgContactAttributes.Location = new System.Drawing.Point(2,2);
            this.tpgContactAttributes.Name = "tpgContactAttributes";
            this.tpgContactAttributes.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.tpgContactAttributes.Controls.Add(this.tableLayoutPanel5);
            //
            // grdAttribute
            //
            this.grdAttribute.Name = "grdAttribute";
            this.grdAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAttribute.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.AttributeFocusedRowChanged);
            //
            // grdDetail
            //
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetail.DoubleClick += new System.EventHandler(this.grdDetailDoubleClick);
            //
            // pnlDummy
            //
            this.pnlDummy.Location = new System.Drawing.Point(2,2);
            this.pnlDummy.Name = "pnlDummy";
            this.pnlDummy.AutoSize = true;
            //
            // pnlButton
            //
            this.pnlButton.Location = new System.Drawing.Point(2,2);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.pnlButton.Controls.Add(this.tableLayoutPanel6);
            //
            // btnAddDetail
            //
            this.btnAddDetail.Location = new System.Drawing.Point(2,2);
            this.btnAddDetail.Name = "btnAddDetail";
            this.btnAddDetail.AutoSize = true;
            this.btnAddDetail.Click += new System.EventHandler(this.AddDetail);
            this.btnAddDetail.Text = "AddDetail";
            //
            // btnRemoveDetail
            //
            this.btnRemoveDetail.Location = new System.Drawing.Point(2,2);
            this.btnRemoveDetail.Name = "btnRemoveDetail";
            this.btnRemoveDetail.AutoSize = true;
            this.btnRemoveDetail.Click += new System.EventHandler(this.RemoveDetail);
            this.btnRemoveDetail.Text = "Remove Detail";
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.btnAddDetail, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnRemoveDetail, 1, 0);
            //
            // grdSelection
            //
            this.grdSelection.Name = "grdSelection";
            this.grdSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 35));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 65));
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.grdAttribute, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.pnlDummy, 0, 1);
            this.tableLayoutPanel5.SetColumnSpan(this.grdSelection, 2);
            this.tableLayoutPanel5.Controls.Add(this.grdSelection, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.grdDetail, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.pnlButton, 1, 1);
            this.tpgContactAttributes.Text = "Contact Attributes";
            this.tpgContactAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.tabReportSettings.Controls.Add(this.tpgReportSorting);
            this.tabReportSettings.Controls.Add(this.tpgReportRange);
            this.tabReportSettings.Controls.Add(this.tpgContactAttributes);
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
            // TFrmPartnerContactReport
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

            this.Name = "TFrmPartnerContactReport";
            this.Text = "Partner Contact Report";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tpgColumns.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.pnlDummy.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tpgContactAttributes.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.grpReportRange.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tpgReportRange.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrSorting.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpgReportSorting.ResumeLayout(false);
            this.tpgGeneralSettings.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgGeneralSettings;
        private Ict.Petra.Client.MReporting.Gui.MPartner.TFrmUC_PartnerSelection ucoPartnerSelection;
        private System.Windows.Forms.TabPage tpgReportSorting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox rgrSorting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtPartnerName;
        private System.Windows.Forms.RadioButton rbtPartnerKey;
        private System.Windows.Forms.TabPage tpgReportRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox grpReportRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Ict.Common.Controls.TCmbAutoComplete cmbContactor;
        private System.Windows.Forms.Label lblContactor;
        private Ict.Common.Controls.TCmbAutoComplete cmbContact;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtDateFrom;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.TabPage tpgContactAttributes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Ict.Common.Controls.TSgrdDataGridPaged grdAttribute;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetail;
        private System.Windows.Forms.Panel pnlDummy;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnAddDetail;
        private System.Windows.Forms.Button btnRemoveDetail;
        private Ict.Common.Controls.TSgrdDataGridPaged grdSelection;
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
