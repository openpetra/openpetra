// auto generated with nant generateWinforms from testChkbox.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2013 by OM International
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
using Owf.Controls;

namespace Ict.Testing
{
    partial class TFrmtestChkbox
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
                        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmtestChkbox));

                        this.pnlContent = new System.Windows.Forms.Panel();
            this.chkParent = new System.Windows.Forms.CheckBox();
            this.chkSonst = new System.Windows.Forms.CheckBox();
            this.chkChild = new System.Windows.Forms.CheckBox();
            this.lblSonst = new System.Windows.Forms.Label();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mniDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            this.chkParent.CheckedChanged += new System.EventHandler(this.chkParentCheckedChanged);
            this.chkSonst.CheckedChanged += new System.EventHandler(this.chkSonstCheckedChanged);
            this.chkChild.CheckedChanged += new System.EventHandler(this.chkChildCheckedChanged);

            //
            // chkParent
            //
            this.chkParent.Name = "chkParent";
            this.chkParent.Location = new System.Drawing.Point(8,7);
            this.chkParent.Size = new System.Drawing.Size(30, 22);
            this.chkParent.Text = "";
            this.chkParent.TabIndex = 0;

            //
            // chkSonst
            //
            this.chkSonst.Name = "chkSonst";
            this.chkSonst.Location = new System.Drawing.Point(62,32);
            this.chkSonst.Size = new System.Drawing.Size(30, 22);
            this.chkSonst.Text = "";
            this.chkSonst.TabIndex = 30;

            //
            // chkChild
            //
            this.chkChild.Name = "chkChild";
            this.chkChild.Location = new System.Drawing.Point(62,7);
            this.chkChild.Size = new System.Drawing.Size(30, 22);
            this.chkChild.Text = "";
            this.chkChild.TabIndex = 10;

            //
            // lblSonst
            //
            this.lblSonst.Name = "lblSonst";
            this.lblSonst.Location = new System.Drawing.Point(5,37);
            this.lblSonst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSonst.Size = new System.Drawing.Size(49, 17);
            this.lblSonst.Text = "Sonst:";
            this.lblSonst.TabIndex = 20;
            this.lblSonst.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlContent
            //
            this.pnlContent.Size = new System.Drawing.Size(94, 59);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Controls.Add(this.chkParent);
            this.pnlContent.Controls.Add(this.lblSonst);
            this.pnlContent.Controls.Add(this.chkChild);
            this.pnlContent.Controls.Add(this.chkSonst);

            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.Size = new System.Drawing.Size(150, 28);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            this.mniClose.Click += new System.EventHandler(this.actClose);

            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new System.Drawing.Size(150, 28);
            this.mniFile.Text = "&File";
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniClose});

            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.Size = new System.Drawing.Size(150, 28);
            this.mniHelpPetraHelp.Text = "&OpenPetra Help";

            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.Size = new System.Drawing.Size(150, 28);
            this.mniSeparator0.Text = "-";

            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.Size = new System.Drawing.Size(150, 28);
            this.mniHelpBugReport.Text = "Bug &Report";

            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.Size = new System.Drawing.Size(150, 28);
            this.mniSeparator1.Text = "-";

            //
            // mniAbout
            //
            this.mniAbout.Name = "mniAbout";
            this.mniAbout.Size = new System.Drawing.Size(150, 28);
            this.mniAbout.Image = ((System.Drawing.Bitmap)resources.GetObject("mniAbout.Glyph"));
            this.mniAbout.Text = "&About OpenPetra";
            this.mniAbout.Click += new System.EventHandler(this.actAbout);

            //
            // mniDevelopmentTeam
            //
            this.mniDevelopmentTeam.Name = "mniDevelopmentTeam";
            this.mniDevelopmentTeam.Size = new System.Drawing.Size(150, 28);
            this.mniDevelopmentTeam.Text = "&The Development Team...";

            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.Size = new System.Drawing.Size(150, 28);
            this.mniHelp.Text = "&Help";
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniHelpPetraHelp,
                        mniSeparator0,
                        mniHelpBugReport,
                        mniSeparator1,
                        mniAbout,
                        mniDevelopmentTeam});

            //
            // mnuMain
            //
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuMain.Size = new System.Drawing.Size(10, 24);
            this.mnuMain.Renderer = new ToolStripProfessionalRenderer(new TOpenPetraMenuColours());
            this.mnuMain.GripStyle = ToolStripGripStyle.Visible;
            this.mnuMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFile,
                        mniHelp});

            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.Size = new System.Drawing.Size(10, 24);

            //
            // TFrmtestChkbox
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(754, 623);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmtestChkbox";
            this.Text = "Test screen";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.CheckBox chkParent;
        private System.Windows.Forms.CheckBox chkSonst;
        private System.Windows.Forms.CheckBox chkChild;
        private System.Windows.Forms.Label lblSonst;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniAbout;
        private System.Windows.Forms.ToolStripMenuItem mniDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}