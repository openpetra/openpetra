/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       martaj
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
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;
using SourceGrid;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using System.Globalization;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for editing Partner Interest (List/Detail view).
    ///
    /// Consists of a Grid that contains a list of records, three buttons that allow
    /// manipulation of the records and a Detail UserControl to show and edit the
    /// details of the selected record.
    ///
    /// @Comment At the moment it simply hosts UC_PartnerInterest. In the end this
    ///   UserControl will be working like the Addresses Tab and Subscriptions Tab:
    ///   a Grid with 0..n Rows of data and Add/Edit/Delete buttons will be at the
    ///   top. The UC_PartnerInterest UserControl below will allow displaying and
    ///   editing of detail data for each Row that is selected in the Grid.
    /// </summary>
    public class TUCPartnerInterests : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private TSgrdDataGrid grdRecordList;
        private System.Windows.Forms.Panel pnlInterestList;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Button btnResizeInterestList;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnEditRecord;
        private System.Windows.Forms.Button btnNewRecord;
        private System.Windows.Forms.Splitter splInterests;

        /// <summary>Detail UserControl to show and edit the details of the selected record</summary>
        private TUCPartnerInterest ucoDetails;
        private System.Windows.Forms.ToolTip tipMain;
        private TexpTextBoxStringLengthCheck expStringLengthCheckInterests;

        /// <summary>Object that holds the logic for this screen</summary>
        protected TUCPartnerInterestsLogic FLogic;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        protected IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        protected new PartnerEditTDS FMainDS;

        /// <summary>tells whether the Grid has been expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Boolean FGridMaximised;

        /// <summary>holds the height of the Grid before it got expanded vertically with btnMaximiseMinimiseGrid</summary>
        protected Int32 FGridMinimisedSize;

        /// <summary>this is for Extracts use, should be removed when Extract? screen is working independently.</summary>
        protected TDelegateIsNewPartner FDelegateIsNewPartner;

        /// <summary>tells whether the currently selected record is being edited, or not</summary>
        protected Boolean FIsEditingRecord;

        /// <summary>tells whether the Currrently edited Interest is new, or not</summary>
        protected Boolean FdataEditOn;
        protected Boolean FnewInterest;

        /// <summary>holds part of the Interest Key of the Interest record that was last selected in the Grid</summary>
        protected String FLastInterest;
        protected String FCategory;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUCPartnerInterests));
            this.components = new System.ComponentModel.Container();
            this.splInterests = new System.Windows.Forms.Splitter();
            this.pnlInterestList = new System.Windows.Forms.Panel();
            this.btnResizeInterestList = new System.Windows.Forms.Button();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnEditRecord = new System.Windows.Forms.Button();
            this.btnNewRecord = new System.Windows.Forms.Button();
            this.grdRecordList = new Ict.Common.Controls.TSgrdDataGrid();
            this.ucoDetails = new Ict.Petra.Client.MPartner.TUCPartnerInterest();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.expStringLengthCheckInterests = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlInterestList.SuspendLayout();
            this.SuspendLayout();

            //
            // splInterests
            //
            this.splInterests.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splInterests.Dock = System.Windows.Forms.DockStyle.Top;
            this.splInterests.Location = new System.Drawing.Point(0, 115);
            this.splInterests.MinExtra = 53;
            this.splInterests.MinSize = 52;
            this.splInterests.Name = "splInterests";
            this.splInterests.Size = new System.Drawing.Size(740, 4);
            this.splInterests.TabIndex = 4;
            this.splInterests.TabStop = false;

            //
            // pnlInterestList
            //
            this.pnlInterestList.Controls.Add(this.btnResizeInterestList);
            this.pnlInterestList.Controls.Add(this.btnDeleteRecord);
            this.pnlInterestList.Controls.Add(this.btnEditRecord);
            this.pnlInterestList.Controls.Add(this.btnNewRecord);
            this.pnlInterestList.Controls.Add(this.grdRecordList);
            this.pnlInterestList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInterestList.ForeColor = System.Drawing.SystemColors.Window;
            this.pnlInterestList.Location = new System.Drawing.Point(0, 0);
            this.pnlInterestList.Name = "pnlInterestList";
            this.pnlInterestList.Size = new System.Drawing.Size(740, 115);
            this.pnlInterestList.TabIndex = 3;

            //
            // btnResizeInterestList
            //
            this.btnResizeInterestList.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnResizeInterestList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResizeInterestList.ImageIndex = 5;
            this.btnResizeInterestList.ImageList = this.imlButtonIcons;
            this.btnResizeInterestList.Location = new System.Drawing.Point(659, 93);
            this.btnResizeInterestList.Name = "btnResizeInterestList";
            this.btnResizeInterestList.Size = new System.Drawing.Size(20, 18);
            this.btnResizeInterestList.TabIndex = 8;
            this.tipMain.SetToolTip(this.btnResizeInterestList, "Make List higher/smal" + "ler");
            this.btnResizeInterestList.Click += new System.EventHandler(this.BtnResizeInterestList_Click);

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
            this.btnDeleteRecord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnDeleteRecord.ImageIndex = 2;
            this.btnDeleteRecord.ImageList = this.imlButtonIcons;
            this.btnDeleteRecord.Location = new System.Drawing.Point(664, 58);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(76, 23);
            move SetStatusBarText to constructor
            split designer file
            this.SetStatusBarText(this.btnDeleteRecord, "Delete current" + "ly selected Interest");
            this.btnDeleteRecord.TabIndex = 7;
            this.btnDeleteRecord.Text = "     &Delete";
            this.btnDeleteRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteRecord.Click += new System.EventHandler(this.BtnDeleteRecord_Click);

            //
            // btnEditRecord
            //
            this.btnEditRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnEditRecord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnEditRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnEditRecord.ImageIndex = 1;
            this.btnEditRecord.ImageList = this.imlButtonIcons;
            this.btnEditRecord.Location = new System.Drawing.Point(664, 34);
            this.btnEditRecord.Name = "btnEditRecord";
            this.btnEditRecord.Size = new System.Drawing.Size(76, 23);
            this.btnEditRecord.TabIndex = 6;
            this.btnEditRecord.Text = "       Edi&t";
            this.btnEditRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditRecord.Click += new System.EventHandler(this.BtnEditRecord_Click);

            //
            // btnNewRecord
            //
            this.btnNewRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnNewRecord.BackColor = System.Drawing.SystemColors.Control;
            this.btnNewRecord.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnNewRecord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNewRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnNewRecord.ImageIndex = 0;
            this.btnNewRecord.ImageList = this.imlButtonIcons;
            this.btnNewRecord.Location = new System.Drawing.Point(664, 10);
            this.btnNewRecord.Name = "btnNewRecord";
            this.btnNewRecord.Size = new System.Drawing.Size(76, 23);
            this.btnNewRecord.TabIndex = 5;
            this.btnNewRecord.Text = "       &New";
            this.btnNewRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewRecord.Click += new System.EventHandler(this.BtnNewRecord_Click);

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
            this.grdRecordList.DeleteQuestionMessage = "You have chosen to delete thi" + "s record.'#13#10#13#10'Do you really want to delete it?";
            this.grdRecordList.FixedRows = 1;
            this.grdRecordList.ForeColor = System.Drawing.SystemColors.ControlText;
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
            this.grdRecordList.TabIndex = 0;
            this.grdRecordList.TabStop = true;
            this.grdRecordList.DoubleClickCell += new TDoubleClickCellEventHandler(this.GrdRecordList_DoubleClickCell);

            //
            // ucoDetails
            //
            this.ucoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucoDetails.Interest = null;
            this.ucoDetails.Location = new System.Drawing.Point(0, 119);
            this.ucoDetails.MainDS = null;
            this.ucoDetails.Name = "ucoDetails";
            this.ucoDetails.Size = new System.Drawing.Size(740, 363);
            this.ucoDetails.TabIndex = 0;
            this.ucoDetails.VerificationResultCollection = null;
            this.ucoDetails.Load += new System.EventHandler(this.UcoDetails_Load);

            //
            // TUCPartnerInterests
            //
            this.Controls.Add(this.ucoDetails);
            this.Controls.Add(this.splInterests);
            this.Controls.Add(this.pnlInterestList);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUCPartnerInterests";
            this.Size = new System.Drawing.Size(740, 482);
            this.pnlInterestList.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Raises Event EnableDisableOtherScreenParts.
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

            if (FIsEditingRecord)
            {
                // mode after editing
                FIsEditingRecord = false;
                FnewInterest = false;
                FDataModeEnum = TDataModeEnum.dmBrowse;
                ucoDetails.SetMode(FDataModeEnum);
                this.btnDeleteRecord.Text = "      Delet&e";
                this.btnEditRecord.Text = "       Edi&t";
                this.btnDeleteRecord.ImageIndex = 2;
                this.btnEditRecord.ImageIndex = 1;
                this.btnNewRecord.Enabled = true;
                this.grdRecordList.Enabled = true;
            }
            else
            {
                // mode before editing
                FIsEditingRecord = true;
                FDataModeEnum = TDataModeEnum.dmEdit;
                ucoDetails.SetMode(FDataModeEnum);
                this.btnNewRecord.Enabled = false;
                this.btnEditRecord.Text = "       Don&e";
                this.btnDeleteRecord.Text = "      Cancel";
                this.btnDeleteRecord.ImageIndex = 4;
                this.btnEditRecord.ImageIndex = 3;
                this.btnDeleteRecord.Enabled = true;
                this.btnEditRecord.Enabled = true;
                this.grdRecordList.Enabled = false;

                if (FnewInterest)
                {
                }
                // MessageBox.Show('FnewInterest');
                else
                {
                    ucoDetails.EditModeForInterest();
                }
            }

            SetButtonsFirstTime();
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
                ucoDetails.MakeScreenInvisible(false);
            }
            else
            {
                ucoDetails.MakeScreenInvisible(true);
            }
        }

        /// <summary>
        /// mj procedure GetSelectedInterestAndCategory(var Interest: DataRow; var Category: String);
        /// Default Constructor.
        /// Defines the screen's logic.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TUCPartnerInterests() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // define the screen's logic
            FLogic = new TUCPartnerInterestsLogic();
        }

        private void UcoDetails_Load(System.Object sender, System.EventArgs e)
        {
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
            this.ActionDeleteRecord();
        }

        /// <summary>
        /// Event Handler for Button Click Event
        /// </summary>
        /// <returns>void</returns>
        private void BtnResizeInterestList_Click(System.Object sender, System.EventArgs e)
        {
            if (FGridMaximised)
            {
                FGridMaximised = false;
                pnlInterestList.Height = FGridMinimisedSize;
            }
            else
            {
                FGridMaximised = true;
                FGridMinimisedSize = pnlInterestList.Height;
                pnlInterestList.Height = 319;
            }
        }

        public DataRow GetSelectedDataRow()
        {
            String AInterest;

            AInterest = FLogic.DetermineCurrentInterest(this.grdRecordList);
            return FLogic.GetSelectedDataRow(AInterest);
        }

        /*
         * //This is code started by talk to Tim. It is not correct and probably not used.
         * procedure TUCPartnerInterests.GetSelectedInterestAndCategory(var Interest: DataRow ; var Category : String);
         * var
         * Ainterest : String;
         *
         * begin
         * AInterest := FLogic.DetermineCurrentInterest(this.grdRecordList);
         * Interest := FLogic.GetSelectedDataRow(AInterest);
         * FCategory := FLogic.GetInterestCategory(Category);
         * Category := FCategory;
         * end; */

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void GrdRecordList_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            ActionEditRecord();
        }

        /// <summary>
        /// Raises custom Event for recalculation of the Tab Header
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
        /// Either Deletes the currently selected record or Cancels the changes of the
        /// currently selected record (in the data grid????).
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionDeleteRecord()
        {
            Int32 NewlySelectedRow;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            TEnableDisableEventArgs EnableDisableEventArgs;
            Int64 tmpPartnerKey;
            Int32 tmpInterestNumber;
            String tmpInterest;
            String tpInterest;

            if (btnDeleteRecord.Enabled)
            {
                // Deleting a record
                if (!FIsEditingRecord)
                {
                    // get the primary key of selected interest
                    FLogic.DetermineCurrentPartnerInterestKey(out tmpPartnerKey, out tmpInterestNumber, out tmpInterest);
                    tpInterest = FLogic.DetermineCurrentInterest(grdRecordList);

                    // try to delete the interest, if successful, true is returned
                    // mj GetSelectedInterestAndCategory(var Interest: DataRow; var Category: String);

                    /* if (FLogic.DeleteRecord(tmpPartnerKey, tmpInterestNumber))
                     * and (tmpInterest = tpInterest) then */
                    if (FLogic.DeleteRecord(tmpPartnerKey, tmpInterestNumber))
                    {
                        // delete the interest from datagrid.
                        // AllowDelete should always be false, but to be able to delete a
                        // record from the grid it is temporary set to true.
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

                        // if the Record grid contains one or more Interests after the
                        // deletion, set the focus to one of them.
                        if (grdRecordList.Rows.Count > 1)
                        {
                            grdRecordList.Selection.ResetSelection(false);
                            grdRecordList.Selection.SelectRow(NewlySelectedRow, true);
                            grdRecordList.ShowCell(new Position(NewlySelectedRow - 1, 0), true);
                        }
                        else
                        {
                            // Actions to be taken when the last Row has been deleted. The
                            // UC_PartnerInterest is hidden here.
                            SetButtonsFirstTime();
                        }

                        // Fire OnRecalculateScreenParts event
                        RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
                        RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
                        OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);

                        // Make the Grid respond to updown keys
                        grdRecordList.Focus();
                    }
                    else
                    {
                    }

                    // MessageBox.Show('No record was deleted.');
                }
                else
                {
                    // Cancelling actions.

                    if (grdRecordList.Selection.ActivePosition != Position.Empty)
                    {
                        NewlySelectedRow = grdRecordList.Selection.ActivePosition.Row;
                    }
                    else
                    {
                        NewlySelectedRow = grdRecordList.Rows.Count - 1;
                    }

                    // removes the Row from DataView as the user has chosen to add a new
                    // interest and has therefore cancelled the action
                    ucoDetails.CancelEditing(true, FLogic.IsRecordBeingAdded3);

                    // Clear any error that might have been set. No need to keep track of the
                    // errors any longer as the changes have been cancelled
                    // mj FVerificationResultCollection.Remove(ucoDetails);
                    // Tell FLogic that we are no longer adding a Location
                    FLogic.IsRecordBeingAdded3 = false;

                    // Fire OnEnableDisableOtherScreenParts event
                    EnableDisableEventArgs = new TEnableDisableEventArgs();
                    EnableDisableEventArgs.Enable = true;
                    OnEnableDisableOtherScreenParts(EnableDisableEventArgs);
                    grdRecordList.Refresh();

                    // set the FnewInterest flag to false.
                    FnewInterest = false;

                    // set buttons
                    this.SwitchDetailReadOnlyModeOrEditMode();

                    // set focus to datagrid.
                    grdRecordList.Focus();
                    grdRecordList.Selection.ResetSelection(false);
                    grdRecordList.Selection.SelectRow(NewlySelectedRow, true);

                    // grdRecordList.ShowCell(Position.Create(NewlySelectedRow  1, 0));
                    DataGrid_FocusRowEntered(this, new RowEventArgs(NewlySelectedRow));
                }
            }
        }

        /// <summary>
        /// Either edits the currently selected record or ends the editing of the
        /// currently selected record.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionEditRecord()
        {
            PPartnerInterestRow ErroneousRow;
            TEnableDisableEventArgs EnableDisableEventArgs;
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;
            String tmpInterest;

            tmpInterest = "";
            MessageBox.Show("TUCPartnerInterests.ActionEditRecord 1");

            if (btnEditRecord.Enabled)
            {
                MessageBox.Show("TUCPartnerInterests.ActionEditRecord 2");

                if (!FIsEditingRecord)
                {
                    MessageBox.Show("TUCPartnerInterests.ActionEditRecord 3");

                    // Record should be edited
                    this.SwitchDetailReadOnlyModeOrEditMode();

                    // sets the Flogics identifier flag of Interest to currently selected interest.
                    FLogic.DetermineCurrentInterest(grdRecordList);

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
                    MessageBox.Show("TUCPartnerInterests.ActionEditRecord 4");

                    // New Record or end of editing the existing record.
                    if (grdRecordList.Selection.ActivePosition != Position.Empty)
                    {
                        tmpInterest = ucoDetails.GetSelectedInterest();
                    }

                    // Finally it must be checked that the selected interest doesn't exist
                    // already.  This cannot be done in verification file, because the
                    // DataBase is not available there.

                    /* replaced below.   if this.FnewInterest and (not FLogic.InterestAlreadyExist(
                     * ucoDetails.GetSelectedInterest)) then */

                    // if this.FnewInterest then
                    if (1 == 2)
                    {
                        //MessageBox.Show("TUCPartnerInterests.ActionEditRecord 5");

                        // no action will be taken if the interest is new for this partner
                    }
                    else
                    {
                        // if the Flogic doesn't already have the current Interest, set it!
                        // If all Rows are deleted, this happens.
                        // Will work without this (tested), but it is best to do this anyway.
                        if (FLogic.Interest == null)
                        {
                            FLogic.Interest = ucoDetails.GetSelectedInterest();
                        }

                        MessageBox.Show("new Row or and of edit: Row being edited");
                        FLogic.ProcessEditedRecord(out ErroneousRow);

                        // this.GridRefresh(tmpInterest);
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
        }

        /// <summary>
        /// Adds a new record.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void ActionNewRecord()
        {
            Int64 newPartnerKey;
            Int32 newInterestNumber;

            /// testInterestNumber : Int32;
            String newInterest;
            String newCategory;
            TEnableDisableEventArgs EnableDisableEventArgs;

            if (this.btnNewRecord.Enabled)
            {
                newPartnerKey = MainDS.PPartner[0].PartnerKey;

                // Check if this partner Key exists in PPartnerInterest table
                // Test key newPartnerKey and InterestNumber 0. InterestNumbers starts with 0.
                // testInterestNumber :=
                // If testInterestNumber = OK
                newInterestNumber = FLogic.DetermineNextInterestKey();

                // Pick up a random pair of interest and category as a base for editing
                FLogic.DetermineNextInterest(newInterestNumber, out newInterest, out newCategory);

                // If InterestNumber is >= 0, Interests exist for this partner
                // Create a new InterestNumber for the new record
                newInterestNumber = FLogic.DetermineNextInterestKey();

                // Pick up the pair of interest and category that has been chosen on screen
                FLogic.DetermineNextInterest(newInterestNumber, out newInterest, out newCategory);

                // Earlier code: Copy from Subscriptions. Below is not correct
                // FLogic.DetermineNewPrimaryKeys(newPartnerKey, newInterestNumber);
                if (newInterestNumber == 0)
                {
                    // Partner has no interests yet
                    // New record in the PartnerInterest table
                    FLogic.DetermineNextInterest(newInterestNumber, out newInterest, out newCategory);
                }
                else
                {
                    // Partner has some interests already
                }

                // set the Interest to UserControl
                ucoDetails.Interest = newInterest;
                FLogic.Interest = newInterest;
                FLastInterest = newInterest;

                // add the new Interest to data grid
                FLogic.CreateNewInterestRow(newPartnerKey, newInterestNumber, newInterest, newCategory);
                FnewInterest = true;

                // Set the buttons.
                this.SwitchDetailReadOnlyModeOrEditMode();

                // Set screenparts to default for new Interest
                ucoDetails.SetScreenPartsForNewInterest();
                EnableDisableEventArgs = new TEnableDisableEventArgs();
                EnableDisableEventArgs.Enable = false;
                OnEnableDisableOtherScreenParts(EnableDisableEventArgs);
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
        /// Applies Petra Security to restrict functionality, if needed.
        /// -------------------------------------------------------------------------------}// procedure ApplySecurity();{*------------------------------------------------------------------------------
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

        /// <summary>
        /// Initialises Delegate Functions to retrieve a partner key
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateIsNewPartner = ADelegateFunction;
        }

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        ///
        /// </summary>
        /// <returns>void</returns>
        public new void InitialiseUserControl()
        {
            // Set up screen logic
            FLogic.MultiTableDS = FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.LoadInterests();
            FLogic.LoadDataOnDemand();
            FLastInterest = "";

            // Create SourceDataGrid columns
            FLogic.CreateColumns(grdRecordList);

            // DataBindingrelated stuff
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

// TODO statusbar
#if TODO
            // Initialise dependent UserControl
            // MessageBox.Show(FVerificationResultCollection.Count.ToString);
            // MJ ucoDetails.VerificationResultCollection := FVerificationResultCollection;
            ucoDetails.StatusBarTextProvider.InstanceStatusBar = sbtUserControl.InstanceStatusBar;

            /* Below line is needed in the case where the TabPage is selected during
             * PartnerEdit form_load procedure
             * MessageBox.Show('TabSetupPartnerAddresses finished'); */
            sbtUserControl = ucoDetails.StatusBarTextProvider;
#endif

            // Until we can do New/Edit/Delete in the Detail section, we don't show the
            // New and Delete Button.
            btnNewRecord.Visible = true;
            btnDeleteRecord.Visible = true;

            // Extender Provider
            this.expStringLengthCheckInterests.RetrieveTextboxes(this);
            this.SetButtonsFirstTime();

            // Hook up the datachange events.
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpInterests));
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid. Also selects the Row containing the
        /// 'Best Address'.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridDataBinding()
        {
            Int32 BestInterestRowNumber;
            String BestInterest;

            FLogic.DataBindGrid(grdRecordList);

            // Hook up event that fires when a different Row is selected
            grdRecordList.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);

            // Determine the Row that should be initially selected
            FLogic.DetermineInitiallySelectedInterest(grdRecordList, out BestInterestRowNumber, out BestInterest);

            // BestInterest := 'DOG';
            FLogic.Interest = BestInterest;

            // Select Row that should be initially selected
            grdRecordList.Selection.SelectRow(BestInterestRowNumber, true);

            // DataBind Detail UserControl
            MessageBox.Show("interest:" + BestInterest);
            ucoDetails.PerformDataBinding(FMainDS, BestInterest);
            ucoDetails.SetMode(TDataModeEnum.dmBrowse);

            // This looks stupid, but gets the Details cleared in case the Partner has no
            // Interests (yet)...
            DataGrid_FocusRowEntered(this, new RowEventArgs(2));
            DataGrid_FocusRowEntered(this, new RowEventArgs(BestInterestRowNumber));
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridVisualAppearance()
        {
            grdRecordList.AutoSizeCells();
            grdRecordList.Width = 652;

            // it is necessary to reassign the width above because the columns don't
            // take up the maximum width
        }

        /// <summary>
        /// Grid refreshment after new Record has been added or existing has been edited.
        /// </summary>
        /// <returns>void</returns>
        private void GridRefresh(String tmpInterest)
        {
            DataTable tmpDT;
            DataRowView TmpDataRowView;
            Int32 TmpRowIndex;

            if (this.FnewInterest)
            {
                tmpDT = ucoDetails.GetPartnerInterestDT();
                FLogic.RefreshDataGrid(ref grdRecordList, tmpDT, true);
            }
            else
            {
            }

            // MJ?? FLogic.RefreshDataGridEDit(grdRecordList,ucoDetails.GetCurrentInterestStatus,ucoDetails.isFreeSubscription);
            // MJ??  FLogic.RefreshDataGridEDit(grdRecordList,ucoDetails.GetSelectedInterest);
            if (grdRecordList.DataSource.Count > 1)
            {
                TmpDataRowView = FLogic.DetermineRecordToSelect((grdRecordList.DataSource as DevAge.ComponentModel.BoundDataView).DataView,
                    tmpInterest);
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
        /// Event Handler for FocusRowEntered Event of the Grid
        /// </summary>
        /// <param name="ASender">The Grid</param>
        /// <param name="AEventArgs">RowEventArgs as specified by the Grid (use Row property
        /// to get the Grid Row for which this Event fires)
        /// </param>
        /// <returns>void</returns>
        protected void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            // check if it is edit mode and continue only if it is not.
            // If there is no Interest, edit button is disabled
            if (FLogic.DetermineCurrentInterest(grdRecordList, AEventArgs.Row) == "")
            {
                btnEditRecord.Enabled = false;
                btnDeleteRecord.Enabled = false;
            }
            else
            {
                btnEditRecord.Enabled = true;
                btnDeleteRecord.Enabled = true;
                btnNewRecord.Enabled = true;
            }

            // mj MessageBox.Show(FLogic.DetermineCurrentInterest(grdRecordList, AEventArgs.Row));
            // (this Event also fires if the user just doubleclicks on the same record again)
            if (FLogic.Interest != FLastInterest)
            {
                ucoDetails.Interest = FLogic.DetermineCurrentInterest(grdRecordList, AEventArgs.Row);
                this.btnEditRecord.Text = "       Edi&t";
                this.btnNewRecord.Text = "       Ne&w";

                // filter display of Address detail section
                // mj ucoDetails.SetMode(TDataModeEnum.dmBrowse);
                this.FdataEditOn = false;
                FLastInterest = ucoDetails.GetSelectedInterest();
            }
        }
    }


    public class UC_PartnerInterests
    {
        public const String StrInvalidDataNotCorrected = "Cannot end editing because invalid data has not been corrected!";
        public const String StrInterestPerson = "Interests are usually added to a PERSON." + "\r\n" + "" +
                                                "Are you sure you want to add to this CLASS?" + "\r\n" + "";
        public const String StrInterestPersonTitle = "Confirm for CLASS(?)";
    }
}