//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class TFrmPrintPreview
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
        }

        /// <summary>
        /// this is InitializeComponent from PetraForm
        /// this will not be needed anymore when the code generation works
        /// </summary>
        private void InitializeComponentPetraForm()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPrintPreview));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDivider1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new TExtStatusBarHelp();
            this.stpInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbClose = new System.Windows.Forms.ToolStripButton();

            this.mnuMain.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.stbMain.SuspendLayout();
            this.SuspendLayout();

            //
            // mnuMain
            //
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(10, 24);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.mniFile,
                    this.mniHelp
                });
            this.mnuMain.Dock = System.Windows.Forms.DockStyle.Top;

            //
            // mniFile
            //
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.mniFileClose
                });
            this.mniFile.Text = "&File";
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;

            //
            // mniFileClose
            //
            this.mniFileClose.Text = "&Close";
            this.mniFileClose.Click += new System.EventHandler(this.MniFile_Click);
            this.mniFileClose.AutoSize = true;
            this.mniFileClose.Name = "mniFileClose";
            this.mniFileClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniFileClose.ToolTipText = "Close the preview";

            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.AutoSize = true;
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.mniHelpPetraHelp,
                    this.mniHelpDivider1,
                    this.mniHelpBugReport,
                    this.mniHelpAboutPetra,
                    this.mniHelpDevelopmentTeam
                });
            this.mniHelp.Text = "&Help";

            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Text = "&Petra Help";
            this.mniHelpPetraHelp.Click += new System.EventHandler(this.MniHelpPetra_Click);
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";

            //
            // mniHelpDivider1
            //
            this.mniHelpDivider1.Text = "-";
            this.mniHelpDivider1.AutoSize = true;
            this.mniHelpDivider1.Name = "mniHelpDivider1";

            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Text = "Bug &Report";
            this.mniHelpBugReport.Click += new System.EventHandler(this.MniHelpBugReport_Click);
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Name = "mniHelpBugReport";

            //
            // mniHelpAboutPetra
            //
            this.mniHelpAboutPetra.Text = "&About OpenPetra...";
            this.mniHelpAboutPetra.Click += new System.EventHandler(this.MniHelpAboutPetra_Click);
            this.mniHelpAboutPetra.AutoSize = true;
            this.mniHelpAboutPetra.Name = "mniHelpAboutPetra";

            //
            // mniHelpDevelopmentTeam
            //
            this.mniHelpDevelopmentTeam.Text = "&The Development Team...";
            this.mniHelpDevelopmentTeam.Click += new System.EventHandler(this.MniHelpDevelopmentTeam_Click);
            this.mniHelpDevelopmentTeam.AutoSize = true;
            this.mniHelpDevelopmentTeam.Name = "mniHelpDevelopmentTeam";

            //
            // stbMain
            //
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.Name = "stbMain";
            this.stbMain.Items.AddRange(new System.Windows.Forms.ToolStripStatusLabel[] {
                    this.stpInfo
                });
            this.stbMain.Size = new System.Drawing.Size(10, 24);
            this.stbMain.Text = "Status Bar";
            this.stbMain.ShowItemToolTips = true;

            //
            // stpInfo
            //
            this.stpInfo.Name = "stpInfo";
            this.stpInfo.Text = "Ready";
            this.stpInfo.Width = 484;

            //
            // tbrMain
            //
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbClose
                });
            this.tbrMain.Location = new System.Drawing.Point(2, 2);
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(74, 25);

            //
            // tbbClose
            //
            this.tbbClose.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbClose.Glyph"));
            this.tbbClose.Click += new System.EventHandler(this.tbbCloseClick);
            this.tbbClose.Text = "Close";
            this.tbbClose.ToolTipText = "Closes this window";
            this.tbbClose.Width = 38;

            //
            // TFrmPetra
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 536);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.stbMain);
            this.MainMenuStrip = mnuMain;
            this.Name = "TPrintPreview";
            this.Text = "Print Preview";
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;

            this.Activated += new System.EventHandler(this.FPetraUtilsObject.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.FPetraUtilsObject.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FPetraUtilsObject.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FPetraUtilsObject.Form_KeyDown);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniHelpDivider1;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbClose;
        private TExtStatusBarHelp stbMain;
        private System.Windows.Forms.ToolStripStatusLabel stpInfo;

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            InitializeComponentPetraForm();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPrintPreview));
            this.components = new System.ComponentModel.Container();
            this.PrintDocument = new System.Drawing.Printing.PrintDocument();
            this.dlgSaveTextFile = new System.Windows.Forms.SaveFileDialog();
            this.dlgSaveCSVFile = new System.Windows.Forms.SaveFileDialog();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPreview = new System.Windows.Forms.TabControl();
            this.tbpText = new System.Windows.Forms.TabPage();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.tbpPreview = new System.Windows.Forms.TabPage();
            this.lblNoPrinter = new System.Windows.Forms.Label();
            this.PrintPreviewControl = new System.Windows.Forms.PrintPreviewControl();
            this.pnlNavigatePreview = new System.Windows.Forms.Panel();
            this.CbB_Zoom = new System.Windows.Forms.ComboBox();
            this.Btn_PreviousPage = new System.Windows.Forms.Button();
            this.Btn_NextPage = new System.Windows.Forms.Button();
            this.tbpGridView = new System.Windows.Forms.TabPage();
            this.sgGridView = new Ict.Common.Controls.TSgrdDataGrid();
            this.ContextMenu1 = new System.Windows.Forms.ContextMenu();
            this.tbtPrint = new System.Windows.Forms.ToolStripButton();
            this.XPToolBarSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbtExportCSV = new System.Windows.Forms.ToolStripButton();
            this.tbtExportText = new System.Windows.Forms.ToolStripButton();
            this.tbtGenerateChart = new System.Windows.Forms.ToolStripButton();
            this.tabPreview.SuspendLayout();
            this.tbpText.SuspendLayout();
            this.tbpPreview.SuspendLayout();
            this.pnlNavigatePreview.SuspendLayout();
            this.tbpGridView.SuspendLayout();
            this.SuspendLayout();

            //
            // tbrMain
            //
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbtPrint,
                    this.XPToolBarSeparator1,
                    this.tbtExportCSV,
                    this.tbtExportText,
                    this.tbtGenerateChart
                });
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(10, 24);
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;

            //
            // stpInfo
            //
            this.stpInfo.Width = 672;

            //
            // PrintDocument
            //
            this.PrintDocument.EndPrint += new PrintEventHandler(this.PrintDocument_EndPrint);

            //
            // dlgSaveTextFile
            //
            this.dlgSaveTextFile.DefaultExt = "txt";
            this.dlgSaveTextFile.Filter = "Text file|*.txt";
            this.dlgSaveTextFile.Title = "Save report as Text file";

            //
            // dlgSaveCSVFile
            //
            this.dlgSaveCSVFile.DefaultExt = "csv";
            this.dlgSaveCSVFile.Title = "Save report as CSV file";

            //
            // ImageList1
            //
            this.ImageList1.ImageSize = new System.Drawing.Size(1, 1);
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;

            //
            // tabPreview
            //
            this.tabPreview.Controls.Add(this.tbpText);
            this.tabPreview.Controls.Add(this.tbpPreview);
            this.tabPreview.Controls.Add(this.tbpGridView);
            this.tabPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreview.Location = new System.Drawing.Point(0, 27);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.SelectedIndex = 0;
            this.tabPreview.Size = new System.Drawing.Size(688, 535);
            this.tabPreview.TabIndex = 12;

            //
            // tbpText
            //
            this.tbpText.Controls.Add(this.txtOutput);
            this.tbpText.Location = new System.Drawing.Point(4, 22);
            this.tbpText.Name = "tbpText";
            this.tbpText.Size = new System.Drawing.Size(680, 509);
            this.tbpText.TabIndex = 0;
            this.tbpText.Text = "Text Preview";

            //
            // txtOutput
            //
            this.txtOutput.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Font =
                new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(680, 509);
            this.txtOutput.TabIndex = 11;
            this.txtOutput.Text = "Text Output";
            this.txtOutput.WordWrap = false;
            this.txtOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtOutputKeyPress);

            //
            // tbpPreview
            //
            this.tbpPreview.Controls.Add(this.lblNoPrinter);
            this.tbpPreview.Controls.Add(this.PrintPreviewControl);
            this.tbpPreview.Controls.Add(this.pnlNavigatePreview);
            this.tbpPreview.Location = new System.Drawing.Point(4, 22);
            this.tbpPreview.Name = "tbpPreview";
            this.tbpPreview.Size = new System.Drawing.Size(680, 509);
            this.tbpPreview.TabIndex = 1;
            this.tbpPreview.Text = "Print Preview";

            //
            // lblNoPrinter
            //
            this.lblNoPrinter.Font = new System.Drawing.Font("Tahoma",
                12,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.lblNoPrinter.Location = new System.Drawing.Point(160, 128);
            this.lblNoPrinter.Name = "lblNoPrinter";
            this.lblNoPrinter.Size = new System.Drawing.Size(336, 48);
            this.lblNoPrinter.TabIndex = 16;
            this.lblNoPrinter.Text = "Unfortunately this function is disabled. Please" + " install a printer to use this page.";

            //
            // PrintPreviewControl
            //
            this.PrintPreviewControl.AutoZoom = false;
            this.PrintPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrintPreviewControl.Font =
                new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.PrintPreviewControl.Location = new System.Drawing.Point(0, 43);
            this.PrintPreviewControl.Name = "PrintPreviewControl";
            this.PrintPreviewControl.Size = new System.Drawing.Size(680, 466);
            this.PrintPreviewControl.TabIndex = 11;
            this.PrintPreviewControl.Zoom = 0.300000011920929;

            //
            // pnlNavigatePreview
            //
            this.pnlNavigatePreview.Controls.Add(this.CbB_Zoom);
            this.pnlNavigatePreview.Controls.Add(this.Btn_PreviousPage);
            this.pnlNavigatePreview.Controls.Add(this.Btn_NextPage);
            this.pnlNavigatePreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavigatePreview.Location = new System.Drawing.Point(0, 0);
            this.pnlNavigatePreview.Name = "pnlNavigatePreview";
            this.pnlNavigatePreview.Size = new System.Drawing.Size(680, 43);
            this.pnlNavigatePreview.TabIndex = 15;

            //
            // CbB_Zoom
            //
            this.CbB_Zoom.Items.AddRange(new object[] { "Fit to Window", "100%", "75%", "50%" });
            this.CbB_Zoom.Location = new System.Drawing.Point(8, 9);
            this.CbB_Zoom.Name = "CbB_Zoom";
            this.CbB_Zoom.TabIndex = 15;
            this.CbB_Zoom.Text = "Select Zoom";
            this.CbB_Zoom.SelectedIndexChanged += new System.EventHandler(this.CbB_Zoom_SelectedIndexChanged);

            //
            // Btn_PreviousPage
            //
            this.Btn_PreviousPage.Location = new System.Drawing.Point(144, 9);
            this.Btn_PreviousPage.Name = "Btn_PreviousPage";
            this.Btn_PreviousPage.Size = new System.Drawing.Size(96, 24);
            this.Btn_PreviousPage.TabIndex = 16;
            this.Btn_PreviousPage.Text = "Previous Page";
            this.Btn_PreviousPage.Click += new System.EventHandler(this.Btn_PreviousPage_Click);

            //
            // Btn_NextPage
            //
            this.Btn_NextPage.Location = new System.Drawing.Point(248, 9);
            this.Btn_NextPage.Name = "Btn_NextPage";
            this.Btn_NextPage.Size = new System.Drawing.Size(80, 24);
            this.Btn_NextPage.TabIndex = 17;
            this.Btn_NextPage.Text = "Next Page";
            this.Btn_NextPage.Click += new System.EventHandler(this.Btn_NextPage_Click);

            //
            // tbpGridView
            //
            this.tbpGridView.Controls.Add(this.sgGridView);
            this.tbpGridView.Location = new System.Drawing.Point(4, 22);
            this.tbpGridView.Name = "tbpGridView";
            this.tbpGridView.Size = new System.Drawing.Size(680, 509);
            this.tbpGridView.TabIndex = 2;
            this.tbpGridView.Text = "Detail Reports";

            //
            // sgGridView
            //
            this.sgGridView.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.sgGridView.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sgGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sgGridView.ContextMenu = this.ContextMenu1;
            this.sgGridView.DeleteQuestionMessage = "You have chosen to delete this r" + "ecord.'#13#10#13#10'Do you really want to delete it?";
            this.sgGridView.FixedRows = 1;
            this.sgGridView.Location = new System.Drawing.Point(0, 0);
            this.sgGridView.MinimumHeight = 19;
            this.sgGridView.Name = "sgGridView";
            this.sgGridView.Size = new System.Drawing.Size(680, 507);
            this.sgGridView.SpecialKeys =
                (SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                  SourceGrid.GridSpecialKeys.PageDownUp) |
                                                 SourceGrid.GridSpecialKeys.Enter) |
                                                SourceGrid.GridSpecialKeys.Escape) |
                                               SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift));
            this.sgGridView.TabIndex = 0;
            this.sgGridView.TabStop = true;

            //
            // tbtPrint
            //
            this.tbtPrint.Text = "Print";
            this.tbtPrint.ToolTipText = "Print the report";
            this.tbtPrint.Click += new System.EventHandler(this.tbtPrintClick);

            //
            // tbtExportCSV
            //
            this.tbtExportCSV.Text = "Export to CSV";
            this.tbtExportCSV.ToolTipText = "Export to CSV or directly into Excel, if" + " it is available";
            this.tbtExportCSV.Click += new System.EventHandler(this.tbtExportCSVClick);
            this.tbtExportCSV.Width = 60;

            //
            // tbtExportText
            //
            this.tbtExportText.Text = "Save as Text file";
            this.tbtExportText.ToolTipText = "Save as a text file (e.g. for email)";
            this.tbtExportText.Click += new System.EventHandler(this.tbtExportTextClick);
            this.tbtExportText.Width = 70;

            //
            // tbtGenerateChart
            //
            this.tbtGenerateChart.Text = "Generate Chart";
            this.tbtGenerateChart.ToolTipText = "Generates a chart in Excel (only ava" + "ilable yet for few reports at the moment)";
            this.tbtGenerateChart.Click += new System.EventHandler(this.tbtGenerateChartClick);
            this.tbtGenerateChart.Width = 60;

            //
            // TPrintPreview
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(688, 585);
            this.Controls.Add(this.tabPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPrintPreview";
            this.Text = "Print Preview";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.tabPreview, 0);

            this.stbMain.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.tbpText.ResumeLayout(false);
            this.tbpPreview.ResumeLayout(false);
            this.pnlNavigatePreview.ResumeLayout(false);
            this.tbpGridView.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PrintPreviewControl PrintPreviewControl;
        private System.Windows.Forms.Button Btn_NextPage;
        private System.Windows.Forms.Button Btn_PreviousPage;
        private System.Windows.Forms.ComboBox CbB_Zoom;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Drawing.Printing.PrintDocument PrintDocument;
        private System.Windows.Forms.SaveFileDialog dlgSaveTextFile;
        private System.Windows.Forms.SaveFileDialog dlgSaveCSVFile;
        private System.Windows.Forms.ImageList ImageList1;
        private System.Windows.Forms.TabControl tabPreview;
        private System.Windows.Forms.TabPage tbpText;
        private System.Windows.Forms.TabPage tbpPreview;
        private System.Windows.Forms.Panel pnlNavigatePreview;
        private System.Windows.Forms.ToolStripButton tbtPrint;
        private System.Windows.Forms.ToolStripSeparator XPToolBarSeparator1;
        private System.Windows.Forms.ToolStripButton tbtExportCSV;
        private System.Windows.Forms.ToolStripButton tbtExportText;
        private System.Windows.Forms.ToolStripButton tbtGenerateChart;
        private System.Windows.Forms.TabPage tbpGridView;
        private TSgrdDataGrid sgGridView;
        private System.Windows.Forms.ContextMenu ContextMenu1;
        private System.Windows.Forms.Label lblNoPrinter;
    }
}