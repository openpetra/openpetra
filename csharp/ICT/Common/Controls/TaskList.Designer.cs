//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
//       sethb
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
namespace Ict.Common.Controls
{
    partial class TTaskList
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Disposes resources used by the control.
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
            this.tPnlGradient1 = new Ict.Common.Controls.TPnlGradient();
            this.tPnlGradient1.SuspendLayout();
            this.SuspendLayout();
            //
            // tPnlGradient1
            //
            this.tPnlGradient1.AutoScroll = true;
            this.tPnlGradient1.AutoSize = true;
            this.tPnlGradient1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tPnlGradient1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tPnlGradient1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPnlGradient1.DontDrawBottomLine = false;
            this.tPnlGradient1.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(203)))), ((int)(((byte)(231)))));
            this.tPnlGradient1.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.tPnlGradient1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.tPnlGradient1.Location = new System.Drawing.Point(0, 0);
            this.tPnlGradient1.Padding = new System.Windows.Forms.Padding(10);
            this.tPnlGradient1.Name = "tPnlGradient1";
            this.tPnlGradient1.Size = new System.Drawing.Size(81, 32);
            this.tPnlGradient1.TabIndex = 0;

            //
            // TTaskList
            //
            this.AccessibleName = "pnlModule";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tPnlGradient1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TTaskList";
            this.Size = new System.Drawing.Size(81, 32);
            this.tPnlGradient1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TPnlGradient tPnlGradient1;
    }
}