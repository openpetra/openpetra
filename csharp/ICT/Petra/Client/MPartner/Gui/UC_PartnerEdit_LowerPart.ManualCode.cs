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

using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerEdit_LowerPart
    {
        #region Fields

        private TFrmPartnerEdit.TModuleSwitchEnum FCurrentModuleTabGroup;
        private TPartnerEditTabPageEnum FInitiallySelectedTabPage;
        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        // private TDelegateIsNewPartner FDelegateIsNewPartner;

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event THookupDataChangeEventHandler HookupDataChange;

        /// <summary>
        /// Raises Event HookupDataChange.
        /// </summary>
        /// <param name="e">Event parameters</param>
        /// <returns>void</returns>
        protected void OnHookupDataChange(System.EventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupPartnerEditDataChange;

        /// <summary>
        /// Raises Event HookupPartnerEditDataChange.
        /// </summary>
        /// <param name="e">Event parameters</param>
        /// <returns>void</returns>
        protected void OnHookupPartnerEditDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupPartnerEditDataChange != null)
            {
                HookupPartnerEditDataChange(this, e);
            }
        }

        /// <summary>todoComment</summary>
        public event TEnableDisableScreenPartsEventHandler EnableDisableOtherScreenParts;

        /// <summary>
        /// Raises Event EnableDisableOtherScreenParts.
        /// </summary>
        /// <param name="e">Event parameters</param>
        /// <returns>void</returns>
        protected void OnEnableDisableOtherScreenParts(TEnableDisableEventArgs e)
        {
            if (EnableDisableOtherScreenParts != null)
            {
                EnableDisableOtherScreenParts(this, e);
            }
        }

        /// <summary>todoComment</summary>
        public event TShowTabEventHandler ShowTab;

        /// <summary>
        /// Raises Event ShowTab.
        /// </summary>
        /// <param name="e">Event parameters</param>
        /// <returns>void</returns>
        protected void OnShowTab(TShowTabEventArgs e)
        {
            if (ShowTab != null)
            {
                ShowTab(this, e);
            }
        }

        #endregion

        #region Properties

        /// <summary>todoComment</summary>
        public TFrmPartnerEdit.TModuleSwitchEnum CurrentModuleTabGroup
        {
            get
            {
                return FCurrentModuleTabGroup;
            }

            set
            {
                FCurrentModuleTabGroup = value;
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialises the UserControl that has the Tabs for the currently selected Tab.
        /// </summary>
        public void InitChildUserControl()
        {
            switch (FCurrentModuleTabGroup)
            {
                case TFrmPartnerEdit.TModuleSwitchEnum.msPartner:

                    ucoPartnerTabSet.PetraUtilsObject = FPetraUtilsObject;
                    ucoPartnerTabSet.PartnerEditUIConnector = FPartnerEditUIConnector;
                    ucoPartnerTabSet.InitiallySelectedTabPage = FInitiallySelectedTabPage;
                    ucoPartnerTabSet.MainDS = FMainDS;
                    ucoPartnerTabSet.SpecialInitUserControl();
                    ucoPartnerTabSet.HookupDataChange += new THookupDataChangeEventHandler(ucoPartnerTabSet_HookupDataChange);
                    ucoPartnerTabSet.HookupPartnerEditDataChange += new THookupPartnerEditDataChangeEventHandler(
                    ucoPartnerTabSet_HookupPartnerEditDataChange);
                    ucoPartnerTabSet.Visible = true;
                    ucoPersonnelTabSet.Visible = false;                    
                    break;

                case TFrmPartnerEdit.TModuleSwitchEnum.msPersonnel:

                    ucoPersonnelTabSet.PetraUtilsObject = FPetraUtilsObject;
                    ucoPersonnelTabSet.PartnerEditUIConnector = FPartnerEditUIConnector;
                    ucoPersonnelTabSet.InitiallySelectedTabPage = FInitiallySelectedTabPage;
                    ucoPersonnelTabSet.MainDS = FMainDS;
                    ucoPersonnelTabSet.SpecialInitUserControl();
                    ucoPersonnelTabSet.Visible = true;
                    ucoPartnerTabSet.Visible = false;
                    
                    break;                    
            }

            // TODO Other TabSets (Personnel Data, Finance Data)
        }

        void ucoPartnerTabSet_HookupPartnerEditDataChange(object Sender, THookupPartnerEditDataChangeEventArgs e)
        {
            OnHookupPartnerEditDataChange(e);
        }

        void ucoPartnerTabSet_HookupDataChange(object Sender, EventArgs e)
        {
            OnHookupDataChange(e);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitialiseDelegateIsNewPartner(TDelegateIsNewPartner ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            // TODO FDelegateIsNewPartner = ADelegateFunction;
        }

        /// <summary>
        /// Gets the data from all controls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls()
        {
            ucoPartnerTabSet.GetDataFromControls();
	    ucoPersonnelTabSet.GetDataFromControls();
			
            // TODO Other TabSets (Finance Data)
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void DisableNewButtonOnAutoCreatedAddress()
        {
            if (FCurrentModuleTabGroup == TFrmPartnerEdit.TModuleSwitchEnum.msPartner)
            {
                if (!ucoPartnerTabSet.IsDynamicallyLoadableTabSetUp(TUC_PartnerEdit_PartnerTabSet.TDynamicLoadableUserControls.dlucAddresses))
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    ucoPartnerTabSet.SetUpPartnerAddress();
                }

                ucoPartnerTabSet.DisableNewButtonOnAutoCreatedAddress();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void CleanupAddressesBeforeMerge()
        {
            if (FCurrentModuleTabGroup == TFrmPartnerEdit.TModuleSwitchEnum.msPartner)
            {
                if (!ucoPartnerTabSet.IsDynamicallyLoadableTabSetUp(TUC_PartnerEdit_PartnerTabSet.TDynamicLoadableUserControls.dlucAddresses))
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    ucoPartnerTabSet.SetUpPartnerAddress();
                }

                ucoPartnerTabSet.CleanupRecordsBeforeMerge();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshAddressesAfterMerge()
        {
            if (FCurrentModuleTabGroup == TFrmPartnerEdit.TModuleSwitchEnum.msPartner)
            {
                if (!ucoPartnerTabSet.IsDynamicallyLoadableTabSetUp(TUC_PartnerEdit_PartnerTabSet.TDynamicLoadableUserControls.dlucAddresses))
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    ucoPartnerTabSet.SetUpPartnerAddress();
                }

                ucoPartnerTabSet.RefreshRecordsAfterMerge();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AParameterDT"></param>
        public void SimilarLocationsProcessing(PartnerAddressAggregateTDSSimilarLocationParametersTable AParameterDT)
        {
            if (FCurrentModuleTabGroup == TFrmPartnerEdit.TModuleSwitchEnum.msPartner)
            {
                if (!ucoPartnerTabSet.IsDynamicallyLoadableTabSetUp(TUC_PartnerEdit_PartnerTabSet.TDynamicLoadableUserControls.dlucAddresses))
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    ucoPartnerTabSet.SetUpPartnerAddress();
                }

                ucoPartnerTabSet.ProcessServerResponseSimilarLocations(AParameterDT);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AAddedOrChangedPromotionDT"></param>
        /// <param name="AParameterDT"></param>
        public void AddressAddedOrChangedProcessing(PartnerAddressAggregateTDSAddressAddedOrChangedPromotionTable AAddedOrChangedPromotionDT,
            PartnerAddressAggregateTDSChangePromotionParametersTable AParameterDT)
        {
            if (FCurrentModuleTabGroup == TFrmPartnerEdit.TModuleSwitchEnum.msPartner)
            {
                if (!ucoPartnerTabSet.IsDynamicallyLoadableTabSetUp(TUC_PartnerEdit_PartnerTabSet.TDynamicLoadableUserControls.dlucAddresses))
                {
                    // The follwing function calls internally 'DynamicLoadUserControl(TDynamicLoadableUserControls.dlucAddresses);'
                    ucoPartnerTabSet.SetUpPartnerAddress();
                }

                ucoPartnerTabSet.ProcessServerResponseAddressAddedOrChanged(AAddedOrChangedPromotionDT, AParameterDT);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshPersonnelDataAfterMerge()
        {
	    ucoPersonnelTabSet.RefreshPersonnelDataAfterMerge();
        }
        
        #endregion
    }
}