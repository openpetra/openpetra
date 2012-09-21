/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 13:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
//using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
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
            if (disposing) {
                if (components != null) {
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
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtYaml
            // 
            this.txtYaml.Location = new System.Drawing.Point(169, 171);
            this.txtYaml.Name = "txtYaml";
            this.txtYaml.Size = new System.Drawing.Size(100, 20);
            this.txtYaml.TabIndex = 2;
            this.txtYaml.Text = "testYaml_AP.yaml";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(48, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "enter yaml file:";
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
            this.cmbVisualStyle.Location = new System.Drawing.Point(169, 198);
            this.cmbVisualStyle.Name = "cmbVisualStyle";
            this.cmbVisualStyle.Size = new System.Drawing.Size(121, 21);
            this.cmbVisualStyle.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(48, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Visual Style";
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
            this.btnCollapsibleTest.Text = "TPnlCollapsible Test";
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
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(48, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(242, 36);
            this.label3.TabIndex = 11;
            this.label3.Text = "The below settings are only used for Shepherd Test. I wish we could decouple this" +
            "!";
            // 
            // MainForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(345, 268);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCollapsibleTest);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShepherdTest;
        private System.Windows.Forms.Button btnCollapsibleTest;
        private System.Windows.Forms.ComboBox cmbVisualStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtYaml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTaskListTest;

    }
}
