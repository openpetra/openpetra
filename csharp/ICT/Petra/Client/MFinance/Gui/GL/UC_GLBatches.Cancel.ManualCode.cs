//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles cancelling of batches
    /// </summary>
    public class TUC_GLBatches_Cancel
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private TFrmGLBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GLBatches_Cancel(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GLBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmGLBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Main Public methods

        /// <summary>
        /// Method to cancel a specified batch
        /// </summary>
        /// <param name="ABatchRowToCancel">The row to cancel</param>
        /// <returns></returns>
        public bool CancelBatch(ABatchRow ABatchRowToCancel)
        {
            //Assign default value(s)
            bool CancellationSuccessful = false;

            if ((ABatchRowToCancel == null))
            {
                return CancellationSuccessful;
            }

            int CurrentBatchNumber = ABatchRowToCancel.BatchNumber;

            if ((MessageBox.Show(String.Format(Catalog.GetString("You have chosen to cancel this batch ({0}).\n\nDo you really want to cancel it?"),
                         CurrentBatchNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return CancellationSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = (GLBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            bool RowToDeleteIsNew = (ABatchRowToCancel.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew)
            {
                //Remove any changes, which may cause validation issues, before cancelling
                FMyForm.GetBatchControl().UndoModifiedBatchRow(ABatchRowToCancel, true);

                if (!(FMyForm.SaveChanges()))
                {
                    MessageBox.Show(Catalog.GetString("Error in trying to save prior to cancelling current Batch!"),
                        Catalog.GetString("Cancellation Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return CancellationSuccessful;
                }
            }

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                //clear any transactions currently being editied in the Transaction Tab
                FMyForm.GetTransactionsControl().ClearCurrentSelection();
                //clear any journals currently being editied in the Journals Tab
                FMyForm.GetJournalsControl().ClearCurrentSelection();

                //Clear Journals etc for current Batch
                FMainDS.ATransAnalAttrib.Clear();
                FMainDS.ATransaction.Clear();
                FMainDS.AJournal.Clear();

                //Load tables afresh
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(FLedgerNumber, CurrentBatchNumber));
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransaction(FLedgerNumber, CurrentBatchNumber));
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForBatch(FLedgerNumber, CurrentBatchNumber));
                FMainDS.AcceptChanges();

                //Delete transactions and analysis attributes
                for (int i = FMainDS.ATransAnalAttrib.Count - 1; i >= 0; i--)
                {
                    FMainDS.ATransAnalAttrib[i].Delete();
                }

                for (int i = FMainDS.ATransaction.Count - 1; i >= 0; i--)
                {
                    FMainDS.ATransaction[i].Delete();
                }

                //Update Journal totals and status
                foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                {
                    if (journal.BatchNumber == CurrentBatchNumber)
                    {
                        journal.BeginEdit();
                        journal.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                        journal.JournalCreditTotal = 0;
                        journal.JournalDebitTotal = 0;
                        journal.LastTransactionNumber = 0;
                        journal.EndEdit();
                    }
                }

                // Edit the batch row
                ABatchRowToCancel.BeginEdit();

                ABatchRowToCancel.BatchCreditTotal = 0;
                ABatchRowToCancel.BatchDebitTotal = 0;
                ABatchRowToCancel.BatchControlTotal = 0;
                ABatchRowToCancel.BatchStatus = MFinanceConstants.BATCH_CANCELLED;

                ABatchRowToCancel.EndEdit();

                FPetraUtilsObject.SetChangedFlag();

                //Need to call save
                if (FMyForm.SaveChanges())
                {
                    MessageBox.Show(Catalog.GetString("The batch has been cancelled successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FMyForm.DisableTransactions();
                    FMyForm.DisableJournals();
                }
                else
                {
                    throw new Exception(Catalog.GetString("The batch failed to save after being cancelled! Reopen the form and retry."));
                }

                CancellationSuccessful = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Cancellation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS.Merge(BackupMainDS);
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