//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;
using System.Collections;
using System.Drawing;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmUnitHierarchy
    {
        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private String FStatus = "";

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
                Parent.Nodes.Insert(Idx,Child);
            }
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

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            Point pt = trvUnits.PointToClient(new Point(e.X, e.Y));
            TreeNode NewDragTarget = trvUnits.GetNodeAt(pt);
            if (NewDragTarget != FDragTarget)
            {
                if (FDragTarget != null)
                {
                    FDragTarget.BackColor = Color.White;
                }
                FDragTarget = NewDragTarget;
            }

            if ((FDragTarget == null) || (FDragTarget == FDragNode) || (IsDescendantOf(FDragTarget, FDragNode)))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move;
                FDragTarget.BackColor = Color.Turquoise;
            }
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            if (FDragNode != null)
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                if (FDragTarget != null)
                {
                    String PrevParent = FDragNode.Parent.Text;
                    TreeNode NewNode = (TreeNode) FDragNode.Clone();
                    ((UnitHierarchyNode)NewNode.Tag).ParentUnitKey = ((UnitHierarchyNode)FDragTarget.Tag).MyUnitKey;
                    InsertAlphabetically(FDragTarget, NewNode);
                    NewNode.Expand();
                    FDragTarget.Expand();
                    FDragTarget.BackColor = Color.White;

                    FStatus += String.Format(Catalog.GetString("{0} was moved from {1} to {2}.\r\n"), 
                        FDragNode.Text, PrevParent, FDragTarget.Text);
                    txtStatus.Text = FStatus;

                    //Remove Original Node
                    FDragNode.Remove();
                    FDragNode = null;
                    FPetraUtilsObject.SetChangedFlag();
                }
            }
        }
        
        private void RunOnceOnActivationManual()
        {
            trvUnits.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeView_ItemDrag);
            trvUnits.DragOver += new System.Windows.Forms.DragEventHandler(treeView_DragOver);
            trvUnits.DragDrop += new System.Windows.Forms.DragEventHandler(treeView_DragDrop);
            trvUnits.AllowDrop = true;

            ArrayList UnitNodes = TRemote.MPersonnel.WebConnectors.GetUnitHeirarchy();
            //
            // The list of nodes returned by the above call are ordered to the extent that:
            //  * The root node appears first,
            //  * a parent appears before its child.
            UnitHierarchyNode RootData = (UnitHierarchyNode)UnitNodes[0];
            TreeNode RootNode = new TreeNode(RootData.Description);
            RootNode.Tag = RootData;
            UnitNodes.RemoveAt(0);
            int ParentNodeIndex = trvUnits.Nodes.Add(RootNode);
            AddChildren(RootNode, UnitNodes);
        }

        private void GetAllChildren(TreeNode Parent, ref ArrayList UnitNodes)
        {
            UnitNodes.Add(Parent.Tag);
            foreach (TreeNode Node in Parent.Nodes)
            {
                GetAllChildren(Node, ref UnitNodes);
            }
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
            GetAllChildren (trvUnits.Nodes[0], ref UnitNodes);
            Boolean SavedOk = TRemote.MPersonnel.WebConnectors.SaveUnitHierarchy(UnitNodes);
            if (SavedOk)
            {
                FPetraUtilsObject.HasChanges = false;
            }
            return SavedOk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            SaveChanges();
        }
    }
}