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
        private TUcoSingleTask FirstTaskInGroup = null;

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

            if (FTasks.Count == 1)
            {
                FirstTaskInGroup = ATask;
            }
        }

        /// <summary>
        /// Removes all Tasks from the Task Group.
        /// </summary>
        public void Clear()
        {
            FTasks.Clear();

            flpTaskGroup.Controls.Clear();
            FirstTaskInGroup = null;
        }

        /// <summary>
        /// Selects (highlights) the task that was first added to a TaskGroup.
        /// </summary>
        public void SelectFirstTask()
        {
            if (FTasks.Count > 0)
            {
                FirstTaskInGroup.SelectTask();
            }
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