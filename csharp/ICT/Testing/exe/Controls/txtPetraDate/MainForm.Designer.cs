//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Client.CommonControls;

namespace Ict.Testing.TxtPetraDate
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
            this.dtpDetailGlEffectiveDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAssignInvalidDate = new System.Windows.Forms.Button();

            this.SuspendLayout();

            //
            // textBox1
            //
            this.textBox1.Location = new System.Drawing.Point(50, 120);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 200);
            this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.textBox1.Dock = DockStyle.Bottom;
            this.textBox1.TabIndex = 0;

            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(50, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please enter a date:";

            //
            // dtpDetailGlEffectiveDate
            //
            this.dtpDetailGlEffectiveDate.Name = "dtpDetailGlEffectiveDate";
            this.dtpDetailGlEffectiveDate.Location = new System.Drawing.Point(170, 22);
            this.dtpDetailGlEffectiveDate.Size = new System.Drawing.Size(94, 22);
            this.dtpDetailGlEffectiveDate.TabIndex = 2919;
            this.dtpDetailGlEffectiveDate.DateChanged += new TPetraDateChangedEventHandler(this.DateChangedHandler);
            this.dtpDetailGlEffectiveDate.Validated += new System.EventHandler(this.ControlValidatedHandler);

            //
            // btnAssignInvalidDate
            //
            this.btnAssignInvalidDate.Location = new System.Drawing.Point(50, 80);
            this.btnAssignInvalidDate.Name = "btnAssignInvalidDate";
            this.btnAssignInvalidDate.Size = new System.Drawing.Size(152, 23);
            this.btnAssignInvalidDate.TabIndex = 0;
            this.btnAssignInvalidDate.Text = "textAssignInvalidDate";
            this.btnAssignInvalidDate.UseVisualStyleBackColor = true;
            this.btnAssignInvalidDate.Click += new System.EventHandler(this.BtnAssignInvalidDateClick);

            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 427);
            this.Name = "MainForm";
            this.Text = "Test txtPetraDate";
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpDetailGlEffectiveDate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAssignInvalidDate);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDetailGlEffectiveDate;
        private System.Windows.Forms.Button btnAssignInvalidDate;
    }
}