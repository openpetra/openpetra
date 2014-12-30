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
namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for displaying Partner Info data.
    partial class TUC_PartnerInfo
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
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.lblStatusCode = new System.Windows.Forms.Label();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.tabPartnerDetailInfo = new Ict.Common.Controls.TTabVersatile();
            this.tbpAddress = new System.Windows.Forms.TabPage();
            this.pnlContactDetails = new System.Windows.Forms.Panel();
            this.pnlAddressDetails = new System.Windows.Forms.Panel();
            this.tlpPartnerLocation = new System.Windows.Forms.TableLayoutPanel();
            this.lblLocationType = new System.Windows.Forms.Label();
            this.txtLocationType = new System.Windows.Forms.TextBox();
            this.lblMailingAddress = new System.Windows.Forms.Label();
            this.txtMailingAddress = new System.Windows.Forms.TextBox();
            this.lblValidFrom = new System.Windows.Forms.Label();
            this.txtValidFrom = new System.Windows.Forms.TextBox();
            this.lblValidTo = new System.Windows.Forms.Label();
            this.txtValidTo = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.lblLoadingPartnerLocation = new System.Windows.Forms.Label();
            this.lblLoadingAddress = new System.Windows.Forms.Label();
            this.tbpTypesSubscr = new System.Windows.Forms.TabPage();
            this.tlpTypesSubscriptions = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSpecialTypes = new System.Windows.Forms.Panel();
            this.pnlAddress1 = new System.Windows.Forms.Panel();
            this.txtSpecialTypes = new System.Windows.Forms.TextBox();
            this.lblSpecialTypes = new System.Windows.Forms.Label();
            this.pnlSubscriptions = new System.Windows.Forms.Panel();
            this.txtSubscriptions = new System.Windows.Forms.TextBox();
            this.lblSubscriptions = new System.Windows.Forms.Label();
            this.lblLoadingTypesSubscr = new System.Windows.Forms.Label();
            this.tbpOther = new System.Windows.Forms.TabPage();
            this.pnlOtherRight = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.pnlOtherInfo = new System.Windows.Forms.Label();
            this.pnlPreviousName = new System.Windows.Forms.Panel();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.pnlOtherRightParentUnit = new System.Windows.Forms.Panel();
            this.txtParentUnit = new System.Windows.Forms.TextBox();
            this.lblParentUnit = new System.Windows.Forms.Label();
            this.pnlOtherRightField = new System.Windows.Forms.Panel();
            this.txtField = new System.Windows.Forms.TextBox();
            this.lblField = new System.Windows.Forms.Label();
            this.pnlOtherLeft = new System.Windows.Forms.Panel();
            this.txtPartnerUpdated = new System.Windows.Forms.TextBox();
            this.lblPartnerUpdated = new System.Windows.Forms.Label();
            this.lblLanguages = new System.Windows.Forms.Label();
            this.lblLastContact = new System.Windows.Forms.Label();
            this.txtLanguages = new System.Windows.Forms.TextBox();
            this.txtLastContact = new System.Windows.Forms.TextBox();
            this.lblLoadingOther = new System.Windows.Forms.Label();
            this.tbpPersonFamily = new System.Windows.Forms.TabPage();
            this.pnlPersonFamilyRight = new System.Windows.Forms.Panel();
            this.pnlFamily = new System.Windows.Forms.Panel();
            this.txtFamily = new System.Windows.Forms.TextBox();
            this.lblFamily = new System.Windows.Forms.Label();
            this.pnlDateOfBirth = new System.Windows.Forms.Panel();
            this.txtDateOfBirth = new System.Windows.Forms.TextBox();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.pnlFamilyMembers = new System.Windows.Forms.Panel();
            this.txtFamilyMembers = new System.Windows.Forms.TextBox();
            this.lblFamilyMembers = new System.Windows.Forms.Label();
            this.lblLoadingPersonFamily = new System.Windows.Forms.Label();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.txtStatusCode = new System.Windows.Forms.TextBox();
            this.txtAcquisitionCode = new System.Windows.Forms.TextBox();
            this.lneDivider = new DevAge.Windows.Forms.Line();
            this.pnlKeyInfo = new System.Windows.Forms.Panel();
            this.lblNoPartner = new System.Windows.Forms.Label();
            this.rtbContactDetails = new Ict.Common.Controls.TRtbHyperlinks();
            this.tabPartnerDetailInfo.SuspendLayout();
            this.tbpAddress.SuspendLayout();
            this.pnlContactDetails.SuspendLayout();
            this.pnlAddressDetails.SuspendLayout();
            this.tlpPartnerLocation.SuspendLayout();
            this.tbpTypesSubscr.SuspendLayout();
            this.tlpTypesSubscriptions.SuspendLayout();
            this.pnlSpecialTypes.SuspendLayout();
            this.pnlSubscriptions.SuspendLayout();
            this.pnlAddress1.SuspendLayout();
            this.tbpOther.SuspendLayout();
            this.pnlOtherRight.SuspendLayout();
            this.pnlPreviousName.SuspendLayout();
            this.pnlOtherRightParentUnit.SuspendLayout();
            this.pnlOtherRightField.SuspendLayout();
            this.pnlOtherLeft.SuspendLayout();
            this.tbpPersonFamily.SuspendLayout();
            this.pnlPersonFamilyRight.SuspendLayout();
            this.pnlFamily.SuspendLayout();
            this.pnlDateOfBirth.SuspendLayout();
            this.pnlFamilyMembers.SuspendLayout();
            this.pnlKeyInfo.SuspendLayout();
            this.SuspendLayout();
            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(350, 2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(80, 18);
            this.lblPartnerKey.TabIndex = 10;
            this.lblPartnerKey.Text = "Partner Key:";
            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(350, 16);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.Size = new System.Drawing.Size(80, 18);
            this.lblAcquisitionCode.TabIndex = 11;
            this.lblAcquisitionCode.Text = "Acquired:";
            //
            // lblStatusCode
            //
            this.lblStatusCode.Location = new System.Drawing.Point(3, 16);
            this.lblStatusCode.Name = "lblStatusCode";
            this.lblStatusCode.Size = new System.Drawing.Size(51, 18);
            this.lblStatusCode.TabIndex = 8;
            this.lblStatusCode.Text = "Status:";
            //
            // lblPartnerName
            //
            this.lblPartnerName.Location = new System.Drawing.Point(3, 2);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(51, 16);
            this.lblPartnerName.TabIndex = 7;
            this.lblPartnerName.Text = "Name:";
            //
            // tabPartnerDetailInfo
            //
            this.tabPartnerDetailInfo.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabPartnerDetailInfo.AllowDrop = true;
            this.tabPartnerDetailInfo.Controls.Add(this.tbpAddress);
            this.tabPartnerDetailInfo.Controls.Add(this.tbpTypesSubscr);
            this.tabPartnerDetailInfo.Controls.Add(this.tbpOther);
            this.tabPartnerDetailInfo.Controls.Add(this.tbpPersonFamily);
            this.tabPartnerDetailInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPartnerDetailInfo.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabPartnerDetailInfo.Font = new System.Drawing.Font("Verdana",
                7F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.tabPartnerDetailInfo.Location = new System.Drawing.Point(0, 34);
            this.tabPartnerDetailInfo.Name = "tabPartnerDetailInfo";
            this.tabPartnerDetailInfo.SelectedIndex = 0;
            this.tabPartnerDetailInfo.ShowToolTips = true;
            this.tabPartnerDetailInfo.Size = new System.Drawing.Size(561, 182);
            this.tabPartnerDetailInfo.TabIndex = 12;
            //
            // tbpAddress
            //
            this.tbpAddress.BackColor = System.Drawing.SystemColors.Info;
            this.tbpAddress.Controls.Add(this.pnlContactDetails);
            this.tbpAddress.Controls.Add(this.pnlAddressDetails);
            this.tbpAddress.Controls.Add(this.lblLoadingPartnerLocation);
            this.tbpAddress.Controls.Add(this.lblLoadingAddress);
            this.tbpAddress.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpAddress.Location = new System.Drawing.Point(4, 4);
            this.tbpAddress.Name = "tbpAddress";
            this.tbpAddress.Padding = new System.Windows.Forms.Padding(6, 3, 2, 3);
            this.tbpAddress.Size = new System.Drawing.Size(553, 157);
            this.tbpAddress.TabIndex = 0;
            this.tbpAddress.Text = "Address && Contact Details";
            this.tbpAddress.ToolTipText = "Details of the selected Address and Contact Details of the Partner";
            //
            // pnlContactDetails
            //
            this.pnlContactDetails.AutoScroll = true;
            this.pnlContactDetails.Controls.Add(this.rtbContactDetails);
            this.pnlContactDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContactDetails.Location = new System.Drawing.Point(206, 3);
            this.pnlContactDetails.Name = "pnlContactDetails";
            this.pnlContactDetails.Size = new System.Drawing.Size(345, 151);
            this.pnlContactDetails.TabIndex = 8;
            //
            // pnlAddressDetails
            //
            this.pnlAddressDetails.AutoScroll = true;
            this.pnlAddressDetails.Controls.Add(this.tlpPartnerLocation);
            this.pnlAddressDetails.Controls.Add(this.pnlAddress1);
            this.pnlAddressDetails.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAddressDetails.Location = new System.Drawing.Point(6, 3);
            this.pnlAddressDetails.Name = "pnlAddressDetails";
            this.pnlAddressDetails.Size = new System.Drawing.Size(340, 200);
            this.pnlAddressDetails.TabIndex = 3;
            //
            // tlpPartnerLocation
            //
            this.tlpPartnerLocation.ColumnCount = 4;
            this.tlpPartnerLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95));
            this.tlpPartnerLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tlpPartnerLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpPartnerLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tlpPartnerLocation.Controls.Add(this.lblLocationType, 0, 0);
            this.tlpPartnerLocation.Controls.Add(this.txtLocationType, 1, 0);
            this.tlpPartnerLocation.Controls.Add(this.lblMailingAddress, 2, 0);
            this.tlpPartnerLocation.Controls.Add(this.txtMailingAddress, 3, 0);
            this.tlpPartnerLocation.Controls.Add(this.lblValidFrom, 0, 1);
            this.tlpPartnerLocation.Controls.Add(this.txtValidFrom, 1, 1);
            this.tlpPartnerLocation.Controls.Add(this.lblValidTo, 2, 1);
            this.tlpPartnerLocation.Controls.Add(this.txtValidTo, 3, 1);
            this.tlpPartnerLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPartnerLocation.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tlpPartnerLocation.Location = new System.Drawing.Point(0, 151);
            this.tlpPartnerLocation.Name = "tlpPartnerLocation";
            this.tlpPartnerLocation.RowCount = 2;
            this.tlpPartnerLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpPartnerLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpPartnerLocation.Size = new System.Drawing.Size(200, 0);
            this.tlpPartnerLocation.TabIndex = 2;
            //
            // lblLocationType
            //
            this.lblLocationType.AutoSize = true;
            this.lblLocationType.Location = new System.Drawing.Point(3, 70);
            this.lblLocationType.Name = "lblLocationType";
            this.lblLocationType.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblLocationType.Size = new System.Drawing.Size(87, 14);
            this.lblLocationType.TabIndex = 2;
            this.lblLocationType.Text = "Location Type:";
            //
            // txtLocationType
            //
            this.txtLocationType.BackColor = System.Drawing.SystemColors.Info;
            this.txtLocationType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLocationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocationType.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocationType.Location = new System.Drawing.Point(96, 73);
            this.txtLocationType.Name = "txtLocationType";
            this.txtLocationType.ReadOnly = true;
            this.txtLocationType.TabIndex = 1;
            //
            // lblMailingAddress
            //
            this.lblMailingAddress.AutoSize = true;
            this.lblMailingAddress.Location = new System.Drawing.Point(3, 84);
            this.lblMailingAddress.Name = "lblMailingAddress";
            this.lblMailingAddress.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblMailingAddress.Size = new System.Drawing.Size(82, 14);
            this.lblMailingAddress.TabIndex = 2;
            this.lblMailingAddress.Text = "Mailing Addr.:";
            //
            // txtMailingAddress
            //
            this.txtMailingAddress.BackColor = System.Drawing.SystemColors.Info;
            this.txtMailingAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMailingAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMailingAddress.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingAddress.Location = new System.Drawing.Point(96, 87);
            this.txtMailingAddress.Name = "txtMailingAddress";
            this.txtMailingAddress.ReadOnly = true;
            this.txtMailingAddress.Size = new System.Drawing.Size(30, 12);
            this.txtMailingAddress.TabIndex = 1;
            //
            // lblValidFrom
            //
            this.lblValidFrom.AutoSize = true;
            this.lblValidFrom.Location = new System.Drawing.Point(3, 112);
            this.lblValidFrom.Name = "lblValidFrom";
            this.lblValidFrom.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblValidFrom.Size = new System.Drawing.Size(69, 14);
            this.lblValidFrom.TabIndex = 0;
            this.lblValidFrom.Text = "Valid From:";
            //
            // txtValidFrom
            //
            this.txtValidFrom.BackColor = System.Drawing.SystemColors.Info;
            this.txtValidFrom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValidFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValidFrom.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValidFrom.Location = new System.Drawing.Point(96, 115);
            this.txtValidFrom.Name = "txtValidFrom";
            this.txtValidFrom.ReadOnly = true;
            this.txtValidFrom.Size = new System.Drawing.Size(70, 12);
            this.txtValidFrom.TabIndex = 1;
            //
            // lblValidTo
            //
            this.lblValidTo.AutoSize = true;
            this.lblValidTo.Location = new System.Drawing.Point(3, 126);
            this.lblValidTo.Name = "lblValidTo";
            this.lblValidTo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblValidTo.Size = new System.Drawing.Size(20, 14);
            this.lblValidTo.TabIndex = 0;
            this.lblValidTo.Text = "Valid To:";
            //
            // txtValidTo
            //
            this.txtValidTo.BackColor = System.Drawing.SystemColors.Info;
            this.txtValidTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValidTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValidTo.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValidTo.Location = new System.Drawing.Point(96, 129);
            this.txtValidTo.Name = "txtValidTo";
            this.txtValidTo.ReadOnly = true;
            this.txtValidTo.Size = new System.Drawing.Size(100, 12);
            this.txtValidTo.TabIndex = 1;
            //
            // txtAddress1
            //
            this.txtAddress1.BackColor = System.Drawing.SystemColors.Info;
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddress1.Location = new System.Drawing.Point(3, 0);
            this.txtAddress1.Multiline = true;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.ReadOnly = true;
            this.txtAddress1.Size = new System.Drawing.Size(200, 151);
            this.txtAddress1.TabIndex = 0;
            //
            // pnlAddress1
            //
            this.pnlAddress1.Controls.Add(this.txtAddress1);
            this.pnlAddress1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAddress1.Name = "pnlAddress1";
            this.pnlAddress1.TabIndex = 0;
            //
            // lblLoadingPartnerLocation
            //
            this.lblLoadingPartnerLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoadingPartnerLocation.Font =
                new System.Drawing.Font("Verdana", 11F, ((System.Drawing.FontStyle)(
                                                             (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),
                    System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingPartnerLocation.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingPartnerLocation.Location = new System.Drawing.Point(6, 3);
            this.lblLoadingPartnerLocation.Name = "lblLoadingPartnerLocation";
            this.lblLoadingPartnerLocation.Size = new System.Drawing.Size(545, 151);
            this.lblLoadingPartnerLocation.TabIndex = 7;
            this.lblLoadingPartnerLocation.Text = "Loading...\r\nPlease wait.MANUALTRANSLATION";
            this.lblLoadingPartnerLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadingPartnerLocation.Visible = false;
            //
            // lblLoadingAddress
            //
            this.lblLoadingAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadingAddress.Font =
                new System.Drawing.Font("Verdana", 13F, ((System.Drawing.FontStyle)(
                                                             (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),
                    System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingAddress.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingAddress.Location = new System.Drawing.Point(0, 0);
            this.lblLoadingAddress.Name = "lblLoadingAddress";
            this.lblLoadingAddress.Size = new System.Drawing.Size(556, 157);
            this.lblLoadingAddress.TabIndex = 32;
            this.lblLoadingAddress.Text = "Loading...\r\nPlease wait.MANUALTRANSLATION";
            this.lblLoadingAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadingAddress.Visible = false;
            //
            // tbpTypesSubscr
            //
            this.tbpTypesSubscr.BackColor = System.Drawing.SystemColors.Info;
            this.tbpTypesSubscr.Controls.Add(this.tlpTypesSubscriptions);
            this.tbpTypesSubscr.Controls.Add(this.lblLoadingTypesSubscr);
            this.tbpTypesSubscr.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpTypesSubscr.Location = new System.Drawing.Point(4, 4);
            this.tbpTypesSubscr.Name = "tbpTypesSubscr";
            this.tbpTypesSubscr.Size = new System.Drawing.Size(553, 157);
            this.tbpTypesSubscr.TabIndex = 1;
            this.tbpTypesSubscr.Text = "Types && Subscriptions";
            this.tbpTypesSubscr.ToolTipText = "Subscriptions and Special Types of the Partner";
            //
            // tlpTypesSubscriptions
            //
            this.tlpTypesSubscriptions.ColumnCount = 2;
            this.tlpTypesSubscriptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTypesSubscriptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTypesSubscriptions.Controls.Add(this.pnlSpecialTypes, 0, 0);
            this.tlpTypesSubscriptions.Controls.Add(this.pnlSubscriptions, 1, 0);
            this.tlpTypesSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTypesSubscriptions.Location = new System.Drawing.Point(0, 0);
            this.tlpTypesSubscriptions.Name = "tlpTypesSubscriptions";
            this.tlpTypesSubscriptions.RowCount = 1;
            this.tlpTypesSubscriptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTypesSubscriptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tlpTypesSubscriptions.Size = new System.Drawing.Size(553, 157);
            this.tlpTypesSubscriptions.TabIndex = 5;
            //
            // pnlSpecialTypes
            //
            this.pnlSpecialTypes.Controls.Add(this.txtSpecialTypes);
            this.pnlSpecialTypes.Controls.Add(this.lblSpecialTypes);
            this.pnlSpecialTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSpecialTypes.Location = new System.Drawing.Point(3, 3);
            this.pnlSpecialTypes.Name = "pnlSpecialTypes";
            this.pnlSpecialTypes.Size = new System.Drawing.Size(270, 151);
            this.pnlSpecialTypes.TabIndex = 0;
            //
            // txtSpecialTypes
            //
            this.txtSpecialTypes.BackColor = System.Drawing.SystemColors.Info;
            this.txtSpecialTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSpecialTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSpecialTypes.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpecialTypes.Location = new System.Drawing.Point(0, 13);
            this.txtSpecialTypes.Multiline = true;
            this.txtSpecialTypes.Name = "txtSpecialTypes";
            this.txtSpecialTypes.ReadOnly = true;
            this.txtSpecialTypes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSpecialTypes.Size = new System.Drawing.Size(270, 138);
            this.txtSpecialTypes.TabIndex = 1;
            //
            // lblSpecialTypes
            //
            this.lblSpecialTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSpecialTypes.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecialTypes.Location = new System.Drawing.Point(0, 0);
            this.lblSpecialTypes.Name = "lblSpecialTypes";
            this.lblSpecialTypes.Size = new System.Drawing.Size(270, 13);
            this.lblSpecialTypes.TabIndex = 2;
            this.lblSpecialTypes.Text = "Special Types:";
            //
            // pnlSubscriptions
            //
            this.pnlSubscriptions.Controls.Add(this.txtSubscriptions);
            this.pnlSubscriptions.Controls.Add(this.lblSubscriptions);
            this.pnlSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubscriptions.Location = new System.Drawing.Point(279, 3);
            this.pnlSubscriptions.Name = "pnlSubscriptions";
            this.pnlSubscriptions.Size = new System.Drawing.Size(271, 151);
            this.pnlSubscriptions.TabIndex = 1;
            //
            // txtSubscriptions
            //
            this.txtSubscriptions.BackColor = System.Drawing.SystemColors.Info;
            this.txtSubscriptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubscriptions.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptions.Location = new System.Drawing.Point(0, 13);
            this.txtSubscriptions.Multiline = true;
            this.txtSubscriptions.Name = "txtSubscriptions";
            this.txtSubscriptions.ReadOnly = true;
            this.txtSubscriptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSubscriptions.Size = new System.Drawing.Size(271, 138);
            this.txtSubscriptions.TabIndex = 3;
            //
            // lblSubscriptions
            //
            this.lblSubscriptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubscriptions.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptions.Location = new System.Drawing.Point(0, 0);
            this.lblSubscriptions.Name = "lblSubscriptions";
            this.lblSubscriptions.Size = new System.Drawing.Size(271, 13);
            this.lblSubscriptions.TabIndex = 4;
            this.lblSubscriptions.Text = "Subscriptions:";
            //
            // lblLoadingTypesSubscr
            //
            this.lblLoadingTypesSubscr.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadingTypesSubscr.Font =
                new System.Drawing.Font("Verdana", 13F, ((System.Drawing.FontStyle)(
                                                             (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),
                    System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingTypesSubscr.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingTypesSubscr.Location = new System.Drawing.Point(0, 0);
            this.lblLoadingTypesSubscr.Name = "lblLoadingTypesSubscr";
            this.lblLoadingTypesSubscr.Size = new System.Drawing.Size(556, 157);
            this.lblLoadingTypesSubscr.TabIndex = 6;
            this.lblLoadingTypesSubscr.Text = "Loading...\r\nPlease wait.MANUALTRANSLATION";
            this.lblLoadingTypesSubscr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadingTypesSubscr.Visible = false;
            //
            // tbpOther
            //
            this.tbpOther.BackColor = System.Drawing.SystemColors.Info;
            this.tbpOther.Controls.Add(this.pnlOtherRight);
            this.tbpOther.Controls.Add(this.pnlOtherRightParentUnit);
            this.tbpOther.Controls.Add(this.pnlOtherRightField);
            this.tbpOther.Controls.Add(this.pnlOtherLeft);
            this.tbpOther.Controls.Add(this.lblLoadingOther);
            this.tbpOther.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpOther.Location = new System.Drawing.Point(4, 4);
            this.tbpOther.Name = "tbpOther";
            this.tbpOther.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.tbpOther.Size = new System.Drawing.Size(553, 157);
            this.tbpOther.TabIndex = 2;
            this.tbpOther.Text = "Other";
            this.tbpOther.ToolTipText = "Other details of the Partner";
            //
            // pnlOtherRight
            //
            this.pnlOtherRight.Controls.Add(this.txtNotes);
            this.pnlOtherRight.Controls.Add(this.pnlOtherInfo);
            this.pnlOtherRight.Controls.Add(this.pnlPreviousName);
            this.pnlOtherRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOtherRight.Location = new System.Drawing.Point(253, 28);
            this.pnlOtherRight.Name = "pnlOtherRight";
            this.pnlOtherRight.Size = new System.Drawing.Size(297, 126);
            this.pnlOtherRight.TabIndex = 25;
            //
            // txtNotes
            //
            this.txtNotes.BackColor = System.Drawing.SystemColors.Info;
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(0, 28);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(297, 98);
            this.txtNotes.TabIndex = 2;
            //
            // pnlOtherInfo
            //
            this.pnlOtherInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOtherInfo.Location = new System.Drawing.Point(0, 14);
            this.pnlOtherInfo.Name = "pnlOtherInfo";
            this.pnlOtherInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.pnlOtherInfo.Size = new System.Drawing.Size(297, 14);
            this.pnlOtherInfo.TabIndex = 24;
            this.pnlOtherInfo.Text = "Other Information:";
            //
            // pnlPreviousName
            //
            this.pnlPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreviousName.Location = new System.Drawing.Point(0, 0);
            this.pnlPreviousName.Name = "pnlPreviousName";
            this.pnlPreviousName.Size = new System.Drawing.Size(297, 14);
            this.pnlPreviousName.TabIndex = 25;
            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.BackColor = System.Drawing.SystemColors.Info;
            this.txtPreviousName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreviousName.Location = new System.Drawing.Point(110, 2);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.ReadOnly = true;
            this.txtPreviousName.Size = new System.Drawing.Size(185, 12);
            this.txtPreviousName.TabIndex = 26;
            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(0, 2);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(94, 18);
            this.lblPreviousName.TabIndex = 25;
            this.lblPreviousName.Text = "Previous Name:";
            //
            // pnlOtherRightParentUnit
            //
            this.pnlOtherRightParentUnit.Controls.Add(this.txtParentUnit);
            this.pnlOtherRightParentUnit.Controls.Add(this.lblParentUnit);
            this.pnlOtherRightParentUnit.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOtherRightParentUnit.Location = new System.Drawing.Point(253, 14);
            this.pnlOtherRightParentUnit.Name = "pnlOtherRightParentUnit";
            this.pnlOtherRightParentUnit.Size = new System.Drawing.Size(297, 14);
            this.pnlOtherRightParentUnit.TabIndex = 24;
            //
            // txtParentUnit
            //
            this.txtParentUnit.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtParentUnit.BackColor = System.Drawing.SystemColors.Info;
            this.txtParentUnit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtParentUnit.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParentUnit.Location = new System.Drawing.Point(110, 2);
            this.txtParentUnit.Name = "txtParentUnit";
            this.txtParentUnit.ReadOnly = true;
            this.txtParentUnit.Size = new System.Drawing.Size(185, 12);
            this.txtParentUnit.TabIndex = 26;
            //
            // lblParentUnit
            //
            this.lblParentUnit.Location = new System.Drawing.Point(0, 2);
            this.lblParentUnit.Name = "lblParentUnit";
            this.lblParentUnit.Size = new System.Drawing.Size(83, 18);
            this.lblParentUnit.TabIndex = 25;
            this.lblParentUnit.Text = "Parent Unit:";
            //
            // pnlOtherRightField
            //
            this.pnlOtherRightField.Controls.Add(this.txtField);
            this.pnlOtherRightField.Controls.Add(this.lblField);
            this.pnlOtherRightField.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOtherRightField.Location = new System.Drawing.Point(253, 0);
            this.pnlOtherRightField.Name = "pnlOtherRightField";
            this.pnlOtherRightField.Size = new System.Drawing.Size(297, 14);
            this.pnlOtherRightField.TabIndex = 26;
            //
            // txtField
            //
            this.txtField.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtField.BackColor = System.Drawing.SystemColors.Info;
            this.txtField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtField.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtField.Location = new System.Drawing.Point(110, 2);
            this.txtField.Name = "txtField";
            this.txtField.ReadOnly = true;
            this.txtField.Size = new System.Drawing.Size(185, 12);
            this.txtField.TabIndex = 26;
            //
            // lblField
            //
            this.lblField.Location = new System.Drawing.Point(0, 2);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(83, 18);
            this.lblField.TabIndex = 25;
            this.lblField.Text = "Field:";
            //
            // pnlOtherLeft
            //
            this.pnlOtherLeft.Controls.Add(this.txtPartnerUpdated);
            this.pnlOtherLeft.Controls.Add(this.lblPartnerUpdated);
            this.pnlOtherLeft.Controls.Add(this.lblLanguages);
            this.pnlOtherLeft.Controls.Add(this.lblLastContact);
            this.pnlOtherLeft.Controls.Add(this.txtLanguages);
            this.pnlOtherLeft.Controls.Add(this.txtLastContact);
            this.pnlOtherLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlOtherLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlOtherLeft.Name = "pnlOtherLeft";
            this.pnlOtherLeft.Size = new System.Drawing.Size(253, 154);
            this.pnlOtherLeft.TabIndex = 23;
            //
            // txtPartnerUpdated
            //
            this.txtPartnerUpdated.BackColor = System.Drawing.SystemColors.Info;
            this.txtPartnerUpdated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerUpdated.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartnerUpdated.Location = new System.Drawing.Point(104, 44);
            this.txtPartnerUpdated.Name = "txtPartnerUpdated";
            this.txtPartnerUpdated.ReadOnly = true;
            this.txtPartnerUpdated.Size = new System.Drawing.Size(74, 12);
            this.txtPartnerUpdated.TabIndex = 22;
            //
            // lblPartnerUpdated
            //
            this.lblPartnerUpdated.Location = new System.Drawing.Point(3, 44);
            this.lblPartnerUpdated.Name = "lblPartnerUpdated";
            this.lblPartnerUpdated.Size = new System.Drawing.Size(107, 19);
            this.lblPartnerUpdated.TabIndex = 21;
            this.lblPartnerUpdated.Text = "Partner Updated:";
            //
            // lblLanguages
            //
            this.lblLanguages.Location = new System.Drawing.Point(3, 14);
            this.lblLanguages.Name = "lblLanguages";
            this.lblLanguages.Size = new System.Drawing.Size(83, 18);
            this.lblLanguages.TabIndex = 20;
            this.lblLanguages.Text = "Speaks (Languages):";
            //
            // lblLastContact
            //
            this.lblLastContact.Location = new System.Drawing.Point(3, 31);
            this.lblLastContact.Name = "lblLastContact";
            this.lblLastContact.Size = new System.Drawing.Size(83, 18);
            this.lblLastContact.TabIndex = 17;
            this.lblLastContact.Text = "Last Contact:";
            //
            // txtLanguages
            //
            this.txtLanguages.BackColor = System.Drawing.SystemColors.Info;
            this.txtLanguages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLanguages.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLanguages.Location = new System.Drawing.Point(85, 3);
            this.txtLanguages.Multiline = true;
            this.txtLanguages.Name = "txtLanguages";
            this.txtLanguages.ReadOnly = true;
            this.txtLanguages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLanguages.Size = new System.Drawing.Size(167, 25);
            this.txtLanguages.TabIndex = 18;
            //
            // txtLastContact
            //
            this.txtLastContact.BackColor = System.Drawing.SystemColors.Info;
            this.txtLastContact.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastContact.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastContact.Location = new System.Drawing.Point(104, 31);
            this.txtLastContact.Name = "txtLastContact";
            this.txtLastContact.ReadOnly = true;
            this.txtLastContact.Size = new System.Drawing.Size(74, 12);
            this.txtLastContact.TabIndex = 19;
            //
            // lblLoadingOther
            //
            this.lblLoadingOther.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadingOther.Font =
                new System.Drawing.Font("Verdana", 13F, ((System.Drawing.FontStyle)(
                                                             (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),
                    System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingOther.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingOther.Location = new System.Drawing.Point(0, 0);
            this.lblLoadingOther.Name = "lblLoadingOther";
            this.lblLoadingOther.Size = new System.Drawing.Size(556, 157);
            this.lblLoadingOther.TabIndex = 27;
            this.lblLoadingOther.Text = "Loading...\r\nPlease wait.MANUALTRANSLATION";
            this.lblLoadingOther.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadingOther.Visible = false;
            //
            // tbpPersonFamily
            //
            this.tbpPersonFamily.BackColor = System.Drawing.SystemColors.Info;
            this.tbpPersonFamily.Controls.Add(this.pnlPersonFamilyRight);
            this.tbpPersonFamily.Controls.Add(this.pnlFamilyMembers);
            this.tbpPersonFamily.Controls.Add(this.lblLoadingPersonFamily);
            this.tbpPersonFamily.Location = new System.Drawing.Point(4, 4);
            this.tbpPersonFamily.Name = "tbpPersonFamily";
            this.tbpPersonFamily.Padding = new System.Windows.Forms.Padding(3);
            this.tbpPersonFamily.Size = new System.Drawing.Size(553, 157);
            this.tbpPersonFamily.TabIndex = 3;
            this.tbpPersonFamily.Text = "Person / Family";
            //
            // pnlPersonFamilyRight
            //
            this.pnlPersonFamilyRight.Controls.Add(this.pnlFamily);
            this.pnlPersonFamilyRight.Controls.Add(this.pnlDateOfBirth);
            this.pnlPersonFamilyRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPersonFamilyRight.Location = new System.Drawing.Point(273, 3);
            this.pnlPersonFamilyRight.Name = "pnlPersonFamilyRight";
            this.pnlPersonFamilyRight.Size = new System.Drawing.Size(277, 151);
            this.pnlPersonFamilyRight.TabIndex = 34;
            //
            // pnlFamily
            //
            this.pnlFamily.Controls.Add(this.txtFamily);
            this.pnlFamily.Controls.Add(this.lblFamily);
            this.pnlFamily.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFamily.Location = new System.Drawing.Point(0, 15);
            this.pnlFamily.Name = "pnlFamily";
            this.pnlFamily.Size = new System.Drawing.Size(277, 15);
            this.pnlFamily.TabIndex = 33;
            //
            // txtFamily
            //
            this.txtFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtFamily.BackColor = System.Drawing.SystemColors.Info;
            this.txtFamily.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFamily.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFamily.Location = new System.Drawing.Point(86, 1);
            this.txtFamily.Name = "txtFamily";
            this.txtFamily.ReadOnly = true;
            this.txtFamily.Size = new System.Drawing.Size(189, 12);
            this.txtFamily.TabIndex = 30;
            //
            // lblFamily
            //
            this.lblFamily.Location = new System.Drawing.Point(4, 1);
            this.lblFamily.Name = "lblFamily";
            this.lblFamily.Size = new System.Drawing.Size(78, 12);
            this.lblFamily.TabIndex = 29;
            this.lblFamily.Text = "Family:";
            //
            // pnlDateOfBirth
            //
            this.pnlDateOfBirth.Controls.Add(this.txtDateOfBirth);
            this.pnlDateOfBirth.Controls.Add(this.lblDateOfBirth);
            this.pnlDateOfBirth.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDateOfBirth.Location = new System.Drawing.Point(0, 0);
            this.pnlDateOfBirth.Name = "pnlDateOfBirth";
            this.pnlDateOfBirth.Size = new System.Drawing.Size(277, 15);
            this.pnlDateOfBirth.TabIndex = 32;
            //
            // txtDateOfBirth
            //
            this.txtDateOfBirth.BackColor = System.Drawing.SystemColors.Info;
            this.txtDateOfBirth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDateOfBirth.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateOfBirth.Location = new System.Drawing.Point(86, 1);
            this.txtDateOfBirth.Name = "txtDateOfBirth";
            this.txtDateOfBirth.ReadOnly = true;
            this.txtDateOfBirth.Size = new System.Drawing.Size(73, 12);
            this.txtDateOfBirth.TabIndex = 30;
            //
            // lblDateOfBirth
            //
            this.lblDateOfBirth.Location = new System.Drawing.Point(4, 1);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new System.Drawing.Size(81, 12);
            this.lblDateOfBirth.TabIndex = 29;
            this.lblDateOfBirth.Text = "Date of Birth:";
            //
            // pnlFamilyMembers
            //
            this.pnlFamilyMembers.Controls.Add(this.txtFamilyMembers);
            this.pnlFamilyMembers.Controls.Add(this.lblFamilyMembers);
            this.pnlFamilyMembers.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFamilyMembers.Location = new System.Drawing.Point(3, 3);
            this.pnlFamilyMembers.Name = "pnlFamilyMembers";
            this.pnlFamilyMembers.Size = new System.Drawing.Size(270, 151);
            this.pnlFamilyMembers.TabIndex = 33;
            //
            // txtFamilyMembers
            //
            this.txtFamilyMembers.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtFamilyMembers.BackColor = System.Drawing.SystemColors.Info;
            this.txtFamilyMembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFamilyMembers.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFamilyMembers.Location = new System.Drawing.Point(0, 13);
            this.txtFamilyMembers.Multiline = true;
            this.txtFamilyMembers.Name = "txtFamilyMembers";
            this.txtFamilyMembers.ReadOnly = true;
            this.txtFamilyMembers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFamilyMembers.Size = new System.Drawing.Size(270, 138);
            this.txtFamilyMembers.TabIndex = 32;
            //
            // lblFamilyMembers
            //
            this.lblFamilyMembers.Location = new System.Drawing.Point(0, 0);
            this.lblFamilyMembers.Name = "lblFamilyMembers";
            this.lblFamilyMembers.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblFamilyMembers.Size = new System.Drawing.Size(158, 15);
            this.lblFamilyMembers.TabIndex = 31;
            this.lblFamilyMembers.Text = "Family Members:";
            //
            // lblLoadingPersonFamily
            //
            this.lblLoadingPersonFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoadingPersonFamily.Font =
                new System.Drawing.Font("Verdana", 13F, ((System.Drawing.FontStyle)(
                                                             (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),
                    System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadingPersonFamily.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLoadingPersonFamily.Location = new System.Drawing.Point(0, 0);
            this.lblLoadingPersonFamily.Name = "lblLoadingPersonFamily";
            this.lblLoadingPersonFamily.Size = new System.Drawing.Size(556, 157);
            this.lblLoadingPersonFamily.TabIndex = 31;
            this.lblLoadingPersonFamily.Text = "Loading...\r\nPlease wait.MANUALTRANSLATION";
            this.lblLoadingPersonFamily.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoadingPersonFamily.Visible = false;
            //
            // txtPartnerName
            //
            this.txtPartnerName.BackColor = System.Drawing.SystemColors.Info;
            this.txtPartnerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartnerName.Location = new System.Drawing.Point(49, 2);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.ReadOnly = true;
            this.txtPartnerName.Size = new System.Drawing.Size(275, 14);
            this.txtPartnerName.TabIndex = 13;
            //
            // txtPartnerKey
            //
            this.txtPartnerKey.BackColor = System.Drawing.SystemColors.Info;
            this.txtPartnerKey.DelegateFallbackLabel = true;
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 9.25F, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.LabelText = "Markus\' World";
            this.txtPartnerKey.Location = new System.Drawing.Point(427, 0);
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PartnerClass = null;
            this.txtPartnerKey.PartnerKey = ((long)(123456789));
            this.txtPartnerKey.ReadOnly = true;
            this.txtPartnerKey.ShowLabel = false;
            this.txtPartnerKey.Size = new System.Drawing.Size(82, 22);
            this.txtPartnerKey.TabIndex = 14;
            this.txtPartnerKey.TextBoxReadOnly = true;
            this.txtPartnerKey.TextBoxWidth = 75;
            //
            // txtStatusCode
            //
            this.txtStatusCode.BackColor = System.Drawing.SystemColors.Info;
            this.txtStatusCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatusCode.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusCode.Location = new System.Drawing.Point(49, 16);
            this.txtStatusCode.Name = "txtStatusCode";
            this.txtStatusCode.ReadOnly = true;
            this.txtStatusCode.Size = new System.Drawing.Size(118, 12);
            this.txtStatusCode.TabIndex = 15;
            //
            // txtAcquisitionCode
            //
            this.txtAcquisitionCode.BackColor = System.Drawing.SystemColors.Info;
            this.txtAcquisitionCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAcquisitionCode.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAcquisitionCode.Location = new System.Drawing.Point(410, 16);
            this.txtAcquisitionCode.Name = "txtAcquisitionCode";
            this.txtAcquisitionCode.ReadOnly = true;
            this.txtAcquisitionCode.Size = new System.Drawing.Size(87, 12);
            this.txtAcquisitionCode.TabIndex = 17;
            this.txtAcquisitionCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // lneDivider
            //
            this.lneDivider.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.lneDivider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lneDivider.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.lneDivider.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            this.lneDivider.Location = new System.Drawing.Point(0, 32);
            this.lneDivider.Name = "lneDivider";
            this.lneDivider.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.lneDivider.Size = new System.Drawing.Size(561, 2);
            this.lneDivider.TabIndex = 18;
            this.lneDivider.TabStop = false;
            //
            // pnlKeyInfo
            //
            this.pnlKeyInfo.AutoSize = true;
            this.pnlKeyInfo.Controls.Add(this.txtAcquisitionCode);
            this.pnlKeyInfo.Controls.Add(this.txtPartnerKey);
            this.pnlKeyInfo.Controls.Add(this.txtPartnerName);
            this.pnlKeyInfo.Controls.Add(this.txtStatusCode);
            this.pnlKeyInfo.Controls.Add(this.lblPartnerName);
            this.pnlKeyInfo.Controls.Add(this.lneDivider);
            this.pnlKeyInfo.Controls.Add(this.lblStatusCode);
            this.pnlKeyInfo.Controls.Add(this.lblAcquisitionCode);
            this.pnlKeyInfo.Controls.Add(this.lblPartnerKey);
            this.pnlKeyInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKeyInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlKeyInfo.Name = "pnlKeyInfo";
            this.pnlKeyInfo.Size = new System.Drawing.Size(561, 34);
            this.pnlKeyInfo.TabIndex = 19;
            //
            // lblNoPartner
            //
            this.lblNoPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoPartner.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoPartner.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblNoPartner.Location = new System.Drawing.Point(0, 0);
            this.lblNoPartner.Name = "lblNoPartner";
            this.lblNoPartner.Size = new System.Drawing.Size(561, 216);
            this.lblNoPartner.TabIndex = 32;
            this.lblNoPartner.Text = "No Partner to display.";
            this.lblNoPartner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoPartner.Visible = false;
            //
            // rtbContactDetails
            //
            this.rtbContactDetails.AutoSize = true;
            this.rtbContactDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rtbContactDetails.BackColor = System.Drawing.SystemColors.Info;
            this.rtbContactDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContactDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContactDetails.UseSmallTextFont = true;
            this.rtbContactDetails.Location = new System.Drawing.Point(0, 0);
            this.rtbContactDetails.Name = "rtbContactDetails";
            this.rtbContactDetails.Size = new System.Drawing.Size(345, 151);
            this.rtbContactDetails.TabIndex = 0;
            this.rtbContactDetails.LinkClicked += rtbContactDetails_LinkClicked;
            //
            // TUC_PartnerInfo
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.Controls.Add(this.tabPartnerDetailInfo);
            this.Controls.Add(this.pnlKeyInfo);
            this.Controls.Add(this.lblNoPartner);
            this.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.Name = "TUC_PartnerInfo";
            this.Size = new System.Drawing.Size(561, 216);
            this.tabPartnerDetailInfo.ResumeLayout(false);
            this.tbpAddress.ResumeLayout(false);
            this.pnlContactDetails.ResumeLayout(false);
            this.pnlContactDetails.PerformLayout();
            this.pnlAddressDetails.ResumeLayout(false);
            this.pnlAddressDetails.PerformLayout();
            this.tlpPartnerLocation.ResumeLayout(false);
            this.tlpPartnerLocation.PerformLayout();
            this.tbpTypesSubscr.ResumeLayout(false);
            this.tlpTypesSubscriptions.ResumeLayout(false);
            this.pnlSpecialTypes.ResumeLayout(false);
            this.pnlAddress1.PerformLayout();
            this.pnlSpecialTypes.PerformLayout();
            this.pnlSubscriptions.ResumeLayout(false);
            this.pnlSubscriptions.PerformLayout();
            this.tbpOther.ResumeLayout(false);
            this.pnlOtherRight.ResumeLayout(false);
            this.pnlOtherRight.PerformLayout();
            this.pnlPreviousName.ResumeLayout(false);
            this.pnlPreviousName.PerformLayout();
            this.pnlOtherRightParentUnit.ResumeLayout(false);
            this.pnlOtherRightParentUnit.PerformLayout();
            this.pnlOtherRightField.ResumeLayout(false);
            this.pnlOtherRightField.PerformLayout();
            this.pnlOtherLeft.ResumeLayout(false);
            this.pnlOtherLeft.PerformLayout();
            this.tbpPersonFamily.ResumeLayout(false);
            this.pnlPersonFamilyRight.ResumeLayout(false);
            this.pnlFamily.ResumeLayout(false);
            this.pnlFamily.PerformLayout();
            this.pnlDateOfBirth.ResumeLayout(false);
            this.pnlDateOfBirth.PerformLayout();
            this.pnlFamilyMembers.ResumeLayout(false);
            this.pnlFamilyMembers.PerformLayout();
            this.pnlKeyInfo.ResumeLayout(false);
            this.pnlKeyInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.pnlOtherRightField.ResumeLayout(false);
            this.pnlOtherRightField.PerformLayout();
            this.pnlOtherLeft.ResumeLayout(false);
            this.pnlOtherLeft.PerformLayout();
            this.tbpPersonFamily.ResumeLayout(false);
            this.pnlPersonFamilyRight.ResumeLayout(false);
            this.pnlFamily.ResumeLayout(false);
            this.pnlFamily.PerformLayout();
            this.pnlDateOfBirth.ResumeLayout(false);
            this.pnlDateOfBirth.PerformLayout();
            this.pnlFamilyMembers.ResumeLayout(false);
            this.pnlFamilyMembers.PerformLayout();
            this.pnlKeyInfo.ResumeLayout(false);
            this.pnlKeyInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblFamily;
        private System.Windows.Forms.TextBox txtFamily;
        private System.Windows.Forms.Panel pnlFamily;
        private System.Windows.Forms.Panel pnlPersonFamilyRight;
        private System.Windows.Forms.Panel pnlDateOfBirth;
        private System.Windows.Forms.Panel pnlFamilyMembers;
        private System.Windows.Forms.Label lblNoPartner;
        private System.Windows.Forms.Panel pnlPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Label lblLoadingAddress;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private System.Windows.Forms.Label lblPartnerUpdated;
        private System.Windows.Forms.Label lblStatusCode;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.Label lblSubscriptions;
        private System.Windows.Forms.Label lblSpecialTypes;
        private System.Windows.Forms.Label lblLanguages;
        private System.Windows.Forms.Label lblLastContact;
        private System.Windows.Forms.Label lblParentUnit;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Label lblLocationType;
        private System.Windows.Forms.Label lblMailingAddress;
        private System.Windows.Forms.Label lblValidFrom;
        private System.Windows.Forms.Label lblValidTo;
        private System.Windows.Forms.Label lblFamilyMembers;
        private System.Windows.Forms.Label lblDateOfBirth;
        private System.Windows.Forms.TextBox txtStatusCode;
        private System.Windows.Forms.Label lblLoadingPersonFamily;
        private System.Windows.Forms.TextBox txtDateOfBirth;
        private System.Windows.Forms.TextBox txtFamilyMembers;
        private System.Windows.Forms.TabPage tbpPersonFamily;
        private System.Windows.Forms.TextBox txtPartnerUpdated;
        private System.Windows.Forms.TextBox txtSubscriptions;
        private System.Windows.Forms.TextBox txtLastContact;
        private System.Windows.Forms.TextBox txtLanguages;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtAcquisitionCode;
        private System.Windows.Forms.TextBox txtMailingAddress;
        private System.Windows.Forms.Panel pnlContactDetails;
        private System.Windows.Forms.Label lblLoadingOther;
        private System.Windows.Forms.Label lblLoadingTypesSubscr;
        private System.Windows.Forms.Label lblLoadingPartnerLocation;
        private System.Windows.Forms.TextBox txtField;
        private System.Windows.Forms.Panel pnlOtherRightField;
        private System.Windows.Forms.Panel pnlOtherRightParentUnit;
        private System.Windows.Forms.TextBox txtParentUnit;
        private System.Windows.Forms.Panel pnlOtherRight;
        private System.Windows.Forms.Label pnlOtherInfo;
        private System.Windows.Forms.Panel pnlSubscriptions;
        private System.Windows.Forms.Panel pnlSpecialTypes;
        private System.Windows.Forms.Panel pnlAddress1;
        private System.Windows.Forms.TableLayoutPanel tlpTypesSubscriptions;
        private System.Windows.Forms.Panel pnlOtherLeft;
        private System.Windows.Forms.Panel pnlKeyInfo;
        private System.Windows.Forms.TextBox txtValidTo;
        private System.Windows.Forms.TextBox txtValidFrom;
        private Ict.Common.Controls.TRtbHyperlinks rtbContactDetails;
        private System.Windows.Forms.TextBox txtLocationType;
        private System.Windows.Forms.TableLayoutPanel tlpPartnerLocation;
        private DevAge.Windows.Forms.Line lneDivider;
        private System.Windows.Forms.TextBox txtSpecialTypes;
        private System.Windows.Forms.TextBox txtAddress1;
        private Ict.Common.Controls.TTxtPartnerKeyTextBox txtPartnerKey;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.TabPage tbpOther;
        private System.Windows.Forms.TabPage tbpTypesSubscr;
        private System.Windows.Forms.TabPage tbpAddress;
        private Ict.Common.Controls.TTabVersatile tabPartnerDetailInfo;
        private System.Windows.Forms.Panel pnlAddressDetails;
    }
}