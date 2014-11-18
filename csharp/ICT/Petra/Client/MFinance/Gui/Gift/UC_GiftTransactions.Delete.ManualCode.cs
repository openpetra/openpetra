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
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        private bool OnPreDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            FGift = GetGiftRow(ARowToDelete.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                AGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.AGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;
            FGiftDetailView.Sort = AGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftTransactionAmount, GetCurrentBatchRow().CurrencyCode);

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
                allowDeletion = false;
            }

            return allowDeletion;
        }

        private bool OnDeleteRowManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;
            string originatingDetailRef = string.Empty;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return deletionSuccessful;
            }

            // temporarily disable  New Donor Warning
            ((TFrmGiftBatch) this.ParentForm).NewDonorWarning = false;

            if ((ARowToDelete.RowState != DataRowState.Added) && !((TFrmGiftBatch) this.ParentForm).SaveChangesManual())
            {
                MessageBox.Show("Error in trying to save prior to deleting current gift detail!");
                return deletionSuccessful;
            }

            ((TFrmGiftBatch) this.ParentForm).NewDonorWarning = true;

            //Backup the Dataset for reversion purposes
            GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();
            FTempDS.Merge(FMainDS);

            int selectedDetailNumber = ARowToDelete.DetailNumber;
            int giftToDeleteTransNo = 0;
            string filterAllGiftsOfBatch = String.Empty;
            string filterAllGiftDetailsOfBatch = String.Empty;

            int detailRowCount = FGiftDetailView.Count;

            try
            {
                if (ARowToDelete.ModifiedDetailKey != null)
                {
                    originatingDetailRef = ARowToDelete.ModifiedDetailKey;
                }

                //Delete current detail row
                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete gift header row
                if (detailRowCount > 1)
                {
                    FGiftSelectedForDeletion = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        if (row.DetailNumber > selectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;

                    FPetraUtilsObject.SetChangedFlag();
                }
                else
                {
                    giftToDeleteTransNo = FGift.GiftTransactionNumber;

                    TLogging.Log("Delete row: " + giftToDeleteTransNo.ToString());

                    // Reduce all Gift Detail row Transaction numbers by 1 if they are greater then gift to be deleted
                    filterAllGiftDetailsOfBatch = String.Format("{0}={1} And {2}>{3}",
                        AGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftDetailTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftDetailView = new DataView(FMainDS.AGiftDetail);
                    giftDetailView.RowFilter = filterAllGiftDetailsOfBatch;
                    giftDetailView.Sort = String.Format("{0} ASC", AGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the gift row to be deleted to the top
                    filterAllGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        AGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftView = new DataView(FMainDS.AGift);
                    giftView.RowFilter = filterAllGiftsOfBatch;
                    giftView.Sort = String.Format("{0} ASC", AGiftTable.GetGiftTransactionNumberDBName());

                    AGiftRow giftRowToReceive = null;
                    AGiftRow giftRowToCopyDown = null;
                    AGiftRow giftRowCurrent = null;

                    int currentGiftTransNo = 0;

                    foreach (DataRowView gv in giftView)
                    {
                        giftRowCurrent = (AGiftRow)gv.Row;

                        currentGiftTransNo = giftRowCurrent.GiftTransactionNumber;

                        if (currentGiftTransNo > giftToDeleteTransNo)
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

                    FPetraUtilsObject.SetChangedFlag();

                    FGiftSelectedForDeletion = true;

                    FBatchRow.LastGiftNumber--;
                }

                //Check if deleting a reversed gift detail
                if (originatingDetailRef.StartsWith("|"))
                {
                    bool ok = TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, originatingDetailRef);

                    if (!ok)
                    {
                        throw new Exception("Error in trying to reset Modified Detail field of the originating gift detail.");
                    }
                }

                //Try to save changes
                if (((TFrmGiftBatch) this.ParentForm).SaveChangesManual())
                {
                    //Clear current batch's gift data and reload from server
                    if (RefreshCurrentBatchGiftData(FBatchNumber))
                    {
                        ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
                    }
                }
                else
                {
                    throw new Exception("Unable to save after deleting a gift!");
                }

                ACompletionMessage = Catalog.GetString("Gift row deleted successfully!");

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Gift Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS.Merge(FTempDS);
            }
            finally
            {
                SetGiftDetailDefaultView();
                FFilterAndFindObject.ApplyFilter();
            }

            UpdateRecordNumberDisplay();

            return deletionSuccessful;
        }

        private void OnPostDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete,
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

                    if (!pnlDetails.Enabled)
                    {
                        ClearControls();
                    }
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

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
            string completionMessage = string.Empty;
            int BatchNumber = FBatchNumber;

            if ((FPreviouslySelectedDetailRow == null) || (FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                ||
                (MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all gifts from batch ({0}).\n\nDo you really want to delete all?"),
                         BatchNumber),
                     Catalog.GetString("Confirm Delete All"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                    completionMessage = String.Format(Catalog.GetString("All gifts and details cancelled successfully."),
                        FPreviouslySelectedDetailRow.BatchNumber);

                    //clear any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection();

                    //Clear out the gift data for the current batch without marking the records for deletion
                    //  and then reload from server
                    if (RefreshCurrentBatchGiftData(BatchNumber))
                    {
                        ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
                    }

                    //Now delete all gift data for current batch
                    DeleteCurrentBatchGiftData(BatchNumber);

                    FBatchRow.BatchTotal = 0;

                    // Be sure to set the last gift number in the parent table before saving all the changes
                    SetBatchLastGiftNumber();

                    FPetraUtilsObject.HasChanges = true;

                    // save first, then post
                    if (!((TFrmGiftBatch)ParentForm).SaveChangesManual())
                    {
                        SelectRowInGrid(1);

                        // saving failed, therefore do not try to cancel
                        MessageBox.Show(Catalog.GetString("The emptied batch failed to save!"));
                    }
                    else
                    {
                        MessageBox.Show(completionMessage,
                            "All Gifts Deleted.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    completionMessage = ex.Message;
                    MessageBox.Show(ex.Message,
                        "Deletion Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    //Return FMainDS to original state
                    FMainDS.RejectChanges();
                }
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                UpdateControlsProtection();
            }

            UpdateRecordNumberDisplay();
        }

        private void DeleteCurrentBatchGiftData(Int32 ABatchNumber)
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
                dr.Delete();
            }

            DataView giftView = new DataView(FMainDS.AGift);

            giftView.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftView.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in giftView)
            {
                dr.Delete();
            }
        }
    }
}