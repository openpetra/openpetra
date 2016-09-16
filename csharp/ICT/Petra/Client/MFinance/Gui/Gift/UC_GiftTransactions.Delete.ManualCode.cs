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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        #region delete all gifts in batch

        /// <summary>
        /// Delete data from current gift batch
        /// </summary>
        /// <param name="ABatchNumber"></param>
        /// <param name="AModifiedDetailKeyRows"></param>
        public void DeleteBatchGiftData(Int32 ABatchNumber, ref List <string>AModifiedDetailKeyRows)
        {
            DataView giftDetailView = new DataView(FMainDS.AGiftDetail);

            giftDetailView.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                AGiftDetailTable.GetDetailNumberDBName());

            foreach (DataRowView dr in giftDetailView)
            {
                AGiftDetailRow gdr = (AGiftDetailRow)dr.Row;

                if (gdr.ModifiedDetail && !string.IsNullOrEmpty(gdr.ModifiedDetailKey))
                {
                    AModifiedDetailKeyRows.Add(gdr.ModifiedDetailKey);
                }

                dr.Delete();
            }

            DataView GiftView = new DataView(FMainDS.AGift);

            GiftView.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            GiftView.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in GiftView)
            {
                dr.Delete();
            }
        }

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
            TFrmGiftBatch FMyForm = (TFrmGiftBatch) this.ParentForm;

            string CompletionMessage = string.Empty;
            int BatchNumberToClear = FBatchNumber;

            List <string>OriginatingDetailRef = new List <string>();

            if ((FPreviouslySelectedDetailRow == null) || (FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }
            else if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
            {
                MessageBox.Show(Catalog.GetString("Please remove the filter before attempting to delete all gifts in this batch."),
                    Catalog.GetString("Delete All Gifts"));

                return;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupDS = (GiftBatchTDS)FMainDS.GetChangesTyped(false);

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have chosen to delete all gifts from Gift Batch: {0}.{1}{1}Are you sure you want to delete all?"),
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

                //Now delete all gift data for current batch
                DeleteBatchGiftData(BatchNumberToClear, ref OriginatingDetailRef);

                FBatchRow.BatchTotal = 0;
                txtBatchTotal.NumberValueDecimal = 0;

                // Be sure to set the last gift number in the parent table before saving all the changes
                FBatchRow.LastGiftNumber = 0;

                FPetraUtilsObject.SetChangedFlag();

                // save changes
                if (((TFrmGiftBatch)ParentForm).SaveChangesManual())
                {
                    //Check if have deleted a reversing gift detail
                    if (OriginatingDetailRef.Count > 0)
                    {
                        TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, OriginatingDetailRef);
                    }

                    CompletionMessage = Catalog.GetString("All Gifts and their details deleted successfully.");

                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("Gifts Deletion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    CompletionMessage = Catalog.GetString("All Gifts and their details have been deleted but saving the changes failed!");

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

        private bool OnPreDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool AllowDeletion = true;

            FGift = GetGiftRow(ARowToDelete.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                AGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.AGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;
            FGiftDetailView.Sort = AGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftTransactionAmount, GetBatchRow().CurrencyCode);

            if (FGiftDetailView.Count == 1)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete gift no. {1} from Gift Batch no. {2}?" +
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
                    String.Format(Catalog.GetString("Are you sure you want to delete detail line: {0} from gift no. {1} in Gift Batch no. {2}?" +
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
                    String.Format(Catalog.GetString("Gift no. {0} in Batch no. {1} has no detail rows in the Gift Detail table!"),
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                AllowDeletion = false;
            }

            return AllowDeletion;
        }

        private bool OnDeleteRowManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            //TODO: Make this like deleton on GL Transactions form
            // e.g. pass copy to delete method on server...
            //GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();
            //TempDS.Merge(FMainDS);

            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (FBatchRow == null)
            {
                FBatchRow = GetBatchRow();
            }

            if ((ARowToDelete == null) || (FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            int CurrentBatchNo = ARowToDelete.BatchNumber;
            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);
            int CurrentRowIndex = GetSelectedRowIndex();

            TFrmGiftBatch FMyForm = (TFrmGiftBatch) this.ParentForm;

            GiftBatchTDS BackupMainDS = null;
            List <string>OriginatingDetailRef = new List <string>();

            int SelectedDetailNumber = ARowToDelete.DetailNumber;
            int GiftToDeleteTransNo = 0;
            string FilterAllGiftsOfBatch = String.Empty;
            string FilterAllGiftDetailsOfBatch = String.Empty;

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
                FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary(CurrentBatchNo);

                if ((ARowToDelete.ModifiedDetailKey != null) && (ARowToDelete.ModifiedDetailKey.Length > 0))
                {
                    OriginatingDetailRef.Add(ARowToDelete.ModifiedDetailKey);
                }

                //Delete current row
                ARowToDelete.RejectChanges();

                if (!RowToDeleteIsNew)
                {
                    ShowDetails(ARowToDelete);
                }

                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete gift header row
                if (DetailRowCount > 1)
                {
                    ACompletionMessage = Catalog.GetString("Gift Detail row deleted successfully!");

                    FGiftSelectedForDeletionFlag = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        if (row.DetailNumber > SelectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;
                }
                else
                {
                    ACompletionMessage = Catalog.GetString("Gift deleted successfully!");

                    GiftToDeleteTransNo = FGift.GiftTransactionNumber;

                    // Reduce all Gift Detail row Transaction numbers by 1 if they are greater then gift to be deleted
                    FilterAllGiftDetailsOfBatch = String.Format("{0}={1} And {2}>{3}",
                        AGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftDetailTable.GetGiftTransactionNumberDBName(),
                        GiftToDeleteTransNo);

                    DataView giftDetailView = new DataView(FMainDS.AGiftDetail);
                    giftDetailView.RowFilter = FilterAllGiftDetailsOfBatch;
                    giftDetailView.Sort = String.Format("{0} ASC", AGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the gift row to be deleted to the top
                    FilterAllGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        AGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftTable.GetGiftTransactionNumberDBName(),
                        GiftToDeleteTransNo);

                    DataView giftView = new DataView(FMainDS.AGift);
                    giftView.RowFilter = FilterAllGiftsOfBatch;
                    giftView.Sort = String.Format("{0} ASC", AGiftTable.GetGiftTransactionNumberDBName());

                    AGiftRow giftRowToReceive = null;
                    AGiftRow giftRowToCopyDown = null;
                    AGiftRow giftRowCurrent = null;

                    int currentGiftTransNo = 0;

                    foreach (DataRowView gv in giftView)
                    {
                        giftRowCurrent = (AGiftRow)gv.Row;

                        currentGiftTransNo = giftRowCurrent.GiftTransactionNumber;

                        if (currentGiftTransNo > GiftToDeleteTransNo)
                        {
                            giftRowToCopyDown = giftRowCurrent;

                            //Copy column values down
                            for (int j = 3; j < giftRowToCopyDown.Table.Columns.Count; j++)
                            {
                                //Update all columns except the pk fields that remain the same
                                if (!giftRowToCopyDown.Table.Columns[j].ColumnName.EndsWith("_text"))
                                {
                                    giftRowToReceive[j] = giftRowToCopyDown[j];
                                }
                            }
                        }

                        if (currentGiftTransNo == FBatchRow.LastGiftNumber)
                        {
                            //Mark last record for deletion
                            giftRowCurrent.GiftStatus = MFinanceConstants.MARKED_FOR_DELETION;
                        }

                        //Will always be previous row
                        giftRowToReceive = giftRowCurrent;
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
                    FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary();

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

                //Check if have deleted a reversing gift detail
                if (OriginatingDetailRef.Count > 0)
                {
                    TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, OriginatingDetailRef);
                }

                //Clear current batch's gift data and reload from server
                RefreshBatchGiftData(FBatchNumber, true);

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

        private void OnPostDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete,
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

                ((TFrmGiftBatch) this.ParentForm).SaveChangesManual();

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