//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Tools.OpenPetraWebServer
{
    /// <summary>
    /// The Form class that displays the server log text.
    /// </summary>
    public partial class ServerLogForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServerLogForm()
        {
            InitializeComponent();
        }

        private void RefreshLog()
        {
            // We check to see if we can append text to the end of the log
            // We can do this so long as the log did not get too long, so the server truncated it
            int currentLength = txtLog.Text.Length;
            string newText = TLogging.GetConsoleLog();

            // check if the caret is at the end - the user may have scrolled up to look at something
            int currentPos = txtLog.SelectionStart;
            int currentSelLength = txtLog.SelectionLength;
            bool caretIsAtEnd = (currentPos == currentLength);

            if (newText.Length >= currentLength)
            {
                txtLog.Text += newText.Substring(currentLength);
            }
            else
            {
                // It must all be new
                txtLog.Text = newText;
                caretIsAtEnd = true;
            }

            if (caretIsAtEnd)
            {
                // If the caret was at the end we keep it at the end and scroll the end into view
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
            else
            {
                txtLog.SelectionStart = currentPos;
                txtLog.SelectionLength = currentSelLength;
            }
        }

        private void ServerLogForm_Activated(object sender, EventArgs e)
        {
            RefreshLog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ServerLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Call this when the server log text has changed
        /// </summary>
        public void OnNewLogMessage()
        {
            if (this.Visible)
            {
                RefreshLog();
            }
        }
    }
}