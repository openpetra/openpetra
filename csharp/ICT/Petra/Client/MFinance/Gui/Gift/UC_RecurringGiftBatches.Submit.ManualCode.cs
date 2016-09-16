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

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles Submitting of batches
    /// </summary>
    public class TUC_RecurringGiftBatches_Submit
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFrmRecurringGiftBatch FMyForm = null;

        private Boolean FSubmittingInProgress = false;
        private Int32 FSelectedBatchNumber = 0;

        #region Properties

        /// <summary>
        /// Returns true if Submitting is in progress
        /// </summary>
        public bool SubmittingInProgress
        {
            get
            {
                return FSubmittingInProgress;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_RecurringGiftBatches_Submit(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GiftBatchTDS AMainDS)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FMyForm = (TFrmRecurringGiftBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Main method to Submit a specified batch
        /// </summary>
        /// <param name="ACurrentRecurringBatchRow">The batch row to Submit</param>
        /// <param name="AWarnOfInactiveValues">True means user is warned if inactive values exist</param>
        /// <param name="ADonorZeroIsValid"></param>
        /// <param name="ARecipientZeroIsValid"></param>
        /// <returns>True if the batch was successfully Submited</returns>
        public bool SubmitBatch(ARecurringGiftBatchRow ACurrentRecurringBatchRow,
            bool AWarnOfInactiveValues = true,
            bool ADonorZeroIsValid = false,
            bool ARecipientZeroIsValid = false)
        {
            if (ACurrentRecurringBatchRow == null)
            {
                return false;
            }

            FSelectedBatchNumber = ACurrentRecurringBatchRow.BatchNumber;

            //Make sure that all control data is in dataset
            FMyForm.GetLatestControlData();

            //Copy all batch data to new table
            GiftBatchTDSARecurringGiftDetailTable RecurringBatchGiftDetails = new GiftBatchTDSARecurringGiftDetailTable();
            DataView RecurringGiftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);

            RecurringGiftDetailDV.RowFilter = string.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                FSelectedBatchNumber);

            RecurringGiftDetailDV.Sort = string.Format("{0} ASC, {1} ASC, {2} ASC",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                ARecurringGiftDetailTable.GetDetailNumberDBName());

            foreach (DataRowView dRV in RecurringGiftDetailDV)
            {
                GiftBatchTDSARecurringGiftDetailRow rGBR = (GiftBatchTDSARecurringGiftDetailRow)dRV.Row;
                RecurringBatchGiftDetails.Rows.Add((object[])rGBR.ItemArray.Clone());
            }

            //Save and check for inactive values and ex-workers and anonymous gifts
            if (FPetraUtilsObject.HasChanges)
            {
                //Keep this conditional check separate from the one above so that it only gets called
                // when necessary and doesn't result in the executon of the same method
                if (!FMyForm.SaveChangesForSubmitting(RecurringBatchGiftDetails))
                {
                    return false;
                }
            }
            else
            {
                //This has to be called here because if there are no changes then the DataSavingValidating
                // method which calls the method below, will not run.
                if (!FMyForm.GetBatchControl().AllowInactiveFieldValues(TExtraGiftBatchChecks.GiftBatchAction.SUBMITTING)
                    || FMyForm.GiftHasExWorkerOrAnon(RecurringBatchGiftDetails)
                    )
                {
                    return false;
                }
            }

            //Check hash total validity
            if ((ACurrentRecurringBatchRow.HashTotal != 0)
                && (ACurrentRecurringBatchRow.BatchTotal != ACurrentRecurringBatchRow.HashTotal))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The recurring gift batch total ({0}) for batch {1} does not equal the hash total ({2})."),
                        StringHelper.FormatUsingCurrencyCode(ACurrentRecurringBatchRow.BatchTotal, ACurrentRecurringBatchRow.CurrencyCode),
                        ACurrentRecurringBatchRow.BatchNumber,
                        StringHelper.FormatUsingCurrencyCode(ACurrentRecurringBatchRow.HashTotal, ACurrentRecurringBatchRow.CurrencyCode)),
                    "Submit Recurring Gift Batch");

                FMyForm.GetBatchControl().Controls["txtDetailHashTotal"].Focus();
                FMyForm.GetBatchControl().Controls["txtDetailHashTotal"].Select();
                return false;
            }

            //Check for zero Donors or Recipients
            if (!ADonorZeroIsValid)
            {
                DataView recurringBatchGiftDV = new DataView(FMainDS.ARecurringGift);

                recurringBatchGiftDV.RowFilter = string.Format("{0}={1} And {2}=0",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber,
                    ARecurringGiftTable.GetDonorKeyDBName());

                int numDonorZeros = recurringBatchGiftDV.Count;

                if (numDonorZeros > 0)
                {
                    string messageListOfOffendingGifts =
                        String.Format(Catalog.GetString(
                                "Recurring Gift Batch {0} contains {1} gift detail(s) with Donor 0000000. Please fix before posting!{2}{2}"),
                            FSelectedBatchNumber,
                            numDonorZeros,
                            Environment.NewLine);

                    string listOfOffendingRows = string.Empty;

                    listOfOffendingRows += "Gift" + Environment.NewLine;
                    listOfOffendingRows += "------------";

                    foreach (DataRowView drv in recurringBatchGiftDV)
                    {
                        ARecurringGiftRow giftRow = (ARecurringGiftRow)drv.Row;

                        listOfOffendingRows += String.Format("{0}{1:0000}",
                            Environment.NewLine,
                            giftRow.GiftTransactionNumber);
                    }

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    extendedMessageBox.ShowDialog((messageListOfOffendingGifts + listOfOffendingRows),
                        Catalog.GetString("Submit Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);

                    return false;
                }
            }

            if (!ARecipientZeroIsValid)
            {
                DataView recurringBatchGiftDetailsDV = new DataView(FMainDS.ARecurringGiftDetail);

                recurringBatchGiftDetailsDV.RowFilter = string.Format("{0}={1} And {2}=0",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber,
                    ARecurringGiftDetailTable.GetRecipientKeyDBName());

                int numRecipientZeros = recurringBatchGiftDetailsDV.Count;

                if (numRecipientZeros > 0)
                {
                    string messageListOfOffendingGifts =
                        String.Format(Catalog.GetString(
                                "Recurring Gift Batch {0} contains {1} gift detail(s) with Recipient 0000000. Please fix before posting!{2}{2}"),
                            FSelectedBatchNumber,
                            numRecipientZeros,
                            Environment.NewLine);

                    string listOfOffendingRows = string.Empty;

                    listOfOffendingRows += "Gift   Detail" + Environment.NewLine;
                    listOfOffendingRows += "-------------------";

                    foreach (DataRowView drv in recurringBatchGiftDetailsDV)
                    {
                        ARecurringGiftDetailRow giftDetailRow = (ARecurringGiftDetailRow)drv.Row;

                        listOfOffendingRows += String.Format("{0}{1:0000}  {2:00}",
                            Environment.NewLine,
                            giftDetailRow.GiftTransactionNumber,
                            giftDetailRow.DetailNumber);
                    }

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    extendedMessageBox.ShowDialog((messageListOfOffendingGifts + listOfOffendingRows),
                        Catalog.GetString("Submit Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);

                    return false;
                }
            }

            //Check for inactive KeyMinistries
            DataTable GiftsWithInactiveKeyMinistries;

            if (TRemote.MFinance.Gift.WebConnectors.InactiveKeyMinistriesFoundInBatch(FLedgerNumber, FSelectedBatchNumber,
                    out GiftsWithInactiveKeyMinistries, true))
            {
                int numInactiveValues = GiftsWithInactiveKeyMinistries.Rows.Count;

                string listOfOffendingRows =
                    String.Format(Catalog.GetString(
                            "{0} inactive key ministries found in Recurring Gift Batch {1}. Do you still want to submit?{2}{2}"),
                        numInactiveValues,
                        FSelectedBatchNumber,
                        Environment.NewLine);

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
                }

                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FPetraUtilsObject.GetForm());

                if (extendedMessageBox.ShowDialog(listOfOffendingRows.ToString(),
                        Catalog.GetString("Submit Batch"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbYesNo,
                        TFrmExtendedMessageBox.TIcon.embiWarning) != TFrmExtendedMessageBox.TResult.embrYes)
                {
                    return false;
                }
            }

            TFrmRecurringGiftBatchSubmit SubmitForm = new TFrmRecurringGiftBatchSubmit(FPetraUtilsObject.GetForm());
            try
            {
                FMyForm.ShowInTaskbar = false;
                SubmitForm.MainDS = FMainDS;
                SubmitForm.BatchRow = ACurrentRecurringBatchRow;
                SubmitForm.ShowDialog();
            }
            finally
            {
                SubmitForm.Dispose();
                FMyForm.ShowInTaskbar = true;
            }

            return true;
        }

        #endregion

        #region Private Helper methods


        #endregion
    }
}