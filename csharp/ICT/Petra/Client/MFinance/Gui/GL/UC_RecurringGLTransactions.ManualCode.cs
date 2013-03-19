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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private string FTransactionCurrency = string.Empty;
        private string FBatchStatus = string.Empty;
        private string FJournalStatus = string.Empty;

        private ARecurringBatchRow FBatchRow = null;

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AForeignCurrencyName"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AJournalStatus"></param>
        public void LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string AForeignCurrencyName,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            string AJournalStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            FBatchRow = GetBatchRow();

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionCurrency == AForeignCurrencyName) && (FBatchStatus == ABatchStatus) && (FJournalStatus == AJournalStatus)
                && (FMainDS.ARecurringTransaction.DefaultView.Count > 0))
            {
                //Same as previously selected
                if ((FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (grdDetails.SelectedRowIndex() > 0))
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                return;
            }

            bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != AForeignCurrencyName);

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            FTransactionCurrency = AForeignCurrencyName;
            FBatchStatus = ABatchStatus;
            FJournalStatus = AJournalStatus;

            FPreviouslySelectedDetailRow = null;

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            FMainDS.ARecurringTransaction.DefaultView.RowFilter = string.Empty;

            if (FMainDS.ARecurringTransaction.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionWithAttributes(ALedgerNumber, ABatchNumber, AJournalNumber));
            }
            else
            {
                FMainDS.ARecurringTransaction.DefaultView.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                    ARecurringTransactionTable.GetLedgerNumberDBName(),
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    ARecurringTransactionTable.GetJournalNumberDBName()
                    );

                if (FMainDS.ARecurringTransaction.DefaultView.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber }) == -1)
                {
                    FMainDS.ARecurringTransaction.Clear();
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionWithAttributes(ALedgerNumber, ABatchNumber,
                            AJournalNumber));
                }
            }

            FMainDS.ARecurringTransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            FMainDS.ARecurringTransaction.DefaultView.Sort = String.Format("{0} ASC",
                ARecurringTransactionTable.GetTransactionNumberDBName()
                );

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransaction.DefaultView);

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            if (requireControlSetup)
            {
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, ActiveOnly, false, AForeignCurrencyName);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
            }

            ShowData();
            ShowDetails();

            btnNew.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;
            btnDelete.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;

            //This will update Batch and journal totals
            UpdateTotals();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                pnlDetails.Enabled = false;
            }
        }

        /// <summary>
        /// Unload the currently loaded transactions
        /// </summary>
        public void UnloadTransactions()
        {
            if (FMainDS.ARecurringTransaction.DefaultView.Count > 0)
            {
                FPreviouslySelectedDetailRow = null;
                FMainDS.ARecurringTransaction.Clear();
                //ClearControls();
            }
        }

        /// <summary>
        /// Update the effective date from outside
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        public void UpdateEffectiveDateForCurrentRow(DateTime AEffectiveDate)
        {
            if ((GetSelectedDetailRow() != null) && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                GetSelectedDetailRow().TransactionDate = AEffectiveDate;
                GetDetailsFromControls(GetSelectedDetailRow());
            }
        }

        /// <summary>
        /// Return the active transaction number and sets the Journal number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        public Int32 ActiveTransactionNumber(Int32 ALedgerNumber, Int32 ABatchNumber, ref Int32 AJournalNumber)
        {
            Int32 activeTrans = 0;

            if (FPreviouslySelectedDetailRow != null)
            {
                activeTrans = FPreviouslySelectedDetailRow.TransactionNumber;
                AJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
            }

            return activeTrans;
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private GLBatchTDSARecurringJournalRow GetJournalRow()
        {
            return ((TFrmRecurringGLBatch)ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        private ARecurringBatchRow GetBatchRow()
        {
            return ((TFrmRecurringGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        private void ShowAttributesTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TFrmRecurringGLBatch.eGLTabs.RecurringAttributes);
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FMainDS.ARecurringTransaction.RejectChanges();
            }
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewARecurringTransaction();

            if (pnlDetails.Enabled == false)
            {
                pnlDetails.Enabled = true;
            }

            // make sure analysis attributes are created
            ((TFrmRecurringGLBatch) this.ParentForm).EnableAttributes();

            cmbDetailCostCentreCode.Focus();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref GLBatchTDSARecurringTransactionRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefJournalRow">this can be null; otherwise this is the journal that the transaction should belong to</param>
        public void NewRowManual(ref GLBatchTDSARecurringTransactionRow ANewRow, ARecurringJournalRow ARefJournalRow)
        {
            if (ARefJournalRow == null)
            {
                ARefJournalRow = GetJournalRow();
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ++ARefJournalRow.LastTransactionNumber;
            ANewRow.TransactionDate = GetBatchRow().DateEffective;

            if (FPreviouslySelectedDetailRow != null)
            {
                ANewRow.AccountCode = FPreviouslySelectedDetailRow.AccountCode;
                ANewRow.CostCentreCode = FPreviouslySelectedDetailRow.CostCentreCode;

                if (ARefJournalRow.JournalCreditTotal != ARefJournalRow.JournalDebitTotal)
                {
                    ANewRow.Reference = FPreviouslySelectedDetailRow.Reference;
                    ANewRow.Narrative = FPreviouslySelectedDetailRow.Narrative;
                    ANewRow.TransactionDate = FPreviouslySelectedDetailRow.TransactionDate;
                    decimal Difference = ARefJournalRow.JournalDebitTotal - ARefJournalRow.JournalCreditTotal;
                    ANewRow.TransactionAmount = Math.Abs(Difference);
                    ANewRow.DebitCreditIndicator = Difference < 0;
                }
            }

            //If first row added
            if (grdDetails.Rows.Count == 2)
            {
                if (cmbDetailCostCentreCode.Count > 1)
                {
                    cmbDetailCostCentreCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailCostCentreCode.SelectedIndex = -1;
                }

                if (cmbDetailAccountCode.Count > 1)
                {
                    cmbDetailAccountCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailAccountCode.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();
                txtJournalNumber.Text = FJournalNumber.ToString();

                string TransactionCurrency = GetJournalRow().TransactionCurrency;
                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), TransactionCurrency);
                txtDebitAmount.CurrencySymbol = TransactionCurrency;
                txtCreditAmount.CurrencySymbol = TransactionCurrency;
                txtCreditTotalAmount.CurrencySymbol = TransactionCurrency;
                txtDebitTotalAmount.CurrencySymbol = TransactionCurrency;

                // foreign currency accounts only get transactions in that currency
                if (FTransactionCurrency != TransactionCurrency)
                {
                    string SelectedAccount = cmbDetailAccountCode.GetSelectedString();

                    // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
                    bool ActiveOnly = this.Enabled;

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, ActiveOnly, false, TransactionCurrency);

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount);

                    if ((GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED) && FPetraUtilsObject.HasChanges)
                    {
                        FPetraUtilsObject.DisableSaveButton();
                    }

                    FTransactionCurrency = TransactionCurrency;
                }
            }

            UpdateChangeableStatus();
        }

        private void ShowDetailsManual(ARecurringTransactionRow ARow)
        {
            txtJournalNumber.Text = FJournalNumber.ToString();

            if (ARow == null)
            {
                ((TFrmRecurringGLBatch)ParentForm).DisableAttributes();
                return;
            }

            if (ARow.DebitCreditIndicator)
            {
                txtDebitAmount.NumberValueDecimal = ARow.TransactionAmount;
                txtCreditAmount.NumberValueDecimal = 0;
            }
            else
            {
                txtDebitAmount.NumberValueDecimal = 0;
                txtCreditAmount.NumberValueDecimal = ARow.TransactionAmount;
            }

            if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                UpdateTotals();
            }
            else if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FPetraUtilsObject.DisableSaveButton();
            }

            if (TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString()))
            {
                ((TFrmRecurringGLBatch)ParentForm).EnableAttributes();
            }
            else
            {
                ((TFrmRecurringGLBatch)ParentForm).DisableAttributes();
            }
        }

        private void GetDetailDataFromControlsManual(ARecurringTransactionRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            Decimal oldTransactionAmount = ARow.TransactionAmount;
            bool oldDebitCreditIndicator = ARow.DebitCreditIndicator;

            if (txtDebitAmount.Text.Length == 0)
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }

            if (txtCreditAmount.Text.Length == 0)
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            ARow.DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator)
            {
                ARow.TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);

                if (txtCreditAmount.NumberValueDecimal.Value != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
            else
            {
                ARow.TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

            if ((oldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount))
                || (oldDebitCreditIndicator != ARow.DebitCreditIndicator))
            {
                UpdateTotals();
            }
        }

        /// <summary>
        /// update amount in other currencies (optional) and recalculate all totals for current batch and journal
        /// </summary>
        public void UpdateTotals()
        {
            bool alreadyChanged;

            if ((FJournalNumber != -1))         // && !pnlDetailsProtected)
            {
                GLBatchTDSARecurringJournalRow journal = GetJournalRow();

                GLRoutines.UpdateTotalsOfRecurringJournal(ref FMainDS, journal);

                alreadyChanged = FPetraUtilsObject.HasChanges;

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.DisableDataChangedEvent();
                }

                txtCreditTotalAmount.NumberValueDecimal = journal.JournalCreditTotal;
                txtDebitTotalAmount.NumberValueDecimal = journal.JournalDebitTotal;

                if (FPreviouslySelectedDetailRow != null)
                {
                    if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                    {
                        txtDebitAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                        txtCreditAmount.NumberValueDecimal = 0;
                    }
                    else
                    {
                        txtDebitAmount.NumberValueDecimal = 0;
                        txtCreditAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                    }
                }

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.EnableDataChangedEvent();
                }

                // refresh the currency symbols
                ShowDataManual();

                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    ((TFrmRecurringGLBatch)ParentForm).GetJournalsControl().UpdateTotals(GetBatchRow());
                    ((TFrmRecurringGLBatch)ParentForm).GetBatchControl().UpdateTotals();
                }

                if (!alreadyChanged && FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            txtCreditAmount.Validated += new EventHandler(ControlHasChanged);
            txtDebitAmount.Validated += new EventHandler(ControlHasChanged);
            cmbDetailCostCentreCode.Validated += new EventHandler(ControlHasChanged);
            cmbDetailAccountCode.Validated += new EventHandler(ControlHasChanged);
            txtDetailNarrative.Validated += new EventHandler(ControlHasChanged);
            txtDetailReference.Validated += new EventHandler(ControlHasChanged);
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            //TODO: Find out why these were put here as they stop the field updates from working
            //SourceGrid.RowEventArgs egrid = new SourceGrid.RowEventArgs(-10);
            //FocusedRowChanged(sender, egrid);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && (GetBatchRow() != null)
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                                 && (GetJournalRow().JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            // pnlDetailsProtected must be changed first: when the enabled property of the control is changed, the focus changes, which triggers validation
            pnlDetailsProtected = !changeable;
            this.btnDelete.Enabled = changeable;
            this.btnNew.Enabled = changeable;
            pnlDetails.Enabled = changeable;
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(ARecurringTransactionRow ARowToDelete, ref string ADeletionQuestion)
        {
            if ((grdDetails.SelectedRowIndex() == -1) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("No Recurring GL Transaction is selected to delete."),
                    Catalog.GetString("Deleting Recurring GL Transaction"));
                return false;
            }
            else
            {
                // ask if the user really wants to cancel the transaction
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Recurring GL Transaction no: {0} ?"),
                    ARowToDelete.TransactionNumber);
                return true;
            }
        }

        private void DeleteRecord(System.Object sender, EventArgs e)
        {
            this.DeleteARecurringTransaction();
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringTransactionRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            if (ARowToDelete == null)
            {
                ACompletionMessage = string.Empty;
                return deletionSuccessful;
            }

            int currentBatchNo = ARowToDelete.BatchNumber;
            int currentJournalNo = ARowToDelete.JournalNumber;
            int recurringTransNumberToDelete = ARowToDelete.TransactionNumber;
            int lastRecurringTransNumber = GetJournalRow().LastTransactionNumber;

            try
            {
                // Delete on client side data through views that is already loaded. Data that is not
                // loaded yet will be deleted with cascading delete on server side so we don't have
                // to worry about this here.

                ACompletionMessage = String.Format(Catalog.GetString("Recurring Transaction no.: {0} deleted successfully."),
                    recurringTransNumberToDelete);

                // Delete the associated recurring transaction analysis attributes
                //Unload any open attributes in the attributes tab
                ((TFrmRecurringGLBatch) this.ParentForm).UnloadAttributes();

                //Load all attributes
                FMainDS.ARecurringTransAnalAttrib.Clear();

                for (int i = 1; i <= FMainDS.ARecurringTransaction.Count; i++)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransAnalAttrib(FLedgerNumber, currentBatchNo, currentJournalNo, i));
                }

                //If attributes do exist
                if (FMainDS.ARecurringTransAnalAttrib.Count > 0)
                {
                    ((TFrmRecurringGLBatch) this.ParentForm).DisableAttributes();

                    //Iterate through higher number attributes and transaction numbers and reduce by one
                    DataView attrView = FMainDS.ARecurringTransAnalAttrib.DefaultView;
                    attrView.RowFilter = String.Empty;

                    attrView.Sort = ARecurringTransAnalAttribTable.GetTransactionNumberDBName();
                    attrView.RowFilter = String.Format("{0} = {1} AND {2} = {3} AND {4} >= {5}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        currentBatchNo,
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        currentJournalNo,
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        recurringTransNumberToDelete);

                    ARecurringTransAnalAttribRow attrRowCurrent = null;

                    foreach (DataRowView gv in attrView)
                    {
                        attrRowCurrent = (ARecurringTransAnalAttribRow)gv.Row;

                        if ((attrRowCurrent.TransactionNumber == recurringTransNumberToDelete))
                        {
                            attrRowCurrent.Delete();
                        }
                        else
                        {
                            attrRowCurrent.TransactionNumber--;
                        }
                    }
                }

                //Bubble the transaction to delete to the top
                DataView transView = FMainDS.ARecurringTransaction.DefaultView;
                transView.RowFilter = String.Empty;

                transView.Sort = ARecurringTransactionTable.GetTransactionNumberDBName();
                transView.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    currentBatchNo,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    currentJournalNo);

                ARecurringTransactionRow transRowToReceive = null;
                ARecurringTransactionRow transRowToCopyDown = null;
                ARecurringTransactionRow transRowCurrent = null;

                int currentTransNo = 0;

                foreach (DataRowView gv in transView)
                {
                    transRowCurrent = (ARecurringTransactionRow)gv.Row;

                    currentTransNo = transRowCurrent.TransactionNumber;

                    if (currentTransNo > recurringTransNumberToDelete)
                    {
                        transRowToCopyDown = transRowCurrent;

                        //Copy column values down
                        for (int j = 4; j < transRowToCopyDown.Table.Columns.Count; j++)
                        {
                            //Update all columns except the pk fields that remain the same
                            transRowToReceive[j] = transRowToCopyDown[j];
                        }
                    }

                    if (currentTransNo == transView.Count)                         //Last row which is the row to be deleted
                    {
                        //Mark last record for deletion
                        transRowCurrent.SubType = MFinanceConstants.MARKED_FOR_DELETION;
                    }

                    //transRowToReceive will become previous row for next recursion
                    transRowToReceive = transRowCurrent;
                }

                FPreviouslySelectedDetailRow = null;
                //grdDetails.DataSource = null;

                FPetraUtilsObject.SetChangedFlag();

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

            //Bubble the deleted transaction to the top
            //FPreviouslySelectedDetailRow = MFinanceConstants.MARKED_FOR_DELETION;
            if (!((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Unable to save after deleting a recurring transaction!");
                deletionSuccessful = false;
            }
            else
            {
                //Reload from server
                grdDetails.DataSource = null;
                FMainDS.ARecurringTransAnalAttrib.Clear();
                FMainDS.ARecurringTransaction.Clear();

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionWithAttributes(FLedgerNumber, currentBatchNo,
                        currentJournalNo));
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransaction.DefaultView);

                ResetJournalLastTransNumber();

                MessageBox.Show("Deletion successful!");
            }

            UpdateTotals();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                pnlDetails.Enabled = false;
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
        private void PostDeleteManual(ARecurringTransactionRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
                {
                    ClearControls();
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!ADeletionPerformed)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (grdDetails.Rows.Count <= 1)
            {
                ((TFrmRecurringGLBatch)ParentForm).GetAttributesControl().ClearCurrentSelection();
                ((TFrmRecurringGLBatch)ParentForm).DisableAttributes();
            }
        }

        private void ResetJournalLastTransNumber()
        {
            string transRowFilter = String.Format("{0}={1} and {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            //Highest trans number first
            string transSortOrder = ARecurringTransactionTable.GetTransactionNumberDBName() + " DESC";

            FMainDS.ARecurringTransaction.DefaultView.RowFilter = transRowFilter;
            FMainDS.ARecurringTransaction.DefaultView.Sort = transSortOrder;

            if (FMainDS.ARecurringTransaction.DefaultView.Count > 0)
            {
                ARecurringTransactionRow transRow = (ARecurringTransactionRow)FMainDS.ARecurringTransaction.DefaultView[0].Row;
                GetJournalRow().LastTransactionNumber = transRow.TransactionNumber;
            }
            else
            {
                GetJournalRow().LastTransactionNumber = 0;
            }

            transSortOrder = ARecurringTransactionTable.GetTransactionNumberDBName() + " ASC";
            FMainDS.ARecurringTransaction.DefaultView.Sort = transSortOrder;
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        private void ClearControls()
        {
            //Stop data change detection
            FPetraUtilsObject.DisableDataChangedEvent();

            //Clear combos
            cmbDetailAccountCode.SelectedIndex = -1;
            cmbDetailCostCentreCode.SelectedIndex = -1;
            cmbMinistry.SelectedIndex = -1;
            //Clear Textboxes
            txtDetailNarrative.Clear();
            txtDetailReference.Clear();
            //Clear Numeric Textboxes
            txtDebitAmount.NumberValueDecimal = 0;
            txtCreditAmount.NumberValueDecimal = 0;

            //Enable data change detection
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            if (TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString()))
            {
                ((TFrmRecurringGLBatch)ParentForm).EnableAttributes();
            }
            else
            {
                ((TFrmRecurringGLBatch)ParentForm).DisableAttributes();
            }
        }

        private void ValidateDataDetailsManual(ARecurringTransactionRow ARow)
        {
            if ((ARow == null) || (GetBatchRow() == null) || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            
            // if "Reference" is mandatory then make sure it is set
            if (TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GLREFMANDATORY, "no") == "yes")
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnReferenceId];
                ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                    ARow.TransactionNumber,
                    ARow.BatchNumber,
                    ARow.JournalNumber);
        
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Reference,
                    "Reference of " + ValidationContext,
                    this, ValidationColumn, null);
        
                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
            
            //Local validation
            if ((txtDebitAmount.NumberValueDecimal == 0) && (txtCreditAmount.NumberValueDecimal == 0))
            {
                TSharedFinanceValidation_GL.ValidateRecurringGLDetailManual(this, FBatchRow, ARow, txtDebitAmount, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else
            {
                TSharedFinanceValidation_GL.ValidateRecurringGLDetailManual(this, FBatchRow, ARow, null, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
        }

        /// <summary>
        /// Set focus to the gid controltab
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