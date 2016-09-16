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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MCommon;
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
    public interface IUC_RecurringGiftBatches
    {
        /// <summary>
        /// Load the batches for the current financial year (used in particular when the screen starts up).
        /// </summary>
        void LoadRecurringBatches(Int32 ALedgerNumber);

        /// <summary>
        /// Create a new Gift Batch
        /// </summary>
        bool CreateNewARecurringGiftBatch();

        /// <summary>
        /// Validate all data
        /// </summary>
        bool ValidateAllData(bool ARecordChangeVerification,
            TErrorProcessingMode ADataValidationProcessingMode,
            Control AValidateSpecificControl = null,
            bool ADontRecordNewDataValidationRun = true);
    }

    public partial class TUC_RecurringGiftBatches : IUC_RecurringGiftBatches, IBoundImageEvaluator
    {
        private Int32 FLedgerNumber;

        // Logic objects
        private TUC_RecurringGiftBatches_LoadAndFilter FLoadAndFilterLogicObject = null;
        private TUC_RecurringGiftBatches_Submit FSubmitLogicObject = null;
        private TUC_RecurringGiftBatches_Delete FDeleteLogicObject = null;
        private TUC_RecurringGiftBatches_AccountAndCostCentre FAccountAndCostCentreLogicObject = null;

        private bool FActiveOnly = false;
        private bool FBankAccountOnly = true;
        private string FSelectedBatchMethodOfPayment = String.Empty;

        //List of all batches and whether or not the user has been warned of the presence
        // of inactive fields on saving.
        private Dictionary <int, bool>FRecurringBatchesVerifiedOnSavingDict = new Dictionary <int, bool>();

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        //System & User Defaults
        private bool FDonorZeroIsValid = false;
        private bool FRecipientZeroIsValid = false;
        private bool FWarnOfInactiveValuesOnSubmitting = false;

        //Used only in Recurring
        //TODO: check if can be done away with
        private Int32 FDeletedBatchRowIndex = 0;

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
            FLoadAndFilterLogicObject = new TUC_RecurringGiftBatches_LoadAndFilter(FPetraUtilsObject, FLedgerNumber, FMainDS, FFilterAndFindObject);
            FSubmitLogicObject = new TUC_RecurringGiftBatches_Submit(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FDeleteLogicObject = new TUC_RecurringGiftBatches_Delete(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FAccountAndCostCentreLogicObject = new TUC_RecurringGiftBatches_AccountAndCostCentre(FLedgerNumber,
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

            FMainDS.ARecurringGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ARecurringGiftBatchTable.GetLedgerNumberDBName(),
                ARecurringGiftBatchTable.GetBatchNumberDBName()
                );

            SetupExtraGridFunctionality();
            FAccountAndCostCentreLogicObject.RefreshBankAccountAndCostCentreData(FLoadAndFilterLogicObject, out FCostCentreTable, out FAccountTable);

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = false; // this.Enabled;
            SetupAccountAndCostCentreCombos(ActiveOnly);

            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            FLoadAndFilterLogicObject.InitialiseDataSources(cmbDetailBankCostCentre, cmbDetailBankAccountCode);
        }

        private void RunOnceOnParentActivationManual()
        {
            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;
                grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
                grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

                LoadRecurringBatches(FLedgerNumber);

                // read system and user defaults
                FDonorZeroIsValid = ((TFrmRecurringGiftBatch)ParentForm).FDonorZeroIsValid;
                FRecipientZeroIsValid = ((TFrmRecurringGiftBatch)ParentForm).FRecipientZeroIsValid;
                FWarnOfInactiveValuesOnSubmitting = ((TFrmRecurringGiftBatch)ParentForm).FWarnOfInactiveValuesOnSubmitting;

                SetInitialFocus();
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// </summary>
        public void RefreshAllData(bool AShowStatusDialogOnLoad = true, bool AIsMessageRefresh = false)
        {
            TFrmRecurringGiftBatch myParentForm = (TFrmRecurringGiftBatch)ParentForm;

            // Remember our current row position
            int CurrentRowIndex = GetSelectedRowIndex();
            int CurrentBatchNumber = -1;

            if ((myParentForm != null) && (myParentForm.InitialBatchNumber > 0))
            {
                CurrentBatchNumber = myParentForm.InitialBatchNumber;
                myParentForm.InitialBatchNumber = -1;
            }
            else if (AIsMessageRefresh)
            {
                if (FPetraUtilsObject.HasChanges && !myParentForm.SaveChanges())
                {
                    string msg = String.Format(Catalog.GetString("A validation error has occured on the Recurring Gift Batches" +
                            " form while trying to refresh.{0}{0}" +
                            "You will need to close and reopen the Recurring Gift Batches form to see the new batch" +
                            " after you have fixed the validation error."),
                        Environment.NewLine);

                    MessageBox.Show(msg, "Refresh Recurring Gift Batches");
                    return;
                }

                CurrentBatchNumber = 1;
            }
            else if (FPreviouslySelectedDetailRow != null)
            {
                CurrentBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
            }

            TFrmRecurringGiftBatch parentForm = (TFrmRecurringGiftBatch)ParentForm;
            Cursor prevCursor = null;

            if (parentForm != null)
            {
                prevCursor = parentForm.Cursor;
            }
            else
            {
                prevCursor = this.Cursor;
            }

            parentForm.Cursor = Cursors.WaitCursor;

            if ((FMainDS != null) && (FMainDS.ARecurringGiftBatch != null))
            {
                // Remove all data from our DataSet object - the grid will go empty!
                FMainDS.ARecurringGiftBatch.Rows.Clear();
            }

            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadARecurringGiftBatch(FLedgerNumber));

                // Now we can select the gift batch we had before (if it still exists on the grid)
                for (int i = 0; (i < FMainDS.ARecurringGiftBatch.Rows.Count); i++)
                {
                    if (FMainDS.ARecurringGiftBatch[i].BatchNumber == CurrentBatchNumber)
                    {
                        DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
                        Int32 RowNumberGrid = DataUtilities.GetDataViewIndexByDataTableIndex(dv, FMainDS.ARecurringGiftBatch, i) + 1;

                        CurrentRowIndex = RowNumberGrid;
                        break;
                    }
                }

                SelectRowInGrid(CurrentRowIndex);

                UpdateRecordNumberDisplay();

                TUC_RecurringGiftTransactions TransactionForm = parentForm.GetTransactionsControl();

                if (TransactionForm != null)
                {
                    parentForm.EnableTransactions(grdDetails.Rows.Count > 1);

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
                parentForm.Cursor = prevCursor;
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
            bool Changeable = (grdDetails.Rows.Count > 1) && (FPreviouslySelectedDetailRow != null);

            pnlDetails.Enabled = Changeable;

            this.btnNew.Enabled = true;
            this.btnDelete.Enabled = Changeable;
            this.btnSubmit.Enabled = Changeable;
            mniBatch.Enabled = true;
            mniSubmit.Enabled = Changeable;
            tbbSubmitBatch.Enabled = Changeable;
        }

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            UpdateRecurringBatchDictionary();
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
        /// <param name="ALedgerNumber"></param>
        public void LoadRecurringBatches(Int32 ALedgerNumber)
        {
            TFrmRecurringGiftBatch MyParentForm = (TFrmRecurringGiftBatch) this.ParentForm;

            MyParentForm.ClearCurrentSelections();

            // Get the data, populate the grid and re-select the current row (or first row if none currently selected) ...
            RefreshAllData();

            FBatchLoaded = true;
        }

        private void SetupAccountAndCostCentreCombos(bool AActiveOnly = true, ARecurringGiftBatchRow ARow = null)
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

        private void RefreshBankAccountAndCostCentreFilters(bool AActiveOnly, ARecurringGiftBatchRow ARow = null)
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
            int indexOfCostCentreCodeDataColumn = 5;
            int indexOfAccountCodeDataColumn = 6;

            // Add red triangle to inactive accounts
            grdDetails.AddAnnotationImage(this, indexOfCostCentreCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.CostCentreCode, BoundGridImage.DisplayImageEnum.Inactive);
            grdDetails.AddAnnotationImage(this, indexOfAccountCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.AccountCode, BoundGridImage.DisplayImageEnum.Inactive);
        }

        /// <summary>
        /// get the row of the current batch
        /// </summary>
        /// <returns>ARecurringGiftBatchRow</returns>
        public ARecurringGiftBatchRow GetCurrentRecurringBatchRow()
        {
            if (FBatchLoaded && (FPreviouslySelectedDetailRow != null))
            {
                return (ARecurringGiftBatchRow)FMainDS.ARecurringGiftBatch.Rows.Find(new object[] { FLedgerNumber,
                                                                                                    FPreviouslySelectedDetailRow.BatchNumber });
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
        public ARecurringGiftBatchRow GetAnyRecurringBatchRow(Int32 ABatchNumber)
        {
            if (FBatchLoaded)
            {
                return (ARecurringGiftBatchRow)FMainDS.ARecurringGiftBatch.Rows.Find(new object[] { FLedgerNumber, ABatchNumber });
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

        private void ShowDetailsManual(ARecurringGiftBatchRow ARow)
        {
            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactions(ARow != null);

            if (ARow == null)
            {
                FSelectedBatchNumber = -1;
                UpdateChangeableStatus();
                txtDetailHashTotal.CurrencyCode = String.Empty;
                return;
            }

            if (!FSubmitLogicObject.SubmittingInProgress)
            {
                bool activeOnly = false;
                RefreshBankAccountAndCostCentreFilters(activeOnly, ARow);
            }

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode = false;
            UpdateChangeableStatus();

            RefreshCurrencyRelatedControls();

            //Check for inactive cost centre and/or account codes
            if (!cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1))
            {
                MessageBox.Show(String.Format(Catalog.GetString("Batch {0} - the Cost Centre: '{1}' is no longer active and so cannot be used."),
                        ARow.BatchNumber,
                        ARow.BankCostCentre),
                    Catalog.GetString("Recurring Gift Batch"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (!cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1))
            {
                MessageBox.Show(String.Format(Catalog.GetString("Batch {0} - the Bank Account: '{1}' is no longer active and so cannot be used."),
                        ARow.BatchNumber,
                        ARow.BankAccountCode),
                    Catalog.GetString("Recurring Gift Batch"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshCurrencyRelatedControls()
        {
            string CurrencyCode = FPreviouslySelectedDetailRow.CurrencyCode;

            txtDetailHashTotal.CurrencyCode = CurrencyCode;
            ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(CurrencyCode);
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            if ((grdDetails.Rows.Count > 1) && ValidateAllData(false, TErrorProcessingMode.Epm_All))
            {
                ((TFrmRecurringGiftBatch)ParentForm).SelectTab(TFrmRecurringGiftBatch.eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// Re-show the specified row
        /// </summary>
        /// <param name="ABatchToDelete"></param>
        /// <param name="ARedisplay"></param>
        /// <param name="ACurrentBatchTransactionsLoadedAndCurrent"></param>
        public void PrepareBatchDataForDeleting(Int32 ABatchToDelete, bool ARedisplay, out bool ACurrentBatchTransactionsLoadedAndCurrent)
        {
            //This code will only be called when the Batch tab is active.

            ACurrentBatchTransactionsLoadedAndCurrent = false;

            DataView GiftBatchDV = new DataView(FMainDS.ARecurringGiftBatch);
            DataView GiftDV = new DataView(FMainDS.ARecurringGift);
            DataView GiftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);

            GiftBatchDV.RowFilter = String.Format("{0}={1}",
                ARecurringGiftBatchTable.GetBatchNumberDBName(),
                ABatchToDelete);

            GiftDV.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchToDelete);

            GiftDetailDV.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchToDelete);

            //Work from lowest level up
            if (GiftDetailDV.Count > 0)
            {
                TFrmRecurringGiftBatch mainForm = (TFrmRecurringGiftBatch) this.ParentForm;
                ACurrentBatchTransactionsLoadedAndCurrent = (mainForm.GetTransactionsControl().FBatchNumber == ABatchToDelete);

                GiftDetailDV.Sort = String.Format("{0}, {1}",
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    ARecurringGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView drv in GiftDetailDV)
                {
                    ARecurringGiftDetailRow gDR = (ARecurringGiftDetailRow)drv.Row;

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
                GiftDV.Sort = String.Format("{0}", ARecurringGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView drv in GiftDV)
                {
                    ARecurringGiftRow gR = (ARecurringGiftRow)drv.Row;

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
                ARecurringGiftBatchRow gB = (ARecurringGiftBatchRow)GiftBatchDV[0].Row;

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
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringGiftTransactionsForBatch(FLedgerNumber, ABatchToDelete));
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

            if (CreateNewARecurringGiftBatch())
            {
                if (!EnsureNewBatchIsVisible())
                {
                    return;
                }

                pnlDetails.Enabled = true;

                UpdateRecordNumberDisplay();

                ((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges();
            }
        }

        private bool EnsureNewBatchIsVisible()
        {
            // Can we see the new row, bearing in mind we have filtering that the standard filter code does not know about?
            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
            Int32 RowNumberGrid = DataUtilities.GetDataViewIndexByDataTableIndex(dv,
                FMainDS.ARecurringGiftBatch,
                FMainDS.ARecurringGiftBatch.Rows.Count - 1) + 1;

            if (RowNumberGrid < 1)
            {
                MessageBox.Show(
                    Catalog.GetString(
                        "The new row has been added but the filter may be preventing it from being displayed. The filter will be reset."),
                    Catalog.GetString("New Recurring Gift Batch"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                if (SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftBatch.Rows.Count - 1))
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
                        Catalog.GetString("New Recurring Gift Batch"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            return true;
        }

        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            FSelectedBatchMethodOfPayment = cmbDetailMethodOfPaymentCode.GetSelectedString();

            if ((FSelectedBatchMethodOfPayment != null) && (FSelectedBatchMethodOfPayment.Length > 0))
            {
                ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateMethodOfPayment();
            }
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            String NewCurrency = cmbDetailCurrencyCode.GetSelectedString();

            if (FPreviouslySelectedDetailRow.CurrencyCode != NewCurrency)
            {
                Console.WriteLine("--- New currency is " + NewCurrency);
                FPreviouslySelectedDetailRow.CurrencyCode = NewCurrency;

                RefreshCurrencyRelatedControls();
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
                TUC_RecurringGiftTransactions t = ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl();

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
        public bool SelectRecurringBatchNumber(Int32 ABatchNumber)
        {
            for (int i = 0; (i < FMainDS.ARecurringGiftBatch.Rows.Count); i++)
            {
                if (FMainDS.ARecurringGiftBatch[i].BatchNumber == ABatchNumber)
                {
                    return SelectDetailRowByDataTableIndex(i);
                }
            }

            return false;
        }

        private void ValidateDataDetailsManual(ARecurringGiftBatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //Hash total special case in view of the textbox handling
            ParseHashTotal(ARow);

            TSharedFinanceValidation_Gift.ValidateRecurringGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);

            //TODO: remove this once database definition is set for Batch Description to be NOT NULL
            // Description is mandatory then make sure it is set
            if (txtDetailBatchDescription.Text.Length == 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringGiftBatchTable.ColumnBatchDescriptionId];
                ValidationContext = String.Format("Recurring Batch number {0}",
                    ARow.BatchNumber);

                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.BatchDescription,
                    "Description of " + ValidationContext,
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }

        private void ParseHashTotal(ARecurringGiftBatchRow ARow)
        {
            decimal CorrectHashValue = 0m;

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

            foreach (DataRowView rowView in FMainDS.ARecurringGiftBatch.DefaultView)
            {
                ARecurringGiftBatchRow row = (ARecurringGiftBatchRow)rowView.Row;

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
        /// <param name="AAction"></param>
        /// <returns></returns>
        public bool AllowInactiveFieldValues(TExtraGiftBatchChecks.GiftBatchAction AAction)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return true;
            }

            TFrmRecurringGiftBatch MainForm = (TFrmRecurringGiftBatch) this.ParentForm;

            bool InSubmitting = (AAction == TExtraGiftBatchChecks.GiftBatchAction.SUBMITTING);
            bool InDeleting = (AAction == TExtraGiftBatchChecks.GiftBatchAction.DELETING);

            int CurrentBatch = FPreviouslySelectedDetailRow.BatchNumber;

            //Variables for building warning message
            string WarningMessage = string.Empty;
            string WarningHeader = string.Empty;
            StringBuilder WarningList = new StringBuilder();

            //Find batches that have changed
            List <ARecurringGiftBatchRow>BatchesToCheck = MainForm.GetUnsavedBatchRowsList(CurrentBatch);
            List <int>BatchesWithInactiveValues = new List <int>();

            if (BatchesToCheck.Count > 0)
            {
                int currentBatchListNo;
                string batchNoList = string.Empty;
                int numInactiveFieldsPresent = 0;
                string bankCostCentre;
                string bankAccount;

                foreach (ARecurringGiftBatchRow gBR in BatchesToCheck)
                {
                    currentBatchListNo = gBR.BatchNumber;

                    bool checkingCurrentBatch = (currentBatchListNo == CurrentBatch);

                    bool batchVerified = false;
                    bool batchExistsInDict = FRecurringBatchesVerifiedOnSavingDict.TryGetValue(currentBatchListNo, out batchVerified);

                    if (batchExistsInDict)
                    {
                        if (batchVerified && !(InSubmitting && checkingCurrentBatch && FWarnOfInactiveValuesOnSubmitting))
                        {
                            continue;
                        }
                    }
                    else if (!(InDeleting && checkingCurrentBatch))
                    {
                        FRecurringBatchesVerifiedOnSavingDict.Add(currentBatchListNo, false);
                    }

                    //If processing batch about to be posted, only warn according to user preferences
                    if ((InSubmitting && checkingCurrentBatch && !FWarnOfInactiveValuesOnSubmitting)
                        || (InDeleting && checkingCurrentBatch))
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

                    BatchesWithInactiveValues.Sort();

                    //Update the dictionary
                    foreach (int batch in BatchesWithInactiveValues)
                    {
                        if (batch == CurrentBatch)
                        {
                            if (!InSubmitting && (FRecurringBatchesVerifiedOnSavingDict[batch] == false)
                                || (InSubmitting && FWarnOfInactiveValuesOnSubmitting))
                            {
                                FRecurringBatchesVerifiedOnSavingDict[batch] = true;
                                batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            }
                        }
                        else if (FRecurringBatchesVerifiedOnSavingDict[batch] == false)
                        {
                            FRecurringBatchesVerifiedOnSavingDict[batch] = true;
                            batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            //Build a list of all batches except current batch
                            otherChangedBatches += (string.IsNullOrEmpty(otherChangedBatches) ? "" : ", ") + batch.ToString();
                        }
                    }

                    //Create header message
                    WarningHeader = "{0} inactive value(s) found in recurring batch{1}{4}{4}Do you still want to continue with ";
                    WarningHeader += AAction.ToString().ToLower() + " batch: {2}";
                    WarningHeader += (otherChangedBatches.Length > 0 ? " and with saving: {3}" : "") + " ?{4}";

                    if (!InSubmitting || (otherChangedBatches.Length > 0))
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

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox((TFrmRecurringGiftBatch)ParentForm);

                    return extendedMessageBox.ShowDialog(WarningMessage,
                        Catalog.GetString((InSubmitting ? "Submit" : (InDeleting ? "Delete" : "Save")) + " Recurring Gift Batch"), string.Empty,
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
        public void UpdateRecurringBatchDictionary(int ABatchNumberToExclude = 0)
        {
            if (ABatchNumberToExclude > 0)
            {
                FRecurringBatchesVerifiedOnSavingDict.Remove(ABatchNumberToExclude);
            }

            DataView BatchDV = new DataView(FMainDS.ARecurringGiftBatch);

            foreach (DataRowView bRV in BatchDV)
            {
                ARecurringGiftBatchRow br = (ARecurringGiftBatchRow)bRV.Row;

                int currentBatch = br.BatchNumber;

                if ((currentBatch != ABatchNumberToExclude) && !FRecurringBatchesVerifiedOnSavingDict.ContainsKey(currentBatch))
                {
                    FRecurringBatchesVerifiedOnSavingDict.Add(br.BatchNumber, false);
                }
            }
        }

        /// <summary>
        /// Select a specified row in the recurring gift batch grid
        /// </summary>
        /// <param name="ARecurringBatchRow"></param>
        public void SelectRowInBatchGrid(int ARecurringBatchRow)
        {
            SelectRowInGrid(ARecurringBatchRow);
        }

        private Boolean LoadAllBatchData(int ABatchNumber = 0)
        {
            return ((TFrmRecurringGiftBatch)ParentForm).EnsureGiftDataPresent(FLedgerNumber, ABatchNumber);
        }

        private void SubmitBatch(System.Object sender, System.EventArgs e)
        {
            bool Success = false;

            if ((GetSelectedRowIndex() < 0) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("Please select a Recurring Gift Batch before submitting!"));
                return;
            }

            TFrmRecurringGiftBatch MainForm = (TFrmRecurringGiftBatch)ParentForm;
            TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());
            bool LoadDialogVisible = false;

            try
            {
                Cursor = Cursors.WaitCursor;
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.SUBMITTING;

                dlgStatus.Show();
                LoadDialogVisible = true;
                dlgStatus.Heading = String.Format(Catalog.GetString("Recurring Gift Batch {0}"), FSelectedBatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading gifts ready for submitting...");

                if (!LoadAllBatchData(FSelectedBatchNumber))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(Catalog.GetString("The Recurring Gift Batch is empty!"), Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    dlgStatus.Close();
                    LoadDialogVisible = false;
                    return;
                }

                dlgStatus.Close();
                LoadDialogVisible = false;

                Success = FSubmitLogicObject.SubmitBatch(FPreviouslySelectedDetailRow,
                    FWarnOfInactiveValuesOnSubmitting,
                    FDonorZeroIsValid,
                    FRecipientZeroIsValid);
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

        private bool DeleteRowManual(ARecurringGiftBatchRow ARowToDelete, ref string ACompletionMessage)
        {
            bool RetVal = false;

            //Notify of deletion process
            TFrmRecurringGiftBatch MainForm = (TFrmRecurringGiftBatch) this.ParentForm;

            try
            {
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.DELETING;

                FDeletedBatchRowIndex = grdDetails.GetFirstHighlightedRowIndex();

                RetVal = FDeleteLogicObject.DeleteRowManual(ARowToDelete, ref FPreviouslySelectedDetailRow, ref ACompletionMessage);
            }
            finally
            {
                MainForm.FCurrentGiftBatchAction = TExtraGiftBatchChecks.GiftBatchAction.NONE;
            }

            return RetVal;
        }

        private void PostDeleteManual(ARecurringGiftBatchRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            FDeleteLogicObject.PostDeleteManual(ARowToDelete,
                AAllowDeletion,
                ADeletionPerformed,
                ACompletionMessage);

            if (ADeletionPerformed)
            {
                //Reset row to fire events
                SelectRowInGrid(FDeletedBatchRowIndex);
                UpdateRecordNumberDisplay();

                //If no row exists in current view after cancellation
                if (grdDetails.Rows.Count < 2)
                {
                    UpdateChangeableStatus();
                }

                //                ((TFrmRecurringGiftBatch)ParentForm).EnableTransactions((grdDetails.Rows.Count > 1));
                //SelectRowInGrid(FDeletedBatchRowIndex > 0 ? FDeletedBatchRowIndex : 1);
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
            ARecurringGiftBatchRow row = (ARecurringGiftBatchRow)ADataRowView.Row;

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