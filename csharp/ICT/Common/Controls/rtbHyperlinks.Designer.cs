//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
namespace Ict.Common.Controls
{
    partial class TRtbHyperlinks
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
            this.rtbTextWithLinks = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbTextWithLinks
            // 
            this.rtbTextWithLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTextWithLinks.Location = new System.Drawing.Point(0, 0);
            this.rtbTextWithLinks.Name = "rtbTextWithLinks";
            this.rtbTextWithLinks.ReadOnly = true;
            this.rtbTextWithLinks.Size = new System.Drawing.Size(300, 80);
            this.rtbTextWithLinks.TabIndex = 0;
            this.rtbTextWithLinks.Text = "";
            // 
            // TRtbHyperlinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbTextWithLinks);
            this.Name = "TRtbHyperlinks";
            this.Size = new System.Drawing.Size(300, 80);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.RichTextBox rtbTextWithLinks;
    }
}