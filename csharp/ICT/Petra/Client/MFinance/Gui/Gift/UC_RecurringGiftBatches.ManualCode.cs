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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftBatches
    {
        private Int32 FLedgerNumber;

        private bool FInitialFocusActionComplete = false;
        private bool FActiveOnly = false;
        private string FSelectedBatchMethodOfPayment = String.Empty;

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        /// <summary>
        /// Flags whether all the gift batch rows for this form have finished loading
        /// </summary>
        public bool FBatchLoaded = false;

        /// <summary>
        /// Currently selected batchnumber
        /// </summary>
        public Int32 FSelectedBatchNumber = -1;

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

        private void InitialiseControls()
        {
            //Add code here to setup any controls
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DoubleClickHeaderCell += new TDoubleClickHeaderCellEventHandler(grdDetails_DoubleClickHeaderCell);
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            AutoSizeGrid();
        }

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// </summary>
        public void RefreshAll()
        {
            if ((FMainDS != null) && (FMainDS.ARecurringGiftBatch != null))
            {
                FMainDS.ARecurringGiftBatch.Rows.Clear();
            }

            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                LoadRecurringBatches(FLedgerNumber);

                if (((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl() != null)
                {
                    ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().RefreshAll();
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
            Boolean changeable = (FPreviouslySelectedDetailRow != null);

            pnlDetails.Enabled = changeable;

            this.btnDelete.Enabled = changeable;
            this.btnSubmit.Enabled = changeable;
        }

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            //Add code here to run before the batch is saved
        }

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            if (FInitialFocusActionComplete)
            {
                return;
            }

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

                FInitialFocusActionComplete = true;
            }
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadRecurringBatches(Int32 ALedgerNumber)
        {
            InitialiseControls();

            FLedgerNumber = ALedgerNumber;

            ((TFrmRecurringGiftBatch)ParentForm).ClearCurrentSelections();

            // TODO: more criteria: state of batch, period, etc
            FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadARecurringGiftBatch(ALedgerNumber));

            // Load Motivation detail in this central place; it will be used by UC_GiftTransactions
            AMotivationDetailTable motivationDetail = (AMotivationDetailTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.MotivationList,
                FLedgerNumber);
            motivationDetail.TableName = FMainDS.AMotivationDetail.TableName;
            FMainDS.Merge(motivationDetail);

            FMainDS.AcceptChanges();

            FMainDS.ARecurringGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ARecurringGiftBatchTable.GetLedgerNumberDBName(),
                ARecurringGiftBatchTable.GetBatchNumberDBName()
                );

            SetupExtraGridFunctionality();

            // Always allow inactive accounts for showing historic data
            bool ActiveOnly = false;
            SetupAccountAndCostCentreCombos(ActiveOnly);

            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            ((TFrmRecurringGiftBatch) this.ParentForm).EnableTransactions(grdDetails.Rows.Count > 1);
            ShowData();

            UpdateRecordNumberDisplay();
            SelectRowInGrid(1);

            FBatchLoaded = true;
        }

        private void SetupAccountAndCostCentreCombos(bool AActiveOnly = true, ARecurringGiftBatchRow ARow = null)
        {
            if (!FBatchLoaded || (FActiveOnly != AActiveOnly))
            {
                FActiveOnly = AActiveOnly;
                cmbDetailBankCostCentre.Clear();
                cmbDetailBankAccountCode.Clear();
                TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, AActiveOnly, true, true);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, AActiveOnly, true, true);

                if (ARow != null)
                {
                    cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1);
                    cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);
                }
            }
        }

        private void RefreshBankAccountAndCostCentreFilters(bool AActiveOnly, ARecurringGiftBatchRow ARow = null)
        {
            if (FActiveOnly != AActiveOnly)
            {
                FActiveOnly = AActiveOnly;
                cmbDetailBankAccountCode.Filter = TFinanceControls.PrepareAccountFilter(true, false, AActiveOnly, true, "");
                cmbDetailBankCostCentre.Filter = TFinanceControls.PrepareCostCentreFilter(true, false, AActiveOnly, true);

                if (ARow != null)
                {
                    cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre, -1);
                    cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);
                }
            }
        }

        private void RefreshBankAccountAndCostCentreData()
        {
            //Populate CostCentreList variable
            DataTable costCentreList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref costCentreList, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)costCentreList;

            //Populate AccountList variable
            DataTable accountList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref accountList, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)accountList;
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
                string accountCode = row[ARecurringGiftBatchTable.ColumnBankAccountCodeId].ToString();
                return !AccountIsActive(accountCode);
            };

            SourceGrid.Conditions.ConditionView conditionCostCentreCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionCostCentreCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string costCentreCode = row[ARecurringGiftBatchTable.ColumnBankCostCentreId].ToString();
                return !CostCentreIsActive(costCentreCode);
            };

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 5;
            int indexOfAccountCodeDataColumn = 6;

            grdDetails.Columns[indexOfCostCentreCodeDataColumn].Conditions.Add(conditionCostCentreCodeActive);
            grdDetails.Columns[indexOfAccountCodeDataColumn].Conditions.Add(conditionAccountCodeActive);
        }

        private bool AccountIsActive(string AAccountCode = "")
        {
            bool retVal = true;

            AAccountRow currentAccountRow = null;

            //If empty, read value from combo
            if (AAccountCode == string.Empty)
            {
                if ((FAccountTable != null) && (cmbDetailBankAccountCode.SelectedIndex != -1) && (cmbDetailBankAccountCode.Count > 0)
                    && (cmbDetailBankAccountCode.GetSelectedString() != null))
                {
                    AAccountCode = cmbDetailBankAccountCode.GetSelectedString();
                }
            }

            if (FAccountTable != null)
            {
                currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });
            }

            if (currentAccountRow != null)
            {
                retVal = currentAccountRow.AccountActiveFlag;
            }

            return retVal;
        }

        private bool CostCentreIsActive(string ACostCentreCode = "")
        {
            bool retVal = true;

            ACostCentreRow currentCostCentreRow = null;

            //If empty, read value from combo
            if (ACostCentreCode == string.Empty)
            {
                if ((FCostCentreTable != null) && (cmbDetailBankCostCentre.SelectedIndex != -1) && (cmbDetailBankCostCentre.Count > 0)
                    && (cmbDetailBankCostCentre.GetSelectedString() != null))
                {
                    ACostCentreCode = cmbDetailBankCostCentre.GetSelectedString();
                }
            }

            if (FCostCentreTable != null)
            {
                currentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });
            }

            if (currentCostCentreRow != null)
            {
                retVal = currentCostCentreRow.CostCentreActiveFlag;
            }

            return retVal;
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

        private void ShowDetailsManual(ARecurringGiftBatchRow ARow)
        {
            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactions(ARow != null);

            if (ARow == null)
            {
                FSelectedBatchNumber = -1;
                return;
            }

            RefreshBankAccountAndCostCentreFilters(FActiveOnly, ARow);

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode = false;
            UpdateChangeableStatus();

            RefreshCurrencyControls(FPreviouslySelectedDetailRow.CurrencyCode);

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
                ((TFrmRecurringGiftBatch)ParentForm).SelectTab(TFrmRecurringGiftBatch.eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewARecurringGiftBatch();

            txtDetailBatchDescription.Focus();

            UpdateRecordNumberDisplay();

            ((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges();
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringGiftBatchRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            int batchNumber = ARowToDelete.BatchNumber;

            bool newBatch = (ARowToDelete.RowState == DataRowState.Added);

            try
            {
                ACompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} deleted successfully."),
                    batchNumber);

                //clear any transactions currently being editied in the Transaction Tab
                ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                if (!newBatch)
                {
                    //Load tables afresh
                    FMainDS.ARecurringGiftDetail.Clear();
                    FMainDS.ARecurringGift.Clear();
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(FLedgerNumber, batchNumber));
                }

                //Delete transactions
                for (int i = FMainDS.ARecurringGiftDetail.Count - 1; i >= 0; i--)
                {
                    FMainDS.ARecurringGiftDetail[i].Delete();
                }

                for (int i = FMainDS.ARecurringGift.Count - 1; i >= 0; i--)
                {
                    FMainDS.ARecurringGift[i].Delete();
                }

                // Delete the recurring batch row.
                ARowToDelete.Delete();

                //FMainDS.AcceptChanges();
                FPreviouslySelectedDetailRow = null;

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            UpdateRecordNumberDisplay();

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ARecurringGiftBatchRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                //causes saving issues
                //UpdateLedgerTableSettings();

                if (((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges())
                {
                    MessageBox.Show(ACompletionMessage, Catalog.GetString("Deletion Completed"));
                }
                else
                {
                    MessageBox.Show("Unable to save after deletion of batch! Try saving manually and closing and reopening the form.");
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
            }
            else if (!ADeletionPerformed)
            {
                //message to user
            }

            UpdateChangeableStatus();

            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactions((grdDetails.Rows.Count > 1));

            SelectRowInGrid(grdDetails.GetFirstHighlightedRowIndex());
        }

        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            FSelectedBatchMethodOfPayment = cmbDetailMethodOfPaymentCode.GetSelectedString();

            if ((FSelectedBatchMethodOfPayment != null) && (FSelectedBatchMethodOfPayment.Length > 0))
            {
                ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateMethodOfPayment(false);
            }
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            String ACurrencyCode = cmbDetailCurrencyCode.GetSelectedString();

            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null))
            {
                FPreviouslySelectedDetailRow.CurrencyCode = ACurrencyCode;
                RefreshCurrencyControls(ACurrencyCode);
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

        /// Select a special batch number from outside
        public void SelectRecurringBatchNumber(Int32 ABatchNumber)
        {
            for (int i = 0; (i < FMainDS.ARecurringGiftBatch.Rows.Count); i++)
            {
                if (FMainDS.ARecurringGiftBatch[i].BatchNumber == ABatchNumber)
                {
                    SelectDetailRowByDataTableIndex(i);
                    break;
                }
            }
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
        }

        private void ParseHashTotal(ARecurringGiftBatchRow ARow)
        {
            decimal correctHashValue;

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
            if (FPreviouslySelectedDetailRow == null)
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

        private void SubmitBatch(System.Object sender, System.EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Batch before submitting."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then submit
                if (!((TFrmRecurringGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to submit
                    MessageBox.Show(Catalog.GetString(
                            "The recurring batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please fix the batch first and then submit it."),
                        Catalog.GetString("Submit Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if ((FPreviouslySelectedDetailRow.HashTotal != 0)
                && (FPreviouslySelectedDetailRow.BatchTotal != FPreviouslySelectedDetailRow.HashTotal))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The recurring gift batch total ({0}) for batch {1} does not equal the hash total ({2})."),
                        StringHelper.FormatUsingCurrencyCode(FPreviouslySelectedDetailRow.BatchTotal, FPreviouslySelectedDetailRow.CurrencyCode),
                        FPreviouslySelectedDetailRow.BatchNumber,
                        StringHelper.FormatUsingCurrencyCode(FPreviouslySelectedDetailRow.HashTotal, FPreviouslySelectedDetailRow.CurrencyCode)),
                    "Submit Recurring Gift Batch");

                txtDetailHashTotal.Focus();
                txtDetailHashTotal.SelectAll();
                return;
            }

            if (!LoadAllBatchData() || !AllowInactiveFieldValues())
            {
                return;
            }

            TFrmRecurringGiftBatchSubmit submitForm = new TFrmRecurringGiftBatchSubmit(FPetraUtilsObject.GetForm());
            try
            {
                ParentForm.ShowInTaskbar = false;
                submitForm.MainDS = FMainDS;
                submitForm.BatchRow = FPreviouslySelectedDetailRow;
                submitForm.ShowDialog();
            }
            finally
            {
                submitForm.Dispose();
                ParentForm.ShowInTaskbar = true;
            }
        }

        private Boolean LoadAllBatchData(int ABatchNumber = 0)
        {
            bool RetVal = false;

            DataView GiftDV = new DataView(FMainDS.ARecurringGift);
            DataView GiftDetailsDV = new DataView(FMainDS.ARecurringGiftDetail);

            bool NoGiftRows = true;
            bool NoGiftDetailRows = true;

            if ((ABatchNumber == 0) && (FPreviouslySelectedDetailRow != null))
            {
                ABatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
            }
            else if (ABatchNumber == 0)
            {
                return RetVal;
            }

            try
            {
                // now load all gift data for this batch
                GiftDV.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber);
                GiftDetailsDV.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber);

                if (GiftDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(FLedgerNumber, FSelectedBatchNumber));
                }

                NoGiftRows = (GiftDV.Count == 0);
                NoGiftDetailRows = (GiftDetailsDV.Count == 0);

                if ((NoGiftRows && !NoGiftDetailRows)
                    || (!NoGiftRows && NoGiftDetailRows))
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The recurring gift batch {0} contains orphaned gift records. PLEASE DELETE THE RECURRING BATCH AND RECREATE!"),
                            FPreviouslySelectedDetailRow.BatchNumber),
                        Catalog.GetString("Orphaned Data"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return RetVal;
                }

                //Restrict now to active gifts only
                GiftDV.RowFilter = String.Format("{0}={1} And {2}=true",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber,
                    ARecurringGiftTable.GetActiveDBName());

                if (GiftDV.Count == 0)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString(
                                    "The recurring gift batch {0} has no active gifts. Do you still want to submit?"),
                                FPreviouslySelectedDetailRow.BatchNumber),
                            Catalog.GetString("Submit Empty Batch"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return RetVal;
                    }
                }

                RetVal = true;
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        private bool AllowInactiveFieldValues()
        {
            if (!AccountIsActive(FPreviouslySelectedDetailRow.BankAccountCode) || !CostCentreIsActive(FPreviouslySelectedDetailRow.BankCostCentre))
            {
                if (MessageBox.Show(String.Format(Catalog.GetString(
                                "Recurring batch no. {0} contains an inactive bank account or cost centre code. Do you want to continue submitting?"),
                            FPreviouslySelectedDetailRow.BatchNumber),
                        Catalog.GetString("Inactive Bank Account/Cost Centre Code"), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshCurrencyControls(string ACurrencyCode)
        {
            txtDetailHashTotal.CurrencyCode = ACurrencyCode;
            ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(ACurrencyCode);
        }

        private void UpdateLedgerTableSettings()
        {
            int LedgerLastRecurringGiftBatchNumber = 0;

            //Update the last recurring gift batch number
            DataView RecurringGiftBatchDV = new DataView(FMainDS.ARecurringGiftBatch);

            RecurringGiftBatchDV.RowFilter = string.Empty;
            RecurringGiftBatchDV.Sort = string.Format("{0} DESC",
                ARecurringGiftBatchTable.GetBatchNumberDBName());

            //Recurring batch numbers can be reused so reset current highest number
            if (RecurringGiftBatchDV.Count > 0)
            {
                LedgerLastRecurringGiftBatchNumber = (int)(RecurringGiftBatchDV[0][ARecurringGiftBatchTable.GetBatchNumberDBName()]);
            }

            if (FMainDS.ALedger[0].LastRecGiftBatchNumber != LedgerLastRecurringGiftBatchNumber)
            {
                FMainDS.ALedger[0].LastRecGiftBatchNumber = LedgerLastRecurringGiftBatchNumber;
            }
        }
    }
}