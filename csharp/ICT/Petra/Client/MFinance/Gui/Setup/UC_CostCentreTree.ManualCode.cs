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
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_CostCentreTree
    {
        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TFrmGLCostCentreHierarchy FParentForm = null;
        private TreeNode HighlightedDropTarget = null;
        // The CostCentre selected in the parent form
        CostCentreNodeDetails FSelectedCostCentre;
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
        public void RefreshSelectedCostCentre()
        {
            if (FSelectedCostCentre != null)
            {
                //
                // The Focus() line below triggers the txtDetailCostCentreCode.Leave handler.
                // which looks at the txtDetailCostCentreCode.Text value too soon - potentially before it's been set!

                trvCostCentres.Focus(); // {NET Bug!} The control doesn't show the selection if it's not visible and in focus.
                trvCostCentres.SelectedNode = FSelectedCostCentre.linkedTreeNode;
            }
        }

        /// <summary>
        /// The CostCentre may have been selected in the list view, and copied here.
        /// </summary>
        public CostCentreNodeDetails SelectedCostCentre
        {
            set
            {
                FSelectedCostCentre = value;
                RefreshSelectedCostCentre();
            }
        }

        //
        // Drag and drop methods
        // (Mostly copied from Microsoft example code) :
        //

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            FDragNode = (TreeNode)e.Item;
            trvCostCentres.DoDragDrop(FDragNode, DragDropEffects.All);
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
            Point pt = trvCostCentres.PointToClient(new Point(e.X, e.Y));

            FDragTarget = trvCostCentres.GetNodeAt(pt);

            if (FDragTarget == null)
            {
                return;
            }

            ScrollIntoView(FDragTarget);

            // Is the referenced node a valid drop target?
            bool CantDropHere = (FDragTarget == FDragNode) || IsDescendantOf(FDragTarget, FDragNode);

            if (!CantDropHere)
            {
                CostCentreNodeDetails NodeDetails = (CostCentreNodeDetails)FDragTarget.Tag;
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
            CostCentreNodeDetails ChildTag = (CostCentreNodeDetails)Child.Tag;

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

            if (!FDuringInitialisation)
            {
                FParentForm.SetSelectedCostCentre(ChildTag);
            }
        }

        /// <summary>
        /// Make this CostCentre a child of the selected one in the hierarchy (from drag-drop).
        /// </summary>
        /// <param name="AChild"></param>
        /// <param name="ANewParent"></param>
        private void DoReassignment(TreeNode AChild, TreeNode ANewParent)
        {
            if ((AChild != null) && (ANewParent != null))
            {
                CostCentreNodeDetails DraggedCostCentre = (CostCentreNodeDetails)AChild.Tag;

                if (DraggedCostCentre.CostCentreRow.SystemCostCentreFlag)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("{0} is a System Cost Centre and cannot be moved."),
                            ((CostCentreNodeDetails)AChild.Tag).CostCentreRow.CostCentreCode),
                        Catalog.GetString("Re-assign Cost Centre"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ShowNodeSelected(null);
                    return;
                }

                String PrevParent = AChild.Parent.Text;
                String NewParentCostCentreCode = ((CostCentreNodeDetails)ANewParent.Tag).CostCentreRow.CostCentreCode;
                TreeNode NewNode = (TreeNode)AChild.Clone();
                DraggedCostCentre.CostCentreRow.CostCentreToReportTo = NewParentCostCentreCode;
                DraggedCostCentre.linkedTreeNode = NewNode;
                InsertInOrder(ANewParent, NewNode);
                NewNode.Expand();
                ANewParent.Expand();
                ((CostCentreNodeDetails)ANewParent.Tag).CostCentreRow.PostingCostCentreFlag = false; // The parent is now a summary CostCentre!
                ANewParent.BackColor = Color.White;
                FParentForm.ShowStatus(String.Format(Catalog.GetString("{0} was moved from {1} to {2}."),
                        AChild.Text, PrevParent, ANewParent.Text));
                //Remove Original Node
                AChild.Remove();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        /// <summary>
        /// Add this new CostCentre as child of the currently selected node
        /// </summary>
        public void AddNewCostCentre(ACostCentreRow CostCentreRow)
        {
            trvCostCentres.BeginUpdate();
            TreeNode newNode = trvCostCentres.SelectedNode.Nodes.Add(CostCentreRow.CostCentreCode);
            CostCentreNodeDetails NewCostCentre = CostCentreNodeDetails.AddNewCostCentre(newNode, CostCentreRow);
            trvCostCentres.EndUpdate();
            FParentForm.SetSelectedCostCentre(NewCostCentre); // This will set my FSelectedCostCentre and my trvCostCentres.SelectedNode
        }

        /// <summary>
        /// Load CostCentre hierarchy from the dataset into the tree view
        /// </summary>
        public void PopulateTreeView(GLSetupTDS MainDS)
        {
            FDuringInitialisation = true;
            trvCostCentres.BeginUpdate();
            trvCostCentres.Nodes.Clear();

            // find the root cost centre
            MainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " IS NULL";

            DataView view = new DataView(MainDS.ACostCentre);
            view.Sort = ACostCentreTable.GetCostCentreCodeDBName();

            InsertNodeIntoTreeView(null,
                view,
                (ACostCentreRow)MainDS.ACostCentre.DefaultView[0].Row);

            MainDS.ACostCentre.DefaultView.RowFilter = "";

            trvCostCentres.EndUpdate();

            this.trvCostCentres.BeforeSelect += TreeViewBeforeSelect;
            this.trvCostCentres.AfterSelect += TreeViewAfterSelect;
            trvCostCentres.EndUpdate();

            FDuringInitialisation = false;

            if (trvCostCentres.Nodes.Count > 0)
            {
                SelectNodeByName(trvCostCentres.Nodes[0].Name); // Select the first item
            }
        }

        private void InsertNodeIntoTreeView(TreeNode AParent, DataView view, ACostCentreRow ADetailRow)
        {
            TreeNode newNode = new TreeNode("");

            CostCentreNodeDetails NewNodeDetails = CostCentreNodeDetails.AddNewCostCentre(newNode, ADetailRow);

            NewNodeDetails.IsNew = false;

            SetNodeLabel(ADetailRow, newNode);

            if (AParent == null)
            {
                trvCostCentres.Nodes.Add(newNode);
            }
            else
            {
                InsertInOrder(AParent, newNode);
            }

            view.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " = '" + ADetailRow.CostCentreCode + "'";

            foreach (DataRowView rowView in view)
            {
                InsertNodeIntoTreeView(newNode, view, (ACostCentreRow)rowView.Row);
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
                if (trvCostCentres == null)
                {
                    return;
                }

                ThisNode = FSelectedCostCentre.linkedTreeNode;
            }

            trvCostCentres.BeginUpdate();
            ThisNode.Text = Label;
            ThisNode.Name = name;
            trvCostCentres.EndUpdate();
        }

        /// <summary>
        /// Update the name of the currently selected node
        /// </summary>
        public void SetNodeLabel(ACostCentreRow ARow, TreeNode ThisNode = null)
        {
            if ((ARow == null) || (ARow.RowState == DataRowState.Deleted) || (ARow.RowState == DataRowState.Detached))
            {
                return;
            }

            SetNodeLabel(ARow.CostCentreCode, ARow.CostCentreName, ThisNode);
        }

        private void TreeViewBeforeSelect(object sender, TreeViewCancelEventArgs treeViewCancelEventArgs)
        {
            // System.Console.WriteLine("TreeViewBeforeSelect:" + treeViewCancelEventArgs.Node.Text);
            try
            {
                if ((FSelectedCostCentre != null) && (FSelectedCostCentre.linkedTreeNode != treeViewCancelEventArgs.Node))
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
                // System.Console.WriteLine("TreeViewSelect is Canceled");
            }
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs treeViewEventArgs)
        {
            // System.Console.WriteLine("TreeViewAfterSelect: " + treeViewEventArgs.Node.Text);

            // store current detail values
            if ((FSelectedCostCentre != null) && (FSelectedCostCentre.linkedTreeNode != treeViewEventArgs.Node))
            {
                SetNodeLabel(FSelectedCostCentre.CostCentreRow);
            }

            FParentForm.SetSelectedCostCentre((CostCentreNodeDetails)treeViewEventArgs.Node.Tag); // This will change my FSelectedCostCentre
            FPetraUtilsObject.SuppressChangeDetection = true;

            FSelectedCostCentre.GetAttrributes();
            FParentForm.PopulateControlsAfterRowSelection();
        }

        /// <summary>
        /// Uses the TreeNode Name attribute to find this CostCentre
        /// </summary>
        public void SelectNodeByName(String CostCentreCode)
        {
            TreeNode[] Matches = trvCostCentres.Nodes.Find(CostCentreCode, true); // looks for a match with this Name property.

            if (Matches.Length > 0)
            {
                FParentForm.SetSelectedCostCentre((CostCentreNodeDetails)Matches[0].Tag);
            }
        }

        /// <summary>
        /// Remove node from tree. The actual CostCentreNodeDetails object is being deleted by the parent form.
        /// </summary>
        public void DeleteSelectedCostCentre()
        {
            // select parent node first
            TreeNode NodeToBeDeleted = FSelectedCostCentre.linkedTreeNode;

            FParentForm.SetSelectedCostCentre((CostCentreNodeDetails)NodeToBeDeleted.Parent.Tag); // This will change the current FSelectedCostCentre.

            trvCostCentres.BeginUpdate();
            NodeToBeDeleted.Remove();
            trvCostCentres.EndUpdate();
        }

        /// <summary>
        /// The SelectedNode has had its name changed, and all its children need
        /// to be informed of the new name.
        /// (This can only happen when both the parent and its children are new this session; there's nothing in the Database yet.)
        /// </summary>
        public void FixupChildrenAfterCostCentreNameChange()
        {
            String NewCostCentreCode = FSelectedCostCentre.CostCentreRow.CostCentreCode;

            foreach (TreeNode childnode in FSelectedCostCentre.linkedTreeNode.Nodes)
            {
                ((CostCentreNodeDetails)childnode.Tag).CostCentreRow.CostCentreToReportTo = NewCostCentreCode;
            }
        }

        /// <summary>
        /// Called from parent form on activation
        /// </summary>
        public void RunOnceOnActivationManual(TFrmGLCostCentreHierarchy ParentForm)
        {
            FParentForm = ParentForm;
            // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
            // It is easier to do this than to configure all the different test execution methods to use STA
            try
            {
                trvCostCentres.AllowDrop = true;
                trvCostCentres.ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
                trvCostCentres.DragOver += new DragEventHandler(treeView_DragOver);
                trvCostCentres.DragDrop += new DragEventHandler(treeView_DragDrop);
            }
            catch (InvalidOperationException)
            {
                // ex.Message is: DragDrop registration did not succeed.
                // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
            }
        }
    }
}