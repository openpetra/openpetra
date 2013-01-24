//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AustinS, ChristianK
//
// Copyright 2004-2013 by OM International
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
using System;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms.Logic;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// The base GUI for all Petra Shepherd Forms: elements will be present in every Shepherd Page.
    /// </summary>
    public partial class TPetraShepherdForm : Form
    {
////        TPetraShepherdFormLogic FLogic;  TODO

        #region Properties
        
        /// <summary>
        /// Collapsible Navigation.
        /// </summary>
        public TPnlCollapsible CollapsibleNavigation
        {
            get
            {
                return pnlCollapsibleNavigation;
            }
        }
        
        /// <summary>
        /// StatusBar of the Shepherd.
        /// </summary>
        public TExtStatusBarHelp ShepherdStatusBar
        {
            get
            {
                return stbMain;
            }
        }
        
        /// <summary>
        /// Label for textual displaying of the Page Progress.
        /// </summary>
        public Label PageProgressLabel
        {
            get
            {
                return lblPageProgress;
            }
        }
        
        /// <summary>
        /// ProgressBar for visual displaying of the Page Progress.
        /// </summary>
        public ProgressBar PageProgressProgressBar
        {
            get
            {
                return prbPageProgress;
            }
        }
        
        /// <summary>
        /// ContentPanel (<c>pnlContent</c> of the Shepherd).
        /// </summary>
        public Panel ContentPanel
        {
            get
            {
                return pnlContent;
            }
        }
        
        /// <summary>
        /// Navigation Panel.
        /// </summary>
        public Panel NavigationPanel
        {
            get
            {
                return pnlNavigation;
            }
        }
        
        /// <summary>
        /// Finish Button.
        /// </summary>
        public Button ButtonFinish
        {
            get
            {
                return btnFinish;
            }
        }
        
        /// <summary>
        /// Next Button.
        /// </summary>
        public Button ButtonNext
        {
            get
            {
                return btnNext;
            }
        }
        
        /// <summary>
        /// Back Button.
        /// </summary>
        public Button ButtonBack
        {
            get
            {
                return btnBack;
            }
        }
        
        /// <summary>
        /// Heading #1.
        /// </summary>
        public Label Heading1
        {
            get
            {
                return lblHeading1;
            }
        }
        
        /// <summary>
        /// Heading #2.
        /// </summary>
        public Label Heading2
        {
            get
            {
                return lblHeading2;    
            }
        }
        
        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TPetraShepherdForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Virtual Method: implementor should handle action for Finish button click.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void BtnFinishClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual Method: implementor should handle action for Next button click.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void BtnNextClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual Method: implementor should handle action for Back button click.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void BtnBackClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual Method: implementor should handle action for Cancel button click.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void BtnCancelClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual Method: implementor should handle action for Help button click.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void BtnHelpClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual Method: implementor should handle the loading of the current form.
        /// </summary>
        /// <param name="sender">Sending Control (supplied by WinForms).</param>
        /// <param name="e">Event Arguments (supplied by WinForms).</param>
        protected virtual void Form_Load(object sender, EventArgs e)
        {
        }

        #endregion
    }
}