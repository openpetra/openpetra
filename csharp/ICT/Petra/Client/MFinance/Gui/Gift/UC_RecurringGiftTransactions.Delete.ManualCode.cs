//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
#region usings

using System;
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftTransactions
    {
        #region delete all gifts in batch

        /// <summary>
        /// Delete data from current recurring gift batch
        /// </summary>
        /// <param name="ABatchNumber"></param>
        public void DeleteRecurringBatchGiftData(Int32 ABatchNumber)
        {
            DataView RecurringGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);

            RecurringGiftDetailView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            RecurringGiftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                ARecurringGiftDetailTable.GetDetailNumberDBName());

            foreach (DataRowView dr in RecurringGiftDetailView)
            {
                ARecurringGiftDetailRow gdr = (ARecurringGiftDetailRow)dr.Row;
                dr.Delete();
            }

            DataView RecurringGiftView = new DataView(FMainDS.ARecurringGift);

            RecurringGiftView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            RecurringGiftView.Sort = String.Format("{0} DESC",
                ARecurringGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in RecurringGiftView)
            {
                dr.Delete();
            }
        }

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
            TFrmRecurringGiftBatch FMyForm = (TFrmRecurringGiftBatch) this.ParentForm;

            string CompletionMessage = string.Empty;
            int BatchNumberToClear = FBatchNumber;

            if ((FPreviouslySelectedDetailRow == null))
            {
                return;
            }
            else if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
            {
                MessageBox.Show(Catalog.GetString("Please remove the filter before attempting to delete all Recurring Gifts in this batch."),
                    Catalog.GetString("Delete All Recurring Gifts"));

                return;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupDS = (GiftBatchTDS)FMainDS.Copy();
            BackupDS.Merge(FMainDS);

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have chosen to delete all Gifts from Recurring Batch: {0}.{1}{1}Are you sure you want to delete all?"),
                        BatchNumberToClear,
                        Environment.NewLine),
                    Catalog.GetString("Confirm Delete All"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Specify current action
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.DELETINGTRANS;

                //clear any transactions currently being editied in the Transaction Tab
                ClearCurrentSelection(0, false);

                //Now delete all Recurring Gift data for current batch
                DeleteRecurringBatchGiftData(BatchNumberToClear);

                FBatchRow.BatchTotal = 0;
                txtBatchTotal.NumberValueDecimal = 0;

                // Be sure to set the last Recurring Gift number in the parent table before saving all the changes
                FBatchRow.LastGiftNumber = 0;

                FPetraUtilsObject.SetChangedFlag();

                // save changes
                if (((TFrmRecurringGiftBatch)ParentForm).SaveChangesManual())
                {
                    CompletionMessage = Catalog.GetString("All Recurring Gifts and their details deleted successfully.");

                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("Recurring Gifts Deletion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    CompletionMessage = Catalog.GetString("All Recurring Gifts and their details have been deleted but saving the changes failed!");

                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("All Gifts Deletion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                //Revert to previous state
                RevertDataSet(FMainDS, BackupDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.NONE;
                this.Cursor = Cursors.Default;
            }

            SetGiftDetailDefaultView();
            FFilterAndFindObject.ApplyFilter();

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                UpdateControlsProtection();
            }

            UpdateRecordNumberDisplay();
        }

        #endregion delete all gifts in batch

        #region delete gift

        private bool OnPreDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool AllowDeletion = true;

            FGift = GetRecurringGiftRow(ARowToDelete.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;
            FGiftDetailView.Sort = ARecurringGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftAmount, GetRecurringBatchRow().CurrencyCode);

            if (FGiftDetailView.Count == 1)
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Are you sure you want to delete Gift no. {1} from Recurring Gift Batch no. {2}?" +
                            "\n\r\n\r" + "     From:  {3}" +
                            "\n\r" + "         To:  {4}" +
                            "\n\r" + "Amount:  {5}"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber,
                        ARowToDelete.DonorName,
                        ARowToDelete.RecipientDescription,
                        formattedDetailAmount);
            }
            else if (FGiftDetailView.Count > 1)
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString(
                            "Are you sure you want to delete detail line: {0} from Gift no. {1} in Recurring Gift Batch no. {2}?" +
                            "\n\r\n\r" + "     From:  {3}" +
                            "\n\r" + "         To:  {4}" +
                            "\n\r" + "Amount:  {5}"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber,
                        ARowToDelete.DonorName,
                        ARowToDelete.RecipientDescription,
                        formattedDetailAmount);
            }
            else //this should never happen
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString(
                            "Gift no. {0} in Recurring Gift Batch no. {1} has no detail rows in the Recurring Gift Detail table!"),
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                AllowDeletion = false;
            }

            return AllowDeletion;
        }

        private bool OnDeleteRowManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            //TODO: Make this like deleton on GL Transactions form
            // e.g. pass copy to delete method on server...
            //GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();
            //TempDS.Merge(FMainDS);

            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (FBatchRow == null)
            {
                FBatchRow = GetRecurringBatchRow();
            }

            if (ARowToDelete == null)
            {
                return false;
            }

            int CurrentBatchNo = ARowToDelete.BatchNumber;
            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);
            int CurrentRowIndex = GetSelectedRowIndex();

            TFrmRecurringGiftBatch FMyForm = (TFrmRecurringGiftBatch) this.ParentForm;

            GiftBatchTDS BackupMainDS = null;

            int SelectedDetailNumber = ARowToDelete.DetailNumber;
            int RecurringGiftToDeleteTransNo = 0;
            string FilterAllRecurringGiftsOfBatch = String.Empty;
            string FilterAllRecurringGiftDetailsOfBatch = String.Empty;

            int DetailRowCount = FGiftDetailView.Count;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Specify current action
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.DELETINGTRANS;
                //Speeds up deletion of larger gift sets
                FMainDS.EnforceConstraints = false;
                // temporarily disable  New Donor Warning
                FMyForm.NewDonorWarning = false;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GiftBatchTDS)FMainDS.GetChangesTyped(false);

                //Don't run an inactive fields check on this batch
                FMyForm.GetBatchControl().UpdateRecurringBatchDictionary(CurrentBatchNo);

                //Delete current row
                ARowToDelete.RejectChanges();

                if (!RowToDeleteIsNew)
                {
                    ShowDetails(ARowToDelete);
                }

                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete Recurring Gift header row
                if (DetailRowCount > 1)
                {
                    ACompletionMessage = Catalog.GetString("Recurring Gift Detail row deleted successfully!");

                    FGiftSelectedForDeletionFlag = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        if (row.DetailNumber > SelectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;
                }
                else
                {
                    ACompletionMessage = Catalog.GetString("Recurring Gift deleted successfully!");

                    RecurringGiftToDeleteTransNo = FGift.GiftTransactionNumber;

                    // Reduce all Recurring Gift Detail row Transaction numbers by 1 if they are greater then Recurring Gift to be deleted
                    FilterAllRecurringGiftDetailsOfBatch = String.Format("{0}={1} And {2}>{3}",
                        ARecurringGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                        RecurringGiftToDeleteTransNo);

                    DataView RecurringGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
                    RecurringGiftDetailView.RowFilter = FilterAllRecurringGiftDetailsOfBatch;
                    RecurringGiftDetailView.Sort = String.Format("{0} ASC", ARecurringGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in RecurringGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the Recurring Gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the Recurring Gift row to be deleted to the top
                    FilterAllRecurringGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        ARecurringGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftTable.GetGiftTransactionNumberDBName(),
                        RecurringGiftToDeleteTransNo);

                    DataView RecurringGiftView = new DataView(FMainDS.ARecurringGift);
                    RecurringGiftView.RowFilter = FilterAllRecurringGiftsOfBatch;
                    RecurringGiftView.Sort = String.Format("{0} ASC", ARecurringGiftTable.GetGiftTransactionNumberDBName());

                    ARecurringGiftRow RecurringGiftRowToReceive = null;
                    ARecurringGiftRow RecurringGiftRowToCopyDown = null;
                    ARecurringGiftRow RecurringGiftRowCurrent = null;

                    int currentRecurringGiftTransNo = 0;

                    foreach (DataRowView gv in RecurringGiftView)
                    {
                        RecurringGiftRowCurrent = (ARecurringGiftRow)gv.Row;

                        currentRecurringGiftTransNo = RecurringGiftRowCurrent.GiftTransactionNumber;

                        if (currentRecurringGiftTransNo > RecurringGiftToDeleteTransNo)
                        {
                            RecurringGiftRowToCopyDown = RecurringGiftRowCurrent;

                            //Copy column values down
                            for (int j = 3; j < RecurringGiftRowToCopyDown.Table.Columns.Count; j++)
                            {
                                //Update all columns except the pk fields that remain the same
                                if (!RecurringGiftRowToCopyDown.Table.Columns[j].ColumnName.EndsWith("_text"))
                                {
                                    RecurringGiftRowToReceive[j] = RecurringGiftRowToCopyDown[j];
                                }
                            }
                        }

                        if (currentRecurringGiftTransNo == FBatchRow.LastGiftNumber)
                        {
                            //Mark last record for deletion
                            RecurringGiftRowCurrent.ChargeStatus = MFinanceConstants.MARKED_FOR_DELETION;
                        }

                        //Will always be previous row
                        RecurringGiftRowToReceive = RecurringGiftRowCurrent;
                    }

                    FPreviouslySelectedDetailRow = null;
                    FGiftSelectedForDeletionFlag = true;
                    FBatchRow.LastGiftNumber--;
                }

                //Save and check for inactive values and ex-workers and anonymous gifts
                //  in other unsaved Batches
                FPetraUtilsObject.SetChangedFlag();

                if (!FMyForm.SaveChangesManual(Logic.TExtraGiftBatchChecks.GiftBatchAction.DELETINGTRANS, false, false))
                {
                    FMyForm.GetBatchControl().UpdateRecurringBatchDictionary();

                    MessageBox.Show(Catalog.GetString("The gift detail has been deleted but the changes are not saved!"),
                        Catalog.GetString("Deletion Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    ACompletionMessage = string.Empty;

                    if (FGiftSelectedForDeletionFlag)
                    {
                        FGiftSelectedForDeletionFlag = false;
                        SetBatchLastGiftNumber();
                        UpdateControlsProtection();
                    }

                    UpdateTotals();

                    return false;
                }

                //Clear current batch's gift data and reload from server
                RefreshRecurringBatchGiftData(FBatchNumber, true);

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS, CurrentRowIndex);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMyForm.NewDonorWarning = true;
                FMainDS.EnforceConstraints = true;
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.NONE;
                this.Cursor = Cursors.Default;
            }

            SetGiftDetailDefaultView();
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();

            return DeletionSuccessful;
        }

        private void OnPostDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                if (FGiftSelectedForDeletionFlag)
                {
                    FGiftSelectedForDeletionFlag = false;
                    SetBatchLastGiftNumber();
                    UpdateControlsProtection();
                }

                UpdateTotals();

                ((TFrmRecurringGiftBatch) this.ParentForm).SaveChangesManual();

                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (!AAllowDeletion && (ACompletionMessage.Length > 0))
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion delete gift

        #region revert to backup

        private void RevertDataSet(GiftBatchTDS AMainDS, GiftBatchTDS ABackupDS, int ASelectRowInGrid = 0)
        {
            if ((ABackupDS != null) && (AMainDS != null))
            {
                AMainDS.RejectChanges();
                AMainDS.Merge(ABackupDS);

                if (ASelectRowInGrid > 0)
                {
                    SelectRowInGrid(ASelectRowInGrid);
                }
            }
        }

        #endregion revert to backup
    }
}