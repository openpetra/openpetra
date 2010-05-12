//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, petrih
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using SourceGrid;
using Ict.Common.Controls;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using System.Threading;
using System.Globalization;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for editing Partner Subscription (List/Detail view).
    ///
    /// Consists of a Grid that contains a list of records, three buttons that allow
    /// manipulation of the records and a Detail UserControl to show and edit the
    /// details of the selected record.
    /// </summary>
    public class TUCPartnerSubscriptions : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        public const String StrErrorMaintainSubscription2 = "maintaining the subscription";

        /// TODO: use resourcestring from Ict.Petra.Client.MPartner.Resourcestrings.pas once this file is in Ict.Petra.Client.MPartner dll!
        //public const String StrErrorNeedToSavePartner1 = "You have to save the Partner first, before ";
        /// TODO: use resourcestring from Ict.Petra.Client.MPartner.Resourcestrings.pas once this file is in Ict.Petra.Client.MPartner dll!
        /// <summary>public const String StrErrorNeedToSavePartnerTitle = "First Save the Partner";</summary>
        public const String StrSubscriptionPerson = "Subscriptions are usually added to a FAMILY." + "\r\n" + "" +
                                                    "Are you sure you want to add to this PERSON?" + "\r\n" + "";
        public const String StrSubscriptionPersonTitle = "Confirm for PERSON";
        public const String StrInvalidDataNotCorrected = "Cannot end editing because invalid data has not been corrected!";
        public const String StrPartnerClassPerson = "PERSON";
        public const String StrCancelAllSubscriptionsTitle = "Cancel All Subscriptions";
        public const String StrCancelAllSubscriptionsNone = "There are no Subscriptions to cancel.";
        public const String StrCancelAllSubscriptionsCanceled = "No Subscriptions were cancelled.";
        public const String StrCancelAllSubscriptionsDoneTitle = "All Subscriptions Cancelled";
        public const String StrCancelAllSubscriptionsDone = "The following {0} Subscription(s) was/were cancelled:" + "\r\n" + "{1}" + "\r\n" +
                                                            "The Partner has no active Subscriptions left.";

        /// <summary>TPetraUserControl <summary> Required designer variable. </summary></summary>
        private System.ComponentModel.IContainer components;
        private TSgrdDataGrid grdRecordList;
        private System.Windows.Forms.Panel pnlSubscriptionList;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Button btnResizeSubscriptionList;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnEditRecord;
        private System.Windows.Forms.Button btnNewRecord;
        private System.Windows.Forms.Splitter splSubscriptions;

        /// <summary>Detail UserControl to show and edit the details of the selected record</summary>
        private TUCPartnerSubscription ucoDetails;
        private System.Windows.Forms.ToolTip tipMain;
        private TexpTextBoxStringLengthCheck expStringLengthCheckSubscriptions;
        private System.Windows.Forms.ContextMenu mnuActions;
        private System.Windows.Forms.MenuItem mniCancelAllSubscriptions;
        private System.Windows.Forms.Panel pnlBalloonTipAnchor;

        /// <summary>Object that holds the logic for this screen</summary>
        protected TUCPartnerSubscriptionsLogic FLogic;

        /// <summary>Holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        protected new PartnerEditTDS FMainDS;

        /// <summary>tells whether the Grid has been expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Boolean FGridMaximised;

        /// <summary>holds the height of the Grid before it got expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Int32 FGridMinimisedSize;

        /// <summary>this is for Extracts use, should be removed when Extract screen is working independently.</summary>
        protected TDelegateIsNewPartner FDelegateIsNewPartner;

        /// <summary>tells whether the currently selected record is beeing edited, or not</summary>
        protected Boolean FIsEditingRecord;

        /// <summary>tells whether the currently selected record is beeing edited, or not tells whether the Currrently edited Subscription is new, or not</summary>
        protected Boolean FdataEditOn;
        protected Boolean FNewSubscription;

        /// <summary>holds the PublicationKey of the Subscription record that was last selected in the Grid</summary>
        protected String FLastPublicationCode;

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

        /// <summary>Custom Event for enabling/disabling of other parts of the screen</summary>
        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        /// <summary>Custom Event for hooking up data change events</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>Custom Event for recalculation of the Tab Header</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this
        /// method with the code editor.
        /// /// <summary>/// Required method for Designer support  do not modify/// the contents of this method with the code editor./// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUCPartnerSubscriptions));
            this.components = new System.ComponentModel.Container();
            this.grdRecordList = new Ict.Common.Controls.TSgrdDataGrid();
            this.pnlSubscriptionList = new System.Windows.Forms.Panel();
            this.pnlBalloonTipAnchor = new System.Windows.Forms.Panel();
            this.mnuActions = new System.Windows.Forms.ContextMenu();
            this.mniCancelAllSubscriptions = new System.Windows.Forms.MenuItem();
            this.btnResizeSubscriptionList = new System.Windows.Forms.Button();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnEditRecord = new System.Windows.Forms.Button();
            this.btnNewRecord = new System.Windows.Forms.Button();
            this.splSubscriptions = new System.Windows.Forms.Splitter();
            this.ucoDetails = new Ict.Petra.Client.MPartner.TUCPartnerSubscription();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.expStringLengthCheckSubscriptions = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlSubscriptionList.SuspendLayout();
            this.SuspendLayout();

            //
            // grdRecordList
            //
            this.grdRecordList.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(255, 255, 255);
            this.grdRecordList.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRecordList.AutoFindColumn = ((Int16)(0));
            this.grdRecordList.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.grdRecordList.AutoStretchColumnsToFitWidth = false;
            this.grdRecordList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdRecordList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdRecordList.DeleteQuestionMessage = "You have chosen to delete thi" + "s record.'#13#10#13#10'Dou you really want to delete it?";
            this.grdRecordList.FixedRows = 1;
            this.grdRecordList.Location = new System.Drawing.Point(4, 6);
            this.grdRecordList.MinimumHeight = 1;
            this.grdRecordList.Name = "grdRecordList";
            this.grdRecordList.Size = new System.Drawing.Size(652, 106);
            this.grdRecordList.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            move SetStatusBarText to constructor
            split designer file
            this.SetStatusBarText(this.grdRecordList, "Subscriptions list");
            this.grdRecordList.TabIndex = 0;
            this.grdRecordList.TabStop = true;
            this.grdRecordList.DoubleClickCell += new TDoubleClickCellEventHandler(this.GrdRecordList_DoubleClickCell);
            this.grdRecordList.InsertKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_InsertKeyPressed);
            this.grdRecordList.EnterKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_EnterKeyPressed);
            this.grdRecordList.DeleteKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_DeleteKeyPressed);

            //
            // pnlSubscriptionList
            //
            this.pnlSubscriptionList.Controls.Add(this.pnlBalloonTipAnchor);

// TODO            this.pnlSubscriptionList.Controls.Add(this.tbrActions);
            this.pnlSubscriptionList.Controls.Add(this.btnResizeSubscriptionList);
            this.pnlSubscriptionList.Controls.Add(this.btnDeleteRecord);
            this.pnlSubscriptionList.Controls.Add(this.btnEditRecord);
            this.pnlSubscriptionList.Controls.Add(this.btnNewRecord);
            this.pnlSubscriptionList.Controls.Add(this.grdRecordList);
            this.pnlSubscriptionList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubscriptionList.Location = new System.Drawing.Point(0, 0);
            this.pnlSubscriptionList.Name = "pnlSubscriptionList";

            // this.rpsUserControl.SetRestoreLocation(this.pnlSubscriptionList, true); Disabled TH, bug 740
            this.pnlSubscriptionList.Size = new System.Drawing.Size(740, 115);
            this.pnlSubscriptionList.TabIndex = 1;

            //
            // pnlBalloonTipAnchor
            //
            this.pnlBalloonTipAnchor.Location = new System.Drawing.Point(365, 0);
            this.pnlBalloonTipAnchor.Name = "pnlBalloonTipAnchor";
            this.pnlBalloonTipAnchor.Size = new System.Drawing.Size(1, 1);
            this.pnlBalloonTipAnchor.TabIndex = 9;

            //
            // mnuActions
            //
            this.mnuActions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.mniCancelAllSubscriptions });

            //
            // mniCancelAllSubscriptions
            //
            this.mniCancelAllSubscriptions.Index = 0;
            this.mniCancelAllSubscriptions.Text = "&Cancel All Subscriptions...";
            this.mniCancelAllSubscriptions.Click += new System.EventHandler(this.MniCancelAllSubscriptions_Click);

            //
            // btnResizeSubscriptionList
            //
            this.btnResizeSubscriptionList.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnResizeSubscriptionList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResizeSubscriptionList.ImageIndex = 5;
            this.btnResizeSubscriptionList.ImageList = this.imlButtonIcons;
            this.btnResizeSubscriptionList.Location = new System.Drawing.Point(658, 93);
            this.btnResizeSubscriptionList.Name = "btnResizeSubscriptionList";
            this.btnResizeSubscriptionList.Size = new System.Drawing.Size(20, 18);
            this.btnResizeSubscriptionList.TabIndex = 5;
            this.tipMain.SetToolTip(this.btnResizeSubscriptionList, "Make List higher/" + "smaller");
            this.btnResizeSubscriptionList.Click += new System.EventHandler(this.BtnResizeSubscriptionList_Click);

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // btnDeleteRecord
            //
            this.btnDeleteRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnDeleteRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnDeleteRecord.ImageIndex = 2;
            this.btnDeleteRecord.ImageList = this.imlButtonIcons;
            this.btnDeleteRecord.Location = new System.Drawing.Point(664, 58);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(76, 23);
            this.SetStatusBarText(this.btnDeleteRecord, "Delete current" + "ly selected Subscription");
            this.btnDeleteRecord.TabIndex = 8;
            this.btnDeleteRecord.Text = "      &Delete";
            this.btnDeleteRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteRecord.Click += new System.EventHandler(this.BtnDeleteRecord_Click);

            //
            // btnEditRecord
            //
            this.btnEditRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnEditRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnEditRecord.ImageIndex = 1;
            this.btnEditRecord.ImageList = this.imlButtonIcons;
            this.btnEditRecord.Location = new System.Drawing.Point(664, 34);
            this.btnEditRecord.Name = "btnEditRecord";
            this.btnEditRecord.Size = new System.Drawing.Size(76, 23);
            this.SetStatusBarText(this.btnEditRecord, "Edit currently s" + "elected Subscription");
            this.btnEditRecord.TabIndex = 7;
            this.btnEditRecord.Text = "       Edi&t";
            this.btnEditRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditRecord.Click += new System.EventHandler(this.BtnEditRecord_Click);

            //
            // btnNewRecord
            //
            this.btnNewRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnNewRecord.BackColor = System.Drawing.SystemColors.Control;
            this.btnNewRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnNewRecord.ImageIndex = 0;
            this.btnNewRecord.ImageList = this.imlButtonIcons;
            this.btnNewRecord.Location = new System.Drawing.Point(664, 10);
            this.btnNewRecord.Name = "btnNewRecord";
            this.btnNewRecord.Size = new System.Drawing.Size(76, 23);
            this.SetStatusBarText(this.btnNewRecord, "Create new Subscr" + "iptions");
            this.btnNewRecord.TabIndex = 6;
            this.btnNewRecord.Text = "       &New";
            this.btnNewRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewRecord.Click += new System.EventHandler(this.BtnNewRecord_Click);

            //
            // splSubscriptions
            //
            this.splSubscriptions.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splSubscriptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.splSubscriptions.Location = new System.Drawing.Point(0, 115);
            this.splSubscriptions.MinExtra = 53;
            this.splSubscriptions.MinSize = 52;
            this.splSubscriptions.Name = "splSubscriptions";
            this.splSubscriptions.Size = new System.Drawing.Size(740, 4);
            this.splSubscriptions.TabIndex = 2;
            this.splSubscriptions.TabStop = false;

            //
            // ucoDetails
            //
            this.ucoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucoDetails.ExctractOrPartnerKey = "PartnerKey";
            this.ucoDetails.Location = new System.Drawing.Point(0, 119);
            this.ucoDetails.MainDS = null;
            this.ucoDetails.Name = "ucoDetails";
            this.ucoDetails.PublicationCode = null;
            this.ucoDetails.Size = new System.Drawing.Size(740, 321);
            this.ucoDetails.TabIndex = 3;
            this.ucoDetails.VerificationResultCollection = null;

            //
            // TUCPartnerSubscriptions
            //
            this.Controls.Add(this.ucoDetails);
            this.Controls.Add(this.splSubscriptions);
            this.Controls.Add(this.pnlSubscriptionList);
            this.Name = "TUCPartnerSubscriptions";
            this.Size = new System.Drawing.Size(740, 440);
            this.pnlSubscriptionList.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Raises Event EnableDisableOtherScreenParts.
        ///
        /// </summary>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        protected void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        /// <summary>
        /// Raises Event HookupDataChange.
        ///
        /// </summary>
        /// <param name="e">Event parameters (nothing in particluar for this Event)
        /// </param>
        /// <returns>void</returns>
        protected void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void SwitchDetailReadOnlyModeOrEditMode()
        {
            TDataModeEnum FDataModeEnum;

            if (!(FIsEditingRecord))
            {
                // mode for editing.
                FIsEditingRecord = true;
                FDataModeEnum = TDataModeEnum.dmEdit;
                ucoDetails.SetMode(FDataModeEnum);
                this.btnNewRecord.Enabled = false;
                this.btnEditRecord.Text = "      " + CommonResourcestrings.StrBtnTextDone;
                this.btnDeleteRecord.Text = "     " + CommonResourcestrings.StrBtnTextCancel;
                this.btnDeleteRecord.ImageIndex = 4;
                this.btnEditRecord.ImageIndex = 3;
                this.btnDeleteRecord.Enabled = true;
                this.btnEditRecord.Enabled = true;

// TODO                tbrActions.Enabled = false;
                this.grdRecordList.Enabled = false;
                ucoDetails.EnableDisableIssuesButton(true);

                if (!FNewSubscription)
                {
                    ucoDetails.EditModeForSubscription();
                }
            }
            else
            {
                // mode after editing
                FIsEditingRecord = false;
                FNewSubscription = false;
                FDataModeEnum = TDataModeEnum.dmBrowse;
                ucoDetails.SetMode(FDataModeEnum);
                this.btnDeleteRecord.Text = "     " + CommonResourcestrings.StrBtnTextDelete;
                this.btnEditRecord.Text = "       " + CommonResourcestrings.StrBtnTextEdit;
                this.btnDeleteRecord.ImageIndex = 2;
                this.btnEditRecord.ImageIndex = 1;
                this.btnNewRecord.Enabled = true;

// TODO                tbrActions.Enabled = true;
                this.grdRecordList.Enabled = true;
            }

            SetButtonsFirstTime();
            ApplySecurity();
        }

        public void AdjustLabelControlsAfterResizing()
        {
            ucoDetails.AdjustLabelControlsAfterResizing();
            SetupDataGridVisualAppearance();
        }

        public void HandleDisabledControlClick(System.Object sender, System.EventArgs e)
        {
            BtnEditRecord_Click(this, null);
        }

        /// <summary>
        /// Set the buttons when there are no Recors
        /// </summary>
        /// <returns>void</returns>
        private void SetButtonsFirstTime()
        {
            if (ucoDetails.DataviewIsEmpty() && (grdRecordList.Rows.Count < 2))
            {
                this.btnEditRecord.Enabled = false;
                this.btnDeleteRecord.Enabled = false;

// TODO                tbrActions.Enabled = false;
                ucoDetails.MakeScreenInvisible(false);
            }
            else
            {
                ucoDetails.MakeScreenInvisible(true);
                ucoDetails.SetPublicationCostFirstTime();
            }
        }

        #endregion

        #region TUCPartnerSubscriptions

        #region Initialise Subscriptions Tab


        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        public new void InitialiseUserControl()
        {
            /*
             * For some reason WinForms (or XPMenu?) doesn't automatically scale the
             * Button Widhts if a DPI resolution other than the standard one (96 DPI
             * ['Small Fonts']) is used, eg. 120 DPI ['Large Fonts']. (Note: The Font of
             * the Buttons is automatically scaled, but then cut off on the Button's right
             * edge).
             */
            if ((TClientSettings.GUIRunningOnNonStandardDPI)
                || (this.ParentForm.AutoScaleBaseSize.Width != PetraForm.AUTOSCALEBASESIZEWIDTHFOR96DPI))
            {
                tbbActions.Width =
                    Convert.ToInt32(System.Decimal.Round(Convert.ToDecimal(tbbActions.Width *
                                (this.ParentForm.AutoScaleBaseSize.Width / PetraForm.AUTOSCALEBASESIZEWIDTHFOR96DPI)))) + 14;                                                                                                   /* The '14' here gives the
                                                                                                                                                                                                                                 * *most accurate
                                                                                                                                                                                                                                 * *representation of the
                                                                                                                                                                                                                                 * *Width compared to the
                                                                                                                                                                                                                                 *Width with 96 DPI... */
            }

            // Set up screen logic
            FLogic.MultiTableDS = FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.LoadPublications();
            FLogic.LoadDataOnDemand();
            FLastPublicationCode = "";             /// just make it something that is not a LocationKey that will be in the screen...

            // Create temp table for grid display
            FLogic.CreateTempSubscriptionsTable();
            FLogic.FillTempSubscriptionsTable();

            // Create SourceDataGrid columns
            FLogic.CreateColumns(grdRecordList);

            // DataBindingrelated stuff
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

            // Initialise dependent UserControl
            ucoDetails.VerificationResultCollection = FVerificationResultCollection;
            ucoDetails.DisabledControlClickHandler = @HandleDisabledControlClick;
            ucoDetails.

            /*
             * Until we can do New/Edit/Delete in the Detail section, we don't show the
             * New and Delete Button.
             */
            btnNewRecord.Visible = true;
            btnDeleteRecord.Visible = true;

            // Extender Provider
            this.expStringLengthCheckSubscriptions.RetrieveTextboxes(this);
            this.SetButtonsFirstTime();

            // Hook up the the datachange events.
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpSubscriptions));
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid. Also selects the Row containing the
        /// 'Best Address'.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridDataBinding()
        {
            Int32 BestPublicationRowNumber;
            String BestPublicationPublicationCode;

            FLogic.DataBindGrid(grdRecordList);

            // Hook up event that fires when a different Row is selected
            grdRecordList.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);

            // Determine the Row that should be initially selected
            FLogic.DetermineInitiallySelectedPublication(grdRecordList, out BestPublicationRowNumber, out BestPublicationPublicationCode);
            FLogic.PublicationCode = BestPublicationPublicationCode;

            // Select Row that should be initially selected
            grdRecordList.Selection.SelectRow(BestPublicationRowNumber, true);

            // DataBind Detail UserControl
            ucoDetails.PerformDataBinding(FMainDS, BestPublicationPublicationCode);

            // This looks stupid, but gets the Details cleared in case the Partner has no Subscriptions (yet)...
            DataGrid_FocusRowEntered(this, new RowEventArgs(2));
            DataGrid_FocusRowEntered(this, new RowEventArgs(BestPublicationRowNumber));
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
                grdRecordList.AutoSizeCells();
                grdRecordList.Width = btnNewRecord.Left - grdRecordList.Left - 6;                 /// it is necessary to reassign the width because the columns don't take up the maximum width
            }
        }

        #endregion


        #region Major Actions

        protected void ActionCancelOrDelete()
        {
            if (btnDeleteRecord.Enabled)
            {
                if (!FIsEditingRecord)
                {
                    // Deleting a record
                    ActionDeleteRecord();
                }
                else
                {
                    // Cancel Edit
                    ActionCancelCurrentEdit();
                }
            }
        }

        /// <summary>
        ///
        /// Delete the Currently Selected Record
        /// </summary>
        /// <returns>void</returns>
        protected void ActionDeleteRecord()
        {
            Int32 NewlySelectedRow;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            Int64 tmpPartnerKey;
            String tmpPublicationCode;

            // Deleting a record

            // Ensure that following Selection.ActivePosition inquiries will work!
            grdRecordList.Focus();

            // get the primarykey of selected subscription
            tmpPartnerKey = FLogic.DetermineCurrentPartnerKey();
            tmpPublicationCode = FLogic.DetermineCurrentPublicationCode(grdRecordList);

            // try to delete the subscription, if succes, returns true
            if (FLogic.DeleteRecord(tmpPublicationCode, tmpPartnerKey))
            {
                // delete the subscription from datagrid.
                FLogic.AllowDelete(this.grdRecordList, true);
                ((DataRowView) this.grdRecordList.SelectedDataRows[0]).Delete();
                FLogic.AllowDelete(this.grdRecordList, false);
                this.grdRecordList.Refresh();

                if (grdRecordList.Selection.ActivePosition != Position.Empty)
                {
                    NewlySelectedRow = grdRecordList.Selection.ActivePosition.Row;
                }
                else
                {
                    NewlySelectedRow = grdRecordList.Rows.Count - 1;
                }

                // if there is after deletion Subscriptions, set focus to one.
                if (grdRecordList.Rows.Count > 1)
                {
                    grdRecordList.Selection.ResetSelection(false);
                    grdRecordList.Selection.SelectRow(NewlySelectedRow, true);
                    grdRecordList.ShowCell(new Position(NewlySelectedRow - 1, 0), true);
                }
                else
                {
                    // actions to do when deletes the last Row! Here the UC_PartnerSubscription is hidded.
                    SetButtonsFirstTime();
                }

                // Fire OnRecalculateScreenParts event
                RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
            }
        }

        /// <summary>
        /// Cancel the current Edit in detail view
        /// </summary>
        /// <returns>void</returns>
        protected void ActionCancelCurrentEdit()
        {
            Int32 NewlySelectedRow;
            TEnableDisableEventArgs EnableDisableEventArgs;

            // Deleting a record

            // Ensure that following Selection.ActivePosition inquiries will work!
            grdRecordList.Focus();

            if (grdRecordList.Selection.ActivePosition != Position.Empty)
            {
                NewlySelectedRow = grdRecordList.Selection.ActivePosition.Row;
            }
            else
            {
                NewlySelectedRow = grdRecordList.Rows.Count - 1;
            }

            //MessageBox.Show("Newly selected Row: " + NewlySelectedRow.ToString()); //

            bool IsBeingAdded = false;

            if (FLogic.IsRecordBeingAdded3)
            {
                // New Row, so delete it:
                ucoDetails.CancelEditing(true, false);
                IsBeingAdded = true;

                //next line is causing issue
                //moved to AFTER other stuff
                //Flogic.RemoveLastRowAdded()
            }
            else
            {
                // existing Row, so roll back changes
                ucoDetails.CancelEditing(true, false);
            }

            // Clear any errors that might have been set. ( these don't matter for the changes have been cancelled)
            FVerificationResultCollection.Remove(ucoDetails);

            // Tell FLogic that we are no longer adding a Location
            FLogic.IsRecordBeingAdded3 = false;

            // Fire OnEnableDisableOtherScreenParts event
            EnableDisableEventArgs = new TEnableDisableEventArgs();
            EnableDisableEventArgs.Enable = true;
            OnEnableDisableOtherScreenParts(EnableDisableEventArgs);
            grdRecordList.Refresh();

            // set the FnewSubscriptionFlag to false.
            FNewSubscription = false;

            // set buttons
            this.SwitchDetailReadOnlyModeOrEditMode();

            // set focus to datagrid.
            grdRecordList.Focus();
            grdRecordList.Selection.ResetSelection(false);
            grdRecordList.Selection.SelectRow(NewlySelectedRow, true);

            // grdRecordList.ShowCell(Position.Create(NewlySelectedRow  1, 0));
            DataGrid_FocusRowEntered(this, new RowEventArgs(NewlySelectedRow));
            SetButtonsFirstTime();

            if (IsBeingAdded == true)
            {
                FLogic.RemoveLastRowAdded();
            }
        }

        /// <summary>
        /// Either edits the currently selected record or ends the editing of the
        /// currently selected record.
        /// </summary>
        /// <returns>void</returns>
        protected void ActionEditRecord()
        {
            PSubscriptionRow ErroneousRow;
            String ErrorMessages;
            Control FirstErrorControl;
            TEnableDisableEventArgs EnableDisableEventArgs;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            String tmpPublicationCode;

            tmpPublicationCode = "";

            if (btnEditRecord.Enabled)
            {
                if (!FIsEditingRecord)
                {
                    //
                    // Record should be edited
                    //
                    this.SwitchDetailReadOnlyModeOrEditMode();

                    // sets the Flogics identifier flag of Publication code to currently selected publication.
                    FLogic.DetermineCurrentPublicationCode(grdRecordList);

                    // sets the Row to beginedit mode.
                    FLogic.EditRecord();

                    // set focus to UserControl
                    ucoDetails.Focus();

                    // Fire OnEnableDisableOtherScreenParts event
                    EnableDisableEventArgs = new TEnableDisableEventArgs();
                    EnableDisableEventArgs.Enable = false;
                    OnEnableDisableOtherScreenParts(EnableDisableEventArgs);
                    return;
                }
                else
                {
                    //
                    // New Record or end of editing an existing record..
                    //
                    tmpPublicationCode = ucoDetails.GetSelectedPublicationcode();

                    // If all data is correct in individual textboxes etc.
                    if (!FVerificationResultCollection.Contains(ucoDetails))
                    {
                        // Data Validation OK
                        // here is checked that data is logically in correct order. Example:
                        // if Subscriptionstatus is 'GIFT' there must be also a Partner that gives the gift
                        if (ucoDetails.CheckCorrectnessOfValues())
                        {
                            // Finally must be checked that the publication selected does not already exist.
                            // This cannot be done at verificationfile, because it does not have the DataBase available there.
                            if (this.FNewSubscription && (!FLogic.PublicationAlreadyExcist(ucoDetails.GetSelectedPublicationcode())))
                            {
                            }
                            // if the subscription is new and the publication excist, no action will be done.
                            else
                            {
                                // if the Flogic does't already have the current publication code, set it
                                // If all Rows are deleted, this happens.
                                // Will work without this (tested), but it is best to do this anyway.
                                if (FLogic.PublicationCode == null)
                                {
                                    FLogic.PublicationCode = ucoDetails.GetSelectedPublicationcode();
                                }

                                FLogic.ProcessEditedRecord(out ErroneousRow);
                                this.GridRefresh(tmpPublicationCode);
                                this.SwitchDetailReadOnlyModeOrEditMode();

                                // Fire OnEnableDisableOtherScreenParts event
                                EnableDisableEventArgs = new TEnableDisableEventArgs();
                                EnableDisableEventArgs.Enable = true;
                                OnEnableDisableOtherScreenParts(EnableDisableEventArgs);

                                // Fire OnRecalculateScreenParts event
                                RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                                RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                                OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                                // Make the Grid respond on updown keys
                                grdRecordList.Focus();
                            }
                        }
                    }
                    else
                    {
                        // Data Validation failed
                        FVerificationResultCollection.BuildScreenVerificationResultList(ucoDetails, out ErrorMessages, out FirstErrorControl);

                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(StrInvalidDataNotCorrected + "" + Environment.NewLine + Environment.NewLine + ErrorMessages,
                            "Record contains invalid data!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        FirstErrorControl.Focus();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new record.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionNewRecord()
        {
            Int64 newPartnerKey;
            String newPublicationCode;

            System.Windows.Forms.DialogResult DialogResult;
            TEnableDisableEventArgs EnableDisableEventArgs;

            // First is checked, that the Partner open is not 'PERSON'. If is, warning below is given.
            DialogResult = System.Windows.Forms.DialogResult.Yes;

            if (FMainDS.PPartner[0].PartnerClass == StrPartnerClassPerson)
            {
                DialogResult = MessageBox.Show(StrSubscriptionPerson,
                    StrSubscriptionPersonTitle,
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
            }

            if ((this.btnNewRecord.Enabled) && (DialogResult == System.Windows.Forms.DialogResult.Yes))
            {
                // determine new primarykey pair for new Subscription. These will be used as base values
                // for the Subscription.
                FLogic.DetermineNewPrimaryKeys(out newPartnerKey, out newPublicationCode);
                FLogic.PublicationCode = newPublicationCode;
                FLastPublicationCode = newPublicationCode;

                // add the new Subscription to database
                FLogic.CreateNewSubscriptionRow(newPartnerKey, newPublicationCode);

                // set the Publicationcode to UserControl
                ucoDetails.PublicationCode = newPublicationCode;
                FNewSubscription = true;

                // Set the buttons.
                this.SwitchDetailReadOnlyModeOrEditMode();

                // Set screenparts to default for new Subscription
                ucoDetails.SetScreenPartsForNewSubscription();
                EnableDisableEventArgs = new TEnableDisableEventArgs();
                EnableDisableEventArgs.Enable = false;
                OnEnableDisableOtherScreenParts(EnableDisableEventArgs);
            }
        }

        /// <summary>
        /// Grid refrehment after new Record is added or existing is edited.
        /// </summary>
        /// <returns>void</returns>
        private void GridRefresh(String tmpPublicationCode)
        {
            DataTable tmpDT;
            DataRowView TmpDataRowView;
            Int32 TmpRowIndex;

            if (this.FNewSubscription)
            {
                tmpDT = ucoDetails.GetSubscriptionDV();
                FLogic.RefreshDataGrid(grdRecordList, tmpDT, true);
            }
            else
            {
                FLogic.RefreshDataGridEdit(grdRecordList, ucoDetails.GetSelectedPublicationcode(),
                    ucoDetails.GetCurrentSubscriptionStatus(), ucoDetails.IsFreeSubscription());
            }

            if (grdRecordList.DataSource.Count > 1)
            {
                TmpDataRowView = FLogic.DetermineRecordToSelect((grdRecordList.DataSource as DevAge.ComponentModel.BoundDataView).DataView,
                    tmpPublicationCode);
                TmpRowIndex = grdRecordList.Rows.DataSourceRowToIndex(TmpDataRowView);
                grdRecordList.Selection.ResetSelection(false);
                grdRecordList.Selection.SelectRow(TmpRowIndex + 1, true);

                // Scroll grid to line where the new record is now displayed
                grdRecordList.ShowCell(new Position(TmpRowIndex + 1, 0), true);
                grdRecordList.Focus();
                grdRecordList.Refresh();
            }
        }

        /// <summary>
        /// Applies Petra Security to restrict functionality, if needed.
        /// </summary>
        /// <returns>void</returns>
        protected void ApplySecurity()
        {
            // Check security for New record operations
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PSubscriptionTable.GetTableDBName()))
            {
                btnNewRecord.Enabled = false;
            }

            // Check security for Edit record operations
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PSubscriptionTable.GetTableDBName()))
            {
                btnEditRecord.Enabled = false;

// TODO                tbrActions.Enabled = false;
            }

            // Check security for Delete record operations
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapDELETE, PSubscriptionTable.GetTableDBName()))
            {
                btnDeleteRecord.Enabled = false;
            }
        }

        #endregion


        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
            Rectangle SelectedTabHeaderRectangle;

            // Calculate where the middle of the TabHeader of the Tab lies at this moment
            SelectedTabHeaderRectangle = ((TTabVersatile) this.Parent.Parent.Parent).SelectedTabHeaderRectangle;
            pnlBalloonTipAnchor.Left = SelectedTabHeaderRectangle.X + Convert.ToInt16(SelectedTabHeaderRectangle.Width / 2.0);

            // Check for BalloonTips to display, and show them
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) == '!')
            {
                System.Threading.ThreadPool.QueueUserWorkItem(@TThreaded.ThreadedCheckForUserTips, pnlBalloonTipAnchor);
            }
            else if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewCancelAllSubscriptions) == '!')
            {
                System.Threading.ThreadPool.QueueUserWorkItem(@TThreaded.ThreadedCheckForUserTips, tbrActions);
            }
        }

        /// <summary>
        /// Default Constructor.
        ///
        /// Define the screen's logic.
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerSubscriptions() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // define the screen's logic
            FLogic = new TUCPartnerSubscriptionsLogic();
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

        public Boolean PerformCancelAllSubscriptions(DateTime ACancelDate)
        {
            Boolean ReturnValue = false;

// TODO PerformCancelAllSubscriptions
#if TODO
            TPartnerSubscCancelAllWinForm Scd;
            string ReasonEnded;
            DateTime DateEnded;
            ArrayList SubscrCancelled;
            int UpdateCounter;
            DataRowView TmpDataRowView;
            int TmpRowIndex;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            String SubscrCancelledString = "";

            ReturnValue = true;

            // Check whether there are any Subscriptions that can be cancelled
            if (FLogic.CancelAllSubscriptionsCount() > 0)
            {
                // Open 'Cancel All Subscriptions' Dialog
                Scd = new TPartnerSubscCancelAllWinForm();

                if (ACancelDate != DateTime.MinValue)
                {
                    Scd.DateEnded = ACancelDate;
                }

                Scd.ShowDialog();

                if (Scd.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    // Get values from the Dialog
                    Scd.GetReturnedParameters(out ReasonEnded, out DateEnded);

                    // Cancel the Subscriptions
                    SubscrCancelled = FLogic.CancelAllSubscriptions(ReasonEnded, DateEnded);

                    /*
                     * Update the Grid and the Detail UserControl to reflect the changes in the
                     * records.
                     * Build a String to tell the user what Subscriptions were cancelled.
                     */
                    for (UpdateCounter = 0; UpdateCounter <= SubscrCancelled.Count - 1; UpdateCounter += 1)
                    {
                        // Raise a FocusRowEntered Event for the Subscription with a certain
                        // Publication Code  this updates the Detail UserControl
                        TmpDataRowView = FLogic.DetermineRecordToSelect((grdRecordList.DataSource as DevAge.ComponentModel.BoundDataView).DataView,
                            SubscrCancelled[UpdateCounter].ToString());
                        TmpRowIndex = grdRecordList.Rows.DataSourceRowToIndex(TmpDataRowView);
                        DataGrid_FocusRowEntered(this, new RowEventArgs(TmpRowIndex + 1));

                        // Now refresh the Grid Row with the changed data
                        // (Subscription Status has changed to 'CANCELED')
                        FLogic.RefreshDataGridEdit(grdRecordList, ucoDetails.GetSelectedPublicationcode(),
                            ucoDetails.GetCurrentSubscriptionStatus(), ucoDetails.IsFreeSubscription());
                        SubscrCancelledString = SubscrCancelledString + "   " + SubscrCancelled[UpdateCounter].ToString() + Environment.NewLine;
                    }

                    // Finally, select the first record in the Grid and update the Detail
                    // UserControl (this one might have been Canceled)
                    grdRecordList.Selection.ResetSelection(false);
                    grdRecordList.Selection.SelectRow(1, true);
                    DataGrid_FocusRowEntered(this, new RowEventArgs(1));

                    // Make sure the Detail UserControl is updated, even if the Row wasn't
                    // changed because there was only one Row...
                    ucoDetails.SetMode(TDataModeEnum.dmBrowse);

                    // Fire OnRecalculateScreenParts event to update the Tab Counters
                    RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                    RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                    OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                    // Tell the user that cancelling of Subscriptions was succesful
                    MessageBox.Show(String.Format(StrCancelAllSubscriptionsDone,
                            SubscrCancelled.Count,
                            SubscrCancelledString), StrCancelAllSubscriptionsDoneTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // User pressed Cancel in the Dialog. Tell the user that nothing was done.
                    MessageBox.Show(StrCancelAllSubscriptionsCanceled,
                        StrCancelAllSubscriptionsTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ReturnValue = false;
                }

                Scd.Dispose();
            }
            else
            {
                // Tell the user that there are no Subscriptions that can be canceled.
                MessageBox.Show(StrCancelAllSubscriptionsNone, StrCancelAllSubscriptionsTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
#endif
            return ReturnValue;
        }

        private void MniCancelAllSubscriptions_Click(System.Object sender, System.EventArgs e)
        {
            PerformCancelAllSubscriptions(DateTime.MinValue);
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnNewRecord_Click(System.Object sender, System.EventArgs e)
        {
            this.ActionNewRecord();
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnDeleteRecord_Click(System.Object sender, System.EventArgs e)
        {
            this.ActionCancelOrDelete();
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnResizeSubscriptionList_Click(System.Object sender, System.EventArgs e)
        {
            if (!FGridMaximised)
            {
                FGridMaximised = true;
                FGridMinimisedSize = pnlSubscriptionList.Height;
                pnlSubscriptionList.Height = 319;
            }
            else
            {
                FGridMaximised = false;
                pnlSubscriptionList.Height = FGridMinimisedSize;
            }
        }

        public DataRow GetSelectedDataRow()
        {
            String APublicationCode;

            APublicationCode = FLogic.DetermineCurrentPublicationCode(this.grdRecordList);
            return FLogic.GetSelectedDataRow(APublicationCode);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                BtnDeleteRecord_Click(this, null);
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            ActionEditRecord();
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            this.BtnEditRecord_Click(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            ActionNewRecord();
        }

        /// <summary>
        /// Raises Event RecalculateScreenParts.
        ///
        /// </summary>
        /// <param name="e">Event parameters
        /// </param>
        /// <returns>void</returns>
        protected void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnEditRecord_Click(System.Object sender, System.EventArgs e)
        {
            this.ActionEditRecord();
        }

        /// <summary>
        /// Default WinForms function, created by the Designer
        ///
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

        public void InitialiseDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            ucoDetails.InitialiseDelegateGetPartnerShortName(ADelegateFunction);
        }

        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateIsNewPartner = ADelegateFunction;
        }

        /// <summary>
        /// Event Handler for FocusRowEntered Event of the Grid
        ///
        /// </summary>
        /// <param name="ASender">The Grid</param>
        /// <param name="AEventArgs">RowEventArgs as specified by the Grid (use Row property to
        /// get the Grid Row for which this Event fires)
        /// </param>
        /// <returns>void</returns>
        protected void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            // check, if edit mode, and continue only if is not
            // if is edit mode, run the verification. and if that is ok, go on.
            // If there is no Publications, edit button is disabled
            if (FLogic.DetermineCurrentPublicationCode(grdRecordList, AEventArgs.Row) == "")
            {
                btnEditRecord.Enabled = false;
                btnDeleteRecord.Enabled = false;

// TODO                tbrActions.Enabled = false;
            }
            else
            {
                btnEditRecord.Enabled = true;
                btnDeleteRecord.Enabled = true;
                btnNewRecord.Enabled = true;

// TODO                tbrActions.Enabled = true;
            }

            ApplySecurity();

            // (this Event also fires if the user just (double)clicks on the same record again)
            if (FLogic.PublicationCode != FLastPublicationCode)
            {
                ucoDetails.PublicationCode = FLogic.DetermineCurrentPublicationCode(grdRecordList, AEventArgs.Row);

                // (this Event also fires if the user just (double)clicks on the same record again)
                this.btnEditRecord.Text = "       " + CommonResourcestrings.StrBtnTextEdit;
                this.btnNewRecord.Text = "       " + CommonResourcestrings.StrBtnTextNew;

                // filter display of Address detail section
                ucoDetails.SetMode(TDataModeEnum.dmBrowse);
                this.FdataEditOn = false;
                FLastPublicationCode = ucoDetails.GetSelectedPublicationcode();
            }
        }

        #endregion
    }

    public class TThreadedSubscriptions : System.Object
    {
// TODO balloon
#if TODO
        private static TBalloonTip UBalloonTip;

        public static void PrepareBalloonTip()
        {
            // Ensure that we have an instance of TBalloonTip
            if (UBalloonTip == null)
            {
                UBalloonTip = new TBalloonTip();
            }
        }

        public static void ThreadedCheckForUserTips(System.Object AControl)
        {
            /*
             * Check for specific TipStatus: '!' means 'show the Tip, but then set it'
             *
             * This is done to pick up Tips only for the Users that they were specifically
             * set for, and not for users where the TipStatus would be un-set ('-')!
             * This is helfpful eg. for Patch Notices - they should be shown to all current
             * users (the patch program would set the TipStatus to '!' for all current users),
             * but not for users that get created after the Patch was applied (they don't
             * need to know what was new/has changed in a certain Patch that was applied in
             * the past!)
             */
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) == '!')
            {
                // Set Tip Status so it doesn't get picked up again!
                TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersSubscriptions);
                PrepareBalloonTip();
                UBalloonTip.ShowBalloonTipNewFunction(
                    "Petra Version 2.2.7 Change: Counter in Subscriptions Tab Header",
                    "The number in the tab header no longer shows just the number of records - from now on it takes" + "\r\n" +
                    "only active Subscriptions (non-CANCELLED and non-EXPIRED Subscriptions) into account." + "\r\n" +
                    "The Addresses tab and Notes tab also have changes to the counters in the tab headers.",
                    (Control)AControl);
                UBalloonTip = null;

                // Dont' show any more Tips in this instance of the Partner Edit screen
                return;
            }

            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewCancelAllSubscriptions) == '!')
            {
                // Set Tip Status so it doesn't get picked up again!
                TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewCancelAllSubscriptions);
                PrepareBalloonTip();
                UBalloonTip.ShowBalloonTipNewFunction(
                    "Petra Version 2.2.7 Change: Cancel All Subscriptions",
                    "This function allows canceling of all active Subscriptions in one go." + "\r\n" +
                    "A Dialog allows specification of 'Reason Ended' and 'Date Ended'." + "\r\n" +
                    "Choose 'Cancel All Subscriptions...' after clicking this button to use" + "\r\n" + "this new functionality.",
                    (Control)AControl);
                UBalloonTip = null;

                // Dont' show any more Tips in this instance of the Partner Edit screen
                return;
            }
        }
#endif

    }
}