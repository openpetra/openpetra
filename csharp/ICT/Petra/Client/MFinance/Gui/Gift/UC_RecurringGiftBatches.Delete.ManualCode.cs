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

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            int BatchNumber = ARowToDelete.BatchNumber;

            ACompletionMessage = string.Empty;

            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = (GiftBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            if (!RowToDeleteIsNew)
            {
                //Return modified row to last saved state to avoid validation failures
                ARowToDelete.RejectChanges();

                if (!FMyForm.SaveChangesManual(TExtraGiftBatchChecks.GiftBatchAction.DELETING))
                {
                    MessageBox.Show(Catalog.GetString("Error in trying to save prior to deleting current recurring gift batch!"),
                        Catalog.GetString("Deletion Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return DeletionSuccessful;
                }
            }

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                ACompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} deleted successfully."),
                    BatchNumber);

                //clear any transactions currently being editied in the Transaction Tab
                FMyForm.GetTransactionsControl().ClearCurrentSelection();

                if (!RowToDeleteIsNew)
                {
                    //Load tables afresh
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringGiftTransactionsForBatch(FLedgerNumber, BatchNumber));
                }

                FMyForm.GetTransactionsControl().DeleteCurrentRecurringBatchGiftData(BatchNumber);

                // Delete the recurring batch row.
                ARowToDelete.Delete();

                AFPrevRow = null;

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                FMainDS.Merge(BackupMainDS);

                ACompletionMessage = ex.Message;
                MessageBox.Show(ACompletionMessage,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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