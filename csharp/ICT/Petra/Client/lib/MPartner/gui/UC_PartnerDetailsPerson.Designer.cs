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
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerDetailsPerson
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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsPerson));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsPerson = new System.Windows.Forms.Panel();
            this.btnCreatedPerson = new Ict.Common.Controls.TbtnCreated();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.pnlOccupation = new System.Windows.Forms.Panel();
            this.txtOccupationCode = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlAcquisition = new System.Windows.Forms.Panel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlMaritalSince = new System.Windows.Forms.Panel();
            this.txtMaritalStatusComment = new System.Windows.Forms.TextBox();
            this.lblMaritalStatusComment = new System.Windows.Forms.Label();
            this.lblMaritalStatusSince = new System.Windows.Forms.Label();
            this.txtMaritalStatusSince = new System.Windows.Forms.TextBox();
            this.pnlMaritalAcademic = new System.Windows.Forms.Panel();
            this.lblMaritalStatus = new System.Windows.Forms.Label();
            this.cmbMaritalStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAcademicTitle = new System.Windows.Forms.Label();
            this.txtAcademicTitle = new System.Windows.Forms.TextBox();
            this.pnlBirthDecoration = new System.Windows.Forms.Panel();
            this.txtDecorations = new System.Windows.Forms.TextBox();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.txtDateOfBirth = new System.Windows.Forms.TextBox();
            this.lblDecorations = new System.Windows.Forms.Label();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreferredName = new System.Windows.Forms.Label();
            this.txtPreferredName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.grpBelieverSince = new System.Windows.Forms.GroupBox();
            this.pnlBelieverSinceYear = new System.Windows.Forms.Panel();
            this.txtBelieverSince = new System.Windows.Forms.TextBox();
            this.lblBelieverSince = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtBelieverComment = new System.Windows.Forms.TextBox();
            this.expStringLengthCheckPerson = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsPerson.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlOccupation.SuspendLayout();
            this.pnlAcquisition.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlMaritalSince.SuspendLayout();
            this.pnlMaritalAcademic.SuspendLayout();
            this.pnlBirthDecoration.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.grpBelieverSince.SuspendLayout();
            this.pnlBelieverSinceYear.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsPerson
            //
            this.pnlPartnerDetailsPerson.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerDetailsPerson.AutoScroll = true;
            this.pnlPartnerDetailsPerson.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsPerson.Controls.Add(this.btnCreatedPerson);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpBelieverSince);
            this.pnlPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsPerson.Name = "pnlPartnerDetailsPerson";
            this.pnlPartnerDetailsPerson.Size = new System.Drawing.Size(634, 342);
            this.pnlPartnerDetailsPerson.TabIndex = 0;

            //
            // btnCreatedPerson
            //
            this.btnCreatedPerson.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedPerson.CreatedBy = null;
            this.btnCreatedPerson.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedPerson.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedPerson.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedPerson.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedPerson.Image")));
            this.btnCreatedPerson.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedPerson.ModifiedBy = null;
            this.btnCreatedPerson.Name = "btnCreatedPerson";
            this.btnCreatedPerson.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedPerson.TabIndex = 3;
            this.btnCreatedPerson.Tag = "dontdisable";

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlOccupation);
            this.grpMisc.Controls.Add(this.pnlAcquisition);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlMaritalSince);
            this.grpMisc.Controls.Add(this.pnlMaritalAcademic);
            this.grpMisc.Controls.Add(this.pnlBirthDecoration);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 208);
            this.grpMisc.TabIndex = 1;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Miscellaneous";

            //
            // pnlOccupation
            //
            this.pnlOccupation.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOccupation.Controls.Add(this.txtOccupationCode);
            this.pnlOccupation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOccupation.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.pnlOccupation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlOccupation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlOccupation.Location = new System.Drawing.Point(3, 181);
            this.pnlOccupation.Name = "pnlOccupation";
            this.pnlOccupation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlOccupation.Size = new System.Drawing.Size(604, 23);
            this.pnlOccupation.TabIndex = 21;

            //
            // txtOccupationCode
            //
            this.txtOccupationCode.ASpecialSetting = true;
            this.txtOccupationCode.ButtonText = "&Occupation...";
            this.txtOccupationCode.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtOccupationCode.ButtonWidth = 108;
            this.txtOccupationCode.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.OccupationList;
            this.txtOccupationCode.Location = new System.Drawing.Point(17, 0);
            this.txtOccupationCode.MaxLength = 32767;
            this.txtOccupationCode.Name = "txtOccupationCode";
            this.txtOccupationCode.PartnerClass = "";
            this.txtOccupationCode.PreventFaultyLeaving = false;
            this.txtOccupationCode.ReadOnly = false;
            this.txtOccupationCode.Size = new System.Drawing.Size(584, 23);
            this.txtOccupationCode.TabIndex = 16;
            this.txtOccupationCode.TextBoxWidth = 170;
            this.txtOccupationCode.VerificationResultCollection = null;
            this.txtOccupationCode.ClickButton += new TDelegateClickButton(this.TxtOccupationCode_ClickButton);

            //
            // pnlAcquisition
            //
            this.pnlAcquisition.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAcquisition.Controls.Add(this.lblAcquisitionCode);
            this.pnlAcquisition.Controls.Add(this.cmbAcquisitionCode);
            this.pnlAcquisition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcquisition.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.pnlAcquisition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAcquisition.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlAcquisition.Location = new System.Drawing.Point(3, 158);
            this.pnlAcquisition.Name = "pnlAcquisition";
            this.pnlAcquisition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisition.Size = new System.Drawing.Size(604, 23);
            this.pnlAcquisition.TabIndex = 20;

            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(17, 0);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.Size = new System.Drawing.Size(108, 22);
            this.lblAcquisitionCode.TabIndex = 12;
            this.lblAcquisitionCode.Text = "&Acquisition Code:";
            this.lblAcquisitionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(127, 0);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(512, 22);
            this.cmbAcquisitionCode.TabIndex = 13;

            if (this.cmbAcquisitionCode.CausesValidation)
            {
                // strange; there was no assignment in delphi.net; but that is not allowed in c#
            }

            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // pnlLanguageCode
            //
            this.pnlLanguageCode.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.pnlLanguageCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlLanguageCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 135);
            this.pnlLanguageCode.Name = "pnlLanguageCode";
            this.pnlLanguageCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlLanguageCode.Size = new System.Drawing.Size(604, 23);
            this.pnlLanguageCode.TabIndex = 19;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(17, 0);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(108, 22);
            this.lblLanguageCode.TabIndex = 10;
            this.lblLanguageCode.Text = "Lang&uage Code:";
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbLanguageCode
            //
            this.cmbLanguageCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguageCode.ComboBoxWidth = 57;
            this.cmbLanguageCode.Filter = null;
            this.cmbLanguageCode.ListTable = TCmbAutoPopulated.TListTableEnum.LanguageCodeList;
            this.cmbLanguageCode.Location = new System.Drawing.Point(127, 0);
            this.cmbLanguageCode.Name = "cmbLanguageCode";
            this.cmbLanguageCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbLanguageCode.SelectedItem")));
            this.cmbLanguageCode.SelectedValue = null;
            this.cmbLanguageCode.Size = new System.Drawing.Size(468, 22);
            this.cmbLanguageCode.TabIndex = 11;

            //
            // pnlMaritalSince
            //
            this.pnlMaritalSince.Controls.Add(this.txtMaritalStatusComment);
            this.pnlMaritalSince.Controls.Add(this.lblMaritalStatusComment);
            this.pnlMaritalSince.Controls.Add(this.lblMaritalStatusSince);
            this.pnlMaritalSince.Controls.Add(this.txtMaritalStatusSince);
            this.pnlMaritalSince.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMaritalSince.Location = new System.Drawing.Point(3, 63);
            this.pnlMaritalSince.Name = "pnlMaritalSince";
            this.pnlMaritalSince.Size = new System.Drawing.Size(604, 72);
            this.pnlMaritalSince.TabIndex = 18;

            //
            // txtMaritalStatusComment
            //
            this.txtMaritalStatusComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaritalStatusComment.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtMaritalStatusComment.Location = new System.Drawing.Point(127, 24);
            this.txtMaritalStatusComment.Multiline = true;
            this.txtMaritalStatusComment.Name = "txtMaritalStatusComment";
            this.txtMaritalStatusComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMaritalStatusComment.Size = new System.Drawing.Size(472, 44);
            this.txtMaritalStatusComment.TabIndex = 19;
            this.txtMaritalStatusComment.Text = "";

            //
            // lblMaritalStatusComment
            //
            this.lblMaritalStatusComment.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.lblMaritalStatusComment.Location = new System.Drawing.Point(1, 27);
            this.lblMaritalStatusComment.Name = "lblMaritalStatusComment";
            this.lblMaritalStatusComment.Size = new System.Drawing.Size(124, 40);
            this.lblMaritalStatusComment.TabIndex = 18;
            this.lblMaritalStatusComment.Text = "Marital Status Comment:";
            this.lblMaritalStatusComment.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblMaritalStatusSince
            //
            this.lblMaritalStatusSince.Location = new System.Drawing.Point(1, 0);
            this.lblMaritalStatusSince.Name = "lblMaritalStatusSince";
            this.lblMaritalStatusSince.Size = new System.Drawing.Size(124, 22);
            this.lblMaritalStatusSince.TabIndex = 4;
            this.lblMaritalStatusSince.Text = "Marital Status Si&nce:";
            this.lblMaritalStatusSince.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtMaritalStatusSince
            //
            this.txtMaritalStatusSince.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtMaritalStatusSince.Location = new System.Drawing.Point(127, 0);
            this.txtMaritalStatusSince.Name = "txtMaritalStatusSince";
            this.txtMaritalStatusSince.Size = new System.Drawing.Size(94, 21);
            this.txtMaritalStatusSince.TabIndex = 5;
            this.txtMaritalStatusSince.Text = "";

            //
            // pnlMaritalAcademic
            //
            this.pnlMaritalAcademic.Controls.Add(this.lblMaritalStatus);
            this.pnlMaritalAcademic.Controls.Add(this.cmbMaritalStatus);
            this.pnlMaritalAcademic.Controls.Add(this.lblAcademicTitle);
            this.pnlMaritalAcademic.Controls.Add(this.txtAcademicTitle);
            this.pnlMaritalAcademic.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMaritalAcademic.Location = new System.Drawing.Point(3, 40);
            this.pnlMaritalAcademic.Name = "pnlMaritalAcademic";
            this.pnlMaritalAcademic.Size = new System.Drawing.Size(604, 23);
            this.pnlMaritalAcademic.TabIndex = 17;

            //
            // lblMaritalStatus
            //
            this.lblMaritalStatus.Location = new System.Drawing.Point(17, 0);
            this.lblMaritalStatus.Name = "lblMaritalStatus";
            this.lblMaritalStatus.Size = new System.Drawing.Size(108, 22);
            this.lblMaritalStatus.TabIndex = 2;
            this.lblMaritalStatus.Text = "Mari&tal Status:";
            this.lblMaritalStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbMaritalStatus
            //
            this.cmbMaritalStatus.ComboBoxWidth = 39;
            this.cmbMaritalStatus.Filter = null;
            this.cmbMaritalStatus.ListTable = TCmbAutoPopulated.TListTableEnum.MaritalStatusList;
            this.cmbMaritalStatus.Location = new System.Drawing.Point(127, 0);
            this.cmbMaritalStatus.Name = "cmbMaritalStatus";
            this.cmbMaritalStatus.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbMaritalStatus.SelectedItem")));
            this.cmbMaritalStatus.SelectedValue = null;
            this.cmbMaritalStatus.Size = new System.Drawing.Size(211, 22);
            this.cmbMaritalStatus.TabIndex = 3;

            //
            // lblAcademicTitle
            //
            this.lblAcademicTitle.Location = new System.Drawing.Point(351, 0);
            this.lblAcademicTitle.Name = "lblAcademicTitle";
            this.lblAcademicTitle.Size = new System.Drawing.Size(104, 22);
            this.lblAcademicTitle.TabIndex = 8;
            this.lblAcademicTitle.Text = "A&cademic Title:";
            this.lblAcademicTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtAcademicTitle
            //
            this.txtAcademicTitle.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAcademicTitle.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtAcademicTitle.Location = new System.Drawing.Point(459, 0);
            this.txtAcademicTitle.Name = "txtAcademicTitle";
            this.txtAcademicTitle.Size = new System.Drawing.Size(140, 21);
            this.txtAcademicTitle.TabIndex = 9;
            this.txtAcademicTitle.Text = "";

            //
            // pnlBirthDecoration
            //
            this.pnlBirthDecoration.Controls.Add(this.txtDecorations);
            this.pnlBirthDecoration.Controls.Add(this.lblDateOfBirth);
            this.pnlBirthDecoration.Controls.Add(this.txtDateOfBirth);
            this.pnlBirthDecoration.Controls.Add(this.lblDecorations);
            this.pnlBirthDecoration.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBirthDecoration.Location = new System.Drawing.Point(3, 17);
            this.pnlBirthDecoration.Name = "pnlBirthDecoration";
            this.pnlBirthDecoration.Size = new System.Drawing.Size(604, 23);
            this.pnlBirthDecoration.TabIndex = 16;

            //
            // txtDecorations
            //
            this.txtDecorations.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDecorations.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDecorations.Location = new System.Drawing.Point(459, 0);
            this.txtDecorations.Name = "txtDecorations";
            this.txtDecorations.Size = new System.Drawing.Size(140, 21);
            this.txtDecorations.TabIndex = 8;
            this.txtDecorations.Text = "";

            //
            // lblDateOfBirth
            //
            this.lblDateOfBirth.Location = new System.Drawing.Point(21, 0);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new System.Drawing.Size(104, 22);
            this.lblDateOfBirth.TabIndex = 0;
            this.lblDateOfBirth.Text = "&Date of Birth:";
            this.lblDateOfBirth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtDateOfBirth
            //
            this.txtDateOfBirth.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtDateOfBirth.Location = new System.Drawing.Point(127, 0);
            this.txtDateOfBirth.Name = "txtDateOfBirth";
            this.txtDateOfBirth.Size = new System.Drawing.Size(94, 21);
            this.txtDateOfBirth.TabIndex = 1;
            this.txtDateOfBirth.Text = "";

            //
            // lblDecorations
            //
            this.lblDecorations.Location = new System.Drawing.Point(351, 0);
            this.lblDecorations.Name = "lblDecorations";
            this.lblDecorations.Size = new System.Drawing.Size(104, 21);
            this.lblDecorations.TabIndex = 6;
            this.lblDecorations.Text = "Dec&orations:";
            this.lblDecorations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlLocalName);
            this.grpNames.Controls.Add(this.pnlPreferedPreviousName);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 68);
            this.grpNames.TabIndex = 0;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Names";

            //
            // pnlLocalName
            //
            this.pnlLocalName.Controls.Add(this.lblLocalName);
            this.pnlLocalName.Controls.Add(this.txtLocalName);
            this.pnlLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocalName.Location = new System.Drawing.Point(3, 40);
            this.pnlLocalName.Name = "pnlLocalName";
            this.pnlLocalName.Size = new System.Drawing.Size(604, 24);
            this.pnlLocalName.TabIndex = 7;

            //
            // lblLocalName
            //
            this.lblLocalName.Location = new System.Drawing.Point(17, 0);
            this.lblLocalName.Name = "lblLocalName";
            this.lblLocalName.Size = new System.Drawing.Size(108, 23);
            this.lblLocalName.TabIndex = 4;
            this.lblLocalName.Text = "&Local Name:";
            this.lblLocalName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtLocalName
            //
            this.txtLocalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLocalName.Location = new System.Drawing.Point(127, 0);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(472, 21);
            this.txtLocalName.TabIndex = 5;
            this.txtLocalName.Text = "";

            //
            // pnlPreferedPreviousName
            //
            this.pnlPreferedPreviousName.Controls.Add(this.lblPreferredName);
            this.pnlPreferedPreviousName.Controls.Add(this.txtPreferredName);
            this.pnlPreferedPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreferedPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreferedPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreferedPreviousName.Location = new System.Drawing.Point(3, 17);
            this.pnlPreferedPreviousName.Name = "pnlPreferedPreviousName";
            this.pnlPreferedPreviousName.Size = new System.Drawing.Size(604, 23);
            this.pnlPreferedPreviousName.TabIndex = 6;

            //
            // lblPreferredName
            //
            this.lblPreferredName.Location = new System.Drawing.Point(17, 0);
            this.lblPreferredName.Name = "lblPreferredName";
            this.lblPreferredName.Size = new System.Drawing.Size(108, 23);
            this.lblPreferredName.TabIndex = 0;
            this.lblPreferredName.Text = "&Preferred Name:";
            this.lblPreferredName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtPreferredName
            //
            this.txtPreferredName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPreferredName.Location = new System.Drawing.Point(127, 0);
            this.txtPreferredName.Name = "txtPreferredName";
            this.txtPreferredName.Size = new System.Drawing.Size(142, 21);
            this.txtPreferredName.TabIndex = 1;
            this.txtPreferredName.Text = "";

            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(319, 0);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(136, 22);
            this.lblPreviousName.TabIndex = 2;
            this.lblPreviousName.Text = "P&revious Name:";
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPreviousName.Location = new System.Drawing.Point(459, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(140, 21);
            this.txtPreviousName.TabIndex = 3;
            this.txtPreviousName.Text = "";

            //
            // grpBelieverSince
            //
            this.grpBelieverSince.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBelieverSince.Controls.Add(this.pnlBelieverSinceYear);
            this.grpBelieverSince.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpBelieverSince.Location = new System.Drawing.Point(4, 276);
            this.grpBelieverSince.Name = "grpBelieverSince";
            this.grpBelieverSince.Size = new System.Drawing.Size(610, 66);
            this.grpBelieverSince.TabIndex = 2;
            this.grpBelieverSince.TabStop = false;
            this.grpBelieverSince.Text = "Believer since";
            this.grpBelieverSince.Visible = false;

            //
            // pnlBelieverSinceYear
            //
            this.pnlBelieverSinceYear.Controls.Add(this.txtBelieverSince);
            this.pnlBelieverSinceYear.Controls.Add(this.lblBelieverSince);
            this.pnlBelieverSinceYear.Controls.Add(this.lblComment);
            this.pnlBelieverSinceYear.Controls.Add(this.txtBelieverComment);
            this.pnlBelieverSinceYear.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBelieverSinceYear.Location = new System.Drawing.Point(3, 17);
            this.pnlBelieverSinceYear.Name = "pnlBelieverSinceYear";
            this.pnlBelieverSinceYear.Size = new System.Drawing.Size(604, 46);
            this.pnlBelieverSinceYear.TabIndex = 5;

            //
            // txtBelieverSince
            //
            this.txtBelieverSince.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtBelieverSince.Location = new System.Drawing.Point(84, 0);
            this.txtBelieverSince.MaxLength = 4;
            this.txtBelieverSince.Name = "txtBelieverSince";
            this.txtBelieverSince.Size = new System.Drawing.Size(39, 21);
            this.txtBelieverSince.TabIndex = 1;
            this.txtBelieverSince.Text = "8888";

            //
            // lblBelieverSince
            //
            this.lblBelieverSince.Location = new System.Drawing.Point(1, 0);
            this.lblBelieverSince.Name = "lblBelieverSince";
            this.lblBelieverSince.Size = new System.Drawing.Size(82, 21);
            this.lblBelieverSince.TabIndex = 0;
            this.lblBelieverSince.Text = "&Year:";
            this.lblBelieverSince.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblComment
            //
            this.lblComment.Location = new System.Drawing.Point(127, 0);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(66, 21);
            this.lblComment.TabIndex = 6;
            this.lblComment.Text = "Comment:";
            this.lblComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtBelieverComment
            //
            this.txtBelieverComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBelieverComment.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtBelieverComment.Location = new System.Drawing.Point(195, 0);
            this.txtBelieverComment.MaxLength = 500;
            this.txtBelieverComment.Multiline = true;
            this.txtBelieverComment.Name = "txtBelieverComment";
            this.txtBelieverComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBelieverComment.Size = new System.Drawing.Size(405, 42);
            this.txtBelieverComment.TabIndex = 2;
            this.txtBelieverComment.Text = "";
            this.txtBelieverComment.TextChanged += new System.EventHandler(this.TxtBelieverComment_TextChanged);

            //
            // TUC_PartnerDetailsPerson
            //
            this.Controls.Add(this.pnlPartnerDetailsPerson);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerDetailsPerson";
            this.Size = new System.Drawing.Size(634, 342);
            this.pnlPartnerDetailsPerson.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlOccupation.ResumeLayout(false);
            this.pnlAcquisition.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlMaritalSince.ResumeLayout(false);
            this.pnlMaritalAcademic.ResumeLayout(false);
            this.pnlBirthDecoration.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.grpBelieverSince.ResumeLayout(false);
            this.pnlBelieverSinceYear.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlPartnerDetailsPerson;
        private System.Windows.Forms.Label lblAcademicTitle;
        private TCmbAutoPopulated cmbMaritalStatus;
        private System.Windows.Forms.Label lblMaritalStatus;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.TextBox txtPreferredName;
        private System.Windows.Forms.Label lblDecorations;
        private System.Windows.Forms.Label lblDateOfBirth;
        private System.Windows.Forms.Label lblLanguageCode;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private System.Windows.Forms.Label lblPreferredName;
        private System.Windows.Forms.TextBox txtDecorations;
        private System.Windows.Forms.TextBox txtAcademicTitle;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.TextBox txtDateOfBirth;
        private TbtnCreated btnCreatedPerson;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Label lblMaritalStatusSince;
        private System.Windows.Forms.TextBox txtMaritalStatusSince;
        private System.Windows.Forms.GroupBox grpBelieverSince;
        private System.Windows.Forms.Label lblBelieverSince;
        private System.Windows.Forms.TextBox txtBelieverSince;
        private System.Windows.Forms.TextBox txtBelieverComment;
        private System.Windows.Forms.Panel pnlBirthDecoration;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Panel pnlMaritalAcademic;
        private System.Windows.Forms.Panel pnlMaritalSince;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Panel pnlAcquisition;
        private System.Windows.Forms.Panel pnlOccupation;
        private System.Windows.Forms.Panel pnlBelieverSinceYear;
        private System.Windows.Forms.Label lblComment;
        private TtxtAutoPopulatedButtonLabel txtOccupationCode;
        private System.Windows.Forms.Label lblMaritalStatusComment;
        private System.Windows.Forms.TextBox txtMaritalStatusComment;
    }
}