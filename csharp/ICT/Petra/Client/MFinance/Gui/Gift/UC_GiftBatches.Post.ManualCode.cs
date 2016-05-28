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
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

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
        /// <param name="APostingAlreadyConfirmed">True means ask user if they want to post</param>
        /// <param name="AWarnOfInactiveValues">True means user is warned if inactive values exist</param>
        /// <param name="ADonorZeroIsValid"></param>
        /// <param name="ARecipientZeroIsValid"></param>
        /// <returns>True if the batch was successfully posted</returns>
        public bool PostBatch(AGiftBatchRow ACurrentBatchRow,
            bool APostingAlreadyConfirmed = false,
            bool AWarnOfInactiveValues = true,
            bool ADonorZeroIsValid = false,
            bool ARecipientZeroIsValid = false)
        {
            //This assumes that all gift data etc is loaded into the batch before arriving here

            bool RetVal = false;

            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return RetVal;
            }

            FSelectedBatchNumber = ACurrentBatchRow.BatchNumber;
            TVerificationResultCollection Verifications;

            try
            {
                //Make sure that all control data is in dataset
                FMyForm.GetControlDataForPosting();

                GiftBatchTDSAGiftDetailTable batchGiftDetails = new GiftBatchTDSAGiftDetailTable();
                DataView batchGiftDetailsDV = new DataView(FMainDS.AGiftDetail);

                batchGiftDetailsDV.RowFilter = string.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber);

                batchGiftDetailsDV.Sort = string.Format("{0} ASC, {1} ASC, {2} ASC",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView drv in batchGiftDetailsDV)
                {
                    GiftBatchTDSAGiftDetailRow gBRow = (GiftBatchTDSAGiftDetailRow)drv.Row;

                    batchGiftDetails.Rows.Add((object[])gBRow.ItemArray.Clone());
                }

                bool CancelledDueToExWorkerOrAnonDonor;

                // save first, then post
                if (!FMyForm.SaveChangesForPosting(batchGiftDetails, out CancelledDueToExWorkerOrAnonDonor))
                {
                    FMyForm.Cursor = Cursors.Default;

                    if (!CancelledDueToExWorkerOrAnonDonor)
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please first correct and save the batch, and then post it!"));
                    }

                    return RetVal;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            //Check for missing international exchange rate
            bool IsTransactionInIntlCurrency = false;

            if (FMyForm.InternationalCurrencyExchangeRate(ACurrentBatchRow, out IsTransactionInIntlCurrency, true) == 0)
            {
                return RetVal;
            }

            //Check for zero Donors or Recipients
            if (!ADonorZeroIsValid)
            {
                DataView batchGiftDV = new DataView(FMainDS.AGift);

                batchGiftDV.RowFilter = string.Format("{0}={1} And {2}=0",
                    AGiftTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber,
                    AGiftTable.GetDonorKeyDBName());

                int numDonorZeros = batchGiftDV.Count;

                if (numDonorZeros > 0)
                {
                    string messageListOfOffendingGifts =
                        String.Format(Catalog.GetString(
                                "Gift Batch {0} contains {1} gift detail(s) with Donor 0000000. Please fix before posting!{2}{2}"),
                            FSelectedBatchNumber,
                            numDonorZeros,
                            Environment.NewLine);

                    string listOfOffendingRows = string.Empty;

                    listOfOffendingRows += "Gift" + Environment.NewLine;
                    listOfOffendingRows += "------------";

                    foreach (DataRowView drv in batchGiftDV)
                    {
                        AGiftRow giftRow = (AGiftRow)drv.Row;

                        listOfOffendingRows += String.Format("{0}{1:0000}",
                            Environment.NewLine,
                            giftRow.GiftTransactionNumber);
                    }

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    extendedMessageBox.ShowDialog((messageListOfOffendingGifts + listOfOffendingRows),
                        Catalog.GetString("Post Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);

                    return RetVal;
                }
            }

            if (!ARecipientZeroIsValid)
            {
                DataView batchGiftDetailsDV = new DataView(FMainDS.AGiftDetail);

                batchGiftDetailsDV.RowFilter = string.Format("{0}={1} And {2}=0",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber,
                    AGiftDetailTable.GetRecipientKeyDBName());

                int numRecipientZeros = batchGiftDetailsDV.Count;

                if (numRecipientZeros > 0)
                {
                    string messageListOfOffendingGifts =
                        String.Format(Catalog.GetString(
                                "Gift Batch {0} contains {1} gift detail(s) with Recipient 0000000. Please fix before posting!{2}{2}"),
                            FSelectedBatchNumber,
                            numRecipientZeros,
                            Environment.NewLine);

                    string listOfOffendingRows = string.Empty;

                    listOfOffendingRows += "Gift   Detail" + Environment.NewLine;
                    listOfOffendingRows += "-------------------";

                    foreach (DataRowView drv in batchGiftDetailsDV)
                    {
                        AGiftDetailRow giftDetailRow = (AGiftDetailRow)drv.Row;

                        listOfOffendingRows += String.Format("{0}{1:0000}  {2:00}",
                            Environment.NewLine,
                            giftDetailRow.GiftTransactionNumber,
                            giftDetailRow.DetailNumber);
                    }

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    extendedMessageBox.ShowDialog((messageListOfOffendingGifts + listOfOffendingRows),
                        Catalog.GetString("Post Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);

                    return RetVal;
                }
            }

            //Check for inactive KeyMinistries
            DataTable GiftsWithInactiveKeyMinistries;
            bool ModifiedDetails = false;

            if (AWarnOfInactiveValues && TRemote.MFinance.Gift.WebConnectors.InactiveKeyMinistriesFoundInBatch(FLedgerNumber, FSelectedBatchNumber,
                    out GiftsWithInactiveKeyMinistries, false))
            {
                int numInactiveValues = GiftsWithInactiveKeyMinistries.Rows.Count;

                string messageNonModifiedBatch =
                    String.Format(Catalog.GetString("Gift Batch {0} contains {1} inactive key ministries. Please fix before posting!{2}{2}"),
                        FSelectedBatchNumber,
                        numInactiveValues,
                        Environment.NewLine);
                string messageModifiedBatch =
                    String.Format(Catalog.GetString(
                            "Reversal/Adjustment Gift Batch {0} contains {1} inactive key ministries. Do you still want to post?{2}{2}"),
                        FSelectedBatchNumber,
                        numInactiveValues,
                        Environment.NewLine);

                string listOfOffendingRows = string.Empty;

                listOfOffendingRows += "Gift      Detail   Recipient          KeyMinistry" + Environment.NewLine;
                listOfOffendingRows += "-------------------------------------------------------------------------------";

                foreach (DataRow dr in GiftsWithInactiveKeyMinistries.Rows)
                {
                    listOfOffendingRows += String.Format("{0}{1:0000}    {2:00}        {3:00000000000}    {4}",
                        Environment.NewLine,
                        dr[0],
                        dr[1],
                        dr[2],
                        dr[3]);

                    bool isModified = Convert.ToBoolean(dr[4]);

                    if (isModified)
                    {
                        ModifiedDetails = true;
                    }
                }

                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                if (ModifiedDetails)
                {
                    if (extendedMessageBox.ShowDialog((messageModifiedBatch + listOfOffendingRows),
                            Catalog.GetString("Post Batch"), string.Empty,
                            TFrmExtendedMessageBox.TButtons.embbYesNo,
                            TFrmExtendedMessageBox.TIcon.embiWarning) == TFrmExtendedMessageBox.TResult.embrYes)
                    {
                        APostingAlreadyConfirmed = true;
                    }
                    else
                    {
                        return RetVal;
                    }
                }
                else
                {
                    extendedMessageBox.ShowDialog((messageNonModifiedBatch + listOfOffendingRows),
                        Catalog.GetString("Post Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);

                    return RetVal;
                }
            }

            // ask if the user really wants to post the batch
            if (!APostingAlreadyConfirmed
                && (MessageBox.Show(String.Format(Catalog.GetString("Do you really want to post gift batch {0}?"), FSelectedBatchNumber),
                        Catalog.GetString("Confirm posting of Gift Batch"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
            {
                return RetVal;
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
                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    StringBuilder errorMessages = new StringBuilder();
                    int counter = 0;

                    errorMessages.AppendLine(Catalog.GetString("________________________Gift Posting Errors________________________"));
                    errorMessages.AppendLine();

                    foreach (TVerificationResult verif in Verifications)
                    {
                        counter++;
                        errorMessages.AppendLine(counter.ToString("000") + " - " + verif.ResultText);
                        errorMessages.AppendLine();
                    }

                    extendedMessageBox.ShowDialog(errorMessages.ToString(),
                        Catalog.GetString("Post Batch Error"),
                        string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"));

                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FPostingInProgress = false;
            }

            return RetVal;
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