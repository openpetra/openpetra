//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftBatches
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
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

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, ActiveOnly, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, ActiveOnly, true);
            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);


            ShowData();
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
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
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private void ShowDetailsManual(ARecurringGiftBatchRow ARow)
        {
            FPetraUtilsObject.DetailProtectedMode = false;
            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactionsTab();
            UpdateChangeableStatus();
            FPetraUtilsObject.DetailProtectedMode = false;
            ((TFrmRecurringGiftBatch)ParentForm).LoadTransactions(
                ARow.LedgerNumber,
                ARow.BatchNumber);

            // FSelectedBatchNumber = ARow.BatchNumber;
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGiftBatch)ParentForm).SelectTab(TFrmRecurringGiftBatch.eGiftTabs.Transactions);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewARecurringGiftBatch();
        }

        private void DeleteRow(System.Object sender, System.EventArgs e)
        {
            //TODO
        }

        private void Submit(System.Object sender, System.EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please select a Batch before submitting."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmRecurringGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then submit it!"));
                    return;
                }
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

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null);


            this.btnDelete.Enabled = changeable;
            pnlDetails.Enabled = changeable;
        }

        /// <summary>
        /// return the method of Payment for the transaction tab
        /// </summary>

        public String MethodOfPaymentCode {
            get
            {
                return cmbDetailMethodOfPaymentCode.GetSelectedString();
            }
        }
        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateControlsProtection();
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            String ACurrencyCode = cmbDetailCurrencyCode.GetSelectedString();

            txtDetailHashTotal.CurrencySymbol = ACurrencyCode;
            ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(ACurrencyCode);
        }

        private void HashTotalChanged(object sender, EventArgs e)
        {
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
    }
}