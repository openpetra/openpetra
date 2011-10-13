//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    /********************************************************************************************************
     *
     * A simple dialog for selecting one of the available tasks into a user-defined sequence of tasks.
     *
     * *****************************************************************************************************/

    /// <summary>
    /// Dialog that contains the complete list of possible items to include in a sequence
    /// </summary>
    public partial class DlgSequenceItem : Form
    {
        /// <summary>
        /// The exit data specified when the user clicked the OK button
        /// </summary>
        public NantTask SelectedTask = null;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public DlgSequenceItem()
        {
            InitializeComponent();
            PopulateList();
            listTasks.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedTask = new NantTask(listTasks.Items[listTasks.SelectedIndex].ToString());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void listTasks_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }

        private void PopulateList()
        {
            for (NantTask.TaskItem i = NantTask.FirstBasicItem; i <= NantTask.LastBasicItem; i++)
            {
                NantTask t = new NantTask(i);
                listTasks.Items.Add(t.Description);
            }

            for (NantTask.TaskItem i = NantTask.FirstCodeGenItem; i <= NantTask.LastCodeGenItem; i++)
            {
                NantTask t = new NantTask(i);
                listTasks.Items.Add(t.Description);
            }

            for (NantTask.TaskItem i = NantTask.FirstCompileItem; i <= NantTask.LastCompileItem; i++)
            {
                NantTask t = new NantTask(i);
                listTasks.Items.Add(t.Description);
            }

            for (NantTask.TaskItem i = NantTask.FirstMiscItem; i <= NantTask.LastMiscItem; i++)
            {
                NantTask t = new NantTask(i);
                listTasks.Items.Add(t.Description);
            }

            for (NantTask.TaskItem i = NantTask.FirstDatabaseItem; i <= NantTask.LastDatabaseItem; i++)
            {
                NantTask t = new NantTask(i);
                listTasks.Items.Add(t.Description);
            }
        }
    }
}