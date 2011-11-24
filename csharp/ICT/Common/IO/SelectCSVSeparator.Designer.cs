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
namespace Ict.Common.IO
{
    partial class TDlgSelectCSVSeparator
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
            this.rbtComma = new System.Windows.Forms.RadioButton();
            this.rbtTabulator = new System.Windows.Forms.RadioButton();
            this.rbtOther = new System.Windows.Forms.RadioButton();
            this.rbtSemicolon = new System.Windows.Forms.RadioButton();
            this.txtOtherSeparator = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grdPreview = new System.Windows.Forms.DataGrid();
            this.lblDateFormat = new System.Windows.Forms.Label();
            this.lblNumberFormat = new System.Windows.Forms.Label();
            this.cmbNumberFormat = new System.Windows.Forms.ComboBox();
            this.cmbDateFormat = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
            this.SuspendLayout();
            //
            // rbtComma
            //
            this.rbtComma.Location = new System.Drawing.Point(12, 11);
            this.rbtComma.Name = "rbtComma";
            this.rbtComma.Size = new System.Drawing.Size(104, 24);
            this.rbtComma.TabIndex = 1;
            this.rbtComma.TabStop = true;
            this.rbtComma.Text = "Comma";
            this.rbtComma.UseVisualStyleBackColor = true;
            this.rbtComma.CheckedChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // rbtTabulator
            //
            this.rbtTabulator.Location = new System.Drawing.Point(238, 11);
            this.rbtTabulator.Name = "rbtTabulator";
            this.rbtTabulator.Size = new System.Drawing.Size(104, 24);
            this.rbtTabulator.TabIndex = 2;
            this.rbtTabulator.TabStop = true;
            this.rbtTabulator.Text = "Tabulator";
            this.rbtTabulator.UseVisualStyleBackColor = true;
            this.rbtTabulator.CheckedChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // rbtOther
            //
            this.rbtOther.Location = new System.Drawing.Point(11, 38);
            this.rbtOther.Name = "rbtOther";
            this.rbtOther.Size = new System.Drawing.Size(104, 24);
            this.rbtOther.TabIndex = 3;
            this.rbtOther.TabStop = true;
            this.rbtOther.Text = "Other Separator:";
            this.rbtOther.UseVisualStyleBackColor = true;
            this.rbtOther.CheckedChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // rbtSemicolon
            //
            this.rbtSemicolon.Location = new System.Drawing.Point(113, 11);
            this.rbtSemicolon.Name = "rbtSemicolon";
            this.rbtSemicolon.Size = new System.Drawing.Size(104, 24);
            this.rbtSemicolon.TabIndex = 4;
            this.rbtSemicolon.TabStop = true;
            this.rbtSemicolon.Text = "Semicolon";
            this.rbtSemicolon.UseVisualStyleBackColor = true;
            this.rbtSemicolon.CheckedChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // txtOtherSeparator
            //
            this.txtOtherSeparator.Location = new System.Drawing.Point(117, 41);
            this.txtOtherSeparator.Name = "txtOtherSeparator";
            this.txtOtherSeparator.Size = new System.Drawing.Size(48, 20);
            this.txtOtherSeparator.TabIndex = 5;
            this.txtOtherSeparator.TextChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(446, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOK
            //
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(446, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // grdPreview
            //
            this.grdPreview.CaptionVisible = false;
            this.grdPreview.DataMember = "";
            this.grdPreview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdPreview.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPreview.Location = new System.Drawing.Point(0, 134);
            this.grdPreview.Name = "grdPreview";
            this.grdPreview.Size = new System.Drawing.Size(533, 154);
            this.grdPreview.TabIndex = 9;
            //
            // lblDateFormat
            //
            this.lblDateFormat.Location = new System.Drawing.Point(11, 77);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(100, 23);
            this.lblDateFormat.TabIndex = 10;
            this.lblDateFormat.Text = "Date format:";
            //
            // lblNumberFormat
            //
            this.lblNumberFormat.Location = new System.Drawing.Point(12, 108);
            this.lblNumberFormat.Name = "lblNumberFormat";
            this.lblNumberFormat.Size = new System.Drawing.Size(100, 23);
            this.lblNumberFormat.TabIndex = 12;
            this.lblNumberFormat.Text = "Number format:";
            //
            // cmbNumberFormat
            //
            this.cmbNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNumberFormat.FormattingEnabled = true;
            this.cmbNumberFormat.Items.AddRange(new object[] {
                    "Decimal Point (12.34)",
                    "Decimal Comma (12,34)"
                });
            this.cmbNumberFormat.Location = new System.Drawing.Point(117, 108);
            this.cmbNumberFormat.Name = "cmbNumberFormat";
            this.cmbNumberFormat.Size = new System.Drawing.Size(160, 21);
            this.cmbNumberFormat.TabIndex = 13;
            //
            // cmbDateFormat
            //
            this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFormat.FormattingEnabled = true;
            this.cmbDateFormat.Items.AddRange(new object[] {
                    "MM/dd/yyyy",
                    "dd/MM/yyyy",
                    "yyyy-MM-dd"
                });
            this.cmbDateFormat.Location = new System.Drawing.Point(118, 77);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Size = new System.Drawing.Size(159, 21);
            this.cmbDateFormat.TabIndex = 14;
            //
            // TDlgSelectCSVSeparator
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(533, 288);
            this.Controls.Add(this.cmbDateFormat);
            this.Controls.Add(this.cmbNumberFormat);
            this.Controls.Add(this.lblNumberFormat);
            this.Controls.Add(this.lblDateFormat);
            this.Controls.Add(this.grdPreview);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtOtherSeparator);
            this.Controls.Add(this.rbtSemicolon);
            this.Controls.Add(this.rbtOther);
            this.Controls.Add(this.rbtTabulator);
            this.Controls.Add(this.rbtComma);
            this.Name = "TDlgSelectCSVSeparator";
            this.Text = "Select CSV Separator";
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblNumberFormat;

        private System.Windows.Forms.ComboBox cmbDateFormat;

        private System.Windows.Forms.ComboBox cmbNumberFormat;

        private System.Windows.Forms.Label lblDateFormat;

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGrid grdPreview;
        private System.Windows.Forms.TextBox txtOtherSeparator;
        private System.Windows.Forms.RadioButton rbtSemicolon;
        private System.Windows.Forms.RadioButton rbtOther;
        private System.Windows.Forms.RadioButton rbtTabulator;
        private System.Windows.Forms.RadioButton rbtComma;
    }
}