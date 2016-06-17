//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// <summary>
    /// this currently only has static methods
    /// </summary>
    public class TSysManMain
    {
        /// <summary>
        /// this function allows to store the content of the whole database
        /// as a text file, and import it somewhere else, across database types etc
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void ExportAllData(Form AParentForm)
        {
            MessageBox.Show(Catalog.GetString("This may take a while. Please just wait!"));

            string zippedYml = TRemote.MSysMan.ImportExport.WebConnectors.ExportAllTables();
            TImportExportDialogs.ExportWithDialogYMLGz(zippedYml, Catalog.GetString("Save Database into File"));
        }

        private static bool WebConnectorResult = false;

        private static void ResetDatabaseInThread(string AZippedYml)
        {
            WebConnectorResult = TRemote.MSysMan.ImportExport.WebConnectors.ResetDatabase(AZippedYml);
        }

        /// <summary>
        /// this will delete the current database, and reset it with the data selected
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void RestoreDatabase(Form AParentForm)
        {
            string StrImportCancelledMsg = Catalog.GetString("Restoring of database got cancelled; no existing data has been deleted or modified!");
            string StrImportCancelledTitle = Catalog.GetString("Restore Cancelled");

            DialogResult r = MessageBox.Show(
                Catalog.GetString(
                    "WARNING: This will THROW AWAY ALL CURRENT DATA that is held in the database (including the users and passwords!) and replace it with the data that was previously backed up and which you chose to restore!\r\n\r\nDo you REALLY want to restore that data?"),
                Catalog.GetString("WARNING: Replace All Data With Previously Backed Up Data?"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (r == DialogResult.Yes)
            {
                string zippedYml = TImportExportDialogs.ImportWithDialogYMLGz(Catalog.GetString("Select Backup File to Restore From"));

                if (zippedYml != null)
                {
                    Thread ResetDBThread = new Thread(() => ResetDatabaseInThread(zippedYml));

                    using (TProgressDialog dialog = new TProgressDialog(ResetDBThread))
                    {
                        if (dialog.ShowDialog() == DialogResult.Cancel)
                        {
                            MessageBox.Show(StrImportCancelledMsg, StrImportCancelledTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return;
                        }
                    }

                    // Ensure that WebConnectorResult got set in Method 'ResetDatabaseInThread' before we get to the if statement below...
                    ResetDBThread.Join();

                    if (WebConnectorResult)
                    {
                        // TODO: reset all caches? for comboboxes etc
                        MessageBox.Show(Catalog.GetString(
                                "The data was successfully restored.\r\n\r\nPlease restart your OpenPetra Client immediately!"),
                            Catalog.GetString("Restore Successful"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("The restoring of the data FAILED. No existing data has been deleted or modified!\r\n\r\n"
                                +
                                "Please check the Server.log file on the server for errors!"), Catalog.GetString("Restore Failed"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(StrImportCancelledMsg, StrImportCancelledTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// change the password of the current user
        /// </summary>
        public static void SetUserPassword(Form AParentForm)
        {
            string Username = Ict.Petra.Shared.UserInfo.GUserInfo.UserID;

            TLoginForm.CreateNewPassword(AParentForm, Username, string.Empty, false);
        }
    }
}
