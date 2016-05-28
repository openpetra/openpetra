//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Groups Tasks (<see cref="TUcoSingleTask" />) into groups.
    /// </summary>
    /// <remarks>
    /// Each Task Group features a heading and a dividing line.
    /// Each Task Group contains a flpTaskGroup Control.
    /// Example in the OpenPetra Main Menu: 'Create Partner' Group in Partner Module, Partners Sub-Module.
    /// </remarks>
    public partial class TUcoTaskGroup : UserControl
    {
        private Dictionary <string, TUcoSingleTask>FTasks = new Dictionary <string, TUcoSingleTask>();
        private TaskAppearance FTaskAppearance;
        private bool FSingleClickExecution = false;
        private int FMaxTaskWidth;
        private TUcoSingleTask FCurrentTask = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoTaskGroup()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
            flpTaskGroup.Resize += new EventHandler(flpTaskGroup_Resize);
        }

        #region Properties

        /// <summary>
        /// Holds the Tasks that are to be displayed in the Task Group.
        /// </summary>
        private Dictionary <string, TUcoSingleTask>Tasks
        {
            get
            {
                return FTasks;
            }
        }

        /// <summary>
        /// Sets the Group Title.
        /// </summary>
        public string GroupTitle
        {
            get
            {
                return nlnGroupTitle.Caption;
            }

            set
            {
                nlnGroupTitle.Caption = value;
                flpTaskGroup.Name = value;                  // for debugging only...
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
                FTaskAppearance = value;

                foreach (var Task in Tasks)
                {
                    Task.Value.TaskAppearance = FTaskAppearance;
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
                }

                foreach (var Task in Tasks)
                {
                    Task.Value.SingleClickAnywhereMeansTaskClicked = FSingleClickExecution;
                }
            }
        }

        /// <summary>
        /// Maximum Task Width.
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

                    foreach (var Task in Tasks)
                    {
                        Task.Value.MaxTaskWidth = value;
                    }
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when a Task is clicked by the user.
        /// </summary>
        public event EventHandler TaskClicked;

        /// <summary>
        /// Fired when a Task is selected by the user (in a region of the Control where a TaskClick isn't fired).
        /// </summary>
        public event EventHandler TaskSelected;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a Task to the Task Group.
        /// </summary>
        /// <param name="ATaskname">Name of the Task to be added.</param>
        /// <param name="ATask">Instance of the Task to be added.</param>
        public void Add(string ATaskname, TUcoSingleTask ATask)
        {
            FTasks.Add(ATaskname, ATask);

            //	Add Task to this UserControls' Controls
            ATask.Margin = new Padding(5);
            flpTaskGroup.Controls.Add(ATask);

            ATask.TaskClicked += new EventHandler(FireTaskClicked);
            ATask.TaskSelected += new EventHandler(FireTaskSelected);
        }

        /// <summary>
        /// Removes all Tasks from the Task Group.
        /// </summary>
        public void Clear()
        {
            FTasks.Clear();

            flpTaskGroup.Controls.Clear();
        }

        /// <summary>
        /// Get the location (row and column) of a specified task.  The values will be -1 if no task is specified
        /// </summary>
        public void GetTaskLocation(TUcoSingleTask ATask, out int ARow, out int AColumn)
        {
            ARow = -1;
            AColumn = -1;

            if (ATask == null)
            {
                return;
            }

            ARow = ATask.Top / ATask.Height;

            if (FTaskAppearance == TaskAppearance.staLargeTile)
            {
                AColumn = ATask.Left / ATask.Width;
            }
            else
            {
                // it is list entry which does not have a fixed width
                foreach (TUcoSingleTask task in FTasks.Values)
                {
                    if ((task.Top == ATask.Top) && (task.Left <= ATask.Left))
                    {
                        AColumn++;
                    }
                }
            }
        }

        /// <summary>
        /// Get the location (row and column) of the current task
        /// </summary>
        public void GetTaskLocation(out int ARow, out int AColumn)
        {
            GetTaskLocation(FCurrentTask, out ARow, out AColumn);
        }

        /// <summary>
        /// Select the next enabled task in the group in the direction specified.  Returns true if a task could be selected.
        /// Returns false if there is no task or if there is a task but it is disabled.  Disabled tasks are skipped if there is an enabled task beyond.
        /// </summary>
        /// <param name="ADirection"></param>
        /// <returns></returns>
        public bool SelectNextTask(Keys ADirection)
        {
            if (FCurrentTask == null)
            {
                return false;
            }

            // Start by discovering the row and column of the current task in the group
            int currentRow, currentColumn;
            GetTaskLocation(out currentRow, out currentColumn);

            TUcoSingleTask nextTask = null;
            bool keepLooking = true;

            while (keepLooking)
            {
                // Find the next task at a specific location
                switch (ADirection)
                {
                    case Keys.Right:
                        nextTask = FindTaskAtLocation(currentRow, ++currentColumn);
                        break;

                    case Keys.Left:
                        nextTask = FindTaskAtLocation(currentRow, --currentColumn);
                        break;

                    case Keys.Up:
                        nextTask = FindTaskAtLocation(--currentRow, currentColumn);
                        break;

                    case Keys.Down:
                        nextTask = FindTaskAtLocation(++currentRow, currentColumn);
                        break;
                }

                if ((nextTask != null) && nextTask.Enabled)
                {
                    // we found one and it is enabled
                    nextTask.FocusTask();
                    return true;
                }

                // if we found a task that was disabled we can go round again
                keepLooking = (nextTask != null);
            }

            return false;
        }

        /// <summary>
        /// Select the next enabled task in the group in the direction specified and starting with the task in a specific row and column.
        /// Returns true if a task could be selected.
        /// Returns false if there is no task or if there is a task but it is disabled.  Disabled tasks are skipped if there is an enabled task beyond.
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AColumn"></param>
        /// <param name="ADirection"></param>
        /// <returns></returns>
        public bool SelectNextTask(int ARow, int AColumn, Keys ADirection)
        {
            TUcoSingleTask nextTask = null;
            bool keepLooking = true;

            while (keepLooking)
            {
                // get the task at the location
                nextTask = FindTaskAtLocation(ARow, AColumn);

                if (nextTask != null)
                {
                    if (nextTask.Enabled)
                    {
                        // we found one and it is enabled
                        nextTask.FocusTask();
                        return true;
                    }

                    // we found one but it was disabled - so try again in the specified direction
                    switch (ADirection)
                    {
                        case Keys.Up:
                            ARow--;
                            break;

                        case Keys.Down:
                            ARow++;
                            break;

                        case Keys.Left:
                            AColumn--;
                            break;

                        case Keys.Right:
                            AColumn++;
                            break;
                    }
                }

                keepLooking = (nextTask != null);
            }

            return false;
        }

        /// <summary>
        /// Sets the focus to the selected task
        /// </summary>
        /// <returns></returns>
        public bool FocusSelectedTask()
        {
            foreach (KeyValuePair <string, TUcoSingleTask>kvp in FTasks)
            {
                if (kvp.Value.IsSelected)
                {
                    kvp.Value.FocusTask();
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        private void FireTaskClicked(object sender, EventArgs e)
        {
            if (TaskClicked != null)
            {
                TaskClicked(sender, null);
            }
        }

        private void FireTaskSelected(object sender, EventArgs e)
        {
            // An individual task has been selected
            FCurrentTask = (TUcoSingleTask)sender;

            // This line makes sure it is completely visible
            ((TLstTasks) this.Parent).ScrollControlIntoView(FCurrentTask);

            if (TaskSelected != null)
            {
                TaskSelected(sender, null);
            }
        }

        private void TUcoTaskGroupResize(object sender, EventArgs e)
        {
            string ControlsList = "\r\n" + flpTaskGroup.Name + "\r\n";

            // Cause the FlowLayoutPanel to resize as well (stangely this doesn't happen automatically!)
            if (this.Width < MaxTaskWidth)
            {
//                this.Width = MaxTaskWidth;
            }
            else
            {
                flpTaskGroup.MaximumSize = new System.Drawing.Size(this.Width, 0);
            }

//TLogging.Log("TUcoTaskGroupResize: ucoTaskGroup " + Name + "'s size: " + Size.ToString());
//TLogging.Log("TUcoTaskGroupResize: flpTaskGroup " + Name + "'s size: " + flpTaskGroup.Size.ToString());

//			TLogging.Log("flpTaskGroup '" + flpTaskGroup.Name + "' FTasks.Count: " + FTasks.Count.ToString());
//TLogging.Log("flpTaskGroup '" + flpTaskGroup.Name + "' Controls.Count: " + flpTaskGroup.Controls.Count.ToString());

            foreach (Control SingleControl in flpTaskGroup.Controls)
            {
                ControlsList += SingleControl.Name + "; X: " + SingleControl.Location.X.ToString() + "; Y: " + SingleControl.Location.Y.ToString() +
                                "; Size: " + SingleControl.Size.ToString() + "\r\n";
            }

//TLogging.Log(ControlsList + "\r\n" + "\r\n");
        }

        /// <summary>
        /// Finds the task at the specified row and column.  Returns null if there is no task at the location requested.
        /// </summary>
        /// <param name="ARow">A 0-based row number.  If the value is int.MaxValue then the last task in the group in the specified column is returned.</param>
        /// <param name="AColumn">A 0-based column number</param>
        /// <returns></returns>
        private TUcoSingleTask FindTaskAtLocation(int ARow, int AColumn)
        {
            int maxRow = -1;
            TUcoSingleTask taskAtMaxRow = null;

            foreach (TUcoSingleTask task in FTasks.Values)
            {
                int tryRow, tryColumn;
                GetTaskLocation(task, out tryRow, out tryColumn);

                if (ARow == int.MaxValue)
                {
                    // we are looking for the last task in the group
                    if ((tryRow > maxRow) && (tryColumn == AColumn))
                    {
                        maxRow = tryRow;
                        taskAtMaxRow = task;
                    }
                }
                else if ((tryRow == ARow) && (tryColumn == AColumn))
                {
                    return task;
                }
            }

            if (taskAtMaxRow != null)
            {
                return taskAtMaxRow;
            }

            return null;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// We need to manually re-size the UserControl on the Resize event of the FlowLayoutPanel.
        /// The reason for that is that when the FlowLayoutPanel is in a UserControl, it doesn't re-size
        /// the UserControl and, consequently, we can't expect it to re-size the UserControl.
        /// If we don't manually re-size the UserControl, the TUcoTaskGroup UserControl doesn't 'shrink'
        /// back in height if the height that was needed to display a FlowLayoutPanel is reduced
        /// because the Main Menu form is enlarged.
        /// </summary>
        /// <param name="sender">Set by WinForms. Ignored.</param>
        /// <param name="e">Set by WinForms. Ignored.</param>
        void flpTaskGroup_Resize(object sender, EventArgs e)
        {
            this.Height = flpTaskGroup.Height + nlnGroupTitle.Height;

            if (flpTaskGroup.Width < MaxTaskWidth)
            {
                flpTaskGroup.Width = MaxTaskWidth;
//                this.Width = MaxTaskWidth;
            }

//TLogging.Log("flpTaskGroup_Resize: ucoTaskGroup " + Name + "'s size: " + Size.ToString());
//TLogging.Log("flpTaskGroup_Resize: flpTaskGroup " + Name + "'s size: " + flpTaskGroup.Size.ToString());
        }

// TODO: Try to prevent the UserControl from shrinking below MaxTaskWidth to allow showing of horizontal scrollbar in the Tasks List. The code below achieves the shrink-stopping, but introduces undesired side effects. Needs more investigation...
//		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
//		{
//            // Set a fixed width for the control.
//            // ADD AN EXTRA HEIGHT VALIDATION TO AVOID INITIALIZATION PROBLEMS
//            // BITWISE 'AND' OPERATION: IF ZERO THEN HEIGHT IS NOT INVOLVED IN THIS OPERATION
//            if ((specified&BoundsSpecified.Width) == 0 || width == MaxTaskWidth)
//            {
//                  if (width < MaxTaskWidth)
//                  {
//TLogging.Log("SetBoundsCore: Before setting ucoTaskGroup " + Name + "'s Width to " + MaxTaskWidth.ToString() + ": Size = " + Size.ToString());
//                    base.SetBoundsCore(x, y, MaxTaskWidth, height, specified);
//TLogging.Log("SetBoundsCore: After setting ucoTaskGroup " + Name + "'s Width to " + MaxTaskWidth.ToString() + ": Size = " + Size.ToString());
//                }
//		    }
//		    else
//		    {
//                return;
//		    }
//TLogging.Log("SetBoundsCore: ucoTaskGroup " + Name + "'s size: " + Size.ToString());
//		}

        #endregion
    }
}