//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2012 by OM International
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
#region usings

using System;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftTransactions
    {
        #region get data

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AForceLoadFromServer">Set to true to get data from the server even though it is apparently the current batch number and status</param>
        /// <returns>True if new gift transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadRecurringGifts(Int32 ALedgerNumber, Int32 ABatchNumber, bool AForceLoadFromServer = false)
        {
            //Set key flags
            bool FirstGiftTransLoad = (FLedgerNumber == -1);
            bool SameCurrentBatch = ((FLedgerNumber == ALedgerNumber)
                                     && (FBatchNumber == ABatchNumber)
                                     && !AForceLoadFromServer);

            FBatchRow = GetRecurringBatchRow();

            if ((FBatchRow == null) && (GetAnyRecurringBatchRow(ABatchNumber) == null))
            {
                MessageBox.Show(String.Format("Cannot load transactions for Recurring Gift Batch {0} as the batch is not currently loaded!",
                        ABatchNumber));
                return false;
            }

            //Set key values from Batch
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchCurrencyCode = FBatchRow.CurrencyCode;
            FBatchMethodOfPayment = FBatchRow.MethodOfPaymentCode;

            if (FirstGiftTransLoad)
            {
                InitialiseControls();
            }

            UpdateCurrencySymbols(FBatchCurrencyCode);

            //Check if the same batch is selected, so no need to apply filter
            if (SameCurrentBatch)
            {
                //Same as previously selected
                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                return false;
            }

            //New Batch
            FCurrentGiftInBatch = 0;

            //New set of transactions to be loaded
            TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());

            if (FShowStatusDialogOnLoadFlag == true)
            {
                dlgStatus.Show();
                FShowStatusDialogOnLoadFlag = false;
                dlgStatus.Heading = String.Format(Catalog.GetString("Recurring Batch {0}"), ABatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading transactions ...");
            }

            FGiftTransactionsLoadedFlag = false;
            FSuppressListChangedFlag = false;

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;

            // if this form is readonly, then we need all codes, because old codes might have been used
            if (FirstGiftTransLoad)
            {
                //Recurring, like posted, shows all historical data
                FActiveOnlyFlag = false;
                dlgStatus.CurrentStatus = Catalog.GetString("Initialising controls ...");

                try
                {
                    //Without this, the Save button enables when it shouldn't!
                    FPetraUtilsObject.SuppressChangeDetection = true;

                    TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, FActiveOnlyFlag);
                    TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetailCode, FLedgerNumber, FActiveOnlyFlag);
                    TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, FActiveOnlyFlag);
                    TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, FActiveOnlyFlag);
                    TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, FActiveOnlyFlag);
                }
                finally
                {
                    FPetraUtilsObject.SuppressChangeDetection = false;
                }
            }

            // This sets the incomplete filter but does check the panel enabled state
            ShowData();

            // This sets the main part of the filter but excluding the additional items set by the user GUI
            // It gets the right sort order
            SetGiftDetailDefaultView();

            // only load from server if there are no transactions loaded yet for this batch
            // otherwise we would overwrite transactions that have already been modified
            if (FMainDS.ARecurringGiftDetail.DefaultView.Count == 0)
            {
                dlgStatus.CurrentStatus = Catalog.GetString("Requesting transactions from server ...");
                //Load all partners in Batch
                FMainDS.DonorPartners.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAllPartnerDataForBatch(ALedgerNumber, ABatchNumber)); //LoadAllPartnerDataForBatch();
                //Include Donor fields
                LoadGiftDataForBatch(ALedgerNumber, ABatchNumber);
            }

            // Now we set the full filter
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            dlgStatus.CurrentStatus = Catalog.GetString("Updating totals for the batch ...");
            UpdateTotals();

            if ((FPreviouslySelectedDetailRow != null))
            {
                bool disableSave = (FBatchRow.RowState == DataRowState.Unchanged && !FPetraUtilsObject.HasChanges);

                if (disableSave && FPetraUtilsObject.HasChanges && !DataUtilities.DataRowColumnsHaveChanged(FBatchRow))
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }

            FGiftTransactionsLoadedFlag = true;
            dlgStatus.Close();

            return true;
        }

        /// <summary>
        /// Ensure the data is loaded for the specified batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>If transactions exist</returns>
        private bool LoadGiftDataForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool RetVal = ((TFrmRecurringGiftBatch)ParentForm).EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);

            UpdateAllRecipientDescriptions(ABatchNumber);

            UpdateAllDonorNames(ABatchNumber);

            return RetVal;
        }

        /// <summary>
        /// Refresh the dataset for this form
        /// </summary>
        public void RefreshData()
        {
            Cursor PrevCursor = ParentForm.Cursor;

            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;

                if ((FMainDS != null) && (FMainDS.ARecurringGiftDetail != null))
                {
                    FMainDS.ARecurringGift.Rows.Clear();
                    FMainDS.ARecurringGiftDetail.Rows.Clear();
                }

                // Get the current batch row from the batch tab
                FBatchRow = GetRecurringBatchRow();

                if (FBatchRow != null)
                {
                    // Be sure to pass the true parameter because we definitely need to update FMainDS.ARecurringGiftDetail as it is now empty!
                    LoadRecurringGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, true);
                }
            }
            finally
            {
                ParentForm.Cursor = PrevCursor;
            }
        }

        /// <summary>
        /// Clear the gift data of the current batch without marking records for delete
        /// </summary>
        private bool RefreshRecurringBatchGiftData(Int32 ABatchNumber,
            bool AAcceptChanges = false,
            bool AHandleDataSetBackup = false)
        {
            bool RetVal = false;

            //Copy and backup the current dataset
            GiftBatchTDS BackupDS = null;
            GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();

            TempDS.Merge(FMainDS);

            if (AHandleDataSetBackup)
            {
                BackupDS = (GiftBatchTDS)FMainDS.GetChangesTyped(false);
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Remove current batch gift data
                DataView giftDetailView = new DataView(TempDS.ARecurringGiftDetail);

                giftDetailView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    ARecurringGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView dr in giftDetailView)
                {
                    dr.Delete();
                }

                DataView giftView = new DataView(TempDS.ARecurringGift);

                giftView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftView.Sort = String.Format("{0} DESC",
                    ARecurringGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView dr in giftView)
                {
                    dr.Delete();
                }

                TempDS.AcceptChanges();

                //Clear all gift data from Main dataset gift tables
                FMainDS.ARecurringGiftDetail.Clear();
                FMainDS.ARecurringGift.Clear();

                //Bring data back in from other batches if it exists
                if (TempDS.ARecurringGift.Count > 0)
                {
                    FMainDS.ARecurringGift.Merge(TempDS.ARecurringGift);
                    FMainDS.ARecurringGiftDetail.Merge(TempDS.ARecurringGiftDetail);
                }

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringGiftTransactionsForBatch(FLedgerNumber, ABatchNumber));

                if (AAcceptChanges)
                {
                    FMainDS.AcceptChanges();
                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                //If not revert on error then calling method will
                if (AHandleDataSetBackup)
                {
                    RevertDataSet(FMainDS, BackupDS);
                }

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return RetVal;
        }

        private ARecurringGiftBatchRow GetRecurringBatchRow()
        {
            return ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        private ARecurringGiftBatchRow GetAnyRecurringBatchRow(Int32 ABatchNumber)
        {
            return ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().GetAnyRecurringBatchRow(ABatchNumber);
        }

        private ARecurringGiftRow GetRecurringGiftRow(Int32 ARecurringGiftTransactionNumber)
        {
            return (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, ARecurringGiftTransactionNumber });
        }

        private GiftBatchTDSARecurringGiftDetailRow GetRecurringGiftDetailRow(Int32 ARecurringGiftTransactionNumber, Int32 ARecurringGiftDetailNumber)
        {
            return (GiftBatchTDSARecurringGiftDetailRow)FMainDS.ARecurringGiftDetail.Rows.Find(new object[] { FLedgerNumber,
                                                                                                              FBatchNumber,
                                                                                                              ARecurringGiftTransactionNumber,
                                                                                                              ARecurringGiftDetailNumber });
        }

        #endregion get data

        #region data handling

        /// <summary>
        /// Select a special gift detail number from outside
        /// </summary>
        /// <param name="ARecurringGiftNumber"></param>
        /// <param name="ARecurringGiftDetailNumber"></param>
        public void SelectRecurringGiftDetailNumber(Int32 ARecurringGiftNumber, Int32 ARecurringGiftDetailNumber)
        {
            DataView myView = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView;

            for (int counter = 0; (counter < myView.Count); counter++)
            {
                int myViewGiftNumber = (int)myView[counter][2];
                int myViewGiftDetailNumber = (int)(int)myView[counter][3];

                if ((myViewGiftNumber == ARecurringGiftNumber) && (myViewGiftDetailNumber == ARecurringGiftDetailNumber))
                {
                    SelectRowInGrid(counter + 1);
                    break;
                }
            }
        }

        private void SetGiftDetailDefaultView()
        {
            if (FBatchNumber != -1)
            {
                string rowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.ARecurringGiftDetail.DefaultView.Sort = string.Format("{0}, {1}",
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    ARecurringGiftDetailTable.GetDetailNumberDBName());

                FMainDS.ARecurringGift.DefaultView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    FBatchNumber);
            }
        }

        private void AutoPopulateComment(string AAutoPopComment)
        {
            if (string.IsNullOrEmpty(txtDetailGiftCommentOne.Text))
            {
                txtDetailGiftCommentOne.Text = AAutoPopComment;
                cmbDetailCommentOneType.SetSelectedString("Both", -1);
            }
            else if (string.IsNullOrEmpty(txtDetailGiftCommentTwo.Text))
            {
                txtDetailGiftCommentTwo.Text = AAutoPopComment;
                cmbDetailCommentTwoType.SetSelectedString("Both", -1);
            }
            else if (string.IsNullOrEmpty(txtDetailGiftCommentThree.Text))
            {
                txtDetailGiftCommentThree.Text = AAutoPopComment;
                cmbDetailCommentThreeType.SetSelectedString("Both", -1);
            }
            else
            {
                if (MessageBox.Show(string.Format(Catalog.GetString(
                                "This Motivation Detail is set to auto populate a gift comment field, but all the comment fields are currently full."
                                +
                                " Do you want to overwrite Comment 1?{0}{0}" +
                                "'No' will keep the current comment,{0}" +
                                "'Yes' will copy Comment 1 to the clipboard and replace it with the automated comment '{1}'"),
                            "\n", AAutoPopComment),
                        Catalog.GetString("Auto Populate Gift Comment"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    Clipboard.SetText(txtDetailGiftCommentOne.Text);
                    txtDetailGiftCommentOne.Text = AAutoPopComment;
                    cmbDetailCommentOneType.SetSelectedString("Both", -1);
                }
            }
        }

        private void RemoveAutoPopulatedComment(string AAutoPopComment)
        {
            if (!string.IsNullOrEmpty(txtDetailGiftCommentOne.Text)
                && (txtDetailGiftCommentOne.Text == AAutoPopComment))
            {
                txtDetailGiftCommentOne.Text = string.Empty;
                cmbDetailCommentOneType.SelectedIndex = -1;
            }
            else if (!string.IsNullOrEmpty(txtDetailGiftCommentTwo.Text)
                     && (txtDetailGiftCommentTwo.Text == AAutoPopComment))
            {
                txtDetailGiftCommentTwo.Text = string.Empty;
                cmbDetailCommentTwoType.SelectedIndex = -1;
            }
            else if (!string.IsNullOrEmpty(txtDetailGiftCommentThree.Text)
                     && (txtDetailGiftCommentThree.Text == AAutoPopComment))
            {
                txtDetailGiftCommentThree.Text = string.Empty;
                cmbDetailCommentThreeType.SelectedIndex = -1;
            }
        }

        private void ProcessGiftAmountChange()
        {
            FGiftAmountChangedInProcess = false;

            if (txtDetailGiftAmount.NumberValueDecimal == null)
            {
                return;
            }

            decimal NewAmount = txtDetailGiftAmount.NumberValueDecimal.Value;

            if (FPreviouslySelectedDetailRow != null)
            {
                FPreviouslySelectedDetailRow.GiftAmount = NewAmount;
            }

            UpdateTotals();
        }

        private void SetBatchLastGiftNumber()
        {
            DataView dv = new DataView(FMainDS.ARecurringGift);

            dv.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                FBatchNumber);

            dv.Sort = String.Format("{0} DESC",
                ARecurringGiftTable.GetGiftTransactionNumberDBName());

            dv.RowStateFilter = DataViewRowState.CurrentRows;

            if (dv.Count > 0)
            {
                ARecurringGiftRow transRow = (ARecurringGiftRow)dv[0].Row;
                FBatchRow.LastGiftNumber = transRow.GiftTransactionNumber;
            }
            else
            {
                FBatchRow.LastGiftNumber = 0;
            }
        }

        private String GetMethodOfPaymentFromBatch()
        {
            if (FBatchMethodOfPayment == string.Empty)
            {
                FBatchMethodOfPayment = ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
            }

            return FBatchMethodOfPayment;
        }

        private Boolean BatchHasMethodOfPayment()
        {
            string BatchMoP = GetMethodOfPaymentFromBatch();

            return BatchMoP != null && BatchMoP.Length > 0;
        }

        #endregion data handling

        #region field updates

        /// <summary>
        /// set the currency symbols for the currency field from outside
        /// </summary>
        public void UpdateCurrencySymbols(String ACurrencyCode)
        {
            if (txtDetailGiftAmount.CurrencyCode != ACurrencyCode)
            {
                txtDetailGiftAmount.CurrencyCode = ACurrencyCode;
            }

            if ((txtGiftTotal.CurrencyCode != ACurrencyCode)
                || (txtBatchTotal.CurrencyCode != ACurrencyCode)
                || (txtHashTotal.CurrencyCode != ACurrencyCode))
            {
                txtGiftTotal.CurrencyCode = ACurrencyCode;
                txtBatchTotal.CurrencyCode = ACurrencyCode;
                txtHashTotal.CurrencyCode = ACurrencyCode;
            }
        }

        /// <summary>
        /// Update the transaction method payment from outside
        /// </summary>
        public void UpdateMethodOfPayment()
        {
            Int32 LedgerNumber;
            Int32 BatchNumber;

            if (!((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetRecurringBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().MethodOfPaymentCode;

            LedgerNumber = FBatchRow.LedgerNumber;
            BatchNumber = FBatchRow.BatchNumber;

            if (!LoadGiftDataForBatch(LedgerNumber, BatchNumber))
            {
                //No transactions exist to process or corporate exchange rate not found
                return;
            }

            if ((FLedgerNumber == LedgerNumber) && (FBatchNumber == BatchNumber))
            {
                //Rows already active in transaction tab. Need to set current row ac code below will not update selected row
                if (FPreviouslySelectedDetailRow != null)
                {
                    FPreviouslySelectedDetailRow.MethodOfPaymentCode = FBatchMethodOfPayment;
                    cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment);
                }
            }

            //Update all transactions
            DataView GiftView = new DataView(FMainDS.ARecurringGift);

            GiftView.RowStateFilter = DataViewRowState.CurrentRows;
            GiftView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drv in GiftView)
            {
                ARecurringGiftRow giftRow = (ARecurringGiftRow)drv.Row;
                giftRow.MethodOfPaymentCode = FBatchMethodOfPayment;
            }

            //Do same at detail level to update the grid
            DataView GiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);

            GiftDetailView.RowStateFilter = DataViewRowState.CurrentRows;
            GiftDetailView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drv in GiftDetailView)
            {
                GiftBatchTDSARecurringGiftDetailRow giftDetailRow = (GiftBatchTDSARecurringGiftDetailRow)drv.Row;
                giftDetailRow.MethodOfPaymentCode = FBatchMethodOfPayment;
            }
        }

        /// <summary>
        /// Set the Hash Total symbols for the currency field from outside
        /// </summary>
        public void UpdateHashTotal(Decimal AHashTotal)
        {
            txtHashTotal.NumberValueDecimal = AHashTotal;
        }

        private void UpdateTotals()
        {
            if ((FPetraUtilsObject == null))
            {
                return;
            }

            Decimal SumTransactions = 0;
            Decimal SumBatch = 0;
            Int32 GiftNumber = 0;

            //Sometimes a change in an unbound textbox causes a data changed condition
            bool SaveButtonWasEnabled = FPetraUtilsObject.HasChanges;
            bool DataChanges = false;

            if (FPreviouslySelectedDetailRow == null)
            {
                if ((txtGiftTotal.NumberValueDecimal.HasValue && (txtGiftTotal.NumberValueDecimal.Value != 0))
                    || (txtBatchTotal.NumberValueDecimal.HasValue && (txtBatchTotal.NumberValueDecimal.Value != 0)))
                {
                    txtGiftTotal.NumberValueDecimal = 0;
                    txtBatchTotal.NumberValueDecimal = 0;
                }

                //If all details have been deleted
                if ((FLedgerNumber != -1) && (FBatchRow != null) && (grdDetails.Rows.Count == 1))
                {
                    //((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
                    //Now we look at the batch and update the batch data
                    if (FBatchRow.BatchTotal != SumBatch)
                    {
                        FBatchRow.BatchTotal = SumBatch;
                        DataChanges = true;
                    }
                }
            }
            else
            {
                GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

                DataView giftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);
                giftDetailDV.RowStateFilter = DataViewRowState.CurrentRows;

                giftDetailDV.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);

                foreach (DataRowView drv in giftDetailDV)
                {
                    ARecurringGiftDetailRow gdr = (ARecurringGiftDetailRow)drv.Row;

                    if (gdr.GiftTransactionNumber == GiftNumber)
                    {
                        if (FPreviouslySelectedDetailRow.DetailNumber == gdr.DetailNumber)
                        {
                            SumTransactions += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                            SumBatch += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                        }
                        else
                        {
                            SumTransactions += gdr.GiftAmount;
                            SumBatch += gdr.GiftAmount;
                        }
                    }
                    else
                    {
                        SumBatch += gdr.GiftAmount;
                    }
                }

                if ((txtGiftTotal.NumberValueDecimal.HasValue == false) || (txtGiftTotal.NumberValueDecimal.Value != SumTransactions))
                {
                    txtGiftTotal.NumberValueDecimal = SumTransactions;
                }

                txtGiftTotal.CurrencyCode = txtDetailGiftAmount.CurrencyCode;
                txtGiftTotal.ReadOnly = true;

                //Now we look at the batch and update the batch data
                if (FBatchRow.BatchTotal != SumBatch)
                {
                    FBatchRow.BatchTotal = SumBatch;
                    DataChanges = true;
                }
            }

            if (txtBatchTotal.NumberValueDecimal.Value != SumBatch)
            {
                txtBatchTotal.NumberValueDecimal = SumBatch;
            }

            if (!DataChanges && !SaveButtonWasEnabled && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        #endregion field updates
    }
}