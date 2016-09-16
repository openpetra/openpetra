//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles Deleteling of batches
    /// </summary>
    public class TUC_RecurringGiftBatches_Delete
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFrmRecurringGiftBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_RecurringGiftBatches_Delete(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GiftBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmRecurringGiftBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="AFPrevRow">Is FPreviouslySelectedDetailRow</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        public bool DeleteRowManual(ARecurringGiftBatchRow ARowToDelete, ref ARecurringGiftBatchRow AFPrevRow, ref string ACompletionMessage)
        {
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            int CurrentBatchNo = ARowToDelete.BatchNumber;
            bool CurrentBatchTransactionsLoadedAndCurrent = false;

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = null;

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                //Backup the changes to allow rollback
                BackupMainDS = (GiftBatchTDS)FMainDS.GetChangesTyped(false);

                //Don't run an inactive fields check on this batch
                FMyForm.GetBatchControl().UpdateRecurringBatchDictionary(CurrentBatchNo);

                //Check if current batch gift details are currently loaded
                CurrentBatchTransactionsLoadedAndCurrent = (FMyForm.GetTransactionsControl().FBatchNumber == CurrentBatchNo);

                //Save and check for inactive values and ex-workers and anonymous gifts
                //  in other unsaved Batches
                FPetraUtilsObject.SetChangedFlag();

                if (!FMyForm.SaveChangesManual(TExtraGiftBatchChecks.GiftBatchAction.DELETING, !CurrentBatchTransactionsLoadedAndCurrent))
                {
                    FMyForm.GetBatchControl().UpdateRecurringBatchDictionary();

                    string msg = String.Format(Catalog.GetString("Recurring Batch {0} has not been deleted."),
                        CurrentBatchNo);

                    MessageBox.Show(msg,
                        "Recurring Gift Batch Deleted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return false;
                }

                //Remove any changes to current batch that may cause validation issues
                FMyForm.GetBatchControl().PrepareBatchDataForDeleting(CurrentBatchNo, true);

                if (CurrentBatchTransactionsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    FMyForm.GetTransactionsControl().ClearCurrentSelection(CurrentBatchNo);
                }

                //Delete transactions
                FMyForm.GetTransactionsControl().DeleteRecurringBatchGiftData(CurrentBatchNo);

                // Delete the recurring batch row and save again after deleting the batch row.
                ARowToDelete.Delete();

                ACompletionMessage = String.Format(Catalog.GetString("Recurring Gift Batch no.: {0} deleted successfully."),
                    CurrentBatchNo);

                AFPrevRow = null;

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                if (BackupMainDS != null)
                {
                    FMainDS.ARecurringGiftDetail.RejectChanges();
                    FMainDS.ARecurringGiftDetail.Merge(BackupMainDS.ARecurringGiftDetail);
                    FMainDS.ARecurringGift.RejectChanges();
                    FMainDS.ARecurringGift.Merge(BackupMainDS.ARecurringGift);
                    FMainDS.ARecurringGiftBatch.RejectChanges();
                    FMainDS.ARecurringGiftBatch.Merge(BackupMainDS.ARecurringGiftBatch);
                    FMainDS.ALedger.RejectChanges();
                    FMainDS.ALedger.Merge(BackupMainDS.ALedger);

                    FMyForm.GetBatchControl().ShowDetailsRefresh();
                }

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }

            return DeletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        public void PostDeleteManual(ARecurringGiftBatchRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                if (FMyForm.SaveChangesManual())
                {
                    MessageBox.Show(ACompletionMessage, Catalog.GetString("Deletion Completed"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString(
                            "Unable to save after deletion of batch! Try saving manually and closing and reopening the form."));
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
            }
            else if (!ADeletionPerformed)
            {
                //message to user
            }
        }

        #endregion
    }
}