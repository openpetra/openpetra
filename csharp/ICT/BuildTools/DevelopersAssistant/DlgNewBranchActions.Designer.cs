//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
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
namespace Ict.Tools.DevelopersAssistant
{
    partial class DlgNewBranchActions
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
            this.chkGenerateSolution = new System.Windows.Forms.CheckBox();
            this.chkCreateConfigFiles = new System.Windows.Forms.CheckBox();
            this.chkInitDatabase = new System.Windows.Forms.CheckBox();
            this.cboInitialDatabase = new System.Windows.Forms.ComboBox();
            this.chkOpenIDE = new System.Windows.Forms.CheckBox();
            this.chkStartServerAndClient = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.optMinimalCompile = new System.Windows.Forms.RadioButton();
            this.optFullCompile = new System.Windows.Forms.RadioButton();
            this.cboSolution = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // chkGenerateSolution
            //
            this.chkGenerateSolution.AutoSize = true;
            this.chkGenerateSolution.Location = new System.Drawing.Point(13, 13);
            this.chkGenerateSolution.Name = "chkGenerateSolution";
            this.chkGenerateSolution.Size = new System.Drawing.Size(127, 17);
            this.chkGenerateSolution.TabIndex = 0;
            this.chkGenerateSolution.Text = "Generate the solution";
            this.chkGenerateSolution.UseVisualStyleBackColor = true;
            this.chkGenerateSolution.CheckedChanged += new System.EventHandler(this.chkGenerateSolution_CheckedChanged);
            //
            // chkCreateConfigFiles
            //
            this.chkCreateConfigFiles.AutoSize = true;
            this.chkCreateConfigFiles.Location = new System.Drawing.Point(13, 81);
            this.chkCreateConfigFiles.Name = "chkCreateConfigFiles";
            this.chkCreateConfigFiles.Size = new System.Drawing.Size(260, 17);
            this.chkCreateConfigFiles.TabIndex = 2;
            this.chkCreateConfigFiles.Text = "Create  .my configuration files for client and server";
            this.chkCreateConfigFiles.UseVisualStyleBackColor = true;
            //
            // chkInitDatabase
            //
            this.chkInitDatabase.AutoSize = true;
            this.chkInitDatabase.Location = new System.Drawing.Point(13, 109);
            this.chkInitDatabase.Name = "chkInitDatabase";
            this.chkInitDatabase.Size = new System.Drawing.Size(388, 17);
            this.chkInitDatabase.TabIndex = 3;
            this.chkInitDatabase.Text = "Set up a database for use with this branch and initialise the configuration files" +
                                        "";
            this.chkInitDatabase.UseVisualStyleBackColor = true;
            //
            // cboInitialDatabase
            //
            this.cboInitialDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInitialDatabase.FormattingEnabled = true;
            this.cboInitialDatabase.Location = new System.Drawing.Point(40, 133);
            this.cboInitialDatabase.Name = "cboInitialDatabase";
            this.cboInitialDatabase.Size = new System.Drawing.Size(410, 21);
            this.cboInitialDatabase.TabIndex = 4;
            //
            // chkOpenIDE
            //
            this.chkOpenIDE.AutoSize = true;
            this.chkOpenIDE.Location = new System.Drawing.Point(13, 167);
            this.chkOpenIDE.Name = "chkOpenIDE";
            this.chkOpenIDE.Size = new System.Drawing.Size(209, 17);
            this.chkOpenIDE.TabIndex = 5;
            this.chkOpenIDE.Text = "Open the solution in your preferred IDE";
            this.chkOpenIDE.UseVisualStyleBackColor = true;
            //
            // chkStartServerAndClient
            //
            this.chkStartServerAndClient.AutoSize = true;
            this.chkStartServerAndClient.Location = new System.Drawing.Point(13, 221);
            this.chkStartServerAndClient.Name = "chkStartServerAndClient";
            this.chkStartServerAndClient.Size = new System.Drawing.Size(147, 17);
            this.chkStartServerAndClient.TabIndex = 6;
            this.chkStartServerAndClient.Text = "Start the server and client";
            this.chkStartServerAndClient.UseVisualStyleBackColor = true;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 253);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status:";
            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(13, 269);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(254, 15);
            this.progressBar.TabIndex = 8;
            //
            // btnStart
            //
            this.btnStart.Location = new System.Drawing.Point(294, 261);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(375, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // panel1
            //
            this.panel1.Controls.Add(this.optMinimalCompile);
            this.panel1.Controls.Add(this.optFullCompile);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 46);
            this.panel1.TabIndex = 1;
            //
            // optMinimalCompile
            //
            this.optMinimalCompile.AutoSize = true;
            this.optMinimalCompile.Location = new System.Drawing.Point(28, 26);
            this.optMinimalCompile.Name = "optMinimalCompile";
            this.optMinimalCompile.Size = new System.Drawing.Size(233, 17);
            this.optMinimalCompile.TabIndex = 1;
            this.optMinimalCompile.TabStop = true;
            this.optMinimalCompile.Text = "Generate the solution with a minimal compile";
            this.optMinimalCompile.UseVisualStyleBackColor = true;
            this.optMinimalCompile.CheckedChanged += new System.EventHandler(this.optMinimalCompile_CheckedChanged);
            //
            // optFullCompile
            //
            this.optFullCompile.AutoSize = true;
            this.optFullCompile.Location = new System.Drawing.Point(28, 4);
            this.optFullCompile.Name = "optFullCompile";
            this.optFullCompile.Size = new System.Drawing.Size(212, 17);
            this.optFullCompile.TabIndex = 0;
            this.optFullCompile.TabStop = true;
            this.optFullCompile.Text = "Generate the solution with a full compile";
            this.optFullCompile.UseVisualStyleBackColor = true;
            this.optFullCompile.CheckedChanged += new System.EventHandler(this.optFullCompile_CheckedChanged);
            //
            // cboSolution
            //
            this.cboSolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSolution.FormattingEnabled = true;
            this.cboSolution.Location = new System.Drawing.Point(40, 189);
            this.cboSolution.Name = "cboSolution";
            this.cboSolution.Size = new System.Drawing.Size(88, 21);
            this.cboSolution.TabIndex = 11;
            //
            // DlgNewBranchActions
            //
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(462, 297);
            this.Controls.Add(this.cboSolution);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.chkStartServerAndClient);
            this.Controls.Add(this.chkOpenIDE);
            this.Controls.Add(this.cboInitialDatabase);
            this.Controls.Add(this.chkInitDatabase);
            this.Controls.Add(this.chkCreateConfigFiles);
            this.Controls.Add(this.chkGenerateSolution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgNewBranchActions";
            this.ShowInTaskbar = false;
            this.Text = "Further Actions for a New Branch";
            this.Shown += new System.EventHandler(this.DlgNewBranchActions_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkGenerateSolution;
        private System.Windows.Forms.CheckBox chkCreateConfigFiles;
        private System.Windows.Forms.CheckBox chkInitDatabase;
        private System.Windows.Forms.ComboBox cboInitialDatabase;
        private System.Windows.Forms.CheckBox chkOpenIDE;
        private System.Windows.Forms.CheckBox chkStartServerAndClient;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optMinimalCompile;
        private System.Windows.Forms.RadioButton optFullCompile;
        private System.Windows.Forms.ComboBox cboSolution;
    }
}