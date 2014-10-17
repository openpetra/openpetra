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
using System.Data;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles posting of batches
    /// </summary>
    public class TUC_GiftBatches_Post
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFrmGiftBatch FMyForm = null;

        private Boolean FPostingInProgress = false;
        private Int32 FSelectedBatchNumber = 0;

        #region Properties

        /// <summary>
        /// Returns true if Posting is in progress
        /// </summary>
        public bool PostingInProgress
        {
            get
            {
                return FPostingInProgress;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_Post(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GiftBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmGiftBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Main method to post a specified batch
        /// </summary>
        /// <param name="ACurrentBatchRow">The batch row to post</param>
        /// <returns>True if the batch was successfully posted</returns>
        public bool PostBatch(AGiftBatchRow ACurrentBatchRow)
        {
            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            FSelectedBatchNumber = ACurrentBatchRow.BatchNumber;
            TVerificationResultCollection Verifications;

            try
            {
                FMyForm.Cursor = Cursors.WaitCursor;

                FMyForm.EnsureGiftDataPresent(FLedgerNumber, FSelectedBatchNumber);

                GiftBatchTDSAGiftDetailTable BatchGiftDetails = new GiftBatchTDSAGiftDetailTable();

                foreach (GiftBatchTDSAGiftDetailRow Row in FMainDS.AGiftDetail.Rows)
                {
                    if (Row.BatchNumber == FSelectedBatchNumber)
                    {
                        BatchGiftDetails.Rows.Add((object[])Row.ItemArray.Clone());
                    }
                }

                // there are no gifts in this batch!
                if (BatchGiftDetails.Rows.Count == 0)
                {
                    FMyForm.Cursor = Cursors.Default;
                    MessageBox.Show(Catalog.GetString("Batch is empty!"), Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                bool CancelledDueToExWorker;

                // save first, then post
                if (!FMyForm.SaveChangesForPosting(BatchGiftDetails, out CancelledDueToExWorker))
                {
                    FMyForm.Cursor = Cursors.Default;

                    if (!CancelledDueToExWorker)
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please first save the batch, and then post it!"));
                    }

                    return false;
                }
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }

            //Check for missing international exchange rate
            bool IsTransactionInIntlCurrency = false;

            if (FMyForm.InternationalCurrencyExchangeRate(ACurrentBatchRow, out IsTransactionInIntlCurrency, true) == 0)
            {
                return false;
            }

            //Check for inactive KeyMinistries
            DataTable GiftsWithInactiveKeyMinistries;

            if (TRemote.MFinance.Gift.WebConnectors.InactiveKeyMinistriesFoundInBatch(FLedgerNumber, FSelectedBatchNumber,
                    out GiftsWithInactiveKeyMinistries))
            {
                string listOfRow = "Gift   Detail   Recipient        KeyMinistry" + Environment.NewLine;
                listOfRow += "------------------------------------------------";

                foreach (DataRow dr in GiftsWithInactiveKeyMinistries.Rows)
                {
                    listOfRow += String.Format("{0}{1:0000}    {2:00}    {3:00000000000}    {4}",
                        Environment.NewLine,
                        dr[0],
                        dr[1],
                        dr[2],
                        dr[3]);
                }

                string msg = String.Format(Catalog.GetString("Cannot post Batch {0} as inactive Key Ministries found in gifts:{1}{1}{2}"),
                    FSelectedBatchNumber,
                    Environment.NewLine,
                    listOfRow);

                MessageBox.Show(msg, Catalog.GetString("Inactive Key Ministries Found"));

                return false;
            }

            // ask if the user really wants to post the batch
            if (MessageBox.Show(String.Format(Catalog.GetString("Do you really want to post gift batch {0}?"),
                        FSelectedBatchNumber),
                    Catalog.GetString("Confirm posting of Gift Batch"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return false;
            }

            Verifications = new TVerificationResultCollection();

            try
            {
                FPostingInProgress = true;

                Thread postingThread = new Thread(() => PostGiftBatch(out Verifications));

                using (TProgressDialog dialog = new TProgressDialog(postingThread))
                {
                    dialog.ShowDialog();
                }

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(Verifications))
                {
                    string ErrorMessages = String.Empty;

                    foreach (TVerificationResult verif in Verifications)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"));

                    return true;
                }
            }
            catch
            {
                //Do nothing
            }
            finally
            {
                FPostingInProgress = false;
            }

            return false;
        }

        #endregion

        #region Private Helper methods

        /// <summary>
        /// executed by progress dialog thread
        /// </summary>
        /// <param name="AVerifications"></param>
        private void PostGiftBatch(out TVerificationResultCollection AVerifications)
        {
            TRemote.MFinance.Gift.WebConnectors.PostGiftBatch(FLedgerNumber, FSelectedBatchNumber, out AVerifications);
        }

        #endregion
    }
}