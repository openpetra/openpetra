// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//     Tim Ingham
//
// Copyright 2004-2014 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using GNU.Gettext;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_AccountsTree
    {
        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TFrmGLAccountHierarchy FParentForm = null;
        private TreeNode HighlightedDropTarget = null;
        // The account selected in the parent form
        private AccountNodeDetails FSelectedAccount;
        Boolean FDuringInitialisation = false;

        /// <summary>
        /// I don't want this, but the auto-generated code references it:
        /// </summary>
        public GLSetupTDS MainDS;

        /// <summary>
        /// The TreeView doesn't show a node selected unless it's visible
        /// (I assume this is a bug.)
        /// So, when it becomes visible, my parent calls here.
        /// </summary>
        public void RefreshSelectedAccount()
        {
            if (FSelectedAccount != null)
            {
                trvAccounts.SelectedNode = FSelectedAccount.linkedTreeNode;
            }
            else
            {
                trvAccounts.SelectedNode = null;
            }
        }

        /// <summary>
        /// The Account may have been selected in the list view, and copied here.
        /// </summary>
        public AccountNodeDetails SelectedAccount
        {
            set
            {
                trvAccounts.Focus(); // {NET Bug!} The control doesn't show the selection if it's not visible and in focus.
                                     // But it's also a pain doing this - it causes validation of the existing controls.

                FSelectedAccount = value;
                RefreshSelectedAccount();
            }
        }

        //
        // Drag and drop methods
        // (Mostly copied from Microsoft example code) :
        //

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (FParentForm.CheckControlsValidateOk())
            {
                FDragNode = (TreeNode)e.Item;
                trvAccounts.DoDragDrop(FDragNode, DragDropEffects.All);
            }
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
            if (HighlightedDropTarget != null)
            {
                HighlightedDropTarget.BackColor = Color.White;
            }

            if (ASelThis != null)
            {
                ASelThis.BackColor = Color.Turquoise;
            }

            HighlightedDropTarget = ASelThis;
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
                NodeDetails.GetAttrributes();

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

        // End of (mostly copied) drag-drop functions

        private void InsertInOrder(TreeNode Parent, TreeNode Child)
        {
            int Idx;
            AccountNodeDetails ChildTag = (AccountNodeDetails)Child.Tag;

/*
 * Apparently it is best to always use the Reporting Order, for both Summary and Posting accounts.
 *
 *          if (ChildTag.AccountRow.PostingStatus) // Posting accounts are sorted alphabetically:
 *          {
 *              for (Idx = 0; Idx < Parent.Nodes.Count; Idx++)
 *              {
 *                  if (Parent.Nodes[Idx].Text.CompareTo(Child.Text) > 0)
 *                  {
 *                      break;
 *                  }
 *              }
 *          }
 *          else // For summary accounts I need to use the ReportOrder, then alphabetic:
 */
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

            if (!FDuringInitialisation)
            {
                FParentForm.SetSelectedAccount(ChildTag);
            }
        }

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

                FParentForm.SetSelectedAccount(null);
                String PrevParent = AChild.Parent.Text;
                AccountNodeDetails DraggedAccount = (AccountNodeDetails)AChild.Tag;
                String NewParentAccountCode = ((AccountNodeDetails)ANewParent.Tag).AccountRow.AccountCode;

                TreeNode NewNode = (TreeNode)AChild.Clone(); // A new TreeNode is made (and the previous will be deleted),
                                                             // but the actual DataRows are only tweaked to show the new parent.

                DraggedAccount.linkedTreeNode = NewNode;
                DraggedAccount.DetailRow.AccountCodeToReportTo = NewParentAccountCode;
                InsertInOrder(ANewParent, NewNode);
                NewNode.Expand();
                ANewParent.Expand();
                ((AccountNodeDetails)ANewParent.Tag).AccountRow.PostingStatus = false; // The parent is now a summary account!
                ANewParent.BackColor = Color.White;
                FParentForm.ShowStatus(String.Format(Catalog.GetString("{0} was moved from {1} to {2}."),
                        AChild.Text, PrevParent, ANewParent.Text));

                AChild.Remove();
                FPetraUtilsObject.SetChangedFlag();
                FParentForm.SetSelectedAccount(DraggedAccount);
//              SetSelectionUsingTimer(DraggedAccount); // Calling SetSelectedAccount directly doesn't work
                // because Remove(), above, has left a selection "in the queue".
            }
        }

        /// <summary>Helps to show what the the new child reporting order should be</summary>
        public Int32 GetLastChildReportingOrder()
        {
            TreeNodeCollection SelectedNodeChildren = FSelectedAccount.linkedTreeNode.Nodes;

            if (SelectedNodeChildren.Count == 0)
            {
                return 0;
            }
            else
            {
                AAccountHierarchyDetailRow siblingRow = ((AccountNodeDetails)SelectedNodeChildren[SelectedNodeChildren.Count - 1].Tag).DetailRow;

                if (siblingRow.IsReportOrderNull())
                {
                    siblingRow.ReportOrder = 1;
                }

                return siblingRow.ReportOrder + 1;
            }
        }

        /// <summary>
        /// Add this new account as child of the currently selected node
        /// </summary>
        public void AddNewAccount(GLSetupTDSAAccountRow AccountRow, AAccountHierarchyDetailRow HierarchyDetailRow)
        {
            trvAccounts.BeginUpdate();
            TreeNode newNode = trvAccounts.SelectedNode.Nodes.Add(AccountRow.AccountCode);
            AccountNodeDetails NewAccount = AccountNodeDetails.AddNewAccount(newNode, AccountRow, HierarchyDetailRow);
            trvAccounts.EndUpdate();
            FParentForm.SetSelectedAccount(NewAccount);
        }

        /// <summary>
        /// Load account hierarchy from the dataset into the tree view
        /// </summary>
        public void PopulateTreeView(GLSetupTDS MainDS, Int32 LedgerNumber, String SelectedHierarchy)
        {
            FParentForm.SetSelectedAccount(null);
            FDuringInitialisation = true;
            trvAccounts.BeginUpdate();
            trvAccounts.Nodes.Clear();

            // TODO: select account hierarchy
            AAccountHierarchyRow accountHierarchy = (AAccountHierarchyRow)MainDS.AAccountHierarchy.Rows.Find(new object[] { LedgerNumber,
                                                                                                                            SelectedHierarchy });

            if (accountHierarchy != null)
            {
                // find the BALSHT account that is reporting to the root account
                MainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                    AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + SelectedHierarchy + "' AND " +
                    AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + accountHierarchy.RootAccountCode + "'";

                DataView view = new DataView(MainDS.AAccountHierarchyDetail);
                view.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName() + ", " + AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();
                InsertNodeIntoTreeView(MainDS, LedgerNumber, null, view,
                    (AAccountHierarchyDetailRow)MainDS.AAccountHierarchyDetail.DefaultView[0].Row);
            }

            // reset filter so that the defaultview can be used for finding accounts (eg. when adding new account)
            MainDS.AAccountHierarchyDetail.DefaultView.RowFilter = "";
            trvAccounts.EndUpdate();

            FDuringInitialisation = false;

            if (trvAccounts.Nodes.Count > 0)
            {
                SelectNodeByName(trvAccounts.Nodes[0].Name); // Select the first (BAL SHT) item
            }

            this.trvAccounts.BeforeSelect +=
                new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeSelect);
            this.trvAccounts.AfterSelect +=
                new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
        }

        private void InsertNodeIntoTreeView(GLSetupTDS MainDS,
            Int32 LedgerNumber,
            TreeNode AParent,
            DataView view,
            AAccountHierarchyDetailRow ADetailRow)
        {
            GLSetupTDSAAccountRow AccountRow = (GLSetupTDSAAccountRow)MainDS.AAccount.Rows.Find(
                new object[] { LedgerNumber, ADetailRow.ReportingAccountCode });

            TreeNode Child = new TreeNode();


            AccountNodeDetails NodeTag = AccountNodeDetails.AddNewAccount(Child, AccountRow, ADetailRow);

            NodeTag.IsNew = false;

            SetNodeLabel(AccountRow, Child);

            if (AParent == null)
            {
                trvAccounts.Nodes.Add(Child);
            }
            else
            {
                InsertInOrder(AParent, Child);
            }

            // Now add the children of this node:
            view.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + ADetailRow.AccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + ADetailRow.ReportingAccountCode + "'";

            if (view.Count > 0)
            {
                // An account cannot be deleted if it has children.
                NodeTag.CanDelete = false;
                NodeTag.Msg = Catalog.GetString("Child accounts must be deleted first.");
                NodeTag.CanHaveChildren = true;

                foreach (DataRowView rowView in view)
                {
                    AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                    InsertNodeIntoTreeView(MainDS, LedgerNumber, Child, view, accountDetail);
                }
            }
        }

        /// <summary>
        /// Update the name of the currently selected node
        /// </summary>
        public void SetNodeLabel(String name, String Descr, TreeNode ThisNode = null)
        {
            string Label = name;

            if (Descr != "")
            {
                Label += " (" + Descr + ")";
            }

            if (ThisNode == null)
            {
                if (FSelectedAccount == null)
                {
                    return;
                }

                ThisNode = FSelectedAccount.linkedTreeNode;
            }

            trvAccounts.BeginUpdate();
            ThisNode.Text = Label;
            ThisNode.Name = name;
            trvAccounts.EndUpdate();
        }

        /// <summary>
        /// Update the name of the currently selected node
        /// </summary>
        public void SetNodeLabel(AAccountRow ARow, TreeNode ThisNode = null)
        {
            if ((ARow == null) || (ARow.RowState == DataRowState.Deleted) || (ARow.RowState == DataRowState.Detached))
            {
                return;
            }

            SetNodeLabel(ARow.AccountCode, ARow.AccountCodeShortDesc, ThisNode);
        }

        private void TreeViewBeforeSelect(object sender, TreeViewCancelEventArgs treeViewCancelEventArgs)
        {
            // System.Console.WriteLine("TreeViewBeforeSelect:" + treeViewCancelEventArgs.Node.Text);
            try
            {
                if ((FSelectedAccount != null) && (FSelectedAccount.linkedTreeNode != treeViewCancelEventArgs.Node))
                {
                    // store current detail values
                    if (!FParentForm.CheckControlsValidateOk())
                    {
                        treeViewCancelEventArgs.Cancel = true;
                    }
                }
            }
            catch (System.Data.ConstraintException)
            {
                treeViewCancelEventArgs.Cancel = true;

                FParentForm.ShowStatus(Catalog.GetString("Validation error caused cancellation of selection."));
                FParentForm.SetSelectedAccount(null);
            }
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            // System.Console.WriteLine("TreeViewAfterSelect: " + treeViewEventArgs.Node.Text);

            // store current detail values
            if ((FSelectedAccount != null) && (FSelectedAccount.linkedTreeNode != treeViewEventArgs.Node))
            {
                SetNodeLabel(FSelectedAccount.AccountRow);
            }

            FParentForm.SetSelectedAccount((AccountNodeDetails)treeViewEventArgs.Node.Tag); // This will change my FSelectedAccount

            FParentForm.PopulateControlsAfterRowSelection();
        }

        /// <summary>
        /// Uses the TreeNode Name attribute to find this Account
        /// </summary>
        public void SelectNodeByName(String AccountCode)
        {
            TreeNode[] Matches = trvAccounts.Nodes.Find(AccountCode, true); // looks for a match with this Name property.

            if (Matches.Length > 0)
            {
                FParentForm.SetSelectedAccount((AccountNodeDetails)Matches[0].Tag);
            }
        }

        /// <summary>
        /// Remove node from tree. The actual AccountNodeDetails object is being deleted by the parent form.
        /// </summary>
        public void DeleteSelectedAccount()
        {
            // select parent node first
            TreeNode NodeToBeDeleted = FSelectedAccount.linkedTreeNode;

            FParentForm.SetSelectedAccount((AccountNodeDetails)NodeToBeDeleted.Parent.Tag); // This will change the current FSelectedAccount.

            trvAccounts.BeginUpdate();
            NodeToBeDeleted.Remove();
            trvAccounts.EndUpdate();
        }

        /// <summary>
        /// The SelectedNode has had its name changed, and all its children need
        /// to be informed of the new name.
        /// (This can only happen when both the parent and its children are new this session; there's nothing in the Database yet.)
        /// </summary>
        public void FixupChildrenAfterAccountNameChange()
        {
            String NewAccountCode = FSelectedAccount.AccountRow.AccountCode;

            foreach (TreeNode childnode in FSelectedAccount.linkedTreeNode.Nodes)
            {
                ((AccountNodeDetails)childnode.Tag).DetailRow.AccountCodeToReportTo = NewAccountCode;
            }
        }

        /// <summary>
        /// Called from parent form on activation
        /// </summary>
        public void RunOnceOnActivationManual(TFrmGLAccountHierarchy ParentForm)
        {
            FParentForm = ParentForm;
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

        private void MarkNodeCommitted(TreeNode ParentNode)
        {
            ((AccountNodeDetails)ParentNode.Tag).IsNew = false;

            foreach (TreeNode ChildNode in ParentNode.Nodes)
            {
                MarkNodeCommitted(ChildNode);
            }
        }

        /// <summary>
        /// After a Save operation my nodes are on the database, so I can no longer treat as 'new'.
        /// (Any further nodes added in this session will be marked as new)
        /// </summary>
        public void MarkAllNodesCommitted()
        {
            MarkNodeCommitted(trvAccounts.Nodes[0]);
        }
    }
}