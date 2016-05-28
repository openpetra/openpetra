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
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftTransactions
    {
        private bool OnPreDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

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
                allowDeletion = false;
            }

            return allowDeletion;
        }

        private bool OnDeleteRowManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew)
            {
                try
                {
                    // temporarily disable  New Donor Warning
                    ((TFrmRecurringGiftBatch) this.ParentForm).NewDonorWarning = false;

                    //Return modified row to last saved state to avoid validation failures
                    ARowToDelete.RejectChanges();
                    ShowDetails(ARowToDelete);

                    if (!((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("Error in trying to save prior to deleting current Gift detail!"),
                            Catalog.GetString("Deletion Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return DeletionSuccessful;
                    }
                }
                finally
                {
                    ((TFrmRecurringGiftBatch) this.ParentForm).NewDonorWarning = true;
                }
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = (GiftBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            //To be used later....Pass copy to delete method.
            //RecurringGiftBatchTDS TempDS = (RecurringGiftBatchTDS)FMainDS.Copy();
            //TempDS.Merge(FMainDS);

            int SelectedDetailNumber = ARowToDelete.DetailNumber;
            int RecurringGiftToDeleteTransNo = 0;
            string FilterAllRecurringGiftsOfBatch = String.Empty;
            string FilterAllRecurringGiftDetailsOfBatch = String.Empty;

            int DetailRowCount = FGiftDetailView.Count;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Speeds up deletion of larger Recurring Gift sets
                FMainDS.EnforceConstraints = false;

                //Delete current detail row
                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete Recurring Gift header row
                if (DetailRowCount > 1)
                {
                    ACompletionMessage = Catalog.GetString("Recurring Gift Detail row deleted successfully!");

                    FGiftSelectedForDeletion = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        if (row.DetailNumber > SelectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;

                    FPetraUtilsObject.SetChangedFlag();
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

                    FPetraUtilsObject.SetChangedFlag();

                    FGiftSelectedForDeletion = true;

                    FBatchRow.LastGiftNumber--;
                }

                //Try to save changes
                if (((TFrmRecurringGiftBatch) this.ParentForm).SaveChangesManual())
                {
                    //Clear current batch's Recurring Gift data and reload from server
                    RefreshCurrentRecurringBatchGiftData(FBatchNumber, true);
                }
                else
                {
                    throw new Exception("Unable to save after deleting a Recurring Gift!");
                }

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMainDS.EnforceConstraints = true;
                SetGiftDetailDefaultView();
                FFilterAndFindObject.ApplyFilter();
                this.Cursor = Cursors.Default;
            }

            UpdateRecordNumberDisplay();

            return DeletionSuccessful;
        }

        private void RevertDataSet(GiftBatchTDS AMainDS, GiftBatchTDS ABackupDS)
        {
            AMainDS.ALedger.Clear();
            AMainDS.AGiftDetail.Clear();
            AMainDS.AGift.Clear();
            AMainDS.AGiftBatch.Clear();

            AMainDS.Merge(ABackupDS);
        }

        private void OnPostDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                if (FGiftSelectedForDeletion)
                {
                    FGiftSelectedForDeletion = false;

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

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
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
            GiftBatchTDS BackupMainDS = (GiftBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have chosen to delete all Gifts from Recurring Batch ({0}).{1}{1}Are you sure you want to delete all?"),
                        BatchNumberToClear,
                        Environment.NewLine),
                    Catalog.GetString("Confirm Delete All"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                    CompletionMessage = String.Format(Catalog.GetString("All Recurring Gifts and details deleted successfully."),
                        FPreviouslySelectedDetailRow.BatchNumber);

                    //clear any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection(false);

                    //Now delete all Recurring Gift data for current batch
                    DeleteCurrentRecurringBatchGiftData(BatchNumberToClear);

                    FBatchRow.BatchTotal = 0;
                    txtBatchTotal.NumberValueDecimal = 0;

                    // Be sure to set the last Recurring Gift number in the parent table before saving all the changes
                    FBatchRow.LastGiftNumber = 0;

                    FPetraUtilsObject.SetChangedFlag();

                    // save first, then post
                    if (((TFrmRecurringGiftBatch)ParentForm).SaveChangesManual())
                    {
                        MessageBox.Show(CompletionMessage,
                            "All Recurring Gifts Deleted.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception("Unable to save after deleting all Recurring Gifts!");
                    }
                }
                catch (Exception ex)
                {
                    //Revert to previous state
                    RevertDataSet(FMainDS, BackupMainDS);

                    TLogging.LogException(ex, Utilities.GetMethodSignature());
                    throw;
                }
                finally
                {
                    SetGiftDetailDefaultView();
                    FFilterAndFindObject.ApplyFilter();
                    this.Cursor = Cursors.Default;
                }
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                UpdateControlsProtection();
            }

            UpdateRecordNumberDisplay();
        }

        /// <summary>
        /// DeleteCurrentRecurringBatchGiftData
        /// </summary>
        /// <param name="ABatchNumber"></param>
        public void DeleteCurrentRecurringBatchGiftData(Int32 ABatchNumber)
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
    }
}