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
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.MFinance.Logic;

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
            TErrorProcessingMode ADataValidationProcessingMode,
            Control AValidateSpecificControl = null,
            bool ADontRecordNewDataValidationRun = true);
    }

    public partial class TUC_GiftBatches : IUC_GiftBatches, IBoundImageEvaluator
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
        private bool FBankAccountOnly = true;
        private string FSelectedBatchMethodOfPayment = String.Empty;

        //List of all batches and whether or not the user has been warned of the presence
        // of inactive fields on saving.
        private Dictionary <int, bool>FUnpostedBatchesVerifiedOnSavingDict = new Dictionary <int, bool>();

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        //System & User Defaults
        private bool FDonorZeroIsValid = false;
        private bool FRecipientZeroIsValid = false;
        private bool FWarnOfInactiveValuesOnPosting = false;

        //Date related
        private DateTime FDefaultDate;
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;

        //Currency related
        private string FLedgerBaseCurrency = String.Empty;

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

                ParentForm.Cursor = Cursors.WaitCursor;
                InitialiseLogicObjects();
                InitialiseLedgerControls();
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
            FLoadAndFilterLogicObject = new TUC_GiftBatches_LoadAndFilter(FPetraUtilsObject, FLedgerNumber, FMainDS, FFilterAndFindObject);
            FImportLogicObject = new TUC_GiftBatches_Import(FPetraUtilsObject, FLedgerNumber, this);
            FPostingLogicObject = new TUC_GiftBatches_Post(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FReceiptingLogicObject = new TUC_GiftBatches_Receipt();
            FCancelLogicObject = new TUC_GiftBatches_Cancel(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FAccountAndCostCentreLogicObject = new TUC_GiftBatches_AccountAndCostCentre(FLedgerNumber,
                FMainDS,
                cmbDetailBankAccountCode,
                cmbDetailBankCostCentre);
        }

        private void InitialiseLedgerControls()
        {
            // Load Motivation detail in this central place; it will be used by UC_GiftTransactions
            AMotivationDetailTable MotivationDetail = (AMotivationDetailTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.MotivationList,
                FLedgerNumber);

            MotivationDetail.TableName = FMainDS.AMotivationDetail.TableName;
            FMainDS.Merge(MotivationDetail);

            FMainDS.AMotivationDetail.AcceptChanges();

            FMainDS.AGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                AGiftBatchTable.GetLedgerNumberDBName(),
                AGiftBatchTable.GetBatchNumberDBName()
                );

            SetupExtraGridFunctionality();
            FAccountAndCostCentreLogicObject.RefreshBankAccountAndCostCentreData(FLoadAndFilterLogicObject, out FCostCentreTable, out FAccountTable);

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = false; //this.Enabled;
            SetupAccountAndCostCentreCombos(ActiveOnly);

            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                out FStartDateCurrentPeriod,
                out FEndDateLastForwardingPeriod,
                out FDefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                FStartDateCurrentPeriod.ToShortDateString(), FEndDateLastForwardingPeriod.ToShortDateString());

            FLoadAndFilterLogicObject.InitialiseDataSources(cmbDetailBankCostCentre, cmbDetailBankAccountCode);
        }

        private void RunOnceOnParentActivationManual()
        {
            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;
                grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
                grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

                // Load the ledger table so we know the base currency
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadALedgerTable(FLedgerNumber));
                FLedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;

                FLoadAndFilterLogicObject.ActivateFilter();
                LoadBatchesForCurrentYear();

                // read system and user defaults
                FDonorZeroIsValid = ((TFrmGiftBatch)ParentForm).FDonorZeroIsValid;
                FRecipientZeroIsValid = ((TFrmGiftBatch)ParentForm).FRecipientZeroIsValid;
                FWarnOfInactiveValuesOnPosting = ((TFrmGiftBatch)ParentForm).FWarnOfInactiveValuesOnPosting;

                SetInitialFocus();
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// The current filter is not changed.  The highlighted row index remains the same (if possible) after the refresh.
        /// </summary>
        public void RefreshAllData(bool AShowStatusDialogOnLoad = true, bool AIsMessageRefresh = false)
        {
            TFrmGiftBatch MyParentForm = (TFrmGiftBatch)ParentForm;

            // Remember our current row position
            int CurrentRowIndex = GetSelectedRowIndex();
            int CurrentBatchNumber = -1;

            if ((MyParentForm != null) && (MyParentForm.InitialBatchNumber > 0))
            {
                CurrentBatchNumber = MyParentForm.InitialBatchNumber;
                MyParentForm.InitialBatchNumber = -1;
            }
            else if (AIsMessageRefresh)
            {
                if (FPetraUtilsObject.HasChanges && !MyParentForm.SaveChanges())
                {
                    string msg = String.Format(Catalog.GetString("A validation error has occured on the Gift Batches" +
                            " form while trying to refresh.{0}{0}" +
                            "You will need to close and reopen the Gift Batches form to see the new batch" +
                            " after you have fixed the validation error."),
                        Environment.NewLine);

                    MessageBox.Show(msg, "Refresh Gift Batches");
                    return;
                }

                CurrentBatchNumber = 1;
            }
            else if (FPreviouslySelectedDetailRow != null)
            {
                CurrentBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
            }

            TFrmGiftBatch PntForm = (TFrmGiftBatch)ParentForm;
            Cursor prevCursor = null;

            if (PntForm != null)
            {
                prevCursor = PntForm.Cursor;
            }
            else
            {
                prevCursor = this.Cursor;
            }

            PntForm.Cursor = Cursors.WaitCursor;

            if ((FMainDS != null) && (FMainDS.AGiftBatch != null))
            {
                // Remove all data from our DataSet object - the grid will go empty!
                FMainDS.AGiftBatch.Rows.Clear();
            }

            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();

                // Calling ApplyFilter() will automatically load the data for the currently selected year
                //  because our ApplyFilterManual() code will do that for us
                FFilterAndFindObject.ApplyFilter();

                // Now we can select the gift batch we had before (if it still exists on the grid)
                for (int i = 0; (i < FMainDS.AGiftBatch.Rows.Count); i++)
                {
                    if (FMainDS.AGiftBatch[i].BatchNumber == CurrentBatchNumber)
                    {
                        DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
                        Int32 RowNumberGrid = DataUtilities.GetDataViewIndexByDataTableIndex(dv, FMainDS.AGiftBatch, i) + 1;

                        CurrentRowIndex = RowNumberGrid;
                        break;
                    }
                }

                ShowDetails(CurrentRowIndex);

                UpdateRecordNumberDisplay();

                TUC_GiftTransactions TransactionForm = PntForm.GetTransactionsControl();

                if (TransactionForm != null)
                {
                    PntForm.EnableTransactions(grdDetails.Rows.Count > 1);

                    // if the batch number = -1 then this is not a valid instance of TUC_GiftTransactions and we do not need to refresh
                    if (TransactionForm.FBatchNumber != -1)
                    {
                        TransactionForm.ShowStatusDialogOnLoad = AShowStatusDialogOnLoad;

                        // This will update the transactions to match the current batch
                        TransactionForm.RefreshData();

                        TransactionForm.ShowStatusDialogOnLoad = true;
                    }
                }
            }
            finally
            {
                FPetraUtilsObject.EnableDataChangedEvent();
                PntForm.Cursor = prevCursor;
            }
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            if (this.FPreviouslySelectedDetailRow == null)
            {
                return;
            }

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
            bool changeable = (FPreviouslySelectedDetailRow != null) && (!ViewMode)
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
            UpdateUnpostedBatchDictionary();
            UpdateBatchPeriod(null, null);
        }

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            if (grdDetails.CanFocus)
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
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public void LoadBatchesForCurrentYear()
        {
            TFrmGiftBatch MyParentForm = (TFrmGiftBatch) this.ParentForm;
            bool PerformStandardLoad = true;

            if (MyParentForm.InitialBatchYear >= 0)
            {
                FLoadAndFilterLogicObject.StatusAll = true;

                int yearIndex = FLoadAndFilterLogicObject.FindYearAsIndex(MyParentForm.InitialBatchYear);

                if (yearIndex >= 0)
                {
                    FLoadAndFilterLogicObject.YearIndex = yearIndex;

                    if (MyParentForm.InitialBatchPeriod >= 0)
                    {
                        FLoadAndFilterLogicObject.PeriodIndex = FLoadAndFilterLogicObject.FindPeriodAsIndex(MyParentForm.InitialBatchPeriod);
                    }
                    else
                    {
                        FLoadAndFilterLogicObject.PeriodIndex = (MyParentForm.InitialBatchYear == FMainDS.ALedger[0].CurrentFinancialYear) ? 1 : 0;
                    }

                    PerformStandardLoad = false;
                }

                // Reset the start-up value
                MyParentForm.InitialBatchYear = -1;
            }

            MyParentForm.ClearCurrentSelections();

            if (ViewMode)
            {
                FMainDS.Merge(ViewModeTDS);
                FLoadAndFilterLogicObject.DisableYearAndPeriod(true);
            }

            if (PerformStandardLoad)
            {
                // Set up for current year with current and forwarding periods (on initial load this will already be set so will not fire a change)
                FLoadAndFilterLogicObject.YearIndex = 0;
                FLoadAndFilterLogicObject.PeriodIndex = 0;
            }

            // Get the data, populate the grid and re-select the current row (or first row if none currently selected) ...
            RefreshAllData();

            FBatchLoaded = true;
        }

        private void SetupAccountAndCostCentreCombos(bool AActiveOnly, AGiftBatchRow ARow = null)
        {
            if (!FBatchLoaded || (FActiveOnly != AActiveOnly))
            {
                FActiveOnly = AActiveOnly;

                FAccountAndCostCentreLogicObject.SetupAccountAndCostCentreCombos(AActiveOnly, ARow);
            }
        }

        /// <summary>
        /// Gift Type radiobutton selection changed
        /// </summary>
        private void GiftTypeChanged(Object sender, EventArgs e)
        {
            bool BankAccountOnly = true;

            // show all accounts for 'Gift In Kind' and 'Other'
            if (rbtGiftInKind.Checked || rbtOther.Checked)
            {
                BankAccountOnly = false;
            }

            if (BankAccountOnly != FBankAccountOnly)
            {
                FAccountAndCostCentreLogicObject.SetupAccountCombo(FActiveOnly,
                    BankAccountOnly,
                    ref lblDetailBankAccountCode,
                    FPreviouslySelectedDetailRow);
                FBankAccountOnly = BankAccountOnly;
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
            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 7;
            int indexOfAccountCodeDataColumn = 8;

            // Add red triangle to inactive accounts
            grdDetails.AddAnnotationImage(this, indexOfCostCentreCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.CostCentreCode, BoundGridImage.DisplayImageEnum.Inactive);
            grdDetails.AddAnnotationImage(this, indexOfAccountCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.AccountCode, BoundGridImage.DisplayImageEnum.Inactive);
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
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widths needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
        }

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            //Nothing to do as yet
        }

        /// <summary>
        /// Call ShowDetails() from outside form
        /// </summary>
        public void ShowDetailsRefresh()
        {
            ShowDetails();
        }

        private void ShowDetailsManual(AGiftBatchRow ARow)
        {
            ((TFrmGiftBatch)ParentForm).EnableTransactions(ARow != null
                && ARow.BatchStatus != MFinanceConstants.BATCH_CANCELLED);

            if (ARow == null)
            {
                FSelectedBatchNumber = -1;
                UpdateChangeableStatus();
                txtDetailHashTotal.CurrencyCode = String.Empty;
                dtpDetailGlEffectiveDate.Date = FDefaultDate;
                return;
            }

            bool Unposted = (ARow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            if (!FPostingLogicObject.PostingInProgress)
            {
                bool activeOnly = false; //unposted
                RefreshBankAccountAndCostCentreFilters(activeOnly, ARow);
            }

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode = (!Unposted || ViewMode);
            UpdateChangeableStatus();

            //Update the batch period if necessary
            UpdateBatchPeriod();

            RefreshCurrencyRelatedControls();

            if (Unposted)
            {
                //Check for inactive cost centre and/or account codes
                if (!cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1))
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Batch {0} - the Cost Centre: '{1}' is no longer active and so cannot be used."),
                            ARow.BatchNumber,
                            ARow.BankCostCentre),
                        Catalog.GetString("Gift Batch"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (!cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1))
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Batch {0} - the Bank Account: '{1}' is no longer active and so cannot be used."),
                            ARow.BatchNumber,
                            ARow.BankAccountCode),
                        Catalog.GetString("Gift Batch"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void RefreshCurrencyRelatedControls()
        {
            string CurrencyCode = FPreviouslySelectedDetailRow.CurrencyCode;

            txtDetailHashTotal.CurrencyCode = CurrencyCode;
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(CurrencyCode);

            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;
            btnGetSetExchangeRate.Enabled = (CurrencyCode != FLedgerBaseCurrency);

            // Note from AlanP Jan 2015:
            // We used to put the focus on the Get/Set button as the text in the currency box changed.
            // This was bad for two reasons
            //  1. Some currencies do not have a unique first letter.  So as a typist you would need to type two or even three letters to select your currency.
            //     This was impossible once the focus had shifted to the button.
            //  2. It was worse than that.  Once the focus is on the button Windows has this great 'feature' that typing a letter perofrms the
            //     action of ALT+letter without needing to press the ALT key!  So very unexpected things happened.
            //if (AFromUserAction && btnGetSetExchangeRate.Enabled)
            //{
            //    btnGetSetExchangeRate.Focus();
            //}
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            if ((grdDetails.Rows.Count > 1) && ValidateAllData(false, TErrorProcessingMode.Epm_All))
            {
                ((TFrmGiftBatch)ParentForm).SelectTab(TFrmGiftBatch.eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// Undo all changes to the specified batch ready to cancel it.
        ///  This avoids unecessary validation errors when cancelling.
        /// </summary>
        /// <param name="ABatchToCancel"></param>
        /// <param name="ARedisplay"></param>
        public void PrepareBatchDataForCancelling(Int32 ABatchToCancel, bool ARedisplay)
        {
            //This code will only be called when the Batch tab is active.

            DataView GiftBatchDV = new DataView(FMainDS.AGiftBatch);
            DataView GiftDV = new DataView(FMainDS.AGift);
            DataView GiftDetailDV = new DataView(FMainDS.AGiftDetail);

            GiftBatchDV.RowFilter = String.Format("{0}={1}",
                AGiftBatchTable.GetBatchNumberDBName(),
                ABatchToCancel);

            GiftDV.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchToCancel);

            GiftDetailDV.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchToCancel);

            //Work from lowest level up
            if (GiftDetailDV.Count > 0)
            {
                GiftDetailDV.Sort = String.Format("{0}, {1}",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView drv in GiftDetailDV)
                {
                    AGiftDetailRow gDR = (AGiftDetailRow)drv.Row;

                    if (gDR.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (gDR.RowState != DataRowState.Unchanged)
                    {
                        gDR.RejectChanges();
                    }
                }
            }

            if (GiftDV.Count > 0)
            {
                GiftDV.Sort = String.Format("{0}", AGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView drv in GiftDV)
                {
                    AGiftRow gR = (AGiftRow)drv.Row;

                    if (gR.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (gR.RowState != DataRowState.Unchanged)
                    {
                        gR.RejectChanges();
                    }
                }
            }

            if (GiftBatchDV.Count > 0)
            {
                AGiftBatchRow gB = (AGiftBatchRow)GiftBatchDV[0].Row;

                //No need to check for Added state as new batches are always saved
                // on creation

                if (gB.RowState != DataRowState.Unchanged)
                {
                    gB.RejectChanges();
                }

                if (ARedisplay)
                {
                    ShowDetails(gB);
                }
            }

            if (GiftDetailDV.Count == 0)
            {
                //Load all related data for batch ready to delete
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadGiftTransactionsForBatch(FLedgerNumber, ABatchToCancel));
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            GetDataFromControls();

            if (!TExtraGiftBatchChecks.CanContinueWithAnyExWorkers(TExtraGiftBatchChecks.GiftBatchAction.NEWBATCH, FMainDS, FPetraUtilsObject))
            {
                return;
            }

            if (CreateNewAGiftBatch())
            {
                if (!EnsureNewBatchIsVisible())
                {
                    return;
                }

                pnlDetails.Enabled = true;

                // NOTE: we need to suppress change detection here because otherwise the DateChanged event fires off
                //   It would not normally be a problem although it can give strange effects of focussing the date box AND the description (!)
                //   and also may sometimes give problems with running some tests.
                //  So do not remove change detection suppression so we do not run UpdateGiftBatchPeriod()
                FPetraUtilsObject.SuppressChangeDetection = true;
                FPreviouslySelectedDetailRow.GlEffectiveDate = FDefaultDate;
                dtpDetailGlEffectiveDate.Date = FDefaultDate;
                FPetraUtilsObject.SuppressChangeDetection = false;

                Int32 yearNumber = 0;
                Int32 periodNumber = 0;

                if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
                {
                    FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;
                }

                UpdateRecordNumberDisplay();

                ((TFrmGiftBatch)ParentForm).SaveChanges();
            }
        }

        private bool EnsureNewBatchIsVisible()
        {
            // Can we see the new row, bearing in mind we have filtering that the standard filter code does not know about?
            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
            Int32 RowNumberGrid = DataUtilities.GetDataViewIndexByDataTableIndex(dv,
                FMainDS.AGiftBatch,
                FMainDS.AGiftBatch.Rows.Count - 1) + 1;

            if (RowNumberGrid < 1)
            {
                MessageBox.Show(
                    Catalog.GetString(
                        "The new row has been added but the filter may be preventing it from being displayed. The filter will be reset."),
                    Catalog.GetString("New Batch"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!FLoadAndFilterLogicObject.StatusEditing)
                {
                    FLoadAndFilterLogicObject.StatusEditing = true;
                }

                //Set year and period to correct value
                FLoadAndFilterLogicObject.YearIndex = 0;
                FLoadAndFilterLogicObject.PeriodIndex = 0;

                FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                if (SelectDetailRowByDataTableIndex(FMainDS.AGiftBatch.Rows.Count - 1))
                {
                    // Good - we found the row so now we need to do the other stuff to the new record
                    txtDetailBatchDescription.Text = MCommonResourcestrings.StrPleaseEnterDescription;
                    txtDetailBatchDescription.Focus();
                }
                else
                {
                    // This is not supposed to happen!!
                    MessageBox.Show(
                        Catalog.GetString(
                            "The filter was reset but unexpectedly the new batch is not in the list. Please close the screen and do not save changes."),
                        Catalog.GetString("New Gift Batch"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            return true;
        }

        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null)
                || (GetCurrentBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            FSelectedBatchMethodOfPayment = cmbDetailMethodOfPaymentCode.GetSelectedString();

            if ((FSelectedBatchMethodOfPayment != null) && (FSelectedBatchMethodOfPayment.Length > 0))
            {
                ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateMethodOfPayment();
            }
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null)
                || (GetCurrentBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            string NewCurrency = cmbDetailCurrencyCode.GetSelectedString();

            if (FPreviouslySelectedDetailRow.CurrencyCode != NewCurrency)
            {
                Console.WriteLine("--- New currency is " + NewCurrency);
                FPreviouslySelectedDetailRow.CurrencyCode = NewCurrency;
                FPreviouslySelectedDetailRow.ExchangeRateToBase = (NewCurrency == FLedgerBaseCurrency) ? 1.0m : 0.0m;

                RefreshCurrencyRelatedControls();
                RecalculateTransactionAmounts();
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

        /// <summary>
        /// Select a special batch number from outside
        /// </summary>
        /// <param name="ABatchNumber"></param>
        /// <returns>True if the record is displayed in the grid, False otherwise</returns>
        public bool SelectBatchNumber(Int32 ABatchNumber)
        {
            for (int i = 0; (i < FMainDS.AGiftBatch.Rows.Count); i++)
            {
                if (FMainDS.AGiftBatch[i].BatchNumber == ABatchNumber)
                {
                    return SelectDetailRowByDataTableIndex(i);
                }
            }

            return false;
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

            TSharedFinanceValidation_Gift.ValidateGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, FAccountTable, FCostCentreTable);
        }

        private void ParseHashTotal(AGiftBatchRow ARow)
        {
            decimal CorrectHashValue = 0m;

            if (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            if ((txtDetailHashTotal.NumberValueDecimal != null) && txtDetailHashTotal.NumberValueDecimal.HasValue)
            {
                CorrectHashValue = txtDetailHashTotal.NumberValueDecimal.Value;
            }
            else
            {
                ARow.HashTotal = 0m;
            }

            if (ARow.HashTotal != CorrectHashValue)
            {
                ARow.HashTotal = CorrectHashValue;
                txtDetailHashTotal.NumberValueDecimal = CorrectHashValue;
                FPetraUtilsObject.SetChangedFlag();
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

        private int GetDataTableRowIndexByPrimaryKeys(int ALedgerNumber, int ABatchNumber)
        {
            int RowPos = 0;
            bool BatchFound = false;

            foreach (DataRowView rowView in FMainDS.AGiftBatch.DefaultView)
            {
                AGiftBatchRow row = (AGiftBatchRow)rowView.Row;

                if ((row.LedgerNumber == ALedgerNumber) && (row.BatchNumber == ABatchNumber))
                {
                    BatchFound = true;
                    break;
                }

                RowPos++;
            }

            if (!BatchFound)
            {
                RowPos = 0;
            }

            //remember grid is out of sync with DataView by 1 because of grid header rows
            return RowPos + 1;
        }

        /// <summary>
        /// Check for inactive field values
        /// </summary>
        /// <param name="AActionConfirmed"></param>
        /// <param name="AAction"></param>
        /// <returns></returns>
        public bool AllowInactiveFieldValues(ref bool AActionConfirmed, TExtraGiftBatchChecks.GiftBatchAction AAction)
        {
            TFrmGiftBatch MainForm = (TFrmGiftBatch) this.ParentForm;

            bool InPosting = (AAction == TExtraGiftBatchChecks.GiftBatchAction.POSTING);
            bool InCancelling = (AAction == TExtraGiftBatchChecks.GiftBatchAction.CANCELLING);
            bool InDeletingTrans = (AAction == TExtraGiftBatchChecks.GiftBatchAction.DELETINGTRANS);

            int CurrentBatch = FPreviouslySelectedDetailRow.BatchNumber;

            //Variables for building warning message
            string WarningMessage = string.Empty;
            string WarningHeader = string.Empty;
            StringBuilder WarningList = new StringBuilder();

            //Find batches that have changed
            List <AGiftBatchRow>BatchesToCheck = MainForm.GetUnsavedBatchRowsList(CurrentBatch);
            List <int>BatchesWithInactiveValues = new List <int>();

            if (BatchesToCheck.Count > 0)
            {
                int currentBatchListNo;
                string batchNoList = string.Empty;

                int numInactiveFieldsPresent = 0;
                string bankCostCentre;
                string bankAccount;

                foreach (AGiftBatchRow gBR in BatchesToCheck)
                {
                    currentBatchListNo = gBR.BatchNumber;

                    bool checkingCurrentBatch = (currentBatchListNo == CurrentBatch);

                    bool batchVerified = false;
                    bool batchExistsInDict = FUnpostedBatchesVerifiedOnSavingDict.TryGetValue(currentBatchListNo, out batchVerified);

                    if (batchExistsInDict)
                    {
                        if (batchVerified && !(InPosting && checkingCurrentBatch && FWarnOfInactiveValuesOnPosting))
                        {
                            continue;
                        }
                    }
                    else if (!(InCancelling && checkingCurrentBatch))
                    {
                        FUnpostedBatchesVerifiedOnSavingDict.Add(currentBatchListNo, false);
                    }

                    //If processing batch about to be posted, only warn according to user preferences
                    if ((InPosting && checkingCurrentBatch && !FWarnOfInactiveValuesOnPosting)
                        || (InCancelling && checkingCurrentBatch))
                    {
                        continue;
                    }

                    //Check for inactive Bank Cost Centre & Account
                    bankCostCentre = gBR.BankCostCentre;
                    bankAccount = gBR.BankAccountCode;

                    if (!FAccountAndCostCentreLogicObject.CostCentreIsActive(bankCostCentre))
                    {
                        WarningList.AppendFormat("   Cost Centre '{0}' in batch: {1}{2}",
                            gBR.BankAccountCode,
                            gBR.BatchNumber,
                            Environment.NewLine);

                        numInactiveFieldsPresent++;
                        BatchesWithInactiveValues.Add(currentBatchListNo);
                    }

                    if (!FAccountAndCostCentreLogicObject.AccountIsActive(bankAccount))
                    {
                        WarningList.AppendFormat(" Bank Account '{0}' in batch: {1}{2}",
                            gBR.BankAccountCode,
                            gBR.BatchNumber,
                            Environment.NewLine);

                        numInactiveFieldsPresent++;

                        if (!BatchesWithInactiveValues.Contains(currentBatchListNo))
                        {
                            BatchesWithInactiveValues.Add(currentBatchListNo);
                        }
                    }
                }

                if (numInactiveFieldsPresent > 0)
                {
                    string batchList = string.Empty;
                    string otherChangedBatches = string.Empty;
                    AActionConfirmed = InPosting;

                    BatchesWithInactiveValues.Sort();

                    //Update the dictionary
                    foreach (int batch in BatchesWithInactiveValues)
                    {
                        if (batch == CurrentBatch)
                        {
                            if ((!InPosting && (FUnpostedBatchesVerifiedOnSavingDict[batch] == false))
                                || (InPosting && FWarnOfInactiveValuesOnPosting))
                            {
                                FUnpostedBatchesVerifiedOnSavingDict[batch] = true;
                                batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            }
                        }
                        else if (FUnpostedBatchesVerifiedOnSavingDict[batch] == false)
                        {
                            FUnpostedBatchesVerifiedOnSavingDict[batch] = true;
                            batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            //Build a list of all batches except current batch
                            otherChangedBatches += (string.IsNullOrEmpty(otherChangedBatches) ? "" : ", ") + batch.ToString();
                        }
                    }

                    //Create header message
                    WarningHeader = "{0} inactive value(s) found in batch{1}{4}{4}Do you still want to continue with ";
                    WarningHeader += (!InDeletingTrans ? AAction.ToString().ToLower() : "deleting gift detail(s) and saving changes to") +
                                     " batch: {2}";
                    WarningHeader += (otherChangedBatches.Length > 0 ? " and with saving: {3}" : "") + " ?{4}";

                    if (!InPosting || (otherChangedBatches.Length > 0))
                    {
                        WarningHeader += "{4}(You will only be warned once about inactive values when saving any batch!){4}";
                    }

                    //Handle plural
                    batchList = (otherChangedBatches.Length > 0 ? "es: " : ": ") + batchList;

                    WarningMessage = String.Format(Catalog.GetString(WarningHeader + "{4}Inactive values:{4}{5}{4}{6}{5}"),
                        numInactiveFieldsPresent,
                        batchList,
                        CurrentBatch,
                        otherChangedBatches,
                        Environment.NewLine,
                        new String('-', 44),
                        WarningList);

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox((TFrmGiftBatch)ParentForm);

                    string header = string.Empty;

                    if (InPosting)
                    {
                        header = "Post";
                    }
                    else if (InCancelling)
                    {
                        header = "Cancel";
                    }
                    else if (InDeletingTrans)
                    {
                        header = "Delete Gift Detail From";
                    }
                    else
                    {
                        header = "Save";
                    }

                    return extendedMessageBox.ShowDialog(WarningMessage,
                        Catalog.GetString(header + " Gift Batch"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbYesNo,
                        TFrmExtendedMessageBox.TIcon.embiQuestion) == TFrmExtendedMessageBox.TResult.embrYes;
                }
            }

            return true;
        }

        /// <summary>
        /// Update the dictionary that stores all unposted batches
        ///  and whether or not they have been warned about inactive
        ///   fields
        /// </summary>
        /// <param name="ABatchNumberToExclude"></param>
        public void UpdateUnpostedBatchDictionary(int ABatchNumberToExclude = 0)
        {
            if (ABatchNumberToExclude > 0)
            {
                FUnpostedBatchesVerifiedOnSavingDict.Remove(ABatchNumberToExclude);
            }

            DataView BatchDV = new DataView(FMainDS.AGiftBatch);

            //Just want unposted batches
            BatchDV.RowFilter = string.Format("{0}='{1}'",
                AGiftBatchTable.GetBatchStatusDBName(),
                MFinanceConstants.BATCH_UNPOSTED);

            foreach (DataRowView bRV in BatchDV)
            {
                AGiftBatchRow br = (AGiftBatchRow)bRV.Row;

                int currentBatch = br.BatchNumber;

                if ((currentBatch != ABatchNumberToExclude) && !FUnpostedBatchesVerifiedOnSavingDict.ContainsKey(currentBatch))
                {
                    FUnpostedBatchesVerifiedOnSavingDict.Add(br.BatchNumber, false);
                }
            }
        }

        /// <summary>
        /// Select a specified row in the gift batch grid
        /// </summary>
        /// <param name="ABatchRow"></param>
        public void SelectRowInBatchGrid(int ABatchRow)
        {
            SelectRowInGrid(ABatchRow);
        }

        private Boolean LoadAllBatchData(int ABatchNumber)
        {
            return ((TFrmGiftBatch)ParentForm).EnsureGiftDataPresent(FLedgerNumber, ABatchNumber);
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            bool Success = false;
            bool PostingAlreadyConfirmed = false;

            if ((GetSelectedRowIndex() < 0) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift Batch before posting!"));
                return;
            }

            TFrmGiftBatch MainForm = (TFrmGiftBatch) this.ParentForm;
            TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());
            bool LoadDialogVisible = false;

            int CurrentlySelectedRow = grdDetails.GetFirstHighlightedRowIndex();

            try
            {
                Cursor = Cursors.WaitCursor;
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.POSTING;

                dlgStatus.Show();
                LoadDialogVisible = true;
                dlgStatus.Heading = String.Format(Catalog.GetString("Batch {0}"), FSelectedBatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading gifts ready for posting...");

                if (!LoadAllBatchData(FSelectedBatchNumber))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(Catalog.GetString("The Gift Batch is empty!"), Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    dlgStatus.Close();
                    LoadDialogVisible = false;
                    return;
                }

                dlgStatus.Close();
                LoadDialogVisible = false;

                Success = FPostingLogicObject.PostBatch(FPreviouslySelectedDetailRow,
                    FWarnOfInactiveValuesOnPosting,
                    FDonorZeroIsValid,
                    FRecipientZeroIsValid,
                    PostingAlreadyConfirmed);

                if (Success)
                {
                    // Posting succeeded so now deal with gift receipting ...
                    GiftBatchTDS PostedGiftTDS = TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatchAndRelatedData(FLedgerNumber,
                        FSelectedBatchNumber,
                        false);

                    FReceiptingLogicObject.PrintGiftBatchReceiptsTemplater(PostedGiftTDS);

                    // Now we need to get the data back from the server to pick up all the changes
                    RefreshAllData();

                    if (FPetraUtilsObject.HasChanges)
                    {
                        ((TFrmGiftBatch)ParentForm).SaveChangesManual();
                    }

                    //Reset row to fire events
                    SelectRowInGrid(CurrentlySelectedRow);
                    UpdateRecordNumberDisplay();

                    //If no row exists in current view after cancellation
                    if (grdDetails.Rows.Count < 2)
                    {
                        UpdateChangeableStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (LoadDialogVisible)
                {
                    dlgStatus.Close();
                    LoadDialogVisible = false;
                }

                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.NONE;
                Cursor = Cursors.Default;
            }
        }

        private void CancelRecord(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                MessageBox.Show(Catalog.GetString("Select the row to cancel first"));
                return;
            }

            TFrmGiftBatch MainForm = (TFrmGiftBatch) this.ParentForm;

            try
            {
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.CANCELLING;

                int currentlySelectedRow = grdDetails.GetFirstHighlightedRowIndex();

                if (FCancelLogicObject.CancelBatch(FPreviouslySelectedDetailRow))
                {
                    //Reset row to fire events
                    SelectRowInGrid(currentlySelectedRow);
                    UpdateRecordNumberDisplay();

                    //If no row exists in current view after cancellation
                    if (grdDetails.Rows.Count < 2)
                    {
                        UpdateChangeableStatus();
                    }
                }
            }
            finally
            {
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.NONE;
            }
        }

        private void RecalculateTransactionAmounts(decimal ANewExchangeRate = 0)
        {
            string CurrencyCode = FPreviouslySelectedDetailRow.CurrencyCode;

            if (ANewExchangeRate == 0)
            {
                if (CurrencyCode == FLedgerBaseCurrency)
                {
                    ANewExchangeRate = 1;
                }
            }

            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateBaseAmount(false);
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            if (FLoadAndFilterLogicObject != null)
            {
                FLoadAndFilterLogicObject.ApplyFilterManual(ref AFilterString);
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

            Int32 PeriodNumber = 0;
            Int32 YearNumber = 0;
            DateTime ActualDateValue;

            try
            {
                if ((dtpDetailGlEffectiveDate.Date != null) && dtpDetailGlEffectiveDate.ValidDate(false))
                {
                    ActualDateValue = dtpDetailGlEffectiveDate.Date.Value;

                    //If invalid date return;
                    if ((ActualDateValue < FStartDateCurrentPeriod) || (ActualDateValue > FEndDateLastForwardingPeriod))
                    {
                        return;
                    }

                    if (FPreviouslySelectedDetailRow.GlEffectiveDate != ActualDateValue)
                    {
                        FPreviouslySelectedDetailRow.GlEffectiveDate = ActualDateValue;

                        // reset exchange rate
                        FPreviouslySelectedDetailRow.ExchangeRateToBase =
                            (FPreviouslySelectedDetailRow.CurrencyCode == FLedgerBaseCurrency) ? 1.0m : 0.0m;

                        RefreshCurrencyRelatedControls();
                        RecalculateTransactionAmounts();
                    }

                    if (GetAccountingYearPeriodByDate(FLedgerNumber, ActualDateValue, out YearNumber, out PeriodNumber))
                    {
                        if (PeriodNumber != FPreviouslySelectedDetailRow.BatchPeriod)
                        {
                            FPreviouslySelectedDetailRow.BatchPeriod = PeriodNumber;

                            //Period has changed, so update transactions DateEntered
                            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateDateEntered(FPreviouslySelectedDetailRow);

                            if (FLoadAndFilterLogicObject.YearIndex != 0)
                            {
                                FLoadAndFilterLogicObject.YearIndex = 0;
                                FLoadAndFilterLogicObject.PeriodIndex = 1;

                                if (AFocusOnDate)
                                {
                                    dtpDetailGlEffectiveDate.Date = ActualDateValue;
                                    dtpDetailGlEffectiveDate.Focus();
                                }
                            }
                            else if (FLoadAndFilterLogicObject.PeriodIndex != 1)
                            {
                                FLoadAndFilterLogicObject.PeriodIndex = 1;

                                if (AFocusOnDate)
                                {
                                    dtpDetailGlEffectiveDate.Date = ActualDateValue;
                                    dtpDetailGlEffectiveDate.Focus();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            FImportLogicObject.ImportBatches(TUC_GiftBatches_Import.TGiftImportDataSourceEnum.FromFile, FMainDS);
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportFromClipboard(System.Object sender, System.EventArgs e)
        {
            FImportLogicObject.ImportBatches(TUC_GiftBatches_Import.TGiftImportDataSourceEnum.FromClipboard, FMainDS);
        }

        /// <summary>
        /// Imports a transactions file
        /// </summary>
        public bool ImportTransactions(TUC_GiftBatches_Import.TGiftImportDataSourceEnum AImportDataSource)
        {
            if (!FLoadAndFilterLogicObject.StatusEditing)
            {
                FLoadAndFilterLogicObject.StatusEditing = true;
            }

            bool bSuccess = FImportLogicObject.ImportTransactions(FPreviouslySelectedDetailRow, AImportDataSource);

            if (bSuccess)
            {
                // We need to update the last transaction number for the batch
                ParentForm.Cursor = Cursors.WaitCursor;

                FMainDS.AGiftBatch.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatchSingle(
                        FLedgerNumber, FPreviouslySelectedDetailRow.BatchNumber).AGiftBatch);
                FMainDS.AGiftBatch.AcceptChanges();

                ParentForm.Cursor = Cursors.Default;
            }

            return bSuccess;
        }

        private void ReverseGiftBatch(System.Object sender, System.EventArgs e)
        {
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().ReverseGiftBatch(null, null);
        }

        private void FieldAdjustment(System.Object sender, System.EventArgs e)
        {
            TFrmGiftFieldAdjustment FieldAdjustmentForm = new TFrmGiftFieldAdjustment(ParentForm);

            FieldAdjustmentForm.LedgerNumber = FLedgerNumber;

            FieldAdjustmentForm.ShowDialog();
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
                    (txtDetailExchangeRateToBase.NumberValueDecimal == null) ? 0.0m : txtDetailExchangeRateToBase.NumberValueDecimal.Value,
                    out selectedExchangeRate,
                    out selectedEffectiveDate,
                    out selectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            if (FPreviouslySelectedDetailRow.ExchangeRateToBase != selectedExchangeRate)
            {
                FPreviouslySelectedDetailRow.ExchangeRateToBase = selectedExchangeRate;

                RefreshCurrencyRelatedControls();
                RecalculateTransactionAmounts(selectedExchangeRate);

                FPetraUtilsObject.VerificationResultCollection.Clear();
            }
        }

        #region BoundImage interface implementation

        /// <summary>
        /// Implementation of the interface member
        /// </summary>
        /// <param name="AContext">The context that identifies the column for which an image is to be evaluated</param>
        /// <param name="ADataRowView">The data containing the column of interest.  You will evaluate whether this column contains data that should have the image or not.</param>
        /// <returns>True if the image should be displayed in the current context</returns>
        public bool EvaluateBoundImage(BoundGridImage.AnnotationContextEnum AContext, DataRowView ADataRowView)
        {
            AGiftBatchRow row = (AGiftBatchRow)ADataRowView.Row;

            switch (AContext)
            {
                case BoundGridImage.AnnotationContextEnum.AccountCode:
                    return !FAccountAndCostCentreLogicObject.AccountIsActive(row.BankAccountCode);

                case BoundGridImage.AnnotationContextEnum.CostCentreCode:
                    return !FAccountAndCostCentreLogicObject.CostCentreIsActive(row.BankCostCentre);
            }

            return false;
        }

        #endregion
    }
}
