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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData_EmergencyData
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

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

        #region Public methods

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            int TmpTabIndex;

            FMainDS = AMainDS;

            LoadDataOnDemand();

            if (FMainDS.PmPersonalData.Rows.Count == 0)
            {
                // There hasn't been data stored yet, so create a new Record
                FMainDS.PmPersonalData.Rows.Add(FMainDS.PmPersonalData.NewRowTyped(true));
                // ... and set its Primary Key
                FMainDS.PmPersonalData[0].PartnerKey = FMainDS.PPerson[0].PartnerKey;
            }

            ShowData(FMainDS.PmPersonalData[0]);

            // Set default facial hair text to 'None' if person is female
            if ((FMainDS.PPerson[0].Gender == "Female") || (FMainDS.PPerson[0].Gender == "FEMALE"))
            {
                cmbFacialHair.SelectedText = "None";
            }

            pnlEmergencyContacts.Top = 0;
            pnlEmergencyContacts.Width = 250;
            pnlEmergencyContacts.Height = 39;
            btnEmergencyContacts.Top = 6;
            btnEmergencyContacts.Left = 99;
            lblIdentMarks.Top = 37;
            txtIdentifyingMarks.Top = 57;
            pnlEmergencyDataRight.Height = 180;
            grpEmergencyData.Height = 200;

            // Alter Tab order of txtIdentifyingMarks and btnEmergencyContacts so that the TextBox comes
            // right after the other TextBox Controls
            TmpTabIndex = txtIdentifyingMarks.TabIndex;
            txtIdentifyingMarks.TabIndex = btnEmergencyContacts.TabIndex;
            btnEmergencyContacts.TabIndex = TmpTabIndex;
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            GetDataFromControls(FMainDS.PmPersonalData[0]);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private methods including LoadDataOnDemand and Emergency Contacts

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
                if (FMainDS.PmPersonalData == null)
                {
                    FMainDS.Tables.Add(new PmPersonalDataTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.PmPersonalData.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiEmergencyData));

                    // Make DataRows unchanged
                    if (FMainDS.PmPersonalData.Rows.Count > 0)
                    {
                        if (FMainDS.PmPersonalData.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmPersonalData.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PmPersonalData.Rows.Count != 0)
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

        #region Emergency Contacts

        private void ShowEmergencyContacts(object sender, EventArgs e)
        {
            string contactFor = string.Empty;
            string primaryContact = Catalog.GetString("PRIMARY EMERGENCY CONTACT") + Environment.NewLine + Environment.NewLine;
            string secondaryContact = Catalog.GetString("SECONDARY EMERGENCY CONTACT") + Environment.NewLine + Environment.NewLine;
            Int64 primaryContactKey = 0;
            Int64 secondaryContactKey = 0;

            if ((FMainDS.PPerson != null) && (FMainDS.PPerson.Rows.Count > 0))
            {
                PPersonRow row = (PPersonRow)FMainDS.PPerson.Rows[0];
                contactFor = String.Format(Catalog.GetString("Emergency Contact Information For: {0} {1} {2} [{3:0000000000}]"),
                    row.Title, row.FirstName, row.FamilyName, row.PartnerKey);

                FPetraUtilsObject.GetForm().Cursor = Cursors.WaitCursor;
                PPartnerRelationshipTable relationshipTable = TRemote.MPartner.Partner.WebConnectors.GetPartnerRelationships(row.PartnerKey);
                FPetraUtilsObject.GetForm().Cursor = Cursors.Default;

                for (int i = 0; i < relationshipTable.Rows.Count; i++)
                {
                    PPartnerRelationshipRow relationshipRow = (PPartnerRelationshipRow)relationshipTable.Rows[i];

                    if (string.Compare(relationshipRow.RelationName, "EMER-1", true) == 0)
                    {
                        ParseEmergencyContactData(ref primaryContact, relationshipRow);
                        primaryContactKey = relationshipRow.PartnerKey;
                    }
                    else if (string.Compare(relationshipRow.RelationName, "EMER-2", true) == 0)
                    {
                        ParseEmergencyContactData(ref secondaryContact, relationshipRow);
                        secondaryContactKey = relationshipRow.PartnerKey;
                    }
                }
            }

            if (primaryContactKey == 0)
            {
                primaryContact += Catalog.GetString("No primary contact");
            }

            if (secondaryContactKey == 0)
            {
                secondaryContact += Catalog.GetString("No secondary contact");
            }

            if ((primaryContactKey != 0) || (secondaryContactKey != 0))
            {
                // Show the emergency contacts dialog and pass it the inofrmation we have found
                TFrmEmergencyContactsDialog dlg = new TFrmEmergencyContactsDialog(FPetraUtilsObject.GetCallerForm());
                dlg.SetParameters(contactFor, primaryContact, secondaryContact, primaryContactKey, secondaryContactKey);
                dlg.Show();
            }
            else
            {
                MessageBox.Show(Catalog.GetString("There is no emergency contact information for this person."),
                    Catalog.GetString("Emergency Contacts"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ParseEmergencyContactData(ref string AContact, PPartnerRelationshipRow ARelationshipRow)
        {
            string primaryPhone;
            string primaryEmail;
            PartnerEditTDS editTDS = TRemote.MPartner.Partner.WebConnectors.GetPartnerDetails(ARelationshipRow.PartnerKey,
                false, false, out primaryPhone, out primaryEmail);

            if ((editTDS.PPartner != null) && (editTDS.PPartner.Rows.Count > 0))
            {
                PPartnerRow partnerRow = (PPartnerRow)editTDS.PPartner.Rows[0];
                AContact += String.Format("{0}{1}{1}", partnerRow.PartnerShortName, Environment.NewLine);
            }

            DateTime? dtLastModified = null;

            if (!ARelationshipRow.IsDateModifiedNull())
            {
                // We have a modified date
                dtLastModified = ARelationshipRow.DateModified;

                // Just check to make sure that the created date is not later for some starnge reason
                if (!ARelationshipRow.IsDateCreatedNull())
                {
                    if (ARelationshipRow.DateCreated > dtLastModified)
                    {
                        dtLastModified = ARelationshipRow.DateCreated;
                    }
                }
            }
            else if (!ARelationshipRow.IsDateCreatedNull())
            {
                // No date modifed but we do have the original creation date
                dtLastModified = ARelationshipRow.DateCreated;
            }

            if (dtLastModified.HasValue)
            {
                AContact += String.Format(Catalog.GetString("This contact was last set on: {0}{1}{1}"),
                    StringHelper.DateToLocalizedString(dtLastModified.Value), Environment.NewLine);
            }

            if ((primaryPhone != null) && (primaryPhone.Length > 0))
            {
                AContact += String.Format(Catalog.GetString("Primary phone: {0}{1}"), primaryPhone, Environment.NewLine);
            }

            if ((primaryEmail != null) && (primaryEmail.Length > 0))
            {
                AContact += String.Format(Catalog.GetString("Primary email: {0}{1}"), primaryEmail, Environment.NewLine);
            }

            if (ARelationshipRow.Comment.Length > 0)
            {
                AContact += String.Format(Catalog.GetString("Extra information:{1}   {0}{1}"), ARelationshipRow.Comment, Environment.NewLine);
            }
        }

        #endregion

        #endregion

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        //// Special Handlers for menus and command keys for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.E | Keys.Control))
            {
                this.txtHeightCm.Focus();
                return true;
            }

            return false;
        }

        #endregion
    }
}