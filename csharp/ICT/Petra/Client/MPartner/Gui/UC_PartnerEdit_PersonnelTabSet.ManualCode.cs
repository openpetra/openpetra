//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;

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
            
            tpgApplications.Enabled = false;    // TODO This feature isn't implemented yet.


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
        	if (FUcoIndividualData != null) 
        	{
        		FUcoIndividualData.GetDataFromControls();	
        	}
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshPersonnelDataAfterMerge()
        {
        	if (FUcoIndividualData != null) 
        	{        	
				FUcoIndividualData.RefreshPersonnelDataAfterMerge();
        	}        	
        }
        
        #endregion
        
        #region Private Methods
        
        private void TabPageEventHandler(object sender, TTabPageEventArgs ATabPageEventArgs)
        {
            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgIndividualData)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPersonnelIndividualData;

                    FUcoIndividualData.PartnerEditUIConnector = FPartnerEditUIConnector;
                    
                    FUcoIndividualData.InitialiseUserControl();

                    CorrectDataGridWidthsAfterDataChange();
                }
                else if (ATabPageEventArgs.Tab == tpgApplications)
                {
                    FCurrentlySelectedTabPage = TPartnerEditTabPageEnum.petpPersonnelApplications;

                    // Hook up RecalculateScreenParts Event
// TODO                    FUcoApplications.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateTabHeaderCounters);

// TODO                   FUcoApplications.PartnerEditUIConnector = FPartnerEditUIConnector;

// TODO                   FUcoApplications.SpecialInitUserControl();

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
            Int32 CountAll = 0;            

            if ((ASender is TUC_PartnerEdit_PersonnelTabSet) || (ASender is TUC_Applications))
            {
                tpgApplications.Text = String.Format(tpgApplications.Text, CountAll);
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
