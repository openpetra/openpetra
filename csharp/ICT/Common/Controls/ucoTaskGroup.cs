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
	/// Groups Tasks.
	/// </summary>
	public partial class TUcoTaskGroup : UserControl
	{
        private Dictionary<string, TUcoSingleTask> FTasks = new Dictionary<string, TUcoSingleTask>();
		private TaskAppearance FTaskAppearance;
		private int FMaxTaskWidth;
		
		/// <summary>
		/// Holds the Tasks that are to be displayed in the Task Group.
		/// </summary>
        private Dictionary<string, TUcoSingleTask> Tasks
        {
        	get
        	{
        		return FTasks;
        	}
        }

        /// <summary>
        /// Fired when a Task is clicked by the user.
        /// </summary>
        public event EventHandler TaskClicked;
        
        /// <summary>
        /// Fired when a Task is selected by the user (in a region of the Control where a TaskClick isn't fired).
        /// </summary>
        public event EventHandler TaskSelected;


        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoTaskGroup()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
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
                FMaxTaskWidth = value;
                
                foreach (var Task in Tasks) 
                {
                	Task.Value.MaxTaskWidth = value;                	                	
                }                
            }
        }
        
        /// <summary>
        /// Adds a Task to the Task Group.
        /// </summary>
        /// <param name="ATaskname">Name of the Task to be added.</param>
        /// <param name="ATask">Instance of the Task to be added.</param>
		public void Add(string ATaskname, TUcoSingleTask ATask)
		{
			FTasks.Add(ATaskname, ATask);
		
			// Add Task to this UserControls' Controls			
			ATask.Dock = DockStyle.Top;
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

        private void FireTaskClicked(object sender, EventArgs e)
        {
            if (TaskClicked != null) {
                TaskClicked(sender, null);
            }
        }

        private void FireTaskSelected(object sender, EventArgs e)
        {
            if (TaskSelected != null) {
                TaskSelected(sender, null);
            }
        }
		
		private void TUcoTaskGroupResize(object sender, EventArgs e)
		{
		    // Cause the FlowLayoutPanel to resize as well (stangely this doesn't happen automatically!)
			flpTaskGroup.MaximumSize = new System.Drawing.Size(this.Width, 0);			
		}
	}
}
