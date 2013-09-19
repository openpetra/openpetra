//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using System.Drawing;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLCostCentreHierarchy
    {
        private const string INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE = "#UNASSIGNEDDETAILCOSTCENTRECODE#";

        private TreeNode FDragNode = null;
        private TreeNode FDragTarget = null;
        private TreeNode FSelectedNode = null;
        private String FStatus = "";

        private Int32 FLedgerNumber;
        private bool FIAmDeleting = false;
        private bool FIAmUpdating;

        private String strOldDetailCostCentreCode; // this string is used to detect that the user has renamed an existing Cost Centre.
        private String strOldDetailCostCentreName;

        private string FRecentlyUpdatedDetailCostCentreCode = INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE;


        private class CostCentreNodeDetails
        {
            /// <summary>
            /// This will be true for Summary cost codes, initially Unknown for "leaves".
            /// On newly created cost codes, this will be true.
            /// On a "need to know" basis, it will be set false for cost codes that already have transactions posted to them.
            /// </summary>
            ///
            public Boolean? CanHaveChildren;

            /// <summary>
            /// This will be initially false for Summary cost codes that have children, unknown for "leaves".
            /// On newly created cost codes, this will be true.
            /// On a "need to know" basis, it will be set false for cost codes that already have transactions posted to them.
            /// </summary>
            public Boolean? CanDelete;
            public Boolean IsNew;
            /// <summary>..and here's the actual data! </summary>
            public ACostCentreRow CostCentreRow;
        };

        /// <summary>
        /// Setup the CostCentre hierarchy of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                PopulateTreeView();
            }
        }

        /// <summary>
        /// Make this Cost Centre a child of the selected one in the hierarchy (from drag-drop).
        /// </summary>
        /// <param name="AChild"></param>
        /// <param name="ANewParent"></param>
        private void DoReassignment(TreeNode AChild, TreeNode ANewParent)
        {

            if (((CostCentreNodeDetails)AChild.Tag).CostCentreRow.SystemCostCentreFlag)
            {
                MessageBox.Show(Catalog.GetString("This is a System Cost Code and cannot be moved."), Catalog.GetString("Re-assign Cost Code"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if ((AChild != null) && (ANewParent != null))
            {
                String PrevParent = AChild.Parent.Text;
                ACostCentreRow NewParentCostCentre = ((CostCentreNodeDetails)ANewParent.Tag).CostCentreRow;

                String NewParentCode = NewParentCostCentre.CostCentreCode;
                TreeNode NewNode = (TreeNode)AChild.Clone();
                ACostCentreRow MovingCostCentre = ((CostCentreNodeDetails)NewNode.Tag).CostCentreRow;

                TreeNode PreviousParentNode = AChild.Parent;

                MovingCostCentre.CostCentreToReportTo = NewParentCode;
                NewParentCostCentre.PostingCostCentreFlag = false; // Perhaps was false already.
                InsertAlphabetically(ANewParent, NewNode);
                NewNode.Expand();
                ANewParent.Expand();
                ANewParent.BackColor = Color.White;
                FStatus += String.Format(Catalog.GetString("{0} was moved from {1} to {2}.\r\n"),
                    AChild.Text, PrevParent, ANewParent.Text);
                txtStatus.Text = FStatus;

                //Remove Original Node
                AChild.Remove();

                // If the previous parent now has no children, it can be used for posting:
                if (PreviousParentNode.Nodes.Count == 0)
                {
                    ((CostCentreNodeDetails)PreviousParentNode.Tag).CostCentreRow.PostingCostCentreFlag = true;
                }

                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.UnhookControl(pnlDetails, true); // I don't want changes in these values to cause SetChangedFlag - I'll set it myself.

            txtDetailCostCentreCode.Leave += new EventHandler(UpdateOnControlChanged);
            txtDetailCostCentreName.Leave += new EventHandler(UpdateOnControlChanged);
            chkDetailCostCentreActiveFlag.CheckedChanged += new System.EventHandler(UpdateOnControlChanged);
            cmbDetailCostCentreType.SelectedIndexChanged += new System.EventHandler(UpdateOnControlChanged);
            FIAmUpdating = false;
            txtDetailCostCentreCode.TextChanged += new EventHandler(txtDetailCostCentreCode_TextChanged);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(OnHierarchySaved);

            // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
            // It is easier to do this than to configure all the different test execution methods to use STA
            try
            {
                trvCostCentres.AllowDrop = true;
                trvCostCentres.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeView_ItemDrag);
                trvCostCentres.DragOver += new System.Windows.Forms.DragEventHandler(treeView_DragOver);
                trvCostCentres.DragDrop += new System.Windows.Forms.DragEventHandler(treeView_DragDrop);
            }
            catch (InvalidOperationException)
            {
                // ex.Message is: DragDrop registration did not succeed.
                // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
            }
        }

        private void OnHierarchySaved (System.Object sender, TDataSavedEventArgs e)
        {
            SetPrimaryKeyReadOnly(false);
        }

        private void txtDetailCostCentreCode_TextChanged(object sender, EventArgs e)
        {
            if (FIAmUpdating)
                return;

            CostCentreNodeDetails nodeDetails = (CostCentreNodeDetails)FCurrentNode.Tag;
            if (nodeDetails.CostCentreRow.SystemCostCentreFlag)
            {
                FIAmUpdating = true;
                txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                FIAmUpdating = false;
                MessageBox.Show(Catalog.GetString("System Cost Centre Code cannot be changed."),
                    Catalog.GetString("Rename Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            bool ICanEditCostCentreCode = (nodeDetails.IsNew || !FPetraUtilsObject.HasChanges);
            btnRename.Visible = (strOldDetailCostCentreCode != txtDetailCostCentreCode.Text) && ICanEditCostCentreCode;
            if (!nodeDetails.IsNew && FPetraUtilsObject.HasChanges) // The user wants to change a cost centre code, but I can't allow it.
            {
                FIAmUpdating = true;
                txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                FIAmUpdating = false;
                MessageBox.Show(Catalog.GetString("Cost Centre Codes cannot be changed while there are other unsaved changes.\r\nSave first, then rename the Cost Centre."),
                    Catalog.GetString("Rename Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

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
                //
                // I will need to check whether it's OK re-order the world like this...
                //
                GetCostCentreAttributes(ref NodeDetails);

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


        private void ShowDetailsManual(ACostCentreRow ARow)
        {
            if (ARow == null)
            {
                txtDetailCostCentreCode.Text = "";
                txtDetailCostCentreName.Text = "";
            }
            else
            {
                // I allow the user to attempt to change the primary key,
                // but if the selected record is not new, AND they have made any other changes,
                // the txtDetailCostCentreCode_TextChanged method will disallow any change.
                SetPrimaryKeyReadOnly(false);
                btnRename.Visible = false;
            }
        }

        /// <summary>
        /// load account hierarchy from the dataset into the tree view
        /// </summary>
        private void PopulateTreeView()
        {
            trvCostCentres.BeginUpdate();
            trvCostCentres.Nodes.Clear();

            // find the root cost centre
            FMainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " IS NULL";

            InsertNodeIntoTreeView(trvCostCentres.Nodes,
                (ACostCentreRow)FMainDS.ACostCentre.DefaultView[0].Row);

            FMainDS.ACostCentre.DefaultView.RowFilter = "";

            trvCostCentres.EndUpdate();

            this.trvCostCentres.BeforeSelect += new TreeViewCancelEventHandler(TreeViewBeforeSelect);
            this.trvCostCentres.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(TreeViewAfterSelect);
        }

        void TreeViewBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!FIAmDeleting)
            {
                if (!ValidateAllData(true, true))
                {
                    e.Cancel = true;
                }
            }
        }

        private static String NodeLabel(ACostCentreRow ADetailRow)
        {
            string nodeLabel = ADetailRow.CostCentreCode;

            if (!ADetailRow.IsCostCentreNameNull())
            {
                nodeLabel += " (" + ADetailRow.CostCentreName + ")";
            }

            return nodeLabel;
        }

        private void InsertNodeIntoTreeView(TreeNodeCollection AParentNodes, ACostCentreRow ADetailRow)
        {
            TreeNode newNode = AParentNodes.Add(NodeLabel(ADetailRow));

            newNode.Name = ADetailRow.CostCentreCode;
            CostCentreNodeDetails NewNodeDetails = new CostCentreNodeDetails();
            NewNodeDetails.CostCentreRow = ADetailRow;
            NewNodeDetails.IsNew = false;
            newNode.Tag = NewNodeDetails;

            DataView view = new DataView(FMainDS.ACostCentre);
            view.Sort = ACostCentreTable.GetCostCentreCodeDBName();
            view.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " = '" + ADetailRow.CostCentreCode + "'";

            foreach (DataRowView rowView in view)
            {
                InsertNodeIntoTreeView(newNode.Nodes, (ACostCentreRow)rowView.Row);
            }
        }

        TreeNode FCurrentNode = null;

        private void GetCostCentreAttributes(ref CostCentreNodeDetails ANodeDetails)
        {
            if (!ANodeDetails.CanHaveChildren.HasValue || !ANodeDetails.CanDelete.HasValue)
            {
                bool RemoteCanBeParent = false;
                bool RemoteCanDelete = false;

                if (TRemote.MFinance.Setup.WebConnectors.GetCostCentreAttributes(FLedgerNumber, ANodeDetails.CostCentreRow.CostCentreCode,
                        out RemoteCanBeParent, out RemoteCanDelete))
                {
                    ANodeDetails.CanHaveChildren = RemoteCanBeParent;
                    ANodeDetails.CanDelete = RemoteCanDelete;
                }
            }
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != e.Node))
            {
                ACostCentreRow currentCostCentre = ((CostCentreNodeDetails)FCurrentNode.Tag).CostCentreRow;

                if (currentCostCentre.RowState != DataRowState.Deleted) // If this row was removed, I can't look at it..
                {
                    GetDetailsFromControls(currentCostCentre);
                    FCurrentNode.Text = NodeLabel(currentCostCentre);
                    FCurrentNode.Name = currentCostCentre.CostCentreCode;
                }
            }

            FCurrentNode = e.Node;

            // update detail panel
            FIAmUpdating = true;
            ACostCentreRow TempRow = ((CostCentreNodeDetails)e.Node.Tag).CostCentreRow;
            ShowDetails(TempRow);
            FIAmUpdating = false;
            strOldDetailCostCentreCode = TempRow.CostCentreCode;
            strOldDetailCostCentreName = TempRow.CostCentreName;
        }

        private void AddNewCostCentre(Object sender, EventArgs e)
        {
            if (FCurrentNode == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new cost centre after selecting a parent cost centre"));
                return;
            }

            if (ValidateAllData(true, true))
            {
                CostCentreNodeDetails ParentNodeDetails = (CostCentreNodeDetails)FCurrentNode.Tag;
                ACostCentreRow ParentRow = ParentNodeDetails.CostCentreRow;
                GetCostCentreAttributes(ref ParentNodeDetails);

                if (!ParentNodeDetails.CanHaveChildren.Value)
                {
                    MessageBox.Show(
                        String.Format(Catalog.GetString("Cost Centre {0} is in use and cannot become a summary Cost Centre."),
                            ParentRow.CostCentreCode), Catalog.GetString("NewCostCentre"));
                    return;
                }

                txtDetailCostCentreCode.Focus();

                string newName = Catalog.GetString("NewCostCentre");
                Int32 countNewCostCentre = 0;

                if (FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
                {
                    while (FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber, newName + countNewCostCentre.ToString() }) != null)
                    {
                        countNewCostCentre++;
                    }

                    newName += countNewCostCentre.ToString();
                }

                ACostCentreRow parentCostCentre =
                    (ACostCentreRow)FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber, ParentRow.CostCentreCode });

                ACostCentreRow newCostCentre = FMainDS.ACostCentre.NewRowTyped();
                newCostCentre.CostCentreCode = newName;
                newCostCentre.LedgerNumber = FLedgerNumber;
                newCostCentre.CostCentreActiveFlag = true;
                newCostCentre.CostCentreType = parentCostCentre.CostCentreType;
                newCostCentre.PostingCostCentreFlag = true;
                newCostCentre.CostCentreToReportTo = parentCostCentre.CostCentreCode;
                FMainDS.ACostCentre.Rows.Add(newCostCentre);

                parentCostCentre.PostingCostCentreFlag = false;

                trvCostCentres.BeginUpdate();
                TreeNode newNode = FCurrentNode.Nodes.Add(newName);

                CostCentreNodeDetails NewNodeDetails = new CostCentreNodeDetails();
                NewNodeDetails.CostCentreRow = newCostCentre;
                NewNodeDetails.IsNew = true;
                newNode.Tag = NewNodeDetails;

                trvCostCentres.EndUpdate();

                trvCostCentres.SelectedNode = newNode;

                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void ExportHierarchy(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(TRemote.MFinance.Setup.WebConnectors.ExportCostCentreHierarchy(FLedgerNumber));

            TImportExportDialogs.ExportWithDialog(doc, Catalog.GetString("Save Cost Centre Hierarchy to file"));
        }

        private void ImportHierarchy(object sender, EventArgs e)
        {
            // TODO: open file; only will work if there are no GLM records and transactions yet
            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Load Cost Centre Hierarchy from file"));

            if (doc == null)
            {
                // import was cancelled
                return;
            }

            if (!TRemote.MFinance.Setup.WebConnectors.ImportCostCentreHierarchy(FLedgerNumber, TXMLParser.XmlToString(doc)))
            {
                MessageBox.Show(Catalog.GetString(
                        "Import of new Cost Centre Hierarchy failed; perhaps there were already balances? Try with a new ledger!"),
                    Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // refresh the screen
                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                PopulateTreeView();
                MessageBox.Show("Import of new Cost Centre Hierarchy has been successful",
                    Catalog.GetString("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CheckForInvalidCostCentre(TreeNodeCollection NodeCol)
        {
            string newName = Catalog.GetString("NewCostCentre");

            foreach (TreeNode ChildNode in NodeCol)
            {
                if (CheckForInvalidCostCentre(ChildNode.Nodes))
                {
                    return true;
                }

                ACostCentreRow CheckRow = ((CostCentreNodeDetails)ChildNode.Tag).CostCentreRow;

                if (CheckRow.CostCentreCode.IndexOf(newName) == 0)
                {
                    MessageBox.Show(
                        String.Format(Catalog.GetString("{0} is not a valid cost centre code.\r\nChange the code or remove it completely."),
                            CheckRow.CostCentreCode),
                        Catalog.GetString("GL Cost Centre Hierarchy"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    trvCostCentres.SelectedNode = ChildNode;
                    return true;
                }

                if (CheckRow.CostCentreCode == "")
                {
                    MessageBox.Show(
                        Catalog.GetString(
                            "Cost centre code is empty.\r\nSupply a valid cost centre code or also remove the name to delete this record."),
                        Catalog.GetString("GL Cost Centre Hierarchy"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    trvCostCentres.SelectedNode = ChildNode;
                    return true;
                }
            }

            return false;
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            //
            // I'll look through and check whether any of the cost centres still have "NewCostCentre"..
            //
            if (CheckForInvalidCostCentre(trvCostCentres.Nodes))
            {
                AVerificationResult = null;
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);
                return TSubmitChangesResult.scrInfoNeeded;
            }

            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitDS, out AVerificationResult);
        }

        /// <summary>
        /// Delete the row in the editor
        /// NOTE: A cost centre with children cannot be deleted.
        /// </summary>
        /// <param name="ASelectedNode">FCurrentNode</param>
        /// <returns>true if the node was deleted</returns>
        private bool DeleteDataFromSelectedRow(TreeNode ASelectedNode)
        {
            CostCentreNodeDetails NodeDetails = (CostCentreNodeDetails)ASelectedNode.Tag;

            GetCostCentreAttributes(ref NodeDetails);

            if (NodeDetails.CanDelete.Value)
            {
                ACostCentreRow SelectedRow = NodeDetails.CostCentreRow;
                SelectedRow.Delete();
                return true;
            }
            else
            {
                MessageBox.Show(Catalog.GetString("This Cost Centre Code is in use and cannot be deleted."));
                return false;
            }
        }

        /// <summary>
        /// Change CostCentre Value
        ///
        /// The Cost Centre code is a foreign key in loads of tables,
        /// so renaming a Cost Centre code is a major work on the server.
        /// From the client's perspective it's easy - we just need to ask the server to do it!
        ///
        /// </summary>
        private bool CheckCostCentreValueChanged()
        {
            if (FIAmUpdating || (strOldDetailCostCentreCode == null))
            {
                return false;
            }

            String strNewDetailCostCentreCode = txtDetailCostCentreCode.Text;
            bool changeAccepted = false;

            if (strNewDetailCostCentreCode != FRecentlyUpdatedDetailCostCentreCode)
            {
                if (strNewDetailCostCentreCode != strOldDetailCostCentreCode)
                {
                    if (strOldDetailCostCentreCode.IndexOf(Catalog.GetString("NewCostCentre")) < 0) // If they're just changing this from the initial value, don't show warning.
                    {
                        if (MessageBox.Show(String.Format(Catalog.GetString(
                                        "You have changed the Cost Centre Code from '{0}' to '{1}'.\r\n\r\nPlease confirm that you want to rename this Cost Centre Code by choosing 'OK'."),
                                    strOldDetailCostCentreCode,
                                    strNewDetailCostCentreCode), Catalog.GetString("Rename Cost Centre Code: Confirmation"),
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                            btnRename.Visible = false;
                            return false;
                        }
                    }

                    FRecentlyUpdatedDetailCostCentreCode = strNewDetailCostCentreCode;
                    CostCentreNodeDetails NodeDetails = (CostCentreNodeDetails)trvCostCentres.SelectedNode.Tag;

                    try
                    {
                        NodeDetails.CostCentreRow.BeginEdit();
                        NodeDetails.CostCentreRow.CostCentreCode = strNewDetailCostCentreCode;
                        NodeDetails.CostCentreRow.EndEdit();

                        trvCostCentres.BeginUpdate();
                        trvCostCentres.SelectedNode.Text = strNewDetailCostCentreCode;
                        trvCostCentres.SelectedNode.Name = strNewDetailCostCentreCode;
                        trvCostCentres.EndUpdate();

                        changeAccepted = true;
                    }
                    catch (System.Data.ConstraintException)
                    {
                        txtDetailCostCentreCode.Text = strOldDetailCostCentreCode;
                        NodeDetails.CostCentreRow.CancelEdit();

                        FRecentlyUpdatedDetailCostCentreCode = INTERNAL_UNASSIGNED_DETAIL_COSTCENTRE_CODE;

                        FStatus += Catalog.GetString("Cost Centre Code change REJECTED!") + Environment.NewLine;
                        txtStatus.Text = FStatus;

                        MessageBox.Show(String.Format(
                                Catalog.GetString(
                                    "Renaming Cost Centre Code '{0}' to '{1}' is not possible because a Cost Centre Code by the name of '{2}' already exists.\r\n\r\n--> Cost Centre Code reverted to previous value!"),
                                strOldDetailCostCentreCode, strNewDetailCostCentreCode, strNewDetailCostCentreCode),
                            Catalog.GetString("Renaming Not Possible - Conflicts With Existing Cost Centre Code"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtDetailCostCentreCode.Focus();
                    }

                    if (changeAccepted)
                    {
                        if (NodeDetails.IsNew)
                        {
                            // This is the code for changes in "un-committed" nodes:
                            // there are no references to this new row yet, apart from children nodes, so I can just change them here and carry on!

                            // fixup children nodes
                            foreach (TreeNode childnode in trvCostCentres.SelectedNode.Nodes)
                            {
                                ((CostCentreNodeDetails)childnode.Tag).CostCentreRow.CostCentreCode = strNewDetailCostCentreCode;
                            }

                            strOldDetailCostCentreCode = strNewDetailCostCentreCode;
                            FPetraUtilsObject.HasChanges = true;
                        }
                        else
                        {
                            FStatus += Catalog.GetString("Updating Cost Centre Code change - please wait.\r\n");
                            txtStatus.Text = FStatus;
                            TVerificationResultCollection VerificationResults;

                            // If this code was previously in the DB, I need to assume that there may be transactions posted against it.
                            // There's a server call I need to use, and after the call I need to re-load this page.
                            // (No other changes will be lost, because the change would not have been allowed if there were already changes.)
                            this.Cursor = Cursors.WaitCursor;
                            bool Success = TRemote.MFinance.Setup.WebConnectors.RenameCostCentreCode(strOldDetailCostCentreCode,
                                strNewDetailCostCentreCode,
                                FLedgerNumber,
                                out VerificationResults);
                            this.Cursor = Cursors.Default;

                            if (Success)
                            {
                                DataTable NewTable;
                                TRemote.MFinance.Cacheable.WebConnectors.RefreshCacheableTable(TCacheableFinanceTablesEnum.CostCentreList,
                                    FLedgerNumber,
                                    out NewTable);
                                FMainDS = TRemote.MFinance.Setup.WebConnectors.LoadCostCentreHierarchy(FLedgerNumber);
                                strOldDetailCostCentreCode = "";
                                txtDetailCostCentreCode.Text = "";
                                FPetraUtilsObject.HasChanges = false;
                                PopulateTreeView();
                                FCurrentNode = null;

                                TreeNode[] NewNode = trvCostCentres.Nodes.Find(strNewDetailCostCentreCode, true);

                                if (NewNode.Length > 0) // should be - unless the server is faulty!
                                {
                                    trvCostCentres.SelectedNode = NewNode[0];
                                    ShowDetails(((CostCentreNodeDetails)NewNode[0].Tag).CostCentreRow);

                                    if (NewNode[0].Parent != null)
                                    {
                                        NewNode[0].Parent.Expand();
                                    }

                                    NewNode[0].Expand();
                                }

                                FStatus = "";
                                txtStatus.Text = FStatus;
                                FPetraUtilsObject.HasChanges = false;
                                FPetraUtilsObject.DisableSaveButton();
                                changeAccepted = true;

                                FStatus += String.Format("Cost Centre Code changed to '{0}'.\r\n", strNewDetailCostCentreCode);
                                txtStatus.Text = FStatus;
                            }
                            else
                            {
                                MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Rename Cost Centre Code"));
                            }
                        }
                    } // if changeAccepted

                } // if changed

            }

            return changeAccepted;
        }

        private void GetDataFromControlsManual()
        {
            if ((!CheckCostCentreValueChanged())
                && (FCurrentNode != null))
            {
                ACostCentreRow SelectedRow = GetSelectedDetailRowManual();
                GetDetailsFromControls(SelectedRow);

                //
                // If I find that there's no data in the new node, I'll remove it right now.
                if ((SelectedRow.CostCentreCode == "") && (SelectedRow.CostCentreName == ""))
                {
                    if (DeleteDataFromSelectedRow(FCurrentNode))
                    {
                        TreeNode SelectThisNode = FCurrentNode.Parent;
                        FIAmDeleting = true;
                        trvCostCentres.Nodes.Remove(FCurrentNode);
                        FIAmDeleting = false;
                        trvCostCentres.SelectedNode = SelectThisNode;
                    }
                    else // The node was not deleted, so I'll restore it as it was!
                    {
                        SelectedRow.CostCentreCode = strOldDetailCostCentreCode;
                        SelectedRow.CostCentreName = strOldDetailCostCentreName;
                    }
                }
            }
        }

        private ACostCentreRow GetSelectedDetailRowManual()
        {
            if (FCurrentNode != null)
            {
                return ((CostCentreNodeDetails)FCurrentNode.Tag).CostCentreRow;
            }
            else
            {
                return null;
            }
        }

        private void UpdateOnControlChanged(Object sender, EventArgs e)
        {
            if (!FIAmUpdating)
            {
                ACostCentreRow Row = GetSelectedDetailRowManual();
                if (Row != null
                    && cmbDetailCostCentreType.GetSelectedString() != Row.CostCentreType
                    && Row.SystemCostCentreFlag)
                {
                    MessageBox.Show(
                        Catalog.GetString(
                            "This is a System Cost Centre and cannot be changed."),
                        Catalog.GetString("Cost Centre Type"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    FIAmUpdating = true;
                    cmbDetailCostCentreType.SetSelectedString(Row.CostCentreType);
                    FIAmUpdating = false;
                }


                if (CheckCostCentreValueChanged())
                {
                    return;
                }  // If not changed, or the rename didn't happen, I can carry on...

                GetDataFromControlsManual();
                if (FCurrentNode != null)
                {
                    String NewNodeName = NodeLabel(Row);
                    if ((FCurrentNode.Text != NewNodeName) 
                        || (FCurrentNode.Name != Row.CostCentreCode) 
                        || (cmbDetailCostCentreType.GetSelectedString() != Row.CostCentreType))
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                    FCurrentNode.Text = NewNodeName;
                    FCurrentNode.Name = Row.CostCentreCode;
                }
            }
        } // UpdateOnControlChanged

        private void LinkPartnerCostCentre(object sender, EventArgs e)
        {
            TFrmLinkPartnerCostCentre PartnerLinkScreen = new TFrmLinkPartnerCostCentre(this);

            PartnerLinkScreen.LedgerNumber = FLedgerNumber;
            PartnerLinkScreen.Show();
        }  // LinkPartnerCostCentre
    } // TFrmGLCostCentreHierarchy
} // namespace