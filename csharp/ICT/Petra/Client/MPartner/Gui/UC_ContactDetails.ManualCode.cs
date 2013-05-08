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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ContactDetails
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private Boolean FPartnerAttributesExist;
        
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
        public void PostInitUserControl(PartnerEditTDS AMainDS)
        {
            FMainDS = AMainDS;
            
            // disable change event while controls are being initialized as otherwise save button might get enabled
            FPetraUtilsObject.DisableDataChangedEvent();

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpContactDetails));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            /* Check if data needs to be retrieved from the PetraServer */
            if (FMainDS.PPartnerAttribute == null)
            {
                FPartnerAttributesExist = LoadDataOnDemand();
            }

            FMainDS.Tables.Add(new PPartnerAttributeCategoryTable(PPartnerAttributeCategoryTable.GetTableName()));
            FMainDS.Tables.Add(new PPartnerAttributeTypeTable(PPartnerAttributeTypeTable.GetTableName()));
            FMainDS.InitVars();
            FMainDS.Merge((PPartnerAttributeCategoryTable) TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.PartnerAttributeCategoryList, PPartnerAttributeCategoryTable.GetTableName()));
            FMainDS.Merge((PPartnerAttributeTypeTable) TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.PartnerAttributeTypeList, PPartnerAttributeTypeTable.GetTableName()));
            
            FPartnerAttributesExist = FMainDS.PPartnerAttribute.Rows.Count > 0;
            
            // Need to enable Relations as the PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable have not been part of the PartnerEditTDS when transferred from the OpenPetra Server!
            // These Relations are needed in Method 'CreateCustomDataColumns'.
            FMainDS.EnableRelation("ContactDetails1");
            FMainDS.EnableRelation("ContactDetails2");
            
            // Create custom data columns on-the-fly
            CreateCustomDataColumns();
            
            
            /* Create SourceDataGrid columns */
            CreateGridColumns();

            /* Setup the DataGrid's visual appearance */
//            SetupDataGridVisualAppearance();

            /* Prepare the Demote and Promote buttons first time */
//            PrepareArrowButtons();

            /* Hook up event that fires when a different Row is selected */
//            grdDetails.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
            
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFamilyMembers));

            if (!FPartnerAttributesExist)
            {
                // TODO
//                /* If Family has no members, these buttons are disabled */
//                this.btnFamilyMemberDemote.Enabled = false;
//                this.btnFamilyMemberPromote.Enabled = false;
//                this.btnMovePersonToOtherFamily.Enabled = false;
//                this.btnEditPerson.Enabled = false;
//                this.btnEditFamilyID.Enabled = false;

                /* this.btnAddExistingPersonToThisFamily.enabled := false; */
                /* this.btnAddNewPersonThisFamily.enabled := false; */
            }

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
            }
            
            // now changes to controls can trigger enabling of save button again
            FPetraUtilsObject.EnableDataChangedEvent();

//            ApplySecurity();            // TODO
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

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // retrieve Contact Details (stored in PPartnerAttribute Table) from PetraServer
            // If the Partner has got no Contact Details: returns false
            try
            {
                // Make sure that Typed DataTable is already there at Client side
                if (FMainDS.PPartnerAttribute == null)
                {
                    FMainDS.Tables.Add(new PPartnerAttributeTable(PPartnerAttributeTable.GetTableName()));
                    FMainDS.InitVars();
                }

                FMainDS.PPartnerAttribute.Rows.Clear();
                FMainDS.Merge(FPartnerEditUIConnector.GetDataContactDetails());
                FMainDS.PPartnerAttribute.AcceptChanges();

                if (FMainDS.PPartnerAttribute.Rows.Count > 0)
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
                ReturnValue = false;
                return false;
            }
            catch (Exception)
            {
                ReturnValue = false;

                // raise;
            }

            return ReturnValue;
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
                cmbContactCategory.Focus();
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
            int LeastSequence = 0;
            int HighestIndex = 0;
            int ThisRow_Sequence = 0;
            int ThisRow_Index = 0;
            PPartnerAttributeTable ThisDT = (PPartnerAttributeTable)ARow.Table;
            
            ARow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
            ARow.AttributeType = "Phone";
            
            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++) 
            {
                ThisRow_Sequence = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetSequenceDBName(), DataRowVersion.Original]);
                if (ThisRow_Sequence < LeastSequence)
                {
                    LeastSequence = ThisRow_Sequence;
                }
            }
            
            ARow.Sequence = LeastSequence  - 1;
         
            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++) 
            {
                ThisRow_Index = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetIndexDBName(), DataRowVersion.Original]);
                
                if (ThisRow_Index > HighestIndex) 
                {
                    HighestIndex = ThisRow_Index;
                }
            }
            
            ARow.Index = HighestIndex + 1;            
            
            ARow.Value = "NEWVALUE" + ARow.Sequence.ToString();
            ARow.Primary = false;
            ARow.WithinOrgansiation = false;
            ARow.Specialised = false;
            ARow.Sensitive = false;
            ARow.Current = true;
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            // Before the auto-generated Method for deletion can be called we need to set the Expressions that refer to 'Parent.*' to "", 
            // otherwise we get a 'System.IndexOutOfRangeException: Cannot find relation 0.' Exception when the DataTable gets deserialised 
            // on the server side in the call to Method 'GetNonCacheableRecordReferenceCount'!
            foreach (DataColumn RemoveExpressionsColumn in FMainDS.PPartnerAttribute.Columns)
            {
                if (RemoveExpressionsColumn.Expression.Length != 0)
                {
                    // TLogging.Log(RemoveExpressionsColumn.Expression, [ToLogFile]);
                    RemoveExpressionsColumn.Expression = "";
                }
            }
            
            this.DeletePPartnerAttribute();
            
            // Restore Column Expressions again!
            SetColumnExpressions();
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
        private bool DeleteRowManual(PPartnerAttributeRow ARowToDelete, ref string ACompletionMessage)
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

        /// <summary>
        /// Creates custom DataColumns that will be shown in the Grid.
        /// </summary>
        /// <returns>void</returns>
        public void CreateCustomDataColumns()
        {
            DataColumn ForeignTableColumn;
            
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerAttributeTypeTable.GetCodeDBName();
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);
            
            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "ContactType";
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);
            
            SetColumnExpressions();
        }
        
        /// <summary>
        /// Sets the Column Expressions for the calculated DataColumns
        /// </summary>
        private void SetColumnExpressions()
        {
            FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetCodeDBName();
            
            FMainDS.PPartnerAttribute.Columns["ContactType"].Expression =
                "IIF(" + PPartnerAttributeTable.GetSpecialisedDBName() + " = true, Parent." + PPartnerAttributeTypeTable.GetSpecialLabelDBName() + ", Parent." + PPartnerAttributeTypeTable.GetCodeDBName() + ")";            
        }
        
        /// <summary>
        /// Creates DataBound columns for the Grid control.
        /// </summary>
        /// <returns>void</returns>
        public void CreateGridColumns()
        {
            // Get rid of the Columns as added per YAML file as we need to show calculated Columns!
            grdDetails.Columns.Clear();
// TODO           FDataGrid.AddImageColumn(@GetAddressKindIconForGridRow);

            //
            // Contact Type
            grdDetails.AddTextColumn("Type Code", FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()]);

            //
            // Contact Type
            grdDetails.AddTextColumn("Contact Type", FMainDS.PPartnerAttribute.Columns["ContactType"]);
            
            // Value
            grdDetails.AddTextColumn("Value", FMainDS.PPartnerAttribute.ColumnValue);

            // Comment
            grdDetails.AddTextColumn("Comments", FMainDS.PPartnerAttribute.ColumnComment);

            // Current
            grdDetails.AddCheckBoxColumn("Current", FMainDS.PPartnerAttribute.ColumnCurrent);

            // Confidential
            grdDetails.AddCheckBoxColumn("Confidential", FMainDS.PPartnerAttribute.ColumnSensitive);
            
            // Modification TimeStamp (for testing purposes only...)
            // grdDetails.AddTextColumn("Modification TimeStamp", FMainDS.PPartnerAttribute.ColumnModificationId);
        }
        
        private void ValidateDataDetailsManual(PPartnerAttributeRow ARow)
        {
            bool NewPartner = (FMainDS.PPartner.Rows[0].RowState == DataRowState.Added);

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

//            TSharedPartnerValidation_Partner.ValidateRelationshipManual(this, ARow, ref VerificationResultCollection,
//                FValidationControlsDict, NewPartner, ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey);
        }

        #endregion
        
        #region Event Handlers
        
        private void ContactTypePromote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Promotion of Contact Types not implemented yet!");
        }

        private void ContactTypeDemote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Demotion of Contact Types not implemented yet!");
        }

        private void LaunchHyperlinkPrefEMail(object sender, EventArgs e)
        {
            throw new NotImplementedException("Launching of E-Mail program not implemented yet!");
        }

        private void LaunchHyperlinkEMailWithinOrg(object sender, EventArgs e)
        {
            throw new NotImplementedException("Launching of E-Mail program not implemented yet!");
        }
        
        private void FilterCriteriaChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException("Filtering is not implemented yet!");
        }
        
        private void ValidChanged(object sender, EventArgs e)
        {
            // TODO
        }
        
        #endregion
    }
}