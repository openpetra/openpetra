/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, petrih
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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Core;
using Ict.Common.Verification;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using System.Globalization;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Common.Controls;
using Ict.Petra.Client.MPartner.Verification;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl that holds the same fields as the Subscription part in the
    /// old Progress Partner Edit screen.
    ///
    /// Since this UserControl can be re-used, it is no longer necessary to recreate
    /// all the layout and fields and verification rules in other places than the
    /// Partner Edit screen, eg. in the Personnel Shepherds!
    ///
    /// @Comment Have a look at UC_PartnerSubscriptions to see how DataBinding works
    /// with this UserControl.
    /// //             TPetraUserControl  System.windows.Forms.UserControl
    /// </summary>
    public class TUCPartnerSubscription : Ict.Petra.Client.CommonForms.TPetraUserControl
    {
        public const String StrSecurityViolationExplanation = "Security Violation";
        public const String StrSecurityViolationExplanationTitle = "Security Violation - Explanation";
        public const String StrDeleteQuestionLine1 = "Are you sure you want to remove this Subscription";
        public const String StrDeleteQuestionShared = "from this partner?";
        public const String StrDeleteQuestionNotShared = "from the database?";
        public const String StrDeleteQuestionTitle = "Delete Subscription?";
        public const String StrIssuesMaintained = "Issues data is usually automatically maintained by Petra." +
                                                  "\r\nAre you sure you want to manually change it?";
        public const String StrIssuesMaintaindedTitle = "Edit Issues";
        public const String StrPartnerHasThePublication = "This Partner already has the selected subscription";
        public const String StrReasonGivenDonation = "DONATION";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlSubscriptionsRight;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.TextBox txtComplimentary;
        private System.Windows.Forms.TextBox txtCopies;
        private System.Windows.Forms.TextBox txtPublicationCost;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.GroupBox grpSubscription;
        private TCmbAutoPopulated cmbPublicationCode;
        private System.Windows.Forms.Label Label3;
        private TCmbAutoPopulated cmbSubscriptionStatus;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.CheckBox chkFreeSubscription;
        private System.Windows.Forms.GroupBox grpDates;
        private System.Windows.Forms.Label Label21;
        private System.Windows.Forms.Label Label9;
        private System.Windows.Forms.Label Label10;
        private System.Windows.Forms.Label Label23;
        private System.Windows.Forms.Label Label19;
        private System.Windows.Forms.GroupBox grpIssues;
        private System.Windows.Forms.Label Label22;
        private System.Windows.Forms.TextBox txtIssuesReceived;
        private System.Windows.Forms.Label Label24;
        private System.Windows.Forms.Label Label25;
        private System.Windows.Forms.ToolTip tipInfo;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.TextBox txtExpiryDate;
        private System.Windows.Forms.TextBox txtDateRenewed;
        private System.Windows.Forms.TextBox txtNoticeSent;
        private System.Windows.Forms.TextBox txtDateEnded;
        private System.Windows.Forms.TextBox txtFirstIssueSent;
        private System.Windows.Forms.TextBox txtLastIssueSent;
        private TbtnCreated btnCreatedSubscriptions;

        /// <summary>stbUCPartnerSubscription: EWSoftware.StatusBarText.StatusBarTextProvider;</summary>
        private TCmbAutoPopulated cmbReasonGiven;
        private TCmbAutoPopulated cmbReasonEnded;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox txtPublicationCostCurrency;
        private TexpTextBoxStringLengthCheck expStringLengthCheckSubscription;
        private TtxtAutoPopulatedButtonLabel txtContactPartnerBox;
        private System.Windows.Forms.Button btnEditIssues;
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>Holds a reference to the DataSet that contains most data that is used on the screen</summary>
        private new PartnerEditTDS FMainDS;

        /// <summary>Currently selected Publicationcode</summary>
        private String FPublicationCode;
        private String FExtractOrPartneKey;

        /// <summary>DataView for the p_subscription record we are working with</summary>
        private DataView FSubscriptionDV;

        /// <summary>Reference to PubclicationCostTable.</summary>
        private PPublicationCostTable FPublicationCostDT;

        /// <summary>CachedDataset.TmpCacheDS: DataSet; Currently selected PublicationCode. Won't update automatically!</summary>
        private System.Object FSelectedPubclicationCode;

        /// <summary>temporary Date Cancelled date to keep the date when user scrolls between different Subscription statuses.</summary>
        private DateTime FtmpDateCancelledDate;

        /// <summary>Is the partnerkey form 'Partner Gift given' valid or not.</summary>
        private Boolean FtmpPartnerKeyValid;

        //        private DataColumn FDataColumnThatCausedVerificationError;
        /// <summary>private DataColumn FDataColumnComparedTo;</summary>
        private ArrayList FDataColumnCollection;
        private CustomEnablingDisabling.TDelegateDisabledControlClick FDelegateDisabledControlClick;

        /// <summary>DataSet that contains most data that is used on the screen</summary>
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

        /// <summary>Publicationkey value we are working with.</summary>
        public String PublicationCode
        {
            get
            {
                return FPublicationCode;
            }

            set
            {
                /// Sets the Publication code
                System.Object GiftFrom;
                System.Object ReasonEnded;
                FPublicationCode = value;

                if (FSubscriptionDV != null)
                {
                    // filter DataView to show only a certain record
                    FSubscriptionDV.RowFilter = PSubscriptionTable.GetPublicationCodeDBName() + " = '" + FPublicationCode + "'";

                    // Gift From and Reason Ended give trouble
                    // so are dont manually
                    GiftFrom = FSubscriptionDV[0][PSubscriptionTable.GetGiftFromKeyDBName()];
                    ReasonEnded = FSubscriptionDV[0][PSubscriptionTable.GetReasonSubsCancelledCodeDBName()];

                    if (ReasonEnded == null)
                    {
                        // don't think this ever happens, but just in case
                        ReasonEnded = "";
                    }

                    if (GiftFrom == null)
                    {
                        txtContactPartnerBox.Text = "0";
                    }
                    else
                    {
                        txtContactPartnerBox.Text = GiftFrom.ToString();
                    }

                    // update Publication Cost and Currency
                    CmbPublicationCode_SelectedValueChanged(new System.Object(), new System.EventArgs());

                    // update Reason Ended
                    cmbReasonEnded.SelectedValue = ReasonEnded.ToString();
                }
            }
        }

        public String ExctractOrPartnerKey
        {
            get
            {
                return FExtractOrPartneKey;
            }

            set
            {
                /// To se the value
                FExtractOrPartneKey = value;
            }
        }

        public CustomEnablingDisabling.TDelegateDisabledControlClick DisabledControlClickHandler
        {
            set
            {
                FDelegateDisabledControlClick = value;
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
                new System.ComponentModel.ComponentResourceManager(typeof(TUCPartnerSubscription));
            this.components = new System.ComponentModel.Container();
            this.pnlSubscriptionsRight = new System.Windows.Forms.Panel();
            this.btnCreatedSubscriptions = new Ict.Common.Controls.TbtnCreated();
            this.grpSubscription = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmbPublicationCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbSubscriptionStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.chkFreeSubscription = new System.Windows.Forms.CheckBox();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.txtPublicationCost = new System.Windows.Forms.TextBox();
            this.txtPublicationCostCurrency = new System.Windows.Forms.TextBox();
            this.cmbReasonEnded = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbReasonGiven = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtComplimentary = new System.Windows.Forms.TextBox();
            this.txtCopies = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtContactPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.grpDates = new System.Windows.Forms.GroupBox();
            this.txtDateEnded = new System.Windows.Forms.TextBox();
            this.txtNoticeSent = new System.Windows.Forms.TextBox();
            this.txtDateRenewed = new System.Windows.Forms.TextBox();
            this.txtExpiryDate = new System.Windows.Forms.TextBox();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label23 = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.grpIssues = new System.Windows.Forms.GroupBox();
            this.btnEditIssues = new System.Windows.Forms.Button();
            this.txtLastIssueSent = new System.Windows.Forms.TextBox();
            this.txtFirstIssueSent = new System.Windows.Forms.TextBox();
            this.Label22 = new System.Windows.Forms.Label();
            this.txtIssuesReceived = new System.Windows.Forms.TextBox();
            this.Label24 = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.expStringLengthCheckSubscription = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlSubscriptionsRight.SuspendLayout();
            this.grpSubscription.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.grpDates.SuspendLayout();
            this.grpIssues.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlSubscriptionsRight
            //
            this.pnlSubscriptionsRight.AutoScroll = true;
            this.pnlSubscriptionsRight.AutoScrollMinSize = new System.Drawing.Size(670, 240);
            this.pnlSubscriptionsRight.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSubscriptionsRight.Controls.Add(this.btnCreatedSubscriptions);
            this.pnlSubscriptionsRight.Controls.Add(this.grpSubscription);
            this.pnlSubscriptionsRight.Controls.Add(this.grpMisc);
            this.pnlSubscriptionsRight.Controls.Add(this.grpDates);
            this.pnlSubscriptionsRight.Controls.Add(this.grpIssues);
            this.pnlSubscriptionsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubscriptionsRight.Location = new System.Drawing.Point(0, 0);
            this.pnlSubscriptionsRight.Name = "pnlSubscriptionsRight";
            this.pnlSubscriptionsRight.Size = new System.Drawing.Size(676, 246);
            this.pnlSubscriptionsRight.TabIndex = 0;
            this.pnlSubscriptionsRight.Tag = "CustomDisableAlthoughInvisible";

            //
            // btnCreatedSubscriptions
            //
            this.btnCreatedSubscriptions.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedSubscriptions.CreatedBy = null;
            this.btnCreatedSubscriptions.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedSubscriptions.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedSubscriptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedSubscriptions.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedSubscriptions.Image")));
            this.btnCreatedSubscriptions.Location = new System.Drawing.Point(660, 6);
            this.btnCreatedSubscriptions.ModifiedBy = null;
            this.btnCreatedSubscriptions.Name = "btnCreatedSubscriptions";
            this.btnCreatedSubscriptions.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedSubscriptions.TabIndex = 4;
            this.btnCreatedSubscriptions.Tag = "dontdisable";

            //
            // grpSubscription
            //
            this.grpSubscription.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSubscription.Controls.Add(this.Label1);
            this.grpSubscription.Controls.Add(this.cmbPublicationCode);
            this.grpSubscription.Controls.Add(this.cmbSubscriptionStatus);
            this.grpSubscription.Controls.Add(this.Label3);
            this.grpSubscription.Controls.Add(this.Label2);
            this.grpSubscription.Controls.Add(this.chkFreeSubscription);
            this.grpSubscription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpSubscription.Location = new System.Drawing.Point(4, 4);
            this.grpSubscription.Name = "grpSubscription";
            this.grpSubscription.Size = new System.Drawing.Size(410, 84);
            this.grpSubscription.TabIndex = 0;
            this.grpSubscription.TabStop = false;
            this.grpSubscription.Tag = "CustomDisableAlthoughInvisible";
            this.grpSubscription.Text = "Subscription";

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(10, 60);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(128, 18);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "Free Subscri&ption:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // cmbPublicationCode
            //
            this.cmbPublicationCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPublicationCode.ComboBoxWidth = 110;
            this.cmbPublicationCode.Filter = null;
            this.cmbPublicationCode.ListTable = TCmbAutoPopulated.TListTableEnum.PublicationList;
            this.cmbPublicationCode.Location = new System.Drawing.Point(142, 16);
            this.cmbPublicationCode.Name = "cmbPublicationCode";
            this.cmbPublicationCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbPublicationCode.SelectedItem")));
            this.cmbPublicationCode.SelectedValue = null;
            this.cmbPublicationCode.Size = new System.Drawing.Size(258, 22);
            this.cmbPublicationCode.TabIndex = 1;
            this.cmbPublicationCode.Tag = "CustomDisableAlthoughInvisible";
            this.cmbPublicationCode.Leave += new System.EventHandler(this.CmbPublicationCode_Leave);

            //
            // cmbSubscriptionStatus
            //
            this.cmbSubscriptionStatus.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSubscriptionStatus.ComboBoxWidth = 110;
            this.cmbSubscriptionStatus.Filter = null;
            this.cmbSubscriptionStatus.ListTable = TCmbAutoPopulated.TListTableEnum.SubscriptionStatus;
            this.cmbSubscriptionStatus.Location = new System.Drawing.Point(142, 38);
            this.cmbSubscriptionStatus.Name = "cmbSubscriptionStatus";
            this.cmbSubscriptionStatus.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbSubscriptionStatus.SelectedItem")));
            this.cmbSubscriptionStatus.SelectedValue = null;
            this.cmbSubscriptionStatus.Size = new System.Drawing.Size(258, 22);
            this.cmbSubscriptionStatus.TabIndex = 3;
            this.cmbSubscriptionStatus.Tag = "CustomDisableAlthoughInvisible";

            //
            // Label3
            //
            this.Label3.Location = new System.Drawing.Point(10, 16);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(128, 20);
            this.Label3.TabIndex = 0;
            this.Label3.Text = "Pub&lication Code:";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label2
            //
            this.Label2.Location = new System.Drawing.Point(10, 38);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(128, 20);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "S&ubscription Status:";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // chkFreeSubscription
            //
            this.chkFreeSubscription.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFreeSubscription.Location = new System.Drawing.Point(142, 58);
            this.chkFreeSubscription.Name = "chkFreeSubscription";
            this.chkFreeSubscription.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkFreeSubscription.Size = new System.Drawing.Size(12, 20);
            this.chkFreeSubscription.TabIndex = 5;
            this.chkFreeSubscription.Tag = "CustomDisableAlthoughInvisible";
            this.chkFreeSubscription.CheckStateChanged += new System.EventHandler(this.ChkFreeSubscription_CheckStateChanged);

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.txtPublicationCost);
            this.grpMisc.Controls.Add(this.txtPublicationCostCurrency);
            this.grpMisc.Controls.Add(this.cmbReasonEnded);
            this.grpMisc.Controls.Add(this.cmbReasonGiven);
            this.grpMisc.Controls.Add(this.Label5);
            this.grpMisc.Controls.Add(this.Label4);
            this.grpMisc.Controls.Add(this.txtComplimentary);
            this.grpMisc.Controls.Add(this.txtCopies);
            this.grpMisc.Controls.Add(this.Label6);
            this.grpMisc.Controls.Add(this.Label7);
            this.grpMisc.Controls.Add(this.Label8);
            this.grpMisc.Controls.Add(this.txtContactPartnerBox);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 90);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(410, 154);
            this.grpMisc.TabIndex = 1;
            this.grpMisc.TabStop = false;
            this.grpMisc.Tag = "CustomDisableAlthoughInvisible";
            this.grpMisc.Text = "Miscellaneous";

            //
            // txtPublicationCost
            //
            this.txtPublicationCost.BackColor = System.Drawing.SystemColors.Control;
            this.txtPublicationCost.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPublicationCost.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPublicationCost.Location = new System.Drawing.Point(144, 16);
            this.txtPublicationCost.Name = "txtPublicationCost";
            this.txtPublicationCost.ReadOnly = true;
            this.txtPublicationCost.Size = new System.Drawing.Size(46, 14);
            this.txtPublicationCost.TabIndex = 1;
            this.txtPublicationCost.TabStop = false;
            this.txtPublicationCost.Tag = "dontdisable";
            this.txtPublicationCost.Text = "0.0";
            this.tipInfo.SetToolTip(this.txtPublicationCost, "Publication Cost (exclud" + "ing Postage)");

            //
            // txtPublicationCostCurrency
            //
            this.txtPublicationCostCurrency.BackColor = System.Drawing.SystemColors.Control;
            this.txtPublicationCostCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPublicationCostCurrency.Font = new System.Drawing.Font("Verdan" + 'a',
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtPublicationCostCurrency.Location = new System.Drawing.Point(200, 16);
            this.txtPublicationCostCurrency.Name = "txtPublicationCostCurrency";
            this.txtPublicationCostCurrency.ReadOnly = true;
            this.txtPublicationCostCurrency.Size = new System.Drawing.Size(42, 14);
            this.txtPublicationCostCurrency.TabIndex = 2;
            this.txtPublicationCostCurrency.TabStop = false;
            this.txtPublicationCostCurrency.Tag = "dontdisable";
            this.txtPublicationCostCurrency.Text = "CUR";
            this.tipInfo.SetToolTip(this.txtPublicationCostCurrency, "Publication Cost" + " Currency");

            //
            // cmbReasonEnded
            //
            this.cmbReasonEnded.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbReasonEnded.ComboBoxWidth = 110;
            this.cmbReasonEnded.Filter = null;
            this.cmbReasonEnded.ListTable = TCmbAutoPopulated.TListTableEnum.ReasonSubscriptionCancelledList;
            this.cmbReasonEnded.Location = new System.Drawing.Point(142, 100);
            this.cmbReasonEnded.Name = "cmbReasonEnded";
            this.cmbReasonEnded.SelectedItem = ((System.Object)(resources.GetObject("cm" + "bReasonEnded.SelectedItem")));
            this.cmbReasonEnded.SelectedValue = null;
            this.cmbReasonEnded.Size = new System.Drawing.Size(258, 22);
            this.cmbReasonEnded.TabIndex = 10;
            this.cmbReasonEnded.Tag = "CustomDisableAlthoughInvisible";

            //
            // cmbReasonGiven
            //
            this.cmbReasonGiven.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbReasonGiven.ComboBoxWidth = 110;
            this.cmbReasonGiven.Filter = null;
            this.cmbReasonGiven.ListTable = TCmbAutoPopulated.TListTableEnum.ReasonSubscriptionGivenList;
            this.cmbReasonGiven.Location = new System.Drawing.Point(142, 78);
            this.cmbReasonGiven.Name = "cmbReasonGiven";
            this.cmbReasonGiven.SelectedItem = ((System.Object)(resources.GetObject("cm" + "bReasonGiven.SelectedItem")));
            this.cmbReasonGiven.SelectedValue = null;
            this.cmbReasonGiven.Size = new System.Drawing.Size(258, 22);
            this.cmbReasonGiven.TabIndex = 8;
            this.cmbReasonGiven.Tag = "CustomDisableAlthoughInvisible";

            //
            // Label5
            //
            this.Label5.Location = new System.Drawing.Point(10, 36);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(126, 23);
            this.Label5.TabIndex = 3;
            this.Label5.Text = "Complimentar&y:";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // Label4
            //
            this.Label4.Location = new System.Drawing.Point(10, 16);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(126, 23);
            this.Label4.TabIndex = 0;
            this.Label4.Text = "Publication Cost:";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // txtComplimentary
            //
            this.txtComplimentary.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtComplimentary.Location = new System.Drawing.Point(142, 34);
            this.txtComplimentary.Name = "txtComplimentary";
            this.txtComplimentary.Size = new System.Drawing.Size(44, 21);
            this.txtComplimentary.TabIndex = 4;
            this.txtComplimentary.Tag = "CustomDisableAlthoughInvisible";
            this.txtComplimentary.Text = "1";

            //
            // txtCopies
            //
            this.txtCopies.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtCopies.Location = new System.Drawing.Point(142, 56);
            this.txtCopies.Name = "txtCopies";
            this.txtCopies.Size = new System.Drawing.Size(44, 21);
            this.txtCopies.TabIndex = 6;
            this.txtCopies.Tag = "CustomDisableAlthoughInvisible";
            this.txtCopies.Text = "1";

            //
            // Label6
            //
            this.Label6.Location = new System.Drawing.Point(10, 58);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(126, 23);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "&Copies:";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // Label7
            //
            this.Label7.Location = new System.Drawing.Point(10, 78);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(128, 20);
            this.Label7.TabIndex = 7;
            this.Label7.Text = "Reason &Given:";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label8
            //
            this.Label8.Location = new System.Drawing.Point(10, 100);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(128, 20);
            this.Label8.TabIndex = 9;
            this.Label8.Text = "Reason Ended";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtContactPartnerBox
            //
            this.txtContactPartnerBox.ASpecialSetting = true;
            this.txtContactPartnerBox.ButtonText = "Gift Given &By";
            this.txtContactPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtContactPartnerBox.ButtonWidth = 108;
            this.txtContactPartnerBox.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtContactPartnerBox.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtContactPartnerBox.Location = new System.Drawing.Point(32, 122);
            this.txtContactPartnerBox.MaxLength = 32767;
            this.txtContactPartnerBox.Name = "txtContactPartnerBox";
            this.txtContactPartnerBox.PartnerClass = "";
            this.txtContactPartnerBox.PreventFaultyLeaving = false;
            this.txtContactPartnerBox.ReadOnly = false;
            this.txtContactPartnerBox.Size = new System.Drawing.Size(366, 23);
            this.txtContactPartnerBox.TabIndex = 8;
            this.txtContactPartnerBox.Tag = "";
            this.txtContactPartnerBox.TextBoxWidth = 80;
            this.txtContactPartnerBox.VerificationResultCollection = null;

            //
            // grpDates
            //
            this.grpDates.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.grpDates.Controls.Add(this.txtDateEnded);
            this.grpDates.Controls.Add(this.txtNoticeSent);
            this.grpDates.Controls.Add(this.txtDateRenewed);
            this.grpDates.Controls.Add(this.txtExpiryDate);
            this.grpDates.Controls.Add(this.txtStartDate);
            this.grpDates.Controls.Add(this.Label21);
            this.grpDates.Controls.Add(this.Label9);
            this.grpDates.Controls.Add(this.Label10);
            this.grpDates.Controls.Add(this.Label23);
            this.grpDates.Controls.Add(this.Label19);
            this.grpDates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpDates.Location = new System.Drawing.Point(418, 4);
            this.grpDates.Name = "grpDates";
            this.grpDates.Size = new System.Drawing.Size(239, 136);
            this.grpDates.TabIndex = 2;
            this.grpDates.TabStop = false;
            this.grpDates.Tag = "CustomDisableAlthoughInvisible";
            this.grpDates.Text = "Dates";

            //
            // txtDateEnded
            //
            this.txtDateEnded.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtDateEnded.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtDateEnded.Location = new System.Drawing.Point(126, 106);
            this.txtDateEnded.Name = "txtDateEnded";
            this.txtDateEnded.TabIndex = 9;
            this.txtDateEnded.Tag = "CustomDisableAlthoughInvisible";
            this.txtDateEnded.Text = "Date Ended";

            //
            // txtNoticeSent
            //
            this.txtNoticeSent.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtNoticeSent.Font = new System.Drawing.Font("Microsoft Sans Seri" + 'f',
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtNoticeSent.Location = new System.Drawing.Point(126, 82);
            this.txtNoticeSent.Name = "txtNoticeSent";
            this.txtNoticeSent.TabIndex = 7;
            this.txtNoticeSent.Tag = "CustomDisableAlthoughInvisible";
            this.txtNoticeSent.Text = "Notice Sent";

            //
            // txtDateRenewed
            //
            this.txtDateRenewed.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtDateRenewed.Font = new System.Drawing.Font("Microsoft Sans Ser" + "if",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtDateRenewed.Location = new System.Drawing.Point(126, 60);
            this.txtDateRenewed.Name = "txtDateRenewed";
            this.txtDateRenewed.TabIndex = 5;
            this.txtDateRenewed.Tag = "CustomDisableAlthoughInvisible";
            this.txtDateRenewed.Text = "Date Renewed";

            //
            // txtExpiryDate
            //
            this.txtExpiryDate.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtExpiryDate.Font = new System.Drawing.Font("Microsoft Sans Seri" + 'f',
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtExpiryDate.Location = new System.Drawing.Point(126, 38);
            this.txtExpiryDate.Name = "txtExpiryDate";
            this.txtExpiryDate.TabIndex = 3;
            this.txtExpiryDate.Tag = "CustomDisableAlthoughInvisible";
            this.txtExpiryDate.Text = "Expiry Date";

            //
            // txtStartDate
            //
            this.txtStartDate.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtStartDate.Location = new System.Drawing.Point(126, 16);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.TabIndex = 1;
            this.txtStartDate.Tag = "CustomDisableAlthoughInvisible";
            this.txtStartDate.Text = "Start Date";

            //
            // Label21
            //
            this.Label21.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label21.Location = new System.Drawing.Point(12, 64);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(108, 20);
            this.Label21.TabIndex = 4;
            this.Label21.Text = "Date Rene&wed:";
            this.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label9
            //
            this.Label9.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label9.Location = new System.Drawing.Point(12, 40);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(108, 20);
            this.Label9.TabIndex = 2;
            this.Label9.Text = "E&xpiry Date:";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label10
            //
            this.Label10.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label10.Location = new System.Drawing.Point(12, 18);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(108, 20);
            this.Label10.TabIndex = 0;
            this.Label10.Text = "S&tart Date:";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label23
            //
            this.Label23.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label23.Location = new System.Drawing.Point(12, 106);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(108, 20);
            this.Label23.TabIndex = 8;
            this.Label23.Text = "Date E&nded:";
            this.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label19
            //
            this.Label19.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label19.Location = new System.Drawing.Point(12, 86);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(108, 20);
            this.Label19.TabIndex = 6;
            this.Label19.Text = "Not&ice Sent:";
            this.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // grpIssues
            //
            this.grpIssues.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.grpIssues.Controls.Add(this.btnEditIssues);
            this.grpIssues.Controls.Add(this.txtLastIssueSent);
            this.grpIssues.Controls.Add(this.txtFirstIssueSent);
            this.grpIssues.Controls.Add(this.Label22);
            this.grpIssues.Controls.Add(this.txtIssuesReceived);
            this.grpIssues.Controls.Add(this.Label24);
            this.grpIssues.Controls.Add(this.Label25);
            this.grpIssues.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpIssues.Location = new System.Drawing.Point(418, 142);
            this.grpIssues.Name = "grpIssues";
            this.grpIssues.Size = new System.Drawing.Size(239, 102);
            this.grpIssues.TabIndex = 3;
            this.grpIssues.TabStop = false;
            this.grpIssues.Tag = "CustomDisableAlthoughInvisible";
            this.grpIssues.Text = "Issues";

            //
            // btnEditIssues
            //
            this.btnEditIssues.Location = new System.Drawing.Point(40, 76);
            this.btnEditIssues.Name = "btnEditIssues";
            this.btnEditIssues.Size = new System.Drawing.Size(186, 22);
            this.btnEditIssues.TabIndex = 6;
            this.btnEditIssues.Tag = "dontdisable";
            this.btnEditIssues.Text = "Edit I&ssues";
            this.btnEditIssues.Click += new System.EventHandler(this.BtnEditIssues_Click);

            //
            // txtLastIssueSent
            //
            this.txtLastIssueSent.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtLastIssueSent.Font = new System.Drawing.Font("Microsoft Sans S" + "erif",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtLastIssueSent.Location = new System.Drawing.Point(126, 53);
            this.txtLastIssueSent.Name = "txtLastIssueSent";
            this.txtLastIssueSent.TabIndex = 5;
            this.txtLastIssueSent.Tag = "CustomDisableAlthoughInvisible";
            this.txtLastIssueSent.Text = "Last Issue Sent";

            //
            // txtFirstIssueSent
            //
            this.txtFirstIssueSent.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.txtFirstIssueSent.Font = new System.Drawing.Font("Microsoft Sans " + "Serif",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtFirstIssueSent.Location = new System.Drawing.Point(126, 32);
            this.txtFirstIssueSent.Name = "txtFirstIssueSent";
            this.txtFirstIssueSent.TabIndex = 3;
            this.txtFirstIssueSent.Tag = "CustomDisableAlthoughInvisible";
            this.txtFirstIssueSent.Text = "First Issue Sent";

            //
            // Label22
            //
            this.Label22.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label22.Location = new System.Drawing.Point(11, 14);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(108, 20);
            this.Label22.TabIndex = 0;
            this.Label22.Text = "Issues Recei&ved:";
            this.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtIssuesReceived
            //
            this.txtIssuesReceived.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtIssuesReceived.Location = new System.Drawing.Point(126, 10);
            this.txtIssuesReceived.Name = "txtIssuesReceived";
            this.txtIssuesReceived.Size = new System.Drawing.Size(40, 21);
            this.txtIssuesReceived.TabIndex = 1;
            this.txtIssuesReceived.Tag = "CustomDisableAlthoughInvisible";
            this.txtIssuesReceived.Text = "0";

            //
            // Label24
            //
            this.Label24.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label24.Location = new System.Drawing.Point(11, 55);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(108, 17);
            this.Label24.TabIndex = 4;
            this.Label24.Text = "L&ast Issue Sent:";
            this.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label25
            //
            this.Label25.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.Label25.Location = new System.Drawing.Point(11, 31);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(108, 20);
            this.Label25.TabIndex = 2;
            this.Label25.Text = "Fi&rst Issue Sent:";
            this.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // tipInfo
            //
            this.tipInfo.AutoPopDelay = 4000;
            this.tipInfo.InitialDelay = 500;
            this.tipInfo.ReshowDelay = 100;

            //
            // TUCPartnerSubscription
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.pnlSubscriptionsRight);
            this.Name = "TUCPartnerSubscription";
            this.Size = new System.Drawing.Size(676, 246);
            this.pnlSubscriptionsRight.ResumeLayout(false);
            this.grpSubscription.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.grpDates.ResumeLayout(false);
            this.grpIssues.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public Boolean IsFreeSubscription()
        {
            return this.chkFreeSubscription.Checked;
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        public void MakeScreenInvisible(Boolean value)
        {
            // Set the groupboxes of this UserControl visible or invisible.
            this.grpMisc.Visible = value;
            this.grpSubscription.Visible = value;
            this.grpDates.Visible = value;
            this.grpIssues.Visible = value;
        }

        #endregion

        /// <summary>
        /// Default Constructor.
        ///
        /// Initialises Fields.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerSubscription() : base ()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FtmpPartnerKeyValid = true;
            FDataColumnCollection = new ArrayList();
        }

        // this is the heart of displaying the data
        // This is set by Subscriptions, and will display the selected
        // subscription on the lower half of screen

        /// <summary>
        /// checks that the publication is valid.
        /// </summary>
        /// <returns>void</returns>
        private void CmbPublicationCode_Leave(System.Object sender, System.EventArgs e)
        {
            DataTable DataCachePublicationListTable;
            PPublicationRow TmpRow;

            try
            {
                // check if the publication selected is valid, if not, gives warning.
                DataCachePublicationListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
                TmpRow = (PPublicationRow)DataCachePublicationListTable.Rows.Find(new Object[] { this.cmbPublicationCode.SelectedItem.ToString() });

                if (TmpRow.ValidPublication)
                {
                    FSelectedPubclicationCode = cmbPublicationCode.SelectedItem;
                }
                else
                {
                    if (MessageBox.Show("Please note that Publication '" + this.cmbPublicationCode.SelectedItem.ToString() +
                            "'\r\nis no longer available." + "\r\n" + "" + "Do you still want to add a subscription for it?", "Create Subscription",
                            System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        // If user selects not to use the publication, the recent publication code is selected.
                        this.cmbPublicationCode.SelectedItem = FSelectedPubclicationCode;
                    }
                    else
                    {
                        FSelectedPubclicationCode = cmbPublicationCode.SelectedItem;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// fires the cmbPublicationCode_SelectedValueChanged(...)
        /// </summary>
        /// <returns>void</returns>
        private void ChkFreeSubscription_CheckStateChanged(System.Object sender, System.EventArgs e)
        {
            CmbPublicationCode_SelectedValueChanged(new object(), new System.EventArgs());
        }

        /// <summary>
        /// adds and returns a new Rowtyped to FMainDS
        /// </summary>
        /// <returns>void</returns>
        public void CreateNewRow(out PSubscriptionRow Row, Boolean DataViewExist)
        {
            // Types a new roe to the MainDataSet.
            Row = FMainDS.PSubscription.NewRowTyped();
        }

        /// <summary>
        /// Sets the Issues groupBox components editable
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditIssues_Click(System.Object sender, System.EventArgs e)
        {
            // if anwered OK to question below, the Issuesgroupbox screenparts are enabled.
            if (Convert.ToInt16(MessageBox.Show(StrIssuesMaintained, StrIssuesMaintaindedTitle, System.Windows.Forms.MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) == 1)
            {
                CustomEnablingDisabling.EnableControlGroup(grpIssues);
                this.btnEditIssues.Visible = false;
                this.txtIssuesReceived.Focus();
            }
            else
            {
            }
        }

        // Working ok.

        /// <summary>
        /// When Publication value is changed sets the other screen components to answer the needs of new value
        /// </summary>
        /// <returns>void</returns>
        private void CmbPublicationCode_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            DataRow[] PublicationCostRows = null;

            // if the Subscription is free, the cost is always 0!
            if (this.chkFreeSubscription.Checked)
            {
                SetupPublicationCost(0, "");
            }
            else
            {
                // If any Publications
                if (FSubscriptionDV.Count > 0)
                {
                    PublicationCostRows = FPublicationCostDT.Select(
                        PPublicationTable.GetPublicationCodeDBName() + " = '" + this.cmbPublicationCode.SelectedItem.ToString() + "'");

                    // if the Subscription has a cost, set it, else set the cost to 0.
                    if (PublicationCostRows.Length > 0)
                    {
                        SetupPublicationCost(((PPublicationCostRow)PublicationCostRows[0]).PublicationCost,
                            ((PPublicationCostRow)PublicationCostRows[0]).CurrencyCode);
                    }
                    else
                    {
                        SetupPublicationCost(0, "");
                    }
                }
                else
                {
                    this.btnEditIssues.Enabled = false;
                }
            }
        }

        /// <summary>
        /// When Subscrication value is changed sets the other screen components to answer the needs of new value
        /// </summary>
        /// <returns>void</returns>
        private void CmbSubscriptionStatus_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            PSubscriptionRow Row;

            // this is giving me so much trouble I am trying a new approach TimH
            // MessageBox.Show('cmbSubscriptionStatus_SelectedValueChanged');
            if (FPublicationCode == null)
            {
                return;

                // don't need to do anything
            }

            // get the current Row
            Row = (PSubscriptionRow)FSubscriptionDV[0].Row;

            if (this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_GIFT)
            {
                // GIFT
                // enable Gift From partner key text box
                this.txtContactPartnerBox.Enabled = true;

                // clear any previously supplied reason for cancellation
                Row.SetReasonSubsCancelledCodeNull();
                this.cmbReasonEnded.SelectedValue = "";
                this.cmbReasonEnded.Enabled = false;

                // clear any previously supplied Date Ended
                this.txtDateEnded.Enabled = false;
                txtDateEnded.Text = "";
                Row.SetDateCancelledNull();
            }
            else if ((this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED)
                     || (this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_EXPIRED))
            {
                // CANCELLED or EXPIRED
                // Set the DateEnded field to todays date:
                this.txtDateEnded.Enabled = true;
                txtDateEnded.Text = StringHelper.DateToLocalizedString(DateTime.Now, false);
                Row.DateCancelled = DateTime.Now.Date;

                // clear any previously supplied partner key
                Row.GiftFromKey = 0;
                txtContactPartnerBox.Text = "0";
                this.txtContactPartnerBox.Enabled = false;

                // allow them to enter a reason ended
                this.cmbReasonEnded.Enabled = true;

                // DONT DO THIS  IT BREAKS DATA BINDING
                // this.cmbReasonEnded.Focus();
                // try and open combo box
                if (FDataMode == TDataModeEnum.dmEdit)
                {
                    this.cmbReasonEnded.DropDown();
                }
            }
            else
            {
                // All other Cases
                // clear any previously supplied partner key
                Row.GiftFromKey = 0;
                txtContactPartnerBox.Text = "0";
                this.txtContactPartnerBox.Enabled = false;

                // clear any previously supplied Date Ended
                Row.SetDateCancelledNull();
                this.txtDateEnded.Text = "";
                this.txtDateEnded.Enabled = false;

                // clear any previously supplied reason for cancellation
                Row.ReasonSubsCancelledCode = "";
                this.cmbReasonEnded.SelectedValue = "";
                this.cmbReasonEnded.Enabled = false;
            }
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
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

        public void DoEditIssues_buttonEvent()
        {
            BtnEditIssues_Click(this, null);
        }

        /// <summary>
        /// Sets some of the screenparts when editMode is on
        /// </summary>
        /// <returns>void</returns>
        public void EditModeForSubscription()
        {
            PSubscriptionRow Row;

            this.cmbPublicationCode.Enabled = false;

            // if any Subscription
            if (FSubscriptionDV.Count > 0)
            {
                // get the selected Row.
                Row = (PSubscriptionRow)FSubscriptionDV[0].Row;

                if (Row.IsDateCancelledNull() == true)
                {
                    FtmpDateCancelledDate = new DateTime(1111, 1, 1);
                }
                else
                {
                    FtmpDateCancelledDate = Row.DateCancelled;
                }

                if (Row.IsDateNoticeSentNull() == true)
                {
                    this.txtNoticeSent.Text = "";
                }

                if (Row.IsExpiryDateNull() == true)
                {
                    this.txtExpiryDate.Text = "";
                }

                if (Row.IsFirstIssueNull() == true)
                {
                    this.txtFirstIssueSent.Text = "";
                }

                if (Row.IsLastIssueNull() == true)
                {
                    this.txtLastIssueSent.Text = "";
                }

                if (Row.IsDateCreatedNull() == true)
                {
                    this.txtStartDate.Text = "";
                }

                if (Row.IsSubscriptionRenewalDateNull() == true)
                {
                    this.txtDateRenewed.Text = "";
                }

                // What to be done in different subscription states:
                this.SubscriptionState(false);

                // txtStartDate.Focus();
                // SendKeys.Send('TAB');
                // SendKeys.Send('TAB');
            }
        }

        /// <summary>
        /// Enables or disables the issuesnutton.
        /// </summary>
        /// <returns>void</returns>
        public void EnableDisableIssuesButton(Boolean value)
        {
            this.btnEditIssues.Enabled = value;
            this.btnEditIssues.Visible = value;
        }

        /// <summary>
        /// Sets the FSubscriptionDV unable to edit.
        /// </summary>
        /// <returns>void</returns>
        public void EndEdit()
        {
            FSubscriptionDV.AllowEdit = false;
        }

        public PSubscriptionRow GetCurrentSubscriptionRow()
        {
            return (PSubscriptionRow)FSubscriptionDV[0].Row;
        }

        /// <summary>
        /// gets the selected Subscriptionstatus
        /// </summary>
        /// <returns>void</returns>
        public String GetCurrentSubscriptionStatus()
        {
            return this.cmbSubscriptionStatus.SelectedItem.ToString();
        }

        /// <summary>
        /// Returns the selected dataRow
        /// </summary>
        /// <returns>void</returns>
        public Boolean GetSelectedDataRow(out PSubscriptionRow tmpRow)
        {
            Boolean ReturnValue;

            tmpRow = null;

            if (FSubscriptionDV.Count > 0)
            {
                tmpRow = (PSubscriptionRow)FSubscriptionDV[0].Row;
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns the PublicationCode selected in combobox.
        /// </summary>
        /// <returns>void</returns>
        public String GetSelectedPublicationcode()
        {
            return this.cmbPublicationCode.SelectedValue.ToString();
        }

        /// <summary>
        /// returns the SubscriptionTable, that has the recent updates.
        /// </summary>
        /// <returns>void</returns>
        public DataTable GetSubscriptionDV()
        {
            return FMainDS.PSubscription;
        }

        public void InitialiseDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            FDelegateGetPartnerShortName = ADelegateFunction;
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
        public void PerformDataBinding(PartnerEditTDS AMainDS, String APublicationCode)
        {
            Binding DateFormatBinding;
            Binding NullableNumberFormatBinding;

            try
            {
                FMainDS = AMainDS;
            }
            catch (System.NullReferenceException e)
            {
                MessageBox.Show(" From TUCPartnerSubscription.PerformDataBinding :" + e.StackTrace);
            }

            // create a DataView
            FSubscriptionDV = new DataView(FMainDS.PSubscription);

            // filter DataView to show only a certain record
            FSubscriptionDV.RowFilter = PSubscriptionTable.GetPublicationCodeDBName() + " = '" + APublicationCode + "'";

            // messagebox.Show('Number of Rows: ' + FSubscriptionDV.Count.ToString);
            // DataBind the controls to the DataViews
            chkFreeSubscription.DataBindings.Add("Checked", FSubscriptionDV, PSubscriptionTable.GetGratisSubscriptionDBName());
            this.txtComplimentary.DataBindings.Add("Text", FSubscriptionDV, PSubscriptionTable.GetNumberComplimentaryDBName());
            this.txtCopies.DataBindings.Add("Text", FSubscriptionDV, PSubscriptionTable.GetPublicationCopiesDBName());

            // Issues
            NullableNumberFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetNumberIssuesReceivedDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtIssuesReceived.DataBindings.Add(NullableNumberFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetFirstIssueDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtFirstIssueSent.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetLastIssueDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtLastIssueSent.DataBindings.Add(DateFormatBinding);

            // Dates
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetStartDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtStartDate.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetExpiryDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtExpiryDate.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetSubscriptionRenewalDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtDateRenewed.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetDateNoticeSentDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtNoticeSent.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FSubscriptionDV, PSubscriptionTable.GetDateCancelledDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            this.txtDateEnded.DataBindings.Add(DateFormatBinding);

            // DataBind AutoPopulatingComboBoxes
            cmbPublicationCode.PerformDataBinding(FSubscriptionDV, PSubscriptionTable.GetPublicationCodeDBName());
            cmbSubscriptionStatus.PerformDataBinding(FSubscriptionDV, PSubscriptionTable.GetSubscriptionStatusDBName());
            cmbReasonGiven.PerformDataBinding(FSubscriptionDV, PSubscriptionTable.GetReasonSubsGivenCodeDBName());
            cmbReasonEnded.PerformDataBinding(FSubscriptionDV, PSubscriptionTable.GetReasonSubsCancelledCodeDBName());
            btnCreatedSubscriptions.UpdateFields(FSubscriptionDV);

            // Extender Provider
            // this.expStringLengthCheckSubscription.RetrieveTextboxes(this);
            // DataBind autopopulting PartnerBox.
            // This proved to be the mother of all evil
            // Now tradionally bound, the VB6 way :)
            // this.txtContactPartnerBox.PerformDataBinding(FSubscriptionDV, PSubscriptionTable.GetGiftFromKeyDBName());
            this.txtContactPartnerBox.InitialiseUserControl();

            // Set StatusBar Texts
            // with stbUCPartnerSubscription do
            SetStatusBarText(this.btnEditIssues, "Enables editing of Issues");
            SetStatusBarText(txtContactPartnerBox, PSubscriptionTable.GetGiftFromKeyHelp());
            SetStatusBarText(cmbPublicationCode, PPublicationTable.GetPublicationCodeHelp());
            SetStatusBarText(this.cmbSubscriptionStatus, PSubscriptionTable.GetSubscriptionStatusHelp());
            SetStatusBarText(this.chkFreeSubscription, PSubscriptionTable.GetFirstIssueHelp());
            SetStatusBarText(this.txtComplimentary, PSubscriptionTable.GetNumberComplimentaryHelp());
            SetStatusBarText(this.txtCopies, PSubscriptionTable.GetPublicationCopiesHelp());
            SetStatusBarText(this.cmbReasonGiven, PSubscriptionTable.GetReasonSubsGivenCodeHelp());
            SetStatusBarText(this.cmbReasonEnded, PSubscriptionTable.GetReasonSubsCancelledCodeHelp());
            SetStatusBarText(this.txtStartDate, PSubscriptionTable.GetStartDateHelp());
            SetStatusBarText(this.txtExpiryDate, PSubscriptionTable.GetExpiryDateHelp());
            SetStatusBarText(this.txtDateRenewed, PSubscriptionTable.GetSubscriptionRenewalDateHelp());
            SetStatusBarText(this.txtNoticeSent, PSubscriptionTable.GetDateNoticeSentHelp());
            SetStatusBarText(this.txtDateEnded, PSubscriptionTable.GetDateCancelledHelp());
            SetStatusBarText(this.txtIssuesReceived, PSubscriptionTable.GetNumberIssuesReceivedHelp());
            SetStatusBarText(this.txtFirstIssueSent, PSubscriptionTable.GetFirstIssueHelp());
            SetStatusBarText(this.txtLastIssueSent, PSubscriptionTable.GetLastIssueHelp());
            FPublicationCostDT = (PPublicationCostTable)TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                TCacheableSubscriptionsTablesEnum.PublicationCost);

            // hook Event that allow data verification
            FMainDS.PSubscription.ColumnChanged += new DataColumnChangeEventHandler(this.OnPartnerSubscriptionDataColumnChanging);

            // hookup Subscription Status combo box
            this.cmbSubscriptionStatus.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbSubscriptionStatus_SelectedValueChanged);

            // hookup Publication Code combo box
            this.cmbPublicationCode.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbPublicationCode_SelectedValueChanged);
        }

        public Boolean CheckCorrectnessOfValues()
        {
            Boolean ReturnValue;
            PSubscriptionRow Row;
            TVerificationResultCollection VerificationResultCollection;
            TVerificationResult VerificationResult;
            DataColumn TmpDC;
            Control BoundControl;

            // tmtString: String;
            // manually update value from partner key text box
            FSubscriptionDV[0][PSubscriptionTable.GetGiftFromKeyDBName()] = txtContactPartnerBox.Text;
            Row = (PSubscriptionRow)FSubscriptionDV[0].Row;

            if (!TPartnerSubscriptionVerification.VerifySubscriptionDataFinal(Row, out VerificationResultCollection, out VerificationResult,
                    out TmpDC, FtmpPartnerKeyValid))
            {
                if (VerificationResult.FResultCode != ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE)
                {
                    // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                    MessageBox.Show(
                        VerificationResult.FResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                        VerificationResult.FResultCode +
                        Environment.NewLine + "Context: " + this.GetType().ToString() + Environment.NewLine + "Release: ",
                        VerificationResult.FResultTextCaption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FSubscriptionDV], TmpDC);

                    if (BoundControl != null)
                    {
                        BoundControl.Focus();
                    }
                }

                ReturnValue = false;
            }
            else
            {
                ReturnValue = true;
                this.cmbPublicationCode.Enabled = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// If FSubscriptionDV is empty, returns true
        /// </summary>
        /// <returns>void</returns>
        public Boolean DataviewIsEmpty()
        {
            Boolean ReturnValue;

            // if no Subscriptions returns true, else return false.
            ReturnValue = false;

            if (FSubscriptionDV.Count < 1)
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Switches between 'Read Only Mode' and 'Edit Mode' of the UserControl.
        ///
        /// </summary>
        /// <param name="ADataMode">Specify dmBrowse for read-only mode or dmEdit for edit mode.
        /// </param>
        /// <returns>void</returns>
        public void SetMode(TDataModeEnum aDataMode)
        {
            FDataMode = aDataMode;
            btnCreatedSubscriptions.UpdateFields(FSubscriptionDV);

            if (aDataMode == TDataModeEnum.dmBrowse)
            {
                CustomEnablingDisabling.DisableControlGroup(grpSubscription, @HandleDisabledControlClick);
                CustomEnablingDisabling.DisableControlGroup(grpMisc, @HandleDisabledControlClick);
                CustomEnablingDisabling.DisableControlGroup(grpDates, @HandleDisabledControlClick);
                CustomEnablingDisabling.DisableControlGroup(grpIssues, @HandleDisabledControlClick);
                this.btnEditIssues.Visible = false;
            }
            else
            {
                CustomEnablingDisabling.EnableControlGroup(grpSubscription);
                CustomEnablingDisabling.EnableControlGroup(grpMisc);
                CustomEnablingDisabling.EnableControlGroup(grpDates);

                // CustomEnablingDisabling.EnableControlGroup(grpIssues);
            }
        }

        public void AdjustLabelControlsAfterResizing()
        {
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpSubscription);
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpMisc);
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpDates);
            CustomEnablingDisabling.AdjustLabelControlsAfterResizingGroup(grpIssues);
        }

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

        // this function will be removed, but it's still useb by UC_PartnerSubscription

        /// <summary>
        /// Base class for all Security Exceptions
        /// </summary>
        /// <returns>void</returns>
        public Boolean SetNewPartner(PSubscriptionRow Row)
        {
            Boolean ReturnValue;

            ReturnValue = true;
            try
            {
                FMainDS.PSubscription.Rows.Add(Row);
            }
            catch (Exception)
            {
                MessageBox.Show(StrPartnerHasThePublication);
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sets the Publicationcost first time. This needs to be done after user presses Edit button
        /// </summary>
        /// <returns>void</returns>
        public void SetPublicationCostFirstTime()
        {
            CmbPublicationCode_SelectedValueChanged(new object(), new System.EventArgs());
        }

        /// <summary>
        /// Sets everything needed editable for a set up of new Subscription
        /// </summary>
        /// <returns>void</returns>
        public void SetScreenPartsForNewSubscription()
        {
            FtmpDateCancelledDate = new DateTime(1111, 1, 1);

            // Set here the ScreenParts for new Subscription
            CustomEnablingDisabling.DisableControlGroup(grpIssues);
            this.cmbPublicationCode.Enabled = true;
            this.cmbSubscriptionStatus.SelectedItem = MPartnerConstants.SUBSCRIPTIONS_STATUS_PERMANENT;

            // this.cmbReasonGiven.SelectedItem := StrReasonGivenDonation;
            // this.cmbReasonGiven.SelectedItem := '';
            this.cmbReasonEnded.SelectedItem = null;
            this.cmbReasonEnded.Enabled = false;
            this.chkFreeSubscription.Checked = true;
            this.txtComplimentary.Text = "1";
            this.txtCopies.Text = "1";
            this.txtExpiryDate.Text = "";
            this.txtFirstIssueSent.Text = "";
            this.txtLastIssueSent.Text = "";
            this.txtNoticeSent.Text = "";
            this.txtDateRenewed.Text = "";
            this.txtStartDate.Text = DateTime.Today.ToShortDateString();
            this.txtStartDate.Focus();
            this.txtContactPartnerBox.Enabled = false;
            this.txtDateEnded.Text = "";
            this.txtDateEnded.Enabled = false;
            this.txtIssuesReceived.Text = "0";
            this.txtContactPartnerBox.Text = "0";
            this.cmbPublicationCode.Focus();
        }

        /// <summary>
        /// Sets the Publication cost
        /// </summary>
        /// <returns>void</returns>
        public void SetupPublicationCost(double APublicationCost, String ACurrencyCode)
        {
            NumberFormatInfo NumberFormat;

            NumberFormat = new NumberFormatInfo();
            NumberFormat.NumberDecimalDigits = 2;

            if (this.chkFreeSubscription.Checked)
            {
                // APublicationCost is ignored in this case
                txtPublicationCost.Text = Convert.ToInt32(0).ToString("N", NumberFormat);
                txtPublicationCostCurrency.Text = "";
                txtPublicationCost.ForeColor = System.Drawing.SystemColors.ControlDark;
            }
            else
            {
                txtPublicationCost.Text = APublicationCost.ToString("N", NumberFormat);
                txtPublicationCostCurrency.Text = ACurrencyCode;
                txtPublicationCost.ForeColor = System.Drawing.SystemColors.WindowText;
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
        /// <param name="ARecordAdded">Set to true if a new there was just created
        /// </param>
        /// <returns>void</returns>
        public void CancelEditing(Boolean ANewFromLocation0, Boolean ARecordAdded)
        {
            System.Windows.Forms.CurrencyManager SubscriptionCurrencyManager;

            //            DataRow FSubscriptionDR;
            //            Boolean DeleteRows;

            // this is giving much trouble, trying new approach, TimH
            SubscriptionCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FSubscriptionDV];
            SubscriptionCurrencyManager.CancelCurrentEdit();
            this.cmbPublicationCode.Enabled = true;
            return;

            //            // ORIGINAL CODE BELOWI see
            //            // Get CurrencyManager that is associated with the DataTables to which the
            //            // Controls are DataBound.
            //            SubscriptionCurrencyManager = (System.Windows.Forms.CurrencyManager) BindingContext[FSubscriptionDV];
            //            // Revert all changes made since BeginEdit was called on a DataRow. This
            //            // affects the data in the DataTables to which the Controls are DataBound to
            //            // and the displayed information in the DataBound Controls.
            //            SubscriptionCurrencyManager.CancelCurrentEdit();
            //            DeleteRows = false;
            //            // Determine whether we need to delete the affected DataRows as well
            //            if (FSubscriptionDV[0].Row.RowState == DataRowState.Added)
            //            {
            //                try
            //                {
            //                    if (! FMainDS.PPartner[0].HasVersion(DataRowVersion.Original))
            //                    {
            //                        // Cancelling Editing with a New Partner
            //                        if (((new DataView(FMainDS.PSubscription, "", "", DataViewRowState.CurrentRows).Count > 1)) && ARecordAdded)
            //                        {
            //                            DeleteRows = true;
            //                        }
            //                        else
            //                        {
            //                        }
            //                        // action to do...
            //                    }
            //                    else
            //                    {
            //                        // Cancelling Editing with an existing Partner
            //                        if (ARecordAdded)
            //                        {
            //                            DeleteRows = true;
            //                        }
            //                    }
            //                }
            //                catch (Exception)
            //                {
            //                }
            //                if (DeleteRows)
            //                {
            //                    // In addition to cancelling the Edit, we also delete the DataRows
            //                    FSubscriptionDR = FSubscriptionDV[0].Row;
            //                    try
            //                    {
            //                        FSubscriptionDV.Table.Rows.Remove(FSubscriptionDR);
            //                    }
            //                    catch (Exception)
            //                    {
            //                    }
            //                }
            //            }
            //            this.cmbPublicationCode.Enabled = true;
        }

        /// <summary>
        /// If can't verify PartnerKey, returns false. This would return wether the dealing with extract or PartnerKey
        /// </summary>
        /// <returns>void</returns>
        public String Get_ExtractOrPartnerKey()
        {
            return "";
        }

        /// <summary>
        /// enables the txtContactPartnerBox
        /// </summary>
        /// <returns>void</returns>
        public void EnabletxtContactPartnerBox()
        {
            this.txtContactPartnerBox.Enabled = true;
        }

        /// <summary>
        /// Find out what's the Subscription status, and sets the other screen parts.
        /// </summary>
        /// <returns>void</returns>
        public void SubscriptionState(Boolean LChanged)
        {
            PSubscriptionRow Row;

            // if there FSubscriptionDV is empty:
            try
            {
                Row = (PSubscriptionRow)FSubscriptionDV[0].Row;

                if (Row.IsDateCancelledNull() == true)
                {
                    this.txtDateEnded.Enabled = false;
                }
                else
                {
                    this.txtDateEnded.Enabled = true;
                }
            }
            catch (Exception)
            {
            }

            // if the Subscription is ' CANCELLED' OR 'EXPIRED' , enable the combo: Reason ended
            if ((this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED)
                || (this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_EXPIRED))
            {
                this.cmbReasonEnded.Enabled = true;

                // fill reason ended txt
                // place updateCombobox() here
            }
            else
            {
                if (LChanged)
                {
                    this.cmbReasonEnded.SelectedItem = null;
                }

                this.cmbReasonEnded.Enabled = false;

                if (this.txtDateEnded.Text == "")
                {
                    this.txtDateEnded.Enabled = false;
                }
                else
                {
                    this.txtDateEnded.Enabled = true;
                }

                // place updateComboBox() here
            }

            // is Subscription is 'GIFT' then make it possible to set the Partner that gives the gift.
            if (this.cmbSubscriptionStatus.SelectedItem.ToString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_GIFT)
            {
                this.txtContactPartnerBox.Enabled = true;
            }
            else
            {
                this.txtContactPartnerBox.Enabled = false;
            }
        }

        /// <summary>
        /// Processes DataColumnChanging Event of the PSubscription DataTable.
        ///
        /// </summary>
        /// <param name="sender">Sender of the Event</param>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        private void OnPartnerSubscriptionDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            //            TVerificationResult VerificationResultReturned;
            //            TScreenVerificationResult VerificationResultEntry;
            //            Control BoundControl;

            // try disablings this logic completely
            return;

            //          try
            //          {
            //              if (TPartnerSubscriptionVerification.VerifySubscriptionData(e, FVerificationResultCollection, out VerificationResultReturned, out FDataColumnComparedTo) == false)
            //              {
            //                  if (VerificationResultReturned.FResultCode != ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE)
            //                  {
            //                      FDataColumnThatCausedVerificationError = e.Column;
            //                      FDataColumnCollection.Add((System.Object) FDataColumnThatCausedVerificationError);
            //                      // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
            //                      MessageBox.Show(VerificationResultReturned.FResultText + Environment.NewLine + Environment.NewLine + "Message Number: " + VerificationResultReturned.FResultCode + Environment.NewLine + "Context: " +
            // this.GetType().ToString() + Environment.NewLine + "Release: ", VerificationResultReturned.FResultTextCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                      // make a check that only one verification of same column is in the collection
            //                      if (! FVerificationResultCollection.contains(e.Column))
            //                      {
            //                          BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FSubscriptionDV], e.Column);
            //                          BoundControl.Focus();
            //                          VerificationResultEntry = new TScreenVerificationResult(this, e.Column, VerificationResultReturned.FResultText, VerificationResultReturned.FResultTextCaption, VerificationResultReturned.FResultCode,
            // BoundControl, VerificationResultReturned.FResultSeverity);
            //                          FVerificationResultCollection.Add(VerificationResultEntry);
            //                      }
            //                  }
            //                  else
            //                  {
            //                      // undo the change in the DataColumn
            //                      e.ProposedValue = e.Row[e.Column.ColumnName];
            //                      // need to assign this to make the change actually visible...
            //                      cmbSubscriptionStatus.SelectedItem = e.ProposedValue.ToString();
            //                      BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FSubscriptionDV], e.Column);
            //                      BoundControl.Focus();
            //                  }
            //              }
            //              else
            //              {
            //                  if (FVerificationResultCollection.contains(e.Column))
            //                  {
            //                      try
            //                      {
            //                          FVerificationResultCollection.Remove(e.Column);
            //                      }
            //                      catch (Exception)
            //                      {
            //                      }
            //                  }
            //              }
            //              if (e.Column == FDataColumnComparedTo)
            //              {
            //                  try
            //                  {
            //                      foreach (object ColumnObject in FDataColumnCollection)
            //                      {
            //                          FVerificationResultCollection.Remove((DataColumn) ColumnObject);
            //                      }
            //                  }
            //                  catch (Exception)
            //                  {
            //                  }
            //              }
            //          }
            //          catch (Exception E)
            //          {
            //              MessageBox.Show(E.ToString());
            //          }
        }
    }
}