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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData_Summary
    {
        private const string DEV_FIX = "DEVELOPER NEEDS TO FIX THIS!!!";

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private string FPhoneOfPerson = null;
        private string FEmailOfPerson = null;
        private Int64[] FSupportingChurchesPartnerKeys = null;

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

        #endregion

        /// <summary>
        /// Ensure that data is loaded and shown.
        /// </summary>
        /// <returns>void</returns>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            SpecialShowData();
        }

        /// <summary>
        /// Calls the normal ShowData() Method and then performs the rest of the
        /// setting up of the data that is shown in the screen.
        /// </summary>
        /// <returns>void</returns>
        private void SpecialShowData()
        {
            ShowData((PPersonRow)FMainDS.PPerson.Rows[0]);

            // Check for unexpected condition...
            if (FMainDS.SummaryData.Rows.Count == 0)
            {
                MessageBox.Show("FMainDS.SummaryData holds NO ROWS!", DEV_FIX);
            }

            // Show note about multiple Churches/Pastors, if applicable
            SetupMultipleRecordsInfoText();

            // Setup Jobs/Commitments Grid
            DataView myDataView = FMainDS.JobAssignmentStaffDataCombined.DefaultView;
            myDataView.AllowNew = false;
            myDataView.Sort = PmJobAssignmentTable.GetFromDateDBName() + " DESC";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            // Determine the 'Primary Phone Number' and the 'Primary E-mail Address' of the PERSON
            if (FMainDS.Tables[PPartnerAttributeTable.GetTableName()] != null)
            {
                DeterminePrimaryEmailAndPrimaryPhone(out FPhoneOfPerson, out FEmailOfPerson);
            }

            // Record current relationship(s) that are supporting Church(es) of the PERSON
            if (FMainDS.Tables[PPartnerRelationshipTable.GetTableName()] != null)
            {
                DetermineChurchRelationships(out FSupportingChurchesPartnerKeys);
            }
        }

        /// <summary>
        /// Triggered after a change to a DataRow in the PPerson DataTable in the *screen's main DataSet*.
        /// </summary>
        /// <param name="APersonRow">PPerson DataRow containing the changed data.</param>
        /// <returns>void</returns>
        public void FMainDS_PPerson_ColumnChanged(PPersonRow APersonRow)
        {
            string MaritalStatusDesc = PartnerCodeHelper.GetMaritalStatusDescription(
                @TDataCache.GetCacheableDataTableFromCache, APersonRow.MaritalStatus);

            if (MaritalStatusDesc != String.Empty)
            {
                MaritalStatusDesc = " - " + MaritalStatusDesc;
            }

            txtGender.Text = APersonRow.Gender;
            txtMaritalStatus.Text = APersonRow.MaritalStatus + MaritalStatusDesc;

            dtpDateOfBirth.Date = APersonRow.DateOfBirth;
        }

        /// <summary>
        /// Called when data got saved in the screen. Performs a check whether reloading
        /// of the 'SummaryData' is necessary to reflect changes that were done elsewhere
        /// in the Partner Edit screen and which just got saved.
        /// </summary>
        /// <returns>void</returns>
        public void CheckForRefreshOfDisplayedData(bool AJobAndStaffDataGridNeedsRefresh)
        {
            bool RefreshNecessary = false;
            string PhoneOfPerson;
            string EmailOfPerson;

            if (!AJobAndStaffDataGridNeedsRefresh)
            {
                Int64[] SupportingChurchesPartnerKeys = new long[0];

                if (FMainDS.Tables[PPartnerAttributeTable.GetTableName()] != null)
                {
                    // Check for change of the 'Primary Phone Number' and the 'Primary E-mail Address' of the PERSON
                    DeterminePrimaryEmailAndPrimaryPhone(out PhoneOfPerson, out EmailOfPerson);

                    if ((PhoneOfPerson != FPhoneOfPerson)
                        || (EmailOfPerson != FEmailOfPerson))
                    {
                        RefreshNecessary = true;
                    }
                }

                if (FMainDS.Tables[PPartnerRelationshipTable.GetTableName()] != null)
                {
                    // Check for change in supporting Church/es relationship(s)
                    DetermineChurchRelationships(out SupportingChurchesPartnerKeys);

                    if ((FSupportingChurchesPartnerKeys == null)
                        || (FSupportingChurchesPartnerKeys.Length != SupportingChurchesPartnerKeys.Length))
                    {
                        RefreshNecessary = true;
                    }
                    else
                    {
                        for (int Counter = 0; Counter < SupportingChurchesPartnerKeys.Length; Counter++)
                        {
                            if (SupportingChurchesPartnerKeys[Counter] != FSupportingChurchesPartnerKeys[Counter])
                            {
                                RefreshNecessary = true;
                            }
                        }
                    }
                }
            }
            else
            {
                RefreshNecessary = true;
            }

            if (RefreshNecessary)
            {
                // Call WebConnector to retrieve SummaryData afresh!
                IndividualDataTDS FillDS = new IndividualDataTDS();
                FillDS.Merge(FMainDS.MiscellaneousData);

                TRemote.MPersonnel.Person.DataElements.WebConnectors.GetSummaryData(FMainDS.PPerson[0].PartnerKey, ref FillDS);

                FMainDS.SummaryData.Rows.Clear();
                FMainDS.Merge(FillDS.SummaryData);
                FMainDS.JobAssignmentStaffDataCombined.Rows.Clear();
                FMainDS.Merge(FillDS.JobAssignmentStaffDataCombined);

                // Refresh the displayed data
                SpecialShowData();
            }
        }

        /// <summary>
        /// This empty Method is needed so that the 'SAVEDATA' section of the template for the auto-generated class can be filled in.
        /// It is a HACK, since this screen is read-only and wouldn't need any saving code at all...
        /// FIXME in the WinForms generator/devise another template for read-only screens...
        /// </summary>
        /// <param name="ARow">Not evaluated.</param>
        /// <returns>void</returns>
        private void GetDataFromControlsManual(PPersonRow ARow)
        {
        }

        /// <summary>
        /// Loads Summary Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.SummaryData == null)
                {
                    FMainDS.Tables.Add(new IndividualDataTDSSummaryDataTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.SummaryData.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiSummary));

                    // Make DataRows unchanged
                    if (FMainDS.SummaryData.Rows.Count > 0)
                    {
                        if (FMainDS.SummaryData.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.SummaryData.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.SummaryData.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }

            return ReturnValue;
        }

        #region Helper Methods

        /// <summary>
        /// Shows one or two 'notes' using the lblMultipleRecordsInfo if there is a condition
        /// that triggers such notes, otherwise no note is shown and lblMultipleRecordsInfo is hidden
        /// so that it doesn't take up space on the screen.
        /// </summary>
        /// <returns>void</returns>
        private void SetupMultipleRecordsInfoText()
        {
            string MultipleRecordsInfoText = String.Empty;
            string NotePrefix = Catalog.GetString("Note: ");

            if (FMainDS.SummaryData[0].NumberOfShownSupportingChurches > 1)
            {
                MultipleRecordsInfoText = NotePrefix + String.Format(Catalog.GetString(
                        "The Person has {0} supporting churches, only one is shown here."),
                    FMainDS.SummaryData[0].NumberOfShownSupportingChurches);
            }

            if (FMainDS.SummaryData[0].NumberOfShownSupportingChurchPastors > 1)
            {
                if (MultipleRecordsInfoText != String.Empty)
                {
                    MultipleRecordsInfoText += Environment.NewLine;
                }

                MultipleRecordsInfoText += NotePrefix;

                MultipleRecordsInfoText += String.Format(Catalog.GetString(
                        "The shown church has {0} pastors, only one is shown here."),
                    FMainDS.SummaryData[0].NumberOfShownSupportingChurchPastors);
            }

            if (MultipleRecordsInfoText != String.Empty)
            {
                lblMultipleRecordsInfo.Text = MultipleRecordsInfoText;
                lblMultipleRecordsInfo.Visible = true;
                lblMultipleRecordsInfo.ForeColor = System.Drawing.Color.SaddleBrown;

                pnlChurchInfoData.Height = 162;
                grpChurchInfo.Height = 181;
                grpJobCommitment.Top = 372;
            }
            else
            {
                lblMultipleRecordsInfo.Visible = false;

                // 'Shrink' Church Info section and move Job/Commitment section up as lblMultipleRecordsInfo isn't taking up space...
                pnlChurchInfoData.Height = 162 - 25;
                grpChurchInfo.Height = 181 - 25;
                grpJobCommitment.Top = 372 - 25;
            }
        }

        /// <summary>
        /// Determines the 'Primary Phone Number' and the 'Primary E-mail Address' of the PERSON.
        /// </summary>
        /// <param name="APrimaryPhoneNumberOfPerson">'Primary Phone Number' of the PERSON.</param>
        /// <param name="APrimaryEmailAddressOfPerson">'Primary E-mail Address'.</param>
        private void DeterminePrimaryEmailAndPrimaryPhone(out string APrimaryPhoneNumberOfPerson,
            out string APrimaryEmailAddressOfPerson)
        {
            Ict.Petra.Shared.MPartner.Calculations.GetPrimaryEmailAndPrimaryPhone(
                (PPartnerAttributeTable)FMainDS.Tables[PPartnerAttributeTable.GetTableName()],
                out APrimaryPhoneNumberOfPerson, out APrimaryEmailAddressOfPerson);
        }

        /// <summary>
        /// Determines whether a PERSON has one or more 'Supporting Church' Relationship(s).
        /// </summary>
        /// <param name="ASupportingChurchesPartnerKeys">PartnerKeys of 'Supporting Churches'.</param>
        /// <returns>void</returns>
        private void DetermineChurchRelationships(out long[] ASupportingChurchesPartnerKeys)
        {
            DataRow[] SupportingChurches;

            // Initialise out Argument
            ASupportingChurchesPartnerKeys = new long[0];

            SupportingChurches = FMainDS.Tables[PPartnerRelationshipTable.GetTableName()].Select(
                PPartnerRelationshipTable.GetRelationNameDBName() + "= 'SUPPCHURCH'");

            if (SupportingChurches != null)
            {
                ASupportingChurchesPartnerKeys = new long[SupportingChurches.Length];

                for (int Counter = 0; Counter < SupportingChurches.Length; Counter++)
                {
                    ASupportingChurchesPartnerKeys[Counter] = (long)SupportingChurches[Counter][PPartnerRelationshipTable.GetPartnerKeyDBName()];
                }
            }
        }

        #endregion
    }
}