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
        /// <param name="ACurrentBatchRow">The row to cancel</param>
        /// <param name="ABatchDescriptionTextBox">Pass a reference to a TextBox that is used to display the batch description text.  This is required
        /// to ensure that validation passes when the cancelled batch is saved.</param>
        /// <returns></returns>
        public bool CancelBatch(ABatchRow ACurrentBatchRow, TextBox ABatchDescriptionTextBox)
        {
            if ((ACurrentBatchRow == null) || !FMyForm.SaveChanges())
            {
                return false;
            }

            int CurrentBatchNumber = ACurrentBatchRow.BatchNumber;

            if ((MessageBox.Show(String.Format(Catalog.GetString("You have chosen to cancel this batch ({0}).\n\nDo you really want to cancel it?"),
                         CurrentBatchNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
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
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionForBatch(FLedgerNumber, CurrentBatchNumber));
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForBatch(FLedgerNumber, CurrentBatchNumber));

                    //Delete transactions
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
                            journal.EndEdit();
                        }
                    }

                    // Edit the batch row
                    ACurrentBatchRow.BeginEdit();

                    //Ensure validation passes
                    if (ACurrentBatchRow.BatchDescription.Length == 0)
                    {
                        ABatchDescriptionTextBox.Text = " ";
                    }

                    ACurrentBatchRow.BatchCreditTotal = 0;
                    ACurrentBatchRow.BatchDebitTotal = 0;
                    ACurrentBatchRow.BatchControlTotal = 0;
                    ACurrentBatchRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;

                    ACurrentBatchRow.EndEdit();

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (FMyForm.SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The batch has been cancelled successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return true;
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The batch has been cancelled but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save the cancellation immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return false;
        }

        #endregion
    }
}