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
namespace Ict.Common.Controls
{
    partial class TUcoSingleTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUcoSingleTask));
            this.pnlBackground = new Owf.Controls.A1Panel();
            this.lblTaskDescription = new System.Windows.Forms.Label();
            this.llbTaskTitle = new System.Windows.Forms.LinkLabel();
            this.pnlIconSpacer = new System.Windows.Forms.Panel();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlBackground
            //
            this.pnlBackground.BackColor = System.Drawing.Color.Transparent;
            this.pnlBackground.BorderColor = System.Drawing.Color.Transparent;
            this.pnlBackground.Controls.Add(this.lblTaskDescription);
            this.pnlBackground.Controls.Add(this.llbTaskTitle);
            this.pnlBackground.Controls.Add(this.pnlIconSpacer);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font =
                new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBackground.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlBackground.GradientEndColor = System.Drawing.Color.Transparent;
            this.pnlBackground.GradientStartColor = System.Drawing.Color.Transparent;
            this.pnlBackground.Image = ((System.Drawing.Image)(resources.GetObject("pnlBackground.Image")));
            this.pnlBackground.ImageLocation = new System.Drawing.Point(10, 10);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBackground.ShadowOffSet = 0;
            this.pnlBackground.Size = new System.Drawing.Size(250, 48);
            this.pnlBackground.TabIndex = 0;
            this.pnlBackground.Click += new System.EventHandler(this.TaskClick);
            this.pnlBackground.DoubleClick += new System.EventHandler(this.DoubleClickAnywhere);
            //
            // lblTaskDescription
            //
            this.lblTaskDescription.AutoEllipsis = true;
            this.lblTaskDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblTaskDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaskDescription.Font =
                new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaskDescription.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTaskDescription.Location = new System.Drawing.Point(46, 20);
            this.lblTaskDescription.Name = "lblTaskDescription";
            this.lblTaskDescription.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.lblTaskDescription.Size = new System.Drawing.Size(202, 26);
            this.lblTaskDescription.TabIndex = 3;
            this.lblTaskDescription.Text = "Task Description";
            this.lblTaskDescription.Click += new System.EventHandler(this.TaskClick);
            this.lblTaskDescription.DoubleClick += new System.EventHandler(this.DoubleClickAnywhere);
            this.lblTaskDescription.MouseEnter += new System.EventHandler(this.TaskMouseEnter);
            this.lblTaskDescription.MouseLeave += new System.EventHandler(this.TaskMouseLeave);
            //
            // llbTaskTitle
            //
            this.llbTaskTitle.ActiveLinkColor = System.Drawing.Color.Orange;
            this.llbTaskTitle.AutoEllipsis = true;
            this.llbTaskTitle.BackColor = System.Drawing.Color.Transparent;
            this.llbTaskTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.llbTaskTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llbTaskTitle.LinkColor = System.Drawing.Color.Black;
            this.llbTaskTitle.Location = new System.Drawing.Point(46, 2);
            this.llbTaskTitle.Name = "llbTaskTitle";
            this.llbTaskTitle.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.llbTaskTitle.Size = new System.Drawing.Size(202, 18);
            this.llbTaskTitle.TabIndex = 2;
            this.llbTaskTitle.TabStop = true;
            this.llbTaskTitle.Text = "Task Title";
            this.llbTaskTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlbTaskTitleLinkClicked);
            this.llbTaskTitle.Click += new System.EventHandler(this.TaskClick);
            this.llbTaskTitle.DoubleClick += new System.EventHandler(this.DoubleClickAnywhere);
            this.llbTaskTitle.MouseEnter += new System.EventHandler(this.TaskTitleMouseEnter);
            this.llbTaskTitle.MouseLeave += new System.EventHandler(this.TaskTitleMouseLeave);
            //
            // pnlIconSpacer
            //
            this.pnlIconSpacer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIconSpacer.Location = new System.Drawing.Point(2, 2);
            this.pnlIconSpacer.Name = "pnlIconSpacer";
            this.pnlIconSpacer.Size = new System.Drawing.Size(44, 44);
            this.pnlIconSpacer.TabIndex = 4;
            this.pnlIconSpacer.Click += new System.EventHandler(this.TaskClick);
            this.pnlIconSpacer.DoubleClick += new System.EventHandler(this.DoubleClickAnywhere);
            //
            // ucoSingleTask
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBackground);
            this.Name = "ucoSingleTask";
            this.Size = new System.Drawing.Size(250, 48);
            this.Click += new System.EventHandler(this.TaskClick);
            this.pnlBackground.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlIconSpacer;
        private Owf.Controls.A1Panel pnlBackground;
        private System.Windows.Forms.Label lblTaskDescription;
        private System.Windows.Forms.LinkLabel llbTaskTitle;
    }
}