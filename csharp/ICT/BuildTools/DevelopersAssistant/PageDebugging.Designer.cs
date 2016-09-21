//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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
    /// <summary>
    /// Designer partial class for the debugging page user control
    /// </summary>
    partial class PageDebugging
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApplyDebugLevels = new System.Windows.Forms.Button();
            this.dataGridViewDebug = new System.Windows.Forms.DataGridView();
            this.nudServerDebugLevel = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.nudClientDebugLevel = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.chkLevelSetByBuildConfig = new System.Windows.Forms.CheckBox();
            this.linkLabelResetDebugging = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDebug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerDebugLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClientDebugLevel)).BeginInit();
            this.SuspendLayout();
            //
            // btnApplyDebugLevels
            //
            this.btnApplyDebugLevels.Location = new System.Drawing.Point(251, 73);
            this.btnApplyDebugLevels.Name = "btnApplyDebugLevels";
            this.btnApplyDebugLevels.Size = new System.Drawing.Size(75, 23);
            this.btnApplyDebugLevels.TabIndex = 5;
            this.btnApplyDebugLevels.Text = "Apply";
            this.btnApplyDebugLevels.UseVisualStyleBackColor = true;
            this.btnApplyDebugLevels.Click += new System.EventHandler(this.btnApplyDebugLevels_Click);
            //
            // dataGridViewDebug
            //
            this.dataGridViewDebug.AllowUserToAddRows = false;
            this.dataGridViewDebug.AllowUserToDeleteRows = false;
            this.dataGridViewDebug.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDebug.Location = new System.Drawing.Point(3, 110);
            this.dataGridViewDebug.Name = "dataGridViewDebug";
            this.dataGridViewDebug.ReadOnly = true;
            this.dataGridViewDebug.RowHeadersVisible = false;
            this.dataGridViewDebug.Size = new System.Drawing.Size(714, 263);
            this.dataGridViewDebug.TabIndex = 6;
            this.dataGridViewDebug.TabStop = false;
            //
            // nudServerDebugLevel
            //
            this.nudServerDebugLevel.Location = new System.Drawing.Point(144, 43);
            this.nudServerDebugLevel.Maximum = new decimal(new int[] {
                    15,
                    0,
                    0,
                    0
                });
            this.nudServerDebugLevel.Name = "nudServerDebugLevel";
            this.nudServerDebugLevel.Size = new System.Drawing.Size(45, 20);
            this.nudServerDebugLevel.TabIndex = 4;
            this.nudServerDebugLevel.ValueChanged += new System.EventHandler(this.nudServerDebugLevel_ValueChanged);
            //
            // label23
            //
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(42, 45);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(96, 13);
            this.label23.TabIndex = 3;
            this.label23.Text = "Server debug level";
            //
            // nudClientDebugLevel
            //
            this.nudClientDebugLevel.Location = new System.Drawing.Point(144, 76);
            this.nudClientDebugLevel.Maximum = new decimal(new int[] {
                    15,
                    0,
                    0,
                    0
                });
            this.nudClientDebugLevel.Name = "nudClientDebugLevel";
            this.nudClientDebugLevel.Size = new System.Drawing.Size(45, 20);
            this.nudClientDebugLevel.TabIndex = 2;
            //
            // label22
            //
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(47, 78);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(91, 13);
            this.label22.TabIndex = 1;
            this.label22.Text = "Client debug level";
            //
            // chkLevelSetByBuildConfig
            //
            this.chkLevelSetByBuildConfig.AutoSize = true;
            this.chkLevelSetByBuildConfig.Location = new System.Drawing.Point(29, 11);
            this.chkLevelSetByBuildConfig.Name = "chkLevelSetByBuildConfig";
            this.chkLevelSetByBuildConfig.Size = new System.Drawing.Size(418, 17);
            this.chkLevelSetByBuildConfig.TabIndex = 0;
            this.chkLevelSetByBuildConfig.Text = "Both client and server levels are set to the same value by the build configuratio" +
                                                 "n file";
            this.chkLevelSetByBuildConfig.UseVisualStyleBackColor = true;
            this.chkLevelSetByBuildConfig.CheckedChanged += new System.EventHandler(this.chkLevelSetByBuildConfig_CheckedChanged);
            //
            // linkLabelResetDebugging
            //
            this.linkLabelResetDebugging.AutoSize = true;
            this.linkLabelResetDebugging.Location = new System.Drawing.Point(3, 383);
            this.linkLabelResetDebugging.Name = "linkLabelResetDebugging";
            this.linkLabelResetDebugging.Size = new System.Drawing.Size(214, 13);
            this.linkLabelResetDebugging.TabIndex = 7;
            this.linkLabelResetDebugging.TabStop = true;
            this.linkLabelResetDebugging.Text = "Reset debugging to default settings (level 0)";
            this.linkLabelResetDebugging.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(
                this.linkLabelResetDebugging_LinkClicked);
            //
            // PageDebugging
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelResetDebugging);
            this.Controls.Add(this.chkLevelSetByBuildConfig);
            this.Controls.Add(this.btnApplyDebugLevels);
            this.Controls.Add(this.dataGridViewDebug);
            this.Controls.Add(this.nudServerDebugLevel);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.nudClientDebugLevel);
            this.Controls.Add(this.label22);
            this.Name = "PageDebugging";
            this.Size = new System.Drawing.Size(720, 400);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDebug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerDebugLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClientDebugLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnApplyDebugLevels;
        private System.Windows.Forms.DataGridView dataGridViewDebug;
        private System.Windows.Forms.NumericUpDown nudServerDebugLevel;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown nudClientDebugLevel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox chkLevelSetByBuildConfig;
        private System.Windows.Forms.LinkLabel linkLabelResetDebugging;
    }
}