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
using System.Windows.Forms;
using Ict.Common.Controls;
using System.Xml;
using Ict.Common.IO;

namespace ControlTestBench
{
partial class MainForm2
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
        this.button1 = new System.Windows.Forms.Button();
        this.tPnlCollapsible1 = new Ict.Common.Controls.TPnlCollapsible();
        this.tPnlCollapsible2 = new Ict.Common.Controls.TPnlCollapsible();
        this.tPnlCollapsible3 = new Ict.Common.Controls.TPnlCollapsible();
        this.button2 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.textBox4 = new System.Windows.Forms.TextBox();
        this.textBox5 = new System.Windows.Forms.TextBox();
        this.textBox6 = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        //
        // button1
        //
        this.button1.Location = new System.Drawing.Point(313, 381);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(86, 23);
        this.button1.TabIndex = 3;
        this.button1.Text = "Toggle Bottom";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.Button1Click);
        //
        // tPnlCollapsible1
        //
        this.tPnlCollapsible1.AutoSize = true;
        this.tPnlCollapsible1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tPnlCollapsible1.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
        this.tPnlCollapsible1.Dock = System.Windows.Forms.DockStyle.Left;
        this.tPnlCollapsible1.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        this.tPnlCollapsible1.Location = new System.Drawing.Point(0, 291);
        this.tPnlCollapsible1.Margin = new System.Windows.Forms.Padding(0);
        this.tPnlCollapsible1.Name = "tPnlCollapsible1";
        this.tPnlCollapsible1.Size = new System.Drawing.Size(297, 173);
        this.tPnlCollapsible1.TabIndex = 0;
        this.tPnlCollapsible1.TaskListInstance = null;
        this.tPnlCollapsible1.TaskListNode = null;
        this.tPnlCollapsible1.UserControlClass = "";
        this.tPnlCollapsible1.UserControlNamespace = "";
        this.tPnlCollapsible1.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
        //
        // tPnlCollapsible2
        //
        this.tPnlCollapsible2.AutoSize = true;
        this.tPnlCollapsible2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tPnlCollapsible2.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.tPnlCollapsible2.Dock = System.Windows.Forms.DockStyle.Top;
        this.tPnlCollapsible2.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckUserControl;
        this.tPnlCollapsible2.Location = new System.Drawing.Point(0, 151);
        this.tPnlCollapsible2.Margin = new System.Windows.Forms.Padding(0);
        this.tPnlCollapsible2.Name = "tPnlCollapsible2";
        this.tPnlCollapsible2.Size = new System.Drawing.Size(669, 140);
        this.tPnlCollapsible2.TabIndex = 0;
        this.tPnlCollapsible2.TaskListInstance = null;
        this.tPnlCollapsible2.TaskListNode = null;
        this.tPnlCollapsible2.UserControlClass = "TUC_PartnerInfo";
        this.tPnlCollapsible2.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
        this.tPnlCollapsible2.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
        //
        // tPnlCollapsible3
        //
        this.tPnlCollapsible3.AutoSize = true;
        this.tPnlCollapsible3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tPnlCollapsible3.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.tPnlCollapsible3.Dock = System.Windows.Forms.DockStyle.Top;
        this.tPnlCollapsible3.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        this.tPnlCollapsible3.Location = new System.Drawing.Point(0, 0);
        this.tPnlCollapsible3.Margin = new System.Windows.Forms.Padding(0);
        this.tPnlCollapsible3.Name = "tPnlCollapsible3";
        this.tPnlCollapsible3.Size = new System.Drawing.Size(669, 151);
        this.tPnlCollapsible3.TabIndex = 0;
        this.tPnlCollapsible3.TaskListInstance = null;
        this.tPnlCollapsible3.TaskListNode = null;
        this.tPnlCollapsible3.UserControlClass = "";
        this.tPnlCollapsible3.UserControlNamespace = "";
        this.tPnlCollapsible3.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
        //
        // button2
        //
        this.button2.Location = new System.Drawing.Point(313, 352);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(86, 23);
        this.button2.TabIndex = 4;
        this.button2.Text = "Toggle Middle";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.Button2Click);
        //
        // button3
        //
        this.button3.Location = new System.Drawing.Point(313, 323);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(86, 23);
        this.button3.TabIndex = 5;
        this.button3.Text = "Toggle Top";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.Button3Click);
        //
        // textBox1
        //
        this.textBox1.Location = new System.Drawing.Point(405, 383);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(95, 20);
        this.textBox1.TabIndex = 6;
        //
        // textBox2
        //
        this.textBox2.Location = new System.Drawing.Point(405, 354);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(95, 20);
        this.textBox2.TabIndex = 7;
        //
        // textBox3
        //
        this.textBox3.Location = new System.Drawing.Point(405, 325);
        this.textBox3.Name = "textBox3";
        this.textBox3.Size = new System.Drawing.Size(95, 20);
        this.textBox3.TabIndex = 8;
        //
        // textBox4
        //
        this.textBox4.Location = new System.Drawing.Point(506, 383);
        this.textBox4.Name = "textBox4";
        this.textBox4.Size = new System.Drawing.Size(100, 20);
        this.textBox4.TabIndex = 9;
        this.textBox4.TextChanged += new System.EventHandler(this.TextBox4TextChanged);
        //
        // textBox5
        //
        this.textBox5.Location = new System.Drawing.Point(506, 354);
        this.textBox5.Name = "textBox5";
        this.textBox5.Size = new System.Drawing.Size(100, 20);
        this.textBox5.TabIndex = 10;
        this.textBox5.TextChanged += new System.EventHandler(this.TextBox5TextChanged);
        //
        // textBox6
        //
        this.textBox6.Location = new System.Drawing.Point(506, 325);
        this.textBox6.Name = "textBox6";
        this.textBox6.Size = new System.Drawing.Size(100, 20);
        this.textBox6.TabIndex = 11;
        this.textBox6.TextChanged += new System.EventHandler(this.TextBox6TextChanged);
        //
        // MainForm2
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(669, 464);
        this.Controls.Add(this.textBox6);
        this.Controls.Add(this.textBox5);
        this.Controls.Add(this.textBox4);
        this.Controls.Add(this.textBox3);
        this.Controls.Add(this.textBox2);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.tPnlCollapsible1);
        this.Controls.Add(this.tPnlCollapsible2);
        this.Controls.Add(this.tPnlCollapsible3);
        this.Name = "MainForm2";
        this.Text = "TestCollapsible";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox textBox6;
    private System.Windows.Forms.TextBox textBox5;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible1;
    private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible2;
    private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible3;
}
}