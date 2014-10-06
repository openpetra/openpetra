//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactAttributeSetup
    {
        // A local variable that saves the column ordinal for our additional column in the main data table
        private int NumDetailCodesColumnOrdinal = 0;

		bool FDataSavedInNoMasterDataToSaveEvent = false;
		
		bool FDataSavingInUserControlRequiredFirst = false;
		
        private void InitializeManualCode()
        {
            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(DataSavingStarted);            
            // We need to capture the 'DataSaved' event so we can save our Extra DataSet
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(DataSaved);
            // We need to capture the 'NoMasterDataToSave' event so we can save our Extra DataSet 
            // in case that no data was changed in this Forms' pnlDetails
            FPetraUtilsObject.NoMasterDataToSave += NoMasterDataToSave;
            
            // We also want to know if the UserControl holds no more detail records
            ucoContactDetail.NoMoreDetailRecords += Uco_NoMoreDetailRecords;
            // ...and if the number of rows in our UserControl changes
            ucoContactDetail.CountChanged += new CountChangedEventHandler(Uco_ContactDetail_CountChanged);

            ucoContactDetail.PetraUtilsObject = FPetraUtilsObject;
            
            // We capture the Leave event of the Code TextBox (This is more consistent than LostFocus. - it always occurs 
            // before validation, whereas LostFocus occurs before or after depending on mouse or keyboard.)
            txtDetailContactAttributeCode.Leave += new EventHandler(txtDetailContactAttributeCode_Leave);
        }

        private void RunOnceOnActivationManual()
        {
            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PContactAttribute.Rows.Count > 0)
            {
                ucoContactDetail.SetContactAttribute(txtDetailContactAttributeCode.Text);
            }

            // Add an extra column to our main data set that contains the number of sub-details for a given code
            NumDetailCodesColumnOrdinal = FMainDS.PContactAttribute.Columns.Add("NumDetails", typeof(int)).Ordinal;

            for (int i = 0; i < FMainDS.PContactAttribute.Rows.Count; i++)
            {
                string code = FMainDS.PContactAttribute.Rows[i][FMainDS.PContactAttribute.ColumnContactAttributeCode.Ordinal].ToString();
                FMainDS.PContactAttribute.Rows[i][NumDetailCodesColumnOrdinal] = ucoContactDetail.NumberOfDetails(code);
            }

            // add a column to the grid and bind it to our new data set column
            grdDetails.AddTextColumn(Catalog.GetString("Number of Detail Codes"), FMainDS.PContactAttribute.Columns[NumDetailCodesColumnOrdinal]);

            // After the above modifications we need to make the DataTable 'unchanged' again so the screen doesn't start with all its DetailTables' DataRows modified!
            FMainDS.PContactAttribute.AcceptChanges();
            
            SelectRowInGrid(1);
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            if(CreateNewPContactAttribute())
            {
                // Create the required initial detail attribute. This will automatically fire the event that updates our details count column
                ucoContactDetail.CreateFirstAttributeDetail(txtDetailContactAttributeCode.Text);
                
                ucoContactDetail.Enabled = true;
                txtDetailContactAttributeCode.ReadOnly = false;
                    
                txtDetailContactAttributeCode.Focus();
            }
        }

        private void NewRowManual(ref PContactAttributeRow ARow)
        {
            string NewName = Catalog.GetString("NEWATTRIBUTE");
            Int32 CountNewDetail = 0;

            if (FMainDS.PContactAttribute.Rows.Find(new object[] { NewName }) != null)
            {
                while (FMainDS.PContactAttribute.Rows.Find(new object[] { NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.ContactAttributeCode = NewName;
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PContactAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            DataView DependentRecordsDV;
                
//            if (!FDataSavingInUserControlRequiredFirst) 
//            {
                DependentRecordsDV = new DataView(FMainDS.PContactAttribute);

                DependentRecordsDV.RowStateFilter = DataViewRowState.CurrentRows;
                DependentRecordsDV.RowFilter = String.Format("{0} = '{1}'",
                    PContactAttributeTable.GetContactAttributeCodeDBName(),
                    ARowToDelete.ContactAttributeCode);
    
                if (DependentRecordsDV.Count > 0)
                {
                    // Tell the user that we cannot allow deletion if any rows exist in the DataView
                    TMessages.MsgRecordCannotBeDeletedDueToDependantRecordsError(
                        "Contact Attribute", "a Contact Attribute", "Contact Attributes", "Contact Detail", "a Contact Detail", 
                        "Contact Details", ARowToDelete.ContactAttributeCode, DependentRecordsDV.Count);
                        
                    return false;
                }

//            }
//            else
//            {
//              TODO We are too late here as the cascading delete check runs first - fix by changing TDeleteGridRows.DeleteRows to allow for an Action<T> to be optionally called instead of showing the standard MessageBox!
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

        private void ShowDetailsManual(PContactAttributeRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucoContactDetail.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucoContactDetail.Enabled = true;

                // Pass the contact attribute to the user control - it will then update itself
                ucoContactDetail.SetContactAttribute(ARow.ContactAttributeCode);
            }
        }

        private void GetDetailDataFromControlsManual(PContactAttributeRow ARow)
        {
            // Tell the user control to get its data, too
            ucoContactDetail.GetDetailsFromControls();
        }

        private void txtDetailContactAttributeCode_Leave(object sender, EventArgs e)
        {
            if (NumDetailCodesColumnOrdinal == 0)
            {
                return;                                                 // No problem if we have no details yet
            }

            string NewCode = txtDetailContactAttributeCode.Text;

            if (NewCode.CompareTo(ucoContactDetail.ContactAttribute) == 0)
            {
                return;                                                                 // same as before
            }

            // The user has edited the Attribute Code and we have some Detail Codes that depended on it!
            // We have to update the Detail Codes provided the new Detail Code is good (and passed validation)
            if (NewCode == String.Empty)
            {
                return;
            }

            // So it is safe to modify the Detail Codes 
            ucoContactDetail.ModifyAttributeCode(NewCode);
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
            
            // Trigger a Leave Event on the Attribute Code in case we have a new Attribute.
            // This is needed to ensure that any change in the Attribute Code is for sure
            // passed on the ucoContactDetail UserControl - as the Leave Event doesn't fire if
            // the user pressed the 'Save' button!
            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                && (txtDetailContactAttributeCode.Focused))
            {
                txtDetailContactAttributeCode_Leave(this, null);                
            }
            
            if (FDataSavingInUserControlRequiredFirst) 
            {
                ucoContactDetail.SaveChanges(out ChildDTWhoseDataGotSaved);
                
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
            
                ucoContactDetail.SaveChanges(out ChildDTWhoseDataGotSaved);
                
                FPetraUtilsObject.DisableSaveButton();                    
            }
        }

		private void NoMasterDataToSave(object Sender, Ict.Common.TNoMasterDataToSaveEventArgs e)
		{
		    TTypedDataTable ChildDTWhoseDataGotSaved;
            bool UCSaveResult;		
            
            // Save the changes in the user control
            UCSaveResult = ucoContactDetail.SaveChanges(out ChildDTWhoseDataGotSaved);
            
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
                "ContactAttributeList",
                DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow),
                RefCountLimit,
                out VerificationResults);
            this.Cursor = Cursors.Default;

            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {            
//                MessageBox.Show(
//                    Catalog.GetString(String.Format(
//                        "In case you want to delete the Attribute '{0}' as well then you need to press 'Save' first before deleting the Attribute (to avoid getting a warning about Details that still depend on that Attribute)",
//                        txtDetailContactAttributeCode.Text)),
//                        Catalog.GetString("Deletion of Attribute: Information"), 
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                FDataSavingInUserControlRequiredFirst = true;
            }
		}
		
        private void Uco_ContactDetail_CountChanged(object Sender, CountEventArgs e)
        {
            // something has changed in our user control (add/delete rows)
            FPreviouslySelectedDetailRow[NumDetailCodesColumnOrdinal] = e.NewCount;
        }
    }
}