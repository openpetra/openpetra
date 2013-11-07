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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Gui.Setup;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLJournals
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumberToDelete = -1;
        private string FBatchStatus = string.Empty;

        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS RecurringJournalFMainDS()
        {
            return FMainDS;
        }

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            bool batchChanged = (FBatchNumber != ABatchNumber);

            //Check if same Journals as previously selected
            if ((FLedgerNumber == ALedgerNumber) && !batchChanged && (FBatchStatus == ABatchStatus)
                && (FMainDS.ARecurringJournal.DefaultView.Count > 0))
            {
                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    if ((GetSelectedRowIndex() > 0)
                        && (GetSelectedDetailRow().RowState != DataRowState.Deleted))
                    {
                        GetDetailsFromControls(GetSelectedDetailRow());
                    }
                }

                return;
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchStatus = ABatchStatus;

            FPreviouslySelectedDetailRow = null;

            if (batchChanged)
            {
                //Clear all previous data.
                FMainDS.ARecurringTransAnalAttrib.Clear();
                FMainDS.ARecurringTransaction.Clear();
                FMainDS.ARecurringJournal.Clear();
            }

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringJournal.DefaultView);

            SetJournalDefaultView();

            // only load from server if there are no journals loaded yet for this batch
            // otherwise we would overwrite journals that have already been modified
            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndContent(ALedgerNumber, ABatchNumber));
            }

            ShowData();

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
            }
            else
            {
                SelectRowInGrid(1);
            }

            UpdateRecordNumberDisplay();
            SetRecordNumberDisplayProperties();

            txtBatchNumber.Text = FBatchNumber.ToString();
        }

        private void SetJournalDefaultView()
        {
            string rowFilter = string.Format("{0} = {1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.ARecurringJournal.DefaultView.RowFilter = rowFilter;
            FFilterPanelControls.SetBaseFilter(rowFilter, true);
            FCurrentActiveFilter = rowFilter;

            FMainDS.ARecurringJournal.DefaultView.Sort = String.Format("{0} DESC",
                ARecurringJournalTable.GetJournalNumberDBName()
                );
        }

        /// <summary>
        /// Update the effective date from outside
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        public void UpdateEffectiveDateForCurrentRow(DateTime AEffectiveDate)
        {
            if ((GetSelectedDetailRow() != null) && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                GetSelectedDetailRow().DateEffective = AEffectiveDate;
                GetDetailsFromControls(GetSelectedDetailRow());
            }
        }

        /// <summary>
        /// Return the active journal number
        /// </summary>
        /// <returns></returns>
        public Int32 ActiveJournalNumber(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            Int32 activeJournal = 0;

            if ((FPreviouslySelectedDetailRow != null) && (FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber))
            {
                activeJournal = FPreviouslySelectedDetailRow.JournalNumber;
            }

            return activeJournal;
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FMainDS.ARecurringJournal.RejectChanges();
            }
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

            foreach (DataRowView v in FMainDS.ARecurringJournal.DefaultView)
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
            if (ARow == null)
            {
                ((TFrmRecurringGLBatch)ParentForm).DisableTransactions();
            }
            else
            {
                ((TFrmRecurringGLBatch)ParentForm).EnableTransactions();

                //Can't cancel an already cancelled row
                btnDelete.Enabled = (ARow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);
                ((TFrmRecurringGLBatch)ParentForm).EnableTransactions();

                if (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }

                UpdateChangeableStatus();
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
            if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewARecurringJournal();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableTransactions();

                txtDetailJournalDescription.Text = Catalog.GetString("Please enter a journal description");
                txtDetailJournalDescription.SelectAll();
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSARecurringJournalRow ANewRow)
        {
            DataView view = new DataView(FMainDS.ARecurringBatch);

            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(ARecurringBatchTable.TableId), ',');
            ARecurringBatchRow row = (ARecurringBatchRow)view.FindRows(new object[] { FLedgerNumber, FBatchNumber })[0].Row;
            ANewRow.LedgerNumber = row.LedgerNumber;
            ANewRow.BatchNumber = row.BatchNumber;
            ANewRow.JournalNumber = row.LastJournal + 1;

            // manually created journals are all GL
            ANewRow.SubSystemCode = "GL";
            ANewRow.TransactionTypeCode = "STD";

            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
            ANewRow.TransactionCurrency = ledger.BaseCurrency;

            ANewRow.ExchangeRateToBase = 1;
            ANewRow.DateEffective = row.DateEffective;
            ANewRow.JournalPeriod = row.BatchPeriod;
            row.LastJournal++;
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
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TFrmRecurringGLBatch.eGLTabs.RecurringTransactions);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && GetBatchRow() != null
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            Boolean journalUpdatable =
                (FPreviouslySelectedDetailRow != null && FPreviouslySelectedDetailRow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            this.btnDelete.Enabled = changeable && journalUpdatable;
            this.btnAdd.Enabled = changeable;
            pnlDetails.Enabled = changeable && journalUpdatable;
            pnlDetailsProtected = !changeable;
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
            SetJournalDefaultView();

            if (FMainDS.ARecurringJournal.DefaultView.Count > 0)
            {
                ARecurringJournalRow jrnlRow = (ARecurringJournalRow)FMainDS.ARecurringJournal.DefaultView[0].Row;
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

        /// <summary>
        /// Shows the Filter/Find UserControl and switches to the Find Tab.
        /// </summary>
        public void ShowFindPanel()
        {
            if (FucoFilterAndFind == null)
            {
                ToggleFilter();
            }

            FucoFilterAndFind.DisplayFindTab();
        }
    }
}