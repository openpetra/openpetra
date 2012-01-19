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
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_ProgressReports FUcoProgressReports;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_CommitmentPeriods FUcoCommitmentPeriods;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonSkills FUcoPersonSkills;
        
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
            dlucProgressReports,
            dlucPersonSkills,
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
                
                // Progress Reports (Person Evaluations)
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucProgressReports))
                {
                    TUC_IndividualData_ProgressReports UCProgressReport =
                        (TUC_IndividualData_ProgressReports)FUserControlSetup[TDynamicLoadableUserControls.dlucProgressReports];
                    UCProgressReport.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPersonEvaluationTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPersonEvaluationTable());
                    }

                    FPartnerEditTDS.Tables[PmPersonEvaluationTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPersonEvaluationTable.GetTableName()].Merge(FMainDS.PmPersonEvaluation);
                }
                
                // Commitment Periods
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucCommitmentPeriods))
                {
                    TUC_IndividualData_CommitmentPeriods UCCommitmentPeriod =
                        (TUC_IndividualData_CommitmentPeriods)FUserControlSetup[TDynamicLoadableUserControls.dlucCommitmentPeriods];
                    UCCommitmentPeriod.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmStaffDataTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmStaffDataTable());
                    }

                    FPartnerEditTDS.Tables[PmStaffDataTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmStaffDataTable.GetTableName()].Merge(FMainDS.PmStaffData);
                }
                
                // Person Skills
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonSkills))
                {
                    TUC_IndividualData_PersonSkills UCPersonSkill =
                        (TUC_IndividualData_PersonSkills)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonSkills];
                    UCPersonSkill.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPersonSkillTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPersonSkillTable());
                    }

                    FPartnerEditTDS.Tables[PmPersonSkillTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPersonSkillTable.GetTableName()].Merge(FMainDS.PmPersonSkill);
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

                case TDynamicLoadableUserControls.dlucProgressReports:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCProgressReports = new Panel();
                    pnlHostForUCProgressReports.AutoSize = true;
                    pnlHostForUCProgressReports.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCProgressReports.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCProgressReports.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCProgressReports);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_ProgressReports ucoProgressReports =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_ProgressReports();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucProgressReports, ucoProgressReports);
                    ucoProgressReports.Location = new Point(0, 2);
                    ucoProgressReports.Dock = DockStyle.Fill;
                    pnlHostForUCProgressReports.Controls.Add(ucoProgressReports);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCProgressReports.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCProgressReports.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoProgressReports;
                    break;
                    
                case TDynamicLoadableUserControls.dlucCommitmentPeriods:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCCommitmentPeriods = new Panel();
                    pnlHostForUCCommitmentPeriods.AutoSize = true;
                    pnlHostForUCCommitmentPeriods.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCCommitmentPeriods.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCCommitmentPeriods.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCCommitmentPeriods);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_CommitmentPeriods ucoCommitmentPeriods =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_CommitmentPeriods();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucCommitmentPeriods, ucoCommitmentPeriods);
                    ucoCommitmentPeriods.Location = new Point(0, 2);
                    ucoCommitmentPeriods.Dock = DockStyle.Fill;
                    pnlHostForUCCommitmentPeriods.Controls.Add(ucoCommitmentPeriods);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCCommitmentPeriods.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCCommitmentPeriods.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoCommitmentPeriods;
                    break;
                    

                case TDynamicLoadableUserControls.dlucPersonSkills:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPersonSkills = new Panel();
                    pnlHostForUCPersonSkills.AutoSize = true;
                    pnlHostForUCPersonSkills.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPersonSkills.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPersonSkills.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPersonSkills);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonSkills ucoPersonSkills =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonSkills();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPersonSkills, ucoPersonSkills);
                    ucoPersonSkills.Location = new Point(0, 2);
                    ucoPersonSkills.Dock = DockStyle.Fill;
                    pnlHostForUCPersonSkills.Controls.Add(ucoPersonSkills);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPersonSkills.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPersonSkills.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPersonSkills;
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

                    if (FUcoPersonalLanguages != null)
                    {
                        FUcoPersonalLanguages.SendToBack();
                    }
                    if (FUcoProgressReports != null)
                    {
                        FUcoProgressReports.SendToBack();
                    }
                    if (FUcoCommitmentPeriods != null)
                    {
                        FUcoCommitmentPeriods.SendToBack();
                    }
                    if (FUcoPersonSkills != null)
                    {
                        FUcoPersonSkills.SendToBack();
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

                    if (FUcoSpecialNeeds != null)
                    {
                        FUcoSpecialNeeds.SendToBack();
                    }
                    if (FUcoProgressReports != null)
                    {
                        FUcoProgressReports.SendToBack();
                    }
                    if (FUcoCommitmentPeriods != null)
                    {
                        FUcoCommitmentPeriods.SendToBack();
                    }
                    if (FUcoPersonSkills != null)
                    {
                        FUcoPersonSkills.SendToBack();
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
            else if (ASender == llbProgressReports)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucProgressReports))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoProgressReports = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_ProgressReports)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucProgressReports);

                    // Hook up RecalculateScreenParts Event
                    FUcoProgressReports.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoProgressReports.MainDS = FMainDS;
                    FUcoProgressReports.PetraUtilsObject = FPetraUtilsObject;
                    FUcoProgressReports.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoProgressReports.SpecialInitUserControl(FMainDS);
                    FUcoProgressReports.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoProgressReports);

                    if (FUcoSpecialNeeds != null)
                    {
                        FUcoSpecialNeeds.SendToBack();
                    }
                    if (FUcoPersonalLanguages != null)
                    {
                        FUcoPersonalLanguages.SendToBack();
                    }
                    if (FUcoCommitmentPeriods != null)
                    {
                        FUcoCommitmentPeriods.SendToBack();
                    }
                    if (FUcoPersonSkills != null)
                    {
                        FUcoPersonSkills.SendToBack();
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

                FUcoProgressReports.Parent.BringToFront();
            }
            else if (ASender == llbCommitmentPeriods)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucCommitmentPeriods))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoCommitmentPeriods = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_CommitmentPeriods)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucCommitmentPeriods);

                    // Hook up RecalculateScreenParts Event
                    FUcoCommitmentPeriods.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoCommitmentPeriods.MainDS = FMainDS;
                    FUcoCommitmentPeriods.PetraUtilsObject = FPetraUtilsObject;
                    FUcoCommitmentPeriods.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoCommitmentPeriods.SpecialInitUserControl(FMainDS);
                    FUcoCommitmentPeriods.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoCommitmentPeriods);

                    if (FUcoSpecialNeeds != null)
                    {
                        FUcoSpecialNeeds.SendToBack();
                    }
                    if (FUcoPersonalLanguages != null)
                    {
                        FUcoPersonalLanguages.SendToBack();
                    }
                    if (FUcoProgressReports != null)
                    {
                        FUcoProgressReports.SendToBack();
                    }
                    if (FUcoPersonSkills != null)
                    {
                        FUcoPersonSkills.SendToBack();
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

                FUcoCommitmentPeriods.Parent.BringToFront();
            }
            else if (ASender == llbPersonSkills)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonSkills))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPersonSkills = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonSkills)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPersonSkills);

                    // Hook up RecalculateScreenParts Event
                    FUcoPersonSkills.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPersonSkills.MainDS = FMainDS;
                    FUcoPersonSkills.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPersonSkills.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPersonSkills.SpecialInitUserControl(FMainDS);
                    FUcoPersonSkills.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonSkills);

                    if (FUcoSpecialNeeds != null)
                    {
                        FUcoSpecialNeeds.SendToBack();
                    }
                    if (FUcoPersonalLanguages != null)
                    {
                        FUcoPersonalLanguages.SendToBack();
                    }
                    if (FUcoProgressReports != null)
                    {
                        FUcoProgressReports.SendToBack();
                    }
                    if (FUcoCommitmentPeriods != null)
                    {
                        FUcoCommitmentPeriods.SendToBack();
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

                FUcoPersonSkills.Parent.BringToFront();
            }     

            
            // TODO Add else branch for all remaining Individual Data Items


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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_PersonalDocuments)))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_PreviousExperience)))
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

            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_JobAssignments)))
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
            
            /*
            if ((ASender is TUC_IndividualData))     // TODO: || (ASender is TUC_IndividualData_PersonSkills)))
            {
                if (FLinkLabelsOrigTexts.TryGetValue(llbPersonSkills.Name, out OrigLabelText))
                {
                    if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonSkills))
                    {
                        llbPersonSkills.Text = String.Format(OrigLabelText,
                            new DataView(FMainDS.PmPersonSkill, "", "", DataViewRowState.CurrentRows).Count);
                    }
                    else
                    {
                        llbPersonSkills.Text = String.Format(OrigLabelText, FMainDS.MiscellaneousData[0].ItemsCountPersonSkills);
                        // TODO: there is no ItemsCountPersonSkills defined... need to define it and uncomment this???
                    }
                }
            }
            */
        }

        #endregion
    }
}