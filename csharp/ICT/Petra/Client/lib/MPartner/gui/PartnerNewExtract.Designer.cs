/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       berndr
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TPartnerNewExtract
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
            this.txtExtractName = new System.Windows.Forms.TextBox();
            this.txtExtractDescription = new System.Windows.Forms.TextBox();
            this.lblExtractName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpExtractDetails = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.grpExtractDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
todo: move statusbar things to constructor
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept data and continue");
            this.btnOK.TabIndex = 0;
            this.btnOK.Click += new System.EventHandler(this.BtnOKClick);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 179);
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(402, 32);

            //
            // btnCancel
            //
            this.sbtForm.SetStatusBarText(this.btnCancel, "Cancel data entry and close");
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(317, 8);
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");
            this.btnHelp.TabIndex = 2;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 211);

            //
            // txtExtractName
            //
            this.txtExtractName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtractName.Location = new System.Drawing.Point(112, 16);
            this.txtExtractName.Name = "txtExtractName";
            this.txtExtractName.Size = new System.Drawing.Size(266, 21);
            this.sbtForm.SetStatusBarText(this.txtExtractName, "Enter the name for the new Extract");
            this.txtExtractName.TabIndex = 1;
            this.txtExtractName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtExtractName_KeyUp);

            //
            // txtExtractDescription
            //
            this.txtExtractDescription.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.txtExtractDescription.Location = new System.Drawing.Point(106, 5);
            this.txtExtractDescription.Multiline = true;
            this.txtExtractDescription.Name = "txtExtractDescription";
            this.txtExtractDescription.Size = new System.Drawing.Size(266, 70);
            this.sbtForm.SetStatusBarText(this.txtExtractDescription, "Enter the description for the new Extract");
            this.txtExtractDescription.TabIndex = 1;

            //
            // lblExtractName
            //
            this.lblExtractName.Location = new System.Drawing.Point(6, 19);
            this.lblExtractName.Name = "lblExtractName";
            this.lblExtractName.Size = new System.Drawing.Size(100, 23);
            this.lblExtractName.TabIndex = 0;
            this.lblExtractName.Text = "Extract &Name:";

            //
            // lblDescription
            //
            this.lblDescription.Location = new System.Drawing.Point(3, 8);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 23);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "&Description:";

            //
            // grpExtractDetails
            //
            this.grpExtractDetails.Controls.Add(this.panel1);
            this.grpExtractDetails.Controls.Add(this.txtExtractName);
            this.grpExtractDetails.Controls.Add(this.lblExtractName);
            this.grpExtractDetails.Location = new System.Drawing.Point(8, 12);
            this.grpExtractDetails.Name = "grpExtractDetails";
            this.grpExtractDetails.Size = new System.Drawing.Size(384, 167);
            this.grpExtractDetails.TabIndex = 1;
            this.grpExtractDetails.TabStop = false;

            //
            // panel1
            //
            this.panel1.Controls.Add(this.lblDescription);
            this.panel1.Controls.Add(this.txtExtractDescription);
            this.panel1.Location = new System.Drawing.Point(6, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 81);
            this.panel1.TabIndex = 2;

            //
            // TPartnerNewExtract
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 233);
            this.Controls.Add(this.grpExtractDetails);
            this.Name = "TPartnerNewExtract";
            this.Text = "New Extract";
            this.Load += new System.EventHandler(this.PartnerNewExtract_Loading);
            this.Shown += new System.EventHandler(this.PartnerNewExtract_Shown);
            this.VisibleChanged += new System.EventHandler(this.TPartnerNewExtractVisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PartnerNewExtract_Closing);
            this.Controls.SetChildIndex(this.grpExtractDetails, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.grpExtractDetails.ResumeLayout(false);
            this.grpExtractDetails.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpExtractDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtExtractName;
        private System.Windows.Forms.TextBox txtExtractDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblExtractName;
    }
}