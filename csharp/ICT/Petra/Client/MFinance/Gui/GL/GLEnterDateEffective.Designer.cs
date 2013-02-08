//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
namespace Ict.Petra.Client.MFinance.Gui.GL
{
    partial class TDlgGLEnterDateEffective
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
            this.dtpDateEffective = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateEffective = new System.Windows.Forms.Label();
            this.lblValidDateRange = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // dtpDateEffective
            //
            this.dtpDateEffective.Location = new System.Drawing.Point(59, 39);
            this.dtpDateEffective.Name = "dtpDateEffective";
            this.dtpDateEffective.Size = new System.Drawing.Size(94, 22);
            this.dtpDateEffective.TabIndex = 0;

            //
            // lblDateEffective
            //
            this.lblDateEffective.Location = new System.Drawing.Point(19, 13);
            this.lblDateEffective.Name = "lblDateEffective";
            this.lblDateEffective.Size = new System.Drawing.Size(240, 23);
            this.lblDateEffective.TabIndex = 1;
            this.lblDateEffective.Text = "label1";

            //
            // lblValidDateRange
            //
            this.lblValidDateRange.Location = new System.Drawing.Point(19, 82);
            this.lblValidDateRange.Name = "lblValidDateRange";
            this.lblValidDateRange.Size = new System.Drawing.Size(261, 53);
            this.lblValidDateRange.TabIndex = 2;
            this.lblValidDateRange.Text = "valid dates from {0} to {1}";

            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(115, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOKClick);

            //
            // TDlgGLEnterDateEffective
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 184);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblValidDateRange);
            this.Controls.Add(this.lblDateEffective);
            this.Controls.Add(this.dtpDateEffective);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TDlgGLEnterDateEffective";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select the posting date";
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblValidDateRange;
        private System.Windows.Forms.Label lblDateEffective;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateEffective;

    }
}