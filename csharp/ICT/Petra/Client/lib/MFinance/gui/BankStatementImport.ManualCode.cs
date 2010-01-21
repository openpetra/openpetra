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
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// manual methods for the generated window
    public partial class TFrmBankStatementImport
    {
        private void InitializeManualCode()
        {
            pnlDetails.Visible = false;
            tbcSelectStatement.ComboBox.SelectedValueChanged += new EventHandler(SelectBankStatement);
            PopulateStatementCombobox();
            tbcSelectStatement.ComboBox.SelectedIndex = tbcSelectStatement.ComboBox.Items.Count - 1;
        }

        private void SelectBankStatement(System.Object sender, EventArgs e)
        {
            BankImportTDS ds =
                TRemote.MFinance.ImportExport.WebConnectors.GetBankStatementTransactionsAndMatches(Convert.ToInt32(tbcSelectStatement.ComboBox.
                        SelectedValue));

            grdAllTransactions.Columns.Clear();
            grdAllTransactions.AddTextColumn(Catalog.GetString("Nr"), ds.AEpTransaction.ColumnOrder, 40);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Account Name"), ds.AEpTransaction.ColumnAccountName, 150);
            grdAllTransactions.AddTextColumn(Catalog.GetString("description"), ds.AEpTransaction.ColumnDescription, 150);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Date Effective"), ds.AEpTransaction.ColumnDateEffective, 70);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Transaction Amount"), ds.AEpTransaction.ColumnTransactionAmount, 70);

            DataView myDataView = ds.AEpTransaction.DefaultView;
            myDataView.AllowNew = false;
            grdAllTransactions.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdAllTransactions.AutoSizeCells();
        }

        private void PopulateStatementCombobox()
        {
            // TODO: add datetimepicker to toolstrip
            // see http://www.daniweb.com/forums/thread109966.html#
            // dtTScomponent = new ToolStripControlHost(dtMyDateTimePicker);
            // MainToolStrip.Items.Add(dtTScomponent);
            DateTime dateStatementsFrom = DateTime.Now.AddMonths(-14);

            // update the combobox with the bank statements
            AEpStatementTable stmts = TRemote.MFinance.ImportExport.WebConnectors.GetImportedBankStatements(dateStatementsFrom);

            tbcSelectStatement.ComboBox.BeginUpdate();
            tbcSelectStatement.ComboBox.DisplayMember = AEpStatementTable.GetDateDBName();
            tbcSelectStatement.ComboBox.ValueMember = AEpStatementTable.GetStatementKeyDBName();
            tbcSelectStatement.ComboBox.DataSource = stmts.DefaultView;
            tbcSelectStatement.ComboBox.EndUpdate();
        }

        private void ImportNewStatement(System.Object sender, EventArgs e)
        {
            // look for available plugin for importing a bank statement.
            // the plugin will upload the data into the tables a_ep_statement and a_ep_transaction on the server/database
            string BankStatementImportPlugin = TAppSettingsManager.GetValueStatic("Plugin.BankStatementImport", "");

            if (BankStatementImportPlugin.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Please install a valid plugin for the import of bank statements!"));
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
                tbcSelectStatement.ComboBox.SelectedValue = StatementKey;
            }
        }

        private void MotivationDetailChanged(System.Object sender, EventArgs e)
        {
            // TODO
        }

        private void NewTransactionCategory(System.Object sender, EventArgs e)
        {
            pnlGiftEdit.Visible = rbtGift.Checked;
        }

        private void AllTransactionsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            pnlDetails.Visible = true;

            // TODO store current selections in the a_ep_match table

            // TODO load selections from the a_ep_match table for the new row
        }

        private void GiftDetailsFocusedRowChanged(System.Object sender, EventArgs e)
        {
        }
    }
}