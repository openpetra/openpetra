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
using Ict.Common.Remoting.Client;
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

        private Dictionary <string, string>FLinkLabelsOrigTexts;
        private IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        private PartnerEditTDS FPartnerEditTDS;
        private SortedList <TDynamicLoadableUserControls, UserControl>FUserControlSetup;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds FUcoSpecialNeeds;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages FUcoPersonalLanguages;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PreviousExperience FUcoPreviousExperience;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalDocuments FUcoPersonalDocuments;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_JobAssignments FUcoJobAssignments;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_LocalPersonnelData FUcoLocalPersonnelData;

        /// <summary>
        /// Enumeration of dynamic loadable UserControls which are used
        /// on this UserControl.
        /// </summary>
        private enum TDynamicLoadableUserControls
        {
            dlucPersonalData,
            dlucEmergencyData,
            dlucPassportDetails,
            ///<summary>Denotes dynamic loadable UserControl FUcoPersonalDocuments</summary>
            dlucPersonalDocuments,
            ///<summary>Denotes dynamic loadable UserControl FUcoSpecialNeeds</summary>
            dlucSpecialNeeds,
            ///<summary>Denotes dynamic loadable UserControl FUcoLocalPersonnelData</summary>
            dlucLocalPersonnelData,
            dlucProfessionalAreas,
            ///<summary>Denotes dynamic loadable UserControl FUcoPersonalLanguages</summary>
            dlucPersonalLanguages,
            dlucPersonalAbilities,
            ///<summary>Denotes dynamic loadable UserControl FUcoPreviousExperience</summary>
            dlucPreviousExperience,
            dlucCommitmentPeriods,
            ///<summary>Denotes dynamic loadable UserControl FUcoJobAssignments</summary>
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

            ucoSummaryData.PetraUtilsObject = FPetraUtilsObject;
            ucoSummaryData.MainDS = FMainDS;
            ucoSummaryData.PartnerEditUIConnector = FPartnerEditUIConnector;
            ucoSummaryData.SpecialInitUserControl(FMainDS);

            // Hook up ColumnChanging Event of the FPartnerEditTDS's PPerson Table
            FPartnerEditTDS.PPerson.ColumnChanged += delegate {
                ucoSummaryData.FMainDS_PPerson_ColumnChanged(FPartnerEditTDS.PPerson[0]);
            };

            // Store the text that the LinkLabels show originally (used for repeated updating the numbers in the strings)
            FUserControlSetup = new SortedList <TDynamicLoadableUserControls, UserControl>();
            StoreLinkLablesOrigText();

            // Initialise the numbers in the strings of the LinkLabels
            CalculateLinkLabelCounters(this);
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
            if (FUserControlSetup != null)
            {
                // Special Needs
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

                // Personal Languages
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
				
				// Previous Experience
				// dluc corresponds to enum above, UCPreviousExperience to UC_IndividualData_PreviousExperience.ManualCode,
				// 		PmPastExperienceTable to the DB table name
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPreviousExperience))
                {
                    TUC_IndividualData_PreviousExperience UCPreviousExperience =
                        (TUC_IndividualData_PreviousExperience)FUserControlSetup[TDynamicLoadableUserControls.dlucPreviousExperience];
                    UCPreviousExperience.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPastExperienceTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPastExperienceTable());
                    }

                    FPartnerEditTDS.Tables[PmPastExperienceTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPastExperienceTable.GetTableName()].Merge(FMainDS.PmPastExperience);
                }
                
                // Personal Documents
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalDocuments))
                {
                    TUC_IndividualData_PersonalDocuments UCPersonalDocuments =
                        (TUC_IndividualData_PersonalDocuments)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalDocuments];
                    UCPersonalDocuments.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmDocumentTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmDocumentTable());
                    }

                    FPartnerEditTDS.Tables[PmDocumentTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmDocumentTable.GetTableName()].Merge(FMainDS.PmDocument);
                }
                
                // Job Assignments
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucJobAssignments))
                {
                    TUC_IndividualData_JobAssignments UCJobAssignments =
                        (TUC_IndividualData_JobAssignments)FUserControlSetup[TDynamicLoadableUserControls.dlucJobAssignments];
                    UCJobAssignments.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmJobAssignmentTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmJobAssignmentTable());
                    }

                    FPartnerEditTDS.Tables[PmJobAssignmentTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmJobAssignmentTable.GetTableName()].Merge(FMainDS.PmJobAssignment);
                }
                
                // Local Personnel Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucLocalPersonnelData))
                {
                    TUC_IndividualData_LocalPersonnelData UCLocalPersonnelData =
                        (TUC_IndividualData_LocalPersonnelData)FUserControlSetup[TDynamicLoadableUserControls.dlucLocalPersonnelData];
                    UCLocalPersonnelData.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PDataLabelValuePartnerTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PDataLabelValuePartnerTable());
                    }

                    FPartnerEditTDS.Tables[PDataLabelValuePartnerTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PDataLabelValuePartnerTable.GetTableName()].Merge(FMainDS.PDataLabelValuePartner);
                }

                // TODO add code for all remaining Individual Data Items
            }
        }

        /// <summary>
        /// Called when data got saved in the screen. This Method takes over changed data
        /// into FMainDS, which is different than the Partner Edit screen's FMainDS, in
        /// order to have current data on which decisions on whether to refresh certain
        /// parts of the 'Overview' need to be updated.
        /// </summary>
        /// <param name="AAddressesOrRelationsChanged">Set to true by the SaveChanges Method
        /// of the Partner Edit screen if Addresses or Relationships have changed.</param>
        /// <returns>void</returns>
        public void RefreshPersonnelDataAfterMerge(bool AAddressesOrRelationsChanged)
        {
            //
            // Need to merge Tables from PartnerEditTDS into IndividualDataTDS so the updated s_modification_id_c of modififed Rows is held correctly in IndividualDataTDS, too!
            //

            // ...but first empty relevant DataTables to ensure that DataRows that got deleted in FPartnerEditTDS are reflected in FMainDS (just performing a Merge wouldn't remove them!)
            if (FMainDS.Tables.Contains(PPartnerLocationTable.GetTableName()))
            {
                FMainDS.Tables[PPartnerLocationTable.GetTableName()].Rows.Clear();
            }

            if (FMainDS.Tables.Contains(PLocationTable.GetTableName()))
            {
                FMainDS.Tables[PLocationTable.GetTableName()].Rows.Clear();
            }

            if (FMainDS.Tables.Contains(PPartnerRelationshipTable.GetTableName()))
            {
                FMainDS.Tables[PPartnerRelationshipTable.GetTableName()].Rows.Clear();
            }

            // Now perform the Merge operation
            FMainDS.Merge(FPartnerEditTDS);

            // Call AcceptChanges on IndividualDataTDS so that we don't have any changed data anymore (this is done to PartnerEditTDS, too, after this Method returns)!
            FMainDS.AcceptChanges();

            // Let the 'Overview' UserControl determine whether it needs to refresh the data it displays.
            if (AAddressesOrRelationsChanged)
            {
                ucoSummaryData.CheckForRefreshOfDisplayedData();
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        /// <returns>void</returns>
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
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds ucoSpecialNeeds =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds();
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
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages ucoPersonalLanguages =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages();
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
                    
                case TDynamicLoadableUserControls.dlucPreviousExperience:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPreviousExperience = new Panel();
                    pnlHostForUCPreviousExperience.AutoSize = true;
                    pnlHostForUCPreviousExperience.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPreviousExperience.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPreviousExperience.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPreviousExperience);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PreviousExperience ucoPreviousExperience =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PreviousExperience();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPreviousExperience, ucoPreviousExperience);
                    ucoPreviousExperience.Location = new Point(0, 2);
                    ucoPreviousExperience.Dock = DockStyle.Fill;
                    pnlHostForUCPreviousExperience.Controls.Add(ucoPreviousExperience);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPreviousExperience.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPreviousExperience.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPreviousExperience;
                    break;
                    
                case TDynamicLoadableUserControls.dlucPersonalDocuments:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPersonalDocuments = new Panel();
                    pnlHostForUCPersonalDocuments.AutoSize = true;
                    pnlHostForUCPersonalDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPersonalDocuments.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPersonalDocuments.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPersonalDocuments);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalDocuments ucoPersonalDocuments =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalDocuments();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPersonalDocuments, ucoPersonalDocuments);
                    ucoPersonalDocuments.Location = new Point(0, 2);
                    ucoPersonalDocuments.Dock = DockStyle.Fill;
                    pnlHostForUCPersonalDocuments.Controls.Add(ucoPersonalDocuments);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPersonalDocuments.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPersonalDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPersonalDocuments;
                    break;
                    
                case TDynamicLoadableUserControls.dlucJobAssignments:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCJobAssignments = new Panel();
                    pnlHostForUCJobAssignments.AutoSize = true;
                    pnlHostForUCJobAssignments.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCJobAssignments.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCJobAssignments.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCJobAssignments);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_JobAssignments ucoJobAssignments =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_JobAssignments();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucJobAssignments, ucoJobAssignments);
                    ucoJobAssignments.Location = new Point(0, 2);
                    ucoJobAssignments.Dock = DockStyle.Fill;
                    pnlHostForUCJobAssignments.Controls.Add(ucoJobAssignments);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCJobAssignments.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCJobAssignments.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoJobAssignments;
                    break;
                    
                case TDynamicLoadableUserControls.dlucLocalPersonnelData:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCLocalPersonnelData = new Panel();
                    pnlHostForUCLocalPersonnelData.AutoSize = true;
                    pnlHostForUCLocalPersonnelData.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCLocalPersonnelData.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCLocalPersonnelData.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCLocalPersonnelData);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_LocalPersonnelData ucoLocalPersonnelData =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_LocalPersonnelData();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucLocalPersonnelData, ucoLocalPersonnelData);
                    ucoLocalPersonnelData.Location = new Point(0, 2);
                    ucoLocalPersonnelData.Dock = DockStyle.Fill;
                    pnlHostForUCLocalPersonnelData.Controls.Add(ucoLocalPersonnelData);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCLocalPersonnelData.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCLocalPersonnelData.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoLocalPersonnelData;
                    break;

                    // TODO Add case code blocks for all remaining Individual Data Items
            }

            return ReturnValue;
        }

        /// <summary>
        /// Stores the text that the LinkLabels show originally (used for repeated updating the numbers in the strings).
        /// </summary>
        /// <returns>void</returns>
        private void StoreLinkLablesOrigText()
        {
            FLinkLabelsOrigTexts = new Dictionary <string, string>();

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

        /// <summary>
        /// Event is fired when an Individual Data Item LinkLabel is 'clicked'.
        /// </summary>
        /// <param name="ASender">One of the Individual Data Item LinkLabels. Determines what action is taken.</param>
        /// <param name="e">Not evaluated.</param>
        /// <returns>void</returns>
        private void IndividualDataItemSelected(object ASender, EventArgs e)
        {
            /*
             * Raise the following Event to inform the base Form that we might be loading some fresh data.
             * We need to bypass the ChangeDetection routine while this happens.
             */
            OnDataLoadingStarted(this, new EventArgs());

            if (ASender == llbOverview)
            {
                ucoSummaryData.Parent.BringToFront();
            }
            else if (ASender == llbSpecialNeeds)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucSpecialNeeds))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoSpecialNeeds = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_SpecialNeeds)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucSpecialNeeds);
                    FUcoSpecialNeeds.MainDS = FMainDS;
                    FUcoSpecialNeeds.PetraUtilsObject = FPetraUtilsObject;
                    FUcoSpecialNeeds.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoSpecialNeeds.SpecialInitUserControl(FMainDS);
                    FUcoSpecialNeeds.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoSpecialNeeds);

                    SendAllOtherItemsToBackExcluding("FUcoSpecialNeeds");

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
            else if (ASender == llbLanguages)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalLanguages))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPersonalLanguages = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalLanguages)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPersonalLanguages);

                    // Hook up RecalculateScreenParts Event
                    FUcoPersonalLanguages.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPersonalLanguages.MainDS = FMainDS;
                    FUcoPersonalLanguages.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPersonalLanguages.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPersonalLanguages.SpecialInitUserControl(FMainDS);
                    FUcoPersonalLanguages.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonalLanguages);

                    SendAllOtherItemsToBackExcluding("FUcoPersonalLanguages");

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
                        FUcoPersonalLanguages.AdjustAfterResizing();
                    }
                }

                FUcoPersonalLanguages.Parent.BringToFront();
            }
            else if (ASender == llbPreviousExperience)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPreviousExperience))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPreviousExperience = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PreviousExperience)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPreviousExperience);

                    // Hook up RecalculateScreenParts Event
                    FUcoPreviousExperience.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPreviousExperience.MainDS = FMainDS;
                    FUcoPreviousExperience.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPreviousExperience.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPreviousExperience.SpecialInitUserControl(FMainDS);
                    FUcoPreviousExperience.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPreviousExperience);

                    SendAllOtherItemsToBackExcluding("FUcoPreviousExperience");

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
                        FUcoPreviousExperience.AdjustAfterResizing();
                    }
                }

                FUcoPreviousExperience.Parent.BringToFront();
            }
            
            else if (ASender == llbPersonalDocuments)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalDocuments))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPersonalDocuments = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalDocuments)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPersonalDocuments);

                    // Hook up RecalculateScreenParts Event
                    FUcoPersonalDocuments.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPersonalDocuments.MainDS = FMainDS;
                    FUcoPersonalDocuments.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPersonalDocuments.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPersonalDocuments.SpecialInitUserControl(FMainDS);
                    FUcoPersonalDocuments.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonalDocuments);

                    SendAllOtherItemsToBackExcluding("FUcoPersonalDocuments");

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
                        FUcoPersonalDocuments.AdjustAfterResizing();
                    }
                }

                FUcoPersonalDocuments.Parent.BringToFront();
            }
            
            else if (ASender == llbJobAssignments)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucJobAssignments))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoJobAssignments = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_JobAssignments)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucJobAssignments);

                    // Hook up RecalculateScreenParts Event
                    FUcoJobAssignments.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoJobAssignments.MainDS = FMainDS;
                    FUcoJobAssignments.PetraUtilsObject = FPetraUtilsObject;
                    FUcoJobAssignments.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoJobAssignments.SpecialInitUserControl(FMainDS);
                    FUcoJobAssignments.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoJobAssignments);

                    SendAllOtherItemsToBackExcluding("FUcoJobAssignments");

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
                        FUcoJobAssignments.AdjustAfterResizing();
                    }
                }

                FUcoJobAssignments.Parent.BringToFront();
            }
                
            else if (ASender == llbLocalPersonnelData)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucLocalPersonnelData))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoLocalPersonnelData = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_LocalPersonnelData)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucLocalPersonnelData);

                    // Hook up RecalculateScreenParts Event
                    //FUcoLocalPersonnelData.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoLocalPersonnelData.MainDS = FMainDS;
                    FUcoLocalPersonnelData.PetraUtilsObject = FPetraUtilsObject;
                    FUcoLocalPersonnelData.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoLocalPersonnelData.InitUserControl();
                    FUcoLocalPersonnelData.SpecialInitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoLocalPersonnelData);

                    SendAllOtherItemsToBackExcluding("FUcoLocalPersonnelData");

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
                        FUcoLocalPersonnelData.AdjustAfterResizing();
                    }
                }

                FUcoLocalPersonnelData.Parent.BringToFront();
            }
            
            // TODO Add else branch for all remaining Individual Data Items


            /*
             * Raise the following Event to inform the base Form that we have finished loading fresh data.
             * We need to turn the ChangeDetection routine back on.
             */
            OnDataLoadingFinished(this, new EventArgs());
        }
        
        /// <summary>
        /// This method sends all views, excluding the current one, to back.  BE SURE TO ADD EACH NEW VIEW TYPE HERE AS THEY ARE ADDED!!
        /// </summary>
        /// <param name="exclude">Type: String;  This is the name of the individualData item that will not be sent to back.</param>
        private void SendAllOtherItemsToBackExcluding(String exclude){
            if (FUcoPersonalLanguages != null && exclude != "FUcoPersonalLanguages")
            {
                FUcoPersonalLanguages.SendToBack();
            }
            if (FUcoSpecialNeeds != null && exclude != "FUcoSpecialNeeds")
            {
                FUcoSpecialNeeds.SendToBack();
            }
            if (FUcoPreviousExperience != null && exclude != "FUcoPreviousExperience")
            {
                FUcoPreviousExperience.SendToBack();
            }
            if (FUcoPersonalDocuments != null && exclude != "FUcoPersonalDocuments")
            {
                FUcoPersonalDocuments.SendToBack();
            }
            if (FUcoJobAssignments != null && exclude != "FUcoJobAssignments")
            {
                FUcoJobAssignments.SendToBack();
            }
            /*if (FUcoLocalPersonnelData != null && exclude != "FUcoLocalPersonnelData")
            {
                FUcoLocalPersonnelData.SendToBack();
            }*/
            
            //TODO Add the other individualData items here.
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
                // Update the numbers in the strings of the LinkLabels
                CalculateLinkLabelCounters(sender);
            }
        }

        /// <summary>
        /// Updates the numbers in the strings of the LinkLabels.
        /// </summary>
        /// <param name="ASender">UserControl that corresponds to one of the Individual Data Item LinkLabels.</param>
        /// <returns>void</returns>
        private void CalculateLinkLabelCounters(System.Object ASender)
        {
            string OrigLabelText;

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_PassportDetails)))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_PersonalDocuments))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_ProfessionalAreas)))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_PersonalAbilities)))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_PreviousExperience))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_CommitmentPeriods)))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_JobAssignments))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_ProgressReports)))
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