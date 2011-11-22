//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
// using Ict.Petra.Shared.MPartner.Partner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>Delegate declaration</summary>
    public delegate PLocationRow TDelegateGetLocationRowOfCurrentlySelectedAddress();

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
        #region TODO ResourceStrings

        /// <summary>todoComment</summary>
        public const String StrAddressesTabHeader = "Addresses";

        /// <summary>todoComment</summary>
        public const String StrSubscriptionsTabHeader = "Subscriptions";

        /// <summary>todoComment</summary>
        public const String StrSpecialTypesTabHeader = "Special Types";

        /// <summary>todoComment</summary>
        public const String StrPartnerRelationshipsTabHeader = "Relationships";

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

        #endregion

        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TPartnerEditTabPageEnum FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpDefault;

        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;

        private String FPartnerClass;

        private Boolean FUserControlInitialised;

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

        #endregion

        #region Public Methods

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

            // for the time beeing, we always hide these Tabs that don't do anything yet...
#if  SHOWUNFINISHEDTABS
#else
            TabsToHide.Add("tbpContacts");
            TabsToHide.Add("tbpReminders");
            TabsToHide.Add("tbpInterests");
#endif
            ControlsUtilities.HideTabs(tabPartners, TabsToHide);
            FUserControlInitialised = true;

            SelectTabPage(FInitiallySelectedTabPage);

            CalculateTabHeaderCounters(this);

            OnDataLoadingFinished();
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
        public void SetUpPartnerAddress()
        {
            FUcoAddresses = (TUCPartnerAddresses)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);

            if (TClientSettings.DelayedDataLoading)
            {
                // Signalise the user that data is beeing loaded
                this.Cursor = Cursors.AppStarting;
            }

            FUcoAddresses.MainDS = FMainDS;
            FUcoAddresses.PetraUtilsObject = FPetraUtilsObject;
            FUcoAddresses.PartnerEditUIConnector = FPartnerEditUIConnector;
            FUcoAddresses.HookupDataChange += new THookupDataChangeEventHandler(this.Uco_HookupDataChange);
            FUcoAddresses.InitialiseUserControl();

            // MessageBox.Show('TabSetupPartnerAddresses finished');
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisableNewButtonOnAutoCreatedAddress()
        {
            if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucAddresses))
            {
                // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                SetupUserControlAddresses();
            }

            FUcoAddresses.DisableNewButtonOnAutoCreatedAddress();
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
            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgAddresses)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpAddresses;

                    // Hook up EnableDisableOtherScreenParts Event that is fired by UserControls on Tabs
                    FUcoAddresses.EnableDisableOtherScreenParts += new TEnableDisableScreenPartsEventHandler(
                        UcoTab_EnableDisableOtherScreenParts);

                    // Hook up RecalculateScreenParts Event
                    FUcoAddresses.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoAddresses.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoAddresses.HookupDataChange += new THookupDataChangeEventHandler(Uco_HookupDataChange);

                    FUcoAddresses.InitialiseUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerDetails)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpDetails;

                    // TODO
                }
                else if (ATabPageEventArgs.Tab == tpgFoundationDetails)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFoundationDetails;

                    // TODO
                }
                else if (ATabPageEventArgs.Tab == tpgSubscriptions)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpSubscriptions;

                    // Hook up RecalculateScreenParts Event
                    FUcoSubscriptions.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoSubscriptions.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoSubscriptions.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoSubscriptions.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerTypes)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerTypes;

                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerTypes.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoPartnerTypes.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPartnerTypes.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoPartnerTypes.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgPartnerRelationships)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPartnerRelationships;

                    // Hook up RecalculateScreenParts Event
                    FUcoPartnerRelationships.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoPartnerRelationships.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPartnerRelationships.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoPartnerRelationships.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgFamilyMembers)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpFamilyMembers;

                    // Hook up RecalculateScreenParts Event
                    FUcoFamilyMembers.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

                    FUcoFamilyMembers.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoFamilyMembers.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoFamilyMembers.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgNotes)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpNotes;

                    // TODO
                }
                else if (ATabPageEventArgs.Tab == tpgOfficeSpecific)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpOfficeSpecific;

                    FUcoOfficeSpecific.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoOfficeSpecific.HookupDataChange += new THookupPartnerEditDataChangeEventHandler(Uco_HookupPartnerEditDataChange);

                    FUcoOfficeSpecific.SpecialInitUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
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

#if TODO
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
#endif

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

                // TODO
//                if (FUcoFamilyMembers != null)
//                {
//                    FUcoFamilyMembers.AdjustAfterResizing();
//                }
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

        private void UcoTab_EnableDisableOtherScreenParts(System.Object sender, TEnableDisableEventArgs e)
        {
            // MessageBox.Show('TUC_PartnerEdit_PartnerTabSet2.ucoTab_EnableDisableOtherScreenParts(' + e.Enable.ToString + ')');
            // Simply fire OnEnableDisableOtherScreenParts event again so that the PartnerEdit screen can catch it
            OnEnableDisableOtherScreenParts(e);

            // Disable all TabPages except the current one (and reverse that)
            tabPartners.EnableDisableAllOtherTabPages(e.Enable);
        }

        private void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
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
                throw new ApplicationException("SelectTabPage must not be called if the UserControl is not yet initialised");
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

#if TODO
                    case TPartnerEditTabPageEnum.petpInterests:
                        tabPartners.SelectedTab = tpgInterests;
                        break;

                    case TPartnerEditTabPageEnum.petpReminders:
                        tabPartners.SelectedTab = tpgReminders;
                        break;

                    case TPartnerEditTabPageEnum.petpRelationships:
                        tabPartners.SelectedTab = tpgRelationships;
                        break;

                    case TPartnerEditTabPageEnum.petpContacts:
                        tabPartners.SelectedTab = tpgContacts;
                        break;
#endif
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
                }
            }
            catch (Ict.Common.Controls.TSelectedIndexChangeDisallowedTabPagedIsDisabledException)
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
    }
}