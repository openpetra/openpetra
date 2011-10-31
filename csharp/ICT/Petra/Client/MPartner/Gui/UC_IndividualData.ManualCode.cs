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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private Dictionary<string, string> FLinkLabelsOrigTexts;
        
        IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        PartnerEditTDS FPartnerEditTDS;
		private SortedList<TDynamicLoadableUserControls, UserControl> FUserControlSetup;        
	    private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds FUcoSpecialNeeds;
	    private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages FUcoPersonalLanguages;
		
	    /// <summary>
	    /// Enumeration of dynamic loadable UserControls which are used
	    /// on this UserControl. 
	    /// </summary>
	    private enum TDynamicLoadableUserControls
	    {
	    	dlucPersonalData,
	    	dlucEmergencyData,
	    	dlucPassportDetails,
	    	dlucPersonalDocuments,
	        ///<summary>Denotes dynamic loadable UserControl FUcoSpecialNeeds</summary>
	        dlucSpecialNeeds,
	        dlucLocalPersonnelData,
	        dlucProfessionalAreas,
	        ///<summary>Denotes dynamic loadable UserControl FUcoPersonalLanguages</summary>
	        dlucPersonalLanguages,
	        dlucPersonalAbilities,
	        dlucPreviousExperience,
	        dlucCommitmentPeriods,
	        dlucJobAssignments,
	        dlucProgressReports
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
            
			FUserControlSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
            StoreLinkLablesOrigText();
            
			CalculateLinkLabelCounters(this);            
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
                    TUC_IndividualData_PersonalLanguages UCPersonalLanguage =
                        (TUC_IndividualData_PersonalLanguages)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalLanguages];
                    UCPersonalLanguage.GetDataFromControls2();

	                if (!FPartnerEditTDS.Tables.Contains(PmPersonLanguageTable.GetTableName()))
	                {
		                FPartnerEditTDS.Tables.Add(new PmPersonLanguageTable());
	                }
	                
	                FPartnerEditTDS.Tables[PmPersonLanguageTable.GetTableName()].Rows.Clear();
	                FPartnerEditTDS.Tables[PmPersonLanguageTable.GetTableName()].Merge(FMainDS.PmPersonLanguage);
	            }
	            
	            // TODO add code for all remaining Individual Data Items
        	}
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RefreshPersonnelDataAfterMerge()
        {
        	// Need to merge Tables from PartnerEditTDS into IndividualDataTDS so the updated s_modification_id_c of modififed Rows is held correctly in IndividualDataTDS, too!
        	FMainDS.Merge(FPartnerEditTDS);
        	
        	// Call AcceptChanges on IndividualDataTDS so that we don't have any changed data anymore (this is done to PartnerEditTDS, too, after this Method returns)!
        	FMainDS.AcceptChanges();
        }
        
        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded.
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
	                Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages ucoPersonalLanguages = new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages();
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
               
	    private void StoreLinkLablesOrigText()
	    {
	    	FLinkLabelsOrigTexts = new Dictionary<string, string>();
	    	
	    	foreach (Control Cntrl in this.pnlIndivDataMenu.Controls[0].Controls)
	    	{
	    		if ((Cntrl is LinkLabel)
	    		     && (Cntrl.Text.Contains("{")))
	    		{
	    			FLinkLabelsOrigTexts.Add(Cntrl.Name, Cntrl.Text);
	    		}	
	    	}
	    }
	    
        #endregion
        
        #region Event Handlers
        
        private void IndividualDataItemSelected(object Sender, EventArgs e)
        {
	        /*
	         * Raise the following Event to inform the base Form that we might be loading some fresh data.
	         * We need to bypass the ChangeDetection routine while this happens.
	         */
	        OnDataLoadingStarted(this, new EventArgs());
	        
	        
            if (Sender == llbOverview)
            {
            	ucoSummaryData.Parent.BringToFront();
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
					

	                
	                if (FUcoPersonalLanguages != null) 
	                {
	                	FUcoPersonalLanguages.SendToBack();	
	                }
	                
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
	            
	            FUcoSpecialNeeds.Parent.BringToFront();	            
            }
            else if (Sender == llbLanguages)
            {
	            if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalLanguages))
	            {
	                if (TClientSettings.DelayedDataLoading)
	                {
	                    // Signalise the user that data is beeing loaded
	                    this.Cursor = Cursors.AppStarting;
	                }
	
	                FUcoPersonalLanguages = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages)DynamicLoadUserControl(TDynamicLoadableUserControls.dlucPersonalLanguages);
	                
                    // Hook up RecalculateScreenParts Event
                    FUcoPersonalLanguages.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);
	                	                
	                FUcoPersonalLanguages.MainDS = FMainDS;
	                FUcoPersonalLanguages.PetraUtilsObject = FPetraUtilsObject;
	                FUcoPersonalLanguages.PartnerEditUIConnector = FPartnerEditUIConnector;
	                FUcoPersonalLanguages.SpecialInitUserControl(FMainDS);
	                FUcoPersonalLanguages.InitUserControl();	                
	                ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonalLanguages);
					                
	                if (FUcoSpecialNeeds != null) 
	                {	                
	                	FUcoSpecialNeeds.SendToBack();
	                }
	                
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
	            
	            FUcoPersonalLanguages.Parent.BringToFront();
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

        private void RecalculateLinkLabelCounters(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            if (e.ScreenPart == TScreenPartEnum.spCounters)
            {
                CalculateLinkLabelCounters(sender);
            }
        }

        private void CalculateLinkLabelCounters(System.Object ASender)
        {
        	string OrigLabelText;
        	
        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_PassportDetails)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbPassportDetails.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPassportDetails))
	                {
	                    llbPassportDetails.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPassportDetails, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbPassportDetails.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPassportDetails);
	                }            		
            	}
            }
        	
        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_PersonalDocuments)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbPersonalDocuments.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalDocuments))
	                {
	                    llbPersonalDocuments.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmDocument, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbPersonalDocuments.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPersonalDocuments);
	                }            		
            	}
            }
        	
        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_ProfessionalAreas)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbProfessionalAreas.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucProfessionalAreas))
	                {
	                    llbProfessionalAreas.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPersonQualification, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbProfessionalAreas.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountProfessionalAreas);
	                }            		
            	}
            }        	        	
        	
            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_PersonalLanguages))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbLanguages.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalLanguages))
	                {
	                    llbLanguages.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPersonLanguage, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbLanguages.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPersonalLanguages);
	                }            		
            	}
            }
        	
        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_PersonalAbilities)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbPersonalAbilities.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalAbilities))
	                {
	                    llbPersonalAbilities.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPersonAbility, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbPersonalAbilities.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPersonalAbilities);
	                }            		
            	}
            }        	        	
        	
        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_PreviousExperience)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbPreviousExperience.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPreviousExperience))
	                {
	                    llbPreviousExperience.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPastExperience, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbPreviousExperience.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPreviousExperience);
	                }            		
            	}
            }        	        	

        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_CommitmentPeriods)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbCommitmentPeriods.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucCommitmentPeriods))
	                {
	                    llbCommitmentPeriods.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmStaffData, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbCommitmentPeriods.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountCommitmentPeriods);
	                }            		
            	}
            }        	        	

        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_JobAssignments)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbJobAssignments.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucJobAssignments))
	                {
	                    llbJobAssignments.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmJobAssignment, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbJobAssignments.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountJobAssignments);
	                }            		
            	}
            }        	        	

        	if ((ASender is TUC_IndividualData)) // TODO: || (ASender is TUC_IndividualData_ProgressReports)))
            {
            	if (FLinkLabelsOrigTexts.TryGetValue(llbProgressReports.Name, out OrigLabelText)) 
            	{
	                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucProgressReports))
	                {
	                    llbProgressReports.Text = String.Format(OrigLabelText, 
	                        new DataView(FMainDS.PmPersonEvaluation, "", "", DataViewRowState.CurrentRows).Count);
	                }
	                else
	                {
	                	llbProgressReports.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountProgressReports);
	                }            		
            	}
            }        	        	
        }
        
        #endregion
    }
}
