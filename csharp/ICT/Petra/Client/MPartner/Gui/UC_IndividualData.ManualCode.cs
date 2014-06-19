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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MPartner;
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
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Abilities FUcoPersonalAbilities;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Passport FUcoPassportDetails;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalData FUcoPersonalData;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_EmergencyData FUcoEmergencyData;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PreviousExperience FUcoPreviousExperience;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalDocuments FUcoPersonalDocuments;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_JobAssignments FUcoJobAssignments;
        private Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_LocalPersonnelData FUcoLocalPersonnelData;

        private LinkLabel FCurrentLinkLabel;

        //the background color that all PanelHelpers will have
        private System.Drawing.Color PanelHelperBackGround = System.Drawing.Color.Yellow;

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

            // In regards to local personnel data items: this will automatically be loaded in
            // TUC_IndividualData_LocalPersonnelData.LoadDataOnDemand so we can empty the table here
            // to make sure we have no data items in there that are not personnel related
            if (FMainDS.PDataLabelValuePartner != null)
            {
                FMainDS.PDataLabelValuePartner.Rows.Clear();
            }

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
        /// returns number of application records existing for current person record
        /// (this is really only needed if application tab has not been activated yet)
        /// </summary>
        /// <returns>int</returns>
        public int CountApplications()
        {
            return FMainDS.MiscellaneousData[0].ItemsCountApplications;
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

            if (FUserControlSetup != null)
            {
                // Special Needs
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucSpecialNeeds))
                {
                    TUC_IndividualData_SpecialNeeds UCSpecialNeeds =
                        (TUC_IndividualData_SpecialNeeds)FUserControlSetup[TDynamicLoadableUserControls.dlucSpecialNeeds];

                    if (!UCSpecialNeeds.ValidateAllData(AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Personal Languages
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalLanguages))
                {
                    TUC_IndividualData_PersonalLanguages UCPersonalLanguage =
                        (TUC_IndividualData_PersonalLanguages)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalLanguages];

                    if (!UCPersonalLanguage.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Abilities
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalAbilities))
                {
                    TUC_IndividualData_Abilities UCAbilities =
                        (TUC_IndividualData_Abilities)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalAbilities];

                    if (!UCAbilities.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                //Passport Details
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPassportDetails))
                {
                    TUC_IndividualData_Passport UCPassport =
                        (TUC_IndividualData_Passport)FUserControlSetup[TDynamicLoadableUserControls.dlucPassportDetails];

                    if (!UCPassport.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                //Personal Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalData))
                {
                    TUC_IndividualData_PersonalData UCPersonalData =
                        (TUC_IndividualData_PersonalData)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalData];

                    if (!UCPersonalData.ValidateAllData(AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                //Emergency Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucEmergencyData))
                {
                    TUC_IndividualData_EmergencyData UCEmergencyData =
                        (TUC_IndividualData_EmergencyData)FUserControlSetup[TDynamicLoadableUserControls.dlucEmergencyData];

                    if (!UCEmergencyData.ValidateAllData(AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Progress Reports (Person Evaluations)
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucProgressReports))
                {
                    TUC_IndividualData_ProgressReports UCProgressReport =
                        (TUC_IndividualData_ProgressReports)FUserControlSetup[TDynamicLoadableUserControls.dlucProgressReports];

                    if (!UCProgressReport.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Commitment Periods
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucCommitmentPeriods))
                {
                    TUC_IndividualData_CommitmentPeriods UCCommitmentPeriod =
                        (TUC_IndividualData_CommitmentPeriods)FUserControlSetup[TDynamicLoadableUserControls.dlucCommitmentPeriods];

                    if (!UCCommitmentPeriod.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Person Skills
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonSkills))
                {
                    TUC_IndividualData_PersonSkills UCPersonSkill =
                        (TUC_IndividualData_PersonSkills)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonSkills];

                    if (!UCPersonSkill.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Previous Experience
                // dluc corresponds to enum above, UCPreviousExperience to UC_IndividualData_PreviousExperience.ManualCode,
                //              PmPastExperienceTable to the DB table name
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPreviousExperience))
                {
                    TUC_IndividualData_PreviousExperience UCPreviousExperience =
                        (TUC_IndividualData_PreviousExperience)FUserControlSetup[TDynamicLoadableUserControls.dlucPreviousExperience];

                    if (!UCPreviousExperience.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Personal Documents
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalDocuments))
                {
                    TUC_IndividualData_PersonalDocuments UCPersonalDocuments =
                        (TUC_IndividualData_PersonalDocuments)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalDocuments];

                    if (!UCPersonalDocuments.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Job Assignments
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucJobAssignments))
                {
                    TUC_IndividualData_JobAssignments UCJobAssignments =
                        (TUC_IndividualData_JobAssignments)FUserControlSetup[TDynamicLoadableUserControls.dlucJobAssignments];

                    if (!UCJobAssignments.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    {
                        ReturnValue = false;
                    }
                }

                // Local Personnel Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucLocalPersonnelData))
                {
                    // TUC_IndividualData_LocalPersonnelData UCLocalPersonnelData =
                    //    (TUC_IndividualData_LocalPersonnelData)FUserControlSetup[TDynamicLoadableUserControls.dlucLocalPersonnelData];

                    //TODO: no proper validation in place yet for local personnel data control
                    //if (!UCLocalPersonnelData.ValidateAllData(false, AProcessAnyDataValidationErrors, AValidateSpecificControl))
                    //{
                    //    ReturnValue = false;
                    //}
                }
            }

            return ReturnValue;
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

                    // remove columns before merging (and re-add them afterwards) as otherwise merging raises exception
                    FPartnerEditTDS.Tables[PmPersonLanguageTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPersonLanguageTable.GetTableName()].Merge(FMainDS.PmPersonLanguage);
                }

                // Abilities
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalAbilities))
                {
                    TUC_IndividualData_Abilities UCAbilities =
                        (TUC_IndividualData_Abilities)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalAbilities];
                    UCAbilities.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPersonAbilityTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPersonAbilityTable());
                    }

                    FPartnerEditTDS.Tables[PmPersonAbilityTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPersonAbilityTable.GetTableName()].Merge(FMainDS.PmPersonAbility);
                }

                //Passport Details
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPassportDetails))
                {
                    TUC_IndividualData_Passport UCPassport =
                        (TUC_IndividualData_Passport)FUserControlSetup[TDynamicLoadableUserControls.dlucPassportDetails];
                    UCPassport.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPassportDetailsTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPassportDetailsTable());
                    }

                    // remove columns before merging (and re-add them afterwards) as otherwise merging raises exception
                    FPartnerEditTDS.Tables[PmPassportDetailsTable.GetTableName()].Rows.Clear();
                    FPartnerEditTDS.Tables[PmPassportDetailsTable.GetTableName()].Merge(FMainDS.PmPassportDetails);
                }

                //Personal Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalData))
                {
                    TUC_IndividualData_PersonalData UCPersonalData =
                        (TUC_IndividualData_PersonalData)FUserControlSetup[TDynamicLoadableUserControls.dlucPersonalData];
                    UCPersonalData.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPersonalDataTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPersonalDataTable());
                    }

                    FPartnerEditTDS.Tables[PmPersonalDataTable.GetTableName()].Merge(FMainDS.PmPersonalData);
                }

                //Emergency Data
                if (FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucEmergencyData))
                {
                    TUC_IndividualData_EmergencyData UCEmergencyData =
                        (TUC_IndividualData_EmergencyData)FUserControlSetup[TDynamicLoadableUserControls.dlucEmergencyData];
                    UCEmergencyData.GetDataFromControls2();

                    if (!FPartnerEditTDS.Tables.Contains(PmPersonalDataTable.GetTableName()))
                    {
                        FPartnerEditTDS.Tables.Add(new PmPersonalDataTable());
                    }

                    FPartnerEditTDS.Tables[PmPersonalDataTable.GetTableName()].Merge(FMainDS.PmPersonalData);
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

                // Previous Experience
                // dluc corresponds to enum above, UCPreviousExperience to UC_IndividualData_PreviousExperience.ManualCode,
                //              PmPastExperienceTable to the DB table name
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

                    // Set the job key before data sets are merged so the primary key of job assignment record
                    // does not have to be changed later. If UmJob record does not exist yet new key is set
                    // here but UmJob record still has to be created on server side.
                    foreach (PmJobAssignmentRow JobAssignmentRow in FMainDS.PmJobAssignment.Rows)
                    {
                        if ((JobAssignmentRow.RowState != DataRowState.Deleted)
                            && (JobAssignmentRow.JobKey < 0))
                        {
                            JobAssignmentRow.JobKey
                                = TRemote.MPersonnel.WebConnectors.GetOrCreateUmJobKey
                                      (JobAssignmentRow.PartnerKey,
                                      JobAssignmentRow.PositionName,
                                      JobAssignmentRow.PositionScope);
                        }
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

                    // don't clear rows as there might be valid rows from local partner data in there
                    FPartnerEditTDS.Tables[PDataLabelValuePartnerTable.GetTableName()].Merge(FMainDS.PDataLabelValuePartner);
                }
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
            // Need to merge Tables from PartnerEditTDS into IndividualDataTDS so the updated s_modification_id_t of modififed Rows is held correctly in IndividualDataTDS, too!
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

                //Personal Abilities
                case TDynamicLoadableUserControls.dlucPersonalAbilities:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPersonalAblities = new Panel();
                    pnlHostForUCPersonalAblities.AutoSize = true;
                    pnlHostForUCPersonalAblities.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPersonalAblities.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPersonalAblities.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPersonalAblities);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Abilities ucoPersonalAbilities =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Abilities();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPersonalAbilities, ucoPersonalAbilities);
                    ucoPersonalAbilities.Location = new Point(0, 2);
                    ucoPersonalAbilities.Dock = DockStyle.Fill;
                    pnlHostForUCPersonalAblities.Controls.Add(ucoPersonalAbilities);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPersonalAblities.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPersonalAblities.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPersonalAbilities;
                    break;

                //Passport Details
                case TDynamicLoadableUserControls.dlucPassportDetails:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPassport = new Panel();
                    pnlHostForUCPassport.AutoSize = true;
                    pnlHostForUCPassport.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPassport.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPassport.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPassport);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Passport ucoPassportDetails =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Passport();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPassportDetails, ucoPassportDetails);
                    ucoPassportDetails.Location = new Point(0, 2);
                    ucoPassportDetails.Dock = DockStyle.Fill;
                    pnlHostForUCPassport.Controls.Add(ucoPassportDetails);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPassport.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPassport.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPassportDetails;
                    break;

                case TDynamicLoadableUserControls.dlucPersonalData:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCPersonalData = new Panel();
                    pnlHostForUCPersonalData.AutoSize = true;
                    pnlHostForUCPersonalData.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCPersonalData.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCPersonalData.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCPersonalData);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalData ucoPersonalData =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalData();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucPersonalData, ucoPersonalData);
                    ucoPersonalData.Location = new Point(0, 2);
                    ucoPersonalData.Dock = DockStyle.Fill;
                    pnlHostForUCPersonalData.Controls.Add(ucoPersonalData);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCPersonalData.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCPersonalData.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoPersonalData;
                    break;

                case TDynamicLoadableUserControls.dlucEmergencyData:
                    // Create a Panel that hosts the UserControl. This is needed to allow scrolling of content in case the screen is too small to shown the whole UserControl
                    Panel pnlHostForUCEmergencyData = new Panel();
                    pnlHostForUCEmergencyData.AutoSize = true;
                    pnlHostForUCEmergencyData.Dock = System.Windows.Forms.DockStyle.Fill;
                    pnlHostForUCEmergencyData.Location = new System.Drawing.Point(0, 0);
                    pnlHostForUCEmergencyData.Padding = new System.Windows.Forms.Padding(2);
                    pnlSelectedIndivDataItem.Controls.Add(pnlHostForUCEmergencyData);

                    // Create the UserControl
                    Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_EmergencyData ucoEmergencyData =
                        new Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_EmergencyData();
                    FUserControlSetup.Add(TDynamicLoadableUserControls.dlucEmergencyData, ucoEmergencyData);
                    ucoEmergencyData.Location = new Point(0, 2);
                    ucoEmergencyData.Dock = DockStyle.Fill;
                    pnlHostForUCEmergencyData.Controls.Add(ucoEmergencyData);

                    /*
                     * The following four commands seem strange and unnecessary; however, they are necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        pnlHostForUCEmergencyData.Dock = System.Windows.Forms.DockStyle.None;
                        pnlHostForUCEmergencyData.Dock = System.Windows.Forms.DockStyle.Fill;
                    }

                    ReturnValue = ucoEmergencyData;
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

            foreach (Control Cntrl in this.pnlIndivDataMenu.Controls)
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
            if ((ASender != FCurrentLinkLabel)
                && !ValidateCurrentDataItem())
            {
                // do not accept new link section if current one is not validated properly
                return;
            }

            /*
             * Raise the following Event to inform the base Form that we might be loading some fresh data.
             * We need to bypass the ChangeDetection routine while this happens.
             */
            OnDataLoadingStarted(this, new EventArgs());

            ResetAllBackColors();

            if (ASender == llbOverview)
            {
                ucoSummaryData.BringToFront();
                llbOverview.BackColor = PanelHelperBackGround;
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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoSpecialNeeds.AdjustAfterResizing();
                    }
                }

                llbSpecialNeeds.BackColor = PanelHelperBackGround;

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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPersonalLanguages.AdjustAfterResizing();
                    }
                }

                llbLanguages.BackColor = PanelHelperBackGround;

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

                    SendAllOtherItemsToBackExcluding("FUcoProgressReports");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoProgressReports.AdjustAfterResizing();
                    }
                }

                llbProgressReports.BackColor = PanelHelperBackGround;

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

                    SendAllOtherItemsToBackExcluding("FUcoCommitmentPeriods");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoCommitmentPeriods.AdjustAfterResizing();
                    }
                }

                llbCommitmentPeriods.BackColor = PanelHelperBackGround;

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

                    SendAllOtherItemsToBackExcluding("FUcoPersonSkills");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPersonSkills.AdjustAfterResizing();
                    }
                }

                llbPersonSkills.BackColor = PanelHelperBackGround;

                FUcoPersonSkills.Parent.BringToFront();
            }
            else if (ASender == llbPersonalAbilities)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalAbilities))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPersonalAbilities = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Abilities)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPersonalAbilities);

                    // Hook up RecalculateScreenParts Event
                    FUcoPersonalAbilities.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPersonalAbilities.MainDS = FMainDS;
                    FUcoPersonalAbilities.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPersonalAbilities.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPersonalAbilities.SpecialInitUserControl(FMainDS);
                    FUcoPersonalAbilities.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonalAbilities);

                    SendAllOtherItemsToBackExcluding("FUcoPersonalAbilities");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPersonalAbilities.AdjustAfterResizing();
                    }
                }

                llbPersonalAbilities.BackColor = PanelHelperBackGround;

                FUcoPersonalAbilities.Parent.BringToFront();
            }
            else if (ASender == llbPassportDetails)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPassportDetails))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPassportDetails = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_Passport)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPassportDetails);

                    // Hook up RecalculateScreenParts Event
                    FUcoPassportDetails.RecalculateScreenParts += new TRecalculateScreenPartsEventHandler(RecalculateLinkLabelCounters);

                    FUcoPassportDetails.MainDS = FMainDS;
                    FUcoPassportDetails.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPassportDetails.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPassportDetails.SpecialInitUserControl(FMainDS);
                    FUcoPassportDetails.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPassportDetails);

                    SendAllOtherItemsToBackExcluding("FUcoPassportDetails");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPassportDetails.AdjustAfterResizing();
                    }
                }

                llbPassportDetails.BackColor = PanelHelperBackGround;

                FUcoPassportDetails.Parent.BringToFront();
            }
            else if (ASender == llbPersonalData)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucPersonalData))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoPersonalData = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_PersonalData)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucPersonalData);
                    FUcoPersonalData.MainDS = FMainDS;
                    FUcoPersonalData.PetraUtilsObject = FPetraUtilsObject;
                    FUcoPersonalData.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoPersonalData.SpecialInitUserControl(FMainDS);
                    FUcoPersonalData.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoPersonalData);

                    SendAllOtherItemsToBackExcluding("FUcoPersonalData");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPersonalData.AdjustAfterResizing();
                    }
                }

                llbPersonalData.BackColor = PanelHelperBackGround;

                FUcoPersonalData.Parent.BringToFront();
            }
            else if (ASender == llbEmergencyData)
            {
                if (!FUserControlSetup.ContainsKey(TDynamicLoadableUserControls.dlucEmergencyData))
                {
                    if (TClientSettings.DelayedDataLoading)
                    {
                        // Signalise the user that data is beeing loaded
                        this.Cursor = Cursors.AppStarting;
                    }

                    FUcoEmergencyData = (Ict.Petra.Client.MPartner.Gui.TUC_IndividualData_EmergencyData)DynamicLoadUserControl(
                        TDynamicLoadableUserControls.dlucEmergencyData);
                    FUcoEmergencyData.MainDS = FMainDS;
                    FUcoEmergencyData.PetraUtilsObject = FPetraUtilsObject;
                    FUcoEmergencyData.PartnerEditUIConnector = FPartnerEditUIConnector;
                    FUcoEmergencyData.SpecialInitUserControl(FMainDS);
                    FUcoEmergencyData.InitUserControl();
                    ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUcoEmergencyData);

                    SendAllOtherItemsToBackExcluding("FUcoEmergencyData");

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgEmergencyData, FUcoEmergencyData, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoEmergencyData.AdjustAfterResizing();
                    }
                }

                llbEmergencyData.BackColor = PanelHelperBackGround;

                FUcoEmergencyData.Parent.BringToFront();
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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPreviousExperience.AdjustAfterResizing();
                    }
                }

                llbPreviousExperience.BackColor = PanelHelperBackGround;

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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoPersonalDocuments.AdjustAfterResizing();
                    }
                }

                llbPersonalDocuments.BackColor = PanelHelperBackGround;

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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoJobAssignments.AdjustAfterResizing();
                    }
                }

                llbJobAssignments.BackColor = PanelHelperBackGround;

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

                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "InitialActivation"));

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // The following code is not needed at the moment unless there would be some special initialization later on
                    // beyond what it is done in SpecialInitUserControl
                    //OnTabPageEvent(new TTabPageEventArgs(tpgPartnerTypes, FUcoPartnerTypes, "SubsequentActivation"));

                    /*
                     * The following command seems strange and unnecessary; however, it is necessary
                     * to make things scale correctly on "Large Fonts (120DPI)" display setting.
                     */
                    if (TClientSettings.GUIRunningOnNonStandardDPI)
                    {
                        FUcoLocalPersonnelData.AdjustAfterResizing();
                    }
                }

                llbLocalPersonnelData.BackColor = PanelHelperBackGround;

                FUcoLocalPersonnelData.Parent.BringToFront();
            }

            // remember the currently selected link label
            FCurrentLinkLabel = ASender as LinkLabel;

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
        private void SendAllOtherItemsToBackExcluding(String exclude)
        {
            if ((FUcoPersonalLanguages != null) && (exclude != "FUcoPersonalLanguages"))
            {
                FUcoPersonalLanguages.SendToBack();
            }

            if ((FUcoSpecialNeeds != null) && (exclude != "FUcoSpecialNeeds"))
            {
                FUcoSpecialNeeds.SendToBack();
            }

            if ((FUcoPersonalAbilities != null) && (exclude != "FUcoPersonalAbilities"))
            {
                FUcoPersonalAbilities.SendToBack();
            }

            if ((FUcoPassportDetails != null) && (exclude != "FUcoPassportDetails"))
            {
                FUcoPassportDetails.SendToBack();
            }

            if ((FUcoPersonalData != null) && (exclude != "FUcoPersonalData"))
            {
                FUcoPersonalData.SendToBack();
            }

            if ((FUcoEmergencyData != null) && (exclude != "FUcoEmergencyData"))
            {
                FUcoEmergencyData.SendToBack();
            }

            if ((FUcoPersonSkills != null) && (exclude != "FUcoPersonSkills"))
            {
                FUcoPersonSkills.SendToBack();
            }

            if ((FUcoProgressReports != null) && (exclude != "FUcoProgressReports"))
            {
                FUcoProgressReports.SendToBack();
            }

            if ((FUcoCommitmentPeriods != null) && (exclude != "FUcoCommitmentPeriods"))
            {
                FUcoCommitmentPeriods.SendToBack();
            }

            if ((FUcoPreviousExperience != null) && (exclude != "FUcoPreviousExperience"))
            {
                FUcoPreviousExperience.SendToBack();
            }

            if ((FUcoPersonalDocuments != null) && (exclude != "FUcoPersonalDocuments"))
            {
                FUcoPersonalDocuments.SendToBack();
            }

            if ((FUcoJobAssignments != null) && (exclude != "FUcoJobAssignments"))
            {
                FUcoJobAssignments.SendToBack();
            }

            if ((FUcoLocalPersonnelData != null) && (exclude != "FUcoLocalPersonnelData"))
            {
                FUcoLocalPersonnelData.SendToBack();
            }
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_Passport))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_Abilities))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_CommitmentPeriods))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_ProgressReports))
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

            if ((ASender is TUC_IndividualData) || (ASender is TUC_IndividualData_PersonSkills))
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
                    }
                }
            }
        }

        /// <summary>
        /// Resets the BackColor on all LinkLabels
        /// </summary>
        /// <returns>void</returns>
        private void ResetAllBackColors()
        {
            llbOverview.ResetBackColor();
            llbEmergencyData.ResetBackColor();
            llbCommitmentPeriods.ResetBackColor();
            llbJobAssignments.ResetBackColor();
            llbPassportDetails.ResetBackColor();
            llbPersonalDocuments.ResetBackColor();
            llbSpecialNeeds.ResetBackColor();
            llbLanguages.ResetBackColor();
            llbPersonSkills.ResetBackColor();
            llbPreviousExperience.ResetBackColor();
            llbProgressReports.ResetBackColor();
            llbLocalPersonnelData.ResetBackColor();
            llbPersonalData.ResetBackColor();
            llbProfessionalAreas.ResetBackColor();
            llbPersonalAbilities.ResetBackColor();
        }

        private bool ValidateCurrentDataItem()
        {
            if (FCurrentLinkLabel == llbOverview)
            {
            }
            else if (FCurrentLinkLabel == llbCommitmentPeriods)
            {
                if (FUcoCommitmentPeriods != null)
                {
                    return FUcoCommitmentPeriods.ValidateAllData(true, true, FUcoCommitmentPeriods);
                }
            }
            else if (FCurrentLinkLabel == llbJobAssignments)
            {
                if (FUcoJobAssignments != null)
                {
                    return FUcoJobAssignments.ValidateAllData(true, true, FUcoJobAssignments);
                }
            }
            else if (FCurrentLinkLabel == llbPassportDetails)
            {
                if (FUcoPassportDetails != null)
                {
                    return FUcoPassportDetails.ValidateAllData(true, true, FUcoPassportDetails);
                }
            }
            else if (FCurrentLinkLabel == llbPersonalDocuments)
            {
                if (FUcoPersonalDocuments != null)
                {
                    return FUcoPersonalDocuments.ValidateAllData(true, true, FUcoPersonalDocuments);
                }
            }
            else if (FCurrentLinkLabel == llbSpecialNeeds)
            {
                if (FUcoSpecialNeeds != null)
                {
                    return FUcoSpecialNeeds.ValidateAllData(true, FUcoSpecialNeeds);
                }
            }
            else if (FCurrentLinkLabel == llbLanguages)
            {
                if (FUcoPersonalLanguages != null)
                {
                    return FUcoPersonalLanguages.ValidateAllData(true, true, FUcoPersonalLanguages);
                }
            }
            else if (FCurrentLinkLabel == llbPersonSkills)
            {
                if (FUcoPersonSkills != null)
                {
                    return FUcoPersonSkills.ValidateAllData(true, true, FUcoPersonSkills);
                }
            }
            else if (FCurrentLinkLabel == llbPreviousExperience)
            {
                if (FUcoPreviousExperience != null)
                {
                    return FUcoPreviousExperience.ValidateAllData(true, true, FUcoPreviousExperience);
                }
            }
            else if (FCurrentLinkLabel == llbProgressReports)
            {
                if (FUcoProgressReports != null)
                {
                    return FUcoProgressReports.ValidateAllData(true, true, FUcoProgressReports);
                }
            }
            else if (FCurrentLinkLabel == llbLocalPersonnelData)
            {
                if (FUcoLocalPersonnelData != null)
                {
                    //TODO: no proper validation in place yet for local personnel data control
                }
            }
            else if (FCurrentLinkLabel == llbPersonalData)
            {
                if (FUcoPersonalData != null)
                {
                    return FUcoPersonalData.ValidateAllData(true, FUcoPersonalData);
                }
            }

            return true;
        }

        #endregion

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if ((FCurrentLinkLabel == llbCommitmentPeriods) && FUcoCommitmentPeriods.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbJobAssignments) && FUcoJobAssignments.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbLanguages) && FUcoPersonalLanguages.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbPassportDetails) && FUcoPassportDetails.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbPersonalDocuments) && FUcoPersonalDocuments.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbPersonSkills) && FUcoPersonSkills.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbPreviousExperience) && FUcoPreviousExperience.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbProgressReports) && FUcoProgressReports.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbEmergencyData) && FUcoEmergencyData.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }
            else if ((FCurrentLinkLabel == llbPersonalData) && FUcoPersonalData.ProcessParentCmdKey(ref msg, keyData))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}