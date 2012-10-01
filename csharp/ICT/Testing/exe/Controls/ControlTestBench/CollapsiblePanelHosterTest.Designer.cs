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
using System.Windows.Forms;
using Ict.Common.Controls;

namespace ControlTestBench
{
partial class CollapsiblePanelHosterTest
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
        this.pnlCollapsiblePanelHostTest = new System.Windows.Forms.Panel();
        this.SuspendLayout();
        // 
        // pnlCollapsiblePanelHostTest
        // 
        this.pnlCollapsiblePanelHostTest.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pnlCollapsiblePanelHostTest.Dock = System.Windows.Forms.DockStyle.Left;
        this.pnlCollapsiblePanelHostTest.Location = new System.Drawing.Point(0, 0);
        this.pnlCollapsiblePanelHostTest.Name = "pnlCollapsiblePanelHostTest";
        this.pnlCollapsiblePanelHostTest.Size = new System.Drawing.Size(221, 359);
        this.pnlCollapsiblePanelHostTest.TabIndex = 1;
        // 
        // CollapsiblePanelHosterTest
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(750, 359);
        this.Controls.Add(this.pnlCollapsiblePanelHostTest);
        this.Name = "CollapsiblePanelHosterTest";
        this.Text = "CollapsiblePanelHosterTest";
        this.ResumeLayout(false);
    }
    private System.Windows.Forms.Panel pnlCollapsiblePanelHostTest;
 }
}