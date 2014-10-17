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
            string CancelMessage = string.Empty;
            string CompletionMessage = string.Empty;
            string ExistingBatchStatus = string.Empty;
            decimal ExistingBatchTotal = 0;

            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            CancelMessage = String.Format(Catalog.GetString("Are you sure you want to cancel gift batch no.: {0}?"),
                ACurrentBatchRow.BatchNumber);

            if ((MessageBox.Show(CancelMessage,
                     "Cancel Batch",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            // first save any changes
            if (!FMyForm.SaveChangesManual(TExWorkerAlert.GiftBatchAction.CANCELLING))
            {
                return false;
            }

            try
            {
                //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                CompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} cancelled successfully."),
                    ACurrentBatchRow.BatchNumber);

                ExistingBatchTotal = ACurrentBatchRow.BatchTotal;
                ExistingBatchStatus = ACurrentBatchRow.BatchStatus;

                //Load all journals for current Batch
                //clear any transactions currently being editied in the Transaction Tab
                FMyForm.GetTransactionsControl().ClearCurrentSelection();

                //Clear gifts and details etc for current Batch
                FMainDS.AGiftDetail.Clear();
                FMainDS.AGift.Clear();

                //Load tables afresh
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(FLedgerNumber, ACurrentBatchRow.BatchNumber));

                FMyForm.ProcessRecipientCostCentreCodeUpdateErrors(false);

                //Delete gift details
                for (int i = FMainDS.AGiftDetail.Count - 1; i >= 0; i--)
                {
                    FMainDS.AGiftDetail[i].Delete();
                }

                //Delete gifts
                for (int i = FMainDS.AGift.Count - 1; i >= 0; i--)
                {
                    FMainDS.AGift[i].Delete();
                }

                //Batch is only cancelled and never deleted
                ACurrentBatchRow.BeginEdit();
                ACurrentBatchRow.BatchTotal = 0;
                ACurrentBatchRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                ACurrentBatchRow.EndEdit();

                FPetraUtilsObject.HasChanges = true;

                // save first, then post
                if (!FMyForm.SaveChanges())
                {
                    ACurrentBatchRow.BeginEdit();
                    //Should normally be Unposted, but allow for other status values in future
                    ACurrentBatchRow.BatchTotal = ExistingBatchTotal;
                    ACurrentBatchRow.BatchStatus = ExistingBatchStatus;
                    ACurrentBatchRow.EndEdit();

                    // saving failed, therefore do not try to cancel
                    MessageBox.Show(Catalog.GetString("The cancelled batch failed to save!"));
                }
                else
                {
                    MessageBox.Show(CompletionMessage,
                        "Batch Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                CompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Cancellation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return false;
        }

        #endregion
    }
}