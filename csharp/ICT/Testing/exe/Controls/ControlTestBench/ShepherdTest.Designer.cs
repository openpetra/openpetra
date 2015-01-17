//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Taylor Students
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
namespace ControlTestBench
{
    partial class ShepherdTest
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
            this.tPnlCollapsible1 = new Ict.Common.Controls.TPnlCollapsible();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            //
            // tPnlCollapsible1
            //
            this.tPnlCollapsible1.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
            this.tPnlCollapsible1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tPnlCollapsible1.ExpandedSize = 200;
            this.tPnlCollapsible1.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.tPnlCollapsible1.Location = new System.Drawing.Point(0, 0);
            this.tPnlCollapsible1.Margin = new System.Windows.Forms.Padding(0);
            this.tPnlCollapsible1.Name = "tPnlCollapsible1";
            this.tPnlCollapsible1.Size = new System.Drawing.Size(200, 359);
            this.tPnlCollapsible1.TabIndex = 0;
            this.tPnlCollapsible1.Text = "Shepherd Test";
            this.tPnlCollapsible1.UserControlClass = "";
            this.tPnlCollapsible1.UserControlNamespace = "";
            this.tPnlCollapsible1.UserControlString = ".";
            this.tPnlCollapsible1.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(200, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 359);
            this.panel1.TabIndex = 1;
            //
            // ShepherdTest
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 359);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tPnlCollapsible1);
            this.Name = "ShepherdTest";
            this.Text = "ShepherdTest";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;
        private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible1;
    }
}