//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Generic;
using System.Xml;
using Mono.Unix;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLAccountHierarchy
    {
        private Int32 FLedgerNumber;
        private string FSelectedHierarchy = "STANDARD";

        /// <summary>
        /// Setup the account hierarchy of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber);

                PopulateTreeView();
            }
        }

        /// <summary>
        /// load account hierarchy from the dataset into the tree view
        /// </summary>
        private void PopulateTreeView()
        {
            trvAccounts.BeginUpdate();
            trvAccounts.Nodes.Clear();

            // TODO: select account hierarchy
            AAccountHierarchyRow accountHierarchy = (AAccountHierarchyRow)FMainDS.AAccountHierarchy.Rows.Find(new object[] { FLedgerNumber,
                                                                                                                             FSelectedHierarchy });

            if (accountHierarchy != null)
            {
                // find the BALSHT account that is reporting to the root account
                FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                    AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + FSelectedHierarchy + "' AND " +
                    AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + accountHierarchy.RootAccountCode + "'";

                InsertNodeIntoTreeView(trvAccounts.Nodes,
                    (AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.DefaultView[0].Row);
            }

            // reset filter so that the defaultview can be used for finding accounts (eg. when adding new account)
            FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter = "";

            trvAccounts.EndUpdate();

            this.trvAccounts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
        }

        private void InsertNodeIntoTreeView(TreeNodeCollection AParentNodes, AAccountHierarchyDetailRow ADetailRow)
        {
            AAccountRow currentAccount = (AAccountRow)FMainDS.AAccount.Rows.Find(
                new object[] { FLedgerNumber, ADetailRow.ReportingAccountCode });

            string nodeLabel = ADetailRow.ReportingAccountCode;

            if (!currentAccount.IsAccountCodeShortDescNull())
            {
                nodeLabel += " (" + currentAccount.AccountCodeShortDesc + ")";
            }

            TreeNode newNode = AParentNodes.Add(nodeLabel);

            newNode.Tag = ADetailRow;

            FMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName();
            FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + ADetailRow.AccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + ADetailRow.ReportingAccountCode + "'";

            foreach (DataRowView rowView in FMainDS.AAccountHierarchyDetail.DefaultView)
            {
                AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                InsertNodeIntoTreeView(newNode.Nodes, accountDetail);
            }
        }

        TreeNode FCurrentNode = null;

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != e.Node))
            {
                GLSetupTDSAAccountRow currentAccount = (GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(
                    new object[] { FLedgerNumber, ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode });
                string oldName = currentAccount.AccountCode;
                GetDetailsFromControls(currentAccount);

                // this only works for new rows; old rows have the primary key fields readonly
                if (currentAccount.AccountCode != oldName)
                {
                    // there are no references to this new row yet, apart from children nodes

                    // change name in the account hierarchy
                    ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode = currentAccount.AccountCode;

                    // fix children nodes, account hierarchy
                    foreach (TreeNode childnode in FCurrentNode.Nodes)
                    {
                        ((AAccountHierarchyDetailRow)childnode.Tag).AccountCodeToReportTo = currentAccount.AccountCode;
                    }
                }

                string nodeLabel = currentAccount.AccountCode;

                if (!currentAccount.IsAccountCodeShortDescNull())
                {
                    nodeLabel += " (" + currentAccount.AccountCodeShortDesc + ")";
                }

                FCurrentNode.Text = nodeLabel;
            }

            FCurrentNode = e.Node;

            // update detail panel
            ShowDetails((GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber,
                                                                                         ((AAccountHierarchyDetailRow)FCurrentNode.Tag).
                                                                                         ReportingAccountCode }));
        }

        private void AddNewAccount(Object sender, EventArgs e)
        {
            if (FCurrentNode == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new account after selecting a parent account"));
                return;
            }

            string newName = Catalog.GetString("NewAccount");
            Int32 countNewAccount = 0;

            if (FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, newName + countNewAccount.ToString() }) != null)
                {
                    countNewAccount++;
                }

                newName += countNewAccount.ToString();
            }

            AAccountRow parentAccount =
                (AAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber,
                                                                       ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode });

            AAccountRow newAccount = FMainDS.AAccount.NewRowTyped();
            newAccount.AccountCode = newName;
            newAccount.LedgerNumber = FLedgerNumber;
            newAccount.AccountActiveFlag = true;
            newAccount.DebitCreditIndicator = parentAccount.DebitCreditIndicator;
            newAccount.AccountType = parentAccount.AccountType;
            newAccount.ValidCcCombo = parentAccount.ValidCcCombo;
            newAccount.PostingStatus = true;
            FMainDS.AAccount.Rows.Add(newAccount);

            AAccountHierarchyDetailRow hierarchyDetailRow = FMainDS.AAccountHierarchyDetail.NewRowTyped();
            hierarchyDetailRow.LedgerNumber = FLedgerNumber;
            hierarchyDetailRow.AccountHierarchyCode = FSelectedHierarchy;
            hierarchyDetailRow.AccountCodeToReportTo = parentAccount.AccountCode;
            hierarchyDetailRow.ReportingAccountCode = newName;

            if (FCurrentNode.Nodes.Count == 0)
            {
                // change posting/summary flag of parent account if it was a leaf
                parentAccount.PostingStatus = false;
                hierarchyDetailRow.ReportOrder = 0;
            }
            else
            {
                AAccountHierarchyDetailRow siblingRow = (AAccountHierarchyDetailRow)FCurrentNode.Nodes[FCurrentNode.Nodes.Count - 1].Tag;

                if (siblingRow.IsReportOrderNull())
                {
                    siblingRow.ReportOrder = 1;
                }

                hierarchyDetailRow.ReportOrder = siblingRow.ReportOrder + 1;
            }

            FMainDS.AAccountHierarchyDetail.Rows.Add(hierarchyDetailRow);

            trvAccounts.BeginUpdate();
            TreeNode newNode = FCurrentNode.Nodes.Add(newName);
            newNode.Tag = hierarchyDetailRow;
            trvAccounts.EndUpdate();

            trvAccounts.SelectedNode = newNode;
        }

        private void ExportHierarchy(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(TRemote.MFinance.Setup.WebConnectors.ExportAccountHierarchy(FLedgerNumber, FSelectedHierarchy));

            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Account Hierarchy to file"));
        }

        private void ImportHierarchy(object sender, EventArgs e)
        {
            // TODO: open file; only will work if there are no GLM records and transactions yet
            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Load Account Hierarchy from file"));

            if (doc == null)
            {
                // was cancelled
                return;
            }

            if (!TRemote.MFinance.Setup.WebConnectors.ImportAccountHierarchy(FLedgerNumber, FSelectedHierarchy, TXMLParser.XmlToString(doc)))
            {
                MessageBox.Show(Catalog.GetString(
                        "Import of new Account Hierarchy failed; perhaps there were already balances? Try with a new ledger!"),
                    Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // refresh the screen
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber);
                PopulateTreeView();

                MessageBox.Show("Import of new Account Hierarchy has been successful",
                    Catalog.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteUnusedAccount(Object sender, EventArgs ev)
        {
            string AccountCode = ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode;

            if (!TRemote.MFinance.Setup.WebConnectors.CanDeleteAccount(FLedgerNumber,
                    AccountCode))
            {
                MessageBox.Show(
                    String.Format(Catalog.GetString(
                            "Account {0} cannot be deleted because it has already been used in GL transactions, or it is a system or summary account; you can deactivate the account, but not delete it."),
                        AccountCode),
                    Catalog.GetString("Account cannot be deleted"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // select parent node first, so that account to be deleted will not be updated later anymore
                TreeNode NodeToBeDeleted = FCurrentNode;
                trvAccounts.SelectedNode = FCurrentNode.Parent;

                trvAccounts.BeginUpdate();
                NodeToBeDeleted.Remove();
                trvAccounts.EndUpdate();

                // TODO: what about other account hierarchies, that are still referencing this account?
                AAccountHierarchyDetailRow AccountHDetailToBeDeleted = (AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.Rows.Find(
                    new object[] { FLedgerNumber, FSelectedHierarchy, AccountCode });
                AccountHDetailToBeDeleted.Delete();
                AAccountRow AccountToBeDeleted = (AAccountRow)FMainDS.AAccount.Rows.Find(
                    new object[] { FLedgerNumber, AccountCode });
                AccountToBeDeleted.Delete();

                // if parent of deleted node has no children, mark as posting account
                // TODO: this also works only if there is just one account hierarchy
                if (trvAccounts.SelectedNode.Nodes.Count == 0)
                {
                    AAccountRow AccountParent = (AAccountRow)FMainDS.AAccount.Rows.Find(
                        new object[] { FLedgerNumber, ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode });
                    AccountParent.PostingStatus = true;
                }
            }
        }

        private void GetDataFromControlsManual()
        {
            // TODO: report to (drag/drop)
            // TODO: report order (drag/drop)
            // TODO: posting/summary (new/delete)

            if (FCurrentNode != null)
            {
                GLSetupTDSAAccountRow currentAccount = (GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(
                    new object[] { FLedgerNumber, ((AAccountHierarchyDetailRow)FCurrentNode.Tag).ReportingAccountCode });
                GetDetailsFromControls(currentAccount);
            }
        }
    }
}