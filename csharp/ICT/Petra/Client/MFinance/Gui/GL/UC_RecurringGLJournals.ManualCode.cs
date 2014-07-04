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
        private Int32 FLedgerNumber = -1;
        private Int32 FJournalNumberToDelete = -1;
        private ARecurringBatchRow FBatchRow = null;
        private string FCurrencyCodeForJournals = string.Empty;
        /// <summary>
        /// The current active Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;

        /// <summary>
        /// flags if the Journal(s) have finished loading
        /// </summary>
        public bool FJournalsLoaded = false;

        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS RecurringJournalFMainDS()
        {
            return FMainDS;
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
            cmbDetailTransactionCurrency.SelectedValueChanged += new System.EventHandler(CurrencyCodeChanged);
        }

        private void RunOnceOnParentActivationManual()
        {
            //nothing to do
        }

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool FirstRun = (FLedgerNumber != ALedgerNumber);
            bool BatchChanged = (FBatchNumber != ABatchNumber);

            FJournalsLoaded = false;
            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                return;
            }

            //Check if same Journals as previously selected
            if (!FirstRun && !BatchChanged)
            {
                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }
            }
            else
            {
                // a different journal
                FLedgerNumber = ALedgerNumber;
                FBatchNumber = ABatchNumber;

                SetJournalDefaultView();
                FPreviouslySelectedDetailRow = null;

                if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndContent(ALedgerNumber, ABatchNumber));
                }

                ShowData();

                // Now set up the complete current filter
                FFilterAndFindObject.ApplyFilter();
            }

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

            //This will also call UpdateChangeableStatus
            SelectRowInGrid((BatchChanged || FirstRun) ? 1 : FPrevRowChangedRow);

            UpdateRecordNumberDisplay();
            SetRecordNumberDisplayProperties();
        }

        private void SetJournalDefaultView()
        {
            string rowFilter = string.Format("{0} = {1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.ARecurringJournal.DefaultView.RowFilter = rowFilter;
            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
            FFilterAndFindObject.CurrentActiveFilter = rowFilter;

            FMainDS.ARecurringJournal.DefaultView.Sort = String.Format("{0} DESC",
                ARecurringJournalTable.GetJournalNumberDBName()
                );
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
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

            UpdateChangeableStatus();
        }

        /// <summary>
        /// update the journal header fields from a batch
        /// </summary>
        /// <param name="ABatch"></param>
        public void UpdateHeaderTotals(ARecurringBatchRow ABatch)
        {
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ABatch.BatchNumber);

            foreach (DataRowView v in JournalDV)
            {
                ARecurringJournalRow r = (ARecurringJournalRow)v.Row;

                sumCredits += r.JournalCreditTotal;
                sumDebits += r.JournalDebitTotal;
            }

            FPetraUtilsObject.DisableDataChangedEvent();
            txtCurrentPeriod.Text = ABatch.BatchPeriod.ToString();
            txtDebit.NumberValueDecimal = sumDebits;
            ABatch.BatchDebitTotal = sumDebits;
            txtCredit.NumberValueDecimal = sumCredits;
            ABatch.BatchCreditTotal = sumCredits;
            txtControl.NumberValueDecimal = ABatch.BatchControlTotal;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private void ShowDetailsManual(ARecurringJournalRow ARow)
        {
            bool JournalRowIsNull = (ARow == null);

            grdDetails.TabStop = (!JournalRowIsNull);

            if (JournalRowIsNull)
            {
                btnAdd.Focus();
                FCurrencyCodeForJournals = string.Empty;
            }
            else
            {
                FCurrencyCodeForJournals = ARow.TransactionCurrency;
            }

            //Enable the transactions tab accordingly
            ((TFrmRecurringGLBatch)ParentForm).EnableTransactions(!JournalRowIsNull);

            UpdateChangeableStatus();
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
            if ((FPreviouslySelectedDetailRow == null) || (grdDetails.Rows.Count == 1))
            {
                ALedgerRow LedgerRow =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                FCurrencyCodeForJournals = LedgerRow.BaseCurrency;
            }

            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewARecurringJournal();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableTransactions();
            }
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

            ANewRow.TransactionCurrency = FCurrencyCodeForJournals;
            ANewRow.ExchangeRateToBase = 1;
            ANewRow.DateEffective = FBatchRow.DateEffective;
            ANewRow.JournalPeriod = FBatchRow.BatchPeriod;
        }

        /// initialise some comboboxes
        private void BeforeShowDetailsManual(ARecurringJournalRow ARow)
        {
            // SubSystemCode: the user can only select GL, but the system can generate eg. AP journals or GR journals
            this.cmbDetailSubSystemCode.Items.Clear();
            this.cmbDetailSubSystemCode.Items.AddRange(new object[] { ARow.SubSystemCode });

            //TFinanceControls.InitialiseTransactionTypeList(ref cmbDetailTransactionTypeCode, FLedgerNumber, ARow.SubSystemCode);
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TFrmRecurringGLBatch.eGLTabs.RecurringTransactions);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean IsChangeable = (!FPetraUtilsObject.DetailProtectedMode && GetBatchRow() != null);

            Boolean JournalUpdatable = (FPreviouslySelectedDetailRow != null);

            this.btnDelete.Enabled = IsChangeable && JournalUpdatable;
            this.btnAdd.Enabled = IsChangeable;
            pnlDetails.Enabled = IsChangeable && JournalUpdatable;
            pnlDetailsProtected = !IsChangeable;

            if (!IsChangeable)
            {
                FPetraUtilsObject.DisableSaveButton();
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
        private bool DeleteRowManual(ARecurringJournalRow ARowToDelete, ref string ACompletionMessage)
        {
            int batchNumber = ARowToDelete.BatchNumber;

            FJournalNumberToDelete = ARowToDelete.JournalNumber;
            bool deletionSuccessful = false;

            // Delete on client side data through views that is already loaded. Data that is not
            // loaded yet will be deleted with cascading delete on server side so we don't have
            // to worry about this here.

            ACompletionMessage = String.Format(Catalog.GetString("Journal no.: {0} deleted successfully."),
                FJournalNumberToDelete);

            try
            {
                //clear any transactions currently being editied in the Transaction Tab
                ((TFrmRecurringGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                // Delete the associated recurring transaction analysis attributes
                DataView viewRecurringTransAnalAttrib = new DataView(FMainDS.ARecurringTransAnalAttrib);
                viewRecurringTransAnalAttrib.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    batchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumberToDelete
                    );

                foreach (DataRowView row in viewRecurringTransAnalAttrib)
                {
                    row.Delete();
                }

                // Delete the associated recurring transactions
                DataView viewRecurringTransaction = new DataView(FMainDS.ARecurringTransaction);
                viewRecurringTransaction.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    batchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumberToDelete
                    );

                foreach (DataRowView row in viewRecurringTransaction)
                {
                    row.Delete();
                }

                // Delete the recurring batch row.
                ARowToDelete.Delete();

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

            return deletionSuccessful;
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
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                RenumberJournals();
                SetBatchLastJournalNumber();
                UpdateHeaderTotals(GetBatchRow());

                if (((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
                {
                    MessageBox.Show(ACompletionMessage, Catalog.GetString("Deletion Completed"));
                }
                else
                {
                    MessageBox.Show(
                        "Unable to save after deletion and renumbering remaining recurring journals! Try saving manually and closing and reopening the form.");
                }
            }

            if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
            {
                ClearControls();
            }

            ((TFrmRecurringGLBatch)ParentForm).EnableTransactions((grdDetails.Rows.Count > 1));
        }

        private void DeleteNewRecords(int ABatchNumber, int AJournalNumber)
        {
            // Delete the associated recurring transaction analysis attributes
            DataView viewRecurringTransAnalAttrib = new DataView(FMainDS.ARecurringTransAnalAttrib);

            viewRecurringTransAnalAttrib.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                AJournalNumber
                );

            foreach (DataRowView row in viewRecurringTransAnalAttrib)
            {
                row.Delete();
            }

            // Delete the associated recurring transactions
            DataView viewRecurringTransaction = new DataView(FMainDS.ARecurringTransaction);
            viewRecurringTransaction.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalNumber
                );

            foreach (DataRowView row in viewRecurringTransaction)
            {
                row.Delete();
            }
        }

        private void RenumberJournals()
        {
            bool jrnlsRenumbered = false;

            DataView jrnlView = new DataView(FMainDS.ARecurringJournal);
            DataView transView = new DataView(FMainDS.ARecurringTransaction);
            DataView attrView = new DataView(FMainDS.ARecurringTransAnalAttrib);

            //Reduce all trans and journal data by 1 in JournalNumber field
            //Reduce those with higher transaction number by one
            jrnlView.RowFilter = String.Format("{0} = {1} AND {2} > {3}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringJournalTable.GetJournalNumberDBName(),
                FJournalNumberToDelete);

            jrnlView.Sort = String.Format("{0} ASC",
                ARecurringJournalTable.GetJournalNumberDBName());

            jrnlsRenumbered = (jrnlView.Count > 0);

            // Delete the associated transaction analysis attributes
            //  if attributes do exist, and renumber those above
            foreach (DataRowView jV in jrnlView)
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
                transView.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    currentJnrlNumber);

                transView.Sort = String.Format("{0} ASC, {1} ASC",
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                //Iterate through higher number attributes and transaction numbers and reduce by one
                ARecurringTransactionRow transRowCurrent = null;

                foreach (DataRowView gv in transView)
                {
                    transRowCurrent = (ARecurringTransactionRow)gv.Row;

                    GLBatchTDSARecurringTransactionRow newTransRow = FMainDS.ARecurringTransaction.NewRowTyped(true);

                    newTransRow.ItemArray = transRowCurrent.ItemArray;

                    //reduce journal number by 1 in the new row
                    newTransRow.JournalNumber--;

                    FMainDS.ARecurringTransaction.Rows.Add(newTransRow);

                    //Repeat process for attributes that belong to current transaction
                    attrView.RowFilter = String.Format("{0} = {1} And {2} = {3} And {4} = {5}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        currentJnrlNumber,
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        transRowCurrent.TransactionNumber);

                    attrView.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                    // Delete the associated transaction analysis attributes
                    //  if attributes do exist, and renumber those above
                    if (attrView.Count > 0)
                    {
                        //Iterate through higher number attributes and transaction numbers and reduce by one
                        ARecurringTransAnalAttribRow attrRowCurrent = null;

                        foreach (DataRowView rV in attrView)
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

            if (jrnlsRenumbered)
            {
                FPetraUtilsObject.SetChangedFlag();
            }

            //Need to refresh FPreviouslySelectedDetailRow else it points to a deleted row
            SelectRowInGrid(grdDetails.GetFirstHighlightedRowIndex());
        }

        private void SetBatchLastJournalNumber()
        {
            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            JournalDV.Sort = String.Format("{0} DESC",
                ARecurringJournalTable.GetJournalNumberDBName()
                );

            if (JournalDV.Count > 0)
            {
                ARecurringJournalRow jrnlRow = (ARecurringJournalRow)JournalDV[0].Row;
                GetBatchRow().LastJournal = jrnlRow.JournalNumber;
            }
            else
            {
                GetBatchRow().LastJournal = 0;
            }
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
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
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
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            FCurrencyCodeForJournals = cmbDetailTransactionCurrency.GetSelectedString();

            DataView CurrentJournalsDV = new DataView(FMainDS.ARecurringJournal);

            CurrentJournalsDV.RowFilter = string.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            if (CurrentJournalsDV.Count > 1)
            {
                foreach (DataRowView drv in CurrentJournalsDV)
                {
                    ARecurringJournalRow rjr = (ARecurringJournalRow)drv.Row;

                    rjr.TransactionCurrency = FCurrencyCodeForJournals;
                }
            }
        }
    }
}