//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ContactDetails
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

//        private TUCPartnerRelationshipsLogic FLogic;

        #region Public Methods

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
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
            // TODO
                // Do not call this method in your manual code.
                // This is a method that is private to the generated code and is part of the Validation process.
                // If you need to update the controls data into the Data Row object, you must use ValidateAllData and be prepared
                //   to handle the consequences of a failed validation.         
//            GetDetailsFromControls(GetSelectedDetailRow());
        }

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            // Set up screen logic
//            FLogic.MultiTableDS = (PartnerEditTDS)FMainDS;
//            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
//            FLogic.LoadDataOnDemand();

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Description", FMainDS.PPartnerRelationship.Columns["RelationDescription"]);
            grdDetails.AddPartnerKeyColumn("Partner Key", FMainDS.PPartnerRelationship.Columns["OtherPartnerKey"]);
            grdDetails.AddTextColumn("Partner Name",
                FMainDS.PPartnerRelationship.Columns[PartnerEditTDSPPartnerRelationshipTable.GetPartnerShortNameDBName()]);
            grdDetails.AddTextColumn("Class", FMainDS.PPartnerRelationship.Columns[PartnerEditTDSPPartnerRelationshipTable.GetPartnerClassDBName()]);
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerRelationship.Columns[PPartnerRelationshipTable.GetCommentDBName()]);

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpPartnerRelationships));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
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

        private void InitializeManualCode()
        {
//            FLogic = new TUCPartnerRelationshipsLogic();
//
//            if (!FMainDS.Tables.Contains(PartnerEditTDSPPartnerRelationshipTable.GetTableName()))
//            {
//                FMainDS.Tables.Add(new PartnerEditTDSPPartnerRelationshipTable());
//            }

            FMainDS.InitVars();
        }

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerAttributeRow ARow)
        {
//            long RelationPartnerKey;

            // show controls if not visible yet
            MakeDetailsInvisible(false);

            btnDelete.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
            }
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeRow ARow)
        {
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRecord(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRecord(this, null);
            }
        }

        /// <summary>
        /// adding a new partner relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            if (CreateNewPPartnerAttribute())
            {
                cmbCode.Focus();
            }

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PPartnerAttributeRow ARow)
        {
            // TODO?
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            // TODO?
//            this.DeletePPartnerAttribute();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PPartnerAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
//            ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Contact Detail record: '{0}'?"),
//                ARowToDelete.RelationName);
            return true;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(PPartnerAttributeRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            // no message to be shown after deletion
            ACompletionMessage = "";

            try
            {
                ARowToDelete.Delete();
                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PPartnerAttributeRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            DoRecalculateScreenParts();

            if (grdDetails.Rows.Count <= 1)
            {
                // hide details part and disable buttons if no record in grid (first row for headings)
                btnDelete.Enabled = false;
                pnlDetails.Visible = false;
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        private void MakeDetailsInvisible(Boolean value)
        {
            /* make the details part of this screen visible or invisible. */
            this.pnlDetails.Visible = !value;
        }

        private void ValidateDataDetailsManual(PPartnerAttributeRow ARow)
        {
            bool NewPartner = (FMainDS.PPartner.Rows[0].RowState == DataRowState.Added);

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

//            TSharedPartnerValidation_Partner.ValidateRelationshipManual(this, ARow, ref VerificationResultCollection,
//                FValidationControlsDict, NewPartner, ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey);
        }

        #endregion
    }
}