//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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
    public partial class TUC_PartnerRelationships
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private TUCPartnerRelationshipsLogic FLogic;

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
            GetDetailsFromControls(GetSelectedDetailRow());
        }

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            // Set up screen logic
            FLogic.MultiTableDS = (PartnerEditTDS)FMainDS;
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.LoadDataOnDemand();

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

            // hook up partner changed event for partner key and relation key so grid can be updated when new record is created
            // where data did not already come from server
            txtPPartnerRelationshipPartnerKey.ValueChanged += new TDelegatePartnerChanged(txtPPartnerRelationshipPartnerKey_ValueChanged);
            txtPPartnerRelationshipRelationKey.ValueChanged += new TDelegatePartnerChanged(txtPPartnerRelationshipRelationKey_ValueChanged);

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
                btnEditOtherPartner.Enabled = false;
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
            FLogic = new TUCPartnerRelationshipsLogic();

            if (!FMainDS.Tables.Contains(PartnerEditTDSPPartnerRelationshipTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PartnerEditTDSPPartnerRelationshipTable());
            }

            FMainDS.InitVars();
        }

        private void ShowDataManual()
        {
        }

        private void ShowDetailsManual(PPartnerRelationshipRow ARow)
        {
            long RelationPartnerKey;

            // show controls if not visible yet
            MakeDetailsInvisible(false);

            btnDelete.Enabled = false;
            btnEditOtherPartner.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;

                // depending on the relation select other partner to be edited
                if (ARow.PartnerKey == ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
                {
                    RelationPartnerKey = GetSelectedDetailRow().RelationKey;
                }
                else
                {
                    RelationPartnerKey = GetSelectedDetailRow().PartnerKey;
                }

                if (RelationPartnerKey != 0)
                {
                    btnEditOtherPartner.Enabled = true;
                }

                if (ARow.RowState == DataRowState.Added)
                {
                    txtPPartnerRelationshipPartnerKey.Enabled = true;
                    cmbPPartnerRelationshipRelationName.Enabled = true;
                    txtPPartnerRelationshipRelationKey.Enabled = true;
                }
                else
                {
                    txtPPartnerRelationshipPartnerKey.Enabled = false;
                    cmbPPartnerRelationshipRelationName.Enabled = false;
                    txtPPartnerRelationshipRelationKey.Enabled = false;
                }
            }
        }

        private void GetDetailDataFromControlsManual(PPartnerRelationshipRow ARow)
        {
        }

        void txtPPartnerRelationshipPartnerKey_ValueChanged(long APartnerKey, string APartnerShortName, bool AValidSelection)
        {
            PartnerEditTDSPPartnerRelationshipRow CurrentRow;
            string PartnerShortName;
            TPartnerClass PartnerClass;

            if (AValidSelection)
            {
                // display "other" partner name and class in grid
                if ((APartnerKey != ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
                    && (APartnerKey != 0))
                {
                    CurrentRow = GetSelectedDetailRow();

                    if (CurrentRow.PartnerKey != APartnerKey)
                    {
                        CurrentRow.PartnerKey = APartnerKey;
                        FPartnerEditUIConnector.GetPartnerShortName(APartnerKey, out PartnerShortName, out PartnerClass);
                        CurrentRow.PartnerShortName = PartnerShortName;
                        CurrentRow.PartnerClass = PartnerClass.ToString();
                    }
                }
            }
        }

        void txtPPartnerRelationshipRelationKey_ValueChanged(long APartnerKey, string APartnerShortName, bool AValidSelection)
        {
            PartnerEditTDSPPartnerRelationshipRow CurrentRow;
            string PartnerShortName;
            TPartnerClass PartnerClass;

            if (AValidSelection)
            {
                // display "other" partner name and class in grid
                if ((APartnerKey != ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
                    && (APartnerKey != 0))
                {
                    CurrentRow = GetSelectedDetailRow();

                    if (CurrentRow.RelationKey != APartnerKey)
                    {
                        CurrentRow.RelationKey = APartnerKey;
                        FPartnerEditUIConnector.GetPartnerShortName(APartnerKey, out PartnerShortName, out PartnerClass);
                        CurrentRow.PartnerShortName = PartnerShortName;
                        CurrentRow.PartnerClass = PartnerClass.ToString();
                    }
                }
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

            if (CreateNewPPartnerRelationship())
            {
                cmbPPartnerRelationshipRelationName.Focus();
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
        private void NewRowManual(ref PartnerEditTDSPPartnerRelationshipRow ARow)
        {
            // Initialize relation with key of this partner on both sides, needs to be changed by user
            ARow.PartnerKey = ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey;
            ARow.RelationName = "";
            ARow.RelationKey = ARow.PartnerKey;
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PartnerEditTDSPPartnerRelationshipRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
            ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Relationship record: '{0}'?"),
                ARowToDelete.RelationName);
            return true;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PartnerEditTDSPPartnerRelationshipRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// button was pressed to edit other partner in relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditOtherPartner(System.Object sender, EventArgs e)
        {
            long RelationPartnerKey;

            if (GetSelectedDetailRow() == null)
            {
                return;
            }

            // depending on the relation select other partner to be edited
            if (GetSelectedDetailRow().PartnerKey == ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey)
            {
                RelationPartnerKey = GetSelectedDetailRow().RelationKey;
            }
            else
            {
                RelationPartnerKey = GetSelectedDetailRow().PartnerKey;
            }

            if (RelationPartnerKey == 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            try
            {
                TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                frm.SetParameters(TScreenMode.smEdit, RelationPartnerKey);
                frm.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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

        private void ValidateDataDetailsManual(PPartnerRelationshipRow ARow)
        {
            bool NewPartner = (FMainDS.PPartner.Rows[0].RowState == DataRowState.Added);

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateRelationshipManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, NewPartner, ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey);
        }

        #endregion
    }
}