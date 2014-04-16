//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//      timop, wolfgangu
//      Tim Ingham
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
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLAccountHierarchy
    {
        private const string INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE = "#UNASSIGNEDDETAILACCOUNTCODE#";

        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TreeNode FSelectedNode = null;
        private String FStatus = "";

        private Int32 FLedgerNumber;
        private string FSelectedHierarchy = "STANDARD";
        private bool FIAmUpdating;

        // The routine ChangeAccountCodeValue() needs the old value of
        // txtDetailAccountCode and the new actual value.
        // This string is used to store the old value.
        private string strOldDetailAccountCode;

        // Pointer to the acual selected TreeViewNode
        TreeNode FCurrentNode = null;

        // TreeView Select will be split into a part Before Select and a part
        // after select. Those parameters are for common use
        GLSetupTDSAAccountRow FSelectedAccountRow;

        private string FRecentlyUpdatedDetailAccountCode = INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE;
        private string FNameForNewAccounts;

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
            public String Msg;

            public AAccountRow AccountRow;
            public AAccountHierarchyDetailRow DetailRow;
        };

        /// <summary>Clear the Status Box</summary>
        public void ClearStatus()
        {
            FStatus = "";
            txtStatus.Text = FStatus;
            txtStatus.Refresh();
        }

        /// <summary>Add this in the Status Box</summary>
        /// <param name="NewStr"></param>
        public void ShowStatus(String NewStr)
        {
            FStatus = FStatus + "\r\n" + NewStr;
            txtStatus.Text = FStatus;
            txtStatus.Refresh();
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
                AccountNodeDetails NodeDetails = GetAccountCodeAttributes(FDragTarget);

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
            AccountNodeDetails ChildTag = (AccountNodeDetails)Child.Tag;

            if (ChildTag.AccountRow.PostingStatus)  // Posting accounts are sorted alphabetically:
            {
                for (Idx = 0; Idx < Parent.Nodes.Count; Idx++)
                {
                    if (Parent.Nodes[Idx].Text.CompareTo(Child.Text) > 0)
                    {
                        break;
                    }
                }
            }
            else // For summary accounts I need to use the ReportOrder, then alphabetic:
            {
                String ChildDescr = ChildTag.DetailRow.ReportOrder.ToString("000") + Child.Text;

                for (Idx = 0; Idx < Parent.Nodes.Count; Idx++)
                {
                    AccountNodeDetails SiblingTag = (AccountNodeDetails)Parent.Nodes[Idx].Tag;

                    if ((SiblingTag.DetailRow.ReportOrder.ToString("000") + Child.Text).CompareTo(ChildDescr) > 0)
                    {
                        break;
                    }
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
        /// Make this account a child of the selected one in the hierarchy (from drag-drop).
        /// </summary>
        /// <param name="AChild"></param>
        /// <param name="ANewParent"></param>
        private void DoReassignment(TreeNode AChild, TreeNode ANewParent)
        {
            if ((AChild != null) && (ANewParent != null))
            {
                if (((AccountNodeDetails)AChild.Tag).AccountRow.SystemAccountFlag)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("{0} is a System Account and cannot be moved."),
                            ((AccountNodeDetails)AChild.Tag).AccountRow.AccountCode),
                        Catalog.GetString("Re-assign Account"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ShowNodeSelected(null);
                    return;
                }

                String PrevParent = AChild.Parent.Text;
                String NewParentAccountCode = ((AccountNodeDetails)ANewParent.Tag).AccountRow.AccountCode;
                TreeNode NewNode = (TreeNode)AChild.Clone();
                ((AccountNodeDetails)NewNode.Tag).DetailRow.AccountCodeToReportTo = NewParentAccountCode;
                InsertAlphabetically(ANewParent, NewNode);
                NewNode.Expand();
                ANewParent.Expand();
                ANewParent.BackColor = Color.White;
                ShowStatus (String.Format(Catalog.GetString("{0} was moved from {1} to {2}."),
                    AChild.Text, PrevParent, ANewParent.Text));

                //Remove Original Node
                AChild.Remove();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.UnhookControl(txtDetailAccountCode, false); // I don't want changes in this edit box to cause SetChangedFlag - I'll set it myself.
            FPetraUtilsObject.UnhookControl(txtStatus, false); // This control is not to be spied on!
            txtDetailAccountCode.TextChanged += new EventHandler(txtDetailAccountCode_TextChanged);
            chkDetailForeignCurrencyFlag.CheckedChanged += new EventHandler(chkDetailForeignCurrencyFlag_CheckedChanged);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(OnHierarchySaved);
            FPetraUtilsObject.ControlChanged += new TValueChangedHandler(FPetraUtilsObject_ControlChanged);
            txtDetailEngAccountCodeLongDesc.LostFocus += new EventHandler(AutoFillDescriptions);

            FIAmUpdating = false;
            FNameForNewAccounts = Catalog.GetString("NEWACCOUNT");

            // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
            // It is easier to do this than to configure all the different test execution methods to use STA
            try
            {
                trvAccounts.AllowDrop = true;
                trvAccounts.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
                trvAccounts.DragOver += new DragEventHandler(treeView_DragOver);
                trvAccounts.DragDrop += new DragEventHandler(treeView_DragDrop);
            }
            catch (InvalidOperationException)
            {
                // ex.Message is: DragDrop registration did not succeed.
                // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
            }
        }

        void FPetraUtilsObject_ControlChanged(Control Sender)
        {
            if (FCurrentNode != null)
            {
                FCurrentNode.Text = NodeLabel(txtDetailAccountCode.Text, txtDetailEngAccountCodeShortDesc.Text);
            }
        }

        void chkDetailForeignCurrencyFlag_CheckedChanged(object sender, EventArgs e)
        {
            cmbDetailForeignCurrencyCode.Enabled = chkDetailForeignCurrencyFlag.Checked;
            String CurrencyLabel = (cmbDetailForeignCurrencyCode.Enabled ? GetSelectedDetailRowManual().ForeignCurrencyCode : "");
            cmbDetailForeignCurrencyCode.SetSelectedString(CurrencyLabel, -1);
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

            FPetraUtilsObject_ControlChanged(txtDetailEngAccountCodeLongDesc);
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
                ucoAccountAnalysisAttributes.ShowStatus = ShowStatus;
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

                DataView view = new DataView(FMainDS.AAccountHierarchyDetail);
                view.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName() + ", " + AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();
                InsertNodeIntoTreeView(null,
                    view,
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

        private void InsertNodeIntoTreeView(TreeNode AParent, DataView view, AAccountHierarchyDetailRow ADetailRow)
        {
            AAccountRow AccountRow = (AAccountRow)FMainDS.AAccount.Rows.Find(
                new object[] { FLedgerNumber, ADetailRow.ReportingAccountCode });

            TreeNode Child = new TreeNode(NodeLabel(AccountRow));


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
            Child.Tag = NodeTag;

            Child.Name = Child.Text;

            if (AParent == null)
            {
                trvAccounts.Nodes.Add(Child);
            }
            else
            {
                InsertAlphabetically(AParent, Child);
            }

            // Now add the children of this node:
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
                    InsertNodeIntoTreeView(Child, view, accountDetail);
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
                    FSelectedAccountRow = (GLSetupTDSAAccountRow)NodeDetails.AccountRow;
                    GetDetailsFromControls(FSelectedAccountRow);

                    if (!ValidateAllData(true, true))
                    {
                        treeViewCancelEventArgs.Cancel = true;
                    }
                }
            }
            catch (System.Data.ConstraintException)
            {
                treeViewCancelEventArgs.Cancel = true;
                // System.Console.WriteLine("TreeViewSelect is Canceled");
            }
        }

        private AccountNodeDetails GetAccountCodeAttributes(TreeNode ANode)
        {
            AccountNodeDetails nodeDetails = (AccountNodeDetails)ANode.Tag;

            if (nodeDetails.IsNew)
            {
                nodeDetails.CanHaveChildren = true;
                nodeDetails.CanDelete = (ANode.Nodes.Count == 0);
                return nodeDetails;
            }

            if (!nodeDetails.CanHaveChildren.HasValue || !nodeDetails.CanDelete.HasValue)
            {
                bool RemoteCanBeParent = false;
                bool RemoteCanDelete = false;
                String Msg;

                if (TRemote.MFinance.Setup.WebConnectors.GetAccountCodeAttributes(FLedgerNumber, nodeDetails.DetailRow.ReportingAccountCode,
                        out RemoteCanBeParent, out RemoteCanDelete, out Msg))
                {
                    nodeDetails.CanHaveChildren = RemoteCanBeParent;
                    nodeDetails.CanDelete = RemoteCanDelete;
                    nodeDetails.Msg = Msg;
                }
            }

            return nodeDetails;
        }

        private String NodeLabel(String name, String Descr)
        {
            string Label = name;

            if (Descr != "")
            {
                Label += " (" + Descr + ")";
            }

            return Label;
        }

        private String NodeLabel(AAccountRow ARow)
        {
            if (ARow == null)
            {
                return "(not found)";
            }

            if ((ARow.RowState == DataRowState.Deleted) || (ARow.RowState == DataRowState.Detached))
            {
                return "(deleted)";
            }

            return NodeLabel(ARow.AccountCode, ARow.AccountCodeShortDesc);
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            // System.Console.WriteLine("TreeViewAfterSelect: " + treeViewEventArgs.Node.Text);
            bool hasChanges = FPetraUtilsObject.HasChanges;

            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != treeViewEventArgs.Node))
            {
                FCurrentNode.Text = NodeLabel(FSelectedAccountRow);
                FCurrentNode.Name = NodeLabel(FSelectedAccountRow);
            }

            FCurrentNode = treeViewEventArgs.Node;
            FPetraUtilsObject.SuppressChangeDetection = true;

            AccountNodeDetails NodeDetails = GetAccountCodeAttributes(FCurrentNode);
            // update detail panel
            FIAmUpdating = true;
            ShowDetails((GLSetupTDSAAccountRow)FMainDS.AAccount.Rows.Find(new object[] {
                        FLedgerNumber,
                        NodeDetails.DetailRow.ReportingAccountCode
                    }));
            FIAmUpdating = false;

            tbbAddNewAccount.Enabled = (NodeDetails.CanHaveChildren.HasValue ? NodeDetails.CanHaveChildren.Value : false);
            tbbDeleteUnusedAccount.Enabled = (NodeDetails.CanDelete.HasValue ? NodeDetails.CanDelete.Value : false);
            FPetraUtilsObject.SuppressChangeDetection = false;

            if (hasChanges)
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void SelectNodeByName(String AccountCode)
        {
            FMainDS.AAccount.DefaultView.RowFilter = String.Format("a_account_code_c='{0}'", AccountCode);

            if (FMainDS.AAccount.DefaultView.Count > 0)
            {
                AAccountRow Row = (AAccountRow)FMainDS.AAccount.DefaultView[0].Row;
                String SearchFor = NodeLabel(Row);
                TreeNode[] FoundNodes = trvAccounts.Nodes.Find(SearchFor, true);

                if (FoundNodes.Length > 0)
                {
                    FoundNodes[0].EnsureVisible();
                    trvAccounts.SelectedNode = FoundNodes[0];
                    trvAccounts.Focus();
                }
            }
        }

        private void ValidateDataDetailsManual(GLSetupTDSAAccountRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Setup.ValidateAccountDetailManual(
                this,
                ARow,
                ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
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

                chkDetailForeignCurrencyFlag.Enabled = (ARow.PostingStatus && !ARow.SystemAccountFlag);
                cmbDetailForeignCurrencyCode.Enabled = (ARow.PostingStatus && !ARow.SystemAccountFlag && ARow.ForeignCurrencyFlag);

                if (!ARow.ForeignCurrencyFlag)
                {
                    cmbDetailForeignCurrencyCode.SelectedIndex = -1;
                    ARow.ForeignCurrencyCode = "";
                }

                chkDetailAccountActiveFlag.Enabled = !ARow.SystemAccountFlag;

                // I allow the user to attempt to change the primary key,
                // but if the selected record is not new, AND they have made any other changes,
                // the txtDetailAccountCode _TextChanged method will disallow any change.
                SetPrimaryKeyReadOnly(false);
                btnRename.Visible = false;
            }
        }

        void txtDetailAccountCode_TextChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating)
            {
                return;
            }

            AccountNodeDetails nodeDetails = (AccountNodeDetails)FCurrentNode.Tag;

            if (nodeDetails.AccountRow.SystemAccountFlag)
            {
                FIAmUpdating = true;
                txtDetailAccountCode.Text = strOldDetailAccountCode;
                FIAmUpdating = false;
                MessageBox.Show(Catalog.GetString("System Account Code cannot be changed."),
                    Catalog.GetString("Rename Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (strOldDetailAccountCode.IndexOf(FNameForNewAccounts) == 0)  // This is the first time the name is being set?
            {
                FPetraUtilsObject_ControlChanged(txtDetailAccountCode);
                return;
            }

            bool ICanEditAccountCode = (nodeDetails.IsNew || !FPetraUtilsObject.HasChanges);
            btnRename.Visible = (strOldDetailAccountCode != "") && (strOldDetailAccountCode != txtDetailAccountCode.Text) && ICanEditAccountCode;

            if (!nodeDetails.IsNew && FPetraUtilsObject.HasChanges) // The user wants to change an Account code, but I can't allow it.
            {
                FIAmUpdating = true;
                txtDetailAccountCode.Text = strOldDetailAccountCode;
                FIAmUpdating = false;
                MessageBox.Show(Catalog.GetString(
                        "Account Codes cannot be changed while there are other unsaved changes.\r\nSave first, then rename the Account."),
                    Catalog.GetString("Rename Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                FPetraUtilsObject_ControlChanged(txtDetailAccountCode);
            }
        }

        private void OnHierarchySaved(System.Object sender, TDataSavedEventArgs e)
        {
            SetPrimaryKeyReadOnly(false);
        }

        private void AddNewAccount(Object sender, EventArgs e)
        {
            if (FCurrentNode == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new account after selecting a parent account"));
                return;
            }

            ValidateAllData(true, false);
            string newName = FNameForNewAccounts;
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

            AAccountRow parentAccount = ((AccountNodeDetails)FCurrentNode.Tag).AccountRow;

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
            txtDetailAccountCode.Focus();
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
            if ((ASubmitDS.AAnalysisAttribute != null) && (ASubmitDS.AAnalysisAttribute.Rows.Count > 0))
            {
                for (int Idx = ASubmitDS.AAnalysisAttribute.Rows.Count - 1; Idx >= 0; Idx--)
                {
                    AAnalysisAttributeRow Row = ASubmitDS.AAnalysisAttribute[Idx];

                    if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisTypeCode.IndexOf("Unassigned") == 0))
                    {
                        Row.Delete();
                    }
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
                MessageBox.Show(Catalog.GetString("Fault: CanDelete status is unknown."), Catalog.GetString(
                        "Delete Account"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!NodeDetails.CanDelete.Value)
            {
                MessageBox.Show(
                    String.Format(Catalog.GetString(
                            "Account {0} cannot be deleted. You can deactivate the account, but not delete it."),
                        AccountCode) +
                    "\r\n" + NodeDetails.Msg,
                    Catalog.GetString("Delete Account"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // select parent node first, so that account to be deleted will not be updated later anymore
            TreeNode NodeToBeDeleted = FCurrentNode;
            trvAccounts.SelectedNode = FCurrentNode.Parent;

            trvAccounts.BeginUpdate();
            NodeToBeDeleted.Remove();
            trvAccounts.EndUpdate();

            //
            // If this account has analysis Attributes,
            // I need to remove them.

            if (FMainDS.AAnalysisAttribute != null)
            {
                DataView DeleteThese = new DataView(FMainDS.AAnalysisAttribute);
                DeleteThese.RowFilter = String.Format("a_ledger_number_i={0} AND a_account_code_c='{1}'",
                    FLedgerNumber, AccountCode);

                foreach (DataRowView rv in DeleteThese)
                {
                    DataRow TempRow = rv.Row;
                    TempRow.Delete();
                }
            }

            AAccountHierarchyDetailRow AccountHDetailToBeDeleted = (AAccountHierarchyDetailRow)FMainDS.AAccountHierarchyDetail.Rows.Find(
                new object[] { FLedgerNumber, FSelectedHierarchy, AccountCode });
            AccountHDetailToBeDeleted.Delete();

            //
            // I can delete this account if it no longer appears in any Hieararchy.

            DataView AHD_stillInUse = new DataView(FMainDS.AAccountHierarchyDetail);
            AHD_stillInUse.RowFilter = String.Format("a_ledger_number_i={0} AND a_reporting_account_code_c='{1}'",
                FLedgerNumber, AccountCode);

            if (AHD_stillInUse.Count == 0)
            {
                AAccountRow AccountToBeDeleted = (AAccountRow)FMainDS.AAccount.Rows.Find(
                    new object[] { FLedgerNumber, AccountCode });
                AccountToBeDeleted.Delete();
            }
            else
            {
                MessageBox.Show(String.Format(
                        Catalog.GetString(
                            "The account {0} is removed from the {1} hierarchy, but not deleted, since it remains part of another heirarchy."),
                        AccountCode,
                        FSelectedHierarchy),
                    Catalog.GetString("Delete Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // If the account [parent of the deleted node] now has no accounts reporting to it (in any hierarchies), mark it as posting account.
            NodeDetails = GetAccountCodeAttributes(trvAccounts.SelectedNode);
            tbbDeleteUnusedAccount.Enabled = (NodeDetails.CanDelete.HasValue ? NodeDetails.CanDelete.Value : false);
            AAccountRow AccountParent = NodeDetails.AccountRow;
            AHD_stillInUse.RowFilter = String.Format("a_ledger_number_i={0} AND a_account_code_to_report_to_c='{1}'",
                FLedgerNumber, AccountParent.AccountCode);

            if (AHD_stillInUse.Count == 0)  // No-one now reports to this account, so I can mark it as "Posting"
            {
                AccountParent.PostingStatus = true;
                chkDetailForeignCurrencyFlag.Enabled = (!AccountParent.SystemAccountFlag);
                cmbDetailForeignCurrencyCode.Enabled = (!AccountParent.SystemAccountFlag && AccountParent.ForeignCurrencyFlag);

                // It's possible this account could now be deleted, but the user would need to save and re-load first,
                // because the server still has it down as a summary account.
            }

            FPetraUtilsObject.SetChangedFlag();
        }

        /// <summary>
        /// Called from ValidateAllData
        /// </summary>
        private void GetDataFromControlsManual()
        {
            if (FCurrentNode != null)
            {
                AutoFillDescriptions(null, null);
                GetDetailsFromControls(GetSelectedDetailRowManual());
                //
                // The auto-generated code doesn't get the details from the UC_AnalasisAttributes control,
                // so I need to do that here:
                ucoAccountAnalysisAttributes.GetDataFromControls();

                //
                // I need to ensure that the AccountHierarchyDetail row has the same AccountCode as the Account Row
                AccountNodeDetails nodeDetails = (AccountNodeDetails)FCurrentNode.Tag;
                nodeDetails.DetailRow.ReportingAccountCode = nodeDetails.AccountRow.AccountCode;
                FCurrentNode.Text = NodeLabel(GetSelectedDetailRowManual());
            }
        }

        private GLSetupTDSAAccountRow GetSelectedDetailRowManual()
        {
            if (FCurrentNode == null)
            {
                return null;
            }

            return (GLSetupTDSAAccountRow)((AccountNodeDetails)FCurrentNode.Tag).AccountRow;
        }

        /// <summary>
        /// Event that invokes ChangeAccountCodeValue().
        /// For details see the description of ChangeAccountCodeValue()
        /// </summary>
        /// <param name="sender">Actually it is only txtDetailAccountCode</param>
        /// <param name="e">Actually it is only the Leave-Event</param>
        public void ChangeAccountCodeValue(object sender, EventArgs e)
        {
            ChangeAccountCodeValue();
        }

        /// <summary>
        /// ChangeAccountCodeValue is invoked when txtDetailAccountCode is left
        /// by ChangeAccountCodeValue(object sender, EventArgs e) (from YAML).
        ///
        /// But if the user invokes an other event - i.e. FileSave the FileSave-Event runs first.
        /// </summary>
        /// <returns>True if the change is allowed</returns>

        public bool ChangeAccountCodeValue()
        {
            String strNewDetailAccountCode = txtDetailAccountCode.Text;
            String strAccountShortDescr = txtDetailEngAccountCodeShortDesc.Text;

            bool changeAccepted = false;

            if (strNewDetailAccountCode != FRecentlyUpdatedDetailAccountCode)
            {
                if (strNewDetailAccountCode != strOldDetailAccountCode)
                {
                    if (strOldDetailAccountCode.IndexOf(FNameForNewAccounts) < 0) // If they're just changing this from the initial value, don't show warning.
                    {
                        if (MessageBox.Show(String.Format(Catalog.GetString(
                                        "You have changed the Account Code from '{0}' to '{1}'.\r\n\r\n" +
                                        "Please confirm that you want to rename this account by choosing 'OK'.\r\n\r\n" +
                                        "(Renaming will take a few moments, then the form will be re-loaded.)"),
                                    strOldDetailAccountCode,
                                    strNewDetailAccountCode),
                                Catalog.GetString("Rename Account: Confirmation"), MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            txtDetailAccountCode.Text = strOldDetailAccountCode;
                            return false;
                        }
                    }

                    this.Cursor = Cursors.WaitCursor;
                    this.Refresh();  // Just to get the Wait Cursor to display...

                    FRecentlyUpdatedDetailAccountCode = strNewDetailAccountCode;
                    AccountNodeDetails NodeDetails = (AccountNodeDetails)trvAccounts.SelectedNode.Tag;

                    try
                    {
                        NodeDetails.AccountRow.BeginEdit();
                        NodeDetails.AccountRow.AccountCode = strNewDetailAccountCode;
                        NodeDetails.DetailRow.ReportingAccountCode = strNewDetailAccountCode;
                        NodeDetails.AccountRow.EndEdit();

                        trvAccounts.BeginUpdate();
                        trvAccounts.SelectedNode.Text = NodeLabel(strNewDetailAccountCode, strAccountShortDescr);
                        trvAccounts.SelectedNode.Name = NodeLabel(strNewDetailAccountCode, strAccountShortDescr);
                        trvAccounts.EndUpdate();

                        changeAccepted = true;
                    }
                    catch (System.Data.ConstraintException)
                    {
                        txtDetailAccountCode.Text = strOldDetailAccountCode;
                        NodeDetails.AccountRow.CancelEdit();
                        NodeDetails.DetailRow.CancelEdit();

                        FRecentlyUpdatedDetailAccountCode = INTERNAL_UNASSIGNED_DETAIL_ACCOUNT_CODE;

                        ShowStatus (Catalog.GetString("Account Code change REJECTED!"));

                        MessageBox.Show(String.Format(
                                Catalog.GetString(
                                    "Renaming Account Code '{0}' to '{1}' is not possible because an Account Code by the name of '{2}' already exists.\r\n\r\n--> Account Code reverted to previous value!"),
                                strOldDetailAccountCode, strNewDetailAccountCode, strNewDetailAccountCode),
                            Catalog.GetString("Renaming Not Possible - Conflicts With Existing Account Code"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtDetailAccountCode.Focus();
                    }

                    if (changeAccepted)
                    {
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
                            ucoAccountAnalysisAttributes.AccountCode = strNewDetailAccountCode;
                        }
                        else
                        {
                            ShowStatus(Catalog.GetString("Updating Account Code change - please wait."));
                            TVerificationResultCollection VerificationResults;

                            // If this code was previously in the DB, I need to assume that there may be transactions posted to it.
                            // There's a server call I need to use, and after the call I need to re-load this page.
                            // (No other changes will be lost, because the txtDetailAccountCode_TextChanged would
                            // have disallowed the change if there were already changes.)
                            bool Success = TRemote.MFinance.Setup.WebConnectors.RenameAccountCode(strOldDetailAccountCode,
                                strNewDetailAccountCode,
                                FLedgerNumber,
                                out VerificationResults);                                                           // This call takes ages..

                            if (Success)
                            {
                                FIAmUpdating = true;
                                FMainDS.Clear();
                                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadAccountHierarchies(FLedgerNumber));      // and this also takes a while!
                                strOldDetailAccountCode = "";
                                FPetraUtilsObject.SuppressChangeDetection = true;
                                PopulateTreeView();
                                ShowDetailsManual(null);
                                ClearStatus();
                                FIAmUpdating = false;
                                FPetraUtilsObject.SuppressChangeDetection = false;
                                SelectNodeByName(FRecentlyUpdatedDetailAccountCode);

                                ShowStatus(String.Format(Catalog.GetString("Account Code changed to '{0}'."), strNewDetailAccountCode));
                            }
                            else
                            {
                                MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Rename Account"));
                            }
                        }

                        btnRename.Visible = false;
                    } // if changeAccepted

                    this.Cursor = Cursors.Default;
                } // if changed

            } // if not handling the same change as before (prevents this method running several times for a single change!)

            return changeAccepted;
        }
    }
}