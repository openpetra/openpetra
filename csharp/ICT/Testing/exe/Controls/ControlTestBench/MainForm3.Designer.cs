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
partial class MainForm3
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
        this.txtYaml = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.cmbVisualStyle = new System.Windows.Forms.ComboBox();
        this.label2 = new System.Windows.Forms.Label();
        this.btnShepherdTest = new System.Windows.Forms.Button();
        this.btnCollapsibleTest = new System.Windows.Forms.Button();
        this.btnTaskListTest = new System.Windows.Forms.Button();
        this.btnTestAll = new System.Windows.Forms.Button();
        this.btnCollapsibleHosterTest = new System.Windows.Forms.Button();
        this.SuspendLayout();
        //
        // txtYaml
        //
        this.txtYaml.Location = new System.Drawing.Point(169, 183);
        this.txtYaml.Name = "txtYaml";
        this.txtYaml.Size = new System.Drawing.Size(100, 20);
        this.txtYaml.TabIndex = 2;
        this.txtYaml.Text = "testYaml_LedgerNew.yaml";
        //
        // label1
        //
        this.label1.Location = new System.Drawing.Point(27, 183);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(136, 23);
        this.label1.TabIndex = 3;
        this.label1.Text = "YAML File for Tests:";
        //
        // cmbVisualStyle
        //
        this.cmbVisualStyle.FormattingEnabled = true;
        this.cmbVisualStyle.Items.AddRange(new object[] {
                "AccordionPanel",
                "TaskPanel",
                "Dashboard",
                "Shepherd",
                "HorizontalCollapse"
            });
        this.cmbVisualStyle.Location = new System.Drawing.Point(169, 210);
        this.cmbVisualStyle.Name = "cmbVisualStyle";
        this.cmbVisualStyle.Size = new System.Drawing.Size(121, 21);
        this.cmbVisualStyle.TabIndex = 4;
        //
        // label2
        //
        this.label2.Location = new System.Drawing.Point(27, 210);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(136, 44);
        this.label2.TabIndex = 5;
        this.label2.Text = "Visual Style (ignored for \'Shepherd Test\' and \'Test All\'):";
        //
        // btnShepherdTest
        //
        this.btnShepherdTest.Location = new System.Drawing.Point(73, 84);
        this.btnShepherdTest.Name = "btnShepherdTest";
        this.btnShepherdTest.Size = new System.Drawing.Size(182, 23);
        this.btnShepherdTest.TabIndex = 8;
        this.btnShepherdTest.Text = "Shepherd Test";
        this.btnShepherdTest.UseVisualStyleBackColor = true;
        this.btnShepherdTest.Click += new System.EventHandler(this.TestShepherd);
        //
        // btnCollapsibleTest
        //
        this.btnCollapsibleTest.Location = new System.Drawing.Point(73, 55);
        this.btnCollapsibleTest.Name = "btnCollapsibleTest";
        this.btnCollapsibleTest.Size = new System.Drawing.Size(182, 23);
        this.btnCollapsibleTest.TabIndex = 9;
        this.btnCollapsibleTest.Text = "CollapsiblePanel Test";
        this.btnCollapsibleTest.UseVisualStyleBackColor = true;
        this.btnCollapsibleTest.Click += new System.EventHandler(this.BtnCollapsibleTestClick);
        //
        // btnTaskListTest
        //
        this.btnTaskListTest.Location = new System.Drawing.Point(73, 26);
        this.btnTaskListTest.Name = "btnTaskListTest";
        this.btnTaskListTest.Size = new System.Drawing.Size(182, 23);
        this.btnTaskListTest.TabIndex = 10;
        this.btnTaskListTest.Text = "Test TaskList";
        this.btnTaskListTest.Click += new System.EventHandler(this.HandlerTaskListTest);
        //
        // btnTestAll
        //
        this.btnTestAll.Location = new System.Drawing.Point(73, 148);
        this.btnTestAll.Name = "btnTestAll";
        this.btnTestAll.Size = new System.Drawing.Size(182, 23);
        this.btnTestAll.TabIndex = 8;
        this.btnTestAll.Text = "Test All";
        this.btnTestAll.UseVisualStyleBackColor = true;
        this.btnTestAll.Click += new System.EventHandler(this.BtnTestAllClick);
        //
        // btnCollapsibleHosterTest
        //
        this.btnCollapsibleHosterTest.Location = new System.Drawing.Point(73, 113);
        this.btnCollapsibleHosterTest.Name = "btnCollapsibleHosterTest";
        this.btnCollapsibleHosterTest.Size = new System.Drawing.Size(182, 23);
        this.btnCollapsibleHosterTest.TabIndex = 9;
        this.btnCollapsibleHosterTest.Text = "CollapsiblePanelHoster Test";
        this.btnCollapsibleHosterTest.UseVisualStyleBackColor = true;
        this.btnCollapsibleHosterTest.Click += new System.EventHandler(this.BtnCollapsibleHosterTestClick);
        //
        // MainForm3
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(345, 268);
        this.Controls.Add(this.btnCollapsibleHosterTest);
        this.Controls.Add(this.btnCollapsibleTest);
        this.Controls.Add(this.btnTestAll);
        this.Controls.Add(this.btnShepherdTest);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.cmbVisualStyle);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtYaml);
        this.Controls.Add(this.btnTaskListTest);
        this.Name = "MainForm3";
        this.Padding = new System.Windows.Forms.Padding(10);
        this.Text = "ControlTestBench";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Button btnCollapsibleHosterTest;
    private System.Windows.Forms.Button btnTestAll;
    private System.Windows.Forms.Button btnShepherdTest;
    private System.Windows.Forms.Button btnCollapsibleTest;
    private System.Windows.Forms.ComboBox cmbVisualStyle;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtYaml;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnTaskListTest;
}
}