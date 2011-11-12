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
using System.Xml;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindow
    {
        private void InitializeManualCode()
        {
            // leave out 'Revision' and 'Build'
            this.Text = "OpenPetra.org " +
                        System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(3);
        }

        private void ExitManualCode()
        {
            // make sure the application exits; also important for alternate navigation style main window
            this.Hide();

            PetraClientShutdown.Shutdown.SaveUserDefaultsAndDisconnect();

            PetraClientShutdown.Shutdown.StopPetraClient();
        }

        /// the main navigation form
        public static Form MainForm = null;

        private void SwitchToNewNavigation(object sender, EventArgs e)
        {
            MainForm = this;

            if (TFrmMainWindowNew.MainForm == null)
            {
                TFrmMainWindowNew.MainForm = new TFrmMainWindowNew(null);
            }

            this.Hide();
            TFrmMainWindowNew.MainForm.Show();
        }
    }
}