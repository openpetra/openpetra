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
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles reversing of batches
    /// </summary>
    public class TUC_GLBatches_Reverse
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private IUC_GLBatches FMyUserControl = null;
        private TFrmGLBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GLBatches_Reverse(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GLBatchTDS AMainDS, IUC_GLBatches AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FMyUserControl = AUserControl;

            FMyForm = (TFrmGLBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Main Public methods

        /// <summary>
        /// Reverses the specified batch
        /// </summary>
        /// <param name="ACurrentBatchRow">The DataRow for the batch to be reversed</param>
        /// <param name="ADateForReverseBatch">The reversal date - this will get checked to ensure the date is valid</param>
        /// <param name="AStartDateCurrentPeriod">The earliest date that can be used as reversal date</param>
        /// <param name="AEndDateLastForwardingPeriod">The latest date that can be used as a reversal date</param>
        /// <returns></returns>
        public bool ReverseBatch(ABatchRow ACurrentBatchRow,
            DateTime ADateForReverseBatch,
            DateTime AStartDateCurrentPeriod,
            DateTime AEndDateLastForwardingPeriod)
        {
            //TODO: Allow the user in a dialog to specify the reverse date

            TVerificationResultCollection Verifications;

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!FMyForm.SaveChanges())
                {
                    // saving failed, therefore do not try to reverse
                    MessageBox.Show(Catalog.GetString("The batch was not reversed due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then you can reverse it!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                int SelectedBatchNumber = ACurrentBatchRow.BatchNumber;
                string Msg = string.Empty;

                // load journals belonging to batch
                GLBatchTDS TempDS = TRemote.MFinance.GL.WebConnectors.LoadAJournalAndContent(FLedgerNumber, ACurrentBatchRow.BatchNumber);
                FMainDS.Merge(TempDS);

                foreach (AJournalRow Journal in TempDS.AJournal.Rows)
                {
                    // if at least one journal in the batch has already been reversed then confirm with user
                    if (Journal.Reversed)
                    {
                        Msg = String.Format(Catalog.GetString("One or more of the Journals in Batch {0} have already been reversed. " +
                                "Are you sure you want to continue?"),
                            SelectedBatchNumber);
                        break;
                    }
                }

                if (Msg == string.Empty)
                {
                    Msg = String.Format(Catalog.GetString("Are you sure you want to reverse Batch {0}?"),
                        SelectedBatchNumber);
                }

                if (MessageBox.Show(Msg, Catalog.GetString("GL Batch Reversal"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    TFrmBatchDateDialog Form = new TFrmBatchDateDialog(FMyForm);
                    Form.SetParameters(AStartDateCurrentPeriod, AEndDateLastForwardingPeriod, SelectedBatchNumber);

                    if (Form.ShowDialog() == DialogResult.Cancel)
                    {
                        return false;
                    }

                    ADateForReverseBatch = Form.BatchDate;

                    int ReversalGLBatch;

                    if (!TRemote.MFinance.GL.WebConnectors.ReverseBatch(FLedgerNumber, SelectedBatchNumber,
                            ADateForReverseBatch,
                            out ReversalGLBatch,
                            out Verifications,
                            false))
                    {
                        string ErrorMessages = String.Empty;

                        foreach (TVerificationResult verif in Verifications)
                        {
                            ErrorMessages += "[" + verif.ResultContext + "] " +
                                             verif.ResultTextCaption + ": " +
                                             verif.ResultText + Environment.NewLine;
                        }

                        System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Reversal failed"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            String.Format(Catalog.GetString("A reversal batch has been created, with batch number {0}!"), ReversalGLBatch),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // refresh the grid, to reflect that the batch has been reversed
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndContent(FLedgerNumber, ReversalGLBatch));

                        // make sure that the current dataset is clean,
                        // otherwise the next save would try to modify the posted batch, even though no values have been changed
                        FMainDS.AcceptChanges();

                        // Ensure these tabs will ask the server for updates
                        FMyForm.GetJournalsControl().ClearCurrentSelection();
                        FMyForm.GetTransactionsControl().ClearCurrentSelection();

                        FMyUserControl.UpdateDisplay();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }

            return false;
        }

        #endregion
    }
}