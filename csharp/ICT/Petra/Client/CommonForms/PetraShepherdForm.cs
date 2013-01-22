//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AustinS, ChristianK
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
using System;
using System.Drawing;
using System.Windows.Forms;

using Ict.Petra.Client.CommonForms.Logic;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// The base GUI for all Petra Shepherd Forms: elements will be present in every Shepherd Page
    /// </summary>
    public partial class TPetraShepherdForm : Form
    {
//        TPetraShepherdFormLogic FLogic;  TODO

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

        #region ButtonEvents
        /// <summary>
        /// Virtual function: implementor should handle action for Finish button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnFinishClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual function: implementor should handle action for Next button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnNextClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual function: implementor should handle action for Back button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnBackClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual function: implementor should handle action for Cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnCancelClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual function: implementor should handle action for Help button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnHelpClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Virtual function: implementor should handle the loading of the current form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Form_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// TODO: This is a temporary Action: we are using this to test the page switching functionality
        /// of the shepherd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox1TextChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}