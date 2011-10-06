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

    public partial class DlgSequenceItem : Form
    {
        /// <summary>
        /// The exit data specified when the user clicked the OK button
        /// </summary>
        public NantTask SelectedTask = null;

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
