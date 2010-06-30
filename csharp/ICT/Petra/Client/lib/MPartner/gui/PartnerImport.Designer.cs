// auto generated with nant generateWinforms from PartnerImport.yaml
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
    partial class TFrmPartnerImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerImport));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlImportSettings = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.chkSemiAutomatic = new System.Windows.Forms.CheckBox();
            this.pnlImportRecord = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtCurrentRecordStatus = new System.Windows.Forms.TextBox();
            this.lblCurrentRecordStatus = new System.Windows.Forms.Label();
            this.grdParsedValues = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtExplanation = new System.Windows.Forms.TextBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblTakeAction = new System.Windows.Forms.Label();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnCreateNewFamilyAndPerson = new System.Windows.Forms.Button();
            this.btnUseSelectedPerson = new System.Windows.Forms.Button();
            this.btnCreateNewPersonForSelectedFamily = new System.Windows.Forms.Button();
            this.btnCreateNewFamily = new System.Windows.Forms.Button();
            this.btnUseSelectedFamily = new System.Windows.Forms.Button();
            this.btnFindOtherPerson = new System.Windows.Forms.Button();
            this.btnFindOtherFamily = new System.Windows.Forms.Button();
            this.chkReplaceAddress = new System.Windows.Forms.CheckBox();
            this.grdMatchingRecords = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbStartImport = new System.Windows.Forms.ToolStripButton();
            this.tbbCancelImport = new System.Windows.Forms.ToolStripButton();
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
            this.pnlImportSettings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlImportRecord.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            // pnlImportSettings
            //
            this.pnlImportSettings.Name = "pnlImportSettings";
            this.pnlImportSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlImportSettings.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlImportSettings.Controls.Add(this.tableLayoutPanel2);
            //
            // txtFilename
            //
            this.txtFilename.Location = new System.Drawing.Point(2,2);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(300, 28);
            this.txtFilename.ReadOnly = true;
            this.txtFilename.TabStop = false;
            //
            // lblFilename
            //
            this.lblFilename.Location = new System.Drawing.Point(2,2);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.AutoSize = true;
            this.lblFilename.Text = "Filename:";
            this.lblFilename.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFilename.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFilename.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // btnSelectFile
            //
            this.btnSelectFile.Location = new System.Drawing.Point(2,2);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.AutoSize = true;
            this.btnSelectFile.Click += new System.EventHandler(this.OpenFile);
            this.btnSelectFile.Text = "Select File";
            //
            // chkSemiAutomatic
            //
            this.chkSemiAutomatic.Location = new System.Drawing.Point(2,2);
            this.chkSemiAutomatic.Name = "chkSemiAutomatic";
            this.chkSemiAutomatic.AutoSize = true;
            this.chkSemiAutomatic.Text = "Semi Automatic Import";
            this.chkSemiAutomatic.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblFilename, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.chkSemiAutomatic, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkSemiAutomatic, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtFilename, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSelectFile, 2, 0);
            //
            // pnlImportRecord
            //
            this.pnlImportRecord.Location = new System.Drawing.Point(2,2);
            this.pnlImportRecord.Name = "pnlImportRecord";
            this.pnlImportRecord.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlImportRecord.Controls.Add(this.tableLayoutPanel3);
            //
            // txtCurrentRecordStatus
            //
            this.txtCurrentRecordStatus.Location = new System.Drawing.Point(2,2);
            this.txtCurrentRecordStatus.Name = "txtCurrentRecordStatus";
            this.txtCurrentRecordStatus.Size = new System.Drawing.Size(500, 28);
            this.txtCurrentRecordStatus.ReadOnly = true;
            this.txtCurrentRecordStatus.TabStop = false;
            //
            // lblCurrentRecordStatus
            //
            this.lblCurrentRecordStatus.Location = new System.Drawing.Point(2,2);
            this.lblCurrentRecordStatus.Name = "lblCurrentRecordStatus";
            this.lblCurrentRecordStatus.AutoSize = true;
            this.lblCurrentRecordStatus.Text = "Current Import File Record Status:";
            this.lblCurrentRecordStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCurrentRecordStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCurrentRecordStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // grdParsedValues
            //
            this.grdParsedValues.Location = new System.Drawing.Point(2,2);
            this.grdParsedValues.Name = "grdParsedValues";
            this.grdParsedValues.Size = new System.Drawing.Size(400, 300);
            //
            // pnlActions
            //
            this.pnlActions.Location = new System.Drawing.Point(2,2);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlActions.Controls.Add(this.tableLayoutPanel4);
            //
            // txtExplanation
            //
            this.txtExplanation.Location = new System.Drawing.Point(2,2);
            this.txtExplanation.Name = "txtExplanation";
            this.txtExplanation.Size = new System.Drawing.Size(150, 28);
            this.txtExplanation.ReadOnly = true;
            this.txtExplanation.TabStop = false;
            this.txtExplanation.Multiline = true;
            //
            // lblExplanation
            //
            this.lblExplanation.Location = new System.Drawing.Point(2,2);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.AutoSize = true;
            this.lblExplanation.Text = "Explanation:";
            this.lblExplanation.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblExplanation.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblExplanation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblTakeAction
            //
            this.lblTakeAction.Location = new System.Drawing.Point(2,2);
            this.lblTakeAction.Name = "lblTakeAction";
            this.lblTakeAction.AutoSize = true;
            this.lblTakeAction.Text = "Take Action:";
            this.lblTakeAction.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // btnSkip
            //
            this.btnSkip.Location = new System.Drawing.Point(2,2);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.AutoSize = true;
            this.btnSkip.Click += new System.EventHandler(this.SkipRecord);
            this.btnSkip.Text = "Skip Record";
            //
            // btnCreateNewFamilyAndPerson
            //
            this.btnCreateNewFamilyAndPerson.Location = new System.Drawing.Point(2,2);
            this.btnCreateNewFamilyAndPerson.Name = "btnCreateNewFamilyAndPerson";
            this.btnCreateNewFamilyAndPerson.Enabled = false;
            this.btnCreateNewFamilyAndPerson.AutoSize = true;
            this.btnCreateNewFamilyAndPerson.Text = "Create new Family and Person";
            //
            // btnUseSelectedPerson
            //
            this.btnUseSelectedPerson.Location = new System.Drawing.Point(2,2);
            this.btnUseSelectedPerson.Name = "btnUseSelectedPerson";
            this.btnUseSelectedPerson.Enabled = false;
            this.btnUseSelectedPerson.AutoSize = true;
            this.btnUseSelectedPerson.Text = "Use selected Person in list below";
            //
            // btnCreateNewPersonForSelectedFamily
            //
            this.btnCreateNewPersonForSelectedFamily.Location = new System.Drawing.Point(2,2);
            this.btnCreateNewPersonForSelectedFamily.Name = "btnCreateNewPersonForSelectedFamily";
            this.btnCreateNewPersonForSelectedFamily.Enabled = false;
            this.btnCreateNewPersonForSelectedFamily.AutoSize = true;
            this.btnCreateNewPersonForSelectedFamily.Text = "Add as new Person to selected Family in list below";
            //
            // btnCreateNewFamily
            //
            this.btnCreateNewFamily.Location = new System.Drawing.Point(2,2);
            this.btnCreateNewFamily.Name = "btnCreateNewFamily";
            this.btnCreateNewFamily.AutoSize = true;
            this.btnCreateNewFamily.Click += new System.EventHandler(this.CreateNewFamily);
            this.btnCreateNewFamily.Text = "Create new Family Record only";
            //
            // btnUseSelectedFamily
            //
            this.btnUseSelectedFamily.Location = new System.Drawing.Point(2,2);
            this.btnUseSelectedFamily.Name = "btnUseSelectedFamily";
            this.btnUseSelectedFamily.Enabled = false;
            this.btnUseSelectedFamily.AutoSize = true;
            this.btnUseSelectedFamily.Text = "Use selected Family in list below";
            //
            // btnFindOtherPerson
            //
            this.btnFindOtherPerson.Location = new System.Drawing.Point(2,2);
            this.btnFindOtherPerson.Name = "btnFindOtherPerson";
            this.btnFindOtherPerson.Enabled = false;
            this.btnFindOtherPerson.AutoSize = true;
            this.btnFindOtherPerson.Text = "Find other Person to use for this Record...";
            //
            // btnFindOtherFamily
            //
            this.btnFindOtherFamily.Location = new System.Drawing.Point(2,2);
            this.btnFindOtherFamily.Name = "btnFindOtherFamily";
            this.btnFindOtherFamily.Enabled = false;
            this.btnFindOtherFamily.AutoSize = true;
            this.btnFindOtherFamily.Text = "Find other Family to add this Person to...";
            //
            // chkReplaceAddress
            //
            this.chkReplaceAddress.Location = new System.Drawing.Point(2,2);
            this.chkReplaceAddress.Name = "chkReplaceAddress";
            this.chkReplaceAddress.Enabled = false;
            this.chkReplaceAddress.AutoSize = true;
            this.chkReplaceAddress.Text = "Replace current address in list below with imported one";
            this.chkReplaceAddress.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 11;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblExplanation, 0, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.lblTakeAction, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblTakeAction, 0, 1);
            this.tableLayoutPanel4.SetColumnSpan(this.btnSkip, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnSkip, 0, 2);
            this.tableLayoutPanel4.SetColumnSpan(this.btnCreateNewFamilyAndPerson, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnCreateNewFamilyAndPerson, 0, 3);
            this.tableLayoutPanel4.SetColumnSpan(this.btnUseSelectedPerson, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnUseSelectedPerson, 0, 4);
            this.tableLayoutPanel4.SetColumnSpan(this.btnCreateNewPersonForSelectedFamily, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnCreateNewPersonForSelectedFamily, 0, 5);
            this.tableLayoutPanel4.SetColumnSpan(this.btnCreateNewFamily, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnCreateNewFamily, 0, 6);
            this.tableLayoutPanel4.SetColumnSpan(this.btnUseSelectedFamily, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnUseSelectedFamily, 0, 7);
            this.tableLayoutPanel4.SetColumnSpan(this.btnFindOtherPerson, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnFindOtherPerson, 0, 8);
            this.tableLayoutPanel4.SetColumnSpan(this.btnFindOtherFamily, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnFindOtherFamily, 0, 9);
            this.tableLayoutPanel4.SetColumnSpan(this.chkReplaceAddress, 2);
            this.tableLayoutPanel4.Controls.Add(this.chkReplaceAddress, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.txtExplanation, 1, 0);
            //
            // grdMatchingRecords
            //
            this.grdMatchingRecords.Location = new System.Drawing.Point(2,2);
            this.grdMatchingRecords.Name = "grdMatchingRecords";
            this.grdMatchingRecords.Size = new System.Drawing.Size(600, 100);
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblCurrentRecordStatus, 0, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.grdParsedValues, 2);
            this.tableLayoutPanel3.Controls.Add(this.grdParsedValues, 0, 1);
            this.tableLayoutPanel3.SetColumnSpan(this.grdMatchingRecords, 3);
            this.tableLayoutPanel3.Controls.Add(this.grdMatchingRecords, 0, 2);
            this.tableLayoutPanel3.SetColumnSpan(this.txtCurrentRecordStatus, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtCurrentRecordStatus, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnlActions, 2, 1);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlImportSettings, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlImportRecord, 0, 1);
            //
            // tbbStartImport
            //
            this.tbbStartImport.Name = "tbbStartImport";
            this.tbbStartImport.AutoSize = true;
            this.tbbStartImport.Click += new System.EventHandler(this.StartImport);
            this.tbbStartImport.Text = "Start Import";
            //
            // tbbCancelImport
            //
            this.tbbCancelImport.Name = "tbbCancelImport";
            this.tbbCancelImport.AutoSize = true;
            this.tbbCancelImport.Click += new System.EventHandler(this.CancelImport);
            this.tbbCancelImport.Text = "Cancel Import";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbStartImport,
                        tbbCancelImport});
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
            // TFrmPartnerImport
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(770, 700);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmPartnerImport";
            this.Text = "Import Partners";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlImportRecord.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlImportSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlImportSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.CheckBox chkSemiAutomatic;
        private System.Windows.Forms.Panel pnlImportRecord;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtCurrentRecordStatus;
        private System.Windows.Forms.Label lblCurrentRecordStatus;
        private Ict.Common.Controls.TSgrdDataGridPaged grdParsedValues;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtExplanation;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblTakeAction;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnCreateNewFamilyAndPerson;
        private System.Windows.Forms.Button btnUseSelectedPerson;
        private System.Windows.Forms.Button btnCreateNewPersonForSelectedFamily;
        private System.Windows.Forms.Button btnCreateNewFamily;
        private System.Windows.Forms.Button btnUseSelectedFamily;
        private System.Windows.Forms.Button btnFindOtherPerson;
        private System.Windows.Forms.Button btnFindOtherFamily;
        private System.Windows.Forms.CheckBox chkReplaceAddress;
        private Ict.Common.Controls.TSgrdDataGridPaged grdMatchingRecords;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbStartImport;
        private System.Windows.Forms.ToolStripButton tbbCancelImport;
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
