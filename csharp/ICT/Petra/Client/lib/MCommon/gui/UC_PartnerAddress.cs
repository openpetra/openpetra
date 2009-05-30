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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Verification;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Controls;
using System.Globalization;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// UserControl that holds the same fields as the address/location part in the
    /// old Progress Partner Edit screen.
    ///
    /// Since this UserControl can be re-used, it is no longer necessary to recreate
    /// all the layout and fields and verification rules in other places than the
    /// Partner Edit screen, eg. in the Personnel Shepherds!
    ///
    /// Interesting feature: this UserControl contains another UserControl -
    /// CountryDropDown_UserControl - which is also DataBound and retrieves its
    /// Country entries on its own from the Client-side DataCache.
    ///
    /// @Comment Have a look at UC_PartnerAddresses to see how DataBinding works
    /// with this UserControl.
    ///
    /// </summary>
    public class TUC_PartnerAddress : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label lblDateGoodUntil;
        private System.Windows.Forms.Label lblDateEffective;
        private System.Windows.Forms.Label lblTelephoneNo;
        private System.Windows.Forms.Label lblLocationType;
        private System.Windows.Forms.Label lblCounty;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblAddLine3;
        private System.Windows.Forms.Label lblStreetName;
        private System.Windows.Forms.Label lblLocality;
        private System.Windows.Forms.TextBox txtTelephoneExt;
        private System.Windows.Forms.TextBox txtTelephoneNo;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.TextBox txtCounty;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtAddLine3;
        private System.Windows.Forms.TextBox txtStreetName;
        private System.Windows.Forms.TextBox txtLocality;
        private System.Windows.Forms.Label lblFaxNo;
        private System.Windows.Forms.TextBox txtFaxNo;
        private System.Windows.Forms.TextBox txtFaxExt;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label lblAltTelephoneNo;
        private System.Windows.Forms.TextBox txtAltTelephoneNo;
        private System.Windows.Forms.TextBox txtMobileTelephoneNo;
        private System.Windows.Forms.Label lblMobileTelephoneNo;
        private System.Windows.Forms.CheckBox chkMailingAddress;
        private System.Windows.Forms.TextBox txtDummyValue;
        private TCmbAutoPopulated cmbLocationType;
        private System.Windows.Forms.GroupBox grpPartnerLocation;
        private System.Windows.Forms.GroupBox grpAddress;
        private System.Windows.Forms.Panel pnlAddress;
        private System.Windows.Forms.Label lblMailingAddress;
        private System.Windows.Forms.TextBox txtValidFrom;
        private System.Windows.Forms.TextBox txtValidTo;
        private TUC_CountryComboBox cmbCountry;
        private TbtnCreated btnCreatedLocation;
        private TbtnCreated btnCreatedPartnerLocation;
        private TexpTextBoxStringLengthCheck expTextBoxStringLengthCheckPartnerAddress;
        private System.Windows.Forms.Panel pnlEmail;
        private System.Windows.Forms.ToolTip ToolTipEmail;

        /// <summary>Holds a reference to the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;

        /// <summary>DataTable Key value for the record we are working with</summary>
        private Int32 FKey;

        /// <summary>DataView for the p_location record we are working with</summary>
        private DataView FLocationDV;

        /// <summary>DataView for the p_partner_location record we are working with</summary>
        private DataView FPartnerLocationDV;

        /// <summary>dmBrowse for readonly mode or dmEdit for edit mode of the UserControl</summary>
        private TDataModeEnum FDataMode;

        /// <summary>Current Address Order (used for optimising the number of TabIndex changes of certain Controls)</summary>
        private Int32 FCurrentAddressOrder;
        private Int32 FLastNonChangedAddressFieldTabIndex;
        private CustomEnablingDisabling.TDelegateDisabledControlClick FDelegateDisabledControlClick;

        /// <summary>DataTable Key value for the record we are working with</summary>
        public Int32 Key
        {
            get
            {
                return FKey;
            }

            set
            {
                FKey = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public CustomEnablingDisabling.TDelegateDisabledControlClick DisabledControlClickHandler
        {
            set
            {
                FDelegateDisabledControlClick = value;
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this
        /// method with the code editor.
        /// /// <summary>/// Required method for Designer support  do not modify/// the contents of this method with the code editor./// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerAddress));
            this.components = new System.ComponentModel.Container();
            this.lblDateGoodUntil = new System.Windows.Forms.Label();
            this.lblDateEffective = new System.Windows.Forms.Label();
            this.lblTelephoneNo = new System.Windows.Forms.Label();
            this.lblLocationType = new System.Windows.Forms.Label();
            this.lblCounty = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.lblPostalCode = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblAddLine3 = new System.Windows.Forms.Label();
            this.lblStreetName = new System.Windows.Forms.Label();
            this.lblLocality = new System.Windows.Forms.Label();
            this.txtTelephoneExt = new System.Windows.Forms.TextBox();
            this.txtTelephoneNo = new System.Windows.Forms.TextBox();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.txtCounty = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtAddLine3 = new System.Windows.Forms.TextBox();
            this.txtStreetName = new System.Windows.Forms.TextBox();
            this.txtLocality = new System.Windows.Forms.TextBox();
            this.lblFaxNo = new System.Windows.Forms.Label();
            this.txtFaxNo = new System.Windows.Forms.TextBox();
            this.txtFaxExt = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.lblAltTelephoneNo = new System.Windows.Forms.Label();
            this.txtAltTelephoneNo = new System.Windows.Forms.TextBox();
            this.txtMobileTelephoneNo = new System.Windows.Forms.TextBox();
            this.lblMobileTelephoneNo = new System.Windows.Forms.Label();
            this.chkMailingAddress = new System.Windows.Forms.CheckBox();
            this.txtDummyValue = new System.Windows.Forms.TextBox();
            this.grpAddress = new System.Windows.Forms.GroupBox();
            this.btnCreatedLocation = new Ict.Common.Controls.TbtnCreated();
            this.cmbCountry = new Ict.Petra.Client.CommonControls.TUC_CountryComboBox();
            this.grpPartnerLocation = new System.Windows.Forms.GroupBox();
            this.pnlEmail = new System.Windows.Forms.Panel();
            this.cmbLocationType = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.btnCreatedPartnerLocation = new Ict.Common.Controls.TbtnCreated();
            this.txtValidFrom = new System.Windows.Forms.TextBox();
            this.lblMailingAddress = new System.Windows.Forms.Label();
            this.txtValidTo = new System.Windows.Forms.TextBox();
            this.pnlAddress = new System.Windows.Forms.Panel();
            this.expTextBoxStringLengthCheckPartnerAddress = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.ToolTipEmail = new System.Windows.Forms.ToolTip(this.components);
            this.grpAddress.SuspendLayout();
            this.grpPartnerLocation.SuspendLayout();
            this.pnlEmail.SuspendLayout();
            this.pnlAddress.SuspendLayout();
            this.SuspendLayout();

            //
            // lblDateGoodUntil
            //
            this.lblDateGoodUntil.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblDateGoodUntil.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDateGoodUntil.Location = new System.Drawing.Point(499, 86);
            this.lblDateGoodUntil.Name = "lblDateGoodUntil";
            this.lblDateGoodUntil.Size = new System.Drawing.Size(94, 17);
            this.lblDateGoodUntil.TabIndex = 19;
            this.lblDateGoodUntil.Text = "Valid To:";
            this.lblDateGoodUntil.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblDateEffective
            //
            this.lblDateEffective.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblDateEffective.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDateEffective.Location = new System.Drawing.Point(499, 64);
            this.lblDateEffective.Name = "lblDateEffective";
            this.lblDateEffective.Size = new System.Drawing.Size(94, 23);
            this.lblDateEffective.TabIndex = 17;
            this.lblDateEffective.Text = "Vali&d From:";
            this.lblDateEffective.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblTelephoneNo
            //
            this.lblTelephoneNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTelephoneNo.Location = new System.Drawing.Point(8, 20);
            this.lblTelephoneNo.Name = "lblTelephoneNo";
            this.lblTelephoneNo.Size = new System.Drawing.Size(72, 23);
            this.lblTelephoneNo.TabIndex = 0;
            this.lblTelephoneNo.Text = "&Phone:";
            this.lblTelephoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblLocationType
            //
            this.lblLocationType.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblLocationType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblLocationType.Location = new System.Drawing.Point(499, 20);
            this.lblLocationType.Name = "lblLocationType";
            this.lblLocationType.Size = new System.Drawing.Size(94, 23);
            this.lblLocationType.TabIndex = 14;
            this.lblLocationType.Text = "&Location Type:";
            this.lblLocationType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblCounty
            //
            this.lblCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblCounty.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCounty.Location = new System.Drawing.Point(260, 86);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(94, 23);
            this.lblCounty.TabIndex = 8;
            this.lblCounty.Text = "County/St&ate:";
            this.lblCounty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblCountry
            //
            this.lblCountry.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCountry.Location = new System.Drawing.Point(8, 130);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(72, 22);
            this.lblCountry.TabIndex = 12;
            this.lblCountry.Text = "Co&untry:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblPostalCode
            //
            this.lblPostalCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPostalCode.Location = new System.Drawing.Point(8, 106);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(72, 23);
            this.lblPostalCode.TabIndex = 10;
            this.lblPostalCode.Text = "Po&st Code:";
            this.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblCity
            //
            this.lblCity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCity.Location = new System.Drawing.Point(8, 86);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(72, 23);
            this.lblCity.TabIndex = 6;
            this.lblCity.Text = "Cit&y/Town:";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblAddLine3
            //
            this.lblAddLine3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblAddLine3.Location = new System.Drawing.Point(8, 62);
            this.lblAddLine3.Name = "lblAddLine3";
            this.lblAddLine3.Size = new System.Drawing.Size(72, 23);
            this.lblAddLine3.TabIndex = 4;
            this.lblAddLine3.Text = "Addr&3:";
            this.lblAddLine3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblStreetName
            //
            this.lblStreetName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblStreetName.Location = new System.Drawing.Point(8, 40);
            this.lblStreetName.Name = "lblStreetName";
            this.lblStreetName.Size = new System.Drawing.Size(72, 23);
            this.lblStreetName.TabIndex = 2;
            this.lblStreetName.Text = "Street-&2:";
            this.lblStreetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblLocality
            //
            this.lblLocality.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblLocality.Location = new System.Drawing.Point(8, 18);
            this.lblLocality.Name = "lblLocality";
            this.lblLocality.Size = new System.Drawing.Size(72, 23);
            this.lblLocality.TabIndex = 0;
            this.lblLocality.Text = "Addr&1:";
            this.lblLocality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtTelephoneExt
            //
            this.txtTelephoneExt.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtTelephoneExt.Location = new System.Drawing.Point(248, 16);
            this.txtTelephoneExt.Name = "txtTelephoneExt";
            this.txtTelephoneExt.Size = new System.Drawing.Size(42, 21);
            this.txtTelephoneExt.TabIndex = 2;
            this.txtTelephoneExt.Tag = "CustomDisableAlthoughInvisible";
            this.txtTelephoneExt.Text = "TelExt";

            //
            // txtTelephoneNo
            //
            this.txtTelephoneNo.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtTelephoneNo.Location = new System.Drawing.Point(84, 16);
            this.txtTelephoneNo.Name = "txtTelephoneNo";
            this.txtTelephoneNo.Size = new System.Drawing.Size(162, 21);
            this.txtTelephoneNo.TabIndex = 1;
            this.txtTelephoneNo.Tag = "CustomDisableAlthoughInvisible";
            this.txtTelephoneNo.Text = "Phone No";

            //
            // txtPostalCode
            //
            this.txtPostalCode.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPostalCode.Location = new System.Drawing.Point(84, 104);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(110, 21);
            this.txtPostalCode.TabIndex = 11;
            this.txtPostalCode.Tag = "CustomDisableAlthoughInvisible";
            this.txtPostalCode.Text = "Postal Code";
            this.txtPostalCode.Validating += new CancelEventHandler(this.TxtPostalCode_Validating);

            //
            // txtCounty
            //
            this.txtCounty.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtCounty.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtCounty.Location = new System.Drawing.Point(360, 82);
            this.txtCounty.Name = "txtCounty";
            this.txtCounty.Size = new System.Drawing.Size(133, 21);
            this.txtCounty.TabIndex = 9;
            this.txtCounty.Tag = "CustomDisableAlthoughInvisible";
            this.txtCounty.Text = "County";

            //
            // txtCity
            //
            this.txtCity.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtCity.Location = new System.Drawing.Point(84, 82);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(170, 21);
            this.txtCity.TabIndex = 7;
            this.txtCity.Tag = "CustomDisableAlthoughInvisible";
            this.txtCity.Text = "City";

            //
            // txtAddLine3
            //
            this.txtAddLine3.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddLine3.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtAddLine3.Location = new System.Drawing.Point(84, 60);
            this.txtAddLine3.Name = "txtAddLine3";
            this.txtAddLine3.Size = new System.Drawing.Size(409, 21);
            this.txtAddLine3.TabIndex = 5;
            this.txtAddLine3.Tag = "CustomDisableAlthoughInvisible";
            this.txtAddLine3.Text = "Address Line 3";

            //
            // txtStreetName
            //
            this.txtStreetName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStreetName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtStreetName.Location = new System.Drawing.Point(84, 38);
            this.txtStreetName.Name = "txtStreetName";
            this.txtStreetName.Size = new System.Drawing.Size(409, 21);
            this.txtStreetName.TabIndex = 3;
            this.txtStreetName.Tag = "CustomDisableAlthoughInvisible";
            this.txtStreetName.Text = "Street Name";

            //
            // txtLocality
            //
            this.txtLocality.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocality.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtLocality.Location = new System.Drawing.Point(84, 16);
            this.txtLocality.Name = "txtLocality";
            this.txtLocality.Size = new System.Drawing.Size(409, 21);
            this.txtLocality.TabIndex = 1;
            this.txtLocality.Tag = "CustomDisableAlthoughInvisible";
            this.txtLocality.Text = "Locality";

            //
            // lblFaxNo
            //
            this.lblFaxNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblFaxNo.Location = new System.Drawing.Point(8, 42);
            this.lblFaxNo.Name = "lblFaxNo";
            this.lblFaxNo.Size = new System.Drawing.Size(72, 23);
            this.lblFaxNo.TabIndex = 3;
            this.lblFaxNo.Text = "Fa&x:";
            this.lblFaxNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtFaxNo
            //
            this.txtFaxNo.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtFaxNo.Location = new System.Drawing.Point(84, 38);
            this.txtFaxNo.Name = "txtFaxNo";
            this.txtFaxNo.Size = new System.Drawing.Size(162, 21);
            this.txtFaxNo.TabIndex = 4;
            this.txtFaxNo.Tag = "CustomDisableAlthoughInvisible";
            this.txtFaxNo.Text = "Fax No";

            //
            // txtFaxExt
            //
            this.txtFaxExt.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtFaxExt.Location = new System.Drawing.Point(248, 38);
            this.txtFaxExt.Name = "txtFaxExt";
            this.txtFaxExt.Size = new System.Drawing.Size(42, 21);
            this.txtFaxExt.TabIndex = 5;
            this.txtFaxExt.Tag = "CustomDisableAlthoughInvisible";
            this.txtFaxExt.Text = "FaxExt";

            //
            // lblEmail
            //
            this.lblEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblEmail.Location = new System.Drawing.Point(2, 4);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(148, 17);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "E&mail:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtEmail
            //
            this.txtEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtEmail.Location = new System.Drawing.Point(2, 22);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(194, 21);
            this.txtEmail.TabIndex = 11;
            this.txtEmail.Tag = "CustomDisableAlthoughInvisible";
            this.txtEmail.Text = "Email";
            this.txtEmail.Validating += new CancelEventHandler(this.TxtEmail_Validating);

            //
            // lblURL
            //
            this.lblURL.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblURL.Location = new System.Drawing.Point(298, 66);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(148, 18);
            this.lblURL.TabIndex = 12;
            this.lblURL.Text = "Websi&te:";
            this.lblURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtURL
            //
            this.txtURL.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtURL.Location = new System.Drawing.Point(296, 82);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(194, 21);
            this.txtURL.TabIndex = 13;
            this.txtURL.Tag = "CustomDisableAlthoughInvisible";
            this.txtURL.Text = "Web URL";

            //
            // lblAltTelephoneNo
            //
            this.lblAltTelephoneNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblAltTelephoneNo.Location = new System.Drawing.Point(8, 86);
            this.lblAltTelephoneNo.Name = "lblAltTelephoneNo";
            this.lblAltTelephoneNo.Size = new System.Drawing.Size(72, 16);
            this.lblAltTelephoneNo.TabIndex = 8;
            this.lblAltTelephoneNo.Text = "Alte&rnate:";
            this.lblAltTelephoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtAltTelephoneNo
            //
            this.txtAltTelephoneNo.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtAltTelephoneNo.Location = new System.Drawing.Point(84, 82);
            this.txtAltTelephoneNo.Name = "txtAltTelephoneNo";
            this.txtAltTelephoneNo.Size = new System.Drawing.Size(206, 21);
            this.txtAltTelephoneNo.TabIndex = 9;
            this.txtAltTelephoneNo.Tag = "CustomDisableAlthoughInvisible";
            this.txtAltTelephoneNo.Text = "Alternate Phone No";

            //
            // txtMobileTelephoneNo
            //
            this.txtMobileTelephoneNo.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtMobileTelephoneNo.Location = new System.Drawing.Point(84, 60);
            this.txtMobileTelephoneNo.Name = "txtMobileTelephoneNo";
            this.txtMobileTelephoneNo.Size = new System.Drawing.Size(206, 21);
            this.txtMobileTelephoneNo.TabIndex = 7;
            this.txtMobileTelephoneNo.Tag = "CustomDisableAlthoughInvisible";
            this.txtMobileTelephoneNo.Text = "Mobile Phone No";

            //
            // lblMobileTelephoneNo
            //
            this.lblMobileTelephoneNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMobileTelephoneNo.Location = new System.Drawing.Point(8, 64);
            this.lblMobileTelephoneNo.Name = "lblMobileTelephoneNo";
            this.lblMobileTelephoneNo.Size = new System.Drawing.Size(72, 23);
            this.lblMobileTelephoneNo.TabIndex = 6;
            this.lblMobileTelephoneNo.Text = "Mo&bile:";
            this.lblMobileTelephoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // chkMailingAddress
            //
            this.chkMailingAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.chkMailingAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkMailingAddress.Location = new System.Drawing.Point(597, 38);
            this.chkMailingAddress.Name = "chkMailingAddress";
            this.chkMailingAddress.Size = new System.Drawing.Size(16, 20);
            this.chkMailingAddress.TabIndex = 16;
            this.chkMailingAddress.Tag = "CustomDisableAlthoughInvisible";

            //
            // txtDummyValue
            //
            this.txtDummyValue.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtDummyValue.Location = new System.Drawing.Point(711, 4);
            this.txtDummyValue.Name = "txtDummyValue";
            this.txtDummyValue.Size = new System.Drawing.Size(24, 21);
            this.txtDummyValue.TabIndex = 35;
            this.txtDummyValue.TabStop = false;
            this.txtDummyValue.Text = "TextBox1";
            this.txtDummyValue.Visible = false;

            //
            // grpAddress
            //
            this.grpAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAddress.Controls.Add(this.btnCreatedLocation);
            this.grpAddress.Controls.Add(this.cmbCountry);
            this.grpAddress.Controls.Add(this.txtLocality);
            this.grpAddress.Controls.Add(this.lblCountry);
            this.grpAddress.Controls.Add(this.lblPostalCode);
            this.grpAddress.Controls.Add(this.lblCity);
            this.grpAddress.Controls.Add(this.lblAddLine3);
            this.grpAddress.Controls.Add(this.lblStreetName);
            this.grpAddress.Controls.Add(this.lblLocality);
            this.grpAddress.Controls.Add(this.txtPostalCode);
            this.grpAddress.Controls.Add(this.txtCounty);
            this.grpAddress.Controls.Add(this.txtCity);
            this.grpAddress.Controls.Add(this.txtAddLine3);
            this.grpAddress.Controls.Add(this.txtStreetName);
            this.grpAddress.Controls.Add(this.lblCounty);
            this.grpAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpAddress.Location = new System.Drawing.Point(4, 2);
            this.grpAddress.Name = "grpAddress";
            this.grpAddress.Size = new System.Drawing.Size(515, 156);
            this.grpAddress.TabIndex = 0;
            this.grpAddress.TabStop = false;
            this.grpAddress.Text = "Address";

            //
            // btnCreatedLocation
            //
            this.btnCreatedLocation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedLocation.CreatedBy = "";
            this.btnCreatedLocation.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedLocation.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedLocation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedLocation.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedLocation.Image")));
            this.btnCreatedLocation.Location = new System.Drawing.Point(497, 12);
            this.btnCreatedLocation.ModifiedBy = "";
            this.btnCreatedLocation.Name = "btnCreatedLocation";
            this.btnCreatedLocation.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedLocation.TabIndex = 14;
            this.btnCreatedLocation.Tag = "dontdisable";

            //
            // cmbCountry
            //
            this.cmbCountry.Location = new System.Drawing.Point(84, 126);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.SelectedValue = null;
            this.cmbCountry.Size = new System.Drawing.Size(322, 22);
            this.cmbCountry.TabIndex = 13;
            this.cmbCountry.Tag = "CustomDisableAlthoughInvisible";

            //
            // grpPartnerLocation
            //
            this.grpPartnerLocation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPartnerLocation.Controls.Add(this.pnlEmail);
            this.grpPartnerLocation.Controls.Add(this.cmbLocationType);
            this.grpPartnerLocation.Controls.Add(this.btnCreatedPartnerLocation);
            this.grpPartnerLocation.Controls.Add(this.txtValidFrom);
            this.grpPartnerLocation.Controls.Add(this.lblTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.lblFaxNo);
            this.grpPartnerLocation.Controls.Add(this.txtFaxNo);
            this.grpPartnerLocation.Controls.Add(this.txtFaxExt);
            this.grpPartnerLocation.Controls.Add(this.txtURL);
            this.grpPartnerLocation.Controls.Add(this.txtTelephoneExt);
            this.grpPartnerLocation.Controls.Add(this.txtAltTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.txtTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.txtMobileTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.lblMobileTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.lblAltTelephoneNo);
            this.grpPartnerLocation.Controls.Add(this.lblURL);
            this.grpPartnerLocation.Controls.Add(this.lblDateGoodUntil);
            this.grpPartnerLocation.Controls.Add(this.chkMailingAddress);
            this.grpPartnerLocation.Controls.Add(this.lblLocationType);
            this.grpPartnerLocation.Controls.Add(this.lblDateEffective);
            this.grpPartnerLocation.Controls.Add(this.lblMailingAddress);
            this.grpPartnerLocation.Controls.Add(this.txtValidTo);
            this.grpPartnerLocation.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpPartnerLocation.Location = new System.Drawing.Point(4, 160);
            this.grpPartnerLocation.Name = "grpPartnerLocation";
            this.grpPartnerLocation.Size = new System.Drawing.Size(731, 110);
            this.grpPartnerLocation.TabIndex = 1;
            this.grpPartnerLocation.TabStop = false;
            this.grpPartnerLocation.Text = "Partner-specific Data for this Address";

            //
            // pnlEmail
            //
            this.pnlEmail.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEmail.Controls.Add(this.lblEmail);
            this.pnlEmail.Controls.Add(this.txtEmail);
            this.pnlEmail.Location = new System.Drawing.Point(294, 16);
            this.pnlEmail.Name = "pnlEmail";
            this.pnlEmail.Size = new System.Drawing.Size(200, 46);
            this.pnlEmail.TabIndex = 10;
            this.pnlEmail.MouseEnter += new System.EventHandler(this.PnlEmail_MouseEnter);

            //
            // cmbLocationType
            //
            this.cmbLocationType.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.cmbLocationType.ComboBoxWidth = 110;
            this.cmbLocationType.Filter = null;
            this.cmbLocationType.ListTable = TCmbAutoPopulated.TListTableEnum.LocationTypeList;
            this.cmbLocationType.Location = new System.Drawing.Point(597, 16);
            this.cmbLocationType.Name = "cmbLocationType";
            this.cmbLocationType.SelectedItem = (System.Object)resources.GetObject('c' + "mbLocationType.SelectedItem");
            this.cmbLocationType.SelectedValue = null;
            this.cmbLocationType.Size = new System.Drawing.Size(111, 22);
            this.cmbLocationType.TabIndex = 15;
            this.cmbLocationType.Tag = "CustomDisableAlthoughInvisible";

            //
            // btnCreatedPartnerLocation
            //
            this.btnCreatedPartnerLocation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedPartnerLocation.CreatedBy = null;
            this.btnCreatedPartnerLocation.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartnerLocation.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartnerLocation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedPartnerLocation.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedPartnerLocation.Image")));
            this.btnCreatedPartnerLocation.Location = new System.Drawing.Point(713, 15);
            this.btnCreatedPartnerLocation.ModifiedBy = null;
            this.btnCreatedPartnerLocation.Name = "btnCreatedPartnerLocation";
            this.btnCreatedPartnerLocation.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedPartnerLocation.TabIndex = 21;
            this.btnCreatedPartnerLocation.Tag = "dontdisable";

            //
            // txtValidFrom
            //
            this.txtValidFrom.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtValidFrom.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtValidFrom.Location = new System.Drawing.Point(597, 60);
            this.txtValidFrom.Name = "txtValidFrom";
            this.txtValidFrom.Size = new System.Drawing.Size(94, 21);
            this.txtValidFrom.TabIndex = 18;
            this.txtValidFrom.Tag = "CustomDisableAlthoughInvisible";
            this.txtValidFrom.Text = "Valid From";

            //
            // lblMailingAddress
            //
            this.lblMailingAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lblMailingAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMailingAddress.Location = new System.Drawing.Point(499, 42);
            this.lblMailingAddress.Name = "lblMailingAddress";
            this.lblMailingAddress.Size = new System.Drawing.Size(94, 23);
            this.lblMailingAddress.TabIndex = 15;
            this.lblMailingAddress.Text = "Mailin&g Address:";
            this.lblMailingAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtValidTo
            //
            this.txtValidTo.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtValidTo.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtValidTo.Location = new System.Drawing.Point(597, 82);
            this.txtValidTo.Name = "txtValidTo";
            this.txtValidTo.Size = new System.Drawing.Size(94, 21);
            this.txtValidTo.TabIndex = 20;
            this.txtValidTo.Tag = "CustomDisableAlthoughInvisible";
            this.txtValidTo.Text = "Valid To";

            //
            // pnlAddress
            //
            this.pnlAddress.AutoScroll = true;
            this.pnlAddress.AutoScrollMinSize = new System.Drawing.Size(650, 280);
            this.pnlAddress.Controls.Add(this.grpAddress);
            this.pnlAddress.Controls.Add(this.txtDummyValue);
            this.pnlAddress.Controls.Add(this.grpPartnerLocation);
            this.pnlAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAddress.Location = new System.Drawing.Point(0, 0);
            this.pnlAddress.Name = "pnlAddress";
            this.pnlAddress.Size = new System.Drawing.Size(739, 360);
            this.pnlAddress.TabIndex = 0;

            //
            // TUC_PartnerAddress
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.AutoScroll = true;
            this.Controls.Add(this.pnlAddress);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerAddress";
            this.Size = new System.Drawing.Size(739, 360);
            this.grpAddress.ResumeLayout(false);
            this.grpPartnerLocation.ResumeLayout(false);
            this.pnlEmail.ResumeLayout(false);
            this.pnlAddress.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Processes SelectedValueChanged Event of the cmbCountry ComboBox.
        ///
        /// </summary>
        /// <param name="Sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void CmbCountry_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            SetAddressFieldOrder();
        }

        /// <summary>
        /// Default Constructor.
        ///
        /// Initialises Fields.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUC_PartnerAddress() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FCurrentAddressOrder = 0;
            FLastNonChangedAddressFieldTabIndex = txtAddLine3.TabIndex;

            // I18N: assign proper font which helps to read asian characters
            txtAddLine3.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtStreetName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtCity.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtCounty.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtLocality.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPostalCode.Font = TAppSettingsManager.GetDefaultBoldFont();
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
            }
        }

        private void PnlEmail_MouseEnter(System.Object sender, System.EventArgs e)
        {
            if (this.txtEmail.TextLength > 25)
            {
                this.ToolTipEmail.SetToolTip(this.pnlEmail, this.txtEmail.Text);
                this.ToolTipEmail.AutomaticDelay = 10000;
                this.ToolTipEmail.AutoPopDelay = 30000;
                this.ToolTipEmail.Active = true;
            }
        }

        private void TxtEmail_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtEmail.Text = txtEmail.Text.Trim();
        }

        /// <summary>
        /// Sets the TabIndexes of certain address fields depending on the Country's
        /// AddressOrder.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetAddressFieldOrder()
        {
            Int32 AddressOrder = 0;

            try
            {
                AddressOrder = TAddressHandling.GetAddressOrder(cmbCountry.SelectedValue.ToString());
            }
            catch (Exception)
            {
            }

            if (AddressOrder == 0)
            {
                // MessageBox.Show('Address Order = 0');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else if (AddressOrder == 1)
            {
                // MessageBox.Show('Address Order = 1');
                if (FCurrentAddressOrder != AddressOrder)
                {
                    // MessageBox.Show('Peforming Address Order change');
                    lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                    txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                    lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                    txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                    lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                    txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                    lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                    cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                    FCurrentAddressOrder = AddressOrder;
                }
            }
            else
            {
                if (AddressOrder == 2)
                {
                    if (FCurrentAddressOrder != AddressOrder)
                    {
                        // MessageBox.Show('Peforming Address Order change');
                        lblCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 1;
                        txtCity.TabIndex = FLastNonChangedAddressFieldTabIndex + 2;
                        lblCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 3;
                        txtCounty.TabIndex = FLastNonChangedAddressFieldTabIndex + 4;
                        lblPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 5;
                        txtPostalCode.TabIndex = FLastNonChangedAddressFieldTabIndex + 6;
                        lblCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 7;
                        cmbCountry.TabIndex = FLastNonChangedAddressFieldTabIndex + 8;
                        FCurrentAddressOrder = AddressOrder;
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SetFocusToFirstField()
        {
            txtLocality.Focus();
        }

        /// <summary>
        /// Processes Validating Event of the txtPostalCode TextBox.
        ///
        /// Just converts the Postal Code to all uppercase characters.
        ///
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void TxtPostalCode_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtPostalCode.Text = txtPostalCode.Text.ToUpper();
        }

        /// <summary>
        /// Default WinForms function, created by the Designer
        ///
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(bool Disposing)
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

        /// <summary>
        /// Processes DataColumnChanging Event of the PPartnerLocation DataTable.
        ///
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void OnPartnerLocationDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerAddressVerification.VerifyPartnerLocationData(e, FPetraUtilsObject.VerificationResultCollection,
                        out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.FResultCode != ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE)
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
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerLocationDV], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();

                        if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                        {
                            FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                        }

                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.FResultText,
                            VerificationResultReturned.FResultTextCaption,
                            VerificationResultReturned.FResultCode,
                            BoundControl,
                            VerificationResultReturned.FResultSeverity);
                        FPetraUtilsObject.VerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        // MessageBox.Show(e.Row[e.Column.ColumnName, DataRowVersion.Current].ToString);
                        // e.ProposedValue := e.Row[e.Column.ColumnName, DataRowVersion.Current];
                        e.ProposedValue = e.Row[e.Column.ColumnName];

                        // need to assign this to make the change actually visible...
                        cmbLocationType.SelectedItem = e.ProposedValue.ToString();
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerLocationDV], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                    {
                        FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        /// <summary>
        /// DataBinds all fields on the UserControl, sets Status Bar Texts and hooks up
        /// an Event that allows data verification.
        ///
        /// This procedure needs to be called when the UserControl is to be DataBound for
        /// the first time.
        ///
        /// </summary>
        /// <param name="AMainDS">DataSet that contains most data that is used on the screen</param>
        /// <param name="AKey">DataTable Key value of the record to which the UserControl should
        /// be DataBound to
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(PartnerEditTDS AMainDS, System.Int32 AKey)
        {
            Binding DateFormatBinding;
            Binding NullableNumberFormatBinding;

            FMainDS = AMainDS;

            // create a DataView for each of the two DataTables
            FLocationDV = new DataView(FMainDS.PLocation);
            FPartnerLocationDV = new DataView(FMainDS.PPartnerLocation);

            // filter each DataView to show only a certain record
            FLocationDV.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + AKey.ToString();
            FPartnerLocationDV.RowFilter = PPartnerLocationTable.GetLocationKeyDBName() + " = " + AKey.ToString();

            // DataBind the controls to the DataViews
            this.txtLocality.DataBindings.Add("Text", FLocationDV, PLocationTable.GetLocalityDBName());
            this.txtStreetName.DataBindings.Add("Text", FLocationDV, PLocationTable.GetStreetNameDBName());
            this.txtAddLine3.DataBindings.Add("Text", FLocationDV, PLocationTable.GetAddress3DBName());
            this.txtCity.DataBindings.Add("Text", FLocationDV, PLocationTable.GetCityDBName());
            this.txtCounty.DataBindings.Add("Text", FLocationDV, PLocationTable.GetCountyDBName());
            this.txtPostalCode.DataBindings.Add("Text", FLocationDV, PLocationTable.GetPostalCodeDBName());
            this.txtDummyValue.DataBindings.Add("Text", FLocationDV, "DUMMY");
            this.chkMailingAddress.DataBindings.Add("Checked", FPartnerLocationDV, PPartnerLocationTable.GetSendMailDBName());
            this.txtTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetTelephoneNumberDBName());
            this.txtFaxNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetFaxNumberDBName());
            this.txtAltTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetAlternateTelephoneDBName());
            this.txtMobileTelephoneNo.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetMobileNumberDBName());
            this.txtEmail.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetEmailAddressDBName());
            this.txtURL.DataBindings.Add("Text", FPartnerLocationDV, PPartnerLocationTable.GetUrlDBName());
            NullableNumberFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetExtensionDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber_2);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtTelephoneExt.DataBindings.Add(NullableNumberFormatBinding);
            NullableNumberFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetFaxExtensionDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber_2);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtFaxExt.DataBindings.Add(NullableNumberFormatBinding);
            DateFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetDateEffectiveDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtValidFrom.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FPartnerLocationDV, PPartnerLocationTable.GetDateGoodUntilDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtValidTo.DataBindings.Add(DateFormatBinding);


            // DataBind AutoPopulatingComboBoxes

            //When these controls are bound, many strange problems happen with
            //data-binding. Towards the goal of not using databind for detail view,
            //we now don't databind these two.


            cmbCountry.InitialiseUserControl();
            cmbCountry.AddNotSetRow("--", "NOT SET");

            cmbLocationType.InitialiseUserControl();

            btnCreatedLocation.UpdateFields(FLocationDV);
            btnCreatedPartnerLocation.UpdateFields(FPartnerLocationDV);

            // Extender Provider
            this.expTextBoxStringLengthCheckPartnerAddress.RetrieveTextboxes(this);

            // Set StatusBar Texts
            FPetraUtilsObject.SetStatusBarText(txtLocality, PLocationTable.GetLocalityHelp());
            FPetraUtilsObject.SetStatusBarText(txtStreetName, PLocationTable.GetStreetNameHelp());
            FPetraUtilsObject.SetStatusBarText(txtAddLine3, PLocationTable.GetAddress3Help());
            FPetraUtilsObject.SetStatusBarText(txtCity, PLocationTable.GetCityHelp());
            FPetraUtilsObject.SetStatusBarText(txtCounty, PLocationTable.GetCountyHelp());
            FPetraUtilsObject.SetStatusBarText(txtPostalCode, PLocationTable.GetPostalCodeHelp());
            FPetraUtilsObject.SetStatusBarText(cmbCountry, PLocationTable.GetCountryCodeHelp());
            FPetraUtilsObject.SetStatusBarText(cmbLocationType, PPartnerLocationTable.GetLocationTypeHelp());
            FPetraUtilsObject.SetStatusBarText(chkMailingAddress, PPartnerLocationTable.GetSendMailHelp());
            FPetraUtilsObject.SetStatusBarText(txtTelephoneNo, PPartnerLocationTable.GetTelephoneNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtFaxNo, PPartnerLocationTable.GetFaxNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtAltTelephoneNo, PPartnerLocationTable.GetAlternateTelephoneHelp());
            FPetraUtilsObject.SetStatusBarText(txtMobileTelephoneNo, PPartnerLocationTable.GetMobileNumberHelp());
            FPetraUtilsObject.SetStatusBarText(txtEmail, PPartnerLocationTable.GetEmailAddressHelp());
            FPetraUtilsObject.SetStatusBarText(txtURL, PPartnerLocationTable.GetUrlHelp());
            FPetraUtilsObject.SetStatusBarText(txtTelephoneExt, PPartnerLocationTable.GetExtensionHelp());
            FPetraUtilsObject.SetStatusBarText(txtFaxExt, PPartnerLocationTable.GetFaxExtensionHelp());
            FPetraUtilsObject.SetStatusBarText(txtValidFrom, PPartnerLocationTable.GetDateEffectiveHelp());
            FPetraUtilsObject.SetStatusBarText(txtValidTo, PPartnerLocationTable.GetDateGoodUntilHelp());

            // hook events that allow data verification
            FMainDS.PPartnerLocation.ColumnChanging += new DataColumnChangeEventHandler(this.OnPartnerLocationDataColumnChanging);
        }

        /// <summary>
        /// Refreshes the data that is displayed in DataBound fields with changed data
        /// from the underlying DataTable.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RefreshDataBoundFields()
        {
            System.Windows.Forms.CurrencyManager LocationsCurrencyManager;
            System.Windows.Forms.CurrencyManager PartnerLocationsCurrencyManager;
            LocationsCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FLocationDV];
            PartnerLocationsCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FPartnerLocationDV];
            LocationsCurrencyManager.Refresh();
            PartnerLocationsCurrencyManager.Refresh();
        }

        /// <summary>
        /// Sets up the Localised County Label and hooks up a data change Event.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            String LocalisedCountyLabel;
            String Dummy;

            // Use Localised String for County Label
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Dummy);
            lblCounty.Text = LocalisedCountyLabel;

            // Hook up data change events
            cmbCountry.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbCountry_SelectedValueChanged);
        }

        /// <summary>
        /// DataBinds all fields on the UserControl to a different record.
        ///
        /// This procedure can be called when the UserControl should be DataBound to a
        /// different record (after the first time).
        ///
        /// </summary>
        /// <param name="AKey">DataTable Key value of the record to which the UserControl
        /// should be DataBound to
        /// </param>
        /// <returns>void</returns>
        public void UpdateDataBinding(Int32 AKey)
        {
            FKey = AKey;

            if (FLocationDV != null)
            {
                // filter each DataView to show only a certain record
                FLocationDV.RowFilter = PLocationTable.GetLocationKeyDBName() + " = " + FKey.ToString();
                FPartnerLocationDV.RowFilter = PPartnerLocationTable.GetLocationKeyDBName() + " = " + FKey.ToString();
                btnCreatedLocation.UpdateFields(FLocationDV);
                btnCreatedPartnerLocation.UpdateFields(FPartnerLocationDV);


                cmbLocationType.SelectedValue = ((PPartnerLocationRow)FPartnerLocationDV[0].Row).LocationType;

                if (((PLocationRow)FLocationDV[0].Row).IsCountryCodeNull())
                {
                    cmbCountry.SelectNotSetRow();
                }
                else
                {
                    cmbCountry.SelectedValue = ((PLocationRow)FLocationDV[0].Row).CountryCode;
                }
            }
        }

        /// <summary>
        /// Used to store the values of any unbound controls back to the dataviews.
        /// </summary>
        public void SaveUnboundControlData()
        {
            if (cmbCountry.NoValueSet())
            {
                ((PLocationRow)FLocationDV[0].Row).SetCountryCodeNull();
            }
            else
            {
                ((PLocationRow)FLocationDV[0].Row).CountryCode = cmbCountry.SelectedValue.ToString();
            }

            ((PPartnerLocationRow)FPartnerLocationDV[0].Row).LocationType = cmbLocationType.SelectedValue.ToString();
        }

        /// <summary>
        /// Switches between 'Read Only Mode' and 'Edit Mode' of t	he UserControl.
        ///
        /// </summary>
        /// <param name="ADataMode">Specify dmBrowse for read-only mode or dmEdit for edit mode.
        /// </param>
        /// <returns>void</returns>
        public void SetMode(TDataModeEnum ADataMode)
        {
            FDataMode = ADataMode;

            // messagebox.show('SetMode (' + aDataMode.ToString("G") + ')');
            if (ADataMode == TDataModeEnum.dmBrowse)
            {
                CustomEnablingDisabling.DisableControlGroup(grpAddress, @HandleDisabledControlClick);
                CustomEnablingDisabling.DisableControlGroup(grpPartnerLocation, @HandleDisabledControlClick);
            }
            else
            {
                CustomEnablingDisabling.EnableControlGroup(grpAddress);
                CustomEnablingDisabling.EnableControlGroup(grpPartnerLocation);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void AdjustLabelControlsAfterResizing()
        {
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpAddress);
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpPartnerLocation);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleDisabledControlClick(System.Object sender, System.EventArgs e)
        {
            if (FDelegateDisabledControlClick != null)
            {
                // Call the Delegate in the Control that instantiated this Control
                FDelegateDisabledControlClick(sender, e);

                // Set the Focus on the clicked control (it is stored in the senders (=Label's) Tag Property...)
                ((Control)((Control)sender).Tag).Focus();
            }
        }

        /// <summary>
        /// Revert all changes made since BeginEdit was called on a DataRow. This
        /// affects the data in the DataTables to which the Controls are DataBound to
        /// and the displayed information in the DataBound Controls.
        ///
        /// Based on the two parameters the procedure also determines whether it needs to
        /// delete the affected DataRows as well.
        ///
        /// </summary>
        /// <param name="ANewFromLocation0">Set to true if a new record was just created when
        /// there was only the Location0 record in the Grid</param>
        /// <param name="ARecordAdded">Set to true if a new there was just created
        /// </param>
        /// <returns>void</returns>
        public void CancelEditing(Boolean ANewFromLocation0, Boolean ARecordAdded)
        {
            System.Windows.Forms.CurrencyManager LocationCurrencyManager;
            System.Windows.Forms.CurrencyManager PartnerLocationCurrencyManager;
            DataRow FPartnerLocationDR;
            DataRow FLocationsDR;
            Boolean DeleteRows;

            // Get CurrencyManager that is associated with the DataTables to which the
            // Controls are DataBound.
            LocationCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FLocationDV];
            PartnerLocationCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FPartnerLocationDV];

            // Revert all changes made since BeginEdit was called on a DataRow. This
            // affects the data in the DataTables to which the Controls are DataBound to
            // and the displayed information in the DataBound Controls.
            LocationCurrencyManager.CancelCurrentEdit();
            PartnerLocationCurrencyManager.CancelCurrentEdit();
            DeleteRows = false;

            // Determine whether we need to delete the affected DataRows as well
            if (FPartnerLocationDV[0].Row.RowState == DataRowState.Added)
            {
                if (!FMainDS.PPartner[0].HasVersion(DataRowVersion.Original))
                {
                    // Cancelling Editing with a New Partner
                    // MessageBox.Show('Cancelling Editing with a New Partner...');
                    if (((new DataView(FMainDS.PPartnerLocation, "", "",
                              DataViewRowState.CurrentRows).Count > 1) || (ANewFromLocation0)) && ARecordAdded)
                    {
                        /*
                         * Both Location and PartnerLocation Rows will be deleted since the
                         * Address is new and it is not the last Address or the new Address was
                         * created when there was only Location 0
                         */

                        // MessageBox.Show('Location and PartnerLocation Rows will be deleted since the Address is new and it' + "\r\n" + 'is not the last Address or the new Address was created when there was only Location 0!');
                        DeleteRows = true;
                    }
                    else
                    {
                    }

                    /*
                     * Both Location and PartnerLocation Rows won't be deleted since the
                     * Address is either not new or it is the last Address
                     */

                    // MessageBox.Show('Location and PartnerLocation Rows won''t be deleted since the Address is either' + "\r\n" + 'not new or it is the last Address!');
                }
                else
                {
                    // Cancelling Editing with an existing Partner
                    // MessageBox.Show('Cancelling Editing with an existing Partner...');
                    if (ARecordAdded)
                    {
                        /*
                         * Both Location and PartnerLocation Rows will be deleted since the
                         * Address is new
                         */

                        // MessageBox.Show('Location and PartnerLocation Rows will be deleted since the Address is new!');
                        DeleteRows = true;
                    }
                }

                if (DeleteRows)
                {
                    // In addition to cancelling the Edit, we also delete the DataRows
                    FPartnerLocationDR = FPartnerLocationDV[0].Row;
                    FLocationsDR = FLocationDV[0].Row;
                    FPartnerLocationDV.Table.Rows.Remove(FPartnerLocationDR);
                    FLocationDV.Table.Rows.Remove(FLocationsDR);
                }
            }
        }
    }
}