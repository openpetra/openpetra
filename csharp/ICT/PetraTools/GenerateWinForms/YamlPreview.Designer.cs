//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
namespace Ict.Tools.GenerateWinForms
{
    partial class TFrmYamlPreview
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmYamlPreview));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.pnlWindow = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.btnPreview
                });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(660, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            //
            // btnPreview
            //
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(49, 22);
            this.btnPreview.Text = "Refresh";
            this.btnPreview.ToolTipText = "Refresh";
            this.btnPreview.Click += new System.EventHandler(this.btnPreviewClick);
            //
            // pnlWindow
            //
            this.pnlWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWindow.Location = new System.Drawing.Point(0, 25);
            this.pnlWindow.Name = "pnlWindow";
            this.pnlWindow.Size = new System.Drawing.Size(660, 309);
            this.pnlWindow.TabIndex = 2;
            //
            // TFrmYamlPreview
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 334);
            this.Controls.Add(this.pnlWindow);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TFrmYamlPreview";
            this.Text = "YamlPreview";
            this.Activated += new System.EventHandler(this.TFrmYamlPreviewActivated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlWindow;

        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}