//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Tools.OpenPetraWebServer
{
    public partial class AboutMe : Form
    {
        private bool _parentIsVisible = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ParentIsVisible"></param>
        public AboutMe(bool ParentIsVisible)
        {
            _parentIsVisible = ParentIsVisible;

            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutMe_Load(object sender, EventArgs e)
        {
            if (_parentIsVisible)
            {
                // Place the window on top of the main window
                this.Left = Owner.Left + 10;
                this.Top = Owner.Top + 10;
            }
            else
            {
                // Place the window at the bottom right of the screen, near the system tray
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 50;
                this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 50;
            }

            VersionLabel.Text = "Version " + Program.FileVersion;
        }
    }
}