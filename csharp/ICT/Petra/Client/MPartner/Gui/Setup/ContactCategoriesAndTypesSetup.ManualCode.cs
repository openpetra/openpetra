//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK
//
// Copyright 2004-2014 by OM International
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
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactCategoriesAndTypesSetup
    {
		bool FDataSavedInNoMasterDataToSaveEvent = false;

		bool FDataSavingInUserControlRequiredFirst = false;
		
		// Instance of a 'Helper Class' for handling the Indexes of the DataRows. (The Grid is sorted by the Index.)
		TSgrdDataGrid.IndexedGridRowsHelper FIndexedGridRowsHelper;
		
        private void InitializeManualCode()
        {            
            // Initialize 'Helper Class' for handling the Indexes of the DataRows.
            FIndexedGridRowsHelper = new TSgrdDataGrid.IndexedGridRowsHelper(
                grdDetails, PPartnerAttributeCategoryTable.ColumnIndexId, btnDemoteCategory, btnPromoteCategory,
                delegate { FPetraUtilsObject.SetChangedFlag(); });
            
            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(DataSavingStarted);
            // We need to capture the 'DataSaved' event so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(DataSaved);
            // We need to capture the 'NoMasterDataToSave' event so we can save our Extra DataSet 
            // in case that no data was changed in this Forms' pnlDetails
            FPetraUtilsObject.NoMasterDataToSave += NoMasterDataToSave;

            // We also want to know if the UserControl holds no more detail records            
            ucoValues.NoMoreDetailRecords += Uco_NoMoreDetailRecords;
            
            ucoValues.PetraUtilsObject = FPetraUtilsObject;
            
            // We capture the Leave event of the Code TextBox (This is more consistent than LostFocus. - it always occurs 
            // before validation, whereas LostFocus occurs before or after depending on mouse or keyboard.)
            txtDetailCategoryCode.Leave += new EventHandler(txtDetailCategoryCode_Leave);
        }
        
        private void RunOnceOnActivationManual()
        {
            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PPartnerAttributeCategory.Rows.Count > 0)
            {
                ucoValues.SetCategoryCode(txtDetailCategoryCode.Text);
            }

            SelectRowInGrid(1);            
        }
        
        private void NewRecord(System.Object sender, EventArgs e)
        {
            if(CreateNewPPartnerAttributeCategory())
            {
                // Create the required initial detail attribute.
                ucoValues.CreateFirstContactType(txtDetailCategoryCode.Text);            
                
                ucoValues.Enabled = true;
                txtDetailCategoryCode.ReadOnly = false;
                
                txtDetailCategoryCode.Focus();
            }
        }

        private void NewRowManual(ref PPartnerAttributeCategoryRow ARow)
        {
            string NewName = Catalog.GetString("NEWCATEGORY");
            Int32 CountNewDetail = 0;

            if (FMainDS.PPartnerAttributeCategory.Rows.Find(new object[] { NewName }) != null)
            {
                while (FMainDS.PPartnerAttributeCategory.Rows.Find(new object[] { NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.CategoryCode = NewName;
            ARow.PartnerContactCategory = true;
            ARow.Deletable = true;  // all manually created Contact Categories are deletable                                 

            // Determine and set the 'Index' (ARow.Index in this case) of the new Row
            FIndexedGridRowsHelper.DetermineIndexForNewRow(ARow);
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PPartnerAttributeCategoryRow ARowToDelete, ref string ADeletionQuestion)
        {
            DataView DependentRecordsDV;
                
//            if (!FDataSavingInUserControlRequiredFirst) 
//            {
                DependentRecordsDV = new DataView(FMainDS.PPartnerAttributeType);

                DependentRecordsDV.RowStateFilter = DataViewRowState.CurrentRows;
                DependentRecordsDV.RowFilter = String.Format("{0} = '{1}'",
                    PPartnerAttributeTypeTable.GetAttributeCategoryDBName(),
                    ARowToDelete.CategoryCode);
    
                if (DependentRecordsDV.Count > 0)
                {
                    // Tell the user that we cannot allow deletion if any rows exist in the DataView
                    TMessages.MsgRecordCannotBeDeletedDueToDependantRecordsError(
                        "Contact Category", "a Contact Category", "Contact Categories", "Contact Type", "a Contact Type", 
                        "Contact Types", ARowToDelete.CategoryCode, DependentRecordsDV.Count);
                        
                    return false;
                }

//            }
//            else
//            {
//              TODO We are too late here as the cascading delete check runs first - fix by changing TDeleteGridRows.DeleteRows to allow for an Action<T> to be optionally called instead of showing the standard MessageBox
//                   - once current trunk has been merged into this Branch!
//                MessageBox.Show(
//                    Catalog.GetString(
//                        "You need to press 'Save' first before deleting the Category (to avoid getting a warning about Contact Types that still depend on that Category)."),
//                        Catalog.GetString("Deletion of Category: Information"), 
//                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//                
//                return false;        
//            }

            return true;
        }
        
        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PPartnerAttributeCategoryRow ARowToDelete, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
        {         
            if (ADeletionPerformed) 
            {
                // TODO Check that the condition-check below works if A) the last row in the grid gets deleted; B) if the row that comes before the last row in the grid gets deleted
                // - once current trunk has been merged into this Branch!                
                int IndexOfDeletedRow = grdDetails.DataSourceRowToIndex2(ARowToDelete);
                
                // If the Row that got selected for deletion wasn't the last Row then we need to adjust the
                // Index of the following Rows
                if (IndexOfDeletedRow != grdDetails.Rows.Count - 2)
                {
                    FIndexedGridRowsHelper.AdjustIndexesOfFollowingRows(GetSelectedRowIndex(), false);
                }
            }            
        }
        
        private void ShowDetailsManual(PPartnerAttributeCategoryRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucoValues.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucoValues.Enabled = true;

                // Pass the category code to the user control - it will then update itself
                ucoValues.SetCategoryCode(txtDetailCategoryCode.Text);                                
            }
            
            FIndexedGridRowsHelper.UpdateButtons(GetSelectedRowIndex());
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeCategoryRow ARow)
        {
            // Tell the user control to get its data, too            
            ucoValues.GetDetailsFromControls();
        }
                
        private void txtDetailCategoryCode_Leave(object sender, EventArgs e)
        {
            string NewCode = txtDetailCategoryCode.Text;

            if (NewCode.CompareTo(ucoValues.ContactCategory) == 0)
            {
                return;                                                                 // same as before
            }

            // The user has edited the Category Code and we have some Contact Types that depended on it!
            // We have to update the Contact Types provided the new Category Code is good (and passed validation)
            if (NewCode == String.Empty)
            {
                return;
            }
           
            // So it is safe to modify the Contact Types
            ucoValues.ModifyCategoryCode(NewCode);
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
            TTypedDataTable ChildDTWhoseDataGotSaved;

            FDataSavedInNoMasterDataToSaveEvent = false;            
            
            // Trigger a Leave Event on the Category Code in case we have a new Category.
            // This is needed to ensure that any change in the Category Code is for sure
            // passed on the ucoValues UserControl - as the Leave Event doesn't fire if
            // the user pressed the 'Save' button!
            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                && (txtDetailCategoryCode.Focused))
            {
                txtDetailCategoryCode_Leave(this, null);                
            }
            
            if (FDataSavingInUserControlRequiredFirst) 
            {
                ucoValues.SaveChanges(out ChildDTWhoseDataGotSaved);
                
                FDataSavingInUserControlRequiredFirst = false;
                FPetraUtilsObject.HasChanges = true;
            }
        }
        
        private void DataSaved(object Sender, TDataSavedEventArgs e)
        {
            TTypedDataTable ChildDTWhoseDataGotSaved;
            
            // Save the changes in the user control
            if ((e.Success)
                && (!FDataSavedInNoMasterDataToSaveEvent))
            {
                FPetraUtilsObject.SetChangedFlag();
            
                ucoValues.SaveChanges(out ChildDTWhoseDataGotSaved);
                                
                FPetraUtilsObject.DisableSaveButton();                                    
            }
        }
        
		private void NoMasterDataToSave(object Sender, Ict.Common.TNoMasterDataToSaveEventArgs e)
		{
		    TTypedDataTable ChildDTWhoseDataGotSaved;
		    bool UCSaveResult;
		    
            // Save the changes in the user control
            UCSaveResult = ucoValues.SaveChanges(out ChildDTWhoseDataGotSaved);

			e.SubmitChangesResult = UCSaveResult ? TSubmitChangesResult.scrOK : TSubmitChangesResult.scrError;            
            e.ChildDataTableWhoseDataGotSaved = ChildDTWhoseDataGotSaved;
		    
            FDataSavedInNoMasterDataToSaveEvent = true;
		}

        /// <summary>
        /// Raised when the Values UserControl holds no more detail records after all detail records
		/// have been deleted
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
		private void Uco_NoMoreDetailRecords(object sender, EventArgs e)
		{
			TVerificationResultCollection VerificationResults = null;
            int RefCountLimit = FPetraUtilsObject.MaxReferenceCountOnDelete;
            
            this.Cursor = Cursors.WaitCursor;
            
            TRemote.MPartner.ReferenceCount.WebConnectors.GetCacheableRecordReferenceCount(
                "ContactCategoryList",
                DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow),
                RefCountLimit,
                out VerificationResults);
            this.Cursor = Cursors.Default;

            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {            
//                MessageBox.Show(
//                    Catalog.GetString(String.Format(
//                        "In case you want to delete the Category '{0}' as well then you need to press 'Save' first before deleting the Category (to avoid getting a warning about Contact Types that still depend on that Category)",
//                        txtDetailCategoryCode.Text)),
//                        Catalog.GetString("Deletion of Category: Information"), 
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                FDataSavingInUserControlRequiredFirst = true;
            }
		}
		
        private void ContactCategoryPromote(System.Object sender, System.EventArgs e)
        {
            FIndexedGridRowsHelper.PromoteRow();
        }

        private void ContactCategoryDemote(System.Object sender, System.EventArgs e)
        {
            FIndexedGridRowsHelper.DemoteRow();
        }		
    }
}