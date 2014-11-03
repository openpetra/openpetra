//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Controllers;
using System.ComponentModel;


namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_FamilyMembers
    {
        private readonly string StrFamilyIDChangeDone1stLine = Catalog.GetString("The Family ID of");
        private readonly string StrFamilyIDChangeDoneWasChangedFrom = Catalog.GetString(" was changed from ");
        private readonly string StrFamilyIDChangeDoneTo = Catalog.GetString(" to ");
        private readonly string StrFamilyIDChangeDoneTitle = Catalog.GetString("Family ID Change Done");

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private SourceGrid.Cells.Editors.ComboBox FFamilyIDEditor;
        private ControllerBase FSpecialCellController = null;
        private Int32[] FamilyIDDropDownValues;
        private System.Data.DataView FFamilyMembersDV;
        private PartnerEditTDSFamilyMembersTable FFamilyMembersDT;

        private TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        private TDelegateIsNewPartner FDelegateIsNewPartner;
        private TDelegateGetPartnerLocationRowOfCurrentlySelectedAddress FDelegateGetPartnerLocationRowOfCurrentlySelectedAddress;

        private Boolean FFamilyMembersExist;

        /// <summary>Keeps track whether the SourceGrid-based Grid was already AutoSized or not.</summary>
        private Boolean FDataGridAutoSized = false;

        /// <summary>isEdited: Boolean;</summary>
        private Boolean FGridEdited;

        private Boolean FDeadlineEditMode;

        // true if the grid is being refreshed because of a broadcast message
        private Boolean FBroadcastRefresh = false;

        #region Public Methods

//        /// <summary>
//        /// Gets the data from all controls on this UserControl.
//        /// The data is stored in the DataTables/DataColumns to which the Controls
//        /// are mapped.
//        /// </summary>
//        public void GetDataFromControls2()
//        {
//            GetDataFromControls(FMainDS.PBank[0]);
//        }

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

        /// <summary>todoComment</summary>
        public Boolean GridEdited
        {
            get
            {
                Boolean ReturnValue;
                DataView ChangesDV;

                if (DataGridExist())
                {
                    ChangesDV = new DataView(((DevAge.ComponentModel.BoundDataView)grdFamilyMembers.DataSource).DataView.Table,
                        "",
                        "",
                        DataViewRowState.ModifiedCurrent);

                    if (ChangesDV.Count > 0)
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }
                }
                else
                {
                    ReturnValue = false;
                }

                return ReturnValue;
            }

            set
            {
                MessageBox.Show("FGridEdited: was: " + FGridEdited.ToString() + ", getting changed to: " + value.ToString());
                FGridEdited = value;
            }
        }

        /// true if the grid is being refreshed because of a broadcast message
        public Boolean BroadcastRefresh
        {
            set
            {
                FBroadcastRefresh = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void RecalculateTabHeaderCounter()
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            /* Fire OnRecalculateScreenParts event to update the Tab Counters */
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;
            Int64 FamilyPartnerKey;

            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                FamilyPartnerKey = FMainDS.PFamily[0].PartnerKey;
            }
            else
            {
                FamilyPartnerKey = FMainDS.PPerson[0].FamilyKey;
            }

            // retrieve Family Members from PetraServer
            // If family has no members, returns false
            try
            {
                // Make sure that Typed DataTable is already there at Client side
                if (FMainDS.FamilyMembers == null)
                {
                    FMainDS.Tables.Add(new PartnerEditTDSFamilyMembersTable(PartnerEditTDSFamilyMembersTable.GetTableName()));
                    FMainDS.InitVars();
                }

                FMainDS.FamilyMembers.Rows.Clear();
                FMainDS.Merge(FPartnerEditUIConnector.GetDataFamilyMembers(FamilyPartnerKey, ""));
                FMainDS.FamilyMembers.AcceptChanges();

                if (FMainDS.FamilyMembers.Rows.Count > 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                ReturnValue = false;
                return false;
            }
            catch (Exception)
            {
                ReturnValue = false;

                // raise;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void SpecialInitUserControl()
        {
            // disable change event while controls are being initialized as otherwise save button might get enabled
            FPetraUtilsObject.DisableDataChangedEvent();

            /* Show/hide parts of the UserControl according to Partner Class of the Partner */
            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                grpFamily.Visible = false;
                grpFamilyMembers.Location = new System.Drawing.Point(4, 4);
            }
            else
            {
                grpFamilyID.Visible = false;
                grpFamilyMembersModify.Visible = false;
                btnFamilyIDHelp.Visible = false;

                /* Set up Family Partner Information */
                txtFamilyPartnerKey.Text = String.Format("{0:0000000000}", FMainDS.PPerson[0].FamilyKey);
            }

            // initialize variables
            FDelegateGetPartnerShortName = @GetPartnerShortName;
            FDeadlineEditMode = false;

            // react to actions on grid
            grdFamilyMembers.DoubleClickCell += new TDoubleClickCellEventHandler(GrdFamilyMembers_DoubleClickCell);
            grdFamilyMembers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GrdFamilyMembers_KeyDown);

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            /* Check if data needs to be retrieved from the PetraServer */
            if (FMainDS.FamilyMembers == null)
            {
                FFamilyMembersExist = LoadDataOnDemand();
            }
            else
            {
                FMainDS.InitVars();
                FFamilyMembersExist = FMainDS.FamilyMembers.Rows.Count > 0;
            }

            /* If Family Members exist, then DataGrid is created. */
            if (FFamilyMembersExist)
            {
                /* Create SourceDataGrid columns */
                CreateGridColumns();

                /* DataBinding */
                DataBindGrid();

                /* Setup the DataGrid's visual appearance */
                SetupDataGridVisualAppearance();

                /* Prepare the Demote and Promote buttons first time */
                PrepareArrowButtons();

                /* Hook up event that fires when a different Row is selected */
                grdFamilyMembers.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
                OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFamilyMembers));
            }
            else
            {
                /* If Family has no members, these buttons are disabled */
                this.btnFamilyMemberDemote.Enabled = false;
                this.btnFamilyMemberPromote.Enabled = false;
                this.btnMovePersonToOtherFamily.Enabled = false;
                this.btnEditPerson.Enabled = false;
                this.btnEditFamilyID.Enabled = false;

                /* this.btnAddExistingPersonToThisFamily.enabled := false; */
                /* this.btnAddNewPersonThisFamily.enabled := false; */
            }

            // now changes to controls can trigger enabling of save button again
            FPetraUtilsObject.EnableDataChangedEvent();

            ApplySecurity();
        }

        /// <summary>
        /// delegate function to determine if this partner is a new record
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            /* set the delegate function from the calling System.Object */
            FDelegateIsNewPartner = ADelegateFunction;
        }

        /// <summary>
        /// delegate function to determine the currently selected location key of the partner
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitialiseDelegateGetPartnerLocationRowOfCurrentlySelectedAddress(
            TDelegateGetPartnerLocationRowOfCurrentlySelectedAddress ADelegateFunction)
        {
            /* set the delegate function from the calling System.Object */
            FDelegateGetPartnerLocationRowOfCurrentlySelectedAddress = ADelegateFunction;
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
            SetupDataGridVisualAppearance();
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;
            int DuplicateFamilyID = -1;
            DataColumn ValidationColumn = null;
            TValidationControlsData ValidationControlsData = new TValidationControlsData();
            TVerificationResult VerificationResult = null;

            if (FMainDS.PPartner[0].PartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                // Validation only needed when displayed on FAMILY screen (as Family ID can only be modified there)
                return true;
            }

            // Same 'Family ID' must only exist once across all family members
            for (int Counter = 0; Counter <= (GetNumberOfRows() - 1); Counter += 1)
            {
                for (int Counter2 = Counter + 1; Counter2 <= (GetNumberOfRows() - 1); Counter2 += 1)
                {
                    if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString() ==
                        FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString())
                    {
                        DuplicateFamilyID = (int)FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                        break;
                    }
                }
            }

            if (DuplicateFamilyID >= 0)
            {
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_DUPLICATE_FAMILY_ID,
                            new string[] { DuplicateFamilyID.ToString() })),
                    ValidationColumn, ValidationControlsData.ValidationControl);
                ReturnValue = false;

                // Handle addition to/removal from TVerificationResultCollection.
                FPetraUtilsObject.VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
            }

            if (AProcessAnyDataValidationErrors)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType());
            }

            return ReturnValue;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        private void InitializeManualCode()
        {
            FMainDS.InitVars();
        }

        /// <summary>
        ///
        /// </summary>
        private void ShowDataManual()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditFamily(System.Object sender, EventArgs e)
        {
            if (FMainDS.PPerson[0].FamilyKey == 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            try
            {
                TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                frm.SetParameters(TScreenMode.smEdit, FMainDS.PPerson[0].FamilyKey, TPartnerEditTabPageEnum.petpFamilyMembers);
                frm.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeFamily(System.Object sender, EventArgs e)
        {
            ChangeFamily(FMainDS.PPerson[0].PartnerKey, GetFamilyKey(), false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FamilyMemberPromote(System.Object sender, EventArgs e)
        {
            PromoteFamilyID();

            /* Here could only be checked the maximum values. */
            SetArrowButtons();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FamilyMemberDemote(System.Object sender, EventArgs e)
        {
            DemoteFamilyID();

            /* Here could only be checked the minimum values. */
            this.SetArrowButtons();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditFamilyID(System.Object sender, EventArgs e)
        {
            FDeadlineEditMode = (!FDeadlineEditMode);

            /* if the "edit FamilyID" button is pressed, disables screenparts, enables the combobox */
            if (FDeadlineEditMode)
            {
                this.EnableScreenParts(!FDeadlineEditMode);
                btnEditFamilyID.Focus();

                /* looks stupid, but is necessary when the keyboard is used! */
                OpenComboBox();
                btnEditFamilyID.Text = MCommonResourcestrings.StrBtnTextDone;
                this.PrepareArrowButtons();
            }
            /* if the "DONE" button is pressed, enables screenparts, disables combobox */
            else
            {
                btnEditFamilyID.Focus();

                /* looks stupid, but is necessary when the keyboard is used! */
                btnEditFamilyID.Text = Catalog.GetString("Manual Edit");
                this.EnableScreenParts(!FDeadlineEditMode);
                DisableEditing();
                this.PrepareArrowButtons();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FamilyIDHelp(System.Object sender, EventArgs e)
        {
            String StrFamilyIDExplained;

            StrFamilyIDExplained = Catalog.GetString(
                "Family Identification Number (Family ID) " + "\r\n" +
                "------------------------------------------------ " + "\r\n" +
                " This number is used to identify the family members within a Family. " +
                "\r\n" + " * Family ID's 0 and 1 are used for parents; " + "\r\n" +
                "    FamilyID's 2, 3, 4 ... 9 are used for children. " + "\r\n" +
                " * All gifts to this Family will be assigned to the Field in the Commitment" +
                "\r\n" +
                "    Record of the family member with the the lowest Family ID" +
                "\r\n" +
                "    of those who have a current Gift Destination." +
                "\r\n" +
                "\r\n" +
                " This system needs to be consistently applied to all Families, to ensure that" +
                "\r\n" +
                " gifts go to the correct Field, and that family members are" +
                "\r\n" +
                " always listed in the same order on screen and on reports.");

            MessageBox.Show(StrFamilyIDExplained, Catalog.GetString("Family ID Explained"));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPerson(System.Object sender, EventArgs e)
        {
            if (GridEdited)
            {
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (FMainDS.PPartner[0].PartnerKey == GetPartnerKeySelected())
                {
                    MessageBox.Show(Catalog.GetString("Partner is already open in this Partner Edit screen."),
                        Catalog.GetString("Partner is already open"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;

                    try
                    {
                        TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                        frm.SetParameters(TScreenMode.smEdit, GetPartnerKeySelected(), TPartnerEditTabPageEnum.petpFamilyMembers);
                        frm.Show();
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        /// <summary>
        /// when double clicked datagrid, opens selected FamilyID for editing
        /// </summary>
        /// <returns>void</returns>
        private void GrdFamilyMembers_DoubleClickCell(object Sender, CellContextEventArgs e)
        {
            if (!FDeadlineEditMode)
            {
                if (DataGridExist() && MembersInFamilyExist())
                {
                    EditPerson(this, null);
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

                    /* F5 key */
                    break;

                case 38:

                    if (e.Control == true)
                    {
                        DemoteFamilyID();
                    }

                    break;

                case 40:

                    if (e.Control == true)
                    {
                        PromoteFamilyID();
                    }

                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePersonToOtherFamily(System.Object sender, EventArgs e)
        {
            if (GridEdited)
            {
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                ChangeFamily(GetPartnerKeySelected(), GetFamilyKey(), false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePersonToThisFamily(System.Object sender, EventArgs e)
        {
            String mResultStringLbl = "";
            TPartnerClass? mPartnerClass;
            TLocationPK mResultLocationPK;

            System.Int64 NewPersonKey = 0;
            System.Int64 OtherFamilyKey = 0;
            String ProblemMessage;

            if (GridEdited)
            {
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 +
                    MPartnerResourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (FDelegateIsNewPartner != null)
                {
                    if (FDelegateIsNewPartner(FMainDS))
                    {
                        MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 +
                            MPartnerResourcestrings.StrErrorChangeFamily2,
                            MPartnerResourcestrings.StrErrorNeedToSavePartnerTitle);
                        return;
                    }

                    // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
                    if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
                    {
                        // delegate IS defined
                        try
                        {
                            TCommonScreensForwarding.OpenPartnerFindScreen.Invoke
                                (SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON),
                                out NewPersonKey,
                                out mResultStringLbl,
                                out mPartnerClass,
                                out mResultLocationPK,
                                this.ParentForm);

                            if (NewPersonKey != -1)
                            {
                                OtherFamilyKey = TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetFamilyKeyForPerson(NewPersonKey);

                                if (OtherFamilyKey == GetFamilyKey())
                                {
                                    MessageBox.Show(Catalog.GetString("You are trying to move the Person to their existing Family!\r\n" +
                                            "This is not allowed. Select a different Person in the Find Screen."),
                                        Catalog.GetString("Moving to Same Family is Not Allowed"));

                                    return;
                                }

                                if (!PersonnelChecks.WarnAboutFamilyChange(NewPersonKey,
                                        RetrievePartnerShortName(NewPersonKey),
                                        OtherFamilyKey,
                                        RetrievePartnerShortName(OtherFamilyKey),
                                        0,
                                        "",
                                        TRemote.MPersonnel.WebConnectors.HasCurrentCommitmentRecord(NewPersonKey),
                                        @ShowFamilyChangeWarning))
                                {
                                    // user chose not to continue
                                    return;
                                }

                                // call the server to perform the actual family database change
                                if (TRemote.MPartner.Partner.WebConnectors.ChangeFamily(NewPersonKey,
                                        OtherFamilyKey,
                                        GetFamilyKey(),
                                        out ProblemMessage))
                                {
                                    // even in case of success there might still be a warning message that needs display
                                    if (ProblemMessage != "")
                                    {
                                        MessageBox.Show(ProblemMessage, Catalog.GetString("Change Family"));
                                    }
                                }
                                else
                                {
                                    // can't continue after error
                                    MessageBox.Show("Change of family failed!");
                                    MessageBox.Show(ProblemMessage, Catalog.GetString("Change Family"));
                                    return;
                                }

                                if (MessageBox.Show(Catalog.GetString("The Family Change is done.\r\n\r\n" +
                                            "Do you want to see the updated list of Family Members of the Family " +
                                            "from where the Person record was moved from?"),
                                        Catalog.GetString("Family ID Change"),
                                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    this.Cursor = Cursors.WaitCursor;

                                    try
                                    {
                                        TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                                        frm.SetParameters(TScreenMode.smEdit, OtherFamilyKey, TPartnerEditTabPageEnum.petpFamilyMembers);
                                        frm.Show();
                                    }
                                    finally
                                    {
                                        this.Cursor = Cursors.Default;
                                    }
                                }

                                /* Refresh DataGrid to show the changed Family Members */
                                RefreshFamilyMembersList(this, null);
                            }
                        }
                        catch (Exception exp)
                        {
                            throw new EOPAppException("Exception occured while calling PartnerFindScreen Delegate!", exp);
                        }
                        // end try
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewPersonToThisFamily(System.Object sender, EventArgs e)
        {
            Int32 FamilysCurrentLocationKey;
            Int64 FamilysCurrentSiteKey;

            if (GridEdited)
            {
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (FDelegateIsNewPartner(FMainDS))
                {
                    MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorChangeFamily2,
                        MPartnerResourcestrings.StrErrorNeedToSavePartnerTitle);
                    return;
                }

                if (FDelegateGetPartnerLocationRowOfCurrentlySelectedAddress != null)
                {
                    FamilysCurrentLocationKey = FDelegateGetPartnerLocationRowOfCurrentlySelectedAddress().LocationKey;
                    FamilysCurrentSiteKey = FDelegateGetPartnerLocationRowOfCurrentlySelectedAddress().SiteKey;
                }
                else
                {
                    throw new EOPAppException("Delegate FGetLocationKeyOfCurrentlySelectedAddress is not set up");
                }

                TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                frm.SetParameters(TScreenMode.smNew, "PERSON", -1, -1, "", "", false,
                    FMainDS.PFamily[0].PartnerKey, FamilysCurrentLocationKey, FamilysCurrentSiteKey);
                frm.Show();
            }
        }

        /// <summary>
        /// Refreshes the list of Family Members and the TabHeader counter.
        /// </summary>
        /// <param name="sender">Not evaluated.</param>
        /// <param name="e">Not evaluated.</param>
        public void RefreshFamilyMembersList(System.Object sender, EventArgs e)
        {
            this.RefreshGrid();

            // reset counter in tab header
            RecalculateTabHeaderCounter();
        }

        /// <summary>
        /// Sets the order of colums
        /// </summary>
        /// <returns>void</returns>
        private void CreateGridColumns()
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor l_editorDt2;
            Ict.Common.TypeConverter.TDateConverter DateTypeConverter;
            FFamilyMembersDT = FMainDS.FamilyMembers;

            // Editor for Date of Birth column (Petra Date format)
            l_editorDt2 = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            l_editorDt2.EditableMode = EditableMode.None;
            DateTypeConverter = new Ict.Common.TypeConverter.TDateConverter();

            // DateTypeConverter.
            l_editorDt2.TypeConverter = DateTypeConverter;
            try
            {
                grdFamilyMembers.AddTextColumn("Person Name",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()], -1, FSpecialCellController, null, null,
                    null);
                grdFamilyMembers.AddTextColumn("Gender",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetGenderDBName()], -1, FSpecialCellController, null, null, null);
                grdFamilyMembers.AddTextColumn("Date of Birth",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetDateOfBirthDBName()], -1, FSpecialCellController, l_editorDt2, null,
                    null);
                grdFamilyMembers.AddPartnerKeyColumn("Partner Key",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);
                FamilyIDDropDownValues = new Int32[] {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 0
                };
                FFamilyIDEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(Int32), FamilyIDDropDownValues, false);
                grdFamilyMembers.AddTextColumn("Family ID",
                    FFamilyMembersDT.Columns[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()], 80, FFamilyIDEditor);
                DisableEditing();
                FFamilyIDEditor.EnableEdit = false;
                FFamilyIDEditor.Control.Validating += new CancelEventHandler(this.FamilyID_Validating);
                // DevAge.ComponentModel.Validator.ValueMapping FamilyIDDropDownMapping =
                new DevAge.ComponentModel.Validator.ValueMapping();
            }
            catch (System.NullReferenceException)
            {
            }

            // to do if no lines.
        }

        /// <summary>
        /// Disables the editing mode of FamilyID column
        /// </summary>
        /// <returns>void</returns>
        private void DisableEditing()
        {
            FFamilyIDEditor.EnableEdit = false;
        }

        private void FamilyID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            const int NEWFAMILYID_DEFAULT = -1;
            Int32 Counter;
            Int64 PartnerKey;
            Boolean IsInUse;

            IsInUse = false;
            int NewFamilyID = NEWFAMILYID_DEFAULT;
            bool ValidFormat = true;

            try
            {
                try
                {
                    NewFamilyID = Convert.ToInt32((sender as Control).Text);
                }
                catch (System.FormatException)
                {
                    ValidFormat = false;
                }
                catch (Exception exp)
                {
                    //e.Cancel = true; // do not cancel at the moment as otherwise problems occur when user selects different row with mouseclick
                    MessageBox.Show("Exception in FamilyID_Validating: " + exp.ToString());
                    throw;
                }

                if (ValidFormat)
                {
                    if ((NewFamilyID < 0) || (NewFamilyID > 99))
                    {
                        MessageBox.Show("Family ID needs to be a number between 0 and 99!");
                        //e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Only numbers are allowed as Family IDs!");
                    //e.Cancel = true; // do not cancel at the moment as otherwise problems occur when user selects different row with mouseclick
                    return;
                }

                PartnerKey = this.GetPartnerKeySelected();

                // checks if the FamilyID selected from the Combobox is already in use
                for (Counter = 0; Counter <= (GetNumberOfRows() - 1); Counter += 1)
                {
                    if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString() == NewFamilyID.ToString())
                    {
                        if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString() ==
                            this.GetFamilyID().ToString())
                        {
                            break;
                        }

                        IsInUse = true;

                        // the validating event is cancelled. (for saving the old FamilyID)
                        //e.Cancel = true; // do not cancel at the moment as otherwise problems occur when user selects different row with mouseclick
                        break;
                    }
                }

                // if The Family ID selected from the Combobox is already in use, copies the old FamilyID to that Person edited.
                // This needs to be done, because the combobox (or FFamilyIDEditor) is databinded to the database.
                if (IsInUse)
                {
                    for (Counter = 0; Counter <= (GetNumberOfRows() - 1); Counter += 1)
                    {
                        if (FFamilyMembersDV[Counter].Row == (object)PartnerKey)
                        {
                            FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)this.GetFamilyID();

                            // Int32 LocationMemory = Counter;

                            break;
                        }
                    }

                    MessageBox.Show("Please, select another Family ID, the one you selected (" + NewFamilyID.ToString() + ") is already in use!");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// returns the selected Family ID
        /// </summary>
        /// <returns>void</returns>
        private Int32 GetFamilyID()
        {
            if (GetNumberOfRows() > 0)
            {
                return Convert.ToInt32(((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the number of rows in datagrid (number of Family members)
        /// </summary>
        /// <returns>void</returns>
        private Int32 GetNumberOfRows()
        {
            if (grdFamilyMembers.DataSource == null)
            {
                return 0;
            }

            return grdFamilyMembers.DataSource.Count;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        private Int64 GetPartnerKeySelected()
        {
            return Convert.ToInt64(((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);
        }

        private Int64 GetFamilyKey()
        {
            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                return FMainDS.PPartner[0].PartnerKey;
            }
            else
            {
                return FMainDS.PPerson[0].FamilyKey;
            }
        }

        private Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            /* MessageBox.Show('TUC_FamilyMembers.GetPartnerShortName got called'); */
            return FPartnerEditUIConnector.GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass);
        }

        private String RetrievePartnerShortName(Int64 APartnerKey)
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

                /* raise EVerificationMissing.Create('this.FDelegateGetPartnerShortName could not be called!'); */
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void DataBindGrid()
        {
            FFamilyMembersDV = FFamilyMembersDT.DefaultView;
            FFamilyMembersDV.AllowNew = false;
            FFamilyMembersDV.AllowEdit = true;
            FFamilyMembersDV.AllowDelete = false;
            FFamilyMembersDV.Sort = PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName() + " ASC";

            // DataBind the DataGrid
            this.grdFamilyMembers.DataSource = new DevAge.ComponentModel.BoundDataView(FFamilyMembersDV);
            this.grdFamilyMembers.Selection.SelectRow(1, true);
        }

        /// <summary>
        /// returns true, if DataGrid is created
        /// </summary>
        /// <returns>void</returns>
        private Boolean DataGridExist()
        {
            Boolean ReturnValue;

            try
            {
                if (grdFamilyMembers.Columns.Count > 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (NullReferenceException)
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridVisualAppearance()
        {
            /*
             * AutoSize the columns.
             * This must be done only once, because when running on non-standard Display settings
             * (eg. "Large Fonts (120DPI)"), the SourceGrid gets messed up badly if AutoSizeCells is called
             * again after the initial display!!!
             */
            if (!FDataGridAutoSized)
            {
                FDataGridAutoSized = true;

                if (MainDS.FamilyMembers.Rows.Count > 0)
                {
                    grdFamilyMembers.AutoSizeCells();
                }
            }

            // It is necessary to reassign the width because the columns don't take up the maximum width
            grdFamilyMembers.Width = pnlGridActions.Left - grdFamilyMembers.Left - 8;
        }

        /// <summary>
        /// Prepares the arRow buttons, when the FamilyMembers screen is opened first time
        ///
        /// </summary>
        /// <returns>void</returns>
        private void PrepareArrowButtons()
        {
            /* The demote and promote buttons are disables if only one Family member exists */
            if ((GetNumberOfRows() <= 1) && (GetFamilyID() == 0))
            {
                this.btnFamilyMemberPromote.Enabled = false;
                this.btnFamilyMemberDemote.Enabled = false;
            }
            else
            {
                /* If Selected FamilyID is zero, Demote button is disabled */
                if (GetFamilyID() == 0)
                {
                    this.btnFamilyMemberDemote.Enabled = false;
                }

                /* If Selected FamilyID is maximum, Promote button is disabled */
                if (IsMaximum())
                {
                    this.btnFamilyMemberPromote.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Sets the ArRowbuttons (Demote,Promote). Disables or enables them. depending
        /// of selected FamilyID.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetArrowButtons()
        {
            /* If selected FamilyID is minimum, Demote button is disabled */
            if (IsMinimum() && (GetFamilyID() == 0))
            {
                this.btnFamilyMemberDemote.Enabled = false;
            }
            else
            {
                this.btnFamilyMemberDemote.Enabled = true;
            }

            /* If selected FamilyID is maximum, Promote button is disabled */
            if (IsMaximum())
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
        /// Checks if the selected FamilyID is maximum.
        /// </summary>
        /// <returns>void</returns>
        public Boolean IsMaximum()
        {
            Boolean ReturnValue = false;
            Int32 Counter;
            Int32 FamilyIDint;

            FamilyIDint = Convert.ToInt32(((DataRowView)grdFamilyMembers.SelectedDataRows[0])[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);

            // Goes throuht the FamilyID:s, If finds larger than selected, breaks and returns false. Otherwice true.
            for (Counter = 0; Counter <= (this.GetNumberOfRows() - 1); Counter += 1)
            {
                if (Convert.ToInt64(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]) > FamilyIDint)
                {
                    ReturnValue = false;
                    break;
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if the selected FamilyID is minimun.
        /// Checks, if the Family ID of selectedrow is the smallest of FamilyID:s.
        /// </summary>
        /// <returns>void</returns>
        public Boolean IsMinimum()
        {
            Boolean ReturnValue = false;
            Int32 Counter;
            Int32 FamilyIDint;

            FamilyIDint = Convert.ToInt32(
                ((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]);

            // Goes throuht the FamilyID:s, If finds smaller than selected, breaks and returns false. Otherwice true.
            for (Counter = 0; Counter <= (this.GetNumberOfRows() - 1); Counter += 1)
            {
                if (Convert.ToInt64(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()]) < FamilyIDint)
                {
                    ReturnValue = false;
                    break;
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        ///
        /// </summary>
        private void RefreshGrid()
        {
            if (GridEdited)
            {
                MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorMaintainFamilyMembers2);
            }
            else
            {
                if (!DataGridExist())
                {
                    try
                    {
                        if (LoadDataOnDemand())
                        {
                            /* Create SourceDataGrid columns */
                            CreateGridColumns();

                            /* DataBinding */
                            DataBindGrid();

                            /* Setup the DataGrid's visual appearance */
                            FDataGridAutoSized = false;  // ensure Columns are Auto-Sized again (needed if there wasn't a Person in the list before)
                            SetupDataGridVisualAppearance();

                            /* Prepare the Demote and Promote buttons first time */
                            PrepareArrowButtons();

                            /* Hook up event that fires when a different Row is selected */
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
                    if (LoadDataOnDemand())
                    {
                        /* One or more Family Members present > select first one in Grid */
                        grdFamilyMembers.Selection.SelectRow(1, true);

                        // if refresh is the result of a broadcast message we do not want to bring the grid into focus
                        if (!FBroadcastRefresh)
                        {
                            /* Make the Grid respond on updown keys */
                            grdFamilyMembers.Focus();
                        }

                        btnEditPerson.Enabled = true;
                        btnMovePersonToOtherFamily.Enabled = true;
                        btnEditFamilyID.Enabled = true;

                        /* Prepare the Demote and Promote buttons */
                        SetArrowButtons();
                    }
                    else
                    {
                        /* No Family Member present > disable buttons that are no longer relevant */
                        btnFamilyMemberDemote.Enabled = false;
                        btnFamilyMemberPromote.Enabled = false;
                        btnEditPerson.Enabled = false;
                        btnMovePersonToOtherFamily.Enabled = false;
                        btnEditFamilyID.Enabled = false;

                        /* Prepare the Demote and Promote buttons */
                        PrepareArrowButtons();
                    }

                    ApplySecurity();
                }
            }
        }

        /// <summary>
        /// procedure grdFamilyMembers_ClickCell(Sender: System.Object; e: SourceGrid.CellContextEventArgs); Sets the ArRowbuttons (Demote,Promote). Disables or enables them. depending of selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        private void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            this.SetArrowButtons();
        }

        private void ChangeFamily(Int64 APersonKey, Int64 AOldFamilyKey, Boolean AChangeToThisFamily)
        {
            String mResultStringLbl = "";
            TPartnerClass? mPartnerClass;

            System.Int64 NewFamilyKey = 0;
            String ProblemMessage;
            TLocationPK mResultLocationPK;

            if (FDelegateIsNewPartner != null)
            {
                if (FDelegateIsNewPartner(FMainDS))
                {
                    MessageBox.Show(MPartnerResourcestrings.StrErrorNeedToSavePartner1 + MPartnerResourcestrings.StrErrorChangeFamily2,
                        MPartnerResourcestrings.StrErrorNeedToSavePartnerTitle);
                    return;
                }

                if (PersonnelChecks.WarnAboutFamilyChange(APersonKey,
                        RetrievePartnerShortName(APersonKey),
                        AOldFamilyKey,
                        RetrievePartnerShortName(AOldFamilyKey),
                        0,
                        "",
                        TRemote.MPersonnel.WebConnectors.HasCurrentCommitmentRecord(APersonKey),
                        @ShowFamilyChangeWarning))
                {
                    // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
                    if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
                    {
                        // delegate IS defined
                        try
                        {
                            TCommonScreensForwarding.OpenPartnerFindScreen.Invoke
                                (SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY),
                                out NewFamilyKey,
                                out mResultStringLbl,
                                out mPartnerClass,
                                out mResultLocationPK,
                                this.ParentForm);

                            if (NewFamilyKey != -1)
                            {
                                if (AOldFamilyKey == NewFamilyKey)
                                {
                                    MessageBox.Show(Catalog.GetString("You are trying to move the Person to their existing Family!\r\n" +
                                            "This is not allowed. Select a different Family in the Find Screen."),
                                        Catalog.GetString("Moving to Same Family is Not Allowed"));

                                    return;
                                }

                                // call the server to perform the actual family database change
                                if (TRemote.MPartner.Partner.WebConnectors.ChangeFamily(APersonKey,
                                        AOldFamilyKey,
                                        NewFamilyKey,
                                        out ProblemMessage))
                                {
                                    // even in case of success there might still be a warning message that needs display
                                    if (ProblemMessage != "")
                                    {
                                        MessageBox.Show(ProblemMessage, Catalog.GetString("Change Family"));
                                    }
                                }
                                else
                                {
                                    // can't continue after error
                                    MessageBox.Show("Change of family failed!");
                                    MessageBox.Show(ProblemMessage, Catalog.GetString("Change Family"));
                                    return;
                                }

                                // update family key on display
                                if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                                {
                                    FMainDS.PPerson[0].FamilyKey = NewFamilyKey;

                                    /* Update Family GroupBox */
                                    txtFamilyPartnerKey.Text = String.Format("{0:0000000000}", NewFamilyKey);
                                }

                                if (MessageBox.Show(Catalog.GetString("The Family Change is done.\r\n\r\n" +
                                            "Do you want to see the updated list of Family Members of the Family " +
                                            "to which the Partner was moved to?"),
                                        Catalog.GetString("Family ID Change"),
                                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    this.Cursor = Cursors.WaitCursor;

                                    try
                                    {
                                        TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                                        frm.SetParameters(TScreenMode.smEdit, NewFamilyKey, TPartnerEditTabPageEnum.petpFamilyMembers);
                                        frm.Show();
                                    }
                                    finally
                                    {
                                        this.Cursor = Cursors.Default;
                                    }
                                }

                                /* Refresh DataGrid to show the changed Family Members */
                                RefreshFamilyMembersList(this, null);
                            }
                        }
                        catch (Exception exp)
                        {
                            throw new EOPAppException("Exception occured while calling PartnerFindScreen Delegate!", exp);
                        }
                        // end try
                    }
                }
            }
        }

        /// <summary>
        /// returns true if Family has members (in the datagrid)
        /// </summary>
        /// <returns>void</returns>
        private Boolean MembersInFamilyExist()
        {
            Boolean ReturnValue;

            try
            {
                if (grdFamilyMembers.Rows.Count > 1)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                // MessageBox.show(' DO NOT PRESS THE BUTTONS, IF THERE IS NOTHING TO REFRESH'+#10+' If you continue your antisocial behaviour, Petra Team will punish you','STOP THAT IMMEDIATELY');
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// enables the screenparts, if false, disables.
        /// </summary>
        /// <returns>void</returns>
        private void EnableScreenParts(Boolean Value)
        {
            this.btnFamilyMemberDemote.Enabled = Value;
            this.btnFamilyMemberPromote.Enabled = Value;
            this.btnMovePersonToOtherFamily.Enabled = Value;
            this.btnEditPerson.Enabled = Value;
            this.btnAddExistingPersonToThisFamily.Enabled = Value;
            this.btnAddNewPersonToThisFamily.Enabled = Value;
            ApplySecurity();
        }

        /// <summary>
        /// enables the FamilyID edit combobox. This causes no errors, but uses the default list for FamilyID:s (1,2,3,4,5,6,7,8,9,0)
        /// </summary>
        /// <returns>void</returns>
        private void OpenComboBox()
        {
            Int32 RowNumber;

            RowNumber = this.GetRowSelected();
            FFamilyIDEditor.EnableEdit = true;
            FFamilyIDEditor.EditableMode = EditableMode.Focus;
            grdFamilyMembers.Selection.Focus(new Position(RowNumber, grdFamilyMembers.Columns.Count - 1), true);
        }

        /// <summary>
        /// Returns the PartnerKey that's selected
        /// </summary>
        /// <returns>void</returns>
        private Int32 GetRowSelected()
        {
            System.Int32 ARowNumber;
            System.Int64 APartnerKey;
            this.GetRowSelected(out ARowNumber, out APartnerKey);
            return ARowNumber;
        }

        /// <summary>
        /// Finds out the number of row, and it's Partnerkey in datagrid that's selected.
        /// </summary>
        /// <returns>void</returns>
        public void GetRowSelected(out Int32 ARowNumber, out Int64 APartnerKey)
        {
            System.Int32 CurrentRow;
            DataView AGridDataView;
            AGridDataView = ((DevAge.ComponentModel.BoundDataView)grdFamilyMembers.DataSource).DataView;
            ARowNumber = 0;
            APartnerKey = Convert.ToInt64(
                ((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]);

            // goes through the FamilyID:s in datagrid, break when comes to selected.
            for (CurrentRow = 0; CurrentRow <= AGridDataView.Count - 1; CurrentRow += 1)
            {
                ARowNumber = ARowNumber + 1;

                if (Convert.ToInt64(AGridDataView[CurrentRow].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()]) == APartnerKey)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Promotes selected ID (and demotes the FamilyID next (up) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        public void PromoteFamilyID()
        {
            Int32 buttonvalue = -1;
            Int32 Counter1;
            Int32 NumberOfRows;
            Int32 Counter2;
            Int32 Counter2ToMax;
            Int32 FamilyIDint;

            System.Object PartnerKey;
            System.Object FamilyID = -1;
            System.Object PersonName1 = "";
            System.Object PersonName2 = "";
            System.Object NextFamilyID = "";
            Boolean MemberFind;
            MessageBoxButtons Button;

            // Get the PartnerKey of the selected Row
            PartnerKey = ((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()];
            NumberOfRows = GetNumberOfRows();

            // for loop to get the selected partners ID
            for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
            {
                if (FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                {
                    FamilyID = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                    PersonName1 = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                    break;
                }
            }

            // loop to find the nearest  FamilyID compared to selected FamilyID.
            // sets the next FamilyID to correct.
            MemberFind = true;
            FamilyIDint = Convert.ToInt32(FamilyID);

            // If Family ID 1 (parent) is about to be promoted
            if (FamilyIDint == 1)
            {
                Button = MessageBoxButtons.YesNo;
                buttonvalue = 0;

                // if pressed No to question below, does nothing.
                if (MessageBox.Show(Catalog.GetString("Parents should be Family ID 0 or 1 \r\nAre you sure you want to change this Family ID?"),
                        Catalog.GetString("Family ID Change"),
                        Button) == DialogResult.No)
                {
                    buttonvalue = 1;
                }
            }

            // Executes this loop, if user wants to promote Family ID
            if (buttonvalue != 1)
            {
                Counter2ToMax = 100;

                // goes through all values from selected +1 to 100+selected.
                for (Counter1 = 1; Counter1 <= Counter2ToMax; Counter1 += 1)
                {
                    // goes through all FamilyID:s
                    for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
                    {
                        // When finds FamilyID that's next (above) to selected FamilyID, Replaces the Found Family members ID with selected ID
                        if (Convert.ToInt32(FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString())
                            == FamilyIDint + Counter1)
                        {
                            // saves the FamilyID just found
                            PersonName2 = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                            NextFamilyID = FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                            FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = FamilyID;
                            MemberFind = false;
                            break;
                        }
                    }

                    if (!MemberFind)
                    {
                        break;
                    }
                }

                // end;
                // loop to set the selected FamilyID to the PreviousFamilyID
                for (Counter2 = 0; Counter2 <= (NumberOfRows - 1); Counter2 += 1)
                {
                    if (FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                    {
                        FFamilyMembersDV[Counter2].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)(FamilyIDint + 1);
                        break;
                    }
                }

                // now select this person record again to make it easier to move it further in the list
                SelectPersonInGrid(Convert.ToInt64(PartnerKey));

                // button := MessageBoxButtons.OK;
                MessageBox.Show((StrFamilyIDChangeDone1stLine + "\r\n" + "    " + PersonName1.ToString() + StrFamilyIDChangeDoneWasChangedFrom +
                                 FamilyID.ToString() + StrFamilyIDChangeDoneTo +
                                 Convert.ToString(FamilyIDint +
                                     1) + "\r\n" + "    " + PersonName2.ToString() + StrFamilyIDChangeDoneWasChangedFrom + NextFamilyID.ToString() +
                                 StrFamilyIDChangeDoneTo + FamilyID.ToString()), StrFamilyIDChangeDoneTitle);

                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
            }
        }

        /// <summary>
        /// Demotes selected ID (and promotes the FamilyID next (lower) to selected FamilyID
        /// </summary>
        /// <returns>void</returns>
        public void DemoteFamilyID()
        {
            Int32 Buttonvalue = -1;
            Int32 NumberOfRows;
            Int32 Counter;
            Int32 CounterToMax;
            Int32 FamilyIDint;

            System.Object PartnerKey;
            System.Object FamilyID = -1;
            System.Object PreviousFamilyID = -1;
            System.Object PersonName1 = "";
            System.Object PersonName2 = "";
            Boolean MemberFind;
            MessageBoxButtons Button;

            // Get the PartnerKey of the selected Row
            PartnerKey = ((DataRowView)grdFamilyMembers.SelectedDataRows[0]).Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()];
            NumberOfRows = GetNumberOfRows();

            // for loop to get the selected partners ID
            for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
            {
                if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                {
                    FamilyID = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                    PersonName1 = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                    break;
                }
            }

            if ((this.IsMinimum()) && (Convert.ToInt32(FamilyID) > 0))
            {
                FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = (object)0;
                Buttonvalue = 1;
                FamilyID = (object)0;
            }

            // loop to find the nearest smaller FamilyID compared to selected FamilyID.
            // sets the previous FamilyID to correct.
            MemberFind = true;
            FamilyIDint = Convert.ToInt32(FamilyID);

            // If Family ID is 2, or A parent is to be replaced with child, gives warning.
            if (FamilyIDint == 2)
            {
                Button = MessageBoxButtons.YesNo;

                if (MessageBox.Show("Parents should be Family ID 0 or 1" + "\r\nAre you sure you want to change this Family ID?", "Family ID Change",
                        Button) == DialogResult.No)
                {
                    Buttonvalue = 1;
                }
            }

            // if FamilyID to be demoted is 2 and cancel is selected from warnind messagebox, does nothing. otherwice goes through the  for loop
            if (Buttonvalue != 1)
            {
                // Goes through the FamilyID:s from 1 to selected FamilyID
                for (CounterToMax = 1; CounterToMax <= FamilyIDint; CounterToMax += 1)
                {
                    // Goes through all the FamilyID:s
                    for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
                    {
                        // when finds FamilyID next to selected (below), Sets the found Family ID to selected
                        if (Convert.ToInt32(FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()].ToString()) ==
                            (FamilyIDint - CounterToMax))
                        {
                            // saves the found FamilyID
                            PersonName2 = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerShortNameDBName()];
                            PreviousFamilyID = FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()];
                            FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] =
                                (object)(Convert.ToInt32(PreviousFamilyID) + 1);

                            MemberFind = false;
                            break;
                        }
                    }

                    if (!MemberFind)
                    {
                        break;
                    }
                }

                // loop to set the selected FamilyID to the PreviousFamilyID
                for (Counter = 0; Counter <= (NumberOfRows - 1); Counter += 1)
                {
                    if (FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == PartnerKey.ToString())
                    {
                        FFamilyMembersDV[Counter].Row[PartnerEditTDSFamilyMembersTable.GetFamilyIdDBName()] = PreviousFamilyID;
                        break;
                    }
                }

                // now select this person record again to make it easier to move it further in the list
                SelectPersonInGrid(Convert.ToInt64(PartnerKey));

                MessageBox.Show((StrFamilyIDChangeDone1stLine + "\r\n" + "    " + PersonName1.ToString() + StrFamilyIDChangeDoneWasChangedFrom +
                                 FamilyID.ToString() + StrFamilyIDChangeDoneTo + PreviousFamilyID.ToString() + "\r\n" + "    " +
                                 PersonName2.ToString() +
                                 StrFamilyIDChangeDoneWasChangedFrom + PreviousFamilyID.ToString() + StrFamilyIDChangeDoneTo +
                                 Convert.ToString(Convert.ToInt32(PreviousFamilyID) + 1)), "Family ID Change Done");

                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
            }
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
            {
                /* need to disable all Buttons that allow modification of p_person record */
                CustomEnablingDisabling.DisableControl(grpFamily, btnChangeFamily);
                CustomEnablingDisabling.DisableControlGroup(grpFamilyID);
                CustomEnablingDisabling.DisableControlGroup(grpFamilyMembersModify);
            }
        }

        private bool ShowFamilyChangeWarning(String AMessage)
        {
            return MessageBox.Show(AMessage, Catalog.GetString("Change Family"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// select person row with given partner key
        /// </summary>
        /// <param name="APartnerKey"></param>
        private void SelectPersonInGrid(Int64 APartnerKey)
        {
            int NumberOfRows = grdFamilyMembers.Rows.Count;

            // loop to set the selected FamilyID to the PreviousFamilyID
            for (int Counter = 1; Counter <= (NumberOfRows); Counter += 1)
            {
                if (FFamilyMembersDV[Counter - 1].Row[PartnerEditTDSFamilyMembersTable.GetPartnerKeyDBName()].ToString() == APartnerKey.ToString())
                {
                    grdFamilyMembers.SelectRowInGrid(Counter);
                    return;
                }
            }
        }

        #endregion

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.L | Keys.Control))
            {
                grdFamilyMembers.Focus();
                return true;
            }

            return false;
        }

        #endregion
    }
}