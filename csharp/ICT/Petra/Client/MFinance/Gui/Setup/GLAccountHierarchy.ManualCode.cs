//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, wolfgangu
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
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.App.Core;
using System.Drawing;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLAccountHierarchy
    {
        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TreeNode FSelectedNode = null;
        private String FStatus = "";

        private Int32 FLedgerNumber;
        private string FSelectedHierarchy = "STANDARD";

        // The routine ChangeAccountCodeValue() needs the old value of
        // txtDetailAccountCode and the new actual value.
        // This string is used to store the old value.
        private string strOldDetailAccountCode;

        // Pointer to the acual selected TreeViewNode
        TreeNode FCurrentNode = null;

        // TreeView Select will be split into a part Before Select and a part
        // after select. Those parameters are for common use
        GLSetupTDSAAccountRow FSelectedAccountRow;

        private class AccountNodeDetails
        {
            /// <summary>
            /// This will be true for Summary accounts, initially Unknown for existing posting accounts.
            /// On newly created accounts, this will be true.
            /// On a "need to know" basis, it will be set false for posting accounts that already have transactions posted to them.
            /// </summary>
            ///
            public Boolean? CanHaveChildren;

            /// <summary>
            /// This will be initially false for Summary accounts that have children, unknown for existing posting accounts.
            /// On newly created accounts, this will be true.
            /// On a "need to know" basis, it will be set false for posting accounts that already have transactions posted to them.
            /// </summary>
            public Boolean? CanDelete;
            public Boolean IsNew;

            public AAccountRow AccountRow;
            public AAccountHierarchyDetailRow DetailRow;
        };

        private void InitializeManualCode()
        {
            txtDetailEngAccountCodeLongDesc.LostFocus += new EventHandler(AutoFillDescriptions);
        }

        //
        // Drag and drop methods
        // (Mostly copied from Microsoft example code) :
        //

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            FDragNode = (TreeNode)e.Item;
            trvAccounts.DoDragDrop(FDragNode, DragDropEffects.All);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADestinationNode"></param>
        /// <param name="ADragNode"></param>
        /// <returns>true if  the destination Node is a child of the node I'm dragging</returns>
        private bool IsDescendantOf(TreeNode ADestinationNode, TreeNode ADragNode)
        {
            if (ADestinationNode.Parent == null)
            {
                return false;
            }

            if (ADestinationNode.Parent == ADragNode)
            {
                return true;
            }

            return IsDescendantOf(ADestinationNode.Parent, ADragNode);
        }

        private void ShowNodeSelected(TreeNode ASelThis)
        {
            if (FSelectedNode != null)
            {
                FSelectedNode.BackColor = Color.White;
            }

            if (ASelThis != null)
            {
                ASelThis.BackColor = Color.Turquoise;
            }

            FSelectedNode = ASelThis;
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            Point pt = trvAccounts.PointToClient(new Point(e.X, e.Y));

            FDragTarget = trvAccounts.GetNodeAt(pt);

            if (FDragTarget == null)
            {
                return;
            }

            ScrollIntoView(FDragTarget);

            // Is the referenced node a valid drop target?
            bool CantDropHere = (FDragTarget == FDragNode) || IsDescendantOf(FDragTarget, FDragNode);

            if (!CantDropHere)
            {
                AccountNodeDetails NodeDetails = (AccountNodeDetails)FDragTarget.Tag;
                GetAccountCodeAttributes(ref NodeDetails);

                if (!NodeDetails.CanHaveChildren.Value)
                {
                    CantDropHere = true;
                }
            }

            if (CantDropHere)
            {
                e.Effect = DragDropEffects.Scroll;
                FDragTarget = null;
            }
            else
            {
                e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
                ShowNodeSelected(FDragTarget);
            }
        }

        private void ScrollIntoView(TreeNode ATarget)
        {
            TreeNode HigherNode = ATarget.PrevVisibleNode;

            if (HigherNode != null)
            {
                if (!HigherNode.IsVisible)
                {
                    HigherNode.EnsureVisible();
                }
            }

            TreeNode LowerNode = ATarget.NextVisibleNode;

            if (LowerNode != null)
            {
                if (!LowerNode.IsVisible)
                {
                    LowerNode.EnsureVisible();
                }
            }
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            DoReassignment(FDragNode, FDragTarget);
            FDragNode = null;
        }

        private void InsertAlphabetically(TreeNode Parent, TreeNode Child)
        {
            int Idx;

            for (Idx = 0; Idx < Parent.Nodes.Count; Idx++)
            {
                if (Parent.Nodes[Idx].Text.CompareTo(Child.Text) > 0)
                {
                    break;
                }
            }

            if (Idx == Parent.Nodes.Count)
            {
                Parent.Nodes.Add(Child);
            }
            else
            {
                Parent.Nodes.Insert(Idx, Child);
            }
        }

        // End of (mostly copied) drag-drop functions


        /// <summary>
        /// Make this account of child of the selected one in the hierarchy (from drag-drop).
        /// </summary>
        /// <param name="AChild"></param>
        /// <param name="ANewParent"></param>
        private void DoReassignment(TreeNode AChild, TreeNode ANewParent)
        {
            if ((AChild != null) && (ANewParent != null))
            {
                String PrevParent = AChild.Parent.Text;
                String NewParentAccountCode = ((AccountNodeDetails)ANewParent.Tag).AccountRow.AccountCode;
                TreeNode NewNode = (TreeNode)AChild.Clone();
                ((AccountNodeDetails)NewNode.Tag).DetailRow.AccountCodeToReportTo = NewParentAccountCode;
                InsertAlphabetically(ANewParent, NewNode);
                NewNode.Expand();
                ANewParent.Expand();
                ANewParent.BackColor = Color.White;
                FStatus += String.Format(Catalog.GetString("{0} was moved from {1} to {2}.\r\n"),
                    AChild.Text, PrevParent, ANewParent.Text);
                txtStatus.Text = FStatus;

                //Remove Original Node
                AChild.Remove();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void RunOnceOnActivationManual()
        {
            trvAccounts.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeView_ItemDrag);
            trvAccounts.DragOver += new System.Windows.Forms.DragEventHandler(treeView_DragOver);
            trvAccounts.DragDrop += new System.Windows.Forms.DragEventHandler(treeView_DragDrop);
            trvAccounts.AllowDrop = true;
        }

        private void AutoFillDescriptions(object sender, EventArgs e)
        {
            String NewText = txtDetailEngAccountCodeLongDesc.Text;

            if (txtDetailEngAccountCodeShortDesc.Text == "")
            {
                txtDetailEngAccountCodeShortDesc.Text = NewText;
            }

            if (txtDetailAccountCodeLongDesc.Text == "")
            {
                txtDetailAccountCodeLongDesc.Text = NewText;
            }

            if (txtDetailAccountCodeShortDesc.Text == "")
            {
                txtDetailAccountCodeShortDesc.Text = NewText;
            }
        }

        /// <summary>
        /// Setup the account hierarchy of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                ucoAccountAnalysisAttributes.LedgerNumber = FLedgerNumber;
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));
                PopulateTreeView();
            }
        }

        /// <summary>
        /// load account hierarchy from the dataset into the tree view
        /// </summary>
        private void PopulateTreeView()
        {
            FCurrentNode = null;

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

            this.trvAccounts.AfterSelect +=
                new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            this.trvAccounts.BeforeSelect +=
                new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeSelect);
        }

        private void InsertNodeIntoTreeView(TreeNodeCollection AParentNodes, AAccountHierarchyDetailRow ADetailRow)
        {
            AAccountRow AccountRow = (AAccountRow)FMainDS.AAccount.Rows.Find(
                new object[] { FLedgerNumber, ADetailRow.ReportingAccountCode });

            string nodeLabel = ADetailRow.ReportingAccountCode;

            if (!AccountRow.IsAccountCodeShortDescNull())
            {
                nodeLabel += " (" + AccountRow.AccountCodeShortDesc + ")";
            }

            TreeNode newNode = AParentNodes.Add(nodeLabel);

            AccountNodeDetails NodeTag = new AccountNodeDetails();

            if (AccountRow.PostingStatus) // A "Posting account" that's not been used may yet be promoted to a "Summary account".
            {
                NodeTag.CanHaveChildren = null;
            }
            else      // A "Summary account" can have children.
            {
                NodeTag.CanHaveChildren = true;
            }

            NodeTag.IsNew = false;
            NodeTag.AccountRow = AccountRow;
            NodeTag.DetailRow = ADetailRow;
            newNode.Tag = NodeTag;

            newNode.Name = nodeLabel;

            // Now add the children of this node:
            DataView view = new DataView(FMainDS.AAccountHierarchyDetail);
            view.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName() + ", " + AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();
            view.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + ADetailRow.AccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + ADetailRow.ReportingAccountCode + "'";

            if (view.Count > 0)
            {
                // A "Summary account" cannot be deleted if it has children.
                NodeTag.CanDelete = false;

                foreach (DataRowView rowView in view)
                {
                    AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                    InsertNodeIntoTreeView(newNode.Nodes, accountDetail);
                }
            }
        }

        private void TreeViewBeforeSelect(object sender, TreeViewCancelEventArgs treeViewCancelEventArgs)
        {
            // System.Console.WriteLine("TreeViewBeforeSelect:" + treeViewCancelEventArgs.Node.Text);
            try
            {
                // store current detail values
                if ((FCurrentNode != null) && (FCurrentNode != treeViewCancelEventArgs.Node))
                {
                    AccountNodeDetails NodeDetails = (AccountNodeDetails)FCurrentNode.Tag;
                    String CurrentReportingAccountCode = NodeDetails.DetailRow.ReportingAccountCode;

                    FSelectedAccountRow = (GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(
                        new object[] { FLedgerNumber, CurrentReportingAccountCode });
                    GetDetailsFromControls(FSelectedAccountRow);
                }
            }
            catch (System.Data.ConstraintException)
            {
                treeViewCancelEventArgs.Cancel = true;
                // System.Console.WriteLine("TreeViewSelect is Canceled");
            }
        }

        private void GetAccountCodeAttributes(ref AccountNodeDetails ANodeDetails)
        {
            if (!ANodeDetails.CanHaveChildren.HasValue || !ANodeDetails.CanDelete.HasValue)
            {
                bool RemoteCanBeParent = false;
                bool RemoteCanDelete = false;

                if (TRemote.MFinance.Setup.WebConnectors.GetAccountCodeAttributes(FLedgerNumber, ANodeDetails.DetailRow.ReportingAccountCode,
                        out RemoteCanBeParent, out RemoteCanDelete))
                {
                    ANodeDetails.CanHaveChildren = RemoteCanBeParent;
                    ANodeDetails.CanDelete = RemoteCanDelete;
                }
            }
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            // System.Console.WriteLine("TreeViewAfterSelect: " + treeViewEventArgs.Node.Text);
            bool hasChanges = FPetraUtilsObject.HasChanges;

            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != treeViewEventArgs.Node))
            {
                string nodeLabel = FSelectedAccountRow.AccountCode;

                if (!FSelectedAccountRow.IsAccountCodeShortDescNull())
                {
                    nodeLabel += " (" + FSelectedAccountRow.AccountCodeShortDesc + ")";
                }

                FCurrentNode.Text = nodeLabel;
                FCurrentNode.Name = nodeLabel;
            }

            FCurrentNode = treeViewEventArgs.Node;
            FPetraUtilsObject.SuppressChangeDetection = true;

            AccountNodeDetails NodeDetails = (AccountNodeDetails)FCurrentNode.Tag;
            // update detail panel
            ShowDetails((GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(new object[] {
                        FLedgerNumber,
                        NodeDetails.DetailRow.ReportingAccountCode
                    }));

            GetAccountCodeAttributes(ref NodeDetails);
            tbbAddNewAccount.Enabled = (NodeDetails.CanHaveChildren.HasValue ? NodeDetails.CanHaveChildren.Value : false);
            tbbDeleteUnusedAccount.Enabled = (NodeDetails.CanDelete.HasValue ? NodeDetails.CanDelete.Value : false);
            FPetraUtilsObject.SuppressChangeDetection = false;

            if (hasChanges)
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void ValidateDataDetailsManual(GLSetupTDSAAccountRow ARow)
        {
        }

        private void ValidateDataManual(GLSetupTDSAAccountRow ARow)
        {
        }

        private void ShowDetailsManual(GLSetupTDSAAccountRow ARow)
        {
            if (ARow == null)
            {
                txtDetailAccountCode.Text = "";
                txtDetailAccountCodeLongDesc.Text = "";
                txtDetailAccountCodeShortDesc.Text = "";
                txtDetailEngAccountCodeLongDesc.Text = "";
                txtDetailEngAccountCodeShortDesc.Text = "";
            }
            else
            {
                strOldDetailAccountCode = txtDetailAccountCode.Text;
                ucoAccountAnalysisAttributes.Enabled = ARow.PostingStatus;
                ucoAccountAnalysisAttributes.AccountCode = ARow.AccountCode;

                chkDetailForeignCurrencyFlag.Enabled = ARow.PostingStatus;

                // I may actually allow the user to change the primary key!
                // But only if the selected record is new, or they have not made any other changes.
                bool ICanEditAccountCode = ((AccountNodeDetails)FCurrentNode.Tag).IsNew || !FPetraUtilsObject.HasChanges;
                SetPrimaryKeyReadOnly(!ICanEditAccountCode);
            }
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

            // ChangeAccountCodeValue() needs this value!
            strOldDetailAccountCode = newName;

            AAccountRow parentAccount =
                (AAccountRow)FMainDS.AAccount.Rows.Find(new object[] { FLedgerNumber,
                                                                       ((AccountNodeDetails)FCurrentNode.Tag).DetailRow.ReportingAccountCode });

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
                AAccountHierarchyDetailRow siblingRow = ((AccountNodeDetails)FCurrentNode.Nodes[FCurrentNode.Nodes.Count - 1].Tag).DetailRow;

                if (siblingRow.IsReportOrderNull())
                {
                    siblingRow.ReportOrder = 1;
                }

                hierarchyDetailRow.ReportOrder = siblingRow.ReportOrder + 1;
            }

            FMainDS.AAccountHierarchyDetail.Rows.Add(hierarchyDetailRow);

            trvAccounts.BeginUpdate();
            TreeNode newNode = FCurrentNode.Nodes.Add(newName);

            AccountNodeDetails NodeDetails = new AccountNodeDetails();
            NodeDetails.CanHaveChildren = true;
            NodeDetails.IsNew = true;
            NodeDetails.DetailRow = hierarchyDetailRow;
            NodeDetails.AccountRow = newAccount;
            newNode.Tag = NodeDetails;

            trvAccounts.EndUpdate();

            trvAccounts.SelectedNode = newNode;

            FPetraUtilsObject.SetChangedFlag();
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
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));
                PopulateTreeView();

                MessageBox.Show("Import of new Account Hierarchy has been successful",
                    Catalog.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            //
            // I need to remove any AnalysisAttribute records that are still set to "Unassigned"
            //
            for (int Idx = ASubmitDS.AAnalysisAttribute.Rows.Count - 1; Idx >= 0; Idx--)
            {
                AAnalysisAttributeRow Row = ASubmitDS.AAnalysisAttribute[Idx];

                if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisTypeCode.IndexOf("Unassigned") == 0))
                {
                    Row.Delete();
                }
            }

            //
            // I'll take this opportunity to remove any similar records in my own TDS
            //
            for (int Idx = FMainDS.AAnalysisAttribute.Rows.Count - 1; Idx >= 0; Idx--)
            {
                AAnalysisAttributeRow Row = FMainDS.AAnalysisAttribute[Idx];

                if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisTypeCode.IndexOf("Unassigned") == 0))
                {
                    Row.Delete();
                }
            }

            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitDS, out AVerificationResult);
        }

        private void DeleteUnusedAccount(Object sender, EventArgs ev)
        {
            AccountNodeDetails NodeDetails = (AccountNodeDetails)FCurrentNode.Tag;
            string AccountCode = NodeDetails.DetailRow.ReportingAccountCode;

            if (!NodeDetails.CanDelete.HasValue)
            {
                MessageBox.Show("Fault: CanDelete status is unknown.");
                return;
            }

            if (!NodeDetails.CanDelete.Value)
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
                FPetraUtilsObject.SetChangedFlag();

                // if parent of deleted node has no children, mark as posting account
                // TODO: this also works only if there is just one account hierarchy
                if (trvAccounts.SelectedNode.Nodes.Count == 0)
                {
                    NodeDetails = (AccountNodeDetails)trvAccounts.SelectedNode.Tag;
                    AAccountRow AccountParent = NodeDetails.AccountRow;
                    AccountParent.PostingStatus = true;
                    NodeDetails.CanDelete = (trvAccounts.SelectedNode.Nodes.Count == 0); // This is likely to work, since the parent was previously a summary account.
                }
            }
        }

        /// <summary>
        /// This routine is the manual part of FileSave()
        /// </summary>
        private void GetDataFromControlsManual()
        {
            // TODO: report to (drag/drop)
            // TODO: report order (drag/drop)
            // TODO: posting/summary (new/delete)

            // If txtDetailAccountCode is not readonly it may have been changed.
            // Here it shall be tested ...
            if (!txtDetailAccountCode.ReadOnly)
            {
                ChangeAccountCodeValue();
            }

            if (FCurrentNode != null)
            {
                GetDetailsFromControls(GetSelectedDetailRowManual());
            }
        }

        private GLSetupTDSAAccountRow GetSelectedDetailRowManual()
        {
            if (FCurrentNode == null)
            {
                return null;
            }

            return (GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(
                new object[] { FLedgerNumber,
                               ((AccountNodeDetails)FCurrentNode.Tag).DetailRow.ReportingAccountCode });
        }

        /// <summary>
        /// Event that invokes ChangeAccountCodeValue().
        /// For details see the description of ChangeAccountCodeValue()
        /// </summary>
        /// <param name="sender">Actually it is only txtDetailAccountCode</param>
        /// <param name="e">Actually it is only the Leave-Event</param>
        public void ChangeAccountCodeValue(object sender, EventArgs e)
        {
            try
            {
                ChangeAccountCodeValue();
            }
            catch (CancelSaveException)
            {
            }
            ;
        }

        /// <summary>
        /// ChangeAccountCodeValue shall be invoked if txtDetailAccountCode has been changed and
        /// the field is left. This is normally done by the
        /// ChangeAccountCodeValue(object sender, EventArgs e).
        ///
        /// But if the user invokes an other event - i.e. FileSave the FileSave-Event runs first.
        /// </summary>

        public bool ChangeAccountCodeValue()
        {
            String strNewDetailAccountCode = txtDetailAccountCode.Text;
            bool changeAccepted = false;

            if (!strNewDetailAccountCode.Equals(strOldDetailAccountCode))
            {
                FStatus += "AccountCode changed.\r\n";
                txtStatus.Text = FStatus;

                if (MessageBox.Show(String.Format(Catalog.GetString("You have changed {0} to {1}. Confirm that you want to re-name this account."),
                            strOldDetailAccountCode,
                            strNewDetailAccountCode), Catalog.GetString("Rename Account"), MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    txtDetailAccountCode.Text = strOldDetailAccountCode;
                    return false;
                }

                AccountNodeDetails NodeDetails = (AccountNodeDetails)trvAccounts.SelectedNode.Tag;

                try
                {
                    NodeDetails.AccountRow.AccountCode = strNewDetailAccountCode;
                    NodeDetails.DetailRow.ReportingAccountCode = strNewDetailAccountCode;

                    trvAccounts.BeginUpdate();
                    trvAccounts.SelectedNode.Text = strNewDetailAccountCode;
                    trvAccounts.SelectedNode.Name = strNewDetailAccountCode;
                    trvAccounts.EndUpdate();

                    changeAccepted = true;
                }
                catch (System.Data.ConstraintException)
                {
                    MessageBox.Show(
                        Catalog.GetString("Sorry but this account already exists: ") + strNewDetailAccountCode +
                        "\r\n" + Catalog.GetString("You cannot use an account name twice!"),
                        Catalog.GetString("Rename Account"));
                    throw new CancelSaveException();
                }

                if (NodeDetails.IsNew)
                {
                    // This is the code for changes in "un-committed" nodes:
                    // there are no references to this new row yet, apart from children nodes, so I can just change them here and carry on!

                    // fixup children nodes
                    foreach (TreeNode childnode in trvAccounts.SelectedNode.Nodes)
                    {
                        ((AccountNodeDetails)childnode.Tag).DetailRow.AccountCodeToReportTo = strNewDetailAccountCode;
                    }

                    strOldDetailAccountCode = strNewDetailAccountCode;
                    FPetraUtilsObject.HasChanges = true;
                }
                else
                {
                    FStatus += Catalog.GetString("Updating AccountCode change - please wait.\r\n");
                    txtStatus.Text = FStatus;
                    TVerificationResultCollection VerificationResults;

                    // If this code was previously in the DB, I need to assume that there may be transactions posted to it.
                    // There's a server call I need to use, and after the call I need to re-load this page.
                    // (No other changes will be lost, because the txtDetailAccountCode will have been ReadOnly if there were already changes.)
                    bool Success = TRemote.MFinance.Setup.WebConnectors.RenameAccountCode(strOldDetailAccountCode,
                        strNewDetailAccountCode,
                        FLedgerNumber,
                        out VerificationResults);

                    if (Success)
                    {
                        FMainDS.Clear();
                        FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));
                        strOldDetailAccountCode = "";
                        FPetraUtilsObject.HasChanges = false;
                        PopulateTreeView();
                        ShowDetailsManual(null);
                        FStatus = "";
                        txtStatus.Text = FStatus;
                        FPetraUtilsObject.HasChanges = false;
                        FPetraUtilsObject.DisableSaveButton();
                    }
                    else
                    {
                        MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Rename Account"));
                    }
                }
            } // if changed

            return changeAccepted;
        }
    }
}