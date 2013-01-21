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
using System.Xml;
using System;
namespace ControlTestBench
{
partial class TaskListCheck
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
    private void InitializeComponent(XmlNode node, Ict.Common.Controls.TVisualStylesEnum Style)
    {
        this.taskList1 = new Ict.Common.Controls.TTaskList(node, Style);
        this.DisableItemButton = new System.Windows.Forms.Button();
        this.HideItemButton = new System.Windows.Forms.Button();
        //this.taskList1.VisualStyle = new Ict.Common.Controls.TVisualStyles(Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel);
        this.container = new System.Windows.Forms.Panel();
        this.SuspendLayout();
        //
        // taskList1
        //
        this.taskList1.BackColor = System.Drawing.SystemColors.AppWorkspace;
        //this.taskList1.Location = new System.Drawing.Point(100, 50);
        this.taskList1.Name = "taskList1";
        this.taskList1.TabIndex = 0;
        //
        // container
        //

        this.container.BackColor = System.Drawing.SystemColors.AppWorkspace;
        this.container.Location = new System.Drawing.Point(83, 53);
        this.container.Name = "taskListContainer";
        this.container.AutoSize = true;
        this.container.Size = new System.Drawing.Size(150, 150);
        this.container.TabIndex = 0;

        //this.DisableItemButton
        this.DisableItemButton.Location = new System.Drawing.Point(0, 50);
        this.DisableItemButton.Name = "DisableItemButton";
        this.DisableItemButton.Size = new System.Drawing.Size(75, 23);
        this.DisableItemButton.TabIndex = 1;
        this.DisableItemButton.Text = "DisableItemButton";
        this.DisableItemButton.UseVisualStyleBackColor = true;
        this.DisableItemButton.Click += new System.EventHandler(this.DisableItemButtonClick);

        //this.HideItemButton
        this.HideItemButton.Location = new System.Drawing.Point(0, 100);
        this.HideItemButton.Name = "HideItemButton";
        this.HideItemButton.Size = new System.Drawing.Size(75, 23);
        this.HideItemButton.TabIndex = 1;
        this.HideItemButton.Text = "HideItemButton";
        this.HideItemButton.UseVisualStyleBackColor = true;
        this.HideItemButton.Click += new System.EventHandler(this.HideItemButtonClick);
        //
        // TaskListCheck
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(500, 500);
        this.container.Controls.Add(this.taskList1);
        this.Controls.Add(this.container);
        this.Controls.Add(this.DisableItemButton);
        this.Controls.Add(this.HideItemButton);
        this.Name = "TaskListCheck";
        this.Text = "TaskListCheck";
        this.ResumeLayout(false);
    }

    void DisableItemButtonClick(object sender, EventArgs e)
    {
        XmlNode temp = this.taskList1.GetTaskByNumber("4", true);

        if (temp != null)
        {
            if (this.taskList1.IsDisabled(temp))
            {
                this.taskList1.DisableTaskItem(temp);
            }
            else
            {
                this.taskList1.EnableTaskItem(temp);
            }
        }
    }

    void HideItemButtonClick(object sender, EventArgs e)
    {
        XmlNode temp = this.taskList1.GetTaskByName("Task3a");

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

    private Ict.Common.Controls.TTaskList taskList1;
    private System.Windows.Forms.Panel container;
    private System.Windows.Forms.Button DisableItemButton;
    private System.Windows.Forms.Button HideItemButton;
}
}