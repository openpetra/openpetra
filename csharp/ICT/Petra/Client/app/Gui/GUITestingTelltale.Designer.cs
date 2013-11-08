//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
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
namespace Ict.Petra.Client.App.Gui
{
    partial class TGuiTestingTelltaleWinForm
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
            if (disposing) {
                if (components != null) {
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
        	this.label1 = new System.Windows.Forms.Label();
        	this.lblClient = new System.Windows.Forms.Label();
        	this.lblClientGroup = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.lblTestingFile = new System.Windows.Forms.Label();
        	this.label10 = new System.Windows.Forms.Label();
        	this.lblRepeats = new System.Windows.Forms.Label();
        	this.label5 = new System.Windows.Forms.Label();
        	this.lblDisconnectTime = new System.Windows.Forms.Label();
        	this.label7 = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(4, 4);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(85, 16);
        	this.label1.TabIndex = 0;
        	this.label1.Text = "Client:";
        	// 
        	// lblClient
        	// 
        	this.lblClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lblClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblClient.Location = new System.Drawing.Point(85, 4);
        	this.lblClient.Name = "lblClient";
        	this.lblClient.Size = new System.Drawing.Size(159, 16);
        	this.lblClient.TabIndex = 1;
        	// 
        	// lblClientGroup
        	// 
        	this.lblClientGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lblClientGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblClientGroup.Location = new System.Drawing.Point(85, 20);
        	this.lblClientGroup.Name = "lblClientGroup";
        	this.lblClientGroup.Size = new System.Drawing.Size(159, 16);
        	this.lblClientGroup.TabIndex = 3;
        	this.lblClientGroup.Visible = false;
        	// 
        	// label3
        	// 
        	this.label3.Location = new System.Drawing.Point(4, 20);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(85, 16);
        	this.label3.TabIndex = 2;
        	this.label3.Text = "ClientGroup:";
        	this.label3.Visible = false;
        	// 
        	// lblTestingFile
        	// 
        	this.lblTestingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lblTestingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblTestingFile.Location = new System.Drawing.Point(85, 36);
        	this.lblTestingFile.Name = "lblTestingFile";
        	this.lblTestingFile.Size = new System.Drawing.Size(159, 16);
        	this.lblTestingFile.TabIndex = 5;
        	// 
        	// label10
        	// 
        	this.label10.Location = new System.Drawing.Point(4, 36);
        	this.label10.Name = "label10";
        	this.label10.Size = new System.Drawing.Size(85, 16);
        	this.label10.TabIndex = 4;
        	this.label10.Text = "TestingFile:";
        	// 
        	// lblRepeats
        	// 
        	this.lblRepeats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lblRepeats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblRepeats.Location = new System.Drawing.Point(85, 52);
        	this.lblRepeats.Name = "lblRepeats";
        	this.lblRepeats.Size = new System.Drawing.Size(159, 16);
        	this.lblRepeats.TabIndex = 7;
        	// 
        	// label5
        	// 
        	this.label5.Location = new System.Drawing.Point(4, 52);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(85, 16);
        	this.label5.TabIndex = 6;
        	this.label5.Text = "Repeats:";
        	// 
        	// lblDisconnectTime
        	// 
        	this.lblDisconnectTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lblDisconnectTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblDisconnectTime.Location = new System.Drawing.Point(85, 68);
        	this.lblDisconnectTime.Name = "lblDisconnectTime";
        	this.lblDisconnectTime.Size = new System.Drawing.Size(159, 16);
        	this.lblDisconnectTime.TabIndex = 9;
        	// 
        	// label7
        	// 
        	this.label7.Location = new System.Drawing.Point(4, 68);
        	this.label7.Name = "label7";
        	this.label7.Size = new System.Drawing.Size(85, 16);
        	this.label7.TabIndex = 8;
        	this.label7.Text = "Disc.Time:";
        	// 
        	// TGuiTestingTelltaleWinForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(248, 87);
        	this.Controls.Add(this.lblDisconnectTime);
        	this.Controls.Add(this.label7);
        	this.Controls.Add(this.lblRepeats);
        	this.Controls.Add(this.label5);
        	this.Controls.Add(this.lblTestingFile);
        	this.Controls.Add(this.label10);
        	this.Controls.Add(this.lblClientGroup);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.lblClient);
        	this.Controls.Add(this.label1);
        	this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "TGuiTestingTelltaleWinForm";
        	this.ShowInTaskbar = false;
        	this.Text = "GuiTesting Telltale Window";
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.Label lblDisconnectTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTestingFile;
        private System.Windows.Forms.Label lblRepeats;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblClientGroup;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label label1;
    }
}