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
    public partial class TUC_GLJournals
    {
        private Int32 FLedgerNumber = -1;
        private string FBatchStatus = string.Empty;
        private ABatchRow FBatchRow = null;

        // Logic Objects
        private TUC_GLJournals_Cancel FCancelLogicObject = null;

        private string FTransactionCurrency = string.Empty;
        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

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

        private void RunOnceOnParentActivationManual()
        {
            //Nothing to do here
        }

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            FJournalsLoaded = false;
            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                return;
            }

            FCancelLogicObject = new TUC_GLJournals_Cancel(FPetraUtilsObject, FLedgerNumber, FMainDS);

            //Make sure the current effective date for the Batch is correct
            DateTime BatchDateEffective = FBatchRow.DateEffective;

            bool FirstRun = (FLedgerNumber != ALedgerNumber);
            bool BatchChanged = (FBatchNumber != ABatchNumber);
            bool BatchStatusChanged = (!BatchChanged && FBatchStatus != ABatchStatus);

            //Check if need to load Journals
            if (FirstRun || BatchChanged || BatchStatusChanged)
            {
                FLedgerNumber = ALedgerNumber;
                FBatchNumber = ABatchNumber;
                FBatchStatus = ABatchStatus;

                SetJournalDefaultView();
                FPreviouslySelectedDetailRow = null;

                //Load Journals
                if (FMainDS.AJournal.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournalAndContent(FLedgerNumber, FBatchNumber));
                }

                if (FBatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    if (!dtpDetailDateEffective.Date.HasValue || (dtpDetailDateEffective.Date.Value != BatchDateEffective))
                    {
                        dtpDetailDateEffective.Date = BatchDateEffective;
                    }
                }

                foreach (DataRowView drv in FMainDS.AJournal.DefaultView)
                {
                    AJournalRow jr = (AJournalRow)drv.Row;

                    if (jr.DateEffective != BatchDateEffective)
                    {
                        ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionTotals("BATCH", true);
                        break;
                    }
                }

                ShowData();

                // Now set up the complete current filter
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FMainDS.AJournal.DefaultView.RowFilter, true);
                FFilterAndFindObject.ApplyFilter();
            }
            else
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

            //This will also call UpdateChangeableStatus
            SelectRowInGrid((BatchChanged || FirstRun) ? 1 : FPrevRowChangedRow);

            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            FJournalsLoaded = true;
        }

        private void SetJournalDefaultView()
        {
            string DVRowFilter = string.Format("{0} = {1}",
                AJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.AJournal.DefaultView.RowFilter = DVRowFilter;
            FMainDS.AJournal.DefaultView.Sort = String.Format("{0} DESC",
                AJournalTable.GetJournalNumberDBName()
                );

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(DVRowFilter, true);
            FFilterAndFindObject.CurrentActiveFilter = DVRowFilter;
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
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSAJournalRow ANewRow)
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
            int CurrentJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;

            if (FCancelLogicObject.CancelRow(FPreviouslySelectedDetailRow, txtDetailJournalDescription, txtDetailExchangeRateToBase))
            {
                UpdateChangeableStatus();
                SetFocusToDetailsGrid();
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

            //TODO: remove this once database definition is set for Batch Description to be NOT NULL
            // Description is mandatory then make sure it is set
            if (txtDetailJournalDescription.Text.Length == 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnJournalDescriptionId];
                ValidationContext = String.Format("Batch no.: {0}, Journal no.: {1}",
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

        private void ResetCurrencyExchangeRate(object sender, EventArgs e)
        {
            bool CurrencyCodeHasChanged = false;

            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null)
                || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            CurrencyCodeHasChanged = (FTransactionCurrency != cmbDetailTransactionCurrency.GetSelectedString());

            if (CurrencyCodeHasChanged)
            {
                FTransactionCurrency = cmbDetailTransactionCurrency.GetSelectedString();

                FPreviouslySelectedDetailRow.TransactionCurrency = FTransactionCurrency;

                FPreviouslySelectedDetailRow.ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    FTransactionCurrency,
                    FMainDS.ALedger[0].BaseCurrency,
                    FBatchRow.DateEffective);

                if (FPreviouslySelectedDetailRow.ExchangeRateToBase > 0)
                {
                    RefreshCurrencyAndExchangeRate();
                }
                else
                {
                    txtDetailExchangeRateToBase.NumberValueDecimal = 0M;
                    btnGetSetExchangeRate.Enabled = true;
                }
            }
        }

        private void RefreshCurrencyAndExchangeRate()
        {
            txtDetailExchangeRateToBase.NumberValueDecimal = FPreviouslySelectedDetailRow.ExchangeRateToBase;

            txtDetailExchangeRateToBase.BackColor =
                (FPreviouslySelectedDetailRow.ExchangeRateToBase == DEFAULT_CURRENCY_EXCHANGE) ? Color.LightPink : Color.Empty;

            ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionTotals("JOURNAL");

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

        private void TransactionTypeCodeChanged(Object sender, EventArgs e)
        {
            if (cmbDetailTransactionTypeCode.GetSelectedString() == CommonAccountingTransactionTypesEnum.ALLOC.ToString())
            {
                btnAddAllocations.Visible = true;
                btnAddAllocations.Text = Catalog.GetString("Add Allocation");
            }
            else if (cmbDetailTransactionTypeCode.GetSelectedString() == CommonAccountingTransactionTypesEnum.REALLOC.ToString())
            {
                btnAddAllocations.Visible = true;
                btnAddAllocations.Text = Catalog.GetString("Add Reallocation");
            }
            else
            {
                btnAddAllocations.Visible = false;
            }
        }

        private void AddAllocations(Object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (cmbDetailTransactionTypeCode.GetSelectedString() == CommonAccountingTransactionTypesEnum.ALLOC.ToString())
            {
                TFrmAllocationJournalDialog AddAllocationJournal = new TFrmAllocationJournalDialog(this.FindForm());
                AddAllocationJournal.Journal = this.GetSelectedDetailRow();

                // open as a modal form
                if (AddAllocationJournal.ShowDialog() == DialogResult.OK)
                {
                    FMainDS.Merge(AddAllocationJournal.MainDS);

                    // manually enable save button (otherwise this doesn't happen)
                    FPetraUtilsObject.SetChangedFlag();
                }
            }
            else if (cmbDetailTransactionTypeCode.GetSelectedString() == CommonAccountingTransactionTypesEnum.REALLOC.ToString())
            {
                TFrmReallocationJournalDialog AddReallocationJournal = new TFrmReallocationJournalDialog(this.FindForm());
                AddReallocationJournal.Journal = this.GetSelectedDetailRow();

                // open as a modal form
                if (AddReallocationJournal.ShowDialog() == DialogResult.OK)
                {
                    FMainDS.Merge(AddReallocationJournal.MainDS);

                    // manually enable save button (otherwise this doesn't happen)
                    FPetraUtilsObject.SetChangedFlag();
                }
            }

            Cursor = Cursors.Default;
        }
    }
}