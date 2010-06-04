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
namespace Ict.Petra.Client.CommonDialogs
{
    partial class TFrmCreateExtract
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
            this.btnCreateExtract = new System.Windows.Forms.Button();
            this.txtExtractName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblExtractName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // btnCreateExtract
            //
            this.btnCreateExtract.Location = new System.Drawing.Point(159, 231);
            this.btnCreateExtract.Name = "btnCreateExtract";
            this.btnCreateExtract.Size = new System.Drawing.Size(121, 23);
            this.btnCreateExtract.TabIndex = 0;
            this.btnCreateExtract.Text = "Create Extract";
            this.btnCreateExtract.UseVisualStyleBackColor = true;
            this.btnCreateExtract.Click += new System.EventHandler(this.BtnCreateExtractClick);
            //
            // txtExtractName
            //
            this.txtExtractName.Location = new System.Drawing.Point(130, 21);
            this.txtExtractName.Name = "txtExtractName";
            this.txtExtractName.Size = new System.Drawing.Size(137, 20);
            this.txtExtractName.TabIndex = 1;
            //
            // txtDescription
            //
            this.txtDescription.Location = new System.Drawing.Point(24, 75);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(243, 134);
            this.txtDescription.TabIndex = 2;
            //
            // lblExtractName
            //
            this.lblExtractName.Location = new System.Drawing.Point(24, 26);
            this.lblExtractName.Name = "lblExtractName";
            this.lblExtractName.Size = new System.Drawing.Size(100, 23);
            this.lblExtractName.TabIndex = 3;
            this.lblExtractName.Text = "Extract Name:";
            //
            // lblDescription
            //
            this.lblDescription.Location = new System.Drawing.Point(24, 57);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 15);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(68, 231);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // TFrmCreateExtract
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblExtractName);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtExtractName);
            this.Controls.Add(this.btnCreateExtract);
            this.Name = "TFrmCreateExtract";
            this.Text = "Create Extract";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtExtractName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblExtractName;
        private System.Windows.Forms.Button btnCreateExtract;
    }
}