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

            if (AJournalRowToCancel == null)
            {
                return CancellationSuccessful;
            }

            int CurrentBatchNumber = AJournalRowToCancel.BatchNumber;
            int CurrentJournalNumber = AJournalRowToCancel.JournalNumber;

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to cancel this journal ({0}).\n\nDo you really want to cancel it?"),
                         CurrentJournalNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return CancellationSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = (GLBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            bool RowToDeleteIsNew = (AJournalRowToCancel.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew)
            {
                //Remove any changes, which may cause validation issues, before cancelling
                FMyForm.GetJournalsControl().UndoModifiedJournalRow(AJournalRowToCancel, true);

                if (!(FMyForm.SaveChanges()))
                {
                    MessageBox.Show(Catalog.GetString("Error in trying to save prior to cancelling current journal!"),
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

                //Clear Journals etc for current Batch
                FMainDS.ATransAnalAttrib.Clear();
                FMainDS.ATransaction.Clear();

                //Load data afresh
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber, CurrentBatchNumber,
                        CurrentJournalNumber));
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

                //Edit current Journal
                AJournalRowToCancel.BeginEdit();

                AJournalRowToCancel.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                AJournalRowToCancel.LastTransactionNumber = 0;

                //Edit current Batch
                ABatchRow CurrentBatchRow = FMyForm.GetBatchControl().GetSelectedDetailRow();

                CurrentBatchRow.BatchCreditTotal -= AJournalRowToCancel.JournalCreditTotal;
                CurrentBatchRow.BatchDebitTotal -= AJournalRowToCancel.JournalDebitTotal;

                if (CurrentBatchRow.BatchControlTotal != 0)
                {
                    CurrentBatchRow.BatchControlTotal -= AJournalRowToCancel.JournalCreditTotal;
                }

                AJournalRowToCancel.JournalCreditTotal = 0;
                AJournalRowToCancel.JournalDebitTotal = 0;

                AJournalRowToCancel.EndEdit();

                //Need to call save
                if (FMyForm.SaveChanges())
                {
                    MessageBox.Show(Catalog.GetString("The journal has been cancelled successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FMyForm.DisableTransactions();

                    CancellationSuccessful = true;
                }
                else
                {
                    // saving failed
                    throw new Exception(Catalog.GetString("The journal failed to save after being cancelled! Reopen the form and retry."));
                }
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