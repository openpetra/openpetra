//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;
using System.Collections;
using System.Drawing;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    /// <summary>
    /// Prototype for handler of reassign event
    /// (Used by Unit tab of Partner edit)
    /// </summary>
    /// <param name="ChildKey"></param>UnitReassignHandler
    /// <param name="ParentKey"></param>
    public delegate void UnitReassignHandler (Int64 ChildKey, Int64 ParentKey);

    public partial class TFrmUnitHierarchy
    {
        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TreeNode FSelectedNode = null;
        private String FStatus = "";
        private TreeNode FChildNodeReference;
        private TreeNode FParentNodeReference;
        private List <Tuple <string, Int64, Int64>>FChangedParents = new List <Tuple <string, long, long>>();

        private UnitHierarchyNode FindNodeWithThisParent(Int64 ParentKey, ArrayList UnitNodes)
        {
            foreach (UnitHierarchyNode Node in UnitNodes)
            {
                if (Node.ParentUnitKey == ParentKey)
                {
                    return Node;
                }
            }

            return null;
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

        private TreeNode FindChild(TreeNode AParentNode, Int64 AUnitKey, Boolean AIncludeParentNodeInSearch = false)
        {
            if (AIncludeParentNodeInSearch)
            {
                if (((UnitHierarchyNode)AParentNode.Tag).MyUnitKey == AUnitKey)
                {
                    return AParentNode;
                }
            }

            foreach (TreeNode Node in AParentNode.Nodes)
            {
                if (((UnitHierarchyNode)Node.Tag).MyUnitKey == AUnitKey)
                {
                    return Node;
                }

                TreeNode ChildResult = FindChild(Node, AUnitKey, false);

                if (ChildResult != null)
                {
                    return ChildResult;
                }
            }

            return null;
        }

        private void AddChildren(TreeNode ParentNode, ArrayList UnitNodes)
        {
            while (true)
            {
                UnitHierarchyNode Child = FindNodeWithThisParent(((UnitHierarchyNode)ParentNode.Tag).MyUnitKey, UnitNodes);

                if (Child == null)
                {
                    ParentNode.Expand();
                    return;
                }

                // Add the node to the specifed parent:
                TreeNode ChildNode = new TreeNode(Child.Description);
                ChildNode.Tag = Child;
                ChildNode.ToolTipText = Child.TypeCode;

                InsertAlphabetically(ParentNode, ChildNode);
                ChildNode.Expand();

                // Remove this node from UnitNodes:
                UnitNodes.Remove(Child);
                // Add any children of this node:
                AddChildren(ChildNode, UnitNodes);
            }
        }

        //
        // Drag and drop methods
        // (Mostly copied from Microsoft example code) :
        //

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            FDragNode = (TreeNode)e.Item;
            trvUnits.DoDragDrop(FDragNode, DragDropEffects.All);
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

        private void SelectNode(TreeNode ASelThis)
        {
            ShowNodeSelected(ASelThis);

            if (ASelThis != null)
            {
                txtChild.Text = ((UnitHierarchyNode)ASelThis.Tag).MyUnitKey.ToString("D10");
                txtParent.Text = ((UnitHierarchyNode)ASelThis.Tag).ParentUnitKey.ToString("D10");
                btnMove.Enabled = false;
                EvaluateParentChange(null, null);
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            Point pt = trvUnits.PointToClient(new Point(e.X, e.Y));

            FDragTarget = trvUnits.GetNodeAt(pt);

            if (FDragTarget == null)
            {
                return;
            }

            ScrollIntoView(FDragTarget);

            if ((FDragTarget == FDragNode) || (IsDescendantOf(FDragTarget, FDragNode)))
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

        private void DoReassignment(TreeNode Child, TreeNode NewParent)
        {
            if ((Child != null) && (NewParent != null))
            {
                String PrevParent = Child.Parent.Text;
                TreeNode NewNode = (TreeNode)Child.Clone();
                ((UnitHierarchyNode)NewNode.Tag).ParentUnitKey = ((UnitHierarchyNode)NewParent.Tag).MyUnitKey;
                InsertAlphabetically(NewParent, NewNode);
                NewNode.Expand();
                NewParent.Expand();
                NewParent.BackColor = Color.White;
                FChangedParents.Add(new Tuple <string, long, long>(
                        ((UnitHierarchyNode)NewParent.Tag).Description,
                        ((UnitHierarchyNode)NewNode.Tag).MyUnitKey,
                        ((UnitHierarchyNode)NewNode.Tag).ParentUnitKey));
                FStatus += String.Format(Catalog.GetString("{0} was moved from {1} to {2}.\r\n"),
                    Child.Text, PrevParent, NewParent.Text);
                txtStatus.Text = FStatus;

                //Select the New Node in the tree view
                SelectNode(NewNode);
                trvUnits.SelectedNode = NewNode;

                //Remove Original Node
                Child.Remove();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            DoReassignment(FDragNode, FDragTarget);
            FDragNode = null;
        }

        private void UnitsClick(object sender, EventArgs e)
        {
            Point pt = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
            TreeNode ClickTarget = trvUnits.GetNodeAt(pt);

            SelectNode(ClickTarget);
        }

        private void EvaluateParentChange(object sender, EventArgs e)
        {
            bool ICanReassign = false;
            Int64 ChildKey;
            Int64 ParentKey;

            try
            {
                ChildKey = Convert.ToInt64(txtChild.Text);
                ParentKey = Convert.ToInt64(txtParent.Text);

                // search child below root organisation node
                FChildNodeReference = FindChild(trvUnits.Nodes[0], ChildKey, false);
                if (FChildNodeReference == null)
                {
                    // if not found yet then search below "Unassigned" node
                    FChildNodeReference = FindChild(trvUnits.Nodes[1], ChildKey, false);
                }

                // search parent as or below root organisation node
                FParentNodeReference = FindChild(trvUnits.Nodes[0], ParentKey, true);
                if (FParentNodeReference == null)
                {
                    // if not found yet then search below "Unassigned" node
                    FParentNodeReference = FindChild(trvUnits.Nodes[1], ParentKey, true);
                }

                if ((FChildNodeReference != null) && (FParentNodeReference != null))
                {
                    if (FChildNodeReference.Parent != FParentNodeReference)
                    {
                        ICanReassign = true;
                    }
                }
            }
            catch (Exception)
            {
            }

            if (!FPetraUtilsObject.SecurityReadOnly)
            {
                btnMove.Enabled = ICanReassign;
            }
            else
            {
                btnMove.Enabled = false;
            }
        }

        private void treeView_MouseWheel(object sender, MouseEventArgs e)
        {
            int YScroll = (e.Delta / -40);

            while (YScroll > 0)
            {
                TreeNode LastNode = trvUnits.GetNodeAt(1, trvUnits.Bottom - 2);

                if ((LastNode != null) && (LastNode.NextVisibleNode != null))
                {
                    LastNode.NextVisibleNode.EnsureVisible();
                }

                YScroll--;
            }

            while (YScroll < 0)
            {
                TreeNode FirstNode = trvUnits.GetNodeAt(1, 1);

                if ((FirstNode != null) && (FirstNode.PrevVisibleNode != null))
                {
                    FirstNode.PrevVisibleNode.EnsureVisible();
                }

                YScroll++;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AUnitKey"></param>
        /// <returns>true if this unit can be shown</returns>
        public bool ShowThisUnit(Int64 AUnitKey)
        {
            TreeNode SelectedNode = FindChild(trvUnits.Nodes[0], AUnitKey);

            if (SelectedNode != null)
            {
                trvUnits.CollapseAll();
                SelectNode(SelectedNode);
                trvUnits.SelectedNode = SelectedNode;
                SelectedNode.Expand();
                SelectedNode.EnsureVisible();
            }

            return SelectedNode != null;
        }

        private void RunOnceOnActivationManual()
        {
            // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
            // It is easier to do this than to configure all the different test execution methods to use STA
            try
            {
                trvUnits.AllowDrop = true;
                trvUnits.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeView_ItemDrag);
                trvUnits.DragOver += new System.Windows.Forms.DragEventHandler(treeView_DragOver);
                trvUnits.DragDrop += new System.Windows.Forms.DragEventHandler(treeView_DragDrop);
            }
            catch (InvalidOperationException)
            {
                // ex.Message is: DragDrop registration did not succeed.
                // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
            }

            trvUnits.Click += new EventHandler(UnitsClick);
            trvUnits.ShowNodeToolTips = true;
            trvUnits.MouseWheel += new MouseEventHandler(treeView_MouseWheel);
            trvUnits.Focus();

            txtChild.TextChanged += new EventHandler(EvaluateParentChange);
            txtParent.TextChanged += new EventHandler(EvaluateParentChange);

            FPetraUtilsObject.UnhookControl(pnlDetails, true); // I don't want changes in these values to cause SetChangedFlag.
            FPetraUtilsObject.UnhookControl(txtStatus, false);

            ArrayList UnitNodes = TRemote.MPersonnel.WebConnectors.GetUnitHeirarchy();
            //
            // The list of nodes returned by the above call are ordered to the extent that:
            //  * The root node appears first,
            //  * a parent appears before its child.
            UnitHierarchyNode RootData = (UnitHierarchyNode)UnitNodes[0];
            UnitHierarchyNode UnassignedData = (UnitHierarchyNode)UnitNodes[1];

            // build up actual root node
            TreeNode RootNode = new TreeNode(RootData.Description);
            RootNode.Tag = RootData;
            RootNode.ToolTipText = RootData.TypeCode;
            UnitNodes.RemoveAt(0);
            trvUnits.Nodes.Add(RootNode);
            AddChildren(RootNode, UnitNodes);

            // build up node for unassigned units
            TreeNode UnassignedNode = new TreeNode(UnassignedData.Description);
            UnassignedNode.Tag = UnassignedData;
            UnassignedNode.ToolTipText = UnassignedData.TypeCode;
            UnitNodes.RemoveAt(0);
            trvUnits.Nodes.Add(UnassignedNode);
            AddChildren(UnassignedNode, UnitNodes);

            Int64 MySiteKey = TSystemDefaults.GetSiteKeyDefault();
            ShowThisUnit(MySiteKey);

            FPetraUtilsObject.ApplySecurity(TSecurityChecks.SecurityPermissionsSetupScreensEditingAndSaving);

            //Active the print menu item
            mniFilePrint.Enabled = true;
            mniFilePrint.Click += new EventHandler(print);
        }

        private void print(object sender, EventArgs ea)
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            TCommonScreensForwarding.OpenPrintUnitHierarchy.Invoke(TRemote.MPartner.Partner.WebConnectors.GetUnitHierarchyRootUnitKey(), MainWindow);
        }

        private void GetAllChildren(TreeNode Parent, ref ArrayList UnitNodes)
        {
            UnitNodes.Add(Parent.Tag);

            foreach (TreeNode Node in Parent.Nodes)
            {
                GetAllChildren(Node, ref UnitNodes);
            }
        }

        /// Our main keyboard handler
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Home | Keys.Control:
                case Keys.Home:

                    if (trvUnits.SelectedNode != null)
                    {
                        SelectNode(trvUnits.SelectedNode.FirstNode);
                    }
                    else
                    {
                        SelectNode(trvUnits.GetNodeAt(1, 1));
                    }

                    break;

                case Keys.Up | Keys.Control:
                case Keys.Up:

                    if (trvUnits.SelectedNode != null)
                    {
                        SelectNode(trvUnits.SelectedNode.PrevVisibleNode);
                    }

                    break;

                case Keys.Down | Keys.Control:
                case Keys.Down:

                    if (trvUnits.SelectedNode != null)
                    {
                        SelectNode(trvUnits.SelectedNode.NextVisibleNode);
                    }

                    break;

                case Keys.End | Keys.Control:
                case Keys.End:

                    if (trvUnits.SelectedNode != null)
                    {
                        SelectNode(trvUnits.SelectedNode.LastNode);
                    }
                    else
                    {
                        SelectNode(trvUnits.GetNodeAt(1, trvUnits.Bottom - 2));
                    }

                    break;

                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Get the number of changed records and specify a message to incorporate into the 'Do you want to save?' message box
        /// </summary>
        /// <param name="AMessage">An optional message to display.  If the parameter is an empty string a default message will be used</param>
        /// <returns>The number of changed records.  Return -1 to imply 'unknown'.</returns>
        public int GetChangedRecordCount(out string AMessage)
        {
            AMessage = String.Empty;
            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            if (MessageBox.Show(FStatus + "\r\n" + Catalog.GetString("Do you want to keep these changes?"),
                    Catalog.GetString("Unit Hierarchy"), MessageBoxButtons.YesNo)
                == DialogResult.No)
            {
                return false;
            }

            ArrayList UnitNodes = new ArrayList();
            // collect all children under root organisational node
            GetAllChildren(trvUnits.Nodes[0], ref UnitNodes);

            // Collect all children under 'Unassigned' so that substructures are preserved (but not the ones directly under 'Unassigned' since that is not actually a Unit).
            // We need to go 2 levels deep as on the server each node is stored alongside it's parent unit key
            foreach (TreeNode Node in trvUnits.Nodes[1].Nodes)
            {
                foreach (TreeNode SubNode in Node.Nodes)
                {
                    GetAllChildren(SubNode, ref UnitNodes);
                }
            }

            TRemote.MPersonnel.WebConnectors.SaveUnitHierarchy(UnitNodes);

            FStatus = "";
            FPetraUtilsObject.HasChanges = false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReassignParent(object sender, EventArgs e)
        {
            DoReassignment(FChildNodeReference, FParentNodeReference);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (SaveChanges())
            {
                // Broadcast message to update partner's Partner Edit screen if open
                TFormsMessage BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcUnitHierarchyChanged);
                BroadcastMessage.SetMessageDataUnitHierarchy(FChangedParents);
                TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);

                FChangedParents = new List <Tuple <string, long, long>>();

                FPetraUtilsObject.HasChanges = false;
                FPetraUtilsObject.DisableSaveButton();
            }

            this.Cursor = Cursors.Default;
        }
    }
}