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
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Contains Extension Methods that add functionality to Controls.
    /// </summary>
    public static class TControlExtensions
    {
        /// <summary>
        /// Finds the UserControl that hosts <paramref name="AControl" />.
        /// </summary>
        /// <param name="AControl">Control to find the hosting UserControl for.</param>
        /// <param name="AIfControlIsUserControlReturnThis">Set to true if the Control that is
        /// passed is a UserControl and this Method should return it instead of searching for
        /// a UserControl that this UserControl is hosted in.</param>
        /// <param name="AIfControlIsTabControlReturnChildUserControl">Set to true if the Control that is
        /// passed is a TabPage and this Method should return the UserControl that is placed on it 
        /// instead of searching for a UserControl that this TabPage is hosted in.</param>
        /// <returns>The UserControl that hosts <paramref name="AControl" /> or null
        /// in case no hosting UserControl could be found.</returns>
        public static object FindUserControlOrForm(this Control AControl, bool AIfControlIsUserControlReturnThis = false, 
            bool AIfControlIsTabControlReturnChildUserControl = false)
        {
            Control ControlSoughtFor;
            
            if (AControl == null) 
            {
                return null;    
            }
            
            ControlSoughtFor = AControl.Parent;
            
            if ((AControl is UserControl)
               && AIfControlIsUserControlReturnThis) 
            {
                return AControl;    
            }
            
            if (!((AControl is System.Windows.Forms.TabControl)
                  && (AIfControlIsTabControlReturnChildUserControl)))
            {
                // Iterate through all Parent Controls to find a UserControl, failing that a Form
                while((!(ControlSoughtFor is UserControl))
                    && (!(ControlSoughtFor is Form)))
                {
                    ControlSoughtFor = ControlSoughtFor.Parent;
                }                
            }
            else
            {
                // Check if there is only one Child Control on the currently selected TabPage and 
                // that this Child Control is indeed a UserControl
                if ((((TabControl)AControl).SelectedTab.Controls.Count == 1)
                    && (((TabControl)AControl).SelectedTab.Controls[0] is UserControl))
                {
                    ControlSoughtFor = ((TabControl)AControl).SelectedTab.Controls[0];
                }
                else
                {
                    ControlSoughtFor = null;
                }
            }
            
            if (ControlSoughtFor != AControl) 
            {
                return ControlSoughtFor;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the UserControl that hosts <paramref name="AControl" />.
        /// </summary>
        /// <param name="AControl">Control to find the hosting UserControl for.</param>
        /// <returns>The UserControl that hosts <paramref name="AControl" /> or null
        /// in case no hosting UserControl could be found.</returns>
        public static UserControl FindUserControl(this Control AControl)
        {
            object FindResult = FindUserControlOrForm(AControl);
            
            if (FindResult != null)
            {
                if (FindResult is UserControl) 
                {
                    return (UserControl)FindResult;    
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}