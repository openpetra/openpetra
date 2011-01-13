//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
	
	public enum TCollapseDirection
	{
		cdVertical,
		cdHorizontal
	}
	
    /// <summary>
    /// UserControl which acts as a 'Collapsible Panel'.
    /// </summary>
    public partial class TPnlCollapsible : System.Windows.Forms.UserControl
    {
        #region Private constants and fields

        /// <summary>Hard-coded value of the collapsed height</summary>
        private const Int16 COLLAPSEDHEIGHT = 29;

        /// <summary>Hard-coded value of the expanded height</summary>
        private const Int16 EXPANDEDHEIGHT = 153;

        /// <summary>Keeps track of the collapsed/expanded state</summary>
        private bool FIsCollapsed = false;

        /// <summary>Caches the translated text for several ToolTips</summary>
        private string FToolTipText;
        
        /// <summary>Hard-coded value of the collapsed width</summary>
        private const Int16 COLLAPSEDWIDTH = 29;

        /// <summary>Hard-coded value of the expanded width</summary>
        private const Int16 EXPANDEDWIDTH = 153;

        #endregion


        #region Events

        /// <summary>Event is fired after the panel has collapsed.</summary>
        public event System.EventHandler Collapsed;

        /// <summary>Event is fired after the panel has expanded.</summary>
        public event System.EventHandler Expanded;

        #endregion


        #region Properties

        /// <summary>
        /// Sets the Title Text.
        /// </summary>
        public override string Text
        {
            get
            {
                return FToolTipText;
            }

            set
            {
                string NewToolTipText;

                // Set Title Text
                lblDetailHeading.Text = value;
                NewToolTipText = String.Format(FToolTipText, value);

                // Update ToolTips
                this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, NewToolTipText);
                this.tipCollapseExpandHints.SetToolTip(this.pnlTitleText, NewToolTipText);
                this.tipCollapseExpandHints.SetToolTip(this.btnToggle, NewToolTipText);
            }
        }

        /// <summary>
        /// Sets the Collapse Direction.
        /// </summary>
        public TCollapseDirection CollapseDirection
        {
            get
            {
                ;
            }

            set
            {
                ;
            }
        }
        
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public TPnlCollapsible()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            FToolTipText = Catalog.GetString("Click here to expand / collapse the {0} panel");
            this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, FToolTipText);
            #endregion

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>True if the panel is collapsed, otherwise false.</summary>
        public bool IsCollapsed
        {
            get
            {
                return FIsCollapsed;
            }

            set
            {
                FIsCollapsed = value;
            }
        }

        #region Public Methods

        /// <summary>
        /// Causes the panel to collapse. Only the Title Panel will be visible after that.
        /// </summary>
        public void Collapse()
        {
            FIsCollapsed = true;
            btnToggle.ImageIndex = 1;
            
            if(CollapseDirection == cdVertical)
            {
                this.Height = COLLAPSEDHEIGHT;
            }
            else
            {
                this.Width = COLLAPSEDWIDTH;
            }
        }

        /// <summary>
        /// Causes the panel to expand. The Title Panel and the Content Panel will be visible after that.
        /// </summary>
        public void Expand()
        {
            FIsCollapsed = false;
            btnToggle.ImageIndex = 0;

            if(CollapseDirection == cdVertical)
            {
                this.Height = EXPANDEDHEIGHT;
            }
            else
            {
            	this.Width = EXPANDEDWIDTH;
            }
            
        }

        #endregion


        #region Helper Methods

        /// <summary>
        /// Raises the 'Collapsed' Event if something subscribed to it.
        /// </summary>
        private void OnCollapsed()
        {
            if (Collapsed != null)
            {
                Collapsed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the 'Expanded' Event if something subscribed to it.
        /// </summary>
        private void OnExpanded()
        {
            if (Expanded != null)
            {
                Expanded(this, new EventArgs());
            }
        }
        
        private void ChangeDirection()
        {
        	//The set of four Icons with arrows for collapsing/expanding need to be swapped with the other set of four icons
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event is raised if the Toggle Button is clicked.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        private void BtnToggleClick(object sender, EventArgs e)
        {
            if (FIsCollapsed)
            {
                Expand();
                OnExpanded();
            }
            else
            {
                Collapse();
                OnCollapsed();
            }
        }

        /// <summary>
        /// Event is raised when the mouse enters the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseEnter(object sender, EventArgs e)
        {
            TLogging.Log("BtnToggleMouseEnter: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());

            if (btnToggle.ImageIndex == 0)
            {
                btnToggle.ImageIndex = 3;
            }
            else
            {
                btnToggle.ImageIndex = 2;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.RoyalBlue;      // RoyalBlue;  SteelBlue     // Blue
            TLogging.Log("BtnToggleMouseEnter END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        /// <summary>
        /// Event is raised if the mouse leaves the Toggle Button.
        /// </summary>
        /// <param name="sender">The Toggle Button.</param>
        /// <param name="e">Not evaluated.</param>
        void BtnToggleMouseLeave(object sender, EventArgs e)
        {
            TLogging.Log("BtnToggleMouseLeave: btnTogglePartnerDetails.ImageIndex: " + btnToggle.ImageIndex.ToString());

            if (btnToggle.ImageIndex == 3)
            {
                btnToggle.ImageIndex = 0;
            }
            else if (btnToggle.ImageIndex != 0)
            {
                btnToggle.ImageIndex = 1;
            }

            lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;  // Blue     // DarkBlue
            TLogging.Log("BtnToggleMouseLeave END: btnToggle.ImageIndex: " + btnToggle.ImageIndex.ToString());
        }

        #endregion
    }
}