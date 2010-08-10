// auto generated with nant generateWinforms from FrequencySetup.yaml
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

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    partial class TFrmFrequencySetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmFrequencySetup));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailFrequencyCode = new System.Windows.Forms.TextBox();
            this.lblDetailFrequencyCode = new System.Windows.Forms.Label();
            this.txtDetailFrequencyDescription = new System.Windows.Forms.TextBox();
            this.lblDetailFrequencyDescription = new System.Windows.Forms.Label();
            this.pnlValues = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailNumberOfYears = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailNumberOfYears = new System.Windows.Forms.Label();
            this.txtDetailNumberOfMonths = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailNumberOfMonths = new System.Windows.Forms.Label();
            this.txtDetailNumberOfDays = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailNumberOfDays = new System.Windows.Forms.Label();
            this.txtDetailNumberOfHours = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailNumberOfHours = new System.Windows.Forms.Label();
            this.txtDetailNumberOfMinutes = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailNumberOfMinutes = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbNew = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoCurrentField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlValues.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.pnlGrid);
            this.pnlContent.Controls.Add(this.pnlDetails);
            //
            // pnlGrid
            //
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.AutoSize = true;
            this.pnlGrid.Controls.Add(this.grdDetails);
            this.pnlGrid.Controls.Add(this.pnlButtons);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            //
            // pnlButtons
            //
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel1);
            //
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRecord);
            this.btnNew.Text = "New";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnNew, 0, 0);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel2);
            //
            // txtDetailFrequencyCode
            //
            this.txtDetailFrequencyCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailFrequencyCode.Name = "txtDetailFrequencyCode";
            this.txtDetailFrequencyCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailFrequencyCode
            //
            this.lblDetailFrequencyCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailFrequencyCode.Name = "lblDetailFrequencyCode";
            this.lblDetailFrequencyCode.AutoSize = true;
            this.lblDetailFrequencyCode.Text = "Frequency Code:";
            this.lblDetailFrequencyCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailFrequencyCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailFrequencyCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailFrequencyDescription
            //
            this.txtDetailFrequencyDescription.Location = new System.Drawing.Point(2,2);
            this.txtDetailFrequencyDescription.Name = "txtDetailFrequencyDescription";
            this.txtDetailFrequencyDescription.Size = new System.Drawing.Size(160, 28);
            //
            // lblDetailFrequencyDescription
            //
            this.lblDetailFrequencyDescription.Location = new System.Drawing.Point(2,2);
            this.lblDetailFrequencyDescription.Name = "lblDetailFrequencyDescription";
            this.lblDetailFrequencyDescription.AutoSize = true;
            this.lblDetailFrequencyDescription.Text = "Frequency Description:";
            this.lblDetailFrequencyDescription.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailFrequencyDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailFrequencyDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlValues
            //
            this.pnlValues.Location = new System.Drawing.Point(2,2);
            this.pnlValues.Name = "pnlValues";
            this.pnlValues.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlValues.Controls.Add(this.tableLayoutPanel3);
            //
            // txtDetailNumberOfYears
            //
            this.txtDetailNumberOfYears.Location = new System.Drawing.Point(2,2);
            this.txtDetailNumberOfYears.Name = "txtDetailNumberOfYears";
            this.txtDetailNumberOfYears.Size = new System.Drawing.Size(30, 28);
            this.txtDetailNumberOfYears.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailNumberOfYears.DecimalPlaces = 2;
            this.txtDetailNumberOfYears.NullValueAllowed = true;
            //
            // lblDetailNumberOfYears
            //
            this.lblDetailNumberOfYears.Location = new System.Drawing.Point(2,2);
            this.lblDetailNumberOfYears.Name = "lblDetailNumberOfYears";
            this.lblDetailNumberOfYears.AutoSize = true;
            this.lblDetailNumberOfYears.Text = "Number Of Years:";
            this.lblDetailNumberOfYears.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNumberOfYears.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNumberOfYears.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailNumberOfMonths
            //
            this.txtDetailNumberOfMonths.Location = new System.Drawing.Point(2,2);
            this.txtDetailNumberOfMonths.Name = "txtDetailNumberOfMonths";
            this.txtDetailNumberOfMonths.Size = new System.Drawing.Size(30, 28);
            this.txtDetailNumberOfMonths.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailNumberOfMonths.DecimalPlaces = 2;
            this.txtDetailNumberOfMonths.NullValueAllowed = true;
            //
            // lblDetailNumberOfMonths
            //
            this.lblDetailNumberOfMonths.Location = new System.Drawing.Point(2,2);
            this.lblDetailNumberOfMonths.Name = "lblDetailNumberOfMonths";
            this.lblDetailNumberOfMonths.AutoSize = true;
            this.lblDetailNumberOfMonths.Text = "Number Of Months:";
            this.lblDetailNumberOfMonths.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNumberOfMonths.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNumberOfMonths.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailNumberOfDays
            //
            this.txtDetailNumberOfDays.Location = new System.Drawing.Point(2,2);
            this.txtDetailNumberOfDays.Name = "txtDetailNumberOfDays";
            this.txtDetailNumberOfDays.Size = new System.Drawing.Size(30, 28);
            this.txtDetailNumberOfDays.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailNumberOfDays.DecimalPlaces = 2;
            this.txtDetailNumberOfDays.NullValueAllowed = true;
            //
            // lblDetailNumberOfDays
            //
            this.lblDetailNumberOfDays.Location = new System.Drawing.Point(2,2);
            this.lblDetailNumberOfDays.Name = "lblDetailNumberOfDays";
            this.lblDetailNumberOfDays.AutoSize = true;
            this.lblDetailNumberOfDays.Text = "Number Of Days:";
            this.lblDetailNumberOfDays.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNumberOfDays.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNumberOfDays.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailNumberOfHours
            //
            this.txtDetailNumberOfHours.Location = new System.Drawing.Point(2,2);
            this.txtDetailNumberOfHours.Name = "txtDetailNumberOfHours";
            this.txtDetailNumberOfHours.Size = new System.Drawing.Size(30, 28);
            this.txtDetailNumberOfHours.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailNumberOfHours.DecimalPlaces = 2;
            this.txtDetailNumberOfHours.NullValueAllowed = true;
            //
            // lblDetailNumberOfHours
            //
            this.lblDetailNumberOfHours.Location = new System.Drawing.Point(2,2);
            this.lblDetailNumberOfHours.Name = "lblDetailNumberOfHours";
            this.lblDetailNumberOfHours.AutoSize = true;
            this.lblDetailNumberOfHours.Text = "Number Of Hours:";
            this.lblDetailNumberOfHours.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNumberOfHours.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNumberOfHours.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailNumberOfMinutes
            //
            this.txtDetailNumberOfMinutes.Location = new System.Drawing.Point(2,2);
            this.txtDetailNumberOfMinutes.Name = "txtDetailNumberOfMinutes";
            this.txtDetailNumberOfMinutes.Size = new System.Drawing.Size(30, 28);
            this.txtDetailNumberOfMinutes.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailNumberOfMinutes.DecimalPlaces = 2;
            this.txtDetailNumberOfMinutes.NullValueAllowed = true;
            //
            // lblDetailNumberOfMinutes
            //
            this.lblDetailNumberOfMinutes.Location = new System.Drawing.Point(2,2);
            this.lblDetailNumberOfMinutes.Name = "lblDetailNumberOfMinutes";
            this.lblDetailNumberOfMinutes.AutoSize = true;
            this.lblDetailNumberOfMinutes.Text = "Number Of Minutes:";
            this.lblDetailNumberOfMinutes.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNumberOfMinutes.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailNumberOfMinutes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNumberOfYears, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNumberOfDays, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNumberOfYears, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNumberOfDays, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNumberOfMonths, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNumberOfHours, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNumberOfMonths, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNumberOfHours, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailNumberOfMinutes, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailNumberOfMinutes, 5, 1);
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDetailFrequencyCode, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.pnlValues, 4);
            this.tableLayoutPanel2.Controls.Add(this.pnlValues, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailFrequencyCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailFrequencyDescription, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailFrequencyDescription, 3, 0);
            //
            // tbbSave
            //
            this.tbbSave.Name = "tbbSave";
            this.tbbSave.AutoSize = true;
            this.tbbSave.Click += new System.EventHandler(this.FileSave);
            this.tbbSave.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSave.Glyph"));
            this.tbbSave.ToolTipText = "Saves changed data";
            this.tbbSave.Text = "&Save";
            //
            // tbbNew
            //
            this.tbbNew.Name = "tbbNew";
            this.tbbNew.AutoSize = true;
            this.tbbNew.Click += new System.EventHandler(this.NewRecord);
            this.tbbNew.Text = "New Frequency";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbNew});
            //
            // mniFileSave
            //
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.AutoSize = true;
            this.mniFileSave.Click += new System.EventHandler(this.FileSave);
            this.mniFileSave.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileSave.Glyph"));
            this.mniFileSave.ToolTipText = "Saves changed data";
            this.mniFileSave.Text = "&Save";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniFilePrint
            //
            this.mniFilePrint.Name = "mniFilePrint";
            this.mniFilePrint.AutoSize = true;
            this.mniFilePrint.Text = "&Print...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
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
                           mniFileSave,
                        mniSeparator0,
                        mniFilePrint,
                        mniSeparator1,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniEditUndoCurrentField
            //
            this.mniEditUndoCurrentField.Name = "mniEditUndoCurrentField";
            this.mniEditUndoCurrentField.AutoSize = true;
            this.mniEditUndoCurrentField.Text = "Undo &Current Field";
            //
            // mniEditUndoScreen
            //
            this.mniEditUndoScreen.Name = "mniEditUndoScreen";
            this.mniEditUndoScreen.AutoSize = true;
            this.mniEditUndoScreen.Text = "&Undo Screen";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniEditFind
            //
            this.mniEditFind.Name = "mniEditFind";
            this.mniEditFind.AutoSize = true;
            this.mniEditFind.Text = "&Find...";
            //
            // mniEdit
            //
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.AutoSize = true;
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniEditUndoCurrentField,
                        mniEditUndoScreen,
                        mniSeparator2,
                        mniEditFind});
            this.mniEdit.Text = "&Edit";
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
                        mniEdit,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmFrequencySetup
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(660, 700);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmFrequencySetup";
            this.Text = "Maintain Frequencies";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlValues.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtDetailFrequencyCode;
        private System.Windows.Forms.Label lblDetailFrequencyCode;
        private System.Windows.Forms.TextBox txtDetailFrequencyDescription;
        private System.Windows.Forms.Label lblDetailFrequencyDescription;
        private System.Windows.Forms.Panel pnlValues;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailNumberOfYears;
        private System.Windows.Forms.Label lblDetailNumberOfYears;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailNumberOfMonths;
        private System.Windows.Forms.Label lblDetailNumberOfMonths;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailNumberOfDays;
        private System.Windows.Forms.Label lblDetailNumberOfDays;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailNumberOfHours;
        private System.Windows.Forms.Label lblDetailNumberOfHours;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailNumberOfMinutes;
        private System.Windows.Forms.Label lblDetailNumberOfMinutes;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbNew;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniEditFind;
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

