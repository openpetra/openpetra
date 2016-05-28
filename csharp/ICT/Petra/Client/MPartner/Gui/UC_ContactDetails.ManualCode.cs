//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
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

        #region Resourcestrings

        private readonly string StrDefaultContactType = Catalog.GetString("Phone");

        private readonly string StrPrimaryPhone = Catalog.GetString("Primary Phone");

        private readonly string StrPhoneWithinOrgansiation = Catalog.GetString("Office Phone");

        private readonly string StrPrimaryEmail = Catalog.GetString("Primary E-Mail");

        private readonly string StrSecondaryEmail = Catalog.GetString("Secondary E-Mail");

        private readonly string StrEmailWithinOrgansiation = Catalog.GetString("Office E-Mail");

        private readonly string StrIntlTelephoneCode = Catalog.GetString("International Telephone Country Code " +
            "(only available for Phone Numbers and Fax Numbers)");

        private readonly string StrFunctionKeysTip = Catalog.GetString(
            " (Press <F5>-<F8> to change Contact Type; <SHIFT>+any of those for alternative.)");

        private readonly string StrFunctionKeysTipWithPrimary = Catalog.GetString(
            " (Press <F5>-<F8> to change Contact Type; <SHIFT>+any of those for alternative,<F12> to make Primary)");

        private readonly string StrPhoneUnavailableConsequenceSelectNewOne = Catalog.GetString(
            "Please select a current Phone Number from the available ones!");

        private readonly string StrPhoneUnavailableConsequenceCleared = Catalog.GetString(
            "The {0} Number has been cleared as there is no other current Phone record.");

        private readonly string StrFunctionKeyPrimaryHint = Catalog.GetString(
            "  (Press <F12> to make Primary.)");

        private readonly string StrFunctionKeyPrimaryAndWithinOrgHint = Catalog.GetString(
            "  (Press <F12> to make Primary, <SHIFT>+<F12> to make Work-specific.)");

        private readonly string StrPhoneUnavailableConsequenceSelectNewOneTitle = Catalog.GetString("{0} Number Needs Adjusting");

        private readonly string StrPhoneUnavailableConsequenceClearedTitle = Catalog.GetString("{0} Number Cleared");

        private readonly string StrEmailUnavailableConsequenceSelectNewOne = Catalog.GetString(
            "Please select a current E-mail Address from the available ones!");

        private readonly string StrEmailUnavailableConsequenceCleared = Catalog.GetString(
            "The {0} Address has been cleared as there is no other current E-Mail Address record.");

        private readonly string StrEmailUnavailableConsequenceSelectNewOneTitle = Catalog.GetString("{0} Address Needs Adjusting");

        private readonly string StrEmailUnavailableConsequenceClearedTitle = Catalog.GetString("{0} Address Cleared");

        private readonly string StrSendEmailFromValueButtonHelptext = Catalog.GetString("Send e-mail to the " +
            "e-mail address (with your standard e-mail program). (<F11>)");

        private readonly string StrSendEmailFromPrimaryEmailButtonHelptext = Catalog.GetString("Send e-mail to the " +
            "Primary E-mail Address (with your standard e-mail program) (<F9>).");

        private readonly string StrSendEmailFromSecondaryEmailButtonHelptext = Catalog.GetString("Send e-mail to the " +
            "Secondary E-mail Address of the Family (with your standard e-mail program). (<F10>)");

        private readonly string StrSendEmailFromOfficeEmailButtonHelptext = Catalog.GetString("Send e-mail to the " +
            "Office E-mail Address (with your standard e-mail program). (<F10>)");

        private readonly string StrOpenHyperlinkFromValueButtonHelptext = Catalog.GetString(
            "Open the hyperlink in a web browser. (<F11>)");

        private readonly string StrInitiateSkypeCallFromValueButtonHelptext = Catalog.GetString(
            "Initate a Skype call, calling the Skype ID. (<F11>)");

        private readonly string StrPhoneTypeCanNoLongerBePrimary = Catalog.GetString("You have changed the Contact Type and " +
            "the Contact Detail that was a Phone Number is no longer one.\r\n" +
            "As a result, this Contact Detail can no longer be the Primary Phone! It has therefore been removed from the " +
            "Primary Phone choices.");

        private readonly string StrPhoneTypeCanNoLongerBePrimaryTitle = Catalog.GetString(
            "No Longer Primary Phone");

        #endregion

        /// <summary>
        /// Label that shows a Country Name of a given Country Code under the International Phone Prefix ComboBox.
        /// Note: this Control is actually a TextBox disguised as a Label. This makes it possible to select and copy
        /// this Control's text
        /// </summary>
        public System.Windows.Forms.TextBox lblIntlPhonePrefixCountryInfo;

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

        private bool FPhoneSelectedValueChangedEvent = false;

        private bool FEmailSelectedValueChangedEvent = false;

        private bool FPrimaryEmailAutoChosen = false;

        private bool FPrimaryPhoneAutoChosen = false;

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
        /// Delegate function to determine the Country Code of the Best Address of the Partner.
        /// </summary>
        private TDelegateForDeterminationOfBestAddressesCountryCode FDelegateForDeterminationOfBestAddressesCountryCode;

        TPartnerClass FPartnersPartnerClass;

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

        /// <summary>Delegate declaration</summary>
        public delegate string TDelegateForDeterminationOfBestAddressesCountryCode();

        /// <summary>
        /// Initialises the Delegate function for the determination of the Country Code of the
        /// 'Best Address' of the Partner.
        /// </summary>
        /// <param name="ADelegateFunction">Function that should get called.</param>
        public void InitialiseDelegateForDeterminationOfBestAddressesCountryCode(
            TDelegateForDeterminationOfBestAddressesCountryCode ADelegateFunction)
        {
            FDelegateForDeterminationOfBestAddressesCountryCode = ADelegateFunction;
        }

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

            UpdateSystemCategoryOvrlContSettgsCombosRecords();

            if (CurrentDetailDR != null)
            {
                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    FValueWithSpecialMeaningChangedButUserDidntLeaveControl =
                        String.Compare(txtValue.Text, CurrentDetailDR.Value, StringComparison.InvariantCulture) != 0;
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, CurrentDetailDR))
                    {
                        FValueWithSpecialMeaningChangedButUserDidntLeaveControl =
                            String.Compare(txtValue.Text,
                                Calculations.ConcatenatePhoneOrFaxNumberWithIntlCountryPrefix(CurrentDetailDR),
                                StringComparison.InvariantCulture) != 0;
                    }
                }

                //
                // Make sure latest screen modifications are saved to FMainDS
                //
                GetDataFromControls();

                // Ensure the 'auto-setting' of the 'Primary Phone Number' happens when the first Phone Number just got entered
                // AND the user has NOT left the Value Control but went straight to the Save Button / File -> Save menu!
                HandleValueLeave(null, null);

                // Refresh the ComboBoxes so they reflect any change in the email address/phone number!
                UpdateEmailComboItemsOfAllEmailCombos(true);
                UpdatePhoneComboItemsOfAllPhoneCombos();

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

        private void EnsureOnlyPhoneAndFaxRowsHaveCountryCodeSet()
        {
            PPartnerAttributeRow PPartnerAttributeDR;

            for (int Counter = 0; Counter < FMainDS.PPartnerAttribute.Rows.Count; Counter++)
            {
                PPartnerAttributeDR = FMainDS.PPartnerAttribute[Counter];

                if (((PPartnerAttributeDR.RowState == DataRowState.Added)
                     || (PPartnerAttributeDR.RowState == DataRowState.Modified))
                    && (!Calculations.RowHasPhoneOrFaxAttributeType(FPhoneAttributesDV, PPartnerAttributeDR, false)))
                {
                    PPartnerAttributeDR.ValueCountry = String.Empty;
                }
            }
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

            cmbIntlPhonePrefix.SelectedValueChanged += CmbIntlPhonePrefix_UpdateIntlPhoneTips;
            cmbIntlPhonePrefix.TextChanged += CmbIntlPhonePrefix_UpdateIntlPhoneTips;

            // Make the Value TextBox automatically select all its text if the record holds the 'NEWVALUE'
            // and the user enters the Value TextBox (see Bug #4548)
            new TWireUpSelectAllTextOnFocus(txtValue,
                delegate
                {
                    var DetailDR = GetSelectedDetailRow();

                    if ((DetailDR != null)
                        && (txtValue.Text.StartsWith(Catalog.GetString("NEWVALUE") + "-")))
                    {
                        return true;
                    }

                    return false;
                }
                );

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
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
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

            FPetraUtilsObject.DataSavingValidated += FPetraUtilsObject_DataSavingValidated;

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

            FPartnersPartnerClass = SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass);

            // Show the 'Within The Organisation' GroupBox only if the Partner is of Partner Class PERSON
            if (FPartnersPartnerClass != TPartnerClass.PERSON)
            {
                grpWithinTheOrganisation.Visible = false;
            }

            if (FPartnersPartnerClass != TPartnerClass.FAMILY)
            {
                pnlFamilyExtraControls.Visible = false;
            }

            // Move the 'Within the Organsiation' GroupBox a bit up from it's automatically assigned position
            grpWithinTheOrganisation.Top = 16;

            // Move the 'Family Extra Controls' Panel a bit up from it's automatically assigned position
            pnlFamilyExtraControls.Top = 50;

            // Move the Panel that groups the 'Current' Controls for layout purposes a bit up from it's automatically assigned position
            pnlCurrentGrouping.Top = 60;
            chkCurrent.Top = 7;
            dtpNoLongerCurrentFrom.Top = 8;
            lblNoLongerCurrentFrom.Top = 12;

            // Move the Panel that groups the 'Value' Controls for layout purposes a bit up from it's automatically assigned position
            pnlValueGrouping.Top = 29;
            txtValue.Top = 3;
            lblValue.Top = 9;
            btnLaunchHyperlinkFromValue.Top = 3;
            cmbIntlPhonePrefix.Top = txtValue.Top;
            cmbIntlPhonePrefix.TabIndex = lblValue.TabIndex + 1;
            pnlValueGrouping.Width = 350;

            chkConfidential.Top = 90;
            lblConfidential.Top = 95;

            CreateIntlPhonePrefixCountryLabel();

            // Make all ComboBoxes in the 'Overall' GroupBox read-only
            cmbPrimaryWayOfContacting.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrimaryPhoneForContacting.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrimaryEMail.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSecondaryEMail.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPhoneWithinTheOrganisation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEMailWithinTheOrganisation.DropDownStyle = ComboBoxStyle.DropDownList;

            // Set up status bar texts for unbound controls and for bound controls whose auto-assigned texts don't match the use here on this screen (these talk about 'Partner Attributes')
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryWayOfContacting,
                Catalog.GetString("Select the primary method by which the Partner should be contacted. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryPhoneForContacting,
                Catalog.GetString("Select one of the Partner's telephone numbers. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryEMail,
                Catalog.GetString(
                    "Select one of the Partner's e-mail addresses. This will be used whenever an automated e-mail is to be sent to this Partner. (<F9> = Send Email)"));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkPrefEMail, StrSendEmailFromPrimaryEmailButtonHelptext);
            FPetraUtilsObject.SetStatusBarText(cmbSecondaryEMail,
                Catalog.GetString(
                    "Select one of the Partner's e-mail addresses. This can optionally be used where an option to utilise this is available. (<F10> = Send Email)"));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkSecondaryEMail, StrSendEmailFromSecondaryEmailButtonHelptext);
            FPetraUtilsObject.SetStatusBarText(cmbPhoneWithinTheOrganisation,
                Catalog.GetString(
                    "Select one of the Partner's telephone numbers to designate it as her/his telephone number within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(cmbEMailWithinTheOrganisation,
                Catalog.GetString(
                    "Select one of the Partner's e-mail addresses to designate it as her/his e-mail address within The Organisation. (<F10> = Send Email)"));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkEMailWithinOrg, StrSendEmailFromOfficeEmailButtonHelptext);

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

            FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkPrefEMail, StrSendEmailFromPrimaryEmailButtonHelptext);
            FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkEMailWithinOrg, StrSendEmailFromOfficeEmailButtonHelptext);
            FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkSecondaryEMail, StrSendEmailFromSecondaryEmailButtonHelptext);

            FPetraUtilsObject.SetStatusBarText(cmbIntlPhonePrefix.cmbCombobox, StrIntlTelephoneCode);

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
            //            rtbValue.BuildLinkWithValue = BuildLinkWithValue;

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // Hide all not-yet-implemented Controls in the 'Overall Contact Settings' GroupBox for the time being - their implementation will follow
            pnlPromoteDemote.Visible = false;
        }

        private void CreateIntlPhonePrefixCountryLabel()
        {
            this.lblIntlPhonePrefixCountryInfo = new System.Windows.Forms.TextBox();
            this.lblIntlPhonePrefixCountryInfo.BackColor = System.Drawing.SystemColors.Control; // System.Drawing.Color.Red - for debugging
            this.lblIntlPhonePrefixCountryInfo.Name = "lblIntlPhonePrefixCountryInfo";
            this.lblIntlPhonePrefixCountryInfo.Size = new System.Drawing.Size(288, 30);
            this.lblIntlPhonePrefixCountryInfo.TabIndex = 1;
            this.lblIntlPhonePrefixCountryInfo.TextAlign = HorizontalAlignment.Left;
            //this.lblIntlPhonePrefixCountryInfo.Paint += new PaintEventHandler(this.lblIntlPhonePrefixCountryInfo);

            lblIntlPhonePrefixCountryInfo.Font = new System.Drawing.Font(cmbIntlPhonePrefix.Font.FontFamily, 6);
            this.lblIntlPhonePrefixCountryInfo.Multiline = false;
            this.lblIntlPhonePrefixCountryInfo.WordWrap = false;
            this.lblIntlPhonePrefixCountryInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblIntlPhonePrefixCountryInfo.ReadOnly = true;
            this.lblIntlPhonePrefixCountryInfo.Location = new System.Drawing.Point(lblValue.Right + 6, cmbIntlPhonePrefix.Bottom + 1);
            this.lblIntlPhonePrefixCountryInfo.TabStop = false;
            this.lblIntlPhonePrefixCountryInfo.Tag = MCommonResourcestrings.StrCtrlSuppressChangeDetection;

            pnlValueGrouping.Controls.Add(lblIntlPhonePrefixCountryInfo);
        }

        private void CmbIntlPhonePrefix_UpdateIntlPhoneTips(object sender, EventArgs e)
        {
            string CountryName;
            string IntlPhoneTipStr;

            if (Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, GetSelectedDetailRow()))
            {
                CountryName = cmbIntlPhonePrefix.GetSelectedString(1);

                if (CountryName != String.Empty)
                {
                    IntlPhoneTipStr = String.Format(Catalog.GetString("International Telephone Code '{0}' is for country '{1}'"),
                        cmbIntlPhonePrefix.Text, CountryName);

                    lblIntlPhonePrefixCountryInfo.Text = "[" + CountryName + "]";
                    FPetraUtilsObject.SetToolTip(lblIntlPhonePrefixCountryInfo, IntlPhoneTipStr);
                    FPetraUtilsObject.SetToolTip(cmbIntlPhonePrefix.cmbCombobox, IntlPhoneTipStr);
                }
                else
                {
                    ClearIntlPhoneTips();
                }
            }
            else
            {
                ClearIntlPhoneTips();
            }
        }

        private void ClearIntlPhoneTips()
        {
            lblIntlPhonePrefixCountryInfo.Text = String.Empty;
            FPetraUtilsObject.SetToolTip(lblIntlPhonePrefixCountryInfo, String.Empty);
            FPetraUtilsObject.SetToolTip(cmbIntlPhonePrefix.cmbCombobox, String.Empty);
        }

        private void FPetraUtilsObject_DataSavingValidated(object Sender, System.ComponentModel.CancelEventArgs e)
        {
            EnsureOnlyPhoneAndFaxRowsHaveCountryCodeSet();
        }

        /// <summary>
        /// Update an invisible Column on which a calculated Column is based which is then shown as the 'Value' Column in the Grid
        /// </summary>
        /// <remarks>We need to do this as the p_partner_attribute Table holds Country Codes (e.g. 'GB') and not International Phone
        /// Prefixes (eg. +44), but we need to show Phone / Fax Numbers in the Grid prefixed with the International Phone Prefix.</remarks>
        private void UpdateIntlPhonePrefixColumn()
        {
            PartnerEditTDSPPartnerAttributeRow PartnerAttributeDR;
            PCountryRow CountryDR;
            bool RowWasUnchangedBefore;

            for (int Counter = 0; Counter < FMainDS.PPartnerAttribute.Rows.Count; Counter++)
            {
                PartnerAttributeDR = FMainDS.PPartnerAttribute[Counter];

                if ((PartnerAttributeDR.RowState == DataRowState.Unchanged)
                    || (PartnerAttributeDR.RowState == DataRowState.Added)
                    || (PartnerAttributeDR.RowState == DataRowState.Modified))
                {
                    RowWasUnchangedBefore = PartnerAttributeDR.RowState == DataRowState.Unchanged;

                    if (PartnerAttributeDR.ValueCountry != String.Empty)
                    {
                        CountryDR = (PCountryRow)Calculations.FindCountryRowInCachedCountryList(PartnerAttributeDR.ValueCountry);

                        if (CountryDR != null)
                        {
                            PartnerAttributeDR[Calculations.CALCCOLUMNNAME_INTLPHONEPREFIX] = "+" + CountryDR.InternatTelephoneCode + " ";
                        }
                        else
                        {
                            PartnerAttributeDR[Calculations.CALCCOLUMNNAME_INTLPHONEPREFIX] = String.Empty;
                        }
                    }
                    else
                    {
                        PartnerAttributeDR[Calculations.CALCCOLUMNNAME_INTLPHONEPREFIX] = String.Empty;
                    }

                    if (RowWasUnchangedBefore)
                    {
                        PartnerAttributeDR.AcceptChanges();
                    }
                }
            }
        }

        private void ShowDataManual()
        {
            FEmailSelectedValueChangedEvent = true;

            UpdateEmailComboItemsOfAllEmailCombos();

            FEmailSelectedValueChangedEvent = false;

            FPhoneSelectedValueChangedEvent = true;

            UpdatePhoneComboItemsOfAllPhoneCombos();

            FPhoneSelectedValueChangedEvent = false;


            SelectSystemCategoryOvrlContSettgsCombosItems();
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

            // Update invisible Column on which a calculated Column is based which is then shown as the 'Value' Column in the Grid
            UpdateIntlPhonePrefixColumn();

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
            // Ensure the 'auto-setting' of the 'Primary Phone Number' happens when the first Phone Number just got entered
            // AND the user has NOT left the Value Control but went straight to the New Button!
            HandleValueLeave(null, null);

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

            if (Calculations.RowHasPhoneOrFaxAttributeType(FPhoneAttributesDV, ARow, false))
            {
                if (FDelegateForDeterminationOfBestAddressesCountryCode != null)
                {
                    ARow.ValueCountry = FDelegateForDeterminationOfBestAddressesCountryCode();
                }
                else
                {
                    throw new EOPAppException("Delegate FDelegateForDeterminationOfBestAddressesCountryCode is not set up");
                }
            }

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

            ARow.Value = Catalog.GetString("NEWVALUE") + ARow.Sequence.ToString();
            ARow.Primary = false;
            ARow.WithinOrganisation = false;
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
            const string SPECIALMEANINGFORMATSTRING = "  [{0}]";
            string SpecialMeaning = String.Empty;
            bool RowToDeleteHasPhoneAttributeType = Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, ARowToDelete);
            PPartnerAttributeTypeRow ContactTypeDR;
            TPartnerAttributeTypeValueKind ValueKind;
            string SecondaryEmailAddress;
            string PrimaryContactMethod;

            // Those values are getting safed for use in the PostDeleteManual Method
            // Trying to establish those values there doesn't work in case the Partner was new,
            // the Row was newly added, and then gets removed (DataRow has DataRowVersion.Detached in
            // this case, and no Original data that can be accessed!)
            FDeletedRowsAttributeType = ARowToDelete.AttributeType;

            if (RowToDeleteHasPhoneAttributeType)
            {
                FDeletedRowsValue = Calculations.ConcatenatePhoneOrFaxNumberWithIntlCountryPrefix(ARowToDelete);

                if (ARowToDelete.Primary)
                {
                    if (!ARowToDelete.WithinOrganisation)
                    {
                        SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPrimaryPhone);
                    }
                    else
                    {
                        SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPrimaryPhone + Catalog.GetString(" AND ") +
                            StrPhoneWithinOrgansiation);
                    }
                }
                else if (ARowToDelete.WithinOrganisation)
                {
                    SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPhoneWithinOrgansiation);
                }
            }
            else
            {
                FDeletedRowsValue = ARowToDelete.Value;

                // Ensure Secondary E-mail setting is written to underlying data
                UpdateSystemCategoryOvrlContSettgsCombosRecords();

                Calculations.DetermineValueKindOfPartnerAttributeRecord(ARowToDelete, FMainDS.PPartnerAttributeType, out ContactTypeDR, out ValueKind);

                if (ValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    GetSystemCategoryOvrlContSettgsValues(out PrimaryContactMethod, out SecondaryEmailAddress);

                    if (ARowToDelete.Primary)
                    {
                        if (!ARowToDelete.WithinOrganisation)
                        {
                            SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPrimaryEmail);

                            if (SecondaryEmailAddress == ARowToDelete.Value)
                            {
                                SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPrimaryEmail + Catalog.GetString(" AND ") +
                                    StrSecondaryEmail);
                            }
                        }
                        else
                        {
                            SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrPrimaryEmail + Catalog.GetString(" AND ") +
                                StrEmailWithinOrgansiation);
                        }
                    }
                    else if (ARowToDelete.WithinOrganisation)
                    {
                        SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrEmailWithinOrgansiation);
                    }
                    else if (ARowToDelete.Value == SecondaryEmailAddress)
                    {
                        SpecialMeaning = String.Format(SPECIALMEANINGFORMATSTRING, StrSecondaryEmail);
                    }
                }
            }

            ADeletionQuestion =
                String.Format(Catalog.GetString(
                        "Are you sure you want to delete the following Contact Detail record?\r\n\r\n    Type: '{0}'{1}\r\n    Value: '{2}'"),
                    ARowToDelete.AttributeType, SpecialMeaning, FDeletedRowsValue);

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
            string PrimEmailConsequence = StrEmailUnavailableConsequenceSelectNewOne;
            string PrimEmailMessageBoxTitle = String.Format(StrEmailUnavailableConsequenceSelectNewOneTitle, StrPrimaryEmail);
            string PrimPhoneConsequence = StrPhoneUnavailableConsequenceSelectNewOne;
            string PrimPhoneMessageBoxTitle = String.Format(StrPhoneUnavailableConsequenceSelectNewOneTitle, StrPrimaryPhone);

            if (ADeletionPerformed)
            {
                StringCollection EmailAttrColl = StringHelper.StrSplit(
                    TSharedDataCache.TMPartner.GetEmailPartnerAttributesConcatStr(), ", ");

                if (EmailAttrColl.Contains("'" + FDeletedRowsAttributeType + "'"))
                {
                    // User deleted an E-mail Contact Detail

                    if (cmbPrimaryEMail.GetSelectedString(-1) == FDeletedRowsValue)
                    {
                        DataView EligibleEmailAddrsDV = Calculations.DeterminePartnerEmailAddresses(
                            FMainDS.PPartnerAttribute, true);

                        if (EligibleEmailAddrsDV.Count == 0)
                        {
                            NoEmailAddressesAvailableAnymore = true;
                            PrimEmailConsequence = String.Format(StrEmailUnavailableConsequenceCleared, StrPrimaryEmail);
                            PrimEmailMessageBoxTitle = String.Format(StrEmailUnavailableConsequenceClearedTitle, StrPrimaryEmail);
                        }

                        // User deleted the E-mail Contact Detail that was set as the 'Primary E-Mail Address':
                        // Refresh all E-Mail Address Combos (which upon that will no longer contain the E-mail
                        // Address of the deleted Contact Detail!) and notify the user that (s)he needs to take action.
                        UpdateEmailComboItemsOfAllEmailCombos(true);

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
                        // Simply refresh all E-Mail Address Combos.
                        UpdateEmailComboItemsOfAllEmailCombos(true);
                    }
                }

                StringCollection PhoneAttrColl = StringHelper.StrSplit(
                    TSharedDataCache.TMPartner.GetPhonePartnerAttributesConcatStr(), ", ");

                if (PhoneAttrColl.Contains("'" + FDeletedRowsAttributeType + "'"))
                {
                    // User deleted a Phone Contact Detail

                    if (cmbPrimaryPhoneForContacting.GetSelectedString(-1) == FDeletedRowsValue)
                    {
                        DataView EligiblePhoneNrsDV = Calculations.DeterminePartnerPhoneNumbers(FMainDS.PPartnerAttribute,
                            true, false);

                        if (EligiblePhoneNrsDV.Count == 0)
                        {
                            NoPhoneNumbersAvailableAnymore = true;
                            PrimPhoneConsequence = String.Format(StrPhoneUnavailableConsequenceCleared, StrPrimaryPhone);
                            PrimPhoneMessageBoxTitle = String.Format(StrPhoneUnavailableConsequenceClearedTitle, StrPrimaryPhone);
                        }

                        // User deleted the Phone Contact Detail that was set as the 'Primary Phone':
                        // Refresh all Phone Combos (which upon that will no longer contain the Phone
                        // Number of the deleted Contact Detail!) and notify the user that (s)he needs to take action.
                        UpdatePhoneComboItemsOfAllPhoneCombos();

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
                        // Simply refresh all Phone Combos.
                        UpdatePhoneComboItemsOfAllPhoneCombos();
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
            grdDetails.Columns[0].Width = 20;
            grdDetails.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            //
            // Contact Type
//            grdDetails.AddTextColumn("Type Code", FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()]);

            //
            // Contact Type (Calculated Expression!)
            grdDetails.AddTextColumn("Contact Type", FMainDS.PPartnerAttribute.Columns[Calculations.CALCCOLUMNNAME_CONTACTTYPE]);

            // Comment
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerAttribute.ColumnComment);

            // Value
            grdDetails.AddTextColumn("Value", FMainDS.PPartnerAttribute.Columns[Calculations.CALCCOLUMNNAME_VALUE]);

            // Current
            grdDetails.AddCheckBoxColumn("Current", FMainDS.PPartnerAttribute.ColumnCurrent);

            // Confidential
            grdDetails.AddCheckBoxColumn("Confidential", FMainDS.PPartnerAttribute.ColumnConfidential);

//            // Sequence (for testing purposes only...)
//            grdDetails.AddTextColumn("Sequence", FMainDS.PPartnerAttribute.ColumnSequence);

//            // Index (for testing purposes only...)
//            grdDetails.AddTextColumn("Index", FMainDS.PPartnerAttribute.ColumnIndex);

//            // Primary (for testing purposes only...)
//            grdDetails.AddCheckBoxColumn("Primary", FMainDS.PPartnerAttribute.ColumnPrimary);

//            // Within Organsiation (for testing purposes only...)
//            if (FPartnersPartnerClass == TPartnerClass.PERSON)
//            {

//                grdDetails.AddCheckBoxColumn("Within Org.", FMainDS.PPartnerAttribute.ColumnWithinOrganisation);
//            }

            // Modification TimeStamp (for testing purposes only...)
//             grdDetails.AddTextColumn("Modification TimeStamp", FMainDS.PPartnerAttribute.ColumnModificationId);

            grdDetails.AutoResizeGrid();
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

            string EmailAddress = cmbPrimaryEMail.GetSelectedString();

            // Don't attempt to launch an 'empty' link
            if (EmailAddress == String.Empty)
            {
                return;
            }

            Launcher.LaunchHyperLink(EmailAddress, "||email||");
        }

        private void LaunchHyperlinkSecondaryEMail(object sender, EventArgs e)
        {
            // TODO Replace this 'quick solution' when the txtValue Control is replaced with the proper rtbValue Control!
            TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

            string EmailAddress = cmbSecondaryEMail.GetSelectedString();

            // Don't attempt to launch an 'empty' link
            if (EmailAddress == String.Empty)
            {
                return;
            }

            Launcher.LaunchHyperLink(EmailAddress, "||email||");
        }

        private void LaunchHyperlinkEMailWithinOrg(object sender, EventArgs e)
        {
            // TODO Replace this 'quick solution' when the txtValue Control is replaced with the proper rtbValue Control!
            TRtbHyperlinks.DisplayHelper Launcher = new TRtbHyperlinks.DisplayHelper(new TRtbHyperlinks());

            string EmailAddress = cmbEMailWithinTheOrganisation.GetSelectedString();

            // Don't attempt to launch an 'empty' link
            if (EmailAddress == String.Empty)
            {
                return;
            }

            Launcher.LaunchHyperLink(EmailAddress, "||email||");
        }

        private void LaunchHyperlinkFromValue(object sender, EventArgs e)
        {
            TRtbHyperlinks TempHyperlinkCtrl = new TRtbHyperlinks();
            string LinkType = String.Empty;

            // Don't attempt to launch an 'empty' link
            if (txtValue.Text == String.Empty)
            {
                return;
            }

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
            bool SecondaryEmailAddressIsThisRecord = false;
            bool PrimaryPhoneNumberIsThisRecord = false;
            bool EmailAddressWithinOrganisationIsThisRecord = false;
            bool PhoneNumberWithinOrganisationIsThisRecord = false;

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

                    if (cmbSecondaryEMail.GetSelectedString() == txtValue.Text)
                    {
                        SecondaryEmailAddressIsThisRecord = true;
                    }

                    if (cmbEMailWithinTheOrganisation.GetSelectedString() == txtValue.Text)
                    {
                        EmailAddressWithinOrganisationIsThisRecord = true;
                    }

                    // Refresh all E-mail ComboBoxes so they show only the 'current' E-Mail Address records (which could possibly be none!)
                    UpdateEmailComboItemsOfAllEmailCombos(true);
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, GetSelectedDetailRow()))
                    {
                        if (cmbPrimaryPhoneForContacting.GetSelectedString() ==
                            Calculations.ConcatenatePhoneOrFaxNumberWithIntlCountryPrefix(txtValue.Text,
                                cmbIntlPhonePrefix.GetSelectedString(PCountryTable.ColumnInternatTelephoneCodeId)))
                        {
                            PrimaryPhoneNumberIsThisRecord = true;
                        }

                        if (cmbPhoneWithinTheOrganisation.GetSelectedString() ==
                            Calculations.ConcatenatePhoneOrFaxNumberWithIntlCountryPrefix(txtValue.Text,
                                cmbIntlPhonePrefix.GetSelectedString(PCountryTable.ColumnInternatTelephoneCodeId)))
                        {
                            PhoneNumberWithinOrganisationIsThisRecord = true;
                        }

                        // Refresh all Phone ComboBoxes so they show only the 'current' Phone Number records (which could possibly be none!)
                        UpdatePhoneComboItemsOfAllPhoneCombos();
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
                        CheckThatNonCurrentEmailAddressIsntSpecificEmailAddr(TOverallContactComboType.occtSecondaryEmail,
                            SecondaryEmailAddressIsThisRecord);

                        CheckThatNonCurrentEmailAddressIsntSpecificEmailAddr(TOverallContactComboType.occtEmailWithinOrganisation,
                            EmailAddressWithinOrganisationIsThisRecord);

                        CheckThatNonCurrentEmailAddressIsntSpecificEmailAddr(TOverallContactComboType.occtPrimaryEmail,
                            PrimaryEmailAddressIsThisRecord);
                    }
                    else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                    {
                        if (Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, GetSelectedDetailRow()))
                        {
                            CheckThatNonCurrentPhoneNrIsntSpecificPhoneNr(TOverallContactComboType.occtPhoneWithinOrganisation,
                                PhoneNumberWithinOrganisationIsThisRecord);

                            CheckThatNonCurrentPhoneNrIsntSpecificPhoneNr(TOverallContactComboType.occtPrimaryPhone,
                                PrimaryPhoneNumberIsThisRecord);
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
            string PreviousValue;
            var SelectedDetailDR = GetSelectedDetailRow();

            // Try to access the selected Detail Row. If there are no records yet at all then we must not execute any
            // further code in this Method.
            if (SelectedDetailDR == null)
            {
                return;
            }

            if ((!FRunningInsideShowDetails)
                && (SelectedDetailDR.RowState != DataRowState.Detached))
            {
                if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                {
                    PreviousValue = SelectedDetailDR.Value;

                    // Ensure current E-mail Address is reflected in the DataRow
                    SelectedDetailDR.Value = txtValue.Text;

                    // Refresh the ComboBoxes so they reflect any change in the E-mail Address!
                    FPrimaryEmailAutoChosen = (SelectedDetailDR.RowState == DataRowState.Added)
                                              && ((GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryEmail, true).Count == 1)
                                                  && (FValueWithSpecialMeaningChangedButUserDidntLeaveControl
                                                      || !FRunningInsideDataSaving));

                    UpdateEmailComboItems(cmbPrimaryEMail, true,
                        FPrimaryEmailAutoChosen ? txtValue.Text : null);
                    UpdateEmailComboItems(cmbEMailWithinTheOrganisation, true);

                    // Ensure Secondary E-mail setting is written to underlying data
                    UpdateSystemCategoryOvrlContSettgsCombosRecords();

                    UpdateEmailComboItems(cmbSecondaryEMail, true, txtValue.Text, PreviousValue);

                    UpdatePhoneComboItemsOfAllPhoneCombos();

                    FValueWithSpecialMeaningChangedButUserDidntLeaveControl = false;
                }
                else if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                {
                    if (Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, SelectedDetailDR))
                    {
                        // Ensure current Phone Number is reflected in the DataRow
                        SelectedDetailDR.Value = txtValue.Text;
                        SelectedDetailDR.ValueCountry = cmbIntlPhonePrefix.GetSelectedString();

                        // Update invisible Column on which a calculated Column is based which is then shown as the 'Value' Column in the Grid
                        UpdateIntlPhonePrefixColumn();

                        // Refresh the ComboBoxes so they reflect any change in the Phone Number!
                        FPrimaryPhoneAutoChosen = (SelectedDetailDR.RowState == DataRowState.Added)
                                                  && ((GetDataViewForContactCombo(TOverallContactComboType.occtPrimaryPhone, true).Count == 1)
                                                      && (FValueWithSpecialMeaningChangedButUserDidntLeaveControl
                                                          || !FRunningInsideDataSaving));

                        UpdatePhoneComboItems(cmbPrimaryPhoneForContacting,
                            FPrimaryPhoneAutoChosen ?
                            Calculations.ConcatenatePhoneOrFaxNumberWithIntlCountryPrefix(txtValue.Text,
                                cmbIntlPhonePrefix.GetSelectedString(PCountryTable.ColumnInternatTelephoneCodeId)) : null);

                        UpdatePhoneComboItems(cmbPhoneWithinTheOrganisation);

                        UpdateEmailComboItemsOfAllEmailCombos(true);

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
            var SelectedDetailDR = GetSelectedDetailRow();

            // Try to access the selected Detail Row. If the user had just deleted the Row then we must not execute any
            // further code in this Method.
            if (SelectedDetailDR == null)
            {
                return;
            }

            if ((!FSuppressOnContactTypeChangedEvent)
                && (cmbContactType.Text != String.Empty))
            {
                ContactTypeDR = (PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow();

                if (!FRunningInsideShowDetails)
                {
                    // If the user created a new Record and changes Attribute Types: Make sure that the change in the Attribute Type is effected
                    // in the Record immediately, and not just when the user leaves a Control that gets Validated. If this isn't done then the
                    // 'Overall Contact Settings' ComboBoxes whose Items are based on the Rows' Attribute Types won't get updated immediately.
                    SelectedDetailDR.AttributeType = cmbContactType.GetSelectedString();
                }

                SelectCorrespondingCategory(ContactTypeDR);

                if (Enum.TryParse <TPartnerAttributeTypeValueKind>(ContactTypeDR.AttributeTypeValueKind, out ValueKind))
                {
                    FValueKind = ValueKind;

                    switch (FValueKind)
                    {
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:

                            btnLaunchHyperlinkFromValue.Visible = false;

                            if (Calculations.RowHasPhoneOrFaxAttributeType(FPhoneAttributesDV, SelectedDetailDR, false))
                            {
                                ShowHideIntlPhonePrefix(true, -1);
                            }
                            else
                            {
                                ShowHideIntlPhonePrefix(false, 290);
                            }

                            break;

                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                            btnLaunchHyperlinkFromValue.Visible = true;

                            ShowHideIntlPhonePrefix(false, 256);

                            break;

                        default:
                            btnLaunchHyperlinkFromValue.Visible = false;

                            ShowHideIntlPhonePrefix(false, 290);


                            throw new Exception("Invalid value for TPartnerAttributeTypeValueKind");
                    }
                }
                else
                {
                    // Fallback!
                    FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;
                }

                UpdateValueManual();

                EnsureOnlyPhoneAndFaxRowsHaveCountryCodeSet();
                UpdateIntlPhonePrefixColumn();

                if (!FRunningInsideShowDetails)
                {
                    if (PreviousValueKind != FValueKind)
                    {
                        if (GetSelectedDetailRow().Primary)
                        {
                            GetSelectedDetailRow().Primary = false;

                            UpdateEmailComboItems(cmbPrimaryEMail, true);
                            UpdatePhoneComboItems(cmbPrimaryPhoneForContacting);

                            if (PreviousValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
                            {
                                if (!FPrimaryEmailAutoChosen)
                                {
                                    MessageBox.Show(Catalog.GetString(
                                            "You have changed the Contact Type and the Contact Detail that was an E-Mail Address is no longer one.\r\n"
                                            +
                                            "As a result, this Contact Detail can no longer be the Primary E-Mail Address! It has therefore been removed from the Primary E-Mail choices."),
                                        Catalog.GetString("No Longer Primary E-Mail"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    FPrimaryEmailAutoChosen = false;
                                }
                            }
                            else
                            {
                                if (!FPrimaryPhoneAutoChosen)
                                {
                                    MessageBox.Show(StrPhoneTypeCanNoLongerBePrimary, StrPhoneTypeCanNoLongerBePrimaryTitle,
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    FPrimaryPhoneAutoChosen = false;
                                }
                            }
                        }
                        else
                        {
                            UpdateSystemCategoryOvrlContSettgsCombosRecords();
                            UpdateEmailComboItemsOfAllEmailCombos(true);
                            UpdatePhoneComboItemsOfAllPhoneCombos();
                        }
                    }
                    else
                    {
                        // This might seem unnecessary as the 'PreviousValueKind' and 'FValueKind' are the same -
                        // However, it isn't, as 'Fax Numbers' are to be excluded from any Phone ComboBox but
                        // if a user changes between 'Phone' and 'Fax' there is no difference between 'PreviousValueKind'
                        // and 'FValueKind' and yet we need to update the Combo!
                        UpdatePhoneComboItemsOfAllPhoneCombos();

                        // In case the user had first created a 'Phone' or 'Mobile Phone' record and set it to be Primary,
                        // but then changes his/her mind to make it a 'Fax' record: remove the Primary flag!
                        if (((GetSelectedDetailRow().Primary))
                            && (PreviousValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL)
                            && (!Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, SelectedDetailDR)))
                        {
                            SelectedDetailDR.Primary = false;

                            MessageBox.Show(StrPhoneTypeCanNoLongerBePrimary, StrPhoneTypeCanNoLongerBePrimaryTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Shows or hides the 'International Phone Prefix' ComboBox. If shown it appears to the left of the 'Value'
        /// TextBox.
        /// </summary>
        /// <param name="AShow">Set to true to show the 'International Phone Prefix' ComboBox, set to false to hide it.</param>
        /// <param name="ValueWidthWithoutIntlPhonePrefix">Widht of the 'Value' TextBox when shown without the
        /// 'International Phone Prefix' ComboBox. (Ignored when <paramref name="AShow" /> is true.)</param>
        private void ShowHideIntlPhonePrefix(bool AShow, int ValueWidthWithoutIntlPhonePrefix)
        {
            int ControlLeft = lblValue.Left + lblValue.Width + 5;

            if (AShow)
            {
                cmbIntlPhonePrefix.Visible = true;
                cmbIntlPhonePrefix.Left = ControlLeft;
                txtValue.Left = 129;
                txtValue.Width = 220;

                CmbIntlPhonePrefix_UpdateIntlPhoneTips(null, null);
            }
            else
            {
                cmbIntlPhonePrefix.Visible = false;
                txtValue.Left = ControlLeft;
                txtValue.Width = ValueWidthWithoutIntlPhonePrefix;
                btnLaunchHyperlinkFromValue.Left = txtValue.Left + txtValue.Width + 5;

                ClearIntlPhoneTips();
            }
        }

        private void UpdateValueManual()
        {
            var SelectedDetailDR = GetSelectedDetailRow();
            string Value;
            string StatusBarText;
            bool CurrentRowHasPhoneAttributeType;

            if (SelectedDetailDR == null)
            {
                return;
            }

            try
            {
                // Try to access the Value Column. If the user had just deleted the Row that had this Value, this will raise one of the
                // Exceptions we are catching here. We must not execute any further code in this Method when that happens.
                Value = SelectedDetailDR.Value;
            }
            catch (System.Data.RowNotInTableException)
            {
                return;
            }
            catch (System.Data.DeletedRowInaccessibleException)
            {
                return;
            }

            switch (FValueKind)
            {
                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:
                    StatusBarText = Catalog.GetString("Enter whatever the Contact Type is about.");
                    CurrentRowHasPhoneAttributeType = Calculations.RowHasPhoneAttributeType(FPhoneAttributesDV, SelectedDetailDR);

                    if (cmbContactCategory.Enabled)
                    {
                        // Row record is a NEW record
                        if (!CurrentRowHasPhoneAttributeType)
                        {
                            StatusBarText += StrFunctionKeysTip;
                        }
                        else
                        {
                            if (!SelectedDetailDR.Primary)
                            {
                                StatusBarText += StrFunctionKeysTipWithPrimary;
                            }
                        }
                    }
                    else
                    {
                        // Row record isn't a new record
                        if ((!SelectedDetailDR.Primary)
                            || (!SelectedDetailDR.WithinOrganisation))
                        {
                            if (FPartnersPartnerClass == TPartnerClass.PERSON)
                            {
                                if (CurrentRowHasPhoneAttributeType)
                                {
                                    StatusBarText += StrFunctionKeyPrimaryAndWithinOrgHint;
                                }
                            }
                            else
                            {
                                if ((CurrentRowHasPhoneAttributeType)
                                    && (!SelectedDetailDR.Primary))
                                {
                                    StatusBarText += StrFunctionKeyPrimaryHint;
                                }
                            }
                        }
                    }

                    FPetraUtilsObject.SetStatusBarText(txtValue, StatusBarText);

                    // TODO UpdateValueManual / CONTACTDETAIL_GENERAL
                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter Hyperlink / URL.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));

                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    StrOpenHyperlinkFromValueButtonHelptext);
                    FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkFromValue,
                    StrOpenHyperlinkFromValueButtonHelptext);

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURL(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter value that becomes part of the Hyperlink / URL.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));

                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    StrOpenHyperlinkFromValueButtonHelptext);
                    FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkFromValue,
                    StrOpenHyperlinkFromValueButtonHelptext);

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURLWithValue(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                    StatusBarText = Catalog.GetString("Enter E-Mail Address.");

                    if (cmbContactCategory.Enabled)
                    {
                        // Row record is a NEW record
                        if (!SelectedDetailDR.Primary)
                        {
                            StatusBarText += StrFunctionKeysTipWithPrimary;
                        }
                    }
                    else
                    {
                        // Row record isn't a new record
                        if ((!SelectedDetailDR.Primary)
                            || (!SelectedDetailDR.WithinOrganisation))
                        {
                            if (FPartnersPartnerClass == TPartnerClass.PERSON)
                            {
                                StatusBarText += StrFunctionKeyPrimaryAndWithinOrgHint;
                            }
                            else
                            {
                                StatusBarText += StrFunctionKeyPrimaryHint;
                            }
                        }
                    }

                    FPetraUtilsObject.SetStatusBarText(txtValue, StatusBarText);

                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue, StrSendEmailFromValueButtonHelptext);
                    FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkFromValue,
                    StrSendEmailFromValueButtonHelptext);

                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayEmailAddress(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                    FPetraUtilsObject.SetStatusBarText(txtValue,
                    Catalog.GetString("Enter Skype ID.") +
                    (cmbContactCategory.Enabled ? StrFunctionKeysTip : String.Empty));

                    FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkFromValue,
                    StrInitiateSkypeCallFromValueButtonHelptext);
                    FPetraUtilsObject.SetToolTip(btnLaunchHyperlinkFromValue,
                    StrInitiateSkypeCallFromValueButtonHelptext);

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
        /// same Link in case of THyperLinkType.Http_With_Value_Replacement.</param>
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
                            "No Primary E-mail Address has been chosen for this Partner, although the Partner has at least one current E-mail Address on record.\r\n\r\n"
                            +
                            "IMPORTANT: OpenPetra can't send e-mails to this Partner in automated situations unless a Primary E-mail Address has been chosen!\r\n"
                            +
                            "Therefore, please choose an E-mail Address from the Primary E-mail Address setting,"),
                        Catalog.GetString("No Primary E-mail Address Set!"),
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
                            "No Primary E-mail Address has been chosen for this Partner.\r\n\r\nThere are non-current E-mail Addresses on record - the Filter has been\r\n"
                            +
                            "set up for you so those can be seen. You might want to check whether a current E-mail Address is available for this Partner."),
                        Catalog.GetString("No Primary E-mail Address Set - No Current E-mail Address"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    default:
                        throw new Exception("Invalid value for TTimerDrivenMessageBoxKind");
                }
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
                        // Select 'Skype for Business'
                        cmbContactCategory.cmbCombobox.SetSelectedString("Instant Messaging & Chat");
                        cmbContactType.cmbCombobox.SetSelectedString("Skype for Business");
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
                if (FPartnersPartnerClass == TPartnerClass.PERSON)
                {
                    LaunchHyperlinkEMailWithinOrg(null, null);
                }
                else if (FPartnersPartnerClass == TPartnerClass.FAMILY)
                {
                    LaunchHyperlinkSecondaryEMail(null, null);
                }
            }

            if (keyData == (Keys.F11))
            {
                if (btnLaunchHyperlinkFromValue.Visible)
                {
                    LaunchHyperlinkFromValue(null, null);
                }
            }

            if (((keyData == (Keys.F12))
                 || (keyData == (Keys.F12 | Keys.Shift))))
            {
                if (txtValue.Text != String.Empty)
                {
                    if (keyData == (Keys.F12 | Keys.Shift))
                    {
                        if (FPartnersPartnerClass == TPartnerClass.PERSON)
                        {
                            SetRecordSpecialFlag(false);
                        }
                    }
                    else
                    {
                        SetRecordSpecialFlag(true);
                    }
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