//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>Delegate declaration</summary>
    public delegate PartnerEditTDSPPartnerLocationRow TDelegateGetPartnerLocationRowOfCurrentlySelectedAddress();

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate bool TDelegateIsNewPartner(PartnerEditTDS AInspectDataSet);

    /// <summary>
    /// temporary class until PartnerInterests are implemented properly
    /// </summary>
    public class TUCPartnerInterests
    {
    }

    public partial class TUC_PartnerEdit_PartnerTabSet
    {
        #region Resourcestrings

        private static readonly string StrAddressesTabHeader = Catalog.GetString("Addresses");

        private static readonly string StrContactDetailsTabHeader = Catalog.GetString("Contact Details");

        private static readonly string StrSubscriptionsTabHeader = Catalog.GetString("Subscriptions");

        private static readonly string StrContactsTabHeader = Catalog.GetString("Contact Logs");

        private static readonly string StrSpecialTypesTabHeader = Catalog.GetString("Special Types");

        private static readonly string StrFamilyMembersTabHeader = Catalog.GetString("Family Members");

        private static readonly string StrPartnerRelationshipsTabHeader = Catalog.GetString("Relationships");

        private static readonly string StrFamilyTabHeader = Catalog.GetString("Family");

        private static readonly string StrInterestsTabHeader = Catalog.GetString("Interests");

        private static readonly string StrNotesTabHeader = Catalog.GetString("Notes");

        private static readonly string StrFinanceDetailsTabHeader = Catalog.GetString("Finance Details");

        private static readonly string StrAddressesSingular = Catalog.GetString("Address");

        private static readonly string StrContactDetailsSingular = Catalog.GetString("Contact Detail");

        private static readonly string StrSubscriptionsSingular = Catalog.GetString("Subscription");

        private static readonly string StrContactsSingular = Catalog.GetString("Contact");

        private static readonly string StrTabHeaderCounterTipSingular = Catalog.GetString("{0} {2}, of which {1} is ");

        private static readonly string StrTabHeaderCounterTipPlural = Catalog.GetString("{0} {2}, of which {1} are ");

        #endregion

        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TPartnerEditTabPageEnum FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpDefault;

        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;

        private String FPartnerClass;

        private Boolean FUserControlInitialised;

        private TDelegateIsNewPartner FDelegateIsNewPartner;

        #endregion

        #region Public Events

        /// <summary>todoComment</summary>
        public event THookupDataChangeEventHandler HookupDataChange;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupPartnerEditDataChange;

        /// <summary>todoComment</summary>
        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        #endregion

        #region Properties

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

        /// <summary>
        /// Returns the PLocation DataRow of the currently selected Address.
        /// </summary>
        /// <remarks>Performs all necessary initialisations in case the Address Tab
        /// hasn't been initialised before.</remarks>
        public PartnerEditTDSPPartnerLocationRow LocationDataRowOfCurrentlySelectedAddress
        {
            get
            {
                if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
                {
                    SetupUserControlAddresses();
                }

                return FUcoAddresses.PartnerLocationDataRowOfCurrentlySelectedAddress;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public PartnerEditTDSPPartnerLocationRow Get_LocationRowOfCurrentlySelectedAddress()
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            return FUcoAddresses.PartnerLocationDataRowOfCurrentlySelectedAddress;
        }

        /// <summary>
        /// Initialization of Manual Code logic.
        /// </summary>
        public void InitializeManualCode()
        {
            if (FTabPageEvent == null)
            {
                FTabPageEvent += this.TabPageEventHandler;
            }
        }

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
        public void SpecialInitUserControl()
        {
            ArrayList TabsToHide;

            OnDataLoadingStarted();

            // Determine which Tabs to show in the ucoPartnerTabSet
            FPartnerClass = FMainDS.PPartner[0].PartnerClass;
            TabsToHide = new ArrayList();

            if (FPartnerClass == "PERSON")
            {
                TabsToHide.Add("tpgFoundationDetails");

                // instead of 'Family Members (?)'
                tpgFamilyMembers.Text = "Family";

                // instead of 'FamilyMembers.ico'
                tpgFamilyMembers.ImageIndex = 4;
            }
            else if (FPartnerClass == "FAMILY")
            {
                // TabsToHide.Add('tbpFamily');
                TabsToHide.Add("tpgFoundationDetails");
            }
            else if (FPartnerClass == "CHURCH")
            {
                TabsToHide.Add("tpgFamilyMembers");
                TabsToHide.Add("tpgFoundationDetails");
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                TabsToHide.Add("tpgFamilyMembers");

                if (!FMainDS.POrganisation[0].Foundation)
                {
                    TabsToHide.Add("tpgFoundationDetails");
                }
                else
                {
                    if (!TSecurity.CheckFoundationSecurity(
                            FMainDS.MiscellaneousData[0].FoundationOwner1Key,
                            FMainDS.MiscellaneousData[0].FoundationOwner2Key))
                    {
                        tpgFoundationDetails.Enabled = false;
                    }

                    if (!CheckSecurityOKToAccessNotesTab())
                    {
                        tpgNotes.Enabled = false;
                    }
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                TabsToHide.Add("tpgFamilyMembers");
                TabsToHide.Add("tpgFoundationDetails");
            }
            else if (FPartnerClass == "BANK")
            {
                TabsToHide.Add("tpgFamilyMembers");
                TabsToHide.Add("tpgFoundationDetails");
            }
            else if (FPartnerClass == "VENUE")
            {
                TabsToHide.Add("tpgFamilyMembers");
                TabsToHide.Add("tpgFoundationDetails");
            }

            if (!FMainDS.MiscellaneousData[0].OfficeSpecificDataLabelsAvailable)
            {
                TabsToHide.Add("tpgOfficeSpecific");
            }

            // for the time being, we always hide these Tabs that don't do anything yet...
#if  SHOWUNFINISHEDTABS
#else
            TabsToHide.Add("tbpReminders");
            TabsToHide.Add("tbpInterests");
#endif
            ControlsUtilities.HideTabs(tabPartners, TabsToHide);
            FUserControlInitialised = true;

            tabPartners.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            // We must switch to the selected TabPage only once the the 'Shown' Event of the Form has been run
            // to make sure that the TabControl does not show the selected TabPage leftmost, but at its' correct
            // place in the order of the Tabs. (See Bug https://tracker.openpetra.org/view.php?id=2392)
            if (FPetraUtilsObject.FormHasBeenShown)
            {
                SelectTabPage(FInitiallySelectedTabPage);
            }

            CalculateTabHeaderCounters(this);

            OnDataLoadingFinished();
        }

        private void RunOnceOnParentActivationManual()
        {
            FUcoAddresses.RunOnceOnParentActivation();
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ADataValidationProcessingMode">Set to TErrorProcessingMode.Epm_All if data validation errors should be shown to the
        /// user, otherwise set it to TErrorProcessingMode.Epm_None.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=null).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(TErrorProcessingMode ADataValidationProcessingMode, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

            switch (GetPartnerDetailsVariableUC())
            {
                case TDynamicLoadableUserControls.dlucPartnerDetailsPerson:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsPerson))
                    {
                        TUC_PartnerDetails_Person UCPartnerDetailsPerson =
                            (TUC_PartnerDetails_Person)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsPerson];

                        if (!UCPartnerDetailsPerson.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsFamily:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsFamily))
                    {
                        TUC_PartnerDetails_Family UCPartnerDetailsFamily =
                            (TUC_PartnerDetails_Family)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsFamily];

                        if (!UCPartnerDetailsFamily.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation))
                    {
                        TUC_PartnerDetails_Organisation UCPartnerDetailsOrganisation =
                            (TUC_PartnerDetails_Organisation)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation];

                        if (!UCPartnerDetailsOrganisation.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsChurch:

                    // Special case: The Church UserControl needs to always be initialised in order for the Validation to work also when the Tab was never switched to
                    // (for checking for empty DenominationList CacheableDataTable)!
                    if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsChurch))
                    {
                        SetupVariableUserControlForTabPagePartnerDetails();
                    }

                    TUC_PartnerDetails_Church UCPartnerDetailsChurch =
                        (TUC_PartnerDetails_Church)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsChurch];

                    if (!UCPartnerDetailsChurch.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsUnit:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsUnit))
                    {
                        TUC_PartnerDetails_Unit UCPartnerDetailsUnit =
                            (TUC_PartnerDetails_Unit)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsUnit];

                        if (!UCPartnerDetailsUnit.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsVenue:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsVenue))
                    {
                        TUC_PartnerDetails_Venue UCPartnerDetailsVenue =
                            (TUC_PartnerDetails_Venue)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsVenue];

                        if (!UCPartnerDetailsVenue.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsBank:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsBank))
                    {
                        TUC_PartnerDetails_Bank UCPartnerDetailsBank =
                            (TUC_PartnerDetails_Bank)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsBank];

                        if (!UCPartnerDetailsBank.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                        {
                            ReturnValue = false;
                        }
                    }

                    break;
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                TUC_PartnerAddresses UCPartnerAddresses =
                    (TUC_PartnerAddresses)FTabSetup[TDynamicLoadableUserControls.dlucAddresses];

                if (!UCPartnerAddresses.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucContacts))
            {
                TUC_Contacts UCContactLogs = (TUC_Contacts)FTabSetup[TDynamicLoadableUserControls.dlucContacts];

                if (!UCContactLogs.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucContactDetails))
            {
                TUC_ContactDetails UCContactDetails =
                    (TUC_ContactDetails)FTabSetup[TDynamicLoadableUserControls.dlucContactDetails];

                if (!UCContactDetails.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }

                if (!UCContactDetails.GetOverallContactSettingsDataFromControls())
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucSubscriptions))
            {
                TUC_Subscriptions UCSubscriptions =
                    (TUC_Subscriptions)FTabSetup[TDynamicLoadableUserControls.dlucSubscriptions];

                if (!UCSubscriptions.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerTypes))
            {
                TUCPartnerTypes UCPartnerTypes =
                    (TUCPartnerTypes)FTabSetup[TDynamicLoadableUserControls.dlucPartnerTypes];

                if (!UCPartnerTypes.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerRelationships))
            {
                TUC_PartnerRelationships UCRelationships =
                    (TUC_PartnerRelationships)FTabSetup[TDynamicLoadableUserControls.dlucPartnerRelationships];

                if (!UCRelationships.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucFamilyMembers))
            {
                TUC_FamilyMembers UCFamilyMembers =
                    (TUC_FamilyMembers)FTabSetup[TDynamicLoadableUserControls.dlucFamilyMembers];

                if (!UCFamilyMembers.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucNotes))
            {
                TUC_PartnerNotes UCNotes =
                    (TUC_PartnerNotes)FTabSetup[TDynamicLoadableUserControls.dlucNotes];

                if (!UCNotes.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucFinanceDetails))
            {
                TUC_FinanceDetails UCFinanceDetails =
                    (TUC_FinanceDetails)FTabSetup[TDynamicLoadableUserControls.dlucFinanceDetails];

                // enable extra validation on tab change
                UCFinanceDetails.ValidateBankingDetailsExtra = true;

                if (!UCFinanceDetails.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }

                if (!UCFinanceDetails.GetPartnerDataFromControls())
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucOfficeSpecific))
            {
                TUC_LocalPartnerData UCLocalPartnerData =
                    (TUC_LocalPartnerData)FTabSetup[TDynamicLoadableUserControls.dlucOfficeSpecific];

                if (!UCLocalPartnerData.ValidateAllData(ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucInterests))
            {
                TUC_PartnerInterests UCPartnerInterests =
                    (TUC_PartnerInterests)FTabSetup[TDynamicLoadableUserControls.dlucInterests];

                if (!UCPartnerInterests.ValidateAllData(false, ADataValidationProcessingMode, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the data from all controls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls()
        {
            switch (GetPartnerDetailsVariableUC())
            {
                case TDynamicLoadableUserControls.dlucPartnerDetailsPerson:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsPerson))
                    {
                        TUC_PartnerDetails_Person UCPartnerDetailsPerson =
                            (TUC_PartnerDetails_Person)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsPerson];
                        UCPartnerDetailsPerson.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsFamily:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsFamily))
                    {
                        TUC_PartnerDetails_Family UCPartnerDetailsFamily =
                            (TUC_PartnerDetails_Family)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsFamily];
                        UCPartnerDetailsFamily.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation))
                    {
                        TUC_PartnerDetails_Organisation UCPartnerDetailsOrganisation =
                            (TUC_PartnerDetails_Organisation)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation];
                        UCPartnerDetailsOrganisation.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsChurch:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsChurch))
                    {
                        TUC_PartnerDetails_Church UCPartnerDetailsChurch =
                            (TUC_PartnerDetails_Church)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsChurch];
                        UCPartnerDetailsChurch.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsUnit:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsUnit))
                    {
                        TUC_PartnerDetails_Unit UCPartnerDetailsUnit =
                            (TUC_PartnerDetails_Unit)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsUnit];
                        UCPartnerDetailsUnit.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsVenue:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsVenue))
                    {
                        TUC_PartnerDetails_Venue UCPartnerDetailsVenue =
                            (TUC_PartnerDetails_Venue)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsVenue];
                        UCPartnerDetailsVenue.GetDataFromControls2();
                    }

                    break;

                case TDynamicLoadableUserControls.dlucPartnerDetailsBank:

                    if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucPartnerDetailsBank))
                    {
                        TUC_PartnerDetails_Bank UCPartnerDetailsBank =
                            (TUC_PartnerDetails_Bank)FTabSetup[TDynamicLoadableUserControls.dlucPartnerDetailsBank];
                        UCPartnerDetailsBank.GetDataFromControls2();
                    }

                    break;
            }
        }

        /// <summary>
        /// Tells whether a specific dynamically loadable Tab has been set up.
        /// </summary>
        /// <param name="ADynamicLoadableUserControl">The Tab.</param>
        /// <returns>True if it has been set up, otherwise false.</returns>
        public bool IsDynamicallyLoadableTabSetUp(TDynamicLoadableUserControls ADynamicLoadableUserControl)
        {
            if (FTabSetup.ContainsKey(ADynamicLoadableUserControl))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void SetUpPartnerAddressTab()
        {
            SetupUserControlAddresses();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CleanupRecordsBeforeMerge()
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            FUcoAddresses.CleanupRecordsBeforeMerge();

            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucContactDetails))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucContactDetails);'
                SetupUserControlContactDetails();
            }

            FUcoContactDetails.CleanupRecordsBeforeMerge();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshRecordsAfterMerge()
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            FUcoAddresses.RefreshRecordsAfterMerge();

            FUcoContactDetails.RefreshRecordsAfterMerge();

            if (FUcoFinanceDetails != null)
            {
                FUcoFinanceDetails.RefreshRecordsAfterMerge();
            }

            if (FUcoPartnerTypes != null)
            {
                FUcoPartnerTypes.RefreshDataGrid();
            }
        }

        /// <summary>
        /// Refreshes the Family Members list on the Family tab
        /// </summary>
        public void RefreshFamilyMembersList(TFormsMessage AFormsMessage)
        {
            IFormsMessagePartnerInterface FormsMessagePartner;

            if (FUcoFamilyMembers != null)
            {
                FormsMessagePartner = (IFormsMessagePartnerInterface)AFormsMessage.MessageObject;

                // return if the partner that has just been deleted/edited is not a member of this partner's family
                if (((AFormsMessage.MessageClass == TFormsMessageClassEnum.mcPartnerDeleted)
                     || (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcExistingPartnerSaved))
                    && (FMainDS.FamilyMembers.Rows.Find(new object[] { FormsMessagePartner.PartnerKey }) == null))
                {
                    return;
                }

                FUcoFamilyMembers.BroadcastRefresh = true;
                FUcoFamilyMembers.RefreshFamilyMembersList(null, null);
                FUcoFamilyMembers.BroadcastRefresh = false;
            }
        }

        /// <summary>
        /// Refreshes position in Uni Hierarchy
        /// </summary>
        public void RefreshUnitHierarchy(Tuple <string, Int64, Int64>AUnitHierarchyChange)
        {
            // if partner details tab has not yet been initialised
            if (FUcoPartnerDetailsUnit == null)
            {
                SetupVariableUserControlForTabPagePartnerDetails();
            }

            FUcoPartnerDetailsUnit.RefreshUnitHierarchy(AUnitHierarchyChange);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AParameterDT"></param>
        public void ProcessServerResponseSimilarLocations(PartnerAddressAggregateTDSSimilarLocationParametersTable AParameterDT)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            FUcoAddresses.ProcessServerResponseSimilarLocations(AParameterDT);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AAddedOrChangedPromotionDT"></param>
        /// <param name="AParameterDT"></param>
        public void ProcessServerResponseAddressAddedOrChanged(
            PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddedOrChangedPromotionDT,
            PartnerAddressAggregateTDSChangePromotionParametersTable AParameterDT)
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            FUcoAddresses.ProcessServerResponseAddressAddedOrChanged(AAddedOrChangedPromotionDT, AParameterDT);
        }

        /// <summary>
        /// Selects the given contact log.
        /// </summary>
        /// <param name="AContactLogID">Contact Log identifier.</param>
        public void SelectContactLog(string AContactLogID)
        {
            tabPartners.SelectedTab = tpgContacts;
            TabSelectionChanged(this, null);
            FUcoContacts.SelectContactLogID(AContactLogID);
        }

        #endregion

        #region Private Methods

        private TDynamicLoadableUserControls GetPartnerDetailsVariableUC()
        {
            switch (FPartnerClass)
            {
                case "PERSON":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsPerson;

                case "FAMILY":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsFamily;

                case "ORGANISATION":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation;

                case "CHURCH":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsChurch;

                case "UNIT":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsUnit;

                case "VENUE":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsVenue;

                case "BANK":
                    return TDynamicLoadableUserControls.dlucPartnerDetailsBank;

                default:
                    return TDynamicLoadableUserControls.dlucPartnerDetailsPerson;
            }
        }

        private void TabPageEventHandler(object sender, TTabPageEventArgs ATabPageEventArgs)
        {
            SetCurrentlySelectedTabPage(ATabPageEventArgs.Tab);

            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgAddresses)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoAddresses.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoAddresses.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoAddresses.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupDataChange);

                    FUcoAddresses.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgContactDetails)
                {
                    FUcoContactDetails.SpecialInitUserControl();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerDetails)
                {
                    // TODO
                }
                else if (ATabPageEventArgs.Tab == tpgFoundationDetails)
                {
                    // TODO
                }
                else if (ATabPageEventArgs.Tab == tpgSubscriptions)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoSubscriptions.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoSubscriptions.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoSubscriptions.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoSubscriptions.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerTypes)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerTypes.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoPartnerTypes.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPartnerTypes.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoPartnerTypes.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerRelationships)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerRelationships.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoPartnerRelationships.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPartnerRelationships.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoPartnerRelationships.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgFamilyMembers)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoFamilyMembers.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoFamilyMembers.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoFamilyMembers.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);
                    FUcoFamilyMembers.InitialiseDelegateIsNewPartner(FDelegateIsNewPartner);
                    FUcoFamilyMembers.InitialiseDelegateGetPartnerLocationRowOfCurrentlySelectedAddress(
                        Get_LocationRowOfCurrentlySelectedAddress);

                    FUcoFamilyMembers.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgNotes)
                {
                    // Hook up RecalculateScreenParts Event
                    FUcoNotes.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);
                }
                else if (ATabPageEventArgs.Tab == tpgOfficeSpecific)
                {
                    FUcoOfficeSpecific.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoOfficeSpecific.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoOfficeSpecific.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgFinanceDetails)
                {
                    // see PreInitUserControl below
                    FUcoFinanceDetails.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);
                }
                else if (ATabPageEventArgs.Tab == tpgInterests)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpInterests;

                    // Hook up RecalculateScreenParts Event
                    FUcoInterests.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoInterests.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoInterests.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoInterests.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgContacts)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpContacts;

                    FUcoContacts.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoContacts.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);
                    FUcoContacts.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);
                }

                FPetraUtilsObject.RestoreAdditionalWindowPositionProperties();
            }
        }

        /// <summary>
        /// This Method *CAN* be implemented in ManualCode to perform special initialisations *before*
        /// InitUserControl() gets called.
        /// </summary>
        partial void PreInitUserControl(UserControl AUserControl)
        {
            if (AUserControl is TUC_FinanceDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFinanceDetails;

                FUcoFinanceDetails.PartnerEditUIConnector = FPartnerEditUIConnector;

                FUcoFinanceDetails.PreInitUserControl(FMainDS);

                CorrectDataGridWidthsAfterDataChange();
            }
            else if (AUserControl is TUC_ContactDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpContactDetails;

                // Hook up RecalculateScreenParts Event
                FUcoContactDetails.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                CorrectDataGridWidthsAfterDataChange();
            }
        }

        /// <summary>
        /// This Method *CAN* be implemented in ManualCode to perform special initialisations *after*
        /// InitUserControl() gets called.
        /// </summary>
        partial void PostInitUserControl(UserControl AUserControl)
        {
            if (AUserControl is TUC_PartnerAddresses)
            {
                FUcoAddresses.PostInitUserControl(FMainDS);

                CorrectDataGridWidthsAfterDataChange();
            }
            else if (AUserControl is TUC_ContactDetails)
            {
                FUcoContactDetails.PostInitUserControl(FMainDS);

                CorrectDataGridWidthsAfterDataChange();
            }
        }

        private void RecalculateTabHeaderCounters(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet2.RecalculateTabHeaderCounters');
            if (e.ScreenPart == TScreenPartEnum.spCounters)
            {
                CalculateTabHeaderCounters(sender);
            }
        }

        private void CalculateTabHeaderCounters(System.Object ASender)
        {
            DataView TmpDV;
            string DynamicTabTitle;
            string DynamicToolTipPart1;
            Int32 CountAll;
            Int32 CountActive;

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_PartnerAddresses))
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
                    tpgAddresses.Text = String.Format(StrAddressesTabHeader + " ({0}!)", CountActive);
                    tpgAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "current", CountAll, CountActive, DynamicToolTipPart1);
                }
                else
                {
                    tpgAddresses.Text = String.Format(StrAddressesTabHeader + " ({0})", CountActive);

                    if (CountActive > 1)
                    {
                        tpgAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "current", CountAll, CountActive, DynamicToolTipPart1);
                    }
                    else
                    {
                        tpgAddresses.ToolTipText = String.Format(StrTabHeaderCounterTipSingular + "current",
                            CountAll,
                            CountActive,
                            DynamicToolTipPart1);
                    }
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_ContactDetails))
            {
                if (FMainDS.Tables.Contains(PPartnerAttributeTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsPartnerContactDetails(FMainDS.PPartnerAttribute, out CountAll, out CountActive);
                    tpgContactDetails.Text = String.Format(StrContactDetailsTabHeader + " ({0})", CountActive);
                }
                else
                {
                    CountAll = 0;
                    CountActive = 0;
                }

                if ((CountAll == 0) || (CountAll > 1))
                {
                    DynamicToolTipPart1 = StrContactDetailsTabHeader;
                }
                else
                {
                    DynamicToolTipPart1 = StrContactDetailsSingular;
                }

                tpgContactDetails.Text = String.Format(StrContactDetailsTabHeader + " ({0})", CountActive);

                if ((CountActive == 0) || (CountActive > 1))
                {
                    tpgContactDetails.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "current",
                        CountAll,
                        CountActive,
                        DynamicToolTipPart1);
                }
                else
                {
                    tpgContactDetails.ToolTipText = String.Format(StrTabHeaderCounterTipSingular + "current",
                        CountAll,
                        CountActive,
                        DynamicToolTipPart1);
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_Subscriptions))
            {
                if (FMainDS.Tables.Contains(PSubscriptionTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsSubscriptions(FMainDS.PSubscription, out CountAll, out CountActive);
                    tpgSubscriptions.Text = String.Format(StrSubscriptionsTabHeader + " ({0})", CountActive);
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

                tpgSubscriptions.Text = String.Format(StrSubscriptionsTabHeader + " ({0})", CountActive);

                if ((CountActive == 0) || (CountActive > 1))
                {
                    tpgSubscriptions.ToolTipText = String.Format(StrTabHeaderCounterTipPlural + "active", CountAll, CountActive, DynamicToolTipPart1);
                }
                else
                {
                    tpgSubscriptions.ToolTipText = String.Format(StrTabHeaderCounterTipSingular + "active",
                        CountAll,
                        CountActive,
                        DynamicToolTipPart1);
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_Contacts))
            {
                if (FMainDS.Tables.Contains(PContactLogTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsContacts(FMainDS.PContactLog, out CountAll);
                    tpgContacts.Text = String.Format(StrContactsTabHeader + " ({0})", CountAll);
                }
                else
                {
                    CountAll = FMainDS.MiscellaneousData[0].ItemsCountContacts;
                }

                if ((CountAll == 0) || (CountAll > 1))
                {
                    DynamicToolTipPart1 = StrContactsTabHeader;
                }
                else
                {
                    DynamicToolTipPart1 = StrContactsSingular;
                }

                tpgContacts.Text = String.Format(StrContactsTabHeader + " ({0})", CountAll);
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerTypes))
            {
                if (FMainDS.Tables.Contains(PPartnerTypeTable.GetTableName()))
                {
                    TmpDV = new DataView(FMainDS.PPartnerType, "", "", DataViewRowState.CurrentRows);
                    tpgPartnerTypes.Text = StrSpecialTypesTabHeader + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tpgPartnerTypes.Text = StrSpecialTypesTabHeader + " (" + FMainDS.MiscellaneousData[0].ItemsCountPartnerTypes.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_PartnerRelationships))
            {
                if (FMainDS.Tables.Contains(PPartnerRelationshipTable.GetTableName()))
                {
                    Calculations.CalculateTabCountsPartnerRelationships(FMainDS.PPartnerRelationship, out CountAll);
                }
                else
                {
                    CountAll = FMainDS.MiscellaneousData[0].ItemsCountPartnerRelationships;
                }

                tpgPartnerRelationships.Text = String.Format(StrPartnerRelationshipsTabHeader + " ({0})", CountAll);
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
                    tpgFamilyMembers.Text = DynamicTabTitle + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tpgFamilyMembers.Text = DynamicTabTitle + " (" + FMainDS.MiscellaneousData[0].ItemsCountFamilyMembers.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUCPartnerInterests))
            {
                if (FMainDS.Tables.Contains(PPartnerInterestTable.GetTableName()))
                {
                    TmpDV = new DataView(FMainDS.PPartnerInterest, "", "", DataViewRowState.CurrentRows);
                    tpgInterests.Text = StrInterestsTabHeader + " (" + TmpDV.Count.ToString() + ')';
                }
                else
                {
                    tpgInterests.Text = StrInterestsTabHeader + " (" + FMainDS.MiscellaneousData[0].ItemsCountInterests.ToString() + ')';
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_PartnerNotes))
            {
                if ((FMainDS.PPartner[0].IsCommentNull()) || (FMainDS.PPartner[0].Comment == ""))
                {
                    tpgNotes.Text = String.Format(StrNotesTabHeader + " ({0})", 0);
                    tpgNotes.ToolTipText = "No Notes entered";
                }
                else
                {
                    // 8730 = 'square root symbol' in Verdana
                    tpgNotes.Text = String.Format(StrNotesTabHeader + " ({0})", (char)8730);
                    tpgNotes.ToolTipText = "Notes are entered";
                }
            }

            if ((ASender is TUC_PartnerEdit_PartnerTabSet) || (ASender is TUC_FinanceDetails))
            {
                if (FMainDS.Tables.Contains(PPartnerBankingDetailsTable.GetTableName()))
                {
                    CountAll = FMainDS.PPartnerBankingDetails.Rows.Count;
                }
                else
                {
                    CountAll = FMainDS.MiscellaneousData[0].ItemsCountPartnerBankingDetails;
                }

                tpgFinanceDetails.Text = String.Format(StrFinanceDetailsTabHeader + " ({0})", CountAll);
            }
        }

        /// <summary>
        /// Changed data (eg. caused by the data saving process) will make a databound SourceGrid redraw,
        /// and through that it can get it's size wrong and appear too wide if the user has
        /// a non-standard display setting, (eg. "Large Fonts (120DPI).
        /// This Method fixes that by calling the 'AdjustAfterResizing' Method in Tabs that
        /// host a SourceGrid.
        /// </summary>
        private void CorrectDataGridWidthsAfterDataChange()
        {
            if (TClientSettings.GUIRunningOnNonStandardDPI)
            {
                if (FUcoAddresses != null)
                {
                    FUcoAddresses.AdjustAfterResizing();
                }

                if (FUcoContactDetails != null)
                {
                    FUcoContactDetails.AdjustAfterResizing();
                }

                if (FUcoSubscriptions != null)
                {
                    FUcoSubscriptions.AdjustAfterResizing();
                }

                if (FUcoPartnerTypes != null)
                {
                    FUcoPartnerTypes.AdjustAfterResizing();
                }

                if (FUcoPartnerRelationships != null)
                {
                    FUcoPartnerRelationships.AdjustAfterResizing();
                }

                if (FUcoFamilyMembers != null)
                {
                    FUcoFamilyMembers.AdjustAfterResizing();
                }

                if (FUcoOfficeSpecific != null)
                {
                    FUcoOfficeSpecific.AdjustAfterResizing();
                }
            }
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

        private void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(TErrorProcessingMode.Epm_All, FCurrentUserControl))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
        }

        private void SetCurrentlySelectedTabPage(TabPage ATabPage)
        {
            if (ATabPage == tpgAddresses)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;
            }
            else if (ATabPage == tpgContactDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpContactDetails;
            }
            else if (ATabPage == tpgPartnerDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpDetails;
            }
            else if (ATabPage == tpgFoundationDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFoundationDetails;
            }
            else if (ATabPage == tpgSubscriptions)
            {
                CurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpSubscriptions;
            }
            else if (ATabPage == tpgPartnerTypes)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerTypes;
            }
            else if (ATabPage == tpgPartnerRelationships)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerRelationships;
            }
            else if (ATabPage == tpgFamilyMembers)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFamilyMembers;
            }
            else if (ATabPage == tpgNotes)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpNotes;
            }
            else if (ATabPage == tpgOfficeSpecific)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpOfficeSpecific;
            }
            else if (ATabPage == tpgFinanceDetails)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFinanceDetails;
            }
            else if (ATabPage == tpgInterests)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpInterests;
            }
            else if (ATabPage == tpgContacts)
            {
                FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpContacts;
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

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabPage"></param>
        public void SelectTabPage(TPartnerEditTabPageEnum ATabPage)
        {
            TabPage SelectedTabPageBeforeReSelecting;

            if (!FUserControlInitialised)
            {
                throw new EOPAppException("SelectTabPage must not be called if the UserControl is not yet initialised");
            }

            OnDataLoadingStarted();

            // supress detection changing
            SelectedTabPageBeforeReSelecting = tabPartners.SelectedTab;

            try
            {
                switch (ATabPage)
                {
                    case TPartnerEditTabPageEnum.petpAddresses:
                    case TPartnerEditTabPageEnum.petpDefault:
                        tabPartners.SelectedTab = tpgAddresses;
                        break;

                    case TPartnerEditTabPageEnum.petpDetails:
                        tabPartners.SelectedTab = tpgPartnerDetails;
                        break;

                    case TPartnerEditTabPageEnum.petpContactDetails:
                        tabPartners.SelectedTab = tpgContactDetails;
                        break;

                    case TPartnerEditTabPageEnum.petpFoundationDetails:
                        tabPartners.SelectedTab = tpgFoundationDetails;
                        break;

                    case TPartnerEditTabPageEnum.petpSubscriptions:
                        tabPartners.SelectedTab = tpgSubscriptions;
                        break;

                    case TPartnerEditTabPageEnum.petpPartnerTypes:
                        tabPartners.SelectedTab = tpgPartnerTypes;
                        break;

                    case TPartnerEditTabPageEnum.petpPartnerRelationships:
                        tabPartners.SelectedTab = tpgPartnerRelationships;
                        break;

                    case TPartnerEditTabPageEnum.petpFamilyMembers:
                        tabPartners.SelectedTab = tpgFamilyMembers;
                        break;

                    case TPartnerEditTabPageEnum.petpOfficeSpecific:
                        tabPartners.SelectedTab = tpgOfficeSpecific;
                        break;

                    case TPartnerEditTabPageEnum.petpInterests:
                        tabPartners.SelectedTab = tpgInterests;
                        break;

#if TODO
                    case TPartnerEditTabPageEnum.petpReminders:
                        tabPartners.SelectedTab = tpgReminders;
                        break;
#endif
                    case TPartnerEditTabPageEnum.petpContacts:
                        tabPartners.SelectedTab = tpgContacts;
                        break;

                    case TPartnerEditTabPageEnum.petpNotes:

                        if (tpgNotes.Enabled)
                        {
                            tabPartners.SelectedTab = tpgNotes;
                        }
                        else
                        {
                            tabPartners.SelectedTab = tpgAddresses;
                        }

                        break;

                    case TPartnerEditTabPageEnum.petpFinanceDetails:
                        tabPartners.SelectedTab = tpgFinanceDetails;
                        break;
                }
            }
            catch (Ict.Common.Controls.ESelectedIndexChangeDisallowedTabPagedIsDisabledException)
            {
                // Desired Tab Page isn't selectable because it is disabled; ignoring this Exception to ignore the selection.
            }

            // Check if the selected TabPage actually changed...
            if (SelectedTabPageBeforeReSelecting == tabPartners.SelectedTab)
            {
                // Tab was already selected; therefore raise the SelectedIndexChanged Event 'manually', which did not get raised by selecting the Tab again!
                TabSelectionChanged(this, new System.EventArgs());
            }

            OnDataLoadingFinished();
        }

        #endregion

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if ((this.tabPartners.SelectedTab == tpgAddresses) && (this.FUcoAddresses.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if (this.tabPartners.SelectedTab == tpgPartnerDetails)
            {
                switch (GetPartnerDetailsVariableUC())
                {
                    case TDynamicLoadableUserControls.dlucPartnerDetailsFamily:

                        if (this.FUcoPartnerDetailsFamily.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsPerson:

                        if (this.FUcoPartnerDetailsPerson.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsBank:

                        if (this.FUcoPartnerDetailsBank.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsChurch:

                        if (this.FUcoPartnerDetailsChurch.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsUnit:

                        if (this.FUcoPartnerDetailsUnit.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsOrganisation:

                        if (this.FUcoPartnerDetailsOrganisation.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;

                    case TDynamicLoadableUserControls.dlucPartnerDetailsVenue:

                        if (this.FUcoPartnerDetailsVenue.ProcessParentCmdKey(ref msg, keyData))
                        {
                            return true;
                        }

                        break;
                }
            }
            else if ((this.tabPartners.SelectedTab == tpgContactDetails) && (this.FUcoContactDetails.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((this.tabPartners.SelectedTab == tpgPartnerRelationships) && (this.FUcoPartnerRelationships.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((this.tabPartners.SelectedTab == tpgFinanceDetails) && (this.FUcoFinanceDetails.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((this.tabPartners.SelectedTab == tpgInterests) && (this.FUcoInterests.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((this.tabPartners.SelectedTab == tpgFamilyMembers) && (this.FUcoFamilyMembers.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((this.tabPartners.SelectedTab == tpgSubscriptions) && (this.FUcoSubscriptions.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}