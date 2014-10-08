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
using System.Drawing;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Setup;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Interface used by logic objects in order to access selected public methods in the TUC_GiftBatches class
    /// </summary>
    public interface IUC_GiftBatches
    {
        /// <summary>
        /// Load the batches for the current financial year (used in particular when the screen starts up).
        /// </summary>
        void LoadBatchesForCurrentYear();

        /// <summary>
        /// Create a new Gift Batch
        /// </summary>
        bool CreateNewAGiftBatch();

        /// <summary>
        /// Validate all data
        /// </summary>
        bool ValidateAllData(bool ARecordChangeVerification,
            bool AProcessAnyDataValidationErrors,
            Control AValidateSpecificControl = null,
            bool ADontRecordNewDataValidationRun = true);
    }

    public partial class TUC_GiftBatches : IUC_GiftBatches
    {
        private Int32 FLedgerNumber;

        // Logic objects
        private TUC_GiftBatches_LoadAndFilter FLoadAndFilterLogicObject = null;
        private TUC_GiftBatches_Import FImportLogicObject = null;
        private TUC_GiftBatches_Post FPostingLogicObject = null;
        private TUC_GiftBatches_Receipt FReceiptingLogicObject = null;
        private TUC_GiftBatches_Cancel FCancelLogicObject = null;
        private TUC_GiftBatches_AccountAndCostCentre FAccountAndCostCentreLogicObject = null;

        private bool FActiveOnly = false;
        private string FSelectedBatchMethodOfPayment = String.Empty;

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        private string FBatchDescription = string.Empty;

        //Date related
        private DateTime FDateEffective;
        private DateTime FDefaultDate;
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;

        //Currency related
        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// <summary>
        /// Flags whether all the gift batch rows for this form have finished loading
        /// </summary>
        public bool FBatchLoaded = false;

        /// <summary>
        /// Currently selected batchnumber
        /// </summary>
        public Int32 FSelectedBatchNumber = -1;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                LoadBatchesForCurrentYear();
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

        /// <summary>
        /// return the method of Payment for the transaction tab
        /// </summary>
        public String MethodOfPaymentCode
        {
            get
            {
                return FSelectedBatchMethodOfPayment;
            }
        }

        private void InitialiseLogicObjects()
        {
            FLoadAndFilterLogicObject = new TUC_GiftBatches_LoadAndFilter(FLedgerNumber, FMainDS, FFilterAndFindObject);
            FImportLogicObject = new TUC_GiftBatches_Import(FPetraUtilsObject, FLedgerNumber, this);
            FPostingLogicObject = new TUC_GiftBatches_Post(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FReceiptingLogicObject = new TUC_GiftBatches_Receipt();
            FCancelLogicObject = new TUC_GiftBatches_Cancel(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FAccountAndCostCentreLogicObject = new TUC_GiftBatches_AccountAndCostCentre(FLedgerNumber,
                FMainDS,
                cmbDetailBankAccountCode,
                cmbDetailBankCostCentre);
        }

        private void RunOnceOnParentActivationManual()
        {
            FLoadAndFilterLogicObject.OnMainScreenActivation(cmbDetailBankCostCentre, cmbDetailBankAccountCode);

            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            SetInitialFocus();
        }

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
                LoadBatchesForCurrentYear();

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
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null) && (!ViewMode)
                                 && (FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            pnlDetails.Enabled = changeable;

            this.btnNew.Enabled = !ViewMode;
            this.btnCancel.Enabled = changeable;
            this.btnPostBatch.Enabled = changeable;
            mniBatch.Enabled = !ViewMode;
            mniPost.Enabled = !ViewMode;
            tbbExportBatches.Enabled = !ViewMode;
            tbbImportBatches.Enabled = !ViewMode;
            tbbPostBatch.Enabled = !ViewMode;
        }

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            UpdateBatchPeriod(null, null);
        }

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            if (grdDetails.Rows.Count < 2)
            {
                btnNew.Focus();
            }
            else
            {
                grdDetails.Focus();
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
            InitialiseLogicObjects();

            FMainDS.Merge(ViewModeTDS);
            FPetraUtilsObject.SuppressChangeDetection = true;

            FLoadAndFilterLogicObject.DisableYearAndPeriod(false);

            FMainDS.AGiftBatch.DefaultView.RowFilter =
                String.Format("{0}={1}", AGiftBatchTable.GetBatchNumberDBName(), ABatchNumber);
            Int32 RowToSelect = GetDataTableRowIndexByPrimaryKeys(ALedgerNumber, ABatchNumber);

            FAccountAndCostCentreLogicObject.RefreshBankAccountAndCostCentreData(FLoadAndFilterLogicObject);
            SetupExtraGridFunctionality();

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;
            SetupAccountAndCostCentreCombos(ActiveOnly);

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
        public void LoadBatchesForCurrentYear()
        {
            //TLogging.Log("Starting LoadBatches()");

            InitialiseLogicObjects();
            //TLogging.Log("Filter/Find logic object has been created...");

            //FLedgerNumber = ALedgerNumber;
            FDateEffective = FDefaultDate;

            ((TFrmGiftBatch)ParentForm).ClearCurrentSelections();

            if (ViewMode)
            {
                FMainDS.Merge(ViewModeTDS);
                FLoadAndFilterLogicObject.DisableYearAndPeriod(true);
            }

            ////////  Initialise the Details Controls

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

            SetupExtraGridFunctionality();
            FAccountAndCostCentreLogicObject.RefreshBankAccountAndCostCentreData(FLoadAndFilterLogicObject);

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;
            SetupAccountAndCostCentreCombos(ActiveOnly);

            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                out FStartDateCurrentPeriod,
                out FEndDateLastForwardingPeriod,
                out FDefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());

            //////////  Populate the Grid Data and show it

            // Now populate the grid...
            //TLogging.Log("Batch initialisation complete... Applying filter to populate grid");
            FFilterAndFindObject.ApplyFilter();

            ((TFrmGiftBatch) this.ParentForm).EnableTransactions((grdDetails.Rows.Count > 1));
            ShowData();

            UpdateRecordNumberDisplay();
            SelectRowInGrid(1);

            FBatchLoaded = true;
        }

        private void SetupAccountAndCostCentreCombos(bool AActiveOnly = true, AGiftBatchRow ARow = null)
        {
            if (!FBatchLoaded || (FActiveOnly != AActiveOnly))
            {
                FActiveOnly = AActiveOnly;

                FAccountAndCostCentreLogicObject.SetupAccountAndCostCentreCombos(AActiveOnly, ARow);
            }
        }

        private void RefreshBankAccountAndCostCentreFilters(bool AActiveOnly, AGiftBatchRow ARow = null)
        {
            if (FActiveOnly != AActiveOnly)
            {
                FActiveOnly = AActiveOnly;
                FAccountAndCostCentreLogicObject.RefreshBankAccountAndCostCentreFilters(AActiveOnly, ARow);
            }
        }

        private void SetupExtraGridFunctionality()
        {
            //Prepare grid to highlight inactive accounts/cost centres
            // Create a cell view for special conditions
            SourceGrid.Cells.Views.Cell strikeoutCell = new SourceGrid.Cells.Views.Cell();
            strikeoutCell.Font = new System.Drawing.Font(grdDetails.Font, FontStyle.Strikeout);
            //strikeoutCell.ForeColor = Color.Crimson;

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView conditionAccountCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionAccountCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string accountCode = row[AGiftBatchTable.ColumnBankAccountCodeId].ToString();
                return !FAccountAndCostCentreLogicObject.AccountIsActive(accountCode);
            };

            SourceGrid.Conditions.ConditionView conditionCostCentreCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionCostCentreCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string costCentreCode = row[AGiftBatchTable.ColumnBankCostCentreId].ToString();
                return !FAccountAndCostCentreLogicObject.CostCentreIsActive(costCentreCode);
            };

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 7;
            int indexOfAccountCodeDataColumn = 8;

            grdDetails.Columns[indexOfCostCentreCodeDataColumn].Conditions.Add(conditionCostCentreCodeActive);
            grdDetails.Columns[indexOfAccountCodeDataColumn].Conditions.Add(conditionAccountCodeActive);
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

        /// <summary>
        /// return any specified gift batch row
        /// </summary>
        /// <returns>AGiftBatchRow</returns>
        public AGiftBatchRow GetAnyBatchRow(Int32 ABatchNumber)
        {
            if (FBatchLoaded)
            {
                return (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, ABatchNumber });
            }
            else
            {
                return null;
            }
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
            //Nothing to do as yet
        }

        private void ShowDetailsManual(AGiftBatchRow ARow)
        {
            ((TFrmGiftBatch)ParentForm).EnableTransactions(ARow != null
                && ARow.BatchStatus != MFinanceConstants.BATCH_CANCELLED);

            if (ARow == null)
            {
                FSelectedBatchNumber = -1;
                dtpDetailGlEffectiveDate.Date = FDefaultDate;
                UpdateChangeableStatus();
                return;
            }

            if (!FPostingLogicObject.PostingInProgress)
            {
                bool ActiveOnly = (ARow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);
                RefreshBankAccountAndCostCentreFilters(ActiveOnly, ARow);
            }

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED) || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED)) || ViewMode;
            UpdateChangeableStatus();

            //Update the batch period if necessary
            UpdateBatchPeriod();

            RefreshCurrencyAndExchangeRateControls();

            Boolean ComboSetsOk = cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1);
            ComboSetsOk &= cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);

            if (!ComboSetsOk)
            {
                MessageBox.Show("Can't set combo box with row details.");
            }
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            if ((grdDetails.Rows.Count > 1) && ValidateAllData(false, true))
            {
                ((TFrmGiftBatch)ParentForm).SelectTab(TFrmGiftBatch.eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            //If viewing posted batches only, show list of editing batches
            //  instead before adding a new batch
            if (!FLoadAndFilterLogicObject.StatusEditing)
            {
                FLoadAndFilterLogicObject.StatusEditing = true;
            }
            else if (FPetraUtilsObject.HasChanges && !ValidateAllData(false, true))
            {
                return;
            }

            //Set year and period to correct value
            FLoadAndFilterLogicObject.YearIndex = 0;            // Latest year
            FLoadAndFilterLogicObject.PeriodIndex = 1;          // Current and forwarding

            pnlDetails.Enabled = true;

            this.CreateNewAGiftBatch();

            txtDetailBatchDescription.Focus();

            FPreviouslySelectedDetailRow.GlEffectiveDate = FDefaultDate;
            dtpDetailGlEffectiveDate.Date = FDefaultDate;

            Int32 yearNumber = 0;
            Int32 periodNumber = 0;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            {
                FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;
            }

            UpdateRecordNumberDisplay();

            ((TFrmGiftBatch)ParentForm).SaveChanges();
        }

        private void CancelRecord(System.Object sender, EventArgs e)
        {
            int CurrentlySelectedRow = grdDetails.GetFirstHighlightedRowIndex();

            FCancelLogicObject.CancelBatch(FPreviouslySelectedDetailRow);

            SelectRowInGrid(CurrentlySelectedRow);

            //if (grdDetails.Rows.Count > 1)
            //{
            //    ((TFrmGiftBatch)ParentForm).EnableTransactions();
            //}
            //else
            //{
            //    ((TFrmGiftBatch)ParentForm).DisableTransactions();
            //    ShowDetails(null);
            //}

            UpdateRecordNumberDisplay();
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

            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null)
                && (GetCurrentBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FPreviouslySelectedDetailRow.CurrencyCode = ACurrencyCode;
                RecalculateTransactionAmounts();
                RefreshCurrencyAndExchangeRateControls(true);
            }
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
            if ((ARow == null) || (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //Hash total special case in view of the textbox handling
            ParseHashTotal(ARow);

            //Check if the user has made a Bank Cost Centre or Account Code inactive
            //this was removed because of speed issues!
            //TODO: Revisit this
            //RefreshBankCostCentreAndAccountCodes();

            TSharedFinanceValidation_Gift.ValidateGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, FAccountTable, FCostCentreTable);
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
        /// Focus on grid
        /// </summary>
        public void SetFocusToGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                grdDetails.Focus();
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }
        }

        private void RefreshGridData(int ABatchNumber, bool ANoFocusChange, bool ASelectOnly = false)
        {
            if (!ASelectOnly)
            {
                FFilterAndFindObject.ApplyFilter();
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                ((TFrmGiftBatch) this.ParentForm).DisableTransactions();
            }
            else if (FBatchLoaded == true)
            {
                //Select same row after refilter
                int newRowToSelectAfterFilter =
                    (ABatchNumber > 0) ? GetDataTableRowIndexByPrimaryKeys(FLedgerNumber, ABatchNumber) : FPrevRowChangedRow;

                if (ANoFocusChange)
                {
                    SelectRowInGrid(newRowToSelectAfterFilter);
                }
                else
                {
                    //TODO this can't be right. Ask Alan
                    SelectRowInGrid(newRowToSelectAfterFilter);
                }
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
            UpdateBatchPeriod(true);
        }

        /// <summary>
        /// Update batch period if necessary
        /// </summary>
        public void UpdateBatchPeriod(bool AFocusOnDate = false)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            Int32 periodNumber = 0;
            Int32 yearNumber = 0;
            DateTime dateValue;

            try
            {
                if (dtpDetailGlEffectiveDate.ValidDate(false))
                {
                    dateValue = dtpDetailGlEffectiveDate.Date.Value;

                    //If invalid date return;
                    if ((dateValue < FStartDateCurrentPeriod) || (dateValue > FEndDateLastForwardingPeriod))
                    {
                        return;
                    }

                    FPreviouslySelectedDetailRow.GlEffectiveDate = dateValue;

                    if (GetAccountingYearPeriodByDate(FLedgerNumber, dateValue, out yearNumber, out periodNumber))
                    {
                        if (periodNumber != FPreviouslySelectedDetailRow.BatchPeriod)
                        {
                            FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;

                            //Period has changed, so update transactions DateEntered
                            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateDateEntered(FPreviouslySelectedDetailRow);

                            if (FLoadAndFilterLogicObject.YearIndex != 0)
                            {
                                FLoadAndFilterLogicObject.YearIndex = 0;
                                FLoadAndFilterLogicObject.PeriodIndex = 1;

                                if (AFocusOnDate)
                                {
                                    dtpDetailGlEffectiveDate.Date = dateValue;
                                    dtpDetailGlEffectiveDate.Focus();
                                }
                            }
                            else if (FLoadAndFilterLogicObject.PeriodIndex != 1)
                            {
                                FLoadAndFilterLogicObject.PeriodIndex = 1;

                                if (AFocusOnDate)
                                {
                                    dtpDetailGlEffectiveDate.Date = dateValue;
                                    dtpDetailGlEffectiveDate.Focus();
                                }
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

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches(System.Object sender, System.EventArgs e)
        {
            FImportLogicObject.ImportBatches();
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            if (GetSelectedRowIndex() < 0)
            {
                return; // Oops - there's no selected row.
            }

            if (FPostingLogicObject.PostBatch(FPreviouslySelectedDetailRow))
            {
                //AGiftBatchRow giftBatchRow = (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FSelectedBatchNumber });

                GiftBatchTDS PostedGiftTDS = TRemote.MFinance.Gift.WebConnectors.LoadGiftBatchData(FLedgerNumber, FSelectedBatchNumber);
                FReceiptingLogicObject.PrintGiftBatchReceipts(PostedGiftTDS);

                RefreshAll();
                RefreshGridData(FSelectedBatchNumber, false, true);

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

        private void RecalculateTransactionAmounts(decimal ANewExchangeRate = 0)
        {
            string CurrencyCode = FPreviouslySelectedDetailRow.CurrencyCode;
            DateTime EffectiveDate = FPreviouslySelectedDetailRow.GlEffectiveDate;

            if (ANewExchangeRate == 0)
            {
                if (CurrencyCode == FMainDS.ALedger[0].BaseCurrency)
                {
                    ANewExchangeRate = 1;
                }
                else
                {
                    ANewExchangeRate = TExchangeRateCache.GetDailyExchangeRate(
                        CurrencyCode,
                        FMainDS.ALedger[0].BaseCurrency,
                        EffectiveDate);
                }
            }

            //Need to get the exchange rate
            FPreviouslySelectedDetailRow.ExchangeRateToBase = ANewExchangeRate;

            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(CurrencyCode);
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateBaseAmount(false);
        }

        private void RefreshCurrencyAndExchangeRateControls(bool AFromUserAction = false)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            txtDetailHashTotal.CurrencyCode = FPreviouslySelectedDetailRow.CurrencyCode;

            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;
            txtDetailExchangeRateToBase.Enabled =
                (FPreviouslySelectedDetailRow.ExchangeRateToBase != DEFAULT_CURRENCY_EXCHANGE);

            if ((FMainDS.ALedger == null) || (FMainDS.ALedger.Count == 0))
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadALedgerTable(FLedgerNumber));
            }

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
                FPreviouslySelectedDetailRow.ExchangeRateToBase = selectedExchangeRate;
                RecalculateTransactionAmounts(selectedExchangeRate);

                //Enforce save needed condition
                FPetraUtilsObject.SetChangedFlag();
            }

            RefreshCurrencyAndExchangeRateControls();
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            if (FLoadAndFilterLogicObject != null)
            {
                FLoadAndFilterLogicObject.ApplyFilterManual(ref AFilterString);
            }
        }
    }
}