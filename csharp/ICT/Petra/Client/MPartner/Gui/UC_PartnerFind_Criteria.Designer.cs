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
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerFindCriteria
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart =
                new Ict.Petra.Client.MPartner.Gui.TUC_PartnerFind_PersonnelCriteria_CollapsiblePart();
            this.spcCriteria = new System.Windows.Forms.SplitContainer();
            this.pnlRightColumn = new System.Windows.Forms.Panel();
            this.pnlLocationKey = new System.Windows.Forms.Panel();
            this.btnLocationKey = new System.Windows.Forms.Button();
            this.txtLocationKey = new System.Windows.Forms.TextBox();
            this.pnlPartnerClass = new System.Windows.Forms.Panel();
            this.chkWorkerFamOnly = new System.Windows.Forms.CheckBox();
            this.cmbPartnerClass = new System.Windows.Forms.ComboBox();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.pnlPartnerKey = new System.Windows.Forms.Panel();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtMaskedTextBox(this.components);
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblPartnerKeyNonExactMatch = new System.Windows.Forms.Label();
            this.pnlPartnerStatus = new System.Windows.Forms.Panel();
            this.rbtPrivate = new System.Windows.Forms.RadioButton();
            this.rbtStatusActive = new System.Windows.Forms.RadioButton();
            this.rbtStatusAll = new System.Windows.Forms.RadioButton();
            this.lblPartnerStatus = new System.Windows.Forms.Label();
            this.pnlPersonnelCriteria = new System.Windows.Forms.Panel();
            this.pnlLeftColumn = new System.Windows.Forms.Panel();
            this.pnlPhoneNumber = new System.Windows.Forms.Panel();
            this.critPhoneNumber = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.pnlAddress3 = new System.Windows.Forms.Panel();
            this.critAddress3 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress3 = new System.Windows.Forms.TextBox();
            this.lblAddress3 = new System.Windows.Forms.Label();
            this.pnlAddress2 = new System.Windows.Forms.Panel();
            this.critAddress2 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.pnlEmail = new System.Windows.Forms.Panel();
            this.critEmail = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.pnlPartnerName = new System.Windows.Forms.Panel();
            this.critPartnerName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.pnlPersonalName = new System.Windows.Forms.Panel();
            this.critPersonalName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPersonalName = new System.Windows.Forms.TextBox();
            this.lblPersonalName = new System.Windows.Forms.Label();
            this.pnlPreviousName = new System.Windows.Forms.Panel();
            this.critPreviousName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.pnlAddress1 = new System.Windows.Forms.Panel();
            this.critAddress1 = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.pnlPostCode = new System.Windows.Forms.Panel();
            this.critPostCode = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblPostCode = new System.Windows.Forms.Label();
            this.txtPostCode = new System.Windows.Forms.TextBox();
            this.pnlCity = new System.Windows.Forms.Panel();
            this.critCity = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.pnlCounty = new System.Windows.Forms.Panel();
            this.critCounty = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtCounty = new System.Windows.Forms.TextBox();
            this.lblCounty = new System.Windows.Forms.Label();
            this.pnlCountry = new System.Windows.Forms.Panel();
            this.ucoCountryComboBox = new Ict.Petra.Client.CommonControls.TUC_CountryComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.pnlMailingAddressOnly = new System.Windows.Forms.Panel();
            this.chkMailingAddressOnly = new System.Windows.Forms.CheckBox();
            this.lblMailingAddressOnly = new System.Windows.Forms.Label();
            this.pnlAccountName = new System.Windows.Forms.Panel();
            this.critAccountName = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.lblAccountName = new System.Windows.Forms.Label();
            this.pnlAccountNumber = new System.Windows.Forms.Panel();
            this.critAccountNumber = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.lblAccountNumber = new System.Windows.Forms.Label();
            this.pnlIban = new System.Windows.Forms.Panel();
            this.critIban = new Ict.Petra.Client.CommonControls.SplitButton();
            this.txtIban = new System.Windows.Forms.TextBox();
            this.lblIban = new System.Windows.Forms.Label();
            this.pnlBic = new System.Windows.Forms.Panel();
            this.critBic = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblBic = new System.Windows.Forms.Label();
            this.txtBic = new System.Windows.Forms.TextBox();
            this.pnlBranchCode = new System.Windows.Forms.Panel();
            this.critBranchCode = new Ict.Petra.Client.CommonControls.SplitButton();
            this.lblBranchCode = new System.Windows.Forms.Label();
            this.txtBranchCode = new System.Windows.Forms.TextBox();
            this.tipUC = new System.Windows.Forms.ToolTip(this.components);
            this.spcCriteria.Panel1.SuspendLayout();
            this.spcCriteria.Panel2.SuspendLayout();
            this.spcCriteria.SuspendLayout();
            this.pnlRightColumn.SuspendLayout();
            this.pnlLocationKey.SuspendLayout();
            this.pnlPartnerClass.SuspendLayout();
            this.pnlPartnerKey.SuspendLayout();
            this.pnlPartnerStatus.SuspendLayout();
            this.pnlLeftColumn.SuspendLayout();
            this.pnlPhoneNumber.SuspendLayout();
            this.pnlAddress3.SuspendLayout();
            this.pnlAddress2.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlPartnerName.SuspendLayout();
            this.pnlPersonalName.SuspendLayout();
            this.pnlPreviousName.SuspendLayout();
            this.pnlAddress1.SuspendLayout();
            this.pnlPostCode.SuspendLayout();
            this.pnlCity.SuspendLayout();
            this.pnlCounty.SuspendLayout();
            this.pnlCountry.SuspendLayout();
            this.pnlMailingAddressOnly.SuspendLayout();
            this.pnlAccountName.SuspendLayout();
            this.pnlAccountNumber.SuspendLayout();
            this.pnlIban.SuspendLayout();
            this.pnlBic.SuspendLayout();
            this.pnlBranchCode.SuspendLayout();
            this.SuspendLayout();

            //
            // ucoPartnerFind_PersonnelCriteria_CollapsiblePart
            //
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.
                                                       Left) |
                                                      System.Windows.Forms.AnchorStyles.
                                                      Right)));
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.BackColor = System.Drawing.SystemColors.Control;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Caption = "Personnel Criteria";
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Location = new System.Drawing.Point(4, 0);
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Name = "ucoPartnerFind_PersonnelCriteria_CollapsiblePart";
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Size = new System.Drawing.Size(302, 126);
// TODO            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.SubCaption = null;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.TabIndex = 0;
            this.ucoPartnerFind_PersonnelCriteria_CollapsiblePart.Visible = false;

            //
            // spcCriteria
            //
            this.spcCriteria.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.spcCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcCriteria.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcCriteria.Location = new System.Drawing.Point(0, 0);
            this.spcCriteria.Name = "spcCriteria";

            //
            // spcCriteria.Panel1
            //
            this.spcCriteria.Panel1.AutoScroll = true;
            this.spcCriteria.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.spcCriteria.Panel1.Controls.Add(this.pnlLeftColumn);

            //
            // spcCriteria.Panel2
            //
            this.spcCriteria.Panel2.AutoScroll = true;
            this.spcCriteria.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.spcCriteria.Panel2.Controls.Add(this.pnlRightColumn);
            this.spcCriteria.Size = new System.Drawing.Size(655, 213);
            this.spcCriteria.SplitterDistance = 326;
            this.spcCriteria.SplitterIncrement = 5;
            this.spcCriteria.SplitterWidth = 2;
            this.spcCriteria.TabIndex = 4;
            this.spcCriteria.TabStop = false;

            //
            // pnlRightColumn
            //
            this.pnlRightColumn.AutoSize = true;
            this.pnlRightColumn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlRightColumn.BackColor = System.Drawing.Color.Transparent;
            this.pnlRightColumn.Controls.Add(this.pnlLocationKey);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerClass);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerKey);
            this.pnlRightColumn.Controls.Add(this.pnlPartnerStatus);
            this.pnlRightColumn.Controls.Add(this.pnlPersonnelCriteria);
            this.pnlRightColumn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRightColumn.Location = new System.Drawing.Point(0, 0);
            this.pnlRightColumn.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRightColumn.Name = "pnlRightColumn";
            this.pnlRightColumn.Size = new System.Drawing.Size(310, 240);
            this.pnlRightColumn.TabIndex = 2;

            //
            // pnlLocationKey
            //
            this.pnlLocationKey.Controls.Add(this.btnLocationKey);
            this.pnlLocationKey.Controls.Add(this.txtLocationKey);
            this.pnlLocationKey.Location = new System.Drawing.Point(0, 76);
            this.pnlLocationKey.Name = "pnlLocationKey";
            this.pnlLocationKey.Size = new System.Drawing.Size(304, 24);
            this.pnlLocationKey.TabIndex = 7;

            //
            // btnLocationKey
            //
            this.btnLocationKey.BackColor = System.Drawing.SystemColors.Control;
            this.btnLocationKey.Location = new System.Drawing.Point(44, 0);
            this.btnLocationKey.Name = "btnLocationKey";
            this.btnLocationKey.Size = new System.Drawing.Size(98, 23);
            this.btnLocationKey.TabIndex = 1;
            this.btnLocationKey.Text = "Location Key";
            this.btnLocationKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLocationKey.UseVisualStyleBackColor = false;
            this.btnLocationKey.Click += new System.EventHandler(this.BtnLocationKey_Click);

            //
            // txtLocationKey
            //
            this.txtLocationKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "LocationKey", true));
            this.txtLocationKey.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.txtLocationKey.Location = new System.Drawing.Point(146, 0);
            this.txtLocationKey.Name = "txtLocationKey";
            this.txtLocationKey.Size = new System.Drawing.Size(76, 21);
            this.txtLocationKey.TabIndex = 2;
            this.txtLocationKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLocationKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtLocationKey_KeyUp);

            //
            // pnlPartnerClass
            //
            this.pnlPartnerClass.Controls.Add(this.chkWorkerFamOnly);
            this.pnlPartnerClass.Controls.Add(this.cmbPartnerClass);
            this.pnlPartnerClass.Controls.Add(this.lblPartnerClass);
            this.pnlPartnerClass.Location = new System.Drawing.Point(0, 176);
            this.pnlPartnerClass.Name = "pnlPartnerClass";
            this.pnlPartnerClass.Size = new System.Drawing.Size(304, 40);
            this.pnlPartnerClass.TabIndex = 2;

            //
            // chkWorkerFamOnly
            //
            this.chkWorkerFamOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.FFindCriteriaDataTable, "WorkerFamOnly", true));
            this.chkWorkerFamOnly.Location = new System.Drawing.Point(150, 22);
            this.chkWorkerFamOnly.Name = "chkWorkerFamOnly";
            this.chkWorkerFamOnly.Size = new System.Drawing.Size(126, 17);
            this.chkWorkerFamOnly.TabIndex = 2;
            this.chkWorkerFamOnly.Text = "Worker Families O&nly";

            //
            // cmbPartnerClass
            //
            this.cmbPartnerClass.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PartnerClass", true));
            this.cmbPartnerClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartnerClass.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.cmbPartnerClass.Location = new System.Drawing.Point(146, 0);
            this.cmbPartnerClass.MaxDropDownItems = 9;
            this.cmbPartnerClass.Name = "cmbPartnerClass";
            this.cmbPartnerClass.Size = new System.Drawing.Size(124, 21);
            this.cmbPartnerClass.TabIndex = 1;
            this.cmbPartnerClass.SelectedValueChanged += new System.EventHandler(this.CmbPartnerClass_SelectedValueChanged);

            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerClass.TabIndex = 0;
            this.lblPartnerClass.Text = "Partner C&lass:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPartnerKey
            //
            this.pnlPartnerKey.Controls.Add(this.txtPartnerKey);
            this.pnlPartnerKey.Controls.Add(this.lblPartnerKey);
            this.pnlPartnerKey.Controls.Add(this.lblPartnerKeyNonExactMatch);
            this.pnlPartnerKey.Location = new System.Drawing.Point(0, 26);
            this.pnlPartnerKey.Name = "pnlPartnerKey";
            this.pnlPartnerKey.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerKey.TabIndex = 2;

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.ControlMode = Ict.Common.Controls.TMaskedTextBoxMode.PartnerKey;
            this.txtPartnerKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PartnerKey", true));
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.Location = new System.Drawing.Point(146, 0);
            this.txtPartnerKey.Mask = "##########";
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PlaceHolder = "0";
            this.txtPartnerKey.Size = new System.Drawing.Size(76, 20);
            this.txtPartnerKey.TabIndex = 1;
            this.txtPartnerKey.Text = "0000000000";
            this.txtPartnerKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPartnerKey_KeyUp);

            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerKey.TabIndex = 0;
            this.lblPartnerKey.Text = "Partner &Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerKeyNonExactMatch
            //
            this.lblPartnerKeyNonExactMatch.Font = new System.Drawing.Font("Verdana", 6.5F);
            this.lblPartnerKeyNonExactMatch.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPartnerKeyNonExactMatch.Location = new System.Drawing.Point(225, 2);
            this.lblPartnerKeyNonExactMatch.Name = "lblPartnerKeyNonExactMatch";
            this.lblPartnerKeyNonExactMatch.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerKeyNonExactMatch.TabIndex = 0;
            this.lblPartnerKeyNonExactMatch.Text = "(trailing 0 = --*)";
            this.lblPartnerKeyNonExactMatch.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblPartnerKeyNonExactMatch.Visible = false;
            this.tipUC.SetToolTip(this.lblPartnerKeyNonExactMatch, StrPartnerKeyNonExactInfoTest);

            //
            // pnlPartnerStatus
            //
            this.pnlPartnerStatus.Controls.Add(this.rbtPrivate);
            this.pnlPartnerStatus.Controls.Add(this.rbtStatusActive);
            this.pnlPartnerStatus.Controls.Add(this.rbtStatusAll);
            this.pnlPartnerStatus.Controls.Add(this.lblPartnerStatus);
            this.pnlPartnerStatus.Location = new System.Drawing.Point(0, 108);
            this.pnlPartnerStatus.Name = "pnlPartnerStatus";
            this.pnlPartnerStatus.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerStatus.TabIndex = 2;

            //
            // rbtPrivate
            //
            this.rbtPrivate.BackColor = System.Drawing.SystemColors.Control;
            this.rbtPrivate.Location = new System.Drawing.Point(245, 0);
            this.rbtPrivate.Name = "rbtPrivate";
            this.rbtPrivate.Size = new System.Drawing.Size(66, 24);
            this.rbtPrivate.TabIndex = 2;
            this.rbtPrivate.TabStop = true;
            this.rbtPrivate.Tag = "PRIVATE";
            this.rbtPrivate.Text = "Private";
            this.rbtPrivate.UseVisualStyleBackColor = false;
            this.rbtPrivate.Click += new System.EventHandler(this.RbtPrivate_Click);

            //
            // rbtStatusActive
            //
            this.rbtStatusActive.BackColor = System.Drawing.SystemColors.Control;
            this.rbtStatusActive.Checked = true;
            this.rbtStatusActive.Location = new System.Drawing.Point(146, 0);
            this.rbtStatusActive.Name = "rbtStatusActive";
            this.rbtStatusActive.Size = new System.Drawing.Size(60, 24);
            this.rbtStatusActive.TabIndex = 1;
            this.rbtStatusActive.TabStop = true;
            this.rbtStatusActive.Tag = "ACTIVE";
            this.rbtStatusActive.Text = "Acti&ve";
            this.rbtStatusActive.UseVisualStyleBackColor = false;
            this.rbtStatusActive.Click += new System.EventHandler(this.RbtStatusActive_Click);

            //
            // rbtStatusAll
            //
            this.rbtStatusAll.BackColor = System.Drawing.SystemColors.Control;
            this.rbtStatusAll.Location = new System.Drawing.Point(206, 0);
            this.rbtStatusAll.Name = "rbtStatusAll";
            this.rbtStatusAll.Size = new System.Drawing.Size(39, 24);
            this.rbtStatusAll.TabIndex = 1;
            this.rbtStatusAll.Tag = "ALL";
            this.rbtStatusAll.Text = "All";
            this.rbtStatusAll.UseVisualStyleBackColor = false;
            this.rbtStatusAll.Click += new System.EventHandler(this.RbtStatusAll_Click);

            //
            // lblPartnerStatus
            //
            this.lblPartnerStatus.Location = new System.Drawing.Point(2, 4);
            this.lblPartnerStatus.Name = "lblPartnerStatus";
            this.lblPartnerStatus.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerStatus.TabIndex = 0;
            this.lblPartnerStatus.Text = "Status:";
            this.lblPartnerStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPersonnelCriteria
            //
            this.pnlPersonnelCriteria.Location = new System.Drawing.Point(2, 138);
            this.pnlPersonnelCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPersonnelCriteria.Name = "pnlPersonnelCriteria";
            this.pnlPersonnelCriteria.Size = new System.Drawing.Size(308, 102);
            this.pnlPersonnelCriteria.TabIndex = 2;

            //
            // pnlLeftColumn
            //
            this.pnlLeftColumn.AutoSize = true;
            this.pnlLeftColumn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLeftColumn.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeftColumn.Controls.Add(this.pnlPhoneNumber);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress3);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress2);
            this.pnlLeftColumn.Controls.Add(this.pnlEmail);
            this.pnlLeftColumn.Controls.Add(this.pnlPartnerName);
            this.pnlLeftColumn.Controls.Add(this.pnlPersonalName);
            this.pnlLeftColumn.Controls.Add(this.pnlPreviousName);
            this.pnlLeftColumn.Controls.Add(this.pnlAddress1);
            this.pnlLeftColumn.Controls.Add(this.pnlPostCode);
            this.pnlLeftColumn.Controls.Add(this.pnlCity);
            this.pnlLeftColumn.Controls.Add(this.pnlCounty);
            this.pnlLeftColumn.Controls.Add(this.pnlCountry);
            this.pnlLeftColumn.Controls.Add(this.pnlMailingAddressOnly);
            this.pnlLeftColumn.Controls.Add(this.pnlAccountName);
            this.pnlLeftColumn.Controls.Add(this.pnlAccountNumber);
            this.pnlLeftColumn.Controls.Add(this.pnlIban);
            this.pnlLeftColumn.Controls.Add(this.pnlBic);
            this.pnlLeftColumn.Controls.Add(this.pnlBranchCode);
            this.pnlLeftColumn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeftColumn.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftColumn.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeftColumn.Name = "pnlLeftColumn";
            this.pnlLeftColumn.Size = new System.Drawing.Size(309, 294);
            this.pnlLeftColumn.TabIndex = 1;

            //
            // pnlPhoneNumber
            //
            this.pnlPhoneNumber.BackColor = System.Drawing.Color.Transparent;
            this.pnlPhoneNumber.Controls.Add(this.critPhoneNumber);
            this.pnlPhoneNumber.Controls.Add(this.lblPhoneNumber);
            this.pnlPhoneNumber.Controls.Add(this.txtPhoneNumber);
            this.pnlPhoneNumber.Location = new System.Drawing.Point(2, 124);
            this.pnlPhoneNumber.Name = "pnlPhoneNumber";
            this.pnlPhoneNumber.Size = new System.Drawing.Size(304, 21);
            this.pnlPhoneNumber.TabIndex = 6;

            //
            // critPhoneNumber
            //
            this.critPhoneNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPhoneNumber.BackColor = System.Drawing.SystemColors.Control;
            this.critPhoneNumber.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPhoneNumber.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PhoneNumberMatch",
                    true));
            this.critPhoneNumber.Location = new System.Drawing.Point(260, 2);
            this.critPhoneNumber.Name = "critPhoneNumber";
            this.critPhoneNumber.SelectedValue = "BEGINS";
            this.critPhoneNumber.Size = new System.Drawing.Size(41, 18);
            this.critPhoneNumber.TabIndex = 4;
            this.critPhoneNumber.TabStop = false;

            //
            // lblPhoneNumber
            //
            this.lblPhoneNumber.Location = new System.Drawing.Point(2, 2);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(142, 23);
            this.lblPhoneNumber.TabIndex = 0;
            this.lblPhoneNumber.Text = "Phone Number:";
            this.lblPhoneNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtPhoneNumber
            //
            this.txtPhoneNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhoneNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PhoneNumber", true));
            this.txtPhoneNumber.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(146, 0);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(112, 21);
            this.txtPhoneNumber.TabIndex = 1;
            this.txtPhoneNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPhoneNumber_KeyUp);
            this.txtPhoneNumber.Leave += new EventHandler(this.TxtPhoneNumber_Leave);

            //
            // pnlAddress3
            //
            this.pnlAddress3.Controls.Add(this.critAddress3);
            this.pnlAddress3.Controls.Add(this.txtAddress3);
            this.pnlAddress3.Controls.Add(this.lblAddress3);
            this.pnlAddress3.Location = new System.Drawing.Point(2, 124);
            this.pnlAddress3.Name = "pnlAddress3";
            this.pnlAddress3.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress3.TabIndex = 5;
            this.pnlAddress3.Tag = "";

            //
            // critAddress3
            //
            this.critAddress3.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress3.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress3.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress3.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address3Match", true));
            this.critAddress3.Location = new System.Drawing.Point(260, 2);
            this.critAddress3.Name = "critAddress3";
            this.critAddress3.SelectedValue = "BEGINS";
            this.critAddress3.Size = new System.Drawing.Size(41, 18);
            this.critAddress3.TabIndex = 4;
            this.critAddress3.TabStop = false;

            //
            // txtAddress3
            //
            this.txtAddress3.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address3", true));
            this.txtAddress3.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress3.Location = new System.Drawing.Point(146, 0);
            this.txtAddress3.Name = "txtAddress3";
            this.txtAddress3.Size = new System.Drawing.Size(112, 21);
            this.txtAddress3.TabIndex = 1;
            this.txtAddress3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress3_KeyUp);
            this.txtAddress3.Leave += new EventHandler(this.TxtAddress3_Leave);

            //
            // lblAddress3
            //
            this.lblAddress3.Location = new System.Drawing.Point(2, 2);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(142, 23);
            this.lblAddress3.TabIndex = 0;
            this.lblAddress3.Text = "Address &3:";
            this.lblAddress3.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAddress2
            //
            this.pnlAddress2.Controls.Add(this.critAddress2);
            this.pnlAddress2.Controls.Add(this.txtAddress2);
            this.pnlAddress2.Controls.Add(this.lblAddress2);
            this.pnlAddress2.Location = new System.Drawing.Point(2, 124);
            this.pnlAddress2.Name = "pnlAddress2";
            this.pnlAddress2.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress2.TabIndex = 4;
            this.pnlAddress2.Tag = "";

            //
            // critAddress2
            //
            this.critAddress2.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress2.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress2.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address2Match", true));
            this.critAddress2.Location = new System.Drawing.Point(260, 2);
            this.critAddress2.Name = "critAddress2";
            this.critAddress2.SelectedValue = "BEGINS";
            this.critAddress2.Size = new System.Drawing.Size(41, 18);
            this.critAddress2.TabIndex = 4;
            this.critAddress2.TabStop = false;

            //
            // txtAddress2
            //
            this.txtAddress2.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address2", true));
            this.txtAddress2.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress2.Location = new System.Drawing.Point(146, 0);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(112, 21);
            this.txtAddress2.TabIndex = 1;
            this.txtAddress2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress2_KeyUp);
            this.txtAddress2.Leave += new EventHandler(this.TxtAddress2_Leave);

            //
            // lblAddress2
            //
            this.lblAddress2.Location = new System.Drawing.Point(2, 2);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(142, 23);
            this.lblAddress2.TabIndex = 0;
            this.lblAddress2.Text = "Address &2:";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlEmail
            //
            this.pnlEmail.Controls.Add(this.critEmail);
            this.pnlEmail.Controls.Add(this.txtEmail);
            this.pnlEmail.Controls.Add(this.lblEmail);
            this.pnlEmail.Location = new System.Drawing.Point(2, 122);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(304, 21);
            this.pnlEmail.TabIndex = 3;

            //
            // critEmail
            //
            this.critEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critEmail.BackColor = System.Drawing.SystemColors.Control;
            this.critEmail.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critEmail.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "EmailMatch", true));
            this.critEmail.Location = new System.Drawing.Point(260, 2);
            this.critEmail.Name = "critEmail";
            this.critEmail.SelectedValue = "BEGINS";
            this.critEmail.Size = new System.Drawing.Size(41, 18);
            this.critEmail.TabIndex = 3;
            this.critEmail.TabStop = false;

            //
            // txtEmail
            //
            this.txtEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Email", true));
            this.txtEmail.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(146, 0);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(112, 21);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtEmail_KeyUp);
            this.txtEmail.Leave += new EventHandler(this.TxtEmail_Leave);

            //
            // lblEmail
            //
            this.lblEmail.Location = new System.Drawing.Point(2, 2);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(142, 22);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "&Email:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPartnerName
            //
            this.pnlPartnerName.Controls.Add(this.critPartnerName);
            this.pnlPartnerName.Controls.Add(this.txtPartnerName);
            this.pnlPartnerName.Controls.Add(this.lblPartnerName);
            this.pnlPartnerName.Location = new System.Drawing.Point(2, 2);
            this.pnlPartnerName.Name = "pnlPartnerName";
            this.pnlPartnerName.Size = new System.Drawing.Size(304, 21);
            this.pnlPartnerName.TabIndex = 2;

            //
            // critPartnerName
            //
            this.critPartnerName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPartnerName.BackColor = System.Drawing.SystemColors.Control;
            this.critPartnerName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PartnerNameMatch",
                    true));
            this.critPartnerName.Location = new System.Drawing.Point(260, 2);
            this.critPartnerName.Name = "critPartnerName";
            this.critPartnerName.SelectedValue = "BEGINS";
            this.critPartnerName.Size = new System.Drawing.Size(41, 18);
            this.critPartnerName.TabIndex = 2;
            this.critPartnerName.TabStop = false;

            //
            // txtPartnerName
            //
            this.txtPartnerName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PartnerName", true));
            this.txtPartnerName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartnerName.Location = new System.Drawing.Point(146, 0);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.Size = new System.Drawing.Size(112, 21);
            this.txtPartnerName.TabIndex = 1;
            this.txtPartnerName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPartnerName_KeyUp);
            this.txtPartnerName.Leave += new EventHandler(this.TxtPartnerName_Leave);

            //
            // lblPartnerName
            //
            this.lblPartnerName.BackColor = System.Drawing.Color.Transparent;
            this.lblPartnerName.Location = new System.Drawing.Point(2, 2);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(142, 23);
            this.lblPartnerName.TabIndex = 0;
            this.lblPartnerName.Text = "Partner &Name:";
            this.lblPartnerName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPersonalName
            //
            this.pnlPersonalName.Controls.Add(this.critPersonalName);
            this.pnlPersonalName.Controls.Add(this.txtPersonalName);
            this.pnlPersonalName.Controls.Add(this.lblPersonalName);
            this.pnlPersonalName.Location = new System.Drawing.Point(2, 28);
            this.pnlPersonalName.Name = "pnlPersonalName";
            this.pnlPersonalName.Size = new System.Drawing.Size(304, 21);
            this.pnlPersonalName.TabIndex = 2;

            //
            // critPersonalName
            //
            this.critPersonalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPersonalName.BackColor = System.Drawing.SystemColors.Control;
            this.critPersonalName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPersonalName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PersonalNameMatch",
                    true));
            this.critPersonalName.Location = new System.Drawing.Point(260, 2);
            this.critPersonalName.Name = "critPersonalName";
            this.critPersonalName.SelectedValue = "BEGINS";
            this.critPersonalName.Size = new System.Drawing.Size(41, 18);
            this.critPersonalName.TabIndex = 3;
            this.critPersonalName.TabStop = false;

            //
            // txtPersonalName
            //
            this.txtPersonalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPersonalName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PersonalName", true));
            this.txtPersonalName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPersonalName.Location = new System.Drawing.Point(146, 0);
            this.txtPersonalName.Name = "txtPersonalName";
            this.txtPersonalName.Size = new System.Drawing.Size(112, 21);
            this.txtPersonalName.TabIndex = 1;
            this.txtPersonalName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPersonalName_KeyUp);
            this.txtPersonalName.Leave += new EventHandler(this.TxtPersonalName_Leave);

            //
            // lblPersonalName
            //
            this.lblPersonalName.Location = new System.Drawing.Point(2, 2);
            this.lblPersonalName.Name = "lblPersonalName";
            this.lblPersonalName.Size = new System.Drawing.Size(142, 22);
            this.lblPersonalName.TabIndex = 0;
            this.lblPersonalName.Text = "Personal &(First) Name:";
            this.lblPersonalName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPreviousName
            //
            this.pnlPreviousName.Controls.Add(this.critPreviousName);
            this.pnlPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreviousName.Location = new System.Drawing.Point(2, 54);
            this.pnlPreviousName.Name = "pnlPreviousName";
            this.pnlPreviousName.Size = new System.Drawing.Size(304, 21);
            this.pnlPreviousName.TabIndex = 2;
            this.pnlPreviousName.Visible = false;

            //
            // critPreviousName
            //
            this.critPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPreviousName.BackColor = System.Drawing.SystemColors.Control;
            this.critPreviousName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPreviousName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PreviousNameMatch",
                    true));
            this.critPreviousName.Location = new System.Drawing.Point(260, 2);
            this.critPreviousName.Name = "critPreviousName";
            this.critPreviousName.SelectedValue = "BEGINS";
            this.critPreviousName.Size = new System.Drawing.Size(41, 18);
            this.critPreviousName.TabIndex = 4;
            this.critPreviousName.TabStop = false;

            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PreviousName", true));
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreviousName.Location = new System.Drawing.Point(146, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(112, 21);
            this.txtPreviousName.TabIndex = 1;
            this.txtPreviousName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPreviousName_KeyUp);
            this.txtPreviousName.Leave += new EventHandler(this.TxtPreviousName_Leave);

            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(2, 2);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(142, 23);
            this.lblPreviousName.TabIndex = 0;
            this.lblPreviousName.Text = "Previous Name:";
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAddress1
            //
            this.pnlAddress1.Controls.Add(this.critAddress1);
            this.pnlAddress1.Controls.Add(this.txtAddress1);
            this.pnlAddress1.Controls.Add(this.lblAddress1);
            this.pnlAddress1.Location = new System.Drawing.Point(2, 78);
            this.pnlAddress1.Name = "pnlAddress1";
            this.pnlAddress1.Size = new System.Drawing.Size(304, 21);
            this.pnlAddress1.TabIndex = 2;
            this.pnlAddress1.Tag = Ict.Common.Controls.Formatting.TSingleLineFlow.BeginGroupIndicator;

            //
            // critAddress1
            //
            this.critAddress1.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAddress1.BackColor = System.Drawing.SystemColors.Control;
            this.critAddress1.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAddress1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "Address1Match", true));
            this.critAddress1.Location = new System.Drawing.Point(260, 2);
            this.critAddress1.Name = "critAddress1";
            this.critAddress1.SelectedValue = "BEGINS";
            this.critAddress1.Size = new System.Drawing.Size(41, 18);
            this.critAddress1.TabIndex = 4;
            this.critAddress1.TabStop = false;

            //
            // txtAddress1
            //
            this.txtAddress1.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Address1", true));
            this.txtAddress1.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress1.Location = new System.Drawing.Point(146, 0);
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(112, 21);
            this.txtAddress1.TabIndex = 1;
            this.txtAddress1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAddress1_KeyUp);
            this.txtAddress1.Leave += new EventHandler(this.TxtAddress1_Leave);

            //
            // lblAddress1
            //
            this.lblAddress1.Location = new System.Drawing.Point(2, 2);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(142, 23);
            this.lblAddress1.TabIndex = 0;
            this.lblAddress1.Text = "Address &1:";
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPostCode
            //
            this.pnlPostCode.BackColor = System.Drawing.Color.Transparent;
            this.pnlPostCode.Controls.Add(this.critPostCode);
            this.pnlPostCode.Controls.Add(this.lblPostCode);
            this.pnlPostCode.Controls.Add(this.txtPostCode);
            this.pnlPostCode.Location = new System.Drawing.Point(2, 197);
            this.pnlPostCode.Name = "pnlPostCode";
            this.pnlPostCode.Size = new System.Drawing.Size(304, 21);
            this.pnlPostCode.TabIndex = 0;

            //
            // critPostCode
            //
            this.critPostCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critPostCode.BackColor = System.Drawing.SystemColors.Control;
            this.critPostCode.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critPostCode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "PostCodeMatch", true));
            this.critPostCode.Location = new System.Drawing.Point(260, 2);
            this.critPostCode.Name = "critPostCode";
            this.critPostCode.SelectedValue = "BEGINS";
            this.critPostCode.Size = new System.Drawing.Size(41, 18);
            this.critPostCode.TabIndex = 4;
            this.critPostCode.TabStop = false;

            //
            // lblPostCode
            //
            this.lblPostCode.Location = new System.Drawing.Point(2, 2);
            this.lblPostCode.Name = "lblPostCode";
            this.lblPostCode.Size = new System.Drawing.Size(142, 23);
            this.lblPostCode.TabIndex = 0;
            this.lblPostCode.Text = "P&ost Code:";
            this.lblPostCode.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtPostCode
            //
            this.txtPostCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "PostCode", true));
            this.txtPostCode.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostCode.Location = new System.Drawing.Point(146, 0);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(112, 21);
            this.txtPostCode.TabIndex = 1;
            this.txtPostCode.Leave += new System.EventHandler(this.TxtPostCode_Leave);
            this.txtPostCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtPostCode_KeyUp);

            //
            // pnlCity
            //
            this.pnlCity.Controls.Add(this.critCity);
            this.pnlCity.Controls.Add(this.lblCity);
            this.pnlCity.Controls.Add(this.txtCity);
            this.pnlCity.Location = new System.Drawing.Point(2, 100);
            this.pnlCity.Name = "pnlCity";
            this.pnlCity.Size = new System.Drawing.Size(304, 21);
            this.pnlCity.TabIndex = 0;

            //
            // critCity
            //
            this.critCity.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critCity.BackColor = System.Drawing.SystemColors.Control;
            this.critCity.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critCity.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "CityMatch", true));
            this.critCity.Location = new System.Drawing.Point(260, 2);
            this.critCity.Name = "critCity";
            this.critCity.SelectedValue = "BEGINS";
            this.critCity.Size = new System.Drawing.Size(41, 18);
            this.critCity.TabIndex = 4;
            this.critCity.TabStop = false;

            //
            // lblCity
            //
            this.lblCity.Location = new System.Drawing.Point(2, 2);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(142, 23);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "Cit&y/Town:";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtCity
            //
            this.txtCity.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "City", true));
            this.txtCity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.Location = new System.Drawing.Point(146, 0);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(112, 21);
            this.txtCity.TabIndex = 1;
            this.txtCity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtCity_KeyUp);
            this.txtCity.Leave += new EventHandler(this.TxtCity_Leave);

            //
            // pnlCounty
            //
            this.pnlCounty.Controls.Add(this.critCounty);
            this.pnlCounty.Controls.Add(this.txtCounty);
            this.pnlCounty.Controls.Add(this.lblCounty);
            this.pnlCounty.Location = new System.Drawing.Point(2, 174);
            this.pnlCounty.Name = "pnlCounty";
            this.pnlCounty.Size = new System.Drawing.Size(304, 21);
            this.pnlCounty.TabIndex = 2;
            this.pnlCounty.Visible = false;

            //
            // critCounty
            //
            this.critCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critCounty.BackColor = System.Drawing.SystemColors.Control;
            this.critCounty.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critCounty.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "CountyMatch", true));
            this.critCounty.Location = new System.Drawing.Point(260, 2);
            this.critCounty.Name = "critCounty";
            this.critCounty.SelectedValue = "BEGINS";
            this.critCounty.Size = new System.Drawing.Size(41, 18);
            this.critCounty.TabIndex = 4;
            this.critCounty.TabStop = false;

            //
            // txtCounty
            //
            this.txtCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtCounty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "County", true));
            this.txtCounty.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCounty.Location = new System.Drawing.Point(146, 0);
            this.txtCounty.Name = "txtCounty";
            this.txtCounty.Size = new System.Drawing.Size(112, 21);
            this.txtCounty.TabIndex = 2;
            this.txtCounty.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtCounty_KeyUp);
            this.txtCounty.Leave += new EventHandler(this.TxtCounty_Leave);

            //
            // lblCounty
            //
            this.lblCounty.Location = new System.Drawing.Point(2, 2);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(142, 23);
            this.lblCounty.TabIndex = 0;
            this.lblCounty.Text = "Co&unty:";
            this.lblCounty.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlCountry
            //
            this.pnlCountry.BackColor = System.Drawing.Color.Transparent;
            this.pnlCountry.Controls.Add(this.ucoCountryComboBox);
            this.pnlCountry.Controls.Add(this.lblCountry);
            this.pnlCountry.Location = new System.Drawing.Point(2, 226);
            this.pnlCountry.Name = "pnlCountry";
            this.pnlCountry.Size = new System.Drawing.Size(304, 22);
            this.pnlCountry.TabIndex = 2;

            //
            // ucoCountryComboBox
            //
            this.ucoCountryComboBox.Location = new System.Drawing.Point(146, 0);
            this.ucoCountryComboBox.Name = "ucoCountryComboBox";
            this.ucoCountryComboBox.SelectedValue = null;
            this.ucoCountryComboBox.Size = new System.Drawing.Size(154, 22);
            this.ucoCountryComboBox.TabIndex = 2;

            //
            // lblCountry
            //
            this.lblCountry.Location = new System.Drawing.Point(2, 2);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(142, 23);
            this.lblCountry.TabIndex = 0;
            this.lblCountry.Text = "Co&untry:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlMailingAddressOnly
            //
            this.pnlMailingAddressOnly.Controls.Add(this.chkMailingAddressOnly);
            this.pnlMailingAddressOnly.Controls.Add(this.lblMailingAddressOnly);
            this.pnlMailingAddressOnly.Location = new System.Drawing.Point(2, 270);
            this.pnlMailingAddressOnly.Name = "pnlMailingAddressOnly";
            this.pnlMailingAddressOnly.Size = new System.Drawing.Size(304, 21);
            this.pnlMailingAddressOnly.TabIndex = 2;
            this.pnlMailingAddressOnly.Tag = Ict.Common.Controls.Formatting.TSingleLineFlow.BeginGroupIndicator;

            //
            // chkMailingAddressOnly
            //
            this.chkMailingAddressOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.FFindCriteriaDataTable, "MailingAddressOnly",
                    true));
            this.chkMailingAddressOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkMailingAddressOnly.Location = new System.Drawing.Point(146, 4);
            this.chkMailingAddressOnly.Name = "chkMailingAddressOnly";
            this.chkMailingAddressOnly.Size = new System.Drawing.Size(12, 12);
            this.chkMailingAddressOnly.TabIndex = 1;

            //
            // lblMailingAddressOnly
            //
            this.lblMailingAddressOnly.Location = new System.Drawing.Point(0, 2);
            this.lblMailingAddressOnly.Name = "lblMailingAddressOnly";
            this.lblMailingAddressOnly.Size = new System.Drawing.Size(144, 23);
            this.lblMailingAddressOnly.TabIndex = 0;
            this.lblMailingAddressOnly.Text = "Mailin&g Addresses Only:";
            this.lblMailingAddressOnly.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAccountName
            //
            this.pnlAccountName.Controls.Add(this.critAccountName);
            this.pnlAccountName.Controls.Add(this.txtAccountName);
            this.pnlAccountName.Controls.Add(this.lblAccountName);
            this.pnlAccountName.Location = new System.Drawing.Point(2, 54);
            this.pnlAccountName.Name = "pnlAccountName";
            this.pnlAccountName.Size = new System.Drawing.Size(304, 21);
            this.pnlAccountName.TabIndex = 2;
            this.pnlAccountName.Visible = false;

            //
            // critAccountName
            //
            this.critAccountName.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAccountName.BackColor = System.Drawing.SystemColors.Control;
            this.critAccountName.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAccountName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "AccountNameMatch",
                    true));
            this.critAccountName.Location = new System.Drawing.Point(260, 2);
            this.critAccountName.Name = "critAccountName";
            this.critAccountName.SelectedValue = "BEGINS";
            this.critAccountName.Size = new System.Drawing.Size(41, 18);
            this.critAccountName.TabIndex = 4;
            this.critAccountName.TabStop = false;

            //
            // txtAccountName
            //
            this.txtAccountName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccountName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "AccountName", true));
            this.txtAccountName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountName.Location = new System.Drawing.Point(146, 0);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(112, 21);
            this.txtAccountName.TabIndex = 1;
            this.txtAccountName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAccountName_KeyUp);
            this.txtAccountName.Leave += new EventHandler(this.TxtAccountName_Leave);

            //
            // lblAccountName
            //
            this.lblAccountName.Location = new System.Drawing.Point(2, 2);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(142, 23);
            this.lblAccountName.TabIndex = 0;
            this.lblAccountName.Text = "Account Name:";
            this.lblAccountName.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlAccountNumber
            //
            this.pnlAccountNumber.Controls.Add(this.critAccountNumber);
            this.pnlAccountNumber.Controls.Add(this.txtAccountNumber);
            this.pnlAccountNumber.Controls.Add(this.lblAccountNumber);
            this.pnlAccountNumber.Location = new System.Drawing.Point(2, 54);
            this.pnlAccountNumber.Name = "pnlAccountNumber";
            this.pnlAccountNumber.Size = new System.Drawing.Size(304, 21);
            this.pnlAccountNumber.TabIndex = 2;
            this.pnlAccountNumber.Visible = false;

            //
            // critAccountNumber
            //
            this.critAccountNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critAccountNumber.BackColor = System.Drawing.SystemColors.Control;
            this.critAccountNumber.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable,
                    "AccountNumberMatch",
                    true));
            this.critAccountNumber.Location = new System.Drawing.Point(260, 2);
            this.critAccountNumber.Name = "critAccountNumber";
            this.critAccountNumber.SelectedValue = "BEGINS";
            this.critAccountNumber.Size = new System.Drawing.Size(41, 18);
            this.critAccountNumber.TabIndex = 4;
            this.critAccountNumber.TabStop = false;

            //
            // txtAccountNumber
            //
            this.txtAccountNumber.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "AccountNumber", true));
            this.txtAccountNumber.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountNumber.Location = new System.Drawing.Point(146, 0);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(112, 21);
            this.txtAccountNumber.TabIndex = 1;
            this.txtAccountNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtAccountNumber_KeyUp);
            this.txtAccountNumber.Leave += new EventHandler(this.TxtAccountNumber_Leave);

            //
            // lblAccountNumber
            //
            this.lblAccountNumber.Location = new System.Drawing.Point(2, 2);
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Size = new System.Drawing.Size(142, 23);
            this.lblAccountNumber.TabIndex = 0;
            this.lblAccountNumber.Text = "Account Number:";
            this.lblAccountNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlIban
            //
            this.pnlIban.Controls.Add(this.critIban);
            this.pnlIban.Controls.Add(this.txtIban);
            this.pnlIban.Controls.Add(this.lblIban);
            this.pnlIban.Location = new System.Drawing.Point(2, 78);
            this.pnlIban.Name = "pnlIban";
            this.pnlIban.Size = new System.Drawing.Size(304, 21);
            this.pnlIban.TabIndex = 2;
            this.pnlIban.Tag = Ict.Common.Controls.Formatting.TSingleLineFlow.BeginGroupIndicator;

            //
            // critIban
            //
            this.critIban.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critIban.BackColor = System.Drawing.SystemColors.Control;
            this.critIban.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critIban.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "IbanMatch", true));
            this.critIban.Location = new System.Drawing.Point(260, 2);
            this.critIban.Name = "critIban";
            this.critIban.SelectedValue = "BEGINS";
            this.critIban.Size = new System.Drawing.Size(41, 18);
            this.critIban.TabIndex = 4;
            this.critIban.TabStop = false;

            //
            // txtIban
            //
            this.txtIban.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtIban.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Iban", true));
            this.txtIban.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIban.Location = new System.Drawing.Point(146, 0);
            this.txtIban.Name = "txtIban";
            this.txtIban.Size = new System.Drawing.Size(112, 21);
            this.txtIban.TabIndex = 1;
            this.txtIban.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtIban_KeyUp);
            this.txtIban.Leave += new EventHandler(this.TxtIban_Leave);

            //
            // lblIban
            //
            this.lblIban.Location = new System.Drawing.Point(2, 2);
            this.lblIban.Name = "lblIban";
            this.lblIban.Size = new System.Drawing.Size(142, 23);
            this.lblIban.TabIndex = 0;
            this.lblIban.Text = "IBAN:";
            this.lblIban.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlBic
            //
            this.pnlBic.BackColor = System.Drawing.Color.Transparent;
            this.pnlBic.Controls.Add(this.critBic);
            this.pnlBic.Controls.Add(this.lblBic);
            this.pnlBic.Controls.Add(this.txtBic);
            this.pnlBic.Location = new System.Drawing.Point(2, 197);
            this.pnlBic.Name = "pnlBic";
            this.pnlBic.Size = new System.Drawing.Size(304, 21);
            this.pnlBic.TabIndex = 0;

            //
            // critBic
            //
            this.critBic.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critBic.BackColor = System.Drawing.SystemColors.Control;
            this.critBic.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critBic.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "BicMatch", true));
            this.critBic.Location = new System.Drawing.Point(260, 2);
            this.critBic.Name = "critBic";
            this.critBic.SelectedValue = "BEGINS";
            this.critBic.Size = new System.Drawing.Size(41, 18);
            this.critBic.TabIndex = 4;
            this.critBic.TabStop = false;

            //
            // lblBic
            //
            this.lblBic.Location = new System.Drawing.Point(2, 2);
            this.lblBic.Name = "lblBic";
            this.lblBic.Size = new System.Drawing.Size(142, 23);
            this.lblBic.TabIndex = 0;
            this.lblBic.Text = "BIC:";
            this.lblBic.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtBic
            //
            this.txtBic.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtBic.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "Bic", true));
            this.txtBic.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBic.Location = new System.Drawing.Point(146, 0);
            this.txtBic.Name = "txtBic";
            this.txtBic.Size = new System.Drawing.Size(112, 21);
            this.txtBic.TabIndex = 1;
            this.txtBic.Leave += new System.EventHandler(this.TxtBic_Leave);
            this.txtBic.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtBic_KeyUp);

            //
            // pnlBranchCode
            //
            this.pnlBranchCode.Controls.Add(this.critBranchCode);
            this.pnlBranchCode.Controls.Add(this.lblBranchCode);
            this.pnlBranchCode.Controls.Add(this.txtBranchCode);
            this.pnlBranchCode.Location = new System.Drawing.Point(2, 100);
            this.pnlBranchCode.Name = "pnlBranchCode";
            this.pnlBranchCode.Size = new System.Drawing.Size(304, 21);
            this.pnlBranchCode.TabIndex = 0;

            //
            // critBranchCode
            //
            this.critBranchCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.critBranchCode.BackColor = System.Drawing.SystemColors.Control;
            this.critBranchCode.ControlMode = Ict.Petra.Client.CommonControls.TControlMode.Matches;
            this.critBranchCode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.FFindCriteriaDataTable, "BranchCodeMatch",
                    true));
            this.critBranchCode.Location = new System.Drawing.Point(260, 2);
            this.critBranchCode.Name = "critBranchCode";
            this.critBranchCode.SelectedValue = "BEGINS";
            this.critBranchCode.Size = new System.Drawing.Size(41, 18);
            this.critBranchCode.TabIndex = 4;
            this.critBranchCode.TabStop = false;

            //
            // lblBranchCode
            //
            this.lblBranchCode.Location = new System.Drawing.Point(2, 2);
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Size = new System.Drawing.Size(142, 23);
            this.lblBranchCode.TabIndex = 0;
            this.lblBranchCode.Text = "Branch/Bank Code:";
            this.lblBranchCode.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtBranchCode
            //
            this.txtBranchCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtBranchCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FFindCriteriaDataTable, "BranchCode", true));
            this.txtBranchCode.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBranchCode.Location = new System.Drawing.Point(146, 0);
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Size = new System.Drawing.Size(112, 21);
            this.txtBranchCode.TabIndex = 1;
            this.txtBranchCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtBranchCode_KeyUp);
            this.txtBranchCode.Leave += new EventHandler(this.TxtBranchCode_Leave);

            //
            // TUC_PartnerFindCriteria
            //
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.spcCriteria);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUC_PartnerFindCriteria";
            this.Size = new System.Drawing.Size(655, 213);
            this.spcCriteria.Panel1.ResumeLayout(false);
            this.spcCriteria.Panel1.PerformLayout();
            this.spcCriteria.Panel2.ResumeLayout(false);
            this.spcCriteria.Panel2.PerformLayout();
            this.spcCriteria.ResumeLayout(false);
            this.pnlRightColumn.ResumeLayout(false);
            this.pnlLocationKey.ResumeLayout(false);
            this.pnlLocationKey.PerformLayout();
            this.pnlPartnerClass.ResumeLayout(false);
            this.pnlPartnerKey.ResumeLayout(false);
            this.pnlPartnerKey.PerformLayout();
            this.pnlPartnerStatus.ResumeLayout(false);
            this.pnlLeftColumn.ResumeLayout(false);
            this.pnlPhoneNumber.ResumeLayout(false);
            this.pnlPhoneNumber.PerformLayout();
            this.pnlAddress3.ResumeLayout(false);
            this.pnlAddress3.PerformLayout();
            this.pnlAddress2.ResumeLayout(false);
            this.pnlAddress2.PerformLayout();
            this.pnlEmail.ResumeLayout(false);
            this.pnlEmail.PerformLayout();
            this.pnlPartnerName.ResumeLayout(false);
            this.pnlPartnerName.PerformLayout();
            this.pnlPersonalName.ResumeLayout(false);
            this.pnlPersonalName.PerformLayout();
            this.pnlPreviousName.ResumeLayout(false);
            this.pnlPreviousName.PerformLayout();
            this.pnlAddress1.ResumeLayout(false);
            this.pnlAddress1.PerformLayout();
            this.pnlPostCode.ResumeLayout(false);
            this.pnlPostCode.PerformLayout();
            this.pnlCity.ResumeLayout(false);
            this.pnlCity.PerformLayout();
            this.pnlCounty.ResumeLayout(false);
            this.pnlCounty.PerformLayout();
            this.pnlCountry.ResumeLayout(false);
            this.pnlMailingAddressOnly.ResumeLayout(false);
            this.pnlAccountName.ResumeLayout(false);
            this.pnlAccountName.PerformLayout();
            this.pnlAccountNumber.ResumeLayout(false);
            this.pnlAccountNumber.PerformLayout();
            this.pnlIban.ResumeLayout(false);
            this.pnlIban.PerformLayout();
            this.pnlBic.ResumeLayout(false);
            this.pnlBic.PerformLayout();
            this.pnlBranchCode.ResumeLayout(false);
            this.pnlBranchCode.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlLeftColumn;
        private System.Windows.Forms.Panel pnlRightColumn;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.Panel pnlPartnerName;
        private System.Windows.Forms.Panel pnlPersonalName;
        private System.Windows.Forms.TextBox txtPersonalName;
        private System.Windows.Forms.Label lblPersonalName;
        private System.Windows.Forms.Panel pnlPartnerClass;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.ComboBox cmbPartnerClass;
        private System.Windows.Forms.Panel pnlPartnerKey;
        private TTxtMaskedTextBox txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblPartnerKeyNonExactMatch;
        private System.Windows.Forms.Panel pnlPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.Panel pnlCounty;
        private System.Windows.Forms.Label lblCounty;
        private System.Windows.Forms.Panel pnlAddress1;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Panel pnlCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Panel pnlCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Panel pnlPostCode;
        private System.Windows.Forms.Label lblPostCode;
        private System.Windows.Forms.TextBox txtPostCode;
        private System.Windows.Forms.Panel pnlPartnerStatus;
        private System.Windows.Forms.Label lblPartnerStatus;
        private System.Windows.Forms.RadioButton rbtStatusActive;
        private System.Windows.Forms.RadioButton rbtStatusAll;
        private System.Windows.Forms.Panel pnlMailingAddressOnly;
        private System.Windows.Forms.Label lblMailingAddressOnly;
        private System.Windows.Forms.CheckBox chkMailingAddressOnly;
        private System.Windows.Forms.Panel pnlPersonnelCriteria;
        private TUC_PartnerFind_PersonnelCriteria_CollapsiblePart ucoPartnerFind_PersonnelCriteria_CollapsiblePart;
        private System.Windows.Forms.TextBox txtCounty;
        private TUC_CountryComboBox ucoCountryComboBox;
        private SplitButton critPartnerName;
        private SplitButton critPersonalName;
        private SplitButton critPreviousName;
        private SplitButton critAddress1;
        private SplitButton critCity;
        private SplitButton critPostCode;
        private SplitButton critCounty;
        private System.Windows.Forms.Panel pnlEmail;
        private SplitButton critEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel pnlAddress2;
        private SplitButton critAddress2;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Panel pnlAddress3;
        private SplitButton critAddress3;
        private System.Windows.Forms.TextBox txtAddress3;
        private System.Windows.Forms.Label lblAddress3;
        private System.Windows.Forms.Panel pnlLocationKey;
        private System.Windows.Forms.TextBox txtLocationKey;
        private System.Windows.Forms.CheckBox chkWorkerFamOnly;
        private System.Windows.Forms.Panel pnlPhoneNumber;
        private SplitButton critPhoneNumber;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Button btnLocationKey;
        private System.Windows.Forms.RadioButton rbtPrivate;
        private System.Windows.Forms.ToolTip tipUC;
        private System.Windows.Forms.SplitContainer spcCriteria;
        private System.Windows.Forms.Panel pnlAccountName;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.Label lblAccountName;
        private System.Windows.Forms.Panel pnlAccountNumber;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Label lblAccountNumber;
        private System.Windows.Forms.Panel pnlIban;
        private System.Windows.Forms.TextBox txtIban;
        private System.Windows.Forms.Label lblIban;
        private System.Windows.Forms.Panel pnlBranchCode;
        private System.Windows.Forms.TextBox txtBranchCode;
        private System.Windows.Forms.Label lblBranchCode;
        private System.Windows.Forms.Panel pnlBic;
        private System.Windows.Forms.Label lblBic;
        private System.Windows.Forms.TextBox txtBic;
        private SplitButton critAccountName;
        private SplitButton critAccountNumber;
        private SplitButton critIban;
        private SplitButton critBranchCode;
        private SplitButton critBic;
    }
}