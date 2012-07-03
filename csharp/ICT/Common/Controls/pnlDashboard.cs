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
                FMaxTaskWidth = value;
                
                ((TLstTasks)this.Controls[0]).MaxTaskWidth = FMaxTaskWidth;
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
                
                ((TLstTasks)this.Controls[0]).TaskAppearance = FTaskAppearance;                
            }
        }
    	        
        /// <summary>
        /// default constructor
        /// </summary>
        public TDashboard()
        {
        	this.Padding = new System.Windows.Forms.Padding(5,3,5,3);
        	this.AutoScroll = true;
        	this.BackColor = System.Drawing.Color.White;            
        }

        /// <summary>
        /// quick implementation of hosting task list
        /// TODO: this needs to support several panels etc
        /// </summary>
        /// <param name="ATaskList"></param>
        public void ReplaceTaskList(TLstTasks ATaskList)
        {
            if (this.Controls.Count > 0)
            {
                this.Controls.RemoveAt(0);
            }

            if (ATaskList != null)
            {
                this.Controls.Add(ATaskList);
            }
        }
    }
}