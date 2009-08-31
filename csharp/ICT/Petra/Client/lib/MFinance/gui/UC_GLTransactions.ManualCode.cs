/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;
        private Int32 FJournalNumber;

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;

            GetDataFromControls();

            FCurrentDetailIndex = -1;

            DataView view = new DataView(FMainDS.ATransaction);

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(AJournalTable.TableId), ",");

            if (view.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber }) == -1)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransaction(ALedgerNumber, ABatchNumber, AJournalNumber));
            }

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceComboboxes.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, ActiveOnly);
            TFinanceComboboxes.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, ActiveOnly, false);

            ShowData();
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private AJournalRow GetJournalRow()
        {
            DataView view = new DataView(FMainDS.AJournal);

            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(AJournalTable.TableId), ",");
            return (AJournalRow)view.FindRows(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber })[0].Row;
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewATransaction();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        private void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            AJournalRow row = GetJournalRow();

            ANewRow.LedgerNumber = row.LedgerNumber;
            ANewRow.BatchNumber = row.BatchNumber;
            ANewRow.JournalNumber = row.JournalNumber;
            ANewRow.TransactionNumber = row.LastTransactionNumber + 1;
            row.LastTransactionNumber++;
        }

        /// <summary>
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = FLedgerNumber.ToString();
            txtBatchNumber.Text = FBatchNumber.ToString();
            txtJournalNumber.Text = FJournalNumber.ToString();

            if (FMainDS.ALedger.Count == 1)
            {
                lblBaseCurrency.Text = String.Format(Catalog.GetString("{0} (Base Currency)"), FMainDS.ALedger[0].BaseCurrency);
                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), GetJournalRow().TransactionCurrency);
            }
        }

        private void ShowDetailsManual(Int32 ACurrentDetailIndex)
        {
            ATransactionRow row = FMainDS.ATransaction[ACurrentDetailIndex];

            if (row.DebitCreditIndicator)
            {
                txtDebitAmountBase.Text = row.AmountInBaseCurrency.ToString();
                txtCreditAmountBase.Text = "0";
                txtDebitAmount.Text = row.TransactionAmount.ToString();
                txtCreditAmount.Text = "0";
            }
            else
            {
                txtDebitAmountBase.Text = "0";
                txtCreditAmountBase.Text = row.AmountInBaseCurrency.ToString();
                txtDebitAmount.Text = "0";
                txtCreditAmount.Text = row.TransactionAmount.ToString();
            }
        }

        private void GetDetailDataFromControlsManual(Int32 ACurrentDetailIndex)
        {
            ATransactionRow row = FMainDS.ATransaction[ACurrentDetailIndex];

            row.DebitCreditIndicator = (txtDebitAmount.Text.Length > 0 && Convert.ToDouble(txtDebitAmount.Text) > 0);

            if (row.DebitCreditIndicator)
            {
                row.TransactionAmount = Convert.ToDouble(txtDebitAmount.Text);
            }
            else
            {
                row.TransactionAmount = Convert.ToDouble(txtCreditAmount.Text);
            }
        }

        // TODO: verification: currency: must be double; check decimal point; only positive

        private void UpdateBaseAndTotals(System.Object sender, EventArgs e)
        {
            // TODO: update base value, and totals
        }
    }
}