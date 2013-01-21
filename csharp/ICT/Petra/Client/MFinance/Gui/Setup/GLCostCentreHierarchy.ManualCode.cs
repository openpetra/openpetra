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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmGLCostCentreHierarchy
    {
        private Int32 FLedgerNumber;
        private bool FIAmDeleting = false;
        private bool FIAmUpdating;

        /// <summary>
        /// Setup the account hierarchy of this ledger
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

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.UnhookControl(pnlDetails, true); // I don't want changes in these values to cause SetChangedFlag - I'll set it myself.

            txtDetailCostCentreCode.TextChanged += new EventHandler(UpdateOnControlChanged);
            txtDetailCostCentreName.TextChanged += new EventHandler(UpdateOnControlChanged);
            chkDetailCostCentreActiveFlag.CheckedChanged += new System.EventHandler(UpdateOnControlChanged);
            cmbDetailCostCentreType.SelectedIndexChanged += new System.EventHandler(UpdateOnControlChanged);
            FIAmUpdating = false;
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

            newNode.Tag = ADetailRow;

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

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // store current detail values
            if ((FCurrentNode != null) && (FCurrentNode != e.Node))
            {
                ACostCentreRow currentCostCentre = (ACostCentreRow)FCurrentNode.Tag;

                if (currentCostCentre.RowState != DataRowState.Deleted) // If this row was removed, I can't look at it..
                {
                    GetDetailsFromControls(currentCostCentre);
                    FCurrentNode.Text = NodeLabel(currentCostCentre);
                }
            }

            FCurrentNode = e.Node;

            // update detail panel
            FIAmUpdating = true;
            ShowDetails((ACostCentreRow)e.Node.Tag);
            FIAmUpdating = false;
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
                    (ACostCentreRow)FMainDS.ACostCentre.Rows.Find(new object[] { FLedgerNumber,
                                                                                 ((ACostCentreRow)FCurrentNode.Tag).CostCentreCode });

                ACostCentreRow newCostCentre = FMainDS.ACostCentre.NewRowTyped();
                newCostCentre.CostCentreCode = newName;
                newCostCentre.LedgerNumber = FLedgerNumber;
                newCostCentre.CostCentreActiveFlag = true;
                newCostCentre.CostCentreType = parentCostCentre.CostCentreType;
                newCostCentre.PostingCostCentreFlag = true;
                newCostCentre.CostCentreToReportTo = parentCostCentre.CostCentreCode;
                FMainDS.ACostCentre.Rows.Add(newCostCentre);

                // TODO: what if the parent cost centre already had a posting balance? do we have to move costcentres around, insert a dummy parent?
                parentCostCentre.PostingCostCentreFlag = false;

                trvCostCentres.BeginUpdate();
                TreeNode newNode = FCurrentNode.Nodes.Add(newName);
                newNode.Tag = newCostCentre;
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

                ACostCentreRow CheckRow = (ACostCentreRow)ChildNode.Tag;

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
                            "Cost centre code is empty.\r\nSupply a valid cost centre code or also remove the Name to delete this record."),
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
        /// NOTE: Before I can delete a cost centre, I have to delete any children it might have...
        /// </summary>
        /// <param name="CostCentreRow">FCurrentNode, or a child node via a recursive call</param>
        /// <returns>The node that should now be selected</returns>
        private void DeleteDataFromSelectedRow(TreeNode CostCentreRow)
        {
            foreach (TreeNode ChildNode in CostCentreRow.Nodes)
            {
                DeleteDataFromSelectedRow(ChildNode);
            }

            ACostCentreRow SelectedRow = (ACostCentreRow)CostCentreRow.Tag;
            SelectedRow.Delete();
        }

        private void GetDataFromControlsManual()
        {
            // TODO: report to (drag/drop)
            // TODO: report order (drag/drop)
            // TODO: posting/summary (new/delete)

            if (FCurrentNode != null)
            {
                GetDetailsFromControls(GetSelectedDetailRowManual());

                //
                // If I find that theere's no data in the new node, I'll remove it right now.
                ACostCentreRow SelectedRow = (ACostCentreRow)FCurrentNode.Tag;

                if ((SelectedRow.CostCentreCode == "") && (SelectedRow.CostCentreName == ""))
                {
                    DeleteDataFromSelectedRow(FCurrentNode);
                    TreeNode SelectThisNode = FCurrentNode.Parent;
                    FIAmDeleting = true;
                    trvCostCentres.Nodes.Remove(FCurrentNode);
                    FIAmDeleting = false;
                    trvCostCentres.SelectedNode = SelectThisNode;
                }
            }
        }

        private ACostCentreRow GetSelectedDetailRowManual()
        {
            if (FCurrentNode != null)
            {
                return (ACostCentreRow)FCurrentNode.Tag;
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
                GetDataFromControlsManual();
                FCurrentNode.Text = NodeLabel(GetSelectedDetailRowManual());
                FPetraUtilsObject.SetChangedFlag();
            }
        }
    }
}