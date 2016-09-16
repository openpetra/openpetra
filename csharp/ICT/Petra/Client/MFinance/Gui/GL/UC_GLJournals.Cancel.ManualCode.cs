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
            //Assign default value(s)
            bool CancellationSuccessful = false;

            string CancelMessage = string.Empty;
            string CompletionMessage = string.Empty;

            if ((AJournalRowToCancel == null) || (AJournalRowToCancel.JournalStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            int CurrentBatchNumber = AJournalRowToCancel.BatchNumber;
            int CurrentJournalNumber = AJournalRowToCancel.JournalNumber;

            CancelMessage = String.Format(Catalog.GetString("Are you sure you want to cancel GL Journal {0} in Batch {1}?"),
                CurrentJournalNumber,
                CurrentBatchNumber);

            if ((MessageBox.Show(CancelMessage,
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

            //Journals, unlike batches, are not auto-saved
            bool RowToDeleteIsNew = (AJournalRowToCancel.RowState == DataRowState.Added);

            try
            {
                if (!RowToDeleteIsNew)
                {
                    //Remove any changes, which may cause validation issues, before cancelling
                    FMyForm.GetJournalsControl().UndoModifiedJournalRow(AJournalRowToCancel, true);
                }

                FMyForm.Cursor = Cursors.WaitCursor;

                CompletionMessage = String.Format(Catalog.GetString("Journal no.: {0} cancelled successfully."),
                    CurrentJournalNumber);

                //Load all data for batch if necessary
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionAndRelatedTablesForJournal(FLedgerNumber, CurrentBatchNumber,
                        CurrentJournalNumber));
                //clear any transactions currently being editied in the Transaction Tab
                FMyForm.GetTransactionsControl().ClearCurrentSelection();
                //Delete transactions and attributes for current jouernal only
                FMyForm.GetTransactionsControl().DeleteTransactionData(CurrentBatchNumber, CurrentJournalNumber);

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

                //Don't run an inactive fields check on this batch
                FMyForm.GetBatchControl().UpdateUnpostedBatchDictionary(CurrentBatchNumber);

                //Need to call save
                if (FMyForm.SaveChangesManual(TGLBatchEnums.GLBatchAction.CANCELLING))
                {
                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("Journal Cancelled"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FMyForm.DisableTransactions();

                    CancellationSuccessful = true;
                }
                else
                {
                    string errMsg = Catalog.GetString("The journal failed to save after being cancelled! Reopen the form and retry.");
                    MessageBox.Show(errMsg, Catalog.GetString("Save Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                //Revert to previous state
                FMainDS.Merge(BackupMainDS);

                CompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    Catalog.GetString("Cancellation Error"),
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