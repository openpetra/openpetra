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
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for editing Partner Details for a Partner of Partner Class UNIT.
    public class TUC_PartnerDetailsUnit : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsUnit;
        private TbtnCreated btnCreatedUnit;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Panel pnlPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Panel pnlAcquisitionCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Label lblUnitType;
        private System.Windows.Forms.Panel pnlUnitType;
        private TCmbAutoPopulated cmbCountryCode;
        private System.Windows.Forms.Panel pnlCountryCode;
        private System.Windows.Forms.Label lblCampainCode;
        private System.Windows.Forms.TextBox txtxyztbdCode;
        private System.Windows.Forms.Label lblCampainOMMS;
        private System.Windows.Forms.Label lblCountryCode;
        private TCmbAutoPopulated cmbUnitType;
        private System.Windows.Forms.Panel pnlCampains;
        private System.Windows.Forms.GroupBox grpxyztbdInfo;
        private System.Windows.Forms.Panel pnlxyztbdCode;
        private TCmbAutoPopulated cmbCurrencyCode;
        private System.Windows.Forms.Panel pnlxyztbdCosts;
        private System.Windows.Forms.Label lblxyztbdCosts;
        private System.Windows.Forms.TextBox txtxyztbdCosts;
        private System.Windows.Forms.Panel pnlPrimaryOffice;
        private TtxtAutoPopulatedButtonLabel txtPrimaryOffice;
        private TexpTextBoxStringLengthCheck expStringLengthCheckUnit;
        protected new PartnerEditTDS FMainDS;
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsUnit));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsUnit = new System.Windows.Forms.Panel();
            this.pnlCampains = new System.Windows.Forms.Panel();
            this.grpxyztbdInfo = new System.Windows.Forms.GroupBox();
            this.pnlxyztbdCode = new System.Windows.Forms.Panel();
            this.txtxyztbdCode = new System.Windows.Forms.TextBox();
            this.lblCampainCode = new System.Windows.Forms.Label();
            this.pnlxyztbdCosts = new System.Windows.Forms.Panel();
            this.cmbCurrencyCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.txtxyztbdCosts = new System.Windows.Forms.TextBox();
            this.lblxyztbdCosts = new System.Windows.Forms.Label();
            this.pnlPrimaryOffice = new System.Windows.Forms.Panel();
            this.txtPrimaryOffice = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.lblCampainOMMS = new System.Windows.Forms.Label();
            this.pnlAcquisitionCode = new System.Windows.Forms.Panel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlUnitType = new System.Windows.Forms.Panel();
            this.lblUnitType = new System.Windows.Forms.Label();
            this.cmbUnitType = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlCountryCode = new System.Windows.Forms.Panel();
            this.cmbCountryCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCountryCode = new System.Windows.Forms.Label();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedUnit = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckUnit = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsUnit.SuspendLayout();
            this.pnlCampains.SuspendLayout();
            this.grpxyztbdInfo.SuspendLayout();
            this.pnlxyztbdCode.SuspendLayout();
            this.pnlxyztbdCosts.SuspendLayout();
            this.pnlPrimaryOffice.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlAcquisitionCode.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlUnitType.SuspendLayout();
            this.pnlCountryCode.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsUnit
            //
            this.pnlPartnerDetailsUnit.AutoScroll = true;
            this.pnlPartnerDetailsUnit.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsUnit.Controls.Add(this.pnlCampains);
            this.pnlPartnerDetailsUnit.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsUnit.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsUnit.Controls.Add(this.btnCreatedUnit);
            this.pnlPartnerDetailsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsUnit.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsUnit.Name = "pnlPartnerDetailsUnit";
            this.pnlPartnerDetailsUnit.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsUnit.TabIndex = 0;

            //
            // pnlCampains
            //
            this.pnlCampains.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCampains.Controls.Add(this.grpxyztbdInfo);
            this.pnlCampains.Controls.Add(this.pnlPrimaryOffice);
            this.pnlCampains.Location = new System.Drawing.Point(4, 210);
            this.pnlCampains.Name = "pnlCampains";
            this.pnlCampains.Size = new System.Drawing.Size(610, 116);
            this.pnlCampains.TabIndex = 3;

            //
            // grpxyztbdInfo
            //
            this.grpxyztbdInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpxyztbdInfo.Controls.Add(this.pnlxyztbdCode);
            this.grpxyztbdInfo.Controls.Add(this.pnlxyztbdCosts);
            this.grpxyztbdInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpxyztbdInfo.Location = new System.Drawing.Point(0, 0);
            this.grpxyztbdInfo.Name = "grpxyztbdInfo";
            this.grpxyztbdInfo.Size = new System.Drawing.Size(610, 69);
            this.grpxyztbdInfo.TabIndex = 0;
            this.grpxyztbdInfo.TabStop = false;
            this.grpxyztbdInfo.Text = "xyztbd Information";

            //
            // pnlxyztbdCode
            //
            this.pnlxyztbdCode.Controls.Add(this.txtxyztbdCode);
            this.pnlxyztbdCode.Controls.Add(this.lblCampainCode);
            this.pnlxyztbdCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlxyztbdCode.Location = new System.Drawing.Point(3, 17);
            this.pnlxyztbdCode.Name = "pnlxyztbdCode";
            this.pnlxyztbdCode.Size = new System.Drawing.Size(604, 24);
            this.pnlxyztbdCode.TabIndex = 0;

            //
            // txtxyztbdCode
            //
            this.txtxyztbdCode.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtxyztbdCode.Location = new System.Drawing.Point(127, 0);
            this.txtxyztbdCode.Name = "txtxyztbdCode";
            this.txtxyztbdCode.Size = new System.Drawing.Size(150, 21);
            this.txtxyztbdCode.TabIndex = 1;
            this.txtxyztbdCode.Text = "";

            //
            // lblCampainCode
            //
            this.lblCampainCode.Location = new System.Drawing.Point(19, 1);
            this.lblCampainCode.Name = "lblCampainCode";
            this.lblCampainCode.Size = new System.Drawing.Size(104, 22);
            this.lblCampainCode.TabIndex = 0;
            this.lblCampainCode.Text = "Campai&gn Code:";
            this.lblCampainCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlxyztbdCosts
            //
            this.pnlxyztbdCosts.Controls.Add(this.cmbCurrencyCode);
            this.pnlxyztbdCosts.Controls.Add(this.txtxyztbdCosts);
            this.pnlxyztbdCosts.Controls.Add(this.lblxyztbdCosts);
            this.pnlxyztbdCosts.Location = new System.Drawing.Point(3, 41);
            this.pnlxyztbdCosts.Name = "pnlxyztbdCosts";
            this.pnlxyztbdCosts.Size = new System.Drawing.Size(604, 22);
            this.pnlxyztbdCosts.TabIndex = 1;

            //
            // cmbCurrencyCode
            //
            this.cmbCurrencyCode.ComboBoxWidth = 60;
            this.cmbCurrencyCode.Filter = null;
            this.cmbCurrencyCode.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            this.cmbCurrencyCode.Location = new System.Drawing.Point(219, 0);
            this.cmbCurrencyCode.Name = "cmbCurrencyCode";
            this.cmbCurrencyCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbCurrencyCode.SelectedItem")));
            this.cmbCurrencyCode.SelectedValue = null;
            this.cmbCurrencyCode.Size = new System.Drawing.Size(378, 22);
            this.cmbCurrencyCode.TabIndex = 2;

            //
            // txtxyztbdCosts
            //
            this.txtxyztbdCosts.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtxyztbdCosts.Location = new System.Drawing.Point(127, 0);
            this.txtxyztbdCosts.Name = "txtxyztbdCosts";
            this.txtxyztbdCosts.Size = new System.Drawing.Size(92, 21);
            this.txtxyztbdCosts.TabIndex = 1;
            this.txtxyztbdCosts.Text = "Campain Costs";

            //
            // lblxyztbdCosts
            //
            this.lblxyztbdCosts.Location = new System.Drawing.Point(17, 0);
            this.lblxyztbdCosts.Name = "lblxyztbdCosts";
            this.lblxyztbdCosts.Size = new System.Drawing.Size(108, 22);
            this.lblxyztbdCosts.TabIndex = 0;
            this.lblxyztbdCosts.Text = "xyztbd Cos&ts:";
            this.lblxyztbdCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlPrimaryOffice
            //
            this.pnlPrimaryOffice.AccessibleDescription = "";
            this.pnlPrimaryOffice.Controls.Add(this.txtPrimaryOffice);
            this.pnlPrimaryOffice.Location = new System.Drawing.Point(2, 80);
            this.pnlPrimaryOffice.Name = "pnlPrimaryOffice";
            this.pnlPrimaryOffice.Size = new System.Drawing.Size(604, 23);
            this.pnlPrimaryOffice.TabIndex = 2;

            //
            // txtPrimaryOffice
            //
            this.txtPrimaryOffice.ASpecialSetting = true;
            this.txtPrimaryOffice.ButtonText = "&Primary Office";
            this.txtPrimaryOffice.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPrimaryOffice.ButtonWidth = 110;
            this.txtPrimaryOffice.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtPrimaryOffice.Location = new System.Drawing.Point(16, 0);
            this.txtPrimaryOffice.MaxLength = 32767;
            this.txtPrimaryOffice.Name = "txtPrimaryOffice";
            this.txtPrimaryOffice.PartnerClass = "UNIT";
            this.txtPrimaryOffice.PreventFaultyLeaving = false;
            this.txtPrimaryOffice.ReadOnly = false;
            this.txtPrimaryOffice.Size = new System.Drawing.Size(585, 23);
            this.txtPrimaryOffice.TabIndex = 3;
            this.txtPrimaryOffice.TextBoxWidth = 80;
            this.txtPrimaryOffice.VerificationResultCollection = null;

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlAcquisitionCode);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlUnitType);
            this.grpMisc.Controls.Add(this.pnlCountryCode);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 142);
            this.grpMisc.TabIndex = 2;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Miscellaneous";

            //
            // lblCampainOMMS
            //
            this.lblCampainOMMS.Location = new System.Drawing.Point(21, 0);
            this.lblCampainOMMS.Name = "lblCampainOMMS";
            this.lblCampainOMMS.Size = new System.Drawing.Size(104, 21);
            this.lblCampainOMMS.TabIndex = 2;
            this.lblCampainOMMS.Text = "OMSS Co&de:";
            this.lblCampainOMMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

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
            this.pnlAcquisitionCode.Location = new System.Drawing.Point(3, 89);
            this.pnlAcquisitionCode.Name = "pnlAcquisitionCode";
            this.pnlAcquisitionCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisitionCode.Size = new System.Drawing.Size(604, 24);
            this.pnlAcquisitionCode.TabIndex = 19;

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
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(127, 0);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(468, 22);
            this.cmbAcquisitionCode.TabIndex = 1;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // pnlLanguageCode
            //
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 65);
            this.pnlLanguageCode.Name = "pnlLanguageCode";
            this.pnlLanguageCode.Size = new System.Drawing.Size(604, 24);
            this.pnlLanguageCode.TabIndex = 18;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(17, 0);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(108, 22);
            this.lblLanguageCode.TabIndex = 0;
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
            this.cmbLanguageCode.TabIndex = 1;

            //
            // pnlUnitType
            //
            this.pnlUnitType.Controls.Add(this.lblUnitType);
            this.pnlUnitType.Controls.Add(this.cmbUnitType);
            this.pnlUnitType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUnitType.Location = new System.Drawing.Point(3, 41);
            this.pnlUnitType.Name = "pnlUnitType";
            this.pnlUnitType.Size = new System.Drawing.Size(604, 24);
            this.pnlUnitType.TabIndex = 17;

            //
            // lblUnitType
            //
            this.lblUnitType.Location = new System.Drawing.Point(1, 0);
            this.lblUnitType.Name = "lblUnitType";
            this.lblUnitType.Size = new System.Drawing.Size(124, 22);
            this.lblUnitType.TabIndex = 0;
            this.lblUnitType.Text = "Unit T&ype:";
            this.lblUnitType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbUnitType
            //
            this.cmbUnitType.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUnitType.ComboBoxWidth = 90;
            this.cmbUnitType.Filter = null;
            this.cmbUnitType.ListTable = TCmbAutoPopulated.TListTableEnum.UnitTypeList;
            this.cmbUnitType.Location = new System.Drawing.Point(127, 0);
            this.cmbUnitType.Name = "cmbUnitType";
            this.cmbUnitType.SelectedItem = ((System.Object)(resources.GetObject("cmbUn" + "itType.SelectedItem")));
            this.cmbUnitType.SelectedValue = null;
            this.cmbUnitType.Size = new System.Drawing.Size(468, 22);
            this.cmbUnitType.TabIndex = 1;

            //
            // pnlCountryCode
            //
            this.pnlCountryCode.Controls.Add(this.cmbCountryCode);
            this.pnlCountryCode.Controls.Add(this.lblCountryCode);
            this.pnlCountryCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCountryCode.Location = new System.Drawing.Point(3, 17);
            this.pnlCountryCode.Name = "pnlCountryCode";
            this.pnlCountryCode.Size = new System.Drawing.Size(604, 24);
            this.pnlCountryCode.TabIndex = 16;

            //
            // cmbCountryCode
            //
            this.cmbCountryCode.ComboBoxWidth = 50;
            this.cmbCountryCode.Filter = null;
            this.cmbCountryCode.ListTable = TCmbAutoPopulated.TListTableEnum.CountryList;
            this.cmbCountryCode.Location = new System.Drawing.Point(127, 0);
            this.cmbCountryCode.Name = "cmbCountryCode";
            this.cmbCountryCode.SelectedItem = ((System.Object)(resources.GetObject("cm" + "bCountryCode.SelectedItem")));
            this.cmbCountryCode.SelectedValue = null;
            this.cmbCountryCode.Size = new System.Drawing.Size(468, 22);
            this.cmbCountryCode.TabIndex = 2;

            //
            // lblCountryCode
            //
            this.lblCountryCode.Location = new System.Drawing.Point(17, 0);
            this.lblCountryCode.Name = "lblCountryCode";
            this.lblCountryCode.Size = new System.Drawing.Size(108, 22);
            this.lblCountryCode.TabIndex = 0;
            this.lblCountryCode.Text = "C&ountry Code:";
            this.lblCountryCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlLocalName);
            this.grpNames.Controls.Add(this.pnlPreviousName);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 68);
            this.grpNames.TabIndex = 1;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Names";

            //
            // pnlLocalName
            //
            this.pnlLocalName.Controls.Add(this.lblLocalName);
            this.pnlLocalName.Controls.Add(this.txtLocalName);
            this.pnlLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocalName.Location = new System.Drawing.Point(3, 41);
            this.pnlLocalName.Name = "pnlLocalName";
            this.pnlLocalName.Size = new System.Drawing.Size(604, 24);
            this.pnlLocalName.TabIndex = 7;

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
            // pnlPreviousName
            //
            this.pnlPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreviousName.Location = new System.Drawing.Point(3, 17);
            this.pnlPreviousName.Name = "pnlPreviousName";
            this.pnlPreviousName.Size = new System.Drawing.Size(604, 24);
            this.pnlPreviousName.TabIndex = 6;

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
            // btnCreatedUnit
            //
            this.btnCreatedUnit.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedUnit.CreatedBy = null;
            this.btnCreatedUnit.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedUnit.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedUnit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedUnit.Image = ((System.Drawing.Image)(resources.GetObject("bt" + "nCreatedUnit.Image")));
            this.btnCreatedUnit.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedUnit.ModifiedBy = null;
            this.btnCreatedUnit.Name = "btnCreatedUnit";
            this.btnCreatedUnit.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedUnit.TabIndex = 4;
            this.btnCreatedUnit.Tag = "dontdisable";

            //
            // TUC_PartnerDetailsUnit
            //
            this.Controls.Add(this.pnlPartnerDetailsUnit);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerDetailsUnit";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsUnit.ResumeLayout(false);
            this.pnlCampains.ResumeLayout(false);
            this.grpxyztbdInfo.ResumeLayout(false);
            this.pnlxyztbdCode.ResumeLayout(false);
            this.pnlxyztbdCosts.ResumeLayout(false);
            this.pnlPrimaryOffice.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlAcquisitionCode.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlUnitType.ResumeLayout(false);
            this.pnlCountryCode.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreviousName.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerDetailsUnit() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            // Hide primary office field for the moment.
            // Reason: It will be used for the Address Book in the future, but it could
            // confuse users at this time.
            this.txtPrimaryOffice.Hide();
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
        public new void InitialiseUserControl()
        {
            Binding NullableNumberFormatBinding;

            // Hook up the Event that retrieves the ShortName for the PartnerKey of the txtContactPartnerBox TextBox
            txtPreviousName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPreviousNameDBName());
            txtLocalName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameLocDBName());
            txtxyztbdCode.DataBindings.Add("Text", FMainDS.PUnit, PUnitTable.GetXyzTbdCodeDBName());
            this.txtPrimaryOffice.PerformDataBinding(FMainDS.PUnit.DefaultView, PUnitTable.GetPrimaryOfficeDBName());
            NullableNumberFormatBinding = new Binding("Text", FMainDS.PUnit, PUnitTable.GetXyzTbdCostDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.DoubleToZeroableNumber);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.ZeroableNumberToDouble);
            this.txtxyztbdCosts.DataBindings.Add(NullableNumberFormatBinding);

            // DataBind AutoPopulatingComboBoxes
            cmbCountryCode.PerformDataBinding(FMainDS.PUnit, PUnitTable.GetCountryCodeDBName());
            cmbUnitType.PerformDataBinding(FMainDS.PUnit, PUnitTable.GetUnitTypeCodeDBName());
            cmbLanguageCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetLanguageCodeDBName());
            cmbAcquisitionCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetAcquisitionCodeDBName());
            cmbCurrencyCode.PerformDataBinding(FMainDS.PUnit, PUnitTable.GetXyzTbdCostCurrencyCodeDBName());
            btnCreatedUnit.UpdateFields(FMainDS.PUnit);

            // Extender Provider
            this.expStringLengthCheckUnit.RetrieveTextboxes(this);

            // Set StatusBar Texts
            SetStatusBarText(txtPreviousName, PPartnerTable.GetPreviousNameHelp());
            SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            SetStatusBarText(txtxyztbdCode, PUnitTable.GetXyzTbdCodeHelp());
            SetStatusBarText(txtxyztbdCosts, PUnitTable.GetXyzTbdCostHelp());
            SetStatusBarText(txtPrimaryOffice, PUnitTable.GetPrimaryOfficeHelp());
            SetStatusBarText(cmbCountryCode, PUnitTable.GetCountryCodeHelp());
            SetStatusBarText(cmbUnitType, PUnitTable.GetUnitTypeCodeHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            SetStatusBarText(cmbCurrencyCode, PUnitTable.GetXyzTbdCostCurrencyCodeHelp());
            #region Verification
            txtPrimaryOffice.VerificationResultCollection = FVerificationResultCollection;
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
            }

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_unit
                CustomEnablingDisabling.DisableControlGroup(pnlCountryCode);
                CustomEnablingDisabling.DisableControlGroup(pnlUnitType);
                CustomEnablingDisabling.DisableControlGroup(grpxyztbdInfo);
                CustomEnablingDisabling.DisableControlGroup(pnlPrimaryOffice);
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
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive3 + "\r\n" +
                            "\r\n" + "Message Number: " + ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE + "\r\n" +
                            "File Name: " + this.GetType().FullName,
                            "Invalid Data Entered", MessageBoxButtons.YesNo, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button2);

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
}