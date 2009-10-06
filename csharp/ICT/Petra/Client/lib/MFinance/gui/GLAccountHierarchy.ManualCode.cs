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
        	string SelectedHierarchy = "STANDARD";
        	FMainDS.AAccountHierarchy.DefaultView.RowFilter =
        		AAccountHierarchyTable.GetAccountHierarchyCodeDBName() + " = '" + SelectedHierarchy + "'";
        	if (FMainDS.AAccountHierarchy.DefaultView.Count == 1)
        	{
        		InsertNodeIntoTreeView(trvAccounts.Nodes, SelectedHierarchy, 
        		                       ((AAccountHierarchyRow)FMainDS.AAccountHierarchy.DefaultView[0].Row).RootAccountCode);
        	}
        	
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
        	FCurrentNode = e.Node;
        	
        	FMainDS.AAccount.DefaultView.Sort = AAccountTable.GetAccountCodeDBName();
        	Int32 SelectedAccountIndex = FMainDS.AAccount.DefaultView.Find(e.Node.Tag.ToString());
			ShowDetails(SelectedAccountIndex);
        	
        	// TODO: store current detail values, update detail panel
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