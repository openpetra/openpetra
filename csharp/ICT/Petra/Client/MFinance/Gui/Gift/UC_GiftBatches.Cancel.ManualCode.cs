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
        /// <param name="ACurrentBatchRow"></param>
        /// <returns></returns>
        public bool CancelBatch(AGiftBatchRow ACurrentBatchRow)
        {
            //Assign default value(s)
            bool CancellationSuccessful = false;

            string CancelMessage = string.Empty;
            string CompletionMessage = string.Empty;

            List <string>ModifiedDetailKeys = new List <string>();

            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return CancellationSuccessful;
            }

            int CurrentBatchNo = ACurrentBatchRow.BatchNumber;

            CancelMessage = String.Format(Catalog.GetString("Are you sure you want to cancel gift batch number: {0}?"),
                ACurrentBatchRow.BatchNumber);

            if ((MessageBox.Show(CancelMessage,
                     "Cancel Batch",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return CancellationSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = (GiftBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                CompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} cancelled successfully."),
                    ACurrentBatchRow.BatchNumber);

                FMyForm.GetBatchControl().UndoModifiedBatchRow(ACurrentBatchRow, true);

                //Load all journals for current Batch
                //clear any transactions currently being editied in the Transaction Tab
                FMyForm.GetTransactionsControl().ClearCurrentSelection();

                //Delete transactions
                DataView GiftDV = new DataView(FMainDS.AGift);
                DataView GiftDetailDV = new DataView(FMainDS.AGiftDetail);

                GiftDV.AllowDelete = true;
                GiftDetailDV.AllowDelete = true;

                GiftDV.RowFilter = String.Format("{0}={1}",
                    AGiftTable.GetBatchNumberDBName(),
                    CurrentBatchNo);

                GiftDV.Sort = AGiftTable.GetGiftTransactionNumberDBName() + " DESC";

                GiftDetailDV.RowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    CurrentBatchNo);

                GiftDetailDV.Sort = String.Format("{0} DESC, {1} DESC",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView drv in GiftDetailDV)
                {
                    AGiftDetailRow gdr = (AGiftDetailRow)drv.Row;

                    // if the gift detail being cancelled is a reversed gift
                    if (gdr.ModifiedDetail && !string.IsNullOrEmpty(gdr.ModifiedDetailKey))
                    {
                        ModifiedDetailKeys.Add(gdr.ModifiedDetailKey);
                    }

                    gdr.Delete();
                }

                for (int i = 0; i < GiftDV.Count; i++)
                {
                    GiftDV.Delete(i);
                }

                //Batch is only cancelled and never deleted
                ACurrentBatchRow.BatchTotal = 0;
                ACurrentBatchRow.LastGiftNumber = 0;
                ACurrentBatchRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;

                FPetraUtilsObject.SetChangedFlag();

                // save first
                if (FMyForm.SaveChanges())
                {
                    if (ModifiedDetailKeys.Count > 0)
                    {
                        TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, ModifiedDetailKeys);
                    }

                    MessageBox.Show(CompletionMessage,
                        "Batch Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception(Catalog.GetString("The batch failed to save after being cancelled! Reopen the form and retry."));
                }

                CancellationSuccessful = true;
            }
            catch (Exception ex)
            {
                //Revert to previous state
                FMainDS.Merge(BackupMainDS);

                CompletionMessage = ex.Message;
                MessageBox.Show(CompletionMessage,
                    "Cancellation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
