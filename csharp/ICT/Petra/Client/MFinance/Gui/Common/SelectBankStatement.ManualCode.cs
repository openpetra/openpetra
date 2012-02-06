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
    public partial class TFrmSelectBankStatement
    {
        private Int32 FLedgerNumber;
        private Int32 FStatementKey = -1;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                btnOK.Visible = false;
                FLedgerNumber = value;
                PopulateStatementCombobox();
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

            cmbSelectStatement.BeginUpdate();
            cmbSelectStatement.DisplayMember = AEpStatementTable.GetFilenameDBName();
            cmbSelectStatement.ValueMember = AEpStatementTable.GetStatementKeyDBName();
            cmbSelectStatement.DataSource = stmts.DefaultView;
            cmbSelectStatement.DropDownWidth = 300;
            cmbSelectStatement.EndUpdate();

            cmbSelectStatement.SelectedIndex = -1;
        }

        private void LoadStatement(object sender, EventArgs e)
        {
            if (cmbSelectStatement.SelectedIndex != -1)
            {
                FStatementKey = cmbSelectStatement.GetSelectedInt32();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ImportNewStatement(object sender, EventArgs e)
        {
            TFrmImportNewBankStatement DlgImport = new TFrmImportNewBankStatement(this);

            DlgImport.LedgerNumber = FLedgerNumber;

            if (DlgImport.ShowDialog() == DialogResult.OK)
            {
                // look for available plugin for importing a bank statement.
                // the plugin will upload the data into the tables a_ep_statement and a_ep_transaction on the server/database

                string BankStatementImportPlugin = TFrmImportNewBankStatement.PluginNamespace + "." + TUserDefaults.GetStringDefault(
                    TUserDefaults.FINANCE_BANKIMPORT_PLUGIN);

                if (!File.Exists(TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + BankStatementImportPlugin + ".dll"))
                {
                    MessageBox.Show(Catalog.GetString("Please select a valid plugin for the import of bank statements!"));
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

                if (ImportBankStatement.ImportBankStatement(out FStatementKey, FLedgerNumber, DlgImport.FAccountCode))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void DeleteStatement(object sender, EventArgs e)
        {
            if (cmbSelectStatement.SelectedIndex != -1)
            {
                if (MessageBox.Show(
                        String.Format(Catalog.GetString("Do you really want to delete the bank statement {0}?"),
                            cmbSelectStatement.GetSelectedDescription()),
                        Catalog.GetString("Confirmation"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (TRemote.MFinance.ImportExport.WebConnectors.DropBankStatement(cmbSelectStatement.GetSelectedInt32()))
                    {
                        PopulateStatementCombobox();
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