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
using System.Collections.Generic;
using Mono.Unix;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
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
                FMainDS = TRemote.MFinance.GL.WebConnectors.LoadAccountHierarchies(FLedgerNumber);

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
            FMainDS.AAccountHierarchy.DefaultView.RowFilter =
                AAccountHierarchyTable.GetAccountHierarchyCodeDBName() + " = '" + FSelectedHierarchy + "'";

            if (FMainDS.AAccountHierarchy.DefaultView.Count == 1)
            {
                InsertNodeIntoTreeView(trvAccounts.Nodes, FSelectedHierarchy,
                    ((AAccountHierarchyRow)FMainDS.AAccountHierarchy.DefaultView[0].Row).RootAccountCode);
            }

            // reset filter so that the defaultview can be used for finding accounts (eg. when adding new account)
            FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter = "";

            trvAccounts.EndUpdate();

            this.trvAccounts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
        }

        private void InsertNodeIntoTreeView(TreeNodeCollection AParentNodes, string AAccountHierarchyCode, string AAccountCode)
        {
            TreeNode newNode = AParentNodes.Add(AAccountCode);

            newNode.Tag = AAccountCode;

            FMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName();
            FMainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + AAccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + AAccountCode + "'";

            foreach (DataRowView rowView in FMainDS.AAccountHierarchyDetail.DefaultView)
            {
                AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                InsertNodeIntoTreeView(newNode.Nodes, AAccountHierarchyCode, accountDetail.ReportingAccountCode);
            }
        }

        TreeNode FCurrentNode = null;

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != e.Node))
            {
                FMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();
                GetDetailsFromControls((AAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, FCurrentNode.Tag.ToString() }));

                // this only works for new rows; old rows have the primary key fields readonly
                if (FCurrentNode.Tag.ToString() != txtDetailAccountCode.Text)
                {
                    // account has been renamed
                    string oldName = FCurrentNode.Tag.ToString();
                    FCurrentNode.Tag = txtDetailAccountCode.Text;
                    FCurrentNode.Text = txtDetailAccountCode.Text;

                    // there are no references to this new row yet, apart from children nodes

                    // change name in the account hierarchy
                    Int32 AccountHierarchyDetailIndex = FMainDS.AAccountHierarchyDetail.DefaultView.Find(oldName);
                    FMainDS.AAccountHierarchyDetail[AccountHierarchyDetailIndex].ReportingAccountCode = txtDetailAccountCode.Text;

                    // fix children nodes, account hierarchy
                    foreach (TreeNode childnode in FCurrentNode.Nodes)
                    {
                        AccountHierarchyDetailIndex = FMainDS.AAccountHierarchyDetail.DefaultView.Find(childnode.Tag.ToString());
                        FMainDS.AAccountHierarchyDetail[AccountHierarchyDetailIndex].AccountCodeToReportTo = txtDetailAccountCode.Text;
                    }
                }
            }

            FCurrentNode = e.Node;

            // update detail panel
            ShowDetails((AAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, e.Node.Tag.ToString() }));
        }

        private void AddNewAccount(Object sender, EventArgs e)
        {
            if (FCurrentNode == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new account after selecting a parent account"));
                return;
            }

            trvAccounts.BeginUpdate();
            string newName = "NewAccount";
            Int32 countNewAccount = 0;

            if (FCurrentNode.Nodes.ContainsKey(newName))
            {
                while (FCurrentNode.Nodes.ContainsKey(newName + countNewAccount.ToString()))
                {
                    countNewAccount++;
                }

                newName += countNewAccount.ToString();
            }

            TreeNode newNode = FCurrentNode.Nodes.Add(newName);
            newNode.Tag = newName;
            trvAccounts.EndUpdate();

            AAccountRow parentAccount = (AAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber, FCurrentNode.Tag.ToString() });

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

            if (FCurrentNode.Nodes.Count == 1)
            {
                // change posting/summary flag of parent account if it was a leaf
                parentAccount.PostingStatus = false;
                hierarchyDetailRow.ReportOrder = 0;
            }
            else
            {
                FMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();
                Int32 PreviousSiblingAccountIndex = FMainDS.AAccountHierarchyDetail.DefaultView.Find(
                    FCurrentNode.Nodes[FCurrentNode.Nodes.Count - 2].Tag.ToString());
                AAccountHierarchyDetailRow siblingRow =
                    (AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.DefaultView[PreviousSiblingAccountIndex].Row;
                hierarchyDetailRow.ReportOrder = siblingRow.ReportOrder + 1;
            }

            FMainDS.AAccountHierarchyDetail.Rows.Add(hierarchyDetailRow);

            trvAccounts.SelectedNode = newNode;
        }

        private void GetDataFromControlsManual()
        {
            // TODO: report to (drag/drop)
            // TODO: report order (drag/drop)
            // TODO: posting/summary (new/delete)
        }
    }
}