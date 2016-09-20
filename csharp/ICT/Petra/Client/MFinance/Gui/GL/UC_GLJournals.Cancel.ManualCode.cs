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
using System.Drawing;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles cancelling of batches
    /// </summary>
    public class TUC_GLJournals_Cancel
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private TFrmGLBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GLJournals_Cancel(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GLBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmGLBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Main Public methods

        /// <summary>
        /// Method to cancel a specified journal
        /// </summary>
        /// <param name="AJournalRowToCancel">The row to cancel</param>
        /// <returns>True if the journal is cancelled.</returns>
        public bool CancelRow(GLBatchTDSAJournalRow AJournalRowToCancel)
        {
            bool CancellationSuccessful = false;

            if ((AJournalRowToCancel == null) || (AJournalRowToCancel.JournalStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            int CurrentBatchNo = AJournalRowToCancel.BatchNumber;
            int CurrentJournalNo = AJournalRowToCancel.JournalNumber;
            bool CurrentJournalTransactionsLoadedAndCurrent = false;

            string CancelMessage = string.Empty;
            string CompletionMessage = string.Empty;

            CancelMessage = String.Format(Catalog.GetString("Are you sure you want to cancel GL Journal {0} in Batch {1}?"),
                CurrentJournalNo,
                CurrentBatchNo);

            if ((MessageBox.Show(CancelMessage,
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return false;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = null;

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GLBatchTDS)FMainDS.GetChangesTyped(false);

                //Don't run an inactive fields check on this batch
                FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary(CurrentBatchNo);

                //Check if current batch details are currently loaded
                CurrentJournalTransactionsLoadedAndCurrent = (FMyForm.GetTransactionsControl().FBatchNumber == CurrentBatchNo);

                //Save and check for inactive values in other unsaved Batches
                FPetraUtilsObject.SetChangedFlag();

                if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction, false, !CurrentJournalTransactionsLoadedAndCurrent))
                {
                    FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary();

                    CompletionMessage = String.Format(Catalog.GetString("Journal {0} in GL Batch {1} has not been cancelled."),
                        CurrentJournalNo,
                        CurrentBatchNo);

                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("GL Journal Cancellation"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return false;
                }

                //Remove any changes, which may cause validation issues, before cancelling
                FMyForm.GetJournalsControl().PrepareJournalDataForCancelling(CurrentBatchNo, CurrentJournalNo, true);

                if (CurrentJournalTransactionsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    FMyForm.GetTransactionsControl().ClearCurrentSelection(CurrentBatchNo);
                }

                //Delete transactions and attributes for current jouernal only
                FMyForm.GetTransactionsControl().DeleteTransactionData(CurrentBatchNo, CurrentJournalNo);

                //Edit current Journal
                decimal journalCreditTotal = AJournalRowToCancel.JournalCreditTotal;
                decimal journalDebitTotal = AJournalRowToCancel.JournalDebitTotal;
                AJournalRowToCancel.BeginEdit();
                AJournalRowToCancel.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                AJournalRowToCancel.LastTransactionNumber = 0;
                AJournalRowToCancel.JournalCreditTotal = 0;
                AJournalRowToCancel.JournalDebitTotal = 0;
                AJournalRowToCancel.EndEdit();

                //Edit current Batch
                ABatchRow CurrentBatchRow = FMyForm.GetBatchControl().GetSelectedDetailRow();
                decimal batchControlTotal = CurrentBatchRow.BatchControlTotal;
                CurrentBatchRow.BeginEdit();
                CurrentBatchRow.BatchCreditTotal -= journalCreditTotal;
                CurrentBatchRow.BatchDebitTotal -= journalDebitTotal;
                CurrentBatchRow.BatchControlTotal -= (batchControlTotal != 0 ? journalCreditTotal : 0);
                CurrentBatchRow.EndEdit();

                FPetraUtilsObject.SetChangedFlag();
                FMyForm.SaveChangesManual();

                CompletionMessage = String.Format(Catalog.GetString("Journal no.: {0} cancelled successfully."),
                    CurrentJournalNo);

                MessageBox.Show(CompletionMessage,
                    Catalog.GetString("Journal Cancelled"),
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
                    FMainDS.RejectChanges();
                    FMainDS.Merge(BackupMainDS);

                    FMyForm.GetJournalsControl().ShowDetailsRefresh();
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