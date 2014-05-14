//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Common
{
    public partial class TFrmSelectBankStatementDialog
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FStatementKey = -1;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                if (FLedgerNumber != value)
                {
                    btnOK.Visible = false;
                    FLedgerNumber = value;
                    dtpShowStatementsFrom.Date = DateTime.Now.AddMonths(-2);
                    // will be called by dtp event: PopulateStatementGrid(null, null);

                    if (((DevAge.ComponentModel.BoundDataView)grdSelectStatement.DataSource).Count == 0)
                    {
                        dtpShowStatementsFrom.Clear();
                        PopulateStatementGrid(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// the selected bank statement
        /// </summary>
        public Int32 StatementKey
        {
            get
            {
                return FStatementKey;
            }
        }

        private bool RunningPopulateStatementGrid = false;

        private void PopulateStatementGrid(object sender, EventArgs e)
        {
            if (RunningPopulateStatementGrid)
            {
                return;
            }

            // somehow, the datetimepicker throws an event, when we are reading the Date property
            RunningPopulateStatementGrid = true;

            DateTime dateStatementsFrom = DateTime.MinValue;

            if (dtpShowStatementsFrom.Date.HasValue)
            {
                dateStatementsFrom = dtpShowStatementsFrom.Date.Value;
            }

            // update the grid with the bank statements
            AEpStatementTable stmts = TRemote.MFinance.ImportExport.WebConnectors.GetImportedBankStatements(FLedgerNumber, dateStatementsFrom);

            grdSelectStatement.Columns.Clear();
            grdSelectStatement.AddTextColumn(Catalog.GetString("Bank statement"), stmts.ColumnFilename);
            grdSelectStatement.AddDateColumn(Catalog.GetString("Date"), stmts.ColumnDate);

            stmts.DefaultView.AllowNew = false;
            stmts.DefaultView.Sort = AEpStatementTable.GetDateDBName() + " desc";
            grdSelectStatement.DataSource = new DevAge.ComponentModel.BoundDataView(stmts.DefaultView);

            grdSelectStatement.AutoSizeCells();

            RunningPopulateStatementGrid = false;
        }

        private void LoadStatement(object sender, EventArgs e)
        {
            DataRowView[] SelectedGridRow = grdSelectStatement.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                FStatementKey = ((AEpStatementRow)SelectedGridRow[0].Row).StatementKey;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ImportNewStatement(object sender, EventArgs e)
        {
            TFrmImportNewBankStatementDialog DlgImport = new TFrmImportNewBankStatementDialog(this);

            DlgImport.LedgerNumber = FLedgerNumber;

            if (DlgImport.ShowDialog() == DialogResult.OK)
            {
                // look for available plugin for importing a bank statement.
                // the plugin will upload the data into the tables a_ep_statement and a_ep_transaction on the server/database

                string BankStatementImportPlugin = TFrmImportNewBankStatementDialog.PluginNamespace + "." + TUserDefaults.GetStringDefault(
                    TUserDefaults.FINANCE_BANKIMPORT_PLUGIN);

                // namespace of the class TBankStatementImport, eg. Plugin.BankImportFromCSV
                // the dll has to be in the normal application directory
                string Namespace = BankStatementImportPlugin;
                string NameOfDll = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + BankStatementImportPlugin + ".dll";
                string NameOfClass = Namespace + ".TBankStatementImport";

                if (!File.Exists(NameOfDll))
                {
                    MessageBox.Show(Catalog.GetString("Please select a valid plugin for the import of bank statements!"));
                    return;
                }

                // dynamic loading of dll
                System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
                System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

                IImportBankStatement ImportBankStatement = (IImportBankStatement)Activator.CreateInstance(CustomClass);

                if (ImportBankStatement.ImportBankStatement(out FStatementKey, FLedgerNumber, DlgImport.FAccountCode))
                {
                    if (FStatementKey > -1)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private void DeleteStatement(object sender, EventArgs e)
        {
            DataRowView[] SelectedGridRow = grdSelectStatement.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                AEpStatementRow toDelete = (AEpStatementRow)SelectedGridRow[0].Row;

                if (MessageBox.Show(
                        String.Format(Catalog.GetString("Do you really want to delete the bank statement {0}?"),
                            toDelete.Filename),
                        Catalog.GetString("Confirmation"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (TRemote.MFinance.ImportExport.WebConnectors.DropBankStatement(toDelete.StatementKey))
                    {
                        PopulateStatementGrid(null, null);
                    }
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // dummy implementation
        }
    }
}