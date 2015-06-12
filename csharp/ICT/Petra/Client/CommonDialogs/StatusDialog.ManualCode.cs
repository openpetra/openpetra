//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Windows.Forms;


namespace Ict.Petra.Client.CommonDialogs
{
    public partial class TFrmStatusDialog
    {
        /// <summary>
        /// Sets the text in the heading of the dialog
        /// </summary>
        public String Heading
        {
            set
            {
                this.lblHeading.Text = value;
                this.lblHeading.Refresh();
            }
        }

        /// <summary>
        /// Sets the text in the status of the dialog
        /// </summary>
        public String CurrentStatus
        {
            set
            {
                this.lblStatusInfo.Text = value;
                this.lblStatusInfo.Refresh();
            }
        }

        /// <summary>
        /// Sets the text in the Title bar
        /// </summary>
        public String Title
        {
            set
            {
                this.Text = value;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // This needs to be here for the code to compile
        }

        private void InitializeManualCode()
        {
            // Set the form properties
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.ShowInTaskbar = false;
            this.ControlBox = false;

            // Position the window relative to the caller form
            Form caller = FPetraUtilsObject.GetCallerForm();
            this.Left = caller.Left + 200;
            this.Top = caller.Top + 200;

            // Set a bold font for the heading
            lblHeading.Font = new System.Drawing.Font(lblHeading.Font, System.Drawing.FontStyle.Bold);
        }
    }
}