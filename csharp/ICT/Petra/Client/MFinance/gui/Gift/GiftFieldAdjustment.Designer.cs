// auto generated with nant generateWinforms from GiftFieldAdjustment.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    partial class TFrmGiftFieldAdjustment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmGiftFieldAdjustment));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpStartDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpDateEffective = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateEffective = new System.Windows.Forms.Label();
            this.txtRecipientKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblRecipientKey = new System.Windows.Forms.Label();
            this.txtOldFieldKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblOldFieldKey = new System.Windows.Forms.Label();
            this.chkNoReceipt = new System.Windows.Forms.CheckBox();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbFieldChangeAdjustment = new System.Windows.Forms.ToolStripButton();
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
            this.pnlParameters.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            // pnlParameters
            //
            this.pnlParameters.Location = new System.Drawing.Point(2,2);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlParameters.Controls.Add(this.tableLayoutPanel2);
            //
            // dtpStartDate
            //
            this.dtpStartDate.Location = new System.Drawing.Point(2,2);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblStartDate
            //
            this.lblStartDate.Location = new System.Drawing.Point(2,2);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpEndDate
            //
            this.dtpEndDate.Location = new System.Drawing.Point(2,2);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblEndDate
            //
            this.lblEndDate.Location = new System.Drawing.Point(2,2);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblEndDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpDateEffective
            //
            this.dtpDateEffective.Location = new System.Drawing.Point(2,2);
            this.dtpDateEffective.Name = "dtpDateEffective";
            this.dtpDateEffective.Size = new System.Drawing.Size(94, 28);
            //
            // lblDateEffective
            //
            this.lblDateEffective.Location = new System.Drawing.Point(2,2);
            this.lblDateEffective.Name = "lblDateEffective";
            this.lblDateEffective.AutoSize = true;
            this.lblDateEffective.Text = "Date Effective:";
            this.lblDateEffective.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateEffective.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateEffective.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtRecipientKey
            //
            this.txtRecipientKey.Location = new System.Drawing.Point(2,2);
            this.txtRecipientKey.Name = "txtRecipientKey";
            this.txtRecipientKey.Size = new System.Drawing.Size(370, 28);
            this.txtRecipientKey.ASpecialSetting = true;
            this.txtRecipientKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtRecipientKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtRecipientKey.PartnerClass = "";
            this.txtRecipientKey.MaxLength = 32767;
            this.txtRecipientKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtRecipientKey.TextBoxWidth = 80;
            this.txtRecipientKey.ButtonWidth = 40;
            this.txtRecipientKey.ReadOnly = false;
            this.txtRecipientKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtRecipientKey.ButtonText = "Find";
            //
            // lblRecipientKey
            //
            this.lblRecipientKey.Location = new System.Drawing.Point(2,2);
            this.lblRecipientKey.Name = "lblRecipientKey";
            this.lblRecipientKey.AutoSize = true;
            this.lblRecipientKey.Text = "Recipient Key:";
            this.lblRecipientKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblRecipientKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRecipientKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtOldFieldKey
            //
            this.txtOldFieldKey.Location = new System.Drawing.Point(2,2);
            this.txtOldFieldKey.Name = "txtOldFieldKey";
            this.txtOldFieldKey.Size = new System.Drawing.Size(370, 28);
            this.txtOldFieldKey.ASpecialSetting = true;
            this.txtOldFieldKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtOldFieldKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtOldFieldKey.PartnerClass = "";
            this.txtOldFieldKey.MaxLength = 32767;
            this.txtOldFieldKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtOldFieldKey.TextBoxWidth = 80;
            this.txtOldFieldKey.ButtonWidth = 40;
            this.txtOldFieldKey.ReadOnly = false;
            this.txtOldFieldKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtOldFieldKey.ButtonText = "Find";
            //
            // lblOldFieldKey
            //
            this.lblOldFieldKey.Location = new System.Drawing.Point(2,2);
            this.lblOldFieldKey.Name = "lblOldFieldKey";
            this.lblOldFieldKey.AutoSize = true;
            this.lblOldFieldKey.Text = "Old Field Key:";
            this.lblOldFieldKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblOldFieldKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblOldFieldKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkNoReceipt
            //
            this.chkNoReceipt.Location = new System.Drawing.Point(2,2);
            this.chkNoReceipt.Name = "chkNoReceipt";
            this.chkNoReceipt.AutoSize = true;
            this.chkNoReceipt.Text = "No Receipt";
            this.chkNoReceipt.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkNoReceipt.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblStartDate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDateEffective, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblRecipientKey, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblOldFieldKey, 0, 3);
            this.tableLayoutPanel2.SetColumnSpan(this.chkNoReceipt, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkNoReceipt, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.dtpStartDate, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpDateEffective, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtRecipientKey, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtOldFieldKey, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblEndDate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpEndDate, 3, 0);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlParameters, 0, 0);
            //
            // tbbFieldChangeAdjustment
            //
            this.tbbFieldChangeAdjustment.Name = "tbbFieldChangeAdjustment";
            this.tbbFieldChangeAdjustment.AutoSize = true;
            this.tbbFieldChangeAdjustment.Click += new System.EventHandler(this.FieldChangeAdjustment);
            this.tbbFieldChangeAdjustment.Text = "&Adjust gifts after field change";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbFieldChangeAdjustment});
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
            // TFrmGiftFieldAdjustment
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(754, 623);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmGiftFieldAdjustment";
            this.Text = "Field Change Adjustment";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlParameters.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlParameters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateEffective;
        private System.Windows.Forms.Label lblDateEffective;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtRecipientKey;
        private System.Windows.Forms.Label lblRecipientKey;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtOldFieldKey;
        private System.Windows.Forms.Label lblOldFieldKey;
        private System.Windows.Forms.CheckBox chkNoReceipt;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbFieldChangeAdjustment;
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
