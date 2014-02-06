//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This class is a collapsible groupbox.
    /// there should be a tooltip for minimizing/maximizing control
    /// there should be a subcaption that can have different background color
    /// there should be a keyboard shortcut for maximizing/minimizing the groupbox
    /// This will be implemented later.
    /// </summary>
    public class TGrpCollapsible : UserControl
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TGrpCollapsible()
        {
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// set the caption for the control
        public string Caption
        {
            set
            {
                this.Text = value;
            }
        }

#if TODO        
        /// the sub caption can be highlighted
        public string SubCaption
        {
            set
            {
                // TODO
            }
        }

        /// <summary>
        /// define if sub caption should be highlighted
        /// </summary>
        public bool SubCaptionHighlighted
        {
            set
            {
                // TODO
            }
        }

        /// <summary>
        /// is it collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get
            {
                // TODO
                return false;
            }
        }
#endif

        /// <summary>
        /// Event when box collapses or expands
        /// </summary>
        public event CollapsingEventHandler CollapsingEvent;

        /// <summary>
        /// function that collapses the box
        /// </summary>
        public void Collapse()
        {
            // TODO
            this.CollapsingEvent(this, null);
        }

        /// expand the control
        public void Expand()
        {
            // TODO
        }
    }

    /// <summary>
    /// this event is triggered when the box collapses
    /// </summary>
    public delegate void CollapsingEventHandler(object sender, CollapsibleEventArgs e);

    /// <summary>
    /// dummy class for further implementation
    /// </summary>
    public class CollapsibleEventArgs : EventArgs
    {
#if TODO        
        /// <summary>
        /// cancel the collapsing
        /// </summary>
        public bool Cancel
        {
            get
            {
                // TODO
                return true;
            }            
            set
            {
                // TODO
            }
        }

        /// <summary>
        /// will it collapse or expand?
        /// </summary>
        public bool WillCollapse
        {
            get
            {
                // TODO
                return false;
            }
        }
#endif        
    }    
}