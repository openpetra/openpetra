//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
namespace Ict.Tools.DevelopersAssistant
{
    partial class DlgSuggestedLinksUpdates
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblUpdateName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentLink = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkApplySuggestion = new System.Windows.Forms.LinkLabel();
            this.lblSuggestedLink = new System.Windows.Forms.Label();
            this.btnNextSuggestion = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Update for:";
            //
            // lblUpdateName
            //
            this.lblUpdateName.AutoSize = true;
            this.lblUpdateName.Location = new System.Drawing.Point(79, 13);
            this.lblUpdateName.Name = "lblUpdateName";
            this.lblUpdateName.Size = new System.Drawing.Size(35, 13);
            this.lblUpdateName.TabIndex = 1;
            this.lblUpdateName.Text = "Name";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Your URL link:";
            //
            // lblCurrentLink
            //
            this.lblCurrentLink.AutoSize = true;
            this.lblCurrentLink.Location = new System.Drawing.Point(140, 41);
            this.lblCurrentLink.Name = "lblCurrentLink";
            this.lblCurrentLink.Size = new System.Drawing.Size(60, 13);
            this.lblCurrentLink.TabIndex = 3;
            this.lblCurrentLink.Text = "Current link";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Suggested URL Link:";
            //
            // linkApplySuggestion
            //
            this.linkApplySuggestion.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkApplySuggestion.AutoSize = true;
            this.linkApplySuggestion.Location = new System.Drawing.Point(12, 108);
            this.linkApplySuggestion.Name = "linkApplySuggestion";
            this.linkApplySuggestion.Size = new System.Drawing.Size(193, 13);
            this.linkApplySuggestion.TabIndex = 5;
            this.linkApplySuggestion.TabStop = true;
            this.linkApplySuggestion.Text = "Replace my link with the suggested link";
            this.linkApplySuggestion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkApplySuggestion_LinkClicked);
            //
            // lblSuggestedLink
            //
            this.lblSuggestedLink.AutoSize = true;
            this.lblSuggestedLink.Location = new System.Drawing.Point(140, 66);
            this.lblSuggestedLink.Name = "lblSuggestedLink";
            this.lblSuggestedLink.Size = new System.Drawing.Size(77, 13);
            this.lblSuggestedLink.TabIndex = 6;
            this.lblSuggestedLink.Text = "Suggested link";
            //
            // btnNextSuggestion
            //
            this.btnNextSuggestion.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextSuggestion.Location = new System.Drawing.Point(312, 103);
            this.btnNextSuggestion.Name = "btnNextSuggestion";
            this.btnNextSuggestion.Size = new System.Drawing.Size(112, 23);
            this.btnNextSuggestion.TabIndex = 7;
            this.btnNextSuggestion.Text = "Next Suggestion";
            this.btnNextSuggestion.UseVisualStyleBackColor = true;
            this.btnNextSuggestion.Click += new System.EventHandler(this.btnNextSuggestion_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(430, 103);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // DlgSuggestedLinksUpdates
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 138);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnNextSuggestion);
            this.Controls.Add(this.lblSuggestedLink);
            this.Controls.Add(this.linkApplySuggestion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblCurrentLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblUpdateName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSuggestedLinksUpdates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Suggested Udates";
            this.Load += new System.EventHandler(this.DlgSuggestedLinksUpdates_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUpdateName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentLink;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkApplySuggestion;
        private System.Windows.Forms.Label lblSuggestedLink;
        private System.Windows.Forms.Button btnNextSuggestion;
        private System.Windows.Forms.Button btnClose;
    }
}