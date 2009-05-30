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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MPartner.Verification;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonForms;
using SourceGrid;
using System.Globalization;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl for editing Partner Details for a Partner of Partner Class BANK.
    public class TUC_PartnerDetailsBank : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsBank;
        private TbtnCreated btnCreatedBank;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Panel pnlEPFormat;
        private System.Windows.Forms.Panel pnlBankBranchCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private TCmbAutoPopulated cmbLanguageCode;
        private TexpTextBoxStringLengthCheck expStringLengthCheckBank;
        private System.Windows.Forms.Label lblBankBranchCode;
        private System.Windows.Forms.TextBox txtBankBranchCode;
        private System.Windows.Forms.TextBox txtBicSwiftCode;
        private System.Windows.Forms.Label lblBicCode;
        private System.Windows.Forms.Label lblEPFormatFile;
        private System.Windows.Forms.TextBox txtEPFormatFile;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Panel pnlBICSwiftCode;
        private System.Windows.Forms.Panel pnlContactPartner;
        private TtxtAutoPopulatedButtonLabel txtContactPartnerBox;
        private System.Windows.Forms.Panel pnlAcquisitionCode;
        private System.Windows.Forms.ToolTip tipFields;
        protected TUC_PartnerDetailsBankLogic FLogic;
        protected new PartnerEditTDS FMainDS;
        protected System.Data.DataView FBankDefaultView;
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsBank));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsBank = new System.Windows.Forms.Panel();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.pnlContactPartner = new System.Windows.Forms.Panel();
            this.txtContactPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.pnlAcquisitionCode = new System.Windows.Forms.Panel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.pnlEPFormat = new System.Windows.Forms.Panel();
            this.txtEPFormatFile = new System.Windows.Forms.TextBox();
            this.lblEPFormatFile = new System.Windows.Forms.Label();
            this.pnlBICSwiftCode = new System.Windows.Forms.Panel();
            this.lblBicCode = new System.Windows.Forms.Label();
            this.txtBicSwiftCode = new System.Windows.Forms.TextBox();
            this.pnlBankBranchCode = new System.Windows.Forms.Panel();
            this.txtBankBranchCode = new System.Windows.Forms.TextBox();
            this.lblBankBranchCode = new System.Windows.Forms.Label();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedBank = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckBank = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.tipFields = new System.Windows.Forms.ToolTip(this.components);
            this.pnlPartnerDetailsBank.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlContactPartner.SuspendLayout();
            this.pnlAcquisitionCode.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlEPFormat.SuspendLayout();
            this.pnlBICSwiftCode.SuspendLayout();
            this.pnlBankBranchCode.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsBank
            //
            this.pnlPartnerDetailsBank.AutoScroll = true;
            this.pnlPartnerDetailsBank.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsBank.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsBank.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsBank.Controls.Add(this.btnCreatedBank);
            this.pnlPartnerDetailsBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsBank.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsBank.Name = "pnlPartnerDetailsBank";
            this.pnlPartnerDetailsBank.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsBank.TabIndex = 0;

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlContactPartner);
            this.grpMisc.Controls.Add(this.pnlAcquisitionCode);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlEPFormat);
            this.grpMisc.Controls.Add(this.pnlBICSwiftCode);
            this.grpMisc.Controls.Add(this.pnlBankBranchCode);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 165);
            this.grpMisc.TabIndex = 1;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Bank Information:";

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
            this.pnlContactPartner.Location = new System.Drawing.Point(3, 137);
            this.pnlContactPartner.Name = "pnlContactPartner";
            this.pnlContactPartner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlContactPartner.Size = new System.Drawing.Size(604, 24);
            this.pnlContactPartner.TabIndex = 22;

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
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.pnlAcquisitionCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAcquisitionCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlAcquisitionCode.Location = new System.Drawing.Point(3, 113);
            this.pnlAcquisitionCode.Name = "pnlAcquisitionCode";
            this.pnlAcquisitionCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisitionCode.Size = new System.Drawing.Size(604, 24);
            this.pnlAcquisitionCode.TabIndex = 21;

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
            this.cmbAcquisitionCode.ComboBoxWidth = 100;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(127, 0);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(412, 22);
            this.cmbAcquisitionCode.TabIndex = 13;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // pnlLanguageCode
            //
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 89);
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
            // pnlEPFormat
            //
            this.pnlEPFormat.Controls.Add(this.txtEPFormatFile);
            this.pnlEPFormat.Controls.Add(this.lblEPFormatFile);
            this.pnlEPFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEPFormat.Location = new System.Drawing.Point(3, 65);
            this.pnlEPFormat.Name = "pnlEPFormat";
            this.pnlEPFormat.Size = new System.Drawing.Size(604, 24);
            this.pnlEPFormat.TabIndex = 17;

            //
            // txtEPFormatFile
            //
            this.txtEPFormatFile.Enabled = false;
            this.txtEPFormatFile.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtEPFormatFile.Location = new System.Drawing.Point(127, 0);
            this.txtEPFormatFile.Name = "txtEPFormatFile";
            this.txtEPFormatFile.ReadOnly = true;
            this.txtEPFormatFile.Size = new System.Drawing.Size(120, 21);
            this.txtEPFormatFile.TabIndex = 5;
            this.txtEPFormatFile.Text = "Not implemented";

            //
            // lblEPFormatFile
            //
            this.lblEPFormatFile.Location = new System.Drawing.Point(25, 1);
            this.lblEPFormatFile.Name = "lblEPFormatFile";
            this.lblEPFormatFile.TabIndex = 4;
            this.lblEPFormatFile.Text = "EP fo&rmat file:";
            this.lblEPFormatFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlBICSwiftCode
            //
            this.pnlBICSwiftCode.Controls.Add(this.lblBicCode);
            this.pnlBICSwiftCode.Controls.Add(this.txtBicSwiftCode);
            this.pnlBICSwiftCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBICSwiftCode.Location = new System.Drawing.Point(3, 41);
            this.pnlBICSwiftCode.Name = "pnlBICSwiftCode";
            this.pnlBICSwiftCode.Size = new System.Drawing.Size(604, 24);
            this.pnlBICSwiftCode.TabIndex = 16;

            //
            // lblBicCode
            //
            this.lblBicCode.AccessibleDescription = "";
            this.lblBicCode.Location = new System.Drawing.Point(15, 0);
            this.lblBicCode.Name = "lblBicCode";
            this.lblBicCode.Size = new System.Drawing.Size(110, 23);
            this.lblBicCode.TabIndex = 4;
            this.lblBicCode.Text = "&BIC/SWIFT Code:";
            this.lblBicCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //
            // txtBicSwiftCode
            //
            this.txtBicSwiftCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
            this.txtBicSwiftCode.Location = new System.Drawing.Point(127, 0);
            this.txtBicSwiftCode.Name = "txtBicSwiftCode";
            this.txtBicSwiftCode.Size = new System.Drawing.Size(120, 21);
            this.txtBicSwiftCode.TabIndex = 6;
            this.txtBicSwiftCode.Text = "";

            //
            // pnlBankBranchCode
            //
            this.pnlBankBranchCode.Controls.Add(this.txtBankBranchCode);
            this.pnlBankBranchCode.Controls.Add(this.lblBankBranchCode);
            this.pnlBankBranchCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBankBranchCode.Location = new System.Drawing.Point(3, 17);
            this.pnlBankBranchCode.Name = "pnlBankBranchCode";
            this.pnlBankBranchCode.Size = new System.Drawing.Size(604, 24);
            this.pnlBankBranchCode.TabIndex = 15;

            //
            // txtBankBranchCode
            //
            this.txtBankBranchCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBankBranchCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
            this.txtBankBranchCode.Location = new System.Drawing.Point(127, 0);
            this.txtBankBranchCode.Name = "txtBankBranchCode";
            this.txtBankBranchCode.Size = new System.Drawing.Size(472, 21);
            this.txtBankBranchCode.TabIndex = 5;
            this.txtBankBranchCode.Text = "";

            //
            // lblBankBranchCode
            //
            this.lblBankBranchCode.Location = new System.Drawing.Point(5, 0);
            this.lblBankBranchCode.Name = "lblBankBranchCode";
            this.lblBankBranchCode.Size = new System.Drawing.Size(120, 23);
            this.lblBankBranchCode.TabIndex = 4;
            this.lblBankBranchCode.Text = "Ban&k/Branch Code:";
            this.lblBankBranchCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

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
            // btnCreatedBank
            //
            this.btnCreatedBank.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedBank.CreatedBy = null;
            this.btnCreatedBank.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedBank.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedBank.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedBank.Image = ((System.Drawing.Image)(resources.GetObject("bt" + "nCreatedBank.Image")));
            this.btnCreatedBank.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedBank.ModifiedBy = null;
            this.btnCreatedBank.Name = "btnCreatedBank";
            this.btnCreatedBank.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedBank.TabIndex = 2;
            this.btnCreatedBank.Tag = "dontdisable";

            //
            // TUC_PartnerDetailsBank
            //
            this.Controls.Add(this.pnlPartnerDetailsBank);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerDetailsBank";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsBank.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlContactPartner.ResumeLayout(false);
            this.pnlAcquisitionCode.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlEPFormat.ResumeLayout(false);
            this.pnlBICSwiftCode.ResumeLayout(false);
            this.pnlBankBranchCode.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal
        public TUC_PartnerDetailsBank() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FLogic = new TUC_PartnerDetailsBankLogic();

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

        #endregion

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
            String LocalisedBankBranchLabel;
            String LocalisedBankBranchToolTip;
            String Dummy;

            // var
            // aResolt: System.Boolean;
            // Use Localised Strings for Bank/Branch Code
            LocalisedStrings.GetLocStrBankBranchCode(out LocalisedBankBranchLabel, out LocalisedBankBranchToolTip, out Dummy);
            lblBankBranchCode.Text = LocalisedBankBranchLabel;
            this.tipFields.SetToolTip(this.txtBankBranchCode, LocalisedBankBranchToolTip);
            #region Set up screen logic
            FLogic.MainDS = this.FMainDS;
            FBankDefaultView = FMainDS.PBank.DefaultView;

            // FLogic.DataGrid := this.grdBankAccountDetails;
            // FLogic.PartnerEditUIConnector := this.FPartnerEditUIConnector;
            // aResolt := FLogic.RefreshBankAccountDetailsDataGrid();
            // messagebox.Show('Hello from UC_PartnerDetailsBank.InitialiseUserControl!');
            // messagebox.Show('aResolt' + aResolt.ToString);
            // if (aResolt) then
            // begin
            // FLogic.CreateColumns();
            //
            // FLogic.DataBindGrid();
            // end
            // else
            // begin
            // Do nothing here at first.
            // end;
            #endregion
            #region Databindings of simple controls (TextBoxes, Buttons, ComboBoxes
            this.txtLocalName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameLocDBName());
            this.txtPreviousName.DataBindings.Add("Text", FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetPreviousNameDBName());
            this.txtBankBranchCode.DataBindings.Add("Text", FMainDS, PBankTable.GetTableName() + '.' + PBankTable.GetBranchCodeDBName());
            this.txtBicSwiftCode.DataBindings.Add("Text", FMainDS, PBankTable.GetTableName() + '.' + PBankTable.GetBicDBName());

            // this.txtEPFormatFile.DataBindings.Add('Text', FMainDS, PBankTable.GetTableName() + '.' + PBankTable.GetEpFormatFileDBName());
            this.cmbLanguageCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetLanguageCodeDBName());
            this.cmbAcquisitionCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetAcquisitionCodeDBName());
            this.txtContactPartnerBox.PerformDataBinding(FMainDS.PBank.DefaultView, PBankTable.GetContactPartnerKeyDBName());

            // TLogging.Log('TUC_PartnerDetailsBank.InitialiseUserControl txtContactPartnerBox.DataType: ' + FMainDS.PBank.DefaultView[0][PBankTable.GetContactPartnerKeyDBName()].GetType().FullName, [TLoggingType.ToLogfile]);
            // TLogging.Log('TUC_PartnerDetailsBank.InitialiseUserControl txtContactPartnerBox.DataType: ' + FMainDS.PBank.ColumnContactPartnerKey.GetType().FullName, [TLoggingType.ToLogfile]);
            // TLogging.Log('TUC_PartnerDetailsBank.InitialiseUserControl txtContactPartnerBox.DataType: ' + FMainDS.PBank.Columns[PBankTable.GetContactPartnerKeyDBName()].GetType().FullName, [TLoggingType.ToLogfile]);
            // TLogging.Log('TUC_PartnerDetailsBank.InitialiseUserControl txtContactPartnerBox.DataType: ' + FMainDS.PBank.Columns[PBankTable.GetContactPartnerKeyDBName()].DataType.FullName, [TLoggingType.ToLogfile]);
            this.btnCreatedBank.UpdateFields(FMainDS.PBank);
            #endregion
            #region Verification
            FMainDS.PBank.ColumnChanging += new DataColumnChangeEventHandler(this.OnPBankColumnChanging);
            txtContactPartnerBox.VerificationResultCollection = FVerificationResultCollection;
            #endregion
            #region Additional Extender Providers

            // Extender Provider
            this.expStringLengthCheckBank.RetrieveTextboxes(this);

            // Set StatusBar Texts
            SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());

            // SetStatusBarText(txtContactPartnerBox, POrganisationTable.GetContactPartnerKeyHelp());
            #endregion
            ApplySecurity();
        }

        #endregion

        #region Helper functions
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

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_bank
                CustomEnablingDisabling.DisableControlGroup(pnlBankBranchCode);
                CustomEnablingDisabling.DisableControlGroup(pnlBICSwiftCode);
                CustomEnablingDisabling.DisableControlGroup(pnlEPFormat);
                CustomEnablingDisabling.DisableControlGroup(pnlContactPartner);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// checks that the Acquisition Code is valid.
        /// </summary>
        /// <returns>void</returns>
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
        private void OnPBankColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // TLogging.Log('TUC_PartnerDetailsBank.OnPBankColumnChanging', [TLoggingType.ToLogfile]);
            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerDetailsBankVerification.VerifyBankDetailsData(e, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.FResultCode != ErrorCodes.PETRAERRORCODE_BANKBICSWIFTCODEINVALID)
                    {
                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.FResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.FResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.FResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PBank], e.Column);
                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.FResultText,
                            VerificationResultReturned.FResultTextCaption,
                            VerificationResultReturned.FResultCode,
                            BoundControl,
                            VerificationResultReturned.FResultSeverity);
                        FVerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName];

                        // need to assign this to make the change actually visible...
                        this.txtBicSwiftCode.Text = e.ProposedValue.ToString();

                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.FResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.FResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.FResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtBicSwiftCode.Focus();
                    }
                }
                else
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }

                    if (e.Column.ColumnName == PPartnerTable.GetStatusCodeDBName())
                    {
                        FMainDS.PPartner[0].StatusChange = DateTime.Today;
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

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