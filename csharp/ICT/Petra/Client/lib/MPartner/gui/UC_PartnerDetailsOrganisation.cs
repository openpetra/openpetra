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
using Ict.Common;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using System.Globalization;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl for editing Partner Details for a Partner of Partner Class ORGANISATION.
    public class TUC_PartnerDetailsOrganisation : TPetraUserControl
    {
        public const String StrFoundationDetailsTip = "Foundation details can be found on the Foundati" + "on Details tab.";
        public const String StrSecurityFoundationCreate = "You cannot make an Organisation a 'Foundation' because you" + "\r\n" +
                                                          "do not have access rights to the DEVUSER Module!";
        public const String StrSecurityFoundationDelete = "You cannot change the 'Foundation' status because you" + "\r\n" +
                                                          "are not it's Owner and you do not have access rights to the DEVADMIN Module!";
        public const String StrDeleteQuestion = "You have chosen to delete this record." + "\r\n" + "\r\n" + "Dou you really want to delete it?";
        public const String StrDeleteQuestionTitle = "Delete Foundation?";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsOrganisation;
        private TbtnCreated btnCreatedOrganisation;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Panel pnlContactPartner;
        private System.Windows.Forms.Panel pnlAcquisitionCode;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Panel pnlBusinessCode;
        private System.Windows.Forms.Label lblBusinessCode;
        private TCmbAutoPopulated cmbBusinessCode;
        private System.Windows.Forms.Panel pnlChristianAndFoundation;
        private System.Windows.Forms.CheckBox chkChristianOrganisation;
        private System.Windows.Forms.Label lblLanguageCode;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.CheckBox chkFoundation;
        private System.Windows.Forms.Label lblFoundationInfo;
        private TtxtAutoPopulatedButtonLabel txtContactPartnerBox;
        private TexpTextBoxStringLengthCheck expStringLengthCheckOrganisation;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        protected new PartnerEditTDS FMainDS;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>Tells whether the InitialiseUserControl method was run</summary>
        protected Boolean FUserControlInitialised;
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        private Boolean FRunningFoundationCheckChanged;
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

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        public event TShowTabEventHandler ShowTab;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsOrganisation));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsOrganisation = new System.Windows.Forms.Panel();
            this.lblFoundationInfo = new System.Windows.Forms.Label();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.pnlContactPartner = new System.Windows.Forms.Panel();
            this.txtContactPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlAcquisitionCode = new System.Windows.Forms.Panel();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.pnlBusinessCode = new System.Windows.Forms.Panel();
            this.lblBusinessCode = new System.Windows.Forms.Label();
            this.cmbBusinessCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlChristianAndFoundation = new System.Windows.Forms.Panel();
            this.chkChristianOrganisation = new System.Windows.Forms.CheckBox();
            this.chkFoundation = new System.Windows.Forms.CheckBox();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedOrganisation = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckOrganisation = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsOrganisation.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlContactPartner.SuspendLayout();
            this.pnlAcquisitionCode.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlBusinessCode.SuspendLayout();
            this.pnlChristianAndFoundation.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsOrganisation
            //
            this.pnlPartnerDetailsOrganisation.AutoScroll = true;
            this.pnlPartnerDetailsOrganisation.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsOrganisation.Controls.Add(this.lblFoundationInfo);
            this.pnlPartnerDetailsOrganisation.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsOrganisation.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsOrganisation.Controls.Add(this.btnCreatedOrganisation);
            this.pnlPartnerDetailsOrganisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsOrganisation.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsOrganisation.Name = "pnlPartnerDetailsOrganisation";
            this.pnlPartnerDetailsOrganisation.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsOrganisation.TabIndex = 0;

            //
            // lblFoundationInfo
            //
            this.lblFoundationInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblFoundationInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFoundationInfo.Location = new System.Drawing.Point(465, 104);
            this.lblFoundationInfo.Name = "lblFoundationInfo";
            this.lblFoundationInfo.Size = new System.Drawing.Size(142, 60);
            this.lblFoundationInfo.TabIndex = 4;

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlContactPartner);
            this.grpMisc.Controls.Add(this.pnlAcquisitionCode);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlBusinessCode);
            this.grpMisc.Controls.Add(this.pnlChristianAndFoundation);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 148);
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
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.pnlContactPartner.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlContactPartner.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlContactPartner.Location = new System.Drawing.Point(3, 113);
            this.pnlContactPartner.Name = "pnlContactPartner";
            this.pnlContactPartner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlContactPartner.Size = new System.Drawing.Size(604, 24);
            this.pnlContactPartner.TabIndex = 20;

            //
            // txtContactPartnerBox
            //
            this.txtContactPartnerBox.ASpecialSetting = true;
            this.txtContactPartnerBox.ButtonText = "Contact &Partner";
            this.txtContactPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtContactPartnerBox.ButtonWidth = 108;
            this.txtContactPartnerBox.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtContactPartnerBox.Location = new System.Drawing.Point(17, 0);
            this.txtContactPartnerBox.MaxLength = 32767;
            this.txtContactPartnerBox.Name = "txtContactPartnerBox";
            this.txtContactPartnerBox.PartnerClass = "";
            this.txtContactPartnerBox.PreventFaultyLeaving = false;
            this.txtContactPartnerBox.ReadOnly = false;
            this.txtContactPartnerBox.Size = new System.Drawing.Size(575, 23);
            this.txtContactPartnerBox.TabIndex = 5;
            this.txtContactPartnerBox.TextBoxWidth = 80;
            this.txtContactPartnerBox.VerificationResultCollection = null;

            //
            // pnlAcquisitionCode
            //
            this.pnlAcquisitionCode.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAcquisitionCode.Controls.Add(this.cmbAcquisitionCode);
            this.pnlAcquisitionCode.Controls.Add(this.lblAcquisitionCode);
            this.pnlAcquisitionCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcquisitionCode.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.pnlAcquisitionCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAcquisitionCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlAcquisitionCode.Location = new System.Drawing.Point(3, 89);
            this.pnlAcquisitionCode.Name = "pnlAcquisitionCode";
            this.pnlAcquisitionCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisitionCode.Size = new System.Drawing.Size(604, 24);
            this.pnlAcquisitionCode.TabIndex = 19;

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
            this.cmbAcquisitionCode.TabIndex = 1;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

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
            // pnlLanguageCode
            //
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 65);
            this.pnlLanguageCode.Name = "pnlLanguageCode";
            this.pnlLanguageCode.Size = new System.Drawing.Size(604, 24);
            this.pnlLanguageCode.TabIndex = 18;

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
            this.cmbLanguageCode.TabIndex = 1;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(17, 0);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(108, 24);
            this.lblLanguageCode.TabIndex = 0;
            this.lblLanguageCode.Text = "Lan&guage Code:";
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlBusinessCode
            //
            this.pnlBusinessCode.Controls.Add(this.lblBusinessCode);
            this.pnlBusinessCode.Controls.Add(this.cmbBusinessCode);
            this.pnlBusinessCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBusinessCode.Location = new System.Drawing.Point(3, 41);
            this.pnlBusinessCode.Name = "pnlBusinessCode";
            this.pnlBusinessCode.Size = new System.Drawing.Size(604, 24);
            this.pnlBusinessCode.TabIndex = 17;

            //
            // lblBusinessCode
            //
            this.lblBusinessCode.Location = new System.Drawing.Point(17, 0);
            this.lblBusinessCode.Name = "lblBusinessCode";
            this.lblBusinessCode.Size = new System.Drawing.Size(108, 22);
            this.lblBusinessCode.TabIndex = 0;
            this.lblBusinessCode.Text = "&Business Code:";
            this.lblBusinessCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbBusinessCode
            //
            this.cmbBusinessCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBusinessCode.ComboBoxWidth = 100;
            this.cmbBusinessCode.Filter = null;
            this.cmbBusinessCode.ListTable = TCmbAutoPopulated.TListTableEnum.BusinessCodeList;
            this.cmbBusinessCode.Location = new System.Drawing.Point(127, 0);
            this.cmbBusinessCode.Name = "cmbBusinessCode";
            this.cmbBusinessCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbBusinessCode.SelectedItem")));
            this.cmbBusinessCode.SelectedValue = null;
            this.cmbBusinessCode.Size = new System.Drawing.Size(453, 22);
            this.cmbBusinessCode.TabIndex = 1;

            //
            // pnlChristianAndFoundation
            //
            this.pnlChristianAndFoundation.Controls.Add(this.chkChristianOrganisation);
            this.pnlChristianAndFoundation.Controls.Add(this.chkFoundation);
            this.pnlChristianAndFoundation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChristianAndFoundation.Location = new System.Drawing.Point(3, 17);
            this.pnlChristianAndFoundation.Name = "pnlChristianAndFoundation";
            this.pnlChristianAndFoundation.Size = new System.Drawing.Size(604, 24);
            this.pnlChristianAndFoundation.TabIndex = 16;

            //
            // chkChristianOrganisation
            //
            this.chkChristianOrganisation.Location = new System.Drawing.Point(127, 0);
            this.chkChristianOrganisation.Name = "chkChristianOrganisation";
            this.chkChristianOrganisation.Size = new System.Drawing.Size(163, 24);
            this.chkChristianOrganisation.TabIndex = 0;
            this.chkChristianOrganisation.Text = "&Christian Organisation";

            //
            // chkFoundation
            //
            this.chkFoundation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.chkFoundation.Location = new System.Drawing.Point(442, 4);
            this.chkFoundation.Name = "chkFoundation";
            this.chkFoundation.Size = new System.Drawing.Size(162, 16);
            this.chkFoundation.TabIndex = 1;
            this.chkFoundation.Text = "Fou&ndation";

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
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLocalName.Location = new System.Drawing.Point(127, 0);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(472, 21);
            this.txtLocalName.TabIndex = 1;
            this.txtLocalName.Text = "";

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
            this.txtPreviousName.Location = new System.Drawing.Point(127, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(472, 21);
            this.txtPreviousName.TabIndex = 3;
            this.txtPreviousName.Text = "";

            //
            // btnCreatedOrganisation
            //
            this.btnCreatedOrganisation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedOrganisation.CreatedBy = null;
            this.btnCreatedOrganisation.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedOrganisation.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedOrganisation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedOrganisation.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedOrganisation.Image")));
            this.btnCreatedOrganisation.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedOrganisation.ModifiedBy = null;
            this.btnCreatedOrganisation.Name = "btnCreatedOrganisation";
            this.btnCreatedOrganisation.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedOrganisation.TabIndex = 2;
            this.btnCreatedOrganisation.Tag = "dontdisable";

            //
            // TUC_PartnerDetailsOrganisation
            //
            this.Controls.Add(this.pnlPartnerDetailsOrganisation);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerDetailsOrganisation";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsOrganisation.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlContactPartner.ResumeLayout(false);
            this.pnlAcquisitionCode.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlBusinessCode.ResumeLayout(false);
            this.pnlChristianAndFoundation.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerDetailsOrganisation() : base()
        {
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
            txtLocalName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameLocDBName());
            this.txtPreviousName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPreviousNameDBName());

            // Miscellaneous
            chkChristianOrganisation.DataBindings.Add("Checked", FMainDS.POrganisation, POrganisationTable.GetReligiousDBName());
            chkFoundation.DataBindings.Add("Checked", FMainDS.POrganisation, POrganisationTable.GetFoundationDBName());
            this.txtContactPartnerBox.PerformDataBinding(FMainDS.POrganisation.DefaultView, POrganisationTable.GetContactPartnerKeyDBName());

            // DataBind AutoPopulatingComboBoxes
            cmbAcquisitionCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetAcquisitionCodeDBName());
            cmbBusinessCode.PerformDataBinding(FMainDS.POrganisation, POrganisationTable.GetBusinessCodeDBName());
            cmbLanguageCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetLanguageCodeDBName());
            btnCreatedOrganisation.UpdateFields(FMainDS.POrganisation);

            // Extender Provider
            this.expStringLengthCheckOrganisation.RetrieveTextboxes(this);

            // Set StatusBar Texts
            SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            SetStatusBarText(chkChristianOrganisation, POrganisationTable.GetReligiousHelp());
            SetStatusBarText(chkFoundation, POrganisationTable.GetFoundationHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            SetStatusBarText(cmbBusinessCode, POrganisationTable.GetBusinessCodeHelp());
            SetStatusBarText(txtContactPartnerBox, POrganisationTable.GetContactPartnerKeyHelp());
            this.chkFoundation.CheckedChanged += new EventHandler(this.ChkFoundation_CheckedChanged);
            #region Verification
            txtContactPartnerBox.VerificationResultCollection = FVerificationResultCollection;
            #endregion
            ApplySecurity();
            FUserControlInitialised = true;
        }

        #endregion

        #region Helper functions
        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControl(pnlPreferedPreviousName, txtPreviousName);
                CustomEnablingDisabling.DisableControl(pnlLocalName, txtLocalName);
                CustomEnablingDisabling.DisableControl(pnlAcquisitionCode, cmbAcquisitionCode);
                CustomEnablingDisabling.DisableControl(pnlLanguageCode, cmbLanguageCode);
            }

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_organisation
                CustomEnablingDisabling.DisableControlGroup(pnlChristianAndFoundation);
                CustomEnablingDisabling.DisableControlGroup(pnlBusinessCode);
                CustomEnablingDisabling.DisableControlGroup(pnlContactPartner);
            }
        }

        private Boolean CheckSecurityOKToCreateFoundation()
        {
            Boolean ReturnValue;

            ReturnValue = false;

            if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER)
                || UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))
            {
                // MessageBox.Show('Organisation Tab / Foundation Security Check: User is member of DEVUSER and/or DEVADMIN Module');
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private Boolean CheckSecurityOKToDeleteFoundation()
        {
            Boolean ReturnValue;
            Int64 Owner1Key;
            Int64 Owner2Key;

            ReturnValue = false;

            // Determine Foundation Owner Keys
            if (FMainDS.Tables.Contains(PFoundationTable.GetTableName()))
            {
                // Data for Foundation is loaded, so check on p_foundation record
                if (FMainDS.PFoundation.Rows.Count != 0)
                {
                    if (FMainDS.PFoundation[0].IsOwner1KeyNull())
                    {
                        Owner1Key = 0;
                    }
                    else
                    {
                        Owner1Key = FMainDS.PFoundation[0].Owner1Key;
                    }

                    if (FMainDS.PFoundation[0].IsOwner2KeyNull())
                    {
                        Owner2Key = 0;
                    }
                    else
                    {
                        Owner2Key = FMainDS.PFoundation[0].Owner2Key;
                    }
                }
                else
                {
                    // Foundation record wasn't created yet
                    // (happens when User just ticked 'Foundation' CheckBox)
                    // MessageBox.Show('Organisation Tab / Foundation Security Check: Foundation record wasn''t created yet');
                    Owner1Key = 0;
                    Owner2Key = 0;
                }
            }
            else
            {
                // Data for Foundation isn't loaded, so check on MiscellaneousData record
                // MessageBox.Show('Organisation Tab / Foundation Security Check: Data for Foundation isn''t loaded');
                Owner1Key = FMainDS.MiscellaneousData[0].FoundationOwner1Key;
                Owner2Key = FMainDS.MiscellaneousData[0].FoundationOwner2Key;
            }

            // Check security
            if ((Owner1Key == 0) && (Owner2Key == 0))
            {
                // MessageBox.Show('Organisation Tab / Foundation Security Check: None of the Owners is set.');
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER)
                    || (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN)))
                {
                    // MessageBox.Show('Organisation Tab / Foundation Security Check: User is member of DEVUSER or DEVADMIN Module');
                    ReturnValue = true;
                }
            }
            else
            {
                // MessageBox.Show('Organisation Tab / Foundation Security Check: One of the Owners is set!');
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))
                {
                    // MessageBox.Show('Organisation Tab / Foundation Security Check: User is member of DEVADMIN Module');
                    ReturnValue = true;
                }
                else
                {
                    // MessageBox.Show('Organisation Tab / Foundation Security Check: User is NOT member of DEVADMIN Module');
                    if (UserInfo.GUserInfo.PetraIdentity.PartnerKey == Owner1Key)
                    {
                        // MessageBox.Show('Organisation Tab / Foundation Security Check: User is Owner1');
                        ReturnValue = true;
                    }

                    if (UserInfo.GUserInfo.PetraIdentity.PartnerKey == Owner2Key)
                    {
                        // MessageBox.Show('Organisation Tab / Foundation Security Check: User is Owner2');
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        public new void DataSavingStartedEventFired()
        {
            // MessageBox.Show('TUC_PartnerDetailsOrganisation.DataSavingStartedEventFired');
            // Due to two strange calls to chkFoundation_CheckedChanged while data is
            // beeing saved after the chkFoundation has been checked we must disable the
            // Event Handler so that we don't execute it.
            this.chkFoundation.CheckedChanged -= this.ChkFoundation_CheckedChanged;
        }

        public new void DataSavedEventFired(Boolean ASuccess)
        {
            // MessageBox.Show('TUC_PartnerDetailsOrganisation.DataSavedEventFired');
            if (FUserControlInitialised)
            {
                // Enable this Event Handler again after saving is done.
                this.chkFoundation.CheckedChanged += new EventHandler(this.ChkFoundation_CheckedChanged);
            }
        }

        #endregion

        #region Event handlers
        private void ChkFoundation_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            PFoundationRow NewFoundationsRow;
            int Counter;

            if (!FRunningFoundationCheckChanged)
            {
                if (chkFoundation.Checked)
                {
                    if (CheckSecurityOKToCreateFoundation())
                    {
                        // Create empty Foundation Row as there isn't any yet in the DB
                        if (FMainDS.PFoundation == null)
                        {
                            // MessageBox.Show('Creating PFoundation... Tables');
                            FMainDS.Tables.Add(new PFoundationTable());
                            FMainDS.Tables.Add(new PFoundationProposalTable());
                            FMainDS.Tables.Add(new PFoundationProposalDetailTable());
                            FMainDS.Tables.Add(new PFoundationDeadlineTable());
                            FMainDS.InitVars();
                            FMainDS.PFoundation.InitVars();
                            FMainDS.PFoundationProposal.InitVars();
                            FMainDS.PFoundationProposalDetail.InitVars();
                            FMainDS.PFoundationDeadline.InitVars();
                        }

                        FMainDS.PFoundation.InitVars();
                        NewFoundationsRow = FMainDS.PFoundation.NewRowTyped(true);
                        NewFoundationsRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                        FMainDS.PFoundation.Rows.Add(NewFoundationsRow);
                        lblFoundationInfo.Text = StrFoundationDetailsTip;

                        // MessageBox.Show('FMainDS.PFoundation.Rows.Count: ' + FMainDS.PFoundation.Rows.Count.ToString);
                        OnShowTab(new TShowTabEventArgs("tbpFoundationDetails", true, "tbpPartnerDetails"));
                    }
                    else
                    {
                        TMessages.MsgSecurityException(new ESecurityModuleAccessDeniedException(StrSecurityFoundationCreate), this.GetType());
                        FRunningFoundationCheckChanged = true;
                        chkFoundation.Checked = false;
                        FRunningFoundationCheckChanged = false;
                        lblFoundationInfo.Text = "";
                    }
                }
                else
                {
                    if (CheckSecurityOKToDeleteFoundation())
                    {
                        if (MessageBox.Show(StrDeleteQuestion, StrDeleteQuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            OnShowTab(new TShowTabEventArgs("tbpFoundationDetails", false));

                            // Delete p_foundation record, if it exists
                            if ((FMainDS.PFoundation != null) && (FMainDS.PFoundation.Rows.Count != 0))
                            {
                                // MessageBox.Show('Deleting PFoundation record, it exists on the Client side.');
                                FMainDS.PFoundation.Rows[0].Delete();

                                // Delete p_foundation_deadline record(s), if they exist
                                if ((FMainDS.PFoundationDeadline != null) && (FMainDS.PFoundationDeadline.Rows.Count != 0))
                                {
                                    for (Counter = 0; Counter <= FMainDS.PFoundationDeadline.Rows.Count - 1; Counter += 1)
                                    {
                                        // MessageBox.Show('Deleting PFoundationDeadline record, it exists on the Client side.');
                                        FMainDS.PFoundationDeadline[Counter].Delete();
                                    }

                                    FMainDS.PFoundationDeadline.AcceptChanges();
                                }
                            }
                            else
                            {
                                // MessageBox.Show('Deleting PFoundation record, it wasn''t loaded yet on the Client side.');
                                // Delete p_foundation record (which wasn't loaded yet) from DB
                                // (but only if it was there when the screen was loaded)
                                if (((Boolean)FMainDS.POrganisation[0][POrganisationTable.GetFoundationDBName(), DataRowVersion.Original]) == true)
                                {
                                    // MessageBox.Show('Deleting PFoundation record: loading it before deleting it.');
                                    // Retrieve p_foundation record to be able to delete the
                                    // existing DataRow from the DB (necessary for Optimistic Locking)
                                    FMainDS.Merge(FPartnerEditUIConnector.GetDataFoundation(true));
                                    FMainDS.InitVars();
                                    FMainDS.PFoundation.Rows[0].Delete();
                                }
                            }

                            lblFoundationInfo.Text = "";
                        }
                        else
                        {
                            FRunningFoundationCheckChanged = true;
                            chkFoundation.Checked = true;
                            FRunningFoundationCheckChanged = false;
                        }
                    }
                    else
                    {
                        TMessages.MsgSecurityException(new ESecurityModuleAccessDeniedException(StrSecurityFoundationDelete), this.GetType());
                        FRunningFoundationCheckChanged = true;
                        chkFoundation.Checked = true;
                        FRunningFoundationCheckChanged = false;
                    }
                }
            }
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

        #region Custom Events

        /// <summary>
        /// Custom Events
        /// </summary>
        /// <returns>void</returns>
        protected void OnShowTab(TShowTabEventArgs e)
        {
            if (ShowTab != null)
            {
                ShowTab(this, e);
            }
        }

        #endregion
    }
}