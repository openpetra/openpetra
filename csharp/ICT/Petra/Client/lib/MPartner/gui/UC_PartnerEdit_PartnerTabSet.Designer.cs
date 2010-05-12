//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerEdit_PartnerTabSet
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

                // need to do this because of custom created tab pages
                SpecialDispose(disposing);
            }
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
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
    }
}