//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Testing.TxtPetraDate
{
/// <summary>
/// Description of MainForm.
/// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            new TLogging();
            TLogging.DebugLevel = 1;
            TLogging.SetStatusBarProcedure(WriteLogging);
        }

        private void WriteLogging(string s)
        {
            textBox1.Text += DateTime.Now.ToLongTimeString() + s +
                             Environment.NewLine;
        }

        private void DateChangedHandler(object sender, EventArgs e)
        {
            TLogging.Log("DateChangedHandler");
        }

        private void ControlValidatedHandler(object sender, EventArgs e)
        {
            TLogging.Log("ControlValidatedHandler");

            // simulate GetDetailsFromControls
            TLogging.Log((dtpDetailGlEffectiveDate.Date.HasValue ? dtpDetailGlEffectiveDate.Date.Value : DateTime.MinValue).ToString());
        }

        void BtnAssignInvalidDateClick(object sender, System.EventArgs e)
        {
            dtpDetailGlEffectiveDate.Text = "30";
        }
    }
}