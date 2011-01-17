// auto generated with nant generateWinforms from GLCostCentreHierarchy.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    partial class TFrmGLCostCentreHierarchy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGLCostCentreHierarchy));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.sptSplitter = new System.Windows.Forms.SplitContainer();
            this.trvCostCentres = new Ict.Common.Controls.TTrvTreeView();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailCostCentreCode = new System.Windows.Forms.TextBox();
            this.lblDetailCostCentreCode = new System.Windows.Forms.Label();
            this.cmbDetailCostCentreType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailCostCentreType = new System.Windows.Forms.Label();
            this.txtDetailCostCentreName = new System.Windows.Forms.TextBox();
            this.lblDetailCostCentreName = new System.Windows.Forms.Label();
            this.chkDetailCostCentreActiveFlag = new System.Windows.Forms.CheckBox();
            this.lblDetailCostCentreActiveFlag = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbAddNewCostCentre = new System.Windows.Forms.ToolStripButton();
            this.tbbExportHierarchy = new System.Windows.Forms.ToolStripButton();
            this.tbbImportHierarchy = new System.Windows.Forms.ToolStripButton();
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
            this.sptSplitter.SuspendLayout();
            this.sptSplitter.Panel1.SuspendLayout();
            this.sptSplitter.Panel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
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
            this.pnlContent.Controls.Add(this.sptSplitter);
            //
            // sptSplitter
            //
            this.sptSplitter.Name = "sptSplitter";
            this.sptSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sptSplitter.SplitterDistance = 50;
            this.sptSplitter.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.sptSplitter.Panel1.Controls.Add(this.trvCostCentres);
            this.sptSplitter.Panel2.Controls.Add(this.pnlDetails);
            //
            // trvCostCentres
            //
            this.trvCostCentres.Name = "trvCostCentres";
            this.trvCostCentres.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel1);
            //
            // txtDetailCostCentreCode
            //
            this.txtDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailCostCentreCode.Name = "txtDetailCostCentreCode";
            this.txtDetailCostCentreCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailCostCentreCode
            //
            this.lblDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreCode.Name = "lblDetailCostCentreCode";
            this.lblDetailCostCentreCode.AutoSize = true;
            this.lblDetailCostCentreCode.Text = "Cost Centre Code:";
            this.lblDetailCostCentreCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCostCentreType
            //
            this.cmbDetailCostCentreType.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCostCentreType.Name = "cmbDetailCostCentreType";
            this.cmbDetailCostCentreType.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailCostCentreType.Items.AddRange(new object[] {"Local","Foreign"});
            //
            // lblDetailCostCentreType
            //
            this.lblDetailCostCentreType.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreType.Name = "lblDetailCostCentreType";
            this.lblDetailCostCentreType.AutoSize = true;
            this.lblDetailCostCentreType.Text = "Cost Centre Type:";
            this.lblDetailCostCentreType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailCostCentreName
            //
            this.txtDetailCostCentreName.Location = new System.Drawing.Point(2,2);
            this.txtDetailCostCentreName.Name = "txtDetailCostCentreName";
            this.txtDetailCostCentreName.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailCostCentreName
            //
            this.lblDetailCostCentreName.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreName.Name = "lblDetailCostCentreName";
            this.lblDetailCostCentreName.AutoSize = true;
            this.lblDetailCostCentreName.Text = "Cost Centre Name:";
            this.lblDetailCostCentreName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailCostCentreActiveFlag
            //
            this.chkDetailCostCentreActiveFlag.Location = new System.Drawing.Point(2,2);
            this.chkDetailCostCentreActiveFlag.Name = "chkDetailCostCentreActiveFlag";
            this.chkDetailCostCentreActiveFlag.Size = new System.Drawing.Size(30, 28);
            this.chkDetailCostCentreActiveFlag.Text = "";
            this.chkDetailCostCentreActiveFlag.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailCostCentreActiveFlag
            //
            this.lblDetailCostCentreActiveFlag.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreActiveFlag.Name = "lblDetailCostCentreActiveFlag";
            this.lblDetailCostCentreActiveFlag.AutoSize = true;
            this.lblDetailCostCentreActiveFlag.Text = "Active:";
            this.lblDetailCostCentreActiveFlag.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreActiveFlag.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCostCentreActiveFlag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblDetailCostCentreCode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailCostCentreType, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailCostCentreName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDetailCostCentreActiveFlag, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailCostCentreCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbDetailCostCentreType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDetailCostCentreName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkDetailCostCentreActiveFlag, 1, 3);
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
            // tbbAddNewCostCentre
            //
            this.tbbAddNewCostCentre.Name = "tbbAddNewCostCentre";
            this.tbbAddNewCostCentre.AutoSize = true;
            this.tbbAddNewCostCentre.Click += new System.EventHandler(this.AddNewCostCentre);
            this.tbbAddNewCostCentre.Text = "Add Cost Centre";
            //
            // tbbExportHierarchy
            //
            this.tbbExportHierarchy.Name = "tbbExportHierarchy";
            this.tbbExportHierarchy.AutoSize = true;
            this.tbbExportHierarchy.Click += new System.EventHandler(this.ExportHierarchy);
            this.tbbExportHierarchy.Text = "Export Hierarchy";
            //
            // tbbImportHierarchy
            //
            this.tbbImportHierarchy.Name = "tbbImportHierarchy";
            this.tbbImportHierarchy.AutoSize = true;
            this.tbbImportHierarchy.Click += new System.EventHandler(this.ImportHierarchy);
            this.tbbImportHierarchy.Text = "Import Hierarchy";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbAddNewCostCentre,
                        tbbExportHierarchy,
                        tbbImportHierarchy});
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
            // TFrmGLCostCentreHierarchy
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

            this.Name = "TFrmGLCostCentreHierarchy";
            this.Text = "GL Cost Centre Hierarchy";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.sptSplitter.Panel2.ResumeLayout(false);
            this.sptSplitter.Panel1.ResumeLayout(false);
            this.sptSplitter.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.SplitContainer sptSplitter;
        private Ict.Common.Controls.TTrvTreeView trvCostCentres;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtDetailCostCentreCode;
        private System.Windows.Forms.Label lblDetailCostCentreCode;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailCostCentreType;
        private System.Windows.Forms.Label lblDetailCostCentreType;
        private System.Windows.Forms.TextBox txtDetailCostCentreName;
        private System.Windows.Forms.Label lblDetailCostCentreName;
        private System.Windows.Forms.CheckBox chkDetailCostCentreActiveFlag;
        private System.Windows.Forms.Label lblDetailCostCentreActiveFlag;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbAddNewCostCentre;
        private System.Windows.Forms.ToolStripButton tbbExportHierarchy;
        private System.Windows.Forms.ToolStripButton tbbImportHierarchy;
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
