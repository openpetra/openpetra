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

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        #region get data

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AForceLoadFromServer">Set to true to get data from the server even though it is apparently the current batch number and status</param>
        /// <returns>True if gift transactions were loaded from server, false if transactions had been loaded already.</returns>
        public bool LoadGifts(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus, bool AForceLoadFromServer = false)
        {
            //Set key flags
            bool FirstGiftTransLoad = (FLedgerNumber == -1);
            bool SameCurrentBatch = ((FLedgerNumber == ALedgerNumber)
                                     && (FBatchNumber == ABatchNumber)
                                     && (FBatchStatus == ABatchStatus)
                                     && !AForceLoadFromServer);

            FBatchRow = GetBatchRow();

            if ((FBatchRow == null) && (GetAnyBatchRow(ABatchNumber) == null))
            {
                MessageBox.Show(String.Format("Cannot load transactions for Gift Batch {0} as the batch is not currently loaded!",
                        ABatchNumber));
                return false;
            }

            //Set key values from Batch
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchCurrencyCode = FBatchRow.CurrencyCode;
            FBatchMethodOfPayment = FBatchRow.MethodOfPaymentCode;
            FBatchStatus = ABatchStatus;
            FBatchUnpostedFlag = (FBatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            if (FirstGiftTransLoad)
            {
                InitialiseControls();
            }

            UpdateCurrencySymbols(FBatchCurrencyCode);

            //Check if the same batch is selected, so no need to apply filter
            if (SameCurrentBatch)
            {
                //Same as previously selected and we have not been asked to force a full refresh
                if (FBatchUnpostedFlag && (GetSelectedRowIndex() > 0))
                {
                    if (FGLEffectivePeriodHasChangedFlag)
                    {
                        //Just in case for the currently selected row, the date field has not been updated
                        FGLEffectivePeriodHasChangedFlag = false;
                        GetSelectedDetailRow().DateEntered = FBatchRow.GlEffectiveDate;
                        dtpDateEntered.Date = FBatchRow.GlEffectiveDate;
                    }

                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                if (FBatchUnpostedFlag
                    && ((FBatchCurrencyCode != FBatchRow.CurrencyCode)
                        || (FBatchExchangeRateToBase != FBatchRow.ExchangeRateToBase)))
                {
                    UpdateBaseAmount(false);
                }

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
                dlgStatus.Heading = String.Format(Catalog.GetString("Batch {0}"), ABatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading transactions ...");
            }

            FGiftTransactionsLoadedFlag = false;
            FSuppressListChangedFlag = false;

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;

            // if this form is readonly, then we need all codes, because old (inactive) codes might have been used
            if (FirstGiftTransLoad || (FActiveOnlyFlag == (ViewMode || !FBatchUnpostedFlag)))
            {
                FActiveOnlyFlag = !(ViewMode || !FBatchUnpostedFlag);
                dlgStatus.CurrentStatus = Catalog.GetString("Initialising controls ...");

                try
                {
                    //Without this, the Save button enables even for Posted batches!
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
            if (FMainDS.AGiftDetail.DefaultView.Count == 0)
            {
                dlgStatus.CurrentStatus = Catalog.GetString("Requesting transactions from server ...");
                //Load all partners in Batch
                FMainDS.DonorPartners.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAllPartnerDataForBatch(ALedgerNumber, ABatchNumber)); //LoadAllPartnerDataForBatch();
                //Include Donor fields
                LoadGiftDataForBatch(ALedgerNumber, ABatchNumber);
            }

            //Check if need to update batch period in each gift
            if (FBatchUnpostedFlag)
            {
                dlgStatus.CurrentStatus = Catalog.GetString("Updating batch period ...");
                ((TFrmGiftBatch)ParentForm).GetBatchControl().UpdateBatchPeriod();
            }

            // Now we set the full filter
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            dlgStatus.CurrentStatus = Catalog.GetString("Updating totals for the batch ...");
            UpdateTotals();

            if ((FPreviouslySelectedDetailRow != null) && (FBatchUnpostedFlag))
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
            bool RetVal = ((TFrmGiftBatch)ParentForm).EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);

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

                if ((FMainDS != null) && (FMainDS.AGiftDetail != null))
                {
                    FMainDS.AGift.Rows.Clear();
                    FMainDS.AGiftDetail.Rows.Clear();
                }

                // Get the current batch row from the batch tab
                FBatchRow = GetBatchRow();

                if (FBatchRow != null)
                {
                    // Be sure to pass the true parameter because we definitely need to update FMainDS.AGiftDetail as it is now empty!
                    LoadGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, FBatchRow.BatchStatus, true);
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
        private bool RefreshBatchGiftData(Int32 ABatchNumber,
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
                DataView giftDetailView = new DataView(TempDS.AGiftDetail);

                giftDetailView.RowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView dr in giftDetailView)
                {
                    dr.Delete();
                }

                DataView giftView = new DataView(TempDS.AGift);

                giftView.RowFilter = String.Format("{0}={1}",
                    AGiftTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftView.Sort = String.Format("{0} DESC",
                    AGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView dr in giftView)
                {
                    dr.Delete();
                }

                TempDS.AcceptChanges();

                //Clear all gift data from Main dataset gift tables
                FMainDS.AGiftDetail.Clear();
                FMainDS.AGift.Clear();

                //Bring data back in from other batches if it exists
                if (TempDS.AGift.Count > 0)
                {
                    FMainDS.AGift.Merge(TempDS.AGift);
                    FMainDS.AGiftDetail.Merge(TempDS.AGiftDetail);
                }

                //TODO: Confirm I need to AcceptChanges
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadGiftTransactionsForBatch(FLedgerNumber, ABatchNumber));

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

        private AGiftBatchRow GetBatchRow()
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().GetCurrentBatchRow();
        }

        private AGiftBatchRow GetAnyBatchRow(Int32 ABatchNumber)
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().GetAnyBatchRow(ABatchNumber);
        }

        private AGiftRow GetGiftRow(Int32 AGiftTransactionNumber)
        {
            return (AGiftRow)FMainDS.AGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, AGiftTransactionNumber });
        }

        private GiftBatchTDSAGiftDetailRow GetGiftDetailRow(Int32 AGiftTransactionNumber, Int32 AGiftDetailNumber)
        {
            return (GiftBatchTDSAGiftDetailRow)FMainDS.AGiftDetail.Rows.Find(new object[] { FLedgerNumber,
                                                                                            FBatchNumber,
                                                                                            AGiftTransactionNumber,
                                                                                            AGiftDetailNumber });
        }

        #endregion get data

        #region data handling

        /// <summary>
        /// Select a special gift detail number from outside the form
        /// </summary>
        /// <param name="AGiftNumber"></param>
        /// <param name="AGiftDetailNumber"></param>
        public void SelectGiftDetailNumber(Int32 AGiftNumber, Int32 AGiftDetailNumber)
        {
            DataView myView = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView;

            for (int counter = 0; (counter < myView.Count); counter++)
            {
                int myViewGiftNumber = (int)myView[counter][2];
                int myViewGiftDetailNumber = (int)(int)myView[counter][3];

                if ((myViewGiftNumber == AGiftNumber) && (myViewGiftDetailNumber == AGiftDetailNumber))
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
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FMainDS.AGiftDetail.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.AGiftDetail.DefaultView.Sort = string.Format("{0}, {1}",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                FMainDS.AGift.DefaultView.RowFilter = String.Format("{0}={1}",
                    AGiftTable.GetBatchNumberDBName(),
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

            if (txtDetailGiftTransactionAmount.NumberValueDecimal == null)
            {
                return;
            }

            decimal NewAmount = txtDetailGiftTransactionAmount.NumberValueDecimal.Value;

            if ((FPreviouslySelectedDetailRow != null)
                && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FPreviouslySelectedDetailRow.GiftTransactionAmount = NewAmount;
                UpdateBaseAmount(true);
            }

            UpdateTotals();
        }

        private void SetBatchLastGiftNumber()
        {
            DataView dv = new DataView(FMainDS.AGift);

            dv.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                FBatchNumber);

            dv.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            dv.RowStateFilter = DataViewRowState.CurrentRows;

            if (dv.Count > 0)
            {
                AGiftRow transRow = (AGiftRow)dv[0].Row;
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
                FBatchMethodOfPayment = ((TFrmGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
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
            if (txtDetailGiftTransactionAmount.CurrencyCode != ACurrencyCode)
            {
                txtDetailGiftTransactionAmount.CurrencyCode = ACurrencyCode;
            }

            if ((txtGiftTotal.CurrencyCode != ACurrencyCode)
                || (txtBatchTotal.CurrencyCode != ACurrencyCode)
                || (txtHashTotal.CurrencyCode != ACurrencyCode))
            {
                txtGiftTotal.CurrencyCode = ACurrencyCode;
                txtBatchTotal.CurrencyCode = ACurrencyCode;
                txtHashTotal.CurrencyCode = ACurrencyCode;
            }

            //Handle-tax related controls
            if (txtTaxDeductAmount.CurrencyCode != ACurrencyCode)
            {
                txtTaxDeductAmount.CurrencyCode = ACurrencyCode;
            }

            if (txtNonDeductAmount.CurrencyCode != ACurrencyCode)
            {
                txtNonDeductAmount.CurrencyCode = ACurrencyCode;
            }
        }

        /// <summary>
        /// Update the transaction method payment from outside
        /// </summary>
        public void UpdateMethodOfPayment()
        {
            Int32 LedgerNumber;
            Int32 BatchNumber;

            if (!((TFrmGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().MethodOfPaymentCode;

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
            DataView GiftView = new DataView(FMainDS.AGift);

            GiftView.RowStateFilter = DataViewRowState.CurrentRows;
            GiftView.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drv in GiftView)
            {
                AGiftRow giftRow = (AGiftRow)drv.Row;
                giftRow.MethodOfPaymentCode = FBatchMethodOfPayment;
            }

            //Do same at detail level to update the grid
            DataView GiftDetailView = new DataView(FMainDS.AGiftDetail);

            GiftDetailView.RowStateFilter = DataViewRowState.CurrentRows;
            GiftDetailView.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drv in GiftDetailView)
            {
                GiftBatchTDSAGiftDetailRow giftDetailRow = (GiftBatchTDSAGiftDetailRow)drv.Row;
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
                    //((TFrmGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
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

                DataView giftDetailDV = new DataView(FMainDS.AGiftDetail);
                giftDetailDV.RowStateFilter = DataViewRowState.CurrentRows;

                giftDetailDV.RowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);

                foreach (DataRowView drv in giftDetailDV)
                {
                    AGiftDetailRow gdr = (AGiftDetailRow)drv.Row;

                    if (gdr.GiftTransactionNumber == GiftNumber)
                    {
                        if (FPreviouslySelectedDetailRow.DetailNumber == gdr.DetailNumber)
                        {
                            SumTransactions += Convert.ToDecimal(txtDetailGiftTransactionAmount.NumberValueDecimal);
                            SumBatch += Convert.ToDecimal(txtDetailGiftTransactionAmount.NumberValueDecimal);
                        }
                        else
                        {
                            SumTransactions += gdr.GiftTransactionAmount;
                            SumBatch += gdr.GiftTransactionAmount;
                        }
                    }
                    else
                    {
                        SumBatch += gdr.GiftTransactionAmount;
                    }
                }

                if ((txtGiftTotal.NumberValueDecimal.HasValue == false) || (txtGiftTotal.NumberValueDecimal.Value != SumTransactions))
                {
                    txtGiftTotal.NumberValueDecimal = SumTransactions;
                }

                txtGiftTotal.CurrencyCode = txtDetailGiftTransactionAmount.CurrencyCode;
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

        /// <summary>
        /// update the transaction DateEntered from outside
        /// </summary>
        /// <param name="ABatchRow"></param>
        public void UpdateDateEntered(AGiftBatchRow ABatchRow)
        {
            Int32 ledgerNumber;
            Int32 batchNumber;
            DateTime batchEffectiveDate;

            if (ABatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            ledgerNumber = ABatchRow.LedgerNumber;
            batchNumber = ABatchRow.BatchNumber;
            batchEffectiveDate = ABatchRow.GlEffectiveDate;

            DataView giftDataView = new DataView(FMainDS.AGift);

            giftDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                AGiftTable.GetLedgerNumberDBName(),
                ledgerNumber,
                AGiftTable.GetBatchNumberDBName(),
                batchNumber);

            DataView giftDetailDataView = new DataView(FMainDS.AGiftDetail);

            giftDetailDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                AGiftDetailTable.GetLedgerNumberDBName(),
                ledgerNumber,
                AGiftDetailTable.GetBatchNumberDBName(),
                batchNumber);

            ((TFrmGiftBatch)ParentForm).EnsureGiftDataPresent(ledgerNumber, batchNumber);

            if ((FPreviouslySelectedDetailRow != null) && (FBatchNumber == batchNumber))
            {
                //Rows already active in transaction tab. Need to set current row as code below will not update currently selected row
                FGLEffectivePeriodHasChangedFlag = true;
                GetSelectedDetailRow().DateEntered = batchEffectiveDate;
            }

            TFrmGiftBatch ParentGiftBatchForm = (TFrmGiftBatch)ParentForm;
            ParentGiftBatchForm.Cursor = Cursors.WaitCursor;

            //Update all gift rows in this batch
            foreach (DataRowView dv in giftDataView)
            {
                AGiftRow giftRow = (AGiftRow)dv.Row;
                giftRow.DateEntered = batchEffectiveDate;
            }

            //Update all gift detail rows in this batch
            foreach (DataRowView dv in giftDetailDataView)
            {
                GiftBatchTDSAGiftDetailRow giftDetailRow = (GiftBatchTDSAGiftDetailRow)dv.Row;
                UpdateGiftDestinationOnDateChange(ref giftDetailRow, batchEffectiveDate);
            }

            ParentGiftBatchForm.Cursor = Cursors.Default;

            //If current row exists then refresh details
            if (FGLEffectivePeriodHasChangedFlag)
            {
                ShowDetails();
            }
        }

        /// <summary>
        /// Update the transaction base amount calculation
        /// </summary>
        /// <param name="AUpdateCurrentRowOnly"></param>
        public void UpdateBaseAmount(Boolean AUpdateCurrentRowOnly)
        {
            Int32 LedgerNumber;
            Int32 CurrentBatchNumber;

            DateTime BatchEffectiveDate;

            decimal BatchExchangeRateToBase = 0;
            string BatchCurrencyCode = string.Empty;
            decimal IntlToBaseCurrencyExchRate = 0;
            bool IsTransactionInIntlCurrency;

            string LedgerBaseCurrency = string.Empty;
            string LedgerIntlCurrency = string.Empty;

            bool TransactionsFromCurrentBatch = false;

            AGiftBatchRow CurrentBatchRow = GetBatchRow();

            if (FShowDetailsInProcess
                || !(((TFrmGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
                || (CurrentBatchRow == null)
                || (CurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            BatchCurrencyCode = CurrentBatchRow.CurrencyCode;
            BatchExchangeRateToBase = CurrentBatchRow.ExchangeRateToBase;

            if ((FBatchRow != null)
                && (CurrentBatchRow.LedgerNumber == FBatchRow.LedgerNumber)
                && (CurrentBatchRow.BatchNumber == FBatchRow.BatchNumber))
            {
                TransactionsFromCurrentBatch = true;
                FBatchCurrencyCode = BatchCurrencyCode;
                FBatchExchangeRateToBase = BatchExchangeRateToBase;
            }

            LedgerNumber = CurrentBatchRow.LedgerNumber;
            CurrentBatchNumber = CurrentBatchRow.BatchNumber;

            BatchEffectiveDate = CurrentBatchRow.GlEffectiveDate;
            LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            IntlToBaseCurrencyExchRate = ((TFrmGiftBatch)ParentForm).InternationalCurrencyExchangeRate(CurrentBatchRow,
                out IsTransactionInIntlCurrency);

            if (!LoadGiftDataForBatch(LedgerNumber, CurrentBatchNumber))
            {
                //No transactions exist to process or corporate exchange rate not found
                return;
            }

            //If only updating the currency active row
            if (AUpdateCurrentRowOnly && (FPreviouslySelectedDetailRow != null))
            {
                try
                {
                    FPreviouslySelectedDetailRow.BeginEdit();

                    FPreviouslySelectedDetailRow.GiftAmount = GLRoutines.Divide((decimal)txtDetailGiftTransactionAmount.NumberValueDecimal,
                        BatchExchangeRateToBase);

                    if (!IsTransactionInIntlCurrency)
                    {
                        FPreviouslySelectedDetailRow.GiftAmountIntl = (IntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(
                            FPreviouslySelectedDetailRow.GiftAmount,
                            IntlToBaseCurrencyExchRate);
                    }
                    else
                    {
                        FPreviouslySelectedDetailRow.GiftAmountIntl = FPreviouslySelectedDetailRow.GiftTransactionAmount;
                    }

                    if (FSETUseTaxDeductiblePercentageFlag)
                    {
                        EnableTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
                        UpdateTaxDeductibilityAmounts(this, null);
                    }
                }
                finally
                {
                    FPreviouslySelectedDetailRow.EndEdit();
                }
            }
            else
            {
                if (TransactionsFromCurrentBatch && (FPreviouslySelectedDetailRow != null))
                {
                    try
                    {
                        //Rows already active in transaction tab. Need to set current row as code further below will not update selected row
                        FPreviouslySelectedDetailRow.BeginEdit();

                        FPreviouslySelectedDetailRow.GiftAmount = GLRoutines.Divide(FPreviouslySelectedDetailRow.GiftTransactionAmount,
                            BatchExchangeRateToBase);

                        if (!IsTransactionInIntlCurrency)
                        {
                            FPreviouslySelectedDetailRow.GiftAmountIntl = (IntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(
                                FPreviouslySelectedDetailRow.GiftAmount,
                                IntlToBaseCurrencyExchRate);
                        }
                        else
                        {
                            FPreviouslySelectedDetailRow.GiftAmountIntl = FPreviouslySelectedDetailRow.GiftTransactionAmount;
                        }
                    }
                    finally
                    {
                        FPreviouslySelectedDetailRow.EndEdit();
                    }
                }

                //Update all transactions
                UpdateTransactionsCurrencyAmounts(CurrentBatchRow, IntlToBaseCurrencyExchRate, IsTransactionInIntlCurrency);
            }
        }

        private void UpdateTransactionsCurrencyAmounts(AGiftBatchRow ABatchRow,
            Decimal AIntlToBaseCurrencyExchRate,
            Boolean ATransactionInIntlCurrency)
        {
            int LedgerNumber = ABatchRow.LedgerNumber;
            int BatchNumber = ABatchRow.BatchNumber;
            decimal BatchExchangeRateToBase = ABatchRow.ExchangeRateToBase;

            if (!LoadGiftDataForBatch(LedgerNumber, BatchNumber))
            {
                return;
            }

            DataView transDV = new DataView(FMainDS.AGiftDetail);
            transDV.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drvTrans in transDV)
            {
                AGiftDetailRow gdr = (AGiftDetailRow)drvTrans.Row;

                gdr.GiftAmount = GLRoutines.Divide(gdr.GiftTransactionAmount, BatchExchangeRateToBase);

                if (!ATransactionInIntlCurrency)
                {
                    gdr.GiftAmountIntl = (AIntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(gdr.GiftAmount, AIntlToBaseCurrencyExchRate);
                }
                else
                {
                    gdr.GiftAmountIntl = gdr.GiftTransactionAmount;
                }

                if (FSETUseTaxDeductiblePercentageFlag)
                {
                    TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref gdr);
                }
            }
        }

        /// <summary>
        /// Update tax deductibility amounts when the gift amount or the tax deductible percentage has changed
        /// </summary>
        private void UpdateTaxDeductibilityAmounts(object sender, EventArgs e)
        {
            if (!FSETUseTaxDeductiblePercentageFlag
                || (FPreviouslySelectedDetailRow == null)
                || FNewGiftInProcess
                || (txtDeductiblePercentage.NumberValueDecimal == null))
            {
                return;
            }

            if (FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
            {
                if (!chkDetailTaxDeductible.Checked)
                {
                    FPreviouslySelectedDetailRow.TaxDeductible = false;
                    FPreviouslySelectedDetailRow.TaxDeductiblePct = 0.0m;
                    FPreviouslySelectedDetailRow.TaxDeductibleAmount = 0.0m;
                    FPreviouslySelectedDetailRow.NonDeductibleAmount = FPreviouslySelectedDetailRow.GiftTransactionAmount;
                }
                else
                {
                    FPreviouslySelectedDetailRow.TaxDeductible = true;

                    try
                    {
                        decimal percentageVal = txtDeductiblePercentage.NumberValueDecimal.Value;

                        if (percentageVal > 100)
                        {
                            //Avoid repeat event code running past initial check (see above)
                            FSETUseTaxDeductiblePercentageFlag = false;

                            //Reset the control
                            txtDeductiblePercentage.NumberValueDecimal = 100m;
                            percentageVal = 100m;
                        }

                        FPreviouslySelectedDetailRow.TaxDeductiblePct = percentageVal;
                    }
                    finally
                    {
                        FSETUseTaxDeductiblePercentageFlag = true;
                    }

                    AGiftDetailRow giftDetails = (AGiftDetailRow)FPreviouslySelectedDetailRow;
                    TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref giftDetails);
                }
            }

            txtTaxDeductAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TaxDeductibleAmount;
            txtNonDeductAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.NonDeductibleAmount;
        }

        private void UpdateGiftDestinationOnDateChange(ref GiftBatchTDSAGiftDetailRow ARow, DateTime ADate)
        {
            // only make changes if gift doesn't have a fixed gift destination
            if ((ARow.IsFixedGiftDestinationNull() || !ARow.FixedGiftDestination)
                && (ARow.IsModifiedDetailNull() || !ARow.ModifiedDetail)
                && (ARow.RecipientKey > 0)
                && (ARow.RecipientClass == TPartnerClass.FAMILY.ToString()))
            {
                ARow.RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(ARow.RecipientKey, ADate);
            }
        }

        #endregion field updates
    }
}