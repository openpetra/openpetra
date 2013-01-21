// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Client.App.Core
{
    partial class TFrmUnhandledExceptionLogFileDialog
    {
        private String FWhatToOpen;
        private String FLogFileContent;
        private String FLogFileLocation;

        /// <summary>Error Details shown on this screen.</summary>
        public String WhatToOpen
        {
            get
            {
                return FWhatToOpen;
            }

            set
            {
                FWhatToOpen = value;
            }
        }


        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnCopyToClipboard_Click(System.Object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(txtLogFileContent.Text);
        }

        private void Form_Load(System.Object sender, System.EventArgs e)
        {
            if (FWhatToOpen.Equals("Server.log"))
            {
                //the client doen't know the serverlog file - unless in standalone use and development environment
                //-> check if there is a Server.log in the same location where the client log is and then display it

                FLogFileLocation = TClientSettings.GetPathLog() + Path.DirectorySeparatorChar + "Server.log";

                Clipboard.SetDataObject(FLogFileLocation);
                try
                {
                    StreamReader TLogFileReader = new StreamReader(FLogFileLocation);
                    FLogFileContent = TLogFileReader.ReadToEnd();
                    TLogFileReader.Close();
                    txtLogFileContent.Text = Catalog.GetString("The Server log file:") + "\r\n " + FLogFileLocation + "\r\n\r\n" + FLogFileContent;
                }
                catch (Exception)
                {
                    txtLogFileContent.Text = Catalog.GetString(
                        "Problem on opening logfile. The server log can (at the moment) only be displayed in this window if you are using the standalone version or the development environment.");
                }
            }
            else if (FWhatToOpen.Equals("PetraClient.log"))
            {
                FLogFileLocation = TClientSettings.GetPathLog() + Path.DirectorySeparatorChar + "PetraClient.log";

                try
                {
                    StreamReader TLogFileReader = new StreamReader(FLogFileLocation);
                    FLogFileContent = TLogFileReader.ReadToEnd();
                    TLogFileReader.Close();
                    txtLogFileContent.Text = Catalog.GetString("The Client log file:") + "\r\n " + FLogFileLocation + "\r\n\r\n" + FLogFileContent;
                }
                catch (Exception)
                {
                    txtLogFileContent.Text = Catalog.GetString("Problem on opening logfile");
                }
            }
            else
            {
                txtLogFileContent.Text =
                    String.Format(Catalog.GetString("An error ocurred. The logfile you are looking for \r\n({0})\r\nis not available!"), FWhatToOpen);
            }
        }
    }
}