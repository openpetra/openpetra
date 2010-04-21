/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Windows.Forms;
TODO: move Statusbar things to constructor
namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TCopyPartnerAddressDialogWinForm
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TCopyPartnerAddressDialogWinForm));
            this.cmbAddressLayout = new Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox();
            this.lblAddressLayout = new System.Windows.Forms.Label();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(13, 8);
            this.sbtForm.SetStatusBarText(this.btnOK, "Copy Address to Clipboard and close");
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 168);
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(318, 32);

            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(91, 8);
            this.sbtForm.SetStatusBarText(this.btnCancel, "Close without copying the Address");

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(231, 8);
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 200);
            this.stbMain.Size = new System.Drawing.Size(318, 22);

            //
            // stpInfo
            //
            this.stpInfo.Width = 318;

            //
            // cmbAddressLayout
            //
            this.cmbAddressLayout.ComboBoxWidth = 100;
            this.cmbAddressLayout.Filter = null;
            this.cmbAddressLayout.ListTable = Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox.TListTableEnum.AddressLayoutList;
            this.cmbAddressLayout.Location = new System.Drawing.Point(153, 9);
            this.cmbAddressLayout.Name = "cmbAddressLayout";
            this.cmbAddressLayout.SelectedItem = ((object)(resources.GetObject("cmbAddressLayout.SelectedItem")));
            this.cmbAddressLayout.SelectedValue = null;
            this.cmbAddressLayout.Size = new System.Drawing.Size(121, 22);
            this.sbtForm.SetStatusBarText(this.cmbAddressLayout, "Address Layout to use for copied Address");
            this.cmbAddressLayout.TabIndex = 0;

            //
            // lblAddressLayout
            //
            this.lblAddressLayout.Location = new System.Drawing.Point(13, 13);
            this.lblAddressLayout.Name = "lblAddressLayout";
            this.lblAddressLayout.Size = new System.Drawing.Size(152, 23);
            this.lblAddressLayout.TabIndex = 997;
            this.lblAddressLayout.Text = "Select Address &Layout:";

            //
            // grpPreview
            //
            this.grpPreview.Controls.Add(this.txtPreview);
            this.grpPreview.Location = new System.Drawing.Point(13, 36);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(293, 131);
            this.grpPreview.TabIndex = 998;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview of Copied Address";

            //
            // txtPreview
            //
            this.txtPreview.Font =
                new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreview.Location = new System.Drawing.Point(7, 21);
            this.txtPreview.Multiline = true;
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtPreview.Size = new System.Drawing.Size(280, 104);
            this.sbtForm.SetStatusBarText(this.txtPreview, "Address formatted as it will be copied to the Clipboard");
            this.txtPreview.TabIndex = 0;
            this.txtPreview.TabStop = false;
            this.txtPreview.WordWrap = false;

            //
            // TCopyPartnerAddressDialogWinForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 222);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.cmbAddressLayout);
            this.Controls.Add(this.lblAddressLayout);
            this.Name = "TCopyPartnerAddressDialogWinForm";
            this.Text = "Copy Partner\'s Address";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.lblAddressLayout, 0);
            this.Controls.SetChildIndex(this.cmbAddressLayout, 0);
            this.Controls.SetChildIndex(this.grpPreview, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.grpPreview.PerformLayout();
            this.ResumeLayout(false);
        }

        private Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox cmbAddressLayout;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.Label lblAddressLayout;

        void Form_Load(object Sender, System.EventArgs e)
        {
            cmbAddressLayout.InitialiseUserControl();
            this.cmbAddressLayout.SelectedValueChanged += new Ict.Petra.Client.CommonControls.TSelectedValueChangedEventHandler(
                this.CmbAddressLayoutSelectedValueChanged);

            cmbAddressLayout.SelectedItem = "SmlLabel";
        }

        void CmbAddressLayoutSelectedValueChanged(object Sender, System.EventArgs e)
        {
            if (cmbAddressLayout.SelectedValue != null)
            {
                txtPreview.Text = GetFormattedAddress(cmbAddressLayout.SelectedValue.ToString());
            }
        }

        void BtnOK_Click(object Sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(txtPreview.Text);
            this.Close();
        }
    }
}