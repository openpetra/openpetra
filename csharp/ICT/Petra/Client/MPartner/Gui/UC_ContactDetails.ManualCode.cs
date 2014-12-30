//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ContactDetails
    {
        private enum TTimerDrivenMessageBoxKind
        {
            tdmbkNoPrimaryEmailAsNoCurrentAvailable,

            tdmbkNoPrimaryEmailButNonCurrentAvailable
        }

        private readonly string StrDefaultContactType = Catalog.GetString("Phone");

        private readonly string StrFunctionKeysTip = Catalog.GetString(
            "  (Press <F5>-<F8> to change Contact Type; press <SHIFT>+any of those keys to choose alternative.)");

        private readonly string StrPrimPhoneConsequence1 = Catalog.GetString(
            "Please select a current Phone Number from the available ones!");

        private readonly string StrPrimPhoneConsequence2 = Catalog.GetString(
            "The Primary Phone Number has been cleared as there is no other current Phone record.");

        private readonly string StrPrimPhoneMessageBoxTitle1 = Catalog.GetString("Primary Phone Number Needs Adjusting");

        private readonly string StrPrimPhoneMessageBoxTitle2 = Catalog.GetString("Primary Phone Number Cleared");

        private readonly string StrPrimEmailConsequence1 = Catalog.GetString(
            "Please select a current E-mail Address from the available ones!");

        private readonly string StrPrimEmailMessageBoxTitle1 = Catalog.GetString("Primary E-Mail Address Needs Adjusting");

        private readonly string StrPrimEmailConsequence2 = Catalog.GetString(
            "The Primary E-mail Address has been cleared as there is no other current E-Mail Address record.");

        private readonly string StrPrimEmailMessageBoxTitle2 = Catalog.GetString("Primary E-Mail Address Cleared");

        private string FDefaultContactType = String.Empty;

        /// <summary>Holds a reference to an ImageList containing Icons that can be shown in Grid Rows</summary>
        private ImageList FGridRowIconsImageList;

        private TPartnerAttributeTypeValueKind FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;

        System.Windows.Forms.Timer ShowMessageBoxTimer = new System.Windows.Forms.Timer();

        private TTimerDrivenMessageBoxKind FTimerDrivenMessageBoxKind;

        private bool FFilterInitialised = false;

        private bool FRunningInsideShowDetails = false;

        private bool FRunningInsideDataSaving = false;

        private bool FValueWithSpecialMeaningChangedButUserDidntLeaveControl = false;

        private bool FSuppressOnContactTypeChangedEvent = false;

        private bool FPrimaryPhoneSelectedValueChangedEvent = false;

        private bool FPrimaryEmailSelectedValueChangedEvent = false;

        private int FSelectedRowIndexBeforeSaving = -1;

        /// <summary>
        /// Usage: see Methods <see cref="PreDeleteManual"/> and <see cref="PostDeleteManual"/>.
        /// </summary>
        private string FDeletedRowsAttributeType = String.Empty;

        /// <summary>
        /// Usage: see Methods <see cref="PreDeleteManual"/> and <see cref="PostDeleteManual"/>.
        /// </summary>
        private string FDeletedRowsValue = String.Empty;


        /// <summary>
        /// Populated by Method <see cref="Calculations.DeterminePhoneAttributes"/>.
        /// </summary>
        private DataView FPhoneAttributesDV = null;

        #region Properties

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        /// <summary>todoComment</summary>

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void HandleDataSavingStarted(System.Object sender, System.EventArgs e)
        {
            FRunningInsideDataSaving = true;
            PPartnerAttributeRow CurrentDetailDR = GetSelectedDetailRow();

            if (CurrentDetailDR != null)
            {
                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    FValueWithSpecialMeaningChangedButUserDidntLeaveControl =
                        String.Compare(txtValue.Text, CurrentDetailDR.Value, StringComparison.InvariantCulture) != 0;
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (RowHasPhoneAttributeType(CurrentDetailDR))
                    {
                        FValueWithSpecialMeaningChangedButUserDidntLeaveControl =
                            String.Compare(txtValue.Text, CurrentDetailDR.Value, StringComparison.InvariantCulture) != 0;
                    }
                }

                // make sure latest screen modifications are saved to FMainDS
                GetDataFromControls();

                // Refresh the ComboBox so it reflects any change in the email address!
                UpdatePrimaryEmailComboItems(true);
                UpdatePrimaryPhoneComboItems();

                FSelectedRowIndexBeforeSaving = grdDetails.GetFirstHighlightedRowIndex();
            }
            else
            {
                FSelectedRowIndexBeforeSaving = -1;
            }
        }

        void HandleDataSaved(object Sender, TDataSavedEventArgs e)
        {
            FRunningInsideDataSaving = false;
        }

        #region Public Methods

        /// <summary>todoComment</summary>
        public void PostInitUserControl(PartnerEditTDS AMainDS)
        {
            DataRow[] DefaultContactTypes;

            FMainDS = AMainDS;

            // disable change event while controls are being initialized as otherwise save button might get enabled
            FPetraUtilsObject.DisableDataChangedEvent();

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.HandleDataSavingStarted);
            FPetraUtilsObject.DataSaved += HandleDataSaved;

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
            }

            // now changes to controls can trigger enabling of save button again
            FPetraUtilsObject.EnableDataChangedEvent();


            // Try to find a Contact Type that matches the default; if found, new Records are defaulting to that Contact Type.
            DefaultContactTypes = FMainDS.PPartnerAttributeType.Select(
                PPartnerAttributeTypeTable.GetAttributeTypeDBName() + " = '" + StrDefaultContactType + "'");

            if (DefaultContactTypes.Length > 0)
            {
                FDefaultContactType = StrDefaultContactType;
            }
            else
            {
                if (FMainDS.PPartnerAttributeType.Count > 0)
                {
                    // The Contact Type that should be Default wasn't found, therefore use the first Contact Type of the first Contact Category
                    DataView SortedPartnerAttr = new DataView(FMainDS.PPartnerAttributeType, String.Empty,
                        "CategoryIndex ASC, " + PPartnerAttributeTypeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);
                    FDefaultContactType = ((PPartnerAttributeTypeRow)SortedPartnerAttr[0].Row).AttributeType;
                }
            }

            // These SelectedValueChanged Events must not be hooked up earlier as that could lead to a 'race condition' in certain data scenarios!
            cmbContactCategory.SelectedValueChanged += new System.EventHandler(this.FilterContactTypeCombo);
            cmbContactType.SelectedValueChanged += new System.EventHandler(this.OnContactTypeChanged);

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//            rtbValue.LinkClicked += new Ict.Common.Controls.TRtbHyperlinks.THyperLinkClickedArgs(rtbValue.Helper.LaunchHyperLink);

            // TODO ApplySecurity();
        }

        /// <summary>
        /// Extra Tab initialisation on inital Tab 'activation'.
        /// </summary>
        public void SpecialInitUserControl()
        {
            // Set up special sort order of Rows in Grid:
            // PPartnerAttributeCategory.Index followed by PPartnerAttributeType.Index followed by PPartnerAttribute.Index!
            DataView gridView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            gridView.Sort = "Parent_Parent_CategoryIndex ASC, Parent_AttributeIndex ASC, " +
                            PPartnerAttributeTable.GetIndexDBName() + " ASC";

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
            }
        }

        void FilterToggledManual(bool AIsCollapsed)
        {
            if (!AIsCollapsed)
            {
                if (FFilterInitialised == false)
                {
                    FFilterAndFindObject.SwitchOnKeepFilterTurnedOn(TUcoFilterAndFind.FilterContext.StandardFilterOnly);
                    FFilterAndFindObject.ToggleFilter();

                    FFilterInitialised = true;
                }
            }
        }

        /// <summary>
        /// Performs necessary actions to make the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet possible.
        /// </summary>
        /// <returns>void</returns>
        public void CleanupRecordsBeforeMerge()
        {
            DataView NewPartnerAttributesDV;

            List <DataRow>PPartnerAttributeDeleteRows = new List <DataRow>();

            /*
             * Check if PartnerAttributes have been added
             * -> remove them on the Client side, otherwise we will end up with these rows
             *   (having Sequence values below 0) plus the rows coming from the Server
             *   (having Sequence values that were determined by a Sequence) after merging
             */
            NewPartnerAttributesDV = new DataView(FMainDS.PPartnerAttribute, "", "", DataViewRowState.Added);

            // First check and remember affected DataRows
            for (int Counter = 0; Counter <= NewPartnerAttributesDV.Count - 1; Counter += 1)
            {
                PPartnerAttributeDeleteRows.Add(NewPartnerAttributesDV[Counter].Row);
            }

            // Now remove affected DataRows
            foreach (DataRow DeleteRow in PPartnerAttributeDeleteRows)
            {
                NewPartnerAttributesDV.Table.Rows.Remove(DeleteRow);
            }
        }

        /// <summary>
        /// Performs necessary actions after the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet.
        /// </summary>
        /// <returns>void</returns>
        public void RefreshRecordsAfterMerge()
        {
            // Make sure selected row in grid is reinitialized after save in case
            // it got replaced during merge process.
            if (FSelectedRowIndexBeforeSaving >= 0)
            {
                grdDetails.SelectRowInGrid(FSelectedRowIndexBeforeSaving);
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            String ResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir", true);

            // initialize Image List
            FGridRowIconsImageList = new System.Windows.Forms.ImageList();
            FGridRowIconsImageList.ImageSize = new System.Drawing.Size(16, 16);
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "PrimaryPhone.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "PrimaryEmail.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.Images.Add(TIconCache.IconCache.AddOrGetExistingIcon(ResourceDirectory + Path.DirectorySeparatorChar +
                    "Completeley_Empty.ico", TIconCache.TIconSize.is16by16));
            FGridRowIconsImageList.TransparentColor = System.Drawing.Color.Transparent;

            if (!FMainDS.Tables.Contains(PPartnerAttributeTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PartnerEditTDSPPartnerAttributeTable());
                FMainDS.InitVars();
            }

            // Set up Timer that is needed for showing MessageBoxes from a Grid Event
            ShowMessageBoxTimer.Tick += new EventHandler(ShowTimerDrivenMessageBox);
            ShowMessageBoxTimer.Interval = 500;

            //
            // Ensure we have instances of PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable in FMainDS. They are needed because
            // the Grid's underlying DataTable has got custom DataColumns with Expressions that reference those DataTables in the DataSet!
            //
            // Note 1: When an existing Partner gets opened, FMainDS does not contain instances of PPartnerAttributeCategoryTable or PPartnerAttributeTypeTable
            // hence we add them throught he following code
            // Note 2: When a new Partner gets created, FMainDS contains instances of PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable hence they will
            // not be created by the following code
            if (!FMainDS.Tables.Contains(PPartnerAttributeCategoryTable.GetTableName())
                && (!FMainDS.Tables.Contains(PPartnerAttributeTypeTable.GetTableName())))
            {
                FMainDS.Tables.Add(new PPartnerAttributeCategoryTable(PPartnerAttributeCategoryTable.GetTableName()));
                FMainDS.Tables.Add(new PPartnerAttributeTypeTable(PPartnerAttributeTypeTable.GetTableName()));
                FMainDS.InitVars();
            }

            if (FMainDS.PPartnerAttributeCategory.Count == 0)
            {
                // Note: If FMainDS contains an instance of the PPartnerAttributeCategoryTable, but it hasn't got any rows
                // we add them here from the corresponding Cacheable DataTable (that is also the case when a new Partner gets created)
                FMainDS.Merge((PPartnerAttributeCategoryTable)TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.
                        ContactCategoryList, PPartnerAttributeCategoryTable.GetTableName()));

                if (FMainDS.PPartnerAttributeCategory.Count == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "There are no Partner Contact Categories available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Category!"),
                        Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (FMainDS.PPartnerAttributeType.Count == 0)
            {
                // Note: If FMainDS contains an instance of the PPartnerAttributeTypeTable, but it hasn't got any rows
                // we add them here from the corresponding Cacheable DataTable (that is also the case when a new Partner gets created)
                FMainDS.Merge((PPartnerAttributeTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.ContactTypeList,
                        PPartnerAttributeTypeTable.GetTableName()));

                if (FMainDS.PPartnerAttributeType.Count == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "There are no Partner Contact Types available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Type!"),
                        Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Need to enable Relations as the PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable have not been part of the PartnerEditTDS when transferred from the OpenPetra Server!
            // These Relations are needed in Method 'CreateCustomDataColumns'.
            FMainDS.EnableRelation("ContactDetails1");
            FMainDS.EnableRelation("ContactDetails2");

            FPhoneAttributesDV = Calculations.DeterminePhoneAttributes(FMainDS.PPartnerAttributeType);

            // Show the 'Within The Organisation' GroupBox only if the Partner is of Partner Class PERSON
            if (FMainDS.PPartner[0].PartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                grpWithinTheOrganisation.Visible = false;
            }

            // Move the 'Within the Organsiation' GroupBox a bit up from it's automatically assigned position
            grpWithinTheOrganisation.Top = 16;

            // Move the Panel that groups the 'Current' Controls for layout purposes a bit up from it's automatically assigned position
            pnlCurrentGrouping.Top = 58;
            chkCurrent.Top = 7;
            dtpNoLongerCurrentFrom.Top = 8;
            lblNoLongerCurrentFrom.Top = 12;

            // Move the Panel that groups the 'Value' Controls for layout purposes a bit up from it's automatically assigned position
            pnlValueGrouping.Top = 29;
            txtValue.Top = 3;
            lblValue.Top = 9;
            btnLaunchHyperlinkFromValue.Top = 3;

            chkConfidential.Top = 88;
            lblConfidential.Top = 93;

            // Set up status bar texts for unbound controls and for bound controls whose auto-assigned texts don't match the use here on this screen (these talk about 'Partner Attributes')
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryWayOfContacting,
                Catalog.GetString("Select the primary method by which the Partner should be contacted. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryPhoneForContacting,
                Catalog.GetString("Select one of the Partner's telephone numbers. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryEMail,
                Catalog.GetString(
                    "Select one of the Partner's e-mail addresses. This will be used whenever an automated e-mail is to be sent to this Partner."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkPrefEMail,
                Catalog.GetString("Click this button to send an email to the Partner's Primary E-mail address."));
            FPetraUtilsObject.SetStatusBarText(cmbPhoneWithinTheOrganisation,
                Catalog.GetString(
                    "Select one of the Partner's telephone numbers to designate it as her/his telephone number within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(cmbEMailWithinTheOrganisation,
                Catalog.GetString("Select one of the Partner's e-mail addresses to designate it as her/his e-mail address within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkEMailWithinOrg,
                Catalog.GetString("Click this button to send an email to the Partner's Office E-mail address."));

            FPetraUtilsObject.SetStatusBarText(btnPromote,
                Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));
            FPetraUtilsObject.SetStatusBarText(btnDemote,
                Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));

            FPetraUtilsObject.SetStatusBarText(cmbContactCategory,
                Catalog.GetString("Contact Category to which the Contact Type belongs to (narrows down available Contact Types)."));
            FPetraUtilsObject.SetStatusBarText(cmbContactType,
                Catalog.GetString("Describes what the Value is (e.g. Phone Number, E-Mail Address, etc)."));
            FPetraUtilsObject.SetStatusBarText(chkSpecialised,
                Catalog.GetString("Tick this if the Value designates a business-related Contact Detail (e.g. business telephone number)."));

            FPetraUtilsObject.SetStatusBarText(txtComment, Catalog.GetString("Comment for this Contact Detail record."));
            FPetraUtilsObject.SetStatusBarText(chkCurrent, Catalog.GetString("Untick this if the Contact Detail record is no longer current."));
            FPetraUtilsObject.SetStatusBarText(dtpNoLongerCurrentFrom,
                Catalog.GetString("Date from which the Contact Detail record is no longer current."));

            FPetraUtilsObject.SetStatusBarText(chkConfidential,
                Catalog.GetString(
                    "Tick this if the Contact Detail record is confidential. Please refer to the User Guide what effect this setting has!"));

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
            FPetraUtilsObject.SetStatusBarText(txtValue,
                Catalog.GetString(
                    "Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about.  (Press F5-F7 to change Type!)"));
//            FPetraUtilsObject.SetStatusBarText(rtbValue, Catalog.GetString("Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about.  (Press F5-F7 to change Type!)"));

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//            rtbValue.BuildLinkWithValue = BuildLinkWithValue;


            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // Hide all not-yet-implemented Controls in the 'Overall Contact Settings' GroupBox for the time being - their implementation will follow
            grpWithinTheOrganisation.Visible = false;
            pnlPromoteDemote.Visible = false;
        }

        private void ShowDataManual()
        {
            FPrimaryEmailSelectedValueChangedEvent = true;

            UpdatePrimaryEmailComboItems(false);

            FPrimaryEmailSelectedValueChangedEvent = false;

            FPrimaryPhoneSelectedValueChangedEvent = true;

            UpdatePrimaryPhoneComboItems();

            FPrimaryPhoneSelectedValueChangedEvent = false;


            SelectPrimaryWayOfContactingComboItem();
        }

        private void ShowDetailsManual(PPartnerAttributeRow ARow)
        {
//            long RelationPartnerKey;

            // show controls if not visible yet
            MakeDetailsInvisible(false);

            btnDelete.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
            }

            OnContactTypeChanged(null, null);

            FRunningInsideShowDetails = false;
        }

        /// <summary>
        /// Manual way for getting the data that is contained in the 'Overall Contact Settings' GroupBox' Control.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool GetOverallContactSettingsDataFromControls()
        {
            bool ReturnValue;

            ReturnValue = UpdatePrimaryEmailAddressRecords(true);

            if (ReturnValue)
            {
                ReturnValue = UpdatePrimaryPhoneNumberRecords(true);
            }

            if (ReturnValue)
            {
                UpdatePrimaryWayOfContactingComboRecord();
            }

            return ReturnValue;
        }

        private void UpdatePrimaryPhoneComboItems(string ANewPhoneNumberValue = null)
        {
            object[] PrimaryPhoneNumbers;
            string ThePrimaryPhoneNumber = String.Empty;
            DataView ElegiblePhoneNrsDV;
            string CurrentlySelectedPhoneNr = cmbPrimaryPhoneForContacting.GetSelectedString(-1);

            // Determine all Partner Attributes that have a Partner Attribute Type that constitutes a Phone Number
            // and that are Current.
            ElegiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute, true, false);

            PrimaryPhoneNumbers = new object[ElegiblePhoneNrsDV.Count + 1];
            PrimaryPhoneNumbers[0] = String.Empty;

            for (int Counter = 0; Counter < ElegiblePhoneNrsDV.Count; Counter++)
            {
                var ThePhoneRow = ((PPartnerAttributeRow)ElegiblePhoneNrsDV[Counter].Row);

                PrimaryPhoneNumbers[Counter + 1] = ThePhoneRow.Value;

                if (ThePhoneRow.Primary)
                {
                    ThePrimaryPhoneNumber = ThePhoneRow.Value;
                }
            }

            // Add the available Phone Numbers to the ComboBox
            cmbPrimaryPhoneForContacting.Items.Clear();
            cmbPrimaryPhoneForContacting.Items.AddRange(PrimaryPhoneNumbers);

            // Select the Primay Phone Number in the ComboBox
            if (ThePrimaryPhoneNumber != String.Empty)
            {
                FPrimaryPhoneSelectedValueChangedEvent = true;

                cmbPrimaryPhoneForContacting.SetSelectedString(ThePrimaryPhoneNumber);

                FPrimaryPhoneSelectedValueChangedEvent = false;
            }
            else
            {
                CurrentlySelectedPhoneNr = ANewPhoneNumberValue ?? CurrentlySelectedPhoneNr;

                cmbPrimaryPhoneForContacting.SetSelectedString(CurrentlySelectedPhoneNr);
            }
        }

        /// <summary>
        /// Updates the records to reflect the 'Primary Phone Number' setting.
        /// </summary>
        /// <param name="ARunValidation">If set to true, various validations are run against
        /// the 'Primary Phone Number' setting.</param>
        /// <returns>True if <paramref name="ARunValidation"/> is false, or when
        /// <paramref name="ARunValidation"/> is true and validation succeeds, otherwise false.</returns>
        public bool UpdatePrimaryPhoneNumberRecords(bool ARunValidation)
        {
            bool ReturnValue = true;
            string PrimaryPhoneChoice;
            bool PrimaryPhoneChoiceFoundAmongEligblePhoneNrs = false;
            DataView ElegiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute,
                false, false);

            PrimaryPhoneChoice = cmbPrimaryPhoneForContacting.GetSelectedString();

            if (PrimaryPhoneChoice != String.Empty)
            {
                for (int Counter = 0; Counter < ElegiblePhoneNrsDV.Count; Counter++)
                {
                    var ThePhonePartnerAttributeRow = ((PPartnerAttributeRow)ElegiblePhoneNrsDV[Counter].Row);

                    // Modify Rows only as necessary
                    if (ThePhonePartnerAttributeRow.Primary)
                    {
                        if (ThePhonePartnerAttributeRow.Value != PrimaryPhoneChoice)
                        {
                            ThePhonePartnerAttributeRow.Primary = false;
                        }
                    }
                    else
                    {
                        if (ThePhonePartnerAttributeRow.Value == PrimaryPhoneChoice)
                        {
                            ThePhonePartnerAttributeRow.Primary = true;
                        }
                    }
                }
            }
            else
            {
                for (int Counter2 = 0; Counter2 < ElegiblePhoneNrsDV.Count; Counter2++)
                {
                    var ThePhonePartnerAttributeRow = ((PPartnerAttributeRow)ElegiblePhoneNrsDV[Counter2].Row);

                    // Modify Rows only as necessary
                    if (ThePhonePartnerAttributeRow.Primary)
                    {
                        ThePhonePartnerAttributeRow.Primary = false;
                    }
                }
            }

            if (ARunValidation)
            {
                DataView CurrentPhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute,
                    true, false);

                if (CurrentPhoneNrsDV.Count != 0)
                {
                    if (PrimaryPhoneChoice != String.Empty)
                    {
                        for (int Counter3 = 0; Counter3 < ElegiblePhoneNrsDV.Count; Counter3++)
                        {
                            var ThePhonePartnerAttributeRow = ((PPartnerAttributeRow)ElegiblePhoneNrsDV[Counter3].Row);

                            if (ThePhonePartnerAttributeRow.Value == PrimaryPhoneChoice)
                            {
                                PrimaryPhoneChoiceFoundAmongEligblePhoneNrs = true;

                                if (!ThePhonePartnerAttributeRow.Current)
                                {
                                    // This condition should not occur, unless the program code that runs when the 'Valid'
                                    // CheckBox is disabled and which should clear all the Items from cmbPrimaryPhoneForContacting is
                                    // somehow not working correctly, or not run at all. This condition is therefore a 'back-stop'
                                    // that will prevent invalid data going to the DB!

                                    // Generate a Validation *Error*. The user cannot ignore this.
                                    ValidationPrimaryPhoneNrSetButItIsntCurrent();

                                    UpdatePrimaryPhoneComboItems();

                                    ReturnValue = false;
                                }
                            }
                        }

                        if (!PrimaryPhoneChoiceFoundAmongEligblePhoneNrs)
                        {
                            // This condition should not occur, unless various bits of program code are somehow
                            // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                            // that will prevent invalid data going to the DB!

                            // Generate a Validation *Error*. The user cannot ignore this.
                            ValidationPrimaryPhoneNrSetButNotAmongEmailAddrs();

                            UpdatePrimaryPhoneComboItems();

                            ReturnValue = false;
                        }
                    }
                }
                else
                {
                    if (PrimaryPhoneChoice != String.Empty)
                    {
                        // This condition should not occur, unless various bits of program code are somehow
                        // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                        // that will prevent invalid data going to the DB!

                        // Generate a Validation *Error*. The user cannot ignore this.
                        ValidationPrimaryPhoneNrSetButNoPhoneNrAvailable();

                        UpdatePrimaryPhoneComboItems();

                        ReturnValue = false;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Updates the records to reflect the 'Primary E-mail Address' setting.
        /// </summary>
        /// <param name="ARunValidation">If set to true, various validations are run against
        /// the 'Primary E-mail Address' setting.</param>
        /// <returns>True if <paramref name="ARunValidation"/> is false, or when
        /// <paramref name="ARunValidation"/> is true and validation succeeds, otherwise false.</returns>
        public bool UpdatePrimaryEmailAddressRecords(bool ARunValidation)
        {
            bool ReturnValue = true;
            string PrimaryEmailChoice;
            bool PrimaryEmailChoiceFoundAmongEligbleEmailAddr = false;
            DataView ElegibleEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute,
                false);

            PrimaryEmailChoice = cmbPrimaryEMail.GetSelectedString();

            if (PrimaryEmailChoice != String.Empty)
            {
                for (int Counter = 0; Counter < ElegibleEmailAddrsDV.Count; Counter++)
                {
                    var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter].Row);

                    // Modify Rows only as necessary
                    if (TheEmailPartnerAttributeRow.Primary)
                    {
                        if (TheEmailPartnerAttributeRow.Value != PrimaryEmailChoice)
                        {
                            TheEmailPartnerAttributeRow.Primary = false;
                        }
                    }
                    else
                    {
                        if (TheEmailPartnerAttributeRow.Value == PrimaryEmailChoice)
                        {
                            TheEmailPartnerAttributeRow.Primary = true;
                        }
                    }
                }
            }
            else
            {
                for (int Counter2 = 0; Counter2 < ElegibleEmailAddrsDV.Count; Counter2++)
                {
                    var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter2].Row);

                    // Modify Rows only as necessary
                    if (TheEmailPartnerAttributeRow.Primary)
                    {
                        TheEmailPartnerAttributeRow.Primary = false;
                    }
                }
            }

            if (ARunValidation)
            {
                DataView CurrentEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute,
                    true);

                if (CurrentEmailAddrsDV.Count != 0)
                {
                    if (PrimaryEmailChoice == String.Empty)
                    {
                        // Generate a Validation *Warning*, not an error. The user can ignore this if (s)he chooses to do so!
                        ValidationPrimaryEmailAddrNotSet();
                    }
                    else
                    {
                        for (int Counter3 = 0; Counter3 < ElegibleEmailAddrsDV.Count; Counter3++)
                        {
                            var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter3].Row);

                            if (TheEmailPartnerAttributeRow.Value == PrimaryEmailChoice)
                            {
                                PrimaryEmailChoiceFoundAmongEligbleEmailAddr = true;

                                if (!TheEmailPartnerAttributeRow.Current)
                                {
                                    // This condition should not occur, unless the program code that runs when the 'Valid'
                                    // CheckBox is disabled and which should clear all the Items from cmbPrimaryEMail is
                                    // somehow not working correctly, or not run at all. This condition is therefore a 'back-stop'
                                    // that will prevent invalid data going to the DB!

                                    // Generate a Validation *Error*. The user cannot ignore this.
                                    ValidationPrimaryEmailAddrSetButItIsntCurrent();

                                    UpdatePrimaryEmailComboItems(false);

                                    ReturnValue = false;
                                }
                            }
                        }

                        if (!PrimaryEmailChoiceFoundAmongEligbleEmailAddr)
                        {
                            // This condition should not occur, unless various bits of program code are somehow
                            // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                            // that will prevent invalid data going to the DB!

                            // Generate a Validation *Error*. The user cannot ignore this.
                            ValidationPrimaryEmailAddrSetButNotAmongEmailAddrs();

                            UpdatePrimaryEmailComboItems(true);

                            ReturnValue = false;
                        }
                    }
                }
                else
                {
                    if (PrimaryEmailChoice != String.Empty)
                    {
                        // This condition should not occur, unless various bits of program code are somehow
                        // not working correctly, or not run at all. This condition is therefore a 'back-stop'
                        // that will prevent invalid data going to the DB!

                        // Generate a Validation *Error*. The user cannot ignore this.
                        ValidationPrimaryEmailAddrSetButNoEmailAddrAvailable();

                        UpdatePrimaryEmailComboItems(true);

                        ReturnValue = false;
                    }
                }
            }

            return ReturnValue;
        }

        private void UpdatePrimaryEmailComboItems(bool ASuppressMessages, string ANewEmailAddressValue = null)
        {
            object[] PrimaryEmailAddresses;
            string ThePrimaryEmailAddress = String.Empty;
            DataView ElegibleEmailAddrsDV;
            DataView AllEmailAddrsDV;
            string CurrentlySelectedEmailAddr = cmbPrimaryEMail.GetSelectedString(-1);

            // Determine all Partner Attributes that have a Partner Attribute Type that constitutes an E-Mail
            // and that are Current.
            ElegibleEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute,
                true);

            PrimaryEmailAddresses = new object[ElegibleEmailAddrsDV.Count + 1];
            PrimaryEmailAddresses[0] = String.Empty;

            for (int Counter = 0; Counter < ElegibleEmailAddrsDV.Count; Counter++)
            {
                var TheEmailRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter].Row);

                PrimaryEmailAddresses[Counter + 1] = TheEmailRow.Value;

                if (TheEmailRow.Primary)
                {
                    ThePrimaryEmailAddress = TheEmailRow.Value;
                }
            }

            // Add the avilable E-Mail addresses to the ComboBox
            cmbPrimaryEMail.Items.Clear();
            cmbPrimaryEMail.Items.AddRange(PrimaryEmailAddresses);

            // Select the Primay Email Address in the ComboBox
            if (ThePrimaryEmailAddress != String.Empty)
            {
                FPrimaryEmailSelectedValueChangedEvent = true;

                cmbPrimaryEMail.SetSelectedString(ThePrimaryEmailAddress);

                FPrimaryEmailSelectedValueChangedEvent = false;
            }
            else
            {
                CurrentlySelectedEmailAddr = ANewEmailAddressValue ?? CurrentlySelectedEmailAddr;

                cmbPrimaryEMail.SetSelectedString(CurrentlySelectedEmailAddr);

                if (!ASuppressMessages)
                {
                    if (ElegibleEmailAddrsDV.Count > 0)
                    {
                        FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailAsNoCurrentAvailable;
                        ShowMessageBoxTimer.Start();
                    }
                    else
                    {
                        AllEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute,
                            false);

                        if (AllEmailAddrsDV.Count > 0)
                        {
                            FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailButNonCurrentAvailable;
                            ShowMessageBoxTimer.Start();
                        }
                    }
                }
            }
        }

        private void SelectPrimaryWayOfContactingComboItem()
        {
            DataView ElegibleSystemCategoryAttributesDV = Calculations.DeterminePartnerSystemCategoryAttributes(
                FMainDS.PPartnerAttribute, TSharedDataCache.TMPartner.GetSystemCategorySettingsConcatStr());
            PPartnerAttributeRow PartnerAttributeDR;
            string PrimaryContactMethod = String.Empty;
            int ComboBoxItemThatMatches;

            for (int Counter = 0; Counter < ElegibleSystemCategoryAttributesDV.Count; Counter++)
            {
                PartnerAttributeDR = (PPartnerAttributeRow)ElegibleSystemCategoryAttributesDV[Counter].Row;

                if (PartnerAttributeDR.AttributeType ==
                    Ict.Petra.Shared.MPartner.Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD)
                {
                    PrimaryContactMethod = PartnerAttributeDR.Value;
                }
            }

            if (PrimaryContactMethod != String.Empty)
            {
                ComboBoxItemThatMatches = cmbPrimaryWayOfContacting.Items.IndexOf(PrimaryContactMethod);

                if (ComboBoxItemThatMatches != -1)
                {
                    cmbPrimaryWayOfContacting.SelectedIndex = ComboBoxItemThatMatches;
                }
            }
        }

        private void UpdatePrimaryWayOfContactingComboRecord()
        {
            string CurrPrimaryWayOfContactingStr = cmbPrimaryWayOfContacting.GetSelectedString();
            string ExistingPrimContactMethStr = String.Empty;

            DataRow[] ExistingPrimContactMethArr = FMainDS.PPartnerAttribute.Select(
                PPartnerAttributeTable.GetAttributeTypeDBName() + " = '" +
                Ict.Petra.Shared.MPartner.Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD + "'");
            PPartnerAttributeRow NewAttributeRow;

            // Check if a p_partner_attribute record exists which holds the information about this Partners' 'Primary Way of Contacting'
            if (ExistingPrimContactMethArr.Length != 0)
            {
                // There must always be only one such record, so we can simply pick the first record in the Array
                ExistingPrimContactMethStr = ((PPartnerAttributeRow)ExistingPrimContactMethArr[0]).Value;
            }

            if (ExistingPrimContactMethStr != String.Empty)
            {
                if (ExistingPrimContactMethStr != CurrPrimaryWayOfContactingStr)
                {
                    if (CurrPrimaryWayOfContactingStr != String.Empty)
                    {
                        // Update the existing record with the new 'Primary Way of Contacting' selection
                        ((PPartnerAttributeRow)ExistingPrimContactMethArr[0]).Value = CurrPrimaryWayOfContactingStr;
                    }
                    else
                    {
                        // If the user chose to clear the 'Primary Way of Contacting': delete the record
                        ExistingPrimContactMethArr[0].Delete();
                    }
                }
            }
            else
            {
                if (CurrPrimaryWayOfContactingStr != String.Empty)
                {
                    // We need to add a record that holds the 'Primary Way of Contacting' selection as there isn't one yet for this Partner
                    NewAttributeRow = FMainDS.PPartnerAttribute.NewRowTyped(true);
                    NewAttributeRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                    NewAttributeRow.AttributeType =
                        Ict.Petra.Shared.MPartner.Calculations.ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD;
                    NewAttributeRow.Sequence = -1;
                    NewAttributeRow.Index = 9999;
                    NewAttributeRow.Value = CurrPrimaryWayOfContactingStr;
                    NewAttributeRow.Current = true;

                    FMainDS.PPartnerAttribute.Rows.Add(NewAttributeRow);
                }
            }
        }

        private void BeforeShowDetailsManual(PPartnerAttributeRow ARow)
        {
            FRunningInsideShowDetails = true;

            if (ARow != null)
            {
                btnDelete.Enabled = true;

                if (FMainDS.PPartnerAttributeType != null)
                {
                    DataRow[] ParnterAttributeRow = FMainDS.PPartnerAttributeType.Select(
                        PPartnerAttributeTypeTable.GetAttributeTypeDBName() + " = " + "'" + ARow.AttributeType + "'");

                    if (ParnterAttributeRow.Length > 0)
                    {
                        SelectCorrespondingCategory((PPartnerAttributeTypeRow)ParnterAttributeRow[0]);
                    }

//                    cmbContactType.SetSelectedString(ARow.AttributeType, -1);
                }
            }
            else
            {
                cmbContactCategory.SelectedIndex = 0;
            }

            cmbContactCategory.Enabled = (ARow.RowState == DataRowState.Added);
        }

        private void CreateFilterFindPanelsManual()
        {
            // Create custom data columns on-the-fly
            Calculations.CreateCustomDataColumnsForAttributeTable(FMainDS.PPartnerAttribute, FMainDS.PPartnerAttributeType);

            /* Create SourceDataGrid columns */
            CreateGridColumns();

            /* Setup the DataGrid's visual appearance */
//            SetupDataGridVisualAppearance();

            string FilterStr = String.Format("{0}='{1}'", PartnerEditTDSPPartnerAttributeTable.GetPartnerContactDetailDBName(), true);

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FilterStr, true);
            FFilterAndFindObject.ApplyFilter();
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeRow ARow)
        {
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRecord(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRecord(this, null);
            }
        }

        /// <summary>
        /// adding a new partner relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            if (CreateNewPPartnerAttribute())
            {
                txtValue.Focus();
            }

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            DoRecalculateScreenParts();
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PartnerEditTDSPPartnerAttributeRow ARow)
        {
            int LeastSequence = 0;
            int HighestIndex = 0;
            int ThisRow_Sequence = 0;
            int ThisRow_Index = 0;

            System.Data.DataRowVersion ThisRow_RowVersion;
            PPartnerAttributeTable ThisDT = (PPartnerAttributeTable)ARow.Table;

            ARow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
            ARow.AttributeType = FDefaultContactType;

            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++)
            {
                if (ThisDT.Rows[Counter].RowState == DataRowState.Deleted)
                {
                    ThisRow_RowVersion = DataRowVersion.Original;
                }
                else
                {
                    ThisRow_RowVersion = DataRowVersion.Current;
                }

                ThisRow_Sequence = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetSequenceDBName(), ThisRow_RowVersion]);

                if (ThisRow_Sequence < LeastSequence)
                {
                    LeastSequence = ThisRow_Sequence;
                }
            }

            ARow.Sequence = LeastSequence - 1;

            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++)
            {
                if (ThisDT.Rows[Counter].RowState == DataRowState.Deleted)
                {
                    ThisRow_RowVersion = DataRowVersion.Original;
                }
                else
                {
                    ThisRow_RowVersion = DataRowVersion.Current;
                }

                ThisRow_Index = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetIndexDBName(), ThisRow_RowVersion]);

                if (ThisRow_Index > HighestIndex)
                {
                    HighestIndex = ThisRow_Index;
                }
            }

            ARow.Index = HighestIndex + 1;

            ARow.Value = "NEWVALUE" + ARow.Sequence.ToString();
            ARow.Primary = false;
            ARow.WithinOrgansiation = false;
            ARow.Specialised = false;
            ARow.Confidential = false;
            ARow.Current = true;
            ARow.PartnerContactDetail = true;

            // If this is the first time the user created a new record then the Contact Type ComboBox
            // hasn't been filtered yet and would display all Contact Types of all Contact Categories.
            // To prevent that we need to initialise the Filter.
            if (cmbContactType.Filter == null)
            {
                FilterContactTypeCombo(null, null);
            }
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            // Before the auto-generated Method for deletion can be called we need to set the Expressions that refer to 'Parent.*' to "",
            // otherwise we get a 'System.IndexOutOfRangeException: Cannot find relation 0.' Exception when the DataTable gets deserialised
            // on the server side in the call to Method 'GetNonCacheableRecordReferenceCount'!
            // Confer http://bytes.com/topic/asp-net/answers/323437-cannot-file-relation-0-a
            foreach (DataColumn RemoveExpressionsColumn in FMainDS.PPartnerAttribute.Columns)
            {
                if (RemoveExpressionsColumn.Expression.Length != 0)
                {
                    // TLogging.Log(RemoveExpressionsColumn.Expression, [ToLogFile]);
                    RemoveExpressionsColumn.Expression = "";
                }
            }

            this.DeletePPartnerAttribute();

            // Restore Column Expressions again!
            Calculations.SetColumnExpressions(FMainDS.PPartnerAttribute);
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PPartnerAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            // Those values are getting safed for use in the PostDeleteManual Method
            // Trying to establish those values there doesn't work in case the Partner was new,
            // the Row was newly added, and then gets removed (DataRow has DataRowVersion.Detached in
            // this case, and no Original data that can be accessed!)
            FDeletedRowsAttributeType = ARowToDelete.AttributeType;
            FDeletedRowsValue = ARowToDelete.Value;

            ADeletionQuestion =
                String.Format(Catalog.GetString(
                        "Are you sure you want to delete the following Contact Detail record?\r\n\r\n    Type: '{0}'\r\n    Value: '{1}'"),
                    ARowToDelete.AttributeType, ARowToDelete.Value);

            return true;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(PPartnerAttributeRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            // no message to be shown after deletion
            ACompletionMessage = "";

            try
            {
                ARowToDelete.Delete();
                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process,
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted,</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete,</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully,</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message,</param>
        private void PostDeleteManual(PPartnerAttributeRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            bool NoEmailAddressesAvailableAnymore = false;
            bool NoPhoneNumbersAvailableAnymore = false;
            string PrimEmailConsequence = StrPrimEmailConsequence1;
            string PrimEmailMessageBoxTitle = StrPrimEmailMessageBoxTitle1;
            string PrimPhoneConsequence = StrPrimPhoneConsequence1;
            string PrimPhoneMessageBoxTitle = StrPrimPhoneMessageBoxTitle1;

            if (ADeletionPerformed)
            {
                StringCollection EmailAttrColl = StringHelper.StrSplit(
                    TSharedDataCache.TMPartner.GetEmailPartnerAttributesConcatStr(), ", ");

                if (EmailAttrColl.Contains("'" + FDeletedRowsAttributeType + "'"))
                {
                    // User deleted an E-mail Contact Detail

                    if (cmbPrimaryEMail.GetSelectedString(-1) == FDeletedRowsValue)
                    {
                        DataView ElegibleEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(
                            FMainDS.PPartnerAttribute, true);

                        if (ElegibleEmailAddrsDV.Count == 0)
                        {
                            NoEmailAddressesAvailableAnymore = true;
                            PrimEmailConsequence = StrPrimEmailConsequence2;
                            PrimEmailMessageBoxTitle = StrPrimEmailMessageBoxTitle2;
                        }

                        // User deleted the E-mail Contact Detail that was set as the 'Primary E-Mail Address':
                        // Refresh the Primary E-Mail Address Combo (which upon that will no longer contain the E-mail
                        // Address of the deleted Contact Detail!) and notify the user that (s)he needs to take action.
                        UpdatePrimaryEmailComboItems(true);

                        MessageBox.Show(
                            String.Format(
                                Catalog.GetString("You have deleted the Contact Detail that was set as the 'Primary E-Mail Address'.\r\n\r\n{0}"),
                                PrimEmailConsequence),
                            PrimEmailMessageBoxTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (!NoEmailAddressesAvailableAnymore)
                        {
                            // Show the other current e-mail-Address(es) to the user
                            cmbPrimaryEMail.DroppedDown = true;
                        }
                    }
                    else
                    {
                        // User deleted a E-mail Contact Detail that was not set as the 'Primary E-Mail Address':
                        // Simply refresh the Primary E-Mail Address Combo
                        UpdatePrimaryEmailComboItems(true);
                    }
                }

                StringCollection PhoneAttrColl = StringHelper.StrSplit(
                    TSharedDataCache.TMPartner.GetPhonePartnerAttributesConcatStr(), ", ");

                if (PhoneAttrColl.Contains("'" + FDeletedRowsAttributeType + "'"))
                {
                    // User deleted a Phone Contact Detail

                    if (cmbPrimaryPhoneForContacting.GetSelectedString(-1) == FDeletedRowsValue)
                    {
                        DataView ElegiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute,
                            true, false);

                        if (ElegiblePhoneNrsDV.Count == 0)
                        {
                            NoPhoneNumbersAvailableAnymore = true;
                            PrimPhoneConsequence = StrPrimPhoneConsequence2;
                            PrimPhoneMessageBoxTitle = StrPrimPhoneMessageBoxTitle2;
                        }

                        // User deleted the Phone Contact Detail that was set as the 'Primary Phone':
                        // Refresh the Primary Phone Combo (which upon that will no longer contain the Phone
                        // Number of the deleted Contact Detail!) and notify the user that (s)he needs to take action.
                        UpdatePrimaryPhoneComboItems();

                        MessageBox.Show(
                            String.Format(
                                Catalog.GetString("You have deleted the Contact Detail that was set as the 'Primary Phone'.\r\n\r\n{0}"),
                                PrimPhoneConsequence),
                            PrimPhoneMessageBoxTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (!NoPhoneNumbersAvailableAnymore)
                        {
                            // Show the other current Phone Number(s) to the user
                            cmbPrimaryPhoneForContacting.DroppedDown = true;
                        }
                    }
                    else
                    {
                        // User deleted a Phone Contact Detail that was not set as the 'Primary Phone':
                        // Simply refresh the Primary Phone Combo
                        UpdatePrimaryPhoneComboItems();
                    }
                }

                DoRecalculateScreenParts();

                if (grdDetails.Rows.Count <= 1)
                {
                    // hide details part and disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                    pnlDetails.Visible = false;
                }
            }

            FDeletedRowsAttributeType = String.Empty;
            FDeletedRowsValue = String.Empty;
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        private void MakeDetailsInvisible(Boolean value)
        {
            /* make the details part of this screen visible or invisible. */
            this.pnlDetails.Visible = !value;
        }

        /// <summary>
        /// Creates DataBound columns for the Grid control.
        /// </summary>
        /// <returns>void</returns>
        public void CreateGridColumns()
        {
            // Get rid of the Columns as added per YAML file as we need to show calculated Columns!
            grdDetails.Columns.Clear();

            grdDetails.AddImageColumn(@GetPrimaryIconForGridRow);

            //
            // Contact Type
//            grdDetails.AddTextColumn("Type Code", FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()]);

            //
            // Contact Type (Calculated Expression!)
            grdDetails.AddTextColumn("Contact Type", FMainDS.PPartnerAttribute.Columns[Calculations.CALCCOLUMNNAME_CONTACTTYPE]);

            // Comment
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerAttribute.ColumnComment);

            // Value
            grdDetails.AddTextColumn("Value", FMainDS.PPartnerAttribute.ColumnValue);

            // Current
            grdDetails.AddCheckBoxColumn("Current", FMainDS.PPartnerAttribute.ColumnCurrent);

            // Confidential
            grdDetails.AddCheckBoxColumn("Confidential", FMainDS.PPartnerAttribute.ColumnConfidential);

//            // Sequence (for testing purposes only...)
//            grdDetails.AddTextColumn("Sequence", FMainDS.PPartnerAttribute.ColumnSequence);
//
//            // Index (for testing purposes only...)
//            grdDetails.AddTextColumn("Index", FMainDS.PPartnerAttribute.ColumnIndex);
//
//            // Primary (for testing purposes only...)
//            grdDetails.AddCheckBoxColumn("Primary", FMainDS.PPartnerAttribute.ColumnPrimary);
//
//            // Within Organsiation (for testing purposes only...)
//            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
//            {
//
//                grdDetails.AddCheckBoxColumn("Within Org.", FMainDS.PPartnerAttribute.ColumnWithinOrgansiation);
//            }

            // Modification TimeStamp (for testing purposes only...)
//             grdDetails.AddTextColumn("Modification TimeStamp", FMainDS.PPartnerAttribute.ColumnModificationId);
        }

        private void ValidateDataDetailsManual(PPartnerAttributeRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateContactDetailsManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, FValueKind);
        }

        #endregion

        #region Event Handlers

        private void ContactTypePromote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Promotion of Contact Types not implemented yet!");
        }

        private void ContactTypeDemote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Demotion of Contact Types not implemented yet!");
        }

        private void LaunchHyperlinkPrefEMail(object sender, EventArgs e)
        {
            // TODO Replace this 'quick solution' when the txtValue Control is replaced with the proper rtbValue Control!
            TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

            Launcher.LaunchHyperLink(cmbPrimaryEMail.GetSelectedString(), "||email||");
        }

        private void LaunchHyperlinkEMailWithinOrg(object sender, EventArgs e)
        {
            // TODO Replace this 'quick solution' when the txtValue Control is replaced with the proper rtbValue Control!
            TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

            Launcher.LaunchHyperLink(cmbPrimaryEMail.GetSelectedString(), "||email||");
        }

        private void LaunchHyperlinkFromValue(object sender, EventArgs e)
        {
            TRtbHyperlinks TempHyperlinkCtrl = new TRtbHyperlinks();
            string LinkType = String.Empty;

            // TODO Replace this 'quick solution' when the txtValue Control is replaced with the proper rtbValue Control!
            TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(TempHyperlinkCtrl);

            switch (FValueKind)
            {
                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                    LinkType = THyperLinkHandling.HYPERLINK_PREFIX_URLLINK;
                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                    TempHyperlinkCtrl.BuildLinkWithValue = BuildLinkWithValue;
                    LinkType = THyperLinkHandling.HYPERLINK_PREFIX_URLWITHVALUELINK;
                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                    LinkType = THyperLinkHandling.HYPERLINK_PREFIX_EMAILLINK;
                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                    LinkType = THyperLinkHandling.HYPERLINK_PREFIX_SKYPELINK;
                    break;
            }

            Launcher.LaunchHyperLink(txtValue.Text, LinkType);
        }

        private void FilterCriteriaChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException("Filtering is not implemented yet!");
        }

        private void HandleCurrentFlagChanged(Object sender, EventArgs e)
        {
            bool PrimaryEmailAddressIsThisRecord = false;
            bool PrimaryPhoneNumberIsThisRecord = false;

            // Ensure current Checked state is reflected in the DataRow
            GetSelectedDetailRow().Current = chkCurrent.Checked;

            dtpNoLongerCurrentFrom.Enabled = !chkCurrent.Checked;

            if (!FRunningInsideShowDetails)
            {
                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    if (cmbPrimaryEMail.GetSelectedString() == txtValue.Text)
                    {
                        PrimaryEmailAddressIsThisRecord = true;
                    }

                    // Refresh the ComboBox so it shows only the 'current' E-Mail Address records (which could possibly be none!)
                    UpdatePrimaryEmailComboItems(true);
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (RowHasPhoneAttributeType(GetSelectedDetailRow()))
                    {
                        if (cmbPrimaryPhoneForContacting.GetSelectedString() == txtValue.Text)
                        {
                            PrimaryPhoneNumberIsThisRecord = true;
                        }

                        // Refresh the ComboBox so it shows only the 'current' Phone Number records (which could possibly be none!)
                        UpdatePrimaryPhoneComboItems();
                    }
                }
            }

            if (!chkCurrent.Checked)
            {
                if (!grdDetails.Focused)
                {
                    dtpNoLongerCurrentFrom.Date = DateTime.Now.Date;
                    dtpNoLongerCurrentFrom.Focus();
                }

                if (!FRunningInsideShowDetails)
                {
                    if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                    {
                        CheckThatNonCurrentEmailAddressIsntPrimaryEmailAddr(PrimaryEmailAddressIsThisRecord);
                    }
                    else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                    {
                        if (RowHasPhoneAttributeType(GetSelectedDetailRow()))
                        {
                            CheckThatNonCurrentPhoneNrIsntPrimaryPhoneNr(PrimaryPhoneNumberIsThisRecord);
                        }
                    }
                }
            }
            else
            {
                dtpNoLongerCurrentFrom.Date = null;
            }

            if (!FRunningInsideShowDetails)
            {
                DoRecalculateScreenParts();
            }
        }

        private void HandleSpecialisedFlagChanged(Object sender, EventArgs e)
        {
            // Ensure current Checked state is reflected in the DataRow
            GetSelectedDetailRow().Specialised = chkSpecialised.Checked;
        }

        private void HandleConfidentialFlagChanged(Object sender, EventArgs e)
        {
            // Ensure current Checked state is reflected in the DataRow
            GetSelectedDetailRow().Confidential = chkConfidential.Checked;

            // TODO Implement 'confidential functionality'
        }

        private void HandleValueLeave(Object sender, EventArgs e)
        {
            var SelectedDetailDR = GetSelectedDetailRow();

            if ((!FRunningInsideShowDetails)
                && (SelectedDetailDR.RowState != DataRowState.Detached))
            {
                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    // Ensure current E-mail Address is reflected in the DataRow
                    SelectedDetailDR.Value = txtValue.Text;

                    // Refresh the ComboBox so it reflects any change in the E-mail Address!
                    UpdatePrimaryEmailComboItems(true,
                        (
                            (cmbPrimaryEMail.SelectedIndex != 0)
                            && (
                                FValueWithSpecialMeaningChangedButUserDidntLeaveControl || !FRunningInsideDataSaving
                                )
                        ) ? txtValue.Text : null);

                    UpdatePrimaryPhoneComboItems();

                    FValueWithSpecialMeaningChangedButUserDidntLeaveControl = false;
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (RowHasPhoneAttributeType(SelectedDetailDR))
                    {
                        // Ensure current Phone Number is reflected in the DataRow
                        SelectedDetailDR.Value = txtValue.Text;

                        // Refresh the ComboBox so it reflects any change in the Phone Number!
                        UpdatePrimaryPhoneComboItems(
                            (
                                (cmbPrimaryPhoneForContacting.SelectedIndex != 0)
                                && (
                                    FValueWithSpecialMeaningChangedButUserDidntLeaveControl || !FRunningInsideDataSaving
                                    )
                            ) ? txtValue.Text : null);

                        UpdatePrimaryEmailComboItems(true);

                        FValueWithSpecialMeaningChangedButUserDidntLeaveControl = false;
                    }
                }
            }

            btnLaunchHyperlinkFromValue.Enabled = (txtValue.Text != String.Empty);
        }

        private void HandleValueChanged(Object sender, EventArgs e)
        {
            btnLaunchHyperlinkFromValue.Enabled = (txtValue.Text != String.Empty);
        }

        private bool RowHasPhoneAttributeType(PPartnerAttributeRow ADetailRow)
        {
            bool ReturnValue = false;

            for (int Counter = 0; Counter < FPhoneAttributesDV.Count; Counter++)
            {
                if (ADetailRow.AttributeType == ((PPartnerAttributeTypeRow)FPhoneAttributesDV[Counter].Row).AttributeType)
                {
                    ReturnValue = true;
                    break;
                }
            }

            return ReturnValue;
        }

        private void HandlePrimaryPhoneSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FPrimaryPhoneSelectedValueChangedEvent)
            {
                UpdatePrimaryPhoneNumberRecords(false);
            }
        }

        private void HandlePrimaryEmailSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FPrimaryEmailSelectedValueChangedEvent)
            {
                UpdatePrimaryEmailAddressRecords(false);
            }

            btnLaunchHyperlinkPrefEMail.Enabled = (cmbPrimaryEMail.Text != String.Empty);
        }

        private void CheckThatNonCurrentPhoneNrIsntPrimaryPhoneNr(bool APrimaryPhoneNumberIsThisRecord)
        {
            bool NoPhoneNumbersAvailableAnymore = false;
            string PrimPhoneConsequence = StrPrimPhoneConsequence1;
            string PrimPhoneMessageBoxTitle = StrPrimPhoneMessageBoxTitle1;

            if (APrimaryPhoneNumberIsThisRecord)
            {
                DataView ElegiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute,
                    true, false);

                if (ElegiblePhoneNrsDV.Count == 0)
                {
                    NoPhoneNumbersAvailableAnymore = true;
                    PrimPhoneConsequence = StrPrimPhoneConsequence2;
                    PrimPhoneMessageBoxTitle = StrPrimPhoneMessageBoxTitle2;
                }

                // Select the 'empty' ComboBox Item
                cmbPrimaryPhoneForContacting.SelectedIndex = 0;

                MessageBox.Show(
                    String.Format(
                        Catalog.GetString(
                            "You have made the Phone Number no longer current, but up till now it was set to be the Primary Phone.\r\n\r\n{0}"),
                        PrimPhoneConsequence),
                    PrimPhoneMessageBoxTitle,
                    MessageBoxButtons.OK, NoPhoneNumbersAvailableAnymore ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                if (!NoPhoneNumbersAvailableAnymore)
                {
                    // Show the other current Phone Number(s) to the user
                    cmbPrimaryPhoneForContacting.DroppedDown = true;
                }
            }
        }

        private void CheckThatNonCurrentEmailAddressIsntPrimaryEmailAddr(bool APrimaryEmailAddressIsThisRecord)
        {
            bool NoEmailAddressesAvailableAnymore = false;
            string PrimEmailConsequence = StrPrimEmailConsequence1;
            string PrimEmailMessageBoxTitle = StrPrimEmailMessageBoxTitle1;

            if (APrimaryEmailAddressIsThisRecord)
            {
                DataView ElegibleEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(FMainDS.PPartnerAttribute,
                    true);

                if (ElegibleEmailAddrsDV.Count == 0)
                {
                    NoEmailAddressesAvailableAnymore = true;
                    PrimEmailConsequence = StrPrimEmailConsequence2;
                    PrimEmailMessageBoxTitle = StrPrimEmailMessageBoxTitle2;
                }

                // Select the 'empty' ComboBox Item
                cmbPrimaryEMail.SelectedIndex = 0;

                MessageBox.Show(
                    String.Format(
                        Catalog.GetString(
                            "You have made the E-Mail Address no longer current, but up till now it was set to be the Primary E-Mail Address.\r\n\r\n{0}"),
                        PrimEmailConsequence),
                    PrimEmailMessageBoxTitle,
                    MessageBoxButtons.OK, NoEmailAddressesAvailableAnymore ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                if (!NoEmailAddressesAvailableAnymore)
                {
                    // Show the other current e-mail-Address(es) to the user
                    cmbPrimaryEMail.DroppedDown = true;
                }
            }
        }

        private void FilterContactTypeCombo(Object sender, EventArgs e)
        {
//            if (!FRunningInsideShowDetails)
//            {
            if (cmbContactCategory.Text != String.Empty)
            {
                FSuppressOnContactTypeChangedEvent = true;

                cmbContactType.Filter = PPartnerAttributeTypeTable.GetCategoryCodeDBName() + " = '" + cmbContactCategory.Text + "'";

                FSuppressOnContactTypeChangedEvent = false;

                // Select the first item in the ComboBox
                if (cmbContactType.Count > 0)
                {
                    cmbContactType.SelectedIndex = 0;
                }

                OnContactTypeChanged(null, null);
            }

//            }
        }

        private void OnContactTypeChanged(Object sender, EventArgs e)
        {
            PPartnerAttributeTypeRow ContactTypeDR;
            TPartnerAttributeTypeValueKind PreviousValueKind = FValueKind;
            TPartnerAttributeTypeValueKind ValueKind;

            if ((!FSuppressOnContactTypeChangedEvent)
                && (cmbContactType.Text != String.Empty))
            {
                ContactTypeDR = (PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow();

                if (!FRunningInsideShowDetails)
                {
                    // If the user created a new Record and changes Attribute Types: Make sure that the change in the Attribute Type is effected
                    // in the Record immediately, and not just when the user leaves a Control that gets Validated. If this isn't done then the
                    // 'Overall Contact Settings' ComboBoxes whose Items are based on the rows' Attribute Types won't get updated immediately.
                    GetSelectedDetailRow().AttributeType = cmbContactType.GetSelectedString();
                }

                SelectCorrespondingCategory(ContactTypeDR);

                if (Enum.TryParse <TPartnerAttributeTypeValueKind>(ContactTypeDR.AttributeTypeValueKind, out ValueKind))
                {
                    FValueKind = ValueKind;

                    switch (FValueKind)
                    {
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:

                            btnLaunchHyperlinkFromValue.Visible = false;
                            txtValue.Width = 290;

                            break;

                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                            btnLaunchHyperlinkFromValue.Visible = true;
                            txtValue.Width = 256;

                            break;

                        default:
                            btnLaunchHyperlinkFromValue.Visible = false;
                            txtValue.Width = 270;

                            throw new Exception("Invalid value for TPartnerAttributeTypeValueKind");
                    }
                }
                else
                {
                    // Fallback!
                    FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;
                }

                UpdateValueManual();

                if (!FRunningInsideShowDetails)
                {
                    if (PreviousValueKind != FValueKind)
                    {
                        if (GetSelectedDetailRow().Primary)
                        {
                            GetSelectedDetailRow().Primary = false;

                            UpdatePrimaryEmailComboItems(true);
                            UpdatePrimaryPhoneComboItems();

                            if (PreviousValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                            {
                                MessageBox.Show(Catalog.GetString(
                                        "You have changed the Contact Type and the Contact Detail that was an E-Mail address is no longer one.\r\n" +
                                        "As a result, this Contact Detail can no longer be the Primary E-Mail address! It has therefore been removed from the Primary E-Mail choices."),
                                    Catalog.GetString("No Longer Primary E-Mail"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show(Catalog.GetString(
                                        "You have changed the Contact Type and the Contact Detail that was a Phone Number is no longer one.\r\n" +
                                        "As a result, this Contact Detail can no longer be the Primary Phone! It has therefore been removed from the Primary Phone choices."),
                                    Catalog.GetString("No Longer Primary Phone"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            UpdatePrimaryEmailComboItems(true);
                            UpdatePrimaryPhoneComboItems();
                        }
                    }
                    else
                    {
                        // This might seem unnecessary as the 'PreviousValueKind' and 'FValueKind' are the same -
                        // However, it isn't, as 'Fax Numbers' are to be excluded from the 'Primary Phone' ComboBox but
                        // if a user changes between 'Phone' and 'Fax' there is no difference between 'PreviousValueKind'
                        // and 'FValueKind' and yet we need to update the Combo!
                        UpdatePrimaryPhoneComboItems();

                        // TODO Also detect if GetSelectedDetailRow().Primary = true and set it to 'false' if Fax was chosen
                        // and show message to the user as above.
                    }
                }
            }
        }

        private void UpdateValueManual()
        {
            var CurrentRow = GetSelectedDetailRow();

            if (CurrentRow == null)
            {
                return;
            }

            string Value = CurrentRow.Value;
//            string ValueText = String.Empty;

            switch (FValueKind)
            {
                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter whatever the Contact Type is about.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));

                    // TODO UpdateValueManual / CONTACTDETAIL_GENERAL
                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter Hyperlink / URL.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));
                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    Catalog.GetString("Click this button open the hyperlink in a web browser."));

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURL(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter value that becomes part of the Hyperlink / URL.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));
                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    Catalog.GetString("Click this button open the hyperlink in a web browser."));

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURLWithValue(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter E-Mail Address.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));
                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    Catalog.GetString("Click this button to send an email to the E-mail address (with your standard e-mail program)."));

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayEmailAddress(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter Skype ID.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));
                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    Catalog.GetString("Click this button to initate a Skype call, calling the Skype ID."));

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplaySkypeID(Value);

                    break;

                default:
                    throw new Exception("Invalid value for TPartnerAttributeTypeValueKind");
            }
        }

        private void SelectCorrespondingCategory(PPartnerAttributeTypeRow ARow)
        {
            int SelectionCounter = 0;

            foreach (DataRowView Drv in cmbContactCategory.Table.DefaultView)
            {
                if (((PPartnerAttributeCategoryRow)(Drv.Row)).CategoryCode == ARow.CategoryCode)
                {
                    break;
                }

                SelectionCounter++;
            }

            cmbContactCategory.SelectedIndex = SelectionCounter;
        }

        /// <summary>
        /// Constructs a valid URL string from a Value that is of a Contact Type that has got a Hyperlink Format set up.
        /// </summary>
        /// <param name="AValue">Value that should replace THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER in the Hyperlink Format strin</param>
        /// <param name="ALinkEnd">Character position of the end of the Link. Used for distinguishing different Links with the
        /// same Link in case of <see cref="THyperLinkHandling.THyperLinkType.Http_With_Value_Replacement"/>.</param>
        /// <returns>URL with the Value replacing THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER.</returns>
        private string BuildLinkWithValue(string AValue, int ALinkEnd)
        {
            return Calculations.BuildLinkWithValue(AValue, FValueKind,
                ((PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow()).HyperlinkFormat);
        }

        /// <summary>
        /// Called from a timer, ShowMessageBoxTimer, so that the FocusRowLeaving Event processing can
        /// complete before the MessageBox is show (would the MessageBox be shown while the Event gets
        /// processed the Grid would get into a strange state in which mouse moves would cause the Grid
        /// to scroll!).
        /// </summary>
        /// <param name="Sender">Gets evaluated to make sure a Timer is calling this Method.</param>
        /// <param name="e">Ignored.</param>
        private void ShowTimerDrivenMessageBox(Object Sender, EventArgs e)
        {
            System.Windows.Forms.Timer SendingTimer = Sender as System.Windows.Forms.Timer;

            if (SendingTimer != null)
            {
                // I got called from a Timer: stop that now so that the following MessageBox gets shown only once!
                SendingTimer.Stop();

                switch (FTimerDrivenMessageBoxKind)
                {
                    case TUC_ContactDetails.TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailAsNoCurrentAvailable:
                        MessageBox.Show(Catalog.GetString(
                            "No Primary Email Address has been chosen for this Partner, although the Partner has at least one current Email Address on record.\r\n\r\n"
                            +
                            "IMPORTANT: OpenPetra can't send emails to this Partner in automated situations unless a Primary Email Address has been chosen!\r\n"
                            +
                            "Therefore, please choose an Email Address from the Primary Email Address setting,"),
                        Catalog.GetString("No Primary Email Address Set!"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        cmbPrimaryEMail.Focus();
                        cmbPrimaryEMail.DroppedDown = true;

                        break;

                    case TUC_ContactDetails.TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailButNonCurrentAvailable:
                        // Adjust the Filter so that non-Valid records are shown, too, and expand the Filter Panel
                        ((CheckBox)FFilterAndFindObject.FilterPanelControls.FStandardFilterPanels[0].PanelControl).CheckState =
                            CheckState.Indeterminate;
                        FFilterAndFindObject.ToggleFilter();

                        MessageBox.Show(Catalog.GetString(
                            "No Primary Email Address has been chosen for this Partner.\r\n\r\nThere are non-current Email Addresses on record - the Filter has been\r\n"
                            +
                            "set up for you so those can be seen. You might want to check whether a current email address is available for this Partner."),
                        Catalog.GetString("No Primary Email Address Set - No Current Email Address"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    default:
                        throw new Exception("Invalid value for TTimerDrivenMessageBoxKind");
                }
            }
        }

        #endregion

        #region Data Validation

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryPhoneForContacting.
        /// </summary>
        private void ValidationPrimaryPhoneNrSetButNoPhoneNrAvailable()
        {
            const string ResCont = "ContactDetails_PrimaryPhone_Set_But_No_Phone_Nr_Available";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryPhoneForContacting, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryPhoneForContacting.
        /// </summary>
        private void ValidationPrimaryPhoneNrSetButItIsntCurrent()
        {
            const string ResCont = "ContactDetails_PrimaryPhone_Set_But_It_Isnt_Current";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryPhoneForContacting, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryPhoneForContacting.
        /// </summary>
        private void ValidationPrimaryPhoneNrSetButNotAmongEmailAddrs()
        {
            const string ResCont = "ContactDetails_PrimaryPhone_Set_But_Not_Among_Phone_Nrs";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryPhoneForContacting, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Warning* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrNotSet()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Not_Set";
            TScreenVerificationResult VerificationResult;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_NOT_SET_DESIPITE_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButNoEmailAddrAvailable()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_No_Email_Addr_Available";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButItIsntCurrent()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_It_Isnt_Current";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButNotAmongEmailAddrs()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_Not_Among_Email_Addrs";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        #endregion

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        //// Special Handlers for menus and command keys for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (cmbContactCategory.Enabled)
            {
                if (((keyData == (Keys.F5))
                     || (keyData == (Keys.F5 | Keys.Shift))))
                {
                    if (keyData == (Keys.F5 | Keys.Shift))
                    {
                        // Select 'Mobile Phone'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Phone");
                        cmbContactType.cmbCombobox.SetSelectedString("Mobile Phone");
                    }
                    else
                    {
                        // Select 'Phone' (=landline)
                        cmbContactCategory.cmbCombobox.SetSelectedString("Phone");
                        cmbContactType.cmbCombobox.SetSelectedString("Phone");
                    }

                    // Effect a Value-leave event to ensure that any change in the Value gets reflected everywhere
                    txtComment.Focus();
                    txtValue.Focus();

                    return true;
                }

                if (((keyData == (Keys.F6))
                     || (keyData == (Keys.F6 | Keys.Shift))))
                {
                    if (keyData == (Keys.F6 | Keys.Shift))
                    {
                        // Select 'Secure E-Mail'
                        cmbContactCategory.cmbCombobox.SetSelectedString("E-Mail");
                        cmbContactType.cmbCombobox.SetSelectedString("Secure E-Mail");
                    }
                    else
                    {
                        // Select 'E-Mail'
                        cmbContactCategory.cmbCombobox.SetSelectedString("E-Mail");
                        cmbContactType.cmbCombobox.SetSelectedString("E-Mail");
                    }

                    // Effect a Value-leave event to ensure that any change in the Value gets reflected everywhere
                    txtComment.Focus();
                    txtValue.Focus();

                    return true;
                }

                if (((keyData == (Keys.F7))
                     || (keyData == (Keys.F7 | Keys.Shift))))
                {
                    if (keyData == (Keys.F7 | Keys.Shift))
                    {
                        // Select 'Twitter'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Digital Media");
                        cmbContactType.cmbCombobox.SetSelectedString("Twitter");
                    }
                    else
                    {
                        // Select 'Web Site'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Digital Media");
                        cmbContactType.cmbCombobox.SetSelectedString("Web Site");
                    }

                    // Effect a Value-leave event to ensure that any change in the Value gets reflected everywhere
                    txtComment.Focus();
                    txtValue.Focus();

                    return true;
                }

                if (((keyData == (Keys.F8))
                     || (keyData == (Keys.F8 | Keys.Shift))))
                {
                    if (keyData == (Keys.F8 | Keys.Shift))
                    {
                        // Select 'Lync'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Instant Messaging & Chat");
                        cmbContactType.cmbCombobox.SetSelectedString("Lync");
                    }
                    else
                    {
                        // Select 'Skype'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Instant Messaging & Chat");
                        cmbContactType.cmbCombobox.SetSelectedString("Skype");
                    }

                    // Effect a Value-leave event to ensure that any change in the Value gets reflected everywhere
                    txtComment.Focus();
                    txtValue.Focus();

                    return true;
                }
            }

            if (keyData == (Keys.F9))
            {
                LaunchHyperlinkPrefEMail(null, null);
            }

            if (keyData == (Keys.F10))
            {
                if (btnLaunchHyperlinkEMailWithinOrg.Visible)
                {
                    LaunchHyperlinkEMailWithinOrg(null, null);
                }
            }

            if (keyData == (Keys.F11))
            {
                if (btnLaunchHyperlinkFromValue.Visible)
                {
                    LaunchHyperlinkFromValue(null, null);
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Determines the Primary Icon (displayed in the first Column of the Grid).
        /// </summary>
        /// <param name="ARow">Grid Row.</param>
        /// <returns>Primary Icon.</returns>
        public System.Drawing.Image GetPrimaryIconForGridRow(int ARow)
        {
            System.Drawing.Image ReturnValue = null;
            PPartnerAttributeRow PartnerAttributeDR = null;
            DataRowView RowView;
            bool IsPrimaryPhone = false;

            if (FGridRowIconsImageList != null)
            {
                RowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARow + 1);

                if (RowView != null)
                {
                    PartnerAttributeDR = (PPartnerAttributeRow)(RowView.Row);
                }

                if ((PartnerAttributeDR != null)
                    && (PartnerAttributeDR.RowState != DataRowState.Deleted)
                    && (PartnerAttributeDR.RowState != DataRowState.Detached))
                {
                    if (PartnerAttributeDR.Primary)
                    {
                        for (int Counter = 0; Counter < FPhoneAttributesDV.Count; Counter++)
                        {
                            if (PartnerAttributeDR.AttributeType == ((PPartnerAttributeTypeRow)FPhoneAttributesDV[Counter].Row).AttributeType)
                            {
                                IsPrimaryPhone = true;
                                break;
                            }
                        }

                        // One of the Primary Icons - either for Primary Phone or Primary Email
                        if (IsPrimaryPhone)
                        {
                            ReturnValue = FGridRowIconsImageList.Images[0];
                        }
                        else
                        {
                            ReturnValue = FGridRowIconsImageList.Images[1];
                        }
                    }
                    else
                    {
                        // Empty Icon
                        ReturnValue = FGridRowIconsImageList.Images[2];
                    }
                }
                else
                {
                    // Empty Icon
                    ReturnValue = FGridRowIconsImageList.Images[2];
                }
            }

            return ReturnValue;
        }
    }
}