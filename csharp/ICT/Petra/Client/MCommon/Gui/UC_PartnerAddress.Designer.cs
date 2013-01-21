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
using System.ComponentModel;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MCommon.Gui
{
    partial class TUC_PartnerAddress
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
            this.txtValidFrom.Size = new System.Drawing.Size(100, 21);
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
            this.txtValidTo.Size = new System.Drawing.Size(100, 21);
            this.txtValidTo.TabIndex = 20;
            this.txtValidTo.Tag = "CustomDisableAlthoughInvisible";
            this.txtValidTo.Text = "Valid To";

            //
            // pnlAddress
            //
            this.pnlAddress.AutoScroll = true;
            this.pnlAddress.AutoScrollMinSize = new System.Drawing.Size(650, 280);
            this.pnlAddress.Controls.Add(this.grpAddress);
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
    }
}