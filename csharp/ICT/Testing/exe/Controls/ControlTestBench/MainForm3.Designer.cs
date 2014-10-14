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
        this.tabControlTypes = new System.Windows.Forms.TabControl();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.button5 = new System.Windows.Forms.Button();
        this.button4 = new System.Windows.Forms.Button();
        this.btnCollapsibleHosterTest = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.btnCollapsibleTest = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.btnTestAll = new System.Windows.Forms.Button();
        this.label4 = new System.Windows.Forms.Label();
        this.btnShepherdTest = new System.Windows.Forms.Button();
        this.comboBox1 = new System.Windows.Forms.ComboBox();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.cmbVisualStyle = new System.Windows.Forms.ComboBox();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.txtYaml = new System.Windows.Forms.TextBox();
        this.btnTaskListTest = new System.Windows.Forms.Button();
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.btnOpenFilterFindUCTestForm = new System.Windows.Forms.Button();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.btnOpenRTBHyperlinksUCTestForm = new System.Windows.Forms.Button();
        this.tabControlTypes.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.SuspendLayout();
        // 
        // tabControlTypes
        // 
        this.tabControlTypes.Controls.Add(this.tabPage3);
        this.tabControlTypes.Controls.Add(this.tabPage4);
        this.tabControlTypes.Controls.Add(this.tabPage1);
        this.tabControlTypes.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControlTypes.Location = new System.Drawing.Point(10, 10);
        this.tabControlTypes.Name = "tabControlTypes";
        this.tabControlTypes.SelectedIndex = 0;
        this.tabControlTypes.Size = new System.Drawing.Size(437, 274);
        this.tabControlTypes.TabIndex = 11;
        this.tabControlTypes.SelectedIndexChanged += new System.EventHandler(this.TabControlTypesSelectedIndexChanged);
        // 
        // tabPage3
        // 
        this.tabPage3.Controls.Add(this.button5);
        this.tabPage3.Controls.Add(this.button4);
        this.tabPage3.Controls.Add(this.btnCollapsibleHosterTest);
        this.tabPage3.Controls.Add(this.button3);
        this.tabPage3.Controls.Add(this.btnCollapsibleTest);
        this.tabPage3.Controls.Add(this.button2);
        this.tabPage3.Controls.Add(this.btnTestAll);
        this.tabPage3.Controls.Add(this.label4);
        this.tabPage3.Controls.Add(this.btnShepherdTest);
        this.tabPage3.Controls.Add(this.comboBox1);
        this.tabPage3.Controls.Add(this.label2);
        this.tabPage3.Controls.Add(this.label3);
        this.tabPage3.Controls.Add(this.cmbVisualStyle);
        this.tabPage3.Controls.Add(this.textBox1);
        this.tabPage3.Controls.Add(this.label1);
        this.tabPage3.Controls.Add(this.button1);
        this.tabPage3.Controls.Add(this.txtYaml);
        this.tabPage3.Controls.Add(this.btnTaskListTest);
        this.tabPage3.Location = new System.Drawing.Point(4, 22);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage3.Size = new System.Drawing.Size(429, 248);
        this.tabPage3.TabIndex = 0;
        this.tabPage3.Text = "Collapsible Panels";
        this.tabPage3.UseVisualStyleBackColor = true;
        // 
        // button5
        // 
        this.button5.Location = new System.Drawing.Point(52, 94);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(182, 23);
        this.button5.TabIndex = 25;
        this.button5.Text = "CollapsiblePanelHoster Test";
        this.button5.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        this.button4.Location = new System.Drawing.Point(52, 36);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(182, 23);
        this.button4.TabIndex = 24;
        this.button4.Text = "CollapsiblePanel Test";
        this.button4.UseVisualStyleBackColor = true;
        // 
        // btnCollapsibleHosterTest
        // 
        this.btnCollapsibleHosterTest.Location = new System.Drawing.Point(52, 94);
        this.btnCollapsibleHosterTest.Name = "btnCollapsibleHosterTest";
        this.btnCollapsibleHosterTest.Size = new System.Drawing.Size(182, 23);
        this.btnCollapsibleHosterTest.TabIndex = 23;
        this.btnCollapsibleHosterTest.Text = "CollapsiblePanelHoster Test";
        this.btnCollapsibleHosterTest.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(52, 129);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(182, 23);
        this.button3.TabIndex = 20;
        this.button3.Text = "Test All";
        this.button3.UseVisualStyleBackColor = true;
        // 
        // btnCollapsibleTest
        // 
        this.btnCollapsibleTest.Location = new System.Drawing.Point(52, 36);
        this.btnCollapsibleTest.Name = "btnCollapsibleTest";
        this.btnCollapsibleTest.Size = new System.Drawing.Size(182, 23);
        this.btnCollapsibleTest.TabIndex = 26;
        this.btnCollapsibleTest.Text = "CollapsiblePanel Test";
        this.btnCollapsibleTest.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(52, 65);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(182, 23);
        this.button2.TabIndex = 21;
        this.button2.Text = "Shepherd Test";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // btnTestAll
        // 
        this.btnTestAll.Location = new System.Drawing.Point(52, 129);
        this.btnTestAll.Name = "btnTestAll";
        this.btnTestAll.Size = new System.Drawing.Size(182, 23);
        this.btnTestAll.TabIndex = 22;
        this.btnTestAll.Text = "Test All";
        this.btnTestAll.UseVisualStyleBackColor = true;
        // 
        // label4
        // 
        this.label4.Location = new System.Drawing.Point(6, 191);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(136, 44);
        this.label4.TabIndex = 17;
        this.label4.Text = "Visual Style (ignored for \'Shepherd Test\' and \'Test All\'):";
        // 
        // btnShepherdTest
        // 
        this.btnShepherdTest.Location = new System.Drawing.Point(52, 65);
        this.btnShepherdTest.Name = "btnShepherdTest";
        this.btnShepherdTest.Size = new System.Drawing.Size(182, 23);
        this.btnShepherdTest.TabIndex = 19;
        this.btnShepherdTest.Text = "Shepherd Test";
        this.btnShepherdTest.UseVisualStyleBackColor = true;
        // 
        // comboBox1
        // 
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Items.AddRange(new object[] {
                        "AccordionPanel",
                        "TaskPanel",
                        "Dashboard",
                        "Shepherd",
                        "HorizontalCollapse"});
        this.comboBox1.Location = new System.Drawing.Point(148, 191);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new System.Drawing.Size(121, 21);
        this.comboBox1.TabIndex = 15;
        // 
        // label2
        // 
        this.label2.Location = new System.Drawing.Point(6, 191);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(136, 44);
        this.label2.TabIndex = 18;
        this.label2.Text = "Visual Style (ignored for \'Shepherd Test\' and \'Test All\'):";
        // 
        // label3
        // 
        this.label3.Location = new System.Drawing.Point(6, 164);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(136, 23);
        this.label3.TabIndex = 13;
        this.label3.Text = "YAML File for Tests:";
        // 
        // cmbVisualStyle
        // 
        this.cmbVisualStyle.FormattingEnabled = true;
        this.cmbVisualStyle.Items.AddRange(new object[] {
                        "AccordionPanel",
                        "TaskPanel",
                        "Dashboard",
                        "Shepherd",
                        "HorizontalCollapse"});
        this.cmbVisualStyle.Location = new System.Drawing.Point(148, 191);
        this.cmbVisualStyle.Name = "cmbVisualStyle";
        this.cmbVisualStyle.Size = new System.Drawing.Size(121, 21);
        this.cmbVisualStyle.TabIndex = 16;
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(148, 164);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(100, 20);
        this.textBox1.TabIndex = 12;
        this.textBox1.Text = "testYaml_LedgerNew.yaml";
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(6, 164);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(136, 23);
        this.label1.TabIndex = 14;
        this.label1.Text = "YAML File for Tests:";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(52, 7);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(182, 23);
        this.button1.TabIndex = 28;
        this.button1.Text = "Test TaskList";
        // 
        // txtYaml
        // 
        this.txtYaml.Location = new System.Drawing.Point(148, 164);
        this.txtYaml.Name = "txtYaml";
        this.txtYaml.Size = new System.Drawing.Size(100, 20);
        this.txtYaml.TabIndex = 11;
        this.txtYaml.Text = "testYaml_LedgerNew.yaml";
        // 
        // btnTaskListTest
        // 
        this.btnTaskListTest.Location = new System.Drawing.Point(52, 7);
        this.btnTaskListTest.Name = "btnTaskListTest";
        this.btnTaskListTest.Size = new System.Drawing.Size(182, 23);
        this.btnTaskListTest.TabIndex = 27;
        this.btnTaskListTest.Text = "Test TaskList";
        // 
        // tabPage4
        // 
        this.tabPage4.Controls.Add(this.btnOpenFilterFindUCTestForm);
        this.tabPage4.Location = new System.Drawing.Point(4, 22);
        this.tabPage4.Name = "tabPage4";
        this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage4.Size = new System.Drawing.Size(429, 248);
        this.tabPage4.TabIndex = 1;
        this.tabPage4.Text = "Filter/Find UserControl";
        this.tabPage4.UseVisualStyleBackColor = true;
        // 
        // btnOpenFilterFindUCTestForm
        // 
        this.btnOpenFilterFindUCTestForm.Location = new System.Drawing.Point(40, 44);
        this.btnOpenFilterFindUCTestForm.Name = "btnOpenFilterFindUCTestForm";
        this.btnOpenFilterFindUCTestForm.Size = new System.Drawing.Size(189, 23);
        this.btnOpenFilterFindUCTestForm.TabIndex = 0;
        this.btnOpenFilterFindUCTestForm.Text = "Filter/Find UC Test Form";
        this.btnOpenFilterFindUCTestForm.UseVisualStyleBackColor = true;
        this.btnOpenFilterFindUCTestForm.Click += new System.EventHandler(this.BtnOpenFilterFindUCTestFormClick);
        // 
        // tabPage1
        // 
        this.tabPage1.Controls.Add(this.btnOpenRTBHyperlinksUCTestForm);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(429, 248);
        this.tabPage1.TabIndex = 2;
        this.tabPage1.Text = "RichTextBox with Hyperlinks Control";
        this.tabPage1.UseVisualStyleBackColor = true;
        // 
        // btnOpenRTBHyperlinksUCTestForm
        // 
        this.btnOpenRTBHyperlinksUCTestForm.Location = new System.Drawing.Point(40, 44);
        this.btnOpenRTBHyperlinksUCTestForm.Name = "btnOpenRTBHyperlinksUCTestForm";
        this.btnOpenRTBHyperlinksUCTestForm.Size = new System.Drawing.Size(234, 23);
        this.btnOpenRTBHyperlinksUCTestForm.TabIndex = 1;
        this.btnOpenRTBHyperlinksUCTestForm.Text = "RichTextBox with Hyperlinks UC Test Form";
        this.btnOpenRTBHyperlinksUCTestForm.UseVisualStyleBackColor = true;
        this.btnOpenRTBHyperlinksUCTestForm.Click += new System.EventHandler(this.BtnOpenRTBHyperlinksUCTestFormClick);
        // 
        // MainForm3
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(457, 294);
        this.Controls.Add(this.tabControlTypes);
        this.Name = "MainForm3";
        this.Padding = new System.Windows.Forms.Padding(10);
        this.Text = "ControlTestBench";
        this.tabControlTypes.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.tabPage3.PerformLayout();
        this.tabPage4.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.ResumeLayout(false);
    }
    private System.Windows.Forms.Button btnOpenRTBHyperlinksUCTestForm;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Button btnOpenFilterFindUCTestForm;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TabControl tabControlTypes;

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