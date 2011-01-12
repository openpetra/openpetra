//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ict.Common.Controls
{
	
	/// <summary>
	/// This tree view provides "common features" 
	/// 
	/// 1. If the tree losts the focus, the left node will stay marked as "has had focus"
	/// </summary>
	public class TTrvTreeView : System.Windows.Forms.TreeView
	{

		// "Last used Node" or the node which was left by "lostFocus"
		// This node is not necessarily the same node if the control get back the focus
		private TreeNode workNode;
		
		// Actual used BackgroundColor which may be selected by System-Settings
        private Color treeBackgroundColor;
		// Actual used (Font) ForeColor which may be selected by System-Settings
        private Color treeForeColor;
        
        // The constructor only installs an Eventhandler which is used only one time. 
        // This avoids some timing conflicts. 
		public TTrvTreeView() {
            GotFocus += new EventHandler(TreeViewFirstFocus);
		}
		
        // GetFocus-Event which is only used for the first GotFocus-Event
        private void TreeViewFirstFocus(object sender, EventArgs e) {
        	GotFocus -= new EventHandler(TreeViewFirstFocus);
        	LostFocus += new EventHandler(TreeViewLostFocus);
        	GotFocus += new EventHandler(TreeViewGotFocus);
        	treeBackgroundColor = SelectedNode.BackColor;
        	treeForeColor = SelectedNode.ForeColor;
        }

        // Common GetFocus-Event
        private void TreeViewLostFocus(object sender, EventArgs e) {
        	workNode = SelectedNode;
        	SelectedNode.BackColor = Color.Gray;  
        	SelectedNode.ForeColor = Color.White;
        }
        
        // Common LostFocus-Event
        private void TreeViewGotFocus(object sender, EventArgs e) {
        	workNode.BackColor = treeBackgroundColor;
        	workNode.ForeColor = treeForeColor;
        }
		
	}
}
