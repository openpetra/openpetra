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
        private string FBatchStatus = string.Empty;
        private string FTransactionCurrency = string.Empty;
        private decimal FIntlRateToBaseCurrency = 0;
        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// <summary>
        /// The current active Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;
        
        /// <summary>
        /// flags if the Journal(s) have finished loading
        /// </summary>
        public bool FJournalsLoaded = false;

        private ABatchRow FBatchRow = null;

        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS JournalFMainDS()
        {
            return FMainDS;
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            btnGetSetExchangeRate.Click += new EventHandler(SetExchangeRateValue);
            cmbDetailTransactionCurrency.SelectedValueChanged += new System.EventHandler(ResetCurrencyExchangeRate);
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowTransactionTab);
        }

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            bool FirstRun = (FLedgerNumber != ALedgerNumber);
            bool BatchChanged = (FBatchNumber != ABatchNumber);

            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                return;
            }

            //Make sure the current effective date for the Batch is correct
            DateTime BatchDateEffective = FBatchRow.DateEffective;
            FIntlRateToBaseCurrency = ((TFrmGLBatch)ParentForm).GetInternationalCurrencyExchangeRate(BatchDateEffective);

            if (ABatchStatus == MFinanceConstants.BATCH_UNPOSTED)
            {
                if ((!dtpDetailDateEffective.Date.HasValue) || (dtpDetailDateEffective.Date.Value != BatchDateEffective))
                {
                    dtpDetailDateEffective.Date = BatchDateEffective;
                    //TODO
                    //Recalculate internation currency amounts for all journals
                }
            }

            // Get a view on the journals for the specified batch
            DataView JournalDV = new DataView(FMainDS.AJournal, String.Format("{0}={1}",
                    AJournalTable.GetBatchNumberDBName(), ABatchNumber), "", DataViewRowState.CurrentRows);

            //Check if same Journals as previously selected
            if (!FirstRun && !BatchChanged && (FBatchStatus == ABatchStatus) && (JournalDV.Count > 0))
            {
                // The journals are the same and we have loaded them already
                if (FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
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

                // This sets the base rowFilter and sort and calls manual code
                ShowData();

                // only load from server if there are no journals loaded yet for this batch
                // otherwise we would overwrite journals that have already been modified
                JournalDV = FMainDS.AJournal.DefaultView;
                JournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    AJournalTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AJournalTable.GetBatchNumberDBName(),
                    ABatchNumber);

                if (JournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournalAndContent(ALedgerNumber, ABatchNumber));
                }

                // Now set up the complete current filter
                FFilterPanelControls.SetBaseFilter(JournalDV.RowFilter, true);
                ApplyFilter();

                FJournalsLoaded = true;
            }

            //This will also call UpdateChangeableStatus
            SelectRowInGrid((BatchChanged || FirstRun) ? 1 : FPrevRowChangedRow);

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
            decimal SumDebits = 0.0M;
            decimal SumCredits = 0.0M;

            DataView JournalDV = new DataView(FMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                    AJournalTable.GetBatchNumberDBName(),
                    ABatch.BatchNumber);

            foreach (DataRowView v in JournalDV)
            {
                AJournalRow r = (AJournalRow)v.Row;

                SumCredits += r.JournalCreditTotal;
                SumDebits += r.JournalDebitTotal;
            }

            FPetraUtilsObject.DisableDataChangedEvent();
            txtCurrentPeriod.Text = ABatch.BatchPeriod.ToString();
            txtDebit.NumberValueDecimal = SumDebits;
            txtCredit.NumberValueDecimal = SumCredits;
            txtControl.NumberValueDecimal = ABatch.BatchControlTotal;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private void ShowDetailsManual(AJournalRow ARow)
        {
            bool JournalRowIsNull = (ARow == null);

            grdDetails.TabStop = (!JournalRowIsNull);

            if (JournalRowIsNull)
            {
                FTransactionCurrency = string.Empty;
                btnAdd.Focus();
            }
            else
            {
                FTransactionCurrency = ARow.TransactionCurrency;
            }

            //Enable the transactions tab accordingly
            ((TFrmGLBatch)ParentForm).EnableTransactions(!JournalRowIsNull && (ARow.JournalStatus != MFinanceConstants.BATCH_CANCELLED)); 
            
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

                txtDetailJournalDescription.Text = Catalog.GetString("PLEASE ENTER A JOURNAL DESCRIPTION");
                txtDetailJournalDescription.SelectAll();
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSAJournalRow ANewRow)
        {
            if (ANewRow == null || FLedgerNumber == -1)
            {
                return;
            }

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            DataView BatchDV = new DataView(FMainDS.ABatch);

            BatchDV.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(ABatchTable.TableId), ',');
            
            ABatchRow BatchRow = (ABatchRow)BatchDV.FindRows(new object[] { FLedgerNumber, FBatchNumber })[0].Row;
            
            ANewRow.LedgerNumber = BatchRow.LedgerNumber;
            ANewRow.BatchNumber = BatchRow.BatchNumber;
            ANewRow.JournalNumber = BatchRow.LastJournal + 1;

            // manually created journals are all GL
            ANewRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
            ANewRow.TransactionTypeCode = MFinanceConstants.STANDARD_JOURNAL;

            ANewRow.TransactionCurrency = LedgerRow.BaseCurrency;
            ANewRow.ExchangeRateToBase = 1;
            ANewRow.DateEffective = BatchRow.DateEffective;
            ANewRow.JournalPeriod = BatchRow.BatchPeriod;
            BatchRow.LastJournal++;
        }

        /// Initialise some comboboxes
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
            Boolean IsChangeable = (!FPetraUtilsObject.DetailProtectedMode)
                                    && (GetBatchRow() != null)
                                    && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            Boolean JournalUpdatable = (FPreviouslySelectedDetailRow != null
                                        && FPreviouslySelectedDetailRow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            //Process buttons
            this.btnCancel.Enabled = IsChangeable && JournalUpdatable;
            this.btnAdd.Enabled = IsChangeable;
            this.btnGetSetExchangeRate.Enabled = IsChangeable && JournalUpdatable
                                                 && (FPreviouslySelectedDetailRow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);

            pnlDetails.Enabled = IsChangeable && JournalUpdatable;
            pnlDetailsProtected = !IsChangeable;

            if (!IsChangeable)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        private void CancelRow(System.Object sender, EventArgs e)
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

                    FBatchRow.BatchCreditTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                    FBatchRow.BatchDebitTotal -= FPreviouslySelectedDetailRow.JournalDebitTotal;

                    if (FBatchRow.BatchControlTotal != 0)
                    {
                        FBatchRow.BatchControlTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
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
                    SetFocusToDetailsGrid();
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
            //Called from Batch tab, so no need to check for GetDetailsFromControls()
            // as tab change does that and current tab is Batch tab.
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
        public void SetFocusToDetailsGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }

        private void ResetCurrencyExchangeRate(object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null)
                && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FTransactionCurrency = cmbDetailTransactionCurrency.GetSelectedString();

                FPreviouslySelectedDetailRow.TransactionCurrency = FTransactionCurrency;

                FPreviouslySelectedDetailRow.ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    FTransactionCurrency,
                    FMainDS.ALedger[0].BaseCurrency,
                    FBatchRow.DateEffective);

                RefreshCurrencyAndExchangeRate();
            }
        }

        private void RefreshCurrencyAndExchangeRate()
        {
            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;

            txtDetailExchangeRateToBase.BackColor = (FPreviouslySelectedDetailRow.ExchangeRateToBase == DEFAULT_CURRENCY_EXCHANGE) ? Color.LightPink : Color.Empty;

            FIntlRateToBaseCurrency = ((TFrmGLBatch)ParentForm).GetInternationalCurrencyExchangeRate(FBatchRow.DateEffective);

            ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionAmounts("JOURNAL");

            btnGetSetExchangeRate.Enabled = (FPreviouslySelectedDetailRow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);
        }

        private void SetExchangeRateValue(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate SetupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            decimal SelectedExchangeRate;
            DateTime SelectedEffectiveDate;
            int SelectedEffectiveTime;

            if (SetupDailyExchangeRate.ShowDialog(
                    FLedgerNumber,
                    dtpDetailDateEffective.Date.HasValue ? dtpDetailDateEffective.Date.Value : DateTime.Today,
                    cmbDetailTransactionCurrency.GetSelectedString(),
                    DEFAULT_CURRENCY_EXCHANGE,
                    out SelectedExchangeRate,
                    out SelectedEffectiveDate,
                    out SelectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            if (FPreviouslySelectedDetailRow.ExchangeRateToBase != SelectedExchangeRate)
            {
                //Enforce save needed condition
                FPetraUtilsObject.SetChangedFlag();
            }

            FPreviouslySelectedDetailRow.ExchangeRateToBase = SelectedExchangeRate;

            RefreshCurrencyAndExchangeRate();
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