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
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Resources;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// UserControl that makes up the TabControl for 'Partner Data' in Partner Edit screen.
    /// </summary>
    public class TUC_PartnerEdit_PartnerTabSet : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary>todoComment</summary>
        public const String StrAddressesTabHeader = "Addresses";

        /// <summary>todoComment</summary>
        public const String StrSubscriptionsTabHeader = "Subscriptions";

        /// <summary>todoComment</summary>
        public const String StrSpecialTypesTabHeader = "Special Types";

        /// <summary>todoComment</summary>
        public const String StrFamilyMembersTabHeader = "Family Members";

        /// <summary>todoComment</summary>
        public const String StrFamilyTabHeader = "Family";

        /// <summary>todoComment</summary>
        public const String StrInterestsTabHeader = "Interests";

        /// <summary>todoComment</summary>
        public const String StrNotesTabHeader = "Notes";

        /// <summary>todoComment</summary>
        public const String StrAddressesSingular = "Address";

        /// <summary>todoComment</summary>
        public const String StrSubscriptionsSingular = "Subscription";

        /// <summary>todoComment</summary>
        public const String StrTabHeaderCounterTipSingular = "{0} {2}, of which {1} is ";

        /// <summary>todoComment</summary>
        public const String StrTabHeaderCounterTipPlural = "{0} {2}, of which {1} are ";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private TTabVersatile tabPartnerTab;
        private System.Windows.Forms.TabPage tbpPartnerDetails;
        private System.Windows.Forms.Panel pnlAddresses;
        private System.Windows.Forms.Panel pnlPersonDetails;
        private System.Windows.Forms.Panel pnlSubscriptions;
        private System.Windows.Forms.Panel pnlPartnerTypes;
        private System.Windows.Forms.Panel pnlPartnerNotes;
        private System.Windows.Forms.Panel pnlFamilyMembers;
        private System.Windows.Forms.Panel pnlPartnerOfficeSpecific;
        private System.Windows.Forms.Panel pnlInterests;
        private System.Windows.Forms.ImageList imlAddressIcons;
        private System.Windows.Forms.ImageList imlTabIcons;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.TabPage tbpPartnerTypes;
        private System.Windows.Forms.TabPage tbpRelationships;
        private System.Windows.Forms.TabPage tbpFamilyMembers;
        private System.Windows.Forms.TabPage tbpContacts;
        private System.Windows.Forms.TabPage tbpNotes;
        private System.Windows.Forms.TabPage tbpInterests;
        private System.Windows.Forms.TabPage tbpReminders;
        private System.Windows.Forms.TabPage tbpAddresses;
        private System.Windows.Forms.TabPage tbpSubscriptions;
        private System.Windows.Forms.Label lblAddressesDynamicLoadInfo;
        private System.Windows.Forms.Label lblPartnerDetailsDynamicLoadInfo;
        private System.Windows.Forms.Label lblSubscriptionsDynamicLoadInfo;
        private System.Windows.Forms.Label lblPartnerTypesDynamicLoadInfo;
        private System.Windows.Forms.Label lblPartnerNotesDynamicLoadInfo;
        private System.Windows.Forms.Label lblFamilyMembersDynamicLoadInfo;
        private System.Windows.Forms.Label lblPartnerOfficeSpecificDynamicLoadInfo;
        private System.Windows.Forms.Label lblInterestsDynamicLoadInfo;
        private System.Windows.Forms.TabPage tbpFoundationDetails;
        private System.Windows.Forms.Panel pnlFoundationContainer;
        private System.Windows.Forms.Label lblFoundationDetailsDynamicLoadInfo;
        private System.Windows.Forms.TabPage tbpOfficeSpecific;
        private System.Windows.Forms.Label lblRemindersDynamicLoadInfo;
        private System.Windows.Forms.Panel pnlReminders;
        private System.Windows.Forms.Label lblRelationshipsDynamicLoadInfo;
        private System.Windows.Forms.Panel pnlRelationships;
        private System.Windows.Forms.Label lblContactsDynamicLoadInfo;
        private System.Windows.Forms.Panel pnlContacts;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private PartnerEditTDS FMainDS;
        private TDelegateIsNewPartner FDelegateIsNewPartner;
        private String FPartnerClass;
        private TPartnerEditTabPageEnum FInitiallySelectedTabPage;
        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;
        private TUCPartnerAddresses FUcoPartnerAddresses;
        private TUC_PartnerDetailsPerson FUcoPartnerDetailsPerson;

// TODO        private TUCPartnerSubscriptions FUcoPartnerSubscriptions;
        private TUCPartnerTypes FUcoPartnerTypes;

// TODO        private TUC_PartnerDetailsFamily FUcoPartnerDetailsFamily;
// TODO        private TUC_PartnerDetailsChurch FUcoPartnerDetailsChurch;
// TODO        private TUC_PartnerDetailsOrganisation FUcoPartnerDetailsOrganisation;
// TODO        private TUC_PartnerDetailsUnit FUcoPartnerDetailsUnit;
// TODO        private TUC_PartnerDetailsBank FUcoPartnerDetailsBank;
// TODO        private TUC_PartnerDetailsVenue FUcoPartnerDetailsVenue;
// TODO        private TUC_PartnerFoundation FUcoFoundation;
// TODO        private TUC_FamilyMembers FUcoFamilyMembers;
        private TUC_PartnerNotes FUcoPartnerNotes;

// TODO        private TUC_PartnerOfficeSpecific FUcoPartnerOfficeSpecific;
// TODO        private TUCPartnerInterests FUcoInterests;
// TODO        private TUCPartnerContacts FUcoContacts;
// TODO        private TUCPartnerRelationships FUcoRelationships;
// TODO        private TUCPartnerReminders FUcoReminders;
// TODO                private Boolean FTabSetupPartnerSubscriptions;
        private Boolean FTabSetupPartnerTypes;
        private Boolean FTabSetupPartnerAddresses;
        private Boolean FTabSetupPartnerDetails;

// TODO                private Boolean FTabSetupFoundation;
// TODO                private Boolean FTabSetupFamilyMembers;
// TODO                private Boolean FTabSetupOfficeSpecific;
        private Boolean FTabSetupPartnerNotes;

// TODO                private Boolean FTabSetupInterests;
// TODO                private Boolean FTabSetupContacts;
// TODO                private Boolean FTabSetupReminders;
// TODO                private Boolean FTabSetupRelationships;
        private Boolean FUserControlInitialised;

        /// <summary>todoComment</summary>
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

        /// <summary>todoComment</summary>
        public TPartnerEditTabPageEnum InitiallySelectedTabPage
        {
            get
            {
                return FInitiallySelectedTabPage;
            }

            set
            {
                FInitiallySelectedTabPage = value;
            }
        }

        /// <summary>todoComment</summary>
        public TPartnerEditTabPageEnum CurrentlySelectedTabPage
        {
            get
            {
                return FCurrentlySelectedTabPage;
            }

            set
            {
                FCurrentlySelectedTabPage = value;
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

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public PLocationRow Get_LocationRowOfCurrentlySelectedAddress()
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            return FUcoPartnerAddresses.LocationDataRowOfCurrentlySelectedRecord;
        }

        /// <summary>todoComment</summary>
        public PLocationRow LocationRowOfCurrentlySelectedAddress
        {
            get
            {
                return Get_LocationRowOfCurrentlySelectedAddress();
            }
        }

        /// <summary>todoComment</summary>
        public PartnerEditTDSPPartnerLocationRow PartnerLocationRowOfCurrentlySelectedAddress
        {
            get
            {
                if (!FTabSetupPartnerAddresses)
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    SetUpPartnerAddress();
                }

                return FUcoPartnerAddresses.PartnerLocationDataRowOfCurrentlySelectedRecord;
            }
        }

        /// <summary>todoComment</summary>
        public Boolean LocationBeingAdded
        {
            get
            {
                if (!FTabSetupPartnerAddresses)
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    SetUpPartnerAddress();
                }

                return FUcoPartnerAddresses.IsRecordBeingAdded;
            }
        }

        /// <summary>todoComment</summary>
        public event System.EventHandler DataLoadingStarted;

        /// <summary>todoComment</summary>
        public event System.EventHandler DataLoadingFinished;

        /// <summary>todoComment</summary>
        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        /// <summary>todoComment</summary>
        public event THookupDataChangeEventHandler HookupDataChange;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupPartnerEditDataChange;

        /// <summary>todoComment</summary>
        public event TShowTabEventHandler ShowTab;

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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_PartnerTabSet));
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.imlTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.imlAddressIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabPartnerTab = new Ict.Common.Controls.TTabVersatile();
            this.tbpAddresses = new System.Windows.Forms.TabPage();
            this.pnlAddresses = new System.Windows.Forms.Panel();
            this.lblAddressesDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpPartnerDetails = new System.Windows.Forms.TabPage();
            this.pnlPersonDetails = new System.Windows.Forms.Panel();
            this.lblPartnerDetailsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpFoundationDetails = new System.Windows.Forms.TabPage();
            this.pnlFoundationContainer = new System.Windows.Forms.Panel();
            this.lblFoundationDetailsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpSubscriptions = new System.Windows.Forms.TabPage();
            this.pnlSubscriptions = new System.Windows.Forms.Panel();
            this.lblSubscriptionsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpPartnerTypes = new System.Windows.Forms.TabPage();
            this.pnlPartnerTypes = new System.Windows.Forms.Panel();
            this.lblPartnerTypesDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpContacts = new System.Windows.Forms.TabPage();
            this.tbpFamilyMembers = new System.Windows.Forms.TabPage();
            this.pnlFamilyMembers = new System.Windows.Forms.Panel();
            this.lblFamilyMembersDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpRelationships = new System.Windows.Forms.TabPage();
            this.tbpInterests = new System.Windows.Forms.TabPage();
            this.pnlInterests = new System.Windows.Forms.Panel();
            this.lblInterestsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpReminders = new System.Windows.Forms.TabPage();
            this.tbpNotes = new System.Windows.Forms.TabPage();
            this.pnlPartnerNotes = new System.Windows.Forms.Panel();
            this.lblPartnerNotesDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tbpOfficeSpecific = new System.Windows.Forms.TabPage();
            this.pnlPartnerOfficeSpecific = new System.Windows.Forms.Panel();
            this.lblPartnerOfficeSpecificDynamicLoadInfo = new System.Windows.Forms.Label();
            this.pnlContacts = new System.Windows.Forms.Panel();
            this.lblContactsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.pnlRelationships = new System.Windows.Forms.Panel();
            this.lblRelationshipsDynamicLoadInfo = new System.Windows.Forms.Label();
            this.pnlReminders = new System.Windows.Forms.Panel();
            this.lblRemindersDynamicLoadInfo = new System.Windows.Forms.Label();
            this.tabPartnerTab.SuspendLayout();
            this.tbpAddresses.SuspendLayout();
            this.pnlAddresses.SuspendLayout();
            this.tbpPartnerDetails.SuspendLayout();
            this.pnlPersonDetails.SuspendLayout();
            this.tbpFoundationDetails.SuspendLayout();
            this.pnlFoundationContainer.SuspendLayout();
            this.tbpSubscriptions.SuspendLayout();
            this.pnlSubscriptions.SuspendLayout();
            this.tbpPartnerTypes.SuspendLayout();
            this.pnlPartnerTypes.SuspendLayout();
            this.tbpContacts.SuspendLayout();
            this.tbpFamilyMembers.SuspendLayout();
            this.pnlFamilyMembers.SuspendLayout();
            this.tbpRelationships.SuspendLayout();
            this.tbpInterests.SuspendLayout();
            this.pnlInterests.SuspendLayout();
            this.tbpReminders.SuspendLayout();
            this.tbpNotes.SuspendLayout();
            this.pnlPartnerNotes.SuspendLayout();
            this.tbpOfficeSpecific.SuspendLayout();
            this.pnlPartnerOfficeSpecific.SuspendLayout();
            this.pnlContacts.SuspendLayout();
            this.pnlRelationships.SuspendLayout();
            this.pnlReminders.SuspendLayout();
            this.SuspendLayout();

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // imlTabIcons
            //
            this.imlTabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlTabIcons.ImageStream")));
            this.imlTabIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlTabIcons.Images.SetKeyName(0, "");
            this.imlTabIcons.Images.SetKeyName(1, "");
            this.imlTabIcons.Images.SetKeyName(2, "");
            this.imlTabIcons.Images.SetKeyName(3, "");
            this.imlTabIcons.Images.SetKeyName(4, "");
            this.imlTabIcons.Images.SetKeyName(5, "");
            this.imlTabIcons.Images.SetKeyName(6, "");
            this.imlTabIcons.Images.SetKeyName(7, "");
            this.imlTabIcons.Images.SetKeyName(8, "");
            this.imlTabIcons.Images.SetKeyName(9, "");
            this.imlTabIcons.Images.SetKeyName(10, "");
            this.imlTabIcons.Images.SetKeyName(11, "");

            //
            // imlAddressIcons
            //
            this.imlAddressIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imlAddressIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlAddressIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // tabPartnerTab
            //
            this.tabPartnerTab.AllowDrop = true;
            this.tabPartnerTab.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.tabPartnerTab.Controls.Add(this.tbpAddresses);
            this.tabPartnerTab.Controls.Add(this.tbpPartnerDetails);
            this.tabPartnerTab.Controls.Add(this.tbpFoundationDetails);
            this.tabPartnerTab.Controls.Add(this.tbpSubscriptions);
            this.tabPartnerTab.Controls.Add(this.tbpPartnerTypes);
            this.tabPartnerTab.Controls.Add(this.tbpContacts);
            this.tabPartnerTab.Controls.Add(this.tbpFamilyMembers);
            this.tabPartnerTab.Controls.Add(this.tbpRelationships);
            this.tabPartnerTab.Controls.Add(this.tbpInterests);
            this.tabPartnerTab.Controls.Add(this.tbpReminders);
            this.tabPartnerTab.Controls.Add(this.tbpNotes);
            this.tabPartnerTab.Controls.Add(this.tbpOfficeSpecific);
            this.tabPartnerTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabPartnerTab.ImageList = this.imlTabIcons;
            this.tabPartnerTab.Location = new System.Drawing.Point(1, 1);
            this.tabPartnerTab.Name = "tabPartnerTab";
            this.tabPartnerTab.SelectedIndex = 1;
            this.tabPartnerTab.ShowToolTips = true;
            this.tabPartnerTab.Size = new System.Drawing.Size(752, 470);
            this.tabPartnerTab.TabIndex = 87;

            //
            // tbpAddresses
            //
            this.tbpAddresses.Controls.Add(this.pnlAddresses);
            this.tbpAddresses.ImageIndex = 0;
            this.tbpAddresses.Location = new System.Drawing.Point(4, 23);
            this.tbpAddresses.Name = "tbpAddresses";
            this.tbpAddresses.Size = new System.Drawing.Size(744, 443);
            this.tbpAddresses.TabIndex = 9;
            this.tbpAddresses.Text = "Addresses";

            //
            // pnlAddresses
            //
            this.pnlAddresses.AutoSize = true;
            this.pnlAddresses.Controls.Add(this.lblAddressesDynamicLoadInfo);
            this.pnlAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAddresses.Location = new System.Drawing.Point(0, 0);
            this.pnlAddresses.Name = "pnlAddresses";
            this.pnlAddresses.Padding = new System.Windows.Forms.Padding(2);
            this.pnlAddresses.Size = new System.Drawing.Size(744, 443);
            this.pnlAddresses.TabIndex = 0;

            //
            // lblAddressesDynamicLoadInfo
            //
            this.lblAddressesDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblAddressesDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblAddressesDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblAddressesDynamicLoadInfo.Name = "lblAddressesDynamicLoadInfo";
            this.lblAddressesDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblAddressesDynamicLoadInfo.TabIndex = 0;
            this.lblAddressesDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlAddresses) in P" +
                                                    "rocedure DynamicLoadUserControl!";
            this.lblAddressesDynamicLoadInfo.Visible = false;

            //
            // tbpPartnerDetails
            //
            this.tbpPartnerDetails.Controls.Add(this.pnlPersonDetails);
            this.tbpPartnerDetails.ImageIndex = 1;
            this.tbpPartnerDetails.Location = new System.Drawing.Point(4, 23);
            this.tbpPartnerDetails.Name = "tbpPartnerDetails";
            this.tbpPartnerDetails.Size = new System.Drawing.Size(744, 443);
            this.tbpPartnerDetails.TabIndex = 1;
            this.tbpPartnerDetails.Text = "Partner Details";

            //
            // pnlPersonDetails
            //
            this.pnlPersonDetails.Controls.Add(this.lblPartnerDetailsDynamicLoadInfo);
            this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPersonDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlPersonDetails.Name = "pnlPersonDetails";
            this.pnlPersonDetails.Padding = new System.Windows.Forms.Padding(2);
            this.pnlPersonDetails.Size = new System.Drawing.Size(744, 443);
            this.pnlPersonDetails.TabIndex = 0;

            //
            // lblPartnerDetailsDynamicLoadInfo
            //
            this.lblPartnerDetailsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblPartnerDetailsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPartnerDetailsDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblPartnerDetailsDynamicLoadInfo.Name = "lblPartnerDetailsDynamicLoadInfo";
            this.lblPartnerDetailsDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblPartnerDetailsDynamicLoadInfo.TabIndex = 0;
            this.lblPartnerDetailsDynamicLoadInfo.Text = "The UserControl that corresponds to the PartnerClass of the Partner is added dyna" +
                                                         "mically at runtime to this Panel (pnlPersonDetails) in Procedure DynamicLoadUser" +
                                                         "Control!";
            this.lblPartnerDetailsDynamicLoadInfo.Visible = false;

            //
            // tbpFoundationDetails
            //
            this.tbpFoundationDetails.Controls.Add(this.pnlFoundationContainer);
            this.tbpFoundationDetails.ImageIndex = 10;
            this.tbpFoundationDetails.Location = new System.Drawing.Point(4, 23);
            this.tbpFoundationDetails.Name = "tbpFoundationDetails";
            this.tbpFoundationDetails.Size = new System.Drawing.Size(744, 443);
            this.tbpFoundationDetails.TabIndex = 11;
            this.tbpFoundationDetails.Text = "Foundation Details";

            //
            // pnlFoundationContainer
            //
            this.pnlFoundationContainer.Controls.Add(this.lblFoundationDetailsDynamicLoadInfo);
            this.pnlFoundationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFoundationContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlFoundationContainer.Name = "pnlFoundationContainer";
            this.pnlFoundationContainer.Padding = new System.Windows.Forms.Padding(2);
            this.pnlFoundationContainer.Size = new System.Drawing.Size(744, 443);
            this.pnlFoundationContainer.TabIndex = 1;

            //
            // lblFoundationDetailsDynamicLoadInfo
            //
            this.lblFoundationDetailsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblFoundationDetailsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblFoundationDetailsDynamicLoadInfo.Location = new System.Drawing.Point(188, 164);
            this.lblFoundationDetailsDynamicLoadInfo.Name = "lblFoundationDetailsDynamicLoadInfo";
            this.lblFoundationDetailsDynamicLoadInfo.Size = new System.Drawing.Size(368, 86);
            this.lblFoundationDetailsDynamicLoadInfo.TabIndex = 0;
            this.lblFoundationDetailsDynamicLoadInfo.Text = resources.GetString("lblFoundationDetailsDynamicLoadInfo.Text");
            this.lblFoundationDetailsDynamicLoadInfo.Visible = false;

            //
            // tbpSubscriptions
            //
            this.tbpSubscriptions.Controls.Add(this.pnlSubscriptions);
            this.tbpSubscriptions.ImageIndex = 2;
            this.tbpSubscriptions.Location = new System.Drawing.Point(4, 23);
            this.tbpSubscriptions.Name = "tbpSubscriptions";
            this.tbpSubscriptions.Size = new System.Drawing.Size(744, 443);
            this.tbpSubscriptions.TabIndex = 10;
            this.tbpSubscriptions.Text = "Subscriptions";

            //
            // pnlSubscriptions
            //
            this.pnlSubscriptions.AutoSize = true;
            this.pnlSubscriptions.Controls.Add(this.lblSubscriptionsDynamicLoadInfo);
            this.pnlSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubscriptions.Location = new System.Drawing.Point(0, 0);
            this.pnlSubscriptions.Name = "pnlSubscriptions";
            this.pnlSubscriptions.Padding = new System.Windows.Forms.Padding(2);
            this.pnlSubscriptions.Size = new System.Drawing.Size(744, 443);
            this.pnlSubscriptions.TabIndex = 0;

            //
            // lblSubscriptionsDynamicLoadInfo
            //
            this.lblSubscriptionsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblSubscriptionsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblSubscriptionsDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblSubscriptionsDynamicLoadInfo.Name = "lblSubscriptionsDynamicLoadInfo";
            this.lblSubscriptionsDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblSubscriptionsDynamicLoadInfo.TabIndex = 0;
            this.lblSubscriptionsDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlSubscriptions) " +
                                                        "in Procedure DynamicLoadUserControl!";
            this.lblSubscriptionsDynamicLoadInfo.Visible = false;

            //
            // tbpPartnerTypes
            //
            this.tbpPartnerTypes.Controls.Add(this.pnlPartnerTypes);
            this.tbpPartnerTypes.ImageIndex = 3;
            this.tbpPartnerTypes.Location = new System.Drawing.Point(4, 23);
            this.tbpPartnerTypes.Name = "tbpPartnerTypes";
            this.tbpPartnerTypes.Size = new System.Drawing.Size(744, 443);
            this.tbpPartnerTypes.TabIndex = 3;
            this.tbpPartnerTypes.Text = "Special Types (?)";

            //
            // pnlPartnerTypes
            //
            this.pnlPartnerTypes.AutoSize = true;
            this.pnlPartnerTypes.Controls.Add(this.lblPartnerTypesDynamicLoadInfo);
            this.pnlPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerTypes.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerTypes.Name = "pnlPartnerTypes";
            this.pnlPartnerTypes.Padding = new System.Windows.Forms.Padding(2);
            this.pnlPartnerTypes.Size = new System.Drawing.Size(744, 443);
            this.pnlPartnerTypes.TabIndex = 0;

            //
            // lblPartnerTypesDynamicLoadInfo
            //
            this.lblPartnerTypesDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblPartnerTypesDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPartnerTypesDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblPartnerTypesDynamicLoadInfo.Name = "lblPartnerTypesDynamicLoadInfo";
            this.lblPartnerTypesDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblPartnerTypesDynamicLoadInfo.TabIndex = 0;
            this.lblPartnerTypesDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlPartnerTypes) i" +
                                                       "n Procedure DynamicLoadUserControl!";
            this.lblPartnerTypesDynamicLoadInfo.Visible = false;

            //
            // tbpContacts
            //
            this.tbpContacts.Controls.Add(this.pnlContacts);
            this.tbpContacts.ImageIndex = 7;
            this.tbpContacts.Location = new System.Drawing.Point(4, 23);
            this.tbpContacts.Name = "tbpContacts";
            this.tbpContacts.Size = new System.Drawing.Size(744, 443);
            this.tbpContacts.TabIndex = 7;
            this.tbpContacts.Text = "Contacts (?)";

            //
            // tbpFamilyMembers
            //
            this.tbpFamilyMembers.Controls.Add(this.pnlFamilyMembers);
            this.tbpFamilyMembers.ImageIndex = 5;
            this.tbpFamilyMembers.Location = new System.Drawing.Point(4, 23);
            this.tbpFamilyMembers.Name = "tbpFamilyMembers";
            this.tbpFamilyMembers.Size = new System.Drawing.Size(744, 443);
            this.tbpFamilyMembers.TabIndex = 5;
            this.tbpFamilyMembers.Text = "Family Members (?)";

            //
            // pnlFamilyMembers
            //
            this.pnlFamilyMembers.AutoSize = true;
            this.pnlFamilyMembers.Controls.Add(this.lblFamilyMembersDynamicLoadInfo);
            this.pnlFamilyMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFamilyMembers.Location = new System.Drawing.Point(0, 0);
            this.pnlFamilyMembers.Name = "pnlFamilyMembers";
            this.pnlFamilyMembers.Padding = new System.Windows.Forms.Padding(2);
            this.pnlFamilyMembers.Size = new System.Drawing.Size(744, 443);
            this.pnlFamilyMembers.TabIndex = 0;

            //
            // lblFamilyMembersDynamicLoadInfo
            //
            this.lblFamilyMembersDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblFamilyMembersDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblFamilyMembersDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblFamilyMembersDynamicLoadInfo.Name = "lblFamilyMembersDynamicLoadInfo";
            this.lblFamilyMembersDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblFamilyMembersDynamicLoadInfo.TabIndex = 0;
            this.lblFamilyMembersDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlFamilyMembers) " +
                                                        "in Procedure DynamicLoadUserControl!";
            this.lblFamilyMembersDynamicLoadInfo.Visible = false;

            //
            // tbpRelationships
            //
            this.tbpRelationships.Controls.Add(this.pnlRelationships);
            this.tbpRelationships.ImageIndex = 6;
            this.tbpRelationships.Location = new System.Drawing.Point(4, 23);
            this.tbpRelationships.Name = "tbpRelationships";
            this.tbpRelationships.Size = new System.Drawing.Size(744, 443);
            this.tbpRelationships.TabIndex = 4;
            this.tbpRelationships.Text = "Relationships (?)";

            //
            // tbpInterests
            //
            this.tbpInterests.Controls.Add(this.pnlInterests);
            this.tbpInterests.ImageIndex = 11;
            this.tbpInterests.Location = new System.Drawing.Point(4, 23);
            this.tbpInterests.Name = "tbpInterests";
            this.tbpInterests.Size = new System.Drawing.Size(744, 443);
            this.tbpInterests.TabIndex = 8;
            this.tbpInterests.Text = "Interests (?)";

            //
            // pnlInterests
            //
            this.pnlInterests.Controls.Add(this.lblInterestsDynamicLoadInfo);
            this.pnlInterests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInterests.Location = new System.Drawing.Point(0, 0);
            this.pnlInterests.Name = "pnlInterests";
            this.pnlInterests.Padding = new System.Windows.Forms.Padding(2);
            this.pnlInterests.Size = new System.Drawing.Size(744, 443);
            this.pnlInterests.TabIndex = 0;

            //
            // lblInterestsDynamicLoadInfo
            //
            this.lblInterestsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblInterestsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblInterestsDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblInterestsDynamicLoadInfo.Name = "lblInterestsDynamicLoadInfo";
            this.lblInterestsDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblInterestsDynamicLoadInfo.TabIndex = 0;
            this.lblInterestsDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlInterests) in P" +
                                                    "rocedure InitialiseUserControl!";
            this.lblInterestsDynamicLoadInfo.Visible = false;

            //
            // tbpReminders
            //
            this.tbpReminders.Controls.Add(this.pnlReminders);
            this.tbpReminders.ImageIndex = 8;
            this.tbpReminders.Location = new System.Drawing.Point(4, 23);
            this.tbpReminders.Name = "tbpReminders";
            this.tbpReminders.Size = new System.Drawing.Size(744, 443);
            this.tbpReminders.TabIndex = 8;
            this.tbpReminders.Text = "Reminders (?)";

            //
            // tbpNotes
            //
            this.tbpNotes.Controls.Add(this.pnlPartnerNotes);
            this.tbpNotes.ImageIndex = 8;
            this.tbpNotes.Location = new System.Drawing.Point(4, 23);
            this.tbpNotes.Name = "tbpNotes";
            this.tbpNotes.Size = new System.Drawing.Size(744, 443);
            this.tbpNotes.TabIndex = 8;
            this.tbpNotes.Text = "Notes";

            //
            // pnlPartnerNotes
            //
            this.pnlPartnerNotes.AutoSize = true;
            this.pnlPartnerNotes.Controls.Add(this.lblPartnerNotesDynamicLoadInfo);
            this.pnlPartnerNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerNotes.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerNotes.Name = "pnlPartnerNotes";
            this.pnlPartnerNotes.Padding = new System.Windows.Forms.Padding(2);
            this.pnlPartnerNotes.Size = new System.Drawing.Size(744, 443);
            this.pnlPartnerNotes.TabIndex = 0;

            //
            // lblPartnerNotesDynamicLoadInfo
            //
            this.lblPartnerNotesDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblPartnerNotesDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPartnerNotesDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblPartnerNotesDynamicLoadInfo.Name = "lblPartnerNotesDynamicLoadInfo";
            this.lblPartnerNotesDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblPartnerNotesDynamicLoadInfo.TabIndex = 0;
            this.lblPartnerNotesDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlPartnerNotes) i" +
                                                       "n Procedure DynamicLoadUserControl!";
            this.lblPartnerNotesDynamicLoadInfo.Visible = false;

            //
            // tbpOfficeSpecific
            //
            this.tbpOfficeSpecific.Controls.Add(this.pnlPartnerOfficeSpecific);
            this.tbpOfficeSpecific.ImageIndex = 9;
            this.tbpOfficeSpecific.Location = new System.Drawing.Point(4, 23);
            this.tbpOfficeSpecific.Name = "tbpOfficeSpecific";
            this.tbpOfficeSpecific.Size = new System.Drawing.Size(744, 443);
            this.tbpOfficeSpecific.TabIndex = 12;
            this.tbpOfficeSpecific.Text = "Local Partner Data";

            //
            // pnlPartnerOfficeSpecific
            //
            this.pnlPartnerOfficeSpecific.AutoSize = true;
            this.pnlPartnerOfficeSpecific.Controls.Add(this.lblPartnerOfficeSpecificDynamicLoadInfo);
            this.pnlPartnerOfficeSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerOfficeSpecific.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerOfficeSpecific.Name = "pnlPartnerOfficeSpecific";
            this.pnlPartnerOfficeSpecific.Padding = new System.Windows.Forms.Padding(2);
            this.pnlPartnerOfficeSpecific.Size = new System.Drawing.Size(744, 443);
            this.pnlPartnerOfficeSpecific.TabIndex = 0;

            //
            // lblPartnerOfficeSpecificDynamicLoadInfo
            //
            this.lblPartnerOfficeSpecificDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Name = "lblPartnerOfficeSpecificDynamicLoadInfo";
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblPartnerOfficeSpecificDynamicLoadInfo.TabIndex = 0;
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlPartnerOfficeSp" +
                                                                "ecific) in Procedure DynamicLoadUserControl!";
            this.lblPartnerOfficeSpecificDynamicLoadInfo.Visible = false;

            //
            // pnlContacts
            //
            this.pnlContacts.AutoSize = true;
            this.pnlContacts.Controls.Add(this.lblContactsDynamicLoadInfo);
            this.pnlContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContacts.Location = new System.Drawing.Point(0, 0);
            this.pnlContacts.Name = "pnlContacts";
            this.pnlContacts.Padding = new System.Windows.Forms.Padding(2);
            this.pnlContacts.Size = new System.Drawing.Size(744, 443);
            this.pnlContacts.TabIndex = 1;

            //
            // lblContactsDynamicLoadInfo
            //
            this.lblContactsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblContactsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblContactsDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblContactsDynamicLoadInfo.Name = "lblContactsDynamicLoadInfo";
            this.lblContactsDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblContactsDynamicLoadInfo.TabIndex = 0;
            this.lblContactsDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlContacts) in Pr" +
                                                   "ocedure DynamicLoadUserControl!";
            this.lblContactsDynamicLoadInfo.Visible = false;

            //
            // pnlRelationships
            //
            this.pnlRelationships.AutoSize = true;
            this.pnlRelationships.Controls.Add(this.lblRelationshipsDynamicLoadInfo);
            this.pnlRelationships.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRelationships.Location = new System.Drawing.Point(0, 0);
            this.pnlRelationships.Name = "pnlRelationships";
            this.pnlRelationships.Padding = new System.Windows.Forms.Padding(2);
            this.pnlRelationships.Size = new System.Drawing.Size(744, 443);
            this.pnlRelationships.TabIndex = 2;

            //
            // lblRelationshipsDynamicLoadInfo
            //
            this.lblRelationshipsDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblRelationshipsDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblRelationshipsDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblRelationshipsDynamicLoadInfo.Name = "lblRelationshipsDynamicLoadInfo";
            this.lblRelationshipsDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblRelationshipsDynamicLoadInfo.TabIndex = 0;
            this.lblRelationshipsDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlRelationships) " +
                                                        "in Procedure DynamicLoadUserControl!";
            this.lblRelationshipsDynamicLoadInfo.Visible = false;

            //
            // pnlReminders
            //
            this.pnlReminders.AutoSize = true;
            this.pnlReminders.Controls.Add(this.lblRemindersDynamicLoadInfo);
            this.pnlReminders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReminders.Location = new System.Drawing.Point(0, 0);
            this.pnlReminders.Name = "pnlReminders";
            this.pnlReminders.Padding = new System.Windows.Forms.Padding(2);
            this.pnlReminders.Size = new System.Drawing.Size(744, 443);
            this.pnlReminders.TabIndex = 3;

            //
            // lblRemindersDynamicLoadInfo
            //
            this.lblRemindersDynamicLoadInfo.BackColor = System.Drawing.Color.White;
            this.lblRemindersDynamicLoadInfo.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblRemindersDynamicLoadInfo.Location = new System.Drawing.Point(188, 176);
            this.lblRemindersDynamicLoadInfo.Name = "lblRemindersDynamicLoadInfo";
            this.lblRemindersDynamicLoadInfo.Size = new System.Drawing.Size(368, 48);
            this.lblRemindersDynamicLoadInfo.TabIndex = 0;
            this.lblRemindersDynamicLoadInfo.Text = "The UserControl is added dynamically at runtime to this Panel (pnlReminders) in P" +
                                                    "rocedure DynamicLoadUserControl!";
            this.lblRemindersDynamicLoadInfo.Visible = false;

            //
            // TUC_PartnerEdit_PartnerTabSet
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tabPartnerTab);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUC_PartnerEdit_PartnerTabSet";
            this.Size = new System.Drawing.Size(752, 472);
            this.tabPartnerTab.ResumeLayout(false);
            this.tbpAddresses.ResumeLayout(false);
            this.tbpAddresses.PerformLayout();
            this.pnlAddresses.ResumeLayout(false);
            this.tbpPartnerDetails.ResumeLayout(false);
            this.pnlPersonDetails.ResumeLayout(false);
            this.tbpFoundationDetails.ResumeLayout(false);
            this.pnlFoundationContainer.ResumeLayout(false);
            this.tbpSubscriptions.ResumeLayout(false);
            this.tbpSubscriptions.PerformLayout();
            this.pnlSubscriptions.ResumeLayout(false);
            this.tbpPartnerTypes.ResumeLayout(false);
            this.tbpPartnerTypes.PerformLayout();
            this.pnlPartnerTypes.ResumeLayout(false);
            this.tbpContacts.ResumeLayout(false);
            this.tbpContacts.PerformLayout();
            this.tbpFamilyMembers.ResumeLayout(false);
            this.tbpFamilyMembers.PerformLayout();
            this.pnlFamilyMembers.ResumeLayout(false);
            this.tbpRelationships.ResumeLayout(false);
            this.tbpRelationships.PerformLayout();
            this.tbpInterests.ResumeLayout(false);
            this.pnlInterests.ResumeLayout(false);
            this.tbpReminders.ResumeLayout(false);
            this.tbpReminders.PerformLayout();
            this.tbpNotes.ResumeLayout(false);
            this.tbpNotes.PerformLayout();
            this.pnlPartnerNotes.ResumeLayout(false);
            this.tbpOfficeSpecific.ResumeLayout(false);
            this.tbpOfficeSpecific.PerformLayout();
            this.pnlPartnerOfficeSpecific.ResumeLayout(false);
            this.pnlContacts.ResumeLayout(false);
            this.pnlRelationships.ResumeLayout(false);
            this.pnlReminders.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerEdit_PartnerTabSet() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpDefault;
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

        private void SetUpPartnerSubscriptions()
        {
            DynamicLoadUserControl(TDynamicLoadableUserControls.dlucSubscriptions);

            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

#if TODO
            FUcoPartnerSubscriptions.MainDS = FMainDS;
            FUcoPartnerSubscriptions.PartnerEditUIConnector = FPartnerEditUIConnector;
            FUcoPartnerSubscriptions.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(this.Uco_HookupPartnerEditDataChange);
            FUcoPartnerSubscriptions.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
            FUcoPartnerSubscriptions.VerificationResultCollection = FVerificationResultCollection;
            FUcoPartnerSubscriptions.InitialiseDelegateGetPartnerShortName(@GetPartnerShortName);
            FUcoPartnerSubscriptions.InitialiseUserControl();
            FUcoPartnerSubscriptions.InitialiseDelegateIsNewPartner(FDelegateIsNewPartner);
            ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerSubscriptions);
            FTabSetupPartnerSubscriptions = true;
#endif

            this.Cursor = Cursors.Default;
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

#if TODO
                /*
                 * The following Dispose calls are necessary to get these UserControls out
                 * of memory if they were created in the Constructor (because of running on
                 * 'Large Fonts (120 DPI)'. Actually, only the Dispose calls on
                 * FUcoInterests and FUcoFoundation are necessary, the others not.
                 * I (Christian K.) don't know why it's those UserControls in particular,
                 * and not all UserControls that are created there, but that's the way it is.
                 * I thought it is a good idea to call Dispose on all of them, just to be on
                 * the safe side for the future.
                 */
                if (FUcoInterests != null)
                {
                    FUcoInterests.Dispose();
                }

                if (FUcoFoundation != null)
                {
                    FUcoFoundation.Dispose();
                }

                if (FUcoPartnerAddresses != null)
                {
                    FUcoPartnerAddresses.Dispose();
                }

                if (FUcoPartnerDetailsPerson != null)
                {
                    FUcoPartnerDetailsPerson.Dispose();
                }

                if (FUcoPartnerDetailsFamily != null)
                {
                    FUcoPartnerDetailsFamily.Dispose();
                }

                if (FUcoPartnerDetailsChurch != null)
                {
                    FUcoPartnerDetailsChurch.Dispose();
                }

                if (FUcoPartnerDetailsOrganisation != null)
                {
                    FUcoPartnerDetailsOrganisation.Dispose();
                }

                if (FUcoPartnerDetailsUnit != null)
                {
                    FUcoPartnerDetailsUnit.Dispose();
                }

                if (FUcoPartnerDetailsBank != null)
                {
                    FUcoPartnerDetailsBank.Dispose();
                }

                if (FUcoPartnerDetailsVenue != null)
                {
                    FUcoPartnerDetailsVenue.Dispose();
                }

                if (FUcoPartnerSubscriptions != null)
                {
                    FUcoPartnerSubscriptions.Dispose();
                }

                if (FUcoPartnerTypes != null)
                {
                    FUcoPartnerTypes.Dispose();
                }

                if (FUcoFamilyMembers != null)
                {
                    FUcoFamilyMembers.Dispose();
                }

                if (FUcoPartnerNotes != null)
                {
                    FUcoPartnerNotes.Dispose();
                }

                if (FUcoPartnerOfficeSpecific != null)
                {
                    FUcoPartnerOfficeSpecific.Dispose();
                }

                if (FUcoContacts != null)
                {
                    FUcoContacts.Dispose();
                }

                if (FUcoRelationships != null)
                {
                    FUcoRelationships.Dispose();
                }

                if (FUcoReminders != null)
                {
                    FUcoReminders.Dispose();
                }
#endif
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisableNewButtonOnAutoCreatedAddress()
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.DisableNewButtonOnAutoCreatedAddress();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASuccess"></param>
        public void DataSavedEventFired(Boolean ASuccess)
        {
#if TODO
            if (FTabSetupOfficeSpecific)
            {
                FUcoPartnerOfficeSpecific.DataSavedEventFired(ASuccess);
            }

            if (FUcoPartnerDetailsOrganisation != null)
            {
                FUcoPartnerDetailsOrganisation.DataSavedEventFired(ASuccess);
            }
#endif
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DataSavingStartedEventFired()
        {
#if TODO
            // Partner Details of an Organisation
            if (FUcoPartnerDetailsOrganisation != null)
            {
                FUcoPartnerDetailsOrganisation.DataSavingStartedEventFired();
            }

            if (FUcoPartnerOfficeSpecific != null)
            {
                FUcoPartnerOfficeSpecific.SaveAllChanges();
            }
#endif
        }

        #region Public Methods

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateIsNewPartner = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            ArrayList TabsToHide;

            OnDataLoadingStarted();

            // Determine which Tabs to show in the ucoPartnerTabSet
            FPartnerClass = FMainDS.PPartner[0].PartnerClass;
            TabsToHide = new ArrayList();

            if (FPartnerClass == "PERSON")
            {
                TabsToHide.Add("tbpFoundationDetails");

                // instead of 'Family Members (?)'
                tbpFamilyMembers.Text = "Family";

                // instead of 'FamilyMembers.ico'
                tbpFamilyMembers.ImageIndex = 4;
            }
            else if (FPartnerClass == "FAMILY")
            {
                // TabsToHide.Add('tbpFamily');
                TabsToHide.Add("tbpFoundationDetails");
            }
            else if (FPartnerClass == "CHURCH")
            {
                TabsToHide.Add("tbpFamilyMembers");
                TabsToHide.Add("tbpFoundationDetails");
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                TabsToHide.Add("tbpFamilyMembers");

                if (!FMainDS.POrganisation[0].Foundation)
                {
                    TabsToHide.Add("tbpFoundationDetails");
                }
                else
                {
                    if (!TSecurity.CheckFoundationSecurity(
                            FMainDS.MiscellaneousData[0].FoundationOwner1Key,
                            FMainDS.MiscellaneousData[0].FoundationOwner2Key))
                    {
                        tbpFoundationDetails.Enabled = false;
                    }

                    if (!CheckSecurityOKToAccessNotesTab())
                    {
                        tbpNotes.Enabled = false;
                    }
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                TabsToHide.Add("tbpFamilyMembers");
                TabsToHide.Add("tbpFoundationDetails");
            }
            else if (FPartnerClass == "BANK")
            {
                TabsToHide.Add("tbpFamilyMembers");
                TabsToHide.Add("tbpFoundationDetails");
            }
            else if (FPartnerClass == "VENUE")
            {
                TabsToHide.Add("tbpFamilyMembers");
                TabsToHide.Add("tbpFoundationDetails");
            }

            if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
            {
                TabsToHide.Add("tbpOfficeSpecific");
            }

            // for the time beeing, we always hide these Tabs that don't do anything yet...
#if  SHOWUNFINISHEDTABS
#else
            TabsToHide.Add("tbpRelationships");
            TabsToHide.Add("tbpContacts");
            TabsToHide.Add("tbpReminders");
            TabsToHide.Add("tbpInterests");
#endif
            ControlsUtilities.HideTabs(tabPartnerTab, TabsToHide);
            FUserControlInitialised = true;

            this.tabPartnerTab.SelectedIndexChanged += new EventHandler(this.TabPartnerTab_SelectedIndexChanged);

            SelectTabPage(FInitiallySelectedTabPage);

            CalculateTabHeaderCounters(this);

            OnDataLoadingFinished();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabPage"></param>
        public void SelectTabPage(TPartnerEditTabPageEnum ATabPage)
        {
            TabPage SelectedTabPageBeforeReSelecting;

            if (!FUserControlInitialised)
            {
                throw new ApplicationException("SelectTabPage must not be called if the UserControl is not yet initialised");
            }

            OnDataLoadingStarted();

            // supress detection changing
            SelectedTabPageBeforeReSelecting = tabPartnerTab.SelectedTab;

            switch (ATabPage)
            {
                case TPartnerEditTabPageEnum.petpAddresses:
                case TPartnerEditTabPageEnum.petpDefault:
                    tabPartnerTab.SelectedTab = tbpAddresses;
                    break;

                case TPartnerEditTabPageEnum.petpDetails:
                    tabPartnerTab.SelectedTab = tbpPartnerDetails;
                    break;

                case TPartnerEditTabPageEnum.petpFoundationDetails:
                    tabPartnerTab.SelectedTab = tbpFoundationDetails;
                    break;

                case TPartnerEditTabPageEnum.petpSubscriptions:
                    tabPartnerTab.SelectedTab = tbpSubscriptions;
                    break;

                case TPartnerEditTabPageEnum.petpPartnerTypes:
                    tabPartnerTab.SelectedTab = tbpPartnerTypes;
                    break;

                case TPartnerEditTabPageEnum.petpFamilyMembers:
                    tabPartnerTab.SelectedTab = tbpFamilyMembers;
                    break;

                case TPartnerEditTabPageEnum.petpOfficeSpecific:
                    tabPartnerTab.SelectedTab = tbpOfficeSpecific;
                    break;

                case TPartnerEditTabPageEnum.petpInterests:
                    tabPartnerTab.SelectedTab = tbpInterests;
                    break;

                case TPartnerEditTabPageEnum.petpReminders:
                    tabPartnerTab.SelectedTab = tbpReminders;
                    break;

                case TPartnerEditTabPageEnum.petpRelationships:
                    tabPartnerTab.SelectedTab = tbpRelationships;
                    break;

                case TPartnerEditTabPageEnum.petpContacts:
                    tabPartnerTab.SelectedTab = tbpContacts;
                    break;

                case TPartnerEditTabPageEnum.petpNotes:

                    if (tbpNotes.Enabled)
                    {
                        tabPartnerTab.SelectedTab = tbpNotes;
                    }
                    else
                    {
                        tabPartnerTab.SelectedTab = tbpAddresses;
                    }

                    break;
            }

            // Check if the selected TabPage actually changed...
            if (SelectedTabPageBeforeReSelecting == tabPartnerTab.SelectedTab)
            {
                // Tab was already selected; therefore raise the SelectedIndexChanged Event 'manually', which did not get raised by selecting the Tab again!
                TabPartnerTab_SelectedIndexChanged(this, new System.EventArgs());
            }

            OnDataLoadingFinished();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CleanupAddressesBeforeMerge()
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.CleanupRecordsBeforeMerge();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFoundAddressLocationRow"></param>
        public void CopyFoundAddressData(PLocationRow AFoundAddressLocationRow)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.CopyFoundAddressData(AFoundAddressLocationRow);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALocationPK"></param>
        /// <returns></returns>
        public Boolean IsAddressRowPresent(TLocationPK ALocationPK)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            return FUcoPartnerAddresses.IsAddressRowPresent(ALocationPK);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshAddressesAfterMerge()
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.RefreshRecordsAfterMerge();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AParameterDT"></param>
        public void SimilarLocationsProcessing(PartnerAddressAggregateTDSSimilarLocationParametersTable AParameterDT)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.ProcessServerResponseSimilarLocations(AParameterDT);
        }

        #endregion

        #region Private Methods
        private void SetUpPartnerAddress()
        {
            DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);

            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoPartnerAddresses.MainDS = FMainDS;
            FUcoPartnerAddresses.PetraUtilsObject = FPetraUtilsObject;
            FUcoPartnerAddresses.PartnerEditUIConnector = FPartnerEditUIConnector;
            FUcoPartnerAddresses.HookupDataChange += new THookupDataChangeEventHandler(this.Uco_HookupDataChange);
            FUcoPartnerAddresses.InitialiseUserControl();

            // MessageBox.Show('TabSetupPartnerAddresses finished');
            FTabSetupPartnerAddresses = true;
            this.Cursor = Cursors.Default;
        }

        private void DynamicLoadUserControl(TDynamicLoadableUserControls AUserControl)
        {
            switch (AUserControl)
            {
                case TDynamicLoadableUserControls.dlucAddresses:
                    FUcoPartnerAddresses = new Ict.Petra.Client.MCommon.TUCPartnerAddresses();
                    FUcoPartnerAddresses.Location = new System.Drawing.Point(0, 2);
                    FUcoPartnerAddresses.Dock = DockStyle.Fill;
                    pnlAddresses.Controls.Add(this.FUcoPartnerAddresses);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make the Custom Disabled Textboxes (actually the labels-instead-of-textboxes) scale
                     * correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlAddresses.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up EnableDisableOtherScreenParts Event that is fired by UserControls on Tabs
                    FUcoPartnerAddresses.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                    this.UcoTab_EnableDisableOtherScreenParts);

                    // Hook up RecalculateScreenParts Event that is fired by several UserControls
                    FUcoPartnerAddresses.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);

                    break;

                case TDynamicLoadableUserControls.dlucSubscriptions:
#if TODO
                    FUcoPartnerSubscriptions = new Ict.Petra.Client.MPartner.TUCPartnerSubscriptions();
                    FUcoPartnerSubscriptions.Location = new System.Drawing.Point(0, 2);
                    FUcoPartnerSubscriptions.Dock = DockStyle.Fill;
                    pnlSubscriptions.Controls.Add(this.FUcoPartnerSubscriptions);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make the Custom Disabled Textboxes (actually the labels-instead-of-textboxes) scale
                     * correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlSubscriptions.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up EnableDisableOtherScreenParts Event that is fired by UserControls on Tabs
                    FUcoPartnerSubscriptions.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                    this.UcoTab_EnableDisableOtherScreenParts);
#endif
                    break;

                case TDynamicLoadableUserControls.dlucPersonDetails:
                    FUcoPartnerDetailsPerson = new TUC_PartnerDetailsPerson();
                    FUcoPartnerDetailsPerson.Location = new Point(0, 2);
                    FUcoPartnerDetailsPerson.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsPerson);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

#if TODO
                case TDynamicLoadableUserControls.dlucFamilyDetails:
                    FUcoPartnerDetailsFamily = new TUC_PartnerDetailsFamily();
                    FUcoPartnerDetailsFamily.Location = new Point(0, 2);
                    FUcoPartnerDetailsFamily.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsFamily);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucChurchDetails:
                    FUcoPartnerDetailsChurch = new TUC_PartnerDetailsChurch();
                    FUcoPartnerDetailsChurch.Location = new Point(0, 2);
                    FUcoPartnerDetailsChurch.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsChurch);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucOrganisationDetails:
                    FUcoPartnerDetailsOrganisation = new TUC_PartnerDetailsOrganisation();
                    FUcoPartnerDetailsOrganisation.Location = new Point(0, 2);
                    FUcoPartnerDetailsOrganisation.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsOrganisation);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up ShowTab Event that is fired by FUcoPartnerDetailsOrganisation
                    FUcoPartnerDetailsOrganisation.ShowTab += new TShowTabEventHandler(this.Uco_ShowTab);

                    break;

                case TDynamicLoadableUserControls.dlucBankDetails:
                    FUcoPartnerDetailsBank = new TUC_PartnerDetailsBank();
                    FUcoPartnerDetailsBank.Location = new Point(0, 2);
                    FUcoPartnerDetailsBank.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsBank);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucUnitDetails:
                    FUcoPartnerDetailsUnit = new TUC_PartnerDetailsUnit();
                    FUcoPartnerDetailsUnit.Location = new Point(0, 2);
                    FUcoPartnerDetailsUnit.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsUnit);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucVenueDetails:
                    FUcoPartnerDetailsVenue = new TUC_PartnerDetailsVenue();
                    FUcoPartnerDetailsVenue.Location = new Point(0, 2);
                    FUcoPartnerDetailsVenue.Dock = DockStyle.Fill;
                    pnlPersonDetails.Controls.Add(FUcoPartnerDetailsVenue);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }
                    break;
#endif

                case TDynamicLoadableUserControls.dlucPartnerTypes:
                    FUcoPartnerTypes = new TUCPartnerTypes();
                    FUcoPartnerTypes.Location = new Point(0, 2);
                    FUcoPartnerTypes.Dock = DockStyle.Fill;
                    pnlPartnerTypes.Controls.Add(FUcoPartnerTypes);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPartnerTypes.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerTypes.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;

                case TDynamicLoadableUserControls.dlucPartnerNotes:
                    FUcoPartnerNotes = new TUC_PartnerNotes();
                    FUcoPartnerNotes.Location = new Point(0, 2);
                    FUcoPartnerNotes.Dock = DockStyle.Fill;
                    pnlPartnerNotes.Controls.Add(FUcoPartnerNotes);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.FUcoPartnerNotes.Dock = System.Windows.Forms.DockStyle.None;
                        this.FUcoPartnerNotes.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerNotes.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;

#if TODO
                case TDynamicLoadableUserControls.dlucFoundations:
                    FUcoFoundation = new TUC_PartnerFoundation();
                    FUcoFoundation.Location = new Point(0, 2);
                    FUcoFoundation.Dock = DockStyle.Fill;
                    pnlFoundationContainer.Controls.Add(FUcoFoundation);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlFoundationContainer.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlFoundationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    // Hook up EnableDisableOtherScreenParts Event that is fired by UserControls on Tabs
                    FUcoFoundation.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                    this.UcoTab_EnableDisableOtherScreenParts);
                    break;

                case TDynamicLoadableUserControls.dlucFamilyMembers:
                    FUcoFamilyMembers = new TUC_FamilyMembers();
                    FUcoFamilyMembers.Location = new Point(0, 2);
                    FUcoFamilyMembers.Dock = DockStyle.Fill;
                    pnlFamilyMembers.Controls.Add(FUcoFamilyMembers);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlFamilyMembers.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlFamilyMembers.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    FUcoFamilyMembers.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    FUcoFamilyMembers.GetLocationRowOfCurrentlySelectedAddress += new TDelegateGetLocationRowOfCurrentlySelectedAddress(
                    Get_LocationRowOfCurrentlySelectedAddress);
                    break;

                case TDynamicLoadableUserControls.dlucPartnerOfficeSpecific:
                    FUcoPartnerOfficeSpecific = new TUC_PartnerOfficeSpecific();
                    FUcoPartnerOfficeSpecific.Location = new Point(0, 2);
                    FUcoPartnerOfficeSpecific.Dock = DockStyle.Fill;
                    pnlPartnerOfficeSpecific.Controls.Add(FUcoPartnerOfficeSpecific);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.FUcoPartnerOfficeSpecific.Dock = System.Windows.Forms.DockStyle.None;
                        this.FUcoPartnerOfficeSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucInterests:
                    FUcoInterests = new TUCPartnerInterests();
                    FUcoInterests.Location = new Point(0, 2);
                    FUcoInterests.Dock = DockStyle.Fill;
                    pnlInterests.Controls.Add(FUcoInterests);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlPersonDetails.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    FUcoInterests.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;

                case TDynamicLoadableUserControls.dlucContacts:
                    FUcoContacts = new TUCPartnerContacts();
                    FUcoContacts.Location = new Point(0, 2);
                    FUcoContacts.Dock = DockStyle.Fill;
                    pnlContacts.Controls.Add(FUcoContacts);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlContacts.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlContacts.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    FUcoContacts.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;

                case TDynamicLoadableUserControls.dlucRelationships:
                    FUcoRelationships = new TUCPartnerRelationships();
                    FUcoRelationships.Location = new Point(0, 2);
                    FUcoRelationships.Dock = DockStyle.Fill;
                    pnlRelationships.Controls.Add(FUcoRelationships);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlRelationships.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlRelationships.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    FUcoRelationships.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;

                case TDynamicLoadableUserControls.dlucReminders:
                    FUcoReminders = new TUCPartnerReminders();
                    FUcoReminders.Location = new Point(0, 2);
                    FUcoReminders.Dock = DockStyle.Fill;
                    pnlReminders.Controls.Add(FUcoReminders);

                    /*
                     *  The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.pnlReminders.Dock = System.Windows.Forms.DockStyle.None;
                        this.pnlReminders.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    FUcoReminders.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(this.RecalculateTabHeaderCounters);
                    break;
#endif
            }
        }

        private Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet.GetPartnerShortName got called');
            return FPartnerEditUIConnector.GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="selectedTab"></param>
        /// <returns></returns>
        public DataRow GetSelectedDataRow(Int32 selectedTab)
        {
            DataRow ReturnValue = null;

            switch (selectedTab)
            {
                case 0:

// TODO                    ReturnValue = FUcoPartnerSubscriptions.GetSelectedDataRow();
                    break;

                case 1:

                    //MessageBox.Show("TUC_PartnerEdit_PartnerTabSet.GetSelectedDataRow 1");
                    break;
            }

            return ReturnValue;
        }

        private void RecalculateTabHeaderCounters(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet.RecalculateTabHeaderCounters');
            if (e.ScreenPart == TScreenPartEnum.spCounters)
            {
                CalculateTabHeaderCounters(sender);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        public void AddNewFoundAddress(Int64 ASiteKey, Int32 ALocationKey)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.AddNewFoundAddress(ASiteKey, ALocationKey);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AAddedOrChangedPromotionDT"></param>
        /// <param name="AParameterDT"></param>
        public void AddressAddedOrChangedProcessing(PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddedOrChangedPromotionDT,
            PartnerAddressAggregateTDSChangePromotionParametersTable AParameterDT)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.ProcessServerResponseAddressAddedOrChanged(AAddedOrChangedPromotionDT, AParameterDT);
        }

        private void CalculateTabHeaderCounters(System.Object ASender)
        {
            DataView TmpDV;
            string DynamicTabTitle;
            string DynamicToolTipPart1;
            Int32 CountAll;
            Int32 CountActive;

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerAddresses))
            {
                if (FMainDS.Tables.Contains(PLocationTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsAddresses(FMainDS.PPartnerLocation, out CountAll, out CountActive);
                }
                else
                {
                    CountAll = FMainDS.MiscellaneousData[0].ItemsCountAddresses;
                    CountActive = FMainDS.MiscellaneousData[0].ItemsCountAddressesActive;
                }

                if ((CountAll == 0) || (CountAll > 1))
                {
                    DynamicToolTipPart1 = StrAddressesTabHeader;
                }
                else
                {
                    DynamicToolTipPart1 = StrAddressesSingular;
                }

                if (CountActive == 0)
                {
                    tbpAddresses.Text = String.Format(StrAddressesTabHeader + " ({0}!)", CountActive);
                    tbpAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "current", CountAll, CountActive, DynamicToolTipPart1);
                }
                else
                {
                    tbpAddresses.Text = String.Format(StrAddressesTabHeader + " ({0})", CountActive);

                    if (CountActive > 1)
                    {
                        tbpAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "current", CountAll, CountActive, DynamicToolTipPart1);
                    }
                    else
                    {
                        tbpAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipSingular + "current",
                            CountAll,
                            CountActive,
                            DynamicToolTipPart1);
                    }
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerSubscriptions))
            {
                if (FMainDS.Tables.Contains(PSubscriptionTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsSubscriptions(FMainDS.PSubscription, out CountAll, out CountActive);
                    tbpSubscriptions.Text = String.Format(StrSubscriptionsTabHeader + " ({0})", CountActive);
                }
                else
                {
                    CountAll = FMainDS.MiscellaneousData[0].ItemsCountSubscriptions;
                    CountActive = FMainDS.MiscellaneousData[0].ItemsCountSubscriptionsActive;
                }

                if ((CountAll == 0) || (CountAll > 1))
                {
                    DynamicToolTipPart1 = StrSubscriptionsTabHeader;
                }
                else
                {
                    DynamicToolTipPart1 = StrSubscriptionsSingular;
                }

                tbpSubscriptions.Text = String.Format(StrSubscriptionsTabHeader + " ({0})", CountActive);

                if ((CountActive == 0) || (CountActive > 1))
                {
                    tbpSubscriptions.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "active", CountAll, CountActive, DynamicToolTipPart1);
                }
                else
                {
                    tbpSubscriptions.ToolTipText = String.Format(StrTabHeaderCounterTipSingular + "active",
                        CountAll,
                        CountActive,
                        DynamicToolTipPart1);
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerTypes))
            {
                if (FMainDS.Tables.Contains(PPartnerTypeTable.GetTableName()))
                {
                    TmpDV = new DataView(FMainDS.PPartnerType, "", "", DataViewRowState.CurrentRows);
                    tbpPartnerTypes.Text = StrSpecialTypesTabHeader + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tbpPartnerTypes.Text = StrSpecialTypesTabHeader + " (" + FMainDS.MiscellaneousData[0].ItemsCountPartnerTypes.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_FamilyMembers))
            {
                // determine Tab Title
                if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
                {
                    DynamicTabTitle = StrFamilyMembersTabHeader;
                }
                else
                {
                    DynamicTabTitle = StrFamilyTabHeader;
                }

                if (FMainDS.Tables.Contains(PartnerEditTDSFamilyMembersTable.GetTableName()))
                {
                    TmpDV = new DataView(FMainDS.FamilyMembers, "", "", DataViewRowState.CurrentRows);
                    tbpFamilyMembers.Text = DynamicTabTitle + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tbpFamilyMembers.Text = DynamicTabTitle + " (" + FMainDS.MiscellaneousData[0].ItemsCountFamilyMembers.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerInterests))
            {
                if (FMainDS.Tables.Contains(PPartnerInterestTable.GetTableName()))
                {
                    TmpDV = new DataView(FMainDS.PPartnerInterest, "", "", DataViewRowState.CurrentRows);
                    tbpInterests.Text = StrInterestsTabHeader + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tbpInterests.Text = StrInterestsTabHeader + " (" + FMainDS.MiscellaneousData[0].ItemsCountInterests.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_PartnerNotes))
            {
                if ((FMainDS.PPartner[0].IsCommentNull()) || (FMainDS.PPartner[0].Comment == ""))
                {
                    tbpNotes.Text = String.Format(StrNotesTabHeader + " ({0})", 0);
                    tbpNotes.ToolTipText = "No Notes entered";
                }
                else
                {
                    // 8730 = 'square root symbol' in Verdana
                    tbpNotes.Text = String.Format(StrNotesTabHeader + " ({0})", (char)8730);
                    tbpNotes.ToolTipText = "Notes are entered";
                }
            }
        }

        private Boolean CheckSecurityOKToAccessNotesTab()
        {
            Boolean ReturnValue;

            ReturnValue = false;

            if ((FMainDS.MiscellaneousData[0].FoundationOwner1Key == 0) && (FMainDS.MiscellaneousData[0].FoundationOwner2Key == 0))
            {
                // MessageBox.Show('Notes Tab: None of the Owners is set.');
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))
                {
                    // MessageBox.Show('Notes Tab: User is member of DEVADMIN Module');
                    ReturnValue = true;
                }
            }
            else
            {
                // MessageBox.Show('Notes Tab: One of the Owners is set!');
                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))
                {
                    // MessageBox.Show('Notes Tab: User is member of DEVADMIN Module');
                    ReturnValue = true;
                }
                else
                {
                    // MessageBox.Show('Notes Tab: User is NOT member of DEVADMIN Module');
                    if ((UserInfo.GUserInfo.PetraIdentity.PartnerKey == FMainDS.MiscellaneousData[0].FoundationOwner1Key)
                        || (UserInfo.GUserInfo.PetraIdentity.PartnerKey == FMainDS.MiscellaneousData[0].FoundationOwner2Key))
                    {
                        // MessageBox.Show('Notes Tab: User is Owner1 or Owner2');
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        #endregion

        #region Custom Events
        private void OnDataLoadingFinished()
        {
            if (DataLoadingFinished != null)
            {
                DataLoadingFinished(this, new EventArgs());
            }
        }

        private void OnDataLoadingStarted()
        {
            if (DataLoadingStarted != null)
            {
                DataLoadingStarted(this, new EventArgs());
            }
        }

        private void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        private void OnShowTab(TShowTabEventArgs e)
        {
            if (ShowTab != null)
            {
                ShowTab(this, e);
            }
        }

#if TODO
        public Boolean PerformCancelAllSubscriptions(DateTime ACancelDate)
        {
            if (!FTabSetupPartnerSubscriptions)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucSubscriptions);'
                SetUpPartnerSubscriptions();
            }

            return FUcoPartnerSubscriptions.PerformCancelAllSubscriptions(ACancelDate);

            // MessageBox.Show('FUcoPartnerSubscriptions.PerformCancelAllSubscriptions Result: ' + Result.ToString);
        }
#endif



        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACancelDate"></param>
        public void ExpireAllCurrentAddresses(DateTime ACancelDate)
        {
            if (!FTabSetupPartnerAddresses)
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetUpPartnerAddress();
            }

            FUcoPartnerAddresses.ExpireAllCurrentAddresses(ACancelDate);
        }

        #endregion

        #region Event Handlers
        private void TabPartnerTab_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            // raise the event to warn the base form that
            // we might be loading some fresh data
            // We need to bypass the ChangeDetection routine
            // while this happens
            OnDataLoadingStarted();

            if (tabPartnerTab.SelectedTab == tbpAddresses)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;

                if (!FTabSetupPartnerAddresses)
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    SetUpPartnerAddress();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerAddresses);
                }
                else
                {
                    /*
                     *  The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerAddresses.AdjustLabelControlsAfterResizing();
                    }
                }

                // Checks whether there any Tips to show to the User; if there are, they will be
                // shown.
                FUcoPartnerAddresses.CheckForUserTips();
            }
            else if (tabPartnerTab.SelectedTab == tbpPartnerDetails)
            {
                if (!FTabSetupPartnerDetails)
                {
                    this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpDetails;

                    // No need for this.Cursor := Cursors.AppStarting here since this data is always retrieved...
                    if (FPartnerClass == "PERSON")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPersonDetails);

                        FUcoPartnerDetailsPerson.MainDS = FMainDS;
                        FUcoPartnerDetailsPerson.PetraUtilsObject = FPetraUtilsObject;
                        FUcoPartnerDetailsPerson.InitialiseUserControl();
                        ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerDetailsPerson);
                    }

#if TODO
                    else if (FPartnerClass == "FAMILY")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucFamilyDetails);

                        FUcoPartnerDetailsFamily.MainDS = FMainDS;
                        FUcoPartnerDetailsFamily.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsFamily);
                    }
                    else if (FPartnerClass == "CHURCH")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucChurchDetails);

                        FUcoPartnerDetailsChurch.MainDS = FMainDS;
                        FUcoPartnerDetailsChurch.VerificationResultCollection = FVerificationResultCollection;
                        FUcoPartnerDetailsChurch.InitialiseDelegateGetPartnerShortName(@GetPartnerShortName);
                        FUcoPartnerDetailsChurch.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsChurch);
                    }
                    else if (FPartnerClass == "ORGANISATION")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucOrganisationDetails);

                        FUcoPartnerDetailsOrganisation.MainDS = FMainDS;
                        FUcoPartnerDetailsOrganisation.VerificationResultCollection = FVerificationResultCollection;
                        FUcoPartnerDetailsOrganisation.PartnerEditUIConnector = FPartnerEditUIConnector;
                        FUcoPartnerDetailsOrganisation.InitialiseDelegateGetPartnerShortName(@GetPartnerShortName);
                        FUcoPartnerDetailsOrganisation.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsOrganisation);
                    }
                    else if (FPartnerClass == "UNIT")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucUnitDetails);

                        FUcoPartnerDetailsUnit.MainDS = FMainDS;
                        FUcoPartnerDetailsUnit.VerificationResultCollection = FVerificationResultCollection;
                        FUcoPartnerDetailsUnit.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsUnit);
                    }
                    else if (FPartnerClass == "VENUE")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucVenueDetails);

                        FUcoPartnerDetailsVenue.MainDS = FMainDS;
                        FUcoPartnerDetailsVenue.VerificationResultCollection = FVerificationResultCollection;
                        FUcoPartnerDetailsVenue.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsVenue);
                    }
                    else if (FPartnerClass == "BANK")
                    {
                        DynamicLoadUserControl(TDynamicLoadableUserControls.dlucBankDetails);

                        FUcoPartnerDetailsBank.MainDS = FMainDS;
                        FUcoPartnerDetailsBank.VerificationResultCollection = FVerificationResultCollection;
                        FUcoPartnerDetailsBank.InitialiseDelegateGetPartnerShortName(@GetPartnerShortName);
                        FUcoPartnerDetailsBank.InitialiseUserControl();
                        ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerDetailsBank);
                    }
#endif

                    // MessageBox.Show('TabSetupPartnerDetails finished');
                    FTabSetupPartnerDetails = true;
                }
            }

#if TODO
            else if (tabPartnerTab.SelectedTab == tbpFoundationDetails)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFoundationDetails;

                if (!FTabSetupFoundation)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucFoundations);

                    FUcoFoundation.MainDS = FMainDS;
                    FUcoFoundation.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoFoundation.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(this.Uco_HookupPartnerEditDataChange);
                    FUcoFoundation.InitialiseUserControl();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoFoundation);

                    // MessageBox.Show('TabSetupFoundation finished');
                    FTabSetupFoundation = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpSubscriptions)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpSubscriptions;

                if (!FTabSetupPartnerSubscriptions)
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucSubscriptions);'
                    SetUpPartnerSubscriptions();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerSubscriptions);
                }
                else
                {
                    /*
                     *  The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPartnerSubscriptions.AdjustLabelControlsAfterResizing();
                    }
                }

                // Checks whether there any Tips to show to the User; if there are, they will be
                // shown.
                FUcoPartnerSubscriptions.CheckForUserTips();
            }
#endif
            else if (tabPartnerTab.SelectedTab == tbpPartnerTypes)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerTypes;

                if (!FTabSetupPartnerTypes)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerTypes);

                    FUcoPartnerTypes.MainDS = FMainDS;
                    FUcoPartnerTypes.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPartnerTypes.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(this.Uco_HookupPartnerEditDataChange);
                    FUcoPartnerTypes.InitialiseUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerTypes);

                    // MessageBox.Show('TabSetupPartnerTypes finished');
                    FTabSetupPartnerTypes = true;
                    this.Cursor = Cursors.Default;
                }
            }
#if TODO
            else if (tabPartnerTab.SelectedTab == tbpFamilyMembers)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFamilyMembers;

                if (!FTabSetupFamilyMembers)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucFamilyMembers);

                    FUcoFamilyMembers.MainDS = FMainDS;
                    FUcoFamilyMembers.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoFamilyMembers.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(this.Uco_HookupPartnerEditDataChange);
                    FUcoFamilyMembers.InitialiseUserControl();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoFamilyMembers);
                    FUcoFamilyMembers.InitialiseDelegateIsNewPartner(FDelegateIsNewPartner);

                    //                  MessageBox.Show("TabSetupFamilyMembers finished");
                    FTabSetupFamilyMembers = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpInterests)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpInterests;

                if (!FTabSetupInterests)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucInterests);

                    FUcoInterests.MainDS = FMainDS;
                    FUcoInterests.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoInterests.InitialiseUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoInterests);

                    // MessageBox.Show('TabSetupInterests finished');
                    FTabSetupInterests = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpContacts)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpContacts;

                if (!FTabSetupContacts)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucContacts);

                    FUcoContacts.MainDS = FMainDS;
                    FUcoContacts.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoContacts.InitialiseUserControl();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoContacts);

                    // MessageBox.Show('TabSetupContacts finished');
                    FTabSetupContacts = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpRelationships)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpRelationships;

                if (!FTabSetupRelationships)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucRelationships);

                    FUcoRelationships.MainDS = FMainDS;
                    FUcoRelationships.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoRelationships.InitialiseUserControl();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoRelationships);

                    // MessageBox.Show('TabSetupRelationships finished');
                    FTabSetupRelationships = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpReminders)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpReminders;

                if (!FTabSetupReminders)
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucReminders);

                    FUcoReminders.MainDS = FMainDS;
                    FUcoReminders.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoReminders.InitialiseUserControl();
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoReminders);

                    // MessageBox.Show('TabSetupReminders finished');
                    FTabSetupReminders = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else if (tabPartnerTab.SelectedTab == tbpOfficeSpecific)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpOfficeSpecific;

                if (!FTabSetupOfficeSpecific)
                {
                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerOfficeSpecific);

                    // No need for this.Cursor := Cursors.AppStarting here since this data is always retrieved...
                    FUcoPartnerOfficeSpecific.MainDS = FMainDS;
                    FUcoPartnerOfficeSpecific.InitialiseDelegateGetPartnerShortName(@GetPartnerShortName);
                    FUcoPartnerOfficeSpecific.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(this.Uco_HookupPartnerEditDataChange);
                    FUcoPartnerOfficeSpecific.InitialiseUserControl();

                    // sbtUserControl := FUcoPartnerTypes.StatusBarTextProvider;   needed in the case where the TabPage is selected during PartnerEdit form_load procedureMessageBox.Show('TabSetupPartnerTypes finished');
                    Uco_HookupPartnerEditDataChange(this, new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpOfficeSpecific));
                    ((IFrmPetra)(this.ParentForm)).HookupAllInContainer(FUcoPartnerOfficeSpecific);

                    // MessageBox.Show('TabSetupOfficeSpecific finished');
                    FTabSetupOfficeSpecific = true;
                }
            }
#endif
            else if (tabPartnerTab.SelectedTab == tbpNotes)
            {
                this.FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpNotes;

                if (!FTabSetupPartnerNotes)
                {
                    DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPartnerNotes);

                    // No need for this.Cursor := Cursors.AppStarting here since this data is always retrieved...
                    FUcoPartnerNotes.MainDS = FMainDS;
                    FUcoPartnerNotes.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPartnerNotes.InitialiseUserControl();

                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPartnerNotes);

                    // MessageBox.Show('TabSetupPartnerNotes finished');
                    FTabSetupPartnerNotes = true;
                }

                // Checks whether there any Tips to show to the User; if there are, they will be
                // shown.
                FUcoPartnerNotes.CheckForUserTips();
            }

            // raise the event to warn the base form that
            // we have finished loading fresh data
            // We need to turn the ChangeDetection routine
            // back on
            OnDataLoadingFinished();
        }

        private void UcoTab_EnableDisableOtherScreenParts(System.Object sender, TEnableDisableEventArgs e)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet.ucoTab_EnableDisableOtherScreenParts(' + e.Enable.ToString + ')');
            // Simply fire OnEnableDisableOtherScreenParts event again so that the PartnerEdit screen can catch it
            OnEnableDisableOtherScreenParts(e);

            // Disable all TabPages except the current one (and reverse that)
            tabPartnerTab.EnableDisableAllOtherTabPages(e.Enable);
        }

        private void Uco_HookupDataChange(System.Object sender, System.EventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void Uco_HookupPartnerEditDataChange(System.Object sender, THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupPartnerEditDataChange != null)
            {
                HookupPartnerEditDataChange(this, e);
            }
        }

        private void Uco_ShowTab(System.Object sender, TShowTabEventArgs e)
        {
            ArrayList TabsToModify;
            TabPage TabPageToAdd;

            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet.uco_ShowTab handled. TabName: ' + e.TabName + '; Show: ' + e.Show.ToString + '; ShowNextToTabName: ' + e.ShowNextToTabName);
            TabPageToAdd = null;

            if (e.Show)
            {
                if (e.TabName == tbpFoundationDetails.Name)
                {
                    TabPageToAdd = tbpFoundationDetails;
                }

                if (TabPageToAdd != null)
                {
                    ControlsUtilities.AddTabNextToTab(tabPartnerTab, TabPageToAdd, e.ShowNextToTabName);
                }
            }
            else
            {
                TabsToModify = new ArrayList();
                TabsToModify.Add(e.TabName);
                ControlsUtilities.HideTabs(tabPartnerTab, TabsToModify);
            }

            // raise Event here so that PartnerEdit Form can catch it
            OnShowTab(new TShowTabEventArgs(e.TabName, e.Show, e.ShowNextToTabName));
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        /// <summary>todoComment</summary>
        dlucAddresses,

        /// <summary>todoComment</summary>
        dlucPersonDetails,

        /// <summary>todoComment</summary>
        dlucFamilyDetails,

        /// <summary>todoComment</summary>
        dlucChurchDetails,

        /// <summary>todoComment</summary>
        dlucOrganisationDetails,

        /// <summary>todoComment</summary>
        dlucBankDetails,

        /// <summary>todoComment</summary>
        dlucUnitDetails,

        /// <summary>todoComment</summary>
        dlucVenueDetails,

        /// <summary>todoComment</summary>
        dlucFoundations,

        /// <summary>todoComment</summary>
        dlucSubscriptions,

        /// <summary>todoComment</summary>
        dlucPartnerTypes,

        /// <summary>todoComment</summary>
        dlucPartnerNotes,

        /// <summary>todoComment</summary>
        dlucFamilyMembers,

        /// <summary>todoComment</summary>
        dlucPartnerOfficeSpecific,

        /// <summary>todoComment</summary>
        dlucInterests,

        /// <summary>todoComment</summary>
        dlucContacts,

        /// <summary>todoComment</summary>
        dlucRelationships,

        /// <summary>todoComment</summary>
        dlucReminders
    };

    /// <summary>Delegate declaration</summary>
    public delegate PLocationRow TDelegateGetLocationRowOfCurrentlySelectedAddress();

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate bool TDelegateIsNewPartner(PartnerEditTDS AInspectDataSet);

    /// just temporary until TUCPartnerSubscriptions are included properly
    public class TUCPartnerSubscriptions
    {
    }

    /// <summary>
    /// temporary class until PartnerInterests are implemented properly
    /// </summary>
    public class TUCPartnerInterests
    {
    }

    /// <summary>
    /// temporary class until FamilyMembers are implemented properly
    /// </summary>
    public class TUC_FamilyMembers
    {
    }
}