//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2013 by OM International
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
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Collections.Generic;
using Ict.Petra.Shared.MPartner;
using Ict.Common.Data;
using Ict.Common.Printing;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftBatches
    {
        private Int32 FLedgerNumber;
        private DateTime FDateEffective;
        private Int32 FSelectedBatchNumber;
        private string FBatchDescription = Catalog.GetString("Please enter batch description");
        private string FStatusFilter = "1 = 1";
        private string FPeriodFilter = "1 = 1";
        private string FCurrentBatchViewOption = MFinanceConstants.GIFT_BATCH_VIEW_EDITING;
        private Int32 FSelectedYear;
        private Int32 FSelectedPeriod;
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;
        private DateTime FDefaultDate;

        /// <summary>
        /// Flags whether all the gift batch rows for this form have finished loading
        /// </summary>
        public bool FBatchLoaded = false;

        /// <summary>
        /// Stores the current batch's method of payment
        /// </summary>//
        public string FSelectedBatchMethodOfPayment = String.Empty;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// </summary>
        public void RefreshAll()
        {
            if ((FMainDS != null) && (FMainDS.AGiftBatch != null))
            {
                FMainDS.AGiftBatch.Rows.Clear();
            }

            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                LoadBatches(FLedgerNumber);

                if (((TFrmGiftBatch)ParentForm).GetTransactionsControl() != null)
                {
                    ((TFrmGiftBatch)ParentForm).GetTransactionsControl().RefreshAll();
                }
            }
            finally
            {
                FPetraUtilsObject.EnableDataChangedEvent();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadOneBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FMainDS.Merge(ViewModeTDS);
            FPetraUtilsObject.SuppressChangeDetection = true;
            rbtPosting.Checked = false;
            rbtEditing.Checked = false;
            rbtAll.Checked = false;
            cmbYear.Enabled = false;
            cmbPeriod.Enabled = false;
            FMainDS.AGiftBatch.DefaultView.RowFilter =
                String.Format("{0}={1}", AGiftBatchTable.GetBatchNumberDBName(), ABatchNumber);
            Int32 RowToSelect = GetDataTableRowIndexByPrimaryKeys(ALedgerNumber, ABatchNumber);

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, ActiveOnly, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, ActiveOnly, true);
            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            SelectRowInGrid(RowToSelect);

            UpdateChangeableStatus();
            FPetraUtilsObject.HasChanges = false;
            FPetraUtilsObject.SuppressChangeDetection = false;
            FBatchLoaded = true;
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FDateEffective = FDefaultDate;

            ((TFrmGiftBatch)ParentForm).ClearCurrentSelections();

            if (ViewMode)
            {
                FMainDS.Merge(ViewModeTDS);
                rbtAll.Checked = true;
                cmbYear.Enabled = false;
                cmbPeriod.Enabled = false;
            }
            else
            {
                try
                {
                    FPetraUtilsObject.DisableDataChangedEvent();
                    TFinanceControls.InitialiseAvailableGiftYearsList(ref cmbYear, FLedgerNumber);
                }
                finally
                {
                    FPetraUtilsObject.EnableDataChangedEvent();
                }

                // only refresh once, seems we are doing too many loads from the db otherwise
                RefreshFilter(null, null);
            }

            // Load Motivation detail in this central place; it will be used by UC_GiftTransactions
            AMotivationDetailTable motivationDetail = (AMotivationDetailTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.MotivationList,
                FLedgerNumber);
            motivationDetail.TableName = FMainDS.AMotivationDetail.TableName;
            FMainDS.Merge(motivationDetail);

            FMainDS.AcceptChanges();

            FMainDS.AGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                AGiftBatchTable.GetLedgerNumberDBName(),
                AGiftBatchTable.GetBatchNumberDBName()
                );

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, ActiveOnly, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, ActiveOnly, true);
            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber,
                out FStartDateCurrentPeriod,
                out FEndDateLastForwardingPeriod,
                out FDefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGiftBatch) this.ParentForm).EnableTransactions();
            }
            else
            {
                ((TFrmGiftBatch) this.ParentForm).DisableTransactions();
            }

            ShowData();
            FBatchLoaded = true;
        }

        void RefreshPeriods(Object sender, EventArgs e)
        {
            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref cmbPeriod, FLedgerNumber, cmbYear.GetSelectedInt32());
            cmbPeriod.SelectedIndex = 0;
        }

        void RefreshFilter(Object sender, EventArgs e)
        {
            bool senderIsRadioButton = (sender is RadioButton);
            int batchNumber = 0;

            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }
            else if ((sender != null) && senderIsRadioButton)
            {
                //Avoid repeat events
                RadioButton rbt = (RadioButton)sender;

                if (rbt.Name.Contains(FCurrentBatchViewOption))
                {
                    return;
                }
            }

            //Record the current batch
            if (FPreviouslySelectedDetailRow != null)
            {
                batchNumber = FPreviouslySelectedDetailRow.BatchNumber;
            }

            if (FPetraUtilsObject.HasChanges && !((TFrmGiftBatch)ParentForm).SaveChanges())
            {
                if (senderIsRadioButton)
                {
                    //Need to cancel the change of option button
                    if ((FCurrentBatchViewOption == MFinanceConstants.GIFT_BATCH_VIEW_EDITING) && (rbtEditing.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtEditing.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                    else if ((FCurrentBatchViewOption == MFinanceConstants.GIFT_BATCH_VIEW_ALL) && (rbtAll.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtAll.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                    else if ((FCurrentBatchViewOption == MFinanceConstants.GIFT_BATCH_VIEW_POSTING) && (rbtPosting.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtPosting.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                }
                else
                {
                    //Reset the combos
                    try
                    {
                        FPetraUtilsObject.DisableDataChangedEvent();
                        cmbYear.SetSelectedInt32(FSelectedYear);
                        cmbPeriod.SetSelectedInt32(FSelectedPeriod);
                    }
                    finally
                    {
                        FPetraUtilsObject.EnableDataChangedEvent();
                    }
                }

                return;
            }

            ClearCurrentSelection();

            FSelectedYear = cmbYear.GetSelectedInt32();
            FSelectedPeriod = cmbPeriod.GetSelectedInt32();

            if ((FSelectedYear == -1) || (FSelectedPeriod == -1))
            {
                FPeriodFilter = "1 = 1";
            }
            else
            {
                FPeriodFilter = String.Format(
                    "{0} = {1} AND ",
                    AGiftBatchTable.GetBatchYearDBName(), FSelectedYear);

                if (FSelectedPeriod == 0)
                {
                    ALedgerRow Ledger =
                        ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                    FPeriodFilter += String.Format(
                        "{0} >= {1}",
                        AGiftBatchTable.GetBatchPeriodDBName(), Ledger.CurrentPeriod);
                }
                else
                {
                    FPeriodFilter += String.Format(
                        "{0} = {1}",
                        AGiftBatchTable.GetBatchPeriodDBName(), FSelectedPeriod);
                }
            }

            if (rbtEditing.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GIFT_BATCH_VIEW_EDITING;

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, MFinanceConstants.BATCH_UNPOSTED, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
            }
            else if (rbtPosting.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GIFT_BATCH_VIEW_POSTING;

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, MFinanceConstants.BATCH_POSTED, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = String.Format("({0} = '{1}') AND ({2} <> 0) AND (({3} = 0) OR ({3} = {2}))",
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED,
                    AGiftBatchTable.GetBatchTotalDBName(),
                    AGiftBatchTable.GetHashTotalDBName());
            }
            else
            {
                FCurrentBatchViewOption = MFinanceConstants.GIFT_BATCH_VIEW_ALL;

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, string.Empty, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = "1 = 1";
            }

            RefreshGridData(batchNumber);

            UpdateChangeableStatus();
        }

        private void RefreshGridData(int ABatchNumber, bool ASelectOnly = false)
        {
            int newRowToSelectAfterFilter = 1;

            if (!ASelectOnly)
            {
                FMainDS.AGiftBatch.DefaultView.RowFilter =
                    String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

                FPreviouslySelectedDetailRow = null;
                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftBatch.DefaultView);
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                ((TFrmGiftBatch) this.ParentForm).DisableTransactions();
            }
            else if (FBatchLoaded == true)
            {
                //Select same row after refilter
                if (ABatchNumber > 0)
                {
                    newRowToSelectAfterFilter = GetDataTableRowIndexByPrimaryKeys(FLedgerNumber, ABatchNumber);
                }

                SelectRowInGrid(newRowToSelectAfterFilter);
            }
        }

        private int GetDataTableRowIndexByPrimaryKeys(int ALedgerNumber, int ABatchNumber)
        {
            int rowPos = 0;
            bool batchFound = false;

            foreach (DataRowView rowView in FMainDS.AGiftBatch.DefaultView)
            {
                AGiftBatchRow row = (AGiftBatchRow)rowView.Row;

                if ((row.LedgerNumber == ALedgerNumber) && (row.BatchNumber == ABatchNumber))
                {
                    batchFound = true;
                    break;
                }

                rowPos++;
            }

            if (!batchFound)
            {
                rowPos = 0;
            }

            //remember grid is out of sync with DataView by 1 because of grid header rows
            return rowPos + 1;
        }

        private void UpdateBatchPeriod(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                Int32 periodNumber = 0;
                Int32 yearNumber = 0;

                if (dtpDetailGlEffectiveDate.ValidDate(false))
                {
                    DateTime dateValue = dtpDetailGlEffectiveDate.Date.Value;

                    FPreviouslySelectedDetailRow.GlEffectiveDate = dateValue;

                    if (GetAccountingYearPeriodByDate(FLedgerNumber, dateValue, out yearNumber, out periodNumber))
                    {
                        if (periodNumber != FPreviouslySelectedDetailRow.BatchPeriod)
                        {
                            FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;

                            //Period has changed, so update transactions DateEntered
                            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateDateEntered(FPreviouslySelectedDetailRow);

                            if (cmbYear.SelectedIndex != 0)
                            {
                                cmbYear.SelectedIndex = 0;
                            }
                            else if (cmbPeriod.SelectedIndex != 0)
                            {
                                cmbPeriod.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Leave BatchPeriod as it is
            }
        }

        private bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            return TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);
        }

        private void ToggleOptionButtonCheckedEvent(bool AToggleOn)
        {
            if (AToggleOn)
            {
                rbtEditing.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtPosting.CheckedChanged += new System.EventHandler(this.RefreshFilter);
            }
            else
            {
                rbtEditing.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtPosting.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
            }
        }

        /// <summary>
        /// get the row of the current batch
        /// </summary>
        /// <returns>AGiftBatchRow</returns>
        public AGiftBatchRow GetCurrentBatchRow()
        {
            if (FBatchLoaded && (FPreviouslySelectedDetailRow != null))
            {
                return (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FPreviouslySelectedDetailRow.BatchNumber });
            }
            else
            {
                return null;
            }
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            if (FPetraUtilsObject.HasChanges)
            {
                GetDataFromControls();
            }

            this.FPreviouslySelectedDetailRow = null;
            ShowData();
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widhts needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
        }

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private void ShowDetailsManual(AGiftBatchRow ARow)
        {
            if (ARow == null)
            {
                ((TFrmGiftBatch)ParentForm).DisableTransactions();
                dtpDetailGlEffectiveDate.Date = FDefaultDate;
                return;
            }

            if (ARow.BatchStatus == MFinanceConstants.BATCH_CANCELLED)
            {
                ((TFrmGiftBatch)ParentForm).DisableTransactions();
            }
            else
            {
                ((TFrmGiftBatch)ParentForm).EnableTransactions();
            }

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED) || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED)) || ViewMode;

            //dtpDetailGlEffectiveDate.Date = ARow.GlEffectiveDate;

            //Update the batch period if necessary
            UpdateBatchPeriod(null, null);

            UpdateChangeableStatus();

            RefreshCurrencyAndExchangeRate();
            Boolean ComboSetsOk = cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1);
            ComboSetsOk &= cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);

            if (!ComboSetsOk)
            {
                MessageBox.Show("Can't set combo box with row details.");
            }
        }

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewMode;
            }
        }
        private GiftBatchTDS ViewModeTDS
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewModeTDS;
            }
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGiftBatch)ParentForm).SelectTab(TFrmGiftBatch.eGiftTabs.Transactions, false);
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            Int32 yearNumber = 0;
            Int32 periodNumber = 0;

            //If viewing posted batches only, show list of editing batches
            //  instead before adding a new batch
            if (!rbtEditing.Checked)
            {
                rbtEditing.Checked = true;
            }
            else if (FPetraUtilsObject.HasChanges && !((TFrmGiftBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            //Set year and period to correct value
            if (cmbYear.GetSelectedInt32() != 0)
            {
                cmbYear.SelectedIndex = 0;
            }
            else if (cmbPeriod.GetSelectedInt32() != 0)
            {
                cmbPeriod.SelectedIndex = 0;
            }

            FPreviouslySelectedDetailRow = null;

            pnlDetails.Enabled = true;
            this.CreateNewAGiftBatch();

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            txtDetailBatchDescription.Focus();

            FPreviouslySelectedDetailRow.GlEffectiveDate = FDefaultDate;
            dtpDetailGlEffectiveDate.Date = FDefaultDate;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            {
                FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;
            }

            ((TFrmGiftBatch)ParentForm).SaveChanges();
        }

        private void CancelRecord(System.Object sender, EventArgs e)
        {
            string completionMessage = string.Empty;
            int currentlySelectedRow = 0;
            string existingBatchStatus = string.Empty;
            decimal existingBatchTotal = 0;

            if ((FPreviouslySelectedDetailRow == null) || (FPreviouslySelectedDetailRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            currentlySelectedRow = grdDetails.GetFirstHighlightedRowIndex();

            try
            {
                //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                completionMessage = String.Format(Catalog.GetString("Batch no.: {0} cancelled successfully."),
                    FPreviouslySelectedDetailRow.BatchNumber);

                existingBatchTotal = FPreviouslySelectedDetailRow.BatchTotal;
                existingBatchStatus = FPreviouslySelectedDetailRow.BatchStatus;

                //Load all journals for current Batch
                //clear any transactions currently being editied in the Transaction Tab
                ((TFrmGiftBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                //Clear gifts and details etc for current Batch
                FMainDS.AGiftDetail.Clear();
                FMainDS.AGift.Clear();

                //Load tables afresh
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(FLedgerNumber, FPreviouslySelectedDetailRow.BatchNumber));

                //Delete gift details
                for (int i = FMainDS.AGiftDetail.Count - 1; i >= 0; i--)
                {
                    FMainDS.AGiftDetail[i].Delete();
                }

                //Delete gifts
                for (int i = FMainDS.AGift.Count - 1; i >= 0; i--)
                {
                    FMainDS.AGift[i].Delete();
                }

                //Batch is only cancelled and never deleted
                FPreviouslySelectedDetailRow.BeginEdit();
                FPreviouslySelectedDetailRow.BatchTotal = 0;
                FPreviouslySelectedDetailRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                FPreviouslySelectedDetailRow.EndEdit();

                FPetraUtilsObject.HasChanges = true;

                // save first, then post
                if (!((TFrmGiftBatch)ParentForm).SaveChanges())
                {
                    FPreviouslySelectedDetailRow.BeginEdit();
                    //Should normally be Unposted, but allow for other status values in future
                    FPreviouslySelectedDetailRow.BatchTotal = existingBatchTotal;
                    FPreviouslySelectedDetailRow.BatchStatus = existingBatchStatus;
                    FPreviouslySelectedDetailRow.EndEdit();

                    SelectRowInGrid(currentlySelectedRow);

                    // saving failed, therefore do not try to cancel
                    MessageBox.Show(Catalog.GetString("The cancelled batch failed to save!"));
                }
                else
                {
                    SelectRowInGrid(currentlySelectedRow);

                    MessageBox.Show(completionMessage,
                        "Batch Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    UpdateChangeableStatus();
                }
            }
            catch (Exception ex)
            {
                completionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Cancellation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGiftBatch)ParentForm).EnableTransactions();
            }
            else
            {
                ((TFrmGiftBatch)ParentForm).DisableTransactions();
                ShowDetails(null);
            }
        }

        /// <summary>
        /// Print a receipt for each gift (one page for each donor) in the batch
        /// </summary>
        /// <param name="AGiftTDS"></param>
        private void PrintGiftBatchReceipts(GiftBatchTDS AGiftTDS)
        {
            AGiftBatchRow GiftBatchRow = AGiftTDS.AGiftBatch[0];

            AGiftTDS.AGift.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                AGiftTable.GetLedgerNumberDBName(), GiftBatchRow.LedgerNumber,
                AGiftTable.GetBatchNumberDBName(), GiftBatchRow.BatchNumber);
            String ReceiptedDonorsList = "";
            List <Int32>ReceiptedGiftTransactions = new List <Int32>();
            SortedList <Int64, AGiftTable>GiftsPerDonor = new SortedList <Int64, AGiftTable>();

            foreach (DataRowView rv in AGiftTDS.AGift.DefaultView)
            {
                AGiftRow GiftRow = (AGiftRow)rv.Row;
                bool ReceiptEachGift;
                String ReceiptLetterFrequency;
                bool EmailGiftStatement;
                bool AnonymousDonor;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerReceiptingInfo(
                    GiftRow.DonorKey,
                    out ReceiptEachGift,
                    out ReceiptLetterFrequency,
                    out EmailGiftStatement,
                    out AnonymousDonor);

                if (ReceiptEachGift)
                {
                    // I want to print a receipt for this gift,
                    // but if there's already one queued for this donor,
                    // I'll add this gift onto the existing receipt.

                    if (!GiftsPerDonor.ContainsKey(GiftRow.DonorKey))
                    {
                        GiftsPerDonor.Add(GiftRow.DonorKey, new AGiftTable());
                    }

                    AGiftRow NewRow = GiftsPerDonor[GiftRow.DonorKey].NewRowTyped();
                    DataUtilities.CopyAllColumnValues(GiftRow, NewRow);
                    GiftsPerDonor[GiftRow.DonorKey].Rows.Add(NewRow);
                }  // if receipt required

            } // foreach gift

            String HtmlDoc = "";

            foreach (Int64 DonorKey in GiftsPerDonor.Keys)
            {
                String DonorShortName;
                TPartnerClass DonorClass;
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(DonorKey, out DonorShortName, out DonorClass);
                DonorShortName = Calculations.FormatShortName(DonorShortName, eShortNameFormat.eReverseShortname);

                string HtmlPage = TRemote.MFinance.Gift.WebConnectors.PrintGiftReceipt(
                    GiftBatchRow.CurrencyCode,
                    GiftBatchRow.DateCreated.Value,
                    DonorShortName,
                    DonorKey,
                    DonorClass,
                    GiftsPerDonor[DonorKey]
                    );

                TFormLettersTools.AttachNextPage(ref HtmlDoc, HtmlPage);
                ReceiptedDonorsList += (DonorShortName + "\r\n");

                foreach (AGiftRow GiftRow in GiftsPerDonor[DonorKey].Rows)
                {
                    ReceiptedGiftTransactions.Add(GiftRow.GiftTransactionNumber);
                }
            }

            TFormLettersTools.CloseDocument(ref HtmlDoc);

            if (ReceiptedGiftTransactions.Count > 0)
            {
                TFrmReceiptControl.PreviewOrPrint(HtmlDoc);

                if (MessageBox.Show(
                        Catalog.GetString(
                            "Press OK if receipts to these recipients were printed correctly.\r\nThe gifts will be marked as receipted.\r\n") +
                        ReceiptedDonorsList,

                        Catalog.GetString("Receipt Printing"),
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (Int32 Trans in ReceiptedGiftTransactions)
                    {
                        TRemote.MFinance.Gift.WebConnectors.MarkReceiptsPrinted(
                            GiftBatchRow.LedgerNumber,
                            GiftBatchRow.BatchNumber,
                            Trans);
                    }
                }
            }
        }

        /// <summary>
        /// executed by progress dialog thread
        /// </summary>
        /// <param name="AVerifications"></param>
        private void PostGiftBatch(out TVerificationResultCollection AVerifications)
        {
            TRemote.MFinance.Gift.WebConnectors.PostGiftBatch(FLedgerNumber, FSelectedBatchNumber, out AVerifications);
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            int currentBatchNo = 0;

            if ((FPreviouslySelectedDetailRow == null) || (FPreviouslySelectedDetailRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            if (rbtAll.Checked)
            {
                currentBatchNo = FPreviouslySelectedDetailRow.BatchNumber;
            }

            Boolean batchIsEmpty = true;
            ((TFrmGiftBatch)ParentForm).LoadTransactions(FPreviouslySelectedDetailRow.LedgerNumber,
                FPreviouslySelectedDetailRow.BatchNumber,
                FPreviouslySelectedDetailRow.BatchStatus, false);

            if (FMainDS.AGift != null)
            {
                FMainDS.AGift.DefaultView.RowFilter = "a_batch_number_i = " + FPreviouslySelectedDetailRow.BatchNumber;
                batchIsEmpty = (FMainDS.AGift.DefaultView.Count == 0);
            }

            if (batchIsEmpty)  // there are no gifts in this batch!
            {
                MessageBox.Show(Catalog.GetString("Batch is empty!"), Catalog.GetString("Posting failed"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            TVerificationResultCollection Verifications;

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then post it!"));
                    return;
                }
            }

            //Read current rows position ready to reposition after removal of posted row from grid
            int newCurrentRowPos = GetSelectedRowIndex();

            if (newCurrentRowPos < 0)
            {
                return; // Oops - there's no selected row.
            }

            // ask if the user really wants to post the batch
            if (MessageBox.Show(String.Format(Catalog.GetString("Do you really want to post gift batch {0}?"),
                        FPreviouslySelectedDetailRow.BatchNumber),
                    Catalog.GetString("Confirm posting of Gift Batch"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            Verifications = new TVerificationResultCollection();

            Thread postingThread = new Thread(() => PostGiftBatch(out Verifications));

            using (TProgressDialog dialog = new TProgressDialog(postingThread))
            {
                dialog.ShowDialog();
            }

            if ((Verifications != null) && Verifications.HasCriticalErrors)
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

                AGiftBatchRow giftBatchRow = (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FSelectedBatchNumber });

                // print reports on successfully posted batch.

                // I need to retrieve the Gift Batch Row, which now has modified fields because it's been posted.
                //

                GiftBatchTDS PostedGiftTDS = TRemote.MFinance.Gift.WebConnectors.LoadGiftBatchData(giftBatchRow.LedgerNumber,
                    giftBatchRow.BatchNumber);
                PrintGiftBatchReceipts(PostedGiftTDS);

                RefreshAll();
                RefreshGridData(currentBatchNo, true);

                if (FPetraUtilsObject.HasChanges)
                {
                    ((TFrmGiftBatch)ParentForm).SaveChanges();
                }
            }
        }

        private void ExportBatches(System.Object sender, System.EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // without save the server does not have the current changes, so forbid it.
                MessageBox.Show(Catalog.GetString("Please save changed Data before the Export!"),
                    Catalog.GetString("Export Error"));
                return;
            }

            TFrmGiftBatchExport exportForm = new TFrmGiftBatchExport(FPetraUtilsObject.GetForm());
            exportForm.LedgerNumber = FLedgerNumber;
            exportForm.Show();
        }

        private void ReverseGiftBatch(System.Object sender, System.EventArgs e)
        {
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().ReverseGiftBatch(null, null);     //.ShowRevertAdjustForm("ReverseGiftBatch");
        }

        /// <summary>
        /// Update the Batch total from the transactions values
        /// </summary>
        /// <param name="ABatchTotal"></param>
        /// <param name="ABatchNumber"></param>
        public void UpdateBatchTotal(decimal ABatchTotal, Int32 ABatchNumber)
        {
            if ((FPreviouslySelectedDetailRow == null) || (FPreviouslySelectedDetailRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }
            else if (FPreviouslySelectedDetailRow.BatchNumber == ABatchNumber)
            {
                FPreviouslySelectedDetailRow.BatchTotal = ABatchTotal;
                FPetraUtilsObject.HasChanges = true;
            }
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null) && (!ViewMode)
                                 && (FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            this.btnCancel.Enabled = changeable;
            this.btnPostBatch.Enabled = changeable;
            pnlDetails.Enabled = changeable;
            mniBatch.Enabled = !ViewMode;
            mniPost.Enabled = !ViewMode;
            tbbExportBatches.Enabled = !ViewMode;
            tbbImportBatches.Enabled = !ViewMode;
            tbbPostBatch.Enabled = !ViewMode;
        }

        /// <summary>
        /// return the method of Payment for the transaction tab
        /// </summary>

        public String MethodOfPaymentCode {
            get
            {
                return cmbDetailMethodOfPaymentCode.GetSelectedString();
            }
        }

        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            FSelectedBatchMethodOfPayment = cmbDetailMethodOfPaymentCode.GetSelectedString();

            if ((FSelectedBatchMethodOfPayment != null) && (FSelectedBatchMethodOfPayment.Length > 0))
            {
                ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateMethodOfPayment(false);
            }
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            String ACurrencyCode = cmbDetailCurrencyCode.GetSelectedString();

            txtDetailHashTotal.CurrencyCode = ACurrencyCode;
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(ACurrencyCode);
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateBaseAmount(false);

            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null)
                && (GetCurrentBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FPreviouslySelectedDetailRow.CurrencyCode = ACurrencyCode;

                FPreviouslySelectedDetailRow.ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    FMainDS.ALedger[0].BaseCurrency,
                    FPreviouslySelectedDetailRow.CurrencyCode,
                    FPreviouslySelectedDetailRow.GlEffectiveDate);

                RefreshCurrencyAndExchangeRate(true);
            }
        }

        private void RefreshCurrencyAndExchangeRate(bool AFromUserAction = false)
        {
            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;
            txtDetailExchangeRateToBase.BackColor =
                (FPreviouslySelectedDetailRow.ExchangeRateToBase == DEFAULT_CURRENCY_EXCHANGE) ? Color.LightPink : Color.Empty;

            btnGetSetExchangeRate.Enabled = (FPreviouslySelectedDetailRow.CurrencyCode != FMainDS.ALedger[0].BaseCurrency);

            if (AFromUserAction && btnGetSetExchangeRate.Enabled)
            {
                btnGetSetExchangeRate.Focus();
            }
        }

        private void SetExchangeRateValue(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate setupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            decimal selectedExchangeRate;
            DateTime selectedEffectiveDate;
            int selectedEffectiveTime;

            if (setupDailyExchangeRate.ShowDialog(
                    FLedgerNumber,
                    dtpDetailGlEffectiveDate.Date.Value,
                    cmbDetailCurrencyCode.GetSelectedString(),
                    DEFAULT_CURRENCY_EXCHANGE,
                    out selectedExchangeRate,
                    out selectedEffectiveDate,
                    out selectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            if (FPreviouslySelectedDetailRow.ExchangeRateToBase != selectedExchangeRate)
            {
                //Enforce save needed condition
                FPetraUtilsObject.SetChangedFlag();
            }

            FPreviouslySelectedDetailRow.ExchangeRateToBase = selectedExchangeRate;

            RefreshCurrencyAndExchangeRate();
        }

        private void HashTotalChanged(object sender, EventArgs e)
        {
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if (txn.NumberValueDecimal == null)
            {
                return;
            }

            Decimal HashTotal = Convert.ToDecimal(txtDetailHashTotal.NumberValueDecimal);
            Form p = ParentForm;

            if (p != null)
            {
                TUC_GiftTransactions t = ((TFrmGiftBatch)ParentForm).GetTransactionsControl();

                if (t != null)
                {
                    t.UpdateHashTotal(HashTotal);
                }
            }
        }

        /// Select a special batch number from outside
        public void SelectBatchNumber(Int32 ABatchNumber)
        {
            for (int i = 0; (i < FMainDS.AGiftBatch.Rows.Count); i++)
            {
                if (FMainDS.AGiftBatch[i].BatchNumber == ABatchNumber)
                {
                    SelectDetailRowByDataTableIndex(i);
                    break;
                }
            }
        }

        private void ValidateDataDetailsManual(AGiftBatchRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            if (ARow == null)
            {
                return;
            }

            //Hash total special case in view of the textbox handling
            ParseHashTotal(ARow);

            TSharedFinanceValidation_Gift.ValidateGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ParseHashTotal(AGiftBatchRow ARow)
        {
            decimal correctHashValue;

            if (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            if ((txtDetailHashTotal.NumberValueDecimal == null) || !txtDetailHashTotal.NumberValueDecimal.HasValue)
            {
                correctHashValue = 0m;
            }
            else
            {
            	correctHashValue = txtDetailHashTotal.NumberValueDecimal.Value;
            }

            txtDetailHashTotal.NumberValueDecimal = correctHashValue;
            ARow.HashTotal = correctHashValue;
        }

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }
    }
}