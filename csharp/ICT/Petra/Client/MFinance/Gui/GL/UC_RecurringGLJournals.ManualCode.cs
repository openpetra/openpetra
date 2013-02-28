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
        private string FBatchStatus = string.Empty;

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            //Check if same Journals as previously selected
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FBatchStatus == ABatchStatus)
                && (FMainDS.ARecurringJournal.DefaultView.Count > 0))
            {
                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    if (grdDetails.SelectedRowIndex() > 0)
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

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringJournal.DefaultView);

            FMainDS.ARecurringJournal.DefaultView.RowFilter = string.Format("{0} = {1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.ARecurringJournal.DefaultView.Sort = String.Format("{0} ASC",
                ARecurringJournalTable.GetJournalNumberDBName()
                );

            // only load from server if there are no journals loaded yet for this batch
            // otherwise we would overwrite journals that have already been modified
            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(ALedgerNumber, ABatchNumber));
            }

            ShowData();
            ShowDetails();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
            }

            txtBatchNumber.Text = FBatchNumber.ToString();

            //This will update Batch totals
            UpdateTotals(GetBatchRow());

            grdDetails.Focus();
        }

        /// <summary>
        /// Load the journals for the current batch in the background
        /// </summary>
        public void UnloadJournals()
        {
            if (FMainDS.ARecurringJournal.DefaultView.Count > 0)
            {
                FPreviouslySelectedDetailRow = null;
                FMainDS.ARecurringJournal.Clear();
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
                GetSelectedDetailRow().DateEffective = AEffectiveDate;
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
        public void UpdateTotals(ARecurringBatchRow ABatch)
        {
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            foreach (DataRowView v in FMainDS.ARecurringJournal.DefaultView)
            {
                ARecurringJournalRow r = (ARecurringJournalRow)v.Row;

                sumCredits += r.JournalCreditTotal;
                sumDebits += r.JournalDebitTotal;
            }

            if (ABatch.BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
            {
                if (ABatch.BatchCreditTotal != sumCredits)
                {
                    ABatch.BatchCreditTotal = sumCredits;
                }

                if (ABatch.BatchDebitTotal != sumDebits)
                {
                    ABatch.BatchDebitTotal = sumDebits;
                }

                if (ABatch.BatchRunningTotal != Math.Round(sumDebits - sumCredits, 2))
                {
                    ABatch.BatchRunningTotal = Math.Round(sumDebits - sumCredits, 2);
                }
            }

            FPetraUtilsObject.DisableDataChangedEvent();
            txtCurrentPeriod.Text = ABatch.BatchPeriod.ToString();
            txtDebit.NumberValueDecimal = sumDebits;
            txtCredit.NumberValueDecimal = sumCredits;
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
            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            this.CreateNewARecurringJournal();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableTransactions();

                txtDetailJournalDescription.Text = "Please enter a journal description";
                txtDetailJournalDescription.SelectAll();
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref RecurringGLBatchTDSARecurringJournalRow ANewRow)
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

        /// <summary>
        /// remove journals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteRecord(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            int currentRowIndex = grdDetails.SelectedRowIndex();

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete this journal ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.JournalNumber),
                        Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                FPreviouslySelectedDetailRow.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                ARecurringBatchRow batchrow = ((TFrmRecurringGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
                batchrow.BatchCreditTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                batchrow.BatchDebitTotal -= FPreviouslySelectedDetailRow.JournalDebitTotal;

                if (batchrow.BatchControlTotal != 0)
                {
                    batchrow.BatchControlTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                }

                FPreviouslySelectedDetailRow.JournalCreditTotal = 0;
                FPreviouslySelectedDetailRow.JournalDebitTotal = 0;

                foreach (ATransactionRow transaction in FMainDS.ARecurringTransaction.Rows)     //alle? ist das richtig?
                {
                    transaction.Delete();
                }

                SelectRowInGrid(currentRowIndex);

                ((TFrmRecurringGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                FPetraUtilsObject.SetChangedFlag();
                UpdateChangeableStatus();

                if (grdDetails.Rows.Count < 2)
                {
                    ClearControls();
                }
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
    }
}