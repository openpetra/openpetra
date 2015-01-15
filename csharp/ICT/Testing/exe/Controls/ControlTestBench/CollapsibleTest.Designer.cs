//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 sbird
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
    partial class CollapsibleTest
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
            this.btnChangeText = new System.Windows.Forms.Button();
            this.btnChangeCollapseDirection = new System.Windows.Forms.Button();
            this.btnChangeHostedControlKind = new System.Windows.Forms.Button();
            this.btnChangeUserControlString = new System.Windows.Forms.Button();
            this.btnChangeTaskListNode = new System.Windows.Forms.Button();
            this.btnChangeVisualStyle = new System.Windows.Forms.Button();
            this.txtChangeText = new System.Windows.Forms.TextBox();
            this.txtChangeUserControlString = new System.Windows.Forms.TextBox();
            this.cboChangeCollapseDirection = new System.Windows.Forms.ComboBox();
            this.cboChangeHostedControlKind = new System.Windows.Forms.ComboBox();
            this.cboChangeVisualStyle = new System.Windows.Forms.ComboBox();
            this.rtbChangeTaskListNode = new System.Windows.Forms.RichTextBox();
            this.txtChangeExpandedSize = new System.Windows.Forms.TextBox();
            this.btnChangeExpandedSize = new System.Windows.Forms.Button();
            this.btnTestEmptyConstructor = new System.Windows.Forms.Button();
            this.btnTestFullConstructor = new System.Windows.Forms.Button();
            this.btnTestUserControlVerticalConstructor = new System.Windows.Forms.Button();
            this.btnTestTaskListVerticalConstructor = new System.Windows.Forms.Button();
            this.btnTestTaskListExpandedConstructor = new System.Windows.Forms.Button();
            this.btnTestTaskListHorizontalConstructor = new System.Windows.Forms.Button();
            this.btnTestStacked = new System.Windows.Forms.Button();
            this.btnTestTaskListHorizontalConstructorRight = new System.Windows.Forms.Button();
            this.btnTaskListHeight = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.btnSetActiveTask = new System.Windows.Forms.Button();
            this.btnGetActiveTask = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // btnChangeText
            //
            this.btnChangeText.Location = new System.Drawing.Point(538, 262);
            this.btnChangeText.Name = "btnChangeText";
            this.btnChangeText.Size = new System.Drawing.Size(169, 23);
            this.btnChangeText.TabIndex = 0;
            this.btnChangeText.Text = "Change Text ( title && tooltip )";
            this.btnChangeText.UseVisualStyleBackColor = true;
            this.btnChangeText.Click += new System.EventHandler(this.ChangeText);
            //
            // btnChangeCollapseDirection
            //
            this.btnChangeCollapseDirection.Location = new System.Drawing.Point(538, 291);
            this.btnChangeCollapseDirection.Name = "btnChangeCollapseDirection";
            this.btnChangeCollapseDirection.Size = new System.Drawing.Size(169, 23);
            this.btnChangeCollapseDirection.TabIndex = 1;
            this.btnChangeCollapseDirection.Text = "Change Direction";
            this.btnChangeCollapseDirection.UseVisualStyleBackColor = true;
            this.btnChangeCollapseDirection.Click += new System.EventHandler(this.ChangeCollapseDirection);
            //
            // btnChangeHostedControlKind
            //
            this.btnChangeHostedControlKind.Location = new System.Drawing.Point(538, 320);
            this.btnChangeHostedControlKind.Name = "btnChangeHostedControlKind";
            this.btnChangeHostedControlKind.Size = new System.Drawing.Size(169, 23);
            this.btnChangeHostedControlKind.TabIndex = 2;
            this.btnChangeHostedControlKind.Text = "Change Hck";
            this.btnChangeHostedControlKind.UseVisualStyleBackColor = true;
            this.btnChangeHostedControlKind.Click += new System.EventHandler(this.ChangeHostedControlKind);
            //
            // btnChangeUserControlString
            //
            this.btnChangeUserControlString.Location = new System.Drawing.Point(538, 349);
            this.btnChangeUserControlString.Name = "btnChangeUserControlString";
            this.btnChangeUserControlString.Size = new System.Drawing.Size(169, 23);
            this.btnChangeUserControlString.TabIndex = 3;
            this.btnChangeUserControlString.Text = "Change UserControl String";
            this.btnChangeUserControlString.UseVisualStyleBackColor = true;
            this.btnChangeUserControlString.Click += new System.EventHandler(this.ChangeUserControlString);
            //
            // btnChangeTaskListNode
            //
            this.btnChangeTaskListNode.Location = new System.Drawing.Point(538, 407);
            this.btnChangeTaskListNode.Name = "btnChangeTaskListNode";
            this.btnChangeTaskListNode.Size = new System.Drawing.Size(169, 23);
            this.btnChangeTaskListNode.TabIndex = 4;
            this.btnChangeTaskListNode.Text = "Change TaskList Node";
            this.btnChangeTaskListNode.UseVisualStyleBackColor = true;
            this.btnChangeTaskListNode.Click += new System.EventHandler(this.ChangeTaskListNode);
            //
            // btnChangeVisualStyle
            //
            this.btnChangeVisualStyle.Location = new System.Drawing.Point(538, 378);
            this.btnChangeVisualStyle.Name = "btnChangeVisualStyle";
            this.btnChangeVisualStyle.Size = new System.Drawing.Size(169, 23);
            this.btnChangeVisualStyle.TabIndex = 5;
            this.btnChangeVisualStyle.Text = "Change Style";
            this.btnChangeVisualStyle.UseVisualStyleBackColor = true;
            this.btnChangeVisualStyle.Click += new System.EventHandler(this.ChangeVisualStyle);
            //
            // txtChangeText
            //
            this.txtChangeText.Location = new System.Drawing.Point(316, 264);
            this.txtChangeText.Name = "txtChangeText";
            this.txtChangeText.Size = new System.Drawing.Size(216, 20);
            this.txtChangeText.TabIndex = 7;
            //
            // txtChangeUserControlString
            //
            this.txtChangeUserControlString.Location = new System.Drawing.Point(316, 351);
            this.txtChangeUserControlString.Name = "txtChangeUserControlString";
            this.txtChangeUserControlString.Size = new System.Drawing.Size(216, 20);
            this.txtChangeUserControlString.TabIndex = 10;
            //
            // cboChangeCollapseDirection
            //
            this.cboChangeCollapseDirection.FormattingEnabled = true;
            this.cboChangeCollapseDirection.Items.AddRange(new object[] {
                    "Horizontal (left)",
                    "Horizontal (right)",
                    "Vertical"
                });
            this.cboChangeCollapseDirection.Location = new System.Drawing.Point(316, 293);
            this.cboChangeCollapseDirection.Name = "cboChangeCollapseDirection";
            this.cboChangeCollapseDirection.Size = new System.Drawing.Size(216, 21);
            this.cboChangeCollapseDirection.TabIndex = 14;
            //
            // cboChangeHostedControlKind
            //
            this.cboChangeHostedControlKind.FormattingEnabled = true;
            this.cboChangeHostedControlKind.Items.AddRange(new object[] {
                    "Task List",
                    "UserControl"
                });
            this.cboChangeHostedControlKind.Location = new System.Drawing.Point(316, 322);
            this.cboChangeHostedControlKind.Name = "cboChangeHostedControlKind";
            this.cboChangeHostedControlKind.Size = new System.Drawing.Size(216, 21);
            this.cboChangeHostedControlKind.TabIndex = 15;
            //
            // cboChangeVisualStyle
            //
            this.cboChangeVisualStyle.FormattingEnabled = true;
            this.cboChangeVisualStyle.Items.AddRange(new object[] {
                    "vsTaskPanel",
                    "vsAccordionPanel",
                    "vsDashboard",
                    "vsShepherd",
                    "vsHorizontalCollapse"
                });
            this.cboChangeVisualStyle.Location = new System.Drawing.Point(316, 380);
            this.cboChangeVisualStyle.Name = "cboChangeVisualStyle";
            this.cboChangeVisualStyle.Size = new System.Drawing.Size(216, 21);
            this.cboChangeVisualStyle.TabIndex = 16;
            //
            // rtbChangeTaskListNode
            //
            this.rtbChangeTaskListNode.Location = new System.Drawing.Point(316, 409);
            this.rtbChangeTaskListNode.Name = "rtbChangeTaskListNode";
            this.rtbChangeTaskListNode.Size = new System.Drawing.Size(216, 105);
            this.rtbChangeTaskListNode.TabIndex = 17;
            this.rtbChangeTaskListNode.Text = "TaskGroup:\n    Task1:\n        Label: Type yml here; then hit button.";
            //
            // txtChangeExpandedSize
            //
            this.txtChangeExpandedSize.Location = new System.Drawing.Point(316, 235);
            this.txtChangeExpandedSize.Name = "txtChangeExpandedSize";
            this.txtChangeExpandedSize.Size = new System.Drawing.Size(216, 20);
            this.txtChangeExpandedSize.TabIndex = 19;
            //
            // btnChangeExpandedSize
            //
            this.btnChangeExpandedSize.Location = new System.Drawing.Point(538, 233);
            this.btnChangeExpandedSize.Name = "btnChangeExpandedSize";
            this.btnChangeExpandedSize.Size = new System.Drawing.Size(169, 23);
            this.btnChangeExpandedSize.TabIndex = 18;
            this.btnChangeExpandedSize.Text = "Change ExpandedSize";
            this.btnChangeExpandedSize.UseVisualStyleBackColor = true;
            this.btnChangeExpandedSize.Click += new System.EventHandler(this.ChangeWidth);
            //
            // btnTestEmptyConstructor
            //
            this.btnTestEmptyConstructor.Location = new System.Drawing.Point(348, 83);
            this.btnTestEmptyConstructor.Name = "btnTestEmptyConstructor";
            this.btnTestEmptyConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestEmptyConstructor.TabIndex = 20;
            this.btnTestEmptyConstructor.Text = "Empty Constructor";
            this.btnTestEmptyConstructor.UseVisualStyleBackColor = true;
            this.btnTestEmptyConstructor.Click += new System.EventHandler(this.TestEmptyConstructor);
            //
            // btnTestFullConstructor
            //
            this.btnTestFullConstructor.Location = new System.Drawing.Point(523, 83);
            this.btnTestFullConstructor.Name = "btnTestFullConstructor";
            this.btnTestFullConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestFullConstructor.TabIndex = 21;
            this.btnTestFullConstructor.Text = "Full Constructor";
            this.btnTestFullConstructor.UseVisualStyleBackColor = true;
            this.btnTestFullConstructor.Click += new System.EventHandler(this.TestFullConstructor);
            //
            // btnTestUserControlVerticalConstructor
            //
            this.btnTestUserControlVerticalConstructor.Location = new System.Drawing.Point(523, 171);
            this.btnTestUserControlVerticalConstructor.Name = "btnTestUserControlVerticalConstructor";
            this.btnTestUserControlVerticalConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestUserControlVerticalConstructor.TabIndex = 23;
            this.btnTestUserControlVerticalConstructor.Text = "UserControl      Vertical";
            this.btnTestUserControlVerticalConstructor.UseVisualStyleBackColor = true;
            this.btnTestUserControlVerticalConstructor.Click += new System.EventHandler(this.TestUserControlVerticalConstructor);
            //
            // btnTestTaskListVerticalConstructor
            //
            this.btnTestTaskListVerticalConstructor.Location = new System.Drawing.Point(523, 112);
            this.btnTestTaskListVerticalConstructor.Name = "btnTestTaskListVerticalConstructor";
            this.btnTestTaskListVerticalConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestTaskListVerticalConstructor.TabIndex = 24;
            this.btnTestTaskListVerticalConstructor.Text = "TaskList    Vertical";
            this.btnTestTaskListVerticalConstructor.UseVisualStyleBackColor = true;
            this.btnTestTaskListVerticalConstructor.Click += new System.EventHandler(this.TestTaskListVerticalConstructor);
            //
            // btnTestTaskListExpandedConstructor
            //
            this.btnTestTaskListExpandedConstructor.Location = new System.Drawing.Point(348, 171);
            this.btnTestTaskListExpandedConstructor.Name = "btnTestTaskListExpandedConstructor";
            this.btnTestTaskListExpandedConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestTaskListExpandedConstructor.TabIndex = 25;
            this.btnTestTaskListExpandedConstructor.Text = "TaskList    Expanded";
            this.btnTestTaskListExpandedConstructor.UseVisualStyleBackColor = true;
            this.btnTestTaskListExpandedConstructor.Click += new System.EventHandler(this.TestTaskListExpandedConstructor);
            //
            // btnTestTaskListHorizontalConstructor
            //
            this.btnTestTaskListHorizontalConstructor.Location = new System.Drawing.Point(348, 112);
            this.btnTestTaskListHorizontalConstructor.Name = "btnTestTaskListHorizontalConstructor";
            this.btnTestTaskListHorizontalConstructor.Size = new System.Drawing.Size(169, 23);
            this.btnTestTaskListHorizontalConstructor.TabIndex = 26;
            this.btnTestTaskListHorizontalConstructor.Text = "TaskList    Horizontal (left)";
            this.btnTestTaskListHorizontalConstructor.UseVisualStyleBackColor = true;
            this.btnTestTaskListHorizontalConstructor.Click += new System.EventHandler(this.TestTaskListHorizontalConstructor);
            //
            // btnTestStacked
            //
            this.btnTestStacked.Location = new System.Drawing.Point(348, 200);
            this.btnTestStacked.Name = "btnTestStacked";
            this.btnTestStacked.Size = new System.Drawing.Size(169, 23);
            this.btnTestStacked.TabIndex = 27;
            this.btnTestStacked.Text = "Stacked panels";
            this.btnTestStacked.UseVisualStyleBackColor = true;
            this.btnTestStacked.Click += new System.EventHandler(this.TestStacked);
            //
            // btnTestTaskListHorizontalConstructorRight
            //
            this.btnTestTaskListHorizontalConstructorRight.Location = new System.Drawing.Point(348, 141);
            this.btnTestTaskListHorizontalConstructorRight.Name = "btnTestTaskListHorizontalConstructorRight";
            this.btnTestTaskListHorizontalConstructorRight.Size = new System.Drawing.Size(169, 23);
            this.btnTestTaskListHorizontalConstructorRight.TabIndex = 26;
            this.btnTestTaskListHorizontalConstructorRight.Text = "TaskList    Horizontal (right)";
            this.btnTestTaskListHorizontalConstructorRight.UseVisualStyleBackColor = true;
            this.btnTestTaskListHorizontalConstructorRight.Click += new System.EventHandler(this.TestTaskListHorizontalRightConstructor);
            //
            // btnTaskListHeight
            //
            this.btnTaskListHeight.Location = new System.Drawing.Point(538, 468);
            this.btnTaskListHeight.Name = "btnTaskListHeight";
            this.btnTaskListHeight.Size = new System.Drawing.Size(169, 23);
            this.btnTaskListHeight.TabIndex = 28;
            this.btnTaskListHeight.Text = "Show TaskList &Height";
            this.btnTaskListHeight.UseVisualStyleBackColor = true;
            this.btnTaskListHeight.Click += new System.EventHandler(this.BtnTaskListHeightClick);
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtTaskName);
            this.groupBox1.Controls.Add(this.btnSetActiveTask);
            this.groupBox1.Location = new System.Drawing.Point(728, 402);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 89);
            this.groupBox1.TabIndex = 30;
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
            // btnGetActiveTask
            //
            this.btnGetActiveTask.Location = new System.Drawing.Point(751, 373);
            this.btnGetActiveTask.Name = "btnGetActiveTask";
            this.btnGetActiveTask.Size = new System.Drawing.Size(147, 23);
            this.btnGetActiveTask.TabIndex = 29;
            this.btnGetActiveTask.Text = "Get Active Task";
            this.btnGetActiveTask.UseVisualStyleBackColor = true;
            this.btnGetActiveTask.Click += new System.EventHandler(this.BtnGetActiveTaskClick);
            //
            // CollapsibleTest
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 525);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetActiveTask);
            this.Controls.Add(this.btnTaskListHeight);
            this.Controls.Add(this.btnTestStacked);
            this.Controls.Add(this.btnTestTaskListHorizontalConstructorRight);
            this.Controls.Add(this.btnTestTaskListHorizontalConstructor);
            this.Controls.Add(this.btnTestTaskListExpandedConstructor);
            this.Controls.Add(this.btnTestTaskListVerticalConstructor);
            this.Controls.Add(this.btnTestUserControlVerticalConstructor);
            this.Controls.Add(this.btnTestFullConstructor);
            this.Controls.Add(this.btnTestEmptyConstructor);
            this.Controls.Add(this.txtChangeExpandedSize);
            this.Controls.Add(this.btnChangeExpandedSize);
            this.Controls.Add(this.rtbChangeTaskListNode);
            this.Controls.Add(this.cboChangeVisualStyle);
            this.Controls.Add(this.cboChangeHostedControlKind);
            this.Controls.Add(this.cboChangeCollapseDirection);
            this.Controls.Add(this.txtChangeUserControlString);
            this.Controls.Add(this.txtChangeText);
            this.Controls.Add(this.btnChangeVisualStyle);
            this.Controls.Add(this.btnChangeTaskListNode);
            this.Controls.Add(this.btnChangeUserControlString);
            this.Controls.Add(this.btnChangeHostedControlKind);
            this.Controls.Add(this.btnChangeCollapseDirection);
            this.Controls.Add(this.btnChangeText);
            this.Name = "CollapsibleTest";
            this.Text = "collapsibleTest";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnGetActiveTask;
        private System.Windows.Forms.Button btnSetActiveTask;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTaskListHeight;
        private System.Windows.Forms.Button btnTestTaskListHorizontalConstructorRight;
        private System.Windows.Forms.Button btnTestStacked;
        private System.Windows.Forms.Button btnTestTaskListExpandedConstructor;
        private System.Windows.Forms.Button btnTestUserControlVerticalConstructor;
        private System.Windows.Forms.Button btnTestTaskListVerticalConstructor;
        private System.Windows.Forms.Button btnTestTaskListHorizontalConstructor;
        private System.Windows.Forms.TextBox txtChangeExpandedSize;
        private System.Windows.Forms.Button btnChangeExpandedSize;
        private System.Windows.Forms.RichTextBox rtbChangeTaskListNode;
        private System.Windows.Forms.Button btnTestFullConstructor;
        private System.Windows.Forms.Button btnTestEmptyConstructor;
        private System.Windows.Forms.TextBox txtChangeUserControlString;
        private System.Windows.Forms.ComboBox cboChangeVisualStyle;
        private System.Windows.Forms.Button btnChangeTaskListNode;
        private System.Windows.Forms.Button btnChangeVisualStyle;
        private System.Windows.Forms.ComboBox cboChangeHostedControlKind;
        private System.Windows.Forms.ComboBox cboChangeCollapseDirection;
        private System.Windows.Forms.Button btnChangeUserControlString;
        private System.Windows.Forms.Button btnChangeHostedControlKind;
        private System.Windows.Forms.Button btnChangeCollapseDirection;
        private System.Windows.Forms.Button btnChangeText;
        private System.Windows.Forms.TextBox txtChangeText;
    }
}