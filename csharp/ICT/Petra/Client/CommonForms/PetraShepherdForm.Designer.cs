//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AustinS
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
        	this.btnHelp = new System.Windows.Forms.Button();
        	this.btnCancel = new System.Windows.Forms.Button();
        	this.btnBack = new System.Windows.Forms.Button();
        	this.btnNext = new System.Windows.Forms.Button();
        	this.btnFinish = new System.Windows.Forms.Button();
        	this.pnlNavigation = new System.Windows.Forms.Panel();
        	this.pnlContent = new System.Windows.Forms.Panel();
        	this.testStatusMessage = new System.Windows.Forms.TextBox();
        	this.lblHeading2 = new System.Windows.Forms.Label();
        	this.lblHeading1 = new System.Windows.Forms.Label();
        	this.pnlTop = new System.Windows.Forms.Panel();
        	this.pnlButtons.SuspendLayout();
        	this.pnlContent.SuspendLayout();
        	this.pnlTop.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// pnlButtons
        	// 
        	this.pnlButtons.Controls.Add(this.btnHelp);
        	this.pnlButtons.Controls.Add(this.btnCancel);
        	this.pnlButtons.Controls.Add(this.btnBack);
        	this.pnlButtons.Controls.Add(this.btnNext);
        	this.pnlButtons.Controls.Add(this.btnFinish);
        	this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.pnlButtons.Location = new System.Drawing.Point(0, 317);
        	this.pnlButtons.Name = "pnlButtons";
        	this.pnlButtons.Size = new System.Drawing.Size(584, 47);
        	this.pnlButtons.TabIndex = 1;
        	// 
        	// btnHelp
        	// 
        	this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.btnHelp.Location = new System.Drawing.Point(12, 12);
        	this.btnHelp.Name = "btnHelp";
        	this.btnHelp.Size = new System.Drawing.Size(75, 23);
        	this.btnHelp.TabIndex = 4;
        	this.btnHelp.Text = "Help";
        	this.btnHelp.UseVisualStyleBackColor = true;
        	this.btnHelp.Click += new System.EventHandler(this.BtnHelpClick);
        	// 
        	// btnCancel
        	// 
        	this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnCancel.Location = new System.Drawing.Point(254, 12);
        	this.btnCancel.Name = "btnCancel";
        	this.btnCancel.Size = new System.Drawing.Size(75, 23);
        	this.btnCancel.TabIndex = 3;
        	this.btnCancel.Text = "Cancel";
        	this.btnCancel.UseVisualStyleBackColor = true;
        	this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
        	// 
        	// btnBack
        	// 
        	this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnBack.Location = new System.Drawing.Point(335, 12);
        	this.btnBack.Name = "btnBack";
        	this.btnBack.Size = new System.Drawing.Size(75, 23);
        	this.btnBack.TabIndex = 2;
        	this.btnBack.Text = "< Back";
        	this.btnBack.UseVisualStyleBackColor = true;
        	this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
        	// 
        	// btnNext
        	// 
        	this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnNext.Location = new System.Drawing.Point(416, 12);
        	this.btnNext.Name = "btnNext";
        	this.btnNext.Size = new System.Drawing.Size(75, 23);
        	this.btnNext.TabIndex = 1;
        	this.btnNext.Text = "Next >";
        	this.btnNext.UseVisualStyleBackColor = true;
        	this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
        	// 
        	// btnFinish
        	// 
        	this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnFinish.Location = new System.Drawing.Point(497, 12);
        	this.btnFinish.Name = "btnFinish";
        	this.btnFinish.Size = new System.Drawing.Size(75, 23);
        	this.btnFinish.TabIndex = 0;
        	this.btnFinish.Text = "Finish";
        	this.btnFinish.UseVisualStyleBackColor = true;
        	this.btnFinish.Click += new System.EventHandler(this.BtnFinishClick);
        	// 
        	// pnlNavigation
        	// 
        	this.pnlNavigation.BackColor = System.Drawing.Color.Yellow;
        	this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
        	this.pnlNavigation.Location = new System.Drawing.Point(0, 0);
        	this.pnlNavigation.Name = "pnlNavigation";
        	this.pnlNavigation.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
        	this.pnlNavigation.Size = new System.Drawing.Size(163, 317);
        	this.pnlNavigation.TabIndex = 2;
        	// 
        	// pnlContent
        	// 
        	this.pnlContent.BackColor = System.Drawing.Color.Purple;
        	this.pnlContent.Controls.Add(this.testStatusMessage);
        	this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.pnlContent.Location = new System.Drawing.Point(163, 0);
        	this.pnlContent.Name = "pnlContent";
        	this.pnlContent.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
        	this.pnlContent.Size = new System.Drawing.Size(421, 317);
        	this.pnlContent.TabIndex = 3;
        	// 
        	// testStatusMessage
        	// 
        	this.testStatusMessage.Location = new System.Drawing.Point(35, 112);
        	this.testStatusMessage.Name = "testStatusMessage";
        	this.testStatusMessage.Size = new System.Drawing.Size(184, 20);
        	this.testStatusMessage.TabIndex = 0;
        	this.testStatusMessage.TextChanged += new System.EventHandler(this.TextBox1TextChanged);
        	// 
        	// lblHeading2
        	// 
        	this.lblHeading2.Dock = System.Windows.Forms.DockStyle.Top;
        	this.lblHeading2.Location = new System.Drawing.Point(5, 24);
        	this.lblHeading2.Name = "lblHeading2";
        	this.lblHeading2.Size = new System.Drawing.Size(411, 16);
        	this.lblHeading2.TabIndex = 1;
        	this.lblHeading2.Text = "lblHeading2----------------------------------------------------------------------" +
        	"--------------------------------------------------------------------------------" +
        	"-----------------------";
        	// 
        	// lblHeading1
        	// 
        	this.lblHeading1.Dock = System.Windows.Forms.DockStyle.Top;
        	this.lblHeading1.Location = new System.Drawing.Point(5, 8);
        	this.lblHeading1.Name = "lblHeading1";
        	this.lblHeading1.Size = new System.Drawing.Size(411, 16);
        	this.lblHeading1.TabIndex = 0;
        	this.lblHeading1.Text = "lblHeading1----------------------------------------------------------------------" +
        	"--------------------------------------------------------------------------------" +
        	"-----------------------";
        	// 
        	// pnlTop
        	// 
        	this.pnlTop.Controls.Add(this.lblHeading2);
        	this.pnlTop.Controls.Add(this.lblHeading1);
        	this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
        	this.pnlTop.Location = new System.Drawing.Point(163, 0);
        	this.pnlTop.Name = "pnlTop";
        	this.pnlTop.Padding = new System.Windows.Forms.Padding(5, 8, 5, 0);
        	this.pnlTop.Size = new System.Drawing.Size(421, 42);
        	this.pnlTop.TabIndex = 4;
        	// 
        	// TPetraShepherdForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(584, 364);
        	this.Controls.Add(this.pnlTop);
        	this.Controls.Add(this.pnlContent);
        	this.Controls.Add(this.pnlNavigation);
        	this.Controls.Add(this.pnlButtons);
        	this.MinimumSize = new System.Drawing.Size(600, 400);
        	this.Name = "TPetraShepherdForm";
        	this.Text = "TPetraShepherdForm";
        	this.Load += new System.EventHandler(this.Form_Load);
        	this.pnlButtons.ResumeLayout(false);
        	this.pnlContent.ResumeLayout(false);
        	this.pnlContent.PerformLayout();
        	this.pnlTop.ResumeLayout(false);
        	this.ResumeLayout(false);
        }
        protected System.Windows.Forms.TextBox testStatusMessage;

        protected System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Label lblHeading2;
        private System.Windows.Forms.Label lblHeading1;
        private System.Windows.Forms.Panel pnlTop;
    }
}
