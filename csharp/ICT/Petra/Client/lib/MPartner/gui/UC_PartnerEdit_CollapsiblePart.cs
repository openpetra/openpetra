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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Globalization;
using Ict.Common;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Verification;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl for the collapsible upper part of the Partner Edit screen.
    /// </summary>
    public class TUC_PartnerEdit_CollapsiblePart : TGrpCollapsible
    {
        /// <summary>todoComment</summary>
        public const String StrUnrecognisedPartnerClass = "Unrecognised Partner Class '";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion1 = "Partner Status change from '{0}' to '{1}':" + "\r\n" +
                                                                   "Should Petra apply this change to all Family Members of this Family?";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion2 = "The Family has the following Family Members:";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion3 = "(Choose 'Cancel' to cancel the change of the Partner Status" + "\r\n" +
                                                                   "for this Partner).";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotionTitle = "Promote Partner Status Change to All Family Members?";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
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

        /// <summary>System.Windows.Forms.TextBox;</summary>
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
        private PartnerEditTDS FMainDS;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private String FPartnerClass;
        private DataView FPartnerDefaultView;
        private TDelegateMaintainWorkerField FDelegateMaintainWorkerField;

        /// <summary>Used for keeping track of data verification errors</summary>
        private TVerificationResultCollection FVerificationResultCollection;

        /// <summary>todoComment</summary>
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

        /// <summary>todoComment</summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }

            set
            {
                FVerificationResultCollection = value;
            }
        }

        /// <summary>
        /// This Event is thrown when the 'main data' of a DataTable for a certain
        /// PartnerClass has changed.
        ///
        /// </summary>
        public event TPartnerClassMainDataChangedHandler PartnerClassMainDataChanged;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
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
            this.cmbAddresseeType.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAddresseeType.SelectedItem")));
            this.cmbAddresseeType.SelectedValue = null;
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
            this.cmbPersonGender.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbPersonGender.SelectedItem")));
            this.cmbPersonGender.SelectedValue = null;
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
            this.SetStatusBarText(this.btnEditWorkerField, "Select " + "Worker Field");
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
            this.cmbPartnerStatus.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbPartnerStatus.SelectedItem")));
            this.cmbPartnerStatus.SelectedValue = null;
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

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerEdit_CollapsiblePart() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // I18N: assign proper font which helps to read asian characters
            txtFamilyTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtFamilyFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtFamilyFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonMiddleName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtOtherName.Font = TAppSettingsManager.GetDefaultBoldFont();
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

        private Boolean PartnerStatusCodeChangePromotion(DataColumnChangeEventArgs e)
        {
            Boolean ReturnValue;
            String FamilyMembersText;
            PartnerEditTDSFamilyMembersTable FamilyMembersDT;
            Int32 Counter;
            Int32 Counter2;
            PartnerEditTDSFamilyMembersRow FamilyMembersDR;
            PartnerEditTDSFamilyMembersInfoForStatusChangeRow FamilyMembersInfoDR;
            DialogResult FamilyMembersResult;
            DataView FamilyMembersDV;

            ReturnValue = true;
            FamilyMembersText = "";

            // Retrieve Family Members from the PetraServer
            FamilyMembersDT = FPartnerEditUIConnector.GetDataFamilyMembers(FMainDS.PPartner[0].PartnerKey, "");
            FamilyMembersDV = new DataView(FamilyMembersDT, "", PPersonTable.GetFamilyIdDBName() + " ASC", DataViewRowState.CurrentRows);

            // Build a formatted String of Family Members' PartnerKeys and ShortNames
            for (Counter = 0; Counter <= FamilyMembersDV.Count - 1; Counter += 1)
            {
                FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter].Row;
                FamilyMembersText = FamilyMembersText + "   " + StringHelper.FormatStrToPartnerKeyString(FamilyMembersDR.PartnerKey.ToString()) +
                                    "   " + FamilyMembersDR.PartnerShortName + Environment.NewLine;
            }

            // If there are Family Members, ...
            if (FamilyMembersText != "")
            {
                // show MessageBox with Family Members to the user, asking whether to promote.
                FamilyMembersResult =
                    MessageBox.Show(String.Format(StrStatusCodeFamilyMembersPromotion1,
                            ((PPartnerRow)e.Row).StatusCode,
                            e.ProposedValue) + Environment.NewLine + Environment.NewLine + StrStatusCodeFamilyMembersPromotion2 +
                        Environment.NewLine +
                        FamilyMembersText + Environment.NewLine + StrStatusCodeFamilyMembersPromotion3, StrStatusCodeFamilyMembersPromotionTitle,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                // Check User's response
                switch (FamilyMembersResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:

                        /*
                         * User wants to promote the Partner StatusCode change to Family
                         * Members: add new DataTable for that purpose if it doesn't exist yet.
                         */
                        if (FMainDS.FamilyMembersInfoForStatusChange == null)
                        {
                            FMainDS.Tables.Add(new PartnerEditTDSFamilyMembersInfoForStatusChangeTable());
                            FMainDS.InitVars();
                        }

                        /*
                         * Remove any existing DataRows so we start from a 'clean slate'
                         * (the user could change the Partner StatusCode more than once...)
                         */
                        FMainDS.FamilyMembersInfoForStatusChange.Rows.Clear();

                        /*
                         * Add the PartnerKeys of the Family Members that we have just displayed
                         * to the user to the DataTable.
                         *
                         * Note: This DataTable will be sent to the PetraServer when the user
                         * saves the Partner. The UIConnector will pick it up and process it.
                         */
                        for (Counter2 = 0; Counter2 <= FamilyMembersDV.Count - 1; Counter2 += 1)
                        {
                            FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter2].Row;
                            FamilyMembersInfoDR = FMainDS.FamilyMembersInfoForStatusChange.NewRowTyped(false);
                            FamilyMembersInfoDR.PartnerKey = FamilyMembersDR.PartnerKey;
                            FMainDS.FamilyMembersInfoForStatusChange.Rows.Add(FamilyMembersInfoDR);
                        }

                        break;

                    case System.Windows.Forms.DialogResult.No:

                        // no promotion wanted > nothing to do
                        // (StatusCode will be changed only for the Family)
                        break;

                    case System.Windows.Forms.DialogResult.Cancel:
                        ReturnValue = false;
                        break;
                }
            }
            else
            {
            }

            // no promotion needed since there are no Family Members
            // (StatusCode will be changed only for the Family)
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Disposing"></param>
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

        #region Public Methods

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMainDS"></param>
        public void PerformDataBinding(PartnerEditTDS AMainDS)
        {
            FMainDS = AMainDS;
            PerformDataBinding();

            // Extender Provider
            this.expStringLengthCheckCollapsiblePart.RetrieveTextboxes(this);
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControlGroup(pnlPartner);
                CustomEnablingDisabling.DisableControlGroup(pnlRightSide);
            }

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_person
                        CustomEnablingDisabling.DisableControlGroup(pnlPerson);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.FAMILY:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PFamilyTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_family
                        CustomEnablingDisabling.DisableControlGroup(pnlFamily);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.CHURCH:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PChurchTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_church
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.ORGANISATION:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_organisation
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.UNIT:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_unit
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.BANK:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_bank
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.VENUE:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PVenueTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_venue
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                default:
                    MessageBox.Show(StrUnrecognisedPartnerClass + FPartnerClass + "'!");
                    break;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void PerformDataBinding()
        {
            Binding DateFormatBinding;

            FPartnerDefaultView = FMainDS.PPartner.DefaultView;

            // Perform DataBinding
            txtPartnerKey.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerKeyDBName());
            txtPartnerClass.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerClassDBName());
            chkNoSolicitations.DataBindings.Add("Checked", FMainDS.PPartner, PPartnerTable.GetNoSolicitationsDBName());
            txtLastGiftDetails.DataBindings.Add("Text", FMainDS.MiscellaneousData, PartnerEditTDSMiscellaneousDataTable.GetLastGiftInfoDBName());

            // DataBind Date fields
            DateFormatBinding = new Binding("Text", FMainDS.PPartner, PPartnerTable.GetStatusChangeDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtStatusChange.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FMainDS,
                PartnerEditTDSMiscellaneousDataTable.GetTableName() + '.' + PartnerEditTDSMiscellaneousDataTable.GetLastGiftDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtLastGiftDate.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FMainDS,
                PartnerEditTDSMiscellaneousDataTable.GetTableName() + '.' + PartnerEditTDSMiscellaneousDataTable.GetLastContactDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtLastContactDate.DataBindings.Add(DateFormatBinding);

            // DataBind AutoPopulatingComboBoxes
            cmbAddresseeType.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetAddresseeTypeCodeDBName());
            cmbPartnerStatus.PerformDataBinding(FPartnerDefaultView, PPartnerTable.GetStatusCodeDBName());

            // Set StatusBar Texts
            SetStatusBarText(chkNoSolicitations, PPartnerTable.GetNoSolicitationsHelp());
            SetStatusBarText(cmbAddresseeType, PPartnerTable.GetAddresseeTypeCodeHelp());
            SetStatusBarText(cmbPartnerStatus, PPartnerTable.GetStatusCodeHelp());
            FMainDS.PPartner.ColumnChanging += new DataColumnChangeEventHandler(this.OnPartnerDataColumnChanging);
            #region Bind and show fields according to Partner Class
            FPartnerClass = FMainDS.PPartner[0].PartnerClass.ToString();

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    txtPersonTitle.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetTitleDBName());
                    txtPersonFirstName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetFirstNameDBName());
                    txtPersonMiddleName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetMiddleName1DBName());
                    txtPersonFamilyName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetFamilyNameDBName());
                    cmbPersonGender.PerformDataBinding(FMainDS.PPerson, PPersonTable.GetGenderDBName());

                    // The following is commented out because currently the Text is set with a call to SetWorkerFieldText from PartnerEdit.pas
                    // txtWorkerField.DataBindings.Add('Text', FMainDS.PPerson, FMainDS.PPerson.GetUnitNameDBName);
                    pnlPerson.Visible = true;
                    pnlWorkerField.Visible = true;
                    txtPartnerClass.BackColor = System.Drawing.Color.Yellow;

                    // Set StatusBar Texts
                    SetStatusBarText(txtPersonTitle, PPersonTable.GetTitleHelp());
                    SetStatusBarText(txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    SetStatusBarText(txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    SetStatusBarText(txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
                    SetStatusBarText(cmbPersonGender, PPersonTable.GetGenderHelp());

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
                    tipMain.SetToolTip(this.txtPersonTitle, PPersonTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    tipMain.SetToolTip(this.txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
                    FMainDS.PPerson.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    this.cmbPersonGender.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbPersonGender_SelectedValueChanged);
                    break;

                case TPartnerClass.FAMILY:
                    txtFamilyTitle.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetTitleDBName());
                    txtFamilyFirstName.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetFirstNameDBName());
                    txtFamilyFamilyName.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetFamilyNameDBName());

                    // The following is commented out because currently the Text is set with a call to SetWorkerFieldText from PartnerEdit.pas
                    // txtWorkerField.DataBindings.Add('Text', FMainDS.PFamily, FMainDS.PFamily.GetUnitNameDBName);
                    pnlFamily.Visible = true;
                    pnlWorkerField.Visible = true;

                    // Set StatusBar Texts
                    SetStatusBarText(txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    SetStatusBarText(txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    SetStatusBarText(txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
                    tipMain.SetToolTip(this.txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());
                    FMainDS.PFamily.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.CHURCH:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PChurch, PChurchTable.GetChurchNameDBName());
                    pnlOther.Visible = true;

                    // Set StatusBar Text
                    SetStatusBarText(txtOtherName, PChurchTable.GetChurchNameHelp());
                    FMainDS.PChurch.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.ORGANISATION:
                    txtOtherName.DataBindings.Add("Text", FMainDS.POrganisation, POrganisationTable.GetOrganisationNameDBName());
                    pnlOther.Visible = true;

                    // Set StatusBar Text
                    SetStatusBarText(txtOtherName, POrganisationTable.GetOrganisationNameHelp());
                    FMainDS.POrganisation.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.UNIT:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PUnit, PUnitTable.GetUnitNameDBName());
                    pnlOther.Visible = true;

                    // Set StatusBar Text
                    SetStatusBarText(txtOtherName, PUnitTable.GetUnitNameHelp());
                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(this.OnUnitDataColumnChanging);
                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.BANK:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PBank, PBankTable.GetBranchNameDBName());
                    pnlOther.Visible = true;

                    // Set StatusBar Text
                    SetStatusBarText(txtOtherName, PBankTable.GetBranchNameHelp());
                    FMainDS.PBank.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.VENUE:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PVenue, PVenueTable.GetVenueNameDBName());
                    pnlOther.Visible = true;

                    // Set StatusBar Text
                    SetStatusBarText(txtOtherName, PVenueTable.GetVenueNameHelp());
                    FMainDS.PVenue.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                default:
                    MessageBox.Show(StrUnrecognisedPartnerClass + FPartnerClass + "'!");
                    break;
            }

            #endregion
            SetupBtnCreated();
            SetupChkNoSolicitations();
            ApplySecurity();
        }

        /// <summary>
        /// Initialises Delegate Function to handle click on the "Change Worker Field" button
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateMaintainWorkerField(TDelegateMaintainWorkerField ADelegateFunction)
        {
            FDelegateMaintainWorkerField = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AIncludePartnerClass"></param>
        /// <returns></returns>
        public String PartnerQuickInfo(Boolean AIncludePartnerClass)
        {
            String TmpString;

            TmpString = txtPartnerKey.Text + "   ";

            if (!FMainDS.PPartner[0].IsPartnerShortNameNull())
            {
                TmpString = TmpString + FMainDS.PPartner[0].PartnerShortName;
            }

            if (AIncludePartnerClass)
            {
                TmpString = TmpString + "   [" + FPartnerClass.ToString() + ']';
            }

            return TmpString;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AWorkerField"></param>
        public void SetWorkerFieldText(String AWorkerField)
        {
            txtWorkerField.Text = AWorkerField;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up the Button that holds the Created and Modified information.
        ///
        /// Since there are always two Tables involved in the data that is displayed in
        /// this UserControl, DateCreated and CreatedBy are taken from the table where
        /// DateCreated is earlier, and DateModified and ModifiedBy are taken from the
        /// table where DateModified is later.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupBtnCreated()
        {
            DateTime DateCreatedPPartner = DateTime.Now;
            DateTime DateModifiedPPartner = DateTime.Now;
            DateTime DateCreatedPartnerClassDependent = DateTime.Now;
            DateTime DateModifiedPartnerClassDependent = DateTime.Now;
            String CreatedByPartnerClassDependent = "";
            String ModifiedByPartnerClassDependent = "";

            #region Determine DateCreated, DateModified, CreatedBy and ModifiedBy according to PartnerClass

            switch (SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass))
            {
                case TPartnerClass.PERSON:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PPerson.ColumnDateCreated, FMainDS.PPerson[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PPerson.ColumnDateModified, FMainDS.PPerson[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PPerson.ColumnCreatedBy, FMainDS.PPerson[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PPerson.ColumnModifiedBy, FMainDS.PPerson[0]);
                    break;

                case TPartnerClass.FAMILY:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PFamily.ColumnDateCreated, FMainDS.PFamily[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PFamily.ColumnDateModified, FMainDS.PFamily[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PFamily.ColumnCreatedBy, FMainDS.PFamily[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PFamily.ColumnModifiedBy, FMainDS.PFamily[0]);
                    break;

                case TPartnerClass.CHURCH:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PChurch.ColumnDateCreated, FMainDS.PChurch[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PChurch.ColumnDateModified, FMainDS.PChurch[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PChurch.ColumnCreatedBy, FMainDS.PChurch[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PChurch.ColumnModifiedBy, FMainDS.PChurch[0]);
                    break;

                case TPartnerClass.ORGANISATION:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.POrganisation.ColumnDateCreated,
                    FMainDS.POrganisation[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.POrganisation.ColumnDateModified,
                    FMainDS.POrganisation[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.POrganisation.ColumnCreatedBy,
                    FMainDS.POrganisation[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.POrganisation.ColumnModifiedBy,
                    FMainDS.POrganisation[0]);
                    break;

                case TPartnerClass.BANK:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PBank.ColumnDateCreated, FMainDS.PBank[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PBank.ColumnDateModified, FMainDS.PBank[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PBank.ColumnCreatedBy, FMainDS.PBank[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PBank.ColumnModifiedBy, FMainDS.PBank[0]);
                    break;

                case TPartnerClass.UNIT:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PUnit.ColumnDateCreated, FMainDS.PUnit[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PUnit.ColumnDateModified, FMainDS.PUnit[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PUnit.ColumnCreatedBy, FMainDS.PUnit[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PUnit.ColumnModifiedBy, FMainDS.PUnit[0]);
                    break;

                case TPartnerClass.VENUE:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PVenue.ColumnDateCreated, FMainDS.PVenue[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PVenue.ColumnDateModified, FMainDS.PVenue[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PVenue.ColumnCreatedBy, FMainDS.PVenue[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PVenue.ColumnModifiedBy, FMainDS.PVenue[0]);
                    break;
            }

            #endregion

            /*
             * Decide on which DateCreated and CreatedBy to display:
             * If PPartner DateCreated is earlier, take them from PPartner, otherwise from
             * the Table according to PartnerClass.
             */
            DateCreatedPPartner = TSaveConvert.DateColumnToDate(FMainDS.PPartner.ColumnDateCreated, FMainDS.PPartner[0]);

            if ((DateCreatedPPartner < DateCreatedPartnerClassDependent) && (DateCreatedPPartner != DateTime.MinValue))
            {
                btnCreatedOverall.DateCreated = DateCreatedPPartner;
                btnCreatedOverall.CreatedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnCreatedBy, FMainDS.PPartner[0]);
            }
            else
            {
                btnCreatedOverall.DateCreated = DateCreatedPartnerClassDependent;
                btnCreatedOverall.CreatedBy = CreatedByPartnerClassDependent;
            }

            /*
             * Decide on which DateModified and ModifiedBy to display:
             * If PPartner DateModified is later, take them from PPartner, otherwise from
             * the Table according to PartnerClass.
             */
            DateModifiedPPartner = TSaveConvert.DateColumnToDate(FMainDS.PPartner.ColumnDateModified, FMainDS.PPartner[0]);

            if ((DateModifiedPPartner > DateModifiedPartnerClassDependent) && (DateModifiedPPartner != DateTime.MinValue))
            {
                btnCreatedOverall.DateModified = DateModifiedPPartner;
                btnCreatedOverall.ModifiedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnModifiedBy, FMainDS.PPartner[0]);
            }
            else
            {
                btnCreatedOverall.DateModified = DateModifiedPartnerClassDependent;
                btnCreatedOverall.ModifiedBy = ModifiedByPartnerClassDependent;
            }
        }

        /// <summary>
        /// Sets the background colour of the CheckBox depending on whether it is
        /// Checked or not.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupChkNoSolicitations()
        {
            if (chkNoSolicitations.Checked)
            {
                chkNoSolicitations.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                chkNoSolicitations.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        #endregion

        #region Event handlers
        private void CmbPersonGender_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            if (cmbPersonGender.SelectedItem.ToString() == "Female")
            {
                cmbAddresseeType.SelectedItem = SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcFEMALE);
            }
            else if (cmbPersonGender.SelectedItem.ToString() == "Male")
            {
                cmbAddresseeType.SelectedItem = SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcMALE);
            }

            /*
             * Also assign the value directly to the databound data field!
             * Strangely enough, this is necessary for the case if the user doesn't TAB out
             * of cmbPersonGender, but uses the mouse to select anything else on the screen
             *except* cmbAddresseeType!
             */
            FMainDS.PPartner[0].AddresseeTypeCode = cmbAddresseeType.SelectedItem.ToString();
        }

        private void ChkNoSolicitations_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            SetupChkNoSolicitations();
        }

        private void OnAnyDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TPartnerClassMainDataChangedEventArgs EventFireArgs;

            // messagebox.show('Column_Changing Event: Column=' + e.Column.ColumnName +
            // '; Column content=' + e.Row[e.Column.ColumnName].ToString +
            // '; ' + e.ProposedValue.ToString);
            // MessageBox.Show('PartnerClass: ' + FPartnerClass.ToString);
            EventFireArgs = new TPartnerClassMainDataChangedEventArgs();
            EventFireArgs.PartnerClass = FPartnerClass;

            if (FPartnerClass == "PERSON")
            {
                if ((e.Column.ColumnName == PPersonTable.GetTitleDBName()) || (e.Column.ColumnName == PPersonTable.GetFirstNameDBName())
                    || (e.Column.ColumnName == PPersonTable.GetMiddleName1DBName()) || (e.Column.ColumnName == PPersonTable.GetFamilyNameDBName()))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtPersonFamilyName.Text,
                        txtPersonTitle.Text,
                        txtPersonFirstName.Text,
                        txtPersonMiddleName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "FAMILY")
            {
                if ((e.Column.ColumnName == PFamilyTable.GetTitleDBName()) || (e.Column.ColumnName == PFamilyTable.GetFirstNameDBName())
                    || (e.Column.ColumnName == PFamilyTable.GetFamilyNameDBName()))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtFamilyFamilyName.Text,
                        txtFamilyTitle.Text,
                        txtFamilyFirstName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "CHURCH")
            {
                if (e.Column.ColumnName == PChurchTable.GetChurchNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                if (e.Column.ColumnName == POrganisationTable.GetOrganisationNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                if (e.Column.ColumnName == PUnitTable.GetUnitNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "BANK")
            {
                if (e.Column.ColumnName == PBankTable.GetBranchNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "VENUE")
            {
                if (e.Column.ColumnName == PVenueTable.GetVenueNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
        }

        private void OnPartnerDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyPartnerData(e, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.FResultCode != ErrorCodes.PETRAERRORCODE_PARTNERSTATUSMERGEDCHANGEUNDONE)
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
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PPartner], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
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
                        cmbPartnerStatus.SelectedItem = e.ProposedValue.ToString();

                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.FResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.FResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.FResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerDefaultView], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }

                    // Business Rule: if the Partner's StatusCode changes, give the user the
                    // option to promote the change to all Family Members (if the Partner is
                    // a FAMILY and has Family Members).
                    if (e.Column.ColumnName == PPartnerTable.GetStatusCodeDBName())
                    {
                        if (PartnerStatusCodeChangePromotion(e))
                        {
                            // Set the StatusChange date (this would be done on the server side
                            // automatically, but we want to display it now for immediate user feedback)
                            FMainDS.PPartner[0].StatusChange = DateTime.Today;
                        }
                        else
                        {
                            // User wants to cancel the change of the Partner StatusCode
                            // Undo the change in the DataColumn
                            e.ProposedValue = e.Row[e.Column.ColumnName];

                            // Need to assign this to make the change actually visible...
                            cmbPartnerStatus.SelectedItem = e.ProposedValue.ToString();
                        }
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void OnUnitDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyUnitData(e, FMainDS, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.FResultCode != ErrorCodes.PETRAERRORCODE_UNITNAMECHANGEUNDONE)
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
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
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
                        e.ProposedValue = e.Row[e.Column.ColumnName, DataRowVersion.Original];

                        // need to assign this to make the change actually visible...
                        txtOtherName.Text = e.ProposedValue.ToString();
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void BtnEditWorkerField_Click(System.Object sender, System.EventArgs e)
        {
            if (this.FDelegateMaintainWorkerField != null)
            {
                try
                {
                    this.FDelegateMaintainWorkerField();
                }
                finally
                {
                }

                // raise EVerificationMissing.Create('this.FDelegateGetPartnerShortName could not be called!');
            }
        }

        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
// TODO balloontip
#if TODO
            TBalloonTip BalloonTip;

            // Show general Tip about a number of changes to Tab Headers if the Tips for
            // these Tab Headers haven't been shown yet.
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersGeneral) == '!')
            {
                if ((TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersAddresses) == '!')
                    && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) == '!')
                    && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersNotes) == '!'))
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersGeneral);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "Petra Version 2.2.7 Change: Counters in Tab Headers",
                        "The counters in the tab headers of the Addresses and Subscription Tabs work different than before." + "\r\n" +
                        "The Notes tab header now also has an indicator that shows whether Partner Notes are entered, or not." + "\r\n" +
                        "Switch to these tabs to see an explanation of the changes.",
                        pnlBalloonTipAnchorTabs);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }
                else
                {
                    // since the precondition was not met, we don't need to show this User Tip anymore
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersGeneral);
                }
            }

            // The following Tips are only shown after all User Tips for the Tab Headers
            // have been shown  this is done to prevent those User Tips from coming up at
            // the same time than a User Tip from the Tabs, which wouldn't look nice.
            if ((TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersAddresses) != '!')
                && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) != '!')
                && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersNotes) != '!'))
            {
                // Show Tip about Video Tutorial.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatPartnerEditVideoTutorial) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatPartnerEditVideoTutorial);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.7: Video Tutorial",
                        "The Video Tutorial that is available for the Partner Edit screen can now be launched directly from the" + "\r\n" +
                        "Partner Edit screen. Choose 'Video Tutorial for Partner Edit screen...' from the 'Help' menu to see the video.",
                        pnlBalloonTipAnchorHelp);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }

                // Show Tip about PartnerStatus promotion for Partner of Class FAMILY.
                // This Tip is only shown after the User Tip for the Video Tutorial has been shown
                // this is done to prevent this User Tip coming up at the same time than the
                // User Tip from the Video Tutorial, which wouldn't look nice.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewPromotePartnerStatusChange) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewPromotePartnerStatusChange);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.8: Change of 'Partner Status' for a FAMILY",
                        "When changing the 'Partner Status' for a FAMILY: if the FAMILY has Family Members, you are now" + "\r\n" +
                        "asked whether this change should be applied to the Partner Statuses of all Family Members." + "\r\n" +
                        "This will help in keeping the Partner Statuses of FAMILYs and their Family Members in sync.",
                        cmbPartnerStatus);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }

                // Show Tip about Deactivate Partner Dialog.
                // This Tip is only shown after the User Tip for the PartnerStatus promotion
                // for Partner of Class FAMILY has been shown  this is done to prevent this
                // User Tip coming up at the same time than the User Tip for the PartnerStatus
                // promotion for Partner of Class FAMILY, which wouldn't look nice.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewDeactivatePartner) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewDeactivatePartner);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.8: Deactivate Partner Dialog",
                        "This new functionality allows the full 'deactivation' of a Partner in one step instead of three steps:" + "\r\n" +
                        "  1) Sets the 'Partner Status' (eg. to 'INVALID')," + "\r\n" + "  2) Cancels all Subscriptions," + "\r\n" +
                        "  3) Expires all Current Addresses." + "\r\n" +
                        "Each of these steps will be done by default, but you can choose to not to perform a certain step." + "\r\n" +
                        "This functionality will help in doing the necessary steps needed to deactivate a Partner more quickly and consistently." +
                        "\r\n" + "It will be very helpful for processing returned mail." + "\r\n" +
                        "Choose 'Deactivate Partner...' from the 'File' menu to use this new functionality.",
                        pnlBalloonTipAnchorHelp);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }
            }
#endif
        }

        #endregion

        #region Custom Events
        private void OnPartnerClassMainDataChanged(TPartnerClassMainDataChangedEventArgs e)
        {
            // MessageBox.Show('OnPartnerClassMainDataChanged. e.PartnerClass: ' + e.PartnerClass.ToString);
            if (PartnerClassMainDataChanged != null)
            {
                PartnerClassMainDataChanged(this, e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TPartnerClassMainDataChangedEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String PartnerClass;
    }

    /// <summary>todoComment</summary>
    public delegate void TDelegateMaintainWorkerField();

    /// <summary>Event handler declaration</summary>
    public delegate void TPartnerClassMainDataChangedHandler(System.Object Sender, TPartnerClassMainDataChangedEventArgs e);
}