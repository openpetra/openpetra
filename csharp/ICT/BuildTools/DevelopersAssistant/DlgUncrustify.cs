//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    public partial class DlgUncrustify : Form
    {
        // Initial value for this is 'ICT' but will be set by the caller using SelectedPath setter
        private string _selectedPath = "ICT";

        /// <summary>
        /// Gets/sets the selected path in the tree view
        /// </summary>
        public string SelectedPath
        {
            set
            {
                _selectedPath = value;
            }

            get
            {
                return _selectedPath;
            }
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        public DlgUncrustify()
        {
            InitializeComponent();
        }

        private void DlgUncrustify_Load(object sender, EventArgs e)
        {
            // Expand all the nodes
            tvBaseFolder.Nodes[0].ExpandAll();

            // Initialise the selected path
            foreach (TreeNode tn in tvBaseFolder.Nodes)
            {
                if (IsSelectedNode(tn))
                {
                    break;
                }
            }
        }

        private bool IsSelectedNode(TreeNode aNode)
        {
            if (aNode.FullPath == _selectedPath)
            {
                tvBaseFolder.SelectedNode = aNode;
                return true;
            }

            foreach (TreeNode tn in aNode.Nodes)
            {
                if (IsSelectedNode(tn))
                {
                    return true;
                }
            }

            return false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tvBaseFolder.SelectedNode == null)
            {
                return;
            }

            _selectedPath = tvBaseFolder.SelectedNode.FullPath;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _selectedPath = null;
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}