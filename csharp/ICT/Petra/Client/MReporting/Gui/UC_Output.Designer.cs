//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Common.Controls;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class UC_Output
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
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
            this.grpCSVOutput = new System.Windows.Forms.GroupBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtCSVSeparator = new System.Windows.Forms.TextBox();
            this.lblCSVSeparator = new System.Windows.Forms.Label();
            this.chbExportToCSVOnly = new System.Windows.Forms.CheckBox();
            this.BtnCSVDestination = new System.Windows.Forms.Button();
            this.lblCSVDestination = new System.Windows.Forms.Label();
            this.txtCSVDestination = new System.Windows.Forms.TextBox();
            this.SaveFileDialogCSV = new System.Windows.Forms.SaveFileDialog();

            //
            // grpCSVOutput
            //
            this.grpCSVOutput.Controls.Add(this.Label2);
            this.grpCSVOutput.Controls.Add(this.Label1);
            this.grpCSVOutput.Controls.Add(this.txtCSVSeparator);
            this.grpCSVOutput.Controls.Add(this.lblCSVSeparator);
            this.grpCSVOutput.Controls.Add(this.chbExportToCSVOnly);
            this.grpCSVOutput.Controls.Add(this.BtnCSVDestination);
            this.grpCSVOutput.Controls.Add(this.lblCSVDestination);
            this.grpCSVOutput.Controls.Add(this.txtCSVDestination);
            this.grpCSVOutput.Location = new System.Drawing.Point(12, 8);
            this.grpCSVOutput.Name = "grpCSVOutput";
            this.grpCSVOutput.Size = new System.Drawing.Size(691, 184);
            this.grpCSVOutput.TabIndex = 23;
            this.grpCSVOutput.TabStop = false;
            this.grpCSVOutput.Text = "Export to CSV";

            //
            // Label2
            //
            this.Label2.Location = new System.Drawing.Point(211, 104);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(403, 32);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "Hint: If you don\'t want quotes around your values, please choose a delimiter that" +
                               " does not occur in your values!";

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(211, 80);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(231, 23);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "(e.g. , or ; or : or Space or Tab)";

            //
            // txtCSVSeparator
            //
            this.txtCSVSeparator.Location = new System.Drawing.Point(115, 80);
            this.txtCSVSeparator.Name = "txtCSVSeparator";
            this.txtCSVSeparator.Size = new System.Drawing.Size(67, 21);
            this.txtCSVSeparator.TabIndex = 5;

            //
            // lblCSVSeparator
            //
            this.lblCSVSeparator.Location = new System.Drawing.Point(19, 80);
            this.lblCSVSeparator.Name = "lblCSVSeparator";
            this.lblCSVSeparator.Size = new System.Drawing.Size(87, 16);
            this.lblCSVSeparator.TabIndex = 4;
            this.lblCSVSeparator.Text = "Delimiter:";

            //
            // chbExportToCSVOnly
            //
            this.chbExportToCSVOnly.Checked = true;
            this.chbExportToCSVOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbExportToCSVOnly.Location = new System.Drawing.Point(19, 144);
            this.chbExportToCSVOnly.Name = "chbExportToCSVOnly";
            this.chbExportToCSVOnly.Size = new System.Drawing.Size(323, 24);
            this.chbExportToCSVOnly.TabIndex = 3;
            this.chbExportToCSVOnly.Text = "Only save as CSV, don\'t print Report";

            //
            // BtnCSVDestination
            //
            this.BtnCSVDestination.Location = new System.Drawing.Point(391, 48);
            this.BtnCSVDestination.Name = "BtnCSVDestination";
            this.BtnCSVDestination.Size = new System.Drawing.Size(47, 23);
            this.BtnCSVDestination.TabIndex = 2;
            this.BtnCSVDestination.Text = "...";
            this.BtnCSVDestination.Click += new System.EventHandler(this.BtnCSVDestination_Click);

            //
            // lblCSVDestination
            //
            this.lblCSVDestination.Location = new System.Drawing.Point(23, 24);
            this.lblCSVDestination.Name = "lblCSVDestination";
            this.lblCSVDestination.Size = new System.Drawing.Size(144, 23);
            this.lblCSVDestination.TabIndex = 1;
            this.lblCSVDestination.Text = "Destination file";

            //
            // txtCSVDestination
            //
            this.txtCSVDestination.Location = new System.Drawing.Point(23, 48);
            this.txtCSVDestination.Name = "txtCSVDestination";
            this.txtCSVDestination.Size = new System.Drawing.Size(368, 21);
            this.txtCSVDestination.TabIndex = 0;

            //
            // SaveFileDialogCSV
            //
            this.SaveFileDialogCSV.DefaultExt = "*.csv";

            //
            // UC_Output
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UC_Output";
        }

        private System.Windows.Forms.GroupBox grpCSVOutput;
        private System.Windows.Forms.CheckBox chbExportToCSVOnly;
        private System.Windows.Forms.Button BtnCSVDestination;
        private System.Windows.Forms.Label lblCSVDestination;
        private System.Windows.Forms.TextBox txtCSVDestination;
        private System.Windows.Forms.SaveFileDialog SaveFileDialogCSV;
        private System.Windows.Forms.Label lblCSVSeparator;
        private System.Windows.Forms.TextBox txtCSVSeparator;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
    }
}