//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerEdit_PersonnelTabSet
    {
        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TPartnerEditTabPageEnum FInitiallySelectedTabPage = TPartnerEditTabPageEnum.petpDefault;

        private TPartnerEditTabPageEnum FCurrentlySelectedTabPage;

        private Boolean FUserControlInitialised;

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

        #region Public Events

//        /// <summary>todoComment</summary>
//        public event THookupDataChangeEventHandler HookupDataChange;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupPartnerEditDataChange;

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
            OnDataLoadingStarted();

            FUserControlInitialised = true;

            tabPersonnel.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            SelectTabPage(FInitiallySelectedTabPage);

            CalculateTabHeaderCounters(this);

            OnDataLoadingFinished();
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=null).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = true;

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucIndividualData))
            {
                TUC_IndividualData UCIndividualData =
                    (TUC_IndividualData)FTabSetup[TDynamicLoadableUserControls.dlucIndividualData];

                if (!UCIndividualData.ValidateAllData(AProcessAnyDataValidationErrors, AValidateSpecificControl))
                {
                    ReturnValue = false;
                }
            }

            if (FTabSetup.ContainsKey(TDynamicLoadableUserControls.dlucApplications))
            {
                TUC_ApplicationData UCApplicationData =
                    (TUC_ApplicationData)FTabSetup[TDynamicLoadableUserControls.dlucApplications];

                if (!UCApplicationData.ValidateAllData(AProcessAnyDataValidationErrors, AValidateSpecificControl))
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
            if (FUcoIndividualData != null)
            {
                FUcoIndividualData.GetDataFromControls();
            }

            if (FUcoApplications != null)
            {
                FUcoApplications.GetDataFromControls();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshPersonnelDataAfterMerge(bool AAddressesOrRelationsChanged)
        {
            if (FUcoIndividualData != null)
            {
                FUcoIndividualData.RefreshPersonnelDataAfterMerge(AAddressesOrRelationsChanged);
            }

            if (FUcoApplications != null)
            {
                FUcoApplications.RefreshPersonnelDataAfterMerge(AAddressesOrRelationsChanged);
            }
        }

        #endregion

        #region Private Methods

        partial void PreInitUserControl(UserControl AUserControl)
        {
            if (AUserControl == FUcoIndividualData)
            {
                FUcoIndividualData.PartnerEditUIConnector = FPartnerEditUIConnector;
                FUcoIndividualData.InitialiseUserControl();
                CalculateTabHeaderCounters(this);
            }
            else if (AUserControl == FUcoApplications)
            {
                FUcoApplications.PartnerEditUIConnector = FPartnerEditUIConnector;
                FUcoApplications.InitialiseUserControl();
                CalculateTabHeaderCounters(FUcoApplications);
            }
        }

        private void TabPageEventHandler(object sender, TTabPageEventArgs ATabPageEventArgs)
        {
            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgIndividualData)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPersonnelIndividualData;

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgApplications)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPersonnelApplications;

                    // Hook up RecalculateScreenParts Event
                    FUcoApplications.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

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
            if ((ASender is TUC_PartnerEdit_PersonnelTabSet) || (ASender is TUC_Applications) || (ASender is TUC_ApplicationData))
            {
                if (FUcoApplications != null)
                {
                    tpgApplications.Text = Catalog.GetString("Applications") + " (" + FUcoApplications.CountApplications().ToString() + ")";
                }
                else if (FUcoIndividualData != null)
                {
                    // This is really only needed if application tab has not been activated yet. The number
                    // of applications still needs to be displayed and will already be available in Individual Data Tab.
                    tpgApplications.Text = Catalog.GetString("Applications") + " (" + FUcoIndividualData.CountApplications().ToString() + ")";
                }
                else
                {
                    tpgApplications.Text = Catalog.GetString("Applications");
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
                if (FUcoIndividualData != null)
                {
                    FUcoIndividualData.AdjustAfterResizing();
                }

                if (FUcoApplications != null)
                {
                    FUcoApplications.AdjustAfterResizing();
                }
            }
        }

//        private void Uco_HookupDataChange(System.Object sender, System.EventArgs e)
//        {
//            if (HookupDataChange != null)
//            {
//                HookupDataChange(this, e);
//            }
//        }

        private void Uco_HookupPartnerEditDataChange(System.Object sender, THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupPartnerEditDataChange != null)
            {
                HookupPartnerEditDataChange(this, e);
            }
        }

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(false))
            {
                //TODO WB: temporary lines as false may be returned from validation wrongly
                if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
                {
                    return;
                }

                Boolean ReturnValue = true;
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType(), null, true);

                if (ReturnValue)
                {
                    // Remove a possibly shown Validation ToolTip as the data validation succeeded
                    FPetraUtilsObject.ValidationToolTip.RemoveAll();
                }

                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
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
            SelectedTabPageBeforeReSelecting = tabPersonnel.SelectedTab;

            try
            {
                switch (ATabPage)
                {
                    case TPartnerEditTabPageEnum.petpPersonnelIndividualData:
                        tabPersonnel.SelectedTab = tpgIndividualData;
                        break;

                    case TPartnerEditTabPageEnum.petpPersonnelApplications:
                        tabPersonnel.SelectedTab = tpgApplications;
                        break;
                }
            }
            catch (Ict.Common.Controls.TSelectedIndexChangeDisallowedTabPagedIsDisabledException)
            {
                // Desired Tab Page isn't selectable because it is disabled; ignoring this Exception to ignore the selection.
            }

            // Check if the selected TabPage actually changed...
            if (SelectedTabPageBeforeReSelecting == tabPersonnel.SelectedTab)
            {
                // Tab was already selected; therefore raise the SelectedIndexChanged Event 'manually', which did not get raised by selecting the Tab again!
                TabSelectionChanged(this, new System.EventArgs());
            }

            OnDataLoadingFinished();
        }

        #endregion
    }
}