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

                    if (this.Controls.Count > 0)
                    {
                        ((TLstTasks) this.Controls[0]).MaxTaskWidth = FMaxTaskWidth;
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
                        ((TLstTasks) this.Controls[0]).TaskAppearance = FTaskAppearance;
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
                        ((TLstTasks) this.Controls[0]).SingleClickExecution = FSingleClickExecution;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows a <see cref="TLstTasks" /> Control.
        /// </summary>
        /// <param name="ATaskList">The <see cref="TLstTasks" /> Control that should be shown. 
        /// If that Control was alreay shown it is not instantiated again but just brought to the foreground,
        /// if it wasn't shown before then an instance of it is created automatically.</param>
        public void ShowTaskList(TLstTasks ATaskList)
        {
            if (ATaskList != null)
            {
                if (FTaskLists.Contains(ATaskList))
                {
//TLogging.Log("Found TaskList '" + ATaskList.Name + "' - bringing it to front.");
                    ATaskList.MaxTaskWidth = FMaxTaskWidth;
                    ATaskList.TaskAppearance = FTaskAppearance;
                    ATaskList.SingleClickExecution = FSingleClickExecution;
                    ATaskList.BringToFront();
                }
                else
                {
//TLogging.Log("Couldn't find TaskList '" + ATaskList.Name + "' - adding it.");
                    this.Controls.Add(ATaskList);
                    ATaskList.MaxTaskWidth = FMaxTaskWidth;
                    ATaskList.TaskAppearance = FTaskAppearance;
                    ATaskList.SingleClickExecution = FSingleClickExecution;
                    ATaskList.BringToFront();

                    FTaskLists.Add(ATaskList);
                }
            }
        }

        #endregion
    }
}