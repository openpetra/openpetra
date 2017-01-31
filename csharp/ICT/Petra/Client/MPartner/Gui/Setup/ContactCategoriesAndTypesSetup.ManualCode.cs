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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmContactCategoriesAndTypesSetup
    {
        bool FDataSavedInNoMasterDataToSaveEvent = false;

        bool FDataSavingInUserControlRequiredFirst = false;

        System.Windows.Forms.Timer ShowMessageBoxTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Index of Row that is to be deleted.
        /// </summary>
        int FIndexOfDeletedRow = -1;

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

            grdDetails.Selection.FocusRowLeaving += HandleFocusRowLeaving;

            // We capture the Leave event of the Code TextBox (This is more consistent than LostFocus. - it always occurs
            // before validation, whereas LostFocus occurs before or after depending on mouse or keyboard.)
            txtDetailCategoryCode.Leave += new EventHandler(txtDetailCategoryCode_Leave);

            // Set up Timer that is needed for showing MessageBoxes from a Grid Event
            ShowMessageBoxTimer.Tick += new EventHandler(ShowTimerDrivenMessageBox);
            ShowMessageBoxTimer.Interval = 100;

            /* fix tab order */
            pnlButtons.TabIndex = grdDetails.TabIndex + 1;
        }

        private void RunOnceOnActivationManual()
        {
            // Hide 'Index' Grid Column - it is only used for debugging
            grdDetails.Columns.HideColumn(3);

            // Set up the correct filter for the bottom grid, based on our initial contact attribute
            if (FMainDS.PPartnerAttributeCategory.Rows.Count > 0)
            {
                ucoValues.SetCategoryCode(txtDetailCategoryCode.Text);
            }

            SelectRowInGrid(1);
        }

        private void NewRecord(System.Object sender, EventArgs e)
        {
            if (FDataSavingInUserControlRequiredFirst)
            {
                MessageBox.Show(Catalog.GetString(
                        "You need to press 'Save' first before you can create a new Category."),
                    Catalog.GetString("Saving of Data Required"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            if (CreateNewPPartnerAttributeCategory())
            {
                // Create the required initial detail attribute.
                ucoValues.CreateFirstContactType(txtDetailCategoryCode.Text, FMainDS.PPartnerAttributeCategory);

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
            FIndexOfDeletedRow = grdDetails.DataSourceRowToIndex2(ARowToDelete);

            return true;
        }

        private void DeleteRecord(object sender, EventArgs e)
        {
            if (ucoValues.Count > 0)
            {
                string msg = string.Empty;

                if (ucoValues.GridCount == 0)
                {
                    // The grid must be filtered because it has no rows!
                    msg += Catalog.GetString(
                        "The selected Contact Category has Contact Types associated with it, but they are being filtered out of the display.");
                    msg += "  ";
                }

                msg += Catalog.GetString("You must delete all the Contact Types for the selected Contact Category before you can delete this record.");

                MessageBox.Show(msg, MCommon.MCommonResourcestrings.StrRecordDeletionTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (FDataSavingInUserControlRequiredFirst)
            {
                MessageBox.Show(Catalog.GetString(
                        "You need to press 'Save' first before deleting the Category."),
                    Catalog.GetString("Saving of Data Required"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            DeletePPartnerAttributeCategory();
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PPartnerAttributeCategoryRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                // If the Row that got selected for deletion wasn't the last Row then we need to adjust the
                // Index of the following Rows
                if (FIndexOfDeletedRow != grdDetails.Rows.Count - 1)
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

            // Need to do the enabling/disabling of the Delete button manually as no auto-generated code
            // gets created since we have our own Delete handling ('DeleteRecord' Method in ManualCode file)
            btnDelete.Enabled = pnlDetails.Enabled && chkDetailDeletable.Checked;

            FIndexedGridRowsHelper.UpdateButtons(GetSelectedRowIndex(), FPetraUtilsObject.SecurityReadOnly);
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

            if (FPreviouslySelectedDetailRow != null)
            {
                // Trigger a Leave Event on the Category Code in case we have a new Category.
                // This is needed to ensure that any change in the Category Code is for sure
                // passed on the ucoValues UserControl - as the Leave Event doesn't fire if
                // the user pressed the 'Save' button!
                if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                    && (txtDetailCategoryCode.Focused))
                {
                    txtDetailCategoryCode_Leave(this, null);
                }
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
            if (e.Success)
            {
                if (!FDataSavedInNoMasterDataToSaveEvent)
                {
                    FPetraUtilsObject.SetChangedFlag();

                    ucoValues.SaveChanges(out ChildDTWhoseDataGotSaved);

                    FPetraUtilsObject.DisableSaveButton();
                }

                TSharedDataCache.TMPartner.MarkPhonePartnerAttributesConcatStrNeedsRefreshing();
                TSharedDataCache.TMPartner.MarkEmailPartnerAttributesConcatStrNeedsRefreshing();
            }

            // Ensure Filter functionality is enabled (might have been disabled in Method 'Uco_NoMoreDetailRecords')
            ActionEnabledEvent(null, new ActionEventArgs("cndFindFilterAvailable", true));
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

                // Need to disable Filter functionality to prevent user from potentially changing to different Rows
                // as that could happen if the user would apply a Filter!
                ActionEnabledEvent(null, new ActionEventArgs("cndFindFilterAvailable", false));

                // Need to set Focus to btnNew as otherwise the 'Filter' button of the UserControl gets the Focus, which isn't helpful
                btnNew.Focus();
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

        private void HandleFocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
        {
            if (FDataSavingInUserControlRequiredFirst)
            {
                DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(e.ProposedRow);
                var SelectedDR = rowView.Row as PPartnerAttributeCategoryRow;

                // This check is needed in case one of the promote/demote Buttons have got clicked: in that case
                // this Event will be fired, but the Row hasn't changed
                if (SelectedDR.CategoryCode != txtDetailCategoryCode.Text)
                {
                    ShowMessageBoxTimer.Start();

                    e.Cancel = true;
                }
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
                        "You need to press 'Save' first before you can change to a different Category."),
                    Catalog.GetString("Saving of Data Required"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            PPartnerAttributeTypeTable typeTable = (PPartnerAttributeTypeTable)TDataCache.TMPartner.
                                                   GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ContactTypeList);
            DataView typeDataView = new DataView(typeTable);

            typeDataView.Sort = PPartnerAttributeTypeTable.GetCategoryCodeDBName();

            TFormDataKeyDescriptionList recordList = new TFormDataKeyDescriptionList();
            recordList.Title = "Contact Categories and Types";
            recordList.KeyTitle = Catalog.GetString("Contact Category");
            recordList.DescriptionTitle = Catalog.GetString("Contact Type");
            recordList.Field3Title = Catalog.GetString("Description");

            PPartnerAttributeTypeRow typeRow;

            foreach (DataRowView typeRowView in typeDataView)
            {
                TFormDataKeyDescription record = new TFormDataKeyDescription();

                typeRow = (PPartnerAttributeTypeRow)typeRowView.Row;
                record.Key = typeRow.CategoryCode;
                record.Description = typeRow.AttributeType;
                record.Field3 = typeRow.Description;
                recordList.Add(record);
            }

            TStandardFormPrint.PrintRecordList(recordList, 3, APrintApplication, typeDataView.Count, typeDataView, "", APreviewMode);
        }
    }
}