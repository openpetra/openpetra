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
    /// A business logic class that handles Submitting of batches
    /// </summary>
    public class TUC_RecurringGiftBatches_Submit
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TFrmRecurringGiftBatch FMyForm = null;

        private Boolean FSubmittingInProgress = false;

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
        /// <param name="ATxtDetailHashTotal">True means ask user if they want to Submit</param>
        /// <param name="ASubmittingAlreadyConfirmed">True means ask user if they want to Submit</param>
        /// <returns>True if the batch was successfully Submited</returns>
        public bool SubmitBatch(ARecurringGiftBatchRow ACurrentRecurringBatchRow,
            Ict.Common.Controls.TTxtCurrencyTextBox ATxtDetailHashTotal,
            ref bool ASubmittingAlreadyConfirmed)
        {
            int SelectedBatchNumber = ACurrentRecurringBatchRow.BatchNumber;

            GiftBatchTDSARecurringGiftDetailTable RecurringBatchGiftDetails = new GiftBatchTDSARecurringGiftDetailTable();

            DataView RecurringGiftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);

            RecurringGiftDetailDV.RowFilter = string.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                SelectedBatchNumber);

            foreach (DataRowView dRV in RecurringGiftDetailDV)
            {
                GiftBatchTDSARecurringGiftDetailRow rGBR = (GiftBatchTDSARecurringGiftDetailRow)dRV.Row;
                RecurringBatchGiftDetails.Rows.Add((object[])rGBR.ItemArray.Clone());
            }

            if (FPetraUtilsObject.HasChanges)
            {
                bool CancelledDueToExWorker;

                // save first, then submit
                if (!FMyForm.SaveChangesForSubmitting(RecurringBatchGiftDetails, out CancelledDueToExWorker))
                {
                    if (!CancelledDueToExWorker)
                    {
                        // saving failed, therefore do not try to submit
                        MessageBox.Show(Catalog.GetString(
                                "The recurring batch was not submitted due to problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please fix the batch first and then submit it."),
                            Catalog.GetString("Submit Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return false;
                }
            }

            if ((ACurrentRecurringBatchRow.HashTotal != 0)
                && (ACurrentRecurringBatchRow.BatchTotal != ACurrentRecurringBatchRow.HashTotal))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The recurring gift batch total ({0}) for batch {1} does not equal the hash total ({2})."),
                        StringHelper.FormatUsingCurrencyCode(ACurrentRecurringBatchRow.BatchTotal, ACurrentRecurringBatchRow.CurrencyCode),
                        ACurrentRecurringBatchRow.BatchNumber,
                        StringHelper.FormatUsingCurrencyCode(ACurrentRecurringBatchRow.HashTotal, ACurrentRecurringBatchRow.CurrencyCode)),
                    "Submit Recurring Gift Batch");

                ATxtDetailHashTotal.Focus();
                ATxtDetailHashTotal.SelectAll();
                return false;
            }

            //Check for inactive KeyMinistries
            DataTable GiftsWithInactiveKeyMinistries;

            if (TRemote.MFinance.Gift.WebConnectors.InactiveKeyMinistriesFoundInBatch(FLedgerNumber, SelectedBatchNumber,
                    out GiftsWithInactiveKeyMinistries, true))
            {
                int numInactiveValues = GiftsWithInactiveKeyMinistries.Rows.Count;

                string listOfOffendingRows =
                    String.Format(Catalog.GetString(
                            "{0} inactive key ministries found in Recurring Gift Batch {1}. Do you still want to submit?{2}{2}"),
                        numInactiveValues,
                        SelectedBatchNumber,
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