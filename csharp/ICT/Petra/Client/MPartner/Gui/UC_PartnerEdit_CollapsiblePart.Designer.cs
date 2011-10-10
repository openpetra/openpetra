//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
    partial class TUC_PartnerEdit_CollapsiblePart
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
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_CollapsiblePart));
            this.components = new System.ComponentModel.Container();
            this.txtWorkerField = new System.Windows.Forms.TextBox();
            this.lblStatusUpdated = new System.Windows.Forms.Label();
            this.txtStatusChange = new System.Windows.Forms.TextBox();
            this.lblPersonGender = new System.Windows.Forms.Label();
            this.lblTitleNamePerson = new System.Windows.Forms.Label();
            this.lblPartnerStatus = new System.Windows.Forms.Label();
            this.txtPersonTitle = new System.Windows.Forms.TextBox();
            this.txtPersonFirstName = new System.Windows.Forms.TextBox();
            this.txtPersonMiddleName = new System.Windows.Forms.TextBox();
            this.txtPersonFamilyName = new System.Windows.Forms.TextBox();
            this.lblAddresseeType = new System.Windows.Forms.Label();
            this.chkNoSolicitations = new System.Windows.Forms.CheckBox();
            this.txtPartnerClass = new System.Windows.Forms.TextBox();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtMaskedTextBox(this.components);
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.pnlPartner = new System.Windows.Forms.Panel();
            this.cmbAddresseeType = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlPerson = new System.Windows.Forms.Panel();
            this.lblPersonPanel = new System.Windows.Forms.Label();
            this.cmbPersonGender = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlFamily = new System.Windows.Forms.Panel();
            this.lblTitleNameFamily = new System.Windows.Forms.Label();
            this.txtFamilyFirstName = new System.Windows.Forms.TextBox();
            this.txtFamilyFamilyName = new System.Windows.Forms.TextBox();
            this.txtFamilyTitle = new System.Windows.Forms.TextBox();
            this.lblFamilyPanel = new System.Windows.Forms.Label();
            this.pnlOther = new System.Windows.Forms.Panel();
            this.lblOtherPanel = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtOtherName = new System.Windows.Forms.TextBox();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnEditWorkerField = new System.Windows.Forms.Button();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.txtLastGiftDate = new System.Windows.Forms.TextBox();
            this.txtLastGiftDetails = new System.Windows.Forms.TextBox();
            this.txtLastContactDate = new System.Windows.Forms.TextBox();
            this.pnlRightSide = new System.Windows.Forms.Panel();
            this.cmbPartnerStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlWorkerField = new System.Windows.Forms.Panel();
            this.btnCreatedOverall = new Ict.Common.Controls.TbtnCreated();
            this.lblLastGiftDate = new System.Windows.Forms.Label();
            this.lblLastContactDate = new System.Windows.Forms.Label();
            this.expStringLengthCheckCollapsiblePart = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlBalloonTipAnchorTabs = new System.Windows.Forms.Panel();
            this.pnlBalloonTipAnchorHelp = new System.Windows.Forms.Panel();
            this.pnlPartner.SuspendLayout();
            this.pnlPerson.SuspendLayout();
            this.pnlFamily.SuspendLayout();
            this.pnlOther.SuspendLayout();
            this.pnlRightSide.SuspendLayout();
            this.pnlWorkerField.SuspendLayout();
            this.SuspendLayout();

            //
            // txtWorkerField
            //
            this.txtWorkerField.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtWorkerField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWorkerField.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtWorkerField.Location = new System.Drawing.Point(114, 2);
            this.txtWorkerField.Name = "txtWorkerField";
            this.txtWorkerField.ReadOnly = true;
            this.txtWorkerField.Size = new System.Drawing.Size(94, 14);
            this.txtWorkerField.TabIndex = 1;
            this.txtWorkerField.TabStop = false;
            this.txtWorkerField.Text = "";
            this.tipMain.SetToolTip(this.txtWorkerField, "OM Field where the Partner is " + "working");

            //
            // lblStatusUpdated
            //
            this.lblStatusUpdated.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblStatusUpdated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblStatusUpdated.Location = new System.Drawing.Point(18, 44);
            this.lblStatusUpdated.Name = "lblStatusUpdated";
            this.lblStatusUpdated.Size = new System.Drawing.Size(96, 18);
            this.lblStatusUpdated.TabIndex = 27;
            this.lblStatusUpdated.Text = "Status Updated:";
            this.lblStatusUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtStatusChange
            //
            this.txtStatusChange.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtStatusChange.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatusChange.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtStatusChange.Location = new System.Drawing.Point(120, 44);
            this.txtStatusChange.Name = "txtStatusChange";
            this.txtStatusChange.ReadOnly = true;
            this.txtStatusChange.Size = new System.Drawing.Size(96, 14);
            this.txtStatusChange.TabIndex = 28;
            this.txtStatusChange.TabStop = false;
            this.txtStatusChange.Text = "";
            this.tipMain.SetToolTip(this.txtStatusChange, "Date of the last change of " + "the Partner Status");

            //
            // lblPersonGender
            //
            this.lblPersonGender.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPersonGender.Location = new System.Drawing.Point(4, 44);
            this.lblPersonGender.Name = "lblPersonGender";
            this.lblPersonGender.Size = new System.Drawing.Size(72, 18);
            this.lblPersonGender.TabIndex = 6;
            this.lblPersonGender.Text = "&Gender:";
            this.lblPersonGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblTitleNamePerson
            //
            this.lblTitleNamePerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTitleNamePerson.Location = new System.Drawing.Point(4, 22);
            this.lblTitleNamePerson.Name = "lblTitleNamePerson";
            this.lblTitleNamePerson.Size = new System.Drawing.Size(72, 23);
            this.lblTitleNamePerson.TabIndex = 1;
            this.lblTitleNamePerson.Text = "Title/Na&me:";
            this.lblTitleNamePerson.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblPartnerStatus
            //
            this.lblPartnerStatus.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblPartnerStatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPartnerStatus.Location = new System.Drawing.Point(18, 24);
            this.lblPartnerStatus.Name = "lblPartnerStatus";
            this.lblPartnerStatus.Size = new System.Drawing.Size(96, 23);
            this.lblPartnerStatus.TabIndex = 13;
            this.lblPartnerStatus.Text = "Partner &Status:";
            this.lblPartnerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtPersonTitle
            //
            this.txtPersonTitle.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPersonTitle.Location = new System.Drawing.Point(80, 18);
            this.txtPersonTitle.Name = "txtPersonTitle";
            this.txtPersonTitle.Size = new System.Drawing.Size(88, 21);
            this.txtPersonTitle.TabIndex = 2;
            this.txtPersonTitle.Text = "";

            //
            // txtPersonFirstName
            //
            this.txtPersonFirstName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPersonFirstName.Location = new System.Drawing.Point(170, 18);
            this.txtPersonFirstName.Name = "txtPersonFirstName";
            this.txtPersonFirstName.Size = new System.Drawing.Size(122, 21);
            this.txtPersonFirstName.TabIndex = 3;
            this.txtPersonFirstName.Text = "";

            //
            // txtPersonMiddleName
            //
            this.txtPersonMiddleName.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtPersonMiddleName.Location = new System.Drawing.Point(294, 18);
            this.txtPersonMiddleName.Name = "txtPersonMiddleName";
            this.txtPersonMiddleName.Size = new System.Drawing.Size(30, 21);
            this.txtPersonMiddleName.TabIndex = 4;
            this.txtPersonMiddleName.Text = "";

            //
            // txtPersonFamilyName
            //
            this.txtPersonFamilyName.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtPersonFamilyName.Location = new System.Drawing.Point(326, 18);
            this.txtPersonFamilyName.Name = "txtPersonFamilyName";
            this.txtPersonFamilyName.Size = new System.Drawing.Size(182, 21);
            this.txtPersonFamilyName.TabIndex = 5;
            this.txtPersonFamilyName.Text = "";

            //
            // lblAddresseeType
            //
            this.lblAddresseeType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblAddresseeType.Location = new System.Drawing.Point(188, 54);
            this.lblAddresseeType.Name = "lblAddresseeType";
            this.lblAddresseeType.Size = new System.Drawing.Size(104, 14);
            this.lblAddresseeType.TabIndex = 8;
            this.lblAddresseeType.Text = "Addressee Type:";
            this.lblAddresseeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // chkNoSolicitations
            //
            this.chkNoSolicitations.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkNoSolicitations.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.chkNoSolicitations.Location = new System.Drawing.Point(106, 0);
            this.chkNoSolicitations.Name = "chkNoSolicitations";
            this.chkNoSolicitations.Size = new System.Drawing.Size(108, 20);
            this.chkNoSolicitations.TabIndex = 10;
            this.chkNoSolicitations.Text = "No Solicitations";
            this.chkNoSolicitations.CheckedChanged += new System.EventHandler(this.ChkNoSolicitations_CheckedChanged);

            //
            // txtPartnerClass
            //
            this.txtPartnerClass.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartnerClass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerClass.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPartnerClass.Location = new System.Drawing.Point(332, 12);
            this.txtPartnerClass.Name = "txtPartnerClass";
            this.txtPartnerClass.ReadOnly = true;
            this.txtPartnerClass.Size = new System.Drawing.Size(112, 14);
            this.txtPartnerClass.TabIndex = 72;
            this.txtPartnerClass.TabStop = false;
            this.txtPartnerClass.Text = "";
            this.tipMain.SetToolTip(this.txtPartnerClass, "Partner Class");

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartnerKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerKey.ControlMode = Ict.Common.Controls.TMaskedTextBoxMode.PartnerKey;
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.Location = new System.Drawing.Point(86, 12);
            this.txtPartnerKey.Mask = "##########";
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PlaceHolder = "0";
            this.txtPartnerKey.ReadOnly = true;
            this.txtPartnerKey.Size = new System.Drawing.Size(88, 13);
            this.txtPartnerKey.TabIndex = 73;
            this.txtPartnerKey.TabStop = false;
            this.txtPartnerKey.Text = "0000000000";
            this.tipMain.SetToolTip(this.txtPartnerKey, "Partner Key");

            //
            // lblPartnerClass
            //
            this.lblPartnerClass.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPartnerClass.Location = new System.Drawing.Point(274, 12);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(52, 16);
            this.lblPartnerClass.TabIndex = 75;
            this.lblPartnerClass.Text = "Class:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblPartnerKey
            //
            this.lblPartnerKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPartnerKey.Location = new System.Drawing.Point(50, 12);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(30, 16);
            this.lblPartnerKey.TabIndex = 74;
            this.lblPartnerKey.Text = "Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlPartner
            //
            this.pnlPartner.Controls.Add(this.cmbAddresseeType);
            this.pnlPartner.Controls.Add(this.chkNoSolicitations);
            this.pnlPartner.Location = new System.Drawing.Point(298, 52);
            this.pnlPartner.Name = "pnlPartner";
            this.pnlPartner.Size = new System.Drawing.Size(224, 23);
            this.pnlPartner.TabIndex = 1;

            //
            // cmbAddresseeType
            //
            this.cmbAddresseeType.ComboBoxWidth = 100;
            this.cmbAddresseeType.Filter = null;
            this.cmbAddresseeType.ListTable = TCmbAutoPopulated.TListTableEnum.AddresseeTypeList;
            this.cmbAddresseeType.Location = new System.Drawing.Point(0, 0);
            this.cmbAddresseeType.Name = "cmbAddresseeType";
            this.cmbAddresseeType.Size = new System.Drawing.Size(100, 22);
            this.cmbAddresseeType.TabIndex = 10;

            //
            // pnlPerson
            //
            this.pnlPerson.Controls.Add(this.lblTitleNamePerson);
            this.pnlPerson.Controls.Add(this.txtPersonMiddleName);
            this.pnlPerson.Controls.Add(this.txtPersonFirstName);
            this.pnlPerson.Controls.Add(this.lblPersonGender);
            this.pnlPerson.Controls.Add(this.txtPersonFamilyName);
            this.pnlPerson.Controls.Add(this.txtPersonTitle);
            this.pnlPerson.Controls.Add(this.lblPersonPanel);
            this.pnlPerson.Controls.Add(this.cmbPersonGender);
            this.pnlPerson.Location = new System.Drawing.Point(4, 12);
            this.pnlPerson.Name = "pnlPerson";
            this.pnlPerson.Size = new System.Drawing.Size(520, 66);
            this.pnlPerson.TabIndex = 0;
            this.pnlPerson.Visible = false;

            //
            // lblPersonPanel
            //
            this.lblPersonPanel.Font =
                new System.Drawing.Font("Verdana", 8.25f, ((System.Drawing.FontStyle)(
                                                               System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)),
                    System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblPersonPanel.ForeColor = System.Drawing.Color.Firebrick;
            this.lblPersonPanel.Location = new System.Drawing.Point(0, 2);
            this.lblPersonPanel.Name = "lblPersonPanel";
            this.lblPersonPanel.Size = new System.Drawing.Size(100, 16);
            this.lblPersonPanel.TabIndex = 22;
            this.lblPersonPanel.Text = "PERSON Panel";
            this.lblPersonPanel.Visible = false;

            //
            // cmbPersonGender
            //
            this.cmbPersonGender.ComboBoxWidth = 88;
            this.cmbPersonGender.Filter = null;
            this.cmbPersonGender.ListTable = TCmbAutoPopulated.TListTableEnum.GenderList;
            this.cmbPersonGender.Location = new System.Drawing.Point(80, 40);
            this.cmbPersonGender.Name = "cmbPersonGender";
            this.cmbPersonGender.Size = new System.Drawing.Size(90, 22);
            this.cmbPersonGender.TabIndex = 6;

            //
            // pnlFamily
            //
            this.pnlFamily.Controls.Add(this.lblTitleNameFamily);
            this.pnlFamily.Controls.Add(this.txtFamilyFirstName);
            this.pnlFamily.Controls.Add(this.txtFamilyFamilyName);
            this.pnlFamily.Controls.Add(this.txtFamilyTitle);
            this.pnlFamily.Controls.Add(this.lblFamilyPanel);
            this.pnlFamily.Location = new System.Drawing.Point(4, 12);
            this.pnlFamily.Name = "pnlFamily";
            this.pnlFamily.Size = new System.Drawing.Size(520, 66);
            this.pnlFamily.TabIndex = 0;
            this.pnlFamily.Visible = false;

            //
            // lblTitleNameFamily
            //
            this.lblTitleNameFamily.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTitleNameFamily.Location = new System.Drawing.Point(4, 22);
            this.lblTitleNameFamily.Name = "lblTitleNameFamily";
            this.lblTitleNameFamily.Size = new System.Drawing.Size(72, 23);
            this.lblTitleNameFamily.TabIndex = 1;
            this.lblTitleNameFamily.Text = "Title/Na&me:";
            this.lblTitleNameFamily.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtFamilyFirstName
            //
            this.txtFamilyFirstName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtFamilyFirstName.Location = new System.Drawing.Point(170, 18);
            this.txtFamilyFirstName.Name = "txtFamilyFirstName";
            this.txtFamilyFirstName.Size = new System.Drawing.Size(154, 21);
            this.txtFamilyFirstName.TabIndex = 3;
            this.txtFamilyFirstName.Text = "";

            //
            // txtFamilyFamilyName
            //
            this.txtFamilyFamilyName.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtFamilyFamilyName.Location = new System.Drawing.Point(326, 18);
            this.txtFamilyFamilyName.Name = "txtFamilyFamilyName";
            this.txtFamilyFamilyName.Size = new System.Drawing.Size(182, 21);
            this.txtFamilyFamilyName.TabIndex = 4;
            this.txtFamilyFamilyName.Text = "";

            //
            // txtFamilyTitle
            //
            this.txtFamilyTitle.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtFamilyTitle.Location = new System.Drawing.Point(80, 18);
            this.txtFamilyTitle.Name = "txtFamilyTitle";
            this.txtFamilyTitle.Size = new System.Drawing.Size(88, 21);
            this.txtFamilyTitle.TabIndex = 2;
            this.txtFamilyTitle.Text = "";

            //
            // lblFamilyPanel
            //
            this.lblFamilyPanel.Font =
                new System.Drawing.Font("Verdana", 8.25f, ((System.Drawing.FontStyle)(
                                                               System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)),
                    System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblFamilyPanel.ForeColor = System.Drawing.Color.Firebrick;
            this.lblFamilyPanel.Location = new System.Drawing.Point(0, 2);
            this.lblFamilyPanel.Name = "lblFamilyPanel";
            this.lblFamilyPanel.Size = new System.Drawing.Size(148, 16);
            this.lblFamilyPanel.TabIndex = 20;
            this.lblFamilyPanel.Text = "FAMILY Panel";
            this.lblFamilyPanel.Visible = false;

            //
            // pnlOther
            //
            this.pnlOther.Controls.Add(this.lblOtherPanel);
            this.pnlOther.Controls.Add(this.lblName);
            this.pnlOther.Controls.Add(this.txtOtherName);
            this.pnlOther.Location = new System.Drawing.Point(4, 12);
            this.pnlOther.Name = "pnlOther";
            this.pnlOther.Size = new System.Drawing.Size(520, 66);
            this.pnlOther.TabIndex = 0;
            this.pnlOther.Visible = false;

            //
            // lblOtherPanel
            //
            this.lblOtherPanel.Font =
                new System.Drawing.Font("Verdana", 8.25f, ((System.Drawing.FontStyle)(
                                                               System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)),
                    System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblOtherPanel.ForeColor = System.Drawing.Color.Firebrick;
            this.lblOtherPanel.Location = new System.Drawing.Point(0, 2);
            this.lblOtherPanel.Name = "lblOtherPanel";
            this.lblOtherPanel.Size = new System.Drawing.Size(180, 16);
            this.lblOtherPanel.TabIndex = 22;
            this.lblOtherPanel.Text = "OTHER Panel";
            this.lblOtherPanel.Visible = false;

            //
            // lblName
            //
            this.lblName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblName.Location = new System.Drawing.Point(4, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Na&me:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtOtherName
            //
            this.txtOtherName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtOtherName.Location = new System.Drawing.Point(80, 18);
            this.txtOtherName.Name = "txtOtherName";
            this.txtOtherName.Size = new System.Drawing.Size(428, 21);
            this.txtOtherName.TabIndex = 2;
            this.txtOtherName.Text = "";

            //
            // tipMain
            //
            this.tipMain.AutoPopDelay = 4000;
            this.tipMain.InitialDelay = 500;
            this.tipMain.ReshowDelay = 100;

            //
            // btnEditWorkerField
            //
            this.btnEditWorkerField.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnEditWorkerField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditWorkerField.ImageIndex = 0;
            this.btnEditWorkerField.ImageList = this.imlButtonIcons;
            this.btnEditWorkerField.Location = new System.Drawing.Point(0, 0);
            this.btnEditWorkerField.Name = "btnEditWorkerField";
            this.btnEditWorkerField.Size = new System.Drawing.Size(108, 23);
            this.btnEditWorkerField.TabIndex = 0;
            this.btnEditWorkerField.Text = "     &Field...";
            this.btnEditWorkerField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipMain.SetToolTip(this.btnEditWorkerField, "Edit Worker Field");
            this.btnEditWorkerField.Click += new System.EventHandler(this.BtnEditWorkerField_Click);

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // txtLastGiftDate
            //
            this.txtLastGiftDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtLastGiftDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastGiftDate.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLastGiftDate.Location = new System.Drawing.Point(84, 78);
            this.txtLastGiftDate.Name = "txtLastGiftDate";
            this.txtLastGiftDate.ReadOnly = true;
            this.txtLastGiftDate.Size = new System.Drawing.Size(88, 14);
            this.txtLastGiftDate.TabIndex = 78;
            this.txtLastGiftDate.TabStop = false;
            this.txtLastGiftDate.Text = "01-JAN-9999";
            this.tipMain.SetToolTip(this.txtLastGiftDate, "Date when this Partner has " + "last given a gift");

            //
            // txtLastGiftDetails
            //
            this.txtLastGiftDetails.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtLastGiftDetails.BackColor = System.Drawing.SystemColors.Control;
            this.txtLastGiftDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastGiftDetails.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLastGiftDetails.Location = new System.Drawing.Point(182, 78);
            this.txtLastGiftDetails.Name = "txtLastGiftDetails";
            this.txtLastGiftDetails.ReadOnly = true;
            this.txtLastGiftDetails.Size = new System.Drawing.Size(328, 14);
            this.txtLastGiftDetails.TabIndex = 79;
            this.txtLastGiftDetails.TabStop = false;
            this.txtLastGiftDetails.Text = "Currency + Amount, Given To";
            this.tipMain.SetToolTip(this.txtLastGiftDetails, "Amount and to whom the g" + "ift was given to");

            //
            // txtLastContactDate
            //
            this.txtLastContactDate.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtLastContactDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtLastContactDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastContactDate.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLastContactDate.Location = new System.Drawing.Point(642, 78);
            this.txtLastContactDate.Name = "txtLastContactDate";
            this.txtLastContactDate.ReadOnly = true;
            this.txtLastContactDate.Size = new System.Drawing.Size(88, 14);
            this.txtLastContactDate.TabIndex = 81;
            this.txtLastContactDate.TabStop = false;
            this.txtLastContactDate.Text = "01-JAN-9999";
            this.tipMain.SetToolTip(this.txtLastContactDate, "Date when the last conta" + "ct was made with this Partner");

            //
            // pnlRightSide
            //
            this.pnlRightSide.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.pnlRightSide.Controls.Add(this.cmbPartnerStatus);
            this.pnlRightSide.Controls.Add(this.pnlWorkerField);
            this.pnlRightSide.Controls.Add(this.btnCreatedOverall);
            this.pnlRightSide.Controls.Add(this.lblStatusUpdated);
            this.pnlRightSide.Controls.Add(this.txtStatusChange);
            this.pnlRightSide.Controls.Add(this.lblPartnerStatus);
            this.pnlRightSide.Location = new System.Drawing.Point(522, 10);
            this.pnlRightSide.Name = "pnlRightSide";
            this.pnlRightSide.Size = new System.Drawing.Size(232, 68);
            this.pnlRightSide.TabIndex = 76;

            //
            // cmbPartnerStatus
            //
            this.cmbPartnerStatus.ComboBoxWidth = 95;
            this.cmbPartnerStatus.Filter = null;
            this.cmbPartnerStatus.ListTable = TCmbAutoPopulated.TListTableEnum.PartnerStatusList;
            this.cmbPartnerStatus.Location = new System.Drawing.Point(118, 20);
            this.cmbPartnerStatus.Name = "cmbPartnerStatus";
            this.cmbPartnerStatus.Size = new System.Drawing.Size(100, 22);
            this.cmbPartnerStatus.TabIndex = 14;

            //
            // pnlWorkerField
            //
            this.pnlWorkerField.Controls.Add(this.btnEditWorkerField);
            this.pnlWorkerField.Controls.Add(this.txtWorkerField);
            this.pnlWorkerField.Location = new System.Drawing.Point(6, 0);
            this.pnlWorkerField.Name = "pnlWorkerField";
            this.pnlWorkerField.Size = new System.Drawing.Size(212, 22);
            this.pnlWorkerField.TabIndex = 11;
            this.pnlWorkerField.Tag = "dontdisable";
            this.pnlWorkerField.Visible = false;

            //
            // btnCreatedOverall
            //
            this.btnCreatedOverall.CreatedBy = null;
            this.btnCreatedOverall.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedOverall.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedOverall.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedOverall.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedOverall.Image")));
            this.btnCreatedOverall.Location = new System.Drawing.Point(218, 0);
            this.btnCreatedOverall.ModifiedBy = null;
            this.btnCreatedOverall.Name = "btnCreatedOverall";
            this.btnCreatedOverall.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedOverall.TabIndex = 15;
            this.btnCreatedOverall.Tag = "dontdisable";

            //
            // lblLastGiftDate
            //
            this.lblLastGiftDate.Location = new System.Drawing.Point(11, 78);
            this.lblLastGiftDate.Name = "lblLastGiftDate";
            this.lblLastGiftDate.Size = new System.Drawing.Size(72, 14);
            this.lblLastGiftDate.TabIndex = 77;
            this.lblLastGiftDate.Text = "Last Gift:";
            this.lblLastGiftDate.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblLastContactDate
            //
            this.lblLastContactDate.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblLastContactDate.Location = new System.Drawing.Point(552, 78);
            this.lblLastContactDate.Name = "lblLastContactDate";
            this.lblLastContactDate.Size = new System.Drawing.Size(86, 14);
            this.lblLastContactDate.TabIndex = 80;
            this.lblLastContactDate.Text = "Last Contact:";
            this.lblLastContactDate.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlBalloonTipAnchorTabs
            //
            this.pnlBalloonTipAnchorTabs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBalloonTipAnchorTabs.Location = new System.Drawing.Point(0, 97);
            this.pnlBalloonTipAnchorTabs.Name = "pnlBalloonTipAnchorTabs";
            this.pnlBalloonTipAnchorTabs.Size = new System.Drawing.Size(756, 1);
            this.pnlBalloonTipAnchorTabs.TabIndex = 82;

            //
            // pnlBalloonTipAnchorHelp
            //
            this.pnlBalloonTipAnchorHelp.Location = new System.Drawing.Point(300, 0);
            this.pnlBalloonTipAnchorHelp.Name = "pnlBalloonTipAnchorHelp";
            this.pnlBalloonTipAnchorHelp.Size = new System.Drawing.Size(1, 1);
            this.pnlBalloonTipAnchorHelp.TabIndex = 83;

            //
            // TUC_ParnerEdit_CollapsiblePart
            //
            this.AutoSize = true;
            this.Controls.Add(this.pnlBalloonTipAnchorHelp);
            this.Controls.Add(this.pnlBalloonTipAnchorTabs);
            this.Controls.Add(this.txtLastContactDate);
            this.Controls.Add(this.lblLastContactDate);
            this.Controls.Add(this.txtLastGiftDetails);
            this.Controls.Add(this.txtLastGiftDate);
            this.Controls.Add(this.lblLastGiftDate);
            this.Controls.Add(this.pnlRightSide);
            this.Controls.Add(this.txtPartnerClass);
            this.Controls.Add(this.txtPartnerKey);
            this.Controls.Add(this.lblAddresseeType);
            this.Controls.Add(this.lblPartnerKey);
            this.Controls.Add(this.lblPartnerClass);
            this.Controls.Add(this.pnlPartner);
            this.Controls.Add(this.pnlFamily);
            this.Controls.Add(this.pnlOther);
            this.Controls.Add(this.pnlPerson);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_ParnerEdit_CollapsiblePart";
            this.Size = new System.Drawing.Size(756, 98);
            this.Controls.SetChildIndex(this.pnlPerson, 0);
            this.Controls.SetChildIndex(this.pnlOther, 0);
            this.Controls.SetChildIndex(this.pnlFamily, 0);
            this.Controls.SetChildIndex(this.pnlPartner, 0);
            this.Controls.SetChildIndex(this.lblPartnerClass, 0);
            this.Controls.SetChildIndex(this.lblPartnerKey, 0);
            this.Controls.SetChildIndex(this.lblAddresseeType, 0);
            this.Controls.SetChildIndex(this.txtPartnerKey, 0);
            this.Controls.SetChildIndex(this.txtPartnerClass, 0);
            this.Controls.SetChildIndex(this.pnlRightSide, 0);
            this.Controls.SetChildIndex(this.lblLastGiftDate, 0);
            this.Controls.SetChildIndex(this.txtLastGiftDate, 0);
            this.Controls.SetChildIndex(this.txtLastGiftDetails, 0);
            this.Controls.SetChildIndex(this.lblLastContactDate, 0);
            this.Controls.SetChildIndex(this.txtLastContactDate, 0);
            this.Controls.SetChildIndex(this.pnlBalloonTipAnchorTabs, 0);
            this.Controls.SetChildIndex(this.pnlBalloonTipAnchorHelp, 0);
            this.pnlPartner.ResumeLayout(false);
            this.pnlPerson.ResumeLayout(false);
            this.pnlFamily.ResumeLayout(false);
            this.pnlOther.ResumeLayout(false);
            this.pnlRightSide.ResumeLayout(false);
            this.pnlWorkerField.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtWorkerField;
        private System.Windows.Forms.Label lblStatusUpdated;
        private System.Windows.Forms.TextBox txtStatusChange;
        private TCmbAutoPopulated cmbAddresseeType;
        private TCmbAutoPopulated cmbPartnerStatus;
        private System.Windows.Forms.Label lblPersonGender;
        private System.Windows.Forms.Label lblTitleNamePerson;
        private System.Windows.Forms.Label lblPartnerStatus;
        private System.Windows.Forms.TextBox txtPersonTitle;
        private System.Windows.Forms.TextBox txtPersonFirstName;
        private System.Windows.Forms.TextBox txtPersonMiddleName;
        private System.Windows.Forms.TextBox txtPersonFamilyName;
        private System.Windows.Forms.Label lblAddresseeType;
        private System.Windows.Forms.CheckBox chkNoSolicitations;
        private System.Windows.Forms.TextBox txtPartnerClass;
        private TTxtMaskedTextBox txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Panel pnlPartner;
        private System.Windows.Forms.Panel pnlPerson;
        private System.Windows.Forms.Panel pnlFamily;
        private System.Windows.Forms.Label lblTitleNameFamily;
        private System.Windows.Forms.TextBox txtFamilyFirstName;
        private System.Windows.Forms.TextBox txtFamilyFamilyName;
        private System.Windows.Forms.TextBox txtFamilyTitle;
        private System.Windows.Forms.Label lblFamilyPanel;
        private System.Windows.Forms.Label lblPersonPanel;
        private System.Windows.Forms.Panel pnlOther;
        private System.Windows.Forms.Label lblOtherPanel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtOtherName;
        private System.Windows.Forms.ToolTip tipMain;
        private System.Windows.Forms.Button btnEditWorkerField;
        private System.Windows.Forms.Panel pnlRightSide;
        private TbtnCreated btnCreatedOverall;
        private System.Windows.Forms.Panel pnlWorkerField;
        private System.Windows.Forms.Label lblLastGiftDate;
        private System.Windows.Forms.TextBox txtLastGiftDate;
        private System.Windows.Forms.TextBox txtLastGiftDetails;
        private System.Windows.Forms.Label lblLastContactDate;
        private System.Windows.Forms.TextBox txtLastContactDate;
        private TCmbAutoPopulated cmbPersonGender;
        private TexpTextBoxStringLengthCheck expStringLengthCheckCollapsiblePart;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Panel pnlBalloonTipAnchorTabs;
        private System.Windows.Forms.Panel pnlBalloonTipAnchorHelp;
    }
}