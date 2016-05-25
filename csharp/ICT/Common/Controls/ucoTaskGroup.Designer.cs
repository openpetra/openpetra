//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
namespace Ict.Common.Controls
{
    partial class TUcoTaskGroup
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
            this.nlnGroupTitle = new TLneCaption();
            this.flpTaskGroup = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            //
            // nlnGroupTitle
            //
//			this.nlnGroupTitle.BackColor = System.Drawing.Color.White;
            this.nlnGroupTitle.Caption = "General";
            this.nlnGroupTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.nlnGroupTitle.Font =
                new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nlnGroupTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.nlnGroupTitle.Location = new System.Drawing.Point(0, 0);
            this.nlnGroupTitle.Name = "nlnGroupTitle";
            this.nlnGroupTitle.Size = new System.Drawing.Size(805, 22);
            this.nlnGroupTitle.TabIndex = 2;
            this.nlnGroupTitle.TabStop = false;
            //
            // flpTaskGroup
            //
            this.flpTaskGroup.AutoSize = true;
            this.flpTaskGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
//			this.flpTaskGroup.BackColor = System.Drawing.Color.White;
            this.flpTaskGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpTaskGroup.Location = new System.Drawing.Point(0, 22);
            this.flpTaskGroup.Name = "flpTaskGroup";
            this.flpTaskGroup.Padding = new System.Windows.Forms.Padding(0, 3, 0, 7);
            this.flpTaskGroup.Size = new System.Drawing.Size(805, 417);
            this.flpTaskGroup.TabIndex = 3;
            //
            // TUcoTaskGroup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//			this.BackColor = System.Drawing.Color.Red;
            this.Controls.Add(this.flpTaskGroup);
            this.Controls.Add(this.nlnGroupTitle);
            this.Name = "TUcoTaskGroup";
            this.Size = new System.Drawing.Size(805, 439);
            this.Resize += new System.EventHandler(this.TUcoTaskGroupResize);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.FlowLayoutPanel flpTaskGroup;
        private TLneCaption nlnGroupTitle;
    }
}