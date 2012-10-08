//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2012 by OM International
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
    partial class DlgAdvancedOptions
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
            this.chkDoPreBuild = new System.Windows.Forms.CheckBox();
            this.chkDoPostBuild = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // chkDoPreBuild
            //
            this.chkDoPreBuild.AutoSize = true;
            this.chkDoPreBuild.Location = new System.Drawing.Point(32, 72);
            this.chkDoPreBuild.Name = "chkDoPreBuild";
            this.chkDoPreBuild.Size = new System.Drawing.Size(442, 17);
            this.chkDoPreBuild.TabIndex = 0;
            this.chkDoPreBuild.Text = "Include a pre-build action on Ict.Common.dll to stop the server before compiling " +
                                      "the code";
            this.chkDoPreBuild.UseVisualStyleBackColor = true;
            //
            // chkDoPostBuild
            //
            this.chkDoPostBuild.AutoSize = true;
            this.chkDoPostBuild.Location = new System.Drawing.Point(32, 95);
            this.chkDoPostBuild.Name = "chkDoPostBuild";
            this.chkDoPostBuild.Size = new System.Drawing.Size(500, 17);
            this.chkDoPostBuild.TabIndex = 1;
            this.chkDoPostBuild.Text = "Include a post-build action on PetraClient.exe to start the server after successf" +
                                       "ully compiling the code";
            this.chkDoPostBuild.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Project code generation (Generate Solution with No Compile only)";
            //
            // btnOk
            //
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(386, 134);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(467, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(29, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(513, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "Be sure to generate the solution with full compile first, then run again with no " +
                               "compile.  After that you can work in your IDE in edit and debug mode.";
            //
            // DlgAdvancedOptions
            //
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(554, 170);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkDoPostBuild);
            this.Controls.Add(this.chkDoPreBuild);
            this.Name = "DlgAdvancedOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Options";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;

        /// <summary>
        /// If checked will generate pre-build action in generateSolutionNoCompile
        /// </summary>
        public System.Windows.Forms.CheckBox chkDoPreBuild;
        /// <summary>
        /// If checked will generate post-build action in generateSolutionNoCompile
        /// </summary>
        public System.Windows.Forms.CheckBox chkDoPostBuild;
    }
}