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
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// The Dashboard can host several <see cref="TLstTasks" /> Controls, which it caches.
    /// <para>
    /// TODO: Future plan is to include hosting of 'Collapsible Panels' (<see cref="TPnlCollapsible" />) as well.
    /// (see Bug #5111 [https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=511]).
    /// </para>
    /// </summary>
    /// <remarks>In the OpenPetra Main Menu it represents the area that is right of the Module Navigation and under
    /// the 'Breadcrumb Trail'.</remarks>
    public class TDashboard : System.Windows.Forms.Panel
    {
        private int FMaxTaskWidth;
        private TaskAppearance FTaskAppearance = TaskAppearance.staLargeTile;
        private bool FSingleClickExecution = false;
        private List <TLstTasks>FTaskLists = new List <TLstTasks>();
        private TLstTasks FCurrentTaskList = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TDashboard()
        {
            this.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.AutoScroll = true;
//              this.BackColor = System.Drawing.Color.Green;            // for debugging only...
        }

        #region Properties

        /// <summary>
        /// Maximum Task Width
        /// </summary>
        public int MaxTaskWidth
        {
            get
            {
                return FMaxTaskWidth;
            }

            set
            {
                if (FMaxTaskWidth != value)
                {
                    FMaxTaskWidth = value;

                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        ((TLstTasks) this.Controls[i]).MaxTaskWidth = FMaxTaskWidth;
                    }
                }
            }
        }

        /// <summary>
        /// Appearance of the Task (Large Tile, ListEntry).
        /// </summary>
        public TaskAppearance TaskAppearance
        {
            get
            {
                return FTaskAppearance;
            }

            set
            {
                if (FTaskAppearance != value)
                {
                    FTaskAppearance = value;

                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        ((TLstTasks) this.Controls[i]).TaskAppearance = FTaskAppearance;
                    }
                }
            }
        }

        /// <summary>
        /// Execution of the Task with a single click of the mouse?
        /// </summary>
        public bool SingleClickExecution
        {
            get
            {
                return FSingleClickExecution;
            }

            set
            {
                if (FSingleClickExecution != value)
                {
                    FSingleClickExecution = value;

                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        ((TLstTasks) this.Controls[i]).SingleClickExecution = FSingleClickExecution;
                    }
                }
            }
        }

        /// <summary>
        /// Get the current Task List
        /// </summary>
        public TLstTasks CurrentTaskList
        {
            get
            {
                return FCurrentTaskList;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a task list that corresponds to a node in the navigation document.  If a list exists already it returns that list.
        /// If there is no list for that node yet it creates a new one, which will be available next time.
        /// </summary>
        /// <param name="ATaskListNode">The node</param>
        /// <returns></returns>
        public TLstTasks GetTaskList(XmlNode ATaskListNode)
        {
            for (int i = 0; i < FTaskLists.Count; i++)
            {
                if (FTaskLists[i].Name.EndsWith(ATaskListNode.Name))
                {
                    return FTaskLists[i];
                }
            }

            TLstTasks newList = new TLstTasks(ATaskListNode, FTaskAppearance);
            FTaskLists.Add(newList);
            return newList;
        }

        /// <summary>
        /// Shows a <see cref="TLstTasks" /> Control.
        /// </summary>
        /// <param name="ATaskList">The <see cref="TLstTasks" /> Control that should be shown.  All other controls are hidden.
        /// This ensures that the TAB key can be used to navigate tasks using the keyboard.</param>
        public void ShowTaskList(TLstTasks ATaskList)
        {
            if (ATaskList != null)
            {
                for (int i = 0; i < FTaskLists.Count; i++)
                {
                    if (FTaskLists[i].Name != ATaskList.Name)
                    {
                        FTaskLists[i].Hide();
                    }
                }

                if (this.Controls.Contains(ATaskList))
                {
                    ATaskList.Show();

                    foreach (TUcoTaskGroup group in ATaskList.Groups.Values)
                    {
                        if (group.FocusSelectedTask())
                        {
                            break;
                        }
                    }
                }
                else
                {
                    ATaskList.SuspendLayout();
                    this.Controls.Add(ATaskList);
                    ATaskList.MaxTaskWidth = FMaxTaskWidth;
                    ATaskList.TaskAppearance = FTaskAppearance;
                    ATaskList.SingleClickExecution = FSingleClickExecution;
                    ATaskList.ResumeLayout();

                    SelectFirstTaskWithFocus(ATaskList);
                }

                FCurrentTaskList = ATaskList;
            }
        }

        /// <summary>
        /// Select the first task in the current task list and set focus to the control
        /// </summary>
        public void SelectFirstTaskWithFocus()
        {
            SelectFirstTaskWithFocus(FCurrentTaskList);
        }

        /// <summary>
        /// Select the first task in the specified task list and set focus to the control
        /// </summary>
        private void SelectFirstTaskWithFocus(TLstTasks ATaskList)
        {
            if (ATaskList == null)
            {
                return;
            }

            for (int i = ATaskList.Controls.Count - 1; i >= 0; i--)
            {
                TUcoTaskGroup group = (TUcoTaskGroup)ATaskList.Controls[i];

                for (int k = 0; k < group.Controls[0].Controls.Count; k++)
                {
                    TUcoSingleTask task = (TUcoSingleTask)group.Controls[0].Controls[k];

                    if (task.Enabled)
                    {
                        task.FocusTask();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Select the last task in the current task list and set focus to the control
        /// </summary>
        public void SelectLastTaskWithFocus()
        {
            SelectLastTaskWithFocus(FCurrentTaskList);
        }

        /// <summary>
        /// Select the last task in the specified task list and set focus to the control
        /// </summary>
        private void SelectLastTaskWithFocus(TLstTasks ATaskList)
        {
            for (int i = 0; i < ATaskList.Controls.Count; i++)
            {
                TUcoTaskGroup group = (TUcoTaskGroup)ATaskList.Controls[i];

                for (int k = group.Controls[0].Controls.Count - 1; k >= 0; k--)
                {
                    TUcoSingleTask task = (TUcoSingleTask)group.Controls[0].Controls[k];

                    if (task.Enabled)
                    {
                        task.FocusTask();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Select the first task in the group after the current group and focus the control
        /// </summary>
        public void SelectNextGroupWithFocus()
        {
            if (FCurrentTaskList != null)
            {
                bool foundThisGroup = false;

                for (int i = FCurrentTaskList.Controls.Count - 1; i >= 0; i--)
                {
                    TUcoTaskGroup group = (TUcoTaskGroup)FCurrentTaskList.Controls[i];

                    if (group.Name == FCurrentTaskList.CurrentGroupName)
                    {
                        foundThisGroup = true;
                    }
                    else if (foundThisGroup)
                    {
                        for (int k = 0; k < group.Controls[0].Controls.Count; k++)
                        {
                            TUcoSingleTask task = (TUcoSingleTask)group.Controls[0].Controls[k];

                            if (task.Enabled)
                            {
                                task.FocusTask();
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Select the first task in the group before the current group and focus the control
        /// </summary>
        public void SelectPreviousGroupWithFocus()
        {
            if (FCurrentTaskList != null)
            {
                bool foundThisGroup = false;

                for (int i = 0; i < FCurrentTaskList.Controls.Count; i++)
                {
                    TUcoTaskGroup group = (TUcoTaskGroup)FCurrentTaskList.Controls[i];

                    if (group.Name == FCurrentTaskList.CurrentGroupName)
                    {
                        foundThisGroup = true;
                    }
                    else if (foundThisGroup)
                    {
                        for (int k = 0; k < group.Controls[0].Controls.Count; k++)
                        {
                            TUcoSingleTask task = (TUcoSingleTask)group.Controls[0].Controls[k];

                            if (task.Enabled)
                            {
                                task.FocusTask();
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The main override for handling command keys such as the arrow keys
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool foundTask = false;

            // Try and select the next task in the direction specified.  Will return false if the group does not have an enabled task in that direction.
            switch (keyData)
            {
                case Keys.Up | Keys.Control:
                    // Jump by a whole group
                    SelectPreviousGroupWithFocus();
                    return true;

                case Keys.Down | Keys.Control:
                    // Jump by a whole group
                    SelectNextGroupWithFocus();
                    return true;

                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    foundTask = FCurrentTaskList.Groups[FCurrentTaskList.CurrentGroupName].SelectNextTask(keyData);
                    break;

                case Keys.Tab:
                case Keys.Tab | Keys.Shift:
                    // The tab key does nothing when the task list has the focus
                    return true;
            }

            if (foundTask)
            {
                // we are done
                return true;
            }

            // We only need to keep looking if the arrow key was up or down
            if ((keyData == Keys.Up) || (keyData == Keys.Down))
            {
                // So now we will need to try the next group, if it exists.
                // Start by finding the index of the current group
                int curGroupIndex = -1;

                for (int i = 0; i < FCurrentTaskList.Controls.Count; i++)
                {
                    if (FCurrentTaskList.Controls[i].Name == FCurrentTaskList.CurrentGroupName)
                    {
                        curGroupIndex = i;
                        break;
                    }
                }

                // And get the current column in the current group
                int currentRow, currentColumn;
                FCurrentTaskList.Groups[FCurrentTaskList.CurrentGroupName].GetTaskLocation(out currentRow, out currentColumn);

                bool keepLooking = true;

                while (keepLooking)
                {
                    // We can try another group.  Group indexes are back to front (due to docking)
                    if ((keyData == Keys.Up) && (curGroupIndex < FCurrentTaskList.Groups.Count - 1))
                    {
                        foundTask = FCurrentTaskList.Groups[FCurrentTaskList.Controls[++curGroupIndex].Name].SelectNextTask(int.MaxValue,
                            currentColumn,
                            keyData);
                    }
                    else if ((keyData == Keys.Down) && (curGroupIndex > 0))
                    {
                        foundTask = FCurrentTaskList.Groups[FCurrentTaskList.Controls[--curGroupIndex].Name].SelectNextTask(0, currentColumn, keyData);
                    }

                    if (foundTask)
                    {
                        return true;
                    }

                    // we are out of groups if our index is at one end or the other
                    keepLooking = (curGroupIndex > 0 && curGroupIndex < FCurrentTaskList.Groups.Count - 1);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}