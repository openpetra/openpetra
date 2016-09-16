//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
//
// Copyright 2004-2015 by OM International
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
    /// A business logic class that handles cancelling of batches
    /// </summary>
    public class TUC_GiftBatches_Cancel
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFrmGiftBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_Cancel(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GiftBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmGiftBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to cancel a specified batch
        /// </summary>
        /// <param name="ABatchRowToCancel"></param>
        /// <returns></returns>
        public bool CancelBatch(AGiftBatchRow ABatchRowToCancel)
        {
            bool CancellationSuccessful = false;

            if ((ABatchRowToCancel == null) || (ABatchRowToCancel.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            int CurrentBatchNo = ABatchRowToCancel.BatchNumber;
            bool CurrentBatchTransactionsLoadedAndCurrent = false;

            string CancelMessage = string.Empty;
            string CompletionMessage = string.Empty;

            List <string>ModifiedDetailKeys = new List <string>();

            CancelMessage = String.Format(Catalog.GetString("Are you sure you want to cancel Gift Batch {0}?"),
                CurrentBatchNo);

            if ((MessageBox.Show(CancelMessage,
                     "Cancel Gift Batch",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = null;

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GiftBatchTDS)FMainDS.GetChangesTyped(false);

                //Don't run an inactive fields check on this batch
                FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary(CurrentBatchNo);

                //Check if current batch gift details are currently loaded
                CurrentBatchTransactionsLoadedAndCurrent = (FMyForm.GetTransactionsControl().FBatchNumber == CurrentBatchNo);

                //Save and check for inactive values and ex-workers and anonymous gifts
                //  in other unsaved Batches
                FPetraUtilsObject.SetChangedFlag();

                if (!FMyForm.SaveChangesManual(TExtraGiftBatchChecks.GiftBatchAction.CANCELLING, !CurrentBatchTransactionsLoadedAndCurrent))
                {
                    FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary();

                    CompletionMessage = String.Format(Catalog.GetString("Gift Batch {0} has not been cancelled."),
                        CurrentBatchNo);

                    MessageBox.Show(CompletionMessage,
                        "Gift Batch Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return false;
                }

                //Remove any changes to current batch that may cause validation issues
                FMyForm.GetBatchControl().PrepareBatchDataForCancelling(CurrentBatchNo, true);

                if (CurrentBatchTransactionsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    FMyForm.GetTransactionsControl().ClearCurrentSelection(CurrentBatchNo);
                }

                //Delete transactions
                FMyForm.GetTransactionsControl().DeleteBatchGiftData(CurrentBatchNo, ref ModifiedDetailKeys);

                //Batch is only cancelled and never deleted
                ABatchRowToCancel.BeginEdit();
                ABatchRowToCancel.BatchTotal = 0;
                ABatchRowToCancel.LastGiftNumber = 0;
                ABatchRowToCancel.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                ABatchRowToCancel.EndEdit();

                if (ModifiedDetailKeys.Count > 0)
                {
                    TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, ModifiedDetailKeys);
                }

                FPetraUtilsObject.SetChangedFlag();
                FMyForm.SaveChangesManual();

                CompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} cancelled successfully."),
                    CurrentBatchNo);

                MessageBox.Show(CompletionMessage,
                    "Gift Batch Cancelled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                FMyForm.DisableTransactions();

                CancellationSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                if (BackupMainDS != null)
                {
                    FMainDS.AGiftDetail.RejectChanges();
                    FMainDS.AGiftDetail.Merge(BackupMainDS.AGiftDetail);
                    FMainDS.AGift.RejectChanges();
                    FMainDS.AGift.Merge(BackupMainDS.AGift);
                    FMainDS.AGiftBatch.RejectChanges();
                    FMainDS.AGiftBatch.Merge(BackupMainDS.AGiftBatch);

                    FMyForm.GetBatchControl().ShowDetailsRefresh();
                }

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }

            return CancellationSuccessful;
        }

        #endregion
    }
}
