//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AustinS, ChristianK
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
namespace Ict.Petra.Client.CommonForms
{
    partial class TPetraShepherdForm
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.pnlCollapsibleNavigation = new Ict.Common.Controls.TPnlCollapsible();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblHeading2 = new System.Windows.Forms.Label();
            this.lblHeading1 = new System.Windows.Forms.Label();
            this.pnlHeadings = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.prbPageProgress = new System.Windows.Forms.ProgressBar();
            this.lblPageProgress = new System.Windows.Forms.Label();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();
            this.pnlButtons.SuspendLayout();
            this.pnlNavigation.SuspendLayout();
            this.pnlHeadings.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlButtons
            //
            this.pnlButtons.Controls.Add(this.panel1);
            this.pnlButtons.Controls.Add(this.btnHelp);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnFinish);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 335);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pnlButtons.Size = new System.Drawing.Size(592, 36);
            this.pnlButtons.TabIndex = 1;
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 1);
            this.panel1.TabIndex = 5;
            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(12, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(102, 23);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelpClick);
            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(157, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            //
            // btnBack
            //
            this.btnBack.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(266, 8);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(102, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "<< &Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            //
            // btnNext
            //
            this.btnNext.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(370, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(102, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next >>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            //
            // btnFinish
            //
            this.btnFinish.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Location = new System.Drawing.Point(479, 8);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(102, 23);
            this.btnFinish.TabIndex = 0;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.BtnFinishClick);
            //
            // pnlNavigation
            //
            this.pnlNavigation.AutoSize = true;
            this.pnlNavigation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlNavigation.BackColor = System.Drawing.Color.White;
            this.pnlNavigation.Controls.Add(this.pnlCollapsibleNavigation);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 0);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlNavigation.Size = new System.Drawing.Size(184, 335);
            this.pnlNavigation.TabIndex = 2;
            //
            // pnlCollapsibleNavigation
            //
            this.pnlCollapsibleNavigation.AutoSize = true;
            this.pnlCollapsibleNavigation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCollapsibleNavigation.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
            this.pnlCollapsibleNavigation.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.pnlCollapsibleNavigation.IsCollapsed = false;
            this.pnlCollapsibleNavigation.Location = new System.Drawing.Point(5, 0);
            this.pnlCollapsibleNavigation.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCollapsibleNavigation.Name = "pnlCollapsibleNavigation";
            this.pnlCollapsibleNavigation.Size = new System.Drawing.Size(179, 335);
            this.pnlCollapsibleNavigation.TabIndex = 0;
            this.pnlCollapsibleNavigation.TaskListInstance = null;
            this.pnlCollapsibleNavigation.TaskListNode = null;
            this.pnlCollapsibleNavigation.UserControlClass = "";
            this.pnlCollapsibleNavigation.UserControlNamespace = "";
            this.pnlCollapsibleNavigation.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
            //
            // pnlContent
            //
            this.pnlContent.AutoSize = true;
            this.pnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(184, 40);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(5);
            this.pnlContent.Size = new System.Drawing.Size(408, 295);
            this.pnlContent.TabIndex = 3;
            //
            // lblHeading2
            //
            this.lblHeading2.AutoSize = true;
            this.lblHeading2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeading2.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading2.Location = new System.Drawing.Point(5, 21);
            this.lblHeading2.MaximumSize = new System.Drawing.Size(0, 420);
            this.lblHeading2.Name = "lblHeading2";
            this.lblHeading2.Padding = new System.Windows.Forms.Padding(0, 1, 0, 5);
            this.lblHeading2.Size = new System.Drawing.Size(73, 19);
            this.lblHeading2.TabIndex = 1;
            this.lblHeading2.Text = "Heading #2";
            //
            // lblHeading1
            //
            this.lblHeading1.AutoSize = true;
            this.lblHeading1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeading1.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading1.Location = new System.Drawing.Point(5, 8);
            this.lblHeading1.MaximumSize = new System.Drawing.Size(0, 420);
            this.lblHeading1.Name = "lblHeading1";
            this.lblHeading1.Size = new System.Drawing.Size(81, 13);
            this.lblHeading1.TabIndex = 0;
            this.lblHeading1.Text = "Heading #1";
            //
            // pnlHeadings
            //
            this.pnlHeadings.AutoSize = true;
            this.pnlHeadings.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeadings.Controls.Add(this.lblHeading2);
            this.pnlHeadings.Controls.Add(this.lblHeading1);
            this.pnlHeadings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeadings.Location = new System.Drawing.Point(0, 0);
            this.pnlHeadings.Name = "pnlHeadings";
            this.pnlHeadings.Padding = new System.Windows.Forms.Padding(5, 8, 8, 0);
            this.pnlHeadings.Size = new System.Drawing.Size(329, 40);
            this.pnlHeadings.TabIndex = 4;
            //
            // pnlTop
            //
            this.pnlTop.AutoSize = true;
            this.pnlTop.Controls.Add(this.pnlHeadings);
            this.pnlTop.Controls.Add(this.panel3);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(184, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(408, 40);
            this.pnlTop.TabIndex = 5;
            //
            // panel3
            //
            this.panel3.Controls.Add(this.prbPageProgress);
            this.panel3.Controls.Add(this.lblPageProgress);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(329, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(79, 40);
            this.panel3.TabIndex = 5;
            //
            // prbPageProgress
            //
            this.prbPageProgress.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prbPageProgress.ForeColor = System.Drawing.Color.ForestGreen;
            this.prbPageProgress.Location = new System.Drawing.Point(6, 19);
            this.prbPageProgress.Name = "prbPageProgress";
            this.prbPageProgress.Size = new System.Drawing.Size(64, 15);
            this.prbPageProgress.TabIndex = 1;
            //
            // lblPageProgress
            //
            this.lblPageProgress.Location = new System.Drawing.Point(4, 3);
            this.lblPageProgress.Name = "lblPageProgress";
            this.lblPageProgress.Size = new System.Drawing.Size(68, 13);
            this.lblPageProgress.TabIndex = 0;
            this.lblPageProgress.Text = "Page n/m";
            this.lblPageProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 371);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(592, 22);
            this.stbMain.TabIndex = 6;
            this.stbMain.Text = "tExtStatusBarHelp1";
            //
            // TPetraShepherdForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(592, 393);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.stbMain);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "TPetraShepherdForm";
            this.Text = "TPetraShepherdForm";
            this.Load += new System.EventHandler(this.Form_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlNavigation.PerformLayout();
            this.pnlHeadings.ResumeLayout(false);
            this.pnlHeadings.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected Ict.Common.Controls.TPnlCollapsible pnlCollapsibleNavigation;

        protected Ict.Common.Controls.TExtStatusBarHelp stbMain;
        protected System.Windows.Forms.Label lblPageProgress;
        protected System.Windows.Forms.ProgressBar prbPageProgress;
        private System.Windows.Forms.Panel pnlHeadings;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        //protected System.Windows.Forms.TextBox testStatusMessage;

        protected System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.Panel pnlNavigation;
        protected System.Windows.Forms.Button btnFinish;
        protected System.Windows.Forms.Button btnNext;
        protected System.Windows.Forms.Button btnBack;
        protected System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Panel pnlButtons;
        protected System.Windows.Forms.Label lblHeading2;
        protected System.Windows.Forms.Label lblHeading1;
        private System.Windows.Forms.Panel pnlTop;
    }
}