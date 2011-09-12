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
using System.Data;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Common
{
    /// manual methods for the generated window
    public partial class TFrmBankStatementImport
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                TFinanceControls.InitialiseAccountList(ref cmbSelectBankAccount, FLedgerNumber, true, false, true, true);

                ALedgerRow Ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
                txtCreditSum.CurrencySymbol = Ledger.BaseCurrency;
                txtDebitSum.CurrencySymbol = Ledger.BaseCurrency;
                txtAmount.CurrencySymbol = Ledger.BaseCurrency;

                // we can only load the statements when the ledger number is known
                PopulateStatementCombobox();
                cmbSelectStatement.SelectedIndex = cmbSelectStatement.Items.Count - 1;
            }
        }

        private BankImportTDS FMainDS = new BankImportTDS();
        private DataView FMatchView = null;
        private DataView FTransactionView = null;

        private void InitializeManualCode()
        {
            pnlDetails.Visible = false;
            cmbSelectStatement.SelectedValueChanged += new EventHandler(SelectBankStatement);
        }

        private void SelectBankStatement(System.Object sender, EventArgs e)
        {
            // TODO: check if we want to save the changed matches?
            SaveMatches(null, null);

            CurrentlySelectedMatch = null;
            CurrentStatement = null;

            if (cmbSelectStatement.GetSelectedInt32() == -1)
            {
                // no statement selected, therefore show no transactions. happens when deleting a statement
                if (FTransactionView != null)
                {
                    FTransactionView.RowFilter = "false";
                    pnlDetails.Visible = false;
                }

                return;
            }

            FMainDS.AEpStatement.DefaultView.RowFilter = String.Format("{0}={1}",
                AEpStatementTable.GetStatementKeyDBName(),
                cmbSelectStatement.GetSelectedInt32());
            CurrentStatement = (AEpStatementRow)FMainDS.AEpStatement.DefaultView[0].Row;
            FMainDS.AEpStatement.DefaultView.RowFilter = String.Empty;

            // load the transactions of the selected statement, and the matches
            FMainDS.Merge(
                TRemote.MFinance.ImportExport.WebConnectors.GetBankStatementTransactionsAndMatches(
                    CurrentStatement.StatementKey, FLedgerNumber));

            grdAllTransactions.Columns.Clear();
            grdAllTransactions.AddTextColumn(Catalog.GetString("Nr"), FMainDS.AEpTransaction.ColumnOrder, 40);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Account Name"), FMainDS.AEpTransaction.ColumnAccountName, 150);
            grdAllTransactions.AddTextColumn(Catalog.GetString("description"), FMainDS.AEpTransaction.ColumnDescription, 200);
            grdAllTransactions.AddDateColumn(Catalog.GetString("Date Effective"), FMainDS.AEpTransaction.ColumnDateEffective);
            grdAllTransactions.AddCurrencyColumn(Catalog.GetString("Transaction Amount"), FMainDS.AEpTransaction.ColumnTransactionAmount);

            FTransactionView = FMainDS.AEpTransaction.DefaultView;
            FTransactionView.AllowNew = false;
            FTransactionView.Sort = AEpTransactionTable.GetOrderDBName() + " ASC";
            grdAllTransactions.DataSource = new DevAge.ComponentModel.BoundDataView(FTransactionView);

            TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetail, FLedgerNumber, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbGiftCostCentre, FLedgerNumber, true, false, true, true);
            TFinanceControls.InitialiseAccountList(ref cmbGiftAccount, FLedgerNumber, true, false, true, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbGLCostCentre, FLedgerNumber, true, false, true, true);
            TFinanceControls.InitialiseAccountList(ref cmbGLAccount, FLedgerNumber, true, false, true, false);

            grdGiftDetails.Columns.Clear();
            grdGiftDetails.AddTextColumn(Catalog.GetString("Motivation"), FMainDS.AEpMatch.ColumnMotivationDetailCode, 100);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre"), FMainDS.AEpMatch.ColumnCostCentreCode, 150);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre Name"), FMainDS.AEpMatch.ColumnCostCentreName, 200);
            grdGiftDetails.AddCurrencyColumn(Catalog.GetString("Amount"), FMainDS.AEpMatch.ColumnGiftTransactionAmount);
            FMatchView = FMainDS.AEpMatch.DefaultView;
            FMatchView.AllowNew = false;
            grdGiftDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMatchView);

            TFinanceControls.InitialiseAccountList(ref cmbSelectBankAccount, FLedgerNumber, true, false, true, true);

            if (CurrentStatement != null)
            {
                FMainDS.AEpStatement.DefaultView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey);
                cmbSelectBankAccount.SetSelectedString(CurrentStatement.BankAccountCode);
                FMainDS.AEpStatement.DefaultView.RowFilter = string.Empty;
            }

            rbtListAll.Checked = true;
            TransactionFilterChanged(null, null);
            grdAllTransactions.SelectRowInGrid(1);
        }

        private void PopulateStatementCombobox()
        {
            // TODO: add datetimepicker to toolstrip
            // see http://www.daniweb.com/forums/thread109966.html#
            // dtTScomponent = new ToolStripControlHost(dtMyDateTimePicker);
            // MainToolStrip.Items.Add(dtTScomponent);
            // DateTime.Now.AddMonths(-100);
            DateTime dateStatementsFrom = DateTime.MinValue;

            // update the combobox with the bank statements
            AEpStatementTable stmts = TRemote.MFinance.ImportExport.WebConnectors.GetImportedBankStatements(dateStatementsFrom);

            FMainDS.Merge(stmts);

            cmbSelectStatement.BeginUpdate();
            cmbSelectStatement.DataSource = stmts.DefaultView;
            cmbSelectStatement.DisplayMember = AEpStatementTable.GetFilenameDBName();
            cmbSelectStatement.ValueMember = AEpStatementTable.GetStatementKeyDBName();
            cmbSelectStatement.DataSource = stmts.DefaultView;
            cmbSelectStatement.DropDownWidth = 300;
            cmbSelectStatement.EndUpdate();

            cmbSelectStatement.SelectedIndex = -1;
        }

        private void ImportNewStatement(System.Object sender, EventArgs e)
        {
            // look for available plugin for importing a bank statement.
            // the plugin will upload the data into the tables a_ep_statement and a_ep_transaction on the server/database
            string BankStatementImportPlugin = TAppSettingsManager.GetValue("Plugin.BankStatementImport", "");

            if (BankStatementImportPlugin.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Please install a valid plugin for the import of bank statements!"));
                return;
            }

            if (cmbSelectBankAccount.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Please create a bank account first, before importing bank statements!"),
                    Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // namespace of the class TBankStatementImport, eg. Plugin.BankImportFromCSV
            // the dll has to be in the normal application directory
            string Namespace = BankStatementImportPlugin;
            string NameOfDll = Namespace + ".dll";
            string NameOfClass = Namespace + ".TBankStatementImport";

            // dynamic loading of dll
            System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
            System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

            IImportBankStatement ImportBankStatement = (IImportBankStatement)Activator.CreateInstance(CustomClass);

            Int32 StatementKey;

            if (ImportBankStatement.ImportBankStatement(out StatementKey))
            {
                PopulateStatementCombobox();

                // select the loaded bank statement and display all transactions
                cmbSelectStatement.SetSelectedInt32(StatementKey);
                SelectBankStatement(null, null);
            }
        }

        private void MotivationDetailChanged(System.Object sender, EventArgs e)
        {
            cmbGiftCostCentre.Enabled = false;
            cmbGiftAccount.Enabled = false;

            // look for the motivation detail.
            if (cmbMotivationDetail.SelectedIndex == -1)
            {
                return;
            }

            DataView v = new DataView(FMainDS.AMotivationDetail);
            v.RowFilter = AMotivationDetailTable.GetMotivationDetailCodeDBName() +
                          " = '" + cmbMotivationDetail.GetSelectedString() + "'";

            if (v.Count == 0)
            {
                return;
            }

            AMotivationDetailRow motivationDetailRow = (AMotivationDetailRow)v[0].Row;

            cmbGiftAccount.Filter = AAccountTable.GetAccountCodeDBName() + " = '" + motivationDetailRow.AccountCode + "'";
            cmbGiftCostCentre.Filter = ACostCentreTable.GetCostCentreCodeDBName() + " = '" + motivationDetailRow.CostCentreCode + "'";
        }

        private void NewTransactionCategory(System.Object sender, EventArgs e)
        {
            // do NOT call GetValuesFromScreen to avoid disappearing transaction from the grid
            // GetValuesFromScreen();
            CurrentlySelectedMatch = null;

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;

            pnlGiftEdit.Visible = rbtGift.Checked;
            pnlGLEdit.Visible = rbtGL.Checked;

            if (rbtGift.Checked)
            {
                // select first detail
                grdGiftDetails.SelectRowInGrid(1);
                AEpMatchRow match = GetSelectedMatch();
                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());
            }

            if (rbtGL.Checked)
            {
                DisplayGLDetails();
            }

            if (rbtUnmatched.Checked && (FMatchView != null))
            {
                foreach (DataRowView rv in FMatchView)
                {
                    ((AEpMatchRow)rv.Row).Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                    if (CurrentlySelectedTransaction.EpMatchKey != ((AEpMatchRow)rv.Row).EpMatchKey)
                    {
                        ((AEpMatchRow)rv.Row).Delete();
                    }
                }
            }

            if (rbtIgnored.Checked && (FMatchView != null))
            {
                foreach (DataRowView rv in FMatchView)
                {
                    ((AEpMatchRow)rv.Row).Action = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;

                    if (CurrentlySelectedTransaction.EpMatchKey != ((AEpMatchRow)rv.Row).EpMatchKey)
                    {
                        ((AEpMatchRow)rv.Row).Delete();
                    }
                }
            }
        }

        private AEpStatementRow CurrentStatement = null;
        private BankImportTDSAEpTransactionRow CurrentlySelectedTransaction = null;
        private BankImportTDSAEpMatchRow CurrentlySelectedMatch = null;
        private bool rbtGLWasChecked = false;
        private bool rbtGiftWasChecked = false;

        private void DeleteStatement(System.Object Sender, EventArgs e)
        {
            if (CurrentStatement != null)
            {
                if (TRemote.MFinance.ImportExport.WebConnectors.DropBankStatement(CurrentStatement.StatementKey))
                {
                    FMainDS.AEpStatement.Rows.Remove(CurrentStatement);
                    CurrentStatement = null;
                    PopulateStatementCombobox();
                    SelectBankStatement(null, null);
                }
            }
        }

        /// store current selections in the a_ep_match table
        private void GetValuesFromScreen()
        {
            if (CurrentStatement != null)
            {
                CurrentStatement.LedgerNumber = FLedgerNumber;
                CurrentStatement.BankAccountCode = cmbSelectBankAccount.GetSelectedString();
            }

            if (CurrentlySelectedTransaction == null)
            {
                return;
            }

            if (rbtUnmatched.Checked)
            {
                if (FMatchView != null)
                {
                    for (int i = 0; i < FMatchView.Count; i++)
                    {
                        AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                        match.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                    }
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
            }

            if (rbtIgnored.Checked)
            {
                if (FMatchView != null)
                {
                    for (int i = 0; i < FMatchView.Count; i++)
                    {
                        AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                        match.Action = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;
                    }
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING;
            }

            if (CurrentlySelectedMatch == null)
            {
                return;
            }

            if (rbtGiftWasChecked)
            {
                for (int i = 0; i < FMatchView.Count; i++)
                {
                    AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                    match.DonorKey = Convert.ToInt64(txtDonorKey.Text);
                    match.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;

                GetGiftDetailValuesFromScreen();

                // TODO: validation> calculate the sum of the gift details and check with the bank transaction amount
            }

            if (rbtGLWasChecked)
            {
                CurrentlySelectedMatch.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;
                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;

                GetGLValuesFromScreen();
            }
        }

        private void AllTransactionsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            pnlDetails.Visible = true;

            GetValuesFromScreen();

            CurrentlySelectedMatch = null;

            try
            {
                CurrentlySelectedTransaction = ((BankImportTDSAEpTransactionRow)grdAllTransactions.SelectedDataRowsAsDataRowView[0].Row);
            }
            catch (System.IndexOutOfRangeException)
            {
                // this can happen when the transaction type has changed, and the row disappears from the grid
                // select another row
                grdAllTransactions.SelectRowInGrid(1);
                return;
            }

            // load selections from the a_ep_match table for the new row
            FMatchView.RowFilter = AEpMatchTable.GetMatchTextDBName() +
                                   " = '" + CurrentlySelectedTransaction.MatchText + "'";

            AEpMatchRow match = (AEpMatchRow)FMatchView[0].Row;

            if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT)
            {
                rbtGift.Checked = true;

                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());

                grdGiftDetails.SelectRowInGrid(1);
                grdAllTransactions.Focus();
            }
            else if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL)
            {
                rbtGL.Checked = true;
                DisplayGLDetails();
            }
            else if (match.Action == MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING)
            {
                rbtIgnored.Checked = true;
            }
            else
            {
                rbtUnmatched.Checked = true;
            }

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;
        }

        private void GiftDetailsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            GetGiftDetailValuesFromScreen();
            CurrentlySelectedMatch = GetSelectedMatch();
            DisplayGiftDetails();
        }

        private BankImportTDSAEpMatchRow GetSelectedMatch()
        {
            DataRowView[] SelectedGridRow = grdGiftDetails.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (BankImportTDSAEpMatchRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        private void DisplayGiftDetails()
        {
            CurrentlySelectedMatch = GetSelectedMatch();

            if (CurrentlySelectedMatch != null)
            {
                txtAmount.NumberValueDecimal = CurrentlySelectedMatch.GiftTransactionAmount;

                if (CurrentlySelectedMatch.IsMotivationDetailCodeNull())
                {
                    cmbMotivationDetail.SelectedIndex = -1;
                }
                else
                {
                    cmbMotivationDetail.SetSelectedString(CurrentlySelectedMatch.MotivationDetailCode);
                }

                if (CurrentlySelectedMatch.IsAccountCodeNull())
                {
                    cmbGiftAccount.SelectedIndex = -1;
                }
                else
                {
                    cmbGiftAccount.SetSelectedString(CurrentlySelectedMatch.AccountCode);
                }

                if (CurrentlySelectedMatch.IsCostCentreCodeNull())
                {
                    cmbGiftCostCentre.SelectedIndex = -1;
                }
                else
                {
                    cmbGiftCostCentre.SetSelectedString(CurrentlySelectedMatch.CostCentreCode);
                }
            }
        }

        private void GetGiftDetailValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                // TODO: support more motivation groups.
                CurrentlySelectedMatch.MotivationGroupCode = FMainDS.AMotivationDetail[0].MotivationGroupCode;
                CurrentlySelectedMatch.MotivationDetailCode = cmbMotivationDetail.GetSelectedString();
                CurrentlySelectedMatch.AccountCode = cmbGiftAccount.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = cmbGiftCostCentre.GetSelectedString();
                CurrentlySelectedMatch.GiftTransactionAmount = txtAmount.NumberValueDecimal.Value;
                CurrentlySelectedMatch.DonorKey = Convert.ToInt64(txtDonorKey.Text);

                FMainDS.ACostCentre.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    ACostCentreTable.GetCostCentreCodeDBName(), CurrentlySelectedMatch.CostCentreCode);

                if (FMainDS.ACostCentre.DefaultView.Count == 1)
                {
                    CurrentlySelectedMatch.CostCentreName = ((ACostCentreRow)FMainDS.ACostCentre.DefaultView[0].Row).CostCentreName;
                }

                FMainDS.ACostCentre.DefaultView.RowFilter = "";
            }
        }

        private void DisplayGLDetails()
        {
            // there is only one match?
            // TODO: support split GL transactions
            CurrentlySelectedMatch = (BankImportTDSAEpMatchRow)FMatchView[0].Row;

            if (CurrentlySelectedMatch != null)
            {
                if (CurrentlySelectedMatch.IsAccountCodeNull())
                {
                    cmbGLAccount.SelectedIndex = -1;
                }
                else
                {
                    cmbGLAccount.SetSelectedString(CurrentlySelectedMatch.AccountCode);
                }

                if (CurrentlySelectedMatch.IsCostCentreCodeNull())
                {
                    cmbGLCostCentre.SelectedIndex = -1;
                }
                else
                {
                    cmbGLCostCentre.SetSelectedString(CurrentlySelectedMatch.CostCentreCode);
                }

                if (!CurrentlySelectedMatch.IsReferenceNull())
                {
                    txtGLReference.Text = CurrentlySelectedMatch.Reference;
                }

                if (CurrentlySelectedMatch.IsNarrativeNull())
                {
                    txtGLNarrative.Text = CurrentlySelectedTransaction.Description;
                }
                else
                {
                    txtGLNarrative.Text = CurrentlySelectedMatch.Narrative;
                }
            }
        }

        private void GetGLValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                CurrentlySelectedMatch.AccountCode = cmbGLAccount.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = cmbGLCostCentre.GetSelectedString();
                CurrentlySelectedMatch.Reference = txtGLReference.Text;
                CurrentlySelectedMatch.Narrative = txtGLNarrative.Text;
            }
        }

        private Int32 NewMatchKey = -1;

        private void AddGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // get a new detail number
            Int32 newDetailNumber = 0;
            decimal amount = 0;
            AEpMatchRow match = null;

            for (int i = 0; i < FMatchView.Count; i++)
            {
                match = (AEpMatchRow)FMatchView[i].Row;

                if (match.Detail >= newDetailNumber)
                {
                    newDetailNumber = match.Detail + 1;
                }

                amount += match.GiftTransactionAmount;
            }

            if (match != null)
            {
                AEpMatchRow newRow = FMainDS.AEpMatch.NewRowTyped();
                newRow.EpMatchKey = NewMatchKey--;
                newRow.MatchText = match.MatchText;
                newRow.Detail = newDetailNumber;
                newRow.LedgerNumber = match.LedgerNumber;
                newRow.AccountCode = match.AccountCode;
                newRow.CostCentreCode = match.CostCentreCode;
                newRow.DonorKey = match.DonorKey;
                newRow.GiftTransactionAmount = CurrentlySelectedTransaction.TransactionAmount -
                                               amount;
                FMainDS.AEpMatch.Rows.Add(newRow);

                // select the new gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        private void RemoveGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            if (CurrentlySelectedMatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a row before deleting a detail"));
                return;
            }

            // we should never allow to delete all details, otherwise we have nothing to copy from
            // also cannot delete the first detail, since there is the foreign key from a_ep_transaction on epmatchkey?
            if (CurrentlySelectedTransaction.EpMatchKey == CurrentlySelectedMatch.EpMatchKey)
            {
                MessageBox.Show(Catalog.GetString("Cannot delete the first detail"));
            }
            else
            {
                CurrentlySelectedMatch.Delete();
                CurrentlySelectedMatch = null;

                // select the last gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        private void SaveMatches(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            if (TRemote.MFinance.ImportExport.WebConnectors.CommitMatches(FMainDS.GetChangesTyped(true)))
            {
                FMainDS.AcceptChanges();
            }
            else
            {
                MessageBox.Show(Catalog.GetString(
                        "The matches could not be stored. Please ask your System Administrator to check the log file on the server."));
            }
        }

        private void CreateGiftBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveMatches(null, null);

            TVerificationResultCollection VerificationResult;
            Int32 GiftBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGiftBatch(FMainDS,
                FLedgerNumber,
                CurrentStatement.StatementKey,
                -1,
                out VerificationResult);

            if (GiftBatchNumber != -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Please check Gift Batch {0}"), GiftBatchNumber));
            }
            else
            {
                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.GetVerificationResult(0).ResultText,
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
                else
                {
                    MessageBox.Show(
                        VerificationResult.GetVerificationResult(0).ResultText,
                        Catalog.GetString("Problem: No gift batch has been created"));
                }
            }
        }

        private void CreateGLBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveMatches(null, null);

            TVerificationResultCollection VerificationResult;
            Int32 GLBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGLBatch(FMainDS,
                FLedgerNumber,
                CurrentStatement.StatementKey,
                -1,
                out VerificationResult);

            if (GLBatchNumber != -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Please check GL Batch {0}"), GLBatchNumber));
            }
            else
            {
                if (VerificationResult != null)
                {
                    MessageBox.Show(
                        VerificationResult.GetVerificationResult(0).ResultText,
                        Catalog.GetString("Problem: No GL batch has been created"));
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Problem: No GL batch has been created"),
                        Catalog.GetString("Error"));
                }
            }
        }

        private void TransactionFilterChanged(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();
            pnlDetails.Visible = false;
            CurrentlySelectedMatch = null;
            CurrentlySelectedTransaction = null;

            if (FTransactionView == null)
            {
                return;
            }

            if (rbtListAll.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey);
            }
            else if (rbtListGift.Checked)
            {
                // TODO: allow splitting a transaction, one part is GL/AP, the other is a donation?
                //       at Top Level: split transaction, results into 2 rows in aeptransaction (not stored). Merge Transactions again?

                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT);
            }
            else if (rbtListUnmatched.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_UNMATCHED);
            }
            else if (rbtListIgnored.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_NO_MATCHING);
            }
            else if (rbtListGL.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AEpStatementTable.GetStatementKeyDBName(),
                    CurrentStatement.StatementKey,
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL);
            }

            // update sumcredit and sumdebit
            decimal sumCredit = 0.0M;
            decimal sumDebit = 0.0M;

            foreach (DataRowView rv in FTransactionView)
            {
                AEpTransactionRow Row = (AEpTransactionRow)rv.Row;

                if (Row.TransactionAmount < 0)
                {
                    sumDebit += Row.TransactionAmount * -1.0M;
                }
                else
                {
                    sumCredit += Row.TransactionAmount;
                }
            }

            txtCreditSum.NumberValueDecimal = sumCredit;
            txtDebitSum.NumberValueDecimal = sumDebit;

            if (FTransactionView.Count > 0)
            {
                grdAllTransactions.SelectRowInGrid(1);
            }
        }
    }
}