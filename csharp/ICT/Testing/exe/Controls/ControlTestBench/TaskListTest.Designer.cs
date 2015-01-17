//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 sethb
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
using Ict.Common.Controls;

namespace ControlTestBench
{
    partial class TaskListTest
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
            this.btnTestDefaultConstructor = new System.Windows.Forms.Button();
            this.btnTestFullConstructor = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnGetActiveTask = new System.Windows.Forms.Button();
            this.btnSetActiveTask = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // btnTestDefaultConstructor
            //
            this.btnTestDefaultConstructor.Location = new System.Drawing.Point(292, 86);
            this.btnTestDefaultConstructor.Name = "btnTestDefaultConstructor";
            this.btnTestDefaultConstructor.Size = new System.Drawing.Size(147, 23);
            this.btnTestDefaultConstructor.TabIndex = 0;
            this.btnTestDefaultConstructor.Text = "Test Default Constructor";
            this.btnTestDefaultConstructor.UseVisualStyleBackColor = true;
            this.btnTestDefaultConstructor.Click += new System.EventHandler(this.TestDefaultConstructor);
            //
            // btnTestFullConstructor
            //
            this.btnTestFullConstructor.Location = new System.Drawing.Point(292, 115);
            this.btnTestFullConstructor.Name = "btnTestFullConstructor";
            this.btnTestFullConstructor.Size = new System.Drawing.Size(147, 23);
            this.btnTestFullConstructor.TabIndex = 1;
            this.btnTestFullConstructor.Text = "Test Full Constructor";
            this.btnTestFullConstructor.UseVisualStyleBackColor = true;
            this.btnTestFullConstructor.Click += new System.EventHandler(this.TestFullConstructor);
            //
            // btnDisable
            //
            this.btnDisable.Location = new System.Drawing.Point(292, 173);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(147, 23);
            this.btnDisable.TabIndex = 4;
            this.btnDisable.Text = "Disable/Enable Task";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.DisableItemButtonClick);
            //
            // btnHide
            //
            this.btnHide.Location = new System.Drawing.Point(292, 202);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(147, 23);
            this.btnHide.TabIndex = 5;
            this.btnHide.Text = "Hide/Show Task";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.HideItemButtonClick);
            //
            // btnGetActiveTask
            //
            this.btnGetActiveTask.Location = new System.Drawing.Point(292, 287);
            this.btnGetActiveTask.Name = "btnGetActiveTask";
            this.btnGetActiveTask.Size = new System.Drawing.Size(147, 23);
            this.btnGetActiveTask.TabIndex = 6;
            this.btnGetActiveTask.Text = "Get Active Task";
            this.btnGetActiveTask.UseVisualStyleBackColor = true;
            this.btnGetActiveTask.Click += new System.EventHandler(this.BtnGetActiveTaskClick);
            //
            // btnSetActiveTask
            //
            this.btnSetActiveTask.Location = new System.Drawing.Point(23, 56);
            this.btnSetActiveTask.Name = "btnSetActiveTask";
            this.btnSetActiveTask.Size = new System.Drawing.Size(161, 23);
            this.btnSetActiveTask.TabIndex = 6;
            this.btnSetActiveTask.Text = "Activate Task by This Name";
            this.btnSetActiveTask.UseVisualStyleBackColor = true;
            this.btnSetActiveTask.Click += new System.EventHandler(this.BtnSetActiveTaskClick);
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtTaskName);
            this.groupBox1.Controls.Add(this.btnSetActiveTask);
            this.groupBox1.Location = new System.Drawing.Point(269, 316);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 89);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activate Task";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Task Name:";
            //
            // txtTaskName
            //
            this.txtTaskName.Location = new System.Drawing.Point(80, 25);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(114, 20);
            this.txtTaskName.TabIndex = 7;
            //
            // TaskListTest
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(674, 500);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetActiveTask);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnTestFullConstructor);
            this.Controls.Add(this.btnTestDefaultConstructor);
            this.Name = "TaskListTest";
            this.Text = "TaskList Test Form Window";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetActiveTask;
        private System.Windows.Forms.Button btnGetActiveTask;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnTestFullConstructor;
        private System.Windows.Forms.Button btnTestDefaultConstructor;
        private Ict.Common.Controls.TTaskList TaskList1;
    }
}