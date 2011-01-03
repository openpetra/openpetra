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
using GNU.Gettext;
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
    public partial class TFrmGLCostCentreHierarchy
    {
        private Int32 FLedgerNumber;

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

            this.trvCostCentres.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
        }

        private void InsertNodeIntoTreeView(TreeNodeCollection AParentNodes, ACostCentreRow ADetailRow)
        {
            string nodeLabel = ADetailRow.CostCentreCode;

            if (!ADetailRow.IsCostCentreNameNull())
            {
                nodeLabel += " (" + ADetailRow.CostCentreName + ")";
            }

            TreeNode newNode = AParentNodes.Add(nodeLabel);

            newNode.Tag = ADetailRow;

            FMainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
            FMainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " = '" + ADetailRow.CostCentreCode + "'";

            foreach (DataRowView rowView in FMainDS.ACostCentre.DefaultView)
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
                string oldCode = currentCostCentre.CostCentreCode;
                GetDetailsFromControls(currentCostCentre);

                // this only works for new rows; old rows have the primary key fields readonly
                if (currentCostCentre.CostCentreCode != oldCode)
                {
                    // there are no references to this new row yet, apart from children nodes
                }

                string nodeLabel = currentCostCentre.CostCentreCode;

                if (!currentCostCentre.IsCostCentreNameNull())
                {
                    nodeLabel += " (" + currentCostCentre.CostCentreName + ")";
                }

                FCurrentNode.Text = nodeLabel;
            }

            FCurrentNode = e.Node;

            // update detail panel
            ShowDetails((ACostCentreRow)e.Node.Tag);
        }

        private void AddNewCostCentre(Object sender, EventArgs e)
        {
            if (FCurrentNode == null)
            {
                MessageBox.Show(Catalog.GetString("You can only add a new cost centre after selecting a parent cost centre"));
                return;
            }

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

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitDS, out AVerificationResult);
        }

        private void GetDataFromControlsManual()
        {
            // TODO: report to (drag/drop)
            // TODO: report order (drag/drop)
            // TODO: posting/summary (new/delete)

            if (FCurrentNode != null)
            {
                ACostCentreRow currentCostCentre = (ACostCentreRow)FCurrentNode.Tag;
                GetDetailsFromControls(currentCostCentre);
            }
        }
    }
}