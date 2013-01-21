// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       jomammele
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


namespace Ict.Petra.Client.App.Core
{
    partial class TFrmUnhandledExceptionDetailsDialog
    {
        private String FErrorDetails;

        /// <summary>Error Details shown on this screen.</summary>
        public String ErrorDetails
        {
            get
            {
                return FErrorDetails;
            }

            set
            {
                FErrorDetails = value;
            }
        }


        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnCopyToClipboard_Click(System.Object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(FErrorDetails);
        }

        private void btnShowServerLog_Click(System.Object sender, System.EventArgs e)
        {
            OpenExtendedMessageBox("Server.log");
            Clipboard.SetDataObject("btnShowServerLog_Click was clicked");
        }

        private void btnShowClientLog_Click(System.Object sender, System.EventArgs e)
        {
            OpenExtendedMessageBox("PetraClient.log");
            Clipboard.SetDataObject("btnShowClientLog_Click was clicked");
        }

        private void Form_Load(System.Object sender, System.EventArgs e)
        {
            txtErrorDetails.Text = FErrorDetails;
        }

        private void OpenExtendedMessageBox(string FWhatToOpen)
        {
            TFrmUnhandledExceptionLogFileDialog UHELFDialogue;

            UHELFDialogue = new TFrmUnhandledExceptionLogFileDialog(this);
            UHELFDialogue.WhatToOpen = FWhatToOpen;
            UHELFDialogue.ShowDialog();

            /* get UnhandledExceptionLogFile Dialogue out of memory */
            UHELFDialogue.Dispose();
        }
    }
}