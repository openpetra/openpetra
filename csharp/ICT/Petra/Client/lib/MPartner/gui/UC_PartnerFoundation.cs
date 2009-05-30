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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.MPartner;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using SourceGrid;
using System.Globalization;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Shared.RemotedExceptions;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl for editing Partner Foundation Details for a Partner of Partner Class ORGANISATION that is a Foundation.
    public class TUC_PartnerFoundation : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private TbtnCreated btnCreatedFoundation;
        private TTabVersatile tabFoundation;
        private System.Windows.Forms.TabPage tbpInfo;
        private System.Windows.Forms.TabPage tbpProposals;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.GroupBox grpOwners;
        private TCmbAutoPopulated cmbOwner1;
        private TCmbAutoPopulated cmbOwner2;
        private TbtnCreated btnCreatedSubmitDates;
        private TbtnCreated TbtnCreated2;
        private System.Windows.Forms.Panel pnlPartnerFoundationDetails;
        private System.Windows.Forms.GroupBox grpKeyContact;
        private System.Windows.Forms.GroupBox grpProposalDeadlines;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblSubmitMethod;
        private TCmbAutoPopulated cmbSubmitMethod;
        private System.Windows.Forms.Label lblSpecialRequirements;
        private System.Windows.Forms.Label lblFormatting;
        private System.Windows.Forms.Label lblSpecialInstructions;
        private System.Windows.Forms.TextBox txtSpecialRequirements;
        private System.Windows.Forms.TextBox txtFormatting;
        private System.Windows.Forms.TextBox txtSpecialInstructions;
        private System.Windows.Forms.Label lblReview;
        private System.Windows.Forms.Label lblSubmit;
        private System.Windows.Forms.Label lblLastSubmit;
        private System.Windows.Forms.Label lblNextSubmit;
        private System.Windows.Forms.TextBox txtLastSubmit;
        private System.Windows.Forms.TextBox txtNextSubmit;
        private TCmbAutoPopulated cmbReviewFrequency;
        private TCmbAutoPopulated cmbSubmitFrequency;
        private System.Windows.Forms.Button btnEditDeadline;
        private System.Windows.Forms.GroupBox grpPossibleDates;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblNextReminder;
        private System.Windows.Forms.Label lblFDOwner1;
        private System.Windows.Forms.Label lblFDOwner2;
        private TSgrdDataGrid grdSubmitByDates;
        private TSgrdDataGrid grdProposals;
        private System.Windows.Forms.Button btnNewProposal;
        private System.Windows.Forms.Button btnEditProposal;
        private System.Windows.Forms.Button btnDeleteProposal;
        private System.Windows.Forms.GroupBox grpProposalDetails;
        private TSgrdDataGrid grdProposalDetails;
        private System.Windows.Forms.Button btnDeleteDetail;
        private System.Windows.Forms.Button btnEditDetail;
        private System.Windows.Forms.Button btnNewDetail;
        private System.Windows.Forms.Button btnOtherFoundations;
        private System.Windows.Forms.Button btnOtherDonations;
        private System.Windows.Forms.Button btnSubmittedBy;
        private System.Windows.Forms.TextBox txtSubmittedBy;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblSubmittedOn;
        private System.Windows.Forms.Label lblRequestedAmount;
        private System.Windows.Forms.Label lblApprovedAmount;
        private System.Windows.Forms.Label lblReceivedAmount;
        private System.Windows.Forms.Label lblGrantedOn;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtSubmittedOn;
        private System.Windows.Forms.TextBox txtRequestedAmount;
        private System.Windows.Forms.TextBox txtApprovedAmount;
        private System.Windows.Forms.TextBox txtReceivedAmount;
        private System.Windows.Forms.TextBox txtGrantedOn;
        private TCmbAutoPopulated cmbProposalStatus;
        private TexpTextBoxStringLengthCheck expTextBoxStringLengthCheckFoundation;
        protected PartnerEditTDS FMainDS;

        /// <summary>Object that holds the logic for this screen</summary>
        protected TUCPartnerFoundationLogic FLogic;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        protected Boolean FDeadlineEditMode;
        protected Boolean FProposalEditMode;
        protected Boolean FProposalDetailEditMode;
        protected DataView FSubmitByTableDV;
        protected DataView FProposalsDV;
        protected DataView FProposalDetailsDV;
        public PartnerEditTDS MainDS
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

        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        /// <summary>for automatic refreshing of the datagrid after data is changed</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerFoundation));
            this.pnlPartnerFoundationDetails = new System.Windows.Forms.Panel();
            this.grpOwners = new System.Windows.Forms.GroupBox();
            this.lblText = new System.Windows.Forms.Label();
            this.lblFDOwner2 = new System.Windows.Forms.Label();
            this.lblFDOwner1 = new System.Windows.Forms.Label();
            this.lblNextReminder = new System.Windows.Forms.Label();
            this.cmbOwner2 = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbOwner1 = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.btnCreatedFoundation = new Ict.Common.Controls.TbtnCreated();
            this.tabFoundation = new Ict.Common.Controls.TTabVersatile();
            this.tbpInfo = new System.Windows.Forms.TabPage();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.cmbSubmitMethod = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblSubmitMethod = new System.Windows.Forms.Label();
            this.grpProposalDeadlines = new System.Windows.Forms.GroupBox();
            this.grpPossibleDates = new System.Windows.Forms.GroupBox();
            this.btnEditDeadline = new System.Windows.Forms.Button();
            this.grdSubmitByDates = new Ict.Common.Controls.TSgrdDataGrid();
            this.cmbSubmitFrequency = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbReviewFrequency = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.txtNextSubmit = new System.Windows.Forms.TextBox();
            this.txtLastSubmit = new System.Windows.Forms.TextBox();
            this.lblNextSubmit = new System.Windows.Forms.Label();
            this.lblLastSubmit = new System.Windows.Forms.Label();
            this.lblSubmit = new System.Windows.Forms.Label();
            this.lblReview = new System.Windows.Forms.Label();
            this.btnCreatedSubmitDates = new Ict.Common.Controls.TbtnCreated();
            this.grpKeyContact = new System.Windows.Forms.GroupBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtFormatting = new System.Windows.Forms.TextBox();
            this.lblFormatting = new System.Windows.Forms.Label();
            this.lblSpecialInstructions = new System.Windows.Forms.Label();
            this.txtSpecialInstructions = new System.Windows.Forms.TextBox();
            this.lblSpecialRequirements = new System.Windows.Forms.Label();
            this.txtSpecialRequirements = new System.Windows.Forms.TextBox();
            this.tbpProposals = new System.Windows.Forms.TabPage();
            this.grpProposalDetails = new System.Windows.Forms.GroupBox();
            this.cmbProposalStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.txtGrantedOn = new System.Windows.Forms.TextBox();
            this.txtReceivedAmount = new System.Windows.Forms.TextBox();
            this.txtApprovedAmount = new System.Windows.Forms.TextBox();
            this.txtRequestedAmount = new System.Windows.Forms.TextBox();
            this.txtSubmittedOn = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblGrantedOn = new System.Windows.Forms.Label();
            this.lblReceivedAmount = new System.Windows.Forms.Label();
            this.lblApprovedAmount = new System.Windows.Forms.Label();
            this.lblRequestedAmount = new System.Windows.Forms.Label();
            this.lblSubmittedOn = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtSubmittedBy = new System.Windows.Forms.TextBox();
            this.btnSubmittedBy = new System.Windows.Forms.Button();
            this.btnOtherDonations = new System.Windows.Forms.Button();
            this.btnOtherFoundations = new System.Windows.Forms.Button();
            this.btnDeleteDetail = new System.Windows.Forms.Button();
            this.btnEditDetail = new System.Windows.Forms.Button();
            this.btnNewDetail = new System.Windows.Forms.Button();
            this.grdProposalDetails = new Ict.Common.Controls.TSgrdDataGrid();
            this.btnDeleteProposal = new System.Windows.Forms.Button();
            this.btnEditProposal = new System.Windows.Forms.Button();
            this.btnNewProposal = new System.Windows.Forms.Button();
            this.grdProposals = new Ict.Common.Controls.TSgrdDataGrid();
            this.TbtnCreated2 = new Ict.Common.Controls.TbtnCreated();
            this.expTextBoxStringLengthCheckFoundation = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerFoundationDetails.SuspendLayout();
            this.grpOwners.SuspendLayout();
            this.tabFoundation.SuspendLayout();
            this.tbpInfo.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.grpProposalDeadlines.SuspendLayout();
            this.grpPossibleDates.SuspendLayout();
            this.grpKeyContact.SuspendLayout();
            this.tbpProposals.SuspendLayout();
            this.grpProposalDetails.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerFoundationDetails
            //
            this.pnlPartnerFoundationDetails.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerFoundationDetails.AutoScroll = true;
            this.pnlPartnerFoundationDetails.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerFoundationDetails.Controls.Add(this.grpOwners);
            this.pnlPartnerFoundationDetails.Controls.Add(this.btnCreatedFoundation);
            this.pnlPartnerFoundationDetails.Controls.Add(this.tabFoundation);
            this.pnlPartnerFoundationDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerFoundationDetails.Name = "pnlPartnerFoundationDetails";
            this.pnlPartnerFoundationDetails.Size = new System.Drawing.Size(739, 363);
            this.pnlPartnerFoundationDetails.TabIndex = 0;

            //
            // grpOwners
            //
            this.grpOwners.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpOwners.Controls.Add(this.lblText);
            this.grpOwners.Controls.Add(this.lblFDOwner2);
            this.grpOwners.Controls.Add(this.lblFDOwner1);
            this.grpOwners.Controls.Add(this.lblNextReminder);
            this.grpOwners.Controls.Add(this.cmbOwner2);
            this.grpOwners.Controls.Add(this.cmbOwner1);
            this.grpOwners.Location = new System.Drawing.Point(8, 4);
            this.grpOwners.Name = "grpOwners";
            this.grpOwners.Size = new System.Drawing.Size(708, 52);
            this.grpOwners.TabIndex = 0;
            this.grpOwners.TabStop = false;
            this.grpOwners.Text = "Owners";

            //
            // lblText
            //
            this.lblText.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(7, 36);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(454, 14);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "Please get owners\' permission before making any contact with this Foundation";

            //
            // lblFDOwner2
            //
            this.lblFDOwner2.Location = new System.Drawing.Point(250, 14);
            this.lblFDOwner2.Name = "lblFDOwner2";
            this.lblFDOwner2.Size = new System.Drawing.Size(80, 17);
            this.lblFDOwner2.TabIndex = 2;
            this.lblFDOwner2.Text = "FD Owner &2:";
            this.lblFDOwner2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblFDOwner1
            //
            this.lblFDOwner1.Location = new System.Drawing.Point(6, 14);
            this.lblFDOwner1.Name = "lblFDOwner1";
            this.lblFDOwner1.Size = new System.Drawing.Size(80, 20);
            this.lblFDOwner1.TabIndex = 0;
            this.lblFDOwner1.Text = "FD Owner &1:";
            this.lblFDOwner1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblNextReminder
            //
            this.lblNextReminder.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNextReminder.Font =
                new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextReminder.Location = new System.Drawing.Point(523, 32);
            this.lblNextReminder.Name = "lblNextReminder";
            this.lblNextReminder.Size = new System.Drawing.Size(179, 12);
            this.lblNextReminder.TabIndex = 3;
            this.lblNextReminder.Text = "18-MAR-2007";
            this.lblNextReminder.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // cmbOwner2
            //
            this.cmbOwner2.ComboBoxWidth = 120;
            this.cmbOwner2.Filter = null;
            this.cmbOwner2.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.FoundationOwnerList;
            this.cmbOwner2.Location = new System.Drawing.Point(330, 13);
            this.cmbOwner2.Name = "cmbOwner2";
            this.cmbOwner2.SelectedItem = ((object)(resources.GetObject("cmbOwner2.SelectedItem")));
            this.cmbOwner2.SelectedValue = null;
            this.cmbOwner2.Size = new System.Drawing.Size(120, 22);
            this.cmbOwner2.TabIndex = 3;

            //
            // cmbOwner1
            //
            this.cmbOwner1.ComboBoxWidth = 120;
            this.cmbOwner1.Filter = null;
            this.cmbOwner1.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.FoundationOwnerList;
            this.cmbOwner1.Location = new System.Drawing.Point(86, 14);
            this.cmbOwner1.Name = "cmbOwner1";
            this.cmbOwner1.SelectedItem = ((object)(resources.GetObject("cmbOwner1.SelectedItem")));
            this.cmbOwner1.SelectedValue = null;
            this.cmbOwner1.Size = new System.Drawing.Size(120, 22);
            this.cmbOwner1.TabIndex = 1;

            //
            // btnCreatedFoundation
            //
            this.btnCreatedFoundation.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatedFoundation.CreatedBy = null;
            this.btnCreatedFoundation.DateCreated = new System.DateTime(((long)(0)));
            this.btnCreatedFoundation.DateModified = new System.DateTime(((long)(0)));
            this.btnCreatedFoundation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedFoundation.Image = ((System.Drawing.Image)(resources.GetObject("btnCreatedFoundation.Image")));
            this.btnCreatedFoundation.Location = new System.Drawing.Point(718, 6);
            this.btnCreatedFoundation.ModifiedBy = null;
            this.btnCreatedFoundation.Name = "btnCreatedFoundation";
            this.btnCreatedFoundation.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedFoundation.TabIndex = 1;
            this.btnCreatedFoundation.Tag = "dontdisable";

            //
            // tabFoundation
            //
            this.tabFoundation.AllowDrop = true;
            this.tabFoundation.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.tabFoundation.Controls.Add(this.tbpInfo);
            this.tabFoundation.Controls.Add(this.tbpProposals);
            this.tabFoundation.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabFoundation.Location = new System.Drawing.Point(8, 58);
            this.tabFoundation.Name = "tabFoundation";
            this.tabFoundation.SelectedIndex = 0;
            this.tabFoundation.Size = new System.Drawing.Size(704, 296);
            this.tabFoundation.TabIndex = 2;

            //
            // tbpInfo
            //
            this.tbpInfo.Controls.Add(this.pnlInfo);
            this.tbpInfo.Location = new System.Drawing.Point(4, 22);
            this.tbpInfo.Name = "tbpInfo";
            this.tbpInfo.Size = new System.Drawing.Size(696, 270);
            this.tbpInfo.TabIndex = 0;
            this.tbpInfo.Text = "Foundation Info";
            this.tbpInfo.Resize += new System.EventHandler(this.TbpInfo_Resize);

            //
            // pnlInfo
            //
            this.pnlInfo.Controls.Add(this.cmbSubmitMethod);
            this.pnlInfo.Controls.Add(this.lblSubmitMethod);
            this.pnlInfo.Controls.Add(this.grpProposalDeadlines);
            this.pnlInfo.Controls.Add(this.grpKeyContact);
            this.pnlInfo.Controls.Add(this.txtFormatting);
            this.pnlInfo.Controls.Add(this.lblFormatting);
            this.pnlInfo.Controls.Add(this.lblSpecialInstructions);
            this.pnlInfo.Controls.Add(this.txtSpecialInstructions);
            this.pnlInfo.Controls.Add(this.lblSpecialRequirements);
            this.pnlInfo.Controls.Add(this.txtSpecialRequirements);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Padding = new System.Windows.Forms.Padding(2);
            this.pnlInfo.Size = new System.Drawing.Size(696, 270);
            this.pnlInfo.TabIndex = 0;

            //
            // cmbSubmitMethod
            //
            this.cmbSubmitMethod.ComboBoxWidth = 100;
            this.cmbSubmitMethod.Filter = null;
            this.cmbSubmitMethod.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.ProposalSubmissionTypeList;
            this.cmbSubmitMethod.Location = new System.Drawing.Point(112, 92);
            this.cmbSubmitMethod.Name = "cmbSubmitMethod";
            this.cmbSubmitMethod.SelectedItem = ((object)(resources.GetObject("cmbSubmitMethod.SelectedItem")));
            this.cmbSubmitMethod.SelectedValue = null;
            this.cmbSubmitMethod.Size = new System.Drawing.Size(200, 22);
            this.cmbSubmitMethod.TabIndex = 2;

            //
            // lblSubmitMethod
            //
            this.lblSubmitMethod.Location = new System.Drawing.Point(15, 92);
            this.lblSubmitMethod.Name = "lblSubmitMethod";
            this.lblSubmitMethod.Size = new System.Drawing.Size(97, 23);
            this.lblSubmitMethod.TabIndex = 1;
            this.lblSubmitMethod.Text = "S&ubmit Method:";
            this.lblSubmitMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // grpProposalDeadlines
            //
            this.grpProposalDeadlines.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpProposalDeadlines.Controls.Add(this.grpPossibleDates);
            this.grpProposalDeadlines.Controls.Add(this.cmbSubmitFrequency);
            this.grpProposalDeadlines.Controls.Add(this.cmbReviewFrequency);
            this.grpProposalDeadlines.Controls.Add(this.txtNextSubmit);
            this.grpProposalDeadlines.Controls.Add(this.txtLastSubmit);
            this.grpProposalDeadlines.Controls.Add(this.lblNextSubmit);
            this.grpProposalDeadlines.Controls.Add(this.lblLastSubmit);
            this.grpProposalDeadlines.Controls.Add(this.lblSubmit);
            this.grpProposalDeadlines.Controls.Add(this.lblReview);
            this.grpProposalDeadlines.Controls.Add(this.btnCreatedSubmitDates);
            this.grpProposalDeadlines.Location = new System.Drawing.Point(420, 4);
            this.grpProposalDeadlines.Name = "grpProposalDeadlines";
            this.grpProposalDeadlines.Size = new System.Drawing.Size(269, 260);
            this.grpProposalDeadlines.TabIndex = 9;
            this.grpProposalDeadlines.TabStop = false;
            this.grpProposalDeadlines.Text = "Proposal Deadlines";

            //
            // grpPossibleDates
            //
            this.grpPossibleDates.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpPossibleDates.Controls.Add(this.btnEditDeadline);
            this.grpPossibleDates.Controls.Add(this.grdSubmitByDates);
            this.grpPossibleDates.Location = new System.Drawing.Point(8, 74);
            this.grpPossibleDates.Name = "grpPossibleDates";
            this.grpPossibleDates.Size = new System.Drawing.Size(237, 125);
            this.grpPossibleDates.TabIndex = 4;
            this.grpPossibleDates.TabStop = false;
            this.grpPossibleDates.Text = "Possible Submit &Dates";

            //
            // btnEditDeadline
            //
            this.btnEditDeadline.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDeadline.Location = new System.Drawing.Point(158, 16);
            this.btnEditDeadline.Name = "btnEditDeadline";
            this.btnEditDeadline.Size = new System.Drawing.Size(73, 21);
            this.btnEditDeadline.TabIndex = 1;
            this.btnEditDeadline.Text = "Edi&t";
            this.btnEditDeadline.Click += new System.EventHandler(this.BtnEditDeadline_Click);

            //
            // grdSubmitByDates
            //
            this.grdSubmitByDates.AlternatingBackgroundColour = System.Drawing.Color.White;
            this.grdSubmitByDates.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grdSubmitByDates.AutoFindColumn = ((short)(-1));
            this.grdSubmitByDates.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.NoAutoFind;
            this.grdSubmitByDates.AutoStretchColumnsToFitWidth = true;
            this.grdSubmitByDates.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdSubmitByDates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdSubmitByDates.DeleteQuestionMessage = "";
            this.grdSubmitByDates.FixedRows = 1;
            this.grdSubmitByDates.KeepRowSelectedAfterSort = true;
            this.grdSubmitByDates.Location = new System.Drawing.Point(8, 19);
            this.grdSubmitByDates.MinimumHeight = 19;
            this.grdSubmitByDates.Name = "grdSubmitByDates";
            this.grdSubmitByDates.Size = new System.Drawing.Size(150, 97);
            this.grdSubmitByDates.SortableHeaders = false;
            this.grdSubmitByDates.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdSubmitByDates.TabIndex = 0;
            this.grdSubmitByDates.TabStop = true;
            this.grdSubmitByDates.ToolTipTextDelegate = null;

            //
            // cmbSubmitFrequency
            //
            this.cmbSubmitFrequency.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSubmitFrequency.ComboBoxWidth = 100;
            this.cmbSubmitFrequency.Filter = null;
            this.cmbSubmitFrequency.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.ProposalSubmitFrequencyList;
            this.cmbSubmitFrequency.Location = new System.Drawing.Point(64, 44);
            this.cmbSubmitFrequency.Name = "cmbSubmitFrequency";
            this.cmbSubmitFrequency.SelectedItem = ((object)(resources.GetObject("cmbSubmitFrequency.SelectedItem")));
            this.cmbSubmitFrequency.SelectedValue = null;
            this.cmbSubmitFrequency.Size = new System.Drawing.Size(192, 22);
            this.cmbSubmitFrequency.TabIndex = 3;

            //
            // cmbReviewFrequency
            //
            this.cmbReviewFrequency.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbReviewFrequency.ComboBoxWidth = 100;
            this.cmbReviewFrequency.Filter = null;
            this.cmbReviewFrequency.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.ProposalReviewFrequencyList;
            this.cmbReviewFrequency.Location = new System.Drawing.Point(64, 18);
            this.cmbReviewFrequency.Name = "cmbReviewFrequency";
            this.cmbReviewFrequency.SelectedItem = ((object)(resources.GetObject("cmbReviewFrequency.SelectedItem")));
            this.cmbReviewFrequency.SelectedValue = null;
            this.cmbReviewFrequency.Size = new System.Drawing.Size(192, 22);
            this.cmbReviewFrequency.TabIndex = 1;

            //
            // txtNextSubmit
            //
            this.txtNextSubmit.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtNextSubmit.BackColor = System.Drawing.SystemColors.Control;
            this.txtNextSubmit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNextSubmit.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNextSubmit.Location = new System.Drawing.Point(88, 232);
            this.txtNextSubmit.Name = "txtNextSubmit";
            this.txtNextSubmit.ReadOnly = true;
            this.txtNextSubmit.Size = new System.Drawing.Size(177, 14);
            this.txtNextSubmit.TabIndex = 8;
            this.txtNextSubmit.TabStop = false;
            this.txtNextSubmit.Text = "TextBox3";

            //
            // txtLastSubmit
            //
            this.txtLastSubmit.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastSubmit.BackColor = System.Drawing.SystemColors.Control;
            this.txtLastSubmit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastSubmit.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastSubmit.Location = new System.Drawing.Point(88, 208);
            this.txtLastSubmit.Name = "txtLastSubmit";
            this.txtLastSubmit.ReadOnly = true;
            this.txtLastSubmit.Size = new System.Drawing.Size(177, 14);
            this.txtLastSubmit.TabIndex = 6;
            this.txtLastSubmit.TabStop = false;
            this.txtLastSubmit.Text = "TextBox2";

            //
            // lblNextSubmit
            //
            this.lblNextSubmit.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNextSubmit.Location = new System.Drawing.Point(6, 232);
            this.lblNextSubmit.Name = "lblNextSubmit";
            this.lblNextSubmit.Size = new System.Drawing.Size(82, 15);
            this.lblNextSubmit.TabIndex = 7;
            this.lblNextSubmit.Text = "Next Submit:";
            this.lblNextSubmit.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblLastSubmit
            //
            this.lblLastSubmit.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLastSubmit.Location = new System.Drawing.Point(8, 208);
            this.lblLastSubmit.Name = "lblLastSubmit";
            this.lblLastSubmit.Size = new System.Drawing.Size(80, 14);
            this.lblLastSubmit.TabIndex = 5;
            this.lblLastSubmit.Text = "Last Submit:";
            this.lblLastSubmit.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblSubmit
            //
            this.lblSubmit.Location = new System.Drawing.Point(8, 42);
            this.lblSubmit.Name = "lblSubmit";
            this.lblSubmit.Size = new System.Drawing.Size(56, 22);
            this.lblSubmit.TabIndex = 2;
            this.lblSubmit.Text = "Su&bmit:";
            this.lblSubmit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblReview
            //
            this.lblReview.Location = new System.Drawing.Point(8, 16);
            this.lblReview.Name = "lblReview";
            this.lblReview.Size = new System.Drawing.Size(56, 22);
            this.lblReview.TabIndex = 0;
            this.lblReview.Text = "Re&view:";
            this.lblReview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // btnCreatedSubmitDates
            //
            this.btnCreatedSubmitDates.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatedSubmitDates.CreatedBy = null;
            this.btnCreatedSubmitDates.DateCreated = new System.DateTime(((long)(0)));
            this.btnCreatedSubmitDates.DateModified = new System.DateTime(((long)(0)));
            this.btnCreatedSubmitDates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedSubmitDates.Image = ((System.Drawing.Image)(resources.GetObject("btnCreatedSubmitDates.Image")));
            this.btnCreatedSubmitDates.Location = new System.Drawing.Point(247, 75);
            this.btnCreatedSubmitDates.ModifiedBy = null;
            this.btnCreatedSubmitDates.Name = "btnCreatedSubmitDates";
            this.btnCreatedSubmitDates.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedSubmitDates.TabIndex = 10;
            this.btnCreatedSubmitDates.Tag = "dontdisable";

            //
            // grpKeyContact
            //
            this.grpKeyContact.Controls.Add(this.txtPhone);
            this.grpKeyContact.Controls.Add(this.lblPhone);
            this.grpKeyContact.Controls.Add(this.txtEmail);
            this.grpKeyContact.Controls.Add(this.lblEmail);
            this.grpKeyContact.Controls.Add(this.txtTitle);
            this.grpKeyContact.Controls.Add(this.lblTitle);
            this.grpKeyContact.Controls.Add(this.txtName);
            this.grpKeyContact.Controls.Add(this.lblName);
            this.grpKeyContact.Location = new System.Drawing.Point(8, 4);
            this.grpKeyContact.Name = "grpKeyContact";
            this.grpKeyContact.Size = new System.Drawing.Size(408, 68);
            this.grpKeyContact.TabIndex = 0;
            this.grpKeyContact.TabStop = false;
            this.grpKeyContact.Text = "Key Contact";

            //
            // txtPhone
            //
            this.txtPhone.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(256, 40);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(144, 21);
            this.txtPhone.TabIndex = 7;
            this.txtPhone.Text = "TextBox2";

            //
            // lblPhone
            //
            this.lblPhone.Location = new System.Drawing.Point(208, 40);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(48, 21);
            this.lblPhone.TabIndex = 6;
            this.lblPhone.Text = "&Phone:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtEmail
            //
            this.txtEmail.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(56, 40);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(152, 21);
            this.txtEmail.TabIndex = 5;
            this.txtEmail.Text = "TextBox2";

            //
            // lblEmail
            //
            this.lblEmail.Location = new System.Drawing.Point(8, 40);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(48, 21);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "&Email:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtTitle
            //
            this.txtTitle.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTitle.Location = new System.Drawing.Point(256, 16);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(144, 21);
            this.txtTitle.TabIndex = 3;
            this.txtTitle.Text = "TextBox2";

            //
            // lblTitle
            //
            this.lblTitle.Location = new System.Drawing.Point(216, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(40, 21);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "&Title:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtName
            //
            this.txtName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(56, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(152, 21);
            this.FPetraUtilsObject.SetStatusBarText(this.txtName, "Name of the Key Contact");
            this.txtName.TabIndex = 1;
            this.txtName.Text = "TextBox1";

            //
            // lblName
            //
            this.lblName.Location = new System.Drawing.Point(8, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 21);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "&Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtFormatting
            //
            this.txtFormatting.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormatting.Location = new System.Drawing.Point(112, 168);
            this.txtFormatting.Multiline = true;
            this.txtFormatting.Name = "txtFormatting";
            this.txtFormatting.Size = new System.Drawing.Size(296, 47);
            this.txtFormatting.TabIndex = 6;
            this.txtFormatting.Text = "TextBox3";

            //
            // lblFormatting
            //
            this.lblFormatting.Location = new System.Drawing.Point(32, 168);
            this.lblFormatting.Name = "lblFormatting";
            this.lblFormatting.Size = new System.Drawing.Size(80, 24);
            this.lblFormatting.TabIndex = 5;
            this.lblFormatting.Text = "Form&atting:";
            this.lblFormatting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblSpecialInstructions
            //
            this.lblSpecialInstructions.Location = new System.Drawing.Point(24, 216);
            this.lblSpecialInstructions.Name = "lblSpecialInstructions";
            this.lblSpecialInstructions.Size = new System.Drawing.Size(88, 32);
            this.lblSpecialInstructions.TabIndex = 7;
            this.lblSpecialInstructions.Text = "Special &Instructions:";
            this.lblSpecialInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtSpecialInstructions
            //
            this.txtSpecialInstructions.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.txtSpecialInstructions.Location = new System.Drawing.Point(112, 216);
            this.txtSpecialInstructions.Multiline = true;
            this.txtSpecialInstructions.Name = "txtSpecialInstructions";
            this.txtSpecialInstructions.Size = new System.Drawing.Size(296, 48);
            this.txtSpecialInstructions.TabIndex = 8;
            this.txtSpecialInstructions.Text = "TextBox4";

            //
            // lblSpecialRequirements
            //
            this.lblSpecialRequirements.Location = new System.Drawing.Point(21, 120);
            this.lblSpecialRequirements.Name = "lblSpecialRequirements";
            this.lblSpecialRequirements.Size = new System.Drawing.Size(91, 32);
            this.lblSpecialRequirements.TabIndex = 3;
            this.lblSpecialRequirements.Text = "Special &Requirements:";
            this.lblSpecialRequirements.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtSpecialRequirements
            //
            this.txtSpecialRequirements.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.txtSpecialRequirements.Location = new System.Drawing.Point(112, 120);
            this.txtSpecialRequirements.Multiline = true;
            this.txtSpecialRequirements.Name = "txtSpecialRequirements";
            this.txtSpecialRequirements.Size = new System.Drawing.Size(296, 47);
            this.txtSpecialRequirements.TabIndex = 4;
            this.txtSpecialRequirements.Text = "TextBox2";

            //
            // tbpProposals
            //
            this.tbpProposals.Controls.Add(this.grpProposalDetails);
            this.tbpProposals.Controls.Add(this.btnDeleteProposal);
            this.tbpProposals.Controls.Add(this.btnEditProposal);
            this.tbpProposals.Controls.Add(this.btnNewProposal);
            this.tbpProposals.Controls.Add(this.grdProposals);
            this.tbpProposals.Controls.Add(this.TbtnCreated2);
            this.tbpProposals.Location = new System.Drawing.Point(4, 22);
            this.tbpProposals.Name = "tbpProposals";
            this.tbpProposals.Size = new System.Drawing.Size(696, 270);
            this.tbpProposals.TabIndex = 1;
            this.tbpProposals.Text = "Proposals";

            //
            // grpProposalDetails
            //
            this.grpProposalDetails.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grpProposalDetails.Controls.Add(this.cmbProposalStatus);
            this.grpProposalDetails.Controls.Add(this.txtGrantedOn);
            this.grpProposalDetails.Controls.Add(this.txtReceivedAmount);
            this.grpProposalDetails.Controls.Add(this.txtApprovedAmount);
            this.grpProposalDetails.Controls.Add(this.txtRequestedAmount);
            this.grpProposalDetails.Controls.Add(this.txtSubmittedOn);
            this.grpProposalDetails.Controls.Add(this.txtNotes);
            this.grpProposalDetails.Controls.Add(this.lblGrantedOn);
            this.grpProposalDetails.Controls.Add(this.lblReceivedAmount);
            this.grpProposalDetails.Controls.Add(this.lblApprovedAmount);
            this.grpProposalDetails.Controls.Add(this.lblRequestedAmount);
            this.grpProposalDetails.Controls.Add(this.lblSubmittedOn);
            this.grpProposalDetails.Controls.Add(this.lblStatus);
            this.grpProposalDetails.Controls.Add(this.lblNotes);
            this.grpProposalDetails.Controls.Add(this.txtSubmittedBy);
            this.grpProposalDetails.Controls.Add(this.btnSubmittedBy);
            this.grpProposalDetails.Controls.Add(this.btnOtherDonations);
            this.grpProposalDetails.Controls.Add(this.btnOtherFoundations);
            this.grpProposalDetails.Controls.Add(this.btnDeleteDetail);
            this.grpProposalDetails.Controls.Add(this.btnEditDetail);
            this.grpProposalDetails.Controls.Add(this.btnNewDetail);
            this.grpProposalDetails.Controls.Add(this.grdProposalDetails);
            this.grpProposalDetails.Location = new System.Drawing.Point(8, 72);
            this.grpProposalDetails.Name = "grpProposalDetails";
            this.grpProposalDetails.Size = new System.Drawing.Size(697, 188);
            this.grpProposalDetails.TabIndex = 31;
            this.grpProposalDetails.TabStop = false;
            this.grpProposalDetails.Text = "Proposal Details";

            //
            // cmbProposalStatus
            //
            this.cmbProposalStatus.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProposalStatus.ComboBoxWidth = 100;
            this.cmbProposalStatus.Filter = null;
            this.cmbProposalStatus.ListTable = Ict.Petra.Client.CommonControls.TCmbAutoPopulated.TListTableEnum.ProposalStatusList;
            this.cmbProposalStatus.Location = new System.Drawing.Point(417, 116);
            this.cmbProposalStatus.Name = "cmbProposalStatus";
            this.cmbProposalStatus.SelectedItem = ((object)(resources.GetObject("cmbProposalStatus.SelectedItem")));
            this.cmbProposalStatus.SelectedValue = null;
            this.cmbProposalStatus.Size = new System.Drawing.Size(104, 22);
            this.cmbProposalStatus.TabIndex = 51;

            //
            // txtGrantedOn
            //
            this.txtGrantedOn.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGrantedOn.Location = new System.Drawing.Point(593, 164);
            this.txtGrantedOn.Name = "txtGrantedOn";
            this.txtGrantedOn.Size = new System.Drawing.Size(96, 21);
            this.txtGrantedOn.TabIndex = 50;
            this.txtGrantedOn.Text = "TextBox7";

            //
            // txtReceivedAmount
            //
            this.txtReceivedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedAmount.Location = new System.Drawing.Point(593, 140);
            this.txtReceivedAmount.Name = "txtReceivedAmount";
            this.txtReceivedAmount.Size = new System.Drawing.Size(96, 21);
            this.txtReceivedAmount.TabIndex = 49;
            this.txtReceivedAmount.Text = "TextBox6";

            //
            // txtApprovedAmount
            //
            this.txtApprovedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApprovedAmount.Location = new System.Drawing.Point(593, 116);
            this.txtApprovedAmount.Name = "txtApprovedAmount";
            this.txtApprovedAmount.Size = new System.Drawing.Size(96, 21);
            this.txtApprovedAmount.TabIndex = 48;
            this.txtApprovedAmount.Text = "TextBox5";

            //
            // txtRequestedAmount
            //
            this.txtRequestedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestedAmount.Location = new System.Drawing.Point(417, 164);
            this.txtRequestedAmount.Name = "txtRequestedAmount";
            this.txtRequestedAmount.Size = new System.Drawing.Size(96, 21);
            this.txtRequestedAmount.TabIndex = 47;
            this.txtRequestedAmount.Text = "TextBox4";

            //
            // txtSubmittedOn
            //
            this.txtSubmittedOn.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubmittedOn.Location = new System.Drawing.Point(417, 140);
            this.txtSubmittedOn.Name = "txtSubmittedOn";
            this.txtSubmittedOn.Size = new System.Drawing.Size(96, 21);
            this.txtSubmittedOn.TabIndex = 46;
            this.txtSubmittedOn.Text = "TextBox3";

            //
            // txtNotes
            //
            this.txtNotes.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(80, 121);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(249, 58);
            this.txtNotes.TabIndex = 45;
            this.txtNotes.Text = "TextBox2";

            //
            // lblGrantedOn
            //
            this.lblGrantedOn.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGrantedOn.Location = new System.Drawing.Point(521, 164);
            this.lblGrantedOn.Name = "lblGrantedOn";
            this.lblGrantedOn.Size = new System.Drawing.Size(72, 21);
            this.lblGrantedOn.TabIndex = 44;
            this.lblGrantedOn.Text = "Granted:";
            this.lblGrantedOn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblReceivedAmount
            //
            this.lblReceivedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReceivedAmount.Location = new System.Drawing.Point(521, 140);
            this.lblReceivedAmount.Name = "lblReceivedAmount";
            this.lblReceivedAmount.Size = new System.Drawing.Size(72, 21);
            this.lblReceivedAmount.TabIndex = 43;
            this.lblReceivedAmount.Text = "Received:";
            this.lblReceivedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblApprovedAmount
            //
            this.lblApprovedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApprovedAmount.Location = new System.Drawing.Point(521, 116);
            this.lblApprovedAmount.Name = "lblApprovedAmount";
            this.lblApprovedAmount.Size = new System.Drawing.Size(72, 21);
            this.lblApprovedAmount.TabIndex = 42;
            this.lblApprovedAmount.Text = "Approved:";
            this.lblApprovedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblRequestedAmount
            //
            this.lblRequestedAmount.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRequestedAmount.Location = new System.Drawing.Point(337, 164);
            this.lblRequestedAmount.Name = "lblRequestedAmount";
            this.lblRequestedAmount.Size = new System.Drawing.Size(80, 21);
            this.lblRequestedAmount.TabIndex = 41;
            this.lblRequestedAmount.Text = "Requested:";
            this.lblRequestedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblSubmittedOn
            //
            this.lblSubmittedOn.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubmittedOn.Location = new System.Drawing.Point(337, 140);
            this.lblSubmittedOn.Name = "lblSubmittedOn";
            this.lblSubmittedOn.Size = new System.Drawing.Size(80, 21);
            this.lblSubmittedOn.TabIndex = 40;
            this.lblSubmittedOn.Text = "Submitted:";
            this.lblSubmittedOn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblStatus
            //
            this.lblStatus.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(337, 116);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 21);
            this.lblStatus.TabIndex = 39;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblNotes
            //
            this.lblNotes.Location = new System.Drawing.Point(8, 112);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(72, 21);
            this.lblNotes.TabIndex = 38;
            this.lblNotes.Text = "Notes:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtSubmittedBy
            //
            this.txtSubmittedBy.Location = new System.Drawing.Point(80, 88);
            this.txtSubmittedBy.Name = "txtSubmittedBy";
            this.txtSubmittedBy.Size = new System.Drawing.Size(104, 21);
            this.txtSubmittedBy.TabIndex = 37;
            this.txtSubmittedBy.Text = "TextBox1";

            //
            // btnSubmittedBy
            //
            this.btnSubmittedBy.Location = new System.Drawing.Point(8, 88);
            this.btnSubmittedBy.Name = "btnSubmittedBy";
            this.btnSubmittedBy.Size = new System.Drawing.Size(72, 21);
            this.btnSubmittedBy.TabIndex = 36;
            this.btnSubmittedBy.Text = "Submitted By";

            //
            // btnOtherDonations
            //
            this.btnOtherDonations.Location = new System.Drawing.Point(456, 48);
            this.btnOtherDonations.Name = "btnOtherDonations";
            this.btnOtherDonations.Size = new System.Drawing.Size(120, 23);
            this.btnOtherDonations.TabIndex = 35;
            this.btnOtherDonations.Text = "Other Donations";

            //
            // btnOtherFoundations
            //
            this.btnOtherFoundations.Location = new System.Drawing.Point(456, 24);
            this.btnOtherFoundations.Name = "btnOtherFoundations";
            this.btnOtherFoundations.Size = new System.Drawing.Size(120, 23);
            this.btnOtherFoundations.TabIndex = 34;
            this.btnOtherFoundations.Text = "Other Foundations";

            //
            // btnDeleteDetail
            //
            this.btnDeleteDetail.Location = new System.Drawing.Point(360, 56);
            this.btnDeleteDetail.Name = "btnDeleteDetail";
            this.btnDeleteDetail.Size = new System.Drawing.Size(56, 16);
            this.btnDeleteDetail.TabIndex = 33;
            this.btnDeleteDetail.Text = "Delete";
            this.btnDeleteDetail.Click += new System.EventHandler(this.BtnDeleteDetail_Click);

            //
            // btnEditDetail
            //
            this.btnEditDetail.Location = new System.Drawing.Point(360, 40);
            this.btnEditDetail.Name = "btnEditDetail";
            this.btnEditDetail.Size = new System.Drawing.Size(56, 16);
            this.btnEditDetail.TabIndex = 32;
            this.btnEditDetail.Text = "Edit";
            this.btnEditDetail.Click += new System.EventHandler(this.BtnEditDetail_Click);

            //
            // btnNewDetail
            //
            this.btnNewDetail.Location = new System.Drawing.Point(360, 24);
            this.btnNewDetail.Name = "btnNewDetail";
            this.btnNewDetail.Size = new System.Drawing.Size(56, 16);
            this.btnNewDetail.TabIndex = 31;
            this.btnNewDetail.Text = "New";
            this.btnNewDetail.Click += new System.EventHandler(this.BtnNewDetail_Click);

            //
            // grdProposalDetails
            //
            this.grdProposalDetails.AlternatingBackgroundColour = System.Drawing.Color.White;
            this.grdProposalDetails.AutoFindColumn = ((short)(-1));
            this.grdProposalDetails.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.NoAutoFind;
            this.grdProposalDetails.AutoStretchColumnsToFitWidth = true;
            this.grdProposalDetails.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdProposalDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdProposalDetails.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete " +
                                                            "it?";
            this.grdProposalDetails.FixedRows = 1;
            this.grdProposalDetails.KeepRowSelectedAfterSort = true;
            this.grdProposalDetails.Location = new System.Drawing.Point(8, 24);
            this.grdProposalDetails.MinimumHeight = 19;
            this.grdProposalDetails.Name = "grdProposalDetails";
            this.grdProposalDetails.Size = new System.Drawing.Size(344, 64);
            this.grdProposalDetails.SortableHeaders = true;
            this.grdProposalDetails.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdProposalDetails.TabIndex = 0;
            this.grdProposalDetails.TabStop = true;
            this.grdProposalDetails.ToolTipTextDelegate = null;

            //
            // btnDeleteProposal
            //
            this.btnDeleteProposal.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteProposal.Location = new System.Drawing.Point(633, 40);
            this.btnDeleteProposal.Name = "btnDeleteProposal";
            this.btnDeleteProposal.Size = new System.Drawing.Size(56, 16);
            this.btnDeleteProposal.TabIndex = 30;
            this.btnDeleteProposal.Text = "Delete";

            //
            // btnEditProposal
            //
            this.btnEditProposal.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditProposal.Location = new System.Drawing.Point(633, 24);
            this.btnEditProposal.Name = "btnEditProposal";
            this.btnEditProposal.Size = new System.Drawing.Size(56, 16);
            this.btnEditProposal.TabIndex = 29;
            this.btnEditProposal.Text = "Edit";

            //
            // btnNewProposal
            //
            this.btnNewProposal.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewProposal.Location = new System.Drawing.Point(633, 8);
            this.btnNewProposal.Name = "btnNewProposal";
            this.btnNewProposal.Size = new System.Drawing.Size(56, 16);
            this.btnNewProposal.TabIndex = 28;
            this.btnNewProposal.Text = "New";

            //
            // grdProposals
            //
            this.grdProposals.AlternatingBackgroundColour =
                System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.grdProposals.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grdProposals.AutoFindColumn = ((short)(-1));
            this.grdProposals.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.NoAutoFind;
            this.grdProposals.AutoStretchColumnsToFitWidth = true;
            this.grdProposals.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdProposals.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdProposals.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete " +
                                                      "it?";
            this.grdProposals.FixedRows = 1;
            this.grdProposals.KeepRowSelectedAfterSort = true;
            this.grdProposals.Location = new System.Drawing.Point(8, 8);
            this.grdProposals.MinimumHeight = 19;
            this.grdProposals.Name = "grdProposals";
            this.grdProposals.Size = new System.Drawing.Size(617, 64);
            this.grdProposals.SortableHeaders = true;
            this.grdProposals.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdProposals.TabIndex = 27;
            this.grdProposals.TabStop = true;
            this.grdProposals.ToolTipTextDelegate = null;

            //
            // TbtnCreated2
            //
            this.TbtnCreated2.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TbtnCreated2.CreatedBy = null;
            this.TbtnCreated2.DateCreated = new System.DateTime(((long)(0)));
            this.TbtnCreated2.DateModified = new System.DateTime(((long)(0)));
            this.TbtnCreated2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TbtnCreated2.Image = ((System.Drawing.Image)(resources.GetObject("TbtnCreated2.Image")));
            this.TbtnCreated2.Location = new System.Drawing.Point(697, 4);
            this.TbtnCreated2.ModifiedBy = null;
            this.TbtnCreated2.Name = "TbtnCreated2";
            this.TbtnCreated2.Size = new System.Drawing.Size(14, 16);
            this.TbtnCreated2.TabIndex = 26;
            this.TbtnCreated2.Tag = "dontdisable";

            //
            // TUC_PartnerFoundation
            //
            this.Controls.Add(this.pnlPartnerFoundationDetails);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUC_PartnerFoundation";
            this.Size = new System.Drawing.Size(739, 360);
            this.pnlPartnerFoundationDetails.ResumeLayout(false);
            this.grpOwners.ResumeLayout(false);
            this.tabFoundation.ResumeLayout(false);
            this.tbpInfo.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.grpProposalDeadlines.ResumeLayout(false);
            this.grpProposalDeadlines.PerformLayout();
            this.grpPossibleDates.ResumeLayout(false);
            this.grpKeyContact.ResumeLayout(false);
            this.grpKeyContact.PerformLayout();
            this.tbpProposals.ResumeLayout(false);
            this.grpProposalDetails.ResumeLayout(false);
            this.grpProposalDetails.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFoundation() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // TODO : Make Proposals tab work.
            tbpProposals.Enabled = false;

            // define the screen's logic
            FLogic = new TUCPartnerFoundationLogic();
            FDeadlineEditMode = false;
            FProposalEditMode = false;
            FProposalDetailEditMode = false;
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

        protected void PerformDataBinding()
        {
            txtName.DataBindings.Add("Text", FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetKeyContactNameDBName());
            txtTitle.DataBindings.Add("Text", FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetKeyContactTitleDBName());
            txtEmail.DataBindings.Add("Text", FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetKeyContactEmailDBName());
            txtPhone.DataBindings.Add("Text", FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetKeyContactPhoneDBName());
            txtSpecialRequirements.DataBindings.Add("Text", FMainDS,
                PFoundationTable.GetTableName() + '.' + PFoundationTable.GetSpecialRequirementsDBName());
            txtFormatting.DataBindings.Add("Text", FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetProposalFormattingDBName());
            txtSpecialInstructions.DataBindings.Add("Text", FMainDS,
                PFoundationTable.GetTableName() + '.' + PFoundationTable.GetSpecialInstructionsDBName());
            cmbOwner1.PerformDataBinding(FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetOwner1KeyDBName());
            cmbOwner2.PerformDataBinding(FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetOwner2KeyDBName());
            cmbSubmitMethod.PerformDataBinding(FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetProposalSubmissionTypeDBName());
            cmbReviewFrequency.PerformDataBinding(FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetReviewFrequencyDBName());
            cmbSubmitFrequency.PerformDataBinding(FMainDS, PFoundationTable.GetTableName() + '.' + PFoundationTable.GetSubmitFrequencyDBName());
            cmbSubmitFrequency.ComboBoxWidth = 128;
            cmbReviewFrequency.ComboBoxWidth = 128;
            cmbProposalStatus.PerformDataBinding(FMainDS,
                PFoundationProposalTable.GetTableName() + '.' + PFoundationProposalTable.GetProposalStatusDBName());
            this.stbUCPartnerFoundation.SetStatusBarText(this.txtTitle, PFoundationTable.GetKeyContactTitleHelp());
        }

        protected void CmbReviewFrequency_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            if ((cmbReviewFrequency.SelectedValue != null) && (cmbReviewFrequency.SelectedValue.GetType() == typeof(string)))
            {
                FLogic.PopulateDeadlines(cmbReviewFrequency.SelectedValue.ToString());
                btnEditDeadline.Enabled = (FMainDS.PFoundationDeadline.Rows.Count > 0);
                ShowNextSubmitDate();
            }
        }

        protected void CmbSubmitFrequency_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            if ((cmbSubmitFrequency.SelectedValue != null) && (cmbSubmitFrequency.SelectedValue.GetType() == typeof(string)))
            {
                ShowNextSubmitDate();
            }
        }

        private void BtnDeleteDetail_Click(System.Object sender, System.EventArgs e)
        {
            if (btnDeleteDetail.Text == "Delete")
            {
                if (grdProposalDetails.Selection.IsEmpty())
                {
                    MessageBox.Show("Please select a Row to delete.", "Proposal Details Delete");
                }
                else
                {
                    FLogic.DeleteProposalDetail((PFoundationProposalDetailRow)((DataRowView)grdProposalDetails.SelectedDataRows[0]).Row);
                }
            }
            else if (btnDeleteDetail.Text == "Cancel")
            {
            }

            // TODO : Cancel button
        }

        private void BtnEditDetail_Click(System.Object sender, System.EventArgs e)
        {
            OnEnableDisableOtherScreenParts(new TEnableDisableEventArgs(FProposalDetailEditMode));
            EnableDisableProposalControls(FProposalDetailEditMode);
            FProposalDetailEditMode = (!FProposalDetailEditMode);
            EnableDisableDataGrid(grdProposalDetails, FProposalDetailEditMode);

            if (FProposalDetailEditMode)
            {
                btnEditDetail.Text = "Done";
                btnDeleteDetail.Text = "Cancel";
            }
            else
            {
                btnEditDetail.Text = "Edit";
                btnDeleteDetail.Text = "Delete";
            }
        }

        private void BtnNewDetail_Click(System.Object sender, System.EventArgs e)
        {
            if (grdProposals.Selection.IsEmpty())
            {
                MessageBox.Show("Please select a proposal.");
                return;
            }

            if (!FProposalDetailEditMode)
            {
                BtnEditDetail_Click(sender, e);
            }

            FLogic.NewProposalDetail((PFoundationProposalRow)((DataRowView)(grdProposals.SelectedDataRows[0])).Row);
        }

        private void BtnEditDeadline_Click(System.Object sender, System.EventArgs e)
        {
            OnEnableDisableOtherScreenParts(new TEnableDisableEventArgs(FDeadlineEditMode));
            EnableDisableInfoControls(FDeadlineEditMode);
            FDeadlineEditMode = (!FDeadlineEditMode);
            EnableDisableDataGrid(grdSubmitByDates, FDeadlineEditMode);

            if (FDeadlineEditMode)
            {
                btnEditDeadline.Text = CommonResourcestrings.StrBtnTextDone;
                grdSubmitByDates.Focus();
            }
            else
            {
                btnEditDeadline.Text = CommonResourcestrings.StrBtnTextEdit;
                btnEditDeadline.Focus();
            }
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

        protected void EnableDisableDataGrid(TSgrdDataGrid Grid, Boolean AFlag)
        {
            foreach (DataGridColumn GridColumn in Grid.Columns)
            {
                if (GridColumn.DataCell.Editor != null)
                {
                    GridColumn.DataCell.Editor.EnableEdit = AFlag;
                }
            }
        }

        /// <summary>
        /// Enable or disable fields on the Info tab.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void EnableDisableInfoControls(Boolean AFlag)
        {
            grpOwners.Enabled = AFlag;
            tbpProposals.Enabled = false;
            grpKeyContact.Enabled = AFlag;
            cmbSubmitMethod.Enabled = AFlag;
            txtSpecialRequirements.Enabled = AFlag;
            txtFormatting.Enabled = AFlag;
            txtSpecialInstructions.Enabled = AFlag;
            cmbReviewFrequency.Enabled = AFlag;
            cmbSubmitFrequency.Enabled = AFlag;
        }

        /// <summary>
        /// Enable or disable fields on the Proposals tab.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void EnableDisableProposalControls(Boolean AFlag)
        {
            grpOwners.Enabled = AFlag;
            tbpInfo.Enabled = AFlag;
            btnNewProposal.Enabled = AFlag;
            btnEditProposal.Enabled = AFlag;
            btnDeleteProposal.Enabled = AFlag;
            btnNewDetail.Enabled = AFlag;
            btnOtherFoundations.Enabled = AFlag;
            btnOtherDonations.Enabled = AFlag;
            btnSubmittedBy.Enabled = AFlag;
            txtSubmittedBy.Enabled = AFlag;
            txtNotes.Enabled = AFlag;
            cmbProposalStatus.Enabled = AFlag;
            txtSubmittedOn.Enabled = AFlag;
            txtRequestedAmount.Enabled = AFlag;
            txtApprovedAmount.Enabled = AFlag;
            txtReceivedAmount.Enabled = AFlag;
            txtGrantedOn.Enabled = AFlag;
        }

        public void InitialiseUserControl()
        {
            DateTime d;

            // Set up screen logic
            FLogic.MainDS = FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;

            if (!FLogic.LoadDataOnDemand())
            {
            }

            // Don't need to do anything here since a p_foundation record has got
            // created in UC_PartnerDetailsOrganisation
            PerformDataBinding();
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFoundationDetails));

            // Specify the DataView that the SubmitBy datagrid will be bound to
            FSubmitByTableDV = FMainDS.PFoundationDeadline.DefaultView;
            FSubmitByTableDV.Sort = FMainDS.PFoundationDeadline.ColumnDeadlineMonth.ColumnName;
            FSubmitByTableDV.AllowDelete = false;
            FSubmitByTableDV.AllowNew = false;

            // Specify the DataView that the Proposals datagrid will be bound to
            FProposalsDV = FMainDS.PFoundationProposal.DefaultView;
            FProposalsDV.AllowDelete = false;
            FProposalsDV.AllowNew = false;

            // Specify the DataView that the ProposalDetails datagrid will be bound to
            FProposalDetailsDV = FMainDS.PFoundationProposalDetail.DefaultView;
            FProposalDetailsDV.AllowDelete = false;
            FProposalDetailsDV.AllowNew = false;

            // Set up screen logic (cont.)
            FLogic.SubmitByDataView = FSubmitByTableDV;
            FLogic.ProposalsDataView = FProposalsDV;
            FLogic.ProposalDetailsDataView = FProposalDetailsDV;

            // Create SourceDataGrid columns
            FLogic.CreateColumns(grdSubmitByDates, (PFoundationDeadlineTable)FSubmitByTableDV.Table);
            FLogic.CreateColumns(grdProposals, (PFoundationProposalTable)FProposalsDV.Table);
            FLogic.CreateColumns(grdProposalDetails, (PFoundationProposalDetailTable)FProposalDetailsDV.Table);


            EnableDisableDataGrid(grdSubmitByDates, false);
            EnableDisableDataGrid(grdProposals, false);
            EnableDisableDataGrid(grdProposalDetails, false);

            // DataBinding
            SetupDataGridDataBinding();

            // Hook up data change events
            this.cmbReviewFrequency.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbReviewFrequency_SelectedValueChanged);
            this.cmbSubmitFrequency.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbSubmitFrequency_SelectedValueChanged);

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();
            d = FLogic.Get_LastSubmittedDate();

            if (d == DateTime.MinValue)
            {
                txtLastSubmit.Text = "";
            }
            else
            {
                txtLastSubmit.Text = StringHelper.DateToLocalizedString(d);
            }

            ShowNextSubmitDate();
            d = FLogic.Get_NextReminderDate();

            if (d == DateTime.MinValue)
            {
                lblNextReminder.Text = "No reminders";
            }
            else
            {
                lblNextReminder.Text = "Reminder: " + StringHelper.DateToLocalizedString(d);
            }

            btnCreatedFoundation.UpdateFields(FMainDS.PFoundation);
            SetupBtnCreatedSubmitDates();
            btnEditDeadline.Enabled = (FMainDS.PFoundationDeadline.Rows.Count > 0);

            // Extender Provider
            this.expTextBoxStringLengthCheckFoundation.RetrieveTextboxes(this);
        }

        /// <summary>
        /// Calculates Created info from the oldest PFoundationDeadline record and
        /// Modified info from the most recently modified PFoundationDeadline record
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupBtnCreatedSubmitDates()
        {
            int Counter;
            DateTime DateCreated = DateTime.Now;
            DateTime DateModified = DateTime.Now;
            DateTime OldestDateCreated = DateTime.Now;
            DateTime NewestDateModified = DateTime.Now;
            String CreatedBy = "";
            String ModifiedBy = "";
            DataView CurrentFoundationDeadlinesDV;

            OldestDateCreated = DateTime.MinValue;
            NewestDateModified = DateTime.MinValue;

            if (FMainDS.PFoundationDeadline != null)
            {
                CurrentFoundationDeadlinesDV = new DataView(FMainDS.PFoundationDeadline, "", "", DataViewRowState.CurrentRows);

                if (CurrentFoundationDeadlinesDV.Count != 0)
                {
                    for (Counter = 0; Counter <= CurrentFoundationDeadlinesDV.Count - 1; Counter += 1)
                    {
                        DateCreated =
                            TSaveConvert.DateColumnToDate(FMainDS.PFoundationDeadline.ColumnDateCreated, CurrentFoundationDeadlinesDV[Counter].Row);
                        DateModified =
                            TSaveConvert.DateColumnToDate(FMainDS.PFoundationDeadline.ColumnDateModified, CurrentFoundationDeadlinesDV[Counter].Row);

                        if ((OldestDateCreated == DateTime.MinValue) || (OldestDateCreated > DateCreated))
                        {
                            OldestDateCreated = DateCreated;
                            CreatedBy =
                                TSaveConvert.StringColumnToString(FMainDS.PFoundationDeadline.ColumnCreatedBy,
                                    CurrentFoundationDeadlinesDV[Counter].Row);
                        }

                        if ((NewestDateModified == DateTime.MinValue) || (NewestDateModified < DateModified))
                        {
                            NewestDateModified = DateModified;
                            ModifiedBy = TSaveConvert.StringColumnToString(FMainDS.PFoundationDeadline.ColumnModifiedBy,
                                CurrentFoundationDeadlinesDV[Counter].Row);
                        }
                    }
                }
            }

            if (OldestDateCreated != DateTime.MinValue)
            {
                btnCreatedSubmitDates.DateCreated = OldestDateCreated;
                btnCreatedSubmitDates.CreatedBy = CreatedBy;
            }

            if (NewestDateModified != DateTime.MinValue)
            {
                btnCreatedSubmitDates.DateModified = NewestDateModified;
                btnCreatedSubmitDates.ModifiedBy = ModifiedBy;
            }
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridDataBinding()
        {
            grdSubmitByDates.DataSource = new DevAge.ComponentModel.BoundDataView(FSubmitByTableDV);
            grdProposals.DataSource = new DevAge.ComponentModel.BoundDataView(FProposalsDV);
            grdProposalDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FProposalDetailsDV);
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridVisualAppearance()
        {
            grdSubmitByDates.AutoSizeCells();
            grdProposals.AutoSizeCells();
            grdProposalDetails.AutoSizeCells();
        }

        protected void ShowNextSubmitDate()
        {
            DateTime d;

            d = FLogic.Get_NextSubmitDate();

            if (d == DateTime.MinValue)
            {
                txtNextSubmit.Text = "";
            }
            else
            {
                txtNextSubmit.Text = StringHelper.DateToLocalizedString(d);
            }
        }

        private void TbpInfo_Resize(System.Object sender, System.EventArgs e)
        {
            txtSpecialRequirements.Height = Convert.ToInt32((tbpInfo.Height - txtSpecialRequirements.Top - 8) / 3.0);

            txtFormatting.Height = txtSpecialRequirements.Height;
            txtFormatting.Top = txtSpecialRequirements.Bottom + 1;
            lblFormatting.Top = txtFormatting.Top;

            txtSpecialInstructions.Height = txtFormatting.Height;
            txtSpecialInstructions.Top = txtFormatting.Bottom + 1;
            lblSpecialInstructions.Top = txtSpecialInstructions.Top;
        }

        protected void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        protected void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }
    }
}