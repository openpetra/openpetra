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
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using SourceGrid;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl for editing Family Members for a Partner of Partner Class FAMILY.
    public class TUC_FamilyMembers : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        public const String StrBtnTextManual = "Manual ";
        public const String StrAlreadyOpen = "Partner is already open in this Partner Edit screen.";
        public const String StrAlreadyOpenTitle = "Partner is already open";
        public const String StrFamilyIDExplained = "Family Identification Number (Family ID) " + "\r\n" +
                                                   "------------------------------------------------ " + "\r\n" +
                                                   " This number is used to identify the family members within a Family. " +
                                                   "\r\n" + " * Family ID's 0 and 1 are used for parents; " + "\r\n" +
                                                   "    FamilyID's 2, 3, 4 ... 9 are used for children. " + "\r\n" +
                                                   " * All gifts to this Family will be assigned to the to the Field in the Commitment Record of the family member "
                                                   +
                                                   "\r\n" +
                                                   "    with the the lowest FamilyID of those who have a current Commitment Record." + "\r\n" +
                                                   "\r\n" +
                                                   " This system needs to be consistently applied to all Families. This ensures that gifts go to the correct Field,"
                                                   +
                                                   "\r\n" +
                                                   " and that family members are always listed in the same order on screen as well as on reports! ";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerFoundationDetails;
        private TSgrdDataGrid grdFamilyMembers;
        private System.Windows.Forms.Button btnFamilyMemberDemote;
        private System.Windows.Forms.Button btnFamilyMemberPromote;
        private System.Windows.Forms.Button btnEditPerson;
        private System.Windows.Forms.Button btnMovePersonToOtherFamily;
        private System.Windows.Forms.Button btnAddExistingPersonToThisFamily;
        private System.Windows.Forms.GroupBox grpFamilyMembersModify;
        private System.Windows.Forms.Button btnAddNewPersonThisFamily;
        private System.Windows.Forms.GroupBox grpChangeFamilyID;
        private System.Windows.Forms.ToolTip TipPromote;
        private System.Windows.Forms.ToolTip TipDemote;
        private System.Windows.Forms.ToolTip TipRefresh;
        private System.Windows.Forms.Button btnEditFamilyID;
        private System.Windows.Forms.Panel pnlFamilyInformation;
        private System.Windows.Forms.Panel pnlFamilyMembers;
        private System.Windows.Forms.GroupBox grpFamily;
        private TTxtPartnerKeyTextBox txtFamilyPartnerKeyTextBox;
        private System.Windows.Forms.GroupBox grpFamilyMembersFake;
        private System.Windows.Forms.Button btnEditFamily;
        private System.Windows.Forms.Panel pnlFamilyArranger;
        private System.Windows.Forms.Button btnChangeFamily;
        private System.Windows.Forms.Button btnRefreshDataGrid;
        private System.Windows.Forms.ToolTip ToolTip1;
        private System.Windows.Forms.Button btnFamilyIDHelp;

        /// <summary>Object that holds the logic for this screen</summary>
        protected Boolean FDeadlineEditMode;
        protected Boolean FDeadLineEditMode2;
        protected TUCFamilyMembersLogic FLogic;
        protected PartnerEditTDS FMainDS;
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        protected TEnableDisableScreenPartsEventHandler FEnableDisableOtherScreenParts;
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        private TDelegateIsNewPartner FDelegateIsNewPartner;
        private Boolean FFamilyMembersExist;
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

        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;
        public event TDelegateGetLocationRowOfCurrentlySelectedAddress GetLocationRowOfCurrentlySelectedAddress;

        /// <summary>for automatic refreshing of the datagrid after data is changed (new person added to family etc.)</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_FamilyMembers));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerFoundationDetails = new System.Windows.Forms.Panel();
            this.pnlFamilyMembers = new System.Windows.Forms.Panel();
            this.pnlFamilyArranger = new System.Windows.Forms.Panel();
            this.btnRefreshDataGrid = new System.Windows.Forms.Button();
            this.grdFamilyMembers = new Ict.Common.Controls.TSgrdDataGrid();
            this.btnEditPerson = new System.Windows.Forms.Button();
            this.grpChangeFamilyID = new System.Windows.Forms.GroupBox();
            this.btnFamilyMemberDemote = new System.Windows.Forms.Button();
            this.btnFamilyMemberPromote = new System.Windows.Forms.Button();
            this.btnEditFamilyID = new System.Windows.Forms.Button();
            this.grpFamilyMembersModify = new System.Windows.Forms.GroupBox();
            this.btnAddNewPersonThisFamily = new System.Windows.Forms.Button();
            this.btnMovePersonToOtherFamily = new System.Windows.Forms.Button();
            this.btnAddExistingPersonToThisFamily = new System.Windows.Forms.Button();
            this.btnFamilyIDHelp = new System.Windows.Forms.Button();
            this.grpFamilyMembersFake = new System.Windows.Forms.GroupBox();
            this.pnlFamilyInformation = new System.Windows.Forms.Panel();
            this.grpFamily = new System.Windows.Forms.GroupBox();
            this.btnChangeFamily = new System.Windows.Forms.Button();
            this.btnEditFamily = new System.Windows.Forms.Button();
            this.txtFamilyPartnerKeyTextBox = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.TipPromote = new System.Windows.Forms.ToolTip(this.components);
            this.TipDemote = new System.Windows.Forms.ToolTip(this.components);
            this.TipRefresh = new System.Windows.Forms.ToolTip(this.components);
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlPartnerFoundationDetails.SuspendLayout();
            this.pnlFamilyMembers.SuspendLayout();
            this.pnlFamilyArranger.SuspendLayout();
            this.grpChangeFamilyID.SuspendLayout();
            this.grpFamilyMembersModify.SuspendLayout();
            this.pnlFamilyInformation.SuspendLayout();
            this.grpFamily.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerFoundationDetails
            //
            this.pnlPartnerFoundationDetails.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerFoundationDetails.AutoScroll = true;
            this.pnlPartnerFoundationDetails.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerFoundationDetails.Controls.Add(this.pnlFamilyMembers);
            this.pnlPartnerFoundationDetails.Controls.Add(this.pnlFamilyInformation);
            this.pnlPartnerFoundationDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerFoundationDetails.Name = "pnlPartnerFoundationDetails";
            this.pnlPartnerFoundationDetails.Size = new System.Drawing.Size(739, 363);
            this.pnlPartnerFoundationDetails.TabIndex = 0;

            //
            // pnlFamilyMembers
            //
            this.pnlFamilyMembers.Controls.Add(this.pnlFamilyArranger);
            this.pnlFamilyMembers.Controls.Add(this.grpFamilyMembersFake);
            this.pnlFamilyMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFamilyMembers.DockPadding.Left = 5;
            this.pnlFamilyMembers.Location = new System.Drawing.Point(0, 44);
            this.pnlFamilyMembers.Name = "pnlFamilyMembers";
            this.pnlFamilyMembers.Size = new System.Drawing.Size(739, 319);
            this.pnlFamilyMembers.TabIndex = 12;

            //
            // pnlFamilyArranger
            //
            this.pnlFamilyArranger.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFamilyArranger.Controls.Add(this.btnRefreshDataGrid);
            this.pnlFamilyArranger.Controls.Add(this.grdFamilyMembers);
            this.pnlFamilyArranger.Controls.Add(this.btnEditPerson);
            this.pnlFamilyArranger.Controls.Add(this.grpChangeFamilyID);
            this.pnlFamilyArranger.Controls.Add(this.grpFamilyMembersModify);
            this.pnlFamilyArranger.Controls.Add(this.btnFamilyIDHelp);
            this.pnlFamilyArranger.Location = new System.Drawing.Point(12, 18);
            this.pnlFamilyArranger.Name = "pnlFamilyArranger";
            this.pnlFamilyArranger.Size = new System.Drawing.Size(662, 290);
            this.pnlFamilyArranger.TabIndex = 0;

            //
            // btnRefreshDataGrid
            //
            this.btnRefreshDataGrid.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnRefreshDataGrid.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshDataGrid.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnRefreshDataGrid.Image")));
            this.btnRefreshDataGrid.Location = new System.Drawing.Point(508, 156);
            this.btnRefreshDataGrid.Name = "btnRefreshDataGrid";
            this.btnRefreshDataGrid.Size = new System.Drawing.Size(20, 18);
            this.btnRefreshDataGrid.TabIndex = 1;
            this.TipRefresh.SetToolTip(this.btnRefreshDataGrid, "Refresh Family Member" + "s List (eg. after moving Persons from/to Family)");
            this.btnRefreshDataGrid.KeyPress += new KeyPressEventHandler(this.BtnRefreshDataGrid_KeyPress);
            this.btnRefreshDataGrid.Click += new System.EventHandler(this.BtnRefreshDataGrid_Click);
            this.btnRefreshDataGrid.MouseEnter += new System.EventHandler(this.BtnRefreshDataGrid_MouseEnter);

            //
            // grdFamilyMembers
            //
            this.grdFamilyMembers.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(255, 255, 255);
            this.grdFamilyMembers.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFamilyMembers.AutoFindColumn = ((Int16)(1));
            this.grdFamilyMembers.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.grdFamilyMembers.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdFamilyMembers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdFamilyMembers.DeleteQuestionMessage = "You have chosen to delete " + "this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdFamilyMembers.FixedRows = 1;
            this.grdFamilyMembers.Location = new System.Drawing.Point(4, 4);
            this.grdFamilyMembers.MinimumHeight = 19;
            this.grdFamilyMembers.Name = "grdFamilyMembers";
            this.grdFamilyMembers.Size = new System.Drawing.Size(502, 170);
            this.grdFamilyMembers.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.FPetraUtilsObject.SetStatusBarText(this.grdFamilyMembers, "Family Me" + "mbers List");
            this.grdFamilyMembers.TabIndex = 0;
            this.grdFamilyMembers.TabStop = true;
            this.grdFamilyMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GrdFamilyMembers_KeyDown);
            this.grdFamilyMembers.DoubleClick += new System.EventHandler(this.GrdFamilyMembers_DoubleClick);

            //
            // btnEditPerson
            //
            this.btnEditPerson.Location = new System.Drawing.Point(10, 178);
            this.btnEditPerson.Name = "btnEditPerson";
            this.btnEditPerson.Size = new System.Drawing.Size(154, 23);
            this.btnEditPerson.TabIndex = 4;
            this.btnEditPerson.Text = "Edit Selected &Person";
            this.btnEditPerson.Click += new System.EventHandler(this.BtnEditPerson_Click);

            //
            // grpChangeFamilyID
            //
            this.grpChangeFamilyID.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.grpChangeFamilyID.Controls.Add(this.btnFamilyMemberDemote);
            this.grpChangeFamilyID.Controls.Add(this.btnFamilyMemberPromote);
            this.grpChangeFamilyID.Controls.Add(this.btnEditFamilyID);
            this.grpChangeFamilyID.Location = new System.Drawing.Point(512, 28);
            this.grpChangeFamilyID.Name = "grpChangeFamilyID";
            this.grpChangeFamilyID.Size = new System.Drawing.Size(122, 114);
            this.grpChangeFamilyID.TabIndex = 2;
            this.grpChangeFamilyID.TabStop = false;
            this.grpChangeFamilyID.Text = "Change Family ID";

            //
            // btnFamilyMemberDemote
            //
            this.btnFamilyMemberDemote.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnFamilyMemberDemote.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnFamilyMemberDemote.Image")));
            this.btnFamilyMemberDemote.Location = new System.Drawing.Point(48, 30);
            this.btnFamilyMemberDemote.Name = "btnFamilyMemberDemote";
            this.btnFamilyMemberDemote.Size = new System.Drawing.Size(32, 23);
            this.btnFamilyMemberDemote.TabIndex = 0;
            this.TipPromote.SetToolTip(this.btnFamilyMemberDemote, "Demote Family ID");
            this.btnFamilyMemberDemote.Click += new System.EventHandler(this.BtnFamilyMemberDemote_Click);

            //
            // btnFamilyMemberPromote
            //
            this.btnFamilyMemberPromote.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnFamilyMemberPromote.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnFamilyMemberPromote.Image")));
            this.btnFamilyMemberPromote.Location = new System.Drawing.Point(48, 57);
            this.btnFamilyMemberPromote.Name = "btnFamilyMemberPromote";
            this.btnFamilyMemberPromote.Size = new System.Drawing.Size(32, 23);
            this.btnFamilyMemberPromote.TabIndex = 1;
            this.TipDemote.SetToolTip(this.btnFamilyMemberPromote, "Promote Family ID");
            this.btnFamilyMemberPromote.Click += new System.EventHandler(this.BtnFamilyMemberPromote_Click);

            //
            // btnEditFamilyID
            //
            this.btnEditFamilyID.Location = new System.Drawing.Point(12, 82);
            this.btnEditFamilyID.Name = "btnEditFamilyID";
            this.btnEditFamilyID.Size = new System.Drawing.Size(100, 24);
            this.btnEditFamilyID.TabIndex = 2;
            this.btnEditFamilyID.Text = "Manual Edi&t";
            this.btnEditFamilyID.Click += new System.EventHandler(this.BtnEditFamilyID_Click2);

            //
            // grpFamilyMembersModify
            //
            this.grpFamilyMembersModify.Controls.Add(this.btnAddNewPersonThisFamily);
            this.grpFamilyMembersModify.Controls.Add(this.btnMovePersonToOtherFamily);
            this.grpFamilyMembersModify.Controls.Add(this.btnAddExistingPersonToThisFamily);
            this.grpFamilyMembersModify.Location = new System.Drawing.Point(0, 210);
            this.grpFamilyMembersModify.Name = "grpFamilyMembersModify";
            this.grpFamilyMembersModify.Size = new System.Drawing.Size(506, 76);
            this.grpFamilyMembersModify.TabIndex = 5;
            this.grpFamilyMembersModify.TabStop = false;
            this.grpFamilyMembersModify.Text = "Move or Create Family Members";

            //
            // btnAddNewPersonThisFamily
            //
            this.btnAddNewPersonThisFamily.Location = new System.Drawing.Point(256, 20);
            this.btnAddNewPersonThisFamily.Name = "btnAddNewPersonThisFamily";
            this.btnAddNewPersonThisFamily.Size = new System.Drawing.Size(240, 23);
            this.btnAddNewPersonThisFamily.TabIndex = 2;
            this.btnAddNewPersonThisFamily.Text = "Create &New Person for This Family";
            this.btnAddNewPersonThisFamily.Click += new System.EventHandler(this.BtnAddNewPersonThisFamily_Click);

            //
            // btnMovePersonToOtherFamily
            //
            this.btnMovePersonToOtherFamily.Location = new System.Drawing.Point(10, 20);
            this.btnMovePersonToOtherFamily.Name = "btnMovePersonToOtherFamily";
            this.btnMovePersonToOtherFamily.Size = new System.Drawing.Size(240, 23);
            this.btnMovePersonToOtherFamily.TabIndex = 0;
            this.btnMovePersonToOtherFamily.Text = "Move Selected Person to Other Fa" + "mil&y";
            this.btnMovePersonToOtherFamily.Click += new System.EventHandler(this.BtnMovePersonToOtherFamily_Click);

            //
            // btnAddExistingPersonToThisFamily
            //
            this.btnAddExistingPersonToThisFamily.Location = new System.Drawing.Point(10, 46);
            this.btnAddExistingPersonToThisFamily.Name = "btnAddExistingPersonToThisF" + "amily";
            this.btnAddExistingPersonToThisFamily.Size = new System.Drawing.Size(240, 23);
            this.btnAddExistingPersonToThisFamily.TabIndex = 1;
            this.btnAddExistingPersonToThisFamily.Text = "Move E&xisting Person to Th" + "is Family";
            this.btnAddExistingPersonToThisFamily.Click += new System.EventHandler(this.BtnAddExistingPersonToThisFamily_Click);

            //
            // btnFamilyIDHelp
            //
            this.btnFamilyIDHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnFamilyIDHelp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFamilyIDHelp.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnFamilyIDHelp.Image")));
            this.btnFamilyIDHelp.Location = new System.Drawing.Point(638, 34);
            this.btnFamilyIDHelp.Name = "btnFamilyIDHelp";
            this.btnFamilyIDHelp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnFamilyIDHelp.Size = new System.Drawing.Size(20, 19);
            this.btnFamilyIDHelp.TabIndex = 3;
            this.ToolTip1.SetToolTip(this.btnFamilyIDHelp, "Family ID Explained");
            this.btnFamilyIDHelp.Click += new System.EventHandler(this.BtnFamilyIDHelp_Click);

            //
            // grpFamilyMembersFake
            //
            this.grpFamilyMembersFake.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFamilyMembersFake.Location = new System.Drawing.Point(4, 4);
            this.grpFamilyMembersFake.Name = "grpFamilyMembersFake";
            this.grpFamilyMembersFake.Size = new System.Drawing.Size(676, 308);
            this.grpFamilyMembersFake.TabIndex = 0;
            this.grpFamilyMembersFake.TabStop = false;
            this.grpFamilyMembersFake.Text = "Family Members";

            //
            // pnlFamilyInformation
            //
            this.pnlFamilyInformation.Controls.Add(this.grpFamily);
            this.pnlFamilyInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFamilyInformation.Location = new System.Drawing.Point(0, 0);
            this.pnlFamilyInformation.Name = "pnlFamilyInformation";
            this.pnlFamilyInformation.Size = new System.Drawing.Size(739, 44);
            this.pnlFamilyInformation.TabIndex = 11;

            //
            // grpFamily
            //
            this.grpFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFamily.Controls.Add(this.btnChangeFamily);
            this.grpFamily.Controls.Add(this.btnEditFamily);
            this.grpFamily.Controls.Add(this.txtFamilyPartnerKeyTextBox);
            this.grpFamily.Location = new System.Drawing.Point(4, 0);
            this.grpFamily.Name = "grpFamily";
            this.grpFamily.Size = new System.Drawing.Size(676, 40);
            this.grpFamily.TabIndex = 0;
            this.grpFamily.TabStop = false;
            this.grpFamily.Text = "Family";

            //
            // btnChangeFamily
            //
            this.btnChangeFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnChangeFamily.Location = new System.Drawing.Point(550, 14);
            this.btnChangeFamily.Name = "btnChangeFamily";
            this.btnChangeFamily.Size = new System.Drawing.Size(116, 23);
            this.btnChangeFamily.TabIndex = 2;
            this.btnChangeFamily.Text = "Chan&ge Family...";
            this.btnChangeFamily.Click += new System.EventHandler(this.BtnChangeFamily_Click);

            //
            // btnEditFamily
            //
            this.btnEditFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnEditFamily.Location = new System.Drawing.Point(428, 14);
            this.btnEditFamily.Name = "btnEditFamily";
            this.btnEditFamily.Size = new System.Drawing.Size(116, 23);
            this.btnEditFamily.TabIndex = 1;
            this.btnEditFamily.Text = "Edit F&amily";
            this.btnEditFamily.Click += new System.EventHandler(this.BtnEditFamily_Click);

            //
            // txtFamilyPartnerKeyTextBox
            //
            this.txtFamilyPartnerKeyTextBox.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFamilyPartnerKeyTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.txtFamilyPartnerKeyTextBox.Font = new System.Drawing.Font("Courie" + "r New", 9.25f, System.Drawing.FontStyle.Bold);
            this.txtFamilyPartnerKeyTextBox.LabelText = "Family Key";
            this.txtFamilyPartnerKeyTextBox.Location = new System.Drawing.Point(12, 15);
            this.txtFamilyPartnerKeyTextBox.MaxLength = 10;
            this.txtFamilyPartnerKeyTextBox.Name = "txtFamilyPartnerKeyTextBox";
            this.txtFamilyPartnerKeyTextBox.PartnerKey = (Int64)123456789;
            this.txtFamilyPartnerKeyTextBox.ReadOnly = true;
            this.txtFamilyPartnerKeyTextBox.ShowLabel = true;
            this.txtFamilyPartnerKeyTextBox.Size = new System.Drawing.Size(232, 22);
            this.txtFamilyPartnerKeyTextBox.TabIndex = 0;
            this.txtFamilyPartnerKeyTextBox.TextBoxReadOnly = true;
            this.txtFamilyPartnerKeyTextBox.TextBoxWidth = 80;

            //
            // TUC_FamilyMembers
            //
            this.Controls.Add(this.pnlPartnerFoundationDetails);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_FamilyMembers";
            this.Size = new System.Drawing.Size(739, 360);
            this.KeyPress += new KeyPressEventHandler(this.BtnRefreshDataGrid_KeyPress);
            this.pnlPartnerFoundationDetails.ResumeLayout(false);
            this.pnlFamilyMembers.ResumeLayout(false);
            this.pnlFamilyArranger.ResumeLayout(false);
            this.grpChangeFamilyID.ResumeLayout(false);
            this.grpFamilyMembersModify.ResumeLayout(false);
            this.pnlFamilyInformation.ResumeLayout(false);
            this.grpFamily.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Custom Event
        /// </summary>
        /// <returns>void</returns>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// Prepares the arRow buttons, when the FamilyMembers screen is opened first time
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void PrepareArRowButtons()
        {
            // The demote and promote buttons are disables if only one Family member exists
            if ((FLogic.GetNumberOfRows() == 1) && (FLogic.GetFamilyID() == 0))
            {
                this.btnFamilyMemberPromote.Enabled = false;
                this.btnFamilyMemberDemote.Enabled = false;
            }
            else
            {
                // If Selected FamilyID is zero, Demote button is disabled
                if (FLogic.GetFamilyID() == 0)
                {
                    this.btnFamilyMemberDemote.Enabled = false;
                }

                // If Selected FamilyID is maximum, Promote button is disabled
                if (FLogic.IsMaximum())
                {
                    this.btnFamilyMemberPromote.Enabled = false;
                }
            }
        }

        private void RefreshGrid()
        {
            if (FLogic.GridEdited)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (!FLogic.DataGridExist())
                {
                    try
                    {
                        if (FLogic.LoadDataOnDemand())
                        {
                            // Create SourceDataGrid columns
                            FLogic.CreateColumns();

                            // DataBinding
                            FLogic.DataBindGrid();

                            // Setup the DataGrid's visual appearance
                            SetupDataGridVisualAppearance();

                            // Prepare the Demote and Promote buttons first time
                            PrepareArRowButtons();

                            // Hook up event that fires when a different Row is selected
                            grdFamilyMembers.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
                            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFamilyMembers));
                            this.btnEditPerson.Enabled = true;
                            this.btnMovePersonToOtherFamily.Enabled = true;
                            this.btnEditFamilyID.Enabled = true;
                            ApplySecurity();
                        }
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                else
                {
                    if (FLogic.LoadDataOnDemand())
                    {
                        // One or more Family Members present > select first one in Grid
                        grdFamilyMembers.Selection.SelectRow(1, true);

                        // Make the Grid respond on updown keys
                        grdFamilyMembers.Focus();
                        btnEditPerson.Enabled = true;
                        btnMovePersonToOtherFamily.Enabled = true;
                        btnEditFamilyID.Enabled = true;

                        // Prepare the Demote and Promote buttons
                        SetArRowButtons();
                    }
                    else
                    {
                        // No Family Member present > disable buttons that are no longer relevant
                        btnFamilyMemberDemote.Enabled = false;
                        btnFamilyMemberPromote.Enabled = false;
                        btnEditPerson.Enabled = false;
                        btnMovePersonToOtherFamily.Enabled = false;
                        btnEditFamilyID.Enabled = false;

                        // Prepare the Demote and Promote buttons
                        PrepareArRowButtons();
                    }

                    ApplySecurity();
                }
            }
        }

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        #endregion

        protected void ChangeFamily(Int64 APartnerKey, Boolean AChangeToThisFamily)
        {
// TODO change family
#if TODO
            TCmdMPartner cmd;

            if (FDelegateIsNewPartner != null)
            {
                if (FDelegateIsNewPartner(FMainDS))
                {
                    MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorChangeFamily2,
                        Resourcestrings.StrErrorNeedToSavePartnerTitle);
                    return;
                }

                cmd = new TCmdMPartner();

                if (AChangeToThisFamily)
                {
                    cmd.RunChangeFamily(this, APartnerKey, MaintainChangeFamilyCallback, FMainDS.PFamily[0].PartnerKey, false);
                }
                else
                {
                    cmd.RunChangeFamily(this, APartnerKey, MaintainChangeFamilyCallback);
                }
            }
            else
            {
                throw new ApplicationException("Delegate FDelegateIsNewPartner is not set up");
            }
#endif
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_FamilyMembers() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FDeadlineEditMode = false;
            FDeadLineEditMode2 = false;

            // define the screen's logic
            FLogic = new TUCFamilyMembersLogic();
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for Petra screens
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

        /// <summary>
        /// Opens a help Messagebox to explain the FamilyID
        /// </summary>
        /// <returns>void</returns>
        private void BtnFamilyIDHelp_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show(StrFamilyIDExplained, "Family ID Explained");
        }

        /// <summary>
        /// Shows the Tooltip
        /// </summary>
        /// <returns>void</returns>
        private void BtnRefreshDataGrid_MouseEnter(System.Object sender, System.EventArgs e)
        {
            if (TipRefresh.Active)
            {
                // strange; there was no assignment in delphi.net; but that is not allowed in c#
            }
        }

        /// <summary>
        /// refreshs the Datagrid
        /// </summary>
        /// <returns>void</returns>
        private void BtnRefreshDataGrid_Click(System.Object sender, System.EventArgs e)
        {
            this.RefreshGrid();
        }

        /// <summary>
        /// refreshs the datagrid, when keypressed, when this button is selected (quite useless procedure)
        /// </summary>
        /// <returns>void</returns>
        private void BtnRefreshDataGrid_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

// TODO maintain screens in 4gl still
#if TODO
        private void MaintainChangeFamilyCallback(TCommand cmd, ref Boolean bExpectAnother)
        {
            Int64 NewFamilyKey;
            Boolean ShowNewFamilyMembers;

            if (cmd.commandName == "SUCCESS")
            {
                NewFamilyKey = cmd.GetIntegerParam(0);
                ShowNewFamilyMembers = cmd.GetBooleanParam(1);

                if (NewFamilyKey != -1)
                {
                    if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                    {
                        // Callback comes after process invoked by clicking btnChangeFamily
                        FMainDS.PPerson[0].FamilyKey = NewFamilyKey;

                        // Update Family GroupBox
                        txtFamilyPartnerKeyTextBox.PartnerKey = NewFamilyKey;
                        txtFamilyPartnerKeyTextBox.LabelText = RetrievePartnerShortName(NewFamilyKey);
                    }

                    if (ShowNewFamilyMembers)
                    {
                        TApplinkOrNetAutoSelector.OpenPartnerEditScreen(this.ParentForm,
                            NewFamilyKey,
                            TPartnerEditTabPageEnum.petpFamilyMembers);
                    }

                    // Refresh DataGrid to show the changed Family Members
                    BtnRefreshDataGrid_Click(this, null);
                }
                else
                {
                }

                // Process was cancelled
                bExpectAnother = false;

                // FLogic.LoadDataOnDemand();
            }
            else if (cmd.commandName == "ERROR")
            {
                // a message why the person could not be moved (e.g. Unable to change family for partner xyz Message Number: MA1003)
                MessageBox.Show(this, cmd.GetStringParam(0), "Error");
                bExpectAnother = false;
            }
            else
            {
                // error, no selection accepted
                bExpectAnother = false;
            }
        }
#endif



        /// <summary>
        /// Changes the family
        /// </summary>
        /// <returns>void</returns>
        private void BtnChangeFamily_Click(System.Object sender, System.EventArgs e)
        {
            ChangeFamily(FMainDS.PPartner[0].PartnerKey, false);
        }

        /// <summary>
        /// opens a familyfind screen for selected person to select new family
        /// </summary>
        /// <returns>void</returns>
        private void BtnMovePersonToOtherFamily_Click(System.Object sender, System.EventArgs e)
        {
            // MessageBox.Show('jjj');
            if (FLogic.GridEdited)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                ChangeFamily(FLogic.GetPartnerKeySelected(), false);
            }
        }

        /// <summary>
        /// Opens Edit Partner screen to add new person to this Family
        /// </summary>
        /// <returns>void</returns>
        private void BtnAddNewPersonThisFamily_Click(System.Object sender, System.EventArgs e)
        {
#if TODO
            /// frmPEDS: TPartnerEditDSWinForm;
            Int32 FamilysCurrentLocationKey;
            Int64 FamilysCurrentSiteKey;
            TCommand cmd;

            if (FLogic.GridEdited)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (FDelegateIsNewPartner(FMainDS))
                {
                    MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorChangeFamily2,
                        Resourcestrings.StrErrorNeedToSavePartnerTitle);
                    return;
                }

                if (GetLocationRowOfCurrentlySelectedAddress != null)
                {
                    FamilysCurrentLocationKey = GetLocationRowOfCurrentlySelectedAddress().LocationKey;
                    FamilysCurrentSiteKey = GetLocationRowOfCurrentlySelectedAddress().SiteKey;
                }
                else
                {
                    throw new ApplicationException("Delegate FGetLocationKeyOfCurrentlySelectedAddress is not set up");
                }

                TApplinkOrNetAutoSelector.OpenNewPartnerFamilyNewPersonDialog(
                    this.ParentForm,
                    FMainDS.PFamily[0].PartnerKey,
                    new TLocationPK(FamilysCurrentSiteKey, FamilysCurrentLocationKey));
            }
#endif
        }

        /// <summary>
        /// opens the family for editing
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditFamily_Click(System.Object sender, System.EventArgs e)
        {
// TODO BtnEditFamily_Click
#if TODO
            TApplinkOrNetAutoSelector.OpenPartnerEditScreen(this.ParentForm,
                FMainDS.PPerson[0].FamilyKey,
                TPartnerEditTabPageEnum.petpFamilyMembers);
#endif
        }

        /// <summary>
        /// when double clicked datagrid, opens selected FamilyID for editing
        /// </summary>
        /// <returns>void</returns>
        private void GrdFamilyMembers_DoubleClick(System.Object sender, System.EventArgs e)
        {
            if (!FDeadlineEditMode)
            {
                if (FLogic.DataGridExist() && FLogic.MembersInFamilyExist())
                {
                    BtnEditPerson_Click(this, null);
                }
            }
        }

        /// <summary>
        /// what to do, when down key is pressed within the DataGrid
        /// </summary>
        /// <returns>void</returns>
        private void GrdFamilyMembers_KeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 116:
                    this.RefreshGrid();

                    // F5 key
                    break;

                case 38:

                    if (e.Control == true)
                    {
                        FLogic.DemoteFamilyID();
                    }

                    break;

                case 40:

                    if (e.Control == true)
                    {
                        FLogic.PromoteFamilyID();
                    }

                    break;
            }
        }

        /// <summary>
        /// Click2 will set to the Combobox the values (1,2,3,4,5,6,7,8,9,0) and if selected a excisting Family ID, gives a message to select anothed value.
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditFamilyID_Click2(System.Object sender, System.EventArgs e)
        {
            FDeadlineEditMode = (!FDeadlineEditMode);

            // if the "edit FamilyID" button is pressed, disables screenparts, enables the combobox
            if (FDeadlineEditMode)
            {
                this.EnableScreenParts(!FDeadlineEditMode);
                OnEnableDisableOtherScreenParts(new TEnableDisableEventArgs(!FDeadlineEditMode));
                btnEditFamilyID.Focus();

                // looks stupid, but is necessary when the keyboard is used!
                FLogic.OpenComboBox();
                btnEditFamilyID.Text = CommonResourcestrings.StrBtnTextDone;
                this.PrepareArRowButtons();
            }
            // if the "DONE" button is pressed, enables screenparts, disables combobox
            else
            {
                btnEditFamilyID.Focus();

                // looks stupid, but is necessary when the keyboard is used!
                btnEditFamilyID.Text = StrBtnTextManual + CommonResourcestrings.StrBtnTextEdit;
                this.EnableScreenParts(!FDeadlineEditMode);
                OnEnableDisableOtherScreenParts(new TEnableDisableEventArgs(!FDeadlineEditMode));
                FLogic.DisableEditing();
                this.PrepareArRowButtons();
            }
        }

        /// <summary>
        /// Opens Partner Find screen (type: person) to find existing person to add to this family
        /// </summary>
        /// <returns>void</returns>
        private void BtnAddExistingPersonToThisFamily_Click(System.Object sender, System.EventArgs e)
        {
            Int64 PartnerKey;
            String ShortName;
            Int32 LocationKey;
            Int64 SiteKey;

            if (FLogic.GridEdited)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 +
                    Resourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (FDelegateIsNewPartner(FMainDS))
                {
                    MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 +
                        Resourcestrings.StrErrorChangeFamily2,
                        Resourcestrings.StrErrorNeedToSavePartnerTitle);
                    return;
                }

// TODO partner find
#if TODO
                CmdMPartner = new TCmdMPartner();
                CmdMPartner.OpenModalScreenPartnerFind("PERSON", this.ParentForm, out PartnerKey, out ShortName, out SiteKey, out LocationKey);
                ChangeFamily(PartnerKey, true);
#endif
            }
        }

        /// <summary>
        /// Opens Edit Partner screen for selected Familymember.
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditPerson_Click(System.Object sender, System.EventArgs e)
        {
            // frmPEDS: TPartnerEditDSWinForm;
            // FDefaultTabPage:
            if (FLogic.GridEdited)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                // MessageBox.Show(frmPeds.GetNewPartnerKey.ToString);
                if (FMainDS.PPartner[0].PartnerKey == FLogic.GetPartnerKeySelected())
                {
                    MessageBox.Show(StrAlreadyOpen,
                        StrAlreadyOpenTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                }
                else
                {
// TODO partner edit link
#if TODO
                    TApplinkOrNetAutoSelector.OpenPartnerEditScreen(this.ParentForm,
                        FLogic.GetPartnerKeySelected(),
                        TPartnerEditTabPageEnum.petpFamilyMembers);
#endif
                }
            }
        }

        /// <summary>
        /// Promotes selected ID (and demotes the FamilyID next (up) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        private void BtnFamilyMemberPromote_Click(System.Object sender, System.EventArgs e)
        {
            FLogic.PromoteFamilyID();

            // Here could only be checked the maximum values.
            this.SetArRowButtons();
        }

        /// <summary>
        /// Demotes selected ID (and promotes the FamilyID next (lower) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        private void BtnFamilyMemberDemote_Click(System.Object sender, System.EventArgs e)
        {
            FLogic.DemoteFamilyID();

            // Here could only be checked the minimum values.
            this.SetArRowButtons();
        }

        /// <summary>
        /// procedure grdFamilyMembers_ClickCell(Sender: System.Object; e: SourceGrid.CellContextEventArgs); Sets the ArRowbuttons (Demote,Promote). Disables or enables them. depending of selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        private void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            this.SetArRowButtons();
        }

        /// <summary>
        /// enables the screenparts, if false, disables.
        /// </summary>
        /// <returns>void</returns>
        protected void EnableScreenParts(Boolean Value)
        {
            this.btnFamilyMemberDemote.Enabled = Value;
            this.btnFamilyMemberPromote.Enabled = Value;
            this.btnMovePersonToOtherFamily.Enabled = Value;
            this.btnEditPerson.Enabled = Value;
            this.btnAddExistingPersonToThisFamily.Enabled = Value;
            this.btnAddNewPersonThisFamily.Enabled = Value;
            ApplySecurity();
        }

        protected Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            // MessageBox.Show('TUC_FamilyMembers.GetPartnerShortName got called');
            return FPartnerEditUIConnector.GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass);
        }

        /// <summary>
        /// disposes
        /// </summary>
        /// <returns>void</returns>
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

        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateIsNewPartner = ADelegateFunction;
        }

        public void InitialiseUserControl()
        {
            // Show/hide parts of the UserControl according to Partner Class of the Partner
            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                pnlFamilyInformation.Visible = false;
                grpFamilyMembersFake.Visible = false;
                pnlFamilyArranger.Location = new System.Drawing.Point(4, 4);
            }
            else
            {
                grpChangeFamilyID.Visible = false;
                grpFamilyMembersModify.Visible = false;
                btnFamilyIDHelp.Visible = false;

                // Set up Family Partner Information
                FDelegateGetPartnerShortName = @GetPartnerShortName;
                txtFamilyPartnerKeyTextBox.PartnerKey = FMainDS.PPerson[0].FamilyKey;
                txtFamilyPartnerKeyTextBox.LabelText = RetrievePartnerShortName(FMainDS.PPerson[0].FamilyKey);
            }

            // Set up screen logic
            FLogic.MainDS = FMainDS;
            FLogic.DataGrid = grdFamilyMembers;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;

            // Hook up RecalculateScreenParts Event that is fired by FLogic
            FLogic.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RethrowRecalculateScreenParts);

            // Check if data needs to be retrieved from the PetraServer
            if (FMainDS.FamilyMembers == null)
            {
                FFamilyMembersExist = FLogic.LoadDataOnDemand();
            }
            else
            {
                FMainDS.InitVars();
                FFamilyMembersExist = FMainDS.FamilyMembers.Rows.Count > 0;
            }

            // If Family Members exist, then DataGrid is created.
            if (FFamilyMembersExist)
            {
                // Create SourceDataGrid columns
                FLogic.CreateColumns();

                // DataBinding
                FLogic.DataBindGrid();

                // Setup the DataGrid's visual appearance
                SetupDataGridVisualAppearance();

                // Prepare the Demote and Promote buttons first time
                PrepareArRowButtons();

                // Hook up event that fires when a different Row is selected
                grdFamilyMembers.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
                OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFamilyMembers));
            }
            else
            {
                // If Family has no members, these buttons are disabled
                this.btnFamilyMemberDemote.Enabled = false;
                this.btnFamilyMemberPromote.Enabled = false;
                this.btnMovePersonToOtherFamily.Enabled = false;
                this.btnEditPerson.Enabled = false;
                this.btnEditFamilyID.Enabled = false;

                // this.btnAddExistingPersonToThisFamily.enabled := false;
                // this.btnAddNewPersonThisFamily.enabled := false;
            }

            ApplySecurity();
        }

        /// <summary>
        /// Sets the ArRowbuttons (Demote,Promote). Disables or enables them. depending
        /// of selected FamilyID.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetArRowButtons()
        {
            // If selected FamilyID is minimum, Demote button is disabled
            if (FLogic.IsMinimum() && (FLogic.GetFamilyID() == 0))
            {
                this.btnFamilyMemberDemote.Enabled = false;
            }
            else
            {
                this.btnFamilyMemberDemote.Enabled = true;
            }

            // If selected FamilyID is maximum, Promote button is disabled
            if (FLogic.IsMaximum())
            {
                this.btnFamilyMemberPromote.Enabled = false;
            }
            else
            {
                this.btnFamilyMemberPromote.Enabled = true;
            }

            ApplySecurity();
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridVisualAppearance()
        {
            /*
             * HACK ***Temporary*** solution to make the Grid appear
             * correctly on "Large Fonts (120DPI)" display setting:
             * Do not call .AutoSize on it; on 120DPI it causes the Grid to
             * become very wide and this pushes the Add, Edit and Delete
             * buttons off the screen!
             * No solution found yet for 120DPI...  :-(
             */
            if (!TClientSettings.GUIRunningOnNonStandardDPI)
            {
                grdFamilyMembers.AutoSizeCells();
            }
        }

        protected String RetrievePartnerShortName(Int64 APartnerKey)
        {
            String ReturnValue;
            string PartnerShortName;
            TPartnerClass PartnerClass;

            ReturnValue = "";

            if (this.FDelegateGetPartnerShortName != null)
            {
                try
                {
                    FDelegateGetPartnerShortName(APartnerKey, out PartnerShortName, out PartnerClass);
                    ReturnValue = PartnerShortName;
                }
                finally
                {
                }

                // raise EVerificationMissing.Create('this.FDelegateGetPartnerShortName could not be called!');
            }

            return ReturnValue;
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
            {
                // need to disable all Buttons that allow modification of p_person record
                CustomEnablingDisabling.DisableControl(grpFamily, btnChangeFamily);
                CustomEnablingDisabling.DisableControlGroup(grpChangeFamilyID);
                CustomEnablingDisabling.DisableControlGroup(grpFamilyMembersModify);
            }
        }

        /// <summary>
        /// calls FEnableDisableOtherScreenParts, if assigned
        /// </summary>
        /// <returns>void</returns>
        protected void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (FEnableDisableOtherScreenParts != null)
            {
                FEnableDisableOtherScreenParts(this, e);
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

    /// <summary>Delegate declaration</summary>
    public delegate PLocationRow TDelegateGetLocationRowOfCurrentlySelectedAddress();
    public delegate bool TDelegateIsNewPartner(PartnerEditTDS AInspectDataSet);
}