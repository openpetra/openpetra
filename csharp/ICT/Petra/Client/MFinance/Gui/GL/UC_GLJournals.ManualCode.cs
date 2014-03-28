//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
    public partial class TUC_GLJournals
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private string FBatchStatus = string.Empty;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;


        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS JournalFMainDS()
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
            bool ledgerChanged = (FLedgerNumber != ALedgerNumber);

            //Make sure the current effective date for the Batch is correct
            DateTime batchDateEffective = GetBatchRow().DateEffective;

            if (ABatchStatus == MFinanceConstants.BATCH_UNPOSTED)
            {
                if ((!dtpDetailDateEffective.Date.HasValue) || (dtpDetailDateEffective.Date.Value != batchDateEffective))
                {
                    dtpDetailDateEffective.Date = batchDateEffective;
                }
            }

            // Get a view on the journals for the specified batch
            DataView dv = new DataView(FMainDS.AJournal, String.Format("{0}={1}",
                    AJournalTable.GetBatchNumberDBName(), ABatchNumber), "", DataViewRowState.CurrentRows);

            //Check if same Journals as previously selected
            if ((FLedgerNumber == ALedgerNumber) && !batchChanged && (FBatchStatus == ABatchStatus) && (dv.Count > 0))
            {
                // The journals are the same and we have loaded them already
                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    if (GetSelectedRowIndex() > 0)
                    {
                        GetDetailsFromControls(GetSelectedDetailRow());
                    }
                }
            }
            else
            {
                // Need to load a new journal
                FLedgerNumber = ALedgerNumber;
                FBatchNumber = ABatchNumber;
                FBatchStatus = ABatchStatus;

                FPreviouslySelectedDetailRow = null;

                if (ledgerChanged)
                {
                    //Clear all previous data.
                    FMainDS.ATransAnalAttrib.Clear();
                    FMainDS.ATransaction.Clear();
                    FMainDS.AJournal.Clear();
                }

                // This sets the base rowFilter and sort and calls manual code
                ShowData();

                // only load from server if there are no journals loaded yet for this batch
                // otherwise we would overwrite journals that have already been modified
                dv = FMainDS.AJournal.DefaultView;

                if (dv.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournalAndContent(ALedgerNumber, ABatchNumber));
                }

                // Now set up the complete current filter
                FFilterPanelControls.SetBaseFilter(dv.RowFilter, true);
                ApplyFilter();
            }

            //This will also call UpdateChangeableStatus
            SelectRowInGrid((batchChanged || ledgerChanged) ? 1 : FPrevRowChangedRow);

            UpdateRecordNumberDisplay();
            SetRecordNumberDisplayProperties();
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
                dtpDetailDateEffective.Date = AEffectiveDate;
                GetDetailsFromControls(GetSelectedDetailRow());
            }
        }

        /// <summary>
        /// Return the active journal number
        /// </summary>
        /// <returns></returns>
        public void CurrentActiveJournalKeyFields(Int32 ALedgerNumber, ref Int32 ABatchNumber, ref Int32 AJournalNumber)
        {
            if (FPreviouslySelectedDetailRow != null)
            {
                ABatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
                AJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
            }
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FMainDS.AJournal.RejectChanges();
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
        }

        /// <summary>
        /// update the journal header fields from a batch
        /// </summary>
        /// <param name="ABatch"></param>
        public void UpdateHeaderTotals(ABatchRow ABatch)
        {
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            foreach (DataRowView v in FMainDS.AJournal.DefaultView)
            {
                AJournalRow r = (AJournalRow)v.Row;

                sumCredits += r.JournalCreditTotal;
                sumDebits += r.JournalDebitTotal;
            }

            FPetraUtilsObject.DisableDataChangedEvent();
            txtCurrentPeriod.Text = ABatch.BatchPeriod.ToString();
            txtDebit.NumberValueDecimal = sumDebits;
            txtCredit.NumberValueDecimal = sumCredits;
            txtControl.NumberValueDecimal = ABatch.BatchControlTotal;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private void ShowDetailsManual(AJournalRow ARow)
        {
            bool RowIsNull = (ARow == null);

            grdDetails.TabStop = (!RowIsNull);

            if (RowIsNull)
            {
                btnAdd.Focus();
            }

            if (RowIsNull || (ARow.JournalStatus == MFinanceConstants.BATCH_CANCELLED))
            {
                ((TFrmGLBatch)ParentForm).DisableTransactions();
            }
            else
            {
                ((TFrmGLBatch)ParentForm).EnableTransactions();
            }

            UpdateChangeableStatus();
        }

        private ABatchRow GetBatchRow()
        {
            return ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
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

            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewAJournal();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGLBatch) this.ParentForm).EnableTransactions();

                txtDetailJournalDescription.Text = Catalog.GetString("Please enter a journal description");
                txtDetailJournalDescription.SelectAll();
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSAJournalRow ANewRow)
        {
            DataView view = new DataView(FMainDS.ABatch);

            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(ABatchTable.TableId), ',');
            ABatchRow row = (ABatchRow)view.FindRows(new object[] { FLedgerNumber, FBatchNumber })[0].Row;
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
        private void BeforeShowDetailsManual(AJournalRow ARow)
        {
            // SubSystemCode: the user can only select GL, but the system can generate eg. AP journals or GR journals
            this.cmbDetailSubSystemCode.Items.Clear();
            this.cmbDetailSubSystemCode.Items.AddRange(new object[] { ARow.SubSystemCode });

            TFinanceControls.InitialiseTransactionTypeList(ref cmbDetailTransactionTypeCode, FLedgerNumber, ARow.SubSystemCode);
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Transactions);
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

            this.btnCancel.Enabled = changeable && journalUpdatable;
            this.btnAdd.Enabled = changeable;
            this.btnGetSetExchangeRate.Enabled = changeable && journalUpdatable
                                                 && (FPreviouslySelectedDetailRow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);
            pnlDetails.Enabled = changeable && journalUpdatable;
            FPnlDetailsProtected = !changeable;

            if (!changeable)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        /// <summary>
        /// remove journals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelRow(System.Object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow == null) || !((TFrmGLBatch)ParentForm).SaveChanges())
            {
                return;
            }

            int CurrentJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to cancel this journal ({0}).\n\nDo you really want to cancel it?"),
                         CurrentJournalNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    //clear any transactions currently being editied in the Transaction Tab
                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                    ////Clear transactions etc for current Journal
                    //FMainDS.ATransAnalAttrib.Clear();
                    //FMainDS.ATransaction.Clear();

                    //Load any new data
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber, FBatchNumber,
                            CurrentJournalNumber));

                    DataView dvAA = new DataView(FMainDS.ATransAnalAttrib);

                    dvAA.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ATransAnalAttribTable.GetJournalNumberDBName(),
                        CurrentJournalNumber);

                    //Delete Analysis Attribs
                    foreach (DataRowView dvr in dvAA)
                    {
                        dvr.Delete();
                    }

                    DataView dvTr = new DataView(FMainDS.ATransaction);

                    dvTr.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        CurrentJournalNumber);

                    //Delete Transactions
                    foreach (DataRowView dvr in dvTr)
                    {
                        dvr.Delete();
                    }

                    FPreviouslySelectedDetailRow.BeginEdit();
                    FPreviouslySelectedDetailRow.JournalStatus = MFinanceConstants.BATCH_CANCELLED;

                    //Ensure validation passes
                    if (FPreviouslySelectedDetailRow.JournalDescription.Length == 0)
                    {
                        txtDetailJournalDescription.Text = " ";
                    }

                    if (FPreviouslySelectedDetailRow.ExchangeRateToBase == 0)
                    {
                        txtDetailExchangeRateToBase.NumberValueDecimal = 1;
                    }

                    ABatchRow batchrow = ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();

                    batchrow.BatchCreditTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                    batchrow.BatchDebitTotal -= FPreviouslySelectedDetailRow.JournalDebitTotal;

                    if (batchrow.BatchControlTotal != 0)
                    {
                        batchrow.BatchControlTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                    }

                    FPreviouslySelectedDetailRow.JournalCreditTotal = 0;
                    FPreviouslySelectedDetailRow.JournalDebitTotal = 0;
                    FPreviouslySelectedDetailRow.EndEdit();

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (((TFrmGLBatch)ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The journal has been cancelled successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ((TFrmGLBatch)ParentForm).DisableTransactions();
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The journal has been cancelled but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save the cancellation immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    UpdateChangeableStatus();
                    grdDetails.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        private void ValidateDataDetailsManual(AJournalRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GL.ValidateGLJournalManual(this, ARow, ref VerificationResultCollection,
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

        private void RefreshCurrencyAndExchangeRate()
        {
            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;
            txtDetailExchangeRateToBase.BackColor =
                (FPreviouslySelectedDetailRow.ExchangeRateToBase == DEFAULT_CURRENCY_EXCHANGE) ? Color.LightPink : Color.Empty;

            // recalculate the base currency amounts for the transactions
            ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionTotals();

            btnGetSetExchangeRate.Enabled = (FPreviouslySelectedDetailRow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);
        }

        private void ResetCurrencyExchangeRate(object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null)
                && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FPreviouslySelectedDetailRow.TransactionCurrency = cmbDetailTransactionCurrency.GetSelectedString();

                ABatchRow batchrow = ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();

                FPreviouslySelectedDetailRow.ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    FMainDS.ALedger[0].BaseCurrency,
                    FPreviouslySelectedDetailRow.TransactionCurrency,
                    batchrow.DateEffective);

                RefreshCurrencyAndExchangeRate();
            }
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            btnGetSetExchangeRate.Click += new EventHandler(SetExchangeRateValue);
            cmbDetailTransactionCurrency.SelectedValueChanged +=
                new System.EventHandler(ResetCurrencyExchangeRate);

            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
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
                    dtpDetailDateEffective.Date.HasValue ? dtpDetailDateEffective.Date.Value : DateTime.Today,
                    cmbDetailTransactionCurrency.GetSelectedString(),
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

        private decimal GetActualExchangeRateForeign()
        {
            return txtDetailExchangeRateToBase.NumberValueDecimal.Value;
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