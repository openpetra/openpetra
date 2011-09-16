//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2011 by OM International
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
    ///
    /// If you think that you can optimize the code feel free to do it but be careful. It
    /// shall run ether in the client an in NUnit ...
    /// </summary>
    public class TTrvTreeView : System.Windows.Forms.TreeView
    {
        // "Last used Node" or the node which was left by "lostFocus"
        // This node is not necessarily the same node if the control get back the focus
        private TreeNode workNode = null;

        // Actual used BackgroundColor which regulary is defined by System-Settings
        // Defaut value is Color.Empty which is a real Color on the Color-Palette.
        private Color treeBackgroundColor = Color.Empty;
        // Actual used (Font) ForeColor ...
        private Color treeForeColor = Color.Empty;


        private bool colorsInitialized = false;

        /// <summary>
        /// The constructor only installs two Eventhandlers.
        /// </summary>
        public TTrvTreeView()
        {
            GotFocus += new EventHandler(TreeViewGotFocus);
            LostFocus += new EventHandler(TreeViewLostFocus);

            // System.Console.WriteLine("TTrvTreeView");
        }

        // Common GetFocus-Event
        private void TreeViewLostFocus(object sender, EventArgs e)
        {
            try
            {
                CheckColorInitializing();

                if (colorsInitialized)
                {
                    SelectedNode.BackColor = Color.Gray;
                    SelectedNode.ForeColor = Color.White;
                }

                workNode = SelectedNode;
            }
            catch (Exception)
            {
            }
            // System.Console.WriteLine("TTrvTreeView.LostFocus");
        }

        // Common LostFocus-Event
        private void TreeViewGotFocus(object sender, EventArgs e)
        {
            try
            {
                CheckColorInitializing();

                if (colorsInitialized)
                {
                    // restore the node colors
                    workNode.BackColor = treeBackgroundColor;
                    workNode.ForeColor = treeForeColor;
                }
            }
            catch (Exception)
            {
            }
            // System.Console.WriteLine("TTrvTreeView.GotFocus");
        }

        private void CheckColorInitializing()
        {
            // If the colors are not initialized - then do so ...
            if (!colorsInitialized)
            {
                treeBackgroundColor = SelectedNode.BackColor;
                treeForeColor = SelectedNode.ForeColor;
                colorsInitialized = true;
            }
        }
    }
}