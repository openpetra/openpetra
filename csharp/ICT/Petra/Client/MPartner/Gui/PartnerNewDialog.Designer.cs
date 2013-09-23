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
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            this.SuspendLayout();
            //
            // lblSitesAvailable
            //
            this.lblSitesAvailable.Location = new System.Drawing.Point(10, 15);
            this.lblSitesAvailable.Name = "lblSitesAvailable";
            this.lblSitesAvailable.Size = new System.Drawing.Size(83, 15);
            this.lblSitesAvailable.TabIndex = 0;
            this.lblSitesAvailable.Text = "S&ites Available:";
            this.lblSitesAvailable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(15, 184);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(78, 21);
            this.lblPartnerKey.TabIndex = 2;
            this.lblPartnerKey.Text = "Partner &Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(18, 206);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(75, 21);
            this.lblPartnerClass.TabIndex = 4;
            this.lblPartnerClass.Text = "Partner C&lass:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(-7, 227);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(100, 18);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "&Acquisition Code:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.BackColor = System.Drawing.SystemColors.Control;
            this.cmbAcquisitionCode.CaseSensitiveSearch = false;
            this.cmbAcquisitionCode.ColumnWidthCol1 = 100;
            this.cmbAcquisitionCode.ColumnWidthCol2 = 350;
            this.cmbAcquisitionCode.ColumnWidthCol3 = 0;
            this.cmbAcquisitionCode.ColumnWidthCol4 = 0;
            this.cmbAcquisitionCode.ComboBoxWidth = 83;
            this.cmbAcquisitionCode.DisplayMember = "";
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAcquisitionCode.ImageColumn = 0;
            this.cmbAcquisitionCode.Images = null;
            this.cmbAcquisitionCode.LabelDisplaysColumn = null;
            this.cmbAcquisitionCode.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(98, 225);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedIndex = -1;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(320, 22);
            this.cmbAcquisitionCode.SuppressSelectionColor = true;
            this.cmbAcquisitionCode.TabIndex = 8;
            this.cmbAcquisitionCode.ValueMember = "";
            //
            // grdInstalledSites
            //
            this.grdInstalledSites.AutoFindColumn = ((short)(-1));
            this.grdInstalledSites.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdInstalledSites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdInstalledSites.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete " +
                                                           "it?";
            this.grdInstalledSites.EnableSort = false;
            this.grdInstalledSites.FixedRows = 1;
            this.grdInstalledSites.Location = new System.Drawing.Point(98, 13);
            this.grdInstalledSites.Name = "grdInstalledSites";
            this.grdInstalledSites.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grdInstalledSites.Size = new System.Drawing.Size(364, 163);
            this.grdInstalledSites.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdInstalledSites.TabIndex = 1;
            this.grdInstalledSites.TabStop = true;
            this.grdInstalledSites.ToolTipText = "";
            this.grdInstalledSites.ToolTipTextDelegate = null;
            //
            // cmbPartnerClass
            //
            this.cmbPartnerClass.BackColor = System.Drawing.SystemColors.Control;
            this.cmbPartnerClass.CaseSensitiveSearch = false;
            this.cmbPartnerClass.ColumnWidthCol1 = 130;
            this.cmbPartnerClass.ColumnWidthCol2 = 0;
            this.cmbPartnerClass.ColumnWidthCol3 = 0;
            this.cmbPartnerClass.ColumnWidthCol4 = 0;
            this.cmbPartnerClass.ComboBoxWidth = 108;
            this.cmbPartnerClass.DisplayMember = "";
            this.cmbPartnerClass.Filter = null;
            this.cmbPartnerClass.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPartnerClass.ImageColumn = 0;
            this.cmbPartnerClass.Images = null;
            this.cmbPartnerClass.LabelDisplaysColumn = null;
            this.cmbPartnerClass.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.PartnerClassList;
            this.cmbPartnerClass.Location = new System.Drawing.Point(98, 203);
            this.cmbPartnerClass.Name = "cmbPartnerClass";
            this.cmbPartnerClass.SelectedIndex = -1;
            this.cmbPartnerClass.Size = new System.Drawing.Size(110, 22);
            this.cmbPartnerClass.SuppressSelectionColor = true;
            this.cmbPartnerClass.TabIndex = 5;
            this.cmbPartnerClass.ValueMember = "";
            this.cmbPartnerClass.SelectedValueChanged += new System.EventHandler(this.CmbPartnerClass_SelectedValueChanged);
            //
            // chkPrivatePartner
            //
            this.chkPrivatePartner.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrivatePartner.Location = new System.Drawing.Point(8, 243);
            this.chkPrivatePartner.Name = "chkPrivatePartner";
            this.chkPrivatePartner.Size = new System.Drawing.Size(102, 23);
            this.chkPrivatePartner.TabIndex = 9;
            this.chkPrivatePartner.Text = "&Private Partner:";
            this.chkPrivatePartner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPrivatePartner.Visible = false;
            //
            // txtPartnerKey
            //
            this.txtPartnerKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartnerKey.DelegateFallbackLabel = true;
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 9.25F, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.LabelText = "Partner Key";
            this.txtPartnerKey.Location = new System.Drawing.Point(98, 182);
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PartnerKey = ((long)(0));
            this.txtPartnerKey.ReadOnly = false;
            this.txtPartnerKey.ShowLabel = false;
            this.txtPartnerKey.Size = new System.Drawing.Size(90, 22);
            this.txtPartnerKey.TabIndex = 3;
            this.txtPartnerKey.TextBoxReadOnly = false;
            this.txtPartnerKey.TextBoxWidth = 80;
            //
            // txtFamilyPartnerBox
            //
            this.txtFamilyPartnerBox.ASpecialSetting = true;
            this.txtFamilyPartnerBox.AutomaticallyUpdateDataSource = false;
            this.txtFamilyPartnerBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtFamilyPartnerBox.ButtonText = "&Family...";
            this.txtFamilyPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFamilyPartnerBox.ButtonWidth = 70;
            this.txtFamilyPartnerBox.ListTable = Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtFamilyPartnerBox.Location = new System.Drawing.Point(213, 202);
            this.txtFamilyPartnerBox.MaxLength = 32767;
            this.txtFamilyPartnerBox.Name = "txtFamilyPartnerBox";
            this.txtFamilyPartnerBox.PartnerClass = "FAMILY";
            this.txtFamilyPartnerBox.PreventFaultyLeaving = false;
            this.txtFamilyPartnerBox.ReadOnly = true;
            this.txtFamilyPartnerBox.ShowLabel = true;
            this.txtFamilyPartnerBox.Size = new System.Drawing.Size(245, 23);
            this.txtFamilyPartnerBox.TabIndex = 6;
            this.txtFamilyPartnerBox.TextBoxWidth = 80;
            this.txtFamilyPartnerBox.VerificationResultCollection = null;
            this.txtFamilyPartnerBox.Visible = false;
            this.txtFamilyPartnerBox.PartnerFound += new Ict.Petra.Client.CommonControls.TDelegatePartnerFound(this.TxtFamilyPartnerBox_PartnerFound);
            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(423, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 19);
            this.btnOK.TabIndex = 997;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(491, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 19);
            this.btnCancel.TabIndex = 998;
            this.btnCancel.Text = "&Cancel";
            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(7, 9);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(62, 19);
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
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 292);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(560, 32);
            this.pnlBtnOKCancelHelpLayout.TabIndex = 996;
            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 324);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(560, 22);
            this.stbMain.TabIndex = 997;
            //
            // TPartnerNewDialogWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 346);
            this.Controls.Add(this.txtFamilyPartnerBox);
            this.Controls.Add(this.txtPartnerKey);
            this.Controls.Add(this.grdInstalledSites);
            this.Controls.Add(this.cmbAcquisitionCode);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblPartnerClass);
            this.Controls.Add(this.lblPartnerKey);
            this.Controls.Add(this.lblSitesAvailable);
            this.Controls.Add(this.pnlBtnOKCancelHelpLayout);
            this.Controls.Add(this.cmbPartnerClass);
            this.Controls.Add(this.chkPrivatePartner);
            this.Controls.Add(this.stbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerNewDialogWinForm";
            this.Text = "New Partner";
            this.Load += new System.EventHandler(this.TPartnerNewDialogWinForm_Load);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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