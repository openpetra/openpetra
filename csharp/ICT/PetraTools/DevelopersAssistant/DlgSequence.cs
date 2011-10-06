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
    /**************************************************************************************************************************************
     *
     * The dialog class that permits the modification of a sequence of tasks
     *
     * ***********************************************************************************************************************************/

    /// <summary>
    /// Dialog that displays the current settings for a sequence of tasks
    /// </summary>
    public partial class DlgSequence : Form
    {
        /// <summary>
        /// The exit data specified when the user clicked the OK button
        /// </summary>
        public List <NantTask.TaskItem>ExitSequence = null;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public DlgSequence()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Call this before ShowDialog to initialise the dialog with entry values to be edited
        /// </summary>
        /// <param name="Sequence"></param>
        public void InitializeList(List <NantTask.TaskItem>Sequence)
        {
            lstSequence.Items.Clear();

            for (int i = 0; i < Sequence.Count; i++)
            {
                NantTask task = new NantTask(Sequence[i]);
                lstSequence.Items.Add(task.Description);
            }

            if (lstSequence.Items.Count > 0)
            {
                lstSequence.SelectedIndex = 0;
            }

            SetEnabledStates();
        }

        // Add a new task to the list at the end of the sequence.  Select the new item
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DlgSequenceItem dlg = new DlgSequenceItem();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            lstSequence.SelectedIndex = lstSequence.Items.Add(dlg.SelectedTask.Description);
            SetEnabledStates();
        }

        // Insert an item above the currently selected one.  The current selection is maintained.
        private void btnInsert_Click(object sender, EventArgs e)
        {
            DlgSequenceItem dlg = new DlgSequenceItem();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            lstSequence.Items.Insert(lstSequence.SelectedIndex, dlg.SelectedTask.Description);
            SetEnabledStates();
        }

        // Swap the current selection and the one above it
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = lstSequence.SelectedIndex;
            string s = lstSequence.Items[index].ToString();

            lstSequence.Items.Insert(index - 1, s);
            lstSequence.Items.RemoveAt(index + 1);
            lstSequence.SelectedIndex = --index;

            SetEnabledStates();
        }

        // Swap the current selection and the one below it
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int index = lstSequence.SelectedIndex;
            string s = lstSequence.Items[index].ToString();

            lstSequence.Items.Insert(index + 2, s);
            lstSequence.Items.RemoveAt(index);
            lstSequence.SelectedIndex = ++index;

            SetEnabledStates();
        }

        // Remove the current selection
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to delete the selected item?", Program.APP_TITLE, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int index = lstSequence.SelectedIndex;
            lstSequence.Items.RemoveAt(index);

            if (index > 0)
            {
                lstSequence.SelectedIndex = --index;
            }
            else if (lstSequence.Items.Count > 0)
            {
                lstSequence.SelectedIndex = 0;
            }

            SetEnabledStates();
        }

        // Set the button enabled states according to the current selection
        private void SetEnabledStates()
        {
            btnInsert.Enabled = lstSequence.Items.Count > 0;
            btnMoveUp.Enabled = lstSequence.SelectedIndex > 0 && lstSequence.Items.Count > 1;
            btnMoveDown.Enabled = lstSequence.SelectedIndex < lstSequence.Items.Count - 1 && lstSequence.Items.Count > 1;
            btnRemove.Enabled = lstSequence.Items.Count > 0;
        }

        private void lstSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEnabledStates();
        }

        // Handle the close action.  Save the current sequence of tasks in the public form variable
        private void btnOK_Click(object sender, EventArgs e)
        {
            ExitSequence = new List <NantTask.TaskItem>();

            for (int i = 0; i < lstSequence.Items.Count; i++)
            {
                NantTask task = new NantTask(lstSequence.Items[i].ToString());
                ExitSequence.Add(task.Item);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}