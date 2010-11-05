//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TPartnerNewDialogWinForm
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
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerNewDialogWinForm));
            this.lblSitesAvailable = new System.Windows.Forms.Label();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.grdInstalledSites = new Ict.Common.Controls.TSgrdDataGrid();
            this.cmbPartnerClass = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.chkPrivatePartner = new System.Windows.Forms.CheckBox();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.txtFamilyPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.pnlBtnOKCancelHelpLayout = new System.Windows.Forms.Panel();
            this.stbMain = new TExtStatusBarHelp();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            this.stbMain.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(396, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 20);
            this.btnOK.TabIndex = 997;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(478, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 20);
            this.btnCancel.TabIndex = 998;
            this.btnCancel.Text = "&Cancel";


            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(8, 10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 20);
            this.btnHelp.TabIndex = 996;
            this.btnHelp.Text = "&Help";
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnCancel);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnOK);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnHelp);
            this.pnlBtnOKCancelHelpLayout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 290);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(560, 34);
            this.pnlBtnOKCancelHelpLayout.TabIndex = 996;

            //
            // lblSitesAvailable
            //
            this.lblSitesAvailable.Location = new System.Drawing.Point(12, 16);
            this.lblSitesAvailable.Name = "lblSitesAvailable";
            this.lblSitesAvailable.Size = new System.Drawing.Size(100, 16);
            this.lblSitesAvailable.TabIndex = 0;
            this.lblSitesAvailable.Text = "S&ites Available:";
            this.lblSitesAvailable.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(18, 198);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(94, 23);
            this.lblPartnerKey.TabIndex = 2;
            this.lblPartnerKey.Text = "Partner &Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(22, 222);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(90, 23);
            this.lblPartnerClass.TabIndex = 4;
            this.lblPartnerClass.Text = "Partner C&lass:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(-8, 244);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(120, 20);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "&Acquisition Code:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(118, 242);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(384, 22);
            this.cmbAcquisitionCode.TabIndex = 8;

            //
            // grdInstalledSites
            //
            this.grdInstalledSites.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(230, 230, 230);
            this.grdInstalledSites.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdInstalledSites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdInstalledSites.DeleteQuestionMessage = "You have chosen to delete" +
                                                           " this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdInstalledSites.FixedRows = 1;
            this.grdInstalledSites.Location = new System.Drawing.Point(118, 14);
            this.grdInstalledSites.MinimumHeight = 19;
            this.grdInstalledSites.Name = "grdInstalledSites";
            this.grdInstalledSites.Size = new System.Drawing.Size(436, 176);
            this.grdInstalledSites.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.grdInstalledSites.TabIndex = 1;
            this.grdInstalledSites.TabStop = true;

            //
            // cmbPartnerClass
            //
            this.cmbPartnerClass.ComboBoxWidth = 130;
            this.cmbPartnerClass.ListTable = TCmbAutoPopulated.TListTableEnum.PartnerClassList;
            this.cmbPartnerClass.Location = new System.Drawing.Point(118, 219);
            this.cmbPartnerClass.Name = "cmbPartnerClass";
            this.cmbPartnerClass.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbPartnerClass.SelectedItem")));
            this.cmbPartnerClass.SelectedValue = null;
            this.cmbPartnerClass.Size = new System.Drawing.Size(132, 22);
            this.cmbPartnerClass.TabIndex = 5;
            this.cmbPartnerClass.SelectedValueChanged += new System.EventHandler(this.CmbPartnerClass_SelectedValueChanged);

            //
            // chkPrivatePartner
            //
            this.chkPrivatePartner.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrivatePartner.Location = new System.Drawing.Point(10, 262);
            this.chkPrivatePartner.Name = "chkPrivatePartner";
            this.chkPrivatePartner.Size = new System.Drawing.Size(122, 24);
            this.chkPrivatePartner.TabIndex = 9;
            this.chkPrivatePartner.Text = "&Private Partner:";
            this.chkPrivatePartner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPrivatePartner.Visible = false;

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartnerKey.DelegateFallbackLabel = true;
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 9.25f, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.LabelText = "Partner Key";
            this.txtPartnerKey.Location = new System.Drawing.Point(118, 196);
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PartnerKey = (Int64)0;
            this.txtPartnerKey.ReadOnly = false;
            this.txtPartnerKey.Size = new System.Drawing.Size(92, 22);
            this.txtPartnerKey.TabIndex = 3;
            this.txtPartnerKey.TextBoxReadOnly = false;
            this.txtPartnerKey.TextBoxWidth = 88;

            //
            // txtFamilyPartnerBox
            //
            this.txtFamilyPartnerBox.ASpecialSetting = true;
            this.txtFamilyPartnerBox.ButtonText = "&Family...";
            this.txtFamilyPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFamilyPartnerBox.ButtonWidth = 70;
            this.txtFamilyPartnerBox.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtFamilyPartnerBox.Location = new System.Drawing.Point(256, 218);
            this.txtFamilyPartnerBox.MaxLength = 32767;
            this.txtFamilyPartnerBox.Name = "txtFamilyPartnerBox";
            this.txtFamilyPartnerBox.PartnerClass = "FAMILY";
            this.txtFamilyPartnerBox.ReadOnly = true;
            this.txtFamilyPartnerBox.Size = new System.Drawing.Size(294, 23);
            this.txtFamilyPartnerBox.TabIndex = 6;
            this.txtFamilyPartnerBox.TextBoxWidth = 80;
            this.txtFamilyPartnerBox.Visible = false;
            this.txtFamilyPartnerBox.PartnerFound += new TDelegatePartnerFound(this.TxtFamilyPartnerBox_PartnerFound);

            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;

            //
            // TPartnerNewDialogWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(560, 346);
            this.Controls.Add(this.txtFamilyPartnerBox);
            this.Controls.Add(this.txtPartnerKey);
            this.Controls.Add(this.grdInstalledSites);
            this.Controls.Add(this.cmbAcquisitionCode);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblPartnerClass);
            this.Controls.Add(this.lblPartnerKey);
            this.Controls.Add(this.lblSitesAvailable);
            this.Controls.Add(this.cmbPartnerClass);
            this.Controls.Add(this.chkPrivatePartner);
            this.Controls.Add(this.pnlBtnOKCancelHelpLayout);
            this.Controls.Add(this.stbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerNewDialogWinForm";
            this.Text = "New Partner";
            this.Load += new System.EventHandler(this.TPartnerNewDialogWinForm_Load);
            this.Controls.SetChildIndex(this.chkPrivatePartner, 0);
            this.Controls.SetChildIndex(this.cmbPartnerClass, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.lblSitesAvailable, 0);
            this.Controls.SetChildIndex(this.lblPartnerKey, 0);
            this.Controls.SetChildIndex(this.lblPartnerClass, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.cmbAcquisitionCode, 0);
            this.Controls.SetChildIndex(this.grdInstalledSites, 0);
            this.Controls.SetChildIndex(this.txtPartnerKey, 0);
            this.Controls.SetChildIndex(this.txtFamilyPartnerBox, 0);
            this.stbMain.ResumeLayout(false);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblSitesAvailable;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.Label Label1;
        private TTxtPartnerKeyTextBox txtPartnerKey;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private TSgrdDataGrid grdInstalledSites;
        private TCmbAutoPopulated cmbPartnerClass;
        private System.Windows.Forms.CheckBox chkPrivatePartner;
        private TtxtAutoPopulatedButtonLabel txtFamilyPartnerBox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBtnOKCancelHelpLayout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
        private TExtStatusBarHelp stbMain;
    }
}