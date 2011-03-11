//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Description of UnhandledExceptionDialog_Designer.
    /// </summary>
    partial class TUnhandledExceptionForm
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
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUnhandledExceptionForm));
            this.pnlUpper = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picPetraBig = new System.Windows.Forms.PictureBox();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblInfo3 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.pnlLower = new System.Windows.Forms.Panel();
            this.btnErrorDetails = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.imlIcons = new System.Windows.Forms.ImageList(this.components);
            this.pnlUpper.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPetraBig)).BeginInit();
            this.pnlMiddle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.pnlLower.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlUpper
            //
            this.pnlUpper.BackColor = System.Drawing.Color.White;
            this.pnlUpper.Controls.Add(this.panel4);
            this.pnlUpper.Controls.Add(this.panel3);
            this.pnlUpper.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUpper.Location = new System.Drawing.Point(0, 0);
            this.pnlUpper.Name = "pnlUpper";
            this.pnlUpper.Size = new System.Drawing.Size(562, 74);
            this.pnlUpper.TabIndex = 0;
            //
            // panel4
            //
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Controls.Add(this.lblInfo1);
            this.panel4.Controls.Add(this.lblHeading);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(81, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(481, 74);
            this.panel4.TabIndex = 4;
            //
            // lblInfo1
            //
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo1.Location = new System.Drawing.Point(0, 23);
            this.lblInfo1.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblInfo1.Size = new System.Drawing.Size(70, 17);
            this.lblInfo1.TabIndex = 2;
            this.lblInfo1.Text = "Info 1 Text";
            //
            // lblHeading
            //
            this.lblHeading.AutoSize = true;
            this.lblHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeading.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(0, 0);
            this.lblHeading.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblHeading.Size = new System.Drawing.Size(69, 23);
            this.lblHeading.TabIndex = 1;
            this.lblHeading.Text = "Title Text";
            //
            // panel3
            //
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.picPetraBig);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(81, 74);
            this.panel3.TabIndex = 3;
            //
            // picPetraBig
            //
            this.picPetraBig.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picPetraBig.Image = ((System.Drawing.Image)(resources.GetObject("picPetraBig.Image")));
            this.picPetraBig.Location = new System.Drawing.Point(16, 14);
            this.picPetraBig.Name = "picPetraBig";
            this.picPetraBig.Size = new System.Drawing.Size(48, 48);
            this.picPetraBig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPetraBig.TabIndex = 0;
            this.picPetraBig.TabStop = false;
            //
            // pnlMiddle
            //
            this.pnlMiddle.AutoSize = true;
            this.pnlMiddle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMiddle.Controls.Add(this.panel2);
            this.pnlMiddle.Controls.Add(this.panel1);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(0, 74);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(562, 85);
            this.pnlMiddle.TabIndex = 1;
            //
            // panel2
            //
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.lblInfo3);
            this.panel2.Controls.Add(this.lblInfo2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(81, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 85);
            this.panel2.TabIndex = 7;
            //
            // lblInfo3
            //
            this.lblInfo3.AutoSize = true;
            this.lblInfo3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo3.Location = new System.Drawing.Point(0, 21);
            this.lblInfo3.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblInfo3.Name = "lblInfo3";
            this.lblInfo3.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.lblInfo3.Size = new System.Drawing.Size(70, 28);
            this.lblInfo3.TabIndex = 4;
            this.lblInfo3.Text = "Info 3 Text";
            //
            // lblInfo2
            //
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo2.Location = new System.Drawing.Point(0, 0);
            this.lblInfo2.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblInfo2.Size = new System.Drawing.Size(70, 21);
            this.lblInfo2.TabIndex = 3;
            this.lblInfo2.Text = "Info 2 Text";
            //
            // panel1
            //
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.picIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(81, 85);
            this.panel1.TabIndex = 6;
            //
            // picIcon
            //
            this.picIcon.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
            this.picIcon.Location = new System.Drawing.Point(24, 28);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(32, 32);
            this.picIcon.TabIndex = 5;
            this.picIcon.TabStop = false;
            //
            // pnlLower
            //
            this.pnlLower.Controls.Add(this.btnErrorDetails);
            this.pnlLower.Controls.Add(this.btnClose);
            this.pnlLower.Controls.Add(this.btnSend);
            this.pnlLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLower.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlLower.Location = new System.Drawing.Point(0, 159);
            this.pnlLower.Name = "pnlLower";
            this.pnlLower.Size = new System.Drawing.Size(562, 46);
            this.pnlLower.TabIndex = 2;
            //
            // btnErrorDetails
            //
            this.btnErrorDetails.AutoSize = true;
            this.btnErrorDetails.Location = new System.Drawing.Point(8, 16);
            this.btnErrorDetails.Name = "btnErrorDetails";
            this.btnErrorDetails.Size = new System.Drawing.Size(111, 23);
            this.btnErrorDetails.TabIndex = 2;
            this.btnErrorDetails.Text = "Error &Details...";
            this.btnErrorDetails.Click += new System.EventHandler(this.BtnErrorDetails_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(218, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close OpenPetra";
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            //
            // btnSend
            //
            this.btnSend.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.AutoSize = true;
            this.btnSend.Location = new System.Drawing.Point(443, 16);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(111, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "&Report Error";
            this.btnSend.Visible = false;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            //
            // imlIcons
            //
            this.imlIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlIcons.ImageStream")));
            this.imlIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlIcons.Images.SetKeyName(0, "");
            this.imlIcons.Images.SetKeyName(1, "");
            this.imlIcons.Images.SetKeyName(2, "petraico-32x32-broken.ico");
            //
            // TUnhandledExceptionForm
            //
            this.AcceptButton = this.btnClose;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(562, 205);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMiddle);
            this.Controls.Add(this.pnlUpper);
            this.Controls.Add(this.pnlLower);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(570, 0);
            this.Name = "TUnhandledExceptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenPetra x.x.x.x Application Error";
            this.Load += new System.EventHandler(this.TUnhandledExceptionForm_Load);
            this.pnlUpper.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPetraBig)).EndInit();
            this.pnlMiddle.ResumeLayout(false);
            this.pnlMiddle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.pnlLower.ResumeLayout(false);
            this.pnlLower.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Panel pnlUpper;
        private System.Windows.Forms.PictureBox picPetraBig;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Panel pnlLower;
        private System.Windows.Forms.Label lblInfo2;
        private System.Windows.Forms.Label lblInfo3;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ImageList imlIcons;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Button btnErrorDetails;
    }
}