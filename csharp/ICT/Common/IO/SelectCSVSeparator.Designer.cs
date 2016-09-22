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
            this.grdPreview = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbDateFormat = new System.Windows.Forms.ComboBox();
            this.cmbNumberFormat = new System.Windows.Forms.ComboBox();
            this.cmbTextEncoding = new System.Windows.Forms.ComboBox();
            this.lblNumberFormat = new System.Windows.Forms.Label();
            this.lblDateFormat = new System.Windows.Forms.Label();
            this.lblTextEncoding = new System.Windows.Forms.Label();
            this.lblTextEncodingHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            //
            // rbtComma
            //
            this.rbtComma.Location = new System.Drawing.Point(12, 11);
            this.rbtComma.Name = "rbtComma";
            this.rbtComma.Size = new System.Drawing.Size(104, 24);
            this.rbtComma.TabIndex = 0;
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
            this.rbtSemicolon.TabIndex = 1;
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
            this.txtOtherSeparator.TabIndex = 4;
            this.txtOtherSeparator.TextChanged += new System.EventHandler(this.RbtCheckedChanged);
            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(688, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(688, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // grdPreview
            //
            this.grdPreview.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grdPreview.Location = new System.Drawing.Point(0, 176);
            this.grdPreview.Name = "grdPreview";
            this.grdPreview.Size = new System.Drawing.Size(782, 335);
            this.grdPreview.TabIndex = 1;
            this.grdPreview.TabStop = false;
            //
            // pnlTop
            //
            this.pnlTop.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.Controls.Add(this.cmbDateFormat);
            this.pnlTop.Controls.Add(this.cmbNumberFormat);
            this.pnlTop.Controls.Add(this.cmbTextEncoding);
            this.pnlTop.Controls.Add(this.lblNumberFormat);
            this.pnlTop.Controls.Add(this.lblDateFormat);
            this.pnlTop.Controls.Add(this.lblTextEncoding);
            this.pnlTop.Controls.Add(this.lblTextEncodingHint);
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.txtOtherSeparator);
            this.pnlTop.Controls.Add(this.rbtSemicolon);
            this.pnlTop.Controls.Add(this.rbtOther);
            this.pnlTop.Controls.Add(this.rbtTabulator);
            this.pnlTop.Controls.Add(this.rbtComma);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(782, 170);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.TabStop = true;
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
            this.cmbDateFormat.TabIndex = 6;
            //
            // cmbNumberFormat
            //
            this.cmbNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNumberFormat.FormattingEnabled = true;
            this.cmbNumberFormat.Items.AddRange(new object[] {
                    "Decimal Point (12.34)",
                    "Decimal Comma (12,34)"
                });
            this.cmbNumberFormat.Location = new System.Drawing.Point(118, 108);
            this.cmbNumberFormat.Name = "cmbNumberFormat";
            this.cmbNumberFormat.Size = new System.Drawing.Size(160, 21);
            this.cmbNumberFormat.TabIndex = 8;
            //
            // cmbTextEncoding
            //
            this.cmbTextEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTextEncoding.FormattingEnabled = true;
            this.cmbTextEncoding.Location = new System.Drawing.Point(118, 139);
            this.cmbTextEncoding.Name = "cmbTextEncoding";
            this.cmbTextEncoding.Size = new System.Drawing.Size(175, 21);
            this.cmbTextEncoding.TabIndex = 10;
            //
            // lblNumberFormat
            //
            this.lblNumberFormat.Location = new System.Drawing.Point(12, 108);
            this.lblNumberFormat.Name = "lblNumberFormat";
            this.lblNumberFormat.Size = new System.Drawing.Size(100, 23);
            this.lblNumberFormat.TabIndex = 7;
            this.lblNumberFormat.Text = "Number format:";
            //
            // lblDateFormat
            //
            this.lblDateFormat.Location = new System.Drawing.Point(12, 77);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(100, 23);
            this.lblDateFormat.TabIndex = 5;
            this.lblDateFormat.Text = "Date format:";
            //
            // lblTextEncoding
            //
            this.lblTextEncoding.Location = new System.Drawing.Point(12, 139);
            this.lblTextEncoding.Name = "lblTextEncoding";
            this.lblTextEncoding.Size = new System.Drawing.Size(100, 23);
            this.lblTextEncoding.TabIndex = 9;
            this.lblTextEncoding.Text = "Text encoding:";
            //
            // lblTextEncodingHint
            //
            this.lblTextEncodingHint.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblTextEncodingHint.Location = new System.Drawing.Point(300, 139);
            this.lblTextEncodingHint.Name = "lblTextEncodingHint";
            this.lblTextEncodingHint.Size = new System.Drawing.Size(470, 31);
            this.lblTextEncodingHint.TabIndex = 11;
            this.lblTextEncodingHint.Text = "Hint: Where a choice of encodings exists, a file exported from OpenPetra or Excel" +
                                            " is probably Unicode but a file exported from Petra is always one of the other options.";
            //
            // TDlgSelectCSVSeparator
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(782, 511);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.grdPreview);
            this.Name = "TDlgSelectCSVSeparator";
            this.Text = "Select CSV Separator";
            this.Activated += new System.EventHandler(this.Form_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblNumberFormat;

        private System.Windows.Forms.ComboBox cmbDateFormat;

        private System.Windows.Forms.ComboBox cmbNumberFormat;

        private System.Windows.Forms.ComboBox cmbTextEncoding;

        private System.Windows.Forms.Label lblDateFormat;

        private System.Windows.Forms.Label lblTextEncoding;
        private System.Windows.Forms.Label lblTextEncodingHint;

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.DataGridView grdPreview;
        private System.Windows.Forms.TextBox txtOtherSeparator;
        private System.Windows.Forms.RadioButton rbtSemicolon;
        private System.Windows.Forms.RadioButton rbtOther;
        private System.Windows.Forms.RadioButton rbtTabulator;
        private System.Windows.Forms.RadioButton rbtComma;
    }
}