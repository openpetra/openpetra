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
    /// Controls the appearance of individual Tasks.
    /// </summary>
    public enum TaskAppearance
    {
        /// <summary>Display the Task as a Tile (incl. Task Description)</summary>
        staLargeTile,
        
        /// <summary>Display the Task as a Liste Entry (excl. Task Description)</summary>
        staListEntry
    }
    
    /// <summary>
    /// Single Task of many in a Task List.
    /// </summary>
    public partial class TUcoSingleTask : UserControl
    {
        bool FTaskSelected = false;
        bool FTaskIndentedIfNoTaskImage = false;
        bool FSingleClickAnywhereMeansTaskClicked = false;
        int FMaxTaskWidth;
        int FMaxTaskHeight;
        TUcoTaskGroup FTaskGroup;
        
        TaskAppearance FSingleTaskAppearance = TaskAppearance.staLargeTile;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoSingleTask()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            FMaxTaskWidth = this.Width;
            FMaxTaskHeight = this.Height;
        }
        
        /// <summary>
        /// The Task Group the Task belongs to.
        /// </summary>
        public TUcoTaskGroup TaskGroup
        {
        	get
        	{
        		return FTaskGroup;
        	}
        	
        	set
        	{
        		FTaskGroup = value;
        	}
        }
        
        /// <summary>
        /// Title of the Task. (Shown as a LinkLabel.)
        /// </summary>
        public string TaskTitle
        {
            get
            {
                return llbTaskTitle.Text;
            }
            
            set
            {
                llbTaskTitle.Text = value;
            }
        }
        
        /// <summary>
        /// Description of the Task. (Shown under the TaskTitle LinkLabel in staLargeTile appearance).
        /// </summary>
        public string TaskDescription
        {
            get
            {
                return lblTaskDescription.Text;
            }
            
            set
            {
                lblTaskDescription.Text = value;
            }
        }

        /// <summary>
        /// Appearance of the Task (Large Tile, ListEntry).
        /// </summary>
        public TaskAppearance TaskAppearance
        {
            get
            {
                return FSingleTaskAppearance;
            }
            
            set
            {
                FSingleTaskAppearance = value;
                
                if(FSingleTaskAppearance == TaskAppearance.staLargeTile)
                {
                    lblTaskDescription.Visible = true;
                    llbTaskTitle.Font = new Font(llbTaskTitle.Font.FontFamily, 9.0f, FontStyle.Regular);
                    
               		// In LargeTile appearance the items should all have the same width
            		Size = new Size(MaxTaskWidth, MaxTaskHeight);
                }
                else
                {
                    lblTaskDescription.Visible = false;
                    llbTaskTitle.Font = new Font(llbTaskTitle.Font.FontFamily, 8.0f, FontStyle.Regular);
					
                    // In ListEntry appearance the items should only be as wide as they need
                	Size = RequiredSize;	
                }

                UpdateLayout();
            }
        }
        
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
//            	if ((FMaxTaskWidth == 0) 
//            	    || (FMaxTaskWidth > value))
//            	{
            		Width = value;
            		
            		UpdateLayout();
//            	}
            	
                FMaxTaskWidth = value;
            }
        }
        
        /// <summary>
        /// Maximum Task Height.
        /// </summary>
        public int MaxTaskHeight
        {
            get
            {
                return FMaxTaskHeight;
            }
            
            set
            {
                FMaxTaskHeight = value;
            }
        }
                
        /// <summary>
        /// Icon to be displayed left of the Task Title LinkLabel.
        /// </summary>
        public Image TaskImage
        {
            get
            {
                return pnlBackground.Image;
            }
            
            set
            {
                pnlBackground.Image = value;

                if (!FTaskIndentedIfNoTaskImage)
                {
                    UpdateLayout();
                    pnlIconSpacer.Visible = pnlBackground.Image != null;
                }
                else
                {
                    pnlIconSpacer.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// Indent Task Title LinkLabel if no Task Image is displayed?
        /// </summary>
        public bool TaskIndententIfNoTaskImage
        {
            get
            {
                return FTaskIndentedIfNoTaskImage;
            }
            
            set
            {
                FTaskIndentedIfNoTaskImage = value;
                
                if (!FTaskIndentedIfNoTaskImage)
                {
                    pnlIconSpacer.Visible = pnlBackground.Image != null;
                }
                else
                {
                    pnlIconSpacer.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// Set to true if a single click anywhere in the Control should cause a 
        /// <see cref="TaskClick" /> Event to be fired.
        /// </summary>
        public bool SingleClickAnywhereMeansTaskClicked
        {
            get
            {
                return FSingleClickAnywhereMeansTaskClicked;
            }
            
            set
            {
                FSingleClickAnywhereMeansTaskClicked = value;
            }
        }
        
        /// <summary>
        /// Required Size.
        /// </summary>
        public Size RequiredSize
        {
            get
            {
                int RequiredWidth;
                int RequiredHeight;
                
                if(FSingleTaskAppearance == TaskAppearance.staLargeTile)
                {
                    RequiredWidth = pnlIconSpacer.Width + GetTitleLength().Width + 8;
                    RequiredHeight = 54;
                }
                else
                {
                    RequiredWidth = pnlIconSpacer.Width + GetTitleLength().Width + 5;
                    RequiredHeight = 24;
                }
                
                if (RequiredWidth > FMaxTaskWidth) 
                {
                    RequiredWidth = FMaxTaskWidth;
                }
                
                return new Size (RequiredWidth, RequiredHeight);
            }
        }
        
        /// <summary>
        /// Selects (highlights) the Task.
        /// </summary>
        public void SelectTask()
        {
            pnlBackground.BorderColor = System.Drawing.Color.FromArgb(125,162,206);

            pnlBackground.GradientStartColor = System.Drawing.Color.FromArgb(220,235,252);
            pnlBackground.GradientEndColor = System.Drawing.Color.FromArgb(193,219,252);
            
            FTaskSelected = false;
        }
        
        /// <summary>
        /// Deselectes (removes highlighting) of the Task.
        /// </summary>
        public void DeselectTask()
        {
            pnlBackground.BorderColor = System.Drawing.Color.Transparent;
            
            pnlBackground.GradientStartColor = System.Drawing.Color.Transparent;
            pnlBackground.GradientEndColor = System.Drawing.Color.Transparent;
            
            FTaskSelected = false;
        }
        
        /// <summary>
        /// Fired when the Task is clicked by the user.
        /// </summary>
        public event EventHandler TaskClicked;
        
        /// <summary>
        /// Fired when the Task is selected by the user (in a region of the Control where a TaskClick isn't fired).
        /// </summary>
        public event EventHandler TaskSelected;
        
        void LlbTaskTitleLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            FireTaskClicked();
        }

        void FireTaskClicked()
        {
            if (TaskClicked != null) {
                TaskClicked(this, null);
            }
        }
        
        void TaskClick(object sender, EventArgs e)
        {
            SelectTask();

            FTaskSelected = true;
            
            if (TaskSelected != null)
            {
                TaskSelected(this, null);
            }
            
            if(FSingleClickAnywhereMeansTaskClicked)
            {
                if((sender == lblTaskDescription)
                   || (sender == pnlIconSpacer)
                   || (sender == pnlBackground))
                {
                    FireTaskClicked();
                }
            }
        }
        
        void TaskTitleMouseEnter(object sender, EventArgs e)
        {
            llbTaskTitle.LinkColor = System.Drawing.Color.DarkBlue;
            
            TaskMouseEnter(sender, e);
        }
        
        void TaskTitleMouseLeave(object sender, EventArgs e)
        {
            llbTaskTitle.LinkColor = System.Drawing.Color.Black;
            
            TaskMouseLeave(sender, e);
        }
        
        void TaskMouseEnter(object sender, EventArgs e)
        {
            pnlBackground.BorderColor = System.Drawing.Color.FromArgb(184,214,152);
        }
        
        void TaskMouseLeave(object sender, EventArgs e)
        {
            if (!FTaskSelected)
            {
                pnlBackground.BorderColor = System.Drawing.Color.Transparent;
            }
        }
        
        private void UpdateLayout()
        {
            if(FSingleTaskAppearance == TaskAppearance.staLargeTile)
            {
                pnlBackground.ImageLocation = new Point(8,11);
                
                pnlIconSpacer.Width = pnlBackground.Image.Width + 12;
                pnlIconSpacer.Height = pnlBackground.Image.Height + 12;
                
                llbTaskTitle.Padding = new Padding(0,2,0,0);
            }
            else
            {
                pnlBackground.ImageLocation = new Point(5,4);
                
                pnlIconSpacer.Width = pnlBackground.Image.Width + 5;
                pnlIconSpacer.Height = pnlBackground.Image.Height + 8;
                
                llbTaskTitle.Padding = new Padding(0,3,0,0);
            }
        }
        
        private Size GetTitleLength()
        {
            return TextRenderer.MeasureText(llbTaskTitle.Text, llbTaskTitle.Font);
        }
        
        void DoubleClickAnywhere(object sender, EventArgs e)
        {
            FireTaskClicked();
        }
    }
}