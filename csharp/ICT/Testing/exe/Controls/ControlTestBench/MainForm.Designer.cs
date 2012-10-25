//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Taylor Students
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
//using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;

namespace Ict.Testing.ControlTestBench
{
    partial class MainForm
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
            this.DisableItemButton = new System.Windows.Forms.Button();
            this.HideItemButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtYaml = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVisualStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // DisableItemButton
            //
            this.DisableItemButton.Location = new System.Drawing.Point(52, 119);
            this.DisableItemButton.Name = "DisableItemButton";
            this.DisableItemButton.Size = new System.Drawing.Size(75, 23);
            this.DisableItemButton.TabIndex = 1;
            this.DisableItemButton.Text = "DisableItemButton";
            this.DisableItemButton.UseVisualStyleBackColor = true;
            this.DisableItemButton.Click += new System.EventHandler(this.DisableItemButtonClick);
            //
            // HideItemButton
            //
            this.HideItemButton.Location = new System.Drawing.Point(159, 119);
            this.HideItemButton.Name = "HideItemButton";
            this.HideItemButton.Size = new System.Drawing.Size(75, 23);
            this.HideItemButton.TabIndex = 1;
            this.HideItemButton.Text = "HideItemButton";
            this.HideItemButton.UseVisualStyleBackColor = true;
            this.HideItemButton.Click += new System.EventHandler(this.HideItemButtonClick);
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(52, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "TaskList Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            //
            // txtYaml
            //
            this.txtYaml.Location = new System.Drawing.Point(134, 29);
            this.txtYaml.Name = "txtYaml";
            this.txtYaml.Size = new System.Drawing.Size(100, 20);
            this.txtYaml.TabIndex = 2;
            this.txtYaml.Text = "testYaml.yaml";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(13, 29);
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
                    "HorizontalCollapse"
                });
            this.cmbVisualStyle.Location = new System.Drawing.Point(134, 56);
            this.cmbVisualStyle.Name = "cmbVisualStyle";
            this.cmbVisualStyle.Size = new System.Drawing.Size(121, 21);
            this.cmbVisualStyle.TabIndex = 4;
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Visual Style";
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(52, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(182, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "TPnlCollapsible Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            //
            // button3
            //
            this.button3.Location = new System.Drawing.Point(52, 197);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(182, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Shepherd Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(700, 273);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.DisableItemButton);
            this.Controls.Add(this.HideItemButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbVisualStyle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtYaml);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "ControlTestBench";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private Ict.Common.Controls.TTaskList taskList1;
        private System.Windows.Forms.ComboBox cmbVisualStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtYaml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button DisableItemButton;
        private System.Windows.Forms.Button HideItemButton;


        void DisableItemButtonClick(object sender, EventArgs e)
        {
            XmlNode temp = this.taskList1.GetTaskByName("Task7");

            if (temp != null)
            {
                if (this.taskList1.IsDisabled(temp))
                {
                    this.taskList1.EnableTaskItem(temp);
                }
                else
                {
                    this.taskList1.DisableTaskItem(temp);
                }
            }
        }

        void HideItemButtonClick(object sender, EventArgs e)
        {
            XmlNode temp = this.taskList1.GetTaskByName("Task4c");

            if (temp != null)
            {
                if (!this.taskList1.IsVisible(temp))
                {
                    this.taskList1.ShowTaskItem(temp);
                }
                else
                {
                    this.taskList1.HideTaskItem(temp);
                }
            }

//			temp = this.taskList1.GetTaskByNumber("3");
//			if(temp != null){
//				this.taskList1.ShowTaskItem(temp);
//			}
        }
    }
}