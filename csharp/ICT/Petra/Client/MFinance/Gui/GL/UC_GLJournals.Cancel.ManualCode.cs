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
        /// <param name="ACurrentJournalRow">The row to cancel</param>
        /// <param name="AJournalDescriptionTextBox">Pass a reference to a TextBox that is used to display the journal description text.  This is required
        /// to ensure that validation passes when the cancelled journal is saved.</param>
        /// <param name="AExchangeRateToBaseTextBox">Pass a reference to a TextBox that is used to display the exchange rate to base.  This is required
        /// to ensure that validation passes when the cancelled journal is saved.</param>
        /// <returns>True if the journal is cancelled.</returns>
        public bool CancelRow(GLBatchTDSAJournalRow ACurrentJournalRow,
            TextBox AJournalDescriptionTextBox,
            TTxtNumericTextBox AExchangeRateToBaseTextBox)
        {
            if ((ACurrentJournalRow == null) || !FMyForm.SaveChanges())
            {
                return false;
            }

            int CurrentBatchNumber = ACurrentJournalRow.BatchNumber;
            int CurrentJournalNumber = ACurrentJournalRow.JournalNumber;

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to cancel this journal ({0}).\n\nDo you really want to cancel it?"),
                         CurrentJournalNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    //clear any transactions currently being editied in the Transaction Tab
                    FMyForm.GetTransactionsControl().ClearCurrentSelection();

                    //Load any new data
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber, CurrentBatchNumber,
                            CurrentJournalNumber));

                    DataView dvAA = new DataView(FMainDS.ATransAnalAttrib);

                    dvAA.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransAnalAttribTable.GetBatchNumberDBName(),
                        CurrentBatchNumber,
                        ATransAnalAttribTable.GetJournalNumberDBName(),
                        CurrentJournalNumber);

                    //Delete Analysis Attribs
                    foreach (DataRowView dvr in dvAA)
                    {
                        dvr.Delete();
                    }

                    DataView dvTr = new DataView(FMainDS.ATransaction);

                    dvTr.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        CurrentBatchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        CurrentJournalNumber);

                    //Delete Transactions
                    foreach (DataRowView dvr in dvTr)
                    {
                        dvr.Delete();
                    }

                    ACurrentJournalRow.BeginEdit();

                    ACurrentJournalRow.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                    ACurrentJournalRow.LastTransactionNumber = 0;

                    //Ensure validation passes
                    if (ACurrentJournalRow.JournalDescription.Length == 0)
                    {
                        AJournalDescriptionTextBox.Text = " ";
                    }

                    if (ACurrentJournalRow.ExchangeRateToBase == 0)
                    {
                        AExchangeRateToBaseTextBox.NumberValueDecimal = 1;
                    }

                    ABatchRow CurrentBatchRow = FMyForm.GetBatchControl().GetSelectedDetailRow();

                    CurrentBatchRow.BatchCreditTotal -= ACurrentJournalRow.JournalCreditTotal;
                    CurrentBatchRow.BatchDebitTotal -= ACurrentJournalRow.JournalDebitTotal;

                    if (CurrentBatchRow.BatchControlTotal != 0)
                    {
                        CurrentBatchRow.BatchControlTotal -= ACurrentJournalRow.JournalCreditTotal;
                    }

                    ACurrentJournalRow.JournalCreditTotal = 0;
                    ACurrentJournalRow.JournalDebitTotal = 0;

                    ACurrentJournalRow.EndEdit();

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (FMyForm.SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The journal has been cancelled successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FMyForm.DisableTransactions();
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The journal has been cancelled but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save the cancellation immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return true;
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