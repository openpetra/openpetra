//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Windows.Forms;
using System.Drawing;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Gui.Setup;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLJournals
    {
        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS RecurringJournalFMainDS()
        {
            return FMainDS;
        }

        /// <summary>
        /// The current active Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;

        /// <summary>
        /// flags if the Journal(s) have finished loading
        /// </summary>
        public bool FJournalsLoaded = false;

        private string FLedgerBaseCurrency = string.Empty;
        private Int32 FLedgerNumber = -1;
        private ARecurringBatchRow FBatchRow = null;
        private Int32 FJournalNumberToDelete = -1;

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
        }

        /// <summary>
        /// Get current Journal row
        /// </summary>
        /// <returns></returns>
        public GLBatchTDSARecurringJournalRow GetCurrentJournalRow()
        {
            return (GLBatchTDSARecurringJournalRow) this.GetSelectedDetailRow();
        }

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ACurrentBatchRow"></param>
        public void LoadJournals(ARecurringBatchRow ACurrentBatchRow)
        {
            FJournalsLoaded = false;

            FBatchRow = ACurrentBatchRow;

            if (FBatchRow == null)
            {
                return;
            }

            Int32 CurrentLedgerNumber = ACurrentBatchRow.LedgerNumber;
            Int32 CurrentBatchNumber = ACurrentBatchRow.BatchNumber;

            bool FirstRun = (FLedgerNumber != CurrentLedgerNumber);
            bool BatchChanged = (FBatchNumber != CurrentBatchNumber);

            if (FirstRun)
            {
                FLedgerNumber = CurrentLedgerNumber;
            }

            if (BatchChanged)
            {
                FBatchNumber = CurrentBatchNumber;
            }

            //Check if same Journals as previously selected
            if (!(FirstRun || BatchChanged))
            {
                //Need to reconnect FPrev in some circumstances
                if (FPreviouslySelectedDetailRow == null)
                {
                    DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(FPrevRowChangedRow);

                    if (rowView != null)
                    {
                        FPreviouslySelectedDetailRow = (GLBatchTDSARecurringJournalRow)(rowView.Row);
                    }
                }

                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }
            }
            else
            {
                // a different journal
                SetJournalDefaultView();
                FPreviouslySelectedDetailRow = null;

                //Load Journals
                if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(FLedgerNumber, FBatchNumber));
                }

                //Ensure Last Batch Number is correct, needed to correct old Petra data
                if ((FBatchRow.LastJournal != FMainDS.ARecurringJournal.DefaultView.Count)
                    && GLRoutines.UpdateRecurringBatchLastJournal(ref FMainDS, ref FBatchRow))
                {
                    FPetraUtilsObject.SetChangedFlag();
                }

                ShowData();

                // Now set up the complete current filter
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FMainDS.ARecurringJournal.DefaultView.RowFilter, true);
                FFilterAndFindObject.ApplyFilter();
            }

            int nRowToSelect = (BatchChanged || FirstRun) ? 1 : FPrevRowChangedRow;

            //This will also call UpdateChangeableStatus
            SelectRowInGrid(nRowToSelect);

            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            //Check for incorrect Exchange rate to base
            foreach (DataRowView drv in FMainDS.ARecurringJournal.DefaultView)
            {
                ARecurringJournalRow rjr = (ARecurringJournalRow)drv.Row;

                if (rjr.ExchangeRateToBase == 0)
                {
                    rjr.ExchangeRateToBase = 1;
                    FPetraUtilsObject.HasChanges = true;
                }
            }

            FJournalsLoaded = true;

            //Check on batch totals
            if (GLRoutines.UpdateRecurringBatchTotals(ref FMainDS, ref FBatchRow))
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void SetJournalDefaultView()
        {
            string DVRowFilter = string.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.ARecurringJournal.DefaultView.RowFilter = DVRowFilter;
            FMainDS.ARecurringJournal.DefaultView.Sort = String.Format("{0} DESC",
                ARecurringJournalTable.GetJournalNumberDBName()
                );

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(DVRowFilter, true);
            FFilterAndFindObject.CurrentActiveFilter = DVRowFilter;
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                if (FLedgerNumber != -1)
                {
                    txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                    txtBatchNumber.Text = FBatchNumber.ToString();
                }

                if (FPreviouslySelectedDetailRow != null)
                {
                    txtDebit.NumberValueDecimal = FPreviouslySelectedDetailRow.JournalDebitTotal;
                    txtCredit.NumberValueDecimal = FPreviouslySelectedDetailRow.JournalCreditTotal;
                    txtControl.NumberValueDecimal =
                        FPreviouslySelectedDetailRow.JournalDebitTotal -
                        FPreviouslySelectedDetailRow.JournalCreditTotal;
                }
            }
            finally
            {
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        /// <summary>
        /// update the journal header fields from a batch
        /// </summary>
        /// <param name="ABatchRowToUpdate"></param>
        public void UpdateHeaderTotals(ARecurringBatchRow ABatchRowToUpdate)
        {
            decimal SumDebits = 0.0M;
            decimal SumCredits = 0.0M;

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ABatchRowToUpdate.BatchNumber);
            JournalDV.RowStateFilter = DataViewRowState.CurrentRows;

            foreach (DataRowView v in JournalDV)
            {
                ARecurringJournalRow r = (ARecurringJournalRow)v.Row;

                SumCredits += r.JournalCreditTotal;
                SumDebits += r.JournalDebitTotal;
            }

            FPetraUtilsObject.DisableDataChangedEvent();

            txtDebit.NumberValueDecimal = SumDebits;
            txtCredit.NumberValueDecimal = SumCredits;
            txtControl.NumberValueDecimal = ABatchRowToUpdate.BatchControlTotal;

            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private void ShowDetailsManual(ARecurringJournalRow ARow)
        {
            bool JournalRowIsNull = (ARow == null);

            grdDetails.TabStop = (!JournalRowIsNull);

            //Enable the transactions tab accordingly
            ((TFrmRecurringGLBatch)ParentForm).EnableTransactions(!JournalRowIsNull);

            UpdateChangeableStatus();

            if (JournalRowIsNull)
            {
                btnAdd.Focus();
            }
        }

        private ARecurringBatchRow GetBatchRow()
        {
            return ((TFrmRecurringGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// add a new journal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewARecurringJournal();

            txtDetailJournalDescription.Text = FBatchRow.BatchDescription;

            //Automatically save changes
            ((TFrmRecurringGLBatch) this.ParentForm).SaveChanges();

            //Enable the Journals if not already enabled
            ((TFrmRecurringGLBatch)ParentForm).EnableJournals();
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSARecurringJournalRow ANewRow)
        {
            if ((ANewRow == null) || (FLedgerNumber == -1))
            {
                return;
            }

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            ANewRow.LedgerNumber = FBatchRow.LedgerNumber;
            ANewRow.BatchNumber = FBatchRow.BatchNumber;
            ANewRow.JournalNumber = ++FBatchRow.LastJournal;

            // manually created journals are all GL
            ANewRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
            ANewRow.TransactionTypeCode = MFinanceConstants.STANDARD_JOURNAL;

            ANewRow.TransactionCurrency = LedgerRow.BaseCurrency;
            ANewRow.ExchangeRateToBase = 1;
            ANewRow.DateEffective = FBatchRow.DateEffective;
            ANewRow.JournalPeriod = FBatchRow.BatchPeriod;
            ANewRow.JournalDebitTotalBase = 0.0M;
            ANewRow.JournalCreditTotalBase = 0.0M;
        }

        /// initialise some comboboxes
        private void BeforeShowDetailsManual(ARecurringJournalRow ARow)
        {
            // SubSystemCode: the user can only select GL, but the system can generate eg. AP journals or GR journals
            this.cmbDetailSubSystemCode.Items.Clear();
            this.cmbDetailSubSystemCode.Items.AddRange(new object[] { ARow.SubSystemCode });

            TFinanceControls.InitialiseTransactionTypeList(ref cmbDetailTransactionTypeCode, FLedgerNumber, ARow.SubSystemCode);
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TGLBatchEnums.eGLTabs.Transactions);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean IsChangeable = ((!FPetraUtilsObject.DetailProtectedMode)
                                    && (GetBatchRow() != null));

            Boolean JournalUpdatable = (FPreviouslySelectedDetailRow != null);

            //Process buttons
            this.btnDelete.Enabled = (IsChangeable && JournalUpdatable);
            this.btnAdd.Enabled = IsChangeable;

            pnlDetails.Enabled = (IsChangeable && JournalUpdatable);
            pnlDetailsProtected = !IsChangeable;
        }

        private void ClearControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();

            txtDetailJournalDescription.Clear();
            cmbDetailTransactionTypeCode.SelectedIndex = -1;
            cmbDetailTransactionCurrency.SelectedIndex = -1;

            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        /// <param name="ABatchToClear"></param>
        /// <param name="AResetFBatchNumber"></param>
        public void ClearCurrentSelection(int ABatchToClear = 0, bool AResetFBatchNumber = true)
        {
            //Called from Batch tab, so no need to check for GetDetailsFromControls()
            // as tab change does that and current tab is Batch tab.
            if (this.FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            else if ((ABatchToClear > 0) && (FPreviouslySelectedDetailRow.BatchNumber != ABatchToClear))
            {
                return;
            }

            //Set selection to null
            this.FPreviouslySelectedDetailRow = null;

            if (AResetFBatchNumber)
            {
                FBatchNumber = -1;
            }
        }

        private void ValidateDataDetailsManual(ARecurringJournalRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GL.ValidateRecurringGLJournalManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);

            //TODO: remove this once database definition is set for Batch Description to be NOT NULL
            // Description is mandatory then make sure it is set
            if (txtDetailJournalDescription.Text.Length == 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringJournalTable.ColumnJournalDescriptionId];
                ValidationContext = String.Format("Recurring Batch no.: {0}, Journal no.: {1}",
                    ARow.BatchNumber,
                    ARow.JournalNumber);

                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.JournalDescription,
                    "Description of " + ValidationContext,
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }

        private void SetFocusToDetailsGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                grdDetails.Focus();
            }
        }

        /// <summary>
        /// Shows the Filter/Find UserControl and switches to the Find Tab.
        /// </summary>
        public void ShowFindPanel()
        {
            if (FFilterAndFindObject.FilterFindPanel == null)
            {
                FFilterAndFindObject.ToggleFilter();
            }

            FFilterAndFindObject.FilterFindPanel.DisplayFindTab();
        }

        private void CurrencyCodeChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            string NewCurrency = cmbDetailTransactionCurrency.GetSelectedString();

            if (FPreviouslySelectedDetailRow.TransactionCurrency != NewCurrency)
            {
                FPreviouslySelectedDetailRow.TransactionCurrency = NewCurrency;
                FPreviouslySelectedDetailRow.ExchangeRateToBase = (NewCurrency == FLedgerBaseCurrency) ? 1.0m : 0.0m;
            }
        }

        private void RunOnceOnParentActivationManual()
        {
            // On a screen with no batches we cannot use FLedgerNumber because that is set by LoadJournals
            int batchFormLedger = ((TFrmRecurringGLBatch)ParentForm).LedgerNumber;

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, batchFormLedger))[0];

            FLedgerBaseCurrency = LedgerRow.BaseCurrency;

            txtControl.CurrencyCode = TTxtCurrencyTextBox.CURRENCY_STANDARD_2_DP;
            txtCredit.CurrencyCode = FLedgerBaseCurrency;
            txtDebit.CurrencyCode = FLedgerBaseCurrency;
        }

        /// <summary>
        /// Delete journal data from current recurring GL Batch
        /// </summary>
        /// <param name="ABatchNumber"></param>
        public void DeleteRecurringJournalData(Int32 ABatchNumber)
        {
            DataView JournalDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ABatchNumber);

            JournalDV.Sort = String.Format("{0} DESC", ARecurringJournalTable.GetJournalNumberDBName());

            foreach (DataRowView dr in JournalDV)
            {
                dr.Delete();
            }
        }

        private bool PreDeleteManual(ARecurringJournalRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            if (FPreviouslySelectedDetailRow != null)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete journal no. {0} from recurring Batch {1}?"),
                    ARowToDelete.JournalNumber,
                    ARowToDelete.BatchNumber);
            }

            return allowDeletion;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(GLBatchTDSARecurringJournalRow ARowToDelete, ref string ACompletionMessage)
        {
            //Assign default value(s)
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            //Delete current row
            ARowToDelete.RejectChanges();
            ShowDetails(ARowToDelete);

            //Take a backup of FMainDS
            GLBatchTDS BackupMainDS = null;
            //Pass a copy of FMainDS to the server-side delete method.
            GLBatchTDS TempDS = (GLBatchTDS)FMainDS.Copy();
            TempDS.Merge(FMainDS);

            int CurrentBatchNumber = ARowToDelete.BatchNumber;
            bool CurrentBatchJournalTransactionsLoadedAndCurrent = false;
            FJournalNumberToDelete = ARowToDelete.JournalNumber;
            int TopMostJrnlNo = FBatchRow.LastJournal;

            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.DELETINGJOURNAL;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GLBatchTDS)FMainDS.GetChangesTyped(false);

                //Check if current batch transactions are loaded and being viewed in their tab
                CurrentBatchJournalTransactionsLoadedAndCurrent = (FMyForm.GetTransactionsControl().FBatchNumber == CurrentBatchNumber
                                                                   && FMyForm.GetTransactionsControl().FJournalNumber == FJournalNumberToDelete);

                //Save and check for inactive values
                FPetraUtilsObject.SetChangedFlag();

                if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction, false, !CurrentBatchJournalTransactionsLoadedAndCurrent))
                {
                    string msg = String.Format(Catalog.GetString("Recurring Journal {0} has not been deleted."),
                        FJournalNumberToDelete);

                    MessageBox.Show(msg,
                        Catalog.GetString("Recurring GL Batch Deletion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return false;
                }

                //Remove any changes to current batch that may cause validation issues
                PrepareJournalDataForDeleting(CurrentBatchNumber, FJournalNumberToDelete, true);

                if (CurrentBatchJournalTransactionsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    FMyForm.GetTransactionsControl().ClearCurrentSelection(CurrentBatchNumber, FJournalNumberToDelete);
                }

                //Load all journals for this batch
                TempDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndRelatedTablesForBatch(FLedgerNumber, FBatchNumber));
                TempDS.AcceptChanges();

                //Clear the transactions and load newly saved dataset
                FMainDS.ARecurringTransAnalAttrib.Clear();
                FMainDS.ARecurringTransaction.Clear();
                FMainDS.ARecurringJournal.Clear();

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.ProcessRecurrJrnlTransAttribForDeletion(TempDS, FLedgerNumber, FBatchNumber,
                        TopMostJrnlNo, FJournalNumberToDelete));

                FPreviouslySelectedDetailRow = null;
                FPetraUtilsObject.SetChangedFlag();

                ACompletionMessage = String.Format(Catalog.GetString("Recurring Journal no.: {0} deleted successfully."),
                    FJournalNumberToDelete);

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Normally set in PostDeleteManual
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;

                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                SetJournalDefaultView();
                FFilterAndFindObject.ApplyFilter();
                this.Cursor = Cursors.Default;
            }

            return DeletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ARecurringJournalRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/

            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                if (ADeletionPerformed)
                {
                    UpdateChangeableStatus();

                    if (!pnlDetails.Enabled)
                    {
                        ClearControls();
                    }

                    //Always update LastTransactionNumber first before updating totals
                    GLRoutines.UpdateRecurringBatchLastJournal(ref FMainDS, ref FBatchRow);
                    GLRoutines.UpdateRecurringBatchTotals(ref FMainDS, ref FBatchRow);
                    UpdateHeaderTotals(FBatchRow);

                    FMyForm.SaveChangesManual();

                    MessageBox.Show(ACompletionMessage,
                        Catalog.GetString("Deletion Successful"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else if (!AAllowDeletion && (ACompletionMessage.Length > 0))
                {
                    //message to user
                    MessageBox.Show(ACompletionMessage,
                        "Deletion not allowed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (!ADeletionPerformed && (ACompletionMessage.Length > 0))
                {
                    //message to user
                    MessageBox.Show(ACompletionMessage,
                        "Deletion failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
            }

            //Enable Transaction tab accordingly
            ((TFrmRecurringGLBatch)ParentForm).EnableTransactions((grdDetails.Rows.Count > 1));
        }

        private void RevertDataSet(GLBatchTDS AMainDS, GLBatchTDS ABackupDS, int ASelectRowInGrid = 0)
        {
            if ((ABackupDS != null) && (AMainDS != null))
            {
                AMainDS.RejectChanges();
                AMainDS.Merge(ABackupDS);

                if (ASelectRowInGrid > 0)
                {
                    SelectRowInGrid(ASelectRowInGrid);
                }
            }
        }

        /// <summary>
        /// Undo all changes to the specified batch ready to cancel it.
        ///  This avoids unecessary validation errors when cancelling.
        /// </summary>
        /// <param name="ACurrentBatch"></param>
        /// <param name="AJournalToDelete"></param>
        /// <param name="ARedisplay"></param>
        public void PrepareJournalDataForDeleting(Int32 ACurrentBatch, Int32 AJournalToDelete, Boolean ARedisplay)
        {
            //This code will only be called when the Batch tab is active.

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
            DataView TransAnalDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            JournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatch,
                ARecurringJournalTable.GetJournalNumberDBName(),
                AJournalToDelete);

            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentBatch,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalToDelete);

            TransAnalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ACurrentBatch,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                AJournalToDelete);

            //Work from lowest level up
            if (TransAnalDV.Count > 0)
            {
                TransAnalDV.Sort = String.Format("{0}, {1}, {2}",
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                foreach (DataRowView drv in TransAnalDV)
                {
                    ARecurringTransAnalAttribRow transAnalRow = (ARecurringTransAnalAttribRow)drv.Row;

                    if (transAnalRow.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (transAnalRow.RowState != DataRowState.Unchanged)
                    {
                        transAnalRow.RejectChanges();
                    }
                }
            }

            if (TransDV.Count > 0)
            {
                TransDV.Sort = String.Format("{0}, {1}",
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                foreach (DataRowView drv in TransDV)
                {
                    ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

                    if (transRow.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (transRow.RowState != DataRowState.Unchanged)
                    {
                        transRow.RejectChanges();
                    }
                }
            }

            if (JournalDV.Count > 0)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)JournalDV[0].Row;

                //No need to check for Added state as new batches are always saved
                // on creation

                if (journalRow.RowState != DataRowState.Unchanged)
                {
                    journalRow.RejectChanges();
                }

                if (ARedisplay)
                {
                    ShowDetails(journalRow);
                }
            }

            if (TransDV.Count == 0)
            {
                //Load all related data for batch ready to delete/cancel
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionAndRelatedTablesForJournal(FLedgerNumber, ACurrentBatch,
                        AJournalToDelete));
            }
        }

        private void ProcessNewlyAddedJournalRowForDeletion(int AJournalNumberToDelete)
        {
            GLBatchTDS BackupDS = (GLBatchTDS)FMainDS.Copy();

            BackupDS.Merge(FMainDS);

            try
            {
                // Delete the associated recurring transaction analysis attributes
                DataView attributesDV = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV.RowFilter = string.Format("{0}={1} And {2}={3}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete
                    );

                foreach (DataRowView attrDRV in attributesDV)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.Delete();
                }

                // Delete the associated recurring transactions
                DataView transactionsDV = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete
                    );

                foreach (DataRowView transDRV in transactionsDV)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.Delete();
                }

                // Delete the recurring journal
                DataView journalDV = new DataView(FMainDS.ARecurringJournal);
                journalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringJournalTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete
                    );

                foreach (DataRowView journalDRV in journalDV)
                {
                    ARecurringJournalRow jrnlRow = (ARecurringJournalRow)journalDRV.Row;
                    jrnlRow.Delete();
                }

                //Renumber the journals, transactions and attributes
                DataView attributesDV2 = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV2.RowFilter = string.Format("{0}={1} And {2}>{3}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete);
                attributesDV2.Sort = String.Format("{0} ASC", ARecurringTransAnalAttribTable.GetJournalNumberDBName());

                foreach (DataRowView attrDRV in attributesDV2)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.JournalNumber--;
                }

                DataView transactionsDV2 = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV2.RowFilter = string.Format("{0}={1} And {2}>{3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete);
                transactionsDV2.Sort = String.Format("{0} ASC", ARecurringTransactionTable.GetJournalNumberDBName());

                foreach (DataRowView transDRV in transactionsDV2)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.JournalNumber--;
                }

                DataView journalDV2 = new DataView(FMainDS.ARecurringJournal);
                journalDV2.RowFilter = string.Format("{0}={1} And {2}>{3}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringJournalTable.GetJournalNumberDBName(),
                    AJournalNumberToDelete);
                journalDV2.Sort = String.Format("{0} ASC", ARecurringJournalTable.GetJournalNumberDBName());

                foreach (DataRowView jrnlDRV in journalDV2)
                {
                    ARecurringJournalRow jrnlRow = (ARecurringJournalRow)jrnlDRV.Row;
                    jrnlRow.JournalNumber--;
                }
            }
            catch (Exception ex)
            {
                FMainDS.Merge(BackupDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
        }

        private void RenumberJournals()
        {
            bool JournalsRenumbered = false;

            DataView JrnlView = new DataView(FMainDS.ARecurringJournal);
            DataView TransView = new DataView(FMainDS.ARecurringTransaction);
            DataView AttrView = new DataView(FMainDS.ARecurringTransAnalAttrib);

            //Reduce all trans and journal data by 1 in JournalNumber field
            //Reduce those with higher transaction number by one
            JrnlView.RowFilter = String.Format("{0}={1} AND {2}>{3}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringJournalTable.GetJournalNumberDBName(),
                FJournalNumberToDelete);

            JrnlView.Sort = String.Format("{0} ASC",
                ARecurringJournalTable.GetJournalNumberDBName());

            JournalsRenumbered = (JrnlView.Count > 0);

            // Delete the associated transaction analysis attributes
            //  if attributes do exist, and renumber those above
            foreach (DataRowView jV in JrnlView)
            {
                GLBatchTDSARecurringJournalRow jrnlRowCurrent = (GLBatchTDSARecurringJournalRow)jV.Row;

                int currentJnrlNumber = jrnlRowCurrent.JournalNumber;

                //Copy current row down to fill gap and then delete it
                GLBatchTDSARecurringJournalRow newJrnlRow = FMainDS.ARecurringJournal.NewRowTyped(true);

                newJrnlRow.ItemArray = jrnlRowCurrent.ItemArray;

                //reduce journal number by 1 in the new row
                newJrnlRow.JournalNumber--;

                FMainDS.ARecurringJournal.Rows.Add(newJrnlRow);

                //Process Transactions
                TransView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    currentJnrlNumber);

                TransView.Sort = String.Format("{0} ASC, {1} ASC",
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                //Iterate through higher number attributes and transaction numbers and reduce by one
                ARecurringTransactionRow transRowCurrent = null;

                foreach (DataRowView gv in TransView)
                {
                    transRowCurrent = (ARecurringTransactionRow)gv.Row;

                    GLBatchTDSARecurringTransactionRow newTransRow = FMainDS.ARecurringTransaction.NewRowTyped(true);

                    newTransRow.ItemArray = transRowCurrent.ItemArray;

                    //reduce journal number by 1 in the new row
                    newTransRow.JournalNumber--;

                    FMainDS.ARecurringTransaction.Rows.Add(newTransRow);

                    //Repeat process for attributes that belong to current transaction
                    AttrView.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        currentJnrlNumber,
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        transRowCurrent.TransactionNumber);

                    AttrView.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                    // Delete the associated transaction analysis attributes
                    //  if attributes do exist, and renumber those above
                    if (AttrView.Count > 0)
                    {
                        //Iterate through higher number attributes and transaction numbers and reduce by one
                        ARecurringTransAnalAttribRow attrRowCurrent = null;

                        foreach (DataRowView rV in AttrView)
                        {
                            attrRowCurrent = (ARecurringTransAnalAttribRow)rV.Row;

                            ARecurringTransAnalAttribRow newAttrRow = FMainDS.ARecurringTransAnalAttrib.NewRowTyped(true);

                            newAttrRow.ItemArray = attrRowCurrent.ItemArray;

                            //reduce journal number by 1
                            newAttrRow.JournalNumber--;

                            FMainDS.ARecurringTransAnalAttrib.Rows.Add(newAttrRow);

                            attrRowCurrent.Delete();
                        }
                    }

                    transRowCurrent.Delete();
                }

                jrnlRowCurrent.Delete();
            }

            if (JournalsRenumbered)
            {
                FPetraUtilsObject.SetChangedFlag();
            }

            //Need to refresh FPreviouslySelectedDetailRow else it points to a deleted row
            SelectRowInGrid(grdDetails.GetFirstHighlightedRowIndex());
        }

        private void FilterToggledManual(bool AIsCollapsed)
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }
        }
    }
}