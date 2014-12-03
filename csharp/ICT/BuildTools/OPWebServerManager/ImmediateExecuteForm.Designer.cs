//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2014 by OM International
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
namespace Ict.Tools.OPWebServerManager
{
    partial class ImmediateExecuteForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImmediateExecuteForm));
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressResponses = new System.Windows.Forms.ProgressBar();
            this.responseTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // lblStatus
            //
            this.lblStatus.Location = new System.Drawing.Point(13, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(387, 49);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            //
            // progressResponses
            //
            this.progressResponses.Location = new System.Drawing.Point(12, 65);
            this.progressResponses.Maximum = 2500;
            this.progressResponses.Name = "progressResponses";
            this.progressResponses.Size = new System.Drawing.Size(388, 17);
            this.progressResponses.TabIndex = 1;
            //
            // responseTimer
            //
            this.responseTimer.Interval = 200;
            this.responseTimer.Tick += new System.EventHandler(this.responseTimer_Tick);
            //
            // ImmediateExecuteForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 94);
            this.Controls.Add(this.progressResponses);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImmediateExecuteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenPetra Development Web Server Manager";
            this.Shown += new System.EventHandler(this.ImmediateExecuteForm_Shown);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressResponses;
        private System.Windows.Forms.Timer responseTimer;
    }
}