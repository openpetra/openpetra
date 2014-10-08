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
using Ict.Petra.Shared.MPartner.Mailroom.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactAttributeSetup
    {
		bool FDataSavedInNoMasterDataToSaveEvent = false;
		
		bool FDataSavingInUserControlRequiredFirst = false;
		
        System.Windows.Forms.Timer ShowMessageBoxTimer = new System.Windows.Forms.Timer();
		
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

            ucoContactDetail.PetraUtilsObject = FPetraUtilsObject;
            
            grdDetails.Selection.FocusRowLeaving += HandleFocusRowLeaving;
            
            // We capture the Leave event of the Code TextBox (This is more consistent than LostFocus. - it always occurs 
            // before validation, whereas LostFocus occurs before or after depending on mouse or keyboard.)
            txtDetailContactAttributeCode.Leave += new EventHandler(txtDetailContactAttributeCode_Leave);
            
            // Set up Timer that is needed for showing MessageBoxes from a Grid Event
            ShowMessageBoxTimer.Tick += new EventHandler(ShowTimerDrivenMessageBox);
            ShowMessageBoxTimer.Interval = 100;            
        }

        private void RunOnceOnActivationManual()
        {
            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PContactAttribute.Rows.Count > 0)
            {
                ucoContactDetail.SetContactAttribute(txtDetailContactAttributeCode.Text);
            }
            
            SelectRowInGrid(1);
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            if (FDataSavingInUserControlRequiredFirst)
            {
                MessageBox.Show(Catalog.GetString(
                    "You need to press 'Save' first before you can create a new Contact Attribute."), 
                    Catalog.GetString("Saving of Data Required"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                return;
            }
                        
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

        private void DeleteRecord(object sender, EventArgs e)
        {
            if (ucoContactDetail.Count > 0)
            {
                string msg = string.Empty;

                if (ucoContactDetail.GridCount == 0)
                {
                    // The grid must be filtered because it has no rows!
                    msg += Catalog.GetString(
                        "The selected Contact Attribute has Contact Details associated with it, but they are being filtered out of the display.");
                    msg += "  ";
                }

                msg += Catalog.GetString("You must delete all the Contact Details for the selected Contact Attribute before you can delete this record.");

                MessageBox.Show(msg, MCommon.MCommonResourcestrings.StrRecordDeletionTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return;
            }

            if (FDataSavingInUserControlRequiredFirst)
            {
                MessageBox.Show(Catalog.GetString(
                    "You need to press 'Save' first before deleting the Contact Attribute."), 
                    Catalog.GetString("Saving of Data Required"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                return;
            }
            
            DeletePContactAttribute();
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
            
            if (FPreviouslySelectedDetailRow != null) 
            {
                // Trigger a Leave Event on the Attribute Code in case we have a new Attribute.
                // This is needed to ensure that any change in the Attribute Code is for sure
                // passed on the ucoContactDetail UserControl - as the Leave Event doesn't fire if
                // the user pressed the 'Save' button!
                if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                    && (txtDetailContactAttributeCode.Focused))
                {
                    txtDetailContactAttributeCode_Leave(this, null);                
                }                
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
		/// have been deleted.
		/// <para>
		/// We need to check in this Event Hanlder whether the deleted records exist in the DB; if so we  
		/// need to prevent a few actions that the user could take to ensure that the UserControls' data 
		/// gets saved first (signalised by the FDataSavingInUserControlRequiredFirst flag). This is necessary  
		/// so that the deleted Rows are no longer in the DB. That in turn is necessary for the user to be able  
		/// to delete the Category as well, as otherwise the reference count on the Category would not be null 
		/// and a deletion of the Category could not go ahead.
		/// </para>
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
		private void Uco_NoMoreDetailRecords(object sender, EventArgs e)
		{
			TVerificationResultCollection VerificationResults = null;

			GetReferenceCount(GetSelectedDataRow(), FPetraUtilsObject.MaxReferenceCountOnDelete, out VerificationResults);
			
            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {            
                FDataSavingInUserControlRequiredFirst = true;
                
                // Need to disable Filter Button to prevent user from potentially changing to different Rows 
                // as that could happen if the user would apply a Filter!
                chkToggleFilter.Enabled = false;                
            }
		}
		
		private void HandleFocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
		{
		    if (FDataSavingInUserControlRequiredFirst)
            {
                ShowMessageBoxTimer.Start();
                
                e.Cancel = true;                                    
            }		    
		}
		
		/// <summary>
		/// Called from a timer, ShowMessageBoxTimer, so that the FocusRowLeaving Event processing can
        /// complete before the MessageBox is show (would the MessageBox be shown while the Event gets
        /// processed the Grid would get into a strange state in which mouse moves would cause the Grid
        /// to scroll!).
		/// </summary>
		/// <param name="Sender">Gets evaluated to make sure a Timer is calling this Method.</param>
		/// <param name="e">Ignored.</param>
		private void ShowTimerDrivenMessageBox(Object Sender, EventArgs e)        
        {            
            System.Windows.Forms.Timer SendingTimer = Sender as System.Windows.Forms.Timer;
            
            if (SendingTimer != null)
            {
                // I got called from a Timer: stop that now so that the following MessageBox gets shown only once!
                SendingTimer.Stop();
                
                MessageBox.Show(
                    Catalog.GetString(
                        "You need to press 'Save' first before you can change to a different Contact Attribute."),
                        Catalog.GetString("Saving of Data Required"), 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
		}				
    }
}