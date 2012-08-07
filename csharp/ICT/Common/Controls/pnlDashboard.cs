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
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// the Dashboard can host several other panels
    /// each panel has a caption, and can be minimized
    /// TODO: this is only implemented to host the tasklist at the moment, and does not host panels yet
    /// </summary>
    public class TDashboard : System.Windows.Forms.Panel
    {
    	private int FMaxTaskWidth;
    	private TaskAppearance FTaskAppearance = TaskAppearance.staLargeTile;
    	private bool FSingleClickExecution = false;
    	private Dictionary<string, TLstTasks> FTaskLists = new Dictionary<string, TLstTasks>();

        /// <summary>
        /// default constructor
        /// </summary>
        public TDashboard()
        {
        	this.Padding = new System.Windows.Forms.Padding(5,3,5,3);
        	this.AutoScroll = true;
//        	this.BackColor = System.Drawing.Color.Green;            // for debugging only...
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
                    
                    if (this.Controls.Count > 0) 
                    {
                        ((TLstTasks)this.Controls[0]).MaxTaskWidth = FMaxTaskWidth;    
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
                    
                    if (this.Controls.Count > 0) 
                    {
                        ((TLstTasks)this.Controls[0]).TaskAppearance = FTaskAppearance;
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
                    
                    if (this.Controls.Count > 0) 
                    {
                        ((TLstTasks)this.Controls[0]).SingleClickExecution = FSingleClickExecution;
                    }                    
                }
            }
       }
    	
       #endregion

        #region Public Methods
        
        /// <summary>
        /// quick implementation of hosting task list
        /// TODO: this needs to support several panels etc
        /// </summary>
        /// <param name="ATaskList"></param>
        public void ShowTaskList(TLstTasks ATaskList)
        {
            TLstTasks ExistingTaskList;
            
            if (ATaskList != null)
            {
                if (FTaskLists.TryGetValue(ATaskList.Name, out ExistingTaskList))
                {
//TLogging.Log("Found TaskList '" + ATaskList.Name + "' - bringing it to front.");
                    ExistingTaskList.MaxTaskWidth = FMaxTaskWidth;
                    ExistingTaskList.TaskAppearance = FTaskAppearance;
                    ExistingTaskList.BringToFront();
                }
                else
                {
//TLogging.Log("Couldn't find TaskList '" + ATaskList.Name + "' - adding it.");                
                    this.Controls.Add(ATaskList);
                    ATaskList.MaxTaskWidth = FMaxTaskWidth;
                    ATaskList.TaskAppearance = FTaskAppearance;
                    ATaskList.BringToFront();
                                        
                    FTaskLists.Add(ATaskList.Name, ATaskList);
                }
            }
        }
        
        #endregion
    }
}