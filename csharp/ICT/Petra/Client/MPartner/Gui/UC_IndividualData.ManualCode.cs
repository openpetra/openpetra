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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        
        IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        PartnerEditTDS FPartnerEditTDS;
		private SortedList<TDynamicLoadableUserControls, UserControl> FUserControlSetup;        
	    private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds FUcoSpecialNeeds;
		
	    /// <summary>
	    /// Enumeration of dynamic loadable UserControls which are used
	    /// on the Tabs of a TabControl. 
	    /// TODO: AUTO-GENERATE at some point!!!
	    /// </summary>
	    private enum TDynamicLoadableUserControls
	    {
	        ///<summary>Denotes dynamic loadable UserControl FUcoSpecialNeeds</summary>
	        dlucSpecialNeeds,
	        ///<summary>Denotes dynamic loadable UserControl FUcoPersonalLanguages</summary>
	        dlucPersonalLanguages,
	    }
        
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
        
        /// dataset for the whole screen
        public PartnerEditTDS MainDS
        {
            get
            {
                return FPartnerEditTDS;
            }
            
            set
            {
                FPartnerEditTDS = value;
            }
        }
        #endregion
        #region Events
        
        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;
        
        #endregion
        
        #region Public Methods

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            FMainDS = new IndividualDataTDS();
            
            // Merge DataTables which are held only in PartnerEditTDS into IndividualDataTDS so that we can access that data in here!
            FMainDS.Merge(FPartnerEditTDS);

            ucoSummaryData.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoSummaryData.SpecialInitUserControl(FMainDS);
        }
        
        /// <summary>
        /// Gets the data from all controls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls()
        {
        	if (FUserControlSetup != null) 
        	{
		        if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucSpecialNeeds))
	            {
	                TUC_IndividualData_SpecialNeeds UCSpecialNeeds =
	                    (TUC_IndividualData_SpecialNeeds)FUserControlSetup[TDynamicLoadableUserControls.dlucSpecialNeeds];
	                UCSpecialNeeds.GetDataFromControls2();
	                
	                if (!FPartnerEditTDS.Tables.Contains(PmSpecialNeedTable.GetTableName()))
	                {
		                FPartnerEditTDS.Tables.Add(new PmSpecialNeedTable());
	                }
	                
	                FPartnerEditTDS.Tables[PmSpecialNeedTable.GetTableName()].Merge(FMainDS.PmSpecialNeed);
	            }
	
	            if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalLanguages))
	            {
//                        TUC_PartnerDetails_Family UCPartnerDetailsFamily =
//                            (TUC_PartnerDetails_Family)FTabSetup[TDynamicLoadableUserControls.dlucPersonalLanguages];
//                        UCPartnerDetailsFamily.GetDataFromControls2();
	            }	            
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
        
	    /// <summary>
	    /// Creates UserControls on request. 
	    /// TODO: AUTO-GENERATE at some point!!!
	    /// </summary>
	    /// <param name="AUserControl">UserControl to load.</param>
	    private UserControl DynamicLoadUserControl(TDynamicLoadableUserControls AUserControl)
	    {
	        UserControl ReturnValue = null;
	
	        switch (AUserControl)
	        {
	            case TDynamicLoadableUserControls.dlucSpecialNeeds:
	                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
	                Panel pnlHostForUCSpecialNeeds = new Panel();
	                pnlHostForUCSpecialNeeds.AutoSize = true;
	                pnlHostForUCSpecialNeeds.Dock = System.Windows.Forms.DockStyle.Fill;
	                pnlHostForUCSpecialNeeds.Location = new System.Drawing.Point(0, 0);
	                pnlHostForUCSpecialNeeds.Padding = new System.Windows.Forms.Padding(2);
	                pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCSpecialNeeds);
	
	                // Create the UserControl
	                Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds ucoSpecialNeeds = new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds();
	                FUserControlSetup.Add(TDynamicLoadableUserControls.dlucSpecialNeeds, ucoSpecialNeeds);
	                ucoSpecialNeeds.Location = new Point(0, 2);
	                ucoSpecialNeeds.Dock = DockStyle.Fill;
	                pnlHostForUCSpecialNeeds.Controls.Add(ucoSpecialNeeds);
	
	                /*
	                 * The following four commands seem strange and unnecessary; however, they are necessary
	                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
	                 */
	                if (TClientSettings.GUIRunningOnNonStandardDPI)
	                {
	                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
	                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	                    pnlHostForUCSpecialNeeds.Dock = System.Windows.Forms.DockStyle.None;
	                    pnlHostForUCSpecialNeeds.Dock = System.Windows.Forms.DockStyle.Fill;
	                }
	
	                ReturnValue = ucoSpecialNeeds;
	                break;
	                
	            case TDynamicLoadableUserControls.dlucPersonalLanguages:
	                // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
	                Panel pnlHostForUCPersonalLanguages = new Panel();
	                pnlHostForUCPersonalLanguages.AutoSize = true;
	                pnlHostForUCPersonalLanguages.Dock = System.Windows.Forms.DockStyle.Fill;
	                pnlHostForUCPersonalLanguages.Location = new System.Drawing.Point(0, 0);
	                pnlHostForUCPersonalLanguages.Padding = new System.Windows.Forms.Padding(2);
	                pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPersonalLanguages);
	
	                // Create the UserControl
	                Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes ucoPersonalLanguages = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerNotes();
	                FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPersonalLanguages, ucoPersonalLanguages);
	                ucoPersonalLanguages.Location = new Point(0, 2);
	                ucoPersonalLanguages.Dock = DockStyle.Fill;
	                pnlHostForUCPersonalLanguages.Controls.Add(ucoPersonalLanguages);
	
	                /*
	                 * The following four commands seem strange and unnecessary; however, they are necessary
	                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
	                 */
	                if (TClientSettings.GUIRunningOnNonStandardDPI)
	                {
	                    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
	                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	                    pnlHostForUCPersonalLanguages.Dock = System.Windows.Forms.DockStyle.None;
	                    pnlHostForUCPersonalLanguages.Dock = System.Windows.Forms.DockStyle.Fill;
	                }
	
	                ReturnValue = ucoPersonalLanguages;
	                break;
	        }
	
	        return ReturnValue;
	    }
               
        #endregion
        
        #region Event Handlers
        
        private void IndividualDataItemSelected(object Sender, EventArgs e)
        {

	        if (FUserControlSetup == null)
	        {
	            FUserControlSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
	        }
	

	        /*
	         * Raise the following Event to inform the base Form that we might be loading some fresh data.
	         * We need to bypass the ChangeDetection routine while this happens.
	         */
	        OnDataLoadingStarted(this, new EventArgs());
	        
	        
            if (Sender == llbOverview)
            {
				// TODO                
            }
            else if (Sender == llbSpecialNeeds)
            {
	            if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucSpecialNeeds))
	            {
	                if (TClientSettings.DelayedDataLoading)
	                {
	                    // Signalise the user that data is beeing loaded
	                    this.Cursor = Cursors.AppStarting;
	                }
	
	                FUcoSpecialNeeds = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucSpecialNeeds);
	                FUcoSpecialNeeds.MainDS = FMainDS;
	                FUcoSpecialNeeds.PetraUtilsObject = FPetraUtilsObject;
	                FUcoSpecialNeeds.PartnerEditUIConnector = FPartnerEditUIConnector;
	                FUcoSpecialNeeds.SpecialInitUserControl(FMainDS);
	                FUcoSpecialNeeds.InitUserControl();	                
	                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoSpecialNeeds);
					
	                FUcoSpecialNeeds.Parent.BringToFront();
//	TODO                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));
	
	                this.Cursor = Cursors.Default;
	            }
	            else
	            {
//	TODO                OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));
	
	                /*
	                 * The following command seems strange and unnecessary; however, it is necessary
	                 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
	                 */
	                if (TClientSettings.GUIRunningOnNonStandardDPI)
	                {
	                    FUcoSpecialNeeds.AdjustAfterResizing();
	                }
	            }
            }
            else if (Sender == llbLanguages)
            {
                // TODO
            }
            
            // TODO else branch for all remaining Individual Data Items
            

	        /*
	         * Raise the following Event to inform the base Form that we have finished loading fresh data.
	         * We need to turn the ChangeDetection routine back on.
	         */
	        OnDataLoadingFinished(this, new EventArgs());            
        }
        

        private void OpenBasicDataShepherd(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void OpenIntranetRegistrationShepherd(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowEmergencyContacts(object Sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
