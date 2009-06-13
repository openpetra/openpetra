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
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using System.Globalization;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for editing Partner Details for a Partner of Partner Class CHURCH.
    public class TUC_PartnerDetailsChurch : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsChurch;
        private TbtnCreated btnCreatedChurch;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Panel pnlContactPartner;
        private System.Windows.Forms.Panel pnlAcquisitionCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Panel pnlAccomodation;
        private System.Windows.Forms.Label lblAccomodation;
        private System.Windows.Forms.TextBox txtAccomodationSize;
        private System.Windows.Forms.Panel pnlChurchSize;
        private System.Windows.Forms.Label lblChurchSize;
        private System.Windows.Forms.TextBox txtChurchSize;
        private System.Windows.Forms.Panel pnlDenomination;
        private System.Windows.Forms.Label lblDenomination;
        private TCmbAutoPopulated cmbDenominationCode;
        private System.Windows.Forms.CheckBox chkMapOnFile;
        private System.Windows.Forms.CheckBox chkPrayerCell;
        private System.Windows.Forms.CheckBox chkAccomodation;
        private System.Windows.Forms.Label lblAccomodationType;
        private TCmbAutoPopulated cmbAccomodationType;
        private TtxtAutoPopulatedButtonLabel txtContactPartnerBox;
        private TexpTextBoxStringLengthCheck expStringLengthCheckChurch;
        protected new PartnerEditTDS FMainDS;
        protected Boolean FPerformDataBindingDone;
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        private System.Object FSelectedAcquisitionCode;
        public new PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsChurch));
            this.pnlPartnerDetailsChurch = new System.Windows.Forms.Panel();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.pnlContactPartner = new System.Windows.Forms.Panel();
            this.txtContactPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlAcquisitionCode = new System.Windows.Forms.Panel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlAccomodation = new System.Windows.Forms.Panel();
            this.cmbAccomodationType = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccomodationType = new System.Windows.Forms.Label();
            this.chkAccomodation = new System.Windows.Forms.CheckBox();
            this.lblAccomodation = new System.Windows.Forms.Label();
            this.txtAccomodationSize = new System.Windows.Forms.TextBox();
            this.pnlChurchSize = new System.Windows.Forms.Panel();
            this.chkPrayerCell = new System.Windows.Forms.CheckBox();
            this.chkMapOnFile = new System.Windows.Forms.CheckBox();
            this.lblChurchSize = new System.Windows.Forms.Label();
            this.txtChurchSize = new System.Windows.Forms.TextBox();
            this.pnlDenomination = new System.Windows.Forms.Panel();
            this.cmbDenominationCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDenomination = new System.Windows.Forms.Label();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedChurch = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckChurch = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsChurch.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlContactPartner.SuspendLayout();
            this.pnlAcquisitionCode.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlAccomodation.SuspendLayout();
            this.pnlChurchSize.SuspendLayout();
            this.pnlDenomination.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsChurch
            //
            this.pnlPartnerDetailsChurch.AutoScroll = true;
            this.pnlPartnerDetailsChurch.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsChurch.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsChurch.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsChurch.Controls.Add(this.btnCreatedChurch);
            this.pnlPartnerDetailsChurch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsChurch.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsChurch.Name = "pnlPartnerDetailsChurch";
            this.pnlPartnerDetailsChurch.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsChurch.TabIndex = 0;

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlContactPartner);
            this.grpMisc.Controls.Add(this.pnlAcquisitionCode);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlAccomodation);
            this.grpMisc.Controls.Add(this.pnlChurchSize);
            this.grpMisc.Controls.Add(this.pnlDenomination);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 164);
            this.grpMisc.TabIndex = 1;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Miscellaneous:";

            //
            // pnlContactPartner
            //
            this.pnlContactPartner.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContactPartner.Controls.Add(this.txtContactPartnerBox);
            this.pnlContactPartner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContactPartner.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.pnlContactPartner.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlContactPartner.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlContactPartner.Location = new System.Drawing.Point(3, 113);
            this.pnlContactPartner.Name = "pnlContactPartner";
            this.pnlContactPartner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlContactPartner.Size = new System.Drawing.Size(604, 24);
            this.pnlContactPartner.TabIndex = 4;

            //
            // txtContactPartnerBox
            //
            this.txtContactPartnerBox.ASpecialSetting = true;
            this.txtContactPartnerBox.AutomaticallyUpdateDataSource = false;
            this.txtContactPartnerBox.ButtonText = "Contact &Partner";
            this.txtContactPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtContactPartnerBox.ButtonWidth = 108;
            this.txtContactPartnerBox.ListTable = Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtContactPartnerBox.Location = new System.Drawing.Point(15, 0);
            this.txtContactPartnerBox.MaxLength = 32767;
            this.txtContactPartnerBox.Name = "txtContactPartnerBox";
            this.txtContactPartnerBox.PartnerClass = "";
            this.txtContactPartnerBox.PreventFaultyLeaving = false;
            this.txtContactPartnerBox.ReadOnly = false;
            this.txtContactPartnerBox.Size = new System.Drawing.Size(575, 23);
            this.txtContactPartnerBox.TabIndex = 6;
            this.txtContactPartnerBox.TextBoxWidth = 80;
            this.txtContactPartnerBox.VerificationResultCollection = null;

            //
            // pnlAcquisitionCode
            //
            this.pnlAcquisitionCode.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAcquisitionCode.Controls.Add(this.lblAcquisitionCode);
            this.pnlAcquisitionCode.Controls.Add(this.cmbAcquisitionCode);
            this.pnlAcquisitionCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcquisitionCode.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.pnlAcquisitionCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAcquisitionCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlAcquisitionCode.Location = new System.Drawing.Point(3, 89);
            this.pnlAcquisitionCode.Name = "pnlAcquisitionCode";
            this.pnlAcquisitionCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisitionCode.Size = new System.Drawing.Size(604, 24);
            this.pnlAcquisitionCode.TabIndex = 3;

            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(17, 0);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.Size = new System.Drawing.Size(108, 22);
            this.lblAcquisitionCode.TabIndex = 0;
            this.lblAcquisitionCode.Text = "&Acquisition Code:";
            this.lblAcquisitionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(127, 0);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((object)(resources.GetObject("cmbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(512, 22);
            this.cmbAcquisitionCode.TabIndex = 1;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // pnlLanguageCode
            //
            this.pnlLanguageCode.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLanguageCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlLanguageCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 65);
            this.pnlLanguageCode.Name = "pnlLanguageCode";
            this.pnlLanguageCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlLanguageCode.Size = new System.Drawing.Size(604, 24);
            this.pnlLanguageCode.TabIndex = 2;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(17, 0);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(108, 22);
            this.lblLanguageCode.TabIndex = 0;
            this.lblLanguageCode.Text = "Lan&guage Code:";
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbLanguageCode
            //
            this.cmbLanguageCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguageCode.ComboBoxWidth = 57;
            this.cmbLanguageCode.Filter = null;
            this.cmbLanguageCode.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.LanguageCodeList;
            this.cmbLanguageCode.Location = new System.Drawing.Point(127, 0);
            this.cmbLanguageCode.Name = "cmbLanguageCode";
            this.cmbLanguageCode.SelectedItem = ((object)(resources.GetObject("cmbLanguageCode.SelectedItem")));
            this.cmbLanguageCode.SelectedValue = null;
            this.cmbLanguageCode.Size = new System.Drawing.Size(468, 22);
            this.cmbLanguageCode.TabIndex = 1;

            //
            // pnlAccomodation
            //
            this.pnlAccomodation.Controls.Add(this.cmbAccomodationType);
            this.pnlAccomodation.Controls.Add(this.lblAccomodationType);
            this.pnlAccomodation.Controls.Add(this.chkAccomodation);
            this.pnlAccomodation.Controls.Add(this.lblAccomodation);
            this.pnlAccomodation.Controls.Add(this.txtAccomodationSize);
            this.pnlAccomodation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAccomodation.Location = new System.Drawing.Point(3, 137);
            this.pnlAccomodation.Name = "pnlAccomodation";
            this.pnlAccomodation.Size = new System.Drawing.Size(604, 24);
            this.pnlAccomodation.TabIndex = 5;

            //
            // cmbAccomodationType
            //
            this.cmbAccomodationType.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAccomodationType.ComboBoxWidth = 121;
            this.cmbAccomodationType.Filter = null;
            this.cmbAccomodationType.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.AccommodationCodeList;
            this.cmbAccomodationType.Location = new System.Drawing.Point(458, 0);
            this.cmbAccomodationType.Name = "cmbAccomodationType";
            this.cmbAccomodationType.SelectedItem = ((object)(resources.GetObject("cmbAccomodationType.SelectedItem")));
            this.cmbAccomodationType.SelectedValue = null;
            this.cmbAccomodationType.Size = new System.Drawing.Size(128, 22);
            this.cmbAccomodationType.TabIndex = 4;

            //
            // lblAccomodationType
            //
            this.lblAccomodationType.Location = new System.Drawing.Point(420, 0);
            this.lblAccomodationType.Name = "lblAccomodationType";
            this.lblAccomodationType.Size = new System.Drawing.Size(40, 23);
            this.lblAccomodationType.TabIndex = 3;
            this.lblAccomodationType.Text = "&Type:";
            this.lblAccomodationType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // chkAccomodation
            //
            this.chkAccomodation.Location = new System.Drawing.Point(22, 0);
            this.chkAccomodation.Name = "chkAccomodation";
            this.chkAccomodation.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAccomodation.Size = new System.Drawing.Size(120, 24);
            this.chkAccomodation.TabIndex = 0;
            this.chkAccomodation.Text = "Acc&omodation:";
            this.chkAccomodation.CheckedChanged += new System.EventHandler(this.ChkAccomodation_CheckedChanged);

            //
            // lblAccomodation
            //
            this.lblAccomodation.Location = new System.Drawing.Point(174, 0);
            this.lblAccomodation.Name = "lblAccomodation";
            this.lblAccomodation.Size = new System.Drawing.Size(124, 22);
            this.lblAccomodation.TabIndex = 1;
            this.lblAccomodation.Text = "Accomodation S&ize:";
            this.lblAccomodation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtAccomodationSize
            //
            this.txtAccomodationSize.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.txtAccomodationSize.Location = new System.Drawing.Point(300, 0);
            this.txtAccomodationSize.Name = "txtAccomodationSize";
            this.txtAccomodationSize.Size = new System.Drawing.Size(94, 21);
            this.txtAccomodationSize.TabIndex = 2;

            //
            // pnlChurchSize
            //
            this.pnlChurchSize.Controls.Add(this.chkPrayerCell);
            this.pnlChurchSize.Controls.Add(this.chkMapOnFile);
            this.pnlChurchSize.Controls.Add(this.lblChurchSize);
            this.pnlChurchSize.Controls.Add(this.txtChurchSize);
            this.pnlChurchSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChurchSize.Location = new System.Drawing.Point(3, 41);
            this.pnlChurchSize.Name = "pnlChurchSize";
            this.pnlChurchSize.Size = new System.Drawing.Size(604, 24);
            this.pnlChurchSize.TabIndex = 1;

            //
            // chkPrayerCell
            //
            this.chkPrayerCell.Location = new System.Drawing.Point(420, 0);
            this.chkPrayerCell.Name = "chkPrayerCell";
            this.chkPrayerCell.Size = new System.Drawing.Size(104, 24);
            this.chkPrayerCell.TabIndex = 3;
            this.chkPrayerCell.Text = "Pra&yer Cell";

            //
            // chkMapOnFile
            //
            this.chkMapOnFile.Location = new System.Drawing.Point(300, 0);
            this.chkMapOnFile.Name = "chkMapOnFile";
            this.chkMapOnFile.Size = new System.Drawing.Size(104, 24);
            this.chkMapOnFile.TabIndex = 2;
            this.chkMapOnFile.Text = "Ma&p on File";

            //
            // lblChurchSize
            //
            this.lblChurchSize.Location = new System.Drawing.Point(17, 0);
            this.lblChurchSize.Name = "lblChurchSize";
            this.lblChurchSize.Size = new System.Drawing.Size(108, 22);
            this.lblChurchSize.TabIndex = 0;
            this.lblChurchSize.Text = "Ch&urch Size:";
            this.lblChurchSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtChurchSize
            //
            this.txtChurchSize.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChurchSize.Location = new System.Drawing.Point(127, 0);
            this.txtChurchSize.Name = "txtChurchSize";
            this.txtChurchSize.Size = new System.Drawing.Size(58, 21);
            this.txtChurchSize.TabIndex = 1;

            //
            // pnlDenomination
            //
            this.pnlDenomination.Controls.Add(this.cmbDenominationCode);
            this.pnlDenomination.Controls.Add(this.lblDenomination);
            this.pnlDenomination.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDenomination.Location = new System.Drawing.Point(3, 17);
            this.pnlDenomination.Name = "pnlDenomination";
            this.pnlDenomination.Size = new System.Drawing.Size(604, 24);
            this.pnlDenomination.TabIndex = 0;

            //
            // cmbDenominationCode
            //
            this.cmbDenominationCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDenominationCode.ComboBoxWidth = 100;
            this.cmbDenominationCode.Filter = null;
            this.cmbDenominationCode.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.DenominationList;
            this.cmbDenominationCode.Location = new System.Drawing.Point(127, 0);
            this.cmbDenominationCode.Name = "cmbDenominationCode";
            this.cmbDenominationCode.SelectedItem = ((object)(resources.GetObject("cmbDenominationCode.SelectedItem")));
            this.cmbDenominationCode.SelectedValue = null;
            this.cmbDenominationCode.Size = new System.Drawing.Size(468, 22);
            this.cmbDenominationCode.TabIndex = 1;

            //
            // lblDenomination
            //
            this.lblDenomination.Location = new System.Drawing.Point(21, 0);
            this.lblDenomination.Name = "lblDenomination";
            this.lblDenomination.Size = new System.Drawing.Size(104, 22);
            this.lblDenomination.TabIndex = 0;
            this.lblDenomination.Text = "&Denomination:";
            this.lblDenomination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlLocalName);
            this.grpNames.Controls.Add(this.pnlPreferedPreviousName);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 68);
            this.grpNames.TabIndex = 0;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Names:";

            //
            // pnlLocalName
            //
            this.pnlLocalName.Controls.Add(this.lblLocalName);
            this.pnlLocalName.Controls.Add(this.txtLocalName);
            this.pnlLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocalName.Location = new System.Drawing.Point(3, 41);
            this.pnlLocalName.Name = "pnlLocalName";
            this.pnlLocalName.Size = new System.Drawing.Size(604, 24);
            this.pnlLocalName.TabIndex = 1;

            //
            // lblLocalName
            //
            this.lblLocalName.Location = new System.Drawing.Point(17, 0);
            this.lblLocalName.Name = "lblLocalName";
            this.lblLocalName.Size = new System.Drawing.Size(108, 23);
            this.lblLocalName.TabIndex = 0;
            this.lblLocalName.Text = "&Local Name:";
            this.lblLocalName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtLocalName
            //
            this.txtLocalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalName.Location = new System.Drawing.Point(127, 0);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(472, 21);
            this.txtLocalName.TabIndex = 1;

            //
            // pnlPreferedPreviousName
            //
            this.pnlPreferedPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreferedPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreferedPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreferedPreviousName.Location = new System.Drawing.Point(3, 17);
            this.pnlPreferedPreviousName.Name = "pnlPreferedPreviousName";
            this.pnlPreferedPreviousName.Size = new System.Drawing.Size(604, 24);
            this.pnlPreferedPreviousName.TabIndex = 0;

            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(17, 0);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(108, 22);
            this.lblPreviousName.TabIndex = 0;
            this.lblPreviousName.Text = "P&revious Name:";
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreviousName.Location = new System.Drawing.Point(127, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(472, 21);
            this.txtPreviousName.TabIndex = 1;

            //
            // btnCreatedChurch
            //
            this.btnCreatedChurch.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatedChurch.CreatedBy = null;
            this.btnCreatedChurch.DateCreated = new System.DateTime(((long)(0)));
            this.btnCreatedChurch.DateModified = new System.DateTime(((long)(0)));
            this.btnCreatedChurch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedChurch.Image = ((System.Drawing.Image)(resources.GetObject("btnCreatedChurch.Image")));
            this.btnCreatedChurch.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedChurch.ModifiedBy = null;
            this.btnCreatedChurch.Name = "btnCreatedChurch";
            this.btnCreatedChurch.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedChurch.TabIndex = 2;
            this.btnCreatedChurch.Tag = "dontdisable";

            //
            // TUC_PartnerDetailsChurch
            //
            this.Controls.Add(this.pnlPartnerDetailsChurch);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUC_PartnerDetailsChurch";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsChurch.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlContactPartner.ResumeLayout(false);
            this.pnlAcquisitionCode.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlAccomodation.ResumeLayout(false);
            this.pnlAccomodation.PerformLayout();
            this.pnlChurchSize.ResumeLayout(false);
            this.pnlChurchSize.PerformLayout();
            this.pnlDenomination.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlLocalName.PerformLayout();
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.pnlPreferedPreviousName.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerDetailsChurch() : base()
        {
            FPerformDataBindingDone = false;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        #region Public methods

        /// <summary>
        /// Initialises Delegate Function to retrieve a partner key
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateGetPartnerShortName = ADelegateFunction;
        }

        public new void InitialiseUserControl()
        {
            // Names
            txtPreviousName.DataBindings.Add("Text", FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetPreviousNameDBName());
            txtLocalName.DataBindings.Add("Text", FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetPartnerShortNameLocDBName());

            // Miscellaneous
            txtChurchSize.DataBindings.Add("Text", FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetApproximateSizeDBName());
            chkMapOnFile.DataBindings.Add("Checked", FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetMapOnFileDBName());
            chkPrayerCell.DataBindings.Add("Checked", FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetPrayerGroupDBName());
            chkAccomodation.DataBindings.Add("Checked", FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetAccomodationDBName());
            txtAccomodationSize.DataBindings.Add("Text", FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetAccomodationSizeDBName());

            // DataBind AutoPopulatingComboBoxes
            cmbLanguageCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetLanguageCodeDBName());
            cmbAcquisitionCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetAcquisitionCodeDBName());
            cmbDenominationCode.PerformDataBinding(FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetDenominationCodeDBName());
            cmbAccomodationType.PerformDataBinding(FMainDS, PChurchTable.GetTableName() + '.' + PChurchTable.GetAccomodationTypeDBName());
            this.txtContactPartnerBox.PerformDataBinding(FMainDS.PChurch.DefaultView, PChurchTable.GetContactPartnerKeyDBName());

            // messagebox.Show(cmbAccomodation.Get_SelectedText);
            FPerformDataBindingDone = true;
            DisplayAccomodationDetails();
            btnCreatedChurch.UpdateFields(FMainDS.PChurch);

            // Extender Provider
            this.expStringLengthCheckChurch.RetrieveTextboxes(this);

            // Set StatusBar Texts
            SetStatusBarText(txtPreviousName, PPartnerTable.GetPreviousNameHelp());
            SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            SetStatusBarText(txtChurchSize, PChurchTable.GetApproximateSizeHelp());
            SetStatusBarText(chkMapOnFile, PChurchTable.GetMapOnFileHelp());
            SetStatusBarText(chkPrayerCell, PChurchTable.GetPrayerGroupHelp());
            SetStatusBarText(txtContactPartnerBox, PChurchTable.GetContactPartnerKeyHelp());
            SetStatusBarText(chkAccomodation, PChurchTable.GetAccomodationHelp());
            SetStatusBarText(txtAccomodationSize, PChurchTable.GetAccomodationSizeHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            SetStatusBarText(cmbDenominationCode, PChurchTable.GetDenominationCodeHelp());
            SetStatusBarText(cmbAccomodationType, PChurchTable.GetAccomodationTypeHelp());
            #region Verification
            txtContactPartnerBox.VerificationResultCollection = FVerificationResultCollection;
            #endregion
            ApplySecurity();
        }

        #endregion

        #region Helper functions
        private void DisplayAccomodationDetails()
        {
            if (chkAccomodation.Checked == false)
            {
                // Make unvisible
                this.lblAccomodation.Visible = false;
                this.txtAccomodationSize.Visible = false;
                this.lblAccomodationType.Visible = false;
                this.cmbAccomodationType.Visible = false;
            }
            else
            {
                // Make visible
                this.lblAccomodation.Visible = true;
                this.txtAccomodationSize.Visible = true;
                this.lblAccomodationType.Visible = true;
                this.cmbAccomodationType.Visible = true;
            }
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControlGroup(grpNames);
                CustomEnablingDisabling.DisableControl(pnlAcquisitionCode, cmbAcquisitionCode);
                CustomEnablingDisabling.DisableControl(pnlLanguageCode, cmbLanguageCode);
            }

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PChurchTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_church
                CustomEnablingDisabling.DisableControlGroup(pnlDenomination);
                CustomEnablingDisabling.DisableControlGroup(pnlChurchSize);
                CustomEnablingDisabling.DisableControlGroup(pnlContactPartner);
                CustomEnablingDisabling.DisableControlGroup(pnlAccomodation);
            }
        }

        #endregion

        #region Event Handlers
        private void ChkAccomodation_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            DisplayAccomodationDetails();
        }

        private void CmbAcquisitionCode_Leave(System.Object sender, System.EventArgs e)
        {
            DataTable DataCacheAcquisitionCodeTable;
            PAcquisitionRow TmpRow;
            DialogResult UseAlthoughUnassignable;

            try
            {
                // check if the publication selected is valid, if not, gives warning.
                DataCacheAcquisitionCodeTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AcquisitionCodeList);
                TmpRow = (PAcquisitionRow)DataCacheAcquisitionCodeTable.Rows.Find(new Object[] { this.cmbAcquisitionCode.SelectedItem.ToString() });

                if (TmpRow != null)
                {
                    if (TmpRow.ValidAcquisition)
                    {
                        FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                    }
                    else
                    {
                        UseAlthoughUnassignable = MessageBox.Show(
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive1 + " '" +
                            this.cmbAcquisitionCode.SelectedItem.ToString() + "' " +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive2 + "\r\n" +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive3 + "\r\n" + "\r\n" +
                            "Message Number: " + ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE + "\r\n" +
                            "File Name: " + this.GetType().FullName, "Invalid Data Entered",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                        if (UseAlthoughUnassignable == System.Windows.Forms.DialogResult.No)
                        {
                            // If user selects not to use the publication, the recent publication code is selected.
                            this.cmbAcquisitionCode.SelectedItem = FSelectedAcquisitionCode;
                        }
                        else
                        {
                            FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }

    /// <summary>TODO 2 oChristianK : Use the Delegate from Ict.Petra.Client.MCommon.Delegates once this Unit is moved into Ict.Petra.Client.MPartner!</summary>
    public delegate Boolean TDelegateGetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass);
}