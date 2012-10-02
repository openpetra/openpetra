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
        this.btnGetTaskList1 = new System.Windows.Forms.Button();
        this.btnGetTaskList2 = new System.Windows.Forms.Button();
        this.btnGetCollPanel1 = new System.Windows.Forms.Button();
        this.btnGetCollPanel2 = new System.Windows.Forms.Button();
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
        // btnGetTaskList1
        // 
        this.btnGetTaskList1.Location = new System.Drawing.Point(351, 33);
        this.btnGetTaskList1.Name = "btnGetTaskList1";
        this.btnGetTaskList1.Size = new System.Drawing.Size(255, 23);
        this.btnGetTaskList1.TabIndex = 2;
        this.btnGetTaskList1.Text = "Get Task List #1";
        this.btnGetTaskList1.UseVisualStyleBackColor = true;
        this.btnGetTaskList1.Click += new System.EventHandler(this.BtnGetTaskList1Click);
        // 
        // btnGetTaskList2
        // 
        this.btnGetTaskList2.Location = new System.Drawing.Point(351, 62);
        this.btnGetTaskList2.Name = "btnGetTaskList2";
        this.btnGetTaskList2.Size = new System.Drawing.Size(255, 23);
        this.btnGetTaskList2.TabIndex = 2;
        this.btnGetTaskList2.Text = "Get Task List #2";
        this.btnGetTaskList2.UseVisualStyleBackColor = true;
        this.btnGetTaskList2.Click += new System.EventHandler(this.BtnGetTaskList2Click);
        // 
        // btnGetCollPanel1
        // 
        this.btnGetCollPanel1.Location = new System.Drawing.Point(351, 115);
        this.btnGetCollPanel1.Name = "btnGetCollPanel1";
        this.btnGetCollPanel1.Size = new System.Drawing.Size(255, 23);
        this.btnGetCollPanel1.TabIndex = 2;
        this.btnGetCollPanel1.Text = "Get Collapsible Panel #1";
        this.btnGetCollPanel1.UseVisualStyleBackColor = true;
        this.btnGetCollPanel1.Click += new System.EventHandler(this.BtnGetCollPanel1Click);
        // 
        // btnGetCollPanel2
        // 
        this.btnGetCollPanel2.Location = new System.Drawing.Point(351, 144);
        this.btnGetCollPanel2.Name = "btnGetCollPanel2";
        this.btnGetCollPanel2.Size = new System.Drawing.Size(255, 23);
        this.btnGetCollPanel2.TabIndex = 2;
        this.btnGetCollPanel2.Text = "Get Collapsible Panel #2";
        this.btnGetCollPanel2.UseVisualStyleBackColor = true;
        this.btnGetCollPanel2.Click += new System.EventHandler(this.BtnGetCollPanel2Click);
        // 
        // CollapsiblePanelHosterTest
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(750, 359);
        this.Controls.Add(this.btnGetCollPanel2);
        this.Controls.Add(this.btnGetTaskList2);
        this.Controls.Add(this.btnGetCollPanel1);
        this.Controls.Add(this.btnGetTaskList1);
        this.Controls.Add(this.pnlCollapsiblePanelHostTest);
        this.Name = "CollapsiblePanelHosterTest";
        this.Text = "CollapsiblePanelHosterTest";
        this.ResumeLayout(false);
    }
    private System.Windows.Forms.Button btnGetCollPanel2;
    private System.Windows.Forms.Button btnGetCollPanel1;
    private System.Windows.Forms.Button btnGetTaskList2;
    private System.Windows.Forms.Button btnGetTaskList1;
    private System.Windows.Forms.Panel pnlCollapsiblePanelHostTest;
 }
}